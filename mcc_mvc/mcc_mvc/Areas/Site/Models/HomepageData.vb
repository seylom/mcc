Imports MCC.Services
Imports MCC.Data
Public Class HomepageData
   Inherits baseViewModel

   Private _articleService As IArticleService
   Private _videoService As IVideoService

   Public Sub New()
      Me.New(New ArticleService(), New VideoService())
   End Sub

   Public Sub New(ByVal _articlesvr As IArticleService, ByVal _videosrvr As IVideoService)
      _articleService = _articlesvr
      _videoService = _videosrvr
      InitArticleList()
   End Sub

   Private Sub InitArticleList()
      _MainArticles = _articleService.GetFrontPageArticles(True, "stories,news", 0, 5, "ReleaseDate DESC")
      _spotlightArticles = _articleService.GetSpotlightArticles(True, "stories", 0, 1, "ReleaseDate DESC")
      _FeaturedVideos = _videoService.GetVideos(True, 0, 3, "AddedDate DESC")

      For Each it As ArticleAdv In _MainArticles
         If it.Abstract.Length > 200 Then
            it.Abstract = it.Abstract.Substring(0, 200) & "..."
         End If
      Next

      For Each it As Article In _spotlightArticles
         If it.Abstract.Length > 200 Then
            it.Abstract = it.Abstract.Substring(0, 200) & "..."
         End If
      Next

      _questions = New vmQuestionSimpleList()
      _popularNews = New popularNewsViewModel()

   End Sub

   Private _MainArticles As List(Of ArticleAdv)
   Public Property MainArticles() As List(Of ArticleAdv)
      Get
         Return _MainArticles
      End Get
      Set(ByVal value As List(Of ArticleAdv))
         _MainArticles = value
      End Set
   End Property


   Private _spotlightArticles As List(Of Article)
   Public Property SpotlightArticles() As List(Of Article)
      Get
         Return _spotlightArticles
      End Get
      Set(ByVal value As List(Of Article))
         _spotlightArticles = value
      End Set
   End Property

   Private _FeaturedVideos As List(Of Video)
   Public Property FeaturedVideos() As List(Of Video)
      Get
         Return _FeaturedVideos
      End Get
      Set(ByVal value As List(Of Video))
         _FeaturedVideos = value
      End Set
   End Property


   Private _questions As vmQuestionSimpleList
   Public Property questions() As vmQuestionSimpleList
      Get
         Return _questions
      End Get
      Set(ByVal value As vmQuestionSimpleList)
         _questions = value
      End Set
   End Property

   Private _popularNews As popularNewsViewModel
   Public Property PopularNews() As popularNewsViewModel
      Get
         Return _popularNews
      End Get
      Set(ByVal value As popularNewsViewModel)
         _popularNews = value
      End Set
   End Property

End Class
