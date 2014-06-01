Public Class TicketRepository
   Implements ITicketRepository
   Private _mdc As MCCDataContext

   Public Sub New()
      Me.New(new MCCDataContext)
   End Sub

   Public Sub New(ByVal mdc As MCCDataContext)
      _mdc = mdc
   End Sub
   Public Function GetTickets() As IQueryable(Of Ticket) Implements ITicketRepository.GetTickets
      Dim q = From t As mcc_Ticket In _mdc.mcc_Tickets _
              Select New Ticket With {.Owner = t.Owner, _
                                       .AddedDate = t.AddedDate, _
                                       .Addedby = t.AddedBy, _
                                       .Description = t.Description, _
                                       .Keywords = t.Keywords, _
                                       .Priority = t.Priority, _
                                       .Resolver = t.Resolver, _
                                       .Status = t.Status, _
                                       .TicketID = t.TicketId, _
                                       .Title = t.Title, _
                                       .Type = t.Type}
      Return q
   End Function

   Public Sub UpdateTicket(ByVal tk As Ticket) Implements ITicketRepository.UpdateTicket
      If tk IsNot Nothing Then
         Dim q = _mdc.mcc_Tickets.Where(Function(t) t.TicketId = tk.TicketID).FirstOrDefault()
         If q IsNot Nothing Then

            With q
               .Owner = tk.Owner
               .Description = tk.Description
               .Keywords = tk.Keywords
               .Priority = tk.Priority
               .Resolver = tk.Resolver
               .Status = tk.Status
               .Title = tk.Title
               .Type = tk.Type
            End With

            _mdc.SubmitChanges()
         End If
      End If
   End Sub

   Public Sub DeleteTicket(ByVal tkId As Integer) Implements ITicketRepository.DeleteTicket
      Dim q = _mdc.mcc_Tickets.Where(Function(t) t.TicketId = tkId).FirstOrDefault
      If q IsNot Nothing Then
         _mdc.mcc_Tickets.DeleteOnSubmit(q)
         _mdc.SubmitChanges()
      End If
   End Sub

   Public Sub DeleteTickets(ByVal tkIds As Integer()) Implements ITicketRepository.DeleteTickets
      Dim q = _mdc.mcc_Tickets.Where(Function(t) tkIds.Contains(t.TicketId))
      If q IsNot Nothing Then
         _mdc.mcc_Tickets.DeleteAllOnSubmit(q)

         ' delete related ticketchanges
         Dim v = _mdc.mcc_TicketChanges.Where(Function(t) tkIds.Contains(t.TicketId))
         If v IsNot Nothing Then
            _mdc.mcc_TicketChanges.DeleteAllOnSubmit(v)
         End If

         _mdc.SubmitChanges()
      End If
   End Sub

   Public Function InsertTicket(ByVal tk As Ticket) As Integer Implements ITicketRepository.InsertTicket
      If tk IsNot Nothing Then
         Dim q As New mcc_Ticket
         With q
            .Owner = tk.Owner
            .AddedDate = tk.AddedDate
            .Addedby = tk.Addedby
            .Description = tk.Description
            .Keywords = tk.Keywords
            .Priority = tk.Priority
            .Resolver = tk.Resolver
            .Status = tk.Status
            .Title = tk.Title
            .Type = tk.Type
         End With
         _mdc.mcc_Tickets.InsertOnSubmit(q)
         _mdc.SubmitChanges()
      End If
   End Function

   Public Function GetTicketsByCommand(ByVal command As String, ByVal params As String) As IQueryable(Of Ticket) Implements ITicketRepository.getTicketsbyCommand
      If Not String.IsNullOrEmpty(command) Then
         Try
            Dim parameters() As String = params.Split("|"c)
            Dim q = _mdc.ExecuteQuery(Of mcc_Ticket)(command, parameters). _
                  Select(Function(t) New Ticket With {.Owner = t.Owner, _
                                                      .AddedDate = t.AddedDate, _
                                                      .Addedby = t.AddedBy, _
                                                      .Description = t.Description, _
                                                      .Keywords = t.Keywords, _
                                                      .Priority = t.Priority, _
                                                      .Resolver = t.Resolver, _
                                                      .Status = t.Status, _
                                                      .TicketID = t.TicketId, _
                                                      .Title = t.Title, _
                                                      .Type = t.Type})

                Return q.AsQueryable
         Catch ex As Exception
            Throw New Exception("Error in the sql Command for Ticket Filtering -")
            Return Nothing
         End Try
      Else
         Return Nothing
      End If
   End Function
End Class
