Imports Microsoft.VisualBasic
Imports System.Web.UI.HtmlControls
Imports System.Security.Principal
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Web
Imports System.Web.Caching
Imports System.Collections
Imports System

Public Class mccObject
   Protected Const MaxRows As Integer = Integer.MaxValue

   Private _cacheKey As String
   Public Property CacheKey() As String
      Get
         Return _cacheKey
      End Get
      Set(ByVal value As String)
         _cacheKey = value
      End Set
   End Property


   Protected Shared ReadOnly Property Cache() As Cache
      Get
         Return HttpContext.Current.Cache
      End Get
   End Property

   Protected Shared ReadOnly Property CurrentUser() As IPrincipal
      Get
         Return HttpContext.Current.User
      End Get
   End Property

   Protected Shared ReadOnly Property CurrentUserName() As String
      Get
         Dim userName As String = ""
         If HttpContext.Current.User.Identity.IsAuthenticated Then
            userName = HttpContext.Current.User.Identity.Name
         End If
         Return userName
      End Get
   End Property

   Protected Shared ReadOnly Property CurrentUserIP() As String
      Get
         Return HttpContext.Current.Request.UserHostAddress
      End Get
   End Property

   Protected Shared Function GetPageIndex(ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As Integer
      If maximumRows <= 0 Then
         Return 0
      Else
         Return CType(Math.Floor(CType(startRowIndex, Double) / CType(maximumRows, Double)), Integer)
      End If
   End Function

   Protected Shared Function ConvertNullToEmptyString(ByVal input As String) As String
      Return (Microsoft.VisualBasic.IIf(input Is Nothing, "", input))
   End Function

   Public Shared Function ConvertNullToZero(ByVal input As Object) As String
      If input Is DBNull.Value Then
            Return "0"
      Else
            Return CInt(input).ToString
      End If
   End Function



   ''' <summary>
   '''  Remove from the ASP.NET cache all items whose key starts with the input prefix
   ''' </summary>
   ''' <param name="prefix"></param>
   ''' <remarks></remarks>
   Protected Shared Sub PurgeCacheItems(ByVal prefix As String)

      prefix = prefix.ToLower()
      Dim itemsToRemove As List(Of String) = New List(Of String)

      Dim enumerator As IDictionaryEnumerator = mccObject.Cache.GetEnumerator()

      Dim str As String = ""
      For Each it As Object In mccObject.Cache
         str += it.Key.ToString() + Environment.NewLine
      Next

      While (enumerator.MoveNext())
         If enumerator.Key.ToString().ToLower().StartsWith(prefix) Then
            itemsToRemove.Add(enumerator.Key.ToString())
         End If
      End While

      For Each itemToRemove As String In itemsToRemove
         mccObject.Cache.Remove(itemToRemove)
      Next
   End Sub


   Public Shared Function GetActualConnectionString() As String
      ' to be modified later
      Dim str As String = "ASPNETDBConnectionString"
      Return str
   End Function
End Class
