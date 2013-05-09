<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTRPCPair
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
        Me.cboEvents = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.DataGridView1 = New System.Windows.Forms.DataGridView
        Me.ID = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Label = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.PairingScheme = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.radSeedings = New System.Windows.Forms.RadioButton
        Me.radRandom = New System.Windows.Forms.RadioButton
        Me.butAllPresets = New System.Windows.Forms.Button
        Me.butPairOnePreset = New System.Windows.Forms.Button
        Me.butPowermatch = New System.Windows.Forms.Button
        Me.Button1 = New System.Windows.Forms.Button
        Me.ListBox1 = New System.Windows.Forms.ListBox
        Me.chkDeleteExisting = New System.Windows.Forms.CheckBox
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'cboEvents
        '
        Me.cboEvents.FormattingEnabled = True
        Me.cboEvents.Location = New System.Drawing.Point(13, 36)
        Me.cboEvents.Name = "cboEvents"
        Me.cboEvents.Size = New System.Drawing.Size(188, 28)
        Me.cboEvents.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 14)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(110, 20)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Select Division:"
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.AllowUserToResizeColumns = False
        Me.DataGridView1.AllowUserToResizeRows = False
        Me.DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ID, Me.Label, Me.PairingScheme})
        Me.DataGridView1.Location = New System.Drawing.Point(13, 71)
        Me.DataGridView1.MultiSelect = False
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.ReadOnly = True
        Me.DataGridView1.RowHeadersVisible = False
        Me.DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridView1.Size = New System.Drawing.Size(229, 250)
        Me.DataGridView1.TabIndex = 2
        '
        'ID
        '
        Me.ID.DataPropertyName = "ID"
        Me.ID.HeaderText = "ID"
        Me.ID.Name = "ID"
        Me.ID.ReadOnly = True
        Me.ID.Visible = False
        '
        'Label
        '
        Me.Label.DataPropertyName = "Label"
        Me.Label.HeaderText = "Round"
        Me.Label.Name = "Label"
        Me.Label.ReadOnly = True
        '
        'PairingScheme
        '
        Me.PairingScheme.DataPropertyName = "PairingScheme"
        Me.PairingScheme.HeaderText = "PairingScheme"
        Me.PairingScheme.Name = "PairingScheme"
        Me.PairingScheme.ReadOnly = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.butPairOnePreset)
        Me.GroupBox1.Controls.Add(Me.butAllPresets)
        Me.GroupBox1.Controls.Add(Me.radRandom)
        Me.GroupBox1.Controls.Add(Me.radSeedings)
        Me.GroupBox1.Location = New System.Drawing.Point(13, 347)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(229, 163)
        Me.GroupBox1.TabIndex = 3
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Preset Rounds"
        '
        'radSeedings
        '
        Me.radSeedings.AutoSize = True
        Me.radSeedings.Checked = True
        Me.radSeedings.Location = New System.Drawing.Point(7, 25)
        Me.radSeedings.Name = "radSeedings"
        Me.radSeedings.Size = New System.Drawing.Size(150, 24)
        Me.radSeedings.TabIndex = 0
        Me.radSeedings.TabStop = True
        Me.radSeedings.Text = "Use team seedings"
        Me.radSeedings.UseVisualStyleBackColor = True
        '
        'radRandom
        '
        Me.radRandom.AutoSize = True
        Me.radRandom.Location = New System.Drawing.Point(7, 49)
        Me.radRandom.Name = "radRandom"
        Me.radRandom.Size = New System.Drawing.Size(81, 24)
        Me.radRandom.TabIndex = 1
        Me.radRandom.Text = "Random"
        Me.radRandom.UseVisualStyleBackColor = True
        '
        'butAllPresets
        '
        Me.butAllPresets.Location = New System.Drawing.Point(7, 80)
        Me.butAllPresets.Name = "butAllPresets"
        Me.butAllPresets.Size = New System.Drawing.Size(216, 28)
        Me.butAllPresets.TabIndex = 2
        Me.butAllPresets.Text = "Pair all presets in room pods"
        Me.butAllPresets.UseVisualStyleBackColor = True
        '
        'butPairOnePreset
        '
        Me.butPairOnePreset.Location = New System.Drawing.Point(6, 114)
        Me.butPairOnePreset.Name = "butPairOnePreset"
        Me.butPairOnePreset.Size = New System.Drawing.Size(217, 28)
        Me.butPairOnePreset.TabIndex = 3
        Me.butPairOnePreset.Text = "Pair a single preset round"
        Me.butPairOnePreset.UseVisualStyleBackColor = True
        '
        'butPowermatch
        '
        Me.butPowermatch.Location = New System.Drawing.Point(13, 528)
        Me.butPowermatch.Name = "butPowermatch"
        Me.butPowermatch.Size = New System.Drawing.Size(229, 51)
        Me.butPowermatch.TabIndex = 4
        Me.butPowermatch.Text = "Pair the power-matched round  selected above"
        Me.butPowermatch.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(62, 656)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 5
        Me.Button1.Text = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'ListBox1
        '
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.ItemHeight = 20
        Me.ListBox1.Location = New System.Drawing.Point(574, 427)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(334, 304)
        Me.ListBox1.TabIndex = 6
        '
        'chkDeleteExisting
        '
        Me.chkDeleteExisting.AutoSize = True
        Me.chkDeleteExisting.Location = New System.Drawing.Point(13, 585)
        Me.chkDeleteExisting.Name = "chkDeleteExisting"
        Me.chkDeleteExisting.Size = New System.Drawing.Size(182, 24)
        Me.chkDeleteExisting.TabIndex = 7
        Me.chkDeleteExisting.Text = "Delete existing pairings"
        Me.chkDeleteExisting.UseVisualStyleBackColor = True
        '
        'frmTRPCPair
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1276, 774)
        Me.Controls.Add(Me.chkDeleteExisting)
        Me.Controls.Add(Me.ListBox1)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.butPowermatch)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cboEvents)
        Me.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "frmTRPCPair"
        Me.Text = "frmTRPCPair"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cboEvents As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents ID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Label As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents PairingScheme As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents radRandom As System.Windows.Forms.RadioButton
    Friend WithEvents radSeedings As System.Windows.Forms.RadioButton
    Friend WithEvents butAllPresets As System.Windows.Forms.Button
    Friend WithEvents butPairOnePreset As System.Windows.Forms.Button
    Friend WithEvents butPowermatch As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents chkDeleteExisting As System.Windows.Forms.CheckBox
End Class
