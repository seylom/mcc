Imports MCC.Data
Imports Webdiyer.WebControls.Mvc

Public Interface IWikiService
   Function GetWikiPageCount() As Integer
   Function GetWikiPageCount(ByVal criteria As String) As Integer
   Function GetWikiPages() As List(Of WikiPage)
   Function GetWikiPages(ByVal criteria As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer) As PagedList(Of WikiPage)
   Function GetWikiPages(ByVal startrowindex As Integer, ByVal maximumrows As Integer) As PagedList(Of WikiPage)
   Function GetWikiPageById(ByVal id As Integer) As WikiPage
   Function GetWikiPageByPagename(ByVal pageName As String) As WikiPage
   Sub InsertWikiPage(ByVal page As WikiPage)
   Sub UpdateWikiPages(ByVal page As WikiPage)
End Interface
