Imports MCC.Data
Imports Webdiyer.WebControls.Mvc
Public Class UserProfileQuestionViewModel
   Inherits baseViewModel

   Private _questions As PagedList(Of UserQuestion)
   Public Property Questions() As PagedList(Of UserQuestion)
      Get
         Return _questions
      End Get
      Set(ByVal value As PagedList(Of UserQuestion))
         _questions = value
      End Set
   End Property
End Class
