Imports System
Imports System.Globalization
Imports System.Data
Imports System.Data.SqlClient
Imports System.Net.Mail
Imports System.IO
Imports System.Data.Odbc
Imports DAL
Imports EN
Public Class PRApprovalDA
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
            objDA.strQuery = " select distinct(T0.DocEntry) AS Code,T0.U_Z_EmpID,T0.U_Z_EmpName,Convert(Varchar(10),T0.U_Z_DocDate,103) AS U_Z_DocDate,T0.U_Z_DeptName,"
            objDA.strQuery += " T0.U_Z_Destination,Case T0.""U_Z_DocStatus"" when 'O' then 'Open' when 'I' then 'InProgress' when 'S' then 'Confirm' when 'L' then 'Cancelled' "
            objDA.strQuery += " when 'D' then 'Draft' when 'R' then 'DLC Rejected' else 'Closed' end AS U_Z_DocStatus"
            objDA.strQuery += "  from [@Z_OPRQ] T0"
            objDA.strQuery += "  Join [@Z_PRQ1] T1 on T0.DocEntry=T1.DocEntry LEFT JOIN [@Z_DLC_APHIS] T5 on T0.DocEntry=T5.U_Z_DocEntry  And T5.U_Z_DocType = 'PR'"
            objDA.strQuery += "  JOIN [@Z_DLC_OAPPT] T3 ON T3.DocEntry = T1.U_Z_ApproveId and (T1.U_Z_AppStatus='P' or T1.U_Z_AppStatus='-') "
            objDA.strQuery += "  JOIN [@Z_DLC_APPT2] T2 ON T3.DocEntry = T2.DocEntry "
            objDA.strQuery += " And (T1.U_Z_CurApprover = '" + objEN.UserCode + "' OR T1.U_Z_NxtApprover = '" + objEN.UserCode + "')"
            objDA.strQuery += " And isnull(T2.U_Z_AMan,'N')='Y' AND isnull(T3.U_Z_Active,'N')='Y' and  isnull(T1.U_Z_AppRequired,'N')='Y' and (isnull(T0.U_Z_DocStatus,'O') ='O' or isnull(T0.U_Z_DocStatus,'O') ='I') and  T2.U_Z_AUser = '" + objEN.UserCode + "' And T3.U_Z_DocType = 'PR'  Order by T0.DocEntry Desc;"

            objDA.strQuery += " select distinct(T0.DocEntry) AS Code,T0.U_Z_EmpID,T0.U_Z_EmpName,Convert(Varchar(10),T0.U_Z_DocDate,103) AS U_Z_DocDate,T0.U_Z_DeptName,"
            objDA.strQuery += " T0.U_Z_Destination,Case T0.""U_Z_DocStatus"" when 'O' then 'Open' when 'I' then 'InProgress' when 'S' then 'Confirm' when 'L' then 'Cancelled'"
            objDA.strQuery += " when 'D' then 'Draft' when 'R' then 'DLC Rejected' else 'Closed' end AS U_Z_DocStatus"
            objDA.strQuery += " from [@Z_OPRQ] T0"
            objDA.strQuery += "  Join [@Z_PRQ1] T1 on T0.DocEntry=T1.DocEntry LEFT JOIN [@Z_DLC_APHIS] T5 on T0.DocEntry=T5.U_Z_DocEntry  And T5.U_Z_DocType = 'PR'"
            objDA.strQuery += "  JOIN [@Z_DLC_OAPPT] T3 ON T3.DocEntry = T1.U_Z_ApproveId "
            objDA.strQuery += "  JOIN [@Z_DLC_APPT2] T2 ON T3.DocEntry = T2.DocEntry "
            objDA.strQuery += " And (T1.U_Z_CurApprover = '" + objEN.UserCode + "' OR T1.U_Z_NxtApprover = '" + objEN.UserCode + "')"
            objDA.strQuery += " And isnull(T2.U_Z_AMan,'N')='Y' AND isnull(T3.U_Z_Active,'N')='Y' and  isnull(T1.U_Z_AppRequired,'N')='Y' "
            objDA.strQuery += " and (isnull(T0.U_Z_DocStatus,'O') ='O' or isnull(T0.U_Z_DocStatus,'O') ='I' or isnull(T0.U_Z_DocStatus,'O') ='C') and  T2.U_Z_AUser = '" + objEN.UserCode + "' And T3.U_Z_DocType = 'PR' Order by T0.DocEntry Desc;"
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
            objDA.strQuery = " Select T4.U_Z_DocNo,T0.DocEntry,T4.LineId,T0.U_Z_EmpID,T0.U_Z_EmpName,Convert(Varchar(10),T0.U_Z_DocDate,103) AS U_Z_DocDate,T0.U_Z_DeptCode,T0.U_Z_DeptName,U_Z_Destination,T4.U_Z_ItemCode,T4.U_Z_ItemName,T4.U_Z_OrdQty,T4.U_Z_OrdUomDesc,T4.U_Z_OrdUom,T4.U_Z_AltItemCode,T4.U_Z_AltItemName,Case Isnull(T4.U_Z_AppStatus,'P') when 'P' then 'Pending' when 'A' then 'Approved' when 'R' then 'Rejected' end  AS U_Z_AppStatus,"
            objDA.strQuery += "T4.U_Z_DeliQty,T4.U_Z_DelUom,T4.U_Z_DelUomDesc,U_Z_BarCode,U_Z_AltBarCode,U_Z_LineStatus,U_Z_GoodIssue,U_Z_CurApprover,U_Z_NxtApprover,"
            objDA.strQuery += " Case U_Z_AppRequired when 'Y' then 'Yes' else 'No' End as  'U_Z_AppRequired',T4.U_Z_RejRemark,Convert(Varchar(10),T4.U_Z_AppReqDate,103) AS U_Z_AppReqDate,CONVERT(VARCHAR(8),U_Z_ReqTime,108) AS 'U_Z_ReqTime'"
            objDA.strQuery += "  From [@Z_OPRQ] T0 JOIN [@Z_PRQ1] T4 on T0.DocEntry=T4.DocEntry "
            'objDA.strQuery += " Left Outer Join [@Z_DLC_APHIS] T5 on T0.DocEntry=T5.U_Z_DocEntry And T5.U_Z_DocType= 'PR' and T5.U_Z_ApproveBy='" + objEN.UserCode + "'"
            objDA.strQuery += "  JOIN [@Z_DLC_OAPPT] T3 ON T3.DocEntry = T4.U_Z_ApproveId and (T4.U_Z_AppStatus='P' or T4.U_Z_AppStatus='-') "
            objDA.strQuery += "  JOIN [@Z_DLC_APPT2] T2 ON T3.DocEntry = T2.DocEntry "
            objDA.strQuery += " And (T4.U_Z_CurApprover = '" + objEN.UserCode + "' OR T4.U_Z_NxtApprover = '" + objEN.UserCode + "')"
            objDA.strQuery += " And isnull(T2.U_Z_AMan,'N')='Y' AND isnull(T3.U_Z_Active,'N')='Y' and  isnull(T4.U_Z_AppRequired,'N')='Y' and  T2.U_Z_AUser = '" + objEN.UserCode + "' And T3.U_Z_DocType = 'PR' where T0.DocEntry='" & objEN.DocEntry & "' Order by Convert(Numeric,T0.DocEntry) Desc;"
            objDA.strQuery += " Select T0.DocEntry,T0.U_Z_EmpID,T0.U_Z_EmpName,Convert(Varchar(10),T0.U_Z_DocDate,103) AS U_Z_DocDate,T0.U_Z_DeptCode,T0.U_Z_DeptName,U_Z_Destination,Case isnull(T0.U_Z_DocStatus,'O') when 'O' then 'Open' when 'I' then 'InProgress' else 'Closed' end AS U_Z_DocStatus from [@Z_OPRQ] T0 where DocEntry='" & objEN.DocEntry & "';"
            objDA.sqlda = New SqlDataAdapter(objDA.strQuery, objDA.con)
            objDA.sqlda.Fill(objDA.Ds1)
            Return objDA.Ds1
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            Throw ex
        End Try
    End Function
    Public Function ReBindPRequest(ByVal objEN As PRApprovalEN) As DataSet
        Try
            objDA.strQuery = " Select T4.U_Z_DocNo,T0.DocEntry,T4.LineId,T0.U_Z_EmpID,T0.U_Z_EmpName,Convert(Varchar(10),T0.U_Z_DocDate,103) AS U_Z_DocDate,T0.U_Z_DeptCode,T0.U_Z_DeptName,U_Z_Destination,T4.U_Z_ItemCode,T4.U_Z_ItemName,T4.U_Z_OrdQty,T4.U_Z_OrdUomDesc,T4.U_Z_OrdUom,T4.U_Z_AltItemCode,T4.U_Z_AltItemName,Case Isnull(T4.U_Z_AppStatus,'P') when 'P' then 'Pending' when 'A' then 'Approved' when 'R' then 'Rejected' end  AS U_Z_AppStatus,"
            objDA.strQuery += "T4.U_Z_DeliQty,T4.U_Z_DelUom,T4.U_Z_DelUomDesc,U_Z_BarCode,U_Z_AltBarCode,U_Z_LineStatus,U_Z_GoodIssue,U_Z_CurApprover,U_Z_NxtApprover,"
            objDA.strQuery += " Case U_Z_AppRequired when 'Y' then 'Yes' else 'No' End as  'U_Z_AppRequired',T4.U_Z_RejRemark,Convert(Varchar(10),T4.U_Z_AppReqDate,103) AS U_Z_AppReqDate,CONVERT(VARCHAR(8),U_Z_ReqTime,108) AS 'U_Z_ReqTime'"
            objDA.strQuery += "  From [@Z_OPRQ] T0 JOIN [@Z_PRQ1] T4 on T0.DocEntry=T4.DocEntry "
            'objDA.strQuery += " Left Outer Join [@Z_DLC_APHIS] T5 on T0.DocEntry=T5.U_Z_DocEntry And T5.U_Z_DocType= 'PR' and T5.U_Z_ApproveBy='" + objEN.UserCode + "'"
            objDA.strQuery += "  JOIN [@Z_DLC_OAPPT] T3 ON T3.DocEntry = T4.U_Z_ApproveId and (T4.U_Z_AppStatus='P' or T4.U_Z_AppStatus='-') "
            objDA.strQuery += "  JOIN [@Z_DLC_APPT2] T2 ON T3.DocEntry = T2.DocEntry "
            objDA.strQuery += " And (T4.U_Z_CurApprover = '" + objEN.UserCode + "' OR T4.U_Z_NxtApprover = '" + objEN.UserCode + "')"
            objDA.strQuery += " And isnull(T2.U_Z_AMan,'N')='Y' AND isnull(T3.U_Z_Active,'N')='Y' and  isnull(T4.U_Z_AppRequired,'N')='Y' and  T2.U_Z_AUser = '" + objEN.UserCode + "' And T3.U_Z_DocType = 'PR' where T0.DocEntry='" & objEN.DocEntry & "' Order by Convert(Numeric,T0.DocEntry) Desc;"
            objDA.sqlda = New SqlDataAdapter(objDA.strQuery, objDA.con)
            objDA.sqlda.Fill(objDA.dss2)
            Return objDA.dss2
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            Throw ex
        End Try
    End Function
    Public Function CancelRequest(ByVal objEN As PRApprovalEN) As String
        Try
            objDA.strQuery = "Update [@Z_PRQ1] set U_Z_AppStatus='R' where Code='" & objEN.DocEntry & "'"
            objDA.cmd = New SqlCommand(objDA.strQuery, objDA.con)
            objDA.con.Open()
            objDA.cmd.ExecuteNonQuery()
            objDA.con.Close()
            Return "Success"
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            Throw ex
        End Try
    End Function
    Public Function BindItemCode(ByVal objen As PRApprovalEN) As DataSet
        Try
            objDA.strQuery = "Select U_AllItemCat  from [@Z_DLC_LOGIN] T0 where U_EMPID=" & objen.EmpId & ""
            objDA.sqlda = New SqlDataAdapter(objDA.strQuery, objDA.con)
            objDA.sqlda.Fill(objDA.Ds1)
            If objDA.Ds1.Tables(0).Rows.Count > 0 Then
                If objDA.Ds1.Tables(0).Rows(0)(0).ToString() = "Y" Then
                    objDA.strQuery = "Select ItemCode,ItemName,isnull(CodeBars,'0') as CodeBars  from OITM where InvntItem='Y';"
                    objDA.strQuery += " select U_Z_ItemCode,U_Z_ItemName,U_Z_BarCode,U_Z_OrdUom,U_Z_OrdQty,U_Z_AltBarCode,U_Z_AltItemCode,U_Z_AltItemName,U_Z_DelUom,U_Z_DeliQty  from [@Z_PRQ1] where DocEntry='" & objen.DocEntry & "' and LineId='" & objen.DocLineId & "';"
                    objDA.sqlda = New SqlDataAdapter(objDA.strQuery, objDA.con)
                    objDA.sqlda.Fill(objDA.Ds2)
                    Return objDA.Ds2
                Else
                    objDA.strQuery = "select ItemCode,ItemName,isnull(CodeBars,'0') as CodeBars from OITM T0 left Join [@Z_ITCAT1] T1 on T0.U_CatCode=T1.U_CatCode "
                    objDA.strQuery += "inner join [@Z_DLC_LOGIN] T2 on T1.DocEntry=T2.DocEntry where T2.U_EMPID = " & objen.EmpId & " and T0.InvntItem='Y';"
                    objDA.strQuery += " select U_Z_ItemCode,U_Z_ItemName,U_Z_BarCode,U_Z_OrdUom,U_Z_OrdQty,U_Z_AltBarCode,U_Z_AltItemCode,U_Z_AltItemName,U_Z_DelUom,U_Z_DeliQty  from [@Z_PRQ1] where DocEntry='" & objen.DocEntry & "' and LineId='" & objen.DocLineId & "';"
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
    Public Function BindUom(ByVal objen As PRApprovalEN) As DataSet
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
    Public Function AlternateRequest(ByVal objEN As PRApprovalEN) As String
        Try
            objDA.strQuery = "Update [@Z_PRQ1] set U_Z_AltItemCode='" & objEN.ItemCode & "',U_Z_AltItemName='" & objEN.Itemdesc & "',U_Z_DeliQty='" & objEN.OrdrQty & "',U_Z_RecQty='" & objEN.OrdrQty & "',U_Z_LineStatus='" & objEN.LineStatus & "'"
            objDA.strQuery += " ,U_Z_DelUom='" & objEN.OrdrUom & "',U_Z_DelUomDesc='" & objEN.OrdrUomDesc & "',U_Z_RecUom='" & objEN.OrdrUom & "',U_Z_RecUomDesc='" & objEN.OrdrUomDesc & "',U_Z_RejRemark='" & objEN.Remarks & "' where DocEntry='" & objEN.DocEntry & "' and LineId='" & objEN.DocLineId & "';"
            objDA.strQuery += "Update [@Z_OPRQ] set U_Z_DocStatus='I' where DocEntry='" & objEN.DocEntry & "';"
            objDA.cmd = New SqlCommand(objDA.strQuery, objDA.con)
            objDA.con.Open()
            objDA.cmd.ExecuteNonQuery()
            objDA.con.Close()
            Return "Success"
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            Throw ex
        End Try
    End Function
    Public Function GetEmpUserid(ByVal objEN As PRApprovalEN) As Integer
        Try
            objDA.strQuery = "select T0.userId from OHEM T0 JOIN OUSR T1 on T0.userId=T1.USERID where T0.empID='" & objEN.EmpId & "'"
            objDA.cmd = New SqlCommand(objDA.strQuery, objDA.con)
            objDA.con.Open()
            objEN.EmpUserId = objDA.cmd.ExecuteScalar()
            objDA.con.Close()
            Return objEN.EmpUserId
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            Throw ex
        End Try
    End Function
    Public Function ApprovalValidation(ByVal objEN As PRApprovalEN) As String
        Try
            If objEN.AppStatus = "R" Then
                If objEN.Remarks = "" Then
                    objDA.strmsg = "Remarks is missing..."
                    Return objDA.strmsg
                End If
            End If
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            objDA.strmsg = "" & ex.Message & ""
            Return objDA.strmsg
        End Try
        Return "Success"
    End Function
    Public Function addUpdateDocument(ByVal objEN As PRApprovalEN) As String
        Try
            Dim oGeneralService As SAPbobsCOM.GeneralService
            Dim oGeneralData As SAPbobsCOM.GeneralData
            Dim oGeneralParams As SAPbobsCOM.GeneralDataParams
            Dim oCompanyService As SAPbobsCOM.CompanyService
            Dim oRecordSet, otestRs, oTemp, oTest As SAPbobsCOM.Recordset
            objDA.objMainCompany = objEN.SapCompany
            oCompanyService = objDA.objMainCompany.GetCompanyService()
            oGeneralService = oCompanyService.GetGeneralService("Z_DLC_APHIS")
            oGeneralData = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData)
            oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
            oRecordSet = objDA.objMainCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            otestRs = objDA.objMainCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oTemp = objDA.objMainCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oTest = objDA.objMainCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            objDA.strQuery = "select T0.DocEntry,T1.LineId from [@Z_DLC_OAPPT] T0 JOIN [@Z_DLC_APPT2] T1 on T0.DocEntry=T1.DocEntry"
            objDA.strQuery += " JOIN [@Z_DLC_APPT3] T2 on T1.DocEntry=T2.DocEntry"
            objDA.strQuery += " where T0.U_Z_DocType='" & objEN.HistoryType & "' AND T2.U_Z_DeptCode='" & objEN.DeptCode & "' AND T1.U_Z_AUser='" & objEN.UserCode & "'"
            otestRs.DoQuery(objDA.strQuery)
            If otestRs.RecordCount > 0 Then
                objEN.HeadDocEntry = otestRs.Fields.Item(0).Value
                objEN.HeadLineId = otestRs.Fields.Item(1).Value
            Else
                objDA.strQuery = "select T0.DocEntry,T1.LineId from [@Z_DLC_OAPPT] T0 JOIN [@Z_DLC_APPT2] T1 on T0.DocEntry=T1.DocEntry"
                objDA.strQuery += " left outer JOIN [@Z_DLC_APPT3] T2 on T1.DocEntry=T2.DocEntry"
                objDA.strQuery += " where T0.U_Z_DocType='" & objEN.HistoryType & "' AND T0.U_Z_AllDept='Y' AND T1.U_Z_AUser='" & objEN.UserCode & "'"
                otestRs.DoQuery(objDA.strQuery)
                If otestRs.RecordCount > 0 Then
                    objEN.HeadDocEntry = otestRs.Fields.Item(0).Value
                    objEN.HeadLineId = otestRs.Fields.Item(1).Value
                End If
            End If
            If objEN.HistoryType = "PR" Then
                objDA.strQuery = "Select * from [@Z_DLC_APHIS] where U_Z_DocEntry='" & objEN.DocEntry & "' and isnull(U_Z_DLineId,'')='" & objEN.DocLineId & "' and U_Z_DocType='" & objEN.HistoryType & "' and U_Z_ApproveBy='" & objEN.UserCode & "'"
            Else
                objDA.strQuery = "Select * from [@Z_DLC_APHIS] where isnull(U_Z_DLineId,'')='" & objEN.DocLineId & "' and  U_Z_DocEntry='" & objEN.DocEntry & "' and U_Z_DocType='" & objEN.HistoryType & "' and U_Z_ApproveBy='" & objEN.UserCode & "'"
            End If
            oRecordSet.DoQuery(objDA.strQuery)
            If oRecordSet.RecordCount > 0 Then
                oGeneralParams.SetProperty("DocEntry", oRecordSet.Fields.Item("DocEntry").Value)
                oGeneralData = oGeneralService.GetByParams(oGeneralParams)
                oGeneralData.SetProperty("U_Z_AppStatus", objEN.LineStatus)
                oGeneralData.SetProperty("U_Z_Remarks", objEN.Remarks)
                oGeneralData.SetProperty("U_Z_ADocEntry", objEN.HeadDocEntry)
                oGeneralData.SetProperty("U_Z_ALineId", objEN.HeadLineId)
                oGeneralData.SetProperty("U_Z_ItemCode", objEN.ItemCode)
                oGeneralData.SetProperty("U_Z_OrdQty", objEN.OrdrQty)
                oGeneralData.SetProperty("U_Z_DelUom", objEN.OrdrUom)
                oGeneralData.SetProperty("U_Z_DelUomDesc", objEN.OrdrUomDesc)
                oGeneralData.SetProperty("U_Z_ItemName", objEN.Itemdesc)


                oTemp.DoQuery("Select * ,isnull(""firstName"",'') +  ' ' + isnull(""middleName"",'') +  ' ' + isnull(""lastName"",'') 'EmpName' from OHEM where ""userid""=" & objEN.EmpUserId & "")
                If oTemp.RecordCount > 0 Then
                    oGeneralData.SetProperty("U_Z_EmpId", oTemp.Fields.Item("empID").Value.ToString())
                    oGeneralData.SetProperty("U_Z_EmpName", oTemp.Fields.Item("EmpName").Value)
                Else
                    oGeneralData.SetProperty("U_Z_EmpId", "")
                    oGeneralData.SetProperty("U_Z_EmpName", "")
                End If
                oGeneralService.Update(oGeneralData)
                objDA.strmsg = "Successfully approved document..."
            ElseIf (objEN.DocEntry <> "" And objEN.DocEntry <> "0") Then
                oTemp.DoQuery("Select * ,isnull(""firstName"",'') + ' ' + isnull(""middleName"",'') +  ' ' + isnull(""lastName"",'') 'EmpName' from OHEM where ""userid""=" & objEN.EmpUserId & "")
                If oTemp.RecordCount > 0 Then
                    oGeneralData.SetProperty("U_Z_EmpId", oTemp.Fields.Item("empID").Value.ToString())
                    oGeneralData.SetProperty("U_Z_EmpName", oTemp.Fields.Item("EmpName").Value)
                Else
                    oGeneralData.SetProperty("U_Z_EmpId", "")
                    oGeneralData.SetProperty("U_Z_EmpName", "")
                End If
                oGeneralData.SetProperty("U_Z_DLineId", objEN.DocLineId)
                oGeneralData.SetProperty("U_Z_DocEntry", objEN.DocEntry)
                oGeneralData.SetProperty("U_Z_DocType", objEN.HistoryType)
                oGeneralData.SetProperty("U_Z_AppStatus", objEN.LineStatus)
                oGeneralData.SetProperty("U_Z_Remarks", objEN.Remarks)
                oGeneralData.SetProperty("U_Z_ApproveBy", objEN.UserCode)
                oGeneralData.SetProperty("U_Z_Approvedt", System.DateTime.Now)
                oGeneralData.SetProperty("U_Z_ADocEntry", objEN.HeadDocEntry)
                oGeneralData.SetProperty("U_Z_ALineId", objEN.HeadLineId)
                oGeneralData.SetProperty("U_Z_ItemCode", objEN.ItemCode)
                oGeneralData.SetProperty("U_Z_OrdQty", objEN.OrdrQty)
                oGeneralData.SetProperty("U_Z_DelUom", objEN.OrdrUom)
                oGeneralData.SetProperty("U_Z_DelUomDesc", objEN.OrdrUomDesc)
                oGeneralData.SetProperty("U_Z_ItemName", objEN.Itemdesc)
                oGeneralService.Add(oGeneralData)
                objDA.strmsg = "Successfully approved document..."
            End If
            If objEN.LineStatus <> "O" Then
                objDA.strmsg = UpdateFinalStatus(objEN)
            End If
            Return objDA.strmsg
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            objDA.strmsg = ex.Message
            Return objDA.strmsg
        End Try
    End Function
    Public Function UpdateFinalStatus(ByVal objEN As PRApprovalEN) As String
        Dim StrMailMessage, strSubject As String
        Try
            Dim oRecordSet, oTemp As SAPbobsCOM.Recordset
            oRecordSet = objEN.SapCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oTemp = objEN.SapCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            If objEN.LineStatus = "D" Or objEN.LineStatus = "C" Then
                objDA.strQuery = " Select T2.DocEntry "
                objDA.strQuery += " From [@Z_DLC_APPT2] T2 "
                objDA.strQuery += " JOIN [@Z_DLC_OAPPT] T3 ON T2.DocEntry = T3.DocEntry  "
                objDA.strQuery += " JOIN [@Z_DLC_APPT3] T4 ON T4.DocEntry = T3.DocEntry  "
                objDA.strQuery += " Where T4.U_Z_DeptCode='" & objEN.DeptCode & "' and  U_Z_AFinal = 'Y'"
                objDA.strQuery += " And T2.U_Z_AUser = '" + objEN.UserCode + "' And T3.U_Z_DocType = '" & objEN.HistoryType & "'"
                oRecordSet.DoQuery(objDA.strQuery)
                If Not oRecordSet.EoF Then
                    Select Case objEN.HistoryType
                        Case "PR"
                            objDA.strQuery = "Update [@Z_PRQ1] Set U_Z_AppStatus='A',U_Z_LineStatus='" & objEN.LineStatus & "' Where DocEntry = '" + objEN.DocEntry + "' and LineId='" & objEN.DocLineId & "'"
                            oRecordSet.DoQuery(objDA.strQuery)
                        Case "MR"
                            objDA.strQuery = "Update [@Z_RPD1] Set U_Z_AppStatus='A',U_Z_LineStatus='" & objEN.LineStatus & "' Where DocEntry = '" + objEN.DocEntry + "' and LineId='" & objEN.DocLineId & "'"
                            oRecordSet.DoQuery(objDA.strQuery)
                    End Select

                Else
                    objDA.strQuery = " Select T2.DocEntry "
                    objDA.strQuery += " From [@Z_DLC_APPT2] T2 "
                    objDA.strQuery += " JOIN [@Z_DLC_OAPPT] T3 ON T2.DocEntry = T3.DocEntry  "
                    objDA.strQuery += " LEFT OUTER JOIN [@Z_DLC_APPT3] T4 ON T4.DocEntry = T3.DocEntry  "
                    objDA.strQuery += " Where  T3.U_Z_AllDept='Y' and  U_Z_AFinal = 'Y'"
                    objDA.strQuery += " And T2.U_Z_AUser = '" + objEN.UserCode + "' And T3.U_Z_DocType = '" & objEN.HistoryType & "'"
                    oRecordSet.DoQuery(objDA.strQuery)
                    If Not oRecordSet.EoF Then
                        Select Case objEN.HistoryType
                            Case "PR"
                                objDA.strQuery = "Update [@Z_PRQ1] Set U_Z_AppStatus='A',U_Z_LineStatus='" & objEN.LineStatus & "' Where DocEntry = '" + objEN.DocEntry + "' and LineId='" & objEN.DocLineId & "'"
                                oRecordSet.DoQuery(objDA.strQuery)
                            Case "MR"
                                objDA.strQuery = "Update [@Z_RPD1] Set U_Z_AppStatus='A',U_Z_LineStatus='" & objEN.LineStatus & "' Where DocEntry = '" + objEN.DocEntry + "' and LineId='" & objEN.DocLineId & "'"
                                oRecordSet.DoQuery(objDA.strQuery)
                        End Select
                    End If
                End If
            ElseIf objEN.LineStatus = "L" Then
                objDA.strQuery = " Select T2.DocEntry "
                objDA.strQuery += " From [@Z_DLC_APPT2] T2 "
                objDA.strQuery += " JOIN [@Z_DLC_OAPPT] T3 ON T2.DocEntry = T3.DocEntry  "
                objDA.strQuery += " LEFT OUTER JOIN [@Z_DLC_APPT3] T4 ON T4.DocEntry = T3.DocEntry  "
                objDA.strQuery += " Where  (T3.U_Z_AllDept='Y' or T4.U_Z_DeptCode='" & objEN.DeptCode & "')"
                objDA.strQuery += " And T2.U_Z_AUser = '" + objEN.UserCode + "' And T3.U_Z_DocType = '" & objEN.HistoryType & "'"
                oRecordSet.DoQuery(objDA.strQuery)
                If Not oRecordSet.EoF Then
                    Select Case objEN.HistoryType
                        Case "PR"
                            objDA.strQuery = "Update [@Z_PRQ1] Set U_Z_AppStatus='R',U_Z_LineStatus='L' Where DocEntry = '" + objEN.DocEntry + "' and LineId='" & objEN.DocLineId & "'"
                            oRecordSet.DoQuery(objDA.strQuery)
                            objDA.strQuery = "Select * from [@Z_PRQ1] where DocEntry = '" + objEN.DocEntry + "' and LineId='" & objEN.DocLineId & "'"
                            objDA.Ds4 = objDA.GetData(objDA.strQuery)
                            If objDA.Ds4.Tables(0).Rows.Count > 0 Then
                                StrMailMessage = "Purchase Requisition rejected for the request number is :" & objEN.DocEntry & _
                                    ", Line number is :" & objEN.Code & " , ItemCode :" & objDA.Ds4.Tables(0).Rows(0)("U_Z_AltItemCode").ToString() & "" & _
                                     " , ItemName :" & objDA.Ds4.Tables(0).Rows(0)("U_Z_AltItemName").ToString() & " ,  Quantity :" & objDA.Ds4.Tables(0).Rows(0)("U_Z_DeliQty").ToString() & "" & _
                                     " , Remarks :" & objDA.Ds4.Tables(0).Rows(0)("U_Z_RejRemark").ToString() & ""
                                strSubject = "Purchase Requisition request is rejected"
                                objDA.SendMail_RequestApproval(StrMailMessage, objEN.EmpId, objEN.SapCompany, strSubject)
                            End If
                        Case "MR"
                            objDA.strQuery = "Update [@Z_RPD1] Set U_Z_AppStatus='R',U_Z_LineStatus='L' Where DocEntry = '" & objEN.DocEntry & "' and LineId='" & objEN.DocLineId & "'"
                            oRecordSet.DoQuery(objDA.strQuery)
                            objDA.strQuery = "Select * from [@Z_RPD1] where DocEntry = '" & objEN.DocEntry & "' and LineId='" & objEN.DocLineId & "'"
                            objDA.Ds4 = objDA.GetData(objDA.strQuery)
                            If objDA.Ds4.Tables(0).Rows.Count > 0 Then
                                StrMailMessage = "Material Return rejected for the request number is :" & objEN.DocEntry & "" & _
                                    ", Line number is :" & objEN.DocLineId & " , ItemCode :" & objDA.Ds4.Tables(0).Rows(0)("U_Z_ItemCode").ToString() & "" & _
                                     " , ItemName :" & objDA.Ds4.Tables(0).Rows(0)("U_Z_ItemName").ToString() & " ,  Quantity :" & objDA.Ds4.Tables(0).Rows(0)("U_Z_OrdQty").ToString() & "" & _
                                     " , Remarks :" & objDA.Ds4.Tables(0).Rows(0)("U_Z_RejRemark").ToString() & ""
                                strSubject = "Material Return request is rejected"
                                objDA.SendMail_RequestApproval(StrMailMessage, objEN.EmpId, objEN.SapCompany, strSubject)
                            End If
                    End Select

                End If
            End If
            Return objDA.strmsg
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            objDA.strmsg = ex.Message
            Return objDA.strmsg
        End Try
    End Function
    Public Function LoadHistory(ByVal objEN As PRApprovalEN) As DataSet
        Try
            objDA.strQuery = " Select DocEntry,U_Z_DocEntry,U_Z_DocType,U_Z_EmpId,U_Z_EmpName,U_Z_ItemCode,U_Z_ItemName,U_Z_OrdQty,U_Z_delUomDesc,U_Z_ApproveBy,convert(varchar(10),CreateDate,103) as CreateDate,CreateTime, convert(varchar(10),UpdateDate,103) as UpdateDate,UpdateTime,Case U_Z_AppStatus when 'O' then 'Open' when 'D' then 'Delivered' when 'C' then 'Close' when 'L' then 'Cancelled' end AS U_Z_AppStatus,U_Z_Remarks From [@Z_DLC_APHIS] "
            objDA.strQuery += " Where U_Z_DocType = 'PR'"
            objDA.strQuery += " And U_Z_DocEntry = '" + objEN.DocEntry + "' and U_Z_DlineId='" & objEN.DocLineId & "'"
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
            objDA.strQuery = " Select T4.U_Z_DocNo,T0.DocEntry,T4.LineId,T0.U_Z_EmpID,T0.U_Z_EmpName,Convert(Varchar(10),T0.U_Z_DocDate,103) AS U_Z_DocDate,T0.U_Z_DeptCode,T0.U_Z_DeptName,U_Z_Destination,T4.U_Z_ItemCode,T4.U_Z_ItemName,T4.U_Z_OrdQty,T4.U_Z_OrdUomDesc,T4.U_Z_OrdUom,T4.U_Z_AltItemCode,T4.U_Z_AltItemName,Case Isnull(T4.U_Z_AppStatus,'P') when 'P' then 'Pending' when 'A' then 'Approved' when 'R' then 'Rejected' end  AS U_Z_AppStatus,"
            objDA.strQuery += "T4.U_Z_DeliQty,T4.U_Z_DelUom,T4.U_Z_DelUomDesc,U_Z_BarCode,U_Z_AltBarCode,U_Z_LineStatus,U_Z_GoodIssue,U_Z_CurApprover,U_Z_NxtApprover,"
            objDA.strQuery += " Case U_Z_AppRequired when 'Y' then 'Yes' else 'No' End as  'U_Z_AppRequired',T4.U_Z_RejRemark,Convert(Varchar(10),T4.U_Z_AppReqDate,103) AS U_Z_AppReqDate,CONVERT(VARCHAR(8),U_Z_ReqTime,108) AS 'U_Z_ReqTime'"
            objDA.strQuery += "  From [@Z_OPRQ] T0 JOIN [@Z_PRQ1] T4 on T0.DocEntry=T4.DocEntry"
            objDA.strQuery += "  JOIN [@Z_DLC_APPT2] T2 ON T4.U_Z_ApproveId = T2.DocEntry"
            objDA.strQuery += "  JOIN [@Z_DLC_OAPPT] T3 ON T2.DocEntry = T3.DocEntry  "
            objDA.strQuery += " And isnull(T2.U_Z_AMan,'N')='Y' AND isnull(T3.U_Z_Active,'N')='Y' and T2.U_Z_AUser = '" + objEN.UserCode + "' And T3.U_Z_DocType = 'PR' where T0.DocEntry='" & objEN.DocEntry & "' Order by Convert(Numeric,T0.DocEntry) Desc;"
            objDA.sqlda = New SqlDataAdapter(objDA.strQuery, objDA.con)
            objDA.sqlda.Fill(objDA.Ds3)
            Return objDA.Ds3
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            Throw ex
        End Try
    End Function
End Class
