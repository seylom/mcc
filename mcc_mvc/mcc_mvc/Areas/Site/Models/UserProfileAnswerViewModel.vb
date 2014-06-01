Imports MCC.Data
Imports Webdiyer.WebControls.Mvc

Public Class UserProfileAnswerViewModel


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
