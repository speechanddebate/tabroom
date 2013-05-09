<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEnterDivisions
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
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.EVENTID = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.TAG = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.VALUE = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridView2 = New System.Windows.Forms.DataGridView
        Me.ID = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.EVENTNAME = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ABBR = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.lblInfo = New System.Windows.Forms.Label
        Me.butHelp = New System.Windows.Forms.Button
        Me.butAddDeleteHelp = New System.Windows.Forms.Button
        Me.butCreateSettings = New System.Windows.Forms.Button
        Me.butResetSettings = New System.Windows.Forms.Button
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.AllowUserToResizeColumns = False
        Me.DataGridView1.AllowUserToResizeRows = False
        Me.DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumn1, Me.EVENTID, Me.TAG, Me.VALUE})
        Me.DataGridView1.Location = New System.Drawing.Point(512, 69)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.RowHeadersVisible = False
        Me.DataGridView1.Size = New System.Drawing.Size(436, 377)
        Me.DataGridView1.TabIndex = 1
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.DataPropertyName = "ID"
        Me.DataGridViewTextBoxColumn1.HeaderText = "ID"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.Visible = False
        '
        'EVENTID
        '
        Me.EVENTID.DataPropertyName = "EVENT"
        Me.EVENTID.HeaderText = "EVENT"
        Me.EVENTID.Name = "EVENTID"
        Me.EVENTID.Visible = False
        '
        'TAG
        '
        Me.TAG.DataPropertyName = "TAG"
        Me.TAG.HeaderText = "Setting"
        Me.TAG.Name = "TAG"
        Me.TAG.ReadOnly = True
        '
        'VALUE
        '
        Me.VALUE.DataPropertyName = "Value"
        Me.VALUE.HeaderText = "VALUE"
        Me.VALUE.Name = "VALUE"
        '
        'DataGridView2
        '
        Me.DataGridView2.AllowUserToResizeColumns = False
        Me.DataGridView2.AllowUserToResizeRows = False
        Me.DataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.DataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView2.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ID, Me.EVENTNAME, Me.ABBR})
        Me.DataGridView2.Location = New System.Drawing.Point(36, 69)
        Me.DataGridView2.Name = "DataGridView2"
        Me.DataGridView2.RowHeadersWidth = 20
        Me.DataGridView2.Size = New System.Drawing.Size(452, 279)
        Me.DataGridView2.TabIndex = 2
        '
        'ID
        '
        Me.ID.DataPropertyName = "ID"
        Me.ID.HeaderText = "ID"
        Me.ID.Name = "ID"
        Me.ID.Visible = False
        '
        'EVENTNAME
        '
        Me.EVENTNAME.DataPropertyName = "EventName"
        Me.EVENTNAME.HeaderText = "Event Name"
        Me.EVENTNAME.Name = "EVENTNAME"
        '
        'ABBR
        '
        Me.ABBR.DataPropertyName = "Abbr"
        Me.ABBR.HeaderText = "Short Name"
        Me.ABBR.Name = "ABBR"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(38, 43)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(185, 22)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Current events/divisions"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(520, 44)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(218, 22)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Settings for selected division"
        '
        'lblInfo
        '
        Me.lblInfo.AutoSize = True
        Me.lblInfo.Location = New System.Drawing.Point(963, 69)
        Me.lblInfo.MaximumSize = New System.Drawing.Size(275, 500)
        Me.lblInfo.MinimumSize = New System.Drawing.Size(275, 500)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(275, 500)
        Me.lblInfo.TabIndex = 5
        Me.lblInfo.Text = "Information appears here..."
        '
        'butHelp
        '
        Me.butHelp.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.butHelp.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butHelp.Location = New System.Drawing.Point(1072, 6)
        Me.butHelp.Name = "butHelp"
        Me.butHelp.Size = New System.Drawing.Size(187, 39)
        Me.butHelp.TabIndex = 9
        Me.butHelp.Text = "How to use this page"
        Me.butHelp.UseVisualStyleBackColor = False
        '
        'butAddDeleteHelp
        '
        Me.butAddDeleteHelp.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.butAddDeleteHelp.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butAddDeleteHelp.Location = New System.Drawing.Point(229, 40)
        Me.butAddDeleteHelp.Name = "butAddDeleteHelp"
        Me.butAddDeleteHelp.Size = New System.Drawing.Size(259, 28)
        Me.butAddDeleteHelp.TabIndex = 10
        Me.butAddDeleteHelp.Text = "How to add and delete divisions"
        Me.butAddDeleteHelp.UseVisualStyleBackColor = False
        '
        'butCreateSettings
        '
        Me.butCreateSettings.Location = New System.Drawing.Point(36, 363)
        Me.butCreateSettings.Name = "butCreateSettings"
        Me.butCreateSettings.Size = New System.Drawing.Size(293, 28)
        Me.butCreateSettings.TabIndex = 11
        Me.butCreateSettings.Text = "Create Settings for New Division"
        Me.butCreateSettings.UseVisualStyleBackColor = True
        '
        'butResetSettings
        '
        Me.butResetSettings.Location = New System.Drawing.Point(36, 398)
        Me.butResetSettings.Name = "butResetSettings"
        Me.butResetSettings.Size = New System.Drawing.Size(293, 60)
        Me.butResetSettings.TabIndex = 12
        Me.butResetSettings.Text = "Reset settings for division by debate type"
        Me.butResetSettings.UseVisualStyleBackColor = True
        '
        'frmEnterDivisions
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 22.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1264, 762)
        Me.Controls.Add(Me.butResetSettings)
        Me.Controls.Add(Me.butCreateSettings)
        Me.Controls.Add(Me.butAddDeleteHelp)
        Me.Controls.Add(Me.butHelp)
        Me.Controls.Add(Me.lblInfo)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.DataGridView2)
        Me.Controls.Add(Me.DataGridView1)
        Me.Font = New System.Drawing.Font("Trebuchet MS", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4, 6, 4, 6)
        Me.Name = "frmEnterDivisions"
        Me.Text = "Set Up Divisions"
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents DataGridView2 As System.Windows.Forms.DataGridView
    Friend WithEvents ID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents EVENTNAME As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ABBR As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents EVENTID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TAG As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents VALUE As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents butHelp As System.Windows.Forms.Button
    Friend WithEvents butAddDeleteHelp As System.Windows.Forms.Button
    Friend WithEvents butCreateSettings As System.Windows.Forms.Button
    Friend WithEvents butResetSettings As System.Windows.Forms.Button
End Class
