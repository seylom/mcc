
Imports MCC.Data
Imports MCC.Services
Public Class ArticleViewModel
   Inherits baseViewModel

   Private _articleService As IArticleService
   Private _articleCategoryService As IArticleCategoryService
   Private _adsService As IAdService
   Private _pollService As IPollService
   Private _pollOptionService As IPollOptionService


   Private _page As Integer = 0
   Private _size As Integer = 20

    'Public Sub New(ByVal Id As Integer)
    '    Me.New(Id, New ArticleService(), New ArticleCategoryService(), New AdService(), New PollService(), New PollOptionService())
    'End Sub

   Public Sub New(ByVal Id As Integer)
      Me.New(Id, New ArticleService(), New ArticleCategoryService(), New AdService(), New PollService(), New PollOptionService())
   End Sub

   Public Sub New(ByVal Id As Integer, ByVal _IArticleSrvr As IArticleService, ByVal _IArticleCategorySrvr As IArticleCategoryService, _
                  ByVal _iadservice As IAdService, ByVal _ipollservice As IPollService, ByVal _IPollOp As IPollOptionService, _
                  ByVal page As Integer, ByVal size As Integer)

      _articleService = _IArticleSrvr
      _articleCategoryService = _IArticleCategorySrvr
      _adsService = _iadservice
      _pollService = _ipollservice
      _pollOptionService = _IPollOp
      _article = _articleService.GetArticleById(Id)

      _page = page
      _size = size
   End Sub


   Public Sub New(ByVal Id As Integer, ByVal _IArticleSrvr As IArticleService, ByVal _IArticleCategorySrvr As IArticleCategoryService, _
               ByVal _iadservice As IAdService, ByVal _ipollservice As IPollService, ByVal _IPollOp As IPollOptionService)

      _articleService = _IArticleSrvr
      _articleCategoryService = _IArticleCategorySrvr
      _adsService = _iadservice
      _pollService = _ipollservice
      _pollOptionService = _IPollOp

      _article = _articleService.GetArticleById(Id)
   End Sub

   Private _article As Article
   Public Property Article() As Article
      Get
         Return _article
      End Get
      Set(ByVal value As Article)
         _article = value
      End Set
   End Property

   Private _Ads As List(Of Ad)
   Public Property ArticleAds() As List(Of Ad)
      Get

         If _Ads Is Nothing And Article IsNot Nothing Then
            _Ads = _adsService.GetAdsByArticle(_article.ArticleID)
         End If
         Return _Ads
      End Get
      Set(ByVal value As List(Of Ad))
         _Ads = value
      End Set
   End Property

   Private _poll As Poll
   Public Property Poll() As Poll
      Get
         If _poll Is Nothing And Article IsNot Nothing Then
            _poll = _pollService.GetPollById(_article.PollId)
         End If
         Return _poll
      End Get
      Set(ByVal value As Poll)
         _poll = value
      End Set
   End Property


   Private _pollOptions As List(Of PollOption)
   Public Property PollOptions() As List(Of PollOption)
      Get
         If _pollOptions Is Nothing AndAlso _poll IsNot Nothing Then
            _pollOptions = _pollOptionService.GetPollOptionsByPollId(_poll.PollID)
         End If
         Return _pollOptions
      End Get
      Set(ByVal value As List(Of PollOption))
         _pollOptions = value
      End Set
   End Property



   Private _categories As List(Of ArticleCategory)
   Public Property Categories() As List(Of ArticleCategory)
      Get
         If _categories Is Nothing AndAlso _article IsNot Nothing Then
            _categories = _articleCategoryService.GetCategoriesByArticleId(_article.ArticleID)
         End If
         Return _categories
      End Get
      Set(ByVal value As List(Of ArticleCategory))
         _categories = value
      End Set
   End Property


   Private _commentsCount As Integer = -1
   Public Property CommentsCount() As Integer
      Get
         If _commentsCount = -1 AndAlso _article IsNot Nothing Then
            _commentsCount = _articleService.CommentsCount(_article.ArticleID)
         End If
         Return _commentsCount
      End Get
      Set(ByVal value As Integer)
         _commentsCount = value
      End Set
   End Property


   Private _tags As List(Of String)
   Public Property ArticleTags() As List(Of String)
      Get
         If _tags Is Nothing AndAlso _article IsNot Nothing Then
            _tags = _article.Tags.Split(New Char() {","}, StringSplitOptions.RemoveEmptyEntries).ToList()
         End If
         Return _tags
      End Get
      Set(ByVal value As List(Of String))
         _tags = value
      End Set
   End Property

   Private _authorInfocard As AuthorInfoCard
   Public ReadOnly Property AuthorInfo() As AuthorInfoCard
      Get
         If _authorInfocard Is Nothing And _article IsNot Nothing Then
            _authorInfocard = New AuthorInfoCard(_article.AddedBy)
         End If
         Return _authorInfocard
      End Get
   End Property

   Private _status As Status = Services.Status.Pending
   Public Property Status() As Status
      Get
         _status = CType(_articleService.GetArticleStatusValue(_article.ArticleID), Status)
         Return _status
      End Get
      Set(ByVal value As Status)
         _status = value
      End Set
   End Property

   Private _commentsModel As ArticleCommentsViewModel
   Public Property CommentsModel() As ArticleCommentsViewModel
      Get
         If _commentsModel Is Nothing Then
            _commentsModel = New ArticleCommentsViewModel(_article.ArticleID, _page, _size)
         End If
         Return _commentsModel
      End Get
      Set(ByVal value As ArticleCommentsViewModel)
         _commentsModel = value
      End Set
   End Property


End Class
