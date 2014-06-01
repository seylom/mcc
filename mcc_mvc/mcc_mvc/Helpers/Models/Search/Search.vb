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
Imports MCC.Data

Namespace Searches
   Public Class Search

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

      Private _title As String = ""
      Public Property Title() As String
         Get
            Return _title
         End Get
         Set(ByVal value As String)
            _title = value
         End Set
      End Property

      Private _abstract As String = ""
      Public Property Abstract() As String
         Get
            Return _abstract
         End Get
         Set(ByVal value As String)
            _abstract = value
         End Set
      End Property

      Private _body As String = Nothing
      Public Property Body() As String
         Get
            Return _body
         End Get
         Set(ByVal value As String)
            _body = value
         End Set
      End Property

      Private _releaseDate As DateTime = DateTime.Now
      Public Property ReleaseDate() As DateTime
         Get
            Return _releaseDate
         End Get
         Set(ByVal value As DateTime)
            _releaseDate = value
         End Set
      End Property

      Private _expireDate As DateTime = DateTime.MaxValue
      Public Property ExpireDate() As DateTime
         Get
            Return _expireDate
         End Get
         Set(ByVal value As DateTime)
            _expireDate = value
         End Set
      End Property

      Private _tags As String = ""
      Public Property Tags() As String
         Get
            Return _tags
         End Get
         Private Set(ByVal value As String)
            _tags = value
         End Set
      End Property


      Private _url As String = ""
      Public Property Url() As String
         Get
            Return _url
         End Get
         Private Set(ByVal value As String)
            _url = value
         End Set
      End Property

      Private _parentUrl As String = ""
      Public Property ParentUrl() As String
         Get
            Return _parentUrl
         End Get
         Private Set(ByVal value As String)
            _parentUrl = value
         End Set
      End Property

      Private _relevance As Integer = 0
      Public Property Relevance() As Integer
         Get
            Return _relevance
         End Get
         Private Set(ByVal value As Integer)
            _relevance = value
         End Set
      End Property

      Private _resultType As SearchLocation
      Public Property ResultType() As SearchLocation
         Get
            Return _resultType
         End Get
         Private Set(ByVal value As SearchLocation)
            _resultType = value
         End Set
      End Property

      Private _iconClassName As String = ""
      Public Property IconClassName() As String
         Get
            Return _iconClassName
         End Get
         Private Set(ByVal value As String)
            _iconClassName = value
         End Set
      End Property


      Public Sub New()

      End Sub


      Public Sub New(ByVal id As Integer, ByVal addedDate As DateTime, ByVal addedBy As String, ByVal title As String, _
       ByVal artabstract As String, ByVal body As String, ByVal releaseDate As DateTime, ByVal expireDate As DateTime, _
               ByVal tags As String, ByVal url As String, ByVal parentUrl As String, ByVal relevance As Integer, _
                     ByVal resultType As SearchLocation, ByVal className As String)
         Me.ID = id
         Me.AddedDate = addedDate
         Me.AddedBy = addedBy
         Me.Title = routines.Encode(title)
         If artabstract.Length > 200 Then
            artabstract = artabstract.Substring(0, 200) & " ..."
         End If
         Me.Abstract = mccHelpers.RemoveHTMLTags(artabstract, 0)
         'Me.Body = MCC.mccHelpers.RemoveHTMLTags(body, 0)
         Me.ReleaseDate = releaseDate
         Me.ExpireDate = expireDate
         Me.Tags = tags
         Me.Url = url
         Me.ParentUrl = parentUrl
         Me.Relevance = relevance
         Me.ResultType = resultType
         Me.IconClassName = className
      End Sub

   End Class
End Namespace