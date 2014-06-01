Imports MCC.Data
Imports Webdiyer.WebControls.Mvc
Public Class VideoCommentService
   Inherits CacheObject
   Implements IVideoCommentService


   Private _videoCommentRepo As IVideoCommentRepository
   Private _videoRepo As IVideoRepository

   Public Sub New()
      Me.New(New VideoCommentRepository(), New VideoRepository())
   End Sub

   Public Sub New(ByVal _IVideoCommRepo As IVideoCommentRepository, ByVal _IVideoRepo As IVideoRepository)
      _videoCommentRepo = _IVideoCommRepo
      _videoRepo = _IVideoRepo
   End Sub

   Public Function GetVideoTitle(ByVal VideoId As Integer) As String Implements IVideoCommentService.GetVideoTitle

      Dim key As String = "VideoComments_VideoComment_VideoTitle_" & VideoId.ToString

      If Cache(key) IsNot Nothing Then
            Return CStr(Cache(key))
      Else
         Dim str As String = _videoRepo.GetVideos.WithVideoID(VideoId).Select(Function(p) p.Title).FirstOrDefault()
         CacheData(key, str)
         Return str
      End If
   End Function

   Public Function GetVideoCommentCount() As Integer Implements IVideoCommentService.GetVideoCommentCount

      Dim key As String = "VideoComments_VideoCommentCount_"

      If Cache(key) IsNot Nothing Then
         Return CInt(Cache(key))
      Else

         Dim it As Integer = _videoCommentRepo.GetVideoComments().Count
         CacheData(key, it)
         Return it
      End If
   End Function


   Public Function GetVideoCommentCount(ByVal id As Integer) As Integer Implements IVideoCommentService.GetVideoCommentCount
      If id > 0 Then

         Dim key As String = "VideoComments_VideoCommentCount_" & id.ToString & "_"

         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            Dim it As Integer = _videoCommentRepo.GetVideoComments.WithVideoID(id).Count()
            CacheData(key, it)
            Return it
         End If
      Else
         Return GetVideoCommentCount()
      End If
   End Function

   Public Function GetVideoComments() As List(Of VideoComment) Implements IVideoCommentService.GetVideoComments
      Dim key As String = "VideoComments_VideoComments_"

      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), List(Of VideoComment))
      Else
         Dim li As List(Of VideoComment) = _videoCommentRepo.GetVideoComments().ToList
         CacheData(key, li)
         Return li
      End If
   End Function

   Public Function GetVideoComments(ByVal videoId As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedDate DESC") As PagedList(Of VideoComment) Implements IVideoCommentService.GetVideoComments
      If videoId > 0 Then
         Dim key As String = "VideoComments_VideoComments_" & videoId.ToString & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"

         If String.IsNullOrEmpty(sortExp) Then
            sortExp = "addedDate DESC"
         End If

         If Cache(key) IsNot Nothing Then
                Dim li As PagedList(Of VideoComment) = DirectCast(Cache(key), PagedList(Of VideoComment))
            Return li
         Else
            Dim li As PagedList(Of VideoComment) = _videoCommentRepo.GetVideoComments.WithVideoID(videoId).ToPagedList(startrowindex, maximumrows)
            CacheData(key, li)
            Return li
         End If
      Else
         Return Nothing
      End If
   End Function

    Public Function GetVideoCommentsByUsername(ByVal username As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedDate DESC") As PagedList(Of VideoComment) Implements IVideoCommentService.GetVideoCommentsByUsername
        If Not String.IsNullOrEmpty(username) Then
            Dim key As String = "VideoComments_VideoComments_" & username.ToString & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"

            If String.IsNullOrEmpty(sortExp) Then
                sortExp = "addedDate DESC"
            End If
            If Cache(key) IsNot Nothing Then
                Return DirectCast(Cache(key), PagedList(Of VideoComment))
            Else

                Dim li As PagedList(Of VideoComment) = _videoCommentRepo.GetVideoComments. _
                Where(Function(p) p.AddedBy = username).ToPagedList(startrowindex, maximumrows)

                CacheData(key, li)
                Return li
            End If
        Else
            Return Nothing
        End If
    End Function


   Public Function GetVideoComments(ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedDate DESC") As PagedList(Of VideoComment) Implements IVideoCommentService.GetVideoComments
      Dim key As String = "VideoComments_VideoComments_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"

      If String.IsNullOrEmpty(sortExp) Then
         sortExp = "addedDate DESC"
      End If

      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), PagedList(Of VideoComment))
      Else
         Dim li As PagedList(Of VideoComment) = _videoCommentRepo.GetVideoComments.SortBy(sortExp).ToPagedList(startrowindex, maximumrows)
         CacheData(key, li)
         Return li
      End If
   End Function

   Public Function GetVideoCommentById(ByVal CommentId As Integer) As VideoComment Implements IVideoCommentService.GetVideoCommentById
      Dim key As String = "VideoComments_VideoComments_" & CommentId.ToString & "_"
      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), VideoComment)
      Else

         Dim fb As VideoComment = _videoCommentRepo.GetVideoComments.WithID(CommentId).FirstOrDefault
         CacheData(key, fb)
         Return fb
      End If
   End Function



   Public Sub InsertVideoComment(ByVal aComment As VideoComment) Implements IVideoCommentService.InsertVideoComment
      With aComment
         .AddedDate = DateTime.Now
         .AddedBy = CacheObject.CurrentUserName
         '.AddedByEmail = mUser.Email
         .AddedByIP = CacheObject.CurrentUserIP
         '.VideoID = videoId
         '.Body = body
      End With

      'mdc.mcc_Comments.InsertOnSubmit(fb)
      _videoCommentRepo.InsertComment(aComment)
      CacheObject.PurgeCacheItems("VideoComments_")
      ' mdc.SubmitChanges()

   End Sub

   Public Sub UpdateVideoComment(ByVal wrd As VideoComment) Implements IVideoCommentService.UpdateVideoComment
      'Dim mdc As MCCDataContext = New MCCDataContext
      'Dim wrd As mcc_Comment = (From t In mdc.mcc_Comments _
      '                            Where t.CommentID = commentId _
      '                            Select t).Single()
      If wrd IsNot Nothing Then
         'wrd.Body = body
         'mdc.SubmitChanges()

         _videoCommentRepo.InsertComment(wrd)

         CacheObject.PurgeCacheItems("VideoComments_VideoComments_")
         CacheObject.PurgeCacheItems("VideoComments_VideoCommentCount_")
      End If
   End Sub

   Public Sub DeleteVideoComment(ByVal commentId As Integer) Implements IVideoCommentService.DeleteVideoComment
      'Dim mdc As MCCDataContext = New MCCDataContext
      'If mdc.mcc_Comments.Count(Function(p) p.CommentID = commentId) > 0 Then
      '   Dim wrd As mcc_Comment = (From t In mdc.mcc_Comments _
      '                               Where t.CommentID = commentId _
      '                               Select t).Single()
      '   If wrd IsNot Nothing Then
      '      mdc.mcc_Comments.DeleteOnSubmit(wrd)
      '      mdc.SubmitChanges()
      '      CacheObject.PurgeCacheItems("VideoComments_VideoComments_")
      '   End If
      'End If

      _videoCommentRepo.DeleteComment(commentId)
      CacheObject.PurgeCacheItems("VideoComments_VideoComments_")
   End Sub

   'Public Function FindComments(ByVal searchType As mccEnum.SearchType, ByVal SearchWord As String) As List(Of mcc_Comment)
   '   Dim comments As List(Of mcc_Comment) = FindComments(searchType, 0, CacheObject.MaxRows, SearchWord)
   '   comments.Sort(New CommentComparer("AddedDate ASC"))
   '   Return comments
   'End Function

   'Public Function FindComments(ByVal searchType As mccEnum.SearchType, ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal SearchWord As String) As List(Of mcc_Comment)

   '   Dim li As New List(Of mcc_Comment)


   '   Select Case searchType
   '      Case mccEnum.SearchType.AnyWord
   '         li = (From it As mcc_Comment In mdc.mcc_Comments Where it.Body.Contains(SearchWord)).ToList
   '      Case mccEnum.SearchType.ExactPhrase
   '         li = (From it As mcc_Comment In mdc.mcc_Comments Where it.Body.Contains(SearchWord)).ToList
   '   End Select

   '   Return li
   'End Function


   Public Function FindCommentsWithExactMatch(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal SearchWord As String) As PagedList(Of VideoComment) Implements IVideoCommentService.FindCommentsWithExactMatch
      Return _videoCommentRepo.GetVideoComments.Where(Function(p) p.Body.Contains(SearchWord)).ToPagedList(startRowIndex, maximumRows)
   End Function


    Public Function FindCommentsWithAnyMatch(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal SearchWord As String) As PagedList(Of VideoComment) Implements IVideoCommentService.FindCommentsWithAnyMatch
        Return _videoCommentRepo.GetVideoComments.Where(Function(p) p.Body.Contains(SearchWord)).ToPagedList(startRowIndex, maximumRows)
    End Function


   '''' <summary>
   '''' Returns the number of total comments matching the search key
   '''' </summary>
   'Public Function FindCommentsCount(ByVal searchType As mccEnum.SearchType, ByVal searchWord As String) As Integer
   '   Dim commentCount As Integer = 0


   '   Select Case searchType
   '      Case mccEnum.SearchType.AnyWord
   '         Dim q = mcc_Comment.ContainsAny(searchWord.Split().ToArray)
   '         commentCount = (From it As mcc_Comment In mdc.mcc_Comments.Where(q)).Count()
   '      Case mccEnum.SearchType.ExactPhrase
   '         commentCount = mdc.mcc_Comments.Count(Function(p) p.Body.Contains(searchWord))
   '   End Select

   '   Return commentCount
   'End Function


    Function FindCommentsWithExactMatchCount(ByVal searchWord As String) As Integer Implements IVideoCommentService.FindCommentsWithExactMatchCount
        Return _videoCommentRepo.GetVideoComments.Where(Function(p) p.Body.Contains(searchWord)).Count()
    End Function

    Function FindCommentsWithAnyMatchCount(ByVal searchWord As String) As Integer Implements IVideoCommentService.FindCommentsWithAnyMatchCount
        Return _videoCommentRepo.GetVideoComments.Where(Function(p) p.Body.Contains(searchWord)).Count()
    End Function

   Protected Sub CacheData(ByVal key As String, ByVal data As Object)
      If data IsNot Nothing Then
         CacheObject.Cache.Insert(key, data, Nothing, DateTime.Now.AddSeconds(3600), TimeSpan.Zero)
      End If
   End Sub
End Class
