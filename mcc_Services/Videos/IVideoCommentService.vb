Imports MCC.Data
Imports Webdiyer.WebControls.Mvc
Public Interface IVideoCommentService
   Function GetVideoTitle(ByVal VideoId As Integer) As String
   Function GetVideoCommentCount() As Integer
   Function GetVideoCommentCount(ByVal id As Integer) As Integer
   Function GetVideoComments() As List(Of VideoComment)
   Function GetVideoComments(ByVal videoId As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedDate DESC") As PagedList(Of VideoComment)
    Function GetVideoCommentsByUsername(ByVal username As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedDate DESC") As PagedList(Of VideoComment)
   Function GetVideoComments(ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedDate DESC") As PagedList(Of VideoComment)
   Function GetVideoCommentById(ByVal CommentId As Integer) As VideoComment
   Sub InsertVideoComment(ByVal aComment As VideoComment)
   Sub UpdateVideoComment(ByVal wrd As VideoComment)
   Sub DeleteVideoComment(ByVal commentId As Integer)
   Function FindCommentsWithExactMatch(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal SearchWord As String) As PagedList(Of VideoComment)
    Function FindCommentsWithAnyMatch(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal SearchWord As String) As PagedList(Of VideoComment)
    Function FindCommentsWithExactMatchCount(ByVal searchWord As String) As Integer
    Function FindCommentsWithAnyMatchCount(ByVal searchWord As String) As Integer
End Interface
