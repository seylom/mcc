Imports Microsoft.VisualBasic

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
Imports MCC.SiteLayers
Imports MCC.routines
Imports System.Linq
Imports MCC.Data

Namespace Advices
   Public Class AdviceCategoryRepository
      Inherits mccObject

      Public Shared Function GetCategoriesCount() As Integer
         Dim key As String = "adviceCategory_adviceCategorycount"
         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            Dim mdc As MCCDataContext = New MCCDataContext
            Dim i As Integer = mdc.mcc_AdviceCategories.Count()
            CacheData(key, i)
            Return i
         End If
      End Function

      Public Shared Function GetCategories() As List(Of mcc_AdviceCategory)
         Dim key As String = "adviceCategory_adviceCategory"
         If Cache(key) IsNot Nothing Then
            Return DirectCast(Cache(key), List(Of mcc_AdviceCategory))
         Else
            Dim mdc As MCCDataContext = New MCCDataContext
            Dim li As List(Of mcc_AdviceCategory) = (From t As mcc_AdviceCategory In mdc.mcc_AdviceCategories Select t).ToList
            CacheData(key, li)
            Return li
         End If
      End Function


      Public Shared Function GetCategories(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, Optional ByVal sort As String = "Title ASC") As List(Of mcc_AdviceCategory)
         Dim key As String = "adviceCategory_adviceCategory" & startRowIndex & "_" & maximumRows & "_"
         If Cache(key) IsNot Nothing Then
            Return DirectCast(Cache(key), List(Of mcc_AdviceCategory))
         Else
            Dim mdc As MCCDataContext = New MCCDataContext
            Dim li As New List(Of mcc_AdviceCategory)
            If startRowIndex > 0 AndAlso maximumRows > 0 Then
               li = (From t As mcc_AdviceCategory In mdc.mcc_AdviceCategories).Skip(startRowIndex - 1).Take(startRowIndex * maximumRows).ToList
            Else
               li = (From t As mcc_AdviceCategory In mdc.mcc_AdviceCategories).Skip(0).Take(20).ToList
            End If
            CacheData(key, li)
            Return li
         End If
      End Function

      Public Shared Function GetCategoryById(ByVal categoryId As Integer) As mcc_AdviceCategory
         Dim key As String = "adviceCategory_adviceCategory_" & categoryId.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Dim c As mcc_AdviceCategory = DirectCast(Cache(key), mcc_AdviceCategory)
            Return c
         Else
            Dim mdc As New MCCDataContext()
            Dim c As mcc_AdviceCategory
            If mdc.mcc_AdviceCategories.Count(Function(p) p.CategoryID = categoryId) > 0 Then
               c = (From it As mcc_AdviceCategory In mdc.mcc_AdviceCategories Where it.CategoryID = categoryId).FirstOrDefault
               CacheData(key, c)
               Return c
            Else
               Return Nothing
            End If
         End If
      End Function

      Public Shared Function GetCategoryBySlug(ByVal slug As String) As mcc_AdviceCategory
         Dim key As String = "adviceCategory_adviceCategory_" & slug & "_"
         If Cache(key) IsNot Nothing Then
            Dim c As mcc_AdviceCategory = DirectCast(Cache(key), mcc_AdviceCategory)
            Return c
         Else
            Dim mdc As New MCCDataContext()
            Dim c As mcc_AdviceCategory
            If mdc.mcc_AdviceCategories.Count(Function(p) p.Slug = slug) > 0 Then
               c = (From it As mcc_AdviceCategory In mdc.mcc_AdviceCategories Where it.Slug = slug).FirstOrDefault
               CacheData(key, c)
               Return c
            Else
               Return Nothing
            End If
         End If
      End Function

      Public Shared Function GetCategoriesByadviceId(ByVal adviceId As Integer) As List(Of mcc_AdviceCategory)
         Dim categories As List(Of mcc_AdviceCategory) = Nothing
         Dim key As String = "advices_Specified_Categories_" + adviceId.ToString

         If mccObject.Cache(key) IsNot Nothing Then
            categories = DirectCast(mccObject.Cache(key), List(Of mcc_AdviceCategory))
         Else
            Dim mdc As New MCCDataContext()
            Dim i As Integer = mdc.mcc_CategoriesAdvices.Count(Function(p) p.AdviceID = adviceId)
            If i > 0 Then
               Dim li As List(Of Integer) = (From it As mcc_CategoriesAdvice In mdc.mcc_CategoriesAdvices Where it.AdviceID = adviceId Select it.CategoryID).ToList
               categories = (From c As mcc_AdviceCategory In mdc.mcc_AdviceCategories Where li.Contains(c.CategoryID)).ToList
               CacheData(key, categories)
            End If
         End If
         Return categories
      End Function

      Public Shared Sub InsertCategory(ByVal title As String, ByVal importance As Integer, ByVal description As String, ByVal imageUrl As String, ByVal parentCategoryId As Integer)
         Dim mdc As MCCDataContext = New MCCDataContext
         Dim c As New mcc_AdviceCategory

         With c
            .AddedDate = DateTime.Now
            .AddedBy = mccObject.CurrentUserName
            .Title = title
            .Importance = importance
            .Description = description
            .ImageUrl = imageUrl
            .Slug = routines.GetSlugFromString(title)
            .ParentCategoryID = parentCategoryId
         End With

         mdc.mcc_AdviceCategories.InsertOnSubmit(c)
         mdc.SubmitChanges()
         mccObject.PurgeCacheItems("adviceCategory_")
      End Sub


      Public Shared Sub UpdateCategory(ByVal categoryId As Integer, ByVal title As String, ByVal importance As Integer, ByVal description As String, ByVal imageUrl As String, ByVal parentCategoryId As Integer)
         Dim mdc As MCCDataContext = New MCCDataContext
         Dim c As mcc_AdviceCategory = (From t In mdc.mcc_AdviceCategories _
                                     Where t.CategoryID = categoryId _
                                     Select t).Single()
         If c IsNot Nothing Then
            With c
               .Title = title
               .Description = description
               .Importance = importance
               .ImageUrl = imageUrl
               .Slug = routines.GetSlugFromString(title)
               .ParentCategoryID = parentCategoryId
            End With
            mdc.SubmitChanges()
            mccObject.PurgeCacheItems("adviceCategory_")
            mccObject.PurgeCacheItems("adviceCategory_adviceCategory_")
         End If
      End Sub


      Public Shared Sub DeleteCategory(ByVal CategoryID As Integer)
         Dim mdc As MCCDataContext = New MCCDataContext
         Dim wrd As mcc_AdviceCategory = (From t In mdc.mcc_AdviceCategories _
                                     Where t.CategoryID = CategoryID _
                                     Select t).Single()

         If wrd IsNot Nothing Then
            mdc.mcc_AdviceCategories.DeleteOnSubmit(wrd)
            Dim cs As System.Data.Linq.ChangeSet = mdc.GetChangeSet()
            mdc.SubmitChanges()
            mccObject.PurgeCacheItems("adviceCategory_")
         End If
      End Sub

      Protected Shared Sub CacheData(ByVal key As String, ByVal data As Object)
         If data IsNot Nothing Then
            mccObject.Cache.Insert(key, data, Nothing, DateTime.Now.AddSeconds(3600), TimeSpan.Zero)
         End If
      End Sub

   End Class
End Namespace
