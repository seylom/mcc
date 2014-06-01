<%@ WebHandler Language="VB" Class="articleSitemap" %>

Imports System
Imports System.Web
Imports System.Xml.Linq

Public Class articleSitemap : Implements IHttpHandler
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.ContentType = "text/xml"
        context.Response.Write(MCC.Utils.articlesMap())
    End Sub
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class