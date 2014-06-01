Imports MCC.Services
Imports MCC.Data

Public Class vmEditAnswerDetail
   Inherits baseViewModel

   Public Sub New(ByVal answerId As Integer)
      If answerId > 0 Then
         Dim asrvr As New UserAnswerService()
         _answer = asrvr.GetUserAnswerById(answerId)
         If _answer IsNot Nothing Then
            Dim uqsrvr As New UserQuestionService
            Dim uq As UserQuestion = uqsrvr.GetUserQuestionById(_answer.UserQuestionID)

            If uq IsNot Nothing Then
               _questionTitle = uq.Title
               _slug = uq.Slug
            End If
         End If

      End If
   End Sub


   Private _answer As UserAnswer
   Public Property Answer() As UserAnswer
      Get
         Return _answer
      End Get
      Set(ByVal value As UserAnswer)
         _answer = value
      End Set
   End Property

   Private _questionTitle As String
   Public Property Title() As String
      Get
         Return _questionTitle
      End Get
      Set(ByVal value As String)
         _questionTitle = value
      End Set
   End Property


   Private _slug As String
   Public Property QuestionSlug() As String
      Get
         Return _slug
      End Get
      Set(ByVal value As String)
         _slug = value
      End Set
   End Property

End Class
