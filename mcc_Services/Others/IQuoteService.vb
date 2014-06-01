Imports MCC.Data
Imports Webdiyer.WebControls.Mvc

Public Interface IQuoteService
   Function GetQuoteCount() As Integer
   Function GetQuoteCountByAuthor(ByVal addedBy As String) As Integer
   Function GetQuoteCount(ByVal PublishedOnly As Boolean) As Integer
   Function GetQuotes() As List(Of Quote)
   Function GetQuotesByAuthor(ByVal addedBy As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedby") As PagedList(Of Quote)
   Function GetQuotesByAuthor(ByVal publishedOnly As Boolean, ByVal addedBy As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedby") As PagedList(Of Quote)
   Function GetQuotes(ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "QuoteId") As PagedList(Of Quote)
   Function GetQuotes(ByVal publishedOnly As Boolean, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "QuoteId") As PagedList(Of Quote)

   Function GetQuoteById(ByVal QuoteId As Integer) As Quote
   ''' <summary>
   ''' Creates a new Quote
   ''' </summary>
   Function InsertQuote(ByVal qt As Quote) As Boolean
   Function UpdateQuote(ByVal qt As Quote) As Boolean
   Sub DeleteQuote(ByVal QuoteId As Integer)
   Sub DeleteQuotes(ByVal QuotesId As Integer())
   Sub ApproveQuote(ByVal QuoteId As Integer)
End Interface
