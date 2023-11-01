Imports System.Data
Imports System.Data.SqlClient
Module Module1
    Public Class ClsKoneksi
        Protected SQL As String
        Protected Ds As DataSet
        Protected Dt As DataTable
        Public conn As SqlConnection = Nothing
        Dim cmd As SqlCommand
        Dim da As SqlDataAdapter

        Public Function OpenConn() As Boolean
            'connection string database sql server
            SQL = "Initial Catalog=prns101;Data Source=localhost;Integrated Security=SSPI;"

            ' ini connection string yang harus sesuai dengan nama database dan nama server
            conn = New SqlConnection(SQL)
            conn.Open()
            If conn.State <> ConnectionState.Open Then
                Return False
            Else
                Return True

            End If
        End Function
        Public Sub CloseConn()
            If Not IsNothing(conn) Then
                conn.Close()
                conn = Nothing
            End If
        End Sub
        Public Function ExecuteQuery(ByVal Query As String) As DataTable
            If Not OpenConn() Then
                MsgBox("Koneksi Gagal..!!", MsgBoxStyle.Critical, "Access Failed")
                Return Nothing
                Exit Function
            End If
            cmd = New SqlCommand(Query, conn)
            da = New SqlDataAdapter
            da.SelectCommand = cmd

            Ds = New Data.DataSet
            da.Fill(Ds)

            Dt = Ds.Tables(0)

            Return Dt

            Dt = Nothing
            Ds = Nothing
            da = Nothing
            cmd = Nothing

            CloseConn()

        End Function
        Public Sub ExecuteNonQuery(ByVal Query As String)
            If Not OpenConn() Then
                MsgBox("Koneksi Gagal..!!", MsgBoxStyle.Critical, "Access Failed..!!")
                Exit Sub
            End If

            cmd = New SqlCommand
            cmd.Connection = conn
            cmd.CommandType = CommandType.Text
            cmd.CommandText = Query
            cmd.ExecuteNonQuery()
            cmd = Nothing
            CloseConn()
        End Sub
    End Class
End Module

