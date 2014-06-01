Imports MCC.Data
Imports Webdiyer.WebControls.Mvc
Public Interface IVideoCategoryService
   Function GetCategoriesCount() As Integer
   Function GetCategories() As List(Of VideoCategory)
   Function GetMainCategories() As List(Of VideoCategory)
   Function GetCategories(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, Optional ByVal sort As String = "Title") As PagedList(Of VideoCategory)
   Function GetParentCategories() As List(Of VideoCategory)
   Function GetCategoryById(ByVal categoryId As Integer) As VideoCategory
   Function GetCategoryBySlug(ByVal slug As String) As VideoCategory
   Function GetCategoriesByVideoId(ByVal VideoId As Integer) As List(Of VideoCategory)
   Sub InsertCategory(ByVal _artCar As VideoCategory)
   Sub UpdateCategory(ByVal _artCat As VideoCategory)
   Sub DeleteCategory(ByVal CategoryID As Integer)
   Sub DeleteCategories(ByVal CategoryIDs As Integer())
End Interface
