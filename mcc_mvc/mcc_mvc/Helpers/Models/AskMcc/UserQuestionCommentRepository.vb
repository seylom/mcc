Imports Microsoft.VisualBasic
Imports System.Linq
Imports MCC.Data

Namespace UserQuestions
   Public Class UserQuestionCommentRepository
      Inherits mccObject

      Public Shared Function GetUserQuestionCommentsCount() As Integer

         Dim key As String = "UserQuestionComments_QuestionCount"

         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            Dim mdc As New MCCDataContext()
            Dim it As Integer = mdc.mcc_UserQuestionComments.Count()
            CacheData(key, it)
            Return it
         End If
      End Function

      Public Shared Function GetUserQuestionCommentsByQuestionIdCount(ByVal userQuestionId As Integer) As Integer

         Dim key As String = "UserQuestionComments_QuestionByQuestionCount" & "_" & userQuestionId.ToString

         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            Dim mdc As New MCCDataContext()
            Dim it As Integer = mdc.mcc_UserQuestionComments.Count(Function(p) p.UserQuestionId = userQuestionId)
            CacheData(key, it)
            Return it
         End If
      End Function

      Public Shared Function GetUserQuestionComments() As List(Of mcc_UserQuestionComment)
         Dim key As String = "UserQuestionComments_UserQuestionComments_"
         If Cache(key) IsNot Nothing Then
            Dim li As List(Of mcc_UserQuestionComment) = DirectCast(Cache(key), List(Of mcc_UserQuestionComment))
            Return li
         Else
            Dim mdc As New MCCDataContext()
            Dim li As List(Of mcc_UserQuestionComment) = mdc.mcc_UserQuestionComments.ToList
            CacheData(key, li)
            Return li
         End If
      End Function

      Public Shared Function GetUserQuestionComments(ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AddedDdate ASC") As List(Of mcc_UserQuestionComment)
         Dim key As String = "UserQuestionComments_UserQuestionComments_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Dim li As List(Of mcc_UserQuestionComment) = DirectCast(Cache(key), List(Of mcc_UserQuestionComment))
            Return li
         Else
            Dim mdc As New MCCDataContext()
            Dim li As List(Of mcc_UserQuestionComment)
            If startrowindex > 0 AndAlso maximumrows > 0 Then
               li = mdc.mcc_UserQuestionComments.Skip(startrowindex - 1).Take(maximumrows).ToList
            Else
               li = mdc.mcc_UserQuestionComments.Skip(0).Take(10).ToList
            End If
            CacheData(key, li)
            Return li
         End If
      End Function

      Public Shared Function GetUserQuestionCommentsByCriteria(ByVal criteria As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AddedDdate ASC") As List(Of mcc_UserQuestionComment)
         If Not String.IsNullOrEmpty(criteria) Then
            Dim key As String = "UserQuestionComments_UserQuestionCommentsbycriteria_" & criteria & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
            If Cache(key) IsNot Nothing Then
               Dim li As List(Of mcc_UserQuestionComment) = DirectCast(Cache(key), List(Of mcc_UserQuestionComment))
               Return li
            Else
               Dim mdc As New MCCDataContext()
               Dim li As List(Of mcc_UserQuestionComment)
               startrowindex = IIf(startrowindex >= 0, startrowindex, 0)
               maximumrows = IIf(maximumrows > 0, maximumrows, 10)

               li = (From it As mcc_UserQuestionComment In mdc.mcc_UserQuestionComments Where it.Body.Contains(criteria)).Skip(startrowindex).Take(maximumrows).ToList

               CacheData(key, li)
               Return li
            End If
         Else
            Return GetUserQuestionComments(startrowindex, maximumrows)
         End If
      End Function

      Public Shared Function GetUserQuestionCommentsByQuestionId(ByVal userQuestionId As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AddedDdate ASC") As List(Of mcc_UserQuestionComment)
         If userQuestionId > 0 Then
            Dim key As String = "UserQuestionComments_UserQuestionCommentsbyquestion_" & userQuestionId & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
            If Cache(key) IsNot Nothing Then
               Dim li As List(Of mcc_UserQuestionComment) = DirectCast(Cache(key), List(Of mcc_UserQuestionComment))
               Return li
            Else
               Dim mdc As New MCCDataContext()
               Dim li As List(Of mcc_UserQuestionComment)
               startrowindex = IIf(startrowindex >= 0, startrowindex, 0)
               maximumrows = IIf(maximumrows > 0, maximumrows, 10)

               li = (From it As mcc_UserQuestionComment In mdc.mcc_UserQuestionComments Where it.UserQuestionId = userQuestionId).Skip(startrowindex).Take(maximumrows).ToList

               CacheData(key, li)
               Return li
            End If
         Else
            Return GetUserQuestionComments(startrowindex, maximumrows)
         End If
      End Function



      Public Shared Function GetUserQuestionById(ByVal id As Integer) As mcc_UserQuestionComment
         Dim key As String = "UserQuestionComments_UserQuestionComments_" & id.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Dim fb As mcc_UserQuestionComment = DirectCast(Cache(key), mcc_UserQuestionComment)
            Return fb
         Else
            Dim mdc As New MCCDataContext()
            Dim fb As mcc_UserQuestionComment = Nothing
            If mdc.mcc_UserQuestionComments.Count(Function(p) p.UserQuestionId = id) > 0 Then
               fb = (From it As mcc_UserQuestionComment In mdc.mcc_UserQuestionComments Where it.UserQuestionId = id).FirstOrDefault
            End If
            CacheData(key, fb)
            Return fb
         End If
      End Function

      Public Shared Function GetUserQuestionCommentsByQuestionId(ByVal userQuestionId As Integer) As List(Of mcc_UserQuestionComment)
         Dim key As String = "UserQuestionComments_UserQuestionCommentsByQuestion_" & userQuestionId.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Dim fb As List(Of mcc_UserQuestionComment) = DirectCast(Cache(key), List(Of mcc_UserQuestionComment))
            Return fb
         Else
            Dim mdc As New MCCDataContext()
            Dim fb As List(Of mcc_UserQuestionComment) = New List(Of mcc_UserQuestionComment)
            If mdc.mcc_UserQuestionComments.Count(Function(p) p.UserQuestionId = userQuestionId) > 0 Then
               fb = (From it As mcc_UserQuestionComment In mdc.mcc_UserQuestionComments Where it.UserQuestionId = userQuestionId).ToList
            End If
            CacheData(key, fb)
            Return fb
         End If
      End Function


      Public Shared Sub InsertUserQuestionComment(ByVal userQuestionId As Integer, ByVal body As String)
         Dim mdc As MCCDataContext = New MCCDataContext
         Dim fb As New mcc_UserQuestionComment()

         With fb
            .UserQuestionId = userQuestionId
            .Body = ConvertNullToEmptyString(body)
            .AddedDate = DateTime.Now
            .AddedBy = mccObject.CurrentUserName
         End With

         mdc.mcc_UserQuestionComments.InsertOnSubmit(fb)
         mccObject.PurgeCacheItems("UserQuestionComments_")
         mdc.SubmitChanges()
      End Sub


      Public Shared Sub UpdateUserQuestionComments(ByVal userQuestionCommentId As Integer, ByVal title As String, ByVal body As String)
         Dim mdc As MCCDataContext = New MCCDataContext

         If mdc.mcc_UserQuestionComments.Count(Function(p) p.UserQuestionCommentId = userQuestionCommentId) > 0 Then
            Dim wrd As mcc_UserQuestionComment = (From t In mdc.mcc_UserQuestionComments _
                                        Where t.UserQuestionCommentId = userQuestionCommentId _
                                        Select t).Single()
            If wrd IsNot Nothing Then
               'wrd.Title = title
               wrd.Body = ConvertNullToEmptyString(body)
               ' do not update post slugs
               'wrd.Slug = routines.GetSlugFromString(title)
               mdc.SubmitChanges()
               mccObject.PurgeCacheItems("UserQuestionComments_UserQuestionComments_")
            End If
         End If
      End Sub


      Public Shared Sub DeleteUserQuestionComment(ByVal UserQuestionCommentId As Integer)
         Dim mdc As MCCDataContext = New MCCDataContext
         If mdc.mcc_UserQuestionComments.Count(Function(p) p.UserQuestionCommentId = UserQuestionCommentId) > 0 Then
            Dim wrd As mcc_UserQuestionComment = (From t In mdc.mcc_UserQuestionComments _
                                        Where t.UserQuestionCommentId = UserQuestionCommentId _
                                        Select t).Single()
            If wrd IsNot Nothing Then
               mdc.mcc_UserQuestionComments.DeleteOnSubmit(wrd)
               mdc.SubmitChanges()
               mccObject.PurgeCacheItems("UserQuestionComments_UserQuestionComments_")
            End If
         End If
      End Sub

      Protected Shared Sub CacheData(ByVal key As String, ByVal data As Object)
         If data IsNot Nothing Then
            mccObject.Cache.Insert(key, data, Nothing, DateTime.Now.AddSeconds(3600), TimeSpan.Zero)
         End If
      End Sub
   End Class
End Namespace

