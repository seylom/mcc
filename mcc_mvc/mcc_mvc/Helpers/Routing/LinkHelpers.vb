Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Routing
Imports System.Web.Mvc

Imports MCC.MvcDomainRouting.Code


Public Module LinkExtensions

   <System.Runtime.CompilerServices.Extension()> _
   Public Function ActionLink(ByVal htmlHelper As HtmlHelper, ByVal linkText As String, ByVal actionName As String, ByVal requireAbsoluteUrl As Boolean) As String
      Return htmlHelper.ActionLink(linkText, actionName, Nothing, New RouteValueDictionary(), New RouteValueDictionary(), requireAbsoluteUrl)
   End Function

   <System.Runtime.CompilerServices.Extension()> _
   Public Function ActionLink(ByVal htmlHelper As HtmlHelper, ByVal linkText As String, ByVal actionName As String, ByVal routeValues As Object, ByVal requireAbsoluteUrl As Boolean) As String
      Return htmlHelper.ActionLink(linkText, actionName, Nothing, New RouteValueDictionary(routeValues), New RouteValueDictionary(), requireAbsoluteUrl)
   End Function

   <System.Runtime.CompilerServices.Extension()> _
   Public Function ActionLink(ByVal htmlHelper As HtmlHelper, ByVal linkText As String, ByVal actionName As String, ByVal controllerName As String, ByVal requireAbsoluteUrl As Boolean) As String
      Return htmlHelper.ActionLink(linkText, actionName, controllerName, New RouteValueDictionary(), New RouteValueDictionary(), requireAbsoluteUrl)
   End Function

   <System.Runtime.CompilerServices.Extension()> _
   Public Function ActionLink(ByVal htmlHelper As HtmlHelper, ByVal linkText As String, ByVal actionName As String, ByVal routeValues As RouteValueDictionary, ByVal requireAbsoluteUrl As Boolean) As String
      Return htmlHelper.ActionLink(linkText, actionName, Nothing, routeValues, New RouteValueDictionary(), requireAbsoluteUrl)
   End Function

   <System.Runtime.CompilerServices.Extension()> _
   Public Function ActionLink(ByVal htmlHelper As HtmlHelper, ByVal linkText As String, ByVal actionName As String, ByVal routeValues As Object, ByVal htmlAttributes As Object, ByVal requireAbsoluteUrl As Boolean) As String
      Return htmlHelper.ActionLink(linkText, actionName, Nothing, New RouteValueDictionary(routeValues), New RouteValueDictionary(htmlAttributes), requireAbsoluteUrl)
   End Function

   <System.Runtime.CompilerServices.Extension()> _
   Public Function ActionLink(ByVal htmlHelper As HtmlHelper, ByVal linkText As String, ByVal actionName As String, ByVal routeValues As RouteValueDictionary, ByVal htmlAttributes As IDictionary(Of String, Object), ByVal requireAbsoluteUrl As Boolean) As String
      Return htmlHelper.ActionLink(linkText, actionName, Nothing, routeValues, htmlAttributes, requireAbsoluteUrl)
   End Function

   <System.Runtime.CompilerServices.Extension()> _
   Public Function ActionLink(ByVal htmlHelper As HtmlHelper, ByVal linkText As String, ByVal actionName As String, ByVal controllerName As String, ByVal routeValues As Object, ByVal htmlAttributes As Object, _
    ByVal requireAbsoluteUrl As Boolean) As String
      Return htmlHelper.ActionLink(linkText, actionName, controllerName, New RouteValueDictionary(routeValues), New RouteValueDictionary(htmlAttributes), requireAbsoluteUrl)
   End Function

   <System.Runtime.CompilerServices.Extension()> _
   Public Function ActionLink(ByVal htmlHelper As HtmlHelper, ByVal linkText As String, ByVal actionName As String, ByVal controllerName As String, ByVal routeValues As RouteValueDictionary, ByVal htmlAttributes As IDictionary(Of String, Object), _
    ByVal requireAbsoluteUrl As Boolean) As String
      If requireAbsoluteUrl Then
         Dim currentContext As HttpContextBase = New HttpContextWrapper(HttpContext.Current)
         Dim routeData As RouteData = RouteTable.Routes.GetRouteData(currentContext)

         routeData.Values("controller") = controllerName
         routeData.Values("action") = actionName

         Dim domainRoute As DomainRoute = TryCast(routeData.Route, DomainRoute)
         If domainRoute IsNot Nothing Then
            Dim domainData As DomainData = domainRoute.GetDomainData(New RequestContext(currentContext, routeData), routeData.Values)
                Return htmlHelper.ActionLink(linkText, actionName, controllerName, domainData.Protocol, domainData.HostName, domainData.Fragment, _
                 routeData.Values, Nothing).ToString
         End If
      End If
        Return htmlHelper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes).ToString
   End Function
End Module
