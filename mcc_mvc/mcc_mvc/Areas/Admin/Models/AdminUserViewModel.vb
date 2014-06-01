Public Class AdminUserViewModel
   Inherits baseViewModel

   Private _username As String
   Public Property Username() As String
      Get
         Return _username
      End Get
      Set(ByVal value As String)
         _username = value
      End Set
   End Property



   Private _email As String
   Public Property Email() As String
      Get
         Return _email
      End Get
      Set(ByVal value As String)
         _email = value
      End Set
   End Property


   Private _isLockedOut As Boolean
   Public Property IsLockedOut() As Boolean
      Get
         Return _isLockedOut
      End Get
      Set(ByVal value As Boolean)
         _isLockedOut = value
      End Set
   End Property


   Private _isApproved As Boolean
   Public Property IsApproved() As Boolean
      Get
         Return _isApproved
      End Get
      Set(ByVal value As Boolean)
         _isApproved = value
      End Set
   End Property


   Private _isOnline As Boolean
   Public Property isOnline() As Boolean
      Get
         Return _isOnline
      End Get
      Set(ByVal value As Boolean)
         _isOnline = value
      End Set
   End Property

   Private _userroles As List(Of String)
   Public Property UserRoles() As List(Of String)
      Get
         Return _userroles
      End Get
      Set(ByVal value As List(Of String))
         _userroles = value
      End Set
   End Property


   Private _roles As List(Of String)
   Public Property Roles() As List(Of String)
      Get
         Return _roles
      End Get
      Set(ByVal value As List(Of String))
         _roles = value
      End Set
   End Property


   Private _lastActivityDate As DateTime
   Public Property LastActivityDate() As DateTime
      Get
         Return _lastActivityDate
      End Get
      Set(ByVal value As DateTime)
         _lastActivityDate = value
      End Set
   End Property



   Private _displayName As String
   Public Property DisplayName() As String
      Get
         Return _displayName
      End Get
      Set(ByVal value As String)
         _displayName = value
      End Set
   End Property


   Private _website As String
   Public Property Website() As String
      Get
         Return _website
      End Get
      Set(ByVal value As String)
         _website = value
      End Set
   End Property



   Private _about As String
   Public Property About() As String
      Get
         Return _about
      End Get
      Set(ByVal value As String)
         _about = value
      End Set
   End Property



End Class
