Imports MCC.Data
Imports Webdiyer.WebControls.Mvc
Public Class WikiService
   Inherits CacheObject
   Implements IWikiService

   Private _wikiRepo As IWikiRepository = New WikiRepository

   Public Function GetWikiPageCount() As Integer Implements IWikiService.GetWikiPageCount

      Dim key As String = "WikiPages_WikiPageCount"

      If Cache(key) IsNot Nothing Then
         Return CInt(Cache(key))
      Else
         Dim it As Integer = _wikiRepo.GetWikiPages.Count
         CacheData(key, it)
         Return it
      End If
   End Function

   Public Function GetWikiPageCount(ByVal criteria As String) As Integer Implements IWikiService.GetWikiPageCount
      If Not String.IsNullOrEmpty(criteria) Then

         Dim key As String = "WikiPages_WikiPageCount_" & criteria & "_"

         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            Dim it As Integer = _wikiRepo.GetWikiPages.Count(Function(p) p.Description.Contains(criteria) Or p.Title.Contains(criteria))
            CacheData(key, it)
            Return it
         End If
      Else
         Return GetWikiPageCount()
      End If
   End Function


   Public Function GetWikiPages() As List(Of WikiPage) Implements IWikiService.GetWikiPages
      Dim key As String = "WikiPages_WikiPages_"
      If Cache(key) IsNot Nothing Then
         Dim li As List(Of WikiPage) = DirectCast(Cache(key), List(Of WikiPage))
         Return li
      Else

         Dim li As List(Of WikiPage) = _wikiRepo.GetWikiPages.ToList
         CacheData(key, li)
         Return li
      End If
   End Function

   Public Function GetWikiPages(ByVal criteria As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer) As PagedList(Of WikiPage) Implements IWikiService.GetWikiPages
      If Not String.IsNullOrEmpty(criteria) Then
         Dim key As String = "WikiPages_WikiPages_" & criteria & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Return DirectCast(Cache(key), PagedList(Of WikiPage))
         Else

            Dim li As PagedList(Of WikiPage) = _wikiRepo.GetWikiPages. _
            Where(Function(p) p.Description.Contains(criteria) Or p.body.Contains(criteria)).ToPagedList(startrowindex, maximumrows)
            CacheData(key, li)
            Return li
         End If
      Else
         Return GetWikiPages(startrowindex, maximumrows)
      End If
   End Function

   Public Function GetWikiPages(ByVal startrowindex As Integer, ByVal maximumrows As Integer) As PagedList(Of WikiPage) Implements IWikiService.GetWikiPages
      Dim key As String = "WikiPages_WikiPages_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
      If Cache(key) IsNot Nothing Then
            Return DirectCast(Cache(key), PagedList(Of WikiPage))
      Else

         Dim li As PagedList(Of WikiPage) = _wikiRepo.GetWikiPages.ToPagedList(startrowindex, maximumrows)
         CacheData(key, li)
         Return li
      End If
   End Function

   Public Function GetWikiPageById(ByVal id As Integer) As WikiPage Implements IWikiService.GetWikiPageById
      Dim key As String = "WikiPages_WikiPages_" & id.ToString & "_"
      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), WikiPage)
      Else
         Dim fb As WikiPage = _wikiRepo.GetWikiPages.Where(Function(p) p.WikiPageID = id).FirstOrDefault()
         CacheData(key, fb)
         Return fb
      End If
   End Function

   Public Function GetWikiPageByPagename(ByVal pageName As String) As WikiPage Implements IWikiService.GetWikiPageByPagename
      Dim key As String = "WikiPages_WikiPages_" & pageName & "_"
      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), WikiPage)
      Else
         Dim fb As WikiPage = _wikiRepo.GetWikiPages.Where(Function(p) p.PageName = pageName).FirstOrDefault()
         CacheData(key, fb)
         Return fb
      End If
   End Function


   Public Sub InsertWikiPage(ByVal page As WikiPage) Implements IWikiService.InsertWikiPage

      With page
         .Description = ConvertNullToEmptyString(.Description)
         .Tags = ConvertNullToEmptyString(.Tags)
         .AddedDate = DateTime.Now
         .AddedBy = CacheObject.CurrentUserName
         .PageName = .Title.ToSlug("_")
      End With

      _wikiRepo.InsertWikiPage(page)
      CacheObject.PurgeCacheItems("WikiPages_")
   End Sub


   Public Sub UpdateWikiPages(ByVal page As WikiPage) Implements IWikiService.UpdateWikiPages
      If page IsNot Nothing Then
         _wikiRepo.UpdateWikiPage(page)
         CacheObject.PurgeCacheItems("WikiPages_WikiPages_")
      End If
   End Sub

   Protected Sub CacheData(ByVal key As String, ByVal data As Object)
      If data IsNot Nothing Then
         CacheObject.Cache.Insert(key, data, Nothing, DateTime.Now.AddSeconds(3600), TimeSpan.Zero)
      End If
   End Sub
End Class
