Namespace MCC.Areas.Site
    Public Class SiteAreaRegistration
        Inherits AreaRegistration

        Public Overrides ReadOnly Property AreaName() As String
            Get
                Return "Site"
            End Get
        End Property

        Public Overrides Sub RegisterArea(ByVal context As System.Web.Mvc.AreaRegistrationContext)
         'context.MapRoute( _
         '    "Site_default", _
         '   "Site/{controller}/{action}/{id}", _
         '    New With {.action = "Index", .id = UrlParameter.Optional} _
         ')

         context.MapRoute("tips", "tips", New With {.controller = "Tip", .action = "Index"})
         context.MapRoute("tipsvote", "tips/{id}/vote", New With {.controller = "Tip", .action = "Vote"}, New With {.id = "[0-9]+"})
         context.MapRoute("tipsvotevaluew", "tips/{id}/getvotevalues", New With {.controller = "Tip", .action = "GetVoteValues"}, New With {.id = "[0-9]+"})

         context.MapRoute("videos", "videos/", New With {.controller = "Video", .action = "Index"})
         context.MapRoute("videosgetvideobyid", "videos/{id}/GetVideoById", New With {.controller = "Video", .action = "GetVideoById"})
         context.MapRoute("ShowVideo", "videos/{id}/{slug}", New With {.controller = "Video", .action = "ShowVideo"}, New With {.id = "[0-9]+", .slug = "[a-zA-Z0-9\-]*"})

         context.MapRoute("showauthor", "authors/{username}", New With {.controller = "Authors", .action = "ShowAuthor"}, _
                        New With {.username = "[a-zA-Z0-9_]*"})

         context.MapRoute("AcceptRejectAnswer", "asv/questions/{answerId}/AcceptRejectAnswer/", New With {.controller = "Ask", .action = "AcceptRejectAnswer"})
         context.MapRoute("CommentQuestion", "asv/questions/{id}/CommentQuestion/", New With {.controller = "Ask", .action = "CommentQuestion"}, New With {.id = "[0-9]+"})
         context.MapRoute("CommentAnswer", "asv/questions/{id}/CommentAnswer", New With {.controller = "Ask", .action = "CommentAnswer"}, New With {.id = "[0-9]+"})
         context.MapRoute("VoteQuestionUp", "asv/questions/{id}/VoteQuestionUp", New With {.controller = "Ask", .action = "VoteQuestionUp"}, New With {.id = "[0-9]+"})
         context.MapRoute("VoteQuestionDown", "asv/questions/{id}/VoteQuestionDown", New With {.controller = "Ask", .action = "VoteQuestionDown"}, New With {.id = "[0-9]+"})
         context.MapRoute("VoteAnswerUp", "asv/questions/{id}/VoteAnswerUp", New With {.controller = "Ask", .action = "VoteAnswerUp"}, New With {.id = "[0-9]+"})
         context.MapRoute("VoteAnswerDown", "asv/questions/{id}/VoteAnswerDown", New With {.controller = "Ask", .action = "VoteAnswerDown"}, New With {.id = "[0-9]+"})
         context.MapRoute("Follow", "asv/questions/{id}/Follow", New With {.controller = "Ask", .action = "Follow"}, New With {.id = "[0-9]+"})

         context.MapRoute("postanswer", "questions/{id}/postanswer", New With {.controller = "Ask", .action = "PostAnswer"}, New With {.id = "[0-9]+"})
         context.MapRoute("questions", "questions", New With {.controller = "Ask", .action = "Index"})
         context.MapRoute("viewquestion", "questions/{id}/{slug}", New With {.controller = "Ask", .action = "ViewQuestion"}, New With {.id = "[0-9]+", .slug = "[a-zA-Z0-9\-]*"})
         context.MapRoute("editquestion", "questions/editquestion/{id}", New With {.controller = "Ask", .action = "EditQuestion"}, New With {.id = "[0-9]+"})
         context.MapRoute("editanswer", "questions/editanswer/{id}", New With {.controller = "Ask", .action = "EditAnswer"}, New With {.id = "[0-9]+"})
         context.MapRoute("askquestion", "questions/ask", New With {.controller = "Ask", .action = "AskQuestion"})

         'routes.Add(New DomainRoute("ask.middleclasscrunch.com", "", New With {.controller = "Ask", .action = "Index"}))
         'routes.Add(New DomainRoute("ask.middleclasscrunch.com", "ask", New With {.controller = "Ask", .action = "AskQuestion"}))
         'routes.Add(New DomainRoute("ask.middleclasscrunch.com", "{id}/postanswer", New With {.controller = "Ask", .action = "PostAnswer"}, New With {.id = "[0-9]+"}))
         'routes.Add(New DomainRoute("ask.middleclasscrunch.com", "{id}/{slug}", New With {.controller = "Ask", .action = "ViewQuestion"}, New With {.id = "[0-9]+", .slug = "[a-zA-Z0-9\-]*"}))
         'routes.Add(New DomainRoute("ask.middleclasscrunch.com", "editquestion/{id}", New With {.controller = "Ask", .action = "EditQuestion"}, New With {.id = "[0-9]+"}))
         'routes.Add(New DomainRoute("ask.middleclasscrunch.com", "editanswer/{id}", New With {.controller = "Ask", .action = "EditAnswer"}, New With {.id = "[0-9]+"}))

         context.MapRoute("printarticle", "articles/{id}/{slug}/print_page", New With {.controller = "Article", .action = "Print"}, New With {.id = "[0-9]+", .slug = "[a-zA-Z0-9\-]*"})
         context.MapRoute("ArticleComments", "articles/{id}/{slug}/Comments_page", New With {.controller = "Article", .action = "ArticleComments"}, New With {.id = "[0-9]+", .slug = "[a-zA-Z0-9\-]*"})


         context.MapRoute("ArticlesIndex", "articles", New With {.controller = "Article", .action = "Index"})
         context.MapRoute("Articlesrate", "articles/ratearticle", New With {.controller = "Article", .action = "RateArticle"})
         context.MapRoute("ArticlesTopicsIndex", "articles/topics/", New With {.controller = "Article", .action = "ArticleCategories"}, _
             New With {.slug = "[a-zA-Z0-9\-]*"})
         context.MapRoute("ArticlesByTopics", "articles/topics/{slug}", New With {.controller = "Article", .action = "ArticlesByCategory", .slug = ""}, _
               New With {.slug = "[a-zA-Z0-9\-]*"})
         context.MapRoute("ArticleView2", "articles/{id}/{slug}", New With {.controller = "Article", .action = "ShowArticle"}, New With {.id = "[0-9]+", .slug = "[a-zA-Z0-9\-]*"})
         context.MapRoute("ArticleView", "articles/{slug}", New With {.controller = "Article", .action = "ViewArticle"}, New With {.slug = "[a-zA-Z0-9\-]*"})

         context.MapRoute("Profile", "profile", New With {.controller = "User", .action = "EditProfile"})
         context.MapRoute("ViewProfile", "users/{username}", New With {.controller = "User", .action = "ViewProfile"}, New With {.username = "[a-zA-Z0-9_]+"})
         context.MapRoute("Profilequestions", "users/{username}/userquestions", New With {.controller = "User", .action = "UserQuestions"}, New With {.username = "[a-zA-Z0-9_]+"})
         context.MapRoute("Profileanswers", "users/{username}/useranswers", New With {.controller = "User", .action = "UserAnswers"}, New With {.username = "[a-zA-Z0-9_]+"})

         context.MapRoute("Feedback", "feedback", New With {.controller = "Feedback", .action = "Index"})

         context.MapRoute("Login", "login", New With {.controller = "Account", .action = "LogOn"})
         context.MapRoute("OpenIdLogin", "OpenIdlogin", New With {.controller = "Account", .action = "AuthenticateWithOpenId"})
         context.MapRoute("LogOff", "logout", New With {.controller = "Account", .action = "LogOff"})
         context.MapRoute("Register", "signup", New With {.controller = "Account", .action = "Register"})
         context.MapRoute("ResetPassword", "resetpassword", New With {.controller = "Account", .action = "ResetPassword"})
         context.MapRoute("AccessDenied", "accessdenied", New With {.controller = "General", .action = "AccessDenied"})
         context.MapRoute("Error", "error", New With {.controller = "General", .action = "ErrorMessage"})


         context.MapRoute("Activation", "activation", New With {.controller = "Account", .action = "Activation"})
         context.MapRoute("ActivationReq", "activationreq", New With {.controller = "Account", .action = "ActivationReq"})
         context.MapRoute("NonActive", "nonactive", New With {.controller = "Account", .action = "NonActiveAccount"})
         context.MapRoute("Help", "Help", New With {.controller = "General", .action = "Help"})


         context.MapRoute("termsofuse", "termsofuse", New With {.controller = "General", .action = "TermsOfUse"})
         context.MapRoute("Sitemap", "sitemap", New With {.controller = "General", .action = "Sitemap"})
         context.MapRoute("SubmitContent", "submitcontent", New With {.controller = "General", .action = "SubmitContent"})
         context.MapRoute("ReportIssues", "reportissues", New With {.controller = "General", .action = "ReportIssues"})
         context.MapRoute("ContactUs", "Contact", New With {.controller = "General", .action = "ContactUs"})
         context.MapRoute("About", "about", New With {.controller = "General", .action = "About"})
         context.MapRoute("Search", "search", New With {.controller = "Search", .action = "Index"})

         context.MapRoute("IronMan2", "ironman2", New With {.controller = "Ad", .action = "ShowIronMan2Ad"})

         context.MapRoute("MainPage", "", New With {.controller = "Home", .action = "Index"})

        End Sub
    End Class
End Namespace

