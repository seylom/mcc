Imports System
Imports System.Data
Imports System.Configuration
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports System.Collections.Generic
Imports System.Linq
Imports MCC.Dynamic
Imports MCC.SiteLayers
Imports MCC.Data

Namespace Quotes
   Public Class Quote
      Inherits mccObject

      Public Shared Function GetQuoteCount() As Integer
         Dim key As String = "Quotes_QuoteCount_"

         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            Dim mdc As New MCCDataContext()
            mdc.mcc_quotes.Count()
            Dim it As Integer = mdc.mcc_quotes.Count()
            CacheData(key, it)
            Return it
         End If
      End Function

      Public Shared Function GetQuoteCountByAuthor(ByVal addedBy As String) As Integer
         If Not String.IsNullOrEmpty(addedBy) Then
            Dim key As String = "Quotes_QuoteCount_" & addedBy & "_"

            If Cache(key) IsNot Nothing Then
               Return CInt(Cache(key))
            Else
               Dim mdc As New MCCDataContext()
               Dim it As Integer = mdc.mcc_quotes.Count(Function(p) p.AddedBy = addedBy)
               CacheData(key, it)
               Return it
            End If
         Else
            Return 0
         End If
      End Function

      'Public Shared Function GetQuoteCount(ByVal categoryId As Integer) As Integer
      '   If categoryId <= 0 Then
      '      Return GetQuoteCount()
      '   End If

      '   Dim key As String = "Quotes_QuoteCount_" & categoryId.ToString & "_"
      '   If Cache(key) IsNot Nothing Then
      '      Return CInt(Cache(key))
      '   Else
      '      Dim mdc As New MCCDataContext()
      '      Dim catAr As List(Of Integer) = (From ef As mcc_CategoriesQuote In mdc.mcc_CategoriesQuotes Where ef.CategoryID = categoryId Select ef.QuoteID).ToList
      '      Dim it As Integer = mdc.mcc_quotes.Count(Function(p) catAr.Contains(p.QuoteId))

      '      CacheData(key, it)
      '      Return it
      '   End If
      'End Function

      Public Shared Function GetQuoteCount(ByVal PublishedOnly As Boolean) As Integer
         If Not PublishedOnly Then
            Return GetQuoteCount()
         End If

         Dim key As String = "Quotes_QuoteCount_" & PublishedOnly.ToString

         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            Dim mdc As New MCCDataContext()
            Dim it As Integer = mdc.mcc_quotes.Count(Function(p) p.Approved = True)
            CacheData(key, it)
            Return it
         End If
      End Function

      Public Shared Function GetQuotes() As List(Of mcc_quote)
         Dim key As String = "Quotes_Quotes_"
         If Cache(key) IsNot Nothing Then
            Dim li As List(Of mcc_quote) = DirectCast(Cache(key), List(Of mcc_quote))
            Return li
         Else
            Dim mdc As New MCCDataContext()
            Dim li As List(Of mcc_quote) = mdc.mcc_quotes.ToList
            CacheData(key, li)
            Return li
         End If
      End Function

      'Public Shared Function GetQuoteCount(ByVal publishedOnly As Boolean, ByVal categoryID As Integer) As Integer
      '   If Not publishedOnly Then
      '      Return GetQuoteCount(categoryID)
      '   End If

      '   If categoryID <= 0 Then
      '      Return GetQuoteCount(publishedOnly)
      '   End If

      '   Dim key As String = "Quotes_QuoteCount_" + publishedOnly.ToString() + "_" + categoryID.ToString()
      '   If Cache(key) IsNot Nothing Then
      '      Return CInt(Cache(key))
      '   Else
      '      Dim mdc As New MCCDataContext()
      '      Dim catAr As List(Of Integer) = (From ef As mcc_CategoriesQuote In mdc.mcc_CategoriesQuotes Where ef.CategoryID = categoryID Select ef.QuoteID).ToList

      '      Dim it As Integer = mdc.mcc_quotes.Count(Function(p) catAr.Contains(p.QuoteId) AndAlso p.Approved = True)

      '      CacheData(key, it)
      '      Return it
      '   End If
      'End Function

      'Public Shared Function GetQuotes(ByVal categoryId As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "QuoteId") As List(Of mcc_quote)
      '   If categoryId > 0 Then
      '      Dim key As String = "Quotes_Quotes_" & categoryId.ToString & "_" & sortExp.Replace(" ", "") & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
      '      If String.IsNullOrEmpty(sortExp) Then
      '         sortExp = "QuoteId"
      '      End If
      '      If Cache(key) IsNot Nothing Then
      '         Dim li As List(Of mcc_quote) = DirectCast(Cache(key), List(Of mcc_quote))
      '         Return li
      '      Else
      '         Dim mdc As New MCCDataContext()
      '         Dim li As List(Of mcc_quote)

      '         Dim catAr As List(Of Integer) = (From ef As mcc_CategoriesQuote In mdc.mcc_CategoriesQuotes Where ef.CategoryID = categoryId Select ef.QuoteID).ToList

      '         li = (From it As mcc_quote In mdc.mcc_quotes Where catAr.Contains(it.QuoteId)).SortBy(sortExp).Skip(startrowindex).Take(maximumrows).ToList
      '         CacheData(key, li)
      '         Return li
      '      End If
      '   Else
      '      Return GetQuotes(startrowindex, maximumrows, sortExp)
      '   End If
      'End Function

      'Public Shared Function GetQuotes(ByVal publishedOnly As Boolean, ByVal categoryId As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "QuoteId") As List(Of mcc_quote)
      '   If Not publishedOnly Then
      '      Return GetQuotes(categoryId, startrowindex, maximumrows, sortExp)
      '   End If

      '   If categoryId > 0 Then
      '      If String.IsNullOrEmpty(sortExp) Then
      '         sortExp = "QuoteId"
      '      End If
      '      Dim key As String = "Quotes_Quotes_" & categoryId.ToString & "_" & sortExp.Replace(" ", "") & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
      '      If Cache(key) IsNot Nothing Then
      '         Dim li As List(Of mcc_quote) = DirectCast(Cache(key), List(Of mcc_quote))
      '         Return li
      '      Else
      '         Dim mdc As New MCCDataContext()
      '         Dim li As List(Of mcc_quote)

      '         Dim catAr As List(Of Integer) = (From ef As mcc_CategoriesQuote In mdc.mcc_CategoriesQuotes Where ef.CategoryID = categoryId Select ef.QuoteID).ToList
      '         li = (From it As mcc_quote In mdc.mcc_quotes Where catAr.Contains(it.QuoteId) AndAlso it.Approved = True).SortBy(sortExp).Skip(startrowindex).Take(maximumrows).ToList
      '         CacheData(key, li)
      '         Return li
      '      End If
      '   Else
      '      Return GetQuotes(publishedOnly, startrowindex, maximumrows, sortExp)
      '   End If
      'End Function

      Public Shared Function GetQuotesByAuthor(ByVal addedBy As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedby") As List(Of mcc_quote)
         If Not String.IsNullOrEmpty(addedBy) Then
            If String.IsNullOrEmpty(sortExp) Then
               sortExp = "QuoteId"
            End If
            Dim key As String = "Quotes_Quotes_" & addedBy & "_" & sortExp.Replace(" ", "") & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
            If Cache(key) IsNot Nothing Then
               Dim li As List(Of mcc_quote) = DirectCast(Cache(key), List(Of mcc_quote))
               Return li
            Else
               Dim mdc As New MCCDataContext()
               Dim li As List(Of mcc_quote)
               li = (From it As mcc_quote In mdc.mcc_quotes Where it.AddedBy = addedBy).SortBy(sortExp).Skip(startrowindex).Take(maximumrows).ToList
               CacheData(key, li)
               Return li
            End If
         Else
            Return GetQuotes(startrowindex, maximumrows, sortExp)
         End If
      End Function

      Public Shared Function GetQuotesByAuthor(ByVal publishedOnly As Boolean, ByVal addedBy As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedby") As List(Of mcc_quote)

         If Not publishedOnly Then
            Return GetQuotesByAuthor(addedBy, startrowindex, maximumrows, sortExp)
         End If

         If Not String.IsNullOrEmpty(addedBy) Then
            If String.IsNullOrEmpty(sortExp) Then
               sortExp = "QuoteId"
            End If
            Dim key As String = "Quotes_Quotes_" & addedBy & "_" & publishedOnly.ToString & "_" & sortExp.Replace(" ", "") & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
            If Cache(key) IsNot Nothing Then
               Dim li As List(Of mcc_quote) = DirectCast(Cache(key), List(Of mcc_quote))
               Return li
            Else
               Dim mdc As New MCCDataContext()
               Dim li As List(Of mcc_quote)
               li = (From it As mcc_quote In mdc.mcc_quotes Where it.AddedBy = addedBy AndAlso it.Approved = True).SortBy(sortExp).Skip(startrowindex).Take(maximumrows).ToList
               CacheData(key, li)
               Return li
            End If
         Else
            Return GetQuotes(startrowindex, maximumrows, sortExp)
         End If
      End Function

      Public Shared Function GetQuotes(ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "QuoteId") As List(Of mcc_quote)
         Dim key As String = "Quotes_Quotes_" & sortExp.Replace(" ", "") & startrowindex.ToString & "_" & "_" & maximumrows.ToString & "_"
         If String.IsNullOrEmpty(sortExp) Then
            sortExp = "QuoteId"
         End If
         If Cache(key) IsNot Nothing Then
            Dim li As List(Of mcc_quote) = DirectCast(Cache(key), List(Of mcc_quote))
            Return li
         Else
            Dim mdc As New MCCDataContext()

            Dim li As New List(Of mcc_quote)
            Dim i As Integer = mdc.mcc_quotes.Count()
            If i > 0 Then
               li = mdc.mcc_quotes.SortBy(sortExp).Skip(startrowindex).Take(maximumrows).ToList
               CacheData(key, li)
            End If
            Return li
         End If
      End Function

      Public Shared Function GetQuotes(ByVal publishedOnly As Boolean, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "QuoteId") As List(Of mcc_quote)

         If publishedOnly Then
            If String.IsNullOrEmpty(sortExp) Then
               sortExp = "QuoteId"
            End If
            Dim key As String = "Quotes_Quotes_" & publishedOnly.ToString & "_" & sortExp.Replace(" ", "") & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
            If Cache(key) IsNot Nothing Then
               Dim li As List(Of mcc_quote) = DirectCast(Cache(key), List(Of mcc_quote))
               Return li
            Else
               Dim mdc As New MCCDataContext()

               Dim li As New List(Of mcc_quote)
               Dim i As Integer = mdc.mcc_quotes.Count()
               If i > 0 Then
                  li = (From a As mcc_quote In mdc.mcc_quotes Where a.Approved = True).SortBy(sortExp).Skip(startrowindex).Take(maximumrows).ToList
                  CacheData(key, li)
               End If
               Return li
            End If
         Else
            Return GetQuotes(startrowindex, maximumrows, sortExp)
         End If
      End Function


      ''' <summary>
      ''' Returns an Quote object with the specified ID
      ''' </summary>
      Public Shared Function GetLatestQuotes(ByVal pageSize As Integer) As List(Of mcc_quote)

         Dim Quotes As New List(Of mcc_quote)
         Dim key As String = "Quotes_Quotes_Latest_" + pageSize.ToString

         If mccObject.Cache(key) IsNot Nothing Then
            Quotes = DirectCast(mccObject.Cache(key), List(Of mcc_quote))
         Else
            Quotes = GetQuotes(True, 0, pageSize, "ReleaseDate DESC")
         End If
         Return Quotes
      End Function

      Public Shared Function GetQuoteById(ByVal QuoteId As Integer) As mcc_quote
         Dim key As String = "Quotes_Quotes_" & QuoteId.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Dim fb As mcc_quote = DirectCast(Cache(key), mcc_quote)
            Return fb
         Else
            Dim mdc As New MCCDataContext()
            Dim fb As mcc_quote
            If mdc.mcc_quotes.Count(Function(p) p.QuoteId = QuoteId) > 0 Then
               fb = (From it As mcc_quote In mdc.mcc_quotes Where it.QuoteId = QuoteId).FirstOrDefault
               CacheData(key, fb)
               Return fb
            Else
               Return Nothing
            End If
         End If
      End Function

      ''' <summary>
      ''' Creates a new Quote
      ''' </summary>
      Public Shared Sub InsertQuote(ByVal body As String, ByVal author As String, ByVal role As String)

         Dim mdc As MCCDataContext = New MCCDataContext
         Dim wrd As New mcc_quote

         body = mccObject.ConvertNullToEmptyString(body)
         author = mccObject.ConvertNullToEmptyString(author)
         role = mccObject.ConvertNullToEmptyString(role)

         With wrd
            .AddedDate = DateTime.Now
            .AddedBy = mccObject.CurrentUserName
            .Body = body
            .Author = author
            .Approved = False
            .Role = role
         End With

         mdc.mcc_quotes.InsertOnSubmit(wrd)
         mdc.SubmitChanges()
         mccObject.PurgeCacheItems("Quotes_Quotes_")
         mccObject.PurgeCacheItems("Quotes_Quotes_Latest_")
         mccObject.PurgeCacheItems("Quotes_Quote")

         mccObject.PurgeCacheItems("fQuotes_fQuotes")
         mccObject.PurgeCacheItems("fQuotes_fQuotecount_")

      End Sub


      Public Shared Sub UpdateQuote(ByVal quoteId As Integer, ByVal body As String, ByVal author As String, ByVal role As String)

         Dim mdc As MCCDataContext = New MCCDataContext
         Dim wrd As mcc_quote = (From t In mdc.mcc_quotes _
                                     Where t.QuoteId = quoteId _
                                     Select t).Single()
         If wrd IsNot Nothing Then

            body = mccObject.ConvertNullToEmptyString(body)
            author = mccObject.ConvertNullToEmptyString(author)
            role = mccObject.ConvertNullToEmptyString(role)

            With wrd
               .Body = body
               .Author = author
               .Approved = False
               .Role = role
               .AddedDate = DateTime.Now
               .AddedBy = mccObject.CurrentUserName
            End With

            mdc.SubmitChanges()

            mccObject.PurgeCacheItems("Quotes_QuoteCount")
            mccObject.PurgeCacheItems("Quotes_Quote_" + quoteId.ToString())
            mccObject.PurgeCacheItems("Quotes_Quotes_Latest_")
            mccObject.PurgeCacheItems("Quotes_Quotes")
            mccObject.PurgeCacheItems("Quotes_Specified_Categories_" + quoteId.ToString)

            mccObject.PurgeCacheItems("fQuotes_fQuotes")

         End If
      End Sub

      Public Shared Sub DeleteQuote(ByVal QuoteId As Integer)
         Dim mdc As MCCDataContext = New MCCDataContext
         If mdc.mcc_quotes.Count(Function(p) p.QuoteId = QuoteId) > 0 Then
            Dim wrd As mcc_quote = (From t In mdc.mcc_quotes _
                                        Where t.QuoteId = QuoteId _
                                        Select t).Single()
            If wrd IsNot Nothing Then
               mdc.mcc_quotes.DeleteOnSubmit(wrd)
               mdc.SubmitChanges()
               mccObject.PurgeCacheItems("Quotes_Quotes_")

               Dim tb As New MCCEvents.MCCEvents.RecordDeletedEvent("Quote", QuoteId, Nothing)
               tb.Raise()

               mccObject.PurgeCacheItems("Quotes_Quote")
               mccObject.PurgeCacheItems("Quotes_Quotes_")
               mccObject.PurgeCacheItems("Quotes_Quotes_Latest_")
               mccObject.PurgeCacheItems("Quotes_Specified_Categories_" + QuoteId.ToString)

               mccObject.PurgeCacheItems("fQuotes_fQuotes")
               mccObject.PurgeCacheItems("fQuotes_fQuotecount_")
            End If
         End If
      End Sub

      Public Shared Sub DeleteQuotes(ByVal QuotesId() As Integer)
         Dim mdc As MCCDataContext = New MCCDataContext
         If mdc.mcc_quotes.Count(Function(p) QuotesId.Contains(p.QuoteId)) > 0 Then
            Dim wrd As List(Of mcc_quote) = (From t In mdc.mcc_quotes _
                                        Where QuotesId.Contains(t.QuoteId)).ToList
            If wrd IsNot Nothing AndAlso wrd.Count > 0 Then


               mdc.mcc_quotes.DeleteAllOnSubmit(wrd)
               mdc.SubmitChanges()

               mccObject.PurgeCacheItems("Quotes_")
            End If
         End If
      End Sub

      Public Shared Sub ApproveQuote(ByVal QuoteId As Integer)
         Dim mdc As MCCDataContext = New MCCDataContext
         If mdc.mcc_quotes.Count(Function(p) p.QuoteId = QuoteId) > 0 Then
            Dim wrd As mcc_quote = (From t In mdc.mcc_quotes _
                                        Where t.QuoteId = QuoteId _
                                        Select t).Single()
            wrd.Approved = True
            mdc.SubmitChanges()
            mccObject.PurgeCacheItems("Quotes_QuoteCount")
            mccObject.PurgeCacheItems("Quotes_Quote_" + QuoteId.ToString())
            mccObject.PurgeCacheItems("Quotes_Quotes")
            mccObject.PurgeCacheItems("Quotes_Quotes_Latest_")

            mccObject.PurgeCacheItems("fQuotes_fQuotes")
            mccObject.PurgeCacheItems("fQuotes_fQuotecount_")
         End If
      End Sub

      Protected Shared Sub CacheData(ByVal key As String, ByVal data As Object)
         If data IsNot Nothing Then
            mccObject.Cache.Insert(key, data, Nothing, DateTime.Now.AddSeconds(3600), TimeSpan.Zero)
         End If
      End Sub


      Public Shared Function GetQuoteIdList() As List(Of Integer)
         Dim li As List(Of Integer) = Nothing
         Dim key As String = "quotes_quotesids"

         If Cache(key) IsNot Nothing Then
            li = DirectCast(Cache(key), List(Of Integer))
         Else
            Using mdc As New MCCDataContext
               If mdc.mcc_quotes.Count() > 0 Then
                  li = (From it As mcc_quote In mdc.mcc_quotes Select it.QuoteId).ToList
               End If
               mdc.Dispose()
               CacheData(key, li)
            End Using
         End If
         Return li

      End Function


      Public Shared Function GetRandomQuote() As mcc_quote
         Dim key As String = "quotes_quotesids"
         If Cache(key) IsNot Nothing Then
            Dim li As List(Of Integer) = DirectCast(Cache(key), List(Of Integer))
            Dim rd As New Random()
            Dim i As Integer = rd.Next(0, li.Count - 1)
            Return GetQuoteById(li(i))
         Else
            GetQuoteIdList()
            Return Nothing
         End If
      End Function
   End Class
End Namespace

