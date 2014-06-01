Imports MCC.Services
Imports MCC.Data
Imports Webdiyer.WebControls.Mvc
Public Class vmQuestionDetail
   Inherits baseViewModel

   'Private _questionId As Integer
   Private _questionService As IUserQuestionService
   Private _answerService As IUserAnswerService

   'Public Sub New(ByVal id As Integer)
   '   Me.New(id, New UserQuestionService(), New UserAnswerService())
   'End Sub


   Public Sub New(ByVal id As Integer, ByVal _questionServ As IUserQuestionService, ByVal _answerSer As IUserAnswerService, ByVal page As Integer, ByVal size As Integer)
      _questionService = _questionServ
      _answerService = _answerSer
      _questionID = id
      InitData(page, size)
   End Sub


   Private Sub InitData(ByVal page As Integer, ByVal size As Integer)
      If _questionID <= 0 Then
         Return
      End If

      _question = _questionService.GetUserQuestionById(_questionID)
      If _question IsNot Nothing Then

         Dim _userQuestionCommentSrvr As New UserQuestionCommentService()
         _comments = _userQuestionCommentSrvr.GetUserQuestionCommentsByQuestionId(_question.UserQuestionID)

         '_question.Subscribers = _questionService.

         Dim useranswers As PagedList(Of UserAnswer) = _answerService.GetUserAnswersByQuestionId(_questionID, PageIndex, Size)
         Dim _userAnswerCommSrvr As New UserAnswerCommentService()
         _answersCount = _answerService.GetUserAnswersCountByQuestionId(_questionID)

         Dim templi As New List(Of vmAnswerDetail)

         For Each it As UserAnswer In useranswers
            Dim vmAns As New vmAnswerDetail(it, _question, _userAnswerCommSrvr)
            templi.Add(vmAns)
         Next

         If templi.Count > 0 Then
            _answers = New PagedList(Of vmAnswerDetail)(templi, PageIndex, Size, _answersCount)
         End If

         _isAuthor = (UserIsAuthenticated AndAlso _
                      HttpContext.Current.User.Identity.Name.Equals(_question.AddedBy, StringComparison.OrdinalIgnoreCase))

         _currentAnswer = New UserAnswer With {.UserQuestionID = _question.UserQuestionID}

         If Not String.IsNullOrEmpty(UserRoutines.CurrentUserName) Then
            _followed = _questionService.IsSubscribed(_question.UserQuestionID, UserRoutines.CurrentUserName)
         End If
      End If

   End Sub



   Private _questionID As Integer
   Public Property questionId() As Integer
      Get
         Return _questionID
      End Get
      Set(ByVal value As Integer)
         _questionID = value
      End Set
   End Property


   Private _question As UserQuestion
   Public Property Question() As UserQuestion
      Get
         Return _question
      End Get
      Set(ByVal value As UserQuestion)
         _question = value
      End Set
   End Property


   Private _Ads As List(Of Ad)
   Public Property Ads() As List(Of Ad)
      Get
         Return _Ads
      End Get
      Set(ByVal value As List(Of Ad))
         _Ads = value
      End Set
   End Property

   Private _answers As PagedList(Of vmAnswerDetail)
   Public Property Answers() As PagedList(Of vmAnswerDetail)
      Get
         Return _answers
      End Get
      Set(ByVal value As PagedList(Of vmAnswerDetail))
         _answers = value
      End Set
   End Property

    Private _isAuthor As Boolean
    Public ReadOnly Property CurrentUserIsAuthor() As Boolean
        Get
            Return _isAuthor
        End Get
    End Property


   Private _answersCount As Integer
   Public ReadOnly Property AnswersCount() As Integer
      Get
         Return _answersCount
      End Get
   End Property


   Private _comments As List(Of UserQuestionComment)
   Public Property Comments() As List(Of UserQuestionComment)
      Get
         Return _comments
      End Get
      Set(ByVal value As List(Of UserQuestionComment))
         _comments = value
      End Set
   End Property


   Private _currentAnswer As UserAnswer
   Public Property CurrentAnswer() As UserAnswer
      Get
         Return _currentAnswer
      End Get
      Set(ByVal value As UserAnswer)
         _currentAnswer = value
      End Set
   End Property


   Private _pageindex As Integer = 0
   Public Property PageIndex() As Integer
      Get
         Return _pageindex
      End Get
      Set(ByVal value As Integer)
         _pageindex = value
      End Set
   End Property

   Private _followed As Boolean
   Public Property Followed() As Boolean
      Get
         Return _followed
      End Get
      Set(ByVal value As Boolean)
         _followed = value
      End Set
   End Property

End Class
