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
Imports MCC.routines
Imports System.Linq
Imports MCC.Dynamic
Imports MCC.Data

Namespace Advices
   Public Class AdviceRepository
      Inherits mccObject
      Implements IAdviceRepository

      Private mdc As MCCDataContext = New MCCDataContext()


      Public Sub New()
         CacheKey = "Advices_"
      End Sub

      Public Function AverageRating(ByVal adviceId As Integer) As Double Implements IAdviceRepository.AverageRating

         Dim wrd As mcc_Advice = (From t In mdc.mcc_Advices _
                                     Where t.AdviceID = adviceId _
                                     Select t).Single()
         If wrd IsNot Nothing Then
            If wrd.Votes >= 1 Then
               Return (CDbl(wrd.TotalRating) / CDbl(wrd.Votes))
            Else
               Return 0
            End If
         Else
            Return 0
         End If
      End Function

      Public Function GetComments(ByVal adviceId As Integer) As List(Of mcc_AdviceComment) Implements IAdviceRepository.GetComments
         Dim key As String = "AdviceComments_" & adviceId.ToString
         If Cache(key) IsNot Nothing Then
            Return DirectCast(Cache(key), List(Of mcc_AdviceComment))
         Else
            Dim _comments As List(Of mcc_AdviceComment) = Nothing
            Dim it As Integer = mdc.mcc_AdviceComments.Count(Function(p) p.AdviceId = adviceId)
            If it > 0 Then
               _comments = (From c As mcc_AdviceComment In mdc.mcc_AdviceComments Where c.AdviceId = adviceId)
               CacheData(key, it)
            End If
            Return _comments
         End If
      End Function

      Public Function CommentsCount(ByVal adviceId As Integer) As Integer Implements IAdviceRepository.CommentsCount
         Return mdc.mcc_AdviceComments.Count(Function(p) p.AdviceId = adviceId)
      End Function

      Public Function GetAdviceCount() As Integer Implements IAdviceRepository.GetAdviceCount
         Dim key As String = "AdviceCount_"

         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            mdc.mcc_Advices.Count()
            Dim it As Integer = mdc.mcc_Advices.Count()
            CacheData(key, it)
            Return it
         End If
      End Function

      Public Function GetAdviceCountByAuthor(ByVal addedBy As String) As Integer Implements IAdviceRepository.GetAdviceCountByAuthor
         If Not String.IsNullOrEmpty(addedBy) Then
            Dim key As String = "AdviceCount_" & addedBy & "_"

            If Cache(key) IsNot Nothing Then
               Return CInt(Cache(key))
            Else
               Dim it As Integer = mdc.mcc_Advices.Count(Function(p) p.AddedBy = addedBy)
               CacheData(key, it)
               Return it
            End If
         Else
            Return 0
         End If
      End Function

      Public Function GetAdviceCount(ByVal categoryId As Integer) As Integer Implements IAdviceRepository.GetAdviceCount
         If categoryId <= 0 Then
            Return GetAdviceCount()
         End If

         Dim key As String = "AdviceCount_" & categoryId.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            Dim catAr As List(Of Integer) = (From ef As mcc_CategoriesAdvice In mdc.mcc_CategoriesAdvices Where ef.CategoryID = categoryId Select ef.AdviceID).ToList
            Dim it As Integer = mdc.mcc_Advices.Count(Function(p) catAr.Contains(p.AdviceID))

            CacheData(key, it)
            Return it
         End If
      End Function

      Public Function GetAdviceCount(ByVal PublishedOnly As Boolean) As Integer Implements IAdviceRepository.GetAdviceCount
         If Not PublishedOnly Then
            Return GetAdviceCount()
         End If

         Dim key As String = "AdviceCount_" & PublishedOnly.ToString

         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            Dim it As Integer = mdc.mcc_Advices.Count(Function(p) p.Approved = True)
            CacheData(key, it)
            Return it
         End If
      End Function

      Public Function GetAdvices() As List(Of mcc_Advice) Implements IAdviceRepository.GetAdvices
         Dim key As String = "Advices_"
         If Cache(key) IsNot Nothing Then
            Dim li As List(Of mcc_Advice) = DirectCast(Cache(key), List(Of mcc_Advice))
            Return li
         Else
            Dim li As List(Of mcc_Advice) = mdc.mcc_Advices.ToList
            CacheData(key, li)
            Return li
         End If
      End Function

      Public Function GetAdviceCount(ByVal publishedOnly As Boolean, ByVal categoryID As Integer) As Integer Implements IAdviceRepository.GetAdviceCount
         If Not publishedOnly Then
            Return GetAdviceCount(categoryID)
         End If

         If categoryID <= 0 Then
            Return GetAdviceCount(publishedOnly)
         End If

         Dim key As String = "AdviceCount_" + publishedOnly.ToString() + "_" + categoryID.ToString()
         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            Dim catAr As List(Of Integer) = (From ef As mcc_CategoriesAdvice In mdc.mcc_CategoriesAdvices Where ef.CategoryID = categoryID Select ef.AdviceID).ToList

            Dim it As Integer = mdc.mcc_Advices.Count(Function(p) catAr.Contains(p.AdviceID) AndAlso p.Approved = True)

            CacheData(key, it)
            Return it
         End If
      End Function

      Public Function GetAdvices(ByVal categoryId As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AdviceId") As List(Of mcc_Advice) Implements IAdviceRepository.GetAdvices
         If categoryId > 0 Then
            Dim key As String = "Advices_" & categoryId.ToString & "_" & sortExp.Replace(" ", "") & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
            If String.IsNullOrEmpty(sortExp) Then
               sortExp = "AdviceId"
            End If
            If Cache(key) IsNot Nothing Then
               Dim li As List(Of mcc_Advice) = DirectCast(Cache(key), List(Of mcc_Advice))
               Return li
            Else
               Dim li As List(Of mcc_Advice)

               Dim catAr As List(Of Integer) = (From ef As mcc_CategoriesAdvice In mdc.mcc_CategoriesAdvices Where ef.CategoryID = categoryId Select ef.AdviceID).ToList

               li = (From it As mcc_Advice In mdc.mcc_Advices Where catAr.Contains(it.AdviceID)).SortBy(sortExp).Skip(startrowindex).Take(maximumrows).ToList
               CacheData(key, li)
               Return li
            End If
         Else
            Return GetAdvices(startrowindex, maximumrows, sortExp)
         End If
      End Function

      Public Function GetAdvices(ByVal publishedOnly As Boolean, ByVal categoryId As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AdviceId") As List(Of mcc_Advice) Implements IAdviceRepository.GetAdvices
         If Not publishedOnly Then
            Return GetAdvices(categoryId, startrowindex, maximumrows, sortExp)
         End If

         If categoryId > 0 Then
            If String.IsNullOrEmpty(sortExp) Then
               sortExp = "AdviceId"
            End If
            Dim key As String = "Advices_" & categoryId.ToString & "_" & sortExp.Replace(" ", "") & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
            If Cache(key) IsNot Nothing Then
               Dim li As List(Of mcc_Advice) = DirectCast(Cache(key), List(Of mcc_Advice))
               Return li
            Else

               Dim li As List(Of mcc_Advice)

               Dim catAr As List(Of Integer) = (From ef As mcc_CategoriesAdvice In mdc.mcc_CategoriesAdvices Where ef.CategoryID = categoryId Select ef.AdviceID).ToList
               li = (From it As mcc_Advice In mdc.mcc_Advices Where catAr.Contains(it.AdviceID) AndAlso it.Approved = True).SortBy(sortExp).Skip(startrowindex).Take(maximumrows).ToList
               CacheData(key, li)
               Return li
            End If
         Else
            Return GetAdvices(publishedOnly, startrowindex, maximumrows, sortExp)
         End If
      End Function

      Public Function GetAdvicesByAuthor(ByVal addedBy As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedby") As List(Of mcc_Advice) Implements IAdviceRepository.GetAdvicesByAuthor
         If Not String.IsNullOrEmpty(addedBy) Then
            If String.IsNullOrEmpty(sortExp) Then
               sortExp = "AdviceId"
            End If
            Dim key As String = "Advices_" & addedBy & "_" & sortExp.Replace(" ", "") & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
            If Cache(key) IsNot Nothing Then
               Dim li As List(Of mcc_Advice) = DirectCast(Cache(key), List(Of mcc_Advice))
               Return li
            Else

               Dim li As List(Of mcc_Advice)
               li = (From it As mcc_Advice In mdc.mcc_Advices Where it.AddedBy = addedBy).SortBy(sortExp).Skip(startrowindex).Take(maximumrows).ToList
               CacheData(key, li)
               Return li
            End If
         Else
            Return GetAdvices(startrowindex, maximumrows, sortExp)
         End If
      End Function

      Public Function GetAdvicesByAuthor(ByVal publishedOnly As Boolean, ByVal addedBy As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedby") As List(Of mcc_Advice) Implements IAdviceRepository.GetAdvicesByAuthor

         If Not publishedOnly Then
            Return GetAdvicesByAuthor(addedBy, startrowindex, maximumrows, sortExp)
         End If

         If Not String.IsNullOrEmpty(addedBy) Then
            If String.IsNullOrEmpty(sortExp) Then
               sortExp = "AdviceId"
            End If
            Dim key As String = "Advices_" & addedBy & "_" & publishedOnly.ToString & "_" & sortExp.Replace(" ", "") & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
            If Cache(key) IsNot Nothing Then
               Dim li As List(Of mcc_Advice) = DirectCast(Cache(key), List(Of mcc_Advice))
               Return li
            Else

               Dim li As List(Of mcc_Advice)
               li = (From it As mcc_Advice In mdc.mcc_Advices Where it.AddedBy = addedBy AndAlso it.Approved = True).SortBy(sortExp).Skip(startrowindex).Take(maximumrows).ToList
               CacheData(key, li)
               Return li
            End If
         Else
            Return GetAdvices(startrowindex, maximumrows, sortExp)
         End If
      End Function

      Public Function GetAdvices(ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AdviceId") As List(Of mcc_Advice) Implements IAdviceRepository.GetAdvices
         Dim key As String = "Advices_" & sortExp.Replace(" ", "") & startrowindex.ToString & "_" & "_" & maximumrows.ToString & "_"
         If String.IsNullOrEmpty(sortExp) Then
            sortExp = "AdviceId"
         End If
         If Cache(key) IsNot Nothing Then
            Dim li As List(Of mcc_Advice) = DirectCast(Cache(key), List(Of mcc_Advice))
            Return li
         Else
            Dim li As New List(Of mcc_Advice)
            Dim i As Integer = mdc.mcc_Advices.Count()
            If i > 0 Then
               li = mdc.mcc_Advices.SortBy(sortExp).Skip(startrowindex).Take(maximumrows).ToList
               CacheData(key, li)
            End If
            Return li
         End If
      End Function

      Public Function GetAdvices(ByVal publishedOnly As Boolean, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "AdviceId") As List(Of mcc_Advice) Implements IAdviceRepository.GetAdvices

         If publishedOnly Then
            If String.IsNullOrEmpty(sortExp) Then
               sortExp = "AdviceId"
            End If
            Dim key As String = "Advices_" & publishedOnly.ToString & "_" & sortExp.Replace(" ", "") & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
            If Cache(key) IsNot Nothing Then
               Dim li As List(Of mcc_Advice) = DirectCast(Cache(key), List(Of mcc_Advice))
               Return li
            Else
               Dim li As New List(Of mcc_Advice)
               Dim i As Integer = mdc.mcc_Advices.Count()
               If i > 0 Then
                  li = (From a As mcc_Advice In mdc.mcc_Advices Where a.Approved = True).SortBy(sortExp).Skip(startrowindex).Take(maximumrows).ToList
                  CacheData(key, li)
               End If
               Return li
            End If
         Else
            Return GetAdvices(startrowindex, maximumrows, sortExp)
         End If
      End Function


      ''' <summary>
      ''' Returns an Advice object with the specified ID
      ''' </summary>
      Public Function GetLatestAdvices(ByVal pageSize As Integer) As List(Of mcc_Advice) Implements IAdviceRepository.GetLatestAdvices

         Dim Advices As New List(Of mcc_Advice)
         Dim key As String = "Advices_Latest_" + pageSize.ToString

         If mccObject.Cache(key) IsNot Nothing Then
            Advices = DirectCast(mccObject.Cache(key), List(Of mcc_Advice))
         Else
            Advices = GetAdvices(True, 0, pageSize, "ReleaseDate DESC")
         End If
         Return Advices
      End Function

      Public Function GetAdviceById(ByVal AdviceId As Integer) As mcc_Advice Implements IAdviceRepository.GetAdviceById
         Dim key As String = "Advices_" & AdviceId.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Dim fb As mcc_Advice = DirectCast(Cache(key), mcc_Advice)
            Return fb
         Else
            Dim fb As mcc_Advice
            If mdc.mcc_Advices.Count(Function(p) p.AdviceID = AdviceId) > 0 Then
               fb = (From it As mcc_Advice In mdc.mcc_Advices Where it.AdviceID = AdviceId).FirstOrDefault
               CacheData(key, fb)
               Return fb
            Else
               Return Nothing
            End If
         End If
      End Function

      Public Function GetAdviceBySlug(ByVal slug As String) As mcc_Advice Implements IAdviceRepository.GetAdviceBySlug
         Dim key As String = "Advices_" & slug & "_"
         If Cache(key) IsNot Nothing Then
            Dim fb As mcc_Advice = DirectCast(Cache(key), mcc_Advice)
            Return fb
         Else
            Dim fb As mcc_Advice
            If mdc.mcc_Advices.Count(Function(p) p.Slug = slug) > 0 Then
               fb = (From it As mcc_Advice In mdc.mcc_Advices Where it.Slug = slug).FirstOrDefault
               CacheData(key, fb)
               Return fb
            Else
               Return Nothing
            End If
         End If
      End Function

      ''' <summary>
      ''' Creates a new Advice
      ''' </summary>
      Public Sub InsertAdvice(ByVal title As String, ByVal Abstract As String, ByVal body As String, ByVal approved As Boolean, ByVal listed As Boolean, ByVal commentsEnabled As Boolean, _
       ByVal onlyForMembers As Boolean, ByVal tags As String) Implements IAdviceRepository.InsertAdvice


         'Dim canApprove As Boolean = (mccObject.CurrentUser.IsInRole("Administrators") OrElse mccObject.CurrentUser.IsInRole("Editors"))
         'If Not canApprove Then
         '   approved = False
         'End If

         ' on an inserted Advice the approved flagged will always be false.
         approved = False

         Dim wrd As New mcc_Advice

         title = mccObject.ConvertNullToEmptyString(title)
         Abstract = mccObject.ConvertNullToEmptyString(Abstract)
         body = mccObject.ConvertNullToEmptyString(body)
         tags = mccObject.ConvertNullToEmptyString(tags)

         With wrd
            .AddedDate = DateTime.Now
            .AddedBy = mccObject.CurrentUserName
            .Title = title
            .Abstract = Abstract
            .Body = body
            .OnlyForMembers = onlyForMembers
            .Approved = approved
            .Listed = listed
            .CommentsEnabled = commentsEnabled
            .Tags = tags
            .Slug = routines.GetSlugFromString(title)
         End With

         mdc.mcc_Advices.InsertOnSubmit(wrd)
         mdc.SubmitChanges()
         mccObject.PurgeCacheItems("Advices_")
         mccObject.PurgeCacheItems("Advices_Latest_")
         mccObject.PurgeCacheItems("Advice")

         mccObject.PurgeCacheItems("fAdvices")
         mccObject.PurgeCacheItems("fAdvicecount_")

      End Sub


      Public Sub UpdateAdvice(ByVal AdviceId As Integer, ByVal title As String, ByVal Abstract As String, ByVal body As String, ByVal approved As Boolean, ByVal listed As Boolean, _
 ByVal commentsEnabled As Boolean, ByVal onlyForMembers As Boolean, ByVal tags As String) Implements IAdviceRepository.UpdateAdvice

         Dim wrd As mcc_Advice = (From t In mdc.mcc_Advices _
                                     Where t.AdviceID = AdviceId _
                                     Select t).Single()
         If wrd IsNot Nothing Then

            title = mccObject.ConvertNullToEmptyString(title)
            Abstract = mccObject.ConvertNullToEmptyString(Abstract)
            body = mccObject.ConvertNullToEmptyString(body)
            tags = mccObject.ConvertNullToEmptyString(tags)

            With wrd
               .Title = title
               .Abstract = Abstract
               .Body = body
               .OnlyForMembers = onlyForMembers
               .Approved = approved
               .Listed = listed
               .CommentsEnabled = commentsEnabled
               .Slug = routines.GetSlugFromString(title)
            End With

            mdc.SubmitChanges()

            mccObject.PurgeCacheItems("AdviceCount")
            mccObject.PurgeCacheItems("Advice_" + AdviceId.ToString())
            mccObject.PurgeCacheItems("Advices_Latest_")
            mccObject.PurgeCacheItems("Advices")
            mccObject.PurgeCacheItems("Specified_Categories_" + AdviceId.ToString)

            mccObject.PurgeCacheItems("fAdvices")

         End If
      End Sub

      Public Sub DeleteAdvice(ByVal AdviceId As Integer) Implements IAdviceRepository.DeleteAdvice
         If mdc.mcc_Advices.Count(Function(p) p.AdviceID = AdviceId) > 0 Then
            Dim wrd As mcc_Advice = (From t In mdc.mcc_Advices _
                                        Where t.AdviceID = AdviceId _
                                        Select t).Single()
            If wrd IsNot Nothing Then
               mdc.mcc_Advices.DeleteOnSubmit(wrd)
               mdc.SubmitChanges()
               mccObject.PurgeCacheItems("Advices_")

               RemoveAdviceFromCategory(AdviceId)
               Dim tb As New MCCEvents.MCCEvents.RecordDeletedEvent("Advice", AdviceId, Nothing)
               tb.Raise()

               mccObject.PurgeCacheItems("Advice")
               mccObject.PurgeCacheItems("Advices_")
               mccObject.PurgeCacheItems("Advices_Latest_")
               mccObject.PurgeCacheItems("Specified_Categories_" + AdviceId.ToString)

               mccObject.PurgeCacheItems("fAdvices")
               mccObject.PurgeCacheItems("fAdvicecount_")
            End If
         End If
      End Sub

      Public Sub ApproveAdvice(ByVal AdviceId As Integer) Implements IAdviceRepository.ApproveAdvice
         If mdc.mcc_Advices.Count(Function(p) p.AdviceID = AdviceId) > 0 Then
            Dim wrd As mcc_Advice = (From t In mdc.mcc_Advices _
                                        Where t.AdviceID = AdviceId _
                                        Select t).Single()
            wrd.Approved = True
            mdc.SubmitChanges()
            mccObject.PurgeCacheItems("AdviceCount")
            mccObject.PurgeCacheItems("Advice_" + AdviceId.ToString())
            mccObject.PurgeCacheItems("Advices")
            mccObject.PurgeCacheItems("Advices_Latest_")

            mccObject.PurgeCacheItems("fAdvices")
            mccObject.PurgeCacheItems("fAdvicecount_")
         End If
      End Sub

      Public Function RemoveAdviceFromCategory(ByVal AdviceID As Integer, Optional ByVal categoryId As Integer = 0) As Boolean Implements IAdviceRepository.RemoveAdviceFromCategory
         'Dim ret As Boolean = SiteProvider.Advices.RemoveAdviceFromCategory(AdviceID, categoryId)
         Dim ret As Boolean = True
         If mdc.mcc_Advices.Count(Function(p) p.AdviceID = AdviceID) > 0 AndAlso mdc.mcc_CategoriesAdvices.Count(Function(p) p.AdviceID = AdviceID AndAlso p.CategoryID = categoryId) > 0 Then
            Dim wrd As mcc_CategoriesAdvice = (From t In mdc.mcc_CategoriesAdvices _
                                        Where t.AdviceID = AdviceID AndAlso t.CategoryID = categoryId Select t).Single()
            mdc.mcc_CategoriesAdvices.DeleteOnSubmit(wrd)
            mdc.SubmitChanges()

            mccObject.PurgeCacheItems("Advice")
            mccObject.PurgeCacheItems("AdviceInCategories_")
            mccObject.PurgeCacheItems("Specified_Categories_" + AdviceID.ToString)
         End If
         Return ret
      End Function

      Public Function AddAdviceToCategory(ByVal adviceId As Integer, ByVal CategoryId As Integer) As Integer Implements IAdviceRepository.AddAdviceToCategory
         'Dim ret As Integer = SiteProvider.Advices.AddAdviceToCategory(AdviceId, CategoryId)
         Dim mdc As MCCDataContext = New MCCDataContext
         If mdc.mcc_CategoriesAdvices.Count(Function(p) p.AdviceID = adviceId AndAlso p.CategoryID = CategoryId) = 0 Then
            Dim cid As New mcc_CategoriesAdvice
            cid.AdviceID = adviceId
            cid.CategoryID = CategoryId

            mdc.mcc_CategoriesAdvices.InsertOnSubmit(cid)
            mdc.SubmitChanges()
            mccObject.PurgeCacheItems("AdviceInCategories_")
         End If
      End Function


      Public Sub IncrementViewCount(ByVal AdviceId As Integer) Implements IAdviceRepository.IncrementViewCount
         If mdc.mcc_Advices.Count(Function(p) p.AdviceID = AdviceId) > 0 Then
            Dim wrd As mcc_Advice = (From t In mdc.mcc_Advices _
                                        Where t.AdviceID = AdviceId _
                                        Select t).Single()
            wrd.ViewCount += 1
            mdc.SubmitChanges()
            mccObject.PurgeCacheItems("Advices_")
            mccObject.PurgeCacheItems("AdviceCount_")
         End If
      End Sub

      Public Sub RateAdvice(ByVal adviceId As Integer, ByVal rating As Integer) Implements IAdviceRepository.RateAdvice
         Dim mdc As MCCDataContext = New MCCDataContext
         If mdc.mcc_Advices.Count(Function(p) p.AdviceID = adviceId) > 0 Then
            Dim wrd As mcc_Advice = (From t In mdc.mcc_Advices _
                                        Where t.AdviceID = adviceId _
                                        Select t).Single()
            wrd.Votes += 1
            wrd.TotalRating += rating

            mdc.SubmitChanges()
            mccObject.PurgeCacheItems("Advices_")
            mccObject.PurgeCacheItems("AdviceCount_")
         End If
      End Sub

      ''' <summary>
      ''' Vote up advice
      ''' </summary>
      Public Function VoteUpAdvice(ByVal adviceId As Integer) As String Implements IAdviceRepository.VoteUpAdvice
         Dim ret As String = ""
         If mdc.mcc_Advices.Count(Function(p) p.AdviceID = adviceId) > 0 Then
            Dim wrd As mcc_Advice = (From t In mdc.mcc_Advices _
                                               Where t.AdviceID = adviceId _
                                               Select t).Single()
            If wrd.VoteUp IsNot Nothing Then
               wrd.VoteUp += 1
            Else
               wrd.VoteUp = 1
            End If

            ret = "success" & "|" & adviceId.ToString & "|" & wrd.VoteUp.ToString & "|" & wrd.VoteDown.ToString

            mdc.SubmitChanges()
            mccObject.PurgeCacheItems("Advices_")
         Else
            ret = "failure" & "|" & adviceId.ToString
         End If
         Return ret
      End Function

      Public Function AllVotes(ByVal adviceId As Integer) As String Implements IAdviceRepository.AllVotes
         Dim str As String = ""
         If mdc.mcc_Advices.Count(Function(p) p.AdviceID = adviceId) > 0 Then
            Dim wrd As mcc_Advice = (From t In mdc.mcc_Advices _
                                                    Where t.AdviceID = adviceId _
                                                    Select t).Single()

            If wrd.VoteUp IsNot Nothing AndAlso wrd.VoteDown Then
               str = wrd.VoteUp & "|" & wrd.VoteDown
            Else
               If wrd.VoteUp IsNot Nothing Then
                  str = wrd.VoteUp & "|0"
               Else
                  str = "0|" & wrd.VoteDown
               End If
            End If

         End If
         Return str
      End Function


      ''' <summary>
      ''' vote down advice
      ''' </summary>
      Public Function VoteDownAdvice(ByVal adviceId As Integer) As String Implements IAdviceRepository.VoteDownAdvice
         Dim ret As String = ""
         If mdc.mcc_Advices.Count(Function(p) p.AdviceID = adviceId) > 0 Then
            Dim wrd As mcc_Advice = (From t In mdc.mcc_Advices _
                                               Where t.AdviceID = adviceId _
                                               Select t).Single()

            If wrd.VoteDown IsNot Nothing Then
               wrd.VoteDown += 1
            Else
               wrd.VoteDown = 1
            End If

            ret = "success" & "|" & adviceId.ToString & "|" & wrd.VoteUp.ToString & "|" & wrd.VoteDown.ToString

            mdc.SubmitChanges()
            mccObject.PurgeCacheItems("Advices_")
         Else
            ret = "failure" & "|" & adviceId.ToString
         End If

         Return ret
      End Function

      'Public Shared Function FindAdvices(ByVal SearchWord As String) As List(Of mcc_Advice)
      '   Return FindAdvices(0, mccObject.MaxRows, SearchWord)
      'End Function

      'Public Shared Function FindAdvices(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal SearchWord As String) As List(Of mcc_Advice)
      '   'Dim Advices As List(Of Advice) = Nothing

      '   'Dim recordset As List(Of AdviceDetails) = SiteProvider.Advices.FindAdvices(GetPageIndex(startRowIndex, maximumRows), maximumRows, SearchWord)
      '   'Advices = GetAdviceListFromAdviceDetailsList(recordset)


      '   Dim mdc As New MCCDataContext()
      '   Dim iLst As List(Of FindAdviceResult) = mdc.FindAdvice(SearchWord).ToList

      '   Dim li As List(Of mcc_Advice) = (From it As mcc_Advice In mdc.mcc_Advices Join fts In mdc.FindAdvice(SearchWord) On it.AdviceID Equals fts.AdviceID Select it).ToList

      '   Return li
      'End Function

      '''' <summary>
      '''' Returns a collection with all Advices for the specified category
      '''' </summary>
      'Public Shared Function FindAdvices(ByVal categoryID As Integer, ByVal SearchWord As String) As List(Of mcc_Advice)
      '   Return FindAdvices(categoryID, 0, mccObject.MaxRows, SearchWord)
      'End Function

      'Public Shared Function FindAdvices(ByVal categoryID As Integer, ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal SearchWord As String) As List(Of mcc_Advice)
      '   If categoryID <= 0 Then
      '      Return FindAdvices(startRowIndex, maximumRows, SearchWord)
      '   End If

      '   'Dim Advices As List(Of Advice) = Nothing

      '   'Dim recordset As List(Of AdviceDetails) = SiteProvider.Advices.FindAdvices(categoryID, GetPageIndex(startRowIndex, maximumRows), maximumRows, SearchWord)
      '   'Advices = GetAdviceListFromAdviceDetailsList(recordset)

      '   Return Nothing
      'End Function

      '''' <summary>
      '''' Returns the number of total Advices matching the search key
      '''' </summary>
      'Public Shared Function FindAdvicesCount(ByVal searchWord As String) As Integer
      '   Dim artCount As Integer = 0
      '   'artCount = SiteProvider.Advices.FindAdviceCount(searchWord)

      '   Dim mdc As New MCCDataContext()
      '   Dim cr As mcc_Advices_FindAdvicesCountResult = mdc.mcc_Advices_FindAdvicesCount(searchWord)
      '   artCount = cr.Column1()  ' single result value

      '   Return artCount
      'End Function

      Protected Sub CacheData(ByVal key As String, ByVal data As Object)
         If data IsNot Nothing Then
            mccObject.Cache.Insert(CacheKey & key, data, Nothing, DateTime.Now.AddSeconds(3600), TimeSpan.Zero)
         End If
      End Sub
   End Class
End Namespace
