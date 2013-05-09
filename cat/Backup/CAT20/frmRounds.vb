'on load, validate that all timeslots actually exist

Public Class frmRounds
    Dim ds As New DataSet
    Dim nJudges As Integer
    Private Sub frmRounds_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Load
        LoadFile(ds, "TourneyData")
        Call CheckForGoodToGo()
        Call RoomJudgeUpdate()

        'fill even cbo
        cboEvent.DataSource = ds.Tables("Event")
        cboEvent.ValueMember = "ID"
        cboEvent.DisplayMember = "EventName"
        cboEvent.Text = "Select Division"

        'eliminate null values
        For x = 0 To ds.Tables("Round").Rows.Count - 1
            If ds.Tables("Round").Rows(x).Item("PairingScheme") Is System.DBNull.Value Then ds.Tables("Round").Rows(x).Item("PairingScheme") = "None"
            If ds.Tables("Round").Rows(x).Item("Flighting") Is System.DBNull.Value Then ds.Tables("Round").Rows(x).Item("Flighting") = False
            If ds.Tables("Round").Rows(x).Item("TB_SET") Is System.DBNull.Value Then ds.Tables("Round").Rows(x).Item("TB_SET") = 1
            If ds.Tables("Round").Rows(x).Item("JudgePlaceScheme") Is System.DBNull.Value Then ds.Tables("Round").Rows(x).Item("JudgePlaceScheme") = "Random"
            ds.Tables("Round").Rows(x).Item("JudgePlaceScheme") = ds.Tables("Round").Rows(x).Item("JudgePlaceScheme").trim
        Next
        Call NullKiller(ds, "Round")

        'Link up the comboboxes that pull from other tables
        Dim dgvc2 As New DataGridViewComboBoxColumn
        dgvc2.DataSource = ds.Tables("Tiebreak_Set")
        dgvc2.ValueMember = "ID"
        dgvc2.DisplayMember = "TBSET_Name"
        dgvc2.DataPropertyName = "TB_SET"
        dgvc2.HeaderText = "Tiebreak Set"
        dgvc2.Name = "TB_SET"
        dgvc2.DisplayIndex = 2
        dgvc2.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        DataGridView1.Columns.Add(dgvc2)

        dgvc2 = New DataGridViewComboBoxColumn
        dgvc2.DataSource = ds.Tables("Timeslot")
        dgvc2.ValueMember = "ID"
        dgvc2.DisplayMember = "TimeSlotName"
        dgvc2.DataPropertyName = "Timeslot"
        dgvc2.HeaderText = "TimeSlot"
        dgvc2.Name = "TimeSlot"
        dgvc2.DisplayIndex = 3
        dgvc2.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        DataGridView1.Columns.Add(dgvc2)

        Dim dt As New DataTable
        dt.Columns.Add("ID", System.Type.GetType("System.Int64"))
        dt.Columns.Add("Rd_Label", System.Type.GetType("System.String"))
        Dim DR As DataRow
        DR = dt.NewRow : DR.Item("ID") = 1 : DR.Item("Rd_Label") = "Prelim 1" : dt.Rows.Add(DR)
        DR = dt.NewRow : DR.Item("ID") = 2 : DR.Item("Rd_Label") = "Prelim 2" : dt.Rows.Add(DR)
        DR = dt.NewRow : DR.Item("ID") = 3 : DR.Item("Rd_Label") = "Prelim 3" : dt.Rows.Add(DR)
        DR = dt.NewRow : DR.Item("ID") = 4 : DR.Item("Rd_Label") = "Prelim 4" : dt.Rows.Add(DR)
        DR = dt.NewRow : DR.Item("ID") = 5 : DR.Item("Rd_Label") = "Prelim 5" : dt.Rows.Add(DR)
        DR = dt.NewRow : DR.Item("ID") = 6 : DR.Item("Rd_Label") = "Prelim 6" : dt.Rows.Add(DR)
        DR = dt.NewRow : DR.Item("ID") = 7 : DR.Item("Rd_Label") = "Prelim 7" : dt.Rows.Add(DR)
        DR = dt.NewRow : DR.Item("ID") = 8 : DR.Item("Rd_Label") = "Prelim 8" : dt.Rows.Add(DR)
        DR = dt.NewRow : DR.Item("ID") = 9 : DR.Item("Rd_Label") = "Prelim 9" : dt.Rows.Add(DR)
        DR = dt.NewRow : DR.Item("ID") = 10 : DR.Item("Rd_Label") = "Quads" : dt.Rows.Add(DR)
        DR = dt.NewRow : DR.Item("ID") = 11 : DR.Item("Rd_Label") = "Triples" : dt.Rows.Add(DR)
        DR = dt.NewRow : DR.Item("ID") = 12 : DR.Item("Rd_Label") = "Doubles" : dt.Rows.Add(DR)
        DR = dt.NewRow : DR.Item("ID") = 13 : DR.Item("Rd_Label") = "Octos" : dt.Rows.Add(DR)
        DR = dt.NewRow : DR.Item("ID") = 14 : DR.Item("Rd_Label") = "Quarters" : dt.Rows.Add(DR)
        DR = dt.NewRow : DR.Item("ID") = 15 : DR.Item("Rd_Label") = "Semis" : dt.Rows.Add(DR)
        DR = dt.NewRow : DR.Item("ID") = 16 : DR.Item("Rd_Label") = "Finals" : dt.Rows.Add(DR)

        dgvc2 = New DataGridViewComboBoxColumn
        dgvc2.DataSource = dt
        dgvc2.ValueMember = "ID"
        dgvc2.DisplayMember = "Rd_Label"
        dgvc2.DataPropertyName = "RD_Name"
        dgvc2.HeaderText = "Round ID"
        dgvc2.Name = "RD-Name2"
        dgvc2.DisplayIndex = 5
        dgvc2.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        DataGridView1.Columns.Add(dgvc2)

        DataGridView1.AutoGenerateColumns = False
        DataGridView1.DataSource = ds.Tables("Round")

    End Sub
    Sub RoomJudgeUpdate()
        If RoomJudgeCheck(ds, "Judge", "Event") <> "OK" Then lblRoomsJudges.Text = "Judges do not appear to be initialized for event/division eligibility.  "
        If RoomJudgeCheck(ds, "Judge", "Timeslot") <> "OK" Then lblRoomsJudges.Text = "Judges do not appear to be initialized for event/division eligibility.  "
        If ds.Tables("Room").Rows.Count > 0 Then
            If RoomJudgeCheck(ds, "Room", "Event") <> "OK" Then lblRoomsJudges.Text = "Rooms do not appear to be initialized for event/division eligibility.  "
            If RoomJudgeCheck(ds, "Room", "Timeslot") <> "OK" Then lblRoomsJudges.Text = "Rooms do not appear to be initialized for event/division eligibility. "
        End If
        If lblRoomsJudges.Text <> "Judges and rooms are initialized" Then
            lblRoomsJudges.Text &= " Click the button below."
        End If
    End Sub
    Sub CheckForGoodToGo()
        'check that divisions exist
        If ds.Tables("Event").Rows.Count = 0 Then
            MsgBox("You MUST set up divisions before you set up the rounds.  Please close this screen, return to the SET UP DIVISIONS screen, and then return to this page.", MsgBoxStyle.OkOnly)
        End If
        'check that timeslots exist
        If ds.Tables("TimeSlot").Rows.Count = 0 Then
            MsgBox("You MUST set up time slots before you set up the rounds.  Please close this screen, return to the ENTER TOURNAMENT-WIDE SETTINGS screen, and then return to this page.", MsgBoxStyle.OkOnly)
        End If
        'check for tiebreakers
        If ds.Tables("Tiebreak").Rows.Count = 0 Or ds.Tables("Tiebreak_Set").Rows.Count = 0 Then
            MsgBox("You MUST set up tiebreakers before you set up the rounds.  Please close this screen, return to the SET UP TIEBREAKERS screen, and then return to this page.", MsgBoxStyle.OkOnly)
            Call MyBase.Close()
        End If
        'scroll events and add rounds if none exist
        Dim fdRds As DataRow()
        For x = 0 To ds.Tables("Event").Rows.Count - 1
            fdRds = ds.Tables("Round").Select("Event=" & ds.Tables("Event").Rows(x).Item("ID"))
            If fdRds.Length = 0 Then Call MakeRoundsByDivision(ds.Tables("Event").Rows(x).Item("ID"))
        Next
    End Sub
    Sub ehandle(ByVal sender As Object, ByVal e As DataGridViewDataErrorEventArgs) Handles DataGridView1.DataError
        MsgBox("Error at row" & e.RowIndex & " and column " & e.ColumnIndex)
    End Sub
    Private Sub frmRounds_Unload(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Disposed
        Call SaveFile(ds)
        Call SetupCompletionStatus(ds)
        ds.Dispose()
    End Sub

    Private Sub ResetAllRounds_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butResetAllRounds.Click

        Dim q As Integer
        q = MsgBox("Do you want to delete all existing rounds?  The much safer option is to select NO; this will maintain all existing rounds but set up default values for all rounds that do not have them.  Selecting YES will delete all existing rounds for all divisions, in the process erasing all currently scheduled rounds and any results.  Selecting NO will simply add default values where they are missing.", MsgBoxStyle.YesNo)
        Dim x As Integer
        If q = vbYes Then
            For x = ds.Tables("round").Rows.Count - 1 To 0 Step -1
                ds.Tables("round").Rows(x).Delete()
            Next x
        End If

        For x = 0 To ds.Tables("Event").Rows.Count - 1
            Call MakeRoundsByDivision(ds.Tables("event").Rows(x).Item("ID"))
            Call MakeElimSeeds(ds, ds.Tables("event").Rows(x).Item("ID"))
        Next x

        ds.Tables("ROUND").AcceptChanges()
        DataGridView1.DataSource = ds.Tables("Round")

    End Sub
    Sub MakeRoundsByDivision(ByVal EventID As Integer)
        Dim y, nprelims, nelims As Integer
        Dim dr As DataRow
        nprelims = getEventSetting(ds, EventID, "nPrelims")
        nelims = getEventSetting(ds, EventID, "nElims")
        Dim drEvent As DataRow
        drEvent = ds.Tables("Event").Rows.Find(EventID)
        Dim fdRows As DataRow()
        Dim TBSET As Integer = GetTBSET(EventID)
        For y = 0 To nprelims - 1
            'see if round exists already; if not, add it.
            fdRows = ds.Tables("Round").Select("Rd_Name =" & y + 1 & " and event=" & EventID)
            If fdRows.Length = 0 Then
                dr = ds.Tables("ROUND").NewRow
                dr.Item("EVENT") = drEvent.Item("ID")
                dr.Item("TIMESLOT") = ds.Tables("TIMESLOT").Rows(y).Item("ID")
                dr.Item("TB_SET") = TBSET
                dr.Item("RD_NAME") = y + 1
                If drEvent.Item("ABBR").trim <> "" Then
                    dr.Item("LABEL") = drEvent.Item("ABBR").trim & " Prelim round " & y + 1.ToString
                Else
                    dr.Item("LABEL") = drEvent.Item("EventName").trim & " Prelim round " & y + 1.ToString
                End If
                dr.Item("FLIGHTING") = 1 : If drEvent.Item("Type") = "Lincoln-Douglas" Then dr.Item("FLIGHTING") = 2
                dr.Item("JUDGESPERPANEL") = 1
                dr.Item("JudgePlaceScheme") = "Random"
                dr.Item("PairingScheme") = "HighLow"
                If y <= Int(nprelims / 3) Then dr.Item("PairingScheme") = "Preset"
                ds.Tables("ROUND").Rows.Add(dr)
            End If
        Next y
        Dim intTimeslot
        For y = 16 To (16 - nelims + 1) Step -1
            fdRows = ds.Tables("Round").Select("Rd_Name = " & y & " and event=" & EventID)
            If fdRows.Length = 0 Then
                dr = ds.Tables("ROUND").NewRow
                dr.Item("EVENT") = drEvent.Item("ID")
                intTimeslot = ds.Tables("Timeslot").Rows.Count
                intTimeslot = intTimeslot - (16 - y)
                dr.Item("TIMESLOT") = ds.Tables("Timeslot").Rows(intTimeslot - 1).Item("ID")
                dr.Item("TB_SET") = TBSET
                dr.Item("RD_NAME") = y
                If drEvent.Item("ABBR").trim <> "" Then
                    dr.Item("LABEL") = drEvent.Item("ABBR").trim & " " & GetElimName(dr.Item("RD_NAME"))
                Else
                    dr.Item("LABEL") = drEvent.Item("ABBR").trim & " " & GetElimName(dr.Item("RD_NAME"))
                End If
                dr.Item("FLIGHTING") = 1 : If drEvent.Item("Type") = "Lincoln-Douglas" Then dr.Item("FLIGHTING") = 2
                dr.Item("JUDGESPERPANEL") = 3
                dr.Item("JudgePlaceScheme") = "Random"
                dr.Item("PairingScheme") = "Elim"
                ds.Tables("ROUND").Rows.Add(dr)
            End If
        Next y
    End Sub
    Function GetTBSET(ByVal EventID As Integer)
        GetTBSET = 1
        Dim x As Integer
        Dim nTeams = getEventSetting(ds, EventID, "TeamsPerRound")
        For x = 0 To ds.Tables("Tiebreak_Set").Rows.Count - 1
            If nTeams = 2 And ds.Tables("Tiebreak_set").Rows(x).Item("TBSET_NAME") = "2 teams - Team Prelims" Then GetTBSET = ds.Tables("Tiebreak_set").Rows(x).Item("ID")
            If nTeams = 4 And ds.Tables("Tiebreak_set").Rows(x).Item("TBSET_NAME") = "4 teams - Team Prelims" Then GetTBSET = ds.Tables("Tiebreak_set").Rows(x).Item("ID")
        Next x
    End Function
    Private Sub butShowDivision_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butShowDivision.Click
        If cboEvent.SelectedIndex = -1 Then
            MsgBox("Please select a division and try again.")
            Exit Sub
        End If
        Dim dtv As New DataView(ds.Tables("Round"))
        dtv.RowFilter = "Event=" & cboEvent.SelectedValue
        DataGridView1.DataSource = dtv
    End Sub

    Private Sub butShowAllDivisions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butShowAllDivisions.Click
        DataGridView1.DataSource = ds.Tables("Round")
        If chkShowRoundID.Checked = True Then DataGridView1.Columns("ID").Visible = True
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        ds.Tables("ElimSeed").Clear()
        For x = 0 To ds.Tables("EVENT").Rows.Count - 1
            Call MakeElimSeeds(ds, ds.Tables("Event").Rows(x).Item("ID"))
        Next
    End Sub
    Private Sub UserDeletingRow(ByVal sender As Object, ByVal e As DataGridViewRowCancelEventArgs) Handles DataGridView1.UserDeletingRow
        Dim drRound As DataRow : drRound = ds.Tables("Round").Rows.Find(DataGridView1.CurrentRow.Cells("ID").Value)
        Dim fdPanel As DataRow()
        fdPanel = ds.Tables("Panel").Select("Round=" & drRound.Item("ID"))
        If fdPanel.Length = 0 Then Exit Sub
        Dim q As Integer
        q = MsgBox("WARNING!  Pairings for this round have been detected.  Continuing with the delete will erase all pairings and results associated with this division!  Click OK to continue or CANCEL to continue without deleting.", MsgBoxStyle.OkCancel)
        If q = vbCancel Then e.Cancel = True
    End Sub
    Private Sub datagridview1_DefaultValuesNeeded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles DataGridView1.DefaultValuesNeeded
        With e.Row
            .Cells("PairingScheme").Value = "None"
            .Cells("TimeSLot").Value = 10
        End With
    End Sub

    Private Sub butChangeRatings_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butChangeRatings.Click
        Dim dr As DataRow
        For x = 0 To DataGridView1.RowCount - 1
            dr = ds.Tables("Round").Rows.Find(DataGridView1.Rows(x).Cells("ID").Value)
            If radRandom.Checked = True Then
                dr.Item("JudgePlaceScheme") = "Random"
            ElseIf radTeamRating.Checked = True Then
                dr.Item("JudgePlaceScheme") = "TeamRating"
            ElseIf radTabRatings.Checked = True Then
                dr.Item("JudgePlaceScheme") = "AssignedRating"
            End If
        Next x
    End Sub

    Private Sub butColKey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butColKey.Click
        Dim strInfo As String = ""
        strInfo &= "The TIEBREAK SET column indicates the set of tiebreakers to be used in the seeding of the round.  Tiebreak sets are defined on the SET UP TIEBREAKERS screen; the help button on that screen has more information about what Tiebreak Sets are and how to use them.  For preset rounds, team rankings will be used regardless of the Tiebreak Set selected here. " & Chr(10) & Chr(10)
        strInfo &= "The TIMESLOT column indicates timeslot in which the round will occur.  Timeslots are defined on the ENTER TOURNAMENT-WIDE SETTINGS screen.  Because multiple divisions can hold debates during the same timeslot, it is important to set this correctly.  The timeslot field is used to avoid double-scheduling teams and rooms.  " & Chr(10) & Chr(10)
        strInfo &= "The FULL LABEL column identifies the text that will be used on all the printouts.  It can be any text that you want that will help identify the round; usually including the division/event name, whether the round is a prelim or elim, and the round number are important.  The computer will supply a default.  " & Chr(10) & Chr(10)
        strInfo &= "The ROUND ID column contains much the same information as the FULL LABEL but the values are pre-set and cannot be customized.  This is the actual value the computer uses during pairing functions; the FULL LABEL is used for display purposes only. " & Chr(10) & Chr(10)
        strInfo &= "The JUDGES PER PANEL column recievs an integer and designated the number of judges assigned to each debate.  Note that you can specify a number of judges by round, and that elimination rounds typically have more judges than preliminary rounds.  " & Chr(10) & Chr(10)
        strInfo &= "The FLIGHT column indicates whether or not the round is flighted.  1 indicates single-flighting (no flighting), 2 indicates double-flighting, 3 indicates triple-flighting, etc." & Chr(10) & Chr(10)
        strInfo &= "The ROUND ID column contains much the same information as the FULL LABEL but the values are pre-set and cannot be customized.  This is the actual value the computer uses during pairing functions; the FULL LABEL is used for display purposes only. " & Chr(10) & Chr(10)
        strInfo &= "The JUDGE PLACEMENT column identifies the judge placement scheme to be used.  TEAMRATING means that team-supplied ratings will be used; this is the same as mutual placement judging.  Team ratings are entered on the ENTER OR EDIT JUDGE PLACEMENT INFORMATION screen.  ASSIGNEDRATING uses tab room assigned ratings; these can be entered on the ENTER OR EDIT JUDGES screen.  RANDOM means that no judge ratings will be used and the computer will simply seek to maximize judge commitments." & Chr(10) & Chr(10)
        strInfo &= "The PAIRING SCHEME column identifies how the computer will pair the teams.  PRESET means team ratings will be used, and the computer will attempt to equalize the overall draw within the preset rounds.  RANDOM assigns opponents randomly while honoring constraints.  HIGHHIGH produces a high-high pairing within brackets, i.e., seed 1 debates seed 2, etc.  HIGHLOW is a high-low power match within brackets, i.e., the top team in the bracket will debate the lowest team in the win bracket.  ELIM indicates an elimination round; for elim rounds opponents are set on the basis of overall bracket seeding. " & Chr(10) & Chr(10)
        strInfo &= "More information about all this terminology and how teams are paired appears in the online HOW TO TAB manual at www.idebate.org/HowToTab.html.  " & Chr(10) & Chr(10)
        MsgBox(strInfo)
    End Sub

    Private Sub butBasicInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butBasicInfo.Click
        Dim strInfo As String = ""
        strInfo &= "Each event can schedule a ROUND in a TIMESLOT.  Ther rounds can be PRELIMINARY or ELIMINATION rounds.  Note that event and timeslot information are stored in each round record.  " & Chr(10) & Chr(10)
        strInfo &= "Default values will load for each division when this screen is first opened; you can use this screen to change any value you wish.  " & Chr(10) & Chr(10)
        strInfo &= "Select the division in the top-left; all rounds for the selected division will display.  " & Chr(10) & Chr(10)
        strInfo &= "If you want to change the way that judges are selected for all rounds of a division, you can use the controls at the bototm of the page.  " & Chr(10) & Chr(10)
        MsgBox(strInfo)
    End Sub

    Private Sub butInitRoomsAndJudges_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butInitRoomsAndJudges.Click
        Call InitializeRooms(ds)
        Call InitializeJudges(ds)
        lblRoomsJudges.Text = "Judges and rooms are initialized"
    End Sub

    Private Sub butAddRound_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butAddRound.Click
        Dim drEv As DataRow
        drEv = ds.Tables("Event").Rows.Find(cboEvent.SelectedValue)
        If drEv Is Nothing Then MsgBox("Please first select an event from the top-left drop-down box.") : Exit Sub
        Dim q As Integer = MsgBox("This will create a new round for " & drEv.Item("EventName") & ".  Hit YES to continue or NO to exit and select a new event in the top-left drop-down list.", MsgBoxStyle.YesNo)
        If q = vbNo Then Exit Sub

        Dim dr As DataRow
        dr = ds.Tables("ROUND").NewRow
        dr.Item("EVENT") = cboEvent.SelectedValue
        dr.Item("TIMESLOT") = ds.Tables("Timeslot").Rows(0).Item("ID")
        dr.Item("TB_SET") = ds.Tables("Tiebreak_set").Rows(0).Item("ID")
        dr.Item("RD_NAME") = 9
        dr.Item("LABEL") = "EDIT LABEL"
        dr.Item("FLIGHTING") = 1
        dr.Item("JUDGESPERPANEL") = 1
        dr.Item("JudgePlaceScheme") = "Random"
        dr.Item("PairingScheme") = "Random"
        ds.Tables("ROUND").Rows.Add(dr)
    End Sub

    Private Sub butBreakout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butBreakout.Click
        Dim strInfo As String = ""
        strInfo &= "Sometimes an event might want to allow a certain set of competitors to compete in separate elim rounds, for example, if JV and novice divisions have been collapsed the director may wish to give the top 4 novice teams a separate breakout elim division. " & Chr(10) & Chr(10)
        strInfo &= "The first step is to make sure that there is a division set up for the new division.  If you want a novice breakout round, for example, go back to the main menu and select setup step 2 to make sure there is a novice division create one if it doesn't exist.  Apply all changes below to the breakout division; they should be the ONLY rounds for that division. " & Chr(10) & Chr(10)
        strInfo &= "To schedule breakout rounds, click on the 'Add new round for manual editing' button and a new round will appear.  " & Chr(10) & Chr(10)
        strInfo &= "Give the round an appropriate label such as 'novice breakout semis' and for RoundID mark it as 'prelim 9'.  " & Chr(10) & Chr(10)
        strInfo &= "You will then need to schedule the teams manually using the manual placement screen; select the appropriate round and pair the teams against one another.  " & Chr(10) & Chr(10)
        strInfo &= "Note that you do not have to schedule all the teams participating in the round.  " & Chr(10) & Chr(10)
        strInfo &= "This is best thought of as a workaround; you can NOT enter the results on the elim entry screen nor will teams automatically advance.  You MUST pair each subsequent round manually, and ALL breakout rounds must bear the RoundID of prelim 9.  The CAT will treat these rounds as prelim rounds even though they are labelled elimination rounds.  " & Chr(10) & Chr(10)
        strInfo &= "Please note that breakout rounds are primarily conducted for educational purposes; neither the NDT nor CEDA organizations counts such rounds toward points. " & Chr(10) & Chr(10)
        strInfo &= "If you plan to break any further than semi-finals, you may wish to ask whether breakout rounds are truly appropriate.  " & Chr(10) & Chr(10)
        MsgBox(strInfo)
    End Sub
    Sub JudgePerRoundFixer() Handles DataGridView1.CellEndEdit
        If DataGridView1.Columns(DataGridView1.CurrentCell.ColumnIndex).Name.ToUpper <> "JUDGESPERPANEL" Then Exit Sub
        If DataGridView1.CurrentCell.Value < nJudges Then
            Call ShrinkPanelSize()
        End If
    End Sub
    Sub JudgePerRoundFixer2() Handles DataGridView1.CellBeginEdit
        If DataGridView1.Columns(DataGridView1.CurrentCell.ColumnIndex).Name.ToUpper <> "JUDGESPERPANEL" Then Exit Sub
        nJudges = DataGridView1.CurrentCell.Value
    End Sub
    Sub ShrinkPanelSize()
        Dim q As Integer = MsgBox("You are shrinking the size of the panels.  If you do this, the program will DELETE all judges exceeding the new panel size for existing pairings for this round.  Continue?", MsgBoxStyle.YesNo)
        If q = vbNo Then Exit Sub
        'Fires if the panel size has reduced, and shrinks panels down to the appropriate size
        Dim DidDelete As Boolean
        Dim Round As Integer = DataGridView1.CurrentRow.Cells("ID").Value
        Dim fdPanel, fdBallots As DataRow()
        fdPanel = ds.Tables("Panel").Select("Round=" & Round)
        Dim nJudges As Integer = DataGridView1.CurrentCell.Value
        For x = 0 To fdPanel.Length - 1
            fdBallots = ds.Tables("Ballot").Select("Panel=" & fdPanel(x).Item("ID"), "Judge DESC")
            For y = fdBallots.Length - 1 To 0 Step -1
                If ((y + 1) / 2) > nJudges Then
                    fdBallots(y).Delete() : DidDelete = True
                End If
            Next y
        Next x
        If DidDelete = True Then
            MsgBox("Judge deletions WERE performed for this round; you may wish to review the pairings.")
        Else
            MsgBox("Judges deletes were NOT necessary even though the panel size shrunk; this is probably because no debates were yet paired, or all debates already had panels of the newly defined size.")
        End If
    End Sub

    Private Sub butRdIDFix_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butRdIDFix.Click
        Dim fdRd As DataRow() : Dim nElims As Integer
        For x = 0 To ds.Tables("Event").Rows.Count - 1
            fdRd = ds.Tables("Round").Select("Event=" & ds.Tables("Event").Rows(x).Item("ID"), "Timeslot DESC")
            nElims = getEventSetting(ds, ds.Tables("Event").Rows(x).Item("ID"), "nElims")
            For y = 16 To 17 - nElims Step -1
                fdRd(16 - y).Item("Rd_Name") = y
            Next y
        Next x
    End Sub
End Class