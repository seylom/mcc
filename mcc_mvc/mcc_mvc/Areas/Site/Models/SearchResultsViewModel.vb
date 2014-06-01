Imports MCC.Data
Imports MCC.Services
Imports Webdiyer.WebControls.Mvc
Public Class SearchResultsViewModel
   Inherits baseViewModel

   Private _searchservice As ISearchService
   Public Sub New(ByVal keyword As String, ByVal _type As SearchQueryType, ByVal _loc As SearchLocation, ByVal searchsrvr As ISearchService)
      Me.New(keyword, _type, _loc, searchsrvr, 0, 10)
   End Sub
   Public Sub New(ByVal keyword As String, ByVal _type As SearchQueryType, ByVal _loc As SearchLocation, ByVal searchsrvr As ISearchService, ByVal page As Integer, ByVal size As Integer)
      _searchservice = searchsrvr
      _keywords = keyword
      _searchType = _type
      _location = _loc
      If Not String.IsNullOrEmpty(Keywords) Then
         _results = _searchservice.FindMatches(keyword, _loc, _type, page, size)
      End If
   End Sub

   Private _keywords As String
   Public Property Keywords() As String
      Get
         Return _keywords
      End Get
      Set(ByVal value As String)
         _keywords = value
      End Set
   End Property

   Private _searchType As SearchQueryType = SearchQueryType.AnyWord
   Public Property Type() As SearchQueryType
      Get
         Return _searchType
      End Get
      Set(ByVal value As SearchQueryType)
         _searchType = value
      End Set
   End Property

   Private _location As SearchLocation = SearchLocation.All
   Public Property Location() As String
      Get
         Return _location
      End Get
      Set(ByVal value As String)
            _location = value
      End Set
   End Property

   Private _results As PagedList(Of SearchResult)
   Public Property Results() As PagedList(Of SearchResult)
      Get
         Return _results
      End Get
      Set(ByVal value As PagedList(Of SearchResult))
         _results = value
      End Set
   End Property

End Class
