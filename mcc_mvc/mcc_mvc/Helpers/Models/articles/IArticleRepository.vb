Namespace Articles
   Public Interface IArticleRepository
      ''' <summary>
      ''' Get the average rating for an article
      ''' </summary>
      ''' <param name="articleId">The article id.</param>
      ''' <returns></returns>
      Function AverageRating(ByVal articleId As Integer) As Double
      ''' <summary>
      ''' Gets the comments.
      ''' </summary>
      ''' <param name="articleId">The article id.</param>
      ''' <returns></returns>
      Function GetComments(ByVal articleId As Integer) As List(Of mcc_Comment)
      Function CommentsCount(ByVal ArticleId As Integer) As Integer
      Function Published(ByVal articleId As Integer) As Boolean
      Function GetArticleCount() As Integer
      Function GetArticleCount(ByVal PublishedOnly As Boolean) As Integer
      Function GetFrontPageArticleCount(ByVal frontPage As Boolean, ByVal PublishedOnly As Boolean) As Integer
      Function GetArticleCountByAuthor(ByVal publishedOnly As Boolean, ByVal addedBy As String) As Integer
      Function GetArticleCountByAuthor(ByVal addedBy As String) As Integer
      Function GetArticleCount(ByVal categoryId As Integer) As Integer
      Function GetArticleCount(ByVal publishedOnly As Boolean, ByVal categoryID As Integer) As Integer
      Function GetArticleCountByMainCategory(ByVal publishedOnly As Boolean, ByVal categoryID As Integer) As Integer
      Function GetArticles() As List(Of mcc_Article)
      Function GetArticles(ByVal publishedOnly As Boolean, ByVal categoryId As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "releaseDate") As List(Of mcc_Article)
      Function GetArticlesByCategoryName(ByVal publishedOnly As Boolean, ByVal categoryName As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "releaseDate") As List(Of mcc_Article)
      Function GetArticles(ByVal categoryId As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "releaseDate") As List(Of mcc_Article)
      Function GetArticlesByCategoryName(ByVal categoryName As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "releaseDate") As List(Of mcc_Article)
      Function GetArticlesByAuthor(ByVal addedBy As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedby") As List(Of mcc_Article)
      Function GetArticlesByAuthor(ByVal publishedOnly As Boolean, ByVal addedBy As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedby") As List(Of mcc_Article)
      Function GetArticles(ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "releasedate DESC") As List(Of mcc_Article)
      Function GetArticles(ByVal publishedOnly As Boolean, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "releasedate DESC") As List(Of mcc_Article)
      Function GetFrontPageArticles(ByVal publishedOnly As Boolean, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "releasedate DESC") As List(Of frontPageArticle)
      Function GetSpotlightArticles(ByVal publishedOnly As Boolean, ByVal categoryName As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "releasedate DESC") As List(Of spotlightArticle)
      ''' <summary>
      ''' Returns an Article object with the specified ID
      ''' </summary>
      Function GetLatestArticles(ByVal pageSize As Integer) As List(Of mcc_Article)
      Function GetArticleById(ByVal ArticleId As Integer) As mcc_Article
      Function GetArticleBySlug(ByVal slug As String) As mcc_Article
      ''' <summary>
      ''' Creates a new Article
      ''' </summary>
      Sub InsertArticle(ByVal title As String, ByVal Abstract As String, ByVal body As String, ByVal country As String, ByVal state As String, ByVal city As String, ByVal releaseDate As DateTime, ByVal expireDate As DateTime, ByVal approved As Boolean, ByVal listed As Boolean, ByVal commentsEnabled As Boolean, ByVal onlyForMembers As Boolean, ByVal ImageNewsUrl As String, ByVal ImageIconUrl As String, ByVal tags As String, ByVal videoId As Integer, ByVal pollId As Integer, ByVal status As Integer, ByVal imageId As Integer)
      Sub UpdateArticle(ByVal articleId As Integer, ByVal title As String, ByVal Abstract As String, ByVal body As String, ByVal country As String, ByVal state As String, ByVal city As String, ByVal releaseDate As DateTime, ByVal expireDate As DateTime, ByVal approved As Boolean, ByVal listed As Boolean, ByVal commentsEnabled As Boolean, ByVal onlyForMembers As Boolean, ByVal ImageNewsUrl As String, ByVal ImageIconUrl As String, ByVal tags As String, ByVal videoId As Integer, ByVal pollId As Integer, ByVal status As Integer, ByVal imageId As Integer)
      Sub DeleteArticles(ByVal ArticleId As Integer())
      Sub DeleteArticle(ByVal ArticleId As Integer)
      Sub ApproveArticle(ByVal articleId As Integer)
      Function RemoveArticleFromCategory(ByVal ArticleID As Integer, Optional ByVal categoryId As Integer = 0) As Boolean
      Function AddArticleToCategory(ByVal ArticleId As Integer, ByVal CategoryId As Integer) As Integer
      Function AddArticleToCategories(ByVal ArticleId As Integer, ByVal CategoryId As Integer()) As Integer
      Sub IncrementViewCount(ByVal articleId As Integer)
      Sub RateArticle(ByVal articleId As Integer, ByVal rating As Integer)
      Function FindArticles(ByVal where As mccEnum.SearchType, ByVal SearchWord As String) As List(Of mcc_Article)
      Function FindArticles(ByVal where As mccEnum.SearchType, ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal SearchWord As String) As List(Of mcc_Article)
      ''' <summary>
      ''' Returns the number of total articles matching the search key
      ''' </summary>
      Function FindArticlesCount(ByVal where As mccEnum.SearchType, ByVal searchWord As String) As Integer
      Function FetchArticlesCount(ByVal status As mccEnum.Status) As Integer
      Function FetchArticlesCount(ByVal status As Integer) As Integer
      Function FetchArticlesCount(ByVal categoryID As Integer, ByVal status As Integer) As Integer
      Function FetchArticles(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, Optional ByVal sortExp As String = "Title") As List(Of mcc_Article)
      Function FetchArticles(ByVal status As Integer, ByVal startRowIndex As Integer, ByVal maximumRows As Integer, Optional ByVal sortExp As String = "Title") As List(Of mcc_Article)
      Function FetchArticles(ByVal categoryID As Integer, ByVal status As Integer, ByVal startRowIndex As Integer, ByVal maximumRows As Integer, Optional ByVal sortExp As String = "AddedDate") As List(Of mcc_Article)
      Sub UpdateArticleStatus(ByVal id As Integer)
      Sub UpdateArticleStatus(ByVal id As Integer, ByVal st As mccEnum.Status)
      Function GetArticleStatusValue(ByVal id As Integer) As Integer
      Function GetArticleStatus(ByVal articleId As Integer) As mccEnum.Status
      Function FixArticlesVideoIds() As Boolean
      Function GetReadersPick() As mcc_Article
   End Interface
End Namespace
