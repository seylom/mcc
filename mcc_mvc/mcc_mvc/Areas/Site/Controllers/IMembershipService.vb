Imports System.Globalization
Imports System.Security.Principal
Imports MCC.Services


Public Interface IMembershipService
   ReadOnly Property MinPasswordLength() As Integer

   Function ChangePassword(ByVal userName As String, ByVal oldPassword As String, ByVal newPassword As String) As Boolean
   Function CreateUser(ByVal userName As String, ByVal password As String, ByVal email As String) As MembershipCreateStatus
   Function ValidateUser(ByVal userName As String, ByVal password As String) As Boolean

End Interface
