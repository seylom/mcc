Imports System
Imports System.Data
Imports System.Configuration
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls

Namespace MCC.SiteLayers.SqlClient
   Class SqlMembershipProviderEx
      Inherits SqlMembershipProvider
      Public Overloads Overrides Function CreateUser(ByVal username As String, ByVal password As String, ByVal email As String, ByVal passwordQuestion As String, ByVal passwordAnswer As String, ByVal isApproved As Boolean, _
       ByVal providerUserKey As Object, ByRef status As MembershipCreateStatus) As MembershipUser
         If username.ToLower() = password.ToLower() Then
            status = MembershipCreateStatus.InvalidPassword
            Return Nothing
         Else
            Return MyBase.CreateUser(username, password, email, passwordQuestion, passwordAnswer, isApproved, _
             providerUserKey, status)
         End If
        End Function
   End Class
End Namespace
