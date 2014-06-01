Public Interface IPollOptionRepository
   Function GetPollOptions() As IQueryable(Of PollOption)
   Function InsertPollOption(ByVal op As PollOption) As Integer
   Sub UpdatePollOption(ByVal op As PollOption)
   Sub DeletePollOption(ByVal opId As Integer)
   Sub DeletePollOptions(ByVal opIds() As Integer)
End Interface
