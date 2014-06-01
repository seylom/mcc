Imports MCC.Data
Public Interface IArticleTagService
   Function GetArticleTagsCount() As Integer
   Function GetArticleTags() As List(Of ArticleTag)
   Function SuggestArticleTags(ByVal criteria As String) As List(Of String)
   Function GetTagById(ByVal tagId As Integer) As ArticleTag
   Sub InsertArticleTags(ByVal id As Integer, ByVal articletags As List(Of String))
   Sub InsertTag(ByVal tagName As String)
   Sub DeleteTag(ByVal tagId As Integer)
   Sub FixArticleTags()
End Interface
