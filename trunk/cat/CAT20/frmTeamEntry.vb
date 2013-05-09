Imports System.IO
Imports System.Net

Public Class frmTeamEntry
    Private masterDataGridView As New DataGridView()
    Private masterBindingSource As New BindingSource()
    Private detailsDataGridView As New DataGridView()
    Private detailsBindingSource As New BindingSource()
    Dim DS As New DataSet
    Dim mDS As New DataSet

    Private Sub frmTeamEntry_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Load
        LoadFile(DS, "TourneyData")
        'Call CheckMasterFile(DS)            disabled b/c no longer using offline download of masterfile

        Call TeamCountUpdate()

        'sort schools
        Dim dtv As New DataView(DS.Tables("School"))
        dtv.Sort = "CODE"

        'This adds the division column as a combobox; the others are managed in the design view by setting the collection
        Dim dgvc As New DataGridViewComboBoxColumn
        dgvc.DataSource = DS.Tables("EVENT")    'this is child/detail table; EVENT in this case
        dgvc.ValueMember = "ID"                 'on the child/detail table
        dgvc.DisplayMember = "EVENTNAME"        'on the child/detail table
        dgvc.DataPropertyName = "EVENT"         'field from the master/parent table; ENTRY in this case
        dgvc.Name = "EVENT"
        dgvc.HeaderText = "Division"
        dgvc.DisplayIndex = 2
        dgvc.SortMode = DataGridViewColumnSortMode.Automatic
        DataGridView1.Columns.Add(dgvc)

        'This adds the school column as a combobox to the students child table
        Dim dgvc2 As New DataGridViewComboBoxColumn
        dgvc2.DataSource = dtv
        dgvc2.ValueMember = "ID"
        dgvc2.DisplayMember = "CODE"
        dgvc2.DataPropertyName = "SCHOOL"
        dgvc2.HeaderText = "SCHOOL"
        dgvc2.Name = "SCHOOL"
        dgvc2.DisplayIndex = 2
        DataGridView2.Columns.Add(dgvc2)

        'This adds the school column as a combobox; the others are managed in the design view by setting the collection
        Dim dgvc3 As New DataGridViewComboBoxColumn
        dgvc3.DataSource = dtv
        dgvc3.ValueMember = "ID"                 'from child table
        dgvc3.DisplayMember = "CODE"             'from child table
        dgvc3.DataPropertyName = "SCHOOL"         'from parent table
        dgvc3.HeaderText = "School"
        dgvc3.Name = "School"
        dgvc3.DisplayIndex = 3
        dgvc3.SortMode = DataGridViewColumnSortMode.Automatic
        DataGridView1.Columns.Add(dgvc3)

        'bind the list of schools for new team entry
        Dim dtv2 As New DataView(DS.Tables("School"))
        dtv2.Sort = "SchoolName"
        cboSchoolAdd.DataSource = dtv2
        cboSchoolAdd.ValueMember = "ID"
        cboSchoolAdd.DisplayMember = "SchoolName"

        'populate the divisions/events cbo for new team entries
        Call MakeUnBoundComboBox("EVENTNAME", "ID", "Select Division", DS.Tables("EVENT"), cboEvent)

        'populate division selector for the main grid; must bind AFTER the main datagrid
        Call MakeUnBoundComboBox("EVENTNAME", "ID", "Show ALL divisions", DS.Tables("EVENT"), cboEntryDivisions)

        'I don't really get what the next 2 chunks of code do, but they end up with a parent/child relationship
        'between the tables

        ' Bind the master data connector to the Customers table.
        masterBindingSource.DataSource = DS
        masterBindingSource.DataMember = "ENTRY"  'table name of master table

        ' Bind the details data connector to the master data connector,
        ' using the DataRelation name to filter the information in the 
        ' details table based on the current row in the master table. 
        detailsBindingSource.DataSource = masterBindingSource
        detailsBindingSource.DataMember = "TeamEntry"  'name of the datarelation

        'These format the columns and binds the data
        DataGridView1.AutoGenerateColumns = False
        DataGridView1.DataSource = masterBindingSource
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        DataGridView2.AutoGenerateColumns = False
        DataGridView2.DataSource = detailsBindingSource
        DataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        'automatically use offline mode; this duplicates code below
        'grpAutoNames.Visible = False
        'grpManualAdd.Location.X.Equals(438)
        'grpManualAdd.Location.Y.Equals(12)
        'grpManualAdd.Refresh()
        'Exit Sub ' exit before

        Dim strdum As String
        strdum = "You are in ONLINE mode. " & Chr(10) & Chr(10)
        strdum &= "If you want to change the spelling of a debater name, you must first manually enable spelling changes by clicking the button in the box to the left. " & Chr(10) & Chr(10)
        strdum &= "If you are adding or dropping a student on the team (NOT just changing the spelling of a student's name), first click on the team below.  Second, delete a competitor from a team using the box to the left.  Third, add the new competitor use the box to the right, and all student changes will be validated against the online database. " & Chr(10) & Chr(10)
        lblOnline.Text = strdum
        DataGridView2.ReadOnly = True : butSpellingChanges.Text = "Spelling changes DISABLED"

        'disable auto names if not in online mode
        If GetTagValue(DS.Tables("TOURN_SETTING"), "Online").Trim.ToUpper = "ALL OFFLINE" Then
            strdum = "You are in OFFLINE mode. " & Chr(10) & Chr(10)
            strdum &= "You can make any changes you wish and the CAT should continue to run just fine." & Chr(10) & Chr(10)
            strdum &= "However, changes made here will NOT be known by the online database, and this will create big problems if you later want to post the pairings, use online ballots, etc." & Chr(10) & Chr(10)
            strdum &= "You can switch to ONLINE mode by returning to the main menu and selecting setup screen #1." & Chr(10) & Chr(10)
            lblOnline.Text = strdum
            grpAutoNames.Visible = False
            grpManualAdd.Location.X.Equals(438)
            grpManualAdd.Location.Y.Equals(12)
            grpManualAdd.Refresh()
            DataGridView2.ReadOnly = False : butSpellingChanges.Text = "Spelling changes ENABLED"
            Exit Sub ' exit before populating student master grid
        End If

        Exit Sub

        'not offline, so load the master student names
        LoadFile(mDS, "TourneyDataMaster")

        'Filter users table to competitors and sort by last name
        Dim dtvStudent_Master As New DataView(mDS.Tables("USER"), "Role like 'COMPETITOR%'", "LASt, First", DataViewRowState.CurrentRows)

        'Add the school column to the auto search grid, but don't display as a combobox
        Dim dgvc4 As New DataGridViewComboBoxColumn
        dgvc4.DataSource = DS.Tables("SCHOOL")          'define the detail table
        dgvc4.ValueMember = "ID"                        'from detail table
        dgvc4.DisplayMember = "CODE"                    'from detail table
        dgvc4.DataPropertyName = "SCHOOL"               'from master/parent table
        dgvc4.Name = "SchoolName"
        dgvc4.HeaderText = "School"
        dgvc4.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing
        dgvc4.ReadOnly = True
        dgvMasterStudents.Columns.Add(dgvc4)

        'enforce referentail integrity of the schools
        Call FixMissingSchools(mDS, DS.Tables("School"))

        'bind the master students info to the grid
        dgvMasterStudents.AutoGenerateColumns = False
        dgvMasterStudents.DataSource = dtvStudent_Master
        dgvMasterStudents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

    End Sub
    Sub TeamCountUpdate()
        'count teams and put on the form header
        Me.Text &= " - " & DS.Tables("ENTRY").Rows.Count.ToString & " total teams entered."
        Dim fdEntries As DataRow() : lblEntryCounts.Text = ""
        For x = 0 To DS.Tables("Event").Rows.Count - 1
            fdEntries = DS.Tables("Entry").Select("Event=" & DS.Tables("Event").Rows(x).Item("ID"))
            lblEntryCounts.Text &= fdEntries.Length & " teams in " & DS.Tables("Event").Rows(x).Item("EventName") & Chr(10)
        Next
    End Sub
    Private Sub frmTeamEntry_Unload(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Disposed
        'save file on page close
        Call SaveFile(DS)
        DS.Dispose()
    End Sub
    Sub LoadDivisions()
        'This is dead code; replaced by "makeDDL" in the modGlobals
        'I'm keeping it here to remember what happened

        ' combobox column datasource
        Dim dtArticles As New DataTable
        dtArticles.Columns.Add("DivisionID", System.Type.GetType("System.String"))
        dtArticles.Columns.Add("DivisionName", System.Type.GetType("System.String"))
        Dim x As Integer
        For x = 0 To DS.Tables("EVENT").Rows.Count - 1
            dtArticles.Rows.Add(New Object() {DS.Tables("EVENT").Rows(x).Item("ID"), DS.Tables("EVENT").Rows(x).Item("EventName")})
        Next x

        'add it to the grid
        Dim dgvcArticle As New DataGridViewComboBoxColumn
        dgvcArticle.DataSource = dtArticles
        dgvcArticle.ValueMember = dtArticles.Columns("DivisionID").ColumnName
        dgvcArticle.DisplayMember = dtArticles.Columns("DivisionName").ColumnName
        dgvcArticle.DataPropertyName = "EVENT"
        dgvcArticle.Name = "DivisionID"
        dgvcArticle.HeaderText = "Division"
        DataGridView1.Columns.Add(dgvcArticle)

    End Sub
    Private Sub butMakeTeamNames_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butMakeTeamNames.Click
        'Updates the FullName field for all entered teams
        Dim x As Integer
        For x = 0 To DS.Tables("Entry").Rows.Count - 1
            DS.Tables("Entry").Rows(x).Item("Fullname") = FullNameMaker(DS, DS.Tables("Entry").Rows(x).Item("ID"), "FULL", 1)
        Next x
    End Sub

    Private Sub butMakeAcro_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butMakeAcro.Click
        'Updates the Code (acronym) field for all entered teams
        Dim x As Integer
        For x = 0 To DS.Tables("Entry").Rows.Count - 1
            DS.Tables("Entry").Rows(x).Item("Code") = FullNameMaker(DS, DS.Tables("Entry").Rows(x).Item("ID"), "ACRO", 1)
        Next x
        Call DupeAcroCheck(DS)
    End Sub
    Sub UpdateNames() Handles DataGridView2.CellEndEdit
        'Processes auto update of Fullname and Code field if the button is checked
        If chkAutoUpdate.Checked = False Then Exit Sub
        Dim currentRow As DataRowView = DirectCast(masterBindingSource.Current, DataRowView)
        currentRow.Item("FullName") = FullNameMaker(DS, currentRow.Item("ID"), "FULL", 1)
        currentRow.Item("Code") = FullNameMaker(DS, currentRow.Item("ID"), "ACRO", 1)
        DS.AcceptChanges()
        'checks all records and fixes and duplicates; not the most efficient, and you might want to change if there's a
        'performance hit
        Call DupeAcroCheck(DS)
    End Sub
    Private Sub dgv_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles DataGridView1.KeyDown
        'Handles deletions; makes sure a row is selected and confirms user really wants to delete
        If e.KeyCode = Keys.Delete Then
            If DataGridView1.SelectedRows.Count = 0 Then
                MsgBox("You are attempting to delete a row with a team entry, but you have only clicked on a specific cell.  Select the entire row by clicking the gray box to the far left of the row and try again.", , "Deleting without selecting row")
                Exit Sub
            End If
            Dim title As String = "Confirm"
            Dim message As String = "Are you sure you want to delete this team?  The Stop Scheduling option is much better if they already have results.  You should only delete teams that won't compete in the tournament at all."
            Dim bt As MessageBoxButtons = MessageBoxButtons.OKCancel
            If MessageBox.Show(message, title, bt) = Windows.Forms.DialogResult.Cancel Then
                e.Handled = True
            Else
                Dim currentRow As DataRowView = DirectCast(masterBindingSource.Current, DataRowView)
                Call DeletePrefsByTeam(currentRow.Item("ID"), DS)
                Call TeamCountUpdate()
            End If
        End If
    End Sub
    Private Sub dgv2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles DataGridView2.KeyDown
        'Handles deletions; makes sure a row is selected and confirms user really wants to delete
        If e.KeyCode = Keys.Delete Then
            If DataGridView2.SelectedRows.Count = 0 Then
                MsgBox("You are attempting to delete a row with a team entry, but you have only clicked on a specific cell.  Select the entire row by clicking the gray box to the far left of the row and try again.", , "Deleting without selecting row")
                Exit Sub
            End If
            Dim title As String = "Confirm"
            Dim message As String = "Are you sure you want to delete this debater from this team?  Please note that this will not update any results that have been entered already."
            Dim bt As MessageBoxButtons = MessageBoxButtons.OKCancel
            If MessageBox.Show(message, title, bt) = Windows.Forms.DialogResult.Cancel Then
                e.Handled = True
            End If
        End If
    End Sub
    Private Sub DeleteDone() Handles DataGridView2.UserDeletedRow
        Call UpdateNames() 'update team names and acronyms if button checked
    End Sub
    Private Sub butInstr_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butInstr.Click
        Dim strInstr As String
        strInstr = "GENERAL NAVIGATION:" & Chr(10) & Chr(10)
        strInstr &= "The large box at the bottom is the team grid; the smaller box in the top left is the debater grid (which will show which debaters are on the team)." & Chr(10) & Chr(10)
        strInstr &= "As soon as you click on a team in the team grid, the debaters will appear in the top-left debater grid.  There's a check box below the debater grid; if that box is checked, any change you make to the debaters will automatically cause the team grid to update." & Chr(10) & Chr(10)
        strInstr &= "You can edit any setting, team name, or team acronym in the lower grid.  However, to change the debaters on the team you MUST use the box in the top-left.  Re-typing the debater names in the lower box WILL NOT change the names of the debaters on the team.  You will ONLY change the team name and acronym as it appears on the pairings." & Chr(10) & Chr(10)
        strInstr &= "Using the buttons on the right, you can automatically generate acronyms and team names based on competitor names and school names entered on the ENTER OR EDIT SCHOOLS page.  It will automatically avoid duplicates." & Chr(10) & Chr(10)
        strInstr &= "It's best to make changes to the debaters and let the team grid update automatically, and use the team grid to enter information about the team (rating, school affiliation, etc.)." & Chr(10) & Chr(10)
        strInstr &= "To delete a team or a debater from a team, select the row and click the delete key on the keyboard." & Chr(10) & Chr(10)
        strInstr &= "THE THING MOST LIKELY TO MESS YOU UP:" & Chr(10) & Chr(10)
        strInstr &= "Changes you make to the debater grid will not cause the team grid to update until you hit tab or enter." & Chr(10) & Chr(10)
        strInstr &= "TO ADD A TEAM:" & Chr(10) & Chr(10)
        strInstr &= "Enter all debater names in the box on the top-right, select a division and school, and click one of the two buttons at the bottom of the grid." & Chr(10) & Chr(10)
        strInstr &= "If you have already entered any judge pref or conflict information, it will be much better to add and drop debaters from the team rather than deleteing the entire team (and hence all the preference information)." & Chr(10) & Chr(10)
        MsgBox(strInstr, , "Page Instructions")
    End Sub
    Function AddTeam(ByVal school As Integer, ByVal TmEvent As Integer) As Integer
        'adds a team to the entry table and returns the ID of that debate
        'create new team
        Dim dr As DataRow
        dr = DS.Tables("ENTRY").NewRow
        dr.Item("CODE") = "New Team"
        dr.Item("FullName") = "New Team"
        dr.Item("Event") = TmEvent
        dr.Item("SCHOOL") = school
        dr.Item("DROPPED") = False
        DS.Tables("ENTRY").Rows.Add(dr)
        DS.AcceptChanges()
        'find the ID value of the newly created team
        AddTeam = dr.Item("ID")
        Call TeamCountUpdate()
    End Function
    Private Sub AddDebaterToTeam(ByVal First As String, ByVal Last As String, ByVal TmEntry As Integer, ByVal school As Integer, ByVal DownLoadRecord As Integer)
        'adds debater to a team
        Dim DR, dr2 As DataRow
        DR = DS.Tables("ENTRY_STUDENT").NewRow
        DR.Item("First") = First
        DR.Item("Last") = Last
        DR.Item("Entry") = TmEntry
        DR.Item("SCHOOL") = school
        If DownLoadRecord = -99 Then DownLoadRecord = GetDownloadRecord(First, Last, school)
        dr2 = DS.Tables("Entry_Student").Rows.Find(DownLoadRecord)
        If Not dr2 Is Nothing Then
            MsgBox("That student is already entered in the tournament; please delete the entry and try again.")
            Exit Sub
        End If
        DR.Item("DOWNLOADRECORD") = DownLoadRecord
        If DownLoadRecord > 0 Then
            DS.EnforceConstraints = False
            DR.Item("ID") = DownLoadRecord
        End If
        DS.Tables("ENTRY_STUDENT").Rows.Add(DR)
        DS.Tables("ENTRY_STUDENT").AcceptChanges()
        DS.EnforceConstraints = True
    End Sub
    Function GetDownloadRecord(ByVal First As String, ByVal Last As String, ByVal school As Integer)
        GetDownloadRecord = -99
        Dim URL As String = "https://www.tabroom.com/api/student_name.mhtml?username=jbruschke@fullerton.edu&password=123Eadie&firstname=" & First & "&lastname=" & Last & "&school_id=" & school.ToString
        Dim request As HttpWebRequest = WebRequest.Create(URL)
        Dim response As HttpWebResponse = request.GetResponse()
        Dim reader As StreamReader = New StreamReader(response.GetResponseStream())
        Dim str As String
        str = reader.ReadToEnd
        reader.Close() : response.Close()
        GetDownloadRecord = Val(str)
    End Function
    Private Sub butAddDebater_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butAddDebater.Click
        'add debater from manual typed in name grid to selected team

        'validate that a division has been checked
        If cboEvent.SelectedValue = -1 Then MsgBox("Please select a division from the box above and try again.") : Exit Sub
        'find the ID value of the newly created team
        Dim currentRow As DataRowView = DirectCast(masterBindingSource.Current, DataRowView)
        'add the student to the team
        Dim x As Integer
        For x = 0 To DataGridView3.Rows.Count - 2
            Call AddDebaterToTeam(DataGridView3.Rows(x).Cells("First").Value.ToString, DataGridView3.Rows(x).Cells("Last").Value.ToString, currentRow.Item("ID"), cboSchoolAdd.SelectedValue, -99)
        Next x
        'build names and acronyms
        Call UpdateNames() 'update team names and acronyms if button checked

    End Sub
    Private Sub butManualCreateTeam_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butManualCreateTeam.Click
        'validate that a division has been checked
        If cboEvent.SelectedValue = -1 Then MsgBox("Please select a division from the box above and try again.") : Exit Sub
        'create a new team and get their ID
        Dim TeamID As Integer = AddTeam(cboSchoolAdd.SelectedValue, cboEvent.SelectedValue)
        'add the student to the team; function will also update team names and acronyms
        Dim x As Integer
        For x = 0 To DataGridView3.Rows.Count - 2
            Call AddDebaterToTeam(DataGridView3.Rows(x).Cells("First").Value.ToString, DataGridView3.Rows(x).Cells("Last").Value.ToString, TeamID, cboSchoolAdd.SelectedValue, -99)
        Next x
        'now select the new row
        masterBindingSource.Position = masterBindingSource.Find("ID", TeamID)
        'build names and acronyms
        Call UpdateNames() 'update team names and acronyms if button checked
        'reselect in case it got resorted
        masterBindingSource.Position = masterBindingSource.Find("ID", TeamID)
    End Sub
    Private Sub doSearch() Handles txtSearch.TextChanged
        Dim x As Integer
        For x = 0 To dgvMasterStudents.Rows.Count - 1
            If Mid(dgvMasterStudents.Rows(x).Cells("MS_Last").Value.ToString, 1, txtSearch.Text.Length).ToUpper = txtSearch.Text.ToUpper Then
                dgvMasterStudents.Rows(x).Selected = True
                dgvMasterStudents.CurrentCell = dgvMasterStudents(0, x)
                dgvMasterStudents.FirstDisplayedScrollingRowIndex = x
                Exit Sub
            End If
        Next x
    End Sub
    Private Sub butMakeTeamFromMaster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butMakeTeamFromMaster.Click
        'create a new team and get their ID
        Dim TeamID As Integer = AddTeam(cboSchoolAdd.SelectedValue, cboEvent.SelectedValue)
        'get the currentrow of the existing debater datagrid
        Dim currentTeamRow As DataRowView = DirectCast(masterBindingSource.Current, DataRowView)
        'add the student to the team; function will also update team names and acronyms
        Call AddDebaterToTeam(dgvMasterStudents.CurrentRow.Cells("First").Value.ToString, dgvMasterStudents.CurrentRow.Cells("Last").Value.ToString, currentTeamRow.Item("ID"), dgvMasterStudents.CurrentRow.Cells("School").Value, dgvMasterStudents.CurrentRow.Cells("ID").Value)
        'now select the new row
        masterBindingSource.Position = masterBindingSource.Find("ID", TeamID)
        'build names and acronyms
        Call UpdateNames() 'update team names and acronyms if button checked
        'reselect in case it got resorted
        masterBindingSource.Position = masterBindingSource.Find("ID", TeamID)
    End Sub

    Private Sub butAddMasterToTeam_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butAddMasterToTeam.Click
        Dim currentTeamRow As DataRowView = DirectCast(masterBindingSource.Current, DataRowView)
        'add the student to the team
        Call AddDebaterToTeam(dgvMasterStudents.CurrentRow.Cells("MS_First").Value.ToString, dgvMasterStudents.CurrentRow.Cells("MS_Last").Value.ToString, currentTeamRow.Item("ID"), dgvMasterStudents.CurrentRow.Cells("School").Value, dgvMasterStudents.CurrentRow.Cells("ID").Value)
        'build names and acronyms
        Call UpdateNames() 'update team names and acronyms if button checked
    End Sub

    Private Sub butPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butPrint.Click
        Dim defCols(DataGridView1.Columns.Count) As Boolean
        defCols(0) = True : defCols(6) = True
        Dim frm As New frmPrint(DataGridView1, DS.Tables("Tourn").Rows(0).Item("TournName").trim & Chr(13) & Chr(10) & "TEAMS ENTERED" & Chr(13), defCols)
        frm.ShowDialog()
    End Sub

    Private Sub butTeamSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butTeamSearch.Click
        'mark the current row
        Dim startrow As Integer = DataGridView1.CurrentRow.Index

        'search from current row down
        If startrow > 0 Then startrow += 1
        Dim x As Integer
        For x = startrow To DataGridView1.Rows.Count - 1
            If InStr(DataGridView1.Rows(x).Cells("FullName").Value.ToString.ToUpper.Trim, txtTeamSearch.Text.ToUpper.Trim) > 0 Then
                DataGridView1.Rows(x).Selected = True
                DataGridView1.CurrentCell = DataGridView1(0, x)
                DataGridView1.FirstDisplayedScrollingRowIndex = x
                Exit Sub
            End If
        Next x
        'exit if started at top
        If startrow = 0 Then Exit Sub
        'if not, go back to the start and search down to where you started
        For x = 0 To startrow
            If InStr(DataGridView1.Rows(x).Cells("FullName").Value.ToString.ToUpper.Trim, txtTeamSearch.Text.ToUpper.Trim) > 0 Then
                DataGridView1.Rows(x).Selected = True
                DataGridView1.CurrentCell = DataGridView1(0, x)
                DataGridView1.FirstDisplayedScrollingRowIndex = x
                Exit Sub
            End If
        Next x
    End Sub
    Private Sub datagridview2_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles DataGridView2.DragDrop
        Call butAddMasterToTeam_Click(sender, e)
    End Sub
    Private Sub dgvMasterStudents_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dgvMasterStudents.MouseDown
        Dim hit As DataGridView.HitTestInfo = dgvMasterStudents.HitTest(e.X, e.Y)
        dgvMasterStudents.DoDragDrop(dgvMasterStudents.Rows(hit.RowIndex), DragDropEffects.Copy)
    End Sub
    Private Sub DataGridView2_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles DataGridView2.DragEnter
        e.Effect = DragDropEffects.Copy
    End Sub
    Private Sub DivChange() Handles cboEntryDivisions.SelectedIndexChanged
        'exit if loading and datagridview1 is empty
        If DataGridView1.Rows.Count = 0 Then Exit Sub
        'if select all, use an all-inclusive filter and exit
        If cboEntryDivisions.SelectedValue = -1 Then masterBindingSource.Filter = "Event>-1" : Exit Sub
        'otherwise, filter by the selected division
        masterBindingSource.Filter = "Event=" & cboEntryDivisions.SelectedValue.ToString
        Call TeamCountUpdate()
    End Sub
    Private Sub DataGridView2_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView2.CellValueChanged
        MsgBox("If you are dropping one debater and adding another, please DELETE the competitor name entirely and add a new one.  If you are simply changing the spelling of a name, make the name change here.")
    End Sub
    Private Sub dataGridView1_CellClick(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) Handles DataGridView1.CellValueChanged
        If e.ColumnIndex = -1 Then Exit Sub
        'checks for clicks on non-editable columns
        Dim strColName As String = DataGridView1.Columns(e.ColumnIndex).Name
        If strColName = "CODE" Or strColName = "FULLNAME" Then
            MsgBox("Making changes here will ONLY change the display name of the team, not the individual competitors. To change competitors (or name spellings), use the box at the top left and this will automatically update this grid.  Unless you REALLY need to change the team name and the names of individual debaters, you should NOT make edits in this box.")
        End If
    End Sub

    Private Sub butSpellingChanges_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butSpellingChanges.Click
        If butSpellingChanges.Text = "Spelling changes ENABLED" Then
            butSpellingChanges.Text = "Spelling changes DISABLED"
            DataGridView2.ReadOnly = True
        Else
            butSpellingChanges.Text = "Spelling changes ENABLED"
            DataGridView2.ReadOnly = False
        End If
    End Sub
End Class