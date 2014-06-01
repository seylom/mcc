Imports MCC.Data
Imports Webdiyer.WebControls.Mvc
Public Class QuoteService
   Inherits CacheObject
   Implements IQuoteService

   Private _quoteRepo As IQuoteRepository
   Public Sub New()
      Me.New(New QuoteRepository())
   End Sub

   Public Sub New(ByVal quoteRepo As IQuoteRepository)
      _quoteRepo = quoteRepo
   End Sub

   Public Function GetQuoteCount() As Integer Implements IQuoteService.GetQuoteCount
      Dim key As String = "Quotes_QuoteCount_"

      If Cache(key) IsNot Nothing Then
         Return CInt(Cache(key))
      Else
         Dim it As Integer = _quoteRepo.GetQuotes.Count()
         CacheData(key, it)
         Return it
      End If
   End Function

   Public Function GetQuoteCountByAuthor(ByVal addedBy As String) As Integer Implements IQuoteService.GetQuoteCountByAuthor
      If Not String.IsNullOrEmpty(addedBy) Then
         Dim key As String = "Quotes_QuoteCount_" & addedBy & "_"

         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            Dim it As Integer = _quoteRepo.GetQuotes.Count(Function(p) p.AddedBy.ToLower = addedBy.ToLower)
            CacheData(key, it)
            Return it
         End If
      Else
         Return 0
      End If
   End Function


   Public Function GetQuoteCount(ByVal PublishedOnly As Boolean) As Integer Implements IQuoteService.GetQuoteCount
      If Not PublishedOnly Then
         Return GetQuoteCount()
      End If

      Dim key As String = "Quotes_QuoteCount_" & PublishedOnly.ToString

      If Cache(key) IsNot Nothing Then
         Return CInt(Cache(key))
      Else
         Dim it As Integer = _quoteRepo.GetQuotes.Count(Function(p) p.Approved = True)
         CacheData(key, it)
         Return it
      End If
   End Function

   Public Function GetQuotes() As List(Of Quote) Implements IQuoteService.GetQuotes
      Dim key As String = "Quotes_Quotes_"
      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), List(Of Quote))
      Else
         Dim li As List(Of Quote) = _quoteRepo.GetQuotes.ToList
         CacheData(key, li)
         Return li
      End If
   End Function

   Public Function GetQuotesByAuthor(ByVal addedBy As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedby") As PagedList(Of Quote) Implements IQuoteService.GetQuotesByAuthor
      If Not String.IsNullOrEmpty(addedBy) Then
         If String.IsNullOrEmpty(sortExp) Then
            sortExp = "QuoteId"
         End If
         Dim key As String = "Quotes_Quotes_" & addedBy & "_" & sortExp.Replace(" ", "") & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Return DirectCast(Cache(key), PagedList(Of Quote))
         Else
            Dim li As PagedList(Of Quote)
            li = _quoteRepo.GetQuotes.Where(Function(it) it.AddedBy = addedBy).SortBy(sortExp).ToPagedList(startrowindex, maximumrows)
            CacheData(key, li)
            Return li
         End If
      Else
         Return GetQuotes(startrowindex, maximumrows, sortExp)
      End If
   End Function

   Public Function GetQuotesByAuthor(ByVal publishedOnly As Boolean, ByVal addedBy As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedby") As PagedList(Of Quote) Implements IQuoteService.GetQuotesByAuthor

      If Not publishedOnly Then
         Return GetQuotesByAuthor(addedBy, startrowindex, maximumrows, sortExp)
      End If

      If Not String.IsNullOrEmpty(addedBy) Then
         If String.IsNullOrEmpty(sortExp) Then
            sortExp = "QuoteId"
         End If
         Dim key As String = "Quotes_Quotes_" & addedBy & "_" & publishedOnly.ToString & "_" & sortExp.Replace(" ", "") & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Dim li As PagedList(Of Quote) = DirectCast(Cache(key), PagedList(Of Quote))
            Return li
         Else
            Dim li As PagedList(Of Quote) = _quoteRepo.GetQuotes.Where(Function(it) it.AddedBy = addedBy AndAlso it.Approved = True).SortBy(sortExp).ToPagedList(startrowindex, maximumrows)
            CacheData(key, li)
            Return li
         End If
      Else
         Return GetQuotes(startrowindex, maximumrows, sortExp)
      End If
   End Function

   Public Function GetQuotes(ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "QuoteId") As PagedList(Of Quote) Implements IQuoteService.GetQuotes
      Dim key As String = "Quotes_Quotes_" & sortExp.Replace(" ", "") & startrowindex.ToString & "_" & "_" & maximumrows.ToString & "_"
      If String.IsNullOrEmpty(sortExp) Then
         sortExp = "QuoteId"
      End If
      If Cache(key) IsNot Nothing Then
         Dim li As PagedList(Of Quote) = DirectCast(Cache(key), PagedList(Of Quote))
         Return li
      Else
         Dim li As PagedList(Of Quote) = _quoteRepo.GetQuotes.SortBy(sortExp).ToPagedList(startrowindex, maximumrows)
         CacheData(key, li)
         Return li
      End If
   End Function

   Public Function GetQuotes(ByVal publishedOnly As Boolean, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "QuoteId") As PagedList(Of Quote) Implements IQuoteService.GetQuotes

      If publishedOnly Then
         If String.IsNullOrEmpty(sortExp) Then
            sortExp = "QuoteId"
         End If

         Dim key As String = "Quotes_Quotes_" & publishedOnly.ToString & "_" & sortExp.Replace(" ", "") & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Dim li As PagedList(Of Quote) = DirectCast(Cache(key), PagedList(Of Quote))
            Return li
         Else
            Dim li As PagedList(Of Quote) = _quoteRepo.GetQuotes.Where(Function(p) p.Approved = True).SortBy(sortExp).ToPagedList(startrowindex, maximumrows)
            CacheData(key, li)
            Return li
         End If
      Else
         Return GetQuotes(startrowindex, maximumrows, sortExp)
      End If
   End Function


   '''' <summary>
   '''' Returns an Quote object with the specified ID
   '''' </summary>
   'Public Function GetLatestQuotes(ByVal pageSize As Integer) As List(Of Quote)
   '   Dim Quotes As New List(Of Quote)
   '   Dim key As String = "Quotes_Quotes_Latest_" + pageSize.ToString

   '   If CacheObject.Cache(key) IsNot Nothing Then
   '      Quotes = DirectCast(CacheObject.Cache(key), List(Of Quote))
   '   Else
   '      Quotes = GetQuotes(True, 0, pageSize, "ReleaseDate DESC")
   '   End If
   '   Return Quotes
   'End Function

   Public Function GetQuoteById(ByVal QuoteId As Integer) As Quote Implements IQuoteService.GetQuoteById
      Dim key As String = "Quotes_Quotes_" & QuoteId.ToString & "_"
      If Cache(key) IsNot Nothing Then
         Dim fb As Quote = DirectCast(Cache(key), Quote)
         Return fb
      Else
         Dim fb As Quote = _quoteRepo.GetQuotes.Where(Function(it) it.QuoteID = QuoteId).FirstOrDefault
         CacheData(key, fb)
         Return fb
      End If
   End Function

   ''' <summary>
   ''' Creates a new Quote
   ''' </summary>
   Public Function InsertQuote(ByVal qt As Quote) As Boolean Implements IQuoteService.InsertQuote
      Try
         If qt IsNot Nothing Then
            With qt
               .AddedDate = DateTime.Now
               .AddedBy = CacheObject.CurrentUserName
            End With
            _quoteRepo.InsertQuote(qt)
            CacheObject.PurgeCacheItems("Quotes_")
         End If
         Return True
      Catch ex As Exception
         Return False
      End Try
   End Function

   Public Function UpdateQuote(ByVal qt As Quote) As Boolean Implements IQuoteService.UpdateQuote
      Try
         If qt IsNot Nothing Then
            _quoteRepo.UpdateQuote(qt)
            CacheObject.PurgeCacheItems("Quotes_")
         End If
         Return True
      Catch ex As Exception
         Return False
      End Try
   End Function

   Public Sub DeleteQuote(ByVal QuoteId As Integer) Implements IQuoteService.DeleteQuote
      If QuoteId > 0 Then
         _quoteRepo.DeleteQuote(QuoteId)
         CacheObject.PurgeCacheItems("Quotes_")
      End If
   End Sub

   Public Sub DeleteQuotes(ByVal QuotesId() As Integer) Implements IQuoteService.DeleteQuotes
      If QuotesId IsNot Nothing Then
         _quoteRepo.DeleteQuotes(QuotesId)
         CacheObject.PurgeCacheItems("Quotes_")
      End If
   End Sub

   Public Sub ApproveQuote(ByVal QuoteId As Integer) Implements IQuoteService.ApproveQuote
      If QuoteId > 0 Then
         Dim q As Quote = _quoteRepo.GetQuotes.Where(Function(p) p.QuoteID = QuoteId).FirstOrDefault()
         If q IsNot Nothing Then
            q.Approved = True
            _quoteRepo.UpdateQuote(q)
         End If
         CacheObject.PurgeCacheItems("Quotes_")
      End If
   End Sub

   Protected Sub CacheData(ByVal key As String, ByVal data As Object)
      If data IsNot Nothing Then
         CacheObject.Cache.Insert(key, data, Nothing, DateTime.Now.AddSeconds(3600), TimeSpan.Zero)
      End If
   End Sub


   'Public Function GetQuoteIdList() As List(Of Integer)
   '   Dim li As List(Of Integer) = Nothing
   '   Dim key As String = "quotes_quotesids"

   '   If Cache(key) IsNot Nothing Then
   '      li = DirectCast(Cache(key), List(Of Integer))
   '   Else
   '      If mdc.quotes.Count() > 0 Then
   '         li = (From it As Quote In mdc.quotes Select it.QuoteID).ToList
   '      End If
   '      mdc.Dispose()
   '      CacheData(key, li)
   '   End If
   '   Return li

   'End Function


   'Public Function GetRandomQuote() As Quote
   '   Dim key As String = "quotes_quotesids"
   '   If Cache(key) IsNot Nothing Then
   '      Dim li As List(Of Integer) = DirectCast(Cache(key), List(Of Integer))
   '      Dim rd As New Random()
   '      Dim i As Integer = rd.Next(0, li.Count - 1)
   '      Return GetQuoteById(li(i))
   '   Else
   '      GetQuoteIdList()
   '      Return Nothing
   '   End If
   'End Function


End Class