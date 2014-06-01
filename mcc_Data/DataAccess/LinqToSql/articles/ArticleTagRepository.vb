Public Class ArticleTagRepository
   Implements IArticleTagRepository

   Private _mdc As MCCDataContext


   Public Sub New()
      Me.New(New MCCDataContext)
   End Sub

   Public Sub New(ByVal dc As MCCDataContext)
      _mdc = dc
   End Sub

   Public Sub DeleteTag(ByVal tagId As Integer) Implements IArticleTagRepository.DeleteTag
      If tagId > 0 Then
         Dim q = _mdc.mcc_ArticleTags.Where(Function(t) t.TagId = tagId).FirstOrDefault
         If q IsNot Nothing Then
            _mdc.mcc_ArticleTags.DeleteOnSubmit(q)

            ' delete tags binding to articles
                Dim v = _mdc.mcc_TagsArticles.Where(Function(p) p.TagID = tagId).FirstOrDefault
            If v IsNot Nothing Then
               _mdc.mcc_TagsArticles.DeleteOnSubmit(v)
            End If
            _mdc.SubmitChanges()
         End If
      End If
   End Sub

   Public Sub DeleteTags(ByVal tagIds() As Integer) Implements IArticleTagRepository.DeleteTags
      If tagIds IsNot Nothing Then
         Dim q = _mdc.mcc_ArticleTags.Where(Function(t) tagIds.Contains(t.TagId))
         If q IsNot Nothing Then
            _mdc.mcc_ArticleTags.DeleteAllOnSubmit(q)

            ' delete tags binding to articles
            Dim v = _mdc.mcc_TagsArticles.Where(Function(p) tagIds.Contains(p.TagID))
            If v IsNot Nothing Then
                    _mdc.mcc_TagsArticles.DeleteAllOnSubmit(v)
            End If
            _mdc.SubmitChanges()
         End If
      End If
   End Sub

   Public Function GetArticleTags(ByVal articleId As Integer) As System.Linq.IQueryable(Of ArticleTag) Implements IArticleTagRepository.GetArticleTags
      If articleId > 0 Then
         Dim v = (From cp As mcc_TagsArticle In _mdc.mcc_TagsArticles Where cp.ArticleID = articleId Select cp.TagID).ToList
         Dim q = From t As mcc_ArticleTag In _mdc.mcc_ArticleTags Where v.Contains(t.TagId) _
                 Select New ArticleTag With { _
                                    .Name = t.Name, _
                                    .TagID = t.TagId, _
                                    .Slug = t.Slug}

         Return q
      End If
      Return Nothing
   End Function

   Public Function GetTags() As System.Linq.IQueryable(Of ArticleTag) Implements IArticleTagRepository.GetTags
      Dim q = From t As mcc_ArticleTag In _mdc.mcc_ArticleTags _
              Select New ArticleTag With { _
                                    .Name = t.Name, _
                                    .TagID = t.TagId, _
                                    .Slug = t.Slug}

      Return q
   End Function


   Public Function InsertTags(ByVal tags As List(Of String)) As List(Of Integer) Implements IArticleTagRepository.InsertTags
      Dim idx As New List(Of Integer)
      Dim q = _mdc.mcc_ArticleTags.Where(Function(p) tags.Contains(p.Name.ToLower)).Select(Function(p) p.Name)
      Dim tagToAdd As List(Of String) = tags.Except(q.ToArray).ToList
      If tagToAdd.Count > 0 Then
         For Each it As String In tagToAdd
            Dim m As New mcc_ArticleTag
            m.Name = it
            m.Slug = it.Replace(" ", "-")
            _mdc.mcc_ArticleTags.InsertOnSubmit(m)
            idx.Add(m.TagId)
         Next
      End If
      Return idx
   End Function


   Public Function InsertTag(ByVal tag As ArticleTag) As Integer Implements IArticleTagRepository.InsertTag
      Dim idx As Integer = -1
      If tag IsNot Nothing Then
         Dim t As New mcc_ArticleTag
         t.Name = tag.Name
         t.Slug = tag.Slug
         _mdc.mcc_ArticleTags.InsertOnSubmit(t)
         _mdc.SubmitChanges()
      End If
      Return idx
   End Function

    Public Sub TagArticle(ByVal articleId As Integer, ByVal tags As List(Of String)) Implements IArticleTagRepository.TagArticle
        If articleId > 0 AndAlso tags IsNot Nothing AndAlso tags.Count > 0 Then
            Dim tagsToAdd As List(Of String) = tags
            'Dim tagsToCreate As List(Of String) = tags
            'Dim q = _mdc.mcc_ArticleTags.Where(Function(p) tags.Contains(p.Name)).Select(Function(p) New With {.Name = p.Name, .Id = p.TagId}).ToList

            'If q IsNot Nothing Then
            '   Dim iniList As List(Of String) = q.Select(Function(p) p.Name).ToList
            '   tagsToCreate = tagsToCreate.Except(iniList)
            'End If

            'Dim existingTags As List(Of Integer) = q.Select(Function(p) p.id).ToList

            If tagsToAdd.Count > 0 Then
                InsertTags(tagsToAdd).ToList()
            End If

            Dim idx As List(Of Integer) = _mdc.mcc_ArticleTags.Where(Function(p) tags.Contains(p.Name)).Select(Function(p) p.TagId).ToList

            'idx = existingTags.Concat(idx)

            ' remove old mapping. I believe it is cheaper ...
            ' but correct me if i am wrong!
            Dim v = _mdc.mcc_TagsArticles.Where(Function(p) p.ArticleID = articleId)
            If v IsNot Nothing Then
                _mdc.mcc_TagsArticles.DeleteAllOnSubmit(v)
            End If

            If tagsToAdd.Count > 0 Then
                For Each it As Integer In idx
                    Dim m As New mcc_TagsArticle
                    m.TagID = it
                    m.ArticleID = articleId
                    _mdc.mcc_TagsArticles.InsertOnSubmit(m)
                Next

                _mdc.SubmitChanges()
            End If
        End If
    End Sub
End Class
