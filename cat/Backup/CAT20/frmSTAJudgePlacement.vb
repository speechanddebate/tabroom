Public Class frmSTAJudgePlacement
    Dim ds As New DataSet
    Dim dtRound As DataTable
    Dim dtTiebreakers As DataTable

    Dim EnableEvents As Boolean         'Prevents values in forms that are changed by algorithms from triggering form events
    Dim Multitesting As Boolean         'Prevents forms events from being triggered during multitesting
    Dim BreakPoint As Byte
    Dim ExtraJudges As Integer
    Dim JudgeAgain As Boolean
    Dim NumCohorts As Byte
    Dim NumPrelims As Integer
    Dim PairDivision As String
    Dim PanelSize As Byte
    Dim PairCohort As Byte

    Dim JudgeCounter As Integer
    Dim PanelCounter As Integer
    Dim PairingCounter As Integer

    Dim JudgeCount As Integer
    Dim PairingCount As Integer
    Dim JudgesObliged As Integer
    Dim JudgesAvailable As Integer
    Dim ExtraRounds As Integer

    Dim MutualPref(0, 0) As Single
    Dim BelowMaximum(0, 0) As Single
    Dim PanelImpact(0, 0) As Single

    Dim PrefDataPct(0, 0, 0) As Single
    Dim TeamIndexPairing(0) As Integer
    Dim TeamIndexSide(0) As Byte
    Dim JudgeIndex(0) As Integer
    Dim JudgeCode(0) As Integer
    Dim JudgeRandom(0) As Single
    Dim RoundsJudged(0) As Integer
    Dim RoundsAvail(0) As Integer
    Dim RoundsRemain(0) As Integer
    Dim JudgeAvail(0) As Boolean
    Dim PermJudgeAvail(0) As Boolean
    Dim JudgeOblige(0) As Boolean
    Dim JudgeRating(0) As Single
    Dim JudgeRatingCount(0) As Integer
    Dim PairingIndex(0, 0, 0) As Integer
    Dim PairingDivision(0) As Integer
    Dim PairingCohort(0) As Byte
    Dim PairingTeam(0, 0) As Integer
    Dim TeamSchool(0, 0) As Integer
    Dim TeamLosses(0, 0) As Integer
    Dim AssignedJudges(0, 0) As Integer
    Dim JudgeUsed(0) As Boolean

    Dim IndivPrefWeight As Single
    Dim PanelPrefWeight As Single
    Dim JudgePrefWeight As Single
    Dim IndivDiffWeight As Single
    Dim PanelDiffWeight As Single
    Dim BalancePreviousWeight As Single = 0

    Dim MaxIndivPref(3) As Single
    Dim MaxIndivDiff(3) As Single
    Dim MaxPanelDiff(3) As Single
    Dim PermMaxPanelDiff(3) As Single
    Dim PermMaxIndivDiff(3) As Single
    Dim PermMaxIndivPref(3) As Single

    Dim MaxRegionDiff(3) As Byte

    'Variables required for MPJ Diagnostics
    Dim PanelSumAff As Single
    Dim PanelSumNeg As Single
    Dim IndivAverage(3) As Single
    Dim PrefCount(3) As Integer
    Dim IndivDifference(3) As Single
    Dim PanelDifference(3) As Single
    Dim WorsethanTarget(3) As Integer
    Dim WorstIndividual(3) As Single
    Dim WorstPanel(3) As Single
    Dim AvgPrefBefore As Single
    Dim AvgPrefBeforeCount As Integer
    Dim AvgPrefAfter As Single
    Dim AvgPrefAfterCount As Integer
    Dim Timeslot As Integer


    Private Sub frmSTAJudgePlacement_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Call LoadFile(ds, "TourneyData")
        Call NullKiller(ds, "Judge")
        Call InitializeSTA(ds)

        EnableEvents = False
        'fill the divisions cbo --- doesn't actually do divisions???
        cboRound.DataSource = ds.Tables("Round")
        cboRound.DisplayMember = "Label"
        cboRound.ValueMember = "ID"
        cboRound.Focus()
        EnableEvents = True

        ConnectToXMLData()

    End Sub

    Private Sub frmSTAJudgePlacement_Unload(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Disposed
        Call SaveFile(ds)
        ds.Dispose()
    End Sub

    Private Sub ConnectToXMLData()

        'Prevents unwanted events from being triggered by changes in values in TextBox controls
        EnableEvents = False

        'Look up default Panel Size for the Round
        Dim dr As DataRow
        dr = ds.Tables("Round").Rows.Find(cboRound.SelectedValue)
        PanelSize = dr.Item("JudgesPerPanel")
        TextBoxPanelSize.Text = PanelSize

        'Clear MPJ diagnostics from Form
        LabIndivAvgAllNEW.Text = ""
        LabIndivAvgBreakNEW.Text = ""
        LabIndivAvgAboveNEW.Text = ""
        LabIndivAvgBelowNEW.Text = ""
        LabWorstBreakNEW.Text = ""
        LabWorstAboveNEW.Text = ""
        LabWorstBelowNEW.Text = ""
        LabWorstPanelBreakNEW.Text = ""
        LabWorstPanelAboveNEW.Text = ""
        LabWorstPanelBelowNEW.Text = ""
        labIndivDiffAvgAllNEW.Text = ""
        labIndivDiffAvgBreakNEW.Text = ""
        labIndivDiffAvgAboveNEW.Text = ""
        labIndivDiffAvgBelowNEW.Text = ""
        labPanelDiffAvgAllNEW.Text = ""
        labPanelDiffAvgBreakNEW.Text = ""
        labPanelDiffAvgAboveNEW.Text = ""
        labPanelDiffAvgBelowNEW.Text = ""
        LabJudgePrefBefore.Text = ""
        LabJudgePrefAfterNEW.Text = ""
        LabLostJudgesNEW.Text = ""

        'Load Judge assignment parameters,constraints, and weights

        dr = ds.Tables("JUDGE_ASSIGN_PARAM").Rows(0)
        JudgeAgain = dr.Item("JUDGE_AGAIN")
        CheckBoxJudgeAgain.Checked = JudgeAgain
        NumCohorts = dr.Item("NUM_COHORTS")
        TextBoxNumCohorts.Text = NumCohorts
        If Multitesting = False Then IndivPrefWeight = dr.Item("INDIV_PREF_WEIGHT")
        TextBoxIndivPrefWeight.Text = IndivPrefWeight
        If Multitesting = False Then PanelPrefWeight = dr.Item("PANEL_PREF_WEIGHT")
        TextBoxPanelPrefWeight.Text = PanelPrefWeight
        If Multitesting = False Then IndivDiffWeight = dr.Item("INDIV_DIFF_WEIGHT")
        TextBoxIndivDiffWeight.Text = IndivDiffWeight
        If Multitesting = False Then PanelDiffWeight = dr.Item("PANEL_DIFF_WEIGHT")
        TextBoxPanelDiffWeight.Text = PanelDiffWeight
        If Multitesting = False Then JudgePrefWeight = dr.Item("JUDGE_PREF_WEIGHT")
        TextBoxJudgePrefWeight.Text = JudgePrefWeight
        TextBoxTargetAbove.Text = dr.Item("PREF_TARGET_ABOVE")
        PermMaxIndivPref(2) = TextBoxTargetAbove.Text
        TextBoxTargetBREAK.Text = dr.Item("PREF_TARGET_BREAK")
        PermMaxIndivPref(1) = TextBoxTargetBREAK.Text
        TextBoxTargetBelow.Text = dr.Item("PREF_TARGET_BELOW")
        PermMaxIndivPref(3) = TextBoxTargetBelow.Text
        TextBoxMaxIndivDiffAbove.Text = dr.Item("MAX_INDIV_DIFF_ABOVE")
        PermMaxIndivDiff(2) = TextBoxMaxIndivDiffAbove.Text
        TextBoxMaxIndivDiffBREAK.Text = dr.Item("MAX_INDIV_DIFF_BREAK")
        PermMaxIndivDiff(1) = TextBoxMaxIndivDiffBREAK.Text
        TextBoxMaxIndivDiffBelow.Text = dr.Item("MAX_INDIV_DIFF_BELOW")
        PermMaxIndivDiff(3) = TextBoxMaxIndivDiffBelow.Text
        TextBoxMaxPanelDiffAbove.Text = dr.Item("MAX_PANEL_DIFF_ABOVE")
        PermMaxPanelDiff(2) = TextBoxMaxPanelDiffAbove.Text
        TextBoxMaxPanelDiffBREAK.Text = dr.Item("MAX_PANEL_DIFF_BREAK")
        PermMaxPanelDiff(1) = TextBoxMaxPanelDiffBREAK.Text
        TextBoxMaxPanelDiffBelow.Text = dr.Item("MAX_PANEL_DIFF_BELOW")
        PermMaxPanelDiff(3) = TextBoxMaxPanelDiffBelow.Text
        TextBoxMaxLostRounds.Text = dr.Item("MAX_LOST_ROUNDS")
        ExtraJudges = TextBoxMaxLostRounds.Text
        TextBoxBreakPoint.Text = dr.Item("BREAKPOINT")
        BreakPoint = TextBoxBreakPoint.Text
        MaxIndivPref(0) = -1

        'Identify number of prelim rounds for the division that has the most prelims in order to calculate remaining judge availability/obligation        
        NumPrelims = GetTagValue(ds.Tables("Event_Setting"), "nprelims") 'At present returns first value rather than max division

        'Identify Maximum TeamID to dimension indexes given high TeamID values
        Dim x As Integer
        Dim MaxTeam As Integer
        For x = 0 To ds.Tables("Entry").Rows.Count - 1
            If ds.Tables("Entry").Rows(x).Item("ID") > MaxTeam Then MaxTeam = ds.Tables("Entry").Rows(x).Item("ID")
        Next
        'TeamIndexPairing and TeamIndexSide will identify the current pairing and the current side for each team indexed by their Entry ID
        ReDim TeamIndexPairing(MaxTeam)
        ReDim TeamIndexSide(MaxTeam)

        'Identify maximum judge code and judge count to dimension indexes (saves memory and execution time for arrays)
        Dim MaxJudge As Integer         'Determines maximum index value for the JudgeIndex array
        For x = 0 To ds.Tables("Judge").Rows.Count - 1
            If ds.Tables("Judge").Rows(x).Item("ID") > MaxJudge Then MaxJudge = ds.Tables("Judge").Rows(x).Item("ID")
        Next
        JudgeCount = x
        ReDim JudgeIndex(MaxJudge)      'Returns the position in the Judge Datatable (0-x) given the Judge ID from the database
        ReDim JudgeCode(JudgeCount)     'Returns the Judge ID from the Judge Datatable given the position in the judge Datatable (0-x)  THIS should be MOOT now

        'All of the following use the JudgeCounter as the index - the position in the judge download (1-x)
        'To obtain one of these values by Judge code use JudgeIndex as an embedded index (e.g. JudgeRegion(JudgeIndex(code))
        ReDim RoundsJudged(JudgeCount)  'Returns number of rounds judged prior to the current pairing
        ReDim RoundsAvail(JudgeCount)   'Returns the sum of SchoolCommt, ForHire and WillDonate 
        ReDim RoundsRemain(JudgeCount)  'Total rounds remaining for judge taking into how many rounds are left in tournament, how many have been judged, how many are available/committed and whether judge is specifically available for each remaining round
        ReDim JudgeAvail(JudgeCount)    'Boolean value indicating whether judge is currently available to be assigned (considers assignments in current pairing)
        ReDim PermJudgeAvail(JudgeCount) 'Boolean value indicating whether judge was available prior to current pairings
        ReDim JudgeOblige(JudgeCount)
        ReDim JudgeRating(JudgeCount)
        ReDim JudgeRatingCount(JudgeCount)

        'Code can be modified to actual number of pairings once I include code to determine number of rounds in largest division
        ReDim PairingIndex(200, 1, 20)        'Returns the ID index from the MasterResults table associated with each pairing
        ReDim PairingCohort(200)
        ReDim PairingTeam(200, 1)
        ReDim TeamSchool(200, 1)
        ReDim TeamLosses(200, 1)

        ReDim MutualPref(200, JudgeCount)      'Baseline evaluation for a pairing/judge combination - includes prefs, constraints, met before, indiv. mutuality
        ReDim BelowMaximum(200, JudgeCount)    'Indicates the worst of the two pref values for the two teams to determine whether it exceeds maximum target
        ReDim PanelImpact(200, JudgeCount)     'Indicates the mutuality impact of a pairing/judge combination (aff pref - neg pref)
        ReDim AssignedJudges(200, 20)
        ReDim PrefDataPct(200, 1, JudgeCount)     'Stores ADJ prefs by pairing number, side and judge counter

        Dim JudgeID, TeamID As Integer
        dr = ds.Tables("Round").Rows.Find(cboRound.SelectedValue)
        Timeslot = dr.Item("TIMESLOT")
        Dim RoundName As Integer = dr.Item("RD_NAME")
        'Load Judges
        Dim JudgeRoundTotal As Integer
        For x = 0 To ds.Tables("JUDGE").Rows.Count - 1
            JudgeCode(x + 1) = ds.Tables("JUDGE").Rows(x).Item("ID")   'Cross references position in array 1-x with JudgeID
            JudgeIndex(ds.Tables("JUDGE").Rows(x).Item("ID")) = x + 1
            RoundsAvail(x + 1) = ds.Tables("JUDGE").Rows(x).Item("OBLIGATION") + ds.Tables("JUDGE").Rows(x).Item("HIRED")
            If ds.Tables("JUDGE").Rows(x).Item("TIMESLOT" & Timeslot) Is System.DBNull.Value Then ds.Tables("JUDGE").Rows(x).Item("TIMESLOT" & Timeslot) = False
            If ds.Tables("JUDGE").Rows(x).Item("STOPSCHEDULING").ToString.ToUpper = "TRUE" Or ds.Tables("JUDGE").Rows(x).Item("TIMESLOT" & Timeslot).ToString.ToUpper = "FALSE" Then
                PermJudgeAvail(x + 1) = False
            Else
                PermJudgeAvail(x + 1) = True
            End If
            'Count Number of Remaining Rounds
            For y = Timeslot To GetLastPrelimTimeSlot(ds, dr.Item("Event"))
                If ds.Tables("Judge").Columns.Contains("TIMESLOT" & y) = True Then
                    If ds.Tables("JUDGE").Rows(x).Item("TIMESLOT" & y) Is System.DBNull.Value Then ds.Tables("JUDGE").Rows(x).Item("TIMESLOT" & y) = False
                    If ds.Tables("JUDGE").Rows(x).Item("TIMESLOT" & y) = True Then RoundsRemain(x + 1) = RoundsRemain(x + 1) + 1
                End If
            Next
            JudgeRoundTotal = JudgeRoundTotal + RoundsAvail(x + 1)
        Next

        PairingCount = 0
        Dim Concurrent, RoundPanels, IndivJudges, DecisionRow As DataRow()
        Dim SchoolLookup As DataRow
        Dim AffID, NegID, Decision As Integer
        Dim VoteForAff, VoteAgainstAff, VoteForNeg, VoteAgainstNeg As Integer
        Concurrent = ds.Tables("Round").Select("TIMESLOT <= " & Timeslot, "TIMESLOT DESC")
        For x = 0 To Concurrent.Length - 1
            Dim RoundID As Integer = Concurrent(x).Item("ID")
            RoundPanels = ds.Tables("PANEL").Select("ROUND=" & RoundID)
            For y = 0 To RoundPanels.Length - 1
                If Concurrent(x).Item("ID") = cboRound.SelectedValue Then PairingCount = PairingCount + 1
                Dim PanelID As Integer = RoundPanels(y).Item("ID")
                IndivJudges = ds.Tables("BALLOT").Select("PANEL=" & PanelID, "JUDGE ASC")
                For z = 0 To IndivJudges.Length - 1
                    PanelCounter = Int(z / 2) + 1
                    TeamID = IndivJudges(z).Item("ENTRY")
                    'Error Trap for assigned BYES (-1)
                    If TeamID < 0 Then TeamID = 0
                    JudgeID = IndivJudges(z).Item("JUDGE")
                    'Error Trap for unassigned (-99) and bye (-1) judges
                    If JudgeID < 0 Then JudgeID = 0
                    If IndivJudges(z).Item("SIDE") = 1 Then AffID = TeamID Else NegID = TeamID
                    If Concurrent(x).Item("ID") = cboRound.SelectedValue Then
                        If IndivJudges(z).Item("SIDE") = 1 Then
                            PairingIndex(PairingCount, 0, PanelCounter) = IndivJudges(z).Item("ID")
                            PairingTeam(PairingCount, 0) = TeamID 'AFF TEAM
                            TeamIndexPairing(TeamID) = PairingCount
                            TeamIndexSide(TeamID) = 0
                            If TeamID > 0 Then
                                SchoolLookup = ds.Tables("ENTRY").Rows.Find(TeamID)
                                TeamSchool(PairingCount, 0) = SchoolLookup.Item("SCHOOL")
                            End If
                        Else
                            PairingIndex(PairingCount, 1, PanelCounter) = IndivJudges(z).Item("ID")
                            PairingTeam(PairingCount, 1) = TeamID 'NEG TEAM
                            TeamIndexPairing(TeamID) = PairingCount
                            TeamIndexSide(TeamID) = 1
                            If TeamID > 0 Then
                                SchoolLookup = ds.Tables("ENTRY").Rows.Find(TeamID)
                                TeamSchool(PairingCount, 0) = SchoolLookup.Item("SCHOOL")
                            End If
                        End If
                        AssignedJudges(PairingCount, PanelCounter) = JudgeID
                    ElseIf Concurrent(x).Item("TIMESLOT") < Timeslot Then
                        'Add to Loss Total if Team Lost selected pror round
                        Dim BallotID As Integer = IndivJudges(z).Item("ID")
                        DecisionRow = ds.Tables("BALLOT_SCORE").Select("BALLOT=" & BallotID & " AND SCORE_ID = 1")
                        If DecisionRow.Length > 0 Then
                            Decision = DecisionRow(0).Item("SCORE")
                            If IndivJudges(z).Item("SIDE") = 1 Then
                                If Decision = 1 Or Decision = 2 Then VoteForAff += 1 Else VoteAgainstAff += 1
                            Else
                                If Decision = 1 Or Decision = 2 Then VoteForNeg += 1 Else VoteAgainstNeg += 1
                            End If
                        End If
                        'Credit Judges for rounds judged prior to current rond
                        If IndivJudges(z).Item("SIDE") = 1 Then RoundsJudged(JudgeIndex(JudgeID)) += 1
                        'Code Judges against judging a team a second time if option selected (by increasing pref constraint to 1000
                        If JudgeAgain = True Then PrefDataPct(TeamIndexPairing(TeamID), TeamIndexSide(TeamID), JudgeIndex(JudgeID)) = 1000
                    Else
                        'Exclude Judges if already assigned to division in same time block as cbo.SelectedRound
                        PermJudgeAvail(JudgeIndex(JudgeID)) = False
                        If IndivJudges(z).Item("SIDE") = 1 Then RoundsJudged(JudgeIndex(JudgeID)) = RoundsJudged(JudgeIndex(JudgeID)) + 1
                    End If
                Next
                If VoteAgainstAff > VoteForAff Then TeamLosses(TeamIndexPairing(AffID), TeamIndexSide(AffID)) += 1
                If VoteAgainstNeg > VoteForNeg Then TeamLosses(TeamIndexPairing(NegID), TeamIndexSide(NegID)) += 1
                VoteForAff = 0
                VoteAgainstAff = 0
                VoteForNeg = 0
                VoteAgainstNeg = 0
            Next
        Next

        'Reseed random number generator for Random judge assignment
        Randomize()

        'Load Judge Preferences for Each Pairing/Side/Judge combination, Constrain if from the same school
        '''''''''ReDim PrefDataPct(PairingCount, 1, JudgeCount)     'Stores ADJ prefs by pairing number, side and judge counter
        Dim OrdPct As Decimal
        For x = 0 To ds.Tables("JUDGEPREF").Rows.Count - 1
            JudgeID = ds.Tables("JUDGEPREF").Rows(x).Item("JUDGE")
            TeamID = ds.Tables("JUDGEPREF").Rows(x).Item("TEAM")
            If ds.Tables("JUDGEPREF").Rows(x).Item("ORDPCT") Is System.DBNull.Value Then ds.Tables("JUDGEPREF").Rows(x).Item("ORDPCT") = 0
            OrdPct = ds.Tables("JUDGEPREF").Rows(x).Item("ORDPCT")
            If PrefDataPct(TeamIndexPairing(TeamID), TeamIndexSide(TeamID), JudgeIndex(JudgeID)) <> 1000 Then PrefDataPct(TeamIndexPairing(TeamID), TeamIndexSide(TeamID), JudgeIndex(JudgeID)) = OrdPct
            If OrdPct > 0 And OrdPct < 333 Then
                JudgeRating(JudgeIndex(JudgeID)) += OrdPct
                JudgeRatingCount(JudgeIndex(JudgeID)) += 1
            End If
        Next

        'Calculate judge availability and preference averages
        AvgPrefBefore = 0
        AvgPrefBeforeCount = 0
        JudgesObliged = 0
        JudgesAvailable = 0
        ExtraRounds = 0
        For x = 1 To JudgeCount
            If JudgeRatingCount(x) > 0 Then JudgeRating(x) = JudgeRating(x) / JudgeRatingCount(x)
            'Determine availability for prelims OR set availability for elims
            If RoundName < 10 Then
                RoundsAvail(x) = Math.Min(RoundsAvail(x) - RoundsJudged(x), RoundsRemain(x))
                If RoundsAvail(x) < 0 Then RoundsAvail(x) = 0
                If RoundsAvail(x) = 0 Then PermJudgeAvail(x) = False
                If RoundsRemain(x) <= RoundsAvail(x) And PermJudgeAvail(x) = True Then
                    JudgeOblige(x) = True
                    JudgesObliged = JudgesObliged + 1
                End If
            ElseIf PermJudgeAvail(x) = True Then
                RoundsAvail(x) = 1
                JudgeOblige(x) = True
                JudgesObliged = JudgesObliged + 1
            End If
            If PermJudgeAvail(x) = True Then JudgesAvailable = JudgesAvailable + 1
            AvgPrefBefore = AvgPrefBefore + JudgeRating(x) * RoundsAvail(x)
            AvgPrefBeforeCount = AvgPrefBeforeCount + RoundsAvail(x)
            ExtraRounds = ExtraRounds + RoundsAvail(x)
        Next

        AvgPrefAfter = AvgPrefBefore
        AvgPrefAfterCount = AvgPrefBeforeCount
        ReDim JudgeUsed(JudgeCount)
        For PairingCounter = 1 To PairingCount
            For PanelCounter = 1 To PanelSize
                If JudgeIndex(AssignedJudges(PairingCounter, PanelCounter)) > 0 Then
                    JudgeUsed(JudgeIndex(AssignedJudges(PairingCounter, PanelCounter))) = True
                    AvgPrefAfter = AvgPrefAfter - JudgeRating(JudgeIndex(AssignedJudges(PairingCounter, PanelCounter)))
                    AvgPrefAfterCount = AvgPrefAfterCount - 1
                End If
            Next
        Next
        Dim LostJudges As Integer           'Counts number of obliged judges who were not paired (thereby a lost round of commitment)
        For JudgeCounter = 1 To JudgeCount
            If JudgeOblige(JudgeCounter) = True And JudgeUsed(JudgeCounter) = False And RoundName < 10 Then
                LostJudges = LostJudges + 1
                AvgPrefAfter = AvgPrefAfter - JudgeRating(JudgeCounter)
                AvgPrefAfterCount = AvgPrefAfterCount - 1
            End If
        Next
        If AvgPrefBeforeCount > 0 Then AvgPrefBefore = AvgPrefBefore / AvgPrefBeforeCount
        If AvgPrefAfterCount < AvgPrefBeforeCount Then AvgPrefAfter = AvgPrefAfter / AvgPrefAfterCount Else AvgPrefAfter = AvgPrefBefore

        'Assign debates to pairing cohorts based on NumCohorts and BreakPoint
        For PairingCounter = 1 To PairingCount
            'Exempt BYES or Teams from Same School from judge assignment by assigning to Cohort 0
            'Assign remaining pairings to cohorts (all elim round pairings in cohort 2)
            If TeamSchool(PairingCounter, 0) = TeamSchool(PairingCounter, 1) Or PairingTeam(PairingCounter, 1) = 0 Or PairingTeam(PairingCounter, 0) = 0 Then
                PairingCohort(PairingCounter) = 0
            ElseIf RoundName > 9 Or NumCohorts = 1 Then
                PairingCohort(PairingCounter) = 2
            ElseIf TeamLosses(PairingCounter, 0) > BreakPoint And TeamLosses(PairingCounter, 1) > BreakPoint Then
                PairingCohort(PairingCounter) = 3
            ElseIf NumCohorts = 3 And (TeamLosses(PairingCounter, 0) = BreakPoint Or TeamLosses(PairingCounter, 1) = BreakPoint) Then
                PairingCohort(PairingCounter) = 1
            Else
                PairingCohort(PairingCounter) = 2
            End If
        Next

        'Variables required for MPJ Diagnostics
        PanelSumAff = 0
        PanelSumNeg = 0
        ReDim IndivAverage(3)
        ReDim PrefCount(3)
        ReDim IndivDifference(3)
        ReDim PanelDifference(3)
        ReDim WorsethanTarget(3)
        ReDim WorstIndividual(3)
        ReDim WorstPanel(3)

        'Display MPJ diagnostics for debate if judges have already been assigned
        For PairingCounter = 1 To PairingCount
            For PanelCounter = 1 To PanelSize
                If AssignedJudges(PairingCounter, PanelCounter) > 0 Then
                    PanelSumAff = PanelSumAff + PrefDataPct(PairingCounter, 0, JudgeIndex(AssignedJudges(PairingCounter, PanelCounter)))
                    PanelSumNeg = PanelSumNeg + PrefDataPct(PairingCounter, 1, JudgeIndex(AssignedJudges(PairingCounter, PanelCounter)))
                    IndivAverage(PairingCohort(PairingCounter)) = IndivAverage(PairingCohort(PairingCounter)) + PrefDataPct(PairingCounter, 0, JudgeIndex(AssignedJudges(PairingCounter, PanelCounter))) + PrefDataPct(PairingCounter, 1, JudgeIndex(AssignedJudges(PairingCounter, PanelCounter)))
                    'Determine whether aff or neg pref for judge is WorstIndividual for the Cohort
                    If PrefDataPct(PairingCounter, 0, JudgeIndex(AssignedJudges(PairingCounter, PanelCounter))) > WorstIndividual(PairingCohort(PairingCounter)) Then WorstIndividual(PairingCohort(PairingCounter)) = PrefDataPct(PairingCounter, 0, JudgeIndex(AssignedJudges(PairingCounter, PanelCounter)))
                    If PrefDataPct(PairingCounter, 1, JudgeIndex(AssignedJudges(PairingCounter, PanelCounter))) > WorstIndividual(PairingCohort(PairingCounter)) Then WorstIndividual(PairingCohort(PairingCounter)) = PrefDataPct(PairingCounter, 1, JudgeIndex(AssignedJudges(PairingCounter, PanelCounter)))
                    IndivDifference(PairingCohort(PairingCounter)) = IndivDifference(PairingCohort(PairingCounter)) + Math.Abs(PrefDataPct(PairingCounter, 0, JudgeIndex(AssignedJudges(PairingCounter, PanelCounter))) - PrefDataPct(PairingCounter, 1, JudgeIndex(AssignedJudges(PairingCounter, PanelCounter))))
                    PrefCount(PairingCohort(PairingCounter)) = PrefCount(PairingCohort(PairingCounter)) + 2
                End If
            Next
            If PanelSumAff + PanelSumNeg > WorstPanel(PairingCohort(PairingCounter)) Then WorstPanel(PairingCohort(PairingCounter)) = PanelSumAff + PanelSumNeg
            PanelDifference(PairingCohort(PairingCounter)) = PanelDifference(PairingCohort(PairingCounter)) + Math.Abs(PanelSumAff - PanelSumNeg)
            PanelSumAff = 0
            PanelSumNeg = 0
        Next

        For PairCohort = 1 To 3
            If PrefCount(PairCohort) > 0 Then
                'Aggregate Cohorts into Overall Results
                IndivAverage(0) = IndivAverage(0) + IndivAverage(PairCohort)
                IndivDifference(0) = IndivDifference(0) + IndivDifference(PairCohort)
                PanelDifference(0) = PanelDifference(0) + PanelDifference(PairCohort)
                PrefCount(0) = PrefCount(0) + PrefCount(PairCohort)
                'Calculate Averages for Individual Cohorts
                IndivAverage(PairCohort) = IndivAverage(PairCohort) / PrefCount(PairCohort)
                IndivDifference(PairCohort) = IndivDifference(PairCohort) / PrefCount(PairCohort) * 2
                PanelDifference(PairCohort) = PanelDifference(PairCohort) / PrefCount(PairCohort) * 2 * PanelSize
            End If
        Next
        'Calculate Averages for Overall Results
        If PrefCount(0) > 0 Then
            IndivAverage(0) = IndivAverage(0) / PrefCount(0)
            IndivDifference(0) = IndivDifference(0) / PrefCount(0) * 2
            PanelDifference(0) = PanelDifference(0) / PrefCount(0) * 2 * PanelSize
        End If

        'Display MPJ diagnostics on Form
        labIndivAvgALL.Text = Format(IndivAverage(0), "##0.00")
        labIndivAvgBreak.Text = Format(IndivAverage(1), "##0.00")
        labIndivAvgAbove.Text = Format(IndivAverage(2), "##0.00")
        labIndivAvgBelow.Text = Format(IndivAverage(3), "##0.00")
        LabWorstBREAK.Text = Format(WorstIndividual(1), "##0.00")
        LabWorstAbove.Text = Format(WorstIndividual(2), "##0.00")
        LabWorstBelow.Text = Format(WorstIndividual(3), "##0.00")
        LabWorstPanelBREAK.Text = Format(WorstPanel(1) / (2 * PanelSize), "##0.00")
        LabWorstPanelAbove.Text = Format(WorstPanel(2) / (2 * PanelSize), "##0.00")
        LabWorstPanelBelow.Text = Format(WorstPanel(3) / (2 * PanelSize), "##0.00")
        LabIndivDiffAvgAll.Text = Format(IndivDifference(0), "##0.00")
        LabIndivDiffAvgBreak.Text = Format(IndivDifference(1), "##0.00")
        LabIndivDiffAvgAbove.Text = Format(IndivDifference(2), "##0.00")
        LabIndivDiffAvgBelow.Text = Format(IndivDifference(3), "##0.00")
        LabPanelDiffAvgAll.Text = Format(PanelDifference(0), "##0.00")
        LabPanelDiffAvgBreak.Text = Format(PanelDifference(1), "##0.00")
        LabPanelDiffAvgAbove.Text = Format(PanelDifference(2), "##0.00")
        LabPanelDiffAvgBelow.Text = Format(PanelDifference(3), "##0.00")
        LabJudgePrefBefore.Text = Format(AvgPrefBefore, "##0.00")
        LabelJudgesAvailable.Text = JudgesAvailable
        LabelJudgesObliged.Text = JudgesObliged
        If PrefCount(0) > 0 Then LabJudgePrefAfter.Text = Format(AvgPrefAfter, "##0.00") Else LabJudgePrefAfter.Text = ""
        If PrefCount(0) > 0 Then LabLostJudges.Text = LostJudges Else LabLostJudges.Text = ""

        EnableEvents = True

    End Sub

    Private Sub AssignNewJudgePanels()

        'ERROR TRAP
        If PairingCount = 0 Then
            MsgBox("No Pairings in the Round / Division selected - Please Try Again")
            Exit Sub
        End If

        'Start clock to measure elapsed time
        Dim Timer As New Stopwatch
        Timer.Reset()
        Timer.Start()

        'Make sure we have ineligible division constraint encoded BRUSCHKE to do on MasterPrefsXXXXXXX

        Dim Assignments As Integer
        Dim TestCounter As Integer

        'Reset the AssginedJudges array back to 0's - required for Multi-testing shortcut
        ReDim AssignedJudges(PairingCount, PanelSize)
        ReDim MutualPref(PairingCount, JudgeCount)
        ReDim BelowMaximum(PairingCount, JudgeCount)
        ReDim PanelImpact(PairingCount, JudgeCount)
        'Returns Max values back to their permanent values - potentially changed in MultiTest if thresholds can't be met during a test
        For TestCounter = 1 To 3
            MaxIndivPref(TestCounter) = PermMaxIndivPref(TestCounter)
            MaxIndivDiff(TestCounter) = PermMaxIndivDiff(TestCounter)
            MaxPanelDiff(TestCounter) = PermMaxPanelDiff(TestCounter)
        Next

        PanelPrefWeight = 1
        PanelDiffWeight = 1

        Dim PanelVariance(PairingCount) As Single
        Dim PanelTotal(PairingCount) As Single

        'Determine Size of "Expanded" Judging Pool (number of non-obliged partial commitment judges minimally required to pair round)
        Dim Expansion As Integer
        Dim LostJudges As Integer
        Expansion = (PairingCount * PanelSize) - JudgesObliged

        Dim Mutuality As Single
        For PairingCounter = 1 To PairingCount

            For JudgeCounter = 1 To JudgeCount
                JudgeAvail(JudgeCounter) = PermJudgeAvail(JudgeCounter)
                'Calculate Parameters for Each Pairing
                If PrefDataPct(PairingCounter, 0, JudgeCounter) >= 999 Or PrefDataPct(PairingCounter, 1, JudgeCounter) >= 999 Then
                    MutualPref(PairingCounter, JudgeCounter) = 100000.0
                    BelowMaximum(PairingCounter, JudgeCounter) = 1000
                    PanelImpact(PairingCounter, JudgeCounter) = 1000
                Else
                    BelowMaximum(PairingCounter, JudgeCounter) = Math.Max(PrefDataPct(PairingCounter, 0, JudgeCounter), PrefDataPct(PairingCounter, 1, JudgeCounter))
                    If PrefDataPct(PairingCounter, 0, JudgeCounter) > 0 And PrefDataPct(PairingCounter, 1, JudgeCounter) > 0 Then
                        Mutuality = PrefDataPct(PairingCounter, 0, JudgeCounter) - PrefDataPct(PairingCounter, 1, JudgeCounter)
                    Else
                        Mutuality = 0
                    End If
                    PanelImpact(PairingCounter, JudgeCounter) = Mutuality
                    If Math.Abs(Mutuality) > MaxIndivDiff(PairingCohort(PairingCounter)) Then
                        MutualPref(PairingCounter, JudgeCounter) = 100000.0
                    Else
                        MutualPref(PairingCounter, JudgeCounter) = PrefDataPct(PairingCounter, 0, JudgeCounter) * (IndivPrefWeight + 0.5) _
                                                                 + PrefDataPct(PairingCounter, 1, JudgeCounter) * (IndivPrefWeight + 0.5) _
                                                                 + Math.Abs(Mutuality) * IndivDiffWeight
                    End If
                End If
            Next
        Next

        Dim CohortSize As Integer
        Dim Eval As Single
        Dim TempRow As Integer
        Dim Test1 As Integer
        Dim PairTestNum(0) As Integer
        Dim PairTestJdg(0) As Integer
        Dim PairTestEval(0) As Object

        For PairCohort = 1 To 3

            'If PairCohort = 3 Then IndivPrefWeight = IndivPrefWeight * 2
            If NumCohorts = 1 And PairCohort > 2 Then Exit For
            If NumCohorts < 3 And PairCohort = 1 Then PairCohort = 2

            Test1 = 0
            Dim JudgeTestCounter As Integer

            For PanelCounter = 1 To PanelSize

                ReDim PairTestNum(0)
                ReDim PairTestJdg(0)
                ReDim PairTestEval(0)
                CohortSize = 0
                'Create array for weighted preference totals for each pairing/judge combination
                TempRow = 0
                For PairingCounter = 1 To PairingCount
                    If PairingCohort(PairingCounter) = PairCohort Then
                        CohortSize = CohortSize + 1
                        JudgeTestCounter = 0
                        For JudgeCounter = 1 To JudgeCount

                            If JudgeAvail(JudgeCounter) = True _
                              And (JudgeOblige(JudgeCounter) = True Or Expansion > 0) Then

                                'Assign baseline weighted value for mutuality and preference
                                Eval = MutualPref(PairingCounter, JudgeCounter)

                                'Assign extreme penalty if Panel Difference exceeds identified maximum - otherwise assign weighted penalty

                                Mutuality = Math.Abs(PanelVariance(PairingCounter) + PanelImpact(PairingCounter, JudgeCounter))
                                If Mutuality > MaxPanelDiff(PairingCohort(PairingCounter)) Then
                                    Eval = Eval + 10000.0
                                Else
                                    Eval = Eval + Mutuality * PanelDiffWeight
                                End If

                                'Panel Total Balance
                                Eval = Eval - (PanelTotal(PairingCounter) * PanelPrefWeight)

                                'Judge Rating - Higher Judge Rating DECREASES Score
                                Eval = Eval - (JudgeRating(JudgeCounter) * JudgePrefWeight)

                                ReDim Preserve PairTestNum(TempRow)
                                ReDim Preserve PairTestJdg(TempRow)
                                ReDim Preserve PairTestEval(TempRow)
                                PairTestNum(TempRow) = PairingCounter
                                PairTestJdg(TempRow) = JudgeCounter
                                PairTestEval(TempRow) = Eval
                                TempRow = TempRow + 1
                            End If
                        Next
                    End If
                Next
                TempRow = TempRow - 1

                Dim IndexArray(TempRow) As Long
                For TestCounter = 0 To TempRow
                    IndexArray(TestCounter) = TestCounter
                Next

                If TempRow > 0 Then QuickSort1(PairTestEval, IndexArray)

                'Insert first pass assignments based on weighted sums of parameters
                Assignments = 0
                For TestCounter = 0 To TempRow
                    PairingCounter = PairTestNum(IndexArray(TestCounter))
                    JudgeCounter = PairTestJdg(IndexArray(TestCounter))
                    If JudgeAvail(JudgeCounter) = True _
                      And AssignedJudges(PairingCounter, PanelCounter) < 1 _
                      And (JudgeOblige(JudgeCounter) = True Or Expansion > 0) Then
                        PanelTotal(PairingCounter) = PanelTotal(PairingCounter) _
                          + PrefDataPct(PairingCounter, 0, JudgeCounter) _
                          + PrefDataPct(PairingCounter, 1, JudgeCounter)
                        PanelVariance(PairingCounter) = PanelVariance(PairingCounter) + PanelImpact(PairingCounter, JudgeCounter)
                        AssignedJudges(PairingCounter, PanelCounter) = JudgeCode(JudgeCounter)
                        JudgeAvail(JudgeCounter) = False
                        If JudgeOblige(JudgeCounter) = False Then Expansion = Expansion - 1
                        Assignments = Assignments + 1
                        If Assignments = CohortSize Then Exit For
                    End If
                Next
            Next
        Next

        'Declare variables required for backtracking algorithms
        Dim Evalx As Single
        Dim NumTests As Integer
        Dim Changes As Integer
        Dim PairingCounter2 As Integer
        Dim PairingCounter3 As Integer
        Dim BestPairingCounter As Integer
        Dim PanelCounter2 As Integer
        Dim PanelCounter3 As Integer
        Dim BestPanelCounter As Integer
        Dim JudgeCounter2 As Integer
        Dim JudgeCounter3 As Integer
        Dim BestJudgeCounter As Integer
        Dim BestJudgeSwapCounter As Integer
        Dim TempVariance1 As Single
        Dim TempVariance2 As Single
        Dim TempVariance3 As Single

Restart1:
        'Backtracking Algorithm #1 - Two-way swaps
        Changes = 0
        For PairingCounter = 1 To PairingCount
            For PanelCounter = 1 To PanelSize
                JudgeCounter = JudgeIndex(AssignedJudges(PairingCounter, PanelCounter))
                For PairingCounter2 = PairingCounter + 1 To PairingCount
                    For PanelCounter2 = 1 To PanelSize
                        JudgeCounter2 = JudgeIndex(AssignedJudges(PairingCounter2, PanelCounter2))
                        'Count Number of Tests
                        NumTests = NumTests + 1

                        'Evaluation of Cross-Cohort Permutations (automatically evaluate if in same cohort but only if changes meet threshold for both cohorts in cross-cohort permutation)
                        If PairingCohort(PairingCounter) = PairingCohort(PairingCounter2) _
                         Or (BelowMaximum(PairingCounter, JudgeCounter2) <= MaxIndivPref(PairingCohort(PairingCounter)) _
                         And BelowMaximum(PairingCounter2, JudgeCounter) <= MaxIndivPref(PairingCohort(PairingCounter2))) Then

                            'Evaluation of Impact of Panel Variances for each pairing
                            TempVariance1 = PanelVariance(PairingCounter) _
                                          - PanelImpact(PairingCounter, JudgeCounter) _
                                          + PanelImpact(PairingCounter, JudgeCounter2)
                            TempVariance2 = PanelVariance(PairingCounter2) _
                                          - PanelImpact(PairingCounter2, JudgeCounter2) _
                                          + PanelImpact(PairingCounter2, JudgeCounter)

                            'CONSTRAINT - Max Panel Variance and Max Individual Variance
                            If Math.Abs(TempVariance1) <= MaxPanelDiff(PairingCohort(PairingCounter)) _
                              And Math.Abs(TempVariance2) <= MaxPanelDiff(PairingCohort(PairingCounter2)) _
                              And Math.Abs(PanelImpact(PairingCounter, JudgeCounter2)) <= MaxIndivDiff(PairingCohort(PairingCounter)) _
                              And Math.Abs(PanelImpact(PairingCounter2, JudgeCounter)) <= MaxIndivDiff(PairingCohort(PairingCounter2)) Then

                                'Evaluation of Current
                                Eval = MutualPref(PairingCounter, JudgeCounter) _
                                     + MutualPref(PairingCounter2, JudgeCounter2) _
                                     + Math.Abs(PanelVariance(PairingCounter)) * PanelDiffWeight _
                                     + Math.Abs(PanelVariance(PairingCounter2)) * PanelDiffWeight
                                'Evaluation of Permutation
                                Evalx = MutualPref(PairingCounter, JudgeCounter2) _
                                      + MutualPref(PairingCounter2, JudgeCounter) _
                                      + Math.Abs(TempVariance1) * PanelDiffWeight _
                                      + Math.Abs(TempVariance2) * PanelDiffWeight
                                If Evalx < Eval Then
                                    Changes = Changes + 1
                                    PanelVariance(PairingCounter) = TempVariance1
                                    PanelVariance(PairingCounter2) = TempVariance2
                                    AssignedJudges(PairingCounter, PanelCounter) = JudgeCode(JudgeCounter2)
                                    AssignedJudges(PairingCounter2, PanelCounter2) = JudgeCode(JudgeCounter)
                                    JudgeCounter = JudgeCounter2
                                End If
                            End If
                        End If
                    Next
                Next
            Next
        Next
        If Changes > 0 And NumTests < 4000000 Then GoTo Restart1

        'BACKTRACKING Algorithm #2 - 3-way swaps
        Changes = 0
        For PairingCounter = 1 To PairingCount
            For PanelCounter = 1 To PanelSize
                JudgeCounter = JudgeIndex(AssignedJudges(PairingCounter, PanelCounter))
                For PairingCounter2 = PairingCounter + 1 To PairingCount
                    For PanelCounter2 = 1 To PanelSize
                        JudgeCounter2 = JudgeIndex(AssignedJudges(PairingCounter2, PanelCounter2))
                        If PairingCohort(PairingCounter) = PairingCohort(PairingCounter2) _
                         Or BelowMaximum(PairingCounter2, JudgeCounter) <= MaxIndivPref(PairingCohort(PairingCounter2)) Then
                            For PairingCounter3 = PairingCounter2 + 1 To PairingCount
                                For PanelCounter3 = 1 To PanelSize
                                    JudgeCounter3 = JudgeIndex(AssignedJudges(PairingCounter3, PanelCounter3))
                                    'Count Number of Tests
                                    NumTests = NumTests + 1
                                    If (PairingCohort(PairingCounter) = PairingCohort(PairingCounter3) _
                                      And PairingCohort(PairingCounter2) = PairingCohort(PairingCounter3)) _
                                      Or (BelowMaximum(PairingCounter, JudgeCounter3) <= MaxIndivPref(PairingCohort(PairingCounter)) _
                                      And BelowMaximum(PairingCounter3, JudgeCounter2) <= MaxIndivPref(PairingCohort(PairingCounter3))) Then

                                        'Evaluation of Impact of Panel Variances
                                        TempVariance1 = PanelVariance(PairingCounter) _
                                                      - PanelImpact(PairingCounter, JudgeCounter) _
                                                      + PanelImpact(PairingCounter, JudgeCounter3)
                                        TempVariance2 = PanelVariance(PairingCounter2) _
                                                      - PanelImpact(PairingCounter2, JudgeCounter2) _
                                                      + PanelImpact(PairingCounter2, JudgeCounter)
                                        TempVariance3 = PanelVariance(PairingCounter3) _
                                                      - PanelImpact(PairingCounter3, JudgeCounter3) _
                                                      + PanelImpact(PairingCounter3, JudgeCounter2)

                                        'CONSTRAINT - Max Panel Variance and Max Individual Variance
                                        If Math.Abs(TempVariance1) <= MaxPanelDiff(PairingCohort(PairingCounter)) _
                                          And Math.Abs(TempVariance2) <= MaxPanelDiff(PairingCohort(PairingCounter2)) _
                                          And Math.Abs(TempVariance3) <= MaxPanelDiff(PairingCohort(PairingCounter3)) _
                                          And Math.Abs(PanelImpact(PairingCounter, JudgeCounter3)) <= MaxIndivDiff(PairingCohort(PairingCounter)) _
                                          And Math.Abs(PanelImpact(PairingCounter2, JudgeCounter)) <= MaxIndivDiff(PairingCohort(PairingCounter2)) _
                                          And Math.Abs(PanelImpact(PairingCounter3, JudgeCounter2)) <= MaxIndivDiff(PairingCohort(PairingCounter3)) Then

                                            'Evaluation of Current
                                            Eval = MutualPref(PairingCounter, JudgeCounter) _
                                                 + MutualPref(PairingCounter2, JudgeCounter2) _
                                                 + MutualPref(PairingCounter3, JudgeCounter3) _
                                                 + Math.Abs(PanelVariance(PairingCounter)) * PanelDiffWeight _
                                                 + Math.Abs(PanelVariance(PairingCounter2)) * PanelDiffWeight _
                                                 + Math.Abs(PanelVariance(PairingCounter3)) * PanelDiffWeight

                                            'Evaluation of Permutation
                                            Evalx = MutualPref(PairingCounter, JudgeCounter3) _
                                                  + MutualPref(PairingCounter2, JudgeCounter) _
                                                  + MutualPref(PairingCounter3, JudgeCounter2) _
                                                  + Math.Abs(TempVariance1) * PanelDiffWeight _
                                                  + Math.Abs(TempVariance2) * PanelDiffWeight _
                                                  + Math.Abs(TempVariance3) * PanelDiffWeight

                                            If Evalx < Eval Then
                                                Changes = Changes + 1
                                                PanelVariance(PairingCounter) = TempVariance1
                                                PanelVariance(PairingCounter2) = TempVariance2
                                                PanelVariance(PairingCounter3) = TempVariance3
                                                AssignedJudges(PairingCounter, PanelCounter) = JudgeCode(JudgeCounter3)
                                                AssignedJudges(PairingCounter2, PanelCounter2) = JudgeCode(JudgeCounter)
                                                AssignedJudges(PairingCounter3, PanelCounter3) = JudgeCode(JudgeCounter2)
                                                JudgeCounter2 = JudgeCounter
                                                JudgeCounter = JudgeCounter3
                                            End If
                                        End If
                                    End If
                                Next
                            Next
                        End If
                    Next
                Next
            Next
        Next
        If Changes > 0 And NumTests < 4000000 Then GoTo Restart1

        'BACKTRACKING #3 - Fix pairings worse than targeted maximum by using EXTRA judges
        Changes = 0
        For PairCohort = 1 To 3
            For PairingCounter = 1 To PairingCount
                If PairCohort = PairingCohort(PairingCounter) Then
                    For PanelCounter = 1 To PanelSize
                        JudgeCounter = JudgeIndex(AssignedJudges(PairingCounter, PanelCounter))
                        If BelowMaximum(PairingCounter, JudgeCounter) > MaxIndivPref(PairingCohort(PairingCounter)) Then
                            Evalx = 100000.0
                            For JudgeCounter2 = 1 To JudgeCount
                                NumTests = NumTests + 1
                                If JudgeAvail(JudgeCounter2) = True _
                                  And BelowMaximum(PairingCounter, JudgeCounter2) <= MaxIndivPref(PairingCohort(PairingCounter)) _
                                  And Math.Abs(PanelImpact(PairingCounter, JudgeCounter2)) <= MaxIndivDiff(PairingCohort(PairingCounter)) _
                                  And Math.Abs(PanelVariance(PairingCounter) - PanelImpact(PairingCounter, JudgeCounter) + PanelImpact(PairingCounter, JudgeCounter2)) <= MaxPanelDiff(PairingCohort(PairingCounter)) Then
                                    'Assign baseline weighted value for mutuality and preference
                                    Eval = MutualPref(PairingCounter, JudgeCounter2)
                                    'Assign weighted value for panel mutuality
                                    Mutuality = Math.Abs(PanelVariance(PairingCounter) - PanelImpact(PairingCounter, JudgeCounter) + PanelImpact(PairingCounter, JudgeCounter2))
                                    Eval = Eval + Mutuality * PanelDiffWeight
                                    'Judge Rating - Higher Judge Rating DECREASES Score
                                    Eval = Eval - (2 * JudgeRating(JudgeCounter2) * JudgePrefWeight)
                                    If Eval < Evalx Then
                                        Evalx = Eval
                                        BestJudgeCounter = JudgeCounter2
                                    End If
                                End If
                            Next
                            If Evalx < 100000.0 Then
                                'Assign the best available fix to the pairing in order to meet threshold (BestJudgeCounter)
                                AssignedJudges(PairingCounter, PanelCounter) = JudgeCode(BestJudgeCounter)
                                'Place previously assigned judge back into the available pool
                                JudgeAvail(JudgeCounter) = True
                                'Mark new judge that is assigned as unavailable
                                JudgeAvail(BestJudgeCounter) = False
                                'Readjust panel and region variance to represent new judge assignment
                                PanelVariance(PairingCounter) = PanelVariance(PairingCounter) - PanelImpact(PairingCounter, JudgeCounter) + PanelImpact(PairingCounter, BestJudgeCounter)
                                Changes = Changes + 1
                                'Count how many judges are lost due to fixing panels - activates backtracking algorithm #6
                                If JudgeOblige(JudgeCounter) = True And JudgeOblige(BestJudgeCounter) = False Then LostJudges = LostJudges + 1
                            End If
                        End If
                    Next
                End If
            Next
        Next

        'BACKTRACKING #3B - Fix pairings worse than targeted maximum by using swaps and EXTRA judges
        Changes = 0
        For PairingCounter = 1 To PairingCount
            For PanelCounter = 1 To PanelSize
                JudgeCounter = JudgeIndex(AssignedJudges(PairingCounter, PanelCounter))
                'Identify PROBLEM pairing by determining whether assigned judge is currently above the targeted maximum
                If BelowMaximum(PairingCounter, JudgeCounter) > MaxIndivPref(PairingCohort(PairingCounter)) Then
                    'Test other pairings to see if an already assigned judge fixes the problem
                    Evalx = 100000.0
                    For PairingCounter2 = 1 To PairingCount
                        'Check to make sure tested pairing isn't the same as pairing to be fixed
                        If PairingCounter2 <> PairingCounter Then
                            For PanelCounter2 = 1 To PanelSize
                                JudgeCounter2 = JudgeIndex(AssignedJudges(PairingCounter2, PanelCounter2))
                                'Ensure that judge from tested pairing meets constraints for pairing to be fixed
                                If BelowMaximum(PairingCounter, JudgeCounter2) <= MaxIndivPref(PairingCohort(PairingCounter)) _
                                  And Math.Abs(PanelImpact(PairingCounter, JudgeCounter2)) <= MaxIndivDiff(PairingCohort(PairingCounter)) _
                                  And Math.Abs(PanelVariance(PairingCounter) - PanelImpact(PairingCounter, JudgeCounter) + PanelImpact(PairingCounter, JudgeCounter2)) <= MaxPanelDiff(PairingCohort(PairingCounter)) Then
                                    'Check unassigned judges to see if they will fit in test pairing so its current judge can be reassigned to fix original problem
                                    For JudgeCounter3 = 1 To JudgeCount
                                        NumTests = NumTests + 1
                                        'Test to see if EXTRA judge meets constraints for the pairing that will have its judge swapped
                                        If JudgeAvail(JudgeCounter3) = True _
                                          And BelowMaximum(PairingCounter2, JudgeCounter3) <= MaxIndivPref(PairingCohort(PairingCounter2)) _
                                          And Math.Abs(PanelImpact(PairingCounter2, JudgeCounter3)) <= MaxIndivDiff(PairingCohort(PairingCounter2)) _
                                          And Math.Abs(PanelVariance(PairingCounter2) - PanelImpact(PairingCounter2, JudgeCounter2) + PanelImpact(PairingCounter2, JudgeCounter3)) <= MaxPanelDiff(PairingCohort(PairingCounter2)) Then
                                            'Assign baseline weighted value for mutuality and preference
                                            Eval = MutualPref(PairingCounter, JudgeCounter2) + MutualPref(PairingCounter2, JudgeCounter3)
                                            'Assign weighted value for panel mutuality for swapped judge in problem panel
                                            Mutuality = Math.Abs(PanelVariance(PairingCounter) - PanelImpact(PairingCounter, JudgeCounter) + PanelImpact(PairingCounter, JudgeCounter2))
                                            Eval = Eval + Mutuality * PanelDiffWeight
                                            'Assing weighted value for panel mutuality for EXTRA judge in swapped panel
                                            Mutuality = Math.Abs(PanelVariance(PairingCounter2) - PanelImpact(PairingCounter2, JudgeCounter2) + PanelImpact(PairingCounter2, JudgeCounter3))
                                            Eval = Eval + Mutuality * PanelDiffWeight
                                            'Judge Rating - Higher Judge Rating DECREASES Score
                                            Eval = Eval - (2 * JudgeRating(JudgeCounter3) * JudgePrefWeight)
                                            If Eval < Evalx Then
                                                Evalx = Eval
                                                BestJudgeCounter = JudgeCounter3
                                                BestPairingCounter = PairingCounter2
                                                BestPanelCounter = PanelCounter2
                                                BestJudgeSwapCounter = JudgeCounter2
                                            End If
                                        End If
                                    Next
                                End If
                            Next
                        End If
                    Next
                    If Evalx < 100000.0 Then
                        'Assign the best available fix to the pairing in order to meet threshold (BestJudgeCounter)
                        AssignedJudges(PairingCounter, PanelCounter) = JudgeCode(BestJudgeSwapCounter)
                        AssignedJudges(BestPairingCounter, BestPanelCounter) = JudgeCode(BestJudgeCounter)
                        'Place previously assigned judge back into the available pool
                        JudgeAvail(JudgeCounter) = True
                        'Mark new judge that is assigned as unavailable
                        JudgeAvail(BestJudgeCounter) = False
                        'Readjust panel and region variance to represent new judge assignment
                        PanelVariance(BestPairingCounter) = PanelVariance(BestPairingCounter) - PanelImpact(BestPairingCounter, BestJudgeSwapCounter) + PanelImpact(BestPairingCounter, BestJudgeCounter)
                        PanelVariance(PairingCounter) = PanelVariance(PairingCounter) - PanelImpact(PairingCounter, JudgeCounter) + PanelImpact(PairingCounter, BestJudgeSwapCounter)
                        Changes = Changes + 1
                        'Count how many judges are lost due to fixing panels - activates backtracking algorithm #6
                        If JudgeOblige(JudgeCounter) = True And JudgeOblige(BestJudgeCounter) = False Then LostJudges = LostJudges + 1
                        JudgeCounter = BestJudgeSwapCounter
                    Else
                        'Beep()
                    End If
                End If
            Next
        Next

Restart4:
        'BACKTRACKING Algorithm #4 - Two-way swaps redone with added judges
        Changes = 0
        For PairingCounter = 1 To PairingCount
            For PanelCounter = 1 To PanelSize
                JudgeCounter = JudgeIndex(AssignedJudges(PairingCounter, PanelCounter))
                For PairingCounter2 = PairingCounter + 1 To PairingCount
                    For PanelCounter2 = 1 To PanelSize
                        JudgeCounter2 = JudgeIndex(AssignedJudges(PairingCounter2, PanelCounter2))
                        'Count Number of Tests
                        NumTests = NumTests + 1
                        'Changes ONLY considered if both resulting pairings meet maximum threshold
                        If (BelowMaximum(PairingCounter, JudgeCounter2) <= MaxIndivPref(PairingCohort(PairingCounter)) _
                          And BelowMaximum(PairingCounter2, JudgeCounter) <= MaxIndivPref(PairingCohort(PairingCounter2))) _
                          And Math.Abs(PanelImpact(PairingCounter, JudgeCounter2)) <= MaxIndivDiff(PairingCohort(PairingCounter)) _
                          And Math.Abs(PanelImpact(PairingCounter2, JudgeCounter)) <= MaxIndivDiff(PairingCohort(PairingCounter2)) Then

                            'Evaluation of Impact of Panel Variances
                            TempVariance1 = PanelVariance(PairingCounter) _
                                          - PanelImpact(PairingCounter, JudgeCounter) _
                                          + PanelImpact(PairingCounter, JudgeCounter2)
                            TempVariance2 = PanelVariance(PairingCounter2) _
                                          - PanelImpact(PairingCounter2, JudgeCounter2) _
                                          + PanelImpact(PairingCounter2, JudgeCounter)

                            'CONSTRAINT - Max Panel Variance and Max Individual Variance
                            If Math.Abs(TempVariance1) <= MaxPanelDiff(PairingCohort(PairingCounter)) _
                              And Math.Abs(TempVariance2) <= MaxPanelDiff(PairingCohort(PairingCounter2)) Then

                                'Evaluation of Current
                                Eval = MutualPref(PairingCounter, JudgeCounter) _
                                     + MutualPref(PairingCounter2, JudgeCounter2) _
                                     + Math.Abs(PanelVariance(PairingCounter)) * PanelDiffWeight _
                                     + Math.Abs(PanelVariance(PairingCounter2)) * PanelDiffWeight
                                'Evaluation of Permutation
                                Evalx = MutualPref(PairingCounter, JudgeCounter2) _
                                      + MutualPref(PairingCounter2, JudgeCounter) _
                                      + Math.Abs(TempVariance1) * PanelDiffWeight _
                                      + Math.Abs(TempVariance2) * PanelDiffWeight
                                If Evalx < Eval Then
                                    Changes = Changes + 1
                                    PanelVariance(PairingCounter) = TempVariance1
                                    PanelVariance(PairingCounter2) = TempVariance2
                                    AssignedJudges(PairingCounter, PanelCounter) = JudgeCode(JudgeCounter2)
                                    AssignedJudges(PairingCounter2, PanelCounter2) = JudgeCode(JudgeCounter)
                                    JudgeCounter = JudgeCounter2
                                End If
                            End If
                        End If
                    Next
                Next
            Next
        Next
        If Changes > 0 And NumTests < 4000000 Then GoTo Restart4

        'BACKTRACKING Algorithm #5 - Two-way swaps redone with added judges from fixing pairings in BACKTRACKING #3
        Changes = 0
        For PairingCounter = 1 To PairingCount
            For PanelCounter = 1 To PanelSize
                JudgeCounter = JudgeIndex(AssignedJudges(PairingCounter, PanelCounter))
                For PairingCounter2 = PairingCounter + 1 To PairingCount
                    For PanelCounter2 = 1 To PanelSize
                        JudgeCounter2 = JudgeIndex(AssignedJudges(PairingCounter2, PanelCounter2))
                        If BelowMaximum(PairingCounter2, JudgeCounter) <= MaxIndivPref(PairingCohort(PairingCounter2)) Then
                            For PairingCounter3 = PairingCounter2 + 1 To PairingCount
                                For PanelCounter3 = 1 To PanelSize
                                    JudgeCounter3 = JudgeIndex(AssignedJudges(PairingCounter3, PanelCounter3))
                                    'Count Number of Tests
                                    NumTests = NumTests + 1
                                    If (BelowMaximum(PairingCounter, JudgeCounter3) <= MaxIndivPref(PairingCohort(PairingCounter)) _
                                      And BelowMaximum(PairingCounter3, JudgeCounter2) <= MaxIndivPref(PairingCohort(PairingCounter3))) Then

                                        'Evaluation of Impact of Panel Variances
                                        TempVariance1 = PanelVariance(PairingCounter) _
                                                      - PanelImpact(PairingCounter, JudgeCounter) _
                                                      + PanelImpact(PairingCounter, JudgeCounter3)
                                        TempVariance2 = PanelVariance(PairingCounter2) _
                                                      - PanelImpact(PairingCounter2, JudgeCounter2) _
                                                      + PanelImpact(PairingCounter2, JudgeCounter)
                                        TempVariance3 = PanelVariance(PairingCounter3) _
                                                      - PanelImpact(PairingCounter3, JudgeCounter3) _
                                                      + PanelImpact(PairingCounter3, JudgeCounter2)

                                        'CONSTRAINT - Max Panel Variance and Max Individual Variance
                                        If Math.Abs(TempVariance1) <= MaxPanelDiff(PairingCohort(PairingCounter)) _
                                          And Math.Abs(TempVariance2) <= MaxPanelDiff(PairingCohort(PairingCounter2)) _
                                          And Math.Abs(TempVariance3) <= MaxPanelDiff(PairingCohort(PairingCounter3)) _
                                          And Math.Abs(PanelImpact(PairingCounter, JudgeCounter3)) <= MaxIndivDiff(PairingCohort(PairingCounter)) _
                                          And Math.Abs(PanelImpact(PairingCounter2, JudgeCounter)) <= MaxIndivDiff(PairingCohort(PairingCounter2)) _
                                          And Math.Abs(PanelImpact(PairingCounter3, JudgeCounter2)) <= MaxIndivDiff(PairingCohort(PairingCounter3)) Then

                                            'Evaluation of Current
                                            Eval = MutualPref(PairingCounter, JudgeCounter) _
                                                 + MutualPref(PairingCounter2, JudgeCounter2) _
                                                 + MutualPref(PairingCounter3, JudgeCounter3) _
                                                 + Math.Abs(PanelVariance(PairingCounter)) * PanelDiffWeight _
                                                 + Math.Abs(PanelVariance(PairingCounter2)) * PanelDiffWeight _
                                                 + Math.Abs(PanelVariance(PairingCounter3)) * PanelDiffWeight

                                            'Evaluation of Permutation
                                            Evalx = MutualPref(PairingCounter, JudgeCounter3) _
                                                  + MutualPref(PairingCounter2, JudgeCounter) _
                                                  + MutualPref(PairingCounter3, JudgeCounter2) _
                                                  + Math.Abs(TempVariance1) * PanelDiffWeight _
                                                  + Math.Abs(TempVariance2) * PanelDiffWeight _
                                                  + Math.Abs(TempVariance3) * PanelDiffWeight

                                            If Evalx < Eval Then
                                                Changes = Changes + 1
                                                PanelVariance(PairingCounter) = TempVariance1
                                                PanelVariance(PairingCounter2) = TempVariance2
                                                PanelVariance(PairingCounter3) = TempVariance3
                                                AssignedJudges(PairingCounter, PanelCounter) = JudgeCode(JudgeCounter3)
                                                AssignedJudges(PairingCounter2, PanelCounter2) = JudgeCode(JudgeCounter)
                                                AssignedJudges(PairingCounter3, PanelCounter3) = JudgeCode(JudgeCounter2)
                                                JudgeCounter2 = JudgeCounter
                                                JudgeCounter = JudgeCounter3
                                            End If
                                        End If
                                    End If
                                Next
                            Next
                        End If
                    Next
                Next
            Next
        Next
        If Changes > 0 And NumTests < 4000000 Then GoTo Restart4

Restart6:
        'Backtracking Algorithm #6 to push excluded judges who were obligated back into pairing
        For JudgeCounter2 = 1 To JudgeCount
            If JudgeAvail(JudgeCounter2) = True And JudgeOblige(JudgeCounter2) = True Then
                Evalx = 100000.0
                For PairingCounter = 1 To PairingCount
                    For PanelCounter = 1 To PanelSize
                        NumTests = NumTests + 1
                        JudgeCounter = JudgeIndex(AssignedJudges(PairingCounter, PanelCounter))
                        If JudgeOblige(JudgeCounter) = False _
                          And BelowMaximum(PairingCounter, JudgeCounter2) <= MaxIndivPref(PairingCohort(PairingCounter)) _
                          And Math.Abs(PanelImpact(PairingCounter, JudgeCounter2)) <= MaxIndivDiff(PairingCohort(PairingCounter)) _
                          And Math.Abs(PanelVariance(PairingCounter) - PanelImpact(PairingCounter, JudgeCounter) + PanelImpact(PairingCounter, JudgeCounter2)) <= MaxPanelDiff(PairingCohort(PairingCounter)) Then
                            'Assign baseline weighted value for mutuality and preference
                            Eval = MutualPref(PairingCounter, JudgeCounter2)
                            'Assign weighted value for panel mutuality
                            Mutuality = Math.Abs(PanelVariance(PairingCounter) - PanelImpact(PairingCounter, JudgeCounter) + PanelImpact(PairingCounter, JudgeCounter2))
                            Eval = Eval + Mutuality * IndivDiffWeight
                            If Eval < Evalx Then
                                Evalx = Eval
                                BestPairingCounter = PairingCounter
                                BestPanelCounter = PanelCounter
                            End If
                        End If
                    Next
                Next
                If Evalx < 100000.0 Then
                    PairingCounter = BestPairingCounter
                    JudgeCounter = JudgeIndex(AssignedJudges(BestPairingCounter, BestPanelCounter))
                    AssignedJudges(BestPairingCounter, BestPanelCounter) = JudgeCode(JudgeCounter2)
                    JudgeAvail(JudgeCounter) = True
                    JudgeAvail(JudgeCounter2) = False
                    PanelVariance(BestPairingCounter) = PanelVariance(BestPairingCounter) - PanelImpact(BestPairingCounter, JudgeCounter) + PanelImpact(BestPairingCounter, JudgeCounter2)
                    LostJudges = LostJudges - 1
                    If LostJudges <= ExtraJudges And Test1 = 5 Then GoTo Completion
                End If
            End If
        Next
        If LostJudges > ExtraJudges Then
            Test1 = 5
            If NumCohorts = 1 And MaxIndivPref(2) < JudgeCount Then
                MaxIndivPref(2) = MaxIndivPref(2) * 1.05
                MaxIndivDiff(2) = MaxIndivDiff(2) * 1.1
                MaxPanelDiff(2) = MaxPanelDiff(2) * 1.1
            ElseIf NumCohorts > 1 And MaxIndivPref(3) < JudgeCount Then
                MaxIndivPref(3) = MaxIndivPref(3) * 1.05
                MaxIndivDiff(3) = MaxIndivDiff(3) * 1.1
                MaxPanelDiff(3) = MaxPanelDiff(3) * 1.1
            ElseIf NumCohorts > 1 And MaxIndivPref(2) < JudgeCount Then
                MaxIndivPref(2) = MaxIndivPref(2) * 1.05
                MaxIndivDiff(2) = MaxIndivDiff(2) * 1.1
                MaxPanelDiff(2) = MaxPanelDiff(2) * 1.1
            ElseIf NumCohorts = 3 And MaxIndivPref(1) < JudgeCount Then
                MaxIndivPref(1) = MaxIndivPref(1) * 1.05
                MaxIndivDiff(1) = MaxIndivDiff(1) * 1.1
                MaxPanelDiff(1) = MaxPanelDiff(1) * 1.1
            Else
                'Beep()
                'Beep()
                GoTo Completion
            End If

            GoTo Restart6
        End If

Completion:

        'Calculate judge availability and preference averages
        AvgPrefBefore = 0
        AvgPrefBeforeCount = 0

        For JudgeCounter = 1 To JudgeCount
            AvgPrefBefore = AvgPrefBefore + JudgeRating(JudgeCounter) * RoundsAvail(JudgeCounter)
            AvgPrefBeforeCount = AvgPrefBeforeCount + RoundsAvail(JudgeCounter)
        Next

        AvgPrefAfter = AvgPrefBefore
        AvgPrefAfterCount = AvgPrefBeforeCount
        ReDim JudgeUsed(JudgeCount)
        For PairingCounter = 1 To PairingCount
            For PanelCounter = 1 To PanelSize
                If JudgeIndex(AssignedJudges(PairingCounter, PanelCounter)) > 0 Then
                    JudgeUsed(JudgeIndex(AssignedJudges(PairingCounter, PanelCounter))) = True
                    AvgPrefAfter = AvgPrefAfter - JudgeRating(JudgeIndex(AssignedJudges(PairingCounter, PanelCounter)))
                    AvgPrefAfterCount = AvgPrefAfterCount - 1
                End If
            Next
        Next
        LostJudges = 0
        For JudgeCounter = 1 To JudgeCount
            If JudgeOblige(JudgeCounter) = True And JudgeUsed(JudgeCounter) = False Then
                LostJudges = LostJudges + 1
                AvgPrefAfter = AvgPrefAfter - JudgeRating(JudgeCounter)
                AvgPrefAfterCount = AvgPrefAfterCount - 1
            End If
        Next
        If AvgPrefBeforeCount > 0 Then AvgPrefBefore = AvgPrefBefore / AvgPrefBeforeCount
        If AvgPrefAfterCount < AvgPrefBeforeCount Then AvgPrefAfter = AvgPrefAfter / AvgPrefAfterCount Else AvgPrefAfter = AvgPrefBefore

        'Variables required for MPJ Diagnostics
        PanelSumAff = 0
        PanelSumNeg = 0
        ReDim IndivAverage(3)
        ReDim PrefCount(3)
        ReDim IndivDifference(3)
        ReDim PanelDifference(3)
        ReDim WorsethanTarget(3)
        ReDim WorstIndividual(3)
        ReDim WorstPanel(3)

        'Display MPJ diagnostics for debate if judges have already been assigned
        For PairingCounter = 1 To PairingCount
            For PanelCounter = 1 To PanelSize
                If AssignedJudges(PairingCounter, PanelCounter) > 0 Then
                    PanelSumAff = PanelSumAff + PrefDataPct(PairingCounter, 0, JudgeIndex(AssignedJudges(PairingCounter, PanelCounter)))
                    PanelSumNeg = PanelSumNeg + PrefDataPct(PairingCounter, 1, JudgeIndex(AssignedJudges(PairingCounter, PanelCounter)))
                    IndivAverage(PairingCohort(PairingCounter)) = IndivAverage(PairingCohort(PairingCounter)) + PrefDataPct(PairingCounter, 0, JudgeIndex(AssignedJudges(PairingCounter, PanelCounter))) + PrefDataPct(PairingCounter, 1, JudgeIndex(AssignedJudges(PairingCounter, PanelCounter)))
                    'Determine whether aff or neg pref for judge is WorstIndividual for the Cohort
                    If PrefDataPct(PairingCounter, 0, JudgeIndex(AssignedJudges(PairingCounter, PanelCounter))) > WorstIndividual(PairingCohort(PairingCounter)) Then WorstIndividual(PairingCohort(PairingCounter)) = PrefDataPct(PairingCounter, 0, JudgeIndex(AssignedJudges(PairingCounter, PanelCounter)))
                    If PrefDataPct(PairingCounter, 1, JudgeIndex(AssignedJudges(PairingCounter, PanelCounter))) > WorstIndividual(PairingCohort(PairingCounter)) Then WorstIndividual(PairingCohort(PairingCounter)) = PrefDataPct(PairingCounter, 1, JudgeIndex(AssignedJudges(PairingCounter, PanelCounter)))

                    IndivDifference(PairingCohort(PairingCounter)) = IndivDifference(PairingCohort(PairingCounter)) + Math.Abs(PrefDataPct(PairingCounter, 0, JudgeIndex(AssignedJudges(PairingCounter, PanelCounter))) - PrefDataPct(PairingCounter, 1, JudgeIndex(AssignedJudges(PairingCounter, PanelCounter))))
                    PrefCount(PairingCohort(PairingCounter)) = PrefCount(PairingCohort(PairingCounter)) + 2
                End If
            Next
            If PanelSumAff + PanelSumNeg > WorstPanel(PairingCohort(PairingCounter)) Then WorstPanel(PairingCohort(PairingCounter)) = PanelSumAff + PanelSumNeg
            PanelDifference(PairingCohort(PairingCounter)) = PanelDifference(PairingCohort(PairingCounter)) + Math.Abs(PanelSumAff - PanelSumNeg)
            PanelSumAff = 0
            PanelSumNeg = 0

        Next

        For PairCohort = 1 To 3
            If PrefCount(PairCohort) > 0 Then
                'Aggregate Cohorts into Overall Results
                IndivAverage(0) = IndivAverage(0) + IndivAverage(PairCohort)
                IndivDifference(0) = IndivDifference(0) + IndivDifference(PairCohort)
                PanelDifference(0) = PanelDifference(0) + PanelDifference(PairCohort)
                PrefCount(0) = PrefCount(0) + PrefCount(PairCohort)
                'Calculate Averages for Individual Cohorts
                IndivAverage(PairCohort) = IndivAverage(PairCohort) / PrefCount(PairCohort)
                IndivDifference(PairCohort) = IndivDifference(PairCohort) / PrefCount(PairCohort) * 2
                PanelDifference(PairCohort) = PanelDifference(PairCohort) / PrefCount(PairCohort) * 2 * PanelSize
            End If
        Next
        'Calculate Averages for Overall Results
        If PrefCount(0) > 0 Then
            IndivAverage(0) = IndivAverage(0) / PrefCount(0)
            IndivDifference(0) = IndivDifference(0) / PrefCount(0) * 2
            PanelDifference(0) = PanelDifference(0) / PrefCount(0) * 2 * PanelSize
        End If

        'Display MPJ diagnostics on Form
        LabIndivAvgAllNEW.Text = Format(IndivAverage(0), "##0.00")
        LabIndivAvgBreakNEW.Text = Format(IndivAverage(1), "##0.00")
        LabIndivAvgAboveNEW.Text = Format(IndivAverage(2), "##0.00")
        LabIndivAvgBelowNEW.Text = Format(IndivAverage(3), "##0.00")
        LabWorstBreakNEW.Text = Format(WorstIndividual(1), "##0.00")
        LabWorstAboveNEW.Text = Format(WorstIndividual(2), "##0.00")
        LabWorstBelowNEW.Text = Format(WorstIndividual(3), "##0.00")
        LabWorstPanelBreakNEW.Text = Format(WorstPanel(1) / (2 * PanelSize), "##0.00")
        LabWorstPanelAboveNEW.Text = Format(WorstPanel(2) / (2 * PanelSize), "##0.00")
        LabWorstPanelBelowNEW.Text = Format(WorstPanel(3) / (2 * PanelSize), "##0.00")
        labIndivDiffAvgAllNEW.Text = Format(IndivDifference(0), "##0.00")
        labIndivDiffAvgBreakNEW.Text = Format(IndivDifference(1), "##0.00")
        labIndivDiffAvgAboveNEW.Text = Format(IndivDifference(2), "##0.00")
        labIndivDiffAvgBelowNEW.Text = Format(IndivDifference(3), "##0.00")
        labPanelDiffAvgAllNEW.Text = Format(PanelDifference(0), "##0.00")
        labPanelDiffAvgBreakNEW.Text = Format(PanelDifference(1), "##0.00")
        labPanelDiffAvgAboveNEW.Text = Format(PanelDifference(2), "##0.00")
        labPanelDiffAvgBelowNEW.Text = Format(PanelDifference(3), "##0.00")
        LabJudgePrefBefore.Text = Format(AvgPrefBefore, "##0.00")
        LabJudgePrefAfterNEW.Text = Format(AvgPrefAfter, "##0.00")
        LabLostJudgesNEW.Text = LostJudges
        If LostJudges > 39 Then
            LostJudges = 39
        End If

        Timer.Stop()
        Dim ts As TimeSpan = Timer.Elapsed
        Dim ElapsedTime As String = String.Format("{0:00}:{1:00}.{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10)
        LabElapsedTime.Text = ElapsedTime

    End Sub
    Sub QuickSort1(ByRef pvarArray As Object, ByRef idxArray As Object, Optional ByVal plngLeft As Long = 0, Optional ByVal plngRight As Long = 0)

        Dim lngFirst As Long
        Dim lngLast As Long
        Dim varMid As Object
        Dim varSwap As Object
        Dim idxSwap As Object

        If plngRight = 0 Then
            plngLeft = LBound(pvarArray)
            plngRight = UBound(pvarArray)
        End If
        lngFirst = plngLeft
        lngLast = plngRight
        varMid = pvarArray((plngLeft + plngRight) \ 2)
        Do
            Do While pvarArray(lngFirst) < varMid And lngFirst < plngRight
                lngFirst = lngFirst + 1
            Loop
            Do While varMid < pvarArray(lngLast) And lngLast > plngLeft
                lngLast = lngLast - 1
            Loop
            If lngFirst <= lngLast Then
                varSwap = pvarArray(lngFirst)
                pvarArray(lngFirst) = pvarArray(lngLast)
                pvarArray(lngLast) = varSwap
                idxSwap = idxArray(lngFirst)
                idxArray(lngFirst) = idxArray(lngLast)
                idxArray(lngLast) = idxSwap
                lngFirst = lngFirst + 1
                lngLast = lngLast - 1
            End If
        Loop Until lngFirst > lngLast
        If plngLeft < lngLast Then QuickSort1(pvarArray, idxArray, plngLeft, lngLast)
        If lngFirst < plngRight Then QuickSort1(pvarArray, idxArray, lngFirst, plngRight)

    End Sub
    'PROCEDURES to activate instructions when TextBoxes or CheckBoxes have focus
    Private Sub TextBoxRoundNumber_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        LabelInstructions.Text = "Enter the Round Number that you wish to assign judges for.  Enter 1-9 for Prelims Enter 10-16 for Elims based on " & _
        "the information you entered on the TimeBlock grid."
    End Sub
    Private Sub TextBoxDvisions_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        LabelInstructions.Text = "Enter the Division number that you wish to assign judges for.  You can enter judges simultaneously to more than one " & _
        "division by entering multiple division numbers (no spaces or punctuation).  If you have more than one cohort and are assigning judges to multiple divisions at the " & _
        "time, it will assign judges to the first cohort for all divisions before assigning judges to the next cohort.  This results in all divisions " & _
        "having substantially the same level of preference and mutuality.  If you wish to privilege a particular division, you should assign judges " & _
        "one division at a time."
    End Sub
    Private Sub CheckBoxJudgeAgain_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBoxJudgeAgain.GotFocus
        LabelInstructions.Text = "If the box is checked, judges will NOT be permitted to judge a team more than once.  For most tournaments, this is " & _
        "the default for preliminary rounds.  Most tournaments, however, do permit judges to judge a team again once the tournaments is in elim rounds. " & _
        "To permit judges to hear a team a second time, the box should NOT be checked."
    End Sub
    Private Sub TextBoxPanelSize_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBoxPanelSize.GotFocus
        LabelInstructions.Text = "Enter the number of judges that you wish to have the computer to assign per panel (1-x).  If you are " & _
        "using strike cards indicate the number of judges that you wish to include on the card."
    End Sub
    Private Sub TextBoxBreakPoint_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBoxBreakPoint.GotFocus
        LabelInstructions.Text = "If you are using more than one cohort, enter the number of losses that you consider to be the break point " & _
        "(the maximum number of losses a team can have and still break).  If, however, you believe that it is possible for a 4-4 to break, " & _
        "you may still wish to define the break point for purposes of privileged judge assignments to be set at 3 losses."
    End Sub
    Private Sub TextBoxNumCohorts_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBoxNumCohorts.GotFocus
        LabelInstructions.Text = "Enter the number of cohorts you wish to use.  If you enter 1, all rounds will be paired at the same time. " & _
        "If you enter 2, rounds involving teams with <= the number of 'break point' losses will be paired first followed by those with more. " & _
        "If you enter 3, rounds involving teams at the break point will be paired first followed by those above and then those below."
    End Sub
    Private Sub TextBoxRegionCnst_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        LabelInstructions.Text = "If you enter 0 then no judge from the region represented by a team in the debate can be assigned to the round unless " & _
        "a judge is also assigned from the region representing the other team (moot if teams from same region).  If a number greater than 0 " & _
        "is entered, the difference can't be greater than the number enter (e.g. if 1, then it is OK to have 1 judge from one region but not the other " & _
        "or 2 have one and 1 from the other. You csouldn't however from 2 judges from one region and none from the other.  NOTE: The constraint ONLY functions " & _
        "for teams that have NOT been eliminated."
    End Sub
    Private Sub CheckBoxRegionCnst_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        LabelInstructions.Text = "Check if you wish to use regional constraints on judge assignments."
    End Sub
    Private Sub TextBoxPrefWeight_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBoxIndivPrefWeight.GotFocus
        LabelInstructions.Text = "This parameter determines the relative weight given to maximizing PREFERENCE during MPJ judge assignments.  A higher " & _
        "value increases the exponential weight given to preference.  As a general rule, the parameter functions most effectively between a value of 0.5 " & _
        "and 2.0."
    End Sub
    Private Sub TextBoxTotalPanelWeight_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBoxPanelPrefWeight.GotFocus
        LabelInstructions.Text = "This parameter determines the relative weight given to maximizing PREFERENCE balanced within an entire panel during MPJ judge assignments.  A higher " & _
        "value increases the exponential weight given to preference.  As a general rule, the parameter functions most effectively between a value of 0.5 " & _
        "and 1.0."
    End Sub
    Private Sub TextBoxIndivDiffWeight_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBoxIndivDiffWeight.GotFocus
        LabelInstructions.Text = "This parameter determines the relative weight given to maximizing the MUTUALITY of an individual judge assignment.  It is " & _
        "typically set within a range of plus or minus 0.5 of the value assigned for Average Prefs.  If it is greater than the value for Preference then " & _
        "higher mutuality will be favored.  If it is less than Preference then higher Preference will be favored."
    End Sub
    Private Sub TextBoxPanelDiffWeight_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBoxPanelDiffWeight.GotFocus
        LabelInstructions.Text = "This parameter determines the relative weight given to maximizing the MUTUALITY for an overall panel.  It is " & _
        "typically set within a range of plus or minus 0.5 of the value assigned for Average Prefs.  If it is greater than the value for Preference then " & _
        "higher mutuality will be favored.  If it is less than Preference then higher Preference will be favored."
    End Sub
    Private Sub TextBoxJudgePrefWeight_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBoxJudgePrefWeight.GotFocus
        LabelInstructions.Text = "This parameter is one of the most important in effectively managing the use of partial commitment judges in order to ensure " & _
        "that highly preferred critics remain available throughout the tournament - particularly for break rounds late in the tournament.  As the value is increased " & _
        "the program will try harder to assign less preferred critics to a round, even if it results in slightly lower preference for the debate.  The goal, particularly " & _
        "early in the tournament should be to have the average remaining pref after a pairing to be at least as good or better than it was prior to the judge " & _
        "assignments.  It should also be noted that the parameter can take negative values, in which case it pushes more highly preferred judges into debates."
    End Sub
    Private Sub TextBoxMaxLostRounds_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBoxMaxLostRounds.GotFocus
        LabelInstructions.Text = "This CONSTRAINT specifies the number of obligated rounds that you are willing to lose. This becomes the highest level constraint (i.e. the " & _
        "constraint that the program will relax last).  NOTE: The program will only use 'extra' rounds to fix rounds that are worse than the target maximums so no more extra " & _
        "judges will be used than necessary."
    End Sub
    Private Sub TextBoxTargetAbove_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBoxTargetAbove.GotFocus
        LabelInstructions.Text = "This CONSTRAINT specifies a TARGET for the worst pref assignment for teams ABOVE the break point.  The ABOVE target applies no matter how " & _
        "many cohorts you use."
    End Sub
    Private Sub TextBoxTargetBREAK_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBoxTargetBREAK.GotFocus
        LabelInstructions.Text = "This CONSTRAINT specifies a TARGET for the worst pref assignment for teams in BREAK rounds.  The BREAK target only applies if you select " & _
        "3 cohorts"
    End Sub
    Private Sub TextBoxTargetBelow_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBoxTargetBelow.GotFocus
        LabelInstructions.Text = "This CONSTRAINT specifies a TARGET for the worst pref assignment for teams BELOW the break point.  The BELOW target applies if you select " & _
        "2 or 3 cohorts."
    End Sub
    Private Sub TextBoxMaxIndivDiffAbove_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBoxMaxIndivDiffAbove.GotFocus
        LabelInstructions.Text = "This CONSTRAINT specifies a TARGET for the maximum individual preference difference for teams ABOVE the break point.  The ABOVE target applies no matter how " & _
        "many cohorts you use."
    End Sub
    Private Sub TextBoxMaxIndivDiffBREAK_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBoxMaxIndivDiffBREAK.GotFocus
        LabelInstructions.Text = "This CONSTRAINT specifies a TARGET for the maximum individual preference difference for teams in BREAK rounds.  The BREAK target only applies if you select " & _
        "3 cohorts"
    End Sub
    Private Sub TextBoxMaxIndivDiffBelow_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBoxMaxIndivDiffBelow.GotFocus
        LabelInstructions.Text = "This CONSTRAINT specifies a TARGET for the maximum individual preference difference for teams BELOW the break point.  The BELOW target applies if you select " & _
        "2 or 3 cohorts."
    End Sub
    Private Sub TextBoxMaxPanelDiffAbove_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBoxMaxPanelDiffAbove.GotFocus
        LabelInstructions.Text = "This CONSTRAINT specifies a TARGET for the maximum panel difference for teams ABOVE the break point.  The ABOVE target applies no matter how " & _
        "many cohorts you use."
    End Sub
    Private Sub TextBoxMaxPanelDiffBREAK_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBoxMaxPanelDiffBREAK.GotFocus
        LabelInstructions.Text = "This CONSTRAINT specifies a TARGET for the maximum panel difference for teams in BREAK rounds.  The BREAK target only applies if you select " & _
        "3 cohorts"
    End Sub
    Private Sub TextBoxMaxPanelDiffBelow_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBoxMaxPanelDiffBelow.GotFocus
        LabelInstructions.Text = "This CONSTRAINT specifies a TARGET for the maximum panel difference for teams BELOW the break point.  The BELOW target applies if you select " & _
        "2 or 3 cohorts."
    End Sub

    'Procedures to control BUTTONS
    Private Sub ButtonAssignJudges_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAssignJudges.Click
        ButtonAssignJudges.BackColor = Color.Red
        ButtonAssignJudges.Refresh()
        'ConnectToXMLData()
        AssignNewJudgePanels()
        ButtonAssignJudges.BackColor = System.Drawing.Color.Cornsilk
    End Sub

    Private Sub ButtonRemoveJudges_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRemoveJudges.Click

        'ADD DIALOG BOX TO WARN OF CONSEQUENCES
        Dim Q As Integer = MsgBox("This will delete all existing judge assignments for this round.  If the pairings have already gone out or if you have already entered results for this round, you probably DO NOT want to do this.  Continue?", MsgBoxStyle.YesNo)
        If Q = vbNo Then Exit Sub
        ButtonRemoveJudges.BackColor = Color.Red
        ButtonRemoveJudges.Refresh()
        'Delete Judges from pairing / division combination
        For PairingCounter = 1 To PairingCount
            For PanelCounter = 1 To PanelSize
                Dim dr As DataRow
                dr = ds.Tables("BALLOT").Rows.Find(PairingIndex(PairingCounter, 0, PanelCounter))
                dr.Item("JUDGE") = -99
                dr = ds.Tables("BALLOT").Rows.Find(PairingIndex(PairingCounter, 1, PanelCounter))
                dr.Item("JUDGE") = -99
            Next
        Next
        ConnectToXMLData()
        ButtonRemoveJudges.BackColor = System.Drawing.Color.LightPink
    End Sub

    Private Sub ButtonRecordJudges_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRecordJudges.Click

        'ADD DIALOG BOX TO WARN OF CONSEQUENCES
        ButtonRecordJudges.BackColor = Color.Red
        ButtonRecordJudges.Refresh()

        For PairingCounter = 1 To PairingCount
            For PanelCounter = 1 To PanelSize

                Dim dr As DataRow
                dr = ds.Tables("BALLOT").Rows.Find(PairingIndex(PairingCounter, 0, PanelCounter))
                dr.Item("JUDGE") = AssignedJudges(PairingCounter, PanelCounter)
                dr = ds.Tables("BALLOT").Rows.Find(PairingIndex(PairingCounter, 1, PanelCounter))
                dr.Item("JUDGE") = AssignedJudges(PairingCounter, PanelCounter)

            Next
        Next

        ConnectToXMLData()
        ButtonRecordJudges.BackColor = System.Drawing.Color.LightGreen
        dgvDTTab.DataSource = Nothing
        dgFinishStats.DataSource = Nothing

    End Sub

    'Procedures to Update Parameters from TextBoxes and CheckBoxes
    Private Sub TextBoxPrefWeight_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBoxIndivPrefWeight.TextChanged

        IndivPrefWeight = Val(TextBoxIndivPrefWeight.Text)
        If EnableEvents = False Then Exit Sub
        ds.Tables("JUDGE_ASSIGN_PARAM").Rows(0).Item("INDIV_PREF_WEIGHT") = IndivPrefWeight

    End Sub

    Private Sub TextBoxIndivDiffWeight_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBoxIndivDiffWeight.TextChanged

        IndivDiffWeight = Val(TextBoxIndivDiffWeight.Text)
        If EnableEvents = False Then Exit Sub
        ds.Tables("JUDGE_ASSIGN_PARAM").Rows(0).Item("INDIV_DIFF_WEIGHT") = IndivDiffWeight

    End Sub

    Private Sub TextBoxPanelDiffWeight_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBoxPanelDiffWeight.TextChanged

        PanelDiffWeight = Val(TextBoxPanelDiffWeight.Text)
        If EnableEvents = False Then Exit Sub
        ds.Tables("JUDGE_ASSIGN_PARAM").Rows(0).Item("PANEL_DIFF_WEIGHT") = PanelDiffWeight

    End Sub

    Private Sub TextBoxJudgePrefWeight_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBoxJudgePrefWeight.TextChanged

        JudgePrefWeight = Val(TextBoxJudgePrefWeight.Text)
        If EnableEvents = False Then Exit Sub
        ds.Tables("JUDGE_ASSIGN_PARAM").Rows(0).Item("JUDGE_PREF_WEIGHT") = JudgePrefWeight

    End Sub

    Private Sub TextBoxNumCohorts_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBoxNumCohorts.TextChanged

        NumCohorts = Val(TextBoxNumCohorts.Text)
        If NumCohorts > 3 Then
            NumCohorts = 1
            TextBoxNumCohorts.Text = "1"
        End If
        If NumCohorts < 2 Then
            TextBoxBreakPoint.Visible = False
            LabBreakPoint.Visible = False
        Else
            TextBoxBreakPoint.Visible = True
            LabBreakPoint.Visible = True
        End If
        If NumCohorts = 3 Then
            TextBoxTargetBREAK.Visible = True
            TextBoxTargetBelow.Visible = True
            TextBoxMaxIndivDiffBREAK.Visible = True
            TextBoxMaxIndivDiffBelow.Visible = True
            If PanelSize > 1 Then
                TextBoxMaxPanelDiffBREAK.Visible = True
                TextBoxMaxPanelDiffBelow.Visible = True
            End If
        ElseIf NumCohorts = 2 Then
            TextBoxTargetBREAK.Visible = False
            TextBoxTargetBelow.Visible = True
            TextBoxMaxIndivDiffBREAK.Visible = False
            TextBoxMaxIndivDiffBelow.Visible = True
            If PanelSize > 1 Then
                TextBoxMaxPanelDiffBREAK.Visible = False
                TextBoxMaxPanelDiffBelow.Visible = True
            End If
        Else
            TextBoxTargetBREAK.Visible = False
            TextBoxTargetBelow.Visible = False
            TextBoxMaxIndivDiffBREAK.Visible = False
            TextBoxMaxIndivDiffBelow.Visible = False
            TextBoxMaxPanelDiffBREAK.Visible = False
            TextBoxMaxPanelDiffBelow.Visible = False
        End If
        If EnableEvents = False Then Exit Sub
        ds.Tables("JUDGE_ASSIGN_PARAM").Rows(0).Item("NUM_COHORTS") = NumCohorts

    End Sub

    Private Sub TextBoxBreakPoint_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBoxBreakPoint.TextChanged

        BreakPoint = Val(TextBoxBreakPoint.Text)
        If EnableEvents = False Then Exit Sub
        ds.Tables("JUDGE_ASSIGN_PARAM").Rows(0).Item("BREAKPOINT") = BreakPoint

    End Sub

    Private Sub TextBoxPanelSize_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBoxPanelSize.TextChanged

        PanelSize = Val(TextBoxPanelSize.Text)
        If PanelSize = 0 Then
            TextBoxPanelSize.Text = ""
            Exit Sub
        End If
        If PanelSize = 1 Then
            TextBoxPanelPrefWeight.Text = 0
            TextBoxPanelDiffWeight.Text = 0
            TextBoxPanelPrefWeight.Visible = False
            TextBoxPanelDiffWeight.Visible = False
            Label8.Visible = False
            Label24.Visible = False
            Label23.Visible = False
            TextBoxMaxPanelDiffAbove.Visible = False
            TextBoxMaxPanelDiffBREAK.Visible = False
            TextBoxMaxPanelDiffBelow.Visible = False
        Else
            TextBoxPanelPrefWeight.Text = 1
            TextBoxPanelDiffWeight.Text = 1
            TextBoxPanelPrefWeight.Visible = True
            TextBoxPanelDiffWeight.Visible = True
            Label8.Visible = True
            Label24.Visible = True
            Label23.Visible = True
            TextBoxMaxPanelDiffAbove.Visible = True
            TextBoxMaxPanelDiffBREAK.Visible = True
            TextBoxMaxPanelDiffBelow.Visible = True
        End If

        If EnableEvents = False Then Exit Sub

        Dim dr As DataRow
        dr = ds.Tables("Round").Rows.Find(cboRound.SelectedValue)
        dr.Item("JudgesPerPanel") = PanelSize


    End Sub

    Private Sub TextBoxMaxLostRounds_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBoxMaxLostRounds.TextChanged

        ExtraJudges = Val(TextBoxMaxLostRounds.Text)
        If EnableEvents = False Then Exit Sub
        ds.Tables("JUDGE_ASSIGN_PARAM").Rows(0).Item("MAX_LOST_ROUNDS") = ExtraJudges

    End Sub

    Private Sub TextBoxTargetAbove_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBoxTargetAbove.TextChanged

        PermMaxIndivPref(2) = Val(TextBoxTargetAbove.Text)
        If EnableEvents = False Then Exit Sub
        ds.Tables("JUDGE_ASSIGN_PARAM").Rows(0).Item("PREF_TARGET_ABOVE") = PermMaxIndivPref(2)

    End Sub

    Private Sub TextBoxTargetBREAK_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBoxTargetBREAK.TextChanged

        PermMaxIndivPref(1) = Val(TextBoxTargetBREAK.Text)
        If EnableEvents = False Then Exit Sub
        ds.Tables("JUDGE_ASSIGN_PARAM").Rows(0).Item("PREF_TARGET_BREAK") = PermMaxIndivPref(1)

    End Sub

    Private Sub TextBoxTargetBelow_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBoxTargetBelow.TextChanged

        PermMaxIndivPref(3) = Val(TextBoxTargetBelow.Text)
        If EnableEvents = False Then Exit Sub
        ds.Tables("JUDGE_ASSIGN_PARAM").Rows(0).Item("PREF_TARGET_BELOW") = PermMaxIndivPref(3)

    End Sub

    Private Sub TextBoxMaxIndivDiffAbove_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBoxMaxIndivDiffAbove.TextChanged

        PermMaxIndivDiff(2) = Val(TextBoxMaxIndivDiffAbove.Text)
        If EnableEvents = False Then Exit Sub
        ds.Tables("JUDGE_ASSIGN_PARAM").Rows(0).Item("MAX_INDIV_DIFF_ABOVE") = PermMaxIndivDiff(2)

    End Sub

    Private Sub TextBoxMaxIndivDiffBREAK_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBoxMaxIndivDiffBREAK.TextChanged

        PermMaxIndivDiff(1) = Val(TextBoxMaxIndivDiffBREAK.Text)
        If EnableEvents = False Then Exit Sub
        ds.Tables("JUDGE_ASSIGN_PARAM").Rows(0).Item("MAX_INDIV_DIFF_BREAK") = PermMaxIndivDiff(1)

    End Sub

    Private Sub TextBoxMaxIndivDiffBelow_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBoxMaxIndivDiffBelow.TextChanged

        PermMaxIndivDiff(3) = Val(TextBoxMaxIndivDiffBelow.Text)
        If EnableEvents = False Then Exit Sub
        ds.Tables("JUDGE_ASSIGN_PARAM").Rows(0).Item("MAX_INDIV_DIFF_BELOW") = PermMaxIndivDiff(3)

    End Sub

    Private Sub TextBoxMaxPanelDiffAbove_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBoxMaxPanelDiffAbove.TextChanged

        PermMaxPanelDiff(2) = Val(TextBoxMaxPanelDiffAbove.Text)
        If EnableEvents = False Then Exit Sub
        ds.Tables("JUDGE_ASSIGN_PARAM").Rows(0).Item("MAX_PANEL_DIFF_ABOVE") = PermMaxPanelDiff(2)

    End Sub

    Private Sub TextBoxMaxPanelDiffBREAK_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBoxMaxPanelDiffBREAK.TextChanged

        PermMaxPanelDiff(1) = Val(TextBoxMaxPanelDiffBREAK.Text)
        If EnableEvents = False Then Exit Sub
        ds.Tables("JUDGE_ASSIGN_PARAM").Rows(0).Item("MAX_PANEL_DIFF_BREAK") = PermMaxPanelDiff(1)

    End Sub

    Private Sub TextBoxMaxPanelDiffBelow_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBoxMaxPanelDiffBelow.TextChanged

        PermMaxPanelDiff(3) = Val(TextBoxMaxPanelDiffBelow.Text)
        If EnableEvents = False Then Exit Sub
        ds.Tables("JUDGE_ASSIGN_PARAM").Rows(0).Item("MAX_PANEL_DIFF_BELOW") = PermMaxPanelDiff(3)

    End Sub

    Private Sub CheckBoxJudgeAgain_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxJudgeAgain.CheckedChanged

        JudgeAgain = CheckBoxJudgeAgain.Checked
        If EnableEvents = False Then Exit Sub
        ds.Tables("JUDGE_ASSIGN_PARAM").Rows(0).Item("JUDGE_AGAIN") = JudgeAgain

    End Sub

    Private Sub TextBoxPanelTotalWeight_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBoxPanelPrefWeight.TextChanged

        PanelPrefWeight = Val(TextBoxPanelPrefWeight.Text)
        If EnableEvents = False Then Exit Sub
        ds.Tables("JUDGE_ASSIGN_PARAM").Rows(0).Item("PANEL_PREF_WEIGHT") = PanelPrefWeight

    End Sub

    Private Sub ButtonMultiTest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonMultiTest.Click

        GroupBoxMultiTest.Visible = True
        Application.DoEvents()

        TextBoxMinIndivPrefWeight.Text = 0
        TextBoxMaxIndivPrefWeight.Text = 1

        TextBoxIncIndivPrefWeight.Text = 0.1
        TextBoxIncPanelPrefWeight.Text = 0.2
        TextBoxIncIndivDiffWeight.Text = 0.2
        TextBoxIncPanelDiffWeight.Text = 0.2

        If PanelSize = 1 Then
            TextBoxMinPanelPrefWeight.Text = 0
            TextBoxMaxPanelPrefWeight.Text = 0
            TextBoxMinINdivDiffWeight.Text = 0
            TextBoxMaxIndivDiffWeight.Text = 1
            TextBoxMinPanelDiffWeight.Text = 0
            TextBoxMaxPanelDiffWeight.Text = 0

            TextBoxMinPanelPrefWeight.Visible = False
            TextBoxMaxPanelPrefWeight.Visible = False
            TextBoxIncPanelPrefWeight.Visible = False
            TextBoxMinPanelDiffWeight.Visible = False
            TextBoxMaxPanelDiffWeight.Visible = False
            TextBoxIncPanelDiffWeight.Visible = False
            Label35.Visible = False
            Label36.Visible = False
        Else
            TextBoxMinPanelPrefWeight.Text = 0
            TextBoxMaxPanelPrefWeight.Text = 1
            TextBoxMinINdivDiffWeight.Text = 0
            TextBoxMaxIndivDiffWeight.Text = 1
            TextBoxMinPanelDiffWeight.Text = 1
            TextBoxMaxPanelDiffWeight.Text = 1
        End If

        TextBoxMaxJudgePrefWeight.Text = LabJudgePrefBefore.Text
        Application.DoEvents()

    End Sub

    Private Sub ButtonMultiTestBegin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonMultiTestBegin.Click

        ButtonMultiTestBegin.BackColor = Color.Red
        ButtonMultiTestBegin.Refresh()

        'Multitest()

        TextBoxIndivPrefWeight.Text = LabelMultiIndivPrefWeight.Text
        TextBoxPanelPrefWeight.Text = LabelMultiPanelPrefWeight.Text
        TextBoxIndivDiffWeight.Text = LabelMultiIndivDiffWeight.Text
        TextBoxPanelDiffWeight.Text = LabelMultiPanelDiffWeight.Text
        TextBoxJudgePrefWeight.Text = LabelMultiJudgePrefWeight.Text
        ButtonMultiTestBegin.BackColor = Color.Cornsilk
        GroupBoxMultiTest.Visible = False
        Application.DoEvents()

        ConnectToXMLData()
        AssignNewJudgePanels()

    End Sub

    Private Sub cboRound_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboRound.SelectedIndexChanged

        If EnableEvents = False Then Exit Sub
        ConnectToXMLData()

        dgvDTTab.DataSource = Nothing
        dgFinishStats.DataSource = Nothing

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Call ShowJudgeSituation()

        'show breakdown
        dtRound = MakePairingTable(ds, cboRound.SelectedValue, "Code")
        Dim dt2 As New DataTable
        dt2 = OrdReport(ds, dtRound, Val(TextBoxMaxIndivDiffAbove.Text))
        dgvDTTab.DataSource = dt2

    End Sub
    Sub ShowJudgeSituation()
        'ugly, but functional reporting of judge situation
        ds.Tables("Judge").Columns.Add("Already", System.Type.GetType("System.Int64"))
        For x = 0 To ds.Tables("Judge").Rows.Count - 1
            ds.Tables("Judge").Rows(x).Item("Already") = GetRoundsJudged(ds, ds.Tables("Judge").Rows(x).Item("ID"))
        Next x

        Dim dt As New DataTable
        dt = JudgeSituation(ds)
        dgFinishStats.DataSource = dt

        If ds.Tables("Judge").Columns.Contains("Already") = True Then
            ds.Tables("Judge").Columns.Remove("Already")
        End If
    End Sub

    Private Sub butBasicInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butBasicInfo.Click
        Dim strInfo As String = ""
        strInfo &= "Generally, you enter values in the white boxes and the computer displays values in the green and beige boxes.  Clicking any white box will display help in the middle-left." & Chr(10) & Chr(10)
        strInfo &= "STEP ONE: Select a round using the drop-down list in the top-left." & Chr(10) & Chr(10)
        strInfo &= "STEP TWO: Make sure all the settings in the top-left portion of the screen are set correctly (especially PANEL SIZE and whether judges can hear a team more than once)." & Chr(10) & Chr(10)
        strInfo &= "COHORTS: A cohort is a group of teams; there are three of them.  (a) Teams over the break line (b) teams at the break line (c) teams below the break line - out of it.  If 3 losses disqualifies you from advancing to elim rounds, all teams are above the break line through round 2, and there is only 1 cohort.  During round 3, there are 2 cohorts, those teams above the breakline with fewer than 2 losses, and those teams at the breakline with 2 losses (1 more loss gives them 3, and they can't clear).  During round 4, there are 3 cohorts: Those with fewer than 2 losses, those with 2 losses who are at the breakline, and those who already have 3 losses and will not clear.  Once you enter the number of cohorts, a box will let you specify the number of losses you can have and still clear." & Chr(10) & Chr(10)
        strInfo &= "STEP THREE: Set the WEIGHTS for preference, mutuality, and remaining judge pref in the vertical column at the middle of the screen.  Clicking any box will reveal help that identifies good default values.  Generally, only advanced users will want to change the defaults for mutuality and pref. " & Chr(10) & Chr(10)
        strInfo &= "STEP FOUR, REMAINING JUDGE PREF (the last weight in the bottom-middle): Please read the help before using this button.  It is the most important and sensitive setting on the page.  It is sensitive to chanegs of .1 -- so changing 2.1 to 2.2 might have a significant effect." & Chr(10) & Chr(10)
        strInfo &= "STEP FIVE, SET THE MAX TARGETS: Just below the green and beige boxes toward the middle-right are white boxes where you can set maximum values for preference and mutuality.  For example, entering a 50 means the computer will avoid placing a judge with an ordinal percentile lower than 50 for either team." & Chr(10) & Chr(10)
        strInfo &= "STEP SIX, ROUNDS TO LOSE: Click the 'Show Saved Assignments Diagnostic' button in the top-right.  The bottom box will show you how many rounds you have to lose for the rest of the tournament in the BALANCE column.  For example, if you still have 3 rounds to complete and you have a balance of 12, you can afford to lose 4 rounds of judging per debate.  Enter the target figure in the lower-right in the white 'max lost' box." & Chr(10) & Chr(10)
        strInfo &= "STEP SEVEN, PLACE AND MASSAGE: In the lower-left click on the beige CREATE JUDGE PANELS button.  The diagnostics for the proposed panels will appear in the beige boxes on the middle-right.  At this point, you might want to try a few different options with slightly different values in the REMAINING JUDGE PREF box." & Chr(10) & Chr(10)
        strInfo &= "THE MOST IMPORTANT THING: Look at the salmon-colored PREF BEFORE box and the beige PREF AFTER box.  If the PREF AFTER score is LOWER, that means the quality of the remaining pool is as good or better than it was before the pairing.  If it's higher, you are losing quality judging." & Chr(10) & Chr(10)
        strInfo &= "When you are ready to save, click the green RECORD JUDGE PANELS in the lower-left.  If you want to delete all the judge placements, click on the salmon REMOVE JUDGE PANELS button." & Chr(10) & Chr(10)
        strInfo &= "After you have saved, you can again click the 'Show Saved Assignments Diagnostic' button for additional information on judge placement and how many rounds of judging you have left." & Chr(10) & Chr(10)
        MsgBox(strInfo)
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim strInfo As String = ""
        strInfo &= "Unless you are very fortunate, most tournaments are fairly tight on judges, and if you are using mutual preference this will be made more difficult by hard-to-place judges." & Chr(10) & Chr(10)
        strInfo &= "All judge placement, therefore, involves balancing the desire to place highly-preferred judges against the risk that you might burn up all your good judging in early rounds and have much worse placements later in the tournament.  The worst thing that can happen is that you run out of rounds of judging and can't finish the tournament." & Chr(10) & Chr(10)
        strInfo &= "A 'good' judge placement involves both high preference and high mutuality.  Preference is simply the rating (converted to an ordinal percentile--see the pref entry page), and lower is better.  A 15% placement means a judge in the top 15% of the team's ratings." & Chr(10) & Chr(10)
        strInfo &= "Mutuality is the difference between the ratings 2 teams give the judge.  If a judge is a 45% for one team and a 35% for another, the mutuality is 10." & Chr(10) & Chr(10)
        strInfo &= "This screen lets you set preference and mutuality targets and assign weightings to which factors are most important to try to maximize the balance between high quality judging and preserving the pool. " & Chr(10) & Chr(10)
        strInfo &= "If you have a reasonably large tournament (40 teams or more) and enough flexibility in the pool to lose 2-3 rounds of judging per round, you can expect the average preference to be in the 20s and the average mutuality to be at or below 10." & Chr(10) & Chr(10)
        strInfo &= "The targets you set are maximum, outer values, so even if you set the maximum preference at 50 you will probably average preference in the 20s." & Chr(10) & Chr(10)
        strInfo &= "" & Chr(10) & Chr(10)
        MsgBox(strInfo)
    End Sub
End Class

