Imports MCC.Data

Public Class AdminUploadImagesViewModel
   Inherits baseViewModel

   Private _imageType As ImageType
   Public Property ImgType() As ImageType
      Get
         Return _imageType
      End Get
      Set(ByVal value As ImageType)
         _imageType = value
      End Set
   End Property

End Class
