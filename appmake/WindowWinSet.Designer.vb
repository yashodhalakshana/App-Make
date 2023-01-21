<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class WindowWinSet
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(WindowWinSet))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtTitle = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.btnColor = New System.Windows.Forms.Button()
        Me.He = New System.Windows.Forms.NumericUpDown()
        Me.Wi = New System.Windows.Forms.NumericUpDown()
        Me.pnl = New System.Windows.Forms.Panel()
        Me.chkTask = New System.Windows.Forms.CheckBox()
        Me.chkIcon = New System.Windows.Forms.CheckBox()
        Me.chkTopmost = New System.Windows.Forms.CheckBox()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnDone = New System.Windows.Forms.Button()
        Me.txtProject = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        CType(Me.He, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Wi, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 38)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(75, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Window Title :"
        '
        'txtTitle
        '
        Me.txtTitle.Location = New System.Drawing.Point(94, 38)
        Me.txtTitle.Name = "txtTitle"
        Me.txtTitle.Size = New System.Drawing.Size(243, 20)
        Me.txtTitle.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(13, 66)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(44, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Height :"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(16, 92)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(41, 13)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "Width :"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(10, 121)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(64, 13)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "Back color :"
        '
        'btnColor
        '
        Me.btnColor.Location = New System.Drawing.Point(154, 116)
        Me.btnColor.Name = "btnColor"
        Me.btnColor.Size = New System.Drawing.Size(97, 23)
        Me.btnColor.TabIndex = 5
        Me.btnColor.Text = "Browse Color"
        Me.btnColor.UseVisualStyleBackColor = True
        '
        'He
        '
        Me.He.Location = New System.Drawing.Point(94, 64)
        Me.He.Maximum = New Decimal(New Integer() {2000, 0, 0, 0})
        Me.He.Name = "He"
        Me.He.Size = New System.Drawing.Size(54, 20)
        Me.He.TabIndex = 6
        '
        'Wi
        '
        Me.Wi.Location = New System.Drawing.Point(94, 90)
        Me.Wi.Maximum = New Decimal(New Integer() {2000, 0, 0, 0})
        Me.Wi.Name = "Wi"
        Me.Wi.Size = New System.Drawing.Size(54, 20)
        Me.Wi.TabIndex = 7
        '
        'pnl
        '
        Me.pnl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl.Location = New System.Drawing.Point(94, 116)
        Me.pnl.Name = "pnl"
        Me.pnl.Size = New System.Drawing.Size(54, 21)
        Me.pnl.TabIndex = 8
        '
        'chkTask
        '
        Me.chkTask.AutoSize = True
        Me.chkTask.Checked = True
        Me.chkTask.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkTask.Location = New System.Drawing.Point(13, 153)
        Me.chkTask.Name = "chkTask"
        Me.chkTask.Size = New System.Drawing.Size(106, 17)
        Me.chkTask.TabIndex = 9
        Me.chkTask.Text = "Show in Taskbar"
        Me.chkTask.UseVisualStyleBackColor = True
        '
        'chkIcon
        '
        Me.chkIcon.AutoSize = True
        Me.chkIcon.Checked = True
        Me.chkIcon.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkIcon.Location = New System.Drawing.Point(13, 176)
        Me.chkIcon.Name = "chkIcon"
        Me.chkIcon.Size = New System.Drawing.Size(76, 17)
        Me.chkIcon.TabIndex = 10
        Me.chkIcon.Text = "Show icon"
        Me.chkIcon.UseVisualStyleBackColor = True
        '
        'chkTopmost
        '
        Me.chkTopmost.AutoSize = True
        Me.chkTopmost.Location = New System.Drawing.Point(13, 199)
        Me.chkTopmost.Name = "chkTopmost"
        Me.chkTopmost.Size = New System.Drawing.Size(70, 17)
        Me.chkTopmost.TabIndex = 11
        Me.chkTopmost.Text = "Top most"
        Me.chkTopmost.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(262, 237)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 12
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnDone
        '
        Me.btnDone.Location = New System.Drawing.Point(175, 237)
        Me.btnDone.Name = "btnDone"
        Me.btnDone.Size = New System.Drawing.Size(75, 23)
        Me.btnDone.TabIndex = 13
        Me.btnDone.Text = "Done"
        Me.btnDone.UseVisualStyleBackColor = True
        '
        'txtProject
        '
        Me.txtProject.Location = New System.Drawing.Point(94, 12)
        Me.txtProject.Name = "txtProject"
        Me.txtProject.Size = New System.Drawing.Size(243, 20)
        Me.txtProject.TabIndex = 15
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(13, 12)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(77, 13)
        Me.Label5.TabIndex = 14
        Me.Label5.Text = "Project Name :"
        '
        'WindowWinSet
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer), CType(CType(238, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(349, 271)
        Me.Controls.Add(Me.txtProject)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.btnDone)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.chkTopmost)
        Me.Controls.Add(Me.chkIcon)
        Me.Controls.Add(Me.chkTask)
        Me.Controls.Add(Me.pnl)
        Me.Controls.Add(Me.Wi)
        Me.Controls.Add(Me.He)
        Me.Controls.Add(Me.btnColor)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtTitle)
        Me.Controls.Add(Me.Label1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(365, 310)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(365, 310)
        Me.Name = "WindowWinSet"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Window Setting"
        CType(Me.He, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Wi, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtTitle As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents btnColor As System.Windows.Forms.Button
    Friend WithEvents He As System.Windows.Forms.NumericUpDown
    Friend WithEvents Wi As System.Windows.Forms.NumericUpDown
    Friend WithEvents pnl As System.Windows.Forms.Panel
    Friend WithEvents chkTask As System.Windows.Forms.CheckBox
    Friend WithEvents chkIcon As System.Windows.Forms.CheckBox
    Friend WithEvents chkTopmost As System.Windows.Forms.CheckBox
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnDone As System.Windows.Forms.Button
    Friend WithEvents txtProject As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
End Class
