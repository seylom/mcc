Imports MCC.Data
Imports Webdiyer.WebControls.Mvc
Imports System.Web.Security

Public Class UserService
    Inherits CacheObject
    Implements IUserService


    Private _userRepo As IUserRepository
    Public Sub New()
        Me.New(New UserRepository())
    End Sub

    Public Sub New(ByVal userSrvr As IUserRepository)
        _userRepo = userSrvr
    End Sub

    Public Function GetUsersCount() As Integer Implements IUserService.GetUsersCount
        Dim uCount As Integer = 0
        Dim key As String = "users_count_"
        If Cache(key) IsNot Nothing Then
            uCount = CInt(Cache(key))
        Else
            uCount = _userRepo.GetUsers.Count 'Membership.GetAllUsers.Count
            CacheData(key, uCount)
        End If
        Return uCount
    End Function

    Public Function GetUsersCount(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal searchtype As String, ByVal criteria As String) As Integer Implements IUserService.GetUsersCount
        Return GetUsersCount(searchtype, criteria)
    End Function

    Public Function GetUsersCount(ByVal searchtype As String, ByVal criteria As String) As Integer Implements IUserService.GetUsersCount
        Dim uCount As Integer = 0
        Dim key As String = "users_" & searchtype & "_" & criteria & "_count_"
        If Cache(key) IsNot Nothing Then
            uCount = CInt(Cache(key))
        Else

            If Not String.IsNullOrEmpty(criteria) Then
                If searchtype = "email" Then
                    uCount = _userRepo.GetUsers.Count(Function(p) p.Email.ToLower.Contains(criteria))  'Membership.FindUsersByEmail(criteria).Count
                Else
                    uCount = _userRepo.GetUsers.Count(Function(p) p.Username.ToLower.Contains(criteria.ToLower))
                End If
            Else
                uCount = _userRepo.GetUsers.Count
            End If
            CacheData(key, uCount)
        End If
        Return uCount
    End Function

    Public Function GetUnapprovedUsersCount() As Integer Implements IUserService.GetUnapprovedUsersCount
        Dim key As String = "users_unapproved_count"
        Dim it As Integer = 0
        If Cache(key) IsNot Nothing Then
            it = CInt(Cache(key))
        Else
            it = _userRepo.GetUsers.Count(Function(p) p.IsApproved = False)
            If it > 0 Then
                CacheData(key, it)
            End If
        End If
        Return it
    End Function

    Public Function FindUsersByEmail(ByVal criteria As String) As List(Of SiteUser) Implements IUserService.FindUsersByEmail
        Dim key As String = String.Format("users_userswithemail_{0}", criteria)
        If Cache(key) IsNot Nothing Then
            Return CType(Cache(key), List(Of SiteUser))
        End If
        Dim q As List(Of SiteUser) = _userRepo.GetUsers.Where(Function(p) p.Username.Contains(criteria)).ToList
        CacheData(key, q)
        Return q
    End Function

    Public Function FindUsersByName(ByVal criteria As String) As List(Of SiteUser) Implements IUserService.FindUsersByName
        Dim key As String = String.Format("users_userswithemail_{0}", criteria)
        If Cache(key) IsNot Nothing Then
            Return CType(Cache(key), List(Of SiteUser))
        End If
        Dim q As List(Of SiteUser) = _userRepo.GetUsers.Where(Function(p) p.Email.Contains(criteria)).ToList
        CacheData(key, q)
        Return q
    End Function


    ''' <summary>
    ''' Function is Higly unefficient and will need to be updated
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetUnapprovedUsers() As List(Of SiteUser) Implements IUserService.GetUnapprovedUsers
        Dim key As String = "users_unapproved_users"
        If Cache(key) IsNot Nothing Then
            Return DirectCast(Cache(key), List(Of SiteUser))
        End If


        Dim users As List(Of SiteUser) = _userRepo.GetUsers.Where(Function(usr) usr.IsApproved = False).ToList
        CacheData(key, users)
        Return users
    End Function

    Public Function GetUsers(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal searchtype As UserFilterType, ByVal criteria As String, Optional ByVal sortExp As String = "Username") As PagedList(Of SiteUser) Implements IUserService.GetUsers
        Dim key As String = String.Format("users_{0}_{1}_{2}_{3}", searchtype, criteria, startRowIndex, maximumRows)

        If Cache(key) IsNot Nothing Then
            Return DirectCast(Cache(key), PagedList(Of SiteUser))
        End If

        Dim users As PagedList(Of SiteUser)

        If Not String.IsNullOrEmpty(criteria) Then
            If searchtype = UserFilterType.Email Then
                users = _userRepo.GetUsers.Where(Function(m) m.Email.ToLower.Contains(criteria.ToLower)).SortBy(sortExp). _
                         ToPagedList(startRowIndex, maximumRows)
            Else
                users = _userRepo.GetUsers.Where(Function(u) u.Username.Contains(criteria)).SortBy(sortExp).ToPagedList(startRowIndex, maximumRows)
            End If
        Else
            users = _userRepo.GetUsers.ToPagedList(startRowIndex, maximumRows)
        End If
        CacheData(key, users)
        Return users
    End Function

    Public Function GetUnapprovedUsers(ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As PagedList(Of SiteUser) Implements IUserService.GetUnapprovedUsers
        Dim key As String = String.Format("users_unapproved_{0}_{1}", startRowIndex, maximumRows)
        If Cache(key) IsNot Nothing Then
            Return CType(Cache(key), PagedList(Of SiteUser))
        End If
        Dim li As PagedList(Of SiteUser) = _userRepo.GetUsers.Where(Function(it) it.IsApproved = False).ToPagedList(startRowIndex, maximumRows)
        Return li
    End Function

    Public Function DeleteUsers(ByVal username() As String) As Boolean Implements IUserService.DeleteUsers
        For Each it As String In username
            Try
                If Not String.IsNullOrEmpty(it) Then
                    Dim muser As MembershipUser = Membership.GetUser(it)
                    If muser IsNot Nothing Then
                        Membership.DeleteUser(muser.UserName)
                    End If
                End If
            Catch ex As Exception

            End Try
        Next
        PurgeCacheItems("users_")
        Return True
    End Function


    Public Function DeleteUser(ByVal username As String) As Boolean Implements IUserService.DeleteUser
        If Not String.IsNullOrEmpty(username) AndAlso Membership.GetUser(username) IsNot Nothing Then
            Membership.DeleteUser(username)
            PurgeCacheItems("users_")
        End If
        Return True
    End Function

    Protected Sub CacheData(ByVal key As String, ByVal data As Object)
        If data IsNot Nothing Then
            CacheObject.Cache.Insert(key, data, Nothing, DateTime.Now.AddSeconds(3600), TimeSpan.Zero)
        End If
    End Sub

    Public Function GetUserByUsername(ByVal username As String) As Data.SiteUser Implements IUserService.GetUserByUsername
        If String.IsNullOrEmpty(username) Then
            Return Nothing
        End If

        Dim loweredUsername As String = username.ToLower
        Dim user As SiteUser = _userRepo.GetUsers.Where(Function(p) p.Username.ToLower = loweredUsername).FirstOrDefault()
        Return user
    End Function
End Class
