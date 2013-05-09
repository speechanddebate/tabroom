<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPrint
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPrint))
        Me.DataGridView1 = New System.Windows.Forms.DataGridView
        Me.Label1 = New System.Windows.Forms.Label
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.butPrint = New System.Windows.Forms.Button
        Me.radRTF = New System.Windows.Forms.RadioButton
        Me.radPrinter = New System.Windows.Forms.RadioButton
        Me.radExcel = New System.Windows.Forms.RadioButton
        Me.radWordFile = New System.Windows.Forms.RadioButton
        Me.lblStatus = New System.Windows.Forms.Label
        Me.MyPrintDocument = New System.Drawing.Printing.PrintDocument
        Me.PrintPreviewDialog1 = New System.Windows.Forms.PrintPreviewDialog
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
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
        Me.DataGridView1.Location = New System.Drawing.Point(31, 125)
        Me.DataGridView1.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.Size = New System.Drawing.Size(320, 231)
        Me.DataGridView1.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(31, 95)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(159, 20)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Select columns to print"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.butPrint)
        Me.GroupBox1.Controls.Add(Me.radRTF)
        Me.GroupBox1.Controls.Add(Me.radPrinter)
        Me.GroupBox1.Controls.Add(Me.radExcel)
        Me.GroupBox1.Controls.Add(Me.radWordFile)
        Me.GroupBox1.Location = New System.Drawing.Point(381, 107)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(207, 249)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Select Print Method"
        '
        'butPrint
        '
        Me.butPrint.Location = New System.Drawing.Point(59, 177)
        Me.butPrint.Name = "butPrint"
        Me.butPrint.Size = New System.Drawing.Size(88, 30)
        Me.butPrint.TabIndex = 4
        Me.butPrint.Text = "PRINT"
        Me.butPrint.UseVisualStyleBackColor = True
        '
        'radRTF
        '
        Me.radRTF.AutoSize = True
        Me.radRTF.Location = New System.Drawing.Point(7, 70)
        Me.radRTF.Name = "radRTF"
        Me.radRTF.Size = New System.Drawing.Size(125, 24)
        Me.radRTF.TabIndex = 3
        Me.radRTF.Text = "Send to .rtf file"
        Me.radRTF.UseVisualStyleBackColor = True
        '
        'radPrinter
        '
        Me.radPrinter.AutoSize = True
        Me.radPrinter.Location = New System.Drawing.Point(6, 130)
        Me.radPrinter.Name = "radPrinter"
        Me.radPrinter.Size = New System.Drawing.Size(173, 24)
        Me.radPrinter.TabIndex = 2
        Me.radPrinter.Text = "Print directly to printer"
        Me.radPrinter.UseVisualStyleBackColor = True
        '
        'radExcel
        '
        Me.radExcel.AutoSize = True
        Me.radExcel.Location = New System.Drawing.Point(7, 100)
        Me.radExcel.Name = "radExcel"
        Me.radExcel.Size = New System.Drawing.Size(140, 24)
        Me.radExcel.TabIndex = 1
        Me.radExcel.Text = "Send to excel file"
        Me.radExcel.UseVisualStyleBackColor = True
        '
        'radWordFile
        '
        Me.radWordFile.AutoSize = True
        Me.radWordFile.Checked = True
        Me.radWordFile.Location = New System.Drawing.Point(7, 40)
        Me.radWordFile.Name = "radWordFile"
        Me.radWordFile.Size = New System.Drawing.Size(143, 24)
        Me.radWordFile.TabIndex = 0
        Me.radWordFile.TabStop = True
        Me.radWordFile.Text = "Send to Word File"
        Me.radWordFile.UseVisualStyleBackColor = True
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Location = New System.Drawing.Point(67, 387)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(0, 20)
        Me.lblStatus.TabIndex = 5
        '
        'MyPrintDocument
        '
        '
        'PrintPreviewDialog1
        '
        Me.PrintPreviewDialog1.AutoScrollMargin = New System.Drawing.Size(0, 0)
        Me.PrintPreviewDialog1.AutoScrollMinSize = New System.Drawing.Size(0, 0)
        Me.PrintPreviewDialog1.ClientSize = New System.Drawing.Size(400, 300)
        Me.PrintPreviewDialog1.Enabled = True
        Me.PrintPreviewDialog1.Icon = CType(resources.GetObject("PrintPreviewDialog1.Icon"), System.Drawing.Icon)
        Me.PrintPreviewDialog1.Name = "PrintPreviewDialog1"
        Me.PrintPreviewDialog1.Visible = False
        '
        'frmPrint
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(628, 538)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.DataGridView1)
        Me.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "frmPrint"
        Me.Text = "Print"
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents butPrint As System.Windows.Forms.Button
    Friend WithEvents radRTF As System.Windows.Forms.RadioButton
    Friend WithEvents radPrinter As System.Windows.Forms.RadioButton
    Friend WithEvents radExcel As System.Windows.Forms.RadioButton
    Friend WithEvents radWordFile As System.Windows.Forms.RadioButton
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents MyPrintDocument As System.Drawing.Printing.PrintDocument
    Friend WithEvents PrintPreviewDialog1 As System.Windows.Forms.PrintPreviewDialog
End Class
