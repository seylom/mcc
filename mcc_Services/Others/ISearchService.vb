Imports MCC.Data
Imports Webdiyer.WebControls.Mvc

Public Interface ISearchService
   Function FindMatches(ByVal searchWord As String, ByVal fwhere As SearchLocation, ByVal searchType As SearchQueryType, ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As PagedList(Of SearchResult)
   Function FindArticles(ByVal searchWord As String, ByVal searchType As SearchQueryType, ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As PagedList(Of SearchResult)
   Function FindAdvices(ByVal searchWord As String, ByVal searchType As SearchQueryType, ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As PagedList(Of SearchResult)
   Function FindUserQuestions(ByVal searchWord As String, ByVal searchType As SearchQueryType, ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As PagedList(Of SearchResult)
   Function FindUserAnswers(ByVal searchWord As String, ByVal searchType As SearchQueryType, ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As PagedList(Of SearchResult)
   Function FindVideos(ByVal searchWord As String, ByVal searchType As SearchQueryType, ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As PagedList(Of SearchResult)
   Function FindMatchesCount(ByVal searchWord As String, ByVal fwhere As SearchLocation, ByVal searchType As SearchQueryType) As Integer
End Interface
