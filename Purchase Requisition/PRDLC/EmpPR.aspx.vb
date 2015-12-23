Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Configuration
Imports System.Xml
Imports System.Collections.Generic
Imports System.IO
Imports System.Net
Imports EN
Imports DAL
Imports BL
Public Class EmpPR
    Inherits System.Web.UI.Page
    Dim dbcon As DBConnectionDA = New DBConnectionDA()
    Dim objBL As EmpPRBL = New EmpPRBL()
    Dim objEN As EmpPREN = New EmpPREN()
    Dim oRecSet As SAPbobsCOM.Recordset
    Dim LineDocEntry, strMailCode, intTempID As String
    Dim dblQty As Double
    Public Sub New()
        dbcon.con = New SqlConnection(dbcon.GetConnection)
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                If Session("UserCode") Is Nothing Then
                    Response.Redirect("Login.aspx?sessionExpired=true", True)
                ElseIf Session("SAPCompany") Is Nothing Then
                    If Session("EmpUserName").ToString() = "" Or Session("UserPwd").ToString() = "" Then
                        strError = dbcon.Connection()
                    Else
                        strError = dbcon.Connection(Session("EmpUserName").ToString(), Session("UserPwd").ToString())
                    End If
                    If strError <> "Success" Then
                        Page.ClientScript.RegisterStartupScript(Me.GetType(), "js", "<script>alert('" & strError & "')</script>")
                    Else
                        Session("SAPCompany") = dbcon.objMainCompany
                        objEN.SAPCompany = Session("SAPCompany")
                    End If
                End If
                btnRecSubmit.Visible = False
                panelview.Visible = True
                PanelNewRequest.Visible = False
                objEN.EmpId = Session("UserCode").ToString()
                objEN.SAPCompany = Session("SAPCompany")
                dbcon.strmsg = objBL.DeleteTempTable(objEN)
                PageLoadBind(objEN)
                objEN.SessionId = Session("SessionId").ToString()
            End If
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            dbcon.strmsg = "alert('" & ex.Message & "')"
            mess(dbcon.strmsg)
        End Try
    End Sub
    Private Sub PageLoadBind(ByVal objEN As EmpPREN)
        Try
            dbcon.ds = objBL.PopulateEmployee(objEN)
            If dbcon.ds.Tables(0).Rows.Count > 0 Then
                lbldept.Text = dbcon.ds.Tables(0).Rows(0)("dept").ToString()
                lbldeptName.Text = dbcon.ds.Tables(0).Rows(0)("DeptName").ToString()
            End If
            If dbcon.ds.Tables(1).Rows.Count > 0 Then
                grdPurchaseRequest.DataSource = dbcon.ds.Tables(1)
                grdPurchaseRequest.DataBind()
            Else
                grdPurchaseRequest.DataBind()
            End If
            dbcon.Ds2 = objBL.BindItemCode(objEN)
            If dbcon.Ds2.Tables(0).Rows.Count > 0 Then
                grdItems.DataSource = dbcon.Ds2.Tables(0)
                grdItems.DataBind()
            Else
                grdItems.DataBind()
            End If
            dbcon.dss2 = objBL.BindWhs(objEN)
            If dbcon.dss2.Tables(0).Rows.Count > 0 Then
                ddldestination.DataTextField = "WhsName"
                ddldestination.DataValueField = "WhsCode"
                ddldestination.DataSource = dbcon.dss2.Tables(0)
                ddldestination.DataBind()
            Else
                ddldestination.DataBind()
            End If
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            dbcon.strmsg = "alert('" & ex.Message & "')"
            mess(dbcon.strmsg)
        End Try
    End Sub
    Protected Sub Btncallpop_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btncallpop.ServerClick
        Try
            Dim str1, str2, str3, str4 As String
            str1 = txtpopunique.Text.Trim()
            str2 = txtpoptno.Text.Trim()
            str3 = txttname.Text.Trim()
            str4 = txtbarcode.Text.Trim()
            If txthidoption.Text = "Items" Then
                If txtpoptno.Text.Trim() <> "" Then
                    objEN.ItemCode = txtpopunique.Text.Trim()
                    objEN.Itemdesc = txtpoptno.Text.Trim()
                    txtItemcode.Text = objEN.ItemCode
                    txtitmdesc.Text = objEN.Itemdesc
                    txtIbarcode.Text = txttname.Text.Trim()
                    dbcon.dss = objBL.BindUom(objEN)
                    If dbcon.dss.Tables(0).Rows.Count > 0 Then
                        ddlUom.DataTextField = "UomCode"
                        ddlUom.DataValueField = "UomEntry"
                        ddlUom.DataSource = dbcon.dss.Tables(0)
                        ddlUom.DataBind()
                        ddlUom.SelectedItem.Value = dbcon.dss.Tables(0).Rows(0)("UomEntry").ToString()
                    End If
                    ModalPopupExtender6.Show()
                End If
            End If
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            dbcon.strmsg = "alert('" & ex.Message & "')"
            mess(dbcon.strmsg)
        End Try
    End Sub

    Private Sub btnnew_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnnew.Click
        System.Threading.Thread.Sleep(2000)
        Try
            lblNExist.Text = "N"
            btnRecSubmit.Visible = False
            NewExpense.Visible = True
            btnSubmit.Visible = True
            panelview.Visible = False
            PanelNewRequest.Visible = True
            btnAdd.Visible = True
            objEN.EmpId = lblempNo.Text.Trim()
            lbldocno.Text = dbcon.Getmaxcode("[@Z_OPRQ]", "DocEntry")
            lblsubdt.Text = Date.Now.ToShortDateString()
            lblempname.Text = Session("UserName").ToString()
            lblempNo.Text = Session("UserCode").ToString()
            dbcon.strmsg = objBL.DeleteTempTable(objEN)
            ddlNewStatus.SelectedItem.Text = "Draft"
            ddlDocStatus.SelectedItem.Text = "Draft"
            ddlDocStatus.Visible = False
            btnWithDraw.Visible = False
            ddlNewStatus.Visible = True
            grdPRequestLines.DataBind()
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            dbcon.strmsg = "alert('" & ex.Message & "')"
            mess(dbcon.strmsg)
        End Try
    End Sub

    Private Sub grdItems_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdItems.RowDataBound
        txtpoptno.Text = ""
        txtpopunique.Text = ""
        txthidoption.Text = ""
        txtbarcode.Text = ""
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("onclick", "popupdisplay('Items','" + (DataBinder.Eval(e.Row.DataItem, "ItemCode")).ToString().Trim() + "','" + (DataBinder.Eval(e.Row.DataItem, "ItemName")).ToString().Trim().Replace("'", "") + "','" + (DataBinder.Eval(e.Row.DataItem, "CodeBars")).ToString().Trim() + "');")
        End If
    End Sub
    Private Sub mess(ByVal str As String)
        ErrHandler.WriteError(dbcon.strmsg)
        ScriptManager.RegisterStartupScript(Update, Update.[GetType](), "strmsg", dbcon.strmsg, True)
    End Sub
    Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        'System.Threading.Thread.Sleep(2000)
        Try
            If txtorderqty.Text.Trim <> "" Then
                dblQty = CDbl(txtorderqty.Text.Trim())
            Else
                dblQty = 0
            End If
            If txtItemcode.Text.Trim = "" Then
                dbcon.strmsg = "alert('Enter the itemcode.....')"
                mess(dbcon.strmsg)
                ModalPopupExtender6.Show()
            ElseIf dblQty <= 0 Then
                dbcon.strmsg = "alert('Order quantity is missing...')" ' "Order quantity is missing"
                mess(dbcon.strmsg)
                ModalPopupExtender6.Show()
            Else
                objEN.EmpId = lblempNo.Text.Trim()
                objEN.EmpName = lblempname.Text.Trim()
                objEN.DeptCode = lbldept.Text.Trim()
                objEN.DeptName = lbldeptName.Text.Trim()
                objEN.DocNo = lbldocno.Text.Trim()
                'objEN.DocStatus = ddlDocStatus.SelectedValue
                objEN.DocStatus = ddlNewStatus.SelectedValue
                objEN.Defwhs = ddldestination.SelectedValue
                objEN.OrdrPatient = ddlPatients.SelectedValue
                objEN.ItemCode = txtItemcode.Text.Trim().Replace("'", "")
                objEN.Itemdesc = txtitmdesc.Text.Trim().Replace("'", "")
                objEN.ItemSpec = txtitemspec.Text.Trim()
                objEN.OrdrUom = ddlUom.SelectedValue.Trim()
                objEN.OrdrQty = txtorderqty.Text.Trim()
                objEN.OrdrUomDesc = ddlUom.SelectedItem.Text.Trim()
                objEN.Barcode = txtIbarcode.Text.Trim()
                objEN.SessionId = Session("SessionId").ToString()

                dbcon.strmsg = objBL.InsertLines(objEN)
                If dbcon.strmsg = "Success" Then
                    ModalPopupExtender6.Hide()
                    LoadDatasource(objEN)
                    Clear()
                Else
                    mess(dbcon.strmsg)
                    ModalPopupExtender6.Show()
                End If
            End If
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            dbcon.strmsg = "alert('" & ex.Message & "')"
            mess(dbcon.strmsg)
        End Try
    End Sub
    Private Sub Clear()
        txtIbarcode.Text = ""
        txtItemcode.Text = ""
        txtitemspec.Text = ""
        txtitmdesc.Text = ""
        ddlUom.SelectedIndex = 0
        txtorderqty.Text = ""
    End Sub
    Private Sub LoadDatasource(ByVal objEN As EmpPREN)
        Try
            dbcon.dss1 = objBL.TempLines(objEN)
            If dbcon.Dss1.Tables(0).Rows.Count > 0 Then
                grdPRequestLines.DataSource = dbcon.dss1.Tables(0)
                grdPRequestLines.DataBind()
            Else
                grdPRequestLines.DataBind()
            End If
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            dbcon.strmsg = "alert('" & ex.Message & "')"
            mess(dbcon.strmsg)
        End Try
    End Sub
  
    Private Function SaveUpdateClaim(ByVal objEN As EmpPREN) As String
        Try
            Dim ApprovalStatus, DocType, DocStatus As String
            Dim LineStatus As String = ""
            If ddlNewStatus.SelectedValue = "S" Then
                DocType = "PRD"
                ApprovalStatus = dbcon.DocApproval("PRD", lbldept.Text.Trim())
                DocStatus = ddlNewStatus.SelectedValue
                If ApprovalStatus = "A" Then
                    ApprovalStatus = dbcon.DocApproval("PR", lbldept.Text.Trim())
                    DocType = "PR"
                    If ApprovalStatus = "A" Then
                        LineStatus = "D"
                        DocStatus = "I"
                    End If
                Else
                    ApprovalStatus = "P"
                    LineStatus = "O"
                    DocStatus = DocStatus
                End If
            Else
                ApprovalStatus = "P"
                LineStatus = "O"
                DocStatus = "D"
            End If
          
            Dim oRecSet, oTemp As SAPbobsCOM.Recordset
            dbcon.objMainCompany = Session("SAPCompany")
            oRecSet = dbcon.objMainCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oTemp = dbcon.objMainCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            Dim oGeneralService As SAPbobsCOM.GeneralService
            Dim oGeneralData1 As SAPbobsCOM.GeneralData
            Dim oGeneralParams As SAPbobsCOM.GeneralDataParams
            Dim oCompanyService As SAPbobsCOM.CompanyService
            Dim oChildren1 As SAPbobsCOM.GeneralDataCollection
            oCompanyService = dbcon.objMainCompany.GetCompanyService()
            Dim oChild As SAPbobsCOM.GeneralData
            oGeneralService = oCompanyService.GetGeneralService("Z_OPRQ")
            oGeneralData1 = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData)
            oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
            If objEN.ItemSpec = "N" Then
                oGeneralData1.SetProperty("U_Z_EmpID", lblempNo.Text.Trim())
                oGeneralData1.SetProperty("U_Z_EmpName", lblempname.Text.Trim())
                oGeneralData1.SetProperty("U_Z_DeptCode", lbldept.Text.Trim())
                oGeneralData1.SetProperty("U_Z_DeptName", lbldeptName.Text.Trim())
                oGeneralData1.SetProperty("U_Z_DocDate", Now.Date)
                oGeneralData1.SetProperty("U_Z_DocStatus", DocStatus)
                oGeneralData1.SetProperty("U_Z_Destination", ddldestination.SelectedValue)
                oGeneralData1.SetProperty("U_Z_Priority", ddlPriority.SelectedValue)

                oChildren1 = oGeneralData1.Child("Z_PRQ1")

                dbcon.strQuery = "Select * from ""U_OPRQ""   where ""SessionId""='" & objEN.SessionId & "' AND ""EmpId""='" & objEN.EmpId & "'"
                oRecSet.DoQuery(dbcon.strQuery)
                For inlloop As Integer = 0 To oRecSet.RecordCount - 1
                    If ApprovalStatus = "P" Then
                        LineStatus = oRecSet.Fields.Item("LineStatus").Value
                    End If
                    oChild = oChildren1.Add()
                    oChild.SetProperty("U_Z_ItemCode", oRecSet.Fields.Item("ItemCode").Value)
                    oChild.SetProperty("U_Z_ItemName", oRecSet.Fields.Item("ItemName").Value)
                    oChild.SetProperty("U_Z_OrdQty", oRecSet.Fields.Item("OrderQty").Value)
                    oChild.SetProperty("U_Z_OrdUom", oRecSet.Fields.Item("OrderUom").Value)
                    oChild.SetProperty("U_Z_OrdUomDesc", oRecSet.Fields.Item("OrderUomDesc").Value)
                    oChild.SetProperty("U_Z_AltItemCode", oRecSet.Fields.Item("AltItemCode").Value)
                    oChild.SetProperty("U_Z_AltItemName", oRecSet.Fields.Item("AltItemDesc").Value)
                    oChild.SetProperty("U_Z_DeliQty", oRecSet.Fields.Item("DelQty").Value)
                    oChild.SetProperty("U_Z_DelUom", oRecSet.Fields.Item("DelUom").Value)
                    oChild.SetProperty("U_Z_DelUomDesc", oRecSet.Fields.Item("DelUomDesc").Value)
                    oChild.SetProperty("U_Z_RecQty", oRecSet.Fields.Item("ReceivedQty").Value)
                    oChild.SetProperty("U_Z_RecUom", oRecSet.Fields.Item("ReceivedUom").Value)
                    oChild.SetProperty("U_Z_RecUomDesc", oRecSet.Fields.Item("ReceivedUomDesc").Value)
                    oChild.SetProperty("U_Z_BarCode", oRecSet.Fields.Item("Barcode").Value)
                    oChild.SetProperty("U_Z_OrdPatient", oRecSet.Fields.Item("OrdPatient").Value)
                    oChild.SetProperty("U_Z_LineStatus", LineStatus)
                    oChild.SetProperty("U_Z_AppStatus", ApprovalStatus) ' dbcon.DocApproval("PR", lbldept.Text.Trim()))
                    oRecSet.MoveNext()
                Next
                oGeneralParams = oGeneralService.Add(oGeneralData1)
            Else
                oGeneralParams.SetProperty("DocEntry", lbldocno.Text.Trim())
                oGeneralData1 = oGeneralService.GetByParams(oGeneralParams)
                oGeneralData1.SetProperty("U_Z_DocStatus", DocStatus)
                oGeneralData1.SetProperty("U_Z_Destination", ddldestination.SelectedValue)
                oGeneralData1.SetProperty("U_Z_Priority", ddlPriority.SelectedValue)

                oChildren1 = oGeneralData1.Child("Z_PRQ1")

                dbcon.strQuery = "Select * from ""U_OPRQ""   where ""SessionId""='" & objEN.SessionId & "' AND ""EmpId""='" & objEN.EmpId & "' and isnull(NewDocStatus,'N')='N'"
                oRecSet.DoQuery(dbcon.strQuery)
                For inlloop As Integer = 0 To oRecSet.RecordCount - 1
                    oChild = oChildren1.Add()
                    If ApprovalStatus = "P" Then
                        LineStatus = oRecSet.Fields.Item("LineStatus").Value
                    End If
                    oChild.SetProperty("U_Z_ItemCode", oRecSet.Fields.Item("ItemCode").Value)
                    oChild.SetProperty("U_Z_ItemName", oRecSet.Fields.Item("ItemName").Value)
                    oChild.SetProperty("U_Z_OrdQty", oRecSet.Fields.Item("OrderQty").Value)
                    oChild.SetProperty("U_Z_OrdUom", oRecSet.Fields.Item("OrderUom").Value)
                    oChild.SetProperty("U_Z_OrdUomDesc", oRecSet.Fields.Item("OrderUomDesc").Value)
                    oChild.SetProperty("U_Z_AltItemCode", oRecSet.Fields.Item("AltItemCode").Value)
                    oChild.SetProperty("U_Z_AltItemName", oRecSet.Fields.Item("AltItemDesc").Value)
                    oChild.SetProperty("U_Z_DeliQty", oRecSet.Fields.Item("DelQty").Value)
                    oChild.SetProperty("U_Z_DelUom", oRecSet.Fields.Item("DelUom").Value)
                    oChild.SetProperty("U_Z_DelUomDesc", oRecSet.Fields.Item("DelUomDesc").Value)
                    oChild.SetProperty("U_Z_RecQty", oRecSet.Fields.Item("ReceivedQty").Value)
                    oChild.SetProperty("U_Z_RecUom", oRecSet.Fields.Item("ReceivedUom").Value)
                    oChild.SetProperty("U_Z_RecUomDesc", oRecSet.Fields.Item("ReceivedUomDesc").Value)
                    oChild.SetProperty("U_Z_BarCode", oRecSet.Fields.Item("Barcode").Value)
                    oChild.SetProperty("U_Z_OrdPatient", oRecSet.Fields.Item("OrdPatient").Value)
                    oChild.SetProperty("U_Z_LineStatus", LineStatus)
                    oChild.SetProperty("U_Z_AppStatus", ApprovalStatus) ' dbcon.DocApproval("PR", lbldept.Text.Trim()))
                    oRecSet.MoveNext()
                Next
                oGeneralService.Update(oGeneralData1)
            End If
            Dim strDocEntry As String = oGeneralParams.GetProperty("DocEntry")
            dbcon.strQuery = "Delete from  ""@Z_PRQ1""  where ""DocEntry""='" & strDocEntry & "' and ""U_Z_ItemCode"" Like '%D'"
            oRecSet.DoQuery(dbcon.strQuery)
            If ddlNewStatus.SelectedValue = "S" Then
                If ApprovalStatus = "P" Then
                    intTempID = dbcon.GetTemplateID(DocType, lbldept.Text.Trim())
                    If intTempID <> "0" Then
                        dbcon.UpdateApprovalRequired("@Z_PRQ1", "DocEntry", strDocEntry, "Y", intTempID)
                    Else
                        dbcon.UpdateApprovalRequired("@Z_PRQ1", "DocEntry", strDocEntry, "N", intTempID)
                    End If
                    If strDocEntry <> "" Then
                        dbcon.InitialMessage("Purchase Requisition", strDocEntry, ApprovalStatus, intTempID, lblempname.Text.Trim(), DocType, dbcon.objMainCompany, strDocEntry)
                    End If
                Else
                    objEN.DocNo = strDocEntry
                    objEN.SAPCompany = Session("SAPCompany")
                    '  dbcon.strmsg = CreateGoodsIssue(objEN)
                End If
               
            End If
            dbcon.strmsg = "Success"
            Return dbcon.strmsg
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "js", "<script>alert('" & ex.Message & "')</script>")
        End Try
    End Function
    Private Function CreateGoodsIssue(ByVal objEN As EmpPREN) As String
        Dim blnLineExists As Boolean
        Dim oItem As SAPbobsCOM.Items
        Dim strMailCode As String = ""
        Dim dblQuantity, dblBatchRequiredQty As Double
        Dim oDocument As SAPbobsCOM.Documents
        Dim oRecordSet, oTemp As SAPbobsCOM.Recordset
        Dim strQuery As String
        oRecordSet = objEN.SapCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
        oTemp = objEN.SapCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
        oDocument = objEN.SAPCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenExit)
        oItem = objEN.SAPCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)
        Try
            dbcon.strQuery = "select * from [@Z_PRQ1]  Where DocENtry = '" + objEN.DocNo + "' and isnull(U_Z_DocNo,'')='' and U_Z_AppStatus='A' and U_Z_GoodIssue='N'"
            oRecordSet.DoQuery(dbcon.strQuery)
            blnLineExists = False
            If oRecordSet.RecordCount > 0 Then
                For intLoop As Integer = 0 To oRecordSet.RecordCount - 1
                 
                    If 1 = 1 Then
                        oDocument.DocDate = Now.Date
                        oDocument.Comments = oRecordSet.Fields.Item("U_Z_RejRemark").Value
                        If intLoop > 0 Then
                            oDocument.Lines.Add()
                        End If
                        dblQuantity = oRecordSet.Fields.Item("U_Z_RecQty").Value
                        If dblQuantity < 0 Then
                            dblQuantity = dblQuantity * -1
                        End If
                        oDocument.Lines.SetCurrentLine(intLoop)
                        oDocument.Lines.Quantity = dblQuantity
                        oDocument.Lines.WarehouseCode = ddldestination.SelectedValue
                        If oRecordSet.Fields.Item("U_Z_RecUom").Value <> -1 Then
                            oDocument.Lines.UseBaseUnits = SAPbobsCOM.BoYesNoEnum.tNO
                            oDocument.Lines.UoMEntry = oRecordSet.Fields.Item("U_Z_RecUom").Value
                        End If
                        oDocument.Lines.ItemCode = oRecordSet.Fields.Item("U_Z_AltItemCode").Value

                        If oItem.GetByKey(oDocument.Lines.ItemCode) Then
                            If oItem.ManageBatchNumbers = SAPbobsCOM.BoYesNoEnum.tYES Then
                                Dim inTbatchLine As Integer = 0

                                Dim batchquantity As Double
                                Dim dblAssignqty As Double = 0
                                strQuery = "select itemcode, ExpDate as exp_date, BatchNum,Quantity, WhsCode from oibt where Quantity <> 0 " & _
                                     " and ItemCode = '" & oDocument.Lines.ItemCode & "' And WhsCode = '" & oDocument.Lines.WarehouseCode & "' order by exp_date "
                                oTemp.DoQuery(strQuery)
                                'Dim w As Integer
                                For intBatch As Integer = 0 To oTemp.RecordCount - 1
                                    While (dblQuantity > 0 And Not oTemp.EoF)
                                        batchquantity = oTemp.Fields.Item("Quantity").Value
                                        If batchquantity >= dblQuantity Then
                                            dblAssignqty = dblQuantity
                                        Else
                                            dblAssignqty = batchquantity
                                        End If

                                        If inTbatchLine > 0 Then
                                            oDocument.Lines.BatchNumbers.Add()
                                        End If
                                        oDocument.Lines.BatchNumbers.SetCurrentLine(inTbatchLine)
                                        oDocument.Lines.BatchNumbers.BatchNumber = oTemp.Fields.Item("BatchNum").Value
                                        oDocument.Lines.BatchNumbers.Quantity = dblAssignqty
                                        inTbatchLine = inTbatchLine + 1
                                        dblQuantity = dblQuantity - dblAssignqty
                                        oTemp.MoveNext()
                                    End While
                                Next
                            End If
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
                        dbcon.strmsg = objEN.SAPCompany.GetLastErrorDescription
                        ErrHandler.WriteError(dbcon.strmsg)
                    Else
                        Dim strdocCode As String
                        objEN.SapCompany.GetNewObjectCode(strdocCode)
                        If oDocument.GetByKey(strdocCode) Then
                            oTemp.DoQuery("Update [@Z_PRQ1] set U_Z_DocNo='" & oDocument.DocNum & "',U_Z_GoodIssue='Y' where DocEntry='" & objEN.DocNo & "' and LineId in (" & strMailCode & ")")
                        End If
                    End If
                End If
            End If
            dbcon.strmsg = "Success"
        Catch ex As Exception
            dbcon.strmsg = ex.Message
            ErrHandler.WriteError(dbcon.strmsg)
        End Try
        Return dbcon.strmsg
    End Function
    Private Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Try
            If Session("UserCode") Is Nothing Or Session("SAPCompany") Is Nothing Then
                Response.Redirect("Login.aspx?sessionExpired=true", True)
            Else
                objEN.EmpId = Session("UserCode").ToString()
                objEN.SessionId = Session("SessionId").ToString()
                objEN.ItemSpec = lblNExist.Text.Trim()
                dbcon.strmsg = SaveUpdateClaim(objEN)
                If dbcon.strmsg = "Success" Then
                    dbcon.strmsg = "Document created Successfully"
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "js", "<script>alert('" & dbcon.strmsg & "')</script>")
                    panelview.Visible = True
                    PanelNewRequest.Visible = False
                    objEN.SessionId = Session("SessionId").ToString()
                    objEN.EmpId = Session("UserCode").ToString()
                    dbcon.strmsg = objBL.DeleteTempTable(objEN)
                    PageLoadBind(objEN)
                Else
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "js", "<script>alert('" & dbcon.strmsg & "')</script>")
                    panelview.Visible = False
                    PanelNewRequest.Visible = True
                End If
            End If
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "js", "<script>alert('" & ex.Message & "')</script>")
        End Try
    End Sub
    Protected Sub lbtndocnum_Click(ByVal sender As Object, ByVal e As EventArgs)
        System.Threading.Thread.Sleep(2000)
        Try
            If Session("UserCode") Is Nothing Or Session("SAPCompany") Is Nothing Then
                dbcon.strmsg = "Your session is Expired..."
                ClientScript.RegisterStartupScript(Me.GetType(), "msg", "<script>alert('" & dbcon.strmsg & "')</script>")
                Response.Redirect("Login.aspx?sessionExpired=true", True)
            Else
                Dim link As LinkButton = CType(sender, LinkButton)
                Dim gv As GridViewRow = CType((link.Parent.Parent), GridViewRow)
                Dim DocNo As LinkButton = CType(gv.FindControl("lbtndocnum"), LinkButton)
                Dim introw As Integer = gv.RowIndex
                For Each row1 As GridViewRow In grdPRequestLines.Rows
                    If row1.RowIndex <> introw Then
                        row1.BackColor = Color.White
                    Else
                        row1.BackColor = Color.LimeGreen
                    End If
                Next
                objEN.SessionId = Session("SessionId").ToString()
                objEN.EmpId = Session("UserCode").ToString()
                objEN.DocNo = DocNo.Text.Trim()
                dbcon.strmsg = objBL.PopulateExistingDocument(objEN)
                If dbcon.strmsg = "Success" Then
                Else
                    mess(dbcon.strmsg)
                End If
                lblNExist.Text = "E"
                BindPRequestApproval(objEN)
            End If
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            dbcon.strmsg = "" & ex.Message & ""
            ClientScript.RegisterStartupScript(Me.GetType(), "msg", "<script>alert('" & dbcon.strmsg & "')</script>")
        End Try
    End Sub
    Private Sub BindPRequestApproval(ByVal objEN As EmpPREN)
        Try
            dbcon.Ds1 = objBL.BindPRequestApproval(objEN)
            If dbcon.Ds1.Tables(1).Rows.Count > 0 Then
                lbldocno.Text = dbcon.Ds1.Tables(1).Rows(0)("DocEntry").ToString()
                lblsubdt.Text = dbcon.Ds1.Tables(1).Rows(0)("U_Z_DocDate").ToString()
                lblempNo.Text = dbcon.Ds1.Tables(1).Rows(0)("U_Z_EmpID").ToString()
                lblempname.Text = dbcon.Ds1.Tables(1).Rows(0)("U_Z_EmpName").ToString()
                lbldept.Text = dbcon.Ds1.Tables(1).Rows(0)("U_Z_DeptCode").ToString()
                lbldeptName.Text = dbcon.Ds1.Tables(1).Rows(0)("U_Z_DeptName").ToString()
                ddldestination.SelectedValue = dbcon.Ds1.Tables(1).Rows(0)("U_Z_Destination").ToString()
                ddlPriority.SelectedValue = dbcon.Ds1.Tables(1).Rows(0)("U_Z_Priority").ToString()
                ddlDocStatus.SelectedItem.Text = dbcon.Ds1.Tables(1).Rows(0)("U_Z_DocStatus").ToString()
                If ddlDocStatus.SelectedItem.Text = "Draft" Or ddlDocStatus.SelectedItem.Text = "Confirm" Then
                    ddlNewStatus.SelectedItem.Text = dbcon.Ds1.Tables(1).Rows(0)("U_Z_DocStatus").ToString()
                ElseIf ddlDocStatus.SelectedItem.Text <> "Draft" Then
                    ddlNewStatus.SelectedItem.Text = "Confirm"
                End If
                If ddlDocStatus.SelectedItem.Text = "Draft" Then
                    NewExpense.Visible = True
                    btnRecSubmit.Visible = False
                    btnSubmit.Visible = True
                    ddlNewStatus.Visible = True
                    ddlDocStatus.Visible = False
                    btnWithDraw.Visible = True
                ElseIf ddlDocStatus.SelectedItem.Text = "Confirm" Then
                    NewExpense.Visible = False
                    btnRecSubmit.Visible = False
                    btnSubmit.Visible = False
                    ddlNewStatus.Visible = False
                    ddlDocStatus.Visible = True
                    btnWithDraw.Visible = True
                ElseIf (ddlDocStatus.SelectedItem.Text = "Closed" Or ddlDocStatus.SelectedItem.Text = "Open" Or ddlDocStatus.SelectedItem.Text = "DLC InProgress" Or ddlDocStatus.SelectedItem.Text = "Cancelled") Then
                    NewExpense.Visible = False
                    btnRecSubmit.Visible = False
                    btnSubmit.Visible = False
                    ddlNewStatus.Visible = False
                    ddlDocStatus.Visible = True
                    btnWithDraw.Visible = False
                    'ElseIf ddlDocStatus.SelectedItem.Text = "InProgress" Then
                    '    NewExpense.Visible = False
                    '    btnRecSubmit.Visible = False
                    '    btnSubmit.Visible = False
                    '    ddlNewStatus.Visible = False
                    '    ddlDocStatus.Visible = True
                    '    btnWithDraw.Visible = False
                Else
                    NewExpense.Visible = False
                    btnRecSubmit.Visible = True
                    btnSubmit.Visible = False
                    ddlNewStatus.Visible = False
                    ddlDocStatus.Visible = True
                    btnWithDraw.Visible = False
                End If
            End If
            If dbcon.Ds1.Tables(0).Rows.Count > 0 Then
                grdPRequestLines.DataSource = dbcon.Ds1.Tables(0)
                grdPRequestLines.DataBind()
            Else
                grdPRequestLines.DataBind()
            End If
           
            panelview.Visible = False
            PanelNewRequest.Visible = True
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            dbcon.strmsg = "" & ex.Message & ""
            ClientScript.RegisterStartupScript(Me.GetType(), "msg", "<script>alert('" & dbcon.strmsg & "')</script>")
        End Try
    End Sub

    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        objEN.SessionId = Session("SessionId").ToString()
        objEN.EmpId = Session("UserCode").ToString()
        dbcon.strmsg = objBL.DeleteTempTable(objEN)
        PageLoadBind(objEN)
        Clear()
        panelview.Visible = True
        PanelNewRequest.Visible = False
    End Sub

    Private Sub grdPRequestLines_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdPRequestLines.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim ddlUoM As DropDownList = CType(e.Row.FindControl("ddlEReceUoM"), DropDownList)
            Dim lblitemcode As Label = CType(e.Row.FindControl("lblAItemCode"), Label)
            dbcon.strQuery = "SELECT  T2.[UomEntry] as UomEntry, T3.[UomCode] as UomCode FROM OITM T0  "
            dbcon.strQuery += " INNER JOIN OUGP T1 ON T0.UgpEntry = T1.UgpEntry INNER JOIN UGP1 T2 ON T1.UgpEntry = T2.UgpEntry "
            dbcon.strQuery += " INNER JOIN OUOM T3 ON T3.UoMEntry = T2.UomEntry WHERE T0.[ItemCode]='" & lblitemcode.Text.Trim() & "'"
            ddlUoM.DataSource = dbcon.GetData(dbcon.strQuery)
            ddlUoM.DataTextField = "UomCode"
            ddlUoM.DataValueField = "UomEntry"
            ddlUoM.DataBind()

            ''Select the Country of Customer in DropDownList
            'Dim lblUoM As String = CType(e.Row.FindControl("lblEReceiveUom"), Label).Text
            'ddlUoM.Items.FindByValue(lblUoM).Selected = True
            Dim lblStatus As Label = CType(e.Row.FindControl("lblEAppstatus"), Label)
            Dim lblLineStatus As Label = CType(e.Row.FindControl("lblELinestatus"), Label)
            Dim imgbtn As ImageButton = CType(e.Row.FindControl("imgbtn"), ImageButton)
            If lblStatus.Text.Trim() = "A" Then
                e.Row.Cells(0).Enabled = False
                e.Row.Cells(16).Enabled = True
                e.Row.Cells(17).Enabled = True
                imgbtn.Visible = True
                btnRecSubmit.Visible = True
            ElseIf lblStatus.Text.Trim() = "R" Or (lblStatus.Text.Trim() = "P" And (lblLineStatus.Text.Trim() = "D" Or lblLineStatus.Text.Trim() = "C")) Then
                e.Row.Cells(0).Enabled = False
                e.Row.Cells(16).Enabled = False
                e.Row.Cells(17).Enabled = False
                imgbtn.Visible = True
            ElseIf lblStatus.Text.Trim() = "P" And ddlDocStatus.SelectedItem.Text = "Draft" Then
                e.Row.Cells(0).Enabled = True
                e.Row.Cells(16).Enabled = False
                e.Row.Cells(17).Enabled = False
                imgbtn.Visible = False
            ElseIf (ddlNewStatus.SelectedItem.Text = "Confirm" And lblStatus.Text.Trim() = "P") Or lblStatus.Text.Trim() = "P" Then
                e.Row.Cells(0).Enabled = False
                e.Row.Cells(16).Enabled = False
                e.Row.Cells(17).Enabled = False
                imgbtn.Visible = False
          
            End If

            If (ddlDocStatus.SelectedItem.Text.Trim() = "Draft" Or ddlDocStatus.SelectedItem.Text.Trim() = "Confirm" Or ddlDocStatus.SelectedItem.Text.Trim() = "Open" Or ddlDocStatus.SelectedItem.Text.Trim() = "Cancelled") Then
                grdPRequestLines.Columns(16).Visible = False
                grdPRequestLines.Columns(17).Visible = False
            Else
                grdPRequestLines.Columns(16).Visible = True
                grdPRequestLines.Columns(17).Visible = True
            End If
            If ddlDocStatus.SelectedItem.Text.Trim() = "Closed" Then
                btnRecSubmit.Visible = False
                btnSubmit.Visible = False
            End If
        End If

    End Sub

    Private Sub grdPRequestLines_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles grdPRequestLines.RowDeleting
        Try
            objEN.DocNo = DirectCast(grdPRequestLines.Rows(e.RowIndex).FindControl("lblECode"), Label).Text
            objEN.LineId = DirectCast(grdPRequestLines.Rows(e.RowIndex).FindControl("lblELineCode"), Label).Text
            objEN.Code = lbldocno.Text.Trim() 'DirectCast(grdPRequestLines.Rows(e.RowIndex).FindControl("lblERefCode"), Label).Text
            objEN.EmpId = lblempNo.Text.Trim()
            dbcon.strmsg = objBL.DeleteLinesTable(objEN)
            If dbcon.strmsg = "Success" Then
                dbcon.strmsg = "alert('Purchase Requisition Deleted successfully...')"
                mess(dbcon.strmsg)
                lblerror.Text = ""
            Else
                mess(dbcon.strmsg)
            End If
            objEN.SessionId = Session("SessionId").ToString()
            objEN.EmpId = Session("UserCode").ToString()
            objEN.DocNo = lbldocno.Text.Trim()
            BindPRequestApproval(objEN)
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            dbcon.strmsg = "alert('" & ex.Message & "')"
            mess(dbcon.strmsg)
        End Try
    End Sub

    Private Sub btnRecSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRecSubmit.Click
        Dim blnFlag As Boolean = False
        Dim blnFlag1 As Boolean = True
        Try
            For Each row1 In grdPRequestLines.Rows
                objEN.SAPCompany = Session("SAPCompany")
                objEN.EmpId = lblempNo.Text.Trim()
                objEN.DocStatus = CType(row1.FindControl("lblELinestatus"), Label).Text
                objEN.ItemCode = CType(row1.FindControl("lblAItemCode"), Label).Text
                objEN.DocNo = CType(row1.FindControl("lblERefCode"), Label).Text
                objEN.Code = CType(row1.FindControl("lblELineCode"), Label).Text
                objEN.DelQty = CType(row1.FindControl("lblEDelQty"), Label).Text
                objEN.OrdrQty = CType(row1.FindControl("txtRecqty"), TextBox).Text
                objEN.OrdrUom = CType(row1.FindControl("ddlEReceUoM"), DropDownList).SelectedValue
                objEN.OrdrUomDesc = CType(row1.FindControl("ddlEReceUoM"), DropDownList).SelectedItem.Text
                If objEN.DocStatus = "C" Or objEN.DocStatus = "D" Then
                    blnFlag = True
                    dbcon.strmsg = objBL.UpdateReceived(objEN)
                    If dbcon.strmsg = "Success" Then
                    Else
                        dbcon.strmsg = "alert('" & dbcon.strmsg & "')"
                        mess(dbcon.strmsg)
                    End If
                ElseIf objEN.DocStatus = "L" Then
                    blnFlag = True
                Else
                    blnFlag1 = False
                End If
            Next
            dbcon.strmsg = CreateGoodsIssue(objEN)
            If blnFlag = blnFlag1 Then
                objEN.DocStatus = "C"
                objEN.EmpId = lblempNo.Text.Trim()
                objEN.Code = lbldocno.Text.Trim()
                dbcon.strmsg = objBL.UpdateStatus(objEN)
                If dbcon.strmsg = "Success" Then
                Else
                    dbcon.strmsg = "alert('" & dbcon.strmsg & "')"
                    mess(dbcon.strmsg)
                End If
            End If
            objEN.SessionId = Session("SessionId").ToString()
            objEN.EmpId = Session("UserCode").ToString()
            dbcon.strmsg = objBL.DeleteTempTable(objEN)
            PageLoadBind(objEN)
            panelview.Visible = True
            PanelNewRequest.Visible = False
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            dbcon.strmsg = "alert('" & ex.Message & "')"
            mess(dbcon.strmsg)
        End Try
    End Sub
    Protected Sub imgbtn_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim btndetails As ImageButton = TryCast(sender, ImageButton)
            Dim gvrow As GridViewRow = DirectCast(btndetails.NamingContainer, GridViewRow)
            Dim CallID As Label = CType(gvrow.FindControl("lblERefCode"), Label)
            Dim LineId As Label = CType(gvrow.FindControl("lblELineCode"), Label)
            Dim EmpID As String = lblempNo.Text.Trim()
            objEN.LineId = LineId.Text.Trim()
            objEN.Code = CallID.Text.Trim()
            objEN.EmpId = EmpID.Trim()
            BindHistory(objEN)
            ModalPopupExtender2.Show()
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            dbcon.strmsg = "" & ex.Message & ""
            ClientScript.RegisterStartupScript(Me.GetType(), "msg", "<script>alert('" & dbcon.strmsg & "')</script>")
        End Try
    End Sub
    Private Sub BindHistory(ByVal objEN As EmpPREN)
        Try
            dbcon.Ds2 = objBL.LoadHistory(objEN)
            If dbcon.Ds2.Tables(0).Rows.Count > 0 Then
                grdRequesttohr.DataSource = dbcon.Ds2.Tables(0)
                grdRequesttohr.DataBind()
            Else
                grdRequesttohr.DataBind()
            End If
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            dbcon.strmsg = "'" & ex.Message & "'"
            ClientScript.RegisterStartupScript(Me.GetType(), "msg", "<script>alert('" & dbcon.strmsg & "')</script>")
        End Try
    End Sub

    Private Sub grdPurchaseRequest_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdPurchaseRequest.PageIndexChanging
        grdPurchaseRequest.PageIndex = e.NewPageIndex
        objEN.EmpId = Session("UserCode").ToString()
        PageLoadBind(objEN)
    End Sub

    Private Sub btnWithDraw_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnWithDraw.Click
        Try
          
            objEN.Code = lbldocno.Text.Trim()
            dbcon.strmsg = objBL.CancelRequest(objEN)
            objEN.EmpId = Session("UserCode").ToString()
            objEN.SAPCompany = Session("SAPCompany")
            PageLoadBind(objEN)
            dbcon.strmsg = objBL.DeleteTempTable(objEN)
            panelview.Visible = True
            PanelNewRequest.Visible = False
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            dbcon.strmsg = "alert('" & ex.Message & "')"
            mess(dbcon.strmsg)
        End Try
    End Sub
    <System.Web.Script.Services.ScriptMethod(), _
