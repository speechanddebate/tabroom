<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmResults
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
        Me.Button1 = New System.Windows.Forms.Button()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.butMakeResults = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.cboround = New System.Windows.Forms.ComboBox()
        Me.butPrint = New System.Windows.Forms.Button()
        Me.cboTieBreakSet = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtShowTop = New System.Windows.Forms.TextBox()
        Me.butOppRatings = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.butJudgeReport = New System.Windows.Forms.Button()
        Me.butElimResultsReaderSheets = New System.Windows.Forms.Button()
        Me.butHelp = New System.Windows.Forms.Button()
        Me.butShowPrefByTeam = New System.Windows.Forms.Button()
        Me.butCheckers = New System.Windows.Forms.Button()
        Me.butWinsBySchool = New System.Windows.Forms.Button()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(15, 212)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(204, 56)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "Show team results"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'DataGridView1
        '
        Me.DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Location = New System.Drawing.Point(26, 12)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.Size = New System.Drawing.Size(1012, 718)
        Me.DataGridView1.TabIndex = 1
        '
        'butMakeResults
        '
        Me.butMakeResults.Location = New System.Drawing.Point(1063, 681)
        Me.butMakeResults.Name = "butMakeResults"
        Me.butMakeResults.Size = New System.Drawing.Size(173, 29)
        Me.butMakeResults.TabIndex = 2
        Me.butMakeResults.Text = "Create Results Files"
        Me.butMakeResults.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(1060, 713)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(127, 20)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Results file status"
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(15, 274)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(197, 48)
        Me.Button2.TabIndex = 4
        Me.Button2.Text = "Show speaker results"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'cboround
        '
        Me.cboround.FormattingEnabled = True
        Me.cboround.Location = New System.Drawing.Point(15, 144)
        Me.cboround.Name = "cboround"
        Me.cboround.Size = New System.Drawing.Size(204, 28)
        Me.cboround.TabIndex = 5
        Me.cboround.Tag = ""
        Me.cboround.Text = "Select last round completed"
        '
        'butPrint
        '
        Me.butPrint.Location = New System.Drawing.Point(1150, 12)
        Me.butPrint.Name = "butPrint"
        Me.butPrint.Size = New System.Drawing.Size(102, 67)
        Me.butPrint.TabIndex = 6
        Me.butPrint.Text = "Print"
        Me.butPrint.UseVisualStyleBackColor = True
        '
        'cboTieBreakSet
        '
        Me.cboTieBreakSet.FormattingEnabled = True
        Me.cboTieBreakSet.Location = New System.Drawing.Point(15, 178)
        Me.cboTieBreakSet.Name = "cboTieBreakSet"
        Me.cboTieBreakSet.Size = New System.Drawing.Size(197, 28)
        Me.cboTieBreakSet.TabIndex = 7
        Me.cboTieBreakSet.Text = "Choose tiebreaker set"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(15, 21)
        Me.Label2.MaximumSize = New System.Drawing.Size(204, 120)
        Me.Label2.MinimumSize = New System.Drawing.Size(204, 120)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(204, 120)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "FOR RESULTS: Select the last round completed and the tiebreaker set to use (note " & _
            "that you probably want a different tiebreaker set for speakers and teams):"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(19, 329)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(73, 20)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "Show top:"
        '
        'txtShowTop
        '
        Me.txtShowTop.Location = New System.Drawing.Point(99, 329)
        Me.txtShowTop.Name = "txtShowTop"
        Me.txtShowTop.Size = New System.Drawing.Size(48, 25)
        Me.txtShowTop.TabIndex = 10
        Me.txtShowTop.Text = "50"
        '
        'butOppRatings
        '
        Me.butOppRatings.Location = New System.Drawing.Point(1044, 461)
        Me.butOppRatings.Name = "butOppRatings"
        Me.butOppRatings.Size = New System.Drawing.Size(212, 40)
        Me.butOppRatings.TabIndex = 11
        Me.butOppRatings.Text = "Show opponent draw"
        Me.butOppRatings.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Button1)
        Me.GroupBox1.Controls.Add(Me.txtShowTop)
        Me.GroupBox1.Controls.Add(Me.Button2)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.cboround)
        Me.GroupBox1.Controls.Add(Me.cboTieBreakSet)
        Me.GroupBox1.Location = New System.Drawing.Point(1044, 85)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(222, 370)
        Me.GroupBox1.TabIndex = 12
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Seeded Results"
        '
        'butJudgeReport
        '
        Me.butJudgeReport.Location = New System.Drawing.Point(1044, 508)
        Me.butJudgeReport.Name = "butJudgeReport"
        Me.butJudgeReport.Size = New System.Drawing.Size(208, 43)
        Me.butJudgeReport.TabIndex = 13
        Me.butJudgeReport.Text = "Judge Report"
        Me.butJudgeReport.UseVisualStyleBackColor = True
        '
        'butElimResultsReaderSheets
        '
        Me.butElimResultsReaderSheets.Location = New System.Drawing.Point(1044, 556)
        Me.butElimResultsReaderSheets.Name = "butElimResultsReaderSheets"
        Me.butElimResultsReaderSheets.Size = New System.Drawing.Size(212, 28)
        Me.butElimResultsReaderSheets.TabIndex = 14
        Me.butElimResultsReaderSheets.Text = "Elim results reader sheets"
        Me.butElimResultsReaderSheets.UseVisualStyleBackColor = True
        '
        'butHelp
        '
        Me.butHelp.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.butHelp.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butHelp.Location = New System.Drawing.Point(1059, 12)
        Me.butHelp.Name = "butHelp"
        Me.butHelp.Size = New System.Drawing.Size(77, 37)
        Me.butHelp.TabIndex = 15
        Me.butHelp.Text = "Help"
        Me.butHelp.UseVisualStyleBackColor = False
        '
        'butShowPrefByTeam
        '
        Me.butShowPrefByTeam.Location = New System.Drawing.Point(1044, 588)
        Me.butShowPrefByTeam.Name = "butShowPrefByTeam"
        Me.butShowPrefByTeam.Size = New System.Drawing.Size(212, 28)
        Me.butShowPrefByTeam.TabIndex = 16
        Me.butShowPrefByTeam.Text = "Pref experience by team"
        Me.butShowPrefByTeam.UseVisualStyleBackColor = True
        '
        'butCheckers
        '
        Me.butCheckers.Location = New System.Drawing.Point(1044, 620)
        Me.butCheckers.Name = "butCheckers"
        Me.butCheckers.Size = New System.Drawing.Size(212, 28)
        Me.butCheckers.TabIndex = 17
        Me.butCheckers.Text = "Checker Sheets for Round"
        Me.butCheckers.UseVisualStyleBackColor = True
        '
        'butWinsBySchool
        '
        Me.butWinsBySchool.Location = New System.Drawing.Point(1044, 652)
        Me.butWinsBySchool.Name = "butWinsBySchool"
        Me.butWinsBySchool.Size = New System.Drawing.Size(212, 28)
        Me.butWinsBySchool.TabIndex = 18
        Me.butWinsBySchool.Text = "Wins by School"
        Me.butWinsBySchool.UseVisualStyleBackColor = True
        '
        'frmResults
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1264, 742)
        Me.Controls.Add(Me.butWinsBySchool)
        Me.Controls.Add(Me.butCheckers)
        Me.Controls.Add(Me.butShowPrefByTeam)
        Me.Controls.Add(Me.butHelp)
        Me.Controls.Add(Me.butElimResultsReaderSheets)
        Me.Controls.Add(Me.butJudgeReport)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.butOppRatings)
        Me.Controls.Add(Me.butPrint)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.butMakeResults)
        Me.Controls.Add(Me.DataGridView1)
        Me.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "frmResults"
        Me.Text = "Results Processing"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents butMakeResults As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents cboround As System.Windows.Forms.ComboBox
    Friend WithEvents butPrint As System.Windows.Forms.Button
    Friend WithEvents cboTieBreakSet As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtShowTop As System.Windows.Forms.TextBox
    Friend WithEvents butOppRatings As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents butJudgeReport As System.Windows.Forms.Button
    Friend WithEvents butElimResultsReaderSheets As System.Windows.Forms.Button
    Friend WithEvents butHelp As System.Windows.Forms.Button
    Friend WithEvents butShowPrefByTeam As System.Windows.Forms.Button
    Friend WithEvents butCheckers As System.Windows.Forms.Button
    Friend WithEvents butWinsBySchool As System.Windows.Forms.Button
End Class
