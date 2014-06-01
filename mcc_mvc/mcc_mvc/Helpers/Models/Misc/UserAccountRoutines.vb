Imports System.Data.SqlClient
Imports Microsoft.VisualBasic
Imports System.Net
Imports System.Net.Mail
Imports System.Configuration
Imports System.Web.Security
Imports System
Imports System.Data
Imports System.IO


Public Module UserRoutines

   '''' <summary>
   '''' Implemented for the registration process to check
   '''' whether or not a user with the same Username already Exists
   '''' </summary>
   '''' <param name="username">Taken from the Username Textbox</param>
   '''' <returns>tru if the username exists</returns>
   '''' <remarks>
   '''' i could use the membership.GetUser method and check whether
   '''' it is nothing or nost but if i didn't add the user yet...
   '''' </remarks>
   'Public Shared Function UserExists(ByVal username As String) As Boolean
   '   Dim bResult As Boolean = False
   '   Dim mUser As MembershipUserCollection = Membership.FindUsersByName(username)
   '   If mUser.Count <> 0 Then
   '      bResult = True
   '   Else
   '      bResult = False
   '   End If
   '   Return bResult
   'End Function

   Public ReadOnly Property BaseUrl() As String
      Get
         Dim url As String = HttpContext.Current.Request.ApplicationPath
         If url.EndsWith("/") Then
            Return url
         Else
            Return url + "/"
         End If
      End Get
   End Property

   Public ReadOnly Property FullBaseUrl() As String
      Get
         Return HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.PathAndQuery, "") + BaseUrl
      End Get
   End Property

   ''' <summary>
   ''' Implemented for the registration process to check
   ''' whether or not a user with the same Username already Exists</summary>
   ''' <param name="username">Taken from the Username Textbox</param>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Public Function UserExistsInMembership(ByVal username As String) As Boolean
      Dim bResult As Boolean = False
      Dim mUser As MembershipUser = Membership.GetUser(username)
      If mUser IsNot Nothing Then
         bResult = True
      End If
      Return bResult
   End Function

   Public ReadOnly Property CurrentUserName() As String
      Get
         Dim uname As String = ""
         If HttpContext.Current.User.Identity.IsAuthenticated Then
            uname = HttpContext.Current.User.Identity.Name
         End If
         Return uname
      End Get
   End Property

   ''' <summary>
   ''' Activation mail sent to the user
   ''' 
   ''' </summary>
   ''' <param name="userID"></param>
   ''' <param name="emailAdress"></param>
   ''' <returns></returns>
   ''' <remarks>mail message :: [to be formated]</remarks>
   Public Function SendNewUserActivationMail(ByVal userID As String, ByVal emailAdress As String) As Boolean
      Dim bSendResult As Boolean = False
      Try

         If File.Exists(HttpContext.Current.Server.MapPath("/app/mailtemplates/ActivationMail.txt")) Then
            Dim sr_abstr As StreamReader = New StreamReader(HttpContext.Current.Server.MapPath("/app/mailtemplates/ActivationMail.txt"))
            Dim mailbody As String = String.Empty
            If sr_abstr IsNot Nothing Then
               mailbody = sr_abstr.ReadToEnd()
            End If

            Dim message As MailMessage = New MailMessage()
            message.To.Add(New MailAddress(emailAdress))
            message.Subject = "MiddleClassCrunch Account Activation"
            message.IsBodyHtml = True
            Dim basePath As String = FullBaseUrl()
            Dim activationLink As String = basePath & "activation?uuid=" & userID

            Dim strbody As New StringBuilder()
            strbody.AppendFormat(mailbody, activationLink)

            message.Body = strbody.ToString
            Dim client As SmtpClient = New SmtpClient()
            client.Send(message)
            bSendResult = True
         Else
            Dim message As MailMessage = New MailMessage()
            message.To.Add(New MailAddress(emailAdress))
            message.Subject = "MiddleClassCrunch Account Activation"
            message.IsBodyHtml = True
            Dim basePath As String = FullBaseUrl()
            Dim activationLink As String = basePath & "activation?uuid=" & userID

            Dim strbody As New StringBuilder()
            strbody.Append("Thank you for registering at The MiddleClassCrunch website!<br/>")
            strbody.Append("Please click on the following link or open it in your browser to activate your account.<br/>")
            strbody.Append("<br/>")
            strbody.AppendFormat("{0}<br/>", activationLink)
            strbody.Append("<br/>")
            strbody.Append("- If you did not register at this web site, just ignore this email.<br/>")
            strbody.Append("- If you did register on our website but fail to activate your account within 72 hours, it will be removed automatically.<br/>")
            strbody.Append("- If the link does not work, your account has probably already been desactivated.You will have to create a new one on the website.<br/>")
            strbody.Append("<br/>")
            strbody.Append("See you online!<br/>")
            strbody.Append("- the MiddleClassCrunch Team <br/>")
            strbody.Append("<br/>")
            strbody.Append("www.middleclasscrunch.com<br/>")

            message.Body = strbody.ToString
            Dim client As SmtpClient = New SmtpClient()
            client.Send(message)
            bSendResult = True
         End If
      Catch ex As Exception

      End Try
      Return False
   End Function


   Public Function SendMail(ByVal username As String, ByVal emailAdress As String, ByVal subject As String, ByVal body As String) As Boolean
      Dim bSendResult As Boolean = False
      Try

         Dim message As MailMessage = New MailMessage()
         message.To.Add(New MailAddress(emailAdress))
         message.Subject = String.Format("New answer notification: {0}", subject)
         message.IsBodyHtml = True

         message.Body = body
         Dim client As SmtpClient = New SmtpClient()
         client.Send(message)
         bSendResult = True

      Catch ex As Exception

      End Try
      Return False
   End Function

   ''' <summary>
   ''' Inside call to the function
   ''' </summary>
   ''' <param name="Username"></param>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Public Function IsUserActivated(ByVal Username As String) As Boolean
      Dim mUser As MembershipUser = Membership.GetUser(Username)

      If mUser IsNot Nothing Then
         Return mUser.IsApproved
      Else
         Return False
      End If
   End Function

   Function IsUserLockedOut(ByVal userName As String) As Boolean
      Dim mUser As MembershipUser = Membership.GetUser(userName)
      If mUser IsNot Nothing Then
         Return mUser.IsLockedOut
      Else
         Return False
      End If
   End Function

   Function IsUserLockedOut(ByVal userID As Guid) As Boolean
      Dim mUser As MembershipUser = Membership.GetUser(userID)
      If mUser IsNot Nothing Then
         Return mUser.IsLockedOut
      Else
         Return False
      End If
   End Function

   ''' <summary>
   ''' Outside call to the function
   ''' </summary>
   ''' <param name="UserID"></param>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Public Function IsUserActivatedID(ByVal UserID As Guid) As Boolean
      Dim mUser As MembershipUser = Membership.GetUser(UserID)
      If mUser IsNot Nothing Then
         Return mUser.IsApproved
      Else
         Return False
      End If
   End Function

   Public Function ActivateUser(ByVal UserID As Guid) As Boolean
      Dim mUser As MembershipUser = Membership.GetUser(UserID)
      If mUser IsNot Nothing AndAlso Not mUser.IsLockedOut AndAlso Not mUser.IsApproved Then
         mUser.IsApproved = True
         Membership.UpdateUser(mUser)
         Return True
      End If
      Return False
   End Function

   Public ReadOnly Property IsAdministrator() As Boolean
      Get
         Return HttpContext.Current.User.Identity.IsAuthenticated AndAlso HttpContext.Current.User.IsInRole("Administrators")
      End Get
   End Property

   Public ReadOnly Property IsEditor() As Boolean
      Get
         Return HttpContext.Current.User.Identity.IsAuthenticated AndAlso (HttpContext.Current.User.IsInRole("Administrators") _
                    OrElse HttpContext.Current.User.IsInRole("Editors"))
      End Get
   End Property


   Public ReadOnly Property HasAdminCredential() As Boolean
      Get
         Return HttpContext.Current.User.Identity.IsAuthenticated AndAlso (HttpContext.Current.User.IsInRole("Administrators") _
                   OrElse HttpContext.Current.User.IsInRole("Editors")) OrElse (HttpContext.Current.User.IsInRole("Contributors"))
      End Get
   End Property



   Public ReadOnly Property UserIsAuthenticated() As Boolean
      Get
         Return HttpContext.Current.User.Identity.IsAuthenticated
      End Get
   End Property


   Public Function LogUserToForum(ByVal boardId As Integer, ByVal username As String, ByVal password As String) As Integer

      Dim hashPassword As String = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "md5")
      Dim params As Object() = {1, username, FormsAuthentication.HashPasswordForStoringInConfigFile(password, "md5")}
      Dim strConn As String = ConfigurationManager.ConnectionStrings("LocalSqlServer").ConnectionString

      Using cn As New SqlConnection(strConn)
         Dim cmd As New SqlCommand("yaf_user_login", cn)
         cmd.CommandType = CommandType.StoredProcedure
         cmd.Parameters.Add("@BoardId", SqlDbType.Int).Value = boardId
         cmd.Parameters.Add("@Name", SqlDbType.NVarChar).Value = username
         cmd.Parameters.Add("@Password", SqlDbType.NVarChar).Value = hashPassword
         cn.Open()

         Dim userID As Object = cmd.ExecuteScalar()
         If userID IsNot DBNull.Value Then
            Return userID
         Else
            Return 0
         End If

      End Using
   End Function


   Public Sub UpdateUserForForums(ByVal UserID As Integer, ByVal boardId As Integer, ByVal username As String, ByVal password As String, ByVal email As String, ByVal hash As String, ByVal location As String, ByVal homepage As String, ByVal Approved As Boolean, ByVal PMNotification As Boolean)

      'yaf.DB.user_save(UserID, boardId, username, password, email, Nothing, Nothing, Nothing, 0, Nothing, Nothing, Nothing, Approved, Nothing, Nothing, Nothing, Nothing, Nothing, _
      '                 Nothing, Nothing, Nothing, Nothing, PMNotification)

   End Sub

   Function AreSameUser(ByVal username As String) As Boolean
      If IsAuthenticated() Then
         Dim currentUsername = HttpContext.Current.User.Identity.Name
         If String.Equals(currentUsername, username, StringComparison.OrdinalIgnoreCase) Then
            Return True
         End If
      End If
      Return False
   End Function


   Function SendActivationCodeToUser(ByVal username As String, ByVal email As String, ByVal usercode As String) As Boolean
      Try


         Dim altLink As String = FullBaseUrl & "/resetpassword?email=" & HttpContext.Current.Server.UrlEncode(email) & "&n=" & usercode & "&s=1"

         Dim message As MailMessage = New MailMessage()
         message.To.Add(New MailAddress(email))
         message.Subject = "Account Password Reset - MiddleClassCrunch"
         message.IsBodyHtml = True

         Dim strbody As New StringBuilder()

         strbody.AppendFormat("Hello {0},<br/>", username)
         strbody.Append("You recently requested a new password.<br/>")
         strbody.Append("<br/>")
         strbody.AppendFormat("Here is your reset code, which you can enter on the password reset page:<br/>")
         strbody.AppendFormat("{0}<br/>", usercode)
         strbody.Append("<br/>")
         strbody.Append("You can also reset your password by following the link below:<br/>")
         strbody.AppendFormat("{0}<br/>", altLink)
         strbody.Append("<br/>")
         strbody.Append("Please note that this email has been sent to all contact emails associated with your account.<br/>")
         strbody.Append("If you did not reset your password, please disregard this message.<br/>")
         strbody.Append("<br/>")
         strbody.Append("See you online,<br/>")
         strbody.Append("- The MiddleClassCrunch Team<br/>")


         message.Body = strbody.ToString

         message.Body = strbody.ToString
         Dim client As SmtpClient = New SmtpClient()
         client.Send(message)
         Return True
      Catch ex As Exception
         Return False
      End Try


      Return False
   End Function
End Module
