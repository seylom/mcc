Public Class BadWord

   Private _badWordID As String
   Public Property BadWordID() As String
      Get
         Return _badWordID
      End Get
      Set(ByVal value As String)
         _badWordID = value
      End Set
   End Property


   Private _word As String
   Public Property Word() As String
      Get
         Return _word
      End Get
      Set(ByVal value As String)
         _word = value
      End Set
   End Property




End Class
