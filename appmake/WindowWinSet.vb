Public Class WindowWinSet
    Public Structure WinSettings
        Dim ProjectName As String
        Dim Title As String
        Dim Color As Color
        Dim Size As Size
        Dim Icon As Boolean
        Dim Task As Boolean
        Dim Top As Boolean
        Dim Resault As Boolean
    End Structure
    Shared setts As New WinSettings
    Public Shared Function ShowWinSetting(panel As Panel, icon As Boolean, task As Boolean, top As Boolean, title As String, name As String) As WinSettings
        Dim w As New WindowWinSet
        w.pnl.BackColor = panel.BackColor
        w.He.Value = panel.Size.Height
        w.Wi.Value = panel.Size.Width
        w.txtTitle.Text = title
        w.chkTopmost.Checked = top
        w.chkTask.Checked = task
        w.chkIcon.Checked = icon
        w.txtProject.Text = name
        w.ShowDialog()
        Return setts
    End Function
    Private Sub btnColor_Click(sender As Object, e As EventArgs) Handles btnColor.Click
        Dim cd As New ColorDialog
        If cd.ShowDialog = Windows.Forms.DialogResult.OK Then
            pnl.BackColor = cd.Color
        End If
    End Sub
    Private Sub btnDone_Click(sender As Object, e As EventArgs) Handles btnDone.Click
        setts.Title = txtTitle.Text
        setts.Color = pnl.BackColor
        setts.Size = New Size(Wi.Value, He.Value)
        setts.Icon = chkIcon.Checked
        setts.Task = chkTask.Checked
        setts.Top = chkTopmost.Checked
        setts.ProjectName = txtProject.Text
        setts.Resault = True
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        setts.Resault = False
        Me.Close()
    End Sub

    Private Sub WindowWinSet_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class