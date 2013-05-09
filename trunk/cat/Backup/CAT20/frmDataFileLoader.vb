Imports System.IO
Imports System.Xml
Imports System.Net


Public Class frmDataFileLoader
    Dim ds As New DataSet
    Dim EnableEvents As Boolean

    Private Sub butLocate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butLocate.Click
        Dim str As String = ""
        str &= "This will allow you to load a file from anywhere on your comptuer, and the program should run without error until you close it." & Chr(10) & Chr(10)
        str &= "However, you will have to run this procedure EVERY time to close the CAT" & Chr(10) & Chr(10)
        str &= "A better solution is to move the data file into the CAT sub-folder in your documents folder." & Chr(10) & Chr(10)
        str &= "For more information, click the 'Important Info for First-Time Users' button in the top-right of the main menu." & Chr(10) & Chr(10)
        MsgBox(str, MsgBoxStyle.OkOnly)
        'User should identify the location
        OpenFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        If OpenFileDialog1.ShowDialog() = DialogResult.Cancel Then Exit Sub
        strFilePath = OpenFileDialog1.FileName
        'Check that there really is a file where the user entered it
        Dim fFile As New FileInfo(strFilePath)
        If Not fFile.Exists Then
            MsgBox("That doesn't appear to be a valid location.  Click the button to try again, or download the file from the internet.")
            Exit Sub
        End If
        frmMainMenu.lblFileLocation.Text = "Current data file is located at " & strFilePath
    End Sub

    
    Private Sub butUDSImport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butUDSImport.Click
        'show warning
        Dim q = MsgBox("This will ERASE all of your current data and replace it with the data in the specified file.  Are you SURE you want to continue?", MsgBoxStyle.YesNo)
        If q = vbNo Then Exit Sub
        'use file dialogue; locate the import file and load it
        OpenFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        OpenFileDialog1.FileName = "TourneyDataResults"
        If OpenFileDialog1.ShowDialog() = DialogResult.Cancel Then Exit Sub
        Dim strNewFilePath As String
        strNewFilePath = OpenFileDialog1.FileName
        'now load
        Dim ds2 As New DataSet
        Dim xmlFile As XmlReader
        xmlFile = XmlReader.Create(strNewFilePath, New XmlReaderSettings())
        'Dim strXsdLocation As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\CAT\" & strNewFilePath & ".xsd"
        'ds.ReadXmlSchema(strXsdLocation)
        Try
            ds2.ReadXml(xmlFile)
            'ds2.ReadXml(xmlFile, XmlReadMode.InferSchema)
        Catch
            MsgBox("Failed to load the new file.")
            Exit Sub
        End Try
        xmlFile.Close()
        Call CheckFile(ds2)
        'now create a blank dataset to read stuff into
        Dim strXsdLocation As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\CAT\TourneyData.xsd"
        ds.ReadXmlSchema(strXsdLocation)
        ds.EnforceConstraints = False
        'Populate with entries
        Call CopyTable(ds2.Tables("Entry"), ds.Tables("Entry"))
        Call CopyTable(ds2.Tables("Entry_Student"), ds.Tables("Entry_Student"))
        Call CopyTable(ds2.Tables("Judge"), ds.Tables("Judge"))
        Call CopyTable(ds2.Tables("Event"), ds.Tables("Event"))
        Call CopyTable(ds2.Tables("School"), ds.Tables("School"))
        Call CopyTable(ds2.Tables("Tourn"), ds.Tables("Tourn"))
        'Generate all the default settings
        Call MakeTourneySettings(ds2, ds)
        'MakeTourneySettings will also fire the results maker if there are results
        'save and close
        Call SaveFile(ds)
        MsgBox("Load complete - please review the information on the setup screens the make SURE that the settings are correct.  In instances where the imported file did not specify settings, they were inferred.  This will speed up the data converstion process but may produce unexpected results.")
    End Sub
    Sub MakeTimeSlots(ByVal ds2 As DataSet, ByRef ds As DataSet)
        'check if RoundResults table is there
        Dim HiNum As Integer = -99
        For x = 0 To ds2.Tables("RoundResult").Rows.Count - 1
            If ds2.Tables("RoundResult").Rows(x).Item("SortOrder") > HiNum Then HiNum = ds2.Tables("RoundResult").Rows(x).Item("SortOrder")
        Next
        Call InitializeTimeSlots(ds, HiNum)
    End Sub
    Sub CopyTable(ByRef dtFrom As DataTable, ByRef dtTo As DataTable)
        'scrolls through the source table, and if the field exists in the recipient table it copies it
        Dim dt As New DataTable
        Dim dr As DataRow
        'strip nulls
        For x = dtFrom.Rows.Count - 1 To 0 Step -1
            If dtFrom.Rows(x).Item("ID") Is System.DBNull.Value Then
                dtFrom.Rows(x).Delete()
            End If
        Next x
        dtFrom.AcceptChanges()
        'add fields to recipient table
        For x = 0 To dtFrom.Rows.Count - 1
            dr = dtTo.NewRow
            For y = 0 To dtFrom.Columns.Count - 1
                If dtTo.Columns.Contains(dtFrom.Columns(y).ColumnName) = True Then
                    If dtTo.Columns(dtFrom.Columns(y).ColumnName).DataType Is GetType(System.Int64) Then
                        If dtFrom.Rows(x).Item(y) Is System.DBNull.Value Then dtFrom.Rows(x).Item(y) = 0
                        If dtFrom.Rows(x).Item(y) = "" Then dtFrom.Rows(x).Item(y) = 0
                    ElseIf dtTo.Columns(dtFrom.Columns(y).ColumnName).DataType Is GetType(System.Boolean) Then
                        If dtFrom.Rows(x).Item(y) Is System.DBNull.Value Then dtFrom.Rows(x).Item(y) = False
                        If dtFrom.Rows(x).Item(y) = "" Then dtFrom.Rows(x).Item(y) = False
                        If dtFrom.Rows(x).Item(y) = "0" Then dtFrom.Rows(x).Item(y) = False
                        If dtFrom.Rows(x).Item(y) = "1" Then dtFrom.Rows(x).Item(y) = True
                        If dtTo.Columns(dtFrom.Columns(y).ColumnName).DataType Is GetType(System.Int64) Then
                            If dtFrom.Rows(x).Item(y) = 0 Then dtFrom.Rows(x).Item(y) = False
                        End If
                    End If
                    dr.Item(dtFrom.Columns(y).ColumnName) = dtFrom.Rows(x).Item(y)
                End If
            Next y
            dtTo.Rows.Add(dr)
        Next
    End Sub
    Sub CheckFile(ByRef ds2 As DataSet)
        'Check unique fields for Judge, Entry_student, entry

        'Check every Judge/Team/Student in results exists in the table
        If ds2.Tables("Scores") Is Nothing Then Exit Sub
        Dim dr, drBallot, drRound As DataRow()
        Dim drEntry As DataRow
        Dim team As Integer
        For x = 0 To ds2.Tables("Score").Rows.Count - 1
            If ds2.Tables("Score").Rows(x).Item("ScoreFor").toupper = "TEAM" Then
                team = ds2.Tables("Score").Rows(x).Item("Recipient")
                dr = ds2.Tables("Entry").Select("ID=" & ds2.Tables("Score").Rows(x).Item("Recipient"))
                If dr.Length = 0 Then
                    MsgBox("There are results for team XXX but no record of them in the entry table.  Creating a blank entry.")
                    drEntry = ds2.Tables("Entry").NewRow
                    drEntry.Item("School") = -1
                    drBallot = ds2.Tables("Ballot").Select("Ballot_ID=" & ds2.Tables("Score").Rows(x).Item("Ballot_Id"))
                    drRound = ds2.Tables("RoundResult").Select("RoundResult_Id=" & drBallot(0).Item("RoundResult_Id"))
                    drEntry.Item("Event") = drRound(0).Item("EventID")
                    If ds2.Tables("Entry").Columns.Contains("ADA") = True Then drEntry.Item("Rating") = 0
                    If ds2.Tables("Entry").Columns.Contains("Code") = True Then drEntry.Item("Code") = "Team" & ds2.Tables("Score").Rows(x).Item("Recipient").ToString.Trim
                    If ds2.Tables("Entry").Columns.Contains("FullName") = True Then drEntry.Item("FullName") = "Team" & ds2.Tables("Score").Rows(x).Item("Recipient").ToString.Trim
                    If ds2.Tables("Entry").Columns.Contains("Dropped") = True Then drEntry.Item("Dropped") = False
                    If ds2.Tables("Entry").Columns.Contains("ADA") = True Then drEntry.Item("ADA") = False
                    If ds2.Tables("Entry").Columns.Contains("TubDisability") = True Then drEntry.Item("TubDisability") = False
                    If ds2.Tables("Entry").Columns.Contains("Notes") = True Then drEntry.Item("Notes") = ""
                    ds2.Tables("Entry").Rows.Add(drEntry)
                End If
            Else
            End If
        Next x
    End Sub

    Sub MakeTourneySettings(ByVal ds2 As DataSet, ByRef ds As DataSet)
        'Default tourney settings?
        Call InitializeTourneySettings(ds)
        'see if there are results
        Dim ResultsExist As Boolean = True
        If ds2.Tables.IndexOf("RoundResult") = -1 Then
            MsgBox("No results seem to exist; run through the setup screens off the main menu after loading this file.")
            ResultsExist = False
        End If
        'create timeslots
        If ds2.Tables.IndexOf("TIMESLOT") = -1 Then
            'table doesn't exist, so infer
            If ResultsExist = True Then Call MakeTimeSlots(ds2, ds)
        Else
            Call CopyTable(ds2.Tables("Timeslot"), ds.Tables("Timeslot"))
        End If
        'initialize judge and rooms based on timeslots; need to have events and timeslots loaded
        Call InitializeRooms(ds)
        Call InitializeJudges(ds)
        'Divisions and default settings
        Call MakePrimaryKey(ds, "Event_Setting", "ID")
        Call EventSettingsCheck(ds)
        'tiebreakers
        If ds2.Tables.IndexOf("Tiebreak") = -1 Then
            'creates default settings for both TIEBREAK and TIEBREAK_SET tables
            'NOTE; you COULD infer these from the FinalRank table, but that's a lot of headache
            Call InitializeTieBreakers(ds, True)
        End If
        'rounds, with timeslots
        Call MakePrimaryKey(ds, "Round", "ID")
        If ResultsExist = True Then Call RoundMaker(ds2, ds)
        'Results
        If ResultsExist = True Then
            Call MakePrimaryKey(ds, "Ballot_Score", "ID")
            Call MakePrimaryKey(ds, "Ballot", "ID")
            Call MakePrimaryKey(ds, "Panel", "ID")
            Call MakePrimaryKey(ds, "Entry_Student", "ID")
            Call ResultsTables(ds2, ds)
        End If
    End Sub
    Sub RoundMaker(ByVal ds2 As DataSet, ByRef ds As DataSet)
        'exit if already rows
        If ds.Tables("Round").Rows.Count > 0 Then Exit Sub
        Dim dr As DataRow : Dim fdTeams, fdPrefs As DataRow()
        For x = 0 To ds2.Tables("RoundResult").Rows.Count - 1
            dr = ds.Tables("Round").NewRow
            dr.Item("Event") = ds2.Tables("RoundResult").Rows(x).Item("EventID")
            dr.Item("Timeslot") = ds2.Tables("RoundResult").Rows(x).Item("SortOrder")
            dr.Item("TB_Set") = 1
            dr.Item("RD_Name") = ds2.Tables("RoundResult").Rows(x).Item("SortOrder")
            dr.Item("Label") = ds2.Tables("RoundResult").Rows(x).Item("RoundName")
            dr.Item("Flighting") = 1
            If ds2.Tables("RoundResult").Rows(x).Item("RoundType").toupper = "ELIM" Then
                dr.Item("JudgesPerPanel") = 2
                dr.Item("PairingScheme") = "Elim"
            Else
                dr.Item("JudgesPerPanel") = 1
                dr.Item("PairingScheme") = "HighLow"
            End If
            'set judgepref; make random if no judgepref table
            If ds2.Tables.IndexOf("JudgePref") = -1 Then
                dr.Item("JudgePlaceScheme") = "Random"
            Else
                'check to see if any team in the division has prefs entered; if so, mark as TEAMRATING
                'if not, leave as RANDOM
                dr.Item("JudgePlaceScheme") = "Random"
                fdTeams = ds.Tables("Entry").Select("Event=" & dr.Item("Event"))
                For y = 0 To fdTeams.Length - 1
                    fdPrefs = ds.Tables("JudgePref").Select("Team=" & fdTeams(y).Item("ID"))
                    If fdPrefs.Length > 0 Then dr.Item("JudgePlaceScheme") = "TeamRating" : Exit For
                Next y
            End If
            ds.Tables("Round").Rows.Add(dr)
        Next x
    End Sub

    Private Sub butInitialize_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butInitialize.Click
        'NEED TO ADD:
        'check for the existence of the .xsd file first

        'show warning
        Dim q = MsgBox("This will ERASE all of your current data and replace it with the data in the specified file.  Are you SURE you want to continue?", MsgBoxStyle.YesNo)
        If q = vbNo Then Exit Sub
        'now load
        Dim strXsdLocation As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\CAT\TourneyData.xsd"
        ds.ReadXmlSchema(strXsdLocation)
        Call SaveFile(ds)
        MsgBox("Done.  Close the program and re-open.")
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        ds.Clear()
        Dim dr As DataRow
        dr = ds.Tables("Tourn").NewRow
        ds.Tables("Tourn").Rows.Add(dr)
        Call SaveFile(ds)
        MsgBox("Done.  Close the program and re-open.")
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        EnableEvents = False
        My.Settings.UserName = txtUserName.Text
        My.Settings.PassWord = txtPassWord.Text
        Dim str As String
        Dim URL As String = "https://www.tabroom.com/api/list_tourns.mhtml?username=" & txtUserName.Text & "&password=" & txtPassWord.Text & "&all=1"
        Dim request As HttpWebRequest = WebRequest.Create(URL)
        Dim response As HttpWebResponse = request.GetResponse()
        Dim reader As StreamReader = New StreamReader(response.GetResponseStream())
        str = reader.ReadToEnd
        response.Close() : reader.Close()
        Try
            Dim XMLreader As StringReader = New System.IO.StringReader(str)
            Dim newDS As DataSet = New DataSet
            newDS.ReadXml(XMLreader)
            DataGridView1.AutoGenerateColumns = False
            DataGridView1.DataSource = newDS.Tables("Tourn")
            'put events on datagridview2
            newDS.Tables("Event").DefaultView.RowFilter = "TOURN=-999"
            DataGridView2.AutoGenerateColumns = False
            DataGridView2.DataSource = newDS.Tables("Event")
            Dim dgv3 As New DataGridViewCheckBoxColumn
            dgv3.DataPropertyName = "Download"
            dgv3.Name = "Download"
            dgv3.HeaderText = "Download"
            DataGridView2.Columns.Add(dgv3)
            For x = 0 To DataGridView1.RowCount - 1
                DataGridView1.Rows(x).Selected = False
            Next x
            Label3.ForeColor = Color.Black : Label4.ForeColor = Color.Red
        Catch
            MsgBox("The tournament listing has failed; please make sure you have entered your username and password correctly, and check to make sure that you are connected to the internet.  Try again.")
        End Try
        EnableEvents = True
    End Sub

    Private Sub frmDataFileLoader_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        txtPassWord.Text = My.Settings.PassWord
        txtUserName.Text = My.Settings.UserName

        Label3.ForeColor = Color.Red

    End Sub
    Sub ShowEvents() Handles DataGridView1.SelectionChanged 'DataGridView1.Click 
        If DataGridView1.CurrentRow Is Nothing Then Exit Sub
        If EnableEvents = False Then Exit Sub
        'dvRound.RowFilter = "EVENT=" & EventID
        Dim dt As New DataTable
        dt = DataGridView2.DataSource
        dt.DefaultView.RowFilter = "TOURN=" & DataGridView1.CurrentRow.Cells("ID").Value
        For x = 0 To DataGridView2.RowCount - 1
            DataGridView2.Rows(x).Cells("Download").Value = True
        Next x
        Label5.ForeColor = Color.Red : Label4.ForeColor = Color.Black
    End Sub

    Private Sub butDownLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butDownLoad.Click
        Label6.ForeColor = Color.Red
        Label5.ForeColor = Color.Black
        Dim q As Integer = MsgBox("This will ERASE all current data and replace it with the download.  Continue?", MsgBoxStyle.YesNo)
        If q = vbNo Then Exit Sub
        Dim dtDummy As DateTime : dtDummy = Now
        Dim strTourn, strDivisions As String
        strTourn = DataGridView1.CurrentRow.Cells("ID").Value.ToString.Trim
        strDivisions = ""
        For x = 0 To DataGridView2.RowCount - 1
            If DataGridView2.Rows(x).Cells("Download").Value = True Then
                If strDivisions <> "" Then strDivisions &= ","
                strDivisions &= DataGridView2.Rows(x).Cells("EventID").Value.ToString.Trim
            End If
        Next x
        Dim URL As String = "https://www.tabroom.com/api/download_tourn.mhtml?username=" & txtUserName.Text & "&password=" & txtPassWord.Text & "&tourn_id=" & strTourn & "&event_id=" & strDivisions
        'MsgBox(URL)
        lblDownLoadStatus.Text = "Opening site...File downloading...." : lblDownLoadStatus.Refresh()
        Dim request As HttpWebRequest = WebRequest.Create(URL)
        Dim response As HttpWebResponse = request.GetResponse()
        Dim reader As StreamReader = New StreamReader(response.GetResponseStream())
        Dim st As StreamWriter = File.CreateText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\CAT\TourneyData.xml")
        st.WriteLine(reader.ReadToEnd())
        st.Close()
        response.Close() : reader.Close()
        lblDownLoadStatus.Text = "Download Complete! Loading schema file..."
        Call SchemaDownloader()
        lblDownLoadStatus.Text = "Download Complete! Loading preset small division file..."
        Call PresetsDownloader()
        'now clean up
        Dim strdummy As String = DateDiff(DateInterval.Second, dtDummy, Now)
        lblDownLoadStatus.Text = "Download complete! " & strdummy & " seconds to process."
        lblDownLoadStatus.ForeColor = Color.Red : Label5.ForeColor = Color.Black : Label6.ForeColor = Color.Black
        strFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\CAT\TourneyData.xml"
        'try a load; will shut down if a correction is necessary
        Call LoadFile(ds, "TourneyData")
        'got this far, so no corretion necessary.  Now update the stuff on the main page and close
        If ds.Tables("Tourn").Rows.Count > 0 Then
            frmMainMenu.lblTourneyName.Text = ds.Tables("Tourn").Rows(0).Item("TournName").trim & " -- " & ds.Tables("Tourn").Rows(0).Item("StartDate").trim & " through " & ds.Tables("Tourn").Rows(0).Item("EndDate").trim
            frmMainMenu.lblFileLocation.Text = "Data file location is " & strFilePath
        Else
            MsgBox("Problem with the datafile; program is closing.  If you have tried the fix following a download, this process is normal, and closing and re-opening will work.")
            ds.Dispose()
            Exit Sub
        End If
        Call SetupCompletionStatus(ds)
        ds.Dispose()
        MsgBox("Download successful!  This page will now close and return you to the main menu.")
        Me.Close()
    End Sub
    Sub SchemaDownloader()

        'see if its there
        Dim j As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\CAT\TourneyData.xsd"
        Dim fFile As New FileInfo(j)
        If fFile.Exists Then Exit Sub

        'not there, so download it
        Try
            Dim URL As String
            URL = "http://www.tabroom.com/jbruschke/TourneyData.xsd"
            Dim request As HttpWebRequest = WebRequest.Create(URL)
            Dim response As HttpWebResponse = request.GetResponse()
            Dim reader As StreamReader = New StreamReader(response.GetResponseStream())
            Dim stw As StreamWriter = File.CreateText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\CAT\TourneyData.xsd")
            lblDownLoadStatus.Text = "Download in progress! "
            stw.WriteLine(reader.ReadToEnd())
            reader.Close()
            stw.Close()
        Catch
            MsgBox("Formatting (.xsd schema) file faile to download , either because the computer is not connected to the internt or the tournament to download was identified incorrectly.  If the file is especially large, you may simply want to try the download again.  Please correct the problem and try again.")
            Exit Sub
        End Try
    End Sub
    Sub PresetsDownloader()

        'see if its there
        Dim j As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\CAT\TourneyData.xsd"
        Dim fFile As New FileInfo(j)
        If fFile.Exists Then Exit Sub

        'not there, so download it
        Try
            Dim URL As String
            URL = "http://www.tabroom.com/jbruschke/Presets.txt"
            Dim request As HttpWebRequest = WebRequest.Create(URL)
            Dim response As HttpWebResponse = request.GetResponse()
            Dim reader As StreamReader = New StreamReader(response.GetResponseStream())
            Dim stw As StreamWriter = File.CreateText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\CAT\Presets.txt")
            lblDownLoadStatus.Text = "Download in progress! "
            stw.WriteLine(reader.ReadToEnd())
            reader.Close()
            stw.Close()
        Catch
            MsgBox("Presets (Presets.txt) file faile to download , either because the computer is not connected to the internt or the tournament to download was identified incorrectly.  If the file is especially large, you may simply want to try the download again.  Please correct the problem and try again.")
            Exit Sub
        End Try
    End Sub

    Sub StepFour() Handles DataGridView2.Click
        Label5.ForeColor = Color.Black : Label6.ForeColor = Color.Red
    End Sub

    Private Sub butTestDownload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butTestDownload.Click
        Call SchemaDownloader()
        Call PresetsDownloader()

        'define default location
        Dim j As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\CAT\TourneyData.xml"

        'Download the data file
        Try
            Dim URL As String
            URL = "http://www.tabroom.com/jbruschke/TourneyData.xml"
            If chkMPJSample.Checked = True Then
                URL = "http://www.tabroom.com/jbruschke/TourneyDataCSUFWithFakeNames.xml"
            End If
            Dim request As HttpWebRequest = WebRequest.Create(URL)
            Dim response As HttpWebResponse = request.GetResponse()
            Dim reader As StreamReader = New StreamReader(response.GetResponseStream())
            Dim stw As StreamWriter = File.CreateText(j)
            lblDownLoadStatus.Text = "Download in progress...." : lblDownLoadStatus.Refresh()
            stw.WriteLine(reader.ReadToEnd())
            reader.Close()
            stw.Close()
            strFilePath = j
        Catch
            MsgBox("The download failed, either because the computer is not connected to the internt or the tournament to download was identified incorrectly.  If the file is especially large, you may simply want to try the download again.  Please correct the problem and try again.")
            Exit Sub
        End Try

        'try a load; will shut down if a correction is necessary
        Call LoadFile(ds, "TourneyData")
        'got this far, so no corretion necessary.  Now update the stuff on the main page and close
        If ds.Tables("Tourn").Rows.Count > 0 Then
            frmMainMenu.lblTourneyName.Text = ds.Tables("Tourn").Rows(0).Item("TournName").trim & " -- " & ds.Tables("Tourn").Rows(0).Item("StartDate").trim & " through " & ds.Tables("Tourn").Rows(0).Item("EndDate").trim
            frmMainMenu.lblFileLocation.Text = "Data file location is " & strFilePath
        Else
            MsgBox("Problem with the datafile; program is closing.  If you have tried the fix following a download, this process is normal, and closing and re-opening will work.")
            ds.Dispose()
            Exit Sub
        End If
        Call SetupCompletionStatus(ds)
        ds.Dispose()
        MsgBox("Download successful!  This page will now close and return you to the main menu.")
        Me.Close()

    End Sub

    Private Sub butDeselectAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butDeselectAll.Click
        For x = 0 To DataGridView2.RowCount - 1
            DataGridView2.Rows(x).Cells("Download").Value = False
        Next x
    End Sub
End Class