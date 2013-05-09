Public Class frmEnterDivisions
    Dim ds As New DataSet
    Dim dtDebateTypes As New DataTable
    Private masterBindingSource As New BindingSource()
    Private detailsBindingSource As New BindingSource()
    Dim RowAddMode As Boolean

    Private Sub frmEnterDivisions_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Call SetUpFixedValues() 'creates a local table with the different types of debate hard-coded in there
        Call LoadFile(ds, "TourneyData")
        Call FillDefaultDebateTypes() 'In case a division doesn't have a type of debate associated with it, this sets it to policy
        Call EventSettingsCheck(ds) 'Makes sure that all the divisions have a field for the supported settings
        Call EventValidator() 'fires error message for out-of-range values
        Call FixLevelCase() 'sets open, jv, novice to lower case

        'add the fixed debate types
        'This adds the division column as a combobox; the others are managed in the design view by setting the collection
        Dim dgvc As New DataGridViewComboBoxColumn
        dgvc.DataSource = dtDebateTypes         'this is child/detail table; debate type datatable in this case
        dgvc.ValueMember = "DebateType"         'on the child/detail table
        dgvc.DisplayMember = "DebateType"       'on the child/detail table
        dgvc.DataPropertyName = "TYPE"          'field from the master/parent table; ENTRY in this case
        dgvc.Name = "TYPE"
        dgvc.HeaderText = "Debate Type"
        dgvc.SortMode = DataGridViewColumnSortMode.Automatic
        DataGridView2.Columns.Add(dgvc)

        'I don't really get what the next 2 chunks of code do, but they end up with a parent/child relationship
        'between the tables

        ' Bind the master data connector .
        masterBindingSource.DataSource = ds
        masterBindingSource.DataMember = "EVENT"

        ' Bind the details data connector to the master data connector,
        ' using the DataRelation name to filter the information in the 
        ' details table based on the current row in the master table. 
        detailsBindingSource.DataSource = masterBindingSource
        detailsBindingSource.DataMember = "EventSetting"

        DataGridView2.DataSource = masterBindingSource
        DataGridView1.AutoGenerateColumns = False
        DataGridView1.DataSource = detailsBindingSource
        Call DownloadFieldStripper()
    End Sub
    Sub DownloadFieldStripper()
        DataGridView1.CurrentCell = Nothing
        For x = 0 To DataGridView1.RowCount - 1
            If DataGridView1.Rows(x).Cells("Tag").Value.ToString.Trim = "code_style" Then DataGridView1.Rows(x).Visible = False
            If DataGridView1.Rows(x).Cells("Tag").Value.ToString.Trim = "code_start" Then DataGridView1.Rows(x).Visible = False
            If DataGridView1.Rows(x).Cells("Tag").Value.ToString.Trim = "point_increments" Then DataGridView1.Rows(x).Visible = False
            If DataGridView1.Rows(x).Cells("Tag").Value.ToString.Trim = "max_points" Then DataGridView1.Rows(x).Visible = False
            If DataGridView1.Rows(x).Cells("Tag").Value.ToString.Trim = "hybrids" Then DataGridView1.Rows(x).Visible = False
            If DataGridView1.Rows(x).Cells("Tag").Value.ToString.Trim = "ballot_type" Then DataGridView1.Rows(x).Visible = False
            If DataGridView1.Rows(x).Cells("Tag").Value.ToString.Trim = "online_ballots" Then DataGridView1.Rows(x).Visible = False
            If DataGridView1.Rows(x).Cells("Tag").Value.ToString.Trim = "deadline" Then DataGridView1.Rows(x).Visible = False
            If DataGridView1.Rows(x).Cells("Tag").Value.ToString.Trim = "min_points" Then DataGridView1.Rows(x).Visible = False
            If DataGridView1.Rows(x).Cells("Tag").Value.ToString.Trim = "code_hide" Then DataGridView1.Rows(x).Visible = False
            If DataGridView1.Rows(x).Cells("Tag").Value.ToString.Trim = "waitlist" Then DataGridView1.Rows(x).Visible = False
            If DataGridView1.Rows(x).Cells("Tag").Value.ToString.Trim = "live_updates" Then DataGridView1.Rows(x).Visible = False
            If DataGridView1.Rows(x).Cells("Tag").Value.ToString.Trim = "cap" Then DataGridView1.Rows(x).Visible = False
            If DataGridView1.Rows(x).Cells("Tag").Value.ToString.Trim = "school_cap" Then DataGridView1.Rows(x).Visible = False
            If DataGridView1.Rows(x).Cells("Tag").Value.ToString.Trim = "field_report" Then DataGridView1.Rows(x).Visible = False
            If DataGridView1.Rows(x).Cells("Tag").Value.ToString.Trim = "self_strike" Then DataGridView1.Rows(x).Visible = False
        Next x
    End Sub
    Private Sub frmEnterDivisions_Unload(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Disposed
        Call SaveFile(ds)
        Call SetupCompletionStatus(ds)
        ds.Dispose()
    End Sub
    Sub SetUpFixedValues()
        'debate types
        dtDebateTypes.Columns.Add("ID", System.Type.GetType("System.Int64"))
        dtDebateTypes.Columns.Add("DebateType", System.Type.GetType("System.String"))
        Dim dr As DataRow
        dr = dtDebateTypes.NewRow : dr.Item("ID") = 1 : dr.Item("DebateType") = "Policy" : dtDebateTypes.Rows.Add(dr)
        dr = dtDebateTypes.NewRow : dr.Item("ID") = 2 : dr.Item("DebateType") = "Parli" : dtDebateTypes.Rows.Add(dr)
        dr = dtDebateTypes.NewRow : dr.Item("ID") = 3 : dr.Item("DebateType") = "Lincoln-Douglas" : dtDebateTypes.Rows.Add(dr)
        dr = dtDebateTypes.NewRow : dr.Item("ID") = 4 : dr.Item("DebateType") = "WUDC" : dtDebateTypes.Rows.Add(dr)
        dr = dtDebateTypes.NewRow : dr.Item("ID") = 5 : dr.Item("DebateType") = "Other" : dtDebateTypes.Rows.Add(dr)
        dtDebateTypes.AcceptChanges()

    End Sub
    Sub FillDefaultDebateTypes()
        Dim x, y As Integer : Dim OK As Boolean
        For x = 0 To ds.Tables("EVENT").Rows.Count - 1
            OK = False
            For y = 0 To dtDebateTypes.Rows.Count - 1
                If ds.Tables("EVENT").Rows(x).Item("Type") Is System.DBNull.Value Then ds.Tables("EVENT").Rows(x).Item("Type") = ""
                If ds.Tables("EVENT").Rows(x).Item("Type") = dtDebateTypes.Rows(y).Item("DebateType") Then OK = True
            Next y
            If OK = False Then ds.Tables("EVENT").Rows(x).Item("Type") = "Policy"
        Next x
    End Sub
    Sub MakeComboBoxes()

        Dim R As Integer

        Dim cb As New DataGridViewComboBoxCell
        cb.Items.Add("Aff/Neg")
        cb.Items.Add("Gov/Opp")
        cb.Items.Add("WUDC")
        cb.ValueMember = "SideDesignation"
        cb.DisplayMember = "SideDesignation"
        R = getDGRow("SideDesignations")
        DataGridView1.Rows(R).Cells("Value") = New DataGridViewComboBoxCell()
        DataGridView1.Rows(R).Cells("Value") = cb

        Dim cb2 As New DataGridViewComboBoxCell
        cb2.Items.Add("open")
        cb2.Items.Add("jv")
        cb2.Items.Add("novice")
        cb2.Items.Add("rookie")
        cb2.ValueMember = "Level"
        cb2.DisplayMember = "Level"
        R = getDGRow("Level")
        DataGridView1.Rows(R).Cells("Value") = New DataGridViewComboBoxCell()
        DataGridView1.Rows(R).Cells("Value") = cb2

        Dim cb3 As New DataGridViewComboBoxCell
        cb3.Items.Add("EqualizePrelims")
        cb3.Items.Add("EqualizeElims")
        cb3.Items.Add("CoinToss")
        cb3.ValueMember = "SideMethod"
        cb3.DisplayMember = "SideMethod"
        R = getDGRow("SideMethod")
        DataGridView1.Rows(R).Cells("Value") = New DataGridViewComboBoxCell()
        DataGridView1.Rows(R).Cells("Value") = cb3

        cb3 = New DataGridViewComboBoxCell
        cb3.Items.Add("true")
        cb3.Items.Add("false")
        cb3.ValueMember = "UseRegions"
        cb3.DisplayMember = "UseRegions"
        R = getDGRow("UseRegions")
        DataGridView1.Rows(R).Cells("Value") = New DataGridViewComboBoxCell()
        DataGridView1.Rows(R).Cells("Value") = cb3

        cb3 = New DataGridViewComboBoxCell
        cb3.Items.Add("true")
        cb3.Items.Add("false")
        cb3.ValueMember = "AllowSameSchoolDebates"
        cb3.DisplayMember = "AllowSameSchoolDebates"
        R = getDGRow("AllowSameSchoolDebates")
        DataGridView1.Rows(R).Cells("Value") = New DataGridViewComboBoxCell()
        DataGridView1.Rows(R).Cells("Value") = cb3

        cb3 = New DataGridViewComboBoxCell
        cb3.Items.Add("true")
        cb3.Items.Add("false")
        cb3.ValueMember = "Allow2dMeeting"
        cb3.DisplayMember = "Allow2dMeeting"
        R = getDGRow("Allow2dMeeting")
        DataGridView1.Rows(R).Cells("Value") = New DataGridViewComboBoxCell()
        DataGridView1.Rows(R).Cells("Value") = cb3

        cb3 = New DataGridViewComboBoxCell
        cb3.Items.Add("true")
        cb3.Items.Add("false")
        cb3.ValueMember = "PanelDecisions"
        cb3.DisplayMember = "PanelDecisions"
        R = getDGRow("PanelDecisions")
        DataGridView1.Rows(R).Cells("Value") = New DataGridViewComboBoxCell()
        DataGridView1.Rows(R).Cells("Value") = cb3

    End Sub
    Function getDGRow(ByVal strEventSetting As String) As Integer
        For x = 0 To DataGridView1.Rows.Count - 1
            If DataGridView1.Rows(x).Cells(2).Value.toupper = strEventSetting.ToUpper Then
                getDGRow = x : Exit Function
            End If
        Next
    End Function
    Sub ActiveCombos() Handles DataGridView1.Click
        Call MakeComboBoxes()
    End Sub
    Sub ShowDGV2Help() Handles DataGridView2.SelectionChanged
        If DataGridView2.CurrentCell Is Nothing Then Exit Sub
        If DataGridView2.CurrentCell.ColumnIndex = 1 Then
            lblInfo.Text = "Enter the full, complete name of this division/event, for example, 'Open Policy Debate'"
        ElseIf DataGridView2.CurrentCell.ColumnIndex = 2 Then
            lblInfo.Text = "Enter a short name forthis division/event, for example, 'Open Policy'"
        ElseIf DataGridView2.CurrentCell.ColumnIndex = 3 Then
            lblInfo.Text = "From the drop-down list, select the type of debate for this division.  Remember that you need to hit TAB or ENTER after making your selection."
        End If
        Call DownloadFieldStripper()
    End Sub
    Sub ShowDGV1Help() Handles DataGridView1.SelectionChanged
        If DataGridView1.CurrentCell Is Nothing Then Exit Sub
        If DataGridView1.CurrentRow.Cells("TAG").Value = "nPrelims" Then
            lblInfo.Text = "Enter the total number of preliminary rounds of debate for this division."
        ElseIf DataGridView1.CurrentRow.Cells("TAG").Value = "DebatersPerTeam" Then
            lblInfo.Text = "Enter the number of debaters per team.  Do not account for hybrid or swing teams, just the number of debaters who would normally appear on a team."
        ElseIf DataGridView1.CurrentRow.Cells("TAG").Value = "Level" Then
            lblInfo.Text = "Select the level of competiton from the drop-down list."
        ElseIf DataGridView1.CurrentRow.Cells("TAG").Value = "nElims" Then
            lblInfo.Text = "Enter the number of elim ROUNDS you will have.  For example, clearing straight to finals=1 elim round, semis=2, quarters=3, octos=4, doubles=5, triples=6, quads=7.  Many tournaments will clear up to half the teams, and very few tournaments clear more than half the entered teams.  If you are not sure, err on the side of a larger number of rounds."
        ElseIf DataGridView1.CurrentRow.Cells("TAG").Value = "SideDesignations" Then
            lblInfo.Text = "Select the manner in which sides will be displayed from the drop-down list."
        ElseIf DataGridView1.CurrentRow.Cells("TAG").Value = "TeamsPerRound" Then
            lblInfo.Text = "Enter the number of teams who will compete against each other in a given preliminary round."
        ElseIf DataGridView1.CurrentRow.Cells("TAG").Value = "SideMethod" Then
            lblInfo.Text = "Normally, if there are 2 teams per debate, sides will be equalized in even-numbered rounds.  Select the side equalization method from the drop-down list."
        ElseIf DataGridView1.CurrentRow.Cells("TAG").Value = "UseRegions" Then
            lblInfo.Text = "Indicate whether geographical region information should be used for the pairings.  If you do use region information, make sure that you have set a region for each school on the schools setup screen."
        ElseIf DataGridView1.CurrentRow.Cells("TAG").Value = "nPresets" Then
            lblInfo.Text = "Enter the number of inital preliminary debates that will be preset."
        ElseIf DataGridView1.CurrentRow.Cells("TAG").Value = "AllowSameSchoolDebates" Then
            lblInfo.Text = "Allow teams from the same school to be paired against one another."
        ElseIf DataGridView1.CurrentRow.Cells("TAG").Value = "Allow2dMeeting" Then
            lblInfo.Text = "Allow teams that have already debated against each other to be paired a second time.  The side from the first meeting will be displayed at pairing time."
        ElseIf DataGridView1.CurrentRow.Cells("TAG").Value = "PanelDecisions" Then
            lblInfo.Text = "A value of TRUE indicates that the entire panel renders a single decision.  A value of FALSE means that each judge casts an individual ballot."
        End If
    End Sub
    Sub ValidateDG1() Handles DataGridView1.CellValueChanged
        Call EventValidator()
    End Sub
    Sub EventValidator()
        If DataGridView1.RowCount = 0 Then Exit Sub
        Dim nPresets, nPrelims, nElims As Integer
        For x = 0 To DataGridView1.RowCount - 1
            If DataGridView1.Rows(x).Cells("TAG").Value = "nPrelims" Then nPrelims = DataGridView1.Rows(x).Cells("Value").Value
            If DataGridView1.Rows(x).Cells("TAG").Value = "nPresets" Then nPresets = DataGridView1.Rows(x).Cells("Value").Value
            If DataGridView1.Rows(x).Cells("TAG").Value = "nElims" Then nElims = DataGridView1.Rows(x).Cells("Value").Value
        Next x
        If nPrelims < nPresets Then
            MsgBox("WARNING:  You have indicated more preset rounds than there are preliminary rounds.  Either increase the number of preliminary rounds or decrease the number of preset rounds.", MsgBoxStyle.OkOnly)
        End If
        If (nPrelims + nElims) > ds.Tables("TimeSLot").Rows.Count Then
            MsgBox("WARNING: You have indicated more preliminary + elimination rounds for this division than there are time slots available.  Either reduce the total number of prelimnary and elimination rounds, or return to the tournament settings screen and increase the number of timeslots.")
        End If
    End Sub

    Private Sub butHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butHelp.Click
        Dim strHelp As String = ""
        strHelp &= "Use the left-hand grid to add or delete divisions.  Note that if you delete a division, you will delete all the entries and any resulst associatied with that division, so do so with extreme caution." & Chr(10) & Chr(10)
        strHelp &= "To add a division, simply type in the new information on the bottom row.  After you have added the division click the button at the bottom of the grid to create default settings." & Chr(10) & Chr(10)
        strHelp &= "To delete a division, select the row by clicking on the gray box to the far left of the grid and then hit the DELETE button on your keyboard." & Chr(10) & Chr(10)
        strHelp &= "Use the right-hand grid to change the settings for the division.  When you click on any row, information about the setting will appear in the space to the right of the grid." & Chr(10) & Chr(10)
        MsgBox(strHelp, MsgBoxStyle.OkOnly)
    End Sub
    Private Sub UserDeletingRow(ByVal sender As Object, ByVal e As DataGridViewRowCancelEventArgs) Handles DataGridView2.UserDeletingRow
        Dim q As Integer
        q = MsgBox("WARNING!  This will delete all entries and results associated with this division!  Click OK to continue or CANCEL to continue without deleting.", MsgBoxStyle.OkCancel)
        If q = vbCancel Then e.Cancel = True
    End Sub

    Private Sub butAddDeleteHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butAddDeleteHelp.Click
        Dim strHelp As String = ""
        strHelp &= "To add a division, simply type in the new information on the bottom row.  After you have added the division click the button at the bottom of the grid to create default settings." & Chr(10) & Chr(10)
        strHelp &= "To delete a division, select the row by clicking on the gray box to the far left of the grid and then hit the DELETE button on your keyboard." & Chr(10) & Chr(10)
        MsgBox(strHelp, MsgBoxStyle.OkOnly)
    End Sub
    Sub EHandler() Handles DataGridView2.DataError
        MsgBox("Please select a valid value from the drop-down list in the last column.  Make sure you click on drop-down list with the mouse, select a valid value, and hit return.")
    End Sub

    Private Sub butCreateSettings_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butCreateSettings.Click
        Call EventSettingsCheck(ds)
    End Sub

    Private Sub butResetSettings_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butResetSettings.Click
        Dim fdSettings As DataRow()
        For x = 0 To DataGridView2.RowCount - 2
            fdSettings = ds.Tables("Event_Setting").Select("Event=" & DataGridView2.Rows(x).Cells("ID").Value)
            For y = fdSettings.Length - 1 To 0 Step -1
                fdSettings(y).Delete()
            Next y
        Next x
        Call EventSettingsCheck(ds)
    End Sub
    Sub FixLevelCase()
        For x = 0 To ds.Tables("Event_Setting").Rows.Count - 1
            If ds.Tables("Event_Setting").Rows(x).Item("Value").tolower = "open" Then
                ds.Tables("Event_Setting").Rows(x).Item("Value") = "open"
            ElseIf ds.Tables("Event_Setting").Rows(x).Item("Value").tolower = "jv" Then
                ds.Tables("Event_Setting").Rows(x).Item("Value") = "jv"
            ElseIf ds.Tables("Event_Setting").Rows(x).Item("Value").tolower = "novice" Then
                ds.Tables("Event_Setting").Rows(x).Item("Value") = "novice"
            End If
        Next x
    End Sub
End Class