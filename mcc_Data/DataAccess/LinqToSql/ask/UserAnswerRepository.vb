Public Class UserAnswerRepository
   Implements IUserAnswerRepository

   Private _mdc As MCCDataContext

   Public Sub New()
      Me.New(New MCCDataContext)
   End Sub

   Public Sub New(ByVal dc As MCCDataContext)
      _mdc = dc
   End Sub
   Public Function GetUserAnswers() As IQueryable(Of UserAnswer) Implements IUserAnswerRepository.GetUserAnswers
      Dim q = From uq As mcc_UserAnswer In _mdc.mcc_UserAnswers _
              Select New UserAnswer With {.UserAnswerID = uq.UserAnswerId, _
                                          .AddedDate = uq.AddedDate, _
                                          .AddedBy = uq.AddedBy, _
                                          .Body = uq.Body, _
                                          .Votes = uq.Votes, _
                                          .RelevanceFactor = uq.RelevanceFactor, _
                                          .UserQuestionID = uq.UserQuestionId, _
                                          .QuestionTitle = uq.mcc_UserQuestion.Title, _
                                          .QuestionSlug = uq.mcc_UserQuestion.Slug}
      Return q
   End Function
   Public Function InsertUserAnswer(ByVal uq As UserAnswer) As StatusItem Implements IUserAnswerRepository.InsertUserAnswer
      Dim sti As New StatusItem
      If uq Is Nothing Then

         sti.Message = "No answer specified, please write an answer"
         Return sti
      End If

      Dim q As New mcc_UserAnswer

      With q
         .AddedDate = uq.AddedDate
         .AddedBy = uq.AddedBy
         .RelevanceFactor = uq.RelevanceFactor
         .UserQuestionId = uq.UserQuestionID
         .Body = uq.Body
         .Votes = uq.Votes
      End With

      _mdc.mcc_UserAnswers.InsertOnSubmit(q)
      _mdc.SubmitChanges()

      sti.Success = True
      sti.Id = q.UserAnswerId
      sti.Data = q

      Return sti
   End Function
   Public Sub UpdateUserAnswer(ByVal uq As UserAnswer) Implements IUserAnswerRepository.UpdateUserAnswer
      If uq IsNot Nothing Then
         Dim q = _mdc.mcc_UserAnswers.Where(Function(p) p.UserAnswerId = uq.UserAnswerID).FirstOrDefault()

         If q IsNot Nothing Then
            With q
               q.Body = uq.Body
               q.RelevanceFactor = uq.RelevanceFactor
               q.Votes = uq.Votes
            End With
         End If

         _mdc.SubmitChanges()
      End If
   End Sub
   Public Sub DeleteUserAnswer(ByVal uqId As Integer) Implements IUserAnswerRepository.DeleteUserAnswer
      If uqId > 0 Then
         Dim q = _mdc.mcc_UserAnswers.Where(Function(p) p.UserAnswerId = uqId).FirstOrDefault
         If q IsNot Nothing Then

            _mdc.mcc_UserAnswers.DeleteOnSubmit(q)

            ' delete associated commetns
            Dim v = _mdc.mcc_UserAnswerComments.Where(Function(p) p.UserAnswerId = uqId)
            If v IsNot Nothing Then
               _mdc.mcc_UserAnswerComments.DeleteAllOnSubmit(v)
            End If

            _mdc.SubmitChanges()
         End If
      End If
   End Sub
   Public Sub DeleteUserAnswers(ByVal uqId As Integer()) Implements IUserAnswerRepository.DeleteUserAnswers
      If uqId IsNot Nothing AndAlso uqId.Count > 0 Then
         Dim q = _mdc.mcc_UserAnswers.Where(Function(p) uqId.Contains(p.UserAnswerId))
         If q IsNot Nothing Then

            _mdc.mcc_UserAnswers.DeleteAllOnSubmit(q)

            ' delete associated commetns
            Dim v = _mdc.mcc_UserAnswerComments.Where(Function(p) uqId.Contains(p.UserAnswerId))
            If v IsNot Nothing Then
               _mdc.mcc_UserAnswerComments.DeleteAllOnSubmit(v)
            End If

            _mdc.SubmitChanges()
         End If
      End If
   End Sub

   Public Function VoteAnswerDown(ByVal uqId As Integer, ByVal username As String) As StatusItem Implements IUserAnswerRepository.VoteAnswerDown
      Dim sti As New StatusItem
      If uqId <= 0 Then
         sti.Message = "Please specify an answer to vote for!"
         Return sti
      End If

      Dim q = _mdc.mcc_UserAnswers.Where(Function(p) p.UserAnswerId = uqId).FirstOrDefault()
      If q Is Nothing Then
         sti.Message = "Unable to find the answer. It may have been deleted"
         Return sti
      End If

      'can we vote?
      If q.AddedBy = username Then
         sti.Message = "You cannot vote for your own answer!"
         Return sti
      End If

      Dim r = _mdc.mcc_UsersAnswer_Votes.Where(Function(p) p.UserAnswerId = uqId AndAlso p.UserId = username).FirstOrDefault()
      If r IsNot Nothing Then
         If Not r.Helpful Then
            q.Votes += 1
            _mdc.mcc_UsersAnswer_Votes.DeleteOnSubmit(r)
         Else
            r.Helpful = False
            q.Votes += -2
         End If
         _mdc.SubmitChanges()
      Else
         q.Votes -= 1
         Dim uvm As New mcc_UsersAnswer_Vote With {.UserId = username.ToLower, .Helpful = False, .UserAnswerId = uqId}
         _mdc.mcc_UsersAnswer_Votes.InsertOnSubmit(uvm)
      End If

      _mdc.SubmitChanges()
      sti.Success = True
      sti.Id = q.UserAnswerId
      sti.Data = q.Votes

      Return sti
   End Function

   Public Function VoteAnswerUp(ByVal uqId As Integer, ByVal username As String) As StatusItem Implements IUserAnswerRepository.VoteAnswerUp
      Dim sti As New StatusItem
      If uqId <= 0 Then
         sti.Message = "Please specify an answer to vote for!"
         Return sti
      End If

      Dim q = _mdc.mcc_UserAnswers.Where(Function(p) p.UserAnswerId = uqId).FirstOrDefault()
      If q Is Nothing Then
         sti.Message = "Unable to find the answer. It may have been deleted"
         Return sti
      End If

      'can we vote?
      If q.AddedBy = username Then
         sti.Message = "You cannot vote for your own answer!"
         Return sti
      End If

      Dim r = _mdc.mcc_UsersAnswer_Votes.Where(Function(p) p.UserAnswerId = uqId AndAlso p.UserId = username).FirstOrDefault()
      If r IsNot Nothing Then
         If r.Helpful Then
            q.Votes += -1
            _mdc.mcc_UsersAnswer_Votes.DeleteOnSubmit(r)
         Else
            r.Helpful = True
            q.Votes += 2
         End If

         _mdc.SubmitChanges()
      Else
         q.Votes += 1
         '
         Dim uvm As New mcc_UsersAnswer_Vote With {.UserId = username.ToLower, .Helpful = True, .UserAnswerId = uqId}
         _mdc.mcc_UsersAnswer_Votes.InsertOnSubmit(uvm)
      End If
      _mdc.SubmitChanges()

      sti.Success = True
      sti.Id = q.UserAnswerId
      sti.Data = q.Votes

      Return sti
   End Function
End Class
