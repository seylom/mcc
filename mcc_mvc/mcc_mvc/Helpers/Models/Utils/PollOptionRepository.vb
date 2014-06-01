Imports Microsoft.VisualBasic
Imports System.Linq
Imports MCC.Data
Namespace Polls
   Public Class pollOptionRepository
      Inherits mccObject

      Public Shared Function GetPollOptionCount() As Integer
         Dim mdc As New MCCDataContext()
         Dim key As String = "pollOptions_pollOptionCount"

         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            mdc.mcc_PollOptions.Count()
            Dim it As Integer = mdc.mcc_PollOptions.Count()
            CacheData(key, it)
            Return it
         End If
      End Function

      Public Shared Function GetPollOptionCount(ByVal criteria As String) As Integer
         If Not String.IsNullOrEmpty(criteria) Then
            Dim mdc As New MCCDataContext()
            Dim key As String = "pollOptions_pollOptionCount_" & criteria & "_"

            If Cache(key) IsNot Nothing Then
               Return CInt(Cache(key))
            Else
               Dim it As Integer = mdc.mcc_PollOptions.Count(Function(p) p.OptionText.Contains(criteria))
               CacheData(key, it)
               Return it
            End If
         Else
            Return GetPollOptionCount()
         End If
      End Function


      Public Shared Function GetpollOptions() As List(Of mcc_PollOption)
         Dim key As String = "pollOptions_pollOptions_"
         If Cache(key) IsNot Nothing Then
            Dim li As List(Of mcc_PollOption) = DirectCast(Cache(key), List(Of mcc_PollOption))
            Return li
         Else
            Dim mdc As New MCCDataContext()
            Dim li As List(Of mcc_PollOption) = mdc.mcc_PollOptions.ToList
            CacheData(key, li)
            Return li
         End If
      End Function

      Public Shared Function GetpollOptions(ByVal criteria As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer) As List(Of mcc_PollOption)
         If Not String.IsNullOrEmpty(criteria) Then
            Dim key As String = "pollOptions_pollOptions_" & criteria & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
            If Cache(key) IsNot Nothing Then
               Dim li As List(Of mcc_PollOption) = DirectCast(Cache(key), List(Of mcc_PollOption))
               Return li
            Else
               Dim mdc As New MCCDataContext()
               Dim li As List(Of mcc_PollOption)
               If startrowindex > 0 AndAlso maximumrows > 0 Then
                  li = (From it As mcc_PollOption In mdc.mcc_PollOptions Where it.OptionText.Contains(criteria)).Skip(startrowindex - 1).Take(maximumrows).ToList
               Else
                  li = (From it As mcc_PollOption In mdc.mcc_PollOptions Where it.OptionText.Contains(criteria)).Skip(0).Take(10).ToList
               End If
               CacheData(key, li)
               Return li
            End If
         Else
            Return GetpollOptions(startrowindex, maximumrows)
         End If
      End Function

      Public Shared Function GetPollOptions(ByVal startrowindex As Integer, ByVal maximumrows As Integer) As List(Of mcc_PollOption)
         Dim key As String = "pollOptions_pollOptions_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Dim li As List(Of mcc_PollOption) = DirectCast(Cache(key), List(Of mcc_PollOption))
            Return li
         Else
            Dim mdc As New MCCDataContext()
            Dim li As List(Of mcc_PollOption)
            If startrowindex > 0 AndAlso maximumrows > 0 Then
               li = mdc.mcc_PollOptions.Skip(startrowindex - 1).Take(maximumrows).ToList
            Else
               li = mdc.mcc_PollOptions.Skip(0).Take(10).ToList
            End If
            CacheData(key, li)
            Return li
         End If
      End Function

      Public Shared Function GetPollOptionsByPollId(ByVal pollId As Integer) As List(Of mcc_PollOption)
         Dim key As String = "pollOptions_pollOptions_" & pollId.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Dim li As List(Of mcc_PollOption) = DirectCast(Cache(key), List(Of mcc_PollOption))
            Return li
         Else
            Dim mdc As New MCCDataContext()
            Dim li As List(Of mcc_PollOption)
            li = (From it As mcc_PollOption In mdc.mcc_PollOptions Where it.PollId = pollId).ToList
            CacheData(key, li)
            Return li
         End If
      End Function

      Public Shared Function GetPollOptionById(ByVal optionId As Integer) As mcc_PollOption
         Dim key As String = "pollOptions_pollOptions_" & optionId.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Dim fb As mcc_PollOption = DirectCast(Cache(key), mcc_PollOption)
            Return fb
         Else
            Dim mdc As New MCCDataContext()
            Dim fb As mcc_PollOption
            If mdc.mcc_PollOptions.Count(Function(p) p.OptionId = optionId) > 0 Then
               fb = (From it As mcc_PollOption In mdc.mcc_PollOptions Where it.OptionId = optionId).FirstOrDefault
               CacheData(key, fb)
               Return fb
            Else
               Return Nothing
            End If
         End If
      End Function


      Public Shared Function GetTotalVotesByPollOptionId(ByVal optionId As Integer) As Integer
         Dim key As String = "pollOptions_pollsvote_" & optionId.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Dim i As Integer = CInt(Cache(key))
            Return i
         Else
            Dim mdc As New MCCDataContext()
            Dim fb As mcc_PollOption
            If mdc.mcc_PollOptions.Count(Function(p) p.OptionId = optionId) > 0 Then
               fb = (From it As mcc_PollOption In mdc.mcc_PollOptions Where it.OptionId = optionId).FirstOrDefault

               Dim i As Integer = 0
               If fb IsNot Nothing Then
                  i = PollRepository.GetTotalVotesById(fb.PollId)
                  CacheData(key, i)
               End If

               Return i
            Else
               Return Nothing
            End If
         End If
      End Function



      Public Shared Sub InsertPollOption(ByVal pollId As Integer, ByVal optionText As String)
         Dim mdc As MCCDataContext = New MCCDataContext
         Dim fb As New mcc_PollOption()

         With fb
            .OptionText = optionText
            .PollId = pollId
            .AddedDate = DateTime.Now
            .AddedBy = mccObject.CurrentUserName
            .Votes = 0
         End With

         mdc.mcc_PollOptions.InsertOnSubmit(fb)
         mccObject.PurgeCacheItems("pollOptions_")
         mdc.SubmitChanges()
      End Sub

      Public Shared Function VotePollOptions(ByVal optionId As Integer) As Integer
         Dim mdc As MCCDataContext = New MCCDataContext

         If mdc.mcc_PollOptions.Count(Function(p) p.OptionId = optionId) > 0 Then
            Dim wrd As mcc_PollOption = (From t In mdc.mcc_PollOptions _
                                                Where t.OptionId = optionId _
                                                Select t).Single()


            If wrd IsNot Nothing Then
               If wrd.Votes IsNot Nothing Then
                  wrd.Votes += 1
                  mdc.SubmitChanges()
                  mccObject.PurgeCacheItems("pollOptions_pollOptions_")
                  mccObject.PurgeCacheItems("polls_pollsvotes_" & wrd.PollId.ToString)

                  Return wrd.PollId
               Else
                  wrd.Votes = 1
                  mdc.SubmitChanges()
                  mccObject.PurgeCacheItems("pollOptions_pollOptions_")
                  mccObject.PurgeCacheItems("polls_pollsvotes_" & wrd.PollId.ToString)

                  Return wrd.PollId
               End If
            Else
               Return 0
            End If
         End If
      End Function

      Public Shared Sub UpdatePollOptions(ByVal optionId As Integer, ByVal pollId As Integer, ByVal optionText As String)
         Dim mdc As MCCDataContext = New MCCDataContext

         If mdc.mcc_PollOptions.Count(Function(p) p.OptionId = optionId) > 0 Then
            Dim wrd As mcc_PollOption = (From t In mdc.mcc_PollOptions _
                                                Where t.OptionId = optionId _
                                                Select t).Single()
            If wrd IsNot Nothing Then
               wrd.PollId = pollId
               wrd.OptionText = optionText
               mdc.SubmitChanges()
               mccObject.PurgeCacheItems("pollOptions_pollOptions_")
            End If
         End If
      End Sub

      Public Shared Sub DeletePollOptions(ByVal optionId As Integer)
         Dim mdc As MCCDataContext = New MCCDataContext

         If mdc.mcc_PollOptions.Count(Function(p) p.OptionId = optionId) > 0 Then
            Dim wrd As mcc_PollOption = (From t In mdc.mcc_PollOptions _
                                                Where t.OptionId = optionId _
                                                Select t).Single()
            If wrd IsNot Nothing Then
               mdc.mcc_PollOptions.DeleteOnSubmit(wrd)
               mdc.SubmitChanges()
               mccObject.PurgeCacheItems("pollOptions_pollOptions_")
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
