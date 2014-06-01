Namespace MCC.Areas.Site.Controllers


   Public Class CredentialController
      Inherits ApplicationController

      '
      ' GET: /Credential/

      Function Index() As ActionResult
         Return View()
      End Function


      Function Login() As ActionResult

         Return View()
      End Function


      <AcceptVerbs(HttpVerbs.Post)> _
      Function Login(ByVal username As String, ByVal password As String, ByVal rememberMe As Boolean, ByVal returnUrl As String) As ActionResult




         Return View()
      End Function



      Function Register() As ActionResult


         Return View()
      End Function
   End Class
End Namespace