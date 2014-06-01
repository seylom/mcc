Imports Microsoft.VisualBasic
Imports System.Linq
Imports MCC.Data
Public Class WebEvents
   Inherits mccObject

   Public Shared Function GetWebEventCount() As Integer
      Dim mdc As New MCCDataContext()
      Dim key As String = "WebEvents_WebEventCount"

      If Cache(key) IsNot Nothing Then
         Return CInt(Cache(key))
      Else
         Dim it As Integer = mdc.mcc_WebEvents.Count()
         CacheData(key, it)
         Return it
      End If
   End Function


   Public Shared Function GetWebEvents() As List(Of mcc_WebEvent)
      Dim key As String = "WebEvents_WebEvents_"
      If Cache(key) IsNot Nothing Then
         Dim li As List(Of mcc_WebEvent) = DirectCast(Cache(key), List(Of mcc_WebEvent))
         Return li
      Else
         Dim mdc As New MCCDataContext()
         Dim li As List(Of mcc_WebEvent) = mdc.mcc_WebEvents.ToList
         CacheData(key, li)
         Return li
      End If
   End Function

   Public Shared Function GetWebEvents(ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "EventTime ASC") As List(Of mcc_WebEvent)
      Dim key As String = "WebEvents_WebEvents_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
      If Cache(key) IsNot Nothing Then
         Dim li As List(Of mcc_WebEvent) = DirectCast(Cache(key), List(Of mcc_WebEvent))
         Return li
      Else
         Dim mdc As New MCCDataContext()
         Dim li As List(Of mcc_WebEvent)
         If startrowindex > 0 AndAlso maximumrows > 0 Then
            li = mdc.mcc_WebEvents.Skip(startrowindex - 1).Take(maximumrows).ToList
         Else
            li = mdc.mcc_WebEvents.Skip(0).Take(10).ToList
         End If
         CacheData(key, li)
         Return li
      End If
   End Function

   Public Shared Function GetWebEventById(ByVal id As Integer) As mcc_WebEvent
      Dim key As String = "WebEvents_WebEvents_" & id.ToString & "_"
      If Cache(key) IsNot Nothing Then
         Dim fb As mcc_WebEvent = DirectCast(Cache(key), mcc_WebEvent)
         Return fb
      Else
         Dim mdc As New MCCDataContext()
         Dim fb As mcc_WebEvent
         If mdc.mcc_WebEvents.Count(Function(p) p.EventId = id) > 0 Then
            fb = (From it As mcc_WebEvent In mdc.mcc_WebEvents Where it.EventId = id).FirstOrDefault
            CacheData(key, fb)
            Return fb
         Else
            Return Nothing
         End If
      End If
   End Function


   Public Shared Sub DeleteWebEvent(ByVal eventId As Integer)
      Dim mdc As MCCDataContext = New MCCDataContext
      If mdc.mcc_WebEvents.Count(Function(p) p.EventId = eventId) > 0 Then
         Dim wrd As mcc_WebEvent = (From t In mdc.mcc_WebEvents _
                                     Where t.EventId = eventId _
                                     Select t).Single()
         If wrd IsNot Nothing Then
            mdc.mcc_WebEvents.DeleteOnSubmit(wrd)
            mdc.SubmitChanges()
            mccObject.PurgeCacheItems("WebEvents_WebEvents_")
         End If
      End If
   End Sub

   Public Shared Sub DeleteOldEvent(ByVal beforeDate As DateTime)
      Dim mdc As MCCDataContext = New MCCDataContext
      If mdc.mcc_WebEvents.Count(Function(p) p.EventTime < beforeDate) > 0 Then
         Dim wrd As List(Of mcc_WebEvent) = (From t In mdc.mcc_WebEvents _
                                     Where t.EventTime < beforeDate _
                                     Select t).ToList
         If wrd IsNot Nothing AndAlso wrd.Count > 0 Then
            mdc.mcc_WebEvents.DeleteAllOnSubmit(wrd)
            mdc.SubmitChanges()
            mccObject.PurgeCacheItems("WebEvents_")
         End If
      End If
   End Sub

   Protected Shared Sub CacheData(ByVal key As String, ByVal data As Object)
      If data IsNot Nothing Then
         mccObject.Cache.Insert(key, data, Nothing, DateTime.Now.AddSeconds(3600), TimeSpan.Zero)
      End If
   End Sub
End Class
