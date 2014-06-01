Public Interface IImageTagRepository
   Sub DeleteTags(ByVal tagIds() As Integer)
   Sub DeleteTag(ByVal tagId As Integer)

   Function GetTags() As IQueryable(Of ImageTag)
   Function GetImageTags(ByVal imageId As Integer) As IQueryable(Of ImageTag)
   Function InsertTag(ByVal tag As ImageTag) As Integer
   Function InsertTags(ByVal tags() As String) As Integer()
    Sub TagImage(ByVal imageId As Integer, ByVal tags() As String)
End Interface
