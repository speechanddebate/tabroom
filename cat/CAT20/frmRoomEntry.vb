Imports System.IO

Public Class frmRoomEntry
    Dim ds As New DataSet

    Private Sub frmRoomEntry_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LoadFile(ds, "TourneyData")
        Call InitializeRooms(ds)
        Call NullKiller(ds, "ROOM")

        'Put columns in the right order
        'ID
        Dim dgv As New DataGridViewTextBoxColumn
        dgv.DataPropertyName = "ID" : dgv.HeaderText = "ID" : dgv.Name = "ID" : dgv.Visible = False
        DataGridView1.Columns.Add(dgv)
        'Room name
        dgv = New DataGridViewTextBoxColumn
        dgv.DataPropertyName = "RoomName" : dgv.HeaderText = "Room" : dgv.Name = "RoomName"
        DataGridView1.Columns.Add(dgv)
        'BUilding
        dgv = New DataGridViewTextBoxColumn
        dgv.DataPropertyName = "Building" : dgv.HeaderText = "Building" : dgv.Name = "Building"
        DataGridView1.Columns.Add(dgv)
        'Quality
        dgv = New DataGridViewTextBoxColumn
        dgv.DataPropertyName = "Quality" : dgv.HeaderText = "Quality" : dgv.Name = "Quality"
        DataGridView1.Columns.Add(dgv)
        'Capacity
        dgv = New DataGridViewTextBoxColumn
        dgv.DataPropertyName = "Capacity" : dgv.HeaderText = "Capacity" : dgv.Name = "Capacity"
        DataGridView1.Columns.Add(dgv)
        'Inactive
        Dim dgv3 As New DataGridViewCheckBoxColumn
        dgv3.DataPropertyName = "Inactive"
        dgv3.Name = "Inactive"
        dgv3.HeaderText = "Stop scheduling"
        DataGridView1.Columns.Add(dgv3)
        
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
                If fdRd(0).Item("Rd_Name") > 9 Then dgv3.Visible = False
                'dgv3.HeaderText = "Time" & ds.Tables("TimeSlot").Rows(x).Item("ID")
                dgv3.HeaderText = "Time" & ds.Tables("TimeSlot").Rows(x).Item("TimeSlotName")
                DataGridView1.Columns.Add(dgv3)
            End If
        Next x

        'Notes
        dgv = New DataGridViewTextBoxColumn
        dgv.DataPropertyName = "Notes" : dgv.HeaderText = "Notes" : dgv.Name = "Notes"
        DataGridView1.Columns.Add(dgv)

        DataGridView1.AutoGenerateColumns = False
        DataGridView1.DataSource = ds.Tables("Room")
        'DataGridView1.Columns("Name").Width = 100
        DataGridView1.Columns("RoomName").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        Call showRoomStatus()
    End Sub
    Private Sub frmRoomEntry_Unload(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Disposed
        Call SaveFile(ds)
        ds.Dispose()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim filename, room As String
        Dim Bldg(99) As String
        Dim nBldg, x As Integer
        OpenFileDialog1.InitialDirectory = "c:\desktop"
        OpenFileDialog1.ShowDialog()
        filename = OpenFileDialog1.FileName
        Dim st As StreamReader = File.OpenText(filename)
        Dim row As DataRow
        While st.Peek() > -1
            room = st.ReadLine()
            row = ds.Tables("Room").NewRow
            If InStr(room, ",") = 0 Then row.Item("RoomName") = room
            If InStr(room, ",") > 0 Then row.Item("RoomName") = Mid(room, 1, InStr(room, ",") - 1)
            For x = 0 To nBldg
                If Mid(room, 1, 2).ToUpper = Bldg(x) Then row.Item("Building") = x
            Next x
            If row.Item("Building") Is System.DBNull.Value Then
                nBldg += 1 : Bldg(nBldg) = Mid(room, 1, 2)
                row.Item("Building") = nBldg
            End If
            If InStr(room, ",") > 0 Then
                row.Item("DivFor") = Val(Mid(room, InStr(room, ",") + 1, room.Length - InStr(room, ",")))
            End If
            row.Item("Quality") = 1 : row.Item("Capacity") = 25 : row.Item("Inactive") = False : row.Item("Notes") = ""
            ds.Tables("Room").Rows.Add(row)
        End While
        st.Close()
    End Sub

    Private Sub butBasicInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butBasicInfo.Click
        Dim strInfo As String = ""
        strInfo &= "To load rooms from a text file, make sure the file has one room per line.  To indicate a division, add a comma to the end of the line and a division designation that matches the divisions shown on this page.  For example, you could enter ""MH 121"" on a line and it would load the room for the default division, or ""MH 121, 2"" and it would load the room for division 2." & Chr(10) & Chr(10)
        MsgBox(strInfo)
    End Sub
    Private Sub dataGridView1_DefaultValuesNeeded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles DataGridView1.DefaultValuesNeeded
        For x = 0 To e.Row.Cells.Count - 1
            'If e.Row.Cells("Inactive").Value Is Nothing Then e.Row.Cells(x).Value = False
            'If e.Row.Cells("Inactive").Value Is System.DBNull.Value Then e.Row.Cells(x).Value = False
            If InStr(e.Row.Cells(x).OwningColumn.Name.ToString.ToUpper, "EVENT") > 0 Then e.Row.Cells(x).Value = True
            If InStr(e.Row.Cells(x).OwningColumn.Name.ToString.ToUpper, "TIME") > 0 Then e.Row.Cells(x).Value = True
        Next
        With e.Row
            .Cells("Quality").Value = 1
            .Cells("Capacity").Value = 25
            .Cells("Inactive").Value = False
            .Cells("Notes").Value = "-"
        End With
    End Sub
    Sub eHandle(ByVal sender As Object, ByVal e As DataGridViewDataErrorEventArgs) Handles DataGridView1.DataError
        If InStr(e.Exception.ToString, "not a valid value for Boolean") > 0 Then DataGridView1.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = False : Exit Sub
        MsgBox(e.Exception.ToString)
    End Sub
    Sub ShowRoomStatus()
        'shows prelim round status

        Dim strTime As String = ""
        strTime &= "Start: " & Now.Second & " " & Now.Millisecond & Chr(10) & Chr(10)

        Dim dt As New DataTable
        dt = MakeRoomStatusTable(ds)

        DataGridView2.DataSource = dt
        'DataGridView2.Columns("TimeSlot").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells

        For x = 0 To DataGridView2.Rows.Count - 1
            If DataGridView2.Rows(x).Cells("Balance").Value < 0 Then
                DataGridView2.Item("Balance", x).Style.BackColor = Color.Red
            End If
        Next

        strTime &= "End: " & Now.Second & " " & Now.Millisecond & Chr(10) & Chr(10)
        'MsgBox(strTime)

    End Sub

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

    Private Sub butHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butHelp.Click
        Dim strInfo As String = ""
        strInfo &= "To load the rooms in from a text file, click the 'Room File Information' button below." & Chr(10) & Chr(10)
        strInfo &= "You can edit any existing room entry simply by editing the information in the grid to the left. You can add a room entry by typing on the last row of the grid (the one with the aserisk to the left)." & Chr(10) & Chr(10)
        strInfo &= "Note that there are not separate prelim and elim screens, but you can toggle between the prelim and elim round timeslots by clicking the button to the left." & Chr(10) & Chr(10)
        strInfo &= "Building numbers are arbitrary and assigned by you; just use the same number for the same building." & Chr(10) & Chr(10)
        strInfo &= "Quality numbers are assigned by you; give lower numbers to better rooms.  Ties are fine; a good rule of thumb is to use a 1-3 ranking system.  For any round where rooms are set by bracket (such as elimination rounds), the rooms with the lowest quality number will be assigned to the highest seeds." & Chr(10) & Chr(10)
        strInfo &= "Capacity counts are not used in room assignments, but are included here for reference.  Selecting 'stop scheduling' will prevent a room from either being placed or appearing on the manual changes page.  Notes are similarly for your reference." & Chr(10) & Chr(10)
        strInfo &= "The grid in the lower-right will show room needs and availability by event and timeslot; note that each timeslot has a summary row indicated with the word ALL." & Chr(10) & Chr(10)
        MsgBox(strInfo)
    End Sub

    Private Sub butUnCheck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butUnCheck.Click
        If DataGridView1.Columns(DataGridView1.CurrentCell.ColumnIndex).CellType.Name <> "DataGridViewCheckBoxCell" Then
            MsgBox("This function only works for check box columns.  Click OK to exit.", MsgBoxStyle.OkOnly)
            Exit Sub
        End If
        For x = 0 To DataGridView1.Rows.Count - 1
            DataGridView1.Rows(x).Cells(DataGridView1.CurrentCell.ColumnIndex).Value = False
        Next x
    End Sub
End Class