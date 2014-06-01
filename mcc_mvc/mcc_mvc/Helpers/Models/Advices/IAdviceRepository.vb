Imports MCC.Data
Namespace Advices
   Public Interface IAdviceRepository
      Function AverageRating(ByVal adviceId As Integer) As Double
      Function GetComments(ByVal adviceId As Integer) As List(Of mcc_AdviceComment)
      Function CommentsCount(ByVal adviceId As Integer) As Integer
      Function GetAdviceCount() As Integer
      Function GetAdviceCountByAuthor(ByVal addedBy As String) As Integer
      Function GetAdviceCount(ByVal categoryId As Integer) As Integer
      Function GetAdviceCount(ByVal PublishedOnly As Boolean) As Integer
      Function GetAdvices() As List(Of mcc_Advice)
      Function GetAdviceCount(ByVal publishedOnly As Boolean, ByVal categoryID As Integer) As Integer
      Function GetAdvices(ByVal categoryId As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AdviceId") As List(Of mcc_Advice)
      Function GetAdvices(ByVal publishedOnly As Boolean, ByVal categoryId As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AdviceId") As List(Of mcc_Advice)
      Function GetAdvicesByAuthor(ByVal addedBy As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedby") As List(Of mcc_Advice)
      Function GetAdvicesByAuthor(ByVal publishedOnly As Boolean, ByVal addedBy As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedby") As List(Of mcc_Advice)
      Function GetAdvices(ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AdviceId") As List(Of mcc_Advice)
      Function GetAdvices(ByVal publishedOnly As Boolean, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AdviceId") As List(Of mcc_Advice)
      Function GetAdviceById(ByVal AdviceId As Integer) As mcc_Advice
      ''' <summary>
      ''' Returns an Advice object with the specified ID
      ''' </summary>
      Function GetLatestAdvices(ByVal pageSize As Integer) As List(Of mcc_Advice)
      Function GetAdviceBySlug(ByVal slug As String) As mcc_Advice
      ''' <summary>
      ''' Creates a new Advice
      ''' </summary>
      Sub InsertAdvice(ByVal title As String, ByVal Abstract As String, ByVal body As String, ByVal approved As Boolean, ByVal listed As Boolean, ByVal commentsEnabled As Boolean, ByVal onlyForMembers As Boolean, ByVal tags As String)
      Sub UpdateAdvice(ByVal AdviceId As Integer, ByVal title As String, ByVal Abstract As String, ByVal body As String, ByVal approved As Boolean, ByVal listed As Boolean, ByVal commentsEnabled As Boolean, ByVal onlyForMembers As Boolean, ByVal tags As String)
      Sub DeleteAdvice(ByVal AdviceId As Integer)
      Sub ApproveAdvice(ByVal AdviceId As Integer)
      Function RemoveAdviceFromCategory(ByVal AdviceID As Integer, Optional ByVal categoryId As Integer = 0) As Boolean
      Function AddAdviceToCategory(ByVal adviceId As Integer, ByVal CategoryId As Integer) As Integer
      Sub IncrementViewCount(ByVal AdviceId As Integer)
      Sub RateAdvice(ByVal adviceId As Integer, ByVal rating As Integer)
      ''' <summary>
      ''' Vote up advice
      ''' </summary>
      Function VoteUpAdvice(ByVal adviceId As Integer) As String
      Function AllVotes(ByVal adviceId As Integer) As String
      ''' <summary>
      ''' vote down advice
      ''' </summary>
      Function VoteDownAdvice(ByVal adviceId As Integer) As String
   End Interface
End Namespace
