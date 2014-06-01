Namespace MCC.Areas.Site.Controllers


   Public Class AuthorsController
      Inherits ApplicationController

      '
      ' GET: /Authors/

      Function Index() As ActionResult



         Return View()
      End Function

      Function ShowAuthor(ByVal username As String) As ActionResult
         Dim _viewdata As New ArticlesFormViewModel(0, 30, username)
         Return View(_viewdata)
      End Function
   End Class
End Namespace