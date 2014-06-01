Public Class ActivationViewModel


   Private _approved As Boolean
   Public Property Approved() As Boolean
      Get
         Return _approved
      End Get
      Set(ByVal value As Boolean)
         _approved = value
      End Set
   End Property


   Private _exists As Boolean
   Public Property Exists() As Boolean
      Get
         Return _exists
      End Get
      Set(ByVal value As Boolean)
         _exists = value
      End Set
   End Property


   Private _lockedOut As Boolean = False
   Public Property LockedOut() As Boolean
      Get
         Return _lockedOut
      End Get
      Set(ByVal value As Boolean)
         _lockedOut = value
      End Set
   End Property

End Class
