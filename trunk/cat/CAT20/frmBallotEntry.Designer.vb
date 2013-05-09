<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBallotEntry
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
        Me.cboRound = New System.Windows.Forms.ComboBox
        Me.butLoad = New System.Windows.Forms.Button
        Me.DataGridView1 = New System.Windows.Forms.DataGridView
        Me.Label1 = New System.Windows.Forms.Label
        Me.DataGridView2 = New System.Windows.Forms.DataGridView
        Me.ID = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Ballot = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Recipient = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Side = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.RecipientName = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Score_ID = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ScoreName = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Score = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.grpDecision = New System.Windows.Forms.GroupBox
        Me.butByeInfo = New System.Windows.Forms.Button
        Me.radDoubLoss = New System.Windows.Forms.RadioButton
        Me.radDoubWin = New System.Windows.Forms.RadioButton
        Me.chkClearJudge = New System.Windows.Forms.CheckBox
        Me.chkLPW = New System.Windows.Forms.CheckBox
        Me.lblDecision = New System.Windows.Forms.Label
        Me.radTeam2 = New System.Windows.Forms.RadioButton
        Me.radTeam1 = New System.Windows.Forms.RadioButton
        Me.butClear = New System.Windows.Forms.Button
        Me.butSave = New System.Windows.Forms.Button
        Me.lblSaved = New System.Windows.Forms.Label
        Me.chkAutoComplete = New System.Windows.Forms.CheckBox
        Me.butWebDownload = New System.Windows.Forms.Button
        Me.Button1 = New System.Windows.Forms.Button
        Me.butEraseDecision = New System.Windows.Forms.Button
        Me.chkSortEntered = New System.Windows.Forms.CheckBox
        Me.Button2 = New System.Windows.Forms.Button
        Me.but3PersonHelp = New System.Windows.Forms.Button
        Me.butHelp = New System.Windows.Forms.Button
        Me.butFlipSides = New System.Windows.Forms.Button
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpDecision.SuspendLayout()
        Me.SuspendLayout()
        '
        'cboRound
        '
        Me.cboRound.FormattingEnabled = True
        Me.cboRound.Location = New System.Drawing.Point(13, 13)
        Me.cboRound.Name = "cboRound"
        Me.cboRound.Size = New System.Drawing.Size(200, 28)
        Me.cboRound.TabIndex = 0
        Me.cboRound.Text = "Select Round"
        '
        'butLoad
        '
        Me.butLoad.Location = New System.Drawing.Point(219, 13)
        Me.butLoad.Name = "butLoad"
        Me.butLoad.Size = New System.Drawing.Size(75, 28)
        Me.butLoad.TabIndex = 1
        Me.butLoad.Text = "Load"
        Me.butLoad.UseVisualStyleBackColor = True
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.AllowUserToResizeColumns = False
        Me.DataGridView1.AllowUserToResizeRows = False
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Location = New System.Drawing.Point(13, 47)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.ReadOnly = True
        Me.DataGridView1.Size = New System.Drawing.Size(650, 670)
        Me.DataGridView1.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(433, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(53, 20)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Label1"
        '
        'DataGridView2
        '
        Me.DataGridView2.AllowUserToAddRows = False
        Me.DataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.DataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView2.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ID, Me.Ballot, Me.Recipient, Me.Side, Me.RecipientName, Me.Score_ID, Me.ScoreName, Me.Score})
        Me.DataGridView2.Location = New System.Drawing.Point(790, 47)
        Me.DataGridView2.Name = "DataGridView2"
        Me.DataGridView2.RowHeadersVisible = False
        Me.DataGridView2.Size = New System.Drawing.Size(466, 368)
        Me.DataGridView2.TabIndex = 4
        '
        'ID
        '
        Me.ID.DataPropertyName = "ID"
        Me.ID.HeaderText = "ID"
        Me.ID.Name = "ID"
        Me.ID.Visible = False
        '
        'Ballot
        '
        Me.Ballot.DataPropertyName = "Ballot"
        Me.Ballot.HeaderText = "Ballot"
        Me.Ballot.Name = "Ballot"
        Me.Ballot.Visible = False
        '
        'Recipient
        '
        Me.Recipient.DataPropertyName = "Recipient"
        Me.Recipient.HeaderText = "Recipient"
        Me.Recipient.Name = "Recipient"
        Me.Recipient.Visible = False
        '
        'Side
        '
        Me.Side.DataPropertyName = "Side"
        Me.Side.HeaderText = "Side"
        Me.Side.Name = "Side"
        Me.Side.Visible = False
        '
        'RecipientName
        '
        Me.RecipientName.DataPropertyName = "RecipientName"
        Me.RecipientName.HeaderText = "Name"
        Me.RecipientName.Name = "RecipientName"
        Me.RecipientName.ReadOnly = True
        '
        'Score_ID
        '
        Me.Score_ID.DataPropertyName = "Score_ID"
        Me.Score_ID.HeaderText = "Score_ID"
        Me.Score_ID.Name = "Score_ID"
        Me.Score_ID.Visible = False
        '
        'ScoreName
        '
        Me.ScoreName.DataPropertyName = "ScoreName"
        Me.ScoreName.HeaderText = "Score Name"
        Me.ScoreName.Name = "ScoreName"
        Me.ScoreName.ReadOnly = True
        '
        'Score
        '
        Me.Score.DataPropertyName = "Score"
        Me.Score.HeaderText = "Score"
        Me.Score.Name = "Score"
        '
        'grpDecision
        '
        Me.grpDecision.Controls.Add(Me.butByeInfo)
        Me.grpDecision.Controls.Add(Me.radDoubLoss)
        Me.grpDecision.Controls.Add(Me.radDoubWin)
        Me.grpDecision.Controls.Add(Me.chkClearJudge)
        Me.grpDecision.Controls.Add(Me.chkLPW)
        Me.grpDecision.Controls.Add(Me.lblDecision)
        Me.grpDecision.Controls.Add(Me.radTeam2)
        Me.grpDecision.Controls.Add(Me.radTeam1)
        Me.grpDecision.Location = New System.Drawing.Point(786, 422)
        Me.grpDecision.Name = "grpDecision"
        Me.grpDecision.Size = New System.Drawing.Size(466, 186)
        Me.grpDecision.TabIndex = 6
        Me.grpDecision.TabStop = False
        Me.grpDecision.Text = "Decision"
        '
        'butByeInfo
        '
        Me.butByeInfo.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.butByeInfo.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butByeInfo.Location = New System.Drawing.Point(338, 73)
        Me.butByeInfo.Name = "butByeInfo"
        Me.butByeInfo.Size = New System.Drawing.Size(122, 49)
        Me.butByeInfo.TabIndex = 8
        Me.butByeInfo.Text = "How to assign byes"
        Me.butByeInfo.UseVisualStyleBackColor = False
        '
        'radDoubLoss
        '
        Me.radDoubLoss.AutoSize = True
        Me.radDoubLoss.Location = New System.Drawing.Point(26, 88)
        Me.radDoubLoss.Name = "radDoubLoss"
        Me.radDoubLoss.Size = New System.Drawing.Size(106, 24)
        Me.radDoubLoss.TabIndex = 7
        Me.radDoubLoss.TabStop = True
        Me.radDoubLoss.Text = "Double Loss"
        Me.radDoubLoss.UseVisualStyleBackColor = True
        '
        'radDoubWin
        '
        Me.radDoubWin.AutoSize = True
        Me.radDoubWin.Location = New System.Drawing.Point(26, 66)
        Me.radDoubWin.Name = "radDoubWin"
        Me.radDoubWin.Size = New System.Drawing.Size(99, 24)
        Me.radDoubWin.TabIndex = 6
        Me.radDoubWin.TabStop = True
        Me.radDoubWin.Text = "Double win"
        Me.radDoubWin.UseVisualStyleBackColor = True
        '
        'chkClearJudge
        '
        Me.chkClearJudge.AutoSize = True
        Me.chkClearJudge.Location = New System.Drawing.Point(338, 23)
        Me.chkClearJudge.Name = "chkClearJudge"
        Me.chkClearJudge.Size = New System.Drawing.Size(104, 24)
        Me.chkClearJudge.TabIndex = 5
        Me.chkClearJudge.Text = "Clear Judge"
        Me.chkClearJudge.UseVisualStyleBackColor = True
        '
        'chkLPW
        '
        Me.chkLPW.AutoSize = True
        Me.chkLPW.Location = New System.Drawing.Point(340, 49)
        Me.chkLPW.Name = "chkLPW"
        Me.chkLPW.Size = New System.Drawing.Size(120, 24)
        Me.chkLPW.TabIndex = 3
        Me.chkLPW.Text = "Low Point Win"
        Me.chkLPW.UseVisualStyleBackColor = True
        '
        'lblDecision
        '
        Me.lblDecision.AutoSize = True
        Me.lblDecision.Location = New System.Drawing.Point(22, 115)
        Me.lblDecision.MaximumSize = New System.Drawing.Size(420, 60)
        Me.lblDecision.MinimumSize = New System.Drawing.Size(420, 60)
        Me.lblDecision.Name = "lblDecision"
        Me.lblDecision.Size = New System.Drawing.Size(420, 60)
        Me.lblDecision.TabIndex = 2
        Me.lblDecision.Text = "No decision entered"
        '
        'radTeam2
        '
        Me.radTeam2.AutoSize = True
        Me.radTeam2.Location = New System.Drawing.Point(26, 43)
        Me.radTeam2.Name = "radTeam2"
        Me.radTeam2.Size = New System.Drawing.Size(117, 24)
        Me.radTeam2.TabIndex = 1
        Me.radTeam2.TabStop = True
        Me.radTeam2.Text = "RadioButton2"
        Me.radTeam2.UseVisualStyleBackColor = True
        '
        'radTeam1
        '
        Me.radTeam1.AutoSize = True
        Me.radTeam1.Location = New System.Drawing.Point(26, 20)
        Me.radTeam1.Name = "radTeam1"
        Me.radTeam1.Size = New System.Drawing.Size(117, 24)
        Me.radTeam1.TabIndex = 0
        Me.radTeam1.TabStop = True
        Me.radTeam1.Text = "RadioButton1"
        Me.radTeam1.UseVisualStyleBackColor = True
        '
        'butClear
        '
        Me.butClear.Location = New System.Drawing.Point(928, 614)
        Me.butClear.Name = "butClear"
        Me.butClear.Size = New System.Drawing.Size(143, 28)
        Me.butClear.TabIndex = 8
        Me.butClear.Text = "Clear Entry Screen"
        Me.butClear.UseVisualStyleBackColor = True
        '
        'butSave
        '
        Me.butSave.Location = New System.Drawing.Point(786, 614)
        Me.butSave.Name = "butSave"
        Me.butSave.Size = New System.Drawing.Size(126, 28)
        Me.butSave.TabIndex = 7
        Me.butSave.Text = "Save decision"
        Me.butSave.UseVisualStyleBackColor = True
        '
        'lblSaved
        '
        Me.lblSaved.AutoSize = True
        Me.lblSaved.Location = New System.Drawing.Point(786, 649)
        Me.lblSaved.Name = "lblSaved"
        Me.lblSaved.Size = New System.Drawing.Size(0, 20)
        Me.lblSaved.TabIndex = 9
        '
        'chkAutoComplete
        '
        Me.chkAutoComplete.AutoSize = True
        Me.chkAutoComplete.Location = New System.Drawing.Point(790, 15)
        Me.chkAutoComplete.Name = "chkAutoComplete"
        Me.chkAutoComplete.Size = New System.Drawing.Size(222, 24)
        Me.chkAutoComplete.TabIndex = 10
        Me.chkAutoComplete.Text = "Autocomplete decimal entries"
        Me.chkAutoComplete.UseVisualStyleBackColor = True
        '
        'butWebDownload
        '
        Me.butWebDownload.Location = New System.Drawing.Point(1077, 614)
        Me.butWebDownload.Name = "butWebDownload"
        Me.butWebDownload.Size = New System.Drawing.Size(175, 41)
        Me.butWebDownload.TabIndex = 11
        Me.butWebDownload.Text = "Download web ballots"
        Me.butWebDownload.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(695, 425)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(64, 87)
        Me.Button1.TabIndex = 12
        Me.Button1.Text = "DownLoad Test"
        Me.Button1.UseVisualStyleBackColor = True
        Me.Button1.Visible = False
        '
        'butEraseDecision
        '
        Me.butEraseDecision.Location = New System.Drawing.Point(928, 649)
        Me.butEraseDecision.Name = "butEraseDecision"
        Me.butEraseDecision.Size = New System.Drawing.Size(143, 50)
        Me.butEraseDecision.TabIndex = 13
        Me.butEraseDecision.Text = "Clear/erase decision"
        Me.butEraseDecision.UseVisualStyleBackColor = True
        '
        'chkSortEntered
        '
        Me.chkSortEntered.AutoSize = True
        Me.chkSortEntered.Location = New System.Drawing.Point(300, 16)
        Me.chkSortEntered.Name = "chkSortEntered"
        Me.chkSortEntered.Size = New System.Drawing.Size(127, 24)
        Me.chkSortEntered.TabIndex = 14
        Me.chkSortEntered.Text = "Sort by entered"
        Me.chkSortEntered.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(1077, 661)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(175, 56)
        Me.Button2.TabIndex = 15
        Me.Button2.Text = "Load and display elim ballots"
        Me.Button2.UseVisualStyleBackColor = True
        Me.Button2.Visible = False
        '
        'but3PersonHelp
        '
        Me.but3PersonHelp.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.but3PersonHelp.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.but3PersonHelp.Location = New System.Drawing.Point(1018, 9)
        Me.but3PersonHelp.Name = "but3PersonHelp"
        Me.but3PersonHelp.Size = New System.Drawing.Size(122, 32)
        Me.but3PersonHelp.TabIndex = 16
        Me.but3PersonHelp.Text = "3-person teams"
        Me.but3PersonHelp.UseVisualStyleBackColor = False
        '
        'butHelp
        '
        Me.butHelp.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.butHelp.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butHelp.Location = New System.Drawing.Point(1188, 8)
        Me.butHelp.Name = "butHelp"
        Me.butHelp.Size = New System.Drawing.Size(58, 32)
        Me.butHelp.TabIndex = 17
        Me.butHelp.Text = "Help"
        Me.butHelp.UseVisualStyleBackColor = False
        '
        'butFlipSides
        '
        Me.butFlipSides.Location = New System.Drawing.Point(786, 672)
        Me.butFlipSides.Name = "butFlipSides"
        Me.butFlipSides.Size = New System.Drawing.Size(126, 30)
        Me.butFlipSides.TabIndex = 9
        Me.butFlipSides.Text = "Flip Sides"
        Me.butFlipSides.UseVisualStyleBackColor = True
        '
        'frmBallotEntry
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1264, 720)
        Me.Controls.Add(Me.butFlipSides)
        Me.Controls.Add(Me.butHelp)
        Me.Controls.Add(Me.but3PersonHelp)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.chkSortEntered)
        Me.Controls.Add(Me.butEraseDecision)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.butWebDownload)
        Me.Controls.Add(Me.chkAutoComplete)
        Me.Controls.Add(Me.lblSaved)
        Me.Controls.Add(Me.butSave)
        Me.Controls.Add(Me.butClear)
        Me.Controls.Add(Me.grpDecision)
        Me.Controls.Add(Me.DataGridView2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.butLoad)
        Me.Controls.Add(Me.cboRound)
        Me.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.MaximumSize = New System.Drawing.Size(1280, 780)
        Me.MinimumSize = New System.Drawing.Size(1278, 758)
        Me.Name = "frmBallotEntry"
        Me.Text = "Ballot Entry"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpDecision.ResumeLayout(False)
        Me.grpDecision.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cboRound As System.Windows.Forms.ComboBox
    Friend WithEvents butLoad As System.Windows.Forms.Button
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents DataGridView2 As System.Windows.Forms.DataGridView
    Friend WithEvents grpDecision As System.Windows.Forms.GroupBox
    Friend WithEvents lblDecision As System.Windows.Forms.Label
    Friend WithEvents radTeam2 As System.Windows.Forms.RadioButton
    Friend WithEvents radTeam1 As System.Windows.Forms.RadioButton
    Friend WithEvents butClear As System.Windows.Forms.Button
    Friend WithEvents butSave As System.Windows.Forms.Button
    Friend WithEvents chkLPW As System.Windows.Forms.CheckBox
    Friend WithEvents ID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Ballot As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Recipient As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Side As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents RecipientName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Score_ID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ScoreName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Score As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents chkClearJudge As System.Windows.Forms.CheckBox
    Friend WithEvents radDoubLoss As System.Windows.Forms.RadioButton
    Friend WithEvents radDoubWin As System.Windows.Forms.RadioButton
    Friend WithEvents butByeInfo As System.Windows.Forms.Button
    Friend WithEvents lblSaved As System.Windows.Forms.Label
    Friend WithEvents chkAutoComplete As System.Windows.Forms.CheckBox
    Friend WithEvents butWebDownload As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents butEraseDecision As System.Windows.Forms.Button
    Friend WithEvents chkSortEntered As System.Windows.Forms.CheckBox
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents but3PersonHelp As System.Windows.Forms.Button
    Friend WithEvents butHelp As System.Windows.Forms.Button
    Friend WithEvents butFlipSides As System.Windows.Forms.Button
End Class
