Imports MCC.Data
Imports Webdiyer.WebControls.Mvc
Public Class UserAnswerCommentService
   Inherits CacheObject
   Implements IUserAnswerCommentService



   Private _UserAnswerCommRepo As IUserAnswerCommentRepository

   Public Sub New()
      Me.New(New UserAnswerCommentRepository())
   End Sub


   Public Sub New(ByVal userAnswerCommRepo As IUserAnswerCommentRepository)
      _UserAnswerCommRepo = userAnswerCommRepo
   End Sub

   Public Function GetUserAnswerCommentsCount() As Integer Implements IUserAnswerCommentService.GetUserAnswerCommentsCount

      Dim key As String = "UserAnswerComments_QuestionCount"

      If Cache(key) IsNot Nothing Then
         Return CInt(Cache(key))
      Else

         Dim it As Integer = _UserAnswerCommRepo.GetUserAnswerComments.Count()
         CacheData(key, it)
         Return it
      End If
   End Function

   'Public Function GetUserAnswerCommentsByAnswerIdCount(ByVal UserAnswerId As Integer) As Integer Implements IUserAnswerCommentService.GetUserAnswerCommentsByAnswerIdCount

   '   Dim key As String = "UserAnswerComments_QuestionByQuestionCount" & "_" & UserAnswerId.ToString

   '   If Cache(key) IsNot Nothing Then
   '      Return CInt(Cache(key))
   '   Else

   '      Dim it As Integer = _UserAnswerCommRepo.GetUserAnswerComments.Count(Function(p) p.UserAnswerID = UserAnswerId)
   '      CacheData(key, it)
   '      Return it
   '   End If
   'End Function

   Public Function GetUserAnswerComments() As List(Of UserAnswerComment) Implements IUserAnswerCommentService.GetUserAnswerComments
      Dim key As String = "UserAnswerComments_UserAnswerComments_"
      If Cache(key) IsNot Nothing Then
         Dim li As List(Of UserAnswerComment) = DirectCast(Cache(key), List(Of UserAnswerComment))
         Return li
      Else

         Dim li As List(Of UserAnswerComment) = _UserAnswerCommRepo.GetUserAnswerComments.ToList
         CacheData(key, li)
         Return li
      End If
   End Function

   Public Function GetUserAnswerComments(ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AddedDate DESC") As PagedList(Of UserAnswerComment) Implements IUserAnswerCommentService.GetUserAnswerComments
      Dim key As String = "UserAnswerComments_UserAnswerComments_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
      If Cache(key) IsNot Nothing Then
            Dim li As PagedList(Of UserAnswerComment) = DirectCast(Cache(key), PagedList(Of UserAnswerComment))
         Return li
      Else

         Dim li As PagedList(Of UserAnswerComment) = _UserAnswerCommRepo.GetUserAnswerComments.SortBy(sortExp).ToPagedList(startrowindex, maximumrows)
         CacheData(key, li)
         Return li
      End If
   End Function

   Public Function GetUserAnswerCommentsByCriteria(ByVal criteria As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AddedDate DESC") As PagedList(Of UserAnswerComment) Implements IUserAnswerCommentService.GetUserAnswerCommentsByCriteria
      If Not String.IsNullOrEmpty(criteria) Then
         Dim key As String = "UserAnswerComments_UserAnswerCommentsbycriteria_" & criteria & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
         If Cache(key) IsNot Nothing Then
                Return DirectCast(Cache(key), PagedList(Of UserAnswerComment))
         Else
            Dim li As PagedList(Of UserAnswerComment) = _UserAnswerCommRepo.GetUserAnswerComments.Where(Function(it) it.Body.Contains(criteria)).SortBy(sortExp).ToPagedList(startrowindex, maximumrows)
            CacheData(key, li)
            Return li
         End If
      Else
         Return GetUserAnswerComments(startrowindex, maximumrows)
      End If
   End Function

   Public Function GetUserAnswerCommentsByAnswerId(ByVal UserAnswerId As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AddedDate DESC") As PagedList(Of UserAnswerComment) Implements IUserAnswerCommentService.GetUserAnswerCommentsByAnswerId
      If UserAnswerId > 0 Then
         Dim key As String = "UserAnswerComments_UserAnswerCommentsbyquestion_" & UserAnswerId & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Return DirectCast(Cache(key), PagedList(Of UserAnswerComment))
         Else
            Dim li As PagedList(Of UserAnswerComment) = _UserAnswerCommRepo.GetUserAnswerComments.Where(Function(it) it.UserAnswerID = UserAnswerId).SortBy(sortExp).ToPagedList(startrowindex, maximumrows)
            CacheData(key, li)
            Return li
         End If
      Else
         Return GetUserAnswerComments(startrowindex, maximumrows)
      End If
   End Function



   Public Function GetUserAnswerCommentById(ByVal id As Integer) As UserAnswerComment Implements IUserAnswerCommentService.GetUserAnswerCommentById
      Dim key As String = "UserAnswerComments_UserAnswerComments_" & id.ToString & "_"
      If Cache(key) IsNot Nothing Then
         Dim fb As UserAnswerComment = DirectCast(Cache(key), UserAnswerComment)
         Return fb
      Else
         Dim fb As UserAnswerComment = _UserAnswerCommRepo.GetUserAnswerComments.Where(Function(it) it.UserAnswerCommentID = id).FirstOrDefault
         CacheData(key, fb)
         Return fb
      End If
   End Function

   Public Function GetUserAnswerCommentsByQuestionId(ByVal UserAnswerId As Integer) As List(Of UserAnswerComment) Implements IUserAnswerCommentService.GetUserAnswerCommentsByAnswerId
      Dim key As String = "UserAnswerComments_UserAnswerCommentsByQuestion_" & UserAnswerId.ToString & "_"
      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), List(Of UserAnswerComment))
      Else
         Dim fb As List(Of UserAnswerComment) = _UserAnswerCommRepo.GetUserAnswerComments.Where(Function(it) it.UserAnswerID = UserAnswerId).ToList
         CacheData(key, fb)
         Return fb
      End If
   End Function


   Public Function InsertUserAnswerComment(ByVal uqc As UserAnswerComment) As Integer Implements IUserAnswerCommentService.InsertUserAnswerComment
      Dim idx As Integer = 0
      If uqc IsNot Nothing Then

         With uqc
            .AddedDate = DateTime.Now
            .AddedBy = CacheObject.CurrentUserName
         End With

         idx = _UserAnswerCommRepo.InsertUserAnswerComment(uqc)
         CacheObject.PurgeCacheItems("UserAnswerComments_")
      End If
      Return idx
   End Function


   Public Sub UpdateUserAnswerComments(ByVal uqc As UserAnswerComment) Implements IUserAnswerCommentService.UpdateUserAnswerComments
      If uqc IsNot Nothing AndAlso uqc.UserAnswerCommentID > 0 Then
         _UserAnswerCommRepo.UpdateUserAnswerComment(uqc)
         CacheObject.PurgeCacheItems("UserAnswerComments_")
      End If
   End Sub


   Public Sub DeleteUserAnswerComment(ByVal UserAnswerCommentId As Integer) Implements IUserAnswerCommentService.DeleteUserAnswerComment
      If UserAnswerCommentId > 0 Then
         _UserAnswerCommRepo.DeleteUserAnswerComment(UserAnswerCommentId)
         CacheObject.PurgeCacheItems("UserAnswerComments_")
      End If
   End Sub


   Public Sub DeleteUserAnswerComments(ByVal Ids() As Integer) Implements IUserAnswerCommentService.DeleteUserAnswerComments
      If Ids IsNot Nothing Then
         _UserAnswerCommRepo.DeleteUserAnswerComments(Ids)
         CacheObject.PurgeCacheItems("UserAnswerComments_")
      End If
   End Sub

   Protected Sub CacheData(ByVal key As String, ByVal data As Object)
      If data IsNot Nothing Then
         CacheObject.Cache.Insert(key, data, Nothing, DateTime.Now.AddSeconds(3600), TimeSpan.Zero)
      End If
   End Sub


End Class
