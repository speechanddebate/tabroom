Imports System.IO
Imports System.Xml

Public Class frmTourneySettings
    Dim DS As New DataSet
    Private Sub frmTourneySettings_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Load
        LoadFile(DS, "TourneyData")
        'Check if file empty; if it is, populate it
        InitializeTourneySettings(DS)
        'format and bind name and date
        DataGridView1.AutoGenerateColumns = False
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DataGridView1.DataSource = DS.Tables("Tourn")
        'format and bind tourn_settings
        'settings include 'Initialized' 'SuppressNavMessages
        DataGridView2.AutoGenerateColumns = False
        DataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DataGridView2.DataSource = DS.Tables("Tourn_Setting")
        'hide the online columns
        DataGridView2.CurrentCell = Nothing
        For x = 0 To DataGridView2.RowCount - 1
            If InStr(DataGridView2.Rows(x).Cells("Tag").Value.ToString, "_") > 0 Then
                DataGridView2.Rows(x).Visible = False
            End If
        Next x
        'TimeSlot
        If GetTagValue(DS.Tables("TOURN_SETTING"), "UseActualTime") = True Then
            Call ShowTimeCols()
        Else
            Call HideTimeCols()
        End If
        dgvTimeSlots.AutoGenerateColumns = False
        dgvTimeSlots.DataSource = DS.Tables("TimeSlot")
    End Sub
    Private Sub frmTourneySettings_Unload(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Disposed
        Call SaveFile(DS)
        Call SetupCompletionStatus(DS)
        DS.Dispose()
        Try
            frmMainMenu.lblTourneyName.Text = DS.Tables("Tourn").Rows(0).Item("TournName").trim & " -- " & DS.Tables("Tourn").Rows(0).Item("StartDate").trim & " through " & DS.Tables("Tourn").Rows(0).Item("EndDate").trim
            frmMainMenu.lblTourneyName.Refresh()
        Catch
        End Try
    End Sub

    Private Sub DataGridView2_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView2.CellClick
        Dim valArray(20) As String
        If DataGridView2.CurrentRow.Cells(0).Value = "TourneyType" Then
            valArray(0) = "Policy" : valArray(1) = "Lincoln-Douglas" : valArray(2) = "Parli"
            valArray(3) = "WUDC" : valArray(4) = "Multi-Event"
            Call PopulateDetailGrid(valArray)
        End If
        If DataGridView2.CurrentRow.Cells(0).Value = "Online" Then
            valArray(0) = "All Online" : valArray(1) = "All Offline" : valArray(2) = "Track Users"
            Call PopulateDetailGrid(valArray)
        End If
        If DataGridView2.CurrentRow.Cells(0).Value = "DownloadSite" Then
            valArray(0) = "iDebate.org" : valArray(1) = "JoyOfTournaments.Com" : valArray(2) = "ForensicsTournament.net"
            valArray(3) = "Other"
            Call PopulateDetailGrid(valArray)
        End If
        If DataGridView2.CurrentRow.Cells(0).Value = "SuppressNavMessages" Then
            valArray(0) = "True" : valArray(1) = "False"
            Call PopulateDetailGrid(valArray)
        End If
        If DataGridView2.CurrentRow.Cells(0).Value = "UseActualTime" Then
            valArray(0) = "True" : valArray(1) = "False"
            Call PopulateDetailGrid(valArray)
        End If
        If DataGridView2.CurrentRow.Cells(0).Value = "CrossEventEntry" Then
            valArray(0) = "True" : valArray(1) = "False"
            Call PopulateDetailGrid(valArray)
        End If
    End Sub
    Sub PopulateDetailGrid(ByVal valarray() As String)
        Dim DT As New DataTable
        DT.Columns.Add("Value", System.Type.GetType("System.String"))
        Dim x As Integer : Dim dr As DataRow
        'fill it with array values
        For x = 0 To valarray.GetUpperBound(0)
            If valarray(x) <> "" Then
                dr = DT.NewRow
                dr.Item("Value") = valarray(x)
                DT.Rows.Add(dr)
            End If
        Next
        DT.AcceptChanges()
        dgvDetails.DataSource = DT
        'find the right one
        For x = 0 To dgvDetails.RowCount - 1
            dgvDetails.Rows(x).Selected = False
            If dgvDetails.Rows(x).Cells(0).Value.ToString.Trim = DataGridView2.CurrentRow.Cells(1).Value.ToString.Trim Then
                'dgvDetails.Rows(x).Selected = True
                dgvDetails.CurrentCell = dgvDetails.Rows(x).Cells(0)
            End If
        Next
    End Sub

    Private Sub dgvDetails_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvDetails.CellClick
        DataGridView2.CurrentRow.Cells(1).Value = dgvDetails.CurrentRow.Cells(0).Value
    End Sub
    Sub dgv2_Click() Handles DataGridView2.MouseClick
        Call SettingsHelpDisplay()
    End Sub
    Sub dgv2_RowChange() Handles DataGridView2.SelectionChanged
        Call SettingsHelpDisplay()
    End Sub
    Sub SettingsHelpDisplay()
        If DataGridView2.CurrentRow.Cells("Tag").Value.toupper = "DOWNLOADSITE" Then
            Label1.Text = "This setting identifies which site the tournament data was downloaded from.  This information is necessary to keep track of results (like cume sheets) and records (like NFL and GSDHS points) that are posted on the internet.  If the site you used doesn't appear, or if you didn't use a website for tournament registration, select 'Other.'"
        ElseIf DataGridView2.CurrentRow.Cells("Tag").Value.toupper = "ONLINE" Then
            Label1.Text = "The OFFLINE option is for paper-only tournaments that did not use an online site for tournament registration and will only use the CAT to tab the tournament and submit results." & Chr(10) & Chr(10)
            Label1.Text &= "The ONLINE option is ONLY for tournaments that used the iDebate.org site for registration and wish to use all the online options, such as online balloting and posting of pairings." & Chr(10) & Chr(10)
            Label1.Text &= "Select TRACK USERS if you want to run the tournament offline but match up name changes after your results are in and before you create the results files." & Chr(10) & Chr(10)
            Label1.Text &= "For more information, click on the help button in the top-right."
        ElseIf DataGridView2.CurrentRow.Cells("Tag").Value.toupper = "TOURNEYTYPE" Then
            Label1.Text = "Identify the type of debate that will run out of this computer.  Select 'WUDC' for World and 4-team-per-round events.  If you will run different types of debate in different events, select 'Multi-Event'."
        ElseIf DataGridView2.CurrentRow.Cells("Tag").Value.toupper = "SUPPRESSNAVMESSAGES" Then
            Label1.Text = "A number of the screens will produce messages as you click on the grids to help you navigate the screens.  These can be helpful the first time the pages are used, but may slow down advanced users.  Change this setting to TRUE to suppress the navigation messages."
        ElseIf DataGridView2.CurrentRow.Cells("Tag").Value.toupper = "USEACTUALTIME" Then
            Label1.Text = "In the TRPC and original CAT, TimeSlots were not associated with specific times.  If you change this setting to TRUE, judge availability will be based on actual time values and not TimeSlots.  For example, if you enter that a judge will be unavailble from 2-5pm, the computer will autoatically track which rounds they will then be available to hear.  Once set to true, you will need to enter start and end times for each TimeSlot.  Unless you are an advanced user, you probably want to set this value to FALSE."
        ElseIf DataGridView2.CurrentRow.Cells("Tag").Value.toupper = "CROSSEVENTENTRY" Then
            Label1.Text = "This setting identifies whether a competitor can enter multiple events that are being run out of this computer.  If set to TRUE, the pairing functions will make sure a competitor does not appear twice in the same TimeSlot.  If set to FALSE, the computer will check to make sure that no competitor is entered in multiple events."
        End If
    End Sub

    Private Sub butHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butHelp.Click
        Dim strMessage As String
        strMessage = "This page sets up tournament-wide settings and should be the first setup screen that you use." & Chr(10) & Chr(10)
        strMessage &= "There are 3 groups of settings that you will need to enter.  Above each group of settings are instructions for how to enter hte informaiton." & Chr(10) & Chr(10)
        strMessage &= "The FIRST group of settings is simply the tournament name and dates." & Chr(10) & Chr(10)
        strMessage &= "The SECOND group of settings establishes how the CAT will handle things during the tournament." & Chr(10) & Chr(10)
        'strMessage &= "The most important of these settings is the ONLINE mode.  A major advantage of the CAT is its interface with the iDebate.org website.  "
        'strMessage &= "Regardless of whether you run the tournament online or offline, when the tournament is over you can submit your results to the iDebate.org site.  That site makes your results publicly available, removes the need to post the cume sheets other places, and autmoatically submits the results for Global Speech and Debate Honor Society points." & Chr(10) & Chr(10)
        'strMessage &= "Submitting results requires that the judge and competitor names used at your tournaments are matched to the iDebate database.  If you used an online site for registration, there will be a file called 'tourneydatamaster' stored in the CAT folder in your documents library.  "
        'strMessage &= "The TRACK USERS option will keep track of any name changes you make during the tournament, and when you create the results files to upload to iDebate you will be able to match the names you changed to those in the database.  Doing so will ensure the accuracy of your results and avoid post-tournament follow-up corrections." & Chr(10) & Chr(10)
        strMessage &= "The THIRD group of settings identifies how many TimeSlots the tournaments will use.  At a minimum, you need enough TimeSlots for every preliminary and elimination round you have.  On the rounds setup screen you can identify which events occur during which TimeSlots, and on this page you only need to identify the total number of time slots necessary to complete the tournament."
        MsgBox(strMessage)
    End Sub


    Private Sub butMakeTimeSlots_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butMakeTimeSlots.Click
        Dim q As Integer
        If DS.Tables.IndexOf("TimeSlot") > -1 Then
            If DS.Tables("TimeSlot").Rows.Count > 0 Then
                q = MsgBox("TimeSlots appear to have already been set.  Continue anyway?", MsgBoxStyle.YesNo)
                If q = vbNo Then Exit Sub
            End If
        End If
        q = InputBox("Enter the total number of time slots, which is calculated by adding together the maximum number of prelims you'll need to the maximum number of elims you'll need.  For example, if you have 6 prelims and 5 elims, you'll need 11 TimeSlots.  Enter total number of TimeSlots here:")
        DS.Tables("TimeSlot").Clear()
        Call InitializeTimeSlots(DS, q)

    End Sub
    Sub ShowTimeCols()
        Label2.Text = "Enter a date followed by a standard time format, for example, '3/31/1969 10AM'.  To add a row type a new entry in the last row with the asterisk by it."
        dgvTimeSlots.Columns("stTime").Visible = True
        dgvTimeSlots.Columns("EndTime").Visible = True
    End Sub
    Sub HideTimeCols()
        Label2.Text = "You can change the name of a time slot simply by re-typing it.  If you want to set specific times for the TimeSlots, change the UseActualTime setting to TRUE using the box above."
        dgvTimeSlots.Columns("stTime").Visible = False
        dgvTimeSlots.Columns("EndTime").Visible = False
    End Sub
    Sub CheckTimeSettingChange() Handles dgvDetails.CurrentCellChanged
        If DataGridView2.CurrentRow.Cells("Tag").Value.ToString.Trim.ToUpper <> "USEACTUALTIME" Then Exit Sub
        If dgvDetails.CurrentRow Is Nothing Then Exit Sub
        If dgvDetails.CurrentRow.Cells("Column1").Value Is Nothing Then Exit Sub
        If dgvDetails.CurrentRow.Cells("Column1").Value.ToString.Trim.ToUpper = "FALSE" Then Call HideTimeCols() : Exit Sub
        Call ShowTimeCols() : Exit Sub
    End Sub

    Private Sub butTimeSlotHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butTimeSlotHelp.Click
        Dim strInfo As String = ""
        strInfo &= "All rounds will occur during TimeSlots you specify; if you are running only 1 division each round will probably be its own TimeSlot." & Chr(10) & Chr(10)
        strInfo &= "However, if you are running multiple divisions, and especially if not all rounds are happening at the same time, you need to define how many differne time slots there are." & Chr(10) & Chr(10)
        strInfo &= "To generate a series of TimeSlots, click the GENERATE TIME SLOTS button.  You should generally only use this button at the begining of the tournament and before you have set up the rounds." & Chr(10) & Chr(10)
        strInfo &= "To ADD an individual TimeSlot, simply type its name in the bottom row." & Chr(10) & Chr(10)
        strInfo &= "To DELETE an individual TimeSlot, select it by clicking the gray box to the left of the grid and use the delete button on your keyboard." & Chr(10) & Chr(10)
        MsgBox(strInfo)
    End Sub
    Private Sub UserDeletingRow(ByVal sender As Object, ByVal e As DataGridViewRowCancelEventArgs) Handles dgvTimeSlots.UserDeletingRow
        Dim drRound As DataRow()
        drRound = DS.Tables("Round").Select("TimeSlot=" & dgvTimeSlots.CurrentRow.Cells("ID").Value)
        If drRound.Length = 0 Then Exit Sub
        Dim q As Integer
        q = MsgBox("WARNING!  Rounds have already been assigned to this TimeSlot.  Continuing with the delete will require the you visit the ROUNDS setup screen and re-enter appropriate TimeSlots!  Click OK to continue with the deletion or CANCEL to continue without deleting.", MsgBoxStyle.OkCancel)
        If q = vbCancel Then e.Cancel = True
    End Sub
End Class