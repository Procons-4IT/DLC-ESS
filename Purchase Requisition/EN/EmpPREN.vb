Public Class EmpPREN
    Private _SAPCompany As SAPbobsCOM.Company
    Private _EmpId As String
    Private _EmpName As String
    Private _DeptCode As String
    Private _DeptName As String
    Private _DocNo As String
    Private _DocStatus As String
    Private _whs As String
    Private _OrdrPat As String
    Private _ItemCode As String
    Private _Itemdesc As String
    Private _ItemSpec As String
    Private _OrdrQty As String
    Private _OrdrUom As String
    Private _Barcode As String
    Private _SessionId As String
    Private _OrdrUomDesc As String
    Private _Code As String
    Private _DelQty As String
    Private _LineId As String
    Public Property SAPCompany() As SAPbobsCOM.Company
        Get
            Return _SAPCompany
        End Get
        Set(ByVal value As SAPbobsCOM.Company)
            _SAPCompany = value
        End Set
    End Property
    Public Property LineId() As String
        Get
            Return _LineId
        End Get
        Set(ByVal value As String)
            _LineId = value
        End Set
    End Property
    Public Property DelQty() As String
        Get
            Return _DelQty
        End Get
        Set(ByVal value As String)
            _DelQty = value
        End Set
    End Property
    Public Property Code() As String
        Get
            Return _Code
        End Get
        Set(ByVal value As String)
            _Code = value
        End Set
    End Property
    Public Property DocStatus() As String
        Get
            Return _DocStatus
        End Get
        Set(ByVal value As String)
            _DocStatus = value
        End Set
    End Property
    Public Property EmpId() As String
        Get
            Return _EmpId
        End Get
        Set(ByVal value As String)
            _EmpId = value
        End Set
    End Property
    Public Property EmpName() As String
        Get
            Return _EmpName
        End Get
        Set(ByVal value As String)
            _EmpName = value
        End Set
    End Property
    Public Property DeptCode() As String
        Get
            Return _DeptCode
        End Get
        Set(ByVal value As String)
            _DeptCode = value
        End Set
    End Property
    Public Property DeptName() As String
        Get
            Return _DeptName
        End Get
        Set(ByVal value As String)
            _DeptName = value
        End Set
    End Property
    Public Property DocNo() As String
        Get
            Return _DocNo
        End Get
        Set(ByVal value As String)
            _DocNo = value
        End Set
    End Property
    Public Property Defwhs() As String
        Get
            Return _whs
        End Get
        Set(ByVal value As String)
            _whs = value
        End Set
    End Property
    Public Property OrdrPatient() As String
        Get
            Return _OrdrPat
        End Get
        Set(ByVal value As String)
            _OrdrPat = value
        End Set
    End Property
    Public Property ItemCode() As String
        Get
            Return _ItemCode
        End Get
        Set(ByVal value As String)
            _ItemCode = value
        End Set
    End Property
    Public Property Itemdesc() As String
        Get
            Return _Itemdesc
        End Get
        Set(ByVal value As String)
            _Itemdesc = value
        End Set
    End Property
    Public Property ItemSpec() As String
        Get
            Return _ItemSpec
        End Get
        Set(ByVal value As String)
            _ItemSpec = value
        End Set
    End Property
    Public Property OrdrQty() As String
        Get
            Return _OrdrQty
        End Get
        Set(ByVal value As String)
            _OrdrQty = value
        End Set
    End Property
    Public Property OrdrUom() As String
        Get
            Return _OrdrUom
        End Get
        Set(ByVal value As String)
            _OrdrUom = value
        End Set
    End Property
    Public Property Barcode() As String
        Get
            Return _Barcode
        End Get
        Set(ByVal value As String)
            _Barcode = value
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
    Public Property OrdrUomDesc() As String
        Get
            Return _OrdrUomDesc
        End Get
        Set(ByVal value As String)
            _OrdrUomDesc = value
        End Set
    End Property
End Class
