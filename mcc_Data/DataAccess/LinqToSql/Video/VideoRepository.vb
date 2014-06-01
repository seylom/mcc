Public Class VideoRepository
   Implements IVideoRepository

   Private _mdc As MCCDataContext


   Public Sub New()
      Me.New(new MCCDataContext)
   End Sub

   Public Sub New(ByVal dc As MCCDataContext)
      _mdc = dc
   End Sub

   Public Sub DeleteVideo(ByVal Id As Integer) Implements IVideoRepository.DeleteVideo
      If Id > 0 Then
         Dim q = _mdc.mcc_Videos.Where(Function(p) p.VideoId = Id).FirstOrDefault()
         If q Is Nothing Then
            Return
         End If
         _mdc.mcc_Videos.DeleteOnSubmit(q)

         ' delete all related comment -
         ' not sure it is ok though, but why would we want to delete an video ...?
         ' seylom: 03/14/10
         Dim cq = _mdc.mcc_VideoComments.Where(Function(p) p.VideoID = Id).AsEnumerable
         If cq IsNot Nothing Then
            _mdc.mcc_VideoComments.DeleteAllOnSubmit(cq)
         End If

         ''delete related tags mapping
         'Dim v = _mdc.mcc_TagsVideos.Where(Function(p) p.VideoID = Id)
         'If v IsNot Nothing Then
         '   _mdc.mcc_TagsVideos.DeleteAllOnSubmit(v)
         'End If

         ' delete category mapping
         Dim w = _mdc.mcc_CategoriesVideos.Where(Function(p) p.VideoId = Id)
         If w IsNot Nothing Then
            _mdc.mcc_CategoriesVideos.DeleteAllOnSubmit(w)
         End If

         _mdc.SubmitChanges()
      End If
   End Sub

   Public Sub DeleteVideos(ByVal Ids() As Integer) Implements IVideoRepository.DeleteVideos
      If Ids IsNot Nothing Then
         Dim q = _mdc.mcc_Videos.Where(Function(p) Ids.Contains(p.VideoId)).AsEnumerable
         If q IsNot Nothing Then
            _mdc.mcc_Videos.DeleteAllOnSubmit(q)

            ''delete related tags mapping
            'Dim v = _mdc.mcc_TagsVideos.Where(Function(p) Ids.Contains(p.VideoID))
            'If v IsNot Nothing Then
            '   _mdc.mcc_TagsVideos.DeleteAllOnSubmit(v)
            'End If

            ' delete category mapping
            Dim w = _mdc.mcc_CategoriesVideos.Where(Function(p) Ids.Contains(p.VideoId))
            If w IsNot Nothing Then
               _mdc.mcc_CategoriesVideos.DeleteAllOnSubmit(w)
            End If

            _mdc.SubmitChanges()
         End If
      End If
   End Sub

   Public Function GetVideos() As IQueryable(Of Video) Implements IVideoRepository.GetVideos
      Dim q = From it As mcc_Video In _mdc.mcc_Videos _
               Select New Video With { _
                                 .VideoID = it.VideoId, _
                                 .AddedDate = it.AddedDate, _
                                 .AddedBy = it.AddedBy, _
                                 .Title = it.Title, _
                                 .Abstract = it.Abstract, _
                                 .Slug = it.Slug, _
                                 .CommentsEnabled = it.CommentsEnabled, _
                                 .Approved = it.Approved, _
                                 .Listed = it.Listed, _
                                 .OnlyForMembers = it.OnlyForMembers, _
                                 .Tags = it.Tags, _
                                 .Votes = it.Votes, _
                                 .ViewCount = it.ViewCount, _
                                 .TotalRating = it.TotalRating, _
                                 .VideoUrl = it.VideoUrl, _
                                 .Duration = it.Duration, _
                                 .Name = it.Name}

      Return q
   End Function



   Public Function GetVideosInCategory(ByVal categoryId As Integer) As IQueryable(Of Video) Implements IVideoRepository.GetVideosInCategory
      Dim categories As List(Of Integer) = (From ac As mcc_CategoriesVideo In _mdc.mcc_CategoriesVideos Where ac.CategoryId = categoryId Select ac.VideoId).ToList
      Dim q = From it As mcc_Video In _mdc.mcc_Videos Where categories.Contains(it.VideoId) _
              Select New Video With { _
                                 .VideoID = it.VideoId, _
                                 .AddedDate = it.AddedDate, _
                                 .AddedBy = it.AddedBy, _
                                 .Title = it.Title, _
                                 .Abstract = it.Abstract, _
                                 .Slug = it.Slug, _
                                 .CommentsEnabled = it.CommentsEnabled, _
                                 .Approved = it.Approved, _
                                 .Listed = it.Listed, _
                                 .OnlyForMembers = it.OnlyForMembers, _
                                 .Tags = it.Tags, _
                                 .Votes = it.Votes, _
                                 .ViewCount = it.ViewCount, _
                                 .TotalRating = it.TotalRating, _
                                  .VideoUrl = it.VideoUrl, _
                                 .Duration = it.Duration, _
                                 .Name = it.Name}

      Return q
   End Function

   Public Function InsertVideo(ByVal videoToInsert As Video) As Integer Implements IVideoRepository.InsertVideo
      If videoToInsert IsNot Nothing Then
         Dim ar As New mcc_Video
         With ar
            .VideoId = videoToInsert.VideoID
            .AddedDate = videoToInsert.AddedDate
            .AddedBy = videoToInsert.AddedBy
            .Title = videoToInsert.Title
            .Abstract = videoToInsert.Abstract
            .Slug = videoToInsert.Slug
            .CommentsEnabled = videoToInsert.CommentsEnabled
            .Approved = videoToInsert.Approved
            .Listed = videoToInsert.Listed
            .OnlyForMembers = videoToInsert.OnlyForMembers
            .VideoId = videoToInsert.VideoID
            .Tags = videoToInsert.Tags
            .Votes = If(videoToInsert.Votes, 0)
            .ViewCount = If(videoToInsert.ViewCount, 0)
            .TotalRating = videoToInsert.TotalRating
            .VideoUrl = videoToInsert.VideoUrl
                .Duration = If(videoToInsert.Duration, 0)
            .Name = videoToInsert.Name
         End With

         _mdc.mcc_Videos.InsertOnSubmit(ar)
         _mdc.SubmitChanges()

         Return ar.VideoId
      End If

      Return -1
   End Function

   Public Sub UpdateVideo(ByVal videoToUpdate As Video) Implements IVideoRepository.UpdateVideo
      If videoToUpdate IsNot Nothing Then
         Dim q = _mdc.mcc_Videos.Where(Function(p) p.VideoId = videoToUpdate.VideoID).FirstOrDefault()
         If q Is Nothing Then
            Return
         End If

         With q
            .Title = videoToUpdate.Title
            .Abstract = videoToUpdate.Abstract
            .Slug = videoToUpdate.Slug
            .CommentsEnabled = videoToUpdate.CommentsEnabled
            .Approved = videoToUpdate.Approved
            .Listed = videoToUpdate.Listed
            .OnlyForMembers = videoToUpdate.OnlyForMembers
            .VideoId = videoToUpdate.VideoID
            .Tags = videoToUpdate.Tags
                .Votes = If(videoToUpdate.Votes, 0)
                .ViewCount = If(videoToUpdate.ViewCount, 0)
            .TotalRating = videoToUpdate.TotalRating
            .VideoUrl = videoToUpdate.VideoUrl
                .Duration = If(videoToUpdate.Duration, 0)
            .Name = videoToUpdate.Name
         End With
         _mdc.SubmitChanges()

      End If
   End Sub
End Class
