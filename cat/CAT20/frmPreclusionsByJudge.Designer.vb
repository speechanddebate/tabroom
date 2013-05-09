<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPreclusionsByJudge
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
        Me.FullName = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Button1 = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.DataGridView2 = New System.Windows.Forms.DataGridView
        Me.butDeletePreclusion = New System.Windows.Forms.Button
        Me.txtSearch = New System.Windows.Forms.TextBox
        Me.lblJudge = New System.Windows.Forms.Label
        Me.butSearch = New System.Windows.Forms.Button
        Me.cboJudge = New System.Windows.Forms.ComboBox
        Me.butLoadJudge = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.butHelp = New System.Windows.Forms.Button
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
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ID, Me.FullName})
        Me.DataGridView1.Location = New System.Drawing.Point(13, 37)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.ReadOnly = True
        Me.DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridView1.Size = New System.Drawing.Size(470, 671)
        Me.DataGridView1.TabIndex = 0
        '
        'ID
        '
        Me.ID.DataPropertyName = "ID"
        Me.ID.HeaderText = "ID"
        Me.ID.Name = "ID"
        Me.ID.ReadOnly = True
        Me.ID.Visible = False
        '
        'FullName
        '
        Me.FullName.DataPropertyName = "FullName"
        Me.FullName.HeaderText = "Team Name"
        Me.FullName.Name = "FullName"
        Me.FullName.ReadOnly = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(503, 93)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(101, 160)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "Add selected team to preclusion list"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(116, 20)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "List of All Teams"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(610, 68)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(156, 20)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Current Preclusion List"
        '
        'DataGridView2
        '
        Me.DataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.DataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView2.Location = New System.Drawing.Point(614, 92)
        Me.DataGridView2.Name = "DataGridView2"
        Me.DataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridView2.Size = New System.Drawing.Size(362, 616)
        Me.DataGridView2.TabIndex = 4
        '
        'butDeletePreclusion
        '
        Me.butDeletePreclusion.Location = New System.Drawing.Point(982, 90)
        Me.butDeletePreclusion.Name = "butDeletePreclusion"
        Me.butDeletePreclusion.Size = New System.Drawing.Size(117, 145)
        Me.butDeletePreclusion.TabIndex = 5
        Me.butDeletePreclusion.Text = "Remove team from preclusion list"
        Me.butDeletePreclusion.UseVisualStyleBackColor = True
        '
        'txtSearch
        '
        Me.txtSearch.Location = New System.Drawing.Point(160, 7)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(213, 24)
        Me.txtSearch.TabIndex = 6
        '
        'lblJudge
        '
        Me.lblJudge.AutoSize = True
        Me.lblJudge.Location = New System.Drawing.Point(874, 11)
        Me.lblJudge.Name = "lblJudge"
        Me.lblJudge.Size = New System.Drawing.Size(147, 20)
        Me.lblJudge.TabIndex = 7
        Me.lblJudge.Text = "Current judge marker"
        '
        'butSearch
        '
        Me.butSearch.Location = New System.Drawing.Point(393, 7)
        Me.butSearch.Name = "butSearch"
        Me.butSearch.Size = New System.Drawing.Size(75, 28)
        Me.butSearch.TabIndex = 8
        Me.butSearch.Text = "Search"
        Me.butSearch.UseVisualStyleBackColor = True
        '
        'cboJudge
        '
        Me.cboJudge.FormattingEnabled = True
        Me.cboJudge.Location = New System.Drawing.Point(619, 11)
        Me.cboJudge.Name = "cboJudge"
        Me.cboJudge.Size = New System.Drawing.Size(121, 28)
        Me.cboJudge.TabIndex = 9
        Me.cboJudge.Text = "Select Judge"
        '
        'butLoadJudge
        '
        Me.butLoadJudge.Location = New System.Drawing.Point(747, 7)
        Me.butLoadJudge.Name = "butLoadJudge"
        Me.butLoadJudge.Size = New System.Drawing.Size(116, 28)
        Me.butLoadJudge.TabIndex = 10
        Me.butLoadJudge.Text = "Load Judge"
        Me.butLoadJudge.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(1121, 68)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(102, 74)
        Me.Button2.TabIndex = 11
        Me.Button2.Text = "Save all changes so far"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'butHelp
        '
        Me.butHelp.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.butHelp.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butHelp.Location = New System.Drawing.Point(1155, 11)
        Me.butHelp.Name = "butHelp"
        Me.butHelp.Size = New System.Drawing.Size(95, 39)
        Me.butHelp.TabIndex = 13
        Me.butHelp.Text = "Help"
        Me.butHelp.UseVisualStyleBackColor = False
        '
        'frmPreclusionsByJudge
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1262, 720)
        Me.Controls.Add(Me.butHelp)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.butLoadJudge)
        Me.Controls.Add(Me.cboJudge)
        Me.Controls.Add(Me.butSearch)
        Me.Controls.Add(Me.lblJudge)
        Me.Controls.Add(Me.txtSearch)
        Me.Controls.Add(Me.butDeletePreclusion)
        Me.Controls.Add(Me.DataGridView2)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.DataGridView1)
        Me.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MaximumSize = New System.Drawing.Size(1280, 780)
        Me.MinimumSize = New System.Drawing.Size(1278, 758)
        Me.Name = "frmPreclusionsByJudge"
        Me.Text = "Enter Preclusions by Judge"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents DataGridView2 As System.Windows.Forms.DataGridView
    Friend WithEvents butDeletePreclusion As System.Windows.Forms.Button
    Friend WithEvents ID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents FullName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txtSearch As System.Windows.Forms.TextBox
    Friend WithEvents lblJudge As System.Windows.Forms.Label
    Friend WithEvents butSearch As System.Windows.Forms.Button
    Friend WithEvents cboJudge As System.Windows.Forms.ComboBox
    Friend WithEvents butLoadJudge As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents butHelp As System.Windows.Forms.Button
End Class
