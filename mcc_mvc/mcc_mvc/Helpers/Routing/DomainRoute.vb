Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Routing
Imports System.Web.Mvc
Imports System.Text.RegularExpressions

Namespace MvcDomainRouting.Code
   Public Class DomainRoute
      Inherits Route
      Private domainRegex As Regex
      Private pathRegex As Regex

      Private _Domain As String
      Public Property Domain() As String
         Get
            Return _Domain
         End Get
         Set(ByVal value As String)
            _Domain = value
         End Set
      End Property

      Public Sub New(ByVal domain__1 As String, ByVal url As String, ByVal defaults As RouteValueDictionary)
         MyBase.New(url, defaults, New MvcRouteHandler())
         Domain = domain__1
      End Sub

      'Public Sub New(ByVal domain__1 As String, ByVal url As String, ByVal defaults As RouteValueDictionary, ByVal routeHandler As IRouteHandler)
      '   MyBase.New(url, defaults, routeHandler)
      '   Domain = domain__1
      'End Sub

      Public Sub New(ByVal domain__1 As String, ByVal url As String, ByVal defaults As Object)
         MyBase.New(url, New RouteValueDictionary(defaults), New MvcRouteHandler())
         Domain = domain__1
      End Sub

      Public Sub New(ByVal domain__1 As String, ByVal url As String, ByVal defaults As Object, ByVal routeHandler As IRouteHandler)
         MyBase.New(url, New RouteValueDictionary(defaults), routeHandler)
         Domain = domain__1
      End Sub

      'Public Sub New(ByVal domain__1 As String, ByVal url As String, ByVal defaults As Object, ByVal routeHandler As IRouteHandler)
      '   MyBase.New(url, New RouteValueDictionary(defaults), routeHandler)
      '   Domain = domain__1
      'End Sub

      Public Sub New(ByVal domain__1 As String, ByVal url As String, ByVal defaults As Object, ByVal constraints As Object)
         MyBase.New(url, New RouteValueDictionary(defaults), New RouteValueDictionary(constraints), New MvcRouteHandler())
         Domain = domain__1
      End Sub

      Public Sub New(ByVal domain__1 As String, ByVal url As String, ByVal defaults As Object, ByVal constraints As Object, ByVal routeHandler As IRouteHandler)
         MyBase.New(url, New RouteValueDictionary(defaults), New RouteValueDictionary(constraints), routeHandler)
         Domain = domain__1
      End Sub

      Public Overloads Overrides Function GetRouteData(ByVal httpContext As HttpContextBase) As RouteData
         ' Build regex
         domainRegex = CreateRegex(Domain)
         pathRegex = CreateRegex(Url)

         ' Request information
         Dim requestDomain As String = httpContext.Request.Headers("host")
         If Not String.IsNullOrEmpty(requestDomain) Then
            If requestDomain.IndexOf(":") > 0 Then
               requestDomain = requestDomain.Substring(0, requestDomain.IndexOf(":"))
            End If
         Else
            requestDomain = httpContext.Request.Url.Host
         End If
         Dim requestPath As String = httpContext.Request.AppRelativeCurrentExecutionFilePath.Substring(2) + httpContext.Request.PathInfo

         ' Match domain and route
         Dim domainMatch As Match = domainRegex.Match(requestDomain)
         Dim pathMatch As Match = pathRegex.Match(requestPath)

         ' Route data
         Dim data As RouteData = Nothing
         If domainMatch.Success AndAlso pathMatch.Success Then
            data = New RouteData(Me, RouteHandler)

            ' Add defaults first
            If Defaults IsNot Nothing Then
               For Each item As KeyValuePair(Of String, Object) In Defaults
                  data.Values(item.Key) = item.Value
               Next
            End If

            ' Iterate matching domain groups
            For i As Integer = 1 To domainMatch.Groups.Count - 1
               Dim group As Group = domainMatch.Groups(i)
               If group.Success Then
                  Dim key As String = domainRegex.GroupNameFromNumber(i)

                  If Not String.IsNullOrEmpty(key) AndAlso Not Char.IsNumber(key, 0) Then
                     If Not String.IsNullOrEmpty(group.Value) Then
                        data.Values(key) = group.Value
                     End If
                  End If
               End If
            Next

            ' Iterate matching path groups
            For i As Integer = 1 To pathMatch.Groups.Count - 1
               Dim group As Group = pathMatch.Groups(i)
               If group.Success Then
                  Dim key As String = pathRegex.GroupNameFromNumber(i)

                  If Not String.IsNullOrEmpty(key) AndAlso Not Char.IsNumber(key, 0) Then
                     If Not String.IsNullOrEmpty(group.Value) Then
                        data.Values(key) = group.Value
                     End If
                  End If
               End If
            Next
         End If

         Return data
      End Function

      Public Overloads Overrides Function GetVirtualPath(ByVal requestContext As RequestContext, ByVal values As RouteValueDictionary) As VirtualPathData
         Return MyBase.GetVirtualPath(requestContext, RemoveDomainTokens(values))
      End Function

      Public Function GetDomainData(ByVal requestContext As RequestContext, ByVal values As RouteValueDictionary) As DomainData
         ' Build hostname
         Dim hostname As String = Domain
         For Each pair As KeyValuePair(Of String, Object) In values
            hostname = hostname.Replace("{" & pair.Key & "}", pair.Value.ToString())
         Next

         ' Return domain data
         Return New DomainData()
      End Function

      Private Function CreateRegex(ByVal source As String) As Regex
         ' Perform replacements
         source = source.Replace("/", "\/?")
         source = source.Replace(".", "\.?")
         source = source.Replace("-", "\-?")
         source = source.Replace("{", "(?<")
         source = source.Replace("}", ">([a-zA-Z0-9_]*))")

         Return New Regex("^" & source & "$")
      End Function

      Private Function RemoveDomainTokens(ByVal values As RouteValueDictionary) As RouteValueDictionary
         Dim tokenRegex As New Regex("({[a-zA-Z0-9_]*})*-?\.?\/?({[a-zA-Z0-9_]*})*-?\.?\/?({[a-zA-Z0-9_]*})*-?\.?\/?({[a-zA-Z0-9_]*})*-?\.?\/?({[a-zA-Z0-9_]*})*-?\.?\/?({[a-zA-Z0-9_]*})*-?\.?\/?({[a-zA-Z0-9_]*})*-?\.?\/?({[a-zA-Z0-9_]*})*-?\.?\/?({[a-zA-Z0-9_]*})*-?\.?\/?({[a-zA-Z0-9_]*})*-?\.?\/?({[a-zA-Z0-9_]*})*-?\.?\/?({[a-zA-Z0-9_]*})*-?\.?\/?")
         Dim tokenMatch As Match = tokenRegex.Match(Domain)
         For i As Integer = 0 To tokenMatch.Groups.Count - 1
            Dim group As Group = tokenMatch.Groups(i)
            If group.Success Then
               Dim key As String = group.Value.Replace("{", "").Replace("}", "")
               If values.ContainsKey(key) Then
                  values.Remove(key)
               End If
            End If
         Next

         Return values
      End Function
   End Class
End Namespace
