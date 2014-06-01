Imports MCC.Services
Imports MCC.Data

Namespace MCC.Areas.Admin.Controllers
   <ValidateInput(False)> _
   Public Class ArticleAdminController
      Inherits AdminController

      Private _articleService As IArticleService
      Private _categoryService As IArticleCategoryService
      Private _adservice As IAdService
      Private _pollservice As IPollService
      Private _videoservice As IVideoService
      Private _articleCommentService As IArticleCommentService

      Public Sub New()
         Me.New(New ArticleService(), New ArticleCategoryService, New VideoService, New PollService, _
                        New AdService())

      End Sub

      Public Sub New(ByVal articlesrvr As IArticleService, ByVal articlecatsrvr As IArticleCategoryService, _
                     ByVal videosrvr As IVideoService, ByVal pollsrvr As IPollService, ByVal adsrvr As IAdService)
         _articleService = articlesrvr
         _categoryService = articlecatsrvr
         _videoservice = videosrvr
         _adservice = adsrvr
         _pollservice = pollsrvr
      End Sub


#Region "1. Articles"


      Function Index(ByVal page? As Integer, ByVal size? As Integer) As ActionResult
         Dim _viewData As New AdminArticleslistViewModel(If(page, 0), If(size, 30), _articleService)
         Return View(_viewData)
      End Function

      Function AddEditArticle(ByVal Id? As Integer) As ActionResult
         Dim _viewdata As New AddArticleViewModel(If(Id, 0), _articleService, _categoryService, _videoservice, _pollservice, _adservice)
         If Id IsNot Nothing Then
            Dim ar As Article = _articleService.GetArticleById(If(Id, 0))
            If ar IsNot Nothing Then
               ar.ArticleID = If(Id, 0)
               ar.FillViewModel(_viewdata)
            End If
         End If

         Return View(_viewdata)
      End Function

      <AcceptVerbs(HttpVerbs.Post), ValidateInput(False)> _
      Function AddEditArticle(ByVal dto As AddArticleViewModel) As ActionResult

         If dto Is Nothing Then
            Return View(dto)
         End If

         If Not ValidateArticle(dto) Then
            Return View(dto)
         End If

         Dim ar As New Article()
         dto.FillDTO(ar)

         If Not _articleService.SaveArticle(ar, dto.CategoryIds.ToArray, dto.AdIds.ToArray) Then
            Return View(dto)
         End If

         Return RedirectToAction("Index")
      End Function

      Function ReviewArticles(ByVal page? As Integer, ByVal size? As Integer) As ActionResult
         Dim _viewData As New AdminArticleslistViewModel(If(page, 0), If(size, 30), _articleService)
         Return View(_viewData)
      End Function


      Function PeekArticles(ByVal Id As Integer) As ActionResult
         Dim _viewdata As ArticleViewModel = New ArticleViewModel(Id)
         Return View(_viewdata)
      End Function


      <AcceptVerbs(HttpVerbs.Post)> _
      Function DeleteArticles(ByVal Ids() As Integer) As ActionResult

         If Ids Is Nothing Then
            Return RedirectToAction("Index")
         End If

         If Request.IsAjaxRequest Then
            _articleService.DeleteArticles(Ids)
            Return Json(True)
         End If

         Return RedirectToAction("Index")
      End Function

      '<AcceptVerbs(HttpVerbs.Post)> _
      Function UpdateStatus(ByVal id As Integer, ByVal status As Integer) As ActionResult

         If id <= 0 Then
            Return RedirectToAction("Index")
         End If

         Dim idx As Integer = CInt(status)
         If idx < 5 AndAlso idx >= 0 Then
            _articleService.UpdateArticleStatus(id, status)
         End If

         Return RedirectToAction("Index")
      End Function



      Function ValidateArticle(ByVal viewmodel As AddArticleViewModel) As Boolean
         Dim isValid As Boolean = True
         If String.IsNullOrWhiteSpace(viewmodel.Title) Then
            viewmodel.Messages.Add("Please add a title for this article")
            isValid = False
         End If
         If String.IsNullOrWhiteSpace(viewmodel.Abstract) Then
            viewmodel.Messages.Add("Please add an Abstract for this article")
            isValid = False
         End If
         If String.IsNullOrWhiteSpace(viewmodel.Body) Then
            viewmodel.Messages.Add("Please add a Body for this article")
            isValid = False
         End If
         If String.IsNullOrWhiteSpace(viewmodel.ImageNewsUrl) Then
            viewmodel.Messages.Add("Please add an ImageNewsUrl for this article")
            isValid = False
         End If
         If String.IsNullOrWhiteSpace(viewmodel.ImageIconUrl) Then
            viewmodel.Messages.Add("Please add an ImageIconUrl for this article")
            isValid = False
         End If

         Return isValid
      End Function


      Function UpdateArticleAuthor(ByVal Id As Integer) As ActionResult

         If Id <= 0 Then
            Return RedirectToAction("Index")
         End If

         Dim ar As Article = _articleService.GetArticleById(Id)
         If ar Is Nothing Then
            Return RedirectToAction("Index")
         End If

         Dim _viewdata As New AdminArticleAuthorUpdateViewModel
         _viewdata.ArticleID = ar.ArticleID
         _viewdata.AddedBy = ar.AddedBy
         _viewdata.Authors = Roles.GetUsersInRole("Editors").ToList
         Return View(_viewdata)

      End Function

#End Region

#Region "2. Categories"

      Function ShowCategories() As ActionResult
         Dim _viewData As New AdminArticleCategoriesViewModel()
         Return View(_viewData)
      End Function

#End Region

#Region "3. Comments"

      Function ArticleComments(ByVal id? As Integer, ByVal page? As Integer, ByVal size? As Integer) As ActionResult
         Dim _viewData As New AdminArticleCommentsViewModel(_articleService, If(id, 0), If(page, 0), If(size, 30))
         Return View(_viewData)
      End Function

      <AcceptVerbs(HttpVerbs.Post)>
      Function DeleteComments(ByVal ids() As Integer) As JsonResult

         If ids IsNot Nothing AndAlso ids.Length > 0 Then
            _articleCommentService = New ArticleCommentService
            _articleCommentService.DeleteArticleComments(ids)
         Else
            Return Json(False)
         End If

         Return Json(True)
      End Function
#End Region


   End Class
End Namespace