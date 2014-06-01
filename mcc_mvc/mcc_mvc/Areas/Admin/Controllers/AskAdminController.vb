Imports MCC.Services
Imports MCC.Data

Namespace MCC.Areas.Admin.Controllers
   Public Class AskAdminController
      Inherits AdminController

      Private _userquestionservice As IUserQuestionService
      Private _useranswerservice As IUserAnswerService
      Private _userquestioncommentservice As IUserQuestionCommentService
      Private _useranswercommentservice As IUserAnswerCommentService

      Public Sub New()
         Me.New(New UserQuestionService(), New UserAnswerService(), New UserQuestionCommentService, New UserAnswerCommentService())
      End Sub


      Public Sub New(ByVal userquetionvrvr As IUserQuestionService, ByVal useranswersrvr As IUserAnswerService, _
                        ByVal userquestioncommentsrvr As IUserQuestionCommentService, ByVal useranswercommentsrvr As IUserAnswerCommentService)
         _userquestionservice = userquetionvrvr
         _useranswerservice = useranswersrvr
         _userquestioncommentservice = userquestioncommentsrvr
         _useranswercommentservice = useranswercommentsrvr
      End Sub

      Function Index(ByVal page? As Integer, ByVal size? As Integer) As ActionResult
         Dim _viewdata As New AdminUserQuestionsViewModel()
         _viewdata.questions = _userquestionservice.GetUserQuestions(If(page, 0), If(size, 30))
         Return View(_viewdata)
      End Function

      Function Answers(ByVal page? As Integer, ByVal size? As Integer) As ActionResult
         Dim _viewdata As New AdminUserAnswersViewModel()
         _viewdata.Answers = _useranswerservice.GetUserAnswers(If(page, 0), If(size, 30))
         Return View(_viewdata)
      End Function


      Function AnswersByQuestion(ByVal Id As Integer, ByVal page? As Integer, ByVal size? As Integer) As ActionResult
         If Id <= 0 Then
            Return RedirectToAction("Index")
         End If

         Dim uq As UserQuestion = _userquestionservice.GetUserQuestionById(Id)
         If uq Is Nothing Then
            Return RedirectToAction("Index")
         End If

         Dim _viewdata As New AdminUserAnswersViewModel()
         _viewdata.Title = uq.Title
         _viewdata.Answers = _useranswerservice.GetUserAnswersByQuestionId(Id, If(page, 0), If(size, 30))
         Return View(_viewdata)
      End Function


      Function QuestionComments(ByVal page? As Integer, ByVal size? As Integer) As ActionResult
         Dim _viewdata As New AdminUserQuestionCommentsViewModel()
         _viewdata.Comments = _userquestioncommentservice.GetUserQuestionComments(If(page, 0), If(size, 30))
         Return View(_viewdata)
      End Function

      Function QuestionCommentsByQuestion(ByVal id As Integer, ByVal page? As Integer, ByVal size? As Integer) As ActionResult

         If id <= 0 Then
            Return RedirectToAction("Index")
         End If

         Dim uq As UserQuestion = _userquestionservice.GetUserQuestionById(id)

         If uq Is Nothing Then
            Return RedirectToAction("Index")
         End If

         Dim _viewdata As New AdminUserQuestionCommentsViewModel()
         _viewdata.Comments = _userquestioncommentservice.GetUserQuestionCommentsByQuestionId(id, If(page, 0), If(size, 30))
         Return View(_viewdata)
      End Function


      Function AnswerComments(ByVal page? As Integer, ByVal size? As Integer) As ActionResult
         Dim _viewdata As New AdminUserAnswerCommentsViewModel()
         _viewdata.Comments = _useranswercommentservice.GetUserAnswerComments(If(page, 0), If(size, 30))
         Return View(_viewdata)
      End Function

      Function AnswerCommentsByAnswer(ByVal id As Integer, ByVal page? As Integer, ByVal size? As Integer) As ActionResult
         If id <= 0 Then
            Return RedirectToAction("Index")
         End If

         Dim ua As UserAnswer = _useranswerservice.GetUserAnswerById(id)
         If ua Is Nothing Then
            Return RedirectToAction("Index")
         End If

         Dim _viewdata As New AdminUserAnswerCommentsViewModel()
         _viewdata.Comments = _useranswercommentservice.GetUserAnswerCommentsByAnswerId(id, If(page, 0), If(size, 30))
         Return View(_viewdata)
      End Function


      <AcceptVerbs(HttpVerbs.Post)> _
      Function DeleteQuestions(ByVal Ids() As Integer) As ActionResult

         If Ids Is Nothing Then
            Return RedirectToAction("Index")
         End If

         If Request.IsAjaxRequest Then
            _userquestionservice.DeleteQuestions(Ids)
            Return Json(True)
         End If

         Return RedirectToAction("Index")
      End Function


      <AcceptVerbs(HttpVerbs.Post)> _
      Function DeleteAnswers(ByVal Ids() As Integer) As ActionResult

         If Ids Is Nothing Then
            Return RedirectToAction("Answers")
         End If

         If Request.IsAjaxRequest Then
            _useranswerservice.DeleteAnswers(Ids)
            Return Json(True)
         End If

         Return RedirectToAction("Answers")
      End Function


      <AcceptVerbs(HttpVerbs.Post)> _
      Function DeleteQuestionComments(ByVal Ids() As Integer) As ActionResult

         If Ids Is Nothing Then
            Return RedirectToAction("Index")
         End If

         If Request.IsAjaxRequest Then
            _userquestioncommentservice.DeleteUserQuestionComments(Ids)
            Return Json(True)
         End If

         Return RedirectToAction("Index")
      End Function

      <AcceptVerbs(HttpVerbs.Post)> _
      Function DeleteAnswerComments(ByVal Ids() As Integer) As ActionResult

         If Ids Is Nothing Then
            Return RedirectToAction("Answers")
         End If

         If Request.IsAjaxRequest Then
            _useranswercommentservice.DeleteUserAnswerComments(Ids)
            Return Json(True)
         End If

         Return RedirectToAction("Answers")
      End Function

   End Class
End Namespace