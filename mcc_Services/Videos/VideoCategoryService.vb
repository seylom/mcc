Imports MCC.Data
Imports Webdiyer.WebControls.Mvc
Public Class VideoCategoryService
   Inherits CacheObject
   Implements IVideoCategoryService


   Private _categoryRepo As IVideoCategoryRepository

   Public Sub New()
      Me.New(New VideoCategoryRepository())
   End Sub

   Public Sub New(ByVal categoryRepo As IVideoCategoryRepository)
      _categoryRepo = categoryRepo
   End Sub


   Public Function GetCategoriesCount() As Integer Implements IVideoCategoryService.GetCategoriesCount
      Dim key As String = "videoCategory_videoCategorycount"
      If Cache(key) IsNot Nothing Then
         Return CInt(Cache(key))
      Else
         Dim i As Integer = _categoryRepo.GetCategories.Count()
         CacheData(key, i)
         Return i
      End If
   End Function

   Public Function GetCategories() As List(Of VideoCategory) Implements IVideoCategoryService.GetCategories
      Dim key As String = "videoCategory_videoCategory"
      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), List(Of VideoCategory))
      Else
         Dim li As List(Of VideoCategory) = _categoryRepo.GetCategories().ToList
         CacheData(key, li)
         Return li
      End If
   End Function

   Public Function GetMainCategories() As List(Of VideoCategory) Implements IVideoCategoryService.GetMainCategories
      Dim key As String = "videoCategory_videoMainCategory"
      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), List(Of VideoCategory))
      Else
            Dim li As List(Of VideoCategory) = _categoryRepo.GetCategories.Where(Function(p) p.ParentCategoryID.Value <= 0).ToList
         CacheData(key, li)
         Return li
      End If
   End Function


   Public Function GetCategories(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, Optional ByVal sort As String = "Title") As PagedList(Of VideoCategory) Implements IVideoCategoryService.GetCategories
      Dim key As String = "videoCategory_videoCategory" & startRowIndex & "_" & maximumRows & "_"
      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), PagedList(Of VideoCategory))
      Else
         Dim li As PagedList(Of VideoCategory) = _categoryRepo.GetCategories.SortBy(sort).ToPagedList(startRowIndex, maximumRows)
         CacheData(key, li)
         Return li
      End If
   End Function


   Public Function GetParentCategories() As List(Of VideoCategory) Implements IVideoCategoryService.GetParentCategories
      Return GetMainCategories()
   End Function

   Public Function GetCategoryById(ByVal categoryId As Integer) As VideoCategory Implements IVideoCategoryService.GetCategoryById
      Dim key As String = "videoCategory_videoCategory_" & categoryId.ToString & "_"
      If Cache(key) IsNot Nothing Then
         Dim c As VideoCategory = DirectCast(Cache(key), VideoCategory)
         Return c
      Else
         Dim c As VideoCategory = _categoryRepo.GetCategories.WithID(categoryId).FirstOrDefault()
         CacheData(key, c)
         Return c
      End If
   End Function

   Public Function GetCategoryBySlug(ByVal slug As String) As VideoCategory Implements IVideoCategoryService.GetCategoryBySlug
      Dim key As String = "videoCategory_videoCategory_" & slug & "_"
      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), VideoCategory)
      Else
         Dim c As VideoCategory = _categoryRepo.GetCategories.WithSlug(slug).FirstOrDefault()
         CacheData(key, c)
         Return c
      End If
   End Function

   Public Function GetCategoriesByVideoId(ByVal VideoId As Integer) As List(Of VideoCategory) Implements IVideoCategoryService.GetCategoriesByVideoId
      Dim key As String = "Videos_Specified_Categories_" + VideoId.ToString

      If CacheObject.Cache(key) IsNot Nothing Then
         Return DirectCast(CacheObject.Cache(key), List(Of VideoCategory))
      Else
         Dim categories As List(Of VideoCategory) = _categoryRepo.GetVideoCategories(VideoId).ToList
         CacheData(key, categories)
         Return categories
      End If
   End Function

   Public Sub InsertCategory(ByVal _artCar As VideoCategory) Implements IVideoCategoryService.InsertCategory

      _artCar.Description = ConvertNullToEmptyString(_artCar.Description)
      _artCar.ImageUrl = ConvertNullToEmptyString(_artCar.ImageUrl)

      With _artCar
         '.AddedDate = DateTime.Now
         '.AddedBy = CacheObject.CurrentUserName
         '.Title = title
         '.Importance = importance
         '.Description = description
         '.ImageUrl = imageUrl
         .Slug = _artCar.Title.ToSlug()
         '.ParentCategoryID = parentCategoryId
      End With

      _categoryRepo.InsertCategory(_artCar)
      CacheObject.PurgeCacheItems("videoCategory_")
   End Sub


   Public Sub UpdateCategory(ByVal _artCat As VideoCategory) Implements IVideoCategoryService.UpdateCategory

      _artCat.Description = ConvertNullToEmptyString(_artCat.Description)
      _artCat.ImageUrl = ConvertNullToEmptyString(_artCat.ImageUrl)

      With _artCat
         '.Title = title
         '.Description = description
         '.Importance = importance
         '.ImageUrl = imageUrl
         .Slug = _artCat.Title.ToSlug()
         '.ParentCategoryID = parentCategoryId
      End With

      _categoryRepo.UpdateCategory(_artCat)

      CacheObject.PurgeCacheItems("videoCategory_")
      CacheObject.PurgeCacheItems("videoCategory_videoCategory_")

   End Sub


   Public Sub DeleteCategory(ByVal CategoryID As Integer) Implements IVideoCategoryService.DeleteCategory
      _categoryRepo.DeleteCategory(CategoryID)
      CacheObject.PurgeCacheItems("videoCategory_")
   End Sub

   Public Sub DeleteCategories(ByVal CategoryIDs() As Integer) Implements IVideoCategoryService.DeleteCategories
      _categoryRepo.DeleteCategories(CategoryIDs)
      CacheObject.PurgeCacheItems("videoCategory_")
      CacheObject.PurgeCacheItems("videoCategory_videoCategory_")
   End Sub


   'Public Sub SetDefaultImageUrl()
   '   For Each it As VideoCategory In mdc.mcc_Categories
   '      If it.ImageUrl Is Nothing Then
   '         it.ImageUrl = String.Empty
   '      End If
   '   Next
   '   mdc.SubmitChanges()
   '   CacheObject.PurgeCacheItems("videoCategory_")
   'End Sub

   Protected Shared Sub CacheData(ByVal key As String, ByVal data As Object)
      If data IsNot Nothing Then
         CacheObject.Cache.Insert(key, data, Nothing, DateTime.Now.AddSeconds(3600), TimeSpan.Zero)
      End If
   End Sub

End Class
