Imports System.Xml
Imports System.IO
Imports System
Imports System.Net
Imports System.Text
Imports System.Collections.Specialized
Imports System.IO.Packaging

Public Class frmUtilities
    Dim ds As New DataSet

    Private Sub butUDSImport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butUDSImport.Click
        'show warning
        Dim q = MsgBox("This will ERASE all of your current data and replace it with the data in the specified file.  Are you SURE you want to continue?", MsgBoxStyle.YesNo)
        If q = vbNo Then Exit Sub
        'use file dialogue; locate the import file and load it
        OpenFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        OpenFileDialog1.FileName = "TourneyDataResults2"
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

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        ' Create a request using a URL that can receive a post. 
        Dim request As WebRequest = WebRequest.Create("https://www.tabroom.com/api/test.mhtml?message=FredIsDeadSexy")
        ' Set the Method property of the request to POST.
        request.Method = "POST"
        ' Create POST data and convert it to a byte array.
        Dim postData As String = "This is a test that posts this string to a Web server."
        Dim byteArray As Byte() = Encoding.UTF8.GetBytes(postData)
        ' Set the ContentType property of the WebRequest.
        request.ContentType = "application/x-www-form-urlencoded"
        ' Set the ContentLength property of the WebRequest.
        request.ContentLength = byteArray.Length
        ' Get the request stream.
        Dim dataStream As Stream = request.GetRequestStream()
        ' Write the data to the request stream.
        dataStream.Write(byteArray, 0, byteArray.Length)
        ' Close the Stream object.
        dataStream.Close()
        ' Get the response.
        Dim response As WebResponse = request.GetResponse()
        ' Display the status.
        Console.WriteLine(CType(response, HttpWebResponse).StatusDescription)
        ' Get the stream containing content returned by the server.
        dataStream = response.GetResponseStream()
        ' Open the stream using a StreamReader for easy access.
        Dim reader As New StreamReader(dataStream)
        ' Read the content.
        Dim responseFromServer As String = reader.ReadToEnd()
        ' Display the content.
        Console.WriteLine(responseFromServer)
        MsgBox(responseFromServer)
        ' Clean up the streams.
        reader.Close()
        dataStream.Close()
        response.Close()
        MsgBox("Done")
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click

        Label1.Text = "Doin' nothing"

        Dim uriString As String = InputBox("Enter URI:", , "https://www.tabroom.com/api/test.mhtml")

        ' Create a new WebClient instance.
        Dim myWebClient As New WebClient()

        Dim fileName As String = InputBox("Please enter the fully qualified path of the file to be uploaded to the URI", , strFilePath)
        Label1.Text = "Uploading..." & fileName & " to " & uriString : Label1.Refresh()

        ' Upload the file to the URI.
        ' The 'UploadFile(uriString,fileName)' method implicitly uses HTTP POST method. 
        Dim responseArray As Byte() = myWebClient.UploadFile(uriString, fileName)

        ' Decode and display the response.
        MsgBox("Response Received.The contents of the file uploaded are: " & System.Text.Encoding.ASCII.GetString(responseArray))
        TextBox1.Text = System.Text.Encoding.ASCII.GetString(responseArray)
        Label1.Text = System.Text.Encoding.ASCII.GetString(responseArray)
        MsgBox("Done")
    End Sub
    Private Function UploadFileEx(ByVal uploadfile As String, ByVal url As String, ByVal fileFormName As String, ByVal contenttype As String, ByVal querystring As NameValueCollection, ByVal cookies As CookieContainer) As String

        Label1.Text = "starting" : Label1.Refresh()

        If (fileFormName Is Nothing) OrElse (fileFormName.Length = 0) Then
            fileFormName = "file"
        End If

        If (contenttype Is Nothing) OrElse (contenttype.Length = 0) Then
            contenttype = "application/octet-stream"
        End If

        Dim postdata As String
        postdata = "?"
        If querystring IsNot Nothing Then
            For Each key As String In querystring.Keys
                postdata += key & "=" & querystring.[Get](key) & "&"
            Next
        End If
        Dim uri As New Uri(url & postdata)


        Dim boundary As String = "----------" & DateTime.Now.Ticks.ToString("x")
        Dim webrequest__1 As HttpWebRequest = DirectCast(WebRequest.Create(uri), HttpWebRequest)
        webrequest__1.CookieContainer = cookies
        webrequest__1.ContentType = "multipart/form-data; boundary=" & boundary
        webrequest__1.Method = "POST"


        Label1.Text = "Making message header..." : Label1.Refresh()
        ' Build up the post message header
        Dim sb As New StringBuilder()
        sb.Append("--")
        sb.Append(boundary)
        sb.Append(vbCr & vbLf)
        sb.Append("Content-Disposition: form-data; name=""")
        sb.Append(fileFormName)
        sb.Append("""; filename=""")
        sb.Append(Path.GetFileName(uploadfile))
        sb.Append("""")
        sb.Append(vbCr & vbLf)
        sb.Append("Content-Type: ")
        sb.Append(contenttype)
        sb.Append(vbCr & vbLf)
        sb.Append(vbCr & vbLf)

        Dim postHeader As String = sb.ToString()
        Dim postHeaderBytes As Byte() = Encoding.UTF8.GetBytes(postHeader)

        ' Build the trailing boundary string as a byte array
        ' ensuring the boundary appears on a line by itself
        Dim boundaryBytes As Byte() = Encoding.ASCII.GetBytes(vbCr & vbLf & "--" & boundary & vbCr & vbLf)

        Dim fileStream As New FileStream(uploadfile, FileMode.Open, FileAccess.Read)
        Dim length As Long = postHeaderBytes.Length + fileStream.Length + boundaryBytes.Length
        webrequest__1.ContentLength = length

        Dim requestStream As Stream = webrequest__1.GetRequestStream()

        ' Write out our post header
        requestStream.Write(postHeaderBytes, 0, postHeaderBytes.Length)

        ' Write out the file contents
        Label1.Text = "About to write " : Label1.Refresh()
        Dim buffer As Byte() = New [Byte](CUInt(Math.Min(4096, CInt(fileStream.Length))) - 1) {}
        Dim bytesRead As Integer = 0
        While (InlineAssignHelper(bytesRead, fileStream.Read(buffer, 0, buffer.Length))) <> 0
            requestStream.Write(buffer, 0, bytesRead)
        End While

        ' Write out the trailing boundary
        requestStream.Write(boundaryBytes, 0, boundaryBytes.Length)
        Dim responce As WebResponse = webrequest__1.GetResponse()
        Dim s As Stream = responce.GetResponseStream()
        Dim sr As New StreamReader(s)

        Return sr.ReadToEnd()

    End Function
    Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, ByVal value As T) As T
        target = value
        Return value
    End Function

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim a As String = UploadFileEx(strFilePath, "https://www.tabroom.com/api/test.mhtml", "TourneyData.xml", Nothing, Nothing, Nothing)
        MsgBox(a)
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Dim zippath As String = strFilePath
        zippath = zippath.Replace(".xml", ".zip")

        'Open the zip file if it exists, else create a new one 
        Dim zip As Package = ZipPackage.Open(zipPath, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)

        'Add as many files as you like:
        AddToArchive(zip, strFilePath)
        
        zip.Close() 'Close the zip file
        MsgBox("Done")

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

    Private Sub butDeleteResults_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butDeleteResults.Click
        Dim q As Integer = MsgBox("Are you sure you want to delete ALL your data?", MsgBoxStyle.YesNo)
        If q = vbYes Then q = MsgBox("Really, really sure?  Click YES one more time to delete everything...", MsgBoxStyle.YesNo)
        If q = vbNo Then Exit Sub
        For x = ds.Tables("Panel").Rows.Count - 1 To 0 Step -1
            Label1.Text = "Deleting " & x & " of " & ds.Tables("Panel").Rows.Count - 1
            ds.Tables("Panel").Rows(x).Delete()
        Next x
        Label1.Text = "Done."
    End Sub

    Private Sub frmUtilities_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Load
        LoadFile(ds, "TourneyData")

        'bind round CBO
        cboround.DataSource = ds.Tables("Round")
        cboround.DisplayMember = "Label"
        cboround.ValueMember = "ID"

    End Sub
    Private Sub frmUtilities_Unload(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Disposed
        'save file on page close
        Call SaveFile(ds)
        ds.Dispose()
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Dim str As String
        Dim URL As String = "https://www.tabroom.com/api/list_tourns.mhtml?username=" & My.Settings.UserName & "&password=" & My.Settings.PassWord & "&all=1"
        Label1.Text = "Opening site..."
        Dim request As HttpWebRequest = WebRequest.Create(URL)
        Dim response As HttpWebResponse = request.GetResponse()
        Dim reader As StreamReader = New StreamReader(response.GetResponseStream())
        str = reader.ReadToEnd
        Dim XMLreader As StringReader = New System.IO.StringReader(str)
        Dim newDS As DataSet = New DataSet
        newDS.ReadXml(XMLreader)
        DataGridView1.DataSource = newDS.Tables("Tourn")
        'Do
        'y = y + 1
        'Label1.Text = y.ToString : Label1.Refresh()
        'str = reader.ReadLine()
        'TextBox1.Text &= str : TextBox1.Refresh()
        'st.WriteLine(str)
        'Loop Until InStr(str.ToUpper, "</TOURNLIST>") > 0
        'st.Close()
        response.Close() : reader.Close()
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        Dim URL As String = "https://www.tabroom.com/api/download_tourn.mhtml?username=" & My.Settings.UserName & "&password=" & My.Settings.PassWord & "&tourn_id=1506&event_id=16894"
        Label1.Text = "Opening site..." : Label1.Refresh()
        Dim request As HttpWebRequest = WebRequest.Create(URL)
        Dim response As HttpWebResponse = request.GetResponse()
        Dim reader As StreamReader = New StreamReader(response.GetResponseStream())
        Dim st As StreamWriter = File.CreateText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\CAT\TestDownload.xml")
        st.WriteLine(reader.ReadToEnd())
        st.Close()
        response.Close() : reader.Close()
        Label1.Text = "Done" : Label1.Refresh()
    End Sub
    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        ds.Tables("Tiebreak").Clear()
        ds.Tables("Round").Clear()
    End Sub

    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click
        For x = 0 To ds.Tables("Round").Rows.Count - 1
            ds.Tables("Round").Rows(x).Item("CreatedOffline") = 2
        Next x
    End Sub

    Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button9.Click
        Dim stw As StreamWriter
        stw = New StreamWriter(strFilePath)
        stw.WriteLine("<?xml version=""1.0""?>")
        stw.WriteLine("<DATABASERECORDS>")
        Dim st As StreamReader = File.OpenText("C:\Users\jbruschke\Desktop\Gonz\students.txt")
        Dim b, FN, LN, EM As String : Dim dr As DataRow : Dim ctr As Integer
        Dim marker As Integer
        While st.Peek > -1
            b = st.ReadLine
            FN = "" : LN = "" : EM = "" : marker = 1
            For x = 1 To b.Length
                If Mid(b, x, 1) = "," Then
                    If LN = "" Then
                        LN = Mid(b, marker, x - marker)
                    ElseIf FN = "" Then
                        ctr += 1
                        FN = Mid(b, marker + 1, x - marker - 1)
                        EM = Mid(b, x + 1, Len(b) - x)
                        stw.WriteLine("  <USER>")
                        stw.WriteLine("    <ID>" & ctr.ToString)
                        stw.WriteLine("    </ID>")
                        stw.WriteLine("    <FIRST>" & FN)
                        stw.WriteLine("    </FIRST>")
                        stw.WriteLine("    <LAST>" & LN)
                        stw.WriteLine("    </LAST>")
                        stw.WriteLine("    <SCHOOL>0")
                        stw.WriteLine("    </SCHOOL>")
                        stw.WriteLine("    <EMAIL>" & EM)
                        stw.WriteLine("    </EMAIL>")
                        stw.WriteLine("    <ROLE>COMPETITOR")
                        stw.WriteLine("    </ROLE>")
                        stw.WriteLine("  </USER>")
                    End If
                    marker = x
                End If
            Next
        End While
        stw.WriteLine("</DATABASERECORDS>")
        stw.Close()
        MsgBox("Done")
    End Sub

    Private Sub Button10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button10.Click
        Dim mDS As New DataSet
        LoadFile(mDS, "TourneyDataMaster")
        For x = 0 To mDS.Tables("User").Rows.Count - 1
            For y = 0 To ds.Tables("Entry_Student").Rows.Count - 1
                If mDS.Tables("User").Rows(x).Item("Last").toupper.trim = ds.Tables("Entry_Student").Rows(y).Item("Last").toupper.trim Then
                    If mDS.Tables("User").Rows(x).Item("First").toupper.trim = ds.Tables("Entry_Student").Rows(y).Item("First").toupper.trim Then
                        ds.Tables("Entry_Student").Rows(y).Item("Email") = mDS.Tables("User").Rows(x).Item("Email")
                    End If
                End If
            Next y
        Next x
        DataGridView1.DataSource = ds.Tables("Entry_Student")
    End Sub

    Private Sub Button11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button11.Click
        DataGridView1.DataSource = ds.Tables("Judge")
    End Sub

    Private Sub Button12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button12.Click
        For x = ds.Tables("Room").Columns.Count - 1 To 0 Step -1
            If InStr(ds.Tables("Room").Columns(x).ColumnName.ToUpper, "EVENT") > 0 Or InStr(ds.Tables("Room").Columns(x).ColumnName.ToUpper, "TIMESLOT") > 0 Then
                ds.Tables("Room").Columns.RemoveAt(x)
            End If
        Next x
        ds.Tables("Room").AcceptChanges()
        MsgBox("dONE")
    End Sub

    Private Sub Button13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button13.Click
        DataGridView1.DataSource = ds.Tables("Entry_student")
        Dim fd As DataRow()
        For x = 0 To ds.Tables("entry_student").Rows.Count - 1
            fd = ds.Tables("entry_student").Select("ID=" & ds.Tables("entry_student").Rows(x).Item("ID"))
            If fd.Length > 1 Then
                If fd(0).Item("School") = fd(1).Item("School") Then MsgBox(ds.Tables("entry_student").Rows(x).Item("Last"))
            End If
        Next x
    End Sub

    Private Sub Button14_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button14.Click
        For x = 0 To ds.Tables("Ballot").Rows.Count - 1
            If ds.Tables("Ballot").Rows(x).Item("Judge") = 0 Then ds.Tables("Ballot").Rows(x).Item("Judge") = -99
        Next
    End Sub

    Private Sub butDeleteRound_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butDeleteRound.Click
        Dim q As Integer = MsgBox("This will delete any pairings or results for this round.  Are you SURE you want to continue?", MsgBoxStyle.YesNo)
        If q = vbNo Then Exit Sub
        Dim fdPanel As DataRow()
        fdPanel = ds.Tables("Panel").Select("Round=" & cboRound.SelectedValue)
        For x = 0 To fdPanel.Length - 1
            fdPanel(x).Delete()
        Next x
        ds.Tables("Panel").AcceptChanges()
        MsgBox("Done")
    End Sub

    Private Sub butEraseResults_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butEraseResults.Click
        Dim q As Integer = MsgBox("This will leave the pairings in tack but delete all results for this round.  Are you SURE you want to continue?", MsgBoxStyle.YesNo)
        If q = vbNo Then Exit Sub
        Dim fdPanel, fdBallot, fdBalScore As DataRow()
        fdPanel = ds.Tables("Panel").Select("Round=" & cboRound.SelectedValue)
        For x = 0 To fdPanel.Length - 1
            fdBallot = ds.Tables("Ballot").Select("Panel=" & fdPanel(x).Item("ID"))
            For y = 0 To fdBallot.Length - 1
                fdBalScore = ds.Tables("Ballot_Score").Select("Ballot=" & fdBallot(y).Item("ID"))
                For z = fdBalScore.Length - 1 To 0 Step -1
                    fdBalScore(z).Delete()
                Next z
            Next y
        Next x
        ds.Tables("Panel").AcceptChanges()
        MsgBox("Done")
    End Sub

    Private Sub Button15_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button15.Click
        For x = ds.Tables("JudgePref").Rows.Count - 1 To 0 Step -1
            If ds.Tables("JudgePref").Rows(x).Item("Rating") = 0 Then ds.Tables("JudgePref").Rows(x).Delete()
        Next x
        ds.AcceptChanges()
    End Sub

    Private Sub Button16_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button16.Click
        Dim dt As New DataTable
        dt.Columns.Add(("Team"), System.Type.GetType("System.String"))
        Dim dtPairing As New DataTable
        dtPairing.Columns.Add(("Round"), System.Type.GetType("System.String"))
        dtPairing.Columns.Add(("Aff"), System.Type.GetType("System.String"))
        dtPairing.Columns.Add(("Neg"), System.Type.GetType("System.String"))
        Dim fdTeam As DataRow() : Dim dr As DataRow
        Dim dummy, strUnit, Aff As String : strUnit = "" : Aff = ""
        Dim colct As Integer
        For Each line As String In TextBox1.Lines '<- array created here
            dummy = line.ToString
            If dummy = "" Then Exit For
            colct = 0
            For x = 1 To dummy.Length
                If Asc(Mid(dummy, x, 1)) = 9 Or x = dummy.Length Then
                    If x = dummy.Length Then strUnit &= Mid(dummy, x, 1)
                    fdTeam = dt.Select("Team='" & strUnit & "'")
                    If fdTeam.Length = 0 Then
                        dr = dt.NewRow : dr.Item("Team") = strUnit : dt.Rows.Add(dr)
                    End If
                    colct += 1
                    If Int(colct / 2) = (colct / 2) Then
                        dr = dtPairing.NewRow
                        dr.Item("Aff") = Aff
                        dr.Item("Round") = colct / 2
                        dr.Item("Neg") = strUnit
                        dtPairing.Rows.Add(dr)
                    Else
                        Aff = strUnit
                    End If
                    strUnit = ""
                Else
                    strUnit &= Mid(dummy, x, 1)
                End If
            Next x
        Next line
        dt.DefaultView.Sort = "team ASC"
        dtPairing.DefaultView.Sort = "round asc"
        DataGridView1.DataSource = dtPairing
        'now convert it to the string
        For x = 0 To dt.DefaultView.Count - 1
            For y = 0 To dtPairing.DefaultView.Count - 1
                If dtPairing.DefaultView(y).Item("Aff").trim = dt.DefaultView(x).Item("Team").trim Then
                    dtPairing.DefaultView(y).Item("Aff") = x.ToString
                    If dtPairing.DefaultView(y).Item("Aff").trim.length = 1 Then dtPairing.DefaultView(y).Item("Aff") = "0" & dtPairing.DefaultView(y).Item("Aff")
                End If
                If dtPairing.DefaultView(y).Item("Neg").trim = dt.DefaultView(x).Item("Team").trim Then
                    dtPairing.DefaultView(y).Item("Neg") = x.ToString
                    If dtPairing.DefaultView(y).Item("Neg").trim.length = 1 Then dtPairing.DefaultView(y).Item("Neg") = "0" & dtPairing.DefaultView(y).Item("Neg")
                End If
                TextBox1.Text &= dtPairing.DefaultView(y).Item("Round") & dtPairing.DefaultView(y).Item("Aff") & dtPairing.DefaultView(y).Item("Neg") & Chr(10) & Chr(13)
            Next y
        Next x
        TextBox1.Text = dt.Rows.Count & "/"
        Dim Cts(5) As Integer : For x = 1 To 5 : Cts(x) = 0 : Next x
        For x = 0 To dt.Rows.Count - 1
            Cts(Val(Mid(dt.Rows(x).Item("Team"), 1, 1))) += 1
        Next x
        For y = 1 To 5
            TextBox1.Text &= Cts(y)
        Next y
        TextBox1.Text &= Chr(13) & Chr(10)
        For y = 0 To dtPairing.DefaultView.Count - 1
            TextBox1.Text &= dtPairing.DefaultView(y).Item("Round") & dtPairing.DefaultView(y).Item("Aff") & dtPairing.DefaultView(y).Item("Neg") & Chr(13) & Chr(10)
        Next y
    End Sub

    Private Sub butBackUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butBackUp.Click
        FolderBrowserDialog1.Description = "Select location to copy backup to:"
        FolderBrowserDialog1.ShowDialog()
        Dim strFolderLocation As String = FolderBrowserDialog1.SelectedPath
        If strFolderLocation.ToUpper & "\TOURNEYDATA.XML" = strFilePath.ToUpper Then
            MsgBox("You can't copy the file on top of itself!  Please try again, and select a different location.")
            Exit Sub
        End If
        File.Decrypt(strFilePath)
        File.Copy(strFilePath, strFolderLocation & "\TourneyData.xml", True)
        MsgBox("Backup complete")
    End Sub

    Private Sub Button17_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button17.Click
        My.Settings.UserName = ""
        My.Settings.PassWord = ""
        Call PassWordCheck()
    End Sub

    Private Sub butBallotClean_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butBallotClean.Click
        Dim dr As DataRow
        For x = ds.Tables("Panel").Rows.Count - 1 To 0 Step -1
            dr = ds.Tables("Round").Rows.Find(ds.Tables("Panel").Rows(x).Item("Round"))
            If dr Is Nothing Then
                ds.Tables("Panel").Rows(x).Delete()
            End If
            dr = ds.Tables("Event").Rows.Find(dr.Item("Event"))
            If dr Is Nothing Then
                ds.Tables("Panel").Rows(x).Delete()
            End If
        Next x
        MsgBox("Done")
    End Sub

    Private Sub Button18_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button18.Click
        Dim drEn As DataRow
        For x = ds.Tables("Panel").Rows.Count - 1 To 0 Step -1
            drEn = ds.Tables("round").Rows.Find(ds.Tables("Panel").Rows(x).Item("Round"))
            If drEn Is Nothing Then ds.Tables("Panel").Rows(x).Delete()
            If drEn.Item("Rd_Name") > 9 Then ds.Tables("Panel").Rows(x).Delete()
        Next x
        MsgBox("Done")
    End Sub

    Private Sub butNames_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butNames.Click
        'Dim stLN As StreamReader = File.OpenText("C:\Users\jbruschke\Documents\Out of the Park Developments\OOTP Baseball 11\database\names.txt")
        'Dim stFN As StreamReader = File.OpenText("C:\Users\jbruschke\Documents\Out of the Park Developments\OOTP Baseball 11\database\first_names.txt")
        Dim stLN As StreamReader = File.OpenText("C:\Users\jbruschke\Documents\CAT\RandomLastNames.txt")
        Dim stFN As StreamReader = File.OpenText("C:\Users\jbruschke\Documents\CAT\RandomFirstNames.txt")
        Dim intLN, intFN As Integer : Dim LN(100000), FN(100000)
        While stLN.Peek > -1
            intLN = intLN + 1
            LN(intLN) = stLN.ReadLine
            If InStr(LN(intLN), ",") > 0 Then
                LN(intLN) = Mid(LN(intLN), 1, InStr(LN(intLN), ",") - 1)
            End If
        End While
        While stFN.Peek > -1
            FN(intFN) = stFN.ReadLine
            intFN = intFN + 1
            'FN(intFN) = Mid(FN(intFN), 1, InStr(FN(intFN), ",") - 1)
        End While
        Dim dummy As Integer
        For x = 0 To ds.Tables("Entry_Student").Rows.Count - 1
            dummy = Int(Rnd() * intFN)
            ds.Tables("Entry_Student").Rows(x).Item("First") = FN(dummy)
        Next
        For x = 0 To ds.Tables("Entry_Student").Rows.Count - 1
            dummy = Int(Rnd() * intLN)
            ds.Tables("Entry_Student").Rows(x).Item("Last") = LN(dummy)
        Next
        For x = 0 To ds.Tables("Judge").Rows.Count - 1
            dummy = Int(Rnd() * intFN)
            ds.Tables("Judge").Rows(x).Item("First") = FN(dummy)
        Next
        For x = 0 To ds.Tables("Judge").Rows.Count - 1
            dummy = Int(Rnd() * intLN)
            ds.Tables("Judge").Rows(x).Item("Last") = LN(dummy)
        Next
        MsgBox("Done -- you probably want to remake the acronmys and team names")
    End Sub

    Private Sub butKillNulls_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butKillNulls.Click
        For x = 0 To ds.Tables.Count - 1
            Label1.Text = x & " of " & ds.Tables.Count - 1 : Label1.Refresh()
            Call NullKiller(ds, ds.Tables(x).TableName)
        Next x
        Label1.Text = "Done"
    End Sub

    Private Sub Button19_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button19.Click
        GroupBox2.Visible = True
    End Sub

    Private Sub Button20_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button20.Click
        Dim fdRow As DataRow()
        fdRow = ds.Tables("Ballot").Select("Judge = null")
        DataGridView1.DataSource = ds.Tables("Ballot")
    End Sub

    Private Sub Button21_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button21.Click
        For x = 0 To ds.Tables.Count - 1
            Label1.Text = x & " of " & ds.Tables.Count - 1 : Label1.Refresh()
            Call NullKiller(ds, ds.Tables(x).TableName)
        Next x
        MsgBox("Done")
    End Sub

    Private Sub butRoundLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butRoundLoad.Click
        Dim ds2 As New DataSet
        Dim xmlFile As XmlReader
        xmlFile = XmlReader.Create(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\CAT\TourneyDataClarion2.xml", New XmlReaderSettings())
        ds2.ReadXml(xmlFile, XmlReadMode.InferSchema)
        Dim dt As New DataTable
        dt.Columns.Add(("CurrentRound"), System.Type.GetType("System.String"))
        dt.Columns.Add(("ID"), System.Type.GetType("System.Int64"))
        dt.Columns.Add(("NewID"), System.Type.GetType("System.Int64"))
        dt.Columns.Add(("OnlineRd"), System.Type.GetType("System.String"))
        dt.Columns.Add(("OnlineID"), System.Type.GetType("System.Int64"))
        Dim dr As DataRow
        ds.Tables("Round").DefaultView.Sort = "Event ASC, rd_Name asc"
        For x = 0 To ds.Tables("ROund").Rows.Count - 1
            dr = dt.NewRow
            dr.Item("CurrentRound") = ds.Tables("Round").DefaultView(x).Item("Label")
            dr.Item("ID") = ds.Tables("Round").DefaultView(x).Item("ID")
            dt.Rows.Add(dr)
        Next x
        ds2.Tables("Round").DefaultView.Sort = "Event ASC"
        For x = 0 To dt.Rows.Count - 1
            If x <= ds2.Tables("Round").Rows.Count - 1 Then
                dt.Rows(x).Item("OnlineRd") = ds2.Tables("Round").DefaultView(x).Item("Label")
                dt.Rows(x).Item("OnlineID") = ds2.Tables("Round").DefaultView(x).Item("ID")
                dt.Rows(x).Item("NewID") = ds2.Tables("Round").DefaultView(x).Item("ID")
            End If
        Next
        DataGridView1.DataSource = dt
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    End Sub

    Private Sub butRoundMatch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butRoundMatch.Click
        Dim oldRd, newRd As Integer
        For x = 0 To DataGridView1.Rows.Count - 1
            oldRd = DataGridView1.Rows(x).Cells("ID").Value
            If Not DataGridView1.Rows(x).Cells("NewID").Value Is System.DBNull.Value Then
                newRd = DataGridView1.Rows(x).Cells("NewID").Value
                For y = 0 To ds.Tables("Round").Rows.Count - 1
                    If ds.Tables("Round").Rows(y).Item("ID") = oldRd Then ds.Tables("Round").Rows(y).Item("ID") = newRd
                Next y
                For y = 0 To ds.Tables("Panel").Rows.Count - 1
                    If ds.Tables("Panel").Rows(y).Item("ROund") = oldRd Then ds.Tables("Panel").Rows(y).Item("Round") = newRd
                Next y
                For y = 0 To ds.Tables("ElimSeed").Rows.Count - 1
                    If ds.Tables("ElimSeed").Rows(y).Item("ROund") = oldRd Then ds.Tables("ElimSeed").Rows(y).Item("Round") = newRd
                Next y
                For y = 0 To ds.Tables("Bracket").Rows.Count - 1
                    If ds.Tables("Bracket").Rows(y).Item("ROund") = oldRd Then ds.Tables("Bracket").Rows(y).Item("Round") = newRd
                Next y
            End If
        Next x
        MsgBox("Done")
    End Sub

    Private Sub butTimeSLotLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butTimeSLotLoad.Click
        Dim ds2 As New DataSet
        Dim xmlFile As XmlReader
        xmlFile = XmlReader.Create(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\CAT\TourneyDataClarion2.xml", New XmlReaderSettings())
        ds2.ReadXml(xmlFile, XmlReadMode.InferSchema)
        Dim dt As New DataTable
        dt.Columns.Add(("CurrentTS"), System.Type.GetType("System.String"))
        dt.Columns.Add(("ID"), System.Type.GetType("System.Int64"))
        dt.Columns.Add(("NewID"), System.Type.GetType("System.Int64"))
        dt.Columns.Add(("OnlineTS"), System.Type.GetType("System.String"))
        dt.Columns.Add(("OnlineID"), System.Type.GetType("System.Int64"))
        Dim dr As DataRow
        ds.Tables("Timeslot").DefaultView.Sort = "ID ASC"
        For x = 0 To ds.Tables("Timeslot").Rows.Count - 1
            dr = dt.NewRow
            dr.Item("CurrentTS") = ds.Tables("Timeslot").DefaultView(x).Item("TimeSLotName")
            dr.Item("ID") = ds.Tables("Timeslot").DefaultView(x).Item("ID")
            dt.Rows.Add(dr)
        Next x
        ds2.Tables("Timeslot").DefaultView.Sort = "ID ASC"
        For x = 0 To dt.Rows.Count - 1
            If x <= ds2.Tables("Timeslot").Rows.Count - 1 Then
                dt.Rows(x).Item("OnlineTS") = ds2.Tables("Timeslot").DefaultView(x).Item("TimeSLotName")
                dt.Rows(x).Item("OnlineID") = ds2.Tables("Timeslot").DefaultView(x).Item("ID")
                dt.Rows(x).Item("NewID") = ds2.Tables("Timeslot").DefaultView(x).Item("ID")
            End If
        Next
        DataGridView1.DataSource = dt
    End Sub

    Private Sub butTSMatch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butTSMatch.Click
        Dim oldID, newID As Integer
        For x = 0 To DataGridView1.Rows.Count - 1
            oldID = DataGridView1.Rows(x).Cells("ID").Value
            If Not DataGridView1.Rows(x).Cells("NewID").Value Is System.DBNull.Value Then
                newID = DataGridView1.Rows(x).Cells("NewID").Value
                For y = 0 To ds.Tables("Timeslot").Rows.Count - 1
                    If ds.Tables("timeslot").Rows(y).Item("ID") = oldID Then ds.Tables("timeslot").Rows(y).Item("ID") = newID
                Next y
                For y = 0 To ds.Tables("Round").Rows.Count - 1
                    If ds.Tables("ROund").Rows(y).Item("TImeslot") = oldID Then ds.Tables("ROund").Rows(y).Item("Timeslot") = newID
                Next y
                For y = 0 To ds.Tables("Judge_Assign_Param").Rows.Count - 1
                    If ds.Tables("Judge_Assign_Param").Rows(y).Item("TImeslot") = oldID Then ds.Tables("Judge_Assign_Param").Rows(y).Item("Timeslot") = newID
                Next y
                For y = 0 To ds.Tables("Event").Columns.Count - 1
                    If ds.Tables("Event").Columns(y).ColumnName = "TIMESLOT" & oldID Then ds.Tables("Event").Columns(y).ColumnName = "TIMESLOT" & newID
                Next y
                For y = 0 To ds.Tables("Judge").Columns.Count - 1
                    If ds.Tables("Judge").Columns(y).ColumnName = "TIMESLOT" & oldID Then ds.Tables("Judge").Columns(y).ColumnName = "TIMESLOT" & newID
                Next y
            End If
        Next x
        MsgBox("Done")
    End Sub

    Private Sub Button22_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button22.Click
        Dim max As Integer = InputBox("Enter max number of rounds for commitment:")
        For x = 0 To ds.Tables("Judge").Rows.Count - 1
            If ds.Tables("Judge").Rows(x).Item("Obligation") > max Then ds.Tables("Judge").Rows(x).Item("Obligation") = max
        Next
        MsgBox("Done")
    End Sub

    Private Sub butTBFix_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butTBFix.Click
        Call InitializeTieBreakers(ds, True)
        Call InitializeScoreSettings(ds)

        'pull existing tiebreaker set for teams
        Dim fdRow As DataRow()
        fdRow = ds.Tables("Tiebreak_set").Select("TBSET_NAME = '2 teams - Team Prelims'")
        'pull the tb_SEt number used in the download
        Dim onlineTB As Integer = ds.Tables("Round").Rows(0).Item("TB_SET")
        'write that number on top of TB_SETs
        For x = 0 To fdRow.Length - 1
            fdRow(x).Item("ID") = onlineTB
        Next x
        MsgBox("Done")
    End Sub

    Private Sub Button23_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button23.Click
        Dim URL As String = "https://www.tabroom.com/api/student_name.mhtml?username=jbruschke@fullerton.edu&password=123Eadie&firstname=max&lastname=bugrov&school_id=33447"
        URL = "https://www.tabroom.com/api/judge_name.mhtml?username=jbruschke@fullerton.edu&password=123Eadie&firstname=max&lastname=bugrov&school_id=33447"
        Dim request As HttpWebRequest = WebRequest.Create(URL)
        Dim response As HttpWebResponse = request.GetResponse()
        Dim reader As StreamReader = New StreamReader(response.GetResponseStream())
        Dim str As String
        str = reader.ReadToEnd
        TextBox1.Text = str
        reader.Close() : response.Close()
    End Sub

    Private Sub Button24_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button24.Click
        For x = ds.Tables("ballot_score").Rows.Count - 1 To 0 Step -1
            If ds.Tables("ballot_score").Rows(x).Item("Score_ID") = 4 Then ds.Tables("ballot_score").Rows(x).Delete() : Label1.Text = x
        Next x
        MsgBox("Done")
    End Sub

    Private Sub Button25_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button25.Click
        Dim q As Integer = InputBox("Enter team number:")
        Call DeletePrefsByTeam(q, ds)
        MsgBox("Done")
    End Sub

    Private Sub butMakeOrdCats_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butMakeOrdCats.Click
        MsgBox("This will create an ordcat column in the judgepref table if one doesn't exist, and set up a grid to convert ordinal percentiles to categories.  Edit the ordinal percentiles as you like and then click the fire button to the left of the grid to perform the conversion.")
        butOrdCatFire.Visible = True
        'create ORDCAT column if it doesn't exit
        If ds.Tables("JudgePref").Columns.Contains("OrdCat") = False Then
            ds.Tables("JudgePref").Columns.Add(("OrdPct"), System.Type.GetType("System.Int16"))
        End If
        'populate the grid
        Dim dt As New DataTable
        dt.Columns.Add(("OrdPct"), System.Type.GetType("System.Single"))
        dt.Columns.Add(("Cat"), System.Type.GetType("System.Single"))
        Dim dr As DataRow
        For x = 1 To 6
            dr = dt.NewRow
            dr.Item("Cat") = x
            dr.Item("OrdPct") = 16.67 * x
            dt.Rows.Add(dr)
        Next x
        DataGridView1.DataSource = dt
    End Sub

    Private Sub butOrdCatFire_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butOrdCatFire.Click
        For x = 0 To ds.Tables("JudgePref").Rows.Count - 1
            Label1.Text = x & "/" & ds.Tables("JudgePref").Rows.Count - 1 : Label1.Refresh()
            For y = 0 To DataGridView1.RowCount - 1
                If Not ds.Tables("JudgePref").Rows(x).Item("OrdPct") = Nothing Then
                    If ds.Tables("JudgePref").Rows(x).Item("OrdPct") <= DataGridView1.Rows(y).Cells("OrdPct").Value Then
                        ds.Tables("JudgePref").Rows(x).Item("OrdCat") = DataGridView1.Rows(y).Cells("Cat").Value
                        Exit For
                    End If
                End If
            Next y
        Next x
        Call SaveFile(ds)
        MsgBox("Done")
    End Sub
End Class