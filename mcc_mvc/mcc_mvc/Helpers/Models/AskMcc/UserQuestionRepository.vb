Imports Microsoft.VisualBasic
Imports System.Linq

Imports MCC.Data

Namespace UserQuestions
   Public Class UserQuestionRepository
      Inherits mccObject

      Public Shared Function GetUserQuestionsCount() As Integer
         Dim key As String = "UserQuestions_QuestionCount"

         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            Dim mdc As New MCCDataContext()
            Dim it As Integer = mdc.mcc_UserQuestions.Count()
            CacheData(key, it)
            Return it
         End If
      End Function

      Public Shared Function GetUserQuestionsCount(ByVal questionState As String) As Integer

         If String.IsNullOrEmpty(questionState) Then
            Return GetUserQuestionsCount()
         End If

         Dim key As String = "UserQuestions_QuestionCountByState"

         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            Dim mdc As New MCCDataContext()
            Dim it As Integer = 0
            Select Case questionState.ToLower
               Case "hot"
                  it = 0
               Case "popular"
                  it = mdc.mcc_UserQuestions.Count()
               Case "unanswered"
                  it = mdc.mcc_UserQuestions.Count(Function(q) q.BestUserAnswerId = 0)
               Case Else
                  it = mdc.mcc_UserQuestions.Count()
            End Select

            CacheData(key, it)
            Return it
         End If
      End Function

      Public Shared Function GetUserQuestionsCountByUser(ByVal user As String) As Integer

         If String.IsNullOrEmpty(user) Then
            Return 0
         End If

         Dim key As String = "UserQuestions_QuestionCountByUser_" & user

         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            Dim mdc As New MCCDataContext()
            Dim it As Integer = mdc.mcc_UserQuestions.Count(Function(q) q.Addedby.ToLower = user.ToLower)
            CacheData(key, it)
            Return it
         End If
      End Function

      'Public Shared Function GetQuestionCount(ByVal criteria As String) As Integer
      '   If Not String.IsNullOrEmpty(criteria) Then
      '      Dim mdc As New MCCDataContext()
      '      Dim key As String = "UserQuestions_QuestionCount_" & criteria & "_"

      '      If Cache(key) IsNot Nothing Then
      '         Return CInt(Cache(key))
      '      Else
      '         Dim it As Integer = mdc.mcc_userquestions.Count(Function(p) p.Description.Contains(criteria) Or p.Title.Contains(criteria))
      '         CacheData(key, it)
      '         Return it
      '      End If
      '   Else
      '      Return GetQuestionCount()
      '   End If
      'End Function


      Public Shared Function GetUserQuestions() As List(Of mcc_UserQuestion)
         Dim key As String = "UserQuestions_UserQuestions_"
         If Cache(key) IsNot Nothing Then
            Dim li As List(Of mcc_UserQuestion) = DirectCast(Cache(key), List(Of mcc_UserQuestion))
            Return li
         Else
            Dim mdc As New MCCDataContext()
            Dim li As List(Of mcc_UserQuestion) = mdc.mcc_UserQuestions.ToList
            CacheData(key, li)
            Return li
         End If
      End Function

      Public Shared Function GetUserQuestions(ByVal criteria As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "ImageId ASC") As List(Of mcc_UserQuestion)
         If Not String.IsNullOrEmpty(criteria) Then
            Dim key As String = "UserQuestions_UserQuestions_" & criteria & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
            If Cache(key) IsNot Nothing Then
               Dim li As List(Of mcc_UserQuestion) = DirectCast(Cache(key), List(Of mcc_UserQuestion))
               Return li
            Else
               Dim mdc As New MCCDataContext()
               Dim li As List(Of mcc_UserQuestion)
               If startrowindex >= 0 AndAlso maximumrows > 0 Then
                  li = (From it As mcc_UserQuestion In mdc.mcc_UserQuestions Where it.Title.Contains(criteria)).Skip(startrowindex).Take(maximumrows).ToList
               Else
                  li = (From it As mcc_UserQuestion In mdc.mcc_UserQuestions Where it.Title.Contains(criteria)).Skip(0).Take(10).ToList
               End If
               CacheData(key, li)
               Return li
            End If
         Else
            Return GetUserQuestions(startrowindex, maximumrows)
         End If
      End Function

      Public Shared Function GetUserQuestions(ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "title ASC") As List(Of mcc_UserQuestion)

         If String.IsNullOrEmpty(sortExp) Then
            sortExp = "AddedDate DESC"
         End If

         Dim key As String = "UserQuestions_UserQuestions_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"

         If Cache(key) IsNot Nothing Then
            Dim li As List(Of mcc_UserQuestion) = DirectCast(Cache(key), List(Of mcc_UserQuestion))
            Return li
         Else
            Dim mdc As New MCCDataContext()
            Dim li As List(Of mcc_UserQuestion)

            startrowindex = IIf(startrowindex >= 0, startrowindex, 0)
            maximumrows = IIf(maximumrows > 0, maximumrows, 30)

            li = mdc.mcc_UserQuestions.SortBy(sortExp).Skip(startrowindex).Take(maximumrows).ToList

            CacheData(key, li)
            Return li
         End If
      End Function

      Public Shared Function GetUserQuestionsByUser(ByVal user As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "title ASC") As List(Of mcc_UserQuestion)

         If String.IsNullOrEmpty(user) Then
            Return Nothing
         End If

         If String.IsNullOrEmpty(sortExp) Then
            sortExp = "AddedDate DESC"
         End If

         Dim key As String = "UserQuestions_UserQuestionsByUser_" & user & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"

         If Cache(key) IsNot Nothing Then
            Dim li As List(Of mcc_UserQuestion) = DirectCast(Cache(key), List(Of mcc_UserQuestion))
            Return li
         Else
            Dim mdc As New MCCDataContext()
            Dim li As List(Of mcc_UserQuestion)

            startrowindex = IIf(startrowindex >= 0, startrowindex, 0)
            maximumrows = IIf(maximumrows > 0, maximumrows, 30)

            li = (From it As mcc_UserQuestion In mdc.mcc_UserQuestions Where it.Addedby.ToLower = user.ToLower).SortBy(sortExp).Skip(startrowindex).Take(maximumrows).ToList

            CacheData(key, li)
            Return li
         End If
      End Function

      Public Shared Function GetUserQuestionsByState(ByVal questionState As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "title ASC") As List(Of mcc_UserQuestion)

         If String.IsNullOrEmpty(questionState) Then
            Return GetUserQuestions(startrowindex, maximumrows, sortExp)
         End If

         If String.IsNullOrEmpty(sortExp) Then
            sortExp = "AddedDate DESC"
         End If

         Dim key As String = "UserQuestions_UserQuestions_" & questionState & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"

         If Cache(key) IsNot Nothing Then
            Dim li As List(Of mcc_UserQuestion) = DirectCast(Cache(key), List(Of mcc_UserQuestion))
            Return li
         Else
            Dim mdc As New MCCDataContext()
            Dim li As List(Of mcc_UserQuestion)

            startrowindex = IIf(startrowindex >= 0, startrowindex, 0)
            maximumrows = IIf(maximumrows > 0, maximumrows, 30)

            Select Case questionState.ToLower
               Case "hot"
                  li = Nothing
               Case "unanswered"
                  li = (From it As mcc_UserQuestion In mdc.mcc_UserQuestions Where it.BestUserAnswerId = 0).SortBy(sortExp).Skip(startrowindex).Take(maximumrows).ToList
               Case "popular"
                  sortExp = "Votes DESC"
                  li = mdc.mcc_UserQuestions.SortBy(sortExp).Skip(startrowindex).Take(maximumrows).ToList
               Case Else
                  li = mdc.mcc_UserQuestions.SortBy(sortExp).Skip(startrowindex).Take(maximumrows).ToList
            End Select

            CacheData(key, li)
            Return li
         End If
      End Function


      Public Shared Function GetUserQuestionById(ByVal id As Integer) As mcc_UserQuestion
         Dim key As String = "UserQuestions_UserQuestions_" & id.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Dim fb As mcc_UserQuestion = DirectCast(Cache(key), mcc_UserQuestion)
            Return fb
         Else
            Dim mdc As New MCCDataContext()
            Dim fb As mcc_UserQuestion = (From it As mcc_UserQuestion In mdc.mcc_UserQuestions Where it.UserQuestionId = id).FirstOrDefault
            CacheData(key, fb)
            Return fb
         End If
      End Function

      Public Shared Function GetUserQuestionSlugById(ByVal id As Integer) As String
         Dim key As String = "UserQuestions_UserQuestionsslug_" & id.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Dim fb As String = Cache(key).ToString
            Return fb
         Else
            Dim mdc As New MCCDataContext()
            Dim fb As String = (From it As mcc_UserQuestion In mdc.mcc_UserQuestions Where it.UserQuestionId = id Select it.Slug).FirstOrDefault
            CacheData(key, fb)
            Return fb
         End If
      End Function

      Public Shared Sub InsertUserQuestion(ByVal title As String, ByVal body As String)
         Dim mdc As MCCDataContext = New MCCDataContext
         Dim fb As New mcc_UserQuestion()

         With fb
            .Title = title
            .Body = ConvertNullToEmptyString(body)
            .AddedDate = DateTime.Now
            .Addedby = mccObject.CurrentUserName
            .Slug = routines.GetSlugFromString(title)
         End With

         mdc.mcc_UserQuestions.InsertOnSubmit(fb)
         mccObject.PurgeCacheItems("UserQuestions_")
         mdc.SubmitChanges()
      End Sub


      Public Shared Sub UpdateUserQuestions(ByVal userQuestionId As Integer, ByVal title As String, ByVal body As String)
         Dim mdc As MCCDataContext = New MCCDataContext

         Dim wrd As mcc_UserQuestion = (From t In mdc.mcc_UserQuestions _
                                     Where t.UserQuestionId = userQuestionId _
                                     Select t).FirstOrDefault
         If wrd IsNot Nothing Then
            wrd.Title = title
            wrd.Body = ConvertNullToEmptyString(body)

            wrd.Slug = routines.GetSlugFromString(title)
            mdc.SubmitChanges()
            mccObject.PurgeCacheItems("UserQuestions_")
         End If
      End Sub

      Public Shared Function VoteUp(ByVal userQuestionId As Integer) As VoteJSON
         ' make sure the user can vote

         Dim mdc As New MCCDataContext
         If mdc.mcc_UserQuestions.Count(Function(p) p.UserQuestionId = userQuestionId) = 0 Then
            Return New VoteJSON(userQuestionId, 0, False, "The question was not found")
         End If

         Dim uq As mcc_UserQuestion = (From it As mcc_UserQuestion In mdc.mcc_UserQuestions Where it.UserQuestionId = userQuestionId).FirstOrDefault

         If uq.Addedby = mccObject.CurrentUserName Then
            Return New VoteJSON(userQuestionId, 0, False, "you cannot vote for your own question")
         End If

         If HttpContext.Current.Request.IsAuthenticated Then
            ' check if the user already voted up
            Dim uname = mccObject.CurrentUserName

            Dim vr As mcc_UsersQuestion_Vote = (From it As mcc_UsersQuestion_Vote In mdc.mcc_UsersQuestion_Votes Where it.UserId = uname).FirstOrDefault
            If vr IsNot Nothing Then
               If Not vr.Helpful Then
                  vr.Helpful = True
                  uq.Votes += 2
                  mdc.SubmitChanges()
               Else
                  mdc.mcc_UsersQuestion_Votes.DeleteOnSubmit(vr)
                  uq.Votes -= 1
                  mdc.SubmitChanges()
               End If
               mccObject.PurgeCacheItems("UserQuestions_")
            Else
               Dim uv As New mcc_UsersQuestion_Vote
               uv.UserQuestionId = userQuestionId
               uv.UserId = uname
               uv.Helpful = True
               uq.Votes += 1
               mdc.mcc_UsersQuestion_Votes.InsertOnSubmit(uv)
               mdc.SubmitChanges()
               mccObject.PurgeCacheItems("UserQuestions_")
            End If

            Return New VoteJSON(userQuestionId, uq.Votes, True, "")
         End If

         Return New VoteJSON(userQuestionId, 0, False, "Unable to cast your vote at this time")

      End Function

      Public Shared Function VoteDown(ByVal userQuestionId As Integer) As VoteJSON
         ' make sure the user can vote
         Dim mdc As New MCCDataContext

         If mdc.mcc_UserQuestions.Count(Function(p) p.UserQuestionId = userQuestionId) = 0 Then
            Return New VoteJSON(userQuestionId, 0, False, "The question was not found")
         End If

         Dim uq As mcc_UserQuestion = (From it As mcc_UserQuestion In mdc.mcc_UserQuestions Where it.UserQuestionId = userQuestionId).FirstOrDefault

         If uq.Addedby = mccObject.CurrentUserName Then
            Return New VoteJSON(userQuestionId, 0, False, "you cannot vote for your own question")
         End If

         If HttpContext.Current.Request.IsAuthenticated Then
            ' check if the user already voted up
            Dim uname = mccObject.CurrentUserName
            Dim vr As mcc_UsersQuestion_Vote = (From it As mcc_UsersQuestion_Vote In mdc.mcc_UsersQuestion_Votes Where it.UserId = uname).FirstOrDefault
            If vr IsNot Nothing Then
               If vr.Helpful Then
                  vr.Helpful = False
                  uq.Votes -= 2
                  mdc.SubmitChanges()
               Else
                  'Delete user vote!
                  mdc.mcc_UsersQuestion_Votes.DeleteOnSubmit(vr)
                  uq.Votes += 1
                  mdc.SubmitChanges()
               End If

               mccObject.PurgeCacheItems("UserQuestions_")
            Else
               Dim uv As New mcc_UsersQuestion_Vote
               uv.UserQuestionId = userQuestionId
               uv.UserId = uname
               uv.Helpful = False

               mdc.mcc_UsersQuestion_Votes.InsertOnSubmit(uv)
               uq.Votes -= 1
               mdc.SubmitChanges()
               mccObject.PurgeCacheItems("UserQuestions_")
            End If

            Return New VoteJSON(userQuestionId, uq.Votes, True, "")
         End If

         Return New VoteJSON(userQuestionId, 0, False, "Unable to cast your vote at this time")

      End Function

      Public Shared Sub DeleteQuestion(ByVal UserQuestionId As Integer)
         Dim mdc As MCCDataContext = New MCCDataContext
         Dim wrd As mcc_UserQuestion = (From t In mdc.mcc_UserQuestions _
                                        Where t.UserQuestionId = UserQuestionId _
                                        Select t).Single()
         If wrd IsNot Nothing Then
            mdc.mcc_UserQuestions.DeleteOnSubmit(wrd)
            mdc.SubmitChanges()
            mccObject.PurgeCacheItems("UserQuestions_")
         End If
      End Sub

      ''' <summary>
      ''' If the answer was already accepted it will un-accept it!
      ''' </summary>
      ''' <param name="questionId"></param>
      ''' <param name="answerId"></param>
      ''' <remarks></remarks>
      Public Shared Function AcceptAnswer(ByVal questionId As Integer, ByVal answerId As Integer) As Boolean
         Dim mdc As MCCDataContext = New MCCDataContext
         ' make sure the answer exists
         Dim wrd As mcc_UserQuestion = (From t In mdc.mcc_UserQuestions _
                                     Where t.UserQuestionId = questionId _
                                     Select t).FirstOrDefault
         If wrd Is Nothing Then
            Return False
         End If

         ' make sure the answer exists
         If mdc.mcc_UserAnswers.Count(Function(p) p.UserAnswerId = answerId AndAlso p.UserQuestionId = questionId) = 0 Then
            Return False
         End If

         If answerId = 0 Then
            Return False
         End If

         wrd.BestUserAnswerId = IIf(wrd.BestUserAnswerId = answerId, 0, answerId)



         mdc.SubmitChanges()
         mccObject.PurgeCacheItems("UserQuestions_")

         Return wrd.BestUserAnswerId = answerId

      End Function

      Public Shared Function IsAnswer(ByVal questionId As Integer, ByVal answerId As Integer) As Boolean
         Dim key As String = "UserQuestions_UserQuestions_isAnswer_" & questionId.ToString & "_" & answerId.ToString
         If (Cache(key) IsNot Nothing) Then
            Return CBool(Cache(key))
         Else
            Dim mdc As New MCCDataContext()
            Dim bIsAnswer As Boolean = (questionId > 0 AndAlso answerId > 0 AndAlso mdc.mcc_UserQuestions.Count(Function(p) p.UserQuestionId = questionId AndAlso p.BestUserAnswerId = answerId) > 0)

            CacheData(key, bIsAnswer)
            Return bIsAnswer
         End If
      End Function


      Public Shared Sub IncrementViewCount(ByVal userQuestionId As Integer)
         Dim mdc As New MCCDataContext
         Dim uq = (From it As mcc_UserQuestion In mdc.mcc_UserQuestions Where it.UserQuestionId = userQuestionId).FirstOrDefault
         uq.Views += 1
         mdc.SubmitChanges()
      End Sub
      Public Shared Function GetUserAnswersCountById(ByVal userQuestionId As Integer) As Integer
         Dim key As String = "UserQuestions_UserQuestions_AnswersCountByQuestions_" & userQuestionId.ToString
         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            Dim val As Integer = 0
            Dim mdc As New MCCDataContext
            val = mdc.mcc_UserAnswers.Count(Function(p) p.UserQuestionId = userQuestionId)
            CacheData(key, val)
            Return val
         End If
      End Function


      Public Shared Function FindQuestions(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal SearchWord As String) As List(Of mcc_UserQuestion)
         Dim key As String = "userquestion_find_" & SearchWord & "_" & startRowIndex.ToString & "_" & maximumRows.ToString

         If Cache(key) IsNot Nothing Then
            Return CType(Cache(key), List(Of mcc_UserQuestion))
         End If

         Dim mdc As New MCCDataContext()
         Dim li As List(Of mcc_UserQuestion) = (From it As mcc_UserQuestion In mdc.mcc_UserQuestions Where it.Title.Contains(SearchWord) Or it.Body.Contains(SearchWord)).ToList

         CacheData(key, li)

         Return li
      End Function

      ''' <summary>
      ''' Returns the number of total questions matching the search key
      ''' </summary>
      Public Shared Function FindQuestionsCount(ByVal searchWord As String) As Integer
         Dim vdCount As Integer = 0

         Dim mdc As New MCCDataContext()
         vdCount = mdc.mcc_UserQuestions.Count(Function(p) p.Body.Contains(searchWord) Or p.Title.Contains(searchWord))
         Return vdCount
      End Function

      Protected Shared Sub CacheData(ByVal key As String, ByVal data As Object)
         If data IsNot Nothing Then
            mccObject.Cache.Insert(key, data, Nothing, DateTime.Now.AddSeconds(3600), TimeSpan.Zero)
         End If
      End Sub
   End Class
End Namespace

