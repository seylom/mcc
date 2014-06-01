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
   Public Interface IArticleCategoryRepository
      Function GetCategoriesCount() As Integer
      Function GetCategories() As List(Of mcc_Category)
      Function GetMainCategories() As List(Of mcc_Category)
      Function GetCategories(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, Optional ByVal sort As String = "Title ASC") As List(Of mcc_Category)
      Function GetParentCategories() As List(Of mcc_Category)
      Function GetCategoryById(ByVal categoryId As Integer) As mcc_Category
      Function GetCategoryBySlug(ByVal slug As String) As mcc_Category
      Function GetCategoriesByArticleId(ByVal ArticleId As Integer) As List(Of mcc_Category)
      Sub InsertCategory(ByVal title As String, ByVal importance As Integer, ByVal description As String, ByVal imageUrl As String, ByVal parentCategoryId As Integer)
      Sub UpdateCategory(ByVal CategoryId As Integer, ByVal title As String, ByVal importance As Integer, ByVal description As String, ByVal imageUrl As String, ByVal parentCategoryId As Integer)
      Sub DeleteCategory(ByVal CategoryID As Integer)
      Sub SetDefaultImageUrl()
   End Interface
End Namespace
