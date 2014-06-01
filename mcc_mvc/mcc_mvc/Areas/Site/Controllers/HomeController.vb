
Imports MCC.Services
Imports MCC.Data

Namespace MCC.Areas.Site.Controllers
   Public Class HomeController
      Inherits ApplicationController

      Function Index() As ActionResult

         'Dim _articleService As IArticleService = New ArticleService()

         'Dim frontArticles As List(Of Article) = _articleService.GetFrontPageArticles(True, 0, 5, "ReleaseDate DESC")
         'Dim SpotArticles As List(Of Article) = _articleService.GetSpotlightArticles(True, "stories", 0, 1, "ReleaseDate DESC")
         'Dim feauredVideos As List(Of mcc_Video) = Videos.VideoRepository.GetVideos(True, 0, 3, "AddedDate DESC")

         'ViewData("FrontPageArticles") = frontArticles
         'ViewData("FeaturedArticles") = SpotArticles
         'ViewData("FeaturedVideos") = feauredVideos

         Dim _viewdata As HomepageData = New HomepageData()

         Return View(_viewdata)
      End Function


   End Class
End Namespace