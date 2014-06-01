Imports MCC.Services
Imports MCC.Data
Imports MCC.MvcJson.Filters

Namespace MCC.Areas.Admin.Controllers
   Public Class ArticleCategoryAdminController
      Inherits AdminController

      Private _articleCategoryService As IArticleCategoryService

      Public Sub New()
         Me.New(New ArticleCategoryService())
      End Sub

      Public Sub New(ByVal articleCategorySrvr As IArticleCategoryService)
         _articleCategoryService = articleCategorySrvr
      End Sub

      Function Index(ByVal page? As Integer, ByVal size? As Integer) As ActionResult
         Dim _viewdata As New AdminArticleCategoriesViewModel(If(page, 0), If(size, 30), _articleCategoryService)
         Return View(_viewdata)
      End Function

      '<AcceptVerbs(HttpVerbs.Post), JsonFilter(param:="ac", jsonDataType:=GetType(ArticleCategory))> _
      <AcceptVerbs(HttpVerbs.Post)> _
      Function SaveCategory(ByVal ac As ArticleCategory) As JsonResult
         If Request.IsAjaxRequest Then
            If ac Is Nothing Then
               Return Json(False)
            End If

            _articleCategoryService.SaveCategory(ac)
            Return Json(True)
         End If

         Return Json(True)
      End Function

      Function EditCategory(ByVal Id As Integer) As JsonResult
         If Request.IsAjaxRequest AndAlso Id > 0 Then
            Dim ac As ArticleCategory = _articleCategoryService.GetCategoryById(Id)
            If ac IsNot Nothing Then
               Return Json(ac, JsonRequestBehavior.AllowGet)
            End If
         End If
         Return Json(Nothing)
      End Function

      <AcceptVerbs(HttpVerbs.Post)> _
      Function DeleteCategories(ByVal Ids() As Integer) As ActionResult

         If Ids Is Nothing Then
            Return RedirectToAction("Index")
         End If

         If Request.IsAjaxRequest Then
            _articleCategoryService.DeleteCategories(Ids)
            Return Json(True)
         End If

         Return RedirectToAction("Index")
      End Function
   End Class
End Namespace