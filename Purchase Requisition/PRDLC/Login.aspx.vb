Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Xml
Imports System.IO
Imports EN
Imports DAL
Imports BL
Public Class Login
    Inherits System.Web.UI.Page
    Dim objEn As LoginEN = New LoginEN()
    Dim objDA As LoginBL = New LoginBL()
    Dim dbCon As DBConnectionDA = New DBConnectionDA()
    Dim strstring As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                strstring = Request.QueryString("SessionExpired")
                If Request.QueryString("SessionExpired") <> Nothing Or Request.QueryString("SessionExpired") = "ture" Then
                    Dim strmsg As String = "Session expired. You will be redirected to Login page"
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "js", "<script>alert('" & strmsg & "')</script>")
                End If
            End If
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "js", "<script>alert('" & ex.Message & "')</script>")
        End Try
    End Sub

    Private Sub BtnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSubmit.Click
        Try
            Session.Clear()
            dbCon.checktable()
            objEn.Uid = TxtUid.Text.Trim()
            objEn.Pwd = TxtPwd.Text.Trim()
            If objEn.Uid = "" Then
                ClientScript.RegisterStartupScript([GetType](), "Message", "<script>alert('Enter the UserName')</script>")
            ElseIf objEn.Pwd = "" Then
                ClientScript.RegisterStartupScript([GetType](), "Message", "<script>alert('Enter the Password')</script>")
            ElseIf (objDA.UserAuthentication(objEn)) = True Then
                Session("SAPCompany") = Application("DBName")
                Session("UserCode") = objDA.GetCardCode(objEn)
                objEn.SessionId = Session("UserCode")
                Session("UserName") = objDA.GetCardName(objEn)
                Session("EmpUserName") = ConfigurationManager.AppSettings("SAPuserName")
                Session("UserPwd") = ConfigurationManager.AppSettings("SAPpassword")
                Session("SessionId") = objDA.SessionDetails(Session("UserCode").ToString())
                Response.Redirect("Home.aspx", False)
            Else
                ClientScript.RegisterStartupScript([GetType](), "Message", "<script>alert('login failed. UserName and Password does not matching')</script>")
            End If
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "js", "<script>alert('" & ex.Message & "')</script>")
        End Try
    End Sub
End Class