Public Interface ITicketRepository
   Function GetTickets() As IQueryable(Of Ticket)
   Function GetTicketsByCommand(ByVal command As String, ByVal params As String) As IQueryable(Of Ticket)
   Sub UpdateTicket(ByVal tk As Ticket)
   Sub DeleteTicket(ByVal tkId As Integer)
   Sub DeleteTickets(ByVal tkIds() As Integer)
   Function InsertTicket(ByVal tk As Ticket) As Integer
End Interface
