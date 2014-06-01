Imports System
Imports System.Data
Imports System.Configuration
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports System.Data.Common
Imports System.Web.Caching

Namespace MCC.SiteLayers
   Public MustInherit Class DataAccess
      Private _connectionString As String = ""
      Protected Property ConnectionString() As String
         Get
            Return _connectionString
         End Get
         Set(ByVal value As String)
            _connectionString = Value
         End Set
      End Property

      Private _enableCaching As Boolean = True
      Protected Property EnableCaching() As Boolean
         Get
            Return _enableCaching
         End Get
         Set(ByVal value As Boolean)
            _enableCaching = Value
         End Set
      End Property

      Private _cacheDuration As Integer = 0
      Protected Property CacheDuration() As Integer
         Get
            Return _cacheDuration
         End Get
         Set(ByVal value As Integer)
            _cacheDuration = Value
         End Set
      End Property

      Protected ReadOnly Property Cache() As Cache
         Get
            Return HttpContext.Current.Cache
         End Get
      End Property

      Protected Function ExecuteNonQuery(ByVal cmd As DbCommand) As Integer
         Return cmd.ExecuteNonQuery()
      End Function

      Protected Function ExecuteReader(ByVal cmd As DbCommand) As IDataReader
         Return ExecuteReader(cmd, CommandBehavior.[Default])
      End Function

      Protected Function ExecuteReader(ByVal cmd As DbCommand, ByVal behavior As CommandBehavior) As IDataReader
         Return cmd.ExecuteReader(behavior)
      End Function

      Protected Function ExecuteScalar(ByVal cmd As DbCommand) As Object
         Return cmd.ExecuteScalar()
      End Function
   End Class
End Namespace
