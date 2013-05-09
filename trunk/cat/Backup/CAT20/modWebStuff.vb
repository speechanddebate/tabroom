Imports System.IO
Imports System.Xml
Imports System
Imports System.Net
Imports System.Text
Imports System.Collections.Specialized
Imports System.IO.Packaging

Module modWebStuff
    Sub MakeTableWithResults(ByVal ds As DataSet, ByVal strWriteType As String)
        'first, fix full team names to strip ampersand
        For x = 0 To ds.Tables("Entry").Rows.Count - 1
            ds.Tables("Entry").Rows(x).Item("Fullname") = FullNameMaker(ds, ds.Tables("Entry").Rows(x).Item("ID"), "FULL", 1)
        Next x
        'go on
        Dim strFileName As String = Replace(strFilePath, "TourneyData.xml", "TourneyDataResults.xml")
        Dim STW As New StreamWriter(strFileName)
        STW.WriteLine("<?xml version=""1.0""?>")
        STW.WriteLine("<TOURNAMENTRESULTS>")
        If strWriteType <> "ALL" Then
            Call WriteTable(ds, STW, "TOURN")
            Call WriteTable(ds, STW, "TOURN_SETTING")
            Call WriteTable(ds, STW, "ENTRY")
            Call WriteTable(ds, STW, "ENTRY_STUDENT")
            Call WriteTable(ds, STW, "JUDGE")
            Call WriteTable(ds, STW, "EVENT")
            Call WriteTable(ds, STW, "EVENT_SETTING")
            Call WriteTable(ds, STW, "SCHOOL")
            Call WriteTable(ds, STW, "ROUND")
            Call WriteTable(ds, STW, "ROOM")
            Call WriteTable(ds, STW, "TIMESLOT")
        Else
            For x = 0 To ds.Tables.Count - 1
                Call WriteTable(ds, STW, ds.Tables(x).TableName.ToUpper)
            Next x
        End If
        For x = 0 To ds.Tables("Event").Rows.Count - 1
            Call WriteFinalRank(ds, STW, ds.Tables("Event").Rows(x).Item("ID"), "TEAM")
            Call WriteFinalRank(ds, STW, ds.Tables("Event").Rows(x).Item("ID"), "SPEAKER")
            Call WriteRoundByRound(ds, STW, ds.Tables("Event").Rows(x).Item("ID"))
        Next x
        STW.WriteLine("</TOURNAMENTRESULTS>")
        STW.Close()
    End Sub
    Sub WriteTable(ByVal ds As DataSet, ByVal stw As StreamWriter, ByVal strTable As String)
        'writes a flat, 2-d table from the dataset

        'strip unwanted fields -- NOT SURE why this was here, but I do want to export this info to TRPC
        Dim dt As New DataTable
        dt = ds.Tables(strTable)
        'If strTable = "ENTRY" Then dt.Columns.Remove("ADA") : dt.Columns.Remove("NOTES") : dt.AcceptChanges()

        Dim x, y As Integer
        Dim dummy As String
        For x = 0 To ds.Tables(strTable).Rows.Count - 1
            stw.WriteLine("  <" & dt.TableName.ToUpper & ">")
            For y = 0 To ds.Tables(strTable).Columns.Count - 1
                dummy = "    <" & dt.Columns(y).ColumnName.ToUpper.Trim & ">" & dt.Rows(x).Item(y).ToString.Trim & "</" & dt.Columns(y).ColumnName.ToUpper & ">"
                stw.WriteLine(dummy)
            Next y
            stw.WriteLine("  </" & dt.TableName.ToUpper & ">")
        Next x
    End Sub
    Sub WriteFinalRank(ByVal ds As DataSet, ByVal stw As StreamWriter, ByVal EventID As Integer, ByVal strResultType As String)

        Dim x, y As Integer
        'find the TB_SET and timeslot of the last prelim
        Dim drRound As DataRow() : Dim TBSet, timeslot, round As Integer
        drRound = ds.Tables("Round").Select("Event=" & EventID, "timeslot asc")
        For Each row In drRound
            If row.Item("RD_Name") <= 9 Then TBSet = row.Item("TB_SET") : timeslot = row.Item("TimeSlot") : round = row.Item("ID")
        Next

        'find tiebreaker set if it's speakers
        If strResultType.ToUpper = "SPEAKER" Then
            TBSet = 0
            For x = 0 To ds.Tables("Tiebreak_Set").Rows.Count - 1
                If ds.Tables("Tiebreak_Set").Rows(x).Item("ScoreFor") = "Speaker" Then TBSet = ds.Tables("Tiebreak_Set").Rows(x).Item("ID")
            Next x
            If TBSet = 0 Then MsgBox("Can't find a tiebreaker set identified for speakers; please identify a tiebreaker set for speakers on the tiebreaker setup screen and try again.")
        End If

        'load the teams in order
        Dim dt As DataTable
        If strResultType.ToUpper = "TEAM" Then
            dt = MakeTBTable(ds, round, "TEAM", "Full", -1, round)
        Else
            dt = MakeTBTable(ds, round, "SPEAKER", "X", TBSet, round)
        End If

        Dim dummy, strSortDirection As String
        stw.WriteLine("  <FINALRANK EventID=""" & EventID & """>" & " RankFor=""" & strResultType & """>")
        For x = 0 To dt.Rows.Count - 1
            stw.WriteLine("    <COMPETITOR EntryID=""" & dt.Rows(x).Item(0) & """>")
            dummy = "    <TIEBREAKER TB_Name=""SEED"" Sortorder=""" & 1 & """ SortDirection=""ASC"">" & x + 1 & "</TIEBREAKER>"
            stw.WriteLine(dummy)
            For y = 3 To dt.Columns.Count - 1
                strSortDirection = "DESC" : If dt.Rows(0).Item(y) < dt.Rows(1).Item(y) Then strSortDirection = "ASC"
                dummy = "    <TIEBREAKER TB_Name=""" & dt.Columns(y).ColumnName.ToUpper.Trim & """ Sortorder=""" & y - 1 & """ SortDirection=""" & strSortDirection & """>" & dt.Rows(x).Item(y).ToString.Trim & "</TIEBREAKER>"
                stw.WriteLine(dummy)
            Next y
            stw.WriteLine("    </COMPETITOR>")
        Next x
        stw.WriteLine("  </FINALRANK>")
    End Sub
    Sub WriteRoundByRound(ByVal ds As DataSet, ByVal stw As StreamWriter, ByVal EventID As Integer)
        Dim dvRound As New DataView(ds.Tables("ROUND"))
        dvRound.RowFilter = "EVENT=" & EventID
        dvRound.Sort = "timeslot ASC"
        Dim x, y, z, w As Integer : Dim dummy, rdType As String
        Dim foundBallot As DataRow()
        Dim foundPanel As DataRow()
        Dim foundResult As DataRow()
        For x = 0 To dvRound.Count - 1
            rdType = "Prelim" : If dvRound(x).Item("TimeSlot") > 9 Then rdType = "Elim"
            dummy = "  <ROUNDRESULT RoundID=""" & dvRound(x).Item("ID").ToString.Trim & """ RoundName=""" & dvRound(x).Item("Label").trim & """ Sortorder=""" & dvRound(x).Item("TimeSlot") & """ RoundType=""" & rdType & """ EventID=""" & dvRound(x).Item("Event") & """" & ">"
            stw.WriteLine(dummy)
            foundPanel = ds.Tables("Panel").Select("Round=" & dvRound(x).Item("ID"))
            For y = 0 To foundPanel.Length - 1
                If foundPanel(y).Item("Flight") Is System.DBNull.Value Then foundPanel(y).Item("Flight") = 1
                foundBallot = ds.Tables("Ballot").Select("Panel=" & foundPanel(y).Item("ID"), "JUDGE ASC")
                For z = 0 To foundBallot.Length - 1
                    dummy = "    <RESULT_BALLOT JudgeID=""" & foundBallot(z).Item("Judge") & """ RoomID=""" & foundPanel(y).Item("ROOM") & """ Flight=""" & foundPanel(y).Item("Flight") & """ Panel=""" & foundPanel(y).Item("ID") & """" & ">"
                    If z = 0 Then stw.WriteLine(dummy)
                    If z > 0 Then If foundBallot(z).Item("Judge") <> foundBallot(z - 1).Item("Judge") Then stw.WriteLine(dummy)
                    foundResult = ds.Tables("Ballot_Score").Select("Ballot=" & foundBallot(z).Item("ID"))
                    For w = 0 To foundResult.Length - 1
                        dummy = "      <SCORE SCORE_NAME=""" & GetRowInfo(ds, "SCORES", foundResult(w).Item("SCORE_ID"), "Score_Name") & """ ScoreFor=""" & GetRowInfo(ds, "SCORES", foundResult(w).Item("SCORE_ID"), "ScoreFor") & """ Recipient=""" & foundResult(w).Item("Recipient") & """"
                        If GetRowInfo(ds, "SCORES", foundResult(w).Item("SCORE_ID"), "ScoreFor") = "Team" Then dummy &= " Side=""" & foundBallot(z).Item("Side") & """"
                        dummy &= ">" & foundResult(w).Item("Score")
                        stw.WriteLine(dummy & "</SCORE>")
                    Next w
                    If z < foundBallot.Length - 1 Then If foundBallot(z).Item("Judge") <> foundBallot(z + 1).Item("Judge") Then stw.WriteLine("    </RESULT_BALLOT>")
                    If z = foundBallot.Length - 1 Then stw.WriteLine("    </RESULT_BALLOT>")
                Next z
            Next y
            stw.WriteLine("  </ROUNDRESULT>")
        Next x
        dvRound.RowFilter = Nothing
    End Sub
    Sub ZipIt()

        'Make it the TourneyDataResults file
        Dim strFileName As String = strFilePath
        'Dim strFileName As String = Replace(strFilePath, "TourneyData.xml", "TourneyDataResults.xml")
        Dim zippath As String = strFileName
        zippath = zippath.Replace(".xml", ".zip")

        'check for existing file and delte it if it's there
        Dim j As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\CAT\TourneyData.zip"
        Dim fFile As New FileInfo(j)
        If fFile.Exists Then File.Delete(j)

        'Open the zip file if it exists, else create a new one 
        Dim zip As Package = ZipPackage.Open(zippath, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)

        'Add as many files as you like:
        AddToArchive(zip, strFileName)

        zip.Close() 'Close the zip file
    End Sub
    Private Sub AddToArchive(ByVal zip As Package, ByVal fileToAdd As String)

        'Replace spaces with an underscore (_) 
        Dim uriFileName As String = fileToAdd.Replace(" ", "_")

        'A Uri always starts with a forward slash "/" 
        Dim zipUri As String = String.Concat("/", IO.Path.GetFileName(uriFileName))

        Dim partUri As New Uri(zipUri, UriKind.Relative)
        Dim contentType As String = Net.Mime.MediaTypeNames.Application.Zip

        'The PackagePart contains the information: 
        ' Where to extract the file when it's extracted (partUri) 
        ' The type of content stream (MIME type):  (contentType) 
        ' The type of compression:  (CompressionOption.Normal)   
        Dim pkgPart As PackagePart = zip.CreatePart(partUri, contentType, CompressionOption.Normal)

        'Read all of the bytes from the file to add to the zip file 
        Dim bites As Byte() = File.ReadAllBytes(fileToAdd)

        'Compress and write the bytes to the zip file 
        pkgPart.GetStream().Write(bites, 0, bites.Length)

    End Sub
    Sub PostFileToWeb()

        Dim strFileName As String = Replace(strFilePath, "TourneyData.xml", "TourneyDataResults.xml")
        strFileName = strFileName.Replace(".xml", ".zip")
        'Following just send the original file
        strFileName = strFilePath

        Call PassWordCheck()
        Dim URistring As String = "https://www.tabroom.com/api/upload_tourn.mhtml?username=" & My.Settings.UserName & "&password=" & My.Settings.PassWord

        ' Create a new WebClient instance.
        Dim myWebClient As New WebClient()

        ' Upload the file to the URI.
        Dim responseArray As Byte() = myWebClient.UploadFile(uriString, strFileName)

        frmWebInteraction.TextBox1.Text = ""
        frmWebInteraction.TextBox1.Text = System.Text.Encoding.ASCII.GetString(responseArray)
        'MsgBox("Response Received.The contents of the file uploaded are: " & System.Text.Encoding.ASCII.GetString(responseArray))

    End Sub
    Sub PostZipFileToWeb(ByVal NoPrefs As Boolean)

        'Dim strFileName As String = Replace(strFilePath, "TourneyData.xml", "TourneyDataResults.xml")
        Dim strFileName As String = strFilePath
        strFileName = strFileName.Replace(".xml", ".zip")

        Call PassWordCheck()
        Dim URistring As String = "https://www.tabroom.com/api/upload_tourn.mhtml?username=" & My.Settings.UserName & "&password=" & My.Settings.PassWord
        If NoPrefs = True Then URistring &= "&no_prefs=marialiurocks"

        ' Create a new WebClient instance.
        Dim myWebClient As New WebClient()

        ' Upload the file to the URI.
        Dim responseArray As Byte() = myWebClient.UploadFile(URistring, strFileName)

        frmWebInteraction.TextBox1.Text = ""
        frmWebInteraction.TextBox1.Text = System.Text.Encoding.ASCII.GetString(responseArray)
        'MsgBox("Response Received.The contents of the file uploaded are: " & System.Text.Encoding.ASCII.GetString(responseArray))

    End Sub
    Sub StripEmptyBallots(ByRef ds As DataSet)
        'strips ballot scores for rounds with no results
        Dim fdBallots, fdJudgeBallots, fdBalScores As DataRow() : Dim isEmpty, doIt, fired As Boolean
        Dim dummy As String = "" : Dim JudgeTocheck As Integer
        For x = 0 To ds.Tables("Panel").Rows.Count - 1
            fdBallots = ds.Tables("Ballot").Select("Panel=" & ds.Tables("Panel").Rows(x).Item("ID"), "Judge ASC")
            For z = 1 To fdBallots.Length - 1
                'pull all ballots for each unique judge
                doIt = False : fired = False
                If fdBallots(z).Item("Judge") <> fdBallots(z - 1).Item("Judge") Then doIt = True : JudgeTocheck = fdBallots(z - 1).Item("Judge")
                If z = fdBallots.Length - 1 And (fdBallots(z).Item("Judge") <> fdBallots(z - 1).Item("Judge") Or fired = False) Then doIt = True : JudgeTocheck = fdBallots(z).Item("Judge")
                If doIt = True Then
                    fired = True
                    fdJudgeBallots = ds.Tables("Ballot").Select("Panel=" & ds.Tables("Panel").Rows(x).Item("ID") & " and judge=" & JudgeTocheck)
                    dummy = MakeSearchString(fdJudgeBallots, "Ballot")
                    fdBalScores = ds.Tables("Ballot_Score").Select(dummy)
                    isEmpty = True
                    For y = 0 To fdBalScores.Length - 1
                        If fdBalScores(y).Item("Score") <> 0 Then isEmpty = False : Exit For
                    Next y
                    'strip it if no scores are entered yet
                    If isEmpty = True Then
                        For y = fdBalScores.Length - 1 To 0 Step -1
                            fdBalScores(y).Delete()
                        Next
                    End If
                End If
            Next z
        Next x
        ds.AcceptChanges()
        SaveFile(ds)
    End Sub
End Module
