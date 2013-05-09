Imports System
Imports System.Drawing
Imports System.Drawing.Printing

Public Class cDataGridViewPrinter
    Private TheDataGridView As DataGridView  ' The DataGridView Control which will be printed
    Private ThePrintDocument As PrintDocument ' The PrintDocument to be used for printing
    Private IsCenterOnPage As Boolean ' Determine if the report will be printed in the Top-Center of the page
    Private IsWithTitle As Boolean  ' Determine if the page contain title text
    Private TheTitleText As String ' The title text to be printed in each page (if IsWithTitle is set to true)
    Private TheTitleFont As Font ' The font to be used with the title text (if IsWithTitle is set to true)
    Private TheTitleColor As Color ' The color to be used with the title text (if IsWithTitle is set to true)
    Private IsWithPaging As Boolean ' Determine if paging is used
    Private Shared CurrentRow As Integer = 0 ' A static parameter that keep track on which Row (in the DataGridView control) that should be printed
    Private Shared PageNumber As Integer
    Private PageWidth As Integer
    Private PageHeight As Integer
    Private LeftMargin As Integer
    Private TopMargin As Integer
    Private RightMargin As Integer
    Private BottomMargin As Integer
    Private CurrentY As Single ' A parameter that keep track on the y coordinate of the page, so the next object to be printed will start from this y coordinate
    Private RowHeaderHeight As Single
    Private RowsHeight As ArrayList
    Private ColumnsWidth As ArrayList
    Private TheDataGridViewWidth As Single
    '======================================
