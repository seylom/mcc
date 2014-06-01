' Note: For instructions on enabling IIS6 or IIS7 classic mode, 
' visit http://go.microsoft.com/?LinkId=9394802

Imports MCC.MvcDomainRouting.Code

Public Class MvcApplication
   Inherits System.Web.HttpApplication

   Shared Sub RegisterRoutes(ByVal routes As RouteCollection)
      routes.IgnoreRoute("{resource}.axd/{*pathInfo}")
      routes.IgnoreRoute("BlogEngine")
      routes.IgnoreRoute("Blog")
      'routes.Add(New SubdomainRoute)

      ' MapRoute takes the following parameters, in order:
      ' (1) Route name
      ' (2) URL with parameters
      ' (3) Parameter defaults

      'routes.MapRoute("admin", "admin", New With {.controller = "adminHome", .action = "Index"})
      'routes.MapRoute("adminNotifications", "admin/notifications", New With {.controller = "adminHome", .action = "Notifications"})
      'routes.MapRoute("adminTasks", "admin/tasks", New With {.controller = "adminHome", .action = "PendingTasks"})

      'routes.MapRoute("adminEditUser", "admin/users/edituser/{username}", New With {.controller = "UserAdmin", .action = "EditUser"}, New With {.username = "[a-zA-Z0-9_]*"})
      'routes.MapRoute("adminIndex", "admin/users/unapprovedusers", New With {.controller = "UserAdmin", .action = "AdminIndex"})
      'routes.MapRoute("adminManageUsers", "admin/users", New With {.controller = "UserAdmin", .action = "ManageUsers"})
      'routes.MapRoute("adminManageUsersdeleteuser", "admin/users/deleteuser", New With {.controller = "UserAdmin", .action = "DeleteUser"})

      'routes.MapRoute("adminarticles", "admin/articles", New With {.controller = "ArticleAdmin", .action = "Index"})
      'routes.MapRoute("adminAddEditarticles", "admin/articles/AddEditArticle", New With {.controller = "ArticleAdmin", .action = "AddEditArticle"})
      'routes.MapRoute("adminarticleCategories", "admin/articles/categories", New With {.controller = "ArticleCategoryAdmin", .action = "Index"})
      'routes.MapRoute("admincategoriesDeletecategories", "admin/articles/categories/DeleteCategories", New With {.controller = "ArticleCategoryAdmin", .action = "DeleteCategories"})
      'routes.MapRoute("adminAddCategoryArticle", "admin/articles/categories/addcategory", New With {.controller = "ArticleAdmin", .action = "AddCategory"})
      'routes.MapRoute("adminEditCategoryArticle", "admin/articles/categories/editcategory", New With {.controller = "ArticleCategoryAdmin", .action = "EditCategory"})
      'routes.MapRoute("adminSaveCategoryarticles", "admin/articles/categories/savecategory", New With {.controller = "ArticleCategoryAdmin", .action = "SaveCategory"})
      'routes.MapRoute("adminArticleComments", "admin/articles/comments", New With {.controller = "ArticleAdmin", .action = "ArticleComments"})
      'routes.MapRoute("adminArticleDeleteComments", "admin/articles/deletecomments", New With {.controller = "ArticleAdmin", .action = "DeleteComments"})

      ''routes.MapRoute("adminEditCategoryarticles", "admin/articleCategories/editCategory", New With {.controller = "ArticleAdmin", .action = "EditCategory"})
      ''routes.MapRoute("adminSaveCategoryarticles", "admin/articleCategories/SaveCategory", New With {.controller = "ArticleCategoryAdmin", .action = "SaveCategory"})
      'routes.MapRoute("adminReviewArticles", "admin/articles/reviewArticles", New With {.controller = "ArticleAdmin", .action = "ReviewArticles"})
      'routes.MapRoute("adminPeekArticles", "admin/articles/PeekArticles/{id}", New With {.controller = "ArticleAdmin", .action = "PeekArticles"}, New With {.id = "[0-9]+"})
      'routes.MapRoute("adminDeleteArticles", "admin/articles/DeleteArticles", New With {.controller = "ArticleAdmin", .action = "DeleteArticles"})
      'routes.MapRoute("adminUpdateArticleStatus", "admin/articles/UpdateStatus/{id}/{status}", New With {.controller = "ArticleAdmin", .action = "UpdateStatus"}, New With {.id = "[0-9]+", .status = "[0-5]"})

      'routes.MapRoute("adminvideos", "admin/videos", New With {.controller = "VideoAdmin", .action = "Index"})
      'routes.MapRoute("adminvideoscategories", "admin/videos/categories", New With {.controller = "VideoAdmin", .action = "ShowCategories"})
      'routes.MapRoute("adminvideosEdit", "admin/videos/EditVideo/{id}", New With {.controller = "VideoAdmin", .action = "EditVideo"}, New With {.Id = "[0-9]+"})
      'routes.MapRoute("adminvideosUpdate", "admin/videos/UpdateVideoFile/{id}", New With {.controller = "VideoAdmin", .action = "UpdateVideoFile"}, New With {.Id = "[0-9]+"})
      'routes.MapRoute("adminvideosUpload", "admin/videos/UploadVideoFile", New With {.controller = "VideoAdmin", .action = "UploadVideoFile"})

      'routes.MapRoute("adminimages", "admin/images", New With {.controller = "ImageAdmin", .action = "Index"})
      'routes.MapRoute("adminimagesEdit", "admin/images/EditImage/", New With {.controller = "ImageAdmin", .action = "EditImage"})
      'routes.MapRoute("adminimagesSave", "admin/images/SaveImage", New With {.controller = "ImageAdmin", .action = "SaveImage"})
      'routes.MapRoute("adminimagesUpdate", "admin/images/updateimage/{Id}", New With {.controller = "ImageAdmin", .action = "UpdateImage"}, New With {.Id = "[0-9]+"})
      'routes.MapRoute("adminimagesCreateThumb", "admin/images/CreateThumbnails/{id}", New With {.controller = "ImageAdmin", .action = "CreateThumbnails"}, New With {.Id = "[0-9]+"})
      'routes.MapRoute("adminimagesupload", "admin/images/uploadimages", New With {.controller = "ImageAdmin", .action = "UploadImages"})

      'routes.MapRoute("adminads", "admin/ads", New With {.controller = "AdAdmin", .action = "Index"})
      'routes.MapRoute("adminCreateAds", "admin/ads/CreateAd", New With {.controller = "AdAdmin", .action = "CreateAd"})
      'routes.MapRoute("adminEditAds", "admin/ads/EditAd/{id}", New With {.controller = "AdAdmin", .action = "EditAd"}, New With {.id = "[0-9]+"})
      'routes.MapRoute("adminimagesdeleteimages", "admin/images/deleteimages", New With {.controller = "ImageAdmin", .action = "DeleteImages"})
      'routes.MapRoute("adminimagessuggestimages", "admin/images/suggestimages", New With {.controller = "ImageAdmin", .action = "SuggestImages"})
      'routes.MapRoute("adminimagesgetimages", "admin/images/getimages", New With {.controller = "ImageAdmin", .action = "GetImages"})
      'routes.MapRoute("adminimagesviewimage", "admin/images/viewimage/{id}", New With {.controller = "ImageAdmin", .action = "ShowImage"}, New With {.id = "[0-9]+"})

      'routes.MapRoute("adminquotes", "admin/quotes", New With {.controller = "AdAdmin", .action = "Index"})

      'routes.MapRoute("adminTickets", "admin/tickets", New With {.controller = "TicketAdmin", .action = "Index"})
      'routes.MapRoute("adminTicketsEdit", "admin/tickets/editticket/{id}", New With {.controller = "TicketAdmin", .action = "EditTicket"}, New With {.id = "[0-9]+"})
      'routes.MapRoute("adminTicketsShow", "admin/tickets/showticket/{id}", New With {.controller = "TicketAdmin", .action = "ShowTicket"}, New With {.id = "[0-9]+"})
      'routes.MapRoute("adminTicketsDelete", "admin/tickets/deleteticket/{id}", New With {.controller = "TicketAdmin", .action = "DeleteTicket"}, New With {.id = "[0-9]+"})
      'routes.MapRoute("adminTicketsCreate", "admin/tickets/createticket", New With {.controller = "TicketAdmin", .action = "CreateTicket"})
      'routes.MapRoute("adminTicketsDeleteChange", "admin/tickets/deleteticketChange/{id}", New With {.controller = "TicketAdmin", .action = "DeleteTicketChange"}, New With {.id = "[0-9]+"})

      'routes.MapRoute("adminTips", "admin/tips", New With {.controller = "TipAdmin", .action = "Index"})
      'routes.MapRoute("adminTipsEdit", "admin/tips/EditTip/{id}", New With {.controller = "TipAdmin", .action = "EditTip"}, New With {.id = "[0-9]+"})
      'routes.MapRoute("adminTipsDelete", "admin/tips/DeleteTip/{id}", New With {.controller = "TipAdmin", .action = "DeleteTip"}, New With {.id = "[0-9]+"})
      'routes.MapRoute("admintipscategories", "admin/tips/categories", New With {.controller = "TipAdmin", .action = "ShowCategories"})
      'routes.MapRoute("adminTipsCreate", "admin/tips/createtip", New With {.controller = "TipAdmin", .action = "CreateTip"})

      'routes.MapRoute("adminPolls", "admin/polls", New With {.controller = "PollAdmin", .action = "Index"})
      'routes.MapRoute("adminWikis", "admin/wikis", New With {.controller = "WikiAdmin", .action = "Index"})

      'routes.MapRoute("adminuserquestions", "admin/questions", New With {.controller = "AskAdmin", .action = "Index"})
      'routes.MapRoute("adminuserquestionsanswers", "admin/questions/{id}/answers", New With {.controller = "AskAdmin", .action = "AnswersByQuestion"}, New With {.id = "[0-9]+"})
      'routes.MapRoute("adminuserquestionsDelete", "admin/questions/DeleteQuestions", New With {.controller = "AskAdmin", .action = "DeleteQuestions"})
      'routes.MapRoute("adminuseranswersDelete", "admin/questions/DeleteAnswers", New With {.controller = "AskAdmin", .action = "DeleteAnswers"})
      'routes.MapRoute("adminuseranswers", "admin/questions/Answers", New With {.controller = "AskAdmin", .action = "Answers"})
      'routes.MapRoute("adminuseranswerscomments", "admin/questions/answerscomments", New With {.controller = "AskAdmin", .action = "AnswerComments"})
      'routes.MapRoute("adminuserquestionscomments", "admin/questions/questionscomments", New With {.controller = "AskAdmin", .action = "QuestionComments"})
      'routes.MapRoute("adminuseranswerscommentsbyanswer", "admin/questions/{id}/answerscomments", New With {.controller = "AskAdmin", .action = "AnswerCommentsByAnswer"}, New With {.id = "[0-9]+"})
      'routes.MapRoute("adminuserquestionscommentsbyquestion", "admin/questions/{id}/questionscomments", New With {.controller = "AskAdmin", .action = "QuestionCommentsbyQuestion"}, New With {.id = "[0-9]+"})
      'routes.MapRoute("adminuserquestionsDeletecomments", "admin/questions/DeleteQuestionComments", New With {.controller = "AskAdmin", .action = "DeleteQuestionComments"})
      'routes.MapRoute("adminuseranswersDeletecomments", "admin/questions/DeleteAnswerComments", New With {.controller = "AskAdmin", .action = "DeleteAnswerComments"})

      'routes.MapRoute("tips", "tips", New With {.controller = "Tip", .action = "Index"})
      'routes.MapRoute("tipsvote", "tips/{id}/vote", New With {.controller = "Tip", .action = "Vote"}, New With {.id = "[0-9]+"})
      'routes.MapRoute("tipsvotevaluew", "tips/{id}/getvotevalues", New With {.controller = "Tip", .action = "GetVoteValues"}, New With {.id = "[0-9]+"})

      'routes.MapRoute("videos", "videos/", New With {.controller = "Video", .action = "Index"})
      'routes.MapRoute("videosgetvideobyid", "videos/{id}/GetVideoById", New With {.controller = "Video", .action = "GetVideoById"})
      'routes.MapRoute("ShowVideo", "videos/{id}/{slug}", New With {.controller = "Video", .action = "ShowVideo"}, New With {.id = "[0-9]+", .slug = "[a-zA-Z0-9\-]*"})

      'routes.MapRoute("showauthor", "authors/{username}", New With {.controller = "Authors", .action = "ShowAuthor"}, _
      '               New With {.username = "[a-zA-Z0-9_]*"})

      'routes.MapRoute("AcceptRejectAnswer", "asv/questions/{answerId}/AcceptRejectAnswer/", New With {.controller = "Ask", .action = "AcceptRejectAnswer"})
      'routes.MapRoute("CommentQuestion", "asv/questions/{id}/CommentQuestion/", New With {.controller = "Ask", .action = "CommentQuestion"}, New With {.id = "[0-9]+"})
      'routes.MapRoute("CommentAnswer", "asv/questions/{id}/CommentAnswer", New With {.controller = "Ask", .action = "CommentAnswer"}, New With {.id = "[0-9]+"})
      'routes.MapRoute("VoteQuestionUp", "asv/questions/{id}/VoteQuestionUp", New With {.controller = "Ask", .action = "VoteQuestionUp"}, New With {.id = "[0-9]+"})
      'routes.MapRoute("VoteQuestionDown", "asv/questions/{id}/VoteQuestionDown", New With {.controller = "Ask", .action = "VoteQuestionDown"}, New With {.id = "[0-9]+"})
      'routes.MapRoute("VoteAnswerUp", "asv/questions/{id}/VoteAnswerUp", New With {.controller = "Ask", .action = "VoteAnswerUp"}, New With {.id = "[0-9]+"})
      'routes.MapRoute("VoteAnswerDown", "asv/questions/{id}/VoteAnswerDown", New With {.controller = "Ask", .action = "VoteAnswerDown"}, New With {.id = "[0-9]+"})
      'routes.MapRoute("Follow", "asv/questions/{id}/Follow", New With {.controller = "Ask", .action = "Follow"}, New With {.id = "[0-9]+"})

      'routes.MapRoute("postanswer", "questions/{id}/postanswer", New With {.controller = "Ask", .action = "PostAnswer"}, New With {.id = "[0-9]+"})
      'routes.MapRoute("questions", "questions", New With {.controller = "Ask", .action = "Index"})
      'routes.MapRoute("viewquestion", "questions/{id}/{slug}", New With {.controller = "Ask", .action = "ViewQuestion"}, New With {.id = "[0-9]+", .slug = "[a-zA-Z0-9\-]*"})
      'routes.MapRoute("editquestion", "questions/editquestion/{id}", New With {.controller = "Ask", .action = "EditQuestion"}, New With {.id = "[0-9]+"})
      'routes.MapRoute("editanswer", "questions/editanswer/{id}", New With {.controller = "Ask", .action = "EditAnswer"}, New With {.id = "[0-9]+"})
      'routes.MapRoute("askquestion", "questions/ask", New With {.controller = "Ask", .action = "AskQuestion"})

      ''routes.Add(New DomainRoute("ask.middleclasscrunch.com", "", New With {.controller = "Ask", .action = "Index"}))
      ''routes.Add(New DomainRoute("ask.middleclasscrunch.com", "ask", New With {.controller = "Ask", .action = "AskQuestion"}))
      ''routes.Add(New DomainRoute("ask.middleclasscrunch.com", "{id}/postanswer", New With {.controller = "Ask", .action = "PostAnswer"}, New With {.id = "[0-9]+"}))
      ''routes.Add(New DomainRoute("ask.middleclasscrunch.com", "{id}/{slug}", New With {.controller = "Ask", .action = "ViewQuestion"}, New With {.id = "[0-9]+", .slug = "[a-zA-Z0-9\-]*"}))
      ''routes.Add(New DomainRoute("ask.middleclasscrunch.com", "editquestion/{id}", New With {.controller = "Ask", .action = "EditQuestion"}, New With {.id = "[0-9]+"}))
      ''routes.Add(New DomainRoute("ask.middleclasscrunch.com", "editanswer/{id}", New With {.controller = "Ask", .action = "EditAnswer"}, New With {.id = "[0-9]+"}))

      'routes.MapRoute("printarticle", "articles/{id}/{slug}/print_page", New With {.controller = "Article", .action = "Print"}, New With {.id = "[0-9]+", .slug = "[a-zA-Z0-9\-]*"})
      'routes.MapRoute("ArticleComments", "articles/{id}/{slug}/Comments_page", New With {.controller = "Article", .action = "ArticleComments"}, New With {.id = "[0-9]+", .slug = "[a-zA-Z0-9\-]*"})


      'routes.MapRoute("ArticlesIndex", "articles", New With {.controller = "Article", .action = "Index"})
      'routes.MapRoute("Articlesrate", "articles/ratearticle", New With {.controller = "Article", .action = "RateArticle"})
      'routes.MapRoute("ArticlesTopicsIndex", "articles/topics/", New With {.controller = "Article", .action = "ArticleCategories"}, _
      '    New With {.slug = "[a-zA-Z0-9\-]*"})
      'routes.MapRoute("ArticlesByTopics", "articles/topics/{slug}", New With {.controller = "Article", .action = "ArticlesByCategory", .slug = ""}, _
      '      New With {.slug = "[a-zA-Z0-9\-]*"})
      'routes.MapRoute("ArticleView2", "articles/{id}/{slug}", New With {.controller = "Article", .action = "ShowArticle"}, New With {.id = "[0-9]+", .slug = "[a-zA-Z0-9\-]*"})
      'routes.MapRoute("ArticleView", "articles/{slug}", New With {.controller = "Article", .action = "ViewArticle"},New With {.slug = "[a-zA-Z0-9\-]*"})

      'routes.MapRoute("Profile", "profile", New With {.controller = "User", .action = "EditProfile"})
      'routes.MapRoute("ViewProfile", "users/{username}", New With {.controller = "User", .action = "ViewProfile"}, New With {.username = "[a-zA-Z0-9_]+"})
      'routes.MapRoute("Profilequestions", "users/{username}/userquestions", New With {.controller = "User", .action = "UserQuestions"}, New With {.username = "[a-zA-Z0-9_]+"})
      'routes.MapRoute("Profileanswers", "users/{username}/useranswers", New With {.controller = "User", .action = "UserAnswers"}, New With {.username = "[a-zA-Z0-9_]+"})

      'routes.MapRoute("Feedback", "feedback", New With {.controller = "Feedback", .action = "Index"})

      'routes.MapRoute("Login", "login", New With {.controller = "Account", .action = "LogOn"})
      'routes.MapRoute("LogOff", "logout", New With {.controller = "Account", .action = "LogOff"})
      'routes.MapRoute("Register", "signup", New With {.controller = "Account", .action = "Register"})
      'routes.MapRoute("ResetPassword", "resetpassword", New With {.controller = "Account", .action = "ResetPassword"})
      'routes.MapRoute("AccessDenied", "accessdenied", New With {.controller = "General", .action = "AccessDenied"})
      'routes.MapRoute("Error", "error", New With {.controller = "General", .action = "ErrorMessage"})


      'routes.MapRoute("Activation", "activation", New With {.controller = "Account", .action = "Activation"})
      'routes.MapRoute("ActivationReq", "activationreq", New With {.controller = "Account", .action = "ActivationReq"})
      'routes.MapRoute("NonActive", "nonactive", New With {.controller = "Account", .action = "NonActiveAccount"})
      'routes.MapRoute("Help", "Help", New With {.controller = "General", .action = "Help"})


      'routes.MapRoute("termsofuse", "termsofuse", New With {.controller = "General", .action = "TermsOfUse"})
      'routes.MapRoute("Sitemap", "sitemap", New With {.controller = "General", .action = "Sitemap"})
      'routes.MapRoute("SubmitContent", "submitcontent", New With {.controller = "General", .action = "SubmitContent"})
      'routes.MapRoute("ReportIssues", "reportissues", New With {.controller = "General", .action = "ReportIssues"})
      'routes.MapRoute("ContactUs", "Contact", New With {.controller = "General", .action = "ContactUs"})
      'routes.MapRoute("About", "about", New With {.controller = "General", .action = "About"})
      'routes.MapRoute("Search", "search", New With {.controller = "Search", .action = "Index"})

      'routes.MapRoute("IronMan2", "ironman2", New With {.controller = "Ad", .action = "ShowIronMan2Ad"})

      'routes.MapRoute("MainPage", "", New With {.controller = "Home", .action = "Index"})



      'routes.MapRoute("Default", "{controller}/{action}/{id}", New With {.controller = "Home", .action = "Index", .id = ""})

   End Sub

   Sub Application_Start()

      AreaRegistration.RegisterAllAreas()
      RegisterRoutes(RouteTable.Routes)
      'Route301Global.ReAssignHandler(RouteTable.Routes)
      'RouteDebug.RouteDebugger.RewriteRoutesForTesting(RouteTable.Routes)

   End Sub

End Class
