Public Class WikiPage



   Private _wikiPageID As Integer
   Public Property WikiPageID() As Integer
      Get
         Return _wikiPageID
      End Get
      Set(ByVal value As Integer)
         _wikiPageID = value
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



   Private _addedby As String
   Public Property AddedBy() As String
      Get
         Return _addedby
      End Get
      Set(ByVal value As String)
         _addedby = value
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



   Private _description As String
   Public Property Description() As String
      Get
         Return _description
      End Get
      Set(ByVal value As String)
         _description = value
      End Set
   End Property



   Private _body As String
   Public Property body() As String
      Get
         Return _body
      End Get
      Set(ByVal value As String)
         _body = value
      End Set
   End Property



   Private _lastEditionDate As DateTime
   Public Property LastEditionDate() As DateTime
      Get
         Return _lastEditionDate
      End Get
      Set(ByVal value As DateTime)
         _lastEditionDate = value
      End Set
   End Property



   Private _lastEditedBy As String
   Public Property LastEditedBy() As String
      Get
         Return _lastEditedBy
      End Get
      Set(ByVal value As String)
         _lastEditedBy = value
      End Set
   End Property


   Private _parentWikiID As String
   Public Property ParentWikiPageID() As String
      Get
         Return _parentWikiID
      End Get
      Set(ByVal value As String)
         _parentWikiID = value
      End Set
   End Property


   Private _tags As String
   Public Property Tags() As String
      Get
         Return _tags
      End Get
      Set(ByVal value As String)
         _tags = value
      End Set
   End Property


   Private _pageName As String
   Public Property PageName() As String
      Get
         Return _pageName
      End Get
      Set(ByVal value As String)
         _pageName = value
      End Set
   End Property

End Class
