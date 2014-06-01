Imports MCC.Data
Imports Webdiyer.WebControls.Mvc
Public Class AdviceCommentService
   Inherits CacheObject
   Implements IAdviceCommentService


   Private _adviceCommentRepo As IAdviceCommentRepository
   Private _adviceRepo As IAdviceRepository

   Public Sub New()
      Me.New(New AdviceCommentRepository(), New AdviceRepository())
   End Sub

    Public Sub New(ByVal _IAdviceCommRepo As IAdviceCommentRepository, ByVal _IAdviceRepo As IAdviceRepository)
        _adviceCommentRepo = _IAdviceCommRepo
        _adviceRepo = _IAdviceRepo
    End Sub

   Public Function GetAdviceTitle(ByVal AdviceId As Integer) As String Implements IAdviceCommentService.GetAdviceTitle

      Dim key As String = "AdviceComments_AdviceComment_AdviceTitle_" & AdviceId.ToString

      If Cache(key) IsNot Nothing Then
            Return CStr(Cache(key))
      Else
         Dim str As String = _adviceRepo.GetAdvices.WithAdviceID(AdviceId).Select(Function(p) p.Title).FirstOrDefault()
         CacheData(key, str)
         Return str
      End If
   End Function

   Public Function GetAdviceCommentCount() As Integer Implements IAdviceCommentService.GetAdviceCommentCount

      Dim key As String = "AdviceComments_AdviceCommentCount_"

      If Cache(key) IsNot Nothing Then
         Return CInt(Cache(key))
      Else

         Dim it As Integer = _adviceCommentRepo.GetAdviceComments().Count
         CacheData(key, it)
         Return it
      End If
   End Function


   Public Function GetAdviceCommentCount(ByVal id As Integer) As Integer Implements IAdviceCommentService.GetAdviceCommentCount
      If id > 0 Then

         Dim key As String = "AdviceComments_AdviceCommentCount_" & id.ToString & "_"

         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            Dim it As Integer = _adviceCommentRepo.GetAdviceComments.WithAdviceID(id).Count()
            CacheData(key, it)
            Return it
         End If
      Else
         Return GetAdviceCommentCount()
      End If
   End Function

   Public Function GetAdviceComments() As List(Of AdviceComment) Implements IAdviceCommentService.GetAdviceComments
      Dim key As String = "AdviceComments_AdviceComments_"

      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), List(Of AdviceComment))
      Else
         Dim li As List(Of AdviceComment) = _adviceCommentRepo.GetAdviceComments().ToList
         CacheData(key, li)
         Return li
      End If
   End Function

   Public Function GetAdviceComments(ByVal adviceId As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedDate DESC") As PagedList(Of AdviceComment) Implements IAdviceCommentService.GetAdviceComments
      If adviceId > 0 Then
         Dim key As String = "AdviceComments_AdviceComments_" & adviceId.ToString & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"

         If String.IsNullOrEmpty(sortExp) Then
            sortExp = "addedDate DESC"
         End If

         If Cache(key) IsNot Nothing Then
                Dim li As PagedList(Of AdviceComment) = DirectCast(Cache(key), PagedList(Of AdviceComment))
            Return li
         Else
            Dim li As PagedList(Of AdviceComment) = _adviceCommentRepo.GetAdviceComments.WithAdviceID(adviceId).ToPagedList(startrowindex, maximumrows)
            CacheData(key, li)
            Return li
         End If
      Else
         Return Nothing
      End If
   End Function

    Public Function GetAdviceCommentsByUsername(ByVal username As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedDate DESC") As PagedList(Of AdviceComment) Implements IAdviceCommentService.GetAdviceCommentsByUsername
        If Not String.IsNullOrEmpty(username) Then
            Dim key As String = "AdviceComments_AdviceComments_" & username.ToString & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"

            If String.IsNullOrEmpty(sortExp) Then
                sortExp = "addedDate DESC"
            End If
            If Cache(key) IsNot Nothing Then
                Return DirectCast(Cache(key), PagedList(Of AdviceComment))
            Else

                Dim li As PagedList(Of AdviceComment) = _adviceCommentRepo.GetAdviceComments. _
                Where(Function(p) p.AddedBy = username).ToPagedList(startrowindex, maximumrows)

                CacheData(key, li)
                Return li
            End If
        Else
            Return Nothing
        End If
    End Function


   Public Function GetAdviceComments(ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedDate DESC") As PagedList(Of AdviceComment) Implements IAdviceCommentService.GetAdviceComments
      Dim key As String = "AdviceComments_AdviceComments_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"

      If String.IsNullOrEmpty(sortExp) Then
         sortExp = "addedDate DESC"
      End If

      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), PagedList(Of AdviceComment))
      Else
         Dim li As PagedList(Of AdviceComment) = _adviceCommentRepo.GetAdviceComments.SortBy(sortExp).ToPagedList(startrowindex, maximumrows)
         CacheData(key, li)
         Return li
      End If
   End Function

   Public Function GetAdviceCommentById(ByVal CommentId As Integer) As AdviceComment Implements IAdviceCommentService.GetAdviceCommentById
      Dim key As String = "AdviceComments_AdviceComments_" & CommentId.ToString & "_"
      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), AdviceComment)
      Else

         Dim fb As AdviceComment = _adviceCommentRepo.GetAdviceComments.WithID(CommentId).FirstOrDefault
         CacheData(key, fb)
         Return fb
      End If
   End Function



   Public Sub InsertAdviceComment(ByVal aComment As AdviceComment) Implements IAdviceCommentService.InsertAdviceComment
      With aComment
         .AddedDate = DateTime.Now
         .AddedBy = CacheObject.CurrentUserName
         '.AddedByEmail = mUser.Email
         .AddedByIP = CacheObject.CurrentUserIP
         '.AdviceID = adviceId
         '.Body = body
      End With

      'mdc.mcc_Comments.InsertOnSubmit(fb)
      _adviceCommentRepo.InsertComment(aComment)
      CacheObject.PurgeCacheItems("AdviceComments_")
      ' mdc.SubmitChanges()

   End Sub

   Public Sub UpdateAdviceComment(ByVal wrd As AdviceComment) Implements IAdviceCommentService.UpdateAdviceComment
      'Dim mdc As MCCDataContext = New MCCDataContext
      'Dim wrd As mcc_Comment = (From t In mdc.mcc_Comments _
      '                            Where t.CommentID = commentId _
      '                            Select t).Single()
      If wrd IsNot Nothing Then
         'wrd.Body = body
         'mdc.SubmitChanges()

         _adviceCommentRepo.InsertComment(wrd)

         CacheObject.PurgeCacheItems("AdviceComments_AdviceComments_")
         CacheObject.PurgeCacheItems("AdviceComments_AdviceCommentCount_")
      End If
   End Sub

   Public Sub DeleteAdviceComment(ByVal commentId As Integer) Implements IAdviceCommentService.DeleteAdviceComment
      'Dim mdc As MCCDataContext = New MCCDataContext
      'If mdc.mcc_Comments.Count(Function(p) p.CommentID = commentId) > 0 Then
      '   Dim wrd As mcc_Comment = (From t In mdc.mcc_Comments _
      '                               Where t.CommentID = commentId _
      '                               Select t).Single()
      '   If wrd IsNot Nothing Then
      '      mdc.mcc_Comments.DeleteOnSubmit(wrd)
      '      mdc.SubmitChanges()
      '      CacheObject.PurgeCacheItems("AdviceComments_AdviceComments_")
      '   End If
      'End If

      _adviceCommentRepo.DeleteComment(commentId)
      CacheObject.PurgeCacheItems("AdviceComments_AdviceComments_")
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


   Public Function FindCommentsWithExactMatch(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal SearchWord As String) As PagedList(Of AdviceComment) Implements IAdviceCommentService.FindCommentsWithExactMatch
      Return _adviceCommentRepo.GetAdviceComments.Where(Function(p) p.Body.Contains(SearchWord)).ToPagedList(startRowIndex, maximumRows)
   End Function


    Public Function FindCommentsWithAnyMatch(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal SearchWord As String) As PagedList(Of AdviceComment) Implements IAdviceCommentService.FindCommentsWithAnyMatch
        Return _adviceCommentRepo.GetAdviceComments.Where(Function(p) p.Body.Contains(SearchWord)).ToPagedList(startRowIndex, maximumRows)
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


    Function FindCommentsWithExactMatchCount(ByVal searchWord As String) As Integer Implements IAdviceCommentService.FindCommentsWithExactMatchCount
        Return _adviceCommentRepo.GetAdviceComments.Where(Function(p) p.Body.Contains(searchWord)).Count()
    End Function

    Function FindCommentsWithAnyMatchCount(ByVal searchWord As String) As Integer Implements IAdviceCommentService.FindCommentsWithAnyMatchCount
        Return _adviceCommentRepo.GetAdviceComments.Where(Function(p) p.Body.Contains(searchWord)).Count()
    End Function

   Protected Sub CacheData(ByVal key As String, ByVal data As Object)
      If data IsNot Nothing Then
         CacheObject.Cache.Insert(key, data, Nothing, DateTime.Now.AddSeconds(3600), TimeSpan.Zero)
      End If
   End Sub
End Class
