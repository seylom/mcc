Imports MCC.Services

Namespace MCC.Areas.Admin.Controllers
   Public Class QuoteAdminController
      Inherits AdminController


      Private _quoteservice As IQuoteService
      Public Sub New()
         Me.New(New QuoteService)
      End Sub

      Public Sub New(ByVal quotesrvr As IQuoteService)
         _quoteservice = quotesrvr
      End Sub

      Function Index(ByVal page? As Integer, ByVal size? As Integer) As ActionResult
         Dim _viewdata As New AdminQuotesViewModel
         _viewdata.quotes = _quoteservice.GetQuotes(If(page, 0), If(size, 30))
         Return View(_viewdata)
      End Function


      Function AddQuote() As ActionResult
         Dim _viewdata As New AdminQuoteViewModel
         Return View(_viewdata)
      End Function


      'Function AddQuote(ByVal vm As AdminQuoteViewModel) As ActionResult
      '   Return RedirectToAction("Index")
      'End Function
   End Class
End Namespace