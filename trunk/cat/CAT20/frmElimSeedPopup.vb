Public Class frmElimSeedPopup
    Dim drRd As DataRow
    Dim drEVent As DataRow
    Private Sub frmElimSeedPopup_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'load in info about current round, division, and event
        Call LoadRounds()
        'load current seeds for the round
        Call LoadCurrentSeeds()
        Call LoadAllTeamsInDivision()
        'clear grids
        DataGridView2.CurrentCell = Nothing : DataGridView2.Rows(0).Selected = False
        DataGridView1.CurrentCell = Nothing : DataGridView1.Rows(0).Selected = False
    End Sub
    Sub LoadAllTeamsInDivision()
        frmElims.ds.Tables("Entry").DefaultView.RowFilter = "Event=" & drRd.Item("Event")
        DataGridView1.AutoGenerateColumns = False
        DataGridView1.DataSource = frmElims.ds.Tables("Entry").DefaultView
    End Sub
    Sub LoadRounds()
        'pull the panel, find the round
        Dim panel As Integer
        Try
            panel = frmElims.DataGridView1.Rows(frmElims.DataGridView1.CurrentRow.Index).Cells(frmElims.DataGridView1.CurrentCell.ColumnIndex - 1).Value
        Catch
            Exit Sub
        End Try

        Dim drPanel As DataRow
        drPanel = frmElims.ds.Tables("Panel").Rows.Find(panel)
        drRd = frmElims.ds.Tables("Round").Rows.Find(drPanel.Item("Round"))
        Dim drevent = frmElims.ds.Tables("Event").Rows.Find(drRd.Item("Event"))

    End Sub
    Sub LoadCurrentSeeds()
        Dim dt As New DataTable
        dt.Columns.Add("Seed", System.Type.GetType("System.Int64"))
        dt.Columns.Add("ElimSeedID", System.Type.GetType("System.Int64"))
        dt.Columns.Add("Entry", System.Type.GetType("System.Int64"))
        dt.Columns.Add("Team", System.Type.GetType("System.String"))

        Dim fdSeeds As DataRow()
        
        fdSeeds = frmElims.ds.Tables("ElimSeed").Select("Round=" & drRd.Item("ID"))

        Dim dr, drTeam As DataRow
        For x = 0 To fdSeeds.Length - 1
            dr = dt.NewRow
            dr.Item("Seed") = fdSeeds(x).Item("Seed")
            drTeam = frmElims.ds.Tables("Entry").Rows.Find(fdSeeds(x).Item("Entry"))
            If Not drTeam Is Nothing Then
                dr.Item("Team") = drTeam.Item("FullName")
                dr.Item("Entry") = fdSeeds(x).Item("Entry")
                dr.Item("ElimSeedID") = fdSeeds(x).Item("ID")
            Else
                dr.Item("Team") = "Not set yet"
            End If
            dt.Rows.Add(dr)
        Next x

        DataGridView2.DataSource = dt

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim q = MsgBox("Drop " & DataGridView2.CurrentRow.Cells("Team").Value & " as the " & DataGridView2.CurrentRow.Cells("Seed").Value & " seed and replace them with " & DataGridView1.CurrentRow.Cells("FullName").Value & "?", MsgBoxStyle.YesNo)
        If q = vbNo Then Exit Sub
        'change the seed table
        Dim drElimSeed As DataRow
        drElimSeed = frmElims.ds.Tables("ElimSeed").Rows.Find(DataGridView2.CurrentRow.Cells("ElimSeedID").Value)
        drElimSeed.Item("Entry") = DataGridView1.CurrentRow.Cells("ID").Value
        'change current pairings
        Dim fdBallot As DataRow()
        Dim TeamOut As Integer = DataGridView2.CurrentRow.Cells("Entry").Value
        fdBallot = frmElims.ds.Tables("Ballot").Select(BuildPanelStringByRound(frmElims.ds, drRd.Item("ID")) & " and entry=" & DataGridView2.CurrentRow.Cells("Entry").Value)
        For x = 0 To fdBallot.Length - 1
            fdBallot(x).Item("Entry") = DataGridView1.CurrentRow.Cells("ID").Value
        Next x
        Me.Close()
        Call frmElims.LoadTheElim()
    End Sub
End Class