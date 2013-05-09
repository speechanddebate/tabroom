<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmJudgeEntry
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
        Me.butSchoolSort = New System.Windows.Forms.Button
        Me.butMarkTrue = New System.Windows.Forms.Button
        Me.butMarkFalse = New System.Windows.Forms.Button
        Me.Button1 = New System.Windows.Forms.Button
        Me.DataGridView2 = New System.Windows.Forms.DataGridView
        Me.Button2 = New System.Windows.Forms.Button
        Me.butToggleTimeslots = New System.Windows.Forms.Button
        Me.butJudgeSitutionUpdate = New System.Windows.Forms.Button
        Me.butAllTSToggle = New System.Windows.Forms.Button
        Me.butAvailByRound = New System.Windows.Forms.Button
        Me.butPrint = New System.Windows.Forms.Button
        Me.butInstr = New System.Windows.Forms.Button
        Me.butInNow = New System.Windows.Forms.Button
        Me.butInLast = New System.Windows.Forms.Button
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.cboEvent = New System.Windows.Forms.ComboBox
        Me.butAvailByElimTimeSlot = New System.Windows.Forms.Button
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'DataGridView1
        '
        Me.DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Location = New System.Drawing.Point(4, 37)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridView1.Size = New System.Drawing.Size(1260, 556)
        Me.DataGridView1.TabIndex = 0
        '
        'butSchoolSort
        '
        Me.butSchoolSort.Location = New System.Drawing.Point(4, 604)
        Me.butSchoolSort.Name = "butSchoolSort"
        Me.butSchoolSort.Size = New System.Drawing.Size(119, 28)
        Me.butSchoolSort.TabIndex = 1
        Me.butSchoolSort.Text = "Sort by School"
        Me.butSchoolSort.UseVisualStyleBackColor = True
        '
        'butMarkTrue
        '
        Me.butMarkTrue.Location = New System.Drawing.Point(129, 604)
        Me.butMarkTrue.Name = "butMarkTrue"
        Me.butMarkTrue.Size = New System.Drawing.Size(119, 49)
        Me.butMarkTrue.TabIndex = 2
        Me.butMarkTrue.Text = "Mark entire column true"
        Me.butMarkTrue.UseVisualStyleBackColor = True
        '
        'butMarkFalse
        '
        Me.butMarkFalse.Location = New System.Drawing.Point(129, 659)
        Me.butMarkFalse.Name = "butMarkFalse"
        Me.butMarkFalse.Size = New System.Drawing.Size(119, 48)
        Me.butMarkFalse.TabIndex = 3
        Me.butMarkFalse.Text = "Mark entire column false"
        Me.butMarkFalse.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(4, 638)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(119, 28)
        Me.Button1.TabIndex = 4
        Me.Button1.Text = "Mark all true"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'DataGridView2
        '
        Me.DataGridView2.AllowUserToAddRows = False
        Me.DataGridView2.AllowUserToDeleteRows = False
        Me.DataGridView2.AllowUserToResizeColumns = False
        Me.DataGridView2.AllowUserToResizeRows = False
        Me.DataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.DataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView2.Location = New System.Drawing.Point(546, 599)
        Me.DataGridView2.Name = "DataGridView2"
        Me.DataGridView2.ReadOnly = True
        Me.DataGridView2.RowHeadersVisible = False
        Me.DataGridView2.Size = New System.Drawing.Size(718, 119)
        Me.DataGridView2.TabIndex = 5
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(4, 673)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(119, 34)
        Me.Button2.TabIndex = 6
        Me.Button2.Text = "delete"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'butToggleTimeslots
        '
        Me.butToggleTimeslots.Location = New System.Drawing.Point(178, 8)
        Me.butToggleTimeslots.Name = "butToggleTimeslots"
        Me.butToggleTimeslots.Size = New System.Drawing.Size(193, 28)
        Me.butToggleTimeslots.TabIndex = 7
        Me.butToggleTimeslots.Text = "Show Elim Timeslots"
        Me.butToggleTimeslots.UseVisualStyleBackColor = True
        '
        'butJudgeSitutionUpdate
        '
        Me.butJudgeSitutionUpdate.Location = New System.Drawing.Point(877, 2)
        Me.butJudgeSitutionUpdate.Name = "butJudgeSitutionUpdate"
        Me.butJudgeSitutionUpdate.Size = New System.Drawing.Size(235, 31)
        Me.butJudgeSitutionUpdate.TabIndex = 8
        Me.butJudgeSitutionUpdate.Text = "Update judging situation"
        Me.butJudgeSitutionUpdate.UseVisualStyleBackColor = True
        Me.butJudgeSitutionUpdate.Visible = False
        '
        'butAllTSToggle
        '
        Me.butAllTSToggle.Location = New System.Drawing.Point(378, 8)
        Me.butAllTSToggle.Name = "butAllTSToggle"
        Me.butAllTSToggle.Size = New System.Drawing.Size(123, 28)
        Me.butAllTSToggle.TabIndex = 9
        Me.butAllTSToggle.Text = "Hide Timeslots"
        Me.butAllTSToggle.UseVisualStyleBackColor = True
        '
        'butAvailByRound
        '
        Me.butAvailByRound.Location = New System.Drawing.Point(451, 599)
        Me.butAvailByRound.Name = "butAvailByRound"
        Me.butAvailByRound.Size = New System.Drawing.Size(89, 76)
        Me.butAvailByRound.TabIndex = 10
        Me.butAvailByRound.Text = "Show availability by round"
        Me.butAvailByRound.UseVisualStyleBackColor = True
        '
        'butPrint
        '
        Me.butPrint.Location = New System.Drawing.Point(1118, 3)
        Me.butPrint.Name = "butPrint"
        Me.butPrint.Size = New System.Drawing.Size(75, 28)
        Me.butPrint.TabIndex = 11
        Me.butPrint.Text = "Print"
        Me.butPrint.UseVisualStyleBackColor = True
        '
        'butInstr
        '
        Me.butInstr.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.butInstr.Location = New System.Drawing.Point(1199, 3)
        Me.butInstr.Name = "butInstr"
        Me.butInstr.Size = New System.Drawing.Size(65, 28)
        Me.butInstr.TabIndex = 12
        Me.butInstr.Text = "Help"
        Me.butInstr.UseVisualStyleBackColor = False
        '
        'butInNow
        '
        Me.butInNow.Location = New System.Drawing.Point(7, 49)
        Me.butInNow.Name = "butInNow"
        Me.butInNow.Size = New System.Drawing.Size(178, 27)
        Me.butInNow.TabIndex = 13
        Me.butInNow.Text = "If teams in current round"
        Me.butInNow.UseVisualStyleBackColor = True
        '
        'butInLast
        '
        Me.butInLast.Location = New System.Drawing.Point(7, 78)
        Me.butInLast.Name = "butInLast"
        Me.butInLast.Size = New System.Drawing.Size(178, 27)
        Me.butInLast.TabIndex = 14
        Me.butInLast.Text = "If teams in last round"
        Me.butInLast.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.cboEvent)
        Me.GroupBox1.Controls.Add(Me.butInNow)
        Me.GroupBox1.Controls.Add(Me.butInLast)
        Me.GroupBox1.Location = New System.Drawing.Point(254, 599)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(191, 108)
        Me.GroupBox1.TabIndex = 15
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Mark Elim Availability"
        '
        'cboEvent
        '
        Me.cboEvent.FormattingEnabled = True
        Me.cboEvent.Location = New System.Drawing.Point(7, 20)
        Me.cboEvent.Name = "cboEvent"
        Me.cboEvent.Size = New System.Drawing.Size(178, 28)
        Me.cboEvent.TabIndex = 15
        '
        'butAvailByElimTimeSlot
        '
        Me.butAvailByElimTimeSlot.Location = New System.Drawing.Point(618, 5)
        Me.butAvailByElimTimeSlot.Name = "butAvailByElimTimeSlot"
        Me.butAvailByElimTimeSlot.Size = New System.Drawing.Size(202, 30)
        Me.butAvailByElimTimeSlot.TabIndex = 16
        Me.butAvailByElimTimeSlot.Text = "# of judges for elim round"
        Me.butAvailByElimTimeSlot.UseVisualStyleBackColor = True
        '
        'frmJudgeEntry
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1264, 720)
        Me.Controls.Add(Me.butAvailByElimTimeSlot)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.butInstr)
        Me.Controls.Add(Me.butPrint)
        Me.Controls.Add(Me.butAvailByRound)
        Me.Controls.Add(Me.butAllTSToggle)
        Me.Controls.Add(Me.butJudgeSitutionUpdate)
        Me.Controls.Add(Me.butToggleTimeslots)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.DataGridView2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.butMarkFalse)
        Me.Controls.Add(Me.butMarkTrue)
        Me.Controls.Add(Me.butSchoolSort)
        Me.Controls.Add(Me.DataGridView1)
        Me.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.MaximumSize = New System.Drawing.Size(1280, 758)
        Me.MinimumSize = New System.Drawing.Size(1278, 758)
        Me.Name = "frmJudgeEntry"
        Me.Text = "Manage Judge Entries"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents butSchoolSort As System.Windows.Forms.Button
    Friend WithEvents butMarkTrue As System.Windows.Forms.Button
    Friend WithEvents butMarkFalse As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents DataGridView2 As System.Windows.Forms.DataGridView
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents butToggleTimeslots As System.Windows.Forms.Button
    Friend WithEvents butJudgeSitutionUpdate As System.Windows.Forms.Button
    Friend WithEvents butAllTSToggle As System.Windows.Forms.Button
    Friend WithEvents butAvailByRound As System.Windows.Forms.Button
    Friend WithEvents butPrint As System.Windows.Forms.Button
    Friend WithEvents butInstr As System.Windows.Forms.Button
    Friend WithEvents butInNow As System.Windows.Forms.Button
    Friend WithEvents butInLast As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents cboEvent As System.Windows.Forms.ComboBox
    Friend WithEvents butAvailByElimTimeSlot As System.Windows.Forms.Button
End Class
