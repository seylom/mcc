Public Class Video

   Private _videoID As Integer
   Public Property VideoID() As Integer
      Get
         Return _videoID
      End Get
      Set(ByVal value As Integer)
         _videoID = value
      End Set
   End Property


   Private _name As String
   Public Property Name() As String
      Get
         Return _name
      End Get
      Set(ByVal value As String)
         _name = value
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


   Private _approved As Boolean
   Public Property Approved() As Boolean
      Get
         Return _approved
      End Get
      Set(ByVal value As Boolean)
         _approved = value
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


   Private _commentsEnabled As Boolean
   Public Property CommentsEnabled() As Boolean
      Get
         Return _commentsEnabled
      End Get
      Set(ByVal value As Boolean)
         _commentsEnabled = value
      End Set
   End Property



   Private _viewCount As Integer?
   Public Property ViewCount() As Integer?
      Get
         Return _viewCount
      End Get
      Set(ByVal value As Integer?)
         _viewCount = value
      End Set
   End Property


   Private _votes As Integer?
   Public Property Votes() As Integer?
      Get
         Return _votes
      End Get
      Set(ByVal value As Integer?)
         _votes = value
      End Set
   End Property


    Private _totalRating As Integer
    Public Property TotalRating() As Integer
        Get
            Return _totalRating
        End Get
        Set(ByVal value As Integer)
            _totalRating = value
        End Set
    End Property



   Private _videoUrl As String
   Public Property VideoUrl() As String
      Get
         Return _videoUrl
      End Get
      Set(ByVal value As String)
         _videoUrl = value
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



   Private _onlyForMembers As Boolean
   Public Property OnlyForMembers() As Boolean
      Get
         Return _onlyForMembers
      End Get
      Set(ByVal value As Boolean)
         _onlyForMembers = value
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


   Private _tags As String
   Public Property Tags() As String
      Get
         Return _tags
      End Get
      Set(ByVal value As String)
         _tags = value
      End Set
   End Property

   Private _Duration As Double?
   Public Property Duration() As Double?
      Get
         Return _Duration
      End Get
      Set(ByVal value As Double?)
         _Duration = value
      End Set
   End Property

   Public Function Copy() As Video
      Dim it As New Video With {.VideoID = Me.VideoID, _
                                .AddedDate = Me.AddedDate, _
                                .AddedBy = Me.AddedBy, _
                                .Approved = Me.Approved, _
                                .Listed = Me.Listed, _
                                .Abstract = Me.Abstract, _
                                .Duration = Me.Duration, _
                                .Votes = Me.Votes, _
                                .ViewCount = Me.ViewCount, _
                                .TotalRating = Me.TotalRating, _
                                .VideoUrl = Me.VideoUrl, _
                                .CommentsEnabled = Me.CommentsEnabled, _
                                .Name = Me.Name, _
                                .OnlyForMembers = Me.OnlyForMembers, _
                                .Title = Me.Title, _
                                .Tags = Me.Tags, _
                                .Slug = Me.Slug}
      Return it
   End Function
End Class
