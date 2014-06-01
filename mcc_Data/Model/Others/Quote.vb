Public Class Quote

   Private _quoteID As Integer
   Public Property QuoteID() As Integer
      Get
         Return _quoteID
      End Get
      Set(ByVal value As Integer)
         _quoteID = value
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



   Private _author As String
   Public Property Author() As String
      Get
         Return _author
      End Get
      Set(ByVal value As String)
         _author = value
      End Set
   End Property


   Private _role As String
   Public Property Role() As String
      Get
         Return _role
      End Get
      Set(ByVal value As String)
         _role = value
      End Set
   End Property

   Private _body As String
   Public Property Body() As String
      Get
         Return _body
      End Get
      Set(ByVal value As String)
         _body = value
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



End Class
