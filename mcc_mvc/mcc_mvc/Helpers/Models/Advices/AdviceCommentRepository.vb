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
Imports Microsoft.VisualBasic
Imports System.Linq
Imports MCC.Dynamic
Imports MCC.Data

Namespace Advices
   Public Class AdviceCommentRepository
      Inherits mccObject

      Public Shared Function GetAdviceTitle(ByVal AdviceId As Integer) As String

         Dim key As String = "AdviceComments_AdviceComment_AdviceTitle_" & AdviceId.ToString

         If Cache(key) IsNot Nothing Then
            Return Cache(key)
         Else
            Dim mdc As MCCDataContext = New MCCDataContext
            Dim mtt As String = ""
            If (mdc.mcc_Advices.Count(Function(p) p.AdviceID = AdviceId) > 0) Then
               mtt = (From it As mcc_Advice In mdc.mcc_Advices Where it.AdviceID = AdviceId Select it.Title).Single()
               CacheData(key, mtt)
            End If

            Return mtt
         End If
      End Function

      Public Shared Function GetAdviceCommentCount() As Integer

         Dim key As String = "AdviceComments_AdviceCommentCount_"

         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            Dim mdc As New MCCDataContext()
            mdc.mcc_AdviceComments.Count()
            Dim it As Integer = mdc.mcc_AdviceComments.Count()
            CacheData(key, it)
            Return it
         End If
      End Function


      Public Shared Function GetAdviceCommentCount(ByVal id As Integer) As Integer
         If id > 0 Then

            Dim key As String = "AdviceComments_AdviceCommentCount_" & id.ToString & "_"

            If Cache(key) IsNot Nothing Then
               Return CInt(Cache(key))
            Else
               Dim mdc As New MCCDataContext()
               Dim it As Integer = mdc.mcc_AdviceComments.Count(Function(p) p.AdviceId = id)
               CacheData(key, it)
               Return it
            End If
         Else
            Return GetAdviceCommentCount()
         End If
      End Function

      Public Shared Function GetAdviceComments() As List(Of mcc_AdviceComment)
         Dim key As String = "AdviceComments_AdviceComments_"

         If Cache(key) IsNot Nothing Then
            Dim li As List(Of mcc_AdviceComment) = DirectCast(Cache(key), List(Of mcc_AdviceComment))
            Return li
         Else
            Dim mdc As New MCCDataContext()
            Dim li As List(Of mcc_AdviceComment) = mdc.mcc_AdviceComments.ToList
            CacheData(key, li)
            Return li
         End If
      End Function

      Public Shared Function GetAdviceComments(ByVal adviceId As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedDate DESC") As List(Of mcc_AdviceComment)
         If adviceId > 0 Then
            Dim key As String = "AdviceComments_AdviceComments_" & adviceId.ToString & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"

            If String.IsNullOrEmpty(sortExp) Then
               sortExp = "addedDate DESC"
            End If

            If Cache(key) IsNot Nothing Then
               Dim li As List(Of mcc_AdviceComment) = DirectCast(Cache(key), List(Of mcc_AdviceComment))
               Return li
            Else
               Dim mdc As New MCCDataContext()
               Dim li As List(Of mcc_AdviceComment)
               If startrowindex > 0 AndAlso maximumrows > 0 Then
                  li = (From it As mcc_AdviceComment In mdc.mcc_AdviceComments Where it.AdviceId = adviceId).SortBy(sortExp).Skip(startrowindex - 1).Take(maximumrows).ToList
               Else
                  li = (From it As mcc_AdviceComment In mdc.mcc_AdviceComments Where it.AdviceId = adviceId).SortBy(sortExp).Skip(0).Take(30).ToList
               End If
               CacheData(key, li)
               Return li
            End If
         Else
            Return Nothing
         End If
      End Function

      Public Shared Function GetAdviceCommentsByUsername(ByVal username As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedDate DESC") As List(Of mcc_AdviceComment)
         If Not String.IsNullOrEmpty(username) Then
            Dim key As String = "AdviceComments_AdviceComments_" & username.ToString & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"

            If String.IsNullOrEmpty(sortExp) Then
               sortExp = "addedDate DESC"
            End If
            If Cache(key) IsNot Nothing Then
               Dim li As List(Of mcc_AdviceComment) = DirectCast(Cache(key), List(Of mcc_AdviceComment))
               Return li
            Else
               Dim mdc As New MCCDataContext()
               Dim li As List(Of mcc_AdviceComment)
               If startrowindex > 0 AndAlso maximumrows > 0 Then
                  li = (From it As mcc_AdviceComment In mdc.mcc_AdviceComments Where it.AddedBy = username).SortBy(sortExp).Skip(startrowindex - 1).Take(maximumrows).ToList
               Else
                  li = (From it As mcc_AdviceComment In mdc.mcc_AdviceComments Where it.AddedBy = username).SortBy(sortExp).Skip(0).Take(30).ToList
               End If
               CacheData(key, li)
               Return li
            End If
         Else
            Return Nothing
         End If
      End Function


      Public Shared Function GetAdviceComments(ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedDate DESC") As List(Of mcc_AdviceComment)
         Dim key As String = "AdviceComments_AdviceComments_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"

         If String.IsNullOrEmpty(sortExp) Then
            sortExp = "addedDate DESC"
         End If

         If Cache(key) IsNot Nothing Then
            Dim li As List(Of mcc_AdviceComment) = DirectCast(Cache(key), List(Of mcc_AdviceComment))
            li.Sort(New CommentComparer(sortExp))
            Return li
         Else
            Dim mdc As New MCCDataContext()
            Dim li As List(Of mcc_AdviceComment)
            If startrowindex > 0 AndAlso maximumrows > 0 Then
               li = mdc.mcc_AdviceComments.SortBy(sortExp).Skip(startrowindex - 1).Take(maximumrows).ToList
            Else
               li = mdc.mcc_AdviceComments.SortBy(sortExp).Skip(0).Take(30).ToList
            End If
            CacheData(key, li)
            Return li
         End If
      End Function

      Public Shared Function GetAdviceCommentById(ByVal CommentId As Integer) As mcc_AdviceComment
         Dim key As String = "AdviceComments_AdviceComments_" & CommentId.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Dim fb As mcc_AdviceComment = DirectCast(Cache(key), mcc_AdviceComment)
            Return fb
         Else
            Dim mdc As New MCCDataContext()
            Dim fb As mcc_AdviceComment
            If mdc.mcc_AdviceComments.Count(Function(p) p.CommentID = CommentId) > 0 Then
               fb = (From it As mcc_AdviceComment In mdc.mcc_AdviceComments Where it.CommentID = CommentId).FirstOrDefault
               CacheData(key, fb)
               Return fb
            Else
               Return Nothing
            End If
         End If
      End Function



      Public Shared Sub InsertAdviceComment(ByVal adviceId As Integer, ByVal body As String)
         Dim mdc As MCCDataContext = New MCCDataContext
         Dim fb As New mcc_AdviceComment()


         Dim mUser As MembershipUser = Membership.GetUser()
         If mUser IsNot Nothing Then
            With fb
               .AddedDate = DateTime.Now
               .AddedBy = mccObject.CurrentUserName
               .AddedByEmail = mUser.Email
               .AddedbyIP = mccObject.CurrentUserIP
               .AdviceId = adviceId
               .Body = body
            End With

            mdc.mcc_AdviceComments.InsertOnSubmit(fb)
            mccObject.PurgeCacheItems("AdviceComments_")
            mdc.SubmitChanges()
         End If

      End Sub

      Public Shared Sub UpdateAdviceComment(ByVal commentId As Integer, ByVal body As String)
         Dim mdc As MCCDataContext = New MCCDataContext
         Dim wrd As mcc_AdviceComment = (From t In mdc.mcc_AdviceComments _
                                     Where t.CommentID = commentId _
                                     Select t).Single()
         If wrd IsNot Nothing Then
            wrd.Body = body
            mdc.SubmitChanges()
            mccObject.PurgeCacheItems("AdviceComments_AdviceComments_")
            mccObject.PurgeCacheItems("AdviceComments_AdviceCommentCount_")
         End If
      End Sub

      Public Shared Sub DeleteAdviceComment(ByVal commentId As Integer)
         Dim mdc As MCCDataContext = New MCCDataContext
         If mdc.mcc_AdviceComments.Count(Function(p) p.CommentID = commentId) > 0 Then
            Dim wrd As mcc_AdviceComment = (From t In mdc.mcc_AdviceComments _
                                        Where t.CommentID = commentId _
                                        Select t).Single()
            If wrd IsNot Nothing Then
               mdc.mcc_AdviceComments.DeleteOnSubmit(wrd)
               mdc.SubmitChanges()
               mccObject.PurgeCacheItems("AdviceComments_AdviceComments_")
            End If
         End If
      End Sub

      Public Shared Function FindComments(ByVal SearchWord As String) As List(Of mcc_AdviceComment)
         Dim comments As List(Of mcc_AdviceComment) = FindComments(0, mccObject.MaxRows, SearchWord)
         comments.Sort(New CommentComparer("AddedDate ASC"))
         Return comments
      End Function

      Public Shared Function FindComments(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal SearchWord As String) As List(Of mcc_AdviceComment)
         'Dim comments As List(Of AdviceComment) = Nothing


         'Dim recordset As List(Of AdviceCommentDetails) = SiteProvider.Advices.FindComments(GetPageIndex(startRowIndex, maximumRows), maximumRows, SearchWord)
         'comments = GetCommentListFromAdviceCommentDetailsList(recordset)

         Dim mdc As New MCCDataContext()
         'Dim iLst As List(Of FindCommentsResult) = mdc.FindComments(SearchWord).ToList

         'Dim li As List(Of mcc_AdviceComment) = (From it As mcc_AdviceComment In mdc.mcc_AdviceComments Join fts In mdc.FindComments(SearchWord) On it.CommentID Equals fts.CommentID Select it).ToList
         Dim li As New List(Of mcc_AdviceComment)

         Return li
      End Function

      ''' <summary>
      ''' Returns a collection with all comments for the specified article
      ''' </summary>
      Public Shared Function FindComments(ByVal articleID As Integer, ByVal SearchWord As String) As List(Of AdviceCommentRepository)
         Dim comments As List(Of AdviceCommentRepository) = FindComments(articleID, 0, mccObject.MaxRows, SearchWord)
         comments.Sort(New CommentComparer("AddedDate ASC"))
         Return comments
      End Function
      Public Shared Function FindComments(ByVal articleID As Integer, ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal SearchWord As String) As List(Of AdviceCommentRepository)
         Dim comments As List(Of AdviceCommentRepository) = Nothing
         Dim key As String = "Advices_Comments_" + articleID.ToString() + "_" + startRowIndex.ToString() + "_" + maximumRows.ToString()


         'Dim recordset As List(Of AdviceCommentDetails) = SiteProvider.Advices.FindComments(articleID, GetPageIndex(startRowIndex, maximumRows), maximumRows, SearchWord)
         'comments = GetCommentListFromAdviceCommentDetailsList(recordset)

         Return comments
      End Function

      '''' <summary>
      '''' Returns the number of total comments matching the search key
      '''' </summary>
      'Public Shared Function FindCommentsCount(ByVal searchWord As String) As Integer
      '   Dim commentCount As Integer = 0
      '   'commentCount = SiteProvider.Advices.FindCommentCount(searchWord)

      '   Dim mdc As New MCCDataContext()
      '   Dim cr As mcc_Advices_FindCommentsCountResult = mdc.mcc_Advices_FindCommentsCount(searchWord)
      '   commentCount = cr.Column1  ' single result value

      '   Return commentCount
      'End Function

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
      Implements IComparer(Of mcc_AdviceComment)
      Private _sortBy As String
      Private _reverse As Boolean

      Public Sub New(ByVal sortBy As String)
         If Not String.IsNullOrEmpty(sortBy) Then
            sortBy = sortBy.ToLower()
            _reverse = sortBy.EndsWith(" desc")
            _sortBy = sortBy.Replace(" desc", "").Replace(" asc", "")
         End If
      End Sub

      Public Overloads Function Equals(ByVal x As mcc_AdviceComment, ByVal y As mcc_AdviceComment) As Boolean
         Return (x.CommentID = y.CommentID)
      End Function

      Public Function Compare(ByVal x As mcc_AdviceComment, ByVal y As mcc_AdviceComment) As Integer Implements IComparer(Of mcc_AdviceComment).Compare
         Dim ret As Integer = 0
         Select Case _sortBy
            Case "addeddate"
               ret = DateTime.Compare(x.AddedDate, y.AddedDate)
               Exit Select
            Case "addedby"
               ret = String.Compare(x.AddedBy, y.AddedBy, StringComparison.InvariantCultureIgnoreCase)
               Exit Select
         End Select
         Return (ret * (IIf(_reverse, -1, 1)))
      End Function
   End Class
End Namespace
