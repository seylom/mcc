Imports MCC.Data
Imports MCC.Services

Public Class vmSimpleQuestionItem

   Dim _uqservice As IUserQuestionService


   Public Sub New(ByVal uq As UserQuestion)
      Me.New(uq, New UserQuestionService())
   End Sub

   Public Sub New(ByVal uq As UserQuestion, ByVal uqsvr As IUserQuestionService)
      _question = uq
      _uqservice = uqsvr
      InitData()
   End Sub

   Sub InitData()
      If _question IsNot Nothing Then
         answerCount = _uqservice.GetUserAnswersCountById(_question.UserQuestionID)
      End If
   End Sub

   Private _question As UserQuestion
   Public Property question() As UserQuestion
      Get
         Return _question
      End Get
      Set(ByVal value As UserQuestion)
         _question = value
      End Set
   End Property


   Private answerCount As Integer = 0
   Public Property AnswersCount() As Integer
      Get
         Return answerCount
      End Get
      Set(ByVal value As Integer)
         answerCount = value
      End Set
   End Property




End Class
