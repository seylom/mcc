Imports MCC.Data
Imports MCC.Services
Imports System.IO

Module ImageHelper


    Public Function UpdateImageFile(ByVal fileToUpload As HttpPostedFileBase, ByVal img As SimpleImage) As FileUploadStatus
        Dim status As New FileUploadStatus()

        If img Is Nothing Then
            status.Success = False
            status.Message.Add("Image not found in the database")
            Return status
        End If

        Dim url As String = img.ImageUrl
        If String.IsNullOrEmpty(url) Or String.IsNullOrEmpty(img.Name) Then
            status.Message.Add("This image cannot be update for server reason. Please contact your administrator")
            status.Success = False
            Return status
        End If

        Dim dirToMake As String = img.Uuid
        Dim res As Boolean = routines.UploadToFTP(fileToUpload, img.Name, dirToMake, mccEnum.FileTypePath.Images)


        'img.ImageUrl = String.Format("/{0}/{1}", dirToMake, img.Name)
        'imageserv.UpdateImage(img)

        status.Success = True
        Return status
    End Function

   Public Function UploadImageFile(ByVal fileToUpload As HttpPostedFileBase, ByVal imagefileType As ImageType, ByVal imagesrvr As IImageService) As FileUploadStatus

      ' should also check the file format!

      Dim status As New FileUploadStatus()
      If fileToUpload.ContentLength < Configs.Uploads.MaxImageSize Then

         ' build the file name
         Dim uid As String = ""
         Dim success As Boolean = False

         Do
            uid = routines.GenerateName(New Random(), 12)
            success = True
         Loop While success = False

         Dim dirToMake As String = uid
         Dim origShortName As String = uid & Path.GetExtension(fileToUpload.FileName) ' fileToUpload.FileName.Substring(fileToUpload.FileName.LastIndexOf("\") + 1)

         Dim res As Boolean = routines.UploadToFTP(fileToUpload, origShortName, dirToMake, mccEnum.FileTypePath.Images)

         If res Then
            status.Success = res
            status.Message.Add(fileToUpload.FileName & " uploaded successfully")
         Else
            status.Success = res
            status.Message.Add(fileToUpload.FileName & " couldn't be uploaded for server reasons")
         End If


         If imagefileType = ImageType.Article Then
            AddImageToDatabase(origShortName, uid, imagefileType, imagesrvr)
         End If

         Return status
      Else
         status.Success = False
         status.Message.Add(fileToUpload.FileName & " was too large to be uploaded. Please specify a smaller file.")
         Return status
      End If
   End Function

   Public Function AddImageToDatabase(ByVal ShortFileName As String, ByVal uuid As String, ByVal type As ImageType, ByVal imagesrvr As ImageService) As Boolean

      Dim img As SimpleImage = New SimpleImage()
      img.Uuid = uuid
      img.AddedDate = DateTime.Now

      img.ImageUrl = "/" & uuid & "/" & ShortFileName.Replace(" ", "")
      img.Name = ShortFileName.Replace(" ", "")
      img.Description = ShortFileName.Replace(" ", "")

      img.Tags = String.Empty
      img.CreditsName = String.Empty
      img.CreditsUrl = String.Empty
      img.ImageType = CInt(type)


      imagesrvr.InsertImage(img)
   End Function


   Function CreateThumbnailsFromImages(ByVal thumbsviewmodel As CreateThumbsViewModel) As Boolean

      If thumbsviewmodel Is Nothing Then
         Return False
      End If

      If thumbsviewmodel.ImageID <= 0 Or String.IsNullOrEmpty(thumbsviewmodel.ImageUrl) Then
         Return False
      End If

      Dim imagefullurl As String = Configs.Paths.CdnRoot & Configs.Paths.CdnImages & thumbsviewmodel.ImageUrl
      Dim imageUri As New Uri(imagefullurl)
      Dim imagefilepath = HttpContext.Current.Server.MapPath("/Uploads/images/" & thumbsviewmodel.ImageUrl)

      Dim fi As New FileInfo(imagefilepath)
      If Not Directory.Exists(fi.Directory.FullName) Then
         Directory.CreateDirectory(fi.Directory.FullName)
      End If

      ' download image
      Dim remoteimage As New System.Net.WebClient
      remoteimage.DownloadFile(imageUri, fi.FullName)

      'create thumbnail from selection


      If Not fi.Exists() Then
         Return False
      End If

      With thumbsviewmodel
         If .CreateMini Then
            routines.CreateCroppedThumbnail(fi.FullName, "mini_" & fi.Name, .W_Mini, .H_Mini, .X_Mini, .Y_Mini, 80, 80)
         End If

         If .CreateLong Then
            routines.CreateCroppedThumbnail(fi.FullName, "long_" & fi.Name, .W_Long, .H_Long, .X_Long, .Y_Long, 250, 100)
         End If

         If .CreateLarge Then
            routines.CreateCroppedThumbnail(fi.FullName, "large_" & fi.Name, .W_Large, .H_Large, .X_Large, .Y_Large, 400, 200)
         End If
      End With

      ' upload files back to ftp
      Dim createdFiles() As FileInfo = fi.Directory.GetFiles()
      For Each it As FileInfo In createdFiles
         routines.UploadLocalFileToFTP(it.FullName, Configs.Paths.CdnImages & fi.Directory.Name)
      Next

      ' delete local directory
      Try
         Directory.Delete(fi.Directory.FullName, True)
      Catch ex As Exception

      End Try

      Return True
   End Function


End Module
