Imports Microsoft.VisualBasic
Imports System.Linq
Imports Dynamic
Imports MCC.Data
Namespace Wikis
   Public Class WikiRepository
      Inherits mccObject

      Public Shared Function GetWikiPageCount() As Integer
         Dim mdc As New MCCDataContext()
         Dim key As String = "WikiPages_WikiPageCount"

         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            mdc.mcc_WikiPages.Count()
            Dim it As Integer = mdc.mcc_WikiPages.Count()
            CacheData(key, it)
            Return it
         End If
      End Function

      Public Shared Function GetWikiPageCount(ByVal criteria As String) As Integer
         If Not String.IsNullOrEmpty(criteria) Then
            Dim mdc As New MCCDataContext()
            Dim key As String = "WikiPages_WikiPageCount_" & criteria & "_"

            If Cache(key) IsNot Nothing Then
               Return CInt(Cache(key))
            Else
               Dim it As Integer = mdc.mcc_WikiPages.Count(Function(p) p.Description.Contains(criteria) Or p.Title.Contains(criteria))
               CacheData(key, it)
               Return it
            End If
         Else
            Return GetWikiPageCount()
         End If
      End Function


      Public Shared Function GetWikiPages() As List(Of mcc_WikiPage)
         Dim key As String = "WikiPages_WikiPages_"
         If Cache(key) IsNot Nothing Then
            Dim li As List(Of mcc_WikiPage) = DirectCast(Cache(key), List(Of mcc_WikiPage))
            Return li
         Else
            Dim mdc As New MCCDataContext()
            Dim li As List(Of mcc_WikiPage) = mdc.mcc_WikiPages.ToList
            CacheData(key, li)
            Return li
         End If
      End Function

      Public Shared Function GetWikiPages(ByVal criteria As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer) As List(Of mcc_WikiPage)
         If Not String.IsNullOrEmpty(criteria) Then
            Dim key As String = "WikiPages_WikiPages_" & criteria & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
            If Cache(key) IsNot Nothing Then
               Dim li As List(Of mcc_WikiPage) = DirectCast(Cache(key), List(Of mcc_WikiPage))
               Return li
            Else
               Dim mdc As New MCCDataContext()
               Dim li As List(Of mcc_WikiPage)
               If startrowindex > 0 AndAlso maximumrows > 0 Then
                  li = (From it As mcc_WikiPage In mdc.mcc_WikiPages Where it.Description.Contains(criteria) Or it.Title.Contains(criteria)).Skip(startrowindex - 1).Take(maximumrows).ToList
               Else
                  li = (From it As mcc_WikiPage In mdc.mcc_WikiPages Where it.Description.Contains(criteria) Or it.Title.Contains(criteria)).Skip(0).Take(10).ToList
               End If
               CacheData(key, li)
               Return li
            End If
         Else
            Return GetWikiPages(startrowindex, maximumrows)
         End If
      End Function

      Public Shared Function GetWikiPages(ByVal startrowindex As Integer, ByVal maximumrows As Integer) As List(Of mcc_WikiPage)
         Dim key As String = "WikiPages_WikiPages_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Dim li As List(Of mcc_WikiPage) = DirectCast(Cache(key), List(Of mcc_WikiPage))
            Return li
         Else
            Dim mdc As New MCCDataContext()
            Dim li As List(Of mcc_WikiPage)
            If startrowindex > 0 AndAlso maximumrows > 0 Then
               li = mdc.mcc_WikiPages.Skip(startrowindex - 1).Take(maximumrows).ToList
            Else
               li = mdc.mcc_WikiPages.Skip(0).Take(10).ToList
            End If
            CacheData(key, li)
            Return li
         End If
      End Function

      Public Shared Function GetWikiPageById(ByVal id As Integer) As mcc_WikiPage
         Dim key As String = "WikiPages_WikiPages_" & id.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Dim fb As mcc_WikiPage = DirectCast(Cache(key), mcc_WikiPage)
            Return fb
         Else
            Dim mdc As New MCCDataContext()
            Dim fb As mcc_WikiPage
            If mdc.mcc_WikiPages.Count(Function(p) p.WikiPageId = id) > 0 Then
               fb = (From it As mcc_WikiPage In mdc.mcc_WikiPages Where it.WikiPageId = id).FirstOrDefault
               CacheData(key, fb)
               Return fb
            Else
               Return Nothing
            End If
         End If
      End Function

      Public Shared Function GetWikiPageByPagename(ByVal pageName As String) As mcc_WikiPage
         Dim key As String = "WikiPages_WikiPages_" & pageName & "_"
         If Cache(key) IsNot Nothing Then
            Dim fb As mcc_WikiPage = DirectCast(Cache(key), mcc_WikiPage)
            Return fb
         Else
            Dim mdc As New MCCDataContext()
            Dim fb As mcc_WikiPage
            If mdc.mcc_WikiPages.Count(Function(p) p.PageName = pageName) > 0 Then
               fb = (From it As mcc_WikiPage In mdc.mcc_WikiPages Where it.PageName = pageName).FirstOrDefault
               CacheData(key, fb)
               Return fb
            Else
               Return Nothing
            End If
         End If
      End Function


      Public Shared Sub InsertWikiPage(ByVal title As String, ByVal description As String, ByVal body As String, ByVal tags As String, ByVal parentWikiId As Integer)
         Dim mdc As MCCDataContext = New MCCDataContext
         Dim fb As New mcc_WikiPage()

         description = ConvertNullToEmptyString(description)
         tags = ConvertNullToEmptyString(tags)

         With fb
            .Title = title
            .Description = description
            .AddedDate = DateTime.Now
            .AddedBy = mccObject.CurrentUserName
            .Body = body
            .Tags = tags
            .LastEditionDate = .AddedDate
            .LastEditedby = mccObject.CurrentUserName
            .ParentWikiId = parentWikiId
            .PageName = routines.GetSlugFromString(title, "_")
         End With

         mdc.mcc_WikiPages.InsertOnSubmit(fb)
         mdc.SubmitChanges()
         mccObject.PurgeCacheItems("WikiPages_")
      End Sub


      Public Shared Sub UpdateWikiPages(ByVal WikiPageId As Integer, ByVal title As String, ByVal description As String, ByVal body As String, ByVal tags As String, ByVal parentWikiId As Integer)
         Dim mdc As MCCDataContext = New MCCDataContext
         Dim wrd As mcc_WikiPage = (From t In mdc.mcc_WikiPages _
                                     Where t.WikiPageId = WikiPageId _
                                     Select t).Single()

         description = ConvertNullToEmptyString(description)
         tags = ConvertNullToEmptyString(tags)

         If wrd IsNot Nothing Then
            wrd.Title = title
            wrd.Description = description
            wrd.Body = body
            wrd.Description = description
            wrd.LastEditionDate = DateTime.Now
            wrd.LastEditedby = mccObject.CurrentUserName
            wrd.ParentWikiId = parentWikiId
            mdc.SubmitChanges()
            mccObject.PurgeCacheItems("WikiPages_WikiPages_")
         End If
      End Sub

      Protected Shared Sub CacheData(ByVal key As String, ByVal data As Object)
         If data IsNot Nothing Then
            mccObject.Cache.Insert(key, data, Nothing, DateTime.Now.AddSeconds(3600), TimeSpan.Zero)
         End If
      End Sub
   End Class
End Namespace

