Public Interface IPollRepository
   Function GetPolls() As IQueryable(Of Poll)
   Function InsertPoll(ByVal op As Poll) As Integer
   Sub UpdatePoll(ByVal op As Poll)
   Sub DeletePoll(ByVal opId As Integer)
   Sub DeletePolls(ByVal opIds() As Integer)
End Interface
