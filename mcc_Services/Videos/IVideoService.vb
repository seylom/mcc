Imports MCC.Data
Imports Webdiyer.WebControls.Mvc
Public Interface IVideoService
   ''' <summary>
   ''' Get the average rating for an video
   ''' </summary>
   ''' <param name="videoId">The video id.</param>
   ''' <returns></returns>
   Function AverageRating(ByVal videoId As Integer) As Double
   ''' <summary>
   ''' Gets the comments.
   ''' </summary>
   ''' <param name="videoId">The video id.</param>
   ''' <returns></returns>
   Function GetComments(ByVal videoId As Integer) As IList(Of VideoComment)
   Function CommentsCount(ByVal VideoId As Integer) As Integer
   Function GetVideoCount() As Integer
   Function GetVideoCount(ByVal PublishedOnly As Boolean) As Integer
   Function GetVideoCount(ByVal categoryId As Integer) As Integer
   Function GetVideoCount(ByVal publishedOnly As Boolean, ByVal categoryID As Integer) As Integer
   Function GetVideos() As List(Of Video)
   Function GetVideos(ByVal publishedOnly As Boolean, ByVal categoryId As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AddedDate") As PagedList(Of Video)
   'Function GetVideosByCategoryName(ByVal publishedOnly As Boolean, ByVal categoryName As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "releaseDate") As PagedList(Of Video)
   Function GetVideos(ByVal categoryId As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AddedDate") As PagedList(Of Video)
   Function GetVideosByAuthor(ByVal publishedOnly As Boolean, ByVal addedBy As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedby") As PagedList(Of Video)
   Function GetVideos(ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AddedDate DESC") As PagedList(Of Video)
   Function GetVideos(ByVal publishedOnly As Boolean, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AddedDate DESC") As PagedList(Of Video)
   ''' <summary>
   ''' Returns an Video object with the specified ID
   ''' </summary>
   Function GetLatestVideos(ByVal pageSize As Integer) As PagedList(Of Video)
   Function GetVideoById(ByVal VideoId As Integer) As Video
   Function GetVideoBySlug(ByVal slug As String) As Video
   ''' <summary>
   ''' Creates a new Video
   ''' </summary>
   Sub InsertVideo(ByVal wrd As Video)
   Sub UpdateVideo(ByVal _art As Video)
   Sub DeleteVideos(ByVal VideoIds As Integer())
   Sub DeleteVideo(ByVal VideoId As Integer)
   Sub ApproveVideo(ByVal videoId As Integer)
   Function RemoveVideoFromCategory(ByVal VideoID As Integer, Optional ByVal categoryId As Integer = 0) As Boolean
   Function AddVideoToCategories(ByVal VideoId As Integer, ByVal CategoryIds As Integer()) As Integer
   Sub IncrementViewCount(ByVal videoId As Integer)
   Sub RateVideo(ByVal videoId As Integer, ByVal rating As Integer)
   Function FindVideosWithExactMatch(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal SearchWord As String) As PagedList(Of Video)
   Function FindVideosWithAnyMatch(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal SearchWord As String) As PagedList(Of Video)
    Function FindVideoWithExactMatchCount(ByVal SearchWord As String) As Integer
    Function FindVideoWithAnyMatchCount(ByVal SearchWord As String) As Integer
   Sub SimpleUpdateVideo(ByVal wrd As Video)
End Interface
