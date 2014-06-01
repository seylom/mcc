Public Class BannedIP

   Private _bannedIpID As Integer
   Public Property BannedIpID() As Integer
      Get
         Return _bannedIpID
      End Get
      Set(ByVal value As Integer)
         _bannedIpID = value
      End Set
   End Property


   Private _mask As String
   Public Property Mask() As String
      Get
         Return _mask
      End Get
      Set(ByVal value As String)
         _mask = value
      End Set
   End Property



   Private _since As DateTime
   Public Property Since() As DateTime
      Get
         Return _since
      End Get
      Set(ByVal value As DateTime)
         _since = value
      End Set
   End Property



End Class
