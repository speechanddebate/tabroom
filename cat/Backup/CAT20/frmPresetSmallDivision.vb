Imports System.IO
Public Class frmPresetSmallDivision
    Dim ds As New DataSet
    Dim nSchools, nTeams, bigSchool As Integer

    Private Sub frmPresetSmallDivision_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LoadFile(ds, "TourneyData")

        'bind round CBO
        cboDivision.DataSource = ds.Tables("Event")
        cboDivision.DisplayMember = "EventName"
        cboDivision.ValueMember = "ID"

        Call LoadPresetTable()
    End Sub
    Private Sub frmPresetSmallDivision_Unload(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Disposed
        Call SaveFile(ds)
        ds.Dispose()
    End Sub
    Sub LoadPresetTable()
        Dim dt As New DataTable
        dt.Columns.Add(("Label"), System.Type.GetType("System.String"))
        dt.Columns.Add(("TotalTeams"), System.Type.GetType("System.Int64"))
        dt.Columns.Add(("Slot1"), System.Type.GetType("System.Int64"))
        dt.Columns.Add(("Slot2"), System.Type.GetType("System.Int64"))
        dt.Columns.Add(("Slot3"), System.Type.GetType("System.Int64"))
        dt.Columns.Add(("Slot4"), System.Type.GetType("System.Int64"))
        dt.Columns.Add(("Slot5"), System.Type.GetType("System.Int64"))
        Dim dr As DataRow
        Dim spots(5) As Integer
        For x = 1 To 23
            For y = 1 To 5 : spots(y) = 0 : Next y
            '4-team scheme
            If x = 22 Then spots(1) = 2 : spots(2) = 2 : spots(3) = 0 : spots(4) = 0 : spots(5) = 0
            If x = 23 Then spots(1) = 1 : spots(2) = 1 : spots(3) = 1 : spots(4) = 1 : spots(5) = 0
            '6-team scheme
            If x = 1 Then spots(1) = 2 : spots(2) = 1 : spots(3) = 1 : spots(4) = 1 : spots(5) = 1
            If x = 2 Then spots(1) = 3 : spots(2) = 3 : spots(3) = 0 : spots(4) = 0 : spots(5) = 0
            If x = 21 Then spots(1) = 2 : spots(2) = 2 : spots(3) = 2 : spots(4) = 0 : spots(5) = 0
            '8-team scheme
            If x = 3 Then spots(1) = 4 : spots(2) = 1 : spots(3) = 1 : spots(4) = 1 : spots(5) = 1
            If x = 4 Then spots(1) = 2 : spots(2) = 2 : spots(3) = 2 : spots(4) = 2 : spots(5) = 0
            '10-teams
            If x = 5 Then spots(1) = 4 : spots(2) = 3 : spots(3) = 3 : spots(4) = 0 : spots(5) = 0
            If x = 6 Then spots(1) = 4 : spots(2) = 2 : spots(3) = 2 : spots(4) = 2 : spots(5) = 0
            If x = 7 Then spots(1) = 3 : spots(2) = 3 : spots(3) = 2 : spots(4) = 2 : spots(5) = 0
            If x = 8 Then spots(1) = 2 : spots(2) = 2 : spots(3) = 2 : spots(4) = 2 : spots(5) = 2
            '12-teams
            If x = 9 Then spots(1) = 5 : spots(2) = 5 : spots(3) = 2 : spots(4) = 0 : spots(5) = 0
            If x = 10 Then spots(1) = 4 : spots(2) = 4 : spots(3) = 4 : spots(4) = 0 : spots(5) = 0
            If x = 11 Then spots(1) = 3 : spots(2) = 3 : spots(3) = 3 : spots(4) = 3 : spots(5) = 0
            If x = 12 Then spots(1) = 3 : spots(2) = 3 : spots(3) = 2 : spots(4) = 2 : spots(5) = 2
            '14-teams
            If x = 13 Then spots(1) = 6 : spots(2) = 6 : spots(3) = 2 : spots(4) = 0 : spots(5) = 0
            If x = 14 Then spots(1) = 5 : spots(2) = 5 : spots(3) = 4 : spots(4) = 0 : spots(5) = 0
            If x = 15 Then spots(1) = 5 : spots(2) = 5 : spots(3) = 2 : spots(4) = 2 : spots(5) = 0
            If x = 16 Then spots(1) = 4 : spots(2) = 4 : spots(3) = 4 : spots(4) = 2 : spots(5) = 0
            If x = 17 Then spots(1) = 3 : spots(2) = 3 : spots(3) = 3 : spots(4) = 3 : spots(5) = 2
            If x = 18 Then spots(1) = 6 : spots(2) = 4 : spots(3) = 4 : spots(4) = 2 : spots(5) = 0
            '16-teams
            If x = 19 Then spots(1) = 5 : spots(2) = 5 : spots(3) = 3 : spots(4) = 3 : spots(5) = 0
            If x = 20 Then spots(1) = 4 : spots(2) = 4 : spots(3) = 4 : spots(4) = 4 : spots(5) = 0

            If spots(1) > 0 Then
                dr = dt.NewRow : dr.Item("TotalTeams") = 0
                For y = 1 To 5
                    dr.Item("Label") &= spots(y).ToString.Trim
                    dr.Item("TotalTeams") += spots(y)
                    dr.Item("Slot" & y.ToString) = spots(y)
                Next
                dr.Item("Label") = dr.Item("TotalTeams") & "/" & dr.Item("Label")
                dt.Rows.Add(dr)
            End If
        Next x

        dt.DefaultView.Sort = "totalteams asc"
        DataGridView1.DataSource = dt
        DataGridView1.Columns("TotalTeams").HeaderText = "Teams"
        DataGridView1.Columns("Label").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells

        'clear the gird
        DataGridView1.CurrentCell = Nothing
        DataGridView1.Rows(0).Selected = False
    End Sub
    Private Sub butLoadDivision_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butLoadDivision.Click
        Dim fdEntries As DataRow()
        fdEntries = ds.Tables("Entry").Select("Event=" & cboDivision.SelectedValue, "School ASC")
        lblStatus.Text = fdEntries.Length & " teams entered."
        Dim dummy As Integer
        For x = 0 To fdEntries.Length - 1
            If x = 0 Then
                nSchools += 1 : dummy = 1
            Else
                If fdEntries(x).Item("School") = fdEntries(x - 1).Item("School") Then dummy += 1
                If fdEntries(x).Item("School") <> fdEntries(x - 1).Item("School") Then
                    nSchools += 1
                    If dummy > bigSchool Then bigSchool = dummy
                    dummy = 1
                End If
            End If
        Next x
        nTeams = fdEntries.Length
        lblStatus.Text = nTeams & " teams, " & nSchools & " schools, largest school has " & bigSchool & " entries."
        Call TestPresets()
    End Sub
    Sub TestPresets()
        'Takes in the number of teams and schools for the selected division, compares that against the preset scheme,
        'And picks the best preset scheme for the division
        Dim bestSchools, bestRow, SchemeTeams, dummy, SchemeSchools As Integer
        Dim dt As New DataTable
        bestRow = -1
        For x = 0 To DataGridView1.RowCount - 1
            dt = TableFromArray(x) 'takes the row from schemes grid and makes it into a populatable datatable
            SchemeTeams = 0 : SchemeSchools = 0
            For y = 1 To 5
                SchemeTeams += DataGridView1.Rows(x).Cells("Slot" & y.ToString).Value 'Count # teams in scheme
                If DataGridView1.Rows(x).Cells("Slot" & y.ToString).Value > 0 Then SchemeSchools += 1
            Next y
            'counts the number of teams in the selected division, and sees if the preset is appropriate
            dummy = nTeams
            If dummy / 2 <> Int(dummy / 2) Then dummy += 1
            If dummy = SchemeTeams Then
                If FillIt(dt, SchemeSchools) = True And SchemeSchools > bestSchools Then
                    bestRow = x
                    bestSchools = SchemeSchools
                End If
            End If
        Next x
        If bestRow = -1 Then MsgBox("The CAT could not find a preset round scheme to accomodate this event/division.  You should return to the main menu and see if you can create one manually.  You might consult the HOW TO TAB text, available online, which has a detailed discussion of the issue.") : Exit Sub
        DataGridView1.Rows(bestRow).Selected = True
        DataGridView1.CurrentCell = DataGridView1(0, bestRow)
        dt = TableFromArray(bestRow) : Call FillIt(dt, bestSchools)
        DataGridView2.DataSource = dt
        Dim strInfo As String = ""
        strInfo &= "Good news!  There is a preset round scheme that matches this division." & Chr(10) & Chr(10)
        strInfo &= "The best fit has been selected in the large grid in the middle of the screen." & Chr(10) & Chr(10)
        strInfo &= "The teams have been placed in appropriate order in the grid in the lower-right." & Chr(10) & Chr(10)
        strInfo &= "You should review the information, and when you are ready, click on the PAIR THE DIVISION USING THE SELECTED PRESET SCHEME button." & Chr(10) & Chr(10)
        strInfo &= "When you do so, all pairings in the division will be preset.  Note that you must still place ROOMS and JUDGES, which you can do from the main menu by clicking the VIEW A PAIRING FOR MANUAL CHANGES button." & Chr(10) & Chr(10)
        strInfo &= "At this time, you cannot make manual changes to this page." & Chr(10) & Chr(10)
        MsgBox(strInfo, , "Small Division Explanation")
        Call nPrelimChecker(bestRow)
    End Sub
    Sub nPrelimChecker(ByVal bestRow As Integer)
        'Resets the number of prelims and displays any necessary message
        Dim nDebates As Integer = 6
        If DataGridView1.Rows(bestRow).Cells("Label").Value = "4/22000" Then
            nDebates = 4
            MsgBox("The best configuration for this division will have 4 debates, and all teams will debate all possible opponents twice, switching sides the second meeting.")
        End If
        If DataGridView1.Rows(bestRow).Cells("Label").Value = "4/11110" Then
            MsgBox("The best configuration for this division will have 6 debates, and all teams will debate all possible opponents twice, switching sides the second meeting.")
        End If
        If DataGridView1.Rows(bestRow).Cells("Label").Value = "6/21111" Then
            nDebates = 4
            MsgBox("The best configuration for this division will have 4 debates.")
        End If
        If DataGridView1.Rows(bestRow).Cells("Label").Value = "6/33000" Then
            MsgBox("The best configuration for this division will have 6 debates, and all teams will debate all possible opponents twice, switching sides the second meeting.")
        End If
        If DataGridView1.Rows(bestRow).Cells("Label").Value = "6/22200" Then
            nDebates = 4
            MsgBox("The best configuration for this division will have 4 debates.")
        End If
        Dim drEvent As DataRow : Dim drEventSetting As DataRow()
        drEvent = ds.Tables("Event").Rows.Find(cboDivision.SelectedValue)
        drEventSetting = ds.Tables("Event_Setting").Select("Event=" & drEvent.Item("ID") & " and tag='nPrelims'")
        If drEventSetting(0).Item("Value") <> nDebates Then
            Dim q As Integer = MsgBox("This event/division is currently set for " & drEventSetting(0).Item("Value") & " prelim rounds, but given the size it will require " & nDebates & " to preset the prelims.  Should the CAT change the settings to match this preset scheme (YES is recommended)?", MsgBoxStyle.YesNo)
            If q = vbYes Then drEventSetting(0).Item("Value") = nDebates
        End If
    End Sub
    Function TableFromArray(ByVal dgRow) As DataTable
        'makes a table of teams to populate from the schemes grid
        Dim spots(5) As Integer
        Dim dt As New DataTable
        dt.Columns.Add(("TeamName"), System.Type.GetType("System.String"))
        dt.Columns.Add(("School"), System.Type.GetType("System.Int64"))
        dt.Columns.Add(("TeamID"), System.Type.GetType("System.Int64"))
        Dim dr As DataRow
        For y = 1 To 5
            spots(y) = DataGridView1.Rows(dgRow).Cells("Slot" & y.ToString).Value
        Next y
        For x = 1 To 5
            For y = 1 To spots(x)
                dr = dt.NewRow : dr.Item("School") = x : dr.Item("TeamName") = "N/A" : dr.Item("TeamID") = 0 : dt.Rows.Add(dr)
            Next y
        Next x
        Return dt
    End Function
    Function FillIt(ByRef dtPreset As DataTable, ByVal totSch As Integer) As Boolean
        'dtPreset is the preset schedule, totsch is the # of different schools in the preset schedule
        'This identify whether the preset schedule fits the current division teams/schools, and if so, 
        'fills the datatable with teams
        FillIt = True
        'Load the teams, put in a datatable and sort by most teams
        Dim fdTeams, fdDummy As DataRow()
        fdTeams = ds.Tables("Entry").Select("Event=" & cboDivision.SelectedValue, "School ASC")
        Dim dtTeams As New DataTable
        dtTeams.Columns.Add(("TeamName"), System.Type.GetType("System.String"))
        dtTeams.Columns.Add(("TeamID"), System.Type.GetType("System.Int64"))
        dtTeams.Columns.Add(("School"), System.Type.GetType("System.Int64"))
        dtTeams.Columns.Add(("nSchool"), System.Type.GetType("System.Int64"))
        Dim dr As DataRow
        For x = 0 To fdTeams.Length - 1
            dr = dtTeams.NewRow
            dr.Item("TeamName") = fdTeams(x).Item("Code")
            dr.Item("School") = fdTeams(x).Item("School")
            dr.Item("TeamID") = fdTeams(x).Item("ID")
            fdDummy = ds.Tables("Entry").Select("Event=" & cboDivision.SelectedValue & " and School=" & fdTeams(x).Item("School"))
            dr.Item("nSchool") = fdDummy.Length
            dtTeams.Rows.Add(dr)
        Next x
        'add a bye if necessary
        If dtTeams.Rows.Count - dtPreset.Rows.Count = -1 Then
            dr = dtTeams.NewRow
            dr.Item("TeamName") = "Bye"
            dr.Item("School") = -99
            dr.Item("TeamID") = -99
            dr.Item("nSchool") = 1
            dtTeams.Rows.Add(dr)
        End If
        dtTeams.DefaultView.Sort = "nSchool DESC, school DESC"
        'now count how many schools there are
        Dim nSchools(10) As Integer : Dim ctr As Integer
        For x = 1 To dtTeams.DefaultView.Count - 1
            If dtTeams.DefaultView(x).Item("School") <> dtTeams.DefaultView(x - 1).Item("School") Then ctr += 1 : nSchools(ctr) = dtTeams.DefaultView(x - 1).Item("School")
            If x = dtTeams.DefaultView.Count - 1 Then ctr += 1 : nSchools(ctr) = dtTeams.DefaultView(x).Item("School")
        Next x
        'loop through the schools and place them
        Dim Matched As Boolean : Dim matches As Integer
        For x = 1 To ctr
            fdTeams = dtTeams.Select("School=" & nSchools(x))
            For x1 = 1 To totSch
                matches = 0
                fdDummy = dtPreset.Select("TeamID=0 and School=" & x1)
                If fdTeams.Length <= fdDummy.Length Then
                    Matched = False
                    For y = 0 To fdTeams.Length - 1
                        For z = 0 To fdDummy.Length - 1
                            If fdDummy(z).Item("TeamID") = 0 Then
                                fdDummy(z).Item("TeamID") = fdTeams(y).Item("TeamID")
                                fdDummy(z).Item("TeamName") = fdTeams(y).Item("TeamName")
                                Matched = True : matches += 1
                                Exit For
                            End If
                        Next z
                    Next y
                End If
                If matches >= fdTeams.Length Then Exit For 'exit as soon as all teams on the school are placed
            Next x1
            If matches < fdTeams.Length Then
                FillIt = False
                Exit Function
            End If
        Next x
    End Function

    Private Sub butAutoPair_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butAutoPair.Click
        Call DeleteExistingPairings()
        'identify the pairing scheme to use
        Dim strPairScheme = DataGridView1.CurrentRow.Cells("Label").Value.ToString.Trim
        Dim fdRd As DataRow()
        Dim Rd, Aff, Neg, panel As Integer
        Dim dummy As String
        Dim reader As StreamReader = File.OpenText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\CAT\Presets.txt")
        Dim Flag As Boolean = False
        Do
            dummy = reader.ReadLine()
            If InStr(dummy, "/") > 0 And Flag = True Then Exit Do
            If Flag = True Then
                Rd = Val(Mid(dummy, 1, 1))
                Aff = Val(Mid(dummy, 2, 2))
                Neg = Val(Mid(dummy, 4, 2))
                fdRd = ds.Tables("Round").Select("Event=" & cboDivision.SelectedValue & " and rd_name=" & Rd)
                panel = AddPanel(ds, fdRd(0).Item("ID"), 0)
                If DataGridView2.Rows(Aff).Cells("TeamID").Value = -99 Then
                    Call AssignBye(ds, panel, DataGridView2.Rows(Neg).Cells("TeamID").Value, fdRd(0).Item("ID"))
                ElseIf DataGridView2.Rows(Neg).Cells("TeamID").Value = -99 Then
                    Call AssignBye(ds, panel, DataGridView2.Rows(Aff).Cells("TeamID").Value, fdRd(0).Item("ID"))
                Else
                    Call AddTeamToPanel(ds, panel, DataGridView2.Rows(Aff).Cells("TeamID").Value, 1)
                    Call AddTeamToPanel(ds, panel, DataGridView2.Rows(Neg).Cells("TeamID").Value, 2)
                End If
            End If
            If dummy.ToUpper.Trim = strPairScheme Then Flag = True
        Loop
        reader.Close()
        MsgBox("Pairings done")
    End Sub
    Sub DeleteExistingPairings()
        Dim fdRd As DataRow()
        fdRd = ds.Tables("Round").Select("event=" & cboDivision.SelectedValue)
        For x = 0 To fdRd.Length - 1
            Call DeleteAllPanelsForRound(ds, fdRd(x).Item("ID"))
        Next
    End Sub

    Private Sub butHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butHelp.Click
        Dim strInfo As String
        strInfo = "Begin by selecting a division/event and clicking load.  The CAT will attempt to automatically detect the best preset pairing for the division, and will do so in the center grid." & Chr(10) & Chr(10)
        strInfo &= "Review the center grid and, if necessary, manually change the preset scheme to use." & Chr(10) & Chr(10)
        strInfo &= "When you are sure the appropriate scheme has been selected in the center grid, click the 'pair the division' button in the right-hand column." & Chr(10) & Chr(10)
        MsgBox(strInfo, , "How to use the page")
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim strInfo As String
        strInfo = "Some entry situations present tight pairing constraints that make matching particularly troublesome.  Generally speaking, when there are very few teams entered in a division, or when one school has entered an inordinately high percentage of the teams in the division, schedule permutations are so constrained that random matching is unlikely to discover a 'clean' preliminary round schedule (a schedule in which no team is paired against another team from its own school, no team is paired against a team for a second time, and each team has an equal number of affirmative and negative debates).  In such cases, you have a 'tight' division where it makes sense to use a matching schematic, a pre-set schedule that uses orderly team rotation rather than random matching or power-matching to pair debates.  In such instances, your only task is to construct a clean schedule of preliminary debates.  Schedule features that you might address in larger divisions, such as variety of opposing schools, side balance with opposing schools, equitable strength-of-schedule, etc., become matters of random chance rather than design, if not from necessity then from administrative efficiency." & Chr(10) & Chr(10)
        MsgBox(strInfo, , "Why preset rounds?")
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim strInfo As String
        strInfo = "As a rule of thumb, when the total number of possible debates is less than the number of prelims times 2, you should strongly consider presetting the division." & Chr(10) & Chr(10)
        strInfo = "The total number of debates possible is the total number of teams in the division minus the largest entry by a single school." & Chr(10) & Chr(10)
        strInfo = "Example: A 10-team division has one school with 3 entries and 6 prelims.  The total number of possible debates is 7 (10 teams in the division minus the 3 teams from one school that can't debate each other.  Seven is less than 6 times 2 (number of prelims times 2), so there will be very little ability to power match later rounds, and you should strongly consider presetting." & Chr(10) & Chr(10)
        MsgBox(strInfo, , "When is a division small?")
    End Sub
End Class