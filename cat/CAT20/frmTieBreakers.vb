Public Class frmTieBreakers
    Private masterBindingSource As New BindingSource()
    Private detailsBindingSource As New BindingSource()
    Private SettingdetailsBindingSource As New BindingSource()
    Dim ds As New DataSet
    Private Sub frmTieBreakers_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Load
        LoadFile(ds, "TourneyData")
        Call InitializeScores(ds)
        Call InitializeTieBreakers(ds, False)
        Call InitializeScoreSettings(ds)

        'This adds the score column as a combobox; the others are managed in the design view by setting the collection
        Dim dgvc As New DataGridViewComboBoxColumn
        dgvc.DataSource = ds.Tables("Scores")
        dgvc.ValueMember = "ID"                 'from child table
        dgvc.DisplayMember = "SCORE_NAME"             'from child table
        dgvc.DataPropertyName = "SCOREID"         'from parent table
        dgvc.HeaderText = "Raw Score"
        dgvc.Name = "Score"
        dgvc.DisplayIndex = 4
        dgvc.SortMode = DataGridViewColumnSortMode.Automatic
        DataGridView3.Columns.Add(dgvc)

        dgvc = New DataGridViewComboBoxColumn
        dgvc.DataSource = ds.Tables("Scores")
        dgvc.ValueMember = "ID"                 'from child table
        dgvc.DisplayMember = "SCORE_NAME"       'from child table
        dgvc.DataPropertyName = "SCORE"        'from parent table
        dgvc.HeaderText = "SCORE"
        dgvc.Name = "Score"
        dgvc.DisplayIndex = 1
        dgvc.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing
        dgvc.ReadOnly = True
        dgvc.SortMode = DataGridViewColumnSortMode.Automatic
        DataGridView4.Columns.Add(dgvc)

        'I don't really get what the next 2 chunks of code do, but they end up with a parent/child relationship
        'between the tables

        ' Bind the master data connector to the Customers table.
        masterBindingSource.DataSource = ds
        masterBindingSource.DataMember = "TIEBREAK_SET"  'table name of master table

        ' Bind the details data connector to the master data connector,
        ' using the DataRelation name to filter the information in the 
        ' details table based on the current row in the master table. 
        detailsBindingSource.DataSource = masterBindingSource
        detailsBindingSource.DataMember = "TieBreakSetToFunction"  'name of the datarelation

        'These format the columns and binds the data
        DataGridView2.AutoGenerateColumns = False
        DataGridView2.DataSource = masterBindingSource

        DataGridView3.AutoGenerateColumns = False
        DataGridView3.DataSource = detailsBindingSource
        
        'Now bind the Score_Settings off the Tiebreak_Set field
        SettingdetailsBindingSource.DataSource = masterBindingSource
        SettingdetailsBindingSource.DataMember = "TieBreakSetToScoreSetting"  'name of the datarelation
        DataGridView4.AutoGenerateColumns = False
        DataGridView4.DataSource = SettingdetailsBindingSource


    End Sub
    Sub SortAfterChanges() Handles DataGridView3.DataBindingComplete
        Try
            If DataGridView3.RowCount > 0 Then
                DataGridView3.Sort(DataGridView3.Columns("SortOrder"), System.ComponentModel.ListSortDirection.Ascending)
            End If
        Catch
            MsgBox("There was a problem with the sorting....this is just a warning.  You may want to manually sort the order of the tiebreakers.")
        End Try
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim q = MsgBox("This button is appropriate if there are no tiebreakers already and you intend to run the tournament all offline.  If you do reset the tiebreakers, make sure you link the rounds to the new tiebreaker sets on setup screen 4.  Continue?", MsgBoxStyle.YesNo)
        If q = vbNo Then Exit Sub
        Call InitializeTieBreakers(ds, True)
        Call InitializeScoreSettings(ds)
    End Sub
    Private Sub frmTieBreakers_Unload(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Disposed
        Call SaveFile(ds)
        Call SetupCompletionStatus(ds)
        ds.Dispose()
    End Sub
    Private Sub ScoreInfo() Handles DataGridView4.MouseClick
        If DataGridView4.CurrentCell.ColumnIndex = 5 Then
            lblScoreInfo.Text = "This column is NOT editable.  It identifies the name of allowable scores, and is preset.  You can edit the last 4 columns."
        ElseIf DataGridView4.CurrentCell.ColumnIndex = 1 Then
            lblScoreInfo.Text = "This column IS editable.  Enter the highest possible value that judges can award for this score."
        ElseIf DataGridView4.CurrentCell.ColumnIndex = 2 Then
            lblScoreInfo.Text = "This column IS editable.  Enter the lowest possible value that judges can award for this score."
        ElseIf DataGridView4.CurrentCell.ColumnIndex = 3 Then
            lblScoreInfo.Text = "This column IS editable.  It indicates whether speakers or teams in a given round can receive the same value for this score.  For example, ranks generally cannot be duplicated, but points may be."
        ElseIf DataGridView4.CurrentCell.ColumnIndex = 4 Then
            lblScoreInfo.Text = "This column IS editable.  It indicates the decimal increments that judges may award.  Enter 0 for whole points only, .5 for half-points, and .1 for decimal increments."
        End If

    End Sub

    Private Sub butTBSETInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butTBSETInfo.Click
        Dim strInfo = "A tiebreaker set is just a group of tiebreakers, in the order you would like the computer to sort them." & Chr(10) & Chr(10)
        strInfo &= "For example, there may be one set of tiebreakers for speaker awards and another one given for team awards (wins might be the top tiebreaker for teams, but a very low tiebreaker for speakers)." & Chr(10) & Chr(10)
        strInfo &= "In fact, in the TRPC you were limited to tiebreaker sets for speakers and teams.  But in CAT 2.0 you can create your own and use as many as you want." & Chr(10) & Chr(10)
        strInfo &= "If this seems scary, just click the button to create the default tiebreakers, review them for any minor adjustments, and you'll be fine." & Chr(10) & Chr(10)
        strInfo &= "If you are using something very sophisticated, like Gary Larson's opponent seed plus seed formula, you might want a separate set of tiebreakers to pair elims (where opponent seed is no longer relevant) than prelims (where opponent seed is the 2nd tiebreaker)." & Chr(10) & Chr(10)
        strInfo &= "You can specify which tiebreakers are used for which rounds on the rounds form." & Chr(10) & Chr(10)
        strInfo &= "To add a new tiebreaker set simply type a new name for it in the last row of the tiebreaker set grid and then add and order the tiebreakers using the top right-hand box." & Chr(10) & Chr(10)
        MsgBox(strInfo)
    End Sub

    Private Sub butTerminology_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butTerminology.Click
        Dim strInfo As String = ""
        strInfo &= "A SCORE is the raw value that a judge assigns, like speaker points, ranks, or a ballot.  Note that ballots are only synonymous with wins for 1-judge panels; otherwise, a team can get a judge's ballot but still lose the round.  The more general way to think is that the ballot is the SCORE the judge awards.  Some calculation might be performed on ballot scores to determine a winner.  There are 5 SCORES and they appear in the box at the bottom of the page." & Chr(10) & Chr(10)
        strInfo &= "SCORES have SCORE SETTINGS.  For example, speaker points may be in half-point, whole point, or decimal settings, and ties for speaker points may or may not be allowed.  You can enter values for the score settings at the bottom of this page." & Chr(10) & Chr(10)
        strInfo &= "A TIEBREAKER involves taking a raw SCORE and performing a calculation on it.  For example, total speaker points involve summing all the points a speaker receives, and might involve the dropping of high and low values." & Chr(10) & Chr(10)
        strInfo &= "TIEBREAKERS can be organized in TIEBREAKER SETS, which are a collection of tiebreakers and the order in which they are used.  You can find out more about tiebreaker sets by clicking on the WHAT ARE TIEBREAKER SETS? button below the tiebreaker set grid on the left-hand side." & Chr(10) & Chr(10)
        strInfo &= "IMPORTANT NOTE FOR WUDC USERS: Ranks are NOT stored, only rank points (1st rank=3, 2nd rank=2, 3rd rank=1, 4th rank=0).  They are sorted in DESCENDING order, so higher scores are better, and 'Total 3 Ranks' is probably the first tiebreaker after team points." & Chr(10) & Chr(10)
        MsgBox(strInfo)
    End Sub

    Private Sub butPageHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butPageHelp.Click
        Dim strInfo As String = ""
        strInfo &= "IF THIS SEEMS OVERWHELMING, JUST CLICK THE 'CREATE DEFAULT TIEBREAKERS' BUTTON, MAYBE CHANGE THE ORDER OF THE TIEBREAKERS A LITTLE BIT, BE DONE WITH IT." & Chr(10) & Chr(10)
        strInfo &= "You can contrl 3 things on this screen: (1) score settings, (2) tiebreaker sets, and (3) tiebreakers." & Chr(10) & Chr(10)
        strInfo &= "(1) The first thing to do is decide how many tiebreaker sets you want and what they are.  You may wish to click on the WHAT ARE TIEBREAKER SETS? button for more information.  Enter all the tiebreaker sets that you want before proceeding to the next step." & Chr(10) & Chr(10)
        strInfo &= "(2) The second step is to define all tiebreakers for each tiebreaker set.  Click on the HOW TO ENTER AND EDIT TIEBREAKERS button for information about how to do this." & Chr(10) & Chr(10)
        strInfo &= "(3) The third and final step is to enter score settings for each tiebreaker set.  For example, you might want to use whole points only for a Public Forum division but half-points for a Policy division.  Click on the tiebreaker set and the current score settings will appear in the box at the bottom of the screen.  Change the score settings by altering the values in the bottom box.  Help will appear to the right of the bottom box as you do so." & Chr(10) & Chr(10)
        MsgBox(strInfo)
    End Sub

    Private Sub butAddDeleteHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butAddDeleteHelp.Click
        Dim strInfo As String = ""
        strInfo &= "To delete a tiebreaker set, click on the desired row and click the delete button.  Be aware that this will delete all tiebreakers associated with this tiebreaker set.  If you have already set up rounds associated with these tiebreakers and tiebreaker sets, you'll need to reset all those values." & Chr(10) & Chr(10)
        strInfo &= "To add a tiebreaker set, type a new name for the tiebreaker set in the last row of the grid and select who the tiebreaker set is for.  Note that you *must* hit either ENTER or TAB after selecting a value from the drop-down list.  Your next step will be to enter tiebreakers for the tiebreaker set using the top-right-hand box." & Chr(10) & Chr(10)
        MsgBox(strInfo)
    End Sub

    Private Sub butTiebreakerHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butTiebreakerHelp.Click
        Dim strInfo As String = ""
        strInfo &= "If you just want to change the order of the tiebreakers, enter a new number in the first column labeled ORDER.  The grid will automatically re-sort after you hit ENTER or TAB." & Chr(10) & Chr(10)
        strInfo &= "If you wish to DELETE an existing tiebreaker, select the row by clicking on the gray box to the left of the grid and use the DELETE button on your keyboard." & Chr(10) & Chr(10)
        strInfo &= "If you wish to ADD a tiebreaker, be aware that you can use PRESET tiebreakers or use them in an entirely CUSTOMIZED way." & Chr(10) & Chr(10)
        strInfo &= "To add a tiebreaker, simply enter values in the bottom row." & Chr(10) & Chr(10)
        strInfo &= "PRESET tiebreakers are defined by the TAG; the TAG value will over-ride all other tiebreaker settings.  To use a preset tiebreaker, enter all values in the bottom row, taking special care to select the appropriate TAG. " & Chr(10) & Chr(10)
        strInfo &= "For a CUSTOMIZED tiebreaker, select 'None' in the TAG column and enter other values. Click the UNDERSTANDING THE TIEBREAKER COLUMNS button for more information" & Chr(10) & Chr(10)
        MsgBox(strInfo)
    End Sub

    Private Sub butTBColumnDefinitions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butTBColumnDefinitions.Click
        Dim strInfo As String = ""
        strInfo &= "The ORDER column indicate the order in which the tiebreaker will be sorted.  1=1st tiebreaker, 2=2nd tiebreaker, etc." & Chr(10) & Chr(10)
        strInfo &= "The LABEL column simply describes the tiebreaker.  You can type in any description that you want." & Chr(10) & Chr(10)
        strInfo &= "The remaining colulmns tell the CAT how to calculate the tiebreakers; you may wish to click the IMPORTANT TERMINOLOGY button before proceeding with a customized tiebreaker." & Chr(10) & Chr(10)
        strInfo &= "The RAW SCORE column indicates the SCORE that will be used in the calcuations.  There are 5 of them, and their settings are defined in the grid at the bottom of the page." & Chr(10) & Chr(10)
        strInfo &= "The H/L DROPS column indicates how many high-low drops should be performed.  Entering 0 will return the total scores; entering 1 will drop the high and low scores, entering 2 will drop the 2 highest and 2 lowest scores, etc." & Chr(10) & Chr(10)
        strInfo &= "The FOR OPPONENT column indicates whether the score should be calculated for the team in question or for their opponents.  Check this box to calculate opponent wins, opponent points, etc." & Chr(10) & Chr(10)
        MsgBox(strInfo)
    End Sub
    Private Sub datagridview3_DefaultValuesNeeded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles DataGridView3.DefaultValuesNeeded

        With e.Row
            .Cells("SortOrder").Value = 99
            .Cells("ForOpponent").Value = False
        End With

    End Sub

    Private Sub butWinsShortcut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butWinsShortcut.Click

        Dim dr As DataRow
        dr = ds.Tables("TIEBREAK").NewRow
        dr.Item("ID") = NextID
        dr.Item("SortOrder") = NextSortOrder
        dr.Item("ScoreID") = 1
        dr.Item("Tag") = "Wins"
        dr.Item("Drops") = 0
        dr.Item("ForOpponent") = False
        dr.Item("TB_SET") = DataGridView2.CurrentRow.Cells("ID").Value
        dr.Item("Label") = "Wins"

        If TBExists(dr) = True Then Exit Sub

        ds.Tables("TIEBREAK").Rows.Add(dr)

    End Sub
    Function NextID() As Integer
        NextID = -99
        For x = 0 To ds.Tables("Tiebreak").Rows.Count - 1
            If ds.Tables("Tiebreak").Rows(x).Item("ID") > NextID Then NextID = ds.Tables("Tiebreak").Rows(x).Item("ID")
        Next
        NextID += 1
    End Function
    Function NextSortOrder() As Integer
        NextSortOrder = -99
        For x = 0 To DataGridView3.Rows.Count - 1
            If DataGridView3.Rows(x).Cells("SortOrder").Value > NextSortOrder Then
                NextSortOrder = DataGridView3.Rows(x).Cells("SortOrder").Value
            End If
        Next x
        NextSortOrder += 1
    End Function
    Function TBExists(ByVal dr As DataRow) As Boolean
        TBExists = False
        'compare scoreID, Tag, drops, ForOpponent, TB_SET
        Dim match As Boolean
        For x = 0 To ds.Tables("Tiebreak").Rows.Count - 1
            If ds.Tables("Tiebreak").Rows(x).Item("TB_SET") = DataGridView2.CurrentRow.Cells("ID").Value Then
                match = True
                If dr.Item("ScoreID") <> ds.Tables("Tiebreak").Rows(x).Item("ScoreID") Then match = False
                If dr.Item("Tag") <> ds.Tables("Tiebreak").Rows(x).Item("Tag") Then match = False
                If dr.Item("Drops") <> ds.Tables("Tiebreak").Rows(x).Item("Drops") Then match = False
                If dr.Item("ForOpponent") <> ds.Tables("Tiebreak").Rows(x).Item("ForOpponent") Then match = False
                If match = True Then TBExists = True : Exit For
            End If
        Next x
        If TBExists = True Then
            MsgBox("Not added; that tiebreaker already appears to be included in this tiebreaker set.")
        End If
    End Function

    Private Sub butBallotsShortCut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butBallotsShortCut.Click
        Dim dr As DataRow
        dr = ds.Tables("TIEBREAK").NewRow
        dr.Item("ID") = NextID()
        dr.Item("SortOrder") = NextSortOrder()
        dr.Item("ScoreID") = 1
        dr.Item("Tag") = "Ballots"
        dr.Item("Drops") = 0
        dr.Item("ForOpponent") = False
        dr.Item("TB_SET") = DataGridView2.CurrentRow.Cells("ID").Value
        dr.Item("Label") = "Ballots"

        If TBExists(dr) = True Then Exit Sub

        ds.Tables("TIEBREAK").Rows.Add(dr)

    End Sub

    Private Sub butJudVarShortcut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butJudVarShortcut.Click
        Dim dr As DataRow
        dr = ds.Tables("TIEBREAK").NewRow
        dr.Item("ID") = NextID()
        dr.Item("SortOrder") = NextSortOrder()
        dr.Item("ScoreID") = 2
        dr.Item("Tag") = "JudgeVariance"
        dr.Item("Drops") = 0
        dr.Item("ForOpponent") = False
        dr.Item("TB_SET") = DataGridView2.CurrentRow.Cells("ID").Value
        dr.Item("Label") = "Judge Variance"

        If TBExists(dr) = True Then Exit Sub

        ds.Tables("TIEBREAK").Rows.Add(dr)

    End Sub

    Private Sub butOppWInShortCut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butOppWInShortCut.Click
        Dim dr As DataRow
        dr = ds.Tables("TIEBREAK").NewRow
        dr.Item("ID") = NextID()
        dr.Item("SortOrder") = NextSortOrder()
        dr.Item("ScoreID") = 1
        dr.Item("Tag") = "OppWins"
        dr.Item("Drops") = 0
        dr.Item("ForOpponent") = True
        dr.Item("TB_SET") = DataGridView2.CurrentRow.Cells("ID").Value
        dr.Item("Label") = "Opposition Wins"

        If TBExists(dr) = True Then Exit Sub

        ds.Tables("TIEBREAK").Rows.Add(dr)

    End Sub

    Private Sub butOppPtsShortCut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butOppPtsShortCut.Click
        Dim dr As DataRow
        dr = ds.Tables("TIEBREAK").NewRow
        dr.Item("ID") = NextID()
        dr.Item("SortOrder") = NextSortOrder()
        dr.Item("ScoreID") = 2
        dr.Item("Tag") = "None"
        dr.Item("Drops") = 0
        dr.Item("ForOpponent") = True
        dr.Item("TB_SET") = DataGridView2.CurrentRow.Cells("ID").Value
        dr.Item("Label") = "Opposition Pts"

        If TBExists(dr) = True Then Exit Sub

        ds.Tables("TIEBREAK").Rows.Add(dr)

    End Sub


    Private Sub butRandomShortCut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butRandomShortCut.Click
        Dim dr As DataRow
        dr = ds.Tables("TIEBREAK").NewRow
        dr.Item("ID") = NextID()
        dr.Item("SortOrder") = NextSortOrder()
        dr.Item("ScoreID") = 1
        dr.Item("Tag") = "Random"
        dr.Item("Drops") = 0
        dr.Item("ForOpponent") = False
        dr.Item("TB_SET") = DataGridView2.CurrentRow.Cells("ID").Value
        dr.Item("Label") = "Random"

        If TBExists(dr) = True Then Exit Sub

        ds.Tables("TIEBREAK").Rows.Add(dr)

    End Sub

    Private Sub butMBAShortCut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butMBAShortCut.Click
        Dim dr As DataRow
        dr = ds.Tables("TIEBREAK").NewRow
        dr.Item("ID") = NextID()
        dr.Item("SortOrder") = NextSortOrder()
        dr.Item("ScoreID") = 1
        dr.Item("Tag") = "MBA"
        dr.Item("Drops") = 0
        dr.Item("ForOpponent") = False
        dr.Item("TB_SET") = DataGridView2.CurrentRow.Cells("ID").Value
        dr.Item("Label") = "MBA HL pts + OppWins"

        If TBExists(dr) = True Then Exit Sub

        ds.Tables("TIEBREAK").Rows.Add(dr)

    End Sub

    Private Sub butRanksShortCut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butRanksShortCut.Click
        Dim dr As DataRow
        dr = ds.Tables("TIEBREAK").NewRow
        dr.Item("ID") = NextID()
        dr.Item("SortOrder") = NextSortOrder()
        dr.Item("ScoreID") = 3
        dr.Item("Tag") = "None"
        dr.Item("Drops") = Val(txtDrops.Text)
        dr.Item("ForOpponent") = False
        dr.Item("TB_SET") = DataGridView2.CurrentRow.Cells("ID").Value
        dr.Item("Label") = "Ranks"
        If Val(txtDrops.Text) > 0 Then dr.Item("Label") = txtDrops.Text.Trim & "x HiLo Ranks"
        If TBExists(dr) = True Then Exit Sub

        ds.Tables("TIEBREAK").Rows.Add(dr)

    End Sub

    Private Sub butSpkrPtsShortCut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butSpkrPtsShortCut.Click
        Dim dr As DataRow
        dr = ds.Tables("TIEBREAK").NewRow
        dr.Item("ID") = NextID()
        dr.Item("SortOrder") = NextSortOrder()
        dr.Item("ScoreID") = 2
        dr.Item("Tag") = "None"
        dr.Item("Drops") = Val(txtDrops.Text)
        dr.Item("ForOpponent") = False
        dr.Item("TB_SET") = DataGridView2.CurrentRow.Cells("ID").Value
        dr.Item("Label") = "Points"
        If Val(txtDrops.Text) > 0 Then dr.Item("Label") = txtDrops.Text.Trim & "x HiLo Points"
        If TBExists(dr) = True Then Exit Sub

        ds.Tables("TIEBREAK").Rows.Add(dr)

    End Sub
    Sub eHandle() Handles DataGridView3.DataError
        'MsgBox("Error at column" & DataGridView3.CurrentCell.ColumnIndex)
    End Sub
    Private Sub butTotal1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butTotal1.Click
        Call TotalRankCreator(1)
    End Sub
    Sub TotalRankCreator(ByVal nRank As Integer)

        Dim dr As DataRow
        dr = ds.Tables("TIEBREAK").NewRow
        dr.Item("ID") = NextID()
        dr.Item("SortOrder") = NextSortOrder()
        dr.Item("ScoreID") = 5
        dr.Item("Tag") = "TotalRanks" & nRank.ToString
        dr.Item("Drops") = 0
        dr.Item("ForOpponent") = False
        dr.Item("TB_SET") = DataGridView2.CurrentRow.Cells("ID").Value
        dr.Item("Label") = "Total " & nRank & " ranks"
        If TBExists(dr) = True Then Exit Sub

        ds.Tables("TIEBREAK").Rows.Add(dr)
    End Sub

    Private Sub butTotal2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butTotal2.Click
        Call TotalRankCreator(2)
    End Sub

    Private Sub butTotal3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butTotal3.Click
        Call TotalRankCreator(3)
    End Sub
End Class