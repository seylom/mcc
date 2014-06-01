Public Interface IImageRepository

   Function GetImages() As IQueryable(Of SimpleImage)
   Sub DeleteImage(ByVal Id As Integer)
   Sub DeleteImages(ByVal Ids() As Integer)
   Function InsertImage(ByVal spImage As SimpleImage) As Integer
   Sub UpdateImage(ByVal spImage As SimpleImage)

End Interface
