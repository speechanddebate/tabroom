<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTieBreakers
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
        Me.Button1 = New System.Windows.Forms.Button
        Me.DataGridView2 = New System.Windows.Forms.DataGridView
        Me.ID = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.TBSET_Name = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.TBScoreFor = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.DataGridView3 = New System.Windows.Forms.DataGridView
        Me.Label2 = New System.Windows.Forms.Label
        Me.lblScoreInfo = New System.Windows.Forms.Label
        Me.butTBSETInfo = New System.Windows.Forms.Button
        Me.DataGridView4 = New System.Windows.Forms.DataGridView
        Me.ScoreID = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Max = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Min = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DupesOK = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.DecimalIncrements = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.butTerminology = New System.Windows.Forms.Button
        Me.butPageHelp = New System.Windows.Forms.Button
        Me.butAddDeleteHelp = New System.Windows.Forms.Button
        Me.butTiebreakerHelp = New System.Windows.Forms.Button
        Me.butTBColumnDefinitions = New System.Windows.Forms.Button
        Me.chkExcludeRanks = New System.Windows.Forms.CheckBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.butMBAShortCut = New System.Windows.Forms.Button
        Me.butHelpShortcuts = New System.Windows.Forms.Button
        Me.txtDrops = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.butRandomShortCut = New System.Windows.Forms.Button
        Me.butOppWInShortCut = New System.Windows.Forms.Button
        Me.butBallotsShortCut = New System.Windows.Forms.Button
        Me.butJudVarShortcut = New System.Windows.Forms.Button
        Me.butRanksShortCut = New System.Windows.Forms.Button
        Me.butSpkrPtsShortCut = New System.Windows.Forms.Button
        Me.butOppPtsShortCut = New System.Windows.Forms.Button
        Me.butWinsShortcut = New System.Windows.Forms.Button
        Me.butTotal1 = New System.Windows.Forms.Button
        Me.butTotal2 = New System.Windows.Forms.Button
        Me.butTotal3 = New System.Windows.Forms.Button
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.SortOrder = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Label = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Tag = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.ForOpponent = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.Drops = New System.Windows.Forms.DataGridViewTextBoxColumn
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridView3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridView4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(25, 12)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(213, 54)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "CREATE DEFAULT TIEBREAKERS"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'DataGridView2
        '
        Me.DataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.DataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView2.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ID, Me.TBSET_Name, Me.TBScoreFor})
        Me.DataGridView2.Location = New System.Drawing.Point(25, 72)
        Me.DataGridView2.MultiSelect = False
        Me.DataGridView2.Name = "DataGridView2"
        Me.DataGridView2.RowHeadersWidth = 20
        Me.DataGridView2.Size = New System.Drawing.Size(387, 254)
        Me.DataGridView2.TabIndex = 2
        '
        'ID
        '
        Me.ID.DataPropertyName = "ID"
        Me.ID.HeaderText = "ID"
        Me.ID.Name = "ID"
        Me.ID.Visible = False
        '
        'TBSET_Name
        '
        Me.TBSET_Name.DataPropertyName = "TBSET_Name"
        Me.TBSET_Name.HeaderText = "Tiebreaker Set"
        Me.TBSET_Name.Name = "TBSET_Name"
        '
        'TBScoreFor
        '
        Me.TBScoreFor.DataPropertyName = "ScoreFor"
        Me.TBScoreFor.HeaderText = "ScoreFor"
        Me.TBScoreFor.Items.AddRange(New Object() {"Team", "Speaker"})
        Me.TBScoreFor.Name = "TBScoreFor"
        Me.TBScoreFor.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.TBScoreFor.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        '
        'DataGridView3
        '
        Me.DataGridView3.AllowDrop = True
        Me.DataGridView3.AllowUserToResizeColumns = False
        Me.DataGridView3.AllowUserToResizeRows = False
        Me.DataGridView3.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.DataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView3.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumn1, Me.SortOrder, Me.Label, Me.Tag, Me.ForOpponent, Me.Drops})
        Me.DataGridView3.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke
        Me.DataGridView3.Location = New System.Drawing.Point(418, 72)
        Me.DataGridView3.Name = "DataGridView3"
        Me.DataGridView3.Size = New System.Drawing.Size(834, 254)
        Me.DataGridView3.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 553)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(430, 20)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "SCORES -- click on a box to edit and help will appear to the right."
        '
        'lblScoreInfo
        '
        Me.lblScoreInfo.AutoSize = True
        Me.lblScoreInfo.Location = New System.Drawing.Point(992, 576)
        Me.lblScoreInfo.MaximumSize = New System.Drawing.Size(240, 150)
        Me.lblScoreInfo.MinimumSize = New System.Drawing.Size(240, 150)
        Me.lblScoreInfo.Name = "lblScoreInfo"
        Me.lblScoreInfo.Size = New System.Drawing.Size(240, 150)
        Me.lblScoreInfo.TabIndex = 8
        Me.lblScoreInfo.Text = "Score settings information"
        '
        'butTBSETInfo
        '
        Me.butTBSETInfo.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.butTBSETInfo.Location = New System.Drawing.Point(25, 332)
        Me.butTBSETInfo.Name = "butTBSETInfo"
        Me.butTBSETInfo.Size = New System.Drawing.Size(213, 31)
        Me.butTBSETInfo.TabIndex = 9
        Me.butTBSETInfo.Text = "What are tiebreaker sets?"
        Me.butTBSETInfo.UseVisualStyleBackColor = False
        '
        'DataGridView4
        '
        Me.DataGridView4.AllowUserToAddRows = False
        Me.DataGridView4.AllowUserToDeleteRows = False
        Me.DataGridView4.AllowUserToResizeColumns = False
        Me.DataGridView4.AllowUserToResizeRows = False
        Me.DataGridView4.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.DataGridView4.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView4.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ScoreID, Me.Max, Me.Min, Me.DupesOK, Me.DecimalIncrements})
        Me.DataGridView4.Location = New System.Drawing.Point(12, 576)
        Me.DataGridView4.MultiSelect = False
        Me.DataGridView4.Name = "DataGridView4"
        Me.DataGridView4.RowHeadersWidth = 20
        Me.DataGridView4.Size = New System.Drawing.Size(974, 164)
        Me.DataGridView4.TabIndex = 4
        '
        'ScoreID
        '
        Me.ScoreID.DataPropertyName = "ID"
        Me.ScoreID.HeaderText = "ID"
        Me.ScoreID.Name = "ScoreID"
        Me.ScoreID.Visible = False
        '
        'Max
        '
        Me.Max.DataPropertyName = "Max"
        Me.Max.HeaderText = "Max allowed"
        Me.Max.Name = "Max"
        '
        'Min
        '
        Me.Min.DataPropertyName = "Min"
        Me.Min.HeaderText = "Min Allowed"
        Me.Min.Name = "Min"
        '
        'DupesOK
        '
        Me.DupesOK.DataPropertyName = "DupesOK"
        Me.DupesOK.HeaderText = "Duplicates OK?"
        Me.DupesOK.Name = "DupesOK"
        Me.DupesOK.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DupesOK.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        '
        'DecimalIncrements
        '
        Me.DecimalIncrements.DataPropertyName = "DecimalIncrements"
        Me.DecimalIncrements.HeaderText = "Increments allowed"
        Me.DecimalIncrements.Name = "DecimalIncrements"
        '
        'butTerminology
        '
        Me.butTerminology.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.butTerminology.Location = New System.Drawing.Point(1057, 4)
        Me.butTerminology.Name = "butTerminology"
        Me.butTerminology.Size = New System.Drawing.Size(195, 28)
        Me.butTerminology.TabIndex = 10
        Me.butTerminology.Text = "Important terminology"
        Me.butTerminology.UseVisualStyleBackColor = False
        '
        'butPageHelp
        '
        Me.butPageHelp.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.butPageHelp.Location = New System.Drawing.Point(1057, 38)
        Me.butPageHelp.Name = "butPageHelp"
        Me.butPageHelp.Size = New System.Drawing.Size(195, 28)
        Me.butPageHelp.TabIndex = 11
        Me.butPageHelp.Text = "How to use this page"
        Me.butPageHelp.UseVisualStyleBackColor = False
        '
        'butAddDeleteHelp
        '
        Me.butAddDeleteHelp.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.butAddDeleteHelp.Location = New System.Drawing.Point(25, 366)
        Me.butAddDeleteHelp.Name = "butAddDeleteHelp"
        Me.butAddDeleteHelp.Size = New System.Drawing.Size(284, 31)
        Me.butAddDeleteHelp.TabIndex = 12
        Me.butAddDeleteHelp.Text = "How to add and delete tiebreaker sets"
        Me.butAddDeleteHelp.UseVisualStyleBackColor = False
        '
        'butTiebreakerHelp
        '
        Me.butTiebreakerHelp.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.butTiebreakerHelp.Location = New System.Drawing.Point(418, 332)
        Me.butTiebreakerHelp.Name = "butTiebreakerHelp"
        Me.butTiebreakerHelp.Size = New System.Drawing.Size(298, 31)
        Me.butTiebreakerHelp.TabIndex = 13
        Me.butTiebreakerHelp.Text = "How to enter and edit tiebreakers"
        Me.butTiebreakerHelp.UseVisualStyleBackColor = False
        '
        'butTBColumnDefinitions
        '
        Me.butTBColumnDefinitions.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.butTBColumnDefinitions.Location = New System.Drawing.Point(418, 369)
        Me.butTBColumnDefinitions.Name = "butTBColumnDefinitions"
        Me.butTBColumnDefinitions.Size = New System.Drawing.Size(298, 31)
        Me.butTBColumnDefinitions.TabIndex = 14
        Me.butTBColumnDefinitions.Text = "Understanding the tiebreaker columns"
        Me.butTBColumnDefinitions.UseVisualStyleBackColor = False
        '
        'chkExcludeRanks
        '
        Me.chkExcludeRanks.AutoSize = True
        Me.chkExcludeRanks.Location = New System.Drawing.Point(245, 14)
        Me.chkExcludeRanks.Name = "chkExcludeRanks"
        Me.chkExcludeRanks.Size = New System.Drawing.Size(118, 24)
        Me.chkExcludeRanks.TabIndex = 15
        Me.chkExcludeRanks.Text = "Exclude ranks"
        Me.chkExcludeRanks.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.butTotal3)
        Me.GroupBox1.Controls.Add(Me.butTotal2)
        Me.GroupBox1.Controls.Add(Me.butTotal1)
        Me.GroupBox1.Controls.Add(Me.butMBAShortCut)
        Me.GroupBox1.Controls.Add(Me.butHelpShortcuts)
        Me.GroupBox1.Controls.Add(Me.txtDrops)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.butRandomShortCut)
        Me.GroupBox1.Controls.Add(Me.butOppWInShortCut)
        Me.GroupBox1.Controls.Add(Me.butBallotsShortCut)
        Me.GroupBox1.Controls.Add(Me.butJudVarShortcut)
        Me.GroupBox1.Controls.Add(Me.butRanksShortCut)
        Me.GroupBox1.Controls.Add(Me.butSpkrPtsShortCut)
        Me.GroupBox1.Controls.Add(Me.butOppPtsShortCut)
        Me.GroupBox1.Controls.Add(Me.butWinsShortcut)
        Me.GroupBox1.Location = New System.Drawing.Point(738, 333)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(514, 237)
        Me.GroupBox1.TabIndex = 16
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "SHORTCUTS"
        '
        'butMBAShortCut
        '
        Me.butMBAShortCut.Location = New System.Drawing.Point(298, 63)
        Me.butMBAShortCut.Name = "butMBAShortCut"
        Me.butMBAShortCut.Size = New System.Drawing.Size(140, 58)
        Me.butMBAShortCut.TabIndex = 15
        Me.butMBAShortCut.Text = "MBA HLPts + OppWins"
        Me.butMBAShortCut.UseVisualStyleBackColor = True
        '
        'butHelpShortcuts
        '
        Me.butHelpShortcuts.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.butHelpShortcuts.Location = New System.Drawing.Point(342, 12)
        Me.butHelpShortcuts.Name = "butHelpShortcuts"
        Me.butHelpShortcuts.Size = New System.Drawing.Size(166, 31)
        Me.butHelpShortcuts.TabIndex = 14
        Me.butHelpShortcuts.Text = "How to use shortcuts"
        Me.butHelpShortcuts.UseVisualStyleBackColor = False
        '
        'txtDrops
        '
        Me.txtDrops.Location = New System.Drawing.Point(156, 203)
        Me.txtDrops.Name = "txtDrops"
        Me.txtDrops.Size = New System.Drawing.Size(80, 24)
        Me.txtDrops.TabIndex = 9
        Me.txtDrops.Text = "0"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(156, 163)
        Me.Label1.MinimumSize = New System.Drawing.Size(200, 28)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(200, 28)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "Drops for points and ranks:"
        '
        'butRandomShortCut
        '
        Me.butRandomShortCut.Location = New System.Drawing.Point(152, 124)
        Me.butRandomShortCut.Name = "butRandomShortCut"
        Me.butRandomShortCut.Size = New System.Drawing.Size(140, 28)
        Me.butRandomShortCut.TabIndex = 7
        Me.butRandomShortCut.Text = "Random"
        Me.butRandomShortCut.UseVisualStyleBackColor = True
        '
        'butOppWInShortCut
        '
        Me.butOppWInShortCut.Location = New System.Drawing.Point(152, 63)
        Me.butOppWInShortCut.Name = "butOppWInShortCut"
        Me.butOppWInShortCut.Size = New System.Drawing.Size(140, 28)
        Me.butOppWInShortCut.TabIndex = 6
        Me.butOppWInShortCut.Text = "Opponent Wins"
        Me.butOppWInShortCut.UseVisualStyleBackColor = True
        '
        'butBallotsShortCut
        '
        Me.butBallotsShortCut.Location = New System.Drawing.Point(16, 93)
        Me.butBallotsShortCut.Name = "butBallotsShortCut"
        Me.butBallotsShortCut.Size = New System.Drawing.Size(130, 28)
        Me.butBallotsShortCut.TabIndex = 5
        Me.butBallotsShortCut.Text = "Ballots"
        Me.butBallotsShortCut.UseVisualStyleBackColor = True
        '
        'butJudVarShortcut
        '
        Me.butJudVarShortcut.Location = New System.Drawing.Point(16, 124)
        Me.butJudVarShortcut.Name = "butJudVarShortcut"
        Me.butJudVarShortcut.Size = New System.Drawing.Size(130, 28)
        Me.butJudVarShortcut.TabIndex = 4
        Me.butJudVarShortcut.Text = "Judge variance"
        Me.butJudVarShortcut.UseVisualStyleBackColor = True
        '
        'butRanksShortCut
        '
        Me.butRanksShortCut.Location = New System.Drawing.Point(16, 163)
        Me.butRanksShortCut.Name = "butRanksShortCut"
        Me.butRanksShortCut.Size = New System.Drawing.Size(130, 28)
        Me.butRanksShortCut.TabIndex = 3
        Me.butRanksShortCut.Text = "Ranks"
        Me.butRanksShortCut.UseVisualStyleBackColor = True
        '
        'butSpkrPtsShortCut
        '
        Me.butSpkrPtsShortCut.Location = New System.Drawing.Point(16, 198)
        Me.butSpkrPtsShortCut.Name = "butSpkrPtsShortCut"
        Me.butSpkrPtsShortCut.Size = New System.Drawing.Size(130, 28)
        Me.butSpkrPtsShortCut.TabIndex = 2
        Me.butSpkrPtsShortCut.Text = "Speaker points"
        Me.butSpkrPtsShortCut.UseVisualStyleBackColor = True
        '
        'butOppPtsShortCut
        '
        Me.butOppPtsShortCut.Location = New System.Drawing.Point(152, 93)
        Me.butOppPtsShortCut.Name = "butOppPtsShortCut"
        Me.butOppPtsShortCut.Size = New System.Drawing.Size(140, 28)
        Me.butOppPtsShortCut.TabIndex = 1
        Me.butOppPtsShortCut.Text = "Opponent points"
        Me.butOppPtsShortCut.UseVisualStyleBackColor = True
        '
        'butWinsShortcut
        '
        Me.butWinsShortcut.Location = New System.Drawing.Point(16, 63)
        Me.butWinsShortcut.Name = "butWinsShortcut"
        Me.butWinsShortcut.Size = New System.Drawing.Size(130, 28)
        Me.butWinsShortcut.TabIndex = 0
        Me.butWinsShortcut.Text = "Wins"
        Me.butWinsShortcut.UseVisualStyleBackColor = True
        '
        'butTotal1
        '
        Me.butTotal1.Location = New System.Drawing.Point(394, 128)
        Me.butTotal1.Name = "butTotal1"
        Me.butTotal1.Size = New System.Drawing.Size(114, 28)
        Me.butTotal1.TabIndex = 16
        Me.butTotal1.Text = "Total 1 ranks (WUDC)"
        Me.butTotal1.UseVisualStyleBackColor = True
        '
        'butTotal2
        '
        Me.butTotal2.Location = New System.Drawing.Point(394, 159)
        Me.butTotal2.Name = "butTotal2"
        Me.butTotal2.Size = New System.Drawing.Size(114, 28)
        Me.butTotal2.TabIndex = 17
        Me.butTotal2.Text = "Total 2 ranks (WUDC)"
        Me.butTotal2.UseVisualStyleBackColor = True
        '
        'butTotal3
        '
        Me.butTotal3.Location = New System.Drawing.Point(394, 193)
        Me.butTotal3.Name = "butTotal3"
        Me.butTotal3.Size = New System.Drawing.Size(114, 28)
        Me.butTotal3.TabIndex = 18
        Me.butTotal3.Text = "Total 3 ranks (WUDC)"
        Me.butTotal3.UseVisualStyleBackColor = True
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.DataPropertyName = "ID"
        Me.DataGridViewTextBoxColumn1.HeaderText = "ID"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.Visible = False
        Me.DataGridViewTextBoxColumn1.Width = 48
        '
        'SortOrder
        '
        Me.SortOrder.DataPropertyName = "SortOrder"
        Me.SortOrder.HeaderText = "Order"
        Me.SortOrder.Name = "SortOrder"
        Me.SortOrder.Width = 70
        '
        'Label
        '
        Me.Label.DataPropertyName = "Label"
        Me.Label.HeaderText = "Label"
        Me.Label.Name = "Label"
        Me.Label.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Label.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Label.Width = 50
        '
        'Tag
        '
        Me.Tag.DataPropertyName = "Tag"
        Me.Tag.HeaderText = "Tag"
        Me.Tag.Items.AddRange(New Object() {"None", "JudgeVariance", "Ballots", "Wins", "OppWins", "OppBallots", "Random", "MBA", "TotalRanks1", "TotalRanks2", "TotalRanks3"})
        Me.Tag.Name = "Tag"
        Me.Tag.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Tag.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.Tag.Width = 57
        '
        'ForOpponent
        '
        Me.ForOpponent.DataPropertyName = "ForOpponent"
        Me.ForOpponent.HeaderText = "For Opponent?"
        Me.ForOpponent.Name = "ForOpponent"
        Me.ForOpponent.Width = 111
        '
        'Drops
        '
        Me.Drops.DataPropertyName = "Drops"
        Me.Drops.HeaderText = "H/L Drops"
        Me.Drops.Name = "Drops"
        '
        'frmTieBreakers
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1264, 742)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.chkExcludeRanks)
        Me.Controls.Add(Me.butTBColumnDefinitions)
        Me.Controls.Add(Me.butTiebreakerHelp)
        Me.Controls.Add(Me.butAddDeleteHelp)
        Me.Controls.Add(Me.butPageHelp)
        Me.Controls.Add(Me.butTerminology)
        Me.Controls.Add(Me.butTBSETInfo)
        Me.Controls.Add(Me.lblScoreInfo)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.DataGridView4)
        Me.Controls.Add(Me.DataGridView3)
        Me.Controls.Add(Me.DataGridView2)
        Me.Controls.Add(Me.Button1)
        Me.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "frmTieBreakers"
        Me.Text = "Set up Tiebreakers"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridView3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridView4, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents DataGridView2 As System.Windows.Forms.DataGridView
    Friend WithEvents DataGridView3 As System.Windows.Forms.DataGridView
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lblScoreInfo As System.Windows.Forms.Label
    Friend WithEvents butTBSETInfo As System.Windows.Forms.Button
    Friend WithEvents ID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TBSET_Name As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TBScoreFor As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents DataGridView4 As System.Windows.Forms.DataGridView
    Friend WithEvents ScoreID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Max As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Min As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DupesOK As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents DecimalIncrements As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents butTerminology As System.Windows.Forms.Button
    Friend WithEvents butPageHelp As System.Windows.Forms.Button
    Friend WithEvents butAddDeleteHelp As System.Windows.Forms.Button
    Friend WithEvents butTiebreakerHelp As System.Windows.Forms.Button
    Friend WithEvents butTBColumnDefinitions As System.Windows.Forms.Button
    Friend WithEvents chkExcludeRanks As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents txtDrops As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents butRandomShortCut As System.Windows.Forms.Button
    Friend WithEvents butOppWInShortCut As System.Windows.Forms.Button
    Friend WithEvents butBallotsShortCut As System.Windows.Forms.Button
    Friend WithEvents butJudVarShortcut As System.Windows.Forms.Button
    Friend WithEvents butRanksShortCut As System.Windows.Forms.Button
    Friend WithEvents butSpkrPtsShortCut As System.Windows.Forms.Button
    Friend WithEvents butOppPtsShortCut As System.Windows.Forms.Button
    Friend WithEvents butWinsShortcut As System.Windows.Forms.Button
    Friend WithEvents butHelpShortcuts As System.Windows.Forms.Button
    Friend WithEvents butMBAShortCut As System.Windows.Forms.Button
    Friend WithEvents butTotal2 As System.Windows.Forms.Button
    Friend WithEvents butTotal1 As System.Windows.Forms.Button
    Friend WithEvents butTotal3 As System.Windows.Forms.Button
    Friend WithEvents DataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents SortOrder As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Label As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Tag As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents ForOpponent As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents Drops As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
