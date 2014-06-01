Imports MCC.Data
Imports Webdiyer.WebControls.Mvc
Public Enum QuestionState
   Hot = 1
   Popular = 2
   Unanswered = 3
   All = 4
End Enum
Public Class UserQuestionService
   Inherits CacheObject
   Implements IUserQuestionService

   Private _userQuestionRepo As IUserQuestionRepository
   Private _userAnswerRepo As IUserAnswerRepository


   Public Sub New()
      Me.New(New UserQuestionRepository(), New UserAnswerRepository())
   End Sub


   Public Sub New(ByVal userQuestionRepo As IUserQuestionRepository, ByVal userAnswerRepo As IUserAnswerRepository)
      _userQuestionRepo = userQuestionRepo
      _userAnswerRepo = userAnswerRepo
   End Sub

   Public Function GetUserQuestionsCount() As Integer Implements IUserQuestionService.GetUserQuestionsCount
      Dim key As String = "UserQuestions_QuestionCount"

      If Cache(key) IsNot Nothing Then
         Return CInt(Cache(key))
      Else

         Dim it As Integer = _userQuestionRepo.GetUserQuestions.Count()
         CacheData(key, it)
         Return it
      End If
   End Function

   Public Function GetUserQuestionsCount(ByVal questionState As String) As Integer Implements IUserQuestionService.GetUserQuestionsCount

      If String.IsNullOrEmpty(questionState) Then
         Return GetUserQuestionsCount()
      End If

      Dim key As String = "UserQuestions_QuestionCountByState"

      If Cache(key) IsNot Nothing Then
         Return CInt(Cache(key))
      Else

         Dim it As Integer = 0
         Select Case questionState.ToLower
            Case "hot"
               it = 0
            Case "popular"
               it = _userQuestionRepo.GetUserQuestions.Count()
            Case "unanswered"
               it = _userQuestionRepo.GetUserQuestions.Count(Function(q) q.BestUserAnswerID = 0)
            Case Else
               it = _userQuestionRepo.GetUserQuestions.Count()
         End Select

         CacheData(key, it)
         Return it
      End If
   End Function

   Public Function GetUserQuestionsCountByUser(ByVal user As String) As Integer Implements IUserQuestionService.GetUserQuestionsCountByUser

      If String.IsNullOrEmpty(user) Then
         Return 0
      End If

      Dim key As String = "UserQuestions_QuestionCountByUser_" & user
      If Cache(key) IsNot Nothing Then
         Return CInt(Cache(key))
      Else
         Dim it As Integer = _userQuestionRepo.GetUserQuestions.Count(Function(q) q.AddedBy.ToLower = user.ToLower)
         CacheData(key, it)
         Return it
      End If
   End Function

   Public Function GetUserQuestions() As List(Of UserQuestion) Implements IUserQuestionService.GetUserQuestions
      Dim key As String = "UserQuestions_UserQuestions_"
      If Cache(key) IsNot Nothing Then
         Dim li As List(Of UserQuestion) = DirectCast(Cache(key), List(Of UserQuestion))
         Return li
      Else

         Dim li As List(Of UserQuestion) = _userQuestionRepo.GetUserQuestions.ToList
         CacheData(key, li)
         Return li
      End If
   End Function

   Public Function GetUserQuestions(ByVal criteria As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AddedDate DESC") As PagedList(Of UserQuestion) Implements IUserQuestionService.GetUserQuestions
      If Not String.IsNullOrEmpty(criteria) Then
         Dim key As String = "UserQuestions_UserQuestions_" & criteria & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Dim li As PagedList(Of UserQuestion) = DirectCast(Cache(key), PagedList(Of UserQuestion))
            Return li
         Else
            Dim li As PagedList(Of UserQuestion) = _userQuestionRepo.GetUserQuestions.Where(Function(it) it.Title.Contains(criteria)).SortBy(sortExp).ToPagedList(startrowindex, maximumrows)
            CacheData(key, li)
            Return li
         End If
      Else
         Return GetUserQuestions(startrowindex, maximumrows)
      End If
   End Function

   Public Function GetUserQuestions(ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AddedDate DESC") As PagedList(Of UserQuestion) Implements IUserQuestionService.GetUserQuestions

      If String.IsNullOrEmpty(sortExp) Then
         sortExp = "AddedDate DESC"
      End If

      Dim key As String = "UserQuestions_UserQuestions_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"

      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), PagedList(Of UserQuestion))
      Else
         Dim li As PagedList(Of UserQuestion) = _userQuestionRepo.GetUserQuestions.SortBy(sortExp).ToPagedList(startrowindex, maximumrows)
         CacheData(key, li)
         Return li
      End If
   End Function

   Public Function GetUserQuestionsByUser(ByVal user As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AddedDate DESC") As PagedList(Of UserQuestion) Implements IUserQuestionService.GetUserQuestionsByUser

      If String.IsNullOrEmpty(user) Then
         Return Nothing
      End If

      If String.IsNullOrEmpty(sortExp) Then
         sortExp = "AddedDate DESC"
      End If

      Dim key As String = "UserQuestions_UserQuestionsByUser_" & user & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"

      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), PagedList(Of UserQuestion))
      Else
         Dim li As PagedList(Of UserQuestion) = _userQuestionRepo.GetUserQuestions.Where(Function(it) it.AddedBy.ToLower = user.ToLower).SortBy(sortExp).ToPagedList(startrowindex, maximumrows)
         CacheData(key, li)
         Return li
      End If
   End Function

   Public Function GetUserQuestionsByState(ByVal uqState As QuestionState, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AddedDate DESC") As PagedList(Of UserQuestion) Implements IUserQuestionService.GetUserQuestionsByState

      If String.IsNullOrEmpty(sortExp) Then
         sortExp = "AddedDate DESC"
      End If

      Dim key As String = "UserQuestions_UserQuestions_" & uqState.ToString & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"

      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), PagedList(Of UserQuestion))
      Else

         Dim li As PagedList(Of UserQuestion)
         Select Case uqState
            Case QuestionState.Hot
               li = Nothing
            Case QuestionState.Unanswered
               li = _userQuestionRepo.GetUserQuestions.Where(Function(it) it.BestUserAnswerID = 0).SortBy(sortExp).ToPagedList(startrowindex, maximumrows)
            Case QuestionState.Popular
               sortExp = "Votes DESC"
               li = _userQuestionRepo.GetUserQuestions.SortBy(sortExp).ToPagedList(startrowindex, maximumrows)
            Case Else
               li = _userQuestionRepo.GetUserQuestions.SortBy(sortExp).ToPagedList(startrowindex, maximumrows)
         End Select

         CacheData(key, li)
         Return li
      End If
   End Function


   Public Function GetUserQuestionById(ByVal id As Integer) As UserQuestion Implements IUserQuestionService.GetUserQuestionById
      Dim key As String = "UserQuestions_UserQuestions_" & id.ToString & "_"
      If Cache(key) IsNot Nothing Then
         Dim fb As UserQuestion = DirectCast(Cache(key), UserQuestion)
         Return fb
      Else
         Dim fb As UserQuestion = _userQuestionRepo.GetUserQuestions.Where(Function(it) it.UserQuestionID = id).FirstOrDefault
         CacheData(key, fb)
         Return fb
      End If
   End Function

   Public Function GetUserQuestionSlugById(ByVal id As Integer) As String Implements IUserQuestionService.GetUserQuestionSlugById
      Dim key As String = "UserQuestions_UserQuestionsslug_" & id.ToString & "_"
      If Cache(key) IsNot Nothing Then
         Dim fb As String = Cache(key).ToString
         Return fb
      Else
         Dim fb As String = _userQuestionRepo.GetUserQuestions.Where(Function(it) it.UserQuestionID = id).Select(Function(p) p.Slug).FirstOrDefault
         CacheData(key, fb)
         Return fb
      End If
   End Function

   Public Function InsertUserQuestion(ByVal uq As UserQuestion) As StatusItem Implements IUserQuestionService.InsertUserQuestion
      Dim sti As New StatusItem()

      ' check for duplicated question with same title
      Dim q As Integer = _userQuestionRepo.GetUserQuestions.Count(Function(p) p.Title.ToLower.Trim = uq.Title.ToLower.Trim)
      If q > 0 Then
         sti.Message = "This is a duplicate of another question."
         sti.Success = False
         sti.Action = ErrorAction.Redirect
         Return sti
      End If

      ' insert question
      If uq IsNot Nothing Then
         With uq
            .Body = uq.Body
            .AddedDate = DateTime.Now
            .AddedBy = CacheObject.CurrentUserName
            .Slug = uq.Title.ToSlug()
         End With

         sti = _userQuestionRepo.InsertUserQuestion(uq)
         If sti.Success AndAlso uq.ActivityNotification Then
            _userQuestionRepo.SubscribeOrUnsubscribeQuestion(sti.Id, CacheObject.CurrentUserName)
         End If
         CacheObject.PurgeCacheItems("UserQuestions_")
      End If
      Return sti
   End Function


   Public Sub UpdateUserQuestions(ByVal uq As UserQuestion) Implements IUserQuestionService.UpdateUserQuestions
      If uq IsNot Nothing Then
         _userQuestionRepo.UpdateUserQuestion(uq)
         CacheObject.PurgeCacheItems("UserQuestions_")
      End If
   End Sub

   Public Function VoteUp(ByVal userQuestionId As Integer) As StatusItem Implements IUserQuestionService.VoteUp
      Dim sti As New StatusItem
      If userQuestionId > 0 AndAlso Not String.IsNullOrEmpty(CurrentUserName) Then
         sti = _userQuestionRepo.VoteQuestionUp(userQuestionId, CurrentUserName)
      End If

      CacheObject.PurgeCacheItems("UserQuestions_")
      Return sti
   End Function

   Public Function VoteDown(ByVal userQuestionId As Integer) As StatusItem Implements IUserQuestionService.VoteDown
      Dim sti As New StatusItem
      If userQuestionId > 0 AndAlso Not String.IsNullOrEmpty(CurrentUserName) Then
         sti = _userQuestionRepo.VoteQuestionDown(userQuestionId, CurrentUserName)
      End If

      CacheObject.PurgeCacheItems("UserQuestions_")

      Return sti
   End Function

   Public Sub DeleteQuestion(ByVal UserQuestionId As Integer) Implements IUserQuestionService.DeleteQuestion
      If UserQuestionId > 0 Then
         _userQuestionRepo.DeleteUserQuestion(UserQuestionId)
         CacheObject.PurgeCacheItems("UserQuestions_")
      End If
   End Sub

   ''' <summary>
   ''' If the answer was already accepted it will un-accept it!
   ''' </summary>
   ''' <param name="questionId"></param>
   ''' <param name="answerId"></param>
   ''' <remarks></remarks>
   Public Sub SetAcceptedAnswer(ByVal questionId As Integer, ByVal answerId As Integer) Implements IUserQuestionService.SetAcceptedAnswer

      If questionId > 0 And answerId > 0 Then
         Dim q As UserQuestion = _userQuestionRepo.GetUserQuestions.Where(Function(p) p.UserQuestionID = questionId).FirstOrDefault
         If q IsNot Nothing Then
            If q.BestUserAnswerID = answerId Then
               Return
            End If

            q.BestUserAnswerID = answerId

            'If _userAnswerRepo.GetUserAnswers.Count(Function(p) p.UserAnswerID = answerId AndAlso p.UserQuestionID = questionId) = 0 Then
            '   Return
            'End If
            'q.BestUserAnswerID = IIf(q.BestUserAnswerID = answerId, 0, answerId)

            _userQuestionRepo.UpdateUserQuestion(q)
            CacheObject.PurgeCacheItems("UserQuestions_")
         End If
      End If
   End Sub

   'Public Function IsAnswer(ByVal questionId As Integer, ByVal answerId As Integer) As Boolean
   '   Dim key As String = "UserQuestions_UserQuestions_isAnswer_" & questionId.ToString & "_" & answerId.ToString
   '   If (Cache(key) IsNot Nothing) Then
   '      Return CBool(Cache(key))
   '   Else
   '      Dim bIsAnswer As Boolean = _userQuestionRepo.GetUserQuestions.Count(Function(p) p.UserQuestionID = questionId AndAlso p.BestUserAnswerID = answerId) > 0
   '      CacheData(key, bIsAnswer)
   '      Return bIsAnswer
   '   End If
   'End Function


   Public Sub IncrementViewCount(ByVal userQuestionId As Integer) Implements IUserQuestionService.IncrementViewCount
      'Dim mdc As New MCCDataContext
      'Dim uq = (From it As UserQuestion In mdc.userquestions Where it.UserQuestionID = userQuestionId).FirstOrDefault
      'uq.Views += 1
      'mdc.SubmitChanges()
      If userQuestionId > 0 Then
         Dim uq = _userQuestionRepo.GetUserQuestions.Where(Function(p) p.UserQuestionID = userQuestionId).FirstOrDefault()
         If uq IsNot Nothing Then
            uq.Views += 1

            _userQuestionRepo.UpdateUserQuestion(uq)
            'CacheObject.PurgeCacheItems("UserQuestions_")
         End If
      End If
   End Sub

   Public Function GetUserAnswersCountById(ByVal userQuestionId As Integer) As Integer Implements IUserQuestionService.GetUserAnswersCountById
      Dim key As String = "UserQuestions_UserQuestions_AnswersCountByQuestions_" & userQuestionId.ToString
      If Cache(key) IsNot Nothing Then
         Return CInt(Cache(key))
      End If

      Dim val As Integer = _userAnswerRepo.GetUserAnswers.Count(Function(p) p.UserQuestionID = userQuestionId)
      CacheData(key, val)
      Return val
   End Function


   Public Function FindQuestionsWithAnyMatch(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal SearchWord As String) As PagedList(Of UserQuestion) Implements IUserQuestionService.FindQuestionsWithAnyMatch
      Dim key As String = "userquestion_find_" & SearchWord & "_" & startRowIndex.ToString & "_" & maximumRows.ToString

      If Cache(key) IsNot Nothing Then
         Return CType(Cache(key), PagedList(Of UserQuestion))
      End If

      Dim searchArray() As String = SearchWord.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)
      Dim li As PagedList(Of UserQuestion) = _userQuestionRepo.GetUserQuestions. _
                  Where(Function(it) it.Title.ContainsAny(searchArray, True) Or it.Body.ContainsAny(searchArray, True)).ToPagedList(startRowIndex, maximumRows)
      CacheData(key, li)
      Return li
   End Function

   'Public Function FindQuestionsWithAllMatch(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal SearchWord As String) As List(Of UserQuestion)

   'End Function

   Public Function FindQuestionsWithExactMatch(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal SearchWord As String) As PagedList(Of UserQuestion) Implements IUserQuestionService.FindQuestionsWithExactMatch
      Dim key As String = "userquestion_find_" & SearchWord & "_" & startRowIndex.ToString & "_" & maximumRows.ToString

      If Cache(key) IsNot Nothing Then
         Return CType(Cache(key), PagedList(Of UserQuestion))
      End If

      Dim li As PagedList(Of UserQuestion) = _userQuestionRepo.GetUserQuestions.Where(Function(it) it.Title = SearchWord Or it.Body = SearchWord).ToPagedList(startRowIndex, maximumRows)
      CacheData(key, li)
      Return li
   End Function

   'Public Function FindQuestions(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal SearchWord As String) As List(Of UserQuestion)
   '   Dim key As String = "userquestion_find_" & SearchWord & "_" & startRowIndex.ToString & "_" & maximumRows.ToString

   '   If Cache(key) IsNot Nothing Then
   '      Return CType(Cache(key), List(Of UserQuestion))
   '   End If


   '   Dim li As List(Of UserQuestion) = (From it As UserQuestion In mdc.userquestions Where it.Title.Contains(SearchWord) Or it.Body.Contains(SearchWord)).ToList

   '   CacheData(key, li)

   '   Return li
   'End Function

   ''' <summary>
   ''' Returns the number of total questions matching the search key
   ''' </summary>
   Public Function FindQuestionsCount(ByVal searchWord As String) As Integer Implements IUserQuestionService.FindQuestionsCount
      Dim key As String = String.Format("userquestion_find_{0}", searchWord)

      If Cache(key) IsNot Nothing Then
         Return CInt(Cache(key))
      End If

      Dim vdCount As Integer = 0
      vdCount = _userQuestionRepo.GetUserQuestions.Count(Function(p) p.Body.Contains(searchWord) Or p.Title.Contains(searchWord))
      CacheData(key, vdCount)
      Return vdCount
   End Function



   Public Sub DeleteQuestions(ByVal Ids() As Integer) Implements IUserQuestionService.DeleteQuestions
      If Ids IsNot Nothing Then
         _userQuestionRepo.DeleteUserQuestions(Ids)
         CacheObject.PurgeCacheItems("UserQuestions_")
      End If
   End Sub

   Public Function SubscribeOrUnsubscribe(ByVal questionId As Integer, ByVal username As String) As Boolean Implements IUserQuestionService.SubscribeOrUnsubscribe
      Return _userQuestionRepo.SubscribeOrUnsubscribeQuestion(questionId, username)
   End Function

   Public Function GetSubscribedQuestions(ByVal username As String) As System.Collections.Generic.IList(Of Integer) Implements IUserQuestionService.GetSubscribedQuestions
      Return _userQuestionRepo.GetSubscribedQuestions(username)
   End Function

   Public Function IsSubscribed(ByVal questionId As Integer, ByVal username As String) As Boolean Implements IUserQuestionService.IsSubscribed
      Return _userQuestionRepo.IsSubscribed(questionId, username)
   End Function

   Public Function GetSubscribers(ByVal questionId As Integer) As System.Collections.Generic.IList(Of String) Implements IUserQuestionService.GetSubscribers
      Return _userQuestionRepo.GetSubscribers(questionId)
   End Function

   Protected Sub CacheData(ByVal key As String, ByVal data As Object)
      If data IsNot Nothing Then
         CacheObject.Cache.Insert(key, data, Nothing, DateTime.Now.AddSeconds(3600), TimeSpan.Zero)
      End If
   End Sub


End Class

