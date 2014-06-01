Public Class VideoCategoryRepository
   Implements IVideoCategoryRepository

   Private _mdc As MCCDataContext

   Public Sub New()
      Me.New(new MCCDataContext)
   End Sub

   Public Sub New(ByVal mdc As MCCDataContext)
      _mdc = mdc
   End Sub

   Public Sub DeleteCategories(ByVal categoryIds() As Integer) Implements IVideoCategoryRepository.DeleteCategories
      If categoryIds IsNot Nothing Then
         Dim q = _mdc.mcc_Categories.Where(Function(p) categoryIds.Contains(p.CategoryID)).AsEnumerable
         If q IsNot Nothing Then
            _mdc.mcc_Categories.DeleteAllOnSubmit(q)
         End If

         ' do we need to delete the child categories???
         Dim cq = _mdc.mcc_Categories.Where(Function(p) categoryIds.Contains(p.ParentCategoryID))
         If cq IsNot Nothing Then
            For Each c As mcc_Category In cq
               c.ParentCategoryID = 0
            Next
         End If

         Dim v = _mdc.mcc_VideoCategories.Where(Function(p) categoryIds.Contains(p.CategoryId))
         If v IsNot Nothing Then
            _mdc.mcc_VideoCategories.DeleteAllOnSubmit(v)
         End If

         _mdc.SubmitChanges()
      End If
   End Sub

   Public Sub DeleteCategory(ByVal categoryId As Integer) Implements IVideoCategoryRepository.DeleteCategory
      If categoryId > 0 Then
         Dim q = _mdc.mcc_Categories.Where(Function(p) p.CategoryID = categoryId).FirstOrDefault()
         If q Is Nothing Then
            Return
         End If

         _mdc.mcc_Categories.DeleteOnSubmit(q)

         Dim v = _mdc.mcc_VideoCategories.Where(Function(p) p.CategoryId = categoryId)
         If v IsNot Nothing Then
            _mdc.mcc_VideoCategories.DeleteAllOnSubmit(v)
         End If
         _mdc.SubmitChanges()
      End If
   End Sub

   Public Function GetCategories() As System.Linq.IQueryable(Of VideoCategory) Implements IVideoCategoryRepository.GetCategories
      Dim q = (From it As mcc_Category In _mdc.mcc_Categories _
               Select New VideoCategory With { _
                           .CategoryID = it.CategoryID, _
                           .AddedDate = it.AddedDate, _
                           .AddedBy = it.AddedBy, _
                           .Description = it.Description, _
                           .Importance = it.Importance, _
                           .ParentCategoryID = it.ParentCategoryID, _
                           .Slug = it.Slug, _
                           .Title = it.Title, _
                           .ImageUrl = it.ImageUrl})
      Return q
   End Function

   Public Sub UpdateCategory(ByVal category As VideoCategory) Implements IVideoCategoryRepository.UpdateCategory
      If category IsNot Nothing Then
         Dim q = _mdc.mcc_Categories.Where(Function(p) p.CategoryID = category.CategoryID).FirstOrDefault
         If q Is Nothing Then
            Return
         End If

         With q
            .Description = category.Description
            .Importance = category.Importance
                .ParentCategoryID = If(category.ParentCategoryID, 0)
            .Slug = category.Slug
            .Title = category.Title
            .ImageUrl = category.ImageUrl
         End With

         _mdc.SubmitChanges()
      End If
   End Sub


   Public Sub CategorizeVideo(ByVal videoId As Integer, ByVal categoryIds() As Integer) Implements IVideoCategoryRepository.CategorizeVideo
      Dim process As Boolean = False
      For Each c As Integer In categoryIds
         Dim id As Integer = c
         If _mdc.mcc_CategoriesVideos.Count(Function(p) p.VideoId = videoId AndAlso p.CategoryId = id) = 0 Then
            Dim cid As New mcc_CategoriesVideo
            cid.VideoId = videoId
            cid.CategoryId = c
            _mdc.mcc_CategoriesVideos.InsertOnSubmit(cid)
            process = True
         End If
      Next

      If process Then
         _mdc.SubmitChanges()
      End If
   End Sub


   Public Sub UnCategorizeVideo(ByVal videoId As Integer) Implements IVideoCategoryRepository.UnCategorizeVideo
      Dim q = _mdc.mcc_CategoriesVideos.Where(Function(p) p.VideoId = videoId)
      If q Is Nothing Then
         Return
      End If

      _mdc.mcc_CategoriesVideos.DeleteAllOnSubmit(q)
      _mdc.SubmitChanges()
   End Sub

   Public Sub RemoveVideosFromCategories(ByVal videoId As Integer, ByVal categoryIds() As Integer) Implements IVideoCategoryRepository.RemoveVideosFromCategories
      Dim q = _mdc.mcc_CategoriesVideos.Where(Function(p) categoryIds.Contains(p.VideoId))
      If q Is Nothing Then
         Return
      End If

      _mdc.mcc_CategoriesVideos.DeleteAllOnSubmit(q)
      _mdc.SubmitChanges()
   End Sub


   Public Function InsertCategory(ByVal cat As VideoCategory) As Integer Implements IVideoCategoryRepository.InsertCategory
      If cat IsNot Nothing Then
         Dim m As New mcc_Category
         With m
            .AddedDate = cat.AddedDate
            .AddedBy = cat.AddedBy
            .Description = cat.Description
            .Importance = cat.Importance
                .ParentCategoryID = If(cat.ParentCategoryID, 0)
            .Slug = cat.Slug
            .Title = cat.Title
            .ImageUrl = cat.ImageUrl
         End With

         _mdc.mcc_Categories.InsertOnSubmit(m)
         _mdc.SubmitChanges()
      End If
   End Function


   Public Function GetVideoCategories(ByVal videoId As Integer) As IQueryable(Of VideoCategory) Implements IVideoCategoryRepository.GetVideoCategories

      Dim p = From c As mcc_CategoriesVideo In _mdc.mcc_CategoriesVideos Where c.VideoId = videoId Select c.CategoryId

      Dim q = (From it As mcc_VideoCategory In _mdc.mcc_VideoCategories Where p.Contains(it.CategoryId) _
              Select New VideoCategory With { _
                          .CategoryID = it.CategoryId, _
                          .AddedDate = it.AddedDate, _
                          .AddedBy = it.AddedBy, _
                          .Description = it.Description, _
                          .Importance = it.Importance, _
                          .ParentCategoryID = it.ParentCategoryID, _
                          .Slug = it.Slug, _
                          .Title = it.Title, _
                          .ImageUrl = it.ImageUrl})
      Return q
   End Function
End Class
