Namespace MCC.Areas.Site.Controllers
   Public Class GeneralController
      Inherits ApplicationController

      '
      ' GET: /General/

      Function Index() As ActionResult
         Return View()
      End Function

      Function Help() As ActionResult
         Return View()
      End Function

      Function About() As ActionResult
         Return View()
      End Function

      Function TermsOfUse() As ActionResult
         Return View()
      End Function

      Function ContactUs() As ActionResult
         Dim _viewdata As New vdContactUs()
         If UserIsAuthenticated Then
            Dim pr As ProfileInfo = ProfileInfo.GetProfile(User.Identity.Name)
            Dim muser As MembershipUser = Membership.GetUser(User.Identity.Name)
            If pr IsNot Nothing Then
               _viewdata.Name = IIf(String.IsNullOrEmpty(pr.DisplayName), User.Identity.Name, pr.DisplayName)

            End If

            If muser IsNot Nothing Then
               _viewdata.Email = muser.Email
            End If
         End If

         Return View(_viewdata)
      End Function


      <AcceptVerbs(HttpVerbs.Post)> _
      Function ContactUs(ByVal name As String, ByVal email As String, ByVal subject As String, ByVal body As String) As ActionResult
         If Not String.IsNullOrEmpty(name) AndAlso Not String.IsNullOrEmpty(email) _
            AndAlso routines.ValidateEmail(email) AndAlso Not String.IsNullOrEmpty(subject) AndAlso Not String.IsNullOrEmpty(body) Then
            Dim res As Boolean = routines.SendMail(name, email, subject, body)
            If Not res Then
               TempData("ErrorMessage") = "We are unable to send you mail. Please make sure the form is properly filled"
               Return RedirectToAction("ContactUs")
            Else
               TempData("Message") = "Message Sent successfully!"
            End If
         Else
            TempData("ErrorMessage") = "Please make sure the form is properly filled"
         End If
         Return RedirectToAction("ContactUs")
      End Function

      Function ErrorMessage() As ActionResult

         Return View()
      End Function

      Function AccessDenied() As ActionResult

         Return View()
      End Function

      Function ReportIssues() As ActionResult

         Return View()
      End Function

      Function Sitemap() As ActionResult
         Return View()
      End Function

      Function SubmitContent() As ActionResult

         Return View()
      End Function


   End Class
End Namespace