Imports Microsoft.VisualBasic
Imports System.Linq

Public Class aspnet_membership

   Public ReadOnly Property Username() As String
      Get
         Dim mdc As New umcDataContext
         Dim user As aspnet_User = (From u As aspnet_User In mdc.aspnet_Users Where u.UserId = Me.UserId).FirstOrDefault
         If user IsNot Nothing Then
            Return user.LoweredUserName
         End If
         Return ""
      End Get
   End Property

   Public ReadOnly Property CreationDate() As Date
      Get    
         Return Me.CreateDate
      End Get
   End Property

   Public ReadOnly Property LastActivityDate() As Date
      Get
         Return Me.LastLoginDate
      End Get
   End Property
End Class
