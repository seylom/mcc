Imports MCC.Services
Imports MCC.Data
Imports MvcSiteMap.Core


Public Enum Mode As Integer
   List = 0
   Tile = 1
End Enum

Namespace MCC.Areas.Admin.Controllers
Public Class ImageAdminController
   Inherits AdminController

   Private _imageservice As IImageService
   Private _imageTagService As IImageTagService

   Public Sub New()
      Me.New(New ImageService(), New ImageTagService)
   End Sub

   Public Sub New(ByVal imagesrvr As IImageService, ByVal imagetagsrvr As IImageTagService)
      _imageservice = imagesrvr
      _imageTagService = imagetagsrvr
   End Sub

   Function Index(ByVal mode? As Integer, ByVal page? As Integer, ByVal size? As Integer) As ActionResult
      Dim md As Integer = If(mode, 0)
      Dim _viewdata As New imageListViewModel(CType(md, Mode))
      _viewdata.Images = _imageservice.GetImages(If(page, 0), If(size, 30))
      Return View(_viewdata)
   End Function

    Public Function ShowImage(ByVal Id As Integer) As ActionResult

        If Id <= 0 Then
            Return RedirectToAction("Index")
        End If

        Dim img As SimpleImage = _imageservice.GetImageById(Id)
        If img Is Nothing Then
            Return RedirectToAction("Index")
        End If

        Dim vm As New AdminImageViewModel()
        img.FillViewModel(vm)
        Return View(vm)

    End Function

   <AcceptVerbs(HttpVerbs.Post)> _
   Public Function EditImage(ByVal Id As Integer) As ActionResult
      If Request.IsAjaxRequest Then
         Dim img As SimpleImage = _imageservice.GetImageById(Id)
         img.ImageUrl = Configs.Paths.CdnRoot & Configs.Paths.CdnImages & img.ImageUrl
         Return Json(img)
      End If
      Return Json(Nothing)
   End Function

   <AcceptVerbs(HttpVerbs.Post)> _
   Public Function SaveImage(ByVal image As SimpleImage) As JsonResult
      If Request.IsAjaxRequest Then
         _imageservice.UpdateImage(image)
         Return Json(True)
      End If
      Return Json(False)
   End Function

   <AcceptVerbs(HttpVerbs.Post)> _
   Function DeleteImage(ByVal Id As Integer) As ActionResult
      If Id <= 0 Then
         Return RedirectToAction("Index")
      End If

      _imageservice.DeleteImage(Id)

      Return RedirectToAction("Index")
   End Function

   <AcceptVerbs(HttpVerbs.Post)> _
  Function DeleteImages(ByVal Ids() As Integer) As JsonResult
      If Request.IsAjaxRequest Then
         If Ids Is Nothing Then
            Return Json(False)
         End If

         _imageservice.DeleteImages(Ids)
      End If

      Return Json(True)
   End Function


   Function CreateThumbnails(ByVal id As Integer) As ActionResult

      If id <= 0 Then
         Return RedirectToAction("Index")
      End If

      Dim spi As SimpleImage = _imageservice.GetImageById(id)

      If spi Is Nothing Then
         Return RedirectToAction("Index")
      End If

      Dim vm As New CreateThumbsViewModel()
      spi.FillViewModel(vm)

      Return View(vm)
   End Function

   <AcceptVerbs(HttpVerbs.Post)> _
   Function CreateThumbnails(ByVal vm As CreateThumbsViewModel) As ActionResult
      If vm.ImageID <= 0 Then
         Return RedirectToAction("Index")
      End If

      Dim spi As SimpleImage = _imageservice.GetImageById(vm.ImageID)
      If spi IsNot Nothing Then
         spi.FillViewModel(vm)
      End If
      Dim result As Boolean = CreateThumbnailsFromImages(vm)

      Return RedirectToAction("Index")
   End Function


   Function UploadImages() As ActionResult
      Dim _viewdata As New AdminUploadImagesViewModel()
      Return View(_viewdata)
   End Function

   <AcceptVerbs(HttpVerbs.Post)> _
   Function UploadAddedFiles(ByVal vm As AdminUploadImagesViewModel) As ActionResult

      If Request.Files.Count = 0 Then
         Return View("UploadImages", vm)
      End If

      For Each it As String In Request.Files
         Dim htp As HttpPostedFileBase = Request.Files(it)
         If htp.ContentLength = 0 Then
            Continue For
         End If

         Dim status As FileUploadStatus = UploadImageFile(htp, vm.ImgType, _imageservice)
         vm.Messages.AddRange(status.Message)
      Next

      Return View("UploadImages", vm)
   End Function


   Function UpdateImage(ByVal Id As Integer) As ActionResult
      If Id <= 0 Then
         Return RedirectToAction("Index")
      End If

      Dim img As SimpleImage = _imageservice.GetImageById(Id)
      If img Is Nothing Then
         Return RedirectToAction("Index")
      End If

      Dim vm As New AdminImageViewModel()
      img.FillViewModel(vm)
      Return View(vm)
   End Function

   <AcceptVerbs(HttpVerbs.Post)> _
   Function UpdateImage(ByVal vm As AdminImageViewModel) As ActionResult
      If vm.ImageID <= 0 Then
         Return RedirectToAction("Index")
      End If

      Dim img As SimpleImage = _imageservice.GetImageById(vm.ImageID)
      If img IsNot Nothing Then
         Dim htp As HttpPostedFileBase = Request.Files(0)
         If htp.ContentLength = 0 Then
            vm.Messages.Add("No file specified. Please select the file to upload")
            Return View(vm)
         End If

         Dim status As FileUploadStatus = ImageHelper.UpdateImageFile(htp, img)
         vm.Messages.AddRange(status.Message)
      End If

      Return RedirectToAction("CreateThumbnails", vm.ImageID)
   End Function


   Function SuggestImagesTags(ByVal q As String) As JsonResult
      If String.IsNullOrEmpty(q) Then
         Return Json(Nothing)
      End If
      If Request.IsAjaxRequest Then
         Dim li As List(Of String) = _imageTagService.SuggestImageTags(q)
         Return Json(String.Join(",", li.ToArray), JsonRequestBehavior.AllowGet)
      Else
         Return Json(Nothing, JsonRequestBehavior.AllowGet)
      End If
   End Function

   Function GetImages(ByVal tag As String) As JsonResult
      If String.IsNullOrEmpty(tag) Then
         Return Json(Nothing)
      End If
      If Request.IsAjaxRequest Then
         Dim li As List(Of Object) = FindImages(tag.Split(New Char() {",c"}, StringSplitOptions.RemoveEmptyEntries).ToList)
         Return Json(li, JsonRequestBehavior.AllowGet)
      Else
         Return Json(Nothing, JsonRequestBehavior.AllowGet)
      End If
   End Function

   Public Function FindImages(ByVal keys As List(Of String)) As List(Of Object)
      Dim li As New List(Of SimpleImage)

      li = _imageservice.GetImages(keys.ToArray)

      Dim iIndex As Integer = 0

      Dim imageList As New List(Of Object)

      For Each it As SimpleImage In li
         Dim large_str As String = ""
         Dim mini_str As String = ""
         Dim long_str As String = ""



         large_str = it.ImageUrl.Insert(it.ImageUrl.LastIndexOf("/") + 1, "large_")
         mini_str = it.ImageUrl.Insert(it.ImageUrl.LastIndexOf("/") + 1, "mini_")
         long_str = it.ImageUrl.Insert(it.ImageUrl.LastIndexOf("/") + 1, "long_")

         Dim urlprefix As String = Configs.Paths.CdnRoot & Configs.Paths.CdnImages
         Dim strUrl As String = Configs.Paths.CdnRoot & "/imagethumb.ashx?img=" & it.ImageUrl & "&w=150&h=80"

         imageList.Add(New With {.ImageID = it.ImageID, _
                                 .UrlPrefix = urlprefix, _
                                 .uuid = it.Uuid, _
                                 .ImageUrl = strUrl, _
                                 .large_url = large_str, _
                                 .long_url = long_str, _
                                 .mini_url = mini_str, _
                                 .CreditsName = it.CreditsName})
      Next

      Return imageList
   End Function

End Class
End Namespace