Imports System.Text.RegularExpressions
Imports FastColoredTextBoxNS
Imports ControlManager
Imports System.Drawing
Imports System.IO
Imports System.Xml
Imports appmake.WindowWinSet
Imports appmake.GetCommand
Imports appmake.WindowWelcome
Imports appmake.WindowCreate
Imports CtrlCloneTst
Imports System.Runtime.InteropServices
Imports System.Security
Public Class WindowMain
    Dim ProjectName As String = "New Project"
    Dim Recentpath As String = FileIO.SpecialDirectories.Temp & "\Tempapmk.txt"
    Dim SelectedObject As Control = Nothing
    Dim SelectedObjectForUn As Control = Nothing
    Dim IsHasProject As Boolean = False
    Dim IsSaved As Boolean = False
    Private ProjectPath As String = Nothing
    Public Title As String = "My App"
    Public He As Integer = 340
    Public Wi As Integer = 550
    Public Task As Boolean = True
    Public TopMos As Boolean = False
    Public SIcon As Boolean = True
    Public BGColor As Color = System.Drawing.Color.FromArgb(255, 240, 240, 240)
#Region "IDE"
    Private Sub WindowMain_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If IsSaved = False Then
            Select Case MessageBox.Show("Do you wan to save changes to Document ?", "App Make", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
                Case Windows.Forms.DialogResult.Yes
                    Save()
                Case Windows.Forms.DialogResult.Cancel
                    e.Cancel = True

            End Select
        End If
    End Sub
    Private Sub WindowMain_Load(sender As Object, e As EventArgs) Handles Me.Load
        tabMain.SelectedIndex = 0
        Try
            FileIO.FileSystem.DeleteFile(FileIO.SpecialDirectories.Temp & "\rd.xml") 'xml for undo
            FileIO.FileSystem.DeleteFile(FileIO.SpecialDirectories.Temp & "\un.xml") 'xml for redo
        Catch ex As Exception

        End Try
    End Sub
    Private Sub WindowMain_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Try
            Dim cmd As String = GetCommand.GetCommand
            If Not cmd = Nothing Then
                cmd = Replace(cmd, Chr(34), "")

                tabMain.Visible = True
                Open(cmd)
                ProjectPath = cmd
                IsHasProject = True
            Else

                Dim proinof As ProjectDetails = ShowWelcome()
                If proinof.OpenType = OpeningType.NewFile Then
                    ProjectName = proinof.Project.ProjectName
                    Title = proinof.Project.Title
                    He = proinof.Project.Size.Height
                    Wi = proinof.Project.Size.Width
                    Task = proinof.Project.Task
                    TopMos = proinof.Project.Top
                    SIcon = proinof.Project.Icon
                    BGColor = proinof.Project.Color
                    pnlBase.BackColor = BGColor
                    pnlBase.Size = New Size With {.Width = Wi, .Height = He}
                    tabMain.Visible = True
                    IsHasProject = True
                ElseIf proinof.OpenType = OpeningType.RecentOrExistFile Then
                    tabMain.Visible = True
                    Open(proinof.File)
                    ProjectPath = proinof.File
                    IsHasProject = True
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message.ToString, "App Make", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub OpeninIDE()
        Dim s As String = InputBox("Please enter the new project name for Codino IDE project.", "Project Name", ProjectName)
        Dim dl As String = FileIO.SpecialDirectories.MyDocuments & "\Codino\Projects"
        If s <> Nothing Then
            Dim re As Boolean = True
            For Each f As String In FileIO.FileSystem.GetDirectories(dl)
                If s = FileIO.FileSystem.GetDirectoryInfo(f).Name Then
                    re = False
                End If
            Next
            If re = True Then
                Process.Start(Application.StartupPath & "\Codino.exe", "apmkCmd" & s & "??" & fctb.Text)
            Else
                MessageBox.Show("Current Project name is already available. Please enter anthoer name to continue.", "Warning !", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                OpeninIDE()
            End If
        End If
    End Sub
    Private Sub btnOpenIDE_Click(sender As Object, e As EventArgs)
        OpeninIDE()
    End Sub
    Private Sub tsbRun_Click(sender As Object, e As EventArgs) Handles tsbRun.Click
        Try
            WriteXML(FileIO.SpecialDirectories.Temp & "\test_Resources.db")
            Process.Start(Application.StartupPath & "\tools\Runner.exe", FileIO.SpecialDirectories.Temp & "\test_Resources.db")

        Catch ex As Exception
            MessageBox.Show(ex.Message.ToString, "App Make", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
    Private Sub WindowSettingToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles WindowSettingToolStripMenuItem.Click
        Dim r As WinSettings = ShowWinSetting(pnlBase, SIcon, Task, TopMos, Title, ProjectName)
        If r.Resault = True Then
            ProjectName = r.ProjectName
            Title = r.Title
            He = r.Size.Height
            Wi = r.Size.Width
            Task = r.Task
            SIcon = r.Icon
            TopMos = r.Top
            BGColor = r.Color
            pnlBase.BackColor = BGColor
            pnlBase.Size = r.Size
        End If
    End Sub
#Region "EditMenu"
    Private Sub SetUndo()
        WriteXML(FileIO.SpecialDirectories.Temp & "\rd.xml", False)
        tsbUndo.Enabled = True
        tsbRedo.Enabled = False
        UndoToolStripMenuItem.Enabled = True
        RedoToolStripMenuItem.Enabled = False
    End Sub
    Private Sub Undo()
        WriteXML(FileIO.SpecialDirectories.Temp & "\un.xml", False)
        pnlBase.Controls.Clear()
        Open(FileIO.SpecialDirectories.Temp & "\rd.xml", False)
        tsbUndo.Enabled = False
        tsbRedo.Enabled = True
        UndoToolStripMenuItem.Enabled = False
        RedoToolStripMenuItem.Enabled = True
    End Sub
    Private Sub Redo()

        WriteXML(FileIO.SpecialDirectories.Temp & "\tr.xml", False)
        pnlBase.Controls.Clear()
        Open(FileIO.SpecialDirectories.Temp & "\un.xml", False)
        tsbUndo.Enabled = True
        tsbRedo.Enabled = False
        UndoToolStripMenuItem.Enabled = True
        RedoToolStripMenuItem.Enabled = False
    End Sub
    Private Sub CutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CutToolStripMenuItem.Click
        ControlFactory.CopyCtrl2ClipBoard(SelectedObject)
        pnlBase.Controls.Remove(SelectedObject)
        SelectedObject = Nothing
        pnlBase.CreateGraphics.Clear(pnlBase.BackColor)
    End Sub
    Private Sub PasteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PasteToolStripMenuItem.Click
        Dim ctrl As Control = ControlFactory.GetCtrlFromClipBoard
        ctrl.Location = New Point(ctrl.Location.X + 10, ctrl.Location.Y + ctrl.Size.Height - 5)
        Select Case ctrl.GetType.ToString
            Case "System.Windows.Forms.CheckBox"
                ctrl.Name = SetName(Tools.CheckBox)
            Case "System.Windows.Forms.Label"
                ctrl.Name = SetName(Tools.Label)
            Case "System.Windows.Forms.Button"
                ctrl.Name = SetName(Tools.Button)
            Case "appmake.SliderX"
                ctrl.Name = SetName(Tools.Slider)
            Case "appmake.TextBox"
                ctrl.Name = SetName(Tools.TextBox)
            Case "appmake.SerialPort"
                ctrl.Name = SetName(Tools.Serial)
        End Select
        SetEvents(ctrl)
        pnlBase.Controls.Add(ctrl)
    End Sub

    Private Sub CopyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyToolStripMenuItem.Click
        ControlFactory.CopyCtrl2ClipBoard(SelectedObject)
    End Sub
    Private Sub DeleteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteToolStripMenuItem.Click
        pnlBase.Controls.Remove(SelectedObject)
        pnlBase.CreateGraphics.Clear(pnlBase.BackColor)
        SelectedObject = Nothing
    End Sub

    Private Sub tsbUndo_Click(sender As Object, e As EventArgs) Handles tsbUndo.Click
        Undo()
    End Sub

    Private Sub tsbRedo_Click(sender As Object, e As EventArgs) Handles tsbRedo.Click
        Redo()
    End Sub

    Private Sub UndoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UndoToolStripMenuItem.Click
        Undo()
    End Sub

    Private Sub RedoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RedoToolStripMenuItem.Click
        Redo()
    End Sub
#End Region
#Region "File Handle"
    Private Function ReadColor(reader As XmlTextReader, PreFix As String) As Color
        Dim a As Integer = reader.ReadElementString(PreFix & "ColorA")
        Dim r As Integer = reader.ReadElementString(PreFix & "ColorR")
        Dim g As Integer = reader.ReadElementString(PreFix & "ColorG")
        Dim b As Integer = reader.ReadElementString(PreFix & "ColorB")
        Dim c As Color = System.Drawing.Color.FromArgb(a, r, g, b)
        Return c
    End Function
    Private Function ReadSize(reader As XmlTextReader) As Size
        Dim H As Integer = reader.ReadElementString("SizeH")
        Dim W As Integer = reader.ReadElementString("SizeW")
        Dim s As New Size With {.Height = H, .Width = W}
        Return s
    End Function
    Private Function ReadLocation(reader As XmlTextReader) As Point
        Dim X As Integer = reader.ReadElementString("LocationX")
        Dim Y As Integer = reader.ReadElementString("LocationY")
        Dim p As New Point With {.X = X, .Y = Y}
        Return p
    End Function
    Private Sub Save()
        If ProjectPath = Nothing Then
            Dim savedialog As New SaveFileDialog
            savedialog.Filter = "App Make file (*.amf)|*.amf"
            If savedialog.ShowDialog = Windows.Forms.DialogResult.OK Then
                WriteXML(savedialog.FileName)
                ProjectPath = savedialog.FileName
                IsSaved = True
            End If
        Else
            WriteXML(ProjectPath)
            IsSaved = True
        End If
    End Sub
    Private Sub SaveAs()
        Dim savedialog As New SaveFileDialog
        savedialog.Filter = "App Make file (*.amf)|*.amf"
        If savedialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            WriteXML(savedialog.FileName)
        End If
    End Sub
    Private Sub Open(file As String, Optional isRecent As Boolean = True)
        Dim m_xmlr As XmlTextReader
        m_xmlr = New XmlTextReader(file)
        m_xmlr.WhitespaceHandling = WhitespaceHandling.None
        m_xmlr.Read()
        m_xmlr.Read()
        While Not m_xmlr.EOF
            m_xmlr.Read()
            If Not m_xmlr.IsStartElement() Then
                Exit While
            End If
            Dim Ctrl = m_xmlr.GetAttribute("Control")
            m_xmlr.Read()
            Select Case Ctrl
                Case "Window"
                    Dim s As Size = ReadSize(m_xmlr)
                    He = s.Height - 35
                    Wi = s.Width - 15
                    pnlBase.Size = New Size(Wi, He)
                    Title = m_xmlr.ReadElementString("Title")
                    Task = m_xmlr.ReadElementString("Task")
                    SIcon = m_xmlr.ReadElementString("Icon")
                    TopMos = m_xmlr.ReadElementString("TopMost")
                    BGColor = ReadColor(m_xmlr, "BG")
                    pnlBase.BackColor = BGColor
                Case "appmake.TextBox"
                    Dim txt As New TextBox
                    txt.Name = m_xmlr.ReadElementString("Name")
                    txt.Location = ReadLocation(m_xmlr)
                    txt.Size = ReadSize(m_xmlr)
                    txt.Anchor = m_xmlr.ReadElementString("Anchor")
                    txt.AutoSize = m_xmlr.ReadElementString("AutoSize")
                    txt.BackColor = ReadColor(m_xmlr, "Back")
                    txt.ForeColor = ReadColor(m_xmlr, "Fore")
                    txt.Dock = m_xmlr.ReadElementString("Dock")
                    txt.Enabled = m_xmlr.ReadElementString("Enabled")
                    txt.Value = m_xmlr.ReadElementString("TextArea")
                    txt.ButtonText = m_xmlr.ReadElementString("ButtonText")
                    txt.ShowButton = m_xmlr.ReadElementString("ShowButton")
                    SetEvents(txt)
                    pnlBase.Controls.Add(txt)
                Case "System.Windows.Forms.Label"
                    Dim lbl As New Label
                    lbl.Name = m_xmlr.ReadElementString("Name")
                    lbl.Location = ReadLocation(m_xmlr)
                    lbl.Size = ReadSize(m_xmlr)
                    lbl.Anchor = m_xmlr.ReadElementString("Anchor")
                    lbl.AutoSize = m_xmlr.ReadElementString("AutoSize")
                    lbl.BackColor = ReadColor(m_xmlr, "Back")
                    lbl.ForeColor = ReadColor(m_xmlr, "Fore")
                    lbl.Dock = m_xmlr.ReadElementString("Dock")
                    lbl.Enabled = m_xmlr.ReadElementString("Enabled")
                    lbl.Text = m_xmlr.ReadElementString("Text")
                    SetEvents(lbl)
                    pnlBase.Controls.Add(lbl)
                Case "appmake.RichTextX"
                    Dim rtb As New RichTextX
                    rtb.Name = m_xmlr.ReadElementString("Name")
                    rtb.Location = ReadLocation(m_xmlr)
                    rtb.Size = ReadSize(m_xmlr)
                    rtb.Anchor = m_xmlr.ReadElementString("Anchor")
                    rtb.AutoSize = m_xmlr.ReadElementString("AutoSize")
                    rtb.BackColor = ReadColor(m_xmlr, "Back")
                    rtb.ForeColor = ReadColor(m_xmlr, "Fore")
                    rtb.Dock = m_xmlr.ReadElementString("Dock")
                    rtb.Enabled = m_xmlr.ReadElementString("Enabled")
                    rtb.Text = m_xmlr.ReadElementString("Text")
                    rtb.InputMode = m_xmlr.ReadElementString("Input")
                    SetEvents(rtb)
                    pnlBase.Controls.Add(rtb)
                Case "System.Windows.Forms.Button"
                    Dim btn As New Button
                    btn.Name = m_xmlr.ReadElementString("Name")
                    btn.Location = ReadLocation(m_xmlr)
                    btn.Size = ReadSize(m_xmlr)
                    btn.Anchor = m_xmlr.ReadElementString("Anchor")
                    btn.AutoSize = m_xmlr.ReadElementString("AutoSize")
                    btn.BackColor = ReadColor(m_xmlr, "Back")
                    btn.ForeColor = ReadColor(m_xmlr, "Fore")
                    btn.Dock = m_xmlr.ReadElementString("Dock")
                    btn.Enabled = m_xmlr.ReadElementString("Enabled")
                    btn.Text = m_xmlr.ReadElementString("Text")
                    btn.TextAlign = m_xmlr.ReadElementString("TextAlign")
                    btn.FlatStyle = m_xmlr.ReadElementString("FlatStyle")
                    SetEvents(btn)
                    pnlBase.Controls.Add(btn)
                Case "appmake.SliderX"
                    Dim sld As New SliderX
                    sld.Name = m_xmlr.ReadElementString("Name")
                    sld.Location = ReadLocation(m_xmlr)
                    sld.Size = ReadSize(m_xmlr)
                    sld.Anchor = m_xmlr.ReadElementString("Anchor")
                    sld.AutoSize = m_xmlr.ReadElementString("AutoSize")
                    sld.BackColor = ReadColor(m_xmlr, "Back")
                    sld.ForeColor = ReadColor(m_xmlr, "Fore")
                    sld.Dock = m_xmlr.ReadElementString("Dock")
                    sld.Enabled = m_xmlr.ReadElementString("Enabled")
                    sld.DisplayType = m_xmlr.ReadElementString("DisplaType")
                    sld.MapMinValue = m_xmlr.ReadElementString("MapMin")
                    sld.MapMaxValue = m_xmlr.ReadElementString("MapMax")
                    sld.Maximum = m_xmlr.ReadElementString("Max")
                    sld.Minimum = m_xmlr.ReadElementString("Min")
                    sld.TickStyle = m_xmlr.ReadElementString("TickStyle")
                    sld.TickFrequency = m_xmlr.ReadElementString("TickFre")
                    sld.Orientation = m_xmlr.ReadElementString("Orient")
                    sld.SmallChange = m_xmlr.ReadElementString("SamllChange")
                    sld.Value = m_xmlr.ReadElementString("Value")
                    sld.Tag = m_xmlr.ReadElementString("BindLabel")
                    SetEvents(sld)
                    pnlBase.Controls.Add(sld)
                Case "appmake.SerialPort"
                    Dim SP As New SerialPort
                    SP.Name = m_xmlr.ReadElementString("Name")
                    SP.Location = ReadLocation(m_xmlr)
                    SP.Size = ReadSize(m_xmlr)
                    SP.Anchor = m_xmlr.ReadElementString("Anchor")
                    SP.AutoSize = m_xmlr.ReadElementString("AutoSize")
                    SP.BackColor = ReadColor(m_xmlr, "Back")
                    SP.ForeColor = ReadColor(m_xmlr, "Fore")
                    SP.Dock = m_xmlr.ReadElementString("Dock")
                    SP.Enabled = m_xmlr.ReadElementString("Enabled")
                    SetEvents(SP)
                    pnlBase.Controls.Add(SP)
                Case "System.Windows.Forms.CheckBox"
                    Dim chk As New CheckBox
                    chk.Name = m_xmlr.ReadElementString("Name")
                    chk.Location = ReadLocation(m_xmlr)
                    chk.Size = ReadSize(m_xmlr)
                    chk.Anchor = m_xmlr.ReadElementString("Anchor")
                    chk.AutoSize = m_xmlr.ReadElementString("AutoSize")
                    chk.BackColor = ReadColor(m_xmlr, "Back")
                    chk.ForeColor = ReadColor(m_xmlr, "Fore")
                    chk.Dock = m_xmlr.ReadElementString("Dock")
                    chk.Enabled = m_xmlr.ReadElementString("Enabled")
                    chk.Checked = m_xmlr.ReadElementString("Value")
                    chk.Text = m_xmlr.ReadElementString("Text")
                    SetEvents(chk)
                    pnlBase.Controls.Add(chk)
                Case "appmake.ProgressBarX"
                    Dim pgb As New ProgressBarX
                    pgb.Name = m_xmlr.ReadElementString("Name")
                    pgb.Location = ReadLocation(m_xmlr)
                    pgb.Size = ReadSize(m_xmlr)
                    pgb.Anchor = m_xmlr.ReadElementString("Anchor")
                    pgb.AutoSize = m_xmlr.ReadElementString("AutoSize")
                    pgb.BackColor = ReadColor(m_xmlr, "Back")
                    pgb.ForeColor = ReadColor(m_xmlr, "Fore")
                    pgb.Dock = m_xmlr.ReadElementString("Dock")
                    pgb.Enabled = m_xmlr.ReadElementString("Enabled")
                    pgb.DisplayType = m_xmlr.ReadElementString("DisplaType")
                    pgb.MapMinValue = m_xmlr.ReadElementString("MapMin")
                    pgb.MapMaxValue = m_xmlr.ReadElementString("MapMax")
                    pgb.Maximum = m_xmlr.ReadElementString("Max")
                    pgb.Minimum = m_xmlr.ReadElementString("Min")
                    pgb.Value = m_xmlr.ReadElementString("Value")
                    pgb.TargetValueIndex = m_xmlr.ReadElementString("TargetValueIndex")
                    SetEvents(pgb)
                    pnlBase.Controls.Add(pgb)
                Case "appmake.LabelX"
                    Dim lbx As New LabelX
                    lbx.Name = m_xmlr.ReadElementString("Name")
                    lbx.Location = ReadLocation(m_xmlr)
                    lbx.Size = ReadSize(m_xmlr)
                    lbx.Anchor = m_xmlr.ReadElementString("Anchor")
                    lbx.AutoSize = m_xmlr.ReadElementString("AutoSize")
                    lbx.BackColor = ReadColor(m_xmlr, "Back")
                    lbx.ForeColor = ReadColor(m_xmlr, "Fore")
                    lbx.Dock = m_xmlr.ReadElementString("Dock")
                    lbx.Enabled = m_xmlr.ReadElementString("Enabled")
                    lbx.DisplayType = m_xmlr.ReadElementString("DisplaType")
                    lbx.MapMinValue = m_xmlr.ReadElementString("MapMin")
                    lbx.MapMaxValue = m_xmlr.ReadElementString("MapMax")
                    lbx.MaximumValue = m_xmlr.ReadElementString("Max")
                    lbx.MinimumValue = m_xmlr.ReadElementString("Min")
                    lbx.TargetValueIndex = m_xmlr.ReadElementString("TargetValueIndex")
                    lbx.Text = m_xmlr.ReadElementString("Text")
                    SetEvents(lbx)
                    pnlBase.Controls.Add(lbx)
            End Select
        End While
        m_xmlr.Close()
        For Each c As Control In pnlBase.Controls
            If c.GetType.ToString = "appmake.SliderX" Then
                Dim s As SliderX = c
                If Not s.Tag = "No" Then
                    For Each l As Control In pnlBase.Controls
                        If l.GetType.ToString = "System.Windows.Forms.Label" Then
                            Dim b As Label = l
                            If b.Name = s.Tag Then
                                s.DisLabel = b
                            End If
                        End If
                    Next
                End If
            End If
        Next

        If isRecent = True Then
            SetRecents(file)
        End If
    End Sub
    Private Sub tsbSave_Click(sender As Object, e As EventArgs) Handles tsbSave.Click
        Save()
    End Sub
    Private Sub tsbSaveas_Click(sender As Object, e As EventArgs) Handles tsbSaveas.Click
        SaveAs()
    End Sub
    Private Sub SaveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveToolStripMenuItem.Click
        Save()
    End Sub
    Private Sub SaveAsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveAsToolStripMenuItem.Click
        SaveAs()
    End Sub
    Private Sub NewAppToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NewAppToolStripMenuItem.Click
        If IsHasProject = False Then
            Dim r As WinSettings = ShowWinSetting(pnlBase, SIcon, Task, TopMos, Title, ProjectName)
            If r.Resault = True Then
                ProjectName = r.ProjectName
                Title = r.Title
                He = r.Size.Height
                Wi = r.Size.Width
                Task = r.Task
                SIcon = r.Icon
                TopMos = r.Top
                BGColor = r.Color
                pnlBase.BackColor = BGColor
                pnlBase.Size = r.Size
                tabMain.Visible = True
                IsHasProject = True
            End If
        Else
            Process.Start(Application.ExecutablePath)
        End If
    End Sub
    Private Sub OpenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenToolStripMenuItem.Click
        If IsHasProject = False Then
            Dim opendialog As New OpenFileDialog
            opendialog.Filter = "App Make file (*.amf)|*.amf"
            If opendialog.ShowDialog = Windows.Forms.DialogResult.OK Then
                Open(opendialog.FileName)
                IsHasProject = True
            End If
        Else
            Process.Start(Application.ExecutablePath)
        End If
    End Sub
    Private Sub ExportCodeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExportCodeToolStripMenuItem.Click
        Dim savedialog As New SaveFileDialog
        savedialog.Filter = "Palin Text (*.txt)|*.txt|Rich Text Format (*.RTF)|*.rtf|HTML file (*.html)|*.html"
        If savedialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            If savedialog.FilterIndex = 1 Then
                fctb.SaveToFile(savedialog.FileName, System.Text.Encoding.ASCII)
            ElseIf savedialog.FilterIndex = 2 Then
                Dim str As String = fctb.Rtf
                FileIO.FileSystem.WriteAllText(savedialog.FileName, str, True)
            Else
                Dim str As String = fctb.Html
                FileIO.FileSystem.WriteAllText(savedialog.FileName, str, True)
            End If
        End If
    End Sub
#End Region
#Region "Recent"
    Public Function ReadALine(ByVal File_Path As String, ByVal TotalLine As Integer, ByVal Line2Read As Integer) As String
        Dim Buffer As Array
        Dim Line As String
        If TotalLine <= Line2Read Then
            Return "No Recent"
        End If
        Buffer = File.ReadAllLines(File_Path)
        Line = Buffer(Line2Read)
        Return Line
    End Function
    Public Function GetNumberOfLines(ByVal file_path As String) As Integer
        Dim sr As New StreamReader(file_path)
        Dim NumberOfLines As Integer
        Do While sr.Peek >= 0
            sr.ReadLine()
            NumberOfLines += 1
        Loop
        sr.Dispose()
        sr.Close()
        Return NumberOfLines
    End Function
    Private Sub SetRecents(File As String)
        If FileIO.FileSystem.FileExists(Recentpath) = True Then
            FileIO.FileSystem.CopyFile(Recentpath, Recentpath & ".tmp", True)
            FileIO.FileSystem.WriteAllText(Recentpath, File, False)
            If GetNumberOfLines(Recentpath & ".tmp") < 10 Then
                For i As Integer = 0 To GetNumberOfLines(Recentpath & ".tmp") - 1
                    If File <> ReadALine(Recentpath & ".tmp", GetNumberOfLines(Recentpath & ".tmp"), i) Then
                        FileIO.FileSystem.WriteAllText(Recentpath, vbNewLine & ReadALine(Recentpath & ".tmp", GetNumberOfLines(Recentpath & ".tmp"), i), True)
                    End If
                Next
            Else
                For i As Integer = 0 To 9
                    If File <> ReadALine(Recentpath & ".tmp", GetNumberOfLines(Recentpath & ".tmp"), i) Then
                        FileIO.FileSystem.WriteAllText(Recentpath, vbNewLine & ReadALine(Recentpath & ".tmp", GetNumberOfLines(Recentpath & ".tmp"), i), True)
                    End If
                Next
            End If
            FileIO.FileSystem.DeleteFile(Recentpath & ".tmp", FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently)
        Else
            FileIO.FileSystem.WriteAllText(Recentpath, File, True)
        End If
    End Sub
#End Region
#Region "XMLWrite"
    Private Sub WriteXML(File As String, Optional isResizing As Boolean = True)
        Dim m_xmlr As New XmlTextWriter(File, System.Text.Encoding.UTF8)
        m_xmlr.WriteStartDocument()
        m_xmlr.WriteStartElement("XMLsaving")
        m_xmlr.WriteStartElement("Properties")
        m_xmlr.WriteAttributeString("Control", "Window")
        If isResizing = True Then
            m_xmlr.WriteElementString("SizeH", He + 35)
            m_xmlr.WriteElementString("SizeW", Wi + 15)
        Else
            m_xmlr.WriteElementString("SizeH", He)
            m_xmlr.WriteElementString("SizeW", Wi)
        End If   
        m_xmlr.WriteElementString("Title", Title)
        m_xmlr.WriteElementString("Task", Task)
        m_xmlr.WriteElementString("Icon", SIcon)
        m_xmlr.WriteElementString("TopMost", TopMos)
        WriteColor(m_xmlr, BGColor, "BGColor")
        m_xmlr.WriteEndElement()
        For Each c As Control In pnlBase.Controls
            WriteControlProperties(m_xmlr, c)
        Next
        m_xmlr.WriteEndElement()
        m_xmlr.WriteEndDocument()
        m_xmlr.Close()
    End Sub
    Private Sub WriteControlProperties(m_xmlr As XmlTextWriter, Ctrl As Control)
        m_xmlr.WriteStartElement("Properties")
        m_xmlr.WriteAttributeString("Control", Ctrl.GetType.ToString)
        m_xmlr.WriteElementString("Name", Ctrl.Name)
        m_xmlr.WriteElementString("LocationX", Ctrl.Location.X)
        m_xmlr.WriteElementString("LocationY", Ctrl.Location.Y)
        m_xmlr.WriteElementString("SizeH", Ctrl.Size.Height)
        m_xmlr.WriteElementString("SizeW", Ctrl.Size.Width)
        m_xmlr.WriteElementString("Anchor", Ctrl.Anchor)
        m_xmlr.WriteElementString("AutoSize", Ctrl.AutoSize)
        WriteColor(m_xmlr, Ctrl.BackColor, "BackColor")
        WriteColor(m_xmlr, Ctrl.ForeColor, "ForeColor")
        m_xmlr.WriteElementString("Dock", Ctrl.Dock)
        m_xmlr.WriteElementString("Enabled", Ctrl.Enabled)
        Select Case Ctrl.GetType.ToString
            Case "System.Windows.Forms.Button"
                Dim t As Button = Ctrl
                m_xmlr.WriteElementString("Text", t.Text)
                m_xmlr.WriteElementString("TextAlign", t.TextAlign)
                m_xmlr.WriteElementString("FlatStyle", t.FlatStyle)
            Case "appmake.TextBox"
                Dim t As TextBox = Ctrl
                m_xmlr.WriteElementString("TextArea", t.Value)
                m_xmlr.WriteElementString("ButtonText", t.ButtonText)
                m_xmlr.WriteElementString("ShowButton", t.ShowButton)
            Case "appmake.SliderX"
                Dim t As SliderX = Ctrl
                m_xmlr.WriteElementString("DisplaType", t.DisplayType)
                m_xmlr.WriteElementString("MapMin", t.MapMinValue)
                m_xmlr.WriteElementString("MapMax", t.MapMaxValue)
                m_xmlr.WriteElementString("Max", t.Maximum)
                m_xmlr.WriteElementString("Min", t.Minimum)
                m_xmlr.WriteElementString("TickStyle", t.TickStyle)
                m_xmlr.WriteElementString("TickFre", t.TickFrequency)
                m_xmlr.WriteElementString("Orient", t.Orientation)
                m_xmlr.WriteElementString("SamllChange", t.SmallChange)
                m_xmlr.WriteElementString("Value", t.Value)
                If Not t.DisLabel Is Nothing Then
                    m_xmlr.WriteElementString("BindLabel", t.DisLabel.Name)
                Else
                    m_xmlr.WriteElementString("BindLabel", "No")
                End If
            Case "appmake.ProgressBarX"
                Dim t As ProgressBarX = Ctrl
                m_xmlr.WriteElementString("DisplaType", t.DisplayType)
                m_xmlr.WriteElementString("MapMin", t.MapMinValue)
                m_xmlr.WriteElementString("MapMax", t.MapMaxValue)
                m_xmlr.WriteElementString("Max", t.Maximum)
                m_xmlr.WriteElementString("Min", t.Minimum)
                m_xmlr.WriteElementString("Value", t.Value)
                m_xmlr.WriteElementString("TargetValueIndex", t.TargetValueIndex)
            Case "System.Windows.Forms.Label"
                Dim t As Label = Ctrl   
                m_xmlr.WriteElementString("Text", t.Text)
            Case "appmake.RichTextX"
                Dim t As RichTextX = Ctrl
                m_xmlr.WriteElementString("Text", t.Text)
                m_xmlr.WriteElementString("Input", t.InputMode)
            Case "System.Windows.Forms.CheckBox"
                Dim t As CheckBox = Ctrl
                m_xmlr.WriteElementString("Value", t.Checked)
                m_xmlr.WriteElementString("Text", t.Text)
            Case "appmake.LabelX"
                Dim t As LabelX = Ctrl
                m_xmlr.WriteElementString("DisplaType", t.DisplayType)
                m_xmlr.WriteElementString("MapMin", t.MapMinValue)
                m_xmlr.WriteElementString("MapMax", t.MapMaxValue)
                m_xmlr.WriteElementString("Max", t.MaximumValue)
                m_xmlr.WriteElementString("Min", t.MinimumValue)
                m_xmlr.WriteElementString("TargetValueIndex", t.TargetValueIndex)
                m_xmlr.WriteElementString("Text", t.Text)
        End Select
        m_xmlr.WriteEndElement()
    End Sub
    Private Sub WriteColor(m_xmlr As XmlTextWriter, color As System.Drawing.Color, PreFix As String)
        m_xmlr.WriteElementString(PreFix & "A", color.A)
        m_xmlr.WriteElementString(PreFix & "R", color.R)
        m_xmlr.WriteElementString(PreFix & "G", color.G)
        m_xmlr.WriteElementString(PreFix & "B", color.B)
    End Sub
#End Region
#Region "ArduinoCode"
#Region "High Lighter"
    Dim keywordStyle As New TextStyle(New SolidBrush(Color.Blue), Nothing, System.Drawing.FontStyle.Regular)
    Dim NumberStyle As New TextStyle(New SolidBrush(BackColor), Nothing, System.Drawing.FontStyle.Regular)
    Dim classStyle As New TextStyle(New SolidBrush(Color.FromArgb(255, 0, 128, 169)), Nothing, System.Drawing.FontStyle.Regular)
    Dim attributeStyle As New TextStyle(New SolidBrush(Color.Gray), Nothing, System.Drawing.FontStyle.Regular)
    Dim commentStyle As New TextStyle(New SolidBrush(Color.Green), Nothing, System.Drawing.FontStyle.Regular)
    Dim stringStyle As New TextStyle(New SolidBrush(Color.Brown), Nothing, System.Drawing.FontStyle.Regular)
    Dim IOStyle As New TextStyle(New SolidBrush(Color.Orange), Nothing, System.Drawing.FontStyle.Regular)
    Dim SameWordsStyle As New MarkerStyle(New SolidBrush(Color.FromArgb(40, Color.Gray)))
    Private Sub ArduinoSyntaxHighlighter(e As TextChangedEventArgs, fctb As FastColoredTextBox)
        fctb.LeftBracket = "("c
        fctb.RightBracket = ")"c
        fctb.LeftBracket2 = ControlChars.NullChar
        fctb.RightBracket2 = ControlChars.NullChar
        'clear style of changed range
        e.ChangedRange.ClearStyle(keywordStyle, NumberStyle, classStyle, attributeStyle, commentStyle, stringStyle, IOStyle)
        'string highlighting
        e.ChangedRange.SetStyle(stringStyle, """""|@""""|''|@"".*?""|(?<!@)(?<range>"".*?[^\\]"")|'.*?[^\\]'")
        'comment highlighting
        e.ChangedRange.SetStyle(commentStyle, "//.*$", RegexOptions.Multiline)
        e.ChangedRange.SetStyle(commentStyle, "(/\*.*?\*/)|(/\*.*)", RegexOptions.Singleline)
        e.ChangedRange.SetStyle(commentStyle, "(/\*.*?\*/)|(.*\*/)", RegexOptions.Singleline Or RegexOptions.RightToLeft)
        'attribute highlighting

        e.ChangedRange.SetStyle(attributeStyle, "^\s*(?<range>\[.+?\])\s*$", RegexOptions.Multiline)
        'class name highlighting
        e.ChangedRange.SetStyle(classStyle, "\b(class|struct|enum|print|println|interface|isAlphaNumeric|isAlpha|isAscii|isWhitespace|isControl|isControl|isDigit|isGraph|isLowerCase|isPrintable|isPunct|isSpace|isUpperCase|isHexadecimalDigit|pinMode|digitalWrite|digitalRead|analogReference|analogRead|analogWrite|analogReadResolution|analogReadResolution|read|parseInt|readString|parse|available|write|begin|setup|loop)\b")
        'number highlighting
        e.ChangedRange.SetStyle(NumberStyle, "\b\d+[\.]?\d*([eE]\-?\d+)?[lLdDfF]?\b|\b0x[a-fA-F\d]+\b")
        'keyword highlighting
        e.ChangedRange.SetStyle(keywordStyle, "\b(void|if|else|for|switch case|while|do|break|continue|return|goto|boolean|char|unsigned char|byte|int|unsigned int|word|long|unsigned long|short|float|double|string|String|array|variable scope|static|volatile|const|sizeof|PROGMEM|true|false|integer constants|floating point constants|tone|noTone|shiftOut|shiftIn|pulseIn|millis|micros|delay|delayMicroseconds|min|max|abs|constrain|map|pow|sqrt|sin|cos|tan|attachInterrupt|detachInterrupt|interrupts|noInterrupts|Keyboard|Keyboard|randomSeed|random|lowByte|highByte|bitRead|bitWrite|bitSet|bitClear|bit|Serial|Stream|Serial1|Serial2|Serial3|Serial4)\b")
        'clear folding markers
        e.ChangedRange.ClearFoldingMarkers()
        'input/output style
        e.ChangedRange.SetStyle(IOStyle, "\b(HIGH|LOW|OUTPUT|INPUT|INPUT_PULLUP|LED_BUILTIN)\b|#define\b|#include\b|#ifdef\b|#else\b|#if\b|#endif\b|#undef\b")
        'set folding markers
        e.ChangedRange.SetFoldingMarkers("{", "}")
        'allow to collapse #region blocks
        e.ChangedRange.SetFoldingMarkers("/\*", "\*/")
        'allow to collapse comment block
    End Sub
    Private Sub fctb_TextChanged(sender As Object, e As TextChangedEventArgs) Handles fctb.TextChanged
        ArduinoSyntaxHighlighter(e, sender)
    End Sub
#End Region
#Region "CreateCode"
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        CreateCode()
    End Sub
    Private Sub CreateCode()
        Dim lastStr As String = Nothing
        Dim StrCode As String = "void setup(){" & vbNewLine & "Serial.begin(9600);" & vbNewLine & "}" & vbNewLine & vbNewLine & "void loop(){" & vbNewLine & "if(Serial.available()>0){"
        Dim res As String = Nothing
        Dim ver As String = Nothing 'reserv for variable
        For Each s As Control In pnlBase.Controls
            Select Case s.GetType.ToString
                Case "appmake.TextBox"
                    Dim t As TextBox = s
                    ver += "int Tx" & Replace(t.Name, "TextBox", "") & ";" & vbNewLine
                    StrCode += vbNewLine & "Tx" & Replace(t.Name, "TextBox", "") & " = Serial.parseInt();" & "    // Variable for " & t.Name & " Button relase value is -1"
                    res += t.Name & ", "
                    lastStr = t.Name
            End Select
        Next
        For Each s As Control In pnlBase.Controls
            Select Case s.GetType.ToString
                Case "appmake.SliderX"
                    Dim t As SliderX = s
                    ver += "int Sl" & Replace(t.Name, "Slider", "") & ";" & vbNewLine
                    StrCode += vbNewLine & "Sl" & Replace(t.Name, "Slider", "") & " = Serial.parseInt();" & "    // Variable for " & t.Name & " Min value is " & t.Minimum & " and Maximum value is " & t.Maximum
                    res += t.Name & ", "
                    lastStr = t.Name
            End Select
        Next
        For Each s As Control In pnlBase.Controls
            Select Case s.GetType.ToString
                Case "System.Windows.Forms.Button"
                    Dim t As Button = s
                    ver += "int Bt" & Replace(t.Name, "Button", "") & ";" & vbNewLine
                    StrCode += vbNewLine & "Bt" & Replace(t.Name, "Button", "") & " = Serial.parseInt();" & "    // Variable for " & t.Name & " Button click value is 1 release value is 0"
                    res += t.Name & ", "
                    lastStr = t.Name
            End Select
        Next
        For Each s As Control In pnlBase.Controls
            Select Case s.GetType.ToString
                Case "System.Windows.Forms.CheckBox"
                    Dim t As CheckBox = s
                    ver += "int Cb" & Replace(t.Name, "CheckBox", "") & ";" & vbNewLine
                    StrCode += vbNewLine & "Cb" & Replace(t.Name, "CheckBox", "") & " = Serial.parseInt();" & "    // Variable for " & t.Name & " Check value is 1 and Uncheck value is 0"
                    res += t.Name & ", "
                    lastStr = t.Name
            End Select
        Next
        StrCode += vbNewLine & "}" & vbNewLine & "}" & vbNewLine & "// The values sending as following structure : " & res
        StrCode = Replace(StrCode, lastStr & ", ", lastStr)
        fctb.Text = ver & StrCode
        fctb.DoAutoIndent()
    End Sub
#End Region
#End Region
#End Region
#Region "AppEditor"
#Region "Graphics"
    Dim DRAG_HANDLE_SIZE As Double = 8

    Private Sub pnlBase_Click(sender As Object, e As EventArgs)
        pnlBase.CreateGraphics.Clear(pnlBase.BackColor)
        lblObject.Text = "Nothing"
    End Sub
    Private Sub DrawControlBorder(sender As Object)
        Dim control As Control = DirectCast(sender, Control)
        'define the border to be drawn, it will be offset by DRAG_HANDLE_SIZE / 2
        'around the control, so when the drag handles are drawn they will seem to be
        'connected in the middle.
        Dim Border As New Rectangle(New Point(control.Location.X - DRAG_HANDLE_SIZE / 2, control.Location.Y - DRAG_HANDLE_SIZE / 2), New Size(control.Size.Width + DRAG_HANDLE_SIZE, control.Size.Height + DRAG_HANDLE_SIZE))
        'define the 8 drag handles, that has the size of DRAG_HANDLE_SIZE
        Dim NW As New Rectangle(New Point(control.Location.X - DRAG_HANDLE_SIZE, control.Location.Y - DRAG_HANDLE_SIZE), New Size(DRAG_HANDLE_SIZE, DRAG_HANDLE_SIZE))
        Dim N As New Rectangle(New Point(control.Location.X + control.Width / 2 - DRAG_HANDLE_SIZE / 2, control.Location.Y - DRAG_HANDLE_SIZE), New Size(DRAG_HANDLE_SIZE, DRAG_HANDLE_SIZE))
        Dim NE As New Rectangle(New Point(control.Location.X + control.Width, control.Location.Y - DRAG_HANDLE_SIZE), New Size(DRAG_HANDLE_SIZE, DRAG_HANDLE_SIZE))
        Dim W As New Rectangle(New Point(control.Location.X - DRAG_HANDLE_SIZE, control.Location.Y + control.Height / 2 - DRAG_HANDLE_SIZE / 2), New Size(DRAG_HANDLE_SIZE, DRAG_HANDLE_SIZE))
        Dim E As New Rectangle(New Point(control.Location.X + control.Width, control.Location.Y + control.Height / 2 - DRAG_HANDLE_SIZE / 2), New Size(DRAG_HANDLE_SIZE, DRAG_HANDLE_SIZE))
        Dim SW As New Rectangle(New Point(control.Location.X - DRAG_HANDLE_SIZE, control.Location.Y + control.Height), New Size(DRAG_HANDLE_SIZE, DRAG_HANDLE_SIZE))
        Dim S As New Rectangle(New Point(control.Location.X + control.Width / 2 - DRAG_HANDLE_SIZE / 2, control.Location.Y + control.Height), New Size(DRAG_HANDLE_SIZE, DRAG_HANDLE_SIZE))
        Dim SE As New Rectangle(New Point(control.Location.X + control.Width, control.Location.Y + control.Height), New Size(DRAG_HANDLE_SIZE, DRAG_HANDLE_SIZE))
        pnlBase.CreateGraphics.Clear(pnlBase.BackColor)
        'get the form graphic
        Dim g As Graphics = pnlBase.CreateGraphics
        'draw the border and drag handles
        ControlPaint.DrawBorder(g, Border, Color.Gray, ButtonBorderStyle.Dotted)
        ControlPaint.DrawGrabHandle(g, NW, True, True)
        ControlPaint.DrawGrabHandle(g, N, True, True)
        ControlPaint.DrawGrabHandle(g, NE, True, True)
        ControlPaint.DrawGrabHandle(g, W, True, True)
        ControlPaint.DrawGrabHandle(g, E, True, True)
        ControlPaint.DrawGrabHandle(g, SW, True, True)
        ControlPaint.DrawGrabHandle(g, S, True, True)
        ControlPaint.DrawGrabHandle(g, SE, True, True)
        g.Dispose()
    End Sub
#End Region
#Region "ControlEvents"
    Private Sub PropertySelector(sender As Control, e As EventArgs)
        DrawControlBorder(sender)
        SelectedObject = sender
        PropertyGrid1.SelectedObject = sender
        lblObject.Text = sender.Name
        IsSaved = False
        VisibleBinder(sender)
        SetUndo()
    End Sub
    Private Sub OKeyDown(sender As Object, e As PreviewKeyDownEventArgs)
        If e.KeyData = Keys.Delete Then
            pnlBase.Controls.Remove(sender)
        End If
    End Sub
    Private Sub OLocationChanged(sender As Object, e As EventArgs)

        DrawControlBorder(sender)
    End Sub
    Private Sub OSizeChanged(sender As Object, e As EventArgs)
        DrawControlBorder(sender)
    End Sub
    Private Sub ctrl_AutoSizeChanged(sender As Object, e As EventArgs)

    End Sub
    Private Sub ctrl_BackColorChanged(sender As Object, e As EventArgs)

    End Sub
    Private Sub ctrl_DockChanged(sender As Object, e As EventArgs)

    End Sub
    Private Sub ctrl_ForeColorChanged(sender As Object, e As EventArgs)

    End Sub
    Private Sub ctrl_TextChanged(sender As Object, e As EventArgs)

    End Sub
    Private Sub SetEvents(Objects As Control)

        AddHandler Objects.Click, AddressOf PropertySelector
        AddHandler Objects.SizeChanged, AddressOf OSizeChanged
        AddHandler Objects.LocationChanged, AddressOf OLocationChanged
        AddHandler Objects.PreviewKeyDown, AddressOf OKeyDown
        AddHandler Objects.BackColorChanged, AddressOf ctrl_BackColorChanged
        AddHandler Objects.ForeColorChanged, AddressOf ctrl_ForeColorChanged
        AddHandler Objects.DockChanged, AddressOf ctrl_DockChanged
        ControlMoverOrResizer.Init(Objects)
        ControlMoverOrResizer.WorkType = ControlMoverOrResizer.MoveOrResize.MoveAndResize
        Objects.ContextMenuStrip = ContextMenuStrip2

    End Sub
#End Region
#Region "SetName"
    Enum Tools
        TextBox
        Label
        Button
        CheckBox
        Serial
        Slider
        ProgressBar
        SerialLabel
        RichTextX
    End Enum
    Private Function SetName(control As Tools) As String
        Dim name As String = Nothing
        Select Case control
            Case Tools.Label
                name = CheckNames("Label")
            Case Tools.Serial
                name = CheckNames("SerialPort")
            Case Tools.TextBox
                name = CheckNames("TextBox")
            Case Tools.Slider
                name = CheckNames("Slider")
            Case Tools.Button
                name = CheckNames("Button")
            Case Tools.CheckBox
                name = CheckNames("CheckBox")
            Case Tools.ProgressBar
                name = CheckNames("ProgressBar")
            Case Tools.SerialLabel
                name = CheckNames("SerialLabel")
            Case Tools.RichTextX
                name = CheckNames("RichTextBoxX")
        End Select
        Return name
    End Function
    Private Function CheckNames(Control As String) As String
        Dim i As Integer = 1
        Do Until IsExisting(Control & i) = False
            i += 1
        Loop
        Return Control & i
    End Function
    Private Function IsExisting(name As String) As Boolean
        Dim Exis As Boolean = False
        For Each n As Control In pnlBase.Controls
            If n.Name = name Then
                Exis = True
            End If
        Next
        Return Exis
    End Function
#End Region
#Region "BindLabel"
    Dim SelectedSlider As SliderX
    Private Sub VisibleBinder(sender As Control)
        If sender.GetType().ToString = "appmake.SliderX" Then
            tsblblBind.Visible = True
            tsdBindControl.Visible = True
            SelectedSlider = sender
            Try
                Dim s As SliderX = sender
                Dim l As Label = s.DisLabel
                tsdBindControl.Text = l.Name
            Catch ex As Exception
                tsdBindControl.Text = "none"
            End Try  
        End If
    End Sub
    Private Sub tsdBindControl_DropDownItemClicked(sender As Object, e As EventArgs)
        Dim c As ToolStripMenuItem = sender
        Dim l As Label = c.Tag
            SelectedSlider.DisLabel = l
            tsdBindControl.Text = c.Name
    End Sub
    Private Sub tsdBindControl_DropDownOpening(sender As Object, e As EventArgs) Handles tsdBindControl.DropDownOpening
        tsdBindControl.DropDownItems.Clear()
        For Each c As Control In pnlBase.Controls
            If c.GetType.ToString = "System.Windows.Forms.Label" Then
                Dim i As New ToolStripMenuItem With {.Text = c.Name, .Tag = c}
                AddHandler i.Click, AddressOf tsdBindControl_DropDownItemClicked
                tsdBindControl.DropDownItems.Add(i)
            End If
        Next
    End Sub
#End Region
#Region "ToolBox"
    Private Sub tsbSerialPort_Click(sender As Object, e As EventArgs) Handles tsbSerialPort.Click
        Dim i As Integer = 0
        For Each s As Control In pnlBase.Controls
            If s.GetType.ToString = "appmake.SerialPort" Then
                i += 1
            End If
        Next
        If i = 0 Then
            Dim AppSerialPort As New appmake.SerialPort
            SetEvents(AppSerialPort)
            SetUndo()
            AppSerialPort.Name = SetName(Tools.Serial)
            pnlBase.Controls.Add(AppSerialPort)
        Else
            MessageBox.Show("Can not add more than one Serial Port", "App Make", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub
    Private Sub tsbTextBox_Click(sender As Object, e As EventArgs) Handles tsbTextBox.Click
        Dim txt As New TextBox With {.Value = 0, .Name = SetName(Tools.TextBox)}
        SetEvents(txt)
        SetUndo()
        pnlBase.Controls.Add(txt)
        CreateCode()
    End Sub
    Private Sub tsbSlider_Click(sender As Object, e As EventArgs) Handles tsbSlider.Click
        Dim Objects As New SliderX With {.Name = SetName(Tools.Slider)}
        SetEvents(Objects)
        SetUndo()
        pnlBase.Controls.Add(Objects)
        CreateCode()
    End Sub
    Private Sub tsbLabel_Click(sender As Object, e As EventArgs) Handles tsbLabel.Click
        Dim lbl As New Label With {.Text = SetName(Tools.Label), .Name = SetName(Tools.Label), .AutoSize = True}
        SetEvents(lbl)
        SetUndo()
        pnlBase.Controls.Add(lbl)
    End Sub
    Private Sub tsbButton_Click(sender As Object, e As EventArgs) Handles tsbButton.Click
        Dim btn As New Button With {.Text = SetName(Tools.Button), .Name = SetName(Tools.Button)}
        SetEvents(btn)
        SetUndo()
        pnlBase.Controls.Add(btn)
        CreateCode()
    End Sub
    Private Sub tsbChecBox_Click(sender As Object, e As EventArgs) Handles tsbChecBox.Click
        Dim chk As New CheckBox With {.Text = SetName(Tools.CheckBox), .Name = SetName(Tools.CheckBox)}
        SetEvents(chk)
        SetUndo()
        pnlBase.Controls.Add(chk)
        CreateCode()
    End Sub
    Private Sub tsbProgrssBar_Click(sender As Object, e As EventArgs) Handles tsbProgrssBar.Click
        Dim pgb As New ProgressBarX With {.Name = SetName(Tools.ProgressBar)}
        SetEvents(pgb)
        SetUndo()
        pnlBase.Controls.Add(pgb)
        CreateCode()
    End Sub
    Private Sub tsbSerialLabel_Click(sender As Object, e As EventArgs) Handles tsbSerialLabel.Click
        Dim lbx As New LabelX With {.Name = SetName(Tools.SerialLabel), .Text = SetName(Tools.SerialLabel)}
        SetEvents(lbx)
        SetUndo()
        pnlBase.Controls.Add(lbx)
        CreateCode()
    End Sub
    Private Sub tsbRichtext_Click(sender As Object, e As EventArgs) Handles tsbRichtext.Click
        Dim rtb As New RichTextX With {.Name = SetName(Tools.RichTextX), .Text = SetName(Tools.RichTextX)}
        SetEvents(rtb)
        SetUndo()
        pnlBase.Controls.Add(rtb)
        CreateCode()
    End Sub
#End Region
#End Region
    Private Sub MakeEXEToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MakeEXEToolStripMenuItem.Click
        Dim r As EXEInfo = ShowEXE()
        If r.Resault = True Then
            FileIO.FileSystem.CreateDirectory(r.Location & "\" & r.Filename)
            FileIO.FileSystem.CopyFile(Application.StartupPath & "\tools\Runner.exe", r.Location & "\" & r.Filename & "\" & r.Filename & ".exe")

            InjectIcon(r.Location & "\" & r.Filename & "\" & r.Filename & ".exe", r.IcoLocation)

            WriteXML(r.Location & "\" & r.Filename & "\Resources.db")
        End If
    End Sub

    Private Sub PrintCodeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PrintCodeToolStripMenuItem.Click
        Dim pp As New PrintDialogSettings
        pp.ShowPrintPreviewDialog = True
        pp.IncludeLineNumbers = True
        fctb.Print(pp)
    End Sub

    Private Sub HelpToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles HelpToolStripMenuItem1.Click
        Process.Start("http://codino.tk")
    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        Dim w As New WindowAbout
        w.ShowDialog()
    End Sub

    Private Sub RunToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RunToolStripMenuItem.Click
        Try
            WriteXML(FileIO.SpecialDirectories.Temp & "\test_Resources.db")
            Process.Start(Application.StartupPath & "\tools\Runner.exe", FileIO.SpecialDirectories.Temp & "\test_Resources.db")
        Catch ex As Exception
            MessageBox.Show(ex.Message.ToString, "App Make", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub btnCopy_Click(sender As Object, e As EventArgs) Handles btnCopy.Click
        Clipboard.SetText(fctb.Text, TextDataFormat.Text)
    End Sub

    Private Sub btnOpen_Click(sender As Object, e As EventArgs) Handles btnOpen.Click
        OpeninIDE()
    End Sub
    <SuppressUnmanagedCodeSecurity()> _
    Private Class NativeMethods
        <DllImport("kernel32")> _
        Public Shared Function BeginUpdateResource( _
    ByVal fileName As String, _
    <MarshalAs(UnmanagedType.Bool)> ByVal deleteExistingResources As Boolean) As IntPtr
        End Function

        <DllImport("kernel32")> _
        Public Shared Function UpdateResource( _
    ByVal hUpdate As IntPtr, _
    ByVal type As IntPtr, _
    ByVal name As IntPtr, _
    ByVal language As Short, _
    <MarshalAs(UnmanagedType.LPArray, SizeParamIndex:=5)> _
    ByVal data() As Byte, _
    ByVal dataSize As Integer) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

        <DllImport("kernel32")> _
        Public Shared Function EndUpdateResource( _
    ByVal hUpdate As IntPtr, _
    <MarshalAs(UnmanagedType.Bool)> ByVal discard As Boolean) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function
    End Class

    ' The first structure in an ICO file lets us know how many images are in the file.
    <StructLayout(LayoutKind.Sequential)> _
    Private Structure ICONDIR
        Public Reserved As UShort  ' Reserved, must be 0
        Public Type As UShort  ' Resource type, 1 for icons.
        Public Count As UShort  ' How many images.
        ' The native structure has an array of ICONDIRENTRYs as a final field.
    End Structure

    ' Each ICONDIRENTRY describes one icon stored in the ico file. The offset says where the icon image data
    ' starts in the file. The other fields give the information required to turn that image data into a valid
    ' bitmap.
    <StructLayout(LayoutKind.Sequential)> _
    Private Structure ICONDIRENTRY
        Public Width As Byte    ' Width, in pixels, of the image
        Public Height As Byte  ' Height, in pixels, of the image
        Public ColorCount As Byte  ' Number of colors in image (0 if >=8bpp)
        Public Reserved As Byte  ' Reserved ( must be 0)
        Public Planes As UShort  ' Color Planes
        Public BitCount As UShort  ' Bits per pixel
        Public BytesInRes As Integer   ' Length in bytes of the pixel data
        Public ImageOffset As Integer  ' Offset in the file where the pixel data starts.
    End Structure

    ' Each image is stored in the file as an ICONIMAGE structure:
    'typdef struct
    '{
    '   BITMAPINFOHEADER   icHeader;  // DIB header
    '   RGBQUAD  icColors[1];   // Color table
    '   BYTE    icXOR[1];  // DIB bits for XOR mask
    '   BYTE    icAND[1];  // DIB bits for AND mask
    '} ICONIMAGE, *LPICONIMAGE;

    <StructLayout(LayoutKind.Sequential)> _
    Private Structure BITMAPINFOHEADER
        Public Size As UInteger
        Public Width As Integer
        Public Height As Integer
        Public Planes As UShort
        Public BitCount As UShort
        Public Compression As UInteger
        Public SizeImage As UInteger
        Public XPelsPerMeter As Integer
        Public YPelsPerMeter As Integer
        Public ClrUsed As UInteger
        Public ClrImportant As UInteger
    End Structure

    ' The icon in an exe/dll file is stored in a very similar structure:
    <StructLayout(LayoutKind.Sequential, Pack:=2)> _
    Private Structure GRPICONDIRENTRY
        Public Width As Byte
        Public Height As Byte
        Public ColorCount As Byte
        Public Reserved As Byte
        Public Planes As UShort
        Public BitCount As UShort
        Public BytesInRes As Integer
        Public ID As UShort
    End Structure

    Public Shared Sub InjectIcon(ByVal exeFileName As String, ByVal iconFileName As String)
        InjectIcon(exeFileName, iconFileName, 1, 1)
    End Sub

    Public Shared Sub InjectIcon(ByVal exeFileName As String, ByVal iconFileName As String, ByVal iconGroupID As UInteger, ByVal iconBaseID As UInteger)
        Const RT_ICON = 3UI
        Const RT_GROUP_ICON = 14UI
        Dim iconFile As IconFile = iconFile.FromFile(iconFileName)
        Dim hUpdate = NativeMethods.BeginUpdateResource(exeFileName, False)
        Dim data = iconFile.CreateIconGroupData(iconBaseID)
        NativeMethods.UpdateResource(hUpdate, New IntPtr(RT_GROUP_ICON), New IntPtr(iconGroupID), 0, data, data.Length)
        For i = 0 To iconFile.ImageCount - 1
            Dim image = iconFile.ImageData(i)
            NativeMethods.UpdateResource(hUpdate, New IntPtr(RT_ICON), New IntPtr(iconBaseID + i), 0, image, image.Length)
        Next
        NativeMethods.EndUpdateResource(hUpdate, False)
    End Sub
    Private Class IconFile

        Private iconDir As New ICONDIR
        Private iconEntry() As ICONDIRENTRY
        Private iconImage()() As Byte

        Public ReadOnly Property ImageCount() As Integer
            Get
                Return iconDir.Count
            End Get
        End Property

        Public ReadOnly Property ImageData(ByVal index As Integer) As Byte()
            Get
                Return iconImage(index)
            End Get
        End Property

        Private Sub New()
        End Sub

        Public Shared Function FromFile(ByVal filename As String) As IconFile
            Dim instance As New IconFile
            ' Read all the bytes from the file.
            Dim fileBytes() As Byte = IO.File.ReadAllBytes(filename)
            ' First struct is an ICONDIR
            ' Pin the bytes from the file in memory so that we can read them.
            ' If we didn't pin them then they could move around (e.g. when the
            ' garbage collector compacts the heap)
            Dim pinnedBytes = GCHandle.Alloc(fileBytes, GCHandleType.Pinned)
            ' Read the ICONDIR
            instance.iconDir = DirectCast(Marshal.PtrToStructure(pinnedBytes.AddrOfPinnedObject, GetType(ICONDIR)), ICONDIR)
            ' which tells us how many images are in the ico file. For each image, there's a ICONDIRENTRY, and associated pixel data.
            instance.iconEntry = New ICONDIRENTRY(instance.iconDir.Count - 1) {}
            instance.iconImage = New Byte(instance.iconDir.Count - 1)() {}
            ' The first ICONDIRENTRY will be immediately after the ICONDIR, so the offset to it is the size of ICONDIR
            Dim offset = Marshal.SizeOf(instance.iconDir)
            ' After reading an ICONDIRENTRY we step forward by the size of an ICONDIRENTRY    
            Dim iconDirEntryType = GetType(ICONDIRENTRY)
            Dim size = Marshal.SizeOf(iconDirEntryType)
            For i = 0 To instance.iconDir.Count - 1
                ' Grab the structure.
                Dim entry = DirectCast(Marshal.PtrToStructure(New IntPtr(pinnedBytes.AddrOfPinnedObject.ToInt64 + offset), iconDirEntryType), ICONDIRENTRY)
                instance.iconEntry(i) = entry
                ' Grab the associated pixel data.
                instance.iconImage(i) = New Byte(entry.BytesInRes - 1) {}
                Buffer.BlockCopy(fileBytes, entry.ImageOffset, instance.iconImage(i), 0, entry.BytesInRes)
                offset += size
            Next
            pinnedBytes.Free()
            Return instance
        End Function

        Public Function CreateIconGroupData(ByVal iconBaseID As UInteger) As Byte()
            ' This will store the memory version of the icon.
            Dim sizeOfIconGroupData As Integer = Marshal.SizeOf(GetType(ICONDIR)) + Marshal.SizeOf(GetType(GRPICONDIRENTRY)) * ImageCount
            Dim data(sizeOfIconGroupData - 1) As Byte
            Dim pinnedData = GCHandle.Alloc(data, GCHandleType.Pinned)
            Marshal.StructureToPtr(iconDir, pinnedData.AddrOfPinnedObject, False)
            Dim offset = Marshal.SizeOf(iconDir)
            For i = 0 To ImageCount - 1
                Dim grpEntry As New GRPICONDIRENTRY
                Dim bitmapheader As New BITMAPINFOHEADER
                Dim pinnedBitmapInfoHeader = GCHandle.Alloc(bitmapheader, GCHandleType.Pinned)
                Marshal.Copy(ImageData(i), 0, pinnedBitmapInfoHeader.AddrOfPinnedObject, Marshal.SizeOf(GetType(BITMAPINFOHEADER)))
                pinnedBitmapInfoHeader.Free()
                grpEntry.Width = iconEntry(i).Width
                grpEntry.Height = iconEntry(i).Height
                grpEntry.ColorCount = iconEntry(i).ColorCount
                grpEntry.Reserved = iconEntry(i).Reserved
                grpEntry.Planes = bitmapheader.Planes
                grpEntry.BitCount = bitmapheader.BitCount
                grpEntry.BytesInRes = iconEntry(i).BytesInRes
                grpEntry.ID = CType(iconBaseID + i, UShort)
                Marshal.StructureToPtr(grpEntry, New IntPtr(pinnedData.AddrOfPinnedObject.ToInt64 + offset), False)
                offset += Marshal.SizeOf(GetType(GRPICONDIRENTRY))
            Next
            pinnedData.Free()
            Return data
        End Function

    End Class

    
End Class