Public Class UserQuestionComment

   Private _userQuestionCommentID As Integer
   Public Property UserQuestionCommentID() As Integer
      Get
         Return _userQuestionCommentID
      End Get
      Set(ByVal value As Integer)
         _userQuestionCommentID = value
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


   Private _userQuestionID As Integer
   Public Property UserQuestionID() As Integer
      Get
         Return _userQuestionID
      End Get
      Set(ByVal value As Integer)
         _userQuestionID = value
      End Set
   End Property



   Private _body As String
   Public Property body() As String
      Get
         Return _body
      End Get
      Set(ByVal value As String)
         _body = value
      End Set
   End Property


End Class
