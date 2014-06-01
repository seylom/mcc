Imports MCC.Data

Public Module TicketsHelper
   <System.Runtime.CompilerServices.Extension()> _
   Public Function GetTypeCaption(ByVal tk As Ticket) As String
      Select Case tk.Status
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

   <System.Runtime.CompilerServices.Extension()> _
   Public Function GetStatusCaption(ByVal tk As Ticket) As String
      Select Case tk.Status
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

   <System.Runtime.CompilerServices.Extension()> _
   Public Function GetPriorityCaption(ByVal tk As Ticket) As String
      Select Case tk.Status
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

End Module
