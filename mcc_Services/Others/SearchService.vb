Imports MCC.Data
Imports Webdiyer.WebControls.Mvc
Public Class SearchService
   Inherits CacheObject
   Implements ISearchService

   'Private _articleService As IArticleService = New ArticleService()
   'Private _articleCommentSrvr As IArticleCommentService = New ArticleCommentService()
   'Private _videoService As IVideoService = New VideoService()
   'Private _videoCommentService As IVideoCommentService = New VideoCommentService()
   'Private _userquestionservice As IUserQuestionService = New UserQuestionService()
   'Private _useranswerservice As IUserAnswerService = New UserAnswerService()

   Private _searchRepository As ISearchRepository

   Public Sub New()
      Me.New(New SearchRepository())
   End Sub


   Public Sub New(ByVal searchRepo As ISearchRepository)
      _searchRepository = searchRepo
   End Sub

   Public Shared noisewords() As String = {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "$", "a", "about", "after", "all", "also", "an", "and", "another", "any", "are", "as", "at", "b", "be", "because", "been", "before", "being", "between", "both", "but", "by", "c", "came", "can", "come", "could", "d", "did", "do", "does", "e", "each", "else", "f", "for", "from", "g", "get", "got", "h", "had", "has", "have", "he", "her", "here", "him", "himself", "his", "how", "i", "if", "in", "into", "is", "it", "its", "j", "just", "k", "l", "like", "m", "make", "many", "me", "might", "more", "most", "much", "must", "my", "n", "never", "now", "o", "of", "on", "only", "or", "other", "our", "out", "over", "p", "q", "r", "re", "s", "said", "same", "see", "should", "since", "so", "some", "still", "such", "t", "take", "Test", "than", "that", "the", "their", "them", "then", "there", "these", "they", "this", "those", "through", "to", "too", "u", "under", "up", "use", "v", "very", "w", "want", "was", "way", "we", "well", "were", "what", "when", "where", "which", "while", "who", "will", "with", "would", "x", "y", "you", "your", "z"}

   Public Function FilterNoiseWords(ByVal strInput As String) As String
      If Not String.IsNullOrEmpty(strInput) Then
         'strInput = strInput.Trim(noisewords)
         Return strInput
      Else
         Return strInput
      End If
   End Function

   Public Function FindMatches(ByVal searchWord As String, ByVal fwhere As SearchLocation, ByVal searchType As SearchQueryType, ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As PagedList(Of SearchResult) Implements ISearchService.FindMatches

      If String.IsNullOrEmpty(searchWord) Then
         Return Nothing
      End If

      searchWord = FilterNoiseWords(searchWord)

      Dim ls As PagedList(Of SearchResult)

      Dim it As Integer = 0
      'If Integer.TryParse(searchType, it) Then
      '   searchWord = ProcessSearchKeyword(searchWord, CType(it, SearchType))
      'Else
      '   searchWord = ProcessSearchKeyword(searchWord, searchType.AnyWord)
      'End If

      Select Case fwhere
         Case SearchLocation.Articles
            ls = FindArticles(searchWord, searchType, startRowIndex, maximumRows)
         Case SearchLocation.Videos
            ls = FindVideos(searchWord, searchType, startRowIndex, maximumRows)
         Case SearchLocation.QuestionsAndAnswers
            ls = FindUserQuestions(searchWord, searchType, startRowIndex, maximumRows)
            Dim la As PagedList(Of SearchResult) = FindUserAnswers(searchWord, searchType, startRowIndex, maximumRows)
            ls.AddRange(la)
         Case SearchLocation.All
            ls = FindArticles(searchWord, searchType, startRowIndex, maximumRows)
            ls.AddRange(FindAdvices(searchWord, searchType, startRowIndex, maximumRows))
            ls.AddRange(FindVideos(searchWord, searchType, startRowIndex, maximumRows))
            ls.AddRange(FindUserQuestions(searchWord, searchType, startRowIndex, maximumRows))
            ls.AddRange(FindUserAnswers(searchWord, searchType, startRowIndex, maximumRows))
            Return ls
         Case Else
            ls = _searchRepository.FindArticles(searchWord, searchType).ToPagedList(startRowIndex, maximumRows)
      End Select

      Return ls
   End Function

   Public Function FindArticles(ByVal searchWord As String, ByVal searchType As SearchQueryType, ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As PagedList(Of SearchResult) Implements ISearchService.FindArticles
      If String.IsNullOrEmpty(searchWord) Then
         Return Nothing
      End If

      Return _searchRepository.FindArticles(searchWord, searchType).ToPagedList(startRowIndex, maximumRows)
   End Function


   Public Function FindAdvices(ByVal searchWord As String, ByVal searchType As SearchQueryType, ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As PagedList(Of SearchResult) Implements ISearchService.FindAdvices
      If String.IsNullOrEmpty(searchWord) Then
         Return Nothing
      End If

      Return _searchRepository.FindAdvices(searchWord, searchType).ToPagedList(startRowIndex, maximumRows)
   End Function

   Public Function FindUserQuestions(ByVal searchWord As String, ByVal searchType As SearchQueryType, ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As PagedList(Of SearchResult) Implements ISearchService.FindUserQuestions
      If String.IsNullOrEmpty(searchWord) Then
         Return Nothing
      End If

      Return _searchRepository.FindUserQuestions(searchWord, searchType).ToPagedList(startRowIndex, maximumRows)
   End Function

   Public Function FindUserAnswers(ByVal searchWord As String, ByVal searchType As SearchQueryType, ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As PagedList(Of SearchResult) Implements ISearchService.FindUserAnswers
      If String.IsNullOrEmpty(searchWord) Then
         Return Nothing
      End If

      Return _searchRepository.FindUserAnswers(searchWord, searchType).ToPagedList(startRowIndex, maximumRows)
   End Function

   Public Function FindVideos(ByVal searchWord As String, ByVal searchType As SearchQueryType, ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As PagedList(Of SearchResult) Implements ISearchService.FindVideos
      If String.IsNullOrEmpty(searchWord) Then
         Return Nothing
      End If

      Return _searchRepository.FindVideos(searchWord, searchType).ToPagedList(startRowIndex, maximumRows)
   End Function



   'Public Shared Function FindMatches(ByVal searchWord As String, ByVal fwhere As SearchLocation, ByVal fetch As Boolean) As List(Of Search)
   '   If fetch Then
   '      If Not String.IsNullOrEmpty(searchWord) Then
   '         searchWord = FilterNoiseWords(searchWord)
   '         Dim ls As New List(Of Search)
   '         Select Case fwhere
   '            Case SearchLocation.Forums
   '               ls = FindPosts(searchWord)
   '            Case SearchLocation.Articles
   '               ls = FindArticles(searchWord)
   '            Case SearchLocation.Comments
   '               ls = FindArticlesComments(searchWord)
   '            Case SearchLocation.Videos
   '               ls = FindVideos(searchWord)
   '            Case SearchLocation.StaticPages

   '            Case SearchLocation.All
   '               ls = FindAll(searchWord)

   '               Dim key As String = "mcc_search_" + searchWord + "All"
   '               BaseSearch.PurgeCacheItems("mcc_search_")
   '               BaseSearch.CacheData(key, ls)

   '            Case Else
   '               ls = FindArticles(searchWord)
   '         End Select


   '         Return ls
   '      Else
   '         Return New List(Of Search)
   '      End If
   '   Else
   '      searchWord = FilterNoiseWords(searchWord)
   '      Dim key As String = "mcc_search_" + searchWord + "All"
   '      Dim rst As New List(Of Search)
   '      Dim artSearch As New List(Of Search)
   '      If BaseSearch.Settings.EnableCaching AndAlso mccObject.Cache(key) IsNot Nothing Then
   '         artSearch = DirectCast(mccObject.Cache(key), List(Of Search))
   '         rst = (From n In artSearch Where n.ResultType = fwhere).ToList
   '      Else
   '         rst = FindMatches(searchWord, fwhere, True)
   '      End If


   '      Return DirectCast(rst, List(Of Search))
   '   End If
   'End Function


   'Public Function FindArticles(ByVal searchType As SearchType, ByVal searchWord As String, ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As List(Of Search) Implements ISearchRepository.FindArticles

   '   Dim key As String = "search_articles_" & searchType.ToString() & "_" & searchWord & "_" & startRowIndex.ToString & "_" & maximumRows
   '   Dim art As List(Of Article)
   '   If Cache(key) IsNot Nothing Then
   '      art = DirectCast(Cache(key), List(Of Article))
   '   Else
   '      Select Case searchType
   '         Case searchType.AnyWord
   '            art = _articleService.FindArticlesWithAnyMatch(startRowIndex, maximumRows, searchWord)
   '         Case searchType.AllWords
   '            art = _articleService.FindArticlesWithAnyMatch(startRowIndex, maximumRows, searchWord)
   '         Case searchType.ExactPhrase
   '            art = _articleService.FindArticlesWithExactMatch(startRowIndex, maximumRows, searchWord)
   '         Case Else
   '            art = _articleService.FindArticlesWithAnyMatch(startRowIndex, maximumRows, searchWord)
   '      End Select
   '      CacheData(key, art)
   '   End If

   '   Dim searchResultList As New List(Of Search)
   '   If art IsNot Nothing Then
   '      For Each it As Article In art

   '         Dim url As String = "~/articles/" & it.Slug

   '         Dim parentUrl As String = ""

   '         Dim abs As String
   '         If it.Abstract.Length > 200 Then
   '            abs = it.Abstract.Substring(200)
   '         Else
   '            abs = it.Abstract
   '         End If
   '         searchResultList.Add(New Search(it.ArticleID, it.AddedDate, it.AddedBy, _
   '                                         it.Title, abs, "", _
   '                                         it.ReleaseDate, it.ExpireDate, it.Tags, url, parentUrl, 0, SearchLocation.Articles, "sr-articles"))
   '      Next
   '   End If

   '   Return searchResultList
   'End Function


   'Private Function FindComments(ByVal searchtype As SearchType, ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal searchWord As String) As PagedList(Of ArticleComment)
   '   Select Case searchtype
   '      Case searchtype.AnyWord
   '         Return _articleCommentSrvr.FindCommentsWithAnyMatch(startRowIndex, maximumRows, searchWord)
   '      Case searchtype.ExactPhrase
   '         Return _articleCommentSrvr.FindCommentsWithExactMatch(startRowIndex, maximumRows, searchWord)
   '      Case searchtype.AllWords
   '         Return _articleCommentSrvr.FindCommentsWithAnyMatch(startRowIndex, maximumRows, searchWord)
   '      Case Else
   '         Return _articleCommentSrvr.FindCommentsWithAnyMatch(startRowIndex, maximumRows, searchWord)
   '   End Select
   'End Function


   'Public Function FindArticlesComments(ByVal searchtype As SearchType, ByVal searchWord As String, ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As List(Of Search) Implements ISearchRepository.FindArticlesComments
   '   Dim art As PagedList(Of ArticleComment)
   '   Dim key As String = "search_articlecomments_" & searchtype.ToString() & searchWord & "_" & startRowIndex.ToString & "_" & maximumRows

   '   If Cache(key) IsNot Nothing Then
   '      art = DirectCast(Cache(key), PagedList(Of ArticleComment))
   '   Else
   '      art = FindComments(searchtype, startRowIndex, maximumRows, searchWord)
   '      CacheData(key, art)
   '   End If



   '   Dim searchResultList As New List(Of Search)
   '   If art IsNot Nothing Then
   '      For Each it As ArticleComment In art
   '         Dim at As Article = _articleService.GetArticleById(it.ArticleID)

   '         'Dim url As String = "~/mcc/articles/ShowArticle.aspx?Id=" & at.ID.ToString & "#comments_" & it.ID.ToString
   '         Dim url As String = "~/articles/" & at.Slug & "/?comments_page#comments_" & it.CommentID.ToString


   '         'Dim parentUrl As String = "~/mcc/articles/ShowArticle.aspx?Id=" & at.ID.ToString
   '         Dim parentUrl As String = "~/articles/" & at.Slug & ".aspx"

   '         searchResultList.Add(New Search(it.ArticleID, it.AddedDate, it.AddedBy, _
   '                                         at.Title, it.Body, _
   '                                        it.Body, at.ReleaseDate, at.ExpireDate, at.Tags, _
   '                                        url, parentUrl, 0, SearchLocation.Comments, "sr-comments"))
   '      Next
   '   End If

   '   Return searchResultList
   'End Function


   'Function SearchForVideos(ByVal searchType As SearchType, ByVal searchWord As String, ByVal startRowIndex As Integer, ByVal maximumRows As Integer)
   '   Select Case searchType
   '      Case searchType.ExactPhrase
   '         Return FindVideosWithExactMatch(searchWord, startRowIndex, maximumRows)
   '      Case searchType.AnyWord
   '         Return FindVideosWithExactMatch(searchWord, startRowIndex, maximumRows)
   '      Case searchType.AllWords
   '         Return FindVideosWithAllMatch(searchWord, startRowIndex, maximumRows)
   '      Case Else
   '         Return FindVideosWithExactMatch(searchWord, startRowIndex, maximumRows)
   '   End Select
   'End Function

   'Function FindVideosWithAllMatch(ByVal searchWord As String, ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As List(Of Video)
   '   Return Nothing
   'End Function

   'Function FindVideosWithAnyMatch(ByVal searchWord As String, ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As List(Of Video)
   '   Return Nothing
   'End Function


   'Function FindVideosWithExactMatch(ByVal searchWord As String, ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As List(Of Video)
   '   Return Nothing
   'End Function

   'Public Function FindVideos(ByVal searchType As SearchType, ByVal searchWord As String, ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As List(Of Search) Implements ISearchRepository.FindVideos

   '   Dim art As List(Of Video)

   '   Dim key As String = "search_videos_" & searchType.ToString() & searchWord & "_" & startRowIndex.ToString & "_" & maximumRows
   '   If Cache(key) IsNot Nothing Then
   '      art = DirectCast(Cache(key), List(Of Video))
   '   Else
   '      art = SearchForVideos(searchType, startRowIndex, maximumRows, searchWord)
   '   End If

   '   CacheData(key, art)


   '   Dim searchResultList As New List(Of SearchResult)
   '   If art IsNot Nothing Then
   '      For Each it As Video In art

   '         Dim url As String = "~/mcc/videos/browseVideos.aspx?Id=" & it.VideoID.ToString
   '         Dim parentUrl As String = "~/mcc/videos/browseVideos.aspx?Id=" & it.VideoID.ToString

   '         searchResultList.Add(New Search(it.VideoID, it.AddedDate, it.AddedBy, _
   '                                         it.Title, it.Abstract, _
   '                                        it.Abstract, Nothing, Nothing, "", _
   '                                        url, parentUrl, 0, SearchLocation.Videos, "sr-videos"))
   '      Next
   '   End If

   '   Return searchResultList
   'End Function

   'Public Function FindVideoComments(ByVal searchType As SearchType, ByVal searchWord As String, ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As List(Of Search) Implements ISearchRepository.FindVideoComments
   '   'Dim art As List(Of VideoComment)

   '   'Dim key As String = "search_videocomments_" & searchType.ToString() & "_ " & searchWord & "_" & startRowIndex.ToString & "_" & maximumRows
   '   'If Cache(key) IsNot Nothing Then
   '   '   art = DirectCast(Cache(key), List(Of VideoComment))
   '   'Else
   '   '   If startRowIndex = -1 AndAlso maximumRows = -1 Then
   '   '      art = _videoService.FindComments(searchType, startRowIndex, maximumRows, searchWord)
   '   '   Else
   '   '      art = _videoService.FindComments(searchType, startRowIndex, maximumRows, searchWord)
   '   '   End If
   '   '   CacheData(key, art)
   '   'End If

   '   'Dim searchResultList As New List(Of Search)
   '   'If art IsNot Nothing Then
   '   '   For Each it As VideoComment In art
   '   '      Dim at As Video = _videoService.GetVideoById(it.VideoID)
   '   '      Dim url As String = "~/mcc/videos/ShowVideo.aspx?Id=" & at.VideoID.ToString & "#comments_" & it.VideoID.ToString
   '   '      Dim parentUrl As String = "~/mcc/articles/ShowVideo.aspx?Id=" & at.VideoID.ToString

   '   '      searchResultList.Add(New Search(it.VideoID, it.AddedDate, it.AddedBy, _
   '   '                                      at.Title, it.Body, it.Body, Nothing, Nothing, "", _
   '   '                                     url, parentUrl, 0, SearchLocation.Comments, "sr-comments"))
   '   '   Next
   '   'End If

   '   'Return searchResultList

   '   Return Nothing
   'End Function


   'Public Function FindQuestions(ByVal searchType As SearchType, ByVal searchWord As String, ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As List(Of Search) Implements ISearchRepository.FindQuestions

   '   Dim key As String = "search_questions_" & searchType.ToString() & "_" & searchWord & "_" & startRowIndex.ToString & "_" & maximumRows
   '   Dim uqList As New List(Of UserQuestion)
   '   If Cache(key) IsNot Nothing Then
   '      uqList = DirectCast(Cache(key), List(Of UserQuestion))
   '   Else
   '      Select Case searchType
   '         Case searchType.AllWords
   '            uqList = Nothing
   '         Case searchType.AnyWord
   '            uqList = _userquestionservice.FindQuestionsWithAnyMatch(startRowIndex, maximumRows, searchWord)
   '         Case searchType.ExactPhrase
   '            uqList = _userquestionservice.FindQuestionsWithExactMatch(startRowIndex, maximumRows, searchWord)
   '      End Select

   '      CacheData(key, uqList)
   '   End If

   '   Dim searchResultList As New List(Of SearchResult)
   '   If uqList IsNot Nothing Then
   '      For Each it As UserQuestion In uqList

   '         Dim url As String = "~/questions/" & it.UserQuestionID & "/" & it.Slug

   '         Dim parentUrl As String = ""

   '         Dim abs As String
   '         If it.Body.Length > 200 Then
   '            abs = it.Body.Substring(200)
   '         Else
   '            abs = it.Body
   '         End If
   '         searchResultList.Add(New Search(it.UserQuestionID, it.AddedDate, it.AddedBy, _
   '                                         it.Title, abs, "", _
   '                                         Nothing, Nothing, Nothing, url, parentUrl, 0, SearchLocation.QuestionsAndAnswers, "sr-questions"))
   '      Next
   '   End If

   '   Return searchResultList
   'End Function

   'Public Function FindAll(ByVal searchType As SearchType, ByVal searchWord As String, ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As List(Of Search) Implements ISearchRepository.FindAll
   '   Dim searchResultList As New List(Of SearchResult)
   '   Dim key As String = "search_all_" & searchWord & "_" & startRowIndex.ToString & "_" & maximumRows

   '   If Cache(key) IsNot Nothing Then
   '      searchResultList = DirectCast(Cache(key), List(Of SearchResult))
   '   Else

   '      searchResultList.AddRange(FindArticles(searchType, searchWord, -1, -1))
   '      searchResultList.AddRange(FindArticlesComments(searchType, searchWord, -1, -1))
   '      searchResultList.AddRange(FindVideos(searchType, searchWord, -1, -1))
   '      searchResultList.AddRange(FindQuestions(searchType, searchWord, -1, -1))
   '      CacheData(key, searchResultList)
   '   End If
   '   Return searchResultList.Skip(startRowIndex).Take(maximumRows).ToList
   'End Function

   'Private _searchResults As List(Of SearchResult)
   'Public Property SearchResults() As List(Of SearchResult) Implements ISearchRepository.SearchResults
   '   Get
   '      Return _searchResults
   '   End Get
   '   Set(ByVal value As List(Of SearchResult))
   '      _searchResults = value
   '   End Set
   'End Property

   'Private Function FindArticlesCount(ByVal searchType As SearchType, ByVal searchWord As String) As Integer

   '   Select Case searchType

   '      Case searchType.AnyWord
   '         Return _articleCommentSrvr.FindCommentsWithAnyMatchCount(searchWord)
   '      Case searchType.ExactPhrase
   '         Return _articleCommentSrvr.FindCommentsWithExactMatchCount(searchWord)

   '      Case Else
   '         Return 0

   '   End Select
   'End Function


   'Private Function FindArticleCommentsCount(ByVal searchType As SearchType, ByVal searchWord As String) As Integer
   '   Select Case searchType
   '      Case searchType.AllWords
   '         Return 0
   '      Case searchType.AnyWord
   '         Return _articleCommentSrvr.FindCommentsWithAnyMatchCount(searchWord)
   '      Case searchType.ExactPhrase
   '         Return _articleCommentSrvr.FindCommentsWithExactMatchCount(searchWord)
   '      Case Else
   '         Return 0
   '   End Select
   'End Function


   'Private Function FindVideosCount(ByVal searchType As SearchType, ByVal searchWord As String) As Integer
   '   Select Case searchType
   '      Case searchType.AnyWord
   '         Return _videoCommentService.FindCommentsWithAnyMatchCount(searchWord)
   '      Case searchType.ExactPhrase
   '         Return _videoCommentService.FindCommentsWithExactMatchCount(searchWord)
   '      Case Else
   '         Return 0
   '   End Select
   'End Function


   'Private Function FindVideoCommentsCount(ByVal searchType As SearchType, ByVal searchWord As String) As Integer
   '   Select Case searchType
   '      Case searchType.AllWords
   '         Return 0
   '      Case searchType.AnyWord
   '         Return _videoCommentService.FindCommentsWithAnyMatchCount(searchWord)
   '      Case searchType.ExactPhrase
   '         Return _videoCommentService.FindCommentsWithExactMatchCount(searchWord)
   '      Case Else
   '         Return 0
   '   End Select
   'End Function

   'Public Function FindQuestionsCount(ByVal searchWord As String, ByVal searchtype As SearchType) As Integer
   '   Select Case searchtype
   '      Case searchtype.AllWords
   '         Return 0
   '      Case searchtype.AnyWord
   '         Return 0
   '      Case searchtype.ExactPhrase
   '         Return 0
   '      Case Else
   '         Return 0
   '   End Select
   'End Function


   Public Function FindMatchesCount(ByVal searchWord As String, ByVal fwhere As SearchLocation, ByVal searchType As SearchQueryType) As Integer Implements ISearchService.FindMatchesCount
      If String.IsNullOrEmpty(searchWord) Then
         Return 0
      End If

      Dim i As Integer
      Select Case fwhere
         Case SearchLocation.All
            i = _searchRepository.FindArticles(searchWord, searchType).Count()
            'i += FindArticleCommentsCount(searchType, searchWord)
            i += _searchRepository.FindVideos(searchWord, searchType).Count
            i += _searchRepository.FindUserQuestions(searchWord, searchType).Count
            i += _searchRepository.FindUserAnswers(searchWord, searchType).Count
            Return i
         Case SearchLocation.Articles
            Return _searchRepository.FindArticles(searchWord, searchType).Count()
            'Case SearchLocation.Comments
            'Return FindArticleCommentsCount(searchType, searchWord)
         Case SearchLocation.QuestionsAndAnswers
            i = _searchRepository.FindUserQuestions(searchWord, searchType).Count()
            i += _searchRepository.FindUserAnswers(searchWord, searchType).Count()
            Return i
         Case SearchLocation.Videos
            Return _searchRepository.FindVideos(searchWord, searchType).Count
      End Select

   End Function

   '''' <summary>
   '''' In case of fulltext search modify the keyword accordingly
   '''' </summary>
   '''' <param name="keyword"></param>
   '''' <param name="searchtype"></param>
   '''' <remarks></remarks>
   'Private Function ProcessSearchKeyword(ByVal keyword As String, ByVal searchtype As SearchType) As String

   '   Dim _keyword As String = HttpContext.Current.Server.HtmlEncode(keyword)
   '   Dim words() As String = _keyword.Split(New Char() {" ", "+", ";", ","})
   '   Dim bFirst As Boolean = True


   '   Dim fts As Boolean = Utils.SiteSettings.FullTextSearch

   '   fts = False

   '   If fts Then
   '      Select Case searchtype
   '         Case searchtype.AnyWord
   '            If fts Then
   '               Dim wd As String = ""
   '               For Each word As String In words
   '                  If Not bFirst Then
   '                     wd += " OR "
   '                  Else
   '                     bFirst = False
   '                  End If
   '                  wd += Convert.ToChar(34).ToString + word + Convert.ToChar(34).ToString
   '               Next
   '               _keyword = wd
   '            End If
   '         Case searchtype.AllWords
   '            If fts Then
   '               Dim wd As String = ""
   '               For Each word As String In words
   '                  If Not bFirst Then
   '                     wd += " AND "
   '                  Else
   '                     bFirst = False
   '                  End If
   '                  wd += Convert.ToChar(34).ToString + word + Convert.ToChar(34).ToString
   '               Next
   '               _keyword = wd
   '            End If
   '         Case searchtype.ExactPhrase
   '            If fts Then
   '               _keyword = Convert.ToChar(34).ToString + _keyword + Convert.ToChar(34).ToString
   '            End If
   '      End Select

   '   Else

   '   End If
   '   Return _keyword
   'End Function

   'Public Shared _selectArticles As String = "SELECT * from mcc_articles a where a.approved = 1 "
   'Public Shared _selectVideos As String = "SELECT * from mcc_videos v "


   'Public Function ArticleFinderSqlCommand(ByVal keyword As String, ByVal searchtype As SearchType) As String Implements ISearchRepository.ArticleFinderSqlCommand
   '   If Not String.IsNullOrEmpty(keyword) Then
   '      Dim searchSql As String = "SELECT * from mcc_articles a where a.approved = 1 AND "

   '      Dim _keyword As String = HttpContext.Current.Server.HtmlEncode(keyword)
   '      Dim words() As String = _keyword.Split(New Char() {" ", ","})
   '      Dim bFirst As Boolean = True

   '      Select Case searchtype
   '         Case searchtype.AnyWord
   '            For Each word As String In words
   '               If Not bFirst Then
   '                  searchSql += " OR "
   '               Else
   '                  bFirst = False
   '                  searchSql += String.Format("a.Title like N'%{0}%' OR a.Body LIKE N'%{0}%' OR a.Abstract LIKE N'%{0}%' OR a.tags LIKE N'%{0}%'", word)
   '               End If
   '            Next
   '         Case searchtype.AllWords
   '            For Each word As String In words
   '               If (Not bFirst) Then
   '                  searchSql += " AND "
   '               Else
   '                  bFirst = False
   '                  searchSql += String.Format("(a.Title like N'%{0}%' OR a.Body LIKE N'%{0}%' OR a.Abstract LIKE N'%{0}%' OR a.tags LIKE N'%{0}%')", word)
   '               End If
   '            Next
   '         Case searchtype.ExactPhrase
   '            searchSql += String.Format("a.Title LIKE N'%{0}%' OR a.Body LIKE N'%{0}%' or a.Abstract LIKE N'%{0}%' OR a.tags LIKE N'%{0}%'", keyword)
   '      End Select
   '      Return searchSql
   '   Else
   '      Return Nothing
   '   End If
   'End Function


   Protected Shared Sub CacheData(ByVal key As String, ByVal data As Object)
      If data IsNot Nothing Then
         CacheObject.Cache.Insert(key, data, Nothing, DateTime.Now.AddSeconds(3600), TimeSpan.Zero)
      End If
   End Sub
End Class
