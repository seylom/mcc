Public Class AdviceCommentRepository
   Implements IAdviceCommentRepository


   Private _mdc As MCCDataContext

   Public Sub New()
      Me.New(New MCCDataContext)
   End Sub

   Public Sub New(ByVal db As MCCDataContext)
      _mdc = db
   End Sub

   Public Sub DeleteComment(ByVal commentId As Integer) Implements IAdviceCommentRepository.DeleteComment
      If commentId > 0 Then
         Dim q = _mdc.mcc_AdviceComments.Where(Function(p) p.CommentID = commentId).FirstOrDefault()
         If q Is Nothing Then
            Return
         End If

         _mdc.mcc_AdviceComments.DeleteOnSubmit(q)
         _mdc.SubmitChanges()
      End If
   End Sub

   Public Sub DeleteComments(ByVal commentIds() As Integer) Implements IAdviceCommentRepository.DeleteComments
      If commentIds IsNot Nothing Then
         Dim q = _mdc.mcc_AdviceComments.Where(Function(p) commentIds.Contains(p.CommentID)).AsEnumerable
         If q Is Nothing Then
            Return
         End If

         _mdc.mcc_AdviceComments.DeleteAllOnSubmit(q)
         _mdc.SubmitChanges()
      End If
   End Sub

   Public Function GetAdviceComments() As System.Linq.IQueryable(Of AdviceComment) Implements IAdviceCommentRepository.GetAdviceComments
      Dim q = From c As mcc_AdviceComment In _mdc.mcc_AdviceComments _
              Select New AdviceComment With { _
                     .CommentID = c.CommentID, _
                     .AddedDate = c.AddedDate, _
                     .AddedBy = c.AddedBy, _
                     .AddedByEmail = c.AddedByEmail, _
                     .AddedByIP = c.AddedbyIP, _
                     .Body = c.Body, _
                     .AdviceID = c.AdviceId, _
                     .Flag = c.flag}

      Return q
   End Function

   Public Sub InsertComment(ByVal comment As AdviceComment) Implements IAdviceCommentRepository.InsertComment
      If comment IsNot Nothing Then
         Dim cm As New mcc_AdviceComment
         With cm
            .AddedDate = comment.AddedDate
            .AddedBy = comment.AddedBy
            .AddedByEmail = comment.AddedByEmail
            .AddedByIP = comment.AddedByIP
            .Body = comment.Body
            .AdviceID = comment.AdviceID
            .flag = comment.Flag
         End With

         _mdc.mcc_AdviceComments.InsertOnSubmit(cm)
         _mdc.SubmitChanges()
      End If
   End Sub

   Public Sub UpdateComment(ByVal commentToUpdate As AdviceComment) Implements IAdviceCommentRepository.UpdateComment
      If commentToUpdate IsNot Nothing Then
         Dim q = _mdc.mcc_AdviceComments.Where(Function(p) p.CommentID = commentToUpdate.CommentID).FirstOrDefault()
         If q Is Nothing Then
            Return
         End If

         _mdc.SubmitChanges()
      End If
   End Sub
End Class
