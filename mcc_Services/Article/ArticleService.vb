Imports MCC.Data
Imports Webdiyer.WebControls.Mvc
Public Enum Status As Integer
   Pending = 0
   Verified = 1
   Accepted = 2
   Rejected = 3
   Approved = 4
   Quarantine = 5
End Enum


Public Class ArticleService
   Inherits CacheObject
   Implements IArticleService

   Private _validationservice As IValidationService
   Private _ArticleRepo As IArticleRepository
   Private _CommentRepo As IArticleCommentRepository
   Private _ArticleCategoryRepo As IArticleCategoryRepository
   Private _ImageRepo As IImageRepository
   Private _articleTagRepo As IArticleTagRepository

   Private _adRepository As IAdRepository
   Public Property AdRepo() As IAdRepository
      Get
         If _adRepository Is Nothing Then
            _adRepository = New AdRepository
         End If
         Return _adRepository
      End Get
      Set(ByVal value As IAdRepository)
         _adRepository = value
      End Set
   End Property

   Public Sub New()
      Me.New(Nothing, New ArticleRepository, New ArticleCategoryRepository, _
             New ArticleCommentRepository, New ImageRepository, New ArticleTagRepository)
   End Sub

   Public Sub New(ByVal validservice As IValidationService)
      Me.New(validservice, New ArticleRepository, New ArticleCategoryRepository, _
             New ArticleCommentRepository, New ImageRepository, New ArticleTagRepository)
   End Sub


   Public Sub New(ByVal validserv As IValidationService, ByVal articleRepo As IArticleRepository, ByVal articleCategoryRepo As IArticleCategoryRepository, _
                  ByVal commentRepo As IArticleCommentRepository, ByVal imageRepo As IImageRepository, ByVal articleTagRepo As IArticleTagRepository)
      _ArticleRepo = articleRepo
      _CommentRepo = commentRepo
      _ArticleCategoryRepo = articleCategoryRepo
      _ImageRepo = imageRepo
      _articleTagRepo = articleTagRepo
      _validationservice = validserv
   End Sub


   ''' <summary>
   ''' Get the average rating for an article
   ''' </summary>
   ''' <param name="articleId">The article id.</param>
   ''' <returns></returns>
   Public Function AverageRating(ByVal articleId As Integer) As Double Implements IArticleService.AverageRating
      Dim key As String = "articles_average_rating_" & articleId.ToString

      If Cache(key) IsNot Nothing Then
         Return CDbl(Cache(key))
      Else
         Dim val As Double = 0
         Dim wrd As Article = _ArticleRepo.GetArticles.Where(Function(p) p.ArticleID = articleId).FirstOrDefault()
         If wrd IsNot Nothing Then
            If wrd.Votes >= 1 Then
               val = (CDbl(wrd.TotalRating) / CDbl(wrd.Votes))
            End If
         End If
         CacheData(key, val)
      End If
   End Function


   ''' <summary>
   ''' Gets the comments.
   ''' </summary>
   ''' <param name="articleId">The article id.</param>
   ''' <returns></returns>
   Public Function GetComments(ByVal articleId As Integer) As IList(Of ArticleComment) Implements IArticleService.GetComments
      If articleId <= 0 Then
         Return GetComments(0, 30)
      End If

      Dim key As String = "articles_articleComments_" & articleId.ToString
      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), List(Of ArticleComment))
      Else
         Dim _comments As List(Of ArticleComment) = Nothing
         _comments = _CommentRepo.GetArticleComments.Where(Function(c) c.ArticleID = articleId).ToList
         CacheData(key, _comments)
         Return _comments
      End If
   End Function

   Public Function GetComments(ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As PagedList(Of ArticleComment) Implements IArticleService.GetComments
      Dim key As String = "articles_articleComments_" & startRowIndex.ToString & "_" & maximumRows.ToString
      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), PagedList(Of ArticleComment))
      Else
         Dim _comments As PagedList(Of ArticleComment)
         _comments = _CommentRepo.GetArticleComments.ToPagedList(startRowIndex, maximumRows)
         CacheData(key, _comments)
         Return _comments
      End If
   End Function

   Public Function CommentsCount(ByVal ArticleId As Integer) As Integer Implements IArticleService.CommentsCount

      'Dim key As String = ""
      'Dim mdc As New MCCDataContext
      'Return mdc.mcc_Comments.Count(Function(p) p.ArticleID = ArticleId)

      'Dim key As String = String.Format("articles_commentCount_{0}", ArticleId)

      Return _CommentRepo.GetArticleComments.WithArticleID(ArticleId).Count
   End Function

   Public Function Published(ByVal articleId As Integer) As Boolean Implements IArticleService.Published
      Dim key As String = "article_article_ispublished_" & articleId.ToString
      If Cache(key) IsNot Nothing Then
         Return CBool(Cache(key))
      Else
         Dim bApproved As Boolean = False
         Dim wrd As Article = _ArticleRepo.GetArticles.Where(Function(p) p.ArticleID = articleId).FirstOrDefault()
         If wrd IsNot Nothing Then
            bApproved = (wrd.Approved AndAlso wrd.ReleaseDate <= DateTime.Now AndAlso wrd.ExpireDate > DateTime.Now)
         End If
         CacheData(key, bApproved)
         Return bApproved
      End If
   End Function


   Public Function GetArticleCount() As Integer Implements IArticleService.GetArticleCount
      Dim key As String = "articles_articleCount_"

      If Cache(key) IsNot Nothing Then
         Return CInt(Cache(key))
      Else
         Dim it As Integer = _ArticleRepo.GetArticles.Count()
         CacheData(key, it)
         Return it
      End If
   End Function

   Public Function GetArticleCount(ByVal PublishedOnly As Boolean) As Integer Implements IArticleService.GetArticleCount
      If Not PublishedOnly Then
         Return GetArticleCount()
      End If

      Dim key As String = "articles_articleCount_" & PublishedOnly.ToString

      If Cache(key) IsNot Nothing Then
         Return CInt(Cache(key))
      Else
         Dim it As Integer = _ArticleRepo.GetArticles.Published.Count
         CacheData(key, it)
         Return it
      End If
   End Function


   Public Function GetArticleCountByAuthor(ByVal publishedOnly As Boolean, ByVal addedBy As String) As Integer Implements IArticleService.GetArticleCountByAuthor

      If Not publishedOnly Then
         GetArticleCountByAuthor(addedBy)
      End If

      If Not String.IsNullOrEmpty(addedBy) Then
         Dim key As String = "articles_articleCount_" & publishedOnly.ToString & "_" & addedBy & "_"

         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            Dim it As Integer = _ArticleRepo.GetArticles.Published.WithAuthor(addedBy).Count
            CacheData(key, it)
            Return it
         End If
      Else
         Return 0
      End If
   End Function


   Public Function GetArticleCountByAuthor(ByVal addedBy As String) As Integer Implements IArticleService.GetArticleCountByAuthor
      If String.IsNullOrEmpty(addedBy) Then
         Return 0
      End If
      Dim key As String = "articles_articleCount_" & addedBy & "_"
      If Cache(key) IsNot Nothing Then
         Return CInt(Cache(key))
      Else
         Dim it As Integer = _ArticleRepo.GetArticles.WithAuthor(addedBy).Count
         CacheData(key, it)
         Return it
      End If
   End Function

   Public Function GetArticleCount(ByVal categoryId As Integer) As Integer Implements IArticleService.GetArticleCount
      If categoryId <= 0 Then
         Return GetArticleCount()
      End If

      Dim key As String = "articles_articleCount_" & categoryId.ToString & "_"
      If Cache(key) IsNot Nothing Then
         Return CInt(Cache(key))
      Else
         Dim it As Integer = _ArticleRepo.GetArticlesInCategory(New Integer() {categoryId}).Count()
         CacheData(key, it)
         Return it
      End If
   End Function

   Public Function GetArticleCount(ByVal publishedOnly As Boolean, ByVal categoryID As Integer) As Integer Implements IArticleService.GetArticleCount
      If publishedOnly Then
         Return GetArticleCount(categoryID)
      End If

      If categoryID <= 0 Then
         Return GetArticleCount(publishedOnly)
      End If

      Dim key As String = "Articles_ArticleCount_" + publishedOnly.ToString() + "_" + categoryID.ToString()
      If Cache(key) IsNot Nothing Then
         Return CInt(Cache(key))
      Else
         Dim it As Integer = _ArticleRepo.GetArticlesInCategory(New Integer() {categoryID}).Published.Count
         CacheData(key, it)
         Return it
      End If
   End Function


   Public Function GetArticles() As List(Of Article) Implements IArticleService.GetArticles
      Dim key As String = "articles_articles_"
      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), List(Of Article))
      Else
         Dim li As List(Of Article) = _ArticleRepo.GetArticles().ToList
         CacheData(key, li)
         Return li
      End If
   End Function


   Public Function GetArticles(ByVal publishedOnly As Boolean, ByVal categoryId As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "releaseDate") As PagedList(Of Article) Implements IArticleService.GetArticles
      If Not publishedOnly Then
         Return GetArticles(categoryId, startrowindex, maximumrows, sortExp)
      End If

      If categoryId > 0 Then
         If String.IsNullOrEmpty(sortExp) Then
            sortExp = "ReleaseDate DESC"
         End If
         Dim key As String = "articles_articles_" & categoryId.ToString & "_" & sortExp.Replace(" ", "") & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Return DirectCast(Cache(key), PagedList(Of Article))
         Else
            Dim li As PagedList(Of Article) = _ArticleRepo.GetArticlesInCategory(New Integer() {categoryId}).Published.SortBy(sortExp).ToPagedList(startrowindex, maximumrows)
            CacheData(key, li)
            Return li
         End If
      Else
         Return GetArticles(publishedOnly, startrowindex, maximumrows, sortExp)
      End If
   End Function

   Public Function GetArticlesByCategoryName(ByVal publishedOnly As Boolean, ByVal categoryName As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "releaseDate") As PagedList(Of Article) Implements IArticleService.GetArticlesByCategoryName

      If String.IsNullOrEmpty(sortExp) Then
         sortExp = "ReleaseDate DESC"
      End If

      If Not publishedOnly Then
         Return GetArticlesByCategoryName(categoryName, startrowindex, maximumrows, sortExp)
      End If

      If String.IsNullOrEmpty(categoryName) Then
         Return GetArticles(publishedOnly, startrowindex, maximumrows, sortExp)
      End If


      Dim key As String = "articles_articles_" & categoryName & "_" & sortExp.Replace(" ", "") & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), PagedList(Of Article))
      Else
         Dim catIds As List(Of Integer) = _ArticleCategoryRepo.GetCategories.WithSlug(categoryName.Split(New Char() {","c}, StringSplitOptions.RemoveEmptyEntries)).Select(Function(q) q.CategoryID).ToList
            Dim childs As List(Of Integer) = _ArticleCategoryRepo.GetCategories.Where(Function(p) catIds.Contains(p.CategoryID) Or catIds.Contains(p.ParentCategoryID.Value)).Select(Function(r) r.CategoryID).ToList

         'catIds = childs.Except(catIds)

         Dim li As PagedList(Of Article) = _ArticleRepo.GetArticlesInCategory(childs.ToArray).Published.SortBy(sortExp).ToPagedList(startrowindex, maximumrows)
         CacheData(key, li)
         Return li
      End If
   End Function

   Public Function GetArticles(ByVal categoryId As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "releaseDate") As PagedList(Of Article) Implements IArticleService.GetArticles
      If categoryId > 0 Then
         Dim key As String = "articles_articles_" & categoryId.ToString & "_" & sortExp.Replace(" ", "") & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
         If String.IsNullOrEmpty(sortExp) Then
            sortExp = "ReleaseDate DESC"
         End If
         If Cache(key) IsNot Nothing Then
                Dim li As PagedList(Of Article) = DirectCast(Cache(key), PagedList(Of Article))
            Return li
         Else
            Dim li As PagedList(Of Article) = _ArticleRepo.GetArticlesInCategory(New Integer() {categoryId}).SortBy(sortExp).ToPagedList(startrowindex, maximumrows)
            CacheData(key, li)
            Return li
         End If
      Else
         Return GetArticles(startrowindex, maximumrows, sortExp)
      End If
   End Function

    Public Function GetArticlesByCategoryName(ByVal categoryName As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "releaseDate") As PagedList(Of Article) Implements IArticleService.GetArticlesByCategoryName
        If Not String.IsNullOrEmpty(categoryName) Then
            Dim key As String = "articles_articles_" & categoryName & "_" & sortExp.Replace(" ", "") & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"

            If String.IsNullOrEmpty(sortExp) Then
                sortExp = "ReleaseDate DESC"
            End If
            If Cache(key) IsNot Nothing Then
                Dim li As PagedList(Of Article) = DirectCast(Cache(key), PagedList(Of Article))
                Return li
            Else

                Dim catIds As List(Of Integer) = _ArticleCategoryRepo.GetCategories.WithSlug(categoryName.Split(New Char() {","c}, StringSplitOptions.RemoveEmptyEntries)).Select(Function(q) q.CategoryID).ToList
                Dim childs As List(Of Integer) = _ArticleCategoryRepo.GetCategories.Where(Function(p) catIds.Contains(p.CategoryID) Or catIds.Contains(p.ParentCategoryID.Value)).Select(Function(r) r.CategoryID).ToList

                'catIds = childs.Except(catIds)

                Dim li As PagedList(Of Article) = _ArticleRepo.GetArticlesInCategory(childs.ToArray).SortBy(sortExp).ToPagedList(startrowindex, maximumrows)
                CacheData(key, li)
                Return li
            End If
        Else
            Return GetArticles(startrowindex, maximumrows, sortExp)
        End If
    End Function


   Public Function GetArticlesByAuthor(ByVal addedBy As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedby") As PagedList(Of Article) Implements IArticleService.GetArticlesByAuthor
      If Not String.IsNullOrEmpty(addedBy) Then
         If String.IsNullOrEmpty(sortExp) Then
            sortExp = "ReleaseDate DESC"
         End If
         Dim key As String = "articles_articles_" & addedBy & "_" & sortExp.Replace(" ", "") & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Dim li As PagedList(Of Article) = DirectCast(Cache(key), PagedList(Of Article))
            Return li
         Else

            Dim li As PagedList(Of Article) = _ArticleRepo.GetArticles.WithAuthor(addedBy).SortBy(sortExp).ToPagedList(startrowindex, maximumrows)
            CacheData(key, li)
            Return li
         End If
      Else
         Return GetArticles(startrowindex, maximumrows, sortExp)
      End If
   End Function

   Public Function GetArticlesByAuthor(ByVal publishedOnly As Boolean, ByVal addedBy As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedby") As PagedList(Of Article) Implements IArticleService.GetArticlesByAuthor

      If Not publishedOnly Then
         Return GetArticlesByAuthor(addedBy, startrowindex, maximumrows, sortExp)
      End If

      If Not String.IsNullOrEmpty(addedBy) Then
         If String.IsNullOrEmpty(sortExp) Then
            sortExp = "ReleaseDate DESC"
         End If
         Dim key As String = "articles_articles_" & addedBy & "_" & publishedOnly.ToString & "_" & sortExp.Replace(" ", "") & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Return DirectCast(Cache(key), PagedList(Of Article))
         Else
            Dim li As PagedList(Of Article) = _ArticleRepo.GetArticles.Published.WithAuthor(addedBy).SortBy(sortExp).ToPagedList(startrowindex, maximumrows)
            CacheData(key, li)
            Return li
         End If
      Else
         Return GetArticles(startrowindex, maximumrows, sortExp)
      End If
   End Function

   Public Function GetArticles(ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "releasedate DESC") As PagedList(Of Article) Implements IArticleService.GetArticles
      Dim key As String = "articles_articles_" & startrowindex.ToString & "_" & sortExp.Replace(" ", "") & "_" & maximumrows.ToString & "_"
      If String.IsNullOrEmpty(sortExp) Then
         sortExp = "ReleaseDate DESC"
      End If
      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), PagedList(Of Article))
      Else
         Dim li As PagedList(Of Article) = _ArticleRepo.GetArticles.SortBy(sortExp).ToPagedList(startrowindex, maximumrows)
         CacheData(key, li)
         Return li
      End If
   End Function

   Public Function GetArticles(ByVal publishedOnly As Boolean, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "releasedate DESC") As PagedList(Of Article) Implements IArticleService.GetArticles
      If publishedOnly Then
         If String.IsNullOrEmpty(sortExp) Then
            sortExp = "ReleaseDate DESC"
         End If
         Dim key As String = "articles_articles_" & publishedOnly.ToString & "_" & sortExp.Replace(" ", "") & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
         If Cache(key) IsNot Nothing Then
                Dim li As PagedList(Of Article) = DirectCast(Cache(key), PagedList(Of Article))
            Return li
         Else
            Dim li As PagedList(Of Article) = _ArticleRepo.GetArticles.Published.SortBy(sortExp).ToPagedList(startrowindex, maximumrows)
            CacheData(key, li)
            Return li
         End If
      Else
         Return GetArticles(startrowindex, maximumrows, sortExp)
      End If
   End Function


   Public Function GetFrontPageArticles(ByVal publishedOnly As Boolean, ByVal CategoryToFilter As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "releasedate DESC") As PagedList(Of ArticleAdv) Implements IArticleService.GetFrontPageArticles
      If publishedOnly Then
         If String.IsNullOrEmpty(sortExp) Then
            sortExp = "ReleaseDate DESC"
         End If
         Dim key As String = "articles_articles_frontpage_" & publishedOnly.ToString & "_" & sortExp.Replace(" ", "") & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
         'If Cache(key) IsNot Nothing Then
         If False Then
            Return DirectCast(Cache(key), PagedList(Of ArticleAdv))
         Else
            Dim catId() As Integer = _ArticleCategoryRepo.GetCategories.WithSlug(CategoryToFilter.Split(New Char() {","c}, StringSplitOptions.RemoveEmptyEntries)).Select(Function(c) c.CategoryID).ToArray '(From it As mcc_Category In mdc.mcc_Categories _
            Dim noselectlist As List(Of Integer) = _ArticleRepo.GetArticlesInCategory(catId).Select(Function(p) p.ArticleID).ToList
            'Dim li As PagedList(Of ArticleAdv) = (From a As ArticleAdv In _ArticleRepo.GetAdvancedArticlesQuickList. _
            '                                      Where(Function(P) Not noselectlist.Contains(P.ArticleID)) _
            '                                  Join cp In _ImageRepo.GetImages() On cp.ImageID Equals a.ImageID _
            '                                  Select a).SortBy(sortExp).ToPagedList(startrowindex, maximumrows)

            Dim li As PagedList(Of ArticleAdv) = (From a As ArticleAdv In _ArticleRepo.GetAdvancedArticlesQuickList. _
                                               Where(Function(P) Not noselectlist.Contains(P.ArticleID))).SortBy(sortExp).ToPagedList(startrowindex, maximumrows)


            CacheData(key, li)
            Return li
         End If
      Else
         Return Nothing
      End If
   End Function

   Public Function GetSpotlightArticles(ByVal publishedOnly As Boolean, ByVal categoryName As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "releasedate DESC") As PagedList(Of Article) Implements IArticleService.GetSpotlightArticles
      If publishedOnly Then
         If String.IsNullOrEmpty(sortExp) Then
            sortExp = "ReleaseDate DESC"
         End If
         Dim key As String = "articles_articles_spotlight_" & publishedOnly.ToString & "_" & categoryName & "_" & sortExp.Replace(" ", "") & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Return DirectCast(Cache(key), PagedList(Of Article))
         Else
            Dim catId() As Integer = _ArticleCategoryRepo.GetCategories.WithSlug(categoryName.Split(New Char() {","c}, StringSplitOptions.RemoveEmptyEntries)).Select(Function(c) c.CategoryID).ToArray '(From it As mcc_Category In mdc.mcc_Categories _
            'Dim subcats() As Integer = _ArticleCategoryRepo.GetCategories.Where(Function(p) catId.Contains(p.ParentCategoryID))
            Dim li As PagedList(Of Article) = _ArticleRepo.GetArticlesInCategoryQuickList(catId).Published.SortBy(sortExp).ToPagedList(startrowindex, maximumrows)
            CacheData(key, li)
            Return li
         End If
      Else
         Return Nothing
      End If
   End Function


   ''' <summary>
   ''' Returns an Article object with the specified ID
   ''' </summary>
   Public Function GetLatestArticles(ByVal pageSize As Integer) As List(Of Article) Implements IArticleService.GetLatestArticles
      Dim Articles As New List(Of Article)
      Dim key As String = "Articles_Articles_Latest_" + pageSize.ToString

      If CacheObject.Cache(key) IsNot Nothing Then
         Articles = DirectCast(CacheObject.Cache(key), List(Of Article))
      Else
         Articles = _ArticleRepo.GetArticles.Published.SortBy("ReleaseDate DESC").ToPagedList(0, pageSize)
         CacheData(key, Articles)
      End If
      Return Articles
   End Function

   Public Function GetArticleById(ByVal ArticleId As Integer) As Article Implements IArticleService.GetArticleById
      Dim key As String = "articles_articles_" & ArticleId.ToString & "_"
      If Cache(key) IsNot Nothing Then
         Dim fb As Article = DirectCast(Cache(key), Article)
         Return fb
      Else
         Dim fb As Article = _ArticleRepo.GetArticles.WithArticleID(ArticleId).FirstOrDefault()
         CacheData(key, fb)
         Return fb
      End If
   End Function

   Public Function GetArticleBySlug(ByVal slug As String) As Article Implements IArticleService.GetArticleBySlug
      Dim key As String = "articles_articles_" & slug & "_"
      If Cache(key) IsNot Nothing Then
         Dim fb As Article = DirectCast(Cache(key), Article)
         Return fb
      Else

         Dim fb As Article = _ArticleRepo.GetArticles.WithSlug(slug).FirstOrDefault()

         CacheData(key, fb)
         Return fb
      End If
   End Function

   ''' <summary>
   ''' Creates a new Article
   ''' </summary>
   Public Function InsertArticle(ByVal wrd As Article) As Integer Implements IArticleService.InsertArticle

      ' on an inserted article the approved flagged will always be false.
      wrd.Approved = False


      wrd.Title = CacheObject.ConvertNullToEmptyString(wrd.Title)
      wrd.Abstract = CacheObject.ConvertNullToEmptyString(wrd.Abstract)
      wrd.Body = CacheObject.ConvertNullToEmptyString(wrd.Body)
      'wrd.country = CacheObject.ConvertNullToEmptyString(country)
      'wrd.state = CacheObject.ConvertNullToEmptyString(state)
      'wrd.city = CacheObject.ConvertNullToEmptyString(city)
      wrd.ImageNewsUrl = CacheObject.ConvertNullToEmptyString(wrd.ImageNewsUrl)
      wrd.ImageIconUrl = CacheObject.ConvertNullToEmptyString(wrd.ImageIconUrl)
      wrd.Tags = CacheObject.ConvertNullToEmptyString(wrd.Tags)

      If wrd.ReleaseDate = DateTime.MinValue Then
         wrd.ReleaseDate = DateTime.Now
      End If
      If wrd.ExpireDate = DateTime.MinValue Then
         wrd.ExpireDate = DateTime.MaxValue
      End If

      If String.IsNullOrEmpty(wrd.AddedBy) Then
         wrd.AddedBy = CacheObject.CurrentUserName
      End If

      With wrd
         .AddedDate = DateTime.Now
         '.AddedBy = CacheObject.CurrentUserName
         '.Title = title
         '.Abstract = Abstract
         '.Body = body
         '.ImageNewsUrl = ImageNewsUrl
         '.ImageIconUrl = ImageIconUrl
         '.OnlyForMembers = onlyForMembers
         '.Approved = approved
         '.Listed = listed
         '.CommentsEnabled = commentsEnabled
         '.Tags = tags
         '.ReleaseDate = releaseDate
         '.ExpireDate = expireDate
         '.Status = status
         '.VideoID = videoId
         '.PollId = pollId
         .Slug = wrd.Title.ToSlug
         '.ImageID = imageId
      End With

      Dim arId As Integer = _ArticleRepo.InsertArticle(wrd)


      Dim id As Integer = wrd.ArticleID
        Dim tagList As List(Of String) = wrd.Tags.Split(New Char() {","c}, StringSplitOptions.RemoveEmptyEntries).ToList

      If Not String.IsNullOrEmpty(wrd.Tags) AndAlso tagList.Count > 0 AndAlso id > 0 Then
         _articleTagRepo.TagArticle(id, tagList)
      End If

      CacheObject.PurgeCacheItems("Articles_")
      Return arId
   End Function


   Public Sub UpdateArticle(ByVal _art As Article) Implements IArticleService.UpdateArticle

      Dim wrd As Article = _ArticleRepo.GetArticles.WithArticleID(_art.ArticleID).FirstOrDefault
      If wrd IsNot Nothing Then

         _art.Title = CacheObject.ConvertNullToEmptyString(_art.Title)
         _art.Abstract = CacheObject.ConvertNullToEmptyString(_art.Abstract)
         _art.Body = CacheObject.ConvertNullToEmptyString(_art.Body)
         'country = CacheObject.ConvertNullToEmptyString(country)
         'state = CacheObject.ConvertNullToEmptyString(state)
         'city = CacheObject.ConvertNullToEmptyString(city)
         _art.ImageNewsUrl = CacheObject.ConvertNullToEmptyString(_art.ImageNewsUrl)
         _art.ImageIconUrl = CacheObject.ConvertNullToEmptyString(_art.ImageIconUrl)
         _art.Tags = CacheObject.ConvertNullToEmptyString(_art.Tags)

         If _art.ReleaseDate = DateTime.MinValue Then
            _art.ReleaseDate = DateTime.Now
         End If
         If _art.ExpireDate = DateTime.MinValue Then
            _art.ExpireDate = DateTime.MaxValue
         End If

         With wrd
            .Title = _art.Title
            .Abstract = _art.Abstract
            .Body = _art.Body
            .AddedBy = _art.AddedBy
            .ImageNewsUrl = _art.ImageNewsUrl
            .ImageIconUrl = _art.ImageIconUrl
            .OnlyForMembers = _art.OnlyForMembers
            .Approved = _art.Approved
            .Listed = _art.Listed
            .CommentsEnabled = _art.CommentsEnabled
            .Tags = _art.Tags
            .ReleaseDate = _art.ReleaseDate
            .ExpireDate = _art.ExpireDate
            .Status = _art.Status
            .VideoID = _art.VideoID
            .PollId = _art.PollId
            ' avoid updating the slug for update - it will break links ...
            .Slug = _art.Title.ToSlug
            .ImageID = _art.ImageID
         End With

         _ArticleRepo.UpdateArticle(wrd)

            Dim tagsToAdd As List(Of String) = _art.Tags.Split(New Char() {","c}, StringSplitOptions.RemoveEmptyEntries).ToList

         If Not String.IsNullOrEmpty(_art.Tags) AndAlso tagsToAdd.Count > 0 Then
            _articleTagRepo.TagArticle(wrd.ArticleID, tagsToAdd)
         End If

         CacheObject.PurgeCacheItems("Articles_")
      End If
   End Sub

   Public Sub DeleteArticles(ByVal ArticleIds() As Integer) Implements IArticleService.DeleteArticles

      If ArticleIds IsNot Nothing And ArticleIds.Count > 0 Then

         _ArticleRepo.DeleteArticles(ArticleIds)

         CacheObject.PurgeCacheItems("Articles_Article")
         CacheObject.PurgeCacheItems("Articles_Articles_")
         CacheObject.PurgeCacheItems("Articles_Articles_Latest_")
         CacheObject.PurgeCacheItems("Articles_Specified_Categories_" + ArticleIds.ToString)

         CacheObject.PurgeCacheItems("farticles_farticles")
         CacheObject.PurgeCacheItems("farticles_farticlecount_")

      End If



      '   End If
      'End If
   End Sub

   Public Sub DeleteArticle(ByVal ArticleId As Integer) Implements IArticleService.DeleteArticle
      If (ArticleId > 0) Then
         _ArticleRepo.DeleteArticle(ArticleId)
         CacheObject.PurgeCacheItems("Articles_")
         CacheObject.PurgeCacheItems("farticles_")
      End If
   End Sub

   Public Sub ApproveArticle(ByVal articleId As Integer) Implements IArticleService.ApproveArticle
      If articleId > 0 Then
         Dim q As Article = _ArticleRepo.GetArticles.WithArticleID(articleId).FirstOrDefault
         If q IsNot Nothing Then
            q.Approved = True
            _ArticleRepo.UpdateArticle(q)
            CacheObject.PurgeCacheItems("Articles_")
            CacheObject.PurgeCacheItems("farticles_")
         End If
      End If
   End Sub

   Public Function RemoveArticleFromCategory(ByVal ArticleID As Integer, Optional ByVal categoryId As Integer = 0) As Boolean Implements IArticleService.RemoveArticleFromCategory
      'Dim ret As Boolean = SiteProvider.Articles.RemoveArticleFromCategory(ArticleID, categoryId)
      'Dim ret As Boolean = True

      If ArticleID > 0 Then
         If categoryId > 0 Then
            _ArticleCategoryRepo.RemoveArticlesFromCategories(ArticleID, New Integer() {categoryId})
         Else
            _ArticleCategoryRepo.UnCategorizeArticle(ArticleID)
         End If

         CacheObject.PurgeCacheItems("Articles_")
      End If
      Return True
   End Function

   'Public Function AddArticleToCategory(ByVal ArticleId As Integer, ByVal CategoryId As Integer) As Integer
   '   'Dim ret As Integer = SiteProvider.Articles.AddArticleToCategory(ArticleId, CategoryId)
   '   If ArticleId > 0 AndAlso CategoryId > 0 Then
   '      _ArticleRepo.AddArticleToCategories(ArticleId, CategoryId)
   '      CacheObject.PurgeCacheItems("Articles_ArticleInCategories_")
   '   End If
   'End Function

   Public Function AddArticleToCategories(ByVal ArticleId As Integer, ByVal CategoryIds() As Integer) As Integer Implements IArticleService.AddArticleToCategories
      If ArticleId > 0 AndAlso CategoryIds IsNot Nothing Then
         _ArticleCategoryRepo.CategorizeArticle(ArticleId, CategoryIds)
         CacheObject.PurgeCacheItems("Articles_")
      End If
   End Function


   Public Sub IncrementViewCount(ByVal articleId As Integer) Implements IArticleService.IncrementViewCount
      If articleId > 0 Then
         Dim q = _ArticleRepo.GetArticles.WithArticleID(articleId).FirstOrDefault()
         If q IsNot Nothing Then
            q.ViewCount += 1
            _ArticleRepo.UpdateArticle(q)
            'CacheObject.PurgeCacheItems("articles_articles_")
            'CacheObject.PurgeCacheItems("articles_articleCount_")
         End If
      End If
   End Sub

   Public Sub RateArticle(ByVal articleId As Integer, ByVal rating As Integer) Implements IArticleService.RateArticle
      If articleId > 0 Then
         Dim q = _ArticleRepo.GetArticles.WithArticleID(articleId).FirstOrDefault
         If q IsNot Nothing Then
            q.Votes += 1
            q.TotalRating += rating
            _ArticleRepo.UpdateArticle(q)
         End If
      End If
   End Sub

   'Public Function FindArticles(ByVal where As mccEnum.SearchType, ByVal SearchWord As String) As List(Of Article)
   '   Return FindArticles(where, 0, CacheObject.MaxRows, SearchWord)
   'End Function


   'Public Function FindArticles(ByVal where As mccEnum.SearchType, ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal SearchWord As String) As PagedList(Of Article)
   '   Dim li As New List(Of Article)
   '   Dim mdc As New MCCDataContext
   '   Select Case where
   '      Case mccEnum.SearchType.AnyWord
   '         Dim q = Article.ContainsAny(SearchWord.Split().ToArray)
   '         li = (From it As Article In mdc.mcc_Articles.Where(q)).Distinct().ToList()
   '      Case mccEnum.SearchType.AllWords

   '      Case mccEnum.SearchType.ExactPhrase
   '         li = _ArticleRepo.GetArticles.Published.Where(Function(it) it.Tags.Contains(SearchWord) Or it.Title.Contains(SearchWord) Or it.Abstract.Contains(SearchWord) _
   '                Or it.Body.Contains(SearchWord)).ToPagedList(startRowIndex, maximumRows)
   '   End Select
   '   Return li
   'End Function

   Public Function FindArticlesWithExactMatch(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal SearchWord As String) As PagedList(Of Article) Implements IArticleService.FindArticlesWithExactMatch
      Dim key As String = String.Format("article_search_{0}_{1}_{2}", SearchWord, startRowIndex, maximumRows)
      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), PagedList(Of Article))
      End If
      Dim li As PagedList(Of Article) = _ArticleRepo.GetArticlesQuickList.Published.Where(Function(it) it.Tags.Contains(SearchWord) Or it.Title.Contains(SearchWord) Or it.Abstract.Contains(SearchWord) _
                 Or it.Body.Contains(SearchWord)).ToPagedList(startRowIndex, maximumRows)

      CacheData(key, li)
      Return li
   End Function


   Public Function FindArticlesWithAnyMatch(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal SearchWord As String) As PagedList(Of Article) Implements IArticleService.FindArticlesWithAnyMatch
      Dim key As String = String.Format("article_search_{0}_{1}_{2}", SearchWord, startRowIndex, maximumRows)
      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), PagedList(Of Article))
      End If

      'Dim q = Article.ContainsAny(SearchWord.Split().ToArray)
      'li = (From it As Article In mdc.mcc_Articles.Where(q)).Distinct().ToList()

        Dim searchKeys() As String = SearchWord.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)

      Dim li As PagedList(Of Article) = _ArticleRepo.GetArticlesQuickList.Published.Where(Function(it) it.Tags.ContainsAny(searchKeys, True) Or it.Title.ContainsAny(searchKeys, True) Or it.Abstract.ContainsAny(searchKeys, True) _
                 Or it.Body.ContainsAny(searchKeys, True)).ToPagedList(startRowIndex, maximumRows)

      '_ArticleRepo.GetArticles.Published.Where(Function(p) p.ContainsAny)
      '_ArticleRepo.GetArticlesQuickList.Published.Where().ToPagedList(startRowIndex, maximumRows)

      CacheData(key, li)
      Return li
   End Function

    Public Function FindArticleWithExactMatchCount(ByVal SearchWord As String) As Integer Implements IArticleService.FindArticleWithExactMatchCount
        Return _ArticleRepo.GetArticlesQuickList.Published.Where(Function(it) it.Tags.Contains(SearchWord) Or it.Title.Contains(SearchWord) Or it.Abstract.Contains(SearchWord) _
                   Or it.Body.Contains(SearchWord)).Count
    End Function

    Public Function FindArticleWithAnyMatchCount(ByVal SearchWord As String) As Integer Implements IArticleService.FindArticleWithAnyMatchCount
        Return _ArticleRepo.GetArticlesQuickList.Published.Where(Function(it) it.Tags.Contains(SearchWord) Or it.Title.Contains(SearchWord) Or it.Abstract.Contains(SearchWord) _
                   Or it.Body.Contains(SearchWord)).Count
    End Function


   '''' <summary>
   '''' Returns the number of total articles matching the search key
   '''' </summary>
   'Public Function FindArticlesCount(ByVal where As mccEnum.SearchType, ByVal searchWord As String) As Integer
   '   Dim artCount As Integer = 0
   '   Select Case where
   '      Case mccEnum.SearchType.AnyWord
   '         Dim q = Article.ContainsAny(searchWord.Split().ToArray)

   '         artCount = (From it As Article In mdc.mcc_Articles.Where(q)).Count()
   '      Case mccEnum.SearchType.ExactPhrase
   '         artCount = _ArticleRepo.GetArticles.Published.Count(Function(p) p.Tags.Contains(searchWord) Or p.Abstract.Contains(searchWord) _
   '                                                    Or p.Title.Contains(searchWord) Or p.Body.Contains(searchWord))
   '   End Select
   '   Return artCount
   'End Function


   Public Function GetArticlesCountByStatus(ByVal status As Integer) As Integer Implements IArticleService.GetArticlesCountByStatus
      Dim key As String = "articles_articlecount_" & "st_" & status.ToString
      If Cache(key) IsNot Nothing Then
         Return CInt(Cache(key))
      Else
         Dim val As Integer = 0
         If status < 0 Then
            val = _ArticleRepo.GetArticles.Count
         Else
            val = _ArticleRepo.GetArticles.Where(Function(p) p.Status = status).Count
         End If
         CacheData(key, val)
      End If
   End Function

   Public Function GetArticlesCountInCategoryByStatus(ByVal categoryID As Integer, ByVal status As Integer) As Integer Implements IArticleService.GetArticlesCountInCategoryByStatus

      Dim key As String = "article_articlecount_" & categoryID.ToString & "_" & "st_" & status.ToString

      If (Cache(key) IsNot Nothing) Then
         Return CInt(Cache(key))
      Else
         If categoryID <= 0 Then
            Return GetArticlesCountByStatus(status)
         End If
         Dim cnt As Integer = _ArticleRepo.GetArticlesInCategory(New Integer() {categoryID}).Where(Function(p) p.Status = status).Count
         CacheData(key, cnt)
         Return cnt
      End If
   End Function

   Public Function GetArticlesByStatus(ByVal status As Integer, ByVal startRowIndex As Integer, ByVal maximumRows As Integer, Optional ByVal sortExp As String = "Title") As PagedList(Of Article) Implements IArticleService.GetArticlesByStatus
      If status < 0 Then
         Return GetArticles(startRowIndex, maximumRows, sortExp)
      End If

      Dim key As String = "articles_articles_" & "status_" & status.ToString & "_" & startRowIndex.ToString & "_" & maximumRows.ToString & "_" & sortExp

      If (Cache(key) IsNot Nothing) Then
            Return DirectCast(Cache(key), PagedList(Of Article))
      Else
         Dim li As PagedList(Of Article) = _ArticleRepo.GetArticles.Where(Function(p) p.Status = status).SortBy(sortExp).ToPagedList(startRowIndex, maximumRows)
         CacheData(key, li)
         Return li
      End If
   End Function

   Public Function GetArticlesByStatus(ByVal categoryID As Integer, ByVal status As Integer, ByVal startRowIndex As Integer, ByVal maximumRows As Integer, Optional ByVal sortExp As String = "AddedDate") As PagedList(Of Article) Implements IArticleService.GetArticlesByStatus
      If categoryID <= 0 Then
         Return GetArticlesByStatus(status, startRowIndex, maximumRows, sortExp)
      End If

      Dim key As String = "articles_articles_" & categoryID.ToString & "_" & "st_" & status.ToString & "_" & startRowIndex.ToString & "_" & maximumRows.ToString & "_" & sortExp

      If (Cache(key) IsNot Nothing) Then
            Return DirectCast(Cache(key), PagedList(Of Article))
      Else
         Dim li As PagedList(Of Article) = _ArticleRepo.GetArticlesInCategory(New Integer() {categoryID}).Where(Function(p) p.Status = status).ToPagedList(startRowIndex, maximumRows)
         CacheData(key, li)
         Return li
      End If
   End Function


   Public Sub UpdateArticleStatus(ByVal id As Integer) Implements IArticleService.UpdateArticleStatus
      If id > 0 Then
         Dim q = _ArticleRepo.GetArticles.WithArticleID(id).FirstOrDefault()
         If q IsNot Nothing Then
            q.Status += 1
            _ArticleRepo.UpdateArticle(q)
            CacheObject.PurgeCacheItems("articles_")
         End If
      End If
   End Sub

   Public Sub UpdateArticleStatus(ByVal id As Integer, ByVal st As Integer) Implements IArticleService.UpdateArticleStatus
      If id > 0 Then
         Dim q = _ArticleRepo.GetArticles.WithArticleID(id).FirstOrDefault()
         If q IsNot Nothing AndAlso st <= 5 AndAlso st >= 0 Then
            q.Status = st
            _ArticleRepo.UpdateArticle(q)
            CacheObject.PurgeCacheItems("articles_")
         End If
      End If
   End Sub

   'Public Sub UpdateArticleStatus(ByVal id As Integer, ByVal st As mccEnum.Status)
   '   Dim it As Article = (From ars As Article In mdc.mcc_Articles Where ars.ArticleID = id).FirstOrDefault
   '   If it IsNot Nothing Then
   '      If st <= 5 AndAlso st >= 0 Then
   '         it.Status = CInt(st)

   '         If st <> mccEnum.Status.Approved Then
   '            it.Approved = False
   '         Else
   '            it.Approved = True
   '         End If

   '         mdc.SubmitChanges()

   '         CacheObject.PurgeCacheItems("articles_")
   '      End If
   '   End If
   'End Sub

   Public Function GetArticleStatusValue(ByVal id As Integer) As Integer Implements IArticleService.GetArticleStatusValue
      Dim key As String = "articles_article_statusval_" & id.ToString
      If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
      Else
         Dim val As Integer = -1
         val = _ArticleRepo.GetArticles.WithArticleID(id).Select(Function(P) P.Status).FirstOrDefault()
         CacheData(key, val)
         Return val
      End If
   End Function

   'Public Function GetArticleStatus(ByVal articleId As Integer) As mccEnum.Status
   '   Dim key As String = "articles_article_status_" & articleId.ToString
   '   If Cache(key) Then
   '      Return Cache(key)
   '   Else
   '      Dim stat As mccEnum.Status = mccEnum.Status.Pending
   '      
   '      Dim ival As Integer = (From it As Article In mdc.mcc_Articles Where it.ArticleID = articleId Select it.Status).FirstOrDefault
   '      stat = (CType(ival, mccEnum.Status))
   '      CacheData(key, stat)
   '      Return stat
   '   End If
   'End Function

   Public Function FixArticlesVideoIds() As Boolean Implements IArticleService.FixArticlesVideoIds

      'Dim li As List(Of Article) = (From it As Article In mdc.mcc_Articles Where it.VideoId Is Nothing).ToList()
      'For Each it As Article In li
      '   it.VideoId = 0
      'Next
      'mdc.SubmitChanges()
      'CacheObject.PurgeCacheItems("articles_")
      'CacheObject.PurgeCacheItems("farticles_")
      'Return True

   End Function

   Public Function GetReadersPick() As Article Implements IArticleService.GetReadersPick
      Dim ar As Article = Nothing
      Dim key As String = "articles_readerpick_"

      If Cache(key) IsNot Nothing Then
         Return CType(Cache(key), Article)
      Else
         ar = _ArticleRepo.GetArticles.SortBy("viewcount DESC").FirstOrDefault()
         CacheData(key, ar)
      End If
      Return ar
   End Function

   Public Function SaveArticle(ByVal ar As Data.Article, ByVal categories() As Integer, ByVal ads() As Integer) As Boolean Implements IArticleService.SaveArticle
      If ar Is Nothing Then
         Return False
      End If

      'If Not ValidateArticle(ar) Then
      '   Return _validationservice.IsValid
      'End If

      Dim idx As Integer = 0
      If ar.ArticleID > 0 Then
         idx = ar.ArticleID
         Me.UpdateArticle(ar)
      Else
         idx = Me.InsertArticle(ar)
      End If

      If idx <= 0 Then
         Return False
      End If

      If categories IsNot Nothing AndAlso categories.Count > 0 Then
         _ArticleCategoryRepo.CategorizeArticle(idx, categories)
      Else
         _ArticleCategoryRepo.UnCategorizeArticle(idx)
      End If

      If ads IsNot Nothing AndAlso ads.Count > 0 Then
         AdRepo.InsertArticleAds(idx, ads)
      Else
         AdRepo.DeleteArticleAds(idx)
      End If

      Return True
   End Function

   Function ValidateArticle(ByVal ar As Article) As Boolean
      If String.IsNullOrEmpty(ar.Title.Trim) Then
         _validationservice.AddError("Title", "Please add a title for this article")
      End If
      If String.IsNullOrEmpty(ar.Abstract.Trim) Then
         _validationservice.AddError("Abstract", "Please add an Abstract for this article")
      End If
      If String.IsNullOrEmpty(ar.Body.Trim) Then
         _validationservice.AddError("Body", "Please add a Body for this article")
      End If
      If String.IsNullOrEmpty(ar.ImageNewsUrl.Trim) Then
         _validationservice.AddError("ImageNewsUrl", "Please add an ImageNewsUrl for this article")
      End If
      If String.IsNullOrEmpty(ar.ImageIconUrl.Trim) Then
         _validationservice.AddError("ImageIconUrl", "Please add an ImageIconUrl for this article")
      End If
      'If String.IsNullOrEmpty(ar.Title.Trim) Then
      '   _validationservice.AddError("Title", "Please add a title for this article")
      'End If
      'If String.IsNullOrEmpty(ar.Title.Trim) Then
      '   _validationservice.AddError("Title", "Please add a title for this article")
      'End If


      Return _validationservice.IsValid
   End Function

   Protected Sub CacheData(ByVal key As String, ByVal data As Object)
      If data IsNot Nothing Then
         CacheObject.Cache.Insert(key, data, Nothing, DateTime.Now.AddSeconds(3600), TimeSpan.Zero)
      End If
   End Sub
End Class
