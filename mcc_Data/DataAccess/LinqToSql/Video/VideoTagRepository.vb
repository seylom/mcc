Public Class VideoTagRepository
   Implements IVideoTagRepository

   Private _mdc As MCCDataContext


   Public Sub New()
      Me.New(new MCCDataContext)
   End Sub

   Public Sub New(ByVal dc As MCCDataContext)
      _mdc = dc
   End Sub

   Public Sub DeleteTag(ByVal tagId As Integer) Implements IVideoTagRepository.DeleteTag
      If tagId > 0 Then
         Dim q = _mdc.mcc_VideoTags.Where(Function(t) t.VideoTagId = tagId).FirstOrDefault
         If q IsNot Nothing Then
            _mdc.mcc_VideoTags.DeleteOnSubmit(q)

            '' delete tags binding to videos
            'Dim v = _mdc.mcc_TagsVideos.Where(Function(p) p.VideoTagId = tagId)
            'If v IsNot Nothing Then
            '   _mdc.mcc_TagsVideos.DeleteOnSubmit(v)
            'End If
            _mdc.SubmitChanges()
         End If
      End If
   End Sub

   Public Sub DeleteTags(ByVal tagIds() As Integer) Implements IVideoTagRepository.DeleteTags
      If tagIds IsNot Nothing Then
         Dim q = _mdc.mcc_VideoTags.Where(Function(t) tagIds.Contains(t.VideoTagId))
         If q IsNot Nothing Then
            _mdc.mcc_VideoTags.DeleteAllOnSubmit(q)

            ' delete tags binding to videos
            'Dim v = _mdc.mcc_TagsVideos.Where(Function(p) tagIds.Contains(p.VideoTagId))
            'If v IsNot Nothing Then
            '   _mdc.mcc_TagsVideos.DeleteOnSubmit(v)
            'End If
            _mdc.SubmitChanges()
         End If
      End If
   End Sub

   Public Function GetVideoTags(ByVal videoId As Integer) As System.Linq.IQueryable(Of VideoTag) Implements IVideoTagRepository.GetVideoTags
      'If videoId > 0 Then
      '   Dim v = (From cp As mcc_TagsVideo In _mdc.mcc_TagsVideos Where cp.VideoID = videoId Select cp.VideoTagId).ToList
      '   Dim q = From t As mcc_VideoTag In _mdc.mcc_VideoTags Where v.Contains(t.TagId) _
      '           Select New VideoTag With { _
      '                              .Name = t.Name, _
      '                              .TagID = t.TagId, _
      '                              .Slug = t.Slug}

      '   Return q
      'End If
      Return Nothing
   End Function

   Public Function GetTags() As System.Linq.IQueryable(Of VideoTag) Implements IVideoTagRepository.GetTags
      Dim q = From t As mcc_VideoTag In _mdc.mcc_VideoTags _
              Select New VideoTag With { _
                                    .Name = t.Name, _
                                    .VideoTagID = t.VideoTagId, _
                                    .Slug = t.Slug}

      Return q
   End Function


   Public Function InsertTags(ByVal tags() As String) As Integer() Implements IVideoTagRepository.InsertTags
      Dim idx As New List(Of Integer)
      Dim q = _mdc.mcc_VideoTags.Where(Function(p) tags.Contains(p.Name.ToLower)).Select(Function(p) p.Name)
        Dim tagToAdd() As String = tags.Except(q.ToArray).ToArray
      If tagToAdd.Length > 0 Then
         For Each it As String In tagToAdd
            Dim m As New mcc_VideoTag
            m.Name = it
            m.Slug = it.Replace(" ", "-")
            _mdc.mcc_VideoTags.InsertOnSubmit(m)
            idx.Add(m.VideoTagId)
         Next
      End If
      Return idx.ToArray
   End Function


   Public Function InsertTag(ByVal tag As VideoTag) As Integer Implements IVideoTagRepository.InsertTag
      Dim idx As Integer = -1
      If tag IsNot Nothing Then
         Dim t As New mcc_VideoTag
         t.Name = tag.Name
         t.Slug = tag.Slug
         _mdc.mcc_VideoTags.InsertOnSubmit(t)
         _mdc.SubmitChanges()
      End If
      Return idx
   End Function

    Public Sub TagVideo(ByVal videoId As Integer, ByVal tags() As String) Implements IVideoTagRepository.TagVideo
        If videoId > 0 AndAlso tags IsNot Nothing AndAlso tags.Length > 0 Then
            Dim tagsToAdd() As String = tags
            Dim q = _mdc.mcc_VideoTags.Where(Function(p) tags.Contains(p.Name)).Select(Function(p) p.Name)
            If q IsNot Nothing Then

                '' remove old mapping. I believe it is cheaper ...
                '' but correct me if i am wrong!
                'Dim v = _mdc.mcc_TagsVideos.Where(Function(p) p.VideoID = videoId)
                'If v IsNot Nothing Then
                '   _mdc.mcc_TagsVideos.DeleteAllOnSubmit(v)
                'End If

                tagsToAdd = q.ToArray.Except(tags.AsEnumerable).ToArray
                _mdc.SubmitChanges()
            End If

            'If tagsToAdd.Length > 0 Then
            '   Dim idx() As Integer = InsertTags(tagsToAdd)
            '   For Each it As Integer In idx
            '      Dim m As New mcc_TagsVideo
            '      m.TagID = it
            '      m.VideoID = videoId
            '      _mdc.mcc_TagsVideos.InsertOnSubmit(m)
            '   Next

            '   _mdc.SubmitChanges()
            'End If
        End If
    End Sub
End Class
