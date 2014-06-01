Public Class Feedback

   Private _feedbackID As String
   Public Property FeedbackID() As String
      Get
         Return _feedbackID
      End Get
      Set(ByVal value As String)
         _feedbackID = value
      End Set
   End Property

   Private _votes As String
   Public Property Votes() As String
      Get
         Return _votes
      End Get
      Set(ByVal value As String)
         _votes = value
      End Set
   End Property


   Private _opened As Boolean
   Public Property Opened() As Boolean
      Get
         Return _opened
      End Get
      Set(ByVal value As Boolean)
         _opened = value
      End Set
   End Property

   Private _approved As Boolean
   Public Property Approved() As Boolean
      Get
         Return _approved
      End Get
      Set(ByVal value As Boolean)
         _approved = value
      End Set
   End Property

   Private _answered As Boolean
   Public Property Answered() As Boolean
      Get
         Return _answered
      End Get
      Set(ByVal value As Boolean)
         _answered = value
      End Set
   End Property


   Private _addedby As String
   Public Property AddedBy() As String
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



   Private _addedDate As DateTime
   Public Property AddedDate() As DateTime
      Get
         Return _addedDate
      End Get
      Set(ByVal value As DateTime)
         _addedDate = value
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



End Class