#Region "Footer Printing"
    'Footer
    Private _CurrentPage As Integer
    Private _PrintFont As New Font(System.Drawing.FontFamily.GenericSansSerif, 9)
    Private _FooterFont As New Font(System.Drawing.FontFamily.GenericSansSerif, 10)
    Private _FooterRectangle As Rectangle
    Private _Textlayout As System.Drawing.StringFormat
    Private _FooterHeightPercent As Integer = 3
    Private _CellGutter As Integer = 5
    Private _FooterPen As New Pen(Color.Green)
    Private _GridPen As New Pen(Color.Black)

    Private Sub PrintFooter(ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        _FooterRectangle = e.MarginBounds

        _FooterRectangle.Height = e.MarginBounds.Height * (_FooterHeightPercent * 0.01)
        'make rectangle height a little larger
        '_FooterRectangle.Height = e.MarginBounds.Height * (_FooterHeightPercent * 0.0125)
        'text appears to be more centered with this approach
        _FooterRectangle.Height = 5 + e.MarginBounds.Height * (_FooterHeightPercent * 0.01)

        _FooterRectangle.Y += (e.MarginBounds.Height * (1 - (0.01 * _FooterHeightPercent)))

        If _FooterRectangle.Height > 0 Then
            e.Graphics.DrawRectangle(_FooterPen, _FooterRectangle)
            Call DrawCellString("iDebate.org", CellTextHorizontalAlignment.LeftAlign, CellTextVerticalAlignment.MiddleAlign, _FooterRectangle, False, e.Graphics, _PrintFont)
            '            Call DrawCellString(DateTime.Now.ToLongDateString, CellTextHorizontalAlignment.CentreAlign, CellTextVerticalAlignment.MiddleAlign, _FooterRectangle, False, e.Graphics, _PrintFont)
            Call DrawCellString("Footer - Middle", CellTextHorizontalAlignment.CentreAlign, CellTextVerticalAlignment.MiddleAlign, _FooterRectangle, False, e.Graphics, _PrintFont)
            '            Call DrawCellString("Page " & _CurrentPage.ToString, CellTextHorizontalAlignment.RightAlign, CellTextVerticalAlignment.MiddleAlign, _FooterRectangle, False, e.Graphics, _PrintFont)
            Call DrawCellString("Footer - Right ", CellTextHorizontalAlignment.RightAlign, CellTextVerticalAlignment.MiddleAlign, _FooterRectangle, False, e.Graphics, _PrintFont)
        End If
    End Sub

    Public Function DrawCellString(ByVal s As String, _
                              ByVal HorizontalAlignment As CellTextHorizontalAlignment, _
                              ByVal VerticalAlignment As CellTextVerticalAlignment, _
                              ByVal BoundingRect As Rectangle, _
                              ByVal DrawRectangle As Boolean, _
                              ByVal Target As Graphics, _
                              ByVal PrintFont As Font)
        If _Textlayout Is Nothing Then
            _Textlayout = New System.Drawing.StringFormat
            _Textlayout.Trimming = StringTrimming.EllipsisCharacter
        End If

        Dim x As Single = 0, y As Single = 0
        If DrawRectangle Then
            Target.DrawRectangle(_GridPen, BoundingRect)
        End If

        '\\ Set the text alignment
        If HorizontalAlignment = CellTextHorizontalAlignment.LeftAlign Then
            ' x = BoundingRect.Left + _CellGutter
            _Textlayout.Alignment = StringAlignment.Near
        ElseIf HorizontalAlignment = CellTextHorizontalAlignment.RightAlign Then
            'x = BoundingRect.Right - _CellGutter - Target.MeasureString(s, PrintFont).Width
            _Textlayout.Alignment = StringAlignment.Far
        Else
            'x = BoundingRect.Left + _CellGutter + ((BoundingRect.Width / 2) - (Target.MeasureString(s, PrintFont).Width / 2))
            _Textlayout.Alignment = StringAlignment.Center
        End If

        Dim BoundingRectF As New RectangleF(BoundingRect.X + _CellGutter, BoundingRect.Y + _CellGutter, BoundingRect.Width - (2 * _CellGutter), BoundingRect.Height - (2 * _CellGutter))
        Target.DrawString(s, PrintFont, System.Drawing.Brushes.Black, BoundingRectF, _Textlayout)
        Return Nothing

    End Function

    Public Property FooterHeightPercent() As Integer
        Get
            Return _FooterHeightPercent
        End Get
        Set(ByVal Value As Integer)
            If Value < 0 OrElse Value >= 30 Then
                Throw New ArgumentException("FooterHeightPercent must be between 0 and 30")
            End If
            _FooterHeightPercent = Value
        End Set
    End Property

    Public Property GridPen() As Pen
        Get
            Return _GridPen
        End Get
        Set(ByVal Value As Pen)
            _GridPen = Value
        End Set
    End Property

    Public Property FooterPen() As Pen
        Get
            Return _FooterPen
        End Get
        Set(ByVal Value As Pen)
            _FooterPen = Value
        End Set
    End Property
    Public Property FooterFont() As Font
        Get
            Return _FooterFont
        End Get
        Set(ByVal Value As Font)
            _FooterFont = Value
        End Set
    End Property

    Public Enum CellTextHorizontalAlignment
        LeftAlign = 1
        CentreAlign = 2
        RightAlign = 3
    End Enum

    Public Enum CellTextVerticalAlignment
        TopAlign = 1
        MiddleAlign = 2
        BottomAlign = 3
    End Enum

#End Region 'Footer Printing

    '========================================
    ' Maintain a generic list to hold start/stop points for the column printing
    ' This will be used for wrapping in situations where the DataGridView will not fit on a single page
    Private mColumnPoints As ArrayList
    Private mColumnPointsWidth As ArrayList
    Private mColumnPoint As Integer

    'The class constructor.
    Public Sub New(ByRef aDataGridView As DataGridView, ByRef aPrintDocument As PrintDocument, ByVal CenterOnPage As Boolean, ByVal WithTitle As Boolean, ByVal aTitleText As String, ByVal aTitleFont As Font, ByVal aTitleColor As Color, ByVal WithPaging As Boolean)
        TheDataGridView = aDataGridView
        ThePrintDocument = aPrintDocument
        IsCenterOnPage = CenterOnPage
        IsWithTitle = WithTitle
        TheTitleText = aTitleText
        TheTitleFont = aTitleFont
        TheTitleColor = aTitleColor
        IsWithPaging = WithPaging

        PageNumber = 0

        RowsHeight = New ArrayList
        ColumnsWidth = New ArrayList

        mColumnPoints = New ArrayList
        mColumnPointsWidth = New ArrayList

        'Calculating the PageWidth and the PageHeight.
        If Not ThePrintDocument.DefaultPageSettings.Landscape Then
            PageWidth = ThePrintDocument.DefaultPageSettings.PaperSize.Width
            PageHeight = ThePrintDocument.DefaultPageSettings.PaperSize.Height
        Else
            PageHeight = ThePrintDocument.DefaultPageSettings.PaperSize.Width
            PageWidth = ThePrintDocument.DefaultPageSettings.PaperSize.Height
        End If

        'Calculating the page margins.
        LeftMargin = ThePrintDocument.DefaultPageSettings.Margins.Left
        TopMargin = ThePrintDocument.DefaultPageSettings.Margins.Top
        RightMargin = ThePrintDocument.DefaultPageSettings.Margins.Right
        BottomMargin = ThePrintDocument.DefaultPageSettings.Margins.Bottom

        'First, the current row to be printed is the first row in the DataGridView control.
        CurrentRow = 0
    End Sub

    ' The function that calculate the height of each row (including the header row), the width of each column (according to the longest text in all its cells including the header cell), and the whole DataGridView width
    Private Sub Calculate(ByVal g As Graphics)
        If PageNumber = 0 Then ' Just calculate once
            PageNumber = 1
            Dim tmpSize As SizeF = New SizeF()
            Dim tmpFont As Font
            Dim tmpWidth As Single
            Dim i As Integer
            Dim j As Integer

            TheDataGridViewWidth = 0
            For i = 0 To TheDataGridView.Columns.Count - 1
                tmpFont = TheDataGridView.ColumnHeadersDefaultCellStyle.Font
                If tmpFont Is Nothing Then ' If there is no special HeaderFont style, then use the default DataGridView font style
                    tmpFont = TheDataGridView.DefaultCellStyle.Font
                End If
                tmpSize = g.MeasureString(TheDataGridView.Columns(i).HeaderText, tmpFont)
                tmpWidth = tmpSize.Width
                RowHeaderHeight = tmpSize.Height

                For j = 0 To TheDataGridView.Rows.Count - 1
                    tmpFont = TheDataGridView.Rows(j).DefaultCellStyle.Font
                    If tmpFont Is Nothing Then ' If the there is no special font style of the CurrentRow, then use the default one associated with the DataGridView control
                        tmpFont = TheDataGridView.DefaultCellStyle.Font
                    End If

                    tmpSize = g.MeasureString("Anything", tmpFont)
                    RowsHeight.Add(tmpSize.Height)

                    tmpSize = g.MeasureString(TheDataGridView.Rows(j).Cells(i).EditedFormattedValue.ToString(), tmpFont)
                    If (tmpSize.Width > tmpWidth) Then
                        tmpWidth = tmpSize.Width
                    End If
                Next
                If TheDataGridView.Columns(i).Visible Then
                    TheDataGridViewWidth += tmpWidth
                End If
                ColumnsWidth.Add(tmpWidth)
            Next

            ' Define the start/stop column points based on the page width and the DataGridView Width
            ' We will use this to determine the columns which are drawn on each page and how wrapping will be handled
            ' By default, the wrapping will occurr such that the maximum number of columns for a page will be determine
            Dim k As Integer

            Dim mStartPoint As Integer = 0
            For k = 0 To TheDataGridView.Columns.Count - 1
                If TheDataGridView.Columns(k).Visible Then
                    mStartPoint = k
                    Exit For
                End If
            Next
            Dim mEndPoint As Integer = TheDataGridView.Columns.Count
            For k = TheDataGridView.Columns.Count - 1 To 0 Step -1
                If TheDataGridView.Columns(k).Visible Then
                    mEndPoint = k + 1
                    Exit For
                End If
            Next

            Dim mTempWidth As Single = TheDataGridViewWidth
            Dim mTempPrintArea As Single = CType(PageWidth, Single) - CType(LeftMargin, Single) - CType(RightMargin, Single)

            ' We only care about handling where the total datagridview width is bigger then the print area
            If TheDataGridViewWidth > mTempPrintArea Then
                mTempWidth = 0.0
                For k = 0 To TheDataGridView.Columns.Count - 1
                    If TheDataGridView.Columns(k).Visible Then
                        mTempWidth += CType(ColumnsWidth(k), Single)

                        ' If the width is bigger than the page area, then define a new column print range
                        If (mTempWidth > mTempPrintArea) Then
                            mTempWidth -= CType(ColumnsWidth(k), Single)
                            mColumnPoints.Add(New Integer() {mStartPoint, mEndPoint})
                            mColumnPointsWidth.Add(mTempWidth)
                            mStartPoint = k
                            mTempWidth = CType(ColumnsWidth(k), Single)
                        End If
                    End If
                    ' Our end point is actually one index above the current index
                    mEndPoint = k + 1
                Next
            End If
            ' Add the last set of columns
            mColumnPoints.Add(New Integer() {mStartPoint, mEndPoint})
            mColumnPointsWidth.Add(mTempWidth)
            mColumnPoint = 0
        End If
    End Sub

    'The funtion that print the title and the header row.
    'The funtion that print the title, page number, and the header row
    Private Sub DrawHeader(ByVal g As Graphics)
        CurrentY = CType(TopMargin, Single)

        ' Printing the page number (if isWithPaging is set to true)
        If IsWithPaging Then
            Dim PageString As String = "Page " + PageNumber.ToString()
            Dim PageStringFormat As StringFormat = New StringFormat()
            PageStringFormat.Trimming = StringTrimming.Word
            PageStringFormat.FormatFlags = StringFormatFlags.NoWrap Or StringFormatFlags.LineLimit Or StringFormatFlags.NoClip
            PageStringFormat.Alignment = StringAlignment.Far

            Dim PageStringFont As Font = New Font("Tahoma", 8, FontStyle.Regular, GraphicsUnit.Point)
            Dim PageStringRectangle As RectangleF = New RectangleF(CType(LeftMargin, Single), CurrentY, CType(PageWidth, Single) - CType(RightMargin, Single) - CType(LeftMargin, Single), g.MeasureString(PageString, PageStringFont).Height)
            g.DrawString(PageString, PageStringFont, New SolidBrush(Color.Black), PageStringRectangle, PageStringFormat)
            CurrentY += g.MeasureString(PageString, PageStringFont).Height
        End If
        PageNumber += 1

        ' Printing the title (if IsWithTitle is set to true)
        If IsWithTitle Then
            Dim TitleFormat As StringFormat = New StringFormat()
            TitleFormat.Trimming = StringTrimming.Word
            TitleFormat.FormatFlags = StringFormatFlags.NoWrap Or StringFormatFlags.LineLimit Or StringFormatFlags.NoClip
            If IsCenterOnPage Then
                TitleFormat.Alignment = StringAlignment.Center
            Else
                TitleFormat.Alignment = StringAlignment.Near
            End If

            Dim TitleRectangle As RectangleF = New RectangleF(CType(LeftMargin, Single), CurrentY, CType(PageWidth, Single) - CType(RightMargin, Single) - CType(LeftMargin, Single), g.MeasureString(TheTitleText, TheTitleFont).Height)
            g.DrawString(TheTitleText, TheTitleFont, New SolidBrush(TheTitleColor), TitleRectangle, TitleFormat)
            CurrentY += g.MeasureString(TheTitleText, TheTitleFont).Height
        End If

        ' Calculating the starting x coordinate that the printing process will start from
        Dim CurrentX As Single = CType(LeftMargin, Single)
        If IsCenterOnPage Then
            CurrentX += ((CType(PageWidth, Single) - CType(RightMargin, Single) - CType(LeftMargin, Single)) - mColumnPointsWidth(mColumnPoint)) / 2.0
        End If
        ' Setting the HeaderFore style
        Dim HeaderForeColor As Color = TheDataGridView.ColumnHeadersDefaultCellStyle.ForeColor
        If HeaderForeColor.IsEmpty Then ' If there is no special HeaderFore style, then use the default DataGridView style
            HeaderForeColor = TheDataGridView.DefaultCellStyle.ForeColor
        End If
        Dim HeaderForeBrush As SolidBrush = New SolidBrush(HeaderForeColor)

        ' Setting the HeaderBack style
        Dim HeaderBackColor As Color = TheDataGridView.ColumnHeadersDefaultCellStyle.BackColor
        If (HeaderBackColor.IsEmpty) Then ' If there is no special HeaderBack style, then use the default DataGridView style
            HeaderBackColor = TheDataGridView.DefaultCellStyle.BackColor
        End If

        Dim HeaderBackBrush As SolidBrush = New SolidBrush(HeaderBackColor)
        ' Setting the LinePen that will be used to draw lines and rectangles (derived from the GridColor property of the DataGridView control)
        Dim TheLinePen As Pen = New Pen(TheDataGridView.GridColor, 1)
        ' Setting the HeaderFont style
        Dim HeaderFont As Font = TheDataGridView.ColumnHeadersDefaultCellStyle.Font

        If HeaderFont Is Nothing Then ' If there is no special HeaderFont style, then use the default DataGridView font style
            HeaderFont = TheDataGridView.DefaultCellStyle.Font
        End If
        ' Calculating and drawing the HeaderBounds        

        Dim HeaderBounds As RectangleF = New RectangleF(CurrentX, CurrentY, CType(mColumnPointsWidth(mColumnPoint), Single), RowHeaderHeight)
        g.FillRectangle(HeaderBackBrush, HeaderBounds)
        ' Setting the format that will be used to print each cell of the header row
        Dim CellFormat As StringFormat = New StringFormat()

        CellFormat.Trimming = StringTrimming.Word
        CellFormat.FormatFlags = StringFormatFlags.NoWrap Or StringFormatFlags.LineLimit Or StringFormatFlags.NoClip

        ' Printing each visible cell of the header row
        Dim CellBounds As RectangleF
        Dim ColumnWidth As Single
        Dim i As Integer
        For i = CType(mColumnPoints(mColumnPoint).GetValue(0), Integer) To CType(mColumnPoints(mColumnPoint).GetValue(1), Integer) - 1
            If Not TheDataGridView.Columns(i).Visible Then Continue For ' If the column is not visible then ignore this iteration

            ColumnWidth = CType(ColumnsWidth(i), Single)

            ' Check the CurrentCell alignment and apply it to the CellFormat
            If (TheDataGridView.ColumnHeadersDefaultCellStyle.Alignment.ToString().Contains("Right")) Then
                CellFormat.Alignment = StringAlignment.Far
            ElseIf (TheDataGridView.ColumnHeadersDefaultCellStyle.Alignment.ToString().Contains("Center")) Then
                CellFormat.Alignment = StringAlignment.Center
            Else
                CellFormat.Alignment = StringAlignment.Near
            End If
            CellBounds = New RectangleF(CurrentX, CurrentY, ColumnWidth, RowHeaderHeight)

            ' Printing the cell text
            g.DrawString(TheDataGridView.Columns(i).HeaderText, HeaderFont, HeaderForeBrush, CellBounds, CellFormat)

            ' Drawing the cell bounds
            If TheDataGridView.RowHeadersBorderStyle <> DataGridViewHeaderBorderStyle.None Then ' Draw the cell border only if the HeaderBorderStyle is not None
                g.DrawRectangle(TheLinePen, CurrentX, CurrentY, ColumnWidth, RowHeaderHeight)
            End If

            CurrentX += ColumnWidth
        Next

        CurrentY += RowHeaderHeight
    End Sub

    ' The function that print a bunch of rows that fit in one page
    ' When it returns true, meaning that there are more rows still not printed, so another PagePrint action is required
    ' When it returns false, meaning that all rows are printed (the CureentRow parameter reaches the last row of the DataGridView control) and no further PagePrint action is required
    Private Function DrawRows(ByVal g As Graphics) As Boolean
        ' Setting the LinePen that will be used to draw lines and rectangles (derived from the GridColor property of the DataGridView control)
        Dim TheLinePen As Pen = New Pen(TheDataGridView.GridColor, 1)

        ' The style paramters that will be used to print each cell
        Dim RowFont As Font
        Dim RowForeColor As Color
        Dim RowBackColor As Color
        Dim RowForeBrush As SolidBrush
        Dim RowBackBrush As SolidBrush
        Dim RowAlternatingBackBrush As SolidBrush

        ' Setting the format that will be used to print each cell
        Dim CellFormat As StringFormat = New StringFormat()
        CellFormat.Trimming = StringTrimming.Word
        CellFormat.FormatFlags = StringFormatFlags.NoWrap Or StringFormatFlags.LineLimit

        ' Printing each visible cell
        Dim RowBounds As RectangleF
        Dim CurrentX As Single
        Dim ColumnWidth As Single
        While CurrentRow < TheDataGridView.Rows.Count
            If TheDataGridView.Rows(CurrentRow).Visible Then ' Print the cells of the CurrentRow only if that row is visible
                ' Setting the row font style
                RowFont = TheDataGridView.Rows(CurrentRow).DefaultCellStyle.Font
                If RowFont Is Nothing Then ' If the there is no special font style of the CurrentRow, then use the default one associated with the DataGridView control
                    RowFont = TheDataGridView.DefaultCellStyle.Font
                End If

                ' Setting the RowFore style
                RowForeColor = TheDataGridView.Rows(CurrentRow).DefaultCellStyle.ForeColor
                If RowForeColor.IsEmpty Then ' If the there is no special RowFore style of the CurrentRow, then use the default one associated with the DataGridView control
                    RowForeColor = TheDataGridView.DefaultCellStyle.ForeColor
                End If
                RowForeBrush = New SolidBrush(RowForeColor)

                ' Setting the RowBack (for even rows) and the RowAlternatingBack (for odd rows) styles
                RowBackColor = TheDataGridView.Rows(CurrentRow).DefaultCellStyle.BackColor
                If RowBackColor.IsEmpty Then ' If the there is no special RowBack style of the CurrentRow, then use the default one associated with the DataGridView control
                    RowBackBrush = New SolidBrush(TheDataGridView.DefaultCellStyle.BackColor)
                    RowAlternatingBackBrush = New SolidBrush(TheDataGridView.AlternatingRowsDefaultCellStyle.BackColor)
                Else ' If the there is a special RowBack style of the CurrentRow, then use it for both the RowBack and the RowAlternatingBack styles
                    RowBackBrush = New SolidBrush(RowBackColor)
                    RowAlternatingBackBrush = New SolidBrush(RowBackColor)
                End If
                ' Calculating the starting x coordinate that the printing process will start from
                CurrentX = CType(LeftMargin, Single)
                If (IsCenterOnPage) Then
                    CurrentX += ((CType(PageWidth, Single) - CType(RightMargin, Single) - CType(LeftMargin, Single)) - (mColumnPointsWidth(mColumnPoint))) / 2.0
                End If
                ' Calculating the entire CurrentRow bounds                
                RowBounds = New RectangleF(CurrentX, CurrentY, CType(mColumnPointsWidth(mColumnPoint), Single), CType(RowsHeight(CurrentRow), Single))

                ' Filling the back of the CurrentRow
                If CurrentRow Mod 2 = 0 Then
                    g.FillRectangle(RowBackBrush, RowBounds)
                Else
                    g.FillRectangle(RowAlternatingBackBrush, RowBounds)
                End If
                ' Printing each visible cell of the CurrentRow
                Dim CurrentCell As Integer
                For CurrentCell = CType(mColumnPoints(mColumnPoint).GetValue(0), Integer) To CType(mColumnPoints(mColumnPoint).GetValue(1), Integer) - 1
                    If Not TheDataGridView.Columns(CurrentCell).Visible Then Continue For ' If the cell is belong to invisible column, then ignore this iteration

                    ' Check the CurrentCell alignment and apply it to the CellFormat
                    If TheDataGridView.Columns(CurrentCell).DefaultCellStyle.Alignment.ToString().Contains("Right") Then
                        CellFormat.Alignment = StringAlignment.Far
                    ElseIf TheDataGridView.Columns(CurrentCell).DefaultCellStyle.Alignment.ToString().Contains("Center") Then
                        CellFormat.Alignment = StringAlignment.Center
                    Else
                        CellFormat.Alignment = StringAlignment.Near
                    End If

                    ColumnWidth = CType(ColumnsWidth(CurrentCell), Single)
                    Dim CellBounds As RectangleF = New RectangleF(CurrentX, CurrentY, ColumnWidth, CType(RowsHeight(CurrentRow), Single))

                    ' Printing the cell text
                    g.DrawString(TheDataGridView.Rows(CurrentRow).Cells(CurrentCell).EditedFormattedValue.ToString(), RowFont, RowForeBrush, CellBounds, CellFormat)

                    ' Drawing the cell bounds
                    If TheDataGridView.CellBorderStyle <> DataGridViewCellBorderStyle.None Then ' Draw the cell border only if the CellBorderStyle is not None
                        g.DrawRectangle(TheLinePen, CurrentX, CurrentY, ColumnWidth, RowsHeight(CurrentRow))
                    End If
                    CurrentX += ColumnWidth
                Next
                CurrentY += CType(RowsHeight(CurrentRow), Single)

                ' Checking if the CurrentY is exceeds the page boundries
                ' If so then exit the function and returning true meaning another PagePrint action is required
                If CType(CurrentY, Integer) > (PageHeight - TopMargin - BottomMargin) Then
                    CurrentRow += 1
                    Return True
                End If
            End If
            CurrentRow += 1
        End While

        CurrentRow = 0
        mColumnPoint += 1 ' Continue to print the next group of columns

        If mColumnPoint = mColumnPoints.Count Then ' Which means all columns are printed
            mColumnPoint = 0
            PageNumber = 1
            Return False
        Else
            Return True
        End If
    End Function

    'The method that calls the functions that print the Title (if IsWithTitle is set to true), header row, and bunch of rows that fit in one page.
    '    Public Function DrawDataGridView(ByRef g As Graphics) As Boolean
    Public Function DrawDataGridView(ByRef g As System.Drawing.Printing.PrintPageEventArgs) As Boolean

        'System.Drawing.Printing.PrintPageEventArgs
        Try
            Calculate(g.Graphics)
            DrawHeader(g.Graphics)
            Dim bContinue As Boolean = DrawRows(g.Graphics)
            PrintFooter(g)
            '============================================================
            'limit number of pages to print
            '            If PageNumber >= 1 Then Exit Function
            'If PageNumber > 1 Then Return bContinue 'Exit Function
            '============================================================

            Return bContinue
        Catch ex As Exception
            MessageBox.Show("Operation failed: " & ex.Message.ToString(), Application.ProductName + " - Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try

    End Function
End Class

