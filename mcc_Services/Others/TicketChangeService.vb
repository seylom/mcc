Imports MCC.Data
Imports Webdiyer.WebControls.Mvc
Public Class TicketChangeService
   Inherits CacheObject
   Implements ITicketChangeService

   Private _ticketChangeRepo As ITicketChangeRepository
   Private _ticketservice As ITicketService

   Public Sub New()
      Me.New(New TicketChangeRepository(), New TicketService)
   End Sub

   Public Sub New(ByVal tkChangeRepo As ITicketChangeRepository, ByVal ticketservr As ITicketService)
      _ticketChangeRepo = tkChangeRepo
      _ticketservice = ticketservr
   End Sub


   Public Function GetTicketChangesCount() As Integer Implements ITicketChangeService.GetTicketChangesCount

      Dim key As String = "ticketschanges_ticketchangesCount"

      If Cache(key) IsNot Nothing Then
         Return CInt(Cache(key))
      Else
         Dim it As Integer = _ticketChangeRepo.GetTicketChanges.Count
         CacheData(key, it)
         Return it
      End If
   End Function

   Public Function GetTicketChangesCount(ByVal ticketId As Integer) As Integer Implements ITicketChangeService.GetTicketChangesCount
      If ticketId > 0 Then

         Dim key As String = "ticketschanges_ticketchangesCount_" & ticketId.ToString & "_"

         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            Dim it As Integer = _ticketChangeRepo.GetTicketChanges.Where(Function(p) p.TicketID = ticketId).Count
            CacheData(key, it)
            Return it
         End If
      Else
         Return GetTicketChangesCount()
      End If
   End Function


   Public Function GetTicketsChanges(ByVal ticketId As Integer) As List(Of TicketChange) Implements ITicketChangeService.GetTicketsChanges
      If ticketId > 0 Then


         Dim key As String = "ticketschanges_ticketschanges_" & ticketId.ToString
         If Cache(key) IsNot Nothing Then
            Return DirectCast(Cache(key), List(Of TicketChange))
         Else

            Dim li As List(Of TicketChange) = _ticketChangeRepo.GetTicketChanges.Where(Function(p) p.TicketID = ticketId).ToList
            CacheData(key, li)
            Return li
         End If
      Else
         Return Nothing
      End If
   End Function

   Public Function GetTicketsChanges(ByVal ticketId As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedby") As PagedList(Of TicketChange) Implements ITicketChangeService.GetTicketsChanges
      If ticketId > 0 Then
         Dim key As String = "ticketschanges_ticketschanges_" & ticketId.ToString & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
         If Cache(key) IsNot Nothing Then
                Dim li As PagedList(Of TicketChange) = DirectCast(Cache(key), PagedList(Of TicketChange))
            Return li
         Else
            Dim li As PagedList(Of TicketChange) = _ticketChangeRepo.GetTicketChanges.Where(Function(p) p.TicketID = ticketId).SortBy(sortExp).ToPagedList(startrowindex, maximumrows)
            CacheData(key, li)
            Return li
         End If
      Else
         Return GetTicketsChanges(startrowindex, maximumrows)
      End If
   End Function

   Public Function GetTicketsChanges(ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "addedby") As PagedList(Of TicketChange) Implements ITicketChangeService.GetTicketsChanges
      Dim key As String = "ticketsChanges_ticketschanges_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
      If Cache(key) IsNot Nothing Then
            Dim li As PagedList(Of TicketChange) = DirectCast(Cache(key), PagedList(Of TicketChange))
         Return li
      Else
         Dim li As PagedList(Of TicketChange) = _ticketChangeRepo.GetTicketChanges.SortBy(sortExp).ToPagedList(startrowindex, maximumrows)
         CacheData(key, li)
         Return li
      End If
   End Function

   Public Function GetTicketChangeById(ByVal id As Integer) As TicketChange Implements ITicketChangeService.GetTicketChangeById
      Dim key As String = "ticketsChanges_ticketschanges_" & id.ToString & "_"
      If Cache(key) IsNot Nothing Then
         Dim fb As TicketChange = DirectCast(Cache(key), TicketChange)
         Return fb
      Else
         Dim fb As TicketChange = _ticketChangeRepo.GetTicketChanges.Where(Function(it) it.TicketChangeID = id).FirstOrDefault
         CacheData(key, fb)
         Return fb
      End If
   End Function

   Public Sub InsertTicketChange(ByVal tkc As TicketChange) Implements ITicketChangeService.InsertTicketChange
      If (tkc IsNot Nothing) Then
         With tkc
            .AddedDate = DateTime.Now
            .AddedBy = CacheObject.CurrentUserName
         End With

         _ticketChangeRepo.InsertTicketChange(tkc)
         CacheObject.PurgeCacheItems("ticketschanges_")
      End If
   End Sub

   Public Sub UpdateTicketChange(ByVal tkc As TicketChange) Implements ITicketChangeService.UpdateTicketChange
      If (tkc IsNot Nothing) Then

            Dim q As TicketChange = _ticketChangeRepo.GetTicketChanges.Where(Function(p) p.TicketChangeID = tkc.TicketChangeID).FirstOrDefault

         If q IsNot Nothing Then
            q.Body = tkc.Body
         End If

         _ticketChangeRepo.UpdateTicketChange(tkc)
         CacheObject.PurgeCacheItems("ticketschanges_")
      End If
   End Sub

   Public Sub DeleteTicketChange(ByVal TicketChangeId As Integer) Implements ITicketChangeService.DeleteTicketChange
      If TicketChangeId > 0 Then
         _ticketChangeRepo.DeleteTicketChange(TicketChangeId)
         CacheObject.PurgeCacheItems("ticketsChanges_ticketsChanges_")
      End If
   End Sub

   ''Function GetChanges(ByVal newTicket As Ticket) As String

   ''   Dim changes As String = ""
   ''   If newTicket.TicketID <> 0 Then

   ''      Dim orig_tck As Ticket = _ticketservice.GetTicketById(newTicket.TicketID)
   ''      If orig_tck IsNot Nothing Then
   ''         changes = TrackChanges(newTicket, orig_tck)
   ''      End If
   ''   End If
   ''   Return changes
   ''End Function

   'Public Sub CreateChangeTicket(ByVal newTicketChange As Ticket)
   '   If newTicket.TicketID <= 0 Then
   '      ' TicketId = GetTicketId()
   '      Return
   '   End If


   '   Dim changes As String = GetChanges(newTicketChange)
   '   If Not String.IsNullOrEmpty(changes) Then
   '      changes += "<br/><br/>"
   '   End If

   '   If String.IsNullOrEmpty(changes) AndAlso String.IsNullOrEmpty(body) Then

   '      Elses()
   '      newTicket.Description = changes + newTicket.Description
   '      InsertTicketChange(New TicketChange with {.TicketID = newTicket.TicketID,.Body = })
   '   End If

   'End Sub

   'Public Function TrackChanges(ByVal tck As Ticket, ByVal origTicket As Ticket) As String
   '   Dim changes As String = ""

   '   If origTicket.Owner <> tck.Owner Then
   '      changes += "- <b>owner</b> changed from <i>" & tck.Owner & "</i> to <i>" & tck.Owner & "</i> <br/>"
   '   End If

   '   If origTicket.Priority <> tck.Priority Then
   '      Dim prio As String = TicketService.GetPriorityCaption(tck.Priority)
   '      Dim new_prio As String = TicketService.GetPriorityCaption(tck.Priority)
   '      changes += "- <b>priority</b> changed from <i>{0}</i> to <i>{1}</i><br/>"
   '      changes = String.Format(changes, prio, new_prio)
   '   End If

   '   If origTicket.Type <> tck.Type Then
   '      Dim tp As String = TicketService.GetTypeCaption(tck.Type)
   '      Dim new_tp As String = TicketService.GetTypeCaption(tck.Type)
   '      changes += "- <b>Type</b> changed from <i>{0}</i> to <i>{1}</i><br/>"
   '      changes = String.Format(changes, tp, new_tp)
   '   End If

   '   If origTicket.Status <> tck.Status Then
   '      Dim tp As String = TicketService.GetStatusCaption(tck.Status)
   '      Dim new_tp As String = TicketService.GetStatusCaption(tck.Status)
   '      changes += "- <b>Status</b> changed from <i>{0}</i> to <i>{1}</i><br/>"
   '      changes = String.Format(changes, tp, new_tp)
   '   End If

   '   'If origTicket.Title <> tck.Title Then
   '   '   Dim tp As String = HtmlEncode(tck.Title)
   '   '   Dim new_tp As String = HtmlEncode(Me.txtPropTitle.Text)
   '   '   changes += "- <b>Title</b> changed from <i>{0}</i> to <i>{1}</i><br/>"
   '   '   changes = String.Format(changes, tp, new_tp)
   '   'End If

   '   'If origTicket.Description <> routines.Decode(tck.Description) Then
   '   '   changes += "- <b>Description</b> was updated"
   '   'End If

   '   Return changes
   'End Function


   Protected Sub CacheData(ByVal key As String, ByVal data As Object)
      If data IsNot Nothing Then
         CacheObject.Cache.Insert(key, data, Nothing, DateTime.Now.AddSeconds(3600), TimeSpan.Zero)
      End If
   End Sub

End Class
