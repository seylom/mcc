Imports Microsoft.VisualBasic
Imports System.Linq
Imports System.IO
Imports System
Imports System.Collections.Generic
Imports System.Web
Imports System.Linq.Expressions
Imports System.Xml
Imports System.Xml.Linq
Imports MCC.Data
Imports MCC.Services
Imports System.Threading
Imports System.Net.Mail
Imports System.Net.Configuration
Imports System.Web.Configuration

Public Class Utils
   Inherits mccObject

   Private Shared _settings As RegistrySettings
   Public Shared Property SiteSettings() As RegistrySettings
      Get
         If _settings Is Nothing Then
            _settings = New RegistrySettings
         End If
         Return _settings
      End Get
      Set(ByVal value As RegistrySettings)
         _settings = value
      End Set
   End Property

   Public Shared ReadOnly Property Version() As String
      Get
         Dim v As String = ""
         Dim rootWebConfig As System.Configuration.Configuration
         rootWebConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath)
         If (0 < rootWebConfig.AppSettings.Settings.Count) Then
            Dim customSetting As System.Configuration.KeyValueConfigurationElement
            customSetting = rootWebConfig.AppSettings.Settings("version")
            If customSetting Is Nothing Then
               v = "100"
            Else
               v = customSetting.Value
            End If
         End If
         Return v
      End Get
   End Property

   Public Shared Function FileVersion(ByVal name As String) As String

      Dim v As String = ""
      Dim rootWebConfig As System.Configuration.Configuration
      rootWebConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath)
      If (0 < rootWebConfig.AppSettings.Settings.Count) Then
         Dim customSetting As System.Configuration.KeyValueConfigurationElement
         customSetting = rootWebConfig.AppSettings.Settings(name)
         If customSetting Is Nothing Then
            v = "100"
         Else
            v = customSetting.Value
         End If
      End If

      Return v
   End Function


   Public Shared Function AssetPath(ByVal name As String) As String

      Dim v As String = ""
      Dim rootWebConfig As System.Configuration.Configuration
      rootWebConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath)
      If (0 < rootWebConfig.AppSettings.Settings.Count) Then
         Dim customSetting As System.Configuration.KeyValueConfigurationElement
         customSetting = rootWebConfig.AppSettings.Settings(name)
         If customSetting Is Nothing Then
            v = "~/"
         Else
            v = customSetting.Value
         End If
      Else
         v = "~/"
      End If

      Return v
   End Function


   Public Shared Function GetThemes() As String()
      If HttpContext.Current.Cache("MccThemes") IsNot Nothing Then
         Return CType(HttpContext.Current.Cache("MccThemes"), String())
      Else
         Dim themesDirPath As String = HttpContext.Current.Server.MapPath("~/App_Themes")
         ' get the array of themes folders under /app_themes
         Dim themes() As String = Directory.GetDirectories(themesDirPath)
         For i As Integer = 0 To themes.Length - 1
            themes(i) = Path.GetFileName(themes(i))
            ' cache the array with a dependency to the folder
            Dim dep As CacheDependency = New CacheDependency(themesDirPath)
            HttpContext.Current.Cache.Insert("MccThemes", themes, dep)
         Next
         Return themes
      End If
   End Function

   Public Shared Function BadWords() As List(Of String)
      Dim mdc As New MCCDataContext()
      Dim str As List(Of String) = (From it As mcc_BadWord In mdc.mcc_BadWords Select it.Word).ToList()
      Return str
   End Function


   Public Shared Function GetStatusCaption(ByVal val As Integer) As String
      Dim key As String = "status_caption_" & val
      If Cache(key) IsNot Nothing Then
         Return Cache(key).ToString
      Else
         Dim mdc As New MCCDataContext()
         Dim mtt As String = (From it As mcc_Status In mdc.mcc_Status Where it.value = val Select it.Title).FirstOrDefault
         CacheData(key, mtt)
         Return mtt
      End If
   End Function


   Protected Shared Sub CacheData(ByVal key As String, ByVal data As Object)
      If data IsNot Nothing Then
         mccObject.Cache.Insert(key, data, Nothing, DateTime.Now.AddSeconds(3600), TimeSpan.Zero)
      End If
   End Sub

   Public Shared Function GetCurrentStatus(ByVal status As Status) As Integer
      Return CInt(status)
   End Function


   Public Shared Sub PurgeCache()
      Dim CacheEnum As IDictionaryEnumerator = Cache.GetEnumerator
      While (CacheEnum.MoveNext)
         Cache.Remove(CacheEnum.Key.ToString)
      End While
   End Sub

   Public Shared Function ImageExists(ByVal imageUrl As String) As Boolean
      'If Not imageUrl.StartsWith("http://") Then
      '   If Not String.IsNullOrEmpty(imageUrl) AndAlso File.Exists(HttpContext.Current.Server.MapPath(imageUrl)) Then
      '      Return True
      '   Else
      '      Return False
      '   End If
      'Else

      'End If
      Return True
   End Function


   Public Shared Function FileExists(ByVal url As String) As Boolean
      If Not url.StartsWith("http://") Then
         If Not String.IsNullOrEmpty(url) AndAlso File.Exists(HttpContext.Current.Server.MapPath(url)) Then
            Return True
         Else
            Return False
         End If
      End If
   End Function


   Public Shared Function ConvertBBCodeToHTML(ByVal str As String) As String

      If String.IsNullOrEmpty(str) Then
         Return str
      End If

      Dim exp As Regex

      ' format the bold tags: [b][/b]
      ' becomes: <strong></strong>
      exp = New Regex("\[b\](.+?)\[/b\]")
      str = exp.Replace(str, "<strong>$1</strong>")

      ' format the italic tags: [i][/i]
      ' becomes: <em></em>
      exp = New Regex("\[i\](.+?)\[/i\]")
      str = exp.Replace(str, "<em>$1</em>")

      ' format the underline tags: [u][/u]
      ' becomes: <u></u>
      exp = New Regex("\[u\](.+?)\[/u\]")
      str = exp.Replace(str, "<u>$1</u>")

      ' format the strike tags: [s][/s]
      ' becomes: <strike></strike>
      exp = New Regex("\[s\](.+?)\[/s\]")
      str = exp.Replace(str, "<strike>$1</strike>")

      ' format the url tags: [url=www.website.com]my site[/url]
      ' becomes: <a href="www.website.com">my site</a>

      exp = New Regex("\[url\=([^\]]+)\]([^\]]+)\[/url\]")
      str = exp.Replace(str, "<a href=""$1"">$2</a>")

      ' format the img tags: [img]www.website.com/img/image.jpeg[/img]
      ' becomes: <img src="www.website.com/img/image.jpeg" />
      exp = New Regex("\[img\]([^\]]+)\[/img\]")
      str = exp.Replace(str, "<img src=""$1"" />")

      ' format img tags with alt: [img=www.website.com/img/image.jpeg]this is the alt text[/img]
      ' becomes: <img src="www.website.com/img/image.jpeg" alt="this is the alt text" />
      exp = New Regex("\[img\=([^\]]+)\]([^\]]+)\[/img\]")
      str = exp.Replace(str, "<img src=""$1"" alt=""$2"" />")

      ' format list tags: [list][*]alpha[*]beta[/list]
      ' becomes: <ul><li>alpha</li><li>beta</li></ul>

      exp = New Regex("\[list\]((:?[\r\n]|\s)*)(((\[\*\])(.*)(:?[\r\n]|\s)*)*)?\[/list\]")
      str = exp.Replace(str, "<ul>$3</ul>")

      exp = New Regex("\[\*\](.*)(:?[\r\n]|\s)")
      str = exp.Replace(str, "<li>$1</li>$2")

      'Dim strtmp As MatchCollection = exp.Matches(str)

      'For Each it As Match In strtmp
      '   If it.Value.Length > 0 Then
      '      Dim expli As Regex = New Regex("\[\*\](.*)(:?[\r\n]|\s)")



      '   End If
      'Next

      str = exp.Replace(str, "<li>$1</li>")

      'format the colour tags: [color=red][/color]
      ' becomes: <font color="red"></font>
      ' supports UK English and US English spelling of colour/color
      exp = New Regex("\[color\=([^\]]+)\]([^\]]+)\[/color\]")
      str = exp.Replace(str, "<font color=""$1"">$2</font>")
      exp = New Regex("\[colour\=([^\]]+)\]([^\]]+)\[/colour\]")
      str = exp.Replace(str, "<font color=""$1"">$2</font>")

      ' format the size tags: [size=3][/size]
      ' becomes: <font size="+3"></font>
      exp = New Regex("\[size\=([^\]]+)\]([^\]]+)\[/size\]")
      str = exp.Replace(str, "<font size=""+$1"">$2</font>")

      ' lastly, replace any new line characters with <br />
      str = str.Replace(vbCr & vbLf, "<br />" & vbCr & vbLf)
      Return str
   End Function



   Public Shared Function articlesMap() As String
      Dim mdc As New MCCDataContext()
      Dim li As List(Of mcc_Article) = (From it As mcc_Article In mdc.mcc_Articles Where it.Approved = True).ToList
      Dim ac As List(Of mcc_Category) = (From it As mcc_Category In mdc.mcc_Categories).ToList

      Dim baseUrl As String = routines.FullBaseUrl()

      'Dim strXml As XDocument = <?xml version="1.0" encoding="UTF-8"?>
      '                          <urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">
      '                             <!--auto generated by the middleclasscrunch handler-->
      '                             <%= From it As mcc_Category In ac Select _
      '                                <url>
      '                                   <loc><%= baseUrl + "articles/Topics/" + it.Slug + "/" %></loc>
      '                                   <priority>0.80</priority>
      '                                   <lastmod><%= DateTime.Now %></lastmod>
      '                                   <changefreq>weekly</changefreq>
      '                                </url> %>

      '                             <%= From it As mcc_Article In li _
      '                                Select _
      '                                <url>
      '                                   <loc><%= baseUrl + "articles/" + it.Slug %></loc>
      '                                   <priority>0.80</priority>
      '                                   <lastmod><%= DateTime.Now %></lastmod>
      '                                   <changefreq>weekly</changefreq>
      '                                </url> %>
      '                          </urlset>


      Dim ns As XNamespace = "http://www.sitemaps.org/schemas/sitemap/0.9"
      Dim elRoot As XElement = New XElement(ns + "urlset")

      For Each it As mcc_Category In ac
         Dim xe As New XElement(ns + "url", _
                        New XElement(ns + "loc", baseUrl + "articles/Topics/" + it.Slug + "/"), _
                        New XElement(ns + "priority", "0.6"), _
                        New XElement(ns + "lastmod", DateTime.Now), _
                        New XElement(ns + "changefreq", "weekly"))

         elRoot.Add(xe)
      Next


      For Each it As mcc_Article In li
         Dim xe As New XElement(ns + "url", _
                        New XElement(ns + "loc", baseUrl + "articles/" + it.ArticleID.ToString + "/" + it.Slug), _
                        New XElement(ns + "priority", "0.8"), _
                        New XElement(ns + "lastmod", DateTime.Now), _
                        New XElement(ns + "changefreq", "weekly"))
         elRoot.Add(xe)
      Next


      Dim strXml As XDocument = New XDocument(New XDeclaration("1.0", "utf-8", "yes"), elRoot)
      Return strXml.ToString()
   End Function

   Public Shared Function tipsMap() As String
      Dim mdc As New MCCDataContext()
      Dim li As List(Of mcc_Advice) = (From it As mcc_Advice In mdc.mcc_Advices Where it.Approved = True).ToList
      Dim ac As List(Of mcc_AdviceCategory) = (From it As mcc_AdviceCategory In mdc.mcc_AdviceCategories).ToList
      Dim baseUrl As String = routines.FullBaseUrl()

      Dim ns As XNamespace = "http://www.sitemaps.org/schemas/sitemap/0.9"
      Dim elRoot As XElement = New XElement(ns + "urlset")

      For Each it As mcc_AdviceCategory In ac
         Dim xe As New XElement(ns + "url", _
                        New XElement(ns + "loc", baseUrl + "tips/Topics/" + it.Slug + "/"), _
                        New XElement(ns + "priority", "0.6"), _
                        New XElement(ns + "lastmod", DateTime.Now), _
                        New XElement(ns + "changefreq", "weekly"))
         elRoot.Add(xe)
      Next


      'For Each it As mcc_Advice In li
      '   Dim xe As New XElement(ns + "url", _
      '                  New XElement(ns + "loc", baseUrl + "tips/" + it.AdviceID.ToString + "/" + it.Slug), _
      '                  New XElement(ns + "priority", "0.8"), _
      '                  New XElement(ns + "lastmod", DateTime.Now), _
      '                  New XElement(ns + "changefreq", "weekly"))
      '   elRoot.Add(xe)
      'Next

      Dim strXml As XDocument = New XDocument(New XDeclaration("1.0", "utf-8", "yes"), elRoot)

      Return strXml.ToString
   End Function


   Public Shared Function videosMap() As String
      Dim mdc As New MCCDataContext()
      Dim li As List(Of mcc_Video) = (From it As mcc_Video In mdc.mcc_Videos Where it.Approved = True).ToList
      'Dim ac As List(Of mcc_) = (From it As mcc_AdviceCategory In mdc.mcc_AdviceCategories).ToList
      Dim baseUrl As String = routines.FullBaseUrl()

      Dim strXml As XDocument = <?xml version="1.0" encoding="UTF-8"?>
                                <urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">
                                   <!--auto generated by the middleclasscrunch handler-->


                                   <%= From it As mcc_Video In li _
                                      Select _
                                      <url>
                                         <loc><%= baseUrl + "videos/" + it.VideoId.ToString + "/" + it.Slug %></loc>
                                         <priority>0.80</priority>
                                         <lastmod><%= DateTime.Now %></lastmod>
                                         <changefreq>weekly</changefreq>
                                      </url> %>
                                </urlset>
      Return strXml.ToString
   End Function

   Public Shared Function questionsMap() As String
      Dim mdc As New MCCDataContext()
      Dim li As List(Of mcc_UserQuestion) = (From it As mcc_UserQuestion In mdc.mcc_UserQuestions).ToList
      Dim baseUrl As String = routines.FullBaseUrl()

      Dim ns As XNamespace = "http://www.sitemaps.org/schemas/sitemap/0.9"
      Dim elRoot As XElement = New XElement(ns + "urlset")

      For Each it As mcc_UserQuestion In li
         Dim xe As New XElement(ns + "url", _
                        New XElement(ns + "loc", baseUrl + "questions/" + it.UserQuestionId.ToString + "/" + it.Slug), _
                        New XElement(ns + "priority", "0.8"), _
                        New XElement(ns + "lastmod", DateTime.Now), _
                        New XElement(ns + "changefreq", "weekly"))
         elRoot.Add(xe)
      Next

      Dim strXml As XDocument = New XDocument(New XDeclaration("1.0", "utf-8", "yes"), elRoot)

      Return strXml.ToString
   End Function


