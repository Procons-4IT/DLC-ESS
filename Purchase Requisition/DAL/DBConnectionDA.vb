Imports Microsoft.VisualBasic
Imports System
Imports System.Web
Imports System.Xml
Imports System.Configuration
Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Imports EN
Imports System.Data.Odbc
Imports System.Net.Mail
Imports System.Collections.Specialized
Imports System.Security.Cryptography
Imports System.Web.HttpServerUtility
Imports System.Text
Imports System.Management
Imports System.Web.UI.WebControls

Public Class DBConnectionDA
    Dim DBConnection As String
    Dim oRecordSet As SAPbobsCOM.Recordset
    Public oCompany, objMainCompany As New SAPbobsCOM.Company
    Public strmsg, strQuery As String
    Public ds As DataSet = New DataSet()
    Public Ds1 As DataSet = New DataSet()
    Public Ds2 As DataSet = New DataSet()
    Public Ds3 As DataSet = New DataSet()
    Public Ds4 As DataSet = New DataSet()
    Public dss As DataSet = New DataSet()
    Public dss1 As DataSet = New DataSet()
    Public dss2 As DataSet = New DataSet()
    Public dss3 As DataSet = New DataSet()
    Public dss4 As DataSet = New DataSet()
    Public sqlreader As SqlDataReader
    Dim strError As String
    Public con As SqlConnection
    Public cmd As SqlCommand
    Public sqlda As SqlDataAdapter
    Public key As String = "!@#$%^*()"
    Dim SmtpServer As New Net.Mail.SmtpClient()
    Dim mail As New Net.Mail.MailMessage
    Dim mailServer As String
    Dim mailPort As String
    Dim mailId As String
    Dim mailUser As String
    Dim mailPwd As String
    Dim mailSSL As String
    Dim toID As String
    Dim ccID As String
    Dim mType As String
    Dim path As String
    Dim sQuery As String
    Dim strEmpName As String
    Public Sub New()
        DBConnection = "data source=" & ConfigurationManager.AppSettings("SAPServer") & ";Integrated Security=SSPI;database=" & ConfigurationManager.AppSettings("CompanyDB") & ";User id=" & ConfigurationManager.AppSettings("DbUserName") & "; password=" & ConfigurationManager.AppSettings("DbPassword")
    End Sub
    Public Function GetConnection() As String
        Return DBConnection
    End Function
    Public Function Encrypt(ByVal strText As String, ByVal strEncrKey _
       As String) As String
        Dim byKey() As Byte = {}
        Dim IV() As Byte = {&H12, &H34, &H56, &H78, &H90, &HAB, &HCD, &HEF}
        Try
            byKey = System.Text.Encoding.UTF8.GetBytes(Strings.Left(strEncrKey, 8))
            Dim des As New DESCryptoServiceProvider()
            Dim inputByteArray() As Byte = Encoding.UTF8.GetBytes(strText)
            Dim ms As New MemoryStream()
            Dim cs As New CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write)
            cs.Write(inputByteArray, 0, inputByteArray.Length)
            cs.FlushFinalBlock()
            Return Convert.ToBase64String(ms.ToArray())
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            Return ex.Message
        End Try
    End Function

    Public Function Decrypt(ByVal strText As String, ByVal sDecrKey _
               As String) As String
        Dim byKey() As Byte = {}
        Dim IV() As Byte = {&H12, &H34, &H56, &H78, &H90, &HAB, &HCD, &HEF}
        Dim inputByteArray(strText.Length) As Byte
        Try
            byKey = System.Text.Encoding.UTF8.GetBytes(Strings.Left(sDecrKey, 8))
            Dim des As New DESCryptoServiceProvider()
            inputByteArray = Convert.FromBase64String(strText)
            Dim ms As New MemoryStream()
            Dim cs As New CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write)
            cs.Write(inputByteArray, 0, inputByteArray.Length)
            cs.FlushFinalBlock()
            Dim encoding As System.Text.Encoding = System.Text.Encoding.UTF8
            Return encoding.GetString(ms.ToArray())
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            Return ex.Message
        End Try
    End Function

    Public Function getLoginPassword(ByVal strLicenseText As String) As String
        Dim fields() As String
        Dim strPwd As String
        If strLicenseText = "" Then
            Return ""
        End If
        Try

            Dim strDecryptText As String = Decrypt(strLicenseText, key)
            fields = strDecryptText.Split("$")

            If fields.Length > 0 Then
                strPwd = fields(0)
            Else
                strPwd = ""
            End If
        Catch ex As Exception
            strPwd = strLicenseText
        End Try
        Return strPwd
    End Function
    Public Sub GridviewBind(ByVal query As String, ByVal Gv As GridView)
        Dim con As SqlConnection = New SqlConnection(GetConnection)
        Dim da As SqlDataAdapter = New SqlDataAdapter(query, con)
        Dim ds As DataSet = New DataSet()
        da.Fill(ds)
        Gv.DataSource = ds
        Gv.DataBind()
    End Sub
    Public Function Connection() As String
        Try
            'readxml()
            oCompany = New SAPbobsCOM.Company
            objMainCompany = New SAPbobsCOM.Company
            oCompany.Server = ConfigurationManager.AppSettings("SAPServer") ' objen.ServerName
            Select Case ConfigurationManager.AppSettings("DbServerType")
                Case "2005"
                    oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2005
                Case "2008"
                    oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2008
                Case "2012"
                    oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2012
                Case "2014"
                    oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2014
            End Select
            oCompany.DbUserName = ConfigurationManager.AppSettings("DbUserName") ' objen.UID
            oCompany.DbPassword = ConfigurationManager.AppSettings("DbPassword") ' objen.PWD
            oCompany.CompanyDB = ConfigurationManager.AppSettings("CompanyDB") ' objen.SQLCompany
            oCompany.UserName = ConfigurationManager.AppSettings("SAPuserName") ' objen.CUID
            oCompany.Password = ConfigurationManager.AppSettings("SAPpassword") ' objen.CPWD
            oCompany.LicenseServer = ConfigurationManager.AppSettings("SAPlicense") ' objen.License
            oCompany.UseTrusted = ConfigurationManager.AppSettings("SAPtursted") ' False
            If oCompany.Connect <> 0 Then
                strError = oCompany.GetLastErrorDescription()
                ErrHandler.WriteError(strError)
                Return strError
            Else
                objMainCompany = oCompany
                Return "Success"
            End If
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            Throw ex
        End Try
    End Function
    Public Function Connection(ByVal UserID As String, ByVal Pwd As String) As String
        Try

            Pwd = Decrypt(Pwd, key)
            oCompany = New SAPbobsCOM.Company
            objMainCompany = New SAPbobsCOM.Company
            oCompany.Server = ConfigurationManager.AppSettings("SAPServer") ' objen.ServerName
            Select Case ConfigurationManager.AppSettings("DbServerType")
                Case "2005"
                    oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2005
                Case "2008"
                    oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2008
                Case "2012"
                    oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2012
                Case "2014"
                    oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2014
            End Select
            oCompany.DbUserName = ConfigurationManager.AppSettings("DbUserName") ' objen.UID
            oCompany.DbPassword = ConfigurationManager.AppSettings("DbPassword") ' objen.PWD
            oCompany.CompanyDB = ConfigurationManager.AppSettings("CompanyDB") ' objen.SQLCompany
            oCompany.UserName = UserID ' ConfigurationManager.AppSettings("SAPuserName") ' objen.CUID
            oCompany.Password = Pwd ' ConfigurationManager.AppSettings("SAPpassword") ' objen.CPWD
            oCompany.LicenseServer = ConfigurationManager.AppSettings("SAPlicense") ' objen.License
            oCompany.UseTrusted = ConfigurationManager.AppSettings("SAPtursted") ' False
            If oCompany.Connect <> 0 Then
                strError = oCompany.GetLastErrorDescription()
                ErrHandler.WriteError(strError)
                Return strError
            Else
                objMainCompany = oCompany
                Return "Success"
            End If
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            Throw ex
        End Try
    End Function
    Public Function Getmaxcode(ByVal sTable As String, ByVal sColumn As String) As Integer
        Dim MaxCode As Integer
        con = New SqlConnection(DBConnection)
        con.Open()
        cmd = New SqlCommand("SELECT isnull(max(isnull(CAST(isnull(" & sColumn & ",'0') AS Numeric),0)),0) FROM " & sTable & "", con)
        cmd.CommandType = CommandType.Text
        MaxCode = Convert.ToString(cmd.ExecuteScalar())
        If MaxCode >= 0 Then
            MaxCode = MaxCode + 1
        Else
            MaxCode = 1
        End If
        con.Close()
        Return MaxCode
    End Function
    Public Function checktable() As String
        con = New SqlConnection(DBConnection)
        Dim exists As Integer = 0
        strQuery = "SELECT COUNT(*) FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[U_OPRQ]') AND type in (N'U')"
        cmd = New SqlCommand(strQuery, con)
        con.Open()
        sqlreader = cmd.ExecuteReader()
        If sqlreader.HasRows Then
            Do
                While sqlreader.Read
                    exists = sqlreader(0)
                End While
            Loop While sqlreader.NextResult()
        End If
        con.Close()
        If exists = 0 Then
            strQuery = "CREATE TABLE [U_OPRQ](UniqueId INT IDENTITY NOT NULL,EmpId nvarchar(50) NOT NULL,Empname nvarchar(100) NULL,DocDate dateTime null,DefWhs varchar(30) Null,"
            strQuery += " DeptCode nvarchar(50) NULL,DeptName nvarchar(100) NULL,DocNum nvarchar(30) NULL,DocStatus [char](10) NULL,Priority [char](10) NULL,"
            strQuery += " OrdPatient [char](1) NULL,Series [nvarchar](20) NULL,RefNo nvarchar(50) NULL,ItemCode nvarchar(50) NULL,ItemName [nvarchar](150) NULL,ItemSpec nvarchar(200) NULL,"
            strQuery += " OrderQty Decimal(18,6) NULL,OrderUom nvarchar(100) NULL,OrderUomDesc nvarchar(100) NULL,AltItemCode nvarchar(200) NULL,AltItemDesc nvarchar(200) NULL,DelQty  Decimal(18,6) NULL,DelUom [nvarchar](30) NULL,DelUomDesc [nvarchar](100) NULL,ReceivedQty  Decimal(18,6) NULL,ReceivedUom [nvarchar](30) NULL,LineStatus nvarchar(40) NULL,"
            strQuery += " Barcode nvarchar(100) NULL,AppStatus nchar(1) NULL,SessionId nchar(100) NULL,ReceivedUomDesc [nvarchar](30) NULL,AlterBarCode [nvarchar](30) NULL,NewDocStatus [nvarchar](30) NULL,CostCenter [nvarchar](250) NULL)"
            cmd = New SqlCommand(strQuery, con)
            con.Open()
            cmd.ExecuteNonQuery()
            con.Close()
        End If

        exists = 0
        strQuery = "SELECT COUNT(*) FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[U_ORPD]') AND type in (N'U')"
        cmd = New SqlCommand(strQuery, con)
        cmd.Connection.Open()
        sqlreader = cmd.ExecuteReader()
        If sqlreader.HasRows Then
            Do
                While sqlreader.Read
                    exists = sqlreader(0)
                End While
            Loop While sqlreader.NextResult()
        End If
        cmd.Connection.Close()
        If exists = 0 Then
            strQuery = "CREATE TABLE [U_ORPD](UniqueId INT IDENTITY NOT NULL,EmpId nvarchar(50) NOT NULL,Empname nvarchar(100) NULL,DocDate dateTime null,DefWhs varchar(30) Null,"
            strQuery += " DocNum nvarchar(30) NULL,DocStatus [char](10) NULL, DeptCode nvarchar(50) NULL,DeptName nvarchar(100) NULL,"
            strQuery += " RefNo nvarchar(50) NULL,ItemCode nvarchar(50) NULL,ItemName [nvarchar](150) NULL,OrderQty Decimal(18,6) NULL,OrderUom nvarchar(100) NULL,OrderUomDesc nvarchar(100) NULL,LineStatus nvarchar(40) NULL,"
            strQuery += " Barcode nvarchar(100) NULL,AppStatus nchar(1) NULL,SessionId nchar(100) NULL,HeadDocNo [nvarchar](30) NULL,NewDocStatus nchar(1) NULL,GoodsReceipt nvarchar(30) NULL)"
            cmd = New SqlCommand(strQuery, con)
            cmd.Connection.Open()
            cmd.ExecuteNonQuery()
            cmd.Connection.Close()
        End If

        exists = 0
        strQuery = "SELECT COUNT(*) FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[U_SESSION]') AND type in (N'U')"
        cmd = New SqlCommand(strQuery, con)
        cmd.Connection.Open()
        sqlreader = cmd.ExecuteReader()
        If sqlreader.HasRows Then
            Do
                While sqlreader.Read
                    exists = sqlreader(0)
                End While
            Loop While sqlreader.NextResult()
        End If
        cmd.Connection.Close()
        If exists = 0 Then
            strQuery = "CREATE TABLE [dbo].[U_SESSION]([U_SESSIONID] [int] IDENTITY(1,1) NOT NULL,[U_EmpCode] [nvarchar](max) NOT NULL, [U_LOGIN_DATE] DATETIME, [U_LOGOUT_DATE] DATETIME)"
            cmd = New SqlCommand(strQuery, con)
            cmd.Connection.Open()
            cmd.ExecuteNonQuery()
            cmd.Connection.Close()
        End If


        exists = 0
        strQuery = "SELECT COUNT(*) FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[U_HRSESSION]') AND type in (N'U')"
        cmd = New SqlCommand(strQuery, con)
        con.Open()
        sqlreader = cmd.ExecuteReader()
        If sqlreader.HasRows Then
            Do
                While sqlreader.Read
                    exists = sqlreader(0)
                End While
            Loop While sqlreader.NextResult()
        End If
        con.Close()
        If exists = 0 Then
            strQuery = "CREATE TABLE U_HRSESSION(U_SESSIONID int IDENTITY(1,1) NOT NULL,empID nvarchar(max) NOT NULL,empName nvarchar(Max) NOT NULL,"
            strQuery += "U_LOGIN_DATE DATETIME, U_LOGOUT_DATE DATETIME)"
            cmd = New SqlCommand(strQuery, con)
            con.Open()
            cmd.ExecuteNonQuery()
            con.Close()
        End If
        Return ""
    End Function

    Public Function DocApproval(ByVal DocType As String, ByVal dept As String) As String
        Try
            Dim strQuery As String = ""
            Dim Status As String = ""
            con = New SqlConnection(GetConnection)
            Select Case DocType
                Case "PR", "MR", "PRD", "MRD"
                    strQuery = "Select * from ""@Z_DLC_OAPPT"" T0 left join ""@Z_DLC_APPT3"" T1 on T0.""DocEntry""=T1.""DocEntry"" where T0.""U_Z_Active""='Y' and T0.""U_Z_DocType""='" & DocType.ToString() & "' and T1.""U_Z_DeptCode""='" & dept & "' "
                    sqlda = New SqlDataAdapter(strQuery, con)
                    sqlda.Fill(Ds2)
                    If Ds2.Tables(0).Rows.Count > 0 Then
                        Status = "P"
                    Else
                        strQuery = "Select * from ""@Z_DLC_OAPPT"" T0 where T0.""U_Z_Active""='Y' and T0.""U_Z_DocType""='" & DocType.ToString() & "' and T0.""U_Z_AllDept""='Y' "
                        sqlda = New SqlDataAdapter(strQuery, con)
                        sqlda.Fill(Ds3)
                        If Ds3.Tables(0).Rows.Count > 0 Then
                            Status = "P"
                        Else
                            Status = "A"
                        End If
                    End If
            End Select
            Return Status
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            Throw ex
            Return False
        End Try
    End Function

    Public Function GetTemplateID(ByVal DocType As String, ByVal dept As String, Optional ByVal LeaveType As String = "") As String
        Try
            Dim strQuery As String = ""
            Dim Status As String = ""
            Dim con As SqlConnection = New SqlConnection(GetConnection)
            Select Case DocType
                Case "PR", "MR", "PRD", "MRD"
                    strQuery = "Select * from ""@Z_DLC_OAPPT"" T0 left join ""@Z_DLC_APPT3"" T1 on T0.""DocEntry""=T1.""DocEntry"" where T0.""U_Z_Active""='Y' and T0.""U_Z_DocType""='" & DocType.ToString() & "' and T1.""U_Z_DeptCode""='" & dept & "' "
                    sqlda = New SqlDataAdapter(strQuery, con)
                    sqlda.Fill(dss2)
                    If dss2.Tables(0).Rows.Count > 0 Then
                        Status = dss2.Tables(0).Rows(0)("DocEntry").ToString()
                    Else
                        strQuery = "Select * from ""@Z_DLC_OAPPT"" T0 where T0.""U_Z_Active""='Y' and T0.""U_Z_DocType""='" & DocType.ToString() & "' and T0.""U_Z_AllDept""='Y' "
                        sqlda = New SqlDataAdapter(strQuery, con)
                        sqlda.Fill(dss3)
                        If dss3.Tables(0).Rows.Count > 0 Then
                            Status = dss3.Tables(0).Rows(0)("DocEntry").ToString()
                        Else
                            Status = "0"
                        End If
                    End If
            End Select
            Return Status
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            Throw ex
        End Try
    End Function

    Public Sub UpdateApprovalRequired(ByVal strTable As String, ByVal sColumn As String, ByVal StrCode As String, ByVal ReqValue As String, ByVal AppTempId As String)
        Try
            Dim con As SqlConnection = New SqlConnection(GetConnection)
            strQuery = "Update [" & strTable & "] set U_Z_AppRequired='" & ReqValue & "',U_Z_AppReqDate=getdate(),U_Z_ApproveId='" & AppTempId & "',"
            strQuery += " U_Z_ReqTime='" & Now.TimeOfDay.ToString() & "' where " & sColumn & "='" & StrCode & "'"
            cmd = New SqlCommand(strQuery, con)
            con.Open()
            cmd.ExecuteNonQuery()
            con.Close()
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            Throw ex
        End Try
    End Sub
    Public Sub InitialMessage(ByVal strReqType As String, ByVal strReqNo As String, ByVal strAppStatus As String _
         , ByVal strTemplateNo As String, ByVal strOrginator As String, ByVal enDocType As String, ByVal objMainCompany As SAPbobsCOM.Company, Optional ByVal strExpNo As String = "")
        Try
            'If ConnectSAP() = True Then
            Dim strQuery, strSubject As String
            Dim strEmailMessage As String
            Dim strMessageUser, strExpReqNo1 As String
            Dim oRecordSet, oTemp As SAPbobsCOM.Recordset
            Dim oCmpSrv As SAPbobsCOM.CompanyService
            Dim oMessageService As SAPbobsCOM.MessagesService
            Dim oMessage As SAPbobsCOM.Message
            Dim pMessageDataColumns As SAPbobsCOM.MessageDataColumns
            Dim pMessageDataColumn As SAPbobsCOM.MessageDataColumn
            Dim oLines As SAPbobsCOM.MessageDataLines
            Dim oLine As SAPbobsCOM.MessageDataLine
            Dim oRecipientCollection As SAPbobsCOM.RecipientCollection
            oCmpSrv = objMainCompany.GetCompanyService()
            oMessageService = oCmpSrv.GetBusinessService(SAPbobsCOM.ServiceTypes.MessagesService)
            oMessage = oMessageService.GetDataInterface(SAPbobsCOM.MessagesServiceDataInterfaces.msdiMessage)
            oRecordSet = objMainCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oTemp = objMainCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            strQuery = "Select Top 1 U_Z_AUser From [@Z_DLC_APPT2] Where DocEntry = '" + strTemplateNo + "'  and isnull(U_Z_AMan,'')='Y' Order By LineId Asc "
            oRecordSet.DoQuery(strQuery)
            If Not oRecordSet.EoF Then
                strMessageUser = oRecordSet.Fields.Item(0).Value
                oMessage.Subject = strReqType + ":" + "Need Your Approval "
                Dim strMessage As String = ""
                Select Case enDocType
                    Case "PR" 'Purchase Requisition"
                        strQuery = "Select * from  [@Z_OPRQ] where DocEntry='" & strReqNo & "'"
                        oTemp.DoQuery(strQuery)
                        strMessage = " Requested by  :" & oTemp.Fields.Item("U_Z_EmpName").Value & ""
                        strOrginator = strMessage
                    Case "MR" 'Material return"
                        strQuery = "Select * from  [@Z_ORPD] where DocEntry='" & strReqNo & "'"
                        oTemp.DoQuery(strQuery)
                        strMessage = " Requested by  :" & oTemp.Fields.Item("U_Z_EmpName").Value & ""
                        strOrginator = strMessage
                    Case "PRD" 'Purchase Requisition DLC"
                        strQuery = "Select * from  [@Z_OPRQ] where DocEntry='" & strReqNo & "'"
                        oTemp.DoQuery(strQuery)
                        strMessage = " Requested by  :" & oTemp.Fields.Item("U_Z_EmpName").Value & ""
                        strOrginator = strMessage
                    Case "MRD" 'Material return DLC"
                        strQuery = "Select * from  [@Z_ORPD] where DocEntry='" & strReqNo & "'"
                        oTemp.DoQuery(strQuery)
                        strMessage = " Requested by  :" & oTemp.Fields.Item("U_Z_EmpName").Value & ""
                        strOrginator = strMessage
                End Select
                Select Case enDocType
                    Case "PR"
                        strQuery = "Update [@Z_PRQ1] set U_Z_CurApprover='" & strMessageUser & "',U_Z_NxtApprover='" & strMessageUser & "' where DocEntry =" & strExpNo & ""
                        oTemp.DoQuery(strQuery)
                        strSubject = "Purchase Requisition needs your approval"
                    Case "MR"
                        strQuery = "Update [@Z_RPD1] set U_Z_CurApprover='" & strMessageUser & "',U_Z_NxtApprover='" & strMessageUser & "' where DocEntry ='" & strExpNo & "'"
                        oTemp.DoQuery(strQuery)
                        strSubject = "Material Return needs your approval"
                    Case "PRD"
                        strQuery = "Update [@Z_PRQ1] set U_Z_CurApprover='" & strMessageUser & "',U_Z_NxtApprover='" & strMessageUser & "' where DocEntry=" & strExpNo & ""
                        oTemp.DoQuery(strQuery)
                        strSubject = "Purchase Requisition DLC needs your approval"
                    Case "MRD"
                        strQuery = "Update [@Z_RPD1] set U_Z_CurApprover='" & strMessageUser & "',U_Z_NxtApprover='" & strMessageUser & "' where DocEntry ='" & strExpNo & "'"
                        oTemp.DoQuery(strQuery)
                        strSubject = "Material Return DLC needs your approval"
                End Select
                Dim IntReqNo As Integer = Integer.Parse(strReqNo)
                Dim strExpReqNo As String = IntReqNo.ToString()
                Dim IntReqNo1 As String = strExpNo
                strExpReqNo1 = IntReqNo1.ToString()
                oMessage.Text = strReqType + "  " + strExpReqNo + " with LineNo :  " + strExpNo + " " + strOrginator + " Needs Your Approval "
                oRecipientCollection = oMessage.RecipientCollection
                oRecipientCollection.Add()
                oRecipientCollection.Item(0).SendInternal = SAPbobsCOM.BoYesNoEnum.tYES
                oRecipientCollection.Item(0).UserCode = strMessageUser
                pMessageDataColumns = oMessage.MessageDataColumns

                pMessageDataColumn = pMessageDataColumns.Add()
                pMessageDataColumn.ColumnName = "Request No"
                oLines = pMessageDataColumn.MessageDataLines()
                oLine = oLines.Add()
                oLine.Value = strExpReqNo1
                oMessageService.SendMessage(oMessage)
                strEmailMessage = strReqType + "  " + strExpReqNo + " with Lines :  " + strExpReqNo1 + " " + strOrginator + " Needs Your Approval "
                SendMail_Approval(strEmailMessage, strMessageUser, strMessageUser, objMainCompany, strSubject)
            End If
            ' End If
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            Throw ex
        End Try
    End Sub
    Public Function GetData(ByVal query As String) As DataSet
        Dim cmd As New SqlCommand(query)
        Using con As New SqlConnection(GetConnection)
            Using sda As New SqlDataAdapter()
                cmd.Connection = con
                sda.SelectCommand = cmd
                Using ds As New DataSet()
                    sda.Fill(ds)
                    Return ds
                End Using
            End Using
        End Using
    End Function
    Public Sub SendMessage(ByVal strReqNo As String, ByVal strEmpNo As String, ByVal strAuthorizer As String, ByVal MailDocEntry As String, ByVal objCompany As SAPbobsCOM.Company, ByVal strEmpName As String, ByVal enDocType As String)
        Try
            Dim strQuery, strOrginator As String
            Dim strMessageUser As String
            Dim strSubjectmsg As String = ""
            Dim strTemplateNo As String = ""
            Dim strEmailMessage As String = ""
            Dim intLineID, IntReqno As Integer
            Dim oRecordSet, oTemp As SAPbobsCOM.Recordset
            Dim oCmpSrv As SAPbobsCOM.CompanyService
            Dim oMessageService As SAPbobsCOM.MessagesService
            Dim oMessage As SAPbobsCOM.Message
            Dim pMessageDataColumns As SAPbobsCOM.MessageDataColumns
            Dim pMessageDataColumn As SAPbobsCOM.MessageDataColumn
            Dim oLines As SAPbobsCOM.MessageDataLines
            Dim oLine As SAPbobsCOM.MessageDataLine
            Dim oRecipientCollection As SAPbobsCOM.RecipientCollection
            oCmpSrv = objCompany.GetCompanyService()
            oMessageService = oCmpSrv.GetBusinessService(SAPbobsCOM.ServiceTypes.MessagesService)
            oMessage = oMessageService.GetDataInterface(SAPbobsCOM.MessagesServiceDataInterfaces.msdiMessage)
            oRecordSet = objCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oTemp = objCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            IntReqno = CInt(strReqNo)
            strQuery = "select T0.DocEntry,T1.LineId from [@Z_DLC_OAPPT] T0 JOIN [@Z_DLC_APPT3] T1 on T0.DocEntry=T1.DocEntry"
            strQuery += " JOIN [@Z_DLC_APPT2] T2 on T1.DocEntry=T2.DocEntry"
            strQuery += " where T0.U_Z_DocType='" & enDocType & "' AND T2.U_Z_AUser='" & strAuthorizer & "' AND (T1.U_Z_DeptCode='" & strEmpNo & "' or T0.U_Z_AllDept='Y')"
            oTemp.DoQuery(strQuery)
            If oTemp.RecordCount > 0 Then
                strTemplateNo = oTemp.Fields.Item(0).Value
            End If
            strQuery = "Select LineId From [@Z_DLC_APPT2] Where DocEntry = '" & strTemplateNo & "' And U_Z_AUser = '" & strAuthorizer & "'"
            oRecordSet.DoQuery(strQuery)
            If Not oRecordSet.EoF Then
                intLineID = CInt(oRecordSet.Fields.Item(0).Value)
                strQuery = "Select Top 1 U_Z_AUser From [@Z_DLC_APPT2] Where  DocEntry = '" & strTemplateNo & "' And LineId > '" & intLineID.ToString() & "' and isnull(U_Z_AMan,'')='Y'  Order By LineId Asc "
                oRecordSet.DoQuery(strQuery)

                If Not oRecordSet.EoF Then
                    strMessageUser = oRecordSet.Fields.Item(0).Value
                    oMessage.Subject = "Purchase Requisition" & ":" & " Need Your Approval "
                    Dim strMessage As String = ""
                    Select Case enDocType
                        Case "PR"
                            strQuery = "Update [@Z_PRQ1] set U_Z_CurApprover='" & strAuthorizer & "',U_Z_NxtApprover='" & strMessageUser & "' where DocEntry='" & strReqNo & "' and LineID in (" & MailDocEntry & ")"
                            oTemp.DoQuery(strQuery)
                            strMessage = " Requested by  :" & strEmpName
                            strOrginator = strMessage
                            oMessage.Text = "Purchase Requisition" & " " & IntReqno.ToString() & strOrginator & " Needs Your Approval "
                            strEmailMessage = "Purchase Requisition" + "  " + IntReqno.ToString() + " " + " with details " + MailDocEntry + " " + strOrginator + " Needs Your Approval "
                            strSubjectmsg = "Purchase Requisition need Approval"
                        Case "MR"
                            strQuery = "Update [@Z_RPD1] set U_Z_CurApprover='" & strAuthorizer & "',U_Z_NxtApprover='" & strMessageUser & "' where DocEntry='" & strReqNo & "' and LineId in (" & MailDocEntry & ")"
                            oTemp.DoQuery(strQuery)
                            strMessage = " Requested by  :" & strEmpName
                            strOrginator = strMessage
                            oMessage.Text = "Material Return" & " " & IntReqno.ToString() & strOrginator & " Needs Your Approval "
                            strEmailMessage = "Material return" + "  " + IntReqno.ToString() + " " + " with details " + MailDocEntry + " " + strOrginator + " Needs Your Approval "
                            strSubjectmsg = "Material return need Approval"
                        Case "PRD"
                            strQuery = "Update [@Z_PRQ1] set U_Z_CurApprover='" & strAuthorizer & "',U_Z_NxtApprover='" & strMessageUser & "' where DocEntry=" & MailDocEntry & ""
                            oTemp.DoQuery(strQuery)
                            strQuery = "Update [@Z_OPRQ] set U_Z_DocStatus='DI' where DocEntry=" & MailDocEntry & ""
                            oTemp.DoQuery(strQuery)
                            strMessage = " Requested by  :" & strEmpName
                            strOrginator = strMessage
                            oMessage.Text = "Purchase Requisition" & " " & IntReqno.ToString() & strOrginator & " Needs Your Approval "
                            strEmailMessage = "Purchase Requisition" + "  " + IntReqno.ToString() + " " + " with details " + MailDocEntry + " " + strOrginator + " Needs Your Approval "
                            strSubjectmsg = "Purchase Requisition need Approval"
                        Case "MRD"
                            strQuery = "Update [@Z_RPD1] set U_Z_CurApprover='" & strAuthorizer & "',U_Z_NxtApprover='" & strMessageUser & "' where DocEntry='" & MailDocEntry & "'"
                            oTemp.DoQuery(strQuery)
                            strQuery = "Update [@Z_ORPD] set U_Z_DocStatus='DI' where DocEntry =" & MailDocEntry & ""
                            oTemp.DoQuery(strQuery)

                            strMessage = " Requested by  :" & strEmpName
                            strOrginator = strMessage
                            oMessage.Text = "Material Return" & " " & IntReqno.ToString() & strOrginator & " Needs Your Approval "
                            strEmailMessage = "Material return" + "  " + IntReqno.ToString() + " " + " with details " + MailDocEntry + " " + strOrginator + " Needs Your Approval "
                            strSubjectmsg = "Material return need Approval"
                    End Select
                    oRecipientCollection = oMessage.RecipientCollection
                    oRecipientCollection.Add()
                    oRecipientCollection.Item(0).SendInternal = SAPbobsCOM.BoYesNoEnum.tYES
                    oRecipientCollection.Item(0).UserCode = strMessageUser
                    pMessageDataColumns = oMessage.MessageDataColumns
                    pMessageDataColumn = pMessageDataColumns.Add()
                    pMessageDataColumn.ColumnName = "Request No"
                    oLines = pMessageDataColumn.MessageDataLines()
                    oLine = oLines.Add()
                    oLine.Value = MailDocEntry
                    oMessageService.SendMessage(oMessage)
                    SendMail_Approval(strEmailMessage, strMessageUser, strMessageUser, objCompany, strSubjectmsg)

                End If
            End If
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            Throw ex
        End Try
    End Sub
    Public Sub SendMail_Approval(ByVal aMessage As String, ByVal aMail As String, ByVal aUser As String, ByVal aCompany As SAPbobsCOM.Company, ByVal strSubject As String)
        Dim oRecordset As SAPbobsCOM.Recordset
        oRecordset = aCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
        Try
            oRecordset.DoQuery("Select U_Z_SMTPSERV,U_Z_SMTPPORT,U_Z_SMTPUSER,U_Z_SMTPPWD,U_Z_SSL From [@Z_DLC_OMAIL]")
            If Not oRecordset.EoF Then
                mailServer = oRecordset.Fields.Item("U_Z_SMTPSERV").Value
                mailPort = oRecordset.Fields.Item("U_Z_SMTPPORT").Value
                mailId = oRecordset.Fields.Item("U_Z_SMTPUSER").Value
                mailPwd = oRecordset.Fields.Item("U_Z_SMTPPWD").Value
                mailSSL = oRecordset.Fields.Item("U_Z_SSL").Value
                If mailServer <> "" And mailId <> "" And mailPwd <> "" Then
                    oRecordset.DoQuery("Select * from OUSR where USER_CODE='" & aUser & "'")
                    aMail = oRecordset.Fields.Item("E_Mail").Value
                    If aMail <> "" Then
                        SendMailforApproval(mailServer, mailPort, mailId, mailPwd, mailSSL, aMail, aMail, "Approval", aMessage, strSubject)
                    End If
                Else
                End If
            End If
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
        End Try
    End Sub

    Private Sub SendMailforApproval(ByVal mailServer As String, ByVal mailPort As String, ByVal mailId As String, ByVal mailpwd As String, ByVal mailSSL As String, ByVal toId As String, ByVal ccId As String, ByVal mType As String, ByVal Message As String, ByVal aSubject As String)
        Try

            'Dim strRptPath As String = System.Windows.Forms.Application.StartupPath.Trim() & "\Report.pdf"
            SmtpServer.Credentials = New Net.NetworkCredential(mailId, mailpwd)
            SmtpServer.Port = mailPort
            SmtpServer.EnableSsl = mailSSL
            SmtpServer.Host = mailServer
            mail = New Net.Mail.MailMessage()
            mail.From = New Net.Mail.MailAddress(mailId, "Purchase Requisition")
            mail.To.Add(toId)
            '  mail.CC.Add(ccId)
            mail.IsBodyHtml = True
            mail.Priority = MailPriority.High
            mail.Subject = aSubject
            mail.Body = Message
            SmtpServer.Send(mail)
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            Throw ex
        Finally
            mail.Dispose()
        End Try
    End Sub

    Public Sub SendMail_RequestApproval(ByVal aMessage As String, ByVal Empid As String, ByVal aCompany As SAPbobsCOM.Company, ByVal aSubject As String, Optional ByVal aMail As String = "")
        Dim oRecordset As SAPbobsCOM.Recordset
        oRecordset = aCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
        Try
            oRecordset.DoQuery("Select U_Z_SMTPSERV,U_Z_SMTPPORT,U_Z_SMTPUSER,U_Z_SMTPPWD,U_Z_SSL From [@Z_DLC_OMAIL]")
            If Not oRecordset.EoF Then
                mailServer = oRecordset.Fields.Item("U_Z_SMTPSERV").Value
                mailPort = oRecordset.Fields.Item("U_Z_SMTPPORT").Value
                mailId = oRecordset.Fields.Item("U_Z_SMTPUSER").Value
                mailPwd = oRecordset.Fields.Item("U_Z_SMTPPWD").Value
                mailSSL = oRecordset.Fields.Item("U_Z_SSL").Value
                If mailServer <> "" And mailId <> "" And mailPwd <> "" Then
                    oRecordset.DoQuery("Select * from OHEM where empID='" & Empid & "'")
                    aMail = oRecordset.Fields.Item("email").Value
                    If aMail <> "" Then
                        SendMailforApproval(mailServer, mailPort, mailId, mailPwd, mailSSL, aMail, aMail, "Approval", aMessage, aSubject)
                    End If
                Else
                End If
            End If
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
        End Try
    End Sub
    Public Function CheckAuthorizer(ByVal UserId As String, ByVal DocType As String) As Boolean
        Try
            Dim MaxCode As String
            con = New SqlConnection(DBConnection)
            con.Open()
            strQuery = "select T0.DocEntry from [@Z_DLC_OAPPT] T0 Inner Join [@Z_DLC_APPT2] T1 On T0.DocEntry=T1.DocEntry where T1.U_Z_AUser='" & UserId & "' "
            strQuery += " and T0.U_Z_DocType='" & DocType.Trim() & "' and T1.U_Z_AMan='Y' and T0.U_Z_Active='Y'"
            cmd = New SqlCommand(strQuery, con)
            cmd.CommandType = CommandType.Text
            MaxCode = Convert.ToString(cmd.ExecuteScalar())
            con.Close()
            If MaxCode <> "" Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            Throw ex
        End Try
    End Function
    Public Function getDepartment(ByVal objen1 As String) As String
        con = New SqlConnection(DBConnection)
        con.Open()
        cmd = New SqlCommand("SELECT dept   FROM OHEM WHERE   empID='" & objen1 & "'", con)
        cmd.CommandType = CommandType.Text
        Dim status As String
        status = cmd.ExecuteScalar()
        con.Close()
        Return status
    End Function
End Class
