Imports MCC.Data
Imports MCC.Services
Imports Webdiyer.WebControls.Mvc
Public Class TipsViewData
   Inherits baseViewModel

   Private _adviceservice As IAdviceService
   Private _advicecategoryservice As IAdviceCategoryService

   Public Sub New(ByVal adviceserv As IAdviceService, ByVal Advicecategorysrvr As IAdviceCategoryService, ByVal pageIndex As Integer)
      _adviceservice = adviceserv
      _advicecategoryservice = Advicecategorysrvr
      _page = pageIndex
      InitData()
   End Sub

   Sub InitData()
      _tips = _adviceservice.GetAdvices(_page, 20)
      _categories = _advicecategoryservice.GetCategories(0, 20)
   End Sub

   Private _tips As PagedList(Of Advice)
   Public Property Tips() As PagedList(Of Advice)
      Get
         Return _tips
      End Get
      Set(ByVal value As PagedList(Of Advice))
         _tips = value
      End Set
   End Property

   Private _categories As PagedList(Of AdviceCategory)
   Public Property categories() As PagedList(Of AdviceCategory)
      Get
         Return _categories
      End Get
      Set(ByVal value As PagedList(Of AdviceCategory))
         _categories = value
      End Set
   End Property


   Private _page As Integer = 0
   Public Property Page() As Integer
      Get
         Return _page
      End Get
      Set(ByVal value As Integer)
         _page = value
      End Set
   End Property


End Class
