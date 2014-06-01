Imports System.Linq.Expressions

Public Class Article

   Private _articleID As Integer
   Public Property ArticleID() As Integer
      Get
         Return _articleID
      End Get
      Set(ByVal value As Integer)
         _articleID = value
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


   Private _releaseDate As DateTime
   Public Property ReleaseDate() As DateTime
      Get
         Return _releaseDate
      End Get
      Set(ByVal value As DateTime)
         _releaseDate = value
      End Set
   End Property



   Private _expireDate As DateTime
   Public Property ExpireDate() As DateTime
      Get
         Return _expireDate
      End Get
      Set(ByVal value As DateTime)
         _expireDate = value
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

   Private _commentsEnabled As Boolean
   Public Property CommentsEnabled() As Boolean
      Get
         Return _commentsEnabled
      End Get
      Set(ByVal value As Boolean)
         _commentsEnabled = value
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

   Public Property Votes() As Integer
   Public Property TotalRating() As Integer
   Public Property ViewCount() As Integer
   Public Property VideoID() As Integer
   Public Property PollId() As Integer?
   Public Property ImageID() As Integer?
   Public Property ImageNewsUrl() As String
   Public Property ImageIconUrl() As String

   Private _status As Integer = 0
   Public Property Status() As Integer
      Get
         Return _status
      End Get
      Set(ByVal value As Integer)
         _status = value
      End Set
   End Property


   Private _city As String
   Public Property City() As String
      Get
         Return _city
      End Get
      Set(ByVal value As String)
         _city = value
      End Set
   End Property


   Private _state As String
   Public Property State() As String
      Get
         Return _state
      End Get
      Set(ByVal value As String)
         _state = value
      End Set
   End Property


   Private _country As String
   Public Property Country() As String
      Get
         Return _country
      End Get
      Set(ByVal value As String)
         _country = value
      End Set
   End Property


   Public Function ContainsAny(ByVal ParamArray keywords As String()) As Expression(Of Func(Of Article, Boolean))
      Dim predicate = PredicateBuilder.[False](Of Article)()
      For Each keyword As String In keywords
         Dim temp As String = keyword
         predicate = predicate.[Or](Function(p) p.Tags.Contains(temp))
         predicate = predicate.[Or](Function(p) p.Title.Contains(temp))
         predicate = predicate.[Or](Function(p) p.Body.Contains(temp))
         predicate = predicate.[Or](Function(p) p.Abstract.Contains(temp))
      Next
      Return predicate
   End Function

   Private _comments As List(Of ArticleComment)
   Public ReadOnly Property Comments() As List(Of ArticleComment)
      Get
         Return _comments
      End Get
   End Property

End Class
