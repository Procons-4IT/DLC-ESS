Imports System
Imports System.Globalization
Imports System.Data
Imports System.Data.SqlClient
Imports System.Net.Mail
Imports System.IO
Imports System.Data.Odbc
Imports DAL
Imports EN
Public Class DLCApprovalDA
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
            objDA.strQuery = " select distinct(T0.DocEntry) AS Code,T1.U_Z_CurApprover,T1.U_Z_NxtApprover,T0.U_Z_DeptCode,T0.U_Z_EmpID,T0.U_Z_EmpName,Convert(Varchar(10),T0.U_Z_DocDate,103) AS U_Z_DocDate,T0.U_Z_DeptName,T0.U_Z_Destination,Case T0.""U_Z_DocStatus"" when 'O' then 'Open' when 'I' then 'InProgress' when 'S' then 'Confirm' when 'L' then 'Cancelled' when 'D' then 'Draft' when 'R' then 'DLC Rejected' when 'DI' then 'DLC InProgress' else 'Closed' end AS U_Z_DocStatus,isnull(T1.U_Z_AppStatus,'P') as  U_Z_AppStatus,CONVERT(varchar(1000),T1.U_Z_RejRemark) as U_Z_Remarks  from [@Z_OPRQ] T0"
            objDA.strQuery += " JOIN [@Z_PRQ1] T1 on T0.DocEntry=T1.DocEntry  LEFT JOIN [@Z_DLC_APHIS] T5 on T0.DocEntry=T5.U_Z_DocEntry And T5.U_Z_DocType = 'PRD'"
            objDA.strQuery += "   JOIN [@Z_DLC_OAPPT] T3 ON T3.DocEntry = T1.U_Z_ApproveId and (T1.U_Z_AppStatus='P' or T1.U_Z_AppStatus='-')"
            objDA.strQuery += "  JOIN [@Z_DLC_APPT2] T2 ON T3.DocEntry = T2.DocEntry "
            objDA.strQuery += " And (T1.U_Z_CurApprover = '" + objEN.UserCode + "' OR T1.U_Z_NxtApprover = '" + objEN.UserCode + "')"
            objDA.strQuery += " And isnull(T2.U_Z_AMan,'N')='Y' AND isnull(T3.U_Z_Active,'N')='Y' and  isnull(T1.U_Z_AppRequired,'N')='Y' and (isnull(T0.U_Z_DocStatus,'O') ='S'  or isnull(T0.U_Z_DocStatus,'O') ='DI') and  T2.U_Z_AUser = '" + objEN.UserCode + "' And T3.U_Z_DocType = 'PRD'  Order by T0.DocEntry Desc;"

            objDA.strQuery += " select distinct(T0.DocEntry) AS Code,T1.U_Z_CurApprover,T1.U_Z_NxtApprover,T0.U_Z_EmpID,T0.U_Z_EmpName,Convert(Varchar(10),T0.U_Z_DocDate,103) AS U_Z_DocDate,T0.U_Z_DeptName,T0.U_Z_Destination,Case T0.""U_Z_DocStatus"" when 'O' then 'Open' when 'I' then 'InProgress' when 'S' then 'Confirm' when 'L' then 'Cancelled' when 'D' then 'Draft' when 'R' then 'DLC Rejected' when 'DI' then 'DLC InProgress' else 'Closed' end AS U_Z_DocStatus,"
            objDA.strQuery += " Case isnull(T1.U_Z_AppStatus,'P') when 'A' then 'Approved' when 'R' then 'Rejected' else 'Pending' end as  U_Z_AppStatus,CONVERT(varchar(1000),T1.U_Z_RejRemark) as U_Z_Remarks from [@Z_OPRQ] T0"
            objDA.strQuery += " Join [@Z_PRQ1] T1 on T0.DocEntry=T1.DocEntry LEFT JOIN [@Z_DLC_APHIS] T5 on T0.DocEntry=T5.U_Z_DocEntry And T5.U_Z_DocType = 'PRD'"
            objDA.strQuery += "   JOIN [@Z_DLC_OAPPT] T3 ON T3.DocEntry = T1.U_Z_ApproveId"
            objDA.strQuery += "  JOIN [@Z_DLC_APPT2] T2 ON T3.DocEntry = T2.DocEntry "
            objDA.strQuery += " And (T1.U_Z_CurApprover = '" + objEN.UserCode + "' OR T1.U_Z_NxtApprover = '" + objEN.UserCode + "')"
            objDA.strQuery += " And isnull(T2.U_Z_AMan,'N')='Y' AND isnull(T3.U_Z_Active,'N')='Y' and  isnull(T1.U_Z_AppRequired,'N')='Y' and (isnull(T0.U_Z_DocStatus,'O') ='S'  or isnull(T0.U_Z_DocStatus,'O') ='DI') and  T2.U_Z_AUser = '" + objEN.UserCode + "' And T3.U_Z_DocType = 'PRD' Order by T0.DocEntry Desc;"
            objDA.sqlda = New SqlDataAdapter(objDA.strQuery, objDA.con)
            objDA.sqlda.Fill(objDA.ds)
            Return objDA.ds
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            Throw ex
        End Try
    End Function
    Public Function BindExpenseSummaryApproval(ByVal objEN As PRApprovalEN) As DataSet
        Try
            objDA.strQuery = " Select T4.U_Z_DocNo,T0.DocEntry,T4.LineId,T0.U_Z_EmpID,T0.U_Z_EmpName,Convert(Varchar(10),T0.U_Z_DocDate,103) AS U_Z_DocDate,T0.U_Z_DeptCode,T0.U_Z_DeptName,U_Z_Destination,T4.U_Z_ItemCode,T4.U_Z_ItemName,CAST(T4.U_Z_OrdQty AS Int) AS U_Z_OrdQty,T4.U_Z_OrdUomDesc,T4.U_Z_OrdUom,T4.U_Z_AltItemCode,T4.U_Z_AltItemName,Case Isnull(T5.U_Z_AppStatus,'P') when 'P' then 'Pending' when 'A' then 'Approved' when 'R' then 'Rejected' end  AS U_Z_AppStatus,"
            objDA.strQuery += " CAST(T4.U_Z_DeliQty AS Int) AS U_Z_DeliQty,T4.U_Z_DelUom,T4.U_Z_DelUomDesc,U_Z_BarCode,U_Z_AltBarCode,U_Z_LineStatus,U_Z_GoodIssue,U_Z_CurApprover,U_Z_NxtApprover,"
            objDA.strQuery += " Case U_Z_AppRequired when 'Y' then 'Yes' else 'No' End as  'U_Z_AppRequired',T4.U_Z_RejRemark,Convert(Varchar(10),T4.U_Z_AppReqDate,103) AS U_Z_AppReqDate,CONVERT(VARCHAR(8),U_Z_ReqTime,108) AS 'U_Z_ReqTime'"
            objDA.strQuery += "  From [@Z_OPRQ] T0 JOIN [@Z_PRQ1] T4 on T0.DocEntry=T4.DocEntry "
            objDA.strQuery += " Left Outer Join [@Z_DLC_APHIS] T5 on T0.DocEntry=T5.U_Z_DocEntry And T5.U_Z_DocType= 'PRD' and T5.U_Z_ApproveBy='" + objEN.UserCode + "'"
            objDA.strQuery += "  JOIN [@Z_DLC_OAPPT] T3 ON T4.U_Z_ApproveId = T3.DocEntry "
            objDA.strQuery += "  JOIN [@Z_DLC_APPT2] T2 ON T3.DocEntry = T2.DocEntry "
            objDA.strQuery += " And isnull(T2.U_Z_AMan,'N')='Y' AND isnull(T3.U_Z_Active,'N')='Y' and T2.U_Z_AUser = '" + objEN.UserCode + "' And T3.U_Z_DocType = 'PRD' where T0.DocEntry='" & objEN.DocEntry & "' Order by Convert(Numeric,T0.DocEntry) Desc;"
            objDA.sqlda = New SqlDataAdapter(objDA.strQuery, objDA.con)
            objDA.sqlda.Fill(objDA.Ds3)
            Return objDA.Ds3
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            Throw ex
        End Try
    End Function
    Public Function LoadHistory(ByVal objEN As PRApprovalEN) As DataSet
        Try
            objDA.strQuery = " Select DocEntry,U_Z_DocEntry,U_Z_DocType,U_Z_EmpId,U_Z_EmpName,U_Z_ItemCode,U_Z_ItemName,CAST(U_Z_OrdQty AS Int) AS U_Z_OrdQty,U_Z_delUomDesc,U_Z_ApproveBy,"
            objDA.strQuery += " convert(varchar(10),CreateDate,103) as CreateDate,CreateTime, convert(varchar(10),UpdateDate,103) as UpdateDate,UpdateTime,"
            objDA.strQuery += " Case U_Z_AppStatus when 'P' then 'Pending' when 'D' then 'Approved' when 'C' then 'Close' when 'L' then 'Cancelled' "
            objDA.strQuery += " when 'R' then 'DLC Rejected' when 'A' then 'DLC Approved' end AS U_Z_AppStatus,U_Z_Remarks From [@Z_DLC_APHIS] "
            objDA.strQuery += " Where U_Z_DocType = 'PRD'"
            objDA.strQuery += " And U_Z_DocEntry = '" + objEN.Code + "'"
            objDA.sqlda = New SqlDataAdapter(objDA.strQuery, objDA.con)
            objDA.sqlda.Fill(objDA.Ds2)
            Return objDA.Ds2
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
            If objEN.HistoryType = "PRD" Then
                objDA.strQuery = "Select * from [@Z_DLC_APHIS] where U_Z_DocEntry='" & objEN.Code & "' and U_Z_DocType='" & objEN.HistoryType & "' and U_Z_ApproveBy='" & objEN.UserCode & "'"
            Else
                objDA.strQuery = "Select * from [@Z_DLC_APHIS] where U_Z_DocEntry='" & objEN.Code & "' and U_Z_DocType='" & objEN.HistoryType & "' and U_Z_ApproveBy='" & objEN.UserCode & "'"
            End If
            oRecordSet.DoQuery(objDA.strQuery)
            If oRecordSet.RecordCount > 0 Then
                oGeneralParams.SetProperty("DocEntry", oRecordSet.Fields.Item("DocEntry").Value)
                oGeneralData = oGeneralService.GetByParams(oGeneralParams)
                oGeneralData.SetProperty("U_Z_AppStatus", objEN.AppStatus)
                oGeneralData.SetProperty("U_Z_Remarks", objEN.Remarks)
                oGeneralData.SetProperty("U_Z_ADocEntry", objEN.HeadDocEntry)
                oGeneralData.SetProperty("U_Z_ALineId", objEN.HeadLineId)
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
            ElseIf (objEN.Code <> "" And objEN.Code <> "0") Then
                oTemp.DoQuery("Select * ,isnull(""firstName"",'') + ' ' + isnull(""middleName"",'') +  ' ' + isnull(""lastName"",'') 'EmpName' from OHEM where ""userid""=" & objEN.EmpUserId & "")
                If oTemp.RecordCount > 0 Then
                    oGeneralData.SetProperty("U_Z_EmpId", oTemp.Fields.Item("empID").Value.ToString())
                    oGeneralData.SetProperty("U_Z_EmpName", oTemp.Fields.Item("EmpName").Value)
                Else
                    oGeneralData.SetProperty("U_Z_EmpId", "")
                    oGeneralData.SetProperty("U_Z_EmpName", "")
                End If
                oGeneralData.SetProperty("U_Z_DocEntry", objEN.Code)
                oGeneralData.SetProperty("U_Z_DocType", objEN.HistoryType)
                oGeneralData.SetProperty("U_Z_AppStatus", objEN.AppStatus)
                oGeneralData.SetProperty("U_Z_Remarks", objEN.Remarks)
                oGeneralData.SetProperty("U_Z_ApproveBy", objEN.UserCode)
                oGeneralData.SetProperty("U_Z_Approvedt", System.DateTime.Now)
                oGeneralData.SetProperty("U_Z_ADocEntry", objEN.HeadDocEntry)
                oGeneralData.SetProperty("U_Z_ALineId", objEN.HeadLineId)
                oGeneralService.Add(oGeneralData)
                objDA.strmsg = "Successfully approved document..."
            End If
            If objEN.AppStatus <> "P" Then
                objDA.SendMessage(objEN.Code, objEN.DeptCode, objEN.UserCode, objEN.Code, objEN.SapCompany, objEN.EmpName, objEN.HistoryType)
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
        Dim StrMailMessage, strSubject, intTempID As String
        Try
            Dim oRecordSet, oTemp As SAPbobsCOM.Recordset
            oRecordSet = objEN.SapCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oTemp = objEN.SapCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            If objEN.AppStatus = "A" Then
                objDA.strQuery = " Select T2.DocEntry "
                objDA.strQuery += " From [@Z_DLC_APPT2] T2 "
                objDA.strQuery += " JOIN [@Z_DLC_OAPPT] T3 ON T2.DocEntry = T3.DocEntry  "
                objDA.strQuery += " JOIN [@Z_DLC_APPT3] T4 ON T4.DocEntry = T3.DocEntry  "
                objDA.strQuery += " Where T4.U_Z_DeptCode='" & objEN.DeptCode & "' and  U_Z_AFinal = 'Y'"
                objDA.strQuery += " And T2.U_Z_AUser = '" + objEN.UserCode + "' And T3.U_Z_DocType = '" & objEN.HistoryType & "'"
                oRecordSet.DoQuery(objDA.strQuery)
                If Not oRecordSet.EoF Then
                    Select Case objEN.HistoryType
                        Case "PRD"
                            objEN.DeptCode = objDA.getDepartment(objEN.EmpId)
                            intTempID = objDA.GetTemplateID("PR", objEN.DeptCode.Trim())
                            If intTempID <> "0" Then
                                objDA.UpdateApprovalRequired("@Z_PRQ1", "DocEntry", objEN.Code, "Y", intTempID)
                                If objEN.Code <> "" Then
                                    objDA.InitialMessage("Purchase request", objEN.Code, objDA.DocApproval("PR", objEN.DeptCode.Trim()), intTempID, objEN.EmpName.Trim(), "PR", objDA.objMainCompany, objEN.Code)
                                End If
                                objDA.strQuery = "Update [@Z_OPRQ] Set U_Z_DocStatus='O' Where DocEntry = '" + objEN.Code + "'"
                                oRecordSet.DoQuery(objDA.strQuery)
                                strSubject = "Purchase Requisition is Approved in DLC"
                                StrMailMessage = "Purchase Requisition Approved for the request number is :" & objEN.Code & ""
                                objDA.SendMail_RequestApproval(StrMailMessage, objEN.EmpId, objEN.SapCompany, strSubject)
                            Else
                                objDA.UpdateApprovalRequired("@Z_PRQ1", "DocEntry", objEN.Code, "N", intTempID)
                                objDA.strQuery = "Update [@Z_PRQ1] Set U_Z_LineStatus='D',U_Z_AppStatus='A' Where DocEntry = '" + objEN.Code + "'"
                                oRecordSet.DoQuery(objDA.strQuery)
                                objDA.strQuery = "Update [@Z_OPRQ] Set U_Z_DocStatus='I' Where DocEntry = '" + objEN.Code + "'"
                                oRecordSet.DoQuery(objDA.strQuery)
                                objDA.strQuery = "Select * from [@Z_PRQ1] where DocEntry='" & objEN.Code & "'"
                                objDA.Ds4 = objDA.GetData(objDA.strQuery)
                                If objDA.Ds4.Tables(0).Rows.Count > 0 Then
                                    For introw As Integer = 0 To objDA.Ds4.Tables(0).Rows.Count - 1
                                        strSubject = "Purchase Requisition is approved"
                                        StrMailMessage = "Purchase Requisition Approved for the request number is :" & objDA.Ds4.Tables(0).Rows(introw)("DocEntry").ToString() & "" & _
                                     ", Line number is :" & objDA.Ds4.Tables(0).Rows(introw)("LineId").ToString() & " , ItemCode :" & objDA.Ds4.Tables(0).Rows(introw)("U_Z_AltItemCode").ToString() & "" & _
                                      " , ItemName :" & objDA.Ds4.Tables(0).Rows(introw)("U_Z_AltItemName").ToString() & " ,  Quantity :" & objDA.Ds4.Tables(0).Rows(introw)("U_Z_DeliQty").ToString() & "" & _
                                      " , Remarks :" & objDA.Ds4.Tables(0).Rows(introw)("U_Z_RejRemark").ToString() & ""
                                        objDA.SendMail_RequestApproval(StrMailMessage, objEN.EmpId, objEN.SapCompany, strSubject)
                                    Next
                                End If
                                'objDA.strmsg = CreateGoodsIssue(objEN)
                            End If
                          

                        Case "MRD"
                            objDA.strQuery = "Update [@Z_ORPD] Set U_Z_DocStatus='O' Where DocEntry = '" + objEN.Code + "'"
                            oRecordSet.DoQuery(objDA.strQuery)
                            objEN.DeptCode = objDA.getDepartment(objEN.EmpId)
                            intTempID = objDA.GetTemplateID("MR", objEN.DeptCode.Trim())
                            If intTempID <> "0" Then
                                objDA.UpdateApprovalRequired("@Z_RPD1", "DocEntry", objEN.Code, "Y", intTempID)
                                If objEN.Code <> "" Then
                                    objDA.InitialMessage("Material request", objEN.Code, objDA.DocApproval("MR", objEN.DeptCode.Trim()), intTempID, objEN.EmpName.Trim(), "MR", objDA.objMainCompany, objEN.Code)
                                End If

                                strSubject = "Material Return request is Approved in DLC"
                                StrMailMessage = "Material Return request Approved for the request number is :" & objEN.Code & ""
                                objDA.SendMail_RequestApproval(StrMailMessage, objEN.EmpId, objEN.SapCompany, strSubject)
                            Else
                                objDA.UpdateApprovalRequired("@Z_RPD1", "DocEntry", objEN.Code, "N", intTempID)
                                objDA.strQuery = "Update [@Z_RPD1] Set U_Z_LineStatus='D',U_Z_AppStatus='A' Where DocEntry = '" + objEN.Code + "'"
                                oRecordSet.DoQuery(objDA.strQuery)
                                objDA.strQuery = "Update [@Z_RPD1] Set U_Z_DocStatus='I' Where DocEntry = '" + objEN.Code + "'"
                                oRecordSet.DoQuery(objDA.strQuery)
                                objDA.strQuery = "Select * from [@Z_RPD1] where DocEntry='" & objEN.Code & "'"
                                objDA.Ds4 = objDA.GetData(objDA.strQuery)
                                If objDA.Ds4.Tables(0).Rows.Count > 0 Then
                                    For introw As Integer = 0 To objDA.Ds4.Tables(0).Rows.Count - 1
                                        strSubject = "Material Return notification"
                                        StrMailMessage = "Material Return " & objDA.Ds4.Tables(0).Rows(introw)("Status").ToString() & " for the request number is :" & objDA.Ds4.Tables(0).Rows(introw)("DocEntry").ToString() & "" & _
                                     ", Line number is :" & objDA.Ds4.Tables(0).Rows(introw)("LineId").ToString() & " , ItemCode :" & objDA.Ds4.Tables(0).Rows(introw)("U_Z_ItemCode").ToString() & "" & _
                                      " , ItemName :" & objDA.Ds4.Tables(0).Rows(introw)("U_Z_ItemName").ToString() & " ,  Quantity :" & objDA.Ds4.Tables(0).Rows(introw)("U_Z_OrdQty").ToString() & "" & _
                                      " , Remarks :" & objDA.Ds4.Tables(0).Rows(introw)("U_Z_RejRemark").ToString() & ""
                                        objDA.SendMail_RequestApproval(StrMailMessage, objEN.EmpId, objEN.SapCompany, strSubject)
                                    Next
                                End If
                                objDA.strmsg = CreateGoodsReceipt(objEN)
                            End If
                          
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
                            Case "PRD"
                                objDA.strQuery = "Update [@Z_OPRQ] Set U_Z_DocStatus='O' Where DocEntry = '" + objEN.Code + "'"
                                oRecordSet.DoQuery(objDA.strQuery)
                                objEN.DeptCode = objDA.getDepartment(objEN.EmpId)
                                intTempID = objDA.GetTemplateID("PR", objEN.DeptCode.Trim())
                                If intTempID <> "0" Then
                                    objDA.UpdateApprovalRequired("@Z_PRQ1", "DocEntry", objEN.Code, "Y", intTempID)
                                    If objEN.Code <> "" Then
                                        objDA.InitialMessage("Purchase request", objEN.Code, objDA.DocApproval("PR", objEN.DeptCode.Trim()), intTempID, objEN.EmpName.Trim(), "PR", objDA.objMainCompany, objEN.Code)
                                    End If
                                    strSubject = "Purchase Requisition is Approved in DLC"
                                    StrMailMessage = "Purchase Requisition Approved for the request number is :" & objEN.Code & ""
                                    objDA.SendMail_RequestApproval(StrMailMessage, objEN.EmpId, objEN.SapCompany, strSubject)
                                Else
                                    objDA.UpdateApprovalRequired("@Z_PRQ1", "DocEntry", objEN.Code, "N", intTempID)
                                    objDA.strQuery = "Update [@Z_PRQ1] Set U_Z_LineStatus='D',U_Z_AppStatus='A' Where DocEntry = '" + objEN.Code + "'"
                                    oRecordSet.DoQuery(objDA.strQuery)
                                    objDA.strQuery = "Update [@Z_OPRQ] Set U_Z_DocStatus='I' Where DocEntry = '" + objEN.Code + "'"
                                    oRecordSet.DoQuery(objDA.strQuery)
                                    objDA.strQuery = "Select * from [@Z_PRQ1] where DocEntry='" & objEN.Code & "'"
                                    objDA.Ds4 = objDA.GetData(objDA.strQuery)
                                    If objDA.Ds4.Tables(0).Rows.Count > 0 Then
                                        For introw As Integer = 0 To objDA.Ds4.Tables(0).Rows.Count - 1
                                            strSubject = "Purchase Requisition is approved"
                                            StrMailMessage = "Purchase Requisition Approved for the request number is :" & objDA.Ds4.Tables(0).Rows(introw)("DocEntry").ToString() & "" & _
                                         ", Line number is :" & objDA.Ds4.Tables(0).Rows(introw)("LineId").ToString() & " , ItemCode :" & objDA.Ds4.Tables(0).Rows(introw)("U_Z_AltItemCode").ToString() & "" & _
                                          " , ItemName :" & objDA.Ds4.Tables(0).Rows(introw)("U_Z_AltItemName").ToString() & " ,  Quantity :" & objDA.Ds4.Tables(0).Rows(introw)("U_Z_DeliQty").ToString() & "" & _
                                          " , Remarks :" & objDA.Ds4.Tables(0).Rows(introw)("U_Z_RejRemark").ToString() & ""
                                            objDA.SendMail_RequestApproval(StrMailMessage, objEN.EmpId, objEN.SapCompany, strSubject)
                                        Next
                                    End If
                                    ' objDA.strmsg = CreateGoodsIssue(objEN)
                                End If
                             
                            Case "MRD"
                                objDA.strQuery = "Update [@Z_ORPD] Set U_Z_DocStatus='O' Where DocEntry = '" + objEN.Code + "'"
                                oRecordSet.DoQuery(objDA.strQuery)
                                objEN.DeptCode = objDA.getDepartment(objEN.EmpId)
                                intTempID = objDA.GetTemplateID("MR", objEN.DeptCode.Trim())
                                If intTempID <> "0" Then
                                    objDA.UpdateApprovalRequired("@Z_RPD1", "DocEntry", objEN.Code, "Y", intTempID)
                                    If objEN.Code <> "" Then
                                        objDA.InitialMessage("Material request", objEN.Code, objDA.DocApproval("MR", objEN.DeptCode.Trim()), intTempID, objEN.EmpName.Trim(), "MR", objDA.objMainCompany, objEN.Code)
                                    End If
                                    strSubject = "Material Return request is Approved in DLC"
                                    StrMailMessage = "Material Return request Approved for the request number is :" & objEN.Code & ""
                                    objDA.SendMail_RequestApproval(StrMailMessage, objEN.EmpId, objEN.SapCompany, strSubject)
                                Else
                                    objDA.UpdateApprovalRequired("@Z_RPD1", "DocEntry", objEN.Code, "N", intTempID)
                                    objDA.strQuery = "Update [@Z_RPD1] Set U_Z_LineStatus='D',U_Z_AppStatus='A' Where DocEntry = '" + objEN.Code + "'"
                                    oRecordSet.DoQuery(objDA.strQuery)
                                    objDA.strQuery = "Update [@Z_ORPD] Set U_Z_DocStatus='C' Where DocEntry = '" + objEN.Code + "'"
                                    oRecordSet.DoQuery(objDA.strQuery)
                                    objDA.strQuery = "Select * from [@Z_RPD1] where DocEntry='" & objEN.Code & "'"
                                    objDA.Ds4 = objDA.GetData(objDA.strQuery)
                                    If objDA.Ds4.Tables(0).Rows.Count > 0 Then
                                        For introw As Integer = 0 To objDA.Ds4.Tables(0).Rows.Count - 1
                                            strSubject = "Material Return notification"
                                            StrMailMessage = "Material Return Approved for the request number is :" & objDA.Ds4.Tables(0).Rows(introw)("DocEntry").ToString() & "" & _
                                         ", Line number is :" & objDA.Ds4.Tables(0).Rows(introw)("LineId").ToString() & " , ItemCode :" & objDA.Ds4.Tables(0).Rows(introw)("U_Z_ItemCode").ToString() & "" & _
                                          " , ItemName :" & objDA.Ds4.Tables(0).Rows(introw)("U_Z_ItemName").ToString() & " ,  Quantity :" & objDA.Ds4.Tables(0).Rows(introw)("U_Z_OrdQty").ToString() & "" & _
                                          " , Remarks :" & objDA.Ds4.Tables(0).Rows(introw)("U_Z_RejRemark").ToString() & ""
                                            objDA.SendMail_RequestApproval(StrMailMessage, objEN.EmpId, objEN.SapCompany, strSubject)
                                        Next
                                    End If
                                    objDA.strmsg = CreateGoodsReceipt(objEN)
                                End If
                        End Select
                    End If
                End If
            ElseIf objEN.AppStatus = "R" Then
                objDA.strQuery = " Select T2.DocEntry "
                objDA.strQuery += " From [@Z_DLC_APPT2] T2 "
                objDA.strQuery += " JOIN [@Z_DLC_OAPPT] T3 ON T2.DocEntry = T3.DocEntry  "
                objDA.strQuery += " LEFT OUTER JOIN [@Z_DLC_APPT3] T4 ON T4.DocEntry = T3.DocEntry  "
                objDA.strQuery += " Where  (T3.U_Z_AllDept='Y' or T4.U_Z_DeptCode='" & objEN.DeptCode & "')"
                objDA.strQuery += " And T2.U_Z_AUser = '" + objEN.UserCode + "' And T3.U_Z_DocType = '" & objEN.HistoryType & "'"
                oRecordSet.DoQuery(objDA.strQuery)
                If Not oRecordSet.EoF Then
                    Select Case objEN.HistoryType
                        Case "PRD"
                            objDA.strQuery = "Update [@Z_OPRQ] Set U_Z_DocStatus='R' Where DocEntry = '" & objEN.Code & "'"
                            oRecordSet.DoQuery(objDA.strQuery)
                            strSubject = "Purchase Requisition is rejected in DLC"
                            StrMailMessage = "Purchase Requisition rejected for the request number is :" & objEN.Code & ""
                            objDA.SendMail_RequestApproval(StrMailMessage, objEN.EmpId, objEN.SapCompany, strSubject)
                        Case "MRD"
                            objDA.strQuery = "Update [@Z_ORPD] Set U_Z_DocStatus='R' Where DocEntry = '" & objEN.Code & "' "
                            oRecordSet.DoQuery(objDA.strQuery)
                            strSubject = "Material Return request is rejected in DLC"
                            StrMailMessage = "Material Return rejected for the request number is :" & objEN.Code & ""
                            objDA.SendMail_RequestApproval(StrMailMessage, objEN.EmpId, objEN.SapCompany, strSubject)
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
    Private Function CreateGoodsIssue(ByVal objEN As PRApprovalEN) As String
        Dim blnLineExists As Boolean
        Dim strMailCode As String = ""
        Dim dblQuantity As Double
        Dim oDocument As SAPbobsCOM.Documents
        Dim oRecordSet, oTemp As SAPbobsCOM.Recordset
        oRecordSet = objEN.SAPCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
        oTemp = objEN.SAPCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
        oDocument = objEN.SAPCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenExit)
        Try
            objDA.strQuery = "select * from [@Z_PRQ1]  Where DocENtry = '" + objEN.Code + "' and isnull(U_Z_DocNo,'')='' and U_Z_AppStatus='A' and U_Z_GoodIssue='N'"
            oRecordSet.DoQuery(objDA.strQuery)
            blnLineExists = False
            If oRecordSet.RecordCount > 0 Then
                For intLoop As Integer = 0 To oRecordSet.RecordCount - 1

                    If 1 = 1 Then
                        oDocument.DocDate = Now.Date
                        oDocument.Comments = oRecordSet.Fields.Item("U_Z_RejRemark").Value
                        If intLoop > 0 Then
                            oDocument.Lines.Add()
                        End If
                        dblQuantity = oRecordSet.Fields.Item("U_Z_DeliQty").Value
                        If dblQuantity < 0 Then
                            dblQuantity = dblQuantity * -1
                        End If
                        oDocument.Lines.SetCurrentLine(intLoop)
                        oDocument.Lines.ItemCode = oRecordSet.Fields.Item("U_Z_AltItemCode").Value
                        oDocument.Lines.Quantity = dblQuantity
                        oDocument.Lines.WarehouseCode = objEN.WhsCode
                        If oRecordSet.Fields.Item("U_Z_DelUom").Value <> -1 Then
                            oDocument.Lines.UseBaseUnits = SAPbobsCOM.BoYesNoEnum.tNO
                            oDocument.Lines.UoMEntry = oRecordSet.Fields.Item("U_Z_DelUom").Value
                        End If
                        If strMailCode = "" Then
                            strMailCode = Integer.Parse(oRecordSet.Fields.Item("LineId").Value)
                        Else
                            strMailCode = strMailCode & "," & Integer.Parse(oRecordSet.Fields.Item("LineId").Value)
                        End If
                        blnLineExists = True
                    End If
                    oRecordSet.MoveNext()
                Next
                If blnLineExists = True Then
                    If oDocument.Add <> 0 Then
                        objDA.strmsg = objEN.SapCompany.GetLastErrorDescription
                        ErrHandler.WriteError(objDA.strmsg)
                    Else
                        Dim strdocCode As String
                        objEN.SapCompany.GetNewObjectCode(strdocCode)
                        If oDocument.GetByKey(strdocCode) Then
                            oTemp.DoQuery("Update [@Z_PRQ1] set U_Z_DocNo='" & oDocument.DocNum & "',U_Z_GoodIssue='Y' where DocEntry='" & objEN.Code & "'")
                        End If
                    End If
                End If
            End If
            objDA.strmsg = "Success"
        Catch ex As Exception
            objDA.strmsg = ex.Message
            ErrHandler.WriteError(objDA.strmsg)
        End Try
        Return objDA.strmsg
    End Function

    Private Function CreateGoodsReceipt(ByVal objEN As PRApprovalEN) As String
        Dim blnLineExists As Boolean
        Dim strMailCode As String = ""
        Dim dblQuantity As Double
        Dim oDocument As SAPbobsCOM.Documents
        Dim oRecordSet, oTemp As SAPbobsCOM.Recordset
        oRecordSet = objEN.SapCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
        oTemp = objEN.SapCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
        oDocument = objEN.SapCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenEntry)
        Try
            objDA.strQuery = "select * from [@Z_RPD1] Where DocEntry = '" + objEN.DocEntry + "' and isnull(U_Z_DocRefNo,'')='' and U_Z_AppStatus='A' and U_Z_GoodReceipt='N'"
            oRecordSet.DoQuery(objDA.strQuery)
            blnLineExists = False
            If oRecordSet.RecordCount > 0 Then
                For intLoop As Integer = 0 To oRecordSet.RecordCount - 1
                    If 1 = 1 Then
                        oDocument.DocDate = Now.Date
                        oDocument.Comments = oRecordSet.Fields.Item("U_Z_RejRemark").Value
                        If intLoop > 0 Then
                            oDocument.Lines.Add()
                        End If
                        dblQuantity = oRecordSet.Fields.Item("U_Z_OrdQty").Value
                        If dblQuantity < 0 Then
                            dblQuantity = dblQuantity * -1
                        End If
                        oDocument.Lines.SetCurrentLine(intLoop)
                        oDocument.Lines.ItemCode = oRecordSet.Fields.Item("U_Z_ItemCode").Value
                        oDocument.Lines.Quantity = dblQuantity
                        oDocument.Lines.WarehouseCode = objEN.WhsCode
                        If oRecordSet.Fields.Item("U_Z_OrdUom").Value <> -1 Then
                            oDocument.Lines.UseBaseUnits = SAPbobsCOM.BoYesNoEnum.tNO
                            oDocument.Lines.UoMEntry = oRecordSet.Fields.Item("U_Z_OrdUom").Value
                        End If
                        If strMailCode = "" Then
                            strMailCode = Integer.Parse(oRecordSet.Fields.Item("LineId").Value)
                        Else
                            strMailCode = strMailCode & "," & Integer.Parse(oRecordSet.Fields.Item("LineId").Value)
                        End If
                        blnLineExists = True
                    End If
                    oRecordSet.MoveNext()
                Next
                If blnLineExists = True Then
                    If oDocument.Add <> 0 Then
                        objDA.strmsg = objEN.SapCompany.GetLastErrorDescription
                        ErrHandler.WriteError(objDA.strmsg)
                    Else
                        Dim strdocCode As String
                        objEN.SapCompany.GetNewObjectCode(strdocCode)
                        If oDocument.GetByKey(strdocCode) Then
                            oTemp.DoQuery("Update [@Z_RPD1] set U_Z_DocRefNo='" & oDocument.DocNum & "',U_Z_GoodReceipt='Y' where LineId in (" & strMailCode & ") and DocEntry='" & objEN.DocEntry & "' ")
                        End If
                    End If
                End If
            End If
            objDA.strmsg = "Success"
        Catch ex As Exception
            objDA.strmsg = ex.Message
            ErrHandler.WriteError(objDA.strmsg)
        End Try
        Return objDA.strmsg
    End Function
End Class
