Imports Webdiyer.WebControls.Mvc
Imports MCC.Data
Imports MCC.Services

Public Class ArticlesFormViewModel
   Inherits baseViewModel

   Private _articlesService As IArticleService
   Private _articleCategoryService As IArticleCategoryService

#Region ".Ctor"

   Public Sub New()
      Me.New(New ArticleService(), New ArticleCategoryService(), "", 0, 30)
   End Sub

   Public Sub New(ByVal pageIndex As Integer, ByVal pageSize As Integer)
      Me.New(New ArticleService(), New ArticleCategoryService(), "", pageIndex, pageSize)
   End Sub

   Public Sub New(ByVal pageIndex As Integer, ByVal pageSize As Integer, ByVal author As String)
      Me.New(New ArticleService(), New ArticleCategoryService(), author, pageIndex, pageSize)
   End Sub

   Public Sub New(ByVal _articleSrvc As IArticleService, ByVal _articleCategorySrvr As IArticleCategoryService, ByVal author As String, ByVal pageIndex As Integer, ByVal pageSize As Integer)
      _articlesService = _articleSrvc
      _articleCategoryService = _articleCategorySrvr
      _author = author
      _pageIndex = pageIndex
      _pageSize = pageSize
      InitDefaultSettings()

      _sideAd = (New AdService).GetRandomAdByType(adType.BannerVertical)
   End Sub

#End Region

   Private Sub InitDefaultSettings()

      If Not String.IsNullOrEmpty(_author) AndAlso UserExistsInMembership(_author) Then
         _articles = _articlesService.GetArticlesByAuthor(_author, PageIndex, PageSize, "ReleaseDate DESC")
         _recentArticles = _articlesService.GetLatestArticles(4)
         _Categories = _articleCategoryService.GetParentCategories()
         _totalArticlesCount = _articlesService.GetArticleCount()
      Else
         _articles = _articlesService.GetArticles(True, PageIndex, PageSize, "ReleaseDate DESC")
         _recentArticles = _articlesService.GetLatestArticles(4)
         _Categories = _articleCategoryService.GetParentCategories()
         _totalArticlesCount = _articlesService.GetArticleCountByAuthor(_author)
      End If
   
   End Sub

   Private _articles As PagedList(Of Article)
   Public Property Articles() As PagedList(Of Article)
      Get
         Return _articles
      End Get
      Set(ByVal value As PagedList(Of Article))
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


   Private _recentArticles As List(Of Article)
   Public Property RecentArticles() As List(Of Article)
      Get
         Return _recentArticles
      End Get
      Set(ByVal value As List(Of Article))
         _recentArticles = value
      End Set
   End Property


    Private _totalArticlesCount As Integer
    Public Property TotalArticlesCount() As Integer
        Get
            Return _totalArticlesCount
        End Get
        Set(ByVal value As Integer)
            _totalArticlesCount = value
        End Set
    End Property


   Private _pageIndex As Integer = 0
   Public Property PageIndex() As Integer
      Get
         Return _pageIndex
      End Get
      Set(ByVal value As Integer)
         _pageIndex = value
      End Set
   End Property


   Private _pageSize As Integer = 30
   Public Property PageSize() As Integer
      Get
         Return _pageSize
      End Get
      Set(ByVal value As Integer)
         _pageSize = value
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
