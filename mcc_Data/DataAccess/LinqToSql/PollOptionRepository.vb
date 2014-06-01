Public Class PollOptionRepository
   Implements IPollOptionRepository


   Private _mdc As MCCDataContext

   Public Sub New()
      Me.New(new MCCDataContext)
   End Sub

   Public Sub New(ByVal dc As MCCDataContext)
      _mdc = dc
   End Sub
   Public Function GetPollOptions() As IQueryable(Of PollOption) Implements IPollOptionRepository.GetPollOptions
        Dim q = From p As mcc_PollOption In _mdc.mcc_PollOptions _
           Select New PollOption With {.PollID = p.PollId, _
                                 .AddedDate = p.AddedDate, _
                                 .AddedBy = p.AddedBy, _
                                 .PollOptionID = p.OptionId, _
                                 .OptionText = p.OptionText, _
                                 .Votes = If(p.Votes, 0)}

      Return q
   End Function
   Public Function InsertPollOption(ByVal op As PollOption) As Integer Implements IPollOptionRepository.InsertPollOption
      If op IsNot Nothing Then
         Dim p As New mcc_PollOption
         With p
            p.AddedDate = op.AddedDate
            p.AddedBy = op.AddedBy
            p.OptionText = op.OptionText
            p.Votes = op.Votes
            p.PollId = op.PollID
         End With
      End If
   End Function
   Public Sub UpdatePollOption(ByVal op As PollOption) Implements IPollOptionRepository.UpdatePollOption
      If op IsNot Nothing Then
            Dim q = _mdc.mcc_PollOptions.Where(Function(p) p.OptionId = op.PollOptionID).FirstOrDefault()
         If q IsNot Nothing Then
            With q
               q.PollId = op.PollID
               q.OptionText = op.OptionText
               q.Votes = op.Votes
            End With
            _mdc.SubmitChanges()
         End If
      End If
   End Sub
   Public Sub DeletePollOption(ByVal opId As Integer) Implements IPollOptionRepository.DeletePollOption
      If opId > 0 Then
            Dim q As mcc_PollOption = _mdc.mcc_PollOptions.Where(Function(p) p.OptionId = opId).FirstOrDefault()
         If q IsNot Nothing Then
            _mdc.mcc_PollOptions.DeleteOnSubmit(q)
            _mdc.SubmitChanges()
         End If
      End If
   End Sub
   Public Sub DeletePollOptions(ByVal opIds As Integer()) Implements IPollOptionRepository.DeletePollOptions
      If opIds IsNot Nothing AndAlso opIds.Count > 0 Then
            Dim q = _mdc.mcc_PollOptions.Where(Function(p) opIds.Contains(p.OptionId)).FirstOrDefault
         If q IsNot Nothing Then
            _mdc.mcc_PollOptions.DeleteOnSubmit(q)
            _mdc.SubmitChanges()
         End If
      End If
   End Sub

End Class
