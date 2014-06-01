Imports MCC.Data
Imports MCC.Services
Imports Webdiyer.WebControls.Mvc
Public Class AdminArticleslistViewModel
   Inherits baseViewModel


   Private _articleService As IArticleService

   Public Sub New()
      Me.New(0, 30, New ArticleService)
   End Sub

   Public Sub New(ByVal pageIndex As Integer, ByVal pageSize As Integer)
      Me.New(pageIndex, pageSize, New ArticleService)
   End Sub

   Public Sub New(ByVal pageIndex As Integer, ByVal pageSize As Integer, ByVal articleServ As IArticleService)
      _pageIndex = pageIndex
      _pageSize = pageSize
      _articleService = articleServ

      _articles = _articleService.GetArticles(False, pageIndex, pageSize)

      _pendingCount = _articleService.GetArticlesCountByStatus(Status.Pending)
      _verifiedCount = _articleService.GetArticlesCountByStatus(Status.Verified)
      _acceptedCount = _articleService.GetArticlesCountByStatus(Status.Accepted)
      _rejectedCount = _articleService.GetArticlesCountByStatus(Status.Rejected)
      _approvedCount = _articleService.GetArticlesCountByStatus(Status.Approved)
      _quarantineCount = _articleService.GetArticlesCountByStatus(Status.Quarantine)
   End Sub


   Private _articles As PagedList(Of Article)
   Public Property Articles() As PagedList(Of Article)
      Get
         Return _articles
      End Get
      Set(ByVal value As PagedList(Of Article))
         _articles = value
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



   Private _AllCount As Integer
   Public Property AllCount() As Integer
      Get
         Return _AllCount
      End Get
      Set(ByVal value As Integer)
         _AllCount = value
      End Set
   End Property



   Private _pendingCount As String
   Public Property PendingCount() As String
      Get
         Return _pendingCOunt
      End Get
      Set(ByVal value As String)
         _pendingCOunt = value
      End Set
   End Property



   Private _acceptedCount As Integer
   Public Property AcceptedCount() As Integer
      Get
         Return _acceptedCount
      End Get
      Set(ByVal value As Integer)
         _acceptedCount = value
      End Set
   End Property



   Private _verifiedCount As Integer
   Public Property VerifiedCount() As Integer
      Get
         Return _verifiedCount
      End Get
      Set(ByVal value As Integer)
         _verifiedCount = value
      End Set
   End Property




   Private _rejectedCount As Integer
   Public Property RejectedCount() As Integer
      Get
         Return _rejectedCount
      End Get
      Set(ByVal value As Integer)
         _rejectedCount = value
      End Set
   End Property


   Private _quarantineCount As Integer
   Public Property QuarantineCount() As Integer
      Get
         Return _quarantineCount
      End Get
      Set(ByVal value As Integer)
         _quarantineCount = value
      End Set
   End Property



   Private _approvedCount As Integer
   Public Property ApprovedCount() As Integer
      Get
         Return _approvedCount
      End Get
      Set(ByVal value As Integer)
         _approvedCount = value
      End Set
   End Property



End Class
