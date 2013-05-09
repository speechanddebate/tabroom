Public Class frmEntriesBySchool
    Dim ds As New DataSet

    Private Sub frmUtilities_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Load
        LoadFile(ds, "TourneyData")

        Call LoadGrid()

        'Show judge situation; ugly but functional

        ds.Tables("Judge").Columns.Add("Already", System.Type.GetType("System.Int64"))
        For x = 0 To ds.Tables("Judge").Rows.Count - 1
            ds.Tables("Judge").Rows(x).Item("Already") = GetRoundsJudged(ds, ds.Tables("Judge").Rows(x).Item("ID"))
        Next x

        Dim dt As New DataTable
        dt = JudgeSituation(ds)
        DataGridView2.DataSource = dt

        If ds.Tables("Judge").Columns.Contains("Already") = True Then
            ds.Tables("Judge").Columns.Remove("Already")
        End If

    End Sub
    Sub LoadGrid()
        Dim dt As New DataTable
        dt.Columns.Add("School", System.Type.GetType("System.String"))
        For x = 0 To ds.Tables("Event").Rows.Count - 1
            dt.Columns.Add(ds.Tables("Event").Rows(x).Item("ABBR"), System.Type.GetType("System.Int64"))
            dt.Columns.Add("Owe " & ds.Tables("Event").Rows(x).Item("ABBR"), System.Type.GetType("System.Int64"))
        Next x
        dt.Columns.Add("TotalOwed", System.Type.GetType("System.Int64"))
        dt.Columns.Add("RoundsProvided", System.Type.GetType("System.Int64"))
        dt.Columns.Add("Balance", System.Type.GetType("System.Int64"))

        Dim dr As DataRow
        Dim fdEntries, fdJudges As DataRow()
        Dim RdsOwed, RdsGot, RdsOwedBySchool As Integer
        Dim TotOwed, TotGot As Integer
        For x = 0 To ds.Tables("School").Rows.Count - 1
            dr = dt.NewRow
            RdsOwedBySchool = 0
            For z = 0 To ds.Tables("Event").Rows.Count - 1
                RdsOwed = 0 : RdsGot = 0
                dr.Item("School") = ds.Tables("School").Rows(x).Item("SchoolName")
                fdEntries = ds.Tables("Entry").Select("School=" & ds.Tables("School").Rows(x).Item("ID") & " and event=" & ds.Tables("Event").Rows(z).Item("ID"))
                dr.Item(ds.Tables("Event").Rows(z).Item("Abbr")) = fdEntries.Count
                For y = 0 To fdEntries.Length - 1
                    RdsOwed += getEventSetting(ds, fdEntries(y).Item("Event"), "nPrelims") / getEventSetting(ds, fdEntries(y).Item("Event"), "TeamsPerRound")
                Next y
                dr.Item("Owe " & ds.Tables("Event").Rows(z).Item("Abbr")) = RdsOwed
                RdsOwedBySchool += RdsOwed
                TotOwed += RdsOwed : TotGot += RdsGot
            Next z
            fdJudges = ds.Tables("Judge").Select("School=" & ds.Tables("School").Rows(x).Item("ID"))
            For y = 0 To fdJudges.Length - 1
                RdsGot += fdJudges(y).Item("Obligation") + fdJudges(y).Item("Hired")
            Next y
            dr.Item("TotalOwed") = RdsOwedBySchool
            dr.Item("RoundsProvided") = RdsGot
            dr.Item("Balance") = dr.Item("RoundsProvided") - dr.Item("TotalOwed")
            dt.Rows.Add(dr)
        Next x
        DataGridView1.DataSource = dt
        DataGridView1.Columns("School").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        TextBox1.Text = TotOwed & " total rounds needed.  " & Chr(10) & Chr(13)
        TextBox1.Text &= TotGot & " total rounds available.  " & Chr(10) & Chr(13)
        TextBox1.Text &= "Total balance is: " & TotGot - TotOwed
        Call ShowProblemChildren()
    End Sub
    Sub ShowProblemChildren()
        For x = 0 To DataGridView1.RowCount - 2
            DataGridView1.Rows(x).Selected = False
            For y = 1 To DataGridView1.ColumnCount - 1
                If DataGridView1.Rows(x).Cells("Balance").Value < 0 Then DataGridView1.Item(y, x).Style.BackColor = Color.Red
            Next y
        Next x
    End Sub
End Class