Public Interface IAdviceCommentRepository
   Function GetAdviceComments() As IQueryable(Of AdviceComment)
   Sub InsertComment(ByVal comment As AdviceComment)
   Sub DeleteComment(ByVal commentId As Integer)
   Sub DeleteComments(ByVal commentIds() As Integer)
   Sub UpdateComment(ByVal commentToUpdate As AdviceComment)
End Interface
