Imports MCC.Data
Public Class ArticleTagService
   Inherits CacheObject
   Implements IArticleTagService

   Private _articleTagRepo As IArticleTagRepository

   Public Sub New()
      Me.New(New ArticleTagRepository())
   End Sub

   Public Sub New(ByVal articleTagRepo As IArticleTagRepository)
      _articleTagRepo = articleTagRepo
   End Sub
   Public Function GetArticleTagsCount() As Integer Implements IArticleTagService.GetArticleTagsCount
      Dim key As String = "articletags_articletagscount"
      If Cache(key) IsNot Nothing Then
         Return CInt(Cache(key))
      Else

         Dim it As Integer = _articleTagRepo.GetTags.Count
         CacheData(key, it)
         Return it

      End If
   End Function


   Protected Sub CacheData(ByVal key As String, ByVal data As Object)
      If data IsNot Nothing Then
         CacheObject.Cache.Insert(key, data, Nothing, DateTime.Now.AddSeconds(3600), TimeSpan.Zero)
      End If
   End Sub

   Public Function GetArticleTags() As List(Of ArticleTag) Implements IArticleTagService.GetArticleTags
      Dim key As String = "articletags_articletags_"
      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), List(Of ArticleTag))
      Else

         Dim it As List(Of ArticleTag) = _articleTagRepo.GetTags.ToList
         CacheData(key, it)
         Return it

      End If
   End Function


   Public Function SuggestArticleTags(ByVal criteria As String) As List(Of String) Implements IArticleTagService.SuggestArticleTags
      Dim key As String = "articletags_articletags_" & criteria & "_"
      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), List(Of String))
      Else
            Dim it As List(Of String) = _articleTagRepo.GetTags.Where(Function(p) p.Name.Contains(criteria)).Select(Function(p) p.Name).ToList
         CacheData(key, it)
         Return it
      End If
   End Function


   Public Function GetTagById(ByVal tagId As Integer) As ArticleTag Implements IArticleTagService.GetTagById
      Dim key As String = "articletags_tag_" & tagId.ToString & "_"

      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), ArticleTag)
      Else

         Dim it As ArticleTag = _articleTagRepo.GetTags.Where(Function(p) p.TagID = tagId).FirstOrDefault()
         CacheData(key, it)
         Return it
      End If
   End Function


   Public Sub InsertArticleTags(ByVal id As Integer, ByVal articletags As List(Of String)) Implements IArticleTagService.InsertArticleTags

      If id > 0 AndAlso articletags IsNot Nothing AndAlso articletags.Count > 0 Then
         _articleTagRepo.TagArticle(id, articletags)
      End If

      'If articletags IsNot Nothing AndAlso articletags.Count > 0 Then

      '   Dim bInserted As Boolean = False

      '   ' Delete previous article tags
      '   If mdc.mcc_TagsArticles.Count(Function(p) p.ArticleID = id) > 0 Then
      '      Dim atl As List(Of mcc_TagsArticle) = (From k As mcc_TagsArticle In mdc.mcc_TagsArticles Where k.ArticleID = id).ToList
      '      mdc.mcc_TagsArticles.DeleteAllOnSubmit(atl)
      '      mdc.SubmitChanges()
      '   End If


      '   For Each Str As String In articletags
      '      Dim it As String = Str.ToLower.Trim
      '      If Not String.IsNullOrEmpty(it) Then
      '         Dim ctid As Integer = 0

      '         If mdc.ArticleTags.Count(Function(p) p.Name.ToLower = it) = 0 Then
      '            Dim tg As New ArticleTag
      '            tg.Name = it
      '            tg.Slug = it.Replace(" ", "_")
      '            mdc.ArticleTags.InsertOnSubmit(tg)
      '            mdc.SubmitChanges()
      '            ctid = tg.TagID
      '         Else
      '            ctid = (From i As ArticleTag In mdc.ArticleTags Where i.Name = it Select i.TagID).Single()
      '         End If

      '         If mdc.mcc_TagsArticles.Count(Function(p) p.TagID = ctid AndAlso p.ArticleID = id) = 0 AndAlso ctid > 0 Then
      '            Dim ta As New mcc_TagsArticle()
      '            ta.ArticleID = id
      '            ta.TagID = ctid
      '            mdc.mcc_TagsArticles.InsertOnSubmit(ta)
      '            bInserted = True
      '         End If
      '      End If
      '   Next

      '   If bInserted Then
      '      mdc.SubmitChanges()
      '      PurgeCacheItems("articletags_articletags")
      '   End If

      'End If
   End Sub

   Public Sub InsertTag(ByVal tagName As String) Implements IArticleTagService.InsertTag
      If Not String.IsNullOrEmpty(tagName) Then

         Dim li As New List(Of String)
         li.Add(tagName)
         _articleTagRepo.InsertTags(li)

         'If mdc.ArticleTags.Count(Function(p) p.Name.ToLower = tagName.ToLower) = 0 Then
         '   Dim tg As New ArticleTag
         '   tg.Name = tagName
         '   tg.Slug = tagName.Replace(" ", "_")
         '   mdc.ArticleTags.InsertOnSubmit(tg)
         '   mdc.SubmitChanges()
         '   PurgeCacheItems("articletags_articletags")
         'End If

         PurgeCacheItems("articletags_articletags")
      End If
   End Sub


   Public Sub DeleteTag(ByVal tagId As Integer) Implements IArticleTagService.DeleteTag

      If tagId > 0 Then
         _articleTagRepo.DeleteTag(tagId)
         PurgeCacheItems("articletags_")
      End If
      'If mdc.ArticleTags.Count(Function(p) p.TagId = tagId) > 0 Then
      '   Dim tg As ArticleTag = (From it As ArticleTag In mdc.ArticleTags Where it.TagID = tagId).Single()
      '   mdc.ArticleTags.DeleteOnSubmit(tg)

      '   Dim tglst As List(Of mcc_TagsArticle) = (From it As mcc_TagsArticle In mdc.mcc_TagsArticles Where it.TagID = tagId).ToList
      '   mdc.mcc_TagsArticles.DeleteAllOnSubmit(tglst)

      '   mdc.SubmitChanges()
      '   PurgeCacheItems("articletags_articletagscount")
      '   PurgeCacheItems("articletags_articletags_")
      'End If

   End Sub


   Public Sub FixArticleTags() Implements IArticleTagService.FixArticleTags
      '
      '    Dim articletags As New List(Of String)
      '    Dim li As List(Of mcc_Article) = mdc.mcc_Articles.ToList()


      '    Dim tl As List(Of ArticleTag) = mdc.ArticleTags.ToList
      '    If tl IsNot Nothing Then
      '        mdc.ArticleTags.DeleteAllOnSubmit(tl)
      '        mdc.SubmitChanges()

      '        PurgeCacheItems("articletags_")
      '    End If

      '    If li IsNot Nothing Then
      '        Dim fl As New List(Of ArticleTag)
      '        For Each it As mcc_Article In li
      '            If Not String.IsNullOrEmpty(it.ArticleTags) Then
      '                Dim tag As String = it.ArticleTags
      '                Dim tar() As String = tag.Split(New Char() {",", ";"})
      '                If tar.Length > 0 Then
      '                    For Each t As String In tar
      '                        If Not String.IsNullOrEmpty(t) AndAlso Not articletags.Contains(t.Trim.ToLower) Then
      '                            articletags.Add(t.Trim.ToLower)
      '                        End If
      '                    Next
      '                End If
      '            End If
      '        Next

      '        If articletags.Count > 0 Then
      '            For Each it As String In articletags
      '                Dim nt As New ArticleTag()
      '                nt.Name = it
      '                nt.Slug = it.Replace(" ", "_")
      '                fl.Add(nt)
      '            Next

      '            mdc.ArticleTags.InsertAllOnSubmit(fl)
      '            mdc.SubmitChanges()
      '            PurgeCacheItems("articletags_")
      '        End If
      '    End If
      'End Using
   End Sub
End Class
