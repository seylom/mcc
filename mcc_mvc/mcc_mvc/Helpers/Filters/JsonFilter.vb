Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Runtime.Serialization.Json
Imports System.Text
Imports System.Web
Imports System.Web.Mvc
Imports Newtonsoft.Json

Namespace MvcJson.Filters
   Public Class JsonFilter
      Inherits ActionFilterAttribute
      Private _Param As String
      Public Property Param() As String
         Get
            Return _Param
         End Get
         Set(ByVal value As String)
            _Param = value
         End Set
      End Property
      Private _JsonDataType As Type
      Public Property JsonDataType() As Type
         Get
            Return _JsonDataType
         End Get
         Set(ByVal value As Type)
            _JsonDataType = value
         End Set
      End Property
      Public Overloads Overrides Sub OnActionExecuting(ByVal filterContext As ActionExecutingContext)
         If filterContext.HttpContext.Request.ContentType.Contains("application/json") Then
            Dim inputContent As String
            Using sr = New StreamReader(filterContext.HttpContext.Request.InputStream)
               inputContent = sr.ReadToEnd()
            End Using
            Dim result = JavaScriptConvert.DeserializeObject(inputContent)
            'Dim result = JavaScriptConvert.DeserializeObject(inputContent, JsonDataType)
            filterContext.ActionParameters(Param) = result
         End If
      End Sub
   End Class
End Namespace

