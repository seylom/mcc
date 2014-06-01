Imports MCC.Data
Imports Webdiyer.WebControls.Mvc
Public Class PollService
   Inherits CacheObject
   Implements IPollService

   Private _pollRepo As IPollRepository
   Private _pollOptionRepo As IPollOptionRepository

   Public Sub New()
      Me.New(New PollRepository(), New PollOptionRepository)
   End Sub

   Public Sub New(ByVal pollRepo As IPollRepository, ByVal pollOp As IPollOptionRepository)
      _pollRepo = pollRepo
      _pollOptionRepo = pollOp
   End Sub


   Public Function GetPollsCount() As Integer Implements IPollService.GetPollsCount

      Dim key As String = "polls_pollCount"

      If Cache(key) IsNot Nothing Then
         Return CInt(Cache(key))
      Else
         Dim it As Integer = _pollRepo.GetPolls.Count
         CacheData(key, it)
         Return it
      End If
   End Function

   Public Function GetPollsCount(ByVal criteria As String) As Integer Implements IPollService.GetPollsCount
      If Not String.IsNullOrEmpty(criteria) Then

         Dim key As String = "polls_pollCount_" & criteria & "_"

         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            Dim it As Integer = _pollRepo.GetPolls.Count(Function(p) p.QuestionText.Contains(criteria))
            CacheData(key, it)
            Return it
         End If
      Else
         Return GetPollsCount()
      End If
   End Function


   Public Function GetPolls() As List(Of Poll) Implements IPollService.GetPolls
      Dim key As String = "polls_polls_"
      If Cache(key) IsNot Nothing Then
         Dim li As List(Of Poll) = DirectCast(Cache(key), List(Of Poll))
         Return li
      Else

         Dim li As List(Of Poll) = _pollRepo.GetPolls.ToList
         CacheData(key, li)
         Return li
      End If
   End Function

   Public Function GetPolls(ByVal criteria As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer) As PagedList(Of Poll) Implements IPollService.GetPolls
      If Not String.IsNullOrEmpty(criteria) Then
         Dim key As String = "polls_polls_" & criteria & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
         If Cache(key) IsNot Nothing Then
                Dim li As PagedList(Of Poll) = DirectCast(Cache(key), PagedList(Of Poll))
            Return li
         Else
            Dim li As PagedList(Of Poll) = _pollRepo.GetPolls.Where(Function(it) it.QuestionText.Contains(criteria)).ToPagedList(startrowindex, maximumrows)
            CacheData(key, li)
            Return li
         End If
      Else
         Return GetPolls(startrowindex, maximumrows)
      End If
   End Function

   Public Function GetPolls(ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "questionText") As PagedList(Of Poll) Implements IPollService.GetPolls
      Dim key As String = "polls_polls_" & startrowindex.ToString & "_" & maximumrows.ToString & "_" & sortExp & "_"
      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), PagedList(Of Poll))
      Else
         Dim li As PagedList(Of Poll) = _pollRepo.GetPolls.SortBy(sortExp).ToPagedList(startrowindex, maximumrows)
         CacheData(key, li)
         Return li
      End If
   End Function

   Public Function GetPollById(ByVal id As Integer) As Poll Implements IPollService.GetPollById
      Dim key As String = "polls_polls_" & id.ToString & "_"
      If Cache(key) IsNot Nothing Then
         Dim fb As Poll = DirectCast(Cache(key), Poll)
         Return fb
      Else
         Dim fb As Poll = _pollRepo.GetPolls.Where(Function(it) it.PollID = id).FirstOrDefault
         CacheData(key, fb)
         Return fb
      End If
   End Function

   Public Function GetTotalVotesById(ByVal id As Integer) As Integer Implements IPollService.GetTotalVotesById
      Dim key As String = "polls_pollsvotes_" & id.ToString & "_"
      If Cache(key) IsNot Nothing Then
         Return CInt(Cache(key))
      Else
         Dim totalVotes As Integer = (Aggregate it In _pollOptionRepo.GetPollOptions Where it.PollID = id Into Sum(it.Votes))
         CacheData(key, totalVotes)
         Return totalVotes
      End If
   End Function

   Public Sub InsertPoll(ByVal p As Poll) Implements IPollService.InsertPoll
      If p IsNot Nothing Then
         p.AddedDate = DateTime.Now
         p.AddedBy = CurrentUserName

         _pollRepo.InsertPoll(p)
         CacheObject.PurgeCacheItems("polls_")
      End If
   End Sub

   Public Sub UpdatePoll(ByVal p As Poll) Implements IPollService.UpdatePoll
      If p IsNot Nothing Then
         _pollRepo.UpdatePoll(p)
         CacheObject.PurgeCacheItems("polls_polls_")
      End If
   End Sub

   Public Sub DeletePoll(ByVal PollId As Integer) Implements IPollService.DeletePoll
      If PollId > 0 Then
         _pollRepo.DeletePoll(PollId)
         CacheObject.PurgeCacheItems("polls_polls_")
      End If
   End Sub

   Protected Sub CacheData(ByVal key As String, ByVal data As Object)
      If data IsNot Nothing Then
         CacheObject.Cache.Insert(key, data, Nothing, DateTime.Now.AddSeconds(3600), TimeSpan.Zero)
      End If
   End Sub
End Class
