Public Module DB
   Private _dataContext As MCCDataContext
   Public ReadOnly Property DataContext() As MCCDataContext
      Get
         If _dataContext Is Nothing Then
            _dataContext = New MCCDataContext
         End If
         Return _dataContext
      End Get
   End Property



   Private _UserDataContext As umcDataContext
   Public ReadOnly Property MembershipContext() As umcDataContext
      Get
         If _UserDataContext Is Nothing Then
            _UserDataContext = New umcDataContext
         End If
         Return _UserDataContext
      End Get
   End Property


End Module
