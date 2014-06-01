Public Class UserAnswerCommentRepository
   Implements IUserAnswerCommentRepository
   Private _mdc As MCCDataContext

   Public Sub New()
      Me.New(new MCCDataContext)
   End Sub

   Public Sub New(ByVal dc As MCCDataContext)
      _mdc = dc
   End Sub
   Public Function GetUserAnswerComments() As IQueryable(Of UserAnswerComment) Implements IUserAnswerCommentRepository.GetUserAnswerComments
      Dim q = From p As mcc_UserAnswerComment In _mdc.mcc_UserAnswerComments Select _
              New UserAnswerComment With {.UserAnswerCommentID = p.UserAnswerCommentId, _
                                            .AddedDate = p.AddedDate, _
                                            .AddedBy = p.AddedBy, _
                                            .body = p.Body, _
                                            .UserAnswerID = p.UserAnswerId}
      Return q
   End Function

   Public Function InsertUserAnswerComment(ByVal uqc As UserAnswerComment) As Integer Implements IUserAnswerCommentRepository.InsertUserAnswerComment
      Dim idx As Integer = 0
      If uqc IsNot Nothing Then
         Dim q As New mcc_UserAnswerComment
         With q
            q.AddedDate = uqc.AddedDate
            q.AddedBy = uqc.AddedBy
            q.Body = uqc.Body
            q.UserAnswerId = uqc.UserAnswerID
         End With

         _mdc.mcc_UserAnswerComments.InsertOnSubmit(q)
         _mdc.SubmitChanges()

         idx = q.UserAnswerCommentId
      End If
      Return idx
   End Function
   Public Sub UpdateUserAnswerComment(ByVal uqc As UserAnswerComment) Implements IUserAnswerCommentRepository.UpdateUserAnswerComment
      If uqc IsNot Nothing Then
         Dim q = _mdc.mcc_UserAnswerComments.Where(Function(p) p.UserAnswerCommentId = uqc.UserAnswerCommentID).FirstOrDefault()
         If q IsNot Nothing Then
            With q
               q.Body = uqc.Body
               'q.UserAnswerId = uqc.UserAnswerID
            End With
            _mdc.SubmitChanges()
         End If
      End If
   End Sub
   Public Sub DeleteUserAnswerComment(ByVal uqcId As Integer) Implements IUserAnswerCommentRepository.DeleteUserAnswerComment
      If uqcId > 0 Then
         Dim q = _mdc.mcc_UserAnswerComments.Where(Function(p) p.UserAnswerCommentId = uqcId).FirstOrDefault()
         If q IsNot Nothing Then
            _mdc.mcc_UserAnswerComments.DeleteOnSubmit(q)
            _mdc.SubmitChanges()
         End If
      End If
   End Sub
   Public Sub DeleteUserAnswerComments(ByVal uqcId As Integer()) Implements IUserAnswerCommentRepository.DeleteUserAnswerComments
      If uqcId IsNot Nothing AndAlso uqcId.Count > 0 Then
         Dim q = _mdc.mcc_UserAnswerComments.Where(Function(p) uqcId.Contains(p.UserAnswerCommentId))
         If q IsNot Nothing Then
            _mdc.mcc_UserAnswerComments.DeleteAllOnSubmit(q)
            _mdc.SubmitChanges()
         End If
      End If
   End Sub
End Class
