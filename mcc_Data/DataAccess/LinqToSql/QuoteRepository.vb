Public Class QuoteRepository
   Implements IQuoteRepository
   Private _mdc As MCCDataContext
   Public Sub New()
      Me.New(new MCCDataContext)
   End Sub

   Public Sub New(ByVal db As MCCDataContext)
      _mdc = db
   End Sub
   Public Function GetQuotes() As IQueryable(Of Quote) Implements IQuoteRepository.GetQuotes
      Dim q = From it As mcc_quote In _mdc.mcc_quotes _
              Select New Quote With {.QuoteID = it.QuoteId, _
                                    .AddedDate = it.AddedDate, _
                                    .AddedBy = it.AddedBy, _
                                    .Author = it.Author, _
                                    .Role = it.Role, _
                                    .Body = it.Body}
      Return q
   End Function

   Public Function InsertQuote(ByVal qt As Quote) As Integer Implements IQuoteRepository.InsertQuote
      If qt IsNot Nothing Then
         Dim q = _mdc.mcc_quotes.Where(Function(p) p.QuoteId = qt.QuoteID).First()
         If q IsNot Nothing Then
            With q
               .AddedDate = qt.AddedDate
               .AddedBy = qt.AddedBy
               .Author = qt.Author
               .Role = qt.Role
               .Approved = qt.approved
            End With

            _mdc.SubmitChanges()
         End If
      End If
   End Function

   Public Sub UpdateQuote(ByVal qt As Quote) Implements IQuoteRepository.UpdateQuote
      If qt IsNot Nothing Then
         Dim q = _mdc.mcc_quotes.Where(Function(p) p.QuoteId = qt.QuoteID).First()
         If q IsNot Nothing Then
            With q
               .Author = qt.Author
               .Role = qt.Role
               .Approved = qt.approved
            End With

            _mdc.SubmitChanges()
         End If
      End If
   End Sub

   Public Sub DeleteQuote(ByVal qId As Integer) Implements IQuoteRepository.DeleteQuote
      If qId > 0 Then
         Dim q = _mdc.mcc_quotes.Where(Function(p) p.QuoteId = qId).FirstOrDefault()
         If q IsNot Nothing Then
            _mdc.mcc_quotes.DeleteOnSubmit(q)
            _mdc.SubmitChanges()
         End If
      End If
   End Sub

   Public Sub DeleteQuotes(ByVal qId As Integer()) Implements IQuoteRepository.DeleteQuotes
      If qId IsNot Nothing AndAlso qId.Length > 0 Then
         Dim q = _mdc.mcc_quotes.Where(Function(p) qId.Contains(p.QuoteId))
         If q IsNot Nothing Then
            _mdc.mcc_quotes.DeleteAllOnSubmit(q)
            _mdc.SubmitChanges()
         End If
      End If
   End Sub
End Class
