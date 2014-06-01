Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web

Namespace MvcDomainRouting.Code
   Public Class DomainData
      Private _Protocol As String
      Public Property Protocol() As String
         Get
            Return _Protocol
         End Get
         Set(ByVal value As String)
            _Protocol = value
         End Set
      End Property
      Private _HostName As String
      Public Property HostName() As String
         Get
            Return _HostName
         End Get
         Set(ByVal value As String)
            _HostName = value
         End Set
      End Property
      Private _Fragment As String
      Public Property Fragment() As String
         Get
            Return _Fragment
         End Get
         Set(ByVal value As String)
            _Fragment = value
         End Set
      End Property
   End Class
End Namespace
