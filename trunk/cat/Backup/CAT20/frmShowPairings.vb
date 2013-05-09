'STILL TO DO ON THIS PAGE
'Get the validation thing in order; 
'Allow to place judges even if all teams aren't present

Public Class frmShowPairings
    Dim ds As New DataSet
    Dim EditMode As String
    Dim dtTeams As DataTable
    Dim EnableEvents As Boolean

    Private Sub frmShowPairings_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        EnableEvents = False

        'Load
        LoadFile(ds, "TourneyData")
        Call NullKiller(ds, "Entry") : Call NullKiller(ds, "Room") : Call NullKiller(ds, "Judge")
        Call CalcPrefAverages(ds)
        Call LoadJudgeSettings()

        'bind round CBO
        cboRound.DataSource = ds.Tables("Round")
        cboRound.DisplayMember = "Label"
        cboRound.ValueMember = "ID"
        cboRound.Focus()

        EnableEvents = True
    End Sub
    Private Sub frmShowPairings_Unload(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Disposed
        'save file on page close
        Call SaveFile(ds)
        ds.Dispose()
    End Sub

    Private Sub butLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butLoad.Click
        'set default for colorcode scheme
        Dim drRound As DataRow
        drRound = ds.Tables("Round").Rows.Find(cboRound.SelectedValue)
        Dim drEvent As DataRow
        drEvent = ds.Tables("Event").Rows.Find(drRound.Item("Event"))
        If drEvent.Item("Type") = "WUDC" Then butSideChange.Visible = True Else butSideChange.Visible = False

        Call LoadTeamTable()
        Call ShowThePairing()
        DataGridView2.Columns.Clear()
        DataGridView2.DataSource = Nothing

    End Sub
    Sub LoadTeamTable()
        dtTeams = New DataTable
        'might not want cboround.selectedvalue -1, gotta actuall find the round in the prior timeslot
        Dim dummy As Integer = GetPriorRound(ds, cboRound.SelectedValue)
        If cboRound.SelectedIndex = 0 Then dummy = cboRound.SelectedValue
        Dim dr As DataRow = ds.Tables("Round").Rows.Find(cboRound.SelectedValue)
        If dr.Item("Rd_Name") = 1 Then dummy = cboRound.SelectedValue
        If dr.Item("PairingScheme").toupper = "ELIM" Then dummy = GetLastPrelim(ds, dr.Item("Event"))
        Dim strNameType As String = "CODE"
        If chkShowFullNames.Checked = True Then strNameType = "FULLNAME"
        dtTeams = MakeTBTable(ds, dummy, "TEAM", strNameType, -1, cboRound.SelectedValue)
        'if it's not a preset, all the tiebreakers will be there.  Delete all but the first 2 tiebreakers
        If dtTeams.Columns.Count > 3 Then
            For x = (dtTeams.Columns.Count - 1) To 5 Step -1
                'dtTeams.Columns.Remove(x)
            Next
        End If
        'Add debatingnow column
        dtTeams.Columns.Add("DebatingNow", System.Type.GetType("System.Boolean"))
        dtTeams.Columns.Add("Sides", System.Type.GetType("System.String"))
        dtTeams.DefaultView.Sort = "DebatingNow ASC, seed asc"
        dtTeams.AcceptChanges()
        Call MarkTeamInAction()
        Call AddSideValues()
    End Sub
    Sub AddSideValues()
        If cboRound.SelectedIndex = -1 Then
            MsgBox("Please select a round and try again.", MsgBoxStyle.OkOnly) : Exit Sub
        End If
        Dim drRd As DataRow
        drRd = ds.Tables("Round").Rows.Find(cboRound.SelectedValue)
        If getEventSetting(ds, drRd.Item("Event"), "TeamsPerRound") = 2 Then
            For x = 0 To dtTeams.Rows.Count - 1
                dtTeams.Rows(x).Item("Sides") = GetSideString(ds, GetSideDue(ds, cboRound.SelectedValue, dtTeams.Rows(x).Item("Competitor")), drRd.Item("Event"))
            Next
        Else
            Call AddSideReport(dtTeams, ds)
        End If

    End Sub
    Sub ShowThePairing()
        Dim dummy As String = "Code" : If chkShowFullNames.Checked = True Then dummy = "Full"
        Call modShowThePairing(ds, DataGridView1, cboRound.SelectedValue, dummy)
        Call AddPrefColumns()
        If chkShowFits.Checked = True Then Call AddFitsColumns()
        If chkJudgeUse.Checked = True Then Call AddJudgeUseColumn()
        If chkShowRecords.Checked = True Then Call AddRecordsToNames()
        If chkColorCode.Checked = True Then Call ColorCodeAssignedJudges()
        Call MarkBlanks()
        Dim drRd As DataRow : drRd = ds.Tables("Round").Rows.Find(cboRound.SelectedValue)
        If drRd.Item("Flighting") > 1 Then
            Dim dtdummy As DataTable : dtdummy = DataGridView1.DataSource
            dtdummy.DefaultView.Sort = "Room ASC, JudgeName1 ASC, Flight ASC"
            DataGridView1.Columns("Flight").Visible = True
        End If
    End Sub
    Sub AddRecordsToNames()
        'Dim dtDummy As DateTime = Now
        'Dim ts As TimeSpan = Now.Subtract(dtDummy)
        'Dim strdummy As String = Now.Millisecond
        'Dim DifferenceInMilliseconds As Double = ts.TotalMilliseconds
        'lblGridKey.Text = ts.ToString
        Dim drrd As DataRow : drrd = ds.Tables("Round").Rows.Find(cboRound.SelectedValue)
        Dim drTeam As DataRow() : Dim dr As DataRow
        For x = 0 To DataGridView1.RowCount - 1
            For y = 0 To DataGridView1.ColumnCount - 1
                If InStr(DataGridView1.Columns(y).Name.ToUpper, "TEAMNAME") > 0 Then
                    If Not DataGridView1.Rows(x).Cells(y - 1).Value Is System.DBNull.Value Then
                        drTeam = dtTeams.Select("Competitor=" & DataGridView1.Rows(x).Cells(y - 1).Value)
                        'wins if they're there
                        If dtTeams.Columns.Contains("Wins") Then
                            If drTeam.Length > 0 Then DataGridView1.Rows(x).Cells(y).Value = drTeam(0).Item("Wins") & "-" & drrd.Item("Rd_Name") - 1 - drTeam(0).Item("Wins") & " " & DataGridView1.Rows(x).Cells(y).Value
                        Else
                            'seeds if they're not
                            If drTeam.Length > 0 Then If drTeam(0).Item("Seed") > 0 Then DataGridView1.Rows(x).Cells(y).Value = drTeam(0).Item("Seed") & " " & DataGridView1.Rows(x).Cells(y).Value
                            'rating if neither
                            If drTeam.Length>0 then
                            If drTeam(0).Item("Seed") = 0 Then
                                dr = ds.Tables("Entry").Rows.Find(DataGridView1.Rows(x).Cells(y - 1).Value)
                                If Not dr Is Nothing Then
                                    DataGridView1.Rows(x).Cells(y).Value = dr.Item("Rating") & " " & DataGridView1.Rows(x).Cells(y).Value
                                End If
                                End If
                            End If
                        End If
                    End If
                End If
            Next y
        Next x
        'ts = Now.Subtract(dtDummy)
        'DifferenceInMilliseconds = ts.TotalMilliseconds
        ' lblGridKey.Text = strdummy & " " & Now.Millisecond & " " & ts.ToString & " seconds to process records."
    End Sub
    Sub AddJudgeUseColumn()
        'Creates a column for each judge that shows their use status
        'add a column for each judge
        Dim dt As New DataTable
        dt = DataGridView1.DataSource
        Dim drJudge, drRound As DataRow : Dim JudgedAlready As Integer
        drRound = ds.Tables("Round").Rows.Find(cboRound.SelectedValue)
        For x = 1 To drRound.Item("JudgesPerPanel")
            dt.Columns.Add("JudgeUse" & x, System.Type.GetType("System.String"))
        Next x
        'populate the columns
        For z = 0 To dt.Rows.Count - 1
            For x = 1 To drRound.Item("JudgesPerPanel")
                drJudge = ds.Tables("Judge").Rows.Find(dt.Rows(z).Item("Judge" & x))
                If drJudge Is Nothing Then
                    dt.Rows(z).Item("JudgeUse" & x) = "- - -"
                Else
                    JudgedAlready = GetRoundsJudged(ds, drJudge.Item("ID"))
                    dt.Rows(z).Item("JudgeUse" & x) = drJudge.Item("Obligation") + drJudge.Item("Hired") - JudgedAlready & "/" & JudgedAlready & "/" & drJudge.Item("Obligation") + drJudge.Item("Hired")
                End If
            Next x
        Next z
    End Sub
    Sub ReCalcAllPercentiles()
        For x = 0 To ds.Tables("Entry").Rows.Count - 1
            Call DSAutoCalc(ds, ds.Tables("Entry").Rows(x).Item("ID"))
        Next
    End Sub
    Sub AddPrefColumns()

        'check that there are percentiles
        Dim dummyfd As DataRow()
        dummyfd = ds.Tables("JudgePref").Select("OrdPct is NULL and Rating>0")
        If dummyfd.Length / ds.Tables("JudgePref").Rows.Count > 0.7 Then
            Dim q As Integer = InputBox("Your judges have ratings but not ordinal percentiles.  Enter 1 if you wish to simply use ratings, or 2 to convert all ratings into ordinal percentiles:", "Missing ordinal percentiles", 2)
            If q = 1 Then
                For x = 1 To ds.Tables("JudgePref").Rows.Count - 1
                    ds.Tables("JudgePref").Rows(x).Item("OrdPct") = ds.Tables("JudgePref").Rows(x).Item("Rating")
                Next x
            Else
                Call ReCalcAllPercentiles()
            End If
        End If

        'if so, show the balance and individual ratings in one column
        Dim dt As New DataTable
        dt = DataGridView1.DataSource
        dt.Columns.Add("Balance", System.Type.GetType("System.String"))

        Dim nTeams, nJudges As Integer
        Dim drRound As DataRow
        drRound = ds.Tables("Round").Rows.Find(cboRound.SelectedValue)
        nTeams = getEventSetting(ds, drRound.Item("Event"), "TeamsPerRound")
        nJudges = drRound.Item("JudgesPerPanel")

        'see if prefs are in use this round, if not, show ratings
        Dim drJudge As DataRow
        If drRound.Item("JudgePlaceScheme").toupper = "ASSIGNEDRATING" Then
            For z = 0 To dt.Rows.Count - 1
                For y = 1 To nJudges
                    drJudge = ds.Tables("Judge").Rows.Find(dt.Rows(z).Item("Judge" & y))
                    If Not drJudge Is Nothing Then
                        dt.Rows(z).Item("Balance") &= drJudge.Item("TabRating")
                    End If
                Next y
            Next z
            DataGridView1.Columns("Balance").HeaderText = "Judge Rating"
            DataGridView1.Columns("Balance").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            'Exit Sub
        End If

        Dim ratings(nTeams) As Integer : Dim Balance As Integer

        For z = 0 To dt.Rows.Count - 1
            'reset counter variables
            Balance = 0
            For x = 0 To nTeams : ratings(x) = 0 : Next x
            'get the rating for each judge and each team
            For x = 1 To nTeams
                For y = 1 To nJudges
                    If dt.Rows(z).Item("Judge" & y) Is System.DBNull.Value Then dt.Rows(z).Item("Judge" & y) = -99
                    If dt.Rows(z).Item("Judge" & y) <> -99 And Not dt.Rows(z).Item("Team" & x) Is System.DBNull.Value Then
                        ratings(x) += GetJudgeRating(ds, dt.Rows(z).Item("Judge" & y), dt.Rows(z).Item("Team" & x), drRound.Item("JudgePlaceScheme"))
                    End If
                Next y
            Next x
            'sum for the balance & add to grid
            For x = 1 To nTeams - 1
                For y = x + 1 To nTeams
                    Balance += Math.Abs(ratings(x) - ratings(y))
                Next y
            Next x
            If drRound.Item("JudgePlaceScheme").toupper = "TEAMRATING" Then dt.Rows(z).Item("Balance") = Balance & " "
            'add ratings
            For x = 1 To nTeams
                If drRound.Item("JudgePlaceScheme").toupper = "TEAMRATING" Then
                    'if using MPJ, show the balance
                    dt.Rows(z).Item("Balance") &= ratings(x)
                    If x <> nTeams Then dt.Rows(z).Item("Balance") &= "-"
                Else
                    'if not, just mark strikes/conflicts
                    If ratings(x) = 999 Then dt.Rows(z).Item("Balance") = "X"
                End If
            Next x
            If dt.Rows(z).Item("Balance") Is System.DBNull.Value Then dt.Rows(z).Item("Balance") = "0"
        Next z

    End Sub
    Sub AddFitsColumns()
        Call NullKiller(ds, "Judge")
        Dim UsingPrefs As Boolean
        'see if prefs are in use this round, exit if not
        Dim dr As DataRow
        dr = ds.Tables("Round").Rows.Find(cboRound.SelectedValue)
        If dr.Item("JudgePlaceScheme") = "TeamRating" Then UsingPrefs = True

        'if so, show the balance and inidividual ratings in one column
        Dim dtDeb As DataTable : Dim nFits, rat1, rat2 As Integer : Dim CanPlace As Boolean

        dtDeb = DataGridView1.DataSource
        If dtDeb.Columns.Contains("Fits") = False Then
            dtDeb.Columns.Add("Fits", System.Type.GetType("System.Int16"))
        End If

        For x = 0 To dtDeb.Rows.Count - 1
            nFits = 0 : dtDeb.Rows(x).Item("Fits") = 0
            For y = 0 To ds.Tables("Judge").Rows.Count - 1
                CanPlace = True
                'test same school
                If dr.Item("Rd_Name") < 10 Then 'prelim
                    'remaining commitment
                    If ds.Tables("Judge").Rows(y).Item("Obligation") - GetRoundsJudged(ds, ds.Tables("Judge").Rows(y).Item("ID")) <= 0 Then CanPlace = False
                End If
                'stop scheduled?
                If ds.Tables("Judge").Rows(y).Item("StopScheduling") = True Then CanPlace = False
                'is judge eligible for division?
                If ds.Tables("Judge").Rows(y).Item("Event" & dr.Item("Event")) = False Then CanPlace = False
                'Is the judge from the same school as any team on the panel?
                If JudgeSameSchool(ds, ds.Tables("Judge").Rows(y).Item("ID"), dtDeb.Rows(x).Item("Panel")) = True Then CanPlace = False
                'If judging twice isn't OK, have they judged any team on the panel before?
                If chkHearOnce.Checked = True And JudgedBefore(ds, ds.Tables("Judge").Rows(y).Item("ID"), dtDeb.Rows(x).Item("Panel")) = True Then CanPlace = False
                'now ratings/conflicts
                rat1 = GetJudgeRating(ds, ds.Tables("Judge").Rows(y).Item("ID"), dtDeb.Rows(x).Item("Team1"), dr.Item("JudgePlaceScheme"))
                If dtDeb.Rows(x).Item("Team2") Is System.DBNull.Value Then dtDeb.Rows(x).Item("Team2") = -99
                rat2 = GetJudgeRating(ds, ds.Tables("Judge").Rows(y).Item("ID"), dtDeb.Rows(x).Item("Team2"), dr.Item("JudgePlaceScheme"))
                If rat1 > DataGridView3.Rows(0).Cells(1).Value Or rat2 > DataGridView3.Rows(0).Cells(1).Value Or Math.Abs(rat1 - rat2) > DataGridView3.Rows(1).Cells(1).Value Then CanPlace = False
                If CanPlace = True Then nFits += 1
            Next y
            dtDeb.Rows(x).Item("Fits") = nFits
        Next x


    End Sub

    Sub MarkBlanks()
        Dim SkipCol As Boolean
        'mark blank entries
        For x = 0 To DataGridView1.RowCount - 2
            DataGridView1.Rows(x).Selected = False
            For y = 1 To DataGridView1.ColumnCount - 1
                SkipCol = False
                If DataGridView1.Rows(x).Cells("JudgeName1").Value.ToString.ToUpper = "BYE" Then SkipCol = True
                If DataGridView1.Columns(y).Name = "Balance" Then SkipCol = True
                If DataGridView1.Columns(y).Name = "ROOMNAME" Then SkipCol = True
                If InStr(DataGridView1.Columns(y).Name.ToUpper, "JUDGEUSE") > 0 Then SkipCol = True
                If DataGridView1.Columns(y).Visible = True And SkipCol = False And Not DataGridView1.Rows(x).Cells(y - 1).Value Is System.DBNull.Value And Not DataGridView1.Columns(y).Name.ToUpper = "FITS" Then
                    If DataGridView1.Rows(x).Cells(y - 1).Value = -99 Then DataGridView1.Item(y, x).Style.BackColor = Color.Red
                ElseIf DataGridView1.Rows(x).Cells(y - 1).Value Is System.DBNull.Value And SkipCol = False Then
                    DataGridView1.Item(y, x).Style.BackColor = Color.Red
                ElseIf DataGridView1.Columns(y).Name = "ROOMNAME" Then
                    If DataGridView1.Rows(x).Cells("JudgeName1").Value.ToString.ToUpper <> "BYE" Then
                        If DataGridView1.Rows(x).Cells("Room").Value Is System.DBNull.Value Then DataGridView1.Item(y, x).Style.BackColor = Color.Red
                    End If
                End If
            Next y
        Next x
    End Sub
    Private Sub Grid1Clicked() Handles DataGridView1.MouseClick
        grpJudgeSettings.Visible = False : grpRoomOptions.Visible = False
        If DataGridView1.DataSource Is Nothing Then Exit Sub
        DataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing
        DataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView2.ColumnHeadersHeight = 28
        If InStr(DataGridView1.Columns(DataGridView1.CurrentCell.ColumnIndex).Name.ToUpper, "JUDGE") > 0 Then
            EditMode = "Judge"
            Call LoadJudges()
            grpJudgeSettings.Visible = True
            lblGridKey.Text = "BLUE = currently ASSIGNED.  RED = ineligible for selected debate."
        ElseIf InStr(DataGridView1.Columns(DataGridView1.CurrentCell.ColumnIndex).Name.ToUpper, "TEAM") > 0 Then
            DataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
            EditMode = "Team"
            lblGridKey.Text = "BLUE = currently UN-ASSIGNED.  RED = ineligible for selected debate."
            Call LoadTeams()
        ElseIf InStr(DataGridView1.Columns(DataGridView1.CurrentCell.ColumnIndex).Name.ToUpper, "ROOM") > 0 Then
            EditMode = "Room"
            Call LoadRooms()
            Call ShowRoomsLastRound()
            grpRoomOptions.Visible = True : grpRoomOptions.Location = New Point(1013, 480)
            lblGridKey.Text = "RED = room unavailble in timeslot or for division."
        End If
    End Sub
    Function FlightingCheck() As String
        FlightingCheck = ""
        Dim dr As DataRow : dr = ds.Tables("Round").Rows.Find(cboRound.SelectedValue)
        If dr.Item("Flighting") < 2 Then Exit Function
        Dim roomsPlaced As Boolean = False
        For x = 0 To DataGridView1.Rows.Count - 1
            If Not DataGridView1.Rows(x).Cells("Room").Value Is System.DBNull.Value Then
                If DataGridView1.Rows(x).Cells("Room").Value > 0 Then roomsPlaced = True
            End If
        Next
        Dim judgesplaced As Boolean = True
        For x = 0 To DataGridView1.Rows.Count - 1
            If Not DataGridView1.Rows(x).Cells("Judge1").Value Is System.DBNull.Value Then
                If DataGridView1.Rows(x).Cells("Judge1").Value > 0 Then judgesplaced = True
            End If
        Next
        If roomsPlaced = False Or judgesplaced = True Then Exit Function
        FlightingCheck = "This is a flighted round but you have placed rooms before judges.  Please delete all room placements, place judges, and then rooms."
    End Function
    Sub LoadJudges()

        Dim strFlightCheck As String = FlightingCheck()
        If strFlightCheck <> "" Then
            MsgBox(strFlightCheck)
            Exit Sub
        End If

        Dim str As String = ""
        str &= "Start: " & Now.Second & " " & Now.Millisecond & Chr(10) & Chr(10)

        Dim drRd, drEvent As DataRow
        drRd = ds.Tables("Round").Rows.Find(cboRound.SelectedValue)
        If drRd Is Nothing Then MsgBox("The round appears invalid; select a valid round and try again.", MsgBoxStyle.OkOnly) : Exit Sub
        drEvent = ds.Tables("Event").Rows.Find(drRd.Item("Event"))
        Dim TeamsPerRound As Integer = getEventSetting(ds, drEvent.Item("ID"), "TeamsPerRound")

        Dim dt As New DataTable
        dt.Columns.Add("ID", System.Type.GetType("System.Int64"))
        dt.Columns.Add("Name", System.Type.GetType("System.String"))
        If chkJudgeSchool.Checked = True Then
            dt.Columns.Add("From", System.Type.GetType("System.String"))
        End If
        dt.Columns.Add("Left", System.Type.GetType("System.Int16"))
        dt.Columns.Add("Pref", System.Type.GetType("System.Int16"))
        dt.Columns.Add("Mutuality", System.Type.GetType("System.Int16"))
        dt.Columns.Add("AvgPref", System.Type.GetType("System.Int16"))
        dt.Columns.Add("Assigned", System.Type.GetType("System.Boolean"))
        dt.Columns.Add("FitsCriteria", System.Type.GetType("System.Boolean"))
        Dim x, y, z As Integer
        If drRd.Item("Flighting") < 1 Then drRd.Item("Flighting") = 1
        If drRd.Item("Flighting") Is Nothing Then drRd.Item("Flighting") = 1
        If drRd.Item("Flighting") Is System.DBNull.Value Then drRd.Item("Flighting") = 1
        For x = 1 To (TeamsPerRound * drRd.Item("Flighting"))
            dt.Columns.Add("Rating" & x, System.Type.GetType("System.Int16"))
        Next
        dt.Columns.Add("Owed", System.Type.GetType("System.Int16"))
        dt.Columns.Add("Judged", System.Type.GetType("System.Int16"))
        dt.Columns.Add("TimeAvail", System.Type.GetType("System.Boolean"))
        dt.Columns.Add("JudgedBefore", System.Type.GetType("System.Boolean"))
        dt.Columns.Add("HearingNow", System.Type.GetType("System.String"))

        Dim dr2, drSchool As DataRow
        'things you need for flighting
        Dim dtDummy As DataTable : dtDummy = DataGridView1.DataSource : Dim fdOtherRooms As DataRow() : Dim ratCt As Integer

        For x = 0 To ds.Tables("Judge").Rows.Count - 1
            If ds.Tables("Judge").Rows(x).Item("StopScheduling") Is System.DBNull.Value Then ds.Tables("Judge").Rows(x).Item("StopScheduling") = False
            If ds.Tables("Judge").Rows(x).Item("StopScheduling") = False Then
                dr2 = dt.NewRow
                dr2.Item("ID") = ds.Tables("Judge").Rows(x).Item("ID")
                dr2.Item("Name") = ds.Tables("Judge").Rows(x).Item("Last").trim & ", " & ds.Tables("Judge").Rows(x).Item("First").trim
                If chkJudgeSchool.Checked = True Then
                    drSchool = ds.Tables("School").Rows.Find(ds.Tables("Judge").Rows(x).Item("School"))
                    'dr2.Item("Name") &= "-" & drSchool.Item("Code")
                    If Not drSchool Is Nothing Then
                        dr2.Item("From") &= drSchool.Item("Code")
                    Else
                        dr2.Item("From") &= "unaff"
                    End If
                End If
                    dr2.Item("Owed") = ds.Tables("Judge").Rows(x).Item("Obligation") + ds.Tables("Judge").Rows(x).Item("Hired")
                    dr2.Item("Judged") = GetRoundsJudged(ds, ds.Tables("Judge").Rows(x).Item("ID"))
                    dr2.Item("Left") = dr2.Item("Owed") - dr2.Item("Judged")
                    If drRd.Item("Rd_Name") > 9 Then dr2.Item("Left") = 1
                    dr2.Item("AvgPref") = ds.Tables("Judge").Rows(x).Item("AvgPref")
                    dr2.Item("Assigned") = ActiveInTimeSlot(ds, ds.Tables("Judge").Rows(x).Item("ID"), cboRound.SelectedValue, "Judge")
                    If dr2.Item("Assigned") = True Then dr2.Item("HearingNow") = GetWinString(ds.Tables("Judge").Rows(x).Item("ID")) Else dr2.Item("HearingNow") = "N/A"
                    For y = 1 To TeamsPerRound
                        If Not DataGridView1.CurrentRow.Cells("Team" & y).Value Is System.DBNull.Value Then
                            dr2.Item("Rating" & y) = GetJudgeRating(ds, ds.Tables("Judge").Rows(x).Item("ID"), DataGridView1.CurrentRow.Cells("Team" & y).Value, drRd.Item("JudgePlaceScheme"))
                        Else
                            dr2.Item("Rating" & y) = 0
                        End If
                    Next y
                    If drRd.Item("Flighting") > 1 Then 'add additional ratings if flighting
                        If Not DataGridView1.CurrentRow.Cells("Room").Value Is System.DBNull.Value Then
                            ratCt = y
                            fdOtherRooms = dtDummy.Select("Room=" & DataGridView1.CurrentRow.Cells("Room").Value & " and panel<>" & DataGridView1.CurrentRow.Cells("Panel").Value)
                            For z = 0 To fdOtherRooms.Length - 1
                                For y = 1 To TeamsPerRound
                                    If Not fdOtherRooms(z).Item("Team" & y) Is System.DBNull.Value Then
                                        dr2.Item("Rating" & ratCt) = GetJudgeRating(ds, ds.Tables("Judge").Rows(x).Item("ID"), fdOtherRooms(z).Item("Team" & y), drRd.Item("JudgePlaceScheme"))
                                    Else
                                        dr2.Item("Rating" & ratCt) = 0
                                    End If
                                    ratCt += 1
                                Next y
                            Next z
                        End If
                    End If
                    dr2.Item("Pref") = dr2.Item("Rating2")
                    If dr2.Item("Rating1") > dr2.Item("rating2") Then dr2.Item("Pref") = dr2.Item("Rating1")
                    dr2.Item("Mutuality") = Math.Abs(dr2.Item("Rating1") - dr2.Item("Rating2"))
                    dr2.Item("FitsCriteria") = True
                    If dr2.Item("Pref") > DataGridView3.Rows(0).Cells(1).Value Then dr2.Item("FitsCriteria") = False
                    If dr2.Item("Mutuality") > DataGridView3.Rows(1).Cells(1).Value Then dr2.Item("FitsCriteria") = False
                    If chkHearOnce.Checked = True Then
                        If JudgedBefore(ds, dr2.Item("ID"), DataGridView1.CurrentRow.Cells("Panel").Value) = True Then dr2.Item("FitsCriteria") = False
                    End If
                    If ds.Tables("Judge").Rows(x).Item("Timeslot" & drRd.Item("Timeslot")) Is System.DBNull.Value Then ds.Tables("Judge").Rows(x).Item("Timeslot" & drRd.Item("Timeslot")) = False

                    If ds.Tables("Judge").Rows(x).Item("Timeslot" & drRd.Item("Timeslot").ToString.Trim) = "" Then ds.Tables("Judge").Rows(x).Item("Timeslot" & drRd.Item("Timeslot").ToString.Trim) = "False"
                    dr2.Item("timeavail") = ds.Tables("Judge").Rows(x).Item("Timeslot" & drRd.Item("Timeslot").ToString.Trim)
                    dr2.Item("JudgedBefore") = JudgedBefore(ds, ds.Tables("Judge").Rows(x).Item("ID"), DataGridView1.CurrentRow.Cells("Panel").Value)
                    dt.Rows.Add(dr2)
                End If
        Next x
        DataGridView2.Columns.Clear()
        dt.DefaultView.Sort = "Assigned, FitsCriteria desc, Left desc, Pref"
        DataGridView2.AutoGenerateColumns = True
        DataGridView2.DataSource = dt
        DataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader
        DataGridView2.Columns("ID").Visible = False
        DataGridView2.Columns("Assigned").SortMode = DataGridViewColumnSortMode.Automatic
        DataGridView2.Columns("FitsCriteria").SortMode = DataGridViewColumnSortMode.Automatic
        If drRd.Item("JudgePlaceScheme").toupper = "ASSIGNEDRATING" And drRd.Item("Flighting") = 1 Then
            DataGridView2.Columns("Pref").HeaderText = "Rating"
            DataGridView2.Columns("Mutuality").Visible = False
            DataGridView2.Columns("AvgPref").Visible = False
            For x = 0 To DataGridView2.Columns.Count - 1
                If InStr(DataGridView2.Columns(x).Name.ToString.ToUpper, "RATING") > 0 Then
                    DataGridView2.Columns(x).Visible = False
                End If
            Next x
        End If
        Call MarkJudgeEligibility()
        DataGridView2.CurrentCell = Nothing
        If DataGridView2.RowCount > 0 Then DataGridView2.Rows(0).Selected = False
        If chkJudgeSchool.Checked = True Then
            DataGridView2.Columns("Owed").Visible = False
            DataGridView2.Columns("Judged").Visible = False
            DataGridView2.Columns("Name").AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            DataGridView2.Columns("Name").Width = 130
        End If
        str &= "End Loop: " & Now.Second & " " & Now.Millisecond & Chr(10) & Chr(10)
        'MsgBox(str)
    End Sub
    Function GetWinString(ByVal Judge As Integer) As String
        GetWinString = ""
        Dim dt As DataTable
        dt = DataGridView1.DataSource
        Dim dr As DataRow()
        dr = dt.Select("Judge1=" & Judge)
        If dr.Length > 0 Then GetWinString = Mid(dr(0).Item("TeamName1"), 1, 1) & "-" & Mid(dr(0).Item("TeamName2"), 1, 1)
    End Function
    Sub MarkJudgeEligibility()
        Dim x As Integer
        For x = 0 To DataGridView2.RowCount - 1
            If ActiveInTimeSlot(ds, DataGridView2.Rows(x).Cells("ID").Value, cboRound.SelectedValue, "Judge") = True Then
                DataGridView2.Rows(x).DefaultCellStyle.BackColor = Color.LightBlue
            End If
            If JudgeSameSchool(ds, DataGridView2.Rows(x).Cells("ID").Value, DataGridView1.CurrentRow.Cells("Panel").Value) = True Then
                DataGridView2.Rows(x).DefaultCellStyle.BackColor = Color.Red
            End If
            If DataGridView2.Rows(x).Cells("Pref").Value = 999 Then
                DataGridView2.Rows(x).DefaultCellStyle.BackColor = Color.Red
            End If
        Next
    End Sub
    Sub MarkTeamInAction()
        For x = 0 To dtTeams.Rows.Count - 1
            If ActiveInTimeSlot(ds, dtTeams.Rows(x).Item("Competitor"), cboRound.SelectedValue, "ENTRY") = True Then
                dtTeams.Rows(x).Item("DebatingNow") = True
            Else
                dtTeams.Rows(x).Item("DebatingNow") = False
            End If
        Next x
    End Sub
    Sub ColorTeamsInAction()
        For x = 0 To DataGridView2.RowCount - 1
            DataGridView2.Rows(x).DefaultCellStyle.BackColor = Color.White
            If DataGridView2.Rows(x).Cells("DebatingNow").Value = False Then
                DataGridView2.Rows(x).DefaultCellStyle.BackColor = Color.LightBlue
            End If
        Next
    End Sub
    Sub LoadTeams()
        'update underlying datatable
        Call MarkTeamInAction()
        'datagrid view settings
        dtTeams.DefaultView.Sort = "DebatingNow asc, seed asc"
        DataGridView2.AutoGenerateColumns = True
        DataGridView2.DataSource = dtTeams
        'shrink the headers
        For x = 1 To DataGridView2.Columns.Count - 1
            If DataGridView2.Columns(x).HeaderText = "Hi-Lo team speaker points" Then DataGridView2.Columns(x).HeaderText = "HL pts"
            If DataGridView2.Columns(x).HeaderText = "Total team speaker points" Then DataGridView2.Columns(x).HeaderText = "TotPts"
            If DataGridView2.Columns(x).HeaderText = "Speaker ranks" Then DataGridView2.Columns(x).HeaderText = "Ranks"
            If DataGridView2.Columns(x).HeaderText = "2x HL points" Then DataGridView2.Columns(x).HeaderText = "2x HL"
            If DataGridView2.Columns(x).HeaderText = "Opposition Wins" Then DataGridView2.Columns(x).HeaderText = "OppWins"
            If DataGridView2.Columns(x).HeaderText = "Judge Variance" Then DataGridView2.Columns(x).HeaderText = "JudVar"
            If InStr(DataGridView2.Columns(x).HeaderText.ToUpper, "JUDGE") > 0 Then DataGridView2.Columns(x).HeaderText = "JudVar"
            If DataGridView2.Columns(x).HeaderText.Length > 12 Then DataGridView2.Columns(x).HeaderText = Mid(DataGridView2.Columns(x).HeaderText.ToString, 1, 12)
        Next x
        DataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing
        If DataGridView2.ColumnCount < 9 Then
            DataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        Else
            DataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader
        End If
        DataGridView2.Columns("CompetitorName").AutoSizeMode = DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader
        DataGridView2.Columns("Competitor").Visible = False
        DataGridView2.Columns("DebatingNow").HeaderText = "Assigned"
        DataGridView2.Columns("DebatingNow").SortMode = DataGridViewColumnSortMode.Automatic
        DataGridView2.Columns("CompetitorName").HeaderText = "Team name"
        'customize the sides column header
        Dim drRd As DataRow : drRd = ds.Tables("Round").Rows.Find(cboRound.SelectedValue)
        If getEventSetting(ds, drRd.Item("Event"), "TeamsPerRound") = 2 Then
            DataGridView2.Columns("Sides").HeaderText = "Side due"
        Else
            DataGridView2.Columns("Sides").HeaderText = "Side report"
        End If
        'repaint to mark whether they have been assigned
        Call ColorTeamsInAction()
        'clear the grid so nothing is selected
        DataGridView2.CurrentCell = Nothing
        DataGridView2.Rows(0).Selected = False
    End Sub
    Sub LoadRooms()

        DataGridView2.Columns.Clear()

        Dim dgvc As New DataGridViewTextBoxColumn
        dgvc.Name = "ID"
        dgvc.DataPropertyName = "ID"
        DataGridView2.Columns.Add(dgvc)
        DataGridView2.Columns(0).Visible = False

        dgvc = New DataGridViewTextBoxColumn
        dgvc.Name = "RoomName" : dgvc.DataPropertyName = "RoomName" : dgvc.HeaderText = "RoomName"
        DataGridView2.Columns.Add(dgvc)

        dgvc = New DataGridViewTextBoxColumn
        dgvc.Name = "Quality" : dgvc.DataPropertyName = "Quality" : dgvc.HeaderText = "Quality"
        DataGridView2.Columns.Add(dgvc)

        dgvc = New DataGridViewTextBoxColumn
        dgvc.Name = "Capacity" : dgvc.DataPropertyName = "Capacity" : dgvc.HeaderText = "Capacity"
        DataGridView2.Columns.Add(dgvc)

        Dim dgvc2 As New DataGridViewCheckBoxColumn
        dgvc2.Name = "Inactive" : dgvc2.DataPropertyName = "Inactive" : dgvc2.HeaderText = "Inactive"
        dgvc2.SortMode = DataGridViewColumnSortMode.Automatic
        DataGridView2.Columns.Add(dgvc2)

        Dim drRound As DataRow
        drRound = ds.Tables("Round").Rows.Find(cboRound.SelectedValue)

        dgvc2 = New DataGridViewCheckBoxColumn
        dgvc2.Name = "DivisionAvailable" : dgvc2.DataPropertyName = "DivisionAvailable" : dgvc2.HeaderText = "EventOK"
        dgvc2.SortMode = DataGridViewColumnSortMode.Automatic
        DataGridView2.Columns.Add(dgvc2)

        dgvc2 = New DataGridViewCheckBoxColumn
        dgvc2.Name = "TimeAvailable" : dgvc2.DataPropertyName = "TimeAvailable" : dgvc2.HeaderText = "TimeOK"
        dgvc2.SortMode = DataGridViewColumnSortMode.Automatic
        DataGridView2.Columns.Add(dgvc2)

        dgvc2 = New DataGridViewCheckBoxColumn
        dgvc2.Name = "InUse" : dgvc2.DataPropertyName = "InUse" : dgvc2.HeaderText = "In Use"
        dgvc2.SortMode = DataGridViewColumnSortMode.Automatic
        DataGridView2.Columns.Add(dgvc2)

        DataGridView2.AutoGenerateColumns = False
        DataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DataGridView2.Columns("RoomName").AutoSizeMode = DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader

        Dim dt As New DataTable
        dt.Columns.Add("ID", System.Type.GetType("System.Int64"))
        dt.Columns.Add("RoomName", System.Type.GetType("System.String"))
        dt.Columns.Add("Quality", System.Type.GetType("System.Int16"))
        dt.Columns.Add("Capacity", System.Type.GetType("System.Int16"))
        dt.Columns.Add("Inactive", System.Type.GetType("System.Boolean"))
        dt.Columns.Add("DivisionAvailable", System.Type.GetType("System.Boolean"))
        dt.Columns.Add("TimeAvailable", System.Type.GetType("System.Boolean"))
        dt.Columns.Add("InUse", System.Type.GetType("System.Boolean"))
        
        Dim dr2 As DataRow
        For x = 0 To ds.Tables("Room").Rows.Count - 1
            If ds.Tables("Room").Rows(x).Item("Inactive") = False Then
                dr2 = dt.NewRow
                dr2.Item("ID") = ds.Tables("Room").Rows(x).Item("ID")
                dr2.Item("RoomName") = ds.Tables("Room").Rows(x).Item("RoomName")
                dr2.Item("Quality") = ds.Tables("Room").Rows(x).Item("Quality")
                dr2.Item("Capacity") = ds.Tables("Room").Rows(x).Item("Capacity")
                dr2.Item("Inactive") = ds.Tables("Room").Rows(x).Item("Inactive")
                If ds.Tables("Room").Rows(x).Item("Event" & drRound.Item("Event")) = "" Then ds.Tables("Room").Rows(x).Item("Event" & drRound.Item("Event")) = False
                dr2.Item("DivisionAvailable") = ds.Tables("Room").Rows(x).Item("Event" & drRound.Item("Event"))
                If ds.Tables("Room").Rows(x).Item("TimeSlot" & drRound.Item("TimeSlot")) = "" Then ds.Tables("Room").Rows(x).Item("TimeSlot" & drRound.Item("TimeSlot")) = False
                dr2.Item("TimeAvailable") = ds.Tables("Room").Rows(x).Item("TimeSlot" & drRound.Item("TimeSlot"))
                dr2.Item("InUse") = RoomActiveInTimeSlot(ds, ds.Tables("Room").Rows(x).Item("ID"), drRound.Item("ID"))
                dt.Rows.Add(dr2)
            End If
        Next x

        dt.DefaultView.Sort = "Inuse asc, inactive asc, divisionavailable desc, timeavailable desc, quality asc, roomname asc"
        DataGridView2.DataSource = dt
        DataGridView2.CurrentCell = Nothing
        Call MarkRoomsInUse()

    End Sub
    Sub MarkRoomsInUse()
        Dim markit As Boolean
        For x = 0 To DataGridView2.RowCount - 1
            markit = False
            If DataGridView2.Rows(x).Cells("InUse").Value = True Then markit = True
            If DataGridView2.Rows(x).Cells("Inactive").Value = True Then markit = True
            If DataGridView2.Rows(x).Cells("DivisionAvailable").Value = False Then markit = True
            If DataGridView2.Rows(x).Cells("TimeAvailable").Value = False Then markit = True
            If markit = True Then DataGridView2.Rows(x).DefaultCellStyle.BackColor = Color.Red
        Next
    End Sub
    Private Sub butDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butDelete.Click
        Call Deleter()
        'load grid 2
        If EditMode = "Judge" Then Call LoadJudges()
        If EditMode = "Team" Then Call LoadTeams()
        'load grid 1
        Call ShowThePairing()
    End Sub
    Sub Deleter()
        Dim dr As DataRow
        If EditMode = "Room" Then
            dr = ds.Tables("Panel").Rows.Find(DataGridView1.CurrentRow.Cells("Panel").Value)
            dr.Item("Room") = System.DBNull.Value
            Exit Sub
        End If
        'Find the judge or team ID to delete
        Dim DeleteId As Integer = DataGridView1.CurrentRow.Cells(DataGridView1.CurrentCell.ColumnIndex - 1).Value
        'FIRST, MAKE THE CHANGE TO THE MASTER DATASET
        Dim DT As DataTable
        If EditMode = "Judge" Then
            DT = PullBallotsByRound(ds.Copy, "JUDGE", DeleteId, cboRound.SelectedValue)
            For x = 0 To DT.Rows.Count - 1
                dr = ds.Tables("Ballot").Rows.Find(DT.Rows(x).Item("ID"))
                If chkSingleChangeFlights.Checked = True Then 'only process the specific round for a flight
                    If dr.Item("Panel") = DataGridView1.CurrentRow.Cells("Panel").Value Then
                        dr.Item("Judge") = -99
                    End If
                Else
                    dr.Item("Judge") = -99
                End If
            Next
        End If
        If EditMode = "Team" Then
            Dim fdBalScores As DataRow()
            DT = PullBallotsByRound(ds.Copy, "ENTRY", DeleteId, cboRound.SelectedValue)
            For x = 0 To DT.Rows.Count - 1
                dr = ds.Tables("Ballot").Rows.Find(DT.Rows(x).Item("ID"))
                fdBalScores = ds.Tables("Ballot_Score").Select("Ballot=" & dr.Item("ID"))
                For y = fdBalScores.Length - 1 To 0 Step -1 : fdBalScores(y).Delete() : Next y
                dr.Item("Entry") = -99
            Next
            ds.Tables("Ballot_Score").AcceptChanges()
        End If
    End Sub
    Private Sub butAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butAdd.Click
        Dim q As Integer
        'check that one cell on the pairing has been clicked
        If DataGridView1.CurrentRow Is Nothing Then
            MsgBox("No debate on the left-hand pairing grid has been selected.  Please select an item and try again.", MsgBoxStyle.OkOnly)
            Exit Sub
        End If
        'check that one row on the add grid has been clicked
        If DataGridView2.CurrentRow Is Nothing Then
            MsgBox("No item to add has been selected on the right-hand grid.  Please select an item and try again.", MsgBoxStyle.OkOnly)
            Exit Sub
        End If
        If EditMode = "Team" Then
            If DataGridView2.CurrentRow.Cells("DebatingNow").Value Is System.DBNull.Value Then DataGridView2.CurrentRow.Cells("DebatingNow").Value = False
            If DataGridView2.CurrentRow.Cells("DebatingNow").Value = True Then
                Label1.Text = "That team is already scheduled to debate in this time slot.  Please select a differen team and try again."
                Exit Sub
            End If
        End If
        Dim dummy As String = "OK"
        'get the panel
        Dim panel As Integer = DataGridView1.CurrentRow.Cells("Panel").Value
        'validate panel first; if poorly structured show error
        dummy = ValidatePanel(ds, panel)
        If dummy <> "OK" Then
            Label1.Text = "Change NOT made..." & dummy
            Exit Sub
        End If
        Dim struck As Boolean
        If EditMode = "Judge" Then
            If DataGridView2.CurrentRow.Cells("TimeAvail").Value Is System.DBNull.Value Then DataGridView2.CurrentRow.Cells("TimeAvail").Value = False
            If DataGridView2.CurrentRow.Cells("TimeAvail").Value = False Then q = MsgBox("Judge is NOT available for timeslot.  Place anyway?", MsgBoxStyle.YesNo)
            If q = vbNo Then Exit Sub
            If DataGridView2.CurrentRow.Cells("FitsCriteria").Value = False Then q = MsgBox("Judge does not fit stated criteria.  Place anyway?", MsgBoxStyle.YesNo)
            If q = vbNo Then Exit Sub
            If DataGridView2.CurrentRow.Cells("Assigned").Value = True Then q = MsgBox("Judge has already been assigned in this timeslot.  Place anyway?", MsgBoxStyle.YesNo)
            If q = vbNo Then Exit Sub
            If DataGridView2.CurrentRow.Cells("Left").Value <= 0 Then q = MsgBox("Judge has no rounds of commitment left.  Place anyway?", MsgBoxStyle.YesNo)
            If q = vbNo Then Exit Sub
            If DataGridView2.CurrentRow.Cells("JudgedBefore").Value = True And chkHearOnce.Checked = True Then q = MsgBox("Judge has heard one or both teams before.  Place anyway?", MsgBoxStyle.YesNo)
            If q = vbNo Then Exit Sub
            struck = False
            For x = 1 To DataGridView2.ColumnCount - 1
                If InStr(DataGridView2.Columns(x).Name.ToUpper, "RATING") > 0 And Not DataGridView2.CurrentRow.Cells(x).Value Is System.DBNull.Value Then
                    If DataGridView2.CurrentRow.Cells(x).Value = 999 Then
                        struck = True
                    End If
                End If
            Next
            If struck = True Then q = MsgBox("Judge has a conflict with one of the teams on this panel; you REALLY should probably not place them.  Place anyway?", MsgBoxStyle.YesNo)
            If q = vbNo Then Exit Sub
            Dim drRD As DataRow : drRD = ds.Tables("Round").Rows.Find(cboRound.SelectedValue)
            If DataGridView1.CurrentRow.Cells("Room").Value Is System.DBNull.Value Then DataGridView1.CurrentRow.Cells("Room").Value = 0
            If drRD.Item("Flighting") > 1 And chkSingleChangeFlights.Checked = False And DataGridView1.CurrentRow.Cells("Room").Value > 0 Then
                Dim fdPrgs As DataRow()
                Dim dtDummy As DataTable : dtDummy = DataGridView1.DataSource
                fdPrgs = dtDummy.Select("Room=" & DataGridView1.CurrentRow.Cells("Room").Value)
                For y = 0 To fdPrgs.Length - 1
                    dummy = AddJudgeToPanel(ds, fdPrgs(y).Item("Panel"), DataGridView2.CurrentRow.Cells("ID").Value, fdPrgs(y).Item("Flight"))
                    If dummy <> "OK" Then
                        Label1.Text = "Change NOT made..." & dummy
                        Exit Sub
                    End If
                Next y
            Else
                If DataGridView1.CurrentRow.Cells("Flight").Value Is System.DBNull.Value Then DataGridView1.CurrentRow.Cells("Flight").Value = 1
                dummy = AddJudgeToPanel(ds, panel, DataGridView2.CurrentRow.Cells("ID").Value, DataGridView1.CurrentRow.Cells("Flight").Value)
            End If
        End If
        If EditMode = "Team" Then dummy = AddTeamToPanel(ds, panel, DataGridView2.CurrentRow.Cells("Competitor").Value, GetSide)
        If EditMode = "Room" Then dummy = AddRoomToPanel(ds, panel, DataGridView2.CurrentRow.Cells("ID").Value)
        If dummy <> "OK" Then
            Label1.Text = "Change NOT made..." & dummy
            Exit Sub
        End If
        Call ShowThePairing()
        If EditMode = "Judge" Then Call LoadJudges()
        If EditMode = "Team" Then Call LoadTeams()
        If EditMode = "Room" Then Call LoadRooms()
        Label1.Text = "Changes completed."
        DataGridView2.CurrentCell = Nothing : DataGridView2.Rows(0).Selected = False
    End Sub
    Sub ClearTheGrids()
        For x = 0 To DataGridView1.RowCount - 1
            DataGridView1.Rows(x).Selected = False
        Next
        For x = 0 To DataGridView2.RowCount - 1
            DataGridView2.Rows(x).Selected = False
        Next
    End Sub
    Function GetSide() As Integer
        Dim str As String = DataGridView1.Columns(DataGridView1.CurrentCell.ColumnIndex).Name
        str = Mid(str, str.Length, 1)
        Return Val(str)
    End Function
    Private Sub butDeleteRow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butDeleteRow.Click
        Dim Panel As Integer = DataGridView1.CurrentRow.Cells("Panel").Value
        Dim drPanel As DataRow
        drPanel = ds.Tables("Panel").Rows.Find(Panel)
        drPanel.Delete()
        If EditMode = "Judge" Then Call LoadJudges()
        If EditMode = "Team" Then Call LoadTeams()
        Call ShowThePairing()
    End Sub
    Sub dg2Sorted() Handles DataGridView2.Sorted
        If EditMode = "Judge" Then MarkJudgeEligibility()
        If EditMode = "Team" Then ColorTeamsInAction()
        If EditMode = "Room" Then MarkRoomsInUse()
    End Sub
    Sub dg1Sorted() Handles DataGridView1.Sorted
        Call MarkBlanks()
    End Sub

    Private Sub butAddPanel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butAddPanel.Click
        Call AddPanel(ds, cboRound.SelectedValue, 0)
        Call ShowThePairing()
        If EditMode = "Judge" Then Call LoadJudges()
        If EditMode = "Team" Then Call LoadTeams()
    End Sub
    Private Sub datagridview2_CellPainting(ByVal sender As Object, ByVal e As DataGridViewCellPaintingEventArgs) Handles DataGridView2.CellPainting
        'If DataGridView2.ColumnCount < 9 Then Exit Sub
        If ((e.RowIndex = -1) AndAlso (e.ColumnIndex >= 2)) Then
            Me.DataGridView2.ColumnHeadersHeightSizeMode = AutoSize
            e.PaintBackground(e.ClipBounds, True)
            Dim rect As Rectangle = Me.DataGridView2.GetColumnDisplayRectangle(e.ColumnIndex, True)
            Dim titleSize As Size = TextRenderer.MeasureText(e.Value.ToString, e.CellStyle.Font)
            If (Me.DataGridView2.ColumnHeadersHeight < titleSize.Width) Then
                Me.DataGridView2.ColumnHeadersHeight = titleSize.Width
            End If
            e.Graphics.TranslateTransform(0, titleSize.Width)
            e.Graphics.RotateTransform(-90.0!)
            e.Graphics.DrawString(e.Value.ToString, Me.Font, Brushes.Black, New PointF(rect.Y, rect.X))
            e.Graphics.RotateTransform(90.0!)
            e.Graphics.TranslateTransform(0, (titleSize.Width * -1))
            DataGridView2.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            e.Handled = True
        End If
    End Sub
    Sub LoadJudgeSettings()
        Dim dt As New DataTable
        dt.Columns.Add("Criteria", System.Type.GetType("System.String"))
        dt.Columns.Add("Value", System.Type.GetType("System.Int16"))
        Dim dr As DataRow
        dr = dt.NewRow : dr.Item("Criteria") = "Max Pref" : dr.Item("Value") = 50 : dt.Rows.Add(dr)
        dr = dt.NewRow : dr.Item("Criteria") = "Max Mutuality" : dr.Item("Value") = 30 : dt.Rows.Add(dr)
        DataGridView3.DataSource = dt
    End Sub
    Sub ReLoadJudges() Handles DataGridView3.CellEndEdit
        Call LoadJudges()
    End Sub

    Sub ColorCodeAssignedJudges()

        Dim drRound As DataRow
        drRound = ds.Tables("Round").Rows.Find(cboRound.SelectedValue)

        Dim mut, rat1, rat2, SpaceFlag, PrefMax, MutMax As Integer : Dim dummy As String
        PrefMax = DataGridView3.Rows(0).Cells(1).Value
        MutMax = DataGridView3.Rows(1).Cells(1).Value
        For x = 0 To DataGridView1.Rows.Count - 1
            If drRound.Item("JudgePlaceScheme").toupper = "TEAMRATING" Then
                dummy = DataGridView1.Rows(x).Cells("Balance").Value.trim
                For y = 1 To dummy.Length
                    If Mid(dummy, y, 1) = " " Then
                        mut = Val(Mid(dummy, 1, y - 1))
                        SpaceFlag = y
                    ElseIf Mid(dummy, y, 1) = "-" Then
                        rat1 = Val(Mid(dummy, SpaceFlag + 1, y - SpaceFlag - 1))
                        rat2 = Val(Mid(dummy, y + 1, dummy.Length - y))
                    End If
                Next y
                If rat1 > Int(PrefMax * 0.7) Or rat2 > Int(PrefMax * 0.7) Or mut > Int(MutMax * 0.5) Then DataGridView1.Item("JudgeName1", x).Style.BackColor = Color.LightGreen
                If rat1 > Int(PrefMax * 0.7) And rat2 > Int(PrefMax * 0.7) And mut < Int(MutMax * 0.5) Then DataGridView1.Item("JudgeName1", x).Style.BackColor = Color.LightYellow
                If rat1 > Int(PrefMax * 0.7) And rat2 > Int(PrefMax * 0.7) And mut > Int(MutMax * 0.5) Then DataGridView1.Item("JudgeName1", x).Style.BackColor = Color.Orange
                If mut > MutMax Or rat1 > PrefMax Or rat2 > PrefMax Then DataGridView1.Item("JudgeName1", x).Style.BackColor = Color.Red
            Else
                If DataGridView1.Rows(x).Cells("Balance").Value.trim = "X" Then DataGridView1.Item("JudgeName1", x).Style.BackColor = Color.Red
            End If
        Next x
    End Sub
    Sub ShowRoomsLastRound()
        Dim PriorRound As Integer = GetPriorRound(ds, cboRound.SelectedValue)
        If PriorRound = Nothing Then Exit Sub
        Label1.Text = ""
        Dim drRoom, drRd As DataRow
        drRd = ds.Tables("Round").Rows.Find(cboRound.SelectedValue)
        Dim nTeams As Integer = getEventSetting(ds, drRd.Item("Event"), "TeamsPerRound")
        For x = 1 To nTeams
            If DataGridView1.CurrentRow.Cells("Team" & x).Value Is System.DBNull.Value Then Exit For
            drRoom = ds.Tables("Room").Rows.Find(RoomInPriorRound(ds, DataGridView1.CurrentRow.Cells("Team" & x).Value, cboRound.SelectedValue))
            If drRoom Is Nothing Then
                Label1.Text &= DataGridView1.CurrentRow.Cells("TeamName" & x).Value & " was in N/A last round." & Chr(10)
            Else
                Label1.Text &= DataGridView1.CurrentRow.Cells("TeamName" & x).Value & " was in " & drRoom.Item("RoomName") & " last round." & Chr(10)
            End If
        Next
    End Sub

    Private Sub butAutoRooms_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butAutoRooms.Click
        Dim SideToStay As Integer = 2
        If radRandom.Checked = True Or cboRound.SelectedIndex = 0 Then SideToStay = 0
        If radBracketOrder.Checked = True Then SideToStay = 0
        Call AutoRoomsByBracket(ds, cboRound.SelectedValue, chkDisability.Checked, chkTubUse.Checked, radBracketOrder.Checked, SideToStay)
        Call ShowThePairing()
        Call LoadRooms()
        If cboRound.SelectedIndex = 0 Then Exit Sub
        Label1.Text = RoomMoveReport(ds, cboRound.SelectedValue)
        Label1.Text &= TimeSlotAudit(ds, cboRound.SelectedValue)
        Label1.Text &= " " & AuditRound(ds, cboRound.SelectedValue)
    End Sub
    Private Sub butDumpRooms_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butDumpRooms.Click
        Dim fdPanels As DataRow()
        fdPanels = ds.Tables("Panel").Select("Round=" & cboRound.SelectedValue)
        For x = 0 To fdPanels.Length - 1
            fdPanels(x).Item("Room") = System.DBNull.Value
        Next x
        Call ShowThePairing()
        Call LoadRooms()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Label1.Text = RoomMoveReport(ds, cboRound.SelectedValue)
    End Sub

    Private Sub butAutoJudges_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butAutoJudges.Click
        'fill datagrid with fits column; non-destructive, so will only add the column if it doesn't exit
        'otherwise, it just udpates the column
        Label1.Text = "Figuring fits by debate..." : Label1.Refresh()
        Call AddFitsColumns()

        'load and stack judges
        Label1.Text = "Stacking judges..." : Label1.Refresh()
        Dim dt As New DataTable
        dt.Columns.Add("ID", System.Type.GetType("System.Int64"))
        dt.Columns.Add("Left", System.Type.GetType("System.Int16"))
        dt.Columns.Add("AvgPref", System.Type.GetType("System.Int16"))
        dt.Columns.Add("Assigned", System.Type.GetType("System.Boolean"))
        Dim dr As DataRow : dr = ds.Tables("Round").Rows.Find(cboRound.SelectedValue)

        Dim dr2 As DataRow
        For x = 0 To ds.Tables("Judge").Rows.Count - 1
            If ds.Tables("Judge").Rows(x).Item("Timeslot" & dr.Item("Timeslot").ToString.Trim) Is System.DBNull.Value Then ds.Tables("Judge").Rows(x).Item("Timeslot" & dr.Item("Timeslot").ToString.Trim) = False
            ds.Tables("Judge").Rows(x).Item("Timeslot" & dr.Item("Timeslot").ToString.Trim) = ds.Tables("Judge").Rows(x).Item("Timeslot" & dr.Item("Timeslot").ToString.Trim).trim
            If ds.Tables("Judge").Rows(x).Item("Timeslot" & dr.Item("Timeslot").ToString.Trim) = True Then
                dr2 = dt.NewRow
                dr2.Item("ID") = ds.Tables("Judge").Rows(x).Item("ID")
                dr2.Item("Left") = ds.Tables("Judge").Rows(x).Item("Obligation") + ds.Tables("Judge").Rows(x).Item("Hired") - GetRoundsJudged(ds, ds.Tables("Judge").Rows(x).Item("ID"))
                dr2.Item("AvgPref") = ds.Tables("Judge").Rows(x).Item("AvgPref")
                'if using tabratings make avgpref the ratings
                If dr.Item("JudgePlaceScheme").toupper = "ASSIGNEDRATING" Then
                    dr2.Item("AvgPref") = ds.Tables("Judge").Rows(x).Item("TabRating")
                End If
                dr2.Item("Assigned") = ActiveInTimeSlot(ds, ds.Tables("Judge").Rows(x).Item("ID"), cboRound.SelectedValue, "Judge")
                dt.Rows.Add(dr2)
            End If
        Next
        dt.DefaultView.Sort = "Left DESC, AvgPref DESC"
        If dr.Item("Rd_Name") > 9 Then dt.DefaultView.Sort = "AvgPref DESC"
        If chkJudgeBracket.Checked = True Then dt.DefaultView.Sort = "AvgPref ASC"
        'If chkJudgeBracket.Checked = True And dr.Item("JudgePlaceScheme").toupper = "ASSIGNEDRATING" Then dt.DefaultView.Sort = "TabRating ASC"
        'DataGridView2.AutoGenerateColumns = True
        'DataGridView2.DataSource = dt

        'add seeding to the table
        Label1.Text = "Adding seeds..." : Label1.Refresh()
        Dim dtPairing As DataTable
        dtPairing = DataGridView1.DataSource
        If Not dtPairing.Columns.Contains("Seed") Then
            dtPairing.Columns.Add("Seed", System.Type.GetType("System.Int16"))
        End If
        Call AddSeedToResults(ds, dtPairing, dtTeams, cboRound.SelectedValue)

        'sort debates by hard to place
        DataGridView1.Sort(DataGridView1.Columns("Fits"), System.ComponentModel.ListSortDirection.Ascending)
        'unless doing by bracked
        If chkJudgeBracket.Checked = True Then DataGridView1.Sort(DataGridView1.Columns("Seed"), System.ComponentModel.ListSortDirection.Ascending)

        'If its a flighted round, process separately
        If dr.Item("Flighting") > 1 Then Call FlightingAutoPlace(dt) : Exit Sub

        Dim rat1, rat2 As Integer
        Dim CanPlace As Boolean : Dim drJudge As DataRow
        For z = 1 To dr.Item("JudgesPerPanel") 'loop judgesperpanel
            For x = 0 To DataGridView1.RowCount - 1 'loop debates
                Label1.Text = x & " of " & DataGridView1.RowCount - 1 : Label1.Refresh()
                'don't process if same school
                If SameSchool(ds, DataGridView1.Rows(x).Cells("Team1").Value, DataGridView1.Rows(x).Cells("Team2").Value) = False Or chkPlaceSameSchool.Checked = True Then
                    For y = 0 To dt.DefaultView.Count - 1 'loop judge table
                        CanPlace = True
                        If dt.DefaultView(y).Item("Assigned") = True Then CanPlace = False
                        If JudgeSameSchool(ds, dt.DefaultView(y).Item("ID"), DataGridView1.Rows(x).Cells("Panel").Value) = True Then CanPlace = False
                        If dr.Item("Rd_Name") < 10 Then 'prelim
                            'remaining commitment
                            If dt.DefaultView(y).Item("Left") <= 0 Then CanPlace = False
                            'judgedbefore
                            If CanJudge(ds, dt.DefaultView(y).Item("ID"), DataGridView1.Rows(x).Cells("Panel").Value, chkHearOnce.Checked) = False Then CanPlace = False
                        End If
                        If CanPlace = True Then 'now check ratings
                            rat1 = GetJudgeRating(ds, dt.DefaultView(y).Item("ID"), DataGridView1.Rows(x).Cells("Team1").Value, dr.Item("JudgePlaceScheme"))
                            rat2 = GetJudgeRating(ds, dt.DefaultView(y).Item("ID"), DataGridView1.Rows(x).Cells("Team2").Value, dr.Item("JudgePlaceScheme"))
                            If rat1 <= DataGridView3.Rows(0).Cells(1).Value And rat2 <= DataGridView3.Rows(0).Cells(1).Value And Math.Abs(rat1 - rat2) <= DataGridView3.Rows(1).Cells(1).Value Then
                                ValidatePanel(ds, DataGridView1.Rows(x).Cells("Panel").Value)
                                AddJudgeToPanel(ds, DataGridView1.Rows(x).Cells("Panel").Value, dt.DefaultView(y).Item("ID"), 1)
                                dt.DefaultView(y).Item("Assigned") = True
                                drJudge = ds.Tables("Judge").Rows.Find(dt.DefaultView(y).Item("ID"))
                                Exit For
                            End If
                        End If
                    Next y
                End If
            Next x
        Next z
        Call ShowThePairing()
        Label1.Text = TimeSlotAudit(ds, cboRound.SelectedValue)
        Label1.Text &= " " & AuditRound(ds, cboRound.SelectedValue)
    End Sub
    Sub FlightingAutoPlace(ByVal dt As DataTable)
        'dt is judges
        Dim dr As DataRow : dr = ds.Tables("Round").Rows.Find(cboRound.SelectedValue)
        'rating is rating for each team
        'panel stores the panelID for each debate
        Dim nTeams As Integer = getEventSetting(ds, dr.Item("Event"), "TeamsPerRound")
        Dim nRatings As Integer = dr.Item("JudgesPerPanel") * nTeams
        Dim rat1, rat2, LoopCtr As Integer
        Dim Panel(dr.Item("Flighting")) As Integer
        Dim CanPlace As Boolean
        Dim debSt, debEnd, debDir, panCt As Integer
        Dim fdPanels As DataRow()
        Dim strPanels As String
        Dim dtDummy As DataTable
        Dim GotJudges As Boolean : Dim fdBallots As DataRow()
        'odd team in flight sort from top of dgv, even sort bottom; top=hardest to place debates, bottom=easiest
        Do
            LoopCtr += 1 : If LoopCtr > DataGridView1.RowCount Then Exit Do 'bail if stuck in a loop
            strPanels = "" : panCt = 0
            'add the panels you're placing judges for into the array
            For x = 1 To dr.Item("Flighting")
                'odd flights count up, even flights count down
                If Int(x / 2) <> x / 2 Then
                    debSt = 0 : debEnd = DataGridView1.RowCount - 1 : debDir = 1
                Else
                    debEnd = 0 : debSt = DataGridView1.RowCount - 1 : debDir = -1
                End If
                For y = debSt To debEnd Step debDir
                    GotJudges = False
                    fdBallots = ds.Tables("Ballot").Select("Panel=" & DataGridView1.Rows(y).Cells("Panel").Value)
                    For z = 0 To fdBallots.Length - 1
                        If fdBallots(z).Item("Judge") > -99 Then GotJudges = True
                    Next z
                    If GotJudges = False Then
                        If strPanels = "" Then strPanels = "(Panel=" Else strPanels &= " or panel="
                        strPanels &= DataGridView1.Rows(y).Cells("Panel").Value.ToString
                        panCt += 1
                        Exit For
                    End If
                Next y
                If panCt = 0 Then Exit Do 'will be true if all debates have judges, so exit loop
                If panCt < x Then Exit For 'Did find some debates for judges, but not a full flight, so stop looking (catches the strays at the end)
            Next x
            strPanels &= ")"
            dtDummy = DataGridView1.DataSource
            fdPanels = dtDummy.Select(strPanels) 'now all panels to pair are stored in fdpanels datarow array
            'don't process if same school
            For z = 1 To dr.Item("JudgesPerPanel") 'loop judgesperpanel
                For y = 0 To dt.DefaultView.Count - 1 'loop judge table
                    'don't place if assigned
                    CanPlace = True
                    If dt.DefaultView(y).Item("Assigned") = True Then CanPlace = False
                    'check judge can hear
                    If dr.Item("Rd_Name") < 10 Then 'prelim
                        'remaining commitment
                        If dt.DefaultView(y).Item("Left") <= 0 Then
                            CanPlace = False
                        Else
                            'judgedbefore
                            For x = 0 To fdPanels.Length - 1
                                If CanJudge(ds, dt.DefaultView(y).Item("ID"), fdPanels(x).Item("Panel"), chkHearOnce.Checked) = False Then CanPlace = False : Exit For
                            Next x
                        End If
                    End If
                    'check same school -> NOT necessary; part of the CanJudge routine
                    'If chkPlaceSameSchool.Checked = False And CanPlace = True Then
                    'For x = 0 To fdPanels.Length - 1
                    'If JudgeSameSchool(ds, dt.DefaultView(y).Item("ID"), fdPanels(x).Item("Panel")) = True Then CanPlace = False : Exit For
                    'Next x
                    'End If
                    'now check ratings
                    If CanPlace = True Then
                        For x = 0 To fdPanels.Length - 1
                            rat1 = GetJudgeRating(ds, dt.DefaultView(y).Item("ID"), fdPanels(x).Item("Team1"), dr.Item("JudgePlaceScheme"))
                            rat2 = GetJudgeRating(ds, dt.DefaultView(y).Item("ID"), fdPanels(x).Item("Team2"), dr.Item("JudgePlaceScheme"))
                            If rat1 > DataGridView3.Rows(0).Cells(1).Value Or rat2 > DataGridView3.Rows(0).Cells(1).Value Or Math.Abs(rat1 - rat2) > DataGridView3.Rows(1).Cells(1).Value Then
                                CanPlace = False : Exit For
                            End If
                        Next x
                        If CanPlace = True Then
                            dt.DefaultView(y).Item("Assigned") = True
                            For x = 0 To fdPanels.Length - 1
                                ValidatePanel(ds, fdPanels(x).Item("Panel"))
                                AddJudgeToPanel(ds, fdPanels(x).Item("Panel"), dt.DefaultView(y).Item("ID"), x + 1)
                            Next x
                            Exit For
                        End If
                    End If
                Next y
            Next z
        Loop

        Call ShowThePairing()
        DataGridView1.Sort(DataGridView1.Columns("Judge1"), System.ComponentModel.ListSortDirection.Ascending)
        Label1.Text = TimeSlotAudit(ds, cboRound.SelectedValue)
        Label1.Text &= " " & AuditRound(ds, cboRound.SelectedValue)
    End Sub
    Private Sub butDumpJudges_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butDumpJudges.Click
        Call DumpAllJudges(ds, cboRound.SelectedValue)
        Call ShowThePairing()
    End Sub

    Private Sub butBye_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butBye.Click
        'check that its OK to assign
        If DataGridView2.CurrentCell Is Nothing Then
            MsgBox("Please select a team from the right-hand box and try again.", MsgBoxStyle.OkOnly)
            Exit Sub
        End If
        If DataGridView2.CurrentRow.Cells("DebatingNow").Value = True Then
            MsgBox("You can only assign a bye to a team not currently paired in a debate.  To assign a bye to a team currently assigned, first delete the team from the left-hand column.", MsgBoxStyle.OkOnly)
            Exit Sub
        End If
        Dim x As Integer
        If HadBye(ds, cboRound.SelectedValue, DataGridView2.CurrentRow.Cells("Competitor").Value) > 0 Then
            x = MsgBox("This team has already received a bye; assign them another one anyway?", MsgBoxStyle.YesNo)
            If x = vbNo Then Exit Sub
        End If
        Dim dummy As String = AssignBye(ds, -1, DataGridView2.CurrentRow.Cells("Competitor").Value, cboRound.SelectedValue)
        If dummy <> "" Then
            MsgBox(dummy, MsgBoxStyle.OkOnly)
        End If
        Call ShowThePairing()
    End Sub

    Private Sub butPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butPrint.Click
        If chkShowRecords.Checked = True Then
            Dim q As Integer = MsgBox("This copy of the pairing shows teams records.  If you want to print the round without team records, uncheck the SHOW RECORDS box in the LOAD SETTINGS grid (bottom center) and reload the round.  Continue print job with team records?", MsgBoxStyle.YesNo)
            If q = vbNo Then Exit Sub
        End If
        'pull the start time
        Dim drRd, drTimeSlot As DataRow
        drRd = ds.Tables("Round").Rows.Find(cboRound.SelectedValue)
        drTimeSlot = ds.Tables("TimeSlot").Rows.Find(drRd.Item("Timeslot"))
        If ds.Tables("timeslot").Columns.Contains("start") = False Then
            ds.Tables("Timeslot").Columns.Add("Start", System.Type.GetType("System.DateTime"))
        End If
        Try
            If drTimeSlot.Item("Start") Is System.DBNull.Value Then drTimeSlot.Item("Start") = Now.AddMinutes(15)
        Catch
            drTimeSlot.Item("Start") = Now.AddMinutes(15)
        End Try
        Dim dtDummy As DateTime = "#" & drTimeSlot.Item("Start") & "#"
        Dim strStartTime = dtDummy.ToString("t")
        If drRd.Item("Flighting") > 1 Then
            For x = 2 To drRd.Item("Flighting")
                dtDummy = dtDummy.AddMinutes(40)
                strStartTime &= "/" & dtDummy.ToString("t")
            Next x
        End If
        strStartTime = InputBox("Enter Start Time:", "Start Time", strStartTime)
        'TimeString(Now.ToShortTimeString)
        Dim defCols(DataGridView1.Columns.Count) As Boolean
        For x = 0 To DataGridView1.ColumnCount - 1
            If DataGridView1.Columns(x).Visible = True Then defCols(x) = True
            If DataGridView1.Columns(x).Name = "Balance" Then defCols(x) = False
            If DataGridView1.Columns(x).Name = "Fits" Then defCols(x) = False
        Next x
        Dim frm As New frmPrint(DataGridView1, ds.Tables("Tourn").Rows(0).Item("TournName").trim & Chr(13) & Chr(10) & cboRound.Text & Chr(13) & Chr(10) & "START TIME: " & strStartTime & Chr(13), defCols)
        frm.ShowDialog()
    End Sub

    Private Sub butRoundAudit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butRoundAudit.Click
        Label1.Text = TimeSlotAudit(ds, cboRound.SelectedValue)
        Label1.Text &= AuditRound(ds, cboRound.SelectedValue)
    End Sub

    Private Sub chkPlaceSameSchool_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkPlaceSameSchool.CheckedChanged

    End Sub

    Private Sub butJudgePlaceHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butJudgePlaceHelp.Click
        Dim strInfo As String = ""
        strInfo &= "There are 3 basic judge placement options this screen will support. " & Chr(10) & Chr(10)
        strInfo &= "If you are using (1) MUTUAL PREFERENCE judging, set the maximum ordinal percentiles and mutuality that will be acceptable.  A 40 for mutuality means that the computer should not place a judge lower than one in the top 40% of the team ratings.  A 20 for mutuality means there can be no more than a 20-point difference in the ordinal percentiles; (the computer could place a 20%-35% judge but not a 5%-30%)." & Chr(10) & Chr(10)
        strInfo &= "The computer will then stack the debates from the hardest to place judges in to the easiest, and stack the judges from the hardest to place to the easiest, and start at the top of both lists and work down for each debate.  That is, the comptuer will start with the hardest-to-fit debate and the hardest-to-fit judge, and place the first judge that fits the specified criteria (in the case of ties, using judges with more round of commitment first, and not using any judge beyond their commiment).  " & Chr(10) & Chr(10)
        strInfo &= "This will MAXIMIZE the use of the judge pool, but will tend to use more judges from the middle, and note that it will not give any greater weight to break rounds than any other debate.  More advanced users might want to use Gary Larson's STA judge placement system, which is available from the main screen.  " & Chr(10) & Chr(10)
        strInfo &= "If you are using (2) TAB-ASSIGNED RATINGS, the computer will still honor the criteria (note that it will be on a different scale; if you use a 1-10 rating system and set a maximum score of 11 all judges will meet the criteria), and still start with the hardest-to-fit debates, but will use tab-assigned judge ratings instead of team ordinal ranks. " & Chr(10) & Chr(10)
        strInfo &= "If you select (3) RANDOM , the computer will honor any preclusions, and then simply use the judges in a way that maximizes commitment (that is, it will lose no rounds of available judging). " & Chr(10) & Chr(10)
        strInfo &= "By default, the computer will not place judges in same-school debates.  If you click the PLACE SAME SCHOOL DEBATES button the computer will assign judges to those debates." & Chr(10) & Chr(10)
        strInfo &= "By default, judges are only allowed to hear a team once.  Un-checking the ONLY HEAR A TEAM ONCE box will allow judges to hear a team a second time (this button should probably be un-checked for elims)." & Chr(10) & Chr(10)
        strInfo &= "If you check the JUDGES IN BRACKET ORDER box the computer will stack the judges from best to worst, based on either their average mutual preference score or tab assigned rating, start with the top seed, and place the highest-ranked judge who fits the criteria for the stated debate. " & Chr(10) & Chr(10)
        strInfo &= "Note that the judge placement scheme for any given round is set up on the ROUNDS setup screen off of the main menu. " & Chr(10) & Chr(10)
        strInfo &= "If you check the SINGLE CHANGE FLIGHTS any manual changes will only be processed for the individual flight you are modifying.  Otherwise, all changes (deletions and additions) will process on all flights that involve the judge for the selected round.  If you are not flighting judges for the round, this setting won't matter. " & Chr(10) & Chr(10)
        strInfo &= "The BEST FIT button will show the debates that the selected judge in the right-hand grid is a good fit for.  " & Chr(10) & Chr(10)
        MsgBox(strInfo, , "How to set up the tournament")
    End Sub
    Sub GoToAdd() Handles DataGridView2.Click
        butAdd.Focus()
        If EditMode = "Judge" Then Call BalanceUpdater()
    End Sub

    Private Sub butLoadSettingHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butLoadSettingHelp.Click
        Dim strInfo As String = ""
        strInfo &= "These settings will customize how the pairing loads. " & Chr(10) & Chr(10)
        strInfo &= "FULL NAMES will display full last names instead of team acronyms." & Chr(10) & Chr(10)
        strInfo &= "JUDGE USE will display the number of rounds a judge is committed to hear and how many they have already judged.  The column will display as rounds left/rounds judged/rounds committed, so 3/4/1 means 3 rounds left, commited for 4, already heard 1.  Note that elimination rounds will be included, so these figures are probably not accurate once elimination rounds begin." & Chr(10) & Chr(10)
        strInfo &= "TEAM RECORDS will show team wins and losses in addition to their team name. For preset rounds, team ratings will display instead of wins and losses." & Chr(10) & Chr(10)
        strInfo &= "N JUDGE FITS will show how many judges are eligible to hear a given debate (thus identifying the most difficult to place rounds) given constraints and the judge placement criteria.  This is most useful when using mutual preference judging." & Chr(10) & Chr(10)
        strInfo &= "JUDGE COLOR will color code judge assignments.  If mutual preference judging is used, cooler colors (green) indicate placements that easily fit the criteria while hotter colors (red) indicate placements that are borderline or exceed the criteria.  For all systems, RED will indicate a serious judge problem." & Chr(10) & Chr(10)
        MsgBox(strInfo, , "Secret decoder ring for load settings")
    End Sub

    Private Sub butJudgeReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butJudgeReport.Click

        'ugly, but functional

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

    Private Sub butOrdReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butOrdReport.Click
        If DataGridView1.DataSource Is Nothing Then
            MsgBox("Load a round into the left-hand column first.")
            Exit Sub
        End If
        DataGridView2.DataSource = OrdReport(ds, DataGridView1.DataSource, DataGridView3.Rows(1).Cells(1).Value)
    End Sub
    Sub BalanceUpdater()
        Label1.Text = ""

        'Show current total
        Dim dummy As String : Dim rat1, rat2, spaceflag, mut As Integer
        dummy = DataGridView1.CurrentRow.Cells("Balance").Value.trim
        For y = 1 To dummy.Length
            If Mid(dummy, y, 1) = " " Then
                mut = Val(Mid(dummy, 1, y - 1))
                SpaceFlag = y
            ElseIf Mid(dummy, y, 1) = "-" Then
                rat1 = Val(Mid(dummy, SpaceFlag + 1, y - SpaceFlag - 1))
                rat2 = Val(Mid(dummy, y + 1, dummy.Length - y))
            End If
        Next y
        Label1.Text = "Current total:" & rat1 + rat2 & Chr(10)

        'dim stuff
        Dim nTeams, nJudges, panTot As Integer
        Dim drRound As DataRow
        drRound = ds.Tables("Round").Rows.Find(cboRound.SelectedValue)
        nTeams = getEventSetting(ds, drRound.Item("Event"), "TeamsPerRound")
        nJudges = drRound.Item("JudgesPerPanel")
        Dim ratings(nTeams, nJudges) As Integer : Dim tot(nTeams) As Integer

        Dim NewJudge As Integer = DataGridView2.CurrentRow.Cells("ID").Value
        'get the rating for each judge and each team
        For z = 1 To nJudges
            'load ratings, substitue newjudge for old
            For x = 1 To nTeams
                For y = 1 To nJudges
                    If y <> z Then
                        ratings(x, y) = GetJudgeRating(ds, DataGridView1.CurrentRow.Cells("Judge" & y).Value, DataGridView1.CurrentRow.Cells("Team" & x).Value, drRound.Item("JudgePlaceScheme"))
                    Else
                        ratings(x, y) = GetJudgeRating(ds, NewJudge, DataGridView1.CurrentRow.Cells("Team" & x).Value, drRound.Item("JudgePlaceScheme"))
                    End If
                Next y
            Next x
            'display
            panTot = 0 : For x = 1 To nTeams : tot(x) = 0 : Next x
            For x = 1 To nTeams
                For y = 1 To nJudges
                    tot(x) += ratings(x, y)
                    panTot += ratings(x, y)
                Next y
            Next x
            If DataGridView1.CurrentRow.Cells("JudgeName" & z).Value Is System.DBNull.Value Then DataGridView1.CurrentRow.Cells("JudgeName" & z).Value = ""
            Label1.Text &= "For " & DataGridView1.CurrentRow.Cells("JudgeName" & z).Value.trim & " " & Math.Abs(tot(1) - tot(2))
            For x = 1 To nTeams
                If x = 1 Then Label1.Text &= " " Else Label1.Text &= "-"
                Label1.Text &= tot(x).ToString.Trim
            Next x
            Label1.Text &= " Total:" & panTot & Chr(10)
        Next z

    End Sub

    Private Sub butPageHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butPageHelp.Click
        Dim strInfo As String = ""
        strInfo &= "This page allows you to make any change to the pairings you want; it is most similar to the 'Display Round' screen in the TRPC." & Chr(10) & Chr(10)
        strInfo &= "LOADING: Begin by checking the settings in bottom-middle; then select a round in the top left and click the 'Load Round' button." & Chr(10) & Chr(10)
        strInfo &= "To edit the pairing, click on any cell in the left-hand pairings grid.  Clicking on a room will load rooms in the right-hand grid, clicking a judge will load judges, clicking a team will load teams.  Note that you need to click on a cell in the grid, not the column header." & Chr(10) & Chr(10)
        strInfo &= "As you click on a team, judge, or room, a control panel for each will appear in the lower-right.  Each of those panels have their own help button." & Chr(10) & Chr(10)
        strInfo &= "To replace any team, judge or room, click on the desired cell in the left-hand grid and click 'delete cell.'  Then click in the desired replacement in the right-hand grid and click 'add.'  Note that you must delete the existing entry before adding; you can only add to an empty cell." & Chr(10) & Chr(10)
        strInfo &= "You can delete a debate entirely by clicking the 'delete entire row' button.  To add a new debate entirely, click the 'Add a new row button,' which will create a blank row you can manually add teams, judges, and a room to." & Chr(10) & Chr(10)
        strInfo &= "Clicking the 'ordinal placement report' will display a diagnostic for assessing the overall quality of judge placement.  Clicking the 'audit' button in the lower-right will display information about any double-schedulig problems, timeslot conflicts, or other errors in the pairing. " & Chr(10) & Chr(10)
        strInfo &= "PRINTING: This is the only page from which you can print a pairing.  The pairing will print with the same information as it appears on the left-hand grid; if inappropriate information (such as judge ratings or team records) appears, you should re-load the round with the appropriate load settings before hitting the 'print' button in the top-right.  " & Chr(10) & Chr(10)
        MsgBox(strInfo, , "Using the Manual Changes Screen")
    End Sub

    Private Sub butBestFit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butBestFit.Click
        'exit if not in judge mode
        If EditMode.ToUpper <> "JUDGE" Then
            MsgBox("This will show the best fit for a selected judge.  Please click on a judge in the pairings table to activate the judge grid.")
            Exit Sub
        End If
        'find judge in dg2
        Dim JudgeN As Integer
        If DataGridView2.CurrentRow Is Nothing Then
            MsgBox("Please select a judge from the right-hand box and try again.")
            Exit Sub
        ElseIf DataGridView2.CurrentRow.Cells("ID").Value Is System.DBNull.Value Then
            MsgBox("Please select a judge from the right-hand box and try again.")
            Exit Sub
        ElseIf DataGridView2.CurrentRow.Cells("ID").Value Is Nothing Then
            MsgBox("Please select a judge from the right-hand box and try again.")
            Exit Sub
        End If
        JudgeN = DataGridView2.CurrentRow.Cells("ID").Value
        'scroll through dg1 and find fits
        Dim dr As DataRow
        dr = ds.Tables("Round").Rows.Find(cboRound.SelectedValue)
        Dim BestFit, FitRow, Rat1, Rat2 As Integer
        Dim strAllFits As String = ""
        BestFit = 999
        For x = 0 To DataGridView1.Rows.Count - 1
            Rat1 = GetJudgeRating(ds, JudgeN, DataGridView1.Rows(x).Cells("Team1").Value, dr.Item("JudgePlaceScheme"))
            Rat2 = GetJudgeRating(ds, JudgeN, DataGridView1.Rows(x).Cells("Team2").Value, dr.Item("JudgePlaceScheme"))
            If CanJudge(ds, JudgeN, DataGridView1.Rows(x).Cells("Panel").Value, chkHearOnce.Checked) = False Then Rat1 = 999 : Rat2 = 999
            FitRow = Rat1 : If Rat2 > Rat1 Then FitRow = Rat2
            If FitRow = 0 Or Rat1 = 0 Or Rat2 = 0 Then FitRow = 9999
            If FitRow < BestFit Then
                BestFit = FitRow
                Label1.Text = "Best fit is a " & BestFit & " In room " & DataGridView1.Rows(x).Cells("RoomName").Value
            End If
            If FitRow <= DataGridView3.Rows(0).Cells(1).Value Then strAllFits &= ", " & DataGridView1.Rows(x).Cells("RoomName").Value
        Next x
        If strAllFits <> "" Then
            Label1.Text &= Chr(10) & Chr(10) & "Under " & DataGridView3.Rows(0).Cells(1).Value & " in " & strAllFits
        End If
    End Sub
    Sub ClearCurrent() Handles cboRound.SelectedIndexChanged
        If EnableEvents = False Then Exit Sub
        DataGridView2.Columns.Clear()
        DataGridView2.DataSource = Nothing
        DataGridView1.DataSource = Nothing
    End Sub

    Private Sub butSideChange_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butSideChange.Click
        Dim f As New Form
        f = frmWUDCSideChange
        f.Show()
    End Sub
End Class
