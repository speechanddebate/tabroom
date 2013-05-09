Public Class frmElims
    Public ds As New DataSet
    Dim strDebateType As String

    Private Sub frmElims_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call LoadFile(ds, "TourneyData")

        cboDivisions.DataSource = ds.Tables("Event")
        cboDivisions.DisplayMember = "EventName"
        cboDivisions.ValueMember = "ID"

        'initialize the seeds for the division; will fill in seeds IF they don't exist.
        For x = 0 To ds.Tables("Event").Rows.Count - 1
            Call MakeElimSeeds(ds, ds.Tables("Event").Rows(x).Item("ID"))
        Next x

        Call RoundsToSeeds()
    End Sub
    Private Sub frmElims_Unload(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Disposed
        Call SaveFile(ds)
        ds.Dispose()
        If frmElimSeedPopup.Visible = True Then frmElimSeedPopup.Dispose()
    End Sub

    Private Sub butLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butLoad.Click
        Dim drEvent As DataRow : drEvent = ds.Tables("Event").Rows.Find(cboDivisions.SelectedValue)
        strDebateType = drEvent.Item("Type")
        Call LoadTheElim()
    End Sub
    Public Sub LoadTheElim()
        'Builds the grid based on panels, but it only works if the seeds are set
        DataGridView1.Columns.Clear()
        'pull all elim rounds for the division
        Dim fdElimRds As DataRow()
        fdElimRds = ds.Tables("Round").Select("Event=" & cboDivisions.SelectedValue & " and Rd_Name >9", "Rd_Name asc")
        'make table
        Dim dt As New DataTable
        DataGridView1.AutoGenerateColumns = False
        For x = 0 To fdElimRds.Length - 1
            dt.Columns.Add("Panel" & x + 1, System.Type.GetType("System.Int64"))
            dt.Columns.Add(fdElimRds(x).Item("Label"), System.Type.GetType("System.String"))
            Dim dgvc As New DataGridViewTextBoxColumn
            dgvc.DataPropertyName = "Panel" & x + 1.ToString
            dgvc.Visible = False
            DataGridView1.Columns.Add(dgvc)
            dgvc = New DataGridViewTextBoxColumn
            dgvc.DataPropertyName = fdElimRds(x).Item("Label")
            dgvc.HeaderText = fdElimRds(x).Item("Label")
            DataGridView1.Columns.Add(dgvc)
        Next x
        'don't really remember why I put this here, but looks like a double-catch to make sure that
        'columns aren't visible until activated
        If DataGridView1.Columns(0).Visible = True Then DataGridView1.Columns(0).Visible = False
        'populate the table
        Dim fdElimPanels, fdElimSeeds, fdElimBallots As DataRow()
        Dim dr As DataRow
        Dim ctr As Integer
        Dim TeamsPerDebate As Integer = getEventSetting(ds, cboDivisions.SelectedValue, "TeamsPerRound")
        For x = 0 To fdElimRds.Length - 1
            ctr = 0
            'pull the panels and seeds in the round
            fdElimPanels = ds.Tables("Panel").Select("Round=" & fdElimRds(x).Item("ID"))
            fdElimSeeds = ds.Tables("ElimSeed").Select("Round=" & fdElimRds(x).Item("ID") & " and event=" & cboDivisions.SelectedValue, "Seed ASC")
            For y = 0 To fdElimPanels.Length - 1
                'pull all the ballots in the round
                fdElimBallots = ds.Tables("Ballot").Select("Panel=" & fdElimPanels(y).Item("ID"))
                'sort ballots by seed order
                For z = 0 To (fdElimSeeds.Length / TeamsPerDebate) - 1
                    For w = 0 To fdElimBallots.Length - 1
                        If fdElimBallots(w).Item("Entry") = fdElimSeeds(z).Item("Entry") Then
                            If x = 0 Then
                                dr = dt.NewRow
                                dr.Item("Panel" & x + 1.ToString) = fdElimBallots(w).Item("Panel")
                                dr.Item(fdElimRds(x).Item("Label")) = MakeTeamString2(fdElimPanels(y).Item("Round"), fdElimSeeds(z).Item("seed"), fdElimBallots(w).Item("Panel"))
                                dt.Rows.Add(dr)
                                Exit For
                            Else
                                dt.Rows(ctr).Item("Panel" & x + 1.ToString) = fdElimBallots(w).Item("Panel")
                                dt.Rows(ctr).Item(fdElimRds(x).Item("Label")) = MakeTeamString2(fdElimPanels(y).Item("Round"), fdElimSeeds(z).Item("seed"), fdElimBallots(w).Item("Panel"))
                                ctr += 1
                                Exit For
                            End If
                        End If
                    Next w
                Next z
            Next y
        Next x
        DataGridView1.DataSource = dt
        DataGridView1.DefaultCellStyle.Font = New Font("Arial", 9, FontStyle.Bold, GraphicsUnit.Point)
        If dt.Rows.Count > 20 Then DataGridView1.DefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Bold, GraphicsUnit.Point)
        If strDebateType = "WUDC" Then
            DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            DataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True
        End If
    End Sub
    Function MakeTeamString(ByVal round As Integer, ByVal seed As Integer, ByVal panel As Integer) As String
        Dim team1, team2 As Integer
        'pull all the ballots
        Dim fdballot As DataRow()
        fdballot = ds.Tables("Ballot").Select("Panel=" & panel)
        'calculate second seed
        Dim seed2 As Integer
        Dim fdDebates As DataRow()
        fdDebates = ds.Tables("Panel").Select("Round=" & round)
        seed2 = ((fdDebates.Length * 2) - seed) + 1
        'find the teams
        For x = 0 To ds.Tables("ElimSeed").Rows.Count - 1
            If ds.Tables("ElimSeed").Rows(x).Item("Seed") = seed And ds.Tables("ElimSeed").Rows(x).Item("Round") = round Then
                If fdballot(0).Item("Entry") = ds.Tables("ElimSeed").Rows(x).Item("Entry") Then
                    team1 = fdballot(0).Item("Entry")
                    team2 = fdballot(1).Item("Entry")
                End If
                If fdballot(1).Item("Entry") = ds.Tables("ElimSeed").Rows(x).Item("Entry") Then
                    team1 = fdballot(1).Item("Entry")
                    team2 = fdballot(0).Item("Entry")
                End If
            End If
        Next
        'build the string
        Dim dr As DataRow
        MakeTeamString = "(" & seed.ToString & ") "
        dr = ds.Tables("Entry").Rows.Find(team1)
        If Not dr Is Nothing Then
            MakeTeamString &= dr.Item("Code").trim
            MakeTeamString &= " (" & seed2.ToString & ") "
        End If
        dr = ds.Tables("Entry").Rows.Find(team2)
        If Not dr Is Nothing Then
            MakeTeamString &= dr.Item("Code").trim
        Else
            MakeTeamString &= "Bye"
        End If
    End Function
    Function MakeTeamString2(ByVal round As Integer, ByVal seed As Integer, ByVal panel As Integer) As String
        'pull all the ballots
        Dim fdballot As DataRow()
        fdballot = ds.Tables("Ballot").Select("Panel=" & panel)
        'declare the table
        Dim dt As New DataTable
        dt.Columns.Add("Team", System.Type.GetType("System.String"))
        dt.Columns.Add("Seed", System.Type.GetType("System.Int64"))
        Dim dr As DataRow
        'find the teams
        For x = 0 To ds.Tables("ElimSeed").Rows.Count - 1
            For y = 0 To fdballot.Length - 1
                If ds.Tables("ElimSeed").Rows(x).Item("Entry") = fdballot(y).Item("Entry") And ds.Tables("ElimSeed").Rows(x).Item("Round") = round Then
                    dr = dt.NewRow
                    dr.Item("Team") = fdballot(y).Item("Entry")
                    dr.Item("Seed") = ds.Tables("ElimSeed").Rows(x).Item("Seed")
                    dt.Rows.Add(dr)
                End If
            Next y
        Next x
        'build the string
        dt.DefaultView.Sort = "SEED ASC"
        MakeTeamString2 = ""
        For x = 0 To dt.Rows.Count - 1
            dr = ds.Tables("Entry").Rows.Find(dt.Rows(x).Item("Team"))
            If Not dr Is Nothing Then
                If InStr(MakeTeamString2, dr.Item("Code")) = 0 Then
                    MakeTeamString2 &= "(" & dt.Rows(x).Item("Seed") & ") " & dr.Item("Code") & " "
                End If
            Else
                If InStr(MakeTeamString2, "Bye") = 0 Then MakeTeamString2 &= "(Bye) "
            End If
        Next x
    End Function
    Sub CellClick() Handles DataGridView1.CellClick

        Dim panel As Integer
        Try
            panel = DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells(DataGridView1.CurrentCell.ColumnIndex - 1).Value
        Catch
            Exit Sub
        End Try

        Dim TeamsPerRd As Integer = getEventSetting(ds, cboDivisions.SelectedValue, "TeamsPerRound")
        
        Dim dt As New DataTable
        dt.Columns.Add("JudgeID", System.Type.GetType("System.Int64"))
        dt.Columns.Add("Judge", System.Type.GetType("System.String"))
        For x = 0 To TeamsPerRd - 1
            dt.Columns.Add("TeamID" & x + 1, System.Type.GetType("System.Int64"))
            dt.Columns.Add("Team" & x + 1, System.Type.GetType("System.Boolean"))
        Next x

        'pull all the ballots for the panel, sort by judge
        DataGridView2.DataSource = Nothing : lblSides.Text = "Judge assignment incomplete."
        Dim fdBallots As DataRow()
        fdBallots = ds.Tables("Ballot").Select("Panel=" & panel, "Judge asc, Entry asc")
        'check for bye, exit if yes
        For x = 0 To fdBallots.Length - 1
            If fdBallots(0).Item("Entry") = -99 Then Call AdvanceBye(panel) : Exit Sub
        Next x
        'no bye, so pull judges
        If fdBallots(0).Item("Judge") > -99 Then
            Dim dr As DataRow : Dim ctr As Integer = 1
            For x = 0 To fdBallots.Length - 1
                If x < fdBallots.Length - 1 Then
                    If fdBallots(x).Item("Judge") <> fdBallots(x + 1).Item("Judge") Then
                        dr = dt.NewRow
                        dr.Item("JudgeID") = fdBallots(x).Item("Judge")
                        dr.Item("Judge") = GetName(ds.Tables("Judge"), fdBallots(x).Item("Judge"), "Last") & ", " & GetName(ds.Tables("Judge"), fdBallots(x).Item("Judge"), "First")
                        Call ballotIn(dr, fdBallots(x).Item("Judge"), panel)
                        dt.Rows.Add(dr)
                    End If
                Else
                    dr = dt.NewRow
                    dr.Item("JudgeID") = fdBallots(x).Item("Judge")
                    dr.Item("Judge") = GetName(ds.Tables("Judge"), fdBallots(x).Item("Judge"), "Last") & ", " & GetName(ds.Tables("Judge"), fdBallots(x).Item("Judge"), "First")
                    Call ballotIn(dr, fdBallots(x).Item("Judge"), panel)
                    dt.Rows.Add(dr)
                End If
            Next x
            DataGridView2.DataSource = dt

            'suppress later rows if WUDC panel decision
            If getEventSetting(ds, cboDivisions.SelectedValue, "PanelDecisions") = True Then
                For x = dt.Rows.Count - 1 To 1 Step -1
                    dt.Rows(x).Delete()
                Next x
            End If

            'suppress ID columns
            For x = 0 To DataGridView2.ColumnCount - 1
                If DataGridView2.Columns(x).Name = "JudgeID" Then DataGridView2.Columns(x).Visible = False
                If Mid(DataGridView2.Columns(x).Name, 1, 6) = "TeamID" Then
                    DataGridView2.Columns(x).Visible = False
                    DataGridView2.Columns(x + 1).HeaderText = FullNameMaker(ds, DataGridView2.Rows(0).Cells(x).Value, "CODE", 1)
                End If
            Next x
            Call UpdateSides(fdBallots)
        End If

        Call UpdateDecision(panel)
        Call CurrentWinners()

        Dim drPanel As DataRow : drPanel = ds.Tables("Panel").Rows.Find(panel)
        Dim drRd As DataRow : drRd = ds.Tables("Round").Rows.Find(drPanel.Item("Round"))
        Dim drRoom As DataRow : drRoom = ds.Tables("Room").Rows.Find(drPanel.Item("Room"))
        If Not drRoom Is Nothing Then lblRoom.Text = drRoom.Item("RoomName")
        butDelete.Text = "Delete all " & drRd.Item("Label") & " debates"

    End Sub
    Sub AdvanceBye(ByVal panel As Integer)
        'pull all the ballots for the panel, find the first non-99 entry
        Dim fdTeam As DataRow()
        fdTeam = ds.Tables("Ballot").Select("Panel=" & panel & " and Entry>-99")
        'update the ballot scores
        Dim fdBallots, fdScores As DataRow()
        fdBallots = ds.Tables("Ballot").Select("Panel=" & panel)
        For x = 0 To fdBallots.Length - 1
            ValidateScoresByBallot(ds, fdBallots(x).Item("ID"))
            fdScores = ds.Tables("Ballot_Score").Select("Ballot=" & fdBallots(x).Item("ID") & " and Score_ID=1")
            For y = 0 To fdScores.Length - 1
                fdScores(y).Item("Score") = 0
                If fdScores(y).Item("Recipient") = fdTeam(0).Item("Entry") Then fdScores(y).Item("Score") = 1
            Next y
        Next x
        Call UpdateDecision(panel)
        Call CurrentWinners()
    End Sub
    Sub UpdateDecision(ByVal Panel As Integer)

        Dim drElimSeed As DataRow()
        'find out who won
        Dim dummy As String
        Dim winner As Integer = GetWinner(ds, Panel, cboDivisions.SelectedValue, dummy)
        If winner = -1 Then
            lblwinner.Text = "No winner presently declared"
            Exit Sub
        End If

        'update the elimseed table for the next round
        Dim drPanel As DataRow : drPanel = ds.Tables("panel").Rows.Find(Panel)
        Dim drRound As DataRow : drRound = ds.Tables("Round").Rows.Find(drPanel.Item("Round"))
        Dim NextRd As Integer = drRound.Item("Rd_Name") + 1
        Dim drNextRdID As DataRow() : drNextRdID = ds.Tables("Round").Select("Rd_Name=" & NextRd & " and Event=" & cboDivisions.SelectedValue)
        Dim SeedSpot As Integer = GetSeedForPanel(Panel)
        If NextRd < 17 Then
            drElimSeed = ds.Tables("ElimSeed").Select("Seed=" & SeedSpot & " and Round=" & drNextRdID(0).Item("ID"))
            If winner = -1 Then drElimSeed(0).Item("Entry") = 0
            If winner <> -1 And drElimSeed.Length > 0 Then drElimSeed(0).Item("Entry") = winner
        End If

        'Update the label
        Dim drEntry As DataRow
        drEntry = ds.Tables("Entry").Rows.Find(winner)
        lblwinner.Text = drEntry.Item("Code").trim & " advances as seed #" & SeedSpot
    End Sub
    Function GetSeedForPanel(ByVal Panel As Integer) As Integer
        Dim drPanel As DataRow
        drPanel = ds.Tables("Panel").Rows.Find(Panel)
        Dim fdBallots As DataRow()
        fdBallots = ds.Tables("Ballot").Select("Panel=" & Panel)
        GetSeedForPanel = 999
        Dim drSeed As DataRow()
        For x = 0 To fdBallots.Length - 1
            drSeed = ds.Tables("ElimSeed").Select("Entry=" & fdBallots(x).Item("Entry") & " and round=" & drPanel.Item("Round"))
            If drSeed(0).Item("Seed") < GetSeedForPanel Then GetSeedForPanel = drSeed(0).Item("Seed")
        Next x

    End Function
    Sub UpdateSides(ByVal fdBallots As DataRow())
        'update sides
        lblSides.Text = ""
        Dim drTeam As DataRow
        For x = 0 To fdBallots.Length - 1
            If x > 0 Then If fdBallots(x).Item("Judge") <> fdBallots(x - 1).Item("Judge") Then Exit For
            drTeam = ds.Tables("Entry").Rows.Find(fdBallots(x).Item("Entry"))
            If Not drTeam Is Nothing Then
                lblSides.Text &= drTeam.Item("Code").trim & " is " & GetSideString(ds, fdBallots(x).Item("Side"), cboDivisions.SelectedValue) & Chr(10)
            End If
        Next x
    End Sub
    Sub ballotIn(ByRef dr As DataRow, ByVal judge As Integer, ByVal panel As Integer)
        'pull all ballots by judge
        Dim fdBallots, fdScore As DataRow()
        Dim strScoreID As String = "1"
        If getEventSetting(ds, cboDivisions.SelectedValue, "PanelDecisions") = True Then strScoreID = "5"
        fdBallots = ds.Tables("Ballot").Select("Panel=" & panel & " and judge=" & judge, "Entry ASC")
        For x = 0 To fdBallots.Length - 1
            'pull the ballot_score and store it in the team column
            dr.Item("TeamID" & x + 1) = fdBallots(x).Item("Entry")
            dr.Item("Team" & x + 1) = False
            fdScore = ds.Tables("Ballot_Score").Select("Ballot=" & fdBallots(x).Item("ID") & " and Recipient=" & fdBallots(x).Item("Entry") & " and Score_ID=" & strScoreID)
            If fdScore.Length > 0 Then
                If fdScore(0).Item("Score") = 1 Then dr.Item("Team" & x + 1) = True
            End If
        Next x
    End Sub
    Private Sub butFillElim_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butFillElim.Click
        'confirm delete
        Dim q As Integer = MsgBox("This will delete all elims for the division and re-seed the first elim round.  Continue?", MsgBoxStyle.YesNo)
        If q = vbNo Then Exit Sub
        'check no flighting change
        If FlightingMatch() = False Then
            q = MsgBox("You are flighting the elims at a higher rate than the prelims, for example, you may have single-flighted the prelims but double-flighted (traditional LD style) the elims.  Note that the panel size is NOT the same as flighting.  A full description is on the round setup page.  Click YES to contine (NOT recommended) or NO to continue with flighted elims (recommended).", MsgBoxStyle.YesNo)
            If q = vbNo Then Exit Sub
        End If
        'check panel size
        Dim foundElims As DataRow()
        foundElims = ds.Tables("Round").Select("Event=" & cboDivisions.SelectedValue & " and Rd_Name>9", "RD_NAME asc")
        If foundElims(0).Item("JudgesPerPanel") = 1 Then
            q = MsgBox("This round is currently set for 1-judge panels; if you wish multiple-judge panels, close this screen and return to the round setup screen to indicated the number of judges per panel.  Click YES to exit this process, or NO to continue with 1-judge panels.", MsgBoxStyle.YesNo)
            If q = vbYes Then Exit Sub
        End If
        Dim strClear
        strClear = InputBox("Select a value to continue: (a) clear full bracket (b) clear only teams with a winning record (c) clear only teams with at least a .500 record, or (d) enter a number of teams to clear.  Enter a value a-c or number to clear:", "How to clear elims", "a")
        'If q < 1 Or q > 3 Then MsgBox("Please try again and select a valid value 1-3") : Exit Sub
        'clear all seeds for division & delete all panels for elim rounds
        Dim drRound, drPanel, drElimSeed As DataRow()
        drRound = ds.Tables("Round").Select("Event=" & cboDivisions.SelectedValue & " and Rd_Name>=9")
        'delete all elim panels for this division
        For x = 0 To drRound.Length - 1
            drPanel = ds.Tables("Panel").Select("Round=" & drRound(x).Item("ID"))
            For y = drPanel.Length - 1 To 0 Step -1 : drPanel(y).Delete() : Next y
        Next x
        'delete all elim seeds for this division
        drRound = ds.Tables("Round").Select("Event=" & cboDivisions.SelectedValue)
        For x = 0 To drRound.Length - 1
            drElimSeed = ds.Tables("ElimSeed").Select("Round=" & drRound(x).Item("ID"))
            For y = drElimSeed.Length - 1 To 0 Step -1 : drElimSeed(y).Delete() : Next y
        Next
        'check that the seeds have been initialized for the division, and create them if they're not
        Call MakeElimSeeds(ds, cboDivisions.SelectedValue)

        'check to see if teams have already been assigned to seed spots; ask user to reset them if they are
        'Dim SeedsAlready As Boolean = False
        'For x = 0 To ds.Tables("ElimSeed").Rows.Count - 1
        ' If ds.Tables("ElimSeed").Rows(x).Item("Entry") > 0 Then SeedsAlready = True
        'Next x
        'If SeedsAlready = True Then
        'Dim q As Integer = MsgBox("This division already has teams assigned to elims.  YES to reset, and NO to exit and load existing seeds:", MsgBoxStyle.YesNo)
        'If q = vbNo Then Exit Sub
        'For x = 0 To ds.Tables("ElimSeed").Rows.Count - 1
        'If ds.Tables("ElimSeed").Rows(x).Item("Event") = cboDivisions.SelectedValue Then ds.Tables("ElimSeed").Rows(x).Item("Entry") = 0
        'Next x
        'End If

        'OK, so run the functions
        q = 0
        If strClear.toupper = "A" Then q = 1
        If strClear.toupper = "B" Then q = 2
        If strClear.toupper = "C" Then q = 3
        If q = 0 Then q = Val(strClear)
        Call FillFirstElim(ds, cboDivisions.SelectedValue, q)
        Call LoadTheElim()
    End Sub
    Sub RoundsToSeeds()
        'checks whether there are rounds but not seeds, and if so fixes it
        Dim fdRds, fdPanels, fdBallot, fdSeed, fdFirstElimRd As DataRow()

        'If it's the first round for an elim and there isn't a seed, run the FillFirstElim rountine
        Dim FirstElim As Integer : Dim NeedToFire As Boolean
        For x = 0 To ds.Tables("Event").Rows.Count - 1
            NeedToFire = False
            FirstElim = ds.Tables("Round").Compute("Min(Rd_Name)", "Rd_Name>9 and event=" & ds.Tables("Event").Rows(x).Item("ID"))
            fdFirstElimRd = ds.Tables("Round").Select("Rd_Name=" & FirstElim)
            fdPanels = ds.Tables("Panel").Select("Round=" & fdFirstElimRd(0).Item("ID"))
            For y = 0 To fdPanels.Length - 1
                fdBallot = ds.Tables("Ballot").Select("Panel=" & fdPanels(y).Item("ID"))
                For z = 0 To fdBallot.Length - 1
                    fdSeed = ds.Tables("ElimSeed").Select("Round=" & FirstElim & " and entry=" & fdBallot(z).Item("Entry"))
                    If fdSeed.Length = 0 Then NeedToFire = True : Exit For
                Next
                If NeedToFire = True Then Exit For
            Next y
            'If NeedToFire = True Then Call FillFirstElim(ds, ds.Tables("Event").Rows(x).Item("ID"))
        Next x

        'Now scroll through the rest of the rounds and plug in the seed for any competing team that doesn't have a seed
        Dim dr As DataRow()
        fdRds = ds.Tables("Round").Select("Rd_Name>9")
        For x = 0 To fdRds.Length - 1
            fdPanels = ds.Tables("Panel").Select("Round=" & fdRds(x).Item("ID"))
            For y = 0 To fdPanels.Length - 1
                fdBallot = ds.Tables("Ballot").Select("Panel=" & fdPanels(y).Item("ID"))
                For z = 0 To fdBallot.Length - 1
                    fdSeed = ds.Tables("ElimSeed").Select("Entry=" & fdBallot(z).Item("Entry") & " and round=" & fdRds(x).Item("ID"))
                    If fdSeed.Length = 0 Then
                        dr = ds.Tables("ElimSeed").Select("Round=" & fdRds(x).Item("ID") & " and seed=" & GetSeed(fdBallot(z).Item("Entry"), fdRds(x).Item("ID")))
                        If dr.Length = 1 Then dr(0).Item("Entry") = fdBallot(z).Item("Entry")
                    End If
                Next z
            Next y
        Next x
    End Sub
    Function GetSeed(ByVal EntryID As Integer, ByVal Round As Integer) As Integer
        'will find the lowest (best) seed spot a given team has competed in during any prior elim
        GetSeed = 999
        Dim fdseeds, fdBallots, fdBallots2 As DataRow()
        'pull all ballots for the team
        fdBallots = ds.Tables("Ballot").Select("Entry=" & EntryID)
        Dim drRd, drPanel As DataRow
        For x = 0 To fdBallots.Length - 1
            'pull the panel and round info for each ballot
            drPanel = ds.Tables("Panel").Rows.Find(fdBallots(x).Item("Panel"))
            drRd = ds.Tables("Round").Rows.Find(drPanel.Item("Round"))
            '...if it's an elim prior to the current elim....
            If drRd.Item("Rd_Name") > 9 And drRd.Item("Rd_Name") < Round Then
                'pull all ballots on the panel, and retrieve the lowest seed
                fdBallots2 = ds.Tables("Ballot").Select("Panel=" & drPanel.Item("ID"))
                For y = 0 To fdBallots2.Length - 1
                    fdseeds = ds.Tables("ElimSeed").Select("Entry=" & fdBallots2(y).Item("Entry") & " and round=" & drRd.Item("ID"))
                    If fdseeds.Length > 1 Then
                        If fdseeds(0).Item("Seed") < GetSeed Then GetSeed = fdseeds(0).Item("Seed")
                    End If
                Next y
            End If
        Next x
    End Function

    Private Sub butFlipSides_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butFlipSides.Click

        Dim panel As Integer
        Try
            panel = DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells(DataGridView1.CurrentCell.ColumnIndex - 1).Value
        Catch
            Exit Sub
        End Try

        'pull all the ballots for the panel, sort by judge
        Dim fdBallots As DataRow()
        fdBallots = ds.Tables("Ballot").Select("Panel=" & panel, "Judge asc, Entry asc")

        For x = 0 To fdBallots.Length - 1
            If fdBallots(x).Item("Side") = 1 Then
                fdBallots(x).Item("Side") = 2
            ElseIf fdBallots(x).Item("Side") = 2 Then
                fdBallots(x).Item("Side") = 1
            End If
        Next x

        Call UpdateSides(fdBallots)

    End Sub
    Sub CurrentWinners()
        'pull the panel, find the round
        Dim panel As Integer
        Try
            panel = DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells(DataGridView1.CurrentCell.ColumnIndex - 1).Value
        Catch
            Exit Sub
        End Try

        Dim drRound, drPanel As DataRow : Dim drnextrd As DataRow()
        drPanel = ds.Tables("Panel").Rows.Find(panel)
        drRound = ds.Tables("Round").Rows.Find(drPanel.Item("Round"))
        Dim NextRd As Integer = drRound.Item("Rd_Name") + 1
        If NextRd <= 16 Then
            drnextrd = ds.Tables("Round").Select("Rd_Name=" & NextRd & " and event=" & drRound.Item("Event"))
            lblAdvancing.Text = "Teams currently advancing to " & drnextrd(0).Item("Label")
            butPair.Text = "Pair " & drnextrd(0).Item("Label")
        End If

        Dim dt As New DataTable
        dt.Columns.Add("Seed", System.Type.GetType("System.Int64"))
        dt.Columns.Add("Team", System.Type.GetType("System.String"))

        Dim fdSeeds As DataRow()
        drnextrd = ds.Tables("Round").Select("Rd_Name=" & NextRd & "and Event=" & cboDivisions.SelectedValue)
        If NextRd > 16 Then Exit Sub

        fdSeeds = ds.Tables("ElimSeed").Select("Round=" & drnextrd(0).Item("ID"))

        Dim dr, drTeam As DataRow
        For x = 0 To fdSeeds.Length - 1
            dr = dt.NewRow
            dr.Item("Seed") = fdSeeds(x).Item("Seed")
            drTeam = ds.Tables("Entry").Rows.Find(fdSeeds(x).Item("Entry"))
            If Not drTeam Is Nothing Then
                dr.Item("Team") = drTeam.Item("FullName")
            Else
                dr.Item("Team") = "Not set yet"
            End If
            dt.Rows.Add(dr)
        Next x

        DataGridView3.DataSource = dt

    End Sub
    Sub BallotChange() Handles DataGridView2.MouseClick
        If getEventSetting(ds, cboDivisions.SelectedValue, "PanelDecisions") = True Then Call WUDCBallotUpdate() : Exit Sub
        Dim panel As Integer
        Try
            panel = DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells(DataGridView1.CurrentCell.ColumnIndex - 1).Value
        Catch
            Exit Sub
        End Try
        'changes not commited yet, so unchecked menas it WILL be checked
        If DataGridView2.CurrentCell.Value = False Then
            Call ChangeOtherColumns(False)
        ElseIf DataGridView2.CurrentCell.Value = True Then
            Call ChangeOtherColumns(True)
        End If
        'Update the ballot scores
        Dim Judge, Team As Integer
        Dim drBallot, drBallotScore As DataRow()
        For x = 0 To DataGridView2.ColumnCount - 1
            If Mid(DataGridView2.Columns(x).Name, 1, 7) = "JudgeID" Then Judge = DataGridView2.CurrentRow.Cells(x).Value
            If Mid(DataGridView2.Columns(x).Name, 1, 6) = "TeamID" Then
                Team = DataGridView2.CurrentRow.Cells(x).Value
                drBallot = ds.Tables("Ballot").Select("Judge=" & Judge & " and entry=" & Team & " and panel=" & panel)
                Call ValidateScoresByBallot(ds, drBallot(0).Item("ID"))
                drBallotScore = ds.Tables("Ballot_Score").Select("Ballot=" & drBallot(0).Item("ID") & " and Score_ID=1")
                'if its the current column
                If (x + 1) = DataGridView2.CurrentCell.ColumnIndex Then
                    drBallotScore(0).Item("Score") = 0
                    'if not checked it will be, so you're the winner
                    If DataGridView2.CurrentCell.Value = False Then drBallotScore(0).Item("Score") = 1
                Else
                    drBallotScore(0).Item("Score") = 0
                    'if is checked it won't be, so non-checked team is the winner
                    If DataGridView2.CurrentCell.Value = True Then drBallotScore(0).Item("Score") = 1
                End If
            End If
        Next x
        'Update the decision
        Call UpdateDecision(panel)
        Call CurrentWinners()
    End Sub
    Sub WUDCBallotUpdate()

        'go through the columns, advance the team if checked
        Dim panel As Integer
        Try
            panel = DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells(DataGridView1.CurrentCell.ColumnIndex - 1).Value
        Catch
            Exit Sub
        End Try
        Dim drRd, drPanel As DataRow
        drPanel = ds.Tables("Panel").Rows.Find(panel)
        drRd = ds.Tables("Round").Rows.Find(drPanel.Item("Round"))

        'changes not commited yet, so update the value of the current cell
        If DataGridView2.CurrentCell.Value = False Then
            DataGridView2.CurrentCell.Value = True
        ElseIf DataGridView2.CurrentCell.Value = True Then
            DataGridView2.CurrentCell.Value = False
        End If

        'get the 2 lowest seeds so far
        Dim strDummy As String : Dim seed1, seed2 As Integer
        strDummy = DataGridView1.CurrentCell.Value.ToString
        For x = 1 To Len(strDummy)
            If seed1 > 0 And seed2 > 0 Then Exit For
            If Mid(strDummy, x, 1) = "(" Then
                If seed1 = 0 Then
                    seed1 = Val(Mid(strDummy, x + 1, 1))
                ElseIf seed2 = 0 Then
                    seed2 = Val(Mid(strDummy, x + 1, 1))
                End If
            End If
        Next x

        Dim ctr As Integer
        For x = 0 To DataGridView2.ColumnCount - 1
            If Mid(DataGridView2.Columns(x).Name, 1, 4) = "Team" And Mid(DataGridView2.Columns(x).Name, 1, 6) <> "TeamID" Then
                If DataGridView2.CurrentRow.Cells(x).Value = True Then
                    ctr += 1
                End If
            End If
        Next x
        If ctr > 2 Then
            MsgBox("You have already advanced 2 teams.  Please try again.")
            DataGridView2.CurrentCell.Value = False
            Exit Sub
        End If

        'Update the ballot scores
        Dim Judge, Team, Win, NextRd, NewTeam1, NewTeam2 As Integer
        Dim drBallot, drBallotScore, drESeed, drNextERd As DataRow() : Dim drTeam As DataRow
        lblwinner.Text = ""
        For x = 0 To DataGridView2.ColumnCount - 1
            If Mid(DataGridView2.Columns(x).Name, 1, 7) = "JudgeID" Then Judge = DataGridView2.CurrentRow.Cells(x).Value
            If Mid(DataGridView2.Columns(x).Name, 1, 6) = "TeamID" Then
                Team = DataGridView2.CurrentRow.Cells(x).Value
                Win = 0 : If DataGridView2.Rows(0).Cells(x + 1).Value = True Then Win = 1
                'update the ballot
                drBallot = ds.Tables("Ballot").Select("Judge=" & Judge & " and entry=" & Team & " and panel=" & panel)
                Call ValidateScoresByBallot(ds, drBallot(0).Item("ID"))
                drBallotScore = ds.Tables("Ballot_Score").Select("Ballot=" & drBallot(0).Item("ID") & " and Score_ID=5")
                drBallotScore(0).Item("Score") = Win
                'update the decision
                If Win = 1 Then
                    drTeam = ds.Tables("Entry").Rows.Find(Team)
                    lblwinner.Text &= drTeam.Item("Code") & " advances. "
                    'find the current seed
                    If newTeam1 = 0 Then newteam1 = Team Else newteam2 = Team
                End If
            End If
        Next x

        'update the elimseed table
        NextRd = drRd.Item("Rd_Name") + 1
        drNextERd = ds.Tables("Round").Select("Event=" & cboDivisions.SelectedValue & " and rd_Name=" & NextRd)
        If NewTeam1 > 0 Then
            drESeed = ds.Tables("ElimSeed").Select("Round=" & drNextERd(0).Item("ID") & " and seed=" & seed1)
            drESeed(0).Item("Entry") = NewTeam1
        End If
        If NewTeam2 > 0 Then
            drESeed = ds.Tables("ElimSeed").Select("Round=" & drNextERd(0).Item("ID") & " and seed=" & seed2)
            drESeed(0).Item("Entry") = NewTeam2
        End If
        Call CurrentWinners()

    End Sub
    Sub ChangeOtherColumns(ByVal newVal As Boolean)
        For x = 0 To DataGridView2.ColumnCount - 1
            If Mid(DataGridView2.Columns(x).Name, 1, 4) = "Team" And Mid(DataGridView2.Columns(x).Name, 1, 6) <> "TeamID" Then
                If x <> DataGridView2.CurrentCell.ColumnIndex Then
                    DataGridView2.CurrentRow.Cells(x).Value = newVal
                End If
            End If
        Next x
    End Sub

    Private Sub butDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butDelete.Click
        'find the round
        Dim panel As Integer
        Try
            panel = DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells(DataGridView1.CurrentCell.ColumnIndex - 1).Value
        Catch
            MsgBox("Click on a debate to indicate the round to delete, and try again.")
            Exit Sub
        End Try
        Dim drPanel As DataRow : drPanel = ds.Tables("Panel").Rows.Find(panel)
        Dim drRd As DataRow : drRd = ds.Tables("Round").Rows.Find(drPanel.Item("Round"))
        'delete it
        Call DeleteAllPanelsForRound(ds, drRd.Item("ID"))
        'reset the grid
        Call LoadTheElim()
    End Sub

    Private Sub butHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butHelp.Click
        Dim strHelp As String = ""
        strHelp &= "Begin by selecting a division/event and clicking the LOAD button.  If no elims have yet been paired, click on the FILL FIRST ELIM button and the first elim round will be paired.  " & Chr(10) & Chr(10)
        strHelp &= "Clicking any debate in a column will select the round for that column as active, for example, clicking on a quarter-final debate will make quarter-finals the active round.  Clicking an empty box will do nothing.  " & Chr(10) & Chr(10)
        strHelp &= "To enter the results for a debate, click on the debate and enter the decision in the ballot box on the top-right.  As soon as a winner is declared, the outcome will display in the winner box and the advancing team will appear in the seeds grid in the bottom-right.  " & Chr(10) & Chr(10)
        strHelp &= "Once all the decisions are in and the advancing seeds have all been set, you can pair the coming round by clicking on the PAIR button (it will indicate the round to pair) above the teams advancing box.  Note that you will need to place judges on the 'View a pairing for manual changes' screen, which has automatic judge and room placement options.  " & Chr(10) & Chr(10)
        strHelp &= "To change the bracket, select an event, load it, reset and fill the first elim, and then click on the CHANGE BRACKET button in the middle-right." & Chr(10) & Chr(10)
        MsgBox(strHelp, MsgBoxStyle.OkOnly)
    End Sub

    Private Sub butPair_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butPair.Click
        'find the round
        Dim panel As Integer
        Try
            panel = DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells(DataGridView1.CurrentCell.ColumnIndex - 1).Value
        Catch
            Exit Sub
        End Try
        Dim drRound, drPanel As DataRow
        drPanel = ds.Tables("Panel").Rows.Find(panel)
        drRound = ds.Tables("Round").Rows.Find(drPanel.Item("Round"))
        Dim q As Integer
        'check panel size
        If drRound.Item("JudgesPerPanel") = 1 Then
            q = MsgBox("This round is currently set for 1-judge panels; if you wish multiple-judge panels, close this screen and return to the round setup screen to indicated the number of judges per panel.  Click YES to exit this process, or NO to continue with 1-judge panels.", MsgBoxStyle.YesNo)
            If q = vbYes Then Exit Sub
        End If
        'check all ballots in
        Dim fdPanels, fdBal, fdScore As DataRow() : Dim ctr As Integer : Dim notIn As Boolean = False
        fdPanels = ds.Tables("Panel").Select("Round=" & drRound.Item("ID"))
        For x = 0 To fdPanels.Length - 1
            fdBal = ds.Tables("Ballot").Select("Panel=" & fdPanels(x).Item("ID"))
            ctr = 0
            For y = 0 To fdBal.Length - 1
                fdScore = ds.Tables("Ballot_Score").Select("Ballot=" & fdBal(y).Item("ID") & " and Score_ID=1")
                If fdScore.Length > 0 Then
                    If fdScore(0).Item("Score") = 1 Then ctr += 1
                End If
            Next y
            If ctr < (fdBal.Length / 2) Then
                notIn = True
            End If
        Next x
        If notIn = True And strDebateType <> "WUDC" Then
            q = MsgBox("Not all debates have all of the ballots turned in yet.  Exit?", MsgBoxStyle.YesNo)
            If q = vbYes Then Exit Sub
        End If
        'now pair
        Dim NextRd As Integer = drRound.Item("Rd_Name") + 1
        Dim NextRdID As DataRow()
        NextRdID = ds.Tables("Round").Select("Rd_Name=" & NextRd & " and Event=" & cboDivisions.SelectedValue)
        'check all the seeds are there
        Dim fdElimSeed As DataRow()
        fdElimSeed = ds.Tables("ElimSeed").Select("Round=" & NextRdID(0).Item("ID"))
        If fdElimSeed.Length = 0 Then MsgBox("The seed spots appear to be empty.  Please fill them and try again.") : Exit Sub
        Dim EmptySlots As Boolean = False
        For x = 0 To fdElimSeed.Length - 1
            If fdElimSeed(x).Item("Entry") = 0 Then emptyslots = True
        Next x
        If EmptySlots = True Then
            q = MsgBox("One ore more slots are blank; their opponents will receive byes.  Continue?", MsgBoxStyle.YesNo)
            If q = vbNo Then Exit Sub
        End If
        'Pair it
        Call PairElim(ds, cboDivisions.SelectedValue, NextRd)
        'refresh screen
        Call LoadTheElim()
    End Sub

    Private Sub butAdvanceWODebate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butAdvanceWODebate.Click
        Dim panel As Integer
        Try
            panel = DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells(DataGridView1.CurrentCell.ColumnIndex - 1).Value
        Catch
            MsgBox("Please click on one and only one debate and try again.")
            Exit Sub
        End Try

        'pull all the ballots for the panel, sort by judge
        Dim nTeams As Integer = getEventSetting(ds, cboDivisions.SelectedValue, "TeamsPerRound")
        Dim fdTeams(nTeams) As Integer : Dim drTeam As DataRow
        Call UniqueItemsOnPanel(ds, panel, "Entry", nTeams, fdTeams)
        Dim dummy As String = "This will advance a team without debating. "
        For x = 1 To nTeams
            drTeam = ds.Tables("Entry").Rows.Find(fdTeams(x))
            dummy &= "Enter a " & x & " to advance " & drTeam.Item("FullName") & ". "
        Next x
        dummy &= "Or, enter a 0 to exit."
        Dim AdvanceWho As Integer = InputBox(dummy, "Advance Without Debating")
        If AdvanceWho = 0 Then Exit Sub
        If AdvanceWho > nTeams Then
            MsgBox("Please enter a valid value and try again; possible entries are 1 through " & nTeams & " and you entered " & AdvanceWho)
            Exit Sub
        End If
        'update the ballot scores
        Dim fdBallots, fdScores As DataRow()
        fdBallots = ds.Tables("Ballot").Select("Panel=" & panel)
        For x = 0 To fdBallots.Length - 1
            ValidateScoresByBallot(ds, fdBallots(x).Item("ID"))
            fdScores = ds.Tables("Ballot_Score").Select("Ballot=" & fdBallots(x).Item("ID") & " and Score_ID=1")
            For y = 0 To fdScores.Length - 1
                fdScores(y).Item("Score") = 0
                If fdScores(y).Item("Recipient") = fdTeams(AdvanceWho) Then
                    fdScores(y).Item("Score") = 1
                Else
                    fdScores(y).Item("Score") = 0
                End If
            Next y
        Next x
        Call UpdateDecision(panel)
        Call CurrentWinners()
    End Sub

    Private Sub butWebBallots_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butWebBallots.Click

        Dim panel As Integer
        Try
            panel = DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells(DataGridView1.CurrentCell.ColumnIndex - 1).Value
        Catch
            MsgBox("You must click on one debate to set the round for download.")
            Exit Sub
        End Try
        Dim drPanel As DataRow : drPanel = ds.Tables("Panel").Rows.Find(panel)

        Dim dtDummy As DateTime = Now
        Call DoNewBallotDownLoad(ds, drPanel.Item("Round"))
        Dim strdummy As String = DateDiff(DateInterval.Second, dtDummy, Now)
        'Label1.Text = "Download done.  Seconds elapsed:" & strdummy : Label1.Refresh()
        'Call showelimballots3(ds, drPanel.Item("Round"))
        Call showelimballots4(ds, drPanel.Item("Round"))
        'update screen
        If DataGridView2.RowCount > 0 Then
            Call CellClick()
            For x = 0 To DataGridView1.RowCount - 1
                If Not DataGridView1.Rows(x).Cells(DataGridView1.CurrentCell.ColumnIndex - 1).Value Is System.DBNull.Value Then
                    panel = DataGridView1.Rows(x).Cells(DataGridView1.CurrentCell.ColumnIndex - 1).Value
                    Dim fdBallots As DataRow()
                    fdBallots = ds.Tables("Ballot").Select("Panel=" & panel, "Judge asc, Entry asc")
                    Call UpdateSides(fdBallots)
                    Call UpdateDecision(panel)
                End If
            Next x
        End If
        Call CurrentWinners()
        MsgBox("Done")
    End Sub

    Private Sub butClearDecision_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butClearDecision.Click
        'find the debate
        Dim panel As Integer
        Try
            panel = DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells(DataGridView1.CurrentCell.ColumnIndex - 1).Value
        Catch
            MsgBox("Click on a debate to indicate the round to delete, and try again.")
            Exit Sub
        End Try
        'clear all the ballot scores
        Dim fdBallot As DataRow() : Dim fdBalScore As DataRow()
        fdBallot = ds.Tables("Ballot").Select("Panel=" & panel)
        For x = 0 To fdBallot.Length - 1
            fdBalScore = ds.Tables("Ballot_Score").Select("Ballot=" & fdBallot(x).Item("ID") & " and score_ID=1")
            For y = 0 To fdBalScore.Length - 1
                fdBalScore(y).Item("Score") = 0
            Next y
        Next x
        Call CellClick()
    End Sub
    Sub ResetGrids() Handles cboDivisions.SelectedValueChanged
        DataGridView1.DataSource = Nothing
        DataGridView2.DataSource = Nothing
        DataGridView3.DataSource = Nothing
    End Sub
    Function FlightingMatch() As Boolean
        FlightingMatch = True
        Dim dr As DataRow
        dr = ds.Tables("Round").Rows.Find(GetFirstElim(ds, cboDivisions.SelectedValue))
        If dr.Item("Flighting") < 2 Then Exit Function
        Dim drLastPrelim As DataRow
        drLastPrelim = ds.Tables("Round").Rows.Find(GetLastPrelim(ds, dr.Item("Event")))
        If drLastPrelim.Item("Flighting") < dr.Item("Flighting") Then FlightingMatch = False
    End Function
    Private Sub butChangeBracket_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butChangeBracket.Click
        If DataGridView1.RowCount = 0 Then
            MsgBox("Please load a division and try again.")
            Exit Sub
        End If
        Dim f As New Form
        f = frmElimSeedPopup
        f.Show()
    End Sub
End Class