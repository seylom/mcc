Imports MCC.Data
Imports Webdiyer.WebControls.Mvc
Public Class AdviceCategoryService
   Inherits CacheObject
   Implements IAdviceCategoryService


   Private _categoryRepo As IAdviceCategoryRepository

   Public Sub New()
      Me.New(New AdviceCategoryRepository())
   End Sub

   Public Sub New(ByVal categoryRepo As IAdviceCategoryRepository)
      _categoryRepo = categoryRepo
   End Sub


   Public Function GetCategoriesCount() As Integer Implements IAdviceCategoryService.GetCategoriesCount
      Dim key As String = "adviceCategory_adviceCategorycount"
      If Cache(key) IsNot Nothing Then
         Return CInt(Cache(key))
      Else
         Dim i As Integer = _categoryRepo.GetCategories.Count()
         CacheData(key, i)
         Return i
      End If
   End Function

   Public Function GetCategories() As List(Of AdviceCategory) Implements IAdviceCategoryService.GetCategories
      Dim key As String = "adviceCategory_adviceCategory"
      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), List(Of AdviceCategory))
      Else
         Dim li As List(Of AdviceCategory) = _categoryRepo.GetCategories().ToList
         CacheData(key, li)
         Return li
      End If
   End Function

   Public Function GetMainCategories() As List(Of AdviceCategory) Implements IAdviceCategoryService.GetMainCategories
      Dim key As String = "adviceCategory_adviceMainCategory"
      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), List(Of AdviceCategory))
      Else
         Dim li As List(Of AdviceCategory) = _categoryRepo.GetCategories.Where(Function(p) p.ParentCategoryID <= 0).ToList
         CacheData(key, li)
         Return li
      End If
   End Function


   Public Function GetCategories(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, Optional ByVal sort As String = "Title") As PagedList(Of AdviceCategory) Implements IAdviceCategoryService.GetCategories
      Dim key As String = "adviceCategory_adviceCategory" & startRowIndex & "_" & maximumRows & "_"
      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), PagedList(Of AdviceCategory))
      Else
         Dim li As PagedList(Of AdviceCategory) = _categoryRepo.GetCategories.SortBy(sort).ToPagedList(startRowIndex, maximumRows)
         CacheData(key, li)
         Return li
      End If
   End Function


   Public Function GetParentCategories() As List(Of AdviceCategory) Implements IAdviceCategoryService.GetParentCategories
      Return GetMainCategories()
   End Function

   Public Function GetCategoryById(ByVal categoryId As Integer) As AdviceCategory Implements IAdviceCategoryService.GetCategoryById
      Dim key As String = "adviceCategory_adviceCategory_" & categoryId.ToString & "_"
      If Cache(key) IsNot Nothing Then
         Dim c As AdviceCategory = DirectCast(Cache(key), AdviceCategory)
         Return c
      Else
         Dim c As AdviceCategory = _categoryRepo.GetCategories.WithID(categoryId).FirstOrDefault()
         CacheData(key, c)
         Return c
      End If
   End Function

   Public Function GetCategoryBySlug(ByVal slug As String) As AdviceCategory Implements IAdviceCategoryService.GetCategoryBySlug
      Dim key As String = "adviceCategory_adviceCategory_" & slug & "_"
      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), AdviceCategory)
      Else
         Dim c As AdviceCategory = _categoryRepo.GetCategories.WithSlug(slug).FirstOrDefault()
         CacheData(key, c)
         Return c
      End If
   End Function

   Public Function GetCategoriesByAdviceId(ByVal AdviceId As Integer) As List(Of AdviceCategory) Implements IAdviceCategoryService.GetCategoriesByAdviceId
      Dim key As String = "Advices_Specified_Categories_" + AdviceId.ToString

      If CacheObject.Cache(key) IsNot Nothing Then
         Return DirectCast(CacheObject.Cache(key), List(Of AdviceCategory))
      Else
         Dim categories As List(Of AdviceCategory) = _categoryRepo.GetAdviceCategories(AdviceId).ToList
         CacheData(key, categories)
         Return categories
      End If
   End Function

   Public Sub InsertCategory(ByVal _artCar As AdviceCategory) Implements IAdviceCategoryService.InsertCategory

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
      CacheObject.PurgeCacheItems("adviceCategory_")
   End Sub


   Public Sub UpdateCategory(ByVal _artCat As AdviceCategory) Implements IAdviceCategoryService.UpdateCategory

      _artCat.Description = ConvertNullToEmptyString(_artCat.Description)
      _artCat.ImageUrl = ConvertNullToEmptyString(_artCat.ImageUrl)

      With _artCat
         '.Title = title
         '.Description = description
         '.Importance = importance
         '.ImageUrl = imageUrl
         .Slug = _artCat.Title.ToSlug
         '.ParentCategoryID = parentCategoryId
      End With

      _categoryRepo.UpdateCategory(_artCat)

      CacheObject.PurgeCacheItems("adviceCategory_")
      CacheObject.PurgeCacheItems("adviceCategory_adviceCategory_")

   End Sub


   Public Sub DeleteCategory(ByVal CategoryID As Integer) Implements IAdviceCategoryService.DeleteCategory
      _categoryRepo.DeleteCategory(CategoryID)
      CacheObject.PurgeCacheItems("adviceCategory_")
   End Sub

   Public Sub DeleteCategories(ByVal CategoryIDs() As Integer) Implements IAdviceCategoryService.DeleteCategories
      _categoryRepo.DeleteCategories(CategoryIDs)
      CacheObject.PurgeCacheItems("adviceCategory_")
      CacheObject.PurgeCacheItems("adviceCategory_adviceCategory_")
   End Sub


   'Public Sub SetDefaultImageUrl()
   '   For Each it As AdviceCategory In mdc.mcc_Categories
   '      If it.ImageUrl Is Nothing Then
   '         it.ImageUrl = String.Empty
   '      End If
   '   Next
   '   mdc.SubmitChanges()
   '   CacheObject.PurgeCacheItems("adviceCategory_")
   'End Sub

   Protected Shared Sub CacheData(ByVal key As String, ByVal data As Object)
      If data IsNot Nothing Then
         CacheObject.Cache.Insert(key, data, Nothing, DateTime.Now.AddSeconds(3600), TimeSpan.Zero)
      End If
   End Sub


End Class
