Imports Microsoft.VisualBasic

Public Class Subdomains
   Implements IHttpModule

   Public Sub Init(ByVal app As HttpApplication) Implements IHttpModule.Init
      AddHandler app.BeginRequest, AddressOf ppBeginRequest
      AddHandler app.EndRequest, AddressOf ppEndRequest
   End Sub

   Public Sub Dispose() Implements IHttpModule.Dispose
   End Sub

   Public Sub ppBeginRequest(ByVal s As Object, ByVal e As EventArgs)
      Dim app As HttpApplication
      Dim urlArr() As String
      app = CType(s, HttpApplication)
      Dim strHostDomain As String = app.Context.Request.ServerVariables("SERVER_NAME")

      urlArr = strHostDomain.Split(".")
      If urlArr.Length = 3 Then
         If urlArr(0).Substring(0, 3) = "www" Then
            ' do nothing here .. normal site
         End If

         If urlArr(0) = "videos" Then  ' Subdomain videos: http://videos.midddleclasscrunch.com 
            app.Context.RewritePath("/videos/")
            ' app.Context.Request.Url.PathAndQuery = if you need the querystring
         End If

         If urlArr(0) = "tools" Then  ' Subdomain tools: http://tools.midddleclasscrunch.com 
            app.Context.RewritePath("/tools/")
            ' app.Context.Request.Url.PathAndQuery = if you need the querystring
         End If
      Else
         'If urlArr.Length = 2 Then
         '   If urlArr(0).ToLower = "middleclasscrunch" AndAlso app.Context.Request.ServerVariables("HTTPS").ToLower = "on" Then
         '      Dim str As String = app.Context.Request.Url.PathAndQuery.ToLower
         '      str = str.Replace("https://middleclasscrunch.com", "https://www.middleclasscrunch.com")
         '      app.Context.RewritePath(str, True)
         '   End If
         'End If
      End If
   End Sub

   Public Sub ppEndRequest(ByVal s As Object, ByVal e As EventArgs)
   End Sub
End Class


