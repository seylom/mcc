Imports MCC.Services
Imports MCC.Data

Public Class AdminVideoViewModel
   Inherits baseViewModel

   'Private _videoService As IVideoService
   'Private _videocategoryService As IVideoCategoryService


   'Public Sub New(ByVal Id As Integer)
   '   Me.New(Id, New VideoService(), New VideoCategoryService())
   'End Sub

   'Public Sub New(ByVal Id As Integer, ByVal videosrvr As IVideoService, ByVal videocatservr As IVideoCategoryService)
   '   _videoId = Id
   '   _videoService = videosrvr
   '   _videocategoryService = videocatservr
   'End Sub

   Private _Categories As List(Of VideoCategory)
   Public Property Categories() As List(Of VideoCategory)
      Get
         Return _Categories
      End Get
      Set(ByVal value As List(Of VideoCategory))
         _Categories = value
      End Set
   End Property


   Private _videoId As Integer
   Public Property VideoID() As Integer
      Get
         Return _videoId
      End Get
      Set(ByVal value As Integer)
         _videoId = value
      End Set
   End Property

   Private _name As String
   Public Property Name() As String
      Get
         Return _name
      End Get
      Set(ByVal value As String)
         _name = value
      End Set
   End Property

   Private _tags As String
   Public Property Tags() As String
      Get
         Return _tags
      End Get
      Set(ByVal value As String)
         _tags = value
      End Set
   End Property

   Private _addedData As DateTime
   Public Property AddedDate() As DateTime
      Get
         Return _addedData
      End Get
      Set(ByVal value As DateTime)
         _addedData = value
      End Set
   End Property

   Private _addedBy As String
   Public Property AddedBy() As String
      Get
         Return _addedBy
      End Get
      Set(ByVal value As String)
         _addedBy = value
      End Set
   End Property

   Private _title As String
   Public Property Title() As String
      Get
         Return _title
      End Get
      Set(ByVal value As String)
         _title = value
      End Set
   End Property

   Private _abstract As String
   Public Property Abstract() As String
      Get
         Return _abstract
      End Get
      Set(ByVal value As String)
         _abstract = value
      End Set
   End Property


   Private _approved As Boolean
   Public Property Approved() As Boolean
      Get
         Return _approved
      End Get
      Set(ByVal value As Boolean)
         _approved = value
      End Set
   End Property


   Private _listed As Boolean
   Public Property Listed() As Boolean
      Get
         Return _listed
      End Get
      Set(ByVal value As Boolean)
         _listed = value
      End Set
   End Property



   Private _onlyForMembers As Boolean
   Public Property OnlyForMembers() As Boolean
      Get
         Return _onlyForMembers
      End Get
      Set(ByVal value As Boolean)
         _onlyForMembers = value
      End Set
   End Property



   Private _commentsEnabled As Boolean
   Public Property CommentsEnabled() As Boolean
      Get
         Return _commentsEnabled
      End Get
      Set(ByVal value As Boolean)
         _commentsEnabled = value
      End Set
   End Property

   Private _categoryIds As List(Of Integer)
   Public Property CategoryIds() As List(Of Integer)
      Get
         Return _categoryIds
      End Get
      Set(ByVal value As List(Of Integer))
         _categoryIds = value
      End Set
   End Property


   Private _videoUrl As String
   Public Property VideoUrl() As String
      Get
         Return _videoUrl
      End Get
      Set(ByVal value As String)
         _videoUrl = value
      End Set
   End Property

   Private _videoStillUrl As String
   Public Property VideoStillUrl() As String
      Get
         Return _videoStillUrl
      End Get
      Set(ByVal value As String)
         _videoStillUrl = value
      End Set
   End Property


End Class
