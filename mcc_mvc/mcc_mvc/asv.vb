Imports System.Web
Imports System.Web.Services
Imports System.Web.Script.Services
Imports System.Web.Services.Protocols
Imports MCC
Imports System.IO
Imports System.Net.Mail
Imports MCC.Services
Imports MCC.Data
Imports Webdiyer.WebControls.Mvc

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class asv
   Inherits System.Web.Services.WebService

   Private Function HasValidCredentials() As Boolean
      If HttpContext.Current.User.Identity.IsAuthenticated Then
         Return True
      End If
      Return False
   End Function

   Private Function HasAdminCredentials() As Boolean
      If HttpContext.Current.User.Identity.IsAuthenticated AndAlso User.IsInRole("Administrators") Then
         Return True
      End If
      Return False
   End Function

#Region "Article"
   <WebMethod()> _
   Public Function FetchCategories(ByVal pageIndex As Integer, ByVal pageSize As Integer) As ArticleCategoryJSON()

      Dim _articleCategoryService As IArticleCategoryService = New ArticleCategoryService
      Dim li As List(Of ArticleCategory) = _articleCategoryService.GetCategories(pageIndex, pageSize)

      Dim obj As New List(Of ArticleCategoryJSON)
      For Each it As ArticleCategory In li
         Dim ar = New ArticleCategoryJSON(it)
         ar.Description = "..." ' too long to put. i might need to strip some char before sending it.
         obj.Add(ar)
      Next

      'Return li
      Return obj.ToArray
   End Function

   '<WebMethod()> _
   'Public Function GetCategoryByID(ByVal id As String) As ArticleCategoryJSON
   '   If Not String.IsNullOrEmpty(id) Then
   '      Dim i As Integer
   '      Integer.TryParse(id, i)
   '      If id > 0 Then
   '         Dim _articleCategoryService As IArticleCategoryService = New ArticleCategoryService
   '         Dim cat As ArticleCategory = _articleCategoryService.GetCategoryById(id)
   '         If cat IsNot Nothing Then
   '            Dim acj As New ArticleCategoryJSON(cat)
   '            Return acj
   '         End If
   '      End If
   '   End If
   '   Return Nothing
   'End Function

   '<WebMethod(), AcceptVerbs(HttpVerbs.Post)> _
   '  Public Function saveCategory(ByVal category As ArticleCategoryJSON) As Boolean
   '   If category IsNot Nothing AndAlso HasValidCredentials() Then

   '      Dim i As Integer
   '      Integer.TryParse(category.CategoryID, i)
   '      If i > 0 Then
   '         Dim _articleCategoryService As IArticleCategoryService = New ArticleCategoryService
   '         Dim cat As ArticleCategory = _articleCategoryService.GetCategoryById(i)
   '         If cat IsNot Nothing Then
   '            _articleCategoryService.UpdateCategory(New ArticleCategory With { _
   '                                                   .CategoryID = category.CategoryID, _
   '                                                  .Title = category.Title, _
   '                                                   .Importance = category.Importance, _
   '                                                    .Description = category.Description, _
   '                                                    .ImageUrl = category.ImageUrl, _
   '                                                    .ParentCategoryID = category.ParentCategoryID})
   '            Return True
   '         End If
   '      Else
   '         If Not String.IsNullOrEmpty(category.Title) Then
   '            Dim _articleCategoryService As IArticleCategoryService = New ArticleCategoryService
   '            _articleCategoryService.InsertCategory(New ArticleCategory With { _
   '                                                  .Title = category.Title, _
   '                                                   .Importance = category.Importance, _
   '                                                    .Description = category.Description, _
   '                                                    .ImageUrl = category.ImageUrl, _
   '                                                    .ParentCategoryID = category.ParentCategoryID})
   '         End If
   '      End If
   '   End If
   '   Return False
   'End Function

   <WebMethod()> _
   Public Function DeleteCategories(ByVal Ids() As Integer) As Boolean

      If Ids.Length > 0 AndAlso HasAdminCredentials() Then
         Dim srvr As New ArticleCategoryService
         srvr.DeleteCategories(Ids)
      End If
      Return False
   End Function

   '<WebMethod()> _
   ' Public Function DeleteArticles(ByVal Ids() As Integer) As Boolean

   '   If Ids.Length > 0 AndAlso HasAdminCredentials() Then
   '      Dim _articleService As IArticleService = New ArticleService()
   '      _articleService.DeleteArticles(Ids)
   '   End If
   '   Return False
   'End Function

   <WebMethod()> _
    Public Function ShareEmail(ByVal source As String, ByVal dest As String, ByVal name As String, ByVal url As String, ByVal title As String, ByVal body As String) As String
      ' 1|success text or  0|error text
      ' 0: error , 1 success
      If Not routines.ValidateEmail(source.Trim) Then
         Return "0|Invalid Email Format for the sender"
      End If

      If Not routines.ValidateEmail(dest.Trim) Then
         Return "0|Invalid Email(s) Format for one or more recipient. Please check the emails provided"
      End If


      Dim smtpc As SmtpClient = New SmtpClient()

        Dim strEmail() As String = dest.Split(New Char() {CChar(",c")}, StringSplitOptions.RemoveEmptyEntries)

      Dim replacements As ListDictionary = New ListDictionary
      replacements.Add("<%Name%>", name)
      replacements.Add("<%From%>", source)
      replacements.Add("<%Title%>", title)
      replacements.Add("<%Message%>", body)

      If strEmail.Length > 0 Then
         'For Each it As String In strEmail

         Dim mbm As New MailDefinition
         mbm.BodyFileName = Server.MapPath("~/mcc/misc/shareArticlemail.txt")
         mbm.From = source
         mbm.Subject = title

         Dim mm As MailMessage = mbm.CreateMailMessage(dest, replacements, New System.Web.UI.Control)

         'Dim mm As MailMessage = New MailMessage
         'mm.From = New MailAddress(source.Trim, name)
         'mm.To.Add(it.Trim())
         'mm.Body = txtMessage.Text

         Try
            smtpc.Send(mm)

            'Me.cuPanel.Visible = False
            'Me.lblConfirm.Visible = True
         Catch ex As Exception
            Return "0|Unable to process your request - a problem occured during the Send"
            'Me.cuPanel.Visible = False
            'Me.lblConfirm.Visible = False
            'Me.lblError.Visible = True
         End Try
         'Next
      End If

      Return "1|Message sent succesfully!"
   End Function


