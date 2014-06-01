Imports MCC.Data
Imports Webdiyer.WebControls.Mvc
Public Class AdviceService
   Inherits CacheObject
   Implements IAdviceService



   Private _AdviceRepo As IAdviceRepository
   Private _CommentRepo As IAdviceCommentRepository
   Private _AdviceCategoryRepo As IAdviceCategoryRepository

   Public Sub New()
      Me.New(New AdviceRepository, New AdviceCategoryRepository, New AdviceCommentRepository)
   End Sub

   Public Sub New(ByVal adviceRepo As IAdviceRepository, ByVal adviceCategoryRepo As IAdviceCategoryRepository, _
                  ByVal commentRepo As IAdviceCommentRepository)
      _AdviceRepo = adviceRepo
      _CommentRepo = commentRepo
      _AdviceCategoryRepo = adviceCategoryRepo
   End Sub


   ''' <summary>
   ''' Get the average rating for an advice
   ''' </summary>
   ''' <param name="adviceId">The advice id.</param>
   ''' <returns></returns>
   Public Function AverageRating(ByVal adviceId As Integer) As Double Implements IAdviceService.AverageRating
      Dim key As String = "advices_average_rating_" & adviceId.ToString

      If Cache(key) IsNot Nothing Then
         Return CDbl(Cache(key))
      Else
         Dim val As Double = 0
         Dim wrd As Advice = _AdviceRepo.GetAdvices.Where(Function(p) p.AdviceID = adviceId).FirstOrDefault()
         If wrd IsNot Nothing Then
            If wrd.Votes >= 1 Then
               val = (CDbl(wrd.TotalRating) / CDbl(wrd.Votes))
            End If
         End If
         CacheData(key, val)
      End If
   End Function


   ''' <summary>
   ''' Gets the comments.
   ''' </summary>
   ''' <param name="adviceId">The advice id.</param>
   ''' <returns></returns>
   Public Function GetComments(ByVal adviceId As Integer) As IList(Of AdviceComment) Implements IAdviceService.GetComments
      Dim key As String = "advices_adviceComments_" & adviceId.ToString
      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), List(Of AdviceComment))
      Else
         Dim _comments As List(Of AdviceComment) = Nothing
         _comments = _CommentRepo.GetAdviceComments.Where(Function(c) c.AdviceID = adviceId).ToList
         CacheData(key, _comments)
         Return _comments
      End If
   End Function

   Public Function CommentsCount(ByVal AdviceId As Integer) As Integer Implements IAdviceService.CommentsCount

      'Dim key As String = ""
      'Dim mdc As New MCCDataContext
      'Return mdc.mcc_Comments.Count(Function(p) p.AdviceID = AdviceId)

      'Dim key As String = String.Format("advices_commentCount_{0}", AdviceId)

      Return _CommentRepo.GetAdviceComments.WithAdviceID(AdviceId).Count
   End Function

   Public Function Published(ByVal adviceId As Integer) As Boolean Implements IAdviceService.Published
      Dim key As String = "advice_advice_ispublished_" & adviceId.ToString
      If Cache(key) IsNot Nothing Then
         Return CBool(Cache(key))
      Else
         Dim bApproved As Boolean = _AdviceRepo.GetAdvices.Where(Function(p) p.AdviceID = adviceId And p.Approved = True).Select(Function(p) p.Approved = True).FirstOrDefault()
         CacheData(key, bApproved)
         Return bApproved
      End If
   End Function


   Public Function GetAdviceCount() As Integer Implements IAdviceService.GetAdviceCount
      Dim key As String = "advices_adviceCount_"

      If Cache(key) IsNot Nothing Then
         Return CInt(Cache(key))
      Else
         Dim it As Integer = _AdviceRepo.GetAdvices.Count()
         CacheData(key, it)
         Return it
      End If
   End Function

   Public Function GetAdviceCount(ByVal PublishedOnly As Boolean) As Integer Implements IAdviceService.GetAdviceCount
      If Not PublishedOnly Then
         Return GetAdviceCount()
      End If

      Dim key As String = "advices_adviceCount_" & PublishedOnly.ToString

      If Cache(key) IsNot Nothing Then
         Return CInt(Cache(key))
      Else
         Dim it As Integer = _AdviceRepo.GetAdvices.Published.Count
         CacheData(key, it)
         Return it
      End If
   End Function


   Public Function GetAdviceCountByAuthor(ByVal publishedOnly As Boolean, ByVal addedBy As String) As Integer Implements IAdviceService.GetAdviceCountByAuthor

      If Not publishedOnly Then
         GetAdviceCountByAuthor(addedBy)
      End If

      If Not String.IsNullOrEmpty(addedBy) Then
         Dim key As String = "advices_adviceCount_" & publishedOnly.ToString & "_" & addedBy & "_"

         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            Dim it As Integer = _AdviceRepo.GetAdvices.Published.WithAuthor(addedBy).Count
            CacheData(key, it)
            Return it
         End If
      Else
         Return 0
      End If
   End Function


   Public Function GetAdviceCountByAuthor(ByVal addedBy As String) As Integer Implements IAdviceService.GetAdviceCountByAuthor
      If String.IsNullOrEmpty(addedBy) Then
         Return 0
      End If
      Dim key As String = "advices_adviceCount_" & addedBy & "_"
      If Cache(key) IsNot Nothing Then
         Return CInt(Cache(key))
      Else
         Dim it As Integer = _AdviceRepo.GetAdvices.WithAuthor(addedBy).Count
         CacheData(key, it)
         Return it
      End If
   End Function

   Public Function GetAdviceCount(ByVal categoryId As Integer) As Integer Implements IAdviceService.GetAdviceCount
      If categoryId <= 0 Then
         Return GetAdviceCount()
      End If

      Dim key As String = "advices_adviceCount_" & categoryId.ToString & "_"
      If Cache(key) IsNot Nothing Then
         Return CInt(Cache(key))
      Else
         Dim it As Integer = _AdviceRepo.GetAdvicesInCategory(categoryId).Count()
         CacheData(key, it)
         Return it
      End If
   End Function

   Public Function GetAdviceCount(ByVal publishedOnly As Boolean, ByVal categoryID As Integer) As Integer Implements IAdviceService.GetAdviceCount
      If publishedOnly Then
         Return GetAdviceCount(categoryID)
      End If

      If categoryID <= 0 Then
         Return GetAdviceCount(publishedOnly)
      End If

      Dim key As String = "Advices_AdviceCount_" + publishedOnly.ToString() + "_" + categoryID.ToString()
      If Cache(key) IsNot Nothing Then
         Return CInt(Cache(key))
      Else
         Dim it As Integer = _AdviceRepo.GetAdvicesInCategory(categoryID).Published.Count
         CacheData(key, it)
         Return it
      End If
   End Function


   Public Function GetAdvices() As List(Of Advice) Implements IAdviceService.GetAdvices
      Dim key As String = "advices_advices_"
      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), List(Of Advice))
      Else
         Dim li As List(Of Advice) = _AdviceRepo.GetAdvices().ToList
         CacheData(key, li)
         Return li
      End If
   End Function


   Public Function GetAdvices(ByVal publishedOnly As Boolean, ByVal categoryId As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AddedDate") As PagedList(Of Advice) Implements IAdviceService.GetAdvices
      If Not publishedOnly Then
         Return GetAdvices(categoryId, startrowindex, maximumrows, sortExp)
      End If

      If categoryId > 0 Then
         If String.IsNullOrEmpty(sortExp) Then
            sortExp = "AddedDate DESC"
         End If
         Dim key As String = "advices_advices_" & categoryId.ToString & "_" & sortExp.Replace(" ", "") & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Return DirectCast(Cache(key), PagedList(Of Advice))
         Else
            Dim li As PagedList(Of Advice) = _AdviceRepo.GetAdvicesInCategory(categoryId).Published.SortBy(sortExp).ToPagedList(startrowindex, maximumrows)
            CacheData(key, li)
            Return li
         End If
      Else
         Return GetAdvices(publishedOnly, startrowindex, maximumrows, sortExp)
      End If
   End Function

   Public Function GetAdvices(ByVal categoryId As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AddedDate") As PagedList(Of Advice) Implements IAdviceService.GetAdvices
      If categoryId > 0 Then
         Dim key As String = "advices_advices_" & categoryId.ToString & "_" & sortExp.Replace(" ", "") & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
         If String.IsNullOrEmpty(sortExp) Then
            sortExp = "AddedDate DESC"
         End If
         If Cache(key) IsNot Nothing Then
                Dim li As PagedList(Of Advice) = DirectCast(Cache(key), PagedList(Of Advice))
            Return li
         Else
            Dim li As PagedList(Of Advice) = _AdviceRepo.GetAdvicesInCategory(categoryId).SortBy(sortExp).ToPagedList(startrowindex, maximumrows)
            CacheData(key, li)
            Return li
         End If
      Else
         Return GetAdvices(startrowindex, maximumrows, sortExp)
      End If
   End Function

   'Public Function GetAdvicesByCategoryName(ByVal categoryName As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "releaseDate") As List(Of Advice) Implements IAdviceService.GetAdvicesByCategoryName
   '   If Not String.IsNullOrEmpty(categoryName) Then
   '      Dim key As String = "advices_advices_" & categoryName & "_" & sortExp.Replace(" ", "") & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"

   '      If String.IsNullOrEmpty(sortExp) Then
   '         sortExp = "ReleaseDate DESC"
   '      End If
   '      If Cache(key) IsNot Nothing Then
   '         Dim li As List(Of Advice) = DirectCast(Cache(key), List(Of Advice))
   '         Return li
   '      Else
   '         Dim catId As Integer = _AdviceCategoryRepo.GetCategories.WithSlug(categoryName).Select(Function(q) q.CategoryID).FirstOrDefault
   '         Dim li As PagedList(Of Advice) = _AdviceRepo.GetAdvicesInCategory(catId).SortBy(sortExp).ToPagedList(startrowindex, maximumrows)
   '         CacheData(key, li)
   '         Return li
   '      End If
   '   Else
   '      Return GetAdvices(startrowindex, maximumrows, sortExp)
   '   End If
   'End Function


   Public Function GetAdvicesByAuthor(ByVal addedBy As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedby") As PagedList(Of Advice) Implements IAdviceService.GetAdvicesByAuthor
      If Not String.IsNullOrEmpty(addedBy) Then
         If String.IsNullOrEmpty(sortExp) Then
            sortExp = "AddedDate DESC"
         End If
         Dim key As String = "advices_advices_" & addedBy & "_" & sortExp.Replace(" ", "") & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
         If Cache(key) IsNot Nothing Then
                Dim li As PagedList(Of Advice) = DirectCast(Cache(key), PagedList(Of Advice))
            Return li
         Else

            Dim li As PagedList(Of Advice) = _AdviceRepo.GetAdvices.WithAuthor(addedBy).SortBy(sortExp).ToPagedList(startrowindex, maximumrows)
            CacheData(key, li)
            Return li
         End If
      Else
         Return GetAdvices(startrowindex, maximumrows, sortExp)
      End If
   End Function

   Public Function GetAdvicesByAuthor(ByVal publishedOnly As Boolean, ByVal addedBy As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedby") As PagedList(Of Advice) Implements IAdviceService.GetAdvicesByAuthor

      If Not publishedOnly Then
         Return GetAdvicesByAuthor(addedBy, startrowindex, maximumrows, sortExp)
      End If

      If Not String.IsNullOrEmpty(addedBy) Then
         If String.IsNullOrEmpty(sortExp) Then
            sortExp = "ReleaseDate DESC"
         End If
         Dim key As String = "advices_advices_" & addedBy & "_" & publishedOnly.ToString & "_" & sortExp.Replace(" ", "") & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Return DirectCast(Cache(key), PagedList(Of Advice))
         Else
            Dim li As PagedList(Of Advice) = _AdviceRepo.GetAdvices.Published.WithAuthor(addedBy).SortBy(sortExp).ToPagedList(startrowindex, maximumrows)
            CacheData(key, li)
            Return li
         End If
      Else
         Return GetAdvices(startrowindex, maximumrows, sortExp)
      End If
   End Function

   Public Function GetAdvices(ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AddedDate DESC") As PagedList(Of Advice) Implements IAdviceService.GetAdvices
      Dim key As String = "advices_advices_" & startrowindex.ToString & "_" & sortExp.Replace(" ", "") & "_" & maximumrows.ToString & "_"
      If String.IsNullOrEmpty(sortExp) Then
         sortExp = "ReleaseDate DESC"
      End If
      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), PagedList(Of Advice))
      Else
         Dim li As PagedList(Of Advice) = _AdviceRepo.GetAdvices.SortBy(sortExp).ToPagedList(startrowindex, maximumrows)
         CacheData(key, li)
         Return li
      End If
   End Function

   Public Function GetAdvices(ByVal publishedOnly As Boolean, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AddedDate DESC") As PagedList(Of Advice) Implements IAdviceService.GetAdvices
      If publishedOnly Then
         If String.IsNullOrEmpty(sortExp) Then
            sortExp = "ReleaseDate DESC"
         End If
         Dim key As String = "advices_advices_" & publishedOnly.ToString & "_" & sortExp.Replace(" ", "") & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
         If Cache(key) IsNot Nothing Then
                Dim li As PagedList(Of Advice) = DirectCast(Cache(key), PagedList(Of Advice))
            Return li
         Else
            Dim li As PagedList(Of Advice) = _AdviceRepo.GetAdvices.Published.SortBy(sortExp).ToPagedList(startrowindex, maximumrows)
            CacheData(key, li)
            Return li
         End If
      Else
         Return GetAdvices(startrowindex, maximumrows, sortExp)
      End If
   End Function


   ''' <summary>
   ''' Returns an Advice object with the specified ID
   ''' </summary>
   Public Function GetLatestAdvices(ByVal pageSize As Integer) As List(Of Advice) Implements IAdviceService.GetLatestAdvices
      Dim Advices As New List(Of Advice)
      Dim key As String = "Advices_Advices_Latest_" + pageSize.ToString

      If CacheObject.Cache(key) IsNot Nothing Then
         Advices = DirectCast(CacheObject.Cache(key), List(Of Advice))
      Else
         Advices = _AdviceRepo.GetAdvices.Published.SortBy("AddedDate DESC").ToPagedList(0, pageSize)
         CacheData(key, Advices)
      End If
      Return Advices
   End Function

   Public Function GetAdviceById(ByVal AdviceId As Integer) As Advice Implements IAdviceService.GetAdviceById
      Dim key As String = "advices_advices_" & AdviceId.ToString & "_"
      If Cache(key) IsNot Nothing Then
         Dim fb As Advice = DirectCast(Cache(key), Advice)
         Return fb
      Else
         Dim fb As Advice = _AdviceRepo.GetAdvices.WithAdviceID(AdviceId).FirstOrDefault()
         CacheData(key, fb)
         Return fb
      End If
   End Function

   Public Function GetAdviceBySlug(ByVal slug As String) As Advice Implements IAdviceService.GetAdviceBySlug
      Dim key As String = "advices_advices_" & slug & "_"
      If Cache(key) IsNot Nothing Then
         Dim fb As Advice = DirectCast(Cache(key), Advice)
         Return fb
      Else

         Dim fb As Advice = _AdviceRepo.GetAdvices.WithSlug(slug).FirstOrDefault()

         CacheData(key, fb)
         Return fb
      End If
   End Function

   '''' <summary>
   '''' Creates a new Advice
   '''' </summary>
   'Public Sub InsertAdvice(ByVal wrd As Advice) Implements IAdviceService.InsertAdvice


   '   ' on an inserted advice the approved flagged will always be false.
   '   wrd.Approved = False

   '   With wrd
   '      .AddedDate = DateTime.Now
   '      .AddedBy = CacheObject.CurrentUserName
   '      .Slug = DataHelper.GetSlugFromString(wrd.Title)
   '   End With

   '   Dim arId As Integer = _AdviceRepo.InsertAdvice(wrd)


   '   CacheObject.PurgeCacheItems("Advices_")

   'End Sub


   'Public Sub UpdateAdvice(ByVal _art As Advice) Implements IAdviceService.UpdateAdvice

   '   Dim wrd As Advice = _AdviceRepo.GetAdvices.WithAdviceID(_art.AdviceID).FirstOrDefault
   '   If wrd IsNot Nothing Then

   '      _art.Title = CacheObject.ConvertNullToEmptyString(_art.Title)
   '      _art.Abstract = CacheObject.ConvertNullToEmptyString(_art.Abstract)
   '      _art.Body = CacheObject.ConvertNullToEmptyString(_art.Body)
   '      _art.Tags = CacheObject.ConvertNullToEmptyString(_art.Tags)
   '      _AdviceRepo.UpdateAdvice(wrd)

   '      CacheObject.PurgeCacheItems("Advices_")
   '   End If
   'End Sub

   Public Function SaveAdvice(ByVal adv As Data.Advice) As Boolean Implements IAdviceService.SaveAdvice
      Try
         If adv.AdviceID > 0 Then
            adv.Slug = adv.Title.ToSlug()
            adv.Body = ConvertNullToEmptyString(adv.Body)
            _AdviceRepo.UpdateAdvice(adv)
         Else
            With adv
               .Body = ConvertNullToEmptyString(.Body)
               .AddedDate = DateTime.Now
               .AddedBy = CurrentUserName
               .Slug = adv.Title.ToSlug
            End With
            _AdviceRepo.InsertAdvice(adv)
         End If

         CacheObject.PurgeCacheItems("Advices_")
         Return True
      Catch ex As Exception
         Return False
      End Try
   End Function

   Public Sub DeleteAdvices(ByVal AdviceIds() As Integer) Implements IAdviceService.DeleteAdvices


      If AdviceIds IsNot Nothing And AdviceIds.Count > 0 Then

         _AdviceRepo.DeleteAdvices(AdviceIds)

         CacheObject.PurgeCacheItems("Advices_Advice")
         CacheObject.PurgeCacheItems("Advices_Advices_")
         CacheObject.PurgeCacheItems("Advices_Advices_Latest_")
         CacheObject.PurgeCacheItems("Advices_Specified_Categories_" + AdviceIds.ToString)

         CacheObject.PurgeCacheItems("fadvices_fadvices")
         CacheObject.PurgeCacheItems("fadvices_fadvicecount_")

      End If



      '   End If
      'End If
   End Sub

   Public Sub DeleteAdvice(ByVal AdviceId As Integer) Implements IAdviceService.DeleteAdvice
      If (AdviceId > 0) Then
         _AdviceRepo.DeleteAdvice(AdviceId)
         CacheObject.PurgeCacheItems("Advices_")
         CacheObject.PurgeCacheItems("fadvices_")
      End If
   End Sub

   Public Sub ApproveAdvice(ByVal adviceId As Integer) Implements IAdviceService.ApproveAdvice
      If adviceId > 0 Then
         Dim q As Advice = _AdviceRepo.GetAdvices.WithAdviceID(adviceId).FirstOrDefault
         If q IsNot Nothing Then
            q.Approved = True
            _AdviceRepo.UpdateAdvice(q)
            CacheObject.PurgeCacheItems("Advices_")
            CacheObject.PurgeCacheItems("fadvices_")
         End If
      End If
   End Sub

   Public Function RemoveAdviceFromCategory(ByVal AdviceID As Integer, Optional ByVal categoryId As Integer = 0) As Boolean Implements IAdviceService.RemoveAdviceFromCategory
      'Dim ret As Boolean = SiteProvider.Advices.RemoveAdviceFromCategory(AdviceID, categoryId)
      'Dim ret As Boolean = True

      If AdviceID > 0 Then
         If categoryId > 0 Then
            _AdviceCategoryRepo.RemoveAdvicesFromCategories(AdviceID, New Integer() {categoryId})
         Else
            _AdviceCategoryRepo.UnCategorizeAdvice(AdviceID)
         End If

         CacheObject.PurgeCacheItems("Advices_")
      End If
      Return True
   End Function

   'Public Function AddAdviceToCategory(ByVal AdviceId As Integer, ByVal CategoryId As Integer) As Integer
   '   'Dim ret As Integer = SiteProvider.Advices.AddAdviceToCategory(AdviceId, CategoryId)
   '   If AdviceId > 0 AndAlso CategoryId > 0 Then
   '      _AdviceRepo.AddAdviceToCategories(AdviceId, CategoryId)
   '      CacheObject.PurgeCacheItems("Advices_AdviceInCategories_")
   '   End If
   'End Function

   Public Function AddAdviceToCategories(ByVal AdviceId As Integer, ByVal CategoryIds() As Integer) As Integer Implements IAdviceService.AddAdviceToCategories
      If AdviceId > 0 AndAlso CategoryIds IsNot Nothing Then
         _AdviceCategoryRepo.CategorizeAdvice(AdviceId, CategoryIds)
         CacheObject.PurgeCacheItems("Advices_")
      End If
   End Function


   Public Sub IncrementViewCount(ByVal adviceId As Integer) Implements IAdviceService.IncrementViewCount
      If adviceId > 0 Then
         Dim q = _AdviceRepo.GetAdvices.WithAdviceID(adviceId).FirstOrDefault()
         If q IsNot Nothing Then
            q.ViewCount += 1
            _AdviceRepo.UpdateAdvice(q)
            'CacheObject.PurgeCacheItems("advices_advices_")
            'CacheObject.PurgeCacheItems("advices_adviceCount_")
         End If
      End If
   End Sub

   Public Sub RateAdvice(ByVal adviceId As Integer, ByVal rating As Integer) Implements IAdviceService.RateAdvice

      If adviceId > 0 Then
         Dim q = _AdviceRepo.GetAdvices.WithAdviceID(adviceId).FirstOrDefault
         If q IsNot Nothing Then
            q.Votes += 1
            q.TotalRating += rating
            'CacheObject.PurgeCacheItems("advices_advices_")
         End If
      End If
   End Sub

   'Public Function FindAdvices(ByVal where As mccEnum.SearchType, ByVal SearchWord As String) As List(Of Advice)
   '   Return FindAdvices(where, 0, CacheObject.MaxRows, SearchWord)
   'End Function


   'Public Function FindAdvices(ByVal where As mccEnum.SearchType, ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal SearchWord As String) As PagedList(Of Advice)
   '   Dim li As New List(Of Advice)
   '   Dim mdc As New MCCDataContext
   '   Select Case where
   '      Case mccEnum.SearchType.AnyWord
   '         Dim q = Advice.ContainsAny(SearchWord.Split().ToArray)
   '         li = (From it As Advice In mdc.mcc_Advices.Where(q)).Distinct().ToList()
   '      Case mccEnum.SearchType.AllWords

   '      Case mccEnum.SearchType.ExactPhrase
   '         li = _AdviceRepo.GetAdvices.Published.Where(Function(it) it.Tags.Contains(SearchWord) Or it.Title.Contains(SearchWord) Or it.Abstract.Contains(SearchWord) _
   '                Or it.Body.Contains(SearchWord)).ToPagedList(startRowIndex, maximumRows)
   '   End Select
   '   Return li
   'End Function

   Public Function FindAdvicesWithExactMatch(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal SearchWord As String) As PagedList(Of Advice) Implements IAdviceService.FindAdvicesWithExactMatch
      Dim key As String = String.Format("advice_search_{0}_{1}_{2}", SearchWord, startRowIndex, maximumRows)
      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), PagedList(Of Advice))
      End If
      Dim li As PagedList(Of Advice) = _AdviceRepo.GetAdvicesQuickList.Published.Where(Function(it) it.Tags.Contains(SearchWord) Or it.Title.Contains(SearchWord) Or it.Abstract.Contains(SearchWord) _
                 Or it.Body.Contains(SearchWord)).ToPagedList(startRowIndex, maximumRows)

      CacheData(key, li)
      Return li
   End Function


   Public Function FindAdvicesWithAnyMatch(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal SearchWord As String) As PagedList(Of Advice) Implements IAdviceService.FindAdvicesWithAnyMatch
      Dim key As String = String.Format("advice_search_{0}_{1}_{2}", SearchWord, startRowIndex, maximumRows)
      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), PagedList(Of Advice))
      End If

      'Dim q = Advice.ContainsAny(SearchWord.Split().ToArray)
      'li = (From it As Advice In mdc.mcc_Advices.Where(q)).Distinct().ToList()

        Dim searchKeys() As String = SearchWord.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)

      Dim li As PagedList(Of Advice) = _AdviceRepo.GetAdvicesQuickList.Published.Where(Function(it) it.Tags.ContainsAny(searchKeys, True) Or it.Title.ContainsAny(searchKeys, True) Or it.Abstract.ContainsAny(searchKeys, True) _
                 Or it.Body.ContainsAny(searchKeys, True)).ToPagedList(startRowIndex, maximumRows)

      '_AdviceRepo.GetAdvices.Published.Where(Function(p) p.ContainsAny)
      '_AdviceRepo.GetAdvicesQuickList.Published.Where().ToPagedList(startRowIndex, maximumRows)

      CacheData(key, li)
      Return li
   End Function

    Public Function FindAdviceWithExactMatchCount(ByVal SearchWord As String) As Integer Implements IAdviceService.FindAdviceWithExactMatchCount
        Return _AdviceRepo.GetAdvicesQuickList.Published.Where(Function(it) it.Tags.Contains(SearchWord) Or it.Title.Contains(SearchWord) Or it.Abstract.Contains(SearchWord) _
                   Or it.Body.Contains(SearchWord)).Count
    End Function

    Public Function FindAdviceWithAnyMatchCount(ByVal SearchWord As String) As Integer Implements IAdviceService.FindAdviceWithAnyMatchCount
        Return _AdviceRepo.GetAdvicesQuickList.Published.Where(Function(it) it.Tags.Contains(SearchWord) Or it.Title.Contains(SearchWord) Or it.Abstract.Contains(SearchWord) _
                 Or it.Body.Contains(SearchWord)).Count
    End Function


   '''' <summary>
   '''' Returns the number of total advices matching the search key
   '''' </summary>
   'Public Function FindAdvicesCount(ByVal where As mccEnum.SearchType, ByVal searchWord As String) As Integer
   '   Dim artCount As Integer = 0
   '   Select Case where
   '      Case mccEnum.SearchType.AnyWord
   '         Dim q = Advice.ContainsAny(searchWord.Split().ToArray)

   '         artCount = (From it As Advice In mdc.mcc_Advices.Where(q)).Count()
   '      Case mccEnum.SearchType.ExactPhrase
   '         artCount = _AdviceRepo.GetAdvices.Published.Count(Function(p) p.Tags.Contains(searchWord) Or p.Abstract.Contains(searchWord) _
   '                                                    Or p.Title.Contains(searchWord) Or p.Body.Contains(searchWord))
   '   End Select
   '   Return artCount
   'End Function


   Public Function GetAdvicesCountByStatus(ByVal status As Integer) As Integer Implements IAdviceService.GetAdvicesCountByStatus
      Dim key As String = "advices_advicecount_" & "st_" & status.ToString
      If Cache(key) IsNot Nothing Then
         Return CInt(Cache(key))
      Else
         Dim val As Integer = 0
         If status < 0 Then
            val = _AdviceRepo.GetAdvices.Count
         Else
            val = _AdviceRepo.GetAdvices.Where(Function(p) p.Status = status).Count
         End If
         CacheData(key, val)
      End If
   End Function

   Public Function GetAdvicesCountInCategoryByStatus(ByVal categoryID As Integer, ByVal status As Integer) As Integer Implements IAdviceService.GetAdvicesCountInCategoryByStatus

      Dim key As String = "advice_advicecount_" & categoryID.ToString & "_" & "st_" & status.ToString

      If (Cache(key) IsNot Nothing) Then
         Return CInt(Cache(key))
      Else
         If categoryID <= 0 Then
            Return GetAdvicesCountByStatus(status)
         End If
         Dim cnt As Integer = _AdviceRepo.GetAdvicesInCategory(categoryID).Where(Function(p) p.Status = status).Count
         CacheData(key, cnt)
         Return cnt
      End If
   End Function

   Public Function GetAdvicesByStatus(ByVal status As Integer, ByVal startRowIndex As Integer, ByVal maximumRows As Integer, Optional ByVal sortExp As String = "Title") As PagedList(Of Advice) Implements IAdviceService.GetAdvicesByStatus
      If status < 0 Then
         Return GetAdvices(startRowIndex, maximumRows, sortExp)
      End If

      Dim key As String = "advices_advices_" & "status_" & status.ToString & "_" & startRowIndex.ToString & "_" & maximumRows.ToString & "_" & sortExp

      If (Cache(key) IsNot Nothing) Then
            Return DirectCast(Cache(key), PagedList(Of Advice))
      Else
         Dim li As PagedList(Of Advice) = _AdviceRepo.GetAdvices.Where(Function(p) p.Status = status).SortBy(sortExp).ToPagedList(startRowIndex, maximumRows)
         CacheData(key, li)
         Return li
      End If
   End Function

   Public Function GetAdvicesByStatus(ByVal categoryID As Integer, ByVal status As Integer, ByVal startRowIndex As Integer, ByVal maximumRows As Integer, Optional ByVal sortExp As String = "AddedDate") As PagedList(Of Advice) Implements IAdviceService.GetAdvicesByStatus
      If categoryID <= 0 Then
         Return GetAdvicesByStatus(status, startRowIndex, maximumRows, sortExp)
      End If

      Dim key As String = "advices_advices_" & categoryID.ToString & "_" & "st_" & status.ToString & "_" & startRowIndex.ToString & "_" & maximumRows.ToString & "_" & sortExp

      If (Cache(key) IsNot Nothing) Then
            Return DirectCast(Cache(key), PagedList(Of Advice))
      Else
         Dim li As PagedList(Of Advice) = _AdviceRepo.GetAdvicesInCategory(categoryID).Where(Function(p) p.Status = status).ToPagedList(startRowIndex, maximumRows)
         CacheData(key, li)
         Return li
      End If
   End Function


   Public Sub UpdateAdviceStatus(ByVal id As Integer) Implements IAdviceService.UpdateAdviceStatus
      If id > 0 Then
         Dim q = _AdviceRepo.GetAdvices.WithAdviceID(id).FirstOrDefault()
         If q IsNot Nothing Then
            q.Status += 1
            _AdviceRepo.UpdateAdvice(q)
            CacheObject.PurgeCacheItems("advices_")
         End If
      End If
   End Sub

   Public Sub UpdateAdviceStatus(ByVal id As Integer, ByVal st As Integer) Implements IAdviceService.UpdateAdviceStatus
      If id > 0 Then
         Dim q = _AdviceRepo.GetAdvices.WithAdviceID(id).FirstOrDefault()
         If q IsNot Nothing AndAlso st <= 5 AndAlso st >= 0 Then
            q.Status = st
            _AdviceRepo.UpdateAdvice(q)
            CacheObject.PurgeCacheItems("advices_")
         End If
      End If
   End Sub

   'Public Sub UpdateAdviceStatus(ByVal id As Integer, ByVal st As mccEnum.Status)
   '   Dim it As Advice = (From ars As Advice In mdc.mcc_Advices Where ars.AdviceID = id).FirstOrDefault
   '   If it IsNot Nothing Then
   '      If st <= 5 AndAlso st >= 0 Then
   '         it.Status = CInt(st)

   '         If st <> mccEnum.Status.Approved Then
   '            it.Approved = False
   '         Else
   '            it.Approved = True
   '         End If

   '         mdc.SubmitChanges()

   '         CacheObject.PurgeCacheItems("advices_")
   '      End If
   '   End If
   'End Sub

   Public Function GetAdviceStatusValue(ByVal id As Integer) As Integer Implements IAdviceService.GetAdviceStatusValue
      Dim key As String = "advices_advice_statusval_" & id.ToString
      If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
      Else
         Dim val As Integer = -1
         val = _AdviceRepo.GetAdvices.WithAdviceID(id).Select(Function(P) P.Status).FirstOrDefault()
         CacheData(key, val)
         Return val
      End If
   End Function

   'Public Function GetAdviceStatus(ByVal adviceId As Integer) As mccEnum.Status
   '   Dim key As String = "advices_advice_status_" & adviceId.ToString
   '   If Cache(key) Then
   '      Return Cache(key)
   '   Else
   '      Dim stat As mccEnum.Status = mccEnum.Status.Pending
   '      
   '      Dim ival As Integer = (From it As Advice In mdc.mcc_Advices Where it.AdviceID = adviceId Select it.Status).FirstOrDefault
   '      stat = (CType(ival, mccEnum.Status))
   '      CacheData(key, stat)
   '      Return stat
   '   End If
   'End Function

   Public Function FixAdvicesVideoIds() As Boolean Implements IAdviceService.FixAdvicesVideoIds

      'Dim li As List(Of Advice) = (From it As Advice In mdc.mcc_Advices Where it.VideoId Is Nothing).ToList()
      'For Each it As Advice In li
      '   it.VideoId = 0
      'Next
      'mdc.SubmitChanges()
      'CacheObject.PurgeCacheItems("advices_")
      'CacheObject.PurgeCacheItems("fadvices_")
      'Return True

   End Function

   Public Function GetReadersPick() As Advice Implements IAdviceService.GetReadersPick
      Dim ar As Advice = Nothing
      Dim key As String = "advices_readerpick_"

      If Cache(key) IsNot Nothing Then
         Return CType(Cache(key), Advice)
      Else
         ar = _AdviceRepo.GetAdvices.SortBy("viewcount DESC").FirstOrDefault()
         CacheData(key, ar)
      End If
      Return ar
   End Function


   Public Function VoteUpAdvice(ByVal adviceId As Integer) As Object Implements IAdviceService.VoteUpAdvice
      Dim q = _AdviceRepo.GetAdvices.Where(Function(p) p.AdviceID = adviceId).FirstOrDefault()
      If q IsNot Nothing Then
         q.VoteUp += 1
         q.Votes += 1

         _AdviceRepo.UpdateAdvice(q)
         Return New With {.success = True, .id = q.AdviceID, .up = q.VoteUp, .down = q.VoteDown, .votes = q.Votes}
      End If
      Return 0
   End Function

   Public Function VoteDownAdvice(ByVal adviceId As Integer) As Object Implements IAdviceService.VoteDownAdvice
      Dim q = _AdviceRepo.GetAdvices.Where(Function(p) p.AdviceID = adviceId).FirstOrDefault()
      If q IsNot Nothing Then
         q.VoteDown += 1
         q.Votes += 1

         _AdviceRepo.UpdateAdvice(q)
         Return New With {.success = True, .id = q.AdviceID, .up = q.VoteUp, .down = q.VoteDown, .votes = q.Votes}
      End If
      Return 0
   End Function

   Protected Sub CacheData(ByVal key As String, ByVal data As Object)
      If data IsNot Nothing Then
         CacheObject.Cache.Insert(key, data, Nothing, DateTime.Now.AddSeconds(3600), TimeSpan.Zero)
      End If
   End Sub

End Class
