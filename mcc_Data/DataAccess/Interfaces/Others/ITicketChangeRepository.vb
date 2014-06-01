Public Interface ITicketChangeRepository
   Function GetTicketChanges() As IQueryable(Of TicketChange)
   Function GetTicketChangesByTicket(ByVal ticketId As Integer) As IQueryable(Of TicketChange)
   Sub UpdateTicketChange(ByVal tk As TicketChange)
   Sub DeleteTicketChange(ByVal tkId As Integer)
   Sub DeleteTicketsChanged(ByVal tkIds() As Integer)
   Function InsertTicketChange(ByVal tk As TicketChange) As Integer
End Interface
