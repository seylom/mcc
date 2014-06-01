Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Configuration
Imports System.Collections
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports MCC.routines

Public Class mccPage
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


   Public ReadOnly Property BaseUrl() As String
      Get
         Dim url As String = Me.Request.ApplicationPath
         If url.EndsWith("/") Then
            Return url
         Else
            Return url + "/"
         End If
      End Get
   End Property

   Public ReadOnly Property FullBaseUrl() As String
      Get
         Return Me.Request.Url.AbsoluteUri.Replace(Me.Request.RawUrl, "") + Me.BaseUrl
      End Get
   End Property

   Protected Sub RequestLogin()
      SslTools.Redirect("~/mcc/users/loginpage.aspx?ReturnUrl=" & Me.Request.Url.LocalPath, RedirectOptions.AbsoluteHttps)
   End Sub

   Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
      If Request.IsSecureConnection Then
         Response.Redirect("http://" + Context.Request.ServerVariables("SERVER_NAME") + Context.Request.Url.PathAndQuery)
      End If

   End Sub

   'Private Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
   '   'If Not String.IsNullOrEmpty(MccSettings.Theme) Then
   '   '   Page.Theme = MccSettings.Theme
   '   'End If
   'End Sub

   ''' <summary>
   ''' Adds a JavaScript reference to the HTML head tag.
   ''' Usualy this method will be place in our BasePage
   ''' </summary>
   Protected Overridable Sub AddJavaScriptInclude(ByVal path As String)
      Dim script As New HtmlGenericControl("script")
      script.Attributes("type") = "text/javascript"

      ' Change the 'src' to jslib.axd file, but keep the releative directory (for relative urls in the js file)
      script.Attributes("src") = ResolveUrl(path).Replace(System.IO.Path.GetFileName(path), "jslib.axd?d=" & Server.UrlEncode(path))
      Page.Header.Controls.Add(script)
   End Sub


   Public Sub getScript(ByVal name As String, ByVal url As String)
      Dim cs As ClientScriptManager = Page.ClientScript
      If Not cs.IsClientScriptIncludeRegistered(name) Then
         cs.RegisterClientScriptInclude(name, ResolveUrl(url))
      End If
   End Sub
End Class