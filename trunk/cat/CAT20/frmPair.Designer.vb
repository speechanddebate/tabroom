<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPair
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
        Me.Button2 = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.DataGridView1 = New System.Windows.Forms.DataGridView
        Me.Button3 = New System.Windows.Forms.Button
        Me.butHackPrelimResults = New System.Windows.Forms.Button
        Me.ListBox1 = New System.Windows.Forms.ListBox
        Me.butElims = New System.Windows.Forms.Button
        Me.Button5 = New System.Windows.Forms.Button
        Me.Button4 = New System.Windows.Forms.Button
        Me.cboRound = New System.Windows.Forms.ComboBox
        Me.chkThisRoundOnly = New System.Windows.Forms.CheckBox
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(29, 22)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(116, 113)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "Hack up random prelim pairings for all divisions"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(547, 17)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(107, 57)
        Me.Button2.TabIndex = 1
        Me.Button2.Text = "Show the round"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(279, 86)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(53, 20)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Label1"
        '
        'DataGridView1
        '
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Location = New System.Drawing.Point(58, 149)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.Size = New System.Drawing.Size(1194, 395)
        Me.DataGridView1.TabIndex = 3
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(660, 77)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(107, 58)
        Me.Button3.TabIndex = 4
        Me.Button3.Text = "test retrieval methods"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'butHackPrelimResults
        '
        Me.butHackPrelimResults.Location = New System.Drawing.Point(154, 21)
        Me.butHackPrelimResults.Name = "butHackPrelimResults"
        Me.butHackPrelimResults.Size = New System.Drawing.Size(107, 48)
        Me.butHackPrelimResults.TabIndex = 5
        Me.butHackPrelimResults.Text = "Hack up prelim results"
        Me.butHackPrelimResults.UseVisualStyleBackColor = True
        '
        'ListBox1
        '
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.ItemHeight = 20
        Me.ListBox1.Location = New System.Drawing.Point(787, 22)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(465, 84)
        Me.ListBox1.TabIndex = 7
        '
        'butElims
        '
        Me.butElims.Location = New System.Drawing.Point(154, 86)
        Me.butElims.Name = "butElims"
        Me.butElims.Size = New System.Drawing.Size(107, 49)
        Me.butElims.TabIndex = 8
        Me.butElims.Text = "Do the elims"
        Me.butElims.UseVisualStyleBackColor = True
        '
        'Button5
        '
        Me.Button5.Location = New System.Drawing.Point(661, 21)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(106, 33)
        Me.Button5.TabIndex = 9
        Me.Button5.Text = "LINQ"
        Me.Button5.UseVisualStyleBackColor = True
        '
        'Button4
        '
        Me.Button4.Location = New System.Drawing.Point(547, 80)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(107, 55)
        Me.Button4.TabIndex = 10
        Me.Button4.Text = "Reset tiebreakers"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'cboRound
        '
        Me.cboRound.FormattingEnabled = True
        Me.cboRound.Location = New System.Drawing.Point(268, 21)
        Me.cboRound.Name = "cboRound"
        Me.cboRound.Size = New System.Drawing.Size(273, 28)
        Me.cboRound.TabIndex = 11
        '
        'chkThisRoundOnly
        '
        Me.chkThisRoundOnly.AutoSize = True
        Me.chkThisRoundOnly.Location = New System.Drawing.Point(268, 51)
        Me.chkThisRoundOnly.Name = "chkThisRoundOnly"
        Me.chkThisRoundOnly.Size = New System.Drawing.Size(198, 24)
        Me.chkThisRoundOnly.TabIndex = 12
        Me.chkThisRoundOnly.Text = "Results for this round only"
        Me.chkThisRoundOnly.UseVisualStyleBackColor = True
        '
        'frmPair
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1264, 742)
        Me.Controls.Add(Me.chkThisRoundOnly)
        Me.Controls.Add(Me.cboRound)
        Me.Controls.Add(Me.Button4)
        Me.Controls.Add(Me.Button5)
        Me.Controls.Add(Me.butElims)
        Me.Controls.Add(Me.ListBox1)
        Me.Controls.Add(Me.butHackPrelimResults)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "frmPair"
        Me.Text = "Pair Rounds"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents butHackPrelimResults As System.Windows.Forms.Button
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents butElims As System.Windows.Forms.Button
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents cboRound As System.Windows.Forms.ComboBox
    Friend WithEvents chkThisRoundOnly As System.Windows.Forms.CheckBox
End Class
