Imports MCC.Services
Imports MCC.Data

Namespace MCC.Areas.Admin.Controllers
   Public Class TicketAdminController
      Inherits AdminController

      Private _ticketService As ITicketService
      Private _ticketChangeService As ITicketChangeService


      Public Sub New()
         Me.New(New TicketService, New TicketChangeService)
      End Sub

      Public Sub New(ByVal ticketsrvr As ITicketService, ByVal ticketchangesrvr As ITicketChangeService)
         _ticketService = ticketsrvr
         _ticketChangeService = ticketchangesrvr
      End Sub


      Function Index(ByVal page? As Integer, ByVal size? As Integer) As ActionResult
         Dim _viewdata As New AdminTicketsViewModel(If(page, 0), If(size, 30), _ticketService)
         Return View(_viewdata)
      End Function


      Function ShowTicket(ByVal Id As Integer) As ActionResult
         If Id <= 0 Then
            Return RedirectToAction("Index")
         End If

         Dim tk As Ticket = _ticketService.GetTicketById(Id)
         If tk Is Nothing Then
            Return RedirectToAction("Index")
         End If

         Dim _viewdata As New ticketViewModel()
         tk.FillViewModel(_viewdata)
         Return View(_viewdata)
      End Function

      Public Function EditTicket(ByVal Id As Integer) As ActionResult

         If Id <= 0 Then
            Return RedirectToAction("Index")
         End If

         Dim tk As Ticket = _ticketService.GetTicketById(Id)
         If tk Is Nothing Then
            Return RedirectToAction("Index")
         End If

         tk.TicketID = Id
         Dim _viewdata As New ticketViewModel()
         tk.FillViewModel(_viewdata)
         Return View(_viewdata)

      End Function

      <AcceptVerbs(HttpVerbs.Post)> _
      Public Function EditTicket(ByVal tvm As ticketViewModel) As ActionResult

         If tvm Is Nothing Then
            Return RedirectToAction("Index")
         End If

         Dim tk As Ticket = _ticketService.GetTicketById(tvm.TicketID)
         If tk Is Nothing Then
            Return RedirectToAction("Index")
         End If

         tvm.FillDTO(tk)
         _ticketService.SaveTicket(tk)

         Return RedirectToAction("ShowTicket", New With {.id = tk.TicketID})
      End Function

      Function CreateTicket() As ActionResult
         Dim _vd As New ticketViewModel(_ticketService, _ticketChangeService)
         Return View(_vd)
      End Function

      <AcceptVerbs(HttpVerbs.Post)> _
      Function CreateTicket(ByVal tkv As ticketViewModel) As ActionResult
         If tkv Is Nothing Then
            Return RedirectToAction("Index")
         End If

         Dim tk As New Ticket
         tkv.FillDTO(tk)

         If Not _ticketService.SaveTicket(tk) Then
            TempData("ErrorMessage") = "unable to create the new ticket ... please check for required field and try again"
            Return View(tkv)
         End If

         Return RedirectToAction("ShowTicket", New With {.id = tk.TicketID})
      End Function

      <AcceptVerbs(HttpVerbs.Post)> _
      Function CreateTicketChange(ByVal Id As Integer, ByVal body As String)

         If Id <= 0 Then
            Return RedirectToAction("Index")
         End If

         If String.IsNullOrEmpty(body) Then
            TempData("ErrorMessage") = "body cannot be empty. Please enter your ticket change comment body"
            Return RedirectToAction("ShowTicket", New With {.id = Id})
         End If

         Dim tki As New TicketChange With {.Body = body, .TicketID = Id}
         _ticketChangeService.InsertTicketChange(tki)

         Return RedirectToAction("ShowTicket", New With {.Id = Id})
      End Function

      <AcceptVerbs(HttpVerbs.Post)> _
      Function DeleteTicketChange(ByVal id As Integer)
         If id > 0 Then
            _ticketChangeService.DeleteTicketChange(id)
         End If
         Return RedirectToAction("Index")
      End Function

      Function QueryTickets() As ActionResult


         Return View()
      End Function
   End Class
End Namespace