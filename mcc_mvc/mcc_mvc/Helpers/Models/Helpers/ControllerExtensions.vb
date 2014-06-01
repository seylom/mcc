Public Module ControllerExtensions
   Public Function IsAdmin() As Boolean
      Return HttpContext.Current.User.Identity.IsAuthenticated AndAlso HttpContext.Current.User.IsInRole("Administrators")
   End Function

   Public Function IsAuthenticated() As Boolean
      Return (HttpContext.Current.User.Identity.IsAuthenticated)
   End Function



   <System.Runtime.CompilerServices.Extension()> _
   Public Function ToSelectList(Of T)(ByVal enumerable As IEnumerable(Of T), ByVal text As Func(Of T, String), ByVal value As Func(Of T, String), ByVal defaultOption As String) As List(Of SelectListItem)
      Dim items = enumerable.[Select](Function(f) New SelectListItem With {.Text = text(f), .Value = value(f)}).ToList()
      items.Insert(0, New SelectListItem With {.Text = defaultOption, .Value = -1})
      Return items
   End Function

End Module
