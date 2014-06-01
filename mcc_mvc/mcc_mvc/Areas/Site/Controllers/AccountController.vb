Imports System.Globalization
Imports System.Security.Principal
Imports MCC.Services
Imports DotNetOpenAuth.OpenId.RelyingParty
Imports DotNetOpenAuth.Messaging
Imports DotNetOpenAuth.OpenId
Imports DotNetOpenAuth.OpenId.Extensions.SimpleRegistration
Imports DotNetOpenAuth.OpenId.Extensions.AttributeExchange


Namespace MCC.Areas.Site.Controllers
   <HandleError()> _
   Public Class AccountController
        Inherits ApplicationController

      Class PasswordResetViewModel
         Inherits baseViewModel

         Dim _usercode As String = String.Empty
         Property UserCode() As String
            Get
               Return _usercode
            End Get
            Set(ByVal value As String)
               _usercode = value
            End Set
         End Property

         Dim _username As String = String.Empty
         Property Username() As String
            Get
               Return _username
            End Get
            Set(ByVal value As String)
               _username = value
            End Set
         End Property

         Dim _newPassword As String = String.Empty
         Property newPassword() As String
            Get
               Return _newPassword
            End Get
            Set(ByVal value As String)
               _newPassword = value
            End Set
         End Property

         Dim _confirmPassword As String = String.Empty
         Property ConfirmPassword() As String
            Get
               Return _confirmPassword
            End Get
            Set(ByVal value As String)
               _confirmPassword = value
            End Set
         End Property
      End Class

      Private _formsAuth As IFormsAuthentication
      Private _service As IMembershipService

      ' This constructor is used by the MVC framework to instantiate the controller using
      ' the default forms authentication and membership providers.

      Sub New()
         Me.New(Nothing, Nothing)
      End Sub

      ' This constructor is not used by the MVC framework but is instead provided for ease
      ' of unit testing this type. See the comments at the end of this file for more
      ' information.

      Sub New(ByVal formsAuth As IFormsAuthentication, ByVal service As IMembershipService)
         _formsAuth = If(formsAuth, New FormsAuthenticationService())
         _service = If(service, New AccountMembershipService())
      End Sub

      ReadOnly Property FormsAuth() As IFormsAuthentication
         Get
            Return _formsAuth
         End Get
      End Property

      ReadOnly Property MembershipService() As IMembershipService
         Get
            Return _service
         End Get
      End Property

      Function LogOn() As ActionResult
         If UserIsAuthenticated Then
            Return Redirect("/")
         End If
         Dim _viewdata As New loginViewModel
         Return View(_viewdata)
      End Function

      <AcceptVerbs(HttpVerbs.Post)> _
      <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", _
              Justification:="Needs to take same parameter type as Controller.Redirect()")> _
      Function LogOn(ByVal loginModel As loginViewModel) As ActionResult

         If loginModel Is Nothing Then
            Return RedirectToAction("Error", New With {.controller = "General"})
         End If

         If (loginModel.ReturnUrl IsNot Nothing AndAlso loginModel.ReturnUrl.IndexOf("/login", StringComparison.OrdinalIgnoreCase) >= 0) Then
            loginModel.ReturnUrl = Nothing
         End If

         If Not ValidateLogOn(loginModel) Then
            loginModel.Messages.Add("Invalid Username of password.")
            Return View(loginModel)
         End If

         FormsAuth.SignIn(loginModel.Username, loginModel.RememberMe)
         If Not String.IsNullOrEmpty(loginModel.ReturnUrl) Then
            Return Redirect(loginModel.ReturnUrl)
         Else
            Return RedirectToAction("Index", "Home")
         End If

      End Function

      Function LogOff() As ActionResult
         FormsAuth.SignOut()
         Return RedirectToAction("Index", "Home")
      End Function

      Function Register() As ActionResult
         If UserIsAuthenticated Then
            Return Redirect("/")
         End If

         Dim _viewdata As New registerViewModel
         Return View(_viewdata)
      End Function

      <AcceptVerbs(HttpVerbs.Post), CaptchaValidator()> _
      Function Register(ByVal reg As registerViewModel, ByVal CaptchaIsValid As Boolean) As ActionResult

         If reg Is Nothing Then
            Return RedirectToAction("Error", New With {.controller = "General"})
         End If

         If Not CaptchaIsValid Then
            reg.Messages.Add("Invalid Code Entered")
            Return View(reg)
         End If

         If ValidateRegistration(reg) Then
            Dim createStatus As MembershipCreateStatus = MembershipService.CreateUser(reg.Username, reg.Password, reg.Email)

            If createStatus = MembershipCreateStatus.Success Then
               Dim muser As MembershipUser = Membership.GetUser(reg.Username)
               If muser IsNot Nothing Then
                  muser.IsApproved = False
                  Membership.UpdateUser(muser)
                  Dim id As String = muser.ProviderUserKey.ToString
                  UserRoutines.SendNewUserActivationMail(id, reg.Email)
                  Return RedirectToAction("ActivationReq")
               Else
                  reg.Messages.Add("A problem occured while creating this account, an email has been automatically sent to us")
                  reg.Messages.Add("We will be investigating this issue shortly. Sorry for the inconvenience")
                  Return View(reg)
               End If
            Else
               reg.Messages.Add(ErrorCodeToString(createStatus))
               View(reg)
            End If
         Else
            View(reg)
         End If

         Return View(reg)
      End Function

      Protected Overrides Sub OnActionExecuting(ByVal filterContext As System.Web.Mvc.ActionExecutingContext)
         If TypeOf filterContext.HttpContext.User.Identity Is WindowsIdentity Then
            Throw New InvalidOperationException("Windows authentication is not supported.")
         End If
      End Sub
