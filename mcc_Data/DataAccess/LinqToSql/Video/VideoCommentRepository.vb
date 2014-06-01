Public Class VideoCommentRepository
   Implements IVideoCommentRepository


   Private _mdc As MCCDataContext

   Public Sub New()
      Me.New(new MCCDataContext)
   End Sub

   Public Sub New(ByVal db As MCCDataContext)
      _mdc = db
   End Sub

   Public Sub DeleteComment(ByVal commentId As Integer) Implements IVideoCommentRepository.DeleteComment
      If commentId > 0 Then
         Dim q = _mdc.mcc_VideoComments.Where(Function(p) p.CommentID = commentId).FirstOrDefault()
         If q Is Nothing Then
            Return
         End If

         _mdc.mcc_VideoComments.DeleteOnSubmit(q)
         _mdc.SubmitChanges()
      End If
   End Sub

   Public Sub DeleteComments(ByVal commentIds() As Integer) Implements IVideoCommentRepository.DeleteComments
      If commentIds IsNot Nothing Then
         Dim q = _mdc.mcc_VideoComments.Where(Function(p) commentIds.Contains(p.CommentID)).AsEnumerable
         If q Is Nothing Then
            Return
         End If

         _mdc.mcc_VideoComments.DeleteAllOnSubmit(q)
         _mdc.SubmitChanges()
      End If
   End Sub

   Public Function GetVideoComments() As System.Linq.IQueryable(Of VideoComment) Implements IVideoCommentRepository.GetVideoComments
      Dim q = From c As mcc_VideoComment In _mdc.mcc_VideoComments _
              Select New VideoComment With { _
                     .CommentID = c.CommentID, _
                     .AddedDate = c.AddedDate, _
                     .AddedBy = c.Addedby, _
                     .AddedByEmail = c.AddedbyEmail, _
                     .AddedByIP = c.AddedbyIP, _
                     .Body = c.Body, _
                     .VideoID = c.VideoID, _
                     .Flag = c.flag}

      Return q
   End Function

   Public Sub InsertComment(ByVal comment As VideoComment) Implements IVideoCommentRepository.InsertComment
      If comment IsNot Nothing Then
         Dim cm As New mcc_VideoComment
         With cm
            .AddedDate = comment.AddedDate
            .Addedby = comment.AddedBy
            .AddedbyEmail = comment.AddedByEmail
            .AddedbyIP = comment.AddedByIP
            .Body = comment.Body
            .VideoID = comment.VideoID
            .flag = comment.Flag
         End With

         _mdc.mcc_VideoComments.InsertOnSubmit(cm)
         _mdc.SubmitChanges()
      End If
   End Sub

   Public Sub UpdateComment(ByVal commentToUpdate As VideoComment) Implements IVideoCommentRepository.UpdateComment
      If commentToUpdate IsNot Nothing Then
         Dim q = _mdc.mcc_VideoComments.Where(Function(p) p.CommentID = commentToUpdate.CommentID).FirstOrDefault()
         If q Is Nothing Then
            Return
         End If

         _mdc.SubmitChanges()
      End If
   End Sub
End Class
