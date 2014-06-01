Imports Microsoft.VisualBasic
Imports System.Linq
Imports System.Data.Linq
Imports MCC.Data

Namespace MCC.Data
   Partial Class mcc_UserQuestion

      Public ReadOnly Property HasAcceptedAnswer() As Boolean
         Get
            Return False
            'Return (Me.BestUserAnswerId > 0)
         End Get
      End Property

      Public ReadOnly Property HasAnswers() As Boolean
         Get
            Dim key As String = "UserQuestions_HasAnswer"
            Dim mdc As New MCCDataContext
            'Return (mdc.mcc_UserAnswers.Count(Function(p) p.UserQuestionId = Me.UserQuestionId) > 0)
            Return False
         End Get
      End Property

      Public Shared Function IsQuestionBestAnswer(ByVal questionId As Integer, ByVal answerId As Integer) As Boolean
         Dim mdc As New MCCDataContext()

         If mdc.mcc_UserQuestions.Count(Function(p) p.UserQuestionId = questionId AndAlso p.BestUserAnswerId = answerId) Then
            Return True
         End If

         Return False
      End Function

   End Class
End Namespace
