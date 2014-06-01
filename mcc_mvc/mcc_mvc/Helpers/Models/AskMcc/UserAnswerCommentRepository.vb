Imports Microsoft.VisualBasic
Imports System.Linq
Imports MCC.Data
Namespace UserAnswers
   Public Class UserAnswerCommentRepository
      Inherits mccObject

      Public Shared Function GetUserAnswerCommentsCount() As Integer

         Dim key As String = "UserAnswerComments_AnswerCount"

         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            Dim mdc As New MCCDataContext()
            Dim it As Integer = mdc.mcc_UserAnswerComments.Count()
            CacheData(key, it)
            Return it
         End If
      End Function

      Public Shared Function GetUserAnswerCommentsByAnswerIdCount(ByVal userAnswerId As Integer) As Integer

         Dim key As String = "UserAnswerComments_AnswerByQuestionCount" & "_" & userAnswerId.ToString

         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            Dim mdc As New MCCDataContext()
            Dim it As Integer = mdc.mcc_UserAnswerComments.Count(Function(p) p.UserAnswerId = userAnswerId)
            CacheData(key, it)
            Return it
         End If
      End Function

      Public Shared Function GetUserAnswerComments() As List(Of mcc_UserAnswerComment)
         Dim key As String = "UserAnswerComments_UserAnswerComments_"
         If Cache(key) IsNot Nothing Then
            Dim li As List(Of mcc_UserAnswerComment) = DirectCast(Cache(key), List(Of mcc_UserAnswerComment))
            Return li
         Else
            Dim mdc As New MCCDataContext()
            Dim li As List(Of mcc_UserAnswerComment) = mdc.mcc_UserAnswerComments.ToList
            CacheData(key, li)
            Return li
         End If
      End Function

      Public Shared Function GetUserAnswerComments(ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AddedDdate ASC") As List(Of mcc_UserAnswerComment)
         Dim key As String = "UserAnswerComments_UserAnswerComments_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Dim li As List(Of mcc_UserAnswerComment) = DirectCast(Cache(key), List(Of mcc_UserAnswerComment))
            Return li
         Else
            Dim mdc As New MCCDataContext()
            Dim li As List(Of mcc_UserAnswerComment)
            startrowindex = IIf(startrowindex >= 0, startrowindex, 0)
            maximumrows = IIf(maximumrows > 0, maximumrows, 10)
            li = mdc.mcc_UserAnswerComments.Skip(startrowindex - 1).Take(maximumrows).ToList

            CacheData(key, li)
            Return li
         End If
      End Function

      Public Shared Function GetUserAnswerCommentsByCriteria(ByVal criteria As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AddedDdate ASC") As List(Of mcc_UserAnswerComment)
         If Not String.IsNullOrEmpty(criteria) Then
            Dim key As String = "UserAnswerComments_UserAnswerCommentsbycriteria_" & criteria & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
            If Cache(key) IsNot Nothing Then
               Dim li As List(Of mcc_UserAnswerComment) = DirectCast(Cache(key), List(Of mcc_UserAnswerComment))
               Return li
            Else
               Dim mdc As New MCCDataContext()
               Dim li As List(Of mcc_UserAnswerComment)

               startrowindex = IIf(startrowindex >= 0, startrowindex, 0)
               maximumrows = IIf(maximumrows > 0, maximumrows, 10)

               li = (From it As mcc_UserAnswerComment In mdc.mcc_UserAnswerComments Where it.Body.Contains(criteria)).Skip(startrowindex).Take(maximumrows).ToList

               CacheData(key, li)
               Return li
            End If
         Else
            Return GetUserAnswerComments(startrowindex, maximumrows)
         End If
      End Function

      Public Shared Function GetUserAnswerCommentsByAnswerId(ByVal userAnswerId As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AddedDdate ASC") As List(Of mcc_UserAnswerComment)
         If userAnswerId > 0 Then
            Dim key As String = "UserAnswerComments_UserAnswerCommentsbyanswer_" & userAnswerId & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
            If Cache(key) IsNot Nothing Then
               Dim li As List(Of mcc_UserAnswerComment) = DirectCast(Cache(key), List(Of mcc_UserAnswerComment))
               Return li
            Else
               Dim mdc As New MCCDataContext()
               Dim li As List(Of mcc_UserAnswerComment)

               startrowindex = IIf(startrowindex >= 0, startrowindex, 0)
               maximumrows = IIf(maximumrows > 0, maximumrows, 10)



               li = (From it As mcc_UserAnswerComment In mdc.mcc_UserAnswerComments Where it.UserAnswerId = userAnswerId).Skip(startrowindex).Take(maximumrows).ToList

               CacheData(key, li)
               Return li
            End If
         Else
            Return GetUserAnswerComments(startrowindex, maximumrows)
         End If
      End Function



      Public Shared Function GetUserAnswerById(ByVal id As Integer) As mcc_UserAnswerComment
         Dim key As String = "UserAnswerComments_UserAnswerComments_" & id.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Dim fb As mcc_UserAnswerComment = DirectCast(Cache(key), mcc_UserAnswerComment)
            Return fb
         Else
            Dim mdc As New MCCDataContext()
            Dim fb As mcc_UserAnswerComment = Nothing
            If mdc.mcc_UserAnswerComments.Count(Function(p) p.UserAnswerId = id) > 0 Then
               fb = (From it As mcc_UserAnswerComment In mdc.mcc_UserAnswerComments Where it.UserAnswerId = id).FirstOrDefault
            End If
            CacheData(key, fb)
            Return fb
         End If
      End Function

      Public Shared Function GetUserAnswerCommentsByAnswerId(ByVal userAnswerId As Integer) As List(Of mcc_UserAnswerComment)
         Dim key As String = "UserAnswerComments_UserAnswerCommentsByQuestion_" & userAnswerId.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Dim fb As List(Of mcc_UserAnswerComment) = DirectCast(Cache(key), List(Of mcc_UserAnswerComment))
            Return fb
         Else
            Dim mdc As New MCCDataContext()
            Dim fb As List(Of mcc_UserAnswerComment) = New List(Of mcc_UserAnswerComment)
            If mdc.mcc_UserAnswerComments.Count(Function(p) p.UserAnswerId = userAnswerId) > 0 Then
               fb = (From it As mcc_UserAnswerComment In mdc.mcc_UserAnswerComments Where it.UserAnswerId = userAnswerId).ToList
            End If
            CacheData(key, fb)
            Return fb
         End If
      End Function


      Public Shared Sub InsertUserAnswerComment(ByVal userAnswerId As Integer, ByVal body As String)
         Dim mdc As MCCDataContext = New MCCDataContext
         Dim fb As New mcc_UserAnswerComment()

         With fb
            .UserAnswerId = userAnswerId
            .Body = ConvertNullToEmptyString(body)
            .AddedDate = DateTime.Now
            .AddedBy = mccObject.CurrentUserName
         End With

         mdc.mcc_UserAnswerComments.InsertOnSubmit(fb)
         mccObject.PurgeCacheItems("UserAnswerComments_")
         mdc.SubmitChanges()
      End Sub


      Public Shared Sub UpdateUserAnswerComments(ByVal userAnswerCommentId As Integer, ByVal title As String, ByVal body As String)
         Dim mdc As MCCDataContext = New MCCDataContext

         If mdc.mcc_UserAnswerComments.Count(Function(p) p.UserAnswerCommentId = userAnswerCommentId) > 0 Then
            Dim wrd As mcc_UserAnswerComment = (From t In mdc.mcc_UserAnswerComments _
                                        Where t.UserAnswerCommentId = userAnswerCommentId _
                                        Select t).Single()
            If wrd IsNot Nothing Then
               'wrd.Title = title
               wrd.Body = ConvertNullToEmptyString(body)
               ' do not update post slugs
               'wrd.Slug = routines.GetSlugFromString(title)
               mdc.SubmitChanges()
               mccObject.PurgeCacheItems("UserAnswerComments_UserAnswerComments_")
            End If
         End If
      End Sub


      Public Shared Sub DeleteUserAnswerComment(ByVal UserAnswerCommentId As Integer)
         Dim mdc As MCCDataContext = New MCCDataContext
         If mdc.mcc_UserAnswerComments.Count(Function(p) p.UserAnswerCommentId = UserAnswerCommentId) > 0 Then
            Dim wrd As mcc_UserAnswerComment = (From t In mdc.mcc_UserAnswerComments _
                                        Where t.UserAnswerCommentId = UserAnswerCommentId _
                                        Select t).Single()
            If wrd IsNot Nothing Then
               mdc.mcc_UserAnswerComments.DeleteOnSubmit(wrd)
               mdc.SubmitChanges()
               mccObject.PurgeCacheItems("UserAnswerComments_UserAnswerComments_")
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

