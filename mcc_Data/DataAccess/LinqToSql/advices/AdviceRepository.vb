Public Class AdviceRepository
   Implements IAdviceRepository
   Private _mdc As MCCDataContext

   Public Sub New()
      Me.New(New MCCDataContext)
   End Sub

   Public Sub New(ByVal mdc As MCCDataContext)
      _mdc = mdc
   End Sub

   Public Sub DeleteAdvice(ByVal Id As Integer) Implements IAdviceRepository.DeleteAdvice
      If Id > 0 Then
         Dim q = _mdc.mcc_Advices.Where(Function(p) p.AdviceID = Id).FirstOrDefault()
         If q Is Nothing Then
            Return
         End If
         _mdc.mcc_Advices.DeleteOnSubmit(q)

         ' delete all related comment -
         ' not sure it is ok though, but why would we want to delete an advice ...?
         ' seylom: 03/14/10
         Dim cq = _mdc.mcc_AdviceComments.Where(Function(p) p.AdviceId = Id).AsEnumerable
         If cq IsNot Nothing Then
            _mdc.mcc_AdviceComments.DeleteAllOnSubmit(cq)
         End If

         ''delete related tags mapping
         'Dim v = _mdc.mcc_TagsAdvices.Where(Function(p) p.AdviceID = Id)
         'If v IsNot Nothing Then
         '   _mdc.mcc_TagsAdvices.DeleteAllOnSubmit(v)
         'End If

         ' delete category mapping
         Dim w = _mdc.mcc_CategoriesAdvices.Where(Function(p) p.AdviceID = Id)
         If w IsNot Nothing Then
            _mdc.mcc_CategoriesAdvices.DeleteAllOnSubmit(w)
         End If

         _mdc.SubmitChanges()
      End If
   End Sub

   Public Sub DeleteAdvices(ByVal Ids() As Integer) Implements IAdviceRepository.DeleteAdvices
      If Ids IsNot Nothing Then
         Dim q = _mdc.mcc_Advices.Where(Function(p) Ids.Contains(p.AdviceID)).AsEnumerable
         If q IsNot Nothing Then
            _mdc.mcc_Advices.DeleteAllOnSubmit(q)

            ''delete related tags mapping
            'Dim v = _mdc.mcc_TagsAdvices.Where(Function(p) Ids.Contains(p.AdviceID))
            'If v IsNot Nothing Then
            '   _mdc.mcc_TagsAdvices.DeleteAllOnSubmit(v)
            'End If

            ' delete category mapping
            Dim w = _mdc.mcc_CategoriesAdvices.Where(Function(p) Ids.Contains(p.AdviceID))
            If w IsNot Nothing Then
               _mdc.mcc_CategoriesAdvices.DeleteAllOnSubmit(w)
            End If

            _mdc.SubmitChanges()
         End If
      End If
   End Sub

   Public Function GetAdvices() As IQueryable(Of Advice) Implements IAdviceRepository.GetAdvices
      Dim q = From it As mcc_Advice In _mdc.mcc_Advices _
               Select New Advice With { _
                                 .AdviceID = it.AdviceID, _
                                 .AddedDate = it.AddedDate, _
                                 .AddedBy = it.AddedBy, _
                                 .Title = it.Title, _
                                 .Abstract = it.Abstract, _
                                 .Body = it.Body, _
                                 .Slug = it.Slug, _
                                 .CommentsEnabled = it.CommentsEnabled, _
                                 .Approved = it.Approved, _
                                 .Listed = it.Listed, _
                                 .OnlyForMembers = it.OnlyForMembers, _
                                 .Tags = it.Tags, _
                                 .Votes = it.Votes, _
                                 .VoteUp = If(it.VoteUp, 0), _
                                 .VoteDown = If(it.VoteDown, 0), _
                                 .ViewCount = it.ViewCount, _
                                 .TotalRating = it.TotalRating, _
                                 .Status = it.Status}

      Return q
   End Function

   Public Function GetAdvicesQuickList() As IQueryable(Of Advice) Implements IAdviceRepository.GetAdvicesQuickList
      Dim q = From it As mcc_Advice In _mdc.mcc_Advices _
               Select New Advice With { _
                                 .AdviceID = it.AdviceID, _
                                 .AddedDate = it.AddedDate, _
                                 .AddedBy = it.AddedBy, _
                                 .Title = it.Title, _
                                 .Abstract = it.Abstract, _
                                 .Slug = it.Slug, _
                                 .Approved = it.Approved, _
                                 .Listed = it.Listed, _
                                 .OnlyForMembers = it.OnlyForMembers, _
                                 .Status = it.Status}

      Return q
   End Function


   Public Function GetAdvicesInCategory(ByVal categoryId As Integer) As IQueryable(Of Advice) Implements IAdviceRepository.GetAdvicesInCategory
      Dim categories As List(Of Integer) = (From ac As mcc_CategoriesAdvice In _mdc.mcc_CategoriesAdvices Where ac.CategoryID = categoryId Select ac.AdviceID).ToList
      Dim q = From it As mcc_Advice In _mdc.mcc_Advices Where categories.Contains(it.AdviceID) _
              Select New Advice With { _
                                 .AdviceID = it.AdviceID, _
                                 .AddedDate = it.AddedDate, _
                                 .AddedBy = it.AddedBy, _
                                 .Title = it.Title, _
                                 .Abstract = it.Abstract, _
                                 .Body = it.Body, _
                                 .Slug = it.Slug, _
                                 .CommentsEnabled = it.CommentsEnabled, _
                                 .Approved = it.Approved, _
                                 .Listed = it.Listed, _
                                 .OnlyForMembers = it.OnlyForMembers, _
                                 .Tags = it.Tags, _
                                 .Votes = it.Votes, _
                                 .VoteUp = If(it.VoteUp, 0), _
                                 .VoteDown = If(it.VoteDown, 0), _
                                 .ViewCount = it.ViewCount, _
                                 .TotalRating = it.TotalRating, _
                                  .Status = it.Status}

      Return q
   End Function


   Public Function GetAdvicesInCategoryQuickList(ByVal categoryId As Integer) As IQueryable(Of Advice) Implements IAdviceRepository.GetAdvicesInCategoryQuickList
      Dim categories As List(Of Integer) = (From ac As mcc_CategoriesAdvice In _mdc.mcc_CategoriesAdvices Where ac.CategoryID = categoryId Select ac.AdviceID).ToList
      Dim q = From it As mcc_Advice In _mdc.mcc_Advices Where categories.Contains(it.AdviceID) _
              Select New Advice With { _
                                 .AdviceID = it.AdviceID, _
                                 .AddedDate = it.AddedDate, _
                                 .AddedBy = it.AddedBy, _
                                 .Title = it.Title, _
                                 .Abstract = it.Abstract, _
                                 .Slug = it.Slug, _
                                 .Approved = it.Approved, _
                                 .Listed = it.Listed, _
                                 .OnlyForMembers = it.OnlyForMembers, _
                                  .Status = it.Status}

      Return q
   End Function



   Public Function InsertAdvice(ByVal adviceToInsert As Advice) As Integer Implements IAdviceRepository.InsertAdvice
      If adviceToInsert IsNot Nothing Then
         Dim ar As New mcc_Advice
         With ar
            .AdviceID = adviceToInsert.AdviceID
            .AddedDate = adviceToInsert.AddedDate
            .AddedBy = adviceToInsert.AddedBy
            .Title = adviceToInsert.Title
            .Abstract = adviceToInsert.Abstract
            .Body = adviceToInsert.Body
            .Slug = adviceToInsert.Slug
            .CommentsEnabled = adviceToInsert.CommentsEnabled
            .Approved = adviceToInsert.Approved
            .Listed = adviceToInsert.Listed
            .OnlyForMembers = adviceToInsert.OnlyForMembers
            .Tags = adviceToInsert.Tags
            .Votes = adviceToInsert.Votes
            .VoteUp = 0
            .VoteDown = 0
            .ViewCount = adviceToInsert.ViewCount
            .TotalRating = adviceToInsert.TotalRating
            .Status = adviceToInsert.Status
         End With

         _mdc.mcc_Advices.InsertOnSubmit(ar)
         _mdc.SubmitChanges()

         Return ar.AdviceID
      End If

      Return -1
   End Function

   Public Sub UpdateAdvice(ByVal adviceToUpdate As Advice) Implements IAdviceRepository.UpdateAdvice
      If adviceToUpdate IsNot Nothing Then
         Dim q = _mdc.mcc_Advices.Where(Function(p) p.AdviceID = adviceToUpdate.AdviceID).FirstOrDefault()
         If q Is Nothing Then
            Return
         End If

         With q
            .Title = adviceToUpdate.Title
            .Abstract = adviceToUpdate.Abstract
            .Body = adviceToUpdate.Body
            .Slug = adviceToUpdate.Slug
            .CommentsEnabled = adviceToUpdate.CommentsEnabled
            .Approved = adviceToUpdate.Approved
            .Listed = adviceToUpdate.Listed
            .OnlyForMembers = adviceToUpdate.OnlyForMembers
            .Tags = adviceToUpdate.Tags
            .Votes = adviceToUpdate.Votes
            .ViewCount = adviceToUpdate.ViewCount
            .TotalRating = adviceToUpdate.TotalRating
            .Status = adviceToUpdate.Status
            .VoteUp = adviceToUpdate.VoteUp
            .VoteDown = adviceToUpdate.VoteDown
         End With
         _mdc.SubmitChanges()

      End If
   End Sub
End Class
