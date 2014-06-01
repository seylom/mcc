Imports MCC.Data

Public Interface IImageTagService
   Function GetImageTagsCount() As Integer
   Function GetImageTags() As List(Of ImageTag)
   Function SuggestImageTags(ByVal criteria As String) As List(Of String)
   Function GetTagById(ByVal tagId As Integer) As ImageTag
   Sub InsertImageTags(ByVal id As Integer, ByVal imagetags As List(Of String))
   Sub InsertTag(ByVal tagName As String)
   Sub DeleteTag(ByVal tagId As Integer)
   Sub FixImageTags()
End Interface
