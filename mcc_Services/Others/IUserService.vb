Imports System.Web.Security
Imports MCC.Data
Imports Webdiyer.WebControls.Mvc

Public Interface IUserService
   Function GetUsersCount() As Integer
   Function GetUsersCount(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal searchtype As String, ByVal criteria As String) As Integer
   Function GetUsersCount(ByVal searchtype As String, ByVal criteria As String) As Integer
   Function GetUnapprovedUsersCount() As Integer
   Function FindUsersByEmail(ByVal criteria As String) As List(Of SiteUser)
   Function FindUsersByName(ByVal criteria As String) As List(Of SiteUser)
   ''' <summary>
   ''' Function is Higly unefficient and will need to be updated
   ''' </summary>
   ''' <returns></returns>
   ''' <remarks></remarks>
   ''' 
   Function GetUnapprovedUsers() As List(Of SiteUser)
   Function GetUsers(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal searchtype As UserFilterType, ByVal criteria As String, Optional ByVal sortExp As String = "Username") As PagedList(Of SiteUser)
   Function GetUnapprovedUsers(ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As PagedList(Of SiteUser)
   Function GetUserByUsername(ByVal username As String) As SiteUser
   Function DeleteUsers(ByVal username As String()) As Boolean
   Function DeleteUser(ByVal username As String) As Boolean
End Interface
