Public Interface IAdviceCategoryRepository
   Function GetCategories() As IQueryable(Of AdviceCategory)
   Function InsertCategory(ByVal cat As AdviceCategory) As Integer
   Sub UpdateCategory(ByVal category As AdviceCategory)
   Sub DeleteCategory(ByVal categoryId As Integer)
   Sub DeleteCategories(ByVal categoryIds() As Integer)

   Sub CategorizeAdvice(ByVal adviceId As Integer, ByVal categoryIds() As Integer)
   Sub RemoveAdvicesFromCategories(ByVal adviceId As Integer, ByVal categoriesId() As Integer)
   Sub UnCategorizeAdvice(ByVal adviceId As Integer)
   Function GetAdviceCategories(ByVal adviceId As Integer) As IQueryable(Of AdviceCategory)
End Interface
