Imports System.IO
Imports System.Xml
Imports System.Net

Public Class frmJudgePref

    Dim ds As New DataSet
    Private Sub frmJudgePref_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LoadFile(ds, "TourneyData")
        Call LoadDataForForm()
    End Sub
    Sub LoadDataForForm()
        ds.Tables("Entry").DefaultView.Sort = "FullName"
        cboTeam.DataSource = ds.Tables("Entry").DefaultView
        cboTeam.ValueMember = "ID"
        cboTeam.DisplayMember = "FullName"

        ds.Tables("School").DefaultView.Sort = "SchoolName"
        cboSchool.DataSource = ds.Tables("School").DefaultView
        cboSchool.ValueMember = "ID"
        cboSchool.DisplayMember = "SchoolName"

        Dim dt As New DataTable
        dt = ds.Tables("Entry")
        cboCopyTo.DataSource = dt
        cboCopyTo.ValueMember = "ID"
        cboCopyTo.DisplayMember = "FullName"

        'add and populate name column
        lblMessage.Text = "Loading judge names..." : lblMessage.Refresh()
        If ds.Tables("JudgePref").Columns.Contains("JudgeName") = False Then
            ds.Tables("JudgePref").Columns.Add(("JudgeName"), System.Type.GetType("System.String"))
        End If
        If ds.Tables("JudgePref").Columns.Contains("Rounds") = False Then
            ds.Tables("JudgePref").Columns.Add(("Rounds"), System.Type.GetType("System.Int16"))
        End If

        Dim dr As DataRow
        For x = 0 To ds.Tables("JudgePref").Rows.Count - 1
            dr = ds.Tables("Judge").Rows.Find(ds.Tables("JudgePref").Rows(x).Item("Judge"))
            If Not dr Is Nothing Then
                ds.Tables("JudgePref").Rows(x).Item("JudgeName") = dr.Item("Last").trim & ", " & dr.Item("First").trim
                ds.Tables("JudgePref").Rows(x).Item("Rounds") = dr.Item("Obligation") + dr.Item("Hired")
            End If
        Next x

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

        'display whether any division is using prefs
        Dim dummy As String = PrefsInUse()
        If dummy <> "" Then lblMessage.Text &= dummy
        'change the message
        lblMessage.Text = "Select a team above; prefs status will appear here." & Chr(10) & Chr(10)
        lblMessage.Text &= "If you have not yet done so, make sure you click the HELP button in the top-right and use the 2 buttons to the right BEFORE you first begin placing judges." : lblMessage.Refresh()
    End Sub
    Private Sub frmJudgePref_Unload(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Disposed
        ds.Tables("JudgePref").Columns.Remove("JudgeName")
        ds.Tables("JudgePref").Columns.Remove("Rounds")
        Call SaveFile(ds)
        ds.Dispose()
    End Sub
    Sub ShowPrefs() Handles cboTeam.SelectedIndexChanged
        If cboTeam.SelectedValue.ToString = "System.Data.DataRowView" Then Exit Sub
        ds.Tables("JudgePref").DefaultView.RowFilter = "Team=" & cboTeam.SelectedValue
        ds.Tables("JudgePref").DefaultView.Sort = "Rating ASC"
        DataGridView1.AutoGenerateColumns = False
        DataGridView1.DataSource = ds.Tables("JudgePref").DefaultView
        DataGridView1.Columns("OrdPct").DefaultCellStyle.Format = "##.##"
        Dim x As Integer
        lblMessage.Text = CheckPrefs(cboTeam.SelectedValue, x)
    End Sub
    Function CheckPrefs(ByVal Team As Integer, ByRef ctr As Integer) As String
        Dim fdPrefs, fdJudges, drPref As DataRow() : Dim drTeam As DataRow
        fdPrefs = ds.Tables("JudgePref").Select("Team=" & Team)
        CheckPrefs = ""
        'Look for unrated judges
        For x = 0 To fdPrefs.Length - 1
            If fdPrefs(x).Item("Rating") Is System.DBNull.Value Then ctr += 1
            If fdPrefs(x).Item("Rating") = 0 Then ctr += 1
            If fdPrefs(x).Item("Rating") = 333 Then ctr += 1
        Next
        If ctr > 0 Then
            CheckPrefs = ctr & " judges appear to be unrated." & Chr(10) & Chr(10)
        End If
        'Look for missing judges
        drTeam = ds.Tables("Entry").Rows.Find(Team)
        fdJudges = ds.Tables("Judge").Select("Event" & drTeam.Item("Event") & "=true")
        Dim ctr2 As Integer = 0
        For x = 0 To fdJudges.Length - 1
            drPref = ds.Tables("JudgePref").Select("Team=" & Team & " and Judge=" & fdJudges(x).Item("ID"))
            If drPref.Length = 0 Then ctr2 += 1
            If drPref.Length > 1 Then CheckPrefs &= drTeam.Item("FullName") & " has too many rating fields for judge " & fdJudges(x).Item("First").trim & " " & fdJudges(x).Item("Last").trim & Chr(10) & Chr(10)
        Next x
        If ctr2 > 0 Then
            CheckPrefs &= ctr2 & " judges have no records in the datafile for this team." & Chr(10) & Chr(10)
        End If
        If CheckPrefs = "" Then CheckPrefs = "Ratings appear to be OK"
    End Function
    Private Sub butChangeFont_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butChangeFont.Click
        Dim x = InputBox("Enter font size:")
        If x < 6 Then x = 6
        If x > 14 Then x = 14
        DataGridView1.Font = New Font("Franklin Gothic Medium", x)
        DataGridView1.AutoResizeRows()
    End Sub
    Sub MadeChange() Handles DataGridView1.CellEndEdit
        'AutoCalc does it from the datagrid; this is necessary because the changes to the dataset don't occur on cellendedit
        Call AutoCalc()
        'Call DSAutoCalc(cboTeam.SelectedValue)
    End Sub
    
    Sub AutoCalc()
        'Find total rounds in the pool
        Dim totRds As Integer = ds.Tables("Judge").Compute("Sum(Obligation)", "")
        totRds += ds.Tables("Judge").Compute("Sum(Hired)", "")

        'calculate
        Dim ctr As Integer = 1 : Dim numerator As Integer = 1
        For x = 0 To DataGridView1.RowCount - 1
            DataGridView1.Rows(x).Cells("OrdPct").Value = FormatNumber((numerator / totRds) * 100, 1)
            ctr += DataGridView1.Rows(x).Cells("Rounds").Value
            If x < DataGridView1.RowCount - 1 Then If DataGridView1.Rows(x).Cells("Rating").Value <> DataGridView1.Rows(x + 1).Cells("Rating").Value Then numerator = ctr
            If DataGridView1.Rows(x).Cells("Rating").Value = 999 Then DataGridView1.Rows(x).Cells("OrdPct").Value = 999
        Next x

    End Sub
    Function PrefsInUse() As String
        PrefsInUse = ""
        Dim fdRd As DataRow()
        For x = 0 To ds.Tables("Event").Rows.Count - 1
            fdRd = ds.Tables("Round").Select("Event=" & ds.Tables("Event").Rows(x).Item("ID"))
            For y = 0 To fdRd.Length - 1
                If fdRd(y).Item("JudgePlaceScheme") = "TeamRating" Then
                    If PrefsInUse <> "" Then PrefsInUse &= " Or "
                    PrefsInUse &= "Event=" & ds.Tables("Event").Rows(x).Item("ID")
                    Exit For
                End If
            Next y
        Next x
    End Function
    Private Sub butTestAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butTestAll.Click
        DataGridView3.Visible = False
        lblMessage.Text = "Finding divisions using team ratings of judges..." : lblMessage.Refresh()
        'See if division is using prefs.
        Dim sql As String = PrefsInUse()
        Dim fdteam As DataRow()
        If sql = "" Then lblMessage.Text = "No divisions appear to be using prefs.  If you want to have a division use mutual preference judging, return to the divisions/events setup screen, change the setting for the desired division, and reutrn to this page." : Exit Sub
        'pull all teams in divisions using prefs
        Dim dt As New DataTable
        dt.Columns.Add(("Team"), System.Type.GetType("System.String"))
        dt.Columns.Add(("Division"), System.Type.GetType("System.String"))
        dt.Columns.Add(("UnRated"), System.Type.GetType("System.Int16"))
        dt.Columns.Add(("Status"), System.Type.GetType("System.String"))
        'scroll and display
        fdteam = ds.Tables("Entry").Select(sql)
        Dim dr, drDiv As DataRow : Dim Ctr As Integer : Dim nUnder2, nOver2 As Integer
        For x = 0 To fdteam.Length - 1
            lblMessage.Text = "Loading team info " & x & "/" & fdteam.Length - 1 : lblMessage.Refresh()
            dr = dt.NewRow : Ctr = 0
            dr.Item("Team") = fdteam(x).Item("Code")
            drDiv = ds.Tables("Event").Rows.Find(fdteam(x).Item("Event"))
            dr.Item("Division") = drDiv.Item("Abbr")
            dr.Item("Status") = CheckPrefs(fdteam(x).Item("ID"), Ctr)
            dr.Item("UnRated") = Ctr
            If Ctr <= 2 Then nUnder2 += 1 Else nOver2 += 1
            dt.Rows.Add(dr)
        Next x
        dt.DefaultView.Sort = "unrated desc"
        DataGridView2.DataSource = dt
        DataGridView2.Visible = True
        butHideGrid.Visible = True
        lblMessage.Text = "Test complete. " & nUnder2 & " missing 2 or fewer ratings, " & nOver2 & " missing more than 2 ratings" : lblMessage.Refresh()
    End Sub

    Private Sub butFixFormat_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butFixFormat.Click
        'check that all teams have a rating field for all eligible judges
        'if no record, store a zero
        'if multiple records and identical, delete one
        'if multiple records and different, ask user

        'PUll teams in divisions that use prefs
        Dim fdteam, fdjudges, fdPrefs As DataRow() : Dim dr As DataRow
        fdteam = ds.Tables("Entry").Select(PrefsInUse)
        Dim SameScore As Boolean : Dim strRating As String = "" : Dim HiPref As Integer
        For x = 0 To fdteam.Length - 1
            lblMessage.Text = "Testing team " & x & " of " & fdteam.Length - 1 : lblMessage.Refresh()
            'PUll all judges eligible to judge in division of the team
            fdjudges = ds.Tables("Judge").Select("Event" & fdteam(x).Item("Event") & "=true")

            For y = 0 To fdjudges.Length - 1
                'Pull prefs by team and judge
                fdPrefs = ds.Tables("JudgePref").Select("team=" & fdteam(x).Item("ID") & " and judge=" & fdjudges(y).Item("ID"))
                'process if no ratings field is present
                If fdPrefs.Length = 0 Then
                    dr = ds.Tables("JudgePref").NewRow
                    dr.Item("Team") = fdteam(x).Item("ID")
                    dr.Item("Judge") = fdjudges(y).Item("ID")
                    dr.Item("Rating") = 0
                    ds.Tables("JudgePref").Rows.Add(dr)
                    'process if more than 1 rating field is present
                ElseIf fdPrefs.Length > 1 Then
                    'find out whether the multiple prefs all have the same rating
                    SameScore = True : HiPref = 9999
                    For z = 1 To fdPrefs.Length - 1
                        If strRating <> "" Then strRating = ", "
                        strRating &= fdPrefs(z).Item("Rating")
                        If fdPrefs(z).Item("Rating") <> fdPrefs(z - 1).Item("Rating") Then SameScore = False
                        If fdPrefs(z).Item("Rating") > 0 And fdPrefs(z).Item("Rating") < HiPref Then HiPref = fdPrefs(z).Item("Rating")
                    Next z
                    'if not, ask the user for the rating and set it to the rating for the first record found
                    If SameScore = False Then
                        fdPrefs(0).Item("Rating") = InputBox(fdteam(x).Item("Code") & " have given " & fdjudges(y).Item("First").Trim & " " & fdjudges(y).Item("Last").trim & " ratings of: " & strRating & ".  Enter the desired pref for this judge:", , HiPref)
                    End If
                    'delete all records after the first one
                    For z = fdPrefs.Length - 1 To 0 Step -1
                        fdPrefs(z).Delete()
                    Next
                End If
            Next
        Next x
        lblMessage.Text = "Done.  If nothing happened, everything is OK." : lblMessage.Refresh()
    End Sub

    Private Sub butHideGrid_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butHideGrid.Click
        DataGridView2.Visible = False
        DataGridView3.Visible = False
        butHideGrid.Visible = False
    End Sub

    Private Sub butCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butCopy.Click
        lblMessage.Text = "Start copying..."
        If DataGridView1.RowCount = 0 Then
            MsgBox("No prefs appear to have been selected.  Select a team from the drop-down list at the top of their page; after you do so the prefs to copy from will appear in the grid to the left.  Please select a team and try again.", MsgBoxStyle.OkOnly)
            Exit Sub
        End If
        Dim q As Integer
        Dim dr1, dr2 As DataRow
        dr1 = ds.Tables("Entry").Rows.Find(cboTeam.SelectedValue)
        dr2 = ds.Tables("Entry").Rows.Find(cboCopyTo.SelectedValue)
        q = MsgBox("If you proceed, you will DELETE all the prefs for " & dr2.Item("FullName") & " and replace the with the prefs for " & dr1.Item("FullName") & ".  Click OK to complete the copy or CANCEL to exit without changes.", MsgBoxStyle.OkCancel)
        If q = vbCancel Then Exit Sub
        'Find current prefs and delete them
        Dim fdPrefs As DataRow()
        fdPrefs = ds.Tables("JudgePref").Select("Team=" & cboCopyTo.SelectedValue)
        For x = 0 To fdPrefs.Length - 1
            fdPrefs(x).Item("Rating") = 0
        Next x
        'now copy
        For x = 0 To DataGridView1.RowCount - 1
            fdPrefs = ds.Tables("JudgePref").Select("Team=" & cboCopyTo.SelectedValue & " and Judge=" & DataGridView1.Rows(x).Cells("Judge").Value)
            If fdPrefs.Length = 0 Then
                lblMessage.Text &= "Copying of " & DataGridView1.Rows(x).Cells("JudgeName").Value & " failed; there appears to be now record for the judge on the recipient team.  If this occurs because the teams are in different divisions, this is nothing to worry about.  Click the help button for information on how to fix this problem." & Chr(10) & Chr(10)
            Else
                fdPrefs(0).Item("Rating") = DataGridView1.Rows(x).Cells("Rating").Value
                fdPrefs(0).Item("OrdPct") = DataGridView1.Rows(x).Cells("OrdPct").Value
            End If
        Next x
        lblMessage.Text &= "Copying complete."
    End Sub

    Private Sub butConvertTo333_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butConvertTo333.Click
        Call DoConvert(0, 333)
    End Sub
    Sub DoConvert(ByVal oldVal As Integer, ByVal newVal As Integer)
        nobugz.PatchMsgBox(New String() {"All", "This team only"})
        Dim q As Integer
        q = MsgBox("Convert for ALL teams or THIS TEAM ONLY?", MsgBoxStyle.YesNo)
        'q=6 means all, q=7 means this team only
        Dim fdPref As DataRow()
        fdPref = ds.Tables("JudgePref").Select("Team=" & cboTeam.SelectedValue)
        If q = 6 Then fdPref = ds.Tables("JudgePref").Select("Team<>-13")
        For x = 0 To fdPref.Length - 1
            If fdPref(x).Item("Rating") = oldVal Then fdPref(x).Item("Rating") = newVal
        Next
        Dim z = MsgBox("Re-calculate ordinal percentiles?", MsgBoxStyle.YesNo)
        If z = vbYes Then
            If q = 6 Then ReCalcAllPercentiles()
            If q = 7 Then Call DSAutoCalc(ds, cboTeam.SelectedValue)
        End If
    End Sub

    Private Sub butConvertTo0_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butConvertTo0.Click
        Call DoConvert(333, 0)
    End Sub

    Private Sub butRecalcAllPercentiles_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butRecalcAllPercentiles.Click
        Call ReCalcAllPercentiles()
    End Sub
    Sub ReCalcAllPercentiles()
        For x = 0 To ds.Tables("Entry").Rows.Count - 1
            lblMessage.Text = "Recalculating percentiles for team " & x & " of " & ds.Tables("Entry").Rows.Count - 1 : lblMessage.Refresh()
            Call DSAutoCalc(ds, ds.Tables("Entry").Rows(x).Item("ID"))
        Next
        lblMessage.Text = "Percentile recalculation complete." : lblMessage.Refresh()
    End Sub

    Private Sub butInitializePrefs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butInitializePrefs.Click
        Call InitializeJudges(ds)
        Call PrefInit(ds)
        Dim q As Integer = MsgBox("Mark judges as conflicted with all teams in a division they are ineligible to hear (necessary if you are using STA auto judge placement)?", MsgBoxStyle.YesNo)
        If q = vbYes Then Call DivisionBlocker(ds)
        Call LoadDataForForm()
        MsgBox("done")
    End Sub

    Private Sub butSchoolConflict_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butSchoolConflict.Click
        lblMessage.Text = ""
        'check that there are selections
        If cboSchool.SelectedIndex = -1 Then
            MsgBox("Plesae select a valid school and try again") : Exit Sub
        ElseIf cboJudge.SelectedIndex = -1 Then
            MsgBox("Plesae select a valid judge and try again") : Exit Sub
        End If
        Dim fdTeams, fdPrefs As DataRow()
        fdTeams = ds.Tables("Entry").Select("School=" & cboSchool.SelectedValue)
        For x = 0 To fdTeams.Length - 1
            fdPrefs = ds.Tables("JudgePref").Select("Judge=" & cboJudge.SelectedValue & " and team=" & fdTeams(x).Item("ID"))
            For y = 0 To fdPrefs.Length - 1
                fdPrefs(y).Item("Rating") = 999 : fdPrefs(y).Item("Ordpct") = 999
                lblMessage.Text &= "Marked conflict for " & fdTeams(x).Item("Code") & ". "
            Next y
            Call DSAutoCalc(ds, fdTeams(x).Item("ID"))
        Next x
        lblMessage.Text &= " DONE."
    End Sub

    Private Sub butBasicInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butBasicInfo.Click
        Dim strInfo As String = ""
        strInfo &= "THE FIRST TIME YOU VISIT THIS PAGE, YOU SHOULD CLICK THE INITIALIZE BUTTON.  This will delete any duplicate ratings that creeped into the ether and make sure that there is a rating for every judge.  It will never delete anything but a duplicate record, so there's no harm in clicking on it." & Chr(10) & Chr(10)
        strInfo &= "GENERAL USE: Select the team for which you wish to enter prefs in the drop-down list in the middle of the page; their prefs will appear in the grid to the left.  Alter prefs by entering a new value in the RATING column; the remainder of the columns will automatically update." & Chr(10) & Chr(10)
        strInfo &= "COPYING: Load prefs by selecting a team in the top-middle drop down list, and then select the team you wish to copy the prefs TO from the copy box.  When both teams are selected, click the COPY button." & Chr(10) & Chr(10)
        strInfo &= "TESTING: Clicking the 'Test all teams' button will show which teams are missing prefs.  The '1 and only 1' button will automatically make corrections, but probably won't be necessary. Clicking 'hide the status grid' will make the grid created by the 'test all teams' button disappear." & Chr(10) & Chr(10)
        strInfo &= "000/333 CONVERSION: By default, a rating of zero will be the highest possible rating a judge can receive, so NOT rating a judge makes you very likely to get them.  This might not be appropriate if a team appears to have made a good faith effort to rank all the judges and just missed one.  Clicking 'convert 0 to 333' will give unranked judges a rating of 333, and make them very UN-likely to be used for the team.  Clicking 'covert 333 to 0' will reverse the operation.  " & Chr(10) & Chr(10)
        strInfo &= "INITIALIZE PREFS: This button will check to make sure that every team has a rating record for every judge, and if it can't find one, will create a blank.  It will not erase existing prefs." & Chr(10) & Chr(10)
        strInfo &= "RECALCUALTE ALL PERCENTILES: This button will re-calculate all percentiles for all teams.  This might be useful if, for example, 2 high-commitment judges that highly rated withdraw from the tournament due to illness or other reasons.  However, this will significantly alter the percentiles AFTER the teams have had a chance to review them, so it should be used sparingly." & Chr(10) & Chr(10)
        strInfo &= "UPDATE WITH ONLINE PREFS: This button will download prefs from the internet and write them over the prefs in the CAT.  if the DISPLAYED TEAM ONLY button is checked, prefs will only be updated for the displayed team.  If it's not checked, all teams will be updated with the online prefs." & Chr(10) & Chr(10)
        MsgBox(strInfo)
    End Sub

    Private Sub butWhyPercentiles_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butWhyPercentiles.Click
        Dim strInfo As String = ""
        strInfo &= "Imagine there are two teams; team A has ranked judges 1-10 who are all commited for 1 round each.  Team B has ranked judges 1-10 who are all committed for 6 rounds each.  Team A therefore has put 10 units of judging in their top 10 ranks, but team B has put 60 units of judging in their top 10 ranks.  Obviously, as the tournament goes on, it will be much easier for the tab room to give team B judges in their top 10 spots than it is for them to give Team A a judge in their top 10 spots (because, among other reasons, a low-commitment judge is more likely to burn their commitment before later rounds occur).  As a result, some tab rooms may calculate a judge's placement as a percentile of the total unit pool rather than their raw rank. As a result, some tab rooms may calculate a judge's placement as a percentile of the total unit pool rather than their raw rank.  If the tournament attempts to give you judges in the top 40% of the pool, the break point will vary considerably depending on whether the denominator is the raw judge count or the total unit count." & Chr(10) & Chr(10)
        strInfo &= "Regardless of the calculation the tab room uses, however, in any ordinal system a team who places low-commitment judges in their better ranking spots runs an increased risk that those rounds of judging will be needed elsewhere or will already be used by the time later rounds occur, and the tab room will be forced to move lower down the list to find a mutally acceptable judge. The unit percentile column gives you a better idea of where your judge ranks in comparison to the overall number of judging units in the pool." & Chr(10) & Chr(10)
        MsgBox(strInfo)
    End Sub

    Private Sub butOnlineUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butOnlineUpdate.Click
        Call PassWordCheck()
        butHideGrid.Visible = False : DataGridView2.Visible = False
        Dim strDivisions As String = ""
        If chkSuppressDownload.Checked = False Then
            For x = 0 To ds.Tables("Event").Rows.Count - 1
                If strDivisions <> "" Then strDivisions &= ","
                strDivisions &= ds.Tables("Event").Rows(x).Item("ID").ToString.Trim
            Next x
            Dim URL As String = "https://www.tabroom.com/api/download_tourn.mhtml?username=" & My.Settings.UserName & "&password=" & My.Settings.PassWord & "&tourn_id=" & ds.Tables("Tourn").Rows(0).Item("ID") & "&event_id=" & strDivisions
            lblMessage.Text = "Opening site...File downloading...." : lblMessage.Refresh()
            Try
                Dim request As HttpWebRequest = WebRequest.Create(URL)
                Dim response As HttpWebResponse = request.GetResponse()
                Dim reader As StreamReader = New StreamReader(response.GetResponseStream())
                Dim st As StreamWriter = File.CreateText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\CAT\PrefUpdate.xml")
                st.WriteLine(reader.ReadToEnd())
                st.Close()
                response.Close() : reader.Close()
            Catch
                lblMessage.Text = "Download of data file failed. Update aborted.  Check your connection."
                Exit Sub
            End Try
            lblMessage.Text = "Download Complete!"
        End If
        Dim dtErr As New DataTable
        dtErr.Columns.Add(("Type"), System.Type.GetType("System.String"))
        dtErr.Columns.Add(("Team"), System.Type.GetType("System.String"))
        dtErr.Columns.Add(("Judge"), System.Type.GetType("System.String"))
        dtErr.Columns.Add(("Rating"), System.Type.GetType("System.String"))
        Dim drErr As DataRow
        'open new file
        Dim ds2 As New DataSet
        Dim xmlFile As XmlReader
        xmlFile = XmlReader.Create(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\CAT\PrefUpdate.xml", New XmlReaderSettings())
        ds2.ReadXml(xmlFile, XmlReadMode.InferSchema)
        xmlFile.Close()
        'clean it; eliminate dupes and put in 999 for ordpct
        Call CleanDownloadedPrefs(ds2.Tables("JudgePref"))
        'scroll and update
        Dim strOver As String = "" : Dim drOvJud, drOvTm As DataRow
        Dim dr As DataRow()
        Dim fdCATPref As DataRow()
        If chkUpdateDisplayedOnly.Checked = True Then
            fdCATPref = ds.Tables("JudgePref").Select("Team=" & cboTeam.SelectedValue)
        Else
            fdCATPref = ds.Tables("JudgePref").Select("ID>-1")
        End If
        Dim nUnder, nOver As Integer
        For x = 0 To fdCATPref.Length - 1
            lblMessage.Text = "Updating " & x & " of " & fdCATPref.Length - 1 : lblMessage.Refresh()
            dr = ds2.Tables("JudgePref").Select("Judge=" & fdCATPref(x).Item("Judge") & " and team=" & fdCATPref(x).Item("Team"), "OrdPct DESC")
            If dr.Length = 0 Then
                nUnder += 1
                drErr = dtErr.NewRow
                drOvJud = ds.Tables("Judge").Rows.Find(fdCATPref(x).Item("Judge"))
                drOvTm = ds.Tables("Entry").Rows.Find(fdCATPref(x).Item("Team"))
                drErr.Item("Type") = "In CAT/not online"
                drErr.Item("Judge") = drOvJud.Item("Last")
                drErr.Item("Team") = drOvTm.Item("Code")
                drErr.Item("Rating") = fdCATPref(x).Item("Rating")
                dtErr.Rows.Add(drErr)
            End If
            If dr.Length > 1 Then
                nOver += 1
                For y = 0 To dr.Length - 1
                    drErr = dtErr.NewRow
                    drOvJud = ds.Tables("Judge").Rows.Find(dr(y).Item("Judge"))
                    drOvTm = ds.Tables("Entry").Rows.Find(dr(y).Item("Team"))
                    drErr.Item("Type") = "Duplicates in online file"
                    drErr.Item("Judge") = drOvJud.Item("Last")
                    drErr.Item("Team") = drOvTm.Item("Code")
                    drErr.Item("Rating") = dr(y).Item("Rating")
                    dtErr.Rows.Add(drErr)
                Next y
            End If
            If dr.Length = 1 Then
                fdCATPref(x).Item("Rating") = dr(0).Item("Rating")
                If dr(0).Item("OrdPct") Is System.DBNull.Value Then dr(0).Item("OrdPct") = 0
                If Val(dr(0).Item("OrdPct")) > 0 Then
                    fdCATPref(x).Item("OrdPct") = dr(0).Item("OrdPct")
                End If
            End If
        Next x
        lblMessage.Text = "Done. If there are pref entries in the CAT that are not online and they are conflicts/strikes (999 ratings), that is normal.  If there is a rating of zero, you may wish to contact the team and ask them to provide a rating for the judge.  Click the help button for more information on how to handle ratings of zero."
        If nUnder > 0 Then lblMessage.Text &= nUnder & " pref entries in CAT not found online. "
        If nOver > 0 Then lblMessage.Text &= nUnder & " multiple pref entries for same team and judge in online file. "
        DataGridView3.DataSource = dtErr : DataGridView3.Visible = True : butHideGrid.Visible = True
        DataGridView3.Columns("Type").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
    End Sub
    Sub CleanDownloadedPrefs(ByRef dt As DataTable)
        'adds 999 to ordpct for struck teams, eliminates dupes giving the highest entered rating
        Dim hiPref As Integer
        'find dupes, mark dupes as -999
        dt.DefaultView.Sort = "Team ASC, Judge ASC, rating DESC"
        For x = 0 To dt.DefaultView.Count - 2
            'If dt.DefaultView(x).Item("Judge") = 109102 And dt.DefaultView(x).Item("Team") = 346865 Then MsgBox("Crap")
            lblMessage.Text = x & " of " & dt.DefaultView.Count - 1 : lblMessage.Refresh()
            If dt.DefaultView(x).Item("Rating") = 999 Then dt.DefaultView(x).Item("OrdPct") = 999
            If dt.DefaultView(x).Item("Judge") = dt.DefaultView(x + 1).Item("Judge") And dt.DefaultView(x).Item("Team") = dt.DefaultView(x + 1).Item("Team") Then
                If dt.DefaultView(x).Item("Rating") > hiPref Then hiPref = dt.DefaultView(x).Item("Rating")
                If dt.DefaultView(x + 1).Item("Rating") > hiPref Then hiPref = dt.DefaultView(x + 1).Item("Rating")
                If dt.DefaultView(x).Item("Rating") < hiPref Then dt.DefaultView(x).Item("Rating") = -999
                If dt.DefaultView(x + 1).Item("Rating") < hiPref Then dt.DefaultView(x + 1).Item("Rating") = -999
                If dt.DefaultView(x).Item("Rating") = dt.DefaultView(x + 1).Item("Rating") Then dt.DefaultView(x).Item("Rating") = -999
            End If
            If dt.DefaultView(x).Item("Judge") <> dt.DefaultView(x + 1).Item("Judge") Or dt.DefaultView(x).Item("Team") <> dt.DefaultView(x + 1).Item("Team") Then
                hiPref = 0
            End If
        Next x
        'delete all -999 values
        lblMessage.Text = "Deleting Duplicates " : lblMessage.Refresh()
        For x = dt.Rows.Count - 1 To 0 Step -1
            If dt.Rows(x).Item("Rating") = -999 Then dt.Rows(x).Delete()
        Next x
        dt.AcceptChanges()
    End Sub
End Class