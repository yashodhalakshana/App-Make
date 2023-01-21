Imports System.ComponentModel

Public Class TextBox
    Public Event ClickSend(text As String)
    Public Event TextChange(text As Integer)
    Dim UseButton As Boolean = True

    <Category("AppMake")>
    Public Property Value As Integer
        Get
            Return TextBox1.Text
        End Get
        Set(value As Integer)
            TextBox1.Text = value
        End Set
    End Property
    <Category("AppMake")>
       Public Property ShowButton As Boolean
        Get
            ShowButton = UseButton
        End Get
        Set(value As Boolean)
            UseButton = value
            If value = True Then
                TextBox1.Size = New Size With {.Width = (Me.Size.Width - 105), .Height = Me.Size.Height}
                Button1.Visible = True
            Else
                TextBox1.Size = Me.Size
                Button1.Visible = False
            End If
        End Set
    End Property
    <Category("AppMake")>
       Public Property ButtonText As String
        Get
            Return Button1.Text
        End Get
        Set(value As String)
            Button1.Text = value
        End Set
    End Property

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        RaiseEvent ClickSend(TextBox1.Text)
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        RaiseEvent TextChange(TextBox1.Text)
    End Sub
End Class
