Public Interface IFeedbackRepository
   Function GetFeedbacks() As IQueryable(Of Feedback)
   Function InsertFeedback(ByVal op As Feedback) As Integer
   Sub UpdateFeedback(ByVal fb As Feedback)
   Sub DeleteFeedback(ByVal fbId As Integer)
   Sub DeleteFeedbacks(ByVal fbIds() As Integer)
End Interface
