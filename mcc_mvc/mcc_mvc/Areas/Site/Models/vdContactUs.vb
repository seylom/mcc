Public Class vdContactUs
   Inherits baseViewModel
   Private _subject As String
   Public Property Subject() As String
      Get
         Return _subject
      End Get
      Set(ByVal value As String)
         _subject = value
      End Set
   End Property

   Private _email As String
   Public Property Email() As String
      Get
         Return _email
      End Get
      Set(ByVal value As String)
         _email = value
      End Set
   End Property

   Private _body As String
   Public Property body() As String
      Get
         Return _body
      End Get
      Set(ByVal value As String)
         _body = value
      End Set
   End Property

   Private _name As String
   Public Property Name() As String
      Get
         Return _name
      End Get
      Set(ByVal value As String)
         _name = value
      End Set
   End Property

End Class
