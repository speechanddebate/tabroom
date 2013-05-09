Module modPairing
    Function PullBallotsByRound(ByVal ds2 As DataSet, ByVal strField As String, ByVal FieldId As Integer, ByVal round As Integer) As DataTable
        'returns a table that holds all ballots for a given judge or team in a given round
        'DT will only include a list of ballotIDS 
        Dim DT As New DataTable
        Dim dr As DataRow
        DT.Columns.Add("ID", System.Type.GetType("System.Int64"))
        'pull all ballots by judge or team for a given round; strfield can = "Judge" or "Team"
        Dim fdBallots As DataRow()
        fdBallots = ds2.Tables("Ballot").Select(strField & "=" & FieldId)
        Dim drPanel As DataRow
        For x = 0 To fdBallots.Length - 1
            drPanel = ds2.Tables("Panel").Rows.Find(fdBallots(x).Item("Panel"))
            If drPanel.Item("Round") = round Then
                dr = DT.NewRow
                dr.Item("ID") = fdBallots(x).Item("ID")
                DT.Rows.Add(dr)
            End If
        Next x
        Return DT
    End Function
    Function PullAllBallotsInRound(ByVal ds2 As DataSet, ByVal round As Integer, ByVal strSORT As String) As DataRow()
        Dim fdPanel, fdballot As DataRow()
        fdPanel = ds2.Tables("Panel").Select("Round=" & round)
        Dim strPanel As String = ""
        For x = 0 To fdPanel.Length - 1
            If strPanel <> "" Then strPanel &= " or "
            strPanel &= "Panel=" & fdPanel(x).Item("ID")
        Next x
        fdballot = ds2.Tables("Ballot").Select(strPanel, strSORT)
        Return fdballot
    End Function
    Function AddRoomToPanel(ByRef ds As DataSet, ByVal panel As Integer, ByVal Room As Integer) As String
        AddRoomToPanel = "OK"
        Dim fdPanel As DataRow
        fdPanel = ds.Tables("Panel").Rows.Find(panel)
        If fdPanel Is Nothing Then AddRoomToPanel = "Invalid panel selected.  Please select an appropriate panel and try again." : Exit Function
        fdPanel.Item("Room") = Room
    End Function
    Function AddJudgeToPanel(ByRef DS As DataSet, ByVal panel As Integer, ByVal judge As Integer, ByVal Flight As Integer) As String
        AddJudgeToPanel = "OK"
        'pull all judge ballots by panel where the judge field is blank; order by teams in ascending order
        Dim fdBallots As DataRow()
        fdBallots = DS.Tables("Ballot").Select("Panel=" & panel & " and judge=-99", "Entry DESC")
        If fdBallots.Length = 0 Then AddJudgeToPanel = "No blank place to insert the judge.  Please delete a team from this pairing first and try again." : Exit Function
        If fdBallots(0).Item("Entry") = -99 Then AddJudgeToPanel = "Please place at least one team before adding judges." : Exit Function
        'Add a judge for each unique team entry
        For x = 0 To fdBallots.Length - 1
            If x = 0 Then
                fdBallots(x).Item("Judge") = judge
            ElseIf fdBallots(x).Item("Entry") <> fdBallots(x - 1).Item("Entry") Then
                fdBallots(x).Item("Judge") = judge
            End If
        Next x
        'mark the flight
        Dim fdPanel As DataRow : fdPanel = DS.Tables("Panel").Rows.Find(panel)
        fdPanel.Item("Flight") = Flight
    End Function
    Function AssignBye(ByRef ds As DataSet, ByVal panel As Integer, ByVal team As Integer, ByVal Round As Integer) As String
        'NOTE: this is ONLY for assigning a single team a bye.  To enter a bye as a result for a round, use the ballot
        'entry screen
        AssignBye = ""
        'create a new panel if there isn't one
        If panel = -1 Then panel = AddPanel(ds, Round, 0)
        'pull all ballots and make sure that they're blank.
        Dim fdBallots As DataRow()
        fdBallots = ds.Tables("Ballot").Select("Panel=" & panel)
        If fdBallots.Length = 0 Then
            AssignBye = "Can't assign a bye on this panel; the panel has no ballots associated with it."
            Exit Function
        End If
        For x = 0 To fdBallots.Length - 1
            If fdBallots(x).Item("Judge") <> -99 And fdBallots(x).Item("Judge") <> -1 Then
                AssignBye = "Can't assign a bye on this panel; there is a judge assigned to it."
            End If
            If fdBallots(x).Item("Entry") <> -99 Then
                AssignBye = "Can't assign a bye on this panel; there are already other teams assigned to it."
            End If
        Next x
        If AssignBye <> "" Then Exit Function
        'add the team to the first ballot, and set the SIDE and JUDGE as -1
        fdBallots(0).Item("Entry") = team
        fdBallots(0).Item("Side") = 1
        For x = 0 To fdBallots.Length - 1
            fdBallots(x).Item("Judge") = -1
        Next
        'Make sure there are ballot_scores, and assign the team a win and averaged points/ranks
        Call ValidateScoresByBallot(ds, fdBallots(0).Item("ID"))
        Dim fdScores As DataRow()
        fdScores = ds.Tables("Ballot_Score").Select("Ballot=" & fdBallots(0).Item("ID"))
        For x = 0 To fdScores.Length - 1
            If fdScores(x).Item("Score_ID") = 1 Then
                fdScores(x).Item("Score") = 1
            Else
                fdScores(x).Item("Score") = -1
            End If
        Next
    End Function
    Function AddTeamToPanel(ByRef DS As DataSet, ByVal panel As Integer, ByVal team As Integer, ByVal Side As Integer) As String
        AddTeamToPanel = "OK"
        'pull all judge ballots by panel where the team field is blank; order by judges in ascending order
        Dim fdBallots As DataRow()
        fdBallots = DS.Tables("Ballot").Select("Panel=" & panel & " and entry=-99", "Judge ASC")
        If fdBallots.Length = 0 Then AddTeamToPanel = "No blank place to insert the team.  Please delete an existing team from the pairing and try again." : Exit Function
        'get the number of judges that need to hear the team
        Dim drPanel, drRound As DataRow
        drPanel = DS.Tables("Panel").Rows.Find(panel)
        drRound = DS.Tables("Round").Rows.Find(drPanel.Item("Round"))
        Dim nJudges = drRound.Item("JudgesPerPanel")
        If fdBallots.Length < nJudges Then
            AddTeamToPanel = "Not enough blank spots to place the team.  Try again." : Exit Function
        End If
        Dim ctr, x As Integer
        'Add a team for each unique judge entry
        For x = 0 To fdBallots.Length - 1
            'Not sure if the parenthetical OR SIDE=-1 will mess everything up; added it to allow TRPC import
            If (fdBallots(x).Item("Side") = Side Or (Side = -1 And ctr = 0)) Then fdBallots(x).Item("entry") = team : ctr += 1
        Next x
        'If you're not on enough ballots, then add in blank judge spots
        x = -1
        'in case of bye
        If Side < 1 Then Side = 1
        Do Until ctr = nJudges
            x = x + 1
            If fdBallots(x).Item("Judge") = -99 And fdBallots(x).Item("Entry") = -99 And fdBallots(x).Item("side") = Side Then
                fdBallots(x).Item("Entry") = team : ctr += 1
            End If
        Loop
    End Function
    Function AddPanel(ByRef ds As DataSet, ByVal round As Int64, ByVal FixedPanel As Integer) As Integer
        'create a blank panel; enters -99 for teams and judges.  Returns the panelID number
        'FixedPanel is a number to set the panel to; if the autonumber function is turned off it can be inserted
        'add the panel
        Dim PanelID As Integer
        Dim dr As DataRow
        dr = ds.Tables("PANEL").NewRow
        dr.Item("Round") = round
        'If ds.Tables("Panel").Columns("ID").AutoIncrement = False Then
        If FixedPanel > 0 Then
            dr.Item("ID") = FixedPanel
        End If
        ds.Tables("Panel").Rows.Add(dr)
        ds.Tables("Panel").AcceptChanges()
        PanelID = dr.Item("ID")
        'pull the number of teams per debate; 
        Dim drRound As DataRow
        drRound = ds.Tables("Round").Rows.Find(round)
        Dim TeamsPerDebate As Integer = getEventSetting(ds, drRound.Item("Event"), "TeamsPerRound")
        'Not doing line below -- will add judges, just only take a ballot for the first
        'If getEventSetting(ds, drRound.Item("Event"), "PanelDecisions") = True Then drRound.Item("JudgesPerPanel") = 1
        'add a ballot for each team on panel by each judge
        Dim x, y As Integer
        For y = 0 To drRound.Item("JudgesPerPanel") - 1
            For x = 0 To TeamsPerDebate - 1
                dr = ds.Tables("Ballot").NewRow
                dr.Item("Panel") = PanelID
                dr.Item("Judge") = -99
                dr.Item("Entry") = -99
                dr.Item("Side") = x + 1
                ds.Tables("Ballot").Rows.Add(dr)
            Next x
        Next y
        Return PanelID
    End Function
    Function ActiveInTimeSlot(ByVal ds As DataSet, ByVal ID As Integer, ByVal round As Integer, ByVal strField As String) As Boolean
        'tells whether a team or judge is in used during current timeblock
        ActiveInTimeSlot = False
        'pull all ballots by judge or team for a given round; strfield can = "Judge" or "Entry"
        Dim fdBallots As DataRow()
        fdBallots = ds.Tables("Ballot").Select(strField & "=" & ID)
        'pull the panel for each ballot, find the round based on the panel, and then retrieve all rounds in the same
        'timeslot
        Dim drPanel As DataRow
        Dim drRound As DataRow
        drRound = ds.Tables("Round").Rows.Find(round)
        Dim RoundsInTimeBlock As DataRow()
        RoundsInTimeBlock = ds.Tables("Round").Select("TimeSlot=" & drRound.Item("TimeSlot"))
        'scroll through each ballot, and see if there is one in a round during the current timeslot
        For x = 0 To fdBallots.Length - 1
            drPanel = ds.Tables("Panel").Rows.Find(fdBallots(x).Item("Panel"))
            For y = 0 To RoundsInTimeBlock.Length - 1
                If drPanel.Item("Round") = RoundsInTimeBlock(y).Item("ID") Then ActiveInTimeSlot = True : Exit Function
            Next y
        Next x
    End Function
    Function RoomActiveInTimeSlot(ByVal ds As DataSet, ByVal room As Integer, ByVal round As Integer) As Boolean
        If room <= 0 Then RoomActiveInTimeSlot = True : Exit Function
        'first test availabilty in timeslot
        Dim drRd, drRoom As DataRow
        drRd = ds.Tables("Round").Rows.Find(round)
        drRoom = ds.Tables("Room").Rows.Find(room)
        If drRoom.Item("Timeslot" & drRd.Item("Timeslot").ToString.Trim) Is Nothing Then drRoom.Item("Timeslot" & drRd.Item("Timeslot").ToString.Trim) = False
        If drRoom.Item("Timeslot" & drRd.Item("Timeslot").ToString.Trim) = False Then
            RoomActiveInTimeSlot = True 'mark as active so won't be placed
            Exit Function
        End If
        'now test in use
        RoomActiveInTimeSlot = False
        'pull all panels in the timeslot for the specified round that have the room
        Dim fdPanels As DataRow()
        Dim fdRd As DataRow : fdRd = ds.Tables("Round").Rows.Find(round)
        Dim fdRds As DataRow() : fdRds = ds.Tables("Round").Select("timeslot=" & fdRd.Item("Timeslot"))
        'identify all rounds in the timeslot
        Dim strDummy As String = ""
        For x = 0 To fdRds.Length - 1
            If strDummy <> "" Then strDummy &= " or "
            strDummy &= "Round=" & fdRds(x).Item("ID")
        Next x
        'now pull the panels in the timeslot with that room
        If ds.Tables("Panel").Columns("Room").DataType.ToString.ToUpper = "SYSTEM.STRING" Then
            strDummy = "(" & strDummy & ") and room='" & room & "'"
        Else
            strDummy = "(" & strDummy & ") and room=" & room
        End If
        fdPanels = ds.Tables("Panel").Select(strDummy)
        If fdPanels.Length > 0 Then RoomActiveInTimeSlot = True
    End Function
    Function SameSchool(ByVal ds As DataSet, ByVal team1 As Integer, ByVal team2 As Integer) As Boolean
        SameSchool = False
        Dim x, y As Integer
        'compare all speakers
        Dim speakers1 As DataRow()
        speakers1 = ds.Tables("Entry_student").Select("Entry=" & team1)
        Dim speakers2 As DataRow()
        speakers2 = ds.Tables("Entry_student").Select("Entry=" & team2)
        For x = 0 To speakers1.Length - 1
            For y = 0 To speakers2.Length - 1
                If speakers1(x).Item("School") = speakers2(y).Item("School") Then SameSchool = True
            Next
        Next x
        'compare teams
        Dim teams As DataRow()
        teams = ds.Tables("Entry").Select("ID=" & team1 & " or ID=" & team2)
        If teams.Length = 1 Then SameSchool = True : Exit Function
        If teams(0).Item("School") = teams(1).Item("school") Then SameSchool = True
    End Function
    Function HitBefore(ByVal ds As DataSet, ByVal team1 As Integer, ByVal team2 As Integer, ByVal panel As Integer) As Boolean
        'returns true if 2 teams have ever been on the same a panel before
        HitBefore = False
        Dim x, y As Integer
        Dim fdTm1, fdTm2 As DataRow()
        fdTm1 = ds.Tables("Ballot").Select("Entry=" & team1)
        fdTm2 = ds.Tables("Ballot").Select("Entry=" & team2)
        For x = 0 To fdTm1.Length - 1
            If fdTm1(x).Item("Panel") <> panel Then
                For y = 0 To fdTm2.Length - 1
                    If fdTm1(x).Item("Panel") = fdTm2(y).Item("Panel") Then HitBefore = True : Exit Function
                Next y
            End If
        Next x
    End Function
    Function CanDebate(ByVal ds As DataSet, ByVal team1 As Integer, ByVal team2 As Integer) As String
        CanDebate = ""
        Dim strtime As String = ""
        strtime &= "START: " & Now.Second & " " & Now.Millisecond & Chr(10) & Chr(10)
        Dim dr As DataRow : dr = ds.Tables("Entry").Rows.Find(team1)
        If TeamBlock(ds, team1, team2) = True Then CanDebate = "CONFLICT! " & GetName(ds.Tables("Entry"), team1, "FullName") & " and " & GetName(ds.Tables("Entry"), team2, "FullName") & " are from the same school."
        If getEventSetting(ds, dr.Item("Event"), "AllowSameSchoolDebates") = False Then
            If SameSchool(ds, team1, team2) = True Then CanDebate = "CONFLICT! " & GetName(ds.Tables("Entry"), team1, "FullName") & " and " & GetName(ds.Tables("Entry"), team2, "FullName") & " are from the same school."
        End If
        strtime &= "Same school done: " & Now.Second & " " & Now.Millisecond & Chr(10) & Chr(10)
        If getEventSetting(ds, dr.Item("Event"), "Allow2dMeeting") = False Then
            'thids dodesn't account for the current round
            If HitBefore(ds, team1, team2, -1) = True Then CanDebate = "CONFLICT! " & GetName(ds.Tables("Entry"), team1, "FullName") & " have hit " & GetName(ds.Tables("Entry"), team2, "FullName").Trim & " before."
        End If
        strtime &= "Hit before done: " & Now.Second & " " & Now.Millisecond & Chr(10) & Chr(10)
        'MsgBox(strtime)
    End Function
    Function TeamBlock(ByVal ds As DataSet, ByVal team1 As Integer, ByVal team2 As Integer) As Boolean
        TeamBlock = False
        Dim x As Integer
        For x = 0 To ds.Tables("TeamBlock").Rows.Count - 1
            If ds.Tables("TeamBlock").Rows(x).Item("Team1") = team1 And ds.Tables("TeamBlock").Rows(x).Item("Team2") = team2 Then TeamBlock = True
            If ds.Tables("TeamBlock").Rows(x).Item("Team1") = team2 And ds.Tables("TeamBlock").Rows(x).Item("Team2") = team1 Then TeamBlock = True
        Next x
    End Function
    Function CanJudge(ByVal ds As DataSet, ByVal Judge As Integer, ByVal panel As Integer, ByVal BlockTwice As Boolean) As Boolean
        'The panel must include teams, and may include existing judge placements
        CanJudge = True
        'Is judge hearing a round already?
        Dim drPanel, drRound, drJudge As DataRow
        drPanel = ds.Tables("Panel").Rows.Find(panel)
        drRound = ds.Tables("Round").Rows.Find(drPanel.Item("Round"))
        drJudge = ds.Tables("Judge").Rows.Find(Judge)
        If ActiveInTimeSlot(ds, Judge, drRound.Item("ID"), "Judge") = True Then CanJudge = False : Exit Function
        'stop scheduled?
        If drJudge.Item("StopScheduling") = True Then CanJudge = False : Exit Function
        'is judge eligible for division?
        If drJudge.Item("Event" & drRound.Item("Event")) = False Then CanJudge = False : Exit Function
        'Is the judge from the same school as any team on the panel?
        If JudgeSameSchool(ds, Judge, panel) = True Then CanJudge = False : Exit Function
        'If judging twice isn't OK, have they judged any team on the panel before?
        If BlockTwice = True And JudgedBefore(ds, Judge, panel) = True Then CanJudge = False
    End Function
    Function JudgedBefore(ByVal ds As DataSet, ByVal Judge As Integer, ByVal panel As Integer) As Boolean
        'will return TRUE if they are scheduled to hear the team in the current debate
        JudgedBefore = False
        Dim fdBallot, drBefore As DataRow()
        fdBallot = ds.Tables("Ballot").Select("Panel=" & panel)
        Dim drTeam As DataRow
        For x = 0 To fdBallot.Length - 1
            If fdBallot(x).Item("Entry") <> -99 Then
                drTeam = ds.Tables("Entry").Rows.Find(fdBallot(x).Item("Entry"))
                drBefore = ds.Tables("Ballot").Select("Judge=" & Judge & " and Entry=" & drTeam.Item("ID") & " and panel<>" & panel)
                If drBefore.Length > 0 Then JudgedBefore = True : Exit Function
            End If
        Next x
    End Function
    Function JudgeSameSchool(ByVal ds As DataSet, ByVal Judge As Integer, ByVal panel As Integer) As Boolean
        JudgeSameSchool = False
        Dim fdBallot As DataRow()
        fdBallot = ds.Tables("Ballot").Select("Panel=" & panel)
        Dim drJudge, drTeam As DataRow
        drJudge = ds.Tables("Judge").Rows.Find(Judge)
        For x = 0 To fdBallot.Length - 1
            drTeam = ds.Tables("Entry").Rows.Find(fdBallot(x).Item("Entry"))
            If Not drTeam Is Nothing And Not drJudge Is Nothing Then
                If drTeam.Item("School") = drJudge.Item("School") Then JudgeSameSchool = True : Exit Function
            End If
        Next x
    End Function
    Function JudgeTeamSameSchool(ByVal ds As DataSet, ByVal Judge As Integer, ByVal team As Integer) As Boolean
        JudgeTeamSameSchool = False
        Dim drJudge, drTeam As DataRow
        Dim drDebaters As DataRow()
        drJudge = ds.Tables("Judge").Rows.Find(Judge)
        drTeam = ds.Tables("Entry").Rows.Find(team)
        drDebaters = ds.Tables("Entry_Student").Select("Entry=" & drTeam.Item("ID"))
        If drJudge.Item("School") = drTeam.Item("School") Then JudgeTeamSameSchool = True : Exit Function
        For x = 0 To drDebaters.Length - 1
            If drDebaters(x).Item("School") = drJudge.Item("School") Then JudgeTeamSameSchool = True : Exit Function
        Next x
    End Function
    Sub modShowThePairing(ByVal ds As DataSet, ByRef DataGridView1 As DataGridView, ByVal round As Integer, ByVal strTeamNameFormat As String)
        'make the results table
        Dim dt As DataTable
        dt = MakePairingTable(ds, round, strTeamNameFormat)
        Dim foundrow As DataRow
        dt.DefaultView.Sort = "Judge1 ASC"
        'show only name columns, hide the rest
        DataGridView1.DataSource = dt
        For x = DataGridView1.ColumnCount - 1 To 0 Step -1
            If InStr(DataGridView1.Columns(x).Name.ToUpper, "NAME") = 0 Or InStr(DataGridView1.Columns(x).Name.ToUpper, "SPKR") > 0 Then DataGridView1.Columns(x).Visible = False
            If InStr(DataGridView1.Columns(x).Name.ToUpper, "TEAMNAME") > 0 Then
                Dim side As Integer = Val(Mid(DataGridView1.Columns(x).Name, DataGridView1.Columns(x).Name.Length, 1))
                foundrow = ds.Tables("Round").Rows.Find(round)
                DataGridView1.Columns(x).HeaderText = GetSideString(ds, side, foundrow.Item("Event"))
            End If
        Next
    End Sub
    Function SideCheck(ByVal ds As DataSet, ByVal team As Integer, ByVal side As Integer, ByVal round As Integer, ByVal SidesSoFar As String) As String
        SideCheck = ""
        'get values you'll need
        Dim drRound As DataRow : drRound = ds.Tables("Round").Rows.Find(round)
        Dim nPrelims As Integer = getEventSetting(ds, drRound.Item("Event"), "nPrelims")
        Dim nSides As Integer
        If getEventSetting(ds, drRound.Item("Event"), "SideDesignations") = "Aff/Neg" Then nSides = 2
        If getEventSetting(ds, drRound.Item("Event"), "SideDesignations") = "Gov/Opp" Then nSides = 2
        If getEventSetting(ds, drRound.Item("Event"), "SideDesignations") = "WUDC" Then nSides = 4
        Dim SidesByRd(SidesSoFar.Length) As Integer
        For x = 1 To SidesSoFar.Length
            SidesByRd(x) = Val(Mid(SidesSoFar, x, 1))
        Next x
        'UNIT comparison; if there are 2 side designations, for example, compare in 2-round units
        'Find the last unit to compare
        Dim st, ctr As Integer
        For x = 1 To nPrelims Step nSides
            If x <= SidesSoFar.Length Then st = x
        Next x
        'mark the sides they've already been in that unit
        Dim SideIn(nSides) As Integer
        For x = st To SidesSoFar.Length
            SideIn(SidesByRd(x)) += 1
        Next x
        ctr = 0
        If SideIn(side) > 0 Then
            SideCheck &= " WARNING: This team has already been " & GetSideString(ds, side, drRound.Item("Event")) & " since round " & st.ToString & ".  Typically, they would need to be"
            For x = 1 To nSides
                If ctr > 0 Then SideCheck &= " or"
                If SideIn(x) = 0 Then SideCheck &= " " & GetSideString(ds, x, drRound.Item("Event")) : ctr += 1
            Next x
            SideCheck &= " before they are " & GetSideString(ds, side, drRound.Item("Event")) & " again."
        End If
        'TOTAL COUNT comparison
        'reset and count total number of times on each side
        For x = 0 To nSides : SideIn(x) = 0 : Next x
        For x = 1 To SidesSoFar.Length : SideIn(SidesByRd(x)) += 1 : Next x
        'figure out the minimum number of times a team needs to be on each side, and count the remaining rounds
        Dim nTimes As Integer
        nTimes = Int(nPrelims / nPrelims)
        Dim RdsLeft As Integer = nPrelims - SidesSoFar.Length
        'show the message if aren't enough rounds left to get enough rounds on each side in
        For x = 1 To nSides
            If RdsLeft <= nTimes - SideIn(x) And side <> x Then
                SideCheck &= " WARNING: This team MUST be " & GetSideString(ds, x, drRound.Item("Event")) & " in this round to have enough " & GetSideString(ds, x, drRound.Item("Event")) & " rounds for the prelims."
            End If
        Next x
    End Function
    Public Function GetSideDue(ByVal ds As DataSet, ByVal round As Integer, ByVal team As Integer) As Integer
        GetSideDue = 0
        'pull side team was last round, and flip it
        'find all panels in round, pick the one that has the team in it and is the round before the current round
        Dim fdBallots As DataRow()
        Dim fdPanel As DataRow
        fdBallots = ds.Tables("Ballot").Select("Entry=" & team)
        For x = 0 To fdBallots.Length - 1
            fdPanel = ds.Tables("Panel").Rows.Find(fdBallots(x).Item("Panel"))
            If fdPanel.Item("Round") = GetPriorRound(ds, round) Then
                If fdBallots(x).Item("Side") = 1 Then GetSideDue = 2 : Exit Function
                If fdBallots(x).Item("Side") = 2 Then GetSideDue = 1 : Exit Function
            End If
        Next x
    End Function
    Function MakeRoomStatusTable(ByVal ds As DataSet) As DataTable
        'Shows prelim room status

        Dim dt As New DataTable
        dt.Columns.Add("Slot", System.Type.GetType("System.String"))
        dt.Columns.Add("Event", System.Type.GetType("System.String"))
        dt.Columns.Add("Need", System.Type.GetType("System.Int16"))
        dt.Columns.Add("Have", System.Type.GetType("System.Int16"))
        dt.Columns.Add("Balance", System.Type.GetType("System.Int16"))

        Dim dr As DataRow
        Dim ShowSlot As Boolean
        Dim fdRds, fdTeams, fdRooms As DataRow()
        Dim z, ctr As Integer
        For y = 0 To ds.Tables("Timeslot").Rows.Count - 1
            ShowSlot = False
            fdRds = ds.Tables("Round").Select("Timeslot=" & ds.Tables("Timeslot").Rows(y).Item("ID"))
            For x = 0 To fdRds.Length - 1
                If fdRds(x).Item("Rd_Name") <= 9 Then ShowSlot = True : Exit For
            Next
            If ShowSlot = True Then
                ctr = 0
                For x = 0 To ds.Tables("Event").Rows.Count - 1
                    dr = dt.NewRow
                    dr.Item("Slot") = ds.Tables("Timeslot").Rows(y).Item("TimeSlotName").trim
                    dr.Item("Event") = ds.Tables("Event").Rows(x).Item("Abbr")
                    fdTeams = ds.Tables("Entry").Select("Event=" & ds.Tables("event").Rows(x).Item("ID"))
                    dr.Item("Need") = Int(fdTeams.Length / getEventSetting(ds, ds.Tables("event").Rows(x).Item("ID"), "TeamsPerRound"))
                    dr.Item("Have") = 0
                    Try
                        fdRooms = ds.Tables("Room").Select("Inactive=false and " & "TimeSlot" & ds.Tables("TimeSlot").Rows(y).Item("ID") & "=true and " & "Event" & ds.Tables("Event").Rows(x).Item("ID") & "=true")
                    Catch
                        fdRooms = ds.Tables("Room").Select("Inactive=false and " & "TimeSlot" & ds.Tables("TimeSlot").Rows(y).Item("ID") & "='true' and " & "Event" & ds.Tables("Event").Rows(x).Item("ID") & "='true'")
                    End Try
                    dr.Item("Have") = fdRooms.Length
                    dr.Item("Balance") = dr.Item("Have") - dr.Item("Need")
                    dt.Rows.Add(dr)
                    ctr += dr.Item("Need")
                Next x
                'add totals row
                dr = dt.NewRow
                dr.Item("Slot") = ds.Tables("Timeslot").Rows(y).Item("TimeSlotName").trim
                dr.Item("Event") = "ALL"
                dr.Item("Need") = ctr
                dr.Item("Have") = 0
                For z = 0 To ds.Tables("Room").Rows.Count - 1
                    If ds.Tables("Room").Rows(z).Item("TimeSlot" & ds.Tables("TimeSlot").Rows(y).Item("ID")) Is System.DBNull.Value Then ds.Tables("Room").Rows(z).Item("TimeSlot" & ds.Tables("TimeSlot").Rows(y).Item("ID")) = False
                    Try
                        If ds.Tables("Room").Rows(z).Item("Inactive") <> True And ds.Tables("Room").Rows(z).Item("TimeSlot" & ds.Tables("TimeSlot").Rows(y).Item("ID")) = True Then dr.Item("Have") += 1
                    Catch
                        If ds.Tables("Room").Rows(z).Item("Inactive").ToString.ToUpper <> "TRUE" And ds.Tables("Room").Rows(z).Item("TimeSlot" & ds.Tables("TimeSlot").Rows(y).Item("ID")).ToString.ToUpper = "TRUE" Then dr.Item("Have") += 1
                    End Try
                Next z
                dr.Item("Balance") = dr.Item("Have") - dr.Item("Need")
                dt.Rows.Add(dr)
            End If
        Next y

        Return dt

    End Function
    Sub AddSideReport(ByRef dt As DataTable, ByVal ds As DataSet)
        Dim strDummy As String
        Dim fdBallot As DataRow()
        Dim drPanel, drRound As DataRow
        For x = 0 To dt.Rows.Count - 1
            fdBallot = ds.Tables("Ballot").Select("Entry=" & dt.Rows(x).Item("Competitor"), "Panel ASC")
            strDummy = ""
            For y = 0 To fdBallot.Length - 1
                drPanel = ds.Tables("Panel").Rows.Find(fdBallot(y).Item("Panel"))
                drRound = ds.Tables("Round").Rows.Find(drPanel.Item("Round"))
                If drRound.Item("RD_NAME") <= 9 Then
                    If y = 0 Then
                        If fdBallot(y).Item("Judge") = -1 Then
                            strDummy = "B"
                        Else
                            strDummy = fdBallot(y).Item("Side")
                        End If
                    ElseIf fdBallot(y).Item("Panel") <> fdBallot(y - 1).Item("Panel") Then
                        If fdBallot(y).Item("Judge") = -1 Then
                            strDummy &= "B"
                        Else
                            strDummy &= fdBallot(y).Item("Side")
                        End If
                    End If
                End If
            Next y
            dt.Rows(x).Item("Sides") = strDummy
        Next x
    End Sub
    Sub AutoRoomsByBracket(ByRef ds As DataSet, ByVal Round As Integer, ByVal HonorDisability As Boolean, ByVal HonorTubUse As Boolean, ByVal byBracket As Boolean, ByVal SideToStay As Integer)
        'if SideToStay=0 then skip minimize room moves and do by bracket order.
        'if sidetostay=3 then either team can stay
        'load pairings table, load seeds table, and match them up
        Dim dtPairings, dtSeeds As DataTable
        dtPairings = MakePairingTable(ds, Round, "Code")
        Dim drRd As DataRow : drRd = ds.Tables("Round").Rows.Find(Round)
        'seed based on current round unless an elim, then use elim seeds
        dtPairings.Columns.Add("Seed", System.Type.GetType("System.Int64"))
        If drRd.Item("Rd_Name") <= 9 Then
            dtSeeds = MakeTBTable(ds, Round, "TEAM", "Code", -1, Round)
            Call AddSeedToResults(ds, dtPairings, dtSeeds, Round)
        Else
            Call AddElimSeedToResults(ds, dtPairings, Round)
        End If
        'strip byes
        For x = dtPairings.Rows.Count - 1 To 0 Step -1
            If dtPairings.Rows(x).Item("Judge1") = -1 Then dtPairings.Rows(x).Delete()
        Next x
        dtPairings.AcceptChanges()
        'now process; 3 functions place rooms; roomsforspecteams, roomsbyside, filloutwithbestrooms
        'this figures out which to call
        Dim drRound As DataRow
        drRound = ds.Tables("Round").Rows.Find(Round)
        Dim TeamsPerRound As Integer = getEventSetting(ds, drRound.Item("Event"), "TeamsPerRound")
        If HonorDisability = True Then Call RoomsForSpecTeams(ds, dtPairings, Round, "ADA")
        If HonorTubUse = True Then Call RoomsForSpecTeams(ds, dtPairings, Round, "TUBDISABILITY")
        If SideToStay > 0 Then Call RoomsBySide(ds, dtPairings, Round, SideToStay)
        'Fill out the rest with best rooms; reset seeds if not a criteria
        If byBracket = False Then
            For x = 0 To dtPairings.Rows.Count - 1
                dtPairings.Rows(x).Item("Seed") = Int(Rnd() * 1000)
            Next
        End If
        Call FillOutWithBestRooms(ds, dtPairings, Round)
    End Sub
    Sub RoomsBySide(ByRef ds As DataSet, ByRef dtPairings As DataTable, ByVal round As Integer, ByVal Side As Integer)
        Dim SideToUse, nTeams, room As Integer
        Dim drRound, drPanel, drRoom As DataRow
        drRound = ds.Tables("Round").Rows.Find(round)
        nTeams = getEventSetting(ds, drRound.Item("Event"), "TeamsPerRound")
        For x = 0 To dtPairings.Rows.Count - 1
            If dtPairings.Rows(x).Item("Room") = 0 Then
                SideToUse = 0 : room = 0
                'set the side; if none specified, find a team that has their prior round room available
                If Side <> 3 Then SideToUse = Side
                If SideToUse = 0 Then
                    For y = 1 To nTeams
                        room = RoomInPriorRound(ds, dtPairings.Rows(x).Item("Team" & y), round)
                        If room > 0 Then SideToUse = 1 : Exit For
                    Next
                End If
                If RoomActiveInTimeSlot(ds, room, round) = True Then room = 0 'test room isn't in current use
                If RoomOKForDivision(ds, drRound.Item("Event"), room) = False Then room = 0 'room not division blocked
                If room = 0 Then
                    If Not dtPairings.Rows(x).Item("Team" & Side) Is System.DBNull.Value Then
                        room = RoomInPriorRound(ds, dtPairings.Rows(x).Item("Team" & Side), round)
                    End If
                End If
                If RoomActiveInTimeSlot(ds, room, round) = True Then room = 0 'test room isn't in current use
                If RoomOKForDivision(ds, drRound.Item("Event"), room) = False Then room = 0 'room not division blocked
                'if no room still, find one in the same building
                If room = 0 Then
                    For y = 1 To nTeams
                        If Not dtPairings.Rows(x).Item("Team" & y) Is System.DBNull.Value Then
                            drRoom = ds.Tables("Room").Rows.Find(RoomInPriorRound(ds, dtPairings.Rows(x).Item("Team" & y), round))
                            If Not drRoom Is Nothing Then room = GetRoomByBuilding(ds, round, drRoom.Item("Building"))
                            If RoomActiveInTimeSlot(ds, room, round) = True Then room = 0 'test room isn't in current use
                            If RoomOKForDivision(ds, drRound.Item("Event"), room) = False Then room = 0 'room not division blocked
                        End If
                        If room > 0 Then Exit For
                    Next
                End If
                'save it to the dataset
                dtPairings.Rows(x).Item("Room") = room
                drPanel = ds.Tables("Panel").Rows.Find(dtPairings.Rows(x).Item("Panel"))
                drPanel.Item("Room") = room
                If drRound.Item("Flighting") > 1 And drPanel.Item("Room") > 0 Then Call FlightRooms(ds, dtPairings, drPanel.Item("ID"), drRound.Item("JudgesPerPanel"))
            End If
        Next x
    End Sub
    Function RoomOKForDivision(ByVal ds As DataSet, ByVal eventID As Integer, ByVal room As Integer) As Boolean
        If room = 0 Then Exit Function
        RoomOKForDivision = True
        Dim dr As DataRow
        dr = ds.Tables("Room").Rows.Find(room)
        If dr.Item("Event" & eventID.ToString.Trim) = False Then RoomOKForDivision = False
    End Function

    Function GetRoomByBuilding(ByVal ds As DataSet, ByVal round As Integer, ByVal Building As Integer) As Integer
        GetRoomByBuilding = 0
        Dim drRound As DataRow
        drRound = ds.Tables("Round").Rows.Find(round)
        Dim fdRoom As DataRow()
        fdRoom = ds.Tables("Room").Select("Inactive=false and EVENT" & drRound.Item("Event") & "= true and TIMESLOT" & drRound.Item("TimeSlot") & "=true And Building=" & Building, "Quality ASC")
        If fdRoom.Length > 0 Then GetRoomByBuilding = fdRoom(0).Item("ID")
    End Function
    Sub FillOutWithBestRooms(ByRef ds As DataSet, ByRef dtpairings As DataTable, ByVal round As Integer)
        'sort rooms by quality, place from top if no room
        'this does it by seed, with higher seeds getting the best rooms
        Dim drPanel As DataRow
        dtpairings.DefaultView.Sort = "Seed ASC"
        Dim drv As DataRowView
        Dim drRound As DataRow : drRound = ds.Tables("Round").Rows.Find(round)
        For x = 0 To dtpairings.DefaultView.Count - 1
            drv = dtpairings.DefaultView.Item(x)
            If drv.Item("Room") Is System.DBNull.Value Then drv.Item("Room") = 0
            If drv.Item("Room") = 0 Then
                drv.Item("Room") = BestRoomAvailable(ds, round)
                drPanel = ds.Tables("Panel").Rows.Find(drv.Item("Panel"))
                drPanel.Item("Room") = drv.Item("Room")
                If drRound.Item("Flighting") > 1 And drPanel.Item("Room") > 0 Then Call FlightRooms(ds, dtpairings, drPanel.Item("ID"), drRound.Item("JudgesPerPanel"))
            End If
        Next x
    End Sub
    Sub RoomsForSpecTeams(ByVal ds As DataSet, ByRef dtPairings As DataTable, ByVal round As Integer, ByVal strField As String)
        'places rooms for teams with a specific field set to true, generally ADA teams or tub users
        Dim drRound As DataRow
        drRound = ds.Tables("Round").Rows.Find(round)
        Dim TeamsPerRound As Integer = getEventSetting(ds, drRound.Item("Event"), "TeamsPerRound")
        Dim TeamToMatch, JudgeToMatch, Room As Integer
        Dim drTeam, drJudge, drPanel As DataRow
        'scroll through the pairings
        For x = 0 To dtPairings.Rows.Count - 1
            TeamToMatch = 0
            If dtPairings.Rows(x).Item("Room") Is System.DBNull.Value Then dtPairings.Rows(x).Item("Room") = 0
            If dtPairings.Rows(x).Item("Room") = 0 Then
                'check to see if a team in the round has a disability
                For y = 1 To TeamsPerRound
                    drTeam = ds.Tables("Entry").Rows.Find(dtPairings.Rows(x).Item("Team" & y))
                    If Not drTeam Is Nothing Then If drTeam.Item(strField) = True Then TeamToMatch = drTeam.Item("ID")
                    For z = 1 To drRound.Item("JudgesPerPanel")
                        drJudge = ds.Tables("Judge").Rows.Find(dtPairings.Rows(x).Item("Judge" & z))
                        If Not drJudge Is Nothing Then
                            If Not drJudge.Item("ADA") Is System.DBNull.Value Then
                                If drJudge.Item("ADA") = True Then JudgeToMatch = drJudge.Item("ID")
                            End If
                        End If
                    Next z
                Next
                If TeamToMatch > 0 Or JudgeToMatch > 0 Then
                    drPanel = ds.Tables("Panel").Rows.Find(dtPairings.Rows(x).Item("Panel"))
                    'if so, keep them in the same room
                    If TeamToMatch > 0 Then Room = RoomInPriorRound(ds, TeamToMatch, round)
                    If JudgeToMatch > 0 Then Room = JudgeRoomInPriorRound(ds, JudgeToMatch, round)
                    If Room > 0 And RoomActiveInTimeSlot(ds, Room, round) = False And RoomOKForDivision(ds, drRound.Item("Event"), Room) = True Then
                        dtPairings.Rows(x).Item("Room") = Room
                        drPanel.Item("Room") = Room
                    End If
                    'if you can't do that, put them in a good room
                    If Room = 0 Or dtPairings.Rows(x).Item("Room") = 0 Then
                        dtPairings.Rows(x).Item("Room") = BestRoomAvailable(ds, round)
                        drPanel.Item("Room") = dtPairings.Rows(x).Item("Room")
                    End If
                    If drRound.Item("Flighting") > 1 And drPanel.Item("Room") > 0 Then Call FlightRooms(ds, dtPairings, drPanel.Item("ID"), drRound.Item("JudgesPerPanel"))
                End If
            End If
        Next
    End Sub
    Sub FlightRooms(ByRef DS As DataSet, ByRef dtPairings As DataTable, ByVal PanelID As Integer, ByVal Njudges As Integer)
        'takes a pairings table and panel, and duplicates the room assignment for all panels with the same judge
        Dim fdPanel As DataRow() : Dim fdPanel2 As DataRow
        fdPanel = dtPairings.Select("Panel=" & PanelID)
        For x = 0 To dtPairings.Rows.Count - 1
            If dtPairings.Rows(x).Item("Panel") <> PanelID Then
                For y = 1 To Njudges
                    If dtPairings.Rows(x).Item("Judge" & y.ToString.Trim) = fdPanel(0).Item("Judge1") Then
                        dtPairings.Rows(x).Item("Room") = fdPanel(0).Item("Room")
                        fdPanel2 = DS.Tables("Panel").Rows.Find(dtPairings.Rows(x).Item("Panel"))
                        fdPanel2.Item("Room") = fdPanel(0).Item("Room")
                    End If
                Next y
            End If
        Next x
    End Sub
    Function BestRoomAvailable(ByVal ds As DataSet, ByVal round As Integer) As Integer
        Dim drRound As DataRow
        drRound = ds.Tables("Round").Rows.Find(round)
        Dim fdRoom As DataRow()
        fdRoom = ds.Tables("Room").Select("Inactive=false and EVENT" & drRound.Item("Event") & "= true and TIMESLOT" & drRound.Item("TimeSlot") & "=true", "Quality ASC")
        For y = 0 To fdRoom.Length - 1
            If RoomActiveInTimeSlot(ds, fdRoom(y).Item("ID"), round) = False And RoomOKForDivision(ds, drRound.Item("Event"), fdRoom(y).Item("ID")) = True Then BestRoomAvailable = fdRoom(y).Item("ID") : Exit Function
        Next y
        BestRoomAvailable = 0
    End Function
    Function RoomInPriorRound(ByVal ds As DataSet, ByVal Team As Integer, ByVal round As Integer) As Integer
        'round is current round
        Dim PriorRound As Integer = GetPriorRound(ds, round)
        If PriorRound = Nothing Then Exit Function
        Dim dt As DataTable
        Dim drPanel, drBallot As DataRow
        dt = PullBallotsByRound(ds, "ENTRY", Team, PriorRound)
        If dt.Rows.Count = 0 Then RoomInPriorRound = 0 : Exit Function
        drBallot = ds.Tables("Ballot").Rows.Find(dt.Rows(0).Item("ID"))
        drPanel = ds.Tables("Panel").Rows.Find(drBallot.Item("Panel"))
        If drPanel.Item("Room") Is System.DBNull.Value Then
            RoomInPriorRound = 0
        Else
            RoomInPriorRound = drPanel.Item("Room")
        End If
    End Function
    Function JudgeRoomInPriorRound(ByVal ds As DataSet, ByVal Judge As Integer, ByVal round As Integer) As Integer
        'round is current round
        Dim PriorRound As Integer = GetPriorRound(ds, round)
        If PriorRound = Nothing Then Exit Function
        Dim fdBallot As DataRow()
        Dim drPanel As DataRow
        fdBallot = ds.Tables("Ballot").Select("Judge=" & Judge & " and " & BuildPanelStringByRound(ds, PriorRound))
        If fdBallot.Length = 0 Then JudgeRoomInPriorRound = 0 : Exit Function
        drPanel = ds.Tables("Panel").Rows.Find(fdBallot(0).Item("Panel"))
        If drPanel.Item("Room") Is System.DBNull.Value Then
            JudgeRoomInPriorRound = 0
        Else
            JudgeRoomInPriorRound = drPanel.Item("Room")
        End If
    End Function
    Sub AddSeedToResults(ByVal ds As DataSet, ByRef dtPairings As DataTable, ByVal dtSeeds As DataTable, ByVal Round As Integer)
        Dim drSeed, drRound As DataRow : Dim dummy As Integer
        drRound = ds.Tables("Round").Rows.Find(Round)
        Dim TeamsPerRound As Integer = getEventSetting(ds, drRound.Item("Event"), "TeamsPerRound")
        For x = 0 To dtPairings.Rows.Count - 1
            dummy = 1000
            For y = 1 To TeamsPerRound
                drSeed = dtSeeds.Rows.Find(dtPairings.Rows(x).Item("Team" & y))
                If drSeed Is Nothing Then
                    dummy = x : Exit For
                ElseIf drSeed.Item("Seed") Is System.DBNull.Value Then
                    dummy = x : Exit For
                End If
                If drSeed.Item("Seed") < dummy Then dummy = drSeed.Item("Seed")
            Next y
            dtPairings.Rows(x).Item("Seed") = dummy
        Next x
    End Sub
    Sub AddElimSeedToResults(ByVal ds As DataSet, ByRef dtpairings As DataTable, ByVal round As Integer)
        Dim fdSeeds As DataRow() : Dim drRound As DataRow : Dim dummy As Integer
        drRound = ds.Tables("Round").Rows.Find(round)
        Dim TeamsPerRound As Integer = getEventSetting(ds, drRound.Item("Event"), "TeamsPerRound")
        For x = 0 To dtpairings.Rows.Count - 1
            dummy = 1000
            For y = 1 To TeamsPerRound
                fdSeeds = ds.Tables("ElimSeed").Select("Entry=" & dtpairings.Rows(x).Item("Team" & y) & " and round=" & round)
                If fdSeeds(0).Item("Seed") < dummy Then dummy = fdSeeds(0).Item("Seed")
            Next y
            dtpairings.Rows(x).Item("Seed") = dummy
        Next x
    End Sub
    Function RoomMoveReport(ByVal ds As DataSet, ByVal round As Integer) As String
        Dim fdPanel, fdBallot As DataRow()
        fdPanel = ds.Tables("Panel").Select("Round=" & round)
        Dim RoomMove, BuildingMove As Integer
        Dim drRmNow, drRmLast As DataRow
        For x = 0 To fdPanel.Length - 1
            drRmNow = ds.Tables("Room").Rows.Find(fdPanel(x).Item("Room"))
            fdBallot = ds.Tables("Ballot").Select("Panel=" & fdPanel(x).Item("ID"))
            For y = 0 To fdBallot.Length - 1
                drRmLast = ds.Tables("Room").Rows.Find(RoomInPriorRound(ds, fdBallot(y).Item("Entry"), round))
                If Not drRmLast Is Nothing And Not drRmNow Is Nothing Then
                    If drRmNow.Item("ID") <> drRmLast.Item("ID") Then RoomMove += 1
                    If drRmNow.Item("Building") <> drRmLast.Item("BUilding") Then BuildingMove += 1
                End If
            Next y
        Next
        RoomMoveReport = fdPanel.Length & " debates in this round with " & RoomMove & " room moves and " & BuildingMove & " building moves."
    End Function
    Sub DumpAllJudges(ByRef ds As DataSet, ByVal round As Integer)
        Dim fdPanel, fdBallot As DataRow()
        fdPanel = ds.Tables("Panel").Select("Round=" & round)
        For x = 0 To fdPanel.Length - 1
            fdBallot = ds.Tables("Ballot").Select("Panel=" & fdPanel(x).Item("ID"))
            For y = 0 To fdBallot.Length - 1
                If fdBallot(y).Item("Judge") <> -1 Then fdBallot(y).Item("Judge") = -99
            Next y
        Next x
    End Sub
    Sub ValidateScoresByBallot(ByRef DS As DataSet, ByVal BallotID As Integer)
        'makes sure that there's an appropriate ballot_score for every ballot

        'find the round, load all tiebreakers, 
        'make sure there is a field for every row on the ballot.  If not, add one.
        'if side/judge=-1 that's a bye, so add -1 values, otherwise store 0 values.

        Dim drBallot, drRd, drPanel As DataRow
        drBallot = DS.Tables("Ballot").Rows.Find(BallotID)
        drPanel = DS.Tables("Panel").Rows.Find(drBallot.Item("Panel"))
        drRd = DS.Tables("Round").Rows.Find(drPanel.Item("Round"))
        Dim fdTB, fdBallotSc, fdSpkr As DataRow()
        fdTB = DS.Tables("Tiebreak").Select("TB_SET=" & drRd.Item("TB_SET"))
        For x = 0 To fdTB.Length - 1
            'process this for teams
            If fdTB(x).Item("ScoreID") = 1 Or fdTB(x).Item("ScoreID") = 4 Or fdTB(x).Item("ScoreID") = 5 Then
                fdBallotSc = DS.Tables("Ballot_Score").Select("Ballot=" & BallotID & " and Score_ID=" & fdTB(x).Item("ScoreID"))
                'add if doesn't exist
                If fdBallotSc.Length = 0 Then
                    Dim dr As DataRow : dr = DS.Tables("Ballot_Score").NewRow
                    dr.Item("Ballot") = BallotID
                    dr.Item("Recipient") = drBallot.Item("Entry")
                    dr.Item("Score_ID") = fdTB(x).Item("ScoreID")
                    If (drBallot.Item("Side") = -1 Or drBallot.Item("Judge") = -1) Then dr.Item("Score") = -1 Else dr.Item("Score") = 0
                    DS.Tables("Ballot_Score").Rows.Add(dr)
                End If
            End If
            'process for speakers
            If fdTB(x).Item("ScoreID") = 2 Or fdTB(x).Item("ScoreID") = 3 Then
                fdSpkr = DS.Tables("Entry_Student").Select("Entry=" & drBallot.Item("Entry"))
                For y = 0 To fdSpkr.Length - 1
                    fdBallotSc = DS.Tables("Ballot_Score").Select("Ballot=" & BallotID & " and Score_ID=" & fdTB(x).Item("ScoreID") & " and recipient=" & fdSpkr(y).Item("ID"))
                    If fdBallotSc.Length = 0 Then
                        Dim dr As DataRow : dr = DS.Tables("Ballot_Score").NewRow
                        dr.Item("Ballot") = BallotID
                        dr.Item("Recipient") = fdSpkr(y).Item("ID")
                        dr.Item("Score_ID") = fdTB(x).Item("ScoreID")
                        If (drBallot.Item("Side") = -1 Or drBallot.Item("Judge") = -1) Then dr.Item("Score") = -1 Else dr.Item("Score") = 0
                        DS.Tables("Ballot_Score").Rows.Add(dr)
                    End If
                Next
            End If
        Next x
    End Sub
    Sub MakeElimSeeds(ByRef ds As DataSet, ByVal eventID As Integer)
        'send an event, it will create blank seed spots if none exist for ALL rounds in the division
        Dim drElimRds As DataRow()
        drElimRds = ds.Tables("Round").Select("Event=" & eventID & " and RD_NAME>9", "RD_NAME ASC")
        Dim nteams As Integer = getEventSetting(ds, eventID, "TeamsPerRound")

        Dim nSeeds, y As Integer
        Dim dr As DataRow : Dim drExisting As DataRow()
        For x = 0 To drElimRds.Length - 1
            nSeeds = nteams
            For y = 1 To (16 - drElimRds(x).Item("RD_NAME"))
                nSeeds = nSeeds * 2
            Next y
            For y = 1 To nSeeds
                drExisting = ds.Tables("ElimSeed").Select("Event=" & eventID & " and seed=" & y & " and round=" & drElimRds(x).Item("ID"))
                If drExisting.Length = 0 Then
                    dr = ds.Tables("ELIMSEED").NewRow
                    dr.Item("Event") = eventID
                    dr.Item("Round") = drElimRds(x).Item("ID")
                    dr.Item("Entry") = 0
                    dr.Item("Seed") = y
                    ds.Tables("ELIMSEED").Rows.Add(dr)
                End If
            Next y
        Next x
        ds.Tables("ELIMSEED").AcceptChanges()
    End Sub
    Sub FillFirstElim(ByRef ds As DataSet, ByVal EventID As Integer, ByVal clearmethod As Integer)
        'clearmethod 1=full bracket, 2=only winning teams, 3=only .500 teams
        'find first elim round
        Dim foundElims As DataRow()
        foundElims = ds.Tables("Round").Select("Event=" & EventID & " and Rd_Name>9", "RD_NAME asc")
        'now foundelims(0) will be the lowest round
        'find the last prelim completed
        Dim rd = GetLastPrelimRdName(ds, EventID)
        Dim fdLastPrelim
        fdLastPrelim = ds.Tables("Round").Select("Rd_name=" & rd & " and event=" & EventID)
        'get the results table
        Dim dt As New DataTable
        dt = MakeTBTable(ds, fdLastPrelim(0).Item("ID"), "TEAM", "FULL", -1, foundElims(0).Item("ID"))
        'fill it
        Dim foundElimSeeds As DataRow()
        foundElimSeeds = ds.Tables("ElimSeed").Select("Event=" & EventID & " and round=" & foundElims(0).Item("ID"), "Seed ASC")
        dt.DefaultView.Sort = "Seed asc"
        'both foundelimseeds and dt are now sorted by seed
        Dim nprelims As Integer = getEventSetting(ds, EventID, "nPrelims")
        For x = 0 To foundElimSeeds.Length - 1
            foundElimSeeds(x).Item("Entry") = dt.Rows(x).Item("Competitor")
            If clearmethod > 1 Then
                If clearmethod = 2 And dt.Rows(x).Item("Wins") / nprelims <= 0.5 Then foundElimSeeds(x).Item("Entry") = -99
                If clearmethod = 3 And dt.Rows(x).Item("Wins") / nprelims < 0.5 Then foundElimSeeds(x).Item("Entry") = -99
                If clearmethod > 3 And x + 1 > clearmethod Then foundElimSeeds(x).Item("Entry") = -99
            End If
        Next x
        Call PairElim(ds, EventID, foundElims(0).Item("Rd_Name"))
    End Sub
    Function GetLastPrelim(ByVal ds As DataSet, ByVal eventId As Integer)
        Dim fdRds As DataRow()
        Dim rd As Integer = 0
        Dim rdID As Integer
        fdRds = ds.Tables("Round").Select("Event=" & eventId)
        For x = 0 To fdRds.Length - 1
            'If fdRds(x).Item("Rd_Name") > rd And fdRds(x).Item("Rd_Name") <= 9 Then rd = fdRds(x).Item("Rd_Name")
            If fdRds(x).Item("Rd_Name") > rd And fdRds(x).Item("Rd_Name") <= 9 Then rd = fdRds(x).Item("Rd_Name") : rdID = fdRds(x).Item("ID")
        Next x
        Return rdID
    End Function
    Function GetLastPrelimTimeSlot(ByVal ds As DataSet, ByVal eventId As Integer)
        Dim fdRds As DataRow()
        Dim rd As Integer = 0
        Dim rdID As Integer
        fdRds = ds.Tables("Round").Select("Event=" & eventId)
        For x = 0 To fdRds.Length - 1
            If fdRds(x).Item("Rd_Name") > rd And fdRds(x).Item("Rd_Name") <= 9 Then rd = fdRds(x).Item("Rd_Name") : rdID = fdRds(x).Item("TimeSlot")
        Next x
        Return rdID
    End Function
    Function GetLastPrelimRdName(ByVal ds As DataSet, ByVal eventId As Integer)
        Dim fdRds As DataRow()
        Dim rd As Integer = 0
        fdRds = ds.Tables("Round").Select("Event=" & eventId)
        For x = 0 To fdRds.Length - 1
            If fdRds(x).Item("Rd_Name") > rd And fdRds(x).Item("Rd_Name") <= 9 Then rd = fdRds(x).Item("Rd_Name")
        Next x
        Return rd
    End Function
    Function GetFirstElim(ByVal ds As DataSet, ByVal eventId As Integer)
        'returns the ROUND ID (not rd_name) of the first elim round for a division
        Dim fdRds As DataRow()
        Dim rd As Integer = 17 : Dim rdToReturn As Integer
        fdRds = ds.Tables("Round").Select("Event=" & eventId)
        For x = 0 To fdRds.Length - 1
            If fdRds(x).Item("Rd_Name") < rd And fdRds(x).Item("Rd_Name") > 9 Then rd = fdRds(x).Item("Rd_Name") : rdToReturn = fdRds(x).Item("ID")
        Next x
        Return rdToReturn
    End Function
    Sub PairElim(ByRef ds As DataSet, ByVal EventID As Integer, ByVal Round As Integer)
        'go to snake it if wudc
        Dim fdEvent As DataRow : fdEvent = ds.Tables("Event").Rows.Find(EventID)
        If fdEvent.Item("Type") = "WUDC" Then Call SnakeIt(ds, EventID, Round) : Exit Sub
        'round=the Rd_Name
        Dim GetRD As DataRow()
        GetRD = ds.Tables("Round").Select("Event=" & EventID & "and Rd_Name=" & Round)
        Dim Panel As Integer
        Dim fdElimSeeds As DataRow()
        fdElimSeeds = ds.Tables("ElimSeed").Select("Event=" & EventID & " and round=" & GetRD(0).Item("ID"), "Seed ASC")
        Dim side1, side2 As Integer
        For x = 0 To (fdElimSeeds.Length / 2) - 1
            'randomize sides
            side1 = 2 : side2 = 1
            If Rnd() < 0.5 Then side1 = 1 : side2 = 2
            Panel = AddPanel(ds, GetRD(0).Item("ID"), 0)
            If fdElimSeeds(fdElimSeeds.Length - x - 1).Item("Entry") = -99 Then side1 = 1 : side2 = 2
            AddTeamToPanel(ds, Panel, fdElimSeeds(x).Item("Entry"), side1)
            AddTeamToPanel(ds, Panel, fdElimSeeds(fdElimSeeds.Length - x - 1).Item("Entry"), side2)
        Next x
    End Sub
    Sub SnakeIt(ByRef ds As DataSet, ByVal EventID As Integer, ByVal Round As Integer)
        'go to snake it if wudc
        Dim fdEvent As DataRow : fdEvent = ds.Tables("Event").Rows.Find(EventID)
        'round=the Rd_Name
        Dim GetRD As DataRow() : GetRD = ds.Tables("Round").Select("Event=" & EventID & "and Rd_Name=" & Round)
        Dim fdElimSeeds As DataRow()
        fdElimSeeds = ds.Tables("ElimSeed").Select("Event=" & EventID & " and round=" & GetRD(0).Item("ID"), "Seed ASC")
        Dim cycle, scalar, ctr, startpt, endpt, endcycle As Integer : Dim Panel(4) As Integer
        endcycle = (fdElimSeeds.Length / 16) : If endcycle < 1 Then endcycle = 1
        For cycle = 1 To endcycle
            For x = 1 To (fdElimSeeds.Length / 4)
                Panel(x) = AddPanel(ds, GetRD(0).Item("ID"), 0)
            Next x
            For x = 1 To 4
                scalar = 1 : startpt = 1 : endpt = (fdElimSeeds.Length / 4)
                If Int(x / 2) = x / 2 Then scalar = -1 : startpt = (fdElimSeeds.Length / 4) : endpt = 1
                For y = startpt To endpt Step scalar
                    ctr += 1
                    AddTeamToPanel(ds, Panel(y), fdElimSeeds(ctr - 1).Item("Entry"), x)
                Next y
            Next x
        Next cycle
        'Call RandomizeSides(ds, EventID, Round)
        'Call WUDCElimSideEqualizer(ds, EventID, Round)
    End Sub
    Sub RandomizeSides(ByRef ds As DataSet, ByVal EventID As Integer, ByVal Round As Integer)
        Dim fdEvent As DataRow : fdEvent = ds.Tables("Event").Rows.Find(EventID)
        Dim GetRD As DataRow() : GetRD = ds.Tables("Round").Select("Event=" & EventID & "and Rd_Name=" & Round)
        Dim fdPanel, fdBallots As DataRow()
        Dim sides(4) As Integer : Dim ctr As Integer
        fdpanel = ds.Tables("Panel").Select("Round=" & GetRD(0).Item("ID"))
        For x = 0 To fdPanel.Length - 1
            'set sides at random for the panel
            ctr = CInt(Rnd() * 4) + 1
            For y = 1 To 4
                sides(y) = ctr
                ctr = ctr + 1
                If ctr > 4 Then ctr = 1
            Next y
            'pull all ballots for the panel and udpate them
            fdBallots = ds.Tables("Ballot").Select("Panel=" & fdPanel(x).Item("ID"))
            For y = 0 To fdBallots.Length - 1
                fdBallots(y).Item("Side") = sides(fdBallots(y).Item("Side"))
            Next y
        Next x
    End Sub
    Sub WUDCElimSideEqualizer(ByRef ds As DataSet, ByVal EventID As Integer, ByVal Round As Integer)
        Dim GetRD As DataRow() : GetRD = ds.Tables("Round").Select("Event=" & EventID & "and Rd_Name=" & Round)
        Dim fdPanel, fdBallots, fdPrior, fdDummy As DataRow() : Dim drDummy As DataRow
        fdPanel = ds.Tables("Panel").Select("Round=" & GetRD(0).Item("ID"))
        Dim DT As New DataTable : Dim strSort As String
        DT.Columns.Add("Entry", System.Type.GetType("System.Int64"))
        DT.Columns.Add("Side1", System.Type.GetType("System.Int64"))
        DT.Columns.Add("Side2", System.Type.GetType("System.Int64"))
        DT.Columns.Add("Side3", System.Type.GetType("System.Int64"))
        DT.Columns.Add("Side4", System.Type.GetType("System.Int64"))
        For x = 0 To fdPanel.Length - 1
            'fill the datatable with side info
            DT.Clear()
            fdBallots = ds.Tables("Ballot").Select("Panel=" & fdPanel(x).Item("ID"))
            For y = 0 To fdBallots.Length - 1
                fdDummy = DT.Select("Entry=" & fdBallots(y).Item("Entry"))
                If fdDummy.Length = 0 Then
                    drDummy = DT.NewRow : drDummy.Item("Side1") = 0 : drDummy.Item("Side2") = 0 : drDummy.Item("Side3") = 0 : drDummy.Item("Side4") = 0
                    drDummy.Item("Entry") = fdBallots(y).Item("Entry")
                    fdPrior = ds.Tables("Ballot").Select("Entry=" & fdBallots(y).Item("Entry"))
                    For z = 0 To fdPrior.Length - 1
                        drDummy.Item("Side" & fdPrior(z).Item("Side")) += 1
                    Next z
                    DT.Rows.Add(drDummy)
                End If
            Next y
            'now scroll and add
            For y = 1 To 4
                strSort = ""
                For z = y To 4
                    If strSort <> "" Then strSort &= ", "
                    strSort &= "side" & z & " ASC"
                Next z
                DT.DefaultView.Sort = strSort
                fdDummy = ds.Tables("Ballot").Select("Panel=" & fdPanel(x).Item("ID") & " and entry=" & DT.DefaultView(0).Item("Entry"))
                fdDummy(0).Item("Side") = y
                DT.DefaultView(0).Delete()
            Next y
        Next
    End Sub
    Function TimeSlotAudit(ByVal ds As DataSet, ByVal round As Integer) As String
        TimeSlotAudit = ""
        Dim fdRounds, fdPanels, fdBallots As DataRow()
        Dim strPanel, strBallot As String
        'find all ballots in the timeslot for the given round
        'first, pull the round to get the timeslot
        Dim drRd As DataRow : drRd = ds.Tables("Round").Rows.Find(round)
        'now pull all rounds in the timeslot
        fdRounds = ds.Tables("Round").Select("Timeslot=" & drRd.Item("TimeSlot"))
        'build panel string & pull all panels in timeslot
        strPanel = ""
        For x = 0 To fdRounds.Length - 1
            If strPanel <> "" Then strPanel &= " or "
            strPanel &= "Round=" & fdRounds(x).Item("ID")
        Next x
        fdPanels = ds.Tables("Panel").Select(strPanel, "Room ASC")
        'build ballot string and pull all ballots in timeslot
        strBallot = ""
        For x = 0 To fdPanels.Length - 1
            If strBallot <> "" Then strBallot &= " or "
            strBallot &= "panel=" & fdPanels(x).Item("ID")
        Next x
        fdBallots = ds.Tables("Ballot").Select(strBallot, "Judge ASC")
        'all judges no more than flighting for
        Dim drTS As DataRow : Dim drPan1, drPan2 As DataRow : Dim nPanels As Integer = 0
        For x = 1 To fdBallots.Length - 1
            If fdBallots(x - 1).Item("Judge") <> fdBallots(x).Item("Judge") Then nPanels = 0
            If fdBallots(x - 1).Item("Judge") = fdBallots(x).Item("Judge") And fdBallots(x).Item("Judge") > 0 Then
                drPan1 = ds.Tables("Panel").Rows.Find(fdBallots(x).Item("Panel"))
                drPan2 = ds.Tables("Panel").Rows.Find(fdBallots(x - 1).Item("Panel"))
                If drPan1.Item("ID") <> drPan2.Item("ID") Then nPanels += 1
                If nPanels >= drRd.Item("Flighting") Then
                    drTS = ds.Tables("Judge").Rows.Find(fdBallots(x).Item("Judge"))
                    TimeSlotAudit &= drTS.Item("Last") & ", " & drTS.Item("First") & " has been scheduled for too many panels this round. "
                End If
            End If
        Next
        'all judges timeslot elig
        For x = 0 To fdBallots.Length - 1
            If fdBallots(x).Item("Judge") <> -99 And fdBallots(x).Item("Judge") > 0 Then
                drTS = ds.Tables("Judge").Rows.Find(fdBallots(x).Item("Judge"))
                If drTS.Item("TimeSlot" & drRd.Item("Timeslot")) Is System.DBNull.Value Then drTS.Item("TimeSlot" & drRd.Item("Timeslot")) = False
                If drTS.Item("TimeSlot" & drRd.Item("Timeslot")) = False Then
                    TimeSlotAudit &= drTS.Item("Last") & ", " & drTS.Item("First") & " has been placed but is not eligible in this timeslot. "
                End If
            End If
        Next x
        'all rooms only once
        nPanels = 0
        For x = 1 To fdPanels.Length - 1
            If Not fdPanels(x - 1).Item("Room") Is System.DBNull.Value And Not fdPanels(x).Item("Room") Is System.DBNull.Value Then
                If fdPanels(x - 1).Item("Room") <> fdPanels(x).Item("Room") Then nPanels = 0
                If fdPanels(x - 1).Item("Room") = fdPanels(x).Item("Room") And fdPanels(x).Item("Room") > 0 Then nPanels += 1
                If nPanels >= drRd.Item("Flighting") Then
                    drTS = ds.Tables("Room").Rows.Find(fdPanels(x).Item("Room"))
                    TimeSlotAudit &= drTS.Item("RoomName") & " has been placed more than once. "
                End If
            End If
        Next
        'all rooms timeslot elig
        Dim boolCheck As Boolean
        For x = 0 To fdPanels.Length - 1
            boolCheck = True
            If fdPanels(x).Item("Room") Is System.DBNull.Value Then
                boolCheck = False
            ElseIf fdPanels(x).Item("Room") = -99 Or fdPanels(x).Item("Room") <= 0 Then
                boolCheck = False
            End If
            If boolCheck = True Then
                drTS = ds.Tables("Room").Rows.Find(fdPanels(x).Item("Room"))
                If Not drTS Is Nothing Then
                    If drTS.Item("TimeSlot" & drRd.Item("Timeslot")) = "" Then drTS.Item("TimeSlot" & drRd.Item("Timeslot")) = False
                End If
                If drTS Is Nothing Then
                    TimeSlotAudit &= "A room has been placed that is no longer in the data file. "
                ElseIf drTS.Item("TimeSlot" & drRd.Item("Timeslot")) = False Then
                    TimeSlotAudit &= drTS.Item("RoomName") & " has been placed but is not eligible in this timeslot. "
                End If
            End If
        Next x
        'check all debates have right # teams, judges, and a room
        If TimeSlotAudit = "" Then TimeSlotAudit = "No time slot conflicts. "
    End Function
    Function AuditRound(ByVal ds As DataSet, ByVal round As Integer) As String
        AuditRound = ""
        'pull all ballots and panels by round
        Dim drRd As DataRow = ds.Tables("Round").Rows.Find(round)
        Dim drDummy As DataRow
        Dim fdBallots, fdPanels, fdDummy As DataRow() : Dim dummy(9) As Integer
        fdPanels = ds.Tables("Panel").Select("Round=" & round)
        Dim strPanel As String = ""
        For x = 0 To fdPanels.Length - 1
            If strPanel <> "" Then strPanel &= " or "
            strPanel &= "Panel=" & fdPanels(x).Item("ID")
        Next
        fdBallots = ds.Tables("Ballot").Select(strPanel)
        'ROOMS
        For x = 0 To 9 : dummy(x) = 0 : Next x
        For x = 0 To fdPanels.Length - 1
            If fdPanels(x).Item("Room") Is System.DBNull.Value Then fdPanels(x).Item("Room") = -99
            If fdPanels(x).Item("Room") <= 0 Then
                'don't count if bye or no judge assigned
                fdDummy = ds.Tables("Ballot").Select("Panel=" & fdPanels(x).Item("ID"), "Judge ASC")
                If fdDummy(0).Item("Judge") > 0 Then dummy(1) += 1
            Else
                drDummy = ds.Tables("Room").Rows.Find(fdPanels(x).Item("Room"))
                If Not drDummy Is Nothing Then
                    If drDummy.Item("Inactive") = True Then dummy(2) += 1
                    If drDummy.Item("Event" & drRd.Item("Event")) = "" Then drDummy.Item("Event" & drRd.Item("Event")) = False
                    If drDummy.Item("Event" & drRd.Item("Event")) = False Then dummy(3) += 1
                End If
            End If
        Next x
        If dummy(1) > 0 Then AuditRound &= dummy(1).ToString.Trim & " debates missing rooms."
        If dummy(2) > 0 Then AuditRound &= dummy(2).ToString.Trim & " debates with rooms that are NOT available."
        If dummy(3) > 0 Then AuditRound &= dummy(3).ToString.Trim & " rooms placed that are NOT available for this event."
        'JUDGES
        Dim strBlockedJudges As String = "" : Dim strTwoTimeJudges As String = ""
        Dim dr As DataRow
        For x = 0 To 9 : dummy(x) = 0 : Next x
        For x = 0 To fdBallots.Length - 1
            If fdBallots(x).Item("Judge") = -99 Then dummy(1) += 1
            If fdBallots(x).Item("Judge") > 0 Then
                fdDummy = ds.Tables("JudgePref").Select("Judge=" & fdBallots(x).Item("Judge") & " and team=" & fdBallots(x).Item("Entry"))
                If fdDummy.Length > 0 Then
                    If fdDummy(0).Item("OrdPct") Is System.DBNull.Value Then fdDummy(0).Item("OrdPct") = 0
                    If fdDummy.Length > 0 Then
                        If fdDummy(0).Item("OrdPct") = 999 Or fdDummy(0).Item("Rating") = 999 Then
                            dummy(2) += 1
                            dr = ds.Tables("Judge").Rows.Find(fdBallots(x).Item("Judge"))
                            strBlockedJudges &= dr.Item("Last")
                        End If
                    End If
                End If
                drDummy = ds.Tables("Judge").Rows.Find(fdBallots(x).Item("Judge"))
                If drDummy.Item("Event" & drRd.Item("Event")) Is System.DBNull.Value Then drDummy.Item("Event" & drRd.Item("Event")) = False
                If drDummy.Item("Event" & drRd.Item("Event")) = "" Then drDummy.Item("Event" & drRd.Item("Event")) = False
                If drDummy.Item("Event" & drRd.Item("Event")) = False Then dummy(3) += 1
                If drDummy.Item("StopScheduling") Is System.DBNull.Value Then drDummy.Item("StopScheduling") = False
                If drDummy.Item("StopScheduling") = True Then dummy(4) += 1
                If JudgedBefore(ds, fdBallots(x).Item("Judge"), fdBallots(x).Item("Panel")) = True Then
                    dummy(5) += 1
                    dr = ds.Tables("Judge").Rows.Find(fdBallots(x).Item("Judge"))
                    strTwoTimeJudges &= dr.Item("Last")
                End If
            End If
        Next x
        If dummy(1) > 0 Then AuditRound &= dummy(1).ToString.Trim & " debates missing judges. " & Chr(10)
        If dummy(2) > 0 Then AuditRound &= dummy(2).ToString.Trim & " judges assigned to teams who have blocked them. " & strBlockedJudges & Chr(10)
        If dummy(3) > 0 Then AuditRound &= Int(dummy(3) / 2).ToString.Trim & " judges assigned in events they are not eligible to hear. " & Chr(10)
        If dummy(4) > 0 Then AuditRound &= Int(dummy(4) / 2).ToString.Trim & " judges marked as STOP SCHEDULING were assigned anyway. " & Chr(10)
        If dummy(5) > 0 Then AuditRound &= dummy(5).ToString.Trim & " judges hearing teams for a second time or scheduled to hear the team in the future. " & "(" & strTwoTimeJudges & ")" & Chr(10)
        'TEAMS
        For x = 0 To 9 : dummy(x) = 0 : Next x
        Dim boolFd As Boolean
        fdDummy = ds.Tables("Entry").Select("Event=" & drRd.Item("Event"))
        For x = 0 To fdDummy.Length - 1
            boolFd = False
            For y = 0 To fdBallots.Length - 1
                If fdBallots(y).Item("Entry") = fdDummy(x).Item("ID") Then boolFd = True
            Next y
            If boolFd = False And fdDummy(x).Item("dropped") = False Then dummy(1) += 1
            If boolFd = True And fdDummy(x).Item("dropped") = True Then dummy(2) += 1
        Next x
        If dummy(1) > 0 Then AuditRound &= dummy(1).ToString.Trim & " teams unscheduled. " & Chr(10)
        If dummy(2) > 0 Then AuditRound &= dummy(2).ToString.Trim & " teams marked as STOP SCHEDULING but paired anyway. " & Chr(10)
        For x = 0 To fdPanels.Length - 1
            fdDummy = ds.Tables("Ballot").Select("Panel=" & fdPanels(x).Item("ID"), "Entry ASC")
            For y = 1 To fdDummy.Length - 1
                If fdDummy(y).Item("Entry") <> fdDummy(y - 1).Item("Entry") And fdDummy(y).Item("Entry") <> -99 And fdDummy(y = -1).Item("Entry") <> -99 Then
                    If SameSchool(ds, fdDummy(y).Item("Entry"), fdDummy(y - 1).Item("Entry")) = True Then dummy(3) += 1
                    If HitBefore(ds, fdDummy(y).Item("Entry"), fdDummy(y - 1).Item("Entry"), fdPanels(x).Item("ID")) = True Then dummy(4) += 1
                End If
            Next y
        Next x
        If dummy(3) > 0 Then AuditRound &= dummy(3).ToString.Trim & " teams from the same school debating. " & Chr(10)
        If dummy(4) > 0 Then AuditRound &= dummy(4).ToString.Trim & " teams hitting for a second time (or scheduled to debate in the future). " & Chr(10)
        'this only works for 2-team events; you might use the SideCheck function instead
        If drRd.Item("Rd_Name") / 2 = drRd.Item("Rd_Name") Then
            For x = 0 To fdBallots.Length - 1
                If GetSideDue(ds, round, fdBallots(x).Item("ENTRY")) <> fdBallots(x).Item("Side") Then dummy(5) += 1
            Next
        End If
        If dummy(5) > 0 Then AuditRound &= dummy(5).ToString.Trim & " teams with side problems. " & Chr(10)
        If AuditRound = "" Then AuditRound = "No round conflicts."
    End Function
    Sub SideFlipper(ByRef ds As DataSet, ByVal PanelID As Integer)
        Dim fdBallots As DataRow()
        fdBallots = ds.Tables("Ballot").Select("Panel=" & PanelID)
        For x = 0 To fdBallots.Length - 1
            If fdBallots(x).Item("Side") = 1 Then
                fdBallots(x).Item("Side") = 2
            ElseIf fdBallots(x).Item("Side") = 2 Then
                fdBallots(x).Item("Side") = 1
            End If
        Next x
    End Sub
End Module
