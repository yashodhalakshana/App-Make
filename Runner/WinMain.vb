Imports System.IO
Imports System.Xml
Imports Runner.ValuesExtracter
Imports Runner.SliderX

Public Class WinMain
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
    Private Sub WinMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Dim m_xmlr As XmlTextReader

            Dim cmd As String = Command()
            Dim path As String = Nothing
            If cmd = Nothing Then
                path = Application.StartupPath & "\Resources.db"
            Else
                cmd = Replace(cmd, Chr(34), "")
                path = cmd
            End If
            m_xmlr = New XmlTextReader(path)
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
                        Me.Size = ReadSize(m_xmlr)
                        Me.MinimumSize = Me.Size
                        Me.MaximumSize = Me.Size
                        Me.Text = m_xmlr.ReadElementString("Title")
                        Me.ShowInTaskbar = m_xmlr.ReadElementString("Task")
                        Me.ShowIcon = m_xmlr.ReadElementString("Icon")
                        Me.TopMost = m_xmlr.ReadElementString("TopMost")
                        Me.BackColor = ReadColor(m_xmlr, "BG")
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
                        Me.Controls.Add(txt)
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
                        Me.Controls.Add(lbl)
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
                        AddHandler btn.MouseDown, AddressOf ButtonClick
                        AddHandler btn.MouseUp, AddressOf ButtonRelease
                        Me.Controls.Add(btn)
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
                        Me.Controls.Add(sld)
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
                        AddHandler SP.ClickConnect, AddressOf ClickConnect
                        AddHandler SP.ClickDisconnect, AddressOf ClickDisconnect
                        Me.Controls.Add(SP)
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
                        Me.Controls.Add(chk)
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
                        Me.Controls.Add(pgb)
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
                        Me.Controls.Add(lbx)
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
                        Me.Controls.Add(rtb)
                End Select
            End While
            'close the reader
            m_xmlr.Close()
            For Each c As Control In Me.Controls
                If c.GetType.ToString = "Runner.SliderX" Then
                    Dim s As SliderX = c
                    If Not s.Tag = "No" Then
                        For Each l As Control In Me.Controls
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
        Catch ex As Exception
            MessageBox.Show(ex.Message.ToString, "Runner")
            End
        End Try
    End Sub
    Private Sub CreateCode()
        Dim res As String = Nothing
        For Each s As Control In Me.Controls
            Select Case s.GetType.ToString
                Case "Runner.TextBox"
                    Dim t As TextBox = s
                    If t.IsClick = True Then
                        res += t.Value & ","
                    Else
                        res += "-1" & ","
                    End If
            End Select
        Next
        For Each s As Control In Me.Controls
            Select Case s.GetType.ToString
                Case "Runner.SliderX"
                    Dim t As SliderX = s
                    res += t.Value & ","
            End Select
        Next
        For Each s As Control In Me.Controls
            Select Case s.GetType.ToString
                Case "System.Windows.Forms.Button"
                    Dim t As Button = s
                    If t.Tag = 1 Then
                        res += "1" & ","
                    Else
                        res += "0" & ","
                    End If
            End Select
        Next
        For Each s As Control In Me.Controls
            Select Case s.GetType.ToString
                Case "System.Windows.Forms.CheckBox"
                    Dim t As CheckBox = s
                    If t.Checked = True Then
                        res += "1" & ","
                    Else
                        res += "0" & ","
                    End If
            End Select
        Next

        Try
            Dim trimchar() As Char = {","}
            res = res.TrimEnd(trimchar)
        Catch ex As Exception
        End Try
        If srlPort.IsOpen = True Then
            srlPort.WriteLine(res)
        End If
    End Sub

#Region "Events"
    Private Sub ClickConnect(COM As String, Baud As String)
        srlPort.PortName = COM
        srlPort.BaudRate = Baud
        Try
            srlPort.Open()
        Catch ex As Exception
            MessageBox.Show(ex.Message.ToString, "App Make Runner", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
    Private Sub ClickDisconnect()
        Try
            srlPort.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message.ToString, "App Make Runner", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub ButtonClick(sender As Object, e As EventArgs)
        Dim s As Button = sender
        s.Tag = 1
    End Sub

    Private Sub ButtonRelease(sender As Object, e As EventArgs)
        Dim s As Button = sender
        s.Tag = 0
    End Sub
#End Region
    Delegate Sub myMethodDelegate(ByVal [text] As String)
    Dim myD1 As New myMethodDelegate(AddressOf myShowStringMethod)
    Dim rtb As New RichTextBox
    Private Sub SerialPort_DataReceived(sender As Object, e As IO.Ports.SerialDataReceivedEventArgs) Handles srlPort.DataReceived
        Dim str As String = srlPort.ReadExisting
        Try
            Invoke(myD1, str)
        Catch ex As Exception
        End Try
    End Sub
    Sub myShowStringMethod(ByVal myString As String)
        Try
            rtb.Text = myString
            Dim txt As String
            If rtb.Lines.Count() > 2 Then
                txt = rtb.Lines(3)
            ElseIf rtb.Lines.Count() = 2 Then
                txt = rtb.Text
            End If

            For Each s As Control In Me.Controls
                Select Case s.GetType.ToString
                    Case "Runner.ProgressBarX"
                        Dim t As ProgressBarX = s
                        t.Value = ExtartValue(txt, t.TargetValueIndex)
                    Case "Runner.LabelX"
                        Dim l As LabelX = s
                        l.SetText(ExtartValue(txt, l.TargetValueIndex))
                    Case "Runner.RichTextX"
                        Dim r As RichTextX = s
                        r.SetText(txt)
                End Select
            Next
        Catch ex As Exception
        End Try
    End Sub
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Try
            CreateCode()
        Catch ex As Exception
            Timer1.Stop()
            MessageBox.Show(ex.Message.ToString, "App Make", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End
        End Try
    End Sub

    Private Sub WinMain_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If srlPort.IsOpen = True Then
            Try
                e.Cancel = True
                Do While srlPort.IsOpen = False
                    srlPort.Close()
                    srlPort.Dispose()
                Loop
                End
            Catch ex As Exception
                End
            End Try
        End If
    End Sub
End Class