Public Class TicketChangeRepository
   Implements ITicketChangeRepository
   Private _mdc As MCCDataContext

   Public Sub New()
      Me.New(new MCCDataContext)
   End Sub

   Public Sub New(ByVal mdc As MCCDataContext)
      _mdc = mdc
   End Sub
   Public Function GetTicketChanges() As IQueryable(Of TicketChange) Implements ITicketChangeRepository.GetTicketChanges
      Dim q = From it As mcc_TicketChange In _mdc.mcc_TicketChanges _
              Select New TicketChange With {.AddedDate = it.AddedDate, _
                                            .AddedBy = it.Addedby, _
                                            .Body = it.Body, _
                                            .TicketChangeID = it.TicketChangeId, _
                                            .TicketID = it.TicketId}
      Return q
   End Function

   Public Function GetTicketChangesByTicket(ByVal ticketId As Integer) As IQueryable(Of TicketChange) Implements ITicketChangeRepository.GetTicketChangesByTicket
      Dim q = From it As mcc_TicketChange In _mdc.mcc_TicketChanges Where it.TicketId = ticketId _
                Select New TicketChange With {.AddedDate = it.AddedDate, _
                                            .AddedBy = it.Addedby, _
                                            .Body = it.Body, _
                                            .TicketChangeID = it.TicketChangeId, _
                                            .TicketID = it.TicketId}
      Return q
   End Function

   Public Sub UpdateTicketChange(ByVal tk As TicketChange) Implements ITicketChangeRepository.UpdateTicketChange
      If tk IsNot Nothing Then
         Dim q = _mdc.mcc_TicketChanges.Where(Function(t) t.TicketChangeId = tk.TicketChangeID).FirstOrDefault()
         If q IsNot Nothing Then
            With q
               .Body = tk.Body
            End With
            _mdc.SubmitChanges()
         End If
      End If
   End Sub

   Public Sub DeleteTicketChange(ByVal tkId As Integer) Implements ITicketChangeRepository.DeleteTicketChange
      Dim q = _mdc.mcc_TicketChanges.Where(Function(t) t.TicketChangeId = tkId).FirstOrDefault
      If q IsNot Nothing Then
         _mdc.mcc_TicketChanges.DeleteOnSubmit(q)
         _mdc.SubmitChanges()
      End If
   End Sub
   Public Sub DeleteTicketsChanged(ByVal tkIds As Integer()) Implements ITicketChangeRepository.DeleteTicketsChanged
      If tkIds IsNot Nothing AndAlso tkIds.Count > 0 Then
         Dim q = _mdc.mcc_TicketChanges.Where(Function(p) tkIds.Contains(p.TicketChangeId))
         If q Is Nothing Then
            Return
         End If

         _mdc.mcc_TicketChanges.DeleteAllOnSubmit(q)
         _mdc.SubmitChanges()
      End If
   End Sub
   Public Function InsertTicketChange(ByVal tk As TicketChange) As Integer Implements ITicketChangeRepository.InsertTicketChange
      If tk IsNot Nothing Then
         Dim q As New mcc_TicketChange
         With q
            .AddedDate = tk.AddedDate
            .Addedby = tk.AddedBy
            .Body = tk.Body
            .TicketId = tk.TicketID
         End With
         _mdc.mcc_TicketChanges.InsertOnSubmit(q)
         _mdc.SubmitChanges()
      End If
   End Function
End Class
