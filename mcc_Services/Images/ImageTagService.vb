Imports MCC.Data

Public Class ImageTagService
   Inherits CacheObject
   Implements IImageTagService

   Private _ImageTagRepo As IImageTagRepository

   Public Sub New()
      Me.New(New ImageTagRepository())
   End Sub

   Public Sub New(ByVal ImageTagRepo As IImageTagRepository)
      _ImageTagRepo = ImageTagRepo
   End Sub
   Public Function GetImageTagsCount() As Integer Implements IImageTagService.GetImageTagsCount
      Dim key As String = "ImageTags_ImageTagscount"
      If Cache(key) IsNot Nothing Then
         Return CInt(Cache(key))
      Else

         Dim it As Integer = _ImageTagRepo.GetTags.Count
         CacheData(key, it)
         Return it

      End If
   End Function


   Protected Sub CacheData(ByVal key As String, ByVal data As Object)
      If data IsNot Nothing Then
         CacheObject.Cache.Insert(key, data, Nothing, DateTime.Now.AddSeconds(3600), TimeSpan.Zero)
      End If
   End Sub

   Public Function GetImageTags() As List(Of ImageTag) Implements IImageTagService.GetImageTags
      Dim key As String = "ImageTags_ImageTags_"
      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), List(Of ImageTag))
      Else

         Dim it As List(Of ImageTag) = _ImageTagRepo.GetTags.ToList
         CacheData(key, it)
         Return it

      End If
   End Function


   Public Function SuggestImageTags(ByVal criteria As String) As List(Of String) Implements IImageTagService.SuggestImageTags
      Dim key As String = "ImageTags_ImageTags_" & criteria & "_"
      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), List(Of String))
      Else
         Dim it As List(Of String) = _ImageTagRepo.GetTags.Where(Function(p) p.Name.Contains(criteria)).Select(Function(p) p.Name).ToList
         CacheData(key, it)
         Return it
      End If
   End Function


   Public Function GetTagById(ByVal tagId As Integer) As ImageTag Implements IImageTagService.GetTagById
      Dim key As String = "ImageTags_tag_" & tagId.ToString & "_"

      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), ImageTag)
      Else

         Dim it As ImageTag = _ImageTagRepo.GetTags.Where(Function(p) p.TagID = tagId).FirstOrDefault()
         CacheData(key, it)
         Return it
      End If
   End Function


   Public Sub InsertImageTags(ByVal id As Integer, ByVal ImageTags As List(Of String)) Implements IImageTagService.InsertImageTags

      If id > 0 AndAlso ImageTags IsNot Nothing AndAlso ImageTags.Count > 0 Then
         _ImageTagRepo.TagImage(id, ImageTags.ToArray)
      End If

      'If ImageTags IsNot Nothing AndAlso ImageTags.Count > 0 Then

      '   Dim bInserted As Boolean = False

      '   ' Delete previous image tags
      '   If mdc.mcc_TagsImages.Count(Function(p) p.ImageID = id) > 0 Then
      '      Dim atl As List(Of mcc_TagsImage) = (From k As mcc_TagsImage In mdc.mcc_TagsImages Where k.ImageID = id).ToList
      '      mdc.mcc_TagsImages.DeleteAllOnSubmit(atl)
      '      mdc.SubmitChanges()
      '   End If


      '   For Each Str As String In ImageTags
      '      Dim it As String = Str.ToLower.Trim
      '      If Not String.IsNullOrEmpty(it) Then
      '         Dim ctid As Integer = 0

      '         If mdc.ImageTags.Count(Function(p) p.Name.ToLower = it) = 0 Then
      '            Dim tg As New ImageTag
      '            tg.Name = it
      '            tg.Slug = it.Replace(" ", "_")
      '            mdc.ImageTags.InsertOnSubmit(tg)
      '            mdc.SubmitChanges()
      '            ctid = tg.TagID
      '         Else
      '            ctid = (From i As ImageTag In mdc.ImageTags Where i.Name = it Select i.TagID).Single()
      '         End If

      '         If mdc.mcc_TagsImages.Count(Function(p) p.TagID = ctid AndAlso p.ImageID = id) = 0 AndAlso ctid > 0 Then
      '            Dim ta As New mcc_TagsImage()
      '            ta.ImageID = id
      '            ta.TagID = ctid
      '            mdc.mcc_TagsImages.InsertOnSubmit(ta)
      '            bInserted = True
      '         End If
      '      End If
      '   Next

      '   If bInserted Then
      '      mdc.SubmitChanges()
      '      PurgeCacheItems("ImageTags_ImageTags")
      '   End If

      'End If
   End Sub

   Public Sub InsertTag(ByVal tagName As String) Implements IImageTagService.InsertTag
      If Not String.IsNullOrEmpty(tagName) Then

         _ImageTagRepo.InsertTags(New String() {tagName})

         'If mdc.ImageTags.Count(Function(p) p.Name.ToLower = tagName.ToLower) = 0 Then
         '   Dim tg As New ImageTag
         '   tg.Name = tagName
         '   tg.Slug = tagName.Replace(" ", "_")
         '   mdc.ImageTags.InsertOnSubmit(tg)
         '   mdc.SubmitChanges()
         '   PurgeCacheItems("ImageTags_ImageTags")
         'End If

         PurgeCacheItems("ImageTags_ImageTags")
      End If
   End Sub


   Public Sub DeleteTag(ByVal tagId As Integer) Implements IImageTagService.DeleteTag

      If tagId > 0 Then
         _ImageTagRepo.DeleteTag(tagId)
         PurgeCacheItems("ImageTags_")
      End If
      'If mdc.ImageTags.Count(Function(p) p.TagId = tagId) > 0 Then
      '   Dim tg As ImageTag = (From it As ImageTag In mdc.ImageTags Where it.TagID = tagId).Single()
      '   mdc.ImageTags.DeleteOnSubmit(tg)

      '   Dim tglst As List(Of mcc_TagsImage) = (From it As mcc_TagsImage In mdc.mcc_TagsImages Where it.TagID = tagId).ToList
      '   mdc.mcc_TagsImages.DeleteAllOnSubmit(tglst)

      '   mdc.SubmitChanges()
      '   PurgeCacheItems("ImageTags_ImageTagscount")
      '   PurgeCacheItems("ImageTags_ImageTags_")
      'End If

   End Sub


   Public Sub FixImageTags() Implements IImageTagService.FixImageTags
      '
      '    Dim ImageTags As New List(Of String)
      '    Dim li As List(Of mcc_Image) = mdc.mcc_Images.ToList()


      '    Dim tl As List(Of ImageTag) = mdc.ImageTags.ToList
      '    If tl IsNot Nothing Then
      '        mdc.ImageTags.DeleteAllOnSubmit(tl)
      '        mdc.SubmitChanges()

      '        PurgeCacheItems("ImageTags_")
      '    End If

      '    If li IsNot Nothing Then
      '        Dim fl As New List(Of ImageTag)
      '        For Each it As mcc_Image In li
      '            If Not String.IsNullOrEmpty(it.ImageTags) Then
      '                Dim tag As String = it.ImageTags
      '                Dim tar() As String = tag.Split(New Char() {",", ";"})
      '                If tar.Length > 0 Then
      '                    For Each t As String In tar
      '                        If Not String.IsNullOrEmpty(t) AndAlso Not ImageTags.Contains(t.Trim.ToLower) Then
      '                            ImageTags.Add(t.Trim.ToLower)
      '                        End If
      '                    Next
      '                End If
      '            End If
      '        Next

      '        If ImageTags.Count > 0 Then
      '            For Each it As String In ImageTags
      '                Dim nt As New ImageTag()
      '                nt.Name = it
      '                nt.Slug = it.Replace(" ", "_")
      '                fl.Add(nt)
      '            Next

      '            mdc.ImageTags.InsertAllOnSubmit(fl)
      '            mdc.SubmitChanges()
      '            PurgeCacheItems("ImageTags_")
      '        End If
      '    End If
      'End Using
   End Sub
End Class
