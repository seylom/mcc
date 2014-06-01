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

Imports System.Linq
Imports MCC.Services
Imports MCC.Data

Namespace Searches
   Public Class SearchRepository
      Inherits mccObject
      Implements ISearchRepository

      Private _articleService As IArticleService = New ArticleService()
      Private _articleCommentSrvr As IArticleCommentService = New ArticleCommentService()
      Private _videoService As IVideoService = New VideoService()
      Private _videoCommentService As IVideoCommentService = New VideoCommentService()
      Private _userquestionservice As IUserQuestionService = New UserQuestionService()
      Private _useranswerservice As IUserAnswerService = New UserAnswerService()

      Public Function FindMatches(ByVal searchWord As String, ByVal fwhere As SearchLocation, ByVal searchType As SearchType, ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As List(Of Search) Implements ISearchRepository.FindMatches
         If Not String.IsNullOrEmpty(searchWord) Then


            Dim ls As New List(Of Search)
            If Utils.SiteSettings.FullTextSearch Then
               If Not String.IsNullOrEmpty(searchWord) Then
                  'searchWord = FilterNoiseWords(searchWord)

                  Dim it As Integer = 0
                  If Integer.TryParse(searchType, it) Then
                     searchWord = ProcessSearchKeyword(searchWord, CType(it, SearchType))
                  Else
                     searchWord = ProcessSearchKeyword(searchWord, searchType.AnyWord)
                  End If

                  Select Case fwhere
                     Case SearchLocation.Articles
                        ls = FindArticles(searchType, searchWord, startRowIndex, maximumRows)
                     Case SearchLocation.Comments
                        ls = FindArticlesComments(searchType, searchWord, startRowIndex, maximumRows)
                     Case SearchLocation.Videos
                        ls = FindVideos(searchType, searchWord, startRowIndex, maximumRows)
                     Case SearchLocation.QuestionsAndAnswers
                        ls = FindQuestions(searchType, searchWord, startRowIndex, maximumRows)
                     Case SearchLocation.StaticPages

                     Case SearchLocation.All
                        ls = FindAll(searchType, searchWord, startRowIndex, maximumRows)
                     Case Else
                        ls = FindArticles(searchType, searchWord, startRowIndex, maximumRows)
                  End Select
                  Return ls
               Else
                  Return New List(Of Search)
               End If

            Else
               Dim sqlSearchCommand As String = ArticleFinderSqlCommand(searchWord, searchType.AnyWord)
               Dim mdc As New MCCDataContext
               Dim li As List(Of mcc_Article) = mdc.ExecuteQuery(Of mcc_Article)(sqlSearchCommand).Skip(startRowIndex).Take(maximumRows).ToList

               If li.Count > 0 Then
                  For Each it As mcc_Article In li

                     Dim url As String = "~/articles/" & it.Slug

                     Dim parentUrl As String = ""

                     Dim abs As String
                     If it.Abstract.Length > 200 Then
                        abs = it.Abstract.Substring(200)
                     Else
                        abs = it.Abstract
                     End If
                     ls.Add(New Search(it.ArticleID, it.AddedDate, it.AddedBy, _
                                                     it.Title, abs, "", _
                                                     it.ReleaseDate, it.ExpireDate, it.Tags, url, parentUrl, 0, SearchLocation.Articles, "sr-articles"))
                  Next
                  Return ls
               Else
                  Return Nothing
               End If
            End If
         Else
            Return Nothing
         End If
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

      Public Shared noisewords() As String = {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "$", "a", "about", "after", "all", "also", "an", "and", "another", "any", "are", "as", "at", "b", "be", "because", "been", "before", "being", "between", "both", "but", "by", "c", "came", "can", "come", "could", "d", "did", "do", "does", "e", "each", "else", "f", "for", "from", "g", "get", "got", "h", "had", "has", "have", "he", "her", "here", "him", "himself", "his", "how", "i", "if", "in", "into", "is", "it", "its", "j", "just", "k", "l", "like", "m", "make", "many", "me", "might", "more", "most", "much", "must", "my", "n", "never", "now", "o", "of", "on", "only", "or", "other", "our", "out", "over", "p", "q", "r", "re", "s", "said", "same", "see", "should", "since", "so", "some", "still", "such", "t", "take", "Test", "than", "that", "the", "their", "them", "then", "there", "these", "they", "this", "those", "through", "to", "too", "u", "under", "up", "use", "v", "very", "w", "want", "was", "way", "we", "well", "were", "what", "when", "where", "which", "while", "who", "will", "with", "would", "x", "y", "you", "your", "z"}

      Public Function FilterNoiseWords(ByVal strInput As String) As String Implements ISearchRepository.FilterNoiseWords
         If Not String.IsNullOrEmpty(strInput) Then
            'strInput = strInput.Trim(noisewords)
            Return strInput
         Else
            Return strInput
         End If
      End Function


      Public Function FindArticles(ByVal searchType As SearchType, ByVal searchWord As String, ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As List(Of Search) Implements ISearchRepository.FindArticles

         Dim key As String = "search_articles_" & searchType.ToString() & "_" & searchWord & "_" & startRowIndex.ToString & "_" & maximumRows
         Dim art As List(Of Article)
         If Cache(key) IsNot Nothing Then
            art = DirectCast(Cache(key), List(Of Article))
         Else
            Select Case searchType
               Case searchType.AnyWord
                  art = _articleService.FindArticlesWithAnyMatch(startRowIndex, maximumRows, searchWord)
               Case searchType.AllWords
                  art = _articleService.FindArticlesWithAnyMatch(startRowIndex, maximumRows, searchWord)
               Case searchType.ExactPhrase
                  art = _articleService.FindArticlesWithExactMatch(startRowIndex, maximumRows, searchWord)
               Case Else
                  art = _articleService.FindArticlesWithAnyMatch(startRowIndex, maximumRows, searchWord)
            End Select
            CacheData(key, art)
         End If

         Dim searchResultList As New List(Of Search)
         If art IsNot Nothing Then
            For Each it As Article In art

               'Dim url As String = "~/mcc/articles/ShowArticle.aspx?Id=" & it.ID.ToString
               Dim url As String = "~/articles/" & it.Slug



               ' " "     "~/mcc/articles/default.aspx?CatId=" & it.Categories.ToString
               Dim parentUrl As String = ""

               Dim abs As String
               If it.Abstract.Length > 200 Then
                  abs = it.Abstract.Substring(200)
               Else
                  abs = it.Abstract
               End If
               searchResultList.Add(New Search(it.ArticleID, it.AddedDate, it.AddedBy, _
                                               it.Title, abs, "", _
                                               it.ReleaseDate, it.ExpireDate, it.Tags, url, parentUrl, 0, SearchLocation.Articles, "sr-articles"))
            Next
         End If

         Return searchResultList
      End Function


      Private Function FindComments(ByVal searchtype As SearchType, ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal searchWord As String) As PagedList(Of ArticleComment)
         Select Case searchtype
            Case searchtype.AnyWord
               Return _articleCommentSrvr.FindCommentsWithAnyMatch(startRowIndex, maximumRows, searchWord)
            Case searchtype.ExactPhrase
               Return _articleCommentSrvr.FindCommentsWithExactMatch(startRowIndex, maximumRows, searchWord)
            Case searchtype.AllWords
               Return _articleCommentSrvr.FindCommentsWithAnyMatch(startRowIndex, maximumRows, searchWord)
            Case Else
               Return _articleCommentSrvr.FindCommentsWithAnyMatch(startRowIndex, maximumRows, searchWord)
         End Select
      End Function


      Public Function FindArticlesComments(ByVal searchtype As SearchType, ByVal searchWord As String, ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As List(Of Search) Implements ISearchRepository.FindArticlesComments
         Dim art As PagedList(Of ArticleComment)
         Dim key As String = "search_articlecomments_" & searchtype.ToString() & searchWord & "_" & startRowIndex.ToString & "_" & maximumRows

         If Cache(key) IsNot Nothing Then
            art = DirectCast(Cache(key), PagedList(Of ArticleComment))
         Else
            art = FindComments(searchtype, startRowIndex, maximumRows, searchWord)
            CacheData(key, art)
         End If



         Dim searchResultList As New List(Of Search)
         If art IsNot Nothing Then
            For Each it As ArticleComment In art
               Dim at As Article = _articleService.GetArticleById(it.ArticleID)

               'Dim url As String = "~/mcc/articles/ShowArticle.aspx?Id=" & at.ID.ToString & "#comments_" & it.ID.ToString
               Dim url As String = "~/articles/" & at.Slug & "/?comments_page#comments_" & it.CommentID.ToString


               'Dim parentUrl As String = "~/mcc/articles/ShowArticle.aspx?Id=" & at.ID.ToString
               Dim parentUrl As String = "~/articles/" & at.Slug & ".aspx"

               searchResultList.Add(New Search(it.ArticleID, it.AddedDate, it.AddedBy, _
                                               at.Title, it.Body, _
                                              it.Body, at.ReleaseDate, at.ExpireDate, at.Tags, _
                                              url, parentUrl, 0, SearchLocation.Comments, "sr-comments"))
            Next
         End If

         Return searchResultList
      End Function


      Function SearchForVideos(ByVal searchType As SearchType, ByVal searchWord As String, ByVal startRowIndex As Integer, ByVal maximumRows As Integer)
         Select Case searchType
            Case searchType.ExactPhrase
               Return FindVideosWithExactMatch(searchWord, startRowIndex, maximumRows)
            Case searchType.AnyWord
               Return FindVideosWithExactMatch(searchWord, startRowIndex, maximumRows)
            Case searchType.AllWords
               Return FindVideosWithAllMatch(searchWord, startRowIndex, maximumRows)
            Case Else
               Return FindVideosWithExactMatch(searchWord, startRowIndex, maximumRows)
         End Select
      End Function

      Function FindVideosWithAllMatch(ByVal searchWord As String, ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As List(Of Video)
         Return Nothing
      End Function

      Function FindVideosWithAnyMatch(ByVal searchWord As String, ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As List(Of Video)
         Return Nothing
      End Function


      Function FindVideosWithExactMatch(ByVal searchWord As String, ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As List(Of Video)
         Return Nothing
      End Function

      Public Function FindVideos(ByVal searchType As SearchType, ByVal searchWord As String, ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As List(Of Search) Implements ISearchRepository.FindVideos

         Dim art As List(Of Video)

         Dim key As String = "search_videos_" & searchType.ToString() & searchWord & "_" & startRowIndex.ToString & "_" & maximumRows
         If Cache(key) IsNot Nothing Then
            art = DirectCast(Cache(key), List(Of Video))
         Else
            art = SearchForVideos(searchType, startRowIndex, maximumRows, searchWord)
         End If

         CacheData(key, art)


         Dim searchResultList As New List(Of Search)
         If art IsNot Nothing Then
            For Each it As Video In art

               Dim url As String = "~/mcc/videos/browseVideos.aspx?Id=" & it.VideoID.ToString
               Dim parentUrl As String = "~/mcc/videos/browseVideos.aspx?Id=" & it.VideoID.ToString

               searchResultList.Add(New Search(it.VideoID, it.AddedDate, it.AddedBy, _
                                               it.Title, it.Abstract, _
                                              it.Abstract, Nothing, Nothing, "", _
                                              url, parentUrl, 0, SearchLocation.Videos, "sr-videos"))
            Next
         End If

         Return searchResultList
      End Function

      Public Function FindVideoComments(ByVal searchType As SearchType, ByVal searchWord As String, ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As List(Of Search) Implements ISearchRepository.FindVideoComments
         'Dim art As List(Of VideoComment)

         'Dim key As String = "search_videocomments_" & searchType.ToString() & "_ " & searchWord & "_" & startRowIndex.ToString & "_" & maximumRows
         'If Cache(key) IsNot Nothing Then
         '   art = DirectCast(Cache(key), List(Of VideoComment))
         'Else
         '   If startRowIndex = -1 AndAlso maximumRows = -1 Then
         '      art = _videoService.FindComments(searchType, startRowIndex, maximumRows, searchWord)
         '   Else
         '      art = _videoService.FindComments(searchType, startRowIndex, maximumRows, searchWord)
         '   End If
         '   CacheData(key, art)
         'End If

         'Dim searchResultList As New List(Of Search)
         'If art IsNot Nothing Then
         '   For Each it As VideoComment In art
         '      Dim at As Video = _videoService.GetVideoById(it.VideoID)
         '      Dim url As String = "~/mcc/videos/ShowVideo.aspx?Id=" & at.VideoID.ToString & "#comments_" & it.VideoID.ToString
         '      Dim parentUrl As String = "~/mcc/articles/ShowVideo.aspx?Id=" & at.VideoID.ToString

         '      searchResultList.Add(New Search(it.VideoID, it.AddedDate, it.AddedBy, _
         '                                      at.Title, it.Body, it.Body, Nothing, Nothing, "", _
         '                                     url, parentUrl, 0, SearchLocation.Comments, "sr-comments"))
         '   Next
         'End If

         'Return searchResultList

         Return Nothing
      End Function


      Public Function FindQuestions(ByVal searchType As SearchType, ByVal searchWord As String, ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As List(Of Search) Implements ISearchRepository.FindQuestions

         Dim key As String = "search_questions_" & searchType.ToString() & "_" & searchWord & "_" & startRowIndex.ToString & "_" & maximumRows
         Dim uqList As New List(Of UserQuestion)
         If Cache(key) IsNot Nothing Then
            uqList = DirectCast(Cache(key), List(Of UserQuestion))
         Else
            Select Case searchType
               Case searchType.AllWords
                  uqList = Nothing
               Case searchType.AnyWord
                  uqList = _userquestionservice.FindQuestionsWithAnyMatch(startRowIndex, maximumRows, searchWord)
               Case searchType.ExactPhrase
                  uqList = _userquestionservice.FindQuestionsWithExactMatch(startRowIndex, maximumRows, searchWord)
            End Select

            CacheData(key, uqList)
         End If

         Dim searchResultList As New List(Of Search)
         If uqList IsNot Nothing Then
            For Each it As UserQuestion In uqList

               Dim url As String = "~/questions/" & it.UserQuestionID & "/" & it.Slug

               Dim parentUrl As String = ""

               Dim abs As String
               If it.Body.Length > 200 Then
                  abs = it.Body.Substring(200)
               Else
                  abs = it.Body
               End If
               searchResultList.Add(New Search(it.UserQuestionID, it.AddedDate, it.AddedBy, _
                                               it.Title, abs, "", _
                                               Nothing, Nothing, Nothing, url, parentUrl, 0, SearchLocation.QuestionsAndAnswers, "sr-questions"))
            Next
         End If

         Return searchResultList
      End Function

      Public Function FindAll(ByVal searchType As SearchType, ByVal searchWord As String, ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As List(Of Search) Implements ISearchRepository.FindAll
         Dim searchResultList As New List(Of Search)
         Dim key As String = "search_all_" & searchWord & "_" & startRowIndex.ToString & "_" & maximumRows

         If Cache(key) IsNot Nothing Then
            searchResultList = DirectCast(Cache(key), List(Of Search))
         Else

            searchResultList.AddRange(FindArticles(searchType, searchWord, -1, -1))
            searchResultList.AddRange(FindArticlesComments(searchType, searchWord, -1, -1))
            searchResultList.AddRange(FindVideos(searchType, searchWord, -1, -1))
            searchResultList.AddRange(FindQuestions(searchType, searchWord, -1, -1))
            CacheData(key, searchResultList)
         End If
         Return searchResultList.Skip(startRowIndex).Take(maximumRows).ToList
      End Function

      Private _searchResults As List(Of Search)
      Public Property SearchResults() As List(Of Search) Implements ISearchRepository.SearchResults
         Get
            Return _searchResults
         End Get
         Set(ByVal value As List(Of Search))
            _searchResults = value
         End Set
      End Property

      Private Function FindArticlesCount(ByVal searchType As SearchType, ByVal searchWord As String) As Integer

         Select Case searchType

            Case searchType.AnyWord
               Return _articleCommentSrvr.FindCommentsWithAnyMatchCount(searchWord)
            Case searchType.ExactPhrase
               Return _articleCommentSrvr.FindCommentsWithExactMatchCount(searchWord)

            Case Else
               Return 0

         End Select
      End Function


      Private Function FindArticleCommentsCount(ByVal searchType As SearchType, ByVal searchWord As String) As Integer
         Select Case searchType
            Case searchType.AllWords
               Return 0
            Case searchType.AnyWord
               Return _articleCommentSrvr.FindCommentsWithAnyMatchCount(searchWord)
            Case searchType.ExactPhrase
               Return _articleCommentSrvr.FindCommentsWithExactMatchCount(searchWord)
            Case Else
               Return 0
         End Select
      End Function


      Private Function FindVideosCount(ByVal searchType As SearchType, ByVal searchWord As String) As Integer
         Select Case searchType
            Case searchType.AnyWord
               Return _videoCommentService.FindCommentsWithAnyMatchCount(searchWord)
            Case searchType.ExactPhrase
               Return _videoCommentService.FindCommentsWithExactMatchCount(searchWord)
            Case Else
               Return 0
         End Select
      End Function


      Private Function FindVideoCommentsCount(ByVal searchType As SearchType, ByVal searchWord As String) As Integer
         Select Case searchType
            Case searchType.AllWords
               Return 0
            Case searchType.AnyWord
               Return _videoCommentService.FindCommentsWithAnyMatchCount(searchWord)
            Case searchType.ExactPhrase
               Return _videoCommentService.FindCommentsWithExactMatchCount(searchWord)
            Case Else
               Return 0
         End Select
      End Function

      Public Function FindQuestionsCount(ByVal searchWord As String, ByVal searchtype As SearchType) As Integer
         Select Case searchtype
            Case searchtype.AllWords
               Return 0
            Case searchtype.AnyWord
               Return 0
            Case searchtype.ExactPhrase
               Return 0
            Case Else
               Return 0
         End Select
      End Function


      Public Function FindMatchesCount(ByVal searchWord As String, ByVal fwhere As SearchLocation, ByVal searchType As SearchType) As Integer Implements ISearchRepository.FindMatchesCount
         If searchWord <> "" Then
            Dim it As Integer = 0
            If Integer.TryParse(searchType, it) Then
               searchWord = ProcessSearchKeyword(searchWord, CType(it, SearchType))
            Else
               searchWord = ProcessSearchKeyword(searchWord, searchType.AnyWord)
            End If

            Dim i As Integer
            If Utils.SiteSettings.FullTextSearch Then
               Select Case fwhere
                  Case SearchLocation.All
                     Dim key As String = "search_allcount_" & searchWord
                     If Cache(key) IsNot Nothing Then
                        i = CInt(Cache(key))
                     Else
                        i = FindArticlesCount(searchType, searchWord)
                        i += FindArticleCommentsCount(searchType, searchWord)
                        i += FindVideosCount(searchType, searchWord)
                        i += FindQuestionsCount(searchWord, searchType)
                        CacheData(key, i)
                     End If
                     Return i
                  Case SearchLocation.Articles
                     Return FindArticlesCount(searchType, searchWord)
                  Case SearchLocation.Comments
                     Return FindArticleCommentsCount(searchType, searchWord)
                  Case SearchLocation.QuestionsAndAnswers
                     Return FindQuestionsCount(searchWord, searchType)
                  Case SearchLocation.Videos
                     Return FindVideosCount(searchType, searchWord)
               End Select
            Else

               Dim sqlSearchCommand As String = ArticleFinderSqlCommand(searchWord, searchType.AnyWord)
               Dim mdc As New MCCDataContext
               i = mdc.ExecuteQuery(Of mcc_Article)(sqlSearchCommand).Count


               Return i
            End If
         Else
            Return 0
         End If
      End Function

      ''' <summary>
      ''' In case of fulltext search modify the keyword accordingly
      ''' </summary>
      ''' <param name="keyword"></param>
      ''' <param name="searchtype"></param>
      ''' <remarks></remarks>
      Private Function ProcessSearchKeyword(ByVal keyword As String, ByVal searchtype As SearchType) As String

         Dim _keyword As String = HttpContext.Current.Server.HtmlEncode(keyword)
         Dim words() As String = _keyword.Split(New Char() {" ", "+", ";", ","})
         Dim bFirst As Boolean = True


         Dim fts As Boolean = Utils.SiteSettings.FullTextSearch


         fts = False

         If fts Then
            Select Case searchtype
               Case searchtype.AnyWord
                  If fts Then
                     Dim wd As String = ""
                     For Each word As String In words
                        If Not bFirst Then
                           wd += " OR "
                        Else
                           bFirst = False
                        End If
                        wd += Convert.ToChar(34).ToString + word + Convert.ToChar(34).ToString
                     Next
                     _keyword = wd
                  End If
               Case searchtype.AllWords
                  If fts Then
                     Dim wd As String = ""
                     For Each word As String In words
                        If Not bFirst Then
                           wd += " AND "
                        Else
                           bFirst = False
                        End If
                        wd += Convert.ToChar(34).ToString + word + Convert.ToChar(34).ToString
                     Next
                     _keyword = wd
                  End If
               Case searchtype.ExactPhrase
                  If fts Then
                     _keyword = Convert.ToChar(34).ToString + _keyword + Convert.ToChar(34).ToString
                  End If
            End Select

         Else

         End If
         Return _keyword
      End Function

      Public Shared _selectArticles As String = "SELECT * from mcc_articles a where a.approved = 1 "
      Public Shared _selectVideos As String = "SELECT * from mcc_videos v "


      Public Function ArticleFinderSqlCommand(ByVal keyword As String, ByVal searchtype As SearchType) As String Implements ISearchRepository.ArticleFinderSqlCommand
         If Not String.IsNullOrEmpty(keyword) Then
            Dim searchSql As String = "SELECT * from mcc_articles a where a.approved = 1 AND "

            Dim _keyword As String = HttpContext.Current.Server.HtmlEncode(keyword)
            Dim words() As String = _keyword.Split(New Char() {" ", ","})
            Dim bFirst As Boolean = True

            Select Case searchtype
               Case searchtype.AnyWord
                  For Each word As String In words
                     If Not bFirst Then
                        searchSql += " OR "
                     Else
                        bFirst = False
                        searchSql += String.Format("a.Title like N'%{0}%' OR a.Body LIKE N'%{0}%' OR a.Abstract LIKE N'%{0}%' OR a.tags LIKE N'%{0}%'", word)
                     End If
                  Next
               Case searchtype.AllWords
                  For Each word As String In words
                     If (Not bFirst) Then
                        searchSql += " AND "
                     Else
                        bFirst = False
                        searchSql += String.Format("(a.Title like N'%{0}%' OR a.Body LIKE N'%{0}%' OR a.Abstract LIKE N'%{0}%' OR a.tags LIKE N'%{0}%')", word)
                     End If
                  Next
               Case searchtype.ExactPhrase
                  searchSql += String.Format("a.Title LIKE N'%{0}%' OR a.Body LIKE N'%{0}%' or a.Abstract LIKE N'%{0}%' OR a.tags LIKE N'%{0}%'", keyword)
            End Select
            Return searchSql
         Else
            Return Nothing
         End If
      End Function


      Protected Shared Sub CacheData(ByVal key As String, ByVal data As Object)
         If data IsNot Nothing Then
            mccObject.Cache.Insert(key, data, Nothing, DateTime.Now.AddSeconds(3600), TimeSpan.Zero)
         End If
      End Sub
   End Class
End Namespace
