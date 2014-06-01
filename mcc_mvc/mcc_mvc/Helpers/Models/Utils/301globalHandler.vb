Public Class The301GlobalHandler
   Inherits MvcHandler
   Public Sub New(ByVal requestContext As RequestContext)
      MyBase.New(requestContext)
   End Sub

   Protected Overloads Overrides Sub ProcessRequest(ByVal httpContext As HttpContextBase)
      'We do not want this handler to process local requests
      'If httpContext.Request.IsLocal Then
      '   MyBase.ProcessRequest(httpContext)
      'Else
      '   ProcessExternalRequest(httpContext)
      'End If

      ProcessExternalRequest(httpContext)
   End Sub

   Private Sub ProcessExternalRequest(ByVal httpContext As HttpContextBase)
      Dim urlChanged As Boolean = False
      Dim url As String = RequestContext.HttpContext.Request.Url.AbsoluteUri
      'Check for non-www version URL
      If Not RequestContext.HttpContext.Request.Url.AbsoluteUri.Contains("www") Then
         urlChanged = True
         'change to www. version URL
         url = url.Replace("http://", "http://www.")
      End If
      ProcessExternalRequest(url, urlChanged, httpContext)
   End Sub

   Private Sub ProcessExternalRequest(ByVal url As String, ByVal urlChanged As Boolean, ByVal httpContext As HttpContextBase)
      If urlChanged Then
         'mark as 301
         httpContext.Response.Status = "301 Moved Permanently"
         httpContext.Response.StatusCode = 301
         httpContext.Response.AppendHeader("Location", url)
      Else
         MyBase.ProcessRequest(httpContext)
      End If
   End Sub
End Class
