Imports MCC.Data
Imports Webdiyer.WebControls.Mvc
Public Interface IAdviceService
   ''' <summary>
   ''' Get the average rating for an advice
   ''' </summary>
   ''' <param name="adviceId">The advice id.</param>
   ''' <returns></returns>
   Function AverageRating(ByVal adviceId As Integer) As Double
   ''' <summary>
   ''' Gets the comments.
   ''' </summary>
   ''' <param name="adviceId">The advice id.</param>
   ''' <returns></returns>
   Function GetComments(ByVal adviceId As Integer) As IList(Of AdviceComment)
   Function CommentsCount(ByVal AdviceId As Integer) As Integer
   Function Published(ByVal adviceId As Integer) As Boolean
   Function GetAdviceCount() As Integer
   Function GetAdviceCount(ByVal PublishedOnly As Boolean) As Integer
   Function GetAdviceCountByAuthor(ByVal publishedOnly As Boolean, ByVal addedBy As String) As Integer
   Function GetAdviceCountByAuthor(ByVal addedBy As String) As Integer
   Function GetAdviceCount(ByVal categoryId As Integer) As Integer
   Function GetAdviceCount(ByVal publishedOnly As Boolean, ByVal categoryID As Integer) As Integer
   Function GetAdvices() As List(Of Advice)
   Function GetAdvices(ByVal publishedOnly As Boolean, ByVal categoryId As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AddedDate") As PagedList(Of Advice)
   'Function GetAdvicesByCategoryName(ByVal publishedOnly As Boolean, ByVal categoryName As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "releaseDate") As PagedList(Of Advice)
   Function GetAdvices(ByVal categoryId As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AddedDate") As PagedList(Of Advice)
   'Function GetAdvicesByCategoryName(ByVal categoryName As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "releaseDate") As List(Of Advice)
   Function GetAdvicesByAuthor(ByVal addedBy As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedby") As PagedList(Of Advice)
   Function GetAdvicesByAuthor(ByVal publishedOnly As Boolean, ByVal addedBy As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedby") As PagedList(Of Advice)
   Function GetAdvices(ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AddedDate DESC") As PagedList(Of Advice)
   Function GetAdvices(ByVal publishedOnly As Boolean, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AddedDate DESC") As PagedList(Of Advice)
     ''' <summary>
   ''' Returns an Advice object with the specified ID
   ''' </summary>
   Function GetLatestAdvices(ByVal pageSize As Integer) As List(Of Advice)
   Function GetAdviceById(ByVal AdviceId As Integer) As Advice
   Function GetAdviceBySlug(ByVal slug As String) As Advice
   ''' <summary>
   ''' Creates a new Advice
   ''' </summary>
   Function SaveAdvice(ByVal adv As Advice) As Boolean
   'Sub InsertAdvice(ByVal wrd As Advice)
   'Sub UpdateAdvice(ByVal _art As Advice)

   Sub DeleteAdvices(ByVal AdviceIds As Integer())
   Sub DeleteAdvice(ByVal AdviceId As Integer)
   Sub ApproveAdvice(ByVal adviceId As Integer)
   Function RemoveAdviceFromCategory(ByVal AdviceID As Integer, Optional ByVal categoryId As Integer = 0) As Boolean
   Function AddAdviceToCategories(ByVal AdviceId As Integer, ByVal CategoryIds As Integer()) As Integer
   Sub IncrementViewCount(ByVal adviceId As Integer)
   Sub RateAdvice(ByVal adviceId As Integer, ByVal rating As Integer)
   Function FindAdvicesWithExactMatch(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal SearchWord As String) As PagedList(Of Advice)
   Function FindAdvicesWithAnyMatch(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal SearchWord As String) As PagedList(Of Advice)
    Function FindAdviceWithExactMatchCount(ByVal SearchWord As String) As Integer
    Function FindAdviceWithAnyMatchCount(ByVal SearchWord As String) As Integer

   Function GetAdvicesCountByStatus(ByVal status As Integer) As Integer
   Function GetAdvicesCountInCategoryByStatus(ByVal categoryID As Integer, ByVal status As Integer) As Integer
   Function GetAdvicesByStatus(ByVal status As Integer, ByVal startRowIndex As Integer, ByVal maximumRows As Integer, Optional ByVal sortExp As String = "Title") As PagedList(Of Advice)
   Function GetAdvicesByStatus(ByVal categoryID As Integer, ByVal status As Integer, ByVal startRowIndex As Integer, ByVal maximumRows As Integer, Optional ByVal sortExp As String = "AddedDate") As PagedList(Of Advice)
   Sub UpdateAdviceStatus(ByVal id As Integer)
   Sub UpdateAdviceStatus(ByVal id As Integer, ByVal st As Integer)
   Function GetAdviceStatusValue(ByVal id As Integer) As Integer
   Function FixAdvicesVideoIds() As Boolean
   Function GetReadersPick() As Advice

   Function VoteUpAdvice(ByVal adviceId As Integer) As Object
   Function VoteDownAdvice(ByVal adviceId As Integer) As Object
End Interface
