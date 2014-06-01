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

Imports System.Linq
Imports MCC.Data

Namespace Searches
   Public Interface ISearchRepository
      Function FindMatches(ByVal searchWord As String, ByVal fwhere As SearchLocation, ByVal searchType As SearchType, ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As List(Of Search)
      Function FilterNoiseWords(ByVal strInput As String) As String
      Function FindArticles(ByVal searchType As SearchType, ByVal searchWord As String, ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As List(Of Search)
      Function FindArticlesComments(ByVal searchtype As SearchType, ByVal searchWord As String, ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As List(Of Search)
      Function FindVideos(ByVal searchType As SearchType, ByVal searchWord As String, ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As List(Of Search)
      Function FindVideoComments(ByVal searchType As SearchType, ByVal searchWord As String, ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As List(Of Search)
      Function FindQuestions(ByVal searchType As SearchType, ByVal searchWord As String, ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As List(Of Search)
      Function FindAll(ByVal searchType As SearchType, ByVal searchWord As String, ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As List(Of Search)
      Property SearchResults() As List(Of Search)
      Function FindMatchesCount(ByVal searchWord As String, ByVal fwhere As SearchLocation, ByVal searchType As SearchType) As Integer
      Function ArticleFinderSqlCommand(ByVal keyword As String, ByVal searchtype As SearchType) As String
   End Interface
End Namespace
