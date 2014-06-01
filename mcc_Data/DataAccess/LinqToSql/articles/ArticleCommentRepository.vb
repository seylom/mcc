Public Class ArticleCommentRepository
   Implements IArticleCommentRepository


   Private _mdc As MCCDataContext

   Public Sub New()
      Me.New(New MCCDataContext)
   End Sub

   Public Sub New(ByVal db As MCCDataContext)
      _mdc = db
   End Sub

   Public Sub DeleteComment(ByVal commentId As Integer) Implements IArticleCommentRepository.DeleteComment
      If commentId > 0 Then
         Dim q = _mdc.mcc_Comments.Where(Function(p) p.CommentID = commentId).FirstOrDefault()
         If q Is Nothing Then
            Return
         End If

         _mdc.mcc_Comments.DeleteOnSubmit(q)
         _mdc.SubmitChanges()
      End If
   End Sub

   Public Sub DeleteComments(ByVal commentIds() As Integer) Implements IArticleCommentRepository.DeleteComments
      If commentIds IsNot Nothing Then
         Dim q = _mdc.mcc_Comments.Where(Function(p) commentIds.Contains(p.CommentID))
         If q Is Nothing Then
            Return
         End If

         _mdc.mcc_Comments.DeleteAllOnSubmit(q)
         _mdc.SubmitChanges()
      End If
   End Sub

   Public Function GetArticleComments() As System.Linq.IQueryable(Of ArticleComment) Implements IArticleCommentRepository.GetArticleComments
      Dim q = From c As mcc_Comment In _mdc.mcc_Comments _
              Select New ArticleComment With { _
                     .CommentID = c.CommentID, _
                     .AddedDate = c.AddedDate, _
                     .AddedBy = c.AddedBy, _
                     .AddedByEmail = c.AddedByEmail, _
                     .AddedByIP = c.AddedByIP, _
                     .Body = c.Body, _
                     .ArticleID = c.ArticleID, _
                     .Flag = c.flag}

      Return q
   End Function

   Public Sub InsertComment(ByVal comment As ArticleComment) Implements IArticleCommentRepository.InsertComment
      If comment IsNot Nothing Then
         Dim cm As New mcc_Comment
         With cm
            .AddedDate = comment.AddedDate
            .AddedBy = comment.AddedBy
            .AddedByEmail = comment.AddedByEmail
            .AddedByIP = comment.AddedByIP
            .Body = comment.Body
            .ArticleID = comment.ArticleID
            .flag = comment.Flag
         End With

         _mdc.mcc_Comments.InsertOnSubmit(cm)
         _mdc.SubmitChanges()
      End If
   End Sub

   Public Sub UpdateComment(ByVal commentToUpdate As ArticleComment) Implements IArticleCommentRepository.UpdateComment
      If commentToUpdate IsNot Nothing Then
         Dim q = _mdc.mcc_Comments.Where(Function(p) p.CommentID = commentToUpdate.CommentID).FirstOrDefault()
         If q Is Nothing Then
            Return
         End If

         _mdc.SubmitChanges()
      End If
   End Sub
End Class
