Public Class PRApprovalEN
    Private _DocNo As String
    Private _EmpName As String
    Private _EmpId As String
    Private _UserCode As String
    Private _DocEntry As String
    Private _ItemCode As String
    Private _Itemdesc As String
    Private _ItemSpec As String
    Private _OrdrQty As String
    Private _OrdrUom As String
    Private _Barcode As String
    Private _SessionId As String
    Private _OrdrUomDesc As String
    Private _AppStatus As String
    Private _EmpUserId As String
    Private _DocMessage As String
    Private _HeadDocEntry As String
    Private _HeadLineId As String
    Private _SapCompany As SAPbobsCOM.Company
    Private _IntEmpInd As String
    Private _IntReqNo As String
    Private _HeaderType As String
    Private _HistoryType As String
    Private _Remarks As String
    Private _GoodsDocNo As String
    Private _DeptCode As String
    Private _LineStatus As String
    Private _DocLineId As String
    Private _WhsCode As String
    Public Property WhsCode() As String
        Get
            Return _WhsCode
        End Get
        Set(ByVal value As String)
            _WhsCode = value
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
    Public Property DocLineId() As String
        Get
            Return _DocLineId
        End Get
        Set(ByVal value As String)
            _DocLineId = value
        End Set
    End Property
    Public Property LineStatus() As String
        Get
            Return _LineStatus
        End Get
        Set(ByVal value As String)
            _LineStatus = value
        End Set
    End Property
    Private _Code As String
    Public Property Code() As String
        Get
            Return _Code
        End Get
        Set(ByVal value As String)
            _Code = value
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
    Public Property GoodsDocNo() As String
        Get
            Return _GoodsDocNo
        End Get
        Set(ByVal value As String)
            _GoodsDocNo = value
        End Set
    End Property
    Public Property Remarks() As String
        Get
            Return _Remarks
        End Get
        Set(ByVal value As String)
            _Remarks = value
        End Set
    End Property
    Public Property HeaderType() As String
        Get
            Return _HeaderType
        End Get
        Set(ByVal value As String)
            _HeaderType = value
        End Set
    End Property
    Public Property HistoryType() As String
        Get
            Return _HistoryType
        End Get
        Set(ByVal value As String)
            _HistoryType = value
        End Set
    End Property
    Public Property IntReqNo() As String
        Get
            Return _IntReqNo
        End Get
        Set(ByVal value As String)
            _IntReqNo = value
        End Set
    End Property
    Public Property InternalEmpInd() As String
        Get
            Return _IntEmpInd
        End Get
        Set(ByVal value As String)
            _IntEmpInd = value
        End Set
    End Property
    Public Property SapCompany() As SAPbobsCOM.Company
        Get
            Return _SapCompany
        End Get
        Set(ByVal value As SAPbobsCOM.Company)
            _SapCompany = value
        End Set
    End Property
    Public Property HeadDocEntry() As String
        Get
            Return _HeadDocEntry
        End Get
        Set(ByVal value As String)
            _HeadDocEntry = value
        End Set
    End Property
    Public Property HeadLineId() As String
        Get
            Return _HeadLineId
        End Get
        Set(ByVal value As String)
            _HeadLineId = value
        End Set
    End Property
    Public Property DocMessage() As String
        Get
            Return _DocMessage
        End Get
        Set(ByVal value As String)
            _DocMessage = value
        End Set
    End Property
    Public Property EmpUserId() As String
        Get
            Return _EmpUserId
        End Get
        Set(ByVal value As String)
            _EmpUserId = value
        End Set
    End Property
    Public Property AppStatus() As String
        Get
            Return _AppStatus
        End Get
        Set(ByVal value As String)
            _AppStatus = value
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
    Public Property DocNo() As String
        Get
            Return _DocNo
        End Get
        Set(ByVal value As String)
            _DocNo = value
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
    Public Property UserCode() As String
        Get
            Return _UserCode
        End Get
        Set(ByVal value As String)
            _UserCode = value
        End Set
    End Property
    Public Property DocEntry() As String
        Get
            Return _DocEntry
        End Get
        Set(ByVal value As String)
            _DocEntry = value
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


