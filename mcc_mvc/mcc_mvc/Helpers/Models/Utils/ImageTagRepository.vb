Imports Microsoft.VisualBasic
Imports System.Linq
Imports MCC.Dynamic
Imports MCC.Data

Namespace Images
   Public Class ImageTagRepository
      Inherits mccObject

      Public Shared Function GetImageTagsCount() As Integer
         Dim key As String = "imageTags_imageTagscount"
         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            Using mdc As New MCCDataContext
               Dim it As Integer = mdc.mcc_ImageTags.Count()
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

      Public Shared Function GetImageTags() As List(Of mcc_ImageTag)
         Dim key As String = "imageTags_imageTags_"
         If Cache(key) IsNot Nothing Then
            Return DirectCast(Cache(key), List(Of mcc_ImageTag))
         Else
            Using mdc As New MCCDataContext
               Dim it As List(Of mcc_ImageTag) = (From t As mcc_ImageTag In mdc.mcc_ImageTags).SortBy("Name").ToList
               CacheData(key, it)
               Return it
            End Using
         End If
      End Function

      Public Shared Function GetImageTags(ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As List(Of mcc_ImageTag)
         Dim key As String = "imageTags_imageTags_" & startRowIndex.ToString & "_" & maximumRows.ToString
         If Cache(key) IsNot Nothing Then
            Return DirectCast(Cache(key), List(Of mcc_ImageTag))
         Else
            Using mdc As New MCCDataContext
               startRowIndex = IIf(startRowIndex >= 0, startRowIndex, 0)
               maximumRows = IIf(maximumRows > 0, maximumRows, 20)
               Dim it As List(Of mcc_ImageTag) = (From t As mcc_ImageTag In mdc.mcc_ImageTags).SortBy("Name").Skip(startRowIndex).Take(maximumRows).ToList
               CacheData(key, it)
               Return it
            End Using
         End If
      End Function


      Public Shared Function SuggestImageTags(ByVal criteria As String) As List(Of String)
         Dim key As String = "imageTags_imageTags_" & criteria & "_"
         If Cache(key) IsNot Nothing Then
            Return DirectCast(Cache(key), List(Of String))
         Else
            Using mdc As New MCCDataContext
               If mdc.mcc_ImageTags.Count(Function(p) p.Name.StartsWith(criteria)) Then
                  Dim it As List(Of String) = (From i As mcc_ImageTag In mdc.mcc_ImageTags Where i.Name.StartsWith(criteria) Select i.Name).ToList
                  CacheData(key, it)
                  Return it
               Else
                  Return Nothing
               End If
            End Using
         End If
      End Function


      Public Shared Function GetImageTagById(ByVal imageTagId As Integer) As mcc_ImageTag
         Dim key As String = "imageTags_imageTag_" & imageTagId.ToString & "_"

         If Cache(key) IsNot Nothing Then
            Return DirectCast(Cache(key), mcc_ImageTag)
         Else
            Using mdc As New MCCDataContext
               If mdc.mcc_ImageTags.Count(Function(p) p.ImageTagId = imageTagId) > 0 Then
                  Dim it As mcc_ImageTag = (From i As mcc_ImageTag In mdc.mcc_ImageTags Where i.ImageTagId = imageTagId).Single()
                  CacheData(key, it)
                  Return it
               Else
                  Return Nothing
               End If
            End Using
         End If
      End Function


      Public Shared Sub InsertImageTags(ByVal imageTags As List(Of String))
         If imageTags IsNot Nothing AndAlso imageTags.Count > 0 Then
            Using mdc As New MCCDataContext
               Dim bInserted As Boolean = False
               For Each Str As String In imageTags
                  Dim it As String = Str.ToLower.Trim
                  If mdc.mcc_ImageTags.Count(Function(p) p.Name.ToLower = it) = 0 Then
                     Dim tg As New mcc_ImageTag
                     tg.Name = it
                     tg.Slug = it.Replace(" ", "_")
                     mdc.mcc_ImageTags.InsertOnSubmit(tg)
                     bInserted = True
                  End If
               Next
               If bInserted Then
                  mdc.SubmitChanges()
                  PurgeCacheItems("imageTags_imageTags")
               End If

            End Using
         End If
      End Sub

      Public Shared Sub InsertImageTag(ByVal imageTagName As String)
         If Not String.IsNullOrEmpty(imageTagName) Then
            Using mdc As New MCCDataContext
               If mdc.mcc_ImageTags.Count(Function(p) p.Name.ToLower = imageTagName.ToLower) = 0 Then
                  Dim tg As New mcc_ImageTag
                  tg.Name = imageTagName
                  tg.Slug = imageTagName.Replace(" ", "_")
                  mdc.mcc_ImageTags.InsertOnSubmit(tg)
                  mdc.SubmitChanges()
                  PurgeCacheItems("imageTags_imageTags")
               End If
            End Using
         End If
      End Sub


      Public Shared Sub DeleteImageTag(ByVal imageTagId As Integer)
         Using mdc As New MCCDataContext
            If mdc.mcc_ImageTags.Count(Function(p) p.ImageTagId = imageTagId) > 0 Then
               Dim tg As mcc_ImageTag = (From it As mcc_ImageTag In mdc.mcc_ImageTags Where it.ImageTagId = imageTagId).Single()
               mdc.mcc_ImageTags.DeleteOnSubmit(tg)
               mdc.SubmitChanges()
               PurgeCacheItems("imageTags_imageTagscount")
               PurgeCacheItems("imageTags_imageTags_")
            End If
         End Using
      End Sub


      Public Shared Sub FixImageTags()
         Using mdc As New MCCDataContext
            Dim imageTags As New List(Of String)
            Dim li As List(Of mcc_Image) = mdc.mcc_Images.ToList()


            Dim tl As List(Of mcc_ImageTag) = mdc.mcc_ImageTags.ToList

            If tl IsNot Nothing Then
               mdc.mcc_ImageTags.DeleteAllOnSubmit(tl)
               mdc.SubmitChanges()

               PurgeCacheItems("imageTags_")
            End If

            If li IsNot Nothing Then
               Dim fl As New List(Of mcc_ImageTag)
               For Each it As mcc_Image In li
                  If Not String.IsNullOrEmpty(it.Tags) Then
                     Dim imageTag As String = it.Tags
                     Dim tar() As String = imageTag.Split(New Char() {",", ";"})
                     If tar.Length > 0 Then
                        For Each t As String In tar
                           If Not String.IsNullOrEmpty(t) AndAlso Not imageTags.Contains(t.Trim.ToLower) Then
                              imageTags.Add(t.Trim.ToLower)
                           End If
                        Next
                     End If
                  End If
               Next

               If imageTags.Count > 0 Then
                  For Each it As String In imageTags
                     Dim nt As New mcc_ImageTag()
                     nt.Name = it
                     nt.Slug = it.Replace(" ", "_")
                     fl.Add(nt)
                  Next

                  mdc.mcc_ImageTags.InsertAllOnSubmit(fl)
                  mdc.SubmitChanges()
                  PurgeCacheItems("imageTags_")
               End If
            End If
         End Using
      End Sub
   End Class
End Namespace
