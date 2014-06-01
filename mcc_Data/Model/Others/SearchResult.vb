Public Enum SearchLocation As Integer
   All = 0
   Articles
   Forums
   Comments
   Videos
   QuestionsAndAnswers
   Tips
End Enum

Public Enum SearchFilters As Integer
   ItemsDate = 1
   Author
   Category
   Type
   None
End Enum

Public Enum SearchQueryType As Integer
   AnyWord = 0 ' default
   AllWords = 1
   ExactPhrase = 2
End Enum

Public Class SearchResult
   Private _id As Integer = 0
   Public Property SearchResultID() As Integer
      Get
         Return _id
      End Get
      Set(ByVal value As Integer)
         _id = value
      End Set
   End Property

   Private _title As String = ""
   Public Property Title() As String
      Get
         Return _title
      End Get
      Set(ByVal value As String)
         _title = value
      End Set
   End Property

   Private _abstract As String = ""
   Public Property Abstract() As String
      Get
         Return _abstract
      End Get
      Set(ByVal value As String)
         _abstract = value
      End Set
   End Property

   Private _body As String = Nothing
   Public Property Body() As String
      Get
         Return _body
      End Get
      Set(ByVal value As String)
         _body = value
      End Set
   End Property


   Private _tags As String = ""
   Public Property Tags() As String
      Get
         Return _tags
      End Get
      Set(ByVal value As String)
         _tags = value
      End Set
   End Property

   Private _url As String = ""
   Public Property Url() As String
      Get
         Return _url
      End Get
      Set(ByVal value As String)
         _url = value
      End Set
   End Property

   Private _parentUrl As String = ""
   Public Property ParentUrl() As String
      Get
         Return _parentUrl
      End Get
      Set(ByVal value As String)
         _parentUrl = value
      End Set
   End Property

   Private _relevance As Integer = 0
   Public Property Relevance() As Integer
      Get
         Return _relevance
      End Get
      Set(ByVal value As Integer)
         _relevance = value
      End Set
   End Property

   Private _resultType As SearchLocation
   Public Property ResultType() As SearchLocation
      Get
         Return _resultType
      End Get
      Set(ByVal value As SearchLocation)
         _resultType = value
      End Set
   End Property

   Private _iconClassName As String = ""
   Public Property IconClassName() As String
      Get
         Return _iconClassName
      End Get
      Set(ByVal value As String)
         _iconClassName = value
      End Set
   End Property


End Class
