Imports Microsoft.VisualBasic
Imports System.Linq
Imports MCC.Dynamic
Imports MCC.mccEnum.FilterKey
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports MCC.Data
Namespace Tickets
   Public Class TicketRepository
      Inherits mccObject

      Public Shared Function GetTicketCount() As Integer

         Dim key As String = "tickets_ticketCount_"

         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            Dim mdc As New MCCDataContext()
            mdc.mcc_Tickets.Count()
            Dim it As Integer = mdc.mcc_Tickets.Count()
            CacheData(key, it)
            Return it
         End If
      End Function


      Public Shared Function GetTicketCount(ByVal owner As String) As Integer
         If Not String.IsNullOrEmpty(owner) Then

            Dim key As String = "tickets_ticketCount_" & owner & "_"

            If Cache(key) IsNot Nothing Then
               Return CInt(Cache(key))
            Else
               Dim mdc As New MCCDataContext()
               Dim it As Integer = mdc.mcc_Tickets.Count(Function(p) p.Owner = owner AndAlso p.Status <> mccEnum.TicketState.closed AndAlso p.Status <> mccEnum.TicketState.resolved)
               CacheData(key, it)
               Return it
            End If
         Else
            Return GetTicketCount()
         End If
      End Function

      Public Shared Function GetTicketCount(ByVal status As Integer, ByVal owner As String) As Integer
         If Not String.IsNullOrEmpty(owner) Then
            If status > -1 Then

               Dim key As String = "tickets_ticketCount_" & status.ToString & "_" & owner & "_"

               If Cache(key) IsNot Nothing Then
                  Return CInt(Cache(key))
               Else
                  Dim mdc As New MCCDataContext()
                  Dim it As Integer = mdc.mcc_Tickets.Count(Function(p) p.Status = status AndAlso p.Owner = owner)
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

      Public Shared Function GetTicketsByCommand(ByVal command As String, ByVal params As String) As List(Of mcc_Ticket)
         If Not String.IsNullOrEmpty(command) Then
            Try
               Dim parameters() As String = params.Split("|"c)
               Dim mdc As New MCCDataContext()

               Return mdc.ExecuteQuery(Of mcc_Ticket)(command, parameters).ToList()

               'Using cn As New SqlConnection(Me.ConnectionString)
               '   Dim cmd As New SqlCommand("", cn)
               '   cmd.CommandType = CommandType.StoredProcedure
               '   cn.Open()
               '   Return GetCategoryCollectionFromReader(ExecuteReader(cmd))
               'End Using

            Catch ex As Exception
               Throw New Exception("Error in the sql Command for Ticket Filtering -")
               Return Nothing
            End Try
         Else
            Return Nothing
         End If
      End Function

      Public Shared Function GetTicketCount(ByVal status As Integer) As Integer
         If status > -1 Then

            Dim key As String = "tickets_ticketCount_" & status.ToString & "_"

            If Cache(key) IsNot Nothing Then
               Return CInt(Cache(key))
            Else
               Dim mdc As New MCCDataContext()

               Dim it As Integer = mdc.mcc_Tickets.Count(Function(p) p.Status = status)
               CacheData(key, it)
               Return it
            End If
         Else
            Return GetTicketCount()
         End If
      End Function


      Public Shared Function GetTickets() As List(Of mcc_Ticket)
         Dim key As String = "tickets_tickets_"
         If Cache(key) IsNot Nothing Then
            Dim li As List(Of mcc_Ticket) = DirectCast(Cache(key), List(Of mcc_Ticket))
            Return li
         Else
            Dim mdc As New MCCDataContext()
            Dim li As List(Of mcc_Ticket) = mdc.mcc_Tickets.ToList
            CacheData(key, li)
            Return li
         End If
      End Function

      Public Shared Function GetTickets(ByVal status As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "Status") As List(Of mcc_Ticket)
         If status > -1 Then
            Dim key As String = "tickets_tickets_" & status.ToString & "_" & sortExp.Replace(" ", "") & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
            If String.IsNullOrEmpty(sortExp) Then
               sortExp = "Status"
            End If
            If Cache(key) IsNot Nothing Then
               Dim li As List(Of mcc_Ticket) = DirectCast(Cache(key), List(Of mcc_Ticket))
               Return li
            Else
               Dim mdc As New MCCDataContext()
               Dim li As List(Of mcc_Ticket)

               startrowindex = IIf(startrowindex >= 0, startrowindex, 0)
               maximumrows = IIf(maximumrows > 0, maximumrows, 50)

               If startrowindex > 0 AndAlso maximumrows > 0 Then
                  li = (From it As mcc_Ticket In mdc.mcc_Tickets Where it.Status = status).SortBy(sortExp).Skip(startrowindex).Take(maximumrows).ToList
               Else
                  li = (From it As mcc_Ticket In mdc.mcc_Tickets Where it.Status = status).SortBy(sortExp).Skip(0).Take(50).ToList
               End If
               CacheData(key, li)
               Return li
            End If
         Else
            Return GetTickets(startrowindex, maximumrows, sortExp)
         End If
      End Function

      Public Shared Function GetTicketsByOwner(ByVal status As Integer, ByVal startrowindex As Integer, ByVal maximumrows As Integer, ByVal owner As String, Optional ByVal sortExp As String = "AddedBy") As List(Of mcc_Ticket)
         If Not String.IsNullOrEmpty(owner) Then
            If status > -1 Then
               Dim key As String = "tickets_tickets_" & status.ToString & "_" & owner & "_" & sortExp.Replace(" ", "") & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
               If String.IsNullOrEmpty(sortExp) Then
                  sortExp = "AddedBy"
               End If
               If Cache(key) IsNot Nothing Then
                  Dim li As List(Of mcc_Ticket) = DirectCast(Cache(key), List(Of mcc_Ticket))
                  Return li
               Else
                  Dim mdc As New MCCDataContext()
                  Dim li As List(Of mcc_Ticket)
                  If startrowindex > 0 AndAlso maximumrows > 0 Then
                     li = (From it As mcc_Ticket In mdc.mcc_Tickets Where it.Status = status AndAlso it.Owner = owner).SortBy(sortExp).Skip(startrowindex).Take(maximumrows).ToList
                  Else
                     li = (From it As mcc_Ticket In mdc.mcc_Tickets Where it.Status = status AndAlso it.Owner = owner).SortBy(sortExp).Skip(0).Take(30).ToList
                  End If
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

      Public Shared Function GetTicketsByOwner(ByVal startrowindex As Integer, ByVal maximumrows As Integer, ByVal owner As String, Optional ByVal sortExp As String = "AddedBy") As List(Of mcc_Ticket)
         If Not String.IsNullOrEmpty(owner) Then
            Dim key As String = "tickets_tickets_" & owner & "_" & sortExp.Replace(" ", "") & startrowindex.ToString & "_" & maximumrows.ToString & "_"
            If String.IsNullOrEmpty(sortExp) Then
               sortExp = "AddedBy"
            End If
            If Cache(key) IsNot Nothing Then
               Dim li As List(Of mcc_Ticket) = DirectCast(Cache(key), List(Of mcc_Ticket))
               Return li
            Else
               Dim mdc As New MCCDataContext()
               Dim li As List(Of mcc_Ticket)
               If startrowindex > 0 AndAlso maximumrows > 0 Then
                  li = (From it As mcc_Ticket In mdc.mcc_Tickets Where it.Owner = owner AndAlso it.Status <> mccEnum.TicketState.closed AndAlso it.Status <> mccEnum.TicketState.resolved).SortBy(sortExp).Skip(startrowindex).Take(maximumrows).ToList
               Else
                  li = (From it As mcc_Ticket In mdc.mcc_Tickets Where it.Owner = owner AndAlso it.Status <> mccEnum.TicketState.closed AndAlso it.Status <> mccEnum.TicketState.resolved).SortBy(sortExp).Skip(0).Take(30).ToList
               End If
               CacheData(key, li)
               Return li
            End If
         Else
            Return GetTickets(startrowindex, maximumrows, sortExp)
         End If
      End Function

      Public Shared Function GetTickets(ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "TicketId DESC") As List(Of mcc_Ticket)
         Dim key As String = "tickets_tickets_" & sortExp.Replace(" ", "") & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
         If String.IsNullOrEmpty(sortExp) Then
            sortExp = "TicketId DESC"
         End If
         If Cache(key) IsNot Nothing Then
            Dim li As List(Of mcc_Ticket) = DirectCast(Cache(key), List(Of mcc_Ticket))
            Return li
         Else
            Dim mdc As New MCCDataContext()
            Dim li As List(Of mcc_Ticket)
            If startrowindex > 0 AndAlso maximumrows > 0 Then
               li = mdc.mcc_Tickets.SortBy(sortExp).Skip(startrowindex - 1).Take(maximumrows).ToList
            Else
               li = mdc.mcc_Tickets.SortBy(sortExp).Skip(0).Take(30).ToList
            End If
            CacheData(key, li)
            Return li
         End If
      End Function

      Public Shared Function GetTicketById(ByVal id As Integer) As mcc_Ticket
         Dim key As String = "tickets_tickets_" & id.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Dim fb As mcc_Ticket = DirectCast(Cache(key), mcc_Ticket)
            Return fb
         Else
            Dim mdc As New MCCDataContext()
            Dim fb As mcc_Ticket
            If mdc.mcc_Tickets.Count(Function(p) p.TicketId = id) > 0 Then
               fb = (From it As mcc_Ticket In mdc.mcc_Tickets Where it.TicketId = id).FirstOrDefault
               CacheData(key, fb)
               Return fb
            Else
               Return Nothing
            End If
         End If
      End Function



      Public Shared Sub InsertTicket(ByVal title As String, ByVal description As String, ByVal type As Integer, ByVal owner As String, Optional ByVal priority As Integer = 0)
         Dim mdc As MCCDataContext = New MCCDataContext
         Dim fb As New mcc_Ticket()

         'description = routines.Encode(ConvertNullToEmptyString(description))
         'title = routines.Encode(ConvertNullToEmptyString(title))

         With fb
            .Title = title
            .Description = description
            .AddedDate = DateTime.Now
            .AddedBy = mccObject.CurrentUserName
            .Status = mccEnum.TicketState.new
            .Priority = priority
            .Type = type
            .Owner = owner
         End With

         mdc.mcc_Tickets.InsertOnSubmit(fb)
         mccObject.PurgeCacheItems("tickets_")
         mdc.SubmitChanges()
      End Sub

      Public Shared Sub UpdateTicketState(ByVal TicketId As Integer, ByVal status As Integer)
         Dim mdc As MCCDataContext = New MCCDataContext
         Dim wrd As mcc_Ticket = (From t In mdc.mcc_Tickets _
                                     Where t.TicketId = TicketId _
                                     Select t).Single()
         If wrd IsNot Nothing Then
            wrd.Status = status
            mdc.SubmitChanges()
            mccObject.PurgeCacheItems("tickets_tickets_")
         End If
      End Sub

      Public Shared Sub UpdateTickets(ByVal TicketId As Integer, ByVal title As String, ByVal description As String, ByVal type As Integer, ByVal owner As String, ByVal priority As Integer)
         Dim mdc As MCCDataContext = New MCCDataContext
         Dim wrd As mcc_Ticket = (From t In mdc.mcc_Tickets _
                                     Where t.TicketId = TicketId _
                                     Select t).Single()

         'description = routines.Encode(ConvertNullToEmptyString(description))
         'title = routines.Encode(ConvertNullToEmptyString(title))

         If wrd IsNot Nothing Then
            wrd.Description = description
            wrd.Title = title
            wrd.Owner = owner
            wrd.Priority = priority
            wrd.Type = type
            mdc.SubmitChanges()
            mccObject.PurgeCacheItems("tickets_tickets_")
            mccObject.PurgeCacheItems("tickets_ticketCount_")
         End If
      End Sub

      Public Shared Sub UpdateTickets(ByVal TicketId As Integer, ByVal title As String, ByVal description As String, ByVal type As Integer, ByVal owner As String, ByVal priority As Integer, ByVal status As Integer)
         Dim mdc As MCCDataContext = New MCCDataContext
         Dim wrd As mcc_Ticket = (From t In mdc.mcc_Tickets _
                                     Where t.TicketId = TicketId _
                                     Select t).Single()

         'description = routines.Encode(ConvertNullToEmptyString(description))
         'title = routines.Encode(ConvertNullToEmptyString(title))

         If wrd IsNot Nothing Then
            wrd.Description = description
            wrd.Title = title
            wrd.Owner = owner
            wrd.Priority = priority
            wrd.Type = type
            wrd.Status = status
            mdc.SubmitChanges()
            mccObject.PurgeCacheItems("tickets_tickets_")
            mccObject.PurgeCacheItems("tickets_ticketCount_")
         End If
      End Sub

      Public Shared Sub DeleteTicket(ByVal TicketId As Integer)
         Dim mdc As MCCDataContext = New MCCDataContext
         If mdc.mcc_Tickets.Count(Function(p) p.TicketId = TicketId) > 0 Then
            Dim wrd As mcc_Ticket = (From t In mdc.mcc_Tickets _
                                        Where t.TicketId = TicketId _
                                        Select t).Single()
            If wrd IsNot Nothing Then
               mdc.mcc_Tickets.DeleteOnSubmit(wrd)
               mdc.SubmitChanges()
               mccObject.PurgeCacheItems("tickets_tickets_")
            End If
         End If
      End Sub

      Protected Shared Sub CacheData(ByVal key As String, ByVal data As Object)
         If data IsNot Nothing Then
            mccObject.Cache.Insert(key, data, Nothing, DateTime.Now.AddSeconds(3600), TimeSpan.Zero)
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

   End Class

   Public Class TicketComparer
      Implements IComparer(Of mcc_Ticket)
      Private _sortBy As String
      Private _reverse As Boolean

      Public Sub New(ByVal sortBy As String)
         If Not String.IsNullOrEmpty(sortBy) Then
            sortBy = sortBy.ToLower()
            _reverse = sortBy.EndsWith(" desc")
            _sortBy = sortBy.Replace(" desc", "").Replace(" asc", "")
         End If
      End Sub

      Public Overloads Function Equals(ByVal x As mcc_Ticket, ByVal y As mcc_Ticket) As Boolean
         Return (x.TicketId = y.TicketId)
      End Function

      Public Function Compare(ByVal x As mcc_Ticket, ByVal y As mcc_Ticket) As Integer Implements IComparer(Of mcc_Ticket).Compare
         Dim ret As Integer = 0
         Select Case _sortBy
            Case "addeddate"
               ret = DateTime.Compare(x.AddedDate, y.AddedDate)
               Exit Select
            Case "addedby"
               ret = String.Compare(x.AddedBy, y.AddedBy, True)
            Case "title"
               ret = String.Compare(x.Title, y.Title, True)
            Case "owner"
               ret = String.Compare(x.Owner, y.Owner, True)
            Case "status"
               'ret = IIf(x.Status > y.Status, 1, 0)
               ret = String.Compare(TicketRepository.GetStatusCaption(x.Status), TicketRepository.GetStatusCaption(y.Status), True)
            Case "priority"
               'ret = IIf(x.Priority > y.Priority, 1, 0)
               ret = String.Compare(TicketRepository.GetPriorityCaption(x.Priority), TicketRepository.GetPriorityCaption(y.Priority), True)
            Case "type"
               'ret = IIf(x.Type > y.Type, 1, 0)
               ret = String.Compare(TicketRepository.GetTypeCaption(x.Type), TicketRepository.GetTypeCaption(y.Type), True)
            Case "ticketid"
               ret = IIf(x.TicketId > y.TicketId, -1, 1)
               Exit Select
         End Select
         Return (ret * (IIf(_reverse, -1, 1)))
      End Function
   End Class
End Namespace

