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
Imports MCC.routines
Imports System.Linq


Namespace Articles
   Public Class ArticleCategoryRepository
      Inherits mccObject
      Implements IArticleCategoryRepository

      Public Function GetCategoriesCount() As Integer Implements IArticleCategoryRepository.GetCategoriesCount
         Dim key As String = "articleCategory_articleCategorycount"
         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            Dim mdc As MCCDataContext = New MCCDataContext
            Dim i As Integer = mdc.mcc_Categories.Count()
            CacheData(key, i)
            Return i
         End If
      End Function

      Public Function GetCategories() As List(Of mcc_Category) Implements IArticleCategoryRepository.GetCategories
         Dim key As String = "articleCategory_articleCategory"
         If Cache(key) IsNot Nothing Then
            Return DirectCast(Cache(key), List(Of mcc_Category))
         Else
            Dim mdc As MCCDataContext = New MCCDataContext
            Dim li As List(Of mcc_Category) = (From t As mcc_Category In mdc.mcc_Categories Select t).ToList
            CacheData(key, li)
            Return li
         End If
      End Function

      Public Function GetMainCategories() As List(Of mcc_Category) Implements IArticleCategoryRepository.GetMainCategories
         Dim key As String = "articleCategory_articleMainCategory"
         If Cache(key) IsNot Nothing Then
            Return DirectCast(Cache(key), List(Of mcc_Category))
         Else
            Dim mdc As MCCDataContext = New MCCDataContext
            Dim li As List(Of mcc_Category) = (From t As mcc_Category In mdc.mcc_Categories Where t.ParentCategoryID <= 0).ToList
            CacheData(key, li)
            Return li
         End If
      End Function


      Public Function GetCategories(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, Optional ByVal sort As String = "Title ASC") As List(Of mcc_Category) Implements IArticleCategoryRepository.GetCategories
         Dim key As String = "articleCategory_articleCategory" & startRowIndex & "_" & maximumRows & "_"
         If Cache(key) IsNot Nothing Then
            Return DirectCast(Cache(key), List(Of mcc_Category))
         Else
            Dim mdc As MCCDataContext = New MCCDataContext
            Dim li As New List(Of mcc_Category)

            startRowIndex = IIf(startRowIndex >= 0, startRowIndex, 0)
            maximumRows = IIf(maximumRows > 0, maximumRows, 20)

            li = (From t As mcc_Category In mdc.mcc_Categories).Skip(startRowIndex).Take(maximumRows).ToList
            CacheData(key, li)
            Return li
         End If
      End Function


      Public Function GetParentCategories() As List(Of mcc_Category) Implements IArticleCategoryRepository.GetParentCategories
         Dim key As String = "articleCategory_articleParentCategory_"
         If Cache(key) IsNot Nothing Then
            Dim c As List(Of mcc_Category) = DirectCast(Cache(key), List(Of mcc_Category))
            Return c
         Else
            Dim mdc As New MCCDataContext()
            Dim c As List(Of mcc_Category) = (From it As mcc_Category In mdc.mcc_Categories Where it.ParentCategoryID <= 0).ToList()
            CacheData(key, c)
            Return c
         End If
      End Function

      Public Function GetCategoryById(ByVal categoryId As Integer) As mcc_Category Implements IArticleCategoryRepository.GetCategoryById
         Dim key As String = "articleCategory_articleCategory_" & categoryId.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Dim c As mcc_Category = DirectCast(Cache(key), mcc_Category)
            Return c
         Else
            Dim mdc As New MCCDataContext()
            Dim c As mcc_Category = (From it As mcc_Category In mdc.mcc_Categories Where it.CategoryID = categoryId).FirstOrDefault
            CacheData(key, c)
            Return c
         End If
      End Function

      Public Function GetCategoryBySlug(ByVal slug As String) As mcc_Category Implements IArticleCategoryRepository.GetCategoryBySlug
         Dim key As String = "articleCategory_articleCategory_" & slug & "_"
         If Cache(key) IsNot Nothing Then
            Dim c As mcc_Category = DirectCast(Cache(key), mcc_Category)
            Return c
         Else
            Dim mdc As New MCCDataContext()
            Dim c As mcc_Category = (From it As mcc_Category In mdc.mcc_Categories Where it.Slug = slug).FirstOrDefault
            CacheData(key, c)
            Return c
         End If
      End Function

      Public Function GetCategoriesByArticleId(ByVal ArticleId As Integer) As List(Of mcc_Category) Implements IArticleCategoryRepository.GetCategoriesByArticleId
         Dim categories As List(Of mcc_Category) = Nothing
         Dim key As String = "Articles_Specified_Categories_" + ArticleId.ToString

         If mccObject.Cache(key) IsNot Nothing Then
            categories = DirectCast(mccObject.Cache(key), List(Of mcc_Category))
         Else
            Dim mdc As New MCCDataContext()
            Dim li As List(Of Integer) = (From it As mcc_ArticleCategory In mdc.mcc_ArticleCategories Where it.ArticleId = ArticleId Select it.CategoryId).ToList
            If li IsNot Nothing AndAlso li.Count > 0 Then
               categories = (From c As mcc_Category In mdc.mcc_Categories Where li.Contains(c.CategoryID)).ToList
            End If
            CacheData(key, categories)
         End If
         Return categories
      End Function

      Public Sub InsertCategory(ByVal title As String, ByVal importance As Integer, ByVal description As String, ByVal imageUrl As String, ByVal parentCategoryId As Integer) Implements IArticleCategoryRepository.InsertCategory
         Dim mdc As MCCDataContext = New MCCDataContext
         Dim c As New mcc_Category

         description = ConvertNullToEmptyString(description)
         imageUrl = ConvertNullToEmptyString(imageUrl)

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

         mdc.mcc_Categories.InsertOnSubmit(c)
         mdc.SubmitChanges()
         mccObject.PurgeCacheItems("articleCategory_")
      End Sub


      Public Sub UpdateCategory(ByVal CategoryId As Integer, ByVal title As String, ByVal importance As Integer, ByVal description As String, ByVal imageUrl As String, ByVal parentCategoryId As Integer) Implements IArticleCategoryRepository.UpdateCategory
         Dim mdc As MCCDataContext = New MCCDataContext
         Dim c As mcc_Category = (From t In mdc.mcc_Categories _
                                     Where t.CategoryID = CategoryId _
                                     Select t).FirstOrDefault


         description = ConvertNullToEmptyString(description)
         imageUrl = ConvertNullToEmptyString(imageUrl)

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
            mccObject.PurgeCacheItems("articleCategory_")
            mccObject.PurgeCacheItems("articleCategory_articleCategory_")
         End If
      End Sub


      Public Sub DeleteCategory(ByVal CategoryID As Integer) Implements IArticleCategoryRepository.DeleteCategory
         Dim mdc As MCCDataContext = New MCCDataContext
         Dim wrd As mcc_Category = (From t In mdc.mcc_Categories _
                                     Where t.CategoryID = CategoryID _
                                     Select t).FirstOrDefault()

         If wrd IsNot Nothing Then
            mdc.mcc_Categories.DeleteOnSubmit(wrd)
            Dim cs As System.Data.Linq.ChangeSet = mdc.GetChangeSet()
            mdc.SubmitChanges()
            mccObject.PurgeCacheItems("articleCategory_")
         End If
      End Sub

      Public Shared Sub DeleteCategories(ByVal CategoryIDs() As Integer)
         'Dim mdc As MCCDataContext = New MCCDataContext
         'Dim wrd As List(Of mcc_Category) = (From t In mdc.mcc_Categories _
         '                            Where CategoryIDs.Contains(t.CategoryID))


         'If wrd IsNot Nothing Then
         '   mdc.mcc_Categories.DeleteAllOnSubmit(wrd)
         '   Dim cs As System.Data.Linq.ChangeSet = mdc.GetChangeSet()
         '   mdc.SubmitChanges()
         '   mccObject.PurgeCacheItems("articleCategory_")
         'End If
      End Sub


      Public Sub SetDefaultImageUrl() Implements IArticleCategoryRepository.SetDefaultImageUrl
         Dim mdc As MCCDataContext = New MCCDataContext()
         For Each it As mcc_Category In mdc.mcc_Categories
            If it.ImageUrl Is Nothing Then
               it.ImageUrl = String.Empty
            End If
         Next
         mdc.SubmitChanges()
         mccObject.PurgeCacheItems("articleCategory_")
      End Sub

      Protected Shared Sub CacheData(ByVal key As String, ByVal data As Object)
         If data IsNot Nothing Then
            mccObject.Cache.Insert(key, data, Nothing, DateTime.Now.AddSeconds(3600), TimeSpan.Zero)
         End If
      End Sub

   End Class
End Namespace