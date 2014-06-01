Public Class UserQuestionCommentRepository
   Implements IUserQuestionCommentRepository
   Private _mdc As MCCDataContext

   Public Sub New()
      Me.New(new MCCDataContext)
   End Sub

   Public Sub New(ByVal dc As MCCDataContext)
      _mdc = dc
   End Sub
   Public Function GetUserQuestionComments() As IQueryable(Of UserQuestionComment) Implements IUserQuestionCommentRepository.GetUserQuestionComments
      Dim q = From p As mcc_UserQuestionComment In _mdc.mcc_UserQuestionComments Select _
              New UserQuestionComment With {.UserQuestionCommentID = p.UserQuestionCommentId, _
                                            .AddedDate = p.AddedDate, _
                                            .AddedBy = p.AddedBy, _
                                            .body = p.Body, _
                                            .UserQuestionID = p.UserQuestionId}
      Return q
   End Function

   Public Function InsertUserQuestionComment(ByVal uqc As UserQuestionComment) As Integer Implements IUserQuestionCommentRepository.InsertUserQuestionComment
      Dim idx As Integer = 0
      If uqc IsNot Nothing Then
         Dim q As New mcc_UserQuestionComment
         With q
            q.AddedDate = uqc.AddedDate
            q.AddedBy = uqc.AddedBy
            q.Body = uqc.body
            q.UserQuestionId = uqc.UserQuestionID
         End With

         _mdc.mcc_UserQuestionComments.InsertOnSubmit(q)
         _mdc.SubmitChanges()

         idx = q.UserQuestionCommentId
      End If

      Return idx
   End Function
   Public Sub UpdateUserQuestionComment(ByVal uqc As UserQuestionComment) Implements IUserQuestionCommentRepository.UpdateUserQuestionComment
      If uqc IsNot Nothing Then
         Dim q = _mdc.mcc_UserQuestionComments.Where(Function(p) p.UserQuestionCommentId = uqc.UserQuestionCommentID).FirstOrDefault()
         If q IsNot Nothing Then
            With q
               q.Body = uqc.body
               'q.UserQuestionId = uqc.UserQuestionID
            End With
            _mdc.SubmitChanges()
         End If   
      End If
   End Sub
   Public Sub DeleteUserQuestionComment(ByVal uqcId As Integer) Implements IUserQuestionCommentRepository.DeleteUserQuestionComment
      If uqcId > 0 Then
         Dim q = _mdc.mcc_UserQuestionComments.Where(Function(p) p.UserQuestionCommentId = uqcId).FirstOrDefault()
         If q IsNot Nothing Then
            _mdc.mcc_UserQuestionComments.DeleteOnSubmit(q)
            _mdc.SubmitChanges()
         End If
      End If
   End Sub
   Public Sub DeleteUserQuestionComments(ByVal uqcId As Integer()) Implements IUserQuestionCommentRepository.DeleteUserQuestionComments
      If uqcId IsNot Nothing AndAlso uqcId.Count > 0 Then
         Dim q = _mdc.mcc_UserQuestionComments.Where(Function(p) uqcId.Contains(p.UserQuestionCommentId))
         If q IsNot Nothing Then
            _mdc.mcc_UserQuestionComments.DeleteAllOnSubmit(q)
            _mdc.SubmitChanges()
         End If
      End If
   End Sub
End Class
