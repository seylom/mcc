Public Class ResetCodeRepository
   Implements IResetCodeRepository

   Private _mdc As MCCDataContext
   Public Sub New()
      Me.New(new MCCDataContext)
   End Sub

   Public Sub New(ByVal db As MCCDataContext)
      _mdc = db
   End Sub
   Public Function GetResetCodes() As IQueryable(Of UserResetCode) Implements IResetCodeRepository.GetUserResetCodes
      Dim q = From it As mcc_ResetCode In _mdc.mcc_ResetCodes _
               Select New UserResetCode With {.Usercode = it.Usercode, _
                                          .UserID = it.UserId, _
                                          .AddedDate = it.AddedDate}
      Return q
   End Function
   Public Sub DeleteResetCode(ByVal id As Guid) Implements IResetCodeRepository.DeleteUserResetCode
      Dim q = _mdc.mcc_ResetCodes.Where(Function(p) p.UserId = id).FirstOrDefault
      If q IsNot Nothing Then
         _mdc.mcc_ResetCodes.DeleteOnSubmit(q)
         _mdc.SubmitChanges()
      End If
   End Sub
   Public Sub DeleteResetCodes(ByVal ids() As Guid) Implements IResetCodeRepository.DeleteUserResetCodes
      If ids IsNot Nothing AndAlso ids.Count > 0 Then
         Dim q = _mdc.mcc_ResetCodes.Where(Function(p) ids.Contains(p.UserId))
         If q IsNot Nothing Then
            _mdc.mcc_ResetCodes.DeleteAllOnSubmit(q)
            _mdc.SubmitChanges()
         End If
      End If
   End Sub
   Public Sub InsertResetCode(ByVal rc As UserResetCode) Implements IResetCodeRepository.InsertUserResetCode
      If rc IsNot Nothing Then
         Dim q As New mcc_ResetCode
         With q
            .AddedDate = rc.AddedDate
            .UserId = rc.UserID
            .Usercode = rc.Usercode
         End With

         _mdc.mcc_ResetCodes.InsertOnSubmit(q)
         _mdc.SubmitChanges()
      End If
   End Sub
End Class
