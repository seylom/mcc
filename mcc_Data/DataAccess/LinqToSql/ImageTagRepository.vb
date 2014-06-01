Public Class ImageTagRepository
   Implements IImageTagRepository

   Private _mdc As MCCDataContext


   Public Sub New()
      Me.New(new MCCDataContext)
   End Sub

   Public Sub New(ByVal dc As MCCDataContext)
      _mdc = dc
   End Sub

   Public Sub DeleteTag(ByVal tagId As Integer) Implements IImageTagRepository.DeleteTag
      If tagId > 0 Then
         Dim q = _mdc.mcc_ImageTags.Where(Function(t) t.ImageTagId = tagId).FirstOrDefault
         If q IsNot Nothing Then
            _mdc.mcc_ImageTags.DeleteOnSubmit(q)

            ' delete tags binding to images
                Dim v = _mdc.mcc_TagsImages.Where(Function(p) p.TagID = tagId).FirstOrDefault
            If v IsNot Nothing Then
               _mdc.mcc_TagsImages.DeleteOnSubmit(v)
            End If
            _mdc.SubmitChanges()
         End If
      End If
   End Sub

   Public Sub DeleteTags(ByVal tagIds() As Integer) Implements IImageTagRepository.DeleteTags
      If tagIds IsNot Nothing Then
         Dim q = _mdc.mcc_ImageTags.Where(Function(t) tagIds.Contains(t.ImageTagId))
         If q IsNot Nothing Then
            _mdc.mcc_ImageTags.DeleteAllOnSubmit(q)

            ' delete tags binding to images
            Dim v = _mdc.mcc_TagsImages.Where(Function(p) tagIds.Contains(p.TagID))
            If v IsNot Nothing Then
                    _mdc.mcc_TagsImages.DeleteAllOnSubmit(v)
            End If
            _mdc.SubmitChanges()
         End If
      End If
   End Sub

   Public Function Getimagetags(ByVal imageId As Integer) As System.Linq.IQueryable(Of ImageTag) Implements IImageTagRepository.GetImageTags
      If imageId > 0 Then
         Dim v = (From cp As mcc_TagsImage In _mdc.mcc_TagsImages Where cp.ImageID = imageId Select cp.TagID).ToList
         Dim q = From t As mcc_ImageTag In _mdc.mcc_ImageTags Where v.Contains(t.ImageTagId) _
                 Select New ImageTag With { _
                                    .Name = t.Name, _
                                    .TagID = t.ImageTagId, _
                                    .Slug = t.Slug}

         Return q
      End If
      Return Nothing
   End Function

   Public Function GetTags() As System.Linq.IQueryable(Of ImageTag) Implements IImageTagRepository.GetTags
      Dim q = From t As mcc_ImageTag In _mdc.mcc_ImageTags _
              Select New ImageTag With { _
                                    .Name = t.Name, _
                                    .TagID = t.ImageTagId, _
                                    .Slug = t.Slug}

      Return q
   End Function


   Public Function InsertTags(ByVal tags() As String) As Integer() Implements IImageTagRepository.InsertTags
      Dim idx As New List(Of Integer)
      Dim q = _mdc.mcc_ImageTags.Where(Function(p) tags.Contains(p.Name.ToLower)).Select(Function(p) p.Name)
        Dim tagToAdd() As String = tags.Except(q.ToArray).ToArray
      If tagToAdd.Length > 0 Then
         For Each it As String In tagToAdd
            Dim m As New mcc_ImageTag
            m.Name = it
            m.Slug = it.Replace(" ", "-")
            _mdc.mcc_ImageTags.InsertOnSubmit(m)
            idx.Add(m.ImageTagId)
         Next
      End If
      Return idx.ToArray
   End Function


   Public Function InsertTag(ByVal tag As ImageTag) As Integer Implements IImageTagRepository.InsertTag
      Dim idx As Integer = -1
      If tag IsNot Nothing Then
         Dim t As New mcc_ImageTag
         t.Name = tag.Name
         t.Slug = tag.Slug
         _mdc.mcc_ImageTags.InsertOnSubmit(t)
         _mdc.SubmitChanges()
      End If
      Return idx
   End Function

    Public Sub TagImage(ByVal imageId As Integer, ByVal tags() As String) Implements IImageTagRepository.TagImage
        If imageId > 0 AndAlso tags IsNot Nothing AndAlso tags.Length > 0 Then
            Dim tagsToAdd() As String = tags
            Dim q = _mdc.mcc_ImageTags.Where(Function(p) tags.Contains(p.Name)).Select(Function(p) p.Name)
            If q IsNot Nothing Then

                ' remove old mapping. I believe it is cheaper ...
                ' but correct me if i am wrong!
                Dim v = _mdc.mcc_TagsImages.Where(Function(p) p.ImageID = imageId)
                If v IsNot Nothing Then
                    _mdc.mcc_TagsImages.DeleteAllOnSubmit(v)
                End If

                tagsToAdd = q.ToArray.Except(tags).ToArray
                _mdc.SubmitChanges()
            End If

            If tagsToAdd.Length > 0 Then
                Dim idx() As Integer = InsertTags(tagsToAdd)
                For Each it As Integer In idx
                    Dim m As New mcc_TagsImage
                    m.TagID = it
                    m.ImageID = imageId
                    _mdc.mcc_TagsImages.InsertOnSubmit(m)
                Next

                _mdc.SubmitChanges()
            End If
        End If
    End Sub
End Class
