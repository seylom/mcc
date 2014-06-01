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
Imports Microsoft.VisualBasic
Imports System.Linq
Imports System.Linq.Expressions
Imports MCC.Data

Namespace Videos
   Public Class VideoCommentRepository
      Inherits mccObject

      Public Shared Function GetVideoTitle(ByVal VideoId As Integer) As String

         Dim key As String = "VideoComments_VideoComment_VideoTitle_" & VideoId.ToString

         If Cache(key) IsNot Nothing Then
            Return Cache(key)
         Else
            Dim mdc As MCCDataContext = New MCCDataContext
            Dim mtt As String = ""
            If (mdc.mcc_Videos.Count(Function(p) p.VideoId = VideoId) > 0) Then
               mtt = (From it As mcc_Video In mdc.mcc_Videos Where it.VideoId = VideoId Select it.Title).Single()
               CacheData(key, mtt)
            End If

            Return mtt
         End If
      End Function

      Public Shared Function GetVideoCommentCount() As Integer

         Dim key As String = "VideoComments_VideoCommentCount_"

         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            Dim mdc As New MCCDataContext()
            mdc.mcc_VideoComments.Count()
            Dim it As Integer = mdc.mcc_VideoComments.Count()
            CacheData(key, it)
            Return it
         End If
      End Function


      Public Shared Function GetVideoCommentCount(ByVal id As Integer) As Integer
         If id > 0 Then

            Dim key As String = "VideoComments_VideoCommentCount_" & id.ToString & "_"

            If Cache(key) IsNot Nothing Then
               Return CInt(Cache(key))
            Else
               Dim mdc As New MCCDataContext()
               Dim it As Integer = mdc.mcc_VideoComments.Count(Function(p) p.VideoID = id)
               CacheData(key, it)
               Return it
            End If
         Else
            Return GetVideoCommentCount()
         End If
      End Function

      Public Shared Function GetVideoComments() As List(Of mcc_VideoComment)
         Dim key As String = "VideoComments_VideoComments_"

         If Cache(key) IsNot Nothing Then
            Dim li As List(Of mcc_VideoComment) = DirectCast(Cache(key), List(Of mcc_VideoComment))
            Return li
         Else
            Dim mdc As New MCCDataContext()
            Dim li As List(Of mcc_VideoComment) = mdc.mcc_VideoComments.ToList
            CacheData(key, li)
            Return li
         End If
      End Function

      Public Shared Function GetVideoComments(ByVal VideoId As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedDate DESC") As List(Of mcc_VideoComment)
         If VideoId > 0 Then
            Dim key As String = "VideoComments_VideoComments_" & VideoId.ToString & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"

            If String.IsNullOrEmpty(sortExp) Then
               sortExp = "addedDate DESC"
            End If

            If Cache(key) IsNot Nothing Then
               Dim li As List(Of mcc_VideoComment) = DirectCast(Cache(key), List(Of mcc_VideoComment))
               li.Sort(New CommentComparer(sortExp))
               Return li
            Else
               Dim mdc As New MCCDataContext()
               Dim li As List(Of mcc_VideoComment)
               If startrowindex > 0 AndAlso maximumrows > 0 Then
                  li = (From it As mcc_VideoComment In mdc.mcc_VideoComments Where it.VideoID = VideoId).SortBy(sortExp).Skip(startrowindex - 1).Take(maximumrows).ToList
               Else
                  li = (From it As mcc_VideoComment In mdc.mcc_VideoComments Where it.VideoID = VideoId).SortBy(sortExp).Skip(0).Take(30).ToList
               End If
               CacheData(key, li)
               Return li
            End If
         Else
            Return Nothing
         End If
      End Function

      Public Shared Function GetVideoCommentsByUsername(ByVal username As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedDate DESC") As List(Of mcc_VideoComment)
         If Not String.IsNullOrEmpty(username) Then
            Dim key As String = "VideoComments_VideoComments_" & username.ToString & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"

            If String.IsNullOrEmpty(sortExp) Then
               sortExp = "addedDate DESC"
            End If
            If Cache(key) IsNot Nothing Then
               Dim li As List(Of mcc_VideoComment) = DirectCast(Cache(key), List(Of mcc_VideoComment))
               li.Sort(New CommentComparer(sortExp))
               Return li
            Else
               Dim mdc As New MCCDataContext()
               Dim li As List(Of mcc_VideoComment)
               If startrowindex > 0 AndAlso maximumrows > 0 Then
                  li = (From it As mcc_VideoComment In mdc.mcc_VideoComments Where it.Addedby = username).SortBy(sortExp).Skip(startrowindex - 1).Take(maximumrows).ToList
               Else
                  li = (From it As mcc_VideoComment In mdc.mcc_VideoComments Where it.Addedby = username).SortBy(sortExp).Skip(0).Take(30).ToList
               End If
               CacheData(key, li)
               Return li
            End If
         Else
            Return Nothing
         End If
      End Function


      Public Shared Function GetVideoComments(ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedDate DESC") As List(Of mcc_VideoComment)
         Dim key As String = "VideoComments_VideoComments_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"

         If String.IsNullOrEmpty(sortExp) Then
            sortExp = "addedDate DESC"
         End If

         If Cache(key) IsNot Nothing Then
            Dim li As List(Of mcc_VideoComment) = DirectCast(Cache(key), List(Of mcc_VideoComment))
            li.Sort(New CommentComparer(sortExp))
            Return li
         Else
            Dim mdc As New MCCDataContext()
            Dim li As List(Of mcc_VideoComment)
            If startrowindex > 0 AndAlso maximumrows > 0 Then
               li = mdc.mcc_VideoComments.SortBy(sortExp).Skip(startrowindex - 1).Take(maximumrows).ToList
            Else
               li = mdc.mcc_VideoComments.SortBy(sortExp).Skip(0).Take(30).ToList
            End If
            CacheData(key, li)
            Return li
         End If
      End Function

      Public Shared Function GetVideoCommentById(ByVal CommentId As Integer) As mcc_VideoComment
         Dim key As String = "VideoComments_VideoComments_" & CommentId.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Dim fb As mcc_VideoComment = DirectCast(Cache(key), mcc_VideoComment)
            Return fb
         Else
            Dim mdc As New MCCDataContext()
            Dim fb As mcc_VideoComment
            If mdc.mcc_VideoComments.Count(Function(p) p.CommentID = CommentId) > 0 Then
               fb = (From it As mcc_VideoComment In mdc.mcc_VideoComments Where it.CommentID = CommentId).FirstOrDefault
               CacheData(key, fb)
               Return fb
            Else
               Return Nothing
            End If
         End If
      End Function



      Public Shared Sub InsertVideoComment(ByVal VideoId As Integer, ByVal body As String)
         Dim mdc As MCCDataContext = New MCCDataContext
         Dim fb As New mcc_VideoComment()


         Dim mUser As MembershipUser = Membership.GetUser()
         If mUser IsNot Nothing Then
            With fb
               .AddedDate = DateTime.Now
               .Addedby = mccObject.CurrentUserName
               .AddedbyEmail = mUser.Email
               .AddedbyIP = mccObject.CurrentUserIP
               .VideoID = VideoId
               .Body = body
            End With

            mdc.mcc_VideoComments.InsertOnSubmit(fb)
            mccObject.PurgeCacheItems("VideoComments_")
            mdc.SubmitChanges()
         End If

      End Sub

      Public Shared Sub UpdateVideoComment(ByVal commentId As Integer, ByVal body As String)
         Dim mdc As MCCDataContext = New MCCDataContext
         Dim wrd As mcc_VideoComment = (From t In mdc.mcc_VideoComments _
                                     Where t.CommentID = commentId _
                                     Select t).Single()
         If wrd IsNot Nothing Then
            wrd.Body = body
            mdc.SubmitChanges()
            mccObject.PurgeCacheItems("VideoComments_VideoComments_")
            mccObject.PurgeCacheItems("VideoComments_VideoCommentCount_")
         End If
      End Sub

      Public Shared Sub DeleteVideoComment(ByVal commentId As Integer)
         Dim mdc As MCCDataContext = New MCCDataContext
         If mdc.mcc_VideoComments.Count(Function(p) p.CommentID = commentId) > 0 Then
            Dim wrd As mcc_VideoComment = (From t In mdc.mcc_VideoComments _
                                        Where t.CommentID = commentId _
                                        Select t).Single()
            If wrd IsNot Nothing Then
               mdc.mcc_VideoComments.DeleteOnSubmit(wrd)
               mdc.SubmitChanges()
               mccObject.PurgeCacheItems("VideoComments_VideoComments_")
            End If
         End If
      End Sub

      Public Shared Function FindComments(ByVal searchType As mccEnum.SearchType, ByVal SearchWord As String) As List(Of mcc_VideoComment)
         Dim comments As List(Of mcc_VideoComment) = FindComments(searchType, 0, mccObject.MaxRows, SearchWord)
         comments.Sort(New CommentComparer("AddedDate ASC"))
         Return comments
      End Function

      Public Shared Function FindComments(ByVal searchType As mccEnum.SearchType, ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal SearchWord As String) As List(Of mcc_VideoComment)

         Dim mdc As New MCCDataContext()
         Dim li As New List(Of mcc_VideoComment)

         Select Case searchType
            Case mccEnum.SearchType.AnyWord
               li = (From it As mcc_VideoComment In mdc.mcc_VideoComments Where it.Body.Contains(SearchWord)).ToList
            Case mccEnum.SearchType.ExactPhrase
               li = (From it As mcc_VideoComment In mdc.mcc_VideoComments Where it.Body.Contains(SearchWord)).ToList
         End Select

         Return li
      End Function

      ''' <summary>
      ''' Returns a collection with all comments for the specified Video
      ''' </summary>
      Public Shared Function FindComments(ByVal VideoID As Integer, ByVal SearchWord As String) As List(Of VideoCommentRepository)
         Dim comments As List(Of VideoCommentRepository) = FindComments(VideoID, 0, mccObject.MaxRows, SearchWord)
         comments.Sort(New CommentComparer("AddedDate ASC"))
         Return comments
      End Function
      Public Shared Function FindComments(ByVal VideoID As Integer, ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal SearchWord As String) As List(Of VideoCommentRepository)
         Dim comments As List(Of VideoCommentRepository) = Nothing
         Dim key As String = "Videos_Comments_" + VideoID.ToString() + "_" + startRowIndex.ToString() + "_" + maximumRows.ToString()


         'Dim recordset As List(Of VideoCommentDetails) = SiteProvider.Videos.FindComments(VideoID, GetPageIndex(startRowIndex, maximumRows), maximumRows, SearchWord)
         'comments = GetCommentListFromVideoCommentDetailsList(recordset)

         Return comments
      End Function

      ''' <summary>
      ''' Returns the number of total comments matching the search key
      ''' </summary>
      Public Shared Function FindCommentsCount(ByVal searchType As mccEnum.SearchType, ByVal searchWord As String) As Integer
         Dim commentCount As Integer = 0

         Dim mdc As New MCCDataContext()

         'Select Case searchType
         '   Case mccEnum.SearchType.AnyWord
         '      Dim q = mcc_Video.ContainsAny(searchWord.Split().ToArray)
         '      commentCount = (From it As mcc_Video In mdc.mcc_Videos.Where(q)).Count()
         '   Case mccEnum.SearchType.ExactPhrase
         '      commentCount = mdc.mcc_Videos.Count(Function(p) p.Title.Contains(searchWord))
         'End Select

         Return commentCount
      End Function

      Protected Shared Sub CacheData(ByVal key As String, ByVal data As Object)
         If data IsNot Nothing Then
            mccObject.Cache.Insert(key, data, Nothing, DateTime.Now.AddSeconds(3600), TimeSpan.Zero)
         End If
      End Sub
   End Class

   ''' <summary>
   ''' 
   ''' </summary>
   ''' <remarks></remarks>
   Public Class CommentComparer
      Implements IComparer(Of mcc_VideoComment)
      Private _sortBy As String
      Private _reverse As Boolean

      Public Sub New(ByVal sortBy As String)
         If Not String.IsNullOrEmpty(sortBy) Then
            sortBy = sortBy.ToLower()
            _reverse = sortBy.EndsWith(" desc")
            _sortBy = sortBy.Replace(" desc", "").Replace(" asc", "")
         End If
      End Sub

      Public Overloads Function Equals(ByVal x As mcc_VideoComment, ByVal y As mcc_VideoComment) As Boolean
         Return (x.CommentID = y.CommentID)
      End Function

      Public Function Compare(ByVal x As mcc_VideoComment, ByVal y As mcc_VideoComment) As Integer Implements IComparer(Of mcc_VideoComment).Compare
         Dim ret As Integer = 0
         Select Case _sortBy
            Case "addeddate"
               ret = DateTime.Compare(x.AddedDate, y.AddedDate)
               Exit Select
            Case "addedby"
               ret = String.Compare(x.Addedby, y.Addedby, StringComparison.InvariantCultureIgnoreCase)
               Exit Select
         End Select
         Return (ret * (IIf(_reverse, -1, 1)))
      End Function
   End Class
End Namespace

Namespace MCC.Data
   Partial Class mcc_VideoComment
      Public Shared Function ContainsAny(ByVal ParamArray keywords As String()) As Expression(Of Func(Of mcc_VideoComment, Boolean))
         Dim predicate = PredicateBuilder.[False](Of mcc_VideoComment)()
         'For Each keyword As String In keywords
         '   Dim temp As String = keyword
         '   predicate = predicate.[Or](Function(p) p.Body.Contains(temp))
         'Next
         Return predicate
      End Function
   End Class
End Namespace
