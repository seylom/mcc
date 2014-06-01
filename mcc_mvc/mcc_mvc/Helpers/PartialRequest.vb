Public Class PartialRequest
   Private _RouteValues As RouteValueDictionary
   Public Property RouteValues() As RouteValueDictionary
      Get
         Return _RouteValues
      End Get
      Private Set(ByVal value As RouteValueDictionary)
         _RouteValues = value
      End Set
   End Property

   Public Sub New(ByVal routeValues__1 As Object)
      RouteValues = New RouteValueDictionary(routeValues__1)
   End Sub

   Public Sub Invoke(ByVal context As ControllerContext)
      Dim rd As New RouteData(context.RouteData.Route, context.RouteData.RouteHandler)
      For Each pair In RouteValues
         rd.Values.Add(pair.Key, pair.Value)
      Next
      Dim handler As IHttpHandler = New MvcHandler(New RequestContext(context.HttpContext, rd))
      handler.ProcessRequest(System.Web.HttpContext.Current)
   End Sub
End Class

Public Module PartialRequestsExtensions
   <System.Runtime.CompilerServices.Extension()> _
   Public Sub RenderPartialRequest(ByVal html As HtmlHelper, ByVal viewDataKey As String)
      Dim parRequest As PartialRequest = TryCast(html.ViewContext.ViewData.Eval(viewDataKey), PartialRequest)
      If parRequest IsNot Nothing Then
         parRequest.Invoke(html.ViewContext)
      End If
   End Sub
End Module
