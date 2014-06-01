Imports MCC.Data
Imports MCC.Services
Imports Webdiyer.WebControls.Mvc
Public Class vmQuestions
   Inherits baseViewModel

   Private _userquestionService As IUserQuestionService
   Private _useranswerService As IUserAnswerService

   Public Sub New()
      Me.New(New UserQuestionService(), New UserAnswerService(), 1, 30)
   End Sub


   Public Sub New(ByVal __pageIndex As Integer, ByVal __pageSize As Integer)
      Me.New(New UserQuestionService(), New UserAnswerService(), __pageIndex, __pageSize)
   End Sub

   Public Sub New(ByVal userQuestService As IUserQuestionService, ByVal userAnswerSrvr As IUserAnswerService, ByVal page As Integer, ByVal size As Integer)
      _userquestionService = userQuestService
      _useranswerService = userAnswerSrvr
      InitData(page, size)
   End Sub


   Private Sub InitData(ByVal page As Integer, ByVal size As Integer)
      _questions = _userquestionService.GetUserQuestionsByState(QuestionState.All, page, size)

      If _questions IsNot Nothing Then
         For Each it As UserQuestion In _questions
            If it.Body.Length > 200 Then
               it.Body = it.Body.Substring(0, 200) & "..."
            End If
            it.AnswerCount = _useranswerService.GetUserAnswersCountByQuestionId(it.UserQuestionID)
         Next

         If Not String.IsNullOrEmpty(UserRoutines.CurrentUserName) Then
            _questionFollowed = _userquestionService.GetSubscribedQuestions(UserRoutines.CurrentUserName)
         End If
      End If
   End Sub

   Private _questions As PagedList(Of UserQuestion)
   Public Property QuestionsList() As PagedList(Of UserQuestion)
      Get
         Return _questions
      End Get
      Set(ByVal value As PagedList(Of UserQuestion))
         _questions = value
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


   Private _questionFollowed As List(Of Integer)
   Public Property QuestionFollowed() As List(Of Integer)
      Get
         If _questionFollowed Is Nothing Then
            _questionFollowed = New List(Of Integer)
         End If
         Return _questionFollowed
      End Get
      Set(ByVal value As List(Of Integer))
         _questionFollowed = value
      End Set
   End Property



End Class
