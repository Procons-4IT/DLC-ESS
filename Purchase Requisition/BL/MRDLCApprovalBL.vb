Imports System
Imports DAL
Imports EN
Public Class MRDLCApprovalBL
    Dim objDA As MRDLCApprovalDA = New MRDLCApprovalDA()
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
