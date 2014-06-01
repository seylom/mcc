Imports MCC.Data
Imports Webdiyer.WebControls.Mvc
Public Class VideoService
   Inherits CacheObject
   Implements IVideoService


   Private _VideoRepo As IVideoRepository
   Private _CommentRepo As IVideoCommentRepository
   Private _VideoCategoryRepo As IVideoCategoryRepository
   Private _VideoTagRepo As IVideoTagRepository = New VideoTagRepository()

   Public Sub New()
      Me.New(New VideoRepository, New VideoCategoryRepository, New VideoCommentRepository)
   End Sub

   Public Sub New(ByVal videoRepo As IVideoRepository, ByVal videoCategoryRepo As IVideoCategoryRepository, _
                  ByVal commentRepo As IVideoCommentRepository)
      _VideoRepo = videoRepo
      _CommentRepo = commentRepo
      _VideoCategoryRepo = videoCategoryRepo
   End Sub


   ''' <summary>
   ''' Get the average rating for an video
   ''' </summary>
   ''' <param name="videoId">The video id.</param>
   ''' <returns></returns>
   Public Function AverageRating(ByVal videoId As Integer) As Double Implements IVideoService.AverageRating
      Dim key As String = "videos_average_rating_" & videoId.ToString

      If Cache(key) IsNot Nothing Then
         Return CDbl(Cache(key))
      Else
         Dim val As Double = 0
         Dim wrd As Video = _VideoRepo.GetVideos.Where(Function(p) p.VideoID = videoId).FirstOrDefault()
         If wrd IsNot Nothing Then
            If wrd.Votes >= 1 Then
               val = (CDbl(wrd.TotalRating) / CDbl(wrd.Votes))
            End If
         End If
         CacheData(key, val)
      End If
   End Function


   ''' <summary>
   ''' Gets the comments.
   ''' </summary>
   ''' <param name="videoId">The video id.</param>
   ''' <returns></returns>
   Public Function GetComments(ByVal videoId As Integer) As IList(Of VideoComment) Implements IVideoService.GetComments
      Dim key As String = "videos_videoComments_" & videoId.ToString
      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), List(Of VideoComment))
      Else
         Dim _comments As List(Of VideoComment) = Nothing
         _comments = _CommentRepo.GetVideoComments.Where(Function(c) c.VideoID = videoId).ToList
         CacheData(key, _comments)
         Return _comments
      End If
   End Function

   Public Function CommentsCount(ByVal VideoId As Integer) As Integer Implements IVideoService.CommentsCount
      Dim key As String = ""
      Return _CommentRepo.GetVideoComments.WithVideoID(VideoId).Count
   End Function


   Public Function GetVideoCount() As Integer Implements IVideoService.GetVideoCount
      Dim key As String = "videos_videoCount_"

      If Cache(key) IsNot Nothing Then
         Return CInt(Cache(key))
      Else
         Dim it As Integer = _VideoRepo.GetVideos.Count()
         CacheData(key, it)
         Return it
      End If
   End Function

   Public Function GetVideoCount(ByVal PublishedOnly As Boolean) As Integer Implements IVideoService.GetVideoCount
      If Not PublishedOnly Then
         Return GetVideoCount()
      End If

      Dim key As String = "videos_videoCount_" & PublishedOnly.ToString

      If Cache(key) IsNot Nothing Then
         Return CInt(Cache(key))
      Else
         Dim it As Integer = _VideoRepo.GetVideos.Published.Count
         CacheData(key, it)
         Return it
      End If
   End Function

   Public Function GetVideoCount(ByVal categoryId As Integer) As Integer Implements IVideoService.GetVideoCount
      If categoryId <= 0 Then
         Return GetVideoCount()
      End If

      Dim key As String = "videos_videoCount_" & categoryId.ToString & "_"
      If Cache(key) IsNot Nothing Then
         Return CInt(Cache(key))
      Else
         Dim it As Integer = _VideoRepo.GetVideosInCategory(categoryId).Count()
         CacheData(key, it)
         Return it
      End If
   End Function

   Public Function GetVideoCount(ByVal publishedOnly As Boolean, ByVal categoryID As Integer) As Integer Implements IVideoService.GetVideoCount
      If publishedOnly Then
         Return GetVideoCount(categoryID)
      End If

      If categoryID <= 0 Then
         Return GetVideoCount(publishedOnly)
      End If

      Dim key As String = "Videos_VideoCount_" + publishedOnly.ToString() + "_" + categoryID.ToString()
      If Cache(key) IsNot Nothing Then
         Return CInt(Cache(key))
      Else
         Dim it As Integer = _VideoRepo.GetVideosInCategory(categoryID).Published.Count
         CacheData(key, it)
         Return it
      End If
   End Function


   Public Function GetVideos() As List(Of Video) Implements IVideoService.GetVideos
      Dim key As String = "videos_videos_"
      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), List(Of Video))
      Else
         Dim li As List(Of Video) = _VideoRepo.GetVideos().ToList
         CacheData(key, li)
         Return li
      End If
   End Function


   Public Function GetVideos(ByVal publishedOnly As Boolean, ByVal categoryId As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AddedDate") As PagedList(Of Video) Implements IVideoService.GetVideos
      If Not publishedOnly Then
         Return GetVideos(categoryId, startrowindex, maximumrows, sortExp)
      End If

      If categoryId > 0 Then
         If String.IsNullOrEmpty(sortExp) Then
            sortExp = "ReleaseDate DESC"
         End If
         Dim key As String = "videos_videos_" & categoryId.ToString & "_" & sortExp.Replace(" ", "") & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Return DirectCast(Cache(key), PagedList(Of Video))
         Else
            Dim li As PagedList(Of Video) = _VideoRepo.GetVideosInCategory(categoryId).Published.SortBy(sortExp).ToPagedList(startrowindex, maximumrows)
            CacheData(key, li)
            Return li
         End If
      Else
         Return GetVideos(publishedOnly, startrowindex, maximumrows, sortExp)
      End If
   End Function

   Public Function GetVideos(ByVal categoryId As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AddedDate") As PagedList(Of Video) Implements IVideoService.GetVideos
      If categoryId > 0 Then
         Dim key As String = "videos_videos_" & categoryId.ToString & "_" & sortExp.Replace(" ", "") & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
         If String.IsNullOrEmpty(sortExp) Then
            sortExp = "ReleaseDate DESC"
         End If
         If Cache(key) IsNot Nothing Then
                Dim li As PagedList(Of Video) = DirectCast(Cache(key), PagedList(Of Video))
            Return li
         Else
            Dim li As PagedList(Of Video) = _VideoRepo.GetVideosInCategory(categoryId).SortBy(sortExp).ToPagedList(startrowindex, maximumrows)
            CacheData(key, li)
            Return li
         End If
      Else
         Return GetVideos(startrowindex, maximumrows, sortExp)
      End If
   End Function

   'Public Function GetVideosByCategoryName(ByVal categoryName As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "releaseDate") As List(Of Video) Implements IVideoService.GetVideosByCategoryName
   '   If Not String.IsNullOrEmpty(categoryName) Then
   '      Dim key As String = "videos_videos_" & categoryName & "_" & sortExp.Replace(" ", "") & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"

   '      If String.IsNullOrEmpty(sortExp) Then
   '         sortExp = "ReleaseDate DESC"
   '      End If
   '      If Cache(key) IsNot Nothing Then
   '         Dim li As List(Of Video) = DirectCast(Cache(key), List(Of Video))
   '         Return li
   '      Else
   '         Dim catId As Integer = _VideoCategoryRepo.GetCategories.WithSlug(categoryName).Select(Function(q) q.CategoryID).FirstOrDefault
   '         Dim li As PagedList(Of Video) = _VideoRepo.GetVideosInCategory(catId).SortBy(sortExp).ToPagedList(startrowindex, maximumrows)
   '         CacheData(key, li)
   '         Return li
   '      End If
   '   Else
   '      Return GetVideos(startrowindex, maximumrows, sortExp)
   '   End If
   'End Function

   Public Function GetVideosByAuthor(ByVal publishedOnly As Boolean, ByVal addedBy As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedby") As PagedList(Of Video) Implements IVideoService.GetVideosByAuthor

        'If Not publishedOnly Then
        '   Return GetVideosByAuthor(addedBy, startrowindex, maximumrows, sortExp)
        'End If

      If Not String.IsNullOrEmpty(addedBy) Then
         If String.IsNullOrEmpty(sortExp) Then
            sortExp = "ReleaseDate DESC"
         End If
         Dim key As String = "videos_videos_" & addedBy & "_" & publishedOnly.ToString & "_" & sortExp.Replace(" ", "") & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Return DirectCast(Cache(key), PagedList(Of Video))
         Else
            Dim li As PagedList(Of Video) = _VideoRepo.GetVideos.Published.WithAuthor(addedBy).SortBy(sortExp).ToPagedList(startrowindex, maximumrows)
            CacheData(key, li)
            Return li
         End If
      Else
         Return GetVideos(startrowindex, maximumrows, sortExp)
      End If
   End Function

   Public Function GetVideos(ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AddedDate DESC") As PagedList(Of Video) Implements IVideoService.GetVideos
      Dim key As String = "videos_videos_" & startrowindex.ToString & "_" & sortExp.Replace(" ", "") & "_" & maximumrows.ToString & "_"
      If String.IsNullOrEmpty(sortExp) Then
         sortExp = "ReleaseDate DESC"
      End If
      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), PagedList(Of Video))
      Else
         Dim li As PagedList(Of Video) = _VideoRepo.GetVideos.SortBy(sortExp).ToPagedList(startrowindex, maximumrows)
         CacheData(key, li)
         Return li
      End If
   End Function

   Public Function GetVideos(ByVal publishedOnly As Boolean, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AddedDate DESC") As PagedList(Of Video) Implements IVideoService.GetVideos
      If publishedOnly Then
         If String.IsNullOrEmpty(sortExp) Then
            sortExp = "ReleaseDate DESC"
         End If
         Dim key As String = "videos_videos_" & publishedOnly.ToString & "_" & sortExp.Replace(" ", "") & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Dim li As PagedList(Of Video) = DirectCast(Cache(key), PagedList(Of Video))
            Return li
         Else
            Dim li As PagedList(Of Video) = _VideoRepo.GetVideos.Published.SortBy(sortExp).ToPagedList(startrowindex, maximumrows)
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
   Public Function GetLatestVideos(ByVal pageSize As Integer) As PagedList(Of Video) Implements IVideoService.GetLatestVideos
      Dim key As String = "Videos_Videos_Latest_" + pageSize.ToString

      If CacheObject.Cache(key) IsNot Nothing Then
         Return DirectCast(CacheObject.Cache(key), PagedList(Of Video))
      Else
         Dim vds As PagedList(Of Video) = _VideoRepo.GetVideos.Published.SortBy("ReleaseDate DESC").ToPagedList(0, pageSize)
         CacheData(key, vds)
         Return vds
      End If
   End Function

   Public Function GetVideoById(ByVal VideoId As Integer) As Video Implements IVideoService.GetVideoById
      Dim key As String = "videos_videos_" & VideoId.ToString & "_"
      If Cache(key) IsNot Nothing Then
         Dim fb As Video = DirectCast(Cache(key), Video)
         Return fb
      Else
         Dim fb As Video = _VideoRepo.GetVideos.WithVideoID(VideoId).FirstOrDefault()
         CacheData(key, fb)
         Return fb
      End If
   End Function

   Public Function GetVideoBySlug(ByVal slug As String) As Video Implements IVideoService.GetVideoBySlug
      Dim key As String = "videos_videos_" & slug & "_"
      If Cache(key) IsNot Nothing Then
         Dim fb As Video = DirectCast(Cache(key), Video)
         Return fb
      Else

         Dim fb As Video = _VideoRepo.GetVideos.WithSlug(slug).FirstOrDefault()

         CacheData(key, fb)
         Return fb
      End If
   End Function

   ''' <summary>
   ''' Creates a new Video
   ''' </summary>
   Public Sub InsertVideo(ByVal wrd As Video) Implements IVideoService.InsertVideo


      ' on an inserted video the approved flagged will always be false.
      wrd.Approved = False
      wrd.Title = ConvertNullToEmptyString(wrd.Title)
      With wrd
         .AddedDate = DateTime.Now
         .AddedBy = CacheObject.CurrentUserName
         .Slug = wrd.Title.ToSlug
      End With

      Dim arId As Integer = _VideoRepo.InsertVideo(wrd)


      CacheObject.PurgeCacheItems("Videos_")

   End Sub


   Public Sub UpdateVideo(ByVal _art As Video) Implements IVideoService.UpdateVideo

      Dim wrd As Video = _VideoRepo.GetVideos.WithVideoID(_art.VideoID).FirstOrDefault
      If wrd IsNot Nothing Then

         _art.Title = CacheObject.ConvertNullToEmptyString(_art.Title)
         _art.Abstract = CacheObject.ConvertNullToEmptyString(_art.Abstract)
         _art.Tags = CacheObject.ConvertNullToEmptyString(_art.Tags)
         _VideoRepo.UpdateVideo(wrd)

         CacheObject.PurgeCacheItems("Videos_")
      End If
   End Sub

   Public Sub SimpleUpdateVideo(ByVal wrd As Video) Implements IVideoService.SimpleUpdateVideo

      If wrd IsNot Nothing Then

         Dim c As Video = _VideoRepo.GetVideos.WithVideoID(wrd.VideoID).FirstOrDefault()
         wrd.Title = ConvertNullToEmptyString(wrd.Title)
         If c IsNot Nothing Then
            With c
               .Title = wrd.Title
               .Abstract = wrd.Abstract
               .Tags = wrd.Tags
               .Slug = wrd.Title.ToSlug
            End With
         End If

            If Not String.IsNullOrEmpty(wrd.Tags) AndAlso wrd.Tags.Split(","c).Count > 0 Then
                _VideoTagRepo.InsertTags(wrd.Tags.Split(","c))
            End If

         PurgeCacheItems("Videos_VideoCount")

      End If
   End Sub


   Public Sub DeleteVideos(ByVal VideoIds() As Integer) Implements IVideoService.DeleteVideos
      'Dim mdc As MCCDataContext = New MCCDataContext
      'If mdc.videos.Count(Function(p) VideoId.Contains(p.VideoID)) > 0 Then
      '   Dim wrd As List(Of Video) = (From t In mdc.videos _
      '                               Where VideoId.Contains(t.VideoID)).ToList
      '   If wrd IsNot Nothing AndAlso wrd.Count > 0 Then


      '      mdc.videos.DeleteAllOnSubmit(wrd)
      '      mdc.SubmitChanges()
      '      CacheObject.PurgeCacheItems("videos_videos_")

      '      For Each it As Video In wrd
      '         RemoveVideoFromCategory(it.VideoID)
      '         Dim tb As New MCCEvents.MCCEvents.RecordDeletedEvent("Video", it.VideoID, Nothing)
      '         tb.Raise()
      '      Next

      If VideoIds IsNot Nothing And VideoIds.Count > 0 Then

         _VideoRepo.DeleteVideos(VideoIds)

         CacheObject.PurgeCacheItems("Videos_Video")
         CacheObject.PurgeCacheItems("Videos_Videos_")
         CacheObject.PurgeCacheItems("Videos_Videos_Latest_")
         CacheObject.PurgeCacheItems("Videos_Specified_Categories_" + VideoIds.ToString)

         CacheObject.PurgeCacheItems("fvideos_fvideos")
         CacheObject.PurgeCacheItems("fvideos_fvideocount_")

      End If



      '   End If
      'End If
   End Sub

   Public Sub DeleteVideo(ByVal VideoId As Integer) Implements IVideoService.DeleteVideo
      If (VideoId > 0) Then
         _VideoRepo.DeleteVideo(VideoId)
         CacheObject.PurgeCacheItems("Videos_")
         CacheObject.PurgeCacheItems("fvideos_")
      End If
   End Sub

   Public Sub ApproveVideo(ByVal videoId As Integer) Implements IVideoService.ApproveVideo
      If videoId > 0 Then
         Dim q As Video = _VideoRepo.GetVideos.WithVideoID(videoId).FirstOrDefault
         If q IsNot Nothing Then
            q.Approved = True
            _VideoRepo.UpdateVideo(q)
            CacheObject.PurgeCacheItems("Videos_")
            CacheObject.PurgeCacheItems("fvideos_")
         End If
      End If
   End Sub

   Public Function RemoveVideoFromCategory(ByVal VideoID As Integer, Optional ByVal categoryId As Integer = 0) As Boolean Implements IVideoService.RemoveVideoFromCategory
      'Dim ret As Boolean = SiteProvider.Videos.RemoveVideoFromCategory(VideoID, categoryId)
      'Dim ret As Boolean = True

      If VideoID > 0 Then
         If categoryId > 0 Then
            _VideoCategoryRepo.RemoveVideosFromCategories(VideoID, New Integer() {categoryId})
         Else
            _VideoCategoryRepo.UnCategorizeVideo(VideoID)
         End If

         CacheObject.PurgeCacheItems("Videos_")
      End If
      Return True
   End Function

   'Public Function AddVideoToCategory(ByVal VideoId As Integer, ByVal CategoryId As Integer) As Integer
   '   'Dim ret As Integer = SiteProvider.Videos.AddVideoToCategory(VideoId, CategoryId)
   '   If VideoId > 0 AndAlso CategoryId > 0 Then
   '      _VideoRepo.AddVideoToCategories(VideoId, CategoryId)
   '      CacheObject.PurgeCacheItems("Videos_VideoInCategories_")
   '   End If
   'End Function

   Public Function AddVideoToCategories(ByVal VideoId As Integer, ByVal CategoryIds() As Integer) As Integer Implements IVideoService.AddVideoToCategories
      If VideoId > 0 AndAlso CategoryIds IsNot Nothing Then
         _VideoCategoryRepo.CategorizeVideo(VideoId, CategoryIds)
         CacheObject.PurgeCacheItems("Videos_")
      End If
   End Function


   Public Sub IncrementViewCount(ByVal videoId As Integer) Implements IVideoService.IncrementViewCount
      If videoId > 0 Then
         Dim q = _VideoRepo.GetVideos.WithVideoID(videoId).FirstOrDefault()
         If q IsNot Nothing Then
            q.ViewCount += 1
            _VideoRepo.UpdateVideo(q)
            'CacheObject.PurgeCacheItems("videos_videos_")
            'CacheObject.PurgeCacheItems("videos_videoCount_")
         End If
      End If
   End Sub

   Public Sub RateVideo(ByVal videoId As Integer, ByVal rating As Integer) Implements IVideoService.RateVideo

      If videoId > 0 Then
         Dim q = _VideoRepo.GetVideos.WithVideoID(videoId).FirstOrDefault
         If q IsNot Nothing Then
            q.Votes += 1
            q.TotalRating += rating
            'CacheObject.PurgeCacheItems("videos_videos_")
         End If
      End If
   End Sub

   'Public Function FindVideos(ByVal where As mccEnum.SearchType, ByVal SearchWord As String) As List(Of Video)
   '   Return FindVideos(where, 0, CacheObject.MaxRows, SearchWord)
   'End Function


   'Public Function FindVideos(ByVal where As mccEnum.SearchType, ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal SearchWord As String) As PagedList(Of Video)
   '   Dim li As New List(Of Video)
   '   Dim mdc As New MCCDataContext
   '   Select Case where
   '      Case mccEnum.SearchType.AnyWord
   '         Dim q = Video.ContainsAny(SearchWord.Split().ToArray)
   '         li = (From it As Video In mdc.videos.Where(q)).Distinct().ToList()
   '      Case mccEnum.SearchType.AllWords

   '      Case mccEnum.SearchType.ExactPhrase
   '         li = _VideoRepo.GetVideos.Published.Where(Function(it) it.Tags.Contains(SearchWord) Or it.Title.Contains(SearchWord) Or it.Abstract.Contains(SearchWord) _
   '                Or it.Body.Contains(SearchWord)).ToPagedList(startRowIndex, maximumRows)
   '   End Select
   '   Return li
   'End Function

   Public Function FindVideosWithExactMatch(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal SearchWord As String) As PagedList(Of Video) Implements IVideoService.FindVideosWithExactMatch
      Dim key As String = String.Format("video_search_{0}_{1}_{2}", SearchWord, startRowIndex, maximumRows)
      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), PagedList(Of Video))
      End If
      Dim li As PagedList(Of Video) = _VideoRepo.GetVideos.Published. _
      Where(Function(it) it.Tags.Contains(SearchWord) Or it.Title.Contains(SearchWord) Or it.Abstract.Contains(SearchWord)).ToPagedList(startRowIndex, maximumRows)

      CacheData(key, li)
      Return li
   End Function


   Public Function FindVideosWithAnyMatch(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal SearchWord As String) As PagedList(Of Video) Implements IVideoService.FindVideosWithAnyMatch
      Dim key As String = String.Format("video_search_{0}_{1}_{2}", SearchWord, startRowIndex, maximumRows)
      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), PagedList(Of Video))
      End If

      'Dim q = Video.ContainsAny(SearchWord.Split().ToArray)
      'li = (From it As Video In mdc.videos.Where(q)).Distinct().ToList()

        Dim searchKeys() As String = SearchWord.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)

      Dim li As PagedList(Of Video) = _VideoRepo.GetVideos.Published. _
            Where(Function(it) it.Tags.ContainsAny(searchKeys, True) Or it.Title.ContainsAny(searchKeys, True) Or it.Abstract.ContainsAny(searchKeys, True)). _
            ToPagedList(startRowIndex, maximumRows)

      '_VideoRepo.GetVideos.Published.Where(Function(p) p.ContainsAny)
      '_VideoRepo.GetVideosQuickList.Published.Where().ToPagedList(startRowIndex, maximumRows)

      CacheData(key, li)
      Return li
   End Function

    Public Function FindVideoWithExactMatchCount(ByVal SearchWord As String) As Integer Implements IVideoService.FindVideoWithExactMatchCount
        Return _VideoRepo.GetVideos.Published.Where(Function(it) it.Tags.Contains(SearchWord) Or it.Title.Contains(SearchWord) Or it.Abstract.Contains(SearchWord)).Count
    End Function

    Public Function FindVideoWithAnyMatchCount(ByVal SearchWord As String) As Integer Implements IVideoService.FindVideoWithAnyMatchCount
        Return _VideoRepo.GetVideos.Published.Where(Function(it) it.Tags.Contains(SearchWord) Or it.Title.Contains(SearchWord) Or it.Abstract.Contains(SearchWord)).Count
    End Function


   '''' <summary>
   '''' Returns the number of total videos matching the search key
   '''' </summary>
   'Public Function FindVideosCount(ByVal where As mccEnum.SearchType, ByVal searchWord As String) As Integer
   '   Dim artCount As Integer = 0
   '   Select Case where
   '      Case mccEnum.SearchType.AnyWord
   '         Dim q = Video.ContainsAny(searchWord.Split().ToArray)

   '         artCount = (From it As Video In mdc.videos.Where(q)).Count()
   '      Case mccEnum.SearchType.ExactPhrase
   '         artCount = _VideoRepo.GetVideos.Published.Count(Function(p) p.Tags.Contains(searchWord) Or p.Abstract.Contains(searchWord) _
   '                                                    Or p.Title.Contains(searchWord) Or p.Body.Contains(searchWord))
   '   End Select
   '   Return artCount
   'End Function

   Protected Sub CacheData(ByVal key As String, ByVal data As Object)
      If data IsNot Nothing Then
         CacheObject.Cache.Insert(key, data, Nothing, DateTime.Now.AddSeconds(3600), TimeSpan.Zero)
      End If
   End Sub
End Class

