Imports MCC.Services
Imports MCC.Data


Namespace MCC.Areas.Site.Controllers
   Public Class SearchController
      Inherits ApplicationController

      Private _searchService As ISearchService

      Public Sub New()
         Me.New(New SearchService)
      End Sub

      Public Sub New(ByVal searchsrvr As ISearchService)
         _searchService = searchsrvr
      End Sub

      Function Index() As ActionResult
         Dim _viewdata As New SearchResultsViewModel("", SearchQueryType.AllWords, SearchLocation.All, _searchService)
         Return View(_viewdata)
      End Function

      <AcceptVerbs(HttpVerbs.Post)> _
      Function Index(ByVal q As String, ByVal locationSearch? As SearchLocation, ByVal TypeSearch? As SearchQueryType, ByVal page? As Integer, ByVal size? As Integer) As ActionResult
         Dim _viewdata As New SearchResultsViewModel(q, If(TypeSearch, SearchQueryType.AnyWord), If(locationSearch, SearchLocation.All), _searchService, If(page, 0), If(size, 10))
         Return View(_viewdata)
      End Function

      'Function Results(ByVal q As String) As ActionResult
      '   Dim _viewdata As New SearchResultsViewModel()
      '   Return View()
      'End Function
   End Class
End Namespace