Imports MCC.Data
Imports Webdiyer.WebControls.Mvc
Public Enum TicketState As Integer
   [new] = 0
   assigned = 1
   resolved = 2
   reopened = 3
   verified = 4
   closed = 5
End Enum

Public Enum TicketType As Integer
   defect = 0
   enhancement = 1
   task = 2
End Enum

Public Enum TicketSeverity As Integer
   trivial = 0
   average = 1
   severe = 2
End Enum

Public Enum TicketPriority As Integer
   trivial = 0
   minor = 1
   major = 2
   critical = 3
   blocker = 4
End Enum
Public Class TicketService
   Inherits CacheObject
   Implements ITicketService



   Private _ticketRepo As ITicketRepository

   Public Sub New()
      Me.New(New TicketRepository())
   End Sub

   Public Sub New(ByVal tkChangeRepo As ITicketRepository)
      _ticketRepo = tkChangeRepo
   End Sub



   Public Function GetTicketCount() As Integer Implements ITicketService.GetTicketCount

      Dim key As String = "tickets_ticketCount_"

      If Cache(key) IsNot Nothing Then
         Return CInt(Cache(key))
      Else
         Dim it As Integer = _ticketRepo.GetTickets.Count()
         CacheData(key, it)
         Return it
      End If
   End Function


   Public Function GetTicketCount(ByVal owner As String) As Integer Implements ITicketService.GetTicketCount
      If Not String.IsNullOrEmpty(owner) Then

         Dim key As String = "tickets_ticketCount_" & owner & "_"

         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            Dim it As Integer = _ticketRepo.GetTickets.Count(Function(p) p.Owner = owner AndAlso p.Status <> TicketState.closed AndAlso p.Status <> TicketState.resolved)
            CacheData(key, it)
            Return it
         End If
      Else
         Return GetTicketCount()
      End If
   End Function

   Public Function GetTicketCount(ByVal status As Integer, ByVal owner As String) As Integer Implements ITicketService.GetTicketCount
      If Not String.IsNullOrEmpty(owner) Then
         If status > -1 Then

            Dim key As String = "tickets_ticketCount_" & status.ToString & "_" & owner & "_"

            If Cache(key) IsNot Nothing Then
               Return CInt(Cache(key))
            Else

               Dim it As Integer = _ticketRepo.GetTickets.Where(Function(p) p.Owner = owner AndAlso p.Status = status).Count
               CacheData(key, it)
               Return it
            End If
         Else
            Return GetTicketCount(owner)
         End If
      Else
         Return GetTicketCount(status)
      End If
   End Function

   Public Function GetTicketsByCommand(ByVal command As String, ByVal params As String) As List(Of Ticket) Implements ITicketService.GetTicketsByCommand
      'If Not String.IsNullOrEmpty(command) Then
      '   Try
      '      Dim parameters() As String = params.Split("|"c)
      '      Return mdc.ExecuteQuery(Of Ticket)(command, parameters).ToList()
      '   Catch ex As Exception
      '      Throw New Exception("Error in the sql Command for Ticket Filtering -")
      '      Return Nothing
      '   End Try
      'Else
      '   Return Nothing
      'End If

      Return _ticketRepo.GetTicketsByCommand(command, params).ToList

   End Function

   Public Function GetTicketCount(ByVal status As Integer) As Integer Implements ITicketService.GetTicketCount
      If status > -1 Then
         Dim key As String = "tickets_ticketCount_" & status.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            Dim it As Integer = _ticketRepo.GetTickets.Count(Function(p) p.Status = status)
            CacheData(key, it)
            Return it
         End If
      Else
         Return GetTicketCount()
      End If
   End Function


   Public Function GetTickets() As List(Of Ticket) Implements ITicketService.GetTickets
      Dim key As String = "tickets_tickets_"
      If Cache(key) IsNot Nothing Then
         Dim li As List(Of Ticket) = DirectCast(Cache(key), List(Of Ticket))
         Return li
      Else

         Dim li As List(Of Ticket) = _ticketRepo.GetTickets.ToList
         CacheData(key, li)
         Return li
      End If
   End Function

   Public Function GetTickets(ByVal status As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "Status") As PagedList(Of Ticket) Implements ITicketService.GetTickets
      If status > -1 Then
         Dim key As String = "tickets_tickets_" & status.ToString & "_" & sortExp.Replace(" ", "") & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
         If String.IsNullOrEmpty(sortExp) Then
            sortExp = "Status"
         End If
         If Cache(key) IsNot Nothing Then
            Dim li As PagedList(Of Ticket) = DirectCast(Cache(key), PagedList(Of Ticket))
            Return li
         Else
            Dim li As PagedList(Of Ticket) = _ticketRepo.GetTickets.Where(Function(it) it.Status = status).SortBy(sortExp).ToPagedList(startrowindex, maximumrows)
            CacheData(key, li)
            Return li
         End If
      Else
         Return GetTickets(startrowindex, maximumrows, sortExp)
      End If
   End Function

   Public Function GetTicketsByOwner(ByVal status As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, ByVal owner As String, Optional ByVal sortExp As String = "AddedBy") As PagedList(Of Ticket) Implements ITicketService.GetTicketsByOwner
      If Not String.IsNullOrEmpty(owner) Then
         If status > -1 Then
            Dim key As String = "tickets_tickets_" & status.ToString & "_" & owner & "_" & sortExp.Replace(" ", "") & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
            If String.IsNullOrEmpty(sortExp) Then
               sortExp = "AddedBy"
            End If
            If Cache(key) IsNot Nothing Then
               Dim li As PagedList(Of Ticket) = DirectCast(Cache(key), PagedList(Of Ticket))
               Return li
            Else
               Dim li As PagedList(Of Ticket) = _ticketRepo.GetTickets.Where(Function(it) it.Status = status AndAlso it.Owner = owner).SortBy(sortExp).ToPagedList(startrowindex, maximumrows)
               CacheData(key, li)
               Return li
            End If
         Else
            Return GetTicketsByOwner(startrowindex, maximumrows, owner, sortExp)
         End If
      Else
         Return GetTickets(status, startrowindex, maximumrows, sortExp)
      End If
   End Function

   Public Function GetTicketsByOwner(ByVal startrowindex As Integer, ByVal maximumrows As Integer, ByVal owner As String, Optional ByVal sortExp As String = "AddedBy") As PagedList(Of Ticket) Implements ITicketService.GetTicketsByOwner
      If Not String.IsNullOrEmpty(owner) Then
         Dim key As String = "tickets_tickets_" & owner & "_" & sortExp.Replace(" ", "") & startrowindex.ToString & "_" & maximumrows.ToString & "_"
         If String.IsNullOrEmpty(sortExp) Then
            sortExp = "AddedBy"
         End If
         If Cache(key) IsNot Nothing Then
            Dim li As PagedList(Of Ticket) = DirectCast(Cache(key), PagedList(Of Ticket))
            Return li
         Else

            Dim li As PagedList(Of Ticket) = _ticketRepo.GetTickets.Where(Function(it) it.Owner = owner AndAlso _
                                                                        it.Status <> TicketState.closed AndAlso _
                                                                        it.Status <> TicketState.resolved).SortBy(sortExp).ToPagedList(startrowindex, maximumrows)
            CacheData(key, li)
            Return li
         End If
      Else
         Return GetTickets(startrowindex, maximumrows, sortExp)
      End If
   End Function

   Public Function GetTickets(ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "TicketId DESC") As PagedList(Of Ticket) Implements ITicketService.GetTickets
      Dim key As String = "tickets_tickets_" & sortExp.Replace(" ", "") & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
      If String.IsNullOrEmpty(sortExp) Then
         sortExp = "TicketId DESC"
      End If
      If Cache(key) IsNot Nothing Then
         Dim li As PagedList(Of Ticket) = DirectCast(Cache(key), PagedList(Of Ticket))
         Return li
      Else
         Dim li As PagedList(Of Ticket) = _ticketRepo.GetTickets.SortBy(sortExp).ToPagedList(startrowindex, maximumrows)
         CacheData(key, li)
         Return li
      End If
   End Function

   Public Function GetTicketById(ByVal id As Integer) As Ticket Implements ITicketService.GetTicketById
      Dim key As String = "tickets_tickets_" & id.ToString & "_"
      If Cache(key) IsNot Nothing Then
         Dim fb As Ticket = DirectCast(Cache(key), Ticket)
         Return fb
      Else
         Dim fb As Ticket = _ticketRepo.GetTickets.Where(Function(it) it.TicketID = id).FirstOrDefault
         CacheData(key, fb)
         Return fb
      End If
   End Function



   Public Sub InsertTicket(ByVal tk As Ticket)
      If tk IsNot Nothing Then
         With tk
            .AddedDate = DateTime.Now
            .Addedby = CacheObject.CurrentUserName
         End With

         _ticketRepo.UpdateTicket(tk)
         CacheObject.PurgeCacheItems("tickets_")
      End If
   End Sub

   Public Sub UpdateTicketState(ByVal TicketId As Integer, ByVal status As Integer) Implements ITicketService.UpdateTicketState
      Dim q = _ticketRepo.GetTickets.Where(Function(t) t.TicketID = TicketId).FirstOrDefault()
      If q IsNot Nothing Then
         q.Status = status
         CacheObject.PurgeCacheItems("tickets_tickets_")
      End If
   End Sub

   Public Sub UpdateTicket(ByVal tk As Ticket)
      If tk IsNot Nothing Then
         _ticketRepo.UpdateTicket(tk)
         CacheObject.PurgeCacheItems("tickets_tickets_")
         CacheObject.PurgeCacheItems("tickets_ticketCount_")
      End If
   End Sub

   'Public Sub UpdateTickets(ByVal TicketId As Integer, ByVal title As String, ByVal description As String, ByVal type As Integer, ByVal owner As String, ByVal priority As Integer, ByVal status As Integer)
   '   Dim mdc As MCCDataContext = New MCCDataContext
   '   Dim wrd As Ticket = (From t In mdc.tickets _
   '                               Where t.TicketId = TicketId _
   '                               Select t).Single()

   '   If wrd IsNot Nothing Then
   '      wrd.Description = description
   '      wrd.Title = title
   '      wrd.Owner = owner
   '      wrd.Priority = priority
   '      wrd.Type = type
   '      wrd.Status = status
   '      mdc.SubmitChanges()
   '      CacheObject.PurgeCacheItems("tickets_tickets_")
   '      CacheObject.PurgeCacheItems("tickets_ticketCount_")
   '   End If
   'End Sub

   Public Sub DeleteTicket(ByVal TicketId As Integer) Implements ITicketService.DeleteTicket
      If (TicketId > 0) Then
         _ticketRepo.DeleteTicket(TicketId)
         CacheObject.PurgeCacheItems("tickets_tickets_")
      End If

      'Dim mdc As MCCDataContext = New MCCDataContext
      'If mdc.tickets.Count(Function(p) p.TicketId = TicketId) > 0 Then
      '   Dim wrd As Ticket = (From t In mdc.tickets _
      '                               Where t.TicketId = TicketId _
      '                               Select t).Single()
      '   If wrd IsNot Nothing Then
      '      mdc.tickets.DeleteOnSubmit(wrd)
      '      mdc.SubmitChanges()
      '      CacheObject.PurgeCacheItems("tickets_tickets_")
      '   End If
      'End If
   End Sub


   Sub DeleteTickets(ByVal TkIds() As Integer) Implements ITicketService.DeleteTickets
      If TkIds IsNot Nothing AndAlso TkIds.Count > 0 Then
         _ticketRepo.DeleteTickets(TkIds)
         CacheObject.PurgeCacheItems("tickets_")
      End If
   End Sub

   Protected Sub CacheData(ByVal key As String, ByVal data As Object)
      If data IsNot Nothing Then
         CacheObject.Cache.Insert(key, data, Nothing, DateTime.Now.AddSeconds(3600), TimeSpan.Zero)
      End If
   End Sub


   Public Shared Function GetTypeCaption(ByVal status As Integer) As String
      Select Case status
         Case 0
            Return "defect"
         Case 1
            Return "enhancement"
         Case 2
            Return "task"
         Case Else
            Return "defect"
      End Select
   End Function

   Public Shared Function GetStatusCaption(ByVal status As Integer) As String
      Select Case status
         Case 0
            Return "new"
         Case 1
            Return "assigned"
         Case 2
            Return "resolved"
         Case 3
            Return "reopened"
         Case 4
            Return "verified"
         Case 5
            Return "closed"
         Case Else
            Return "new"
      End Select
   End Function

   Public Shared Function GetPriorityCaption(ByVal status As Integer) As String
      Select Case status
         Case 0
            Return "low"
         Case 1
            Return "normal"
         Case 2
            Return "high"
         Case Else
            Return "low"
      End Select
   End Function

   Public Function SaveTicket(ByVal tk As Data.Ticket) As Boolean Implements ITicketService.SaveTicket
      Try
         If tk.TicketID > 0 Then
            UpdateTicket(tk)
         Else
            InsertTicket(tk)
         End If
         Return True
      Catch ex As Exception
         Return False
      End Try
   End Function
End Class
