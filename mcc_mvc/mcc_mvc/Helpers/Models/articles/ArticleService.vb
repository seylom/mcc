Namespace Articles
   Public Class ArticleService
      Implements IArticleService


      Private _repository As IArticleRepository

      Public Sub New()
         Me.New(New ArticleRepository())
      End Sub

      Public Sub New(ByVal repo As IArticleRepository)
         _repository = repo
      End Sub

      Public Function AddArticleToCategories(ByVal ArticleId As Integer, ByVal CategoryId() As Integer) As Integer Implements IArticleService.AddArticleToCategories
         Return _repository.AddArticleToCategories(ArticleId, CategoryId)
      End Function

      Public Function AddArticleToCategory(ByVal ArticleId As Integer, ByVal CategoryId As Integer) As Integer Implements IArticleService.AddArticleToCategory
         Return _repository.AddArticleToCategory(ArticleId, CategoryId)
      End Function

      Public Function ApproveArticle(ByVal articleId As Integer) As Boolean Implements IArticleService.ApproveArticle
         _repository.ApproveArticle(articleId)
         Return True
      End Function

      Public Function AverageRating(ByVal articleId As Integer) As Double Implements IArticleService.AverageRating
         Return _repository.AverageRating(articleId)
      End Function

      Public Function CommentsCount(ByVal ArticleId As Integer) As Integer Implements IArticleService.CommentsCount
         Return _repository.CommentsCount(ArticleId)
      End Function

      Public Function DeleteArticle(ByVal ArticleId As Integer) As Boolean Implements IArticleService.DeleteArticle
         _repository.DeleteArticle(ArticleId)
         Return True
      End Function

      Public Function DeleteArticles(ByVal ArticleId() As Integer) As Boolean Implements IArticleService.DeleteArticles
         _repository.DeleteArticles(ArticleId)
         Return True
      End Function

      Public Function FetchArticles(ByVal categoryID As Integer, ByVal status As Integer, ByVal startRowIndex As Integer, ByVal maximumRows As Integer, Optional ByVal sortExp As String = "AddedDate") As System.Collections.Generic.List(Of mcc_Article) Implements IArticleService.FetchArticles
         Return _repository.FetchArticles(categoryID, status, startRowIndex, maximumRows, sortExp)
      End Function

      Public Function FetchArticles(ByVal status As Integer, ByVal startRowIndex As Integer, ByVal maximumRows As Integer, Optional ByVal sortExp As String = "Title") As System.Collections.Generic.List(Of mcc_Article) Implements IArticleService.FetchArticles
         Return _repository.FetchArticles(status, startRowIndex, maximumRows, sortExp)
      End Function

      Public Function FetchArticles(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, Optional ByVal sortExp As String = "Title") As System.Collections.Generic.List(Of mcc_Article) Implements IArticleService.FetchArticles
         Return _repository.FetchArticles(startRowIndex, maximumRows, sortExp)
      End Function

      Public Function FetchArticlesCount(ByVal status As Integer) As Integer Implements IArticleService.FetchArticlesCount
         Return _repository.FetchArticlesCount(status)
      End Function

      Public Function FetchArticlesCount(ByVal categoryID As Integer, ByVal status As Integer) As Integer Implements IArticleService.FetchArticlesCount
         Return _repository.FetchArticlesCount(categoryID)
      End Function

      Public Function FetchArticlesCount(ByVal status As mccEnum.Status) As Integer Implements IArticleService.FetchArticlesCount
         Return _repository.FetchArticlesCount(status)
      End Function

      Public Function FindArticles(ByVal where As mccEnum.SearchType, ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal SearchWord As String) As System.Collections.Generic.List(Of mcc_Article) Implements IArticleService.FindArticles
         Return _repository.FindArticles(where, startRowIndex, maximumRows, SearchWord)
      End Function

      Public Function FindArticles(ByVal where As mccEnum.SearchType, ByVal SearchWord As String) As System.Collections.Generic.List(Of mcc_Article) Implements IArticleService.FindArticles
         Return _repository.FindArticles(where, SearchWord)
      End Function

      Public Function FindArticlesCount(ByVal where As mccEnum.SearchType, ByVal searchWord As String) As Integer Implements IArticleService.FindArticlesCount
         Return _repository.FindArticlesCount(where, searchWord)
      End Function

      Public Function FixArticlesVideoIds() As Boolean Implements IArticleService.FixArticlesVideoIds
         Return _repository.FixArticlesVideoIds()
      End Function

      Public Function GetArticleById(ByVal ArticleId As Integer) As mcc_Article Implements IArticleService.GetArticleById
         Return _repository.GetArticleById(ArticleId)
      End Function

      Public Function GetArticleBySlug(ByVal slug As String) As mcc_Article Implements IArticleService.GetArticleBySlug
         Return _repository.GetArticleBySlug(slug)
      End Function

      Public Function GetArticleCount() As Integer Implements IArticleService.GetArticleCount
         Return _repository.GetArticleCount()
      End Function

      Public Function GetArticleCount(ByVal PublishedOnly As Boolean) As Integer Implements IArticleService.GetArticleCount
         Return _repository.GetArticleCount(PublishedOnly)
      End Function

      Public Function GetArticleCount(ByVal publishedOnly As Boolean, ByVal categoryID As Integer) As Integer Implements IArticleService.GetArticleCount
         Return _repository.GetArticleCount(publishedOnly, categoryID)
      End Function

      Public Function GetArticleCount(ByVal categoryId As Integer) As Integer Implements IArticleService.GetArticleCount
         Return _repository.GetArticleCount(categoryId)
      End Function

      Public Function GetArticleCountByAuthor(ByVal publishedOnly As Boolean, ByVal addedBy As String) As Integer Implements IArticleService.GetArticleCountByAuthor
         Return _repository.GetArticleCountByAuthor(publishedOnly, addedBy)
      End Function

      Public Function GetArticleCountByAuthor(ByVal addedBy As String) As Integer Implements IArticleService.GetArticleCountByAuthor
         Return _repository.GetArticleCountByAuthor(addedBy)
      End Function

      Public Function GetArticleCountByMainCategory(ByVal publishedOnly As Boolean, ByVal categoryID As Integer) As Integer Implements IArticleService.GetArticleCountByMainCategory
         Return _repository.GetArticleCountByMainCategory(publishedOnly, categoryID)
      End Function

      Public Function GetArticles() As System.Collections.Generic.List(Of mcc_Article) Implements IArticleService.GetArticles
         Return _repository.GetArticles()
      End Function

      Public Function GetArticles(ByVal publishedOnly As Boolean, ByVal categoryId As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "releaseDate") As System.Collections.Generic.List(Of mcc_Article) Implements IArticleService.GetArticles
         Return _repository.GetArticles(publishedOnly, categoryId, startrowindex, maximumrows, sortExp)
      End Function

      Public Function GetArticles(ByVal publishedOnly As Boolean, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "releasedate DESC") As System.Collections.Generic.List(Of mcc_Article) Implements IArticleService.GetArticles
         Return _repository.GetArticles(publishedOnly, startrowindex, maximumrows, sortExp)
      End Function

      Public Function GetArticles(ByVal categoryId As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "releaseDate") As System.Collections.Generic.List(Of mcc_Article) Implements IArticleService.GetArticles
         Return _repository.GetArticles(categoryId, startrowindex, maximumrows, sortExp)
      End Function

      Public Function GetArticles(ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "releasedate DESC") As System.Collections.Generic.List(Of mcc_Article) Implements IArticleService.GetArticles
         Return _repository.GetArticles(startrowindex, maximumrows, sortExp)
      End Function

      Public Function GetArticlesByAuthor(ByVal publishedOnly As Boolean, ByVal addedBy As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedby") As System.Collections.Generic.List(Of mcc_Article) Implements IArticleService.GetArticlesByAuthor
         Return _repository.GetArticlesByAuthor(publishedOnly, addedBy, startrowindex, maximumrows, sortExp)
      End Function

      Public Function GetArticlesByAuthor(ByVal addedBy As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedby") As System.Collections.Generic.List(Of mcc_Article) Implements IArticleService.GetArticlesByAuthor
         Return _repository.GetArticlesByAuthor(addedBy, startrowindex, maximumrows, sortExp)
      End Function

      Public Function GetArticlesByCategoryName(ByVal publishedOnly As Boolean, ByVal categoryName As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "releaseDate") As System.Collections.Generic.List(Of mcc_Article) Implements IArticleService.GetArticlesByCategoryName
         Return _repository.GetArticlesByCategoryName(publishedOnly, categoryName, startrowindex, maximumrows, sortExp)
      End Function

      Public Function GetArticlesByCategoryName(ByVal categoryName As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "releaseDate") As System.Collections.Generic.List(Of mcc_Article) Implements IArticleService.GetArticlesByCategoryName
         Return _repository.GetArticlesByCategoryName(categoryName, startrowindex, maximumrows, sortExp)
      End Function

      Public Function GetArticleStatus(ByVal articleId As Integer) As mccEnum.Status Implements IArticleService.GetArticleStatus
         Return _repository.GetArticleStatus(articleId)
      End Function

      Public Function GetArticleStatusValue(ByVal id As Integer) As Integer Implements IArticleService.GetArticleStatusValue
         Return _repository.GetArticleStatusValue(id)
      End Function

      Public Function GetComments(ByVal articleId As Integer) As System.Collections.Generic.List(Of mcc_Comment) Implements IArticleService.GetComments
         Return _repository.GetComments(articleId)
      End Function

      Public Function GetFrontPageArticleCount(ByVal frontPage As Boolean, ByVal PublishedOnly As Boolean) As Integer Implements IArticleService.GetFrontPageArticleCount
         Return _repository.GetFrontPageArticleCount(frontPage, PublishedOnly)
      End Function

      Public Function GetLatestArticles(ByVal pageSize As Integer) As System.Collections.Generic.List(Of mcc_Article) Implements IArticleService.GetLatestArticles
         Return _repository.GetLatestArticles(pageSize)
      End Function

      Public Function GetReadersPick() As mcc_Article Implements IArticleService.GetReadersPick
         Return _repository.GetReadersPick()
      End Function

      Public Function IncrementViewCount(ByVal articleId As Integer) As Boolean Implements IArticleService.IncrementViewCount
         _repository.IncrementViewCount(articleId)
         Return True
      End Function

      Public Function InsertArticle(ByVal title As String, ByVal Abstract As String, ByVal body As String, ByVal country As String, ByVal state As String, ByVal city As String, ByVal releaseDate As Date, ByVal expireDate As Date, ByVal approved As Boolean, ByVal listed As Boolean, ByVal commentsEnabled As Boolean, ByVal onlyForMembers As Boolean, ByVal ImageNewsUrl As String, ByVal ImageIconUrl As String, ByVal tags As String, ByVal videoId As Integer, ByVal pollId As Integer, ByVal status As Integer, ByVal imageId As Integer) As Boolean Implements IArticleService.InsertArticle
         _repository.InsertArticle(title, Abstract, body, country, state, city, releaseDate, expireDate, approved, listed, commentsEnabled, _
                        onlyForMembers, ImageNewsUrl, ImageIconUrl, tags, videoId, pollId, status, imageId)
         Return True
      End Function

      Public Function Published(ByVal articleId As Integer) As Boolean Implements IArticleService.Published
         Return _repository.Published(articleId)
      End Function

      Public Function RateArticle(ByVal articleId As Integer, ByVal rating As Integer) As Boolean Implements IArticleService.RateArticle
         _repository.RateArticle(articleId, rating)
         Return True
      End Function

      Public Function RemoveArticleFromCategory(ByVal ArticleID As Integer, Optional ByVal categoryId As Integer = 0) As Boolean Implements IArticleService.RemoveArticleFromCategory
         Return _repository.RemoveArticleFromCategory(ArticleID)
      End Function

      Public Function UpdateArticle(ByVal articleId As Integer, ByVal title As String, ByVal Abstract As String, ByVal body As String, ByVal country As String, ByVal state As String, ByVal city As String, ByVal releaseDate As Date, ByVal expireDate As Date, ByVal approved As Boolean, ByVal listed As Boolean, ByVal commentsEnabled As Boolean, ByVal onlyForMembers As Boolean, ByVal ImageNewsUrl As String, ByVal ImageIconUrl As String, ByVal tags As String, ByVal videoId As Integer, ByVal pollId As Integer, ByVal status As Integer, ByVal imageId As Integer) As Boolean Implements IArticleService.UpdateArticle
         _repository.UpdateArticle(articleId, title, Abstract, body, country, state, city, releaseDate, expireDate, approved, listed, commentsEnabled, _
                                          onlyForMembers, ImageNewsUrl, ImageIconUrl, tags, videoId, pollId, status, imageId)
         Return True
      End Function

      Public Function UpdateArticleStatus(ByVal id As Integer) As Boolean Implements IArticleService.UpdateArticleStatus
         _repository.UpdateArticleStatus(id)
         Return True
      End Function

      Public Function UpdateArticleStatus(ByVal id As Integer, ByVal st As mccEnum.Status) As Object Implements IArticleService.UpdateArticleStatus
         _repository.UpdateArticleStatus(id, st)
         Return True
      End Function

      Public Function GetFrontPageArticles(ByVal publishedOnly As Boolean, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "releasedate DESC") As System.Collections.Generic.List(Of frontPageArticle) Implements IArticleService.GetFrontPageArticles
         Return _repository.GetFrontPageArticles(publishedOnly, startrowindex, maximumrows, sortExp)
      End Function

      Public Function GetSpotlightArticles(ByVal publishedOnly As Boolean, ByVal categoryName As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "releasedate DESC") As System.Collections.Generic.List(Of spotlightArticle) Implements IArticleService.GetSpotlightArticles
         Return _repository.GetSpotlightArticles(publishedOnly, categoryName, startrowindex, maximumrows, sortExp)
      End Function
   End Class
End Namespace
