Imports MCC.Data
Imports MCC.Services
Imports Webdiyer.WebControls.Mvc
Public Class AdminVideosViewModel
   Inherits baseViewModel

   Private _videoservice As IVideoService


   Public Sub New()
      Me.New(0, 30, New VideoService())
   End Sub

   Public Sub New(ByVal pageIndex As Integer, ByVal pageSize As Integer)
      Me.New(pageIndex, pageSize, New VideoService())
   End Sub

   Public Sub New(ByVal pageIndex As Integer, ByVal pageSize As Integer, ByVal videoSrvr As IVideoService)
      _pageIndex = pageIndex
      _pageSize = pageSize
      _videoservice = videoSrvr

      _videos = _videoservice.GetVideos(0, 30)
   End Sub

   Private _videos As PagedList(Of Video)
   Public Property Videos() As PagedList(Of Video)
      Get
         Return _videos
      End Get
      Set(ByVal value As PagedList(Of Video))
         _videos = value
      End Set
   End Property


   Private _pageIndex As Integer = 0
   Public Property PageIndex() As Integer
      Get
         Return _pageIndex
      End Get
      Set(ByVal value As Integer)
         _pageIndex = value
      End Set
   End Property


   Private _pageSize As Integer = 30
   Public Property PageSize() As Integer
      Get
         Return _pageSize
      End Get
      Set(ByVal value As Integer)
         _pageSize = value
      End Set
   End Property





End Class
