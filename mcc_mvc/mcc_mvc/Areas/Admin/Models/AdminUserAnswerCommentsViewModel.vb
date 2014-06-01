Imports MCC.Data
Imports Webdiyer.WebControls.Mvc

Public Class AdminUserAnswerCommentsViewModel
   Inherits baseViewModel


   Private _comments As PagedList(Of UserAnswerComment)
   Public Property Comments() As PagedList(Of UserAnswerComment)
      Get
         Return _comments
      End Get
      Set(ByVal value As PagedList(Of UserAnswerComment))
         _comments = value
      End Set
   End Property


End Class
