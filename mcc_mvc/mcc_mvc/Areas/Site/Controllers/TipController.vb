Imports MCC.Services
Imports MCC.Data

Namespace MCC.Areas.Site.Controllers
   Public Class TipController
      Inherits ApplicationController

      Private _tipservice As IAdviceService
      Private _tipcategoryservice As IAdviceCategoryService

      Public Sub New()
         Me.New(New AdviceService, New AdviceCategoryService)
      End Sub

      Public Sub New(ByVal tipserv As IAdviceService, ByVal tipcategorysrvr As IAdviceCategoryService)
         _tipservice = tipserv
         _tipcategoryservice = tipcategorysrvr
      End Sub

      Function Index(ByVal page? As Integer) As ActionResult
         Dim _viewdata As New TipsViewData(_tipservice, _tipcategoryservice, If(page, 0))
         Return View(_viewdata)
      End Function


      Function ShowTip() As ActionResult

         Return View()
      End Function

      <AcceptVerbs(HttpVerbs.Post)> _
      Function Vote(ByVal Id As Integer, ByVal value As Integer) As JsonResult
         If Id <= 0 Then
            Return Json(Nothing)
         End If

         Dim obj As Object = Nothing
         If value = 1 Then
            obj = _tipservice.VoteUpAdvice(Id)
         Else
            obj = _tipservice.VoteDownAdvice(Id)
         End If

         Return Json(obj)
      End Function


      Function GetVoteValues(ByVal Id As Integer) As JsonResult

         If Id <= 0 Then
            Return Json(Nothing)
         End If

         Dim tip As Advice = _tipservice.GetAdviceById(Id)
         If tip Is Nothing Then
            Return Json(Nothing)
         End If

         Return Json(New With {.up = tip.VoteUp, .down = tip.VoteDown})
      End Function
   End Class
End Namespace