Public Class TipViewData



   Public Sub New()

   End Sub



   Private _body As String
   Public Property Body() As String
      Get
         Return _body
      End Get
      Set(ByVal value As String)
         _body = value
      End Set
   End Property



   Private _addedDate As String
   Public Property AddedDate() As String
      Get
         Return _addedDate
      End Get
      Set(ByVal value As String)
         _addedDate = value
      End Set
   End Property


End Class