System.Web.Services.WebMethod()> _
    Public Shared Function GetItems(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Try
            Dim con As SqlConnection = New SqlConnection("data source=" & ConfigurationManager.AppSettings("SAPServer") & ";Integrated Security=SSPI;database=" & ConfigurationManager.AppSettings("CompanyDB") & ";User id=" & ConfigurationManager.AppSettings("DbUserName") & "; password=" & ConfigurationManager.AppSettings("DbPassword"))
            Dim cmd As SqlCommand = New SqlCommand
            cmd.CommandText = "select ItemCode from OITM where  ItemCode like @SearchText + '%'"
            cmd.Parameters.AddWithValue("@SearchText", prefixText)
            cmd.Connection = con
            con.Open()
            Dim customers As List(Of String) = New List(Of String)
            Dim sdr As SqlDataReader = cmd.ExecuteReader
            While sdr.Read
                customers.Add(sdr("ItemCode").ToString)
            End While
            con.Close()
            Return customers
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
        End Try
    End Function

    <System.Web.Script.Services.ScriptMethod(), _
System.Web.Services.WebMethod()> _
    Public Shared Function GetItemsName(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Dim con As SqlConnection = New SqlConnection("data source=" & ConfigurationManager.AppSettings("SAPServer") & ";Integrated Security=SSPI;database=" & ConfigurationManager.AppSettings("CompanyDB") & ";User id=" & ConfigurationManager.AppSettings("DbUserName") & "; password=" & ConfigurationManager.AppSettings("DbPassword"))
        Dim cmd As SqlCommand = New SqlCommand
        cmd.CommandText = "select ItemName from OITM where ItemName like @SearchText + '%'"
        cmd.Parameters.AddWithValue("@SearchText", prefixText)
        cmd.Connection = con
        con.Open()
        Dim customers As List(Of String) = New List(Of String)
        Dim sdr As SqlDataReader = cmd.ExecuteReader
        While sdr.Read
            customers.Add(sdr("ItemName").ToString)
        End While
        con.Close()
        Return customers
    End Function
    Protected Sub lbtpopnview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtpopnview.Click
        objEN.EmpId = Session("UserCode").ToString()
        objEN.SAPCompany = Session("SAPCompany")
        PageLoadBind(objEN)
        ModalPopupExtender7.Show()
    End Sub

    Private Sub btngoItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btngoItem.Click
        objEN.EmpId = Session("UserCode").ToString()
        Try
            dbcon.strQuery = "Select U_AllItemCat  from [@Z_DLC_LOGIN] T0 where U_EMPID=" & objEN.EmpId & ""
            dbcon.sqlda = New SqlDataAdapter(dbcon.strQuery, dbcon.con)
            dbcon.sqlda.Fill(dbcon.Ds1)
            If dbcon.Ds1.Tables(0).Rows.Count > 0 Then
                If dbcon.Ds1.Tables(0).Rows(0)(0).ToString() = "Y" And txtsearchItem.Text.Trim() <> "" Then
                    dbcon.strQuery = "Select ItemCode,ItemName,isnull(CodeBars,'0') as CodeBars  from OITM where isnull(U_CatCode,'')<>'' and ItemCode  like '%" + txtsearchItem.Text.Trim().Replace("'", "''") + "%'"
                    dbcon.GridviewBind(dbcon.strQuery, grdItems)
                ElseIf dbcon.Ds1.Tables(0).Rows(0)(0).ToString() = "Y" And txtsearchItemNa.Text.Trim() <> "" Then
                    dbcon.strQuery = "Select ItemCode,ItemName,isnull(CodeBars,'0') as CodeBars  from OITM where isnull(U_CatCode,'')<>'' and ItemName  like '%" + txtsearchItemNa.Text.Trim().Replace("'", "''") + "%'"
                    dbcon.GridviewBind(dbcon.strQuery, grdItems)
                ElseIf txtsearchItem.Text.Trim() <> "" Then
                    dbcon.strQuery = "select ItemCode,ItemName,isnull(CodeBars,'0') as CodeBars from OITM T0 left Join [@Z_ITCAT1] T1 on T0.U_CatCode=T1.U_CatCode "
                    dbcon.strQuery += "  inner join [@Z_DLC_LOGIN] T2 on T1.DocEntry=T2.DocEntry where T2.U_EMPID = " & objEN.EmpId & " and ItemCode  like '%" + txtsearchItem.Text.Trim().Replace("'", "''") + "%'"
                    dbcon.GridviewBind(dbcon.strQuery, grdItems)
                ElseIf txtsearchItemNa.Text.Trim() <> "" Then
                    dbcon.strQuery = "select ItemCode,ItemName,isnull(CodeBars,'0') as CodeBars from OITM T0 left Join [@Z_ITCAT1] T1 on T0.U_CatCode=T1.U_CatCode "
                    dbcon.strQuery += "  inner join [@Z_DLC_LOGIN] T2 on T1.DocEntry=T2.DocEntry where T2.U_EMPID = " & objEN.EmpId & " and ItemCode  like '%" + txtsearchItemNa.Text.Trim().Replace("'", "''") + "%'"
                    dbcon.GridviewBind(dbcon.strQuery, grdItems)
                ElseIf dbcon.Ds1.Tables(0).Rows(0)(0).ToString() = "Y" Then
                    dbcon.strQuery = "Select ItemCode,ItemName,isnull(CodeBars,'0') as CodeBars  from OITM where isnull(U_CatCode,'')<>''"
                    dbcon.GridviewBind(dbcon.strQuery, grdItems)
                Else
                    dbcon.strQuery = "select ItemCode,ItemName,isnull(CodeBars,'0') as CodeBars from OITM T0 left Join [@Z_ITCAT1] T1 on T0.U_CatCode=T1.U_CatCode "
                    dbcon.strQuery += "  inner join [@Z_DLC_LOGIN] T2 on T1.DocEntry=T2.DocEntry where T2.U_EMPID = " & objEN.EmpId & ""
                    dbcon.GridviewBind(dbcon.strQuery, grdItems)
                End If
            End If
            ModalPopupExtender7.Show()
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
        End Try
    End Sub
End Class