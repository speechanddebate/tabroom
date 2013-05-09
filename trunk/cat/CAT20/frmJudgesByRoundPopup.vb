Public Class frmJudgesByRoundPopup
    Dim ds As New DataSet
    Private Sub frmShowPairings_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LoadFile(ds, "TourneyData")

        Dim dt As New DataTable
        dt.Columns.Add("Round", System.Type.GetType("System.String"))
        dt.Columns.Add("Needed", System.Type.GetType("System.Int16"))
        dt.Columns.Add("Available", System.Type.GetType("System.Int16"))
        dt.Columns.Add("Balance", System.Type.GetType("System.Int16"))

        Dim dr, drDummy As DataRow : Dim fdTeams As DataRow()
        Dim JudgesPerDebate, TeamsPerDebate, TotAvail As Integer
        Dim OK As Boolean
        For x = 0 To ds.Tables("Round").Rows.Count - 1
            If ds.Tables("Round").Rows(x).Item("Rd_Name") <= 9 Then
                dr = dt.NewRow : dr.Item("Round") = ds.Tables("Round").Rows(x).Item("Label")
                drDummy = ds.Tables("Round").Rows.Find(ds.Tables("Round").Rows(x).Item("ID"))
                JudgesPerDebate = drDummy.Item("JudgesPerPanel")
                TeamsPerDebate = getEventSetting(ds, drDummy.Item("Event"), "TeamsPerRound")
                fdTeams = ds.Tables("Entry").Select("Event=" & drDummy.Item("Event"))
                dr.Item("Needed") = (Int(fdTeams.Length) / TeamsPerDebate) * JudgesPerDebate
                TotAvail = 0
                For y = 0 To ds.Tables("Judge").Rows.Count - 1
                    OK = True
                    If ds.Tables("Judge").Rows(y).Item("Event" & drDummy.Item("Event")) = False Then OK = False
                    If ds.Tables("Judge").Rows(y).Item("Timeslot" & drDummy.Item("timeslot")) = False Then OK = False
                    If OK = True Then TotAvail += 1
                Next y
                dr.Item("Available") = TotAvail
                dt.Rows.Add(dr)
            End If
        Next
        DataGridView1.DataSource = dt
        For x = 0 To DataGridView1.RowCount - 1
            DataGridView1.Rows(x).Cells("Balance").Value = DataGridView1.Rows(x).Cells("Available").Value - DataGridView1.Rows(x).Cells("Needed").Value
            If DataGridView1.Rows(x).Cells("Balance").Value < 0 Then
                DataGridView1.Item(3, x).Style.BackColor = Color.Red
            ElseIf DataGridView1.Rows(x).Cells("Balance").Value = 0 Then
                DataGridView1.Item(3, x).Style.BackColor = Color.Yellow
            End If
        Next x

    End Sub

End Class