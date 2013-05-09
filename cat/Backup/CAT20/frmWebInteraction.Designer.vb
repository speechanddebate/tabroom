<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmWebInteraction
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
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.radKeyTablesOnly = New System.Windows.Forms.RadioButton
        Me.radWholeFile = New System.Windows.Forms.RadioButton
        Me.butSendFileToWeb = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.DataGridView1 = New System.Windows.Forms.DataGridView
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.butCheck = New System.Windows.Forms.Button
        Me.Button1 = New System.Windows.Forms.Button
        Me.chkNoPrefs = New System.Windows.Forms.CheckBox
        Me.butMakeResults = New System.Windows.Forms.Button
        Me.GroupBox1.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.radKeyTablesOnly)
        Me.GroupBox1.Controls.Add(Me.radWholeFile)
        Me.GroupBox1.Location = New System.Drawing.Point(919, 24)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(333, 96)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "What version of the file?"
        Me.GroupBox1.Visible = False
        '
        'radKeyTablesOnly
        '
        Me.radKeyTablesOnly.AutoSize = True
        Me.radKeyTablesOnly.Location = New System.Drawing.Point(17, 65)
        Me.radKeyTablesOnly.Name = "radKeyTablesOnly"
        Me.radKeyTablesOnly.Size = New System.Drawing.Size(299, 24)
        Me.radKeyTablesOnly.TabIndex = 1
        Me.radKeyTablesOnly.TabStop = True
        Me.radKeyTablesOnly.Text = "Key tables only (exludes tab settings, etc.)"
        Me.radKeyTablesOnly.UseVisualStyleBackColor = True
        '
        'radWholeFile
        '
        Me.radWholeFile.AutoSize = True
        Me.radWholeFile.Location = New System.Drawing.Point(17, 35)
        Me.radWholeFile.Name = "radWholeFile"
        Me.radWholeFile.Size = New System.Drawing.Size(117, 24)
        Me.radWholeFile.TabIndex = 0
        Me.radWholeFile.TabStop = True
        Me.radWholeFile.Text = "The whole file"
        Me.radWholeFile.UseVisualStyleBackColor = True
        '
        'butSendFileToWeb
        '
        Me.butSendFileToWeb.Location = New System.Drawing.Point(919, 126)
        Me.butSendFileToWeb.Name = "butSendFileToWeb"
        Me.butSendFileToWeb.Size = New System.Drawing.Size(147, 63)
        Me.butSendFileToWeb.TabIndex = 1
        Me.butSendFileToWeb.Text = "Send file to web"
        Me.butSendFileToWeb.UseVisualStyleBackColor = True
        Me.butSendFileToWeb.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(245, 4)
        Me.Label1.MaximumSize = New System.Drawing.Size(500, 160)
        Me.Label1.MinimumSize = New System.Drawing.Size(500, 160)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(500, 160)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Update status here"
        '
        'DataGridView1
        '
        Me.DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Location = New System.Drawing.Point(13, 175)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.ReadOnly = True
        Me.DataGridView1.Size = New System.Drawing.Size(732, 464)
        Me.DataGridView1.TabIndex = 3
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(751, 195)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(364, 436)
        Me.TextBox1.TabIndex = 4
        '
        'butCheck
        '
        Me.butCheck.Location = New System.Drawing.Point(12, 87)
        Me.butCheck.Name = "butCheck"
        Me.butCheck.Size = New System.Drawing.Size(200, 63)
        Me.butCheck.TabIndex = 5
        Me.butCheck.Text = "Check Rounds Without Upload"
        Me.butCheck.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(12, 33)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(200, 48)
        Me.Button1.TabIndex = 6
        Me.Button1.Text = "Post tournament file to web"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'chkNoPrefs
        '
        Me.chkNoPrefs.AutoSize = True
        Me.chkNoPrefs.Checked = True
        Me.chkNoPrefs.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkNoPrefs.Location = New System.Drawing.Point(12, 3)
        Me.chkNoPrefs.Name = "chkNoPrefs"
        Me.chkNoPrefs.Size = New System.Drawing.Size(227, 24)
        Me.chkNoPrefs.TabIndex = 7
        Me.chkNoPrefs.Text = "Exclude Prefs in zip file upload"
        Me.chkNoPrefs.UseVisualStyleBackColor = True
        '
        'butMakeResults
        '
        Me.butMakeResults.Location = New System.Drawing.Point(751, 27)
        Me.butMakeResults.Name = "butMakeResults"
        Me.butMakeResults.Size = New System.Drawing.Size(122, 61)
        Me.butMakeResults.TabIndex = 8
        Me.butMakeResults.Text = "Create results file"
        Me.butMakeResults.UseVisualStyleBackColor = True
        '
        'frmWebInteraction
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1264, 720)
        Me.Controls.Add(Me.butMakeResults)
        Me.Controls.Add(Me.chkNoPrefs)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.butCheck)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.butSendFileToWeb)
        Me.Controls.Add(Me.GroupBox1)
        Me.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.MaximumSize = New System.Drawing.Size(1280, 780)
        Me.MinimumSize = New System.Drawing.Size(1278, 758)
        Me.Name = "frmWebInteraction"
        Me.Text = "Interact with the Web"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents radKeyTablesOnly As System.Windows.Forms.RadioButton
    Friend WithEvents radWholeFile As System.Windows.Forms.RadioButton
    Friend WithEvents butSendFileToWeb As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents butCheck As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents chkNoPrefs As System.Windows.Forms.CheckBox
    Friend WithEvents butMakeResults As System.Windows.Forms.Button
End Class
