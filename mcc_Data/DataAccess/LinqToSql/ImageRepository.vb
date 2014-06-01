Public Class ImageRepository
   Implements IImageRepository


   Private _mdc As MCCDataContext


   Public Sub New()
      Me.New(New MCCDataContext)
   End Sub

   Public Sub New(ByVal dc As MCCDataContext)
      _mdc = dc
   End Sub

   Public Sub DeleteImage(ByVal Id As Integer) Implements IImageRepository.DeleteImage
      If Id > 0 Then
            Dim q = _mdc.mcc_Images.Where(Function(p) p.ImageID = Id).FirstOrDefault
         If q IsNot Nothing Then
            _mdc.mcc_Images.DeleteOnSubmit(q)
            _mdc.SubmitChanges()
         End If
      End If
   End Sub

   Public Sub DeleteImages(ByVal Ids() As Integer) Implements IImageRepository.DeleteImages
      If Ids IsNot Nothing Then
         Dim q = _mdc.mcc_Images.Where(Function(p) Ids.Contains(p.ImageID))
         If q IsNot Nothing Then
            _mdc.mcc_Images.DeleteAllOnSubmit(q)
            _mdc.SubmitChanges()
         End If
      End If
   End Sub

   Public Function GetImages() As System.Linq.IQueryable(Of SimpleImage) Implements IImageRepository.GetImages
        Dim q = From it As mcc_Image In _mdc.mcc_Images _
                Select New SimpleImage With { _
                             .ImageID = it.ImageID, _
                             .AddedDate = If(it.AddedDate, DateTime.Now), _
                             .ImageUrl = it.ImageUrl, _
                             .Name = it.Name, _
                             .CreditsName = it.CreditsName, _
                             .CreditsUrl = it.CreditsUrl, _
                             .Description = it.Description, _
                             .Tags = it.Tags, _
                             .ImageType = it.Type, _
                             .Uuid = it.uuid}

      Return q
   End Function



   Public Function InsertImage(ByVal spImage As SimpleImage) As Integer Implements IImageRepository.InsertImage
      If spImage IsNot Nothing Then
         Dim m As New mcc_Image
         With m
            .AddedDate = spImage.AddedDate
            .ImageUrl = spImage.ImageUrl
            .Name = spImage.Name
            .CreditsName = spImage.CreditsName
            .CreditsUrl = spImage.CreditsUrl
            .Description = spImage.Description
            .Tags = spImage.Tags
            .Type = spImage.ImageType
            .uuid = spImage.Uuid
         End With

         _mdc.mcc_Images.InsertOnSubmit(m)
         _mdc.SubmitChanges()
      End If
   End Function

   Public Sub UpdateImage(ByVal spImage As SimpleImage) Implements IImageRepository.UpdateImage
      If spImage IsNot Nothing Then
         Dim m As mcc_Image = _mdc.mcc_Images.Where(Function(p) p.ImageID = spImage.ImageID).FirstOrDefault()
         If m Is Nothing Then
            Return
         End If
         With m
            '.AddedDate = spImage.AddedDate
            '.ImageUrl = spImage.ImageUrl
            .Name = spImage.Name
            .CreditsName = spImage.CreditsName
            .CreditsUrl = spImage.CreditsUrl
            .Description = spImage.Description
            .Tags = spImage.Tags
            .Type = spImage.ImageType
            .uuid = spImage.Uuid
         End With

         _mdc.SubmitChanges()
      End If
   End Sub
End Class
