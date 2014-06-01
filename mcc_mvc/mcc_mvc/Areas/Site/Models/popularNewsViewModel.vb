Imports MCC.Data
Imports MCC.Services

Public Class popularNewsViewModel

   Private _articlesrvr As IArticleService
   Private _categorysrvr As IArticleCategoryService

   Public Sub New()
      Me.New(New ArticleService, New ArticleCategoryService)
   End Sub

   Public Sub New(ByVal articlesrv As IArticleService, ByVal articlecategorysrvr As IArticleCategoryService)
      _articlesrvr = articlesrv
      _categorysrvr = articlecategorysrvr
      InitData()
   End Sub

   Private Sub InitData()
      _articles = New List(Of PopularArticle)

      _articles.Add(New PopularArticle(_articlesrvr.GetArticlesByCategoryName("Money", 0, 1).FirstOrDefault, "Money"))
      _articles.Add(New PopularArticle(_articlesrvr.GetArticlesByCategoryName("News", 0, 1).FirstOrDefault, "News"))
      _articles.Add(New PopularArticle(_articlesrvr.GetArticlesByCategoryName("Stories", 0, 1).FirstOrDefault, "Stories"))
      _articles.Add(New PopularArticle(_articlesrvr.GetArticlesByCategoryName("Rants", 0, 1).FirstOrDefault, "Rants"))

   End Sub

   Private _articles As IList(Of PopularArticle)
   Public Property Articles() As IList(Of PopularArticle)
      Get
         Return _articles
      End Get
      Set(ByVal value As IList(Of PopularArticle))
         _articles = value
      End Set
   End Property

End Class
