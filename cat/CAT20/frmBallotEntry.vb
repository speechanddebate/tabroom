Imports System.Net
Imports System.IO
Imports System.Xml

Public Class frmBallotEntry
    Dim DS As New DataSet
    Dim ds2 As New DataSet
    Dim strDebateType As String
    Private Sub frmBallotEntry_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Load
        LoadFile(DS, "TourneyData")

        'bind round CBO
        cboRound.DataSource = DS.Tables("Round")
        cboRound.DisplayMember = "Label"
        cboRound.ValueMember = "ID"

    End Sub
    Private Sub frmBallotEntry_Unload(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Disposed
        'save file on page close
        Call SaveFile(DS)
        DS.Dispose()
    End Sub

    Private Sub butLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butLoad.Click
        Dim drRd As DataRow : drRd = DS.Tables("Round").Rows.Find(cboRound.SelectedValue)
        Dim drEvent As DataRow : drEvent = DS.Tables("Event").Rows.Find(drRd.Item("Event"))
        strDebateType = drEvent.Item("Type")
        If drRd.Item("Rd_Name") > 9 Then
            MsgBox("This appears to be an elim round.  To download elim rounds, go to the main menu and click the elims button.  The download button is in the top-right.")
            Exit Sub
        End If
        Call ShowThePairing()
        Dim dt As New DataTable
        dt = DataGridView1.DataSource
        Dim fdRd As DataRow : fdRd = DS.Tables("Round").Rows.Find(cboRound.SelectedValue)
        For x = 1 To fdRd.Item("JudgesPerPanel")
            dt.Columns.Add("Entered" & x, System.Type.GetType("System.Boolean"))
        Next x
        Call MarkBallotsIn(dt)
        DataGridView1.DataSource = dt
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        radTeam1.Text = GetSideString(DS, 1, fdRd.Item("Event"))
        radTeam2.Text = GetSideString(DS, 2, fdRd.Item("Event"))
        'show ballots and byes IF wins/losses are being awarded
        grpDecision.Visible = False
        Dim fdBalScores As DataRow()
        fdBalScores = DS.Tables("TieBreak").Select("TB_SET=" & fdRd.Item("TB_SET"))
        For x = 0 To fdBalScores.Length - 1
            If fdBalScores(x).Item("ScoreID") = 1 Then grpDecision.Visible = True
        Next x
        If chkSortEntered.Checked = True Then
            DataGridView1.Sort(DataGridView1.Columns("Entered1"), System.ComponentModel.ListSortDirection.Ascending)
        End If
        If getEventSetting(DS, fdRd.Item("Event"), "PanelDecisions") = True Then Call PanelDecision()
    End Sub
    Sub MarkBallotsIn(ByRef dt As DataTable)
        'marks which ballots are in and counts them
        Dim Entered As Integer
        Dim drPanel As DataRow
        Dim drBallots As DataRow()
        Dim fdRd As DataRow : fdRd = DS.Tables("Round").Rows.Find(cboRound.SelectedValue)
        For x = 0 To dt.Rows.Count - 1
            For w = 1 To fdRd.Item("JudgesPerPanel")
                dt.Rows(x).Item("Entered" & w) = False
                drPanel = DS.Tables("Panel").Rows.Find(dt.Rows(x).Item("Panel"))
                If dt.Rows(x).Item("Judge" & w) Is System.DBNull.Value Then dt.Rows(x).Item("Judge" & w) = -99
                drBallots = DS.Tables("Ballot").Select("Panel=" & drPanel.Item("ID") & " and judge=" & dt.Rows(x).Item("Judge" & w))
                For y = 0 To drBallots.Length - 1
                    If IsBallotIn(DS, drBallots(y).Item("ID")) = True Then dt.Rows(x).Item("Entered" & w) = True
                    If dt.Rows(x).Item("Entered" & w) = True Then Entered += 1 : Exit For
                Next y
            Next w
        Next x
        Dim ballots As Integer = fdRd.Item("JudgesPerPanel") * dt.Rows.Count
        Label1.Text = ballots - Entered & " ballots still out; " & Entered & " ballots in."
    End Sub
    Sub ShowThePairing()
        Call modShowThePairing(DS, DataGridView1, cboRound.SelectedValue, "Code")
    End Sub
    Sub PanelDecision()
        'Only 1 decision per panel, so string additional ballots
        Dim dr As DataRow : dr = DS.Tables("Round").Rows.Find(cboRound.SelectedValue)
        Dim nJudges = dr.Item("JudgesPerPanel")
        For x = 0 To DataGridView1.Columns.Count - 1
            If DataGridView1.Columns(x).Visible = True Then
                For y = 2 To nJudges
                    If DataGridView1.Columns(x).Name = "JUDGENAME" & y.ToString Or DataGridView1.Columns(x).Name = "Entered" & y.ToString Then
                        DataGridView1.Columns(x).Visible = False
                    End If
                Next y
            End If
        Next
    End Sub

    Sub LoadBallot()

        'get judge number
        Dim dummy As String = DataGridView1.Columns(DataGridView1.CurrentCell.ColumnIndex).HeaderText
        dummy = Replace(dummy, "JUDGENAME", "")
        If Val(dummy) = 0 Then
            MsgBox("Please click on a judge name.  Try again.", MsgBoxStyle.OkOnly)
            Exit Sub
        End If
        'pull judge and panel IDs
        Dim Judge As Integer = DataGridView1.CurrentRow.Cells("Judge" & dummy).Value
        Dim Panel As Integer = DataGridView1.CurrentRow.Cells("Panel").Value
        'MsgBox(Panel)

        'dim the underlying datatable
        Dim dt As New DataTable
        dt.Columns.Add("ID", System.Type.GetType("System.Int32"))
        dt.Columns.Add("Ballot", System.Type.GetType("System.Int32"))
        dt.Columns.Add("Side", System.Type.GetType("System.Int16"))
        dt.Columns.Add("Recipient", System.Type.GetType("System.Int64"))
        dt.Columns.Add("Score_ID", System.Type.GetType("System.Int32"))
        dt.Columns.Add("Score", System.Type.GetType("System.Single"))
        dt.Columns.Add("RecipientName", System.Type.GetType("System.String"))
        dt.Columns.Add("ScoreName", System.Type.GetType("System.String"))

        radTeam1.Checked = False : radTeam2.Checked = False : radDoubLoss.Checked = False : radDoubWin.Checked = False
        lblSaved.Text = ""

        'pull ballots and ballot scores
        Dim fdBallots, fdBalScores As DataRow() : Dim dr, dr2 As DataRow
        Dim Winner As Integer : Dim ShowWinner As Boolean = False
        fdBallots = DS.Tables("Ballot").Select("Judge=" & Judge & " and panel=" & Panel, "Side ASC")
        For x = 0 To fdBallots.Length - 1
            ValidateScoresByBallot(DS, fdBallots(x).Item("ID"))
            If strDebateType = "WUDC" Then
                fdBalScores = DS.Tables("Ballot_Score").Select("Ballot=" & fdBallots(x).Item("ID"), "Score_ID ASC, Recipient ASC")
            Else
                fdBalScores = DS.Tables("Ballot_Score").Select("Ballot=" & fdBallots(x).Item("ID"), "Recipient ASC, Score_ID ASC")
            End If
            'populate each row
            For y = 0 To fdBalScores.Length - 1
                If fdBalScores(y).Item("Score_ID") > 1 Then
                    dr = dt.NewRow
                    dr.Item("ID") = fdBalScores(y).Item("ID")
                    dr.Item("Ballot") = fdBalScores(y).Item("Ballot")
                    dr.Item("Side") = fdBallots(x).Item("Side")
                    dr.Item("Recipient") = fdBalScores(y).Item("Recipient")
                    dr.Item("Score_ID") = fdBalScores(y).Item("Score_ID")
                    dr.Item("Score") = fdBalScores(y).Item("Score")
                    dr2 = DS.Tables("Scores").Rows.Find(fdBalScores(y).Item("Score_ID"))
                    dr.Item("ScoreName") = dr2.Item("Score_Name")
                    If fdBalScores(y).Item("Score_ID") = 2 Or fdBalScores(y).Item("Score_ID") = 3 Then
                        dr2 = DS.Tables("Entry_Student").Rows.Find(fdBalScores(y).Item("Recipient"))
                        dr.Item("RecipientName") = dr2.Item("Last").trim & ", " & dr2.Item("First").trim
                    Else
                        dr2 = DS.Tables("Entry").Rows.Find(fdBalScores(y).Item("Recipient"))
                        dr.Item("RecipientName") = dr2.Item("Code")
                    End If
                    dt.Rows.Add(dr)
                ElseIf fdBalScores(y).Item("Score_ID") = 1 Then
                    ShowWinner = True
                    If Winner > 0 And fdBalScores(y).Item("Score") = 1 And fdBallots(x).Item("Side") <> -1 Then
                        radDoubWin.Checked = True
                    End If
                    If fdBalScores(y).Item("Score") = 1 Then Winner = fdBalScores(y).Item("Recipient")
                End If
            Next y
        Next x
        DataGridView2.AutoGenerateColumns = False
        DataGridView2.DataSource = dt
        If grpDecision.Visible = True Then
            If radDoubWin.Checked = False Then
                If Winner = DataGridView1.CurrentRow.Cells("Team1").Value Then
                    radTeam1.Checked = True
                ElseIf Winner = DataGridView1.CurrentRow.Cells("Team2").Value Then
                    radTeam2.Checked = True
                End If
                If Winner = 0 Then radDoubLoss.Checked = True
            End If
            Call UpdateDecision()
        End If
        DataGridView2.CurrentCell = DataGridView2("Score", 0)
        DataGridView2.Focus()
    End Sub
    Sub UpdateDecision()
        lblDecision.Text = "No decision entered."
        If radTeam1.Checked = True Then lblDecision.Text = "Winner is " & DataGridView1.CurrentRow.Cells("TeamName1").Value & " on the " & radTeam1.Text
        If radTeam2.Checked = True Then lblDecision.Text = "Winner is " & DataGridView1.CurrentRow.Cells("TeamName2").Value & " on the " & radTeam2.Text
        If radDoubLoss.Checked = True Then lblDecision.Text = "Double LOSS"
        If radDoubWin.Checked = True Then lblDecision.Text = "Double WIN"
    End Sub
    Sub ReadClick() Handles DataGridView1.MouseClick
        Call LoadBallot()
    End Sub
    Private Sub CellValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DataGridView2.CurrentCellDirtyStateChanged
        If chkAutoComplete.Checked = False Then Exit Sub
        If Not DataGridView2.CurrentCell.Value Is System.DBNull.Value Then
            If DataGridView2.CurrentCell.Value < 0 Then
                'MsgBox("To enter a bye, exit autocomplete mode, click the judge name, and try again.")
                Exit Sub
            End If
        End If
        If DataGridView2.DataSource Is Nothing Then Exit Sub
        If InStr(DataGridView2.CurrentRow.Cells("ScoreName").Value.toupper, "POINT") > 0 Then Call AutoEnterPoints()
        If InStr(DataGridView2.CurrentRow.Cells("ScoreName").Value.toupper, "RANK") > 0 Then Call AutoEnterRanks()
        If DataGridView2.IsCurrentCellDirty And DataGridView2.CurrentCell.Value > 0 Then
            DataGridView2.CommitEdit(DataGridViewDataErrorContexts.Commit)
        End If
    End Sub
    Sub thingy() Handles DataGridView2.CellEnter
        If DataGridView2.RowCount - 1 = 0 Or strDebateType <> "WUDC" Then Exit Sub
        'if currentrow is team points, sum individual points
        If DataGridView2.CurrentRow.Cells("Score_ID").Value = 4 Then
            DataGridView2.CurrentRow.Cells("Score").Value = 0
            Dim fdStudents As DataRow()
            fdStudents = DS.Tables("Entry_Student").Select("Entry=" & DataGridView2.CurrentRow.Cells("Recipient").Value, "Last ASC")
            For x = 0 To DataGridView2.RowCount - 1
                For y = 0 To fdStudents.Length - 1
                    If DataGridView2.Rows(x).Cells("Recipient").Value = fdStudents(y).Item("ID") Then
                        DataGridView2.CurrentRow.Cells("Score").Value += DataGridView2.Rows(x).Cells("Score").Value
                    End If
                Next y
            Next x
        End If
        If DataGridView2.CurrentCell.RowIndex = DataGridView2.RowCount - 1 And strDebateType = "WUDC" Then
            Dim hasDupes As Boolean = False
            Dim dt As New DataTable : Dim dr As DataRow
            dt.Columns.Add("Recipient", System.Type.GetType("System.Int64"))
            dt.Columns.Add("Pts", System.Type.GetType("System.Int16"))
            'check for dupes
            For x = 0 To DataGridView2.Rows.Count - 1
                For y = x + 1 To DataGridView2.Rows.Count - 1
                    If DataGridView2.Rows(x).Cells("Score").Value > 0 And (DataGridView2.Rows(x).Cells("Score").Value = DataGridView2.Rows(y).Cells("Score").Value) And DataGridView2.Rows(x).Cells("Score_ID").Value = 4 Then
                        MsgBox("Duplicate point scores have been given.  Please re-enter.")
                        Exit Sub
                    End If
                Next y
            Next x
            'calculate and populate with ranks
            For x = 0 To DataGridView2.Rows.Count - 1 Step 2
                If DataGridView2.Rows(x).Cells("Score_ID").Value = 4 Then
                    dr = dt.NewRow
                    dr.Item("Recipient") = DataGridView2.Rows(x).Cells("Recipient").Value
                    dr.Item("Pts") = DataGridView2.Rows(x).Cells("Score").Value
                    dt.Rows.Add(dr)
                End If
            Next x
            dt.DefaultView.Sort = "Pts Desc"
            For x = 0 To dt.DefaultView.Count - 1
                For y = 0 To DataGridView2.RowCount - 1
                    If DataGridView2.Rows(y).Cells("Score_ID").Value = 5 Then
                        If DataGridView2.Rows(y).Cells("Recipient").Value = dt.DefaultView(x).Item("Recipient") Then
                            DataGridView2.Rows(y).Cells("Score").Value = 3 - x
                        End If
                    End If
                Next y
            Next x
            butSave.BackColor = Color.Red : butSave.Focus()
            DataGridView2.Rows(DataGridView2.CurrentCell.RowIndex).Selected = False
        End If
    End Sub
    Sub AutoEnterRanks()
        If DataGridView2.CurrentCell.Value > 0 And DataGridView2.CurrentCell.Value.ToString.Trim.Length = 1 Then Call AdvanceRow()
    End Sub
    Sub AutoEnterPoints()
        If DataGridView2.CurrentCell.Value.ToString.Trim.Length <> 2 Then Exit Sub
        Dim dummy As Single
        dummy = DataGridView2.CurrentCell.Value
        dummy = dummy / 10
        dummy = dummy + 20
        DataGridView2.CurrentCell.Value = dummy
        Call AdvanceRow()
    End Sub
    Sub AdvanceRow()
        'if its the last row, move to the decision grid if visible, otherwise go to the save button
        If DataGridView2.CurrentCell.RowIndex + 1 >= DataGridView2.RowCount Then
            If grpDecision.Visible = True Then
                Call GuessWinner() 'radTeam1.Focus()
            End If
            butSave.BackColor = Color.Red : butSave.Focus()
            Exit Sub
        End If
        'if not the last row, move to the next row
        DataGridView2.CurrentCell = DataGridView2("Score", DataGridView2.CurrentCell.RowIndex + 1)
    End Sub
    Sub GuessWinner()
        'assumes 2 teams
        Dim Pts1, pts2 As Single
        For x = 0 To DataGridView2.RowCount - 1
            If DataGridView2.Rows(x).Cells("Score_ID").Value = 2 Then
                If x + 1 <= DataGridView2.RowCount / 2 Then
                    Pts1 += DataGridView2.Rows(x).Cells("Score").Value
                Else
                    pts2 += DataGridView2.Rows(x).Cells("Score").Value
                End If
            End If
        Next x
        If Pts1 > pts2 Then radTeam1.Focus() Else radTeam2.Focus()
    End Sub
    Private Sub butClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butClear.Click
        Call ClearDecision()
    End Sub
    Sub ClearDecision()
        For x = 0 To DataGridView2.RowCount - 1
            DataGridView2.Rows(x).Cells("Score").Value = System.DBNull.Value
        Next
        radTeam1.Checked = False
        radTeam2.Checked = False
        radDoubLoss.Checked = False
        radDoubWin.Checked = False
        chkClearJudge.Checked = False
        chkLPW.Checked = False
        lblDecision.Text = ""
        lblSaved.Text = ""
        Dim dummy As String = DataGridView1.Columns(DataGridView1.CurrentCell.ColumnIndex).HeaderText
        dummy = Replace(dummy, "JUDGENAME", "")
        Try 'replace this with a test for the existence of the column
            DataGridView1.CurrentRow.Cells("Entered" & dummy).Value = False
        Catch
        End Try
        DataGridView2.CurrentCell = DataGridView2("Score", 0)
        DataGridView2.Focus()
    End Sub
    Sub DecisionChange() Handles radTeam1.CheckedChanged, radTeam2.CheckedChanged
        Call UpdateDecision()
    End Sub

    Private Sub butSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butSave.Click
        If ValidateBallot() <> "" Then
            Dim q As Integer = MsgBox(ValidateBallot() & "  Continue with save?", MsgBoxStyle.YesNo)
            If q = vbNo Then Exit Sub
        End If
        Call SaveDecision()
        Call SaveFile(DS) 'physically save to file after each completed entry
        DataGridView1.Refresh()
        butSave.BackColor = Color.LightGray
    End Sub
    Sub SaveDecision()
        Dim dummy As String
        dummy = DataGridView1.Columns(DataGridView1.CurrentCell.ColumnIndex).HeaderText
        dummy = Replace(dummy, "JUDGENAME", "")
        'save all the points on the top grid
        Dim dr As DataRow
        For x = 0 To DataGridView2.RowCount - 1
            dr = DS.Tables("Ballot_Score").Rows.Find(DataGridView2.Rows(x).Cells("ID").Value)
            dr.Item("Score") = DataGridView2.Rows(x).Cells("Score").Value
        Next x
        'if present, save a win/loss
        If grpDecision.Visible = False Then
            Call ClearDecision()
            lblSaved.Text = "Decision SAVED."
            Call MarkBallotsIn(DataGridView1.DataSource)
            Exit Sub
        End If
        'get judge number
        Dim Judge As Integer = DataGridView1.CurrentRow.Cells("Judge" & dummy).Value
        Dim Panel As Integer = DataGridView1.CurrentRow.Cells("Panel").Value
        Dim fdBallots, fdBalScores As DataRow()
        'team 1 (aff)
        fdBallots = DS.Tables("Ballot").Select("Judge=" & Judge & " and panel=" & Panel & " and side=1")
        If chkClearJudge.Checked = True Then fdBallots(0).Item("Judge") = -1
        fdBalScores = DS.Tables("Ballot_Score").Select("Ballot=" & fdBallots(0).Item("ID") & " and Score_ID=1")
        If radTeam1.Checked = True Then fdBalScores(0).Item("Score") = 1 Else fdBalScores(0).Item("Score") = 0
        If radDoubWin.Checked = True Then fdBalScores(0).Item("Score") = 1
        If radDoubWin.Checked = True Then fdBalScores(0).Item("Score") = 1
        'team 2 (neg)
        fdBallots = DS.Tables("Ballot").Select("Judge=" & Judge & " and panel=" & Panel & " and side=2")
        If chkClearJudge.Checked = True Then fdBallots(0).Item("Judge") = -1
        fdBalScores = DS.Tables("Ballot_Score").Select("Ballot=" & fdBallots(0).Item("ID") & " and Score_ID=1")
        If radTeam2.Checked = True Then fdBalScores(0).Item("Score") = 1 Else fdBalScores(0).Item("Score") = 0
        If radDoubWin.Checked = True Then fdBalScores(0).Item("Score") = 1
        If radDoubWin.Checked = True Then fdBalScores(0).Item("Score") = 1
        Call ClearDecision()
        lblSaved.Text = "Decision SAVED."
        'mark decision as entered
        DataGridView1.CurrentRow.Cells("Entered" & dummy).Value = True
        Call MarkBallotsIn(DataGridView1.DataSource)
    End Sub
    Private Sub butByeInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butByeInfo.Click
        Dim strInfo As String
        strInfo = "There are generally 3 types of byes; (1) a team is assigned a bye by the tab room and there is no opponent, (2) a debate is scheduled, but someone (team or judge) fails to appear, (3) a debate occurs but for some reason a traditional decision is inappropriate (one team withdraws during the round, etc.)" & Chr(10) & Chr(10)
        strInfo &= "For all sorts of byes, you can enter speaker points or a -1 to have the points averaged." & Chr(10) & Chr(10)
        strInfo &= "If you wish to assign zero speaker points, entering zero points will prevent averaging and award the speakers no points." & Chr(10) & Chr(10)
        strInfo &= "If no debate occured, you should clear the judge information so that the judge will not be credited with having judged a round and will remain eligible to judge the teams in future prelim rounds." & Chr(10) & Chr(10)
        strInfo &= "To remove the judge, check the CLEAR JUDGE box before saving.  If you do not clear the judge, the round will count against the judge's commitment, that is, if they owe 4 rounds, the bye round will count as 1 and you can only assign them to 3 more debates." & Chr(10) & Chr(10)
        strInfo &= "Use one of the 4 decision options to indicate which team should get credit for winning." & Chr(10) & Chr(10)
        strInfo &= "EXAMPLE #1: The affirmative team shows up but the negative team no-shows and you wish to award an aff win and a neg forfeit.  Enter -1 scores for all affirmative point/rank scores, scores of 0 for all negative point/rank scores, check the 'clear judge' box, and mark the aff as the winner." & Chr(10) & Chr(10)
        strInfo &= "EXAMPLE #2: Both teams went to the right room but the judge never showed up and you wish to award a double-bye.  Enter scores of -1 for all point/rank scores, click the double win button, and check the 'clear judge' box." & Chr(10) & Chr(10)
        strInfo &= "EXAMPLE #3: The affirmative team shows up but the negative team becomes ill and withdraws during rebuttal speeches; and you wish to award an aff win, the neg forfeit, and credi the judge for having heard a debate.  Enter -1 scores for all affirmative point/rank scores, scores of 0 for all negative point/rank scores, do NOT check the 'clear judge' box, and mark the aff as the winner." & Chr(10) & Chr(10)
        MsgBox(strInfo, , "How to assign byes")
    End Sub
    Function ValidateBallot() As String
        ValidateBallot = ""
        If IsBye() = True Or strDebateType = "WUDC" Then Exit Function
        ValidateBallot &= LPWProblem()
        'check all scores within range
        ValidateBallot &= ScoresInRange()
        'check point/ranks match
        ValidateBallot &= RankMatch()
        'check duplicates if not allowed
        ValidateBallot &= DupeCheck()
        'check increments
        ValidateBallot &= IncrementCheck()
    End Function
    Function IsBye() As Boolean
        IsBye = True
        For x = 0 To DataGridView2.RowCount - 1
            If DataGridView2.Rows(x).Cells("Score").Value > 0 Then IsBye = False
        Next
    End Function
    Function LPWProblem() As String
        LPWProblem = ""
        'only test if normal decisions and wins are being recorded
        If grpDecision.Visible = False Then Exit Function
        If radDoubLoss.Checked = True Then Exit Function
        If radDoubWin.Checked = True Then Exit Function
        Dim pts1, pts2 As Single
        Dim rank1, rank2 As Integer
        'add up the speaker points
        For x = 0 To DataGridView2.RowCount - 1
            If DataGridView2.Rows(x).Cells("Side").Value = 1 And DataGridView2.Rows(x).Cells("Score_ID").Value = 2 Then
                pts1 += DataGridView2.Rows(x).Cells("Score").Value
            ElseIf DataGridView2.Rows(x).Cells("Side").Value = 2 And DataGridView2.Rows(x).Cells("Score_ID").Value = 2 Then
                pts2 += DataGridView2.Rows(x).Cells("Score").Value
            End If
            If DataGridView2.Rows(x).Cells("Side").Value = 1 And DataGridView2.Rows(x).Cells("Score_ID").Value = 3 Then
                rank1 += DataGridView2.Rows(x).Cells("Score").Value
            ElseIf DataGridView2.Rows(x).Cells("Side").Value = 2 And DataGridView2.Rows(x).Cells("Score_ID").Value = 3 Then
                rank2 += DataGridView2.Rows(x).Cells("Score").Value
            End If
        Next
        If pts1 < pts2 And radTeam1.Checked = True And chkLPW.Checked = False Then
            LPWProblem &= "You have indicated the " & radTeam1.Text & " side won, but they have lower points and the low point win box is not checked.  Either adjust the points or check the low point win box."
        End If
        If pts1 > pts2 And radTeam2.Checked = True And chkLPW.Checked = False Then
            LPWProblem &= "You have indicated the " & radTeam2.Text & " side won, but they have lower points and the low point win box is not checked.  Either adjust the points or check the low point win box."
        End If
        If pts1 > pts2 And radTeam1.Checked = True And chkLPW.Checked = True Then
            LPWProblem &= "You have indicated the " & radTeam1.Text & " side won in a low point win, but they actually had more points.  Either adjust the points or un-check the low point win box."
        End If
        If pts2 > pts1 And radTeam2.Checked = True And chkLPW.Checked = True Then
            LPWProblem &= "You have indicated the " & radTeam2.Text & " side won in a low point win, but they actually had more points.  Either adjust the points or un-check the low point win box."
        End If
        If rank1 > rank2 And radTeam1.Checked = True And chkLPW.Checked = False Then
            LPWProblem &= "You have indicated the " & radTeam1.Text & " side won, but they have higher ranks and the low point win box is not checked.  Either adjust the points or check the low point win box."
        End If
        If rank2 > rank1 And radTeam2.Checked = True And chkLPW.Checked = False Then
            LPWProblem &= "You have indicated the " & radTeam2.Text & " side won, but they have higher ranks and the low point win box is not checked.  Either adjust the points or check the low point win box."
        End If
    End Function
    Function ScoresInRange() As String
        ScoresInRange = ""
        'pull round and get score settings for the tb_set
        Dim fdRd As DataRow : fdRd = DS.Tables("Round").Rows.Find(cboRound.SelectedValue)
        Dim fdSettings()
        fdSettings = DS.Tables("Score_Setting").Select("TB_SET=" & fdRd.Item("TB_SET"))
        For x = 0 To DataGridView2.RowCount - 1
            fdSettings = DS.Tables("Score_Setting").Select("TB_SET=" & fdRd.Item("TB_SET") & " and Score=" & DataGridView2.Rows(x).Cells("Score_ID").Value)
            If DataGridView2.Rows(x).Cells("Score").Value > fdSettings(0).item("Max") Then
                ScoresInRange &= DataGridView2.Rows(x).Cells("RecipientName").Value & " received " & DataGridView2.Rows(x).Cells("score").Value & " " & DataGridView2.Rows(x).Cells("ScoreName").Value & " but the maximum score allowed is " & fdSettings(0).item("Max") & ". "
            End If
            If DataGridView2.Rows(x).Cells("Score").Value < fdSettings(0).item("Min") And DataGridView2.Rows(x).Cells("Score").Value <> -1 Then
                ScoresInRange &= DataGridView2.Rows(x).Cells("RecipientName").Value & " received " & DataGridView2.Rows(x).Cells("score").Value & " " & DataGridView2.Rows(x).Cells("ScoreName").Value & " but the minimum score allowed is " & fdSettings(0).item("Min") & ". "
            End If
        Next x
    End Function
    Function RankMatch() As String
        RankMatch = ""
        'scroll through all rows
        For x = 0 To DataGridView2.RowCount - 1
            'process if speaker rank (score_ID=3) or team rank (score_ID=5)
            If DataGridView2.Rows(x).Cells("Score_ID").Value = 3 Or DataGridView2.Rows(x).Cells("Score_ID").Value = 5 Then
                'scroll through again
                For y = 0 To DataGridView2.RowCount - 1
                    'pull if its the same rank and not the same row
                    If DataGridView2.Rows(y).Cells("Score_ID").Value = DataGridView2.Rows(x).Cells("Score_ID").Value And x <> y Then
                        'make comparison and throw message if there's a problem 
                        If DataGridView2.Rows(y - 1).Cells("Score").Value > DataGridView2.Rows(x - 1).Cells("Score").Value And DataGridView2.Rows(y).Cells("Score").Value > DataGridView2.Rows(x).Cells("Score").Value Then
                            RankMatch &= DataGridView2.Rows(y).Cells("RecipientName").Value & " has more speaker points than " & DataGridView2.Rows(x).Cells("RecipientName").Value & " but received a worse ranking. "
                        End If
                    End If
                Next y
            End If
        Next x
    End Function
    Function DupeCheck() As String
        DupeCheck = ""
        'pull round and get score settings for the tb_set
        Dim fdRd As DataRow : fdRd = DS.Tables("Round").Rows.Find(cboRound.SelectedValue)
        Dim fdSettings()
        fdSettings = DS.Tables("Score_Setting").Select("TB_SET=" & fdRd.Item("TB_SET"))
        For x = 0 To DataGridView2.RowCount - 1
            fdSettings = DS.Tables("Score_Setting").Select("TB_SET=" & fdRd.Item("TB_SET") & " and Score=" & DataGridView2.Rows(x).Cells("Score_ID").Value)
            If fdSettings(0).item("DupesOK") = False Then
                For y = 0 To DataGridView2.RowCount - 1
                    If DataGridView2.Rows(y).Cells("Score").Value = DataGridView2.Rows(x).Cells("Score").Value And x <> y Then
                        DupeCheck &= DataGridView2.Rows(y).Cells("RecipientName").Value & " and " & DataGridView2.Rows(x).Cells("RecipientName").Value & " have the same " & DataGridView2.Rows(y).Cells("ScoreName").Value & " but ties are not allowed for this score. "
                    End If
                Next y
            End If
        Next x
    End Function
    Function IncrementCheck()
        IncrementCheck = ""
        'pull round and get score settings for the tb_set
        Dim fdRd As DataRow : fdRd = DS.Tables("Round").Rows.Find(cboRound.SelectedValue)
        Dim fdSettings()
        fdSettings = DS.Tables("Score_Setting").Select("TB_SET=" & fdRd.Item("TB_SET"))
        For x = 0 To DataGridView2.RowCount - 1
            fdSettings = DS.Tables("Score_Setting").Select("TB_SET=" & fdRd.Item("TB_SET") & " and Score=" & DataGridView2.Rows(x).Cells("Score_ID").Value)
            If fdSettings(0).item("DecimalIncrements") = 0 And Int(DataGridView2.Rows(x).Cells("Score").Value) <> DataGridView2.Rows(x).Cells("Score").Value Then
                IncrementCheck &= DataGridView2.Rows(x).Cells("RecipientName").Value & " has a " & DataGridView2.Rows(x).Cells("ScoreName").Value & " score of " & DataGridView2.Rows(x).Cells("Score").Value & " but only whole integers are allowed for this score."
            End If
            If fdSettings(0).item("DecimalIncrements") = 0.5 And (Mid(DataGridView2.Rows(x).Cells("Score").Value.ToString, DataGridView2.Rows(x).Cells("Score").Value.ToString.Length, 1) <> "0" And Mid(DataGridView2.Rows(x).Cells("Score").Value.ToString, DataGridView2.Rows(x).Cells("Score").Value.ToString.Length, 1) <> "5") And Int(DataGridView2.Rows(x).Cells("Score").Value) <> DataGridView2.Rows(x).Cells("Score").Value Then
                IncrementCheck &= DataGridView2.Rows(x).Cells("RecipientName").Value & " has a " & DataGridView2.Rows(x).Cells("ScoreName").Value & " score of " & DataGridView2.Rows(x).Cells("Score").Value & " but only .5 increments are allowed for this score."
            End If
            If fdSettings(0).item("DecimalIncrements") = 0.1 And Len(DataGridView2.Rows(x).Cells("Score").Value.ToString) - InStr(DataGridView2.Rows(x).Cells("Score").Value.ToString, ".") > 1 And InStr(DataGridView2.Rows(x).Cells("Score").Value.ToString, ".") > 0 Then
                IncrementCheck &= DataGridView2.Rows(x).Cells("RecipientName").Value & " has a " & DataGridView2.Rows(x).Cells("ScoreName").Value & " score of " & DataGridView2.Rows(x).Cells("Score").Value & " but only .1 increments are allowed for this score."
            End If
        Next x
    End Function

    Private Sub butWebDownload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butWebDownload.Click
        If DataGridView1.RowCount = 0 Then MsgBox("Please load a round before attempting the online download") : Exit Sub
        Dim dtDummy As DateTime = Now
        'Call DoBallotDownLoad()
        Call DoNewBallotDownLoad(DS, cboRound.SelectedValue)
        'download and save
        Dim strdummy As String = DateDiff(DateInterval.Second, dtDummy, Now)
        Label1.Text = "Done.  Seconds elapsed:" & strdummy : Label1.Refresh()
        'Call UpdateFromWeb()
        Call UpdateFromWeb3()
        Call MarkBallotsIn(DataGridView1.DataSource)
        Label1.Text &= DateDiff(DateInterval.Second, dtDummy, Now).ToString & " seconds to process."
    End Sub
    Sub DoBallotDownLoad()
        'Hey -- only download the event that's in question?
        'Dim URL As String = "https://www.tabroom.com/api/download_tourn.mhtml?username=jbruschke@fullerton.edu&password=123Eadie&tourn_id=1499&event_id=16866,16867,16868"
        'Dim dr As DataRow : dr = DS.Tables("Round").Rows.Find(cboRound.SelectedValue)

        'Dim URL As String = "https://www.tabroom.com/api/download_tourn.mhtml?username=jbruschke@fullerton.edu&password=123Eadie&tourn_id=" & DS.Tables("Tourn").Rows(0).Item("ID") & "&event_id=" & dr.Item("Event")
        'Label1.Text = "Opening site..." : Label1.Refresh()
        'Dim request As HttpWebRequest = WebRequest.Create(URL)
        'Dim response As HttpWebResponse = request.GetResponse()
        'Dim reader As StreamReader = New StreamReader(response.GetResponseStream())
        'Dim st As StreamWriter = File.CreateText("C:\Users\jbruschke\Documents\CAT\BallotDownload.xml")
        'st.WriteLine(reader.ReadToEnd())
        'st.Close()
        'response.Close() : reader.Close()
    End Sub
    Sub UpdateFromWeb()
        'read it in
        Dim xmlFile As XmlReader
        xmlFile = XmlReader.Create(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\CAT\BallotDownload.xml", New XmlReaderSettings())
        ds2.ReadXml(xmlFile, XmlReadMode.InferSchema)
        xmlFile.Close()
        'pull from the web round; assumes event and rd_name, not ID
        Dim dr As DataRow : Dim dr2, fdWebBallots, fdLocalBallots, fdWebScores, fdLocalScores As DataRow()
        dr = DS.Tables("Round").Rows.Find(cboRound.SelectedValue)
        dr2 = ds2.Tables("Round").Select("Event=" & dr.Item("Event") & " and rd_name='" & dr.Item("rd_name") & "'")
        fdWebBallots = PullAllBallotsInRound(ds2, dr2(0).Item("ID"), "Judge ASC") 'now holds all ballots from the downloaded file for this round
        fdLocalBallots = PullAllBallotsInRound(DS, cboRound.SelectedValue, "Judge ASC") 'now holds all ballots from the local file for this round
        'make sure all the scores are there
        For x = 0 To fdLocalBallots.Length - 1
            ValidateScoresByBallot(DS, fdLocalBallots(x).Item("ID"))
        Next x
        'match ballots by judge and entry
        'match scores by recipient and score_ID, and over-write
        For x = 0 To fdLocalBallots.Length - 1
            Label1.Text = "Checking " & x & " of " & fdLocalBallots.Length - 1 : Label1.Refresh()
            If IsBallotIn(DS, fdLocalBallots(x).Item("ID")) = False Then
                For y = 0 To fdWebBallots.Length - 1
                    If fdWebBallots(y).Item("Judge") = fdLocalBallots(x).Item("Judge") And fdWebBallots(y).Item("Entry") = fdLocalBallots(x).Item("Entry") Then
                        'If fdWebBallots(y).Item("Judge") = 107901 Then MsgBox("jim")
                        'pull the scores and over-write
                        fdWebScores = ds2.Tables("Ballot_Score").Select("Ballot=" & fdWebBallots(y).Item("ID"))
                        fdLocalScores = DS.Tables("Ballot_Score").Select("Ballot=" & fdLocalBallots(x).Item("ID"))
                        For z = 0 To fdWebScores.Length - 1
                            'If fdWebScores(z).Item("Score_ID") = 3 Then MsgBox(fdWebBallots(y).Item("Judge"))
                            For w = 0 To fdLocalScores.Length - 1
                                If fdWebScores(z).Item("Recipient") = fdLocalScores(w).Item("Recipient") And fdWebScores(z).Item("Score_ID") = fdLocalScores(w).Item("Score_ID") Then
                                    fdLocalScores(w).Item("Score") = fdWebScores(z).Item("Score")
                                End If
                            Next w
                        Next z
                    End If
                Next y
            End If
        Next x
        Label1.Text &= " COMPLETE."
    End Sub
    Sub UpdateFromWeb2()
        'read it in
        Dim xmlFile As XmlReader
        xmlFile = XmlReader.Create(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\CAT\BallotDownload.xml", New XmlReaderSettings())
        ds2.ReadXml(xmlFile, XmlReadMode.InferSchema)
        xmlFile.Close()
        'pull from the web round; assumes event and rd_name, not ID
        Dim dr As DataRow : Dim fdLocalBallots, fdWebScores, fdLocalScores As DataRow()
        dr = DS.Tables("Round").Rows.Find(cboRound.SelectedValue)
        fdLocalBallots = PullAllBallotsInRound(DS, cboRound.SelectedValue, "Judge ASC") 'now holds all ballots from the local file for this round
        'make sure all the scores are there
        For x = 0 To fdLocalBallots.Length - 1
            ValidateScoresByBallot(DS, fdLocalBallots(x).Item("ID"))
        Next x
        'match ballots by judge and entry
        'match scores by recipient and score_ID, and over-write
        Dim q As Integer
        For x = 0 To fdLocalBallots.Length - 1
            Label1.Text = "Checking " & x & " of " & fdLocalBallots.Length - 1 : Label1.Refresh()
            If IsBallotIn(DS, fdLocalBallots(x).Item("ID")) = False Then
                For y = 0 To ds2.Tables("Ballot").Rows.Count - 1
                    If ds2.Tables("Ballot").Rows(y).Item("Judge") = fdLocalBallots(x).Item("Judge") And ds2.Tables("Ballot").Rows(y).Item("Entry") = fdLocalBallots(x).Item("Entry") Then
                        'If fdWebBallots(y).Item("Judge") = 107901 Then MsgBox("jim")
                        'pull the scores and over-write
                        fdWebScores = ds2.Tables("Ballot_Score").Select("Ballot=" & ds2.Tables("Ballot").Rows(y).Item("ID"))
                        fdLocalScores = DS.Tables("Ballot_Score").Select("Ballot=" & fdLocalBallots(x).Item("ID"))
                        For z = 0 To fdWebScores.Length - 1
                            'If fdWebScores(z).Item("Score_ID") = 3 Then MsgBox(fdWebBallots(y).Item("Judge"))
                            For w = 0 To fdLocalScores.Length - 1
                                If fdWebScores(z).Item("Recipient") = fdLocalScores(w).Item("Recipient") And fdWebScores(z).Item("Score_ID") = fdLocalScores(w).Item("Score_ID") Then
                                    q = vbYes
                                    'q = MsgBox(fdWebScores(z).Item("Audit"))
                                    If fdWebScores(z).Item("Score") = 0 And fdLocalScores(w).Item("Score") > 0 Then
                                        q = MsgBox("Web has a score of zero and local score is greater than zero; write ZERO on top of existing score?  Say NO unless you are very sure!", MsgBoxStyle.YesNo)
                                    End If
                                    'replace this when the audit comes through
                                    If fdWebScores(z).Item("Score") <> fdLocalScores(w).Item("Score") And fdLocalScores(w).Item("Score") > 0 Then
                                        q = MsgBox("Web score is different from local score and local score is greater than zero; write web score on top of existing score?  Say NO unless you are very sure!", MsgBoxStyle.YesNo)
                                    End If
                                    If q = vbYes Then fdLocalScores(w).Item("Score") = fdWebScores(z).Item("Score")
                                End If
                            Next w
                        Next z
                    End If
                Next y
            End If
        Next x
        Label1.Text &= " COMPLETE."
    End Sub
    Sub UpdateFromWeb3()
        'read it in
        Dim xmlFile As XmlReader
        xmlFile = XmlReader.Create(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\CAT\BallotDownload.xml", New XmlReaderSettings())
        ds2.ReadXml(xmlFile, XmlReadMode.InferSchema)
        xmlFile.Close()
        If ds2 Is Nothing Then
            MsgBox("There do not appear to be any online ballots for this round; this is probably because no judge has entered a decision yet.  Check to make sure that at least one judge has entered an online ballot and try again.")
            Exit Sub
        End If
        'pull from the web round; assumes event and rd_name, not ID
        Dim dr, drRecipient As DataRow : Dim fdLocalBallots, fdWebScores, fdLocalScores As DataRow()
        dr = DS.Tables("Round").Rows.Find(cboRound.SelectedValue)
        fdLocalBallots = PullAllBallotsInRound(DS, cboRound.SelectedValue, "Judge ASC") 'now holds all ballots from the local file for this round
        'make sure all the scores are there
        For x = 0 To fdLocalBallots.Length - 1
            ValidateScoresByBallot(DS, fdLocalBallots(x).Item("ID"))
        Next x
        'match ballots by judge and entry
        'match scores by recipient and score_ID, and over-write
        Dim q As Integer
        For x = 0 To fdLocalBallots.Length - 1
            Label1.Text = "Checking " & x & " of " & fdLocalBallots.Length - 1 : Label1.Refresh()
            For y = 0 To ds2.Tables("Ballot").Rows.Count - 1
                If ds2.Tables("Ballot").Rows(y).Item("Judge") = fdLocalBallots(x).Item("Judge") And ds2.Tables("Ballot").Rows(y).Item("Entry") = fdLocalBallots(x).Item("Entry") Then
                    'If fdWebBallots(y).Item("Judge") = 107901 Then MsgBox("jim")
                    'pull the scores and over-write
                    fdWebScores = ds2.Tables("Ballot_Score").Select("Ballot=" & ds2.Tables("Ballot").Rows(y).Item("ID"))
                    fdLocalScores = DS.Tables("Ballot_Score").Select("Ballot=" & fdLocalBallots(x).Item("ID"))
                    For z = 0 To fdWebScores.Length - 1
                        'If fdWebScores(z).Item("Score_ID") = 3 Then MsgBox(fdWebBallots(y).Item("Judge"))
                        For w = 0 To fdLocalScores.Length - 1
                            If fdWebScores(z).Item("Recipient") = fdLocalScores(w).Item("Recipient") And fdWebScores(z).Item("Score_ID") = fdLocalScores(w).Item("Score_ID") Then
                                q = vbYes
                                'q = MsgBox(fdWebScores(z).Item("Audit"))
                                If fdWebScores(z).Item("Score") = 0 And fdLocalScores(w).Item("Score") > 0 Then
                                    If fdWebScores(z).Item("Score_ID") = 1 Then
                                        drRecipient = DS.Tables("Entry").Rows.Find(fdWebScores(z).Item("Recipient"))
                                        q = MsgBox("Web ballots shows " & drRecipient.Item("FullName") & " losing and local ballot shows them winning; change the win to a loss?  Say NO unless you are very sure!", MsgBoxStyle.YesNo)
                                    ElseIf fdWebScores(z).Item("Score_ID") = 2 Then
                                        drRecipient = DS.Tables("Entry_Student").Rows.Find(fdWebScores(z).Item("Recipient"))
                                        q = MsgBox("Web ballots shows " & drRecipient.Item("First") & " " & drRecipient.Item("Last") & " with speaker points of zero and local ballot shows them with " & fdLocalScores(w).Item("Score") & "; write ZERO on top of existing score?  Say NO unless you are very sure!", MsgBoxStyle.YesNo)
                                        q = MsgBox("Web ballots shows " & drRecipient.Item("First") & " " & drRecipient.Item("Last") & " with rank of zero and local ballot shows them with " & fdLocalScores(w).Item("Score") & "; write ZERO on top of existing score?  Say NO unless you are very sure!", MsgBoxStyle.YesNo)
                                    End If
                                End If
                                'replace this when the audit comes through
                                If (fdWebScores(z).Item("Score_ID") >= 2 And fdWebScores(z).Item("Score_ID") <= 3) And fdWebScores(z).Item("Score") <> fdLocalScores(w).Item("Score") And fdLocalScores(w).Item("Score") > 0 Then
                                    drRecipient = DS.Tables("Entry_Student").Rows.Find(fdWebScores(z).Item("Recipient"))
                                    If fdWebScores(z).Item("Score_ID") = 2 Then q = MsgBox(drRecipient.Item("First").trim & " " & drRecipient.Item("Last").trim & " has an online speaker point score of " & fdWebScores(z).Item("Score") & " and a local score of " & fdLocalScores(w).Item("Score") & ".  Write web score on top of existing score?  Say NO unless you are very sure!", MsgBoxStyle.YesNo)
                                    If fdWebScores(z).Item("Score_ID") = 3 Then q = MsgBox(drRecipient.Item("First").trim & " " & drRecipient.Item("Last").trim & " has an online speaker rank score of " & fdWebScores(z).Item("Score") & " and a local score of " & fdLocalScores(w).Item("Score") & ".  Write web score on top of existing score?  Say NO unless you are very sure!", MsgBoxStyle.YesNo)
                                End If
                                If q = vbYes Then fdLocalScores(w).Item("Score") = fdWebScores(z).Item("Score")
                            End If
                        Next w
                    Next z
                End If
            Next y
        Next x
        Label1.Text &= " COMPLETE."
    End Sub
    Sub OldCode()
        Dim y As Integer : Dim str As String
        Dim URL As String = "http://commweb.fullerton.edu/jbruschke/web/BallotView.aspx?BallotID="
        Dim request As HttpWebRequest = WebRequest.Create(URL)
        Dim response As HttpWebResponse = request.GetResponse()
        Dim reader As StreamReader = New StreamReader(response.GetResponseStream())
        Dim st As StreamWriter = File.CreateText("C:\CatDummy.txt")
        y = 0
        Do
            y = y + 1
            str = reader.ReadLine()
            st.WriteLine(str)
        Loop Until InStr(str.ToUpper, "</HTML>") > 0 Or y > 100
        st.Close()
        response.Close() : reader.Close()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Call UpdateFromWeb()
        MsgBox("Done")
    End Sub

    Private Sub butEraseDecision_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butEraseDecision.Click
        'mark decision as entered
        Dim dummy As String = DataGridView1.Columns(DataGridView1.CurrentCell.ColumnIndex).HeaderText
        dummy = Replace(dummy, "JUDGENAME", "")
        DataGridView1.CurrentRow.Cells("Entered" & dummy).Value = False
        'get judge number
        Dim Judge As Integer = DataGridView1.CurrentRow.Cells("Judge" & dummy).Value
        Dim Panel As Integer = DataGridView1.CurrentRow.Cells("Panel").Value
        Dim fdBallots, fdBalScores As DataRow()
        'Pull all ballots by the judge and set all scores to zero
        fdBallots = DS.Tables("Ballot").Select("Judge=" & Judge & " and panel=" & Panel)
        For x = 0 To fdBallots.Length - 1
            fdBalScores = DS.Tables("Ballot_score").Select("Ballot=" & fdBallots(x).Item("ID"))
            For y = 0 To fdBalScores.Length - 1
                fdBalScores(y).Item("Score") = 0
            Next y
        Next x
        Call ClearDecision()
        lblSaved.Text = "Decision CLEARED."
        Call SaveFile(DS) 'now physically save to the disk
        Call MarkBallotsIn(DataGridView1.DataSource) 'update ballot counts
    End Sub
    Sub ShowElimBallots()
        'dead code --- assumes you can use the makepairing function, but it doesn't work with the ballot download file
        'read it in
        Dim xmlFile As XmlReader
        xmlFile = XmlReader.Create(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\CAT\BallotDownload.xml", New XmlReaderSettings())
        Dim strXsdLocation As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\CAT\TourneyData.xsd"
        ds2.ReadXmlSchema(strXsdLocation)
        ds2.ReadXml(xmlFile, XmlReadMode.InferSchema)
        xmlFile.Close()
        'pull from the web round; assumes event and rd_name, not ID
        Dim dr As DataRow : Dim dr2, fdWebBallots As DataRow()
        dr = DS.Tables("Round").Rows.Find(cboRound.SelectedValue)
        dr2 = ds2.Tables("Round").Select("Event=" & dr.Item("Event") & " and rd_name='" & dr.Item("rd_name") & "'")
        fdWebBallots = PullAllBallotsInRound(ds2, dr2(0).Item("ID"), "Judge ASC") 'now holds all ballots from the downloaded file for this round
        Dim dt As New DataTable
        dt = MakePairingTable(DS, dr2(0).Item("ID"), "FULL")
        DataGridView1.DataSource = dt : Exit Sub
        Dim dt2 As New DataTable
        dt2.Columns.Add("Judge", System.Type.GetType("System.String"))
        dt2.Columns.Add("VotesFor", System.Type.GetType("System.String"))
        For x = 0 To dt.Rows.Count - 1
            For y = 0 To dt.Columns.Count - 1
                If InStr(dt.Columns(y).ColumnName.ToUpper, "BALLOT") > 1 Then
                    If dt.Rows(x).Item(y) Is System.DBNull.Value Then dt.Rows(x).Item(y) = 0
                    If dt.Rows(x).Item(y) = 1 Then
                        dr = dt2.NewRow
                        dr.Item("Judge") = dt.Rows(x).Item(GetIDString(dt.Columns(y).ColumnName.ToString, "JUDGE"))
                        dr.Item("VotesFor") = dt.Rows(x).Item(GetIDString(dt.Columns(y).ColumnName.ToString, "TEAM"))
                        dt2.Rows.Add(dr)
                    End If
                End If
            Next y
        Next x
        DataGridView1.DataSource = dt2
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    End Sub
    Sub showelimballots2()
        'read it in
        Dim xmlFile As XmlReader
        xmlFile = XmlReader.Create(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\CAT\BallotDownload.xml", New XmlReaderSettings())
        Dim strXsdLocation As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\CAT\TourneyData.xsd"
        ds2.ReadXmlSchema(strXsdLocation)
        ds2.ReadXml(xmlFile, XmlReadMode.InferSchema)
        xmlFile.Close()
        'pull from the web round; assumes event and rd_name, not ID
        Dim dr, drT2 As DataRow : Dim dr2, fdWebBallots As DataRow()
        dr = DS.Tables("Round").Rows.Find(cboRound.SelectedValue)
        dr2 = ds2.Tables("Round").Select("Event=" & dr.Item("Event") & " and rd_name='" & dr.Item("rd_name") & "'")
        fdWebBallots = PullAllBallotsInRound(ds2, dr2(0).Item("ID"), "Judge ASC") 'now holds all ballots from the downloaded file for this round
        Dim dt2 As New DataTable
        dt2.Columns.Add("Judge", System.Type.GetType("System.String"))
        dt2.Columns.Add("VotesFor", System.Type.GetType("System.String"))
        Dim fdballotscores As DataRow()
        For x = 0 To fdWebBallots.Length - 1
            fdballotscores = ds2.Tables("Ballot_Score").Select("Ballot=" & fdWebBallots(x).Item("ID"))
            For y = 0 To fdballotscores.Length - 1
                If fdballotscores(y).Item("Score_ID") = 1 And fdballotscores(y).Item("Score") = 1 Then
                    dr = DS.Tables("Judge").Rows.Find(fdWebBallots(x).Item("Judge"))
                    drT2 = dt2.NewRow
                    drT2.Item("Judge") = dr.Item("Last") & ", " & dr.Item("First")
                    dr = DS.Tables("Entry").Rows.Find(fdWebBallots(x).Item("Entry"))
                    drT2.Item("VotesFor") = dr.Item("Fullname")
                    dt2.Rows.Add(drT2)
                    Call StoreResult(DS, dt2, fdWebBallots(x).Item("Judge"), fdWebBallots(x).Item("Entry"), cboRound.SelectedValue)
                End If
            Next y
        Next x
        DataGridView1.DataSource = dt2
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    End Sub
    Function GetIDString(ByVal strdummy As String, ByVal strMatch As String) As String
        GetIDString = ""
        For x = 1 To 20
            If strMatch = "JUDGE" Then
                If InStr(strdummy, "Judge" & x.ToString.Trim) > 0 Then GetIDString = "JudgeName" & x.ToString.Trim : Exit Function
            ElseIf strMatch = "TEAM" Then
                If InStr(strdummy, "Team" & x.ToString.Trim) > 0 Then GetIDString = "TeamName" & x.ToString.Trim : Exit Function
            End If
        Next x
    End Function
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim dtDummy As DateTime = Now
        'Call DoBallotDownLoad()
        Call DoNewBallotDownLoad(DS, cboRound.SelectedValue)
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        Dim strdummy As String = DateDiff(DateInterval.Second, dtDummy, Now)
        Label1.Text = "Download done.  Seconds elapsed:" & strdummy : Label1.Refresh()
        DataGridView1.DataSource = showelimballots3(DS, cboRound.SelectedValue)
    End Sub

    Private Sub but3PersonHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles but3PersonHelp.Click
        Dim strInfo As String
        strInfo = "If you are running a 2-person event but one team has 3 speakers who rotate competing, this situation applies to you.  Otherwise, you can ignore it." & Chr(10) & Chr(10)
        strInfo &= "Enter scores of zero for the speaker who did not compete (for all scores that display on the ballot, including points and ranks)." & Chr(10) & Chr(10)
        strInfo &= "This will throw error messages, which you can ignore.  However, you should carefully read through the error message to make sure there aren't additional potential difficulties with the ballot (such as unmarked low point wins)." & Chr(10) & Chr(10)
        MsgBox(strInfo, , "Ballot entry for 3-person teams in 2-person events")
    End Sub

    Private Sub butHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butHelp.Click
        Dim strInfo As String
        strInfo = "Begin by selecting a round and clicking load.  If you check 'sort by entered' the un-entered ballots will appear at the top." & Chr(10) & Chr(10)
        strInfo &= "POLICY BALLOT ENTRY" & Chr(10) & Chr(10)
        strInfo &= "If you are using online ballot entry, click the 'download web ballots' button in the bottom-right." & Chr(10) & Chr(10)
        strInfo &= "For manual ballot entry, click on the name of a judge." & Chr(10) & Chr(10)
        strInfo &= "If you check the 'autocomplete decimal entries' box, you need only enter the last 2 digits for speaker points without a period, so for a 27.5 you would only enter 75" & Chr(10) & Chr(10)
        strInfo &= "Select a decision from the radio buttons at the bottom, and then click the 'save decision' button.  The screen will blank out and the decision will be indicated as entered in the left-hand box." & Chr(10) & Chr(10)
        strInfo &= "If the selections in the right-hand ballot grid get messed up, you can reset it by clicking the 'clear entry screen' button.  This will not erase the decision, but will clear the ballot entry." & Chr(10) & Chr(10)
        strInfo &= "To completely erase an entered decision, click a judge name on the left-hand grid and then the 'clear/erase decision' button." & Chr(10) & Chr(10)
        strInfo &= "See the special help buttons for 3-person team and bye entry should you encounter those situations." & Chr(10) & Chr(10)
        strInfo &= "WUDC BALLOT ENTRY" & Chr(10) & Chr(10)
        strInfo &= "Enter the points for each speaker; after entering points for the second speaker and hitting return the team points will automatically calculate." & Chr(10) & Chr(10)
        strInfo &= "Hit return to advance to the first speaker on the next team (a total of 3 returns after entering the points for the second speaker)." & Chr(10) & Chr(10)
        strInfo &= "When you enter points for the last team, team ranks will be automatically populated, and the SAVE button will automatically be highlighted." & Chr(10) & Chr(10)
        strInfo &= "Note that ranks will automatically be converted into points, so that 1st=3 points, 2nd=2 points, 3rd=1 point, 4th=zero points." & Chr(10) & Chr(10)
        MsgBox(strInfo, , "How to use the page")
    End Sub

    Private Sub butFlipSides_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butFlipSides.Click
        Dim Panel As Integer = DataGridView1.CurrentRow.Cells("Panel").Value
        Call SideFlipper(DS, Panel)
        Call ClearDecision()
        Call ShowThePairing()
    End Sub
    Sub ClearGrids() Handles cboRound.SelectedValueChanged
        DataGridView1.DataSource = Nothing
        DataGridView2.DataSource = Nothing
    End Sub
End Class