Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports Webdiyer.WebControls.Mvc

'Namespace System.Web.Mvc
Public Interface IPagedList
   Property TotalCount() As Integer
   Property PageIndex() As Integer
   Property PageSize() As Integer
   ReadOnly Property IsPreviousPage() As Boolean
   ReadOnly Property IsNextPage() As Boolean
End Interface



Public Class _PagedList(Of T)
   Inherits List(Of T)
   Implements IPagedList
   Public Sub New(ByVal source As IQueryable(Of T), ByVal index As Integer, ByVal pageSize As Integer)
      Me.TotalCount = source.Count()
      Me.PageSize = pageSize
      Me.PageIndex = index
      Me.AddRange(source.Skip(index * pageSize).Take(pageSize).ToList())
   End Sub

   Public Sub New(ByVal source As List(Of T), ByVal index As Integer, ByVal pageSize As Integer)
      Me.TotalCount = source.Count()
      Me.PageSize = pageSize
      Me.PageIndex = index
      Me.AddRange(source.Skip(index * pageSize).Take(pageSize).ToList())
   End Sub

   Private _TotalCount As Integer
   Public Property TotalCount() As Integer Implements IPagedList.TotalCount
      Get
         Return _TotalCount
      End Get
      Set(ByVal value As Integer)
         _TotalCount = value
      End Set
   End Property

   Private _PageIndex As Integer
   Public Property PageIndex() As Integer Implements IPagedList.PageIndex
      Get
         Return _PageIndex
      End Get
      Set(ByVal value As Integer)
         _PageIndex = value
      End Set
   End Property

   Private _PageSize As Integer
   Public Property PageSize() As Integer Implements IPagedList.PageSize
      Get
         Return _PageSize
      End Get
      Set(ByVal value As Integer)
         _PageSize = value
      End Set
   End Property

   Public ReadOnly Property IsPreviousPage() As Boolean Implements IPagedList.IsPreviousPage
      Get
         Return (PageIndex > 0)
      End Get
   End Property

   Public ReadOnly Property IsNextPage() As Boolean Implements IPagedList.IsNextPage
      Get
         Return (PageIndex * PageSize) <= TotalCount
      End Get
   End Property
End Class

'<HideModuleName()> _
'Public Module Pagination
'   <Runtime.CompilerServices.Extension()> _
'   Public Function ToPagedList(Of T)(ByVal source As IQueryable(Of T), ByVal index As Integer, ByVal pageSize As Integer) As PagedList(Of T)
'      Return New PagedList(Of T)(source, index, pageSize)
'   End Function

'   <Runtime.CompilerServices.Extension()> _
'   Public Function ToPagedList(Of T)(ByVal source As IQueryable(Of T), ByVal index As Integer) As PagedList(Of T)
'      Return New PagedList(Of T)(source, index, 10)
'   End Function
'End Module

