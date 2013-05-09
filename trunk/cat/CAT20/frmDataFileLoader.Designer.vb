<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDataFileLoader
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
        Me.butInitialize = New System.Windows.Forms.Button
        Me.butLocate = New System.Windows.Forms.Button
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.butUDSImport = New System.Windows.Forms.Button
        Me.Button1 = New System.Windows.Forms.Button
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.lblDownLoadStatus = New System.Windows.Forms.Label
        Me.butDownLoad = New System.Windows.Forms.Button
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.DataGridView2 = New System.Windows.Forms.DataGridView
        Me.EventID = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.EventName = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridView1 = New System.Windows.Forms.DataGridView
        Me.ID = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.TournName = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.StartDate = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.EndDate = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Button2 = New System.Windows.Forms.Button
        Me.txtPassWord = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtUserName = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.butTestDownload = New System.Windows.Forms.Button
        Me.chkMPJSample = New System.Windows.Forms.CheckBox
        Me.butDeselectAll = New System.Windows.Forms.Button
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'butInitialize
        '
        Me.butInitialize.Location = New System.Drawing.Point(6, 23)
        Me.butInitialize.Name = "butInitialize"
        Me.butInitialize.Size = New System.Drawing.Size(167, 50)
        Me.butInitialize.TabIndex = 0
        Me.butInitialize.Text = "Initialize a blank data file"
        Me.butInitialize.UseVisualStyleBackColor = True
        '
        'butLocate
        '
        Me.butLocate.Location = New System.Drawing.Point(6, 79)
        Me.butLocate.Name = "butLocate"
        Me.butLocate.Size = New System.Drawing.Size(167, 50)
        Me.butLocate.TabIndex = 1
        Me.butLocate.Text = "Locate an existing file on your computer"
        Me.butLocate.UseVisualStyleBackColor = True
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'butUDSImport
        '
        Me.butUDSImport.Location = New System.Drawing.Point(6, 191)
        Me.butUDSImport.Name = "butUDSImport"
        Me.butUDSImport.Size = New System.Drawing.Size(167, 87)
        Me.butUDSImport.TabIndex = 3
        Me.butUDSImport.Text = "Import file using the Universal Data Structure"
        Me.butUDSImport.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(6, 135)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(167, 50)
        Me.Button1.TabIndex = 4
        Me.Button1.Text = "Erase and re-initialize an EXISTING data file"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.butInitialize)
        Me.GroupBox1.Controls.Add(Me.butUDSImport)
        Me.GroupBox1.Controls.Add(Me.Button1)
        Me.GroupBox1.Controls.Add(Me.butLocate)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(218, 300)
        Me.GroupBox1.TabIndex = 5
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Offline File Processing"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.butDeselectAll)
        Me.GroupBox2.Controls.Add(Me.lblDownLoadStatus)
        Me.GroupBox2.Controls.Add(Me.butDownLoad)
        Me.GroupBox2.Controls.Add(Me.Label6)
        Me.GroupBox2.Controls.Add(Me.Label5)
        Me.GroupBox2.Controls.Add(Me.Label4)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.DataGridView2)
        Me.GroupBox2.Controls.Add(Me.DataGridView1)
        Me.GroupBox2.Controls.Add(Me.Button2)
        Me.GroupBox2.Controls.Add(Me.txtPassWord)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Controls.Add(Me.txtUserName)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Location = New System.Drawing.Point(255, 12)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(699, 718)
        Me.GroupBox2.TabIndex = 6
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Load a File From the Internet (iDebate.org tournaments only)"
        '
        'lblDownLoadStatus
        '
        Me.lblDownLoadStatus.AutoSize = True
        Me.lblDownLoadStatus.Location = New System.Drawing.Point(201, 612)
        Me.lblDownLoadStatus.Name = "lblDownLoadStatus"
        Me.lblDownLoadStatus.Size = New System.Drawing.Size(160, 20)
        Me.lblDownLoadStatus.TabIndex = 12
        Me.lblDownLoadStatus.Text = "No file yet downloaded."
        '
        'butDownLoad
        '
        Me.butDownLoad.Location = New System.Drawing.Point(271, 554)
        Me.butDownLoad.Name = "butDownLoad"
        Me.butDownLoad.Size = New System.Drawing.Size(104, 28)
        Me.butDownLoad.TabIndex = 11
        Me.butDownLoad.Text = "Download!"
        Me.butDownLoad.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(11, 561)
        Me.Label6.MaximumSize = New System.Drawing.Size(650, 50)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(243, 20)
        Me.Label6.TabIndex = 10
        Me.Label6.Text = "STEP FOUR: Click here to download:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(11, 325)
        Me.Label5.MaximumSize = New System.Drawing.Size(650, 50)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(487, 20)
        Me.Label5.TabIndex = 9
        Me.Label5.Text = "STEP THREE: Check the divisions you wish to download into this computer"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(11, 107)
        Me.Label4.MaximumSize = New System.Drawing.Size(650, 50)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(382, 20)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "STEP TWO: Click on the tournament you wish to download"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(11, 24)
        Me.Label3.MaximumSize = New System.Drawing.Size(650, 50)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(626, 40)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "STEP ONE: Enter your username and password, and click the ""Show My Tournaments"" b" & _
            "utton to display the tournaments you can download."
        '
        'DataGridView2
        '
        Me.DataGridView2.AllowUserToAddRows = False
        Me.DataGridView2.AllowUserToDeleteRows = False
        Me.DataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.DataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView2.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.EventID, Me.EventName})
        Me.DataGridView2.Location = New System.Drawing.Point(11, 367)
        Me.DataGridView2.Name = "DataGridView2"
        Me.DataGridView2.RowHeadersVisible = False
        Me.DataGridView2.Size = New System.Drawing.Size(673, 164)
        Me.DataGridView2.TabIndex = 6
        '
        'EventID
        '
        Me.EventID.DataPropertyName = "ID"
        Me.EventID.HeaderText = "Event ID"
        Me.EventID.Name = "EventID"
        Me.EventID.ReadOnly = True
        Me.EventID.Visible = False
        '
        'EventName
        '
        Me.EventName.DataPropertyName = "EventName"
        Me.EventName.HeaderText = "Event"
        Me.EventName.Name = "EventName"
        Me.EventName.ReadOnly = True
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.AllowUserToResizeColumns = False
        Me.DataGridView1.AllowUserToResizeRows = False
        Me.DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ID, Me.TournName, Me.StartDate, Me.EndDate})
        Me.DataGridView1.Location = New System.Drawing.Point(11, 150)
        Me.DataGridView1.MultiSelect = False
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.ReadOnly = True
        Me.DataGridView1.RowHeadersVisible = False
        Me.DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridView1.Size = New System.Drawing.Size(682, 150)
        Me.DataGridView1.TabIndex = 5
        '
        'ID
        '
        Me.ID.DataPropertyName = "ID"
        Me.ID.HeaderText = "ID"
        Me.ID.Name = "ID"
        Me.ID.ReadOnly = True
        Me.ID.Visible = False
        '
        'TournName
        '
        Me.TournName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.TournName.DataPropertyName = "TournName"
        Me.TournName.HeaderText = "Tourney Name"
        Me.TournName.Name = "TournName"
        Me.TournName.ReadOnly = True
        Me.TournName.Width = 125
        '
        'StartDate
        '
        Me.StartDate.DataPropertyName = "StartDate"
        Me.StartDate.HeaderText = "Start"
        Me.StartDate.Name = "StartDate"
        Me.StartDate.ReadOnly = True
        '
        'EndDate
        '
        Me.EndDate.DataPropertyName = "EndDate"
        Me.EndDate.HeaderText = "End"
        Me.EndDate.Name = "EndDate"
        Me.EndDate.ReadOnly = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(475, 68)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(218, 28)
        Me.Button2.TabIndex = 4
        Me.Button2.Text = "Show My Tournaments"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'txtPassWord
        '
        Me.txtPassWord.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.txtPassWord.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.AllUrl
        Me.txtPassWord.Location = New System.Drawing.Point(319, 68)
        Me.txtPassWord.Name = "txtPassWord"
        Me.txtPassWord.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPassWord.Size = New System.Drawing.Size(133, 24)
        Me.txtPassWord.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(237, 68)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(75, 20)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Password:"
        '
        'txtUserName
        '
        Me.txtUserName.Location = New System.Drawing.Point(89, 68)
        Me.txtUserName.Name = "txtUserName"
        Me.txtUserName.Size = New System.Drawing.Size(133, 24)
        Me.txtUserName.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(7, 68)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(79, 20)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "UserName:"
        '
        'butTestDownload
        '
        Me.butTestDownload.Location = New System.Drawing.Point(12, 337)
        Me.butTestDownload.Name = "butTestDownload"
        Me.butTestDownload.Size = New System.Drawing.Size(173, 61)
        Me.butTestDownload.TabIndex = 7
        Me.butTestDownload.Text = "Download a sample file to play around with"
        Me.butTestDownload.UseVisualStyleBackColor = True
        '
        'chkMPJSample
        '
        Me.chkMPJSample.AutoSize = True
        Me.chkMPJSample.Location = New System.Drawing.Point(12, 405)
        Me.chkMPJSample.Name = "chkMPJSample"
        Me.chkMPJSample.Size = New System.Drawing.Size(174, 24)
        Me.chkMPJSample.TabIndex = 8
        Me.chkMPJSample.Text = "Download MPJ sample"
        Me.chkMPJSample.UseVisualStyleBackColor = True
        '
        'butDeselectAll
        '
        Me.butDeselectAll.Location = New System.Drawing.Point(556, 306)
        Me.butDeselectAll.Name = "butDeselectAll"
        Me.butDeselectAll.Size = New System.Drawing.Size(128, 55)
        Me.butDeselectAll.TabIndex = 9
        Me.butDeselectAll.Text = "De-select all events"
        Me.butDeselectAll.UseVisualStyleBackColor = True
        '
        'frmDataFileLoader
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1264, 742)
        Me.Controls.Add(Me.chkMPJSample)
        Me.Controls.Add(Me.butTestDownload)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.MaximumSize = New System.Drawing.Size(1280, 780)
        Me.MinimumSize = New System.Drawing.Size(1278, 758)
        Me.Name = "frmDataFileLoader"
        Me.Text = "Data File Loader"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents butInitialize As System.Windows.Forms.Button
    Friend WithEvents butLocate As System.Windows.Forms.Button
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents butUDSImport As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents txtPassWord As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtUserName As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents ID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TournName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents StartDate As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents EndDate As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridView2 As System.Windows.Forms.DataGridView
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents butDownLoad As System.Windows.Forms.Button
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents lblDownLoadStatus As System.Windows.Forms.Label
    Friend WithEvents butTestDownload As System.Windows.Forms.Button
    Friend WithEvents chkMPJSample As System.Windows.Forms.CheckBox
    Friend WithEvents EventID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents EventName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents butDeselectAll As System.Windows.Forms.Button
End Class
