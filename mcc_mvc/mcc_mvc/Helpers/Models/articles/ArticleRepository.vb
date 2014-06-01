Imports System
Imports System.Data
Imports System.Configuration
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports System.Collections.Generic
Imports MCC.routines
Imports MCCEvents
Imports System.Linq
Imports System.Linq.Expressions
Imports MCC.Dynamic
Imports System.Data.Linq.SqlClient

Namespace Articles

   ''' <summary>
   ''' 
   ''' </summary>
   Public Class ArticleRepository
      Inherits mccObject
      Implements IArticleRepository

      ''' <summary>
      ''' Get the average rating for an article
      ''' </summary>
      ''' <param name="articleId">The article id.</param>
      ''' <returns></returns>
      Public Function AverageRating(ByVal articleId As Integer) As Double Implements IArticleRepository.AverageRating

         Dim key As String = "articles_article_average_rating_" & articleId.ToString

         If Cache(key) IsNot Nothing Then
            Return CDbl(Cache(key))
         Else
            Dim val As Double = 0
            Dim mdc As MCCDataContext = New MCCDataContext
            Dim wrd As mcc_Article = (From t In mdc.mcc_Articles _
                                        Where t.ArticleID = articleId _
                                        Select t).FirstOrDefault
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
      Public Function GetComments(ByVal articleId As Integer) As List(Of mcc_Comment) Implements IArticleRepository.GetComments
         Dim key As String = "articles_articleComments_" & articleId.ToString
         If Cache(key) IsNot Nothing Then
            Return DirectCast(Cache(key), List(Of mcc_Comment))
         Else
            Dim mdc As New MCCDataContext()
            Dim _comments As List(Of mcc_Comment) = Nothing

            _comments = (From c As mcc_Comment In mdc.mcc_Comments Where c.ArticleID = articleId).ToList

            CacheData(key, _comments)
            Return _comments
         End If
      End Function

      Public Function CommentsCount(ByVal ArticleId As Integer) As Integer Implements IArticleRepository.CommentsCount
         'Dim key As String = ""
         'Dim mdc As New MCCDataContext
         'Return mdc.mcc_Comments.Count(Function(p) p.ArticleID = ArticleId)
         Return ArticleCommentRepository.GetArticleCommentCount(ArticleId)
      End Function

      Public Function Published(ByVal articleId As Integer) As Boolean Implements IArticleRepository.Published
         Dim key As String = "article_article_ispublished_" & articleId.ToString

         If Cache(key) IsNot Nothing Then
            Return CBool(Cache(key))
         Else
            Dim bApproved As Boolean = False
            Dim mdc As MCCDataContext = New MCCDataContext
            Dim wrd As mcc_Article = (From t In mdc.mcc_Articles _
                                        Where t.ArticleID = articleId _
                                        Select t).FirstOrDefault
            If wrd IsNot Nothing Then
               bApproved = (wrd.Approved AndAlso wrd.ReleaseDate <= DateTime.Now AndAlso wrd.ExpireDate > DateTime.Now)
            End If

            CacheData(key, bApproved)
            Return bApproved
         End If
      End Function


      Public Function GetArticleCount() As Integer Implements IArticleRepository.GetArticleCount
         Dim key As String = "articles_articleCount_"

         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            Dim mdc As New MCCDataContext()
            Dim it As Integer = mdc.mcc_Articles.Count()
            CacheData(key, it)
            Return it
         End If
      End Function

      Public Function GetArticleCount(ByVal PublishedOnly As Boolean) As Integer Implements IArticleRepository.GetArticleCount
         If Not PublishedOnly Then
            Return GetArticleCount()
         End If

         Dim key As String = "articles_articleCount_" & PublishedOnly.ToString

         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            Dim mdc As New MCCDataContext()
            Dim it As Integer = mdc.mcc_Articles.Count(Function(p) p.Approved = True AndAlso p.ReleaseDate <= DateTime.Now AndAlso p.ExpireDate > DateTime.Now)
            CacheData(key, it)
            Return it
         End If
      End Function

      Public Function GetFrontPageArticleCount(ByVal frontPage As Boolean, ByVal PublishedOnly As Boolean) As Integer Implements IArticleRepository.GetFrontPageArticleCount
         If Not frontPage Then
            Return GetArticleCount(PublishedOnly)
         Else
            If Not PublishedOnly Then
               Return GetArticleCount()
            End If

            Dim key As String = "articles_articleCount_frontpage_" & PublishedOnly.ToString

            If Cache(key) IsNot Nothing Then
               Return CInt(Cache(key))
            Else
               Dim mdc As New MCCDataContext()
               Dim li As List(Of Integer) = (From k As mcc_ArticleCategory In mdc.mcc_ArticleCategories Where k.CategoryId = 1 Select k.ArticleId).ToList
               Dim it As Integer = mdc.mcc_Articles.Count(Function(p) p.Approved = True AndAlso p.ReleaseDate <= DateTime.Now AndAlso p.ExpireDate > DateTime.Now _
                                                               AndAlso Not li.Contains(p.ArticleID))
               CacheData(key, it)
               Return it
            End If
         End If
      End Function

      Public Function GetArticleCountByAuthor(ByVal publishedOnly As Boolean, ByVal addedBy As String) As Integer Implements IArticleRepository.GetArticleCountByAuthor
         If Not String.IsNullOrEmpty(addedBy) Then
            Dim key As String = "articles_articleCount_" & publishedOnly.ToString & "_" & addedBy & "_"

            If Cache(key) IsNot Nothing Then
               Return CInt(Cache(key))
            Else
               Dim mdc As New MCCDataContext()
               Dim it As Integer = mdc.mcc_Articles.Count(Function(p) p.AddedBy = addedBy AndAlso p.Approved = True)
               CacheData(key, it)
               Return it
            End If
         Else
            Return 0
         End If
      End Function


      Public Function GetArticleCountByAuthor(ByVal addedBy As String) As Integer Implements IArticleRepository.GetArticleCountByAuthor
         If Not String.IsNullOrEmpty(addedBy) Then
            Dim key As String = "articles_articleCount_" & addedBy & "_"

            If Cache(key) IsNot Nothing Then
               Return CInt(Cache(key))
            Else
               Dim mdc As New MCCDataContext()
               Dim it As Integer = mdc.mcc_Articles.Count(Function(p) p.AddedBy = addedBy)
               CacheData(key, it)
               Return it
            End If
         Else
            Return 0
         End If
      End Function

      Public Function GetArticleCount(ByVal categoryId As Integer) As Integer Implements IArticleRepository.GetArticleCount
         If categoryId <= 0 Then
            Return GetArticleCount()
         End If

         Dim key As String = "articles_articleCount_" & categoryId.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            Dim mdc As New MCCDataContext()
            Dim catAr As List(Of Integer) = (From ef As mcc_ArticleCategory In mdc.mcc_ArticleCategories Where ef.CategoryId = categoryId Select ef.ArticleId).ToList
            Dim it As Integer = mdc.mcc_Articles.Count(Function(p) catAr.Contains(p.ArticleID))

            CacheData(key, it)
            Return it
         End If
      End Function

      Public Function GetArticleCount(ByVal publishedOnly As Boolean, ByVal categoryID As Integer) As Integer Implements IArticleRepository.GetArticleCount
         If Not publishedOnly Then
            Return GetArticleCount(categoryID)
         End If

         If categoryID <= 0 Then
            Return GetArticleCount(publishedOnly)
         End If

         Dim key As String = "Articles_ArticleCount_" + publishedOnly.ToString() + "_" + categoryID.ToString()
         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            Dim mdc As New MCCDataContext()
            Dim catAr As List(Of Integer) = (From ef As mcc_ArticleCategory In mdc.mcc_ArticleCategories Where ef.CategoryId = categoryID Select ef.ArticleId).ToList

            Dim it As Integer = mdc.mcc_Articles.Count(Function(p) catAr.Contains(p.ArticleID) AndAlso p.Approved = True _
            AndAlso p.ReleaseDate <= DateTime.Now AndAlso p.ExpireDate > DateTime.Now)

            CacheData(key, it)
            Return it
         End If
      End Function


      Public Function GetArticleCountByMainCategory(ByVal publishedOnly As Boolean, ByVal categoryID As Integer) As Integer Implements IArticleRepository.GetArticleCountByMainCategory
         If Not publishedOnly Then
            Return GetArticleCount(categoryID)
         End If

         If categoryID <= 0 Then
            Return GetArticleCount(publishedOnly)
         End If

         Dim key As String = "Articles_ArticleCount_" + publishedOnly.ToString() + "_" + categoryID.ToString()
         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            Dim mdc As New MCCDataContext()
            Dim catAr As List(Of Integer) = (From ef As mcc_ArticleCategory In mdc.mcc_ArticleCategories Where ef.CategoryId = categoryID Select ef.ArticleId).ToList

            Dim it As Integer = mdc.mcc_Articles.Count(Function(p) catAr.Contains(p.ArticleID) AndAlso p.Approved = True _
            AndAlso p.ReleaseDate <= DateTime.Now AndAlso p.ExpireDate > DateTime.Now)

            CacheData(key, it)
            Return it
         End If
      End Function

      Public Function GetArticles() As List(Of mcc_Article) Implements IArticleRepository.GetArticles
         Dim key As String = "articles_articles_"
         If Cache(key) IsNot Nothing Then
            Dim li As List(Of mcc_Article) = DirectCast(Cache(key), List(Of mcc_Article))
            Return li
         Else
            Dim mdc As New MCCDataContext()
            Dim li As List(Of mcc_Article) = mdc.mcc_Articles.ToList
            CacheData(key, li)
            Return li
         End If
      End Function


      Public Function GetArticles(ByVal publishedOnly As Boolean, ByVal categoryId As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "releaseDate") As List(Of mcc_Article) Implements IArticleRepository.GetArticles
         If Not publishedOnly Then
            Return GetArticles(categoryId, startrowindex, maximumrows, sortExp)
         End If

         If categoryId > 0 Then
            If String.IsNullOrEmpty(sortExp) Then
               sortExp = "ReleaseDate DESC"
            End If
            Dim key As String = "articles_articles_" & categoryId.ToString & "_" & sortExp.Replace(" ", "") & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
            If Cache(key) IsNot Nothing Then
               Dim li As List(Of mcc_Article) = DirectCast(Cache(key), List(Of mcc_Article))
               Return li
            Else
               Dim mdc As New MCCDataContext()
               Dim li As List(Of mcc_Article)

               Dim catAr As List(Of Integer) = (From ef As mcc_ArticleCategory In mdc.mcc_ArticleCategories Where ef.CategoryId = categoryId Select ef.ArticleId).ToList
               li = (From it As mcc_Article In mdc.mcc_Articles Where catAr.Contains(it.ArticleID) AndAlso it.Approved = True _
                        AndAlso it.ReleaseDate <= DateTime.Now AndAlso it.ExpireDate > DateTime.Now).SortBy(sortExp).Skip(startrowindex).Take(maximumrows).ToList
               CacheData(key, li)
               Return li
            End If
         Else
            Return GetArticles(publishedOnly, startrowindex, maximumrows, sortExp)
         End If
      End Function

      Public Function GetArticlesByCategoryName(ByVal publishedOnly As Boolean, ByVal categoryName As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "releaseDate") As List(Of mcc_Article) Implements IArticleRepository.GetArticlesByCategoryName
         If Not publishedOnly Then
            Return GetArticlesByCategoryName(categoryName, startrowindex, maximumrows, sortExp)
         End If

         If Not String.IsNullOrEmpty(categoryName) Then
            If String.IsNullOrEmpty(sortExp) Then
               sortExp = "ReleaseDate DESC"
            End If
            Dim key As String = "articles_articles_" & categoryName & "_" & sortExp.Replace(" ", "") & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
            If Cache(key) IsNot Nothing Then
               Return DirectCast(Cache(key), List(Of mcc_Article))
            Else
               Dim mdc As New MCCDataContext()
               Dim li As List(Of mcc_Article)

               Dim catId As Integer = (From it As mcc_Category In mdc.mcc_Categories _
                       Where it.Slug.ToLower = categoryName Select it.CategoryID).Single()


               Dim catAr As List(Of Integer) = (From ef As mcc_ArticleCategory In mdc.mcc_ArticleCategories Where ef.CategoryId = catId Select ef.ArticleId).ToList
               li = (From it As mcc_Article In mdc.mcc_Articles Where catAr.Contains(it.ArticleID) AndAlso it.Approved = True _
                        AndAlso it.ReleaseDate <= DateTime.Now AndAlso it.ExpireDate > DateTime.Now).SortBy(sortExp).Skip(startrowindex).Take(maximumrows).ToList
               CacheData(key, li)
               Return li
            End If
         Else
            Return GetArticles(publishedOnly, startrowindex, maximumrows, sortExp)
         End If
      End Function

      Public Function GetArticles(ByVal categoryId As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "releaseDate") As List(Of mcc_Article) Implements IArticleRepository.GetArticles
         If categoryId > 0 Then
            Dim key As String = "articles_articles_" & categoryId.ToString & "_" & sortExp.Replace(" ", "") & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
            If String.IsNullOrEmpty(sortExp) Then
               sortExp = "ReleaseDate DESC"
            End If
            If Cache(key) IsNot Nothing Then
               Dim li As List(Of mcc_Article) = DirectCast(Cache(key), List(Of mcc_Article))
               Return li
            Else
               Dim mdc As New MCCDataContext()
               Dim li As List(Of mcc_Article)

               Dim catAr As List(Of Integer) = (From ef As mcc_ArticleCategory In mdc.mcc_ArticleCategories Where ef.CategoryId = categoryId Select ef.ArticleId).ToList

               li = (From it As mcc_Article In mdc.mcc_Articles Where catAr.Contains(it.ArticleID)).SortBy(sortExp).Skip(startrowindex).Take(maximumrows).ToList
               CacheData(key, li)
               Return li
            End If
         Else
            Return GetArticles(startrowindex, maximumrows, sortExp)
         End If
      End Function

      Public Function GetArticlesByCategoryName(ByVal categoryName As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "releaseDate") As List(Of mcc_Article) Implements IArticleRepository.GetArticlesByCategoryName
         If Not String.IsNullOrEmpty(categoryName) Then
            Dim key As String = "articles_articles_" & categoryName & "_" & sortExp.Replace(" ", "") & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"

            If String.IsNullOrEmpty(sortExp) Then
               sortExp = "ReleaseDate DESC"
            End If
            If Cache(key) IsNot Nothing Then
               Dim li As List(Of mcc_Article) = DirectCast(Cache(key), List(Of mcc_Article))
               Return li
            Else
               Dim mdc As New MCCDataContext()
               Dim li As List(Of mcc_Article)

               Dim catId As Integer = (From it As mcc_Category In mdc.mcc_Categories _
                       Where it.Slug.ToLower = categoryName Select it.CategoryID).Single()

               Dim catAr As List(Of Integer) = (From ef As mcc_ArticleCategory In mdc.mcc_ArticleCategories Where ef.CategoryId = catId Select ef.ArticleId).ToList

               li = (From it As mcc_Article In mdc.mcc_Articles Where catAr.Contains(it.ArticleID)).SortBy(sortExp).Skip(startrowindex).Take(maximumrows).ToList
               CacheData(key, li)
               Return li
            End If
         Else
            Return GetArticles(startrowindex, maximumrows, sortExp)
         End If
      End Function


      Public Function GetArticlesByAuthor(ByVal addedBy As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedby") As List(Of mcc_Article) Implements IArticleRepository.GetArticlesByAuthor
         If Not String.IsNullOrEmpty(addedBy) Then
            If String.IsNullOrEmpty(sortExp) Then
               sortExp = "ReleaseDate DESC"
            End If
            Dim key As String = "articles_articles_" & addedBy & "_" & sortExp.Replace(" ", "") & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
            If Cache(key) IsNot Nothing Then
               Dim li As List(Of mcc_Article) = DirectCast(Cache(key), List(Of mcc_Article))
               Return li
            Else
               Dim mdc As New MCCDataContext()
               Dim li As List(Of mcc_Article)
               li = (From it As mcc_Article In mdc.mcc_Articles Where it.AddedBy = addedBy).SortBy(sortExp).Skip(startrowindex).Take(maximumrows).ToList
               CacheData(key, li)
               Return li
            End If
         Else
            Return GetArticles(startrowindex, maximumrows, sortExp)
         End If
      End Function

      Public Function GetArticlesByAuthor(ByVal publishedOnly As Boolean, ByVal addedBy As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedby") As List(Of mcc_Article) Implements IArticleRepository.GetArticlesByAuthor

         If Not publishedOnly Then
            Return GetArticlesByAuthor(addedBy, startrowindex, maximumrows, sortExp)
         End If

         If Not String.IsNullOrEmpty(addedBy) Then
            If String.IsNullOrEmpty(sortExp) Then
               sortExp = "ReleaseDate DESC"
            End If
            Dim key As String = "articles_articles_" & addedBy & "_" & publishedOnly.ToString & "_" & sortExp.Replace(" ", "") & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
            If Cache(key) IsNot Nothing Then
               Dim li As List(Of mcc_Article) = DirectCast(Cache(key), List(Of mcc_Article))
               Return li
            Else
               Dim mdc As New MCCDataContext()
               Dim li As List(Of mcc_Article)
               li = (From it As mcc_Article In mdc.mcc_Articles Where it.AddedBy = addedBy AndAlso it.Approved = True _
                     AndAlso it.ReleaseDate <= DateTime.Now AndAlso it.ExpireDate > DateTime.Now).SortBy(sortExp).Skip(startrowindex).Take(maximumrows).ToList
               CacheData(key, li)
               Return li
            End If
         Else
            Return GetArticles(startrowindex, maximumrows, sortExp)
         End If
      End Function

      Public Function GetArticles(ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "releasedate DESC") As List(Of mcc_Article) Implements IArticleRepository.GetArticles
         Dim key As String = "articles_articles_" & startrowindex.ToString & "_" & sortExp.Replace(" ", "") & "_" & maximumrows.ToString & "_"
         If String.IsNullOrEmpty(sortExp) Then
            sortExp = "ReleaseDate DESC"
         End If
         If Cache(key) IsNot Nothing Then
            Dim li As List(Of mcc_Article) = DirectCast(Cache(key), List(Of mcc_Article))
            Return li
         Else
            Dim mdc As New MCCDataContext()

            Dim li As New List(Of mcc_Article)

            li = mdc.mcc_Articles.SortBy(sortExp).Skip(startrowindex).Take(maximumrows).ToList
            CacheData(key, li)

            Return li
         End If
      End Function

      Public Function GetArticles(ByVal publishedOnly As Boolean, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "releasedate DESC") As List(Of mcc_Article) Implements IArticleRepository.GetArticles

         If publishedOnly Then
            If String.IsNullOrEmpty(sortExp) Then
               sortExp = "ReleaseDate DESC"
            End If
            Dim key As String = "articles_articles_" & publishedOnly.ToString & "_" & sortExp.Replace(" ", "") & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
            If Cache(key) IsNot Nothing Then
               Dim li As List(Of mcc_Article) = DirectCast(Cache(key), List(Of mcc_Article))
               Return li
            Else
               Dim mdc As New MCCDataContext()

               Dim li As New List(Of mcc_Article)
               li = (From a As mcc_Article In mdc.mcc_Articles Where a.Approved = True AndAlso a.ReleaseDate <= DateTime.Now _
                                       AndAlso a.ExpireDate > DateTime.Now).SortBy(sortExp).Skip(startrowindex).Take(maximumrows).ToList
               CacheData(key, li)
               Return li
            End If
         Else
            Return GetArticles(startrowindex, maximumrows, sortExp)
         End If
      End Function


      Public Function GetFrontPageArticles(ByVal publishedOnly As Boolean, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "releasedate DESC") As List(Of frontPageArticle) Implements IArticleRepository.GetFrontPageArticles
         If publishedOnly Then
            If String.IsNullOrEmpty(sortExp) Then
               sortExp = "ReleaseDate DESC"
            End If
            Dim key As String = "articles_articles_frontpage_" & publishedOnly.ToString & "_" & sortExp.Replace(" ", "") & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
            If Cache(key) IsNot Nothing Then
               Dim li As List(Of frontPageArticle) = DirectCast(Cache(key), List(Of frontPageArticle))
               Return li
            Else
               Dim mdc As New MCCDataContext()

               Dim li As New List(Of frontPageArticle)
               Dim h = (From k As mcc_ArticleCategory In mdc.mcc_ArticleCategories Where k.CategoryId = 1 Select k.ArticleId)

               li = (From a As mcc_Article In mdc.mcc_Articles _
                           Group Join im In mdc.mcc_Images On a.ImageID Equals im.ImageID Into arts = Group _
               Where a.Approved = True AndAlso a.ReleaseDate <= DateTime.Now _
                                    AndAlso a.ExpireDate > DateTime.Now AndAlso Not h.Contains(a.ArticleID) _
               From ar In arts.DefaultIfEmpty() _
               Select New frontPageArticle With { _
                                              .ArticleID = a.ArticleID, _
                                              .Title = a.Title, _
                                              .Abstract = a.Abstract, _
                                              .Slug = a.Slug, _
                                              .ReleaseDate = a.ReleaseDate, _
                                              .AddedDate = a.AddedDate, _
                                              .AddedBy = a.AddedBy, _
                                              .ImageID = a.ImageID, _
                                              .ImageNewsUrl = a.ImageNewsUrl, _
                                              .ImageCreditsName = ar.CreditsName, _
                                              .ImageCreditsUrl = ar.CreditsUrl}).SortBy(sortExp).Skip(startrowindex).Take(maximumrows).ToList

               CacheData(key, li)
               Return li
            End If
         Else
            Return Nothing
         End If
      End Function

      Public Function GetSpotlightArticles(ByVal publishedOnly As Boolean, ByVal categoryName As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "releasedate DESC") As List(Of spotlightArticle) Implements IArticleRepository.GetSpotlightArticles
         If publishedOnly Then
            If String.IsNullOrEmpty(sortExp) Then
               sortExp = "ReleaseDate DESC"
            End If
            Dim key As String = "articles_articles_spotlight_" & publishedOnly.ToString & "_" & categoryName & "_" & sortExp.Replace(" ", "") & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
            If Cache(key) IsNot Nothing Then
               Dim li As List(Of spotlightArticle) = DirectCast(Cache(key), List(Of spotlightArticle))
               Return li
            Else
               Dim mdc As New MCCDataContext()
               Dim li As New List(Of spotlightArticle)
               Dim catId As Integer = (From it As mcc_Category In mdc.mcc_Categories _
                         Where it.Slug.ToLower = categoryName Select it.CategoryID).Single()


               Dim h = (From k As mcc_ArticleCategory In mdc.mcc_ArticleCategories Where k.CategoryId = catId Select k.ArticleId)

               li = (From a As mcc_Article In mdc.mcc_Articles Where a.Approved = True AndAlso a.ReleaseDate <= DateTime.Now _
                                    AndAlso a.ExpireDate > DateTime.Now AndAlso h.Contains(a.ArticleID) _
               Select New spotlightArticle With { _
                                              .ArticleID = a.ArticleID, _
                                              .Title = a.Title, _
                                              .Abstract = a.Abstract, _
                                              .Slug = a.Slug, _
                                              .ReleaseDate = a.ReleaseDate, _
                                              .AddedDate = a.AddedDate, _
                                              .ImageNewsUrl = a.ImageNewsUrl, _
                                              .ImageIconUrl = a.ImageIconUrl}).SortBy(sortExp).Skip(startrowindex).Take(maximumrows).ToList


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
      Public Function GetLatestArticles(ByVal pageSize As Integer) As List(Of mcc_Article) Implements IArticleRepository.GetLatestArticles

         Dim Articles As New List(Of mcc_Article)
         Dim key As String = "Articles_Articles_Latest_" + pageSize.ToString

         If mccObject.Cache(key) IsNot Nothing Then
            Articles = DirectCast(mccObject.Cache(key), List(Of mcc_Article))
            Articles.Sort(New ArticleComparer("ReleaseDate DESC"))
         Else
            Articles = GetArticles(True, 0, pageSize, "ReleaseDate DESC")
            CacheData(key, Articles)
         End If
         Return Articles
      End Function

      Public Function GetArticleById(ByVal ArticleId As Integer) As mcc_Article Implements IArticleRepository.GetArticleById
         Dim key As String = "articles_articles_" & ArticleId.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Dim fb As mcc_Article = DirectCast(Cache(key), mcc_Article)
            Return fb
         Else
            Dim mdc As New MCCDataContext()
            Dim fb As mcc_Article = (From it As mcc_Article In mdc.mcc_Articles Where it.ArticleID = ArticleId).FirstOrDefault
            CacheData(key, fb)
            Return fb
         End If
      End Function

      Public Function GetArticleBySlug(ByVal slug As String) As mcc_Article Implements IArticleRepository.GetArticleBySlug
         Dim key As String = "articles_articles_" & slug & "_"
         If Cache(key) IsNot Nothing Then
            Dim fb As mcc_Article = DirectCast(Cache(key), mcc_Article)
            Return fb
         Else
            Dim mdc As New MCCDataContext()
            Dim fb As mcc_Article = (From it As mcc_Article In mdc.mcc_Articles Where it.Slug = slug).FirstOrDefault

            CacheData(key, fb)
            Return fb
         End If
      End Function

      ''' <summary>
      ''' Creates a new Article
      ''' </summary>
      Public Sub InsertArticle(ByVal title As String, ByVal Abstract As String, ByVal body As String, ByVal country As String, ByVal state As String, _
       ByVal city As String, ByVal releaseDate As DateTime, ByVal expireDate As DateTime, ByVal approved As Boolean, ByVal listed As Boolean, ByVal commentsEnabled As Boolean, _
       ByVal onlyForMembers As Boolean, ByVal ImageNewsUrl As String, ByVal ImageIconUrl As String, ByVal tags As String, ByVal videoId As Integer, ByVal pollId As Integer, _
       ByVal status As Integer, ByVal imageId As Integer) Implements IArticleRepository.InsertArticle


         'Dim canApprove As Boolean = (mccObject.CurrentUser.IsInRole("Administrators") OrElse mccObject.CurrentUser.IsInRole("Editors"))
         'If Not canApprove Then
         '   approved = False
         'End If

         ' on an inserted article the approved flagged will always be false.
         approved = False

         Dim mdc As MCCDataContext = New MCCDataContext
         Dim wrd As New mcc_Article

         title = mccObject.ConvertNullToEmptyString(title)
         Abstract = mccObject.ConvertNullToEmptyString(Abstract)
         body = mccObject.ConvertNullToEmptyString(body)
         country = mccObject.ConvertNullToEmptyString(country)
         state = mccObject.ConvertNullToEmptyString(state)
         city = mccObject.ConvertNullToEmptyString(city)
         ImageNewsUrl = mccObject.ConvertNullToEmptyString(ImageNewsUrl)
         ImageIconUrl = mccObject.ConvertNullToEmptyString(ImageIconUrl)
         tags = mccObject.ConvertNullToEmptyString(tags)

         If releaseDate = DateTime.MinValue Then
            releaseDate = DateTime.Now
         End If
         If expireDate = DateTime.MinValue Then
            expireDate = DateTime.MaxValue
         End If

         With wrd
            .AddedDate = DateTime.Now
            .AddedBy = mccObject.CurrentUserName
            .Title = title
            .Abstract = Abstract
            .Body = body
            .Country = country
            .State = state
            .City = city
            .ImageNewsUrl = ImageNewsUrl
            .ImageIconUrl = ImageIconUrl
            .OnlyForMembers = onlyForMembers
            .Approved = approved
            .Listed = listed
            .CommentsEnabled = commentsEnabled
            .Tags = tags
            .ReleaseDate = releaseDate
            .ExpireDate = expireDate
            .Status = status
            .VideoId = videoId
            .PollId = pollId
            .Slug = routines.GetSlugFromString(title)
            .ImageID = imageId
         End With


         mdc.mcc_Articles.InsertOnSubmit(wrd)
         mdc.SubmitChanges()
         Dim id As Integer = wrd.ArticleID

         If Not String.IsNullOrEmpty(tags) AndAlso tags.Split(",").Count > 0 Then
            Articles.ArticleTagRepository.InsertArticleTags(id, tags.Split(",").ToList())
         End If

         mccObject.PurgeCacheItems("Articles_Articles_")
         mccObject.PurgeCacheItems("Articles_Articles_Latest_")
         mccObject.PurgeCacheItems("Articles_Article")

         mccObject.PurgeCacheItems("farticles_farticles")
         mccObject.PurgeCacheItems("farticles_farticlecount_")

      End Sub


      Public Sub UpdateArticle(ByVal articleId As Integer, ByVal title As String, ByVal Abstract As String, ByVal body As String, ByVal country As String, _
 ByVal state As String, ByVal city As String, ByVal releaseDate As DateTime, ByVal expireDate As DateTime, ByVal approved As Boolean, ByVal listed As Boolean, _
 ByVal commentsEnabled As Boolean, ByVal onlyForMembers As Boolean, ByVal ImageNewsUrl As String, ByVal ImageIconUrl As String, ByVal tags As String, _
            ByVal videoId As Integer, ByVal pollId As Integer, ByVal status As Integer, ByVal imageId As Integer) Implements IArticleRepository.UpdateArticle

         Dim mdc As MCCDataContext = New MCCDataContext
         Dim wrd As mcc_Article = (From t In mdc.mcc_Articles _
                                     Where t.ArticleID = articleId _
                                     Select t).Single()
         If wrd IsNot Nothing Then

            title = mccObject.ConvertNullToEmptyString(title)
            Abstract = mccObject.ConvertNullToEmptyString(Abstract)
            body = mccObject.ConvertNullToEmptyString(body)
            country = mccObject.ConvertNullToEmptyString(country)
            state = mccObject.ConvertNullToEmptyString(state)
            city = mccObject.ConvertNullToEmptyString(city)
            ImageNewsUrl = mccObject.ConvertNullToEmptyString(ImageNewsUrl)
            ImageIconUrl = mccObject.ConvertNullToEmptyString(ImageIconUrl)
            tags = mccObject.ConvertNullToEmptyString(tags)

            If releaseDate = DateTime.MinValue Then
               releaseDate = DateTime.Now
            End If
            If expireDate = DateTime.MinValue Then
               expireDate = DateTime.MaxValue
            End If

            With wrd
               .Title = title
               .Abstract = Abstract
               .Body = body
               .Country = country
               .State = state
               .City = city
               .ImageNewsUrl = ImageNewsUrl
               .ImageIconUrl = ImageIconUrl
               .OnlyForMembers = onlyForMembers
               .Approved = approved
               .Listed = listed
               .CommentsEnabled = commentsEnabled
               .Tags = tags
               .ReleaseDate = releaseDate
               .ExpireDate = expireDate
               .Status = status
               .VideoId = videoId
               .PollId = pollId
               ' avoid updating the slug for update - it will break links ...
               .Slug = routines.GetSlugFromString(title)
               .ImageID = imageId
            End With

            mdc.SubmitChanges()

            If Not String.IsNullOrEmpty(tags) AndAlso tags.Split(",").Count > 0 Then
               Articles.ArticleTagRepository.InsertArticleTags(articleId, tags.Split(",").ToList())
            End If

            mccObject.PurgeCacheItems("Articles_ArticleCount")
            mccObject.PurgeCacheItems("Articles_Article_" + articleId.ToString())
            mccObject.PurgeCacheItems("Articles_Articles_Latest_")
            mccObject.PurgeCacheItems("Articles_Articles")
            mccObject.PurgeCacheItems("Articles_Specified_Categories_" + articleId.ToString)

            mccObject.PurgeCacheItems("farticles_farticles")

         End If
      End Sub

      Public Sub DeleteArticles(ByVal ArticleId() As Integer) Implements IArticleRepository.DeleteArticles
         Dim mdc As MCCDataContext = New MCCDataContext
         If mdc.mcc_Articles.Count(Function(p) ArticleId.Contains(p.ArticleID)) > 0 Then
            Dim wrd As List(Of mcc_Article) = (From t In mdc.mcc_Articles _
                                        Where ArticleId.Contains(t.ArticleID)).ToList
            If wrd IsNot Nothing AndAlso wrd.Count > 0 Then


               mdc.mcc_Articles.DeleteAllOnSubmit(wrd)
               mdc.SubmitChanges()
               mccObject.PurgeCacheItems("articles_articles_")

               For Each it As mcc_Article In wrd
                  RemoveArticleFromCategory(it.ArticleID)
                  Dim tb As New MCCEvents.MCCEvents.RecordDeletedEvent("Article", it.ArticleID, Nothing)
                  tb.Raise()
               Next


               mccObject.PurgeCacheItems("Articles_Article")
               mccObject.PurgeCacheItems("Articles_Articles_")
               mccObject.PurgeCacheItems("Articles_Articles_Latest_")
               mccObject.PurgeCacheItems("Articles_Specified_Categories_" + ArticleId.ToString)

               mccObject.PurgeCacheItems("farticles_farticles")
               mccObject.PurgeCacheItems("farticles_farticlecount_")
            End If
         End If
      End Sub

      Public Sub DeleteArticle(ByVal ArticleId As Integer) Implements IArticleRepository.DeleteArticle
         Dim mdc As MCCDataContext = New MCCDataContext
         If mdc.mcc_Articles.Count(Function(p) p.ArticleID = ArticleId) > 0 Then
            Dim wrd As mcc_Article = (From t In mdc.mcc_Articles _
                                        Where t.ArticleID = ArticleId _
                                        Select t).Single()
            If wrd IsNot Nothing Then
               mdc.mcc_Articles.DeleteOnSubmit(wrd)
               mdc.SubmitChanges()
               mccObject.PurgeCacheItems("articles_articles_")

               RemoveArticleFromCategory(ArticleId)
               Dim tb As New MCCEvents.MCCEvents.RecordDeletedEvent("Article", ArticleId, Nothing)
               tb.Raise()

               mccObject.PurgeCacheItems("Articles_Article")
               mccObject.PurgeCacheItems("Articles_Articles_")
               mccObject.PurgeCacheItems("Articles_Articles_Latest_")
               mccObject.PurgeCacheItems("Articles_Specified_Categories_" + ArticleId.ToString)

               mccObject.PurgeCacheItems("farticles_farticles")
               mccObject.PurgeCacheItems("farticles_farticlecount_")
            End If
         End If
      End Sub

      Public Sub ApproveArticle(ByVal articleId As Integer) Implements IArticleRepository.ApproveArticle
         Dim mdc As MCCDataContext = New MCCDataContext
         If mdc.mcc_Articles.Count(Function(p) p.ArticleID = articleId) > 0 Then
            Dim wrd As mcc_Article = (From t In mdc.mcc_Articles _
                                        Where t.ArticleID = articleId _
                                        Select t).Single()
            wrd.Approved = True
            mdc.SubmitChanges()
            mccObject.PurgeCacheItems("Articles_ArticleCount")
            mccObject.PurgeCacheItems("Articles_Article_" + articleId.ToString())
            mccObject.PurgeCacheItems("Articles_Articles")
            mccObject.PurgeCacheItems("Articles_Articles_Latest_")

            mccObject.PurgeCacheItems("farticles_farticles")
            mccObject.PurgeCacheItems("farticles_farticlecount_")
         End If
      End Sub

      Public Function RemoveArticleFromCategory(ByVal ArticleID As Integer, Optional ByVal categoryId As Integer = 0) As Boolean Implements IArticleRepository.RemoveArticleFromCategory
         'Dim ret As Boolean = SiteProvider.Articles.RemoveArticleFromCategory(ArticleID, categoryId)
         Dim ret As Boolean = True

         Dim mdc As MCCDataContext = New MCCDataContext
         If mdc.mcc_Articles.Count(Function(p) p.ArticleID = ArticleID) > 0 AndAlso mdc.mcc_ArticleCategories.Count(Function(p) p.ArticleId = ArticleID) > 0 Then

            Dim wrd As New List(Of mcc_ArticleCategory)

            If categoryId = 0 Then
               wrd = (From t In mdc.mcc_ArticleCategories _
                                                   Where t.ArticleId = ArticleID).ToList
            Else
               wrd = (From t In mdc.mcc_ArticleCategories _
                                                      Where t.ArticleId = ArticleID AndAlso t.CategoryId = categoryId Select t).ToList
            End If

            For Each it As mcc_ArticleCategory In wrd
               mdc.mcc_ArticleCategories.DeleteOnSubmit(it)
            Next

            mdc.SubmitChanges()

            mccObject.PurgeCacheItems("Articles_Article")
            mccObject.PurgeCacheItems("Articles_ArticleInCategories_")
            mccObject.PurgeCacheItems("Articles_Specified_Categories_" + ArticleID.ToString)
         End If
         Return ret
      End Function

      Public Function AddArticleToCategory(ByVal ArticleId As Integer, ByVal CategoryId As Integer) As Integer Implements IArticleRepository.AddArticleToCategory
         'Dim ret As Integer = SiteProvider.Articles.AddArticleToCategory(ArticleId, CategoryId)
         Dim mdc As MCCDataContext = New MCCDataContext
         If mdc.mcc_ArticleCategories.Count(Function(p) p.ArticleId = ArticleId AndAlso p.CategoryId = CategoryId) = 0 Then
            Dim cid As New mcc_ArticleCategory
            cid.ArticleId = ArticleId
            cid.CategoryId = CategoryId

            mdc.mcc_ArticleCategories.InsertOnSubmit(cid)
            mdc.SubmitChanges()
            mccObject.PurgeCacheItems("Articles_ArticleInCategories_")
         End If
      End Function

      Public Function AddArticleToCategories(ByVal ArticleId As Integer, ByVal CategoryId() As Integer) As Integer Implements IArticleRepository.AddArticleToCategories
         'Dim ret As Integer = SiteProvider.Articles.AddArticleToCategory(ArticleId, CategoryId)
         Dim mdc As MCCDataContext = New MCCDataContext

         Dim process As Boolean = False
         Dim catList As List(Of mcc_ArticleCategory) = (From it As mcc_ArticleCategory In mdc.mcc_ArticleCategories Where it.ArticleId = ArticleId AndAlso CategoryId.Contains(it.CategoryId)).ToList
         If catList.Count > 0 Then
            mdc.mcc_ArticleCategories.DeleteAllOnSubmit(catList)
            mdc.SubmitChanges()
            process = True
         End If

         For Each c As Integer In CategoryId
            Dim id As Integer = c
            If mdc.mcc_ArticleCategories.Count(Function(p) p.ArticleId = ArticleId AndAlso p.CategoryId = id) = 0 Then
               Dim cid As New mcc_ArticleCategory
               cid.ArticleId = ArticleId
               cid.CategoryId = c
               mdc.mcc_ArticleCategories.InsertOnSubmit(cid)
               process = True
            End If
         Next

         If process Then
            mdc.SubmitChanges()
            'mccObject.PurgeCacheItems("Articles_ArticleInCategories_")
            mccObject.PurgeCacheItems("Articles_")
         End If

      End Function


      Public Sub IncrementViewCount(ByVal articleId As Integer) Implements IArticleRepository.IncrementViewCount
         Dim mdc As MCCDataContext = New MCCDataContext
         Dim wrd As mcc_Article = (From t In mdc.mcc_Articles _
                                        Where t.ArticleID = articleId _
                                        Select t).FirstOrDefault
         If wrd IsNot Nothing Then
            wrd.ViewCount += 1
            mdc.SubmitChanges()
            'mccObject.PurgeCacheItems("articles_articles_")
            'mccObject.PurgeCacheItems("articles_articleCount_")
         End If
      End Sub

      Public Sub RateArticle(ByVal articleId As Integer, ByVal rating As Integer) Implements IArticleRepository.RateArticle
         Dim mdc As MCCDataContext = New MCCDataContext
         Dim wrd As mcc_Article = (From t In mdc.mcc_Articles _
                                        Where t.ArticleID = articleId _
                                        Select t).FirstOrDefault

         If wrd IsNot Nothing Then
            wrd.Votes += 1
            wrd.TotalRating += rating

            mdc.SubmitChanges()
            mccObject.PurgeCacheItems("articles_articles_")
            mccObject.PurgeCacheItems("articles_articleCount_")
         End If
      End Sub

      Public Function FindArticles(ByVal where As mccEnum.SearchType, ByVal SearchWord As String) As List(Of mcc_Article) Implements IArticleRepository.FindArticles
         Return FindArticles(where, 0, mccObject.MaxRows, SearchWord)
      End Function

      Public Function FindArticles(ByVal where As mccEnum.SearchType, ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal SearchWord As String) As List(Of mcc_Article) Implements IArticleRepository.FindArticles
         Dim li As New List(Of mcc_Article)
         Dim mdc As New MCCDataContext
         Select Case where
            Case mccEnum.SearchType.AnyWord
               Dim q = mcc_Article.ContainsAny(SearchWord.Split().ToArray)
               li = (From it As mcc_Article In mdc.mcc_Articles.Where(q)).Distinct().ToList()
            Case mccEnum.SearchType.AllWords

            Case mccEnum.SearchType.ExactPhrase
               li = (From it As mcc_Article In mdc.mcc_Articles Where it.Tags.Contains(SearchWord) Or it.Title.Contains(SearchWord) Or it.Abstract.Contains(SearchWord) _
                      Or it.Body.Contains(SearchWord)).ToList
         End Select

         Return li
      End Function

      '''' <summary>
      '''' Returns a collection with all Articles for the specified category
      '''' </summary>
      'Public  Function FindArticles(ByVal categoryID As Integer, ByVal SearchWord As String) As List(Of mcc_Article)
      '   Return FindArticles(categoryID, 0, mccObject.MaxRows, SearchWord)
      'End Function

      'Public  Function FindArticles(ByVal categoryID As Integer, ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal SearchWord As String) As List(Of mcc_Article)
      '   If categoryID <= 0 Then
      '      Return FindArticles(startRowIndex, maximumRows, SearchWord)
      '   End If

      '   'Dim Articles As List(Of Article) = Nothing

      '   'Dim recordset As List(Of ArticleDetails) = SiteProvider.Articles.FindArticles(categoryID, GetPageIndex(startRowIndex, maximumRows), maximumRows, SearchWord)
      '   'Articles = GetArticleListFromArticleDetailsList(recordset)

      '   Return Nothing
      'End Function

      ''' <summary>
      ''' Returns the number of total articles matching the search key
      ''' </summary>
      Public Function FindArticlesCount(ByVal where As mccEnum.SearchType, ByVal searchWord As String) As Integer Implements IArticleRepository.FindArticlesCount
         Dim artCount As Integer = 0
         Dim mdc As New MCCDataContext()

         Select Case where
            Case mccEnum.SearchType.AnyWord
               Dim q = mcc_Article.ContainsAny(searchWord.Split().ToArray)
               artCount = (From it As mcc_Article In mdc.mcc_Articles.Where(q)).Count()
            Case mccEnum.SearchType.ExactPhrase
               artCount = mdc.mcc_Articles.Count(Function(p) p.Tags.Contains(searchWord) Or p.Abstract.Contains(searchWord) _
                                                          Or p.Title.Contains(searchWord) Or p.Body.Contains(searchWord))
         End Select

         Return artCount
      End Function

      'Public  Function GetArticlesCountByStatus(ByVal status As mccEnum.Status) As Integer

      '   Dim key As String = "farticles_farticlecount_" & "st_" & status.ToString
      '   If Cache(key) IsNot Nothing Then
      '      Return CInt(Cache(key))
      '   Else
      '      Dim mdc As New MCCDataContext
      '      If status < 0 Or status > 5 Then
      '         Return 0
      '      Else
      '         Return mdc.mcc_Articles.Count(Function(p) p.Status = CInt(status))
      '      End If
      '   End If
      'End Function

      Public Function FetchArticlesCount(ByVal status As mccEnum.Status) As Integer Implements IArticleRepository.FetchArticlesCount

         Dim key As String = "farticles_farticlecount_" & "st_" & status.ToString
         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            Dim mdc As New MCCDataContext
            If status < 0 Or status > 5 Then
               Return 0
            Else
               Return mdc.mcc_Articles.Count(Function(p) p.Status = CInt(status))
            End If
         End If
      End Function

      Public Function FetchArticlesCount(ByVal status As Integer) As Integer Implements IArticleRepository.FetchArticlesCount

         Dim key As String = "farticles_farticlecount_" & "st_" & status.ToString
         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            Dim mdc As New MCCDataContext
            If status < 0 Then
               Return mdc.mcc_Articles.Count()
            Else
               Return mdc.mcc_Articles.Count(Function(p) p.Status = status)
            End If
         End If
      End Function

      Public Function FetchArticlesCount(ByVal categoryID As Integer, ByVal status As Integer) As Integer Implements IArticleRepository.FetchArticlesCount

         Dim key As String = "farticle_farticlecount_" & categoryID.ToString & "_" & "st_" & status.ToString

         If (Cache(key) IsNot Nothing) Then
            Return CInt(Cache(key))
         Else
            Dim mdc As New MCCDataContext

            If categoryID <= 0 Then
               Return FetchArticlesCount(status)
            End If
            Dim catAr As List(Of Integer) = (From ef As mcc_ArticleCategory In mdc.mcc_ArticleCategories Where ef.CategoryId = categoryID Select ef.ArticleId).ToList
            Dim cnt As Integer = mdc.mcc_Articles.Count(Function(p) p.Status = status And catAr.Contains(p.ArticleID))
            CacheData(key, cnt)
            Return cnt
         End If
      End Function

      Public Function FetchArticles(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, Optional ByVal sortExp As String = "Title") As List(Of mcc_Article) Implements IArticleRepository.FetchArticles

         Dim li As New List(Of mcc_Article)
         Dim key As String = "farticles_farticles_" & "_" & startRowIndex.ToString & "_" & maximumRows.ToString & "_" & sortExp

         If (Cache(key) IsNot Nothing) Then
            li = DirectCast(Cache(key), List(Of mcc_Article))
         Else
            Dim mdc As New MCCDataContext
            Dim i As Integer = mdc.mcc_Articles.Count()
            If i > 0 Then
               If startRowIndex > 0 AndAlso maximumRows > 0 Then
                  li = (From it As mcc_Article In mdc.mcc_Articles).SortBy(sortExp).Skip(startRowIndex).Take(maximumRows).ToList
               Else
                  li = (From it As mcc_Article In mdc.mcc_Articles).SortBy(sortExp).Skip(0).Take(30).ToList
                  Dim ic = (From it As mcc_Article In mdc.mcc_Articles).SortBy(sortExp).Skip(0).Take(30)
               End If
               CacheData(key, li)
            End If
         End If
         Return li
      End Function

      Public Function FetchArticles(ByVal status As Integer, ByVal startRowIndex As Integer, ByVal maximumRows As Integer, Optional ByVal sortExp As String = "Title") As List(Of mcc_Article) Implements IArticleRepository.FetchArticles
         If status < 0 Then
            Return FetchArticles(startRowIndex, maximumRows, sortExp)
         End If
         Dim li As New List(Of mcc_Article)
         Dim key As String = "farticles_farticles_" & "st_" & status.ToString & "_" & startRowIndex.ToString & "_" & maximumRows.ToString & "_" & sortExp

         If (Cache(key) IsNot Nothing) Then
            li = DirectCast(Cache(key), List(Of mcc_Article))
         Else
            Dim mdc As New MCCDataContext

            If startRowIndex > 0 AndAlso maximumRows > 0 Then
               li = (From it As mcc_Article In mdc.mcc_Articles Where it.Status = status).SortBy(sortExp).Skip(startRowIndex).Take(maximumRows).ToList
            Else
               li = (From it As mcc_Article In mdc.mcc_Articles Where it.Status = status).SortBy(sortExp).Skip(0).Take(30).ToList
            End If
            CacheData(key, li)
         End If

         Return li
      End Function

      Public Function FetchArticles(ByVal categoryID As Integer, ByVal status As Integer, ByVal startRowIndex As Integer, ByVal maximumRows As Integer, Optional ByVal sortExp As String = "AddedDate") As List(Of mcc_Article) Implements IArticleRepository.FetchArticles

         If categoryID <= 0 Then
            Return FetchArticles(status, startRowIndex, maximumRows, sortExp)
         End If

         Dim key As String = "farticles_farticles_" & categoryID.ToString & "_" & "st_" & status.ToString & "_" & startRowIndex.ToString & "_" & maximumRows.ToString & "_" & sortExp
         Dim li As New List(Of mcc_Article)

         If (Cache(key) IsNot Nothing) Then
            li = DirectCast(Cache(key), List(Of mcc_Article))
         Else
            Dim mdc As New MCCDataContext
            Dim catAr As List(Of Integer) = (From ef As mcc_ArticleCategory In mdc.mcc_ArticleCategories Where ef.CategoryId = categoryID Select ef.ArticleId).ToList

            If startRowIndex > 0 AndAlso maximumRows > 0 Then
               li = (From it As mcc_Article In mdc.mcc_Articles Where catAr.Contains(it.ArticleID) And it.Status = status).SortBy(sortExp).Skip(startRowIndex).Take(maximumRows).ToList
            Else
               li = (From it As mcc_Article In mdc.mcc_Articles Where catAr.Contains(it.ArticleID) And it.Status = status).SortBy(sortExp).Skip(0).Take(30).ToList
            End If
            CacheData(key, li)
         End If
         Return li
      End Function


      Public Sub UpdateArticleStatus(ByVal id As Integer) Implements IArticleRepository.UpdateArticleStatus
         Dim mdc As New MCCDataContext()
         If mdc.mcc_Articles.Count(Function(p) p.ArticleID = id) > 0 Then
            Dim it As mcc_Article = (From ars As mcc_Article In mdc.mcc_Articles Where ars.ArticleID = id).FirstOrDefault
            If it IsNot Nothing AndAlso it.Status < 5 Then
               it.Status += 1
               mdc.SubmitChanges()
               mccObject.PurgeCacheItems("articles_")
               mccObject.PurgeCacheItems("farticles_")
            End If
         End If
      End Sub

      Public Sub UpdateArticleStatus(ByVal id As Integer, ByVal st As mccEnum.Status) Implements IArticleRepository.UpdateArticleStatus
         Dim mdc As New MCCDataContext()
         Dim it As mcc_Article = (From ars As mcc_Article In mdc.mcc_Articles Where ars.ArticleID = id).FirstOrDefault
         If it IsNot Nothing Then
            If st <= 5 AndAlso st >= 0 Then
               it.Status = CInt(st)

               If st <> mccEnum.Status.Approved Then
                  it.Approved = False
               Else
                  it.Approved = True
               End If

               mdc.SubmitChanges()
               mccObject.PurgeCacheItems("articles_")
               mccObject.PurgeCacheItems("farticles_")
            End If
         End If
      End Sub

      Public Function GetArticleStatusValue(ByVal id As Integer) As Integer Implements IArticleRepository.GetArticleStatusValue
         Dim key As String = "articles_article_statusval_" & id.ToString
         If Cache(key) IsNot Nothing Then
            Return Cache(key)
         Else
            Dim val As Integer = -1
            Dim mdc As New MCCDataContext()
            val = (From it As mcc_Article In mdc.mcc_Articles Where it.ArticleID = id Select it.Status).FirstOrDefault
            CacheData(key, val)
            Return val
         End If
      End Function

      Public Function GetArticleStatus(ByVal articleId As Integer) As mccEnum.Status Implements IArticleRepository.GetArticleStatus
         Dim key As String = "articles_article_status_" & articleId.ToString
         If Cache(key) Then
            Return Cache(key)
         Else
            Dim stat As mccEnum.Status = mccEnum.Status.Pending
            Dim mdc As New MCCDataContext()
            Dim ival As Integer = (From it As mcc_Article In mdc.mcc_Articles Where it.ArticleID = articleId Select it.Status).FirstOrDefault
            stat = (CType(ival, mccEnum.Status))
            CacheData(key, stat)
            Return stat
         End If
      End Function

      Public Function FixArticlesVideoIds() As Boolean Implements IArticleRepository.FixArticlesVideoIds
         Dim mdc As New MCCDataContext()
         Dim li As List(Of mcc_Article) = (From it As mcc_Article In mdc.mcc_Articles Where it.VideoId Is Nothing).ToList()
         For Each it As mcc_Article In li
            it.VideoId = 0
         Next
         mdc.SubmitChanges()
         mccObject.PurgeCacheItems("articles_")
         mccObject.PurgeCacheItems("farticles_")
         Return True
      End Function

      Public Function GetReadersPick() As mcc_Article Implements IArticleRepository.GetReadersPick
         Dim ar As mcc_Article = Nothing
         Dim key As String = "articles_articles_readerpick_"

         If Cache(key) IsNot Nothing Then
            Return CType(Cache(key), mcc_Article)
         Else
            Dim mdc As New MCCDataContext()
            ar = (From ac As mcc_Article In mdc.mcc_Articles).SortBy("viewcount DESC").FirstOrDefault()
            CacheData(key, ar)
         End If
         Return ar
      End Function

      Protected Sub CacheData(ByVal key As String, ByVal data As Object)
         If data IsNot Nothing Then
            mccObject.Cache.Insert(key, data, Nothing, DateTime.Now.AddSeconds(3600), TimeSpan.Zero)
         End If
      End Sub
   End Class

   ''' <summary>
   ''' 
   ''' </summary>
   ''' <remarks></remarks>
   Public Class ArticleComparer
      Implements IComparer(Of mcc_Article)
      Private _sortBy As String
      Private _reverse As Boolean

      Public Sub New(ByVal sortBy As String)
         If Not String.IsNullOrEmpty(sortBy) Then
            sortBy = sortBy.ToLower()
            _reverse = sortBy.EndsWith(" desc")
            _sortBy = sortBy.Replace(" desc", "").Replace(" asc", "")
         End If
      End Sub

      Public Overloads Function Equals(ByVal x As mcc_Article, ByVal y As mcc_Article) As Boolean
         Return (x.ArticleID = y.ArticleID)
      End Function

      Public Function Compare(ByVal x As mcc_Article, ByVal y As mcc_Article) As Integer Implements IComparer(Of mcc_Article).Compare
         Dim ret As Integer = 0
         Select Case _sortBy
            Case "addeddate"
               ret = DateTime.Compare(x.AddedDate, y.AddedDate)
               Exit Select
            Case "addedby"
               ret = String.Compare(x.AddedBy, y.AddedBy, True)
            Case "title"
               ret = String.Compare(x.Title, y.Title, True)
            Case "releasedate"
               ret = DateTime.Compare(x.ReleaseDate, y.ReleaseDate)
            Case "expiredate"
               ret = DateTime.Compare(x.ExpireDate, y.ExpireDate)
            Case "status"
               ret = IIf(x.Status > y.Status, -1, 1)
               Exit Select
         End Select
         Return (ret * (IIf(_reverse, -1, 1)))
      End Function
   End Class


   Public Class frontPageArticle   ' front page article
      Public Sub New()

      End Sub

      Public Sub New(ByVal id As Integer, ByVal title As String, ByVal abstract As String, ByVal slug As String, _
               ByVal releaseDate As Date, ByVal addedDate As Date, ByVal addedBy As String, ByVal imageNewsUrl As String, _
               ByVal imageCreditsName As String, ByVal imageCreditsUrl As String)

         Me.ArticleID = id
         Me.Title = title
         Me.Abstract = abstract
         Me.Slug = slug
         Me.AddedDate = addedDate
         Me.AddedBy = addedBy
         Me.ReleaseDate = releaseDate
         Me.ImageNewsUrl = imageNewsUrl
         Me.ImageCreditsName = imageCreditsName
         Me.ImageCreditsUrl = imageCreditsUrl
      End Sub


      Private _articleId As String
      Public Property ArticleID() As String
         Get
            Return _articleId
         End Get
         Set(ByVal value As String)
            _articleId = value
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


      Private _abstract As String
      Public Property Abstract() As String
         Get
            Return _abstract
         End Get
         Set(ByVal value As String)
            If value.Length > 200 Then
               _abstract = value.Substring(0, 200) & "..."
            Else
               _abstract = value
            End If
         End Set
      End Property


      Private _addedDate As Date
      Public Property AddedDate() As Date
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

      Private _releaseDate As Date
      Public Property ReleaseDate() As Date
         Get
            Return _releaseDate
         End Get
         Set(ByVal value As Date)
            _releaseDate = value
         End Set
      End Property

      Private _imageNewsUrl As String
      Public Property ImageNewsUrl() As String
         Get
            Return _imageNewsUrl
         End Get
         Set(ByVal value As String)
            _imageNewsUrl = value
         End Set
      End Property

      Private _imageCreditUrl As String
      Public Property ImageCreditsUrl() As String
         Get
            Return _imageCreditUrl
         End Get
         Set(ByVal value As String)
            _imageCreditUrl = value
         End Set
      End Property


      Private _imageCreditsName As String
      Public Property ImageCreditsName() As String
         Get
            Return _imageCreditsName
         End Get
         Set(ByVal value As String)
            _imageCreditsName = value
         End Set
      End Property



      Private _imageID? As Integer = 0
      Public Property ImageID() As Integer?
         Get
            Return _imageID
         End Get
         Set(ByVal value? As Integer)
            _imageID = value.GetValueOrDefault(0)
         End Set
      End Property


   End Class

   Public Class spotlightArticle   ' front page spotlight article
      Public Sub New()

      End Sub

      Public Sub New(ByVal id As Integer, ByVal title As String, ByVal abstract As String, ByVal slug As String, _
               ByVal releaseDate As Date, ByVal addedDate As Date, ByVal addedBy As String, ByVal imageIconUrl As String)

         Me.ArticleID = id
         Me.Title = title
         Me.Abstract = abstract
         Me.Slug = slug
         Me.AddedDate = addedDate
         Me.ReleaseDate = releaseDate
         Me.ImageNewsUrl = imageIconUrl
      End Sub


      Private _articleId As String
      Public Property ArticleID() As String
         Get
            Return _articleId
         End Get
         Set(ByVal value As String)
            _articleId = value
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


      Private _abstract As String
      Public Property Abstract() As String
         Get
            Return _abstract
         End Get
         Set(ByVal value As String)
            If value.Length > 230 Then
               _abstract = value.Substring(0, 230) & "..."
            Else
               _abstract = value
            End If
         End Set
      End Property


      Private _addedDate As Date
      Public Property AddedDate() As Date
         Get
            Return _addedDate
         End Get
         Set(ByVal value As DateTime)
            _addedDate = value
         End Set
      End Property

      Private _releaseDate As Date
      Public Property ReleaseDate() As Date
         Get
            Return _releaseDate
         End Get
         Set(ByVal value As Date)
            _releaseDate = value
         End Set
      End Property

      Private _imageNewsUrl As String
      Public Property ImageNewsUrl() As String
         Get
            Return _imageNewsUrl
         End Get
         Set(ByVal value As String)
            _imageNewsUrl = value
         End Set
      End Property

      Private _imageIconUrl As String
      Public Property ImageIconUrl() As String
         Get
            Return _imageIconUrl
         End Get
         Set(ByVal value As String)
            _imageIconUrl = value
         End Set
      End Property

   End Class
End Namespace


Partial Class mcc_Article
   Public Shared Function ContainsAny(ByVal ParamArray keywords As String()) As Expression(Of Func(Of mcc_Article, Boolean))
      Dim predicate = PredicateBuilder.[False](Of mcc_Article)()
      For Each keyword As String In keywords
         Dim temp As String = keyword
         predicate = predicate.[Or](Function(p) p.Tags.Contains(temp))
         predicate = predicate.[Or](Function(p) p.Title.Contains(temp))
         predicate = predicate.[Or](Function(p) p.Body.Contains(temp))
         predicate = predicate.[Or](Function(p) p.Abstract.Contains(temp))
      Next
      Return predicate
   End Function

   'Public Shared Function ContainsAll(ByVal ParamArray keywords As String()) As Expression(Of Func(Of mcc_Article, Boolean))
   '   Dim predicate = PredicateBuilder.[False](Of mcc_Article)()
   '   For Each keyword As String In keywords
   '      Dim temp As String = keyword
   '      predicate = predicate.[And](Function(p) p.Tags.Contains(temp))
   '      predicate = predicate.[And](Function(p) p.Title.Contains(temp))
   '      predicate = predicate.[And](Function(p) p.Body.Contains(temp))
   '      predicate = predicate.[And](Function(p) p.Abstract.Contains(temp))
   '   Next
   '   Return predicate
   'End Function
End Class