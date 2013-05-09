Public Class frmWebInteraction
    Dim ds As New DataSet
    Private Sub frmWebInteraction_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Load
        LoadFile(ds, "TourneyData")
    End Sub

    Private Sub butSendFileToWeb_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butSendFileToWeb.Click
        'DEAD CODE -- only processes the zipped file
        Call RoundChecker()
        'create file
        Dim strTime As DateTime = Now
        'strTime &= "Creating file for export...Start: " & Now.Second
        'If radWholeFile.Checked = True Then
        'Call MakeTableWithResults(ds, "ALL")
        'Else
        'Call MakeTableWithResults(ds, "SHORT")
        'End If
        'strTime &= " End: " & Now.Second & " " & Now.Millisecond & Chr(10)
        'Label1.Text = strTime : Label1.Refresh()
        'compact it
        'strTime = "Compacting file...Start: " & Now.Second
        'Call ZipIt()
        'strTime &= " End: " & Now.Second & " " & Now.Millisecond & Chr(10)
        'Label1.Text &= strTime : Label1.Refresh()
        'post it
        Label1.Text = "Sending file to the web..." : Label1.Refresh()
        Call PostFileToWeb()
        'done
        Label1.Text = "COMPLETE.  File posted to website. Seconds elapsed: " & DateDiff("s", strTime, Now).ToString
    End Sub
    Sub RoundChecker()
        Call NullKiller(ds, "Judge")
        Dim dt As New DataTable
        dt.Columns.Add("Round", System.Type.GetType("System.String"))
        dt.Columns.Add("RoundConflicts", System.Type.GetType("System.String"))
        dt.Columns.Add("TimeSlotConflicts", System.Type.GetType("System.String"))
        Dim fdPanels As DataRow() : Dim dr As DataRow
        For x = 0 To ds.Tables("Round").Rows.Count - 1
            dr = dt.NewRow : dr.Item("Round") = ds.Tables("Round").Rows(x).Item("Label")
            fdPanels = ds.Tables("Panel").Select("Round=" & ds.Tables("Round").Rows(x).Item("ID"))
            If fdPanels.Length = 0 Then
                dr.Item("RoundConflicts") = "Not scheduled yet."
                dr.Item("TimeSlotConflicts") = "Not scheduled yet."
            Else
                dr.Item("RoundConflicts") = TimeSlotAudit(ds, ds.Tables("Round").Rows(x).Item("ID"))
                dr.Item("TimeSlotConflicts") = AuditRound(ds, ds.Tables("Round").Rows(x).Item("ID"))
            End If
            dt.Rows.Add(dr)
        Next
        dt.AcceptChanges()
        DataGridView1.DataSource = dt
    End Sub

    Private Sub butCheck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butCheck.Click
        Call RoundChecker()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim strInfo As String
        strInfo = "WARNING! As a best data management practice, you should download any ballots from the web BEFORE uploading the datafile." & Chr(10) & Chr(10)
        strInfo &= "If judges are not entering online ballots now (for example, if you have just posted the pairings and the debates are just starting), it is safe to procced.  If judges ARE entering ballots now (for example, the round is over and some judges have already started entering their ballots), exit this operation, download the ballots, and then return here.  " & Chr(10) & Chr(10)
        strInfo &= "(The software should not erase entered online ballots with blanks in the offline data file, but it is safer to download the ballots before proceeding with the upload.)" & Chr(10) & Chr(10)
        strInfo &= "Continue with the upload?"
        Dim q As Integer = MsgBox(strInfo, MsgBoxStyle.YesNo)
        If q = vbNo Then Exit Sub

        'check teh rounds
        Label1.Text = "Checking round..." : Label1.Refresh()
        Try
            Call RoundChecker()
        Catch
            MsgBox("Round checker didn't fire correctly; this is likely a bug and not a problem with the round.  Round will now post, but you might want to double-check it on the manual change screen.")
        End Try

        'create file
        Dim strTime As DateTime = Now
        'strTime &= "Creating file for export...Start: " & Now.Second
        'If radWholeFile.Checked = True Then
        'Call MakeTableWithResults(ds, "ALL")
        'Else
        'Call MakeTableWithResults(ds, "SHORT")
        'End If
        'strTime &= " End: " & Now.Second & " " & Now.Millisecond & Chr(10)
        'Label1.Text = strTime : Label1.Refresh()
        Label1.Text = "Stripping empty decision..." : Label1.Refresh()
        Call StripEmptyBallots(ds)
        'compact it
        'strTime = "Compacting file...Start: " & Now.Second
        Label1.Text = "Zipping file..." : Label1.Refresh()
        Call ZipIt()
        'Label1.Text = "Check, Strip, & zip time: " & DateDiff("s", strTime, Now).ToString : Label1.Refresh()
        'strTime &= " End: " & Now.Second & " " & Now.Millisecond & Chr(10)
        'Label1.Text &= strTime : Label1.Refresh()
        'post it
        Label1.Text = "Sending file to the web..." : Label1.Refresh()
        Call PostZipFileToWeb(chkNoPrefs.Checked)
        'done
        Label1.Text = "COMPLETE.  File posted to website. Seconds elapsed: " & DateDiff("s", strTime, Now).ToString
    End Sub

    Private Sub butMakeResults_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butMakeResults.Click
        'this is live and works, but needs to be changed to zip up and send 
        Call MakeTableWithResults(ds, "ALL")
        MsgBox("Done -- saved in CAT folder as TourneyDataResults.xml")
    End Sub
End Class