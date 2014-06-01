Imports MCC.Data
Imports Webdiyer.WebControls.Mvc

Public Interface IUserQuestionService
   Function GetUserQuestionsCount() As Integer
   Function GetUserQuestionsCount(ByVal questionState As String) As Integer
   Function GetUserQuestionsCountByUser(ByVal user As String) As Integer
   Function GetUserQuestions() As List(Of UserQuestion)
   Function GetUserQuestions(ByVal criteria As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AddedDate DESC") As PagedList(Of UserQuestion)
   Function GetUserQuestions(ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AddedDate DESC") As PagedList(Of UserQuestion)
   Function GetUserQuestionsByUser(ByVal user As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AddedDate DESC") As PagedList(Of UserQuestion)
   Function GetUserQuestionsByState(ByVal uqState As QuestionState, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AddedDate DESC") As PagedList(Of UserQuestion)
   Function GetUserQuestionById(ByVal id As Integer) As UserQuestion
   Function GetUserQuestionSlugById(ByVal id As Integer) As String
   Function InsertUserQuestion(ByVal uq As UserQuestion) As StatusItem
   Sub UpdateUserQuestions(ByVal uq As UserQuestion)
   Function VoteUp(ByVal userQuestionId As Integer) As StatusItem
   Function VoteDown(ByVal userQuestionId As Integer) As StatusItem
   Sub DeleteQuestion(ByVal UserQuestionId As Integer)
   Sub DeleteQuestions(ByVal Ids() As Integer)
   ''' <summary>
   ''' If the answer was already accepted it will un-accept it!
   ''' </summary>
   ''' <param name="questionId"></param>
   ''' <param name="answerId"></param>
   ''' <remarks></remarks>
   Sub SetAcceptedAnswer(ByVal questionId As Integer, ByVal answerId As Integer)
   Sub IncrementViewCount(ByVal userQuestionId As Integer)
   Function GetUserAnswersCountById(ByVal userQuestionId As Integer) As Integer
   Function FindQuestionsWithAnyMatch(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal SearchWord As String) As PagedList(Of UserQuestion)
   Function FindQuestionsWithExactMatch(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal SearchWord As String) As PagedList(Of UserQuestion)
   ''' <summary>
   ''' Returns the number of total questions matching the search key
   ''' </summary>
   Function FindQuestionsCount(ByVal searchWord As String) As Integer
   Function SubscribeOrUnsubscribe(ByVal questionId As Integer, ByVal username As String) As Boolean
   Function GetSubscribers(ByVal questionId As Integer) As IList(Of String)
   Function GetSubscribedQuestions(ByVal username As String) As IList(Of Integer)
   Function IsSubscribed(ByVal questionId As Integer, ByVal username As String) As Boolean
End Interface
