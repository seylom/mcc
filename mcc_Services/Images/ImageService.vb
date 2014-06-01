Imports MCC.Data
Imports EeekSoft.Query

Imports Webdiyer.WebControls.Mvc
Public Class ImageService
   Inherits CacheObject
   Implements IImageService

   Private _imageRepo As IImageRepository

   Public Sub New()
      Me.New(New ImageRepository())
   End Sub


   Public Sub New(ByVal _imageRep As IImageRepository)
      _imageRepo = _imageRep
   End Sub

   Public Function GetImagesCount() As Integer Implements IImageService.GetImagesCount
      Dim key As String = "images_imagescount"
      If Cache(key) IsNot Nothing Then
         Return CInt(Cache(key))
      Else
         Dim it As Integer = _imageRepo.GetImages.Count()
         CacheData(key, it)
         Return it
      End If
   End Function


   Protected Sub CacheData(ByVal key As String, ByVal data As Object)
      If data IsNot Nothing Then
         CacheObject.Cache.Insert(key, data, Nothing, DateTime.Now.AddSeconds(3600), TimeSpan.Zero)
      End If
   End Sub

   Public Function GetImages() As List(Of SimpleImage) Implements IImageService.GetImages
      Dim key As String = "images_images_"
      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), List(Of SimpleImage))
      Else

         Dim it As List(Of SimpleImage) = _imageRepo.GetImages.ToList
         CacheData(key, it)
         Return it

      End If
   End Function

   Public Function GetImages(ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "Name") As PagedList(Of SimpleImage) Implements IImageService.GetImages
      Dim key As String = "images_images_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), PagedList(Of SimpleImage))
      Else
         Dim it As PagedList(Of SimpleImage) = _imageRepo.GetImages.SortBy(sortExp).ToPagedList(startrowindex, maximumrows)
         Return it
      End If
   End Function

   'Public  Function SuggestImages(ByVal criteria As String) As List(Of String)
   '   Dim key As String = "images_images_" & criteria & "_"
   '   If Cache(key) IsNot Nothing Then
   '      Return DirectCast(Cache(key), List(Of String))
   '   Else
   '      
   '         If mdc.simpleimages.Count(Function(p) p.Name.StartsWith(criteria)) Then
   '            Dim it As List(Of String) = (From i As simpleimage In mdc.simpleimages Where i.Name.StartsWith(criteria) Select i.Name).ToList
   '            CacheData(key, it)
   '            Return it
   '         Else
   '            Return Nothing
   '         End If
   '      
   '   End If
   'End Function

   Public Function GetImages(ByVal keys() As String) As List(Of SimpleImage) Implements IImageService.GetImages
      Array.Sort(keys)
      Dim ark As String = String.Join("_", keys)
      Dim key As String = "images_images_" & ark.ToString & "_"
      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), List(Of SimpleImage))
      Else
         Dim li As List(Of SimpleImage) = _imageRepo.GetImages.toexpandable.Where(Function(p) p.Tags.ContainsAny(keys)).ToList
         CacheData(key, li)
         Return li
      End If
   End Function


   Public Function GetImageByUuid(ByVal uuid As String) As SimpleImage Implements IImageService.GetImageByUuid
      Dim key As String = "images_imageuuid_" & uuid.ToString & "_"

      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), SimpleImage).Copy
      Else
         Dim it As SimpleImage = _imageRepo.GetImages.Where(Function(p) p.Uuid = uuid).FirstOrDefault()
         CacheData(key, it)
         Return it.Copy
      End If
   End Function

   Public Function GetImageById(ByVal imageId As Integer) As SimpleImage Implements IImageService.GetImageById
      Dim key As String = "images_images_" & imageId.ToString & "_"

      If Cache(key) IsNot Nothing Then
         Dim it As SimpleImage = DirectCast(Cache(key), SimpleImage)
         Return it.Copy
      Else
         Dim it As SimpleImage = _imageRepo.GetImages.Where(Function(p) p.ImageID = imageId).FirstOrDefault()
         CacheData(key, it)
         Return it.Copy
      End If
   End Function



   Public Sub InsertImage(ByVal img As SimpleImage) Implements IImageService.InsertImage
      'If mdc.simpleimages.Count(Function(p) p.uuid = img.Uuid) = 0 Then
      '   mdc.simpleimages.InsertOnSubmit(img)
      '   mdc.SubmitChanges()

      '   If Not String.IsNullOrEmpty(img.Tags) AndAlso img.Tags.Split(",").Count > 0 Then
      '      ImageTagRepository.InsertImageTags(img.Tags.Split(",").ToList())
      '   End If

      '   PurgeCacheItems("images_")
      'End If

      If img IsNot Nothing Then
         _imageRepo.InsertImage(img)
         PurgeCacheItems("images_")
      End If
   End Sub

   'Public Sub UpdateImageByUUID(ByVal uuid As String, ByVal description As String, ByVal creditsName As String, ByVal creditsUrl As String, ByVal tags As String, ByVal type As Integer)

   '   If mdc.simpleimages.Count(Function(p) p.uuid = uuid) > 0 Then
   '      Dim wrd As SimpleImage = (From t In mdc.simpleimages _
   '                                  Where t.uuid = uuid _
   '                                  Select t).Single()

   '      tags = ConvertNullToEmptyString(tags)
   '      creditsName = ConvertNullToEmptyString(creditsName)
   '      creditsUrl = ConvertNullToEmptyString(creditsUrl)
   '      description = ConvertNullToEmptyString(description)
   '      If wrd IsNot Nothing Then
   '         wrd.CreditsName = creditsName
   '         wrd.CreditsUrl = creditsUrl
   '         wrd.Tags = tags
   '         wrd.Description = description
   '         wrd.Type = type
   '         mdc.SubmitChanges()

   '         If Not String.IsNullOrEmpty(tags) AndAlso tags.Split(",").Count > 0 Then
   '            ImageTagRepository.InsertImageTags(tags.Split(",").ToList())
   '         End If

   '         CacheObject.PurgeCacheItems("images_images_" & wrd.ImageID.ToString)
   '         CacheObject.PurgeCacheItems("images_images_")
   '      End If
   '   End If

   'End Sub


   Public Sub UpdateImage(ByVal wrd As SimpleImage) Implements IImageService.UpdateImage



      If wrd IsNot Nothing Then

         wrd.Tags = ConvertNullToEmptyString(wrd.Tags)
         wrd.CreditsName = ConvertNullToEmptyString(wrd.CreditsName)
         wrd.CreditsUrl = ConvertNullToEmptyString(wrd.CreditsUrl)
         wrd.Description = ConvertNullToEmptyString(wrd.Description)

         If Not String.IsNullOrEmpty(wrd.CreditsUrl) Then
            If wrd.CreditsUrl.ToLower.StartsWith("www.") Then
               wrd.CreditsUrl = "http://" & wrd.CreditsUrl
            End If
         End If

         wrd.CreditsName = wrd.CreditsName
         wrd.CreditsUrl = wrd.CreditsUrl
         wrd.Tags = wrd.Tags
         wrd.Description = wrd.Description
         wrd.ImageType = wrd.ImageType


         _imageRepo.UpdateImage(wrd)

         'If Not String.IsNullOrEmpty(wrd.Tags) AndAlso wrd.Tags.Split(",").Count > 0 Then
         '   ImageTagRepository.InsertImageTags(wrd.Tags.Split(",").ToList())
         'End If


         CacheObject.PurgeCacheItems("images_images_")
         CacheObject.PurgeCacheItems("images_images_" & wrd.ImageID.ToString)
      End If
   End Sub


   Public Sub DeleteImage(ByVal imageId As Integer) Implements IImageService.DeleteImage

      'If mdc.simpleimages.Count(Function(p) p.ImageID = imageId) > 0 Then
      '   Dim tg As SimpleImage = (From it As SimpleImage In mdc.simpleimages Where it.ImageID = imageId).Single()

      '   Dim fi As New System.IO.FileInfo(HttpContext.Current.Server.MapPath(tg.ImageUrl))
      '   If fi IsNot Nothing AndAlso fi.Exists Then
      '      If fi.Name.IndexOf(fi.Directory.Name) > -1 Then
      '         System.IO.Directory.Delete(fi.Directory.FullName, True)
      '      End If
      '   End If

      '   mdc.simpleimages.DeleteOnSubmit(tg)
      '   mdc.SubmitChanges()
      '   PurgeCacheItems("images_imagescount")
      '   PurgeCacheItems("images_images_")
      'End If

      _imageRepo.DeleteImage(imageId)

      PurgeCacheItems("images_imagescount")
      PurgeCacheItems("images_images_")

   End Sub

   Public Sub DeleteImages(ByVal Ids() As Integer) Implements IImageService.DeleteImages

      'If mdc.simpleimages.Count(Function(p) Ids.Contains(p.ImageID)) > 0 Then

      '   Dim tg As List(Of SimpleImage) = (From it As SimpleImage In mdc.simpleimages Where Ids.Contains(it.ImageID)).ToList

      '   For Each img As SimpleImage In tg
      '      Dim fi As New System.IO.FileInfo(HttpContext.Current.Server.MapPath(img.ImageUrl))
      '      If fi IsNot Nothing AndAlso fi.Exists Then
      '         If fi.Name.IndexOf(fi.Directory.Name) > -1 Then
      '            System.IO.Directory.Delete(fi.Directory.FullName, True)
      '         End If
      '      End If
      '   Next

      '   mdc.simpleimages.DeleteAllOnSubmit(tg)
      '   mdc.SubmitChanges()
      '   PurgeCacheItems("images_imagescount")
      '   PurgeCacheItems("images_images_")
      'End If

      _imageRepo.DeleteImages(Ids)

      PurgeCacheItems("images_imagescount")
      PurgeCacheItems("images_images_")

   End Sub

   

   Public Sub PurgeImageCache() Implements IImageService.PurgeImageCache
      PurgeCacheItems("images_")
   End Sub

End Class
