Public Class frmJudgeEntry
    Dim ds As New DataSet
    Dim AlreadyLoaded As Boolean
    Private Sub frmJudgeEntry_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        LoadFile(ds, "TourneyData")
        'If JudgeSchoolCheck() = False Then Exit Sub
        Call UnaffiliatedCheck(ds) 'makes sure all the unaffiliated judges have a school record
        Call CheckMasterFile(ds) 'check to see if masterfile is there; move to offline mode if it isn't
        Call InitializeJudges(ds)
        Call NullKiller(ds, "JUDGE")
        'add a rounds heard column; will delete on form close
        ds.Tables("Judge").Columns.Add("Already", System.Type.GetType("System.Int64"))
        For x = 0 To ds.Tables("Judge").Rows.Count - 1
            ds.Tables("Judge").Rows(x).Item("Already") = GetRoundsJudged(ds, ds.Tables("Judge").Rows(x).Item("ID"))
        Next x

        'populate elim cbo
        cboEvent.DataSource = ds.Tables("Event")
        cboEvent.DisplayMember = "EventName"
        cboEvent.ValueMember = "ID"

        'sort schools
        ds.Tables("School").DefaultView.Sort = "CODE"

        'Add Columns
        'ID
        Dim dgv As New DataGridViewTextBoxColumn
        dgv.DataPropertyName = "ID" : dgv.HeaderText = "ID" : dgv.Name = "ID" : dgv.Visible = False
        DataGridView1.Columns.Add(dgv)
        'Last name
        dgv = New DataGridViewTextBoxColumn
        dgv.DataPropertyName = "Last" : dgv.HeaderText = "Last Name" : dgv.Name = "Last"
        DataGridView1.Columns.Add(dgv)
        'First name
        dgv = New DataGridViewTextBoxColumn
        dgv.DataPropertyName = "First" : dgv.HeaderText = "First Name" : dgv.Name = "First"
        DataGridView1.Columns.Add(dgv)
        'stop scheduling
        Dim dgv3 As DataGridViewCheckBoxColumn
        dgv3 = New DataGridViewCheckBoxColumn
        dgv3.DataPropertyName = "StopScheduling"
        dgv3.Name = "StopScheduling"
        dgv3.HeaderText = "StopScheduling"
        DataGridView1.Columns.Add(dgv3)
        'mobility/disability 
        dgv3 = New DataGridViewCheckBoxColumn
        dgv3.DataPropertyName = "ADA"
        dgv3.Name = "ADA"
        dgv3.HeaderText = "Disability"
        DataGridView1.Columns.Add(dgv3)

        'School as combobox
        Dim dgvc2 As New DataGridViewComboBoxColumn
        dgvc2.DataSource = ds.Tables("School").DefaultView
        dgvc2.ValueMember = "ID"
        dgvc2.DisplayMember = "CODE"
        dgvc2.DataPropertyName = "SCHOOL"
        dgvc2.HeaderText = "SCHOOL"
        dgvc2.Name = "SCHOOL"
        DataGridView1.Columns.Add(dgvc2)

        'Obligation
        dgv = New DataGridViewTextBoxColumn
        dgv.DataPropertyName = "Obligation" : dgv.HeaderText = "Obligation" : dgv.Name = "Obligation"
        DataGridView1.Columns.Add(dgv)
        'Hired
        dgv = New DataGridViewTextBoxColumn
        dgv.DataPropertyName = "Hired" : dgv.HeaderText = "Hired" : dgv.Name = "Hired"
        DataGridView1.Columns.Add(dgv)
        'Heard Already
        dgv = New DataGridViewTextBoxColumn
        dgv.DataPropertyName = "Already" : dgv.HeaderText = "Judged" : dgv.Name = "Already"
        DataGridView1.Columns.Add(dgv)
        'Rating
        dgv = New DataGridViewTextBoxColumn
        dgv.DataPropertyName = "TabRating" : dgv.HeaderText = "Rating" : dgv.Name = "TabRating"
        DataGridView1.Columns.Add(dgv)

        'Divisions
        For x = 0 To ds.Tables("Event").Rows.Count - 1
            dgv3 = New DataGridViewCheckBoxColumn
            dgv3.DataPropertyName = "Event" & ds.Tables("Event").Rows(x).Item("ID")
            dgv3.Name = "Event" & ds.Tables("Event").Rows(x).Item("ID")
            dgv3.HeaderText = ds.Tables("Event").Rows(x).Item("Abbr")
            DataGridView1.Columns.Add(dgv3)
        Next x
        'timeslots
        Dim fdRd As DataRow()
        For x = 0 To ds.Tables("Timeslot").Rows.Count - 1
            fdRd = ds.Tables("Round").Select("Timeslot=" & ds.Tables("Timeslot").Rows(x).Item("ID"))
            If fdRd.Length > 0 Then
                dgv3 = New DataGridViewCheckBoxColumn
                dgv3.DataPropertyName = "TimeSlot" & ds.Tables("TimeSlot").Rows(x).Item("ID")
                dgv3.Name = "TimeSlot" & ds.Tables("TimeSlot").Rows(x).Item("ID")
                dgv3.HeaderText = "Time" & ds.Tables("TimeSlot").Rows(x).Item("ID")
                dgv3.HeaderText = "Time" & ds.Tables("TimeSlot").Rows(x).Item("TimeSlotName")
                If fdRd(0).Item("Rd_Name") >= 10 Then dgv3.Visible = False
                DataGridView1.Columns.Add(dgv3)
            End If
        Next x
        'Notes
        dgv = New DataGridViewTextBoxColumn
        dgv.DataPropertyName = "Notes" : dgv.HeaderText = "Notes" : dgv.Name = "Notes"
        DataGridView1.Columns.Add(dgv)

        'bind it
        DataGridView1.AutoGenerateColumns = False
        ds.Tables("Judge").DefaultView.Sort = "Last"
        DataGridView1.DataSource = ds.Tables("Judge")
        DataGridView1.Columns("First").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        DataGridView1.Columns("Last").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        DataGridView1.Columns("School").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        Call JudgeSituationUpdate()
        Me.Text &= " - " & ds.Tables("JUDGE").Rows.Count.ToString & " total judges entered."
    End Sub
    Sub JudgeSituationUpdate()
        'Dim strTime As String
        'strTime = "Start: " & Now.Second & " " & Now.Millisecond & Chr(10) & Chr(10)
        Dim dt As New DataTable
        dt = JudgeSituation(ds)
        DataGridView2.DataSource = dt
        'strTime &= "End: " & Now.Second & " " & Now.Millisecond & Chr(10) & Chr(10)
        'MsgBox(strTime)
        For x = 0 To DataGridView2.Rows.Count - 1
            If DataGridView2.Rows(x).Cells("TotalBalance").Value < 0 Then
                DataGridView2.Item(DataGridView2.ColumnCount - 1, x).Style.BackColor = Color.Red
            End If
        Next
    End Sub
    Private Sub frmJudgeEntry_Unload(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Disposed
        'Delete temporary column
        If ds.Tables("Judge").Columns.Contains("Already") = True Then
            ds.Tables("Judge").Columns.Remove("Already")
        End If

        Call SaveFile(ds)
        ds.Dispose()
    End Sub

    Private Sub butSchoolSort_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butSchoolSort.Click
        ds.Tables("Judge").DefaultView.Sort = "School ASC"
    End Sub

    Private Sub butMarkTrue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butMarkTrue.Click
        Call DoMarker(True)
    End Sub
    Sub DoMarker(ByVal defVal As Boolean)
        If DataGridView1.Columns(DataGridView1.CurrentCell.ColumnIndex).CellType.Name <> "DataGridViewCheckBoxCell" Then
            MsgBox("This function only works for check box columns.  Click OK to exit.", MsgBoxStyle.OkOnly)
            Exit Sub
        End If
        For x = 0 To DataGridView1.ColumnCount - 1
            If DataGridView1.CurrentCell.ColumnIndex = x Then
                For y = 0 To ds.Tables("Judge").Rows.Count - 1
                    ds.Tables("Judge").Rows(y).Item(DataGridView1.Columns(x).Name) = defVal
                Next y
            End If
        Next x
        Call JudgeSituationUpdate()
    End Sub

    Private Sub butMarkFalse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butMarkFalse.Click
        Call DoMarker(False)
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        For x = 0 To DataGridView1.ColumnCount - 1
            If DataGridView1.Columns(x).CellType.Name = "DataGridViewCheckBoxCell" Then
                For y = 0 To DataGridView1.RowCount - 1
                    DataGridView1.Rows(y).Cells(x).Value = True
                Next y
            End If
        Next x
        Call JudgeSituationUpdate()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim drRd As DataRow
        For x = ds.Tables("Panel").Rows.Count - 1 To 0 Step -1
            drRd = ds.Tables("Round").Rows.Find(ds.Tables("Panel").Rows(x).Item("Round"))
            If drRd.Item("Rd_Name") >= 8 Then ds.Tables("Panel").Rows(x).Delete()
        Next
    End Sub
    Sub thing() Handles DataGridView1.DataBindingComplete
        'AlreadyLoaded = True
    End Sub
    Sub Updater() Handles DataGridView1.RowLeave
        'If AlreadyLoaded = False Then Exit Sub
        'Call JudgeSituationUpdate()
    End Sub
    Function JudgeSchoolCheck() As Boolean
        JudgeSchoolCheck = True
        Dim dr As DataRow
        For x = 0 To ds.Tables("Judge").Rows.Count - 1
            dr = ds.Tables("School").Rows.Find(ds.Tables("Judge").Rows(x).Item("School"))
            If dr Is Nothing Then
                MsgBox("No school record for " & ds.Tables("Judge").Rows(x).Item("Last"))
                JudgeSchoolCheck = False
            End If
        Next x
    End Function

    Private Sub butToggleTimeslots_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butToggleTimeslots.Click
        Dim fdRd As DataRow()
        'reset all to not visible
        If butToggleTimeslots.Text = "Show Elim Timeslots" Then
            butToggleTimeslots.Text = "Show Prelim Timeslots"
        Else
            butToggleTimeslots.Text = "Show Elim Timeslots"
        End If
        For x = 0 To ds.Tables("Timeslot").Rows.Count - 1
            fdRd = ds.Tables("Round").Select("Timeslot=" & ds.Tables("Timeslot").Rows(x).Item("ID"))
            If fdRd.Length > 0 Then
                DataGridView1.Columns("TimeSlot" & ds.Tables("TimeSlot").Rows(x).Item("ID")).visible = False
                If fdRd(0).Item("Rd_Name") > 9 And butToggleTimeslots.Text = "Show Prelim Timeslots" Then
                    DataGridView1.Columns("TimeSlot" & ds.Tables("TimeSlot").Rows(x).Item("ID")).visible = True
                End If
                If fdRd(0).Item("Rd_Name") < 9 And butToggleTimeslots.Text = "Show Elim Timeslots" Then
                    DataGridView1.Columns("TimeSlot" & ds.Tables("TimeSlot").Rows(x).Item("ID")).visible = True
                End If
            End If
        Next x
    End Sub
    Private Sub datagridview1_DefaultValuesNeeded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles DataGridView1.DefaultValuesNeeded
        For x = 0 To e.Row.Cells.Count - 1
            If e.Row.Cells(x).GetType.ToString = "System.Windows.Forms.DataGridViewCheckBoxCell" Then
                If DataGridView1.Columns(x).Name.ToUpper = "STOPSCHEDULING" Then
                    e.Row.Cells(x).Value = False
                Else
                    e.Row.Cells(x).Value = True
                End If
            End If
            If e.Row.Cells(x).GetType.ToString = "System.Windows.Forms.DataGridViewComboBoxCell" Then
                e.Row.Cells(x).Value = -1
            End If
            If e.Row.Cells(x).GetType.ToString = "System.Windows.Forms.DataGridViewTextBoxCell" Then
                If e.Row.Cells(x).OwningColumn.Name = "First" Then e.Row.Cells(x).Value = "X"
                If e.Row.Cells(x).OwningColumn.Name = "Last" Then e.Row.Cells(x).Value = "X"
                If e.Row.Cells(x).OwningColumn.Name = "Obligation" Then e.Row.Cells(x).Value = 6
                If e.Row.Cells(x).OwningColumn.Name = "Hired" Then e.Row.Cells(x).Value = 0
            End If
        Next x
        'e.Row.Cells("First").Value = "X"
    End Sub
    Sub WhatTheHell(ByVal sender As Object, ByVal e As DataGridViewDataErrorEventArgs) Handles DataGridView1.DataError
        If DataGridView1.CurrentRow.Cells("First").Value Is System.DBNull.Value Then DataGridView1.CurrentRow.Cells("First").Value = "X"
        For x = 0 To DataGridView1.Columns.Count - 1
            If DataGridView1.Columns(x).GetType.ToString = "System.Windows.Forms.DataGridViewCheckBoxColumn" Then
                If DataGridView1.CurrentRow.Cells(x).Value.ToString = "" Then DataGridView1.CurrentRow.Cells(x).Value = "False"
                DataGridView1.CurrentRow.Cells(x).Value = Convert.ToBoolean(DataGridView1.CurrentRow.Cells(x).Value)
            End If
        Next x
    End Sub

    Private Sub butJudgeSitutionUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butJudgeSitutionUpdate.Click
        Call JudgeSituationUpdate()
    End Sub
    Sub AutoUpdate() Handles DataGridView1.CellEndEdit
        If DataGridView1.Columns(DataGridView1.CurrentCell.ColumnIndex).Name = "Obligation" Then
            Call JudgeSituationUpdate()
        ElseIf DataGridView1.Columns(DataGridView1.CurrentCell.ColumnIndex).Name = "Hired" Then
            Call JudgeSituationUpdate()
        End If
    End Sub
    Private Sub butAllTSToggle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butAllTSToggle.Click
        Dim Hide As Boolean
        'reset all to not visible
        If butAllTSToggle.Text = "Hide Timeslots" Then
            butAllTSToggle.Text = "Show Timeslots"
            Hide = True
        Else
            butAllTSToggle.Text = "Hide Timeslots"
        End If
        For x = 0 To DataGridView1.ColumnCount - 1
            If InStr(DataGridView1.Columns(x).Name.ToString.ToUpper, "TIME") > 0 Then
                If Hide = False Then DataGridView1.Columns(x).Visible = True Else DataGridView1.Columns(x).Visible = False
            End If
        Next x
    End Sub

    Private Sub butAvailByRound_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butAvailByRound.Click
        Dim frm As New frmJudgesByRoundPopup
        frm.ShowDialog()
    End Sub

    Private Sub butPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butPrint.Click
        MsgBox("By default, all displayed columns will print.  You may wish to hide the timeslots.  You can further customize the printout on the next screen.")
        Dim defCols(DataGridView1.Columns.Count) As Boolean
        For x = 0 To DataGridView1.ColumnCount - 1
            If DataGridView1.Columns(x).Visible = True Then defCols(x) = True
        Next x
        Dim frm As New frmPrint(DataGridView1, ds.Tables("TOURN").Rows(0).Item("TournName") & Chr(10) & "Judges in Attendance", defCols)
        frm.ShowDialog()
    End Sub

    Private Sub butInstr_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butInstr.Click
        Dim strInstr As String
        strInstr = "GENERAL SCHEME:" & Chr(10) & Chr(10)
        strInstr &= "This screen allows you to show either prelim or elim timeslots by clicking the button on the top-left.  You can also hide all of the timeslots.  Using these options will help you keep the screen from becoming too croweded." & Chr(10) & Chr(10)
        strInstr &= "The lower-right box will show you the overall judging circumstances for the tournament.  Clicking the SHOW AVAILABILITY BY ROUND button will pop up a screen that will display the judge situation for all specific rounds." & Chr(10) & Chr(10)
        strInstr &= "You can change the values for any judge simply type the new value; remember that for drop-down lists you'll need to select a new value and then hit tab or enter." & Chr(10) & Chr(10)
        strInstr &= "Buttons in the bottom-left allow deletion and can apply changes to all judges." & Chr(10) & Chr(10)
        MsgBox(strInstr, , "Page Instructions")
    End Sub

    Private Sub butInNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butInNow.Click
        Call MarkByTimeSlot("CURRENT")
    End Sub
    Function GetTimeslot() As Integer
        GetTimeslot = 0
        Dim strTS = DataGridView1.Columns(DataGridView1.CurrentCell.ColumnIndex).Name
        If InStr(strTS.ToUpper, "TIMESLOT") = 0 Then
            MsgBox("Please click on a column that is a timeslot and try again.")
            Exit Function
        End If
        strTS = Replace(strTS, "TimeSlot", "")
        GetTimeslot = Val(strTS)
        If GetTimeslot = 0 Then
            MsgBox("Unable to find value for timeslot.")
            Exit Function
        End If
        Dim fdrd As DataRow()
        fdrd = ds.Tables("Round").Select("Timeslot=" & GetTimeslot)
        Dim isElim As Boolean = False
        For x = 0 To fdrd.Length - 1
            If fdrd(x).Item("Rd_Name") > 9 Then isElim = True
        Next x
        If isElim = False Then
            MsgBox("Warning: This function is usually used for elims, and there are no elims in this timeslot")
        End If
    End Function
    Sub MarkByTimeSlot(ByVal strMarkType As String)
        'pull the timeslot
        Dim ts As Integer = GetTimeslot()
        If ts = 0 Then Exit Sub
        Dim drRd As DataRow()
        'test the event/timeslot combos work
        drRd = ds.Tables("Round").Select("Event=" & cboEvent.SelectedValue & " and TimeSlot=" & ts)
        If drRd.Length = 0 Then
            MsgBox("Can't find a round for this event in this timeslot")
            Exit Sub
        ElseIf drRd.Length > 1 Then
            MsgBox("Found more than 1 round for this event in this timeslot; visit the ROUNDS setup screen (step 4) to fix.")
            Exit Sub
        End If
        'get the round
        Dim round As Integer
        If strMarkType = "PRIOR" Then
            round = GetPriorRound(ds, drRd(0).Item("ID"))
        Else
            round = drRd(0).Item("ID")
        End If
        'now pull the teams in the round
        Dim dt As New DataTable
        dt.Columns.Add("School", System.Type.GetType("System.Int64"))
        dt.Constraints.Add("PrimaryKey", dt.Columns(0), True)
        Dim fdBallots As DataRow()
        fdBallots = ds.Tables("Ballot").Select(BuildPanelStringByRound(ds, round))
        Dim dr, fdTeam, fdInTable As DataRow
        For x = 0 To fdBallots.Length - 1
            fdTeam = ds.Tables("Entry").Rows.Find(fdBallots(x).Item("Entry"))
            fdInTable = dt.Rows.Find(fdTeam.Item("School"))
            If fdInTable Is Nothing Then
                dr = dt.NewRow
                dr.Item("School") = fdTeam.Item("School")
                dt.Rows.Add(dr)
            End If
        Next x
        'mark judges
        For x = 0 To DataGridView1.Rows.Count - 1
            fdInTable = dt.Rows.Find(DataGridView1.Rows(x).Cells("School").Value)
            If Not fdInTable Is Nothing Then
                DataGridView1.Rows(x).Cells("TimeSlot" & ts).Value = True
            End If
        Next
    End Sub

    Private Sub butInLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butInLast.Click
        Call MarkByTimeSlot("PRIOR")
    End Sub

    Private Sub butAvailByElimTimeSlot_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butAvailByElimTimeSlot.Click
        'pull the timeslot
        Dim ts As Integer = GetTimeslot()
        If ts = 0 Then Exit Sub
        Dim ctr As Integer = 0
        For x = 0 To DataGridView1.Rows.Count - 1
            If DataGridView1.Rows(x).Cells("TimeSlot" & ts).Value Is System.DBNull.Value Then DataGridView1.Rows(x).Cells("TimeSlot" & ts).Value = False
            If DataGridView1.Rows(x).Cells("TimeSlot" & ts).Value = "" Then DataGridView1.Rows(x).Cells("TimeSlot" & ts).Value = False
            If DataGridView1.Rows(x).Cells("TimeSlot" & ts).Value = True And DataGridView1.Rows(x).Cells("StopScheduling").Value = False Then ctr += 1
        Next
        MsgBox(ctr & " judges available for " & DataGridView1.Columns(DataGridView1.CurrentCell.ColumnIndex).HeaderText)
    End Sub
End Class