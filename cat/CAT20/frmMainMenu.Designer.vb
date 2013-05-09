<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMainMenu
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMainMenu))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.butSetupHelp = New System.Windows.Forms.Button()
        Me.lblSetupStatus = New System.Windows.Forms.Label()
        Me.butTieBreakers = New System.Windows.Forms.Button()
        Me.butSetUpRounds = New System.Windows.Forms.Button()
        Me.butDivisionSetup = New System.Windows.Forms.Button()
        Me.butTourneySetup = New System.Windows.Forms.Button()
        Me.butFindFile = New System.Windows.Forms.Button()
        Me.butDownload = New System.Windows.Forms.Button()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.butTeamBlocks = New System.Windows.Forms.Button()
        Me.butJudgePref = New System.Windows.Forms.Button()
        Me.butSchoolEntry = New System.Windows.Forms.Button()
        Me.butEnterSchools = New System.Windows.Forms.Button()
        Me.butEnterPrefs = New System.Windows.Forms.Button()
        Me.butEnterTeams = New System.Windows.Forms.Button()
        Me.butEnterRooms = New System.Windows.Forms.Button()
        Me.butEnterJudges = New System.Windows.Forms.Button()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.butPresetSmallDivision = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.butBallotEntry = New System.Windows.Forms.Button()
        Me.butTRPCPair = New System.Windows.Forms.Button()
        Me.butManualPair = New System.Windows.Forms.Button()
        Me.butElims = New System.Windows.Forms.Button()
        Me.butAssignJudges = New System.Windows.Forms.Button()
        Me.butManualChange = New System.Windows.Forms.Button()
        Me.butRooms = New System.Windows.Forms.Button()
        Me.butPairRound = New System.Windows.Forms.Button()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.butJudgeCards = New System.Windows.Forms.Button()
        Me.butWebIt = New System.Windows.Forms.Button()
        Me.butCards = New System.Windows.Forms.Button()
        Me.butResults = New System.Windows.Forms.Button()
        Me.butRoundInfo = New System.Windows.Forms.Button()
        Me.but = New System.Windows.Forms.Button()
        Me.butBasicInfo = New System.Windows.Forms.Button()
        Me.lblFileLocation = New System.Windows.Forms.Label()
        Me.lblTourneyName = New System.Windows.Forms.Label()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.butSTASynch = New System.Windows.Forms.Button()
        Me.butBackUp = New System.Windows.Forms.Button()
        Me.butUtilities = New System.Windows.Forms.Button()
        Me.lblVersion = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Franklin Gothic Medium", 24.75!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(355, 10)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(570, 38)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "CAT 2.0 - Computer Assisted Tabulation"
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Franklin Gothic Medium", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(37, 55)
        Me.Label2.MaximumSize = New System.Drawing.Size(850, 36)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(841, 36)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "An open-source project by Gary Larson, Rich Edwards, and Jon Bruschke.   Supporte" & _
            "d by the Internationl Debate Education Association at www.iDebate.org"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.butSetupHelp)
        Me.GroupBox1.Controls.Add(Me.lblSetupStatus)
        Me.GroupBox1.Controls.Add(Me.butTieBreakers)
        Me.GroupBox1.Controls.Add(Me.butSetUpRounds)
        Me.GroupBox1.Controls.Add(Me.butDivisionSetup)
        Me.GroupBox1.Controls.Add(Me.butTourneySetup)
        Me.GroupBox1.Font = New System.Drawing.Font("Franklin Gothic Medium", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(10, 210)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(308, 520)
        Me.GroupBox1.TabIndex = 3
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "TOURNAMENT SETUP"
        '
        'butSetupHelp
        '
        Me.butSetupHelp.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.butSetupHelp.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butSetupHelp.Location = New System.Drawing.Point(6, 39)
        Me.butSetupHelp.Name = "butSetupHelp"
        Me.butSetupHelp.Size = New System.Drawing.Size(167, 35)
        Me.butSetupHelp.TabIndex = 8
        Me.butSetupHelp.Text = "How does this work?"
        Me.butSetupHelp.UseVisualStyleBackColor = False
        '
        'lblSetupStatus
        '
        Me.lblSetupStatus.AutoSize = True
        Me.lblSetupStatus.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSetupStatus.Location = New System.Drawing.Point(6, 251)
        Me.lblSetupStatus.MaximumSize = New System.Drawing.Size(290, 185)
        Me.lblSetupStatus.MinimumSize = New System.Drawing.Size(290, 185)
        Me.lblSetupStatus.Name = "lblSetupStatus"
        Me.lblSetupStatus.Size = New System.Drawing.Size(290, 185)
        Me.lblSetupStatus.TabIndex = 5
        Me.lblSetupStatus.Text = "Label3"
        '
        'butTieBreakers
        '
        Me.butTieBreakers.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butTieBreakers.Location = New System.Drawing.Point(5, 172)
        Me.butTieBreakers.Name = "butTieBreakers"
        Me.butTieBreakers.Size = New System.Drawing.Size(296, 35)
        Me.butTieBreakers.TabIndex = 4
        Me.butTieBreakers.Text = "STEP 3: Set up tiebreakers"
        Me.butTieBreakers.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.butTieBreakers.UseVisualStyleBackColor = True
        '
        'butSetUpRounds
        '
        Me.butSetUpRounds.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butSetUpRounds.Location = New System.Drawing.Point(6, 213)
        Me.butSetUpRounds.Name = "butSetUpRounds"
        Me.butSetUpRounds.Size = New System.Drawing.Size(296, 35)
        Me.butSetUpRounds.TabIndex = 3
        Me.butSetUpRounds.Text = "STEP 4: Set Up Round Schedule"
        Me.butSetUpRounds.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.butSetUpRounds.UseVisualStyleBackColor = True
        '
        'butDivisionSetup
        '
        Me.butDivisionSetup.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butDivisionSetup.Location = New System.Drawing.Point(5, 131)
        Me.butDivisionSetup.Name = "butDivisionSetup"
        Me.butDivisionSetup.Size = New System.Drawing.Size(296, 35)
        Me.butDivisionSetup.TabIndex = 2
        Me.butDivisionSetup.Text = "STEP 2: Set up divisions"
        Me.butDivisionSetup.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.butDivisionSetup.UseVisualStyleBackColor = True
        '
        'butTourneySetup
        '
        Me.butTourneySetup.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butTourneySetup.Location = New System.Drawing.Point(5, 88)
        Me.butTourneySetup.Name = "butTourneySetup"
        Me.butTourneySetup.Size = New System.Drawing.Size(296, 35)
        Me.butTourneySetup.TabIndex = 1
        Me.butTourneySetup.Text = "STEP 1: Enter tournament-wide settings"
        Me.butTourneySetup.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.butTourneySetup.UseVisualStyleBackColor = True
        '
        'butFindFile
        '
        Me.butFindFile.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butFindFile.Location = New System.Drawing.Point(6, 29)
        Me.butFindFile.Name = "butFindFile"
        Me.butFindFile.Size = New System.Drawing.Size(296, 50)
        Me.butFindFile.TabIndex = 4
        Me.butFindFile.Text = "Generate/load datafile"
        Me.butFindFile.UseVisualStyleBackColor = True
        '
        'butDownload
        '
        Me.butDownload.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butDownload.Location = New System.Drawing.Point(11, 189)
        Me.butDownload.Name = "butDownload"
        Me.butDownload.Size = New System.Drawing.Size(296, 24)
        Me.butDownload.TabIndex = 3
        Me.butDownload.Text = "Download a tournament from the internet"
        Me.butDownload.UseVisualStyleBackColor = True
        Me.butDownload.Visible = False
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.butTeamBlocks)
        Me.GroupBox2.Controls.Add(Me.butJudgePref)
        Me.GroupBox2.Controls.Add(Me.butSchoolEntry)
        Me.GroupBox2.Controls.Add(Me.butEnterSchools)
        Me.GroupBox2.Controls.Add(Me.butEnterPrefs)
        Me.GroupBox2.Controls.Add(Me.butEnterTeams)
        Me.GroupBox2.Controls.Add(Me.butEnterRooms)
        Me.GroupBox2.Controls.Add(Me.butEnterJudges)
        Me.GroupBox2.Font = New System.Drawing.Font("Franklin Gothic Medium", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.Location = New System.Drawing.Point(322, 210)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(308, 520)
        Me.GroupBox2.TabIndex = 4
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "ENTRY MANAGEMENT"
        '
        'butTeamBlocks
        '
        Me.butTeamBlocks.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butTeamBlocks.Location = New System.Drawing.Point(5, 403)
        Me.butTeamBlocks.Name = "butTeamBlocks"
        Me.butTeamBlocks.Size = New System.Drawing.Size(296, 45)
        Me.butTeamBlocks.TabIndex = 7
        Me.butTeamBlocks.Text = "Enter or edit team blocks"
        Me.butTeamBlocks.UseVisualStyleBackColor = True
        '
        'butJudgePref
        '
        Me.butJudgePref.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butJudgePref.Location = New System.Drawing.Point(6, 337)
        Me.butJudgePref.Name = "butJudgePref"
        Me.butJudgePref.Size = New System.Drawing.Size(296, 57)
        Me.butJudgePref.TabIndex = 6
        Me.butJudgePref.Text = "Enter or edit judge preference and conflict information"
        Me.butJudgePref.UseVisualStyleBackColor = True
        '
        'butSchoolEntry
        '
        Me.butSchoolEntry.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butSchoolEntry.Location = New System.Drawing.Point(6, 284)
        Me.butSchoolEntry.Name = "butSchoolEntry"
        Me.butSchoolEntry.Size = New System.Drawing.Size(296, 45)
        Me.butSchoolEntry.TabIndex = 5
        Me.butSchoolEntry.Text = "Enter or edit schools and change acronyms"
        Me.butSchoolEntry.UseVisualStyleBackColor = True
        '
        'butEnterSchools
        '
        Me.butEnterSchools.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butEnterSchools.Location = New System.Drawing.Point(6, 465)
        Me.butEnterSchools.Name = "butEnterSchools"
        Me.butEnterSchools.Size = New System.Drawing.Size(296, 29)
        Me.butEnterSchools.TabIndex = 4
        Me.butEnterSchools.Text = "View entries by school"
        Me.butEnterSchools.UseVisualStyleBackColor = True
        '
        'butEnterPrefs
        '
        Me.butEnterPrefs.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butEnterPrefs.Location = New System.Drawing.Point(5, 224)
        Me.butEnterPrefs.Name = "butEnterPrefs"
        Me.butEnterPrefs.Size = New System.Drawing.Size(296, 52)
        Me.butEnterPrefs.TabIndex = 3
        Me.butEnterPrefs.Text = "Enter conflicts by Judge"
        Me.butEnterPrefs.UseVisualStyleBackColor = True
        '
        'butEnterTeams
        '
        Me.butEnterTeams.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butEnterTeams.Location = New System.Drawing.Point(6, 39)
        Me.butEnterTeams.Name = "butEnterTeams"
        Me.butEnterTeams.Size = New System.Drawing.Size(296, 45)
        Me.butEnterTeams.TabIndex = 0
        Me.butEnterTeams.Text = "Enter or edit teams"
        Me.butEnterTeams.UseVisualStyleBackColor = True
        '
        'butEnterRooms
        '
        Me.butEnterRooms.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butEnterRooms.Location = New System.Drawing.Point(5, 162)
        Me.butEnterRooms.Name = "butEnterRooms"
        Me.butEnterRooms.Size = New System.Drawing.Size(296, 45)
        Me.butEnterRooms.TabIndex = 2
        Me.butEnterRooms.Text = "Enter or edit rooms"
        Me.butEnterRooms.UseVisualStyleBackColor = True
        '
        'butEnterJudges
        '
        Me.butEnterJudges.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butEnterJudges.Location = New System.Drawing.Point(6, 102)
        Me.butEnterJudges.Name = "butEnterJudges"
        Me.butEnterJudges.Size = New System.Drawing.Size(296, 45)
        Me.butEnterJudges.TabIndex = 1
        Me.butEnterJudges.Text = "Enter or edit judges"
        Me.butEnterJudges.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.butPresetSmallDivision)
        Me.GroupBox3.Controls.Add(Me.Label3)
        Me.GroupBox3.Controls.Add(Me.butBallotEntry)
        Me.GroupBox3.Controls.Add(Me.butTRPCPair)
        Me.GroupBox3.Controls.Add(Me.butManualPair)
        Me.GroupBox3.Controls.Add(Me.butElims)
        Me.GroupBox3.Controls.Add(Me.butAssignJudges)
        Me.GroupBox3.Controls.Add(Me.butManualChange)
        Me.GroupBox3.Controls.Add(Me.butRooms)
        Me.GroupBox3.Controls.Add(Me.butPairRound)
        Me.GroupBox3.Font = New System.Drawing.Font("Franklin Gothic Medium", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox3.Location = New System.Drawing.Point(635, 210)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(308, 520)
        Me.GroupBox3.TabIndex = 5
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "SCHEDULING"
        '
        'butPresetSmallDivision
        '
        Me.butPresetSmallDivision.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butPresetSmallDivision.Location = New System.Drawing.Point(6, 231)
        Me.butPresetSmallDivision.Name = "butPresetSmallDivision"
        Me.butPresetSmallDivision.Size = New System.Drawing.Size(296, 37)
        Me.butPresetSmallDivision.TabIndex = 10
        Me.butPresetSmallDivision.Text = "Preset a Small Division"
        Me.butPresetSmallDivision.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Franklin Gothic Medium", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(7, 351)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(283, 24)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "Buttons that don't do anything yet"
        Me.Label3.Visible = False
        '
        'butBallotEntry
        '
        Me.butBallotEntry.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butBallotEntry.Location = New System.Drawing.Point(5, 180)
        Me.butBallotEntry.Name = "butBallotEntry"
        Me.butBallotEntry.Size = New System.Drawing.Size(296, 45)
        Me.butBallotEntry.TabIndex = 8
        Me.butBallotEntry.Text = "Prelim Ballot Entry"
        Me.butBallotEntry.UseVisualStyleBackColor = True
        '
        'butTRPCPair
        '
        Me.butTRPCPair.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butTRPCPair.Location = New System.Drawing.Point(6, 465)
        Me.butTRPCPair.Name = "butTRPCPair"
        Me.butTRPCPair.Size = New System.Drawing.Size(296, 45)
        Me.butTRPCPair.TabIndex = 7
        Me.butTRPCPair.Text = "TRPC automatic pairing"
        Me.butTRPCPair.UseVisualStyleBackColor = True
        Me.butTRPCPair.Visible = False
        '
        'butManualPair
        '
        Me.butManualPair.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butManualPair.Location = New System.Drawing.Point(6, 32)
        Me.butManualPair.Name = "butManualPair"
        Me.butManualPair.Size = New System.Drawing.Size(296, 45)
        Me.butManualPair.TabIndex = 6
        Me.butManualPair.Text = "Pair a round manually"
        Me.butManualPair.UseVisualStyleBackColor = True
        '
        'butElims
        '
        Me.butElims.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butElims.Location = New System.Drawing.Point(6, 136)
        Me.butElims.Name = "butElims"
        Me.butElims.Size = New System.Drawing.Size(296, 38)
        Me.butElims.TabIndex = 5
        Me.butElims.Text = "Elims and Elim Ballots"
        Me.butElims.UseVisualStyleBackColor = True
        '
        'butAssignJudges
        '
        Me.butAssignJudges.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butAssignJudges.Location = New System.Drawing.Point(6, 277)
        Me.butAssignJudges.Name = "butAssignJudges"
        Me.butAssignJudges.Size = New System.Drawing.Size(296, 52)
        Me.butAssignJudges.TabIndex = 2
        Me.butAssignJudges.Text = "Assign judges to a pairing (STA automatic judge placement system)"
        Me.butAssignJudges.UseVisualStyleBackColor = True
        '
        'butManualChange
        '
        Me.butManualChange.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butManualChange.Location = New System.Drawing.Point(5, 84)
        Me.butManualChange.Name = "butManualChange"
        Me.butManualChange.Size = New System.Drawing.Size(296, 45)
        Me.butManualChange.TabIndex = 4
        Me.butManualChange.Text = "View a pairing for manual changes"
        Me.butManualChange.UseVisualStyleBackColor = True
        '
        'butRooms
        '
        Me.butRooms.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butRooms.Location = New System.Drawing.Point(5, 377)
        Me.butRooms.Name = "butRooms"
        Me.butRooms.Size = New System.Drawing.Size(296, 35)
        Me.butRooms.TabIndex = 3
        Me.butRooms.Text = "Assign Rooms"
        Me.butRooms.UseVisualStyleBackColor = True
        Me.butRooms.Visible = False
        '
        'butPairRound
        '
        Me.butPairRound.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butPairRound.Location = New System.Drawing.Point(6, 414)
        Me.butPairRound.Name = "butPairRound"
        Me.butPairRound.Size = New System.Drawing.Size(296, 45)
        Me.butPairRound.TabIndex = 1
        Me.butPairRound.Text = "Pair a round"
        Me.butPairRound.UseVisualStyleBackColor = True
        Me.butPairRound.Visible = False
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.butJudgeCards)
        Me.GroupBox4.Controls.Add(Me.butWebIt)
        Me.GroupBox4.Controls.Add(Me.butCards)
        Me.GroupBox4.Controls.Add(Me.butResults)
        Me.GroupBox4.Controls.Add(Me.butRoundInfo)
        Me.GroupBox4.Controls.Add(Me.but)
        Me.GroupBox4.Font = New System.Drawing.Font("Franklin Gothic Medium", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox4.Location = New System.Drawing.Point(948, 210)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(308, 285)
        Me.GroupBox4.TabIndex = 6
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "DISPLAY AND PRINT"
        '
        'butJudgeCards
        '
        Me.butJudgeCards.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butJudgeCards.Location = New System.Drawing.Point(6, 167)
        Me.butJudgeCards.Name = "butJudgeCards"
        Me.butJudgeCards.Size = New System.Drawing.Size(296, 35)
        Me.butJudgeCards.TabIndex = 7
        Me.butJudgeCards.Text = "Judge Cards"
        Me.butJudgeCards.UseVisualStyleBackColor = True
        '
        'butWebIt
        '
        Me.butWebIt.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butWebIt.Location = New System.Drawing.Point(6, 206)
        Me.butWebIt.Name = "butWebIt"
        Me.butWebIt.Size = New System.Drawing.Size(296, 35)
        Me.butWebIt.TabIndex = 6
        Me.butWebIt.Text = "Put it on the web"
        Me.butWebIt.UseVisualStyleBackColor = True
        '
        'butCards
        '
        Me.butCards.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butCards.Location = New System.Drawing.Point(6, 126)
        Me.butCards.Name = "butCards"
        Me.butCards.Size = New System.Drawing.Size(296, 35)
        Me.butCards.TabIndex = 5
        Me.butCards.Text = "Team Cards"
        Me.butCards.UseVisualStyleBackColor = True
        '
        'butResults
        '
        Me.butResults.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butResults.Location = New System.Drawing.Point(6, 84)
        Me.butResults.Name = "butResults"
        Me.butResults.Size = New System.Drawing.Size(296, 35)
        Me.butResults.TabIndex = 4
        Me.butResults.Text = "Results Information"
        Me.butResults.UseVisualStyleBackColor = True
        '
        'butRoundInfo
        '
        Me.butRoundInfo.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butRoundInfo.Location = New System.Drawing.Point(8, 242)
        Me.butRoundInfo.Name = "butRoundInfo"
        Me.butRoundInfo.Size = New System.Drawing.Size(296, 35)
        Me.butRoundInfo.TabIndex = 3
        Me.butRoundInfo.Text = "Information about a specific round"
        Me.butRoundInfo.UseVisualStyleBackColor = True
        Me.butRoundInfo.Visible = False
        '
        'but
        '
        Me.but.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.but.Location = New System.Drawing.Point(6, 42)
        Me.but.Name = "but"
        Me.but.Size = New System.Drawing.Size(296, 35)
        Me.but.TabIndex = 2
        Me.but.Text = "Print Registration, Ballots, Strike Cards"
        Me.but.UseVisualStyleBackColor = True
        '
        'butBasicInfo
        '
        Me.butBasicInfo.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.butBasicInfo.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butBasicInfo.Location = New System.Drawing.Point(995, 6)
        Me.butBasicInfo.Name = "butBasicInfo"
        Me.butBasicInfo.Size = New System.Drawing.Size(261, 39)
        Me.butBasicInfo.TabIndex = 7
        Me.butBasicInfo.Text = "Important info for first-time users!"
        Me.butBasicInfo.UseVisualStyleBackColor = False
        '
        'lblFileLocation
        '
        Me.lblFileLocation.AutoSize = True
        Me.lblFileLocation.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFileLocation.Location = New System.Drawing.Point(15, 171)
        Me.lblFileLocation.Name = "lblFileLocation"
        Me.lblFileLocation.Size = New System.Drawing.Size(136, 20)
        Me.lblFileLocation.TabIndex = 8
        Me.lblFileLocation.Text = "Data file location is"
        '
        'lblTourneyName
        '
        Me.lblTourneyName.AutoSize = True
        Me.lblTourneyName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTourneyName.Font = New System.Drawing.Font("Franklin Gothic Medium", 18.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTourneyName.Location = New System.Drawing.Point(40, 120)
        Me.lblTourneyName.MaximumSize = New System.Drawing.Size(1200, 32)
        Me.lblTourneyName.MinimumSize = New System.Drawing.Size(1200, 32)
        Me.lblTourneyName.Name = "lblTourneyName"
        Me.lblTourneyName.Size = New System.Drawing.Size(1200, 32)
        Me.lblTourneyName.TabIndex = 9
        Me.lblTourneyName.Text = "No tournament loaded!"
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.butSTASynch)
        Me.GroupBox5.Controls.Add(Me.butBackUp)
        Me.GroupBox5.Controls.Add(Me.butUtilities)
        Me.GroupBox5.Controls.Add(Me.butDownload)
        Me.GroupBox5.Controls.Add(Me.butFindFile)
        Me.GroupBox5.Font = New System.Drawing.Font("Franklin Gothic Medium", 16.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox5.Location = New System.Drawing.Point(954, 507)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(306, 223)
        Me.GroupBox5.TabIndex = 10
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "DATA FILE"
        '
        'butSTASynch
        '
        Me.butSTASynch.Location = New System.Drawing.Point(11, 176)
        Me.butSTASynch.Name = "butSTASynch"
        Me.butSTASynch.Size = New System.Drawing.Size(75, 37)
        Me.butSTASynch.TabIndex = 8
        Me.butSTASynch.Text = "STA"
        Me.butSTASynch.UseVisualStyleBackColor = True
        Me.butSTASynch.Visible = False
        '
        'butBackUp
        '
        Me.butBackUp.BackColor = System.Drawing.Color.SkyBlue
        Me.butBackUp.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butBackUp.Location = New System.Drawing.Point(5, 129)
        Me.butBackUp.Name = "butBackUp"
        Me.butBackUp.Size = New System.Drawing.Size(296, 39)
        Me.butBackUp.TabIndex = 6
        Me.butBackUp.Text = "BACK UP"
        Me.butBackUp.UseVisualStyleBackColor = False
        '
        'butUtilities
        '
        Me.butUtilities.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.butUtilities.Location = New System.Drawing.Point(6, 85)
        Me.butUtilities.Name = "butUtilities"
        Me.butUtilities.Size = New System.Drawing.Size(296, 39)
        Me.butUtilities.TabIndex = 5
        Me.butUtilities.Text = "Utilities"
        Me.butUtilities.UseVisualStyleBackColor = True
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.Location = New System.Drawing.Point(6, 6)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(235, 20)
        Me.lblVersion.TabIndex = 11
        Me.lblVersion.Text = "Update 2/17/2013 10:10 TP Time"
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(890, 47)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(366, 45)
        Me.PictureBox1.TabIndex = 12
        Me.PictureBox1.TabStop = False
        '
        'frmMainMenu
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(1264, 730)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.GroupBox5)
        Me.Controls.Add(Me.lblTourneyName)
        Me.Controls.Add(Me.lblFileLocation)
        Me.Controls.Add(Me.butBasicInfo)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Franklin Gothic Medium", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "frmMainMenu"
        Me.Text = "CAT 2.0 Main Menu"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox5.ResumeLayout(False)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents butBasicInfo As System.Windows.Forms.Button
    Friend WithEvents butEnterJudges As System.Windows.Forms.Button
    Friend WithEvents butEnterTeams As System.Windows.Forms.Button
    Friend WithEvents butEnterPrefs As System.Windows.Forms.Button
    Friend WithEvents butEnterRooms As System.Windows.Forms.Button
    Friend WithEvents butEnterSchools As System.Windows.Forms.Button
    Friend WithEvents butDivisionSetup As System.Windows.Forms.Button
    Friend WithEvents butTourneySetup As System.Windows.Forms.Button
    Friend WithEvents butRooms As System.Windows.Forms.Button
    Friend WithEvents butAssignJudges As System.Windows.Forms.Button
    Friend WithEvents butPairRound As System.Windows.Forms.Button
    Friend WithEvents butManualChange As System.Windows.Forms.Button
    Friend WithEvents butResults As System.Windows.Forms.Button
    Friend WithEvents butRoundInfo As System.Windows.Forms.Button
    Friend WithEvents but As System.Windows.Forms.Button
    Friend WithEvents butDownload As System.Windows.Forms.Button
    Friend WithEvents butFindFile As System.Windows.Forms.Button
    Friend WithEvents lblFileLocation As System.Windows.Forms.Label
    Friend WithEvents lblTourneyName As System.Windows.Forms.Label
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents butUtilities As System.Windows.Forms.Button
    Friend WithEvents butSetUpRounds As System.Windows.Forms.Button
    Friend WithEvents butTieBreakers As System.Windows.Forms.Button
    Friend WithEvents butElims As System.Windows.Forms.Button
    Friend WithEvents butCards As System.Windows.Forms.Button
    Friend WithEvents butManualPair As System.Windows.Forms.Button
    Friend WithEvents butTRPCPair As System.Windows.Forms.Button
    Friend WithEvents lblSetupStatus As System.Windows.Forms.Label
    Friend WithEvents butSchoolEntry As System.Windows.Forms.Button
    Friend WithEvents butJudgePref As System.Windows.Forms.Button
    Friend WithEvents butBallotEntry As System.Windows.Forms.Button
    Friend WithEvents butSetupHelp As System.Windows.Forms.Button
    Friend WithEvents butTeamBlocks As System.Windows.Forms.Button
    Friend WithEvents butWebIt As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lblVersion As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents butPresetSmallDivision As System.Windows.Forms.Button
    Friend WithEvents butJudgeCards As System.Windows.Forms.Button
    Friend WithEvents butBackUp As System.Windows.Forms.Button
    Friend WithEvents FolderBrowserDialog1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents butSTASynch As System.Windows.Forms.Button

End Class
