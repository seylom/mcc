Public Class PollRepository
   Implements IPollRepository
   Private _mdc As MCCDataContext


   Public Sub New()
      Me.New(new MCCDataContext)
   End Sub

   Public Sub New(ByVal dc As MCCDataContext)
      _mdc = dc
   End Sub
   Public Function GetPolls() As IQueryable(Of Poll) Implements IPollRepository.GetPolls
      Dim q = From p As mcc_Poll In _mdc.mcc_Polls _
              Select New Poll With {.PollID = p.PollId, _
                                    .AddedDate = p.AddedDate, _
                                    .AddedBy = p.AddedBy, _
                                    .IsArchived = p.isArchived, _
                                    .ArchiveDate = p.ArchiveDate, _
                                    .QuestionText = p.QuestionText}

      Return q
   End Function
   Public Function InsertPoll(ByVal op As Poll) As Integer Implements IPollRepository.InsertPoll
      If op IsNot Nothing Then
         Dim p As New mcc_Poll
         With p
            p.AddedDate = op.AddedDate
            p.AddedBy = op.AddedBy
            p.ArchiveDate = op.ArchiveDate
            p.QuestionText = op.QuestionText
            p.isArchived = op.IsArchived
         End With
      End If
   End Function
   Public Sub UpdatePoll(ByVal op As Poll) Implements IPollRepository.UpdatePoll
      If op IsNot Nothing Then
         Dim q = _mdc.mcc_Polls.Where(Function(p) p.PollId = op.PollID).FirstOrDefault()
         If q IsNot Nothing Then
            With q
               q.ArchiveDate = op.AddedDate
               q.QuestionText = op.QuestionText
               q.isArchived = op.IsArchived
            End With

            _mdc.SubmitChanges()
         End If
      End If
   End Sub
   Public Sub DeletePoll(ByVal opId As Integer) Implements IPollRepository.DeletePoll
      If opId > 0 Then
         Dim q = _mdc.mcc_Polls.Where(Function(p) p.PollId = opId).FirstOrDefault()
         If q IsNot Nothing Then
            _mdc.mcc_Polls.DeleteOnSubmit(q)


            Dim r = _mdc.mcc_PollOptions.Where(Function(it) it.PollId = opId)
            If r IsNot Nothing Then
               _mdc.mcc_PollOptions.DeleteAllOnSubmit(r)
            End If

            _mdc.SubmitChanges()
         End If
      End If
   End Sub
   Public Sub DeletePolls(ByVal opIds As Integer()) Implements IPollRepository.DeletePolls
      If opIds IsNot Nothing AndAlso opIds.Count > 0 Then
         Dim q = _mdc.mcc_Polls.Where(Function(p) opIds.Contains(p.PollId))
         If q IsNot Nothing Then
                _mdc.mcc_Polls.DeleteAllOnSubmit(q)


            Dim r = _mdc.mcc_PollOptions.Where(Function(p) opIds.Contains(p.PollId))
            If r IsNot Nothing Then
               _mdc.mcc_PollOptions.DeleteAllOnSubmit(r)
            End If

            _mdc.SubmitChanges()
         End If
      End If
   End Sub
End Class
