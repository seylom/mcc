Public Class Route301GlobalHandler
   Implements IRouteHandler

   Public Function GetHttpHandler(ByVal requestContext As RequestContext) As IHttpHandler Implements System.Web.Routing.IRouteHandler.GetHttpHandler
      Return New The301GlobalHandler(requestContext)
   End Function

End Class

Public Module Route301Global
   Public Sub ReAssignHandler(ByVal routes As RouteCollection)
      Using routes.GetReadLock()
         AssignRoute301GlobalHandler(routes)
      End Using
   End Sub

   Private Sub AssignRoute301GlobalHandler(ByVal routes As IEnumerable(Of RouteBase))
      For Each routeBase In routes
         AssignRoute301GlobalHandler(routeBase)
      Next
   End Sub

   Private Sub AssignRoute301GlobalHandler(ByVal routeBase As RouteBase)
      Dim route = TryCast(routeBase, Route)
      If route Is Nothing Then
         Exit Sub
      End If
      If route.RouteHandler.GetType IsNot GetType(Route301GlobalHandler) Then
         route.RouteHandler = New Route301GlobalHandler()
      End If
   End Sub
End Module
