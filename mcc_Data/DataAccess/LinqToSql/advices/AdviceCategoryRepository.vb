Public Class AdviceCategoryRepository
   Implements IAdviceCategoryRepository

   Private _mdc As MCCDataContext

   Public Sub New()
      Me.New(New MCCDataContext)
   End Sub

   Public Sub New(ByVal mdc As MCCDataContext)
      _mdc = mdc
   End Sub

   Public Sub DeleteCategories(ByVal categoryIds() As Integer) Implements IAdviceCategoryRepository.DeleteCategories
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

         Dim v = _mdc.mcc_AdviceCategories.Where(Function(p) categoryIds.Contains(p.CategoryID))
         If v IsNot Nothing Then
            _mdc.mcc_AdviceCategories.DeleteAllOnSubmit(v)
         End If

         _mdc.SubmitChanges()
      End If
   End Sub

   Public Sub DeleteCategory(ByVal categoryId As Integer) Implements IAdviceCategoryRepository.DeleteCategory
      If categoryId > 0 Then
         Dim q = _mdc.mcc_Categories.Where(Function(p) p.CategoryID = categoryId).FirstOrDefault()
         If q Is Nothing Then
            Return
         End If

         _mdc.mcc_Categories.DeleteOnSubmit(q)

         Dim v = _mdc.mcc_AdviceCategories.Where(Function(p) p.CategoryID = categoryId)
         If v IsNot Nothing Then
            _mdc.mcc_AdviceCategories.DeleteAllOnSubmit(v)
         End If
         _mdc.SubmitChanges()
      End If
   End Sub

   Public Function GetCategories() As System.Linq.IQueryable(Of AdviceCategory) Implements IAdviceCategoryRepository.GetCategories
      Dim q = (From it As mcc_Category In _mdc.mcc_Categories _
               Select New AdviceCategory With { _
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

   Public Sub UpdateCategory(ByVal category As AdviceCategory) Implements IAdviceCategoryRepository.UpdateCategory
      If category IsNot Nothing Then
         Dim q = _mdc.mcc_Categories.Where(Function(p) p.CategoryID = category.CategoryID).FirstOrDefault
         If q Is Nothing Then
            Return
         End If

         With q
            .Description = category.Description
            .Importance = category.Importance
            .ParentCategoryID = category.ParentCategoryID
            .Slug = category.Slug
            .Title = category.Title
            .ImageUrl = category.ImageUrl
         End With

         _mdc.SubmitChanges()
      End If
   End Sub


   Public Sub CategorizeAdvice(ByVal adviceId As Integer, ByVal categoryIds() As Integer) Implements IAdviceCategoryRepository.CategorizeAdvice
      Dim process As Boolean = False
      For Each c As Integer In categoryIds
         Dim id As Integer = c
         If _mdc.mcc_CategoriesAdvices.Count(Function(p) p.AdviceID = adviceId AndAlso p.CategoryID = id) = 0 Then
            Dim cid As New mcc_CategoriesAdvice
            cid.AdviceId = adviceId
            cid.CategoryID = c
            _mdc.mcc_CategoriesAdvices.InsertOnSubmit(cid)
            process = True
         End If
      Next

      If process Then
         _mdc.SubmitChanges()
      End If
   End Sub


   Public Sub UnCategorizeAdvice(ByVal adviceId As Integer) Implements IAdviceCategoryRepository.UnCategorizeAdvice
      Dim q = _mdc.mcc_CategoriesAdvices.Where(Function(p) p.AdviceID = adviceId)
      If q Is Nothing Then
         Return
      End If

      _mdc.mcc_CategoriesAdvices.DeleteAllOnSubmit(q)
      _mdc.SubmitChanges()
   End Sub

   Public Sub RemoveAdvicesFromCategories(ByVal adviceId As Integer, ByVal categoryIds() As Integer) Implements IAdviceCategoryRepository.RemoveAdvicesFromCategories
      Dim q = _mdc.mcc_CategoriesAdvices.Where(Function(p) categoryIds.Contains(p.AdviceID))
      If q Is Nothing Then
         Return
      End If

      _mdc.mcc_CategoriesAdvices.DeleteAllOnSubmit(q)
      _mdc.SubmitChanges()
   End Sub


   Public Function InsertCategory(ByVal cat As AdviceCategory) As Integer Implements IAdviceCategoryRepository.InsertCategory
      If cat IsNot Nothing Then
         Dim m As New mcc_Category
         With m
            .AddedDate = cat.AddedDate
            .AddedBy = cat.AddedBy
            .Description = cat.Description
            .Importance = cat.Importance
            .ParentCategoryID = cat.ParentCategoryID
            .Slug = cat.Slug
            .Title = cat.Title
            .ImageUrl = cat.ImageUrl
         End With

         _mdc.mcc_Categories.InsertOnSubmit(m)
         _mdc.SubmitChanges()
      End If
   End Function


   Public Function GetAdviceCategories(ByVal adviceId As Integer) As IQueryable(Of AdviceCategory) Implements IAdviceCategoryRepository.GetAdviceCategories

      Dim p = From c As mcc_CategoriesAdvice In _mdc.mcc_CategoriesAdvices Where c.AdviceID = adviceId Select c.CategoryID

        Dim q = (From it As mcc_AdviceCategory In _mdc.mcc_AdviceCategories Where p.Contains(it.CategoryID) _
                Select New AdviceCategory With { _
                            .CategoryID = it.CategoryID, _
                            .AddedDate = it.AddedDate, _
                            .AddedBy = it.AddedBy, _
                            .Description = it.Description, _
                            .Importance = it.Importance, _
                            .ParentCategoryID = If(it.ParentCategoryID, 0), _
                            .Slug = it.Slug, _
                            .Title = it.Title, _
                            .ImageUrl = it.ImageUrl})
      Return q
   End Function
End Class
