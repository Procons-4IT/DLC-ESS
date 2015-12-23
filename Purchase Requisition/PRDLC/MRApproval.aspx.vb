Imports System
Imports System.Drawing
Imports System.Globalization
Imports System.Configuration
Imports System.Xml
Imports System.IO
Imports BL
Imports DAL
Imports EN
Public Class MRApproval
    Inherits System.Web.UI.Page
    Dim dbCon As DBConnectionDA = New DBConnectionDA()
    Dim objBL As MRApprovalBL = New MRApprovalBL()
    Dim objBL1 As PRApprovalBL = New PRApprovalBL()
    Dim objEN As PRApprovalEN = New PRApprovalEN()
    Dim strMailDocEntry As String = ""
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                If Session("UserCode") Is Nothing Then
                    Response.Redirect("Login.aspx?sessionExpired=true", True)
                ElseIf Session("SAPCompany") Is Nothing Then
                    If Session("EmpUserName").ToString() = "" Or Session("UserPwd").ToString() = "" Then
                        strError = dbCon.Connection()
                    Else
                        strError = dbCon.Connection(Session("EmpUserName").ToString(), Session("UserPwd").ToString())
                    End If
                    If strError <> "Success" Then
                        Page.ClientScript.RegisterStartupScript(Me.GetType(), "js", "<script>alert('" & strError & "')</script>")
                    Else
                        Session("SAPCompany") = dbCon.objMainCompany
                    End If
                End If
                objEN.EmpId = Session("UserCode").ToString()
                objEN.UserCode = objBL.GetUserCode(objEN)
                PanelMain.Visible = True
                panelview.Visible = False
                ReqApproval(objEN)
            End If
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
        End Try
    End Sub
    Private Sub ReqApproval(ByVal objEN As PRApprovalEN)
        Try
            dbCon.ds = objBL.MainGridBind(objEN)
            If dbCon.ds.Tables(0).Rows.Count > 0 Then
                GrdLoadRequest.DataSource = dbCon.ds.Tables(0)
                GrdLoadRequest.DataBind()
            Else
                GrdLoadRequest.DataBind()
            End If

            If dbCon.ds.Tables(1).Rows.Count > 0 Then
                grdSummaryLoad.DataSource = dbCon.ds.Tables(1)
                grdSummaryLoad.DataBind()
            Else
                grdSummaryLoad.DataBind()
            End If

            If dbCon.ds.Tables(2).Rows.Count > 0 Then
                ddldestination.DataTextField = "WhsName"
                ddldestination.DataValueField = "WhsCode"
                ddldestination.DataSource = dbCon.ds.Tables(2)
                ddldestination.DataBind()
            Else
                ddldestination.DataBind()
            End If
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            dbCon.strmsg = "" & ex.Message & ""
            ClientScript.RegisterStartupScript(Me.GetType(), "msg", "<script>alert('" & dbCon.strmsg & "')</script>")
        End Try
    End Sub
    Protected Sub lnbtnlblRCode_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            If Session("UserCode") Is Nothing Or Session("SAPCompany") Is Nothing Then
                dbCon.strmsg = "Your session is Expired..."
                ClientScript.RegisterStartupScript(Me.GetType(), "msg", "<script>alert('" & dbCon.strmsg & "')</script>")
                Response.Redirect("Login.aspx?sessionExpired=true", True)
            Else
                Dim link As LinkButton = CType(sender, LinkButton)
                Dim gv As GridViewRow = CType((link.Parent.Parent), GridViewRow)
                Dim DocNo As LinkButton = CType(gv.FindControl("lblRCode"), LinkButton)
                Dim introw As Integer = gv.RowIndex
                For Each row1 As GridViewRow In grdRequestApproval.Rows
                    If row1.RowIndex <> introw Then
                        row1.BackColor = Color.White
                    Else
                        row1.BackColor = Color.LimeGreen
                    End If
                Next
                objEN.EmpId = Session("UserCode").ToString()
                objEN.UserCode = objBL.GetUserCode(objEN)
                objEN.DocEntry = DocNo.Text.Trim()
                BindPRequestApproval(objEN)
                grdApprovalHis.DataBind()
            End If
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            dbCon.strmsg = "" & ex.Message & ""
            ClientScript.RegisterStartupScript(Me.GetType(), "msg", "<script>alert('" & dbCon.strmsg & "')</script>")
        End Try
    End Sub
    Private Sub BindPRequestApproval(ByVal objEN As PRApprovalEN)
        Try
            dbCon.Ds1 = objBL.BindPRequestApproval(objEN)
            If dbCon.Ds1.Tables(0).Rows.Count > 0 Then
                grdRequestApproval.DataSource = dbCon.Ds1.Tables(0)
                grdRequestApproval.DataBind()
            Else
                grdRequestApproval.DataBind()
            End If
            If dbCon.Ds1.Tables(1).Rows.Count > 0 Then
                lbldocno.Text = dbCon.Ds1.Tables(1).Rows(0)("DocEntry").ToString()
                lblsubdt.Text = dbCon.Ds1.Tables(1).Rows(0)("U_Z_DocDate").ToString()
                lblempNo.Text = dbCon.Ds1.Tables(1).Rows(0)("U_Z_EmpID").ToString()
                lblempname.Text = dbCon.Ds1.Tables(1).Rows(0)("U_Z_EmpName").ToString()
                lbldept.Text = dbCon.Ds1.Tables(1).Rows(0)("U_Z_DeptCode").ToString()
                lbldeptName.Text = dbCon.Ds1.Tables(1).Rows(0)("U_Z_DeptName").ToString()
                ddldestination.SelectedValue = dbCon.Ds1.Tables(1).Rows(0)("U_Z_Destination").ToString()
                lblDocStatus.Text = dbCon.Ds1.Tables(1).Rows(0)("U_Z_DocStatus").ToString()
            End If
            PanelMain.Visible = False
            panelview.Visible = True
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            dbCon.strmsg = "" & ex.Message & ""
            ClientScript.RegisterStartupScript(Me.GetType(), "msg", "<script>alert('" & dbCon.strmsg & "')</script>")
        End Try
    End Sub
    Private Sub btncancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncancel.Click
        PanelMain.Visible = True
        panelview.Visible = False
    End Sub
    Private Sub grdRequestApproval_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdRequestApproval.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblStatus As Label = CType(e.Row.FindControl("lblsgoods"), Label)
            If lblStatus.Text.Trim() = "" Then
                e.Row.Cells(1).Enabled = True
            Else
                e.Row.Cells(1).Enabled = False
            End If
        End If
    End Sub
    Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Dim StrMailMessage As String = ""
        Dim strSubject As String
        Dim row1 As GridViewRow
        Dim blnFlag As Boolean = False
        Dim blnFlag1 As Boolean = True
        objEN.SapCompany = Session("SAPCompany")
        Dim oRecordSet As SAPbobsCOM.Recordset
        oRecordSet = objEN.SapCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
        Try
            If Session("UserCode") Is Nothing Or Session("SAPCompany") Is Nothing Then
                dbCon.strmsg = "Your session is Expired..."
                ClientScript.RegisterStartupScript(Me.GetType(), "msg", "<script>alert('" & dbCon.strmsg & "')</script>")
                Response.Redirect("Login.aspx?sessionExpired=true", True)
            Else
                objEN.EmpId = Session("UserCode").ToString()
                objEN.UserCode = objBL.GetUserCode(objEN)
                objEN.EmpUserId = objBL1.GetEmpUserid(objEN)
                objEN.DeptCode = lbldept.Text.Trim()
                For Each row1 In grdRequestApproval.Rows
                    Dim chkRow As CheckBox = TryCast(row1.Cells(0).FindControl("chkGoods"), CheckBox)
                    If chkRow.Checked Then
                        objEN.EmpId = lblempNo.Text.Trim()
                        objEN.DocEntry = lbldocno.Text.Trim()
                        objEN.DocLineId = CType(row1.FindControl("lblCode"), LinkButton).Text
                        objEN.LineStatus = CType(row1.FindControl("lblsLinests"), Label).Text
                        objEN.AppStatus = CType(row1.FindControl("ddlAppStatus"), DropDownList).SelectedValue
                        objEN.Remarks = CType(row1.FindControl("txtRemarks"), TextBox).Text
                        objEN.GoodsDocNo = CType(row1.FindControl("lblsgoods"), Label).Text
                        If objEN.AppStatus = "P" Then
                            objEN.LineStatus = "O"
                        ElseIf objEN.AppStatus = "A" Then
                            objEN.LineStatus = "D"
                        ElseIf objEN.AppStatus = "R" Then
                            objEN.LineStatus = "L"
                        End If
                        objEN.ItemCode = CType(row1.FindControl("lblsItemCode"), Label).Text
                        objEN.Itemdesc = CType(row1.FindControl("lblsItemName"), Label).Text
                        objEN.OrdrQty = CType(row1.FindControl("lblsordQty"), Label).Text
                        objEN.OrdrUom = CType(row1.FindControl("lblsordUomdec"), Label).Text
                        objEN.OrdrUomDesc = CType(row1.FindControl("lblsordUom"), Label).Text
                        objEN.HistoryType = "MR"
                        objEN.HeaderType = "MR"
                        objEN.DocMessage = "Material Return"
                        objEN.SapCompany = Session("SAPCompany")
                        dbCon.strmsg = objBL1.addUpdateDocument(objEN)
                        If dbCon.strmsg <> "Success" And dbCon.strmsg <> "Successfully approved document..." Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "msg", "<script>alert('" & dbCon.strmsg & "')</script>")
                        End If
                        If objEN.LineStatus = "D" Then
                            If strMailDocEntry = "" Then
                                strMailDocEntry = Integer.Parse(objEN.DocLineId)
                            Else
                                strMailDocEntry = strMailDocEntry & "," & Integer.Parse(objEN.DocLineId)
                            End If
                        End If
                        If objEN.LineStatus = "O" Then
                            blnFlag = True
                        Else
                            blnFlag1 = False
                        End If
                    End If
                Next
            

                objEN.DeptCode = lbldept.Text.Trim()
                objEN.EmpId = lblempNo.Text.Trim()
                objEN.DocEntry = lbldocno.Text.Trim()

                dbCon.strmsg = CreateGoodsReceipt(objEN)
                objEN.EmpId = Session("UserCode").ToString()
                objEN.UserCode = objBL.GetUserCode(objEN)
                objEN.SapCompany = Session("SAPCompany")
                If strMailDocEntry <> "" Then
                    dbCon.SendMessage(lbldocno.Text.Trim(), lbldept.Text.Trim(), objEN.UserCode, strMailDocEntry, objEN.SapCompany, lblempname.Text.Trim(), "MR")
                    dbCon.strQuery = "Select case U_Z_AppStatus when 'A' then 'Approved' when 'R' then 'Rejected' else 'Pending' end as 'Status', * from [@Z_RPD1] where DocEntry='" & objEN.Code & "' and LineId in (" & strMailDocEntry & ")"
                    dbCon.Ds4 = dbCon.GetData(dbCon.strQuery)
                    If dbCon.Ds4.Tables(0).Rows.Count > 0 Then
                        For introw As Integer = 0 To dbCon.Ds4.Tables(0).Rows.Count - 1
                            strSubject = "Material Return notification"
                            StrMailMessage = "Material Return " & dbCon.Ds4.Tables(0).Rows(introw)("Status").ToString() & " for the request number is :" & dbCon.Ds4.Tables(0).Rows(introw)("DocEntry").ToString() & "" & _
                         ", Line number is :" & dbCon.Ds4.Tables(0).Rows(introw)("LineId").ToString() & " , ItemCode :" & dbCon.Ds4.Tables(0).Rows(introw)("U_Z_ItemCode").ToString() & "" & _
                          " , ItemName :" & dbCon.Ds4.Tables(0).Rows(introw)("U_Z_ItemName").ToString() & " ,  Quantity :" & dbCon.Ds4.Tables(0).Rows(introw)("U_Z_OrdQty").ToString() & "" & _
                          " , Remarks :" & dbCon.Ds4.Tables(0).Rows(introw)("U_Z_RejRemark").ToString() & ""
                            dbCon.SendMail_RequestApproval(StrMailMessage, objEN.EmpId, objEN.SapCompany, strSubject)
                        Next
                    End If
                End If
                If blnFlag = blnFlag1 Then
                    dbCon.strQuery = "Update [@Z_ORPD] set U_Z_DocStatus='C' where DocEntry='" & lbldocno.Text.Trim() & "'"
                Else
                    dbCon.strQuery = "Update [@Z_ORPD] set U_Z_DocStatus='I' where DocEntry='" & lbldocno.Text.Trim() & "'"
                End If
                oRecordSet.DoQuery(dbCon.strQuery)
                PanelMain.Visible = True
                panelview.Visible = False
                ClientScript.RegisterStartupScript(Me.GetType(), "msg", "<script>alert('" & dbCon.strmsg & "')</script>")
                objEN.EmpId = Session("UserCode").ToString()
                objEN.UserCode = objBL.GetUserCode(objEN)
                ReqApproval(objEN)
            End If
            ClientScript.RegisterStartupScript(Me.GetType(), "msg", "<script>alert('" & dbCon.strmsg & "')</script>")
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            dbCon.strmsg = "" & ex.Message & ""
            ClientScript.RegisterStartupScript(Me.GetType(), "msg", "<script>alert('" & dbCon.strmsg & "')</script>")
        End Try
    End Sub
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
            dbCon.strQuery = "select * from [@Z_RPD1] Where DocEntry = '" + objEN.DocEntry + "' and isnull(U_Z_DocRefNo,'')='' and U_Z_AppStatus='A' and U_Z_GoodReceipt='N'"
            oRecordSet.DoQuery(dbCon.strQuery)
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
                        oDocument.Lines.WarehouseCode = ddldestination.SelectedValue
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
                        dbCon.strmsg = objEN.SapCompany.GetLastErrorDescription
                        ErrHandler.WriteError(dbCon.strmsg)
                    Else
                        Dim strdocCode As String
                        objEN.SapCompany.GetNewObjectCode(strdocCode)
                        If oDocument.GetByKey(strdocCode) Then
                            oTemp.DoQuery("Update [@Z_RPD1] set U_Z_DocRefNo='" & oDocument.DocNum & "',U_Z_GoodReceipt='Y' where LineId in (" & strMailCode & ") and DocEntry='" & objEN.DocEntry & "' ")
                        End If
                    End If
                End If
            End If
            dbCon.strmsg = "Success"
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            dbCon.strmsg = ex.Message
        End Try
        Return dbCon.strmsg
    End Function
    Protected Sub lbtnlblSCode_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            If Session("UserCode") Is Nothing Or Session("SAPCompany") Is Nothing Then
                dbCon.strmsg = "Your session is Expired..."
                ClientScript.RegisterStartupScript(Me.GetType(), "msg", "<script>alert('" & dbCon.strmsg & "')</script>")
                Response.Redirect("Login.aspx?sessionExpired=true", True)
            Else
                Dim link As LinkButton = CType(sender, LinkButton)
                Dim gv As GridViewRow = CType((link.Parent.Parent), GridViewRow)
                Dim LineId As LinkButton = CType(gv.FindControl("lblSCode"), LinkButton)
                Dim DocEntry As Label = CType(gv.FindControl("lblsDocEntry"), Label)
                Dim introw As Integer = gv.RowIndex
                For Each row1 As GridViewRow In grdSummary.Rows
                    If row1.RowIndex <> introw Then
                        row1.BackColor = Color.White
                    Else
                        row1.BackColor = Color.Orange
                    End If
                Next
                objEN.EmpId = Session("UserCode").ToString()
                objEN.DocEntry = DocEntry.Text.Trim()
                objEN.HeadLineId = LineId.Text.Trim()
                BindSummaryHistory(objEN)
                ModalPopupExtender6.Show()
            End If
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            dbCon.strmsg = "" & ex.Message & ""
            ClientScript.RegisterStartupScript(Me.GetType(), "msg", "<script>alert('" & dbCon.strmsg & "')</script>")
        End Try
    End Sub
    Private Sub BindSummaryHistory(ByVal objEN As PRApprovalEN)
        Try
            dbCon.Ds2 = objBL.LoadHistory(objEN)
            If dbCon.Ds2.Tables(0).Rows.Count > 0 Then
                grdHistorySummary.DataSource = dbCon.Ds2.Tables(0)
                grdHistorySummary.DataBind()
            Else
                grdHistorySummary.DataBind()
            End If
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            dbCon.strmsg = "'" & ex.Message & "'"
            ClientScript.RegisterStartupScript(Me.GetType(), "msg", "<script>alert('" & dbCon.strmsg & "')</script>")
        End Try
    End Sub
    Protected Sub lnbtnlblSCode_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            If Session("UserCode") Is Nothing Or Session("SAPCompany") Is Nothing Then
                dbCon.strmsg = "Your session is Expired..."
                ClientScript.RegisterStartupScript(Me.GetType(), "msg", "<script>alert('" & dbCon.strmsg & "')</script>")
                Response.Redirect("Login.aspx?sessionExpired=true", True)
            Else
                Dim link As LinkButton = CType(sender, LinkButton)
                Dim gv As GridViewRow = CType((link.Parent.Parent), GridViewRow)
                Dim DocNo As LinkButton = CType(gv.FindControl("lblSCode"), LinkButton)
                Dim introw As Integer = gv.RowIndex
                For Each row1 As GridViewRow In grdSummaryLoad.Rows
                    If row1.RowIndex <> introw Then
                        row1.BackColor = Color.White
                    Else
                        row1.BackColor = Color.Orange
                    End If
                Next
                objEN.EmpId = Session("UserCode").ToString()
                objEN.UserCode = objBL.GetUserCode(objEN)
                objEN.DocEntry = DocNo.Text.Trim()
                BindExpenseSummaryApproval(objEN)
            End If
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            dbCon.strmsg = "" & ex.Message & ""
            ClientScript.RegisterStartupScript(Me.GetType(), "msg", "<script>alert('" & dbCon.strmsg & "')</script>")
        End Try
    End Sub
    Private Sub BindExpenseSummaryApproval(ByVal objEN As PRApprovalEN)
        Try
            dbCon.Ds3 = objBL.BindExpenseSummaryApproval(objEN)
            If dbCon.Ds3.Tables(0).Rows.Count > 0 Then
                grdSummary.DataSource = dbCon.Ds3.Tables(0)
                grdSummary.DataBind()
            Else
                grdSummary.DataBind()
            End If
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            dbCon.strmsg = "" & ex.Message & ""
            ClientScript.RegisterStartupScript(Me.GetType(), "msg", "<script>alert('" & dbCon.strmsg & "')</script>")
        End Try
    End Sub
    Protected Sub lbtnlblCode_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            If Session("UserCode") Is Nothing Or Session("SAPCompany") Is Nothing Then
                dbCon.strmsg = "Your session is Expired..."
                ClientScript.RegisterStartupScript(Me.GetType(), "msg", "<script>alert('" & dbCon.strmsg & "')</script>")
                Response.Redirect("Login.aspx?sessionExpired=true", True)
            Else
                Dim link As LinkButton = CType(sender, LinkButton)
                Dim gv As GridViewRow = CType((link.Parent.Parent), GridViewRow)
                Dim LineId As LinkButton = CType(gv.FindControl("lblCode"), LinkButton)
                Dim DocNo As LinkButton = CType(gv.FindControl("lblRefCode"), LinkButton)
                objEN.EmpId = Session("UserCode").ToString()
                objEN.DocEntry = lbldocno.Text.Trim()
                objEN.HeadLineId = LineId.Text.Trim()
                Dim introw As Integer = gv.RowIndex
                For Each row1 As GridViewRow In grdRequestApproval.Rows
                    If row1.RowIndex <> introw Then
                        row1.BackColor = Color.White
                    Else
                        row1.BackColor = Color.Orange
                    End If
                Next
                BindHistory(objEN)
            End If
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            dbCon.strmsg = "" & ex.Message & ""
            ClientScript.RegisterStartupScript(Me.GetType(), "msg", "<script>alert('" & dbCon.strmsg & "')</script>")
        End Try
    End Sub
    Private Sub BindHistory(ByVal objEN As PRApprovalEN)
        Try
            dbCon.Ds2 = objBL.LoadHistory(objEN)
            If dbCon.Ds2.Tables(0).Rows.Count > 0 Then
                grdApprovalHis.DataSource = dbCon.Ds2.Tables(0)
                grdApprovalHis.DataBind()
            Else
                grdApprovalHis.DataBind()
            End If
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            dbCon.strmsg = "'" & ex.Message & "'"
            ClientScript.RegisterStartupScript(Me.GetType(), "msg", "<script>alert('" & dbCon.strmsg & "')</script>")
        End Try
    End Sub

    Private Sub GrdLoadRequest_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GrdLoadRequest.PageIndexChanging
        GrdLoadRequest.PageIndex = e.NewPageIndex
        objEN.EmpId = Session("UserCode").ToString()
        objEN.UserCode = objBL.GetUserCode(objEN)
        ReqApproval(objEN)
    End Sub

    Private Sub grdSummaryLoad_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdSummaryLoad.PageIndexChanging
        grdSummaryLoad.PageIndex = e.NewPageIndex
        objEN.EmpId = Session("UserCode").ToString()
        objEN.UserCode = objBL.GetUserCode(objEN)
        ReqApproval(objEN)
    End Sub
End Class