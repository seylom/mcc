Imports Microsoft.VisualBasic
Imports System.Linq
Imports MCC.Data

Namespace Tickets
   Public Class TicketChangeRepository
      Inherits mccObject
      Public Shared Function GetTicketChangesCount() As Integer
         Dim mdc As New MCCDataContext()
         Dim key As String = "ticketschanges_ticketchangesCount"

         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            mdc.mcc_TicketChanges.Count()
            Dim it As Integer = mdc.mcc_TicketChanges.Count()
            CacheData(key, it)
            Return it
         End If
      End Function

      Public Shared Function GetTicketChangesCount(ByVal ticketId As Integer) As Integer
         If ticketId > 0 Then
            Dim mdc As New MCCDataContext()
            Dim key As String = "ticketschanges_ticketchangesCount_" & ticketId.ToString & "_"

            If Cache(key) IsNot Nothing Then
               Return CInt(Cache(key))
            Else
               Dim it As Integer = mdc.mcc_TicketChanges.Count(Function(p) p.TicketId = ticketId)
               CacheData(key, it)
               Return it
            End If
         Else
            Return GetTicketChangesCount()
         End If
      End Function


      Public Shared Function GetTicketsChanges(ByVal ticketId As Integer) As List(Of mcc_TicketChange)
         If ticketId > 0 Then


            Dim key As String = "ticketschanges_ticketschanges_" & ticketId.ToString
            If Cache(key) IsNot Nothing Then
               Dim li As List(Of mcc_TicketChange) = DirectCast(Cache(key), List(Of mcc_TicketChange))
               Return li
            Else
               Dim mdc As New MCCDataContext()
               Dim li As List(Of mcc_TicketChange) = (From it As mcc_TicketChange In mdc.mcc_TicketChanges Where it.TicketId = ticketId).ToList
               CacheData(key, li)
               Return li
            End If
         Else
            Return Nothing
         End If
      End Function

      Public Shared Function GetTicketsChanges(ByVal ticketId As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedby") As List(Of mcc_TicketChange)
         If ticketId > 0 Then
            Dim key As String = "ticketschanges_ticketschanges_" & ticketId.ToString & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
            If Cache(key) IsNot Nothing Then
               Dim li As List(Of mcc_TicketChange) = DirectCast(Cache(key), List(Of mcc_TicketChange))
               Return li
            Else
               Dim mdc As New MCCDataContext()
               Dim li As List(Of mcc_TicketChange)
               If startrowindex > 0 AndAlso maximumrows > 0 Then
                  li = (From it As mcc_TicketChange In mdc.mcc_TicketChanges Where it.TicketId = ticketId).Skip(startrowindex - 1).Take(maximumrows).ToList
               Else
                  li = (From it As mcc_TicketChange In mdc.mcc_TicketChanges Where it.TicketId = ticketId).Skip(0).Take(10).ToList
               End If
               CacheData(key, li)
               Return li
            End If
         Else
            Return GetTicketsChanges(startrowindex, maximumrows)
         End If
      End Function

      Public Shared Function GetTicketsChanges(ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedby") As List(Of mcc_TicketChange)
         Dim key As String = "ticketsChanges_ticketschanges_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Dim li As List(Of mcc_TicketChange) = DirectCast(Cache(key), List(Of mcc_TicketChange))
            Return li
         Else
            Dim mdc As New MCCDataContext()
            Dim li As List(Of mcc_TicketChange)
            If startrowindex > 0 AndAlso maximumrows > 0 Then
               li = mdc.mcc_TicketChanges.Skip(startrowindex - 1).Take(maximumrows).ToList
            Else
               li = mdc.mcc_TicketChanges.Skip(0).Take(10).ToList
            End If
            CacheData(key, li)
            Return li
         End If
      End Function

      Public Shared Function GetTicketChangeById(ByVal id As Integer) As mcc_TicketChange
         Dim key As String = "ticketsChanges_ticketschanges_" & id.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Dim fb As mcc_TicketChange = DirectCast(Cache(key), mcc_TicketChange)
            Return fb
         Else
            Dim mdc As New MCCDataContext()
            Dim fb As mcc_TicketChange
            If mdc.mcc_TicketChanges.Count(Function(p) p.TicketChangeId = id) > 0 Then
               fb = (From it As mcc_TicketChange In mdc.mcc_TicketChanges Where it.TicketChangeId = id).FirstOrDefault
               CacheData(key, fb)
               Return fb
            Else
               Return Nothing
            End If
         End If
      End Function



      Public Shared Sub InsertTicket(ByVal ticketId As Integer, ByVal body As String)
         Dim mdc As MCCDataContext = New MCCDataContext
         Dim fb As New mcc_TicketChange
         With fb
            .AddedDate = DateTime.Now
            .Addedby = mccObject.CurrentUserName
            .TicketId = ticketId
            .Body = body
         End With

         mdc.mcc_TicketChanges.InsertOnSubmit(fb)
         mccObject.PurgeCacheItems("ticketschanges_")
         mdc.SubmitChanges()
      End Sub

      Public Shared Sub DeleteTicketChange(ByVal TicketChangeId As Integer)
         Dim mdc As MCCDataContext = New MCCDataContext
         If mdc.mcc_TicketChanges.Count(Function(p) p.TicketChangeId = TicketChangeId) > 0 Then
            Dim wrd As mcc_TicketChange = (From t In mdc.mcc_TicketChanges _
                                        Where t.TicketChangeId = TicketChangeId _
                                        Select t).Single()
            If wrd IsNot Nothing Then
               mdc.mcc_TicketChanges.DeleteOnSubmit(wrd)
               mdc.SubmitChanges()
               mccObject.PurgeCacheItems("ticketsChanges_ticketsChanges_")
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