#End Region

#Region "Images"

   <WebMethod()> _
   Public Function FetchImages(ByVal pageIndex As Integer, ByVal pageSize As Integer) As ImageJSON()
      Dim _imgsrvr As New ImageService
      Dim li As PagedList(Of SimpleImage) = _imgsrvr.GetImages(pageIndex, pageSize)

      Dim obj As New List(Of ImageJSON)
      For Each it As SimpleImage In li
         Dim ar = New ImageJSON(it)
         ar.Description = "..." ' too long to put. i might need to strip some char before sending it.
         ar.ImageUrl = Configs.Paths.CdnRoot & "/imagethumb.ashx?img=" & (Configs.Paths.CdnImages & ar.ImageUrl) & "&w=150&h=80"
         obj.Add(ar)
      Next

      'Return li
      Return obj.ToArray
   End Function

   <WebMethod()> _
    Public Function saveImage(ByVal image As ImageJSON) As Boolean

      If image IsNot Nothing AndAlso HasValidCredentials() Then
         Dim i As Integer
         Integer.TryParse(image.ImageID, i)
         If i > 0 Then
            Dim imrepo As New ImageRepository
            imrepo.UpdateImage(New SimpleImage With { _
                .ImageID = image.ImageID, _
                .Description = image.Description, _
                .CreditsName = image.CreditsName, _
               .CreditsUrl = image.CreditsUrl, _
               .Tags = image.Tags, _
                .ImageType = image.Type})
            Return True
         End If
      End If
      Return False
   End Function

   <WebMethod()> _
   Public Function GetImageByID(ByVal id As String) As ImageJSON
      If Not String.IsNullOrEmpty(id) Then
         Dim _imagesrvr As New ImageService()
         Dim i As Integer
         Integer.TryParse(id, i)
         If i > 0 Then
            Dim im As SimpleImage = _imagesrvr.GetImageById(i)
            If im IsNot Nothing Then
               Dim acj As New ImageJSON(im)
               acj.ImageUrl = Configs.Paths.CdnRoot & "/imagethumb.ashx?img=" & (Configs.Paths.CdnImages & im.ImageUrl) & "&w=150&h=80"
               Return acj
            End If
         End If
      End If
      Return Nothing
   End Function

   '<ScriptMethod(UseHttpGet:=True)> _
   <WebMethod(CacheDuration:=60)> _
   Public Function GetImages(ByVal keys() As String) As ImageJSON()
      Dim imgsrvr As New ImageService
      Dim li As New List(Of SimpleImage)

      li = imgsrvr.GetImages(keys)
      Dim obj As New List(Of ImageJSON)
      If li.Count > 0 Then
         For Each it As SimpleImage In li

            Dim large_str As String = ""
            Dim mini_str As String = ""
            Dim long_str As String = ""
            Dim fi As New FileInfo(HttpContext.Current.Server.MapPath(it.ImageUrl))




            If fi IsNot Nothing Then
               large_str = Configs.Paths.CdnRoot & it.ImageUrl.Substring(0, it.ImageUrl.LastIndexOf("/")) & "large_" & it.Name
               long_str = Configs.Paths.CdnRoot & it.ImageUrl.Substring(0, it.ImageUrl.LastIndexOf("/")) & "long_" & it.Name
               mini_str = Configs.Paths.CdnRoot & it.ImageUrl.Substring(0, it.ImageUrl.LastIndexOf("/")) & "mini_" & it.Name
            End If

            Dim ar = New ImageJSON(it)
            ar.LargeImageUrl = large_str
            ar.MiniImageUrl = mini_str
            ar.LongImageUrl = long_str

            obj.Add(ar)
         Next
      End If

      Return obj.ToArray()
   End Function

   <WebMethod()> _
   Public Function DeleteImages(ByVal Ids() As Integer) As Boolean

      If Ids.Length > 0 AndAlso HasAdminCredentials() Then
         Dim imrepo As New ImageRepository
         imrepo.DeleteImages(Ids)
      End If
      Return False
   End Function
