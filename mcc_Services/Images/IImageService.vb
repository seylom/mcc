Imports MCC.Data
Imports Webdiyer.WebControls.Mvc
Public Interface IImageService
   Function GetImagesCount() As Integer
   Function GetImages() As List(Of SimpleImage)
   Function GetImages(ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "Name") As PagedList(Of SimpleImage)
   Function GetImages(ByVal keys As String()) As List(Of SimpleImage)
   Function GetImageByUuid(ByVal uuid As String) As SimpleImage
   Function GetImageById(ByVal imageId As Integer) As SimpleImage
   Sub InsertImage(ByVal img As SimpleImage)
   Sub UpdateImage(ByVal wrd As SimpleImage)
   Sub DeleteImage(ByVal imageId As Integer)
   Sub DeleteImages(ByVal Ids As Integer())
   Sub PurgeImageCache()
End Interface
