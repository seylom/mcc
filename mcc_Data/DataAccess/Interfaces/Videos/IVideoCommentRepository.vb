Public Interface IVideoCommentRepository
   Function GetVideoComments() As IQueryable(Of VideoComment)
   Sub InsertComment(ByVal comment As VideoComment)
   Sub DeleteComment(ByVal commentId As Integer)
   Sub DeleteComments(ByVal commentIds() As Integer)
   Sub UpdateComment(ByVal commentToUpdate As VideoComment)
End Interface
