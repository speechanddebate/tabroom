Module modInitialize
    'All of the table structures are set in the .xsd file, so you only need to populate the settings
    Sub InitializeTourneySettings(ByRef DS)
        'tourney settings table
        If DS.Tables.IndexOf("TOURN_SETTING") = -1 Then
            DS.Tables.Add("TOURN_SETTING")
        End If
        FieldCheck(DS, "TOURN_SETTING", "ID", "System.Int16")
        FieldCheck(DS, "TOURN_SETTING", "TAG", "System.String")
        FieldCheck(DS, "TOURN_SETTING", "VALUE", "System.String")
        TagCheck(DS, "TOURN_SETTING", "DownloadSite", "iDebate.org")
        TagCheck(DS, "TOURN_SETTING", "Online", "Track Users")
        TagCheck(DS, "TOURN_SETTING", "TourneyType", "Multi-Event")
        TagCheck(DS, "TOURN_SETTING", "SuppressNavMessages", "False")
        TagCheck(DS, "TOURN_SETTING", "UseActualTime", "False")
        TagCheck(DS, "TOURN_SETTING", "CrossEventEntry", "True")
    End Sub
    Sub TagCheck(ByRef ds As DataSet, ByVal strTblName As String, ByVal strTag As String, ByVal strValue As String)
        'checks for a particular tag in a table; exits if it's there, adds it if it's not
        'check if it exists
        Dim foundrow As DataRow()
        foundrow = ds.Tables(strTblName).Select("TAG='" & strTag & "'")
        'does, so exit
        If foundrow.Length = 1 Then Exit Sub
        'duplicate, so delete duplicate instances
        Dim x As Integer
        If foundrow.Length > 1 Then
            For x = 1 To foundrow.Length - 1
                foundrow(x).Delete()
            Next x
            ds.Tables(strTblName).AcceptChanges()
            Exit Sub
        End If
        'add if it doesn't
        Dim dr As DataRow
        dr = ds.Tables(strTblName).NewRow
        dr.Item("TAG") = strTag : dr.Item("VALUE") = strValue
        ds.Tables(strTblName).Rows.Add(dr)
        ds.Tables(strTblName).AcceptChanges()
    End Sub
    Sub FieldCheck(ByRef ds As DataSet, ByVal tablename As String, ByVal fieldname As String, ByVal strType As String)
        'checks if a field exists in a table, if not add it
        If ds.Tables(tablename).Columns.Contains(fieldname) = True Then Exit Sub
        ds.Tables(tablename).Columns.Add(fieldname, System.Type.GetType(strType))
        If tablename.ToUpper = "JUDGE" And InStr(fieldname.ToUpper, "EVENT") > 0 Then Call MarkAsTrue(ds, tablename, fieldname)
        If tablename.ToUpper = "JUDGE" And InStr(fieldname.ToUpper, "TIME") > 0 Then Call MarkAsTrue(ds, tablename, fieldname)
        If tablename.ToUpper = "ROOM" And InStr(fieldname.ToUpper, "EVENT") > 0 Then Call MarkAsTrue(ds, tablename, fieldname)
        If tablename.ToUpper = "ROOM" And InStr(fieldname.ToUpper, "TIME") > 0 Then Call MarkAsTrue(ds, tablename, fieldname)
    End Sub
    Sub MarkAsTrue(ByRef ds As DataSet, ByVal tablename As String, ByVal fieldname As String)
        For x = 0 To ds.Tables(tablename).Rows.Count - 1
            ds.Tables(tablename).Rows(x).Item(fieldname) = True
        Next x
        ds.Tables(tablename).Columns(fieldname).DefaultValue = True
    End Sub
    Sub InitializeDivisions(ByRef ds)
        Dim x, ctr As Integer
        Dim dr As DataRow
        For x = 0 To ds.Tables("EVENT").Rows.Count - 1
            'Debaters
            dr = ds.Tables("EVENT_SETTING").NewRow()
            ctr += 1 : dr.Item("ID") = ctr : dr.Item("Event") = ds.Tables("EVENT").Rows(x).Item("ID")
            dr.Item("TAG") = "DebatersPerTeam" : dr.Item("Value") = 2 : ds.Tables("EVENT_SETTING").Rows.Add(dr)
            'Level
            dr = ds.Tables("EVENT_SETTING").NewRow()
            ctr += 1 : dr.Item("ID") = ctr : dr.Item("Event") = ds.Tables("EVENT").Rows(x).Item("ID")
            dr.Item("TAG") = "Level" : dr.Item("Value") = "Open" : ds.Tables("EVENT_SETTING").Rows.Add(dr)
            'nPrelims
            dr = ds.Tables("EVENT_SETTING").NewRow()
            ctr += 1 : dr.Item("ID") = ctr : dr.Item("Event") = ds.Tables("EVENT").Rows(x).Item("ID")
            dr.Item("TAG") = "nPrelims" : dr.Item("Value") = 8 : ds.Tables("EVENT_SETTING").Rows.Add(dr)
            'nElims
            dr = ds.Tables("EVENT_SETTING").NewRow()
            ctr += 1 : dr.Item("ID") = ctr : dr.Item("Event") = ds.Tables("EVENT").Rows(x).Item("ID")
            dr.Item("TAG") = "nElims" : dr.Item("Value") = 5 : ds.Tables("EVENT_SETTING").Rows.Add(dr)
            'SideDesignations
            dr = ds.Tables("EVENT_SETTING").NewRow()
            ctr += 1 : dr.Item("ID") = ctr : dr.Item("Event") = ds.Tables("EVENT").Rows(x).Item("ID")
            dr.Item("TAG") = "SideDesignations" : dr.Item("Value") = "AffNeg" : ds.Tables("EVENT_SETTING").Rows.Add(dr)
            'TeamsPerRound
            dr = ds.Tables("EVENT_SETTING").NewRow()
            ctr += 1 : dr.Item("ID") = ctr : dr.Item("Event") = ds.Tables("EVENT").Rows(x).Item("ID")
            dr.Item("TAG") = "TeamsPerRound" : dr.Item("Value") = 2 : ds.Tables("EVENT_SETTING").Rows.Add(dr)
        Next
        ds.Tables("EVENT_SETTING").AcceptChanges()
    End Sub
    Sub InitializeTieBreakers(ByRef ds As DataSet, ByVal forced As Boolean)

        Dim HaveAlready As Boolean = True
        Dim x As Integer
        If ds.Tables.IndexOf("Tiebreak_Set") = -1 Then
            HaveAlready = False
        ElseIf ds.Tables("TieBreak_Set").Rows.Count = 0 Then
            HaveAlready = False
        End If

        'check for duplicates if tiebreakers exist
        Dim fdTB As DataRow() : Dim DeleteFired As Boolean = False
        If HaveAlready = True Then
            For x = 0 To ds.Tables("Tiebreak").Rows.Count - 1
                If ds.Tables("TieBreak").Rows(x).Item("Tag") <> "KILLME" Then
                    'check tag match
                    fdTB = ds.Tables("Tiebreak").Select("TB_SET=" & ds.Tables("Tiebreak").Rows(x).Item("TB_SET") & "and tag='" & ds.Tables("Tiebreak").Rows(x).Item("Tag") & "' and TAG<>'None'")
                    If fdTB.Length > 0 Then
                        For y = fdTB.Length - 1 To 1 Step -1
                            fdTB(y).Item("Tag") = "KILLME"
                        Next
                    End If
                    'check label match
                    fdTB = ds.Tables("Tiebreak").Select("TB_SET=" & ds.Tables("Tiebreak").Rows(x).Item("TB_SET") & "and tag='" & ds.Tables("Tiebreak").Rows(x).Item("Label") & "'")
                    If fdTB.Length > 0 Then
                        For y = fdTB.Length - 1 To 1 Step -1
                            fdTB(y).Item("Tag") = "KILLME"
                        Next
                    End If
                    'check content match
                    fdTB = ds.Tables("Tiebreak").Select("TB_SET=" & ds.Tables("Tiebreak").Rows(x).Item("TB_SET") & "and ScoreID=" & ds.Tables("Tiebreak").Rows(x).Item("ScoreID") & "and Drops=" & ds.Tables("Tiebreak").Rows(x).Item("Drops") & " and ForOpponent=" & ds.Tables("Tiebreak").Rows(x).Item("ForOpponent") & " and tag='None'")
                    If fdTB.Length > 0 Then
                        For y = fdTB.Length - 1 To 1 Step -1
                            fdTB(y).Item("Tag") = "KILLME"
                        Next
                    End If
                End If
            Next x
        End If
        For x = ds.Tables("Tiebreak").Rows.Count - 1 To 0 Step -1
            If ds.Tables("Tiebreak").Rows(x).Item("Tag") = "KILLME" Then
                ds.Tables("Tiebreak").Rows(x).Delete()
                DeleteFired = True
            End If
        Next x
        If DeleteFired = True Then MsgBox("Found and deleted duplicate tiebreakers.")

        'exit if there are already tiebreakers and the reset isn't forced by the usser
        If HaveAlready = True And forced = False Then Exit Sub

        'clear them all
        For x = ds.Tables("TIEBREAK_SET").Rows.Count - 1 To 0 Step -1
            ds.Tables("TIEBREAK_SET").Rows(x).Delete()
        Next
        ds.Tables("TIEBREAK").Clear()
        ds.AcceptChanges()

        'make new ones
        For x = 0 To ds.Tables("Event").Rows.Count - 1
            If getEventSetting(ds, ds.Tables("Event").Rows(x).Item("ID"), "TeamsPerRound") = 2 Then Call Add2TeamTB(ds)
            If getEventSetting(ds, ds.Tables("Event").Rows(x).Item("ID"), "TeamsPerRound") = 4 Then Call Add4TeamTB(ds)
        Next x

        'save
        ds.AcceptChanges()

    End Sub
    Sub Add2TeamTB(ByVal ds As DataSet)

        'check for existing rows
        Dim fdRow As DataRow()
        fdRow = ds.Tables("Tiebreak_set").Select("TBSET_NAME = '2 teams - Team Prelims'")
        If fdRow.Length > 0 Then Exit Sub

        Dim Set1 As Integer = ds.Tables("Tiebreak_set").Rows.Count + 1
        Dim Set2 As Integer = ds.Tables("Tiebreak_set").Rows.Count + 2

        Dim dr As DataRow
        Dim x As Integer
        'SET UP DEFAULT TB_SET
        dr = ds.Tables("TIEBREAK_SET").NewRow
        dr.Item("ID") = fdRow.Length + 1 : dr.Item("TBSET_NAME") = "2 teams - Team Prelims" : dr.Item("ScoreFor") = "Team" : ds.Tables("TIEBREAK_SET").Rows.Add(dr)
        dr = ds.Tables("TIEBREAK_SET").NewRow
        dr.Item("ID") = fdRow.Length + 2 : dr.Item("TBSET_NAME") = "2 teams - Speakers" : dr.Item("ScoreFor") = "Speaker" : ds.Tables("TIEBREAK_SET").Rows.Add(dr)
        dr = ds.Tables("TIEBREAK_SET").NewRow

        'SET UP DEFAULT TIEBREAKERS FOR THE TB_SET

        x = ds.Tables("Tiebreak").Rows.Count

        'TEAMS
        'team tiebreakers - row 1/wins
        dr = ds.Tables("TIEBREAK").NewRow : x = x + 1 : dr.Item("ID") = x
        dr.Item("SortOrder") = 1 : dr.Item("ScoreID") = 1 : dr.Item("Tag") = "Wins"
        dr.Item("Drops") = 0 : dr.Item("ForOpponent") = False : dr.Item("TB_SET") = Set1
        dr.Item("Label") = "Wins"
        ds.Tables("TIEBREAK").Rows.Add(dr)
        'team tiebreakers - row 2 hi-lo
        dr = ds.Tables("TIEBREAK").NewRow : x = x + 1 : dr.Item("ID") = x
        dr.Item("SortOrder") = 2 : dr.Item("ScoreID") = 2 : dr.Item("Tag") = "None"
        dr.Item("Drops") = 1 : dr.Item("ForOpponent") = False : dr.Item("TB_SET") = Set1
        dr.Item("Label") = "Hi-Lo team speaker points"
        ds.Tables("TIEBREAK").Rows.Add(dr)
        'team tiebreakers - row 3 total points
        dr = ds.Tables("TIEBREAK").NewRow : x = x + 1 : dr.Item("ID") = x
        dr.Item("SortOrder") = 3 : dr.Item("ScoreID") = 2 : dr.Item("Tag") = "None"
        dr.Item("Drops") = 0 : dr.Item("ForOpponent") = False : dr.Item("TB_SET") = Set1
        dr.Item("Label") = "Total team speaker points"
        ds.Tables("TIEBREAK").Rows.Add(dr)
        'team tiebreakers - row 4 ranks
        If frmTieBreakers.chkExcludeRanks.Checked = False Then
            dr = ds.Tables("TIEBREAK").NewRow : x = x + 1 : dr.Item("ID") = x
            dr.Item("SortOrder") = 4 : dr.Item("ScoreID") = 3 : dr.Item("Tag") = "None"
            dr.Item("Drops") = 0 : dr.Item("ForOpponent") = False : dr.Item("TB_SET") = Set1
            dr.Item("Label") = "Speaker ranks"
            ds.Tables("TIEBREAK").Rows.Add(dr)
        End If
        'team tiebreakers - row 5 2x HL points
        dr = ds.Tables("TIEBREAK").NewRow : x = x + 1 : dr.Item("ID") = x
        dr.Item("SortOrder") = 5 : dr.Item("ScoreID") = 2 : dr.Item("Tag") = "None"
        dr.Item("Drops") = 2 : dr.Item("ForOpponent") = False : dr.Item("TB_SET") = Set1
        dr.Item("Label") = "2x HL points"
        ds.Tables("TIEBREAK").Rows.Add(dr)
        'team tiebreakers - row 6 Opp Wins
        dr = ds.Tables("TIEBREAK").NewRow : x = x + 1 : dr.Item("ID") = x
        dr.Item("SortOrder") = 6 : dr.Item("ScoreID") = 1 : dr.Item("Tag") = "OppWins"
        dr.Item("Drops") = 0 : dr.Item("ForOpponent") = True : dr.Item("TB_SET") = Set1
        dr.Item("Label") = "Opposition Wins"
        ds.Tables("TIEBREAK").Rows.Add(dr)
        'team tiebreakers - row 7 Judge Variance
        dr = ds.Tables("TIEBREAK").NewRow : x = x + 1 : dr.Item("ID") = x
        dr.Item("SortOrder") = 7 : dr.Item("ScoreID") = 2 : dr.Item("Tag") = "JudgeVariance"
        dr.Item("Drops") = 0 : dr.Item("ForOpponent") = False : dr.Item("TB_SET") = Set1
        dr.Item("Label") = "Judge Variance"
        ds.Tables("TIEBREAK").Rows.Add(dr)
        'team tiebreakers - row 8 random
        dr = ds.Tables("TIEBREAK").NewRow : x = x + 1 : dr.Item("ID") = x
        dr.Item("SortOrder") = 8 : dr.Item("ScoreID") = 1 : dr.Item("Tag") = "Random"
        dr.Item("Drops") = 0 : dr.Item("ForOpponent") = False : dr.Item("TB_SET") = Set1
        dr.Item("Label") = "Random"
        ds.Tables("TIEBREAK").Rows.Add(dr)

        'SPEAKERS

        'Speaker tiebreakers - row 1/HL pts
        dr = ds.Tables("TIEBREAK").NewRow : x = x + 1 : dr.Item("ID") = x
        dr.Item("SortOrder") = 1 : dr.Item("ScoreID") = 2 : dr.Item("Tag") = "None"
        dr.Item("Drops") = 1 : dr.Item("ForOpponent") = False : dr.Item("TB_SET") = Set2
        dr.Item("Label") = "Hi-Lo Speaker points"
        ds.Tables("TIEBREAK").Rows.Add(dr)
        'Speaker tiebreakers - row 2/Total
        dr = ds.Tables("TIEBREAK").NewRow : x = x + 1 : dr.Item("ID") = x
        dr.Item("SortOrder") = 2 : dr.Item("ScoreID") = 2 : dr.Item("Tag") = "None"
        dr.Item("Drops") = 0 : dr.Item("ForOpponent") = False : dr.Item("TB_SET") = Set2
        dr.Item("Label") = "Total Speaker points"
        ds.Tables("TIEBREAK").Rows.Add(dr)
        'Speaker tiebreakers - row 3/2x HL
        dr = ds.Tables("TIEBREAK").NewRow : x = x + 1 : dr.Item("ID") = x
        dr.Item("SortOrder") = 3 : dr.Item("ScoreID") = 2 : dr.Item("Tag") = "None"
        dr.Item("Drops") = 2 : dr.Item("ForOpponent") = False : dr.Item("TB_SET") = Set2
        dr.Item("Label") = "2x Hi-Lo points"
        ds.Tables("TIEBREAK").Rows.Add(dr)
        'Speaker tiebreakers - row 4/ranks
        If frmTieBreakers.chkExcludeRanks.Checked = False Then
            dr = ds.Tables("TIEBREAK").NewRow : x = x + 1 : dr.Item("ID") = x
            dr.Item("SortOrder") = 4 : dr.Item("ScoreID") = 3 : dr.Item("Tag") = "None"
            dr.Item("Drops") = 0 : dr.Item("ForOpponent") = False : dr.Item("TB_SET") = Set2
            dr.Item("Label") = "Ranks"
            ds.Tables("TIEBREAK").Rows.Add(dr)
        End If
        'Speaker tiebreakers - row 5/OppWins
        dr = ds.Tables("TIEBREAK").NewRow : x = x + 1 : dr.Item("ID") = x
        dr.Item("SortOrder") = 5 : dr.Item("ScoreID") = 1 : dr.Item("Tag") = "OppWins"
        dr.Item("Drops") = 0 : dr.Item("ForOpponent") = True : dr.Item("TB_SET") = Set2
        dr.Item("Label") = "Opp Wins"
        ds.Tables("TIEBREAK").Rows.Add(dr)
        'Speaker tiebreakers - row 6/Judge variance
        dr = ds.Tables("TIEBREAK").NewRow : x = x + 1 : dr.Item("ID") = x
        dr.Item("SortOrder") = 6 : dr.Item("ScoreID") = 2 : dr.Item("Tag") = "JudgeVariance"
        dr.Item("Drops") = 0 : dr.Item("ForOpponent") = False : dr.Item("TB_SET") = Set2
        dr.Item("Label") = "Judge Variance"
        ds.Tables("TIEBREAK").Rows.Add(dr)
        'Speaker tiebreakers - row 7/Opp Pts
        dr = ds.Tables("TIEBREAK").NewRow : x = x + 1 : dr.Item("ID") = x
        dr.Item("SortOrder") = 7 : dr.Item("ScoreID") = 2 : dr.Item("Tag") = "None"
        dr.Item("Drops") = 0 : dr.Item("ForOpponent") = True : dr.Item("TB_SET") = Set2
        dr.Item("Label") = "Opp Pts"
        ds.Tables("TIEBREAK").Rows.Add(dr)
        'Speaker tiebreakers - row 8/Random
        dr = ds.Tables("TIEBREAK").NewRow : x = x + 1 : dr.Item("ID") = x
        dr.Item("SortOrder") = 8 : dr.Item("ScoreID") = 2 : dr.Item("Tag") = "Random"
        dr.Item("Drops") = 0 : dr.Item("ForOpponent") = False : dr.Item("TB_SET") = Set2
        dr.Item("Label") = "Random"
        ds.Tables("TIEBREAK").Rows.Add(dr)

    End Sub
    Sub Add2TeamTBNotDestructive(ByVal ds As DataSet)

        'check for existing rows
        Dim fdRow As DataRow()
        fdRow = ds.Tables("Tiebreak_set").Select("TBSET_NAME = '2 teams - Team Prelims'")

        Dim Set1 As Integer
        Dim Set2 As Integer

        Dim dr As DataRow : Dim dr2 As DataRow()
        Dim x As Integer

        'Check for the TB set, add one if not present
        dr2 = ds.Tables("Tiebreak_Set").Select("ScoreFor='Team'")
        If dr2 Is Nothing Then
            dr = ds.Tables("TIEBREAK_SET").NewRow
            dr.Item("ID") = fdRow.Length + 1 : dr.Item("TBSET_NAME") = "2 teams - Team Prelims" : dr.Item("ScoreFor") = "Team" : ds.Tables("TIEBREAK_SET").Rows.Add(dr)
            Set1 = ds.Tables("Tiebreak_set").Rows.Count + 1
        Else
            Set1 = dr2(0).Item("ID")
        End If
        dr2 = ds.Tables("Tiebreak_Set").Select("ScoreFor='Speaker'")
        If dr2 Is Nothing Then
            dr = ds.Tables("TIEBREAK_SET").NewRow
            dr.Item("ID") = fdRow.Length + 2 : dr.Item("TBSET_NAME") = "2 teams - Speakers" : dr.Item("ScoreFor") = "Speaker" : ds.Tables("TIEBREAK_SET").Rows.Add(dr)
            Set2 = ds.Tables("Tiebreak_set").Rows.Count + 2
        Else
            Set2 = dr2(0).Item("ID")
        End If

        dr = ds.Tables("TIEBREAK_SET").NewRow

        'SET UP DEFAULT TIEBREAKERS FOR THE TB_SET

        x = ds.Tables("Tiebreak").Rows.Count

        'TEAMS
        'team tiebreakers - row 1/wins
        dr = ds.Tables("TIEBREAK").NewRow : x = x + 1 : dr.Item("ID") = x
        dr.Item("SortOrder") = 1 : dr.Item("ScoreID") = 1 : dr.Item("Tag") = "Wins"
        dr.Item("Drops") = 0 : dr.Item("ForOpponent") = False : dr.Item("TB_SET") = Set1
        dr.Item("Label") = "Wins"
        ds.Tables("TIEBREAK").Rows.Add(dr)
        'team tiebreakers - row 2 hi-lo
        dr = ds.Tables("TIEBREAK").NewRow : x = x + 1 : dr.Item("ID") = x
        dr.Item("SortOrder") = 2 : dr.Item("ScoreID") = 2 : dr.Item("Tag") = "None"
        dr.Item("Drops") = 1 : dr.Item("ForOpponent") = False : dr.Item("TB_SET") = Set1
        dr.Item("Label") = "Hi-Lo team speaker points"
        ds.Tables("TIEBREAK").Rows.Add(dr)
        'team tiebreakers - row 3 total points
        dr = ds.Tables("TIEBREAK").NewRow : x = x + 1 : dr.Item("ID") = x
        dr.Item("SortOrder") = 3 : dr.Item("ScoreID") = 2 : dr.Item("Tag") = "None"
        dr.Item("Drops") = 0 : dr.Item("ForOpponent") = False : dr.Item("TB_SET") = Set1
        dr.Item("Label") = "Total team speaker points"
        ds.Tables("TIEBREAK").Rows.Add(dr)
        'team tiebreakers - row 4 ranks
        If frmTieBreakers.chkExcludeRanks.Checked = False Then
            dr = ds.Tables("TIEBREAK").NewRow : x = x + 1 : dr.Item("ID") = x
            dr.Item("SortOrder") = 4 : dr.Item("ScoreID") = 3 : dr.Item("Tag") = "None"
            dr.Item("Drops") = 0 : dr.Item("ForOpponent") = False : dr.Item("TB_SET") = Set1
            dr.Item("Label") = "Speaker ranks"
            ds.Tables("TIEBREAK").Rows.Add(dr)
        End If
        'team tiebreakers - row 5 2x HL points
        dr = ds.Tables("TIEBREAK").NewRow : x = x + 1 : dr.Item("ID") = x
        dr.Item("SortOrder") = 5 : dr.Item("ScoreID") = 2 : dr.Item("Tag") = "None"
        dr.Item("Drops") = 2 : dr.Item("ForOpponent") = False : dr.Item("TB_SET") = Set1
        dr.Item("Label") = "2x HL points"
        ds.Tables("TIEBREAK").Rows.Add(dr)
        'team tiebreakers - row 6 Opp Wins
        dr = ds.Tables("TIEBREAK").NewRow : x = x + 1 : dr.Item("ID") = x
        dr.Item("SortOrder") = 6 : dr.Item("ScoreID") = 1 : dr.Item("Tag") = "OppWins"
        dr.Item("Drops") = 0 : dr.Item("ForOpponent") = True : dr.Item("TB_SET") = Set1
        dr.Item("Label") = "Opposition Wins"
        ds.Tables("TIEBREAK").Rows.Add(dr)
        'team tiebreakers - row 7 Judge Variance
        dr = ds.Tables("TIEBREAK").NewRow : x = x + 1 : dr.Item("ID") = x
        dr.Item("SortOrder") = 7 : dr.Item("ScoreID") = 2 : dr.Item("Tag") = "JudgeVariance"
        dr.Item("Drops") = 0 : dr.Item("ForOpponent") = False : dr.Item("TB_SET") = Set1
        dr.Item("Label") = "Judge Variance"
        ds.Tables("TIEBREAK").Rows.Add(dr)
        'team tiebreakers - row 8 random
        dr = ds.Tables("TIEBREAK").NewRow : x = x + 1 : dr.Item("ID") = x
        dr.Item("SortOrder") = 8 : dr.Item("ScoreID") = 1 : dr.Item("Tag") = "Random"
        dr.Item("Drops") = 0 : dr.Item("ForOpponent") = False : dr.Item("TB_SET") = Set1
        dr.Item("Label") = "Random"
        ds.Tables("TIEBREAK").Rows.Add(dr)

        'SPEAKERS

        'Speaker tiebreakers - row 1/HL pts
        dr = ds.Tables("TIEBREAK").NewRow : x = x + 1 : dr.Item("ID") = x
        dr.Item("SortOrder") = 1 : dr.Item("ScoreID") = 2 : dr.Item("Tag") = "None"
        dr.Item("Drops") = 1 : dr.Item("ForOpponent") = False : dr.Item("TB_SET") = Set2
        dr.Item("Label") = "Hi-Lo Speaker points"
        ds.Tables("TIEBREAK").Rows.Add(dr)
        'Speaker tiebreakers - row 2/Total
        dr = ds.Tables("TIEBREAK").NewRow : x = x + 1 : dr.Item("ID") = x
        dr.Item("SortOrder") = 2 : dr.Item("ScoreID") = 2 : dr.Item("Tag") = "None"
        dr.Item("Drops") = 0 : dr.Item("ForOpponent") = False : dr.Item("TB_SET") = Set2
        dr.Item("Label") = "Total Speaker points"
        ds.Tables("TIEBREAK").Rows.Add(dr)
        'Speaker tiebreakers - row 3/2x HL
        dr = ds.Tables("TIEBREAK").NewRow : x = x + 1 : dr.Item("ID") = x
        dr.Item("SortOrder") = 3 : dr.Item("ScoreID") = 2 : dr.Item("Tag") = "None"
        dr.Item("Drops") = 2 : dr.Item("ForOpponent") = False : dr.Item("TB_SET") = Set2
        dr.Item("Label") = "2x Hi-Lo points"
        ds.Tables("TIEBREAK").Rows.Add(dr)
        'Speaker tiebreakers - row 4/ranks
        If frmTieBreakers.chkExcludeRanks.Checked = False Then
            dr = ds.Tables("TIEBREAK").NewRow : x = x + 1 : dr.Item("ID") = x
            dr.Item("SortOrder") = 4 : dr.Item("ScoreID") = 3 : dr.Item("Tag") = "None"
            dr.Item("Drops") = 0 : dr.Item("ForOpponent") = False : dr.Item("TB_SET") = Set2
            dr.Item("Label") = "Ranks"
            ds.Tables("TIEBREAK").Rows.Add(dr)
        End If
        'Speaker tiebreakers - row 5/OppWins
        dr = ds.Tables("TIEBREAK").NewRow : x = x + 1 : dr.Item("ID") = x
        dr.Item("SortOrder") = 5 : dr.Item("ScoreID") = 1 : dr.Item("Tag") = "OppWins"
        dr.Item("Drops") = 0 : dr.Item("ForOpponent") = True : dr.Item("TB_SET") = Set2
        dr.Item("Label") = "Opp Wins"
        ds.Tables("TIEBREAK").Rows.Add(dr)
        'Speaker tiebreakers - row 6/Judge variance
        dr = ds.Tables("TIEBREAK").NewRow : x = x + 1 : dr.Item("ID") = x
        dr.Item("SortOrder") = 6 : dr.Item("ScoreID") = 2 : dr.Item("Tag") = "JudgeVariance"
        dr.Item("Drops") = 0 : dr.Item("ForOpponent") = False : dr.Item("TB_SET") = Set2
        dr.Item("Label") = "Judge Variance"
        ds.Tables("TIEBREAK").Rows.Add(dr)
        'Speaker tiebreakers - row 7/Opp Pts
        dr = ds.Tables("TIEBREAK").NewRow : x = x + 1 : dr.Item("ID") = x
        dr.Item("SortOrder") = 7 : dr.Item("ScoreID") = 2 : dr.Item("Tag") = "None"
        dr.Item("Drops") = 0 : dr.Item("ForOpponent") = True : dr.Item("TB_SET") = Set2
        dr.Item("Label") = "Opp Pts"
        ds.Tables("TIEBREAK").Rows.Add(dr)
        'Speaker tiebreakers - row 8/Random
        dr = ds.Tables("TIEBREAK").NewRow : x = x + 1 : dr.Item("ID") = x
        dr.Item("SortOrder") = 8 : dr.Item("ScoreID") = 2 : dr.Item("Tag") = "Random"
        dr.Item("Drops") = 0 : dr.Item("ForOpponent") = False : dr.Item("TB_SET") = Set2
        dr.Item("Label") = "Random"
        ds.Tables("TIEBREAK").Rows.Add(dr)

    End Sub
    Sub Add4TeamTB(ByVal ds As DataSet)

        'check for existing rows
        Dim fdRow As DataRow()
        fdRow = ds.Tables("Tiebreak_set").Select("TBSET_NAME = '4 teams - Team Prelims'")
        If fdRow.Length > 0 Then Exit Sub

        Dim dr As DataRow
        Dim Set1 As Integer = ds.Tables("Tiebreak_set").Rows.Count + 1
        Dim Set2 As Integer = ds.Tables("Tiebreak_set").Rows.Count + 1

        'SET UP DEFAULT TB_SET
        dr = ds.Tables("TIEBREAK_SET").NewRow
        dr.Item("ID") = Set1 : dr.Item("TBSET_NAME") = "4 teams - Team Prelims" : dr.Item("ScoreFor") = "Team" : ds.Tables("TIEBREAK_SET").Rows.Add(dr)
        dr = ds.Tables("TIEBREAK_SET").NewRow
        'dr.Item("ID") = Set2 : dr.Item("TBSET_NAME") = "4 teams - Speakers" : dr.Item("ScoreFor") = "Speaker" : ds.Tables("TIEBREAK_SET").Rows.Add(dr)
        'dr = ds.Tables("TIEBREAK_SET").NewRow

        'SET UP DEFAULT TIEBREAKERS FOR THE TB_SET

        Dim x As Integer = ds.Tables("Tiebreak").Rows.Count

        'TEAMS
        'team tiebreakers - row 1/wins
        dr = ds.Tables("TIEBREAK").NewRow : x = x + 1 : dr.Item("ID") = x
        dr.Item("SortOrder") = 1 : dr.Item("ScoreID") = 5 : dr.Item("Tag") = "None"
        dr.Item("Drops") = 0 : dr.Item("ForOpponent") = False : dr.Item("TB_SET") = Set1
        dr.Item("Label") = "Team Ranks"
        ds.Tables("TIEBREAK").Rows.Add(dr)
        'team tiebreakers - row 2 hi-lo
        dr = ds.Tables("TIEBREAK").NewRow : x = x + 1 : dr.Item("ID") = x
        dr.Item("SortOrder") = 2 : dr.Item("ScoreID") = 2 : dr.Item("Tag") = "None"
        dr.Item("Drops") = 0 : dr.Item("ForOpponent") = False : dr.Item("TB_SET") = Set1
        dr.Item("Label") = "Team speaker points"
        ds.Tables("TIEBREAK").Rows.Add(dr)
        'team tiebreakers - ranks of 3
        dr = ds.Tables("TIEBREAK").NewRow : x = x + 1 : dr.Item("ID") = x
        dr.Item("SortOrder") = 3 : dr.Item("ScoreID") = 5 : dr.Item("Tag") = "None"
        dr.Item("Drops") = 0 : dr.Item("ForOpponent") = False : dr.Item("TB_SET") = Set1
        dr.Item("Label") = "Total 3 ranks"
        ds.Tables("TIEBREAK").Rows.Add(dr)
        'team tiebreakers - ranks of 2
        If frmTieBreakers.chkExcludeRanks.Checked = False Then
            dr = ds.Tables("TIEBREAK").NewRow : x = x + 1 : dr.Item("ID") = x
            dr.Item("SortOrder") = 4 : dr.Item("ScoreID") = 5 : dr.Item("Tag") = "None"
            dr.Item("Drops") = 1 : dr.Item("ForOpponent") = False : dr.Item("TB_SET") = Set1
            dr.Item("Label") = "Total 2 ranks"
            ds.Tables("TIEBREAK").Rows.Add(dr)
        End If
        'team tiebreakers - ranks of 1
        dr = ds.Tables("TIEBREAK").NewRow : x = x + 1 : dr.Item("ID") = x
        dr.Item("SortOrder") = 5 : dr.Item("ScoreID") = 5 : dr.Item("Tag") = "None"
        dr.Item("Drops") = 2 : dr.Item("ForOpponent") = False : dr.Item("TB_SET") = Set1
        dr.Item("Label") = "Total 1 ranks"
        ds.Tables("TIEBREAK").Rows.Add(dr)
        'team tiebreakers - random
        dr = ds.Tables("TIEBREAK").NewRow : x = x + 1 : dr.Item("ID") = x
        dr.Item("SortOrder") = 6 : dr.Item("ScoreID") = 1 : dr.Item("Tag") = "Random"
        dr.Item("Drops") = 0 : dr.Item("ForOpponent") = False : dr.Item("TB_SET") = Set1
        dr.Item("Label") = "Random"
        ds.Tables("TIEBREAK").Rows.Add(dr)

        'SPEAKERS

        'Speaker tiebreakers - row 1/HL pts
        dr = ds.Tables("TIEBREAK").NewRow : x = x + 1 : dr.Item("ID") = x
        dr.Item("SortOrder") = 1 : dr.Item("ScoreID") = 2 : dr.Item("Tag") = "None"
        dr.Item("Drops") = 1 : dr.Item("ForOpponent") = False : dr.Item("TB_SET") = Set2
        dr.Item("Label") = "Hi-Lo Speaker points"
        ds.Tables("TIEBREAK").Rows.Add(dr)
        'Speaker tiebreakers - row 2/Total
        dr = ds.Tables("TIEBREAK").NewRow : x = x + 1 : dr.Item("ID") = x
        dr.Item("SortOrder") = 2 : dr.Item("ScoreID") = 2 : dr.Item("Tag") = "None"
        dr.Item("Drops") = 0 : dr.Item("ForOpponent") = False : dr.Item("TB_SET") = Set2
        dr.Item("Label") = "Total Speaker points"
        ds.Tables("TIEBREAK").Rows.Add(dr)
        'Speaker tiebreakers - row 3/2x HL
        dr = ds.Tables("TIEBREAK").NewRow : x = x + 1 : dr.Item("ID") = x
        dr.Item("SortOrder") = 3 : dr.Item("ScoreID") = 2 : dr.Item("Tag") = "None"
        dr.Item("Drops") = 2 : dr.Item("ForOpponent") = False : dr.Item("TB_SET") = Set2
        dr.Item("Label") = "2x Hi-Lo points"
        ds.Tables("TIEBREAK").Rows.Add(dr)
        'Speaker tiebreakers - row 4/ranks
        If frmTieBreakers.chkExcludeRanks.Checked = False Then
            dr = ds.Tables("TIEBREAK").NewRow : x = x + 1 : dr.Item("ID") = x
            dr.Item("SortOrder") = 4 : dr.Item("ScoreID") = 3 : dr.Item("Tag") = "None"
            dr.Item("Drops") = 0 : dr.Item("ForOpponent") = False : dr.Item("TB_SET") = Set2
            dr.Item("Label") = "Ranks"
            ds.Tables("TIEBREAK").Rows.Add(dr)
        End If
        'Speaker tiebreakers - row 5/OppWins
        dr = ds.Tables("TIEBREAK").NewRow : x = x + 1 : dr.Item("ID") = x
        dr.Item("SortOrder") = 5 : dr.Item("ScoreID") = 1 : dr.Item("Tag") = "OppWins"
        dr.Item("Drops") = 0 : dr.Item("ForOpponent") = True : dr.Item("TB_SET") = Set2
        dr.Item("Label") = "Opp Wins"
        ds.Tables("TIEBREAK").Rows.Add(dr)
        'Speaker tiebreakers - row 6/Opp Pts
        dr = ds.Tables("TIEBREAK").NewRow : x = x + 1 : dr.Item("ID") = x
        dr.Item("SortOrder") = 7 : dr.Item("ScoreID") = 2 : dr.Item("Tag") = "None"
        dr.Item("Drops") = 0 : dr.Item("ForOpponent") = True : dr.Item("TB_SET") = Set2
        dr.Item("Label") = "Opp Pts"
        ds.Tables("TIEBREAK").Rows.Add(dr)
        'Speaker tiebreakers - row 7/Random
        dr = ds.Tables("TIEBREAK").NewRow : x = x + 1 : dr.Item("ID") = x
        dr.Item("SortOrder") = 8 : dr.Item("ScoreID") = 2 : dr.Item("Tag") = "Random"
        dr.Item("Drops") = 0 : dr.Item("ForOpponent") = False : dr.Item("TB_SET") = Set2
        dr.Item("Label") = "Random"
        ds.Tables("TIEBREAK").Rows.Add(dr)

    End Sub
    Sub InitializeScores(ByRef ds As DataSet)

        'don't reset because ID index needs to stay constant
        Dim HaveAlready As Boolean = True
        If ds.Tables.IndexOf("SCORES") = -1 Then HaveAlready = False
        If HaveAlready = True Then
            If ds.Tables("Scores").Rows.Count <> 5 Then HaveAlready = False
        End If
        If HaveAlready = True Then Exit Sub

        'clear the table
        Dim dr As DataRow
        Dim x As Integer
        For x = ds.Tables("SCORES").Rows.Count - 1 To 0 Step -1
            ds.Tables("SCORES").Rows(x).Delete()
        Next
        ds.Tables("SCORES").AcceptChanges()
        ds.Tables("Scores").Clear()
        ds.Tables("Scores").Columns("ID").AutoIncrementSeed = 1

        dr = ds.Tables("SCORES").NewRow
        dr.Item("SCOREFOR") = "Team" : dr.Item("SCORE_NAME") = "Ballot" : dr.Item("SORTORDER") = "DESC"
        ds.Tables("SCORES").Rows.Add(dr)

        dr = ds.Tables("SCORES").NewRow
        dr.Item("SCOREFOR") = "Speaker" : dr.Item("SCORE_NAME") = "Speaker Points" : dr.Item("SORTORDER") = "DESC"
        ds.Tables("SCORES").Rows.Add(dr)

        dr = ds.Tables("SCORES").NewRow
        dr.Item("SCOREFOR") = "Speaker" : dr.Item("SCORE_NAME") = "Speaker Rank" : dr.Item("SORTORDER") = "ASC"
        ds.Tables("SCORES").Rows.Add(dr)

        dr = ds.Tables("SCORES").NewRow
        dr.Item("SCOREFOR") = "Team" : dr.Item("SCORE_NAME") = "Team Points" : dr.Item("SORTORDER") = "DESC"
        ds.Tables("SCORES").Rows.Add(dr)

        dr = ds.Tables("SCORES").NewRow
        dr.Item("SCOREFOR") = "Team" : dr.Item("SCORE_NAME") = "Team Rank" : dr.Item("SORTORDER") = "DESC"
        ds.Tables("SCORES").Rows.Add(dr)

        ds.AcceptChanges()

    End Sub
    Sub InitializeScoreSettings(ByRef ds As DataSet)

        'check whether you need to fire this
        Dim HaveAlready As Boolean = True
        If ds.Tables.IndexOf("SCORE_SETTING") = -1 Then HaveAlready = False
        If HaveAlready = True Then
            If ds.Tables("Score_Setting").Rows.Count <> ds.Tables("Tiebreak_Set").Rows.Count * 5 Then HaveAlready = False
        End If
        If HaveAlready = True Then Exit Sub

        Dim dr As DataRow

        For x = ds.Tables("SCORE_SETTING").Rows.Count - 1 To 0 Step -1
            ds.Tables("SCORE_SETTING").Rows(x).Delete()
        Next

        For x = 0 To ds.Tables("Tiebreak_Set").Rows.Count - 1
            For y = 1 To 5
                dr = ds.Tables("SCORE_SETTING").NewRow
                dr.Item("TB_SET") = ds.Tables("Tiebreak_set").Rows(x).Item("ID")
                dr.Item("Score") = y
                If y = 1 Then dr.Item("MAX") = 1 : dr.Item("MIN") = 0
                If y = 2 Or y = 4 Then dr.Item("MAX") = 30 : dr.Item("MIN") = 1
                If y = 3 Or y = 5 Then dr.Item("MAX") = 4 : dr.Item("MIN") = 1
                dr.Item("DUPESOK") = "False" : If y = 2 Or y = 4 Then dr.Item("DUPESOK") = "False"
                dr.Item("DecimalIncrements") = 0 : If y = 2 Or y = 4 Then If y = 2 Or y = 4 Then dr.Item("DECIMALINCREMENTS") = 0.1
                ds.Tables("SCORE_SETTING").Rows.Add(dr)
            Next y
        Next x

    End Sub
    Sub InitializeTimeSlots(ByRef ds, ByVal nRounds)

        Dim q As Integer = MsgBox("This will erase and reset all timeslots.  If you have already set up the rounds, it will be much better to add or delete individual timeslots as necessary.  If you reset all the timeslots, make sure you visit the SET UP ROUND SCHEDULE screen and reset all the timeslots.  Continue with reset?", MsgBoxStyle.YesNo)
        If q = vbNo Then Exit Sub

        'check that the table exists and is formatted correctly
        If ds.Tables.IndexOf("TIMESLOT") = -1 Then
            ds.Tables.Add("TIMESLOT")
        End If
        FieldCheck(ds, "TIMESLOT", "ID", "System.Int64")
        FieldCheck(ds, "TIMESLOT", "TIMESLOTNAME", "System.String")
        FieldCheck(ds, "TIMESLOT", "STARTTIME", "System.DateTime")
        FieldCheck(ds, "TIMESLOT", "ENDTIME", "System.DateTime")


        ds.tables("Timeslot").clear()
        Dim dr As DataRow
        For x = 0 To nRounds - 1
            dr = ds.tables("TimeSlot").newrow
            dr.Item("TimeSlotName") = "TimeSlot # " & x + 1
            If dr.Item("ID") Is System.DBNull.Value Then
                dr.Item("ID") = x + 1
            End If
            ds.tables("TimeSlot").rows.add(dr)
        Next x

    End Sub
    Sub InitializeViewSettings(ByRef ds As DataSet)
        If ds.Tables.IndexOf("VIEWSETTINGS") = -1 Then
            ds.Tables.Add("VIEWSETTINGS")
        End If
        FieldCheck(ds, "VIEWSETTINGS", "ROUND", "System.Int64")
        FieldCheck(ds, "VIEWSETTINGS", "TAG", "System.String")
        FieldCheck(ds, "VIEWSETTINGS", "SORTORDER", "System.Int16")

    End Sub
    Sub InitializeJudges(ByRef ds As DataSet)
        If ds.Tables.IndexOf("JUDGE") = -1 Then
            ds.Tables.Add("JUDGE")
        End If
        'add columns for every event
        For x = 0 To ds.Tables("Event").Rows.Count - 1
            FieldCheck(ds, "JUDGE", "Event" & ds.Tables("Event").Rows(x).Item("ID"), "System.Boolean")
        Next x
        'add columns for every timeslot
        For x = 0 To ds.Tables("Timeslot").Rows.Count - 1
            FieldCheck(ds, "JUDGE", "Timeslot" & ds.Tables("timeslot").Rows(x).Item("ID"), "System.Boolean")
        Next x
        'kill null values
        For x = 0 To ds.Tables("Judge").Rows.Count - 1
            If ds.Tables("Judge").Rows(x).Item("Hired") Is System.DBNull.Value Then ds.Tables("Judge").Rows(x).Item("Hired") = 0
            If ds.Tables("Judge").Rows(x).Item("Obligation") Is System.DBNull.Value Then ds.Tables("Judge").Rows(x).Item("Obligation") = 0
        Next x
    End Sub
    Sub InitializeRooms(ByRef ds As DataSet)
        If ds.Tables.IndexOf("ROOM") = -1 Then
            ds.Tables.Add("ROOM")
        End If
        'add columns for every event
        For x = 0 To ds.Tables("Event").Rows.Count - 1
            FieldCheck(ds, "ROOM", "Event" & ds.Tables("Event").Rows(x).Item("ID"), "System.Boolean")
        Next x
        'add columns for every timeslot
        For x = 0 To ds.Tables("Timeslot").Rows.Count - 1
            FieldCheck(ds, "ROOM", "Timeslot" & ds.Tables("timeslot").Rows(x).Item("ID"), "System.Boolean")
        Next x
    End Sub
    Sub SetupCompletionStatus(ByVal ds As DataSet)

        frmMainMenu.lblSetupStatus.Text = "Setup complete!  Ready to start pairing."

        'TOURNAMENT-WIDE STUFF
        If ds.Tables("Tourn").Rows.Count = 0 Then
            Dim dr As DataRow
            dr = ds.Tables("Tourn").NewRow : dr.Item("ID") = -99
            dr.Item("TournName") = "Enter Tournament Name"
            ds.Tables("Tourn").Rows.Add(dr)
            ds.Tables("Tourn").AcceptChanges()
            Call SaveFile(ds)
        End If
        If ds.Tables("Tourn").Rows(0).Item("Tournname").trim = "" Then
            frmMainMenu.lblSetupStatus.Text = "No tournament name is identified.  Enter one by clicking the ENTER TOURNAMENT-WIDE SETTINGS button." : Exit Sub
        End If
        If ds.Tables("Tourn").Rows(0).Item("ID") = 0 Then
            frmMainMenu.lblSetupStatus.Text = "No tournament has been identified.  Either download one or start from scratch." : Exit Sub
        End If
        If ds.Tables.IndexOf("TOURN_SETTING") = -1 Then
            frmMainMenu.lblSetupStatus.Text = "No tournament settings exist in the database.  Click on the ENTER TOURNAMENT-WIDE SETTINGS button to create defaults." : Exit Sub
        End If
        If ds.Tables("TOURN_SETTING").Rows.Count < 6 Then
            frmMainMenu.lblSetupStatus.Text = "Not all tournament settings exist in the database.  Click on the ENTER TOURNAMENT-WIDE SETTINGS button to create defaults." : Exit Sub
        End If
        If ds.Tables.IndexOf("Timeslot") = -1 Or ds.Tables("Timeslot").Rows.Count = 0 Then
            frmMainMenu.lblSetupStatus.Text = "No timeslots have yet been created.  Click on the ENTER TOURNAMENT-WIDE SETTINGS button to create defaults." : Exit Sub
        End If

        'DIVISION-WIDE STUFF
        'division exist
        If ds.Tables("Event").Rows.Count = 0 Then
            frmMainMenu.lblSetupStatus.Text = "No divisions have been entered yet.  Click on the SET UP DIVISIONS button to enter them." : Exit Sub
        End If
        'all divisions have settings
        Dim fdRows As DataRow()
        For x = 0 To ds.Tables("Event").Rows.Count - 1
            fdRows = ds.Tables("Event_Setting").Select("Event=" & ds.Tables("Event").Rows(x).Item("ID"))
            If fdRows.Length = 0 Then frmMainMenu.lblSetupStatus.Text = "One or more divisions appear to be missing their settings.  Click on the SET UP DIVISIONS button to enter them.  You can view the event settings by clicking on the name of each event on the set up screen." : Exit Sub
            If fdRows.Length < 9 Then frmMainMenu.lblSetupStatus.Text = "One or more divisions appear to have incomplete settings.  Click on the SET UP DIVISIONS button to reset them.  You can view the event settings by clicking on the name of each event on the set up screen; all divisions should have 9 settings." : Exit Sub
        Next x

        'TIEBREAKERS
        'at least 1 TIBREAK_SET
        If ds.Tables("Tiebreak_Set").Rows.Count = 0 Then
            frmMainMenu.lblSetupStatus.Text = "There are no tiebreaker sets set up.  Click on the SET UP TIEBREAKERS button to enter them.  Click the help button on that page to familiarize yourself with the difference between tiebreakers, tiebreaker sets, and scores." : Exit Sub
        End If
        'all tiebreaker sets have tiebreakers
        For x = 0 To ds.Tables("Tiebreak_Set").Rows.Count - 1
            fdRows = ds.Tables("Tiebreak").Select("TB_Set=" & ds.Tables("Tiebreak_set").Rows(x).Item("ID"))
            If fdRows.Length = 0 Then frmMainMenu.lblSetupStatus.Text = "At least one tiebreaker set has no tiebreakers.  Click on the SET UP TIEBREAKERS button to enter them.  Click the help button on that page to familiarize yourself with the difference between tiebreakers, tiebreaker sets, and scores." : Exit Sub
        Next x
        'scores exit
        If ds.Tables("Scores").Rows.Count = 0 Then
            frmMainMenu.lblSetupStatus.Text = "No scores appear to be set up.  Click on the SET UP TIEBREAKERS button to enter them.  Click the help button on that page to familiarize yourself with the difference between tiebreakers, tiebreaker sets, and scores." : Exit Sub
        End If
        'scores have settings
        If ds.Tables("Score_Setting").Rows.Count <> (5 * ds.Tables("Tiebreak_Set").Rows.Count) Then frmMainMenu.lblSetupStatus.Text = "Each score is not matched to a group of settings.  Click on the SET UP TIEBREAKERS button to enter them.  Click the help button on that page to familiarize yourself with the difference between tiebreakers, tiebreaker sets, and scores." : Exit Sub
        For x = 0 To ds.Tables("Scores").Rows.Count - 1
            fdRows = ds.Tables("Score_Setting").Select("Score=" & ds.Tables("Scores").Rows(x).Item("ID"))
            If fdRows.Length = 0 Then frmMainMenu.lblSetupStatus.Text = "At least one of the scores has no settings.  Click on the SET UP TIEBREAKERS button to enter them.  Click the help button on that page to familiarize yourself with the difference between tiebreakers, tiebreaker sets, and scores." : Exit Sub
        Next x

        'Check that ROUNDS are ready to go
        Dim dummy As Integer
        For x = 0 To ds.Tables("Event").Rows.Count - 1
            fdRows = ds.Tables("Round").Select("Event=" & ds.Tables("Event").Rows(x).Item("ID"))
            If fdRows.Length = 0 Then
                frmMainMenu.lblSetupStatus.Text = "No rounds appear to be set up for one or more events.  Click on the SET UP ROUND SCHEDULE button to enter them.  Click the help button on that page for more inofmration.  Note that you must first set up events/divisions and tiebreakers." : Exit Sub
            End If
            dummy = getEventSetting(ds, ds.Tables("Event").Rows(x).Item("ID"), "nPrelims")
            dummy += getEventSetting(ds, ds.Tables("Event").Rows(x).Item("ID"), "nElims")
            If fdRows.Length < dummy Then
                frmMainMenu.lblSetupStatus.Text = "Some rounds exist, but one or more events/divisions are missing rounds. Click on the SET UP ROUND SCHEDULE button to enter them. It is also possible that the number of elims for a division has been set incorrectly; fix that on the SET UP DIVISIONS screen." : Exit Sub
            End If
        Next x

        'See if ROOMS AND JUDGES are initialized
        If RoomJudgeCheck(ds, "Judge", "Event") <> "OK" Then frmMainMenu.lblSetupStatus.Text = "Judges do not appear to be initialized for event/division eligibility.  Go to the SET UP ROUND SCHEDULE screen and click on the INITIALIZE ROOMS AND JUDGES button. " : Exit Sub
        If RoomJudgeCheck(ds, "Judge", "Timeslot") <> "OK" Then frmMainMenu.lblSetupStatus.Text = "Judges do not appear to be initialized for event/division eligibility.  Go to the SET UP ROUND SCHEDULE screen and click on the INITIALIZE ROOMS AND JUDGES button. " : Exit Sub
        If ds.Tables("Room").Rows.Count > 0 Then
            If RoomJudgeCheck(ds, "Room", "Event") <> "OK" Then frmMainMenu.lblSetupStatus.Text = "Judges do not appear to be initialized for event/division eligibility.  Go to the SET UP ROUND SCHEDULE screen and click on the INITIALIZE ROOMS AND JUDGES button. " : Exit Sub
            If RoomJudgeCheck(ds, "Room", "Timeslot") <> "OK" Then frmMainMenu.lblSetupStatus.Text = "Judges do not appear to be initialized for event/division eligibility.  Go to the SET UP ROUND SCHEDULE screen and click on the INITIALIZE ROOMS AND JUDGES button. " : Exit Sub
        End If

        'REFERENTIAL INTEGRITY CHECKS -- won't these fire on load?
        'Schools and students for ENTRY
        'schools for ENTRY_STUDENT
        'events for EVENT_SETTING
        'Judge, panel, and entry for BALLOT
        'ballot and recipient for BALLOT_SCRE
        'score for SCORE_SETTING
        'Event for TIEBREAK
        'School for JUDGE
        'Event, timeslot, TB_SET for ROUND
        'Round for PANEL
        'Event, round, entry, for ELIMSEED

    End Sub
    Function RoomJudgeCheck(ByVal ds As DataSet, ByVal strTable As String, ByVal strHeader As String) As String
        RoomJudgeCheck = "OK"
        Dim DoneAlready As Boolean = False
        For x = 0 To ds.Tables(strTable).Columns.Count - 1
            If InStr(ds.Tables(strTable).Columns(x).ColumnName.ToUpper, strHeader.ToUpper) Then DoneAlready = True
        Next x
        If DoneAlready = False Then
            RoomJudgeCheck = strTable & " do not appear to be initialized for " & strHeader
        End If
    End Function
    Sub CalcPrefAverages(ByRef ds)
        Dim str As String = ""
        str &= "start: " & Now.Second & " " & Now.Millisecond & Chr(10) & Chr(10)
        For x = 0 To ds.tables("Judge").rows.count - 1
            Try
                ds.tables("Judge").rows(x).item("AvgPref") = FormatNumber(ds.Tables("JudgePref").Compute("Avg(OrdPct)", "Judge = " & ds.tables("Judge").rows(x).item("ID") & "and rating<999 and rating<>333 and rating>0"), 1)
            Catch
                ds.tables("Judge").rows(x).item("AvgPref") = 99
            End Try
        Next x
        str &= "end: " & Now.Second & " " & Now.Millisecond & Chr(10) & Chr(10)
        'MsgBox(str)
    End Sub
    Function GetAvgPref(ByVal ds As DataSet, ByVal Judge As Integer) As Single
        Dim fdPrefs As DataRow()
        fdPrefs = ds.Tables("JudgePref").Select("Judge=" & Judge & " and rating>0")
        GetAvgPref = ds.Tables("JudgePref").Compute("Avg(Rating)", "Judge = " & Judge)
    End Function
    Sub EventSettingsCheck(ByRef DS As DataSet)
        'scroll through each division, and make sure that it has all the settings
        'if not, add a row with the setting and set default value

        Dim tag(12, 2) As String 'tag, default value
        tag(1, 1) = "DebatersPerTeam" : tag(2, 1) = "Level" : tag(3, 1) = "nPrelims"
        tag(4, 1) = "nElims" : tag(5, 1) = "SideDesignations" : tag(6, 1) = "nPresets"
        tag(7, 1) = "SideMethod" : tag(8, 1) = "TeamsPerRound" : tag(9, 1) = "UseRegions"
        tag(10, 1) = "AllowSameSchoolDebates" : tag(11, 1) = "Allow2dMeeting" : tag(12, 1) = "PanelDecisions"
        Dim foundrow As DataRow() : Dim dr As DataRow
        Dim x, y As Integer

        'scroll through all the events
        Dim fdRds As DataRow()
        For x = 0 To DS.Tables("EVENT").Rows.Count - 1
            If DS.Tables("EVENT").Rows(x).Item("TYPE").toupper = "DEBATE" Then DS.Tables("EVENT").Rows(x).Item("TYPE") = "Policy"
            If DS.Tables("EVENT").Rows(x).Item("TYPE").toupper = "LD" Then DS.Tables("EVENT").Rows(x).Item("TYPE") = "LINCOLN-DOUGLAS"
            fdRds = DS.Tables("Round").Select("Event=" & DS.Tables("Event").Rows(x).Item("ID") & " and PAIRINGSCHEME='Elim'")
            'set defaults by debate type
            If DS.Tables("EVENT").Rows(x).Item("TYPE").toupper = "POLICY" Then
                tag(1, 2) = "2" : tag(2, 2) = "Open" : tag(3, 2) = "6"
                tag(4, 2) = fdRds.Length : tag(5, 2) = "Aff/Neg" : tag(6, 2) = 3
                tag(7, 2) = "EqualizePrelims" : tag(8, 2) = 2 : tag(9, 2) = "false"
                tag(10, 2) = "false" : tag(11, 2) = "false" : tag(12, 2) = "false"
            ElseIf DS.Tables("EVENT").Rows(x).Item("TYPE").TOUPPER = "PARLI" Then
                tag(1, 2) = "2" : tag(2, 2) = "Open" : tag(3, 2) = "6"
                tag(4, 2) = fdRds.Length : tag(5, 2) = "Gov/Opp" : tag(6, 2) = 3
                tag(7, 2) = "EqualizePrelims" : tag(8, 2) = 2 : tag(9, 2) = "false"
                tag(10, 2) = "false" : tag(11, 2) = "false" : tag(12, 2) = "false"
            ElseIf DS.Tables("EVENT").Rows(x).Item("TYPE").TOUPPER = "LINCOLN-DOUGLAS" Then
                tag(1, 2) = "1" : tag(2, 2) = "Open" : tag(3, 2) = "6"
                tag(4, 2) = fdRds.Length : tag(5, 2) = "Aff/Neg" : tag(6, 2) = 3
                tag(7, 2) = "EqualizePrelims" : tag(8, 2) = 2 : tag(9, 2) = "false"
                tag(10, 2) = "false" : tag(11, 2) = "false" : tag(12, 2) = "false"
            ElseIf DS.Tables("EVENT").Rows(x).Item("TYPE").TOUPPER = "WUDC" Then
                tag(1, 2) = "2" : tag(2, 2) = "Open" : tag(3, 2) = "6"
                tag(4, 2) = fdRds.Length : tag(5, 2) = "WUDC" : tag(6, 2) = 3
                tag(7, 2) = "EqualizePrelims" : tag(8, 2) = 4 : tag(9, 2) = "false"
                tag(10, 2) = "true" : tag(11, 2) = "false" : tag(12, 2) = "true"
            End If
            'scroll through all the possible tags, add them if they aren't there
            For y = 1 To 12
                foundrow = DS.Tables("EVENT_SETTING").Select("EVENT=" & DS.Tables("EVENT").Rows(x).Item("ID") & " and Tag='" & tag(y, 1) & "'")
                If foundrow.Length = 0 Then
                    dr = DS.Tables("EVENT_SETTING").NewRow()
                    dr.Item("Event") = DS.Tables("EVENT").Rows(x).Item("ID")
                    dr.Item("TAG") = tag(y, 1) : dr.Item("Value") = tag(y, 2)
                    DS.Tables("EVENT_SETTING").Rows.Add(dr)
                End If
            Next y
        Next x
        ds.Tables("EVENT_SETTING").AcceptChanges()
    End Sub
    Sub ResultsTables(ByVal ds2 As DataSet, ByRef ds As DataSet)
        'Reads ROUNDRESULTS table and converts it to the PANEL, BALLOT, and BALLOT_SCORE tables
        'MUST have rounds set up already; this will find the round by the timeslot stored in SORTORDER field

        'go through each ballot; 1. add the panel if it doesn't exist already
        '2. add the judge to the panel
        '3. if recipient is a team and isn't on the panel, add the team to the panel
        '4. do ballot scores

        Dim fdPanel, fdRd, fdTeam, fdScore, fdBallot As DataRow() : Dim drRd, drTeam As DataRow
        Dim dr As DataRow : Dim Round, panel, nTeam, side, Team As Integer
        For x = 0 To ds2.Tables("Result_Ballot").Rows.Count - 1
            'count the number of teams on the ballot; the defalut if no SIDE is included
            nTeam = 0

            'ADD THE PANEL IF IT DOESN'T ALREADY EXIST
            fdPanel = ds.Tables("Panel").Select("ID=" & ds2.Tables("Result_Ballot").Rows(x).Item("Panel"))
            If fdPanel.Length = 0 Then
                'Pull the round info to get the Round ID, and add it
                drRd = ds2.Tables("RoundResult").Rows.Find(ds2.Tables("Result_Ballot").Rows(x).Item("RoundResult_Id"))
                'fdRd = ds.Tables("Round").Select("Event=" & drRd.Item("EventID") & " and timeslot=" & drRd.Item("SortOrder"))
                fdRd = ds.Tables("Round").Select("ID=" & drRd.Item("RoundID"))
                If fdRd.Length > 0 Then Round = fdRd(0).Item("ID")
                ds.EnforceConstraints = False
                panel = AddPanel(ds, Round, ds2.Tables("Result_Ballot").Rows(x).Item("Panel"))
                ds.EnforceConstraints = True
                'check to see if a room is present, if so, add it
                If ds2.Tables("Result_Ballot").Columns.Contains("RoomID") = True Then
                    dr = ds.Tables("Panel").Rows.Find(panel)
                    dr.Item("Room") = ds2.Tables("Result_Ballot").Rows(x).Item("RoomID")
                End If
            End If

            'ADD TEAMS to the panel; gotta scroll through the scores to do it
            fdScore = ds2.Tables("Result_Score").Select("Result_Ballot_ID=" & ds2.Tables("Result_Ballot").Rows(x).Item("Result_Ballot_Id"))
            For y = 0 To fdScore.Length - 1
                If fdScore(y).Item("ScoreFor").toupper = "TEAM" Then
                    fdTeam = ds.Tables("Ballot").Select("Panel=" & ds2.Tables("Result_Ballot").Rows(x).Item("Panel") & " and Entry=" & fdScore(y).Item("Recipient"))
                    If fdTeam.Length = 0 Then
                        nTeam += 1
                        side = nTeam
                        If Not fdScore(y).Item("Side") Is Nothing Then
                            side = fdScore(y).Item("Side")
                        End If
                        Call AddTeamToPanel(ds, ds2.Tables("Result_Ballot").Rows(x).Item("Panel"), fdScore(y).Item("Recipient"), side)
                    End If
                End If
            Next y

            'ADD JUDGE TO THE PANEL
            'Doesn't check to see if the judge is already on the panel, that is assumed to have been handled when the
            'results file was created
            If ds2.Tables("Result_Ballot").Rows(x).Item("Flight") Is System.DBNull.Value Then ds2.Tables("Result_Ballot").Rows(x).Item("Flight") = 1
            AddJudgeToPanel(ds, ds2.Tables("Result_Ballot").Rows(x).Item("Panel"), ds2.Tables("Result_Ballot").Rows(x).Item("JudgeID"), ds2.Tables("Result_Ballot").Rows(x).Item("Flight"))

            'Now ADD SCORES
            For y = 0 To fdScore.Length - 1
                dr = ds.Tables("Ballot_Score").NewRow
                Team = fdScore(y).Item("Recipient")
                If fdScore(y).Item("ScoreFor").toUpper <> "TEAM" Then
                    drTeam = ds.Tables("Entry_Student").Rows.Find(fdScore(y).Item("Recipient"))
                    Team = drTeam.Item("Entry")
                End If
                fdBallot = ds.Tables("Ballot").Select("Panel=" & ds2.Tables("Result_Ballot").Rows(x).Item("Panel") & " and Judge=" & ds2.Tables("Result_Ballot").Rows(x).Item("JudgeID") & " and Entry=" & Team)
                dr.Item("Ballot") = fdBallot(0).Item("ID")
                dr.Item("Recipient") = fdScore(y).Item("Recipient")
                dr.Item("Score_ID") = GetScoreID(fdScore(y).Item("Score_Name"), fdScore(y).Item("ScoreFor"))
                dr.Item("Score") = fdScore(y).Item("Result_Score_Text")
                ds.Tables("Ballot_Score").Rows.Add(dr)
            Next y
        Next x

        'delete roundresults table
        For x = ds.Tables("Result_Score").Constraints.Count - 1 To 0 Step -1
            ds.Tables("Result_Score").Constraints.Remove(ds.Tables("Result_Score").Constraints(x))
        Next
        For x = ds.Tables("Result_ballot").Constraints.Count - 1 To 0 Step -1
            Try
                ds.Tables("Result_ballot").Constraints.Remove(ds.Tables("Result_ballot").Constraints(x))
            Catch
            End Try
        Next
        For x = ds.Relations.Count - 1 To 0 Step -1
            If ds.Relations(x).ParentTable.TableName = "ROUNDRESULT" Then ds.Relations.Remove(ds.Relations(x))
            If ds.Relations(x).ChildTable.TableName = "ROUNDRESULT" Then ds.Relations.Remove(ds.Relations(x))
            If ds.Relations(x).ParentTable.TableName = "RESULT_SCORE" Then ds.Relations.Remove(ds.Relations(x))
            If ds.Relations(x).ChildTable.TableName = "RESULT_SCORE" Then ds.Relations.Remove(ds.Relations(x))
            If ds.Relations(x).ParentTable.TableName = "RESULT_BALLOT" Then ds.Relations.Remove(ds.Relations(x))
            If ds.Relations(x).ChildTable.TableName = "RESULT_BALLOT" Then ds.Relations.Remove(ds.Relations(x))
        Next x
        If ds.Tables.IndexOf("Result_Score") <> -1 Then ds.Tables.Remove("Result_Score")
        If ds.Tables.IndexOf("Result_Ballot") <> -1 Then ds.Tables.Remove("Result_Ballot")
        If ds.Tables.IndexOf("ROUNDRESULT") <> -1 Then ds.Tables.Remove("RoundResult")
        Call SaveFile(ds)
    End Sub
    Function GetScoreID(ByVal strScName As String, ByVal strScoreFor As String) As Integer
        If strScoreFor.ToUpper = "SPEAKER" Then
            GetScoreID = 2
            If InStr(strScName.Trim.ToUpper, "RANK") Then GetScoreID = 3
            If InStr(strScName.Trim.ToUpper, "RK") Then GetScoreID = 3
        Else
            GetScoreID = 1
            If InStr(strScName.Trim.ToUpper, "RANK") Then GetScoreID = 5
            If InStr(strScName.Trim.ToUpper, "RK") Then GetScoreID = 5
            If InStr(strScName.Trim.ToUpper, "POINT") Then GetScoreID = 4
            If InStr(strScName.Trim.ToUpper, "PTS") Then GetScoreID = 4
        End If
    End Function
    Sub DSAutoCalc(ByRef ds As DataSet, ByVal team As Integer)

        'Find total rounds in the pool
        'OBSOLETE: Doesn't account for precluded judges
        'Dim totRds As Integer = ds.Tables("Judge").Compute("Sum(Obligation)", "")
        'totRds += ds.Tables("Judge").Compute("Sum(Hired)", "")

        'pull ratings for team
        Dim fdPref As DataRow()
        fdPref = ds.Tables("JudgePref").Select("Team=" & team, "Rating ASC")

        'add up total
        Dim totRds As Integer = 0 : Dim Dr As DataRow
        For x = 0 To fdPref.Length - 1
            If fdPref(x).Item("Rating") <> 999 Then
                Dr = ds.Tables("Judge").Rows.Find(fdPref(x).Item("Judge"))
                totRds += Dr.Item("Obligation")
                totRds += Dr.Item("Hired")
            End If
        Next

        'calculate
        Dim ctr As Integer = 1 : Dim numerator As Integer = 1
        For x = 0 To fdPref.Length - 1
            fdPref(x).Item("OrdPct") = FormatNumber((numerator / totRds) * 100, 1)
            ctr += fdPref(x).Item("Rounds")
            If x < fdPref.Length - 1 Then If fdPref(x).Item("Rating") <> fdPref(x + 1).Item("Rating") Then numerator = ctr
            If fdPref(x).Item("Rating") = 999 Then fdPref(x).Item("OrdPct") = 999
        Next x

    End Sub
    Sub UnaffiliatedCheck(ByRef ds As DataSet)
        Dim dr, dr2 As DataRow
        'chek to see if there's an unaffilated school record
        dr = ds.Tables("School").Rows.Find(-1)
        'if not, add one
        If dr Is Nothing Then
            ds.EnforceConstraints = False
            dr2 = ds.Tables("School").NewRow
            dr2.Item("ID") = -1
            dr2.Item("DownloadRecord") = 0
            dr2.Item("Region") = 0
            dr2.Item("Code") = "Unaffil"
            dr2.Item("SchoolName") = "Unaffiliated/No school"
            ds.Tables("School").Rows.Add(dr2)
            ds.EnforceConstraints = True
            dr = ds.Tables("School").Rows.Find(-1) 'now load it in
        End If
        'change all judges with no school affiliation to the unaffiliated school
        Dim drSchool As DataRow
        For x = 0 To ds.Tables("Judge").Rows.Count - 1
            If ds.Tables("Judge").Rows(x).Item("School") Is System.DBNull.Value Then ds.Tables("Judge").Rows(x).Item("School") = dr.Item("ID") '-99
            If ds.Tables("Judge").Rows(x).Item("School") <= 0 Then ds.Tables("Judge").Rows(x).Item("School") = dr.Item("ID")
            If ds.Tables("Judge").Rows(x).Item("School") <> dr.Item("ID") Then
                drSchool = ds.Tables("School").Rows.Find(ds.Tables("Judge").Rows(x).Item("School"))
                If drSchool Is Nothing Then ds.Tables("Judge").Rows(x).Item("School") = dr.Item("ID")
            End If
        Next x
    End Sub
    Sub PrefInit(ByRef ds As DataSet)
        Dim hiRating, hiRow As Integer
        Dim dr As DataRow() : Dim newDr As DataRow
        For x = 0 To ds.Tables("Entry").Rows.Count - 1
            For y = 0 To ds.Tables("Judge").Rows.Count - 1
                dr = ds.Tables("JudgePref").Select("Judge=" & ds.Tables("Judge").Rows(y).Item("ID") & " and Team=" & ds.Tables("Entry").Rows(x).Item("ID"))
                For z = 0 To dr.Length - 1
                    If dr(z).Item("Rating") = 999 Then dr(z).Item("OrdPct") = 999
                Next z
                'delete duplicates; keep the highest
                If dr.Length > 1 Then
                    hiRating = 0 : hiRow = 0
                    For z = 0 To dr.Length - 1
                        If dr(z).Item("Rating") = 999 Then dr(z).Item("OrdPct") = 999
                        If dr(z).Item("Rating") > hiRating Then hiRating = dr(z).Item("Rating") : hiRow = z
                    Next z
                    For z = dr.Length - 1 To 0 Step -1
                        If z <> hiRow Then dr(z).Delete()
                    Next z
                End If
                'make sure there is a rating for every judge
                If dr.Length = 0 Then
                    newDr = ds.Tables("JudgePref").NewRow
                    newDr.Item("Judge") = ds.Tables("Judge").Rows(y).Item("ID")
                    newDr.Item("Team") = ds.Tables("Entry").Rows(x).Item("ID")
                    newDr.Item("Rating") = 0 : newDr.Item("Ordpct") = 0
                    If JudgeTeamSameSchool(ds, newDr.Item("Judge"), newDr.Item("Team")) = True Then
                        newDr.Item("Rating") = 999 : newDr.Item("Ordpct") = 999
                    End If
                    ds.Tables("JudgePref").Rows.Add(newDr)
                End If
            Next y
        Next x
        ds.Tables("JudgePref").AcceptChanges()
    End Sub
    Sub DivisionBlocker(ByVal ds As DataSet)
        If ds.Tables("Event").Rows.Count = 1 Then Exit Sub
        For x = 0 To ds.Tables("Event").Rows.Count - 1
            For y = 0 To ds.Tables("Judge").Rows.Count - 1
                If ds.Tables("Judge").Rows(y).Item("Event" & ds.Tables("Event").Rows(x).Item("ID")) Is System.DBNull.Value Then ds.Tables("Judge").Rows(y).Item("Event" & ds.Tables("Event").Rows(x).Item("ID")) = False
                If ds.Tables("Judge").Rows(y).Item("Event" & ds.Tables("Event").Rows(x).Item("ID")) = False Then
                    Dim fdPref As DataRow() : Dim fdTeam As DataRow
                    fdPref = ds.Tables("JudgePref").Select("Judge=" & ds.Tables("Judge").Rows(y).Item("ID"))
                    For z = 0 To fdPref.Length - 1
                        fdTeam = ds.Tables("Entry").Rows.Find(fdPref(z).Item("Team"))
                        If fdTeam.Item("Event") = ds.Tables("Event").Rows(x).Item("ID") Then
                            fdPref(z).Item("Rating") = 999
                            fdPref(z).Item("OrdPct") = 999
                        End If
                    Next z
                End If
            Next y
        Next x
        ds.Tables("JudgePref").AcceptChanges()
    End Sub
    Sub InitializeSTA(ByVal ds As DataSet)
        Dim dr As DataRow
        If ds.Tables("Judge_Assign_Param").Rows.Count = 0 Then
            dr = ds.Tables("Judge_Assign_Param").NewRow
            dr.Item("Judge_again") = False
            dr.Item("num_cohorts") = 1
            dr.Item("breakpoint") = 3
            dr.Item("indiv_pref_weight") = 1.2
            dr.Item("panel_pref_weight") = 1
            dr.Item("indiv_diff_weight") = 0
            dr.Item("panel_diff_weight") = 0.8
            dr.Item("judge_pref_weight") = 2
            dr.Item("pref_target_above") = 50
            dr.Item("pref_target_break") = 40
            dr.Item("pref_target_below") = 70
            dr.Item("max_indiv_diff_above") = 35
            dr.Item("max_indiv_diff_break") = 30
            dr.Item("max_indiv_diff_below") = 50
            dr.Item("max_panel_diff_above") = 35
            dr.Item("max_panel_diff_break") = 30
            dr.Item("max_panel_diff_below") = 50
            dr.Item("max_lost_rounds") = 5
            dr.Item("timeslot") = ds.Tables("Timeslot").Rows(0).Item("ID")
            ds.Tables("Judge_Assign_Param").Rows.Add(dr)
            Exit Sub
        End If
        For x = 0 To ds.Tables("Judge_Assign_Param").Rows.Count - 1
            If ds.Tables("Judge_Assign_Param").Rows(x).Item("Judge_Again") Is System.DBNull.Value Then
                ds.Tables("Judge_Assign_Param").Rows(x).Item("Judge_Again") = False
            End If
            If ds.Tables("Judge_Assign_Param").Rows(x).Item("num_cohorts") Is System.DBNull.Value Then
                ds.Tables("Judge_Assign_Param").Rows(x).Item("num_cohorts") = 1
            End If
            If ds.Tables("Judge_Assign_Param").Rows(x).Item("breakpoint") Is System.DBNull.Value Then
                ds.Tables("Judge_Assign_Param").Rows(x).Item("breakpoint") = 3
            End If
            If ds.Tables("Judge_Assign_Param").Rows(x).Item("indiv_pref_weight") Is System.DBNull.Value Then
                ds.Tables("Judge_Assign_Param").Rows(x).Item("indiv_pref_weight") = 1.2
            End If
            If ds.Tables("Judge_Assign_Param").Rows(x).Item("panel_pref_weight") Is System.DBNull.Value Then
                ds.Tables("Judge_Assign_Param").Rows(x).Item("panel_pref_weight") = 1.0
            End If
            If ds.Tables("Judge_Assign_Param").Rows(x).Item("indiv_diff_weight") Is System.DBNull.Value Then
                ds.Tables("Judge_Assign_Param").Rows(x).Item("indiv_diff_weight") = 0.0
            End If
            If ds.Tables("Judge_Assign_Param").Rows(x).Item("panel_diff_weight") Is System.DBNull.Value Then
                ds.Tables("Judge_Assign_Param").Rows(x).Item("panel_diff_weight") = 0.8
            End If
            If ds.Tables("Judge_Assign_Param").Rows(x).Item("judge_pref_weight") Is System.DBNull.Value Then
                ds.Tables("Judge_Assign_Param").Rows(x).Item("judge_pref_weight") = 2
            End If
            If ds.Tables("Judge_Assign_Param").Rows(x).Item("indiv_pref_weight") Is System.DBNull.Value Then
                ds.Tables("Judge_Assign_Param").Rows(x).Item("indiv_pref_weight") = 1.2
            End If
            If ds.Tables("Judge_Assign_Param").Rows(x).Item("pref_target_above") Is System.DBNull.Value Then
                ds.Tables("Judge_Assign_Param").Rows(x).Item("pref_target_above") = 50
            End If
            If ds.Tables("Judge_Assign_Param").Rows(x).Item("pref_target_break") Is System.DBNull.Value Then
                ds.Tables("Judge_Assign_Param").Rows(x).Item("pref_target_break") = 40
            End If
            If ds.Tables("Judge_Assign_Param").Rows(x).Item("pref_target_below") Is System.DBNull.Value Then
                ds.Tables("Judge_Assign_Param").Rows(x).Item("pref_target_below") = 70
            End If
            If ds.Tables("Judge_Assign_Param").Rows(x).Item("max_indiv_diff_above") Is System.DBNull.Value Then
                ds.Tables("Judge_Assign_Param").Rows(x).Item("max_indiv_diff_above") = 35
            End If
            If ds.Tables("Judge_Assign_Param").Rows(x).Item("max_indiv_diff_break") Is System.DBNull.Value Then
                ds.Tables("Judge_Assign_Param").Rows(x).Item("max_indiv_diff_break") = 30
            End If
            If ds.Tables("Judge_Assign_Param").Rows(x).Item("max_indiv_diff_below") Is System.DBNull.Value Then
                ds.Tables("Judge_Assign_Param").Rows(x).Item("max_indiv_diff_below") = 50
            End If
            If ds.Tables("Judge_Assign_Param").Rows(x).Item("max_panel_diff_above") Is System.DBNull.Value Then
                ds.Tables("Judge_Assign_Param").Rows(x).Item("max_panel_diff_above") = 35
            End If
            If ds.Tables("Judge_Assign_Param").Rows(x).Item("max_panel_diff_break") Is System.DBNull.Value Then
                ds.Tables("Judge_Assign_Param").Rows(x).Item("max_panel_diff_break") = 30
            End If
            If ds.Tables("Judge_Assign_Param").Rows(x).Item("max_panel_diff_below") Is System.DBNull.Value Then
                ds.Tables("Judge_Assign_Param").Rows(x).Item("max_panel_diff_below") = 50
            End If
            If ds.Tables("Judge_Assign_Param").Rows(x).Item("max_lost_rounds") Is System.DBNull.Value Then
                ds.Tables("Judge_Assign_Param").Rows(x).Item("max_lost_rounds") = 5
            End If
        Next x
    End Sub
End Module
