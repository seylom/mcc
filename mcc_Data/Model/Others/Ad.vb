

Public Class Ad
    Private _adID As Integer
    Public Property AdID() As Integer
        Get
            Return _adID
        End Get
        Set(ByVal value As Integer)
            _adID = value
        End Set
    End Property

    Private _addedDate As DateTime
    Public Property AddedDate() As DateTime
        Get
            Return _addedDate
        End Get
        Set(ByVal value As DateTime)
            _addedDate = value
        End Set
    End Property


    Private _addedBy As String
    Public Property AddedBy() As String
        Get
            Return _addedBy
        End Get
        Set(ByVal value As String)
            _addedBy = value
        End Set
    End Property


    Private _Body As String
    Public Property Body() As String
        Get
            Return _Body
        End Get
        Set(ByVal value As String)
            _Body = value
        End Set
    End Property


    Private _approved As Boolean
    Public Property Approved() As Boolean
        Get
            Return _approved
        End Get
        Set(ByVal value As Boolean)
            _approved = value
        End Set
    End Property



    Private _description As String
    Public Property Description() As String
        Get
            Return _description
        End Get
        Set(ByVal value As String)
            _description = value
        End Set
    End Property



    Private _keywords As String
    Public Property keywords() As String
        Get
            Return _keywords
        End Get
        Set(ByVal value As String)
            _keywords = value
        End Set
    End Property



    Private _title As String
    Public Property Title() As String
        Get
            Return _title
        End Get
        Set(ByVal value As String)
            _title = value
        End Set
    End Property


    Private _type As Integer
    Public Property Type() As Integer
        Get
            Return _type
        End Get
        Set(ByVal value As Integer)
            _type = value
        End Set
    End Property

    Private _zoneId As Integer
    Public Property ZoneId() As Integer
        Get
            Return _zoneId
        End Get
        Set(ByVal value As Integer)
            _zoneId = value
        End Set
    End Property

    Private _advertizerId As Integer
    Public Property AdvertizerID() As Integer
        Get
            Return _advertizerId
        End Get
        Set(ByVal value As Integer)
            _advertizerId = value
        End Set
    End Property

    Private _Task As Integer
    Public Property Task() As Integer
        Get
            Return _Task
        End Get
        Set(ByVal value As Integer)
            _Task = value
        End Set
    End Property
End Class
