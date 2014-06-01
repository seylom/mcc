Imports MCC.Services
Imports MCC.Data

Namespace MCC.Areas.Site.Controllers
   Public Class VideoController
      Inherits ApplicationController


      Private _videoservice As IVideoService
      Private _videocategoryservice As IVideoCategoryService
      Public Sub New()
         Me.New(New VideoService, New VideoCategoryService)
      End Sub

      Public Sub New(ByVal videosrvr As IVideoService, ByVal videocatsrvr As IVideoCategoryService)
         _videoservice = videosrvr
         _videocategoryservice = videocatsrvr
      End Sub

      Function Index(ByVal page? As Integer, ByVal size? As Integer) As ActionResult
         Dim _viewdata As New videosViewData(_videoservice, _videocategoryservice, If(page, 0), If(size, 8))
         Return View(_viewdata)
      End Function


      Function ShowVideo(ByVal id As Integer, ByVal slug As String) As ActionResult
         If id <= 0 Then
            Return RedirectToAction("Index")
         End If
         Dim _viewdata As New VideoViewModel(id, _videoservice, _videocategoryservice)
         Return View(_viewdata)
      End Function

      Function GetVideoById(ByVal id As Integer) As JsonResult

         If id <= 0 Then
            Return Json(Nothing, JsonRequestBehavior.AllowGet)
         End If

         Dim vd As Video = _videoservice.GetVideoById(id)
         If vd Is Nothing Then
            Return Json(Nothing, JsonRequestBehavior.AllowGet)
         End If

         Return Json(vd, JsonRequestBehavior.AllowGet)
      End Function
   End Class
End Namespace