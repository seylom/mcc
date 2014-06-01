Public Class VideoComment
   Private _commentID As Integer
   Public Property CommentID() As Integer
      Get
         Return _commentID
      End Get
      Set(ByVal value As Integer)
         _commentID = value
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



   Private _addedByEmail As String
   Public Property AddedByEmail() As String
      Get
         Return _addedByEmail
      End Get
      Set(ByVal value As String)
         _addedByEmail = value
      End Set
   End Property


   Private _addedByIP As String
   Public Property AddedByIP() As String
      Get
         Return _addedByIP
      End Get
      Set(ByVal value As String)
         _addedByIP = value
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


   Private _videoID As Integer
   Public Property VideoID() As Integer
      Get
         Return _videoID
      End Get
      Set(ByVal value As Integer)
         _videoID = value
      End Set
   End Property



   Private _flag As Boolean
   Public Property Flag() As Boolean
      Get
         Return _flag
      End Get
      Set(ByVal value As Boolean)
         _flag = value
      End Set
   End Property
End Class
