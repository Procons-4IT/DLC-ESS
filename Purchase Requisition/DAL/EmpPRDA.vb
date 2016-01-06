Imports System
Imports System.Globalization
Imports System.Web.UI.WebControls
Imports System.Data
Imports System.Data.SqlClient
Imports EN
Public Class EmpPRDA
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
            objDA.strQuery += " Case T0.""U_Z_DocStatus"" when 'O' then 'Open' when 'I' then 'InProgress' when 'S' then 'Confirm' when 'L' then 'Cancelled' when 'D' then 'Draft' when 'R' then 'DLC Rejected' when 'DI' then 'DLC InProgress' else 'Closed' end AS ""U_Z_DocStatus"""
            objDA.strQuery += " , Case T0.""U_Z_Priority"" when 'L' then 'Low' when 'M' then 'Medium' when 'H' then 'High' end as U_Z_Priority FROM  ""@Z_OPRQ"" T0 WHERE T0.""U_Z_EmpID"" ='" & objen.EmpId & "' order by T0.""DocEntry"" desc;"
            objDA.sqlda = New SqlDataAdapter(objDA.strQuery, objDA.con)
            objDA.sqlda.Fill(objDA.ds)
            Return objDA.ds
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            Throw ex
        End Try
    End Function
    Public Function GetCostCenter(ByVal objen As EmpPREN) As String
        Dim status As String = ""
        Try
            objDA.con.Open()
            objDA.cmd = New SqlCommand("select U_Dimension  from [@Z_DLC_LOGIN] WHERE   U_EMPID='" & objen.EmpId & "'", objDA.con)
            objDA.cmd.CommandType = CommandType.Text
            status = objDA.cmd.ExecuteScalar()
            objDA.con.Close()
            Return status
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
        End Try
        Return status
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
    Public Function InsertLines(ByVal objen As EmpPREN) As String
        Try
            objDA.strQuery = "Insert Into [U_OPRQ] (EmpId,Empname,DocDate,DefWhs,DeptCode,DeptName,DocNum,DocStatus,OrdPatient,ItemCode,ItemName,ItemSpec,OrderQty,OrderUom,AltItemCode,AltItemDesc,DelQty,DelUom,Barcode,LineStatus,SessionId,"
            objDA.strQuery += " OrderUomDesc,DelUomDesc,AppStatus,NewDocStatus,CostCenter) Values ('" & objen.EmpId & "','" & objen.EmpName & "',getdate(),'" & objen.Defwhs & "','" & objen.DeptCode & "','" & objen.DeptName & "','" & objen.DocNo & "','" & objen.DocStatus & "','" & objen.OrdrPatient & "','" & objen.ItemCode & "','" & objen.Itemdesc & "','" & objen.ItemSpec & "',"
            objDA.strQuery += " '" & objen.OrdrQty & "','" & objen.OrdrUom & "','" & objen.ItemCode & "','" & objen.Itemdesc & "','" & objen.OrdrQty & "','" & objen.OrdrUom & "'"
            objDA.strQuery += " ,'" & objen.Barcode & "','O','" & objen.SessionId & "' ,'" & objen.OrdrUomDesc & "','" & objen.OrdrUomDesc & "','P','N','" & objen.CostCenter & "')"
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
            objDA.strQuery = "Select convert(varchar(10),T0.U_Z_DocDate,103),T0.DocEntry as 'DocNum',*  from [@Z_OPRQ] T0 inner join [@Z_PRQ1] T1 on T0.DocEntry=T1.DocEntry where T0.DocEntry='" & objen.DocNo & "'"
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
                    objDA.strQuery = "Insert Into [U_OPRQ] (DocNum,RefNo,SessionId,Empid,Empname,DocDate,DefWhs,DeptCode,DeptName,DocStatus,"
                    objDA.strQuery += " OrdPatient,ItemCode,ItemName,OrderQty,OrderUom,OrderUomDesc,AltItemCode,AltItemDesc,DelQty,DelUom,DelUomDesc,"
                    objDA.strQuery += " ReceivedQty,ReceivedUom,ReceivedUomDesc,LineStatus,Barcode,AlterBarCode,AppStatus,NewDocStatus,CostCenter) Values ('" & objDA.dss4.Tables(0).Rows(introw)("LineId").ToString() & "','" & objDA.dss4.Tables(0).Rows(introw)("DocNum").ToString() & "','" & objen.SessionId & "',"
                    objDA.strQuery += " '" & objDA.dss4.Tables(0).Rows(introw)("U_Z_EmpID").ToString() & "','" & objDA.dss4.Tables(0).Rows(introw)("U_Z_EmpName").ToString() & "','" & dtsubdt.ToString("yyyy-MM-dd") & "','" & objDA.dss4.Tables(0).Rows(introw)("U_Z_Destination").ToString() & "', "
                    objDA.strQuery += " '" & objDA.dss4.Tables(0).Rows(introw)("U_Z_DeptCode").ToString() & "','" & objDA.dss4.Tables(0).Rows(introw)("U_Z_DeptName").ToString() & "','" & objDA.dss4.Tables(0).Rows(introw)("U_Z_DocStatus").ToString() & "', "
                    objDA.strQuery += " '" & objDA.dss4.Tables(0).Rows(introw)("U_Z_OrdPatient").ToString() & "','" & objDA.dss4.Tables(0).Rows(introw)("U_Z_ItemCode").ToString() & "','" & objDA.dss4.Tables(0).Rows(introw)("U_Z_ItemName").ToString() & "',"
                    objDA.strQuery += " '" & objDA.dss4.Tables(0).Rows(introw)("U_Z_OrdQty").ToString() & "','" & objDA.dss4.Tables(0).Rows(introw)("U_Z_OrdUom").ToString() & "','" & objDA.dss4.Tables(0).Rows(introw)("U_Z_OrdUomDesc").ToString() & "','" & objDA.dss4.Tables(0).Rows(introw)("U_Z_AltItemCode").ToString() & "', "
                    objDA.strQuery += " '" & objDA.dss4.Tables(0).Rows(introw)("U_Z_AltItemName").ToString() & "','" & objDA.dss4.Tables(0).Rows(introw)("U_Z_DeliQty").ToString() & "','" & objDA.dss4.Tables(0).Rows(introw)("U_Z_DelUom").ToString() & "','" & objDA.dss4.Tables(0).Rows(introw)("U_Z_DelUomDesc").ToString() & "',"
                    objDA.strQuery += " '" & objDA.dss4.Tables(0).Rows(introw)("U_Z_RecQty").ToString() & "','" & objDA.dss4.Tables(0).Rows(introw)("U_Z_RecUom").ToString() & "','" & objDA.dss4.Tables(0).Rows(introw)("U_Z_RecUomDesc").ToString() & "','" & objDA.dss4.Tables(0).Rows(introw)("U_Z_LineStatus").ToString().Trim() & "', "
                    objDA.strQuery += " '" & objDA.dss4.Tables(0).Rows(introw)("U_Z_BarCode").ToString() & "','" & objDA.dss4.Tables(0).Rows(introw)("U_Z_AltBarCode").ToString() & "', "
                    objDA.strQuery += " '" & objDA.dss4.Tables(0).Rows(introw)("U_Z_AppStatus").ToString().Trim() & "','E','" & objDA.dss4.Tables(0).Rows(introw)("U_Dimension").ToString().Trim() & "')"
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
    Public Function TempLines(ByVal objen As EmpPREN) As DataSet
        Try
            objDA.strQuery = "Select case LineStatus when 'O' then 'Open' when 'D' then 'Delivered' when 'C' then 'Close' when 'L' then 'Cancelled' end as 'LineDesc',"
            objDA.strQuery += " Case AppStatus when 'P' then 'Pending' when 'A' then 'Approved' when 'R' then 'Rejected' end as 'AppDesc',CAST(OrderQty AS int) AS OrderQty,CAST(DelQty AS int) AS DelQty, *  from [U_OPRQ]  where SessionId=" & objen.SessionId & " and EmpId='" & objen.EmpId & "'"
            objDA.sqlda = New SqlDataAdapter(objDA.strQuery, objDA.con)
            objDA.sqlda.Fill(objDA.dss1)
            Return objDA.dss1
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
            objDA.strQuery = "Delete from ""U_OPRQ""   where ""EmpId""='" & objen.EmpId & "'"
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
    Public Function BindPRequestApproval(ByVal objEN As EmpPREN) As DataSet
        Try
            objDA.strQuery = "Select case LineStatus when 'O' then 'Open' when 'D' then 'Delivered' when 'C' then 'Close' when 'L' then 'Cancelled' end as 'LineDesc',"
            objDA.strQuery += " Case AppStatus when 'P' then 'Pending' when 'A' then 'Approved' when 'R' then 'Rejected' end as 'AppDesc',CAST(OrderQty AS int) AS OrderQty,CAST(DelQty AS int) AS DelQty,CAST(ReceivedQty AS Int) AS ReceivedQty, *  from [U_OPRQ]  where SessionId=" & objEN.SessionId & " and EmpId='" & objEN.EmpId & "' and (isnull(RefNo,'')='" & objEN.DocNo & "' or isnull(RefNo,'')='');"
            objDA.strQuery += " Select T0.DocEntry,T0.U_Z_EmpID,T0.U_Z_EmpName,Convert(Varchar(10),T0.U_Z_DocDate,103) AS U_Z_DocDate,T0.U_Z_DeptCode,T0.U_Z_DeptName,U_Z_Destination,Case isnull(T0.""U_Z_DocStatus"",'O') when 'O' then 'Open' when 'I' then 'InProgress' when 'S' then 'Confirm' when 'L' then 'Cancelled' when 'D' then 'Draft' when 'R' then 'DLC Rejected' when 'DI' then 'DLC InProgress'  else 'Closed' end AS U_Z_DocStatus,U_Z_Priority,U_Dimension from [@Z_OPRQ] T0 where DocEntry='" & objEN.DocNo & "';"
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
            objDA.strQuery = "Delete from ""U_OPRQ""   where ""EmpId""='" & objen.EmpId & "' and UniqueId='" & objen.DocNo & "';"
            objDA.strQuery += "Update  ""@Z_PRQ1"" set ""U_Z_ItemCode""=isnull(""U_Z_ItemCode"",'') + 'D'  where ""DocEntry""='" & objen.Code & "' and LineId='" & objen.LineId & "';"
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
    Public Function UpdateReceived(ByVal objen As EmpPREN) As String
        Dim strMessage, strSubject As String
        Try
            objDA.strQuery = "Update  ""@Z_PRQ1"" set U_Z_RecQty='" & objen.OrdrQty & "',U_Z_RecUom='" & objen.OrdrUom & "',U_Z_RecUomDesc='" & objen.OrdrUomDesc & "'  where ""DocEntry""='" & objen.DocNo & "' and LineId='" & objen.Code & "';"
            objDA.cmd = New SqlCommand(objDA.strQuery, objDA.con)
            objDA.con.Open()
            objDA.cmd.ExecuteNonQuery()
            objDA.con.Close()
            objDA.strmsg = "Success"
            If objen.DelQty = objen.OrdrQty Then
            Else
                objDA.strQuery = "Select U_Z_ApproveBy from [@Z_DLC_APHIS] where (U_Z_AppStatus='D' or U_Z_AppStatus='C') and U_Z_DocEntry='" & objen.DocNo & "' and U_Z_DLineId='" & objen.Code & "'"
                objDA.Ds3 = objDA.GetData(objDA.strQuery)
                If objDA.Ds3.Tables(0).Rows.Count > 0 Then
                    objDA.strQuery = "Select * from [@Z_PRQ1] where DocEntry = '" & objen.DocNo & "' and LineID='" & objen.Code & "'"
                    objDA.Ds4 = objDA.GetData(objDA.strQuery)
                    If objDA.Ds4.Tables(0).Rows.Count > 0 Then
                        strMessage = "Purchase Requisition Quantity differences for the request number is :" & objen.DocNo & "" & vbCrLf & _
                            ", Line number is :" & objen.Code & " " & vbCrLf & " ItemCode :" & objDA.Ds4.Tables(0).Rows(0)("U_Z_AltItemCode").ToString() & "" & vbCrLf & _
                             " , ItemName :" & objDA.Ds4.Tables(0).Rows(0)("U_Z_AltItemName").ToString() & "" & vbCrLf & " Delivered Quantity :" & objDA.Ds4.Tables(0).Rows(0)("U_Z_DeliQty").ToString() & "" & vbCrLf & _
                             " , Rceived Quantity is :" & objen.OrdrQty & " "
                    End If
                    '  strMessage = "The Quantity differences in ItemCode is :" & objen.ItemCode & " and Delivered Quantity is :" & objen.DelQty & " and Rceived Quantity is :" & objen.OrdrQty
                    strSubject = "Quantity difference in purchase requisition"
                    objDA.SendMail_Approval(strMessage, "", objDA.Ds3.Tables(0).Rows(0)("U_Z_ApproveBy").ToString(), objen.SAPCompany, strSubject)
                End If
            End If
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            objDA.strmsg = ex.Message
        End Try
        Return objDA.strmsg
    End Function
    Public Function UpdateStatus(ByVal objen As EmpPREN) As String
        Try
            If objDA.con.State = ConnectionState.Open Then
                objDA.con.Close()
            End If
            objDA.strQuery = "Update  ""@Z_OPRQ"" set U_Z_DocStatus='" & objen.DocStatus & "'  where ""DocEntry""='" & objen.Code & "' and U_Z_EmpID='" & objen.EmpId & "';"
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
            objDA.strQuery = " Select DocEntry,U_Z_DocEntry,U_Z_DocType,U_Z_EmpId,U_Z_EmpName,U_Z_ItemCode,U_Z_ItemName,CAST(U_Z_OrdQty AS Int) AS U_Z_OrdQty,U_Z_delUomDesc,U_Z_ApproveBy,convert(varchar(10),CreateDate,103) as CreateDate,CreateTime, convert(varchar(10),UpdateDate,103) as UpdateDate,UpdateTime,Case U_Z_AppStatus when 'O' then 'Open' when 'D' then 'Delivered' when 'C' then 'Close' when 'L' then 'Cancelled' when 'R' then 'DLC Rejected' when 'A' then 'DLC Approved' end AS U_Z_AppStatus,U_Z_Remarks From [@Z_DLC_APHIS] "
            objDA.strQuery += " Where (U_Z_DocType = 'PRD' or U_Z_DocType='PR')"
            objDA.strQuery += " And U_Z_DocEntry = '" + objEN.Code + "' and (isnull(U_Z_DLineId,'')='" & objEN.LineId & "' or isnull(U_Z_DLineId,'')='')"
            objDA.sqlda = New SqlDataAdapter(objDA.strQuery, objDA.con)
            objDA.sqlda.Fill(objDA.Ds2)
            Return objDA.Ds2
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            Throw ex
        End Try
    End Function
    Public Function CancelRequest(ByVal objen As EmpPREN) As String
        Try
            ' objDA.strQuery = "Delete from  ""@Z_OPRQ"" where ""DocEntry""='" & objen.Code & "';"
            objDA.strQuery = "Update ""@Z_OPRQ"" set U_Z_DocStatus='L' where ""DocEntry""='" & objen.Code & "';"
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
