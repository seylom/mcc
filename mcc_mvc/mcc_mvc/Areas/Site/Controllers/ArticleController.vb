
Imports MCC.Services
Imports MCC.Data

Namespace MCC.Areas.Site.Controllers
   Public Class ArticleController
      Inherits ApplicationController

      Private _articleService As IArticleService
      Private _articleCategoryService As IArticleCategoryService
      Private _ArticleCommentService As IArticleCommentService
      Private _AdService As IAdService
      Private _pollService As IPollService
      Private _pollOptionService As IPollOptionService

      Public Sub New()
         Me.New(New ArticleService(), New ArticleCategoryService(), New ArticleCommentService(), New AdService(), _
                New PollService(), New PollOptionService())
      End Sub

      Public Sub New(ByVal _articlesvr As IArticleService, ByVal _articleCategorySvr As IArticleCategoryService, _
                     ByVal _articleCommentsrvr As IArticleCommentService, ByVal _adsrvr As IAdService, ByVal _pollSrvr As IPollService, _
                     ByVal _pollOptionsrvr As IPollOptionService)
         _articleService = _articlesvr
         _articleCategoryService = _articleCategorySvr
         _ArticleCommentService = _articleCommentsrvr
         _AdService = _adsrvr
         _pollService = _pollSrvr
         _pollOptionService = _pollOptionsrvr
      End Sub

      Private _canIncrementViewCount As Boolean = True

      Function Index(ByVal page? As Integer, ByVal size? As Integer) As ActionResult
         Dim _viewdata As ArticlesFormViewModel = New ArticlesFormViewModel(If(page, 0), If(size, 10))
         _viewdata.PageTitle = "Articles"
         Return View(_viewdata)
      End Function

      Function ShowArticle(ByVal Id As Integer, ByVal slug As String, ByVal page? As Integer, ByVal size? As Integer) As ActionResult

         If Id <= 0 Then
            Return RedirectToAction("Index")
         End If

         Dim _viewdata As ArticleViewModel = New ArticleViewModel(Id, _articleService, _articleCategoryService, _AdService, _
                                                                _pollService, _pollOptionService, If(page, 0), If(size, 20))
         If _viewdata Is Nothing Then
            Return RedirectToAction("Index")
         End If

         If _viewdata.Article Is Nothing Then
            Return RedirectToAction("Index")
         End If

         _viewdata.PageTitle = routines.Encode(_viewdata.Article.Title)
         _viewdata.MetaDescription = routines.Encode(_viewdata.Article.Title)
         _viewdata.MetaKeywords = String.Join(",", _viewdata.ArticleTags.ToArray)

         'ViewData("Comments") = New PartialRequest(New With {.controller = "article", .action = "ArticleComments", _
         '                                                    .id = Id, .slug = slug})

         If _viewdata.Article.OnlyForMembers Then
            Return RedirectToAction("/accesdenied")
         End If

         If _canIncrementViewCount Then
            _articleService.IncrementViewCount(_viewdata.Article.ArticleID)
         End If

         Return View("ViewArticle", _viewdata)
      End Function


      Function ViewArticle(ByVal slug As String) As ActionResult
         Dim art As Article = _articleService.GetArticleBySlug(slug)
         If art Is Nothing Then
            Return RedirectToAction("Index")
         End If

         Dim _viewdata As ArticleViewModel = New ArticleViewModel(art.ArticleID, _articleService, _articleCategoryService, _AdService, _
                                                                _pollService, _pollOptionService)
         If _viewdata Is Nothing Then
            Return RedirectToAction("Index")
         End If

         If _viewdata.Article Is Nothing Then
            Return RedirectToAction("Index")
         End If

         If _viewdata.Article Is Nothing Then
            Return RedirectToAction("Index")
         End If

         _viewdata.PageTitle = routines.Encode(_viewdata.Article.Title)
         _viewdata.MetaDescription = routines.Encode(_viewdata.Article.Title)
         _viewdata.MetaKeywords = String.Join(",", _viewdata.ArticleTags.ToArray)

         If _viewdata.Article.OnlyForMembers Then
            Return RedirectToAction("/accesdenied")
         End If

         If _canIncrementViewCount Then
            _articleService.IncrementViewCount(_viewdata.Article.ArticleID)
         End If

         Return View(_viewdata)
      End Function


      Function ArticleCategories() As ActionResult
         Dim _viewdata As List(Of ArticleCategory) = _articleCategoryService.GetCategories(0, 20)

         Return View(_viewdata)
      End Function


      Function ArticlesByCategory(ByVal slug As String, ByVal page? As Integer, ByVal size? As Integer) As ActionResult
         Dim _viewdata As ArticleByCategoryViewModel = New ArticleByCategoryViewModel(slug, _articleService, _articleCategoryService, _
                                                                                      If(page, 0), If(size, 10))
         If _viewdata Is Nothing Then
            Return RedirectToAction("Index")
         End If

         If _viewdata.Category Is Nothing Then
            Return RedirectToAction("Index")
         End If

         _viewdata.PageTitle = "Articles Topics"
         Return View(_viewdata)
      End Function


      'Function ArticleComments(ByVal Id As Integer, ByVal slug As String) As ActionResult

      '   Dim _articleservice As IArticleService = New ArticleService()
      '   'Dim _article As Article = _articleservice.GetArticleBySlug(slug)

      '   Dim _viewdata As ArticleCommentsViewModel = New ArticleCommentsViewModel(Id)

      '   If _viewdata.MainArticle Is Nothing Then
      '      RedirectToRoute("ShowArticle", New With {.id = Id, .slug = slug})
      '   End If

      '   'Dim _viewdata As List(Of ArticleComment) = _ArticleCommentService.GetArticleComments(_article.ArticleID, 0, 50, "AddedDate")

      '   If _viewdata Is Nothing Then
      '      RedirectToRoute("ShowArticle", New With {.id = Id, .slug = slug})
      '   End If

      '   'ViewData("article") = _article

      '   Return View(_viewdata)
      'End Function


      'Function PostComment() As ActionResult
      '   Return View()
      'End Function


      <Authorize(), AcceptVerbs(HttpVerbs.Post)> _
      Function PostComment(ByVal commentBody As String, ByVal articleId As String) As ActionResult

         Dim _articleservice As IArticleService = New ArticleService()

         If Not User.Identity.IsAuthenticated Then
            Return Redirect("~/accessdenied")
         End If

         Dim ar As Article = _articleservice.GetArticleById(articleId)

         If ar Is Nothing Then
            Return RedirectToAction("Index")
         End If

         If Not ar.CommentsEnabled Then
            Return RedirectToAction("ShowArticle", New With {.Id = ar.ArticleID, .slug = ar.Slug})
         End If

         If ValidateComment(commentBody) Then
            _ArticleCommentService.InsertArticleComment(New ArticleComment With _
                                                        {.ArticleID = ar.ArticleID, _
                                                         .Body = commentBody, _
                                                         .AddedDate = DateTime.Now})
         End If

         Return RedirectToAction("ShowArticle", New With {.Id = ar.ArticleID, .slug = ar.Slug})
      End Function


      Private Function ValidateComment(ByVal commentBody As String) As Boolean

         If String.IsNullOrEmpty(commentBody) Then
            TempData("ErrorMessage") = "Please write your comments"
         End If

         If commentBody.Length > 250 Or commentBody.Length < 1 Then
            TempData("ErrorMessage") = "Please enter between 1 and 250 characters"
         End If



         Return ModelState.IsValid


      End Function

      Function Print(ByVal id As Integer) As ActionResult
         If id <= 0 Then
            Return RedirectToAction("Index")
         End If

         Dim uqsrvr As New ArticleService()
         Dim vda As Article = uqsrvr.GetArticleById(id)

         If vda Is Nothing Then
            Return RedirectToAction("Index")
         End If

         Return View(vda)
      End Function

      <AcceptVerbs(HttpVerbs.Post)> _
      Function RateArticle(ByVal Id As Integer, ByVal rating As Integer) As JsonResult


         If Request.IsAjaxRequest() Then
            If Id <= 0 Then
               Return Json(False)
            End If

            _articleService.RateArticle(Id, rating)

            Return Json(True)
         End If

         Return Json(False)
      End Function
   End Class
End Namespace