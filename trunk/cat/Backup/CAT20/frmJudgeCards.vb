Public Class frmJudgeCards
    Dim ds As New DataSet

    Private Sub frmJudgeCards_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Load
        LoadFile(ds, "TourneyData")

        Dim dt2 As New DataTable
        dt2 = ds.Tables("Judge").Copy
        dt2.Columns.Add(("JudgeName"), System.Type.GetType("System.String"))
        For x = 0 To dt2.Rows.Count - 1
            dt2.Rows(x).Item("JudgeName") = dt2.Rows(x).Item("Last").trim & ", " & dt2.Rows(x).Item("First").trim
        Next
        dt2.DefaultView.Sort = "JudgeName"
        cboJudges.DataSource = dt2
        cboJudges.ValueMember = "ID"
        cboJudges.DisplayMember = "JudgeName"

    End Sub
    Private Sub butShowCard_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butShowCard.Click
        Dim dt As New DataTable
        dt.Columns.Add("RoundID", System.Type.GetType("System.Int64"))
        dt.Columns.Add("Round", System.Type.GetType("System.String"))
        dt.Columns.Add("Aff", System.Type.GetType("System.String"))
        dt.Columns.Add("Neg", System.Type.GetType("System.String"))
        dt.Columns.Add("Room", System.Type.GetType("System.String"))
        dt.Columns.Add("Points", System.Type.GetType("System.String"))
        dt.Columns.Add("Decision", System.Type.GetType("System.String"))

        Call PopulateTable(dt)
        DataGridView1.DataSource = dt
        DataGridView1.Columns("RoundID").Visible = False
        DataGridView1.Columns("Points").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
    End Sub
    Sub PopulateTable(ByRef DT As DataTable)
        Dim fdBallots, fdScores As DataRow()
        fdBallots = ds.Tables("Ballot").Select("Judge=" & cboJudges.SelectedValue, "Panel ASC")
        Dim dr, dr2, drPanel, drRound, drRoom, drDebater As DataRow
        Dim strName, strPts As String : strPts = ""
        dr = DT.NewRow
        For x = 0 To fdBallots.Length - 1
            'if the panel is over, end the row and get a new one
            If x > 0 Then
                If fdBallots(x).Item("Panel") <> fdBallots(x - 1).Item("Panel") Then
                    dr.Item("Points") = Mid(dr.Item("Points"), 1, dr.Item("Points").length - 2)
                    DT.Rows.Add(dr)
                    dr = DT.NewRow
                End If
            End If
            'otherwise, do ballots
            dr2 = ds.Tables("Entry").Rows.Find(fdBallots(x).Item("Entry"))
            dr.Item(GetSideString(ds, fdBallots(x).Item("Side"), dr2.Item("Event"))) = dr2.Item("Code")
            drPanel = ds.Tables("Panel").Rows.Find(fdBallots(x).Item("Panel"))
            drRound = ds.Tables("Round").Rows.Find(drPanel.Item("Round"))
            dr.Item("RoundID") = drRound.Item("ID")
            dr.Item("Round") = drRound.Item("Label")
            drRoom = ds.Tables("Room").Rows.Find(drPanel.Item("Room"))
            dr.Item("Room") = drRoom.Item("RoomName")
            fdScores = ds.Tables("Ballot_Score").Select("Ballot=" & fdBallots(x).Item("ID") & "and Score_ID=1")
            For y = 0 To fdScores.Length - 1
                If fdScores(y).Item("Score") = 1 Then
                    dr.Item("Decision") = dr2.Item("Code")
                    dr.Item("Decision") &= " (" & GetSideString(ds, fdBallots(x).Item("Side"), dr2.Item("Event")) & ")"
                End If
            Next y
            fdScores = ds.Tables("Ballot_Score").Select("Ballot=" & fdBallots(x).Item("ID") & "and Score_ID>=2 and Score-ID<=3", "Recipient, Score_ID ASC")
            strPts = "" : strName = ""
            For y = 0 To fdScores.Length - 1
                drDebater = ds.Tables("Entry_Student").Rows.Find(fdScores(y).Item("Recipient"))
                strName = drDebater.Item("Last")
                strPts &= " " & fdScores(y).Item("Score")
                If y = fdScores.Length - 1 Then
                    dr.Item("Points") &= strName & " " & strPts & ", "
                    strPts = ""
                ElseIf y < fdScores.Length - 1 Then
                    If fdScores(y).Item("Recipient") <> fdScores(y + 1).Item("Recipient") Then
                        dr.Item("Points") &= strName & strPts & ", "
                        strPts = ""
                    End If
                End If
            Next y
        Next x
        If Not dr Is Nothing Then
            If Not dr.Item("Points") Is System.DBNull.Value Then
                dr.Item("Points") = Mid(dr.Item("Points"), 1, dr.Item("Points").length - 2)
            End If
            DT.Rows.Add(dr)
        End If
    End Sub
End Class