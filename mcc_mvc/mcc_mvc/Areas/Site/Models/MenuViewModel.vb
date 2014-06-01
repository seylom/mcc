Public Class MenuViewModel

   Private _money As List(Of String)
   Public Property Money() As List(Of String)
      Get
         If _money Is Nothing Then
            _money = New List(Of String)
         End If
         Return _money
      End Get
      Set(ByVal value As List(Of String))
         _money = value
      End Set
   End Property

   Private _stories As List(Of String)
   Public Property Stories() As List(Of String)
      Get
         If _stories Is Nothing Then
            _stories = New List(Of String)
         End If
         Return _stories
      End Get
      Set(ByVal value As List(Of String))
         _stories = value
      End Set
   End Property

   Private _news As List(Of String)
   Public Property News() As List(Of String)
      Get
         If _news Is Nothing Then
            _news = New List(Of String)
         End If
         Return _news
      End Get
      Set(ByVal value As List(Of String))
         _news = value
      End Set
   End Property

   Private _rants As List(Of String)
   Public Property Rants() As List(Of String)
      Get
         If _rants Is Nothing Then
            _rants = New List(Of String)
         End If
         Return _rants
      End Get
      Set(ByVal value As List(Of String))
         _rants = value
      End Set
   End Property

End Class
