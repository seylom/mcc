Public Interface IQuoteRepository
   Function GetQuotes() As IQueryable(Of Quote)
   Function InsertQuote(ByVal qt As Quote) As Integer
   Sub UpdateQuote(ByVal qt As Quote)
   Sub DeleteQuote(ByVal qId As Integer)
   Sub DeleteQuotes(ByVal qId() As Integer)
End Interface
