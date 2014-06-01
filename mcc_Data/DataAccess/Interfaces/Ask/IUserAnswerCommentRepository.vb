Public Interface IUserAnswerCommentRepository
   Function GetUserAnswerComments() As IQueryable(Of UserAnswerComment)
   Function InsertUserAnswerComment(ByVal uqc As UserAnswerComment) As Integer
   Sub UpdateUserAnswerComment(ByVal uqc As UserAnswerComment)
   Sub DeleteUserAnswerComment(ByVal uqcId As Integer)
   Sub DeleteUserAnswerComments(ByVal uqcId() As Integer)
End Interface
