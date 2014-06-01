<%@ WebHandler Language="VB" Class="captcha" %>

Imports System
Imports System.Web
Imports System.Drawing

Public Class captcha : Implements IHttpHandler
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        If context.Request.QueryString("uuid") IsNot Nothing Then
            Dim uuid As String = ""
            uuid = context.Request.QueryString("uuid")
           
            context.Response.ContentType = "image/jpeg"

            Dim MyCaptcha As VBCaptchaControl = CType(context.Cache.Get(uuid), VBCaptchaControl)
            'ci.GetCaptchaAs(Imaging.ImageFormat.Jpeg, context.Response.OutputStream)
            
            MyCaptcha.CaptchaImage.Save(context.Response.OutputStream, Imaging.ImageFormat.Jpeg)
        End If
    End Sub
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property
End Class