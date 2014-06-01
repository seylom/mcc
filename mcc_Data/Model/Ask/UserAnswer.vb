Public Class UserAnswer
   Private _UserAnswerID As Integer
   Public Property UserAnswerID() As Integer
      Get
         Return _UserAnswerID
      End Get
      Set(ByVal value As Integer)
         _UserAnswerID = value
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

   Private _body As String
   Public Property Body() As String
      Get
         Return _body
      End Get
      Set(ByVal value As String)
         _body = value
      End Set
   End Property

   Private _relevanceFactor As Integer?
   Public Property RelevanceFactor() As Integer?
      Get
         Return _relevanceFactor
      End Get
      Set(ByVal value As Integer?)
         _relevanceFactor = value
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

   Private _userQuestionID As Integer
   Public Property UserQuestionID() As Integer
      Get
         Return _UserQuestionID
      End Get
      Set(ByVal value As Integer)
         _UserQuestionID = value
      End Set
   End Property

   Private _questiontitle As String
   Public Property QuestionTitle() As String
      Get
         Return _questiontitle
      End Get
      Set(ByVal value As String)
         _questiontitle = value
      End Set
   End Property

   Private _questionSlug As String
   Public Property QuestionSlug() As String
      Get
         Return _questionSlug
      End Get
      Set(ByVal value As String)
         _questionSlug = value
      End Set
   End Property



End Class
