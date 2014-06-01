Imports System.Linq

Public Class ArticleRepository
   Implements IArticleRepository

   Private _mdc As MCCDataContext


   Public Sub New()
      Me.New(New MCCDataContext)
   End Sub

   Public Sub New(ByVal dc As MCCDataContext)
      _mdc = dc
   End Sub

   Public Sub DeleteArticle(ByVal Id As Integer) Implements IArticleRepository.DeleteArticle
      If Id > 0 Then
         Dim q = _mdc.mcc_Articles.Where(Function(p) p.ArticleID = Id).FirstOrDefault()
         If q Is Nothing Then
            Return
         End If
         _mdc.mcc_Articles.DeleteOnSubmit(q)

         ' delete all related comment -
         ' not sure it is ok though, but why would we want to delete an article ...?
         ' seylom: 03/14/10
         Dim cq = _mdc.mcc_Comments.Where(Function(p) p.ArticleID = Id).AsEnumerable
         If cq IsNot Nothing Then
            _mdc.mcc_Comments.DeleteAllOnSubmit(cq)
         End If

         'delete related tags mapping
         Dim v = _mdc.mcc_TagsArticles.Where(Function(p) p.ArticleID = Id)
         If v IsNot Nothing Then
            _mdc.mcc_TagsArticles.DeleteAllOnSubmit(v)
         End If

         ' delete category mapping
         Dim w = _mdc.mcc_ArticleCategories.Where(Function(p) p.ArticleId = Id)
         If w IsNot Nothing Then
            _mdc.mcc_ArticleCategories.DeleteAllOnSubmit(w)
         End If

         _mdc.SubmitChanges()
      End If
   End Sub

   Public Sub DeleteArticles(ByVal Ids() As Integer) Implements IArticleRepository.DeleteArticles
      If Ids IsNot Nothing Then
         Dim q = _mdc.mcc_Articles.Where(Function(p) Ids.Contains(p.ArticleID)).AsEnumerable
         If q IsNot Nothing Then
            _mdc.mcc_Articles.DeleteAllOnSubmit(q)

            'delete related tags mapping
            Dim v = _mdc.mcc_TagsArticles.Where(Function(p) Ids.Contains(p.ArticleID))
            If v IsNot Nothing Then
               _mdc.mcc_TagsArticles.DeleteAllOnSubmit(v)
            End If

            ' delete category mapping
            Dim w = _mdc.mcc_ArticleCategories.Where(Function(p) Ids.Contains(p.ArticleId))
            If w IsNot Nothing Then
               _mdc.mcc_ArticleCategories.DeleteAllOnSubmit(w)
            End If

            _mdc.SubmitChanges()
         End If
      End If
   End Sub

   Public Function GetArticles() As IQueryable(Of Article) Implements IArticleRepository.GetArticles
        Dim q = From it As mcc_Article In _mdc.mcc_Articles _
                 Select New Article With { _
                                   .ArticleID = it.ArticleID, _
                                   .AddedDate = it.AddedDate, _
                                   .AddedBy = it.AddedBy, _
                                   .Title = it.Title, _
                                   .Abstract = it.Abstract, _
                                   .Body = it.Body, _
                                   .Slug = it.Slug, _
                                   .ReleaseDate = If(it.ReleaseDate, DateTime.Now), _
                                   .ExpireDate = If(it.ExpireDate, DateTime.Now.AddYears(2)), _
                                   .CommentsEnabled = it.CommentsEnabled, _
                                   .Approved = it.Approved, _
                                   .Listed = it.Listed, _
                                   .OnlyForMembers = it.OnlyForMembers, _
                                   .ImageNewsUrl = it.ImageNewsUrl, _
                                   .ImageIconUrl = it.ImageIconUrl, _
                                   .PollId = it.PollId, _
                                   .VideoID = If(it.VideoId, 0), _
                                   .Tags = it.Tags, _
                                   .Votes = it.Votes, _
                                   .ViewCount = it.ViewCount, _
                                   .TotalRating = it.TotalRating, _
                                   .Status = it.Status, _
                                   .ImageID = it.ImageID}

      Return q
   End Function

   Public Function GetArticlesQuickList() As IQueryable(Of Article) Implements IArticleRepository.GetArticlesQuickList
        Dim q = From it As mcc_Article In _mdc.mcc_Articles _
                 Select New Article With { _
                                   .ArticleID = it.ArticleID, _
                                   .AddedDate = it.AddedDate, _
                                   .AddedBy = it.AddedBy, _
                                   .Title = it.Title, _
                                   .Abstract = it.Abstract, _
                                   .Slug = it.Slug, _
                                   .ReleaseDate = If(it.ReleaseDate, DateTime.Now), _
                                   .ExpireDate = If(it.ExpireDate, DateTime.Now.AddYears(2)), _
                                   .Approved = it.Approved, _
                                   .Listed = it.Listed, _
                                   .OnlyForMembers = it.OnlyForMembers, _
                                   .ImageNewsUrl = it.ImageNewsUrl, _
                                   .ImageIconUrl = it.ImageIconUrl, _
                                   .Status = it.Status, _
                                   .ImageID = it.ImageID}

      Return q
   End Function

   Public Function GetAdvancedArticlesQuickList() As IQueryable(Of ArticleAdv) Implements IArticleRepository.GetAdvancedArticlesQuickList
      Dim q = From it As mcc_Article In _mdc.mcc_Articles Where it.Approved = True AndAlso _
                                 it.ReleaseDate <= DateTime.Now AndAlso it.ExpireDate > DateTime.Now _
              Join cp As mcc_Image In _mdc.mcc_Images On cp.ImageID Equals it.ImageID _
               Select New ArticleAdv With { _
                              .ArticleID = it.ArticleID, _
                              .Slug = it.Slug, _
                              .Abstract = it.Abstract, _
                              .Title = it.Title, _
                              .ReleaseDate = it.ReleaseDate, _
                              .ImageNewsUrl = it.ImageNewsUrl, _
                              .ImageCreditsName = cp.CreditsName, _
                              .ImageCreditsUrl = cp.CreditsUrl, _
                              .ImageID = cp.ImageID}

      Return q
   End Function



   Public Function GetArticlesInCategory(ByVal categoryId() As Integer) As IQueryable(Of Article) Implements IArticleRepository.GetArticlesInCategory

      'Dim relatedCategories As List(Of Integer) = _mdc.mcc_Categories.Where(Function(p) p.CategoryID = categoryId Or p.ParentCategoryID = categoryId).Select(Function(r) r.CategoryID).ToList()

      Dim categories As List(Of Integer) = (From ac As mcc_ArticleCategory In _mdc.mcc_ArticleCategories Where categoryId.Contains(ac.CategoryId) Select ac.ArticleId).ToList
        Dim q = From it As mcc_Article In _mdc.mcc_Articles Where categories.Contains(it.ArticleID) _
                Select New Article With { _
                                   .ArticleID = it.ArticleID, _
                                   .AddedDate = it.AddedDate, _
                                   .AddedBy = it.AddedBy, _
                                   .Title = it.Title, _
                                   .Abstract = it.Abstract, _
                                   .Body = it.Body, _
                                   .Slug = it.Slug, _
                                   .ReleaseDate = If(it.ReleaseDate, DateTime.Now), _
                                   .ExpireDate = If(it.ExpireDate, DateTime.Now.AddYears(2)), _
                                   .CommentsEnabled = it.CommentsEnabled, _
                                   .Approved = it.Approved, _
                                   .Listed = it.Listed, _
                                   .OnlyForMembers = it.OnlyForMembers, _
                                   .ImageNewsUrl = it.ImageNewsUrl, _
                                   .ImageIconUrl = it.ImageIconUrl, _
                                   .PollId = it.PollId, _
                                   .VideoID = If(it.VideoId, 0), _
                                   .Tags = it.Tags, _
                                   .Votes = it.Votes, _
                                   .ViewCount = it.ViewCount, _
                                   .TotalRating = it.TotalRating, _
                                   .Status = it.Status, _
                                   .ImageID = it.ImageID}

      Return q
   End Function


   Public Function GetArticlesInCategoryQuickList(ByVal categoryId() As Integer) As IQueryable(Of Article) Implements IArticleRepository.GetArticlesInCategoryQuickList

      Dim categories As List(Of Integer) = (From ac As mcc_ArticleCategory In _mdc.mcc_ArticleCategories Where categoryId.Contains(ac.CategoryId) Select ac.ArticleId).ToList

        Dim q = From it As mcc_Article In _mdc.mcc_Articles Where categories.Contains(it.ArticleID) _
                Select New Article With { _
                                   .ArticleID = it.ArticleID, _
                                   .AddedDate = it.AddedDate, _
                                   .AddedBy = it.AddedBy, _
                                   .Title = it.Title, _
                                   .Abstract = it.Abstract, _
                                   .Slug = it.Slug, _
                                   .ReleaseDate = If(it.ReleaseDate, DateTime.Now), _
                                   .ExpireDate = If(it.ExpireDate, DateTime.Now.AddYears(2)), _
                                   .Approved = it.Approved, _
                                   .Listed = it.Listed, _
                                   .OnlyForMembers = it.OnlyForMembers, _
                                   .ImageNewsUrl = it.ImageNewsUrl, _
                                   .ImageIconUrl = it.ImageIconUrl, _
                                    .Status = it.Status, _
                                   .ImageID = it.ImageID}

      Return q
   End Function



   Public Function InsertArticle(ByVal articleToInsert As Article) As Integer Implements IArticleRepository.InsertArticle
      If articleToInsert IsNot Nothing Then
         Dim ar As New mcc_Article
         With ar
            .ArticleID = articleToInsert.ArticleID
            .AddedDate = articleToInsert.AddedDate
            .AddedBy = articleToInsert.AddedBy
            .Title = articleToInsert.Title
            .Abstract = articleToInsert.Abstract
            .Body = articleToInsert.Body
            .Slug = articleToInsert.Slug
            .ReleaseDate = articleToInsert.ReleaseDate
            .ExpireDate = articleToInsert.ExpireDate
            .CommentsEnabled = articleToInsert.CommentsEnabled
            .Approved = articleToInsert.Approved
            .Listed = articleToInsert.Listed
            .OnlyForMembers = articleToInsert.OnlyForMembers
            .ImageNewsUrl = articleToInsert.ImageNewsUrl
            .ImageIconUrl = articleToInsert.ImageIconUrl
            .PollId = articleToInsert.PollId
            .VideoId = articleToInsert.VideoID
            .Tags = articleToInsert.Tags
            .Votes = articleToInsert.Votes
            .ViewCount = articleToInsert.ViewCount
            .TotalRating = articleToInsert.TotalRating
            .ImageID = articleToInsert.ImageID
            .Status = articleToInsert.Status
         End With

         _mdc.mcc_Articles.InsertOnSubmit(ar)
         _mdc.SubmitChanges()

         Return ar.ArticleID
      End If

      Return -1
   End Function

   Public Sub UpdateArticle(ByVal articleToUpdate As Article) Implements IArticleRepository.UpdateArticle
      Try

  
         If articleToUpdate IsNot Nothing Then
            Dim q = _mdc.mcc_Articles.Where(Function(p) p.ArticleID = articleToUpdate.ArticleID).FirstOrDefault()
            If q Is Nothing Then
               Return
            End If

            With q
               .Title = articleToUpdate.Title
               .Abstract = articleToUpdate.Abstract
               .Body = articleToUpdate.Body
               .Slug = articleToUpdate.Slug
               .ReleaseDate = articleToUpdate.ReleaseDate
               .ExpireDate = articleToUpdate.ExpireDate
               .AddedBy = articleToUpdate.AddedBy
               .CommentsEnabled = articleToUpdate.CommentsEnabled
               .Approved = articleToUpdate.Approved
               .Listed = articleToUpdate.Listed
               .OnlyForMembers = articleToUpdate.OnlyForMembers
               .ImageNewsUrl = articleToUpdate.ImageNewsUrl
               .ImageIconUrl = articleToUpdate.ImageIconUrl
               .PollId = articleToUpdate.PollId
                    .VideoId = articleToUpdate.VideoID
               .Tags = articleToUpdate.Tags
               .Votes = articleToUpdate.Votes
               .ViewCount = articleToUpdate.ViewCount
               .TotalRating = articleToUpdate.TotalRating
               .ImageID = articleToUpdate.ImageID
               .Status = articleToUpdate.Status
            End With
            _mdc.SubmitChanges(System.Data.Linq.ConflictMode.FailOnFirstConflict)

         End If
      Catch ex As Exception

      End Try
   End Sub
End Class
