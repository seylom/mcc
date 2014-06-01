Imports MCC.Data
Imports Webdiyer.WebControls.Mvc

Public Interface IUserAnswerCommentService
   Function GetUserAnswerCommentsCount() As Integer
   Function GetUserAnswerComments() As List(Of UserAnswerComment)
   Function GetUserAnswerComments(ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AddedDate DESC") As PagedList(Of UserAnswerComment)
   Function GetUserAnswerCommentsByCriteria(ByVal criteria As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AddedDate DESC") As PagedList(Of UserAnswerComment)
   Function GetUserAnswerCommentsByAnswerId(ByVal UserAnswerId As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AddedDate DESC") As PagedList(Of UserAnswerComment)
   Function GetUserAnswerCommentById(ByVal id As Integer) As UserAnswerComment
   Function GetUserAnswerCommentsByAnswerId(ByVal UserAnswerId As Integer) As List(Of UserAnswerComment)
   Function InsertUserAnswerComment(ByVal uqc As UserAnswerComment) As Integer
   Sub UpdateUserAnswerComments(ByVal uqc As UserAnswerComment)
   Sub DeleteUserAnswerComment(ByVal UserAnswerCommentId As Integer)
   Sub DeleteUserAnswerComments(ByVal Ids() As Integer)
End Interface
