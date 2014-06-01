<%@ WebHandler Language="VB" Class="questionSitemap" %>

Imports System
Imports System.Web

Public Class questionSitemap : Implements IHttpHandler
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
      context.Response.ContentType = "text/xml"
      context.Response.Write(MCC.Utils.questionsMap())
    End Sub
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class