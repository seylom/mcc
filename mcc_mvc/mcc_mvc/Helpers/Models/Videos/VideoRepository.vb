Imports System
Imports System.Data
Imports System.Configuration
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports System.Collections.Generic
Imports MCC.SiteLayers
Imports MCC.routines
Imports System.Linq
Imports MCC.Dynamic
Imports System.Linq.Expressions
Imports System.IO
Imports MCC.Data

Namespace Videos
   Public Class VideoRepository
      Inherits mccObject
      Public Shared Function AverageRating(ByVal VideoId As Integer) As Double

         Dim key As String = String.Format("videos_video_average_{0}", VideoId.ToString)
         If Cache(key) Then
            Return CInt(Cache(key))
         Else
            Dim val As Double = 0
            Dim mdc As MCCDataContext = New MCCDataContext
            Dim wrd As mcc_Video = (From t In mdc.mcc_Videos _
                                        Where t.VideoId = VideoId _
                                        Select t).Single()
            If wrd IsNot Nothing Then
               If wrd.Votes >= 1 Then
                  val = (CDbl(wrd.TotalRating) / CDbl(wrd.Votes))
               End If
            End If
            CacheData(key, val)
            Return val
         End If
      End Function

      Public Shared Function Published(ByVal VideoId As Integer) As Boolean

         Dim key As String = String.Format("videos_video_ispublished_{0}", VideoId.ToString)
         If Cache(key) IsNot Nothing Then
            Return CBool(Cache(key))
         Else
            Dim bApproved As Boolean = False
            Dim mdc As MCCDataContext = New MCCDataContext
            Dim wrd As mcc_Video = (From t In mdc.mcc_Videos _
                                        Where t.VideoId = VideoId _
                                        Select t).Single()
            If wrd IsNot Nothing Then
               bApproved = (wrd.Approved)
            End If

            CacheData(key, bApproved)
            Return bApproved
         End If
      End Function


      Public Shared Function GetVideoCount() As Integer
         Dim key As String = "Videos_VideoCount_"

         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            Dim mdc As New MCCDataContext()
            Dim it As Integer = mdc.mcc_Videos.Count()
            CacheData(key, it)
            Return it
         End If
      End Function

      Public Shared Function GetVideoCount(ByVal publishedOnly As Boolean) As Integer
         If Not publishedOnly Then
            Return GetVideoCount()
         End If

         Dim key As String = "Videos_VideoCount_" & publishedOnly.ToString

         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            Dim mdc As New MCCDataContext()
            Dim it As Integer = mdc.mcc_Videos.Count(Function(p) p.Approved = True)
            CacheData(key, it)
            Return it
         End If
      End Function


      Public Shared Function GetVideoCountByAuthor(ByVal addedBy As String) As Integer
         If Not String.IsNullOrEmpty(addedBy) Then
            Dim key As String = "Videos_VideoCount_" & addedBy & "_"

            If Cache(key) IsNot Nothing Then
               Return CInt(Cache(key))
            Else
               Dim mdc As New MCCDataContext()
               Dim it As Integer = mdc.mcc_Videos.Count(Function(p) p.AddedBy = addedBy)
               CacheData(key, it)
               Return it
            End If
         Else
            Return 0
         End If
      End Function


      Public Shared Function GetVideoCount(ByVal publishedOnly As Boolean, ByVal categoryID As Integer) As Integer
         If Not publishedOnly Then
            Return GetVideoCount(categoryID)
         End If

         If categoryID <= 0 Then
            Return GetVideoCount(publishedOnly)
         End If

         Dim key As String = "Videos_VideoCount_" + publishedOnly.ToString() + "_" + categoryID.ToString()
         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            Dim mdc As New MCCDataContext()
            Dim catAr As List(Of Integer) = (From ef As mcc_CategoriesVideo In mdc.mcc_CategoriesVideos Where ef.CategoryId = categoryID Select ef.VideoId).ToList

            Dim it As Integer = mdc.mcc_Videos.Count(Function(p) catAr.Contains(p.VideoId) AndAlso p.Approved = True)

            CacheData(key, it)
            Return it
         End If
      End Function

      Public Shared Function GetVideos() As List(Of mcc_Video)
         Dim key As String = "Videos_Videos_"
         If Cache(key) IsNot Nothing Then
            Dim li As List(Of mcc_Video) = DirectCast(Cache(key), List(Of mcc_Video))
            Return li
         Else
            Dim mdc As New MCCDataContext()
            Dim li As List(Of mcc_Video) = mdc.mcc_Videos.ToList
            CacheData(key, li)
            Return li
         End If
      End Function

      Public Shared Function GetVideos(ByVal publishedOnly As Boolean, ByVal categoryId As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "Title") As List(Of mcc_Video)
         If Not publishedOnly Then
            Return GetVideos(categoryId, startrowindex, maximumrows, sortExp)
         End If

         If categoryId > 0 Then
            If String.IsNullOrEmpty(sortExp) Then
               sortExp = "Title DESC"
            End If
            Dim key As String = "Videos_Videos_" & categoryId.ToString & "_" & sortExp.Replace(" ", "") & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
            If Cache(key) IsNot Nothing Then
               Dim li As List(Of mcc_Video) = DirectCast(Cache(key), List(Of mcc_Video))
               Return li
            Else
               Dim mdc As New MCCDataContext()
               Dim li As List(Of mcc_Video)

               Dim catAr As List(Of Integer) = (From ef As mcc_CategoriesVideo In mdc.mcc_CategoriesVideos Where ef.CategoryId = categoryId Select ef.VideoId).ToList
               li = (From it As mcc_Video In mdc.mcc_Videos Where catAr.Contains(it.VideoId) AndAlso it.Approved = True).SortBy(sortExp).Skip(startrowindex).Take(maximumrows).ToList
               CacheData(key, li)
               Return li
            End If
         Else
            Return GetVideos(publishedOnly, startrowindex, maximumrows, sortExp)
         End If
      End Function

      Public Shared Function GetVideos(ByVal categoryId As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "Title") As List(Of mcc_Video)
         If categoryId > 0 Then
            Dim key As String = "Videos_Videos_" & categoryId.ToString & "_" & sortExp.Replace(" ", "") & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
            If String.IsNullOrEmpty(sortExp) Then
               sortExp = "Title DESC"
            End If
            If Cache(key) IsNot Nothing Then
               Dim li As List(Of mcc_Video) = DirectCast(Cache(key), List(Of mcc_Video))
               Return li
            Else
               Dim mdc As New MCCDataContext()
               Dim li As List(Of mcc_Video)

               Dim catAr As List(Of Integer) = (From ef As mcc_CategoriesVideo In mdc.mcc_CategoriesVideos Where ef.CategoryId = categoryId Select ef.VideoId).ToList

               li = (From it As mcc_Video In mdc.mcc_Videos Where catAr.Contains(it.VideoId)).SortBy(sortExp).Skip(startrowindex).Take(maximumrows).ToList
               CacheData(key, li)
               Return li
            End If
         Else
            Return GetVideos(startrowindex, maximumrows, sortExp)
         End If
      End Function

      Public Shared Function GetVideosByAuthor(ByVal addedBy As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedby") As List(Of mcc_Video)
         If Not String.IsNullOrEmpty(addedBy) Then
            If String.IsNullOrEmpty(sortExp) Then
               sortExp = "Title DESC"
            End If
            Dim key As String = "Videos_Videos_" & addedBy & "_" & sortExp.Replace(" ", "") & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
            If Cache(key) IsNot Nothing Then
               Dim li As List(Of mcc_Video) = DirectCast(Cache(key), List(Of mcc_Video))
               Return li
            Else
               Dim mdc As New MCCDataContext()
               Dim li As List(Of mcc_Video)
               li = (From it As mcc_Video In mdc.mcc_Videos Where it.AddedBy = addedBy).SortBy(sortExp).Skip(startrowindex).Take(maximumrows).ToList
               CacheData(key, li)
               Return li
            End If
         Else
            Return GetVideos(startrowindex, maximumrows, sortExp)
         End If
      End Function

      Public Shared Function GetVideosByAuthor(ByVal publishedOnly As Boolean, ByVal addedBy As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedby") As List(Of mcc_Video)

         If Not publishedOnly Then
            Return GetVideosByAuthor(addedBy, startrowindex, maximumrows, sortExp)
         End If

         If Not String.IsNullOrEmpty(addedBy) Then
            If String.IsNullOrEmpty(sortExp) Then
               sortExp = "Title DESC"
            End If
            Dim key As String = "Videos_Videos_" & addedBy & "_" & publishedOnly.ToString & "_" & sortExp.Replace(" ", "") & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
            If Cache(key) IsNot Nothing Then
               Dim li As List(Of mcc_Video) = DirectCast(Cache(key), List(Of mcc_Video))
               Return li
            Else
               Dim mdc As New MCCDataContext()
               Dim li As List(Of mcc_Video)
               li = (From it As mcc_Video In mdc.mcc_Videos Where it.AddedBy = addedBy AndAlso it.Approved = True).SortBy(sortExp).Skip(startrowindex).Take(maximumrows).ToList
               CacheData(key, li)
               Return li
            End If
         Else
            Return GetVideos(startrowindex, maximumrows, sortExp)
         End If
      End Function

      Public Shared Function GetVideos(ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "Title DESC") As List(Of mcc_Video)
         Dim key As String = "Videos_Videos_" & startrowindex.ToString & "_" & sortExp.Replace(" ", "") & "_" & maximumrows.ToString & "_"
         If String.IsNullOrEmpty(sortExp) Then
            sortExp = "Title DESC"
         End If
         If Cache(key) IsNot Nothing Then
            Dim li As List(Of mcc_Video) = DirectCast(Cache(key), List(Of mcc_Video))
            Return li
         Else
            Dim mdc As New MCCDataContext()

            Dim li As New List(Of mcc_Video)
            Dim i As Integer = mdc.mcc_Videos.Count()
            If i > 0 Then
               li = mdc.mcc_Videos.SortBy(sortExp).Skip(startrowindex).Take(maximumrows).ToList
            End If
            CacheData(key, li)
            Return li
         End If
      End Function

      Public Shared Function GetVideos(ByVal publishedOnly As Boolean, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "Title DESC") As List(Of mcc_Video)

         If publishedOnly Then
            If String.IsNullOrEmpty(sortExp) Then
               sortExp = "AddedDate DESC"
            End If
            Dim key As String = "Videos_Videos_" & publishedOnly.ToString & "_" & sortExp.Replace(" ", "") & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
            If Cache(key) IsNot Nothing Then
               Dim li As List(Of mcc_Video) = DirectCast(Cache(key), List(Of mcc_Video))
               Return li
            Else
               Dim mdc As New MCCDataContext()

               Dim li As List(Of mcc_Video) = (From a As mcc_Video In mdc.mcc_Videos Where a.Approved = True).SortBy(sortExp).Skip(startrowindex).Take(maximumrows).ToList
               CacheData(key, li)
               Return li
            End If
         Else
            Return GetVideos(startrowindex, maximumrows, sortExp)
         End If
      End Function


      ''' <summary>
      ''' Returns an Video object with the specified ID
      ''' </summary>
      Public Shared Function GetLatestVideos(ByVal pageSize As Integer) As List(Of mcc_Video)

         Dim Videos As New List(Of mcc_Video)
         Dim key As String = "Videos_Videos_Latest_" + pageSize.ToString

         If mccObject.Cache(key) IsNot Nothing Then
            Videos = DirectCast(mccObject.Cache(key), List(Of mcc_Video))
            'Videos.Sort(New VideoComparer("ReleaseDate DESC"))
         Else
            Videos = GetVideos(True, 0, pageSize, "AddedDate DESC")
         End If
         Return Videos
      End Function

      Public Shared Function GetVideoById(ByVal VideoId As Integer) As mcc_Video
         Dim key As String = "Videos_Videos_" & VideoId.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Dim fb As mcc_Video = DirectCast(Cache(key), mcc_Video)
            Return fb
         Else
            Dim mdc As New MCCDataContext()
            Dim fb As mcc_Video = (From it As mcc_Video In mdc.mcc_Videos Where it.VideoId = VideoId).FirstOrDefault
            CacheData(key, fb)
            Return fb
         End If
      End Function

      Public Shared Function GetVideoBySlug(ByVal slug As String) As mcc_Video
         Dim key As String = "Videos_Videos_" & slug & "_"
         If Cache(key) IsNot Nothing Then
            Dim fb As mcc_Video = DirectCast(Cache(key), mcc_Video)
            Return fb
         Else
            Dim mdc As New MCCDataContext()
            Dim fb As mcc_Video = (From it As mcc_Video In mdc.mcc_Videos Where it.Slug = slug).FirstOrDefault
            CacheData(key, fb)
            Return fb
         End If
      End Function


      Public Shared Function GetVideosByTags(ByVal keys() As String) As List(Of mcc_Video)

         Array.Sort(keys)

         Dim ark As String = String.Join("_", keys)

         Dim key As String = "videos_videos_" & ark.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Return DirectCast(Cache(key), List(Of mcc_Video))
         Else
            Dim li As New List(Of mcc_Video)
            'Using mdc As New MCCDataContext
            '   Dim q = mcc_Video.ContainsAny(keys.ToArray)
            '   li = (From it As mcc_Video In mdc.mcc_Videos.Where(q)).Distinct().ToList()
            'End Using
            CacheData(key, li)
            Return li
         End If
      End Function


      ''' <summary>
      ''' Creates a new Video
      ''' </summary>
      Public Shared Sub InsertVideo(ByVal title As String, ByVal name As String, ByVal Abstract As String, ByVal videoUrl As String, ByVal approved As Boolean, ByVal listed As Boolean, ByVal commentsEnabled As Boolean, _
       ByVal onlyForMembers As Boolean, ByVal tags As String, ByVal slug As String, ByVal imageUrl As String)

         ' on an inserted Video the approved flagged will always be false.
         approved = False

         Dim mdc As MCCDataContext = New MCCDataContext
         Dim wrd As New mcc_Video

         title = mccObject.ConvertNullToEmptyString(title)
         Abstract = mccObject.ConvertNullToEmptyString(Abstract)
         videoUrl = mccObject.ConvertNullToEmptyString(videoUrl)
         tags = mccObject.ConvertNullToEmptyString(tags)


         With wrd
            .AddedDate = DateTime.Now
            .AddedBy = mccObject.CurrentUserName
            .Title = title
            .Abstract = Abstract
            .VideoUrl = videoUrl
            .OnlyForMembers = onlyForMembers
            .Approved = approved
            .Listed = listed
            .CommentsEnabled = commentsEnabled
            .Name = name
            .Tags = tags
            .Slug = routines.GetSlugFromString(title)
         End With


         mdc.mcc_Videos.InsertOnSubmit(wrd)
         mdc.SubmitChanges()

         If Not String.IsNullOrEmpty(wrd.Tags) AndAlso wrd.Tags.Split(",").Count > 0 Then
            VideoTagRepository.InsertVideoTags(wrd.Tags.Split(",").ToList())
         End If


         mccObject.PurgeCacheItems("Videos_Videos_")
         mccObject.PurgeCacheItems("Videos_Videos_Latest_")
         mccObject.PurgeCacheItems("Videos_Video")

         mccObject.PurgeCacheItems("fVideos_fVideos")
         mccObject.PurgeCacheItems("fVideos_fVideocount_")

      End Sub


      ''' <summary>
      ''' Creates a new Video
      ''' </summary>
      Public Shared Sub InsertVideo(ByVal vd As mcc_Video)

         ' on an inserted Video the approved flagged will always be false.
         vd.Approved = False

         Dim mdc As MCCDataContext = New MCCDataContext


         vd.Title = mccObject.ConvertNullToEmptyString(vd.Title)
         vd.Abstract = mccObject.ConvertNullToEmptyString(vd.Abstract)
         vd.VideoUrl = mccObject.ConvertNullToEmptyString(vd.VideoUrl)
         vd.Tags = mccObject.ConvertNullToEmptyString(vd.Tags)


         With vd
            .Name = vd.Name
            .AddedDate = DateTime.Now
            .AddedBy = mccObject.CurrentUserName
            .Slug = routines.GetSlugFromString(vd.Title)
         End With

         mdc.mcc_Videos.InsertOnSubmit(vd)
         mdc.SubmitChanges()


         If Not String.IsNullOrEmpty(vd.Tags) AndAlso vd.Tags.Split(",").Count > 0 Then
            VideoTagRepository.InsertVideoTags(vd.Tags.Split(",").ToList())
         End If


         mccObject.PurgeCacheItems("Videos_Videos_")
         mccObject.PurgeCacheItems("Videos_Videos_Latest_")
         mccObject.PurgeCacheItems("Videos_Video")

         mccObject.PurgeCacheItems("fVideos_fVideos")
         mccObject.PurgeCacheItems("fVideos_fVideocount_")

      End Sub

      Public Shared Sub UpdateVideoInfo(ByVal videoId As Integer, ByVal title As String, ByVal Abstract As String, ByVal tags As String)

         Dim mdc As MCCDataContext = New MCCDataContext

         Dim wrd As mcc_Video = (From t In mdc.mcc_Videos _
                                                Where t.VideoId = videoId _
                                                Select t).FirstOrDefault
         If wrd IsNot Nothing Then

            title = mccObject.ConvertNullToEmptyString(title)
            Abstract = mccObject.ConvertNullToEmptyString(Abstract)
            tags = mccObject.ConvertNullToEmptyString(tags)


            'Dim avpb As New Microsoft.DirectX.AudioVideoPlayback.Video(HttpContext.Current.Server.MapPath(wrd.VideoUrl))

            If File.Exists(HttpContext.Current.Server.MapPath(wrd.VideoUrl)) Then
               Dim duration As Double = 0
               'Using tml As Timeline.ITimeline = New Timeline.DefaultTimeline()
               '   tml.AddAudioGroup().AddTrack()
               '   tml.AddVideoGroup(24, 320, 240).AddTrack()
               '   tml.AddVideo(HttpContext.Current.Server.MapPath(wrd.VideoUrl))
               '   duration = tml.Duration
               'End Using
               wrd.Duration = duration
            End If

            With wrd
               .Title = title
               .Abstract = Abstract
               .Tags = tags
               .Slug = routines.GetSlugFromString(title)
            End With

            mdc.SubmitChanges()


            If Not String.IsNullOrEmpty(wrd.Tags) AndAlso wrd.Tags.Split(",").Count > 0 Then
               VideoTagRepository.InsertVideoTags(wrd.Tags.Split(",").ToList())
            End If


            mccObject.PurgeCacheItems("Videos_VideoCount")
            mccObject.PurgeCacheItems("Videos_Video_" + videoId.ToString())
            mccObject.PurgeCacheItems("Videos_Videos_Latest_")
            mccObject.PurgeCacheItems("Videos_Videos")
            mccObject.PurgeCacheItems("Videos_Specified_Categories_" + videoId.ToString)

            mccObject.PurgeCacheItems("fVideos_fVideos")


         End If
      End Sub

      Public Shared Sub UpdateVideo(ByVal VideoId As Integer, ByVal title As String, ByVal Abstract As String, ByVal videoUrl As String, ByVal approved As Boolean, ByVal listed As Boolean, _
      ByVal commentsEnabled As Boolean, ByVal onlyForMembers As Boolean, ByVal tags As String, ByVal slug As String, ByVal imageUrl As String)

         Dim mdc As MCCDataContext = New MCCDataContext

         Dim wrd As mcc_Video = (From t In mdc.mcc_Videos _
                                     Where t.VideoId = VideoId _
                                     Select t).FirstOrDefault()
         If wrd IsNot Nothing Then

            title = mccObject.ConvertNullToEmptyString(title)
            Abstract = mccObject.ConvertNullToEmptyString(Abstract)
            videoUrl = mccObject.ConvertNullToEmptyString(videoUrl)
            tags = mccObject.ConvertNullToEmptyString(tags)

            'Dim avpb As New Microsoft.DirectX.AudioVideoPlayback.Video(HttpContext.Current.Server.MapPath(wrd.VideoUrl))
            'If avpb IsNot Nothing Then
            '   wrd.Duration = avpb.Duration
            'End If

            With wrd
               .Title = title
               .Abstract = Abstract
               .VideoUrl = videoUrl
               .OnlyForMembers = onlyForMembers
               .Approved = approved
               .Listed = listed
               .CommentsEnabled = commentsEnabled
               .Tags = tags

               '.ImageUrl = imageUrl
               .Slug = routines.GetSlugFromString(title)
            End With

            mdc.SubmitChanges()


            If Not String.IsNullOrEmpty(wrd.Tags) AndAlso wrd.Tags.Split(",").Count > 0 Then
               VideoTagRepository.InsertVideoTags(wrd.Tags.Split(",").ToList())
            End If


            mccObject.PurgeCacheItems("Videos_VideoCount")
            mccObject.PurgeCacheItems("Videos_Video_" + VideoId.ToString())
            mccObject.PurgeCacheItems("Videos_Videos_Latest_")
            mccObject.PurgeCacheItems("Videos_Videos")
            mccObject.PurgeCacheItems("Videos_Specified_Categories_" + VideoId.ToString)

            mccObject.PurgeCacheItems("fVideos_fVideos")

         End If
      End Sub

      Public Shared Sub SimpleUpdateVideo(ByVal videoId As Integer, ByVal title As String, ByVal Abstract As String, _
      ByVal tags As String)

         Dim mdc As MCCDataContext = New MCCDataContext

         Dim wrd As mcc_Video = (From t In mdc.mcc_Videos _
                                     Where t.VideoId = videoId _
                                     Select t).FirstOrDefault
         If wrd IsNot Nothing Then

            title = mccObject.ConvertNullToEmptyString(title)
            Abstract = mccObject.ConvertNullToEmptyString(Abstract)
            tags = mccObject.ConvertNullToEmptyString(tags)

            With wrd
               .Title = title
               .Abstract = Abstract
               .Tags = tags
               .Slug = routines.GetSlugFromString(title)
            End With

            mdc.SubmitChanges()


            If Not String.IsNullOrEmpty(wrd.Tags) AndAlso wrd.Tags.Split(",").Count > 0 Then
               VideoTagRepository.InsertVideoTags(wrd.Tags.Split(",").ToList())
            End If


            mccObject.PurgeCacheItems("Videos_VideoCount")
            mccObject.PurgeCacheItems("Videos_Video_" + videoId.ToString())
            mccObject.PurgeCacheItems("Videos_Videos_Latest_")
            mccObject.PurgeCacheItems("Videos_Videos")
            mccObject.PurgeCacheItems("Videos_Specified_Categories_" + videoId.ToString)

            mccObject.PurgeCacheItems("fVideos_fVideos")

         End If
      End Sub

      Public Shared Sub DeleteVideo(ByVal VideoId As Integer)
         Dim mdc As MCCDataContext = New MCCDataContext
         If mdc.mcc_Videos.Count(Function(p) p.VideoId = VideoId) > 0 Then
            Dim wrd As mcc_Video = (From t In mdc.mcc_Videos _
                                        Where t.VideoId = VideoId _
                                        Select t).FirstOrDefault()
            If wrd IsNot Nothing Then

               RemoveVideoFromCategory(VideoId)
               Dim tb As New MCCEvents.MCCEvents.RecordDeletedEvent("Video", VideoId, Nothing)
               tb.Raise()


               mdc.mcc_Videos.DeleteOnSubmit(wrd)
               mdc.SubmitChanges()
               mccObject.PurgeCacheItems("Videos_Videos_")

               Dim fi As New System.IO.FileInfo(HttpContext.Current.Server.MapPath(wrd.VideoUrl))
               If fi IsNot Nothing AndAlso fi.Exists Then
                  If fi.Name.IndexOf(fi.Directory.Name) > -1 Then
                     System.IO.Directory.Delete(fi.Directory.FullName, True)
                  End If
               End If

               mccObject.PurgeCacheItems("Videos_Video")
               mccObject.PurgeCacheItems("Videos_Videos_")
               mccObject.PurgeCacheItems("Videos_Videos_Latest_")
               mccObject.PurgeCacheItems("Videos_Specified_Categories_" + VideoId.ToString)

               mccObject.PurgeCacheItems("fVideos_fVideos")
               mccObject.PurgeCacheItems("fVideos_fVideocount_")
            End If
         End If
      End Sub

      Public Shared Sub DeleteVideos(ByVal VideoIds() As Integer)
         Dim mdc As MCCDataContext = New MCCDataContext
         Dim wrd As List(Of mcc_Video) = (From t In mdc.mcc_Videos _
                                        Where VideoIds.Contains(t.VideoId)).ToList
         If wrd IsNot Nothing AndAlso wrd.Count > 0 Then


            mdc.mcc_Videos.DeleteAllOnSubmit(wrd)
            mdc.SubmitChanges()
            mccObject.PurgeCacheItems("videos_")

            For Each it As mcc_Video In wrd
               RemoveVideoFromCategory(it.VideoId)
               'Dim tb As New MCCEvents.MCCEvents.RecordDeletedEvent("Article", it.ArticleID, Nothing)
               'tb.Raise()
            Next

            'mccObject.PurgeCacheItems("Articles_Article")
            'mccObject.PurgeCacheItems("Articles_Articles_")
            'mccObject.PurgeCacheItems("Articles_Articles_Latest_")
            'mccObject.PurgeCacheItems("Articles_Specified_Categories_" + ArticleId.ToString)

            'mccObject.PurgeCacheItems("farticles_farticles")
            'mccObject.PurgeCacheItems("farticles_farticlecount_")
         End If
      End Sub

      Public Shared Sub ApproveVideo(ByVal VideoId As Integer)
         Dim mdc As MCCDataContext = New MCCDataContext
         Dim wrd As mcc_Video = (From t In mdc.mcc_Videos _
                                        Where t.VideoId = VideoId _
                                        Select t).FirstOrDefault()
         If wrd IsNot Nothing Then

            wrd.Approved = True
            mdc.SubmitChanges()
            mccObject.PurgeCacheItems("Videos_VideoCount")
            mccObject.PurgeCacheItems("Videos_Video_" + VideoId.ToString())
            mccObject.PurgeCacheItems("Videos_Videos")
            mccObject.PurgeCacheItems("Videos_Videos_Latest_")

            mccObject.PurgeCacheItems("fVideos_fVideos")
            mccObject.PurgeCacheItems("fVideos_fVideocount_")
         End If
      End Sub

      Public Shared Function RemoveVideoFromCategory(ByVal VideoID As Integer, Optional ByVal categoryId As Integer = 0) As Boolean
         'Dim ret As Boolean = SiteProvider.Videos.RemoveVideoFromCategory(VideoID, categoryId)
         Dim ret As Boolean = True

         Dim mdc As MCCDataContext = New MCCDataContext
         If mdc.mcc_Videos.Count(Function(p) p.VideoId = VideoID) > 0 _
            AndAlso mdc.mcc_CategoriesVideos.Count(Function(p) p.VideoId = VideoID) > 0 Then

            Dim wrd As New List(Of mcc_CategoriesVideo)

            If categoryId = 0 Then
               wrd = (From t In mdc.mcc_CategoriesVideos Where t.VideoId = VideoID).ToList()
            Else
               wrd = (From t In mdc.mcc_CategoriesVideos Where t.VideoId = VideoID AndAlso t.CategoryId = categoryId).ToList()
            End If

            For Each it As mcc_CategoriesVideo In wrd
               mdc.mcc_CategoriesVideos.DeleteOnSubmit(it)
            Next

            mdc.SubmitChanges()

            mccObject.PurgeCacheItems("Videos_Video")
            mccObject.PurgeCacheItems("Videos_VideoInCategories_")
            mccObject.PurgeCacheItems("Videos_Specified_Categories_" + VideoID.ToString)
         End If
         Return ret
      End Function

      Public Shared Function AddVideoToCategory(ByVal VideoId As Integer, ByVal CategoryId As Integer) As Integer
         'Dim ret As Integer = SiteProvider.Videos.AddVideoToCategory(VideoId, CategoryId)
         Dim mdc As MCCDataContext = New MCCDataContext
         If mdc.mcc_CategoriesVideos.Count(Function(p) p.VideoId = VideoId AndAlso p.CategoryId = CategoryId) = 0 Then
            Dim cid As New mcc_CategoriesVideo
            cid.VideoId = VideoId
            cid.CategoryId = CategoryId

            mdc.mcc_CategoriesVideos.InsertOnSubmit(cid)
            mdc.SubmitChanges()
            mccObject.PurgeCacheItems("Videos_VideoInCategories_")
         End If
      End Function


      Public Shared Sub IncrementViewCount(ByVal VideoId As Integer)
         Dim mdc As MCCDataContext = New MCCDataContext
         Dim wrd As mcc_Video = (From t In mdc.mcc_Videos _
                                        Where t.VideoId = VideoId _
                                        Select t).FirstOrDefault

         If wrd IsNot Nothing Then
            wrd.ViewCount += 1
            mdc.SubmitChanges()
            mccObject.PurgeCacheItems("Videos_Videos_")
            mccObject.PurgeCacheItems("Videos_VideoCount_")
         End If
      End Sub

      Public Shared Sub RateVideo(ByVal VideoId As Integer, ByVal rating As Integer)
         Dim mdc As MCCDataContext = New MCCDataContext
         Dim wrd As mcc_Video = (From t In mdc.mcc_Videos _
                                     Where t.VideoId = VideoId _
                                     Select t).FirstOrDefault
         If wrd IsNot Nothing Then
            wrd.Votes += 1
            wrd.TotalRating += rating

            mdc.SubmitChanges()
            mccObject.PurgeCacheItems("Videos_Videos_")
            mccObject.PurgeCacheItems("Videos_VideoCount_")
         End If
      End Sub

      Public Shared Function FindVideos(ByVal SearchWord As String) As List(Of mcc_Video)
         Return FindVideos(0, mccObject.MaxRows, SearchWord)
      End Function

      Public Shared Function FindVideos(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal SearchWord As String) As List(Of mcc_Video)
         'Dim Videos As List(Of Video) = Nothing

         'Dim recordset As List(Of VideoDetails) = SiteProvider.Videos.FindVideos(GetPageIndex(startRowIndex, maximumRows), maximumRows, SearchWord)
         'Videos = GetVideoListFromVideoDetailsList(recordset)


         Dim mdc As New MCCDataContext()
         'Dim iLst As List(Of FindVideoResult) = mdc.FindVideo(SearchWord).ToList

         'Dim li As List(Of mcc_Video) = (From it As mcc_Video In mdc.mcc_Videos Join fts In mdc.FindVideo(SearchWord) On it.VideoId Equals fts.VideoID Select it).ToList
         Dim li As List(Of mcc_Video) = (From it As mcc_Video In mdc.mcc_Videos Where it.Title.Contains(SearchWord) Or it.Abstract.Contains(SearchWord)).ToList
         Return li
      End Function

      ''' <summary>
      ''' Returns a collection with all Videos for the specified category
      ''' </summary>
      Public Shared Function FindVideos(ByVal categoryID As Integer, ByVal SearchWord As String) As List(Of mcc_Video)
         Return FindVideos(categoryID, 0, mccObject.MaxRows, SearchWord)
      End Function

      Public Shared Function FindVideos(ByVal categoryID As Integer, ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal SearchWord As String) As List(Of mcc_Video)
         If categoryID <= 0 Then
            Return FindVideos(startRowIndex, maximumRows, SearchWord)
         End If

         'Dim Videos As List(Of Video) = Nothing

         'Dim recordset As List(Of VideoDetails) = SiteProvider.Videos.FindVideos(categoryID, GetPageIndex(startRowIndex, maximumRows), maximumRows, SearchWord)
         'Videos = GetVideoListFromVideoDetailsList(recordset)

         Return Nothing
      End Function

      ''' <summary>
      ''' Returns the number of total Videos matching the search key
      ''' </summary>
      Public Shared Function FindVideosCount(ByVal searchWord As String) As Integer
         Dim vdCount As Integer = 0

         Dim mdc As New MCCDataContext()
         'vdCount = mdc.mcc_Videos_FindVideosCount_linq(searchWord)
         vdCount = mdc.mcc_Videos.Count(Function(p) p.Abstract.Contains(searchWord) Or p.Title.Contains(searchWord))
         Return vdCount
      End Function

      'Public Shared Function FetchVideosCount(ByVal status As mccEnum.Status) As Integer

      '   Dim key As String = "fVideos_fVideocount_" & "st_" & status.ToString
      '   If Cache(key) IsNot Nothing Then
      '      Return CInt(Cache(key))
      '   Else
      '      Dim mdc As New MCCDataContext
      '      If status < 0 Or status > 5 Then
      '         Return 0
      '      Else
      '         Return mdc.mcc_Videos.Count(Function(p) p.Status = CInt(status))
      '      End If
      '   End If
      'End Function

      'Public Shared Function FetchVideosCount(ByVal status As Integer) As Integer

      '   Dim key As String = "fVideos_fVideocount_" & "st_" & status.ToString
      '   If Cache(key) IsNot Nothing Then
      '      Return CInt(Cache(key))
      '   Else
      '      Dim mdc As New MCCDataContext
      '      If status < 0 Then
      '         Return mdc.mcc_Videos.Count()
      '      Else
      '         Return mdc.mcc_Videos.Count(Function(p) p.Status = status)
      '      End If
      '   End If
      'End Function

      'Public Shared Function FetchVideosCount(ByVal categoryID As Integer, ByVal status As Integer) As Integer

      '   Dim key As String = "fVideo_fVideocount_" & categoryID.ToString & "_" & "st_" & status.ToString

      '   If (Cache(key) IsNot Nothing) Then
      '      Return CInt(Cache(key))
      '   Else
      '      Dim mdc As New MCCDataContext

      '      If categoryID <= 0 Then
      '         Return FetchVideosCount(status)
      '      End If
      '      Dim catAr As List(Of Integer) = (From ef As mcc_VideoCategory In mdc.mcc_VideoCategories Where ef.CategoryId = categoryID Select ef.VideoId).ToList
      '      Dim cnt As Integer = mdc.mcc_Videos.Count(Function(p) p.Status = status And catAr.Contains(p.VideoId))
      '      CacheData(key, cnt)
      '      Return cnt
      '   End If
      'End Function

      Public Shared Function FetchVideos(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, Optional ByVal sortExp As String = "Title") As List(Of mcc_Video)

         Dim li As New List(Of mcc_Video)
         Dim key As String = "fVideos_fVideos_" & "_" & startRowIndex.ToString & "_" & maximumRows.ToString & "_" & sortExp

         If (Cache(key) IsNot Nothing) Then
            li = DirectCast(Cache(key), List(Of mcc_Video))
         Else
            Dim mdc As New MCCDataContext
            Dim i As Integer = mdc.mcc_Videos.Count()
            If i > 0 Then
               If startRowIndex > 0 AndAlso maximumRows > 0 Then
                  li = (From it As mcc_Video In mdc.mcc_Videos).SortBy(sortExp).Skip(startRowIndex).Take(maximumRows).ToList
               Else
                  li = (From it As mcc_Video In mdc.mcc_Videos).SortBy(sortExp).Skip(0).Take(12).ToList
                  Dim ic = (From it As mcc_Video In mdc.mcc_Videos).SortBy(sortExp).Skip(0).Take(12)
               End If
            End If
            CacheData(key, li)
         End If
         Return li
      End Function

      'Public Shared Function FetchVideos(ByVal status As Integer, ByVal startRowIndex As Integer, ByVal maximumRows As Integer, Optional ByVal sortExp As String = "Title") As List(Of mcc_Video)
      '   If status < 0 Then
      '      Return FetchVideos(startRowIndex, maximumRows, sortExp)
      '   End If
      '   Dim li As New List(Of mcc_Video)
      '   Dim key As String = "fVideos_fVideos_" & "st_" & status.ToString & "_" & startRowIndex.ToString & "_" & maximumRows.ToString & "_" & sortExp

      '   If (Cache(key) IsNot Nothing) Then
      '      li = DirectCast(Cache(key), List(Of mcc_Video))
      '   Else
      '      Dim mdc As New MCCDataContext

      '      Dim i As Integer = mdc.mcc_Videos.Count(Function(p) p.Status = status)
      '      If i > 0 Then
      '         If startRowIndex > 0 AndAlso maximumRows > 0 Then
      '            li = (From it As mcc_Video In mdc.mcc_Videos Where it.Status = status).SortBy(sortExp).Skip(startRowIndex).Take(maximumRows).ToList
      '         Else
      '            li = (From it As mcc_Video In mdc.mcc_Videos Where it.Status = status).SortBy(sortExp).Skip(0).Take(12).ToList
      '         End If
      '         CacheData(key, li)
      '      End If
      '   End If


      '   Return li
      'End Function

      'Public Shared Function FetchVideos(ByVal categoryID As Integer, ByVal status As Integer, ByVal startRowIndex As Integer, ByVal maximumRows As Integer, Optional ByVal sortExp As String = "AddedDate") As List(Of mcc_Video)

      '   If categoryID <= 0 Then
      '      Return FetchVideos(status, startRowIndex, maximumRows, sortExp)
      '   End If

      '   Dim key As String = "fVideos_fVideos_" & categoryID.ToString & "_" & "st_" & status.ToString & "_" & startRowIndex.ToString & "_" & maximumRows.ToString & "_" & sortExp
      '   Dim li As New List(Of mcc_Video)

      '   If (Cache(key) IsNot Nothing) Then
      '      li = DirectCast(Cache(key), List(Of mcc_Video))
      '   Else
      '      Dim mdc As New MCCDataContext
      '      Dim catAr As List(Of Integer) = (From ef As mcc_VideoCategory In mdc.mcc_VideoCategories Where ef.CategoryId = categoryID Select ef.VideoId).ToList

      '      If startRowIndex > 0 AndAlso maximumRows > 0 Then
      '         li = (From it As mcc_Video In mdc.mcc_Videos Where catAr.Contains(it.VideoId) And it.Status = status).SortBy(sortExp).Skip(startRowIndex).Take(maximumRows).ToList
      '      Else
      '         li = (From it As mcc_Video In mdc.mcc_Videos Where catAr.Contains(it.VideoId) And it.Status = status).SortBy(sortExp).Skip(0).Take(12).ToList
      '      End If
      '      CacheData(key, li)
      '   End If
      '   Return li
      'End Function


      'Public Shared Sub UpdateVideoStatus(ByVal id As Integer)
      '   Dim mdc As New MCCDataContext()
      '   If mdc.mcc_Videos.Count(Function(p) p.VideoId = id) > 0 Then
      '      Dim it As mcc_Video = (From ars As mcc_Video In mdc.mcc_Videos Where ars.VideoId = id).FirstOrDefault
      '      If it IsNot Nothing AndAlso it.Status < 5 Then
      '         it.Status += 1
      '         mdc.SubmitChanges()
      '         mccObject.PurgeCacheItems("Videos_")
      '         mccObject.PurgeCacheItems("fVideos_")
      '      End If
      '   End If
      'End Sub

      'Public Shared Sub UpdateVideoStatus(ByVal id As Integer, ByVal st As mccEnum.Status)
      '   Dim mdc As New MCCDataContext()
      '   If mdc.mcc_Videos.Count(Function(p) p.VideoId = id) > 0 Then
      '      Dim it As mcc_Video = (From ars As mcc_Video In mdc.mcc_Videos Where ars.VideoId = id).First
      '      If it IsNot Nothing Then
      '         If st <= 5 AndAlso st >= 0 Then
      '            it.Status = CInt(st)

      '            If st <> mccEnum.Status.Approved Then
      '               it.Approved = False
      '            Else
      '               it.Approved = True
      '            End If

      '            mdc.SubmitChanges()
      '            mccObject.PurgeCacheItems("Videos_")
      '            mccObject.PurgeCacheItems("fVideos_")
      '         End If
      '      End If
      '   End If
      'End Sub

      'Public Shared Function GetVideoStatusValue(ByVal id As Integer) As Integer
      '   Dim mdc As New MCCDataContext()
      '   If mdc.mcc_Videos.Count(Function(p) p.VideoId = id) > 0 Then
      '      Return (From it As mcc_Video In mdc.mcc_Videos Where it.VideoId = id Select it.Status).First
      '   End If
      '   Return -1
      'End Function

      'Public Shared Function GetVideoStatus(ByVal VideoId As Integer) As mccEnum.Status
      '   Dim mdc As New MCCDataContext()
      '   If mdc.mcc_Videos.Count(Function(p) p.VideoId = VideoId) > 0 Then
      '      Dim ival As Integer = (From it As mcc_Video In mdc.mcc_Videos Where it.VideoId = VideoId Select it.Status).First
      '      Return CType(ival, mccEnum.Status)
      '   End If
      '   Return mccEnum.Status.Pending
      'End Function

      Protected Shared Sub CacheData(ByVal key As String, ByVal data As Object)
         If data IsNot Nothing Then
            mccObject.Cache.Insert(key, data, Nothing, DateTime.Now.AddSeconds(3600), TimeSpan.Zero)
         End If
      End Sub

   End Class
End Namespace

Namespace MCC.Data
   Partial Class mcc_Video
      Public Shared Function ContainsAny(ByVal ParamArray keywords As String()) As Expression(Of Func(Of mcc_Video, Boolean))
         Dim predicate = PredicateBuilder.[False](Of mcc_Video)()
         'For Each keyword As String In keywords
         '   Dim temp As String = keyword
         '   predicate = predicate.[Or](Function(p) p.Abstract.Contains(temp))
         '   predicate = predicate.[Or](Function(p) p.Title.Contains(temp))
         '   predicate = predicate.[Or](Function(p) p.Tags.Contains(temp))
         'Next
         Return predicate
      End Function
   End Class
End Namespace

