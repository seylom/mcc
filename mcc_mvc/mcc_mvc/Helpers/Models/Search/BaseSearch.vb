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


Namespace Searches
   Public MustInherit Class BaseSearch
      Inherits mccObject
      Private _id As Integer = 0
      Public Property ID() As Integer
         Get
            Return _id
         End Get
         Protected Set(ByVal value As Integer)
            _id = value
         End Set
      End Property

      Private _addedDate As DateTime = DateTime.Now
      Public Property AddedDate() As DateTime
         Get
            Return _addedDate
         End Get
         Protected Set(ByVal value As DateTime)
            _addedDate = value
         End Set
      End Property

      Private _addedBy As String = ""
      Public Property AddedBy() As String
         Get
            Return _addedBy
         End Get
         Protected Set(ByVal value As String)
            _addedBy = value
         End Set
      End Property

      'Protected Shared ReadOnly Property Settings() As ArticlesElement
      '   Get
      '      Return MCCGlobals.Settings.Articles
      '   End Get
      'End Property

      '''' <summary>
      '''' Cache the input data, if caching is enabled
      '''' </summary>
      'Protected Shared Sub CacheData(ByVal key As String, ByVal data As Object)
      '   If Settings.EnableCaching AndAlso data IsNot Nothing Then
      '      mccObject.Cache.Insert(key, data, Nothing, DateTime.Now.AddSeconds(Settings.CacheDuration), TimeSpan.Zero)
      '   End If
      'End Sub
   End Class
End Namespace
