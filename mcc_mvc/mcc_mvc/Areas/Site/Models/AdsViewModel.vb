Imports MCC.Data
Imports MCC.Services
Imports Webdiyer.WebControls.Mvc
Public Class AdsViewModel
   Inherits baseViewModel

   'Private _adservice As IAdService

   'Public Sub New()
   '   'Me.New(0, 30)
   'End Sub

   'Public Sub New()
   '   Me.New(0, 30, New AdService())
   'End Sub

   'Public Sub New(ByVal pageIndex As Integer, ByVal pageSize As Integer)
   '   Me.New(pageIndex, pageSize, New AdService())
   'End Sub

   'Public Sub New(ByVal pageIndex As Integer, ByVal pageSize As Integer, ByVal adServ As IAdService)
   '   _pageIndex = pageIndex
   '   _pageSize = pageSize
   '   _adservice = adServ

   '   _ads = _adservice.GetAds(pageIndex, pageSize)
   'End Sub

   'Public Sub New(ByVal pageIndex As Integer, ByVal pageSize As Integer)
   '   _pageIndex = pageIndex
   '   _pageSize = pageSize

   '   _ads = _adservice.GetAds(pageIndex, pageSize)
   'End Sub


   Private _ads As PagedList(Of Ad)

   Public Property Ads() As PagedList(Of Ad)
      Get
         Return _ads
      End Get
      Set(ByVal value As PagedList(Of Ad))
         _ads = value
      End Set
   End Property



   Private _pageIndex As Integer
   Public Property PageIndex() As Integer
      Get
         Return _pageIndex
      End Get
      Set(ByVal value As Integer)
         _pageIndex = value
      End Set
   End Property


   Private _pageSize As Integer
   Public Property PageSize() As Integer
      Get
         Return _pageSize
      End Get
      Set(ByVal value As Integer)
         _pageSize = value
      End Set
   End Property


   Private _adType As adType
   Public Property AdItemType() As adType
      Get
         Return _adType
      End Get
      Set(ByVal value As adType)
         _adType = value
      End Set
   End Property


End Class
