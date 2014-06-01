Imports MCC.Data
Imports Webdiyer.WebControls.Mvc
Public Class ArticleCategoryService
   Inherits CacheObject
   Implements IArticleCategoryService

   Private _categoryRepo As IArticleCategoryRepository

   Public Sub New()
      Me.New(New ArticleCategoryRepository())
   End Sub

   Public Sub New(ByVal categoryRepo As IArticleCategoryRepository)
      _categoryRepo = categoryRepo
   End Sub


   Public Function GetCategoriesCount() As Integer Implements IArticleCategoryService.GetCategoriesCount
      Dim key As String = "articleCategory_articleCategorycount"
      If Cache(key) IsNot Nothing Then
         Return CInt(Cache(key))
      Else
         Dim i As Integer = _categoryRepo.GetCategories.Count()
         CacheData(key, i)
         Return i
      End If
   End Function

   Public Function GetCategories() As List(Of ArticleCategory) Implements IArticleCategoryService.GetCategories
      Dim key As String = "articleCategory_articleCategory"
      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), List(Of ArticleCategory))
      Else
         Dim li As List(Of ArticleCategory) = _categoryRepo.GetCategories().ToList
         CacheData(key, li)
         Return li
      End If
   End Function

   Public Function GetMainCategories() As List(Of ArticleCategory) Implements IArticleCategoryService.GetMainCategories
      Dim key As String = "articleCategory_articleMainCategory"
      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), List(Of ArticleCategory))
      Else
            Dim li As List(Of ArticleCategory) = _categoryRepo.GetCategories.Where(Function(p) p.ParentCategoryID.Value <= 0).ToList
         CacheData(key, li)
         Return li
      End If
   End Function


   Public Function GetCategories(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, Optional ByVal sort As String = "Title") As PagedList(Of ArticleCategory) Implements IArticleCategoryService.GetCategories
      Dim key As String = "articleCategory_articleCategory" & startRowIndex & "_" & maximumRows & "_"
      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), PagedList(Of ArticleCategory))
      Else
         Dim li As PagedList(Of ArticleCategory) = _categoryRepo.GetCategories.SortBy(sort).ToPagedList(startRowIndex, maximumRows)
         CacheData(key, li)
         Return li
      End If
   End Function


   Public Function GetParentCategories() As List(Of ArticleCategory) Implements IArticleCategoryService.GetParentCategories
      Return GetMainCategories()
   End Function

   Public Function GetChildrenCategories(ByVal category As String) As List(Of String) Implements IArticleCategoryService.GetChildrenCategories
      Dim key As String = "articleCategory_childcategory_" & category
      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), List(Of String))
      Else
         Dim id As Integer = _categoryRepo.GetCategories.WithSlug(category.ToLower.Split(New Char() {","c}, StringSplitOptions.RemoveEmptyEntries)).Select(Function(p) p.CategoryID).FirstOrDefault

         If id <= 0 Then
            Return Nothing
         End If

            Dim li As List(Of String) = _categoryRepo.GetCategories.Where(Function(p) p.ParentCategoryID.Value = id).Select(Function(p) p.Title).ToList
         CacheData(key, li)
         Return li
      End If
   End Function

   Public Function GetCategoryById(ByVal categoryId As Integer) As ArticleCategory Implements IArticleCategoryService.GetCategoryById
      Dim key As String = "articleCategory_articleCategory_" & categoryId.ToString & "_"
      If Cache(key) IsNot Nothing Then
         Dim c As ArticleCategory = DirectCast(Cache(key), ArticleCategory)
         Return c
      Else
         Dim c As ArticleCategory = _categoryRepo.GetCategories.WithID(categoryId).FirstOrDefault()
         CacheData(key, c)
         Return c
      End If
   End Function

   Public Function GetCategoryBySlug(ByVal slug As String) As ArticleCategory Implements IArticleCategoryService.GetCategoryBySlug
      Dim key As String = "articleCategory_articleCategory_" & slug & "_"
      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), ArticleCategory)
      Else
         Dim c As ArticleCategory = _categoryRepo.GetCategories.WithSlug(slug.Split(New Char() {","c}, StringSplitOptions.RemoveEmptyEntries)).FirstOrDefault()
         CacheData(key, c)
         Return c
      End If
   End Function

   Public Function GetCategoriesByArticleId(ByVal ArticleId As Integer) As List(Of ArticleCategory) Implements IArticleCategoryService.GetCategoriesByArticleId
      Dim key As String = "Articles_Specified_Categories_" + ArticleId.ToString

      If CacheObject.Cache(key) IsNot Nothing Then
         Return DirectCast(CacheObject.Cache(key), List(Of ArticleCategory))
      Else
         Dim categories As List(Of ArticleCategory) = _categoryRepo.GetArticleCategories(ArticleId).ToList
         CacheData(key, categories)
         Return categories
      End If
   End Function

   'Public Sub InsertCategory(ByVal _artCar As ArticleCategory) Implements IArticleCategoryService.InsertCategory

   '   _artCar.Description = ConvertNullToEmptyString(_artCar.Description)
   '   _artCar.ImageUrl = ConvertNullToEmptyString(_artCar.ImageUrl)

   '   With _artCar
   '      '.AddedDate = DateTime.Now
   '      '.AddedBy = CacheObject.CurrentUserName
   '      '.Title = title
   '      '.Importance = importance
   '      '.Description = description
   '      '.ImageUrl = imageUrl
   '      .Slug = DataHelper.GetSlugFromString(_artCar.Title)
   '      '.ParentCategoryID = parentCategoryId
   '   End With

   '   _categoryRepo.InsertCategory(_artCar)
   '   CacheObject.PurgeCacheItems("articleCategory_")
   'End Sub


   'Public Sub UpdateCategory(ByVal _artCat As ArticleCategory) Implements IArticleCategoryService.UpdateCategory

   '   _artCat.Description = ConvertNullToEmptyString(_artCat.Description)
   '   _artCat.ImageUrl = ConvertNullToEmptyString(_artCat.ImageUrl)

   '   With _artCat
   '      '.Title = title
   '      '.Description = description
   '      '.Importance = importance
   '      '.ImageUrl = imageUrl
   '      .Slug = DataHelper.GetSlugFromString(_artCat.Title)
   '      '.ParentCategoryID = parentCategoryId
   '   End With

   '   _categoryRepo.UpdateCategory(_artCat)

   '   CacheObject.PurgeCacheItems("articleCategory_")
   '   CacheObject.PurgeCacheItems("articleCategory_articleCategory_")
   'End Sub


   Public Sub DeleteCategory(ByVal CategoryID As Integer) Implements IArticleCategoryService.DeleteCategory
      _categoryRepo.DeleteCategory(CategoryID)
      CacheObject.PurgeCacheItems("articleCategory_")
   End Sub

   Public Sub DeleteCategories(ByVal CategoryIDs() As Integer) Implements IArticleCategoryService.DeleteCategories
      _categoryRepo.DeleteCategories(CategoryIDs)
      CacheObject.PurgeCacheItems("articleCategory_")
      CacheObject.PurgeCacheItems("articleCategory_articleCategory_")
   End Sub


   'Public Sub SetDefaultImageUrl()
   '   For Each it As ArticleCategory In mdc.mcc_Categories
   '      If it.ImageUrl Is Nothing Then
   '         it.ImageUrl = String.Empty
   '      End If
   '   Next
   '   mdc.SubmitChanges()
   '   CacheObject.PurgeCacheItems("articleCategory_")
   'End Sub

   Protected Shared Sub CacheData(ByVal key As String, ByVal data As Object)
      If data IsNot Nothing Then
         CacheObject.Cache.Insert(key, data, Nothing, DateTime.Now.AddSeconds(3600), TimeSpan.Zero)
      End If
   End Sub

   Public Sub SaveCategory(ByVal _artCat As Data.ArticleCategory) Implements IArticleCategoryService.SaveCategory
      _artCat.Description = ConvertNullToEmptyString(_artCat.Description)
      _artCat.ImageUrl = ConvertNullToEmptyString(_artCat.ImageUrl)

      If _artCat.CategoryID > 0 Then
         With _artCat
            .Slug = _artCat.Title.ToSlug
            '.ParentCategoryID = parentCategoryId
         End With
         _categoryRepo.UpdateCategory(_artCat)
      Else
         With _artCat
            .AddedBy = CurrentUserName
            .AddedDate = DateTime.Now
            .Slug = _artCat.Title.ToSlug
         End With
         _categoryRepo.InsertCategory(_artCat)
      End If

      CacheObject.PurgeCacheItems("articleCategory_")
      CacheObject.PurgeCacheItems("articleCategory_articleCategory_")
   End Sub
End Class
