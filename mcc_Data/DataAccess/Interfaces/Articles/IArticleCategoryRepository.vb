Imports System.Linq
Public Interface IArticleCategoryRepository
   Function GetCategories() As IQueryable(Of ArticleCategory)
   Function InsertCategory(ByVal cat As ArticleCategory) As Integer
   Sub UpdateCategory(ByVal category As ArticleCategory)
   Sub DeleteCategory(ByVal categoryId As Integer)
   Sub DeleteCategories(ByVal categoryIds() As Integer)

   Sub CategorizeArticle(ByVal articleId As Integer, ByVal categoryIds() As Integer)
   Sub RemoveArticlesFromCategories(ByVal articleId As Integer, ByVal categoriesId() As Integer)
   Sub UnCategorizeArticle(ByVal articleId As Integer)
   Function GetArticleCategories(ByVal articleId As Integer) As IQueryable(Of ArticleCategory)
End Interface
