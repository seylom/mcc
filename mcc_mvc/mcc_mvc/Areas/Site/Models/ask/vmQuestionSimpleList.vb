Imports MCC.Data
Imports MCC.Services
Imports Webdiyer.WebControls.Mvc
Public Class vmQuestionSimpleList
   Private _userQuestionService As IUserQuestionService

   Public Sub New()
      Me.New(New UserQuestionService())
   End Sub

   Public Sub New(ByVal userquestionSrvr As IUserQuestionService)
      _userQuestionService = userquestionSrvr
      InitData()
   End Sub

   Sub InitData()
      Dim _questionsList As PagedList(Of UserQuestion) = _userQuestionService.GetUserQuestions(0, 4)
      _questions = New List(Of vmSimpleQuestionItem)
      For Each it As UserQuestion In _questionsList
         Dim vmq As New vmSimpleQuestionItem(it, _userQuestionService)
         _questions.Add(vmq)
      Next
   End Sub

   Private _questions As List(Of vmSimpleQuestionItem)
   Public Property Questions() As List(Of vmSimpleQuestionItem)
      Get
         Return _questions
      End Get
      Set(ByVal value As List(Of vmSimpleQuestionItem))
         _questions = value
      End Set
   End Property

   Private _answerCount As Integer
   Public Property AnswersCount() As Integer
      Get
         Return _answerCount
      End Get
      Set(ByVal value As Integer)
         _answerCount = value
      End Set
   End Property

End Class
