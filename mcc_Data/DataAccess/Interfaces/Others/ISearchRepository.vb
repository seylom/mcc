Public Interface ISearchRepository
   'results
   Function FindArticles(ByVal query As String, ByVal type As SearchQueryType) As IQueryable(Of SearchResult)
   Function FindAdvices(ByVal query As String, ByVal type As SearchQueryType) As IQueryable(Of SearchResult)
   Function FindVideos(ByVal query As String, ByVal type As SearchQueryType) As IQueryable(Of SearchResult)
   Function FindUserQuestions(ByVal query As String, ByVal type As SearchQueryType) As IQueryable(Of SearchResult)
   Function FindUserAnswers(ByVal query As String, ByVal type As SearchQueryType) As IQueryable(Of SearchResult)

   'count ?
End Interface
