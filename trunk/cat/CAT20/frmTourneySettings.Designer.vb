<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTourneySettings
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
        Me.TournName = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.StartDate = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.EndDate = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridView2 = New System.Windows.Forms.DataGridView
        Me.Tag = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Value = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.dgvDetails = New System.Windows.Forms.DataGridView
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.lblNameAndDates = New System.Windows.Forms.Label
        Me.lblSettings = New System.Windows.Forms.Label
        Me.dgvTimeSlots = New System.Windows.Forms.DataGridView
        Me.Label1 = New System.Windows.Forms.Label
        Me.butHelp = New System.Windows.Forms.Button
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.butTimeSlotHelp = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.butMakeTimeSlots = New System.Windows.Forms.Button
        Me.ID = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.SlotName = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.stTime = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.EndTime = New System.Windows.Forms.DataGridViewTextBoxColumn
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvDetails, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvTimeSlots, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.SuspendLayout()
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.DataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.TournName, Me.StartDate, Me.EndDate})
        Me.DataGridView1.Location = New System.Drawing.Point(10, 55)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.RowHeadersVisible = False
        Me.DataGridView1.Size = New System.Drawing.Size(790, 54)
        Me.DataGridView1.TabIndex = 0
        Me.DataGridView1.Tag = "Tourn"
        '
        'TournName
        '
        Me.TournName.DataPropertyName = "TournName"
        Me.TournName.HeaderText = "Tourney Name"
        Me.TournName.Name = "TournName"
        Me.TournName.Width = 125
        '
        'StartDate
        '
        Me.StartDate.DataPropertyName = "StartDate"
        Me.StartDate.HeaderText = "Start Date"
        Me.StartDate.Name = "StartDate"
        Me.StartDate.Width = 101
        '
        'EndDate
        '
        Me.EndDate.DataPropertyName = "EndDate"
        Me.EndDate.HeaderText = "End Date"
        Me.EndDate.Name = "EndDate"
        Me.EndDate.Width = 93
        '
        'DataGridView2
        '
        Me.DataGridView2.AllowUserToAddRows = False
        Me.DataGridView2.AllowUserToDeleteRows = False
        Me.DataGridView2.AllowUserToResizeRows = False
        Me.DataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.DataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView2.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Tag, Me.Value})
        Me.DataGridView2.Location = New System.Drawing.Point(10, 56)
        Me.DataGridView2.MultiSelect = False
        Me.DataGridView2.Name = "DataGridView2"
        Me.DataGridView2.ReadOnly = True
        Me.DataGridView2.RowHeadersVisible = False
        Me.DataGridView2.Size = New System.Drawing.Size(350, 305)
        Me.DataGridView2.TabIndex = 1
        '
        'Tag
        '
        Me.Tag.DataPropertyName = "TAG"
        Me.Tag.HeaderText = "Setting"
        Me.Tag.Name = "Tag"
        Me.Tag.ReadOnly = True
        '
        'Value
        '
        Me.Value.DataPropertyName = "VALUE"
        Me.Value.HeaderText = "Current Value"
        Me.Value.Name = "Value"
        Me.Value.ReadOnly = True
        '
        'dgvDetails
        '
        Me.dgvDetails.AllowUserToAddRows = False
        Me.dgvDetails.AllowUserToResizeRows = False
        Me.dgvDetails.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvDetails.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1})
        Me.dgvDetails.Location = New System.Drawing.Point(366, 56)
        Me.dgvDetails.MultiSelect = False
        Me.dgvDetails.Name = "dgvDetails"
        Me.dgvDetails.ReadOnly = True
        Me.dgvDetails.RowHeadersVisible = False
        Me.dgvDetails.Size = New System.Drawing.Size(269, 305)
        Me.dgvDetails.TabIndex = 2
        '
        'Column1
        '
        Me.Column1.DataPropertyName = "Value"
        Me.Column1.HeaderText = "Select New Setting Value"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        '
        'lblNameAndDates
        '
        Me.lblNameAndDates.AutoSize = True
        Me.lblNameAndDates.Location = New System.Drawing.Point(6, 25)
        Me.lblNameAndDates.Name = "lblNameAndDates"
        Me.lblNameAndDates.Size = New System.Drawing.Size(825, 20)
        Me.lblNameAndDates.TabIndex = 3
        Me.lblNameAndDates.Text = "To change the name or tournament dates, enter them below.  Remember to hit return" & _
            " or tab when you are done with each cell."
        '
        'lblSettings
        '
        Me.lblSettings.AutoSize = True
        Me.lblSettings.Location = New System.Drawing.Point(6, 25)
        Me.lblSettings.Name = "lblSettings"
        Me.lblSettings.Size = New System.Drawing.Size(966, 20)
        Me.lblSettings.TabIndex = 4
        Me.lblSettings.Text = "The left box shows your current settings.  To change them, click on the desired s" & _
            "etting in the left box and select the new value in the right-hand box."
        '
        'dgvTimeSlots
        '
        Me.dgvTimeSlots.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvTimeSlots.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvTimeSlots.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ID, Me.SlotName, Me.stTime, Me.EndTime})
        Me.dgvTimeSlots.Location = New System.Drawing.Point(11, 23)
        Me.dgvTimeSlots.Name = "dgvTimeSlots"
        Me.dgvTimeSlots.Size = New System.Drawing.Size(838, 144)
        Me.dgvTimeSlots.TabIndex = 6
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(651, 56)
        Me.Label1.MaximumSize = New System.Drawing.Size(500, 305)
        Me.Label1.MinimumSize = New System.Drawing.Size(500, 305)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(500, 305)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "Help appears here...."
        '
        'butHelp
        '
        Me.butHelp.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.butHelp.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butHelp.Location = New System.Drawing.Point(1065, 12)
        Me.butHelp.Name = "butHelp"
        Me.butHelp.Size = New System.Drawing.Size(187, 39)
        Me.butHelp.TabIndex = 8
        Me.butHelp.Text = "How to use this page"
        Me.butHelp.UseVisualStyleBackColor = False
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.lblNameAndDates)
        Me.GroupBox1.Controls.Add(Me.DataGridView1)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(966, 143)
        Me.GroupBox1.TabIndex = 9
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "GROUP ONE:  Tournament names and dates"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.lblSettings)
        Me.GroupBox2.Controls.Add(Me.DataGridView2)
        Me.GroupBox2.Controls.Add(Me.dgvDetails)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Location = New System.Drawing.Point(12, 161)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(1240, 396)
        Me.GroupBox2.TabIndex = 10
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "GROUP TWO: Tournament settings"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.butTimeSlotHelp)
        Me.GroupBox3.Controls.Add(Me.Label2)
        Me.GroupBox3.Controls.Add(Me.butMakeTimeSlots)
        Me.GroupBox3.Controls.Add(Me.dgvTimeSlots)
        Me.GroupBox3.Location = New System.Drawing.Point(12, 563)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(1240, 175)
        Me.GroupBox3.TabIndex = 11
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "GROUP THREE: Time slots"
        '
        'butTimeSlotHelp
        '
        Me.butTimeSlotHelp.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.butTimeSlotHelp.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butTimeSlotHelp.Location = New System.Drawing.Point(1071, 18)
        Me.butTimeSlotHelp.Name = "butTimeSlotHelp"
        Me.butTimeSlotHelp.Size = New System.Drawing.Size(155, 39)
        Me.butTimeSlotHelp.TabIndex = 9
        Me.butTimeSlotHelp.Text = "Help on timeslots"
        Me.butTimeSlotHelp.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(913, 64)
        Me.Label2.MaximumSize = New System.Drawing.Size(300, 100)
        Me.Label2.MinimumSize = New System.Drawing.Size(300, 100)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(300, 100)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Enter a date followed by a standard time format, for example, ""3/31/1969 10AM"""
        '
        'butMakeTimeSlots
        '
        Me.butMakeTimeSlots.Location = New System.Drawing.Point(913, 23)
        Me.butMakeTimeSlots.Name = "butMakeTimeSlots"
        Me.butMakeTimeSlots.Size = New System.Drawing.Size(152, 34)
        Me.butMakeTimeSlots.TabIndex = 7
        Me.butMakeTimeSlots.Text = "Generate Time Slots"
        Me.butMakeTimeSlots.UseVisualStyleBackColor = True
        '
        'ID
        '
        Me.ID.DataPropertyName = "ID"
        Me.ID.HeaderText = "ID"
        Me.ID.Name = "ID"
        Me.ID.Visible = False
        '
        'SlotName
        '
        Me.SlotName.DataPropertyName = "TimeSlotName"
        Me.SlotName.HeaderText = "Slot Name"
        Me.SlotName.Name = "SlotName"
        '
        'stTime
        '
        Me.stTime.DataPropertyName = "Start"
        Me.stTime.HeaderText = "Start Time"
        Me.stTime.Name = "stTime"
        '
        'EndTime
        '
        Me.EndTime.DataPropertyName = "End"
        Me.EndTime.HeaderText = "End Time"
        Me.EndTime.Name = "EndTime"
        '
        'frmTourneySettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1264, 742)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.butHelp)
        Me.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "frmTourneySettings"
        Me.Text = "Enter Tournament-Wide Settings"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvDetails, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvTimeSlots, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents TournName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents StartDate As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents EndDate As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridView2 As System.Windows.Forms.DataGridView
    Friend WithEvents dgvDetails As System.Windows.Forms.DataGridView
    Friend WithEvents lblNameAndDates As System.Windows.Forms.Label
    Friend WithEvents lblSettings As System.Windows.Forms.Label
    Friend WithEvents Tag As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Value As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents dgvTimeSlots As System.Windows.Forms.DataGridView
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents butHelp As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents butMakeTimeSlots As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents butTimeSlotHelp As System.Windows.Forms.Button
    Friend WithEvents ID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents SlotName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents stTime As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents EndTime As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
