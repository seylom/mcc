Imports MCC.Data
Imports Webdiyer.WebControls.Mvc

Public Interface IUserQuestionCommentService
   Function GetUserQuestionCommentsCount() As Integer
   Function GetUserQuestionCommentsByQuestionIdCount(ByVal userQuestionId As Integer) As Integer
   Function GetUserQuestionComments() As List(Of UserQuestionComment)
   Function GetUserQuestionComments(ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AddedDate DESC") As PagedList(Of UserQuestionComment)
   Function GetUserQuestionCommentsByCriteria(ByVal criteria As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AddedDate DESC") As PagedList(Of UserQuestionComment)
   Function GetUserQuestionCommentsByQuestionId(ByVal userQuestionId As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AddedDate DESC") As PagedList(Of UserQuestionComment)
   Function GetUserQuestionCommentById(ByVal id As Integer) As UserQuestionComment
   Function GetUserQuestionCommentsByQuestionId(ByVal userQuestionId As Integer) As List(Of UserQuestionComment)
   Function InsertUserQuestionComment(ByVal uqc As UserQuestionComment) As Integer
   Sub UpdateUserQuestionComments(ByVal uqc As UserQuestionComment)
   Sub DeleteUserQuestionComment(ByVal UserQuestionCommentId As Integer)
   Sub DeleteUserQuestionComments(ByVal ids() As Integer)
End Interface
