Imports MCC.Data
Imports Webdiyer.WebControls.Mvc

Public Interface IPollService
   Function GetPollsCount() As Integer
   Function GetPollsCount(ByVal criteria As String) As Integer
   Function GetPolls() As List(Of Poll)
   Function GetPolls(ByVal criteria As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer) As PagedList(Of Poll)
   Function GetPolls(ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "questionText") As PagedList(Of Poll)
   Function GetPollById(ByVal id As Integer) As Poll
   Function GetTotalVotesById(ByVal id As Integer) As Integer
   Sub InsertPoll(ByVal p As Poll)
   Sub UpdatePoll(ByVal p As Poll)
   Sub DeletePoll(ByVal PollId As Integer)
End Interface
