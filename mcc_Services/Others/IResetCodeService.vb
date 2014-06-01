Imports MCC.Data


Public Interface IResetCodeService
   Function UsercodeExists(ByVal usercode As String) As Boolean
   Function ValidateUsercode(ByVal userId As Guid, ByVal usercode As String) As Boolean
   Function AssignUsercode(ByVal userId As Guid) As String
   Function SaveResetCode(ByVal UserId As Guid, ByVal userCode As String) As Boolean
   Sub DeleteResetCode(ByVal userId As Guid)
End Interface
