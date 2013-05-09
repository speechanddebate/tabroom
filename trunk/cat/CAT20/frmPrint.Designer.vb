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
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.radHTML = New System.Windows.Forms.RadioButton()
        Me.butPrint = New System.Windows.Forms.Button()
        Me.radRTF = New System.Windows.Forms.RadioButton()
        Me.radPrinter = New System.Windows.Forms.RadioButton()
        Me.radExcel = New System.Windows.Forms.RadioButton()
        Me.radWordFile = New System.Windows.Forms.RadioButton()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.MyPrintDocument = New System.Drawing.Printing.PrintDocument()
        Me.PrintPreviewDialog1 = New System.Windows.Forms.PrintPreviewDialog()
        Me.WebBrowser1 = New System.Windows.Forms.WebBrowser()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.chkLandScape = New System.Windows.Forms.CheckBox()
        Me.txtFont = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
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
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.txtFont)
        Me.GroupBox1.Controls.Add(Me.chkLandScape)
        Me.GroupBox1.Controls.Add(Me.radHTML)
        Me.GroupBox1.Controls.Add(Me.butPrint)
        Me.GroupBox1.Controls.Add(Me.radRTF)
        Me.GroupBox1.Controls.Add(Me.radPrinter)
        Me.GroupBox1.Controls.Add(Me.radExcel)
        Me.GroupBox1.Controls.Add(Me.radWordFile)
        Me.GroupBox1.Location = New System.Drawing.Point(381, 107)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(207, 276)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Select Print Method"
        '
        'radHTML
        '
        Me.radHTML.AutoSize = True
        Me.radHTML.Location = New System.Drawing.Point(7, 193)
        Me.radHTML.MinimumSize = New System.Drawing.Size(0, 24)
        Me.radHTML.Name = "radHTML"
        Me.radHTML.Size = New System.Drawing.Size(117, 24)
        Me.radHTML.TabIndex = 5
        Me.radHTML.Text = "Print as HTML"
        Me.radHTML.UseVisualStyleBackColor = True
        '
        'butPrint
        '
        Me.butPrint.Location = New System.Drawing.Point(59, 219)
        Me.butPrint.Name = "butPrint"
        Me.butPrint.Size = New System.Drawing.Size(88, 30)
        Me.butPrint.TabIndex = 4
        Me.butPrint.Text = "PRINT"
        Me.butPrint.UseVisualStyleBackColor = True
        '
        'radRTF
        '
        Me.radRTF.AutoSize = True
        Me.radRTF.Location = New System.Drawing.Point(7, 103)
        Me.radRTF.MinimumSize = New System.Drawing.Size(0, 24)
        Me.radRTF.Name = "radRTF"
        Me.radRTF.Size = New System.Drawing.Size(125, 24)
        Me.radRTF.TabIndex = 3
        Me.radRTF.Text = "Send to .rtf file"
        Me.radRTF.UseVisualStyleBackColor = True
        '
        'radPrinter
        '
        Me.radPrinter.AutoSize = True
        Me.radPrinter.Location = New System.Drawing.Point(6, 163)
        Me.radPrinter.MinimumSize = New System.Drawing.Size(0, 24)
        Me.radPrinter.Name = "radPrinter"
        Me.radPrinter.Size = New System.Drawing.Size(173, 24)
        Me.radPrinter.TabIndex = 2
        Me.radPrinter.Text = "Print directly to printer"
        Me.radPrinter.UseVisualStyleBackColor = True
        '
        'radExcel
        '
        Me.radExcel.AutoSize = True
        Me.radExcel.Location = New System.Drawing.Point(7, 133)
        Me.radExcel.MinimumSize = New System.Drawing.Size(0, 24)
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
        Me.radWordFile.Location = New System.Drawing.Point(7, 23)
        Me.radWordFile.MinimumSize = New System.Drawing.Size(0, 24)
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
        'WebBrowser1
        '
        Me.WebBrowser1.Location = New System.Drawing.Point(360, 406)
        Me.WebBrowser1.MinimumSize = New System.Drawing.Size(20, 20)
        Me.WebBrowser1.Name = "WebBrowser1"
        Me.WebBrowser1.Size = New System.Drawing.Size(250, 120)
        Me.WebBrowser1.TabIndex = 6
        Me.WebBrowser1.Visible = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(377, 9)
        Me.Label2.MaximumSize = New System.Drawing.Size(230, 90)
        Me.Label2.MinimumSize = New System.Drawing.Size(230, 90)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(230, 90)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Word and excel printing require Office 2007 or later.  HTML format is the least f" & _
            "ormatted but most likely to print on all systems."
        '
        'chkLandScape
        '
        Me.chkLandScape.AutoSize = True
        Me.chkLandScape.Location = New System.Drawing.Point(23, 47)
        Me.chkLandScape.Name = "chkLandScape"
        Me.chkLandScape.Size = New System.Drawing.Size(97, 24)
        Me.chkLandScape.TabIndex = 8
        Me.chkLandScape.Text = "Landscape"
        Me.chkLandScape.UseVisualStyleBackColor = True
        '
        'txtFont
        '
        Me.txtFont.Location = New System.Drawing.Point(23, 72)
        Me.txtFont.Name = "txtFont"
        Me.txtFont.Size = New System.Drawing.Size(24, 25)
        Me.txtFont.TabIndex = 9
        Me.txtFont.Text = "10"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(49, 74)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(67, 20)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "Font size"
        '
        'frmPrint
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(628, 538)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.WebBrowser1)
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
    Friend WithEvents radHTML As System.Windows.Forms.RadioButton
    Friend WithEvents WebBrowser1 As System.Windows.Forms.WebBrowser
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents chkLandScape As System.Windows.Forms.CheckBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtFont As System.Windows.Forms.TextBox
End Class
