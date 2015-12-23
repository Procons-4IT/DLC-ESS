Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Odbc
Imports DAL
Imports EN
Public Class ChangePwdDA
    Dim objEN As ChangePwdEN = New ChangePwdEN()
    Dim objDA As DBConnectionDA = New DBConnectionDA()
    Dim Password As String
    Public Sub New()
        objDA.con = New SqlConnection(objDA.GetConnection)
    End Sub
    Public Function Checkpassword(ByVal objen As ChangePwdEN) As Boolean
        Try
            Password = objDA.Encrypt(objen.OldPwd, objDA.key)
            objDA.strQuery = "select * from ""@Z_DLC_LOGIN"" where U_EMPID='" & objen.EmpId & "' and U_PWD='" & Password & "'"
            objDA.sqlda = New SqlDataAdapter(objDA.strQuery, objDA.con)
            objDA.sqlda.Fill(objDA.ds)
            If objDA.ds.Tables(0).Rows.Count > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
        End Try
    End Function
    Public Sub UpdatePassword(ByVal objen As ChangePwdEN)
        Try
            Password = objDA.Encrypt(objen.ConfirmPwd, objDA.key)
            objDA.strQuery = "Update ""@Z_DLC_LOGIN"" set U_PWD='" & Password & "' where U_EMPID='" & objen.EmpId & "'"
            objDA.cmd = New SqlCommand(objDA.strQuery, objDA.con)
            objDA.con.Open()
            objDA.cmd.ExecuteNonQuery()
            objDA.con.Close()
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
        End Try
      
    End Sub
End Class
