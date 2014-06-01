<%@ WebHandler Language="VB" Class="tipSitemap" %>

Imports System
Imports System.Web

Public Class tipSitemap : Implements IHttpHandler
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.ContentType = "text/xml"
        context.Response.Write(MCC.Utils.tipsMap())
    End Sub
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property
End Class