Public Class ArticleCategoryRepository
   Implements IArticleCategoryRepository

   Private _mdc As MCCDataContext

   Public Sub New()
      Me.New(New MCCDataContext)
   End Sub

   Public Sub New(ByVal mdc As MCCDataContext)
      _mdc = mdc
   End Sub

   Public Sub DeleteCategories(ByVal categoryIds() As Integer) Implements IArticleCategoryRepository.DeleteCategories
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

         Dim v = _mdc.mcc_ArticleCategories.Where(Function(p) categoryIds.Contains(p.CategoryId))
         If v IsNot Nothing Then
            _mdc.mcc_ArticleCategories.DeleteAllOnSubmit(v)
         End If

         _mdc.SubmitChanges()
      End If
   End Sub

   Public Sub DeleteCategory(ByVal categoryId As Integer) Implements IArticleCategoryRepository.DeleteCategory
      If categoryId > 0 Then
         Dim q = _mdc.mcc_Categories.Where(Function(p) p.CategoryID = categoryId).FirstOrDefault()
         If q Is Nothing Then
            Return
         End If

         _mdc.mcc_Categories.DeleteOnSubmit(q)

         Dim v = _mdc.mcc_ArticleCategories.Where(Function(p) p.CategoryId = categoryId)
         If v IsNot Nothing Then
            _mdc.mcc_ArticleCategories.DeleteAllOnSubmit(v)
         End If
         _mdc.SubmitChanges()
      End If
   End Sub

   Public Function GetCategories() As System.Linq.IQueryable(Of ArticleCategory) Implements IArticleCategoryRepository.GetCategories
      Dim q = (From it As mcc_Category In _mdc.mcc_Categories _
               Select New ArticleCategory With { _
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

   Public Sub UpdateCategory(ByVal category As ArticleCategory) Implements IArticleCategoryRepository.UpdateCategory
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


   Public Sub CategorizeArticle(ByVal articleId As Integer, ByVal categoryIds() As Integer) Implements IArticleCategoryRepository.CategorizeArticle
      Dim process As Boolean = False

      'delete old category mapping
      UnCategorizeArticle(articleId)

      For Each c As Integer In categoryIds
         Dim id As Integer = c
         If _mdc.mcc_ArticleCategories.Count(Function(p) p.ArticleId = articleId AndAlso p.CategoryId = id) = 0 Then
            Dim cid As New mcc_ArticleCategory
            cid.ArticleId = articleId
            cid.CategoryId = c
            _mdc.mcc_ArticleCategories.InsertOnSubmit(cid)
            process = True
         End If
      Next

      If process Then
         _mdc.SubmitChanges()
      End If
   End Sub


   Public Sub UnCategorizeArticle(ByVal articleId As Integer) Implements IArticleCategoryRepository.UnCategorizeArticle
      Dim q = _mdc.mcc_ArticleCategories.Where(Function(p) p.ArticleId = articleId)
      If q Is Nothing Then
         Return
      End If

      _mdc.mcc_ArticleCategories.DeleteAllOnSubmit(q)
      _mdc.SubmitChanges()
   End Sub

   Public Sub RemoveArticlesFromCategories(ByVal articleId As Integer, ByVal categoryIds() As Integer) Implements IArticleCategoryRepository.RemoveArticlesFromCategories
      Dim q = _mdc.mcc_ArticleCategories.Where(Function(p) categoryIds.Contains(p.ArticleId))
      If q Is Nothing Then
         Return
      End If

      _mdc.mcc_ArticleCategories.DeleteAllOnSubmit(q)
      _mdc.SubmitChanges()
   End Sub


   Public Function InsertCategory(ByVal cat As ArticleCategory) As Integer Implements IArticleCategoryRepository.InsertCategory
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


   Public Function GetArticleCategories(ByVal articleId As Integer) As IQueryable(Of ArticleCategory) Implements IArticleCategoryRepository.GetArticleCategories

      Dim p = From c As mcc_ArticleCategory In _mdc.mcc_ArticleCategories Where c.ArticleId = articleId Select c.CategoryId

      Dim q = (From it As mcc_Category In _mdc.mcc_Categories Where p.Contains(it.CategoryID) _
              Select New ArticleCategory With { _
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
End Class
