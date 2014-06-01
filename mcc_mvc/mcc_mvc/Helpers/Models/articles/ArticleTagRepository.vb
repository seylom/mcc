Imports Microsoft.VisualBasic
Imports System.Linq
Imports MCC.Data

Namespace Articles
   Public Class ArticleTagRepository
      Inherits mccObject


      Public Shared Function GetArticleTagsCount() As Integer
         Dim key As String = "articletags_articletagscount"
         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            Using mdc As New MCCDataContext
               Dim it As Integer = mdc.mcc_ArticleTags.Count()
               CacheData(key, it)
               Return it
            End Using
         End If
      End Function


      Protected Shared Sub CacheData(ByVal key As String, ByVal data As Object)
         If data IsNot Nothing Then
            mccObject.Cache.Insert(key, data, Nothing, DateTime.Now.AddSeconds(3600), TimeSpan.Zero)
         End If
      End Sub

      Public Shared Function GetArticleTags() As List(Of mcc_ArticleTag)
         Dim key As String = "articletags_articletags_"
         If Cache(key) IsNot Nothing Then
            Return DirectCast(Cache(key), List(Of mcc_ArticleTag))
         Else
            Using mdc As New MCCDataContext
               Dim it As List(Of mcc_ArticleTag) = mdc.mcc_ArticleTags.ToList
               CacheData(key, it)
               Return it
            End Using
         End If
      End Function


      Public Shared Function SuggestArticleTags(ByVal criteria As String) As List(Of String)
         Dim key As String = "articletags_articletags_" & criteria & "_"
         If Cache(key) IsNot Nothing Then
            Return DirectCast(Cache(key), List(Of String))
         Else
            Using mdc As New MCCDataContext
               If mdc.mcc_ArticleTags.Count(Function(p) p.Name.StartsWith(criteria)) Then
                  Dim it As List(Of String) = (From i As mcc_ArticleTag In mdc.mcc_ArticleTags Where i.Name.StartsWith(criteria) Select i.Name).ToList
                  CacheData(key, it)
                  Return it
               Else
                  Return Nothing
               End If
            End Using
         End If
      End Function


      Public Shared Function GetTagById(ByVal tagId As Integer) As mcc_ArticleTag
         Dim key As String = "articletags_tag_" & tagId.ToString & "_"

         If Cache(key) IsNot Nothing Then
            Return DirectCast(Cache(key), mcc_ArticleTag)
         Else
            Using mdc As New MCCDataContext
               If mdc.mcc_ArticleTags.Count(Function(p) p.TagId = tagId) > 0 Then
                  Dim it As mcc_ArticleTag = (From i As mcc_ArticleTag In mdc.mcc_ArticleTags Where i.TagId = tagId).Single()
                  CacheData(key, it)
                  Return it
               Else
                  Return Nothing
               End If
            End Using
         End If
      End Function


      Public Shared Sub InsertArticleTags(ByVal id As Integer, ByVal articletags As List(Of String))
         If articletags IsNot Nothing AndAlso articletags.Count > 0 Then
            Using mdc As New MCCDataContext
               Dim bInserted As Boolean = False

               ' Delete previous article tags
               If mdc.mcc_TagsArticles.Count(Function(p) p.ArticleID = id) > 0 Then
                  Dim atl As List(Of mcc_TagsArticle) = (From k As mcc_TagsArticle In mdc.mcc_TagsArticles Where k.ArticleID = id).ToList
                  mdc.mcc_TagsArticles.DeleteAllOnSubmit(atl)
                  mdc.SubmitChanges()
               End If


               For Each Str As String In articletags
                  Dim it As String = Str.ToLower.Trim
                  If Not String.IsNullOrEmpty(it) Then
                     Dim ctid As Integer = 0

                     If mdc.mcc_ArticleTags.Count(Function(p) p.Name.ToLower = it) = 0 Then
                        Dim tg As New mcc_ArticleTag
                        tg.Name = it
                        tg.Slug = it.Replace(" ", "_")
                        mdc.mcc_ArticleTags.InsertOnSubmit(tg)
                        mdc.SubmitChanges()
                        ctid = tg.TagId
                     Else
                        ctid = (From i As mcc_ArticleTag In mdc.mcc_ArticleTags Where i.Name = it Select i.TagId).Single()
                     End If

                     If mdc.mcc_TagsArticles.Count(Function(p) p.TagID = ctid AndAlso p.ArticleID = id) = 0 AndAlso ctid > 0 Then
                        Dim ta As New mcc_TagsArticle()
                        ta.ArticleID = id
                        ta.TagID = ctid
                        mdc.mcc_TagsArticles.InsertOnSubmit(ta)
                        bInserted = True
                     End If
                  End If
               Next

               If bInserted Then
                  mdc.SubmitChanges()
                  PurgeCacheItems("articletags_articletags")
               End If
            End Using
         End If
      End Sub

      Public Shared Sub InsertTag(ByVal tagName As String)
         If Not String.IsNullOrEmpty(tagName) Then
            Using mdc As New MCCDataContext
               If mdc.mcc_ArticleTags.Count(Function(p) p.Name.ToLower = tagName.ToLower) = 0 Then
                  Dim tg As New mcc_ArticleTag
                  tg.Name = tagName
                  tg.Slug = tagName.Replace(" ", "_")
                  mdc.mcc_ArticleTags.InsertOnSubmit(tg)
                  mdc.SubmitChanges()
                  PurgeCacheItems("articletags_articletags")
               End If
            End Using
         End If
      End Sub


      Public Shared Sub DeleteTag(ByVal tagId As Integer)
         Using mdc As New MCCDataContext
            If mdc.mcc_ArticleTags.Count(Function(p) p.TagId = tagId) > 0 Then
               Dim tg As mcc_ArticleTag = (From it As mcc_ArticleTag In mdc.mcc_ArticleTags Where it.TagId = tagId).Single()
               mdc.mcc_ArticleTags.DeleteOnSubmit(tg)

               Dim tglst As List(Of mcc_TagsArticle) = (From it As mcc_TagsArticle In mdc.mcc_TagsArticles Where it.TagID = tagId).ToList
               mdc.mcc_TagsArticles.DeleteAllOnSubmit(tglst)

               mdc.SubmitChanges()
               PurgeCacheItems("articletags_articletagscount")
               PurgeCacheItems("articletags_articletags_")
            End If
         End Using
      End Sub


      Public Shared Sub FixArticleTags()
         'Using mdc As New MCCDataContext
         '    Dim articletags As New List(Of String)
         '    Dim li As List(Of mcc_Article) = mdc.mcc_Articles.ToList()


         '    Dim tl As List(Of mcc_ArticleTag) = mdc.mcc_ArticleTags.ToList
         '    If tl IsNot Nothing Then
         '        mdc.mcc_ArticleTags.DeleteAllOnSubmit(tl)
         '        mdc.SubmitChanges()

         '        PurgeCacheItems("articletags_")
         '    End If

         '    If li IsNot Nothing Then
         '        Dim fl As New List(Of mcc_ArticleTag)
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
         '                Dim nt As New mcc_ArticleTag()
         '                nt.Name = it
         '                nt.Slug = it.Replace(" ", "_")
         '                fl.Add(nt)
         '            Next

         '            mdc.mcc_ArticleTags.InsertAllOnSubmit(fl)
         '            mdc.SubmitChanges()
         '            PurgeCacheItems("articletags_")
         '        End If
         '    End If
         'End Using
      End Sub
   End Class
End Namespace

