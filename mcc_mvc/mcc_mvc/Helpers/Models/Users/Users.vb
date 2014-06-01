Imports Microsoft.VisualBasic
Imports System.Linq
Imports MCC.Data

Public Class Users
   Inherits mccObject
   Private _userCount As Integer = -1

   Public Function GetUsersCount() As Integer
      Dim uCount As Integer = 0
      Dim key As String = "users_count_"
      If Cache(key) IsNot Nothing Then
         uCount = CInt(Cache(key))
      Else
         Dim umc As New umcDataContext
         uCount = umc.aspnet_Users.Count 'Membership.GetAllUsers.Count
         CacheData(key, uCount)
      End If
      Return uCount
   End Function

   Public Function GetUsersCount(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal searchtype As String, ByVal criteria As String) As Integer
      Return GetUsersCount(searchtype, criteria)
   End Function

   Public Function GetUsersCount(ByVal searchtype As String, ByVal criteria As String) As Integer
      Dim uCount As Integer = 0
      Dim key As String = "users_" & searchtype & "_" & criteria & "_count_"
      If Cache(key) IsNot Nothing Then
         uCount = CInt(Cache(key))
      Else
         Dim umc As New umcDataContext
         If Not String.IsNullOrEmpty(criteria) Then
            If searchtype = "email" Then
               uCount = umc.aspnet_Memberships.Count(Function(p) p.Email.ToLower.Contains(criteria))  'Membership.FindUsersByEmail(criteria).Count
            Else
               uCount = umc.aspnet_Users.Count(Function(p) p.LoweredUserName.Contains(criteria.ToLower))
            End If
         Else
            uCount = umc.aspnet_Users.Count
         End If
         CacheData(key, uCount)
      End If
      Return uCount
   End Function

   Public Function GetUnapprovedUsersCount() As Integer
      Dim key As String = "users_unapproved_count"
      Dim it As Integer = 0
      If Cache(key) IsNot Nothing Then
         it = CInt(mccObject.Cache(key))
      Else
         Dim umc As New umcDataContext
         it = umc.aspnet_Memberships.Count(Function(p) p.IsApproved = False)
         If it > 0 Then
            CacheData(key, it)
         End If
      End If
      Return it
   End Function


   Public Shared Function GetUsernameByUserID(ByVal userId As Guid) As String
      Dim uname As String = String.Empty
      Dim muser = Membership.GetUser(userId)
      If muser IsNot Nothing Then
         uname = muser.UserName
      End If

      Return uname
   End Function

   Public Function FindUsersByEmail(ByVal criteria As String) As List(Of MembershipUser)
      Dim _allUsers As List(Of MembershipUser)
      Dim mUsers As MembershipUserCollection = Membership.FindUsersByEmail(criteria)
      _allUsers = (From mu As MembershipUser In mUsers).ToList
      Return _allUsers
   End Function

   Public Function FindUsersByName(ByVal criteria As String) As List(Of MembershipUser)
      Dim _allUsers As List(Of MembershipUser)
      Dim mUsers As MembershipUserCollection = Membership.FindUsersByName(criteria)
      _allUsers = (From mu As MembershipUser In mUsers).ToList
      Return _allUsers
   End Function


   ''' <summary>
   ''' Function is Higly unefficient and will need to be updated
   ''' </summary>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Public Shared Function GetUnapprovedUsers() As List(Of aspnet_membership)
      'Dim musers As MembershipUserCollection = Membership.GetAllUsers()
      Dim _allUsers As List(Of aspnet_membership)

      Dim key As String = "users_unapproved_users"
      If Cache(key) IsNot Nothing Then
         _allUsers = DirectCast(Cache(key), List(Of aspnet_membership))
      Else
         Dim umc As New umcDataContext
         _allUsers = (From usr As aspnet_membership In umc.aspnet_Memberships Where usr.IsApproved = False).ToList
         If _allUsers IsNot Nothing Then
            CacheData(key, _allUsers)
         End If
      End If
      Return _allUsers
   End Function

   Public Shared Function GetUsers(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal searchtype As String, ByVal criteria As String, Optional ByVal sortExp As String = "Username") As List(Of aspnet_membership)
      Dim key As String = "users_" & searchtype & "_" & criteria & "_" & startRowIndex.ToString & "_" & maximumRows.ToString & "_"
      Dim _allUsers As List(Of aspnet_membership)

      If Cache(key) IsNot Nothing Then
         _allUsers = DirectCast(Cache(key), List(Of aspnet_membership))
      Else
         Dim umc As New umcDataContext

         startRowIndex = IIf(startRowIndex >= 0, startRowIndex, 0)
         maximumRows = IIf(maximumRows > 0, maximumRows, 30)

         If Not String.IsNullOrEmpty(criteria) Then
            If searchtype = "email" Then
               _allUsers = (From m As aspnet_membership In umc.aspnet_Memberships _
                         Where m.LoweredEmail.Contains(criteria.ToLower)).SortBy(sortExp).Skip(startRowIndex).Take(maximumRows).ToList
            Else
               _allUsers = (From m As aspnet_membership In umc.aspnet_Memberships _
                                 Let users = (From u As aspnet_User In umc.aspnet_Users Where u.LoweredUserName.Contains(criteria) Select u.UserId) _
                                 Where users.Contains(m.UserId) Select m).SortBy(sortExp).Skip(startRowIndex).Take(maximumRows).ToList
            End If
         Else
            _allUsers = (From m As aspnet_membership In umc.aspnet_Memberships).SortBy(sortExp).Skip(startRowIndex).Take(maximumRows).ToList
         End If
         CacheData(key, _allUsers)
      End If
      Return _allUsers
   End Function

   Public Shared Function GetUnapprovedUsers(ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As List(Of aspnet_membership)

      Dim umc As New umcDataContext
      startRowIndex = IIf(startRowIndex >= 0, startRowIndex, 0)
      maximumRows = IIf(maximumRows > 0, maximumRows, 30)
      Dim li As List(Of aspnet_membership) = (From it As aspnet_membership In umc.aspnet_Memberships _
                                              Where it.IsApproved = False).Skip(startRowIndex).Take(maximumRows).ToList

      Return li
   End Function

   Public Shared Function DeleteUsers(ByVal username() As String) As Boolean
      For Each it As String In username
         If Not String.IsNullOrEmpty(it) AndAlso Membership.GetUser(it) IsNot Nothing Then
            Membership.DeleteUser(it)
         End If
      Next
      PurgeCacheItems("users_")
      Return True
   End Function


   Public Shared Function DeleteUser(ByVal username As String) As Boolean

      If Not String.IsNullOrEmpty(username) AndAlso Membership.GetUser(username) IsNot Nothing Then
         Membership.DeleteUser(username)
         PurgeCacheItems("users_")
      End If

      Return True
   End Function

   Protected Shared Sub CacheData(ByVal key As String, ByVal data As Object)
      If data IsNot Nothing Then
         mccObject.Cache.Insert(key, data, Nothing, DateTime.Now.AddSeconds(3600), TimeSpan.Zero)
      End If
   End Sub
End Class
