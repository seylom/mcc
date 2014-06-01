Public Class Advice



   Private _adviceID As Integer
   Public Property AdviceID() As Integer
      Get
         Return _adviceID
      End Get
      Set(ByVal value As Integer)
         _adviceID = value
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


   Private _title As String
   Public Property Title() As String
      Get
         Return _title
      End Get
      Set(ByVal value As String)
         _title = value
      End Set
   End Property


   Private _abstract As String
   Public Property Abstract() As String
      Get
         Return _abstract
      End Get
      Set(ByVal value As String)
         _abstract = value
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


   Private _listed As Boolean
   Public Property Listed() As Boolean
      Get
         Return _listed
      End Get
      Set(ByVal value As Boolean)
         _listed = value
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



   Private _tags As String
   Public Property Tags() As String
      Get
         Return _tags
      End Get
      Set(ByVal value As String)
         _tags = value
      End Set
   End Property



    Private _TotalRating As Integer
    Public Property TotalRating() As Integer
        Get
            Return _TotalRating
        End Get
        Set(ByVal value As Integer)
            _TotalRating = value
        End Set
    End Property



   Private _CommentsEnabled As Boolean
   Public Property CommentsEnabled() As Boolean
      Get
         Return _CommentsEnabled
      End Get
      Set(ByVal value As Boolean)
         _CommentsEnabled = value
      End Set
   End Property



   Private _onlyForMembers As Boolean
   Public Property OnlyForMembers() As Boolean
      Get
         Return _onlyForMembers
      End Get
      Set(ByVal value As Boolean)
         _onlyForMembers = value
      End Set
   End Property


   Private _Slug As String
   Public Property Slug() As String
      Get
         Return _Slug
      End Get
      Set(ByVal value As String)
         _Slug = value
      End Set
   End Property


   Private _voteUp As Integer
   Public Property VoteUp() As Integer
      Get
         Return _voteUp
      End Get
      Set(ByVal value As Integer)
         _voteUp = value
      End Set
   End Property

   Private _voteDown As Integer
   Public Property VoteDown() As Integer
      Get
         Return _voteDown
      End Get
      Set(ByVal value As Integer)
         _voteDown = value
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


   Private _votes As Integer
   Public Property Votes() As Integer
      Get
         Return _votes
      End Get
      Set(ByVal value As Integer)
         _votes = value
      End Set
   End Property


   Private _viewCount As Integer
   Public Property ViewCount() As Integer
      Get
         Return _viewCount
      End Get
      Set(ByVal value As Integer)
         _viewCount = value
      End Set
   End Property


End Class
