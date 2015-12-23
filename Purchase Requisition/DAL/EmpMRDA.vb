Imports System
Imports System.Globalization
Imports System.Web.UI.WebControls
Imports System.Data
Imports System.Data.SqlClient
Imports EN
Public Class EmpMRDA
    Dim objen As EmpPREN = New EmpPREN()
    Dim objDA As DBConnectionDA = New DBConnectionDA()
    Dim stSubdt As String
    Dim dtsubdt As Date
    Public Sub New()
        objDA.con = New SqlConnection(objDA.GetConnection)
    End Sub
    Public Function PageLoadBind(ByVal objen As EmpPREN) As DataSet
        Try
            objDA.strQuery = "Select dept,isnull(Remarks,0) as DeptName  from OHEM T0 left join OUDP T1 on T0.dept=t1.Code  where empID=" & objen.EmpId & ";"
            objDA.strQuery += "SELECT T0.""DocEntry"",T0.""U_Z_EmpID"",T0.""U_Z_EmpName"",convert(Varchar(10),T0.""U_Z_DocDate"",103) AS ""U_Z_DocDate"", T0.""U_Z_DeptName"", T0.""U_Z_Destination"","
            objDA.strQuery += " Case T0.""U_Z_DocStatus"" when 'O' then 'Open' when 'I' then 'InProgress' when 'S' then 'Confirm' when 'L' then 'Cancelled' when 'D' then 'Draft' when 'R' then 'DLC Rejected' when 'DI' then 'DLC InProgress' else 'Closed' end AS ""U_Z_DocStatus"" FROM ""@Z_ORPD"" T0 WHERE T0.""U_Z_EmpID"" ='" & objen.EmpId & "' order by T0.""DocEntry"" desc;"
            objDA.sqlda = New SqlDataAdapter(objDA.strQuery, objDA.con)
            objDA.sqlda.Fill(objDA.ds)
            Return objDA.ds
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            Throw ex
        End Try
    End Function
    Public Function BindItemCode(ByVal objen As EmpPREN) As DataSet
        Try
            objDA.strQuery = "Select U_AllItemCat  from [@Z_DLC_LOGIN] T0 where U_EMPID=" & objen.EmpId & ""
            objDA.sqlda = New SqlDataAdapter(objDA.strQuery, objDA.con)
            objDA.sqlda.Fill(objDA.Ds1)
            If objDA.Ds1.Tables(0).Rows.Count > 0 Then
                If objDA.Ds1.Tables(0).Rows(0)(0).ToString() = "Y" Then
                    objDA.strQuery = "Select ItemCode,ItemName,isnull(CodeBars,'0') as CodeBars  from OITM where isnull(U_CatCode,'')<>''"
                    objDA.sqlda = New SqlDataAdapter(objDA.strQuery, objDA.con)
                    objDA.sqlda.Fill(objDA.Ds2)
                    Return objDA.Ds2
                Else
                    objDA.strQuery = "select ItemCode,ItemName,isnull(CodeBars,'0') as CodeBars from OITM T0 left Join [@Z_ITCAT1] T1 on T0.U_CatCode=T1.U_CatCode "
                    objDA.strQuery += "  inner join [@Z_DLC_LOGIN] T2 on T1.DocEntry=T2.DocEntry where T2.U_EMPID = " & objen.EmpId & ""
                    objDA.sqlda = New SqlDataAdapter(objDA.strQuery, objDA.con)
                    objDA.sqlda.Fill(objDA.Ds2)
                    Return objDA.Ds2
                End If
            End If
            Return objDA.Ds2
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
        End Try
    End Function
    Public Function BindUom(ByVal objen As EmpPREN) As DataSet
        Try
            objDA.strQuery = "SELECT  T2.[UomEntry] as UomEntry, T3.[UomCode] as UomCode FROM OITM T0  "
            objDA.strQuery += " INNER JOIN OUGP T1 ON T0.UgpEntry = T1.UgpEntry INNER JOIN UGP1 T2 ON T1.UgpEntry = T2.UgpEntry "
            objDA.strQuery += " INNER JOIN OUOM T3 ON T3.UoMEntry = T2.UomEntry WHERE T0.[ItemCode]='" & objen.ItemCode & "'"
            objDA.sqlda = New SqlDataAdapter(objDA.strQuery, objDA.con)
            objDA.sqlda.Fill(objDA.dss)
            Return objDA.dss
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
        End Try
    End Function
    Public Function BindWhs(ByVal objen As EmpPREN) As DataSet
        Try
            objDA.strQuery = "Select U_AllWhs  from [@Z_DLC_LOGIN] T0 where U_EMPID=" & objen.EmpId & ""
            objDA.sqlda = New SqlDataAdapter(objDA.strQuery, objDA.con)
            objDA.sqlda.Fill(objDA.Ds3)
            If objDA.Ds3.Tables(0).Rows.Count > 0 Then
                If objDA.Ds3.Tables(0).Rows(0)(0).ToString() = "Y" Then
                    objDA.strQuery = "Select WhsCode,WhsName  from OWHS where U_ESSWhs='Y'"
                    objDA.sqlda = New SqlDataAdapter(objDA.strQuery, objDA.con)
                    objDA.sqlda.Fill(objDA.dss2)
                    Return objDA.dss2
                Else
                    objDA.strQuery = "select WhsCode,WhsName from OWHS T0 left Join [@Z_ITCAT2] T1 on T0.WhsCode=T1.U_whsCode"
                    objDA.strQuery += "  inner join [@Z_DLC_LOGIN] T2 on T1.DocEntry=T2.DocEntry where T2.U_EMPID = " & objen.EmpId & ""
                    objDA.sqlda = New SqlDataAdapter(objDA.strQuery, objDA.con)
                    objDA.sqlda.Fill(objDA.dss2)
                    Return objDA.dss2
                End If
            End If
            Return objDA.dss2
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
        End Try
      
    End Function
    Public Function DeleteTempTable(ByVal objen As EmpPREN) As String
        Try
            objDA.strQuery = "Delete from ""U_ORPD""   where ""EmpId""='" & objen.EmpId & "';"
            objDA.strQuery += "Update  ""@Z_RPD1"" set ""U_Z_ItemCode""=REPLACE(T0.""U_Z_ItemCode"",'D','')  from ""@Z_RPD1"" T0 JOIN ""@Z_ORPD"" T1 on T0.DocEntry=T1.DocEntry"
            objDA.strQuery += " where T1.U_Z_EmpID='1' and T0.""U_Z_ItemCode"" Like '%D'; "
            objDA.cmd = New SqlCommand(objDA.strQuery, objDA.con)
            objDA.con.Open()
            objDA.cmd.ExecuteNonQuery()
            objDA.con.Close()
            objDA.strmsg = "Success"
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            objDA.strmsg = ex.Message
        End Try
        Return objDA.strmsg
    End Function
    Public Function LoadHistory(ByVal objEN As EmpPREN) As DataSet
        Try
            objDA.strQuery = " Select DocEntry,U_Z_DocEntry,U_Z_DocType,U_Z_EmpId,U_Z_EmpName,U_Z_ItemCode,U_Z_ItemName,U_Z_OrdQty,U_Z_delUomDesc,U_Z_ApproveBy,convert(varchar(10),CreateDate,103) as CreateDate,CreateTime, convert(varchar(10),UpdateDate,103) as UpdateDate,UpdateTime,Case U_Z_AppStatus when 'O' then 'Open' when 'D' then 'Delivered' when 'C' then 'Close' when 'L' then 'Cancelled' when 'R' then 'DLC Rejected' when 'A' then 'DLC Approved' end AS U_Z_AppStatus,U_Z_Remarks From [@Z_DLC_APHIS] "
            objDA.strQuery += " Where (U_Z_DocType = 'MRD' or U_Z_DocType='MR') And U_Z_DocEntry = '" & objEN.Code & "' and (isnull(U_Z_DLineId,'')='" & objEN.ItemSpec & "' or isnull(U_Z_DLineId,'')='')"
            objDA.sqlda = New SqlDataAdapter(objDA.strQuery, objDA.con)
            objDA.sqlda.Fill(objDA.Ds2)
            Return objDA.Ds2
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            Throw ex
        End Try
    End Function
    Public Function TempLines(ByVal objen As EmpPREN) As DataSet
        Try
            objDA.strQuery = "Select case LineStatus when 'O' then 'Open' when 'D' then 'Delivered' when 'L' then 'Cancelled' end as 'LineDesc',"
            objDA.strQuery += " Case AppStatus when 'P' then 'Pending' when 'A' then 'Approved' when 'R' then 'Rejected' end as 'AppDesc',*  from [U_ORPD]  where SessionId=" & objen.SessionId & " and EmpId='" & objen.EmpId & "'"
            objDA.sqlda = New SqlDataAdapter(objDA.strQuery, objDA.con)
            objDA.sqlda.Fill(objDA.dss1)
            Return objDA.dss1
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
        End Try
    End Function
    Public Function InsertLines(ByVal objen As EmpPREN) As String
        Try
            objDA.strQuery = "Insert Into [U_ORPD] (EmpId,Empname,DocDate,DefWhs,DeptCode,DeptName,DocNum,DocStatus,ItemCode,ItemName,OrderQty,OrderUom,Barcode,LineStatus,SessionId,"
            objDA.strQuery += " OrderUomDesc,AppStatus,NewDocStatus) Values ('" & objen.EmpId & "','" & objen.EmpName & "',getdate(),'" & objen.Defwhs & "','" & objen.DeptCode & "','" & objen.DeptName & "','" & objen.DocNo & "','" & objen.DocStatus & "','" & objen.ItemCode & "','" & objen.Itemdesc & "',"
            objDA.strQuery += " '" & objen.OrdrQty & "','" & objen.OrdrUom & "'"
            objDA.strQuery += " ,'" & objen.Barcode & "','O','" & objen.SessionId & "' ,'" & objen.OrdrUomDesc & "','P','N')"
            objDA.cmd = New SqlCommand(objDA.strQuery, objDA.con)
            objDA.con.Open()
            objDA.cmd.ExecuteNonQuery()
            objDA.con.Close()
            objDA.strmsg = "Success"
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            objDA.strmsg = ex.Message
            Return objDA.strmsg
        End Try
        Return objDA.strmsg
    End Function
    Public Function PopulateExistingDocument(ByVal objen As EmpPREN) As String
        Try
            objDA.strQuery = "Select convert(varchar(10),T0.U_Z_DocDate,103),T0.DocEntry as 'DocNum',*  from [@Z_ORPD] T0 inner join [@Z_RPD1] T1 on T0.DocEntry=T1.DocEntry where T0.DocEntry='" & objen.DocNo & "'"
            objDA.sqlda = New SqlDataAdapter(objDA.strQuery, objDA.con)
            objDA.sqlda.Fill(objDA.dss4)
            If objDA.dss4.Tables(0).Rows.Count > 0 Then
                For introw As Integer = 0 To objDA.dss4.Tables(0).Rows.Count - 1
                    stSubdt = objDA.dss4.Tables(0).Rows(introw)(0).ToString()
                    If stSubdt <> "" Then
                        dtsubdt = Date.ParseExact(stSubdt.Trim().Replace("-", "/").Replace(".", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture)
                    Else
                        dtsubdt = Now.Date
                    End If
                    objDA.strQuery = "Insert Into [U_ORPD] (DocNum,RefNo,SessionId,Empid,Empname,DocDate,DefWhs,DeptCode,DeptName,DocStatus,"
                    objDA.strQuery += " ItemCode,ItemName,OrderQty,OrderUom,OrderUomDesc,LineStatus,Barcode,AppStatus,NewDocStatus,GoodsReceipt)"
                    objDA.strQuery += "  Values ('" & objDA.dss4.Tables(0).Rows(introw)("LineId").ToString() & "','" & objDA.dss4.Tables(0).Rows(introw)("DocNum").ToString() & "','" & objen.SessionId & "',"
                    objDA.strQuery += " '" & objDA.dss4.Tables(0).Rows(introw)("U_Z_EmpID").ToString() & "','" & objDA.dss4.Tables(0).Rows(introw)("U_Z_EmpName").ToString() & "','" & dtsubdt.ToString("yyyy-MM-dd") & "','" & objDA.dss4.Tables(0).Rows(introw)("U_Z_Destination").ToString() & "', "
                    objDA.strQuery += " '" & objDA.dss4.Tables(0).Rows(introw)("U_Z_DeptCode").ToString() & "','" & objDA.dss4.Tables(0).Rows(introw)("U_Z_DeptName").ToString() & "','" & objDA.dss4.Tables(0).Rows(introw)("U_Z_DocStatus").ToString() & "', "
                    objDA.strQuery += " '" & objDA.dss4.Tables(0).Rows(introw)("U_Z_ItemCode").ToString() & "','" & objDA.dss4.Tables(0).Rows(introw)("U_Z_ItemName").ToString() & "',"
                    objDA.strQuery += " '" & objDA.dss4.Tables(0).Rows(introw)("U_Z_OrdQty").ToString() & "','" & objDA.dss4.Tables(0).Rows(introw)("U_Z_OrdUom").ToString() & "','" & objDA.dss4.Tables(0).Rows(introw)("U_Z_OrdUomDesc").ToString() & "',"
                    objDA.strQuery += " '" & objDA.dss4.Tables(0).Rows(introw)("U_Z_LineStatus").ToString().Trim() & "','" & objDA.dss4.Tables(0).Rows(introw)("U_Z_BarCode").ToString() & "', "
                    objDA.strQuery += " '" & objDA.dss4.Tables(0).Rows(introw)("U_Z_AppStatus").ToString().Trim() & "','" & objDA.dss4.Tables(0).Rows(introw)("U_Z_NewDoc").ToString().Trim() & "','" & objDA.dss4.Tables(0).Rows(introw)("U_Z_DocRefNo").ToString().Trim() & "')"
                    objDA.cmd = New SqlCommand(objDA.strQuery, objDA.con)
                    objDA.con.Open()
                    objDA.cmd.ExecuteNonQuery()
                    objDA.con.Close()
                    objDA.strmsg = "Success"
                Next
            End If
            objDA.strmsg = "Success"
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            objDA.strmsg = ex.Message
        End Try
        Return objDA.strmsg
    End Function
    Public Function BindPRequestApproval(ByVal objEN As EmpPREN) As DataSet
        Try
            objDA.strQuery = "Select case LineStatus when 'O' then 'Open' when 'D' then 'Delivered' when 'C' then 'Close' when 'L' then 'Cancelled' end as 'LineDesc',"
            objDA.strQuery += " Case AppStatus when 'P' then 'Pending' when 'A' then 'Approved' when 'R' then 'Rejected' end as 'AppDesc', *  from [U_ORPD]  where SessionId=" & objEN.SessionId & " and EmpId='" & objEN.EmpId & "' and (isnull(RefNo,'')='" & objEN.DocNo & "' or isnull(RefNo,'')='');"
            objDA.strQuery += " Select T0.DocEntry,T0.U_Z_EmpID,T0.U_Z_EmpName,Convert(Varchar(10),T0.U_Z_DocDate,103) AS U_Z_DocDate,T0.U_Z_DeptCode,T0.U_Z_DeptName,U_Z_Destination,Case T0.""U_Z_DocStatus"" when 'O' then 'Open' when 'I' then 'InProgress' when 'S' then 'Confirm' when 'L' then 'Cancelled' when 'D' then 'Draft' when 'R' then 'DLC Rejected' when 'DI' then 'DLC InProgress' else 'Closed' end AS ""U_Z_DocStatus"" from [@Z_ORPD] T0 where T0.DocEntry='" & objEN.DocNo & "';"
            objDA.sqlda = New SqlDataAdapter(objDA.strQuery, objDA.con)
            objDA.sqlda.Fill(objDA.Ds1)
            Return objDA.Ds1
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            Throw ex
        End Try
    End Function
    Public Function DeleteLinesTable(ByVal objen As EmpPREN) As String
        Try
            objDA.strQuery = "Delete from ""U_ORPD""   where ""EmpId""='" & objen.EmpId & "' and UniqueId='" & objen.DocNo & "';"
            objDA.strQuery += "Update  ""@Z_RPD1"" set ""U_Z_ItemCode""=isnull(""U_Z_ItemCode"",'') + 'D'  where ""DocEntry""='" & objen.Code & "' and LineId='" & objen.LineId & "';"
            objDA.cmd = New SqlCommand(objDA.strQuery, objDA.con)
            objDA.con.Open()
            objDA.cmd.ExecuteNonQuery()
            objDA.con.Close()
            objDA.strmsg = "Success"
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            objDA.strmsg = ex.Message
        End Try
        Return objDA.strmsg
    End Function
    Public Function CancelRequest(ByVal objen As EmpPREN) As String
        Try

            objDA.strQuery = "Update ""@Z_ORPD"" set U_Z_DocStatus='L' where ""DocEntry""='" & objen.Code & "';"
            objDA.cmd = New SqlCommand(objDA.strQuery, objDA.con)
            objDA.con.Open()
            objDA.cmd.ExecuteNonQuery()
            objDA.con.Close()
            objDA.strmsg = "Success"
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            objDA.strmsg = ex.Message
        End Try
        Return objDA.strmsg
    End Function
End Class
