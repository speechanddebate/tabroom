Imports System.Xml
Imports System.IO

Module modGlobals
    Public strFilePath As String

    Public Sub SaveFile(ByVal DS As DataSet)
        Dim xmlFile As XmlWriter
        xmlFile = XmlWriter.Create(strFilePath)
        Try
            DS.WriteXml(xmlFile)
        Catch e As Exception
            MsgBox(e.Message)
        End Try

        xmlFile.Close()
    End Sub
    Public Sub LoadFile(ByRef ds As DataSet, ByVal strDataFileName As String)

        Dim xmlFile As XmlReader
        xmlFile = XmlReader.Create(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\CAT\" & strDataFileName & ".xml", New XmlReaderSettings())
        Dim strXsdLocation As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\CAT\" & strDataFileName & ".xsd"
        ds.ReadXmlSchema(strXsdLocation)
        Try
            ds.ReadXml(xmlFile, XmlReadMode.InferSchema)
        Catch e As Exception
            If InStr(e.Message.ToString.ToUpper, "INPUT STRING") > 0 Then
                Dim q As Integer = MsgBox("There is a problem with the format of the datafile.  Attempt fix?", MsgBoxStyle.YesNo)
                If q = vbYes Then
                    xmlFile.Close()
                    Call NullFixer()
                    Exit Sub
                End If
            End If
            MsgBox(e.Message)
        End Try
        xmlFile.Close()

        'set primary keys and relations IF tourneydata file and not the master file
        If strDataFileName = "TourneyData" Then
            'set primary keys; could do in .xml schema, but it doesn't save any code
            Call MakePrimaryKey(ds, "ENTRY", "ID") : Call MakePrimaryKey(ds, "ENTRY_STUDENT", "ID")
            Call MakePrimaryKey(ds, "EVENT", "ID") : Call MakePrimaryKey(ds, "EVENT_SETTING", "ID")
            Call MakePrimaryKey(ds, "ROUND", "ID") : Call MakePrimaryKey(ds, "PANEL", "ID")
            Call MakePrimaryKey(ds, "BALLOT", "ID") : Call MakePrimaryKey(ds, "BALLOT_SCORE", "ID")
            Call MakePrimaryKey(ds, "SCORES", "ID") : Call MakePrimaryKey(ds, "ELIMSEED", "ID")
            Call MakePrimaryKey(ds, "TIEBREAK", "ID") : Call MakePrimaryKey(ds, "SCORE_SETTING", "ID")
            Call MakePrimaryKey(ds, "JUDGE", "ID") : Call MakePrimaryKey(ds, "TIMESLOT", "ID")
            Call MakePrimaryKey(ds, "BRACKET", "ID") : Call MakePrimaryKey(ds, "SCHOOL", "ID")
            Call MakePrimaryKey(ds, "ROOM", "ID") : Call MakePrimaryKey(ds, "TEAMBLOCK", "ID")
            Call MakePrimaryKey(ds, "JUDGEPREF", "ID") : Call MakePrimaryKey(ds, "TIEBREAK_SET", "ID")

            ' Establish a relationship between the two tables.  Necessary for the grids to work
            Dim relation As New DataRelation("TeamEntry", ds.Tables("ENTRY").Columns("ID"), ds.Tables("ENTRY_STUDENT").Columns("Entry"))
            ds.Relations.Add(relation)

            'set up relations for results so you can do cascating updates

            'round to panel
            If ds.Tables.IndexOf("ROUND") > -1 And ds.Tables.IndexOf("PANEL") > -1 Then
                Dim relation1 As New DataRelation("RdPanel", ds.Tables("ROUND").Columns("ID"), ds.Tables("PANEL").Columns("Round"))
                ds.Relations.Add(relation1)
            End If

            'panel to ballot
            If ds.Tables.IndexOf("PANEL") > -1 And ds.Tables.IndexOf("BALLOT") > -1 Then
                Dim relation2 As New DataRelation("PanBallot", ds.Tables("PANEL").Columns("ID"), ds.Tables("Ballot").Columns("Panel"))
                ds.Relations.Add(relation2)
            End If

            'ballot to ballot_score
            If ds.Tables.IndexOf("BALLOT") > -1 And ds.Tables.IndexOf("BALLOT_SCORE") > -1 Then
                Dim relation3 As New DataRelation("BalScore", ds.Tables("BALLOT").Columns("ID"), ds.Tables("Ballot_Score").Columns("Ballot"))
                ds.Relations.Add(relation3)
            End If

            'EVENT to EVENT_SETTING
            If ds.Tables.IndexOf("EVENT") > -1 And ds.Tables.IndexOf("EVENT_SETTING") > -1 Then
                Dim relation3 As New DataRelation("EventSetting", ds.Tables("EVENT").Columns("ID"), ds.Tables("Event_Setting").Columns("EVENT"))
                ds.Relations.Add(relation3)
            End If

            'EVENT to ENTRY
            If ds.Tables.IndexOf("EVENT") > -1 And ds.Tables.IndexOf("EVENT_SETTING") > -1 Then
                Dim relation3 As New DataRelation("EventEntry", ds.Tables("EVENT").Columns("ID"), ds.Tables("Entry").Columns("EVENT"))
                ds.Relations.Add(relation3)
            End If

            'Tiebreak_Set to tiebreak
            If ds.Tables.IndexOf("TIEBREAK_SET") > -1 And ds.Tables.IndexOf("TIEBREAK") > -1 Then
                Dim relation3 As New DataRelation("TieBreakSetToFunction", ds.Tables("TIEBREAK_SET").Columns("ID"), ds.Tables("Tiebreak").Columns("TB_SET"))
                ds.Relations.Add(relation3)
            End If

            'Tiebreak_Set to Score_Setting
            If ds.Tables.IndexOf("TIEBREAK_SET") > -1 And ds.Tables.IndexOf("TIEBREAK") > -1 Then
                Dim relation3 As New DataRelation("TieBreakSetToScoreSetting", ds.Tables("TIEBREAK_SET").Columns("ID"), ds.Tables("Score_Setting").Columns("TB_SET"))
                ds.Relations.Add(relation3)
            End If

            'Judge to JudgePref
            If ds.Tables.IndexOf("TIEBREAK_SET") > -1 And ds.Tables.IndexOf("TIEBREAK") > -1 Then
                Dim relation3 As New DataRelation("JudgeToPref", ds.Tables("JUDGE").Columns("ID"), ds.Tables("JudgePref").Columns("Judge"))
                ds.Relations.Add(relation3)
            End If

        End If

    End Sub
    Sub MakePrimaryKey(ByRef ds As DataSet, ByVal strDT As String, ByVal strColumn As String)
        'If DupePVCheck(ds, strDT, strColumn) = False Then
        'Exit Sub
        ' End If
        Dim x As Integer = ds.Tables(strDT).Columns.IndexOf(strColumn)
        ds.Tables(strDT).Constraints.Add("PrimaryKey", ds.Tables(strDT).Columns(x), True)
        ds.Tables(strDT).Columns(strColumn).AutoIncrement = True
        ds.Tables(strDT).Columns(strColumn).Unique = True
        Dim dtv As New DataView(ds.Tables(strDT))
        dtv.Sort = strColumn & " DESC"
        If dtv.Count > 0 Then
            ds.Tables(strDT).Columns(strColumn).AutoIncrementSeed = dtv(0).Item(strColumn) + 1
        Else
            ds.Tables(strDT).Columns(strColumn).AutoIncrementSeed = 1
        End If
    End Sub
    Function DupePVCheck(ByVal ds As DataSet, ByVal strdt As String, ByVal strcolumn As String) As Boolean
        DupePVCheck = True
        For x = 0 To ds.Tables(strdt).Rows.Count - 1
            For y = x + 1 To ds.Tables(strdt).Rows.Count - 1
                If ds.Tables(strdt).Rows(x).Item(strcolumn) = ds.Tables(strdt).Rows(y).Item(strcolumn) Then
                    DupePVCheck = False
                    MsgBox(strdt & " contains duplicate " & strcolumn & " records with the value " & ds.Tables(strdt).Rows(x).Item(strcolumn))
                End If
            Next
        Next
    End Function
    Sub DupeAcroCheck(ByVal ds As DataSet)
        Dim fdDupes As DataRow()
        Dim ctr As Integer
        For x = 0 To ds.Tables("Entry").Rows.Count - 1
            ctr = 1
            Do
                ctr += 1
                fdDupes = ds.Tables("Entry").Select("Code='" & ds.Tables("Entry").Rows(x).Item("Code") & "' and Event=" & ds.Tables("Entry").Rows(x).Item("Event"))
                If fdDupes.Length > 1 Then
                    For y = 0 To fdDupes.Length - 1
                        fdDupes(y).Item("Code") = FullNameMaker(ds, fdDupes(y).Item("ID"), "Code", ctr)
                    Next y
                End If
                If ctr > 10 Then MsgBox("Having trouble fixing duplicate acronyms for " & fdDupes(0).Item("FullName") & ". Please fix manually.") : Exit Do
            Loop Until fdDupes.Length = 1
        Next x
    End Sub
    Public Function FullNameMaker(ByVal DS As DataSet, ByVal strTeam As Integer, ByVal strType As String, ByVal LnLength As Integer) As String
        'strType can be FULL for full names; anything else will return acronyms
        If strTeam = -99 Then FullNameMaker = "None" : Exit Function
        Dim strSchoolField As String = "SchoolName"
        If strType.ToUpper <> "FULL" Then strSchoolField = "Code"
        Dim x, y As Integer : Dim MatchedPrior As Boolean
        Dim StudentRow As DataRow()
        Dim SchoolRow As DataRow()
        'select all students for a team entry
        StudentRow = DS.Tables("Entry_Student").Select("Entry=" & strTeam.ToString, "Last ASC")
        If StudentRow.Length = 0 Then FullNameMaker = "Error - no students for team" : Exit Function
        'add first school
        SchoolRow = DS.Tables("School").Select("ID=" & StudentRow(0).Item("School"))
        FullNameMaker = SchoolRow(0).Item(strSchoolField).trim
        'test against prior schools, add additional school names if it's a hybrid
        For x = 1 To StudentRow.Count - 1
            MatchedPrior = False
            For y = 0 To x - 1
                If StudentRow(y).Item("School") = StudentRow(x).Item("School") Then MatchedPrior = True
            Next y
            If MatchedPrior = False Then
                SchoolRow = DS.Tables("School").Select("ID=" & StudentRow(x).Item("School"))
                If SchoolRow.Length = 0 Then FullNameMaker &= "N/A" Else FullNameMaker &= "\" & SchoolRow(0).Item(strSchoolField).trim
            End If
        Next x
        'add first name
        If strType.ToUpper = "FULL" Then
            FullNameMaker &= " - " & StudentRow(0).Item("Last").trim
        Else
            FullNameMaker &= " " & Mid(StudentRow(0).Item("Last"), 1, LnLength)
            'removed space for TRPC export
            'FullNameMaker &= Mid(StudentRow(0).Item("Last"), 1, LnLength)
        End If
        'add additional names
        For x = 1 To StudentRow.Count - 1
            If strType.ToUpper = "FULL" Then
                FullNameMaker &= " and " & StudentRow(x).Item("Last").trim
            Else
                FullNameMaker &= Mid(StudentRow(x).Item("Last").trim, 1, LnLength)
            End If
        Next x
    End Function
    Public Sub FixMissingSchools(ByRef ds As DataSet, ByVal dtSchools As DataTable)
        'create a hired/no affil school record at -1
        'check judges, competitors, master users table for missing schools

        'removes any missing schools from the master student list
        'enforce ref integ of schools
        Dim foundrow As DataRow()
        Dim x As Integer
        For x = 0 To ds.Tables("USER").Rows.Count - 1
            If ds.Tables("USER").Rows(x).Item("School") Is System.DBNull.Value Then ds.Tables("USER").Rows(x).Item("School") = 11
            If ds.Tables("USER").Rows(x).Item("School") = 0 Then ds.Tables("USER").Rows(x).Item("School") = 11
            foundrow = dtSchools.Select("ID=" & ds.Tables("USER").Rows(x).Item("School"))
            If foundrow.Length = 0 Then
                ds.Tables("USER").Rows(x).Item("School") = 11
            End If
        Next x
    End Sub
    Sub MakeUnBoundComboBox(ByVal strDisplayMember As String, ByVal strValueMember As String, ByVal strDefault As String, ByVal DT As DataTable, ByRef CBO As ComboBox)
        'creates an unbound combobox
        'displaymember must be a a string, valuemember must be an integer
        Dim tempDT As New DataTable
        Dim DR As DataRow
        tempDT.Columns.Add(strDisplayMember, System.Type.GetType("System.String"))
        tempDT.Columns.Add(strValueMember, System.Type.GetType("System.Int16"))
        'set up default DR = tempDT.NewRow
        DR = tempDT.NewRow
        DR.Item(strDisplayMember) = strDefault
        DR.Item(strValueMember) = -1
        tempDT.Rows.Add(DR)
        For x = 0 To DT.Rows.Count - 1
            DR = tempDT.NewRow
            DR.Item(strDisplayMember) = DT.Rows(x).Item(strDisplayMember)
            DR.Item(strValueMember) = DT.Rows(x).Item(strValueMember)
            tempDT.Rows.Add(DR)
        Next x
        tempDT.AcceptChanges()
        CBO.DataSource = tempDT
        CBO.ValueMember = strValueMember
        CBO.DisplayMember = strDisplayMember
    End Sub
    Function GetTagValue(ByVal DT As DataTable, ByVal strTag As String) As String
        GetTagValue = "N/A"
        Dim foundrow As DataRow()
        foundrow = DT.Select("TAG='" & strTag & "'")
        If foundrow.Length = 0 Then Exit Function
        GetTagValue = foundrow(0).Item("Value")
    End Function
    Function getEventSetting(ByVal ds As DataSet, ByVal intEvent As Integer, ByVal strTag As String)
        getEventSetting = 0
        Dim foundrow As DataRow()
        foundrow = ds.Tables("Event_Setting").Select("TAG='" & strTag & "' and Event=" & intEvent)
        If foundrow.Length = 0 Then Exit Function
        getEventSetting = foundrow(0).Item("Value")
    End Function
    Function GetName(ByVal Dt As DataTable, ByVal ID As Integer, ByVal strNameField As String) As String
        GetName = "N/A"
        Dim dr As DataRow
        dr = Dt.Rows.Find(ID)
        If dr Is Nothing Then Exit Function
        GetName = dr.Item(strNameField).trim
    End Function
    Function GetTeamBySpeaker(ByVal DS As DataSet, ByVal Speaker As Integer) As Integer
        'returns entry ID for the team a speakers is on
        Dim foundrow As DataRow()
        foundrow = DS.Tables("ENTRY_STUDENT").Select("ID=" & Speaker.ToString)
        If foundrow.Length = 0 Then GetTeamBySpeaker = -99
        GetTeamBySpeaker = foundrow(0).Item("School")
    End Function
    Function GetSpeakersByTeam(ByVal DS As DataSet, ByVal team As Integer, ByVal strIDString As String) As String
        'takes team ID, returns an sql string with student numbers
        GetSpeakersByTeam = ""
        Dim foundrow As DataRow()
        foundrow = DS.Tables("ENTRY_STUDENT").Select("ENTRY=" & team)
        Dim x As Integer
        For x = 0 To foundrow.Length - 1
            If GetSpeakersByTeam <> "" Then GetSpeakersByTeam &= " or "
            GetSpeakersByTeam &= strIDString & "=" & foundrow(x).Item("ID")
        Next x
    End Function
    Function MakePairingTable(ByVal DS As DataSet, ByVal Round As Integer, ByVal strTeamName As String) As DataTable
        'converts all pairing and results info for an event into a complete and messy datatable; Do all sorting/filtering from the routine that called this one
        'Links rounds to panels to judges/ballots and retrieves names

        Dim w, x, y, z As Integer
        Dim scores(DS.Tables("Scores").Rows.Count) As Integer

        'strip rounds that aren't this one; this will cascade and delete the panels, ballots, and ballot_scores that
        'aren't this round
        'For x = DS.Tables("Round").Rows.Count - 1 To 0 Step -1
        ' If DS.Tables("Round").Rows(x).Item("ID") <> Round Then DS.Tables("Round").Rows(x).Delete()
        'Next x
        Dim drEvent As DataRow()
        drEvent = DS.Tables("Round").Select("ID=" & Round)

        'pull the round info; eventID, TB_SET in use, and JudgesPerPanel
        Dim eventID As Integer = drEvent(0).Item("Event")
        Dim TB_SET As Integer = drEvent(0).Item("TB_SET")
        Dim nJudges As Integer = drEvent(0).Item("JudgesPerPanel")

        Dim Dt As New DataTable

        'find nDebaters, nTeams per panel; now know number of judges, teams, and debaters per panel
        'Dim nDebaters As Integer = GetTagValue(DS.Tables("Event_Setting"), "DebatersPerTeam")
        Dim drEventSetting As DataRow()
        drEventSetting = DS.Tables("Event_Setting").Select("Event=" & eventID & " and Tag='DebatersPerTeam'")
        Dim nDebaters As Integer = drEventSetting(0).Item("Value")
        drEventSetting = DS.Tables("Event_Setting").Select("Event=" & eventID & " and Tag='TeamsPerRound'")
        Dim nTeams As Integer = drEventSetting(0).Item("Value")
        Dim arrTeams(nTeams) As Integer

        'BUILD the datatable
        Dt.Columns.Add("PANEL", System.Type.GetType("System.Int64"))
        'add judge columns for each judge based on number of judges
        For x = 0 To nJudges - 1
            Dt.Columns.Add("JUDGE" & x + 1.ToString, System.Type.GetType("System.Int64"))
            Dt.Columns.Add("JUDGENAME" & x + 1.ToString, System.Type.GetType("System.String"))
        Next x
        'add team columns for each team based on number of teams
        For y = 0 To nTeams - 1
            Dt.Columns.Add("Team" & y + 1.ToString, System.Type.GetType("System.Int64"))
            Dt.Columns.Add("Teamname" & y + 1.ToString, System.Type.GetType("System.String"))
            Dt.Columns.Add("Teamside" & y + 1.ToString, System.Type.GetType("System.String"))
            'add speakers per team based on number of debaters
            For w = 0 To nDebaters - 1
                Dt.Columns.Add("Team" & y + 1.ToString & "Spkr" & w + 1.ToString, System.Type.GetType("System.Int64"))
                Dt.Columns.Add("Team" & y + 1.ToString & "Spkr" & w + 1.ToString & "Name", System.Type.GetType("System.String"))
            Next w
        Next y
        Dt.Columns.Add("ROOM", System.Type.GetType("System.Int64"))
        Dt.Columns.Add("FLIGHT", System.Type.GetType("System.Int64"))
        Dt.Columns.Add("ROOMNAME", System.Type.GetType("System.String"))

        Call UniqueScores(DS, TB_SET, scores)
        'add team and judges scores for each team and speaker by each judge
        For x = 0 To nJudges - 1
            For y = 0 To nTeams - 1
                'add team scores by n judges and nteams
                For z = 0 To DS.Tables("Scores").Rows.Count - 1
                    If DS.Tables("Scores").Rows(z).Item("ScoreFor") = "Team" And ScoreActive(scores, DS.Tables("Scores").Rows(z).Item("ID")) = True Then
                        Dt.Columns.Add("Judge" & x + 1.ToString & "Team" & y + 1.ToString & DS.Tables("Scores").Rows(z).Item("SCORE_NAME"), System.Type.GetType("System.Single"))
                    End If
                Next z
                'add speaker scores by njudges, nteams, ndebaters
                For w = 0 To nDebaters - 1
                    For z = 0 To DS.Tables("Scores").Rows.Count - 1
                        If DS.Tables("Scores").Rows(z).Item("ScoreFor") = "Speaker" And ScoreActive(scores, DS.Tables("Scores").Rows(z).Item("ID")) = True Then
                            Dt.Columns.Add("Judge" & x + 1.ToString & "Team" & y + 1.ToString & "Spkr" & w + 1.ToString & DS.Tables("Scores").Rows(z).Item("SCORE_NAME"), System.Type.GetType("System.Single"))
                        End If
                    Next z
                Next w
            Next y
        Next x

        'POPULATE
        Dim fdPanel, fdBallot, fdScores, fdSpkrs As DataRow()
        Dim drScoreInfo, drRoom As DataRow
        Dim dr, drteam As DataRow
        Dim chk3Person As Integer
        fdPanel = DS.Tables("Panel").Select("Round=" & Round)
        Dim Judges, Spkrs, TeamNum As Integer
        For x = 0 To fdPanel.Length - 1
            dr = Dt.NewRow
            'add panel ID
            dr.Item("Panel") = fdPanel(x).Item("ID")
            dr.Item("Flight") = fdPanel(x).Item("Flight")
            'room name
            If fdPanel(x).Item("Room") Is System.DBNull.Value Then fdPanel(x).Item("Room") = -99
            If fdPanel(x).Item("Room") = "" Then fdPanel(x).Item("Room") = -99
            drRoom = DS.Tables("Room").Rows.Find(fdPanel(x).Item("Room"))
            If Not drRoom Is Nothing Then
                dr.Item("RoomName") = drRoom.Item("RoomName")
                dr.Item("Room") = fdPanel(x).Item("Room")
            End If
            'add all teams in side spot
            Call UniqueItemsOnPanel(DS, fdPanel(x).Item("ID"), "Entry", nTeams, arrTeams)
            For y = 1 To nTeams
                For z = 1 To nTeams
                    fdBallot = DS.Tables("Ballot").Select("Entry=" & arrTeams(z) & " and panel=" & fdPanel(x).Item("ID"))
                    If fdBallot.Length > 0 Then
                        If fdBallot(0).Item("Side") = y Or fdBallot(0).Item("Side") = -1 Then
                            dr.Item("Team" & y) = arrTeams(z)
                            drteam = DS.Tables("Entry").Rows.Find(arrTeams(z))
                            If drteam Is Nothing Then
                                dr.Item("TeamName" & y) = "N/A"
                            Else
                                dr.Item("TeamName" & y) = drteam.Item("Code")
                                If strTeamName.ToUpper = "FULL" Then dr.Item("TeamName" & y) = drteam.Item("FullName")
                            End If
                            'dr.Item("TeamName" & y) = FullNameMaker(DS, arrTeams(z), strTeamName, 1)
                            dr.Item("TeamSide" & y) = GetSideString(DS, fdBallot(0).Item("Side"), eventID)
                            Exit For
                        End If
                    End If
                Next z
            Next y
            'pull all ballots on the panel
            fdBallot = DS.Tables("Ballot").Select("Panel=" & fdPanel(x).Item("ID"), "Judge DESC, Entry DESC")
            Judges = 0
            For y = 0 To fdBallot.Length - 1
                'add judge; because a judge will have 1 ballot per team, only add 1 column per judge
                If y = 0 Then
                    Judges += 1 : dr.Item("Judge" & Judges) = fdBallot(y).Item("Judge")
                    If dr.Item("Judge" & Judges) = -1 Then
                        dr.Item("JudgeName" & Judges) = "Bye"
                    Else
                        dr.Item("JudgeName" & Judges) = GetName(DS.Tables("Judge"), fdBallot(y).Item("Judge"), "LAST") & ", " & GetName(DS.Tables("Judge"), fdBallot(y).Item("Judge"), "FIRST")
                    End If
                ElseIf fdBallot(y).Item("Judge") <> fdBallot(y - 1).Item("Judge") Then
                    Judges += 1 : dr.Item("Judge" & Judges) = fdBallot(y).Item("Judge")
                    dr.Item("JudgeName" & Judges) = GetName(DS.Tables("Judge"), fdBallot(y).Item("Judge"), "LAST") & ", " & GetName(DS.Tables("Judge"), fdBallot(y).Item("Judge"), "FIRST")
                End If
                TeamNum = 0
                For z = 0 To arrTeams.GetUpperBound(0)
                    If arrTeams(z) = fdBallot(y).Item("Entry") Then TeamNum = z
                Next z
                'pull the team scores for that ballot and add to table
                fdScores = DS.Tables("Ballot_Score").Select("Ballot=" & fdBallot(y).Item("ID") & "and recipient=" & fdBallot(y).Item("Entry"))
                For z = 0 To fdScores.Length - 1
                    drScoreInfo = DS.Tables("Scores").Rows.Find(fdScores(z).Item("Score_ID"))
                    If drScoreInfo.Item("ScoreFor") = "Team" Then
                        dr.Item("Judge" & Judges & "Team" & TeamNum & drScoreInfo.Item("Score_Name")) = fdScores(z).Item("Score")
                    End If
                Next z
                'pull all speakers for team
                fdSpkrs = DS.Tables("Entry_Student").Select("Entry=" & fdBallot(y).Item("Entry"))
                'For z = 0 To fdSpkrs.Length - 1
                'this might not support 3-person teams
                chk3Person = fdSpkrs.Length
                If chk3Person > nDebaters Then chk3Person = nDebaters
                For z = 0 To chk3Person - 1
                    fdScores = DS.Tables("Ballot_Score").Select("Ballot=" & fdBallot(y).Item("ID") & "and recipient=" & fdSpkrs(z).Item("ID"))
                    'only add the speaker if they receive a score
                    If fdScores.Length > 0 Then
                        Spkrs += 1
                        dr.Item("Team" & TeamNum & "Spkr" & Spkrs) = fdSpkrs(z).Item("ID")
                        dr.Item("Team" & TeamNum & "Spkr" & Spkrs & "Name") = GetName(DS.Tables("Entry_Student"), fdSpkrs(z).Item("ID"), "LAST") & ", " & GetName(DS.Tables("Entry_Student"), fdSpkrs(z).Item("ID"), "FIRST")
                    End If
                    For w = 0 To fdScores.Length - 1
                        drScoreInfo = DS.Tables("Scores").Rows.Find(fdScores(w).Item("Score_ID"))
                        If drScoreInfo.Item("ScoreFor") = "Speaker" Then
                            dr.Item("Judge" & Judges & "Team" & TeamNum & "Spkr" & z + 1 & drScoreInfo.Item("Score_Name")) = fdScores(w).Item("Score")
                        End If
                    Next w
                Next z
                Spkrs = 0
            Next y
            Dt.Rows.Add(dr)
        Next x
        Dt.AcceptChanges()
        MakePairingTable = Dt

    End Function
    Function GetTeamN(ByVal teamN As Integer, ByVal dr As DataRow, ByVal nteams As Integer)
        GetTeamN = 0
        Dim x As Integer
        For x = 0 To nteams - 1
            If dr.Item("Team" & x + 1) = teamN Then GetTeamN = x + 1
        Next x
        If GetTeamN = 0 Then GetTeamN = 1
    End Function
    Function GetSpkrN(ByVal spkrN As Integer, ByVal dr As DataRow, ByVal teamN As Integer, ByVal nteams As Integer, ByVal nspeakers As Integer)
        GetSpkrN = 0
        Dim x As Integer
        For x = 0 To nspeakers - 1
            If dr.Item("Team" & teamN & "Spkr" & x + 1) = spkrN Then spkrN = x + 1
        Next x
        If GetSpkrN = 0 Then GetSpkrN = 1
    End Function
    Function GetTBList(ByVal ds As DataSet, ByVal strScoreFor As String, ByVal TBSET As Integer, ByRef TBArry() As Integer) As Integer
        'returns and array that contains the ID values from the scores table for every score that needs to be collected
        'filter the table so it only includes this TB_SET
        Dim fdTieBreak As DataRow()
        fdTieBreak = ds.Tables("Tiebreak").Select("TB_SET=" & TBSET)
        Dim x, y, ctr As Integer : Dim ThereAlready As Boolean
        Dim foundScore As DataRow()
        ctr = 0
        'scroll through the tiebreakers
        For x = 0 To fdTieBreak.Length - 1
            'pull the score value
            foundScore = ds.Tables("SCORES").Select("ID=" & fdTieBreak(x).Item("Score"))
            'see if the score is for this array
            ThereAlready = False
            If foundScore(0).Item("ScoreFor") = strScoreFor Then
                'see if the score is in the array; if not, add it
                If ctr = 0 Then
                    ThereAlready = False
                Else
                    For y = 0 To ctr - 1
                        If TBArry(y) = foundScore(0).Item("ID") Then ThereAlready = True
                    Next y
                End If
                If ThereAlready = False Then
                    ReDim Preserve TBArry(ctr)
                    TBArry(ctr) = foundScore(0).Item("ID") : ctr += 1
                End If
            End If
        Next x
    End Function
    Function GetRowInfo(ByVal ds As DataSet, ByVal strTable As String, ByVal ID As Integer, ByVal strField As String)
        'receives the name of a table and ID for row, and returns the field value
        'works for any table with a unique identifier field named ID
        Dim foundrow As DataRow()
        foundrow = ds.Tables(strTable).Select("ID=" & ID)
        GetRowInfo = foundrow(0).Item(strField)
    End Function
    Function GetElimName(ByVal RD_NAME As Integer) As String
        GetElimName = "N/A"
        If RD_NAME = 16 Then GetElimName = "Finals"
        If RD_NAME = 15 Then GetElimName = "Semis"
        If RD_NAME = 14 Then GetElimName = "Quarters"
        If RD_NAME = 13 Then GetElimName = "Octos"
        If RD_NAME = 12 Then GetElimName = "Doubles"
        If RD_NAME = 11 Then GetElimName = "Triples"
        If RD_NAME = 10 Then GetElimName = "Quads"
    End Function
    Public Function GetSideString(ByVal ds As DataSet, ByVal side As Integer, ByVal eventID As Integer) As String
        GetSideString = "None assigned"
        If getEventSetting(ds, eventID, "SideDesignations") = "Aff/Neg" Then
            If side = 1 Then GetSideString = "Aff"
            If side = 2 Then GetSideString = "Neg"
        End If
        If getEventSetting(ds, eventID, "SideDesignations") = "Gov/Opp" Then
            If side = 1 Then GetSideString = "Government"
            If side = 2 Then GetSideString = "Opposition"
        End If
        If getEventSetting(ds, eventID, "SideDesignations") = "WUDC" Then
            If side = 1 Then GetSideString = "Opening Gvt"
            If side = 2 Then GetSideString = "Opening Opp"
            If side = 3 Then GetSideString = "Closing Gvt"
            If side = 4 Then GetSideString = "Closing Opp"
        End If
    End Function
    Public Function GetSideNumber(ByVal Side As String) As Integer
        If Side = "Aff" Then GetSideNumber = 1
        If Side = "Neg" Then GetSideNumber = 2
        If Side = "Government" Then GetSideNumber = 1
        If Side = "Opposition" Then GetSideNumber = 2
        If Side = "Opening Gvt" Then GetSideNumber = 1
        If Side = "Opening Opp" Then GetSideNumber = 2
        If Side = "Closing Gvt" Then GetSideNumber = 3
        If Side = "Closing Opp" Then GetSideNumber = 4
    End Function
    Function ValidatePanel(ByVal ds As DataSet, ByVal panel As Integer) As String
        'check the number of judge and team spots match the max for the round
        'check that no judge or team appears more than the max times identified
        'if it's an actual judge and not -99 to indicate a placeholder for a future judge assignment,
        'they should have exactly the number of ballots for the number of competing teams; error for more or less

        ValidatePanel = "OK"

        'pull the panel and all ballots
        Dim drPanel, drRound, drEvent As DataRow
        drPanel = ds.Tables("Panel").Rows.Find(panel)
        drRound = ds.Tables("Round").Rows.Find(drPanel.Item("Round"))
        Dim nJudges = drRound.Item("JudgesPerPanel")
        drEvent = ds.Tables("Event").Rows.Find(drRound.Item("Event"))
        Dim nTeams = Val(getEventSetting(ds, drRound.Item("Event"), "TeamsPerRound"))

        Dim fdBallot As DataRow()
        fdBallot = ds.Tables("Ballot").Select("Panel=" & panel, "Judge ASC")

        'check right total number of ballots
        If fdBallot.Length <> nTeams * nJudges Then
            ValidatePanel = "The wrong number of ballots exists for this round"
        End If

        'check ballots were returned
        If fdBallot.Length = 0 Then
            ValidatePanel = "No information appears to be stored for this round." : Exit Function
        End If

        'check judges don't appear too often
        Dim ctr As Integer
        If fdBallot(0).Item("Judge") <> -99 Then ctr += 1
        For x = 1 To fdBallot.Length - 2
            If fdBallot(x).Item("Judge") = -99 Then Exit For
            If fdBallot(x).Item("Judge") <> fdBallot(x - 1).Item("Judge") Then ctr += 1
        Next
        If ctr > nJudges Then ValidatePanel = "There are more judges on this panel than are allowed by the round settings."

        'check teams don't appear too often
        fdBallot = ds.Tables("Ballot").Select("Panel=" & panel, "Entry ASC")
        ctr = 0
        If fdBallot(0).Item("Entry") <> -99 Then ctr += 1
        For x = 1 To fdBallot.Length - 1
            If fdBallot(x).Item("Entry") = -99 Then Exit For
            If fdBallot(x).Item("Entry") <> fdBallot(x - 1).Item("Entry") Then ctr += 1
        Next
        If ctr > nTeams Then ValidatePanel = "There are more teams on this panel than are allowed by the round settings."

    End Function
    Sub UniqueItemsOnPanel(ByVal ds As DataSet, ByVal panel As Integer, ByVal strField As String, ByVal nItems As Integer, ByRef arrItems() As Integer)
        'recieves a panel ID number, a field (either "Judge" or "Entry"), and a required number of records to return
        'returns an array with only the unique Judge and team records
        Dim foundrow As DataRow()
        foundrow = ds.Tables("Ballot").Select("Panel=" & panel, strField & " ASC")
        If strField = "Entry" Then foundrow = ds.Tables("Ballot").Select("Panel=" & panel, "Side ASC, Entry ASC")
        Dim ctr As Integer = 0
        Dim OKtoAdd As Boolean
        For y = 0 To arrItems.GetUpperBound(0)
            arrItems(y) = -99
        Next y
        For x = 0 To foundrow.Length - 1
            OKtoAdd = True
            For y = 0 To arrItems.GetUpperBound(0)
                If arrItems(y) = foundrow(x).Item(strField) Then OKtoAdd = False
            Next y
            If OKtoAdd = True Then
                ctr += 1
                arrItems(ctr) = foundrow(x).Item(strField)
            End If
        Next x
        If ctr > nItems Then MsgBox("Totally messed up")
    End Sub
    Sub UniqueScores(ByVal ds As DataSet, ByVal TBSet As Integer, ByRef arrItems() As Integer)
        'recieves a a tiebreak_set, and returns the number of unique scores to be used for collection and tiebreaking
        'un-used scores return -99
        Dim foundrow As DataRow()
        foundrow = ds.Tables("Tiebreak").Select("TB_SET=" & TBSet)
        Dim ctr As Integer = 0
        Dim OKtoAdd As Boolean
        For y = 0 To arrItems.GetUpperBound(0)
            arrItems(y) = -99
        Next y
        For x = 0 To foundrow.Length - 1
            OKtoAdd = True
            For y = 0 To arrItems.GetUpperBound(0)
                If arrItems(y) = foundrow(x).Item("ScoreID") Then OKtoAdd = False
            Next y
            If OKtoAdd = True Then
                ctr += 1
                arrItems(ctr) = foundrow(x).Item("ScoreID")
            End If
        Next x
    End Sub
    Function ScoreActive(ByVal arrScores() As Integer, ByVal score As Integer) As Boolean
        ScoreActive = False
        For x = 0 To arrScores.GetUpperBound(0)
            If arrScores(x) = score Then ScoreActive = True
        Next x
    End Function
    Function GetPriorRound(ByVal ds As DataSet, ByVal round As Integer) As Integer
        GetPriorRound = 0
        'takes a round, and gets the round that immediately preceded it for that event/division
        'returns a zero if it's the first round
        Dim dr As DataRow : dr = ds.Tables("Round").Rows.Find(round)
        Dim fdRds As DataRow()
        fdRds = ds.Tables("Round").Select("Event=" & dr.Item("event"), "Rd_Name ASC")
        For x = 0 To fdRds.Length - 1
            If fdRds(x).Item("rd_Name") < dr.Item("rd_Name") Then GetPriorRound = fdRds(x).Item("ID")
            'If fdRds(x).Item("ID") = round Then GetPriorRound = fdRds(x - 1).Item("ID") : Exit Function
        Next x
    End Function
    Function GetLastCompleteRound(ByVal ds As DataSet, ByVal round As Integer) As Integer
        'returns the last round for which there are complete results
        'counts ballots in if 1/2 scores are entered
        'counts round as in if 80% of ballots make the 1/2 criteria
        Dim drRd As DataRow : drRd = ds.Tables("Round").Rows.Find(round)
        Dim fdRd As DataRow() : fdRd = ds.Tables("Round").Select("Event=" & drRd.Item("Event") & " and Rd_Name <=" & drRd.Item("Rd_Name"), "Timeslot ASC")
        Dim fdBalScore, fdBallot As DataRow()
        Dim BallotsIn, ctr As Integer
        For x = 0 To fdRd.Length - 1
            If BuildPanelStringByRound(ds, fdRd(x).Item("ID")) <> "" Then
                fdBallot = ds.Tables("Ballot").Select(BuildPanelStringByRound(ds, fdRd(x).Item("ID")))
                BallotsIn = 0
                For y = 0 To fdBallot.Length - 1
                    fdBalScore = ds.Tables("Ballot_Score").Select("Ballot=" & fdBallot(y).Item("ID"))
                    ctr = 0
                    For z = 0 To fdBalScore.Length - 1
                        If fdBalScore(z).Item("Score") <> 0 Then ctr += 1
                    Next z
                    If ctr > (fdBalScore.Length / 2) Then BallotsIn += 1
                Next y
                If BallotsIn >= (fdBallot.Length * 0.8) And fdBallot.Length > 0 Then GetLastCompleteRound = fdRd(x).Item("ID")
            End If
        Next x
    End Function
    Function GetRegion(ByVal ds As DataSet, ByVal team As Integer)
        Dim drSchool, drEntry As DataRow
        drEntry = ds.Tables("Entry").Rows.Find(team)
        drSchool = ds.Tables("School").Rows.Find(drEntry.Item("School"))
        GetRegion = drSchool.Item("Region")
    End Function
    Sub DeleteAllPanelsForRound(ByRef ds As DataSet, ByVal round As Integer)
        'delete all existing panels for the round
        For x = ds.Tables("Panel").Rows.Count - 1 To 0 Step -1
            If ds.Tables("Panel").Rows(x).Item("Round") = round Then
                ds.Tables("Panel").Rows(x).Delete()
            End If
        Next x
        ds.AcceptChanges()
    End Sub
    Function JudgeSituation(ByVal ds As DataSet) As DataTable
        Dim strTime As String
        strTime = "Start: " & Now.Second & " " & Now.Millisecond & Chr(10) & Chr(10)
        'creates a table that shows how many rounds of judging are available and needed by division
        Dim dt As New DataTable
        dt.Columns.Add("Event", System.Type.GetType("System.String"))
        dt.Columns.Add("Need", System.Type.GetType("System.Int64"))
        dt.Columns.Add("Assigned", System.Type.GetType("System.Int64"))
        dt.Columns.Add("NeedBalance", System.Type.GetType("System.Int64"))
        dt.Columns.Add("Eligible", System.Type.GetType("System.Int64"))
        dt.Columns.Add("Available", System.Type.GetType("System.Int64"))
        dt.Columns.Add("TotalBalance", System.Type.GetType("System.Int64"))
        If ds.Tables("Round").Columns.Contains("RoundOver") = False Then
            ds.Tables("Round").Columns.Add("RoundOver", System.Type.GetType("System.Boolean"))
        End If
        If ds.Tables("Judge").Columns.Contains("MaxCanHear") = False Then
            ds.Tables("Judge").Columns.Add("MaxCanHear", System.Type.GetType("System.Int16"))
        End If
        For x = 0 To ds.Tables("Judge").Rows.Count - 1
            ds.Tables("Judge").Rows(x).Item("MaxCanHear") = 0
        Next x
        Dim dr, drJudge As DataRow
        Dim fdEntries, fdJudges, fdRound, fdBallots As DataRow()
        Dim rdsAvail, maxCanHear As Integer
        'strTime &= "Start event scroll: " & Now.Second & " " & Now.Millisecond & Chr(10) & Chr(10)
        For x = 0 To ds.Tables("Event").Rows.Count - 1
            'If x = 0 Then strTime &= "Make datarow: " & Now.Second & " " & Now.Millisecond & Chr(10) & Chr(10)
            dr = dt.NewRow : dr.Item("Eligible") = 0 : dr.Item("Need") = 0 : dr.Item("Available") = 0
            dr.Item("Event") = ds.Tables("Event").Rows(x).Item("EventName")
            'If x = 0 Then strTime &= "Pull entries: " & Now.Second & " " & Now.Millisecond & Chr(10) & Chr(10)
            fdEntries = ds.Tables("Entry").Select("Event=" & ds.Tables("Event").Rows(x).Item("ID"))
            'If x = 0 Then strTime &= "Pull round: " & Now.Second & " " & Now.Millisecond & Chr(10) & Chr(10)
            fdRound = ds.Tables("Round").Select("Event=" & ds.Tables("Event").Rows(x).Item("ID") & " and Rd_Name<=9")
            'If x = 0 Then strTime &= "Needed Start: " & Now.Second & " " & Now.Millisecond & Chr(10) & Chr(10)
            'Test whether teams per round is set and initialize if not
            If getEventSetting(ds, ds.Tables("Event").Rows(x).Item("ID"), "TeamsPerRound") = 0 Then Call EventSettingsCheck(ds)
            For y = 0 To fdRound.Length - 1
                'calculate the need
                dr.Item("Need") += Int(fdEntries.Length / getEventSetting(ds, ds.Tables("Event").Rows(x).Item("ID"), "TeamsPerRound")) * fdRound(y).Item("JudgesPerPanel")
                'mark if unpaneled
                fdRound(y).Item("RoundOver") = False
                fdBallots = ds.Tables("Ballot").Select(BuildPanelStringByRound(ds, fdRound(y).Item("ID")) & " and Judge>-1")
                If fdBallots.Length > 0 Then fdRound(y).Item("RoundOver") = True
            Next y
            'START CLOG
            'If x = 0 Then strTime &= "Find assignments Start: " & Now.Second & " " & Now.Millisecond & Chr(10) & Chr(10)
            dr.Item("Assigned") = GetBallotsAlreadyAssigned(ds, ds.Tables("Event").Rows(x).Item("ID"))
            dr.Item("NeedBalance") = dr.Item("Need") - dr.Item("Assigned")
            'END CLOG
            'If x = 0 Then strTime &= "Pull Judges Start: " & Now.Second & " " & Now.Millisecond & Chr(10) & Chr(10)
            Try
                fdJudges = ds.Tables("Judge").Select("Event" & ds.Tables("Event").Rows(x).Item("ID") & "=true")
            Catch
                fdJudges = ds.Tables("Judge").Select("Event" & ds.Tables("Event").Rows(x).Item("ID") & "='true'")
            End Try
            'If x = 0 Then strTime &= "Elig/Avail Start: " & Now.Second & " " & Now.Millisecond & Chr(10) & Chr(10)
            For y = 0 To fdJudges.Length - 1
                'Count total rounds they can judge
                dr.Item("Eligible") += fdJudges(y).Item("Obligation") + fdJudges(y).Item("Hired")
                'Subtract what they've already judged
                rdsAvail = fdJudges(y).Item("Obligation") + fdJudges(y).Item("Hired") - fdJudges(y).Item("Already")
                If rdsAvail < 0 Then rdsAvail = 0
                'Check to see how many remaining rounds they're here for
                drJudge = ds.Tables("Judge").Rows.Find(fdJudges(y).Item("ID"))
                maxCanHear = MaxLeft(drJudge, fdRound)
                If rdsAvail > maxCanHear Then rdsAvail = maxCanHear
                'set the max left that any judge can here for tournament-wide dumming
                If maxCanHear > fdJudges(y).Item("MaxCanHear") Then fdJudges(y).Item("MaxCanHear") = maxCanHear
                'add it up
                dr.Item("Available") += rdsAvail
            Next y
            dr.Item("TotalBalance") = dr.Item("Available") - dr.Item("NeedBalance")
            dt.Rows.Add(dr)
            If x = 0 Then strTime &= "Finished first division: " & Now.Second & " " & Now.Millisecond & Chr(10) & Chr(10)
        Next x
        'do totals
        'strTime &= "Start totals: " & Now.Second & " " & Now.Millisecond & Chr(10) & Chr(10)
        dr = dt.NewRow : dr.Item("Eligible") = 0 : dr.Item("Need") = 0 : dr.Item("Available") = 0 : dr.Item("Assigned") = 0
        dr.Item("Event") = "Total"
        For x = 0 To dt.Rows.Count - 1
            dr.Item("Need") += dt.Rows(x).Item("Need")
            dr.Item("Assigned") += dt.Rows(x).Item("Assigned")
        Next x
        dr.Item("NeedBalance") = dr.Item("Need") - dr.Item("Assigned")
        For y = 0 To ds.Tables("Judge").Rows.Count - 1
            rdsAvail = ds.Tables("Judge").Rows(y).Item("Obligation") + ds.Tables("Judge").Rows(y).Item("Hired") - ds.Tables("Judge").Rows(y).Item("Already")
            If rdsAvail < 0 Then rdsAvail = 0
            If rdsAvail > ds.Tables("Judge").Rows(y).Item("MaxCanHear") Then rdsAvail = ds.Tables("Judge").Rows(y).Item("MaxCanHear")
            dr.Item("Available") += rdsAvail
        Next
        dr.Item("TotalBalance") = dr.Item("Available") - dr.Item("NeedBalance")
        dt.Rows.Add(dr)
        strTime &= "Done: " & Now.Second & " " & Now.Millisecond & Chr(10) & Chr(10)
        'MsgBox(strTime)
        ds.Tables("Round").Columns.Remove("RoundOver")
        ds.Tables("Judge").Columns.Remove("MaxCanHear")
        Return dt
    End Function
    Function MaxLeft(ByVal drJudge As DataRow, ByVal fdRound As DataRow()) As Integer
        'returns the maximum number of remaining rounds a judge can hear by division
        For x = 0 To fdRound.Length - 1
            If drJudge.Item("Timeslot" & fdRound(x).Item("Timeslot")) Is System.DBNull.Value Then drJudge.Item("Timeslot" & fdRound(x).Item("Timeslot")) = False
            drJudge.Item("Timeslot" & fdRound(x).Item("Timeslot")) = drJudge.Item("Timeslot" & fdRound(x).Item("Timeslot")).trim
            If fdRound(x).Item("RoundOver") = False And drJudge.Item("Timeslot" & fdRound(x).Item("Timeslot")) = True Then
                MaxLeft += 1
            End If
        Next x
    End Function
    Function GetRoundsJudged(ByVal ds As DataSet, ByVal JudgeID As Integer) As Integer
        GetRoundsJudged = 0
        Dim fdballots As DataRow()
        fdballots = ds.Tables("Ballot").Select("Judge=" & JudgeID, "Panel ASC")
        Dim drPan, drRd As DataRow
        For x = 0 To fdballots.Length - 1
            drPan = ds.Tables("Panel").Rows.Find(fdballots(x).Item("Panel"))
            drRd = ds.Tables("Round").Rows.Find(drPan.Item("Round"))
            If drRd.Item("Rd_Name") <= 9 Then
                If x = 0 Then
                    GetRoundsJudged += 1
                ElseIf fdballots(x).Item("panel") <> fdballots(x - 1).Item("panel") Then
                    GetRoundsJudged += 1
                End If
            End If
        Next
    End Function
    Function GetBallotsAlreadyAssigned(ByVal ds As DataSet, ByVal EventID As Integer) As Integer
        GetBallotsAlreadyAssigned = 0
        Dim fdRound, fdPanel, fdBallots As DataRow()
        fdRound = ds.Tables("Round").Select("Event=" & EventID & " and Rd_Name<=9")
        For x = 0 To fdRound.Length - 1
            fdPanel = ds.Tables("Panel").Select("Round=" & fdRound(x).Item("ID"))
            For y = 0 To fdPanel.Length - 1
                fdBallots = ds.Tables("Ballot").Select("Panel=" & fdPanel(y).Item("ID") & " and Judge<>-1 and Judge<>-99")
                GetBallotsAlreadyAssigned += (fdBallots.Length / getEventSetting(ds, EventID, "TeamsPerRound"))
            Next y
        Next x
    End Function
    Sub CheckMasterFile(ByRef ds As DataSet)
        Dim dr As DataRow()
        dr = ds.Tables("Tourn_Setting").Select("Tag='Online'")
        If dr.Length = 0 Then Exit Sub
        If dr(0).Item("Value").toupper = "ALL OFFLINE" Then Exit Sub
        If dr(0).Item("Value").toupper <> "ALL OFFLINE" Then dr(0).Item("Value") = "All Offline" : Exit Sub
        Dim j As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\CAT\TourneyDataMaster.xml"
        Dim fFile As New FileInfo(j)
        If fFile.Exists Then Exit Sub
        Dim strInfo As String = ""
        strInfo &= "You have indicated that you wish to use the online functions, but no master file of judges and competitors appears to exist." & Chr(10) & Chr(10)
        strInfo &= "The tournament will now switch to offline mode, and you can continue pairing." & Chr(10) & Chr(10)
        strInfo &= "If you wish to use the online functions, please locate and save the master names file and then return to the tournament settings page to switch back to an online mode." & Chr(10) & Chr(10)
        MsgBox(strInfo, MsgBoxStyle.OkOnly)
        dr(0).Item("Value") = "All Offline"
    End Sub
    Function GetJudgeRating(ByVal ds As DataSet, ByVal Judge As Integer, ByVal Team As Integer, ByVal strRatingType As String) As Single

        'pull the judge and exit if there's a conflict
        Dim fdRow As DataRow()
        fdRow = ds.Tables("JudgePref").Select("Team=" & Team & " and judge=" & Judge)
        If Not fdRow Is Nothing Then
            If fdRow.Length > 0 Then
                If fdRow(0).Item("Rating") = 999 Then GetJudgeRating = 999 : Exit Function
            End If
        End If

        'if using tab assigend ratings, return that and exit
        If strRatingType.ToUpper = "ASSIGNEDRATING" Then
            Dim dr As DataRow
            dr = ds.Tables("Judge").Rows.Find(Judge)
            If dr Is Nothing Then GetJudgeRating = 0 : Exit Function
            GetJudgeRating = dr.Item("TabRating")
            Exit Function
        End If

        'not using tab ratings, so return ordinal percentile
        If fdRow.Length = 0 Then GetJudgeRating = 0 : Exit Function
        If fdRow.Length > 1 Then GetJudgeRating = -99 : Exit Function
        'If strRatingType.ToUpper = "TEAMRATING" Then
        If Not fdRow(0).Item("OrdPct") Is System.DBNull.Value Then
            GetJudgeRating = fdRow(0).Item("OrdPct")
            If GetJudgeRating = 0 And fdRow(0).Item("Rating") > 0 Then GetJudgeRating = 1
        Else
            GetJudgeRating = 0
        End If
        'End If

    End Function
    Sub MakeBallotFile(ByVal Ds As DataSet, ByVal Round As Integer, ByVal strStartTime As String, ByVal strSortBy As String, ByVal ElimInstr As Boolean)
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
        Dim drEvent As DataRow : drEvent = Ds.Tables("Event").Rows.Find(drRound.Item("Event"))
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
        If ElimInstr = True Then strScoreRange &= "Teams should switch sides if they have hit before and otherwise should flip a coin to determine sides.  Please indicate sides next to the word 'flip' below."
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
            fdBallots = Ds.Tables("Ballot").Select("Panel=" & fdPanel(x).Item("ID"), "Judge DESC")
            For y = 0 To fdBallots.Length - 1
                If y > 0 Then
                    If drEvent.Item("Type") = "WUDC" Then
                        If fdBallots(y).Item("Judge") <> fdBallots(y - 1).Item("Judge") Then
                            Exit For
                        End If
                    End If
                End If
                If fdBallots(y).Item("Judge") > -1 Then
                    dr.Item("Judge") = fdBallots(y).Item("Judge")
                    drJudge = Ds.Tables("Judge").Rows.Find(fdBallots(y).Item("Judge"))
                    If Not drJudge Is Nothing Then
                        dr.Item("JudgeName") = drJudge.Item("Last").trim & ", " & drJudge.Item("First").trim
                    Else
                        dr.Item("JudgeName") = "NA"
                    End If
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
            If dtJudges.DefaultView(x).Item("JudgeName") <> "NA" Then
                drRoom = Ds.Tables("Room").Rows.Find(dtJudges.DefaultView(x).Item("Room"))
                st.WriteLine("<div class=" & Chr(34) & "Heading" & Chr(34) & ">" & dtJudges.DefaultView(x).Item("JudgeName") & "</DIV>")
                st.WriteLine("<b>Room:</b>" & drRoom.Item("RoomName") & Chr(9) & Chr(9) & "<b>Start time:</b>" & strStartTime & "<br>")
                'st.WriteLine("Ballot code:" & Ds.Tables("Tourn").Rows(0).Item("ID") & "-" & Round & "-" & dtJudges.DefaultView(x).Item("Judge"))
                st.WriteLine("<div class=" & Chr(34) & "Heading" & Chr(34) & "><center>" & Ds.Tables("Tourn").Rows(0).Item("TournName").trim & "</center></div><br>")
                st.WriteLine("<div class=" & Chr(34) & "Heading" & Chr(34) & "><center>" & drRound.Item("Label") & "</center></DIV>")
                st.WriteLine("<br>" & strScoreRange & "<br><hr/><br>")
                If dtTeam.Columns.Count > 1 Or ElimInstr = True Then
                    st.WriteLine("<TABLE>")
                    st.WriteLine("<THEAD>")
                    st.WriteLine("<TR>")
                    st.WriteLine("<TH>Side</TH>")
                    st.WriteLine("<TH>Team</TH>")
                    For y = 1 To dtTeam.Columns.Count - 1
                        st.WriteLine("<TH>" & dtTeam.Columns(y).ColumnName & "</TH>")
                    Next y
                    st.WriteLine("</TR>")
                    st.WriteLine("</THEAD>")
                    For y = 1 To nTeams
                        drTeam = Ds.Tables("Entry").Rows.Find(dtJudges.DefaultView(x).Item("Ballot" & y))
                        st.WriteLine("<TR>")
                        If ElimInstr = True Then
                            st.WriteLine("<TD>Flip (enter side):____________</td>")
                        Else
                            st.WriteLine("<TD>" & GetSideString(Ds, dtJudges.DefaultView(x).Item("Side" & y), drRound.Item("Event")) & "</td>")
                        End If
                        st.WriteLine("<TD>" & drTeam.Item("FullName") & "</td>")
                        For z = 1 To dtTeam.Columns.Count - 1
                            st.WriteLine("<td>" & "    " & "</Td>")
                        Next z
                        st.WriteLine("</tr>")
                    Next y
                    st.WriteLine("</TABLE><br>")
                End If
                If dtSpeakers.Columns.Count > 1 Then
                    st.WriteLine("<TABLE>")
                    strTH = "<THEAD><TR>"
                    For z = 1 To nTeams
                        strTH &= "<TH>Speaker</TH>"
                        For y = 1 To dtSpeakers.Columns.Count - 1
                            strTH &= "<TH>" & dtSpeakers.Columns(y).ColumnName & "</TH>"
                        Next y
                    Next z
                    strTH &= "</TR></THEAD>"
                    st.WriteLine("<THEAD><TR>")
                    For y = 1 To nTeams
                        drTeam = Ds.Tables("Entry").Rows.Find(dtJudges.DefaultView(x).Item("Ballot" & y))
                        If drEvent.Item("Type") = "WUDC" Then
                            st.WriteLine("<center><TH colspan=" & Chr(34) & "2" & Chr(34) & ">" & GetSideString(Ds, dtJudges.DefaultView(x).Item("Side" & y), drRound.Item("Event")) & ": " & drTeam.Item("FullName") & "</TH></center>")
                        Else
                            st.WriteLine("<center><TH colspan=" & Chr(34) & "3" & Chr(34) & ">" & GetSideString(Ds, dtJudges.DefaultView(x).Item("Side" & y), drRound.Item("Event")) & ": " & drTeam.Item("FullName") & "</TH></center>")
                        End If
                    Next y
                    st.WriteLine("</tr></THEAD>")
                    st.WriteLine(strTH)
                    'fill the speaker array
                    For y = 1 To nTeams
                        fdSpeakers = Ds.Tables("Entry_Student").Select("Entry=" & dtJudges.DefaultView(x).Item("Ballot" & y), "ID asc")
                        For w = 0 To fdSpeakers.Length - 1
                            arrSpkrs(y, w) = fdSpeakers(w).Item("Last").trim & ", " & fdSpeakers(w).Item("First").trim
                        Next w
                    Next y
                    For y = 0 To nspkrs - 1
                        st.WriteLine("<TR>")
                        For z = 1 To nTeams
                            st.WriteLine("<td>" & arrSpkrs(z, y) & "</Td>")
                            For w = 1 To dtSpeakers.Columns.Count - 1
                                st.WriteLine("<td>" & "    " & "</Td>")
                            Next w
                        Next z
                        st.WriteLine("</tr>")
                    Next y
                    st.WriteLine("</TABLE><br>")
                End If
                If hasBallot = True Then
                    st.WriteLine("Please circle the team that you intend to vote for.  Check here to indicate a low point win is intended ____")
                End If
                st.WriteLine("<hr/>")
                st.WriteLine("Comments and reason for decision:")
                'add hard return
                st.WriteLine(strBreakline)
            End If
        Next x
        st.WriteLine("</HTML>")
        st.Close()
    End Sub
    Sub NullKiller(ByRef ds As DataSet, ByVal strTable As String)
        Dim dr As DataRow()
        For x = 0 To ds.Tables(strTable).Columns.Count - 1
            dr = ds.Tables(strTable).Select(ds.Tables(strTable).Columns(x).ColumnName & " is NULL")
            If dr.Length > 0 Then
                For y = 0 To dr.Length - 1
                    If ds.Tables(strTable).Columns(x).DataType.ToString.ToUpper = "SYSTEM.STRING" Then
                        dr(y).Item(x) = ""
                    ElseIf ds.Tables(strTable).Columns(x).DataType.ToString.ToUpper = "SYSTEM.INT32" Then
                        dr(y).Item(x) = 0
                    ElseIf ds.Tables(strTable).Columns(x).DataType.ToString.ToUpper = "SYSTEM.INT64" Then
                        dr(y).Item(x) = 0
                    ElseIf ds.Tables(strTable).Columns(x).DataType.ToString.ToUpper = "SYSTEM.BOOLEAN" Then
                        dr(y).Item(x) = False
                    ElseIf (strTable.ToUpper = "ROOM" Or strTable.ToUpper = "JUDGE") And (InStr(ds.Tables(strTable).Columns(x).ColumnName.ToUpper, "EVENT") > 0 Or InStr(ds.Tables(strTable).Columns(x).ColumnName.ToUpper, "TIMESLOT") > 0) Then
                        ds.Tables(strTable).Rows(y).Item(x) = Convert.ToBoolean(ds.Tables(strTable).Rows(y).Item(x))
                    End If
                Next y
            End If
            For y = 0 To ds.Tables(strTable).Rows.Count - 1
                If (strTable.ToUpper = "ROOM" Or strTable.ToUpper = "JUDGE") And (InStr(ds.Tables(strTable).Columns(x).ColumnName.ToUpper, "EVENT") > 0 Or InStr(ds.Tables(strTable).Columns(x).ColumnName.ToUpper, "TIMESLOT") > 0) Then
                    If ds.Tables(strTable).Rows(y).Item(x) = "" Then ds.Tables(strTable).Rows(y).Item(x) = "False"
                    ds.Tables(strTable).Rows(y).Item(x) = Convert.ToBoolean(ds.Tables(strTable).Rows(y).Item(x))
                End If
            Next y
        Next x
    End Sub
    Sub NullFixer()
        'if the load detects the dreaded "input string not in correct foramt" error, this will try to fix it
        'first, it will load without validation
        'second, fix known issues
        'STEP ONE: Load without validation
        Dim ds As New DataSet
        Dim xmlFile As XmlReader
        xmlFile = XmlReader.Create(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\CAT\TourneyData.xml", New XmlReaderSettings())
        ds.ReadXml(xmlFile, XmlReadMode.InferSchema)
        'STEP TWO: Load a blank dataset with the xsd formatting
        Dim ds2 As New DataSet
        Dim strXsdLocation As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\CAT\TourneyData.xsd"
        ds2.ReadXmlSchema(strXsdLocation)
        'STEP THREE: 1 and 0 to true and false
        Dim DoIt As Boolean
        For x = 0 To ds.Tables.Count - 1
            For y = 0 To ds.Tables(x).Columns.Count - 1
                For z = 0 To ds.Tables(x).Rows.Count - 1
                    'only process if it's in the schema table
                    If Not ds2.Tables(ds.Tables(x).TableName).Columns(ds.Tables(x).Columns(y).ColumnName) Is Nothing Then
                        'kill the nulls
                        DoIt = False
                        If ds.Tables(x).Rows(z).Item(y) Is System.DBNull.Value Then
                            DoIt = True
                        ElseIf ds.Tables(x).Rows(z).Item(y) = "" Then
                            DoIt = True
                        End If
                        If DoIt = True Then
                            If ds2.Tables(ds.Tables(x).TableName).Columns(ds.Tables(x).Columns(y).ColumnName).DataType.ToString.ToUpper = "SYSTEM.STRING" Then
                                ds.Tables(x).Rows(z).Item(y) = "-"
                            ElseIf ds2.Tables(ds.Tables(x).TableName).Columns(ds.Tables(x).Columns(y).ColumnName).DataType.ToString.ToUpper = "SYSTEM.INT32" Then
                                ds.Tables(x).Rows(z).Item(y) = "0"
                            ElseIf ds2.Tables(ds.Tables(x).TableName).Columns(ds.Tables(x).Columns(y).ColumnName).DataType.ToString.ToUpper = "SYSTEM.INT64" Then
                                ds.Tables(x).Rows(z).Item(y) = "0"
                            ElseIf ds2.Tables(ds.Tables(x).TableName).Columns(ds.Tables(x).Columns(y).ColumnName).DataType.ToString.ToUpper = "SYSTEM.DECIMAL" Then
                                ds.Tables(x).Rows(z).Item(y) = "0"
                            ElseIf ds2.Tables(ds.Tables(x).TableName).Columns(ds.Tables(x).Columns(y).ColumnName).DataType.ToString.ToUpper = "SYSTEM.BOOLEAN" Then
                                ds.Tables(x).Rows(z).Item(y) = "false"
                            End If
                        End If
                        'if boolean, convert 1 and 0 to true and false
                        If ds2.Tables(ds.Tables(x).TableName).Columns(ds.Tables(x).Columns(y).ColumnName).DataType.ToString.ToUpper = "SYSTEM.BOOLEAN" Then
                            If ds.Tables(x).Rows(z).Item(y) = "1" Then ds.Tables(x).Rows(z).Item(y) = "true"
                            If ds.Tables(x).Rows(z).Item(y) = "0" Then ds.Tables(x).Rows(z).Item(y) = "false"
                        End If
                    End If
                Next z
            Next y
        Next x
        xmlFile.Close()
        Call SaveFile(ds)
        ds.Dispose()
        MsgBox("Fix attempt complete.  The program will now automatically close.  You simply need to re-open the program (not re-boot the computer)." & Chr(10) & Chr(10) & "When the program re-starts, follow the process in the TOURNAMENT SETUP box on the right-hand side of the main menu screen.")
        Application.Exit()
    End Sub
    Function BuildPanelStringByRound(ByVal ds As DataSet, ByVal round As Integer) As String
        BuildPanelStringByRound = ""
        Dim fdPanels As DataRow()
        fdPanels = ds.Tables("Panel").Select("Round=" & round)
        For x = 0 To fdPanels.Length - 1
            If BuildPanelStringByRound = "" Then
                BuildPanelStringByRound = "(Panel="
            Else
                BuildPanelStringByRound &= " or panel="
            End If
            BuildPanelStringByRound &= fdPanels(x).Item("ID")
        Next x
        If BuildPanelStringByRound <> "" Then BuildPanelStringByRound &= ")"
        If BuildPanelStringByRound = "" Then BuildPanelStringByRound = "(Panel=-99)"
    End Function
    Function MakeSearchString(ByVal fdStuff As DataRow(), ByVal strField As String) As String
        'takes a datarow array and a field in it, and returns a search string that will pull items in the array from 
        'a different table; example=get an array of panels, and can get a search string to use with a .select command
        'that will return all the ballots in those panels
        Dim dummy As String = ""
        For y = 0 To fdStuff.Length - 1
            If y = 0 Then dummy = "(" Else dummy &= " or "
            dummy &= strField & "=" & fdStuff(y).Item("ID")
        Next y
        dummy &= ")"
        Return dummy
    End Function
    Sub PassWordCheck()
        If My.Settings.UserName = "" Then
            My.Settings.UserName = InputBox("Enter username:")
        End If
        If My.Settings.PassWord = "" Then
            My.Settings.PassWord = InputBox("Enter password:")
        End If
    End Sub
    Function OrdReport(ByVal ds As DataSet, ByVal dt As DataTable, ByVal MutMax As Integer) As DataTable
        'receives a pairings table
        Dim OrdPlace(4, 4) As Integer 'category, mutuality (1=good, 2=OK, 3=over)
        Dim WorstJudge As String : WorstJudge = ""
        Dim rat1, rat2, mut, cat As Single
        Dim tot, totMut, N, worst As Integer : worst = -1
        Dim dr As DataRow
        For x = 0 To dt.Rows.Count - 1
            rat1 = CInt(GetJudgeRating(ds, dt.Rows(x).Item("Judge1"), dt.Rows(x).Item("Team1"), "OrdPct"))
            rat2 = CInt(GetJudgeRating(ds, dt.Rows(x).Item("Judge1"), dt.Rows(x).Item("Team2"), "OrdPct"))
            If rat1 > 0 Then tot += rat1 : N += 1
            If rat2 > 0 Then tot += rat2 : N += 1
            If rat1 > worst Then worst = rat1 : WorstJudge = dt.Rows(x).Item("JudgeName1")
            If rat2 > worst Then worst = rat2 : WorstJudge = dt.Rows(x).Item("JudgeName1")
            If rat1 <= 30 And rat2 <= 30 Then
                cat = 1
            ElseIf rat1 <= 40 And rat2 <= 40 And (rat1 > 30 Or rat2 > 30) Then
                cat = 2
            ElseIf rat1 <= 50 And rat2 <= 50 And (rat1 > 40 Or rat2 > 40) Then
                cat = 3
            ElseIf rat1 > 50 Or rat2 > 50 Then
                cat = 4
            End If
            OrdPlace(cat, 1) += 1
            mut = Math.Abs(rat1 - rat2) : totMut += mut
            If mut < (MutMax / 2) Then
                OrdPlace(cat, 2) += 1
            ElseIf mut < MutMax Then
                OrdPlace(cat, 3) += 1
            Else
                OrdPlace(cat, 4) += 1
            End If
        Next x
        'Now put to a table
        Dim dt2 As New DataTable
        dt2.Columns.Add("Ordinals", System.Type.GetType("System.String"))
        dt2.Columns.Add("N", System.Type.GetType("System.Single"))
        dt2.Columns.Add("Mut GOOD", System.Type.GetType("System.Int16"))
        dt2.Columns.Add("Mut OK", System.Type.GetType("System.Int16"))
        dt2.Columns.Add("Mut OVER", System.Type.GetType("System.Int16"))
        For x = 1 To 4
            dr = dt2.NewRow
            If x = 1 Then dr.Item("Ordinals") = "Mutual top 30"
            If x = 2 Then dr.Item("Ordinals") = "Mutual top 40"
            If x = 3 Then dr.Item("Ordinals") = "Mutual top 50"
            If x = 4 Then dr.Item("Ordinals") = "Over 50"
            dr.Item("N") = OrdPlace(x, 1)
            dr.Item("Mut GOOD") = OrdPlace(x, 2)
            dr.Item("Mut OK") = OrdPlace(x, 3)
            dr.Item("Mut OVER") = OrdPlace(x, 4)
            dt2.Rows.Add(dr)
        Next x
        dr = dt2.NewRow : dr.Item("Ordinals") = "Average" : dr.Item("N") = FormatNumber(tot / N, 1) : dt2.Rows.Add(dr)
        dr = dt2.NewRow : dr.Item("Ordinals") = "Worst:" & WorstJudge : dr.Item("N") = worst : dt2.Rows.Add(dr)
        dr = dt2.NewRow : dr.Item("Ordinals") = "Avg Mutuality" : dr.Item("N") = FormatNumber(totMut / dt.Rows.Count - 1, 1) : dt2.Rows.Add(dr)
        Return dt2
    End Function
End Module
