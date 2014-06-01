Public Interface IUserQuestionRepository
   Function GetUserQuestions() As IQueryable(Of UserQuestion)
   Function InsertUserQuestion(ByVal uq As UserQuestion) As StatusItem
   Sub UpdateUserQuestion(ByVal uq As UserQuestion)
   Sub DeleteUserQuestion(ByVal uqId As Integer)
   Sub DeleteUserQuestions(ByVal uqId() As Integer)

   Function VoteQuestionUp(ByVal uqId As Integer, ByVal username As String) As StatusItem
   Function VoteQuestionDown(ByVal uqId As Integer, ByVal username As String) As StatusItem

   Function SubscribeOrUnsubscribeQuestion(ByVal questionId As Integer, ByVal username As String) As Boolean
   Function GetSubscribedQuestions(ByVal username As String) As IList(Of Integer)
   Function GetSubscribers(ByVal questionId As Integer) As IList(Of String)
   Function IsSubscribed(ByVal questionId As Integer, ByVal username As String) As Boolean
End Interface
