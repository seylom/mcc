Imports Webdiyer.WebControls.Mvc
Imports MCC.Data
Imports MCC.Services
Public Class ArticleByCategoryViewModel
   Inherits baseViewModel

   Private _articleService As IArticleService
   Private _articleCategoryService As IArticleCategoryService

    Public Sub New(ByVal slug As String)
        Me.New(slug, New ArticleService(), New ArticleCategoryService(), 0, 10)
    End Sub

   Public Sub New(ByVal slug As String, ByVal _articleSrvc As IArticleService, ByVal _articleCategorySrvr As IArticleCategoryService, ByVal pageIndex As Integer, ByVal pageSize As Integer)
      _articleService = _articleSrvc
      _articleCategoryService = _articleCategorySrvr
      InitDefaultSettings(slug, pageIndex, pageSize)
      _sideAd = (New AdService).GetRandomAdByType(adType.BannerVertical)
   End Sub

   Private Sub InitDefaultSettings(ByVal slug As String, ByVal pageindex As Integer, ByVal pagesize As Integer)
      _Category = _articleCategoryService.GetCategoryBySlug(slug)
      _articles = _articleService.GetArticlesByCategoryName(True, slug, pageindex, pagesize, "ReleaseDate DESC")
      _recentArticles = _articleService.GetLatestArticles(4)
      _Categories = _articleCategoryService.GetParentCategories()
   End Sub

   Private _articles As pagedlist(Of Article)
   Public Property Articles() As pagedlist(Of Article)
      Get
         Return _articles
      End Get
      Set(ByVal value As pagedlist(Of Article))
         _articles = value
      End Set
   End Property

   Private _Categories As List(Of ArticleCategory)
   Public Property Categories() As List(Of ArticleCategory)
      Get
         Return _Categories
      End Get
      Set(ByVal value As List(Of ArticleCategory))
         _Categories = value
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


   Private _recentArticles As List(Of Article)
   Public Property RecentArticles() As List(Of Article)
      Get
         Return _recentArticles
      End Get
      Set(ByVal value As List(Of Article))
         _recentArticles = value
      End Set
   End Property


   Private _Category As ArticleCategory
   Public Property Category() As ArticleCategory
      Get
         Return _Category
      End Get
      Set(ByVal value As ArticleCategory)
         _Category = value
      End Set
   End Property

   Private _sideAd As Ad
   Public Property SideAd() As Ad
      Get
         Return _sideAd
      End Get
      Set(ByVal value As Ad)
         _sideAd = value
      End Set
   End Property

End Class
