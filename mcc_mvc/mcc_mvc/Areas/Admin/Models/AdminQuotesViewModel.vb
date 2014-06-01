Imports MCC.Data
Imports Webdiyer.WebControls.Mvc

Public Class AdminQuotesViewModel
   Inherits baseViewModel


   Private _quotes As PagedList(Of Quote)
   Public Property quotes() As PagedList(Of Quote)
      Get
         Return _quotes
      End Get
      Set(ByVal value As PagedList(Of Quote))
         _quotes = value
      End Set
   End Property


End Class
