Namespace MCC.Areas.Admin
    Public Class AdminAreaRegistration
        Inherits AreaRegistration

        Public Overrides ReadOnly Property AreaName() As String
            Get
                Return "Admin"
            End Get
        End Property

        Public Overrides Sub RegisterArea(ByVal context As System.Web.Mvc.AreaRegistrationContext)
      
         context.MapRoute("admin", "admin", New With {.controller = "AdminHome", .action = "Index"})
         context.MapRoute("adminNotifications", "admin/notifications", New With {.controller = "adminHome", .action = "Notifications"})
         context.MapRoute("adminTasks", "admin/tasks", New With {.controller = "adminHome", .action = "PendingTasks"})

         context.MapRoute("adminEditUser", "admin/users/edituser/{username}", New With {.controller = "UserAdmin", .action = "EditUser"}, New With {.username = "[a-zA-Z0-9_]*"})
         context.MapRoute("adminIndex", "admin/users/unapprovedusers", New With {.controller = "UserAdmin", .action = "AdminIndex"})
         context.MapRoute("adminManageUsers", "admin/users", New With {.controller = "UserAdmin", .action = "ManageUsers"})
         context.MapRoute("adminManageUsersdeleteuser", "admin/users/deleteuser", New With {.controller = "UserAdmin", .action = "DeleteUser"})

         context.MapRoute("adminarticles", "admin/articles", New With {.controller = "ArticleAdmin", .action = "Index"})
         context.MapRoute("adminAddEditarticles", "admin/articles/AddEditArticle", New With {.controller = "ArticleAdmin", .action = "AddEditArticle"})
         context.MapRoute("adminarticleCategories", "admin/articles/categories", New With {.controller = "ArticleCategoryAdmin", .action = "Index"})
         context.MapRoute("admincategoriesDeletecategories", "admin/articles/categories/DeleteCategories", New With {.controller = "ArticleCategoryAdmin", .action = "DeleteCategories"})
         context.MapRoute("adminAddCategoryArticle", "admin/articles/categories/addcategory", New With {.controller = "ArticleAdmin", .action = "AddCategory"})
         context.MapRoute("adminEditCategoryArticle", "admin/articles/categories/editcategory", New With {.controller = "ArticleCategoryAdmin", .action = "EditCategory"})
         context.MapRoute("adminSaveCategoryarticles", "admin/articles/categories/savecategory", New With {.controller = "ArticleCategoryAdmin", .action = "SaveCategory"})
         context.MapRoute("adminArticleComments", "admin/articles/comments", New With {.controller = "ArticleAdmin", .action = "ArticleComments"})
         context.MapRoute("adminArticleDeleteComments", "admin/articles/deletecomments", New With {.controller = "ArticleAdmin", .action = "DeleteComments"})

         'context.MapRoute("adminEditCategoryarticles", "admin/articleCategories/editCategory", New With {.controller = "ArticleAdmin", .action = "EditCategory"})
         'context.MapRoute("adminSaveCategoryarticles", "admin/articleCategories/SaveCategory", New With {.controller = "ArticleCategoryAdmin", .action = "SaveCategory"})
         context.MapRoute("adminReviewArticles", "admin/articles/reviewArticles", New With {.controller = "ArticleAdmin", .action = "ReviewArticles"})
         context.MapRoute("adminPeekArticles", "admin/articles/PeekArticles/{id}", New With {.controller = "ArticleAdmin", .action = "PeekArticles"}, New With {.id = "[0-9]+"})
         context.MapRoute("adminDeleteArticles", "admin/articles/DeleteArticles", New With {.controller = "ArticleAdmin", .action = "DeleteArticles"})
         context.MapRoute("adminUpdateArticleStatus", "admin/articles/UpdateStatus/{id}/{status}", New With {.controller = "ArticleAdmin", .action = "UpdateStatus"}, New With {.id = "[0-9]+", .status = "[0-5]"})

         context.MapRoute("adminvideos", "admin/videos", New With {.controller = "VideoAdmin", .action = "Index"})
         context.MapRoute("adminvideoscategories", "admin/videos/categories", New With {.controller = "VideoAdmin", .action = "ShowCategories"})
         context.MapRoute("adminvideosEdit", "admin/videos/EditVideo/{id}", New With {.controller = "VideoAdmin", .action = "EditVideo"}, New With {.Id = "[0-9]+"})
         context.MapRoute("adminvideosUpdate", "admin/videos/UpdateVideoFile/{id}", New With {.controller = "VideoAdmin", .action = "UpdateVideoFile"}, New With {.Id = "[0-9]+"})
         context.MapRoute("adminvideosUpload", "admin/videos/UploadVideoFile", New With {.controller = "VideoAdmin", .action = "UploadVideoFile"})

         context.MapRoute("adminimages", "admin/images", New With {.controller = "ImageAdmin", .action = "Index"})
         context.MapRoute("adminimagesEdit", "admin/images/EditImage/", New With {.controller = "ImageAdmin", .action = "EditImage"})
         context.MapRoute("adminimagesSave", "admin/images/SaveImage", New With {.controller = "ImageAdmin", .action = "SaveImage"})
         context.MapRoute("adminimagesUpdate", "admin/images/updateimage/{Id}", New With {.controller = "ImageAdmin", .action = "UpdateImage"}, New With {.Id = "[0-9]+"})
         context.MapRoute("adminimagesCreateThumb", "admin/images/CreateThumbnails/{id}", New With {.controller = "ImageAdmin", .action = "CreateThumbnails"}, New With {.Id = "[0-9]+"})
         context.MapRoute("adminimagesupload", "admin/images/uploadimages", New With {.controller = "ImageAdmin", .action = "UploadImages"})

         context.MapRoute("adminads", "admin/ads", New With {.controller = "AdAdmin", .action = "Index"})
         context.MapRoute("adminCreateAds", "admin/ads/CreateAd", New With {.controller = "AdAdmin", .action = "CreateAd"})
         context.MapRoute("adminEditAds", "admin/ads/EditAd/{id}", New With {.controller = "AdAdmin", .action = "EditAd"}, New With {.id = "[0-9]+"})
         context.MapRoute("adminimagesdeleteimages", "admin/images/deleteimages", New With {.controller = "ImageAdmin", .action = "DeleteImages"})
         context.MapRoute("adminimagessuggestimages", "admin/images/suggestimages", New With {.controller = "ImageAdmin", .action = "SuggestImages"})
         context.MapRoute("adminimagesgetimages", "admin/images/getimages", New With {.controller = "ImageAdmin", .action = "GetImages"})
         context.MapRoute("adminimagesviewimage", "admin/images/viewimage/{id}", New With {.controller = "ImageAdmin", .action = "ShowImage"}, New With {.id = "[0-9]+"})

         context.MapRoute("adminquotes", "admin/quotes", New With {.controller = "AdAdmin", .action = "Index"})

         context.MapRoute("adminTickets", "admin/tickets", New With {.controller = "TicketAdmin", .action = "Index"})
         context.MapRoute("adminTicketsEdit", "admin/tickets/editticket/{id}", New With {.controller = "TicketAdmin", .action = "EditTicket"}, New With {.id = "[0-9]+"})
         context.MapRoute("adminTicketsShow", "admin/tickets/showticket/{id}", New With {.controller = "TicketAdmin", .action = "ShowTicket"}, New With {.id = "[0-9]+"})
         context.MapRoute("adminTicketsDelete", "admin/tickets/deleteticket/{id}", New With {.controller = "TicketAdmin", .action = "DeleteTicket"}, New With {.id = "[0-9]+"})
         context.MapRoute("adminTicketsCreate", "admin/tickets/createticket", New With {.controller = "TicketAdmin", .action = "CreateTicket"})
         context.MapRoute("adminTicketsDeleteChange", "admin/tickets/deleteticketChange/{id}", New With {.controller = "TicketAdmin", .action = "DeleteTicketChange"}, New With {.id = "[0-9]+"})

         context.MapRoute("adminTips", "admin/tips", New With {.controller = "TipAdmin", .action = "Index"})
         context.MapRoute("adminTipsEdit", "admin/tips/EditTip/{id}", New With {.controller = "TipAdmin", .action = "EditTip"}, New With {.id = "[0-9]+"})
         context.MapRoute("adminTipsDelete", "admin/tips/DeleteTip/{id}", New With {.controller = "TipAdmin", .action = "DeleteTip"}, New With {.id = "[0-9]+"})
         context.MapRoute("admintipscategories", "admin/tips/categories", New With {.controller = "TipAdmin", .action = "ShowCategories"})
         context.MapRoute("adminTipsCreate", "admin/tips/createtip", New With {.controller = "TipAdmin", .action = "CreateTip"})

         context.MapRoute("adminPolls", "admin/polls", New With {.controller = "PollAdmin", .action = "Index"})
         context.MapRoute("adminWikis", "admin/wikis", New With {.controller = "WikiAdmin", .action = "Index"})

         context.MapRoute("adminuserquestions", "admin/questions", New With {.controller = "AskAdmin", .action = "Index"})
         context.MapRoute("adminuserquestionsanswers", "admin/questions/{id}/answers", New With {.controller = "AskAdmin", .action = "AnswersByQuestion"}, New With {.id = "[0-9]+"})
         context.MapRoute("adminuserquestionsDelete", "admin/questions/DeleteQuestions", New With {.controller = "AskAdmin", .action = "DeleteQuestions"})
         context.MapRoute("adminuseranswersDelete", "admin/questions/DeleteAnswers", New With {.controller = "AskAdmin", .action = "DeleteAnswers"})
         context.MapRoute("adminuseranswers", "admin/questions/Answers", New With {.controller = "AskAdmin", .action = "Answers"})
         context.MapRoute("adminuseranswerscomments", "admin/questions/answerscomments", New With {.controller = "AskAdmin", .action = "AnswerComments"})
         context.MapRoute("adminuserquestionscomments", "admin/questions/questionscomments", New With {.controller = "AskAdmin", .action = "QuestionComments"})
         context.MapRoute("adminuseranswerscommentsbyanswer", "admin/questions/{id}/answerscomments", New With {.controller = "AskAdmin", .action = "AnswerCommentsByAnswer"}, New With {.id = "[0-9]+"})
         context.MapRoute("adminuserquestionscommentsbyquestion", "admin/questions/{id}/questionscomments", New With {.controller = "AskAdmin", .action = "QuestionCommentsbyQuestion"}, New With {.id = "[0-9]+"})
         context.MapRoute("adminuserquestionsDeletecomments", "admin/questions/DeleteQuestionComments", New With {.controller = "AskAdmin", .action = "DeleteQuestionComments"})
         context.MapRoute("adminuseranswersDeletecomments", "admin/questions/DeleteAnswerComments", New With {.controller = "AskAdmin", .action = "DeleteAnswerComments"})

         context.MapRoute( _
          "Admin_default", _
         "Admin/{controller}/{action}/{id}", _
          New With {.action = "Index", .id = UrlParameter.Optional} _
         )

        End Sub
    End Class
End Namespace

