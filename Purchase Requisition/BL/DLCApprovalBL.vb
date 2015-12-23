Imports System
Imports DAL
Imports EN
Public Class DLCApprovalBL
    Dim objDA As DLCApprovalDA = New DLCApprovalDA()
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
    Public Function BindExpenseSummaryApproval(ByVal objEN As PRApprovalEN) As DataSet
        Try
            Return objDA.BindExpenseSummaryApproval(objEN)
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
    Public Function GetEmpUserid(ByVal objEN As PRApprovalEN) As Integer
        Try
            Return objDA.GetEmpUserid(objEN)
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
End Class
