Imports Microsoft.VisualBasic
Imports System.Windows.Forms
Imports System.Drawing.Printing
Imports System.Drawing.Graphics
Imports Word = Microsoft.Office.Interop.Word
Imports System.IO


Public Class frmPrint
    Dim dgv As DataGridView
    Dim strHeaderContent As String
    Dim defCols(50) As Boolean
    Private MyDataGridViewPrinter As cDataGridViewPrinter
    Private bLandscapePageMode As Boolean = True
    Public Sub New(ByVal DGVin As DataGridView, ByVal strHdr As String, ByVal usecols As Array)
        InitializeComponent()
        dgv = DGVin
        strHeaderContent = strHdr
        defCols = usecols
    End Sub
    Private Sub frmPrint_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim dt As New DataTable
        dt.Columns.Add("Column", System.Type.GetType("System.String"))
        dt.Columns.Add("Print", System.Type.GetType("System.Boolean"))
        Dim x As Integer
        Dim dr As DataRow
        'put columns in display order
        For x = 0 To dgv.Columns.Count - 1
            For y = 0 To dgv.Columns.Count - 1
                If dgv.Columns(y).DisplayIndex = x Then
                    dr = dt.NewRow
                    dr.Item("Column") = dgv.Columns(y).HeaderText
                    dr.Item("Print") = defCols(y)
                    dt.Rows.Add(dr)
                End If
            Next y
        Next
        DataGridView1.DataSource = dt
        DataGridView1.Columns(0).ReadOnly = True
        DataGridView1.ClearSelection()
        butPrint.Focus()
    End Sub

    Private Sub butPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butPrint.Click
        Dim dt As New DataTable
        Call MakeTableFromGrid(dt)
        If radWordFile.Checked = True Then Call PrintTableToWord(dt)
        If radRTF.Checked = True Then
            MsgBox("This will launch a word file and save it in .rft format in the CAT sub-folder in your documents folder.  The file will be called 'SavedFile.rtf'")
            Call PrintTableToWord(dt)
        End If
        If radExcel.Checked = True Then Call ExportToExcel(dt)
        If radPrinter.Checked = True Then Call SendToPrinter()
        If radHTML.Checked = True Then WebBrowser1.ShowPrintDialog()
    End Sub
    Sub PrintasHTML()
        Dim strBreakline = "<p style=" & Chr(34) & "page-break-before: always" & Chr(34) & ">"
        Dim j As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\CAT\PrintContent.html"
        Dim st As StreamWriter = File.CreateText(j)
        st.WriteLine("<HTML>")
        st.WriteLine("<HEAD>")
        st.WriteLine("<style type=" & Chr(34) & "text/css" & Chr(34) & ">")
        st.WriteLine("table, th, td")
        st.WriteLine("{")
        st.WriteLine("font-family:" & Chr(34) & "Arial" & Chr(34) & ";")
        st.WriteLine("border:1px solid black;")
        st.WriteLine("border-collapse:collapse;")
        st.WriteLine("padding:3px 7px 2px 7px;")
        st.WriteLine("}")
        st.WriteLine("body")
        st.WriteLine("{")
        st.WriteLine("font-family:" & Chr(34) & "Arial" & Chr(34) & ";")
        st.WriteLine("font-size:18;")
        st.WriteLine("font-weight:bold;")
        st.WriteLine("}")
        st.WriteLine("div.Regular")
        st.WriteLine("{")
        st.WriteLine("font-family:" & Chr(34) & "Arial" & Chr(34) & ";")
        st.WriteLine("font-size:14;")
        st.WriteLine("font-weight:normal;")
        st.WriteLine("}")
        st.WriteLine("</style>")
        st.WriteLine("</HEAD>")
        Dim strHdr As String = strHeaderContent
        strHdr = strHdr.Replace(ControlChars.Lf, "<br/>")
        st.WriteLine("<center>" & strHdr & "</center><br>")
        st.WriteLine("<TABLE>")
        st.WriteLine("<THEAD>")
        st.WriteLine("<TR>")
        For x = 0 To dgv.Columns.Count - 1
            If dgv.Columns(x).Visible = True Then
                st.WriteLine("<TH>" & dgv.Columns(x).HeaderText & "</TH>")
            End If
        Next x
        st.WriteLine("</TR>")
        st.WriteLine("</THEAD>")
        For x = 0 To dgv.Rows.Count - 1
            st.WriteLine("<TR>")
            For y = 0 To dgv.Columns.Count - 1
                If dgv.Columns(y).Visible = True Then
                    If dgv.Rows(x).Cells(y).Value Is Nothing Then dgv.Rows(x).Cells(y).Value = ""
                    st.WriteLine("<TD>" & dgv.Rows(x).Cells(y).Value.ToString & "</TD>")
                End If
            Next y
            st.WriteLine("</TR>")
        Next x
        st.WriteLine("</TABLE><br>")
        st.WriteLine("</HTML>")
        st.Close()
        WebBrowser1.Navigate(j)
    End Sub
    Sub SendToPrinter()
        If SetupThePrinting() Then MyPrintDocument.Print()
    End Sub
    Private Function SetupThePrinting() As Boolean
        Dim MyPrintDialog As PrintDialog = New PrintDialog()
        MyPrintDialog.AllowCurrentPage = False
        MyPrintDialog.AllowPrintToFile = False
        MyPrintDialog.AllowSelection = False
        MyPrintDialog.AllowSomePages = True
        MyPrintDialog.PrintToFile = False
        MyPrintDialog.ShowHelp = False
        MyPrintDialog.ShowNetwork = False

        If MyPrintDialog.ShowDialog() <> System.Windows.Forms.DialogResult.OK Then Return False

        MyPrintDocument.DocumentName = "CAT 2.0 Report"
        MyPrintDocument.PrinterSettings = MyPrintDialog.PrinterSettings
        MyPrintDocument.DefaultPageSettings = MyPrintDialog.PrinterSettings.DefaultPageSettings
        MyPrintDocument.DefaultPageSettings.Margins = New Margins(40, 40, 40, 40)

        Select Case bLandscapePageMode     'true=portrait  false=landscape
            Case True : MyPrintDocument.DefaultPageSettings.Landscape = True
            Case Else : MyPrintDocument.DefaultPageSettings.Landscape = False
        End Select

        MyDataGridViewPrinter = New cDataGridViewPrinter(dgv, MyPrintDocument, True, True, strHeaderContent, New Font("Tahoma", 18, FontStyle.Bold, GraphicsUnit.Point), Color.Black, True)
        Return True
    End Function
    Private Sub MyPrintDocument_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles MyPrintDocument.PrintPage

        Dim more As Boolean
        Try
            more = MyDataGridViewPrinter.DrawDataGridView(e)
            If more Then e.HasMorePages = True
        Catch Ex As Exception
            'MessageBox.Show(Ex.Message & vbCrLf & Ex.StackTrace, MyConstants.CaptionFehler, MessageBoxButtons.OK, MessageBoxIcon.Error)        End Try
        End Try

    End Sub

    Sub MakeTableFromGrid(ByRef DT As DataTable)
        'First, puts the columns in the right order.  Second, puts headertext in dt.caption tag
        'add colulmns in displayindex order
        Dim x, y As Integer
        Dim dr As DataRow
        'add columns to the talbe in the order on the datagridview1 table if selected
        For x = 0 To DataGridView1.Rows.Count - 1
            If DataGridView1.Rows(x).Cells("Print").Value = "True" Then
                For y = 0 To dgv.Columns.Count - 1
                    If dgv.Columns(y).HeaderText = DataGridView1.Rows(x).Cells("Column").Value Then
                        DT.Columns.Add(dgv.Columns(y).Name, System.Type.GetType("System.String"))
                        DT.Columns(dgv.Columns(y).Name).Caption = dgv.Columns(y).HeaderText
                    End If
                Next y
            End If
        Next x
        'fill the table
        'scroll through each row in datagrid
        For x = 0 To dgv.Rows.Count - 1
            dr = DT.NewRow
            'scroll each column in datatable and add its value
            For y = 0 To DT.Columns.Count - 1
                dr.Item(y) = dgv.Rows(x).Cells(DT.Columns(y).ColumnName).FormattedValue.ToString
            Next y
            DT.Rows.Add(dr)
        Next x
    End Sub
    Sub PrintTableToWord(ByVal DT As DataTable)
        'Set up word table
        Dim oWord As Word.Application
        Dim oDoc As Word.Document
        Dim oTable As Word.Table
        Dim oPara1 As Word.Paragraph

        oWord = CreateObject("Word.Application")
        oWord.Visible = False
        oDoc = oWord.Documents.Add
        If chkLandScape.Checked = True Then oDoc.PageSetup.Orientation = Word.WdOrientation.wdOrientLandscape
        oDoc.Paragraphs.Format.SpaceAfter = 0
        oPara1 = oDoc.Content.Paragraphs.Add

        'add a table
        oPara1.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft
        oTable = oDoc.Tables.Add(oDoc.Bookmarks.Item("\endofdoc").Range, DT.Rows.Count + 1, DT.Columns.Count)
        oTable.AutoFitBehavior(Word.WdAutoFitBehavior.wdAutoFitContent)
        oTable.Range.Font.Size = Val(txtFont.Text)
        oTable.Range.Font.Underline = False : oTable.Range.Font.Bold = False
        oTable.Range.ParagraphFormat.SpaceBefore = 0
        oTable.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle
        oTable.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle
        oTable.Style = Word.WdBuiltinStyle.wdStyleTableLightGridAccent1
        oTable.Rows(1).HeadingFormat = True

        'Header
        Dim section As Microsoft.Office.Interop.Word.Section
        For Each section In oDoc.Sections
            section.Headers(Word.WdHeaderFooterIndex.wdHeaderFooterPrimary).Range.Text = strHeaderContent
            section.Headers(Word.WdHeaderFooterIndex.wdHeaderFooterPrimary).Range.Font.Size = 14
            section.Headers(Word.WdHeaderFooterIndex.wdHeaderFooterPrimary).Range.Font.Bold = True
            section.Headers(Word.WdHeaderFooterIndex.wdHeaderFooterPrimary).Range.Font.Name = Font.Name.Trim = "Calibri"
            section.Headers(Word.WdHeaderFooterIndex.wdHeaderFooterPrimary).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter
        Next

        'Read in column names
        'read column headers and write to top row
        Dim x, y As Integer
        For x = 0 To DT.Columns.Count() - 1
            oTable.Cell(1, x + 1).Range.Text = DT.Columns(x).Caption.ToString.ToUpper
            oTable.Cell(1, x + 1).Range.Bold = True
            oTable.Cell(1, x + 1).Shading.BackgroundPatternColor = Word.WdColor.wdColorGray20
        Next x
        'read line by line
        For x = 0 To DT.Columns.Count() - 1
            lblStatus.Text = "Processing column " & x + 1 & " of " & DT.Columns.Count : lblStatus.Refresh()
            For y = 0 To DT.Rows.Count() - 1
                If Not DT.Rows(y).Item(x) Is Nothing Then
                    oTable.Cell(y + 2, x + 1).Range.Text = DT.Rows(y).Item(x).ToString.Trim
                    'If Mid(oTable.Cell(y + 2, x + 1).Range.Text, 1, 11) = "HARD RETURN" Then
                    'oTable.Cell(y + 2, x + 1).Range.InsertBreak()
                    'End If
                End If
            Next y
        Next x
        oWord.Visible = True
        If radRTF.Checked = True Then oDoc.SaveAs(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\CAT\SavedFile.Rtf", FileFormat:=6)
        oWord = Nothing
        oDoc = Nothing
        lblStatus.Text = "Done - check systray (the ribbon on the bottom of the screen) for word document"
    End Sub
    Sub PrintGridToWord()
        'probably obsolete; preferred method is converting grid to table and printing table
        'Set up word table
        Dim oWord As Word.Application
        Dim oDoc As Word.Document
        Dim oTable As Word.Table
        Dim oPara1 As Word.Paragraph


        oWord = CreateObject("Word.Application")
        oWord.Visible = False
        oDoc = oWord.Documents.Add
        oDoc.Paragraphs.Format.SpaceAfter = 0
        oPara1 = oDoc.Content.Paragraphs.Add

        'add a table
        oPara1.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft
        oTable = oDoc.Tables.Add(oDoc.Bookmarks.Item("\endofdoc").Range, dgv.Rows.Count(), dgv.Columns.Count())
        oTable.AutoFitBehavior(Word.WdAutoFitBehavior.wdAutoFitContent)
        oTable.Range.Font.Size = 10 : oTable.Range.Font.Underline = False : oTable.Range.Font.Bold = False
        oTable.Range.ParagraphFormat.SpaceAfter = 0
        oTable.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle
        oTable.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle
        'oTable.AutoFitBehavior(Word.WdRowHeightRule.wdRowHeightAuto)

        'oTable.Style = Word.WdBuiltinStyle.wdStyleTableLightGridAccent1
        oTable.Rows(1).HeadingFormat = True

        'Read in column names
        'read column headers and write to top row
        Dim x, y As Integer
        For x = 0 To dgv.Columns.Count() - 1
            oTable.Cell(1, x + 1).Range.Text = dgv.Columns(x).HeaderText.ToString.ToUpper.Trim
            oTable.Cell(1, x + 1).Range.Bold = True
            oTable.Cell(1, x + 1).Shading.BackgroundPatternColor = Word.WdColor.wdColorGray20
        Next x
        'read line by line
        For x = 0 To dgv.Columns.Count() - 1
            For y = 0 To dgv.Rows.Count() - 1
                'If dgv.Columns(x).celltemplae = System.Windows.Forms.DataGridViewTextBoxCell Then dgv.Rows(y).Cells(x).Value.ToString.Trim
                If Not dgv.Rows(y).Cells(x).FormattedValue Is Nothing Then
                    oTable.Cell(y + 2, x + 1).Range.Text = dgv.Rows(y).Cells(x).FormattedValue.ToString.Trim
                End If
            Next y
        Next x
        oWord.Visible = True
        oWord = Nothing
        oDoc = Nothing
    End Sub
    Private Sub ExportToExcel(ByVal DT As DataTable)
        Dim excel As New Microsoft.Office.Interop.Excel.ApplicationClass
        Dim wBook As Microsoft.Office.Interop.Excel.Workbook
        Dim wSheet As Microsoft.Office.Interop.Excel.Worksheet

        wBook = excel.Workbooks.Add()
        wSheet = wBook.ActiveSheet()

        Dim dc As System.Data.DataColumn
        Dim dr As System.Data.DataRow
        Dim colIndex As Integer = 0
        Dim rowIndex As Integer = 0

        For Each dc In DT.Columns
            colIndex = colIndex + 1
            excel.Cells(1, colIndex) = dc.ColumnName
        Next

        excel.Cells(1, 1).EntireRow.Font.Bold = True
        'excel.Cells(1, 1).EntireRow.inter()

        For Each dr In DT.Rows
            rowIndex = rowIndex + 1
            colIndex = 0
            For Each dc In DT.Columns
                colIndex = colIndex + 1
                excel.Cells(rowIndex + 1, colIndex) = dr(dc.ColumnName)
            Next
        Next

        wSheet.Columns.AutoFit()
        'wSheet.Columns(1).ColumnWidth = wSheet.Columns.EntireColumn.AutoFit


        Dim strFileName As String = "CAT Output.xls"
        Dim blnFileOpen As Boolean = False
        Try
            Dim fileTemp As System.IO.FileStream = System.IO.File.OpenWrite(strFileName)
            fileTemp.Close()
        Catch ex As Exception
            blnFileOpen = False
        End Try

        If System.IO.File.Exists(strFileName) Then
            System.IO.File.Delete(strFileName)
        End If

        wBook.SaveAs(strFileName)
        excel.Workbooks.Open(strFileName)
        excel.Visible = True
    End Sub
    Sub HTMLCheckCheck() Handles radHTML.CheckedChanged
        Call PrintasHTML()
    End Sub
End Class