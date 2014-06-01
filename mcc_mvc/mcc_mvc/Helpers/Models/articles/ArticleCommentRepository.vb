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
Imports MCC.Dynamic
Imports System.Linq.Expressions

Namespace Articles
   Public Class ArticleCommentRepository
      Inherits mccObject

      Public Shared Function GetArticleTitle(ByVal ArticleId As Integer) As String

         Dim key As String = "ArticleComments_ArticleComment_ArticleTitle_" & ArticleId.ToString

         If Cache(key) IsNot Nothing Then
            Return Cache(key)
         Else
            Dim mdc As MCCDataContext = New MCCDataContext
            Dim mtt As String = ""
            If (mdc.mcc_Articles.Count(Function(p) p.ArticleID = ArticleId) > 0) Then
               mtt = (From it As mcc_Article In mdc.mcc_Articles Where it.ArticleID = ArticleId Select it.Title).Single()
               CacheData(key, mtt)
            End If

            Return mtt
         End If
      End Function

      Public Shared Function GetArticleCommentCount() As Integer

         Dim key As String = "ArticleComments_ArticleCommentCount_"

         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            Dim mdc As New MCCDataContext()
            Dim it As Integer = mdc.mcc_Comments.Count()
            CacheData(key, it)
            Return it
         End If
      End Function


      Public Shared Function GetArticleCommentCount(ByVal id As Integer) As Integer
         If id > 0 Then

            Dim key As String = "ArticleComments_ArticleCommentCount_" & id.ToString & "_"

            If Cache(key) IsNot Nothing Then
               Return CInt(Cache(key))
            Else
               Dim mdc As New MCCDataContext()
               Dim it As Integer = mdc.mcc_Comments.Count(Function(p) p.ArticleID = id)
               CacheData(key, it)
               Return it
            End If
         Else
            Return GetArticleCommentCount()
         End If
      End Function

      Public Shared Function GetArticleComments() As List(Of mcc_Comment)
         Dim key As String = "ArticleComments_ArticleComments_"

         If Cache(key) IsNot Nothing Then
            Dim li As List(Of mcc_Comment) = DirectCast(Cache(key), List(Of mcc_Comment))
            Return li
         Else
            Dim mdc As New MCCDataContext()
            Dim li As List(Of mcc_Comment) = mdc.mcc_Comments.ToList
            CacheData(key, li)
            Return li
         End If
      End Function

      Public Shared Function GetArticleComments(ByVal articleId As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedDate DESC") As List(Of mcc_Comment)
         If articleId > 0 Then
            Dim key As String = "ArticleComments_ArticleComments_" & articleId.ToString & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"

            If String.IsNullOrEmpty(sortExp) Then
               sortExp = "addedDate DESC"
            End If

            If Cache(key) IsNot Nothing Then
               Dim li As List(Of mcc_Comment) = DirectCast(Cache(key), List(Of mcc_Comment))
               li.Sort(New CommentComparer(sortExp))
               Return li
            Else
               Dim mdc As New MCCDataContext()
               Dim li As List(Of mcc_Comment)
               If startrowindex > 0 AndAlso maximumrows > 0 Then
                  li = (From it As mcc_Comment In mdc.mcc_Comments Where it.ArticleID = articleId).SortBy(sortExp).Skip(startrowindex - 1).Take(maximumrows).ToList
               Else
                  li = (From it As mcc_Comment In mdc.mcc_Comments Where it.ArticleID = articleId).SortBy(sortExp).Skip(0).Take(30).ToList
               End If
               CacheData(key, li)
               Return li
            End If
         Else
            Return Nothing
         End If
      End Function

      Public Shared Function GetArticleCommentsByUsername(ByVal username As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedDate DESC") As List(Of mcc_Comment)
         If Not String.IsNullOrEmpty(username) Then
            Dim key As String = "ArticleComments_ArticleComments_" & username.ToString & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"

            If String.IsNullOrEmpty(sortExp) Then
               sortExp = "addedDate DESC"
            End If
            If Cache(key) IsNot Nothing Then
               Dim li As List(Of mcc_Comment) = DirectCast(Cache(key), List(Of mcc_Comment))
               li.Sort(New CommentComparer(sortExp))
               Return li
            Else
               Dim mdc As New MCCDataContext()
               Dim li As List(Of mcc_Comment)
               If startrowindex > 0 AndAlso maximumrows > 0 Then
                  li = (From it As mcc_Comment In mdc.mcc_Comments Where it.AddedBy = username).SortBy(sortExp).Skip(startrowindex - 1).Take(maximumrows).ToList
               Else
                  li = (From it As mcc_Comment In mdc.mcc_Comments Where it.AddedBy = username).SortBy(sortExp).Skip(0).Take(30).ToList
               End If
               CacheData(key, li)
               Return li
            End If
         Else
            Return Nothing
         End If
      End Function


      Public Shared Function GetArticleComments(ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedDate DESC") As List(Of mcc_Comment)
         Dim key As String = "ArticleComments_ArticleComments_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"

         If String.IsNullOrEmpty(sortExp) Then
            sortExp = "addedDate DESC"
         End If

         If Cache(key) IsNot Nothing Then
            Dim li As List(Of mcc_Comment) = DirectCast(Cache(key), List(Of mcc_Comment))
            li.Sort(New CommentComparer(sortExp))
            Return li
         Else
            Dim mdc As New MCCDataContext()
            Dim li As List(Of mcc_Comment)
            If startrowindex > 0 AndAlso maximumrows > 0 Then
               li = mdc.mcc_Comments.SortBy(sortExp).Skip(startrowindex - 1).Take(maximumrows).ToList
            Else
               li = mdc.mcc_Comments.SortBy(sortExp).Skip(0).Take(30).ToList
            End If
            CacheData(key, li)
            Return li
         End If
      End Function

      Public Shared Function GetArticleCommentById(ByVal CommentId As Integer) As mcc_Comment
         Dim key As String = "ArticleComments_ArticleComments_" & CommentId.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Dim fb As mcc_Comment = DirectCast(Cache(key), mcc_Comment)
            Return fb
         Else
            Dim mdc As New MCCDataContext()
            Dim fb As mcc_Comment
            If mdc.mcc_Comments.Count(Function(p) p.CommentID = CommentId) > 0 Then
               fb = (From it As mcc_Comment In mdc.mcc_Comments Where it.CommentID = CommentId).FirstOrDefault
               CacheData(key, fb)
               Return fb
            Else
               Return Nothing
            End If
         End If
      End Function



      Public Shared Sub InsertArticleComment(ByVal articleId As Integer, ByVal body As String)
         Dim mdc As MCCDataContext = New MCCDataContext
         Dim fb As New mcc_Comment()


         Dim mUser As MembershipUser = Membership.GetUser()
         If mUser IsNot Nothing Then
            With fb
               .AddedDate = DateTime.Now
               .AddedBy = mccObject.CurrentUserName
               .AddedByEmail = mUser.Email
               .AddedByIP = mccObject.CurrentUserIP
               .ArticleID = articleId
               .Body = body
            End With

            mdc.mcc_Comments.InsertOnSubmit(fb)
            mccObject.PurgeCacheItems("ArticleComments_")
            mdc.SubmitChanges()
         End If

      End Sub

      Public Shared Sub UpdateArticleComment(ByVal commentId As Integer, ByVal body As String)
         Dim mdc As MCCDataContext = New MCCDataContext
         Dim wrd As mcc_Comment = (From t In mdc.mcc_Comments _
                                     Where t.CommentID = commentId _
                                     Select t).Single()
         If wrd IsNot Nothing Then
            wrd.Body = body
            mdc.SubmitChanges()
            mccObject.PurgeCacheItems("ArticleComments_ArticleComments_")
            mccObject.PurgeCacheItems("ArticleComments_ArticleCommentCount_")
         End If
      End Sub

      Public Shared Sub DeleteArticleComment(ByVal commentId As Integer)
         Dim mdc As MCCDataContext = New MCCDataContext
         If mdc.mcc_Comments.Count(Function(p) p.CommentID = commentId) > 0 Then
            Dim wrd As mcc_Comment = (From t In mdc.mcc_Comments _
                                        Where t.CommentID = commentId _
                                        Select t).Single()
            If wrd IsNot Nothing Then
               mdc.mcc_Comments.DeleteOnSubmit(wrd)
               mdc.SubmitChanges()
               mccObject.PurgeCacheItems("ArticleComments_ArticleComments_")
            End If
         End If
      End Sub

      Public Shared Function FindComments(ByVal searchType As mccEnum.SearchType, ByVal SearchWord As String) As List(Of mcc_Comment)
         Dim comments As List(Of mcc_Comment) = FindComments(searchType, 0, mccObject.MaxRows, SearchWord)
         comments.Sort(New CommentComparer("AddedDate ASC"))
         Return comments
      End Function

      Public Shared Function FindComments(ByVal searchType As mccEnum.SearchType, ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal SearchWord As String) As List(Of mcc_Comment)

         Dim li As New List(Of mcc_Comment)
         Dim mdc As New MCCDataContext()

         Select Case searchType
            Case mccEnum.SearchType.AnyWord
               li = (From it As mcc_Comment In mdc.mcc_Comments Where it.Body.Contains(SearchWord)).ToList
            Case mccEnum.SearchType.ExactPhrase
               li = (From it As mcc_Comment In mdc.mcc_Comments Where it.Body.Contains(SearchWord)).ToList
         End Select

         Return li
      End Function

      '''' <summary>
      '''' Returns a collection with all comments for the specified article
      '''' </summary>
      'Public Shared Function FindComments(ByVal articleID As Integer, ByVal SearchWord As String) As List(Of ArticleComment)
      '   Dim comments As List(Of ArticleComment) = FindComments(articleID, 0, mccObject.MaxRows, SearchWord)
      '   comments.Sort(New CommentComparer("AddedDate ASC"))
      '   Return comments
      'End Function
      'Public Shared Function FindComments(ByVal articleID As Integer, ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal SearchWord As String) As List(Of ArticleComment)
      '   Dim comments As List(Of ArticleComment) = Nothing
      '   Dim key As String = "Articles_Comments_" + articleID.ToString() + "_" + startRowIndex.ToString() + "_" + maximumRows.ToString()


      '   'Dim recordset As List(Of ArticleCommentDetails) = SiteProvider.Articles.FindComments(articleID, GetPageIndex(startRowIndex, maximumRows), maximumRows, SearchWord)
      '   'comments = GetCommentListFromArticleCommentDetailsList(recordset)

      '   Return comments
      'End Function

      ''' <summary>
      ''' Returns the number of total comments matching the search key
      ''' </summary>
      Public Shared Function FindCommentsCount(ByVal searchType As mccEnum.SearchType, ByVal searchWord As String) As Integer
         Dim commentCount As Integer = 0
         Dim mdc As New MCCDataContext()

         Select Case searchType
            Case mccEnum.SearchType.AnyWord
               Dim q = mcc_Comment.ContainsAny(searchWord.Split().ToArray)
               commentCount = (From it As mcc_Comment In mdc.mcc_Comments.Where(q)).Count()
            Case mccEnum.SearchType.ExactPhrase
               commentCount = mdc.mcc_Comments.Count(Function(p) p.Body.Contains(searchWord))
         End Select

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
      Implements IComparer(Of mcc_Comment)
      Private _sortBy As String
      Private _reverse As Boolean

      Public Sub New(ByVal sortBy As String)
         If Not String.IsNullOrEmpty(sortBy) Then
            sortBy = sortBy.ToLower()
            _reverse = sortBy.EndsWith(" desc")
            _sortBy = sortBy.Replace(" desc", "").Replace(" asc", "")
         End If
      End Sub

      Public Overloads Function Equals(ByVal x As mcc_Comment, ByVal y As mcc_Comment) As Boolean
         Return (x.CommentID = y.CommentID)
      End Function

      Public Function Compare(ByVal x As mcc_Comment, ByVal y As mcc_Comment) As Integer Implements IComparer(Of mcc_Comment).Compare
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

Partial Class mcc_Comment
   Public Shared Function ContainsAny(ByVal ParamArray keywords As String()) As Expression(Of Func(Of mcc_Comment, Boolean))
      Dim predicate = PredicateBuilder.[False](Of mcc_Comment)()
      For Each keyword As String In keywords
         Dim temp As String = keyword
         predicate = predicate.[Or](Function(p) p.Body.Contains(temp))
      Next
      Return predicate
   End Function
End Class
