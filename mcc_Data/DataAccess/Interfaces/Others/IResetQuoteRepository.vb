Public Interface IResetCodeRepository
   Function GetUserResetCodes() As IQueryable(Of UserResetCode)
   Sub DeleteUserResetCode(ByVal id As Guid)
   Sub DeleteUserResetCodes(ByVal ids() As Guid)
   Sub InsertUserResetCode(ByVal rc As UserResetCode)
End Interface
