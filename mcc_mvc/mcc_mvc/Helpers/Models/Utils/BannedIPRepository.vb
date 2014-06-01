Imports Microsoft.VisualBasic
Imports System.Linq
Imports MCC.Data

Namespace BannedIPs
   Public Class BannedIPRepository

      Public Shared Function GetBannedIPCount() As Integer
         Dim mdc As MCCDataContext = New MCCDataContext
         Return (From t As mcc_BannedIP In mdc.mcc_BannedIPs Select t).Count
      End Function

      Public Shared Function GetBannedIps() As List(Of mcc_BannedIP)
         Dim mdc As MCCDataContext = New MCCDataContext
         Dim li As New List(Of mcc_BannedIP)
         Return (From t As mcc_BannedIP In mdc.mcc_BannedIPs Select t).ToList
      End Function

      Public Shared Function GetBannedIps(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, Optional ByVal sort As String = "mask ASC") As List(Of mcc_BannedIP)
         Dim mdc As MCCDataContext = New MCCDataContext
         Dim li As New List(Of mcc_BannedIP)
         If startRowIndex > 0 AndAlso maximumRows > 0 Then
            Return (From t As mcc_BannedIP In mdc.mcc_BannedIPs Select t).Skip(startRowIndex - 1).Take(startRowIndex * maximumRows).ToList
         Else
            Return (From t As mcc_BannedIP In mdc.mcc_BannedIPs Select t).Skip(0).Take(20).ToList
         End If
      End Function

      Public Shared Sub AddToBlockedIP(ByVal IP As String)
         InsertIP(IP)
      End Sub


      Public Shared Sub InsertIP(ByVal mask As String)
         Dim mdc As MCCDataContext = New MCCDataContext
         Dim bw As New mcc_BannedIP With {.mask = mask, .since = DateTime.Now.ToString("d")}
         mdc.mcc_BannedIPs.InsertOnSubmit(bw)
         mdc.SubmitChanges()
      End Sub


      Public Shared Sub UpdateIP(ByVal ID As Integer, ByVal mask As String)
         Dim mdc As MCCDataContext = New MCCDataContext
         Dim wrd As mcc_BannedIP = (From t In mdc.mcc_BannedIPs _
                                     Where t.ID = ID _
                                     Select t).Single()
         If wrd IsNot Nothing Then
            wrd.Mask = mask
            mdc.SubmitChanges()
         End If
      End Sub


      Public Shared Sub DeleteIP(ByVal ID As Integer)
         Dim mdc As MCCDataContext = New MCCDataContext
         Dim wrd As mcc_BannedIP = (From t In mdc.mcc_BannedIPs _
                                     Where t.ID = ID _
                                     Select t).Single()

         If wrd IsNot Nothing Then
            mdc.mcc_BannedIPs.DeleteOnSubmit(wrd)
            Dim cs As System.Data.Linq.ChangeSet = mdc.GetChangeSet()
            mdc.SubmitChanges()
         End If
      End Sub


      Public Function isBannedIP(ByVal mask As String) As Boolean
         Dim bIP As Boolean = False
         Dim mdc As MCCDataContext = New MCCDataContext

         Dim wrd As mcc_BannedIP = (From t In mdc.mcc_BannedIPs _
                                    Where t.Mask = mask _
                                    Select t).FirstOrDefault
         If wrd IsNot Nothing Then
            Return True
         End If
         Return bIP
      End Function
   End Class
End Namespace
