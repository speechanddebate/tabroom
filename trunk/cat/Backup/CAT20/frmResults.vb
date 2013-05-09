Imports System.IO

Public Class frmResults
    Dim ds As New DataSet
    Dim strHeaderStuff As String

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If cboround.SelectedIndex = -1 Then
            MsgBox("pick a valid round and try again.")
            Exit Sub
        End If
        Dim dt As DataTable
        Dim rd2 As Integer = cboround.SelectedValue
        Dim dr As DataRow : dr = ds.Tables("Round").Rows.Find(cboround.SelectedValue)
        If dr.Item("PairingScheme").toupper = "PRESET" Then rd2 = GetFirstElim(ds, dr.Item("Event"))
        dt = MakeTBTable(ds, cboround.SelectedValue, "TEAM", "CODE", cboTieBreakSet.SelectedValue, rd2)
        Call LowStripper(dt)
        DataGridView1.DataSource = dt
        DataGridView1.Columns("Competitor").Visible = False
        strHeaderStuff = "TEAMS IN SEED ORDER"
    End Sub
    Sub LowStripper(ByRef DT As DataTable)
        For x = DT.Rows.Count - 1 To 0 Step -1
            If x > Val(txtShowTop.Text) - 1 Then DT.Rows(x).Delete()
        Next x
        DT.AcceptChanges()
    End Sub

    Private Sub frmResults_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call LoadFile(ds, "TourneyData")

        'bind round CBO
        cboround.DataSource = ds.Tables("Round")
        cboround.DisplayMember = "Label"
        cboround.ValueMember = "ID"

        'bind tiebreak set

        cboTieBreakSet.DataSource = ds.Tables("Tiebreak_set")
        cboTieBreakSet.DisplayMember = "TBSET_Name"
        cboTieBreakSet.ValueMember = "ID"

    End Sub

    Private Sub butMakeResults_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butMakeResults.Click
        Label1.Text = "Writing entry and settings tables..." : Label1.Refresh()
        Dim strFileName As String = Replace(strFilePath, "TourneyData.xml", "TourneyDataResults.xml")
        Dim STW As New StreamWriter(strFileName)
        STW.WriteLine("<?xml version=""1.0""?>")
        STW.WriteLine("<TOURNAMENTRESULTS>")
        Call WriteTable(STW, "TOURN")
        Call WriteTable(STW, "ENTRY")
        Call WriteTable(STW, "ENTRY_STUDENT")
        Call WriteTable(STW, "JUDGE")
        Call WriteTable(STW, "EVENT")
        Call WriteTable(STW, "SCHOOL")
        Call WriteTable(STW, "ROUND")
        Call WriteTable(STW, "ROOM")
        For x = 0 To ds.Tables("Event").Rows.Count - 1
            Label1.Text = "Writing results for " & ds.Tables("Event").Rows(x).Item("EventName") : Label1.Refresh()
            Call WriteFinalRank(STW, ds.Tables("Event").Rows(x).Item("ID"))
            Call WriteRoundByRound(STW, ds.Tables("Event").Rows(x).Item("ID"))
        Next x
        STW.WriteLine("</TOURNAMENTRESULTS>")
        STW.Close()
        Label1.Text = "Done" : Label1.Refresh()
        MsgBox("Done")
    End Sub
    Sub WriteTable(ByVal stw As StreamWriter, ByVal strTable As String)
        'writes a flat, 2-d table from the dataset

        'strip unwanted fields -- NOT SURE why this was here, but I do want to export this info to TRPC
        Dim dt As New DataTable
        dt = ds.Tables(strTable)
        'If strTable = "ENTRY" Then dt.Columns.Remove("ADA") : dt.Columns.Remove("NOTES") : dt.AcceptChanges()

        Dim x, y As Integer
        Dim dummy As String
        For x = 0 To ds.Tables(strTable).Rows.Count - 1
            stw.WriteLine("  <" & dt.TableName.ToUpper & ">")
            For y = 0 To ds.Tables(strTable).Columns.Count - 1
                dummy = "    <" & dt.Columns(y).ColumnName.ToUpper.Trim & ">" & dt.Rows(x).Item(y).ToString.Trim & "</" & dt.Columns(y).ColumnName.ToUpper & ">"
                stw.WriteLine(dummy)
            Next y
            stw.WriteLine("  </" & dt.TableName.ToUpper & ">")
        Next x
    End Sub
    Sub WriteFinalRank(ByVal stw As StreamWriter, ByVal EventID As Integer)
        'find the TB_SET and timeslot of the last prelim
        Dim drRound As DataRow() : Dim TBSet, timeslot, round As Integer
        drRound = ds.Tables("Round").Select("Event=" & EventID, "timeslot asc")
        For Each row In drRound
            If row.Item("RD_Name") <= 9 Then TBSet = row.Item("TB_SET") : timeslot = row.Item("TimeSlot") : round = row.Item("ID")
        Next
        'load the teams in order
        Dim dt As DataTable
        'MsgBox("YOU NEED TO MARK THE ROUND; LAST PRELM CURRENTLY MARKED AS 58")
        dt = MakeTBTable(ds, round, "TEAM", "Full", -1, round)

        Dim x, y As Integer
        Dim dummy, strSortDirection As String
        stw.WriteLine("  <FINALRANK EventID=""" & EventID & """>")
        For x = 0 To dt.Rows.Count - 1
            stw.WriteLine("    <TEAM EntryID=""" & dt.Rows(x).Item(0) & """>")
            dummy = "    <TIEBREAKER TB_Name=""SEED"" Sortorder=""" & 1 & """ SortDirection=""ASC"">" & x + 1 & "</TIEBREAKER>"
            stw.WriteLine(dummy)
            For y = 3 To dt.Columns.Count - 1
                strSortDirection = "DESC" : If dt.Rows(0).Item(y) < dt.Rows(1).Item(y) Then strSortDirection = "ASC"
                dummy = "    <TIEBREAKER TB_Name=""" & dt.Columns(y).ColumnName.ToUpper.Trim & """ Sortorder=""" & y - 1 & """ SortDirection=""" & strSortDirection & """>" & dt.Rows(x).Item(y).ToString.Trim & "</TIEBREAKER>"
                stw.WriteLine(dummy)
            Next y
            stw.WriteLine("    </TEAM>")
        Next x
        stw.WriteLine("  </FINALRANK>")
    End Sub
    Sub WriteRoundByRound(ByVal stw As StreamWriter, ByVal EventID As Integer)
        Dim dvRound As New DataView(ds.Tables("ROUND"))
        dvRound.RowFilter = "EVENT=" & EventID
        dvRound.Sort = "timeslot ASC"
        Dim x, y, z, w As Integer : Dim dummy, rdType As String
        Dim foundBallot As DataRow()
        Dim foundPanel As DataRow()
        Dim foundResult As DataRow()
        For x = 0 To dvRound.Count - 1
            rdType = "Prelim" : If dvRound(x).Item("TimeSlot") > 9 Then rdType = "Elim"
            dummy = "  <ROUNDRESULT RoundID=""" & dvRound(x).Item("ID").ToString.Trim & """ RoundName=""" & dvRound(x).Item("Label").trim & """ Sortorder=""" & dvRound(x).Item("TimeSlot") & """ RoundType=""" & rdType & """ EventID=""" & dvRound(x).Item("Event") & """" & ">"
            stw.WriteLine(dummy)
            foundPanel = ds.Tables("Panel").Select("Round=" & dvRound(x).Item("ID"))
            For y = 0 To foundPanel.Length - 1
                If foundPanel(y).Item("Flight") Is System.DBNull.Value Then foundPanel(y).Item("Flight") = 1
                foundBallot = ds.Tables("Ballot").Select("Panel=" & foundPanel(y).Item("ID"), "JUDGE ASC")
                For z = 0 To foundBallot.Length - 1
                    dummy = "    <BALLOT JudgeID=""" & foundBallot(z).Item("Judge") & """ RoomID=""" & foundPanel(y).Item("ROOM") & """ Flight=""" & foundPanel(y).Item("Flight") & """ Panel=""" & foundPanel(y).Item("ID") & """" & ">"
                    If z = 0 Then stw.WriteLine(dummy)
                    If z > 0 Then If foundBallot(z).Item("Judge") <> foundBallot(z - 1).Item("Judge") Then stw.WriteLine(dummy)
                    foundResult = ds.Tables("Ballot_Score").Select("Ballot=" & foundBallot(z).Item("ID"))
                    For w = 0 To foundResult.Length - 1
                        dummy = "      <SCORE SCORE_NAME=""" & GetRowInfo(ds, "SCORES", foundResult(w).Item("SCORE_ID"), "Score_Name") & """ ScoreFor=""" & GetRowInfo(ds, "SCORES", foundResult(w).Item("SCORE_ID"), "ScoreFor") & """ Recipient=""" & foundResult(w).Item("Recipient") & """"
                        If GetRowInfo(ds, "SCORES", foundResult(w).Item("SCORE_ID"), "ScoreFor") = "Team" Then dummy &= " Side=""" & foundBallot(z).Item("Side") & """"
                        dummy &= ">" & foundResult(w).Item("Score")
                        stw.WriteLine(dummy & "</SCORE>")
                    Next w
                    If z = foundBallot.Length - 1 Then stw.WriteLine("    </BALLOT>")
                    If z < foundBallot.Length - 1 Then If foundBallot(z).Item("Judge") <> foundBallot(z + 1).Item("Judge") Then stw.WriteLine("    </JUDGE>")
                Next z
            Next y
            stw.WriteLine("  </ROUNDRESULT>")
        Next x
        dvRound.RowFilter = Nothing
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If cboround.SelectedIndex = -1 Then
            MsgBox("pick a valid round and try again.")
            Exit Sub
        End If
        Dim dt As DataTable
        dt = MakeTBTable(ds, cboround.SelectedValue, "SPEAKER", "X", cboTieBreakSet.SelectedValue, cboround.SelectedValue)
        Dim drDude, drSchool As DataRow
        For x = 0 To dt.Rows.Count - 1
            drDude = ds.Tables("Entry_Student").Rows.Find(dt.Rows(x).Item("Competitor"))
            drSchool = ds.Tables("School").Rows.Find(drDude.Item("School"))
            dt.Rows(x).Item("CompetitorName") &= " (" & drSchool.Item("SchoolName") & ")"
        Next x
        DataGridView1.DataSource = dt
        Call LowStripper(dt)
        DataGridView1.Columns("Competitor").Visible = False
        strHeaderStuff = "SPEAKERS IN SEEDED ORDER"
    End Sub

    Private Sub butPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butPrint.Click
        Dim defCols(DataGridView1.Columns.Count) As Boolean
        For x = 0 To DataGridView1.ColumnCount - 1
            If DataGridView1.Columns(x).Visible = True Then defCols(x) = True
            If DataGridView1.Columns(x).Name = "Balance" Then defCols(x) = False
        Next x
        Dim frm As New frmPrint(DataGridView1, strHeaderStuff & Chr(13) & Chr(10), defCols)
        frm.ShowDialog()
    End Sub

    Private Sub butOppRatings_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butOppRatings.Click
        Dim dt As New DataTable
        Call MakeOppTable(ds, dt, cboround.SelectedValue)
        DataGridView1.DataSource = dt
    End Sub

    Private Sub butJudgeReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butJudgeReport.Click
        Dim dt As New DataTable
        dt.Columns.Add(("JudgeName"), System.Type.GetType("System.String"))
        Dim dr, drPanel, drRd As DataRow
        For x = 0 To ds.Tables("Timeslot").Rows.Count - 1
            dt.Columns.Add("Slot" & ds.Tables("Timeslot").Rows(x).Item("ID"), System.Type.GetType("System.Boolean"))
        Next x
        dt.Columns.Add(("Total"), System.Type.GetType("System.Int16"))
        dt.Columns.Add(("AvgPref"), System.Type.GetType("System.Single"))
        Dim fdBallot As DataRow()
        For x = 0 To ds.Tables("Judge").Rows.Count - 1
            dr = dt.NewRow
            dr.Item("JudgeName") = ds.Tables("Judge").Rows(x).Item("Last") & ", " & ds.Tables("Judge").Rows(x).Item("First")
            dr.Item("AvgPref") = ds.Tables("Judge").Rows(x).Item("AvgPref")
            fdBallot = ds.Tables("Ballot").Select("Judge=" & ds.Tables("Judge").Rows(x).Item("ID"))
            For y = 0 To fdBallot.Length - 1
                drPanel = ds.Tables("Panel").Rows.Find(fdBallot(y).Item("Panel"))
                drRd = ds.Tables("Round").Rows.Find(drPanel.Item("Round"))
                dr.Item("Slot" & drRd.Item("Timeslot").ToString) = True
            Next y
            dt.Rows.Add(dr)
        Next x
        DataGridView1.DataSource = dt
        Dim ctr As Integer : Dim drTS As DataRow
        For x = 0 To DataGridView1.RowCount - 1
            ctr = 0
            For y = 0 To DataGridView1.Columns.Count - 1
                If InStr(DataGridView1.Columns(y).Name.ToUpper, "SLOT") > 0 Then
                    If Not DataGridView1.Rows(x).Cells(y).Value Is System.DBNull.Value Then
                        If DataGridView1.Rows(x).Cells(y).Value = True Then ctr += 1
                    End If
                End If
            Next
            DataGridView1.Rows(x).Cells("Total").Value = ctr
        Next
        For y = 0 To DataGridView1.Columns.Count - 1
            If InStr(DataGridView1.Columns(y).Name.ToUpper, "TIME") > 0 Then
                drTS = ds.Tables("Timeslot").Rows.Find(DataGridView1.Columns(y).Name)
                DataGridView1.Columns(y).HeaderText = drTS.Item("TimeSlotName")
            End If
        Next y
        DataGridView1.Columns("JudgeName").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
    End Sub

    Private Sub butElimResultsReaderSheets_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butElimResultsReaderSheets.Click
        Dim dt As New DataTable
        dt.Columns.Add("EventID", System.Type.GetType("System.Int64"))
        dt.Columns.Add("Rd_Name", System.Type.GetType("System.Int64"))
        dt.Columns.Add("Event", System.Type.GetType("System.String"))
        dt.Columns.Add("Round", System.Type.GetType("System.String"))
        dt.Columns.Add("Result", System.Type.GetType("System.String"))
        Dim drRd, drDummy, drEvent, drTeam As DataRow
        Dim dummy As Integer : Dim strBallotCount As String = ""
        Dim fdballots As DataRow()
        Dim strWinner, strLoser, strDecision As String : strWinner = "" : strLoser = "" : strDecision = ""
        For x = 0 To ds.Tables("Panel").Rows.Count - 1
            drRd = ds.Tables("Round").Rows.Find(ds.Tables("Panel").Rows(x).Item("Round"))
            If drRd.Item("Rd_Name") >= 10 Then
                fdBallots = ds.Tables("Ballot").Select("Panel=" & ds.Tables("Panel").Rows(x).Item("ID"), "Entry ASC")
                drDummy = dt.NewRow
                drEvent = ds.Tables("Event").Rows.Find(drRd.Item("Event"))
                drDummy.Item("EventID") = drRd.Item("Event")
                drDummy.Item("Rd_Name") = drRd.Item("Rd_Name")
                drDummy.Item("Event") = drEvent.Item("EventName")
                drDummy.Item("Round") = drRd.Item("Label")
                dummy = GetWinner(ds, ds.Tables("Panel").Rows(x).Item("ID"), drRd.Item("Event"), strBallotCount)
                For y = 0 To fdballots.Length - 1
                    drTeam = ds.Tables("Entry").Rows.Find(fdballots(y).Item("Entry"))
                    If fdballots(y).Item("Entry") = dummy Then
                        strWinner = drTeam.Item("FullName")
                    Else
                        strLoser = drTeam.Item("FullName")
                    End If
                Next y
                If dummy = -1 Then
                    drDummy.Item("Result") = ""
                    For y = 1 To fdballots.Length - 1
                        If fdballots(y).Item("Entry") <> fdballots(y - 1).Item("Entry") Then
                            drTeam = ds.Tables("Entry").Rows.Find(fdballots(1).Item("Entry"))
                            If strWinner = "" Then
                                strWinner = drTeam.Item("FullName")
                            Else
                                strLoser = drTeam.Item("FullName")
                            End If
                            strDecision = " have no decision yet."
                        End If
                    Next y
                    drDummy.Item("Result") = strWinner & " vs. " & strLoser & " have no decision yet."
                Else
                    drDummy.Item("Result") = strWinner & " defeat " & strLoser & " on a " & strBallotCount & " decision."
                End If
                dt.Rows.Add(drDummy)
            End If
        Next x
        dt.DefaultView.Sort = "EventID ASC, Rd_Name ASC"
        DataGridView1.DataSource = dt.DefaultView
        DataGridView1.Columns(0).Visible = False
        DataGridView1.Columns(1).Visible = False
        DataGridView1.Columns(2).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        DataGridView1.Columns(3).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        DataGridView1.Columns(4).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        MsgBox("Done")
    End Sub

    Private Sub butHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butHelp.Click
        Dim strInfo As String = ""
        strInfo &= "The buttons below the 'Seeded Results' grid will simply populate ther grid." & Chr(10) & Chr(10)
        strInfo &= "Within the 'Seed Results' group, identify both a round completed and a tiebreaker set that you wish to use." & Chr(10) & Chr(10)
        strInfo &= "More information about tiebreaker sets can be found on the 'set up tiebreakers' page." & Chr(10) & Chr(10)
        strInfo &= "With those selected, click either the 'show team results' or 'show speaker results' button.  Note that if you have not selected the appropriate tiebreaker set, you may get unexpected results." & Chr(10) & Chr(10)
        strInfo &= "Once the grid is loaded via any method, clicking the 'Print' button will send the contents of the grid to the printer." & Chr(10) & Chr(10)
        strInfo &= "The 'create results files' button will not be necessary if you are sending pairings to tabroom.com.  It's technical function is to create a file in the full Universal Data Structure format." & Chr(10) & Chr(10)
        MsgBox(strInfo, , "Results Page Help")
    End Sub

    Private Sub butShowPrefByTeam_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butShowPrefByTeam.Click
        'format data table
        Dim dt As New DataTable
        dt.Columns.Add(("Team"), System.Type.GetType("System.String"))
        dt.Columns.Add(("Event"), System.Type.GetType("System.String"))
        Dim dr, drPanel, drRd, drEvent As DataRow
        Dim fdRd As DataRow() : Dim isPrelim As Boolean
        For x = 0 To ds.Tables("Timeslot").Rows.Count - 1
            fdRd = ds.Tables("Round").Select("Timeslot=" & ds.Tables("TimeSlot").Rows(x).Item("ID"))
            isPrelim = False
            For y = 0 To fdRd.Length - 1
                If fdRd(y).Item("Rd_Name") <= 9 Then isPrelim = True
            Next y
            If isPrelim = True Then dt.Columns.Add("Slot" & ds.Tables("Timeslot").Rows(x).Item("ID"), System.Type.GetType("System.Single"))
        Next x
        dt.Columns.Add(("Avg"), System.Type.GetType("System.Single"))
        'populate
        Dim fdBallot As DataRow()
        For x = 0 To ds.Tables("Entry").Rows.Count - 1
            dr = dt.NewRow
            dr.Item("Team") = ds.Tables("Entry").Rows(x).Item("FullName")
            drEvent = ds.Tables("Event").Rows.Find(ds.Tables("Entry").Rows(x).Item("Event"))
            dr.Item("Event") = drEvent.Item("EventName")
            fdBallot = ds.Tables("Ballot").Select("Entry=" & ds.Tables("Entry").Rows(x).Item("ID"), "Panel ASC")
            For y = 0 To fdBallot.Length - 1
                drPanel = ds.Tables("Panel").Rows.Find(fdBallot(y).Item("Panel"))
                drRd = ds.Tables("Round").Rows.Find(drPanel.Item("Round"))
                dr.Item("Slot" & drRd.Item("Timeslot").ToString) = GetJudgeRating(ds, fdBallot(y).Item("Judge"), ds.Tables("Entry").Rows(x).Item("ID"), "TEAMRATING")
            Next y
            dt.Rows.Add(dr)
        Next x
        'bind
        dt.DefaultView.Sort = "Event ASC"
        DataGridView1.DataSource = dt
        DataGridView1.Columns("Team").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        DataGridView1.Columns("Avg").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        'Calculate
        Dim ctr As Integer : Dim Tot As Single
        For x = 0 To DataGridView1.RowCount - 1
            ctr = 0 : Tot = 0
            For y = 0 To DataGridView1.Columns.Count - 1
                If InStr(DataGridView1.Columns(y).Name.ToUpper, "SLOT") > 0 Then
                    If Not DataGridView1.Rows(x).Cells(y).Value Is System.DBNull.Value Then
                        ctr += 1
                        Tot += DataGridView1.Rows(x).Cells(y).Value
                    End If
                End If
            Next
            DataGridView1.Rows(x).Cells("Avg").Value = Tot / ctr
        Next
    End Sub
End Class