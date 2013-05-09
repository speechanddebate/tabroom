Imports System.IO
Imports System.Net
Imports System.Xml

Module modBallots
    Sub DoNewBallotDownLoad(ByVal ds As DataSet, ByVal Round As Integer)
        'Hey -- only download the event that's in question?
        Dim dr As DataRow : dr = ds.Tables("Round").Rows.Find(Round)
        Call PassWordCheck()
        Dim URL As String = "https://www.tabroom.com/api/download_round.mhtml?username=" & My.Settings.UserName & "&password=" & My.Settings.PassWord & "&tourn_id=" & ds.Tables("Tourn").Rows(0).Item("ID") & "&round_id=" & Round
        'Label1.Text = "Opening site..." : Label1.Refresh()
        Try
            Dim request As HttpWebRequest = WebRequest.Create(URL)
            Dim response As HttpWebResponse = request.GetResponse()
            Dim reader As StreamReader = New StreamReader(response.GetResponseStream())
            Dim st As StreamWriter = File.CreateText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\CAT\BallotDownload.xml")
            st.WriteLine(reader.ReadToEnd())
            st.Close()
            response.Close() : reader.Close()
        Catch
            MsgBox("The ballot download failed.  This may be due to an incorrect password or lost internet connection.  You can reset your password on the utilities screen.")
        End Try
    End Sub
    Function showelimballots3(ByRef ds As DataSet, ByVal Round As Integer) As DataTable
        Dim ds2 As New DataSet
        'read ballot file into ds2
        Dim xmlFile As XmlReader
        xmlFile = XmlReader.Create(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\CAT\BallotDownload.xml", New XmlReaderSettings())
        Dim strXsdLocation As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\CAT\TourneyData.xsd"
        ds2.ReadXmlSchema(strXsdLocation)
        ds2.ReadXml(xmlFile, XmlReadMode.InferSchema)
        xmlFile.Close()
        'pull from the web round; assumes event and rd_name, not ID
        Dim dr, drT2 As DataRow : Dim dr2 As DataRow()
        dr = ds.Tables("Round").Rows.Find(Round)
        dr2 = ds2.Tables("Round").Select("Event=" & dr.Item("Event") & " and rd_name='" & dr.Item("rd_name") & "'")
        Dim dt2 As New DataTable
        dt2.Columns.Add("Judge", System.Type.GetType("System.String"))
        dt2.Columns.Add("VotesFor", System.Type.GetType("System.String"))
        Dim fdballotscores, fdBallot As DataRow()
        For x = 0 To ds2.Tables("Ballot").Rows.Count - 1
            'set the side; not finding round, because ONLY downloading debates for this round
            fdBallot = ds.Tables("Ballot").Select("JUDGE=" & ds2.Tables("Ballot").Rows(x).Item("Judge") & " and ENTRY=" & ds2.Tables("Ballot").Rows(x).Item("Entry") & " and " & BuildPanelStringByRound(ds, Round))
            If fdballot.Length = 1 Then
                fdballot(0).Item("Side") = ds2.Tables("Ballot").Rows(x).Item("Side")
            Else
                MsgBox("Found an online ballot but can't find a local ballot to match it to.  Exiting attempt.")
                Exit For
            End If
            'load the scores
            fdballotscores = ds2.Tables("Ballot_Score").Select("Ballot=" & ds2.Tables("Ballot").Rows(x).Item("ID"))
            For y = 0 To fdballotscores.Length - 1
                If fdballotscores(y).Item("Score_ID") = 1 And fdballotscores(y).Item("Score") = 1 Then
                    dr = ds.Tables("Judge").Rows.Find(ds2.Tables("Ballot").Rows(x).Item("Judge"))
                    drT2 = dt2.NewRow
                    drT2.Item("Judge") = dr.Item("Last") & ", " & dr.Item("First")
                    dr = ds.Tables("Entry").Rows.Find(ds2.Tables("Ballot").Rows(x).Item("Entry"))
                    drT2.Item("VotesFor") = dr.Item("Fullname")
                    dt2.Rows.Add(drT2)
                    Call StoreResult(ds, dt2, ds2.Tables("Ballot").Rows(x).Item("Judge"), ds2.Tables("Ballot").Rows(x).Item("Entry"), Round)
                End If
            Next y
        Next x
        Return dt2
    End Function
    Function showelimballots4(ByRef ds As DataSet, ByVal Round As Integer) As DataTable
        Dim ds2 As New DataSet
        'read ballot file into ds2
        Dim xmlFile As XmlReader
        xmlFile = XmlReader.Create(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\CAT\BallotDownload.xml", New XmlReaderSettings())
        Dim strXsdLocation As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\CAT\TourneyData.xsd"
        ds2.ReadXml(xmlFile, XmlReadMode.InferSchema)
        xmlFile.Close()
        'pull from the web round; assumes event and rd_name, not ID
        Dim dt2 As New DataTable
        dt2.Columns.Add("Judge", System.Type.GetType("System.String"))
        dt2.Columns.Add("VotesFor", System.Type.GetType("System.String"))
        Dim fdballotscores, fdBallot As DataRow()
        Dim dr, drT2 As DataRow
        For x = 0 To ds2.Tables("Ballot").Rows.Count - 1
            If ds2.Tables("Ballot").Rows(x).Item("Judge") <> "" Then
                'set the side; not finding round, because ONLY downloading debates for this round
                fdBallot = ds.Tables("Ballot").Select("JUDGE=" & ds2.Tables("Ballot").Rows(x).Item("Judge") & " and ENTRY=" & ds2.Tables("Ballot").Rows(x).Item("Entry") & " and " & BuildPanelStringByRound(ds, Round))
                If fdBallot.Length = 1 Then
                    fdBallot(0).Item("Side") = ds2.Tables("Ballot").Rows(x).Item("Side")
                Else
                    MsgBox("Found an online ballot but can't find a local ballot to match it to.  Exiting attempt.")
                    Exit For
                End If
                'load the scores
                If Not ds2.Tables("Ballot_Score") Is Nothing Then
                    fdballotscores = ds2.Tables("Ballot_Score").Select("Ballot=" & ds2.Tables("Ballot").Rows(x).Item("ID"))
                    For y = 0 To fdballotscores.Length - 1
                        If fdballotscores(y).Item("Score_ID") = 1 And fdballotscores(y).Item("Score") = 1 Then
                            dr = ds.Tables("Judge").Rows.Find(ds2.Tables("Ballot").Rows(x).Item("Judge"))
                            drT2 = dt2.NewRow
                            drT2.Item("Judge") = dr.Item("Last") & ", " & dr.Item("First")
                            dr = ds.Tables("Entry").Rows.Find(ds2.Tables("Ballot").Rows(x).Item("Entry"))
                            drT2.Item("VotesFor") = dr.Item("Fullname")
                            dt2.Rows.Add(drT2)
                            Call StoreResult(ds, dt2, ds2.Tables("Ballot").Rows(x).Item("Judge"), ds2.Tables("Ballot").Rows(x).Item("Entry"), Round)
                        End If
                    Next y
                End If
            End If
        Next x
        Return dt2
    End Function

    Sub StoreResult(ByRef ds As DataSet, ByVal dt As DataTable, ByVal judge As Integer, ByVal entry As Integer, ByVal round As Integer)
        'receives ballotdownload Dt, judge#, entry#, and datarow with the round
        Dim fdlocalballots, fdLocalScores As DataRow()
        fdlocalballots = PullAllBallotsInRound(ds, round, "Judge ASC") 'now holds all ballots from the local file for this round
        'make sure all the scores are there
        For x = 0 To fdlocalballots.Length - 1
            ValidateScoresByBallot(ds, fdlocalballots(x).Item("ID"))
        Next x
        'match ballots by judge and entry
        'match scores by recipient and score_ID, and over-write
        For x = 0 To fdlocalballots.Length - 1
            'Label1.Text = "Checking " & x & " of " & fdlocalballots.Length - 1 : Label1.Refresh()
            If IsBallotIn(ds, fdlocalballots(x).Item("ID")) = False Then
                If judge = fdlocalballots(x).Item("Judge") And entry = fdlocalballots(x).Item("Entry") Then
                    fdLocalScores = ds.Tables("Ballot_Score").Select("Ballot=" & fdlocalballots(x).Item("ID"))
                    For w = 0 To fdLocalScores.Length - 1
                        If entry = fdLocalScores(w).Item("Recipient") And fdLocalScores(w).Item("Score_ID") = 1 Then
                            fdLocalScores(w).Item("Score") = 1
                        End If
                    Next w
                End If
            End If
        Next x
    End Sub
    Function IsBallotIn(ByVal ds As DataSet, ByVal ballotID As Integer) As Boolean
        IsBallotIn = False
        Dim drBallotScore As DataRow()
        drBallotScore = DS.Tables("Ballot_Score").Select("Ballot=" & ballotID)
        For z = 0 To drBallotScore.Length - 1
            If drBallotScore(z).Item("Score") > 0 Then IsBallotIn = True : Exit For
        Next z
    End Function
    Function GetWinner(ByVal DS As DataSet, ByVal Panel As Integer, ByVal EventID As Integer, ByRef strBallotCount As String) As Integer
        'returns the number of the winning team
        Dim fdBallots As DataRow()
        fdBallots = DS.Tables("Ballot").Select("Panel=" & Panel)
        Dim nTeams As Integer = getEventSetting(DS, EventID, "TeamsPerRound")
        Dim nVotes(nTeams, 2) As Integer
        'assign all teams to a row of the array
        Dim found As Boolean : Dim NextOpen As Integer
        For x = 0 To fdBallots.Length - 1
            found = False : NextOpen = -1
            For y = 0 To nTeams - 1
                If nVotes(y, 1) = fdBallots(x).Item("Entry") Then found = True
                If nVotes(y, 1) = 0 And NextOpen = -1 Then NextOpen = y
            Next y
            If found = False Then nVotes(NextOpen, 1) = fdBallots(x).Item("Entry")
        Next x
        'count votes
        Dim score As Integer
        For x = 0 To fdBallots.Length - 1
            For y = 0 To nTeams - 1
                If nVotes(y, 1) = fdBallots(x).Item("Entry") Then
                    score = GetBallot_Score(DS, fdBallots(x).Item("ID"), 1)
                    If score = 1 Or score = 2 Then nVotes(y, 2) += 1
                End If
            Next y
        Next x
        'find winner and return; -1 means there's a tie
        GetWinner = -1
        Dim WinningVoteTotal As Integer = -1
        For x = 0 To nTeams - 1
            If nVotes(x, 2) > WinningVoteTotal Then
                GetWinner = nVotes(x, 1)
                WinningVoteTotal = nVotes(x, 2)
                strBallotCount = nVotes(x, 2).ToString.Trim
                If x = 0 Then strBallotCount &= "-" & nVotes(1, 2).ToString.Trim Else strBallotCount &= "-" & nVotes(0, 2).ToString.Trim
            ElseIf nVotes(x, 2) = WinningVoteTotal Then
                GetWinner = -1
            End If
        Next
    End Function
End Module
