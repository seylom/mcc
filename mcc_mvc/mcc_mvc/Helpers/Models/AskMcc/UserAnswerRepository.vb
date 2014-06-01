Imports Microsoft.VisualBasic
Imports System.Linq

Imports MCC.Data

Namespace UserAnswers
   Public Class UserAnswerRepository
      Inherits mccObject

      Public Shared Function GetUserAnswersCount() As Integer

         Dim key As String = "UserAnswers_AnswerCount"

         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            Dim mdc As New MCCDataContext()
            Dim it As Integer = mdc.mcc_UserAnswers.Count()
            CacheData(key, it)
            Return it
         End If
      End Function

      Public Shared Function GetUserAnswersCountByUser(ByVal user As String) As Integer

         If String.IsNullOrEmpty(user) Then
            Return 0
         End If

         Dim key As String = "UserAnswers_AnswerCountByUser_" & user

         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            Dim mdc As New MCCDataContext()
            Dim it As Integer = mdc.mcc_UserAnswers.Count()
            CacheData(key, it)
            Return it
         End If
      End Function

      Public Shared Function GetUserAnswersByQuestionIdCount(ByVal userQuestionId As Integer) As Integer

         Dim key As String = "UserAnswers_AnswerByQuestionCount" & "_" & userQuestionId.ToString

         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            Dim mdc As New MCCDataContext()
            Dim it As Integer = mdc.mcc_UserAnswers.Count(Function(p) p.UserQuestionId = userQuestionId)
            CacheData(key, it)
            Return it
         End If
      End Function

      Public Shared Function GetUserAnswers() As List(Of mcc_UserAnswer)
         Dim key As String = "UserAnswers_All_"
         If Cache(key) IsNot Nothing Then
            Dim li As List(Of mcc_UserAnswer) = DirectCast(Cache(key), List(Of mcc_UserAnswer))
            Return li
         Else
            Dim mdc As New MCCDataContext()
            Dim li As List(Of mcc_UserAnswer) = mdc.mcc_UserAnswers.ToList
            CacheData(key, li)
            Return li
         End If
      End Function

      Public Shared Function GetUserAnswers(ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AddedDate DESC") As List(Of mcc_UserAnswer)
         Dim key As String = "UserAnswers_All_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Dim li As List(Of mcc_UserAnswer) = DirectCast(Cache(key), List(Of mcc_UserAnswer))
            Return li
         Else
            Dim mdc As New MCCDataContext()
            Dim li As List(Of mcc_UserAnswer)
            If startrowindex > 0 AndAlso maximumrows > 0 Then
               li = mdc.mcc_UserAnswers.Skip(startrowindex - 1).Take(maximumrows).ToList
            Else
               li = mdc.mcc_UserAnswers.Skip(0).Take(10).ToList
            End If
            CacheData(key, li)
            Return li
         End If
      End Function

      Public Shared Function GetUserAnswersByCriteria(ByVal criteria As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AddedDdate ASC") As List(Of mcc_UserAnswer)
         If Not String.IsNullOrEmpty(criteria) Then
            Dim key As String = "UserAnswers_All_Criteria" & criteria & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
            If Cache(key) IsNot Nothing Then
               Dim li As List(Of mcc_UserAnswer) = DirectCast(Cache(key), List(Of mcc_UserAnswer))
               Return li
            Else
               Dim mdc As New MCCDataContext()
               Dim li As List(Of mcc_UserAnswer)
               If startrowindex > 0 AndAlso maximumrows > 0 Then
                  li = (From it As mcc_UserAnswer In mdc.mcc_UserAnswers Where it.Body.Contains(criteria)).Skip(startrowindex).Take(maximumrows).ToList
               Else
                  li = (From it As mcc_UserAnswer In mdc.mcc_UserAnswers Where it.Body.Contains(criteria)).Skip(0).Take(20).ToList
               End If
               CacheData(key, li)
               Return li
            End If
         Else
            Return GetUserAnswers(startrowindex, maximumrows)
         End If
      End Function

      Public Shared Function GetUserAnswersByQuestionId(ByVal userQuestionId As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "Votes DESC") As List(Of mcc_UserAnswer)
         If userQuestionId > 0 Then
            Dim key As String = "UserAnswers_ByQuestion_" & userQuestionId.ToString & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
            If String.IsNullOrEmpty(sortExp) Then
               sortExp = "Votes DESC"
            End If
            If Cache(key) IsNot Nothing Then
               Dim li As List(Of mcc_UserAnswer) = DirectCast(Cache(key), List(Of mcc_UserAnswer))
               Return li
            Else
               Dim mdc As New MCCDataContext()
               Dim li As List(Of mcc_UserAnswer)
               startrowindex = IIf(startrowindex >= 0, startrowindex, 0)
               maximumrows = IIf(maximumrows > 0, maximumrows, 20)

               li = (From it As mcc_UserAnswer In mdc.mcc_UserAnswers Where it.UserQuestionId = userQuestionId).SortBy(sortExp).Skip(startrowindex).Take(maximumrows).ToList

               CacheData(key, li)
               Return li
               Return GetUserAnswers(startrowindex, maximumrows)
            End If
         Else
            Return Nothing
         End If
      End Function

      Public Shared Function GetUserAnswersByUser(ByVal user As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "Votes DESC") As List(Of mcc_UserAnswer)
         If Not String.IsNullOrEmpty(user) Then
            Dim key As String = "UserAnswers_ByUser_" & user & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
            If String.IsNullOrEmpty(sortExp) Then
               sortExp = "AddedDate DESC"
            End If
            If Cache(key) IsNot Nothing Then
               Dim li As List(Of mcc_UserAnswer) = DirectCast(Cache(key), List(Of mcc_UserAnswer))
               Return li
            Else
               Dim mdc As New MCCDataContext()
               Dim li As List(Of mcc_UserAnswer)
               startrowindex = IIf(startrowindex >= 0, startrowindex, 0)
               maximumrows = IIf(maximumrows > 0, maximumrows, 20)

               li = (From it As mcc_UserAnswer In mdc.mcc_UserAnswers Where it.AddedBy.ToLower = user.ToLower).SortBy(sortExp).Skip(startrowindex).Take(maximumrows).ToList

               CacheData(key, li)
               Return li
               Return GetUserAnswers(startrowindex, maximumrows)
            End If
         Else
            Return Nothing
         End If
      End Function

      Public Shared Function GetUserAnswerById(ByVal id As Integer) As mcc_UserAnswer
         Dim key As String = "UserAnswers_ById_" & id.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Dim fb As mcc_UserAnswer = DirectCast(Cache(key), mcc_UserAnswer)
            Return fb
         Else
            Dim mdc As New MCCDataContext()
            Dim fb As mcc_UserAnswer
            If mdc.mcc_UserAnswers.Count(Function(p) p.UserAnswerId = id) > 0 Then
               fb = (From it As mcc_UserAnswer In mdc.mcc_UserAnswers Where it.UserAnswerId = id).FirstOrDefault
               CacheData(key, fb)
               Return fb
            Else
               Return Nothing
            End If
         End If
      End Function

      Public Shared Function GetUserAnswersByQuestionId(ByVal userQuestionId As Integer) As List(Of mcc_UserAnswer)
         Dim key As String = "UserAnswers_ByQuestion_" & userQuestionId.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Dim fb As List(Of mcc_UserAnswer) = DirectCast(Cache(key), List(Of mcc_UserAnswer))
            Return fb
         Else
            Dim mdc As New MCCDataContext()
            Dim fb As List(Of mcc_UserAnswer)
            If mdc.mcc_UserAnswers.Count(Function(p) p.UserQuestionId = userQuestionId) > 0 Then
               fb = (From it As mcc_UserAnswer In mdc.mcc_UserAnswers Where it.UserAnswerId = userQuestionId).ToList
               CacheData(key, fb)
               Return fb
            Else
               Return Nothing
            End If
         End If
      End Function


      Public Shared Function InsertUserAnswer(ByVal userQuestionId As Integer, ByVal body As String) As Boolean
         Try
            Dim mdc As MCCDataContext = New MCCDataContext
            Dim fb As New mcc_UserAnswer()

            With fb
               .UserQuestionId = userQuestionId
               .Body = ConvertNullToEmptyString(body)
               .AddedDate = DateTime.Now
               .AddedBy = mccObject.CurrentUserName
            End With

            mdc.mcc_UserAnswers.InsertOnSubmit(fb)
            mccObject.PurgeCacheItems("UserQuestions_")
            mccObject.PurgeCacheItems("UserAnswers_")
            mdc.SubmitChanges()

            Return True
         Catch ex As Exception
            Return False
         End Try
      End Function


      Public Shared Sub UpdateUserAnswers(ByVal userAnswerId As Integer, ByVal body As String)
         Dim mdc As MCCDataContext = New MCCDataContext

         If mdc.mcc_UserAnswers.Count(Function(p) p.UserAnswerId = userAnswerId) > 0 Then
            Dim wrd As mcc_UserAnswer = (From t In mdc.mcc_UserAnswers _
                                        Where t.UserAnswerId = userAnswerId _
                                        Select t).Single()
            If wrd IsNot Nothing Then
               'wrd.Title = title
               wrd.Body = ConvertNullToEmptyString(body)
               ' do not update post slugs
               'wrd.Slug = routines.GetSlugFromString(title)
               mdc.SubmitChanges()
               mccObject.PurgeCacheItems("UserAnswers_")
               mccObject.PurgeCacheItems("UserQuestions_")
            End If
         End If
      End Sub


      Public Shared Sub DeleteAnswer(ByVal UserAnswerId As Integer)
         Dim mdc As MCCDataContext = New MCCDataContext
         If mdc.mcc_UserAnswers.Count(Function(p) p.UserAnswerId = UserAnswerId) > 0 Then
            Dim wrd As mcc_UserAnswer = (From t In mdc.mcc_UserAnswers _
                                        Where t.UserAnswerId = UserAnswerId _
                                        Select t).Single()
            If wrd IsNot Nothing Then
               mdc.mcc_UserAnswers.DeleteOnSubmit(wrd)
               mdc.SubmitChanges()
               mccObject.PurgeCacheItems("UserQuestions_")
               mccObject.PurgeCacheItems("UserAnswers_")
            End If
         End If
      End Sub

      Public Shared Function VoteUp(ByVal userAnswerId As Integer) As VoteJSON
         ' make sure the user can vote
         Dim mdc As New MCCDataContext


         If mdc.mcc_UserAnswers.Count(Function(p) p.UserAnswerId = userAnswerId) = 0 Then
            Return New VoteJSON(userAnswerId, 0, False, "The answer was not found")
         End If

         Dim uq As mcc_UserAnswer = (From it As mcc_UserAnswer In mdc.mcc_UserAnswers Where it.UserAnswerId = userAnswerId).FirstOrDefault

         If uq.AddedBy = mccObject.CurrentUserName Then
            Return New VoteJSON(userAnswerId, 0, False, "you cannot vote for your own answer")
         End If

         If HttpContext.Current.Request.IsAuthenticated Then
            ' check if the user already voted up
            Dim uname = mccObject.CurrentUserName
            If mdc.mcc_UsersAnswer_Votes.Count(Function(p) p.UserId.ToLower = uname.ToLower AndAlso p.UserAnswerId = userAnswerId) > 0 Then
               Dim vr As mcc_UsersAnswer_Vote = (From it As mcc_UsersAnswer_Vote In mdc.mcc_UsersAnswer_Votes Where it.UserId = uname).FirstOrDefault
               If Not vr.Helpful Then
                  vr.Helpful = True
                  uq.Votes += 2
                  mdc.SubmitChanges()
               Else
                  mdc.mcc_UsersAnswer_Votes.DeleteOnSubmit(vr)
                  uq.Votes -= 1
                  mdc.SubmitChanges()
               End If
               mccObject.PurgeCacheItems("UserAnswers_")
            Else
               Dim uv As New mcc_UsersAnswer_Vote
               uv.UserAnswerId = userAnswerId
               uv.UserId = uname
               uv.Helpful = True
               uq.Votes += 1
               mdc.mcc_UsersAnswer_Votes.InsertOnSubmit(uv)
               mdc.SubmitChanges()
               mccObject.PurgeCacheItems("UserAnswers_")
            End If

            Return New VoteJSON(userAnswerId, uq.Votes, True, "")
         End If

         Return New VoteJSON(userAnswerId, 0, False, "Unable to cast your vote at this time")

      End Function

      Public Shared Function VoteDown(ByVal userAnswerId As Integer) As VoteJSON
         ' make sure the user can vote
         Dim mdc As New MCCDataContext

         If mdc.mcc_UserAnswers.Count(Function(p) p.UserAnswerId = userAnswerId) = 0 Then

         End If

         Dim uq As mcc_UserAnswer = (From it As mcc_UserAnswer In mdc.mcc_UserAnswers Where it.UserAnswerId = userAnswerId).FirstOrDefault

         If uq.AddedBy = mccObject.CurrentUserName Then
            Return New VoteJSON(userAnswerId, 0, False, "you cannot vote for your own answer")
         End If

         If HttpContext.Current.Request.IsAuthenticated Then
            ' check if the user already voted up
            Dim uname = mccObject.CurrentUserName
            If mdc.mcc_UsersAnswer_Votes.Count(Function(p) p.UserId.ToLower = uname.ToLower AndAlso p.UserAnswerId = userAnswerId) > 0 Then
               Dim vr As mcc_UsersAnswer_Vote = (From it As mcc_UsersAnswer_Vote In mdc.mcc_UsersAnswer_Votes Where it.UserId = uname).FirstOrDefault
               If vr.Helpful Then
                  vr.Helpful = False
                  uq.Votes -= 2
                  mdc.SubmitChanges()
               Else
                  'Delete user vote!
                  mdc.mcc_UsersAnswer_Votes.DeleteOnSubmit(vr)
                  uq.Votes += 1
                  mdc.SubmitChanges()
               End If

               mccObject.PurgeCacheItems("UserAnswers_")
            Else
               Dim uv As New mcc_UsersAnswer_Vote
               uv.UserAnswerId = userAnswerId
               uv.UserId = uname
               uv.Helpful = False

               mdc.mcc_UsersAnswer_Votes.InsertOnSubmit(uv)
               uq.Votes -= 1
               mdc.SubmitChanges()
               mccObject.PurgeCacheItems("UserAnswers_")
            End If
            Return New VoteJSON(userAnswerId, uq.Votes, True, "")
         End If
         Return New VoteJSON(userAnswerId, 0, False, "Unable to cast your vote at this time")
      End Function


      Protected Shared Sub CacheData(ByVal key As String, ByVal data As Object)
         If data IsNot Nothing Then
            mccObject.Cache.Insert(key, data, Nothing, DateTime.Now.AddSeconds(3600), TimeSpan.Zero)
         End If
      End Sub
   End Class
End Namespace

