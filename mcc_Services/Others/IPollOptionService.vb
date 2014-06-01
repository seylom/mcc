Imports MCC.Data
Imports Webdiyer.WebControls.Mvc

Public Interface IPollOptionService
   Function GetPollOptionCount() As Integer
   Function GetPollOptionCount(ByVal criteria As String) As Integer
    Function GetPollOptions() As List(Of PollOption)
    Function GetPollOptions(ByVal criteria As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer) As PagedList(Of PollOption)
    Function GetPollOptions(ByVal startrowindex As Integer, ByVal maximumrows As Integer) As PagedList(Of PollOption)
    Function GetPollOptionsByPollId(ByVal pollId As Integer) As List(Of PollOption)
   Function GetPollOptionById(ByVal optionId As Integer) As PollOption
   Function GetTotalVotesByPollOptionId(ByVal optionId As Integer) As Integer
   Sub InsertPollOption(ByVal p As PollOption)
   Function VotePollOptions(ByVal optionId As Integer) As Integer
   Sub UpdatePollOptions(ByVal p As PollOption)
   Sub DeletePollOptions(ByVal optionId As Integer)
End Interface
