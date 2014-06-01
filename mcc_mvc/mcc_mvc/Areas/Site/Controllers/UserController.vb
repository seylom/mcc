Imports MCC.Services

Namespace MCC.Areas.Site.Controllers
   Public Class UserController
      Inherits ApplicationController
      '
      ' GET: /User/

      Function Index() As ActionResult
         Return View()
      End Function


      <Authorize()> _
      Function EditProfile() As ActionResult

         Dim pi As ProfileInfo = ProfileInfo.GetProfile(User.Identity.Name)

         If pi Is Nothing Then
            Return Redirect("/")
         End If

         Dim _viewdata As New UserProfileViewModel
         _viewdata.DisplayName = pi.DisplayName
         _viewdata.body = pi.About
         _viewdata.Website = pi.Website


         Dim _questionsrvr As New UserQuestionService()
         Dim _answersrvr As New UserAnswerService()

         Dim muser As MembershipUser = Membership.GetUser(User.Identity.Name)
         If muser IsNot Nothing Then
            _viewdata.Email = muser.Email
         End If

         Return View(_viewdata)
      End Function

      <Authorize(), AcceptVerbs(HttpVerbs.Post)> _
      Function UpdateProfile(ByVal profileData As UserProfileViewModel) As ActionResult

         If Not routines.ValidateEmail(profileData.Email) Then
            profileData.Messages.Add("Incorrect Email. Please Correct the email field")
            Return View("EditProfile", profileData)
         End If

         Dim muser As MembershipUser = Membership.GetUser(User.Identity.Name)
         If muser IsNot Nothing Then
            muser.Email = profileData.Email
            Membership.UpdateUser(muser)
         End If

         Dim pr As ProfileInfo = ProfileInfo.GetProfile(User.Identity.Name)
         pr.DisplayName = profileData.DisplayName
         pr.Website = profileData.Website
         pr.About = profileData.body

         pr.Save()

         TempData("SuccessMessage") = "Profile Updated Successully!"

         Return RedirectToAction("EditProfile")
      End Function


      Function ViewProfile(ByVal username As String) As ActionResult
         Dim _viewdata = ProfileInfo.GetProfile(username)

         If _viewdata Is Nothing Then
            Return RedirectToRoute("UserNotFound")
         End If

         Dim muser As MembershipUser = Membership.GetUser(username)
         If muser IsNot Nothing Then
            ViewData("Email") = muser.Email
         End If

         Return View(_viewdata)
      End Function

      <AcceptVerbs(HttpVerbs.Post)> _
      Function ChangePassword(ByVal vd As UserProfileViewModel) As ActionResult

         SetUserProfileInfo(vd)

         If Not ValidatePasswordChange(vd) Then
            Return View("EditProfile", vd)
         End If

         Dim _service As New AccountMembershipService
         Dim bt As Boolean = _service.ChangePassword(User.Identity.Name, vd.Password, vd.NewPassword)
         If Not bt Then
            vd.Messages.Add("Password change unsuccessful. Please check the information provided.")
            Return View("EditProfile", vd)
         End If

         Return RedirectToAction("EditProfile")
      End Function


      Private Function ValidatePasswordChange(ByVal vd As UserProfileViewModel) As Boolean
         Dim isValid As Boolean = True
         If String.IsNullOrEmpty(vd.Password) Then
            vd.Messages.Add("Please provide your old password for verification purposes.")
            isValid = False
         End If

         If String.IsNullOrEmpty(vd.Password) Then
            vd.Messages.Add("Please provide your new password.")
            isValid = False
         End If

         If String.IsNullOrEmpty(vd.Password) Then
            vd.Messages.Add("Please confirm your new password.")
            isValid = False
         End If

         If Not vd.NewPassword.Equals(vd.ConfirmPassword, StringComparison.OrdinalIgnoreCase) Then
            vd.Messages.Add("Your new password and confirmation password don't match.")
            isValid = False
         End If

         Return isValid
      End Function


      Sub SetUserProfileInfo(ByVal upvm As UserProfileViewModel)
         If upvm Is Nothing Then
            upvm = New UserProfileViewModel
         End If

         Dim pi As ProfileInfo = ProfileInfo.GetProfile(User.Identity.Name)

         If pi Is Nothing Then
            Return
         End If

         upvm.DisplayName = pi.DisplayName
         upvm.body = pi.About
         upvm.Website = pi.Website

         Dim muser As MembershipUser = Membership.GetUser(User.Identity.Name)
         If muser IsNot Nothing Then
            upvm.Email = muser.Email
         End If

      End Sub

      Function UserQuestions(ByVal user As String, ByVal page? As Integer, ByVal size? As Integer) As ActionResult
         Dim _viewdata As New UserProfileQuestionViewModel
         Dim userquestionsrvr As New UserQuestionService
         _viewdata.Questions = userquestionsrvr.GetUserQuestionsByUser(user, If(page, 0), If(size, 10))

         Return View(_viewdata)
      End Function

      Function UserAnswers(ByVal user As String, ByVal page? As Integer, ByVal size? As Integer) As ActionResult

         Dim _viewdata As New UserProfileAnswerViewModel
         Dim useranswerservice As New UserAnswerService
         _viewdata.Answers = useranswerservice.GetUserAnswersByUser(user, If(page, 0), If(size, 10))

         Return View(_viewdata)
      End Function
   End Class
End Namespace