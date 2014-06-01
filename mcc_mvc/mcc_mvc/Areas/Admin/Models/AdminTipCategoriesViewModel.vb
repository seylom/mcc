Imports MCC.Data
Imports MCC.Services
Imports Webdiyer.WebControls.Mvc
Public Class AdminTipCategoriesViewModel
   Inherits baseViewModel

   Private _tipcategoryservice As IAdviceCategoryService

   Public Sub New()
      Me.New(0, 30, New AdviceCategoryService())
   End Sub

   Public Sub New(ByVal pageindex As Integer, ByVal pageSize As Integer)
      Me.New(pageindex, pageSize, New AdviceCategoryService())
   End Sub

   Public Sub New(ByVal pageindex As Integer, ByVal pageSize As Integer, ByVal tipcategoryservice As IAdviceCategoryService)
      _pageIndex = pageindex
      _pageSize = pageSize
      _tipcategoryservice = tipcategoryservice

      _categories = _tipcategoryservice.GetCategories(pageindex, pageSize)
   End Sub



   Private _categories As PagedList(Of AdviceCategory)
   Public Property Categories() As PagedList(Of AdviceCategory)
      Get
         Return _categories
      End Get
      Set(ByVal value As PagedList(Of AdviceCategory))
         _categories = value
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


End Class
