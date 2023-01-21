Imports System.IO
Imports appmake.WindowWinSet
Public Class WindowWelcome
    Dim Recentpath As String = FileIO.SpecialDirectories.Temp & "\Tempapmk.txt"
    Shared ProInfo As ProjectDetails
    Public Enum OpeningType
        CancelWelcome
        RecentOrExistFile
        NewFile
    End Enum
    Public Structure ProjectDetails
        Dim File As String
        Dim OpenType As OpeningType
        Dim Project As WinSettings
    End Structure
#Region "Recents"
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
    Private Sub GetRecent()
        If FileIO.FileSystem.FileExists(Recentpath) = True Then
            For i As Integer = 0 To GetNumberOfLines(Recentpath) - 1
                AddRecent(ReadALine(Recentpath, GetNumberOfLines(Recentpath), i))
            Next
        End If
    End Sub
    Private Sub AddRecent(File As String)
        Dim label As New LinkLabel
        label.BackColor = Color.Transparent
        label.ActiveLinkColor = Drawing.Color.FromArgb(0, 159, 200)
        label.VisitedLinkColor = Drawing.Color.FromArgb(0, 153, 191)
        label.LinkColor = Drawing.Color.FromArgb(0, 153, 191)
        label.LinkBehavior = LinkBehavior.HoverUnderline
        label.Tag = File
        label.Font = lblNew.Font
        label.Text = Replace(FileIO.FileSystem.GetFileInfo(File).Name, ".amf", "")
        label.Width = 188
        AddHandler label.LinkClicked, AddressOf label_click
        pnlList.Controls.Add(label)
    End Sub
    Private Sub label_click(sender As Object, e As LinkLabelLinkClickedEventArgs)
        Dim i As LinkLabel = sender
        ProInfo.File = i.Tag
        ProInfo.OpenType = OpeningType.RecentOrExistFile
        Me.Close()
    End Sub
#End Region

    Private Sub WindowWelcome_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If ProInfo.OpenType = OpeningType.NewFile Then
        ElseIf ProInfo.OpenType = OpeningType.RecentOrExistFile Then
        Else
            ProInfo.OpenType = OpeningType.CancelWelcome
        End If
    End Sub

    
    Private Sub WindowWelcome_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        GetRecent()
    End Sub
    Public Shared Function ShowWelcome() As ProjectDetails
        Dim w As New WindowWelcome
        w.ShowDialog()
        Return ProInfo
    End Function

    Private Sub lblNew_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lblNew.LinkClicked
        Dim re As WinSettings = ShowWinSetting(WindowMain.pnlBase, True, True, False, "App Make", "App Make Project")
        If re.Resault = True Then
            ProInfo.OpenType = OpeningType.NewFile
            ProInfo.Project.Icon = re.Icon
            ProInfo.Project.ProjectName = re.ProjectName
            ProInfo.Project.Size = re.Size
            ProInfo.Project.Task = re.Task
            ProInfo.Project.Title = re.Title
            ProInfo.Project.Top = re.Top
            ProInfo.Project.Color = re.Color
            Me.Close()
        End If
    End Sub

    Private Sub lblOpen_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lblOpen.LinkClicked
        Dim opendialog As New OpenFileDialog
        opendialog.Filter = "App Make file (*.amf)|*.amf"
        If opendialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            ProInfo.OpenType = OpeningType.RecentOrExistFile
            ProInfo.File = opendialog.FileName
            Me.Close()
        End If
    End Sub


    Private Sub lnkClear_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnkClear.LinkClicked
        If FileIO.FileSystem.FileExists(Recentpath) = True Then
            FileIO.FileSystem.DeleteFile(Recentpath)
            pnlList.Controls.Clear()
            GetRecent()
        End If
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start("http://codino.tk/")
    End Sub
End Class