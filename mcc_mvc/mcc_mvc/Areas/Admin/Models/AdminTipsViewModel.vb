Imports MCC.Data
Imports MCC.Services
Imports Webdiyer.WebControls.Mvc
Public Class AdminTipsViewModel
   Inherits baseViewModel

   Private _tipservice As IAdviceService
   Public Sub New(ByVal advicesrvr As IAdviceService)
      Me.New(0, 30, advicesrvr)
   End Sub

   Public Sub New(ByVal pageIndex As Integer, ByVal pageSize As Integer, ByVal tipservice As IAdviceService)
      _pageIndex = pageIndex
      _pageSize = pageSize
      _tipservice = tipservice

      _Tips = _tipservice.GetAdvices(_pageIndex, _pageSize)
   End Sub


   Private _Tips As PagedList(Of Advice)
   Public Property Tips() As PagedList(Of Advice)
      Get
         Return _Tips
      End Get
      Set(ByVal value As PagedList(Of Advice))
         _Tips = value
      End Set
   End Property

   Private _pageIndex As Integer = 30
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
