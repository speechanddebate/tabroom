Public Class frmteamblock
    Dim ds As New DataSet
    Private Sub frmteamblock_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call LoadFile(ds, "TourneyData")
        DataGridView1.AutoGenerateColumns = False
        DataGridView1.DataSource = ds.Tables("Entry")
        DataGridView1.ClearSelection()

        'add fullnames columns to the existing timeblocks column
        Dim dgvc As New DataGridViewComboBoxColumn
        dgvc.DataSource = ds.Tables("ENTRY")    'this is child/detail table; EVENT in this case
        dgvc.ValueMember = "ID"                 'on the child/detail table
        dgvc.DisplayMember = "FULLNAME"        'on the child/detail table
        dgvc.DataPropertyName = "TEAM1"         'field from the master/parent table; ENTRY in this case
        dgvc.Name = "TEAM1"
        dgvc.HeaderText = "Team 1"
        dgvc.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing
        DataGridView2.Columns.Add(dgvc)

        dgvc = New DataGridViewComboBoxColumn
        dgvc.DataSource = ds.Tables("ENTRY")    'this is child/detail table; EVENT in this case
        dgvc.ValueMember = "ID"                 'on the child/detail table
        dgvc.DisplayMember = "FULLNAME"        'on the child/detail table
        dgvc.DataPropertyName = "TEAM2"         'field from the master/parent table; ENTRY in this case
        dgvc.Name = "TEAM2"
        dgvc.HeaderText = "Team 2"
        dgvc.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing
        DataGridView2.Columns.Add(dgvc)

        DataGridView2.AutoGenerateColumns = False
        DataGridView2.DataSource = ds.Tables("teamblock")
        DataGridView2.ClearSelection()

    End Sub

    Private Sub butAddBlock_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butAddBlock.Click
        'check the block isn't there now
        'add it
        Dim dr As DataRow
        Dim x, team1, team2 As Integer
        Dim selectedRowCount As Integer = DataGridView1.Rows.GetRowCount(DataGridViewElementStates.Selected)
        If selectedRowCount <> 2 Then
            MsgBox("You have selected too many or too few teams.  Please select exactly 2 teams and try again.")
            Exit Sub
        End If
        For x = 0 To selectedRowCount - 1
            If team1 = 0 Then
                team1 = DataGridView1.SelectedRows(x).Cells("ID").Value
            Else
                team2 = DataGridView1.SelectedRows(x).Cells("ID").Value
            End If
        Next x
        dr = ds.Tables("teamblock").NewRow
        dr.Item("Team1") = team1
        dr.Item("Team2") = team2
        ds.Tables("teamblock").Rows.Add(dr)
        DataGridView1.ClearSelection()
        DataGridView2.ClearSelection()
    End Sub

    Private Sub butDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butDelete.Click
        DataGridView1.ClearSelection()
        DataGridView2.ClearSelection()
    End Sub
    Private Sub frmteamblock_Unload(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Disposed
        'save file on page close
        Call SaveFile(ds)
        ds.Dispose()
    End Sub
End Class