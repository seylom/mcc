Public Interface IAdviceRepository
   Function GetAdvices() As IQueryable(Of Advice)
   Function GetAdvicesQuickList() As IQueryable(Of Advice)
   Function GetAdvicesInCategory(ByVal categoryID As Integer) As IQueryable(Of Advice)
   Function GetAdvicesInCategoryQuickList(ByVal categoryID As Integer) As IQueryable(Of Advice)

   Sub DeleteAdvice(ByVal Id As Integer)
   Sub DeleteAdvices(ByVal Ids() As Integer)

   Function InsertAdvice(ByVal adviceToInsert As Advice) As Integer
   Sub UpdateAdvice(ByVal adviceToUpdate As Advice)
End Interface
