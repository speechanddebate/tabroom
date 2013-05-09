Public Class frmTRPCPair
    Dim ds As New DataSet
    Private Sub frmTRPCPair_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call LoadFile(ds, "TourneyData")

        'fill the divisions cbo
        cboEvents.DataSource = ds.Tables("Event")
        cboEvents.DisplayMember = "EventName"
        cboEvents.ValueMember = "ID"
        If ds.Tables("Event").Rows.Count = 1 Then
            cboEvents.SelectedIndex = 0 : Call ShowRounds()
        End If
    End Sub
    Private Sub frmTRPCPair_Unload(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Disposed
        Call SaveFile(ds)
        ds.Tables("Round").DefaultView.RowFilter.Remove(0)
        ds.Dispose()
    End Sub

    Private Sub cboEvents_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboEvents.SelectedIndexChanged
        If cboEvents.SelectedValue.ToString = "System.Data.DataRowView" Then Exit Sub
        Call ShowRounds()
    End Sub
    Sub ShowRounds()
        ds.Tables("Round").DefaultView.RowFilter = "Event=" & cboEvents.SelectedValue & " and Rd_Name <=9"
        DataGridView1.AutoGenerateColumns = False
        DataGridView1.DataSource = ds.Tables("Round")
    End Sub

    Private Sub butPairOnePreset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butPairOnePreset.Click
        'Should we check the radio button to see if seedings are checked?  Can you pair one round off seedings?
    End Sub

    Private Sub butPowermatch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butPowermatch.Click
        'check row is selected
        If DataGridView1.SelectedRows.Count = 0 Then
            MsgBox("Please select one row from the grid above and try again.", MsgBoxStyle.OkOnly)
            Exit Sub
        End If
        'Check this is a powered round
        Dim MatchType As String = DataGridView1.CurrentRow.Cells("PairingScheme").Value
        If MatchType = "PRESET" Then
            MsgBox("This round is marked as a preset round.  To power-match this round, close this screen, click on the ROUNDS button, and change the Pairing Scheme for this round to one of the power-matching options.", MsgBoxStyle.OkOnly)
            Exit Sub
        End If
        'delete existing pairings if checked
        If chkDeleteExisting.Checked = True Then Call DeleteAllPanelsForRound(ds, DataGridView1.CurrentRow.Cells("ID").Value)
        'now ready for you to pair it...
        'define and load in the array
        Dim SeedingArray(400, 20) As Single
        Dim UseRegions As Boolean = False
        Call MakeTRPCArray(SeedingArray, UseRegions)

    End Sub
    Sub MakeTRPCArray(ByRef arrTRPC(,) As Single, ByRef UseRegions As Boolean)
        'set whether regions are in use
        UseRegions = getEventSetting(ds, cboEvents.SelectedValue, "UseRegions")

        'load in the seeds table with all the tiebreakers
        Dim dt As New DataTable
        dt = MakeTBTable(ds, GetPriorRound(ds, DataGridView1.CurrentRow.Cells("ID").Value), "Team", "Code", -1, DataGridView1.CurrentRow.Cells("ID").Value)
        If dt.Columns("OppWins") Is Nothing Then
            MsgBox("Opposition wins do not appear to have been set as a tiebreaker and will be ignored in this pairing.  If you wish to include opposition wins, ")
        End If
        Dim dr As DataRow

        'Check whether this is a preset
        Dim MatchType As String = DataGridView1.CurrentRow.Cells("PairingScheme").Value

        'If it's a preset, Perform this load and exit the sub
        If MatchType.ToUpper = "PRESET" Then
            For x = 0 To dt.Rows.Count - 1
                arrTRPC(x, 1) = dt.Rows(x).Item("Competitor")
                dr = ds.Tables("Entry").Rows.Find(dt.Rows(x).Item("Competitor"))
                arrTRPC(x, 2) = dr.Item("Rating")
                arrTRPC(x, 2) = 0
                arrTRPC(x, 4) = 0
                arrTRPC(x, 5) = 0
                arrTRPC(x, 6) = HadBye(ds, DataGridView1.CurrentRow.Cells("ID").Value, dt.Rows(x).Item("Competitor"))
                arrTRPC(x, 7) = GetRegion(ds, dt.Rows(x).Item("Competitor"))
            Next
            Exit Sub
        End If

        'it's not a preset, so add all the info
        'find the first 2 tiebreakers
        Dim TB1, TB2 As Integer
        For x = 4 To dt.Columns.Count - 1
            If dt.Columns(x).ColumnName <> "Wins" And dt.Columns(x).ColumnName <> "OppWins" And TB1 = 0 Then
                TB1 = x
            End If
            If dt.Columns(x).ColumnName <> "Wins" And dt.Columns(x).ColumnName <> "OppWins" And TB2 = 0 And TB1 <> x Then
                TB2 = x
            End If
            If TB1 > 0 And TB2 > 0 Then Exit For
        Next

        For x = 0 To dt.Rows.Count - 1
            arrTRPC(x, 1) = dt.Rows(x).Item("Competitor")
            arrTRPC(x, 2) = dt.Rows(x).Item("Wins")
            arrTRPC(x, 3) = dt.Rows(x).Item("OppWins")
            arrTRPC(x, 4) = dt.Rows(x).Item(TB1)
            arrTRPC(x, 5) = dt.Rows(x).Item(TB2)
            arrTRPC(x, 6) = HadBye(ds, DataGridView1.CurrentRow.Cells("ID").Value, dt.Rows(x).Item("Competitor"))
            arrTRPC(x, 7) = GetRegion(ds, dt.Rows(x).Item("Competitor"))
        Next x

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        'x=team
        'y1=CAT team #, y2=wins, y3=opp wins, y4=1st tiebreaker, y5=2nd tiebreaker, y6=number of byes, where any number
        'greater than zero means a bye, y7=geographical region
        Dim SeedingArray(400, 20) As Single
        Dim UseRegions As Boolean = False
        Call MakeTRPCArray(SeedingArray, UseRegions)
        ListBox1.Items.Clear()
        Dim str As String
        For x = 0 To 400
            str = ""
            For y = 1 To 7
                str &= SeedingArray(x, y)
                If y < 7 Then str &= ", "
            Next y
            ListBox1.Items.Add(str)
        Next x

        
    End Sub

    Private Sub butAllPresets_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butAllPresets.Click
        Dim SeedingArray(400, 20) As Single
        Dim UseRegions As Boolean = False
        For x = 0 To DataGridView1.RowCount - 1
            If DataGridView1.Rows(x).Cells("PairingScheme").Value = "Preset" Then
                DataGridView1.Rows(x).Selected = True
                Call MakeTRPCArray(SeedingArray, UseRegions)
                'From here, call a function that pairs the round passing the SeedingArray and UseRegions values
            End If
        Next x
    End Sub
End Class