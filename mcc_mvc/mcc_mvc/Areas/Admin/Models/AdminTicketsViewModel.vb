Imports MCC.Data
Imports MCC.Services
Imports Webdiyer.WebControls.Mvc
Public Class AdminTicketsViewModel
   Inherits baseViewModel

   Private _ticketService As ITicketService

   Public Sub New()
      Me.New(0, 30, New TicketService())
   End Sub

   Public Sub New(ByVal pageIndex As Integer, ByVal pageSize As Integer)
      Me.New(PageIndex, pageSize, New TicketService())
   End Sub

   Public Sub New(ByVal pageIndex As Integer, ByVal pageSize As Integer, ByVal ticketsrvr As ITicketService)
      _pageIndex = pageIndex
      _pageSize = pageSize
      _ticketService = ticketsrvr

      _tickets = _ticketService.GetTickets(pageIndex, pageSize)

      Me.AllCount = _ticketService.GetTicketCount()
      Me.NewCount = _ticketService.GetTicketCount(TicketState.new)
      Me.AssignedCount = _ticketService.GetTicketCount(TicketState.assigned)
      Me.ResolvedCount = _ticketService.GetTicketCount(TicketState.resolved)
      Me.VerifiedCount = _ticketService.GetTicketCount(TicketState.verified)
      Me.ClosedCount = _ticketService.GetTicketCount(TicketState.closed)
      Me.ReopenedCount = _ticketService.GetTicketCount(TicketState.reopened)
      Me.MyTicketsCount = _ticketService.GetTicketCount(HttpContext.Current.User.Identity.Name)

   End Sub

   Private _tickets As PagedList(Of Ticket)
   Public Property Tickets() As PagedList(Of Ticket)
      Get
         Return _tickets
      End Get
      Set(ByVal value As PagedList(Of Ticket))
         _tickets = value
      End Set
   End Property



   Private _pageIndex As Integer = 0
   Public Property PageIndex() As Integer
      Get
         Return _pageIndex
      End Get
      Set(ByVal value As Integer)
         _pageIndex = value
      End Set
   End Property

   Private _pageSize As Integer = 30
   Public Property PageSize() As Integer
      Get
         Return _pageSize
      End Get
      Set(ByVal value As Integer)
         _pageSize = value
      End Set
   End Property



   Private _allTicketCount As Integer
   Public Property AllCount() As Integer
      Get
         Return _allTicketCount
      End Get
      Set(ByVal value As Integer)
         _allTicketCount = value
      End Set
   End Property



   Private _assigneCount As Integer
   Public Property AssignedCount() As Integer
      Get
         Return _assigneCount
      End Get
      Set(ByVal value As Integer)
         _assigneCount = value
      End Set
   End Property


   Private _closedCount As Integer
   Public Property ClosedCount() As Integer
      Get
         Return _closedCount
      End Get
      Set(ByVal value As Integer)
         _closedCount = value
      End Set
   End Property


   Private _resolvedCount As Integer
   Public Property ResolvedCount() As Integer
      Get
         Return _resolvedCount
      End Get
      Set(ByVal value As Integer)
         _resolvedCount = value
      End Set
   End Property



   Private _verifiedCount As Integer
    Public Property VerifiedCount() As Integer
        Get
            Return _verifiedCount
        End Get
        Set(ByVal value As Integer)
            _verifiedCount = value
        End Set
    End Property



   Private _reopened As Integer
   Public Property ReopenedCount() As Integer
      Get
         Return _reopened
      End Get
      Set(ByVal value As Integer)
         _reopened = value
      End Set
   End Property

   Private _newCount As Integer
   Public Property NewCount() As Integer
      Get
         Return _newCount
      End Get
      Set(ByVal value As Integer)
         _newCount = value
      End Set
   End Property


   Private _MyTicketsCount As Integer
   Public Property MyTicketsCount() As Integer
      Get
         Return _MyTicketsCount
      End Get
      Set(ByVal value As Integer)
         _MyTicketsCount = value
      End Set
   End Property


End Class