#Region "Send e-mail"

   ''' <summary>
   ''' Sends a MailMessage object using the SMTP settings.
   ''' </summary>
   ''' <param name="message"></param>
   ''' <remarks></remarks>
   Public Shared Sub SendMailMessage(ByVal message As MailMessage)

      If (message Is Nothing) Then
         Throw New ArgumentNullException("message")
      End If

      Try
         message.IsBodyHtml = True
         message.BodyEncoding = Encoding.UTF8
         Dim smtp As SmtpClient = New SmtpClient()

         'Dim config As System.Configuration.Configuration = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath)
         'Dim settings As MailSettingsSectionGroup = CType(config.GetSectionGroup("system.net/mailSettings"), MailSettingsSectionGroup)

         ' don't send credentials if a server doesn't require it,
         ' linux smtp servers don't like that

         'If (Not String.IsNullOrEmpty(BlogSettings.Instance.SmtpUserName)) Then
         '   smtp.Credentials = New System.Net.NetworkCredential(settings., BlogSettings.Instance.SmtpPassword)
         'End If

         'smtp.Port = BlogSettings.Instance.SmtpServerPort
         'smtp.EnableSsl = BlogSettings.Instance.EnableSsl


         smtp.Send(message)

         'OnEmailSent(message)

      Catch ex As SmtpException
         'OnEmailFailed(message)
      Finally
         'Remove the pointer to the message object so the GC can close the thread.
         message.Dispose()
         message = Nothing
      End Try
   End Sub

   ''' <summary>
   '''  Sends the mail message asynchronously in another thread.
   ''' </summary>
   ''' <remarks></remarks>
   Public Shared Sub SendMailMessageAsync(ByVal message As MailMessage)
      ThreadPool.QueueUserWorkItem(AddressOf Utils.SendMailMessage, message)
   End Sub


#End Region


End Class