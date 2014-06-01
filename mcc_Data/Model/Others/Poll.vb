Public Class Poll

   Private _pollID As Integer
   Public Property PollID() As Integer
      Get
         Return _pollID
      End Get
      Set(ByVal value As Integer)
         _pollID = value
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



   Private _questionText As String
   Public Property QuestionText() As String
      Get
         Return _questionText
      End Get
      Set(ByVal value As String)
         _questionText = value
      End Set
   End Property


   Private _isArchived As Boolean
   Public Property IsArchived() As Boolean
      Get
         Return _isArchived
      End Get
      Set(ByVal value As Boolean)
         _isArchived = value
      End Set
   End Property



   Private _archivedDate As DateTime?
   Public Property ArchiveDate() As DateTime?
      Get
         Return _archivedDate
      End Get
      Set(ByVal value As DateTime?)
         _archivedDate = value
      End Set
   End Property




End Class
