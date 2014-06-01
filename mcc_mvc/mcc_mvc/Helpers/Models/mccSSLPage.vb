Imports Microsoft.VisualBasic

Public Class mccSSLPage
   Inherits System.Web.UI.Page
   Private _mccSettings As RegistrySettings

   Public Property MccSettings() As RegistrySettings
      Get
         If _mccSettings Is Nothing Then
            _mccSettings = New RegistrySettings
         End If
         Return _mccSettings
      End Get
      Set(ByVal value As RegistrySettings)
         _mccSettings = value
      End Set
   End Property

   Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
      If Not Request.IsSecureConnection Then
         'SslTools.SwitchToSsl()
         'Response.Redirect(Request.Url.AbsoluteUri.Replace("http://", "https://"))

         Response.Redirect("https://" + Context.Request.ServerVariables("SERVER_NAME") + Context.Request.Url.PathAndQuery)
      End If
   End Sub

   Protected Sub RequestLogin()
      Dim path As String = ResolveUrl("~/mcc/users/loginpage.aspx?ReturnUrl=" & Me.Request.Url.LocalPath)
      'SslTools.Redirect("~/mcc/users/loginpage.aspx?ReturnUrl=" & Me.Request.Url.LocalPath, RedirectOptions.AbsoluteHttps)
   End Sub

End Class
