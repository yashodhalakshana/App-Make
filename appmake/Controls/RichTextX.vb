Imports System.ComponentModel

Public Class RichTextX
    Inherits RichTextBox
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)
        'Add your custom paint code here
    End Sub
    Enum InputModes
        Append
        Insert
    End Enum
    Dim Input As InputModes = InputModes.Append
    Public Sub SetText(text As String)
        If Input = InputModes.Append Then
            Me.AppendText(text)
        Else
            Me.Text = text
        End If
    End Sub
    <Category("AppMake")>
    Public Property InputMode As InputModes
        Get
            InputMode = Input
        End Get
        Set(value As InputModes)
            Input = value
        End Set
    End Property
End Class
