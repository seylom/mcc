Public Class ProfileGroupLocation
   Inherits ProfileGroupBase

   Public Property City() As String
      Get
         Return CType(Me.GetPropertyValue("City"), String)
      End Get
      Set(ByVal value As String)
         Me.SetPropertyValue("City", value)
      End Set
   End Property


   Private _state As String
   Public Property State() As String
      Get
         Return CType(Me.GetPropertyValue("State"), String)
      End Get
      Set(ByVal value As String)
         Me.SetPropertyValue("State", value)
      End Set
   End Property


   Private _country As String
   Public Property Country() As String
      Get
         Return CType(Me.GetPropertyValue("Country"), String)
      End Get
      Set(ByVal value As String)
         Me.SetPropertyValue("Country", value)
      End Set
   End Property


   Private _imageUrl As String
   Public Property ImageUrl() As String
      Get
         Return _imageUrl
      End Get
      Set(ByVal value As String)
         _imageUrl = value
      End Set
   End Property

End Class
