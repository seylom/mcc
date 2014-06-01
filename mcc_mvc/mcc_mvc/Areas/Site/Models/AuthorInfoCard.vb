Public Class AuthorInfoCard
   Inherits baseViewModel

   Private _avatarUrl As String
   Public ReadOnly Property AvatarUrl() As String
      Get
         Return _avatarUrl
      End Get
   End Property

   Private _about As String
   Public ReadOnly Property About() As String
      Get
         Return _about
      End Get
   End Property

   Public Sub New(ByVal username As String)
      Dim pi As ProfileInfo = ProfileInfo.GetProfile(username)
      Dim muser As MembershipUser = Membership.GetUser(username)
      If muser IsNot Nothing Then
         _avatarUrl = routines.Gravatar(muser.Email, 60)
      End If

      _about = pi.About
   End Sub
End Class
