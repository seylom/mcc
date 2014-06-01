Imports MCC.Data
Imports Webdiyer.WebControls.Mvc

Public Class AdminUserAnswersViewModel
   Inherits baseViewModel

   'Private _questionId As Integer
   'Public Property QuestionID() As Integer
   '   Get
   '      Return _questionId
   '   End Get
   '   Set(ByVal value As Integer)
   '      _questionId = value
   '   End Set
   'End Property

   Private _title As String
   Public Property Title() As String
      Get
         Return _title
      End Get
      Set(ByVal value As String)
         _title = value
      End Set
   End Property



   Private _answers As PagedList(Of UserAnswer)
   Public Property Answers() As PagedList(Of UserAnswer)
      Get
         Return _answers
      End Get
      Set(ByVal value As PagedList(Of UserAnswer))
         _answers = value
      End Set
   End Property

End Class
