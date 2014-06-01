Public Interface IVideoCategoryRepository
   Function GetCategories() As IQueryable(Of VideoCategory)
   Function InsertCategory(ByVal cat As VideoCategory) As Integer
   Sub UpdateCategory(ByVal category As VideoCategory)
   Sub DeleteCategory(ByVal categoryId As Integer)
   Sub DeleteCategories(ByVal categoryIds() As Integer)

   Sub CategorizeVideo(ByVal videoId As Integer, ByVal categoryIds() As Integer)
   Sub RemoveVideosFromCategories(ByVal videoId As Integer, ByVal categoriesId() As Integer)
   Sub UnCategorizeVideo(ByVal videoId As Integer)
   Function GetVideoCategories(ByVal videoId As Integer) As IQueryable(Of VideoCategory)
End Interface
