Imports Microsoft.VisualBasic
Imports System.Linq
Imports MCC.Dynamic
Imports MCC.Data

Namespace Videos
   Public Class VideoTagRepository
      Inherits mccObject

      Public Shared Function GetVideoTagsCount() As Integer
         Dim key As String = "videoTags_videoTagscount"
         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            Using mdc As New MCCDataContext
               Dim it As Integer = mdc.mcc_VideoTags.Count()
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

      Public Shared Function GetVideoTags() As List(Of mcc_VideoTag)
         Dim key As String = "videoTags_videoTags_"
         If Cache(key) IsNot Nothing Then
            Return DirectCast(Cache(key), List(Of mcc_VideoTag))
         Else
            Using mdc As New MCCDataContext
               Dim it As List(Of mcc_VideoTag) = (From t As mcc_VideoTag In mdc.mcc_VideoTags).SortBy("Name").ToList
               CacheData(key, it)
               Return it
            End Using
         End If
      End Function

      Public Shared Function GetVideoTags(ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As List(Of mcc_VideoTag)
         Dim key As String = "videoTags_videoTags_" & startRowIndex.ToString & "_" & maximumRows.ToString
         If Cache(key) IsNot Nothing Then
            Return DirectCast(Cache(key), List(Of mcc_VideoTag))
         Else
            Using mdc As New MCCDataContext
               startRowIndex = IIf(startRowIndex >= 0, startRowIndex, 0)
               maximumRows = IIf(maximumRows > 0, maximumRows, 20)
               Dim it As List(Of mcc_VideoTag) = (From t As mcc_VideoTag In mdc.mcc_VideoTags).SortBy("Name").ToList
               CacheData(key, it)
               Return it
            End Using
         End If
      End Function


      Public Shared Function SuggestVideoTags(ByVal criteria As String) As List(Of String)
         Dim key As String = "videoTags_videoTags_" & criteria & "_"
         If Cache(key) IsNot Nothing Then
            Return DirectCast(Cache(key), List(Of String))
         Else
            Using mdc As New MCCDataContext
               If mdc.mcc_VideoTags.Count(Function(p) p.Name.StartsWith(criteria)) Then
                  Dim it As List(Of String) = (From i As mcc_VideoTag In mdc.mcc_VideoTags Where i.Name.StartsWith(criteria) Select i.Name).ToList
                  CacheData(key, it)
                  Return it
               Else
                  Return Nothing
               End If
            End Using
         End If
      End Function


      Public Shared Function GetVideoTagById(ByVal videoTagId As Integer) As mcc_VideoTag
         Dim key As String = "videoTags_videoTag_" & videoTagId.ToString & "_"

         If Cache(key) IsNot Nothing Then
            Return DirectCast(Cache(key), mcc_VideoTag)
         Else
            Using mdc As New MCCDataContext
               If mdc.mcc_VideoTags.Count(Function(p) p.VideoTagId = videoTagId) > 0 Then
                  Dim it As mcc_VideoTag = (From i As mcc_VideoTag In mdc.mcc_VideoTags Where i.VideoTagId = videoTagId).Single()
                  CacheData(key, it)
                  Return it
               Else
                  Return Nothing
               End If
            End Using
         End If
      End Function


      Public Shared Sub InsertVideoTags(ByVal videoTags As List(Of String))
         If videoTags IsNot Nothing AndAlso videoTags.Count > 0 Then
            Using mdc As New MCCDataContext
               Dim bInserted As Boolean = False
               For Each Str As String In videoTags
                  Dim it As String = Str.ToLower.Trim
                  If mdc.mcc_VideoTags.Count(Function(p) p.Name.ToLower = it) = 0 Then
                     Dim tg As New mcc_VideoTag
                     tg.Name = it
                     tg.Slug = it.Replace(" ", "_")
                     mdc.mcc_VideoTags.InsertOnSubmit(tg)
                     bInserted = True
                  End If
               Next
               If bInserted Then
                  mdc.SubmitChanges()
                  PurgeCacheItems("videoTags_videoTags")
               End If

            End Using
         End If
      End Sub

      Public Shared Sub InsertVideoTag(ByVal videoTagName As String)
         If Not String.IsNullOrEmpty(videoTagName) Then
            Using mdc As New MCCDataContext
               If mdc.mcc_VideoTags.Count(Function(p) p.Name.ToLower = videoTagName.ToLower) = 0 Then
                  Dim tg As New mcc_VideoTag
                  tg.Name = videoTagName
                  tg.Slug = videoTagName.Replace(" ", "_")
                  mdc.mcc_VideoTags.InsertOnSubmit(tg)
                  mdc.SubmitChanges()
                  PurgeCacheItems("videoTags_videoTags")
               End If
            End Using
         End If
      End Sub


      Public Shared Sub DeleteVideoTag(ByVal videoTagId As Integer)
         Using mdc As New MCCDataContext
            If mdc.mcc_VideoTags.Count(Function(p) p.VideoTagId = videoTagId) > 0 Then
               Dim tg As mcc_VideoTag = (From it As mcc_VideoTag In mdc.mcc_VideoTags Where it.VideoTagId = videoTagId).Single()
               mdc.mcc_VideoTags.DeleteOnSubmit(tg)
               mdc.SubmitChanges()
               PurgeCacheItems("videoTags_videoTagscount")
               PurgeCacheItems("videoTags_videoTags_")
            End If
         End Using
      End Sub


      Public Shared Sub FixVideoTags()
         Using mdc As New MCCDataContext
            Dim videoTags As New List(Of String)
            Dim li As List(Of mcc_Video) = mdc.mcc_Videos.ToList()


            Dim tl As List(Of mcc_VideoTag) = mdc.mcc_VideoTags.ToList

            If tl IsNot Nothing Then
               mdc.mcc_VideoTags.DeleteAllOnSubmit(tl)
               mdc.SubmitChanges()

               PurgeCacheItems("videoTags_")
            End If

            If li IsNot Nothing Then
               Dim fl As New List(Of mcc_VideoTag)
               For Each it As mcc_Video In li
                  If Not String.IsNullOrEmpty(it.Tags) Then
                     Dim videoTag As String = it.Tags
                     Dim tar() As String = videoTag.Split(New Char() {",", ";"})
                     If tar.Length > 0 Then
                        For Each t As String In tar
                           If Not String.IsNullOrEmpty(t) AndAlso Not videoTags.Contains(t.Trim.ToLower) Then
                              videoTags.Add(t.Trim.ToLower)
                           End If
                        Next
                     End If
                  End If
               Next

               If videoTags.Count > 0 Then
                  For Each it As String In videoTags
                     Dim nt As New mcc_VideoTag()
                     nt.Name = it
                     nt.Slug = it.Replace(" ", "_")
                     fl.Add(nt)
                  Next

                  mdc.mcc_VideoTags.InsertAllOnSubmit(fl)
                  mdc.SubmitChanges()
                  PurgeCacheItems("videoTags_")
               End If
            End If
         End Using
      End Sub
   End Class
End Namespace

