<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmShowPairings
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
        Me.DataGridView1 = New System.Windows.Forms.DataGridView
        Me.DataGridView2 = New System.Windows.Forms.DataGridView
        Me.butLoad = New System.Windows.Forms.Button
        Me.cboRound = New System.Windows.Forms.ComboBox
        Me.butDelete = New System.Windows.Forms.Button
        Me.butAdd = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.butDeleteRow = New System.Windows.Forms.Button
        Me.butAddPanel = New System.Windows.Forms.Button
        Me.DataGridView3 = New System.Windows.Forms.DataGridView
        Me.Criteria = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Value = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.chkJudgeUse = New System.Windows.Forms.CheckBox
        Me.grpJudgeSettings = New System.Windows.Forms.GroupBox
        Me.butBestFit = New System.Windows.Forms.Button
        Me.butOrdReport = New System.Windows.Forms.Button
        Me.chkSingleChangeFlights = New System.Windows.Forms.CheckBox
        Me.butJudgeReport = New System.Windows.Forms.Button
        Me.butJudgePlaceHelp = New System.Windows.Forms.Button
        Me.chkJudgeBracket = New System.Windows.Forms.CheckBox
        Me.chkPlaceSameSchool = New System.Windows.Forms.CheckBox
        Me.chkHearOnce = New System.Windows.Forms.CheckBox
        Me.butAutoJudges = New System.Windows.Forms.Button
        Me.butDumpJudges = New System.Windows.Forms.Button
        Me.chkColorCode = New System.Windows.Forms.CheckBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.chkJudgeSchool = New System.Windows.Forms.CheckBox
        Me.butLoadSettingHelp = New System.Windows.Forms.Button
        Me.chkShowRecords = New System.Windows.Forms.CheckBox
        Me.chkShowFullNames = New System.Windows.Forms.CheckBox
        Me.chkShowFits = New System.Windows.Forms.CheckBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.lblGridKey = New System.Windows.Forms.Label
        Me.butAutoRooms = New System.Windows.Forms.Button
        Me.butDumpRooms = New System.Windows.Forms.Button
        Me.grpRoomOptions = New System.Windows.Forms.GroupBox
        Me.radRandom = New System.Windows.Forms.RadioButton
        Me.Button1 = New System.Windows.Forms.Button
        Me.radBracketOrder = New System.Windows.Forms.RadioButton
        Me.radMinimizeMoves = New System.Windows.Forms.RadioButton
        Me.chkTubUse = New System.Windows.Forms.CheckBox
        Me.chkDisability = New System.Windows.Forms.CheckBox
        Me.butBye = New System.Windows.Forms.Button
        Me.butPrint = New System.Windows.Forms.Button
        Me.butRoundAudit = New System.Windows.Forms.Button
        Me.butPageHelp = New System.Windows.Forms.Button
        Me.butSideChange = New System.Windows.Forms.Button
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridView3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpJudgeSettings.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.grpRoomOptions.SuspendLayout()
        Me.SuspendLayout()
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToResizeColumns = False
        Me.DataGridView1.AllowUserToResizeRows = False
        Me.DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Location = New System.Drawing.Point(14, 66)
        Me.DataGridView1.MultiSelect = False
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.ReadOnly = True
        Me.DataGridView1.RowHeadersVisible = False
        Me.DataGridView1.Size = New System.Drawing.Size(699, 672)
        Me.DataGridView1.TabIndex = 0
        '
        'DataGridView2
        '
        Me.DataGridView2.AllowUserToAddRows = False
        Me.DataGridView2.AllowUserToDeleteRows = False
        Me.DataGridView2.AllowUserToResizeColumns = False
        Me.DataGridView2.AllowUserToResizeRows = False
        Me.DataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader
        Me.DataGridView2.Location = New System.Drawing.Point(715, 65)
        Me.DataGridView2.MultiSelect = False
        Me.DataGridView2.Name = "DataGridView2"
        Me.DataGridView2.ReadOnly = True
        Me.DataGridView2.RowHeadersVisible = False
        Me.DataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridView2.Size = New System.Drawing.Size(538, 410)
        Me.DataGridView2.TabIndex = 2
        '
        'butLoad
        '
        Me.butLoad.Location = New System.Drawing.Point(226, 2)
        Me.butLoad.Name = "butLoad"
        Me.butLoad.Size = New System.Drawing.Size(144, 28)
        Me.butLoad.TabIndex = 3
        Me.butLoad.Text = "Load Round"
        Me.butLoad.UseVisualStyleBackColor = True
        '
        'cboRound
        '
        Me.cboRound.FormattingEnabled = True
        Me.cboRound.Location = New System.Drawing.Point(14, 16)
        Me.cboRound.Name = "cboRound"
        Me.cboRound.Size = New System.Drawing.Size(206, 28)
        Me.cboRound.TabIndex = 4
        '
        'butDelete
        '
        Me.butDelete.Location = New System.Drawing.Point(620, 5)
        Me.butDelete.Name = "butDelete"
        Me.butDelete.Size = New System.Drawing.Size(89, 47)
        Me.butDelete.TabIndex = 5
        Me.butDelete.Text = "Delete Cell"
        Me.butDelete.UseVisualStyleBackColor = True
        '
        'butAdd
        '
        Me.butAdd.Location = New System.Drawing.Point(1099, 32)
        Me.butAdd.Name = "butAdd"
        Me.butAdd.Size = New System.Drawing.Size(95, 26)
        Me.butAdd.TabIndex = 6
        Me.butAdd.Text = "Add"
        Me.butAdd.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.butAdd.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(715, 480)
        Me.Label1.MaximumSize = New System.Drawing.Size(275, 120)
        Me.Label1.MinimumSize = New System.Drawing.Size(275, 120)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(275, 120)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "Messages appear here. "
        '
        'butDeleteRow
        '
        Me.butDeleteRow.Font = New System.Drawing.Font("Franklin Gothic Medium", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butDeleteRow.Location = New System.Drawing.Point(519, 5)
        Me.butDeleteRow.Name = "butDeleteRow"
        Me.butDeleteRow.Size = New System.Drawing.Size(95, 47)
        Me.butDeleteRow.TabIndex = 8
        Me.butDeleteRow.Text = "Delete Entire Row"
        Me.butDeleteRow.UseVisualStyleBackColor = True
        '
        'butAddPanel
        '
        Me.butAddPanel.Font = New System.Drawing.Font("Franklin Gothic Medium", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butAddPanel.Location = New System.Drawing.Point(418, 5)
        Me.butAddPanel.Name = "butAddPanel"
        Me.butAddPanel.Size = New System.Drawing.Size(95, 47)
        Me.butAddPanel.TabIndex = 9
        Me.butAddPanel.Text = "Add a new Row"
        Me.butAddPanel.UseVisualStyleBackColor = True
        '
        'DataGridView3
        '
        Me.DataGridView3.AllowUserToAddRows = False
        Me.DataGridView3.AllowUserToDeleteRows = False
        Me.DataGridView3.AllowUserToResizeColumns = False
        Me.DataGridView3.AllowUserToResizeRows = False
        Me.DataGridView3.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.DataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView3.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Criteria, Me.Value})
        Me.DataGridView3.Location = New System.Drawing.Point(6, 23)
        Me.DataGridView3.MaximumSize = New System.Drawing.Size(228, 84)
        Me.DataGridView3.MinimumSize = New System.Drawing.Size(228, 84)
        Me.DataGridView3.Name = "DataGridView3"
        Me.DataGridView3.RowHeadersVisible = False
        Me.DataGridView3.Size = New System.Drawing.Size(228, 84)
        Me.DataGridView3.TabIndex = 10
        '
        'Criteria
        '
        Me.Criteria.DataPropertyName = "Criteria"
        Me.Criteria.HeaderText = "Criteria"
        Me.Criteria.Name = "Criteria"
        Me.Criteria.ReadOnly = True
        '
        'Value
        '
        Me.Value.DataPropertyName = "Value"
        Me.Value.HeaderText = "Value"
        Me.Value.Name = "Value"
        '
        'chkJudgeUse
        '
        Me.chkJudgeUse.Location = New System.Drawing.Point(6, 44)
        Me.chkJudgeUse.Name = "chkJudgeUse"
        Me.chkJudgeUse.Size = New System.Drawing.Size(185, 22)
        Me.chkJudgeUse.TabIndex = 11
        Me.chkJudgeUse.Text = "Judge Use"
        Me.chkJudgeUse.UseVisualStyleBackColor = True
        '
        'grpJudgeSettings
        '
        Me.grpJudgeSettings.Controls.Add(Me.butBestFit)
        Me.grpJudgeSettings.Controls.Add(Me.butOrdReport)
        Me.grpJudgeSettings.Controls.Add(Me.chkSingleChangeFlights)
        Me.grpJudgeSettings.Controls.Add(Me.butJudgeReport)
        Me.grpJudgeSettings.Controls.Add(Me.butJudgePlaceHelp)
        Me.grpJudgeSettings.Controls.Add(Me.chkJudgeBracket)
        Me.grpJudgeSettings.Controls.Add(Me.chkPlaceSameSchool)
        Me.grpJudgeSettings.Controls.Add(Me.chkHearOnce)
        Me.grpJudgeSettings.Controls.Add(Me.butAutoJudges)
        Me.grpJudgeSettings.Controls.Add(Me.butDumpJudges)
        Me.grpJudgeSettings.Controls.Add(Me.DataGridView3)
        Me.grpJudgeSettings.Location = New System.Drawing.Point(1013, 480)
        Me.grpJudgeSettings.Name = "grpJudgeSettings"
        Me.grpJudgeSettings.Size = New System.Drawing.Size(240, 249)
        Me.grpJudgeSettings.TabIndex = 12
        Me.grpJudgeSettings.TabStop = False
        Me.grpJudgeSettings.Text = "Judge Placement Settings"
        Me.grpJudgeSettings.Visible = False
        '
        'butBestFit
        '
        Me.butBestFit.Location = New System.Drawing.Point(7, 215)
        Me.butBestFit.Name = "butBestFit"
        Me.butBestFit.Size = New System.Drawing.Size(67, 29)
        Me.butBestFit.TabIndex = 27
        Me.butBestFit.Text = "Best fit"
        Me.butBestFit.UseVisualStyleBackColor = True
        '
        'butOrdReport
        '
        Me.butOrdReport.Location = New System.Drawing.Point(170, 214)
        Me.butOrdReport.Name = "butOrdReport"
        Me.butOrdReport.Size = New System.Drawing.Size(70, 28)
        Me.butOrdReport.TabIndex = 24
        Me.butOrdReport.Text = "Ord Rpt"
        Me.butOrdReport.UseVisualStyleBackColor = True
        '
        'chkSingleChangeFlights
        '
        Me.chkSingleChangeFlights.AutoSize = True
        Me.chkSingleChangeFlights.Font = New System.Drawing.Font("Franklin Gothic Medium", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSingleChangeFlights.Location = New System.Drawing.Point(7, 193)
        Me.chkSingleChangeFlights.MaximumSize = New System.Drawing.Size(215, 20)
        Me.chkSingleChangeFlights.MinimumSize = New System.Drawing.Size(215, 20)
        Me.chkSingleChangeFlights.Name = "chkSingleChangeFlights"
        Me.chkSingleChangeFlights.Size = New System.Drawing.Size(215, 20)
        Me.chkSingleChangeFlights.TabIndex = 26
        Me.chkSingleChangeFlights.Text = "Single change flights"
        Me.chkSingleChangeFlights.UseVisualStyleBackColor = True
        '
        'butJudgeReport
        '
        Me.butJudgeReport.Location = New System.Drawing.Point(75, 215)
        Me.butJudgeReport.Name = "butJudgeReport"
        Me.butJudgeReport.Size = New System.Drawing.Size(93, 28)
        Me.butJudgeReport.TabIndex = 23
        Me.butJudgeReport.Text = "Judge Rp"
        Me.butJudgeReport.UseVisualStyleBackColor = True
        '
        'butJudgePlaceHelp
        '
        Me.butJudgePlaceHelp.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.butJudgePlaceHelp.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butJudgePlaceHelp.Location = New System.Drawing.Point(185, 109)
        Me.butJudgePlaceHelp.Name = "butJudgePlaceHelp"
        Me.butJudgePlaceHelp.Size = New System.Drawing.Size(49, 35)
        Me.butJudgePlaceHelp.TabIndex = 25
        Me.butJudgePlaceHelp.Text = "Help"
        Me.butJudgePlaceHelp.UseVisualStyleBackColor = False
        '
        'chkJudgeBracket
        '
        Me.chkJudgeBracket.AutoSize = True
        Me.chkJudgeBracket.Font = New System.Drawing.Font("Franklin Gothic Medium", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkJudgeBracket.Location = New System.Drawing.Point(6, 138)
        Me.chkJudgeBracket.MaximumSize = New System.Drawing.Size(215, 22)
        Me.chkJudgeBracket.MinimumSize = New System.Drawing.Size(215, 22)
        Me.chkJudgeBracket.Name = "chkJudgeBracket"
        Me.chkJudgeBracket.Size = New System.Drawing.Size(215, 22)
        Me.chkJudgeBracket.TabIndex = 24
        Me.chkJudgeBracket.Text = "Judges In Bracket Order"
        Me.chkJudgeBracket.UseVisualStyleBackColor = True
        '
        'chkPlaceSameSchool
        '
        Me.chkPlaceSameSchool.AutoSize = True
        Me.chkPlaceSameSchool.Font = New System.Drawing.Font("Franklin Gothic Medium", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPlaceSameSchool.Location = New System.Drawing.Point(6, 175)
        Me.chkPlaceSameSchool.MaximumSize = New System.Drawing.Size(215, 20)
        Me.chkPlaceSameSchool.MinimumSize = New System.Drawing.Size(215, 20)
        Me.chkPlaceSameSchool.Name = "chkPlaceSameSchool"
        Me.chkPlaceSameSchool.Size = New System.Drawing.Size(215, 20)
        Me.chkPlaceSameSchool.TabIndex = 23
        Me.chkPlaceSameSchool.Text = "Place Same School Debates"
        Me.chkPlaceSameSchool.UseVisualStyleBackColor = True
        '
        'chkHearOnce
        '
        Me.chkHearOnce.AutoSize = True
        Me.chkHearOnce.Checked = True
        Me.chkHearOnce.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkHearOnce.Font = New System.Drawing.Font("Franklin Gothic Medium", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkHearOnce.Location = New System.Drawing.Point(6, 157)
        Me.chkHearOnce.MaximumSize = New System.Drawing.Size(215, 20)
        Me.chkHearOnce.MinimumSize = New System.Drawing.Size(215, 20)
        Me.chkHearOnce.Name = "chkHearOnce"
        Me.chkHearOnce.Size = New System.Drawing.Size(215, 20)
        Me.chkHearOnce.TabIndex = 22
        Me.chkHearOnce.Text = "Only hear a team once"
        Me.chkHearOnce.UseVisualStyleBackColor = True
        '
        'butAutoJudges
        '
        Me.butAutoJudges.Location = New System.Drawing.Point(90, 110)
        Me.butAutoJudges.Name = "butAutoJudges"
        Me.butAutoJudges.Size = New System.Drawing.Size(93, 28)
        Me.butAutoJudges.TabIndex = 20
        Me.butAutoJudges.Text = "Auto Place"
        Me.butAutoJudges.UseVisualStyleBackColor = True
        '
        'butDumpJudges
        '
        Me.butDumpJudges.Location = New System.Drawing.Point(6, 110)
        Me.butDumpJudges.Name = "butDumpJudges"
        Me.butDumpJudges.Size = New System.Drawing.Size(80, 28)
        Me.butDumpJudges.TabIndex = 21
        Me.butDumpJudges.Text = "Dump All"
        Me.butDumpJudges.UseVisualStyleBackColor = True
        '
        'chkColorCode
        '
        Me.chkColorCode.AutoSize = True
        Me.chkColorCode.Checked = True
        Me.chkColorCode.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkColorCode.Location = New System.Drawing.Point(148, 38)
        Me.chkColorCode.Name = "chkColorCode"
        Me.chkColorCode.Size = New System.Drawing.Size(104, 24)
        Me.chkColorCode.TabIndex = 13
        Me.chkColorCode.Text = "Judge Color"
        Me.chkColorCode.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.chkJudgeSchool)
        Me.GroupBox1.Controls.Add(Me.butLoadSettingHelp)
        Me.GroupBox1.Controls.Add(Me.chkShowRecords)
        Me.GroupBox1.Controls.Add(Me.chkColorCode)
        Me.GroupBox1.Controls.Add(Me.chkShowFullNames)
        Me.GroupBox1.Controls.Add(Me.chkShowFits)
        Me.GroupBox1.Controls.Add(Me.chkJudgeUse)
        Me.GroupBox1.Location = New System.Drawing.Point(719, 618)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(288, 111)
        Me.GroupBox1.TabIndex = 14
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Load Settings"
        '
        'chkJudgeSchool
        '
        Me.chkJudgeSchool.AutoSize = True
        Me.chkJudgeSchool.Checked = True
        Me.chkJudgeSchool.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkJudgeSchool.Location = New System.Drawing.Point(6, 84)
        Me.chkJudgeSchool.Name = "chkJudgeSchool"
        Me.chkJudgeSchool.Size = New System.Drawing.Size(112, 24)
        Me.chkJudgeSchool.TabIndex = 27
        Me.chkJudgeSchool.Text = "Judge school"
        Me.chkJudgeSchool.UseVisualStyleBackColor = True
        '
        'butLoadSettingHelp
        '
        Me.butLoadSettingHelp.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.butLoadSettingHelp.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butLoadSettingHelp.Location = New System.Drawing.Point(148, 59)
        Me.butLoadSettingHelp.Name = "butLoadSettingHelp"
        Me.butLoadSettingHelp.Size = New System.Drawing.Size(102, 28)
        Me.butLoadSettingHelp.TabIndex = 26
        Me.butLoadSettingHelp.Text = "Decoder"
        Me.butLoadSettingHelp.UseVisualStyleBackColor = False
        '
        'chkShowRecords
        '
        Me.chkShowRecords.AutoSize = True
        Me.chkShowRecords.Checked = True
        Me.chkShowRecords.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkShowRecords.Location = New System.Drawing.Point(6, 63)
        Me.chkShowRecords.Name = "chkShowRecords"
        Me.chkShowRecords.Size = New System.Drawing.Size(115, 24)
        Me.chkShowRecords.TabIndex = 16
        Me.chkShowRecords.Text = "Team records"
        Me.chkShowRecords.UseVisualStyleBackColor = True
        '
        'chkShowFullNames
        '
        Me.chkShowFullNames.AutoSize = True
        Me.chkShowFullNames.Location = New System.Drawing.Point(6, 23)
        Me.chkShowFullNames.Name = "chkShowFullNames"
        Me.chkShowFullNames.Size = New System.Drawing.Size(136, 24)
        Me.chkShowFullNames.TabIndex = 15
        Me.chkShowFullNames.Text = "Full team names"
        Me.chkShowFullNames.UseVisualStyleBackColor = True
        '
        'chkShowFits
        '
        Me.chkShowFits.AutoSize = True
        Me.chkShowFits.Location = New System.Drawing.Point(148, 18)
        Me.chkShowFits.Name = "chkShowFits"
        Me.chkShowFits.Size = New System.Drawing.Size(102, 24)
        Me.chkShowFits.TabIndex = 14
        Me.chkShowFits.Text = "N judge fits"
        Me.chkShowFits.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(226, 33)
        Me.Label2.MinimumSize = New System.Drawing.Size(150, 25)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(182, 25)
        Me.Label2.TabIndex = 15
        Me.Label2.Text = "(See settings bottom-right)"
        '
        'lblGridKey
        '
        Me.lblGridKey.AutoSize = True
        Me.lblGridKey.Location = New System.Drawing.Point(715, 13)
        Me.lblGridKey.MaximumSize = New System.Drawing.Size(320, 40)
        Me.lblGridKey.MinimumSize = New System.Drawing.Size(320, 40)
        Me.lblGridKey.Name = "lblGridKey"
        Me.lblGridKey.Size = New System.Drawing.Size(320, 40)
        Me.lblGridKey.TabIndex = 16
        Me.lblGridKey.Text = "Label3"
        '
        'butAutoRooms
        '
        Me.butAutoRooms.Location = New System.Drawing.Point(137, 23)
        Me.butAutoRooms.Name = "butAutoRooms"
        Me.butAutoRooms.Size = New System.Drawing.Size(97, 28)
        Me.butAutoRooms.TabIndex = 17
        Me.butAutoRooms.Text = "Auto Rooms"
        Me.butAutoRooms.UseVisualStyleBackColor = True
        '
        'butDumpRooms
        '
        Me.butDumpRooms.Location = New System.Drawing.Point(6, 23)
        Me.butDumpRooms.Name = "butDumpRooms"
        Me.butDumpRooms.Size = New System.Drawing.Size(125, 28)
        Me.butDumpRooms.TabIndex = 18
        Me.butDumpRooms.Text = "Dump all rooms"
        Me.butDumpRooms.UseVisualStyleBackColor = True
        '
        'grpRoomOptions
        '
        Me.grpRoomOptions.Controls.Add(Me.radRandom)
        Me.grpRoomOptions.Controls.Add(Me.Button1)
        Me.grpRoomOptions.Controls.Add(Me.radBracketOrder)
        Me.grpRoomOptions.Controls.Add(Me.radMinimizeMoves)
        Me.grpRoomOptions.Controls.Add(Me.chkTubUse)
        Me.grpRoomOptions.Controls.Add(Me.chkDisability)
        Me.grpRoomOptions.Controls.Add(Me.butAutoRooms)
        Me.grpRoomOptions.Controls.Add(Me.butDumpRooms)
        Me.grpRoomOptions.Location = New System.Drawing.Point(994, 508)
        Me.grpRoomOptions.MaximumSize = New System.Drawing.Size(240, 214)
        Me.grpRoomOptions.MinimumSize = New System.Drawing.Size(240, 214)
        Me.grpRoomOptions.Name = "grpRoomOptions"
        Me.grpRoomOptions.Size = New System.Drawing.Size(240, 214)
        Me.grpRoomOptions.TabIndex = 19
        Me.grpRoomOptions.TabStop = False
        Me.grpRoomOptions.Text = "Room Placement Options"
        Me.grpRoomOptions.Visible = False
        '
        'radRandom
        '
        Me.radRandom.AutoSize = True
        Me.radRandom.Location = New System.Drawing.Point(7, 134)
        Me.radRandom.Name = "radRandom"
        Me.radRandom.Size = New System.Drawing.Size(81, 24)
        Me.radRandom.TabIndex = 24
        Me.radRandom.TabStop = True
        Me.radRandom.Text = "Random"
        Me.radRandom.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(6, 160)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(63, 28)
        Me.Button1.TabIndex = 23
        Me.Button1.Text = "Report"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'radBracketOrder
        '
        Me.radBracketOrder.AutoSize = True
        Me.radBracketOrder.Location = New System.Drawing.Point(7, 106)
        Me.radBracketOrder.Name = "radBracketOrder"
        Me.radBracketOrder.Size = New System.Drawing.Size(113, 24)
        Me.radBracketOrder.TabIndex = 22
        Me.radBracketOrder.TabStop = True
        Me.radBracketOrder.Text = "BracketOrder"
        Me.radBracketOrder.UseVisualStyleBackColor = True
        '
        'radMinimizeMoves
        '
        Me.radMinimizeMoves.AutoSize = True
        Me.radMinimizeMoves.Checked = True
        Me.radMinimizeMoves.Location = New System.Drawing.Point(7, 79)
        Me.radMinimizeMoves.Name = "radMinimizeMoves"
        Me.radMinimizeMoves.Size = New System.Drawing.Size(173, 24)
        Me.radMinimizeMoves.TabIndex = 21
        Me.radMinimizeMoves.TabStop = True
        Me.radMinimizeMoves.Text = "Minimize Room moves"
        Me.radMinimizeMoves.UseVisualStyleBackColor = True
        '
        'chkTubUse
        '
        Me.chkTubUse.AutoSize = True
        Me.chkTubUse.Location = New System.Drawing.Point(137, 53)
        Me.chkTubUse.Name = "chkTubUse"
        Me.chkTubUse.Size = New System.Drawing.Size(79, 24)
        Me.chkTubUse.TabIndex = 20
        Me.chkTubUse.Text = "Tub Use"
        Me.chkTubUse.UseVisualStyleBackColor = True
        '
        'chkDisability
        '
        Me.chkDisability.AutoSize = True
        Me.chkDisability.Checked = True
        Me.chkDisability.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkDisability.Location = New System.Drawing.Point(6, 53)
        Me.chkDisability.Name = "chkDisability"
        Me.chkDisability.Size = New System.Drawing.Size(133, 24)
        Me.chkDisability.TabIndex = 19
        Me.chkDisability.Text = "Honor disabillity"
        Me.chkDisability.UseVisualStyleBackColor = True
        '
        'butBye
        '
        Me.butBye.Location = New System.Drawing.Point(719, 590)
        Me.butBye.Name = "butBye"
        Me.butBye.Size = New System.Drawing.Size(95, 26)
        Me.butBye.TabIndex = 20
        Me.butBye.Text = "Assign bye"
        Me.butBye.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.butBye.UseVisualStyleBackColor = True
        '
        'butPrint
        '
        Me.butPrint.Location = New System.Drawing.Point(1200, 5)
        Me.butPrint.Name = "butPrint"
        Me.butPrint.Size = New System.Drawing.Size(52, 47)
        Me.butPrint.TabIndex = 21
        Me.butPrint.Text = "Print"
        Me.butPrint.UseVisualStyleBackColor = True
        '
        'butRoundAudit
        '
        Me.butRoundAudit.Location = New System.Drawing.Point(1099, 2)
        Me.butRoundAudit.Name = "butRoundAudit"
        Me.butRoundAudit.Size = New System.Drawing.Size(95, 28)
        Me.butRoundAudit.TabIndex = 22
        Me.butRoundAudit.Text = "Audit prg"
        Me.butRoundAudit.UseVisualStyleBackColor = True
        '
        'butPageHelp
        '
        Me.butPageHelp.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.butPageHelp.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butPageHelp.Location = New System.Drawing.Point(1047, 5)
        Me.butPageHelp.Name = "butPageHelp"
        Me.butPageHelp.Size = New System.Drawing.Size(49, 48)
        Me.butPageHelp.TabIndex = 26
        Me.butPageHelp.Text = "Help"
        Me.butPageHelp.UseVisualStyleBackColor = False
        '
        'butSideChange
        '
        Me.butSideChange.Location = New System.Drawing.Point(820, 590)
        Me.butSideChange.Name = "butSideChange"
        Me.butSideChange.Size = New System.Drawing.Size(100, 28)
        Me.butSideChange.TabIndex = 27
        Me.butSideChange.Text = "Side change"
        Me.butSideChange.UseVisualStyleBackColor = True
        Me.butSideChange.Visible = False
        '
        'frmShowPairings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1264, 742)
        Me.Controls.Add(Me.butSideChange)
        Me.Controls.Add(Me.butPageHelp)
        Me.Controls.Add(Me.butRoundAudit)
        Me.Controls.Add(Me.butPrint)
        Me.Controls.Add(Me.grpRoomOptions)
        Me.Controls.Add(Me.butBye)
        Me.Controls.Add(Me.lblGridKey)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.grpJudgeSettings)
        Me.Controls.Add(Me.butAddPanel)
        Me.Controls.Add(Me.butDeleteRow)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.butAdd)
        Me.Controls.Add(Me.butDelete)
        Me.Controls.Add(Me.cboRound)
        Me.Controls.Add(Me.butLoad)
        Me.Controls.Add(Me.DataGridView2)
        Me.Controls.Add(Me.DataGridView1)
        Me.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "frmShowPairings"
        Me.Text = "Display Pairings"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridView3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpJudgeSettings.ResumeLayout(False)
        Me.grpJudgeSettings.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.grpRoomOptions.ResumeLayout(False)
        Me.grpRoomOptions.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents DataGridView2 As System.Windows.Forms.DataGridView
    Friend WithEvents butLoad As System.Windows.Forms.Button
    Friend WithEvents cboRound As System.Windows.Forms.ComboBox
    Friend WithEvents butDelete As System.Windows.Forms.Button
    Friend WithEvents butAdd As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents butDeleteRow As System.Windows.Forms.Button
    Friend WithEvents butAddPanel As System.Windows.Forms.Button
    Friend WithEvents DataGridView3 As System.Windows.Forms.DataGridView
    Friend WithEvents Criteria As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Value As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents chkJudgeUse As System.Windows.Forms.CheckBox
    Friend WithEvents grpJudgeSettings As System.Windows.Forms.GroupBox
    Friend WithEvents chkColorCode As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lblGridKey As System.Windows.Forms.Label
    Friend WithEvents butAutoRooms As System.Windows.Forms.Button
    Friend WithEvents butDumpRooms As System.Windows.Forms.Button
    Friend WithEvents grpRoomOptions As System.Windows.Forms.GroupBox
    Friend WithEvents radBracketOrder As System.Windows.Forms.RadioButton
    Friend WithEvents radMinimizeMoves As System.Windows.Forms.RadioButton
    Friend WithEvents chkTubUse As System.Windows.Forms.CheckBox
    Friend WithEvents chkDisability As System.Windows.Forms.CheckBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents butAutoJudges As System.Windows.Forms.Button
    Friend WithEvents butDumpJudges As System.Windows.Forms.Button
    Friend WithEvents radRandom As System.Windows.Forms.RadioButton
    Friend WithEvents butBye As System.Windows.Forms.Button
    Friend WithEvents chkHearOnce As System.Windows.Forms.CheckBox
    Friend WithEvents butPrint As System.Windows.Forms.Button
    Friend WithEvents butRoundAudit As System.Windows.Forms.Button
    Friend WithEvents chkShowFits As System.Windows.Forms.CheckBox
    Friend WithEvents chkShowFullNames As System.Windows.Forms.CheckBox
    Friend WithEvents chkShowRecords As System.Windows.Forms.CheckBox
    Friend WithEvents chkPlaceSameSchool As System.Windows.Forms.CheckBox
    Friend WithEvents chkJudgeBracket As System.Windows.Forms.CheckBox
    Friend WithEvents butJudgePlaceHelp As System.Windows.Forms.Button
    Friend WithEvents butLoadSettingHelp As System.Windows.Forms.Button
    Friend WithEvents chkSingleChangeFlights As System.Windows.Forms.CheckBox
    Friend WithEvents butJudgeReport As System.Windows.Forms.Button
    Friend WithEvents butOrdReport As System.Windows.Forms.Button
    Friend WithEvents butPageHelp As System.Windows.Forms.Button
    Friend WithEvents butBestFit As System.Windows.Forms.Button
    Friend WithEvents chkJudgeSchool As System.Windows.Forms.CheckBox
    Friend WithEvents butSideChange As System.Windows.Forms.Button
End Class
