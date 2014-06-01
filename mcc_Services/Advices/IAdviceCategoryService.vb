Imports MCC.Data
Imports Webdiyer.WebControls.Mvc
Public Interface IAdviceCategoryService
   Function GetCategoriesCount() As Integer
   Function GetCategories() As List(Of AdviceCategory)
   Function GetMainCategories() As List(Of AdviceCategory)
   Function GetCategories(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, Optional ByVal sort As String = "Title") As PagedList(Of AdviceCategory)
   Function GetParentCategories() As List(Of AdviceCategory)
   Function GetCategoryById(ByVal categoryId As Integer) As AdviceCategory
   Function GetCategoryBySlug(ByVal slug As String) As AdviceCategory
   Function GetCategoriesByAdviceId(ByVal AdviceId As Integer) As List(Of AdviceCategory)
   Sub InsertCategory(ByVal _artCar As AdviceCategory)
   Sub UpdateCategory(ByVal _artCat As AdviceCategory)
   Sub DeleteCategory(ByVal CategoryID As Integer)
   Sub DeleteCategories(ByVal CategoryIDs As Integer())
End Interface
