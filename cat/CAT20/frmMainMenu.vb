'TO DO LIST
'Reset dates and tournament name after you change it on the tournament settings page

Imports System.IO
Imports System.Net
Imports System.Xml
Imports System.Xml.Schema

Public Class frmMainMenu
    Dim DS As New DataSet

    Private Sub butBasicInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butBasicInfo.Click
        Dim strInfo As String
        strInfo = "FRIENDLY ADVICE:" & Chr(10) & Chr(10)
        strInfo &= "On most pages, there will be a button like this one that will have some tips for navigating the page.  The first time you use a page, it's probably a good idea to click on it to get oriented and figure out how to get around on the form." & Chr(10) & Chr(10)
        strInfo &= "UNDERSTANDING WHEN DATA SAVES AND FORM NAVIGATION:" & Chr(10) & Chr(10)
        strInfo &= "The buttons on this page will take you to other forms to complete your tabulation tasks." & Chr(10) & Chr(10)
        strInfo &= "You can only have ONE form open at a time; this will make sure all the information is saved correctly." & Chr(10) & Chr(10)
        strInfo &= "All data saves when you close the form you are working on and return to the main menu.  It will not save to the disk until you close the page." & Chr(10) & Chr(10)
        strInfo &= "DATA FILE LOCATION:" & Chr(10) & Chr(10)
        strInfo &= "The CAT will create a folder in your documents folder very cleverly called 'CAT.'  There is only one data file the CAT will use and it will be stored the CAT folder in your documents." & Chr(10) & Chr(10)
        strInfo &= "The file is called 'TourneyData.xml,' it is a text file, and .xml files are supposed to be human-readable, but be aware that any change you make might make the file unusable by the program." & Chr(10) & Chr(10)
        strInfo &= "You can copy and rename the file to back it up, naming it something like 'TourneyDataAfterRd1.'" & Chr(10) & Chr(10)
        strInfo &= "NOTES ON DATA GRIDS:" & Chr(10) & Chr(10)
        strInfo &= "Almost all the data will be displayed in data grids.  Generally, you select an entire row by clicking in the gray box to the far left of the row." & Chr(10) & Chr(10)
        strInfo &= "You can select an individual cell by clicking on it, and you can sort the columns by clicking on them.  The delete button will delete an entire row if the row has been selected." & Chr(10) & Chr(10)
        strInfo &= "You can add a row to a datagrid by clicking on the little asterisk to the far left of the last row on the grid and then entering the appropriate values." & Chr(10) & Chr(10)
        strInfo &= "There will be times, however, when specific circumstances mean these general rules won't apply (some grids won't allow adding rows, or some columns can't be sorted, etc.)" & Chr(10) & Chr(10)
        MsgBox(strInfo, , "Basic CAT 2.0 Information")
    End Sub
    Private Sub frmMainMenu_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'check only a single instance is running
        Call checkdupeinstances()
        'Check that the data directory exists the documents folder; if not, create it
        Call checkDirectory()
        'Check that the data file is in the documents folder; if not, get user to identify location
        If CheckFile() = False Then
            MsgBox("Can't find an existing data file in the right directory!  Use the GENERATE/LOAD DATAFILE button in the bottom-right to either download one from the internet, provide the location of the file on your computer, or create a blank file for manual entry.  NO OTHER function will work until you identify the data file.", MsgBoxStyle.OkOnly)
            Exit Sub
        Else
            'first check; generic try/catch
            Try
                Call LoadFile(DS, "TourneyData")
            Catch
                MsgBox("There was a problem loading the datafile.  Click the GENERATE/LOAD datafile to load another file or create a blank one.")
                DS.Dispose() : Exit Sub
            End Try
            'second check; make sure the tourney table loaded with 1 row
            If DS.Tables("Tourn").Rows.Count > 0 Then
                lblTourneyName.Text = DS.Tables("Tourn").Rows(0).Item("TournName").trim & " -- " & DS.Tables("Tourn").Rows(0).Item("StartDate").trim & " through " & DS.Tables("Tourn").Rows(0).Item("EndDate").trim
            Else
                MsgBox("Problem with the datafile; program is closing.  If you have tried the fix following a download, this process is normal, and closing and re-opening will work.")
                DS.Dispose() : Exit Sub
            End If
        End If
        'check if there's a roundresults table, and if so, convert it to the CAT2.0 results tables
        If DS.Tables.IndexOf("ROUNDRESULT") <> -1 Then Call ResultsTables(DS, DS)
        'do initialization checks
        Call InitializeTourneySettings(DS)
        'show the setup status
        If Not DS Is Nothing Then Call CheckSetupStatus()
        'close it...this form won't use it anymore
        DS.Dispose()
        'now display STA button if it's you
        If My.Settings.UserName = "jbruschke@fullerton.edu" Then butSTASynch.Visible = True
    End Sub
    Sub CheckSetupStatus()
        'check things and post the message
        Call SetupCompletionStatus(DS)
        'change the font depending on whether things are ready to go
        lblSetupStatus.ForeColor = Color.Black
        lblSetupStatus.Font = New Font(lblSetupStatus.Font, FontStyle.Regular)
        If lblSetupStatus.Text <> "Setup complete!  Ready to start pairing." Then
            lblSetupStatus.ForeColor = Color.Red
            lblSetupStatus.Font = New Font(lblSetupStatus.Font, FontStyle.Bold)
        End If
        lblSetupStatus.Refresh()
    End Sub
    Sub checkDirectory()
        Dim j As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        Dim dDir As New DirectoryInfo(j & "\CAT")
        If dDir.Exists = False Then
            Directory.CreateDirectory(j & "\CAT")
        End If
    End Sub
    Function CheckFile() As Boolean
        'See if the data file exists.  If so, set the global variable strFilePath to its value.  If not, look for a 
        'user-defined location and repeat.  If it is in neither location, return a value of false.
        CheckFile = True
        'First, check for default location of file.  If it's there, exit
        Dim j As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\CAT\TourneyData.xml"
        Dim fFile As New FileInfo(j)
        If fFile.Exists Then
            CheckFile = True : strFilePath = j
            lblFileLocation.Text = "Current data file is located at " & strFilePath
            Exit Function
        End If
        'If not, see if the user has defined another location.  If so, try to open it.
        j = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\CAT\FileLocation.txt"
        fFile = New FileInfo(j)
        'exit if no user-specified location
        If Not fFile.Exists Then
            CheckFile = False
            lblFileLocation.Text = "NO CURRENT DATA FILE!!!! Identify one using the buttons below."
            Exit Function
        End If
        'There is a user-specified location, so check to see if the file exists
        Dim strFileLoc As String
        Dim reader As StreamReader = File.OpenText(j)
        strFileLoc = reader.ReadLine()
        reader.Close()
        fFile = New FileInfo(strFileLoc)
        If Not fFile.Exists Then
            CheckFile = False
            lblFileLocation.Text = "NO CURRENT DATA FILE!!!! Identify one using the buttons below."
        Else
            strFilePath = strFileLoc
            lblFileLocation.Text = "Current data file is located at " & strFilePath
        End If
    End Function

    Private Sub butTourneySetup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butTourneySetup.Click
        Call FormsOpen(frmTourneySettings)
    End Sub

    Private Sub butDivisionSetup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butDivisionSetup.Click
        Call FormsOpen(frmEnterDivisions)
    End Sub
    Sub FormsOpen(ByVal frmName As Form)
        Dim frm As Form : Dim ctr As Integer
        For Each frm In My.Application.OpenForms
            ctr += 1
        Next
        If ctr > 1 Then
            MsgBox("There is already one form open; you can only have one form open at a time.  Please check the systray at the bottom of the page and try again.")
            Exit Sub
        End If
        frmName.Show()
    End Sub


    Private Sub butDownload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butDownload.Click
        'define default location
        Dim j As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\CAT\TourneyData.xml"
        'check for existing file
        If CheckFile() = True Then
            Dim q As Integer = MsgBox("An exisiting data file has been identified!  If you continue the CAT will save the data file to " & j & ", delete any existing information there, and set it as the datafile location.  Click 'YES' to perform the download and 'NO' to exit without any changes.", MsgBoxStyle.YesNo)
            If q = vbNo Then Exit Sub
        End If
        'First the xsd file
        Try
            Dim URL As String
            URL = "http://www.tabroom.com/jbruschke/TourneyData.xsd"
            Dim request As HttpWebRequest = WebRequest.Create(URL)
            Dim response As HttpWebResponse = request.GetResponse()
            Dim reader As StreamReader = New StreamReader(response.GetResponseStream())
            Dim stw As StreamWriter = File.CreateText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\CAT\TourneyData.xsd")
            lblFileLocation.Text = "Download in progress...." : lblFileLocation.Refresh()
            stw.WriteLine(reader.ReadToEnd())
            reader.Close()
            stw.Close()
        Catch
            MsgBox("Formatting file faile to download , either because the computer is not connected to the internt or the tournament to download was identified incorrectly.  If the file is especially large, you may simply want to try the download again.  Please correct the problem and try again.")
            Exit Sub
        End Try

        'Download the data file
        Try
            Dim URL As String
            URL = "http://www.tabroom.com/jbruschke/TourneyData.xml"
            Dim request As HttpWebRequest = WebRequest.Create(URL)
            Dim response As HttpWebResponse = request.GetResponse()
            Dim reader As StreamReader = New StreamReader(response.GetResponseStream())
            Dim stw As StreamWriter = File.CreateText(j)
            lblFileLocation.Text = "Download in progress...." : lblFileLocation.Refresh()
            stw.WriteLine(reader.ReadToEnd())
            reader.Close()
            stw.Close()
            MsgBox("Download complete!  The CAT will now attempt to open the file; if the file location appears above the Tournament Setup column the download was successful.")
            strFilePath = j
            lblFileLocation.Text = "Current data file is located at " & strFilePath
        Catch
            MsgBox("The download failed, either because the computer is not connected to the internt or the tournament to download was identified incorrectly.  If the file is especially large, you may simply want to try the download again.  Please correct the problem and try again.")
            Exit Sub
        End Try

    End Sub
    Sub OpenDataFile()
        'Try
        'Call LoadFile(DS, "TourneyData")
        'If DS.Tables("Tourn").Rows.Count > 0 Then
        'lblTourneyName.Text = DS.Tables("Tourn").Rows(0).Item("TournName").trim & " -- " & DS.Tables("Tourn").Rows(0).Item("StartDate").trim & " through " & DS.Tables("Tourn").Rows(0).Item("EndDate").trim
        'End If
        'Catch
        'MsgBox("The file exists, but failed to load properly, probably due to an error in the formatting of the .xml document.  Return to the last known good backup.  Advanced users may attempt editing the .xml file.")
        'End Try
        'Catch
        'MsgBox("The file failed to open.  Check the format of the .xml file.")
        'Exit Sub
        'End Try
    End Sub
    Private Sub butEnterTeams_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butEnterTeams.Click
        Call FormsOpen(frmTeamEntry)
    End Sub
    Private Sub butUtilities_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butUtilities.Click
        Call FormsOpen(frmUtilities)
    End Sub
    Private Sub butSetUpRounds_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butSetUpRounds.Click
        Call FormsOpen(frmRounds)
    End Sub
    Private Sub butManualChange_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butManualChange.Click
        Call FormsOpen(frmShowPairings)
    End Sub
    Private Sub butTieBreakers_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butTieBreakers.Click
        Call FormsOpen(frmTieBreakers)
    End Sub
    Private Sub butResults_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butResults.Click
        Call FormsOpen(frmResults)
    End Sub
    Private Sub butPairRound_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butPairRound.Click
        Call FormsOpen(frmPair)
    End Sub

    Private Sub butElims_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butElims.Click
        Call FormsOpen(frmElims)
    End Sub
    Private Sub butCards_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butCards.Click
        Call FormsOpen(frmCards)
    End Sub
    Private Sub butManualPair_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butManualPair.Click
        Call FormsOpen(frmManualPair)
    End Sub
    Private Sub butTRPCPair_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butTRPCPair.Click
        Call FormsOpen(frmTRPCPair)
    End Sub

    Private Sub butSchoolEntry_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butSchoolEntry.Click
        Call FormsOpen(frmSchoolEntry)
    End Sub

    Private Sub butEnterJudges_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butEnterJudges.Click
        Call FormsOpen(frmJudgeEntry)
    End Sub

    Private Sub butJudgePref_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butJudgePref.Click
        Call FormsOpen(frmJudgePref)
    End Sub

    Private Sub butEnterRooms_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butEnterRooms.Click
        Call FormsOpen(frmRoomEntry)
    End Sub

    Private Sub butBallotEntry_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butBallotEntry.Click
        Call FormsOpen(frmBallotEntry)
    End Sub

    Private Sub but_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles but.Click
        Call FormsOpen(frmRegPrint)
    End Sub

    Private Sub butSetupHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butSetupHelp.Click
        Dim strInfo As String = ""
        strInfo &= "The current setup status will appear in the box at the bottom of this group of buttons, and should tell you which button to click next. " & Chr(10) & Chr(10)
        strInfo &= "Generally, you just need to click these 4 buttons in order from top to bottom." & Chr(10) & Chr(10)
        strInfo &= "Each screen will have its own help button, usually in the top-right." & Chr(10) & Chr(10)
        strInfo &= "When you are done, the message 'Setup complete!  Ready to start pairing.' will appear in the information box, and the text will appear in black rather than red." & Chr(10) & Chr(10)
        MsgBox(strInfo, , "How to set up the tournament")
    End Sub

    Private Sub butAssignJudges_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butAssignJudges.Click
        Call FormsOpen(frmSTAJudgePlacement)
    End Sub

    Private Sub butTeamBlocks_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butTeamBlocks.Click
        Call FormsOpen(frmteamblock)
    End Sub

    Private Sub butWebIt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butWebIt.Click
        Call FormsOpen(frmWebInteraction)
    End Sub

    Private Sub butFindFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butFindFile.Click
        FormsOpen(frmDataFileLoader)
    End Sub

    Private Sub butEnterPrefs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butEnterPrefs.Click
        FormsOpen(frmPreclusionsByJudge)
    End Sub

    Private Sub butPresetSmallDivision_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butPresetSmallDivision.Click
        FormsOpen(frmPresetSmallDivision)
    End Sub

    
    Private Sub butEnterSchools_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butEnterSchools.Click
        FormsOpen(frmEntriesBySchool)
    End Sub

    Private Sub butJudgeCards_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butJudgeCards.Click
        FormsOpen(frmJudgeCards)
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

    Private Sub butSTASynch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butSTASynch.Click
        Call AddSTAResults()
    End Sub
    Sub CheckDupeInstances()
        If Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1 Then
            MsgBox("Application XXXX already running. Only one instance of this application is allowed")
            Return
        End If
    End Sub
End Class
