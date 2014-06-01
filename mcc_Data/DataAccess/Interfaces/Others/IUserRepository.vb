Public Interface IUserRepository
   Function GetUsers() As IQueryable(Of SiteUser)
End Interface
