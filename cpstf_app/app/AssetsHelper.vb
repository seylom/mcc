Imports Microsoft.VisualBasic
Imports System.Drawing
Imports System.Drawing.Imaging


Public NotInheritable Class AssetsHelper
   ''' <summary>
   ''' This method returns a fully qualified absolute server Url which includes
   ''' the protocol, server, port in addition to the server relative Url.
   ''' 
   ''' Works like Control.ResolveUrl including support for ~ syntax
   ''' but returns an absolute URL.
   ''' </summary>
   ''' <param name="ServerUrl">Any Url, either App relative or fully qualified</param>
   ''' <param name="forceHttps">if true forces the url to use https</param>
   ''' <returns></returns>
   Public Shared Function ResolveServerUrl(ByVal serverUrl As String, ByVal forceHttps As Boolean) As String
      ' *** Is it already an absolute Url?

      If serverUrl.IndexOf("://") > -1 Then

         Return serverUrl
      End If

      ' *** Start by fixing up the Url an Application relative Url
      Dim newUrl As String = ResolveSiteUrl(serverUrl)
      Dim originalUri As Uri = HttpContext.Current.Request.Url
      newUrl = ((If(forceHttps, "https", originalUri.Scheme)) & "://") + originalUri.Authority + newUrl
      Return newUrl
   End Function





   ''' <summary>
   ''' This method returns a fully qualified absolute server Url which includes
   ''' the protocol, server, port in addition to the server relative Url.
   ''' 
   ''' It work like Page.ResolveUrl, but adds these to the beginning.
   ''' This method is useful for generating Urls for AJAX methods
   ''' </summary>
   ''' <param name="ServerUrl">Any Url, either App relative or fully qualified</param>
   ''' <returns></returns>
   Public Shared Function ResolveServerUrl(ByVal serverUrl As String) As String
      Return ResolveServerUrl(serverUrl, False)
   End Function

   ''' <summary>
   ''' Returns a site relative HTTP path from a partial path starting out with a ~.
   ''' Same syntax that ASP.Net internally supports but this method can be used
   ''' outside of the Page framework.
   ''' 
   ''' Works like Control.ResolveUrl including support for ~ syntax
   ''' but returns an absolute URL.
   ''' </summary>
   ''' <param name="originalUrl">Any Url including those starting with ~</param>
   ''' <returns>relative url</returns>

   Public Shared Function ResolveSiteUrl(ByVal originalUrl As String) As String
      If originalUrl Is Nothing Then
         Return Nothing
      End If
      ' *** Absolute path - just return

      If originalUrl.IndexOf("://") <> -1 Then

         Return originalUrl
      End If

      ' *** Fix up image path for ~ root app dir directory

      If originalUrl.StartsWith("~") Then
         Dim newUrl As String = ""
         If HttpContext.Current IsNot Nothing Then
            newUrl = HttpContext.Current.Request.ApplicationPath + originalUrl.Substring(1).Replace("//", "/")
         Else
            ' *** Not context: assume current directory is the base directory
            Throw New ArgumentException("Invalid URL: Relative URL not allowed.")
         End If

         ' *** Just to be sure fix up any double slashes
         Return newUrl
      End If

      Return originalUrl
   End Function

   Public Shared Function CreateThumbnail(ByVal lcFilename As String, ByVal lnWidth As Integer, ByVal lnHeight As Integer) As Bitmap

      Dim bmpOut As System.Drawing.Bitmap = Nothing
      Try

         Dim loBMP As New Bitmap(lcFilename)
         Dim loFormat As ImageFormat = loBMP.RawFormat
         Dim lnRatio As Decimal
         Dim lnNewWidth As Integer = 0
         Dim lnNewHeight As Integer = 0

         'If the image is smaller than a thumbnail just return it 

         If loBMP.Width < lnWidth AndAlso loBMP.Height < lnHeight Then
            Return loBMP
         End If


         If loBMP.Width > loBMP.Height Then
            lnRatio = CDec(lnWidth) / loBMP.Width
            lnNewWidth = lnWidth
            Dim lnTemp As Decimal = loBMP.Height * lnRatio
            lnNewHeight = CInt(lnTemp)
         Else
            lnRatio = CDec(lnHeight) / loBMP.Height
            lnNewHeight = lnHeight
            Dim lnTemp As Decimal = loBMP.Width * lnRatio
            lnNewWidth = CInt(lnTemp)
         End If

         ' System.Drawing.Image imgOut = 
         ' loBMP.GetThumbnailImage(lnNewWidth,lnNewHeight, 
         ' null,IntPtr.Zero); 
         ' This code creates cleaner (though bigger) thumbnails and properly 
         ' and handles GIF files better by generating a white background for 
         ' transparent images (as opposed to black)

         bmpOut = New Bitmap(lnNewWidth, lnNewHeight)
         Dim g As Graphics = Graphics.FromImage(bmpOut)
         g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic
         g.FillRectangle(Brushes.White, 0, 0, lnNewWidth, lnNewHeight)
         g.DrawImage(loBMP, 0, 0, lnNewWidth, lnNewHeight)
         loBMP.Dispose()

      Catch ex As Exception
         Return Nothing
      End Try

      Return bmpOut
   End Function


   Public Shared Function ImageFormatFromExtension(ByVal ext As String) As ImageFormat
      Select Case ext.ToLower
         Case ".bmp"
            Return ImageFormat.Bmp
         Case ".jpg", "jpeg"
            Return ImageFormat.Jpeg
         Case ".png"
            Return ImageFormat.Png
         Case ".gif"
            Return ImageFormat.Gif
         Case ".tiff"
            Return ImageFormat.Tiff
         Case Else
            Return ImageFormat.Jpeg
      End Select
   End Function


End Class
