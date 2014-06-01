Public Class ResetPasswordInfo

   Private _stepIndex As Integer
   Public Property StepIndex() As Integer
      Get
         Return _stepIndex
      End Get
      Set(ByVal value As Integer)
         _stepIndex = value
      End Set
   End Property



   Private _userinfo As String
   Public Property Userinfo() As String
      Get
         Return _userinfo
      End Get
      Set(ByVal value As String)
         _userinfo = value
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
