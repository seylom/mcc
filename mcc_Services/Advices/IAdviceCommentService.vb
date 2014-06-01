Imports MCC.Data
Imports Webdiyer.WebControls.Mvc
Public Interface IAdviceCommentService
   Function GetAdviceTitle(ByVal AdviceId As Integer) As String
   Function GetAdviceCommentCount() As Integer
   Function GetAdviceCommentCount(ByVal id As Integer) As Integer
   Function GetAdviceComments() As List(Of AdviceComment)
   Function GetAdviceComments(ByVal adviceId As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedDate DESC") As PagedList(Of AdviceComment)
    Function GetAdviceCommentsByUsername(ByVal username As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedDate DESC") As PagedList(Of AdviceComment)
   Function GetAdviceComments(ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedDate DESC") As PagedList(Of AdviceComment)
   Function GetAdviceCommentById(ByVal CommentId As Integer) As AdviceComment
   Sub InsertAdviceComment(ByVal aComment As AdviceComment)
   Sub UpdateAdviceComment(ByVal wrd As AdviceComment)
   Sub DeleteAdviceComment(ByVal commentId As Integer)
   Function FindCommentsWithExactMatch(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal SearchWord As String) As PagedList(Of AdviceComment)
    Function FindCommentsWithAnyMatch(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal SearchWord As String) As PagedList(Of AdviceComment)
    Function FindCommentsWithExactMatchCount(ByVal searchWord As String) As Integer
    Function FindCommentsWithAnyMatchCount(ByVal searchWord As String) As Integer
End Interface
