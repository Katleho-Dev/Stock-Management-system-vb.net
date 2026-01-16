Public Class Form1
    '0607090260086
    'Setting the provider and the data source for DB connection
    Dim strProvider As String = "Provider=Microsoft.ACE.OLEDB.12.0;"
    Dim strDataSource As String = "Data Source=C:\Users\katle\OneDrive\Documents\PhakediAss2.accdb"

    Dim strFilepath As String = strProvider & strDataSource
    Dim bs As BindingSource

    Private Sub btnDisplay_Click(sender As Object, e As EventArgs) Handles btnDisplay.Click
        'Establishing the DB connection
        Dim conn As New OleDb.OleDbConnection
        Dim command As New OleDb.OleDbCommand
        Dim table As New Data.DataTable
        Dim strSQL As String = ""
        Try
            conn.ConnectionString = strFilepath
            conn.Open()
            command.Connection = conn
            strSQL = "SELECT * FROM Products;"
            command.CommandText = strSQL
            table.Load(command.ExecuteReader())
            conn.Close()
            DataGridView1.DataSource = table

        Catch ex As Exception

        End Try

    End Sub

    Private Sub btnInsert_Click(sender As Object, e As EventArgs) Handles btnInsert.Click

        'Defensive programming
        If String.IsNullOrWhiteSpace(txtProdName.Text) Then
            MessageBox.Show("Please fill in all fields before continuing", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
        Dim conn As New OleDb.OleDbConnection
        Dim comm As New OleDb.OleDbCommand
        Try
            conn.ConnectionString = strFilepath
            conn.Open()
            comm.Connection = conn
            'Sql Statement
            comm.CommandText = "INSERT INTO [Order] (OrderID, OrderQuantity) VALUES (@OrderID, @OrderQuantity)"
            comm.Parameters.AddWithValue("@OrderQuantity", txtQuantityO.Text)
            comm.Parameters.AddWithValue("@OrderID", txtOrderid.Text)
            'Execute Insert
            comm.ExecuteNonQuery()
            MessageBox.Show("Order Inserted Successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            'Reload data into the datagrid
            Dim table As New DataTable
            comm.CommandText = "SELECT * FROM [Order]"
            comm.Parameters.Clear()
            Dim adapter As New OleDb.OleDbDataAdapter(comm)
            adapter.Fill(table)
            DataGridView1.DataSource = table
        Catch ex As Exception
            MessageBox.Show("Error" & ex.Message, "Insert Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Dim conn As New OleDb.OleDbConnection
        Dim table As New Data.DataTable
        Dim command As New OleDb.OleDbCommand
        Dim strsql As String = ""
        Try
            conn.ConnectionString = strFilepath
            conn.Open()
            command.Connection = conn
            strsql = "UPDATE [Products] SET" & "ProductName = @ProductName, CostPrice = @CostPrice" & "WHERE ProductID = @ProductID"
            command.CommandText = strsql
            command.Parameters.AddWithValue("@ProductName", txtProdName.Text)
            command.Parameters.AddWithValue("@CostPrice", txtCostprice.Text)
            command.Parameters.AddWithValue("@ProductID", txtProdId.Text)

            Dim rowsAffected As Integer = command.ExecuteNonQuery()
            If rowsAffected > 0 Then
                MessageBox.Show("Product updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
            Dim adapter = New OleDb.OleDbDataAdapter("SELECT * FROM [Products]", conn)
            table.Load(command.ExecuteReader())
            adapter.Fill(table)
            DataGridView1.DataSource = table

            conn.Close()
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message, "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Dim conn As New OleDb.OleDbConnection
        Dim command As New OleDb.OleDbCommand
        Dim table As New Data.DataTable
        Dim strSQl As String = ""
        Try
            conn.ConnectionString = strFilepath
            conn.Open()
            command.Connection = conn
            strSQl = "DELETE FROM Supplier WHERE SupplierID = 1;"
            command.CommandText = strSQl
            table.Load(command.ExecuteReader())
            conn.Close()
            DataGridView1.DataSource = table

        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        'Defensive Programming
        If String.IsNullOrWhiteSpace(txtSupplier.Text) Then
            MessageBox.Show("Please enter Supplier Id to search", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        'Declare connection, command and data table
        Dim conn As New OleDb.OleDbConnection
        Dim command As New OleDb.OleDbCommand
        Dim table As New DataTable
        Try
            conn.ConnectionString = strFilepath
            conn.Open()
            command.Connection = conn
            command.CommandText = "SELECT * FROM Supplier WHERE SupplierID =?;"
            command.Parameters.AddWithValue("?", txtSupplier.Text)
            'Load Results
            table.Load(command.ExecuteReader)
            conn.Close()
            If table.Rows.Count = 0 Then
                MessageBox.Show("Supplier Not Found", "Not Found", MessageBoxButtons.RetryCancel, MessageBoxIcon.Information)
            End If
            'Show results in DataGrid
            DataGridView1.DataSource = table

        Catch ex As Exception

        End Try
    End Sub
End Class
