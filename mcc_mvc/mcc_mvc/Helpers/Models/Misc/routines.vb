Imports Microsoft.VisualBasic
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Json
Imports System.IO
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Web.UI
Imports System.Net
Imports System.Web
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Net.Mail
Imports System.Net.Configuration
Imports System.Web.Configuration
Imports System
Imports System.Collections.Generic
Imports MCC.Data


Public Module routines

   Public Function SiteUrl() As String
      Return "http://www.middleclasscrunch.com"
   End Function

   Public Function Encode(ByVal str As String) As String
      Return HttpContext.Current.Server.HtmlEncode(str)
   End Function

   Public Function Decode(ByVal str As String) As String
      Return HttpContext.Current.Server.HtmlDecode(str)
   End Function

   Public Function BaseUrl() As String
      Dim url As String = HttpContext.Current.Request.ApplicationPath
      If url.EndsWith("/") Then
         Return url
      Else
         Return url + "/"
      End If
   End Function


   Public Function FullBaseUrl() As String
      Return HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.PathAndQuery, "") + BaseUrl()
   End Function

   Public Function FixUrl(ByVal inURL As String) As String
        Dim str As String = IIf(inURL.StartsWith("~"), _
                  HttpContext.Current.Request.ApplicationPath & inURL.Substring(1), _
                  inURL).Replace("//", "/")
        Return str
   End Function


   '<Runtime.CompilerServices.Extension()> _
   'Public Function CleanString(ByVal str As String, Optional ByVal replacewith As String = "-") As String
   '   Dim repWith As String = "-"
   '   If Not String.IsNullOrEmpty(replacewith) Then
   '      repWith = replacewith
   '   End If

   '   'str = str.Trim.Replace(" ", repWith)
   '   'str = Regex.Replace(str, "[@^\.?:\/*""<>|'\$,!;&#%()]", repWith).ToLower

   '   str = Regex.Replace(str, "[^\w-]", repWith).ToLower
   '   str = str.TrimEnd(repWith).TrimStart(repWith)

   '   Return str
   'End Function



   'Public Function BuildUrlFromTitleAndId(ByVal Id As Integer, ByVal title As String) As String
   '   Dim str As String = ""
   '   str = GetSlugFromString(title)
   '   'str = Id.ToString + "_" + str + ".aspx"
   '   str += ".aspx"
   '   Return str
   'End Function


   'Public Function GetSlugFromString(ByVal str As String, Optional ByVal replaceWith As String = "-") As String
   '   Dim slug As String = ""
   '   slug = CleanString(str, replaceWith)
   '   Return slug
   'End Function

   Public Function AbsUrl(ByVal relativeResolvedUrl As String) As String
      If (HttpContext.Current.Request.IsSecureConnection) Then
         Return String.Format("https://{0}{1}", HttpContext.Current.Request.Url.Host, relativeResolvedUrl)
      Else
         Return String.Format("http://{0}{1}", HttpContext.Current.Request.Url.Host, relativeResolvedUrl)
      End If
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

   Public Function ResolveSiteUrl(ByVal originalUrl As String) As String
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


   'Public  Function ResolveStaticFileUrl(ByVal originalUrl As String) As String
   '   If originalUrl Is Nothing Then
   '      Return Nothing
   '   End If
   '   ' *** Absolute path - just return

   '   If originalUrl.IndexOf("://") <> -1 Then
   '      Return originalUrl
   '   End If

   '   ' *** Fix up image path for ~ root app dir directory

   '   If originalUrl.StartsWith("~") Then
   '      Dim newUrl As String = ""
   '      If HttpContext.Current IsNot Nothing Then


   '         newUrl = "http://static.middleclasscrunch.com" + HttpContext.Current.Request.ApplicationPath + originalUrl.Substring(1).Replace("//", "/")
   '         'newUrl = HttpContext.Current.Request.ApplicationPath + originalUrl.Substring(1).Replace("//", "/")
   '      Else
   '         ' *** Not context: assume current directory is the base directory
   '         Throw New ArgumentException("Invalid URL: Relative URL not allowed.")
   '      End If

   '      ' *** Just to be sure fix up any double slashes
   '      Return newUrl
   '   Else
   '      Return Nothing
   '   End If
   'End Function


   Public Function ToMinutesAndSeconds(ByVal Seconds As Double, Optional ByVal Verbose As Boolean = False) As String

      Dim ts As TimeSpan = TimeSpan.FromSeconds(Seconds)
      Dim out As String = ts.Minutes.ToString("##00") & ":" & ts.Seconds.ToString("##00")
      Return out


      ''if verbose = false, returns
      ''something like
      ''02:22.08
      ''if true, returns
      ''2 hours, 22 minutes, and 8 seconds

      'Dim lHrs As Long
      'Dim lMinutes As Long
      'Dim lSeconds As Long

      'lSeconds = Seconds

      'lHrs = Math.Floor(lSeconds / 3600)
      'lMinutes = (Math.Floor(lSeconds / 60)) - (lHrs * 60)
      'lSeconds = Math.Floor(lSeconds Mod 60)

      'Dim sAns As String

      'If lSeconds = 60 Then
      '   lMinutes = lMinutes + 1
      '   lSeconds = 0
      'End If

      'If lMinutes = 60 Then
      '   lMinutes = 0
      '   lHrs = lHrs + 1
      'End If

      'If lHrs = 0 Then
      '   sAns = Format(lMinutes.ToString, "0") & ":" & Format(lSeconds.ToString, "00")
      'Else
      '   sAns = Format(lHrs.ToString, "#####0") & ":" & Format(lMinutes.ToString, "0") & ":" & Format(lSeconds.ToString, "00")
      'End If

      'If Verbose Then
      '   sAns = TimeStringtoEnglish(sAns)
      'End If

      'Return sAns

   End Function

   Private Function TimeStringToEnglish(ByVal sTimeString As String) As String

      Dim sAns As String = ""
      Dim sHour, sMin As String, sSec As String
      Dim iTemp As Integer, sTemp As String
      Dim iPos As Integer
      iPos = InStr(sTimeString, ":") - 1

      sHour = Left$(sTimeString, iPos)
      If CLng(sHour) <> 0 Then
         sAns = CLng(sHour) & " hour"
         If CLng(sHour) > 1 Then sAns = sAns & "s"
         sAns = sAns & ", "
      End If

      sMin = Mid$(sTimeString, iPos + 2, 2)

      iTemp = sMin

      If sMin = "00" Then
         sAns = IIf(Len(sAns), sAns & "0 minutes, and ", "")
      Else
         sTemp = IIf(iTemp = 1, " minute", " minutes")
         sTemp = IIf(Len(sAns), sTemp & ", and ", sTemp & " and ")
         sAns = sAns & Format$(iTemp, "##") & sTemp
      End If

      iTemp = Val(Right$(sTimeString, 2))
      sSec = Format$(iTemp, "#0")
      sAns = sAns & sSec & " second"
      If iTemp <> 1 Then
         sAns = sAns & "s"
      End If


      Return sAns

   End Function

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
   Public Function ResolveServerUrl(ByVal serverUrl As String, ByVal forceHttps As Boolean) As String
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
   Public Function ResolveServerUrl(ByVal serverUrl As String) As String
      Return ResolveServerUrl(serverUrl, False)
   End Function

   Public Function ArticlePageCount(ByVal body As String) As Integer
      Dim it As Integer = 1
      If body.IndexOf("<p><!-- pagebreak --></p>") <> -1 Then
         Dim Pages As String() = Regex.Split(body, "<p><!-- pagebreak --></p>")
         Dim TotalPages As Integer = Pages.GetUpperBound(0) + 1
         it = TotalPages
      End If
      Return it
   End Function


   Public Function GetArticlePageContent(ByVal body As String, ByVal pageIndex As Integer) As String
      Dim Output As String
      If body.IndexOf("<p><!-- pagebreak --></p>") <> -1 Then
         Dim Pages As String() = Regex.Split(body, "<p><!-- pagebreak --></p>")
         Dim TotalPages As Integer = Pages.GetUpperBound(0) + 1
         Dim Counter As Integer = 1
         Dim PageNumber As Integer = 1
         If PageNumber <= Pages.Length Then
            Output = Pages(PageNumber - 1)
         Else
            Output = ""
         End If
      Else
         Output = body
      End If
      Return Output
   End Function


   Public Function PageArticle(ByVal Article As String, ByRef pagecount As Integer) As String
      Dim Output As String


      Dim url As String = HttpContext.Current.Request.RawUrl
      Dim baseUrl = ""

      If HttpContext.Current.Request.RawUrl.IndexOf("?") <> -1 Then
         baseUrl = url.Substring(0, url.IndexOf("?"))
      Else
         baseUrl = url
      End If

      If Not baseUrl.EndsWith("/") Then
         baseUrl += "/"
      End If


      Dim PageNo As String
      PageNo = HttpContext.Current.Request.QueryString("Page")

      If Article.IndexOf("<p><!-- pagebreak --></p>") <> -1 Then
         Dim Pages As String() = Regex.Split(Article, "<p><!-- pagebreak --></p>")
         Dim TotalPages As Integer = Pages.GetUpperBound(0) + 1
         pagecount = TotalPages
         Dim Counter As Integer = 1
         Dim PageNumber As Integer = 1
         If PageNo IsNot Nothing Then
            PageNumber = System.Convert.ToInt32(PageNo)
         End If
         Output = Pages(PageNumber - 1)


         Dim head_foot_paging As String = "<p><b>Page</b> "
         While Counter <= TotalPages
            If Counter = PageNumber Then
               head_foot_paging += Counter.ToString() & " "
            Else
               If Counter = 1 Then
                  head_foot_paging += ("<a class='lnkpage' href='" & baseUrl)
               Else
                  head_foot_paging += ("<a class='lnkpage' href='" & baseUrl & "?Page=") + Counter.ToString()
               End If
               head_foot_paging += "'><span class='page'>" & Counter.ToString() & "</span></a> "
            End If
            Counter += 1
         End While
         head_foot_paging += "</p>"

         Output = head_foot_paging + Output + head_foot_paging
      Else
         pagecount = 1
         Output = Article
      End If
      Return Output
   End Function



   Public Function CreateCroppedThumbnail(ByVal imgFilePath As String, ByVal thumbName As String, ByVal w As Integer, ByVal h As Integer, _
                                             ByVal x1 As Integer, ByVal y1 As Integer, ByVal dfWidth As Integer, ByVal dfheight As Integer) As Boolean
      Dim fi As New FileInfo(imgFilePath)
      If fi IsNot Nothing AndAlso fi.Exists Then
         Dim photo As Image = Image.FromFile(fi.FullName)

         'Try
         '   photo = New Bitmap(imgFilePath)
         'Catch generatedExceptionName As ArgumentException
         '   'Throw New HttpException(404, "image not found.")
         '   Return False
         'End Try

         Dim img As Image = CropImage(photo, New Rectangle(x1, y1, w, h))
         Dim strName As String = fi.Directory.FullName & "/" & thumbName
         Dim ext As String = Path.GetExtension(strName)
         If ext.ToLower = "jpg" Then
            img.SaveCompressed(strName, 95, String.Format("Image/{0}", ext.Replace(".", "")))
         Else
            img.Save(strName, ImageFormatFromExtension(ext))
         End If

         'If photo IsNot Nothing Then
         '   photo.Dispose()
         'End If
      Else
         Return False
      End If

      Return True
   End Function

   Private Function CropImage(ByVal img As Image, ByVal cropArea As Rectangle) As Image
      Dim bmpImage As Bitmap = New Bitmap(img)
      Dim bmpCrop As Bitmap = bmpImage.Clone(cropArea, img.PixelFormat)
      Return CType(bmpCrop, Image)
   End Function

   Public Function CreateThumbnail(ByVal loBMP As Bitmap, ByVal lnWidth As Integer, ByVal lnHeight As Integer) As Bitmap

      If loBMP Is Nothing Then
         Return Nothing
      End If
      Dim bmpOut As System.Drawing.Bitmap = Nothing
      Try

         'Dim loBMP As New Bitmap(lcFilename)
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




   Public Function GetThumbImagePath(ByVal imagepathrel As String, ByVal width As Integer, ByVal height As Integer) As String

      Dim strPath As String = String.Empty
      Dim imname As String = String.Empty
      Dim imgresToSave As String = String.Empty

      If Not String.IsNullOrEmpty(imagepathrel) AndAlso File.Exists(HttpContext.Current.Request.MapPath(imagepathrel)) Then
         ' if the specified thumbnail size was already created, use it

         imname = width.ToString & "_" & height.ToString & "_" & imagepathrel.Substring(imagepathrel.LastIndexOf("/") + 1)
         Dim npath As String = imagepathrel.Substring(0, imagepathrel.LastIndexOf("/")) & "/" & imname
         imgresToSave = HttpContext.Current.Request.MapPath(npath)

         Dim fi As New FileInfo(HttpContext.Current.Request.MapPath(imagepathrel))
         If File.Exists(fi.Directory.FullName & "/" & width.ToString & "_" & height.ToString & "_" & fi.Name) Then
            strPath = routines.ResolveServerUrl(npath)
            Return strPath
         End If
      End If


      Dim imgUrl As String = imagepathrel

      Dim bmp As Bitmap = Nothing
      Dim target As Bitmap = Nothing


      If File.Exists(HttpContext.Current.Server.MapPath(imgUrl)) Then
         Dim Path As String = HttpContext.Current.Server.MapPath(imgUrl)
         bmp = New Bitmap(Path)


         target = New Bitmap(width, height)
         Using graphics__1 As Graphics = Graphics.FromImage(target)
            graphics__1.FillRectangle(Brushes.White, 0, 0, width, height)
            graphics__1.CompositingQuality = CompositingQuality.HighQuality
            graphics__1.InterpolationMode = InterpolationMode.HighQualityBicubic
            graphics__1.CompositingMode = CompositingMode.SourceCopy
            graphics__1.DrawImage(bmp, 0, 0, width, height)
         End Using
      Else
         Return strPath
      End If

      If target Is Nothing Then
         Return strPath
      End If

      If target IsNot Nothing Then
         Try
            'context.Response.ContentType = "image/jpeg"
            'target.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg)

            ' save image to disk

            'SaveCompressedImageToStream(target, context.Response.OutputStream, 100, "image/jpeg")

            target.Save(imgresToSave)
            target.Dispose()
            Return imgresToSave
         Catch ex As Exception
            target.Dispose()
            bmp.Dispose()
         End Try
      End If

      Return strPath
   End Function


   Const ValidChars As String = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ"

   Public Function GenerateName(ByVal random As Random, ByVal length As Integer) As String
      Dim chars As Char() = New Char(length - 1) {}
      For i As Integer = 0 To length - 1
         chars(i) = ValidChars(random.[Next](ValidChars.Length))
      Next
      Return New String(chars).ToLower()
   End Function



   Public Function insertScript(ByVal relativeUrl As String) As String
      Dim rel As String = ResolveServerUrl(relativeUrl, False)
      Dim str As String = "<script type='text/javascript' src='" & rel & "'></script>"
      Return str
      'Dim rs As RegisteredScript = HttpContext.Current.Request.r 
      'cs.RegisterClientScriptInclude(relativeUrl, relativeUrl)
   End Function


   Public Function SaveCompressedImageToStream(ByVal img As Image, ByVal outStream As Stream, ByVal compression As Long, ByVal mimeType As String) As Boolean
      If img IsNot Nothing Then
         img.SaveToStreamCompressed(outStream, compression, mimeType)
         Return True
      End If
      Return False
   End Function


   Public Function ValidateEmail(ByVal emailsToValidate As String) As Boolean
      Dim emails() As String = emailsToValidate.Trim.Split(New Char() {","}, StringSplitOptions.RemoveEmptyEntries)
      Dim valid As Boolean = True
      Dim reg As New Regex("^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$")
      If emails.Length > 0 Then
         For Each it As String In emails
            If Not reg.IsMatch(it.Trim) Then
               valid = False
               Exit For
            End If
         Next
      Else
         valid = False
      End If

      Return valid
   End Function


   Public Function GetFtpPath(ByVal FileType As ImageType) As String
      Dim ftpPath As String = Configs.Paths.CdnImages

      Select Case FileType
         Case mccEnum.FileTypePath.Videos
            ftpPath = Configs.Paths.CdnVideos
         Case mccEnum.FileTypePath.Images
            ftpPath = Configs.Paths.CdnImages
         Case Else
            ftpPath = Configs.Paths.CdnImages
      End Select

      Return ftpPath
   End Function


   Public Function UploadLocalFileToFTP(ByVal filename As String, ByVal folderpath As String) As Boolean
      Dim statusCode As FtpStatusCode = FtpStatusCode.Undefined

      If String.IsNullOrEmpty(folderpath) Then
         Return False
      End If
      Try


         Dim fi As New FileInfo(filename)
         If fi.Exists Then

            Dim dirToMake As String = fi.Directory.Name

            'Dim folderpath As String = Configs.Paths.CdnVideos & fi.Directory.Name

            Dim uploadUrl As String = Configs.Paths.CdnFtpRoot & folderpath & "/"

            If uploadUrl.Length = 0 Then
               Return False
            End If

            Dim ftphelp As New ftpHelperObject(uploadUrl, Configs.Credentials.FTPUserId, Configs.Credentials.FTPPassword)

            If dirToMake.Length > 0 Then
               Dim DirArray As String()
               DirArray = folderpath.Split(New Char() {"/"}, StringSplitOptions.RemoveEmptyEntries)

               Dim CurrentDirToCreate As String = "/"
               Dim dir As String
               For Each dir In DirArray
                  CurrentDirToCreate += dir + "/"
                  ' Check if exists?
                  ftphelp.ftpServerIP = Configs.Paths.CdnFtpRoot
                  statusCode = ftphelp.MakeDir(CurrentDirToCreate)
               Next
               ftphelp.ftpServerIP = uploadUrl
            End If

            Dim result As Boolean = ftphelp.Upload(fi.FullName)
            Return result
         End If
      Catch

      End Try

      Return False
   End Function

   Public Function UploadToFTP(ByVal fileToUpload As HttpPostedFileBase, ByVal shortname As String, ByVal dirToMake As String, ByVal fileType As mccEnum.FileTypePath) As Boolean
      Dim statusCode As FtpStatusCode = FtpStatusCode.Undefined
      Try

         Dim folderpath As String = GetFtpPath(fileType) & dirToMake
         Dim uploadUrl As String = Configs.Paths.CdnFtpRoot & folderpath & "/"

         If uploadUrl.Length = 0 Then
            Return False
         End If

         'Dim ftphelp As New ftpHelperObject(uploadUrl, "0068268|middleclass", "krhibu756")
         Dim ftphelp As New ftpHelperObject(uploadUrl, Configs.Credentials.FTPUserId, Configs.Credentials.FTPPassword)

         If dirToMake.Length > 0 Then
            'uploadUrl += dirToMake & "/"


            Dim DirArray As String()
            DirArray = folderpath.Split(New Char() {"/"}, StringSplitOptions.RemoveEmptyEntries)

            Dim CurrentDirToCreate As String = "/"
            Dim dir As String
            For Each dir In DirArray
               CurrentDirToCreate += dir + "/"
               ' Check if exists?

               ftphelp.ftpServerIP = Configs.Paths.CdnFtpRoot
               statusCode = ftphelp.MakeDir(CurrentDirToCreate)
               'If statusCode <> FtpStatusCode.CommandOK Then
               '   'Return statusCode
               'End If
            Next

            ftphelp.ftpServerIP = uploadUrl

         End If

         statusCode = ftphelp.UploadHttpPostFile(fileToUpload, shortname)

         Return True
      Catch

      End Try

      Return False
   End Function


   ''Public  Function GetImageFolderNameByImageType(ByVal type As ImageType) As String
   ''   Dim strFolder As String = String.Empty
   ''   Select Case type
   ''      Case ImageType.ArticleImage
   ''         strFolder = Configs.Folders.ImagesFolder
   ''      Case ImageType.Avatar
   ''         strFolder = Configs.Folders.AvatarsFolder
   ''      Case ImageType.None
   ''         strFolder = ""
   ''      Case Else
   ''   End Select

   ''   Return strFolder
   ''End Function


   Function GravatarFromUsername(ByVal username As String, ByVal size As Integer)
      Dim muser As MembershipUser = Membership.GetUser(username)
      If muser IsNot Nothing Then
         Return Gravatar(muser.Email, size)
      End If
      Return appHelpers.ImageUrl("noavatar.gif")
   End Function

   Public Function Gravatar(ByVal email As String, ByVal size As Integer) As String

      If Not HttpContext.Current.Request.IsLocal Then



         If String.IsNullOrEmpty(email) Then
            Return Nothing
         End If
         Dim md5 As System.Security.Cryptography.MD5 = New System.Security.Cryptography.MD5CryptoServiceProvider()
         Dim result As Byte() = md5.ComputeHash(Encoding.ASCII.GetBytes(email))

         Dim hash As New System.Text.StringBuilder()
         For i As Integer = 0 To result.Length - 1
            hash.Append(result(i).ToString("x2"))
         Next

         Dim imageUrl As New System.Text.StringBuilder()
         'imageUrl.Append("<img src=""")
         imageUrl.Append("http://www.gravatar.com/avatar.php?")
         imageUrl.Append("gravatar_id=" & hash.ToString())
         imageUrl.Append("&amp;rating=G")
         imageUrl.Append("&amp;size=" & size)
         imageUrl.Append("&amp;d=identicon")
         'imageUrl.Append(Configs.Paths.CdnRoot & Configs.Paths.Images & "noavatar.gif")
         'imageUrl.Append(""" alt="""" />")
         Return imageUrl.ToString()
      Else
         Return appHelpers.ImageUrl("noavatar.gif")
      End If
   End Function

   ''' <summary>
   ''' This only return the nine first account
   ''' </summary>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Public Function MostActiveMembers() As MembershipUserCollection
      Dim ucount As Integer = 0
      Dim li As MembershipUserCollection = Membership.GetAllUsers(0, 9, ucount)
      Return li
   End Function


   Public Function IsValidResetCode(ByVal email As String, ByVal code As String) As Boolean
      ' TODO: implement the reset code validation
      Return True
   End Function


    Public Function SendMail(ByVal senderName As String, ByVal senderEmail As String, ByVal subject As String, ByVal body As String) As Boolean
        Dim smtpc As SmtpClient = New SmtpClient()

        Dim config As System.Configuration.Configuration = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath)
        Dim settings As MailSettingsSectionGroup = CType(config.GetSectionGroup("system.net/mailSettings"), MailSettingsSectionGroup)

        Dim mm As MailMessage = New MailMessage
        mm.From = New MailAddress(senderEmail, senderName)
        mm.To.Add(settings.Smtp.From)
        mm.Body = body
        mm.Subject = subject


        Try
            smtpc.Send(mm)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

   Public Function HighlightKeyword(ByVal InputTxt As String, ByVal keywords As String) As String

      Dim hRegex = New Regex(keywords.Replace(" ", "|").Trim(), RegexOptions.IgnoreCase)
      Return hRegex.Replace(InputTxt, New MatchEvaluator(AddressOf ReplaceKeywords))

      'Return Regex.Replace(InputTxt, "\b(" & Regex.Escape(Search_Str) & ")\b", _
      '        StartTag & "$1" & EndTag, RegexOptions.IgnoreCase)

   End Function

   Private Function ReplaceKeywords(ByVal m As Match) As String
      Return "<span class='search-highlight'>" & m.Value & "</span>"
   End Function

End Module




Module ImageExtensions
   Private Sub CompressAndSaveImage(ByVal img As Bitmap, ByVal fileName As String, ByVal quality As Long, ByVal mimeType As String)
      Dim parameters As New EncoderParameters(1)
      parameters.Param(0) = New EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality)
      img.Save(fileName, GetCodecInfo(mimeType), parameters)
   End Sub

   Private Function GetCodecInfo(ByVal mimeType As String) As ImageCodecInfo
      For Each encoder As ImageCodecInfo In ImageCodecInfo.GetImageEncoders()
         If encoder.MimeType = mimeType Then
            Return encoder
         End If
      Next
      Throw New ArgumentOutOfRangeException(String.Format("'{0}' not supported", mimeType))
   End Function

   <System.Runtime.CompilerServices.Extension()> _
   Public Sub SaveCompressed(ByVal img As Image, ByVal fileName As String, ByVal compression As Long, ByVal mimeType As String)
      Dim parameters As New EncoderParameters(1)
      parameters.Param(0) = New EncoderParameter(System.Drawing.Imaging.Encoder.Compression, compression)
      img.Save(fileName, GetCodecInfo(mimeType), parameters)
   End Sub

   <System.Runtime.CompilerServices.Extension()> _
   Public Sub SaveToStreamCompressed(ByVal img As Image, ByVal outpuStream As Stream, ByVal compression As Long, ByVal mimeType As String)
      Dim parameters As New EncoderParameters(1)
      parameters.Param(0) = New EncoderParameter(System.Drawing.Imaging.Encoder.Compression, compression)
      img.Save(outpuStream, GetCodecInfo(mimeType), parameters)
   End Sub

   Public Function ImageFormatFromExtension(ByVal ext As String) As ImageFormat
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
End Module




''' <summary>
''' Provides various image untilities, such as high quality resizing and the ability to save a JPEG.
''' </summary>
Public Module ImageUtilities

   ''' <summary>
   ''' A quick lookup for getting image encoders
   ''' </summary>
   Private m_encoders As Dictionary(Of String, ImageCodecInfo) = Nothing

   ''' <summary>
   ''' A quick lookup for getting image encoders
   ''' </summary>
   Public ReadOnly Property Encoders() As Dictionary(Of String, ImageCodecInfo)
      'get accessor that creates the dictionary on demand
      Get
         'if the quick lookup isn't initialised, initialise it
         If m_encoders Is Nothing Then
            m_encoders = New Dictionary(Of String, ImageCodecInfo)()
         End If

         'if there are no codecs, try loading them
         If m_encoders.Count = 0 Then
            'get all the codecs
            For Each codec As ImageCodecInfo In ImageCodecInfo.GetImageEncoders()
               'add each codec to the quick lookup
               m_encoders.Add(codec.MimeType.ToLower(), codec)
            Next
         End If

         'return the lookup
         Return m_encoders
      End Get
   End Property

   ''' <summary>
   ''' Resize the image to the specified width and height.
   ''' </summary>
   ''' <param name="image">The image to resize.</param>
   ''' <param name="width">The width to resize to.</param>
   ''' <param name="height">The height to resize to.</param>
   ''' <returns>The resized image.</returns>
   Public Function ResizeImage(ByVal image As System.Drawing.Image, ByVal width As Integer, ByVal height As Integer) As System.Drawing.Bitmap
      'a holder for the result
      Dim result As New Bitmap(width, height)

      'use a graphics object to draw the resized image into the bitmap
      Using graphics__1 As Graphics = Graphics.FromImage(result)
         'set the resize quality modes to high quality
         graphics__1.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality
         graphics__1.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic
         graphics__1.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality
         'draw the image into the target bitmap
         graphics__1.DrawImage(image, 0, 0, result.Width, result.Height)
      End Using

      'return the resulting bitmap
      Return result
   End Function

   ''' <summary> 
   ''' Saves an image as a jpeg image, with the given quality 
   ''' </summary> 
   ''' <param name="path">Path to which the image would be saved.</param> 
   ''' <param name="quality">An integer from 0 to 100, with 100 being the 
   ''' highest quality</param> 
   ''' <exception cref="ArgumentOutOfRangeException">
   ''' An invalid value was entered for image quality.
   ''' </exception>
   <System.Runtime.CompilerServices.Extension()> _
   Public Sub SaveJpeg(ByVal image As Image, ByVal path As String, ByVal quality As Integer)
      'ensure the quality is within the correct range
      If (quality < 0) OrElse (quality > 100) Then
         'create the error message
         Dim [error] As String = String.Format("Jpeg image quality must be between 0 and 100, with 100 being the highest quality.  A value of {0} was specified.", quality)
         'throw a helpful exception
         Throw New ArgumentOutOfRangeException([error])
      End If

      'create an encoder parameter for the image quality
      Dim qualityParam As New EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality)
      'get the jpeg codec
      Dim jpegCodec As ImageCodecInfo = GetEncoderInfo("image/jpeg")

      'create a collection of all parameters that we will pass to the encoder
      Dim encoderParams As New EncoderParameters(1)
      'set the quality parameter for the codec
      encoderParams.Param(0) = qualityParam
      'save the image using the codec and the parameters
      image.Save(path, jpegCodec, encoderParams)
   End Sub

   ''' <summary> 
   ''' Returns the image codec with the given mime type 
   ''' </summary> 
   Public Function GetEncoderInfo(ByVal mimeType As String) As ImageCodecInfo
      'do a case insensitive search for the mime type
      Dim lookupKey As String = mimeType.ToLower()

      'the codec to return, default to null
      Dim foundCodec As ImageCodecInfo = Nothing

      'if we have the encoder, get it to return
      If Encoders.ContainsKey(lookupKey) Then
         'pull the codec from the lookup
         foundCodec = Encoders(lookupKey)
      End If

      Return foundCodec
   End Function
End Module
