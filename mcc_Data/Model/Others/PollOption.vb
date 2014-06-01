Public Class PollOption

    Private _PollOptionID As Integer
    Public Property PollOptionID() As Integer
        Get
            Return _PollOptionID
        End Get
        Set(ByVal value As Integer)
            _PollOptionID = value
        End Set
    End Property

   Private _PollID As Integer
   Public Property PollID() As Integer
      Get
         Return _PollID
      End Get
      Set(ByVal value As Integer)
         _PollID = value
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

   Private _optionText As String
   Public Property OptionText() As String
      Get
         Return _optionText
      End Get
      Set(ByVal value As String)
         _optionText = value
      End Set
   End Property


   Private _votes As Integer
   Public Property Votes() As Integer
      Get
         Return _votes
      End Get
      Set(ByVal value As Integer)
         _votes = value
      End Set
   End Property
End Class
