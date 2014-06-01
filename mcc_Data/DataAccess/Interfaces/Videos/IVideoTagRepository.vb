Public Interface IVideoTagRepository
   Sub DeleteTags(ByVal tagIds() As Integer)
   Sub DeleteTag(ByVal tagId As Integer)

   Function GetTags() As IQueryable(Of VideoTag)
   Function GetVideoTags(ByVal videoId As Integer) As IQueryable(Of VideoTag)
   Function InsertTag(ByVal tag As VideoTag) As Integer
   Function InsertTags(ByVal tags() As String) As Integer()
    Sub TagVideo(ByVal videoId As Integer, ByVal tags() As String)
End Interface
