Imports MCC.Data
Namespace Advices
   Public Class AdviceService
      Implements IAdviceService

      Private _repository As IAdviceRepository

      Public Sub New()
         Me.New(New AdviceRepository())
      End Sub

      Public Sub New(ByVal adviceRepo As IAdviceRepository)
         _repository = adviceRepo
      End Sub

      Public Function AddAdviceToCategory(ByVal adviceId As Integer, ByVal CategoryId As Integer) As Integer Implements IAdviceService.AddAdviceToCategory
         Return _repository.AddAdviceToCategory(adviceId, CategoryId)
      End Function

      Public Function AllVotes(ByVal adviceId As Integer) As String Implements IAdviceService.AllVotes
         Return _repository.AllVotes(adviceId)
      End Function

      Public Function ApproveAdvice(ByVal AdviceId As Integer) As Boolean Implements IAdviceService.ApproveAdvice
         _repository.ApproveAdvice(AdviceId)
         Return True
      End Function

      Public Function AverageRating(ByVal adviceId As Integer) As Double Implements IAdviceService.AverageRating
         Return _repository.AverageRating(adviceId)
      End Function

      Public Function CommentsCount(ByVal adviceId As Integer) As Integer Implements IAdviceService.CommentsCount
         Return _repository.CommentsCount(adviceId)
      End Function

      Public Function DeleteAdvice(ByVal AdviceId As Integer) As Boolean Implements IAdviceService.DeleteAdvice
         _repository.DeleteAdvice(AdviceId)
         Return True
      End Function

      Public Function GetAdviceBySlug(ByVal slug As String) As mcc_Advice Implements IAdviceService.GetAdviceBySlug
         Return _repository.GetAdviceBySlug(slug)
      End Function

      Public Function GetAdviceCount() As Integer Implements IAdviceService.GetAdviceCount
         Return _repository.GetAdviceCount()
      End Function

      Public Function GetAdviceCount(ByVal PublishedOnly As Boolean) As Integer Implements IAdviceService.GetAdviceCount
         Return _repository.GetAdviceCount(PublishedOnly)
      End Function

      Public Function GetAdviceCount(ByVal publishedOnly As Boolean, ByVal categoryID As Integer) As Integer Implements IAdviceService.GetAdviceCount
         Return _repository.GetAdviceCount(publishedOnly, categoryID)
      End Function

      Public Function GetAdviceCount(ByVal categoryId As Integer) As Integer Implements IAdviceService.GetAdviceCount
         Return _repository.GetAdviceCount(categoryId)
      End Function

      Public Function GetAdviceCountByAuthor(ByVal addedBy As String) As Integer Implements IAdviceService.GetAdviceCountByAuthor
         Return _repository.GetAdviceCountByAuthor(addedBy)
      End Function

      ''' <summary>
      ''' Too generic of a method to be used
      ''' </summary>
      ''' <returns></returns>
      ''' <remarks></remarks>
      Public Function GetAdvices() As System.Collections.Generic.List(Of mcc_Advice) Implements IAdviceService.GetAdvices
         Return _repository.GetAdvices()
      End Function

      Public Function GetAdvices(ByVal publishedOnly As Boolean, ByVal categoryId As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AdviceId") As System.Collections.Generic.List(Of mcc_Advice) Implements IAdviceService.GetAdvices
         Return _repository.GetAdvices(publishedOnly, categoryId, startrowindex, maximumrows, sortExp)
      End Function

      Public Function GetAdvices(ByVal publishedOnly As Boolean, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AdviceId") As System.Collections.Generic.List(Of mcc_Advice) Implements IAdviceService.GetAdvices
         Return _repository.GetAdvices(publishedOnly, startrowindex, maximumrows, sortExp)
      End Function

      Public Function GetAdvices(ByVal categoryId As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AdviceId") As System.Collections.Generic.List(Of mcc_Advice) Implements IAdviceService.GetAdvices
         Return _repository.GetAdvices(categoryId, startrowindex, maximumrows, sortExp)
      End Function

      Public Function GetAdvices(ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AdviceId") As System.Collections.Generic.List(Of mcc_Advice) Implements IAdviceService.GetAdvices
         Return _repository.GetAdvices(startrowindex, maximumrows, sortExp)
      End Function

      Public Function GetAdvicesByAuthor(ByVal publishedOnly As Boolean, ByVal addedBy As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedby") As System.Collections.Generic.List(Of mcc_Advice) Implements IAdviceService.GetAdvicesByAuthor
         Return _repository.GetAdvicesByAuthor(publishedOnly, addedBy, startrowindex, maximumrows, sortExp)
      End Function

      Public Function GetAdvicesByAuthor(ByVal addedBy As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedby") As System.Collections.Generic.List(Of mcc_Advice) Implements IAdviceService.GetAdvicesByAuthor
         Return _repository.GetAdvicesByAuthor(addedBy, startrowindex, maximumrows, sortExp)
      End Function

      Public Function GetComments(ByVal adviceId As Integer) As System.Collections.Generic.List(Of mcc_AdviceComment) Implements IAdviceService.GetComments
         Return _repository.GetComments(adviceId)
      End Function

      Public Function GetLatestAdvices(ByVal pageSize As Integer) As System.Collections.Generic.List(Of mcc_Advice) Implements IAdviceService.GetLatestAdvices
         Return _repository.GetLatestAdvices(pageSize)
      End Function

      Public Function GetAdviceById(ByVal AdviceId As Integer) As mcc_Advice Implements IAdviceService.GetAdviceById
         Return _repository.GetAdviceById(AdviceId)
      End Function

      Public Function IncrementViewCount(ByVal AdviceId As Integer) As Boolean Implements IAdviceService.IncrementViewCount
         _repository.IncrementViewCount(AdviceId)
         Return True
      End Function

      Public Function InsertAdvice(ByVal title As String, ByVal Abstract As String, ByVal body As String, ByVal approved As Boolean, ByVal listed As Boolean, ByVal commentsEnabled As Boolean, ByVal onlyForMembers As Boolean, ByVal tags As String) As Boolean Implements IAdviceService.InsertAdvice
         _repository.InsertAdvice(title, Abstract, body, approved, listed, commentsEnabled, onlyForMembers, tags)
         Return True
      End Function

      Public Function RateAdvice(ByVal adviceId As Integer, ByVal rating As Integer) As Boolean Implements IAdviceService.RateAdvice
         _repository.RateAdvice(adviceId, rating)
         Return True
      End Function

      Public Function RemoveAdviceFromCategory(ByVal AdviceID As Integer, Optional ByVal categoryId As Integer = 0) As Boolean Implements IAdviceService.RemoveAdviceFromCategory
         Return _repository.RemoveAdviceFromCategory(AdviceID, categoryId)
      End Function

      Public Function UpdateAdvice(ByVal AdviceId As Integer, ByVal title As String, ByVal Abstract As String, ByVal body As String, ByVal approved As Boolean, ByVal listed As Boolean, ByVal commentsEnabled As Boolean, ByVal onlyForMembers As Boolean, ByVal tags As String) As Boolean Implements IAdviceService.UpdateAdvice
         _repository.UpdateAdvice(AdviceId, title, Abstract, body, approved, listed, commentsEnabled, onlyForMembers, tags)
         Return True
      End Function

      Public Function VoteDownAdvice(ByVal adviceId As Integer) As String Implements IAdviceService.VoteDownAdvice
         Return _repository.AllVotes(adviceId)
      End Function

      Public Function VoteUpAdvice(ByVal adviceId As Integer) As String Implements IAdviceService.VoteUpAdvice
         Return _repository.VoteUpAdvice(adviceId)
      End Function
   End Class
End Namespace
