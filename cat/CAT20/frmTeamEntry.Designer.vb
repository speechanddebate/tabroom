<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTeamEntry
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
        Dim DataGridViewCellStyle13 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle14 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle15 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle16 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.FULLNAME = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.CODE = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Rating = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.StopSched = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.TubUser = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.ADA = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.butMakeTeamNames = New System.Windows.Forms.Button()
        Me.butMakeAcro = New System.Windows.Forms.Button()
        Me.butInstr = New System.Windows.Forms.Button()
        Me.lblTeamBox = New System.Windows.Forms.Label()
        Me.grpAutoNames = New System.Windows.Forms.GroupBox()
        Me.butAddMasterToTeam = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.butMakeTeamFromMaster = New System.Windows.Forms.Button()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.dgvMasterStudents = New System.Windows.Forms.DataGridView()
        Me.MS_Last = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.MS_First = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.School = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.grpManualAdd = New System.Windows.Forms.GroupBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cboEvent = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cboSchoolAdd = New System.Windows.Forms.ComboBox()
        Me.DataGridView3 = New System.Windows.Forms.DataGridView()
        Me.First = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Last = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.butManualCreateTeam = New System.Windows.Forms.Button()
        Me.butAddDebater = New System.Windows.Forms.Button()
        Me.butPrint = New System.Windows.Forms.Button()
        Me.txtTeamSearch = New System.Windows.Forms.TextBox()
        Me.butTeamSearch = New System.Windows.Forms.Button()
        Me.cboEntryDivisions = New System.Windows.Forms.ComboBox()
        Me.chkAutoUpdate = New System.Windows.Forms.CheckBox()
        Me.LASTNAME = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.FIRSTNAME = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridView2 = New System.Windows.Forms.DataGridView()
        Me.grpDebatersOnTeam = New System.Windows.Forms.GroupBox()
        Me.lblEntryCounts = New System.Windows.Forms.Label()
        Me.lblOnline = New System.Windows.Forms.Label()
        Me.butSpellingChanges = New System.Windows.Forms.Button()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpAutoNames.SuspendLayout()
        CType(Me.dgvMasterStudents, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpManualAdd.SuspendLayout()
        CType(Me.DataGridView3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpDebatersOnTeam.SuspendLayout()
        Me.SuspendLayout()
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        DataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle13.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle13.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridView1.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle13
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.FULLNAME, Me.CODE, Me.Rating, Me.StopSched, Me.TubUser, Me.ADA})
        Me.DataGridView1.Location = New System.Drawing.Point(8, 414)
        Me.DataGridView1.MultiSelect = False
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.Size = New System.Drawing.Size(1244, 316)
        Me.DataGridView1.TabIndex = 0
        Me.DataGridView1.Tag = "Team Entries"
        '
        'FULLNAME
        '
        Me.FULLNAME.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells
        Me.FULLNAME.DataPropertyName = "FULLNAME"
        DataGridViewCellStyle14.Format = "N2"
        Me.FULLNAME.DefaultCellStyle = DataGridViewCellStyle14
        Me.FULLNAME.HeaderText = "Full Names"
        Me.FULLNAME.Name = "FULLNAME"
        Me.FULLNAME.ToolTipText = "Full Team Name"
        Me.FULLNAME.Width = 106
        '
        'CODE
        '
        Me.CODE.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells
        Me.CODE.DataPropertyName = "CODE"
        Me.CODE.HeaderText = "Acronym"
        Me.CODE.Name = "CODE"
        Me.CODE.Width = 89
        '
        'Rating
        '
        Me.Rating.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader
        Me.Rating.DataPropertyName = "RATING"
        DataGridViewCellStyle15.Format = "N0"
        DataGridViewCellStyle15.NullValue = Nothing
        Me.Rating.DefaultCellStyle = DataGridViewCellStyle15
        Me.Rating.HeaderText = "Rating"
        Me.Rating.Name = "Rating"
        Me.Rating.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Rating.Width = 77
        '
        'StopSched
        '
        Me.StopSched.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader
        Me.StopSched.DataPropertyName = "DROPPED"
        Me.StopSched.HeaderText = "Stop Scheduling"
        Me.StopSched.Name = "StopSched"
        Me.StopSched.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.StopSched.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.StopSched.Width = 140
        '
        'TubUser
        '
        Me.TubUser.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader
        Me.TubUser.DataPropertyName = "TUBDISABILITY"
        Me.TubUser.HeaderText = "Tub User"
        Me.TubUser.Name = "TubUser"
        Me.TubUser.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.TubUser.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.TubUser.Width = 90
        '
        'ADA
        '
        Me.ADA.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader
        Me.ADA.DataPropertyName = "ADA"
        Me.ADA.HeaderText = "Disability"
        Me.ADA.Name = "ADA"
        Me.ADA.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.ADA.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.ADA.Width = 94
        '
        'butMakeTeamNames
        '
        Me.butMakeTeamNames.Location = New System.Drawing.Point(1132, 219)
        Me.butMakeTeamNames.Name = "butMakeTeamNames"
        Me.butMakeTeamNames.Size = New System.Drawing.Size(124, 75)
        Me.butMakeTeamNames.TabIndex = 2
        Me.butMakeTeamNames.Text = "Make Full Team names for all entries"
        Me.butMakeTeamNames.UseVisualStyleBackColor = True
        '
        'butMakeAcro
        '
        Me.butMakeAcro.Location = New System.Drawing.Point(1132, 158)
        Me.butMakeAcro.Name = "butMakeAcro"
        Me.butMakeAcro.Size = New System.Drawing.Size(120, 55)
        Me.butMakeAcro.TabIndex = 3
        Me.butMakeAcro.Text = "Make acronyms for all entries"
        Me.butMakeAcro.UseVisualStyleBackColor = True
        '
        'butInstr
        '
        Me.butInstr.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.butInstr.Location = New System.Drawing.Point(1132, 25)
        Me.butInstr.Name = "butInstr"
        Me.butInstr.Size = New System.Drawing.Size(120, 55)
        Me.butInstr.TabIndex = 5
        Me.butInstr.Text = "How to use this page"
        Me.butInstr.UseVisualStyleBackColor = False
        '
        'lblTeamBox
        '
        Me.lblTeamBox.AutoSize = True
        Me.lblTeamBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTeamBox.Location = New System.Drawing.Point(8, 391)
        Me.lblTeamBox.Name = "lblTeamBox"
        Me.lblTeamBox.Size = New System.Drawing.Size(85, 22)
        Me.lblTeamBox.TabIndex = 6
        Me.lblTeamBox.Text = "TEAM GRID"
        '
        'grpAutoNames
        '
        Me.grpAutoNames.Controls.Add(Me.butAddMasterToTeam)
        Me.grpAutoNames.Controls.Add(Me.Label3)
        Me.grpAutoNames.Controls.Add(Me.butMakeTeamFromMaster)
        Me.grpAutoNames.Controls.Add(Me.txtSearch)
        Me.grpAutoNames.Controls.Add(Me.dgvMasterStudents)
        Me.grpAutoNames.Location = New System.Drawing.Point(433, 158)
        Me.grpAutoNames.Name = "grpAutoNames"
        Me.grpAutoNames.Size = New System.Drawing.Size(323, 208)
        Me.grpAutoNames.TabIndex = 9
        Me.grpAutoNames.TabStop = False
        Me.grpAutoNames.Text = "Add a Team by Looking Up Names"
        Me.grpAutoNames.Visible = False
        '
        'butAddMasterToTeam
        '
        Me.butAddMasterToTeam.Location = New System.Drawing.Point(12, 55)
        Me.butAddMasterToTeam.Name = "butAddMasterToTeam"
        Me.butAddMasterToTeam.Size = New System.Drawing.Size(298, 29)
        Me.butAddMasterToTeam.TabIndex = 2
        Me.butAddMasterToTeam.Text = "Add debater below to selected team"
        Me.butAddMasterToTeam.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(7, 25)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(82, 20)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "Search Box"
        '
        'butMakeTeamFromMaster
        '
        Me.butMakeTeamFromMaster.Location = New System.Drawing.Point(11, 87)
        Me.butMakeTeamFromMaster.Name = "butMakeTeamFromMaster"
        Me.butMakeTeamFromMaster.Size = New System.Drawing.Size(298, 29)
        Me.butMakeTeamFromMaster.TabIndex = 4
        Me.butMakeTeamFromMaster.Text = "Make new team with debter selected below"
        Me.butMakeTeamFromMaster.UseVisualStyleBackColor = True
        '
        'txtSearch
        '
        Me.txtSearch.Location = New System.Drawing.Point(95, 20)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(222, 25)
        Me.txtSearch.TabIndex = 1
        '
        'dgvMasterStudents
        '
        Me.dgvMasterStudents.AllowDrop = True
        Me.dgvMasterStudents.AllowUserToAddRows = False
        Me.dgvMasterStudents.ColumnHeadersHeight = 24
        Me.dgvMasterStudents.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.MS_Last, Me.MS_First, Me.School, Me.ID})
        Me.dgvMasterStudents.Location = New System.Drawing.Point(7, 138)
        Me.dgvMasterStudents.MultiSelect = False
        Me.dgvMasterStudents.Name = "dgvMasterStudents"
        Me.dgvMasterStudents.RowHeadersWidth = 24
        Me.dgvMasterStudents.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvMasterStudents.Size = New System.Drawing.Size(312, 58)
        Me.dgvMasterStudents.TabIndex = 0
        Me.dgvMasterStudents.Visible = False
        '
        'MS_Last
        '
        Me.MS_Last.DataPropertyName = "Last"
        Me.MS_Last.HeaderText = "Last"
        Me.MS_Last.Name = "MS_Last"
        '
        'MS_First
        '
        Me.MS_First.DataPropertyName = "First"
        Me.MS_First.HeaderText = "First"
        Me.MS_First.Name = "MS_First"
        '
        'School
        '
        Me.School.DataPropertyName = "School"
        Me.School.HeaderText = "School"
        Me.School.Name = "School"
        Me.School.Visible = False
        '
        'ID
        '
        Me.ID.DataPropertyName = "ID"
        Me.ID.HeaderText = "ID"
        Me.ID.Name = "ID"
        Me.ID.Visible = False
        '
        'grpManualAdd
        '
        Me.grpManualAdd.Controls.Add(Me.Label2)
        Me.grpManualAdd.Controls.Add(Me.cboEvent)
        Me.grpManualAdd.Controls.Add(Me.Label1)
        Me.grpManualAdd.Controls.Add(Me.cboSchoolAdd)
        Me.grpManualAdd.Controls.Add(Me.DataGridView3)
        Me.grpManualAdd.Controls.Add(Me.butManualCreateTeam)
        Me.grpManualAdd.Controls.Add(Me.butAddDebater)
        Me.grpManualAdd.Location = New System.Drawing.Point(781, 12)
        Me.grpManualAdd.Name = "grpManualAdd"
        Me.grpManualAdd.Size = New System.Drawing.Size(330, 346)
        Me.grpManualAdd.TabIndex = 10
        Me.grpManualAdd.TabStop = False
        Me.grpManualAdd.Text = "Change competitors or create new team"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(4, 176)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(65, 20)
        Me.Label2.TabIndex = 10
        Me.Label2.Text = "Division:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cboEvent
        '
        Me.cboEvent.FormattingEnabled = True
        Me.cboEvent.Location = New System.Drawing.Point(70, 173)
        Me.cboEvent.Name = "cboEvent"
        Me.cboEvent.Size = New System.Drawing.Size(239, 28)
        Me.cboEvent.TabIndex = 9
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(4, 143)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(57, 20)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "School:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cboSchoolAdd
        '
        Me.cboSchoolAdd.FormattingEnabled = True
        Me.cboSchoolAdd.Location = New System.Drawing.Point(70, 138)
        Me.cboSchoolAdd.Name = "cboSchoolAdd"
        Me.cboSchoolAdd.Size = New System.Drawing.Size(239, 28)
        Me.cboSchoolAdd.TabIndex = 7
        '
        'DataGridView3
        '
        Me.DataGridView3.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.DataGridView3.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        Me.DataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.DataGridView3.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.First, Me.Last})
        Me.DataGridView3.Location = New System.Drawing.Point(17, 25)
        Me.DataGridView3.Name = "DataGridView3"
        Me.DataGridView3.RowHeadersWidth = 24
        Me.DataGridView3.Size = New System.Drawing.Size(307, 106)
        Me.DataGridView3.TabIndex = 6
        '
        'First
        '
        Me.First.HeaderText = "First Name"
        Me.First.Name = "First"
        '
        'Last
        '
        Me.Last.HeaderText = "Last Name"
        Me.Last.Name = "Last"
        '
        'butManualCreateTeam
        '
        Me.butManualCreateTeam.Location = New System.Drawing.Point(17, 228)
        Me.butManualCreateTeam.Name = "butManualCreateTeam"
        Me.butManualCreateTeam.Size = New System.Drawing.Size(292, 32)
        Me.butManualCreateTeam.TabIndex = 5
        Me.butManualCreateTeam.Text = "Create a new team with these debaters"
        Me.butManualCreateTeam.UseVisualStyleBackColor = True
        '
        'butAddDebater
        '
        Me.butAddDebater.Location = New System.Drawing.Point(18, 267)
        Me.butAddDebater.Name = "butAddDebater"
        Me.butAddDebater.Size = New System.Drawing.Size(252, 36)
        Me.butAddDebater.TabIndex = 4
        Me.butAddDebater.Text = "Add debater to team selected below"
        Me.butAddDebater.UseVisualStyleBackColor = True
        '
        'butPrint
        '
        Me.butPrint.Location = New System.Drawing.Point(1132, 86)
        Me.butPrint.Name = "butPrint"
        Me.butPrint.Size = New System.Drawing.Size(120, 53)
        Me.butPrint.TabIndex = 11
        Me.butPrint.Text = "Print (sort before clicking)"
        Me.butPrint.UseVisualStyleBackColor = True
        '
        'txtTeamSearch
        '
        Me.txtTeamSearch.Location = New System.Drawing.Point(163, 388)
        Me.txtTeamSearch.Name = "txtTeamSearch"
        Me.txtTeamSearch.Size = New System.Drawing.Size(220, 25)
        Me.txtTeamSearch.TabIndex = 12
        '
        'butTeamSearch
        '
        Me.butTeamSearch.Location = New System.Drawing.Point(390, 388)
        Me.butTeamSearch.Name = "butTeamSearch"
        Me.butTeamSearch.Size = New System.Drawing.Size(75, 23)
        Me.butTeamSearch.TabIndex = 13
        Me.butTeamSearch.Text = "Search"
        Me.butTeamSearch.UseVisualStyleBackColor = True
        '
        'cboEntryDivisions
        '
        Me.cboEntryDivisions.FormattingEnabled = True
        Me.cboEntryDivisions.Location = New System.Drawing.Point(591, 385)
        Me.cboEntryDivisions.Name = "cboEntryDivisions"
        Me.cboEntryDivisions.Size = New System.Drawing.Size(232, 28)
        Me.cboEntryDivisions.TabIndex = 14
        Me.cboEntryDivisions.Text = "Show all divisions"
        '
        'chkAutoUpdate
        '
        Me.chkAutoUpdate.AutoSize = True
        Me.chkAutoUpdate.Checked = True
        Me.chkAutoUpdate.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkAutoUpdate.Location = New System.Drawing.Point(19, 197)
        Me.chkAutoUpdate.MaximumSize = New System.Drawing.Size(300, 50)
        Me.chkAutoUpdate.MinimumSize = New System.Drawing.Size(300, 50)
        Me.chkAutoUpdate.Name = "chkAutoUpdate"
        Me.chkAutoUpdate.Size = New System.Drawing.Size(300, 50)
        Me.chkAutoUpdate.TabIndex = 4
        Me.chkAutoUpdate.Text = "Update team names and acronym when debater names change"
        Me.chkAutoUpdate.UseVisualStyleBackColor = True
        '
        'LASTNAME
        '
        Me.LASTNAME.DataPropertyName = "LAST"
        Me.LASTNAME.HeaderText = "Last Name"
        Me.LASTNAME.Name = "LASTNAME"
        Me.LASTNAME.ReadOnly = True
        '
        'FIRSTNAME
        '
        Me.FIRSTNAME.DataPropertyName = "FIRST"
        Me.FIRSTNAME.HeaderText = "First Name"
        Me.FIRSTNAME.Name = "FIRSTNAME"
        Me.FIRSTNAME.ReadOnly = True
        '
        'DataGridView2
        '
        Me.DataGridView2.AllowDrop = True
        Me.DataGridView2.AllowUserToAddRows = False
        DataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle16.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle16.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle16.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle16.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle16.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle16.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridView2.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle16
        Me.DataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView2.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.FIRSTNAME, Me.LASTNAME})
        Me.DataGridView2.Location = New System.Drawing.Point(19, 41)
        Me.DataGridView2.Name = "DataGridView2"
        Me.DataGridView2.ReadOnly = True
        Me.DataGridView2.Size = New System.Drawing.Size(381, 150)
        Me.DataGridView2.TabIndex = 1
        '
        'grpDebatersOnTeam
        '
        Me.grpDebatersOnTeam.Controls.Add(Me.butSpellingChanges)
        Me.grpDebatersOnTeam.Controls.Add(Me.lblEntryCounts)
        Me.grpDebatersOnTeam.Controls.Add(Me.DataGridView2)
        Me.grpDebatersOnTeam.Controls.Add(Me.chkAutoUpdate)
        Me.grpDebatersOnTeam.Location = New System.Drawing.Point(8, 12)
        Me.grpDebatersOnTeam.Name = "grpDebatersOnTeam"
        Me.grpDebatersOnTeam.Size = New System.Drawing.Size(406, 354)
        Me.grpDebatersOnTeam.TabIndex = 15
        Me.grpDebatersOnTeam.TabStop = False
        Me.grpDebatersOnTeam.Text = "Debaters on Team"
        '
        'lblEntryCounts
        '
        Me.lblEntryCounts.AutoSize = True
        Me.lblEntryCounts.Location = New System.Drawing.Point(19, 246)
        Me.lblEntryCounts.MaximumSize = New System.Drawing.Size(365, 120)
        Me.lblEntryCounts.MinimumSize = New System.Drawing.Size(365, 120)
        Me.lblEntryCounts.Name = "lblEntryCounts"
        Me.lblEntryCounts.Size = New System.Drawing.Size(365, 120)
        Me.lblEntryCounts.TabIndex = 5
        Me.lblEntryCounts.Text = "Entry counts"
        '
        'lblOnline
        '
        Me.lblOnline.AutoSize = True
        Me.lblOnline.Location = New System.Drawing.Point(433, 25)
        Me.lblOnline.MaximumSize = New System.Drawing.Size(323, 300)
        Me.lblOnline.MinimumSize = New System.Drawing.Size(323, 300)
        Me.lblOnline.Name = "lblOnline"
        Me.lblOnline.Size = New System.Drawing.Size(323, 300)
        Me.lblOnline.TabIndex = 16
        Me.lblOnline.Text = "Label4"
        '
        'butSpellingChanges
        '
        Me.butSpellingChanges.Location = New System.Drawing.Point(312, 198)
        Me.butSpellingChanges.Name = "butSpellingChanges"
        Me.butSpellingChanges.Size = New System.Drawing.Size(88, 84)
        Me.butSpellingChanges.TabIndex = 6
        Me.butSpellingChanges.Text = "Spelling changes ENABLED"
        Me.butSpellingChanges.UseVisualStyleBackColor = True
        '
        'frmTeamEntry
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1264, 742)
        Me.Controls.Add(Me.lblOnline)
        Me.Controls.Add(Me.grpDebatersOnTeam)
        Me.Controls.Add(Me.butMakeTeamNames)
        Me.Controls.Add(Me.grpAutoNames)
        Me.Controls.Add(Me.butMakeAcro)
        Me.Controls.Add(Me.cboEntryDivisions)
        Me.Controls.Add(Me.butTeamSearch)
        Me.Controls.Add(Me.txtTeamSearch)
        Me.Controls.Add(Me.butPrint)
        Me.Controls.Add(Me.grpManualAdd)
        Me.Controls.Add(Me.lblTeamBox)
        Me.Controls.Add(Me.butInstr)
        Me.Controls.Add(Me.DataGridView1)
        Me.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "frmTeamEntry"
        Me.Text = "Team Entry Management"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpAutoNames.ResumeLayout(False)
        Me.grpAutoNames.PerformLayout()
        CType(Me.dgvMasterStudents, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpManualAdd.ResumeLayout(False)
        Me.grpManualAdd.PerformLayout()
        CType(Me.DataGridView3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpDebatersOnTeam.ResumeLayout(False)
        Me.grpDebatersOnTeam.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents butMakeTeamNames As System.Windows.Forms.Button
    Friend WithEvents butMakeAcro As System.Windows.Forms.Button
    Friend WithEvents butInstr As System.Windows.Forms.Button
    Friend WithEvents lblTeamBox As System.Windows.Forms.Label
    Friend WithEvents grpAutoNames As System.Windows.Forms.GroupBox
    Friend WithEvents grpManualAdd As System.Windows.Forms.GroupBox
    Friend WithEvents butAddDebater As System.Windows.Forms.Button
    Friend WithEvents butManualCreateTeam As System.Windows.Forms.Button
    Friend WithEvents DataGridView3 As System.Windows.Forms.DataGridView
    Friend WithEvents First As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Last As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents cboSchoolAdd As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cboEvent As System.Windows.Forms.ComboBox
    Friend WithEvents dgvMasterStudents As System.Windows.Forms.DataGridView
    Friend WithEvents txtSearch As System.Windows.Forms.TextBox
    Friend WithEvents butAddMasterToTeam As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents butMakeTeamFromMaster As System.Windows.Forms.Button
    Friend WithEvents MS_Last As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents MS_First As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents School As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents butPrint As System.Windows.Forms.Button
    Friend WithEvents txtTeamSearch As System.Windows.Forms.TextBox
    Friend WithEvents butTeamSearch As System.Windows.Forms.Button
    Friend WithEvents cboEntryDivisions As System.Windows.Forms.ComboBox
    Friend WithEvents chkAutoUpdate As System.Windows.Forms.CheckBox
    Friend WithEvents LASTNAME As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents FIRSTNAME As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridView2 As System.Windows.Forms.DataGridView
    Friend WithEvents grpDebatersOnTeam As System.Windows.Forms.GroupBox
    Friend WithEvents FULLNAME As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CODE As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Rating As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents StopSched As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents TubUser As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents ADA As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents lblEntryCounts As System.Windows.Forms.Label
    Friend WithEvents lblOnline As System.Windows.Forms.Label
    Friend WithEvents butSpellingChanges As System.Windows.Forms.Button
End Class
