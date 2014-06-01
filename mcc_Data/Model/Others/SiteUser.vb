Public Enum UserFilterType As Integer
   Username = 0
   Email = 1
End Enum


Public Class SiteUser
   Inherits DataEntity


   Public Property Username() As String
   Public Property CreationDate() As DateTime
   Public Property UserID() As Guid

    Public Property Email() As String
   Public Property LastActivityDate() As DateTime
   Public Property IsApproved() As Boolean
   Public Property IsOnline() As Boolean
    Public Property IsLockedOut() As Boolean


End Class
