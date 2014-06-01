Public Interface IArticleTagRepository
   Sub DeleteTags(ByVal tagIds() As Integer)
   Sub DeleteTag(ByVal tagId As Integer)

   Function GetTags() As IQueryable(Of ArticleTag)
   Function GetArticleTags(ByVal articleId As Integer) As IQueryable(Of ArticleTag)
   Function InsertTag(ByVal tag As ArticleTag) As Integer
   Function InsertTags(ByVal tags As List(Of String)) As List(Of Integer)
    Sub TagArticle(ByVal articleId As Integer, ByVal tags As List(Of String))
End Interface
