Imports MCC.Data

Public Interface IUserResetCodesRepository
   Function UsercodeExists(ByVal usercode As String) As Boolean
   Function ValidateUsercode(ByVal userId As Guid, ByVal usercode As String) As Boolean
   Function GetUsercode(ByVal userId As Guid) As String
   Function SaveResetCode(ByVal UserId As Guid, ByVal userCode As String) As Boolean
   Sub DeleteResetCode(ByVal userId As Guid)
End Interface

Public Class UserResetCodesRepository
   Implements IUserResetCodesRepository

   Private _mdc As MCCDataContext

   Public Sub New()
      _mdc = New MCCDataContext()
   End Sub

   Public Function UsercodeExists(ByVal usercode As String) As Boolean Implements IUserResetCodesRepository.UsercodeExists
      Return _mdc.mcc_ResetCodes.Count(Function(p) p.Usercode) > 0
   End Function


   Public Function ValidateUsercode(ByVal userId As Guid, ByVal usercode As String) As Boolean Implements IUserResetCodesRepository.ValidateUsercode
      Return _mdc.mcc_ResetCodes.Count(Function(p) p.UserId = userId AndAlso p.Usercode = usercode) > 0
   End Function

   Public Function GetUsercode(ByVal userId As Guid) As String Implements IUserResetCodesRepository.GetUsercode
      ' does the user have a recent day
      Dim usercode As String
      Dim uc As mcc_ResetCode = (From it As mcc_ResetCode In _mdc.mcc_ResetCodes Where it.UserId = userId).FirstOrDefault

      If uc IsNot Nothing Then
         Dim addedDate As DateTime = uc.AddedDate
         If addedDate.AddDays(2) < DateTime.Now Then
            _mdc.mcc_ResetCodes.DeleteOnSubmit(uc)
            _mdc.SubmitChanges()
            usercode = AutoGenerateUsercode()
         Else
            usercode = uc.Usercode
         End If
      Else
         usercode = AutoGenerateUsercode()
      End If
      Return usercode
   End Function

   Private Function AutoGenerateUsercode() As String
      Dim success As Boolean = False
      Dim usercode As String = String.Empty
      Do
         usercode = routines.GenerateName(New Random(), 12)
         If Not UsercodeExists(usercode) Then
            success = True
         End If
      Loop While success = False
      Return usercode
   End Function

   Public Function SaveResetCode(ByVal UserId As Guid, ByVal userCode As String) As Boolean Implements IUserResetCodesRepository.SaveResetCode
      If String.IsNullOrEmpty(userCode) Then
         Return False
      End If

      If Not ValidateUsercode(UserId, userCode) Then
         Dim uc As New mcc_ResetCode
         uc.Usercode = userCode
         uc.UserId = UserId
         uc.AddedDate = DateTime.Now

         _mdc.mcc_ResetCodes.InsertOnSubmit(uc)
         _mdc.SubmitChanges()
      End If
   End Function


   Public Sub DeletetResetCode(ByVal userId As Guid) Implements IUserResetCodesRepository.DeleteResetCode
      Dim uc As mcc_ResetCode = (From it As mcc_ResetCode In _mdc.mcc_ResetCodes Where it.UserId = userId)
      If uc IsNot Nothing Then
         _mdc.mcc_ResetCodes.DeleteOnSubmit(uc)
         _mdc.SubmitChanges()
      End If
   End Sub

End Class
