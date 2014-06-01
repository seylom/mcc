Imports MCC.Data
Imports MCC.Services

Public Class AdminDashboardViewData


   Private _articlesrvr As IArticleService
   Private _commentsrvr As IArticleCommentService
   Private _usersrvr As IUserService
   Private _questionsrvr As IUserQuestionService

   Public Sub New()
      Me.New(New ArticleService, New ArticleCommentService, New UserService, New UserQuestionService)
   End Sub

   Public Sub New(ByVal articlesrvr As IArticleService,
                  ByVal commentsrvr As IArticleCommentService,
                  ByVal usersrvr As IUserService,
                  ByVal questionsrvr As IUserQuestionService)

      _articles = articlesrvr.GetArticles(0, 5).ToList
      _comments = commentsrvr.GetArticleComments(0, 5).ToList
      _users = usersrvr.GetUnapprovedUsers(0, 5).ToList
      _questions = questionsrvr.GetUserQuestions(0, 5).ToList
   End Sub


   Private _comments As IList(Of ArticleComment)
   Public Property Comments() As IList(Of ArticleComment)
      Get
         Return _comments
      End Get
      Set(ByVal value As IList(Of ArticleComment))
         _comments = value
      End Set
   End Property



   Private _questions As IList(Of UserQuestion)
   Public Property Questions() As IList(Of UserQuestion)
      Get
         Return _questions
      End Get
      Set(ByVal value As IList(Of UserQuestion))
         _questions = value
      End Set
   End Property


   Private _articles As IList(Of Article)
   Public Property Articles() As IList(Of Article)
      Get
         Return _articles
      End Get
      Set(ByVal value As IList(Of Article))
         _articles = value
      End Set
   End Property

   Private _users As IList(Of SiteUser)
   Public Property Users() As IList(Of SiteUser)
      Get
         Return _users
      End Get
      Set(ByVal value As IList(Of SiteUser))
         _users = value
      End Set
   End Property


   Private _notifications As IList(Of Notification)
   Public Property Notifications() As IList(Of Notification)
      Get
         Return _notifications
      End Get
      Set(ByVal value As IList(Of Notification))
         _notifications = value
      End Set
   End Property

End Class
