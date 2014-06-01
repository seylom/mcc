Imports System.Data
Imports MCC.Data
Imports Webdiyer.WebControls.Mvc

Public Interface IArticleCommentService
   Function GetArticleTitle(ByVal ArticleId As Integer) As String
   Function GetArticleCommentCount() As Integer
   Function GetArticleCommentCount(ByVal id As Integer) As Integer
   Function GetArticleComments() As List(Of ArticleComment)
   Function GetArticleComments(ByVal articleId As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedDate DESC") As PagedList(Of ArticleComment)
    Function GetArticleCommentsByUsername(ByVal username As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedDate DESC") As PagedList(Of ArticleComment)
   Function GetArticleComments(ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedDate DESC") As PagedList(Of ArticleComment)
   Function GetArticleCommentById(ByVal CommentId As Integer) As ArticleComment
   Sub InsertArticleComment(ByVal aComment As ArticleComment)
   Sub UpdateArticleComment(ByVal wrd As ArticleComment)
   Sub DeleteArticleComment(ByVal commentId As Integer)
   Sub DeleteArticleComments(ByVal commentIds() As Integer)
   Function FindCommentsWithExactMatch(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal SearchWord As String) As PagedList(Of ArticleComment)
    Function FindCommentsWithAnyMatch(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal SearchWord As String) As PagedList(Of ArticleComment)
    Function FindCommentsWithExactMatchCount(ByVal searchWord As String) As Integer
    Function FindCommentsWithAnyMatchCount(ByVal searchWord As String) As Integer
End Interface
