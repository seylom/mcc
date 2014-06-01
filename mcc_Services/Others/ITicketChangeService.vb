Imports MCC.Data
Imports Webdiyer.WebControls.Mvc
Public Interface ITicketChangeService
   Function GetTicketChangesCount() As Integer
   Function GetTicketChangesCount(ByVal ticketId As Integer) As Integer
   Function GetTicketsChanges(ByVal ticketId As Integer) As List(Of TicketChange)
   Function GetTicketsChanges(ByVal ticketId As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedby") As PagedList(Of TicketChange)
   Function GetTicketsChanges(ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedby") As PagedList(Of TicketChange)
   Function GetTicketChangeById(ByVal id As Integer) As TicketChange
   Sub InsertTicketChange(ByVal tkc As TicketChange)
   Sub UpdateTicketChange(ByVal tkc As TicketChange)
   Sub DeleteTicketChange(ByVal TicketChangeId As Integer)
End Interface
