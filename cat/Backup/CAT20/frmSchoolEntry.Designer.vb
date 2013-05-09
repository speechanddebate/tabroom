<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSchoolEntry
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
        Me.ID = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.SchoolName = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Code = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Region = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.butLimitToEntered = New System.Windows.Forms.Button
        Me.butRemakeAcros = New System.Windows.Forms.Button
        Me.Button1 = New System.Windows.Forms.Button
        Me.butPrint = New System.Windows.Forms.Button
        Me.butHelp = New System.Windows.Forms.Button
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToResizeColumns = False
        Me.DataGridView1.AllowUserToResizeRows = False
        Me.DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ID, Me.SchoolName, Me.Code, Me.Region})
        Me.DataGridView1.Location = New System.Drawing.Point(13, 13)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.Size = New System.Drawing.Size(669, 717)
        Me.DataGridView1.TabIndex = 0
        '
        'ID
        '
        Me.ID.DataPropertyName = "ID"
        Me.ID.HeaderText = "ID"
        Me.ID.Name = "ID"
        Me.ID.Visible = False
        '
        'SchoolName
        '
        Me.SchoolName.DataPropertyName = "SchoolName"
        Me.SchoolName.HeaderText = "School"
        Me.SchoolName.Name = "SchoolName"
        '
        'Code
        '
        Me.Code.DataPropertyName = "Code"
        Me.Code.HeaderText = "Short Name (12 characters)"
        Me.Code.MaxInputLength = 12
        Me.Code.Name = "Code"
        '
        'Region
        '
        Me.Region.DataPropertyName = "Region"
        Me.Region.HeaderText = "Region"
        Me.Region.Name = "Region"
        '
        'butLimitToEntered
        '
        Me.butLimitToEntered.Location = New System.Drawing.Point(689, 13)
        Me.butLimitToEntered.Name = "butLimitToEntered"
        Me.butLimitToEntered.Size = New System.Drawing.Size(168, 62)
        Me.butLimitToEntered.TabIndex = 1
        Me.butLimitToEntered.Text = "Limit list to schools currently entered"
        Me.butLimitToEntered.UseVisualStyleBackColor = True
        '
        'butRemakeAcros
        '
        Me.butRemakeAcros.Location = New System.Drawing.Point(689, 82)
        Me.butRemakeAcros.Name = "butRemakeAcros"
        Me.butRemakeAcros.Size = New System.Drawing.Size(168, 86)
        Me.butRemakeAcros.TabIndex = 2
        Me.butRemakeAcros.Text = "Re-make all acronyms based on short names entered here"
        Me.butRemakeAcros.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(689, 174)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(168, 88)
        Me.Button1.TabIndex = 3
        Me.Button1.Text = "Re-make all full team names based on the school names entered here"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'butPrint
        '
        Me.butPrint.Location = New System.Drawing.Point(1157, 57)
        Me.butPrint.Name = "butPrint"
        Me.butPrint.Size = New System.Drawing.Size(95, 45)
        Me.butPrint.TabIndex = 4
        Me.butPrint.Text = "Print"
        Me.butPrint.UseVisualStyleBackColor = True
        '
        'butHelp
        '
        Me.butHelp.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.butHelp.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butHelp.Location = New System.Drawing.Point(1157, 12)
        Me.butHelp.Name = "butHelp"
        Me.butHelp.Size = New System.Drawing.Size(95, 39)
        Me.butHelp.TabIndex = 14
        Me.butHelp.Text = "Help"
        Me.butHelp.UseVisualStyleBackColor = False
        '
        'frmSchoolEntry
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1264, 742)
        Me.Controls.Add(Me.butHelp)
        Me.Controls.Add(Me.butPrint)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.butRemakeAcros)
        Me.Controls.Add(Me.butLimitToEntered)
        Me.Controls.Add(Me.DataGridView1)
        Me.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "frmSchoolEntry"
        Me.Text = "frmSchoolEntry"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents ID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents SchoolName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Code As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Region As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents butLimitToEntered As System.Windows.Forms.Button
    Friend WithEvents butRemakeAcros As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents butPrint As System.Windows.Forms.Button
    Friend WithEvents butHelp As System.Windows.Forms.Button
End Class
