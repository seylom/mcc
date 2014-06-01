Public Class AdminArticleAuthorUpdateViewModel
   Inherits baseViewModel


   Private _articleId As Integer
   Public Property ArticleID() As Integer
      Get
         Return _articleId
      End Get
      Set(ByVal value As Integer)
         _articleId = value
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


   Private _authors As List(Of String)
   Public Property Authors() As List(Of String)
      Get
         Return _authors
      End Get
      Set(ByVal value As List(Of String))
         _authors = value
      End Set
   End Property


End Class
