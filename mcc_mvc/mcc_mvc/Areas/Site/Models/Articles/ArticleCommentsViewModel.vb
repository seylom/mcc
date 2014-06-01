Imports MCC.Data
Imports MCC.Services
Imports Webdiyer.WebControls.Mvc

Public Class ArticleCommentsViewModel
   Inherits baseViewModel

   Private _articleSrvr As IArticleService
   Private _articleCommentSrvr As IArticleCommentService

   Public Sub New(ByVal id As Integer)
      Me.New(id, New ArticleService, New ArticleCommentService(), 0, 30)
   End Sub

   Public Sub New(ByVal id As Integer, ByVal pageIndex As Integer, ByVal pagesize As Integer)
      Me.New(id, New ArticleService, New ArticleCommentService(), pageIndex, pagesize)
   End Sub

    Public Sub New(ByVal id As Integer, ByVal articleService As IArticleService, _
                  ByVal commentService As IArticleCommentService, ByVal pageIndex As Integer, ByVal size As Integer)
        _articleSrvr = articleService
        _articleCommentSrvr = commentService
        _Article = _articleSrvr.GetArticleById(id)
        pageIndex = pageIndex
        PageSize = PageSize
        InitList()
    End Sub


   Sub InitList()
      If _Article IsNot Nothing Then
         _Comments = _articleCommentSrvr.GetArticleComments(_Article.ArticleID, _pageIndex, _pageSize)
      End If
   End Sub

   Private _Article As Article
   Public ReadOnly Property MainArticle() As Article
      Get
         Return _Article
      End Get
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

   Private _Comments As PagedList(Of ArticleComment)
   Public Property Comments() As PagedList(Of ArticleComment)
      Get
         Return _Comments
      End Get
      Set(ByVal value As PagedList(Of ArticleComment))
         _Comments = value
      End Set
   End Property

End Class
