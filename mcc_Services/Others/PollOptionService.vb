Imports MCC.Data
Imports Webdiyer.WebControls.Mvc
Public Class PollOptionService
   Inherits CacheObject
   Implements IPollOptionService


   Private _pollOptionRepo As IPollOptionRepository
   Private _pollRepo As IPollRepository

   Public Sub New()
      Me.New(New PollOptionRepository(), New PollRepository())
   End Sub

   Public Sub New(ByVal optionRepo As IPollOptionRepository, ByVal pollRepo As IPollRepository)
      _pollOptionRepo = optionRepo
      _pollRepo = pollRepo
   End Sub

   Public Function GetPollOptionCount() As Integer Implements IPollOptionService.GetPollOptionCount
      Dim key As String = "pollOptions_pollOptionCount"
      If Cache(key) IsNot Nothing Then
         Return CInt(Cache(key))
      Else
         Dim it As Integer = _pollOptionRepo.GetPollOptions.Count()
         CacheData(key, it)
         Return it
      End If
   End Function

   Public Function GetPollOptionCount(ByVal criteria As String) As Integer Implements IPollOptionService.GetPollOptionCount
      If Not String.IsNullOrEmpty(criteria) Then
         Dim key As String = "pollOptions_pollOptionCount_" & criteria & "_"
         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            Dim it As Integer = _pollOptionRepo.GetPollOptions.Count(Function(p) p.OptionText.Contains(criteria))
            CacheData(key, it)
            Return it
         End If
      Else
         Return GetPollOptionCount()
      End If
   End Function


   Public Function GetpollOptions() As List(Of PollOption) Implements IPollOptionService.GetpollOptions
      Dim key As String = "pollOptions_pollOptions_"
      If Cache(key) IsNot Nothing Then
         Dim li As List(Of PollOption) = DirectCast(Cache(key), List(Of PollOption))
         Return li
      Else
         Dim li As List(Of PollOption) = _pollOptionRepo.GetPollOptions.ToList
         CacheData(key, li)
         Return li
      End If
   End Function

    Public Function GetPollOptions(ByVal criteria As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer) As PagedList(Of PollOption) Implements IPollOptionService.GetpollOptions
        If Not String.IsNullOrEmpty(criteria) Then
            Dim key As String = "pollOptions_pollOptions_" & criteria & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
            If Cache(key) IsNot Nothing Then
                Return DirectCast(Cache(key), PagedList(Of PollOption))
            Else
                Dim li As PagedList(Of PollOption) = _pollOptionRepo.GetPollOptions.Where(Function(it) it.OptionText.Contains(criteria)).ToPagedList(startrowindex, maximumrows)
                CacheData(key, li)
                Return li
            End If
        Else
            Return GetPollOptions(startrowindex, maximumrows)
        End If
    End Function

    Public Function GetPollOptions(ByVal startrowindex As Integer, ByVal maximumrows As Integer) As PagedList(Of PollOption) Implements IPollOptionService.GetPollOptions
        Dim key As String = "pollOptions_pollOptions_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
        If Cache(key) IsNot Nothing Then
            Dim li As PagedList(Of PollOption) = DirectCast(Cache(key), PagedList(Of PollOption))
            Return li
        Else
            Dim li As PagedList(Of PollOption) = _pollOptionRepo.GetPollOptions.ToPagedList(startrowindex, maximumrows)
            CacheData(key, li)
            Return li
        End If
    End Function

   Public Function GetPollOptionsByPollId(ByVal pollId As Integer) As List(Of PollOption) Implements IPollOptionService.GetPollOptionsByPollId
      Dim key As String = "pollOptions_pollOptions_" & pollId.ToString & "_"
      If Cache(key) IsNot Nothing Then
         Dim li As List(Of PollOption) = DirectCast(Cache(key), List(Of PollOption))
         Return li
      Else
         Dim li As List(Of PollOption) = _pollOptionRepo.GetPollOptions.Where(Function(it) it.PollID = pollId).ToList
         CacheData(key, li)
         Return li
      End If
   End Function

   Public Function GetPollOptionById(ByVal optionId As Integer) As PollOption Implements IPollOptionService.GetPollOptionById
      Dim key As String = "pollOptions_pollOptions_" & optionId.ToString & "_"
      If Cache(key) IsNot Nothing Then
         Dim fb As PollOption = DirectCast(Cache(key), PollOption)
         Return fb
      Else
         Dim fb As PollOption = _pollOptionRepo.GetPollOptions.Where(Function(it) it.PollOptionID = optionId).FirstOrDefault
         CacheData(key, fb)
         Return fb
      End If
   End Function


   Public Function GetTotalVotesByPollOptionId(ByVal optionId As Integer) As Integer Implements IPollOptionService.GetTotalVotesByPollOptionId
      Dim key As String = "pollOptions_pollsvote_" & optionId.ToString & "_"
      If Cache(key) IsNot Nothing Then
         Return CInt(Cache(key))
      Else
         Dim q = _pollOptionRepo.GetPollOptions.Where(Function(p) p.PollOptionID = optionId).Select(Function(t) t.PollID).FirstOrDefault()
         If q <= 0 Then
            Return 0
         End If

         Dim votes As Integer = (Aggregate it In _pollOptionRepo.GetPollOptions Where it.PollID = q Into Sum(it.Votes))
         CacheData(key, votes)
         Return votes
      End If
   End Function

   Public Sub InsertPollOption(ByVal p As PollOption) Implements IPollOptionService.InsertPollOption
      If p IsNot Nothing Then
         p.AddedBy = CurrentUserName
         p.AddedDate = DateTime.Now
         p.Votes = 0

         _pollOptionRepo.InsertPollOption(p)
         CacheObject.PurgeCacheItems("pollOptions_")
      End If
   End Sub

   Public Function VotePollOptions(ByVal optionId As Integer) As Integer Implements IPollOptionService.VotePollOptions
      If optionId > 0 Then
         Dim q As PollOption = _pollOptionRepo.GetPollOptions.Where(Function(p) p.PollOptionID = optionId).FirstOrDefault()
         If q IsNot Nothing Then
            q.Votes += 1

            _pollOptionRepo.UpdatePollOption(q)

            CacheObject.PurgeCacheItems("pollOptions_pollOptions_")
            CacheObject.PurgeCacheItems("polls_pollsvotes_" & q.PollID.ToString)
         End If
      End If
   End Function

   Public Sub UpdatePollOptions(ByVal p As PollOption) Implements IPollOptionService.UpdatePollOptions
      If p IsNot Nothing Then
         _pollOptionRepo.UpdatePollOption(p)
         CacheObject.PurgeCacheItems("pollOptions_pollOptions_")
      End If
   End Sub

   Public Sub DeletePollOptions(ByVal optionId As Integer) Implements IPollOptionService.DeletePollOptions
      If optionId > 0 Then
         _pollOptionRepo.DeletePollOption(optionId)
         CacheObject.PurgeCacheItems("pollOptions_pollOptions_")
      End If
   End Sub

   Protected Sub CacheData(ByVal key As String, ByVal data As Object)
      If data IsNot Nothing Then
         CacheObject.Cache.Insert(key, data, Nothing, DateTime.Now.AddSeconds(3600), TimeSpan.Zero)
      End If
   End Sub
End Class
