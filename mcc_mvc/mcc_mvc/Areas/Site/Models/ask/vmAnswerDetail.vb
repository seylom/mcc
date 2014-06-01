Imports MCC.Data
Imports MCC.Services

Public Class vmAnswerDetail
   Inherits baseViewModel

   Private _userAnswerCommentService As IUserAnswerCommentService
   Public Sub New(ByVal answerIt As UserAnswer, ByVal questionIt As UserQuestion)
      Me.New(answerIt, questionIt, New UserAnswerCommentService())
   End Sub

   Public Sub New(ByVal answerIt As UserAnswer, ByVal questionIt As UserQuestion, ByVal userCommentService As IUserAnswerCommentService)
      _userAnswerCommentService = userCommentService
      _answer = answerIt
      _question = questionIt
      If _answer IsNot Nothing Then
         _comments = _userAnswerCommentService.GetUserAnswerCommentsByAnswerId(_answer.UserAnswerID)
      End If
   End Sub


   Private _question As UserQuestion
   Public ReadOnly Property Question() As UserQuestion
      Get
         Return _question
      End Get
   End Property

   Private _isSelectedAnswer As Boolean
   Public ReadOnly Property IsAnswer() As Boolean
      Get
         If _question IsNot Nothing Then
            Return (_question.BestUserAnswerID = _answer.UserAnswerID)
         End If
         Return False
      End Get
   End Property

   Private _answer As UserAnswer
   Public Property Answer() As UserAnswer
      Get
         Return _answer
      End Get
      Set(ByVal value As UserAnswer)
         _answer = value
      End Set
   End Property

   Private _comments As List(Of UserAnswerComment)
   Public Property Comments() As List(Of UserAnswerComment)
      Get
         Return _comments
      End Get
      Set(ByVal value As List(Of UserAnswerComment))
         _comments = value
      End Set
   End Property

   Public ReadOnly Property IsQuestionPoster() As Boolean
      Get
         If _question IsNot Nothing Then
            Return AreSameUser(_question.AddedBy)
         End If
         Return False
      End Get
   End Property

   Public ReadOnly Property IsAnswerPoster() As Boolean
      Get
         If _answer IsNot Nothing Then
            Return AreSameUser(_answer.AddedBy)
         End If
         Return False
      End Get
   End Property


End Class
