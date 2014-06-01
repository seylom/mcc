
Public Class CaptchaValidatorAttribute
   Inherits ActionFilterAttribute
   Private Const CHALLENGE_FIELD_KEY As String = "recaptcha_challenge_field"
   Private Const RESPONSE_FIELD_KEY As String = "recaptcha_response_field"

   Public Overloads Overrides Sub OnActionExecuting(ByVal filterContext As ActionExecutingContext)
      Dim captchaChallengeValue = filterContext.HttpContext.Request.Form(CHALLENGE_FIELD_KEY)
      Dim captchaResponseValue = filterContext.HttpContext.Request.Form(RESPONSE_FIELD_KEY)
      Dim captchaValidtor = New Recaptcha.RecaptchaValidator With {.PrivateKey = "6LeFJgwAAAAAANJ4LxmPSU94lpBQplmCbON0xC4l", _
                                                                   .Challenge = captchaChallengeValue, _
                                                                   .RemoteIP = filterContext.HttpContext.Request.UserHostAddress, _
                                                                   .Response = captchaResponseValue}
      Dim KEY As String = ""

      Dim recaptchaResponse = captchaValidtor.Validate()

      ' this will push the result value into a parameter in our Action
      filterContext.ActionParameters("CaptchaIsValid") = recaptchaResponse.IsValid

      MyBase.OnActionExecuting(filterContext)
   End Sub
End Class


Public Class UserNameFilter
   Inherits ActionFilterAttribute
   Public Overloads Overrides Sub OnActionExecuting(ByVal filterContext As ActionExecutingContext)
      Const Key As String = "userName"

      If filterContext.ActionParameters.ContainsKey(Key) Then
         If filterContext.HttpContext.User.Identity.IsAuthenticated Then
            filterContext.ActionParameters(Key) = filterContext.HttpContext.User.Identity.Name
         End If
      End If

      MyBase.OnActionExecuting(filterContext)
   End Sub
End Class

Public Class ArticleRatingFilter
   Inherits ActionFilterAttribute
   Public Overloads Overrides Sub OnActionExecuting(ByVal filterContext As ActionExecutingContext)
      Const Key As String = "rating"

      If filterContext.ActionParameters.ContainsKey(Key) Then
         If filterContext.HttpContext.User.Identity.IsAuthenticated Then
            Dim rating As Integer = 0
            Dim cookie As HttpCookie = filterContext.HttpContext.Request.Cookies("Rating_Article")
            If cookie IsNot Nothing Then
               rating = Integer.Parse(cookie.Value)
            End If
            filterContext.ActionParameters(Key) = rating
         End If
      End If

      MyBase.OnActionExecuting(filterContext)
   End Sub
End Class

