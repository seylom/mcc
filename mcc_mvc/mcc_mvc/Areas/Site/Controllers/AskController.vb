Imports MCC.Services
Imports MCC.Data
Imports System.Net.Mail

Namespace MCC.Areas.Site.Controllers
   Public Class AskController
      Inherits ApplicationController

      Private _questionService As IUserQuestionService
      Private _answerservice As IUserAnswerService

      Public Sub New()
         Me.New(New UserQuestionService, New UserAnswerService)
      End Sub

      Public Sub New(ByVal questionsrvr As IUserQuestionService, ByVal answersrvr As IUserAnswerService)
         _questionService = questionsrvr
         _answerservice = answersrvr
      End Sub

      Function Index(ByVal page? As Integer, ByVal size? As Integer) As ActionResult
         Dim _viewdata As New vmQuestions(_questionService, _answerservice, If(page, 0), If(size, 30))
         _viewdata.PageTitle = "Questions and Answers"
         _viewdata.MetaDescription = ""
         Return View(_viewdata)
      End Function

      ''' <summary>
      ''' 
      ''' </summary>
      ''' <param name="Id"></param>
      ''' <returns></returns>
      ''' <remarks></remarks>
      Function ViewQuestion(ByVal Id As Integer, ByVal page? As Integer, ByVal size? As Integer) As ActionResult

         If Id <= 0 Then
            Return RedirectToAction("Index")
         End If

         Dim _viewdata As New vmQuestionDetail(Id, _questionService, _answerservice, If(page, 0), If(size, 10))
         _viewdata.PageTitle = routines.Encode(_viewdata.Question.Title)
         _viewdata.MetaDescription = routines.Encode(_viewdata.Question.Title)


         Dim uqsvr As New UserQuestionService()
         uqsvr.IncrementViewCount(Id)

         Return View(_viewdata)
      End Function

      Function EditQuestion(ByVal Id As Integer) As ActionResult
         Dim uaserv As New UserQuestionService()
         Dim _viewdata As UserQuestion = uaserv.GetUserQuestionById(Id)


         If _viewdata Is Nothing Then
            Return View("Index")
         End If

         Return View(_viewdata)
      End Function

      <Authorize(), AcceptVerbs(HttpVerbs.Post)> _
      Function EditQuestion(ByVal id As Integer, ByVal Title As String, ByVal body As String, ByVal questionId As Integer, ByVal slug As String) As ActionResult

         If String.IsNullOrEmpty(Title) Then
            TempData("ErrorMessage") = "Please enter your question title"
            Return RedirectToAction("EditQuestion", New With {.id = id})
         End If

         If String.IsNullOrEmpty(body) Then
            TempData("ErrorMessage") = "Please enter your question details"
            Return RedirectToAction("EditQuestion", New With {.id = id})
         End If

         Dim uaserv As New UserQuestionService()
         Dim origUq As UserQuestion = uaserv.GetUserQuestionById(id)

         If origUq IsNot Nothing Then
            origUq.Title = Title
            origUq.Body = body
            uaserv.UpdateUserQuestions(origUq)
         End If

         Return RedirectToAction("ViewQuestion", New With {.id = id})
      End Function


      Function ValidateEdition(ByVal title As String, ByVal body As String) As Boolean
         If String.IsNullOrEmpty(title) Then
            ModelState.AddModelError("Title", "Please add some title")
         End If

         If String.IsNullOrEmpty(body) Then
            ModelState.AddModelError("Body", "Please enter some detail for the question")
         End If

         Return ModelState.IsValid
      End Function

      <Authorize(), AcceptVerbs(HttpVerbs.Post)> _
      Function PostAnswer(ByVal id As Integer, ByVal body As String) As ActionResult

         If id <= 0 Then
            Return RedirectToAction("Index")
         End If

         If String.IsNullOrEmpty(body) Then
            TempData("ErrorMessage") = "Please enter some text in the answer box"
            Return RedirectToAction("ViewQuestion", New With {.id = id})
         End If

         Dim uqservice As New UserQuestionService()
         Dim uq As UserQuestion = uqservice.GetUserQuestionById(id)

         If uq Is Nothing Then
            Return RedirectToAction("Index")
         End If

         Dim usrvr As New UserAnswerService()
         Dim ua As New UserAnswer With {.Body = body, .UserQuestionID = id}
         Dim sti As StatusItem = usrvr.InsertUserAnswer(ua)

         If sti.Success Then
            ua.UserAnswerID = sti.Id
            SendNotifications(ua, uq)
         End If

         Return RedirectToAction("ViewQuestion", New With {.id = id, .slug = uq.Slug})

      End Function


      Sub SaveQuestion(ByVal q As UserQuestion)
         TempData("question") = q
      End Sub

      <Authorize()> _
      Function AskQuestion() As ActionResult
         Dim _viewdata As New AskQuestionViewModel
         Return View(_viewdata)
      End Function

      <Authorize(), AcceptVerbs(HttpVerbs.Post)> _
      Function AskQuestion(ByVal question As AskQuestionViewModel) As ActionResult
         If question Is Nothing Then
            Return RedirectToAction("Index")
         End If

         If String.IsNullOrEmpty(question.Title) Or String.IsNullOrEmpty(question.Body) Then
            question.Messages.Add("please fill in all fields.")
            Return View(question)
         End If

         Dim idx As Integer = 0
         If ModelState.IsValid Then
            Dim uq As New UserQuestion With {.Title = question.Title, .Body = question.Body, .ActivityNotification = question.Subscribe}
            Dim uqsrvr As New UserQuestionService()
            Dim sti As New StatusItem
            sti = uqsrvr.InsertUserQuestion(uq)
            If Not sti.Success Then
               question.Messages.Add(sti.Message)
               Return View(question)
            End If

            idx = sti.Id
         End If

         Return RedirectToAction("ViewQuestion", New With {.id = idx})
      End Function

      '<Authorize(), AcceptVerbs(HttpVerbs.Post)> _
      'Function CommentQuestion(ByVal id As Integer, ByVal slug As String, ByVal body As String) As ActionResult

      '   If id <= 0 Then
      '      RedirectToAction("index")
      '   End If

      '   If String.IsNullOrEmpty(body) Then
      '      TempData("ErrorMessage") = "Enter some comment!"
      '      RedirectToAction("ViewQuestion", New With {.id = id, .slug = slug})
      '   End If


      '   Dim uqsvr As New UserQuestionCommentService()
      '   Dim uqc As New UserQuestionComment With {.body = body, .UserQuestionID = id}
      '   Dim idx As Integer = uqsvr.InsertUserQuestionComment(uqc)

      '   Return RedirectToAction("ViewQuestion", New With {.id = id, .slug = slug})
      'End Function


      '<Authorize(), AcceptVerbs(HttpVerbs.Post)> _
      'Function CommentAnswer(ByVal id As Integer, ByVal body As String, ByVal questionId As Integer, ByVal slug As String) As ActionResult
      '   If id <= 0 Then
      '      RedirectToAction("index")
      '   End If

      '   If String.IsNullOrEmpty(body) Then
      '      TempData("ErrorMessage") = "Enter some comment!"
      '      RedirectToAction("ViewQuestion", New With {.id = questionId, .slug = slug})
      '   End If


      '   Dim uasvr As New UserAnswerCommentService()
      '   Dim uac As New UserAnswerComment With {.body = body, .UserAnswerID = id}
      '   Dim idx As Integer = uasvr.InsertUserAnswerComment(uac)

      '   If idx <= 0 Then
      '      TempData("ErrorMessage") = "Unable to add a comment at this time!"
      '   End If

      '   Return RedirectToAction("ViewQuestion", New With {.id = questionId, .slug = slug})
      'End Function


      Function EditAnswer(ByVal Id As Integer) As ActionResult
         If Id <= 0 Then
            RedirectToAction("Index")
         End If
         Dim _viewdata As New vmEditAnswerDetail(Id)
         Return View(_viewdata)
      End Function

      <Authorize(), AcceptVerbs(HttpVerbs.Post)> _
      Function EditAnswer(ByVal id As Integer, ByVal body As String, ByVal questionId As Integer, ByVal slug As String) As ActionResult

         If id <= 0 Then
            Return RedirectToAction("ViewQuestion", New With {.id = questionId, .slug = slug})
         End If

         Dim vm As New vmEditAnswerDetail(id)
         UpdateModel(vm)

         If String.IsNullOrEmpty(body) Then
            TempData("ErrorMessage") = "Please Enter some text for your answer"
            Return View(vm)
         End If

         Dim uasrvr As New UserAnswerService()
         Dim ua As UserAnswer = uasrvr.GetUserAnswerById(id)
         ua.Body = body
         uasrvr.UpdateUserAnswers(ua)

         Return RedirectToAction("ViewQuestion", New With {.id = questionId, .slug = slug})
      End Function


