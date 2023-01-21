Imports System.ComponentModel

Public Class ProgressBarX
    Inherits ProgressBar
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)
        'Add your custom paint code here
    End Sub
    Public DisLabel As Label
    Dim MapMin As Integer = 0
    Dim MapMax As Integer = 10
    Dim valIndex As Integer = 0
    Dim DisType As DisplayTypes = DisplayTypes.Real
    Enum DisplayTypes
        Real
        Precentage
        Map
    End Enum
    Public Sub SetLabelData()
        If Not DisLabel Is Nothing Then
            Select Case DisType
                Case DisplayTypes.Real
                    DisLabel.Text = Me.Value
                Case DisplayTypes.Precentage
                    DisLabel.Text = Math.Round(Me.Value / Maximum * 100, 0) & "%"
                Case DisplayTypes.Map
                    DisLabel.Text = Math.Round(MapMax - ((MapMax - MapMin) * (Maximum - Me.Value) / (Maximum - Minimum)), 1)
            End Select
        End If
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
