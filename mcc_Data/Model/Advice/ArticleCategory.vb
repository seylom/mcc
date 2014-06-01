Public Class AdviceCategory
   Private _categoryId As Integer
   Public Property CategoryID() As Integer
      Get
         Return _categoryId
      End Get
      Set(ByVal value As Integer)
         _categoryId = value
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


   Private _importance As Integer
   Public Property Importance() As Integer
      Get
         Return _importance
      End Get
      Set(ByVal value As Integer)
         _importance = value
      End Set
   End Property


   Private _parentCategoryID As Integer
   Public Property ParentCategoryID() As Integer
      Get
         Return _parentCategoryID
      End Get
      Set(ByVal value As Integer)
         _parentCategoryID = value
      End Set
   End Property


   Private _categoryImageUrl As String
   Public Property ImageUrl() As String
      Get
         Return _categoryImageUrl
      End Get
      Set(ByVal value As String)
         _categoryImageUrl = value
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


   Private _slug As String
   Public Property Slug() As String
      Get
         Return _slug
      End Get
      Set(ByVal value As String)
         _slug = value
      End Set
   End Property

End Class
