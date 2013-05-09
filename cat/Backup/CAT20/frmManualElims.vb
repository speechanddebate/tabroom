Public Class frmManualElims
    Dim ds As New DataSet
    Private Sub frmManualElims_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LoadFile(ds, "TourneyData")
        DataGridView1.DataSource = ds.Tables("Entry").DefaultView
    End Sub
    Private Sub frmManualElims_Unload(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Disposed
        Call SaveFile(ds)
        ds.Dispose()
    End Sub

End Class