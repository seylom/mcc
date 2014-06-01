Imports MCC.Data
Imports Webdiyer.WebControls.Mvc
Public Class UserQuestionCommentService
   Inherits CacheObject
   Implements IUserQuestionCommentService



   Private _userquestionCommRepo As IUserQuestionCommentRepository

   Public Sub New()
      Me.New(New UserQuestionCommentRepository())
   End Sub


   Public Sub New(ByVal userQuestionCommRepo As IUserQuestionCommentRepository)
      _userquestionCommRepo = userQuestionCommRepo
   End Sub

   Public Function GetUserQuestionCommentsCount() As Integer Implements IUserQuestionCommentService.GetUserQuestionCommentsCount

      Dim key As String = "UserQuestionComments_QuestionCount"

      If Cache(key) IsNot Nothing Then
         Return CInt(Cache(key))
      Else

         Dim it As Integer = _userquestionCommRepo.GetUserQuestionComments.Count()
         CacheData(key, it)
         Return it
      End If
   End Function

   Public Function GetUserQuestionCommentsByQuestionIdCount(ByVal userQuestionId As Integer) As Integer Implements IUserQuestionCommentService.GetUserQuestionCommentsByQuestionIdCount

      Dim key As String = "UserQuestionComments_QuestionByQuestionCount" & "_" & userQuestionId.ToString

      If Cache(key) IsNot Nothing Then
         Return CInt(Cache(key))
      Else

         Dim it As Integer = _userquestionCommRepo.GetUserQuestionComments.Count(Function(p) p.UserQuestionID = userQuestionId)
         CacheData(key, it)
         Return it
      End If
   End Function

   Public Function GetUserQuestionComments() As List(Of UserQuestionComment) Implements IUserQuestionCommentService.GetUserQuestionComments
      Dim key As String = "UserQuestionComments_UserQuestionComments_"
      If Cache(key) IsNot Nothing Then
         Dim li As List(Of UserQuestionComment) = DirectCast(Cache(key), List(Of UserQuestionComment))
         Return li
      Else

         Dim li As List(Of UserQuestionComment) = _userquestionCommRepo.GetUserQuestionComments.ToList
         CacheData(key, li)
         Return li
      End If
   End Function

   Public Function GetUserQuestionComments(ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AddedDate DESC") As PagedList(Of UserQuestionComment) Implements IUserQuestionCommentService.GetUserQuestionComments
      Dim key As String = "UserQuestionComments_UserQuestionComments_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
      If Cache(key) IsNot Nothing Then
            Dim li As PagedList(Of UserQuestionComment) = DirectCast(Cache(key), PagedList(Of UserQuestionComment))
         Return li
      Else

         Dim li As PagedList(Of UserQuestionComment) = _userquestionCommRepo.GetUserQuestionComments.SortBy(sortExp).ToPagedList(startrowindex, maximumrows)
         CacheData(key, li)
         Return li
      End If
   End Function

   Public Function GetUserQuestionCommentsByCriteria(ByVal criteria As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AddedDate DESC") As PagedList(Of UserQuestionComment) Implements IUserQuestionCommentService.GetUserQuestionCommentsByCriteria
      If Not String.IsNullOrEmpty(criteria) Then
         Dim key As String = "UserQuestionComments_UserQuestionCommentsbycriteria_" & criteria & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
         If Cache(key) IsNot Nothing Then
                Return DirectCast(Cache(key), PagedList(Of UserQuestionComment))
         Else
            Dim li As PagedList(Of UserQuestionComment) = _userquestionCommRepo.GetUserQuestionComments.Where(Function(it) it.body.Contains(criteria)).SortBy(sortExp).ToPagedList(startrowindex, maximumrows)
            CacheData(key, li)
            Return li
         End If
      Else
         Return GetUserQuestionComments(startrowindex, maximumrows)
      End If
   End Function

   Public Function GetUserQuestionCommentsByQuestionId(ByVal userQuestionId As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AddedDate DESC") As PagedList(Of UserQuestionComment) Implements IUserQuestionCommentService.GetUserQuestionCommentsByQuestionId
      If userQuestionId > 0 Then
         Dim key As String = "UserQuestionComments_UserQuestionCommentsbyquestion_" & userQuestionId & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Return DirectCast(Cache(key), PagedList(Of UserQuestionComment))
         Else
            Dim li As PagedList(Of UserQuestionComment) = _userquestionCommRepo.GetUserQuestionComments.Where(Function(it) it.UserQuestionID = userQuestionId).SortBy(sortExp).ToPagedList(startrowindex, maximumrows)
            CacheData(key, li)
            Return li
         End If
      Else
         Return GetUserQuestionComments(startrowindex, maximumrows)
      End If
   End Function



   Public Function GetUserQuestionCommentById(ByVal id As Integer) As UserQuestionComment Implements IUserQuestionCommentService.GetUserQuestionCommentById
      Dim key As String = "UserQuestionComments_UserQuestionComments_" & id.ToString & "_"
      If Cache(key) IsNot Nothing Then
         Dim fb As UserQuestionComment = DirectCast(Cache(key), UserQuestionComment)
         Return fb
      Else
         Dim fb As UserQuestionComment = _userquestionCommRepo.GetUserQuestionComments.Where(Function(it) it.UserQuestionCommentID = id).FirstOrDefault
         CacheData(key, fb)
         Return fb
      End If
   End Function

   Public Function GetUserQuestionCommentsByQuestionId(ByVal userQuestionId As Integer) As List(Of UserQuestionComment) Implements IUserQuestionCommentService.GetUserQuestionCommentsByQuestionId
      Dim key As String = "UserQuestionComments_UserQuestionCommentsByQuestion_" & userQuestionId.ToString & "_"
      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), List(Of UserQuestionComment))
      Else
         Dim fb As List(Of UserQuestionComment) = _userquestionCommRepo.GetUserQuestionComments.Where(Function(it) it.UserQuestionID = userQuestionId).ToList
         CacheData(key, fb)
         Return fb
      End If
   End Function


   Public Function InsertUserQuestionComment(ByVal uqc As UserQuestionComment) As Integer Implements IUserQuestionCommentService.InsertUserQuestionComment
      Dim idx As Integer = 0
      If uqc IsNot Nothing Then

         With uqc
            .AddedDate = DateTime.Now
            .AddedBy = CacheObject.CurrentUserName
         End With

         idx = _userquestionCommRepo.InsertUserQuestionComment(uqc)
         CacheObject.PurgeCacheItems("UserQuestionComments_")
      End If
      Return idx
   End Function


   Public Sub UpdateUserQuestionComments(ByVal uqc As UserQuestionComment) Implements IUserQuestionCommentService.UpdateUserQuestionComments
      If uqc IsNot Nothing AndAlso uqc.UserQuestionCommentID > 0 Then
         _userquestionCommRepo.UpdateUserQuestionComment(uqc)
         CacheObject.PurgeCacheItems("UserQuestionComments_")
      End If
   End Sub


   Public Sub DeleteUserQuestionComment(ByVal UserQuestionCommentId As Integer) Implements IUserQuestionCommentService.DeleteUserQuestionComment
      If UserQuestionCommentId > 0 Then
         _userquestionCommRepo.DeleteUserQuestionComment(UserQuestionCommentId)
         CacheObject.PurgeCacheItems("UserQuestionComments_")
      End If
   End Sub

   Public Sub DeleteUserQuestionComments(ByVal ids() As Integer) Implements IUserQuestionCommentService.DeleteUserQuestionComments
      If ids IsNot Nothing Then
         _userquestionCommRepo.DeleteUserQuestionComments(ids)
         CacheObject.PurgeCacheItems("UserQuestionComments_")
      End If
   End Sub

   Protected Sub CacheData(ByVal key As String, ByVal data As Object)
      If data IsNot Nothing Then
         CacheObject.Cache.Insert(key, data, Nothing, DateTime.Now.AddSeconds(3600), TimeSpan.Zero)
      End If
   End Sub


End Class
