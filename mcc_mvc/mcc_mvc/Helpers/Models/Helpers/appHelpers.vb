Imports Microsoft.VisualBasic
Imports System.Web
Imports MCC.Services
Imports MCC.Data

Public NotInheritable Class appHelpers

   ''' <summary>
   ''' Builds a script url
   ''' </summary>
   ''' <param name="scriptFile"></param>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Public Shared Function ScriptsTagUrl(ByVal scriptFile As String, Optional ByVal version As Integer = -1) As String

      Dim path As String = String.Empty

      Dim vpath As String = ""
      If version > -1 Then
         vpath = "?v=" & version.ToString
      End If

      If scriptFile.StartsWith("~") Then
         path = VirtualPathUtility.ToAbsolute(scriptFile) & vpath
      Else
         Dim rs As String = Configs.Paths.CdnRoot & Configs.Paths.Scripts
         If rs.StartsWith("~/") Then
            path = VirtualPathUtility.ToAbsolute("~/_assets/scripts/" & scriptFile) & vpath
         Else
            path = rs & scriptFile & vpath
         End If
      End If

      Dim str As String = "<script type=""text/javascript"" src=""{0}"" ></script>"
      Return String.Format(str, path)
   End Function


   ''' <summary>
   ''' Builds a script url of type myscript-1234.js
   ''' </summary>
   ''' <param name="scriptFile"></param>
   ''' <returns></returns>
   ''' <remarks>I use url rewritting to point to the actual file</remarks>
   Public Shared Function AdvancedScriptsTagUrl(ByVal scriptFile As String, ByVal version As Integer) As String

      Dim path As String = String.Empty

      Dim fileWithoutExt As String = scriptFile.Substring(0, scriptFile.LastIndexOf("."))

      Dim vpath As String = ""
      If version > -1 Then
         vpath = "?v=" & version.ToString
      End If

      If scriptFile.StartsWith("~") Then
         path = VirtualPathUtility.ToAbsolute(scriptFile) & vpath
      Else
         Dim rs As String = Configs.Paths.CdnRoot & Configs.Paths.Scripts
         If rs.StartsWith("~/") Then
            path = VirtualPathUtility.ToAbsolute("~/_assets/scripts/" & fileWithoutExt & "-" & version.ToString & ".js") & vpath
         Else
            path = rs & scriptFile & vpath
         End If
      End If


      'If scriptFile.StartsWith("~") Then
      '   path = VirtualPathUtility.ToAbsolute(scriptFile)
      'Else
      '   path = VirtualPathUtility.ToAbsolute("~/_assets/scripts/" & fileWithoutExt & "-" & version.ToString & ".js")
      'End If

      Dim str As String = "<script type=""text/javascript"" src=""{0}"" ></script>"
      Return String.Format(str, path)
   End Function


   ''' <summary>
   ''' Builds a css url
   ''' </summary>
   ''' <param name="cssFile"></param>
   ''' <returns></returns>
   ''' <remarks>This is the default css path in _assets/css folder</remarks>
   Public Shared Function CssTagUrl(ByVal cssFile As String, Optional ByVal media As String = "screen") As String

      Dim path As String = String.Empty
      If cssFile.StartsWith("~") Then
         path = VirtualPathUtility.ToAbsolute(cssFile)
      Else
         Dim rs As String = Configs.Paths.CdnRoot & Configs.Paths.Css
         If rs.StartsWith("~/") Then
            path = VirtualPathUtility.ToAbsolute("~/_assets/css/" & cssFile)
         Else
            path = rs & cssFile
         End If
      End If

      Dim str As String = "<link type=""text/css"" href=""{0}"" rel=""stylesheet"" media=""{1}"" />"
      Return String.Format(str, path, media)
   End Function

   ''' <summary>
   ''' Builds a css url with version info
   ''' </summary>
   ''' <param name="cssFile"></param>
   ''' <returns></returns>
   ''' <remarks>This is the default css path in _assets/css folder</remarks>
   Public Shared Function AdvancedCssTagUrl(ByVal cssFile As String, ByVal version As Integer, Optional ByVal media As String = "screen") As String

      Dim path As String = String.Empty

      If cssFile.StartsWith("~") Then
         path = VirtualPathUtility.ToAbsolute(cssFile)
      Else
         Dim rs As String = Configs.Paths.CdnRoot & Configs.Paths.Css
         If rs.StartsWith("~/") Then
            path = VirtualPathUtility.ToAbsolute("~/_assets/css/" & cssFile) & "?v=" & version.ToString
         Else
            path = rs & cssFile & "?v=" & version
         End If
      End If

      Dim str As String = "<link type=""text/css"" href=""{0}"" rel=""stylesheet"" media=""{1}"" />"
      Return String.Format(str, path, media)
   End Function

   ''' <summary>
   ''' Builds a css url
   ''' </summary>
   ''' <param name="cssFile"></param>
   ''' <returns></returns>
   ''' <remarks>css in app_Themes</remarks>
   Public Shared Function CssThemeTagUrl(ByVal cssFile As String, Optional ByVal media As String = "screen") As String

      Dim path As String = VirtualPathUtility.ToAbsolute("~/app_Themes/" & cssFile)
      Dim str As String = "<link type=""text/css"" href=""{0}"" rel=""stylesheet"" media=""{1}"" />"
      Return String.Format(str, path, media)
   End Function


   ''' <summary>
   ''' Builds a css url
   ''' </summary>
   ''' <param name="cssFile"></param>
   ''' <returns></returns>
   ''' <remarks>css in app_Themes</remarks>
   Public Shared Function AdvancedCssThemeTagUrl(ByVal cssFile As String, ByVal version As Integer, Optional ByVal media As String = "screen") As String

      'Dim path As String = VirtualPathUtility.ToAbsolute("~/app_Themes/" & cssFile) & "?v=" & version

      Dim path As String = String.Empty

      If cssFile.StartsWith("~") Then
         path = VirtualPathUtility.ToAbsolute(cssFile) & version.ToString
      Else
         Dim rs As String = Configs.Paths.CdnRoot & Configs.Paths.Themes
         If rs.StartsWith("~/") Then
            path = VirtualPathUtility.ToAbsolute("~/app_themes/" & cssFile) & "?v=" & version
         Else
            path = rs & cssFile & "?v=" & version
         End If
      End If

      Dim str As String = "<link type=""text/css"" href=""{0}"" rel=""stylesheet"" media=""{1}"" />"
      Return String.Format(str, path, media)
   End Function


   ''' <summary>
   ''' Builds an image url
   ''' </summary>
   ''' <param name="imageFile"></param>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Public Shared Function ImageUrl(ByVal imageFile As String) As String
      If Not imageFile.StartsWith("~/") Then
         Dim rs As String = Configs.Paths.CdnRoot & Configs.Paths.Images
         If rs.StartsWith("~/") Then
            Return VirtualPathUtility.ToAbsolute("~/_assets/images/" & imageFile)
         Else
            Return rs & imageFile
         End If
      Else
         Return VirtualPathUtility.ToAbsolute(imageFile, Configs.Paths.CdnRoot)
      End If
   End Function

   ''' <summary>
   ''' Builds an image url
   ''' </summary>
   ''' <param name="imageFile"></param>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Public Shared Function StaticImageUrl(ByVal imageFile As String) As String
      If Not imageFile.StartsWith("~/") Then
         Dim rs As String = Configs.Paths.CdnRoot & Configs.Paths.Images
         If rs.StartsWith("~/") Then
            Return VirtualPathUtility.ToAbsolute("~/_assets/images/" & imageFile)
         Else
            Return rs & imageFile
         End If
      Else
         Return VirtualPathUtility.ToAbsolute(imageFile, Configs.Paths.CdnRoot)
      End If
   End Function

   ''' <summary>
   ''' Builds an image url
   ''' </summary>
   ''' <param name="imageFile"></param>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Public Shared Function UploadUrl(ByVal imageFile As String) As String
      If Not imageFile.StartsWith("~/") Then
         Dim rs As String = Configs.Paths.CdnRoot
         If rs.StartsWith("~/") Then
            Return VirtualPathUtility.ToAbsolute("~/" & imageFile)
         Else
            Return rs & imageFile
         End If
      Else
         Return VirtualPathUtility.ToAbsolute(imageFile, Configs.Paths.CdnRoot)
      End If
   End Function

   Public Shared Function SetPath(ByVal path As String) As String
      Return VirtualPathUtility.ToAbsolute(path)
   End Function

   Public Shared Function AdType(ByVal adValue As Integer) As String
      Return [Enum].GetName(GetType(adType), adValue)
   End Function

   Public Shared Function ImageType(ByVal adValue As Integer) As String
      Return [Enum].GetName(GetType(imageType), adValue)
   End Function


   Private Shared _tags As New Regex("<[^>]*(>|$)", RegexOptions.Singleline Or RegexOptions.ExplicitCapture Or RegexOptions.Compiled)
   Private Shared _whitelist As New Regex(vbCr & vbLf & "    ^</?(b(lockquote)?|code|d(d|t|l|el)|em|h(1|2|3)|i|kbd|li|ol|p(re)?|s(ub|up|trong|trike)?|ul)>$|" & vbCr & vbLf & "    ^<(b|h)r\s?/?>$", RegexOptions.Singleline Or RegexOptions.ExplicitCapture Or RegexOptions.Compiled Or RegexOptions.IgnorePatternWhitespace)
   Private Shared _whitelist_a As New Regex(vbCr & vbLf & "    ^<a\s" & vbCr & vbLf & "    href=""(\#\d+|(https?|ftp)://[-a-z0-9+&@#/%?=~_|!:,.;\(\)]+)""" & vbCr & vbLf & "    (\stitle=""[^""<>]+"")?\s?>$|" & vbCr & vbLf & "    ^</a>$", RegexOptions.Singleline Or RegexOptions.ExplicitCapture Or RegexOptions.Compiled Or RegexOptions.IgnorePatternWhitespace)
   Private Shared _whitelist_img As New Regex(vbCr & vbLf & "    ^<img\s" & vbCr & vbLf & "    src=""https?://[-a-z0-9+&@#/%?=~_|!:,.;\(\)]+""" & vbCr & vbLf & "    (\swidth=""\d{1,3}"")?" & vbCr & vbLf & "    (\sheight=""\d{1,3}"")?" & vbCr & vbLf & "    (\salt=""[^""<>]*"")?" & vbCr & vbLf & "    (\stitle=""[^""<>]*"")?" & vbCr & vbLf & "    \s?/?>$", RegexOptions.Singleline Or RegexOptions.ExplicitCapture Or RegexOptions.Compiled Or RegexOptions.IgnorePatternWhitespace)


   ''' <summary>
   ''' sanitize any potentially dangerous tags from the provided raw HTML input using 
   ''' a whitelist based approach, leaving the "safe" HTML tags
   ''' CODESNIPPET:4100A61A-1711-4366-B0B0-144D1179A937
   ''' </summary>
   Public Shared Function Sanitize(ByVal html As String) As String
      If [String].IsNullOrEmpty(html) Then
         Return html
      End If

      Dim tagname As String
      Dim tag As Match

      ' match every HTML tag in the input
      Dim tags As MatchCollection = _tags.Matches(html)
      For i As Integer = tags.Count - 1 To -1 + 1 Step -1
         tag = tags(i)
         tagname = tag.Value.ToLowerInvariant()

         If Not (_whitelist.IsMatch(tagname) OrElse _whitelist_a.IsMatch(tagname) OrElse _whitelist_img.IsMatch(tagname)) Then
            html = html.Remove(tag.Index, tag.Length)
            System.Diagnostics.Debug.WriteLine("tag sanitized: " & tagname)
         End If
      Next

      Return html
   End Function

   ''' <summary>
   ''' Builds a script url for files on the same local server
   ''' </summary>
   ''' <param name="scriptFile"></param>
   ''' <returns></returns>
   ''' <remarks>This method is created because of the same origin policy security</remarks>
   Public Shared Function LocalScriptsTagUrl(ByVal scriptFile As String, Optional ByVal version As Integer = -1) As String

      Dim path As String = String.Empty

      Dim vpath As String = ""
      If version > -1 Then
         vpath = "?v=" & version.ToString
      End If

      If scriptFile.StartsWith("~") Then
         path = VirtualPathUtility.ToAbsolute(scriptFile) & vpath
      Else
         path = VirtualPathUtility.ToAbsolute("~/_assets/scripts/" & scriptFile) & vpath
      End If

      Dim str As String = "<script type=""text/javascript"" src=""{0}"" ></script>"
      Return String.Format(str, path)
   End Function
End Class
