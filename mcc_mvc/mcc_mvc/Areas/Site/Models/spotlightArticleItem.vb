
Imports MCC.Data

Public Class spotlightArticleItem

   Private _article As Article
   Public ReadOnly Property Article() As Article
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

   Public Sub New(ByVal art As Article, ByVal index As Integer)
      _article = art
      _itemIndex = index
   End Sub
End Class
