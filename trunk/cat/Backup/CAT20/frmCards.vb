Public Class frmCards

    Dim ds As New DataSet
    Private Sub frmCards_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Load
        LoadFile(ds, "TourneyData")

        'bind round CBO
        ds.Tables("Entry").DefaultView.Sort = "FullName asc"
        cboTeams.DataSource = ds.Tables("Entry").DefaultView
        cboTeams.DisplayMember = "FullName"
        cboTeams.ValueMember = "ID"
    End Sub

    Private Sub butLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butLoad.Click
        'pull all ballots for team
        Dim fdBallots As DataRow()
        fdBallots = ds.Tables("Ballot").Select("Entry=" & cboTeams.SelectedValue, "Panel ASC")
        
        'build the datatable
        Dim DT As New DataTable
        Call MakeCardTable(DT, cboTeams.SelectedValue, fdBallots)

        'populate
        Call PopulateTable(DT, cboTeams.SelectedValue, fdBallots)

        DT.DefaultView.Sort = "RoundID ASC"
        DataGridView1.DataSource = DT.DefaultView
        DataGridView1.Columns("RoundID").Visible = False

    End Sub
    Sub MakeCardTable(ByRef DT As DataTable, ByVal team As Integer, ByVal fdballots As DataRow())
        DT.Columns.Add("Judge", System.Type.GetType("System.String"))
        DT.Columns.Add("RoundID", System.Type.GetType("System.Int64"))
        DT.Columns.Add("Round", System.Type.GetType("System.String"))
        DT.Columns.Add("Opponent", System.Type.GetType("System.String"))
        DT.Columns.Add("Room", System.Type.GetType("System.String"))
        DT.Columns.Add("Side", System.Type.GetType("System.String"))

        'Add columns for all team scores
        Dim fdScores As DataRow()
        Dim drPanel, drRd As DataRow
        For x = 0 To fdballots.Length - 1
            drPanel = ds.Tables("Panel").Rows.Find(fdballots(x).Item("Panel"))
            drRd = ds.Tables("Round").Rows.Find(drPanel.Item("Round"))
            If drRd.Item("Rd_Name") <= 9 Then
                fdScores = ds.Tables("Ballot_Score").Select("Ballot=" & fdballots(x).Item("ID"))
                For y = 0 To fdScores.Length - 1
                    If fdScores(y).Item("Score_ID") = 1 And DT.Columns.Contains("Decision") = False Then
                        DT.Columns.Add("Decision", System.Type.GetType("System.String"))
                    End If
                    If fdScores(y).Item("Score_ID") = 4 And DT.Columns.Contains("TeamPoints") = False Then
                        DT.Columns.Add("TeamPoints", System.Type.GetType("System.String"))
                    End If
                    If fdScores(y).Item("Score_ID") = 5 And DT.Columns.Contains("TeamRank") = False Then
                        DT.Columns.Add("TeamRank", System.Type.GetType("System.String"))
                    End If
                Next y
            End If
        Next

        'add columns for all speakers
        Dim fdSpkrs As DataRow() : Dim dummy As String
        fdSpkrs = ds.Tables("Entry_Student").Select("Entry=" & team)
        For x = 0 To fdSpkrs.Length - 1
            dummy = fdSpkrs(x).Item("Last").trim & ", " & fdSpkrs(x).Item("First").trim
            DT.Columns.Add(dummy, System.Type.GetType("System.String"))
        Next x

    End Sub
    Sub PopulateTable(ByRef DT As DataTable, ByVal team As Integer, ByVal fdballots As DataRow())

        Dim fdScores As DataRow()
        Dim drPanel, drRd, drTableRow, drSpkr, drRoom As DataRow
        For x = 0 To fdballots.Length - 1
            drPanel = ds.Tables("Panel").Rows.Find(fdballots(x).Item("Panel"))
            drRd = ds.Tables("Round").Rows.Find(drPanel.Item("Round"))
            If drRd.Item("Rd_Name") <= 9 Then
                drTableRow = DT.NewRow
                drTableRow.Item("Round") = drRd.Item("Label")
                drTableRow.Item("RoundID") = drRd.Item("ID")
                drTableRow.Item("Judge") = GetName(ds.Tables("Judge"), fdballots(x).Item("Judge"), "Last") & ", " & GetName(ds.Tables("Judge"), fdballots(x).Item("Judge"), "First")
                drTableRow.Item("Opponent") = GetOpponentStr(team, fdballots(x).Item("Panel"))
                drRoom = ds.Tables("Room").Rows.Find(drPanel.Item("Room"))
                If drRoom Is Nothing Then
                    drTableRow.Item("Room") = "N/A"
                Else
                    drTableRow.Item("Room") = drRoom.Item("RoomName")
                End If
                drTableRow.Item("Side") = GetSideString(ds, fdballots(x).Item("Side"), drRd.Item("Event"))
                fdScores = ds.Tables("Ballot_Score").Select("Ballot=" & fdballots(x).Item("ID"), "Score_ID ASC")
                For y = 0 To fdScores.Length - 1
                    If fdScores(y).Item("Score_ID") = 1 Then
                        If fdScores(y).Item("Score") = 1 Then drTableRow.Item("Decision") = "W" Else drTableRow.Item("Decision") = "L"
                    End If
                    If fdScores(y).Item("Score_ID") = 4 Then
                        drTableRow.Item("TeamPoints") = fdScores(y).Item("Score")
                    End If
                    If fdScores(y).Item("Score_ID") = 5 Then
                        drTableRow.Item("TeamRank") = fdScores(y).Item("Score")
                    End If
                    If fdScores(y).Item("Score_ID") = 2 Then
                        drSpkr = ds.Tables("Entry_Student").Rows.Find(fdScores(y).Item("Recipient"))
                        drTableRow.Item(drSpkr.Item("Last").trim & ", " & drSpkr.Item("First").trim) = fdScores(y).Item("Score")
                    End If
                    If fdScores(y).Item("Score_ID") = 3 Then
                        drSpkr = ds.Tables("Entry_Student").Rows.Find(fdScores(y).Item("Recipient"))
                        drTableRow.Item(drSpkr.Item("Last").trim & ", " & drSpkr.Item("First").trim) &= " / " & fdScores(y).Item("Score")
                    End If
                Next y
                DT.Rows.Add(drTableRow)
            End If
        Next
        DT.AcceptChanges()
    End Sub
    Function GetOpponentStr(ByVal team As Integer, ByVal panel As Integer) As String
        GetOpponentSTr = ""
        Dim fdteams As DataRow() : Dim drTeam As DataRow
        fdteams = ds.Tables("Ballot").Select("Panel=" & panel & " and entry<>" & team)
        For x = 0 To fdteams.Length - 1
            drTeam = ds.Tables("Entry").Rows.Find(fdteams(x).Item("Entry"))
            If Not drTeam Is Nothing Then
                GetOpponentStr &= drTeam.Item("Code")
            End If
        Next x
        If GetOpponentSTr = "" Then GetOpponentSTr = "N/A"
    End Function
    Sub ScreenReset() Handles cboTeams.SelectedIndexChanged
        DataGridView1.DataSource = Nothing
    End Sub

End Class