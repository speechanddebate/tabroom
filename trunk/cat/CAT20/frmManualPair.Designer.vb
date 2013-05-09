<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmManualPair
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
        Me.dgvPairing = New System.Windows.Forms.DataGridView
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.butCopyDisplay = New System.Windows.Forms.Button
        Me.butPageHelpInfo = New System.Windows.Forms.Button
        Me.chkSideConstrained = New System.Windows.Forms.CheckBox
        Me.butResetDisplay = New System.Windows.Forms.Button
        Me.butLoad = New System.Windows.Forms.Button
        Me.dgvDisplayItems = New System.Windows.Forms.DataGridView
        Me.Tag = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.SortOrder = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.cboRound = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.DataGridView1 = New System.Windows.Forms.DataGridView
        Me.DataGridView2 = New System.Windows.Forms.DataGridView
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.butDumpRound = New System.Windows.Forms.Button
        Me.butCreateBlankPairings = New System.Windows.Forms.Button
        Me.butCreatePanelWithTeam = New System.Windows.Forms.Button
        Me.butAddTeamToSelectedPanel = New System.Windows.Forms.Button
        Me.grpPairings = New System.Windows.Forms.GroupBox
        Me.butFlipSidePaired = New System.Windows.Forms.Button
        Me.butCheckRound = New System.Windows.Forms.Button
        Me.butDeleteOneDebate = New System.Windows.Forms.Button
        Me.butDeleteFromPairing = New System.Windows.Forms.Button
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.butAutoPair = New System.Windows.Forms.Button
        Me.Button1 = New System.Windows.Forms.Button
        Me.butBracketReset = New System.Windows.Forms.Button
        Me.butPairingHlep = New System.Windows.Forms.Button
        Me.butWUDCAutoPair = New System.Windows.Forms.Button
        Me.butFlipSide = New System.Windows.Forms.Button
        Me.butRandomSeeds = New System.Windows.Forms.Button
        Me.butRandomPair = New System.Windows.Forms.Button
        Me.butAssignBye = New System.Windows.Forms.Button
        Me.butAutoHighLow = New System.Windows.Forms.Button
        Me.butWUDCTest = New System.Windows.Forms.Button
        Me.butDeleteBracketDebates = New System.Windows.Forms.Button
        Me.butPairHighHigh = New System.Windows.Forms.Button
        Me.butDownBracket = New System.Windows.Forms.Button
        Me.butUpBracket = New System.Windows.Forms.Button
        Me.butMoveDown = New System.Windows.Forms.Button
        Me.butMoveUp = New System.Windows.Forms.Button
        Me.butClear = New System.Windows.Forms.Button
        Me.butShowBracket = New System.Windows.Forms.Button
        Me.txtBracket = New System.Windows.Forms.TextBox
        Me.chkPairedToBottom = New System.Windows.Forms.CheckBox
        Me.butPairTeams = New System.Windows.Forms.Button
        Me.butAutoInfo = New System.Windows.Forms.Button
        CType(Me.dgvPairing, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        CType(Me.dgvDisplayItems, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox2.SuspendLayout()
        Me.grpPairings.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.SuspendLayout()
        '
        'dgvPairing
        '
        Me.dgvPairing.AllowUserToAddRows = False
        Me.dgvPairing.AllowUserToDeleteRows = False
        Me.dgvPairing.AllowUserToResizeColumns = False
        Me.dgvPairing.AllowUserToResizeRows = False
        Me.dgvPairing.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvPairing.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvPairing.Location = New System.Drawing.Point(15, 13)
        Me.dgvPairing.Name = "dgvPairing"
        Me.dgvPairing.Size = New System.Drawing.Size(343, 558)
        Me.dgvPairing.TabIndex = 0
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.butCopyDisplay)
        Me.GroupBox1.Controls.Add(Me.butPageHelpInfo)
        Me.GroupBox1.Controls.Add(Me.chkSideConstrained)
        Me.GroupBox1.Controls.Add(Me.butResetDisplay)
        Me.GroupBox1.Controls.Add(Me.butLoad)
        Me.GroupBox1.Controls.Add(Me.dgvDisplayItems)
        Me.GroupBox1.Controls.Add(Me.cboRound)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(1035, 13)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(229, 489)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Load a Round to Pair"
        '
        'butCopyDisplay
        '
        Me.butCopyDisplay.Location = New System.Drawing.Point(87, 436)
        Me.butCopyDisplay.Name = "butCopyDisplay"
        Me.butCopyDisplay.Size = New System.Drawing.Size(136, 26)
        Me.butCopyDisplay.TabIndex = 9
        Me.butCopyDisplay.Text = "Copy Display"
        Me.butCopyDisplay.UseVisualStyleBackColor = True
        '
        'butPageHelpInfo
        '
        Me.butPageHelpInfo.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.butPageHelpInfo.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butPageHelpInfo.Location = New System.Drawing.Point(124, 17)
        Me.butPageHelpInfo.Name = "butPageHelpInfo"
        Me.butPageHelpInfo.Size = New System.Drawing.Size(97, 28)
        Me.butPageHelpInfo.TabIndex = 8
        Me.butPageHelpInfo.Text = "Help Info"
        Me.butPageHelpInfo.UseVisualStyleBackColor = False
        '
        'chkSideConstrained
        '
        Me.chkSideConstrained.AutoSize = True
        Me.chkSideConstrained.Location = New System.Drawing.Point(7, 465)
        Me.chkSideConstrained.Name = "chkSideConstrained"
        Me.chkSideConstrained.Size = New System.Drawing.Size(185, 24)
        Me.chkSideConstrained.TabIndex = 5
        Me.chkSideConstrained.Text = "Side Constrained Round"
        Me.chkSideConstrained.UseVisualStyleBackColor = True
        '
        'butResetDisplay
        '
        Me.butResetDisplay.Location = New System.Drawing.Point(87, 408)
        Me.butResetDisplay.Name = "butResetDisplay"
        Me.butResetDisplay.Size = New System.Drawing.Size(136, 26)
        Me.butResetDisplay.TabIndex = 4
        Me.butResetDisplay.Text = "Reset display settings"
        Me.butResetDisplay.UseVisualStyleBackColor = True
        '
        'butLoad
        '
        Me.butLoad.Location = New System.Drawing.Point(6, 408)
        Me.butLoad.Name = "butLoad"
        Me.butLoad.Size = New System.Drawing.Size(75, 26)
        Me.butLoad.TabIndex = 3
        Me.butLoad.Text = "Load"
        Me.butLoad.UseVisualStyleBackColor = True
        '
        'dgvDisplayItems
        '
        Me.dgvDisplayItems.AllowUserToAddRows = False
        Me.dgvDisplayItems.AllowUserToDeleteRows = False
        Me.dgvDisplayItems.AllowUserToResizeColumns = False
        Me.dgvDisplayItems.AllowUserToResizeRows = False
        Me.dgvDisplayItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvDisplayItems.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Tag, Me.SortOrder})
        Me.dgvDisplayItems.Location = New System.Drawing.Point(6, 82)
        Me.dgvDisplayItems.Name = "dgvDisplayItems"
        Me.dgvDisplayItems.RowHeadersVisible = False
        Me.dgvDisplayItems.Size = New System.Drawing.Size(217, 320)
        Me.dgvDisplayItems.TabIndex = 2
        '
        'Tag
        '
        Me.Tag.DataPropertyName = "Tag"
        Me.Tag.HeaderText = "Tag"
        Me.Tag.Name = "Tag"
        Me.Tag.ReadOnly = True
        Me.Tag.Width = 175
        '
        'SortOrder
        '
        Me.SortOrder.DataPropertyName = "SortOrder"
        Me.SortOrder.HeaderText = "Order"
        Me.SortOrder.Name = "SortOrder"
        Me.SortOrder.Width = 37
        '
        'cboRound
        '
        Me.cboRound.FormattingEnabled = True
        Me.cboRound.Location = New System.Drawing.Point(6, 48)
        Me.cboRound.Name = "cboRound"
        Me.cboRound.Size = New System.Drawing.Size(217, 28)
        Me.cboRound.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(17, 25)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(96, 20)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Select Round"
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.AllowUserToResizeColumns = False
        Me.DataGridView1.AllowUserToResizeRows = False
        Me.DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Location = New System.Drawing.Point(373, 12)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.RowHeadersVisible = False
        Me.DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridView1.Size = New System.Drawing.Size(330, 559)
        Me.DataGridView1.TabIndex = 2
        '
        'DataGridView2
        '
        Me.DataGridView2.AllowUserToAddRows = False
        Me.DataGridView2.AllowUserToDeleteRows = False
        Me.DataGridView2.AllowUserToResizeColumns = False
        Me.DataGridView2.AllowUserToResizeRows = False
        Me.DataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView2.Location = New System.Drawing.Point(709, 13)
        Me.DataGridView2.Name = "DataGridView2"
        Me.DataGridView2.RowHeadersVisible = False
        Me.DataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridView2.Size = New System.Drawing.Size(320, 558)
        Me.DataGridView2.TabIndex = 3
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Location = New System.Drawing.Point(1041, 508)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(223, 232)
        Me.GroupBox2.TabIndex = 4
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Pair Options"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(11, 20)
        Me.Label2.MaximumSize = New System.Drawing.Size(200, 200)
        Me.Label2.MinimumSize = New System.Drawing.Size(200, 200)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(200, 200)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Label2"
        '
        'butDumpRound
        '
        Me.butDumpRound.Location = New System.Drawing.Point(7, 23)
        Me.butDumpRound.Name = "butDumpRound"
        Me.butDumpRound.Size = New System.Drawing.Size(139, 29)
        Me.butDumpRound.TabIndex = 0
        Me.butDumpRound.Text = "Dump all debates"
        Me.butDumpRound.UseVisualStyleBackColor = True
        '
        'butCreateBlankPairings
        '
        Me.butCreateBlankPairings.Location = New System.Drawing.Point(6, 58)
        Me.butCreateBlankPairings.Name = "butCreateBlankPairings"
        Me.butCreateBlankPairings.Size = New System.Drawing.Size(202, 28)
        Me.butCreateBlankPairings.TabIndex = 1
        Me.butCreateBlankPairings.Text = "Create Blank Pairings"
        Me.butCreateBlankPairings.UseVisualStyleBackColor = True
        '
        'butCreatePanelWithTeam
        '
        Me.butCreatePanelWithTeam.Location = New System.Drawing.Point(6, 48)
        Me.butCreatePanelWithTeam.Name = "butCreatePanelWithTeam"
        Me.butCreatePanelWithTeam.Size = New System.Drawing.Size(202, 27)
        Me.butCreatePanelWithTeam.TabIndex = 2
        Me.butCreatePanelWithTeam.Text = "Create a new panel"
        Me.butCreatePanelWithTeam.UseVisualStyleBackColor = True
        '
        'butAddTeamToSelectedPanel
        '
        Me.butAddTeamToSelectedPanel.Location = New System.Drawing.Point(6, 75)
        Me.butAddTeamToSelectedPanel.Name = "butAddTeamToSelectedPanel"
        Me.butAddTeamToSelectedPanel.Size = New System.Drawing.Size(202, 28)
        Me.butAddTeamToSelectedPanel.TabIndex = 3
        Me.butAddTeamToSelectedPanel.Text = "Add team to selected panel"
        Me.butAddTeamToSelectedPanel.UseVisualStyleBackColor = True
        '
        'grpPairings
        '
        Me.grpPairings.Controls.Add(Me.butFlipSidePaired)
        Me.grpPairings.Controls.Add(Me.butCheckRound)
        Me.grpPairings.Controls.Add(Me.butDeleteOneDebate)
        Me.grpPairings.Controls.Add(Me.butDeleteFromPairing)
        Me.grpPairings.Controls.Add(Me.butDumpRound)
        Me.grpPairings.Controls.Add(Me.butCreateBlankPairings)
        Me.grpPairings.Location = New System.Drawing.Point(13, 578)
        Me.grpPairings.Name = "grpPairings"
        Me.grpPairings.Size = New System.Drawing.Size(345, 162)
        Me.grpPairings.TabIndex = 5
        Me.grpPairings.TabStop = False
        Me.grpPairings.Text = "Pairing Functions"
        '
        'butFlipSidePaired
        '
        Me.butFlipSidePaired.Location = New System.Drawing.Point(259, 92)
        Me.butFlipSidePaired.Name = "butFlipSidePaired"
        Me.butFlipSidePaired.Size = New System.Drawing.Size(75, 58)
        Me.butFlipSidePaired.TabIndex = 5
        Me.butFlipSidePaired.Text = "Flip Sides"
        Me.butFlipSidePaired.UseVisualStyleBackColor = True
        '
        'butCheckRound
        '
        Me.butCheckRound.Location = New System.Drawing.Point(214, 58)
        Me.butCheckRound.Name = "butCheckRound"
        Me.butCheckRound.Size = New System.Drawing.Size(125, 28)
        Me.butCheckRound.TabIndex = 4
        Me.butCheckRound.Text = "Check round"
        Me.butCheckRound.UseVisualStyleBackColor = True
        '
        'butDeleteOneDebate
        '
        Me.butDeleteOneDebate.Location = New System.Drawing.Point(166, 23)
        Me.butDeleteOneDebate.Name = "butDeleteOneDebate"
        Me.butDeleteOneDebate.Size = New System.Drawing.Size(173, 29)
        Me.butDeleteOneDebate.TabIndex = 3
        Me.butDeleteOneDebate.Text = "Delete Selected Debate"
        Me.butDeleteOneDebate.UseVisualStyleBackColor = True
        '
        'butDeleteFromPairing
        '
        Me.butDeleteFromPairing.Location = New System.Drawing.Point(6, 92)
        Me.butDeleteFromPairing.Name = "butDeleteFromPairing"
        Me.butDeleteFromPairing.Size = New System.Drawing.Size(243, 31)
        Me.butDeleteFromPairing.TabIndex = 2
        Me.butDeleteFromPairing.Text = "Delete selected team from pairing"
        Me.butDeleteFromPairing.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.butAutoInfo)
        Me.GroupBox3.Controls.Add(Me.butAutoPair)
        Me.GroupBox3.Controls.Add(Me.Button1)
        Me.GroupBox3.Controls.Add(Me.butBracketReset)
        Me.GroupBox3.Controls.Add(Me.butPairingHlep)
        Me.GroupBox3.Controls.Add(Me.butWUDCAutoPair)
        Me.GroupBox3.Controls.Add(Me.butFlipSide)
        Me.GroupBox3.Controls.Add(Me.butRandomSeeds)
        Me.GroupBox3.Controls.Add(Me.butRandomPair)
        Me.GroupBox3.Controls.Add(Me.butAssignBye)
        Me.GroupBox3.Controls.Add(Me.butAutoHighLow)
        Me.GroupBox3.Controls.Add(Me.butWUDCTest)
        Me.GroupBox3.Controls.Add(Me.butDeleteBracketDebates)
        Me.GroupBox3.Controls.Add(Me.butPairHighHigh)
        Me.GroupBox3.Controls.Add(Me.butDownBracket)
        Me.GroupBox3.Controls.Add(Me.butUpBracket)
        Me.GroupBox3.Controls.Add(Me.butMoveDown)
        Me.GroupBox3.Controls.Add(Me.butMoveUp)
        Me.GroupBox3.Controls.Add(Me.butClear)
        Me.GroupBox3.Controls.Add(Me.butShowBracket)
        Me.GroupBox3.Controls.Add(Me.txtBracket)
        Me.GroupBox3.Controls.Add(Me.chkPairedToBottom)
        Me.GroupBox3.Controls.Add(Me.butPairTeams)
        Me.GroupBox3.Controls.Add(Me.butAddTeamToSelectedPanel)
        Me.GroupBox3.Controls.Add(Me.butCreatePanelWithTeam)
        Me.GroupBox3.Location = New System.Drawing.Point(373, 578)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(656, 162)
        Me.GroupBox3.TabIndex = 6
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Team Placement"
        '
        'butAutoPair
        '
        Me.butAutoPair.Location = New System.Drawing.Point(7, 21)
        Me.butAutoPair.Name = "butAutoPair"
        Me.butAutoPair.Size = New System.Drawing.Size(100, 27)
        Me.butAutoPair.TabIndex = 22
        Me.butAutoPair.Text = "Auto Pair"
        Me.butAutoPair.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(318, 120)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(158, 28)
        Me.Button1.TabIndex = 14
        Me.Button1.Text = "WUDC whatever"
        Me.Button1.UseVisualStyleBackColor = True
        Me.Button1.Visible = False
        '
        'butBracketReset
        '
        Me.butBracketReset.Location = New System.Drawing.Point(570, 74)
        Me.butBracketReset.Name = "butBracketReset"
        Me.butBracketReset.Size = New System.Drawing.Size(74, 58)
        Me.butBracketReset.TabIndex = 20
        Me.butBracketReset.Text = "Reset Bracket"
        Me.butBracketReset.UseVisualStyleBackColor = True
        '
        'butPairingHlep
        '
        Me.butPairingHlep.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.butPairingHlep.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butPairingHlep.Location = New System.Drawing.Point(381, 104)
        Me.butPairingHlep.Name = "butPairingHlep"
        Me.butPairingHlep.Size = New System.Drawing.Size(50, 46)
        Me.butPairingHlep.TabIndex = 19
        Me.butPairingHlep.Text = "Help"
        Me.butPairingHlep.UseVisualStyleBackColor = False
        '
        'butWUDCAutoPair
        '
        Me.butWUDCAutoPair.Location = New System.Drawing.Point(318, 92)
        Me.butWUDCAutoPair.Name = "butWUDCAutoPair"
        Me.butWUDCAutoPair.Size = New System.Drawing.Size(158, 28)
        Me.butWUDCAutoPair.TabIndex = 15
        Me.butWUDCAutoPair.Text = "Auto Pair Bracket"
        Me.butWUDCAutoPair.UseVisualStyleBackColor = True
        Me.butWUDCAutoPair.Visible = False
        '
        'butFlipSide
        '
        Me.butFlipSide.Location = New System.Drawing.Point(380, 15)
        Me.butFlipSide.Name = "butFlipSide"
        Me.butFlipSide.Size = New System.Drawing.Size(51, 71)
        Me.butFlipSide.TabIndex = 18
        Me.butFlipSide.Text = "Flip Side"
        Me.butFlipSide.UseVisualStyleBackColor = True
        '
        'butRandomSeeds
        '
        Me.butRandomSeeds.Location = New System.Drawing.Point(215, 133)
        Me.butRandomSeeds.Name = "butRandomSeeds"
        Me.butRandomSeeds.Size = New System.Drawing.Size(158, 28)
        Me.butRandomSeeds.TabIndex = 17
        Me.butRandomSeeds.Text = "Randomize seeds"
        Me.butRandomSeeds.UseVisualStyleBackColor = True
        '
        'butRandomPair
        '
        Me.butRandomPair.Location = New System.Drawing.Point(318, 64)
        Me.butRandomPair.Name = "butRandomPair"
        Me.butRandomPair.Size = New System.Drawing.Size(158, 28)
        Me.butRandomPair.TabIndex = 21
        Me.butRandomPair.Text = "Random bracket pair"
        Me.butRandomPair.UseVisualStyleBackColor = True
        Me.butRandomPair.Visible = False
        '
        'butAssignBye
        '
        Me.butAssignBye.Location = New System.Drawing.Point(215, 104)
        Me.butAssignBye.Name = "butAssignBye"
        Me.butAssignBye.Size = New System.Drawing.Size(157, 28)
        Me.butAssignBye.TabIndex = 16
        Me.butAssignBye.Text = "Assign bye"
        Me.butAssignBye.UseVisualStyleBackColor = True
        '
        'butAutoHighLow
        '
        Me.butAutoHighLow.Location = New System.Drawing.Point(215, 74)
        Me.butAutoHighLow.Name = "butAutoHighLow"
        Me.butAutoHighLow.Size = New System.Drawing.Size(157, 28)
        Me.butAutoHighLow.TabIndex = 15
        Me.butAutoHighLow.Text = "Auto Pair High-Low"
        Me.butAutoHighLow.UseVisualStyleBackColor = True
        '
        'butWUDCTest
        '
        Me.butWUDCTest.Location = New System.Drawing.Point(317, 36)
        Me.butWUDCTest.Name = "butWUDCTest"
        Me.butWUDCTest.Size = New System.Drawing.Size(158, 28)
        Me.butWUDCTest.TabIndex = 14
        Me.butWUDCTest.Text = "WUDC bracket info"
        Me.butWUDCTest.UseVisualStyleBackColor = True
        Me.butWUDCTest.Visible = False
        '
        'butDeleteBracketDebates
        '
        Me.butDeleteBracketDebates.Location = New System.Drawing.Point(437, 132)
        Me.butDeleteBracketDebates.Name = "butDeleteBracketDebates"
        Me.butDeleteBracketDebates.Size = New System.Drawing.Size(207, 28)
        Me.butDeleteBracketDebates.TabIndex = 14
        Me.butDeleteBracketDebates.Text = "Dump all debates in bracket"
        Me.butDeleteBracketDebates.UseVisualStyleBackColor = True
        '
        'butPairHighHigh
        '
        Me.butPairHighHigh.Location = New System.Drawing.Point(215, 46)
        Me.butPairHighHigh.Name = "butPairHighHigh"
        Me.butPairHighHigh.Size = New System.Drawing.Size(158, 28)
        Me.butPairHighHigh.TabIndex = 13
        Me.butPairHighHigh.Text = "Auto Pair High-High"
        Me.butPairHighHigh.UseVisualStyleBackColor = True
        '
        'butDownBracket
        '
        Me.butDownBracket.Location = New System.Drawing.Point(533, 43)
        Me.butDownBracket.Name = "butDownBracket"
        Me.butDownBracket.Size = New System.Drawing.Size(111, 28)
        Me.butDownBracket.TabIndex = 12
        Me.butDownBracket.Text = "Down bracket"
        Me.butDownBracket.UseVisualStyleBackColor = True
        '
        'butUpBracket
        '
        Me.butUpBracket.Location = New System.Drawing.Point(437, 43)
        Me.butUpBracket.Name = "butUpBracket"
        Me.butUpBracket.Size = New System.Drawing.Size(90, 28)
        Me.butUpBracket.TabIndex = 11
        Me.butUpBracket.Text = "Up bracket"
        Me.butUpBracket.UseVisualStyleBackColor = True
        '
        'butMoveDown
        '
        Me.butMoveDown.Location = New System.Drawing.Point(437, 103)
        Me.butMoveDown.Name = "butMoveDown"
        Me.butMoveDown.Size = New System.Drawing.Size(133, 28)
        Me.butMoveDown.TabIndex = 10
        Me.butMoveDown.Text = "Move Team Down"
        Me.butMoveDown.UseVisualStyleBackColor = True
        '
        'butMoveUp
        '
        Me.butMoveUp.Location = New System.Drawing.Point(437, 73)
        Me.butMoveUp.Name = "butMoveUp"
        Me.butMoveUp.Size = New System.Drawing.Size(133, 28)
        Me.butMoveUp.TabIndex = 9
        Me.butMoveUp.Text = "Move Team Up"
        Me.butMoveUp.UseVisualStyleBackColor = True
        '
        'butClear
        '
        Me.butClear.Location = New System.Drawing.Point(6, 104)
        Me.butClear.Name = "butClear"
        Me.butClear.Size = New System.Drawing.Size(202, 27)
        Me.butClear.TabIndex = 8
        Me.butClear.Text = "Clear selections"
        Me.butClear.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.butClear.UseVisualStyleBackColor = True
        '
        'butShowBracket
        '
        Me.butShowBracket.Location = New System.Drawing.Point(481, 13)
        Me.butShowBracket.Name = "butShowBracket"
        Me.butShowBracket.Size = New System.Drawing.Size(160, 30)
        Me.butShowBracket.TabIndex = 7
        Me.butShowBracket.Text = "Show Bracket"
        Me.butShowBracket.UseVisualStyleBackColor = True
        '
        'txtBracket
        '
        Me.txtBracket.Location = New System.Drawing.Point(437, 15)
        Me.txtBracket.Name = "txtBracket"
        Me.txtBracket.Size = New System.Drawing.Size(38, 24)
        Me.txtBracket.TabIndex = 6
        '
        'chkPairedToBottom
        '
        Me.chkPairedToBottom.AutoSize = True
        Me.chkPairedToBottom.Location = New System.Drawing.Point(6, 132)
        Me.chkPairedToBottom.Name = "chkPairedToBottom"
        Me.chkPairedToBottom.Size = New System.Drawing.Size(181, 24)
        Me.chkPairedToBottom.TabIndex = 5
        Me.chkPairedToBottom.Text = "Paired teams to bottom"
        Me.chkPairedToBottom.UseVisualStyleBackColor = True
        '
        'butPairTeams
        '
        Me.butPairTeams.Location = New System.Drawing.Point(215, 17)
        Me.butPairTeams.Name = "butPairTeams"
        Me.butPairTeams.Size = New System.Drawing.Size(158, 28)
        Me.butPairTeams.TabIndex = 4
        Me.butPairTeams.Text = "Pair Selected Teams"
        Me.butPairTeams.UseVisualStyleBackColor = True
        '
        'butAutoInfo
        '
        Me.butAutoInfo.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.butAutoInfo.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butAutoInfo.Location = New System.Drawing.Point(110, 19)
        Me.butAutoInfo.Name = "butAutoInfo"
        Me.butAutoInfo.Size = New System.Drawing.Size(100, 27)
        Me.butAutoInfo.TabIndex = 23
        Me.butAutoInfo.Text = "Auto Info"
        Me.butAutoInfo.UseVisualStyleBackColor = False
        '
        'frmManualPair
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1276, 742)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.grpPairings)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.DataGridView2)
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.dgvPairing)
        Me.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.Name = "frmManualPair"
        Me.Text = "Pair a Round Manually"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.dgvPairing, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.dgvDisplayItems, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.grpPairings.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents dgvPairing As System.Windows.Forms.DataGridView
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents butLoad As System.Windows.Forms.Button
    Friend WithEvents dgvDisplayItems As System.Windows.Forms.DataGridView
    Friend WithEvents cboRound As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents DataGridView2 As System.Windows.Forms.DataGridView
    Friend WithEvents Tag As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents SortOrder As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents butResetDisplay As System.Windows.Forms.Button
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents butDumpRound As System.Windows.Forms.Button
    Friend WithEvents butAddTeamToSelectedPanel As System.Windows.Forms.Button
    Friend WithEvents butCreatePanelWithTeam As System.Windows.Forms.Button
    Friend WithEvents butCreateBlankPairings As System.Windows.Forms.Button
    Friend WithEvents grpPairings As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents butDeleteFromPairing As System.Windows.Forms.Button
    Friend WithEvents butDeleteOneDebate As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents chkSideConstrained As System.Windows.Forms.CheckBox
    Friend WithEvents butPairTeams As System.Windows.Forms.Button
    Friend WithEvents chkPairedToBottom As System.Windows.Forms.CheckBox
    Friend WithEvents butShowBracket As System.Windows.Forms.Button
    Friend WithEvents txtBracket As System.Windows.Forms.TextBox
    Friend WithEvents butClear As System.Windows.Forms.Button
    Friend WithEvents butMoveUp As System.Windows.Forms.Button
    Friend WithEvents butMoveDown As System.Windows.Forms.Button
    Friend WithEvents butDownBracket As System.Windows.Forms.Button
    Friend WithEvents butUpBracket As System.Windows.Forms.Button
    Friend WithEvents butPairHighHigh As System.Windows.Forms.Button
    Friend WithEvents butAutoHighLow As System.Windows.Forms.Button
    Friend WithEvents butDeleteBracketDebates As System.Windows.Forms.Button
    Friend WithEvents butAssignBye As System.Windows.Forms.Button
    Friend WithEvents butRandomSeeds As System.Windows.Forms.Button
    Friend WithEvents butFlipSide As System.Windows.Forms.Button
    Friend WithEvents butPageHelpInfo As System.Windows.Forms.Button
    Friend WithEvents butPairingHlep As System.Windows.Forms.Button
    Friend WithEvents butCopyDisplay As System.Windows.Forms.Button
    Friend WithEvents butCheckRound As System.Windows.Forms.Button
    Friend WithEvents butBracketReset As System.Windows.Forms.Button
    Friend WithEvents butFlipSidePaired As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents butWUDCAutoPair As System.Windows.Forms.Button
    Friend WithEvents butRandomPair As System.Windows.Forms.Button
    Friend WithEvents butWUDCTest As System.Windows.Forms.Button
    Friend WithEvents butAutoPair As System.Windows.Forms.Button
    Friend WithEvents butAutoInfo As System.Windows.Forms.Button
End Class
