Public Class PopularArticle


   Public Sub New()

   End Sub

   Public Sub New(ByVal art As Article, ByVal categoryName As String)
      If art IsNot Nothing Then
         _category = categoryName
         _title = art.Title
         _imageUrl = art.ImageNewsUrl
         _addedBy = art.AddedBy
         _id = art.ArticleID
         _slug = art.Slug
         _abstract = If(art.Abstract.Length > 200, art.Abstract.Substring(0, 200) & "...", art.Abstract)
      End If
   End Sub

   Private _category As String
   Public Property Category() As String
      Get
         Return _category
      End Get
      Set(ByVal value As String)
         _category = value
      End Set
   End Property

   Private _id As Integer
   Public Property ArticleId() As Integer
      Get
         Return _id
      End Get
      Set(ByVal value As Integer)
         _id = value
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


   Private _title As String
   Public Property Title() As String
      Get
         Return _title
      End Get
      Set(ByVal value As String)
         _title = value
      End Set
   End Property

   Private _imageUrl As String
   Public Property ImageUrl() As String
      Get
         Return _imageUrl
      End Get
      Set(ByVal value As String)
         _imageUrl = value
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


   Private _abstract As String
   Public Property Abstract() As String
      Get
         Return _abstract
      End Get
      Set(ByVal value As String)
         _abstract = value
      End Set
   End Property

End Class
