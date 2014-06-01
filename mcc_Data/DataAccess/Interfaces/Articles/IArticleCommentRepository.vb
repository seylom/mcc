Public Interface IArticleCommentRepository
   Function GetArticleComments() As IQueryable(Of ArticleComment)
   Sub InsertComment(ByVal comment As ArticleComment)
   Sub DeleteComment(ByVal commentId As Integer)
   Sub DeleteComments(ByVal commentIds() As Integer)
   Sub UpdateComment(ByVal commentToUpdate As ArticleComment)
End Interface
