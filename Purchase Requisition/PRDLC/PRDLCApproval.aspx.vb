Imports System
Imports System.Drawing
Imports System.Globalization
Imports System.Configuration
Imports System.Xml
Imports System.IO
Imports BL
Imports DAL
Imports EN
Public Class PRDLCApproval
    Inherits System.Web.UI.Page
    Dim dbCon As DBConnectionDA = New DBConnectionDA()
    Dim objBL As DLCApprovalBL = New DLCApprovalBL()
    Dim objEN As PRApprovalEN = New PRApprovalEN()
    Dim BlnFlag As Boolean = True
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
                BlnFlag = dbCon.CheckAuthorizer(objEN.UserCode, "PRD")
                If BlnFlag = True Then
                    PanelMain.Visible = True
                    ReqApproval(objEN)
                Else
                    strError = "You are not authorized to this action."
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "js", "<script>alert('" & strError & "')</script>")
                End If
            End If
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "js", "<script>alert('" & ex.Message & "')</script>")
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
                grdRequestApproval.DataBind()
            End If

            If dbCon.ds.Tables(1).Rows.Count > 0 Then
                grdSummaryLoad.DataSource = dbCon.ds.Tables(1)
                grdSummaryLoad.DataBind()
            Else
                grdSummaryLoad.DataBind()
                grdSummary.DataBind()
            End If

        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            dbCon.strmsg = "" & ex.Message & ""
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
                For Each row1 As GridViewRow In GrdLoadRequest.Rows
                    If row1.RowIndex <> introw Then
                        row1.BackColor = Color.White
                    Else
                        row1.BackColor = Color.Orange
                    End If
                Next
                objEN.EmpId = Session("UserCode").ToString()
                objEN.UserCode = objBL.GetUserCode(objEN)
                objEN.DocEntry = DocNo.Text.Trim()
                BindLoadRequestApproval(objEN)
            End If
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            dbCon.strmsg = "" & ex.Message & ""
            ClientScript.RegisterStartupScript(Me.GetType(), "msg", "<script>alert('" & dbCon.strmsg & "')</script>")
        End Try
    End Sub
    Private Sub BindLoadRequestApproval(ByVal objEN As PRApprovalEN)
        Try
            dbCon.Ds3 = objBL.BindExpenseSummaryApproval(objEN)
            If dbCon.Ds3.Tables(0).Rows.Count > 0 Then
                grdRequestApproval.DataSource = dbCon.Ds3.Tables(0)
                grdRequestApproval.DataBind()
            Else
                grdRequestApproval.DataBind()
            End If
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            dbCon.strmsg = "" & ex.Message & ""
            ClientScript.RegisterStartupScript(Me.GetType(), "msg", "<script>alert('" & dbCon.strmsg & "')</script>")
        End Try
    End Sub
    Protected Sub imgbtnSum_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim btndetails As ImageButton = TryCast(sender, ImageButton)
            Dim gvrow As GridViewRow = DirectCast(btndetails.NamingContainer, GridViewRow)
            Dim CallID As LinkButton = CType(gvrow.FindControl("lblSCode"), LinkButton)
            Dim lblempNo As Label = CType(gvrow.FindControl("lblSEmpid"), Label)
            Dim EmpID As String = lblempNo.Text.Trim()
            objEN.Code = CallID.Text.Trim()
            objEN.EmpId = EmpID.Trim()
            BindHistory(objEN)
            ModalPopupExtender2.Show()
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            dbCon.strmsg = "" & ex.Message & ""
            ClientScript.RegisterStartupScript(Me.GetType(), "msg", "<script>alert('" & dbCon.strmsg & "')</script>")
        End Try
    End Sub
    Protected Sub imgbtnReq_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim btndetails As ImageButton = TryCast(sender, ImageButton)
            Dim gvrow As GridViewRow = DirectCast(btndetails.NamingContainer, GridViewRow)
            Dim CallID As LinkButton = CType(gvrow.FindControl("lblRCode"), LinkButton)
            Dim lblempNo As Label = CType(gvrow.FindControl("lblREmpid"), Label)
            Dim EmpID As String = lblempNo.Text.Trim()
            objEN.Code = CallID.Text.Trim()
            objEN.EmpId = EmpID.Trim()
            BindHistory(objEN)
            ModalPopupExtender2.Show()
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
                grdRequesttohr.DataSource = dbCon.Ds2.Tables(0)
                grdRequesttohr.DataBind()
            Else
                grdRequesttohr.DataBind()
            End If
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            dbCon.strmsg = "'" & ex.Message & "'"
            ClientScript.RegisterStartupScript(Me.GetType(), "msg", "<script>alert('" & dbCon.strmsg & "')</script>")
        End Try
    End Sub

    Private Sub GrdLoadRequest_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GrdLoadRequest.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim ddlStatus As DropDownList = CType(e.Row.FindControl("ddlHAppStatus"), DropDownList)
            Dim lblDocStatus As Label = CType(e.Row.FindControl("lblRDocStatus"), Label)
            Dim imgbtn As ImageButton = CType(e.Row.FindControl("imgbtnReq"), ImageButton)
            If (ddlStatus.SelectedValue <> "P" Or lblDocStatus.Text.Trim() = "DLC InProgress") Then
                imgbtn.Visible = True
            Else
                imgbtn.Visible = False
            End If
        End If
    End Sub

    Private Sub grdSummaryLoad_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdSummaryLoad.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblStatus As Label = CType(e.Row.FindControl("lblSAppStatus"), Label)
            Dim lblDocStatus As Label = CType(e.Row.FindControl("lblSDocStatus"), Label)
            Dim imgbtn As ImageButton = CType(e.Row.FindControl("imgbtnSum"), ImageButton)
            If (lblStatus.Text.Trim() <> "Pending" Or lblDocStatus.Text.Trim() = "DLC InProgress") Then
                imgbtn.Visible = True
            Else
                imgbtn.Visible = False
            End If
        End If
    End Sub

    Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Dim StrMailMessage As String = ""
        Dim row1 As GridViewRow
        Try
            If Session("UserCode") Is Nothing Or Session("SAPCompany") Is Nothing Then
                dbCon.strmsg = "Your session is Expired..."
                ClientScript.RegisterStartupScript(Me.GetType(), "msg", "<script>alert('" & dbCon.strmsg & "')</script>")
                Response.Redirect("Login.aspx?sessionExpired=true", True)
            ElseIf Validation() = True Then
                objEN.EmpId = Session("UserCode").ToString()
                objEN.UserCode = objBL.GetUserCode(objEN)
                objEN.EmpUserId = objBL.GetEmpUserid(objEN)
                For Each row1 In GrdLoadRequest.Rows
                    objEN.DeptCode = CType(row1.FindControl("lblRdeptCode"), Label).Text
                    objEN.EmpId = CType(row1.FindControl("lblREmpid"), Label).Text
                    objEN.EmpName = CType(row1.FindControl("lblREmpname"), Label).Text
                    objEN.Code = CType(row1.FindControl("lblRCode"), LinkButton).Text
                    objEN.AppStatus = CType(row1.FindControl("ddlHAppStatus"), DropDownList).SelectedValue
                    objEN.Remarks = CType(row1.FindControl("txtHRemarks"), TextBox).Text
                    objEN.WhsCode = CType(row1.FindControl("lblRwhs"), Label).Text
                    objEN.HistoryType = "PRD"
                    objEN.HeaderType = "PRD"
                    objEN.DocMessage = "Purchase Requisition DLC Approval"
                    objEN.SapCompany = Session("SAPCompany")
                    dbCon.strmsg = objBL.addUpdateDocument(objEN)
                    If dbCon.strmsg <> "Success" And dbCon.strmsg <> "Successfully approved document..." Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "msg", "<script>alert('" & dbCon.strmsg & "')</script>")
                    End If
                Next
                ClientScript.RegisterStartupScript(Me.GetType(), "msg", "<script>alert('" & dbCon.strmsg & "')</script>")
                objEN.EmpId = Session("UserCode").ToString()
                objEN.UserCode = objBL.GetUserCode(objEN)
                ReqApproval(objEN)
            End If
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
        End Try
    End Sub
    Private Function Validation() As Boolean
        Try
            For Each row1 In GrdLoadRequest.Rows
                objEN.AppStatus = CType(row1.FindControl("ddlHAppStatus"), DropDownList).SelectedValue
                objEN.Remarks = CType(row1.FindControl("txtHRemarks"), TextBox).Text
                If objEN.AppStatus = "R" And objEN.Remarks = "" Then
                    ' ClientScript.RegisterStartupScript(Me.GetType(), "msg", "<script>alert('You must enter remarks for rejected requests.')</script>")

                    dbCon.strmsg = "alert('You must enter remarks for rejected requests.')"
                    mess(dbCon.strmsg)
                    Return False
                End If
            Next
            Return True
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            Return False
        End Try
    End Function
    Private Sub mess(ByVal str As String)
        ErrHandler.WriteError(dbCon.strmsg)
        ScriptManager.RegisterStartupScript(Update, Update.[GetType](), "strmsg", dbcon.strmsg, True)
    End Sub
End Class