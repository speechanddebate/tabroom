Public Class frmWUDCSideChange
    Dim ds As New DataSet
    Dim EnableEvents As Boolean
    Dim panel, eventID As Integer
    Private Sub frmWUDCSideChange_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        EnableEvents = False
        LoadFile(ds, "TourneyData")

        panel = frmShowPairings.DataGridView1.CurrentRow.Cells("Panel").Value
        Dim fdpanel As DataRow : fdpanel = ds.Tables("Panel").Rows.Find(panel)
        Dim fdRound As DataRow : fdRound = ds.Tables("Round").Rows.Find(fdpanel.Item("Round"))

        eventID = fdRound.Item("Event")
        'fill the sides cbo
        Dim dt As New DataTable
        dt.Columns.Add("Side", System.Type.GetType("System.Int16"))
        dt.Columns.Add("SideName", System.Type.GetType("System.String"))
        Dim dr As DataRow
        For x = 1 To 4
            dr = dt.NewRow
            dr.Item("Side") = x
            dr.Item("SideName") = GetSideString(ds, x, eventID)
            dt.Rows.Add(dr)
        Next x

        'bind cbo
        cboSide.DataSource = dt
        cboSide.DisplayMember = "SideName"
        cboSide.ValueMember = "Side"
        cboSide.Focus()

        Call UpdateGrid()
        lblStatus.Text = strSideCheck() : If lblStatus.Text = "" Then lblStatus.Text = "Sides OK"
        EnableEvents = True
    End Sub
    Sub ReallyLeave(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If strSideCheck() <> "" Then
            Dim q As Integer = MsgBox(strSideCheck() & " Leave anyway?", MsgBoxStyle.YesNo)
            If q = vbNo Then e.Cancel = True
        End If
    End Sub
    Private Sub frmWUDCSideChange_Unload(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Disposed
        Call SaveFile(ds)
        ds.Dispose()
    End Sub
    Sub UpdateGrid()
        'fill datagridview
        Dim fdballots As DataRow()
        fdballots = ds.Tables("Ballot").Select("Panel=" & panel, "Side ASC")
        Dim dt2 As New DataTable
        dt2.Columns.Add("EntryID", System.Type.GetType("System.Int64"))
        dt2.Columns.Add("EntryName", System.Type.GetType("System.String"))
        dt2.Columns.Add("Side", System.Type.GetType("System.String"))
        Dim dr, dr2 As DataRow
        Dim ShouldIAddThisStupidRow As Boolean
        For x = 0 To fdballots.Length - 1
            ShouldIAddThisStupidRow = False
            If fdballots.Length - 1 = x Then
                ShouldIAddThisStupidRow = True
            ElseIf fdballots(x).Item("Entry") <> fdballots(x + 1).Item("Entry") Then
                ShouldIAddThisStupidRow = True
            End If
            If ShouldIAddThisStupidRow = True Then
                dr = dt2.NewRow
                dr.Item("EntryID") = fdballots(x).Item("Entry")
                dr2 = ds.Tables("Entry").Rows.Find(fdballots(x).Item("Entry"))
                dr.Item("EntryName") = dr2.Item("Code")
                dr.Item("Side") = GetSideString(ds, fdballots(x).Item("Side"), eventID)
                dt2.Rows.Add(dr)
            End If
        Next x
        DataGridView1.DataSource = dt2
        DataGridView1.Columns("EntryID").Visible = False
    End Sub
    Sub ChangeSide() Handles cboSide.SelectedValueChanged
        If EnableEvents = False Then Exit Sub
        Dim fdBallots As DataRow()
        fdBallots = ds.Tables("Ballot").Select("Panel=" & panel & " and entry=" & DataGridView1.CurrentRow.Cells("EntryID").Value)
        For x = 0 To fdBallots.Length - 1
            fdBallots(x).Item("Side") = cboSide.SelectedValue
        Next x
        Call UpdateGrid()
        lblStatus.Text = strSideCheck() : If lblStatus.Text = "" Then lblStatus.Text = "Sides OK"
    End Sub
    Function strSideCheck()
        strSideCheck = ""
        Dim strDummy As String : Dim ctr As Integer
        For x = 1 To 4
            strDummy = GetSideString(ds, x, eventID)
            ctr = 0
            For y = 0 To DataGridView1.Rows.Count - 1
                If DataGridView1.Rows(y).Cells("Side").Value = strDummy Then ctr += 1
            Next y
            If ctr > 1 Then strSideCheck &= strDummy & " appears too MANY times. "
            If ctr = 0 Then strSideCheck &= "No team is assigned as " & strDummy & ". "
        Next x
    End Function
End Class