#End Region

#Region "Ads"

   <WebMethod()> _
Public Function GetAdByID(ByVal id As String) As AdJSON
      If Not String.IsNullOrEmpty(id) Then
         Dim i As Integer
         Dim svr As New AdService()
         Integer.TryParse(id, i)
         If i > 0 Then
            Dim im As Ad = svr.GetAdById(i)
            If im IsNot Nothing Then
               Dim acj As New AdJSON(im)
               Return acj
            End If
         End If
      End If
      Return Nothing
   End Function

   <WebMethod()> _
  Public Function saveAd(ByVal ad As AdJSON) As Boolean

      If ad IsNot Nothing AndAlso HasValidCredentials() Then
         Dim i As Integer
         Integer.TryParse(ad.AdID, i)
         Dim svr As New AdService()
         If i > 0 Then
            Dim cat As Ad = svr.GetAdById(i)
            If cat IsNot Nothing Then
               svr.UpdateAd(New Ad With { _
               .AdID = ad.AdID, _
               .Title = ad.Title, _
               .Body = ad.Body, _
               .Description = ad.Description, _
               .keywords = ad.Keywords, _
               .Type = ad.Type})
               Return True
            End If
         Else
            If Not String.IsNullOrEmpty(ad.Title) Then
               svr.InsertAd(New Ad With { _
               .Title = ad.Title, _
               .Body = ad.Body, _
               .Description = ad.Description, _
               .keywords = ad.Keywords, _
               .Type = ad.Type})
            End If
         End If
      End If
      Return False
   End Function

   <WebMethod()> _
Public Function DeleteAds(ByVal Ids() As Integer) As Boolean
      If Ids.Length > 0 AndAlso HasValidCredentials() Then
         Dim svr As New AdService()
         svr.DeleteAds(Ids)
      End If
      Return False
   End Function

#End Region

