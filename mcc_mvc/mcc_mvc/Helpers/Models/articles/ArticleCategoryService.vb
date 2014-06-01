Namespace Articles
   Public Class ArticleCategoryService
      Implements IArticleCategoryService

      Private _repository As IArticleCategoryRepository

      Public Sub New()
         Me.New(New ArticleCategoryRepository())
      End Sub

      Public Sub New(ByVal repo As IArticleCategoryRepository)
         _repository = repo
      End Sub

      Public Sub DeleteCategory(ByVal CategoryID As Integer) Implements IArticleCategoryService.DeleteCategory
         _repository.DeleteCategory(CategoryID)
      End Sub

      Public Function GetCategories() As System.Collections.Generic.List(Of mcc_Category) Implements IArticleCategoryService.GetCategories
         Return _repository.GetCategories()
      End Function

      Public Function GetCategories(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, Optional ByVal sort As String = "Title ASC") As System.Collections.Generic.List(Of mcc_Category) Implements IArticleCategoryService.GetCategories
         Return _repository.GetCategories(startRowIndex, maximumRows, sort)
      End Function

      Public Function GetCategoriesByArticleId(ByVal ArticleId As Integer) As System.Collections.Generic.List(Of mcc_Category) Implements IArticleCategoryService.GetCategoriesByArticleId
         Return _repository.GetCategoriesByArticleId(ArticleId)
      End Function

      Public Function GetCategoriesCount() As Integer Implements IArticleCategoryService.GetCategoriesCount
         Return _repository.GetCategoriesCount()
      End Function

      Public Function GetCategoryById(ByVal categoryId As Integer) As mcc_Category Implements IArticleCategoryService.GetCategoryById
         Return _repository.GetCategoryById(categoryId)
      End Function

      Public Function GetCategoryBySlug(ByVal slug As String) As mcc_Category Implements IArticleCategoryService.GetCategoryBySlug
         Return _repository.GetCategoryBySlug(slug)
      End Function

      Public Function GetMainCategories() As System.Collections.Generic.List(Of mcc_Category) Implements IArticleCategoryService.GetMainCategories
         Return _repository.GetMainCategories()
      End Function

      Public Function GetParentCategories() As System.Collections.Generic.List(Of mcc_Category) Implements IArticleCategoryService.GetParentCategories
         Return _repository.GetParentCategories()
      End Function

      Public Sub InsertCategory(ByVal title As String, ByVal importance As Integer, ByVal description As String, ByVal imageUrl As String, ByVal parentCategoryId As Integer) Implements IArticleCategoryService.InsertCategory
         _repository.InsertCategory(title, importance, description, imageUrl, parentCategoryId)
      End Sub

      Public Sub SetDefaultImageUrl() Implements IArticleCategoryService.SetDefaultImageUrl

      End Sub

      Public Sub UpdateCategory(ByVal CategoryId As Integer, ByVal title As String, ByVal importance As Integer, ByVal description As String, ByVal imageUrl As String, ByVal parentCategoryId As Integer) Implements IArticleCategoryService.UpdateCategory
         _repository.UpdateCategory(CategoryId, title, importance, description, imageUrl, parentCategoryId)
      End Sub
   End Class
End Namespace
