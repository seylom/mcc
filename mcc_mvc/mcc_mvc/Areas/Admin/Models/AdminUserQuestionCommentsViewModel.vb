Imports MCC.Data
Imports Webdiyer.WebControls.Mvc

Public Class AdminUserQuestionCommentsViewModel
   Inherits baseViewModel

   Private _comments As PagedList(Of UserQuestionComment)
   Public Property Comments() As PagedList(Of UserQuestionComment)
      Get
         Return _comments
      End Get
      Set(ByVal value As PagedList(Of UserQuestionComment))
         _comments = value
      End Set
   End Property

End Class
