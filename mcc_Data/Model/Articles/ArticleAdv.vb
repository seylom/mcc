Public Class ArticleAdv   ' front page article



   Private _articleID As Integer
   Public Property ArticleID() As Integer
      Get
         Return _articleID
      End Get
      Set(ByVal value As Integer)
         _articleID = value
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

   Private _slug As String
   Public Property Slug() As String
      Get
         Return _slug
      End Get
      Set(ByVal value As String)
         _slug = value
      End Set
   End Property


   Private _abstract As String
   Public Property Abstract() As String
      Get
         Return _abstract
      End Get
      Set(ByVal value As String)
         If value.Length > 200 Then
            _abstract = value.Substring(0, 200) & "..."
         Else
            _abstract = value
         End If
      End Set
   End Property


   Private _imageNewsUrl As String
   Public Property ImageNewsUrl() As String
      Get
         Return _imageNewsUrl
      End Get
      Set(ByVal value As String)
         _imageNewsUrl = value
      End Set
   End Property


   Private _imageCreditUrl As String
   Public Property ImageCreditsUrl() As String
      Get
         Return _imageCreditUrl
      End Get
      Set(ByVal value As String)
         _imageCreditUrl = value
      End Set
   End Property

   Private _imageCreditsName As String
   Public Property ImageCreditsName() As String
      Get
         Return _imageCreditsName
      End Get
      Set(ByVal value As String)
         _imageCreditsName = value
      End Set
   End Property

    Property ImageID As Integer
    Property ReleaseDate As DateTime?

End Class

