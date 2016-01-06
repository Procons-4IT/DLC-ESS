Imports System
Imports DAL
Imports EN
Public Class EmpPRBL
    Dim objDA As EmpPRDA = New EmpPRDA()
    Public Function PopulateEmployee(ByVal objen As EmpPREN) As DataSet
        Try
            Return objDA.PageLoadBind(objen)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetCostCenter(ByVal objen As EmpPREN) As String
        Try
            Return objDA.GetCostCenter(objen)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function BindItemCode(ByVal objen As EmpPREN) As DataSet
        Try
            Return objDA.BindItemCode(objen)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function BindUom(ByVal objen As EmpPREN) As DataSet
        Try
            Return objDA.BindUom(objen)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function InsertLines(ByVal objen As EmpPREN) As String
        Try
            Return objDA.InsertLines(objen)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function TempLines(ByVal objen As EmpPREN) As DataSet
        Try
            Return objDA.TempLines(objen)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function BindWhs(ByVal objen As EmpPREN) As DataSet
        Try
            Return objDA.BindWhs(objen)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function DeleteTempTable(ByVal objen As EmpPREN) As String
        Try
            Return objDA.DeleteTempTable(objen)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function BindPRequestApproval(ByVal objEN As EmpPREN) As DataSet
        Try
            Return objDA.BindPRequestApproval(objEN)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function PopulateExistingDocument(ByVal objen As EmpPREN) As String
        Try
            Return objDA.PopulateExistingDocument(objen)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function DeleteLinesTable(ByVal objen As EmpPREN) As String
        Try
            Return objDA.DeleteLinesTable(objen)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function UpdateReceived(ByVal objen As EmpPREN) As String
        Try
            Return objDA.UpdateReceived(objen)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function UpdateStatus(ByVal objen As EmpPREN) As String
        Try
            Return objDA.UpdateStatus(objen)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function LoadHistory(ByVal objEN As EmpPREN) As DataSet
        Try
            Return objDA.LoadHistory(objEN)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function CancelRequest(ByVal objen As EmpPREN) As String
        Try
            Return objDA.CancelRequest(objen)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
