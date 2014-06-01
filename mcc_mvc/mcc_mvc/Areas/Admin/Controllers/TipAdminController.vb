Imports MCC.Services
Imports MCC.Data

Namespace MCC.Areas.Admin.Controllers
   Public Class TipAdminController
      Inherits AdminController

      Private _adviceservice As IAdviceService
      Private _advicecategoryservice As IAdviceCategoryService

      Public Sub New()
         Me.New(New AdviceService, New AdviceCategoryService)
      End Sub

      Public Sub New(ByVal advicesrvr As IAdviceService, ByVal advicecatservr As IAdviceCategoryService)
         _adviceservice = advicesrvr
         _advicecategoryservice = advicecatservr
      End Sub

      Function Index(ByVal page? As Integer, ByVal size? As Integer) As ActionResult
         Dim _viewdata As New AdminTipsViewModel(If(page, 0), If(size, 30), _adviceservice)
         Return View(_viewdata)
      End Function

      Function ShowCategories() As ActionResult
         Dim _viewdata As New AdminTipCategoriesViewModel()


         Return View(_viewdata)
      End Function


      Function CreateTip() As ActionResult
         Dim _viewdata As New AdminTipViewModel(_adviceservice, _advicecategoryservice)
         Return View(_viewdata)
      End Function

      Function EditTip(ByVal Id As Integer) As ActionResult

         If Id <= 0 Then
            Return RedirectToAction("index")
         End If

         Dim adv As Advice = _adviceservice.GetAdviceById(Id)
         Dim _viewdata As New AdminTipViewModel(_adviceservice, _advicecategoryservice)
         adv.FillViewModel(_viewdata)
         Return View(_viewdata)
      End Function

      <AcceptVerbs(HttpVerbs.Post)> _
      Function EditTip(ByVal tip As AdminTipViewModel) As ActionResult

         If tip Is Nothing Then
            Return RedirectToAction("Index")
         End If

         Dim adv As New Advice
         tip.FillDTO(adv)

         Dim rv As Boolean = _adviceservice.SaveAdvice(adv)
         If Not rv Then
            TempData("ErrorMessage") = "Unable to save the advice - please check for errors..."
            Return View(tip)
         End If

         Return RedirectToAction("Index")
      End Function

      <AcceptVerbs(HttpVerbs.Post)> _
      Function DeleteTip(ByVal Id As Integer) As ActionResult

         If Id > 0 Then
            _adviceservice.DeleteAdvice(Id)
         End If
         Return RedirectToAction("Index")
      End Function
   End Class
End Namespace