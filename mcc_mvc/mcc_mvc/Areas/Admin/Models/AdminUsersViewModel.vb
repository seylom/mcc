Imports MCC.Services
Imports MCC.Data
Imports Webdiyer.WebControls.Mvc
Public Class AdminUsersViewModel
   Inherits baseViewModel

   Private _userservice As IUserService


   'Public Sub New(ByVal approvedOnly As Boolean)
   '   Me.New(approvedOnly, 0, 50, New UserService)
   'End Sub

   'Public Sub New(ByVal approvedOnly As Boolean, ByVal pageindex As Integer, ByVal pageSize As Integer)
   '   Me.New(approvedOnly, pageindex, pageSize, New UserService)
   'End Sub


   'Public Sub New(ByVal approvedOnly As Boolean, ByVal pageindex As Integer, ByVal pageSize As Integer, ByVal userservr As IUserService)
   '   _pageIndex = pageindex
   '   _pageSize = pageSize
   '   _userservice = userservr

   '   If approvedOnly Then
   '      _users = _userservice.GetUnapprovedUsers(pageindex, pageSize)
   '   Else
   '      _users = _userservice.GetUsers(pageindex, pageSize, "", "")
   '   End If

   'End Sub

   Private _users As PagedList(Of SiteUser)
   Public Property Users() As PagedList(Of SiteUser)
      Get
         Return _users
      End Get
      Set(ByVal value As PagedList(Of SiteUser))
         _users = value
      End Set
   End Property



   Private _pageIndex As Integer
   Public Property PageIndex() As Integer
      Get
         Return _pageIndex
      End Get
      Set(ByVal value As Integer)
         _pageIndex = value
      End Set
   End Property



   Private _pageSize As Integer
   Public Property PageSize() As Integer
      Get
         Return _pageSize
      End Get
      Set(ByVal value As Integer)
         _pageSize = value
      End Set
   End Property

   Private _userFilterType As UserFilterType = UserFilterType.Username
   Public Property FilterType() As UserFilterType
      Get
         Return _userFilterType
      End Get
      Set(ByVal value As UserFilterType)
         _userFilterType = value
      End Set
   End Property


   Private _searchKey As String
   Public Property SearchKey() As String
      Get
         Return _searchKey
      End Get
      Set(ByVal value As String)
         _searchKey = value
      End Set
   End Property


   Private _alphabet As String = "ABCDEGHIJKLMNOPQRSTUVWXYZ"
   Public ReadOnly Property AlphabetItems() As String
      Get
         Return _alphabet
      End Get
   End Property


End Class
