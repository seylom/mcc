Public Class AdminQuoteViewModel
   Inherits baseViewModel

   Private _quoteId As Integer
   Public Property QuoteId() As Integer
      Get
         Return _quoteId
      End Get
      Set(ByVal value As Integer)
         _quoteId = value
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

   Private _addedBy As String
   Public Property AddedBy() As String
      Get
         Return _addedBy
      End Get
      Set(ByVal value As String)
         _addedBy = value
      End Set
   End Property


End Class
