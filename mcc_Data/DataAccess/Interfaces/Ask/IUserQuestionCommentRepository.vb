Public Interface IUserQuestionCommentRepository
   Function GetUserQuestionComments() As IQueryable(Of UserQuestionComment)
   Function InsertUserQuestionComment(ByVal uqc As UserQuestionComment) As Integer
   Sub UpdateUserQuestionComment(ByVal uqc As UserQuestionComment)
   Sub DeleteUserQuestionComment(ByVal uqcId As Integer)
   Sub DeleteUserQuestionComments(ByVal uqcId() As Integer)
End Interface
