Imports System.IO
Imports System.Windows.Forms.HtmlElementCollection

Public Class frmRegPrint
    Dim ds As New DataSet
    Dim EnableEvents As Boolean

    Private Sub frmTRPCPair_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        EnableEvents = False

        Call LoadFile(ds, "TourneyData")

        cboRound.DataSource = ds.Tables("Round")
        cboRound.DisplayMember = "Label"
        cboRound.ValueMember = "ID"

        EnableEvents = True
    End Sub
    Private Sub frmTRPCPair_Unload(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Disposed
        Call SaveFile(ds)
        ds.Dispose()
    End Sub

    Private Sub butRegSheeets_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butRegSheets.Click
        Call MakeRegSheets()
        lblStatus.Text = "Data all loaded...waiting on the printer...."
        lblStatus.Refresh()
        Dim strFile = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\CAT\RegSheets.html"
        WebBrowser1.Navigate(strFile)
    End Sub
    Sub MakeRegSheets()
        Dim strBreakline = "<p style=" & Chr(34) & "page-break-before: always" & Chr(34) & ">"
        Dim j As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\CAT\RegSheets.html"
        Dim st As StreamWriter = File.CreateText(j)
        Dim fdEntries, fdJudges As DataRow() : Dim dr As DataRow : Dim rdsHave, rdsOwed As Integer
        st.WriteLine("<HTML>")
        st.WriteLine("<HEAD>")
        st.WriteLine("<style type=" & Chr(34) & "text/css" & Chr(34) & ">")
        st.WriteLine("table, th, td")
        st.WriteLine("{")
        st.WriteLine("font-family:" & Chr(34) & "Arial" & Chr(34) & ";")
        st.WriteLine("border:1px solid black;")
        st.WriteLine("border-collapse:collapse;")
        st.WriteLine("padding:3px 7px 2px 7px;")
        st.WriteLine("}")
        st.WriteLine("body")
        st.WriteLine("{")
        st.WriteLine("font-family:" & Chr(34) & "Arial" & Chr(34) & ";")
        st.WriteLine("font-size:18;")
        st.WriteLine("font-weight:bold;")
        st.WriteLine("}")
        st.WriteLine("div.Regular")
        st.WriteLine("{")
        st.WriteLine("font-family:" & Chr(34) & "Arial" & Chr(34) & ";")
        st.WriteLine("font-size:14;")
        st.WriteLine("font-weight:normal;")
        st.WriteLine("}")
        st.WriteLine("</style>")
        st.WriteLine("</HEAD>")
        For x = 0 To ds.Tables("School").Rows.Count - 1
            rdsOwed = 0 : rdsHave = 0
            lblStatus.Text = x & " of " & ds.Tables("School").Rows.Count - 1 : lblStatus.Refresh()
            fdEntries = ds.Tables("Entry").Select("School=" & ds.Tables("School").Rows(x).Item("ID"))
            fdJudges = ds.Tables("Judge").Select("School=" & ds.Tables("School").Rows(x).Item("ID"))
            If fdEntries.Length > 0 Or fdJudges.Length > 0 Then
                'print top of page
                st.WriteLine("<center>" & ds.Tables("Tourn").Rows(0).Item("TournName").trim & "</center><br>")
                st.WriteLine("<Center>" & ds.Tables("Tourn").Rows(0).Item("StartDate").trim & " through " & ds.Tables("Tourn").Rows(0).Item("EndDate").trim & "</center><br>")
                st.WriteLine("Registration for " & ds.Tables("School").Rows(x).Item("SchoolName").trim & "<br><br>")
                'print teams
                If fdEntries.Length > 0 Then
                    st.WriteLine("<TABLE>")
                    st.WriteLine("<THEAD>")
                    st.WriteLine("<TR>")
                    st.WriteLine("<TH>Team</TH>")
                    st.WriteLine("<TH>Event</TH>")
                    st.WriteLine("<TH>ADA</TH>")
                    st.WriteLine("<TH>Notes</TH>")
                    st.WriteLine("</TR>")
                    st.WriteLine("</THEAD>")
                    For y = 0 To fdEntries.Length - 1
                        st.WriteLine("<TR>")
                        st.WriteLine("<TD>" & fdEntries(y).Item("FullName") & "</TD>")
                        dr = ds.Tables("Event").Rows.Find(fdEntries(y).Item("Event"))
                        st.WriteLine("<TD>" & dr.Item("EventName") & "</TD>")
                        st.WriteLine("<TD>" & fdEntries(y).Item("ADA") & "</TD>")
                        st.WriteLine("<TD>" & fdEntries(y).Item("Notes") & "</TD>")
                        st.WriteLine("</TR>")
                        rdsOwed += getEventSetting(ds, dr.Item("ID"), "nPrelims") / getEventSetting(ds, dr.Item("ID"), "TeamsPerRound")
                    Next y
                    st.WriteLine("</TABLE><br>")
                End If
                'print judges
                If fdJudges.Length > 0 Then
                    st.WriteLine("<TABLE>")
                    st.WriteLine("<THEAD>")
                    st.WriteLine("<TR>")
                    st.WriteLine("<TH>Last Name</TH>")
                    st.WriteLine("<TH>First Name</TH>")
                    st.WriteLine("<TH>Obligation</TH>")
                    st.WriteLine("<TH>Hired</TH>")
                    st.WriteLine("<TH>Notes</TH>")
                    st.WriteLine("</TR>")
                    st.WriteLine("</THEAD>")
                    For y = 0 To fdJudges.Length - 1
                        st.WriteLine("<TR>")
                        st.WriteLine("<TD>" & fdJudges(y).Item("Last") & "</TD>")
                        st.WriteLine("<TD>" & fdJudges(y).Item("First") & "</TD>")
                        st.WriteLine("<TD>" & fdJudges(y).Item("Obligation") & "</TD>")
                        st.WriteLine("<TD>" & fdJudges(y).Item("Hired") & "</TD>")
                        st.WriteLine("<TD>" & fdJudges(y).Item("Notes") & "</TD>")
                        st.WriteLine("</TR>")
                        rdsHave += fdJudges(y).Item("Obligation") + fdJudges(y).Item("Hired")
                    Next y
                    st.WriteLine("</TABLE>")
                End If
                'print notes
                st.WriteLine("<BR><div class=" & Chr(34) & "Regular" & Chr(34) & ">School owes " & rdsOwed & " rounds of judging and has provided " & rdsHave & "</div>")
                'add hard page break
                st.WriteLine(strBreakline)
            End If
        Next x
        st.WriteLine("</HTML>")
        st.Close()
    End Sub

    Private Sub butPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butPrint.Click
        'WebBrowser1.Print()
        WebBrowser1.ShowPrintDialog()
    End Sub

    Private Sub butPrefConfirm_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butPrefConfirm.Click
        Call MakePrefCheckerSheets()
        Dim strFile = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\CAT\PrefCheckerSheets.html"
        WebBrowser1.Navigate(strFile)
    End Sub
    Sub MakePrefCheckerSheets()
        Dim strBreakline = "<p style=" & Chr(34) & "page-break-before: always" & Chr(34) & ">"
        Dim j As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\CAT\PrefCheckerSheets.html"
        Dim st As StreamWriter = File.CreateText(j)
        st.WriteLine("<HTML>")
        st.WriteLine("<HEAD>")
        st.WriteLine("<style type=" & Chr(34) & "text/css" & Chr(34) & ">")
        st.WriteLine("table, th, td")
        st.WriteLine("{")
        st.WriteLine("font-family:" & Chr(34) & "Arial" & Chr(34) & ";")
        st.WriteLine("font-size:8;")
        st.WriteLine("border:1px solid black;")
        st.WriteLine("border-collapse:collapse;")
        st.WriteLine("padding:2px 2px 2px 2px;")
        st.WriteLine("}")
        st.WriteLine("body")
        st.WriteLine("{")
        st.WriteLine("font-family:" & Chr(34) & "Arial" & Chr(34) & ";")
        st.WriteLine("font-size:8;")
        st.WriteLine("font-weight:normal;")
        st.WriteLine("}")
        st.WriteLine("div.Heading")
        st.WriteLine("{")
        st.WriteLine("font-family:" & Chr(34) & "Arial" & Chr(34) & ";")
        st.WriteLine("font-size:12;")
        st.WriteLine("font-weight:bold;")
        st.WriteLine("}")
        st.WriteLine("</style>")
        st.WriteLine("</HEAD>")
        Dim fdprefs As DataRow() : Dim nRows As Integer : Dim dr, drSchool As DataRow
        For x = 0 To ds.Tables("Entry").Rows.Count - 1
            lblStatus.Text = x & " of " & ds.Tables("Entry").Rows.Count - 1 : lblStatus.Refresh()
            fdprefs = ds.Tables("JudgePref").Select("Team=" & ds.Tables("Entry").Rows(x).Item("ID"), "Rating ASC")
            drSchool = ds.Tables("School").Rows.Find(ds.Tables("entry").Rows(x).Item("School"))
            If fdprefs.Length > 0 Then
                'print top of page
                st.WriteLine("<Center><div class=" & Chr(34) & "Heading" & Chr(34) & ">Pref checker sheet for " & drSchool.Item("Code") & " " & ds.Tables("Entry").Rows(x).Item("Fullname").trim & "</div></center><br>")
                st.WriteLine("<TABLE>")
                st.WriteLine("<THEAD>")
                st.WriteLine("<TR>")
                st.WriteLine("<TH>Judge</TH>")
                st.WriteLine("<TH>Rating</TH>")
                st.WriteLine("<TH>Ord%</TH>")
                st.WriteLine("<TH>Judge</TH>")
                st.WriteLine("<TH>Rating</TH>")
                st.WriteLine("<TH>Ord%</TH>")
                st.WriteLine("<TH>Judge</TH>")
                st.WriteLine("<TH>Rating</TH>")
                st.WriteLine("<TH>Ord%</TH>")
                st.WriteLine("<TH>Judge</TH>")
                st.WriteLine("<TH>Rating</TH>")
                st.WriteLine("<TH>Ord%</TH>")
                st.WriteLine("</TR>")
                st.WriteLine("</THEAD>")
                nRows = Int(fdprefs.Length / 4)
                For y = 0 To nRows
                    st.WriteLine("<TR>")
                    dr = ds.Tables("Judge").Rows.Find(fdprefs(y).Item("Judge"))
                    st.WriteLine("<TD>" & dr.Item("Last").trim & ", " & dr.Item("First").trim & "</TD>")
                    st.WriteLine("<TD>" & fdprefs(y).Item("Rating") & "</TD>")
                    st.WriteLine("<TD>" & fdprefs(y).Item("OrdPct") & "</TD>")
                    'col 2
                    dr = ds.Tables("Judge").Rows.Find(fdprefs(y + nRows + 1).Item("Judge"))
                    If Not dr Is Nothing Then
                        st.WriteLine("<TD>" & dr.Item("Last").trim & ", " & dr.Item("First").trim & "</TD>")
                        st.WriteLine("<TD>" & fdprefs(y + nRows + 1).Item("Rating") & "</TD>")
                        st.WriteLine("<TD>" & fdprefs(y + nRows + 1).Item("OrdPct") & "</TD>")
                    End If
                    'col 3
                    If (y + (nRows * 2) + 2) < fdprefs.Length Then
                        dr = ds.Tables("Judge").Rows.Find(fdprefs(y + (nRows * 2) + 2).Item("Judge"))
                        st.WriteLine("<TD>" & dr.Item("Last").trim & ", " & dr.Item("First").trim & "</TD>")
                        st.WriteLine("<TD>" & fdprefs(y + (nRows * 2) + 2).Item("Rating") & "</TD>")
                        st.WriteLine("<TD>" & fdprefs(y + (nRows * 2) + 2).Item("OrdPct") & "</TD>")
                    End If
                    'col 4
                    If (y + (nRows * 3) + 3) < fdprefs.Length Then
                        dr = ds.Tables("Judge").Rows.Find(fdprefs(y + (nRows * 3) + 3).Item("Judge"))
                        st.WriteLine("<TD>" & dr.Item("Last").trim & ", " & dr.Item("First").trim & "</TD>")
                        st.WriteLine("<TD>" & fdprefs(y + (nRows * 3) + 3).Item("Rating") & "</TD>")
                        st.WriteLine("<TD>" & fdprefs(y + (nRows * 3) + 3).Item("OrdPct") & "</TD>")
                    End If
                    st.WriteLine("</TR>")
                Next y
                st.WriteLine("</TABLE>")
            End If
            'add hard page break
            st.WriteLine(strBreakline)
        Next x
        st.Close()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim drRd, drTimeSlot As DataRow
        drRd = ds.Tables("Round").Rows.Find(cboRound.SelectedValue)
        drTimeSlot = ds.Tables("TimeSlot").Rows.Find(drRd.Item("Timeslot"))
        If drTimeSlot.Item("StartTime") Is System.DBNull.Value Then drTimeSlot.Item("StartTime") = Now.AddMinutes(15)
        Dim dtDummy As DateTime
        Try
            dtDummy = "#" & drTimeSlot.Item("StartTime").ToString & "#"
        Catch
        End Try
        Dim strStartTime As String = InputBox("Enter Start Time:", "Start Time", dtDummy.ToString("t"))
        Dim strSortBy As String = "JUDGE" : If radRoom.Checked = True Then strSortBy = "ROOM"
        Call MakeBallotFile(ds, cboRound.SelectedValue, strStartTime, strSortBy, chkIsElim.Checked)
        Dim strFile = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\CAT\Ballots.html"
        WebBrowser1.Navigate(strFile)
    End Sub

    Private Sub butDisasterPlan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butDisasterPlan.Click
        Dim drRd, drTimeSlot As DataRow
        drRd = ds.Tables("Round").Rows.Find(cboRound.SelectedValue)
        drTimeSlot = ds.Tables("TimeSlot").Rows.Find(drRd.Item("Timeslot"))
        If drTimeSlot.Item("StartTime") Is System.DBNull.Value Then drTimeSlot.Item("StartTime") = Now.AddMinutes(15)
        Dim dtDummy As DateTime = "#" & drTimeSlot.Item("StartTime") & "#"
        Dim strStartTime As String = InputBox("Enter Start Time:", "Start Time", dtDummy.ToString("t"))
        Dim strSortBy As String = "JUDGE" : If radRoom.Checked = True Then strSortBy = "ROOM"
        Call MakeEmailBallotFile(cboRound.SelectedValue, strStartTime, strSortBy)
        Dim strFile = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\CAT\Ballots.html"
        WebBrowser1.Navigate(strFile)

    End Sub
    Sub MakeEmailBallotFile(ByVal Round As Integer, ByVal strStartTime As String, ByVal strSortBy As String)
        Dim strBreakline = "<p style=" & Chr(34) & "page-break-before: always" & Chr(34) & ">"
        Dim j As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\CAT\Ballots.html"
        Dim st As StreamWriter = File.CreateText(j)
        st.WriteLine("<HTML>")
        st.WriteLine("<HEAD>")
        st.WriteLine("<style type=" & Chr(34) & "text/css" & Chr(34) & ">")
        st.WriteLine("table, th, td")
        st.WriteLine("{")
        st.WriteLine("font-family:" & Chr(34) & "Arial" & Chr(34) & ";")
        st.WriteLine("font-size:12;")
        st.WriteLine("border:1px solid black;")
        st.WriteLine("border-collapse:collapse;")
        st.WriteLine("padding:3px 7px 2px 7px;")
        st.WriteLine("}")
        st.WriteLine("body")
        st.WriteLine("{")
        st.WriteLine("font-family:" & Chr(34) & "Arial" & Chr(34) & ";")
        st.WriteLine("font-size:12;")
        st.WriteLine("font-weight:normal;")
        st.WriteLine("}")
        st.WriteLine("div.Heading")
        st.WriteLine("{")
        st.WriteLine("font-family:" & Chr(34) & "Arial" & Chr(34) & ";")
        st.WriteLine("font-size:18;")
        st.WriteLine("font-weight:bold;")
        st.WriteLine("}")
        st.WriteLine("</style>")
        st.WriteLine("</HEAD>")
        'build the tables
        'set up the tables with a first column that is always recipient name
        Dim dtTeam, dtSpeakers As New DataTable
        dtTeam.Columns.Add("Recipient", System.Type.GetType("System.String"))
        dtSpeakers.Columns.Add("Recipient", System.Type.GetType("System.String"))
        'Load the TB_SET for the round, and add a column to the appropriate table for each scores needed
        'also, add the range to the description field.
        Dim drRound As DataRow : drRound = Ds.Tables("Round").Rows.Find(Round)
        Dim drSet, drTB As DataRow() : drTB = Ds.Tables("TieBreak").Select("TB_SET=" & drRound.Item("TB_SET"))
        Dim drScore As DataRow
        Dim strScoreRange As String = "<b>Instructions:</b> "
        Dim hasBallot As Boolean = False
        For x = 0 To drTB.Length - 1
            drScore = Ds.Tables("Scores").Rows.Find(drTB(x).Item("ScoreID"))
            drSet = Ds.Tables("Score_Setting").Select("TB_SET=" & drRound.Item("TB_SET") & " and SCORE=" & drScore.Item("ID"))
            If drScore.Item("ID") = 1 Then hasBallot = True
            If drScore.Item("ID") > 1 Then
                If drScore.Item("ScoreFor").toupper = "TEAM" And dtTeam.Columns.Contains(drScore.Item("Score_Name")) = False Then
                    dtTeam.Columns.Add(drScore.Item("Score_Name"), System.Type.GetType("System.String"))
                    strScoreRange &= "For each team enter a " & drScore.Item("Score_Name") & " score between " & drSet(0).Item("Min") & " and " & drSet(0).Item("Max") & ". "
                    If drSet(0).Item("DupesOK") = False Then strScoreRange &= "Do not enter duplicate scores for " & drScore.Item("Score_Name") & ". "
                End If
                If drScore.Item("ScoreFor").toupper = "SPEAKER" And dtSpeakers.Columns.Contains(drScore.Item("Score_Name")) = False Then
                    dtSpeakers.Columns.Add(drScore.Item("Score_Name"), System.Type.GetType("System.String"))
                    strScoreRange &= "For each speaker enter a " & drScore.Item("Score_Name") & " score between " & drSet(0).Item("Min") & " and " & drSet(0).Item("Max") & ". "
                    If drSet(0).Item("DupesOK") = False Then strScoreRange &= "Do not enter duplicate scores for " & drScore.Item("Score_Name") & ". "
                End If
            End If
        Next x
        'Find the judge, pull all ballots for judge
        Dim fdPanel, fdBallots As DataRow()
        fdPanel = Ds.Tables("Panel").Select("Round=" & Round)
        'Load/build judge ballots and sort
        Dim dtJudges As New DataTable
        dtJudges.Columns.Add("Judge", System.Type.GetType("System.String"))
        dtJudges.Columns.Add("JudgeName", System.Type.GetType("System.String"))
        dtJudges.Columns.Add("Room", System.Type.GetType("System.Int16"))
        Dim nTeams As Integer = getEventSetting(Ds, drRound.Item("Event"), "TeamsPerRound")
        Dim nspkrs As Integer = getEventSetting(Ds, drRound.Item("Event"), "DebatersPerTeam")
        Dim arrSpkrs(nTeams, nspkrs) As String
        For x = 1 To nTeams
            dtJudges.Columns.Add("Ballot" & x, System.Type.GetType("System.Int32"))
            dtJudges.Columns.Add("Side" & x, System.Type.GetType("System.Int32"))
        Next x
        Dim dr, drJudge As DataRow : Dim z, w As Integer
        dr = dtJudges.NewRow
        For x = 0 To fdPanel.Length - 1
            fdBallots = Ds.Tables("Ballot").Select("Panel=" & fdPanel(x).Item("ID"), "Judge ASC")
            For y = 0 To fdBallots.Length - 1
                If fdBallots(y).Item("Judge") > -1 Then
                    dr.Item("Judge") = fdBallots(y).Item("Judge")
                    drJudge = Ds.Tables("Judge").Rows.Find(fdBallots(y).Item("Judge"))
                    dr.Item("JudgeName") = drJudge.Item("Last").trim & ", " & drJudge.Item("First").trim & " " & drJudge.Item("Email")
                    z = z + 1
                    dr.Item("Ballot" & z) = fdBallots(y).Item("Entry")
                    dr.Item("Side" & z) = fdBallots(y).Item("Side")
                    dr.Item("Room") = fdPanel(x).Item("Room")
                    If z = nTeams Then
                        z = 0 : dtJudges.Rows.Add(dr) : dr = dtJudges.NewRow
                    End If
                End If
            Next y
        Next x
        dtJudges.DefaultView.Sort = "JudgeName ASC"
        If strSortBy = "ROOM" Then dtJudges.DefaultView.Sort = "Room ASC"
        'Figure whether you need team scores; if so, print a team scores table
        'Figure whether you need speaker scores; if so, print a speaker scores table
        st.WriteLine("<HTML>")
        Dim drTeam, drRoom As DataRow : Dim fdSpeakers As DataRow() : Dim strTH As String
        For x = 0 To dtJudges.DefaultView.Count - 1
            st.WriteLine("TO ENTER YOUR BALLOT, reply to this email.  Look below the dashed line below, and fill in the areas in ALL CAPS (points and ranks, and indicate a winner).")
            drRoom = ds.Tables("Room").Rows.Find(dtJudges.DefaultView(x).Item("Room"))
            st.WriteLine("<div class=" & Chr(34) & "Heading" & Chr(34) & ">" & dtJudges.DefaultView(x).Item("JudgeName") & "</DIV>")
            st.WriteLine("<b>Room:</b>" & drRoom.Item("RoomName") & Chr(9) & Chr(9) & "<b>Start time:</b>" & strStartTime & "<br>")
            'st.WriteLine("Ballot code:" & Ds.Tables("Tourn").Rows(0).Item("ID") & "-" & Round & "-" & dtJudges.DefaultView(x).Item("Judge"))
            st.WriteLine("<div class=" & Chr(34) & "Heading" & Chr(34) & ">" & ds.Tables("Tourn").Rows(0).Item("TournName").trim & "</div><br>")
            st.WriteLine("<div class=" & Chr(34) & "Heading" & Chr(34) & ">" & drRound.Item("Label") & "</DIV>")
            st.WriteLine("<br>" & strScoreRange & "<br><hr/><br>")
            If dtTeam.Columns.Count > 1 Then
                st.WriteLine("<TABLE>")
                For y = 1 To nTeams
                    drTeam = ds.Tables("Entry").Rows.Find(dtJudges.DefaultView(x).Item("Ballot" & y))
                    st.WriteLine("<TR>")
                    st.WriteLine("<TD>" & GetSideString(ds, dtJudges.DefaultView(x).Item("Side" & y), drRound.Item("Event")) & "</td>")
                    st.WriteLine("<TD>" & drTeam.Item("FullName") & "</td>")
                    For z = 1 To dtTeam.Columns.Count - 1
                        st.WriteLine("<td>" & "    " & "</Td>")
                    Next z
                    st.WriteLine("</tr>")
                Next y
                st.WriteLine("</TABLE><br>")
            End If
            If dtSpeakers.Columns.Count > 1 Then
                For y = 1 To nTeams
                    drTeam = ds.Tables("Entry").Rows.Find(dtJudges.DefaultView(x).Item("Ballot" & y))
                    st.WriteLine("<center><TH colspan=" & Chr(34) & "3" & Chr(34) & ">" & GetSideString(ds, dtJudges.DefaultView(x).Item("Side" & y), drRound.Item("Event")) & ": " & drTeam.Item("FullName") & "</TH></center>")
                Next y
                'fill the speaker array
                For y = 1 To nTeams
                    fdSpeakers = ds.Tables("Entry_Student").Select("Entry=" & dtJudges.DefaultView(x).Item("Ballot" & y))
                    For w = 0 To fdSpeakers.Length - 1
                        arrSpkrs(y, w) = fdSpeakers(w).Item("Last").trim & ", " & fdSpeakers(w).Item("First").trim & "    POINTS:   RANK:" & "<br>"
                    Next w
                Next y
                For y = 0 To nspkrs - 1
                    For z = 1 To nTeams
                        st.WriteLine("<td>" & arrSpkrs(z, y) & "</Td>")
                        For w = 1 To dtSpeakers.Columns.Count - 1
                            st.WriteLine("<td>" & "    " & "</Td>")
                        Next w
                    Next z
                Next y
            End If
            st.WriteLine("WINNER:")
            st.WriteLine("<hr/>")
            'add hard return
            st.WriteLine(strBreakline)
        Next x
        st.WriteLine("</HTML>")
        st.Close()
    End Sub
    Sub AutoElimSetter() Handles cboRound.SelectedValueChanged
        If EnableEvents = False Then Exit Sub
        Dim dr As DataRow
        dr = ds.Tables("Round").Rows.Find(cboRound.SelectedValue)
        If dr.Item("Rd_Name") > 9 Then chkIsElim.Checked = True
    End Sub
    Sub ClearGrid() Handles cboRound.SelectedValueChanged
        WebBrowser1.DocumentText = ""
    End Sub

    Private Sub butStrikeCards_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butStrikeCards.Click
        Dim drRd As DataRow
        drRd = ds.Tables("Round").Rows.Find(cboRound.SelectedValue)
        If drRd.Item("JudgesPerPanel") < 2 Then MsgBox("This function is only for panels with 2 or mroe judges.") : Exit Sub
        Call MakeStrikeCards(ds, cboRound.SelectedValue)
        Dim strFile = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\CAT\StrikeCards.html"
        WebBrowser1.Navigate(strFile)
    End Sub
    Sub MakeStrikeCards(ByVal ds, ByVal RoundID)
        Dim strInstr As String = InputBox("Enter instructions that appear on the strike cards.  Edit text as necessary in the box below:", "Strike card language", "You may strike zero, one, two, or three judges.  If you wish to strike a judge, draw a line through the name.  Please return your sheet where you picked it up within five minutes.")
        'Set up the html doc
        Dim strBreakline = "<p style=" & Chr(34) & "page-break-before: always" & Chr(34) & ">"
        Dim j As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\CAT\StrikeCards.html"
        Dim st As StreamWriter = File.CreateText(j)
        st.WriteLine("<HTML>")
        st.WriteLine("<HEAD>")
        st.WriteLine("<style type=" & Chr(34) & "text/css" & Chr(34) & ">")
        st.WriteLine("table, th, td")
        st.WriteLine("{")
        st.WriteLine("font-family:" & Chr(34) & "Arial" & Chr(34) & ";")
        st.WriteLine("font-size:12;")
        st.WriteLine("border:1px solid black;")
        st.WriteLine("border-collapse:collapse;")
        st.WriteLine("padding:3px 7px 2px 7px;")
        st.WriteLine("}")
        st.WriteLine("body")
        st.WriteLine("{")
        st.WriteLine("font-family:" & Chr(34) & "Arial" & Chr(34) & ";")
        st.WriteLine("font-size:12;")
        st.WriteLine("font-weight:normal;")
        st.WriteLine("}")
        st.WriteLine("div.Heading")
        st.WriteLine("{")
        st.WriteLine("font-family:" & Chr(34) & "Arial" & Chr(34) & ";")
        st.WriteLine("font-size:18;")
        st.WriteLine("font-weight:bold;")
        st.WriteLine("}")
        st.WriteLine("</style>")
        st.WriteLine("</HEAD>")
        'load the round
        Dim dt As DataTable
        st.WriteLine("<HTML>")
        dt = MakePairingTable(ds, RoundID, "FULL")
        For x = 0 To dt.Rows.Count - 1
            For y = 1 To 2
                st.WriteLine("<b>Strike card for " & dt.Rows(x).Item("TeamName" & y) & "</b><br><br>")
                st.WriteLine(strInstr & "<br><br>")
                For z = 0 To dt.Columns.Count - 1
                    If Mid(dt.Columns(z).ColumnName.ToString, 1, 9).ToUpper = "JUDGENAME" Then
                        st.WriteLine(dt.Rows(x).Item(z) & "<br>")
                    End If
                Next z
                st.WriteLine(strBreakline)
            Next y
        Next x
        st.WriteLine("</HTML>")
        st.Close()
    End Sub
End Class