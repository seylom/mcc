Imports Microsoft.VisualBasic
Imports System.Linq
Imports MCC.Dynamic
Imports System.Linq.Expressions

Namespace Images
   Public Class ImageRepository
      Inherits mccObject


      Public Shared Function GetImagesCount() As Integer
         Dim key As String = "images_imagescount"
         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            Using mdc As New MCCDataContext
               Dim it As Integer = mdc.mcc_Images.Count()
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

      Public Shared Function GetImages() As List(Of mcc_Image)
         Dim key As String = "images_images_"
         If Cache(key) IsNot Nothing Then
            Return DirectCast(Cache(key), List(Of mcc_Image))
         Else
            Using mdc As New MCCDataContext
               Dim it As List(Of mcc_Image) = mdc.mcc_Images.ToList
               CacheData(key, it)
               Return it
            End Using
         End If
      End Function

      Public Shared Function GetImages(ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "title ASC") As List(Of mcc_Image)
         Dim key As String = "images_images_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Return DirectCast(Cache(key), List(Of mcc_Image))
         Else
            Using mdc As New MCCDataContext
               Dim it As List(Of mcc_Image)
               If startrowindex > 0 AndAlso maximumrows > 0 Then
                  it = mdc.mcc_Images.Skip(startrowindex).Take(maximumrows).ToList
                  CacheData(key, it)
               Else
                  it = mdc.mcc_Images.Skip(0).Take(20).ToList
                  CacheData(key, it)
               End If

               Return it
            End Using
         End If
      End Function

      'Public Shared Function SuggestImages(ByVal criteria As String) As List(Of String)
      '   Dim key As String = "images_images_" & criteria & "_"
      '   If Cache(key) IsNot Nothing Then
      '      Return DirectCast(Cache(key), List(Of String))
      '   Else
      '      Using mdc As New MCCDataContext
      '         If mdc.mcc_Images.Count(Function(p) p.Name.StartsWith(criteria)) Then
      '            Dim it As List(Of String) = (From i As mcc_Image In mdc.mcc_Images Where i.Name.StartsWith(criteria) Select i.Name).ToList
      '            CacheData(key, it)
      '            Return it
      '         Else
      '            Return Nothing
      '         End If
      '      End Using
      '   End If
      'End Function

      Public Shared Function GetImages(ByVal keys() As String) As List(Of mcc_Image)

         Array.Sort(keys)

         Dim ark As String = String.Join("_", keys)

         Dim key As String = "images_images_" & ark.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Return DirectCast(Cache(key), List(Of mcc_Image))
         Else
            Dim li As List(Of mcc_Image)
            Using mdc As New MCCDataContext

               Dim q = mcc_Image.ContainsAny(keys.ToArray)
               li = (From it As mcc_Image In mdc.mcc_Images.Where(q)).Distinct().ToList()
               CacheData(key, li)
            End Using
            Return li
         End If
      End Function



      Public Shared Function GetImageByUuid(ByVal uuid As String) As mcc_Image
         Dim key As String = "images_imageuuid_" & uuid.ToString & "_"

         If Cache(key) IsNot Nothing Then
            Return DirectCast(Cache(key), mcc_Image)
         Else
            Using mdc As New MCCDataContext
               If mdc.mcc_Images.Count(Function(p) p.uuid = uuid) > 0 Then
                  Dim it As mcc_Image = (From i As mcc_Image In mdc.mcc_Images Where i.uuid = uuid).Single()
                  CacheData(key, it)
                  Return it
               Else
                  Return Nothing
               End If
            End Using
         End If
      End Function

      Public Shared Function GetImageById(ByVal imageId As Integer) As mcc_Image
         Dim key As String = "images_images_" & imageId.ToString & "_"

         If Cache(key) IsNot Nothing Then
            Return DirectCast(Cache(key), mcc_Image)
         Else
            Using mdc As New MCCDataContext
               If mdc.mcc_Images.Count(Function(p) p.ImageID = imageId) > 0 Then
                  Dim it As mcc_Image = (From i As mcc_Image In mdc.mcc_Images Where i.ImageID = imageId).Single()
                  CacheData(key, it)
                  Return it
               Else
                  Return Nothing
               End If
            End Using
         End If
      End Function



      Public Shared Sub InsertImage(ByVal img As mcc_Image)
         Using mdc As New MCCDataContext
            If mdc.mcc_Images.Count(Function(p) p.uuid = img.uuid) = 0 Then
               mdc.mcc_Images.InsertOnSubmit(img)
               mdc.SubmitChanges()

               If Not String.IsNullOrEmpty(img.Tags) AndAlso img.Tags.Split(",").Count > 0 Then
                  ImageTagRepository.InsertImageTags(img.Tags.Split(",").ToList())
               End If

               PurgeCacheItems("images_")
            End If
         End Using
      End Sub

      Public Shared Sub UpdateImageByUUID(ByVal uuid As String, ByVal description As String, ByVal creditsName As String, ByVal creditsUrl As String, ByVal tags As String, ByVal type As Integer)
         Using mdc As New MCCDataContext
            If mdc.mcc_Images.Count(Function(p) p.uuid = uuid) > 0 Then
               Dim wrd As mcc_Image = (From t In mdc.mcc_Images _
                                           Where t.uuid = uuid _
                                           Select t).Single()

               tags = ConvertNullToEmptyString(tags)
               creditsName = ConvertNullToEmptyString(creditsName)
               creditsUrl = ConvertNullToEmptyString(creditsUrl)
               description = ConvertNullToEmptyString(description)
               If wrd IsNot Nothing Then
                  wrd.CreditsName = creditsName
                  wrd.CreditsUrl = creditsUrl
                  wrd.Tags = tags
                  wrd.Description = description
                  wrd.Type = type
                  mdc.SubmitChanges()

                  If Not String.IsNullOrEmpty(tags) AndAlso tags.Split(",").Count > 0 Then
                     ImageTagRepository.InsertImageTags(tags.Split(",").ToList())
                  End If

                  mccObject.PurgeCacheItems("images_images_" & wrd.ImageID.ToString)
                  mccObject.PurgeCacheItems("images_images_")
               End If
            End If
         End Using
      End Sub


      Public Shared Sub UpdateImage(ByVal imageId As Integer, ByVal description As String, ByVal creditsName As String, ByVal creditsUrl As String, ByVal tags As String, ByVal type As Integer)
         Dim mdc As MCCDataContext = New MCCDataContext
         If mdc.mcc_Images.Count(Function(p) p.ImageID = imageId) > 0 Then
            Dim wrd As mcc_Image = (From t In mdc.mcc_Images _
                                        Where t.ImageID = imageId _
                                        Select t).Single()

            tags = ConvertNullToEmptyString(tags)
            creditsName = ConvertNullToEmptyString(creditsName)
            creditsUrl = ConvertNullToEmptyString(creditsUrl)
            description = ConvertNullToEmptyString(routines.Encode(description))

            If wrd IsNot Nothing Then
               wrd.CreditsName = creditsName
               wrd.CreditsUrl = creditsUrl
               wrd.Tags = tags
               wrd.Description = description
               wrd.Type = type

               If Not String.IsNullOrEmpty(tags) AndAlso tags.Split(",").Count > 0 Then
                  ImageTagRepository.InsertImageTags(tags.Split(",").ToList())
               End If

               mdc.SubmitChanges()
               mccObject.PurgeCacheItems("images_images_")
               mccObject.PurgeCacheItems("images_images_" & wrd.ImageID.ToString)
            End If
         End If
      End Sub


      Public Shared Sub DeleteImage(ByVal imageId As Integer)
         Using mdc As New MCCDataContext
            If mdc.mcc_Images.Count(Function(p) p.ImageID = imageId) > 0 Then
               Dim tg As mcc_Image = (From it As mcc_Image In mdc.mcc_Images Where it.ImageID = imageId).Single()

               Dim fi As New System.IO.FileInfo(HttpContext.Current.Server.MapPath(tg.ImageUrl))
               If fi IsNot Nothing AndAlso fi.Exists Then
                  If fi.Name.IndexOf(fi.Directory.Name) > -1 Then
                     System.IO.Directory.Delete(fi.Directory.FullName, True)
                  End If
               End If

               mdc.mcc_Images.DeleteOnSubmit(tg)
               mdc.SubmitChanges()
               PurgeCacheItems("images_imagescount")
               PurgeCacheItems("images_images_")
            End If
         End Using
      End Sub

      Public Shared Sub DeleteImages(ByVal Ids() As Integer)
         Using mdc As New MCCDataContext
            If mdc.mcc_Images.Count(Function(p) Ids.Contains(p.ImageID)) > 0 Then

               Dim tg As List(Of mcc_Image) = (From it As mcc_Image In mdc.mcc_Images Where Ids.Contains(it.ImageID)).ToList

               For Each img As mcc_Image In tg
                  Dim fi As New System.IO.FileInfo(HttpContext.Current.Server.MapPath(img.ImageUrl))
                  If fi IsNot Nothing AndAlso fi.Exists Then
                     If fi.Name.IndexOf(fi.Directory.Name) > -1 Then
                        System.IO.Directory.Delete(fi.Directory.FullName, True)
                     End If
                  End If
               Next

               mdc.mcc_Images.DeleteAllOnSubmit(tg)
               mdc.SubmitChanges()
               PurgeCacheItems("images_imagescount")
               PurgeCacheItems("images_images_")
            End If
         End Using
      End Sub

      Public Shared Sub PurgeImageCache()
         PurgeCacheItems("images_")
      End Sub

   End Class
End Namespace

Partial Class mcc_Image
   Public Shared Function ContainsAny(ByVal ParamArray keywords As String()) As Expression(Of Func(Of mcc_Image, Boolean))
      Dim predicate = PredicateBuilder.[False](Of mcc_Image)()
      For Each keyword As String In keywords
         Dim temp As String = keyword
         predicate = predicate.[Or](Function(p) p.Tags.Contains(temp))
      Next
      Return predicate
   End Function
End Class
