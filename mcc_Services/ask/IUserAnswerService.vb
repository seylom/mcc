Imports MCC.Data
Imports Webdiyer.WebControls.Mvc

Public Interface IUserAnswerService
   Function GetUserAnswersCount() As Integer
   Function GetUserAnswersCountByUser(ByVal user As String) As Integer
   Function GetUserAnswers() As List(Of UserAnswer)
   Function GetUserAnswers(ByVal criteria As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AddedDate DESC") As PagedList(Of UserAnswer)
   Function GetUserAnswers(ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AddedDate DESC") As PagedList(Of UserAnswer)
   Function GetUserAnswersByUser(ByVal user As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AddedDate DESC") As PagedList(Of UserAnswer)
   Function GetUserAnswerById(ByVal id As Integer) As UserAnswer
   Function GetUserAnswersByQuestionId(ByVal userQuestionId As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "Votes DESC") As PagedList(Of UserAnswer)
   Function InsertUserAnswer(ByVal uq As UserAnswer) As StatusItem
   Sub UpdateUserAnswers(ByVal uq As UserAnswer)
   Function VoteUp(ByVal userAnswerId As Integer) As StatusItem
   Function VoteDown(ByVal userAnswerId As Integer) As StatusItem
   Sub DeleteAnswer(ByVal UserAnswerId As Integer)
   Sub DeleteAnswers(ByVal Ids() As Integer)

   Sub IncrementViewCount(ByVal userAnswerId As Integer)
   Function GetUserAnswersCountByQuestionId(ByVal userAnswerId As Integer) As Integer
   Function FindAnswersWithAnyMatch(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal SearchWord As String) As PagedList(Of UserAnswer)
   Function FindAnswersWithExactMatch(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal SearchWord As String) As PagedList(Of UserAnswer)
   ''' <summary>
   ''' Returns the number of total answers matching the search key
   ''' </summary>
   Function FindAnswersCount(ByVal searchWord As String) As Integer
End Interface
