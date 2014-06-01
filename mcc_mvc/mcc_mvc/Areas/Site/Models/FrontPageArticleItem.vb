
Imports MCC.Data

Public Class FrontPageArticleItem
   Inherits baseViewModel

   Private _article As ArticleAdv
   Public ReadOnly Property Article() As ArticleAdv
      Get
         Return _article
      End Get
   End Property


   Private _itemIndex As Integer
   Public ReadOnly Property ItemIndex() As Integer
      Get
         Return _itemIndex
      End Get
   End Property



   Public Sub New(ByVal art As ArticleAdv, ByVal index As Integer)
      _article = art
      _itemIndex = index
   End Sub

End Class
