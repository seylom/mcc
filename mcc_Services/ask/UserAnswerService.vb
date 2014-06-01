Imports MCC.Data
Imports Webdiyer.WebControls.Mvc
Public Class UserAnswerService
   Inherits CacheObject
   Implements IUserAnswerService



   Private _userAnswerRepo As IUserAnswerRepository
   Private _userQuestionRepo As IUserQuestionRepository


   Public Sub New()
      Me.New(New UserAnswerRepository(), New UserQuestionRepository())
   End Sub


   Public Sub New(ByVal userAnswerRepo As IUserAnswerRepository, ByVal userQuestionRepo As IUserQuestionRepository)
      _userAnswerRepo = userAnswerRepo
      _userQuestionRepo = userQuestionRepo
   End Sub

   Public Function GetUserAnswersCount() As Integer Implements IUserAnswerService.GetUserAnswersCount
      Dim key As String = "UserAnswers_AnswerCount"

      If Cache(key) IsNot Nothing Then
         Return CInt(Cache(key))
      Else

         Dim it As Integer = _userAnswerRepo.GetUserAnswers.Count()
         CacheData(key, it)
         Return it
      End If
   End Function

   Public Function GetUserAnswersCountByUser(ByVal user As String) As Integer Implements IUserAnswerService.GetUserAnswersCountByUser

      If String.IsNullOrEmpty(user) Then
         Return 0
      End If

      Dim key As String = "UserAnswers_AnswerCountByUser_" & user
      If Cache(key) IsNot Nothing Then
         Return CInt(Cache(key))
      Else
         Dim it As Integer = _userAnswerRepo.GetUserAnswers.Count(Function(q) q.AddedBy.ToLower = user.ToLower)
         CacheData(key, it)
         Return it
      End If
   End Function

   Public Function GetUserAnswers() As List(Of UserAnswer) Implements IUserAnswerService.GetUserAnswers
      Dim key As String = "UserAnswers_UserAnswers_"
      If Cache(key) IsNot Nothing Then
         Dim li As List(Of UserAnswer) = DirectCast(Cache(key), List(Of UserAnswer))
         Return li
      Else

         Dim li As List(Of UserAnswer) = _userAnswerRepo.GetUserAnswers.ToList
         CacheData(key, li)
         Return li
      End If
   End Function

   Public Function GetUserAnswers(ByVal criteria As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AddedDate DESC") As PagedList(Of UserAnswer) Implements IUserAnswerService.GetUserAnswers
      If Not String.IsNullOrEmpty(criteria) Then
         Dim key As String = "UserAnswers_UserAnswers_" & criteria & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
         If Cache(key) IsNot Nothing Then
                Dim li As PagedList(Of UserAnswer) = DirectCast(Cache(key), PagedList(Of UserAnswer))
            Return li
         Else
            Dim li As PagedList(Of UserAnswer) = _userAnswerRepo.GetUserAnswers.Where(Function(it) it.Body.Contains(criteria)).SortBy(sortExp).ToPagedList(startrowindex, maximumrows)
            CacheData(key, li)
            Return li
         End If
      Else
         Return GetUserAnswers(startrowindex, maximumrows)
      End If
   End Function

   Public Function GetUserAnswers(ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AddedDate DESC") As PagedList(Of UserAnswer) Implements IUserAnswerService.GetUserAnswers

      If String.IsNullOrEmpty(sortExp) Then
         sortExp = "AddedDate DESC"
      End If

      Dim key As String = "UserAnswers_UserAnswers_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"

      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), PagedList(Of UserAnswer))
      Else
         Dim li As PagedList(Of UserAnswer) = _userAnswerRepo.GetUserAnswers.SortBy(sortExp).ToPagedList(startrowindex, maximumrows)
         CacheData(key, li)
         Return li
      End If
   End Function

   Public Function GetUserAnswersByUser(ByVal user As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AddedDate DESC") As PagedList(Of UserAnswer) Implements IUserAnswerService.GetUserAnswersByUser

      If String.IsNullOrEmpty(user) Then
         Return Nothing
      End If

      If String.IsNullOrEmpty(sortExp) Then
         sortExp = "AddedDate DESC"
      End If

      Dim key As String = "UserAnswers_UserAnswersByUser_" & user & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"

      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), PagedList(Of UserAnswer))
      Else
            Dim li As PagedList(Of UserAnswer) = _userAnswerRepo.GetUserAnswers.Where(Function(it) it.AddedBy.ToLower = user.ToLower).SortBy(sortExp).ToPagedList(startrowindex, maximumrows)
         CacheData(key, li)
         Return li
      End If
   End Function


   Public Function GetUserAnswerById(ByVal id As Integer) As UserAnswer Implements IUserAnswerService.GetUserAnswerById
      Dim key As String = "UserAnswers_UserAnswers_" & id.ToString & "_"
      If Cache(key) IsNot Nothing Then
         Dim fb As UserAnswer = DirectCast(Cache(key), UserAnswer)
         Return fb
      Else
         Dim fb As UserAnswer = _userAnswerRepo.GetUserAnswers.Where(Function(it) it.UserAnswerID = id).FirstOrDefault
         CacheData(key, fb)
         Return fb
      End If
   End Function


   Public Function GetUserAnswersByQuestionId(ByVal userQuestionId As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "Votes DESC") As PagedList(Of UserAnswer) Implements IUserAnswerService.GetUserAnswersByQuestionId
      If userQuestionId > 0 Then
         Dim key As String = "UserAnswers_ByQuestion_" & userQuestionId.ToString & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
         If String.IsNullOrEmpty(sortExp) Then
            sortExp = "Votes DESC"
         End If
         If Cache(key) IsNot Nothing Then
            Return DirectCast(Cache(key), PagedList(Of UserAnswer))
         Else
            Dim li As PagedList(Of UserAnswer) = _userAnswerRepo.GetUserAnswers.Where(Function(it) it.UserQuestionID = userQuestionId).SortBy(sortExp).ToPagedList(startrowindex, maximumrows)
            CacheData(key, li)
            Return li
         End If
      Else
         Return Nothing
      End If
   End Function


   Public Function InsertUserAnswer(ByVal uq As UserAnswer) As StatusItem Implements IUserAnswerService.InsertUserAnswer
      Dim sti As New StatusItem
      If uq IsNot Nothing Then
         With uq
            .AddedDate = DateTime.Now
            .AddedBy = CacheObject.CurrentUserName
         End With

         sti = _userAnswerRepo.InsertUserAnswer(uq)
         CacheObject.PurgeCacheItems("UserAnswers_")
      End If
      Return sti
   End Function


   Public Sub UpdateUserAnswers(ByVal uq As UserAnswer) Implements IUserAnswerService.UpdateUserAnswers
      If uq IsNot Nothing Then
         _userAnswerRepo.UpdateUserAnswer(uq)
         CacheObject.PurgeCacheItems("UserAnswers_")
      End If
   End Sub

   Public Function VoteUp(ByVal userAnswerId As Integer) As StatusItem Implements IUserAnswerService.VoteUp
      Dim sti As New StatusItem

      If userAnswerId > 0 AndAlso Not String.IsNullOrEmpty(CurrentUserName) Then
         sti = _userAnswerRepo.VoteAnswerUp(userAnswerId, CurrentUserName)
      End If
      CacheObject.PurgeCacheItems("UserAnswers_")

      Return sti
   End Function

   Public Function VoteDown(ByVal userAnswerId As Integer) As StatusItem Implements IUserAnswerService.VoteDown

      Dim sti As New StatusItem
      If userAnswerId > 0 AndAlso Not String.IsNullOrEmpty(CurrentUserName) Then
         sti = _userAnswerRepo.VoteAnswerDown(userAnswerId, CurrentUserName)
      End If

      CacheObject.PurgeCacheItems("UserAnswers_")

      Return sti
   End Function

   Public Sub DeleteAnswer(ByVal UserAnswerId As Integer) Implements IUserAnswerService.DeleteAnswer
      If UserAnswerId > 0 Then
         _userAnswerRepo.DeleteUserAnswer(UserAnswerId)
         CacheObject.PurgeCacheItems("UserAnswers_")
      End If
   End Sub

   '''' <summary>
   '''' If the answer was already accepted it will un-accept it!
   '''' </summary>
   '''' <param name="answerId"></param>
   '''' <param name="answerId"></param>
   '''' <remarks></remarks>
   'Public Sub SetAcceptedAnswer(ByVal answerId As Integer, ByVal answerId As Integer)

   '   If answerId > 0 And answerId > 0 Then
   '      Dim q As UserAnswer = _userAnswerRepo.GetUserAnswers.Where(Function(p) p.UserAnswerID = answerId).FirstOrDefault
   '      If q IsNot Nothing Then
   '         If q.BestUserAnswerID = answerId Then
   '            Return
   '         End If

   '         q.BestUserAnswerID = answerId

   '         'If _userAnswerRepo.GetUserAnswers.Count(Function(p) p.UserAnswerID = answerId AndAlso p.UserAnswerID = answerId) = 0 Then
   '         '   Return
   '         'End If
   '         'q.BestUserAnswerID = IIf(q.BestUserAnswerID = answerId, 0, answerId)

   '         _userAnswerRepo.UpdateUserAnswer(q)
   '         CacheObject.PurgeCacheItems("UserAnswers_")
   '      End If
   '   End If
   'End Sub

   'Public Function IsAnswer(ByVal answerId As Integer, ByVal answerId As Integer) As Boolean
   '   Dim key As String = "UserAnswers_UserAnswers_isAnswer_" & answerId.ToString & "_" & answerId.ToString
   '   If (Cache(key) IsNot Nothing) Then
   '      Return CBool(Cache(key))
   '   Else
   '      Dim bIsAnswer As Boolean = _userAnswerRepo.GetUserAnswers.Count(Function(p) p.UserAnswerID = answerId AndAlso p.BestUserAnswerID = answerId) > 0
   '      CacheData(key, bIsAnswer)
   '      Return bIsAnswer
   '   End If
   'End Function


   Public Sub IncrementViewCount(ByVal userAnswerId As Integer) Implements IUserAnswerService.IncrementViewCount
      'Dim mdc As New MCCDataContext
      'Dim uq = (From it As UserAnswer In mdc.useranswers Where it.UserAnswerID = userAnswerId).FirstOrDefault
      'uq.Views += 1
      'mdc.SubmitChanges()
      If userAnswerId > 0 Then
         Dim uq = _userAnswerRepo.GetUserAnswers.Where(Function(p) p.UserAnswerID = userAnswerId).FirstOrDefault()
         If uq IsNot Nothing Then
            uq.Votes += 1

            _userAnswerRepo.UpdateUserAnswer(uq)
            'CacheObject.PurgeCacheItems("UserAnswers_")
         End If
      End If
   End Sub

   Public Function GetUserAnswersCountByQuestionId(ByVal userQuestionId As Integer) As Integer Implements IUserAnswerService.GetUserAnswersCountByQuestionId
      Dim key As String = "UserAnswers_UserAnswers_AnswersCountByAnswers_" & userQuestionId.ToString
      If Cache(key) IsNot Nothing Then
         Return CInt(Cache(key))
      End If

      Dim val As Integer = _userAnswerRepo.GetUserAnswers.Count(Function(p) p.UserQuestionID = userQuestionId)
      CacheData(key, val)
      Return val
   End Function


   Public Function FindAnswersWithAnyMatch(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal SearchWord As String) As PagedList(Of UserAnswer) Implements IUserAnswerService.FindAnswersWithAnyMatch
      Dim key As String = "useranswer_find_" & SearchWord & "_" & startRowIndex.ToString & "_" & maximumRows.ToString

      If Cache(key) IsNot Nothing Then
         Return CType(Cache(key), PagedList(Of UserAnswer))
      End If

        Dim searchArray() As String = SearchWord.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)
      Dim li As PagedList(Of UserAnswer) = _userAnswerRepo.GetUserAnswers. _
                  Where(Function(it) it.Body.ContainsAny(searchArray, True)).ToPagedList(startRowIndex, maximumRows)
      CacheData(key, li)
      Return li
   End Function

   'Public Function FindAnswersWithAllMatch(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal SearchWord As String) As List(Of UserAnswer)

   'End Function

   Public Function FindAnswersWithExactMatch(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal SearchWord As String) As PagedList(Of UserAnswer) Implements IUserAnswerService.FindAnswersWithExactMatch
      Dim key As String = "useranswer_find_" & SearchWord & "_" & startRowIndex.ToString & "_" & maximumRows.ToString

      If Cache(key) IsNot Nothing Then
         Return CType(Cache(key), PagedList(Of UserAnswer))
      End If

      Dim li As PagedList(Of UserAnswer) = _userAnswerRepo.GetUserAnswers.Where(Function(it) it.Body = SearchWord).ToPagedList(startRowIndex, maximumRows)
      CacheData(key, li)
      Return li
   End Function


   ''' <summary>
   ''' Returns the number of total answers matching the search key
   ''' </summary>
   Public Function FindAnswersCount(ByVal searchWord As String) As Integer Implements IUserAnswerService.FindAnswersCount
      Dim key As String = String.Format("useranswer_find_{0}", searchWord)

      If Cache(key) IsNot Nothing Then
         Return CInt(Cache(key))
      End If

      Dim vdCount As Integer = 0
      vdCount = _userAnswerRepo.GetUserAnswers.Count(Function(p) p.Body.Contains(searchWord))
      CacheData(key, vdCount)
      Return vdCount
   End Function

   Public Sub DeleteAnswers(ByVal Ids() As Integer) Implements IUserAnswerService.DeleteAnswers
      If Ids IsNot Nothing Then
         _userAnswerRepo.DeleteUserAnswers(Ids)
         CacheObject.PurgeCacheItems("UserAnswers_")
      End If
   End Sub

   Protected Sub CacheData(ByVal key As String, ByVal data As Object)
      If data IsNot Nothing Then
         CacheObject.Cache.Insert(key, data, Nothing, DateTime.Now.AddSeconds(3600), TimeSpan.Zero)
      End If
   End Sub


End Class
