Public Interface IVideoRepository
   Function GetVideos() As IQueryable(Of Video)
   Function GetVideosInCategory(ByVal categoryID As Integer) As IQueryable(Of Video)

   Sub DeleteVideo(ByVal Id As Integer)
   Sub DeleteVideos(ByVal Ids() As Integer)

   Function InsertVideo(ByVal _vd As Video) As Integer
   Sub UpdateVideo(ByVal _vd As Video)
End Interface
