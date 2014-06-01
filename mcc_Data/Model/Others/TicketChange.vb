Public Class TicketChange

   Private _ticketChangeID As Integer
   Public Property TicketChangeID() As Integer
      Get
         Return _ticketChangeID
      End Get
      Set(ByVal value As Integer)
         _ticketChangeID = value
      End Set
   End Property

   Private _ticketID As Integer
   Public Property TicketID() As Integer
      Get
         Return _ticketID
      End Get
      Set(ByVal value As Integer)
         _ticketID = value
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

   Private _addeddate As DateTime
   Public Property AddedDate() As DateTime
      Get
         Return _addeddate
      End Get
      Set(ByVal value As DateTime)
         _addeddate = value
      End Set
   End Property

   Private _body As String
   Public Property Body() As String
      Get
         Return _body
      End Get
      Set(ByVal value As String)
         _body = value
      End Set
   End Property

End Class
