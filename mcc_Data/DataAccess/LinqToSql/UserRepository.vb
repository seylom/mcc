Public Class UserRepository
   Implements IUserRepository

   Private _umc As umcDataContext


   Public Sub New()
      Me.New(DB.MembershipContext)
   End Sub

   Public Sub New(ByVal dc As umcDataContext)
      _umc = dc
   End Sub
   Public Function GetUsers() As IQueryable(Of SiteUser) Implements IUserRepository.GetUsers
      Dim q = From uc As aspnet_Membership In _umc.aspnet_Memberships _
              Select New SiteUser With {.Username = uc.aspnet_User.UserName, _
                                        .UserID = uc.UserId, _
                                        .CreationDate = uc.CreateDate, _
                                        .LastActivityDate = uc.LastLoginDate, _
                                        .Email = uc.Email, _
                                        .IsApproved = uc.IsApproved, _
                                        .IsLockedOut = uc.IsLockedOut}

      Return q
   End Function
End Class