#Region "ajax"

      <Authorize(), AcceptVerbs(HttpVerbs.Post)> _
      Function VoteQuestionUp(ByVal id As Integer) As ActionResult
         If Request.IsAjaxRequest() Then
            If id > 0 Then
               Dim usr As New UserQuestionService()
               Dim sti As New StatusItem
               sti = usr.VoteUp(id)
               Return Json(sti)
            Else
               Return RedirectToAction("Index")
            End If
         Else
            Return RedirectToAction("ViewQuestion", New With {.id = id})
         End If
      End Function

      <Authorize(), AcceptVerbs(HttpVerbs.Post)> _
      Function VoteQuestionDown(ByVal id As Integer) As ActionResult
         If Request.IsAjaxRequest() Then
            If id > 0 Then
               Dim usr As New UserQuestionService()
               Dim sti As New StatusItem
               sti = usr.VoteDown(id)
               Return Json(sti)
            Else
               Return RedirectToAction("Index")
            End If
         Else
            Return RedirectToAction("ViewQuestion", New With {.id = id})
         End If
      End Function


      <Authorize(), AcceptVerbs(HttpVerbs.Post)> _
      Function VoteAnswerUp(ByVal id As Integer) As ActionResult
         If Request.IsAjaxRequest() Then
            If id > 0 Then
               Dim usr As New UserAnswerService()
               Dim sti As New StatusItem
               sti = usr.VoteUp(id)
               Return Json(sti)
            Else
               Return RedirectToAction("Index")
            End If
         Else
            Return RedirectToAction("ViewQuestion", New With {.id = id})
         End If
      End Function

      <Authorize(), AcceptVerbs(HttpVerbs.Post)> _
      Function VoteAnswerDown(ByVal id As Integer) As ActionResult
         If Request.IsAjaxRequest() Then
            If id > 0 Then
               Dim usr As New UserAnswerService()
               Dim sti As New StatusItem
               sti = usr.VoteDown(id)
               Return Json(sti)
            Else
               Return RedirectToAction("Index")
            End If
         Else
            Return RedirectToAction("ViewQuestion", New With {.id = id})
         End If
      End Function


      <Authorize(), AcceptVerbs(HttpVerbs.Post), ValidateInput(False)> _
      Public Function CommentQuestion(ByVal Id As Integer, ByVal comment As String) As ActionResult
         If Id <= 0 Or String.IsNullOrEmpty(comment) Then
            Return Json("false")
         End If

         Dim _uqsrv As New UserQuestionCommentService()
         Dim idx As Integer = _uqsrv.InsertUserQuestionComment(New UserQuestionComment With {.UserQuestionID = Id, .body = comment})

         If Request.IsAjaxRequest() Then
            Return Json(New CommentJSON With {.Id = Id, .Body = routines.Encode(comment), .Success = idx > 0, .AddedBy = User.Identity.Name})
         Else
            Return RedirectToAction("ViewQuestion", New With {.id = Id})
         End If
      End Function

      <Authorize(), AcceptVerbs(HttpVerbs.Post), ValidateInput(False)> _
      Public Function CommentAnswer(ByVal Id As Integer, ByVal comment As String) As ActionResult
         If Id <= 0 Or String.IsNullOrEmpty(comment) Then
            Return View()
         End If
         If Request.IsAjaxRequest() Then
            Dim _uqsrv As New UserAnswerCommentService()
            Dim idx As Integer = _uqsrv.InsertUserAnswerComment(New UserAnswerComment With {.UserAnswerID = Id, .Body = comment})
            Return Json(New CommentJSON With {.Id = Id, .Body = routines.Encode(comment), .Success = idx > 0, .AddedBy = User.Identity.Name})
         End If
         Return View()
      End Function

      <Authorize(), AcceptVerbs(HttpVerbs.Post)> _
      Public Function AcceptRejectAnswer(ByVal questionId As Integer, ByVal answerId As Integer) As ActionResult
         If answerId > 0 AndAlso answerId > 0 Then
            Dim _uqsrv As New UserQuestionService()
            _uqsrv.SetAcceptedAnswer(questionId, answerId)
            Return Json(New With {.Id = answerId, .Success = True})
         End If
         Return View()
      End Function


      <Authorize(), AcceptVerbs(HttpVerbs.Post)>
      Public Function Follow(ByVal Id As Integer) As JsonResult
         If Id > 0 Then
            Dim _uqsrv As New UserQuestionService()
            Dim result As Boolean = _uqsrv.SubscribeOrUnsubscribe(Id, User.Identity.Name)
            Return Json(result)
         End If
         Return Json(False)
      End Function


      ''' <summary>
      ''' Sends a notification to all visitors  that has registered
      ''' to retrieve notifications for the specific post.
      ''' </summary>
      ''' <param name="answer"></param>
      ''' <remarks></remarks>
      Private Sub SendNotifications(ByVal answer As UserAnswer, ByVal question As UserQuestion)

         Dim subscribers As List(Of String) = New UserQuestionService().GetSubscribers(answer.UserQuestionID)

         If question.ActivityNotification AndAlso Not subscribers.Contains(question.AddedBy) Then
            subscribers.Add(question.AddedBy)
         End If

         If (subscribers Is Nothing Or subscribers.Count = 0) Then
            Return
         End If

         For Each subscriber As String In subscribers

            Dim mu As MembershipUser = Membership.GetUser(subscriber)

            If mu IsNot Nothing AndAlso subscriber <> answer.AddedBy Then
               Dim subsEmail As String = mu.Email

               Dim PermaLink As String = Url.Action("ViewQuestion", "Ask", New With {.id = question.UserQuestionID, .slug = question.Slug}, "http")

               Dim unsubscribeLink As String = PermaLink
               unsubscribeLink += IIf(unsubscribeLink.Contains("?"), "&", "?") + "unsubscribe-email=" + HttpUtility.UrlEncode(subsEmail)

               Dim Mail As MailMessage = New MailMessage
               'Mail.From = New MailAddress(BlogSettings.Instance.Email, BlogSettings.Instance.Name)
               Mail.Subject = "(Ask MCC) New answer available : " + question.Title
               Mail.Body += "View the complete question at " & String.Format("<a href='{0}'>{1}</a>", PermaLink + "#answer_" + answer.UserAnswerID.ToString, PermaLink) & "<br /><br />"
               Mail.Body += "Answer posted by: " + answer.AddedBy + "<br />"
               Mail.Body += "Question: " + question.Title & "<br /><br />"

               Dim body As String = String.Empty
               body = answer.Body.Replace(Environment.NewLine, "<br />") + "<br /><br />"
               body = String.Format("<div style='margin:5px 10px;padding:5px;background-color:#eaeaea;'>{0}</div>", body)
               Mail.Body += body

               Mail.Body += "<br /><br /><hr />"
               Mail.Body += "<div style='font-size:10px'>You were sent this email because you opted to receive email notifications when someone answered to this question<br/>"
               Mail.Body += String.Format("<a href='{0}'>{1}</a>", unsubscribeLink, "Unsubscribe") & " from future answer notification emails to this question.<br/><br/>"
               Mail.Body += "<div> - The MiddleClassCrunch Team <div/><div/>"

               Mail.To.Add(subsEmail)
               Utils.SendMailMessageAsync(Mail)

            End If
         Next

      End Sub

#End Region

   End Class
End Namespace