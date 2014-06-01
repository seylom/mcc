Imports MCC.Data
Imports Webdiyer.WebControls.Mvc
Public Interface ITicketService
   Function GetTicketCount() As Integer
   Function GetTicketCount(ByVal owner As String) As Integer
   Function GetTicketCount(ByVal status As Integer, ByVal owner As String) As Integer
   Function GetTicketsByCommand(ByVal command As String, ByVal params As String) As List(Of Ticket)
   Function GetTicketCount(ByVal status As Integer) As Integer
   Function GetTickets() As List(Of Ticket)
   Function GetTickets(ByVal status As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "Status") As PagedList(Of Ticket)
   Function GetTicketsByOwner(ByVal status As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, ByVal owner As String, Optional ByVal sortExp As String = "AddedBy") As PagedList(Of Ticket)
   Function GetTicketsByOwner(ByVal startrowindex As Integer, ByVal maximumrows As Integer, ByVal owner As String, Optional ByVal sortExp As String = "AddedBy") As PagedList(Of Ticket)
   Function GetTickets(ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "TicketId DESC") As PagedList(Of Ticket)
   Function GetTicketById(ByVal id As Integer) As Ticket
   'Sub InsertTicket(ByVal tk As Ticket)
   Sub UpdateTicketState(ByVal TicketId As Integer, ByVal status As Integer)
   'Sub UpdateTicket(ByVal tk As Ticket)
   Function SaveTicket(ByVal tk As Ticket) As Boolean
   Sub DeleteTicket(ByVal TicketId As Integer)
   Sub DeleteTickets(ByVal TkIds As Integer())
 
End Interface
