Imports System.Data
Imports MCC.Data
Imports Webdiyer.WebControls.Mvc
Public Class ArticleCommentService
   Inherits CacheObject
   Implements IArticleCommentService


   Private _articleCommentRepo As IArticleCommentRepository
   Private _articleRepo As IArticleRepository

   Public Sub New()
      Me.New(New ArticleCommentRepository(), New ArticleRepository())
   End Sub

   Public Sub New(ByVal _IArticleCommRepo As IArticleCommentRepository, ByVal _IArticleRepo As IArticleRepository)
      _articleCommentRepo = _IArticleCommRepo
      _articleRepo = _IArticleRepo
   End Sub

   Public Function GetArticleTitle(ByVal ArticleId As Integer) As String Implements IArticleCommentService.GetArticleTitle

      Dim key As String = "ArticleComments_ArticleComment_ArticleTitle_" & ArticleId.ToString

      If Cache(key) IsNot Nothing Then
            Return CStr(Cache(key))
      Else
         Dim str As String = _articleRepo.GetArticles.WithArticleID(ArticleId).Select(Function(p) p.Title).FirstOrDefault()
         CacheData(key, str)
         Return str
      End If
   End Function

   Public Function GetArticleCommentCount() As Integer Implements IArticleCommentService.GetArticleCommentCount

      Dim key As String = "ArticleComments_ArticleCommentCount_"

      If Cache(key) IsNot Nothing Then
         Return CInt(Cache(key))
      Else

         Dim it As Integer = _articleCommentRepo.GetArticleComments().Count
         CacheData(key, it)
         Return it
      End If
   End Function


   Public Function GetArticleCommentCount(ByVal id As Integer) As Integer Implements IArticleCommentService.GetArticleCommentCount
      If id > 0 Then

         Dim key As String = "ArticleComments_ArticleCommentCount_" & id.ToString & "_"

         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            Dim it As Integer = _articleCommentRepo.GetArticleComments.WithArticleID(id).Count()
            CacheData(key, it)
            Return it
         End If
      Else
         Return GetArticleCommentCount()
      End If
   End Function

   Public Function GetArticleComments() As List(Of ArticleComment) Implements IArticleCommentService.GetArticleComments
      Dim key As String = "ArticleComments_ArticleComments_"

      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), List(Of ArticleComment))
      Else
         Dim li As List(Of ArticleComment) = _articleCommentRepo.GetArticleComments().ToList
         CacheData(key, li)
         Return li
      End If
   End Function

   Public Function GetArticleComments(ByVal articleId As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedDate DESC") As PagedList(Of ArticleComment) Implements IArticleCommentService.GetArticleComments
      If articleId > 0 Then
         Dim key As String = "ArticleComments_ArticleComments_" & articleId.ToString & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"

         If String.IsNullOrEmpty(sortExp) Then
            sortExp = "addedDate DESC"
         End If

         If Cache(key) IsNot Nothing Then
                Dim li As PagedList(Of ArticleComment) = DirectCast(Cache(key), PagedList(Of ArticleComment))
            Return li
         Else
            Dim li As PagedList(Of ArticleComment) = _articleCommentRepo.GetArticleComments.WithArticleID(articleId).ToPagedList(startrowindex, maximumrows)
            CacheData(key, li)
            Return li
         End If
      Else
         Return Nothing
      End If
   End Function

    Public Function GetArticleCommentsByUsername(ByVal username As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedDate DESC") As PagedList(Of ArticleComment) Implements IArticleCommentService.GetArticleCommentsByUsername
        If Not String.IsNullOrEmpty(username) Then
            Dim key As String = "ArticleComments_ArticleComments_" & username.ToString & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"

            If String.IsNullOrEmpty(sortExp) Then
                sortExp = "addedDate DESC"
            End If
            If Cache(key) IsNot Nothing Then
                Return DirectCast(Cache(key), PagedList(Of ArticleComment))
            Else

                Dim li As PagedList(Of ArticleComment) = _articleCommentRepo.GetArticleComments. _
                Where(Function(p) p.AddedBy = username).ToPagedList(startrowindex, maximumrows)

                CacheData(key, li)
                Return li
            End If
        Else
            Return Nothing
        End If
    End Function


   Public Function GetArticleComments(ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedDate DESC") As PagedList(Of ArticleComment) Implements IArticleCommentService.GetArticleComments
      Dim key As String = "ArticleComments_ArticleComments_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"

      If String.IsNullOrEmpty(sortExp) Then
         sortExp = "addedDate DESC"
      End If

      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), PagedList(Of ArticleComment))
      Else
         Dim li As PagedList(Of ArticleComment) = _articleCommentRepo.GetArticleComments.SortBy(sortExp).ToPagedList(startrowindex, maximumrows)
         CacheData(key, li)
         Return li
      End If
   End Function

   Public Function GetArticleCommentById(ByVal CommentId As Integer) As ArticleComment Implements IArticleCommentService.GetArticleCommentById
      Dim key As String = "ArticleComments_ArticleComments_" & CommentId.ToString & "_"
      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), ArticleComment)
      Else

         Dim fb As ArticleComment = _articleCommentRepo.GetArticleComments.WithID(CommentId).FirstOrDefault
         CacheData(key, fb)
         Return fb
      End If
   End Function



   Public Sub InsertArticleComment(ByVal aComment As ArticleComment) Implements IArticleCommentService.InsertArticleComment
      With aComment
         .AddedDate = DateTime.Now
         .AddedBy = CacheObject.CurrentUserName
         .AddedByEmail = CacheObject.CurrentUserEmail
         .AddedByIP = CacheObject.CurrentUserIP
      End With

      _articleCommentRepo.InsertComment(aComment)
      CacheObject.PurgeCacheItems("ArticleComments_")
      CacheObject.PurgeCacheItems("articles_articleComments_")

   End Sub

   Public Sub UpdateArticleComment(ByVal wrd As ArticleComment) Implements IArticleCommentService.UpdateArticleComment
      If wrd IsNot Nothing Then

         _articleCommentRepo.UpdateComment(wrd)

         CacheObject.PurgeCacheItems("ArticleComments_ArticleComments_")
         CacheObject.PurgeCacheItems("ArticleComments_ArticleCommentCount_")
         CacheObject.PurgeCacheItems("articles_articleComments_")
      End If
   End Sub

   Public Sub DeleteArticleComment(ByVal commentId As Integer) Implements IArticleCommentService.DeleteArticleComment
      _articleCommentRepo.DeleteComment(commentId)
      CacheObject.PurgeCacheItems("ArticleComments_ArticleComments_")
      CacheObject.PurgeCacheItems("articles_articleComments_")
   End Sub

   Public Sub DeleteArticleComments(ByVal commentIds() As Integer) Implements IArticleCommentService.DeleteArticleComments
      _articleCommentRepo.DeleteComments(commentIds)
      CacheObject.PurgeCacheItems("ArticleComments_ArticleComments_")
      CacheObject.PurgeCacheItems("articles_articleComments_")
   End Sub

   Public Function FindCommentsWithExactMatch(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal SearchWord As String) As PagedList(Of ArticleComment) Implements IArticleCommentService.FindCommentsWithExactMatch
      Return _articleCommentRepo.GetArticleComments.Where(Function(p) p.Body.Contains(SearchWord)).ToPagedList(startRowIndex, maximumRows)
   End Function

    Public Function FindCommentsWithAnyMatch(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal SearchWord As String) As PagedList(Of ArticleComment) Implements IArticleCommentService.FindCommentsWithAnyMatch
        Return _articleCommentRepo.GetArticleComments.Where(Function(p) p.Body.Contains(SearchWord)).ToPagedList(startRowIndex, maximumRows)
    End Function

    Function FindCommentsWithExactMatchCount(ByVal searchWord As String) As Integer Implements IArticleCommentService.FindCommentsWithExactMatchCount
        Return _articleCommentRepo.GetArticleComments.Where(Function(p) p.Body.Contains(searchWord)).Count()
    End Function

    Function FindCommentsWithAnyMatchCount(ByVal searchWord As String) As Integer Implements IArticleCommentService.FindCommentsWithAnyMatchCount
        Return _articleCommentRepo.GetArticleComments.Where(Function(p) p.Body.Contains(searchWord)).Count()
    End Function

   Protected Sub CacheData(ByVal key As String, ByVal data As Object)
      If data IsNot Nothing Then
         CacheObject.Cache.Insert(key, data, Nothing, DateTime.Now.AddSeconds(3600), TimeSpan.Zero)
      End If
   End Sub
End Class
