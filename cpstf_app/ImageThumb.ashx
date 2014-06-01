<%@ WebHandler Language="VB" Class="ImageThumb" %>

Imports System
Imports System.Web
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.IO
Imports cpstf_app

Public Class ImageThumb : Implements IHttpHandler

   Private width As Integer = 120
   Private height As Integer = 80
    
   Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
      Try
         context.Response.Cache.SetCacheability(HttpCacheability.Public)
         context.Response.Cache.SetExpires(DateTime.Now.AddMonths(1))           
         context.Response.ContentType = "image/jpg"
         
         Dim originalImageUrl As String = context.Request.QueryString("img")
         Dim qs_thumbType As String = context.Request.QueryString("s")
         Dim qs_autoSet As String = context.Request.QueryString("auto")
         Dim qs_save As String = context.Request.QueryString("save")
         Dim qs_refresh As String = context.Request.QueryString("refresh")
         Dim qs_width As String = context.Request.QueryString("w")
         Dim qs_height As String = context.Request.QueryString("h")
         Dim qs_cache As String = context.Request.QueryString("cache")
         
         Dim width As Integer = 120
         Dim height As Integer = 80
         Dim save As Integer = 0
         Dim refresh As Integer = 0
         Dim cache As Integer = 1
         
         If Not String.IsNullOrEmpty(qs_width) Then
            width = Int32.Parse(qs_width)
         End If

         If Not String.IsNullOrEmpty(qs_height) Then
            height = Int32.Parse(qs_height)
         End If
         
         If Not String.IsNullOrEmpty(qs_save) Then
            save = Int32.Parse(qs_save)
         End If
         
         If Not String.IsNullOrEmpty(qs_refresh) Then
            refresh = Int32.Parse(qs_refresh)
         End If
            
         If Not String.IsNullOrEmpty(qs_cache) Then
            cache = Int32.Parse(qs_cache)
         End If   
         
         Dim urlprefix As String = "/uploads/images/"
         'Dim urlPrefix As String = ""
         
         Dim originalImageName As String = originalImageUrl.Substring(originalImageUrl.LastIndexOf("/") + 1)
         
         Dim originalImagePath As String = HttpContext.Current.Request.MapPath(urlprefix & originalImageUrl)
         
         
         If String.IsNullOrEmpty(originalImageUrl) Or Not File.Exists(originalImagePath) Then
            ' get a blank image instead of returning a 404 ...
            context.Response.Write("No image")
            Return
         End If
         
         
         Dim thumbName As String = String.Empty
         Dim thumbToSaveToDisk As String = String.Empty
         
         ' ej: thumbnail name is "150_80_originalImageName.jpg"
         thumbName = width.ToString & "_" & height.ToString & "_" & originalImageName
         
         Dim thumbnailPath As String = originalImageUrl.Substring(0, originalImageUrl.LastIndexOf("/") + 1) & thumbName
         
         Dim imageExtension As String = Path.GetExtension(originalImagePath).ToLower
         
         thumbToSaveToDisk = HttpContext.Current.Request.MapPath(urlprefix & thumbnailPath)
         
         ' load  from cache if Ok!
         If cache <> 0 AndAlso refresh = 0 Then
            If HttpContext.Current.Cache(thumbName) IsNot Nothing Then
               Dim bmp As Image = CType(HttpContext.Current.Cache(thumbName), Image)
               context.Response.ContentType = String.Format("image/{0}", imageExtension.Replace(".", ""))
               bmp.Save(context.Response.OutputStream, AssetsHelper.ImageFormatFromExtension(imageExtension))
               Return
            End If
         ElseIf refresh <> 0 Then
            HttpContext.Current.Cache.Remove(thumbName)
         End If    
         
         Dim target As Bitmap = AssetsHelper.CreateThumbnail(originalImagePath, width, height)
         
       
         If save = 1 And Not File.Exists(thumbToSaveToDisk) Then
            ' create the file an return it!                          
            Try
               target.Save(thumbToSaveToDisk)
               target.Dispose()
            Catch ex As Exception
               target.Dispose()
               Me.ErrorResult(context)
               Return
            End Try
         End If
            
         'If File.Exists(thumbToSaveToDisk) Then
         '   Dim pict As Image = Image.FromFile(thumbToSaveToDisk)
         '   context.Response.ContentType = String.Format("image/{0}", imageExtension.Replace(".", ""))
         '   'context.Response.WriteFile(thumbToSaveToDisk)
         '   pict.Save(context.Response.OutputStream, ImageFormat.Jpeg)
         '   If cache <> 0 Then
         '      HttpContext.Current.Cache(thumbName) = pict
         '   End If
         '   Return
         'End If
             
         context.Response.ContentType = String.Format("image/{0}", imageExtension.Replace(".", ""))
         target.Save(context.Response.OutputStream, AssetsHelper.ImageFormatFromExtension(imageExtension))
         If cache <> 0 Then
            HttpContext.Current.Cache(thumbName) = target
         End If
         Return
        
                     
      Catch ex As Exception
         context.Response.ContentType = "text/plain"
         context.Response.Write("image not found")
      End Try
   End Sub
 
   Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
      Get
         Return False
      End Get
   End Property
    
   Private Sub ErrorResult(ByVal context As HttpContext)
      context.Response.Clear()
      context.Response.StatusCode = 404
      context.Response.[End]()
   End Sub
    
    
End Class