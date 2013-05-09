Public Class frmPair
    Dim ds As New DataSet
    Private Sub frmPair_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call LoadFile(ds, "TourneyData")
        'bind round CBO
        cboRound.DataSource = ds.Tables("Round")
        cboRound.DisplayMember = "Label"
        cboRound.ValueMember = "ID"
        cboRound.Focus()

    End Sub
    Private Sub frmPair_Unload(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Disposed
        Call SaveFile(ds)
        ds.Dispose()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim x, y As Integer
        ListBox1.Items.Clear()
        ds.Tables("Round").DefaultView.Sort = "ID"
        For x = ds.Tables("Panel").Rows.Count - 1 To 0 Step -1
            ds.Tables("panel").Rows(x).Delete()
        Next x
        Dim worked As Boolean
        For x = 0 To 0 'ds.Tables("ROUND").Rows.Count - 1
            If ds.Tables("Round").Rows(x).Item("RD_NAME") <= 9 Then
                worked = False
                Do
                    worked = randompair(ds.Tables("Round").Rows(x).Item("ID"))
                    If worked = False Then
                        For y = 0 To ds.Tables("Panel").Rows.Count - 1
                            If ds.Tables("Panel").Rows(y).Item("Round") = ds.Tables("Round").Rows(x).Item("ID") Then ds.Tables("Panel").Rows(y).Delete()
                        Next y
                        ds.AcceptChanges()
                    End If
                Loop While worked = False
            End If
        Next x
        Call SaveFile(ds)
        Label1.Text = "Done -- rounds saved"
    End Sub
    Function randompair(ByVal round As Integer) As Boolean
        'just define some variables you'll need later
        Dim panel, z, ctr As Integer
        'pull all teams in event
        Dim teams As DataRow()
        Dim eventID As Integer = GetRowInfo(ds, "Round", round, "Event")
        Dim DT As New DataTable
        DT = ds.Tables("Entry").Copy
        DT.Columns.Add("Used", System.Type.GetType("System.Boolean"))
        teams = DT.Select("Event=" & eventID)
        For x = 0 To teams.Length - 1
            teams(x).Item("Used") = False
        Next
        'find number of teams
        Dim TeamsPerRd As Integer = getEventSetting(ds, eventID, "TeamsPerRound")
        'get the number of debates based on the number of teams per round
        Dim nDebates As Integer = Int(teams.Length / TeamsPerRd)
        'scroll through debates
        Dim fdballot As DataRow()
        Dim drAvailTeams As DataRow()
        Dim FitsInRound As Boolean
        For x = 1 To nDebates
            Label1.Text = "Rd " & round & " debate " & x.ToString & " of " & nDebates.ToString : Label1.Refresh()
            'create a new panel and get its ID number
            panel = AddPanel(ds, round, 0)
            'scroll through the number of teams per round
            For y = 1 To TeamsPerRd
                'pull the current ballots for the panel
                fdballot = ds.Tables("Ballot").Select("Panel=" & panel)
                'pull remaining teams
                drAvailTeams = DT.Select("Used=False and Event=" & eventID)
                'pull at random from remaining teams until they fit
                ctr = 0
                Do
                    z = 0
                    If drAvailTeams.Length > 1 Then z = Int(Rnd() * drAvailTeams.Length) : ctr += 1
                    'check team fits against all opponents
                    FitsInRound = True
                    For w = 0 To fdballot.Length - 1
                        'skip if entry is blank
                        If fdballot(w).Item("Entry") <> -99 Then
                            If CanDebate(ds, fdballot(w).Item("Entry"), drAvailTeams(z).Item("ID")) = "" Then FitsInRound = False
                        End If
                    Next
                    'If ctr = 10 Then MsgBox("10")
                Loop Until FitsInRound = True Or ctr > 20
                'add them if they fit
                If FitsInRound = True Then
                    AddTeamToPanel(ds, panel, drAvailTeams(z).Item("ID"), y)
                    drAvailTeams(z).Item("Used") = True
                End If
            Next y
        Next x
        'check number of leftovers; exit if 0, bye if 1,stick rest on a panel if >1
        drAvailTeams = DT.Select("Used=False and Event=" & eventID)
        If drAvailTeams.Length > 0 Then
            panel = AddPanel(ds, round, 0)
            For x = 0 To drAvailTeams.Length - 1
                AddTeamToPanel(ds, panel, drAvailTeams(x).Item("ID"), x + 1)
            Next x
        End If
        'Now ADD JUDGES
        'find the number of judges
        Dim drRound As DataRow 'Dim strMS As String
        drRound = ds.Tables("Round").Rows.Find(round)
        Dim fdPanels As DataRow()
        fdPanels = ds.Tables("Panel").Select("Round=" & round)
        'strMS = "Start scroll " & Now.Millisecond
        For x = 0 To fdPanels.Length - 1
            Label1.Text = "Rd " & round & " judge " & x.ToString & " of " & fdPanels.Length - 1.ToString : Label1.Refresh()
            For y = 1 To drRound.Item("JudgesPerPanel")
                For z = 0 To ds.Tables("Judge").Rows.Count - 1
                    'strMS = "Can judge " & Now.Millisecond
                    If CanJudge(ds, ds.Tables("Judge").Rows(z).Item("ID"), fdPanels(x).Item("ID"), False) Then
                        'strMS = "add judge " & Now.Millisecond
                        AddJudgeToPanel(ds, fdPanels(x).Item("ID"), ds.Tables("Judge").Rows(z).Item("ID"), 1)
                        Exit For
                        'strMS = "Add Judge Done " & Now.Millisecond
                    End If
                Next z
            Next y
        Next x
        randompair = True
        Label1.Text = "Done" : Label1.Refresh()
    End Function
    Sub GiveBye(ByVal team As Integer, ByVal round As Integer)
        Dim dr As DataRow
        dr = ds.Tables("PANEL").NewRow
        dr.Item("Round") = round
        ds.Tables("Panel").Rows.Add(dr)
        ds.Tables("Panel").AcceptChanges()
        Dim PanelID = dr.Item("ID")
        dr = ds.Tables("Ballot").NewRow
        dr.Item("Panel") = PanelID
        dr.Item("Judge") = -1
        dr.Item("Entry") = team
        dr.Item("Side") = -1
        ds.Tables("Ballot").Rows.Add(dr)
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        'shows the round
        Dim x As Integer = InputBox("Enter Round number")
        Dim dt As DataTable = MakePairingTable(ds, x, "Full")
        DataGridView1.DataSource = dt
        
    End Sub
    Function DebatingNow(ByVal team As Integer, ByVal round As Integer) As Boolean
        'only retaining for speed tests

        DebatingNow = False

        Dim foundpanel As DataRow()
        Dim foundballot As DataRow()

        'limit panels by round
        foundpanel = ds.Tables("Panel").Select("Round=" & round)

        'pull all ballots on panel
        For x = 0 To foundpanel.Length - 1
            foundballot = ds.Tables("Ballot").Select("Panel=" & foundpanel(x).Item("ID") & " and entry=" & team)
            If foundballot.Length > 0 Then DebatingNow = True : Exit Function
        Next x

    End Function
    Function debatingnow2(ByVal team As Integer, ByVal round As Integer) As Integer
        'only retaining for speed tests

        debatingnow2 = 0

        'pull all the ballots with team
        Dim foundballot As DataRow()
        foundballot = ds.Tables("Ballot").Select("Entry=" & team)

        Dim dr As DataRow
        For x = 0 To foundballot.Length - 1
            dr = ds.Tables("Panel").Rows.Find(foundballot(x).Item("Panel"))
            If dr.Item("Round") = round Then debatingnow2 += 1
        Next x

    End Function
    Function debatingnow3(ByVal team As Integer, ByVal round As Integer) As Boolean
        'only retaining for speed tests
        debatingnow3 = False
        Dim ds2 As New DataSet
        ds2 = ds.Copy
        For x = ds2.Tables("Round").Rows.Count - 1 To 0 Step -1
            If ds2.Tables("Round").Rows(x).Item("ID") <> round Then ds2.Tables("Round").Rows(x).Delete()
        Next x

        'pull all the ballots with team
        Dim dr As DataRow()
        dr = ds.Tables("Ballot").Select("Entry=" & team)
        If dr.Length > 0 Then debatingnow3 = True

    End Function
    Function debatingnow4(ByVal team As Integer, ByVal dt As DataTable) As Boolean
        'only retaining for speed tests
        debatingnow4 = False
        Dim i As Integer = -1
        i = dt.DefaultView.Find(team)
        If i > -1 Then debatingnow4 = True
    End Function
    

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        'THE SPEED TEST
        Dim dummy As String
        Dim dummy2 As Boolean

        dummy = Now.Millisecond
        dummy2 = DebatingNow(4347, 349)
        dummy &= " " & Now.Millisecond
        Label1.Text = "FWD SELECT " & dummy

        dummy = Now.Millisecond
        dummy2 = debatingnow2(4347, 349)
        dummy &= " " & Now.Millisecond
        Label1.Text &= "  BALLOTS 1ST " & dummy

        dummy = Now.Millisecond
        dummy2 = debatingnow3(4347, 349)
        dummy &= " " & Now.Millisecond
        Label1.Text &= "  DUPE DS  " & dummy

        Dim ds2 As New DataSet
        ds2 = ds.Copy
        For x = ds2.Tables("Round").Rows.Count - 1 To 0 Step -1
            If ds2.Tables("Round").Rows(x).Item("ID") <> 349 Then ds2.Tables("Round").Rows(x).Delete()
        Next x
        Dim dt As New DataTable
        dt = ds2.Tables("Ballot")
        'dt.DefaultView.Sort = "entry asc"
        dummy = Now.Millisecond
        dummy2 = debatingnow4(4347, dt)
        dummy &= " " & Now.Millisecond
        Label1.Text &= "  DUPE 1ST  " & dummy

    End Sub

    Private Sub butHackPrelimResults_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butHackPrelimResults.Click
        Dim dr As DataRow
        Dim y, z, w As Integer : Dim scores(5) As Integer
        Dim arrJudges(), arrTeams() As Integer
        Dim spts(3, 4) As Decimal : Dim srank(3, 4) As Integer
        Dim win() As Integer : Dim tmrank() As Integer : Dim tmPts() As Decimal
        Dim drIndividualBallot : Dim fdSpkrs As DataRow()
        If chkThisRoundOnly.Checked = False Then ds.Tables("Ballot_Score").Clear()
        Dim ProcessPanel As Boolean
        For x = 0 To ds.Tables("panel").Rows.Count - 1
            'pull the round, read the tiebreakers
            Dim drRound As DataRow
            drRound = ds.Tables("Round").Rows.Find(ds.Tables("Panel").Rows(x).Item("Round"))
            ProcessPanel = True
            If chkThisRoundOnly.Checked = True Then
                If drRound.Item("ID") <> cboRound.SelectedValue Then ProcessPanel = False
            End If
            If ProcessPanel = True Then
                Call UniqueScores(ds, drRound.Item("TB_SET"), scores)
                ReDim arrJudges(drRound.Item("JudgesPerPanel"))
                Call UniqueItemsOnPanel(ds, ds.Tables("Panel").Rows(x).Item("ID"), "Judge", drRound.Item("JudgesPerPanel"), arrJudges)
                Label1.Text = x & "/" & ds.Tables("panel").Rows.Count - 1 : Label1.Refresh()
                'pull scores
                Dim nTeams As Integer = getEventSetting(ds, drRound.Item("Event"), "TeamsPerRound")
                Dim nSpkrs As Integer = getEventSetting(ds, drRound.Item("Event"), "DebatersPerTeam")
                ReDim arrTeams(nTeams)
                'If ds.Tables("Panel").Rows(x).Item("ID") = 11618 Then MsgBox("11618")
                Call UniqueItemsOnPanel(ds, ds.Tables("Panel").Rows(x).Item("ID"), "Entry", nTeams, arrTeams)
                'scroll through the number of judges
                For z = 1 To arrJudges.GetUpperBound(0)
                    'generate random scores for the judge
                    Call MakeRandomScores(spts, srank, win, tmrank, tmPts, drRound.Item("TB_SET"), drRound.Item("Event"), nTeams, nSpkrs)
                    'see if the scores are being recorded for this tbset
                    For w = 1 To 5
                        If scores(w) = 1 Then
                            For y = 1 To nTeams
                                drIndividualBallot = ds.Tables("Ballot").Select("Judge=" & arrJudges(z) & " and Entry=" & arrTeams(y) & " and panel=" & ds.Tables("Panel").Rows(x).Item("ID"))
                                dr = ds.Tables("Ballot_Score").NewRow
                                If Not dr.Item("Recipient") Is System.DBNull.Value Then
                                    dr.Item("Recipient") = drIndividualBallot(0).item("Entry")
                                    dr.Item("Ballot") = drIndividualBallot(0).item("ID")
                                    dr.Item("Score_ID") = scores(w)
                                    dr.Item("Score") = win(y - 1)
                                    ds.Tables("Ballot_Score").Rows.Add(dr)
                                End If
                            Next y
                        End If
                        If scores(w) = 4 Then
                            For y = 1 To nTeams
                                drIndividualBallot = ds.Tables("Ballot").Select("Judge=" & arrJudges(z) & " and Entry=" & arrTeams(y) & " and panel=" & ds.Tables("Panel").Rows(x).Item("ID"))
                                dr = ds.Tables("Ballot_Score").NewRow
                                dr.Item("Recipient") = drIndividualBallot(0).item("Entry")
                                dr.Item("Ballot") = drIndividualBallot(0).item("ID")
                                dr.Item("Score_ID") = scores(w)
                                dr.Item("Score") = tmPts(y - 1)
                                ds.Tables("Ballot_Score").Rows.Add(dr)
                            Next y
                        End If
                        If scores(w) = 5 Then
                            For y = 1 To nTeams
                                drIndividualBallot = ds.Tables("Ballot").Select("Judge=" & arrJudges(z) & " and Entry=" & arrTeams(y) & " and panel=" & ds.Tables("Panel").Rows(x).Item("ID"))
                                dr = ds.Tables("Ballot_Score").NewRow
                                dr.Item("Recipient") = drIndividualBallot(0).item("Entry")
                                dr.Item("Ballot") = drIndividualBallot(0).item("ID")
                                dr.Item("Score_ID") = scores(w)
                                dr.Item("Score") = tmrank(y - 1)
                                ds.Tables("Ballot_Score").Rows.Add(dr)
                            Next y
                        End If
                        If scores(w) = 2 Then
                            For y = 1 To nTeams
                                drIndividualBallot = ds.Tables("Ballot").Select("Judge=" & arrJudges(z) & " and Entry=" & arrTeams(y) & " and panel=" & ds.Tables("Panel").Rows(x).Item("ID"))
                                fdSpkrs = ds.Tables("Entry_Student").Select("Entry=" & arrTeams(y))
                                For q = 0 To fdSpkrs.Length - 1
                                    dr = ds.Tables("Ballot_Score").NewRow
                                    dr.Item("Recipient") = fdSpkrs(q).Item("ID")
                                    dr.Item("Ballot") = drIndividualBallot(0).item("ID")
                                    dr.Item("Score_ID") = scores(w)
                                    dr.Item("Score") = spts(y - 1, q)
                                    ds.Tables("Ballot_Score").Rows.Add(dr)
                                Next q
                            Next y
                        End If
                        If scores(w) = 3 Then
                            For y = 1 To nTeams
                                drIndividualBallot = ds.Tables("Ballot").Select("Judge=" & arrJudges(z) & " and Entry=" & arrTeams(y) & " and panel=" & ds.Tables("Panel").Rows(x).Item("ID"))
                                fdSpkrs = ds.Tables("Entry_Student").Select("Entry=" & arrTeams(y))
                                For q = 0 To fdSpkrs.Length - 1
                                    dr = ds.Tables("Ballot_Score").NewRow
                                    dr.Item("Recipient") = fdSpkrs(q).Item("ID")
                                    dr.Item("Ballot") = drIndividualBallot(0).item("ID")
                                    dr.Item("Score_ID") = scores(w)
                                    dr.Item("Score") = srank(y - 1, q)
                                    ds.Tables("Ballot_Score").Rows.Add(dr)
                                Next q
                            Next y
                        End If
                    Next w
                Next z
            End If
        Next x
        For x = 0 To ds.Tables("Ballot_Score").Rows.Count - 1
            If ds.Tables("Ballot_Score").Rows(x).Item("Score_ID") = 1 Then
                'MsgBox("stored a score of 1")
            End If
        Next
        Label1.Text = "Done" : Label1.Refresh()
    End Sub
    Sub MakeRandomScores(ByRef spts(,) As Decimal, ByRef srank(,) As Integer, ByRef win() As Integer, ByRef tmrank() As Integer, ByRef tmpts() As Decimal, ByVal tbset As Integer, ByVal EventID As Integer, ByVal nTeams As Integer, ByVal nSpkrs As Integer)
        'pull the event, get number of speakers and teams
        ReDim spts(nTeams, nSpkrs) : ReDim srank(nTeams, nSpkrs) : ReDim win(nTeams)
        ReDim tmrank(nTeams) : ReDim tmpts(nTeams)
        'wins
        If Rnd() < 0.5 Then
            win(0) = 1 : win(1) = 0
        Else
            win(0) = 0 : win(1) = 1
        End If

        'team points
        Dim dt2 As New DataTable
        dt2.Columns.Add()
        dt2.Columns.Add("Team", System.Type.GetType("System.Int16"))
        dt2.Columns.Add("Spkr", System.Type.GetType("System.Int16"))
        dt2.Columns.Add("Pts", System.Type.GetType("System.Single"))

        Dim drScoreSetting As DataRow()
        drScoreSetting = ds.Tables("Score_Setting").Select("Score=4 and TB_SET=" & tbset)
        Dim a(nTeams, 1)
        Dim dr As DataRow
        For x = 0 To nTeams - 1
            tmpts(x) = drScoreSetting(0).Item("Max") - CInt(Rnd() * (drScoreSetting(0).Item("Max") / 4))
            dr = dt2.NewRow : dr.Item("team") = x : dr.Item("Pts") = tmpts(x) : dt2.Rows.Add(dr)
        Next x

        'team ranks
        dt2.DefaultView.Sort = "Pts Desc"
        For x = 0 To dt2.DefaultView.Count - 1
            tmrank(dt2.DefaultView(x).Item("Team")) = x + 1
        Next

        'Speaker poitns
        drScoreSetting = ds.Tables("Score_Setting").Select("Score=2 and TB_SET=" & tbset)
        dt2.Clear() : dt2.AcceptChanges()
        For x = 0 To nTeams - 1
            For y = 0 To nSpkrs - 1
                spts(x, y) = drScoreSetting(0).Item("Max") - CInt(Rnd() * (drScoreSetting(0).Item("Max") / 4))
                If drScoreSetting(0).Item("DecimalIncrements") = 0.5 Then
                    If Rnd() < 0.5 Then spts(x, y) += 0.5
                End If
                If drScoreSetting(0).Item("DecimalIncrements") = 0.1 Then
                    spts(x, y) += FormatNumber(Rnd(), 1)
                End If
                If spts(x, y) > drScoreSetting(0).Item("Max") Then spts(x, y) = drScoreSetting(0).Item("Max")
                dr = dt2.NewRow : dr.Item("team") = x : dr.Item("Spkr") = y : dr.Item("Pts") = spts(x, y) : dt2.Rows.Add(dr)
            Next y
        Next x

        'speaker ranks
        dt2.DefaultView.Sort = "Pts Desc"
        For x = 0 To dt2.DefaultView.Count - 1
            srank(dt2.DefaultView(x).Item("Team"), dt2.DefaultView(x).Item("Spkr")) = x + 1
        Next
    End Sub

    Private Sub butElims_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butElims.Click
        'clear current elim seeds
        For x = 0 To ds.Tables("ElimSeed").Rows.Count - 1
            ds.Tables("ElimSeed").Rows(x).Item("Entry") = 0
        Next
        'clear current elim rounds
        Dim dr As DataRow
        For x = ds.Tables("Panel").Rows.Count - 1 To 0 Step -1
            dr = ds.Tables("Round").Rows.Find(ds.Tables("Panel").Rows(x).Item("Round"))
            If dr.Item("Rd_NAME") > 9 Then ds.Tables("Panel").Rows(x).Delete()
        Next x
        For x = 0 To ds.Tables("Event").Rows.Count - 1
            Label1.Text = "Filling in first elim seedings..." : Label1.Refresh()
            Call FillFirstElim(ds, ds.Tables("Event").Rows(x).Item("ID"), 1)
            Label1.Text = "Completing elim results..." : Label1.Refresh()
            If getEventSetting(ds, ds.Tables("Event").Rows(x).Item("ID"), "TeamsPerRound") = 2 Then
                Call SimulateElims(ds.Tables("Event").Rows(x).Item("ID"))
            Else
                Call SimulateElims4Teams(ds.Tables("Event").Rows(x).Item("ID"))
            End If
        Next x
        Label1.Text = "Done"
    End Sub
    Sub SimulateElims(ByVal EventID As Integer)
        'find first elim round
        Dim foundElims As DataRow()
        foundElims = ds.Tables("Round").Select("Event=" & EventID & " and Rd_Name>9", "RD_NAME asc")
        'scroll through elims and make results
        Dim judge As Integer = 0 : Dim PanelID, affwin, ballotID, VotesForHi, VotesForLo As Integer
        Dim foundSeeds, nextSeed, drNextRd As DataRow()
        Dim drPanel, drBallot, drBallot_score As DataRow
        For x = 0 To foundElims.Length - 1
            'load all seeds for the round
            foundSeeds = ds.Tables("ElimSeed").Select("Round=" & foundElims(x).Item("ID"))
            For z = 0 To (foundSeeds.Length / 2) - 1
                VotesForHi = 0 : VotesForLo = 0
                'create a panel for each debate that will happen
                drPanel = ds.Tables("Panel").NewRow
                drPanel.Item("Round") = foundElims(x).Item("ID")
                PanelID = drPanel.Item("ID")
                ds.Tables("Panel").Rows.Add(drPanel)
                'add judges to the panel based on the number of judges for the round
                For y = 0 To foundElims(x).Item("JudgesPerPanel") - 1
                    'add a ballot for each team, and a ballot score for each ballot
                    'here's the ballot for the higher seed, always set to aff
                    drBallot = ds.Tables("Ballot").NewRow
                    drBallot.Item("Panel") = PanelID
                    drBallot.Item("Entry") = foundSeeds(z).Item("Entry")
                    drBallot.Item("Side") = 1
                    drBallot.Item("Judge") = ds.Tables("Judge").Rows(judge).Item("ID")
                    ballotID = drBallot.Item("ID")
                    ds.Tables("Ballot").Rows.Add(drBallot)
                    'balllot score for higher seed ballot
                    affwin = CInt(Rnd())
                    drBallot_score = ds.Tables("Ballot_Score").NewRow
                    drBallot_score.Item("Ballot") = ballotID
                    drBallot_score.Item("Recipient") = drBallot.Item("Entry")
                    drBallot_score.Item("Score_ID") = 1
                    drBallot_score.Item("Score") = affwin
                    ds.Tables("Ballot_Score").Rows.Add(drBallot_score)
                    'here's the ballot for the lower seed, always set to neg
                    drBallot = ds.Tables("Ballot").NewRow
                    drBallot.Item("Panel") = PanelID
                    drBallot.Item("Entry") = foundSeeds(foundSeeds.Length - z - 1).Item("Entry")
                    drBallot.Item("Side") = 2
                    drBallot.Item("Judge") = ds.Tables("Judge").Rows(judge).Item("ID")
                    ballotID = drBallot.Item("ID")
                    ds.Tables("Ballot").Rows.Add(drBallot)
                    'balllot score for lower seed ballot
                    drBallot_score = ds.Tables("Ballot_Score").NewRow
                    drBallot_score.Item("Ballot") = ballotID
                    drBallot_score.Item("Recipient") = drBallot.Item("Entry")
                    drBallot_score.Item("Score_ID") = 1
                    If affwin = 1 Then drBallot_score.Item("Score") = 0 Else drBallot_score.Item("Score") = 1
                    ds.Tables("Ballot_Score").Rows.Add(drBallot_score)
                    'advance to the next judge
                    If affwin = 1 Then VotesForHi += 1 Else VotesForLo += 1
                    judge += 1
                Next y
                'advance the winning team if not finals
                If foundElims(x).Item("Rd_Name") + 1 <= 16 Then
                    'find next round and seedspot
                    drNextRd = ds.Tables("Round").Select("Rd_Name=" & foundElims(x).Item("Rd_Name") + 1 & " and event=" & EventID)
                    nextSeed = ds.Tables("ElimSeed").Select("Round=" & drNextRd(0).Item("ID") & " and seed=" & foundSeeds(z).Item("Seed"))
                    If VotesForHi > VotesForLo Then
                        nextSeed(0).Item("Entry") = foundSeeds(z).Item("Entry")
                    Else
                        nextSeed(0).Item("Entry") = foundSeeds(foundSeeds.Length - z - 1).Item("Entry")
                    End If
                End If
            Next z
        Next x
    End Sub
    Sub SimulateElims4Teams(ByVal EventID As Integer)
        'find first elim round
        Dim foundElims, fdballot As DataRow()
        foundElims = ds.Tables("Round").Select("Event=" & EventID & " and Rd_Name>9", "Event ASC, RD_NAME asc")
        Dim intRank, intSeed As Integer
        'scroll through elims and make results
        Dim judge As Integer = -1 : Dim PanelID As Integer
        Dim foundSeeds, nextSeed, drNextRd As DataRow()
        Dim dr As DataRow
        For x = 0 To foundElims.Length - 2
            judge = -1 : intSeed = 0
            'load all seeds for the round
            foundSeeds = ds.Tables("ElimSeed").Select("Round=" & foundElims(x).Item("ID"))
            '1 debate for every 4 teams; divide teams by half and move forward 2 at a time, pulling lower seeds
            For z = 0 To (foundSeeds.Length / 2) - 1 Step 2
                'create a panel for each debate that will happen
                PanelID = AddPanel(ds, foundElims(x).Item("ID"), 0)
                'add teams
                For y = z To z + 1
                    Call AddTeamToPanel(ds, PanelID, foundSeeds(y).Item("Entry"), 1)
                    Call AddTeamToPanel(ds, PanelID, foundSeeds(foundSeeds.Length - y - 1).Item("Entry"), 2)
                Next y
                'add judges to the panel based on the number of judges for the round
                For y = 0 To foundElims(x).Item("JudgesPerPanel") - 1
                    judge += 1
                    Call AddJudgeToPanel(ds, PanelID, ds.Tables("Judge").Rows(judge).Item("ID"), 1)
                Next y
                'add results
                'see if the scores are being recorded for this tbset
                fdballot = ds.Tables("Ballot").Select("Panel=" & PanelID, "Judge ASC")
                intRank = Int(Rnd() * 4) + 1
                For y = 0 To 11
                    If y = 4 Or y = 8 Then intRank = Int(Rnd() * 4) + 1
                    intRank += 1 : If intRank > 4 Then intRank = 1
                    dr = ds.Tables("Ballot_Score").NewRow
                    dr.Item("Recipient") = fdballot(y).Item("Entry")
                    dr.Item("Ballot") = fdballot(y).Item("ID")
                    dr.Item("Score_ID") = 5
                    dr.Item("Score") = intRank
                    ds.Tables("Ballot_Score").Rows.Add(dr)
                    'advance if ranked in top 2 if not finals
                    If intRank <= 2 And foundElims(x).Item("Rd_Name") + 1 <= 16 And y <= 3 Then
                        intSeed += 1
                        drNextRd = ds.Tables("Round").Select("Rd_Name=" & foundElims(x).Item("Rd_Name") + 1 & " and event=" & EventID)
                        nextSeed = ds.Tables("ElimSeed").Select("Round=" & drNextRd(0).Item("ID") & " and seed=" & intSeed)
                        nextSeed(0).Item("Entry") = fdballot(y).Item("Entry")
                    End If
                Next y
            Next z
        Next x
    End Sub
    Public Sub DataSetLinq101()
        'LINQ test

        'Pulls students from an entry

        Dim students = ds.Tables("Entry_Student").AsEnumerable()
        Dim subStudents = From Entry_student In students Where Entry_student!Entry = 1219 Select Entry_student

        For Each entry_student In subStudents
            MsgBox(entry_student!Last)
        Next

    End Sub
    Public Sub DataSetLinq102()
        'LINQ test
        Dim Panels = ds.Tables("Panel").AsEnumerable()
        Dim subPanel = From Panel In Panels Where Panel!Round = 123 Select Panel

    End Sub
    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        'LINQ test
        Call DataSetLinq101()
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Call InitializeTieBreakers(ds, True)
    End Sub
End Class
