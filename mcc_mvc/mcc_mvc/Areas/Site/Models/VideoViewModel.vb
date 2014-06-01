Imports MCC.Data
Imports Webdiyer.WebControls.Mvc
Imports MCC.Services

Public Class VideoViewModel
   Inherits baseViewModel

   Private _videoservice As IVideoService
   Private _videocategoryservice As IVideoCategoryService

   Public Sub New(ByVal id As Integer)
      Me.New(id, New VideoService(), New VideoCategoryService())
   End Sub

   Public Sub New(ByVal id As Integer, ByVal videoserv As IVideoService, ByVal videocategorysrvr As IVideoCategoryService)
      _videoservice = videoserv
      _videocategoryservice = videocategorysrvr

      _videos = _videoservice.GetVideos(0, 8)
      _categories = _videocategoryservice.GetCategories(0, 10)

      If id > 0 Then
         Dim vd As Video = _videoservice.GetVideoById(id)
         If vd Is Nothing Then
            Return
         End If

         Me.VideoID = vd.VideoID
         Me.Title = vd.Title
         Me.Description = vd.Abstract
         Me.VideoUrl = vd.VideoUrl
         Me.Views = vd.ViewCount
         Me._addedDate = vd.AddedDate
      End If
   End Sub

   Private _videoId As Integer
   Public Property VideoID() As Integer
      Get
         Return _videoId
      End Get
      Set(ByVal value As Integer)
         _videoId = value
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

   Private _description As String
   Public Property Description() As String
      Get
         Return _description
      End Get
      Set(ByVal value As String)
         _description = value
      End Set
   End Property

   Private _addedDate As DateTime
   Public Property AddedDate() As DateTime
      Get
         Return _addedDate
      End Get
      Set(ByVal value As DateTime)
         _addedDate = value
      End Set
   End Property

   Private _views As Integer
   Public Property Views() As Integer
      Get
         Return _views
      End Get
      Set(ByVal value As Integer)
         _views = value
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

   Private _videos As PagedList(Of Video)
   Public Property Videos() As PagedList(Of Video)
      Get
         Return _videos
      End Get
      Set(ByVal value As PagedList(Of Video))
         _videos = value
      End Set
   End Property

   Private _categories As List(Of VideoCategory)
   Public Property Categories() As List(Of VideoCategory)
      Get
         Return _categories
      End Get
      Set(ByVal value As List(Of VideoCategory))
         _categories = value
      End Set
   End Property



End Class
