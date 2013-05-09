<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRounds
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
        Me.butResetAllRounds = New System.Windows.Forms.Button
        Me.DataGridView1 = New System.Windows.Forms.DataGridView
        Me.ID = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.rdEvent = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Label = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Rd_Name = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.JudgesPerPanel = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Flight = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.JudgePlaceScheme = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.PairingScheme = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.cboEvent = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.butShowDivision = New System.Windows.Forms.Button
        Me.butShowAllDivisions = New System.Windows.Forms.Button
        Me.Button3 = New System.Windows.Forms.Button
        Me.grpJudgePlaceScheme = New System.Windows.Forms.GroupBox
        Me.radTabRatings = New System.Windows.Forms.RadioButton
        Me.radTeamRating = New System.Windows.Forms.RadioButton
        Me.radRandom = New System.Windows.Forms.RadioButton
        Me.butChangeRatings = New System.Windows.Forms.Button
        Me.butBasicInfo = New System.Windows.Forms.Button
        Me.butColKey = New System.Windows.Forms.Button
        Me.lblRoomsJudges = New System.Windows.Forms.Label
        Me.butInitRoomsAndJudges = New System.Windows.Forms.Button
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.butAddRound = New System.Windows.Forms.Button
        Me.butBreakout = New System.Windows.Forms.Button
        Me.butRdIDFix = New System.Windows.Forms.Button
        Me.chkShowRoundID = New System.Windows.Forms.CheckBox
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpJudgePlaceScheme.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'butResetAllRounds
        '
        Me.butResetAllRounds.Location = New System.Drawing.Point(25, 577)
        Me.butResetAllRounds.Name = "butResetAllRounds"
        Me.butResetAllRounds.Size = New System.Drawing.Size(120, 52)
        Me.butResetAllRounds.TabIndex = 0
        Me.butResetAllRounds.Text = "Reset all rounds rounds"
        Me.butResetAllRounds.UseVisualStyleBackColor = True
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ID, Me.rdEvent, Me.Label, Me.Rd_Name, Me.JudgesPerPanel, Me.Flight, Me.JudgePlaceScheme, Me.PairingScheme})
        Me.DataGridView1.Location = New System.Drawing.Point(12, 70)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.Size = New System.Drawing.Size(1249, 501)
        Me.DataGridView1.TabIndex = 1
        '
        'ID
        '
        Me.ID.DataPropertyName = "ID"
        Me.ID.HeaderText = "ID"
        Me.ID.Name = "ID"
        Me.ID.Visible = False
        '
        'rdEvent
        '
        Me.rdEvent.DataPropertyName = "Event"
        Me.rdEvent.HeaderText = "Event"
        Me.rdEvent.Name = "rdEvent"
        Me.rdEvent.Visible = False
        '
        'Label
        '
        Me.Label.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.Label.DataPropertyName = "Label"
        Me.Label.HeaderText = "Full Label"
        Me.Label.Name = "Label"
        Me.Label.Width = 89
        '
        'Rd_Name
        '
        Me.Rd_Name.DataPropertyName = "RD_Name"
        Me.Rd_Name.HeaderText = "Numeric ID"
        Me.Rd_Name.Name = "Rd_Name"
        Me.Rd_Name.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Rd_Name.Visible = False
        '
        'JudgesPerPanel
        '
        Me.JudgesPerPanel.DataPropertyName = "JudgesPerPanel"
        Me.JudgesPerPanel.HeaderText = "Judges Per Panel"
        Me.JudgesPerPanel.Name = "JudgesPerPanel"
        '
        'Flight
        '
        Me.Flight.DataPropertyName = "Flighting"
        Me.Flight.HeaderText = "Flighting"
        Me.Flight.Name = "Flight"
        Me.Flight.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Flight.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'JudgePlaceScheme
        '
        Me.JudgePlaceScheme.DataPropertyName = "JudgePlaceScheme"
        Me.JudgePlaceScheme.HeaderText = "Judge Placement"
        Me.JudgePlaceScheme.Items.AddRange(New Object() {"TeamRating", "AssignedRating", "Random"})
        Me.JudgePlaceScheme.Name = "JudgePlaceScheme"
        Me.JudgePlaceScheme.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.JudgePlaceScheme.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        '
        'PairingScheme
        '
        Me.PairingScheme.DataPropertyName = "PairingScheme"
        Me.PairingScheme.HeaderText = "Pairing Scheme"
        Me.PairingScheme.Items.AddRange(New Object() {"None", "Preset", "Random", "HighHigh", "HighLow", "Elim"})
        Me.PairingScheme.Name = "PairingScheme"
        Me.PairingScheme.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.PairingScheme.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        '
        'cboEvent
        '
        Me.cboEvent.FormattingEnabled = True
        Me.cboEvent.Location = New System.Drawing.Point(12, 36)
        Me.cboEvent.Name = "cboEvent"
        Me.cboEvent.Size = New System.Drawing.Size(121, 28)
        Me.cboEvent.TabIndex = 4
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(104, 20)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Division/Event"
        '
        'butShowDivision
        '
        Me.butShowDivision.Location = New System.Drawing.Point(140, 36)
        Me.butShowDivision.Name = "butShowDivision"
        Me.butShowDivision.Size = New System.Drawing.Size(179, 28)
        Me.butShowDivision.TabIndex = 7
        Me.butShowDivision.Text = "Show Selected Division"
        Me.butShowDivision.UseVisualStyleBackColor = True
        '
        'butShowAllDivisions
        '
        Me.butShowAllDivisions.Location = New System.Drawing.Point(341, 36)
        Me.butShowAllDivisions.Name = "butShowAllDivisions"
        Me.butShowAllDivisions.Size = New System.Drawing.Size(151, 28)
        Me.butShowAllDivisions.TabIndex = 8
        Me.butShowAllDivisions.Text = "Show All Divisions"
        Me.butShowAllDivisions.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(25, 685)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(120, 53)
        Me.Button3.TabIndex = 9
        Me.Button3.Text = "Hack up elim seeds"
        Me.Button3.UseVisualStyleBackColor = True
        Me.Button3.Visible = False
        '
        'grpJudgePlaceScheme
        '
        Me.grpJudgePlaceScheme.Controls.Add(Me.radTabRatings)
        Me.grpJudgePlaceScheme.Controls.Add(Me.radTeamRating)
        Me.grpJudgePlaceScheme.Controls.Add(Me.radRandom)
        Me.grpJudgePlaceScheme.Controls.Add(Me.butChangeRatings)
        Me.grpJudgePlaceScheme.Location = New System.Drawing.Point(165, 578)
        Me.grpJudgePlaceScheme.Name = "grpJudgePlaceScheme"
        Me.grpJudgePlaceScheme.Size = New System.Drawing.Size(351, 152)
        Me.grpJudgePlaceScheme.TabIndex = 10
        Me.grpJudgePlaceScheme.TabStop = False
        Me.grpJudgePlaceScheme.Text = "Mass Change Judge Placement Scheme"
        '
        'radTabRatings
        '
        Me.radTabRatings.AutoSize = True
        Me.radTabRatings.Location = New System.Drawing.Point(6, 122)
        Me.radTabRatings.Name = "radTabRatings"
        Me.radTabRatings.Size = New System.Drawing.Size(171, 24)
        Me.radTabRatings.TabIndex = 3
        Me.radTabRatings.Text = "Tab Assigned Ratings "
        Me.radTabRatings.UseVisualStyleBackColor = True
        '
        'radTeamRating
        '
        Me.radTeamRating.AutoSize = True
        Me.radTeamRating.Location = New System.Drawing.Point(6, 100)
        Me.radTeamRating.Name = "radTeamRating"
        Me.radTeamRating.Size = New System.Drawing.Size(304, 24)
        Me.radTeamRating.TabIndex = 2
        Me.radTeamRating.Text = "Team Ratings (mutual prerference judging)"
        Me.radTeamRating.UseVisualStyleBackColor = True
        '
        'radRandom
        '
        Me.radRandom.AutoSize = True
        Me.radRandom.Checked = True
        Me.radRandom.Location = New System.Drawing.Point(6, 78)
        Me.radRandom.Name = "radRandom"
        Me.radRandom.Size = New System.Drawing.Size(81, 24)
        Me.radRandom.TabIndex = 1
        Me.radRandom.TabStop = True
        Me.radRandom.Text = "Random"
        Me.radRandom.UseVisualStyleBackColor = True
        '
        'butChangeRatings
        '
        Me.butChangeRatings.Location = New System.Drawing.Point(6, 23)
        Me.butChangeRatings.Name = "butChangeRatings"
        Me.butChangeRatings.Size = New System.Drawing.Size(339, 48)
        Me.butChangeRatings.TabIndex = 0
        Me.butChangeRatings.Text = "Change judge placement scheme for all displayed rounds to the one selected below"
        Me.butChangeRatings.UseVisualStyleBackColor = True
        '
        'butBasicInfo
        '
        Me.butBasicInfo.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.butBasicInfo.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butBasicInfo.Location = New System.Drawing.Point(1097, 13)
        Me.butBasicInfo.Name = "butBasicInfo"
        Me.butBasicInfo.Size = New System.Drawing.Size(164, 28)
        Me.butBasicInfo.TabIndex = 11
        Me.butBasicInfo.Text = "How to use this screen"
        Me.butBasicInfo.UseVisualStyleBackColor = False
        '
        'butColKey
        '
        Me.butColKey.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.butColKey.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butColKey.Location = New System.Drawing.Point(633, 36)
        Me.butColKey.Name = "butColKey"
        Me.butColKey.Size = New System.Drawing.Size(240, 28)
        Me.butColKey.TabIndex = 12
        Me.butColKey.Text = "What do these columns mean?"
        Me.butColKey.UseVisualStyleBackColor = False
        '
        'lblRoomsJudges
        '
        Me.lblRoomsJudges.AutoSize = True
        Me.lblRoomsJudges.Location = New System.Drawing.Point(10, 33)
        Me.lblRoomsJudges.MaximumSize = New System.Drawing.Size(440, 70)
        Me.lblRoomsJudges.MinimumSize = New System.Drawing.Size(440, 70)
        Me.lblRoomsJudges.Name = "lblRoomsJudges"
        Me.lblRoomsJudges.Size = New System.Drawing.Size(440, 70)
        Me.lblRoomsJudges.TabIndex = 15
        Me.lblRoomsJudges.Text = "Judges and rooms are initialized"
        '
        'butInitRoomsAndJudges
        '
        Me.butInitRoomsAndJudges.Location = New System.Drawing.Point(10, 119)
        Me.butInitRoomsAndJudges.MinimumSize = New System.Drawing.Size(440, 0)
        Me.butInitRoomsAndJudges.Name = "butInitRoomsAndJudges"
        Me.butInitRoomsAndJudges.Size = New System.Drawing.Size(440, 28)
        Me.butInitRoomsAndJudges.TabIndex = 14
        Me.butInitRoomsAndJudges.Text = "Initialize Rooms and Judges"
        Me.butInitRoomsAndJudges.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.lblRoomsJudges)
        Me.GroupBox1.Controls.Add(Me.butInitRoomsAndJudges)
        Me.GroupBox1.Location = New System.Drawing.Point(803, 578)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(458, 153)
        Me.GroupBox1.TabIndex = 16
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "JUDGE/ROOM INITIALIZATION"
        '
        'butAddRound
        '
        Me.butAddRound.Location = New System.Drawing.Point(522, 611)
        Me.butAddRound.Name = "butAddRound"
        Me.butAddRound.Size = New System.Drawing.Size(145, 48)
        Me.butAddRound.TabIndex = 17
        Me.butAddRound.Text = "Add new round for manual editing"
        Me.butAddRound.UseVisualStyleBackColor = True
        '
        'butBreakout
        '
        Me.butBreakout.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.butBreakout.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butBreakout.Location = New System.Drawing.Point(522, 578)
        Me.butBreakout.Name = "butBreakout"
        Me.butBreakout.Size = New System.Drawing.Size(240, 28)
        Me.butBreakout.TabIndex = 18
        Me.butBreakout.Text = "How to set up breakout rounds"
        Me.butBreakout.UseVisualStyleBackColor = False
        '
        'butRdIDFix
        '
        Me.butRdIDFix.Location = New System.Drawing.Point(25, 636)
        Me.butRdIDFix.Name = "butRdIDFix"
        Me.butRdIDFix.Size = New System.Drawing.Size(120, 32)
        Me.butRdIDFix.TabIndex = 19
        Me.butRdIDFix.Text = "Fix Round IDs"
        Me.butRdIDFix.UseVisualStyleBackColor = True
        '
        'chkShowRoundID
        '
        Me.chkShowRoundID.AutoSize = True
        Me.chkShowRoundID.Location = New System.Drawing.Point(1097, 40)
        Me.chkShowRoundID.Name = "chkShowRoundID"
        Me.chkShowRoundID.Size = New System.Drawing.Size(127, 24)
        Me.chkShowRoundID.TabIndex = 20
        Me.chkShowRoundID.Text = "Show Round ID"
        Me.chkShowRoundID.UseVisualStyleBackColor = True
        '
        'frmRounds
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1264, 742)
        Me.Controls.Add(Me.chkShowRoundID)
        Me.Controls.Add(Me.butRdIDFix)
        Me.Controls.Add(Me.butBreakout)
        Me.Controls.Add(Me.butAddRound)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.butColKey)
        Me.Controls.Add(Me.butBasicInfo)
        Me.Controls.Add(Me.grpJudgePlaceScheme)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.butShowAllDivisions)
        Me.Controls.Add(Me.butShowDivision)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cboEvent)
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.butResetAllRounds)
        Me.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "frmRounds"
        Me.Text = "Schedule rounds by event and timeslot"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpJudgePlaceScheme.ResumeLayout(False)
        Me.grpJudgePlaceScheme.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents butResetAllRounds As System.Windows.Forms.Button
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents cboEvent As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents butShowDivision As System.Windows.Forms.Button
    Friend WithEvents butShowAllDivisions As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents grpJudgePlaceScheme As System.Windows.Forms.GroupBox
    Friend WithEvents radRandom As System.Windows.Forms.RadioButton
    Friend WithEvents butChangeRatings As System.Windows.Forms.Button
    Friend WithEvents radTabRatings As System.Windows.Forms.RadioButton
    Friend WithEvents radTeamRating As System.Windows.Forms.RadioButton
    Friend WithEvents butBasicInfo As System.Windows.Forms.Button
    Friend WithEvents butColKey As System.Windows.Forms.Button
    Friend WithEvents lblRoomsJudges As System.Windows.Forms.Label
    Friend WithEvents butInitRoomsAndJudges As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents butAddRound As System.Windows.Forms.Button
    Friend WithEvents butBreakout As System.Windows.Forms.Button
    Friend WithEvents ID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents rdEvent As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Label As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Rd_Name As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents JudgesPerPanel As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Flight As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents JudgePlaceScheme As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents PairingScheme As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents butRdIDFix As System.Windows.Forms.Button
    Friend WithEvents chkShowRoundID As System.Windows.Forms.CheckBox
End Class
