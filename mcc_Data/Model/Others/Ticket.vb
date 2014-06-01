Public Enum FilterValue As Integer
   Contains = 0
   DoesNotContain = 1
   StartsWith = 2
   EndsWith = 3
   [Is] = 4
   [IsNot] = 5
End Enum

Public Class Ticket

   Private _ticketId As Integer
   Public Property TicketID() As Integer
      Get
         Return _ticketId
      End Get
      Set(ByVal value As Integer)
         _ticketId = value
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


   Private _status As Integer
   Public Property Status() As Integer
      Get
         Return _status
      End Get
      Set(ByVal value As Integer)
         _status = value
      End Set
   End Property

   Private _priority As Integer
   Public Property Priority() As Integer
      Get
         Return _priority
      End Get
      Set(ByVal value As Integer)
         _priority = value
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


   Private _addedby As String
   Public Property Addedby() As String
      Get
         Return _addedby
      End Get
      Set(ByVal value As String)
         _addedby = value
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



   Private _keywords As String
   Public Property Keywords() As String
      Get
         Return _keywords
      End Get
      Set(ByVal value As String)
         _keywords = value
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


   Private _addeddate As DateTime
   Public Property AddedDate() As DateTime
      Get
         Return _addeddate
      End Get
      Set(ByVal value As DateTime)
         _addeddate = value
      End Set
   End Property


End Class
