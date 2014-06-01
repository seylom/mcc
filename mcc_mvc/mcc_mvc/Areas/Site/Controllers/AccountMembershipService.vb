Imports System.Globalization
Imports System.Security.Principal
Imports MCC.Services


Public Class AccountMembershipService
   Implements IMembershipService

   Private _provider As MembershipProvider

   Sub New()
      Me.New(Nothing)
   End Sub

   Sub New(ByVal provider As MembershipProvider)
      _provider = If(provider, Membership.Provider)
   End Sub

   Public ReadOnly Property MinPasswordLength() As Integer Implements IMembershipService.MinPasswordLength
      Get
         Return _provider.MinRequiredPasswordLength
      End Get
   End Property

   Function ChangePassword(ByVal userName As String, ByVal oldPassword As String, ByVal newPassword As String) As Boolean Implements IMembershipService.ChangePassword
      Dim currentUser As MembershipUser = _provider.GetUser(userName, True)
      Return currentUser.ChangePassword(oldPassword, newPassword)
   End Function

   Function CreateUser(ByVal userName As String, ByVal password As String, ByVal email As String) As MembershipCreateStatus Implements IMembershipService.CreateUser
      Dim status As MembershipCreateStatus
      _provider.CreateUser(userName, password, email, Nothing, Nothing, True, Nothing, status)
      Return status
   End Function

   Function ValidateUser(ByVal userName As String, ByVal password As String) As Boolean Implements IMembershipService.ValidateUser
      Return _provider.ValidateUser(userName, password)
   End Function

End Class
