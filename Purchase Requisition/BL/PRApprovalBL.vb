Imports System
Imports DAL
Imports EN
Public Class PRApprovalBL
    Dim objDA As PRApprovalDA = New PRApprovalDA()
    Public Function GetUserCode(ByVal objEN As PRApprovalEN) As String
        Try
            Return objDA.GetUserCode(objEN)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function MainGridBind(ByVal objen As PRApprovalEN) As DataSet
        Try
            Return objDA.MainGridBind(objen)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function BindPRequestApproval(ByVal objEN As PRApprovalEN) As DataSet
        Try
            Return objDA.BindPRequestApproval(objEN)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function CancelRequest(ByVal objEN As PRApprovalEN) As String
        Try
            Return objDA.CancelRequest(objEN)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function BindItemCode(ByVal objen As PRApprovalEN) As DataSet
        Try
            Return objDA.BindItemCode(objen)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function BindUom(ByVal objen As PRApprovalEN) As DataSet
        Try
            Return objDA.BindUom(objen)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function AlternateRequest(ByVal objEN As PRApprovalEN) As String
        Try
            Return objDA.AlternateRequest(objEN)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ReBindPRequest(ByVal objEN As PRApprovalEN) As DataSet
        Try
            Return objDA.ReBindPRequest(objEN)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetEmpUserid(ByVal objEN As PRApprovalEN) As Integer
        Try
            Return objDA.GetEmpUserid(objEN)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ApprovalValidation(ByVal objEN As PRApprovalEN) As String
        Try
            Return objDA.ApprovalValidation(objEN)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function addUpdateDocument(ByVal objEN As PRApprovalEN) As String
        Try
            Return objDA.addUpdateDocument(objEN)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function LoadHistory(ByVal objEN As PRApprovalEN) As DataSet
        Try
            Return objDA.LoadHistory(objEN)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function BindExpenseSummaryApproval(ByVal objEN As PRApprovalEN) As DataSet
        Try
            Return objDA.BindExpenseSummaryApproval(objEN)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
   
End Class
