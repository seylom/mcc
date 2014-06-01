Imports MCC.Data
Imports Webdiyer.WebControls.Mvc
Public Interface IArticleCategoryService
   Function GetCategoriesCount() As Integer
   Function GetCategories() As List(Of ArticleCategory)
   Function GetMainCategories() As List(Of ArticleCategory)
   Function GetCategories(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, Optional ByVal sort As String = "Title") As PagedList(Of ArticleCategory)
   Function GetParentCategories() As List(Of ArticleCategory)
   Function GetChildrenCategories(ByVal category As String) As List(Of String)
   Function GetCategoryById(ByVal categoryId As Integer) As ArticleCategory
   Function GetCategoryBySlug(ByVal slug As String) As ArticleCategory
   Function GetCategoriesByArticleId(ByVal ArticleId As Integer) As List(Of ArticleCategory)
   'Sub InsertCategory(ByVal _artCar As ArticleCategory)
   'Sub UpdateCategory(ByVal _artCat As ArticleCategory)
   Sub SaveCategory(ByVal _artCar As ArticleCategory)
   Sub DeleteCategory(ByVal CategoryID As Integer)
   Sub DeleteCategories(ByVal CategoryIDs As Integer())
End Interface
