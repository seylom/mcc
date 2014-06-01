Imports MCC.Data
Namespace Searches
   Public Class SearchService
      Implements ISearchService

      Private _searchRepository As ISearchRepository = New SearchRepository()

      Public Function ArticleFinderSqlCommand(ByVal keyword As String, ByVal searchtype As SearchType) As String Implements ISearchService.ArticleFinderSqlCommand
         Return _searchRepository.ArticleFinderSqlCommand(keyword, searchtype)
      End Function

      Public Function FilterNoiseWords(ByVal strInput As String) As String Implements ISearchService.FilterNoiseWords
         Return _searchRepository.FilterNoiseWords(strInput)
      End Function

      Public Function FindAll(ByVal searchType As SearchType, ByVal searchWord As String, ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As System.Collections.Generic.List(Of Search) Implements ISearchService.FindAll
         Return _searchRepository.FindAll(searchType, searchWord, startRowIndex, maximumRows)
      End Function

      Public Function FindArticles(ByVal searchType As SearchType, ByVal searchWord As String, ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As System.Collections.Generic.List(Of Search) Implements ISearchService.FindArticles
         Return _searchRepository.FindArticles(searchType, searchWord, startRowIndex, maximumRows)
      End Function

      Public Function FindArticlesComments(ByVal searchtype As SearchType, ByVal searchWord As String, ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As System.Collections.Generic.List(Of Search) Implements ISearchService.FindArticlesComments
         Return _searchRepository.FindArticlesComments(searchtype, searchWord, startRowIndex, maximumRows)
      End Function

      Public Function FindMatches(ByVal searchWord As String, ByVal fwhere As SearchLocation, ByVal searchType As SearchType, ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As System.Collections.Generic.List(Of Search) Implements ISearchService.FindMatches
         Return _searchRepository.FindMatches(searchWord, fwhere, searchType, startRowIndex, maximumRows)
      End Function

      Public Function FindMatchesCount(ByVal searchWord As String, ByVal fwhere As SearchLocation, ByVal searchType As SearchType) As Integer Implements ISearchService.FindMatchesCount
         Return _searchRepository.FindMatchesCount(searchWord, fwhere, searchType)
      End Function

      Public Function FindQuestions(ByVal searchType As SearchType, ByVal searchWord As String, ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As System.Collections.Generic.List(Of Search) Implements ISearchService.FindQuestions
         Return _searchRepository.FindQuestions(searchType, searchWord, startRowIndex, maximumRows)
      End Function

      Public Function FindVideoComments(ByVal searchType As SearchType, ByVal searchWord As String, ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As System.Collections.Generic.List(Of Search) Implements ISearchService.FindVideoComments
         Return _searchRepository.FindVideoComments(searchType, searchWord, startRowIndex, maximumRows)
      End Function

      Public Function FindVideos(ByVal searchType As SearchType, ByVal searchWord As String, ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As System.Collections.Generic.List(Of Search) Implements ISearchService.FindVideos
         Return _searchRepository.FindVideos(searchType, searchWord, startRowIndex, maximumRows)
      End Function

      Public ReadOnly Property SearchResults() As System.Collections.Generic.List(Of Search) Implements ISearchService.SearchResults
         Get
            Return _searchRepository.SearchResults
         End Get
      End Property
   End Class
End Namespace
