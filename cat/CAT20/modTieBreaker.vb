Module modTieBreaker
    
    Private DS As New DataSet
    Private dtJVar As DataTable
    Dim foundrow As DataRow()


    Public Function MakeTBTable(ByVal DtS As DataSet, ByVal Round As Integer, ByVal SortType As String, ByVal strTeamDisplay As String, ByVal TBSET As Integer, ByVal PairForRd As Integer) As DataTable
        'takes the dataset, a round, a sort type (team or speaker), team display mode, TBSET, and timeslot rounds are through
        'I THINK rdtopair is the round being paired, so you can identify that it's an elim but seed through only the last prelim
        'if TBSET is -1, it will read the TBSET associated with the round, otherwise it will use the one received
        'need to test to make sure that the tiebreakers requested have been collected
        Dim str As String = "Start make TBTable: " & Now.Second.ToString & " " & Now.Millisecond.ToString & Chr(10)

        DS = DtS.Copy
        Dim x, y As Integer

        'Pull eventID and timeslot by round
        Dim drRound, drPairFor As DataRow
        drRound = DS.Tables("Round").Rows.Find(Round)
        drPairFor = DS.Tables("Round").Rows.Find(PairForRd)
        Dim EventID As Integer = drRound.Item("Event")
        Dim timeslot As Integer = drRound.Item("TimeSlot")
        If TBSET = -1 Then TBSET = drRound.Item("TB_SET")

        Dim DT As New DataTable : Dim dr As DataRow
        Dim strSortOrder As String = ""

        'first 3 columns are set
        DT.Columns.Add("COMPETITOR", System.Type.GetType("System.Int64"))
        DT.Columns.Add("COMPETITORNAME", System.Type.GetType("System.String"))
        DT.Columns.Add("SEED", System.Type.GetType("System.Int64"))

        'find the teams to sort
        Dim dtvEntry As New DataView(DS.Tables("ENTRY"))
        dtvEntry.RowFilter = "EVENT = " & EventID.ToString & " and Dropped=False"

        'if it's a preset, load ratings into seed and be done with it
        If drPairFor.Item("PairingScheme").toupper = "PRESET" And SortType = "TEAM" Then
            For x = 0 To dtvEntry.Count - 1
                dr = DT.NewRow
                dr.Item("Competitor") = dtvEntry(x).Item("ID")
                dr.Item("CompetitorName") = dtvEntry(x).Item(strTeamDisplay)
                dr.Item("Seed") = dtvEntry(x).Item("Rating")
                DT.Rows.Add(dr)
            Next x
            DT.Constraints.Add("PrimaryKey", DT.Columns("Competitor"), True)
            Return DT
            Exit Function
        End If

        'strip rounds later than the specified timeslot
        'this will only work if timeslots in order, which they won't necessarily be
        'For x = DS.Tables("Round").Rows.Count - 1 To 0 Step -1
        'If DS.Tables("Round").Rows(x).Item("Timeslot") > timeslot Then DS.Tables("Round").Rows(x).Delete()
        'Next x
        'this is a sub
        For x = DS.Tables("Round").Rows.Count - 1 To 0 Step -1
            If DS.Tables("Round").Rows(x).Item("rd_Name") > drRound.Item("Rd_Name") Then DS.Tables("Round").Rows(x).Delete()
        Next x
        DS.AcceptChanges()

        'Populate the Judge Variance table
        Call FillJudVar(DS)

        'Put average scores in for byes
        str &= "Start Avg: " & Now.Second.ToString & " " & Now.Millisecond.ToString & Chr(10)
        Call AvgForByes(DS)
        str &= "End Avg: " & Now.Second.ToString & " " & Now.Millisecond.ToString & Chr(10)

        'TEAM and SPEAKER are valid sort types
        SortType = SortType.ToUpper

        'retrieve TBset ID from the name
        'get the rows for that TBSET and sort
        Dim dtvTB As New DataView(DS.Tables("TIEBREAK"))
        dtvTB.RowFilter = "TB_SET = " & TBSET
        dtvTB.Sort = "SortOrder ASC"

        'create columns in order of tiebreaker set
        Dim drScore As DataRow : Dim strDummy As String
        For x = 0 To dtvTB.Count - 1
            If dtvTB(x).Item("TAG") <> "None" Then
                DT.Columns.Add(dtvTB(x).Item("TAG"), System.Type.GetType("System.Single"))
                strDummy = dtvTB(x).Item("TAG")
            Else
                DT.Columns.Add(dtvTB(x).Item("LABEL"), System.Type.GetType("System.Single"))
                strDummy = dtvTB(x).Item("LABEL")
            End If
            If strSortOrder <> "" Then strSortOrder &= ", "
            drScore = DS.Tables("Scores").Rows.Find(dtvTB(x).Item("SCOREID"))
            strSortOrder &= strDummy & " " & drScore.Item("SortOrder")
        Next

        'Fill with raw values, and build sortstring
        Dim fdEntry As DataRow()
        For x = 0 To dtvEntry.Count - 1
            If SortType = "TEAM" Then
                fdEntry = DS.Tables("Entry").Select("ID=" & dtvEntry(x).Item("ID"))
            Else
                fdEntry = DS.Tables("Entry_STUDENT").Select("Entry=" & dtvEntry(x).Item("ID"))
            End If
            For z = 0 To fdEntry.Length - 1
                dr = DT.NewRow
                dr.Item("Competitor") = fdEntry(z).Item("ID")
                dr.Item("Seed") = 0
                If strTeamDisplay.ToUpper = "FULL" Then strTeamDisplay = "FULLNAME"
                If SortType = "TEAM" Then dr.Item("CompetitorName") = dtvEntry(x).Item(strTeamDisplay)
                If SortType = "SPEAKER" Then dr.Item("CompetitorName") = GetName(DS.Tables("ENTRY_STUDENT"), fdEntry(z).Item("ID"), "First").Trim & " " & GetName(DS.Tables("ENTRY_STUDENT"), fdEntry(z).Item("ID"), "Last").Trim
                For y = 3 To 3 + dtvTB.Count - 1
                    dr.Item(y) = GetScore(SortType, fdEntry(z).Item("ID"), dtvTB(y - 3).Item("ID"))
                    If Int(dr.Item(y)) <> dr.Item(y) Then dr.Item(y) = FormatNumber(dr.Item(y), 2)
                Next y
                DT.Rows.Add(dr)
            Next z
        Next x

        'copy the datatable to a dataview and sort it
        Dim dvTable As New DataView(DT)
        dvTable.Sort = strSortOrder
        'clone the table and clear it, then add the rows in seed order
        Dim clonetable As New DataTable
        clonetable = DT.Clone : clonetable.Clear()
        For x = 0 To dvTable.Count - 1
            dr = clonetable.NewRow
            For y = 0 To DT.Columns.Count - 1
                dr.Item(y) = dvTable(x).Item(y)
            Next y
            dr.Item("Seed") = x + 1
            clonetable.Rows.Add(dr)
        Next x

        'set table to return
        MakeTBTable = clonetable
        MakeTBTable.Constraints.Add("PrimaryKey", MakeTBTable.Columns("Competitor"), True)
        str &= "End Sub: " & Now.Second.ToString & " " & Now.Millisecond.ToString & Chr(10)
        'MsgBox(str)

    End Function
    Sub AvgForByes(ByVal ds As DataSet)
        'pull all ballots; check that there's a ballot-score for every ballot
        For x = 0 To ds.Tables("Ballot").Rows.Count - 1
            Call ValidateScoresByBallot(ds, ds.Tables("Ballot").Rows(x).Item("ID"))
        Next x
        'now put average scores in every ballot_score where there's a -1
        Dim fdScores As DataRow() : Dim fdballot As DataRow
        For x = 0 To ds.Tables("Ballot_Score").Rows.Count - 1
            If ds.Tables("Ballot_Score").Rows(x).Item("Score") = -1 Then
                'set ballot judge and side to -1 to indicate byes for judge variance calculations
                fdballot = ds.Tables("Ballot").Rows.Find(ds.Tables("Ballot_score").Rows(x).Item("Ballot"))
                fdballot.Item("Judge") = -1 : fdballot.Item("Side") = -1
                'now pull all scores for competitor
                fdScores = ds.Tables("Ballot_Score").Select("Score_ID=" & ds.Tables("Ballot_Score").Rows(x).Item("Score_ID") & " and recipient=" & ds.Tables("Ballot_Score").Rows(x).Item("Recipient") & "and score>-1")
                ds.Tables("Ballot_Score").Rows(x).Item("Score") = 0
                For y = 0 To fdScores.Length - 1
                    ds.Tables("Ballot_Score").Rows(x).Item("Score") += fdScores(y).Item("Score")
                Next y
                If fdScores.Length > 0 Then
                    ds.Tables("Ballot_Score").Rows(x).Item("Score") = ds.Tables("Ballot_Score").Rows(x).Item("Score") / fdScores.Length
                Else
                    Try
                        'nothing to average, so give the tourney avg
                        ds.Tables("Ballot_Score").Rows(x).Item("Score") = ds.Tables("Ballot_Score").Compute("Avg(Score)", "Score_ID=" & ds.Tables("Ballot_Score").Rows(x).Item("Score_ID") & " and score>0")
                        ds.Tables("Ballot_Score").Rows(x).Item("Score") = FormatNumber(ds.Tables("Ballot_Score").Rows(x).Item("Score"), 1)
                    Catch
                        If ds.Tables("Ballot_Score").Rows(x).Item("Score_ID") = 2 Then ds.Tables("Ballot_Score").Rows(x).Item("Score") = 28
                        If ds.Tables("Ballot_Score").Rows(x).Item("Score_ID") = 3 Then ds.Tables("Ballot_Score").Rows(x).Item("Score") = 2.5
                    End Try
                End If
            End If
        Next
    End Sub
    Sub FillJudVar(ByRef ds As DataSet)

        'add columns to overall dataset; they shouldn't save
        ds.Tables("Judge").Columns.Add("zAvg", System.Type.GetType("System.Single"))
        ds.Tables("Judge").Columns.Add("zSD", System.Type.GetType("System.Single"))

        'create a new datatable to hold scores
        Dim dt As New DataTable
        dt.Columns.Add("Points", System.Type.GetType("System.Single"))

        Dim fdBallot, fdScore As DataRow()
        Dim dr As DataRow

        'populate the table and compute the values
        For x = 0 To ds.Tables("Judge").Rows.Count - 1
            dt.Clear()
            fdBallot = ds.Tables("Ballot").Select("Judge=" & ds.Tables("Judge").Rows(x).Item("ID"))
            For y = 0 To fdBallot.Length - 1
                fdScore = ds.Tables("Ballot_Score").Select("Ballot=" & fdBallot(y).Item("ID") & " and score>0")
                For z = 0 To fdScore.Length - 1
                    If fdScore(z).Item("Score_ID") = 2 Then
                        dr = dt.NewRow : dr.Item("Points") = fdScore(z).Item("Score") : dt.Rows.Add(dr)
                    End If
                Next z
            Next y
            If dt.Rows.Count > 0 Then
                ds.Tables("Judge").Rows(x).Item("zAvg") = dt.Compute("Avg(Points)", "")
                ds.Tables("Judge").Rows(x).Item("zSD") = dt.Compute("StDev(Points)", "")
            End If
        Next x
    End Sub
    Function GetScore(ByVal SortType As String, ByVal CompetitorID As Integer, ByVal TBID As Integer) As Single
        'receives the sortType as TEAM OR SPEAKER, a competitorID, and the ID value for the tiebreak row
        'first, pull the tiebreak row and use it's values to figure how to compute the tiebreaker
        Dim drTiebreak As DataRow
        drTiebreak = DS.Tables("Tiebreak").Rows.Find(TBID)
        If drTiebreak.Item("Tag") = "Random" Then GetScore = Int(Rnd() * 1000) : Exit Function
        Dim drScore As DataRow
        drScore = DS.Tables("Scores").Rows.Find(drTiebreak.Item("ScoreID"))
        If SortType = "TEAM" Then
            If drScore.Item("ScoreFor").toupper = "SPEAKER" And drTiebreak.Item("Tag") = "None" Then GetScore = SumSpkrByTeam(CompetitorID, drTiebreak.Item("ScoreID"), drTiebreak.Item("Drops"))
            If drScore.Item("ScoreFor").toupper = "TEAM" And drTiebreak.Item("Tag") = "None" Then GetScore = SumScore(CompetitorID, drTiebreak.Item("ScoreID"), drTiebreak.Item("Drops"))
            If drTiebreak.Item("Tag") = "Wins" Then GetScore = GetWins(CompetitorID)
            If drTiebreak.Item("Tag") = "OppWins" Then GetScore = GetOppWins(CompetitorID)
            If drTiebreak.Item("Tag") = "Ballots" Then GetScore = SumScore(CompetitorID, drTiebreak.Item("ScoreID"), drTiebreak.Item("Drops"))
            If drTiebreak.Item("Tag") = "JudgeVariance" Then GetScore = JudgeVariance(CompetitorID, SortType)
            If drTiebreak.Item("Tag") = "MBA" Then GetScore = MBA(CompetitorID)
            If InStr(drTiebreak.Item("Tag").ToString, "TotalRanks") > 0 Then GetScore = GetRanks(CompetitorID, drTiebreak.Item("Tag"))
        ElseIf SortType = "SPEAKER" Then
            If drTiebreak.Item("Tag") = "None" Then GetScore = SumScore(CompetitorID, drTiebreak.Item("ScoreID"), drTiebreak.Item("Drops"))
            If drTiebreak.Item("Tag") = "JudgeVariance" Then GetScore = JudgeVariance(CompetitorID, SortType)
            If drTiebreak.Item("Tag") = "OppWins" Then
                Dim dr As DataRow : dr = DS.Tables("Entry_Student").Rows.Find(CompetitorID)
                GetScore = GetOppWins(dr.Item("Entry"))
            End If
        End If
    End Function
    Function GetRanks(ByVal competitorID As Integer, ByVal strTB As String) As Integer
        GetRanks = 0
        Dim fdscores As DataRow()
        fdscores = DS.Tables("Ballot_Score").Select("Recipient=" & competitorID & " and score_ID=" & 5)
        Dim rank As Integer = Val(Mid(strTB, strTB.Length, 1))
        For x = 0 To fdscores.Length - 1
            If fdscores(x).Item("Score") = rank Then GetRanks += 1
        Next x
    End Function
    Function MBA(ByVal CompetitorID) As Single
        MBA = GetOppWins(CompetitorID)
        MBA += SumSpkrByTeam(CompetitorID, 2, 1)
    End Function
    Function JudgeVariance(ByVal CompetitorID As Integer, ByVal strType As String) As Single

        JudgeVariance = 0
        Dim fdCompetitors, fdScores As DataRow()
        
        'process all competitors if a team, but only inidividual student in a speaker
        If strType = "TEAM" Then
            fdCompetitors = DS.Tables("Entry_Student").Select("Entry=" & CompetitorID)
        Else
            fdCompetitors = DS.Tables("Entry_Student").Select("ID=" & CompetitorID)
        End If

        Dim TotUsed, ToAvg As Integer
        ToAvg = 0
        If fdCompetitors.Length > 0 Then
            'build the search string to pull ballot scores based on that
            Dim strSearch As String = "("
            For x = 0 To fdCompetitors.Length - 1
                If strSearch <> "(" Then strSearch &= " or "
                strSearch &= "Recipient=" & fdCompetitors(x).Item("ID")
            Next
            strSearch &= ") and Score_ID=2 and Score<>0"

            'scroll through the ballot scores and assign judge variance scores for them
            fdScores = DS.Tables("Ballot_Score").Select(strSearch)
            Dim drJudge, drBallot As DataRow
            For x = 0 To fdScores.Length - 1
                drBallot = DS.Tables("Ballot").Rows.Find(fdScores(x).Item("Ballot"))
                drJudge = DS.Tables("Judge").Rows.Find(drBallot.Item("Judge"))
                If Not drJudge Is Nothing And drBallot.Item("Judge") <> -1 And drBallot.Item("Side") <> -1 Then
                    If Not drJudge.Item("zAvg") Is System.DBNull.Value Then
                        'If CompetitorID = 128648 Then MsgBox("128648")
                        JudgeVariance += ((fdScores(x).Item("Score") - drJudge.Item("zAvg")) / drJudge.Item("zSD"))
                        TotUsed += 1
                    End If
                ElseIf drBallot.Item("Judge") = -1 Or drBallot.Item("Side") = -1 Then
                    ToAvg += 1
                End If
            Next x
        End If

        'now add the averaged rounds
        If ToAvg = 0 Then Exit Function
        Dim dummy As Single = JudgeVariance / TotUsed
        For x = 1 To ToAvg
            JudgeVariance += dummy
        Next x
    End Function
    Function SumSpkrByTeam(ByVal Entry, ByVal strField, ByVal nHiLo) As Single
        'receive teamID, raw score to calculate, pulls all speakers off the team and generates a team total
        SumSpkrByTeam = 0
        Dim fdSpkrs, fdBallots As DataRow()
        'Pull all speakers on the team
        fdSpkrs = DS.Tables("Entry_Student").Select("Entry=" & Entry)

        'Set up 1 datatable to store raw scores you can sort by judge
        Dim dt As New DataTable
        dt.Columns.Add("Panel", System.Type.GetType("System.Int64"))
        dt.Columns.Add("Judge", System.Type.GetType("System.Int64"))
        dt.Columns.Add("Score", System.Type.GetType("System.Single"))
        'Set up 2nd datatable to hold summed team scores by judge
        Dim dtScores As New DataTable
        dtScores.Columns.Add("Score", System.Type.GetType("System.Single"))

        Dim dr, drBallot As DataRow
        'pull all ballot by competitor
        For x = 0 To fdSpkrs.Length - 1
            fdBallots = DS.Tables("Ballot_Score").Select("Recipient=" & fdSpkrs(x).Item("ID") & " and Score_ID=" & strField)
            For y = 0 To fdBallots.Length - 1
                drBallot = DS.Tables("Ballot").Rows.Find(fdBallots(y).Item("Ballot"))
                dr = dt.NewRow
                dr.Item("Judge") = drBallot.Item("Judge")
                dr.Item("Panel") = drBallot.Item("Panel")
                dr.Item("Score") = fdBallots(y).Item("Score")
                dt.Rows.Add(dr)
            Next
        Next x
        dt.DefaultView.Sort = "Panel asc, judge asc"
        'now the datatable includes all ballot scores, sorted by panel and judge

        'Put the team total for each judge on a row of dtScores
        Dim tot As Single
        For x = 0 To dt.DefaultView.Count - 1
            'drv = dt.DefaultView.Item(x)
            tot += dt.DefaultView.Item(x).Item("Score")
            If x = dt.DefaultView.Count - 1 Then
                dr = dtScores.NewRow : dr.Item("Score") = tot : dtScores.Rows.Add(dr) : tot = 0
            ElseIf dt.DefaultView.Item(x).Item("Judge") <> dt.DefaultView.Item(x + 1).Item("Judge") Or dt.DefaultView.Item(x).Item("Panel") <> dt.DefaultView.Item(x + 1).Item("Panel") Then
                'ElseIf dt.DefaultView.Item(x).Item("Judge") <> dt.DefaultView.Item(x + 1).Item("Judge") Then
                dr = dtScores.NewRow : dr.Item("Score") = tot : dtScores.Rows.Add(dr) : tot = 0
            End If
        Next x

        'Drop highs and lows
        dtScores.DefaultView.Sort = "Score ASC"
        For x = 0 To dtScores.DefaultView.Count - 1
            If x >= nHiLo And x <= dtScores.DefaultView.Count - 1 - nHiLo Then SumSpkrByTeam += dtScores.DefaultView.Item(x).Item("Score")
        Next x

    End Function
    Function SumScore(ByVal CompetitorID, ByVal strField, ByVal nHiLo) As Single
        'receive competitorID, raw score to calculate, number of hi-lo drops
        'assigns one score per panel, and drops the highs and lows

        SumScore = 0

        'pull all ballot by competitor
        Dim fdBallots As DataRow()
        fdBallots = DS.Tables("Ballot_Score").Select("Recipient=" & CompetitorID & " and Score_ID=" & strField, "Score DESC")

        'drop highs and lows
        For x = 0 To fdBallots.Length - 1
            If x >= nHiLo And x <= (fdBallots.Length - 1 - nHiLo) Then
                SumScore += fdBallots(x).Item("Score")
            End If
        Next x

    End Function
    Function SumOppScore(ByVal CompetitorID, ByVal strField) As Single
        'receive competitorID as team ID, raw score to calculate, number of hi-lo drops
        'assigns one score per panel, and drops the highs and lows

        SumOppScore = 0

        'pull all ballot by competitor
        Dim fdBallots, fdOtherBallots, fdScores As DataRow()
        fdBallots = DS.Tables("Ballot").Select("Recipient=" & CompetitorID)

        For x = 0 To fdBallots.Length - 1
            'pull all other ballots from the panel
            fdOtherBallots = DS.Tables("Ballot").Select("Panel=" & fdBallots(x).Item("Panel") & " and Entry<>" & CompetitorID)
            For z = 0 To fdOtherBallots.Length - 1
                'pull scores from other ballots
                fdScores = DS.Tables("Ballot_Score").Select("Ballot=" & fdOtherBallots(z).Item("Ballot") & " and Score_ID=" & strField)
                For y = 0 To fdScores.Length - 1
                    SumOppScore += fdBallots(x).Item("Score")
                Next
            Next z
        Next x

    End Function
    Function GetTeamPoints(ByVal team As Integer) As Single
        GetTeamPoints = 0
        Dim strSearch As String = ""
        strSearch = GetSpeakersByTeam(DS, team, "Recipient")
        'find the record that stores the points
        foundrow = DS.Tables("Ballot_Score").Select("(" & strSearch & ") and Score_ID=" & 2)
        Dim x As Integer
        For x = 0 To foundrow.Length - 1
            GetTeamPoints += foundrow(x).Item("Score")
        Next
        x = 1
    End Function
    Function GetBallot_Score(ByVal ds As DataSet, ByVal Ballot As Integer, ByVal ScoreID As Integer)
        'returns a score for a specific tiebreaker on a specific ballot from the ballot_score table
        'tiebreaker is the MasterID number, so need to find which tiebreaker ID is the master TB number
        Dim foundscore As DataRow()
        foundscore = DS.Tables("Ballot_Score").Select("Ballot=" & Ballot & " and Score_ID=" & ScoreID)
        GetBallot_Score = -99
        If foundscore.Length > 0 Then GetBallot_Score = foundscore(0).Item("Score")
    End Function
    Function GetWins(ByVal compID As Integer) As Single
        'returns the win total for a given team
        Dim x, score As Integer
        GetWins = 0
        Dim y As Integer = 1
        'pull all ballots involving team
        foundrow = DS.Tables("Ballot").Select("Entry=" & compID, "Panel ASC")
        Dim voteFor, voteVs As Integer
        'loop through the ballots in panel order
        For x = 0 To foundrow.Length - 1
            'pull all the win field from each ballot and count votes
            score = GetBallot_Score(DS, foundrow(x).Item("ID"), 1)
            If score = 1 Or score = 2 Then voteFor += 1 Else voteVs += 1
            'check for last ballot on the panel; if it is, count the win and reset ballot counts
            If x = foundrow.Length - 1 Then y = 0
            If (foundrow(x).Item("Panel") <> foundrow(x + y).Item("panel")) Or y = 0 Then
                If voteFor > voteVs Then GetWins += 1
                voteFor = 0 : voteVs = 0
            End If
        Next x
    End Function
    Function GetOppWins(ByVal team As Integer) As Integer
        'If team = 341286 Then MsgBox("Mega mf")
        GetOppWins = 0
        'pull all ballots by team
        Dim fdBallots, fdPanel As DataRow()
        fdBallots = DS.Tables("Ballot").Select("Entry=" & team, "Panel ASC")
        'pull all the ballots for every panel the team is on
        Dim ProcessPanel As Boolean : Dim nPanels, nByes As Integer
        For x = 0 To fdBallots.Length - 1
            ProcessPanel = False
            If x = 0 Then
                ProcessPanel = True
            ElseIf fdBallots(x).Item("Panel") <> fdBallots(x - 1).Item("Panel") Then
                ProcessPanel = True
            End If
            If ProcessPanel = True Then
                nPanels += 1
                If fdBallots(x).Item("Judge") = -1 Or fdBallots(x).Item("Side") = -1 Then nByes += 1
                fdPanel = DS.Tables("Ballot").Select("Panel=" & fdBallots(x).Item("Panel"), "Entry ASC")
                For y = 0 To fdPanel.Length - 1
                    'if the ballot isn't for the team, pull the number of wins for that team
                    If fdPanel(y).Item("Entry") <> team Then
                        If y = fdPanel.Length - 1 Then
                            GetOppWins += GetWins(fdPanel(y).Item("Entry"))
                        ElseIf fdPanel(y).Item("Entry") <> fdPanel(y + 1).Item("Entry") Then
                            GetOppWins += GetWins(fdPanel(y).Item("Entry"))
                        End If
                    End If
                Next y
            End If
        Next x
        'now add in bye values; this isn't your average, but assumes a .500 opponent
        If nByes = 0 Then Exit Function
        For x = 1 To nByes
            GetOppWins += nPanels / 2
        Next x
    End Function
    Function GetWinsForPullUps(ByVal ds As DataSet, ByVal compID As Integer, ByVal round As Integer) As Single
        'same thing, but want to pull only the wins from the dataset and not make another entire table
        'ONLY processes rounds before the passed round
        Dim x, score As Integer
        GetWinsForPullUps = 0
        Dim y As Integer = 1
        Dim drRound, drpanel, drPanRd As DataRow
        drRound = ds.Tables("Round").Rows.Find(round)
        'pull all ballots involving team
        foundrow = ds.Tables("Ballot").Select("Entry=" & compID, "Panel ASC")
        Dim voteFor, voteVs As Integer
        'loop through the ballots in panel order
        For x = 0 To foundrow.Length - 1
            drpanel = ds.Tables("Panel").Rows.Find(foundrow(x).Item("Panel"))
            drPanRd = ds.Tables("Round").Rows.Find(drpanel.Item("Round"))
            If drPanRd.Item("timeslot") < drRound.Item("timeslot") Then
                'process if bye
                If foundrow(x).Item("Side") = -1 Or foundrow(x).Item("Judge") = -1 Then
                    GetWinsForPullUps += 1
                Else
                    'pull all the win field from each ballot and count votes
                    score = GetBallot_Score(ds, foundrow(x).Item("ID"), 1)
                    If score = 1 Or score = 2 Then voteFor += 1 Else voteVs += 1
                    'check for last ballot on the panel; if it is, count the win and reset ballot counts
                    If x = foundrow.Length - 1 Then y = 0
                    If (foundrow(x).Item("Panel") <> foundrow(x + y).Item("panel")) Or y = 0 Then
                        If voteFor > voteVs Then GetWinsForPullUps += 1
                        voteFor = 0 : voteVs = 0
                    End If
                End If
            End If
        Next x
    End Function
    Function HadBye(ByVal ds As DataSet, ByVal round As Integer, ByVal Team As Integer) As Integer
        HadBye = 0
        Dim fdBallot As DataRow()
        Dim drPanel, drPanelRd, drRound As DataRow
        drRound = DS.Tables("Round").Rows.Find(round)

        'Pull all ballots for team
        fdBallot = ds.Tables("Ballot").Select("Entry=" & Team)
        For x = 0 To fdBallot.Length - 1
            drPanel = ds.Tables("Panel").Rows.Find(fdBallot(x).Item("Panel"))
            drPanelRd = ds.Tables("Round").Rows.Find(drPanel.Item("Round"))
            If drPanelRd.Item("Timeslot") < drRound.Item("TimeSlot") Then
                If fdBallot(x).Item("Judge") = -1 Or fdBallot(x).Item("Side") = -1 Then HadBye += 1
            End If
        Next x

    End Function
    Public Sub MakeOppTable(ByVal ds As DataSet, ByRef Dt As DataTable, ByVal Round As Integer)
        'size the table
        Dim drRd As DataRow
        drRd = ds.Tables("Round").Rows.Find(Round)
        Dim rdToMake As DataRow()
        rdToMake = ds.Tables("Round").Select("Event=" & drRd.Item("Event") & " and timeslot<=" & drRd.Item("Timeslot"), "timeslot ASC")
        Dt.Columns.Add("Team", System.Type.GetType("System.String"))
        For x = 0 To rdToMake.Length - 1
            Dt.Columns.Add(rdToMake(x).Item("Label"), System.Type.GetType("System.Int64"))
        Next x
        Dt.Columns.Add("Total", System.Type.GetType("System.Int64"))

        'populate it
        Dim fdBallots, fdOpps As DataRow()
        Dim drFdRd, drFdPanel, drFdOpp, drTableRow As DataRow
        'scroll through all teams
        For x = 0 To ds.Tables("Entry").Rows.Count - 1
            'process if they're in the right division
            If ds.Tables("Entry").Rows(x).Item("Event") = drRd.Item("Event") Then
                'if so, creat a new row for the table and pull all their ballots
                drTableRow = Dt.NewRow : drTableRow.Item("Team") = ds.Tables("entry").Rows(x).Item("FullName")
                fdBallots = ds.Tables("Ballot").Select("Entry=" & ds.Tables("Entry").Rows(x).Item("ID"))
                For z = 0 To fdBallots.Length - 1
                    'pull their opponents from their panel and enter the opponent rating for the datagrid
                    drFdPanel = ds.Tables("Panel").Rows.Find(fdBallots(z).Item("Panel"))
                    drFdRd = ds.Tables("ROund").Rows.Find(drFdPanel.Item("Round"))
                    If drFdRd.Item("timeslot") <= drRd.Item("Timeslot") Then
                        fdOpps = ds.Tables("Ballot").Select("Panel=" & fdBallots(z).Item("Panel") & " and entry<>" & fdBallots(z).Item("Entry"))
                        For y = 0 To fdOpps.Length - 1
                            drFdOpp = ds.Tables("Entry").Rows.Find(fdOpps(y).Item("Entry"))
                            If Not drFdOpp Is Nothing Then
                                drTableRow.Item(drFdRd.Item("Label")) = drFdOpp.Item("Rating")
                            Else
                                drTableRow.Item(drFdRd.Item("Label")) = 99
                            End If
                        Next y
                    End If
                Next z
                Dt.Rows.Add(drTableRow)
            End If
        Next x
        Dt.AcceptChanges()

        'sum it
        For x = 0 To Dt.Rows.Count - 1
            Dt.Rows(x).Item(Dt.Columns.Count - 1) = 0
            For y = 1 To Dt.Columns.Count - 2
                If Dt.Rows(x).Item(y) Is System.DBNull.Value Then Dt.Rows(x).Item(y) = 0
                Dt.Rows(x).Item(Dt.Columns.Count - 1) += Dt.Rows(x).Item(y)
            Next y
        Next
    End Sub
End Module
