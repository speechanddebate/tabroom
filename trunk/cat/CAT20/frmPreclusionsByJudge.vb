Public Class frmPreclusionsByJudge
    Dim ds As New DataSet
    Private Sub frmPreclusionsByJudge_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Load
        LoadFile(ds, "TourneyData")
        Call CheckMasterFile(ds)

        DataGridView1.AutoGenerateColumns = False
        DataGridView1.DataSource = ds.Tables("Entry")

        Dim dt2 As New DataTable
        dt2 = ds.Tables("Judge").Copy
        dt2.Columns.Add(("JudgeName"), System.Type.GetType("System.String"))
        For x = 0 To dt2.Rows.Count - 1
            dt2.Rows(x).Item("JudgeName") = dt2.Rows(x).Item("Last").trim & ", " & dt2.Rows(x).Item("First").trim
        Next
        dt2.DefaultView.Sort = "JudgeName"
        cboJudge.DataSource = dt2
        cboJudge.ValueMember = "ID"
        cboJudge.DisplayMember = "JudgeName"

        'Dim dr As DataRow
        'dr = ds.Tables("Judge").Rows.Find(JudgeNum)
        'If dr Is Nothing Then lblJudge.Text = "ERROR!!!! Judge not found.  Close this form and try again."
        'lblJudge.Text = "Entering prelcusions for " & dr.Item("First").trim & " " & dr.Item("Last").trim

        Call FillPreclusionBox()

    End Sub
    Private Sub frmPreclusionsByJudge_Unload(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Disposed
        'save file on page close
        Call SaveFile(ds)
        ds.Dispose()
    End Sub
    Sub FillPreclusionBox()
        Dim dt As New DataTable
        Dim fdStrikes As DataRow()
        fdStrikes = ds.Tables("JudgePref").Select("Judge=" & cboJudge.SelectedValue & " and (Rating=999 or OrdPct=999)")
        Dim dr, dr2 As DataRow
        dt.Columns.Add("TeamID", System.Type.GetType("System.Int64"))
        dt.Columns.Add("TeamName", System.Type.GetType("System.String"))
        For x = 0 To fdStrikes.Length - 1
            dr = dt.NewRow
            dr.Item("TeamID") = fdStrikes(x).Item("Team")
            dr2 = ds.Tables("Entry").Rows.Find(fdStrikes(x).Item("Team"))
            dr.Item("TeamName") = dr2.Item("FullName")
            dt.Rows.Add(dr)
        Next x
        dt.AcceptChanges()
        DataGridView2.DataSource = dt
        DataGridView2.Columns("TeamID").Visible = False
    End Sub
    Private Sub doSearch() Handles txtSearch.TextChanged
        Dim x As Integer
        For x = 0 To DataGridView1.Rows.Count - 1
            If Not DataGridView1.Rows(x).Cells("FullName").Value Is Nothing Then
                If Mid(DataGridView1.Rows(x).Cells("FullName").Value.ToString, 1, txtSearch.Text.Length).ToUpper = txtSearch.Text.ToUpper Then
                    DataGridView1.Rows(x).Selected = True
                    DataGridView1.CurrentCell = DataGridView1(1, x)
                    DataGridView1.FirstDisplayedScrollingRowIndex = x
                    Exit Sub
                End If
            End If
        Next x
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If DataGridView1.SelectedRows.Count <> 1 Then
            MsgBox("Please select one and only one team.  You can select a team by clicking on the gray box next to the team name.  Please try again.")
            Exit Sub
        End If
        Dim dr As DataRow()
        dr = ds.Tables("JudgePref").Select("Team=" & DataGridView1.CurrentRow.Cells("ID").Value & " and Judge=" & cboJudge.SelectedValue)
        If dr.Length = 0 Then
            MsgBox("Couldn't find a pref record for this team and this judge.  Visit the judge pref screen and initialize the prefs for this event.")
            Exit Sub
        End If
        If dr.Length > 1 Then MsgBox("Error") : Exit Sub
        dr(0).Item("Rating") = 999
        dr(0).Item("OrdPct") = 999
        Call FillPreclusionBox()
        txtSearch.Focus()
    End Sub

    Private Sub butDeletePreclusion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butDeletePreclusion.Click
        If DataGridView2.SelectedRows.Count <> 1 Then
            MsgBox("Plese select one and only one row and try again.")
            Exit Sub
        End If
        Dim dr As DataRow()
        dr = ds.Tables("JudgePref").Select("Team=" & DataGridView2.CurrentRow.Cells("TeamID").Value & " and Judge=" & cboJudge.SelectedValue)
        If dr.Length = 0 Then
            MsgBox("Couldn't find a pref record for thsi team and this judge.  Visit the judge pref screen and initialize the prefs for this event.")
            Exit Sub
        End If
        dr(0).Item("Rating") = 0
        dr(0).Item("OrdPct") = 0
        Call FillPreclusionBox()
        MsgBox("The rating for this judge and this team is currently zero.  If you are using a mutual preference system, you should enter a valid rating instead, which you can do on the judge preference entry screen.")
    End Sub
    Sub SetDef() Handles DataGridView1.Click
        Button1.Focus()
    End Sub

    Private Sub butSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butSearch.Click
        Dim x As Integer
        For x = 0 To DataGridView1.Rows.Count - 1
            If Not DataGridView1.Rows(x).Cells("FullName") Is Nothing Then
                If InStr(DataGridView1.Rows(x).Cells("FullName").Value.ToString.ToUpper, txtSearch.Text.ToUpper) > 0 Then
                    DataGridView1.Rows(x).Selected = True
                    DataGridView1.CurrentCell = DataGridView1(1, x)
                    DataGridView1.FirstDisplayedScrollingRowIndex = x
                    Button1.Focus()
                    Exit Sub
                End If
            End If
        Next x
    End Sub
    Private Sub butLoadJudge_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butLoadJudge.Click
        Dim dr As DataRow
        dr = ds.Tables("Judge").Rows.Find(cboJudge.SelectedValue)
        If dr Is Nothing Then lblJudge.Text = "ERROR!!!! Judge not found.  Close this form and try again."
        lblJudge.Text = "Entering prelcusions for " & dr.Item("First").trim & " " & dr.Item("Last").trim
        Call FillPreclusionBox()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Call SaveFile(ds)
    End Sub

    Private Sub butHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butHelp.Click
        Dim strInfo As String = ""
        strInfo &= "Begin by selecting a judge from the drop-down list in the middle of the screen and clicking the 'load judge' button.  THe judge name will then show up in the box to the right of the load button, and all current preclusions will show up on the grid under the selected judge." & Chr(10) & Chr(10)
        strInfo &= "To REMOVE a preclusion, click on the appropriate row in the right-hand grid and then click the 'Remove team from preclusion list' button to the right of that grid.  NOTE THAT IF YOU ARE USING MUTUAL PREFERENCE JUDGING YOU MAY NEED TO MANUALLY RE-ENTER THE PREFERENCE RATING." & Chr(10) & Chr(10)
        strInfo &= "To ADD a preclusion, click on the appropriate row in the left-hand grid and then click the 'Add selected team to preclusion list' button; it's in between the two grids." & Chr(10) & Chr(10)
        strInfo &= "To navigate the team list in a more efficient way, you can begin typing a portion of the team name in the box in the top-left.  The grid will automatically move to a row that begins with the text you type.  To search for text that isn't at the start of the team name, type the text and then click the 'Search' button." & Chr(10) & Chr(10)
        strInfo &= "Changes will automatically save when you exit the screen, but if you are making lots of changes and want to save them along the way, you can click the 'save all changes so far' button." & Chr(10) & Chr(10)
        MsgBox(strInfo)
    End Sub
End Class