Imports Microsoft.VisualBasic
Imports System.Linq
Imports MCC.Dynamic
Imports MCC.Data

Namespace Polls
   Public Class PollRepository
      Inherits mccObject

      Public Shared Function GetPollsCount() As Integer
         Dim mdc As New MCCDataContext()
         Dim key As String = "polls_pollCount"

         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            mdc.mcc_Polls.Count()
            Dim it As Integer = mdc.mcc_Polls.Count()
            CacheData(key, it)
            Return it
         End If
      End Function

      Public Shared Function GetPollsCount(ByVal criteria As String) As Integer
         If Not String.IsNullOrEmpty(criteria) Then
            Dim mdc As New MCCDataContext()
            Dim key As String = "polls_pollCount_" & criteria & "_"

            If Cache(key) IsNot Nothing Then
               Return CInt(Cache(key))
            Else
               Dim it As Integer = mdc.mcc_Polls.Count(Function(p) p.QuestionText.Contains(criteria))
               CacheData(key, it)
               Return it
            End If
         Else
            Return GetPollsCount()
         End If
      End Function


      Public Shared Function GetPolls() As List(Of mcc_Poll)
         Dim key As String = "polls_polls_"
         If Cache(key) IsNot Nothing Then
            Dim li As List(Of mcc_Poll) = DirectCast(Cache(key), List(Of mcc_Poll))
            Return li
         Else
            Dim mdc As New MCCDataContext()
            Dim li As List(Of mcc_Poll) = mdc.mcc_Polls.ToList
            CacheData(key, li)
            Return li
         End If
      End Function

      Public Shared Function GetPolls(ByVal criteria As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer) As List(Of mcc_Poll)
         If Not String.IsNullOrEmpty(criteria) Then
            Dim key As String = "polls_polls_" & criteria & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
            If Cache(key) IsNot Nothing Then
               Dim li As List(Of mcc_Poll) = DirectCast(Cache(key), List(Of mcc_Poll))
               Return li
            Else
               Dim mdc As New MCCDataContext()
               Dim li As List(Of mcc_Poll)
               If startrowindex > 0 AndAlso maximumrows > 0 Then
                  li = (From it As mcc_Poll In mdc.mcc_Polls Where it.QuestionText.Contains(criteria)).Skip(startrowindex - 1).Take(maximumrows).ToList
               Else
                  li = (From it As mcc_Poll In mdc.mcc_Polls Where it.QuestionText.Contains(criteria)).Skip(0).Take(10).ToList
               End If
               CacheData(key, li)
               Return li
            End If
         Else
            Return GetPolls(startrowindex, maximumrows)
         End If
      End Function

      Public Shared Function GetPolls(ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "questionText") As List(Of mcc_Poll)
         Dim key As String = "polls_polls_" & startrowindex.ToString & "_" & maximumrows.ToString & "_" & sortExp & "_"
         If Cache(key) IsNot Nothing Then
            Dim li As List(Of mcc_Poll) = DirectCast(Cache(key), List(Of mcc_Poll))
            Return li
         Else
            Dim mdc As New MCCDataContext()
            Dim li As List(Of mcc_Poll)
            If startrowindex > 0 AndAlso maximumrows > 0 Then
               li = mdc.mcc_Polls.SortBy(sortExp).Skip(startrowindex - 1).Take(maximumrows).ToList
            Else
               li = mdc.mcc_Polls.SortBy(sortExp).Skip(0).Take(10).ToList
            End If
            CacheData(key, li)
            Return li
         End If
      End Function

      Public Shared Function GetPollById(ByVal id As Integer) As mcc_Poll
         Dim key As String = "polls_polls_" & id.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Dim fb As mcc_Poll = DirectCast(Cache(key), mcc_Poll)
            Return fb
         Else
            Dim mdc As New MCCDataContext()
            Dim fb As mcc_Poll
            If mdc.mcc_Polls.Count(Function(p) p.PollId = id) > 0 Then
               fb = (From it As mcc_Poll In mdc.mcc_Polls Where it.PollId = id).FirstOrDefault
               CacheData(key, fb)
               Return fb
            Else
               Return Nothing
            End If
         End If
      End Function

      Public Shared Function GetTotalVotesById(ByVal id As Integer) As Integer
         Dim key As String = "polls_pollsvotes_" & id.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Dim fb As Integer = CInt(Cache(key))
            Return fb
         Else
            Dim mdc As New MCCDataContext()
            Dim fb As Integer
            If mdc.mcc_PollOptions.Count(Function(p) p.PollId = id) > 0 Then
               fb = (Aggregate it In mdc.mcc_PollOptions Where it.PollId = id Into Sum(it.Votes)).Value
               CacheData(key, fb)
               Return fb
            Else
               Return 0
            End If
         End If
      End Function

      Public Shared Sub InsertPoll(ByVal questionText As String)
         Dim mdc As MCCDataContext = New MCCDataContext
         Dim fb As New mcc_Poll()

         With fb
            .QuestionText = questionText
            .isArchived = False
            .AddedDate = DateTime.Now
            .AddedBy = mccObject.CurrentUserName
         End With

         mdc.mcc_Polls.InsertOnSubmit(fb)
         mccObject.PurgeCacheItems("polls_")
         mdc.SubmitChanges()
      End Sub


      Public Shared Sub UpdatePoll(ByVal PollId As Integer, ByVal questionText As String)
         Dim mdc As MCCDataContext = New MCCDataContext
         Dim wrd As mcc_Poll = (From t In mdc.mcc_Polls _
                                     Where t.PollId = PollId _
                                     Select t).Single()
         If wrd IsNot Nothing Then
            wrd.QuestionText = questionText
            mdc.SubmitChanges()
            mccObject.PurgeCacheItems("polls_polls_")
         End If
      End Sub

      Public Shared Sub DeletePoll(ByVal PollId As Integer)
         Dim mdc As MCCDataContext = New MCCDataContext
         If mdc.mcc_Polls.Count(Function(p) p.PollId = PollId) > 0 Then
            Dim wrd As mcc_Poll = (From t In mdc.mcc_Polls _
                                        Where t.PollId = PollId _
                                        Select t).Single()
            If wrd IsNot Nothing Then
               mdc.mcc_Polls.DeleteOnSubmit(wrd)
               mdc.SubmitChanges()
               mccObject.PurgeCacheItems("polls_polls_")
            End If
         End If
      End Sub

      Protected Shared Sub CacheData(ByVal key As String, ByVal data As Object)
         If data IsNot Nothing Then
            mccObject.Cache.Insert(key, data, Nothing, DateTime.Now.AddSeconds(3600), TimeSpan.Zero)
         End If
      End Sub
   End Class
End Namespace
