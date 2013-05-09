<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmJudgePref
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
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.ID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.JudgeName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Judge = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Rounds = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Rating = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.OrdPct = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.cboTeam = New System.Windows.Forms.ComboBox()
        Me.butChangeFont = New System.Windows.Forms.Button()
        Me.lblMessage = New System.Windows.Forms.Label()
        Me.butTestAll = New System.Windows.Forms.Button()
        Me.DataGridView2 = New System.Windows.Forms.DataGridView()
        Me.Team = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Division = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.UnRated = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Status = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.butFixFormat = New System.Windows.Forms.Button()
        Me.butBasicInfo = New System.Windows.Forms.Button()
        Me.butHideGrid = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.lblCopyNotes = New System.Windows.Forms.Label()
        Me.butCopy = New System.Windows.Forms.Button()
        Me.cboCopyTo = New System.Windows.Forms.ComboBox()
        Me.butRecalcAllPercentiles = New System.Windows.Forms.Button()
        Me.butConvertTo333 = New System.Windows.Forms.Button()
        Me.butConvertTo0 = New System.Windows.Forms.Button()
        Me.butInitializePrefs = New System.Windows.Forms.Button()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.butSchoolConflict = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cboJudge = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cboSchool = New System.Windows.Forms.ComboBox()
        Me.butWhyPercentiles = New System.Windows.Forms.Button()
        Me.butOnlineUpdate = New System.Windows.Forms.Button()
        Me.chkUpdateDisplayedOnly = New System.Windows.Forms.CheckBox()
        Me.chkSuppressDownload = New System.Windows.Forms.CheckBox()
        Me.DataGridView3 = New System.Windows.Forms.DataGridView()
        Me.butDivConflicts = New System.Windows.Forms.Button()
        Me.chkJudgeOnly = New System.Windows.Forms.CheckBox()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.DataGridView3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.AllowUserToResizeColumns = False
        Me.DataGridView1.AllowUserToResizeRows = False
        Me.DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ID, Me.JudgeName, Me.Judge, Me.Rounds, Me.Rating, Me.OrdPct})
        Me.DataGridView1.Location = New System.Drawing.Point(12, 12)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.Size = New System.Drawing.Size(453, 678)
        Me.DataGridView1.TabIndex = 0
        '
        'ID
        '
        Me.ID.DataPropertyName = "ID"
        Me.ID.HeaderText = "ID"
        Me.ID.Name = "ID"
        Me.ID.Visible = False
        '
        'JudgeName
        '
        Me.JudgeName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.JudgeName.DataPropertyName = "JudgeName"
        Me.JudgeName.HeaderText = "Judge"
        Me.JudgeName.Name = "JudgeName"
        Me.JudgeName.ReadOnly = True
        Me.JudgeName.Width = 72
        '
        'Judge
        '
        Me.Judge.DataPropertyName = "Judge"
        Me.Judge.HeaderText = "JudgeNumber"
        Me.Judge.Name = "Judge"
        Me.Judge.Visible = False
        '
        'Rounds
        '
        Me.Rounds.DataPropertyName = "Rounds"
        Me.Rounds.HeaderText = "Rounds"
        Me.Rounds.Name = "Rounds"
        '
        'Rating
        '
        Me.Rating.DataPropertyName = "Rating"
        Me.Rating.HeaderText = "Rating"
        Me.Rating.Name = "Rating"
        '
        'OrdPct
        '
        Me.OrdPct.DataPropertyName = "OrdPct"
        Me.OrdPct.HeaderText = "Ordinal Percentile"
        Me.OrdPct.Name = "OrdPct"
        Me.OrdPct.ReadOnly = True
        '
        'cboTeam
        '
        Me.cboTeam.FormattingEnabled = True
        Me.cboTeam.Location = New System.Drawing.Point(467, 13)
        Me.cboTeam.Name = "cboTeam"
        Me.cboTeam.Size = New System.Drawing.Size(477, 28)
        Me.cboTeam.TabIndex = 1
        '
        'butChangeFont
        '
        Me.butChangeFont.Location = New System.Drawing.Point(1084, 371)
        Me.butChangeFont.Name = "butChangeFont"
        Me.butChangeFont.Size = New System.Drawing.Size(164, 28)
        Me.butChangeFont.TabIndex = 2
        Me.butChangeFont.Text = "Change Font Size"
        Me.butChangeFont.UseVisualStyleBackColor = True
        '
        'lblMessage
        '
        Me.lblMessage.AutoSize = True
        Me.lblMessage.Location = New System.Drawing.Point(467, 48)
        Me.lblMessage.MaximumSize = New System.Drawing.Size(300, 175)
        Me.lblMessage.MinimumSize = New System.Drawing.Size(300, 175)
        Me.lblMessage.Name = "lblMessage"
        Me.lblMessage.Size = New System.Drawing.Size(300, 175)
        Me.lblMessage.TabIndex = 3
        Me.lblMessage.Text = "Label1"
        '
        'butTestAll
        '
        Me.butTestAll.Location = New System.Drawing.Point(1084, 109)
        Me.butTestAll.Name = "butTestAll"
        Me.butTestAll.Size = New System.Drawing.Size(164, 29)
        Me.butTestAll.TabIndex = 4
        Me.butTestAll.Text = "Test all Teams"
        Me.butTestAll.UseVisualStyleBackColor = True
        '
        'DataGridView2
        '
        Me.DataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView2.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Team, Me.Division, Me.UnRated, Me.Status})
        Me.DataGridView2.Location = New System.Drawing.Point(471, 263)
        Me.DataGridView2.Name = "DataGridView2"
        Me.DataGridView2.RowHeadersVisible = False
        Me.DataGridView2.Size = New System.Drawing.Size(600, 467)
        Me.DataGridView2.TabIndex = 5
        Me.DataGridView2.Visible = False
        '
        'Team
        '
        Me.Team.DataPropertyName = "Team"
        Me.Team.HeaderText = "Team"
        Me.Team.Name = "Team"
        Me.Team.Width = 125
        '
        'Division
        '
        Me.Division.DataPropertyName = "Division"
        Me.Division.HeaderText = "Division"
        Me.Division.Name = "Division"
        Me.Division.Width = 75
        '
        'UnRated
        '
        Me.UnRated.DataPropertyName = "UnRated"
        Me.UnRated.HeaderText = "UnRated"
        Me.UnRated.Name = "UnRated"
        '
        'Status
        '
        Me.Status.DataPropertyName = "Status"
        Me.Status.HeaderText = "Status"
        Me.Status.Name = "Status"
        Me.Status.Width = 400
        '
        'butFixFormat
        '
        Me.butFixFormat.Location = New System.Drawing.Point(1084, 143)
        Me.butFixFormat.Name = "butFixFormat"
        Me.butFixFormat.Size = New System.Drawing.Size(164, 50)
        Me.butFixFormat.TabIndex = 6
        Me.butFixFormat.Text = "Test 1 and only 1 rating for all judges"
        Me.butFixFormat.UseVisualStyleBackColor = True
        '
        'butBasicInfo
        '
        Me.butBasicInfo.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.butBasicInfo.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butBasicInfo.Location = New System.Drawing.Point(1084, 13)
        Me.butBasicInfo.Name = "butBasicInfo"
        Me.butBasicInfo.Size = New System.Drawing.Size(164, 39)
        Me.butBasicInfo.TabIndex = 8
        Me.butBasicInfo.Text = "How to use this page"
        Me.butBasicInfo.UseVisualStyleBackColor = False
        '
        'butHideGrid
        '
        Me.butHideGrid.Location = New System.Drawing.Point(471, 235)
        Me.butHideGrid.Name = "butHideGrid"
        Me.butHideGrid.Size = New System.Drawing.Size(157, 28)
        Me.butHideGrid.TabIndex = 9
        Me.butHideGrid.Text = "Hide the status grid"
        Me.butHideGrid.UseVisualStyleBackColor = True
        Me.butHideGrid.Visible = False
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.lblCopyNotes)
        Me.GroupBox1.Controls.Add(Me.butCopy)
        Me.GroupBox1.Controls.Add(Me.cboCopyTo)
        Me.GroupBox1.Location = New System.Drawing.Point(783, 48)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(295, 200)
        Me.GroupBox1.TabIndex = 10
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "COPY"
        '
        'lblCopyNotes
        '
        Me.lblCopyNotes.AutoSize = True
        Me.lblCopyNotes.Location = New System.Drawing.Point(7, 24)
        Me.lblCopyNotes.MaximumSize = New System.Drawing.Size(275, 100)
        Me.lblCopyNotes.MinimumSize = New System.Drawing.Size(275, 100)
        Me.lblCopyNotes.Name = "lblCopyNotes"
        Me.lblCopyNotes.Size = New System.Drawing.Size(275, 100)
        Me.lblCopyNotes.TabIndex = 2
        Me.lblCopyNotes.Text = "This function will copy the prefs displayed at the left to the team selected belo" & _
            "w.  This will ERASE all existing prefs for the team selected below."
        '
        'butCopy
        '
        Me.butCopy.Location = New System.Drawing.Point(7, 168)
        Me.butCopy.Name = "butCopy"
        Me.butCopy.Size = New System.Drawing.Size(75, 28)
        Me.butCopy.TabIndex = 1
        Me.butCopy.Text = "Copy"
        Me.butCopy.UseVisualStyleBackColor = True
        '
        'cboCopyTo
        '
        Me.cboCopyTo.FormattingEnabled = True
        Me.cboCopyTo.Location = New System.Drawing.Point(7, 133)
        Me.cboCopyTo.Name = "cboCopyTo"
        Me.cboCopyTo.Size = New System.Drawing.Size(282, 28)
        Me.cboCopyTo.TabIndex = 0
        '
        'butRecalcAllPercentiles
        '
        Me.butRecalcAllPercentiles.Location = New System.Drawing.Point(1085, 252)
        Me.butRecalcAllPercentiles.Name = "butRecalcAllPercentiles"
        Me.butRecalcAllPercentiles.Size = New System.Drawing.Size(163, 48)
        Me.butRecalcAllPercentiles.TabIndex = 11
        Me.butRecalcAllPercentiles.Text = "Recalculate all percentiles"
        Me.butRecalcAllPercentiles.UseVisualStyleBackColor = True
        '
        'butConvertTo333
        '
        Me.butConvertTo333.Location = New System.Drawing.Point(1084, 307)
        Me.butConvertTo333.Name = "butConvertTo333"
        Me.butConvertTo333.Size = New System.Drawing.Size(164, 28)
        Me.butConvertTo333.TabIndex = 12
        Me.butConvertTo333.Text = "Convert 0 to 333"
        Me.butConvertTo333.UseVisualStyleBackColor = True
        '
        'butConvertTo0
        '
        Me.butConvertTo0.Location = New System.Drawing.Point(1084, 339)
        Me.butConvertTo0.Name = "butConvertTo0"
        Me.butConvertTo0.Size = New System.Drawing.Size(164, 28)
        Me.butConvertTo0.TabIndex = 13
        Me.butConvertTo0.Text = "Convert 333 to 0"
        Me.butConvertTo0.UseVisualStyleBackColor = True
        '
        'butInitializePrefs
        '
        Me.butInitializePrefs.Location = New System.Drawing.Point(1085, 55)
        Me.butInitializePrefs.Name = "butInitializePrefs"
        Me.butInitializePrefs.Size = New System.Drawing.Size(163, 48)
        Me.butInitializePrefs.TabIndex = 14
        Me.butInitializePrefs.Text = "Initialize Prefs (not destructive)"
        Me.butInitializePrefs.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.butSchoolConflict)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Controls.Add(Me.cboJudge)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Controls.Add(Me.cboSchool)
        Me.GroupBox2.Location = New System.Drawing.Point(1085, 402)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(169, 191)
        Me.GroupBox2.TabIndex = 15
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "School conflicts"
        '
        'butSchoolConflict
        '
        Me.butSchoolConflict.Location = New System.Drawing.Point(10, 134)
        Me.butSchoolConflict.Name = "butSchoolConflict"
        Me.butSchoolConflict.Size = New System.Drawing.Size(153, 54)
        Me.butSchoolConflict.TabIndex = 4
        Me.butSchoolConflict.Text = "Conflict this judge with this school"
        Me.butSchoolConflict.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 77)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(90, 20)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Select judge"
        '
        'cboJudge
        '
        Me.cboJudge.FormattingEnabled = True
        Me.cboJudge.Location = New System.Drawing.Point(6, 100)
        Me.cboJudge.Name = "cboJudge"
        Me.cboJudge.Size = New System.Drawing.Size(157, 28)
        Me.cboJudge.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 18)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(100, 20)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Select school:"
        '
        'cboSchool
        '
        Me.cboSchool.FormattingEnabled = True
        Me.cboSchool.Location = New System.Drawing.Point(6, 41)
        Me.cboSchool.Name = "cboSchool"
        Me.cboSchool.Size = New System.Drawing.Size(157, 28)
        Me.cboSchool.TabIndex = 0
        '
        'butWhyPercentiles
        '
        Me.butWhyPercentiles.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.butWhyPercentiles.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butWhyPercentiles.Location = New System.Drawing.Point(950, 13)
        Me.butWhyPercentiles.Name = "butWhyPercentiles"
        Me.butWhyPercentiles.Size = New System.Drawing.Size(128, 39)
        Me.butWhyPercentiles.TabIndex = 16
        Me.butWhyPercentiles.Text = "Why percentiles?"
        Me.butWhyPercentiles.UseVisualStyleBackColor = False
        '
        'butOnlineUpdate
        '
        Me.butOnlineUpdate.Location = New System.Drawing.Point(1089, 599)
        Me.butOnlineUpdate.Name = "butOnlineUpdate"
        Me.butOnlineUpdate.Size = New System.Drawing.Size(163, 32)
        Me.butOnlineUpdate.TabIndex = 17
        Me.butOnlineUpdate.Text = "Online pref update"
        Me.butOnlineUpdate.UseVisualStyleBackColor = True
        '
        'chkUpdateDisplayedOnly
        '
        Me.chkUpdateDisplayedOnly.AutoSize = True
        Me.chkUpdateDisplayedOnly.Checked = True
        Me.chkUpdateDisplayedOnly.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkUpdateDisplayedOnly.Location = New System.Drawing.Point(1095, 632)
        Me.chkUpdateDisplayedOnly.MaximumSize = New System.Drawing.Size(163, 24)
        Me.chkUpdateDisplayedOnly.MinimumSize = New System.Drawing.Size(163, 24)
        Me.chkUpdateDisplayedOnly.Name = "chkUpdateDisplayedOnly"
        Me.chkUpdateDisplayedOnly.Size = New System.Drawing.Size(163, 24)
        Me.chkUpdateDisplayedOnly.TabIndex = 18
        Me.chkUpdateDisplayedOnly.Text = "Displayed team only"
        Me.chkUpdateDisplayedOnly.UseVisualStyleBackColor = True
        '
        'chkSuppressDownload
        '
        Me.chkSuppressDownload.AutoSize = True
        Me.chkSuppressDownload.Location = New System.Drawing.Point(1096, 654)
        Me.chkSuppressDownload.Name = "chkSuppressDownload"
        Me.chkSuppressDownload.Size = New System.Drawing.Size(156, 24)
        Me.chkSuppressDownload.TabIndex = 19
        Me.chkSuppressDownload.Text = "Suppress Download"
        Me.chkSuppressDownload.UseVisualStyleBackColor = True
        '
        'DataGridView3
        '
        Me.DataGridView3.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.DataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView3.Location = New System.Drawing.Point(471, 319)
        Me.DataGridView3.Name = "DataGridView3"
        Me.DataGridView3.Size = New System.Drawing.Size(600, 406)
        Me.DataGridView3.TabIndex = 20
        Me.DataGridView3.Visible = False
        '
        'butDivConflicts
        '
        Me.butDivConflicts.Location = New System.Drawing.Point(1085, 199)
        Me.butDivConflicts.Name = "butDivConflicts"
        Me.butDivConflicts.Size = New System.Drawing.Size(163, 49)
        Me.butDivConflicts.TabIndex = 21
        Me.butDivConflicts.Text = "Write division conflicts to prefs"
        Me.butDivConflicts.UseVisualStyleBackColor = True
        '
        'chkJudgeOnly
        '
        Me.chkJudgeOnly.AutoSize = True
        Me.chkJudgeOnly.Location = New System.Drawing.Point(1096, 675)
        Me.chkJudgeOnly.Name = "chkJudgeOnly"
        Me.chkJudgeOnly.Size = New System.Drawing.Size(157, 24)
        Me.chkJudgeOnly.TabIndex = 22
        Me.chkJudgeOnly.Text = "Selected Judge only"
        Me.chkJudgeOnly.UseVisualStyleBackColor = True
        '
        'frmJudgePref
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1264, 720)
        Me.Controls.Add(Me.chkJudgeOnly)
        Me.Controls.Add(Me.butDivConflicts)
        Me.Controls.Add(Me.DataGridView3)
        Me.Controls.Add(Me.chkSuppressDownload)
        Me.Controls.Add(Me.chkUpdateDisplayedOnly)
        Me.Controls.Add(Me.butOnlineUpdate)
        Me.Controls.Add(Me.butWhyPercentiles)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.butInitializePrefs)
        Me.Controls.Add(Me.butConvertTo0)
        Me.Controls.Add(Me.butConvertTo333)
        Me.Controls.Add(Me.butRecalcAllPercentiles)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.butHideGrid)
        Me.Controls.Add(Me.butBasicInfo)
        Me.Controls.Add(Me.butFixFormat)
        Me.Controls.Add(Me.DataGridView2)
        Me.Controls.Add(Me.butTestAll)
        Me.Controls.Add(Me.lblMessage)
        Me.Controls.Add(Me.butChangeFont)
        Me.Controls.Add(Me.cboTeam)
        Me.Controls.Add(Me.DataGridView1)
        Me.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MaximumSize = New System.Drawing.Size(1280, 780)
        Me.MinimumSize = New System.Drawing.Size(1278, 726)
        Me.Name = "frmJudgePref"
        Me.Text = "Judge Preference information entered by teams"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.DataGridView3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents cboTeam As System.Windows.Forms.ComboBox
    Friend WithEvents butChangeFont As System.Windows.Forms.Button
    Friend WithEvents lblMessage As System.Windows.Forms.Label
    Friend WithEvents butTestAll As System.Windows.Forms.Button
    Friend WithEvents DataGridView2 As System.Windows.Forms.DataGridView
    Friend WithEvents butFixFormat As System.Windows.Forms.Button
    Friend WithEvents butBasicInfo As System.Windows.Forms.Button
    Friend WithEvents butHideGrid As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents lblCopyNotes As System.Windows.Forms.Label
    Friend WithEvents butCopy As System.Windows.Forms.Button
    Friend WithEvents cboCopyTo As System.Windows.Forms.ComboBox
    Friend WithEvents butRecalcAllPercentiles As System.Windows.Forms.Button
    Friend WithEvents butConvertTo333 As System.Windows.Forms.Button
    Friend WithEvents butConvertTo0 As System.Windows.Forms.Button
    Friend WithEvents butInitializePrefs As System.Windows.Forms.Button
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents butSchoolConflict As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cboJudge As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cboSchool As System.Windows.Forms.ComboBox
    Friend WithEvents ID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents JudgeName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Judge As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Rounds As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Rating As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents OrdPct As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents butWhyPercentiles As System.Windows.Forms.Button
    Friend WithEvents butOnlineUpdate As System.Windows.Forms.Button
    Friend WithEvents chkUpdateDisplayedOnly As System.Windows.Forms.CheckBox
    Friend WithEvents Team As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Division As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents UnRated As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Status As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents chkSuppressDownload As System.Windows.Forms.CheckBox
    Friend WithEvents DataGridView3 As System.Windows.Forms.DataGridView
    Friend WithEvents butDivConflicts As System.Windows.Forms.Button
    Friend WithEvents chkJudgeOnly As System.Windows.Forms.CheckBox
End Class
