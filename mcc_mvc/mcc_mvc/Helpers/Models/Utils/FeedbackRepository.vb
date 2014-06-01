Imports Microsoft.VisualBasic
Imports System.Linq
Imports MCC.Data
Namespace Feedbacks
   Public Class feedbackRepository
      Inherits mccObject

      Public Shared Function GetFeedbackCount() As Integer
         Dim mdc As New MCCDataContext()
         Dim key As String = "feedbacks_feedbackCount"

         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            mdc.mcc_Feedbacks.Count()
            Dim it As Integer = mdc.mcc_Feedbacks.Count()
            CacheData(key, it)
            Return it
         End If
      End Function

      Public Shared Function GetFeedbackCount(ByVal criteria As String) As Integer
         If Not String.IsNullOrEmpty(criteria) Then
            Dim mdc As New MCCDataContext()
            Dim key As String = "feedbacks_feedbackCount_" & criteria & "_"

            If Cache(key) IsNot Nothing Then
               Return CInt(Cache(key))
            Else
               Dim it As Integer = mdc.mcc_Feedbacks.Count(Function(p) p.Description.Contains(criteria) Or p.Title.Contains(criteria))
               CacheData(key, it)
               Return it
            End If
         Else
            Return GetFeedbackCount()
         End If
      End Function


      Public Shared Function GetFeedbacks() As List(Of mcc_Feedback)
         Dim key As String = "feedbacks_feedbacks_"
         If Cache(key) IsNot Nothing Then
            Dim li As List(Of mcc_Feedback) = DirectCast(Cache(key), List(Of mcc_Feedback))
            Return li
         Else
            Dim mdc As New MCCDataContext()
            Dim li As List(Of mcc_Feedback) = mdc.mcc_Feedbacks.ToList
            CacheData(key, li)
            Return li
         End If
      End Function

      Public Shared Function GetFeedbacks(ByVal criteria As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer) As List(Of mcc_Feedback)
         If Not String.IsNullOrEmpty(criteria) Then
            Dim key As String = "feedbacks_feedbacks_" & criteria & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
            If Cache(key) IsNot Nothing Then
               Dim li As List(Of mcc_Feedback) = DirectCast(Cache(key), List(Of mcc_Feedback))
               Return li
            Else
               Dim mdc As New MCCDataContext()
               Dim li As List(Of mcc_Feedback)
               If startrowindex > 0 AndAlso maximumrows > 0 Then
                  li = (From it As mcc_Feedback In mdc.mcc_Feedbacks Where it.Description.Contains(criteria) Or it.Title.Contains(criteria)).Skip(startrowindex - 1).Take(maximumrows).ToList
               Else
                  li = (From it As mcc_Feedback In mdc.mcc_Feedbacks Where it.Description.Contains(criteria) Or it.Title.Contains(criteria)).Skip(0).Take(10).ToList
               End If
               CacheData(key, li)
               Return li
            End If
         Else
            Return GetFeedbacks(startrowindex, maximumrows)
         End If
      End Function

      Public Shared Function GetFeedbacks(ByVal startrowindex As Integer, ByVal maximumrows As Integer) As List(Of mcc_Feedback)
         Dim key As String = "feedbacks_feedbacks_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Dim li As List(Of mcc_Feedback) = DirectCast(Cache(key), List(Of mcc_Feedback))
            Return li
         Else
            Dim mdc As New MCCDataContext()
            Dim li As List(Of mcc_Feedback)
            If startrowindex > 0 AndAlso maximumrows > 0 Then
               li = mdc.mcc_Feedbacks.Skip(startrowindex - 1).Take(maximumrows).ToList
            Else
               li = mdc.mcc_Feedbacks.Skip(0).Take(10).ToList
            End If
            CacheData(key, li)
            Return li
         End If
      End Function

      Public Shared Function GetFeedbackById(ByVal id As Integer) As mcc_Feedback
         Dim key As String = "feedbacks_feedbacks_" & id.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Dim fb As mcc_Feedback = DirectCast(Cache(key), mcc_Feedback)
            Return fb
         Else
            Dim mdc As New MCCDataContext()
            Dim fb As mcc_Feedback
            If mdc.mcc_Feedbacks.Count(Function(p) p.FeedbackID = id) > 0 Then
               fb = (From it As mcc_Feedback In mdc.mcc_Feedbacks Where it.FeedbackID = id).FirstOrDefault
               CacheData(key, fb)
               Return fb
            Else
               Return Nothing
            End If
         End If
      End Function



      Public Shared Sub InsertFeedback(ByVal title As String, ByVal description As String, ByVal author As String)
         Dim mdc As MCCDataContext = New MCCDataContext
         Dim fb As New mcc_Feedback()

         With fb
            .Title = title
            .Description = description
            .AddedDate = DateTime.Now
            .Opened = False
            .Approved = False
            .Answered = False
            .Votes = 0
            .AddedBy = author
         End With

         mdc.mcc_Feedbacks.InsertOnSubmit(fb)
         mccObject.PurgeCacheItems("feedbacks_")
         mdc.SubmitChanges()
      End Sub


      Public Shared Sub UpdateFeedbacks(ByVal FeedbackId As Integer, ByVal description As String)
         Dim mdc As MCCDataContext = New MCCDataContext
         Dim wrd As mcc_Feedback = (From t In mdc.mcc_Feedbacks _
                                     Where t.FeedbackID = FeedbackId _
                                     Select t).Single()
         If wrd IsNot Nothing Then
            wrd.Description = description
            mdc.SubmitChanges()
            mccObject.PurgeCacheItems("feedbacks_feedbacks_")
         End If
      End Sub

      Protected Shared Sub CacheData(ByVal key As String, ByVal data As Object)
         If data IsNot Nothing Then
            mccObject.Cache.Insert(key, data, Nothing, DateTime.Now.AddSeconds(3600), TimeSpan.Zero)
         End If
      End Sub
   End Class

End Namespace
