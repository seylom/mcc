Public Class UserProfileViewModel
   Inherits baseViewModel

   Private _displayName As String
   Public Property DisplayName() As String
      Get
         Return _displayName
      End Get
      Set(ByVal value As String)
         _displayName = value
      End Set
   End Property

   Private _body As String
   Public Property body() As String
      Get
         Return _body
      End Get
      Set(ByVal value As String)
         _body = value
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


   Private _emailAddress As String
   Public Property Email() As String
      Get
         Return _emailAddress
      End Get
      Set(ByVal value As String)
         _emailAddress = value
      End Set
   End Property


   Private _password As String
   Public Property Password() As String
      Get
         Return _password
      End Get
      Set(ByVal value As String)
         _password = value
      End Set
   End Property


   Private _newPassword As String
   Public Property NewPassword() As String
      Get
         Return _newPassword
      End Get
      Set(ByVal value As String)
         _newPassword = value
      End Set
   End Property


   Private _confirmPassword As String
   Public Property ConfirmPassword() As String
      Get
         Return _confirmPassword
      End Get
      Set(ByVal value As String)
         _confirmPassword = value
      End Set
   End Property

End Class
