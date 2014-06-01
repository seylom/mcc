Imports MCC.Data
Imports MCC.Services
Imports Webdiyer.WebControls.Mvc
Public Class AdminVideoCategoriesViewModel
   Inherits baseViewModel

   'Private _videocategoyservice As IVideoCategoryService

   'Public Sub New()
   '   Me.New(0, 30, New VideoCategoryService())
   'End Sub

   'Public Sub New(ByVal pageIndex As Integer, ByVal pageSize As Integer)

   '   Me.New(pageIndex, pageSize, New VideoCategoryService())
   'End Sub

   'Public Sub New(ByVal pageIndex As Integer, ByVal pageSize As Integer, ByVal videocatserv As IVideoCategoryService)

   '   _pageIndex = pageIndex
   '   _pageSize = pageSize
   '   _videocategoyservice = videocatserv
   '   _categories = _videocategoyservice.GetCategories(pageIndex, pageSize)

   '   _parentCategories = _videocategoyservice.GetParentCategories.ToSelectList(Function(p) p.Title, Function(p) p.CategoryID, "- please select a parent category ")
   'End Sub


   Private _categories As PagedList(Of VideoCategory)
   Public Property Categories() As PagedList(Of VideoCategory)
      Get
         Return _categories
      End Get
      Set(ByVal value As PagedList(Of VideoCategory))
         _categories = value
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



   Private _parentCategories As List(Of SelectListItem)
   Public Property ParentCategories() As List(Of SelectListItem)
      Get
         Return _parentCategories
      End Get
      Set(ByVal value As List(Of SelectListItem))
         _parentCategories = value
      End Set
   End Property



End Class
