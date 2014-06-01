' SslHelper by Dominick Baier - www.leastprivilege.com

Imports System
Imports System.Data
Imports System.Configuration
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls

Public Enum RedirectOptions
    Relative
    AbsoluteHttp
    AbsoluteHttps
    AbsoluteIisConfig
End Enum

Public Enum ProtocolOptions
    Http
    Https
End Enum

Public Class SslTools
    Public Shared Sub Redirect(ByVal url As String)
        Redirect(url, RedirectOptions.Relative)
    End Sub
    
    Public Shared Sub Redirect(ByVal url As String, ByVal options As RedirectOptions)
        Dim context As HttpContext = HttpContext.Current
        Dim absolutePath As String = ""
        
        If options = RedirectOptions.Relative Then
            context.Response.Redirect(url)
            Exit Sub
        End If
        
        If options = RedirectOptions.AbsoluteHttp Then
            absolutePath = GetAbsoluteUrl(url, ProtocolOptions.Http)
        End If
        
        If options = RedirectOptions.AbsoluteHttps Then
            absolutePath = GetAbsoluteUrl(url, ProtocolOptions.Https)
        End If
        
        If options = RedirectOptions.AbsoluteIisConfig Then
            Throw New NotImplementedException()
        End If
        
        context.Response.Redirect(absolutePath)
    End Sub
    
    Public Shared Function GetAbsoluteUrl(ByVal url As String, ByVal protocol As ProtocolOptions) As String
        If url Is Nothing Then
            Return url
        End If
        
        ' check for querystring parameters
        Dim path As String, query As String
        If url.Contains("?") Then
            Dim qpos As Integer = url.IndexOf("?"C)
            path = url.Substring(0, qpos)
            query = url.Substring(qpos)
        Else
            path = url
            query = ""
        End If

      If Not path.ToLower.StartsWith("http") Then
         If VirtualPathUtility.IsAppRelative(path) Then
            path = VirtualPathUtility.ToAbsolute(path)
         End If
      End If

      Dim baseUri As Uri
      Dim hostName As String = HttpContext.Current.Request.Url.Host

      If protocol = ProtocolOptions.Http Then
         baseUri = New Uri([String].Format("http://{0}", hostName))
      Else
         baseUri = New Uri([String].Format("https://{0}", hostName))
      End If

      Return New Uri(baseUri, path).ToString() + query
   End Function
    
    Public Shared Sub SwitchToSsl()
      Dim context As HttpContext = HttpContext.Current
      'Redirect(context.Request.Url.AbsolutePath + context.Request.Url.Query, RedirectOptions.AbsoluteHttps)
    End Sub
    
    Public Shared Sub SwitchToClearText()
        Dim context As HttpContext = HttpContext.Current
        Redirect(context.Request.Url.ToString(), RedirectOptions.AbsoluteHttp)
    End Sub
End Class
