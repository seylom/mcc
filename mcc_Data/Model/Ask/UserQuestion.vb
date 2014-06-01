Imports Webdiyer.WebControls.Mvc
Public Class UserQuestion

   Private _UserQuestionID As Integer
   Public Property UserQuestionID() As Integer
      Get
         Return _UserQuestionID
      End Get
      Set(ByVal value As Integer)
         _UserQuestionID = value
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

   Private _bestUserAnswerId As Integer = 0
   Public Property BestUserAnswerID() As Integer
      Get
         Return _bestUserAnswerId
      End Get
      Set(ByVal value As Integer)
         _bestUserAnswerId = value
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

   Private _slug As String
   Public Property Slug() As String
      Get
         Return _slug
      End Get
      Set(ByVal value As String)
         _slug = value
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

   Private _views As Integer
   Public Property Views() As Integer
      Get
         Return _views
      End Get
      Set(ByVal value As Integer)
         _views = value
      End Set
   End Property

   Private _activityNotification As Boolean
   Public Property ActivityNotification() As Boolean
      Get
         Return _activityNotification
      End Get
      Set(ByVal value As Boolean)
         _activityNotification = value
      End Set
   End Property



#Region "Extra Properties"

   Private _permalink As String
   Public Property Permalink() As String
      Get
         Return _permalink
      End Get
      Set(ByVal value As String)
         _permalink = value
      End Set
   End Property

   Private _AnswerCount As Integer
   Public Property AnswerCount() As Integer
      Get
         Return _AnswerCount
      End Get
      Set(ByVal value As Integer)
         _AnswerCount = value
      End Set
   End Property


   Private _commentsList As PagedList(Of UserQuestionComment)
   Public Property Comments() As PagedList(Of UserQuestionComment)
      Get
         Return _commentsList
      End Get
      Set(ByVal value As PagedList(Of UserQuestionComment))
         _commentsList = value
      End Set
   End Property

   'Private _subscribers As List(Of String)
   'Public Property Subscribers() As List(Of String)
   '   Get
   '      If _subscribers Is Nothing Then
   '         _subscribers = New List(Of String)
   '      End If
   '      Return _subscribers
   '   End Get
   '   Set(ByVal value As List(Of String))
   '      _subscribers = value
   '   End Set
   'End Property

#End Region

End Class
