Namespace MCC.Areas.Admin.Controllers


   Public Class PollAdminController
      Inherits AdminController

      '
      ' GET: /PollAdmin/

      Function Index() As ActionResult
         Dim _viewdata As New AdminPollsViewModel()
         Return View(_viewdata)
      End Function



      Function ShowPoll(ByVal Id As Integer) As ActionResult


         Return View()
      End Function


      Function EditPoll(ByVal Id As Integer) As ActionResult


         Return View()
      End Function

      <AcceptVerbs(HttpVerbs.Post)> _
      Function EditPoll()


         Return View()
      End Function
   End Class
End Namespace