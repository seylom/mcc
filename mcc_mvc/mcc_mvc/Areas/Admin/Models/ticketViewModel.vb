Imports MCC.Data
Imports MCC.Services

Public Class ticketViewModel
   Inherits baseViewModel

   Private _ticketService As ITicketService
   Private _ticketChangeService As ITicketChangeService


   Public Sub New()
      Me.New(New TicketService, New TicketChangeService)
   End Sub

   Public Sub New(ByVal ticketsrvr As ITicketService, ByVal ticketchangesrvr As ITicketChangeService)
      _ticketService = ticketsrvr
      _ticketChangeService = ticketchangesrvr
   End Sub


   Private _ticketID As Integer = 0
   Public Property TicketID() As Integer
      Get
         Return _ticketID
      End Get
      Set(ByVal value As Integer)
         _ticketID = value
      End Set
   End Property


   Private _title As String
   Public Property Title() As String
      Get
         Return _title
      End Get
      Set(ByVal value As String)
         _title = value
      End Set
   End Property


   Private _description As String
   Public Property Description() As String
      Get
         Return _description
      End Get
      Set(ByVal value As String)
         _description = value
      End Set
   End Property


   Private _owner As String
   Public Property Owner() As String
      Get
         Return _owner
      End Get
      Set(ByVal value As String)
         _owner = value
      End Set
   End Property


   Private _status As Integer
   Public Property Status() As Integer
      Get
         Return _status
      End Get
      Set(ByVal value As Integer)
         _status = value
      End Set
   End Property


   Private _resolver As String
   Public Property Resolver() As String
      Get
         Return _resolver
      End Get
      Set(ByVal value As String)
         _resolver = value
      End Set
   End Property


   Private _priority As String
   Public Property Priority() As String
      Get
         Return _priority
      End Get
      Set(ByVal value As String)
         _priority = value
      End Set
   End Property



   Private _keywords As String
   Public Property keywords() As String
      Get
         Return _keywords
      End Get
      Set(ByVal value As String)
         _keywords = value
      End Set
   End Property


   Private _type As Integer
   Public Property Type() As Integer
      Get
         Return _type
      End Get
      Set(ByVal value As Integer)
         _type = value
      End Set
   End Property



   Private _addedDate As DateTime
   Public Property AddedDate() As DateTime
      Get
         Return _addedDate
      End Get
      Set(ByVal value As DateTime)
         _addedDate = value
      End Set
   End Property


   Private _addedBy As String
   Public Property AddedBy() As String
      Get
         Return _addedBy
      End Get
      Set(ByVal value As String)
         _addedBy = value
      End Set
   End Property


   Private _ticketChanges As List(Of TicketChange)
   Public Property TicketChanges() As List(Of TicketChange)
      Get
         If _ticketChanges Is Nothing AndAlso _ticketID > 0 Then
            _ticketChanges = _ticketChangeService.GetTicketsChanges(_ticketID)
         End If
         Return _ticketChanges
      End Get
      Set(ByVal value As List(Of TicketChange))
         _ticketChanges = value
      End Set
   End Property



End Class
