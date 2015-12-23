Imports System
Imports System.Globalization
Imports System.Data
Imports System.Data.SqlClient
Imports System.Net.Mail
Imports System.IO
Imports System.Data.Odbc
Imports DAL
Imports EN
Public Class MRDLCApprovalDA
    Dim objEN As PRApprovalEN = New PRApprovalEN()
    Dim objDA As DBConnectionDA = New DBConnectionDA()
    Public Sub New()
        objDA.con = New SqlConnection(objDA.GetConnection)
    End Sub
    Public Function GetUserCode(ByVal objEN As PRApprovalEN) As String
        Try
            objDA.strQuery = "select U_EMPUID from [@Z_DLC_LOGIN]  where U_EMPID='" & objEN.EmpId & "'"
            objDA.cmd = New SqlCommand(objDA.strQuery, objDA.con)
            objDA.con.Open()
            objEN.UserCode = objDA.cmd.ExecuteScalar()
            objDA.con.Close()
            Return objEN.UserCode
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            Throw ex
        End Try
    End Function
    Public Function MainGridBind(ByVal objEN As PRApprovalEN) As DataSet
        Try
            objDA.strQuery = " select distinct( T0.DocEntry) DocEntry,T0.U_Z_EmpID,T0.U_Z_EmpName,T0.U_Z_DeptCode,T1.U_Z_CurApprover,T1.U_Z_NxtApprover,Convert(Varchar(10),T0.U_Z_DocDate,103) AS U_Z_DocDate,T0.U_Z_DeptName,T0.U_Z_Destination,Case T0.""U_Z_DocStatus"" when 'O' then 'Open' when 'I' then 'InProgress' when 'S' then 'Confirm' when 'L' then 'Cancelled' when 'D' then 'Draft' when 'R' then 'DLC Rejected' when 'DI' then 'DLC InProgress' else 'Closed' end AS ""U_Z_DocStatus"",isnull(T1.U_Z_AppStatus,'P') as  U_Z_AppStatus,CONVERT(varchar(1000),T1.U_Z_RejRemark) as U_Z_Remarks  from [@Z_ORPD] T0"
            objDA.strQuery += "  Join [@Z_RPD1] T1 on T0.DocEntry=T1.DocEntry "
            objDA.strQuery += "   JOIN [@Z_DLC_OAPPT] T3 ON T3.DocEntry = T1.U_Z_ApproveId and (T1.U_Z_AppStatus='P' or T1.U_Z_AppStatus='-')"
            objDA.strQuery += "  JOIN [@Z_DLC_APPT2] T2 ON T3.DocEntry = T2.DocEntry "
            objDA.strQuery += " And (T1.U_Z_CurApprover = '" + objEN.UserCode + "' OR T1.U_Z_NxtApprover = '" + objEN.UserCode + "')"
            objDA.strQuery += " And isnull(T2.U_Z_AMan,'N')='Y' AND isnull(T3.U_Z_Active,'N')='Y' and  isnull(T1.U_Z_AppRequired,'N')='Y' and isnull(T0.U_Z_DocStatus,'O')<>'C' and  T2.U_Z_AUser = '" + objEN.UserCode + "' And T3.U_Z_DocType = 'MRD'  Order by T0.DocEntry Desc;"

            objDA.strQuery += " select distinct( T0.DocEntry) DocEntry,T0.U_Z_EmpID,T1.U_Z_CurApprover,T0.U_Z_DeptCode,T1.U_Z_NxtApprover,T0.U_Z_EmpName,Convert(Varchar(10),T0.U_Z_DocDate,103) AS U_Z_DocDate,T0.U_Z_DeptName,T0.U_Z_Destination,Case T0.""U_Z_DocStatus"" when 'O' then 'Open' when 'I' then 'InProgress' when 'S' then 'Confirm' when 'L' then 'Cancelled' when 'D' then 'Draft' when 'R' then 'DLC Rejected' when 'DI' then 'DLC InProgress' else 'Closed' end AS ""U_Z_DocStatus"",Case isnull(T1.U_Z_AppStatus,'P') when 'A' then 'Approved' when 'R' then 'Rejected' else 'Pending' end as  U_Z_AppStatus,CONVERT(varchar(1000),T1.U_Z_RejRemark) as U_Z_Remarks  from [@Z_ORPD] T0"
            objDA.strQuery += " Join [@Z_RPD1] T1 on T0.DocEntry=T1.DocEntry "
            objDA.strQuery += "   JOIN [@Z_DLC_OAPPT] T3 ON T3.DocEntry = T1.U_Z_ApproveId "
            objDA.strQuery += "  JOIN [@Z_DLC_APPT2] T2 ON T3.DocEntry = T2.DocEntry "
            objDA.strQuery += " And (T1.U_Z_CurApprover = '" + objEN.UserCode + "' OR T1.U_Z_NxtApprover = '" + objEN.UserCode + "')"
            objDA.strQuery += " And isnull(T2.U_Z_AMan,'N')='Y' AND isnull(T3.U_Z_Active,'N')='Y' and  isnull(T1.U_Z_AppRequired,'N')='Y' and  T2.U_Z_AUser = '" + objEN.UserCode + "' And T3.U_Z_DocType = 'MRD' Order by T0.DocEntry Desc;"
            objDA.strQuery += "Select WhsCode,WhsName  from OWHS;"
            objDA.sqlda = New SqlDataAdapter(objDA.strQuery, objDA.con)
            objDA.sqlda.Fill(objDA.ds)
            Return objDA.ds
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            Throw ex
        End Try
    End Function
    Public Function BindPRequestApproval(ByVal objEN As PRApprovalEN) As DataSet
        Try
            objDA.strQuery = " Select T0.U_Z_DocRefNo,T0.LineId,T0.DocEntry,T0.U_Z_ItemCode,T0.U_Z_ItemName,T0.U_Z_OrdQty,T0.U_Z_OrdUomDesc,T0.U_Z_OrdUom,Isnull(T0.U_Z_AppStatus,'P') AS U_Z_AppStatus,"
            objDA.strQuery += "U_Z_BarCode,U_Z_LineStatus,U_Z_CurApprover,U_Z_NxtApprover,"
            objDA.strQuery += " Case U_Z_AppRequired when 'Y' then 'Yes' else 'No' End as  'U_Z_AppRequired',T0.U_Z_RejRemark,Convert(Varchar(10),T0.U_Z_AppReqDate,103) AS U_Z_AppReqDate,CONVERT(VARCHAR(8),U_Z_ReqTime,108) AS 'U_Z_ReqTime'"
            objDA.strQuery += " From [@Z_RPD1] T0 Left Outer Join [@Z_ORPD] T5 on T0.DocEntry=T5.DocEntry and T5.DocEntry='" & objEN.DocEntry & "'"
            objDA.strQuery += "   JOIN [@Z_DLC_OAPPT] T3 ON T3.DocEntry = T0.U_Z_ApproveId and (T0.U_Z_AppStatus='P' or T0.U_Z_AppStatus='-')"
            objDA.strQuery += "  JOIN [@Z_DLC_APPT2] T2 ON T3.DocEntry = T2.DocEntry "
            objDA.strQuery += " And (T0.U_Z_CurApprover = '" + objEN.UserCode + "' OR T0.U_Z_NxtApprover = '" + objEN.UserCode + "')"
            objDA.strQuery += " And isnull(T2.U_Z_AMan,'N')='Y' AND isnull(T3.U_Z_Active,'N')='Y' and  isnull(T0.U_Z_AppRequired,'N')='Y' and  T2.U_Z_AUser = '" + objEN.UserCode + "' And T3.U_Z_DocType = 'MRD' where T0.DocEntry='" & objEN.DocEntry & "' Order by Convert(Numeric,T0.DocEntry) Desc;"
            objDA.strQuery += " Select T0.DocEntry,T0.U_Z_EmpID,T0.U_Z_EmpName,Convert(Varchar(10),T0.U_Z_DocDate,103) AS U_Z_DocDate,T0.U_Z_DeptCode,T0.U_Z_DeptName,U_Z_Destination,Case isnull(T0.U_Z_DocStatus,'O') when 'O' then 'Open' when 'I' then 'InProgress' else 'Closed' end AS U_Z_DocStatus from [@Z_ORPD] T0 where DocEntry='" & objEN.DocEntry & "';"
            objDA.sqlda = New SqlDataAdapter(objDA.strQuery, objDA.con)
            objDA.sqlda.Fill(objDA.Ds1)
            Return objDA.Ds1
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            Throw ex
        End Try
    End Function
    Public Function LoadHistory(ByVal objEN As PRApprovalEN) As DataSet
        Try
            objDA.strQuery = " Select DocEntry,U_Z_DocEntry,U_Z_DocType,U_Z_EmpId,U_Z_EmpName,U_Z_ItemCode,U_Z_ItemName,U_Z_OrdQty,U_Z_delUomDesc,U_Z_ApproveBy,convert(varchar(10),CreateDate,103) as CreateDate,CreateTime, convert(varchar(10),UpdateDate,103) as UpdateDate,UpdateTime,Case U_Z_AppStatus when 'O' then 'Open' when 'D' then 'Delivered' when 'C' then 'Close' when 'L' then 'Cancelled' end AS U_Z_AppStatus,U_Z_Remarks From [@Z_DLC_APHIS] "
            objDA.strQuery += " Where U_Z_DocType = 'MRD' And U_Z_DocEntry = '" & objEN.Code & "' and isnull(U_Z_DLineId,'')='" & objEN.HeadLineId & "'"
            objDA.sqlda = New SqlDataAdapter(objDA.strQuery, objDA.con)
            objDA.sqlda.Fill(objDA.Ds2)
            Return objDA.Ds2
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            Throw ex
        End Try
    End Function
    Public Function BindExpenseSummaryApproval(ByVal objEN As PRApprovalEN) As DataSet
        Try
            objDA.strQuery = " Select T0.U_Z_DocRefNo,T0.LineId,T0.DocEntry,T0.U_Z_ItemCode,T0.U_Z_ItemName,T0.U_Z_OrdQty,T0.U_Z_OrdUomDesc,T0.U_Z_OrdUom,Case Isnull(T0.U_Z_AppStatus,'P') when 'A' then 'Approved' when 'R' then 'Rejected' else 'Pending' end AS U_Z_AppStatus,"
            objDA.strQuery += "U_Z_BarCode,U_Z_LineStatus,U_Z_CurApprover,U_Z_NxtApprover,"
            objDA.strQuery += " Case U_Z_AppRequired when 'Y' then 'Yes' else 'No' End as  'U_Z_AppRequired',T0.U_Z_RejRemark,Convert(Varchar(10),T0.U_Z_AppReqDate,103) AS U_Z_AppReqDate,CONVERT(VARCHAR(8),U_Z_ReqTime,108) AS 'U_Z_ReqTime'"
            objDA.strQuery += " From [@Z_RPD1] T0 Left Outer Join [@Z_ORPD] T5 on T0.DocEntry=T5.DocEntry and T5.DocEntry='" & objEN.DocEntry & "'"
            objDA.strQuery += "   JOIN [@Z_DLC_OAPPT] T3 ON T3.DocEntry = T0.U_Z_ApproveId"
            objDA.strQuery += "  JOIN [@Z_DLC_APPT2] T2 ON T3.DocEntry = T2.DocEntry "
            objDA.strQuery += " And (T0.U_Z_CurApprover = '" + objEN.UserCode + "' OR T0.U_Z_NxtApprover = '" + objEN.UserCode + "')"
            objDA.strQuery += " And isnull(T2.U_Z_AMan,'N')='Y' AND isnull(T3.U_Z_Active,'N')='Y' and T2.U_Z_AUser = '" + objEN.UserCode + "' And T3.U_Z_DocType = 'MRD' where T0.DocEntry='" & objEN.DocEntry & "' Order by T0.DocEntry Desc;"
            objDA.sqlda = New SqlDataAdapter(objDA.strQuery, objDA.con)
            objDA.sqlda.Fill(objDA.Ds3)
            Return objDA.Ds3
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            Throw ex
        End Try
    End Function
End Class