#Region "Validation Methods"

      Private Function ValidateChangePassword(ByVal currentPassword As String, ByVal newPassword As String, ByVal confirmPassword As String) As Boolean
         If String.IsNullOrEmpty(currentPassword) Then
            ModelState.AddModelError("currentPassword", "You must specify a current password.")
         End If

         If newPassword Is Nothing OrElse newPassword.Length < MembershipService.MinPasswordLength Then
            ModelState.AddModelError("newPassword", String.Format(CultureInfo.CurrentCulture, _
                                                   "You must specify a new password of {0} or more characters.", _
                                                   MembershipService.MinPasswordLength))
         End If

         If Not String.Equals(newPassword, confirmPassword, StringComparison.Ordinal) Then
            ModelState.AddModelError("_FORM", "The new password and confirmation password do not match.")
         End If

         Return ModelState.IsValid
      End Function

      Private Function ValidateLogOn(ByVal loginModel As loginViewModel) As Boolean
         Dim isValid As Boolean = True
         If String.IsNullOrEmpty(loginModel.Username) Then
            loginModel.Messages.Add("You must specify a username.")
            isValid = False
         End If

         If String.IsNullOrEmpty(loginModel.Password) Then
            loginModel.Messages.Add("You must specify a password.")
            isValid = False
         End If

         If Not MembershipService.ValidateUser(loginModel.Username, loginModel.Password) Then
            loginModel.Messages.Add("The username or password provided is incorrect.")
            isValid = False
         End If

         Return isValid
      End Function

      Private Function ValidateRegistration(ByVal model As registerViewModel) As Boolean
         Dim valid As Boolean = True
         If String.IsNullOrEmpty(model.Username) Then
            model.Messages.Add("You must specify a username.")
            valid = False
         End If

         If String.IsNullOrEmpty(model.Email) Then
            model.Messages.Add("You must specify an email address.")
            valid = False
         End If

         If model.Password Is Nothing OrElse model.Password.Length < MembershipService.MinPasswordLength Then
            model.Messages.Add(String.Format(CultureInfo.CurrentCulture, _
                                                   "You must specify a password of {0} or more characters.", _
                                                   MembershipService.MinPasswordLength))
            valid = False
         End If

         If Not String.Equals(model.Password, model.ConfirmPassword, StringComparison.Ordinal) Then
            model.Messages.Add("The new password and confirmation password do not match.")
            valid = False
         End If

         Return valid
      End Function

      Private Shared Function ErrorCodeToString(ByVal createStatus As MembershipCreateStatus) As String
         ' See http://msdn.microsoft.com/en-us/library/system.web.security.membershipcreatestatus.aspx for
         ' a full list of status codes.
         Select Case createStatus
            Case MembershipCreateStatus.DuplicateUserName
               Return "Username already exists. Please enter a different user name."

            Case MembershipCreateStatus.DuplicateEmail
               Return "A username for that e-mail address already exists. Please enter a different e-mail address."

            Case MembershipCreateStatus.InvalidPassword
               Return "The password provided is invalid. Please enter a valid password value."

            Case MembershipCreateStatus.InvalidEmail
               Return "The e-mail address provided is invalid. Please check the value and try again."

            Case MembershipCreateStatus.InvalidAnswer
               Return "The password retrieval answer provided is invalid. Please check the value and try again."

            Case MembershipCreateStatus.InvalidQuestion
               Return "The password retrieval question provided is invalid. Please check the value and try again."

            Case MembershipCreateStatus.InvalidUserName
               Return "The user name provided is invalid. Please check the value and try again."

            Case MembershipCreateStatus.ProviderError
               Return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator."

            Case MembershipCreateStatus.UserRejected
               Return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator."

            Case Else
               Return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator."
         End Select

      End Function
