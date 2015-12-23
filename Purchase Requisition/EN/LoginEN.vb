Public Class LoginEN
    Private _Uid As String
    Private _Pwd As String
    Private _SessionId As String
    Public Property Uid() As String
        Get
            Return _Uid
        End Get
        Set(ByVal value As String)
            _Uid = value
        End Set
    End Property
    Public Property Pwd() As String
        Get
            Return _Pwd
        End Get
        Set(ByVal value As String)
            _Pwd = value
        End Set
    End Property
    Public Property SessionId() As String
        Get
            Return _SessionId
        End Get
        Set(ByVal value As String)
            _SessionId = value
        End Set
    End Property
End Class
