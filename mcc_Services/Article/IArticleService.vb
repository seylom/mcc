Imports MCC.Data
Imports Webdiyer.WebControls.Mvc
Public Interface IArticleService
   ''' <summary>
   ''' Get the average rating for an article
   ''' </summary>
   ''' <param name="articleId">The article id.</param>
   ''' <returns></returns>
   Function AverageRating(ByVal articleId As Integer) As Double
   Function GetComments(ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As PagedList(Of ArticleComment)
   Function GetComments(ByVal articleId As Integer) As IList(Of ArticleComment)
   Function CommentsCount(ByVal ArticleId As Integer) As Integer
   Function Published(ByVal articleId As Integer) As Boolean
   Function GetArticleCount() As Integer
   Function GetArticleCount(ByVal PublishedOnly As Boolean) As Integer
   Function GetArticleCountByAuthor(ByVal publishedOnly As Boolean, ByVal addedBy As String) As Integer
   Function GetArticleCountByAuthor(ByVal addedBy As String) As Integer
   Function GetArticleCount(ByVal categoryId As Integer) As Integer
   Function GetArticleCount(ByVal publishedOnly As Boolean, ByVal categoryID As Integer) As Integer
   Function GetArticles() As List(Of Article)
   Function GetArticles(ByVal publishedOnly As Boolean, ByVal categoryId As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "releaseDate") As PagedList(Of Article)
   Function GetArticlesByCategoryName(ByVal publishedOnly As Boolean, ByVal categoryName As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "releaseDate") As PagedList(Of Article)
   Function GetArticles(ByVal categoryId As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "releaseDate") As PagedList(Of Article)
    Function GetArticlesByCategoryName(ByVal categoryName As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "releaseDate") As PagedList(Of Article)
   Function GetArticlesByAuthor(ByVal addedBy As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedby") As PagedList(Of Article)
   Function GetArticlesByAuthor(ByVal publishedOnly As Boolean, ByVal addedBy As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedby") As PagedList(Of Article)
   Function GetArticles(ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "releasedate DESC") As PagedList(Of Article)
   Function GetArticles(ByVal publishedOnly As Boolean, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "releasedate DESC") As PagedList(Of Article)
   Function GetFrontPageArticles(ByVal publishedOnly As Boolean, ByVal categoryToFilter As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "releasedate DESC") As PagedList(Of ArticleAdv)
   Function GetSpotlightArticles(ByVal publishedOnly As Boolean, ByVal categoryName As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "releasedate DESC") As PagedList(Of Article)
   ''' <summary>
   ''' Returns an Article object with the specified ID
   ''' </summary>
   Function GetLatestArticles(ByVal pageSize As Integer) As List(Of Article)
   Function GetArticleById(ByVal ArticleId As Integer) As Article
   Function GetArticleBySlug(ByVal slug As String) As Article
   ''' <summary>
   ''' Creates a new Article
   ''' </summary>
   Function InsertArticle(ByVal wrd As Article) As Integer
   Sub UpdateArticle(ByVal _art As Article)
   Sub DeleteArticles(ByVal ArticleIds As Integer())
   Sub DeleteArticle(ByVal ArticleId As Integer)
   Sub ApproveArticle(ByVal articleId As Integer)
   Function RemoveArticleFromCategory(ByVal ArticleID As Integer, Optional ByVal categoryId As Integer = 0) As Boolean
   Function AddArticleToCategories(ByVal ArticleId As Integer, ByVal CategoryIds As Integer()) As Integer
   Sub IncrementViewCount(ByVal articleId As Integer)
   Sub RateArticle(ByVal articleId As Integer, ByVal rating As Integer)
   Function FindArticlesWithExactMatch(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal SearchWord As String) As PagedList(Of Article)
   Function FindArticlesWithAnyMatch(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal SearchWord As String) As PagedList(Of Article)
    Function FindArticleWithExactMatchCount(ByVal SearchWord As String) As Integer
    Function FindArticleWithAnyMatchCount(ByVal SearchWord As String) As Integer

   Function GetArticlesCountByStatus(ByVal status As Integer) As Integer
   Function GetArticlesCountInCategoryByStatus(ByVal categoryID As Integer, ByVal status As Integer) As Integer
   Function GetArticlesByStatus(ByVal status As Integer, ByVal startRowIndex As Integer, ByVal maximumRows As Integer, Optional ByVal sortExp As String = "Title") As PagedList(Of Article)
   Function GetArticlesByStatus(ByVal categoryID As Integer, ByVal status As Integer, ByVal startRowIndex As Integer, ByVal maximumRows As Integer, Optional ByVal sortExp As String = "AddedDate") As PagedList(Of Article)
   Sub UpdateArticleStatus(ByVal id As Integer)
   Sub UpdateArticleStatus(ByVal id As Integer, ByVal st As Integer)
   Function GetArticleStatusValue(ByVal id As Integer) As Integer
   Function FixArticlesVideoIds() As Boolean
   Function GetReadersPick() As Article

   Function SaveArticle(ByVal ar As Article, ByVal categories() As Integer, ByVal ads() As Integer) As Boolean
End Interface
