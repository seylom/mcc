Public Class resetInfoViewModel
   Inherits baseViewModel


   Private _userinfo As String
   Public Property UserInfo() As String
      Get
         Return _userinfo
      End Get
      Set(ByVal value As String)
         _userinfo = value
      End Set
   End Property

End Class
