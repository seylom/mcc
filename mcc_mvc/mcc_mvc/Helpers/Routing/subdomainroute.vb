Public Class SubdomainRoute
   Inherits RouteBase

   Public Overloads Overrides Function GetRouteData(ByVal httpContext As HttpContextBase) As RouteData

      Dim subRouteData As RouteData = Nothing

      Dim url = httpContext.Request.Headers("HOST")
      Dim index = url.IndexOf(".")
      If index < 0 Then
         Return Nothing
      End If

      Dim subDomain = url.Substring(0, index)

      Select Case subDomain.ToLowerInvariant()
         Case "ask"
            subRouteData = New RouteData(Me, New MvcRouteHandler())
            subRouteData.Values.Add("controller", "ask")
            subRouteData.Values.Add("action", "Index")
            'Case "video"
            '   subRouteData = New RouteData(Me, New MvcRouteHandler())
            '   subRouteData.Values.Add("controller", "Video")
            '   subRouteData.Values.Add("action", "Index")
      End Select

      Return subRouteData
   End Function

   Public Overrides Function GetVirtualPath(ByVal requestContext As RequestContext, ByVal values As RouteValueDictionary) As VirtualPathData
      'Implement your formating Url formating here
      Return Nothing
   End Function
End Class