#Region "Videos"

   <WebMethod()> _
   Public Function GetVideoByID(ByVal id As Integer) As VideoJSON
      If id > 0 Then
         Dim _videoservice As IVideoService = New VideoService()
         Dim vd As Video = _videoservice.GetVideoById(id)
         If vd IsNot Nothing Then
            Dim acj As New VideoJSON(vd)
            Return acj
         End If
      End If
      Return Nothing
   End Function

   <WebMethod()> _
 Public Function saveVideo(ByVal video As VideoJSON) As Boolean

      If video IsNot Nothing AndAlso HasValidCredentials() Then
         Dim _videoservice As IVideoService = New VideoService()
         Dim i As Integer
         Integer.TryParse(video.VideoID, i)
         If i > 0 Then
            _videoservice.SimpleUpdateVideo(New Video With {.VideoID = video.VideoID, .Title = video.Title, .Abstract = video.Abstract, _
                                                            .Tags = video.Tags})
            Return True
         End If
      End If
      Return False
   End Function

   <WebMethod()> _
   Public Function DeleteVideos(ByVal Ids() As Integer) As Boolean

      If Ids.Length > 0 AndAlso HasAdminCredentials() Then
         Dim _videoservice As IVideoService = New VideoService()
         _videoservice.DeleteVideos(Ids)
      End If
      Return False
   End Function


#End Region

#Region "quotes"
   <WebMethod()> _
Public Function GetQuoteByID(ByVal id As Integer) As QuoteJSON
      If id > 0 Then
         Dim qtserv As New QuoteService()
         Dim qt As Quote = qtserv.GetQuoteById(id)
         If qt IsNot Nothing Then
            Dim acj As New QuoteJSON(qt)
            Return acj
         End If
      End If
      Return Nothing
   End Function

   <WebMethod()> _
 Public Function saveQuote(ByVal quote As QuoteJSON) As Boolean

      If quote IsNot Nothing AndAlso HasValidCredentials() Then
         Dim i As Integer
         Integer.TryParse(quote.QuoteID, i)
         If i > 0 Then
            Dim qts As New QuoteService
                qts.UpdateQuote(New Quote With {.QuoteID = quote.QuoteID, .Body = quote.Description, .Author = quote.Author, .Role = quote.Role})
            Return True
         End If
      End If
      Return False
   End Function


   <WebMethod()> _
Public Function DeleteQuotes(ByVal Ids() As Integer) As Boolean
      If Ids.Length > 0 AndAlso HasValidCredentials() Then
         Dim qs As New QuoteService()
         qs.DeleteQuotes(Ids)
      End If
      Return False
   End Function
