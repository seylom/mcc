Public Class registerViewModel
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



   Private _password As String
   Public Property Password() As String
      Get
         Return _password
      End Get
      Set(ByVal value As String)
         _password = value
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



   Private _verificationCode As String
   Public Property VerificationCode() As String
      Get
         Return _verificationCode
      End Get
      Set(ByVal value As String)
         _verificationCode = value
      End Set
   End Property


End Class
