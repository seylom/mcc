Imports MCC.Data
Public Class ResetCodeService
   Implements IResetCodeService

   Private _resetCodeRepository As IResetCodeRepository

   Public Sub New()
      Me.New(New ResetCodeRepository())
   End Sub

   Public Sub New(ByVal quoteRepo As IResetCodeRepository)
      _resetCodeRepository = quoteRepo
   End Sub

   Public Function UsercodeExists(ByVal usercode As String) As Boolean Implements IResetCodeService.UsercodeExists
      Return _resetCodeRepository.GetUserResetCodes.Count(Function(p) p.Usercode = usercode) > 0
   End Function


   Public Function ValidateUsercode(ByVal userId As Guid, ByVal usercode As String) As Boolean Implements IResetCodeService.ValidateUsercode
      Return _resetCodeRepository.GetUserResetCodes.Count(Function(p) p.UserID = userId AndAlso p.Usercode = usercode) > 0
   End Function

   Public Function AssignUsercode(ByVal userId As Guid) As String Implements IResetCodeService.AssignUsercode
      ' does the user have a recent day
      Dim usercode As String
      Dim uc As UserResetCode = _resetCodeRepository.GetUserResetCodes.Where(Function(it) it.UserID = userId).FirstOrDefault

      If uc IsNot Nothing Then
         Dim addedDate As DateTime = uc.AddedDate
         If addedDate.AddDays(2) < DateTime.Now Then
            _resetCodeRepository.DeleteUserResetCode(uc.UserID)
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
         usercode = GenerateName(New Random(), 12)
         If Not UsercodeExists(usercode) Then
            success = True
         End If
      Loop While success = False
      Return usercode
   End Function

   Public Function SaveResetCode(ByVal UserId As Guid, ByVal userCode As String) As Boolean Implements IResetCodeService.SaveResetCode
      If String.IsNullOrEmpty(userCode) Then
         Return False
      End If

      If Not ValidateUsercode(UserId, userCode) Then
         Dim uc As New UserResetCode
         uc.Usercode = userCode
         uc.UserId = UserId
         uc.AddedDate = DateTime.Now
         _resetCodeRepository.InsertUserResetCode(uc)
      End If
   End Function


   Public Sub DeleteResetCode(ByVal userId As Guid) Implements IResetCodeService.DeleteResetCode
      _resetCodeRepository.DeleteUserResetCode(userId)
   End Sub

End Class
