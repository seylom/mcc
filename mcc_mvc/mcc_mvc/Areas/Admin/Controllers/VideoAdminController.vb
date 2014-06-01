Imports MCC.Services
Imports MCC.Data
Imports System.IO

Namespace MCC.Areas.Admin.Controllers
   Public Class VideoAdminController
      Inherits AdminController



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
         Dim _viewdata As New AdminVideosViewModel(If(page, 0), If(size, 30), _videoservice)
         Return View(_viewdata)
      End Function

      Function ShowCategories() As ActionResult
         Dim _viewdata As New AdminVideoCategoriesViewModel
         _viewdata.Categories = _videocategoryservice.GetCategories(0, 20)
         _viewdata.ParentCategories = _videocategoryservice.GetParentCategories.ToSelectList(Function(p) p.Title, Function(p) p.CategoryID, "- please select a parent category ")
         Return View(_viewdata)
      End Function


      Function EditVideo(ByVal Id As Integer) As ActionResult

         If Id <= 0 Then
            Return RedirectToAction("Index")
         End If

         Dim vd As Video = _videoservice.GetVideoById(Id)
         If vd Is Nothing Then
            Return RedirectToAction("Index")
         End If

         Dim vmodel As New AdminVideoViewModel()
         vmodel.Categories = _videocategoryservice.GetCategories()
         vmodel.CategoryIds = _videocategoryservice.GetCategoriesByVideoId(Id).Select(Function(p) p.CategoryID).ToList
         vd.FillViewModel(vmodel)

         Return View(vmodel)
      End Function

      Function UpdateVideoFile(ByVal Id As Integer) As ActionResult
         If Id <= 0 Then
            Return RedirectToAction("Index")
         End If

         Dim vd As Video = _videoservice.GetVideoById(Id)
         If vd Is Nothing Then
            Return RedirectToAction("Index")
         End If
         Dim updateFileModel As New AdminUpdateVideoViewModel()
         vd.FillViewModel(updateFileModel)
         Return View(updateFileModel)
      End Function

      <AcceptVerbs(HttpVerbs.Post)> _
      Function UpdateVideoFile(ByVal viewModel As AdminUpdateVideoViewModel) As ActionResult

         If viewModel.VideoID <= 0 Then
            viewModel.Messages.Add("Unable to find the video file to update")
            Return View(viewModel)
         End If

         If Request.Files.Count <= 0 Then
            viewModel.Messages.Add("No video file specified")
            Return View(viewModel)
         End If

         Dim hpf As HttpPostedFileBase = Request.Files(0)
         If hpf.ContentLength = 0 Then
            viewModel.Messages.Add("No video file specified, or video content is empty...")
            Return View(viewModel)
         End If

         ' upload file here!
         Dim status As FileUploadStatus = VideoHelper.UploadVideoFile(hpf, viewModel.Name)
         '
         Dim updateFile As New AdminUpdateVideoViewModel()
         updateFile.UploadResults = status.Message


         Return View("UpdateVideoFile", updateFile)
      End Function


      Function UploadVideoFile() As ActionResult
         Dim status As New FileUploadStatus
         Return View(status)
      End Function


      <AcceptVerbs(HttpVerbs.Post)> _
      Function UploadAddedFiles() As ActionResult

         If Request.Files.Count = 0 Then
            TempData("ErrorMessage") = "No video file specified"
            Return View("UploadVideoFile")
         End If

         Dim status As New FileUploadStatus With {.Success = True}
         For Each it As String In Request.Files
            Dim htp As HttpPostedFileBase = Request.Files(it)
            If htp.ContentLength = 0 Then
               Continue For
            End If

            Dim fi As New FileInfo(htp.FileName)

            ' upload file
            Dim uid As String = routines.GenerateName(New Random, 12)

            Dim vd As New Video()
            vd.Title = htp.FileName
            vd.Approved = False
            vd.Listed = True
            vd.Name = uid
            vd.VideoUrl = uid & "/" & uid & fi.Extension
            vd.Duration = 0


            Dim _locStatus As FileUploadStatus = VideoHelper.UploadVideoFile(htp, uid)
            status.Message.AddRange(_locStatus.Message)

            If status.Success Then
               _videoservice.InsertVideo(vd)
            End If
         Next

         Return View("UploadVideoFile", status)
      End Function
   End Class
End Namespace