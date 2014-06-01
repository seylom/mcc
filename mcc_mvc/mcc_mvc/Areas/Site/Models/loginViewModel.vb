Public Class loginViewModel
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

   Private _password As String
   Public Property Password() As String
      Get
         Return _password
      End Get
      Set(ByVal value As String)
         _password = value
      End Set
   End Property


   Private _rememberMe As Boolean
   Public Property RememberMe() As Boolean
      Get
         Return _rememberMe
      End Get
      Set(ByVal value As Boolean)
         _rememberMe = value
      End Set
   End Property

   Private _returnUrl As String
   Public Property ReturnUrl() As String
      Get
         Return _returnUrl
      End Get
      Set(ByVal value As String)
         _returnUrl = value
      End Set
   End Property


End Class
