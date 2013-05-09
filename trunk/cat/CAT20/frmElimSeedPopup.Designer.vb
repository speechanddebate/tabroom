<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmElimSeedPopup
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
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.DataGridView2 = New System.Windows.Forms.DataGridView()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Seed = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Entry = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ElimSeedID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Team = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.FullName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ID = New System.Windows.Forms.DataGridViewTextBoxColumn()
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
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.FullName, Me.ID})
        Me.DataGridView1.Location = New System.Drawing.Point(512, 95)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.ReadOnly = True
        Me.DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridView1.Size = New System.Drawing.Size(395, 565)
        Me.DataGridView1.TabIndex = 0
        '
        'DataGridView2
        '
        Me.DataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.DataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView2.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Seed, Me.Entry, Me.ElimSeedID, Me.Team})
        Me.DataGridView2.Location = New System.Drawing.Point(12, 95)
        Me.DataGridView2.Name = "DataGridView2"
        Me.DataGridView2.ReadOnly = True
        Me.DataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridView2.Size = New System.Drawing.Size(437, 565)
        Me.DataGridView2.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 74)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(101, 20)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Current Seeds"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(512, 75)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(156, 20)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Possible replacements"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(17, 13)
        Me.Label3.MaximumSize = New System.Drawing.Size(600, 56)
        Me.Label3.MinimumSize = New System.Drawing.Size(600, 56)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(600, 56)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Click on the current seed in the left-hand box and the team to replace them in th" & _
            "e right-hand box, and then click the ""Process Change"" button"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(668, 13)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(145, 56)
        Me.Button1.TabIndex = 5
        Me.Button1.Text = "Process Change"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Seed
        '
        Me.Seed.DataPropertyName = "Seed"
        Me.Seed.FillWeight = 40.60914!
        Me.Seed.HeaderText = "Seed"
        Me.Seed.Name = "Seed"
        Me.Seed.ReadOnly = True
        '
        'Entry
        '
        Me.Entry.DataPropertyName = "Entry"
        Me.Entry.HeaderText = "Entry"
        Me.Entry.Name = "Entry"
        Me.Entry.ReadOnly = True
        Me.Entry.Visible = False
        '
        'ElimSeedID
        '
        Me.ElimSeedID.DataPropertyName = "ElimSeedID"
        Me.ElimSeedID.HeaderText = "ElimSeedID"
        Me.ElimSeedID.Name = "ElimSeedID"
        Me.ElimSeedID.ReadOnly = True
        Me.ElimSeedID.Visible = False
        '
        'Team
        '
        Me.Team.DataPropertyName = "Team"
        Me.Team.FillWeight = 159.3909!
        Me.Team.HeaderText = "Team"
        Me.Team.Name = "Team"
        Me.Team.ReadOnly = True
        '
        'FullName
        '
        Me.FullName.DataPropertyName = "FullName"
        Me.FullName.HeaderText = "Team"
        Me.FullName.Name = "FullName"
        Me.FullName.ReadOnly = True
        '
        'ID
        '
        Me.ID.DataPropertyName = "ID"
        Me.ID.HeaderText = "ID"
        Me.ID.Name = "ID"
        Me.ID.ReadOnly = True
        Me.ID.Visible = False
        '
        'frmElimSeedPopup
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(936, 662)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.DataGridView2)
        Me.Controls.Add(Me.DataGridView1)
        Me.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "frmElimSeedPopup"
        Me.Text = "Set Elim Seeds"
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents DataGridView2 As System.Windows.Forms.DataGridView
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents FullName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Seed As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Entry As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ElimSeedID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Team As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
