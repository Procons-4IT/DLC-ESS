Imports Microsoft.VisualBasic
Imports System
Imports System.Web
Imports System.Xml
Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Imports EN
Public Class LoginDA
    Dim objen As LoginEN = New LoginEN()
    Dim objDA As DBConnectionDA = New DBConnectionDA()
    Dim Password As String
    Public Sub New()
        objDA.con = New SqlConnection(objDA.GetConnection)
    End Sub
    Public Function ValidateActiveEmployee(ByVal objen1 As String) As String
        Dim status As String = ""
        Try
            objDA.con.Open()
            objDA.cmd = New SqlCommand("SELECT Active   FROM OHEM WHERE   empID='" & objen1 & "'", objDA.con)
            objDA.cmd.CommandType = CommandType.Text
            status = objDA.cmd.ExecuteScalar()
            objDA.con.Close()
            Return status
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
        End Try
        Return status
    End Function
    Public Function UserAuthentication(ByVal objen As LoginEN) As Boolean
        Dim status As String = ""
        Try
            objDA.con.Open()
            Password = objDA.Encrypt(objen.Pwd, objDA.key)
            'objDA.cmd = New SqlCommand("select U_EMPID  from [@Z_DLC_LOGIN] WHERE U_UID='" & objen.Uid & "' AND U_PWD='" & Password & "' and isnull(U_ESSLogType,'E')='E'", objDA.con)
            objDA.cmd = New SqlCommand("select U_EMPID  from [@Z_DLC_LOGIN] WHERE U_UID='" & objen.Uid & "' AND U_PWD='" & Password & "'", objDA.con)
            objDA.cmd.CommandType = CommandType.Text
            status = objDA.cmd.ExecuteScalar()
            objDA.con.Close()
            If status <> "" Then
                If ValidateActiveEmployee(status) = "N" Then
                    Return False
                End If
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
        End Try
    End Function
    Public Function GetCardCode(ByVal objen As LoginEN) As String
        Dim status As String = ""
        Try
            objDA.con.Open()
            Password = objDA.Encrypt(objen.Pwd, objDA.key)
            objDA.cmd = New SqlCommand("SELECT isnull(U_EMPID,'') FROM [@Z_DLC_LOGIN] WHERE U_UID='" & objen.Uid & "' AND U_PWD='" & Password & "'", objDA.con)
            objDA.cmd.CommandType = CommandType.Text
            status = objDA.cmd.ExecuteScalar()
            objDA.con.Close()
            Return status
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
        End Try
        Return status
    End Function
    Public Function GetCardName(ByVal objen As LoginEN) As String
        Dim status As String = ""
        Try
            objDA.con.Open()
            objDA.cmd = New SqlCommand("SELECT isnull(firstName,'') +' '+ isnull(middleName,'') +' '+ isnull(lastName,'')   FROM OHEM WHERE empID='" & objen.SessionId & "'", objDA.con)
            objDA.cmd.CommandType = CommandType.Text
            status = objDA.cmd.ExecuteScalar()
            objDA.con.Close()
            Return status
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
        End Try
        Return status
    End Function
    Public Function CompanyAddress() As DataSet
        Try
            objDA.con.Open()
            objDA.sqlda = New SqlDataAdapter("select T0.[compnyname],isnull(T1.[Street],''),isnull(T1.[Block],'')+','+isnull(T1.[City],'')+','+isnull(T1.[Zipcode],''),isnull(T1.[State],'')+','+ isnull(T1.[Country],'') from OADM T0 inner join ADM1 T1 ON T0.[Country]=T1.[Country]", objDA.con)
            objDA.sqlda.Fill(objDA.ds)
            If objDA.ds.Tables(0).Rows.Count > 0 Then
                Return objDA.ds
            End If
            Return objDA.ds
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
        End Try
    End Function
    Public Function ESSStoreKeeper(ByVal objen As LoginEN) As Boolean
        Dim status As String = ""
        Try
            objDA.con.Open()
            Password = objDA.Encrypt(objen.Pwd, objDA.key)
            objDA.cmd = New SqlCommand("select U_EMPID  from [@Z_DLC_LOGIN] WHERE U_UID='" & objen.Uid & "' AND U_PWD='" & Password & "' and isnull(U_ESSLogType,'E')='S'", objDA.con)
            objDA.cmd.CommandType = CommandType.Text
            status = objDA.cmd.ExecuteScalar()
            objDA.con.Close()
            If status <> "" Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
        End Try
        Return status
    End Function
    Public Function SessionDetails(ByVal CustCode As String) As Integer
        Try
            Dim exists As Integer = 0
            objDA.strQuery = "INSERT INTO U_SESSION(U_EmpCode,U_LOGIN_DATE) VALUES('" & CustCode & "',GETDATE())"
            objDA.cmd = New SqlCommand(objDA.strQuery, objDA.con)
            objDA.cmd.Connection.Open()
            objDA.cmd.ExecuteNonQuery()
            objDA.cmd.Connection.Close()
            objDA.con.Open()
            objDA.cmd = New SqlCommand("SELECT MAX(U_SESSIONID) FROM U_SESSION", objDA.con)
            objDA.cmd.CommandType = CommandType.Text
            exists = objDA.cmd.ExecuteScalar()
            objDA.con.Close()
            Return exists
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
        End Try
    End Function
    Public Function GetSAPLogin(ByVal objen As LoginEN) As DataSet
        Try
            objDA.sqlda = New SqlDataAdapter("select U_EMPUID,U_USERPWD  from [@Z_DLC_LOGIN] WHERE U_EMPID='" & objen.SessionId & "' and isnull(U_ESSLogType,'E')='S'", objDA.con)
            objDA.sqlda.Fill(objDA.Ds1)
            Return objDA.Ds1
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
        End Try
    End Function
End Class
