Imports MCC.Data
Imports MCC.Services
Imports Webdiyer.WebControls.Mvc
Public Class videosViewData
   Inherits baseViewModel

   Private _videoservice As IVideoService
   Private _videocategoryservice As IVideoCategoryService

   Public Sub New()
      Me.New(New VideoService(), New VideoCategoryService(), 0, 8)
   End Sub

   Public Sub New(ByVal videoserv As IVideoService, ByVal videocategorysrvr As IVideoCategoryService, ByVal page As Integer, ByVal size As Integer)
      _videoservice = videoserv
      _videocategoryservice = videocategorysrvr
      _videos = _videoservice.GetVideos(page, size)
      _categories = _videocategoryservice.GetCategories(0, 10)
   End Sub

   Private _categories As PagedList(Of VideoCategory)
   Public Property categories() As PagedList(Of VideoCategory)
      Get
         Return _categories
      End Get
      Set(ByVal value As PagedList(Of VideoCategory))
         _categories = value
      End Set
   End Property

   Private _videos As PagedList(Of Video)
   Public Property videos() As PagedList(Of Video)
      Get
         Return _videos
      End Get
      Set(ByVal value As PagedList(Of Video))
         _videos = value
      End Set
   End Property

End Class
