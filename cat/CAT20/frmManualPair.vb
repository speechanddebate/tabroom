Public Class frmManualPair
    Dim ds As New DataSet
    Dim dtG1 As DataTable
    Dim dtG2 As DataTable
    Dim strDebateType As String
    Dim ExitAutoPair As Boolean = False

    Private Sub frmManualPair_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call LoadFile(ds, "TourneyData")

        'Get debate type
        InitializeTourneySettings(ds)
        Dim fdSet As DataRow()
        fdSet = ds.Tables("Tourn_Setting").Select("Tag='TourneyType'")
        strDebateType = fdSet(0).Item("Value")

        'Make sure view settings are in place
        Call InitializeViewSettings(ds)

        'Set screen by debate type
        Call SetScreenByDebateType()

        'bind round CBO
        cboRound.DataSource = ds.Tables("Round")
        cboRound.DisplayMember = "Label"
        cboRound.ValueMember = "ID"
        cboRound.Focus()

        dgvDisplayItems.AutoGenerateColumns = False
    End Sub
    Private Sub frmManualPair_Unload(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Disposed
        Call SaveFile(ds)
        ds.Dispose()
    End Sub
    Sub EraseViewSettings()
        'erase current settings
        For x = ds.Tables("ViewSettings").Rows.Count - 1 To 0 Step -1
            If ds.Tables("ViewSettings").Rows(x).Item("Round") = cboRound.SelectedValue Then
                ds.Tables("ViewSettings").Rows(x).Delete()
            End If
        Next x

    End Sub
    Sub MakeLoadSettings()
        'Creates the table that will be used to enter the load settings

        'Load the round and check there's a TBSET attached to it
        Dim drRound As DataRow
        drRound = ds.Tables("Round").Rows.Find(cboRound.SelectedValue)
        If drRound.Item("TB_SET") Is System.DBNull.Value Then
            MsgBox("There does not appear to be a list of tiebreakers associated with this round yet.  Close this screen, go to the ROUNDS screen, and set up the tiebreakers there first.")
            Exit Sub
        End If

        Call EraseViewSettings()

        'populate with standard selections
        Dim dr As DataRow
        dr = ds.Tables("ViewSettings").NewRow : dr.Item("Tag") = "Side Counts" : dr.Item("SortOrder") = 99 : dr.Item("Round") = cboRound.SelectedValue : ds.Tables("ViewSettings").Rows.Add(dr)
        dr = ds.Tables("ViewSettings").NewRow : dr.Item("Tag") = "SOP" : dr.Item("SortOrder") = 3 : dr.Item("Round") = cboRound.SelectedValue : ds.Tables("ViewSettings").Rows.Add(dr)
        dr = ds.Tables("ViewSettings").NewRow : dr.Item("Tag") = "OppSeed" : dr.Item("SortOrder") = 4 : dr.Item("Round") = cboRound.SelectedValue : ds.Tables("ViewSettings").Rows.Add(dr)
        dr = ds.Tables("ViewSettings").NewRow : dr.Item("Tag") = "Times pulled up" : dr.Item("SortOrder") = 99 : dr.Item("Round") = cboRound.SelectedValue : ds.Tables("ViewSettings").Rows.Add(dr)
        dr = ds.Tables("ViewSettings").NewRow : dr.Item("Tag") = "Times pulled down" : dr.Item("SortOrder") = 99 : dr.Item("Round") = cboRound.SelectedValue : ds.Tables("ViewSettings").Rows.Add(dr)
        dr = ds.Tables("ViewSettings").NewRow : dr.Item("Tag") = "Seed" : dr.Item("SortOrder") = 1 : dr.Item("Round") = cboRound.SelectedValue : ds.Tables("ViewSettings").Rows.Add(dr)

        'populate with all tiebreakers in use for this round
        Dim fdTB As DataRow()
        fdTB = ds.Tables("TieBreak").Select("TB_SET=" & drRound.Item("TB_SET"))
        If fdTB.Length = 0 Then
            MsgBox("There does not appear to be a list of tiebreakers associated with the tiebreaker set associated with this round yet.  Close this screen, go to the ROUNDS screen, and set up the tiebreakers there first.")
            Call EraseViewSettings()
            Exit Sub
        End If
        For x = 0 To fdTB.Length - 1
            dr = ds.Tables("ViewSettings").NewRow()
            dr.Item("Round") = cboRound.SelectedValue
            If x = 0 Then
                dr.Item("SortOrder") = 2
            Else
                dr.Item("SortOrder") = 99
            End If
            dr.Item("Tag") = fdTB(x).Item("Label")
            ds.Tables("ViewSettings").Rows.Add(dr)
        Next x
        If strDebateType = "WUDC" Then
            For x = 0 To ds.Tables("ViewSettings").Rows.Count - 1
                If ds.Tables("ViewSettings").Rows(x).Item("Tag") = "Side Counts" Then ds.Tables("ViewSettings").Rows(x).Item("SortOrder") = 3
                If ds.Tables("ViewSettings").Rows(x).Item("Tag") = "OppSeed" Then ds.Tables("ViewSettings").Rows(x).Item("SortOrder") = 99
                If ds.Tables("ViewSettings").Rows(x).Item("Tag") = "SOP" Then ds.Tables("ViewSettings").Rows(x).Item("SortOrder") = 99
            Next
        End If
    End Sub

    Private Sub butLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butLoad.Click
        Call LoadRound()
        If strDebateType = "WUDC" Then Call Randomizer()
        Dim drRd, drEvent As DataRow
        drRd = ds.Tables("Round").Rows.Find(cboRound.SelectedValue)
        drEvent = ds.Tables("Event").Rows.Find(drRd.Item("Event"))
        If strDebateType <> "Policy" And drEvent.Item("Type") = "Policy" Then strDebateType = "Policy"
    End Sub
    Sub LoadRound()
        Dim strtime As String
        strtime = "Start: " & Now.Second & " " & Now.Millisecond & Chr(10) & Chr(10)
        'make it so that you can only select one team per gird
        DataGridView1.MultiSelect = False
        DataGridView2.MultiSelect = False
        If chkSideConstrained.Checked = False Then
            DataGridView1.MultiSelect = True 'allow the selection of multiple teams
            DataGridView2.Visible = False
            DataGridView1.Width = 660
            DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            LoadCompetitors(DataGridView1, 0, dtG1)
            'DataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowMode.AllCells
        Else
            DataGridView2.Visible = True
            DataGridView1.Width = 330
            DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            DataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            LoadCompetitors(DataGridView1, 1, dtG1)
            LoadCompetitors(DataGridView2, 2, dtG2)
            DataGridView1.Columns("CompetitorName").Width = 100
            DataGridView2.Columns("CompetitorName").Width = 100
        End If
        strtime &= "Load Teams End: " & Now.Second & " " & Now.Millisecond & Chr(10) & Chr(10)
        LoadPairings()
        strtime &= "Load Pairings End: " & Now.Second & " " & Now.Millisecond
        'Label2.Text = strtime
        Call ClearCompetitorGrids()
        Call ResetActives()
        Call SmallCheck()
    End Sub
    Sub SmallCheck()
        Dim drRd, drEvent As DataRow
        Dim nTeams, nPrelims As Integer
        drRd = ds.Tables("Round").Rows.Find(cboRound.SelectedValue)
        If drRd.Item("Rd_Name") <> 1 Then Exit Sub
        drEvent = ds.Tables("Event").Rows.Find(drRd.Item("Event"))
        nTeams = DataGridView1.RowCount
        If DataGridView2.Visible = True Then nTeams += DataGridView2.RowCount
        nPrelims = getEventSetting(ds, drRd.Item("Event"), "nPrelims")
        If nTeams - nPrelims <= nPrelims Then
            Dim strInfo As String = ""
            strInfo &= "This is a small division, which presents 2 special challenges." & Chr(10) & Chr(10)
            strInfo &= "First, power matching might not be meaningful since there will be so few possible opponents (for any team) in the later rounds." & Chr(10) & Chr(10)
            strInfo &= "Second, unless things are carefully planned, later rounds might not be possible without requiring teams to debate a second time or violating side constraints." & Chr(10) & Chr(10)
            strInfo &= "You might consider using the PAIR A SMALL DIVISION option from the main menu." & Chr(10) & Chr(10)
            strInfo &= "To do so, simply close this screen and click the PAIR A SMALL DIVISION button on the main menu." & Chr(10) & Chr(10)
            MsgBox(strInfo, , "Small Division Warning.")
        End If
    End Sub
    Sub LoadCompetitors(ByVal DGV As DataGridView, ByVal SideDue As Integer, ByRef DT As DataTable)

        'Pull the tiebreakers and put them in DGV

        'Load prior round, unless it hasn't got any results, and then load last round with results
        'unless current round is a preset
        Dim intRound As Integer
        Dim rdname1, rdname2 As DataRow
        Dim drRandom As DataRow
        drRandom = ds.Tables("Round").Rows.Find(cboRound.SelectedValue)
        If drRandom.Item("Rd_Name") > 1 Then
            rdname1 = ds.Tables("Round").Rows.Find(GetLastCompleteRound(ds, cboRound.SelectedValue))
        Else
            rdname1 = drRandom
        End If
        drRandom = ds.Tables("Round").Rows.Find(GetPriorRound(ds, cboRound.SelectedValue))
        If drRandom Is Nothing Then
            rdname2 = ds.Tables("Round").Rows.Find(cboRound.SelectedValue)
        Else
            rdname2 = ds.Tables("Round").Rows.Find(GetPriorRound(ds, cboRound.SelectedValue))
        End If
        If rdname1 Is Nothing Then rdname1 = ds.Tables("Round").Rows.Find(cboRound.SelectedValue)
        If rdname2 Is Nothing Then rdname2 = ds.Tables("Round").Rows.Find(cboRound.SelectedValue)
        If rdname1.Item("Rd_Name") < rdname2.Item("Rd_Name") Then
            intRound = rdname1.Item("ID")
        Else
            intRound = rdname2.Item("ID")
        End If
        If intRound = 0 Then intRound = cboRound.SelectedValue
        Dim drRd As DataRow : drRd = ds.Tables("Round").Rows.Find(cboRound.SelectedValue)
        If drRd.Item("PairingScheme").toupper = "PRESET" Then intRound = cboRound.SelectedValue
        DT = MakeTBTable(ds, intRound, "TEAM", "Code", -1, cboRound.SelectedValue)
        'add SOP before you strip for sides
        For y = 0 To dgvDisplayItems.Rows.Count - 1
            If dgvDisplayItems.Rows(y).Cells("Tag").Value.ToString.ToUpper = "SOP" And dgvDisplayItems.Rows(y).Cells("SortOrder").Value < 99 Then
                DT.Columns.Add("SOP", System.Type.GetType("System.Single"))
                DT.Columns.Add("OppSeed", System.Type.GetType("System.Single"))
                Call AddSOPValues(DT)
            End If
        Next y

        'delete due otherside if not due
        If SideDue > 0 Then
            For x = DT.Rows.Count - 1 To 0 Step -1
                If GetSideDue(ds, cboRound.SelectedValue, DT.Rows(x).Item("Competitor")) <> SideDue Then
                    DT.Rows(x).Delete()
                End If
            Next x
        End If

        'now format the datagrid to display
        Dim colIndex As Integer

        'always add side reports and in round
        DT.Columns.Add("Sides", System.Type.GetType("System.String"))
        DT.Columns.Add("InRound", System.Type.GetType("System.Boolean"))
        Call AddSideReport(DT, ds)
        Call MarkActiveTeams(DT)
        DGV.DataSource = DT.DefaultView
        If chkPairedToBottom.Checked = True Then DT.DefaultView.Sort = "InRound ASC"

        'add brackets if powered round & NEED TO ADD: Only do for divisions with 2 teams per round
        Dim drRound As DataRow : drRound = ds.Tables("Round").Rows.Find(cboRound.SelectedValue)
        If drRound.Item("PairingScheme") = "HighHigh" Or drRound.Item("PairingScheme") = "HighLow" Then
            DT.Columns.Add("Bracket", System.Type.GetType("System.Int64"))
            Call BracketMarker(DT)
        End If
        'hide all columns except for the team name column
        For x = 0 To DGV.ColumnCount - 1
            DGV.Columns(x).Visible = False
            If DGV.Columns(x).Name = "COMPETITORNAME" Or DGV.Columns(x).Name = "TEAM" Then
                DGV.Columns(x).Visible = True
                DGV.Columns(x).HeaderText = "TEAM"
                DGV.Columns(x).DisplayIndex = colIndex
                colIndex += 1
            End If
        Next x

        'add columns that need to be calculated now, or make them visible if they're on the list
        For y = 0 To dgvDisplayItems.Rows.Count - 1
            If dgvDisplayItems.Rows(y).Cells("SortOrder").Value < 99 Then
                'check columns to add
                If dgvDisplayItems.Rows(y).Cells("Tag").Value.ToString.ToUpper = "TIMES PULLED UP" Or dgvDisplayItems.Rows(y).Cells("Tag").Value.ToString.ToUpper = "TIMES PULLED DOWN" Then
                    If DT.Columns.IndexOf("PulledUp") = -1 Then
                        DT.Columns.Add("PulledUp", System.Type.GetType("System.Int16"))
                        DT.Columns.Add("PulledDown", System.Type.GetType("System.Int16"))
                        Call AddPullups(DT)
                        DGV.Columns("PulledDown").Visible = False : DGV.Columns("PulledUp").Visible = False
                    End If
                End If
                If dgvDisplayItems.Rows(y).Cells("Tag").Value.ToString.ToUpper = "TIMES PULLED DOWN" Then DGV.Columns("PulledDown").Visible = True
                If dgvDisplayItems.Rows(y).Cells("Tag").Value.ToString.ToUpper = "TIMES PULLED UP" Then DGV.Columns("PulledUp").Visible = True
                If dgvDisplayItems.Rows(y).Cells("Tag").Value.ToString.ToUpper = "SIDE COUNTS" Then
                    DGV.Columns("Sides").Visible = True
                    DGV.Columns("Sides").DisplayIndex = dgvDisplayItems.Rows(y).Cells("SortOrder").Value
                    colIndex += 1
                End If
                For x = 0 To DGV.ColumnCount - 1
                    If dgvDisplayItems.Rows(y).Cells("Tag").Value.ToString.Trim.ToUpper = DGV.Columns(x).Name.Trim.ToUpper Then
                        DGV.Columns(x).Visible = True
                        DGV.Columns(x).DisplayIndex = dgvDisplayItems.Rows(y).Cells("SortOrder").Value
                        colIndex += 1
                        Exit For
                    End If
                Next x
            End If
        Next y

        'put active now in last column
        DGV.Columns("InRound").Visible = True
        DGV.Columns("InRound").DisplayIndex = colIndex
        DGV.Columns("InRound").SortMode = DataGridViewColumnSortMode.Automatic

    End Sub
    Sub BracketMarker(ByVal dt As DataTable)
        'updates the bracket table in the overall dataset, but only puts it in a bracket if it isn't in one already
        For x = 0 To dt.Rows.Count - 1
            'load current bracket
            Dim dr As DataRow() : Dim dr2 As DataRow
            dr = ds.Tables("Bracket").Select("Team=" & dt.Rows(x).Item("Competitor") & " and round=" & cboRound.SelectedValue)
            'if found, mark it; if not found, add it and set to wins
            If dr.Length = 1 Then
                dt.Rows(x).Item("Bracket") = dr(0).Item("WinBracket")
            ElseIf dr.Length = 0 Then
                dr2 = ds.Tables("Bracket").NewRow
                dr2.Item("Round") = cboRound.SelectedValue
                dr2.Item("Team") = dt.Rows(x).Item("Competitor")
                If strDebateType = "Policy" Then
                    dt.Rows(x).Item("Bracket") = dt.Rows(x).Item("Wins")
                    dr2.Item("WinBracket") = dt.Rows(x).Item("Wins")
                ElseIf strDebateType = "WUDC" Then
                    dt.Rows(x).Item("Bracket") = dt.Rows(x).Item("Team Ranks")
                    dr2.Item("WinBracket") = dt.Rows(x).Item("Team Ranks")
                End If
                ds.Tables("Bracket").Rows.Add(dr2)
            End If
        Next x
    End Sub
    Sub LoadPairings()
        'now load any existing pairing
        Call modShowThePairing(ds, dgvPairing, cboRound.SelectedValue, "Code")
        Dim nCols As Integer
        For x = 0 To dgvPairing.ColumnCount - 1
            If Mid(dgvPairing.Columns(x).Name, 1, 5).ToUpper = "JUDGE" Then dgvPairing.Columns(x).Visible = False
            If dgvPairing.Columns(x).Visible = True Then nCols += 1
        Next x
        If nCols > 3 Then dgvPairing.Font = New Font("Franklin Gothic Medium", 9)
        'set the cursor
        For x = 0 To dgvPairing.ColumnCount - 1
            For y = 0 To dgvPairing.RowCount - 1
                If dgvPairing.Columns(x).Visible = True And Mid(dgvPairing.Rows(y).Cells(x).Value.ToString.ToUpper, 1, 4) = "NONE" Then dgvPairing.CurrentCell = dgvPairing.Rows(y).Cells(x) : Exit For
            Next y
        Next x
    End Sub
    Sub AddPullups(ByRef dt As DataTable)
        'Pull all ballots by team
        'only consider those in powered rounds prior to this one
        'Get the records of the opponents and compare
        Dim x, y, youwin, theywin, PullUps, PullDowns As Integer
        Dim fdBallots, fdTeamsOnPanel As DataRow()
        Dim drRound, drPanel As DataRow
        'scroll through the teams, pull their opponents, and compare records
        Dim ProcessBallot As Boolean
        For x = 0 To dt.Rows.Count - 1
            PullUps = 0 : PullDowns = 0
            Label2.Text = x.ToString : Label2.Refresh()
            fdBallots = ds.Tables("Ballot").Select("Entry=" & dt.Rows(x).Item("Competitor"), "Panel ASC")
            For y = 0 To fdBallots.Length - 1
                'only process 1 ballot per panel
                ProcessBallot = False
                If y = 0 Then
                    ProcessBallot = True
                ElseIf fdBallots(y).Item("Panel") <> fdBallots(y - 1).Item("Panel") Then
                    ProcessBallot = True
                End If
                'only process if it's a power-matched round
                drPanel = ds.Tables("Panel").Rows.Find(fdBallots(y).Item("Panel"))
                drRound = ds.Tables("Round").Rows.Find(drPanel.Item("Round"))
                If drRound.Item("PairingScheme") <> "HighHigh" And drRound.Item("PairingScheme") <> "HighLow" Then ProcessBallot = False
                'only process if it's an earlier timeblock than the round you're pairing now
                If drRound.Item("timeslot") >= cboRound.SelectedValue Then ProcessBallot = False
                If ProcessBallot = True Then
                    fdTeamsOnPanel = ds.Tables("Ballot").Select("Panel=" & fdBallots(y).Item("Panel") & " and Entry<>" & dt.Rows(x).Item("Competitor"))
                    youwin = GetWinsForPullUps(ds, dt.Rows(x).Item("Competitor"), drRound.Item("ID"))
                    theywin = GetWinsForPullUps(ds, fdTeamsOnPanel(0).Item("Entry"), drRound.Item("ID"))
                    If theywin > youwin Then PullUps += 1
                    If theywin < youwin Then PullDowns += 1
                End If
            Next y
            dt.Rows(x).Item("PulledUp") = PullUps : dt.Rows(x).Item("PulledDown") = PullDowns
        Next x
    End Sub
    Sub AddSOPValues(ByRef dt As DataTable)
        Dim fdBallot, fdPanel As DataRow()
        Dim drTeam As DataRow
        Dim OppCt, OppSdTot As Integer
        Dim drRd As DataRow = ds.Tables("Round").Rows.Find(cboRound.SelectedValue)
        Dim drPanRd, drRdTS As DataRow
        For x = 0 To dt.Rows.Count - 1
            'pulls all ballots with the competitor on it
            fdBallot = ds.Tables("Ballot").Select("Entry=" & dt.Rows(x).Item("Competitor"))
            For y = 0 To fdBallot.Length - 1
                'pulls all ballots on the panel
                fdPanel = ds.Tables("Ballot").Select("Panel=" & fdBallot(y).Item("Panel") & " and Entry<>" & dt.Rows(x).Item("Competitor"))
                For z = 0 To fdPanel.Length - 1
                    'get the round the panel is in
                    drPanRd = ds.Tables("Panel").Rows.Find(fdPanel(z).Item("Panel"))
                    drRdTS = ds.Tables("Round").Rows.Find(drPanRd.Item("Round"))
                    If drRdTS.Item("Timeslot") < drRd.Item("Timeslot") Then
                        drTeam = dt.Rows.Find(fdPanel(z).Item("Entry"))
                        If Not drTeam Is Nothing Then OppSdTot += drTeam.Item("Seed") : OppCt += 1
                    End If
                Next
            Next y
            If dt.Rows(x).Item("Seed") Is System.DBNull.Value Then dt.Rows(x).Item("Seed") = 0
            dt.Rows(x).Item("SOP") = FormatNumber(dt.Rows(x).Item("Seed") + (OppSdTot / OppCt), 2)
            dt.Rows(x).Item("OppSeed") = FormatNumber((OppSdTot / OppCt), 2)
        Next x
    End Sub
    Sub LoadViewSettingsByRound() Handles cboRound.SelectedValueChanged
        'don't process if this is the first load
        If cboRound.SelectedValue.ToString = "System.Data.DataRowView" Then Exit Sub
        'Filter by round
        ds.Tables("ViewSettings").DefaultView.RowFilter = "Round=" & cboRound.SelectedValue.ToString
        'Populate if empty
        If ds.Tables("ViewSettings").DefaultView.Count = 0 Then Call MakeLoadSettings()
        'sort by order to appear
        ds.Tables("ViewSettings").DefaultView.Sort = "SortOrder ASC"
        'bind
        dgvDisplayItems.DataSource = ds.Tables("ViewSettings").DefaultView
    End Sub
    Private Sub butResetDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butResetDisplay.Click
        Call MakeLoadSettings()
    End Sub
    Private Sub butDumpRound_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butDumpRound.Click
        Call DeleteAllPanelsForRound(ds, cboRound.SelectedValue)
        Call LoadPairings()
        Call ResetBrackets()
        Call LoadRound()
        'ResetActives()
        'ClearCompetitorGrids()
    End Sub
    Sub ResetBrackets()
        'need to update the brackets table AND the datagrides
        If dtG1.Columns.Contains("Wins") = True And (strDebateType = "Policy" Or strDebateType = "Lincoln-Douglas") Then
            For x = 0 To dtG1.Rows.Count - 1
                dtG1.Rows(x).Item("Bracket") = dtG1.Rows(x).Item("Wins")
                Call UpdateTeamBracket(dtG1.Rows(x).Item("Competitor"), dtG1.Rows(x).Item("Wins"))
            Next
        End If
        If dtG1.Columns.Contains("Team Ranks") = True And strDebateType = "WUDC" Then
            For x = 0 To dtG1.Rows.Count - 1
                dtG1.Rows(x).Item("Bracket") = dtG1.Rows(x).Item("Team Ranks")
                Call UpdateTeamBracket(dtG1.Rows(x).Item("Competitor"), dtG1.Rows(x).Item("Team Ranks"))
            Next
        End If
        'For x = 0 To DataGridView1.RowCount - 1
        'DataGridView1.Rows(x).Cells("Bracket").Value = DataGridView1.Rows(x).Cells("Wins").Value
        'Next x
        'For x = 0 To dtG1.DefaultView.Count - 1
        'dtG1.DefaultView(x).Item("Bracket") = dtG1.DefaultView(x).Item("Wins")
        'Next x
        If dtG2 Is Nothing Then Exit Sub
        If dtG1.Columns.Contains("Wins") = False Then Exit Sub
        For x = 0 To dtG2.Rows.Count - 1
            dtG2.Rows(x).Item("Bracket") = dtG2.Rows(x).Item("Wins")
            Call UpdateTeamBracket(dtG2.Rows(x).Item("Competitor"), dtG2.Rows(x).Item("Wins"))
        Next
    End Sub
    Sub FillOutRound()
        'FIGURES OUT HOW MANY DEBATES THERE SHOULD BE PER ROUND, AND ADDS EXTRA BLANK SPOTS FOR ANY MISSING OPENINGS

        'pull round info to find the event
        Dim drRound As DataRow
        drRound = ds.Tables("Round").Rows.Find(cboRound.SelectedValue)
        'pull all teams in the event
        Dim teams As DataRow()
        teams = ds.Tables("Entry").Select("Event=" & drRound.Item("Event"))
        'find number of teams per debate
        Dim TeamsPerRd As Integer = getEventSetting(ds, drRound.Item("Event"), "TeamsPerRound")
        'get the number of debates based on the number of teams per round
        Dim nDebates As Integer = Int(teams.Length / TeamsPerRd)
        'add one for a partial round
        If Int(teams.Length / TeamsPerRd) <> teams.Length / TeamsPerRd Then nDebates += 1
        'now add blank panels to fill out the rest
        For x = 0 To nDebates - dgvPairing.Rows.Count
            Call AddPanel(ds, cboRound.SelectedValue, 0)
        Next x
    End Sub

    Private Sub butCreatePanelWithTeam_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butCreatePanelWithTeam.Click
        Dim team As Integer = GetSelectedTeam()
        If team < 0 Then
            Exit Sub
            MsgBox("Please select a team to include in the new panel that is created.")
        End If
        Dim panelID As Integer = AddPanel(ds, cboRound.SelectedValue, 0)
        AddTeamToPanel(ds, panelID, team, 1)
        Call LoadPairings()
        Dim dr As DataRow : dr = dtG1.Rows.Find(DataGridView1.CurrentRow.Cells("Competitor").Value) : dr.Item("InRound") = True
    End Sub

    Private Sub butAddTeamToSelectedPanel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butAddTeamToSelectedPanel.Click
        'get panel and team number
        Dim panel As Integer = dgvPairing.CurrentRow.Cells("Panel").Value
        Dim team As Integer = GetSelectedTeam() : If team < 0 Then Exit Sub
        'check teams are OK to debate
        Dim drRound, drPanel As DataRow
        drPanel = ds.Tables("Panel").Rows.Find(panel)
        drRound = ds.Tables("Round").Rows.Find(drPanel.Item("Round"))
        Dim nTeams As Integer = getEventSetting(ds, drRound.Item("Event"), "TeamsPerRound")
        Dim arrTeams(nTeams) As Integer
        Call UniqueItemsOnPanel(ds, panel, "Entry", nTeams, arrTeams)
        Dim strConflicts As String = ""
        For x = 0 To nTeams
            If arrTeams(x) <> -99 Then
                strConflicts &= CanDebate(ds, arrTeams(x), team)
            End If
        Next
        'Check sides are OK
        Dim Side As Integer = GetSideNumber(dgvPairing.Columns(dgvPairing.CurrentCell.ColumnIndex).HeaderText)
        strConflicts &= SideCheck(ds, team, Side, drRound.Item("ID"), DataGridView1.CurrentRow.Cells("Sides").Value)
        Dim q As Integer
        If strConflicts.Trim <> "" Then q = MsgBox(strConflicts.Trim & ". Continue with placement?", MsgBoxStyle.YesNo)
        If q = vbNo Then Exit Sub
        AddTeamToPanel(ds, panel, team, Side)
        Call LoadPairings()
        Dim dr As DataRow : dr = dtG1.Rows.Find(DataGridView1.CurrentRow.Cells("Competitor").Value) : dr.Item("InRound") = True
    End Sub
    Sub MarkActiveTeams(ByVal DT As DataTable)
        For x = 0 To DT.Rows.Count - 1
            If ActiveInTimeSlot(ds, DT.Rows(x).Item("Competitor"), cboRound.SelectedValue, "Entry") = True Then DT.Rows(x).Item("InRound") = True
        Next x
        Exit Sub
        'mark blank entries
        For x = 0 To DataGridView1.Rows.Count - 1
            If ActiveInTimeSlot(ds, DataGridView1.Rows(x).Cells("Competitor").Value, cboRound.SelectedValue, "Entry") = True Then DataGridView1.Item(1, x).Style.BackColor = Color.Red
        Next x
    End Sub
    Private Sub butDeleteFromPairing_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butDeleteFromPairing.Click
        Dim DeleteId As Integer = dgvPairing.CurrentRow.Cells(dgvPairing.CurrentCell.ColumnIndex - 1).Value
        Dim DT As DataTable
        Dim dr As DataRow
        DT = PullBallotsByRound(ds.Copy, "ENTRY", DeleteId, cboRound.SelectedValue)
        For x = 0 To DT.Rows.Count - 1
            dr = ds.Tables("Ballot").Rows.Find(DT.Rows(x).Item("ID"))
            dr.Item("Entry") = -99
        Next x
        Call LoadPairings()
        Call ResetActives()
        Call UpdateSides()
    End Sub

    Private Sub butDeleteOneDebate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butDeleteOneDebate.Click
        If dgvPairing.CurrentRow Is Nothing Then
            MsgBox("Please select one and only one row and try again.")
            Exit Sub
        End If
        Dim panel As Integer = dgvPairing.CurrentRow.Cells("Panel").Value
        Dim drPanel As DataRow : drPanel = ds.Tables("Panel").Rows.Find(panel)
        drPanel.Delete()
        ds.AcceptChanges()
        Call LoadPairings()
        Call ResetActives()
        Call ClearCompetitorGrids()
        Call UpdateSides()
    End Sub
    Sub ResetActives()
        'Updates which teams are and are not scheduled to debate in the current round
        Dim strActive As String : Dim ctr As Integer
        strActive = DataGridView1.RowCount & " teams in column 1, "
        Dim dr As DataRow
        For x = 0 To DataGridView1.RowCount - 1
            dr = dtG1.Rows.Find(DataGridView1.Rows(x).Cells("Competitor").Value)
            dr.Item("InRound") = False
            If ActiveInTimeSlot(ds, dr.Item("Competitor"), cboRound.SelectedValue, "Entry") = True Then
                ctr += 1
                dr.Item("InRound") = True
            End If
        Next x
        strActive &= ctr & " of which are currently paired. " : Label2.Text = strActive
        If DataGridView2.Visible = False Then Exit Sub
        ctr = 0
        strActive &= DataGridView2.RowCount & " teams in column 2, "
        For x = 0 To DataGridView2.RowCount - 1
            dr = dtG2.Rows.Find(DataGridView2.Rows(x).Cells("Competitor").Value)
            dr.Item("InRound") = False
            If ActiveInTimeSlot(ds, dr.Item("Competitor"), cboRound.SelectedValue, "Entry") = True Then
                ctr += 1
                dr.Item("InRound") = True
            End If
        Next x
        strActive &= ctr & " of which are currently paired. "
        Label2.Text = strActive
    End Sub
    Sub SetSideConstrainedRD() Handles cboRound.SelectedValueChanged
        'if it's an even round, check the side constrained box
        If cboRound.SelectedValue.ToString = "System.Data.DataRowView" Then Exit Sub
        Dim drRound As DataRow : drRound = ds.Tables("Round").Rows.Find(cboRound.SelectedValue)
        Dim nTeams As Integer = getEventSetting(ds, drRound.Item("Event"), "TeamsPerRound")
        Dim debateType As String = getEventSetting(ds, drRound.Item("Event"), "Type")
        If nTeams > 2 Or (debateType = "WUDC" Or debateType = "Other") Then Exit Sub
        chkSideConstrained.Checked = False
        Dim dr As DataRow : dr = ds.Tables("Round").Rows.Find(cboRound.SelectedValue)
        If dr.Item("rd_name") / 2 = Int(dr.Item("rd_name") / 2) Then chkSideConstrained.Checked = True
    End Sub
    Private Sub butPairTeams_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butPairTeams.Click
        Dim drRound As DataRow : drRound = ds.Tables("Round").Rows.Find(cboRound.SelectedValue)
        Dim nTeams As Integer = getEventSetting(ds, drRound.Item("Event"), "TeamsPerRound")
        If DataGridView2.Visible = True Then
            Call DoSideConstrainedAdd(nTeams)
        Else
            Call doNoSideConstrainedAdd(nTeams)
        End If
        'reset screen
        Call PostPairScreenReset()
    End Sub
    Sub doNoSideConstrainedAdd(ByVal nteams)
        Dim teams(nteams) As Integer
        'check no more teams selected than debate in a round
        Dim ctr As Integer : Dim activeNow As Boolean
        For x = 0 To DataGridView1.RowCount - 1
            If DataGridView1.Rows(x).Selected = True Then
                ctr += 1
                If ctr > nteams Then
                    MsgBox("You have selected more teams than appear in a round for this division.  Please select only " & nteams & " teams and try again.")
                    Exit Sub
                End If
                If DataGridView1.Rows(x).Cells("InRound").Value Is System.DBNull.Value Then DataGridView1.Rows(x).Cells("InRound").Value = False
                If DataGridView1.Rows(x).Cells("InRound").Value = True Then activeNow = True
                teams(ctr) = DataGridView1.Rows(x).Cells("Competitor").Value
            End If
        Next
        'check no selected teams are debating now
        If activeNow = True Then
            MsgBox("One or more of the teams selected to debate is already scheduled for this round.  Either delete them from the debate they are currently paired in or select a different team.")
            Exit Sub
        End If
        'check all selected teams can debate
        Dim strError As String = ""
        For x = 1 To nteams - 1
            For y = x + 1 To nteams
                strError &= CanDebate(ds, teams(x), teams(y))
            Next y
        Next x
        Dim q As Integer
        If strError <> "" Then q = MsgBox(strError, MsgBoxStyle.YesNo)
        If q = vbNo Then Exit Sub
        'process pairing
        Dim panel As Integer = AddPanel(ds, cboRound.SelectedValue, 0)
        For x = 1 To nteams
            'add code to snake sides here
            AddTeamToPanel(ds, panel, teams(x), x)
        Next
        'refresh grid
        'Call ResetActives()
        'If chkPairedToBottom.Checked = True Then
        'DataGridView1.Sort(DataGridView1.Columns("InROund"), System.ComponentModel.ListSortDirection.Ascending)
        'End If
        'Call LoadPairings()
    End Sub
    Sub DoSideConstrainedAdd(ByVal nTeams As Integer)
        'check for one and only one team on each grid

        'clean up null values
        If DataGridView1.CurrentRow.Cells("InRound").Value Is System.DBNull.Value Then DataGridView1.CurrentRow.Cells("InRound").Value = False
        If DataGridView2.CurrentRow.Cells("InRound").Value Is System.DBNull.Value Then DataGridView2.CurrentRow.Cells("InRound").Value = False

        'check teams not debating now
        If DataGridView1.CurrentRow.Cells("InRound").Value = True Then
            MsgBox("The affirmative team is already scheduled to debate.  Select another team to pair, or remove this team from the current round.")
            Exit Sub
        End If
        If DataGridView2.CurrentRow.Cells("InRound").Value = True Then
            MsgBox("The negative team is already scheduled to debate.  Select another team to pair, or remove this team from the current round.")
            Exit Sub
        End If
        Dim team1 As Integer = DataGridView1.CurrentRow.Cells("Competitor").Value
        Dim team2 As Integer = DataGridView2.CurrentRow.Cells("Competitor").Value
        'check teams are OK to debate
        Dim strConflicts As String = ""
        strConflicts &= CanDebate(ds, team1, team2)
        Dim q As Integer
        If strConflicts.Trim <> "" Then
            q = MsgBox(strConflicts.Trim & ". Continue with placement?", MsgBoxStyle.YesNo)
            If q = vbNo Then Exit Sub
        End If
        'Perform add and update screen
        Dim panel As Integer = AddPanel(ds, cboRound.SelectedValue, 0)
        AddTeamToPanel(ds, panel, team1, 1)
        AddTeamToPanel(ds, panel, team2, 2)

    End Sub

    Private Sub butShowBracket_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butShowBracket.Click
        Call LoadBracket()
    End Sub
    Sub LoadBracket()
        Call FilterByBracket()
        Call ClearCompetitorGrids()
        Call ResetActives()
    End Sub
    Sub FilterByBracket()
        Dim dtv As DataView
        dtv = DataGridView1.DataSource
        If txtBracket.Text.Trim = "" Then
            dtv.RowFilter = ""
        Else
            dtv.RowFilter = "Bracket=" & txtBracket.Text
        End If
        DataGridView1.DataSource = dtv

        If DataGridView2.Visible = False Then Exit Sub

        dtv = DataGridView2.DataSource
        If txtBracket.Text.Trim = "" Then
            dtv.RowFilter = ""
        Else
            dtv.RowFilter = "Bracket=" & txtBracket.Text
        End If
        DataGridView2.DataSource = dtv

    End Sub

    Private Sub butClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butClear.Click
        Call ClearCompetitorGrids()
    End Sub
    Sub ClearCompetitorGrids()
        For x = 0 To DataGridView1.RowCount - 1
            DataGridView1.Item(1, x).Style.BackColor = Color.White
            If DataGridView1.Rows(x).Cells("InRound").Value Is System.DBNull.Value Then DataGridView1.Rows(x).Cells("InRound").Value = False
            If DataGridView1.Rows(x).Cells("InRound").Value = True Then DataGridView1.Item(1, x).Style.BackColor = Color.LightBlue
            DataGridView1.Rows(x).Selected = False
        Next x
        If DataGridView2.Visible = False Then Exit Sub
        For x = 0 To DataGridView2.RowCount - 1
            DataGridView2.Item(1, x).Style.BackColor = Color.White
            If DataGridView2.Rows(x).Cells("InRound").Value Is System.DBNull.Value Then DataGridView2.Rows(x).Cells("InRound").Value = False
            If DataGridView2.Rows(x).Cells("InRound").Value = True Then DataGridView2.Item(1, x).Style.BackColor = Color.LightBlue
            DataGridView2.Rows(x).Selected = False
        Next x
    End Sub

    Private Sub butMoveUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butMoveUp.Click
        Call DoMove("UP")
    End Sub
    Sub DoMove(ByVal strMoveType As String)

        'check only one row selected in both columns combined
        Dim ctr As Integer = 0
        For x = 0 To DataGridView1.RowCount - 1
            If DataGridView1.Rows(x).Selected = True Then ctr += 1
        Next
        If DataGridView2.Visible = True Then
            For x = 0 To DataGridView2.RowCount - 1
                If DataGridView2.Rows(x).Selected = True Then ctr += 1
            Next
        End If
        If ctr > 1 Then MsgBox("Please select one and only one team to be moved up", MsgBoxStyle.OkOnly) : Exit Sub

        'move down unless moveup specified
        Dim unit As Integer = -1
        If strMoveType.ToUpper = "UP" Then unit = 1

        'process the change on the underlying datatable
        Dim dr As DataRow
        For x = 0 To DataGridView1.Rows.Count - 1
            If DataGridView1.Rows(x).Selected = True Then
                dr = dtG1.Rows.Find(DataGridView1.Rows(x).Cells("Competitor").Value)
                dr.Item("Bracket") = dr.Item("Bracket") + unit
                dtG1.AcceptChanges()
                Call UpdateTeamBracket(dr.Item("Competitor"), dr.Item("Bracket"))
                Exit Sub
            End If
        Next
        For x = 0 To DataGridView2.RowCount - 1
            If DataGridView2.Rows(x).Selected = True Then
                dr = dtG2.Rows.Find(DataGridView2.Rows(x).Cells("Competitor").Value)
                dr.Item("Bracket") = dr.Item("Bracket") + unit
                dtG2.AcceptChanges()
                Call UpdateTeamBracket(dr.Item("Competitor"), dr.Item("Bracket"))
                Exit Sub
            End If
        Next x
    End Sub

    Private Sub butMoveDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butMoveDown.Click
        Call DoMove("Down")
    End Sub

    Private Sub butUpBracket_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butUpBracket.Click
        txtBracket.Text = Val(txtBracket.Text) + 1
        Call FilterByBracket()
        Call ClearCompetitorGrids()
        Call ResetActives()
    End Sub

    Private Sub butDownBracket_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butDownBracket.Click
        txtBracket.Text = Val(txtBracket.Text) - 1
        Call FilterByBracket()
        Call ClearCompetitorGrids()
        Call ResetActives()
    End Sub
    Sub UpdateTeamBracket(ByVal Team As Integer, ByVal WinBracket As Integer)
        Dim dr As DataRow() : Dim dr2 As DataRow
        dr = ds.Tables("Bracket").Select("Team=" & Team & " and round=" & cboRound.SelectedValue)
        If dr.Length = 1 Then
            dr(0).Item("WinBracket") = WinBracket
        ElseIf dr.Length = 0 Then
            dr2 = ds.Tables("Bracket").NewRow
            dr2.Item("Round") = cboRound.SelectedValue
            dr2.Item("Team") = Team
            dr2.Item("WinBracket") = WinBracket
            ds.Tables("Bracket").Rows.Add(dr2)
        End If
    End Sub
    Sub Grid1Click() Handles DataGridView1.Click
        If DataGridView2.Visible = True Then Call MarkTeamConflicts(DataGridView1, DataGridView2) : Exit Sub
        Call MarkTeamConflicts(DataGridView1, DataGridView1)
        If DataGridView1.SelectedRows.Count = 2 Then butPairTeams.Focus()
    End Sub
    Sub Grid2Click() Handles DataGridView2.Click
        Call MarkTeamConflicts(DataGridView2, DataGridView1)
        butPairTeams.Focus()
    End Sub
    Sub MarkTeamConflicts(ByVal dgv1 As DataGridView, ByVal dgv2 As DataGridView)
        Dim strTime As String : strTime = Now.Second & " " & Now.Millisecond & Chr(10) & Chr(10)
        'exit if no dg1 team selected
        If dgv1.CurrentRow Is Nothing Then Exit Sub
        Dim Team1 As Integer = dgv1.CurrentRow.Cells("Competitor").Value

        'clear first grid of red markings
        strTime &= "CLEAR GRID 1: " & Now.Second & " " & Now.Millisecond & Chr(10) & Chr(10)
        For x = 0 To dgv1.Rows.Count - 1
            If dgv1.Rows(x).Selected = False Then
                'mark conflicts as nothing
                If dgv1.Item(1, x).Style.BackColor = Color.Red Then dgv1.Item(1, x).Style.BackColor = Color.White
                're-mark if debating
                If dgv1.Rows(x).Cells("InRound").Value Is System.DBNull.Value Then dgv1.Rows(x).Cells("InRound").Value = False
                If dgv1.Rows(x).Cells("InRound").Value = True Then dgv1.Item(1, x).Style.BackColor = Color.LightBlue
            End If
        Next x

        'marks teams on opposite side that can't hear the selected team
        strTime &= "PROCESS GRID 2: " & Now.Second & " " & Now.Millisecond & Chr(10) & Chr(10)
        For x = 0 To dgv2.Rows.Count - 1
            dgv2.Item(1, x).Style.BackColor = Color.White
            If dgv2.Rows(x).Cells("InRound").Value Is System.DBNull.Value Then dgv2.Rows(x).Cells("InRound").Value = False
            If dgv2.Rows(x).Cells("InRound").Value = True Then dgv2.Item(1, x).Style.BackColor = Color.LightBlue
            If CanDebate(ds, Team1, dgv2.Rows(x).Cells("Competitor").Value) <> "" Then dgv2.Item(1, x).Style.BackColor = Color.Red
        Next x
        strTime &= "END: " & Now.Second & " " & Now.Millisecond
        'MsgBox(strTime)
    End Sub

    Private Sub butDeleteBracketDebates_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butDeleteBracketDebates.Click
        'Make sure that a bracket is displayed, otherwise, exit
        If txtBracket.Text.Trim = "" Then
            MsgBox("There does not appear to be a bracket displayed; if you want to delete and entire round use the button in the lower-left beneanth the pairings box.  Otherwise, load a bracket and try again.")
            Exit Sub
        End If
        'get a confirm
        Dim q As Integer = MsgBox("This will erase ALL the debates in this bracket that have been paired.  Click YES to continue and delete them, or NO to exit.", MsgBoxStyle.YesNo)
        If q = vbNo Then Exit Sub
        'scroll through all teams displayed in the grids, pull the panels for those teams in this round, and delete
        Dim dt As DataTable : Dim drPanel, drBallot As DataRow
        For x = 0 To DataGridView1.Rows.Count - 1
            dt = PullBallotsByRound(ds, "Entry", DataGridView1.Rows(x).Cells("Competitor").Value, cboRound.SelectedValue)
            For y = 0 To dt.Rows.Count - 1
                drBallot = ds.Tables("Ballot").Rows.Find(dt.Rows(y).Item(0))
                If Not drBallot Is Nothing Then
                    drPanel = ds.Tables("Panel").Rows.Find(drBallot.Item("Panel"))
                    If Not drPanel Is Nothing Then drPanel.Delete()
                End If
            Next y
        Next x
        'clean up screen
        ds.AcceptChanges()
        Call LoadPairings()
        Call ResetActives()
        Call ClearCompetitorGrids()
    End Sub

    Private Sub butAutoHighLow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butAutoHighLow.Click
        If txtBracket.Text = "" Then
            Dim q = MsgBox("This function is to pair within a bracket, but no bracket has been selected.  If you continue, this will conduct a high-low pair on the WHOLE tournament, NOT just an individual bracket.  Click YES to continue or NO to exit.", MsgBoxStyle.YesNo)
            If q = vbNo Then Exit Sub
        End If
        If DataGridView2.Visible = False Then Call OddPair("HL", ExitAutoPair) : Exit Sub
        Call EvenPair(DataGridView2.Rows.Count - 1, 0, -1, ExitAutoPair)
    End Sub
    Sub OddPair(ByVal strPairType As String, ByRef ExitAutoPair As Boolean)
        'auto pairs a round with no side constraints; only really works for 2-team systems
        Dim ctr, panel, intStart, intStop, intStep, side1, side2 As Integer
        For x = 0 To DataGridView1.Rows.Count - 1
            If DataGridView1.Rows(x).Cells("InRound").Value = False Then
                ctr = 0
                If strPairType = "HH" Then intStart = x + 1 : intStop = DataGridView1.RowCount - 1 : intStep = 1
                If strPairType = "HL" Then intStart = DataGridView1.RowCount - 1 : intStop = x + 1 : intStep = -1
                For y = intStart To intStop Step intStep
                    If DataGridView1.Rows(y).Cells("InRound").Value = False Then
                        'check to see if it's time to exit and let the user fix it
                        ctr += 1
                        If ctr >= 3 Or (ctr > 1 And y = intStop) Then
                            MsgBox("There appears to be a problem that requires manual correction.  Please pair the next team in the affirmative bracket manually and continue.", MsgBoxStyle.OkOnly)
                            ExitAutoPair = True
                            Call PostPairScreenReset()
                            Exit Sub
                        End If
                        'it's not, so see if you can pair.  If not, it scrolls to the next team
                        If CanDebate(ds, DataGridView1.Rows(x).Cells("Competitor").Value, DataGridView1.Rows(y).Cells("Competitor").Value) = "" Then
                            panel = AddPanel(ds, cboRound.SelectedValue, 0)
                            Call GetSide(side1, side2)
                            AddTeamToPanel(ds, panel, DataGridView1.Rows(x).Cells("Competitor").Value, side1)
                            AddTeamToPanel(ds, panel, DataGridView1.Rows(y).Cells("Competitor").Value, side2)
                            DataGridView1.Rows(x).Cells("InRound").Value = True
                            DataGridView1.Rows(y).Cells("InRound").Value = True
                            Exit For
                        End If
                    End If
                    If y = intStop And DataGridView1.Rows(y).Cells("InRound").Value = False Then
                        MsgBox("There appears to be a problem that requires manual correction.  Please address it before pairing the next bracket.", MsgBoxStyle.OkOnly)
                        ExitAutoPair = True
                    End If
                Next y
            End If
        Next x
        Call PostPairScreenReset()
        DataGridView1.Refresh()

    End Sub
    Sub GetSide(ByRef side1 As Integer, ByRef side2 As Integer)
        'returns sides for odd numbered rounds; alternates whether the higher seed is aff or neg
        side1 = 1 : side2 = 2
        Dim fdpanels As DataRow()
        fdpanels = ds.Tables("Panel").Select("Round=" & cboRound.SelectedValue)
        If fdpanels.Length / 2 = Int(fdpanels.Length / 2) Then side1 = 2 : side2 = 1
    End Sub
    Sub EvenPair(ByVal intStart As Integer, ByVal intStop As Integer, ByVal intStep As Integer, ByRef ExitAutoPair As Boolean)
        Dim ctr, panel As Integer
        For x = 0 To DataGridView1.Rows.Count - 1
            If DataGridView1.Rows(x).Cells("InRound").Value = False Then
                ctr = 0
                For y = intStart To intStop Step intStep
                    If DataGridView2.Rows(y).Cells("InRound").Value = False Then
                        'check to see if it's time to exit and let the user fix it
                        ctr += 1
                        If ctr >= 3 Or (ctr > 1 And y = intStop) Then
                            MsgBox("There appears to be a problem that requires manual correction.  Please pair the next team in the affirmative bracket manually and continue.", MsgBoxStyle.OkOnly)
                            Call PostPairScreenReset()
                            exitautopair = True
                            Exit Sub
                        End If
                        'it's not, so see if you can pair.  If not, it scrolls to the next team
                        If CanDebate(ds, DataGridView1.Rows(x).Cells("Competitor").Value, DataGridView2.Rows(y).Cells("Competitor").Value) = "" Then
                            panel = AddPanel(ds, cboRound.SelectedValue, 0)
                            AddTeamToPanel(ds, panel, DataGridView1.Rows(x).Cells("Competitor").Value, 1)
                            AddTeamToPanel(ds, panel, DataGridView2.Rows(y).Cells("Competitor").Value, 2)
                            DataGridView1.Rows(x).Cells("InRound").Value = True
                            DataGridView2.Rows(y).Cells("InRound").Value = True
                            Exit For
                        End If
                    End If
                    If y = intStop And DataGridView2.Rows(y).Cells("InRound").Value = False Then
                        MsgBox("There appears to be a problem that requires manual correction.  Please address it before pairing the next bracket.", MsgBoxStyle.OkOnly)
                        exitautopair = True
                    End If
                Next y
            End If
        Next x
        Call PostPairScreenReset()
    End Sub
    Sub PostPairScreenReset()
        'reset screen
        Call LoadPairings()
        Call ResetActives()
        If chkPairedToBottom.Checked = True Then
            DataGridView1.Sort(DataGridView1.Columns("InRound"), System.ComponentModel.ListSortDirection.Ascending)
            If DataGridView2.Visible = True Then
                DataGridView2.Sort(DataGridView2.Columns("InROund"), System.ComponentModel.ListSortDirection.Ascending)
            End If
        End If
        Call ClearCompetitorGrids()
    End Sub

    Private Sub butPairHighHigh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butPairHighHigh.Click
        If txtBracket.Text = "" Then
            Dim q = MsgBox("This function is to pair within a bracket, but no bracket has been selected.  If you continue, this will conduct a high-high pair on the WHOLE tournament, NOT just an individual bracket.  Click YES to continue or NO to exit.", MsgBoxStyle.YesNo)
            If q = vbNo Then Exit Sub
        End If
        If DataGridView2.Visible = False Then
            'check that all teams are dumped in one big bracket
            Call OddPair("HH", ExitAutoPair) : Exit Sub
        End If
        'check that there are an even number of teams
        Call EvenPair(0, DataGridView2.Rows.Count - 1, 1, ExitAutoPair)
    End Sub

    Private Sub butRandomSeeds_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butRandomSeeds.Click
        Call Randomizer()
    End Sub
    Sub Randomizer()
        For x = 0 To DataGridView1.RowCount - 1
            DataGridView1.Rows(x).Cells("Seed").Value = Int(Rnd() * 1000)
        Next x
        Dim dtv As DataView = DataGridView1.DataSource
        dtv.Sort = "Seed ASC"
        If DataGridView2.Visible = False Then Exit Sub
        For x = 0 To DataGridView2.RowCount - 1
            DataGridView2.Rows(x).Cells("Seed").Value = Int(Rnd() * 1000)
        Next x
        dtv = DataGridView2.DataSource
        dtv.Sort = "Seed ASC"
        DataGridView2.Refresh()
    End Sub
    Private Sub butAssignBye_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butAssignBye.Click
        Dim team As Integer = GetSelectedTeam() : If team < 0 Then Exit Sub
        If HadBye(ds, cboRound.SelectedValue, team) > 0 Then
            Dim q As Integer = MsgBox("This team as already reacieved a bye.  Assign then another?", MsgBoxStyle.YesNo)
            If q = vbNo Then Exit Sub
        End If
        Dim dummy As String = AssignBye(ds, -1, team, cboRound.SelectedValue)
        If dummy <> "" Then
            MsgBox(dummy, MsgBoxStyle.OkOnly)
        End If
        Call PostPairScreenReset()
    End Sub
    Function GetSelectedTeam() As Integer
        GetSelectedTeam = -1
        Dim selectedRowCount As Integer = DataGridView1.Rows.GetRowCount(DataGridViewElementStates.Selected)
        If selectedRowCount > 1 Then
            MsgBox("Please select one and only one team")
        ElseIf selectedRowCount = 1 Then
            GetSelectedTeam = DataGridView1.SelectedRows(0).Cells("Competitor").Value
        End If
        'if it's an odd round you're good
        If DataGridView2.Visible = False Then Exit Function
        'if not, test to see if there's a team in datagridview2
        selectedRowCount = DataGridView2.Rows.GetRowCount(DataGridViewElementStates.Selected)
        If selectedRowCount = 1 And GetSelectedTeam > 0 Then
            MsgBox("You have selected one team from each grid.  Please select one and only team and try again.", MsgBoxStyle.OkOnly)
        End If
        If selectedRowCount = 1 And GetSelectedTeam = -1 Then GetSelectedTeam = DataGridView2.SelectedRows(0).Cells("Competitor").Value : Exit Function
        MsgBox("No team appears to be selected.  Please select one and only team and try again.", MsgBoxStyle.OkOnly)
    End Function

    Private Sub butFlipSide_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butFlipSide.Click
        If DataGridView2.Visible = False Then
            MsgBox("This function flips a team from the aff bracket to the neg bracket or vice versa; it can only be used in side constrainted rounds.")
            Exit Sub
        End If
        Dim nSelected, sideSelected As Integer
        If DataGridView1.SelectedRows.Count > 0 Then
            nSelected += DataGridView1.SelectedRows.Count : sideSelected = 1
        End If
        If DataGridView2.SelectedRows.Count > 0 Then
            nSelected += DataGridView2.SelectedRows.Count : sideSelected = 2
        End If
        If nSelected > 1 Then
            MsgBox("Please select only one team from the grids above, do NOT select one team from each grid.  This function will move a single team from one grid to the other.  Hit the CLEAR SELECTION button and try again.", MsgBoxStyle.OkOnly)
            Exit Sub
        End If
        If sideSelected = 1 Then
            Dim dr As DataRowView = DataGridView2.DataSource.addnew()
            For x = 0 To DataGridView1.ColumnCount - 1
                dr.Item(x) = DataGridView1.CurrentRow.Cells(x).Value
            Next x
            dr.EndEdit()
            DataGridView1.Rows.Remove(DataGridView2.CurrentRow)
        ElseIf sideSelected = 2 Then
            Dim dr As DataRowView = DataGridView1.DataSource.addnew()
            For x = 0 To DataGridView2.ColumnCount - 1
                dr.Item(x) = DataGridView2.CurrentRow.Cells(x).Value
            Next x
            dr.EndEdit()
            DataGridView2.Rows.Remove(DataGridView2.CurrentRow)
        End If
        Call ResetActives()
    End Sub

    Private Sub butBasicInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butPageHelpInfo.Click
        Dim strInfo As String = ""
        strInfo &= "This page will help you pair a round manually, with lots of computer assistance.  It is most similar to the computer assited power match screen on the TRPC." & Chr(10) & Chr(10)
        strInfo &= "The grid on the far left will display your current pairigns for the round; the grids in the middle show the teams and information about them, and the grids to the far right only control how the round is loaded." & Chr(10) & Chr(10)
        strInfo &= "Begin by loading a round; select a round from the drop-down list in this section and clicking the LOAD button at the bottom.  When you do, team information will load." & Chr(10) & Chr(10)
        strInfo &= "You can customize the information displayed about teams by using the grid just below the ROUND drop-down list." & Chr(10) & Chr(10)
        strInfo &= "Entering any number lower than 99 will cause the column to display on the team grid; entering 99 means the column will not display." & Chr(10) & Chr(10)
        strInfo &= "For all preset rounds, the columns will ONLY display team rankings under the SEED column." & Chr(10) & Chr(10)
        strInfo &= "If you are using a 2-team policy division and it is a side constrained round, teams will appear in 2 columns.  Otherwise, all teams will appear in a single column.  Note that you can manually change whether the round is side constrained by checking or unchecking the 'side constrained round' box before loading." & Chr(10) & Chr(10)
        strInfo &= "The CAT will automatically detect whether it is a side constrained round, but you can manually force a side constrained round by checking the box at the bottom of the right-hand grid." & Chr(10) & Chr(10)
        strInfo &= "SOP stands for Seed+Opposition Seed, and is used in Gary Larson's advanced power-matching system." & Chr(10) & Chr(10)
        strInfo &= "More info on the SOP system can be found on the CEDA forums." & Chr(10) & Chr(10)
        strInfo &= "For information on how to change information in the pairing grid or use the computer assist buttons, click the help buttons in those sections. " & Chr(10) & Chr(10)
        MsgBox(strInfo, , "Basic Manual Pairing Page information")
    End Sub

    Private Sub butPairingHlep_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butPairingHlep.Click
        Dim strInfo As String = ""
        strInfo &= "PAIRING A 2-TEAM EVENT (Policy, parli, LD)" & Chr(10) & Chr(10)
        strInfo &= "If it is NOT a side-constrained round, select 2 teams from the grid and click the PAIR SELECTED TEAMS button.  Select the second team by holding down the control button while clicking.  If it is side-constrained, select one team from each column." & Chr(10) & Chr(10)
        strInfo &= "The autopair buttons will perform an autopair on the displayed bracket." & Chr(10) & Chr(10)
        strInfo &= "To pair a round randomly, put all the teams in a single bracket, click the RANDOMIZE SEEDS button, sort each grid by seed (or the only grid in a non-side constrained round), and click either of the AutoPair buttons." & Chr(10) & Chr(10)
        strInfo &= "Use the buttons on the right to display individual brackets and move teams in and out of win brackets." & Chr(10) & Chr(10)
        strInfo &= "The white box to the left of the SHOW BRACKET button indicated the bracket to display; for example, entering a 5 will show the 5-win bracket." & Chr(10) & Chr(10)
        strInfo &= "The up and down bracket buttons will show the next bracket up or down.  To move a TEAM up or down a bracket, click on the team in the grid and then the move team up or move team down button." & Chr(10) & Chr(10)
        strInfo &= "If you have already started assigning pairings for a bracket and want to erase them all and start over, click the DUMP ALL DEBATES IN BRACKET button" & Chr(10) & Chr(10)
        strInfo &= "PAIRING A 4-TEAM EVENT (Worlds format)" & Chr(10) & Chr(10)
        strInfo &= "PAIRING A 2-TEAM EVENT (Policy, parli, LD)" & Chr(10) & Chr(10)
        MsgBox(strInfo, , "Basic Manual Pairing Page information")
    End Sub

    Private Sub butCopyDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butCopyDisplay.Click
        Dim dr As DataRow
        Dim dt As DataTable = ds.Tables("ViewSettings").Clone
        'copy grid to a new table
        dt.Clear()
        For x = 0 To dgvDisplayItems.RowCount - 1
            dr = dt.NewRow()
            dr.Item("SortOrder") = dgvDisplayItems.Rows(x).Cells("SortOrder").Value
            dr.Item("Tag") = dgvDisplayItems.Rows(x).Cells("Tag").Value
            dt.Rows.Add(dr)
        Next x
        'scroll through the rounds, if power-matched erase current settings and populate with those above
        For y = 0 To ds.Tables("Round").Rows.Count - 1
            If ds.Tables("Round").Rows(y).Item("PairingScheme").toupper <> "PRESET" Then
                'delete current settings
                For x = ds.Tables("ViewSettings").Rows.Count - 1 To 0 Step -1
                    If ds.Tables("ViewSettings").Rows(x).Item("Round") = ds.Tables("Round").Rows(y).Item("ID") Then
                        ds.Tables("ViewSettings").Rows(x).Delete()
                    End If
                Next x
                'copy those on datagrid
                For x = 0 To dt.Rows.Count - 1
                    dr = ds.Tables("ViewSettings").NewRow()
                    dr.Item("Round") = ds.Tables("Round").Rows(y).Item("ID")
                    dr.Item("SortOrder") = dt.Rows(x).Item("SortOrder")
                    dr.Item("Tag") = dt.Rows(x).Item("Tag")
                    ds.Tables("ViewSettings").Rows.Add(dr)
                Next x
            End If
        Next y
        ds.Tables("ViewSettings").AcceptChanges()
    End Sub

    Private Sub butCheckRound_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butCheckRound.Click
        MsgBox(AuditRound(ds, cboRound.SelectedValue) & " " & TimeSlotAudit(ds, cboRound.SelectedValue))
    End Sub

    Private Sub butBracketReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butBracketReset.Click
        Call ResetBrackets()
        DataGridView1.DataSource = Nothing
        DataGridView2.DataSource = Nothing
        MsgBox("Done -- reload round")
    End Sub

    Private Sub butFlipSidePaired_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butFlipSidePaired.Click
        If dgvPairing.CurrentRow Is Nothing Then
            MsgBox("Please select one and only one row and try again.")
            Exit Sub
        End If
        Dim panel As Integer = dgvPairing.CurrentRow.Cells("Panel").Value
        Dim drPanel As DataRow : drPanel = ds.Tables("Panel").Rows.Find(panel)
        Call SideFlipper(ds, drPanel.Item("ID"))
        ds.AcceptChanges()
        Call LoadPairings()
        Call ResetActives()
    End Sub
    Sub ClearGrids() Handles cboRound.SelectedValueChanged
        DataGridView1.DataSource = Nothing
        dgvPairing.DataSource = Nothing
        DataGridView2.DataSource = Nothing
    End Sub
    Sub SetScreenByDebateType()
        If strDebateType = "WUDC" Then
            butPairHighHigh.Visible = False
            butAutoHighLow.Visible = False
            butAssignBye.Visible = False
            butAutoPair.Visible = False
            butAutoInfo.Visible = False
            'butRandomSeeds.Visible = False
            butWUDCTest.Visible = True : butWUDCTest.Location = New Point(215, 46)
            'butRandomPair.Visible = True : butRandomPair.Location = New Point(215, 74)
            butWUDCAutoPair.Visible = True : butWUDCAutoPair.Location = New Point(215, 104)
        End If
    End Sub

    Private Sub butWUDCTest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butWUDCTest.Click
        Dim nteams As Integer = DataGridView1.RowCount
        Dim strMessage As String = ""
        strMessage &= nteams & " teams"
        If Int(nteams / 4) <> (nteams / 4) Then strMessage &= " -- NOT DIVISIBLE BY FOUR!!!" Else strMessage &= ", which is evenly divisible by four."
        MsgBox(strMessage)
    End Sub

    Private Sub butWUDCAutoPair_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butWUDCAutoPair.Click
        'check backet first?
        If Int(DataGridView1.RowCount / 4) <> (DataGridView1.RowCount / 4) Then
            MsgBox("Bracket not even -- make sure there is a number of teams divisible by 4 and try again.")
            Exit Sub
        End If

        'declare stuff
        Dim dr, drRd As DataRow
        drRd = ds.Tables("Round").Rows.Find(cboRound.SelectedValue)
        Dim nTeams As Integer = getEventSetting(ds, drRd.Item("Event"), "TeamsPerRound")
        Dim strDummy, strSort, strSide, strColName As String

        'create enough blank panels for this bracket
        Dim nUnPaired As Integer
        For x = 0 To DataGridView1.Rows.Count - 1
            If DataGridView1.Rows(x).Cells("InRound").Value = False Then nUnPaired += 1
        Next x
        For x = 0 To Int(nUnPaired / 4) - 1
            Call AddPanel(ds, cboRound.SelectedValue, 0)
        Next
        Call LoadPairings()

        'create table
        Dim dt As New DataTable
        dt.Columns.Add("Competitor", System.Type.GetType("System.Int64"))
        For x = 1 To nTeams
            dt.Columns.Add("Side" & x.ToString, System.Type.GetType("System.Int64"))
        Next x

        'populate from datagridview
        For x = 0 To DataGridView1.RowCount - 1
            If DataGridView1.Rows(x).Cells("InRound").Value = False Then
                dr = dt.NewRow
                dr.Item("Competitor") = DataGridView1.Rows(x).Cells("Competitor").Value
                strDummy = DataGridView1.Rows(x).Cells("Sides").Value.ToString
                For y = 1 To strDummy.Length
                    If dr.Item("Side" & Mid(strDummy, y, 1)) Is System.DBNull.Value Then dr.Item("Side" & Mid(strDummy, y, 1)) = 0
                    dr.Item("Side" & Mid(strDummy, y, 1)) += 1
                Next y
                dt.Rows.Add(dr)
            End If
        Next x

        'sort and place
        For x = 1 To nTeams
            strSide = GetSideString(ds, x, drRd.Item("Event"))
            strColName = ""
            For y = 0 To dgvPairing.Columns.Count - 1
                If dgvPairing.Columns(y).HeaderText = strSide Then
                    strColName = dgvPairing.Columns(y).Name
                End If
            Next y
            'sort teams
            strSort = "Side" & x.ToString & " ASC"
            For y = x + 1 To nTeams
                strSort &= ", Side" & y.ToString & " DESC"
            Next y
            dt.DefaultView.Sort = strSort
            'now put in the pairings; scroll each row and put a team that fits in the blank side spot
            For y = 0 To dgvPairing.Rows.Count - 1
                If dgvPairing.Rows(y).Cells(strColName).Value.ToString = "" Or dgvPairing.Rows(y).Cells(strColName).Value.ToString = "N/A" Then
                    AddTeamToPanel(ds, dgvPairing.Rows(y).Cells("Panel").Value, dt.DefaultView(0).Item("Competitor"), x)
                    dgvPairing.Rows(y).Cells(strColName).Value = "Placed"
                    dt.DefaultView(0).Delete() : dt.AcceptChanges()
                End If
            Next y
        Next x

        'update the grid
        Call ResetActives()
        Call ClearCompetitorGrids()
        Call UpdateSides()
        Call LoadPairings()

        'validate pairings? fix display crash when you change the number of columns
    End Sub
    Sub UpdateSides()
        Dim dt As DataTable
        Dim dtv As New DataView
        dtv = DataGridView1.DataSource
        dt = dtv.ToTable()
        'dt = DataGridView1.DataSource
        Call AddSideReport(dt, ds)
        For x = 0 To dt.Rows.Count - 1
            For y = 0 To dtv.Count - 1
                If dt.Rows(x).Item("Competitor") = dtv(y).Item("Competitor") Then dtv(y).Item("Sides") = dt.Rows(x).Item("Sides")
            Next
        Next x
        'DataGridView1.DataSource = dt.DefaultView
    End Sub

    Private Sub butAutoPair_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butAutoPair.Click
        'find top bracket
        Dim TopBracket As Integer
        Dim drBracket As DataRow()
        drBracket = ds.Tables("Bracket").Select("Round=" & cboRound.SelectedValue)
        If drBracket Is Nothing Then
            MsgBox("The bracket does not appear to be set for this round yet.  Click the RESET BRACKET button and try again.")
            Exit Sub
        End If
        TopBracket = -99
        For x = 0 To drBracket.Length - 1
            If drBracket(x).Item("WinBracket") > TopBracket Then TopBracket = drBracket(x).Item("WinBracket")
        Next x
        'scroll through the brackets
        ExitAutoPair = False
        For x = TopBracket To 1 Step -1
            txtBracket.Text = x.ToString
            Call LoadBracket()
            DataGridView1.Refresh() : DataGridView2.Refresh()
            'even the bracket
            If chkSideConstrained.Checked = True Then
                Call EvenBracketSideConstrained(ExitAutoPair)
                If ExitAutoPair = True Then Exit Sub
            Else
                Call EvenBracketNoConstraints(ExitAutoPair)
                If ExitAutoPair = True Then Exit Sub
            End If
        Next x
        For x = TopBracket To 0 Step -1
            txtBracket.Text = x.ToString
            Call LoadBracket()
            DataGridView1.Refresh()
            DataGridView1.Sort(DataGridView1.Columns("SOP"), System.ComponentModel.ListSortDirection.Ascending)
            'now pair
            If DataGridView2.Visible = False Then
                Call OddPair("HL", ExitAutoPair)
            Else
                DataGridView2.Sort(DataGridView2.Columns("SOP"), System.ComponentModel.ListSortDirection.Ascending)
                Call EvenPair(DataGridView2.Rows.Count - 1, 0, -1, ExitAutoPair)
            End If
            If ExitAutoPair = True Then Exit For
        Next x
        'if one team is left, give them a bye
    End Sub
    Sub EvenBracketSideConstrained(ByRef ExitAutoPair As Boolean)
        'exit if even
        If DataGridView1.RowCount = DataGridView2.RowCount Then Exit Sub
        Dim nToGet = Math.Abs(DataGridView1.RowCount - DataGridView2.RowCount)
        Dim PullUpAff As Boolean = True
        If DataGridView1.RowCount < DataGridView2.RowCount Then PullUpAff = False
        'Go down a bracket
        txtBracket.Text = Val(txtBracket.Text) - 1
        Call LoadBracket()
        DataGridView1.Refresh() : DataGridView2.Refresh()
        'figure which bracket to even
        If PullUpAff = True Then
            If nToGet > DataGridView2.RowCount Then MsgBox("Requires a 2-bracket skew: Must complete manually") : ExitAutoPair = True : Exit Sub
            'sort it, select, and move up
            DataGridView2.Sort(DataGridView2.Columns("OppSeed"), System.ComponentModel.ListSortDirection.Descending)
            For x = 0 To nToGet - 1
                DataGridView2.Rows(0).Selected = True
                Call DoMove("UP")
            Next x
        Else
            If nToGet > DataGridView1.RowCount Then MsgBox("Requires a 2-bracket skew: Must complete manually") : ExitAutoPair = True : Exit Sub
            'sort it, select, and move up
            DataGridView1.Sort(DataGridView1.Columns("OppSeed"), System.ComponentModel.ListSortDirection.Descending)
            For x = 0 To nToGet - 1
                DataGridView1.Rows(0).Selected = True
                Call DoMove("UP")
            Next x
        End If
        'return to original bracket
        txtBracket.Text = Val(txtBracket.Text) + 1
    End Sub
    Sub EvenBracketNoConstraints(ByRef ExitAutoPair As Boolean)
        'exit if even
        If DataGridView1.RowCount / 2 = Int(DataGridView1.RowCount / 2) Then Exit Sub
        'not even, so go down to next bracket down and pull up
        txtBracket.Text = Val(txtBracket.Text) - 1
        Call LoadBracket()
        'sort it
        DataGridView1.Sort(DataGridView1.Columns("OppSeed"), System.ComponentModel.ListSortDirection.Descending)
        'exit if no teams to pull up
        If DataGridView1.RowCount = 0 Then MsgBox("Requires a 2-bracket skew: Must complete manually") : ExitAutoPair = True : Exit Sub
        'mark highest oppseeds
        DataGridView1.Rows(0).Selected = True
        'move 'em up
        Call DoMove("UP")
        'restore original bracket
        txtBracket.Text = Val(txtBracket.Text) + 1
    End Sub

    Private Sub butAutoInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butAutoInfo.Click
        Dim strInfo As String = ""
        strInfo &= "WHAT THIS WILL DO" & Chr(10) & Chr(10)
        strInfo &= "It will automatically even the brackets, unless there aren't enough teams to pull up or a 2-bracket skew is required." & Chr(10) & Chr(10)
        strInfo &= "It will then pair the round unless there is a 3-seed skew involved (that is, the teams that should debate must be changed more than 2 seed spots due to conflicts)." & Chr(10) & Chr(10)
        strInfo &= "In either event, you can manaully fix the problem and then click the button again." & Chr(10) & Chr(10)
        strInfo &= "If there are several issues, you are probably better off changing things a bracket at a time." & Chr(10) & Chr(10)
        strInfo &= "WHAT YOU CAN EXPECT" & Chr(10) & Chr(10)
        strInfo &= "If you have 25 teams or fewer, the constraints in the later rounds will be so tight auto pairing probably won't work.  You may be best off putting all teams in a single bracket and matching them up as best you can; remember that there are no more than 12 debates to pair, so side constraints and prior meetings will seriously reduce your options." & Chr(10) & Chr(10)
        strInfo &= "With 25-40 teams, the autopair is probably worth a try, although small brackets may create headaches you'll need to fix manually." & Chr(10) & Chr(10)
        strInfo &= "With 40 teams or more, it should generally work pretty well.  The larger the tournament, the more automatic pairing is likely to work seamlessly." & Chr(10) & Chr(10)
        strInfo &= "Final note: With 20 teams or fewer and 6 prelims, presetting the entire division is not a bad idea at all.  There is some chance you could do some power matching in the last round, but it's also possible to set things up make the final round a big mess." & Chr(10) & Chr(10)
        strInfo &= "Here's the math: 20 teams=10 debates, so 10 possible opponents given side constraints, minus prior meetings and school constraints, which means that virtually no team will have more than 3-5 possible opponents in the last round anyway.  Things get really tight at 16 teams." & Chr(10) & Chr(10)
        MsgBox(strInfo, , "Auto Pair Information")
    End Sub
End Class