Public Class baseViewModel

   Private _title As String = "Practical financial solutions for today's middle class"
   Public Property PageTitle() As String
      Get
         Return _title
      End Get
      Set(ByVal value As String)
         _title = value
      End Set
   End Property

   Private _metaDescription As String = "Practical financial solutions for today's middle class"
   Public Property MetaDescription() As String
      Get
         Return _metaDescription
      End Get
      Set(ByVal value As String)
         _metaDescription = value
      End Set
   End Property

   Private _metaKeywords As String = "middle class crunch,financial classes,middle class,credit,housing market,financial advice"
   Public Property MetaKeywords() As String
      Get
         Return _metaKeywords
      End Get
      Set(ByVal value As String)
         _metaKeywords = value
      End Set
   End Property


   Private _messages As List(Of String) = New List(Of String)
   Public Property Messages() As List(Of String)
      Get
         Return _messages
      End Get
      Set(ByVal value As List(Of String))
         _messages = value
      End Set
   End Property


   'Private _size As Integer = 10
   'Public Property Size() As Integer
   '   Get
   '      Return _size
   '   End Get
   '   Set(ByVal value As Integer)
   '      _size = value
   '   End Set
   'End Property

   Private _menu As MenuViewModel
   Public Property Menu() As MenuViewModel
      Get
         Return _menu
      End Get
      Set(ByVal value As MenuViewModel)
         _menu = value
      End Set
   End Property


End Class
