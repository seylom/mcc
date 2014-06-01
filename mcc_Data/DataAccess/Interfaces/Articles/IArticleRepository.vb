Public Interface IArticleRepository

   Function GetArticles() As IQueryable(Of Article)
   Function GetArticlesQuickList() As IQueryable(Of Article)
   Function GetAdvancedArticlesQuickList() As IQueryable(Of ArticleAdv)
   Function GetArticlesInCategory(ByVal categoryID() As Integer) As IQueryable(Of Article)
   Function GetArticlesInCategoryQuickList(ByVal categoryID() As Integer) As IQueryable(Of Article)

   Sub DeleteArticle(ByVal Id As Integer)
   Sub DeleteArticles(ByVal Ids() As Integer)

   Function InsertArticle(ByVal articleToInsert As Article) As Integer
   Sub UpdateArticle(ByVal articleToUpdate As Article)

End Interface
