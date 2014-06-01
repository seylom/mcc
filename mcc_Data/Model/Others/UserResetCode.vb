Public Class UserResetCode

   Private _userID As Guid
   Public Property UserID() As Guid
      Get
         Return _userID
      End Get
      Set(ByVal value As Guid)
         _userID = value
      End Set
   End Property

   Private _userCode As String
   Public Property Usercode() As String
      Get
         Return _userCode
      End Get
      Set(ByVal value As String)
         _userCode = value
      End Set
   End Property

   Private _addedDate As DateTime
   Public Property AddedDate() As DateTime
      Get
         Return _addedDate
      End Get
      Set(ByVal value As DateTime)
         _addedDate = value
      End Set
   End Property

End Class
