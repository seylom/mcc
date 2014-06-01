Imports MCC.Services
Imports MCC.Data
Imports Webdiyer.WebControls.Mvc
Public Class imageListViewModel
   Inherits baseViewModel

   Public Sub New()
      Me.New(Mode.List)
   End Sub


   Public Sub New(ByVal viewmd As Mode)
      _ViewMode = viewmd
   End Sub

   ''Private _imageservice As IImageService
   ''Public Sub New(ByVal imageserv As IImageService)
   ''   Me.New(0, 30, New ImageService())
   ''End Sub

   ' ''Public Sub New(ByVal pageIndex As Integer, ByVal pageSize As Integer)
   ' ''   Me.New(pageIndex, pageSize, New ImageService())
   ' ''End Sub

   ''Public Sub New(ByVal pageIndex As Integer, ByVal pageSize As Integer,Mode As  ByVal imageSrvr As IImageService)
   ''   _pageIndex = pageIndex
   ''   _pageSize = pageSize
   ''   _imageservice = imageSrvr

   ''   _images = _imageservice.GetImages(pageIndex, pageSize)
   ''End Sub

   Private _images As PagedList(Of SimpleImage)
   Public Property Images() As PagedList(Of SimpleImage)
      Get
         Return _images
      End Get
      Set(ByVal value As PagedList(Of SimpleImage))
         _images = value
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


   Private _ViewMode As Mode = Mode.List
   Public Property ViewMode() As Mode
      Get
         Return _ViewMode
      End Get
      Set(ByVal value As Mode)
         _ViewMode = value
      End Set
   End Property



End Class
