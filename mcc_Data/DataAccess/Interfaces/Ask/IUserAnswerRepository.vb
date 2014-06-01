Public Interface IUserAnswerRepository
   Function GetUserAnswers() As IQueryable(Of UserAnswer)
   Function InsertUserAnswer(ByVal uq As UserAnswer) As StatusItem
   Sub UpdateUserAnswer(ByVal uq As UserAnswer)
   Sub DeleteUserAnswer(ByVal uqId As Integer)
   Sub DeleteUserAnswers(ByVal uqId() As Integer)

   Function VoteAnswerUp(ByVal uqId As Integer, ByVal username As String) As StatusItem
   Function VoteAnswerDown(ByVal uqId As Integer, ByVal username As String) As StatusItem
End Interface
