Public Class frmSchoolEntry
    Dim ds As New DataSet

    Private Sub frmSchoolEntry_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LoadFile(ds, "TourneyData")
        ds.Tables("School").DefaultView.Sort = "Schoolname ASC"

        DataGridView1.AutoGenerateColumns = False
        DataGridView1.DataSource = ds.Tables("School")
    End Sub
    Private Sub frmSchoolEntry_Unload(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Disposed
        'delete the ENTERED column if it was there
        If ds.Tables("School").Columns.Contains("Entered") = True Then ds.Tables("School").Columns.Remove("Entered")
        ds.Tables("School").DefaultView.RowFilter = Nothing
        Call SaveFile(ds)
        ds.Dispose()
    End Sub

    Private Sub butRemakeAcros_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butRemakeAcros.Click
        'Updates the Code (acronym) field for all entered teams
        Dim x As Integer
        For x = 0 To ds.Tables("Entry").Rows.Count - 1
            ds.Tables("Entry").Rows(x).Item("Code") = FullNameMaker(ds, ds.Tables("Entry").Rows(x).Item("ID"), "ACRO", 1)
        Next x
        Call DupeAcroCheck(ds)
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        'Updates the FullName field for all entered teams
        Dim x As Integer
        For x = 0 To ds.Tables("Entry").Rows.Count - 1
            ds.Tables("Entry").Rows(x).Item("Fullname") = FullNameMaker(ds, ds.Tables("Entry").Rows(x).Item("ID"), "FULL", 1)
        Next x
    End Sub

    Private Sub butLimitToEntered_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butLimitToEntered.Click
        'add a column that you'll delete on unload and filter by it
        ds.Tables("School").Columns.Add("Entered", System.Type.GetType("System.Boolean"))
        'mark it
        Dim fdEntry As DataRow()
        For x = 0 To ds.Tables("School").Rows.Count - 1
            'check for team entries
            fdEntry = ds.Tables("Entry").Select("School=" & ds.Tables("School").Rows(x).Item("ID"))
            If fdEntry.Length > 0 Then ds.Tables("School").Rows(x).Item("Entered") = True
            'check for judge entries
            fdEntry = ds.Tables("Judge").Select("School=" & ds.Tables("School").Rows(x).Item("ID"))
            If fdEntry.Length > 0 Then ds.Tables("School").Rows(x).Item("Entered") = True
            'check for competitor entries
            fdEntry = ds.Tables("Entry_Student").Select("School=" & ds.Tables("School").Rows(x).Item("ID"))
            If fdEntry.Length > 0 Then ds.Tables("School").Rows(x).Item("Entered") = True
        Next x
        ds.Tables("School").DefaultView.RowFilter = "Entered=true"

    End Sub

    Private Sub butPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butPrint.Click
        Dim defCols(DataGridView1.Columns.Count) As Boolean
        For x = 0 To DataGridView1.ColumnCount - 1
            If x = 1 Then defCols(x) = True
        Next x
        Dim frm As New frmPrint(DataGridView1, ds.Tables("TOURN").Rows(0).Item("TournName") & Chr(10) & "Schools in Attendance", defCols)
        frm.ShowDialog()
    End Sub

    Private Sub butHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butHelp.Click
        Dim strInfo As String = ""
        strInfo &= "This screen will make acronyms and full team names with school information.  For example, if a team of Seaver and Grote are entered for CSU Fullerton, you might want an acronym of 'CSUF SG'." & Chr(10) & Chr(10)
        strInfo &= "On this screen, you can edit full team names and short team names, and by clicking the appropriate buttons automatically update all teams at the tournament." & Chr(10) & Chr(10)
        strInfo &= "The procedures will automatically eliminate any duplicates, so you shouldn't have duplicate names appearing on the pairings." & Chr(10) & Chr(10)
        strInfo &= "If the list of teams is especially long, clicking the 'limit list to schools currently entered' button will eliminate school records for which there are no team or judge entries." & Chr(10) & Chr(10)
        strInfo &= "Clicking the PRINT button will print a full list of teams at the tournament, either for general release or to give to judges so they can indicate conflicts." & Chr(10) & Chr(10)
        MsgBox(strInfo)
    End Sub
End Class