#End Region

   '#Region "questions"
   '   <WebMethod()> _
   '   Public Function VoteQuestionUp(ByVal id As Integer) As VoteJSON

   '      If Not HasValidCredentials() Then
   '         Return New VoteJSON(id, 0, False, "Invalid Credentials")
   '      End If

   '      If id > 0 Then
   '         Dim _uqsrv As New UserQuestionService()
   '         _uqsrv.VoteUp(id)
   '         Return Nothing
   '      End If

   '      Return New VoteJSON(id, 0, False, "Unable to cast your vote at this time")
   '   End Function

   '   <WebMethod()> _
   '  Public Function VoteQuestionDown(ByVal id As Integer) As VoteJSON

   '      If Not HasValidCredentials() Then
   '         Return New VoteJSON(id, 0, False, "Invalid Credentials")
   '      End If

   '      If id > 0 Then
   '         Dim _uqsrv As New UserQuestionService()
   '         _uqsrv.VoteDown(id)
   '         Return Nothing
   '      End If
   '      Return New VoteJSON(id, 0, False, "Unable to cast your vote at this time")
   '   End Function

   '   <WebMethod()> _
   '   Public Function AcceptRejectAnswer(ByVal questionId As Integer, ByVal answerId As Integer) As String()

   '      If Not HasValidCredentials() Then
   '         Return New String() {"Invalid credentials"}
   '      End If

   '      If answerId > 0 Then
   '         Dim _uqsrv As New UserQuestionService()
   '         _uqsrv.SetAcceptedAnswer(questionId, answerId)
   '      End If
   '      Return Nothing
   '   End Function

   '   <WebMethod()> _
   '   Public Function CommentQuestion(ByVal questionId As Integer, ByVal body As String) As Boolean
   '      If Not HasValidCredentials() Then
   '         Return False
   '      End If

   '      If questionId > 0 AndAlso Not String.IsNullOrEmpty(body.Trim()) Then
   '         Dim _uqsrv As New UserQuestionCommentService()
   '         _uqsrv.InsertUserQuestionComment(New UserQuestionComment With {.UserQuestionID = questionId, .body = body})
   '         Return True
   '      End If

   '      Return False
   '   End Function

   '   <WebMethod(), Authorize()> _
   '  Public Function PostAnswer(ByVal Id As Integer, ByVal body As String) As Boolean
   '      If Not HasValidCredentials() Then
   '         Return False
   '      End If

   '      If Id > 0 AndAlso Not String.IsNullOrEmpty(body.Trim()) Then
   '         Dim _uqsrv As New UserAnswerService()
   '         _uqsrv.InsertUserAnswer(New UserAnswer With {.UserQuestionID = Id, .Body = body})
   '         Return True
   '      End If

   '      Return False
   '   End Function
   '#End Region

   '#Region "answers"
   '   <WebMethod()> _
   '   Public Function VoteAnswerUp(ByVal id As Integer) As VoteJSON

   '      If Not HasValidCredentials() Then
   '         Return New VoteJSON(id, 0, False, "Invalid Credentials")
   '      End If

   '      If id > 0 Then
   '         Dim _uqsrv As New UserAnswerService()
   '         _uqsrv.VoteUp(id)
   '         Return Nothing
   '      End If

   '      Return New VoteJSON(id, 0, False, "Unable to cast your vote at this time")
   '   End Function

   '   <WebMethod()> _
   '  Public Function VoteAnswerDown(ByVal id As Integer) As VoteJSON
   '      If Not HasValidCredentials() Then
   '         Return New VoteJSON(id, 0, False, "Invalid Credentials")
   '      End If

   '      If id > 0 Then
   '         Dim _uqsrv As New UserAnswerService()
   '         _uqsrv.VoteDown(id)
   '         Return Nothing
   '      End If
   '      Return New VoteJSON(id, 0, False, "Unable to cast your vote at this time")
   '   End Function


   '   <WebMethod()> _
   '  Public Function CommentAnswer(ByVal answerId As Integer, ByVal body As String) As Boolean
   '      If Not HasValidCredentials() Then
   '         Return False
   '      End If

   '      If answerId > 0 AndAlso Not String.IsNullOrEmpty(body.Trim) Then
   '         Dim _uqsrv As New UserAnswerCommentService()
   '         _uqsrv.InsertUserAnswerComment(New UserAnswerComment With {.UserAnswerID = answerId, .Body = body})
   '         Return True
   '      End If

   '      Return False
   '   End Function
   '#End Region

   '#Region "Users"
   '   <WebMethod(), Authorize(Roles:="Administrators")> _
   '  Public Function DeleteUsers(ByVal Ids() As String) As Boolean
   '      If Ids.Length > 0 AndAlso HasAdminCredentials() Then
   '         Dim usrservice As New UserService()
   '         usrservice.DeleteUsers(Ids)
   '      End If
   '      Return False
   '   End Function

   '   <WebMethod()> _
   'Public Function GetUsers(ByVal pageIndex As Integer, ByVal pageSize As Integer) As UserJSON()

   '      If Not HasAdminCredentials() Then
   '         Return Nothing
   '      End If

   '      Dim userservice As New UserService()
   '      Dim li As List(Of SiteUser) = userservice.GetUsers(pageIndex, pageSize, "", "")

   '      Dim obj As New List(Of UserJSON)
   '      For Each it As SiteUser In li
   '         Dim ar = New UserJSON(it.Username, it.Email, it.IsApproved, it.CreationDate, it.LastActivityDate)
   '         obj.Add(ar)
   '      Next

   '      Return obj.ToArray
   '   End Function
   '#End Region

#Region "Poll"
   <WebMethod()> _
   Public Function VotePoll(ByVal Id As Integer) As String
      Dim result As String = String.Empty
      If HttpContext.Current.User.Identity.IsAuthenticated Then
         Dim _pollOpSrvr As New PollOptionService()
         Dim pid As Integer = -1
         pid = _pollOpSrvr.VotePollOptions(Id)
         Dim tv As Integer = _pollOpSrvr.GetTotalVotesByPollOptionId(Id)
         Return "success" & "|" & pid.ToString & "|" & tv.ToString
      End If
      Return result
   End Function
#End Region

End Class
