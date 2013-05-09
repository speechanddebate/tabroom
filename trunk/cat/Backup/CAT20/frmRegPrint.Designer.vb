<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRegPrint
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
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.WebBrowser1 = New System.Windows.Forms.WebBrowser
        Me.butRegSheets = New System.Windows.Forms.Button
        Me.butPrint = New System.Windows.Forms.Button
        Me.lblStatus = New System.Windows.Forms.Label
        Me.butPrefConfirm = New System.Windows.Forms.Button
        Me.Button1 = New System.Windows.Forms.Button
        Me.cboRound = New System.Windows.Forms.ComboBox
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.radRoom = New System.Windows.Forms.RadioButton
        Me.radJudgeName = New System.Windows.Forms.RadioButton
        Me.butDisasterPlan = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.chkIsElim = New System.Windows.Forms.CheckBox
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.WebBrowser1)
        Me.GroupBox1.Location = New System.Drawing.Point(376, 29)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(896, 668)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "File Display"
        '
        'WebBrowser1
        '
        Me.WebBrowser1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.WebBrowser1.Location = New System.Drawing.Point(3, 20)
        Me.WebBrowser1.MinimumSize = New System.Drawing.Size(20, 20)
        Me.WebBrowser1.Name = "WebBrowser1"
        Me.WebBrowser1.Size = New System.Drawing.Size(890, 645)
        Me.WebBrowser1.TabIndex = 0
        '
        'butRegSheets
        '
        Me.butRegSheets.Location = New System.Drawing.Point(33, 70)
        Me.butRegSheets.Name = "butRegSheets"
        Me.butRegSheets.Size = New System.Drawing.Size(143, 60)
        Me.butRegSheets.TabIndex = 1
        Me.butRegSheets.Text = "Load Registration Sheets for Print"
        Me.butRegSheets.UseVisualStyleBackColor = True
        '
        'butPrint
        '
        Me.butPrint.Font = New System.Drawing.Font("Franklin Gothic Medium", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butPrint.Location = New System.Drawing.Point(93, 531)
        Me.butPrint.Name = "butPrint"
        Me.butPrint.Size = New System.Drawing.Size(165, 63)
        Me.butPrint.TabIndex = 2
        Me.butPrint.Text = "PRINT"
        Me.butPrint.UseVisualStyleBackColor = True
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Location = New System.Drawing.Point(207, 85)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(140, 20)
        Me.lblStatus.TabIndex = 3
        Me.lblStatus.Text = "Status appears here"
        '
        'butPrefConfirm
        '
        Me.butPrefConfirm.Location = New System.Drawing.Point(33, 138)
        Me.butPrefConfirm.Name = "butPrefConfirm"
        Me.butPrefConfirm.Size = New System.Drawing.Size(143, 79)
        Me.butPrefConfirm.TabIndex = 4
        Me.butPrefConfirm.Text = "Load Pref confirmation sheets for print"
        Me.butPrefConfirm.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(33, 299)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(336, 40)
        Me.Button1.TabIndex = 5
        Me.Button1.Text = "Load ballots for round selected above"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'cboRound
        '
        Me.cboRound.FormattingEnabled = True
        Me.cboRound.Location = New System.Drawing.Point(33, 236)
        Me.cboRound.Name = "cboRound"
        Me.cboRound.Size = New System.Drawing.Size(337, 28)
        Me.cboRound.TabIndex = 6
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.radRoom)
        Me.GroupBox2.Controls.Add(Me.radJudgeName)
        Me.GroupBox2.Location = New System.Drawing.Point(33, 345)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(200, 100)
        Me.GroupBox2.TabIndex = 7
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Sort ballots by..."
        '
        'radRoom
        '
        Me.radRoom.AutoSize = True
        Me.radRoom.Location = New System.Drawing.Point(7, 54)
        Me.radRoom.Name = "radRoom"
        Me.radRoom.Size = New System.Drawing.Size(65, 24)
        Me.radRoom.TabIndex = 1
        Me.radRoom.Text = "Room"
        Me.radRoom.UseVisualStyleBackColor = True
        '
        'radJudgeName
        '
        Me.radJudgeName.AutoSize = True
        Me.radJudgeName.Checked = True
        Me.radJudgeName.Location = New System.Drawing.Point(7, 24)
        Me.radJudgeName.Name = "radJudgeName"
        Me.radJudgeName.Size = New System.Drawing.Size(105, 24)
        Me.radJudgeName.TabIndex = 0
        Me.radJudgeName.TabStop = True
        Me.radJudgeName.Text = "Judge name"
        Me.radJudgeName.UseVisualStyleBackColor = True
        '
        'butDisasterPlan
        '
        Me.butDisasterPlan.Location = New System.Drawing.Point(239, 357)
        Me.butDisasterPlan.Name = "butDisasterPlan"
        Me.butDisasterPlan.Size = New System.Drawing.Size(108, 50)
        Me.butDisasterPlan.TabIndex = 8
        Me.butDisasterPlan.Text = "Email ballots"
        Me.butDisasterPlan.UseVisualStyleBackColor = True
        Me.butDisasterPlan.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label1.Location = New System.Drawing.Point(29, 18)
        Me.Label1.MaximumSize = New System.Drawing.Size(300, 45)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(284, 42)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "STEP ONE: Click one of the buttons below (this will populate the box to the right" & _
            ")"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label2.Location = New System.Drawing.Point(28, 466)
        Me.Label2.MaximumSize = New System.Drawing.Size(300, 45)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(293, 42)
        Me.Label2.TabIndex = 10
        Me.Label2.Text = "STEP TWO: Click the button below (this will send the contents of the box to the p" & _
            "rinter)"
        '
        'chkIsElim
        '
        Me.chkIsElim.AutoSize = True
        Me.chkIsElim.Location = New System.Drawing.Point(33, 270)
        Me.chkIsElim.Name = "chkIsElim"
        Me.chkIsElim.Size = New System.Drawing.Size(187, 24)
        Me.chkIsElim.TabIndex = 11
        Me.chkIsElim.Text = "Include Elim Instructions"
        Me.chkIsElim.UseVisualStyleBackColor = True
        '
        'frmRegPrint
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1284, 782)
        Me.Controls.Add(Me.chkIsElim)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.butDisasterPlan)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.cboRound)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.butPrefConfirm)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.butPrint)
        Me.Controls.Add(Me.butRegSheets)
        Me.Controls.Add(Me.GroupBox1)
        Me.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "frmRegPrint"
        Me.Text = "Print Registration Documents"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents WebBrowser1 As System.Windows.Forms.WebBrowser
    Friend WithEvents butRegSheets As System.Windows.Forms.Button
    Friend WithEvents butPrint As System.Windows.Forms.Button
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents butPrefConfirm As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents cboRound As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents radRoom As System.Windows.Forms.RadioButton
    Friend WithEvents radJudgeName As System.Windows.Forms.RadioButton
    Friend WithEvents butDisasterPlan As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents chkIsElim As System.Windows.Forms.CheckBox
End Class
