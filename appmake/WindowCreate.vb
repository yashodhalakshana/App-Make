
Public Class WindowCreate
    Dim iconfile As String
    Structure EXEInfo
        Dim Location As String
        Dim IcoLocation As String
        Dim Filename As String
        Dim Resault As Boolean
    End Structure
    Shared resault As EXEInfo
    Public Shared Function ShowEXE() As EXEInfo
        Dim win As New WindowCreate
        win.ShowDialog()
        Return resault
    End Function

    Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
        Dim fbd As New FolderBrowserDialog
        If fbd.ShowDialog = Windows.Forms.DialogResult.OK Then
            txtLocation.Text = fbd.SelectedPath
        End If
    End Sub

    Private Sub btnMake_Click(sender As Object, e As EventArgs) Handles btnMake.Click
        If txtLocation.Text = Nothing Then
            MsgBox("Please enter location to continue.")
        Else
            If txtName.Text = Nothing Then
                MsgBox("Please enter file name to continue.")
            Else
                resault.Filename = txtName.Text
                resault.Location = txtLocation.Text
                resault.IcoLocation = txtIcon.Text
                resault.Resault = True
                Me.Close()
            End If
        End If
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        resault.Resault = False
        Me.Close()
    End Sub

    Private Sub btnicon_Click(sender As Object, e As EventArgs) Handles btnicon.Click
        Dim OpenFileDialog As New OpenFileDialog
        OpenFileDialog.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
        OpenFileDialog.Filter = "Icon Files (*.ico)|*.ico"
        If (OpenFileDialog.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK) Then
            iconfile = OpenFileDialog.FileName
            txtIcon.Text = iconfile
        End If
    End Sub
   
   
End Class