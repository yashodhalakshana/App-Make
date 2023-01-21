Public Class SerialPort
    Public Event ClcikConnect(COM As String, Baud As String)
    Public Event ClickDisconnect()
    Private Sub SerialPort_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        For Each s As String In My.Computer.Ports.SerialPortNames
            cmbPorts.Items.Add(s)
        Next
        cmbPorts.SelectedIndex = cmbPorts.Items.Count - 1
        cmbBaud.SelectedIndex = 4
    End Sub

    Private Sub btnPorts_Click(sender As Object, e As EventArgs) Handles btnPorts.Click
        cmbPorts.Items.Clear()
        For Each s As String In My.Computer.Ports.SerialPortNames
            cmbPorts.Items.Add(s)
        Next
        cmbPorts.SelectedIndex = cmbPorts.Items.Count - 1
    End Sub

    Private Sub btnConnect_Click(sender As Object, e As EventArgs) Handles btnConnect.Click
        If btnConnect.Text = "Connect" Then
            RaiseEvent ClcikConnect(cmbPorts.SelectedItem, cmbBaud.SelectedItem)
            btnPorts.Enabled = False
            cmbBaud.Enabled = False
            cmbPorts.Enabled = False
            btnConnect.Text = "Disonnect"
        Else
            RaiseEvent ClickDisconnect()
            btnPorts.Enabled = True
            cmbBaud.Enabled = True
            cmbPorts.Enabled = True
            btnConnect.Text = "Connect"
        End If
    End Sub
End Class
