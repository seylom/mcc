Public Class UserQuestionRepository
   Implements IUserQuestionRepository



   Private _mdc As MCCDataContext

   Public Sub New()
      Me.New(New MCCDataContext)
   End Sub

   Public Sub New(ByVal dc As MCCDataContext)
      _mdc = dc
   End Sub
   Public Function GetUserQuestions() As IQueryable(Of UserQuestion) Implements IUserQuestionRepository.GetUserQuestions
      Dim q = From uq As mcc_UserQuestion In _mdc.mcc_UserQuestions _
              Select New UserQuestion With {.BestUserAnswerID = uq.BestUserAnswerId, _
                                          .AddedDate = uq.AddedDate, _
                                          .AddedBy = uq.Addedby, _
                                          .Body = uq.Body, _
                                          .Votes = uq.Votes, _
                                          .Title = uq.Title, _
                                          .Slug = uq.Slug, _
                                          .Views = uq.Views, _
                                          .ActivityNotification = If(uq.ActivityNotification, False), _
                                          .UserQuestionID = uq.UserQuestionId}
      Return q
   End Function
   Public Function InsertUserQuestion(ByVal uq As UserQuestion) As StatusItem Implements IUserQuestionRepository.InsertUserQuestion
      Dim sti As New StatusItem
      If uq Is Nothing Then
         sti.Message = "No question specified, please enter a question"
         Return sti
      End If

      Dim q As New mcc_UserQuestion

      With q
         .AddedDate = uq.AddedDate
         .Addedby = uq.AddedBy
         .Views = uq.Views
         .BestUserAnswerId = uq.BestUserAnswerID
         .Title = uq.Title
         .Slug = uq.Slug
         .Body = uq.Body
         .ActivityNotification = uq.ActivityNotification
         .Votes = uq.Votes
      End With
      _mdc.mcc_UserQuestions.InsertOnSubmit(q)
      _mdc.SubmitChanges()

      With sti
         .Data = q
         .Id = q.UserQuestionId
         .Success = True
      End With

      Return sti
   End Function
   Public Sub UpdateUserQuestion(ByVal uq As UserQuestion) Implements IUserQuestionRepository.UpdateUserQuestion
      If uq IsNot Nothing Then
         Dim q = _mdc.mcc_UserQuestions.Where(Function(p) p.UserQuestionId = uq.UserQuestionID).FirstOrDefault()

         If q IsNot Nothing Then
            With q
               q.Body = uq.Body
               q.Title = uq.Title
               q.Slug = uq.Slug
               q.Views = uq.Views
               q.Votes = uq.Votes
               .ActivityNotification = uq.ActivityNotification
               q.BestUserAnswerId = uq.BestUserAnswerID
            End With
         End If

         _mdc.SubmitChanges()
      End If
   End Sub
   Public Sub DeleteUserQuestion(ByVal uqId As Integer) Implements IUserQuestionRepository.DeleteUserQuestion
      If uqId > 0 Then
         Dim q = _mdc.mcc_UserQuestions.Where(Function(p) p.UserQuestionId = uqId).FirstOrDefault
         If q IsNot Nothing Then

            _mdc.mcc_UserQuestions.DeleteOnSubmit(q)

            ' delete associated commetns
            Dim v = _mdc.mcc_UserQuestionComments.Where(Function(p) p.UserQuestionId = uqId)
            If v IsNot Nothing Then
               _mdc.mcc_UserQuestionComments.DeleteAllOnSubmit(v)
            End If

            ' delete associated answers and their comments
            Dim w = _mdc.mcc_UserAnswers.Where(Function(p) p.UserQuestionId = uqId)
            Dim answerIds = w.Select(Function(a) a.UserAnswerId)

            If w IsNot Nothing Then
               _mdc.mcc_UserAnswers.DeleteAllOnSubmit(w)
            End If

            If answerIds IsNot Nothing AndAlso answerIds.Count > 0 Then
               Dim x = _mdc.mcc_UserAnswerComments.Where(Function(p) answerIds.Contains(p.UserAnswerId))
               If x IsNot Nothing Then
                  _mdc.mcc_UserAnswerComments.DeleteAllOnSubmit(x)
               End If
            End If


            _mdc.SubmitChanges()
         End If
      End If
   End Sub
   Public Sub DeleteUserQuestions(ByVal uqId As Integer()) Implements IUserQuestionRepository.DeleteUserQuestions
      If uqId IsNot Nothing AndAlso uqId.Count > 0 Then
         Dim q = _mdc.mcc_UserQuestions.Where(Function(p) uqId.Contains(p.UserQuestionId))
         If q IsNot Nothing Then

            _mdc.mcc_UserQuestions.DeleteAllOnSubmit(q)

            ' delete associated commetns
            Dim v = _mdc.mcc_UserQuestionComments.Where(Function(p) uqId.Contains(p.UserQuestionId))
            If v IsNot Nothing Then
               _mdc.mcc_UserQuestionComments.DeleteAllOnSubmit(v)
            End If
            _mdc.SubmitChanges()
         End If
      End If
   End Sub

   Public Function VoteQuestionDown(ByVal uqId As Integer, ByVal username As String) As StatusItem Implements IUserQuestionRepository.VoteQuestionDown
      Dim sti As New StatusItem

      If uqId <= 0 Then
         sti.Message = "No question was specified for this vote."
         Return sti
      End If

      Dim q = _mdc.mcc_UserQuestions.Where(Function(p) p.UserQuestionId = uqId).FirstOrDefault()
      If q Is Nothing Then
         sti.Message = "Unable to find the question your voted for."
         Return sti
      End If

      'can we vote?
      If q.Addedby = username Then
         sti.Message = "You cannot vote for your own question"
         Return sti
      End If

      Dim r = _mdc.mcc_UsersQuestion_Votes.Where(Function(p) p.UserQuestionId = uqId AndAlso p.UserId = username).FirstOrDefault()
      If r IsNot Nothing Then
         If Not r.Helpful Then
            q.Votes += 1
            _mdc.mcc_UsersQuestion_Votes.DeleteOnSubmit(r)
         Else
            r.Helpful = False
            q.Votes += -2
         End If
         _mdc.SubmitChanges()
      Else
         q.Votes -= 1
         Dim uvm As New mcc_UsersQuestion_Vote With {.UserId = username.ToLower, .Helpful = False, .UserQuestionId = uqId}
         _mdc.mcc_UsersQuestion_Votes.InsertOnSubmit(uvm)
      End If

      _mdc.SubmitChanges()

      sti.Success = True
      sti.Id = q.UserQuestionId
      sti.Data = q.Votes

      Return sti

   End Function

   Public Function VoteQuestionUp(ByVal uqId As Integer, ByVal username As String) As StatusItem Implements IUserQuestionRepository.VoteQuestionUp
      Dim sti As New StatusItem()

      If uqId <= 0 Then
         sti.Message = "No question was specified for this vote."
         Return sti
      End If

      Dim q = _mdc.mcc_UserQuestions.Where(Function(p) p.UserQuestionId = uqId).FirstOrDefault()
      If q Is Nothing Then
         sti.Message = "Unable to find your question. It may have been deleted"
         Return sti
      End If

      'can we vote?
      If q.Addedby = username Then
         sti.Message = "You cannot vote for your own question"
         Return sti
      End If

      Dim r = _mdc.mcc_UsersQuestion_Votes.Where(Function(p) p.UserQuestionId = uqId AndAlso p.UserId = username).FirstOrDefault()
      If r IsNot Nothing Then
         If r.Helpful Then
            q.Votes += -1
            _mdc.mcc_UsersQuestion_Votes.DeleteOnSubmit(r)
         Else
            r.Helpful = True
            q.Votes += 2
         End If
         _mdc.SubmitChanges()
      Else
         q.Votes += 1
         Dim uvm As New mcc_UsersQuestion_Vote With {.UserId = username.ToLower, .Helpful = True, .UserQuestionId = uqId}
         _mdc.mcc_UsersQuestion_Votes.InsertOnSubmit(uvm)
      End If
      _mdc.SubmitChanges()


      sti.Success = True
      sti.Id = q.UserQuestionId
      sti.Data = q.Votes

      Return sti
   End Function

   Public Function SubscribeOrUnsubscribeQuestion(ByVal questionId As Integer, ByVal username As String) As Boolean Implements IUserQuestionRepository.SubscribeOrUnsubscribeQuestion
      If _mdc.mcc_QuestionTrackings.Count(Function(p) p.UserQuestionId = questionId AndAlso p.UserId.ToLower = username) > 0 Then
         Dim q = _mdc.mcc_QuestionTrackings.Where(Function(p) p.UserQuestionId = questionId AndAlso p.UserId.ToLower = username).FirstOrDefault()
         _mdc.mcc_QuestionTrackings.DeleteOnSubmit(q)
         _mdc.SubmitChanges()
         Return False
      Else
         Dim uqt As New mcc_QuestionTracking With {.UserId = username, .UserQuestionId = questionId}
         _mdc.mcc_QuestionTrackings.InsertOnSubmit(uqt)
         _mdc.SubmitChanges()
         Return True
      End If
   End Function

   Public Function GetSubscribedQuestions(ByVal username As String) As System.Collections.Generic.IList(Of Integer) Implements IUserQuestionRepository.GetSubscribedQuestions
      Return _mdc.mcc_QuestionTrackings.Where(Function(p) p.UserId = username).Select(Function(q) q.UserQuestionId).ToList
   End Function

   Public Function IsSubscribed(ByVal questionId As Integer, ByVal username As String) As Boolean Implements IUserQuestionRepository.IsSubscribed
      If _mdc.mcc_QuestionTrackings.Count(Function(p) p.UserQuestionId = questionId AndAlso p.UserId.ToLower = username) > 0 Then
         Return True
      Else
         Return False
      End If
   End Function

   Public Function GetSubscribers(ByVal questionId As Integer) As System.Collections.Generic.IList(Of String) Implements IUserQuestionRepository.GetSubscribers
      Return _mdc.mcc_QuestionTrackings.Where(Function(p) p.UserQuestionId = questionId).Select(Function(q) q.UserId).ToList
   End Function
End Class
