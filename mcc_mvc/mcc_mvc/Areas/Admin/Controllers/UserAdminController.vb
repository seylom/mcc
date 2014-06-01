Imports MCC.Services
Imports MCC.Data
Imports Webdiyer.WebControls.Mvc

Namespace MCC.Areas.Admin.Controllers
   <Authorize(Roles:="Administrators")> _
   Public Class UserAdminController
      Inherits AdminController


      Private _userservice As IUserService

      Public Sub New()
         Me.New(New UserService)
      End Sub

      Public Sub New(ByVal usersrvr As IUserService)
         _userservice = usersrvr
      End Sub

      Function Index(ByVal page? As Integer, ByVal size? As Integer) As ActionResult
         Dim _viewdata As New AdminUsersViewModel()
         _viewdata.Users = _userservice.GetUsers(If(page, 0), If(size, 100), UserFilterType.Username, "")
         Return View(_viewdata)
      End Function

      Function EditUser(ByVal username As String) As ActionResult
         If String.IsNullOrEmpty(username) Then
            Return RedirectToAction("Index")
         End If
         Dim _viewdata As New AdminUserViewModel()
         Dim user As SiteUser = _userservice.GetUserByUsername(username)
         If user Is Nothing Then
            Return RedirectToAction("Index")
         End If

         _viewdata.Roles = Roles.GetAllRoles().ToList
         _viewdata.UserRoles = Roles.GetRolesForUser(user.Username).ToList
         user.FillViewModel(_viewdata)
         Return View(_viewdata)
      End Function

      <AcceptVerbs(HttpVerbs.Post)> _
      Function UpdateUserRoles(ByVal username As String, ByVal UserRoles() As String) As ActionResult

         If String.IsNullOrEmpty(username) Then
            Return RedirectToAction("Index")
         End If

         If UserRoles IsNot Nothing Then
            Dim oldroles() As String = Roles.GetRolesForUser(username)
            If oldroles.Count > 0 Then
               Roles.RemoveUserFromRoles(username, oldroles)
            End If

            If UserRoles.Count > 0 Then
               Roles.AddUserToRoles(username, UserRoles)
            End If
         End If

         Return RedirectToAction("EditUser", New With {.username = username})
      End Function

      <AcceptVerbs(HttpVerbs.Post)> _
      Function EditUser(ByVal vm As AdminUserViewModel) As ActionResult
         If String.IsNullOrEmpty(vm.Username) Then
            Return RedirectToAction("ManageUsers")
         End If

         Dim muser As MembershipUser = Membership.GetUser(vm.Username)
         If muser Is Nothing Then
            Return RedirectToAction("ManageUsers")
         End If

         muser.Email = vm.Email
         Membership.UpdateUser(muser)

         Dim pi As ProfileInfo = ProfileInfo.GetProfile(vm.Username)
         If pi IsNot Nothing Then
            pi.DisplayName = vm.DisplayName
            pi.About = vm.About
            pi.Website = vm.Website

            pi.Save()
         End If

         Return RedirectToAction("EditUser")
      End Function

      Function AdminIndex() As ActionResult
         Dim _viewdata As New AdminUsersViewModel()
         _viewdata.Users = _userservice.GetUnapprovedUsers(0, 30)

         Return View(_viewdata)
      End Function


      Function ManageUsers(ByVal page? As Integer, ByVal size? As Integer) As ActionResult
         Dim _viewdata As New AdminUsersViewModel()
         _viewdata.Users = _userservice.GetUsers(If(page, 0), If(size, 30), UserFilterType.Username, "")
         Return View(_viewdata)
      End Function

      <AcceptVerbs(HttpVerbs.Post)> _
      Function ManageUsers(ByVal SearchKey As String, ByVal FilterType As UserFilterType) As ActionResult
         Dim _viewdata As New AdminUsersViewModel()
         _viewdata.Users = _userservice.GetUsers(0, 30, FilterType, If(SearchKey, ""))
         Return View(_viewdata)
      End Function

      <AcceptVerbs(HttpVerbs.Post)> _
      Function DeleteUser(ByVal username As String) As ActionResult

         If username Is Nothing Then
            Return RedirectToAction("ManageUsers")
         End If

         Dim rs As Boolean = _userservice.DeleteUser(username)
         Return RedirectToAction("ManageUsers")

      End Function

      <AcceptVerbs(HttpVerbs.Post)> _
      Function DeleteUsers(ByVal Id() As String) As ActionResult
         If Request.IsAjaxRequest Then
            If Id Is Nothing Then
               Return Json(False)
            Else
               Dim rs As Boolean = _userservice.DeleteUsers(Id)
               Return Json(rs)
            End If
         Else
            Return RedirectToAction("Index")
         End If
      End Function
   End Class
End Namespace