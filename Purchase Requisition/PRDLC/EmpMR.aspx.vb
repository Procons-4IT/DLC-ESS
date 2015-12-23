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

Public Class EmpMR
    Inherits System.Web.UI.Page
    Dim dbcon As DBConnectionDA = New DBConnectionDA()
    Dim objBL As EmpMRBL = New EmpMRBL()
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
            NewExpense.Visible = True
            btnSubmit.Visible = True
            panelview.Visible = False
            PanelNewRequest.Visible = True
            btnAdd.Visible = True
            objEN.EmpId = lblempNo.Text.Trim()
            lbldocno.Text = dbcon.Getmaxcode("[@Z_ORPD]", "DocEntry")
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
                dbcon.strmsg = "alert('Enter the itemcode ...')"
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
                objEN.DocStatus = ddlNewStatus.SelectedValue
                'objEN.DocStatus = ddlDocStatus.SelectedValue
                objEN.Defwhs = ddldestination.SelectedValue
                objEN.ItemCode = txtItemcode.Text.Trim().Replace("'", "")
                objEN.Itemdesc = txtitmdesc.Text.Trim().Replace("'", "")
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
        txtitmdesc.Text = ""
        ddlUom.SelectedIndex = 0
        txtorderqty.Text = ""
    End Sub
    Private Sub LoadDatasource(ByVal objEN As EmpPREN)
        Try
            dbcon.dss1 = objBL.TempLines(objEN)
            If dbcon.dss1.Tables(0).Rows.Count > 0 Then
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
    Private Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        System.Threading.Thread.Sleep(2000)
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
            lblNExist.Text = ""
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "js", "<script>alert('" & ex.Message & "')</script>")
        End Try
    End Sub
    Private Function SaveUpdateClaim(ByVal objEN As EmpPREN) As String
        Try
            Dim ApprovalStatus, DocType, DocStatus As String
            Dim LineStatus As String = ""
            If ddlNewStatus.SelectedValue = "S" Then
                DocType = "MRD"
                ApprovalStatus = dbcon.DocApproval("MRD", lbldept.Text.Trim())
                DocStatus = ddlNewStatus.SelectedValue
                If ApprovalStatus = "A" Then
                    ApprovalStatus = dbcon.DocApproval("MR", lbldept.Text.Trim())
                    DocType = "MR"
                    If ApprovalStatus = "A" Then
                        LineStatus = "D"
                        DocStatus = "C"
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
            oGeneralService = oCompanyService.GetGeneralService("Z_ORPD")
            oGeneralData1 = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData)
            oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
            If objEN.ItemSpec = "N" Then
                oGeneralData1.SetProperty("U_Z_EmpID", lblempNo.Text.Trim())
                oGeneralData1.SetProperty("U_Z_EmpName", lblempname.Text.Trim())
                oGeneralData1.SetProperty("U_Z_DeptCode", lbldept.Text.Trim())
                oGeneralData1.SetProperty("U_Z_DeptName", lbldeptName.Text.Trim())
                oGeneralData1.SetProperty("U_Z_DocDate", lblsubdt.Text.Trim())
                oGeneralData1.SetProperty("U_Z_DocStatus", DocStatus)
                oGeneralData1.SetProperty("U_Z_Destination", ddldestination.SelectedValue)

                oChildren1 = oGeneralData1.Child("Z_RPD1")

                dbcon.strQuery = "Select * from ""U_ORPD""   where ""SessionId""='" & objEN.SessionId & "' AND ""EmpId""='" & objEN.EmpId & "' and isnull(NewDocStatus,'N')='N'"
                oRecSet.DoQuery(dbcon.strQuery)
                For inlloop As Integer = 0 To oRecSet.RecordCount - 1
                    oChild = oChildren1.Add()
                    oChild.SetProperty("U_Z_ItemCode", oRecSet.Fields.Item("ItemCode").Value)
                    oChild.SetProperty("U_Z_ItemName", oRecSet.Fields.Item("ItemName").Value)
                    oChild.SetProperty("U_Z_OrdQty", oRecSet.Fields.Item("OrderQty").Value)
                    oChild.SetProperty("U_Z_OrdUom", oRecSet.Fields.Item("OrderUom").Value)
                    oChild.SetProperty("U_Z_OrdUomDesc", oRecSet.Fields.Item("OrderUomDesc").Value)
                    oChild.SetProperty("U_Z_BarCode", oRecSet.Fields.Item("Barcode").Value)
                    oChild.SetProperty("U_Z_LineStatus", LineStatus)
                    oChild.SetProperty("U_Z_AppStatus", ApprovalStatus)
                    oChild.SetProperty("U_Z_NewDoc", "Y")
                    oRecSet.MoveNext()
                Next
                oGeneralParams = oGeneralService.Add(oGeneralData1)
            Else
                oGeneralParams.SetProperty("DocEntry", lbldocno.Text.Trim())
                oGeneralData1 = oGeneralService.GetByParams(oGeneralParams)
                oGeneralData1.SetProperty("U_Z_Destination", ddldestination.SelectedValue)

                oChildren1 = oGeneralData1.Child("Z_RPD1")

                dbcon.strQuery = "Select * from ""U_ORPD""   where ""SessionId""='" & objEN.SessionId & "' AND ""EmpId""='" & objEN.EmpId & "' and isnull(NewDocStatus,'N')='N'"
                oRecSet.DoQuery(dbcon.strQuery)
                For inlloop As Integer = 0 To oRecSet.RecordCount - 1
                    oChild = oChildren1.Add()
                    oChild.SetProperty("U_Z_ItemCode", oRecSet.Fields.Item("ItemCode").Value)
                    oChild.SetProperty("U_Z_ItemName", oRecSet.Fields.Item("ItemName").Value)
                    oChild.SetProperty("U_Z_OrdQty", oRecSet.Fields.Item("OrderQty").Value)
                    oChild.SetProperty("U_Z_OrdUom", oRecSet.Fields.Item("OrderUom").Value)
                    oChild.SetProperty("U_Z_OrdUomDesc", oRecSet.Fields.Item("OrderUomDesc").Value)
                    oChild.SetProperty("U_Z_BarCode", oRecSet.Fields.Item("Barcode").Value)
                    oChild.SetProperty("U_Z_LineStatus", LineStatus)
                    oChild.SetProperty("U_Z_AppStatus", ApprovalStatus)
                    oChild.SetProperty("U_Z_NewDoc", "Y")
                    oRecSet.MoveNext()
                Next
                oGeneralService.Update(oGeneralData1)
            End If
            Dim strDocEntry As String = oGeneralParams.GetProperty("DocEntry")
            dbcon.strQuery = "Delete from  ""@Z_RPD1""  where ""DocEntry""='" & strDocEntry & "' and ""U_Z_ItemCode"" Like '%D'"
            oRecSet.DoQuery(dbcon.strQuery)
            If ddlNewStatus.SelectedValue = "S" Then
                If ApprovalStatus = "P" Then
                    intTempID = dbcon.GetTemplateID("MRD", lbldept.Text.Trim())
                    If intTempID <> "0" Then
                        dbcon.UpdateApprovalRequired("@Z_RPD1", "DocEntry", strDocEntry, "Y", intTempID)
                    Else
                        dbcon.UpdateApprovalRequired("@Z_RPD1", "DocEntry", strDocEntry, "N", intTempID)
                    End If
                    If strDocEntry <> "" Then
                        dbcon.InitialMessage("Material Return", strDocEntry, ApprovalStatus, intTempID, lblempname.Text.Trim(), DocType, dbcon.objMainCompany, strDocEntry)
                    End If
                Else
                    objEN.DocNo = strDocEntry
                    objEN.SAPCompany = Session("SAPCompany")
                    dbcon.strmsg = CreateGoodsReceipt(objEN)
                End If
            End If

            dbcon.strmsg = "Success"
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "js", "<script>alert('" & ex.Message & "')</script>")
        End Try
        Return dbcon.strmsg
    End Function
    Private Function CreateGoodsReceipt(ByVal objEN As EmpPREN) As String
        Dim blnLineExists As Boolean
        Dim strMailCode As String = ""
        Dim dblQuantity As Double
        Dim oDocument As SAPbobsCOM.Documents
        Dim oRecordSet, oTemp As SAPbobsCOM.Recordset
        oRecordSet = objEN.SAPCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
        oTemp = objEN.SAPCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
        oDocument = objEN.SAPCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenEntry)
        Try
            dbcon.strQuery = "select * from [@Z_RPD1] Where DocEntry = '" + objEN.DocNo + "' and isnull(U_Z_DocRefNo,'')='' and U_Z_AppStatus='A' and U_Z_GoodReceipt='N'"
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
                        dbcon.strmsg = objEN.SAPCompany.GetLastErrorDescription
                        ErrHandler.WriteError(dbcon.strmsg)
                    Else
                        Dim strdocCode As String
                        objEN.SAPCompany.GetNewObjectCode(strdocCode)
                        If oDocument.GetByKey(strdocCode) Then
                            oTemp.DoQuery("Update [@Z_RPD1] set U_Z_DocRefNo='" & oDocument.DocNum & "',U_Z_GoodReceipt='Y' where LineId in (" & strMailCode & ") and DocEntry='" & objEN.DocNo & "' ")
                        End If
                    End If
                End If
            End If
            dbcon.strmsg = "Success"
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            dbcon.strmsg = ex.Message
        End Try
        Return dbcon.strmsg
    End Function
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
                ddlDocStatus.SelectedItem.Text = dbcon.Ds1.Tables(1).Rows(0)("U_Z_DocStatus").ToString()
                If ddlDocStatus.SelectedItem.Text = "Draft" Or ddlDocStatus.SelectedItem.Text = "Confirm" Then
                    ddlNewStatus.SelectedItem.Text = dbcon.Ds1.Tables(1).Rows(0)("U_Z_DocStatus").ToString()
                ElseIf ddlDocStatus.SelectedItem.Text <> "Draft" Then
                    ddlNewStatus.SelectedItem.Text = "Confirm"
                End If
                If ddlDocStatus.SelectedItem.Text = "Draft" Then
                    NewExpense.Visible = True
                    btnSubmit.Visible = True
                    btnWithDraw.Visible = True
                    ddlNewStatus.Visible = True
                    ddlDocStatus.Visible = False
                ElseIf ddlDocStatus.SelectedItem.Text = "Confirm" Then
                    NewExpense.Visible = False
                    btnSubmit.Visible = False
                    btnWithDraw.Visible = True
                    ddlNewStatus.Visible = False
                    ddlDocStatus.Visible = True
                ElseIf ddlDocStatus.SelectedItem.Text = "Closed" Then
                    NewExpense.Visible = False
                    btnSubmit.Visible = False
                    btnWithDraw.Visible = False
                    ddlNewStatus.Visible = False
                    ddlDocStatus.Visible = True
                Else
                    NewExpense.Visible = False
                    btnSubmit.Visible = False
                    btnWithDraw.Visible = False
                    ddlNewStatus.Visible = False
                    ddlDocStatus.Visible = True
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
    Protected Sub imgbtn_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim btndetails As ImageButton = TryCast(sender, ImageButton)
            Dim gvrow As GridViewRow = DirectCast(btndetails.NamingContainer, GridViewRow)
            Dim CallID As Label = CType(gvrow.FindControl("lblERefCode"), Label)
            Dim LineId As Label = CType(gvrow.FindControl("lblEDocCode"), Label)
            Dim EmpID As String = lblempNo.Text.Trim()
            objEN.Code = CallID.Text.Trim()
            objEN.ItemSpec = LineId.Text.Trim()
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

    Private Sub grdPRequestLines_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdPRequestLines.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblStatus As Label = CType(e.Row.FindControl("lblEAppstatus"), Label)
            Dim lblLineStatus As Label = CType(e.Row.FindControl("lblELinestatus"), Label)
            Dim imgbtn As ImageButton = CType(e.Row.FindControl("imgbtn"), ImageButton)
            If lblStatus.Text.Trim() = "A" Then
                e.Row.Cells(0).Enabled = False
                imgbtn.Visible = True
            ElseIf lblStatus.Text.Trim() = "R" Or (lblStatus.Text.Trim() = "P" And (lblLineStatus.Text.Trim() = "D")) Then
                e.Row.Cells(0).Enabled = False
                imgbtn.Visible = True
            ElseIf lblStatus.Text.Trim() = "P" Then
                e.Row.Cells(0).Enabled = True
                imgbtn.Visible = False
            End If
            If ddlNewStatus.SelectedItem.Text = "Draft" Then
                e.Row.Cells(0).Enabled = True
            Else
                e.Row.Cells(0).Enabled = False
            End If
        End If

    End Sub

    Private Sub grdPRequestLines_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles grdPRequestLines.RowDeleting
        Try
            objEN.DocNo = DirectCast(grdPRequestLines.Rows(e.RowIndex).FindControl("lblECode"), Label).Text
            objEN.Code = DirectCast(grdPRequestLines.Rows(e.RowIndex).FindControl("lblERefCode"), Label).Text
            objEN.LineId = DirectCast(grdPRequestLines.Rows(e.RowIndex).FindControl("lblEDocCode"), Label).Text
            objEN.EmpId = lblempNo.Text.Trim()
            dbcon.strmsg = objBL.DeleteLinesTable(objEN)
            If dbcon.strmsg = "Success" Then
                dbcon.strmsg = "alert('Material Return Deleted successfully...')"
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

    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        objEN.SessionId = Session("SessionId").ToString()
        objEN.EmpId = Session("UserCode").ToString()
        dbcon.strmsg = objBL.DeleteTempTable(objEN)
        PageLoadBind(objEN)
        Clear()
        panelview.Visible = True
        PanelNewRequest.Visible = False
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