#End Region


      Function NewRegistrationMailReq() As ActionResult
         Return View()
      End Function


      Function ResetPassword() As ActionResult
         Dim _viewdata As New resetInfoViewModel()
         Return View(_viewdata)
      End Function


      <AcceptVerbs(HttpVerbs.Post)> _
      Function ResetPassword(ByVal info As resetInfoViewModel) As ActionResult

         If String.IsNullOrEmpty(info.UserInfo) Then
            info.Messages.Add("Enter your user information")
            Return View(info)
         End If

         ' check username or email
         Dim email As String = String.Empty
         Dim username As String = String.Empty
         Dim userId As Guid = Nothing

         If routines.ValidateEmail(info.UserInfo) Then
            email = info.UserInfo
            username = Membership.GetUserNameByEmail(info.UserInfo)
         Else
            username = info.UserInfo
         End If

         Dim muser As MembershipUser = Membership.GetUser(username)
         If muser Is Nothing Then
            info.Messages.Add("user not found. Please check the username or email entered!")
            Return View(info)
         End If

         email = muser.Email
         userId = muser.ProviderUserKey

         Dim _IResetService As IResetCodeService = New ResetCodeService()
         Dim newUserCode As String = _IResetService.AssignUsercode(userId)
         _IResetService.SaveResetCode(userId, newUserCode)

         Dim prd As New PasswordResetViewModel With {.UserCode = newUserCode, .Username = username}

         Dim result As Boolean = SendActivationCodeToUser(username, email, newUserCode)
         If Not result Then
            info.Messages.Add("We are currently unable to issue the email for your account to be reset.")
            Return View(info)
         End If

         ' go to next step
         Return RedirectToAction("SetActivationCode", prd)
      End Function

      'Function SetActivationCode(ByVal prd As PasswordResetData) As ActionResult


      '   Return View()
      'End Function

      Function SetActivationCode() As ActionResult
         Dim _viewdata As New PasswordResetViewModel
         Return View(_viewdata)
      End Function


      <AcceptVerbs(HttpVerbs.Post)> _
      Function SetActivationCode(ByVal prd As PasswordResetViewModel) As ActionResult


         UpdateModel(prd)
         If Not ModelState.IsValid Then
            Return View(prd)
         End If

         If String.IsNullOrEmpty(prd.Username) Then
            prd.Messages.Add("Please Enter the code received in the Email")
            Return View(prd)
         End If

         If String.IsNullOrEmpty(prd.UserCode) Then
            prd.Messages.Add("Please Enter the code received in the Email")
            Return View(prd)
         End If

         ' verify code and Move to Last step    
         Dim _IResetService As IResetCodeService = New ResetCodeService()
         If Not _IResetService.ValidateUsercode(Membership.GetUser(prd.Username).ProviderUserKey, prd.UserCode) Then
            prd.Messages.Add("Invalid activation code. Pease make sure the activation code was entered correctly")
            Return View(prd)
         End If

         Return RedirectToAction("SetNewPasswordForReset", prd)

      End Function

      Function SetNewPasswordForReset() As ActionResult
         Dim _viewdata As New PasswordResetViewModel
         Return View(_viewdata)
      End Function

      <AcceptVerbs(HttpVerbs.Post)> _
      Function SetNewPasswordForReset(ByVal prd As PasswordResetViewModel) As ActionResult

         If Not ValidateResetPassword(prd) Then
            Return View(prd)
         End If


         If String.IsNullOrEmpty(prd.Username) Then
            prd.Messages.Add("User not found.")
            Return View(prd)
         End If

         Dim muser As MembershipUser = Membership.GetUser(prd.Username)
         If muser Is Nothing Then
            prd.Messages.Add("User not found.")
            Return View(prd)
         End If

         Dim cp As Boolean = MembershipService.ChangePassword(prd.Username, muser.GetPassword, prd.newPassword)
         If Not cp Then
            prd.Messages.Add("The password change failed. Please contact us for more information on the problem.")
            Return View(prd)
         End If

         ' send a password changed verification mail
         ' authenticate the user

         _formsAuth.SignIn(prd.Username, False)

         Dim _iresetService As IResetCodeService = New ResetCodeService()
         _iresetService.DeleteResetCode(Membership.GetUser(prd.Username).ProviderUserKey)

         Return RedirectToAction("PasswordResetSuccess")
      End Function


      Function PasswordResetSuccess() As ActionResult
         Return View()
      End Function

      Function ActivationReq() As ActionResult
         Return View()
      End Function

      Function Activation(ByVal uuid As String) As ActionResult

         If String.IsNullOrEmpty(uuid) Then
            Return Redirect("Error")
         End If
         Dim _viewdata As New ActivationViewModel
         Dim userID As Guid = New Guid(uuid)
         If IsUserActivatedID(userID) Then
            Return RedirectToAction("AccountActivation")
         End If

         Dim res As Boolean = ActivateUser(userID)

         If IsUserLockedOut(userID) Then
            _viewdata.LockedOut = True
            Return View(_viewdata)
         End If

         _viewdata.Approved = res
         If res Then
            Dim muser As MembershipUser = Membership.GetUser(New Guid(uuid))
            If muser IsNot Nothing Then
               _formsAuth.SignIn(muser.UserName, False)
            End If
         End If

         Return View(_viewdata)
      End Function

      Function AccountActivation() As ActionResult
         Return View()
      End Function

      Function NonActiveAccount() As ActionResult
         Return View()
      End Function

      Private Function ValidateResetPassword(ByVal model As PasswordResetViewModel) As Boolean
         Dim IsValid As Boolean = True
         If String.IsNullOrEmpty(model.newPassword) OrElse String.IsNullOrEmpty(model.ConfirmPassword) OrElse Not model.newPassword.Equals(model.ConfirmPassword) Then
            model.Messages.Add("Please enter your new password and verification to match.")
            IsValid = False
         End If

         If model.newPassword Is Nothing OrElse model.newPassword.Length < MembershipService.MinPasswordLength Then
            model.Messages.Add(String.Format(CultureInfo.CurrentCulture, _
                                                   "You must specify a new password of {0} or more characters.", _
                                                   MembershipService.MinPasswordLength))
            IsValid = False
         End If
         Return IsValid
      End Function



      <ValidateInput(False)> _
      Public Function AuthenticateWithOpenId(ByVal returnUrl As String) As ActionResult
         Dim openid As OpenIdRelyingParty = New OpenIdRelyingParty()
         Dim response = openid.GetResponse()

         If response Is Nothing Then
            ' Stage 2: user submitting Identifier
            Dim id As Identifier
            If Identifier.TryParse(Request.Form("openid_identifier"), id) Then
               Try
                  Dim req As IAuthenticationRequest = openid.CreateRequest(Request.Form("openid_identifier"))

                  Dim fetch = New FetchRequest()
                  fetch.Attributes.AddRequired(WellKnownAttributes.Contact.Email)
                  req.AddExtension(fetch)

                  Return req.RedirectingResponse.AsActionResult()
               Catch ex As ProtocolException
                  ViewData("Message") = ex.Message
                  Return View("Login")
               End Try
            Else
               ViewData("Message") = "Invalid identifier"
               Return View("Login")
            End If
         Else
            ' Stage 3: OpenID Provider sending assertion response
            Select Case response.Status
               Case AuthenticationStatus.Authenticated
                  Session("FriendlyIdentifier") = response.FriendlyIdentifierForDisplay

                  Dim fetch = response.GetExtension(Of FetchResponse)()
                  Dim email As String = String.Empty
                  If (fetch IsNot Nothing) Then
                     email = fetch.GetAttributeValue(WellKnownAttributes.Contact.Email)
                  End If

                  Dim uname As String = String.Empty
                  If Not String.IsNullOrWhiteSpace(email) Then
                     uname = Membership.GetUserNameByEmail(email)
                  End If

                  If Not String.IsNullOrWhiteSpace(uname) Then
                     Dim mUser As MembershipUser = Membership.GetUser(uname)
                     If mUser IsNot Nothing Then
                        FormsAuthentication.SetAuthCookie(mUser.UserName, False)
                     Else
                        'associate user to profile
                     End If
                  Else
                     FormsAuthentication.SetAuthCookie(response.ClaimedIdentifier, False)
                  End If

                  If Not String.IsNullOrEmpty(returnUrl) Then
                     Return Redirect(returnUrl)
                  Else
                     Return RedirectToAction("Index", "Home")
                  End If

               Case AuthenticationStatus.Canceled
                  ViewData("Message") = "Canceled at provider"
                  Return View("Login")
               Case AuthenticationStatus.Failed
                  ViewData("Message") = response.Exception.Message
                  Return View("Login")
            End Select
         End If
         Return New EmptyResult()
      End Function


   End Class

   ' The FormsAuthentication type is sealed and contains static members, so it is difficult to
   ' unit test code that calls its members. The interface and helper class below demonstrate
   ' how to create an abstract wrapper around such a type in order to make the AccountController
   ' code unit testable.

   Public Interface IFormsAuthentication

      Sub SignIn(ByVal userName As String, ByVal createPersistentCookie As Boolean)
      Sub SignOut()

   End Interface


   Public Class FormsAuthenticationService
      Implements IFormsAuthentication

      Public Sub SignIn(ByVal userName As String, ByVal createPersistentCookie As Boolean) Implements IFormsAuthentication.SignIn
         FormsAuthentication.SetAuthCookie(userName, createPersistentCookie)
      End Sub

      Public Sub SignOut() Implements IFormsAuthentication.SignOut
         FormsAuthentication.SignOut()
      End Sub

   End Class

End Namespace