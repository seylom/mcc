Public Class UserAnswerComment



   Private _userAnswerCommentID As Integer
   Public Property UserAnswerCommentID() As Integer
      Get
         Return _userAnswerCommentID
      End Get
      Set(ByVal value As Integer)
         _userAnswerCommentID = value
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


   Private _userAnswerID As Integer
   Public Property UserAnswerID() As Integer
      Get
         Return _userAnswerID
      End Get
      Set(ByVal value As Integer)
         _userAnswerID = value
      End Set
   End Property


   Private _parentCommentID As Integer
   Public Property ParentCommentID() As Integer
      Get
         Return _parentCommentID
      End Get
      Set(ByVal value As Integer)
         _parentCommentID = value
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
