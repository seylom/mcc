Imports MCC.Data
Imports MCC.Services
Imports Webdiyer.WebControls.Mvc

Public Class AdminArticleCommentsViewModel
   Inherits baseViewModel


   Private _articlesrvr As IArticleService

   Public Sub New(ByVal id As Integer)
      Me.New(New ArticleService, id, 0, 30)
   End Sub

   Public Sub New(ByVal id As Integer, ByVal pageIndex As Integer, ByVal pagesize As Integer)
      Me.New(New ArticleService, id, pageIndex, pagesize)
   End Sub

   Public Sub New(ByVal articlesrvr As IArticleService, ByVal id As Integer, ByVal pageindex As Integer, ByVal pagesize As Integer)
      _pageIndex = pageindex
      _pageSize = pagesize
      _articlesrvr = articlesrvr
      _articleId = id
   End Sub

   Private _articleId As Integer
   Public Property ArticleId() As Integer
      Get
         Return _articleId
      End Get
      Set(ByVal value As Integer)
         _articleId = value
      End Set
   End Property



   Private _comments As PagedList(Of ArticleComment)
   Public Property Comments() As PagedList(Of ArticleComment)
      Get
         _comments = _articlesrvr.GetComments(PageIndex, PageSize)
         Return _comments
      End Get
      Set(ByVal value As PagedList(Of ArticleComment))
         _comments = value
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
