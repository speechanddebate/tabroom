<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmElims
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
        Me.cboDivisions = New System.Windows.Forms.ComboBox
        Me.butLoad = New System.Windows.Forms.Button
        Me.DataGridView1 = New System.Windows.Forms.DataGridView
        Me.DataGridView2 = New System.Windows.Forms.DataGridView
        Me.butFillElim = New System.Windows.Forms.Button
        Me.lblSides = New System.Windows.Forms.Label
        Me.butFlipSides = New System.Windows.Forms.Button
        Me.lblAdvancing = New System.Windows.Forms.Label
        Me.DataGridView3 = New System.Windows.Forms.DataGridView
        Me.lblwinner = New System.Windows.Forms.Label
        Me.butDelete = New System.Windows.Forms.Button
        Me.butPair = New System.Windows.Forms.Button
        Me.butHelp = New System.Windows.Forms.Button
        Me.butAdvanceWODebate = New System.Windows.Forms.Button
        Me.butWebBallots = New System.Windows.Forms.Button
        Me.butClearDecision = New System.Windows.Forms.Button
        Me.lblRoom = New System.Windows.Forms.Label
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridView3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cboDivisions
        '
        Me.cboDivisions.FormattingEnabled = True
        Me.cboDivisions.Location = New System.Drawing.Point(12, 13)
        Me.cboDivisions.Name = "cboDivisions"
        Me.cboDivisions.Size = New System.Drawing.Size(155, 28)
        Me.cboDivisions.TabIndex = 0
        Me.cboDivisions.Text = "Select Event"
        '
        'butLoad
        '
        Me.butLoad.Location = New System.Drawing.Point(173, 12)
        Me.butLoad.Name = "butLoad"
        Me.butLoad.Size = New System.Drawing.Size(98, 28)
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
        Me.DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.DataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Location = New System.Drawing.Point(13, 61)
        Me.DataGridView1.MultiSelect = False
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.ReadOnly = True
        Me.DataGridView1.Size = New System.Drawing.Size(876, 669)
        Me.DataGridView1.TabIndex = 2
        '
        'DataGridView2
        '
        Me.DataGridView2.AllowUserToAddRows = False
        Me.DataGridView2.AllowUserToDeleteRows = False
        Me.DataGridView2.AllowUserToResizeColumns = False
        Me.DataGridView2.AllowUserToResizeRows = False
        Me.DataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.DataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView2.Location = New System.Drawing.Point(895, 61)
        Me.DataGridView2.Name = "DataGridView2"
        Me.DataGridView2.RowHeadersVisible = False
        Me.DataGridView2.Size = New System.Drawing.Size(357, 138)
        Me.DataGridView2.TabIndex = 3
        '
        'butFillElim
        '
        Me.butFillElim.Location = New System.Drawing.Point(297, 7)
        Me.butFillElim.Name = "butFillElim"
        Me.butFillElim.Size = New System.Drawing.Size(128, 48)
        Me.butFillElim.TabIndex = 4
        Me.butFillElim.Text = "Reset and fill first elim"
        Me.butFillElim.UseVisualStyleBackColor = True
        '
        'lblSides
        '
        Me.lblSides.AutoSize = True
        Me.lblSides.Location = New System.Drawing.Point(896, 206)
        Me.lblSides.MaximumSize = New System.Drawing.Size(230, 50)
        Me.lblSides.MinimumSize = New System.Drawing.Size(230, 50)
        Me.lblSides.Name = "lblSides"
        Me.lblSides.Size = New System.Drawing.Size(230, 50)
        Me.lblSides.TabIndex = 5
        Me.lblSides.Text = "Side indicator"
        '
        'butFlipSides
        '
        Me.butFlipSides.Location = New System.Drawing.Point(1132, 200)
        Me.butFlipSides.Name = "butFlipSides"
        Me.butFlipSides.Size = New System.Drawing.Size(120, 28)
        Me.butFlipSides.TabIndex = 6
        Me.butFlipSides.Text = "Flip sides"
        Me.butFlipSides.UseVisualStyleBackColor = True
        '
        'lblAdvancing
        '
        Me.lblAdvancing.AutoSize = True
        Me.lblAdvancing.Location = New System.Drawing.Point(896, 308)
        Me.lblAdvancing.MaximumSize = New System.Drawing.Size(230, 50)
        Me.lblAdvancing.MinimumSize = New System.Drawing.Size(230, 50)
        Me.lblAdvancing.Name = "lblAdvancing"
        Me.lblAdvancing.Size = New System.Drawing.Size(230, 50)
        Me.lblAdvancing.TabIndex = 7
        Me.lblAdvancing.Text = "Teams advancing"
        '
        'DataGridView3
        '
        Me.DataGridView3.AllowUserToAddRows = False
        Me.DataGridView3.AllowUserToDeleteRows = False
        Me.DataGridView3.AllowUserToResizeColumns = False
        Me.DataGridView3.AllowUserToResizeRows = False
        Me.DataGridView3.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.DataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView3.Location = New System.Drawing.Point(896, 361)
        Me.DataGridView3.Name = "DataGridView3"
        Me.DataGridView3.RowHeadersVisible = False
        Me.DataGridView3.Size = New System.Drawing.Size(357, 369)
        Me.DataGridView3.TabIndex = 8
        '
        'lblwinner
        '
        Me.lblwinner.AutoSize = True
        Me.lblwinner.ForeColor = System.Drawing.Color.Red
        Me.lblwinner.Location = New System.Drawing.Point(895, 263)
        Me.lblwinner.MaximumSize = New System.Drawing.Size(357, 45)
        Me.lblwinner.MinimumSize = New System.Drawing.Size(357, 45)
        Me.lblwinner.Name = "lblwinner"
        Me.lblwinner.Size = New System.Drawing.Size(357, 45)
        Me.lblwinner.TabIndex = 9
        Me.lblwinner.Text = "Winner"
        '
        'butDelete
        '
        Me.butDelete.Location = New System.Drawing.Point(481, 7)
        Me.butDelete.Name = "butDelete"
        Me.butDelete.Size = New System.Drawing.Size(207, 48)
        Me.butDelete.TabIndex = 10
        Me.butDelete.Text = "Delete Debates in selected column"
        Me.butDelete.UseVisualStyleBackColor = True
        '
        'butPair
        '
        Me.butPair.Location = New System.Drawing.Point(1132, 294)
        Me.butPair.Name = "butPair"
        Me.butPair.Size = New System.Drawing.Size(121, 61)
        Me.butPair.TabIndex = 11
        Me.butPair.Text = "Pair"
        Me.butPair.UseVisualStyleBackColor = True
        '
        'butHelp
        '
        Me.butHelp.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.butHelp.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butHelp.Location = New System.Drawing.Point(1200, 7)
        Me.butHelp.Name = "butHelp"
        Me.butHelp.Size = New System.Drawing.Size(52, 39)
        Me.butHelp.TabIndex = 12
        Me.butHelp.Text = "Help"
        Me.butHelp.UseVisualStyleBackColor = False
        '
        'butAdvanceWODebate
        '
        Me.butAdvanceWODebate.Location = New System.Drawing.Point(1133, 228)
        Me.butAdvanceWODebate.Name = "butAdvanceWODebate"
        Me.butAdvanceWODebate.Size = New System.Drawing.Size(120, 28)
        Me.butAdvanceWODebate.TabIndex = 13
        Me.butAdvanceWODebate.Text = "Advance w/bye"
        Me.butAdvanceWODebate.UseVisualStyleBackColor = True
        '
        'butWebBallots
        '
        Me.butWebBallots.Location = New System.Drawing.Point(771, 7)
        Me.butWebBallots.Name = "butWebBallots"
        Me.butWebBallots.Size = New System.Drawing.Size(118, 48)
        Me.butWebBallots.TabIndex = 14
        Me.butWebBallots.Text = "Download Web Ballots"
        Me.butWebBallots.UseVisualStyleBackColor = True
        '
        'butClearDecision
        '
        Me.butClearDecision.Location = New System.Drawing.Point(895, 7)
        Me.butClearDecision.Name = "butClearDecision"
        Me.butClearDecision.Size = New System.Drawing.Size(85, 48)
        Me.butClearDecision.TabIndex = 15
        Me.butClearDecision.Text = "Clear Decision"
        Me.butClearDecision.UseVisualStyleBackColor = True
        '
        'lblRoom
        '
        Me.lblRoom.AutoSize = True
        Me.lblRoom.Location = New System.Drawing.Point(986, 9)
        Me.lblRoom.MaximumSize = New System.Drawing.Size(200, 40)
        Me.lblRoom.MinimumSize = New System.Drawing.Size(200, 40)
        Me.lblRoom.Name = "lblRoom"
        Me.lblRoom.Size = New System.Drawing.Size(200, 40)
        Me.lblRoom.TabIndex = 16
        Me.lblRoom.Text = "Room"
        '
        'frmElims
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1264, 742)
        Me.Controls.Add(Me.lblRoom)
        Me.Controls.Add(Me.butClearDecision)
        Me.Controls.Add(Me.butWebBallots)
        Me.Controls.Add(Me.butAdvanceWODebate)
        Me.Controls.Add(Me.butHelp)
        Me.Controls.Add(Me.butPair)
        Me.Controls.Add(Me.butDelete)
        Me.Controls.Add(Me.lblwinner)
        Me.Controls.Add(Me.DataGridView3)
        Me.Controls.Add(Me.lblAdvancing)
        Me.Controls.Add(Me.butFlipSides)
        Me.Controls.Add(Me.lblSides)
        Me.Controls.Add(Me.butFillElim)
        Me.Controls.Add(Me.DataGridView2)
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.butLoad)
        Me.Controls.Add(Me.cboDivisions)
        Me.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "frmElims"
        Me.Text = "Elimination Rounds"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridView3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cboDivisions As System.Windows.Forms.ComboBox
    Friend WithEvents butLoad As System.Windows.Forms.Button
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents DataGridView2 As System.Windows.Forms.DataGridView
    Friend WithEvents butFillElim As System.Windows.Forms.Button
    Friend WithEvents lblSides As System.Windows.Forms.Label
    Friend WithEvents butFlipSides As System.Windows.Forms.Button
    Friend WithEvents lblAdvancing As System.Windows.Forms.Label
    Friend WithEvents DataGridView3 As System.Windows.Forms.DataGridView
    Friend WithEvents lblwinner As System.Windows.Forms.Label
    Friend WithEvents butDelete As System.Windows.Forms.Button
    Friend WithEvents butPair As System.Windows.Forms.Button
    Friend WithEvents butHelp As System.Windows.Forms.Button
    Friend WithEvents butAdvanceWODebate As System.Windows.Forms.Button
    Friend WithEvents butWebBallots As System.Windows.Forms.Button
    Friend WithEvents butClearDecision As System.Windows.Forms.Button
    Friend WithEvents lblRoom As System.Windows.Forms.Label
End Class
