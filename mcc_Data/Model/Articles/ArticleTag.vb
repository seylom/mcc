Public Class ArticleTag

   Private _tagId As Integer
   Public Property TagID() As Integer
      Get
         Return _tagId
      End Get
      Set(ByVal value As Integer)
         _tagId = value
      End Set
   End Property


   Private _name As String
   Public Property Name() As String
      Get
         Return _name
      End Get
      Set(ByVal value As String)
         _name = value
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
