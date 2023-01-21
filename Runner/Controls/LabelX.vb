Imports System.ComponentModel

Public Class LabelX
    Inherits Label
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)
        'Add your custom paint code here
    End Sub
    Enum DisplayTypes
        RealString
        Precentage
        Map
    End Enum
    Dim MapMin As Integer = 0
    Dim Minimum As Integer = 0
    Dim MapMax As Integer = 10
    Dim Maximum As Integer = 100
    Dim DisType As DisplayTypes = DisplayTypes.RealString
    Dim valIndex As Integer = 0
    Public Sub SetText(text As String)
        Try
            Select Case DisType
                Case DisplayTypes.RealString
                    Me.Text = text
                Case DisplayTypes.Precentage
                    Me.Text = Math.Round(text / Maximum * 100, 0) & "%"
                Case DisplayTypes.Map
                    Me.Text = Math.Round(MapMax - ((MapMax - MapMin) * (Maximum - text) / (Maximum - Minimum)), 1)
            End Select
        Catch ex As Exception
            Me.Text = ex.Message.ToString
        End Try
    End Sub
    <Category("AppMake")>
    Public Property TargetValueIndex As Integer
        Get
            TargetValueIndex = valIndex
        End Get
        Set(value As Integer)
            valIndex = value
        End Set
    End Property
    <Category("AppMake")>
    Public Property MinimumValue As Integer
        Get
            MinimumValue = Minimum
        End Get
        Set(value As Integer)
            Minimum = value
        End Set
    End Property
    <Category("AppMake")>
    Public Property MaximumValue As Integer
        Get
            MaximumValue = Maximum
        End Get
        Set(value As Integer)
            Maximum = value
        End Set
    End Property
    <Category("AppMake")>
    Public Property MapMinValue As Integer
        Get
            MapMinValue = MapMin
        End Get
        Set(value As Integer)
            MapMin = value
        End Set
    End Property
    <Category("AppMake")>
    Public Property MapMaxValue As Integer
        Get
            MapMaxValue = MapMax
        End Get
        Set(value As Integer)
            MapMax = value
        End Set
    End Property
    <Category("AppMake")>
    Public Property DisplayType As DisplayTypes
        Get
            DisplayType = DisType
        End Get
        Set(value As DisplayTypes)
            DisType = value
        End Set
    End Property
End Class
