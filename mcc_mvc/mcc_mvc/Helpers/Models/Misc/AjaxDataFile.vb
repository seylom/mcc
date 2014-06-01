Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System.Data.SqlClient.SqlCommandBuilder
Imports Webdiyer.WebControls.Mvc
Imports MCC.routines
Imports System.Runtime.Serialization
Imports System.IO

Imports System.Linq

Imports MCC.Services
Imports MCC.Data


Public Class AjaxDataFile

   Public Function GetQuoteDataJSON() As String

      Dim qserv As New QuoteService()
      Dim li As PagedList(Of Quote) = qserv.GetQuotes(0, 10)
      Dim iIndex As Integer = 0
      Dim oBuilder As StringBuilder = New StringBuilder()
      oBuilder.Append("{" + Convert.ToChar(34) + "quotes" + Convert.ToChar(34) + ": [")
      For Each it As Quote In li
         oBuilder.Append("{")
         oBuilder.AppendFormat(Convert.ToChar(34) + "{0}" + Convert.ToChar(34) + " : " + Convert.ToChar(34) + "{1}" + Convert.ToChar(34) + ",", "id", it.QuoteID.ToString)
         oBuilder.AppendFormat(Convert.ToChar(34) + "{0}" + Convert.ToChar(34) + " : " + Convert.ToChar(34) + "{1}" + Convert.ToChar(34) + ",", "author", it.Author)
         oBuilder.AppendFormat(Convert.ToChar(34) + "{0}" + Convert.ToChar(34) + " : " + Convert.ToChar(34) + "{1}" + Convert.ToChar(34) + ",", "role", it.Role)
         oBuilder.AppendFormat(Convert.ToChar(34) + "{0}" + Convert.ToChar(34) + " : " + Convert.ToChar(34) + "{1}" + Convert.ToChar(34), "body", it.Body)
         oBuilder.Append("}")

         If iIndex <> li.Count - 1 Then
            oBuilder.Append(",")
         End If
         iIndex += 1
      Next

      oBuilder.Append("]}")
      Return oBuilder.ToString()
   End Function


   '''' <summary>
   '''' Retrieves a specific number of quotes
   '''' </summary>
   'Public Function GetQuotes(ByVal pageIndex As Integer, ByVal pageSize As Integer) As List(Of Quote)
   '   Dim strConn As String = ConfigurationManager.ConnectionStrings("LocalSqlServer").ConnectionString
   '   Using cn As New SqlConnection(strConn)
   '      Dim cmd As New SqlCommand("mcc_quotes_GetQuotes", cn)
   '      cmd.CommandType = CommandType.StoredProcedure
   '      cmd.Parameters.Add("@PageIndex", SqlDbType.Int).Value = pageIndex
   '      cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize
   '      cn.Open()
   '      Return GetQuoteCollectionFromReader(cmd.ExecuteReader(CommandBehavior.Default), False)
   '   End Using
   'End Function

   'Protected Function GetQuoteCollectionFromReader(ByVal reader As IDataReader, ByVal readBody As Boolean) As List(Of Quote)
   '   Dim quotes As New List(Of Quote)()
   '   While reader.Read()
   '      quotes.Add(GetQuoteFromReader(reader, readBody))
   '   End While
   '   Return quotes
   'End Function

   'Protected Overridable Function GetQuoteFromReader(ByVal reader As IDataReader, ByVal readBody As Boolean) As Quote
   '   Dim mQuote As New Quote(CInt(reader("QuoteID")), DirectCast(reader("AddedDate"), DateTime), reader("AddedBy").ToString(), _
   '                        reader("Author").ToString(), reader("Body").ToString(), reader("Role").ToString())
   '   Return mQuote
   'End Function

   Private homePageArt As Article

   Public Function FetchRecentPosts() As String
      'Dim _articleService As IArticleService = New ArticleService()
      Dim str As String = "Content Not Available"
      'Dim baseUrl As String = routines.FullBaseUrl()
      'Dim li As List(Of Article) = _articleService.GetArticles(True, 0, 4)
      'If li IsNot Nothing AndAlso li.Count <> 0 Then

      '   homePageArt = li(0)

      '   'building the recenp post sections
      '   Dim strbuilder As New StringBuilder()
      '   strbuilder.Append("<ul >")


      '   For Each it As Article In li
      '      strbuilder.Append("<li class='rcpost'><span class='date'>" + it.AddedDate.ToString("dd/MM") + " | </span>")
      '      Dim artTitle As String = BuildUrlFromTitleAndId(it.ArticleID, it.Title)

      '      strbuilder.Append("<a class='hpLink'" + _
      '                      "href='" + baseUrl + "articles/" + artTitle + "'>" + it.Title + "</a>" + _
      '                                                    "<div style='font-size: 10px; color: #9a9a9b; padding-left: 10px;'></div>" + _
      '                                               "</li>")

      '   Next
      '   strbuilder.Append("</ul>")

      '   Return strbuilder.ToString
      'End If
      Return str
   End Function

   Public Function FetchHomeArticle() As String
      Dim _articleService As IArticleService = New ArticleService()
      Dim str As String = "Content Not Available"
      Dim baseUrl As String = routines.FullBaseUrl()

      If homePageArt Is Nothing Then
         Dim li As List(Of Article) = _articleService.GetArticles(True, 0, 1)
         If li IsNot Nothing AndAlso li.Count <> 0 Then
            homePageArt = li(0)
         End If
      End If

      Dim strbuilder As New StringBuilder()

      If homePageArt IsNot Nothing Then

         If Not String.IsNullOrEmpty(homePageArt.ImageNewsUrl) AndAlso File.Exists(HttpContext.Current.Server.MapPath(homePageArt.ImageNewsUrl)) Then
            strbuilder.Append("<img style='float: left;' src='" + baseUrl + homePageArt.ImageNewsUrl.Replace("~/", "") + "' alt='" + homePageArt.Title + "'  /><br style='clear: left;' />")
         End If

         strbuilder.Append("<h2><a href='" + baseUrl + "articles/" + homePageArt.Slug + "'>" + homePageArt.Title + "</a></h2>")


         Dim strBd As String = ""
         If homePageArt.Body.Length > 800 Then
            strBd = RemoveHTMLTags(HttpContext.Current.Server.HtmlDecode(homePageArt.Body.Substring(0, 800)), 0) + " ..."
         Else
            strBd = RemoveHTMLTags(HttpContext.Current.Server.HtmlDecode(homePageArt.Body), 0)
         End If
         strbuilder.Append("<p>" + strBd + "</p>")
         strbuilder.Append("<p class='morebox'><a class='morelink' href='" + baseUrl + "articles/" + homePageArt.Slug + "'>Read</a></p>")

         Return strbuilder.ToString
      End If

      Return str
   End Function

   Function RemoveHTMLTags(ByVal strHTML As String, ByVal intWorkFlow As Integer) As String

      Dim regEx As Regex
      Dim regOptions As RegexOptions
      Dim strResult As String

      strResult = strHTML

      regOptions = regOptions And RegexOptions.IgnoreCase


      If intWorkFlow <> 1 Then
         regEx = New Regex("<[^>]*>", regOptions)
         strResult = regEx.Replace(strResult, "")
      End If

      If intWorkFlow > 0 And intWorkFlow < 3 Then
         regEx = New Regex("[<]", regOptions)
         strResult = regEx.Replace(strResult, "<")

         regEx = New Regex("[>]", regOptions)
         strResult = regEx.Replace(strResult, ">")
      End If

      regEx = Nothing
      Return strResult
   End Function


   Public Function GetCategoriesJSON() As String

      Dim _articleCategoryService As IArticleCategoryService = New ArticleCategoryService()
      Dim li As List(Of ArticleCategory) = _articleCategoryService.GetCategories()
      Dim iIndex As Integer = 0
      Dim oBuilder As StringBuilder = New StringBuilder()

      oBuilder.Append("<ul style='list-style-type: square; margin-left: 10px;'>")
      For Each it As ArticleCategory In li
         If iIndex < 4 Then
            oBuilder.Append("<li><a class='global'  href='articles/topics/" + it.Slug + "/' title='" + it.Title + "'>" + it.Title + "</a></li>")
         End If
         iIndex += 1
      Next

      oBuilder.Append("</ul>")
      Return oBuilder.ToString()
   End Function


   Public Function GetVideoJSON(ByVal id As Integer) As String
      Dim _videoservice As IVideoService = New VideoService()
      Dim vid As Video = _videoservice.GetVideoById(id)
      Dim oBuilder As StringBuilder = New StringBuilder()

      Dim baseUrl As String = routines.FullBaseUrl()
      Dim fLenth As Integer = 0

      Dim fi As FileInfo = New FileInfo(HttpContext.Current.Server.MapPath(vid.VideoUrl))
      If fi.Exists Then
         fLenth = fi.Length
      End If

      If vid Is Nothing Then
         vid = New Video()
      End If


      oBuilder.Append("{")
      oBuilder.AppendFormat("'" + "{0}" + "'" + " : " + Convert.ToChar(34) + "{1}" + Convert.ToChar(34) + ",", "id", vid.VideoId.ToString)
      oBuilder.AppendFormat(Convert.ToChar(34) + "{0}" + Convert.ToChar(34) + " : " + Convert.ToChar(34) + "{1}" + Convert.ToChar(34) + ",", "author", vid.AddedBy)
      oBuilder.AppendFormat(Convert.ToChar(34) + "{0}" + Convert.ToChar(34) + " : " + Convert.ToChar(34) + "{1}" + Convert.ToChar(34) + ",", "description", vid.Abstract)
      oBuilder.AppendFormat(Convert.ToChar(34) + "{0}" + Convert.ToChar(34) + " : " + Convert.ToChar(34) + "{1}" + Convert.ToChar(34) + ",", "duration", vid.Duration)
      oBuilder.AppendFormat(Convert.ToChar(34) + "{0}" + Convert.ToChar(34) + " : " + Convert.ToChar(34) + "{1}" + Convert.ToChar(34) + ",", "file", vid.VideoUrl.Replace("~/", ""))
      oBuilder.AppendFormat(Convert.ToChar(34) + "{0}" + Convert.ToChar(34) + " : " + Convert.ToChar(34) + "{1}" + Convert.ToChar(34) + ",", "link", baseUrl + vid.VideoUrl.Replace("~/", ""))
      oBuilder.AppendFormat(Convert.ToChar(34) + "{0}" + Convert.ToChar(34) + " : " + Convert.ToChar(34) + "{1}" + Convert.ToChar(34) + ",", "image", "")
      oBuilder.AppendFormat(Convert.ToChar(34) + "{0}" + Convert.ToChar(34) + " : " + Convert.ToChar(34) + "{1}" + Convert.ToChar(34) + ",", "start", "false")
      oBuilder.AppendFormat(Convert.ToChar(34) + "{0}" + Convert.ToChar(34) + " : " + Convert.ToChar(34) + "{1}" + Convert.ToChar(34) + ",", "title", vid.Title)
      oBuilder.AppendFormat(Convert.ToChar(34) + "{0}" + Convert.ToChar(34) + " : " + Convert.ToChar(34) + "{1}" + Convert.ToChar(34), "type", "video")
      oBuilder.Append("}")

      Return oBuilder.ToString
   End Function


   Public Function AdviceVote(ByVal id As Integer, ByVal agree As Boolean) As String

      Dim _adviceService As New AdviceService()
      Dim ret As String = ""

      If HttpContext.Current.Request.Cookies("mcc_adv_votelist") IsNot Nothing Then
         Dim str As HttpCookie = HttpContext.Current.Request.Cookies("mcc_adv_votelist")
         Dim cv As String = HttpContext.Current.Server.HtmlDecode(str.Value)
         Dim idlist() As String = cv.Split(";")
         For Each it As String In idlist
            If it = id.ToString Then
               Return False
            End If
         Next
      End If

      If agree Then
         ret = _adviceService.VoteUpAdvice(id)
      Else
         ret = _adviceService.VoteDownAdvice(id)
      End If

      Return ret
   End Function

   Public Function GetQuoteJSON() As String
      'Dim qs As New QuoteService()
      'Dim qid As Quote = qs.GetQuotes(0, 5)
      'Dim oBuilder As StringBuilder = New StringBuilder()

      'Dim baseUrl As String = FullBaseUrl()
      'Dim fLenth As Integer = 0

      ''Dim fi As FileInfo = New FileInfo(HttpContext.Current.Server.MapPath(vid.VideoUrl))
      ''If fi IsNot Nothing Then
      ''   fLenth = fi.Length
      ''End If


      'oBuilder.Append("{")
      'oBuilder.AppendFormat(Convert.ToChar(34) + "{0}" + Convert.ToChar(34) + " : " + Convert.ToChar(34) + "{1}" + Convert.ToChar(34) + ",", "id", qid.QuoteId.ToString)
      'oBuilder.AppendFormat(Convert.ToChar(34) + "{0}" + Convert.ToChar(34) + " : " + Convert.ToChar(34) + "{1}" + Convert.ToChar(34) + ",", "author", qid.Author)
      'oBuilder.AppendFormat(Convert.ToChar(34) + "{0}" + Convert.ToChar(34) + " : " + Convert.ToChar(34) + "{1}" + Convert.ToChar(34) + ",", "role", qid.Role)
      'oBuilder.AppendFormat(Convert.ToChar(34) + "{0}" + Convert.ToChar(34) + " : " + Convert.ToChar(34) + "{1}" + Convert.ToChar(34), "body", qid.Body)
      'oBuilder.Append("}")

      'Return oBuilder.ToString

      Return Nothing
   End Function

   Public Function GetArticleTags(ByVal keys As String) As String
      Dim oBuilder As New StringBuilder
      If Not String.IsNullOrEmpty(keys) Then
         Dim articletagsvr As IArticleTagService = New ArticleTagService()
         Dim li As List(Of String) = articletagsvr.SuggestArticleTags(keys)

         If li IsNot Nothing Then
            For Each it As String In li
               oBuilder.AppendLine(it)
            Next
         End If

      End If
      Return oBuilder.ToString
   End Function

   Public Function GetImageTags(ByVal keys As String) As String
      Dim oBuilder As New StringBuilder
      If Not String.IsNullOrEmpty(keys) Then
         Dim _imagetagservice = New ImageTagService()
         Dim li As List(Of String) = _imagetagservice.SuggestImageTags(keys)

         If li IsNot Nothing Then
            For Each it As String In li
               oBuilder.AppendLine(it)
            Next
         End If

      End If
      Return oBuilder.ToString
   End Function

   Public Function GetImages(ByVal keys As List(Of String)) As List(Of Object)
      Dim li As New List(Of SimpleImage)

      Dim imageSrvr As New ImageService
      li = imageSrvr.GetImages(keys.ToArray)

      Dim baseUrl As String = routines.FullBaseUrl()
      Dim iIndex As Integer = 0

      Dim imageList As New List(Of Object)

      For Each it As SimpleImage In li
         Dim large_str As String = ""
         Dim mini_str As String = ""
         Dim long_str As String = ""
         Dim fi As New FileInfo(HttpContext.Current.Server.MapPath(it.ImageUrl))
         If fi IsNot Nothing Then
            If File.Exists(fi.Directory.FullName & "/" & "large_" & fi.Name.Replace(fi.Extension, ".jpg")) Then
               large_str = it.ImageUrl.Replace(fi.Name, "large_" & fi.Name.Replace(fi.Extension, ".jpg"))
            End If

            If File.Exists(fi.Directory.FullName & "/" & "mini_" & fi.Name.Replace(fi.Extension, ".jpg")) Then
               mini_str = it.ImageUrl.Replace(fi.Name, "mini_" & fi.Name.Replace(fi.Extension, ".jpg"))
            End If

            If File.Exists(fi.Directory.FullName & "/" & "long_" & fi.Name.Replace(fi.Extension, ".jpg")) Then
               long_str = it.ImageUrl.Replace(fi.Name, "long_" & fi.Name.Replace(fi.Extension, ".jpg"))
            End If
         End If

         Dim strUrl As String = Configs.Paths.CdnRoot & "/imagethumb.ashx?img=" & it.ImageUrl & "&w=150&h=80"

         imageList.Add(New With {.ImageId = it.ImageID, _
                                 .uuid = it.Uuid, _
                                 .ImageUrl = strUrl, _
                                 .large_url = large_str, _
                                 .long_url = long_str, _
                                 .mini_url = mini_str, _
                                 .CreditsName = it.CreditsName})
      Next

      Return imageList

      'Dim oBuilder As StringBuilder = New StringBuilder()
      'oBuilder.Append("{" + Convert.ToChar(34) + "images" + Convert.ToChar(34) + ": [")
      'For Each it As SimpleImage In li
      '   Dim large_str As String = ""
      '   Dim mini_str As String = ""
      '   Dim long_str As String = ""
      '   Dim fi As New FileInfo(HttpContext.Current.Server.MapPath(it.ImageUrl))
      '   If fi IsNot Nothing Then
      '      If File.Exists(fi.Directory.FullName & "/" & "large_" & fi.Name.Replace(fi.Extension, ".jpg")) Then
      '         large_str = it.ImageUrl.Replace(fi.Name, "large_" & fi.Name.Replace(fi.Extension, ".jpg"))
      '      End If

      '      If File.Exists(fi.Directory.FullName & "/" & "mini_" & fi.Name.Replace(fi.Extension, ".jpg")) Then
      '         mini_str = it.ImageUrl.Replace(fi.Name, "mini_" & fi.Name.Replace(fi.Extension, ".jpg"))
      '      End If

      '      If File.Exists(fi.Directory.FullName & "/" & "long_" & fi.Name.Replace(fi.Extension, ".jpg")) Then
      '         long_str = it.ImageUrl.Replace(fi.Name, "long_" & fi.Name.Replace(fi.Extension, ".jpg"))
      '      End If
      '   End If

      '   Dim strUrl As String = Configs.Paths.CdnRoot & "/imagethumb.ashx?img=" & it.ImageUrl & "&w=150&h=80"

      '   oBuilder.Append("{")
      '   oBuilder.AppendFormat(Convert.ToChar(34) + "{0}" + Convert.ToChar(34) + " : " + Convert.ToChar(34) + "{1}" + Convert.ToChar(34) + ",", "imageId", it.ImageID)
      '   oBuilder.AppendFormat(Convert.ToChar(34) + "{0}" + Convert.ToChar(34) + " : " + Convert.ToChar(34) + "{1}" + Convert.ToChar(34) + ",", "uuid", it.Uuid)
      '   oBuilder.AppendFormat(Convert.ToChar(34) + "{0}" + Convert.ToChar(34) + " : " + Convert.ToChar(34) + "{1}" + Convert.ToChar(34) + ",", "url", strUrl)
      '   oBuilder.AppendFormat(Convert.ToChar(34) + "{0}" + Convert.ToChar(34) + " : " + Convert.ToChar(34) + "{1}" + Convert.ToChar(34) + ",", "large_url", large_str)
      '   oBuilder.AppendFormat(Convert.ToChar(34) + "{0}" + Convert.ToChar(34) + " : " + Convert.ToChar(34) + "{1}" + Convert.ToChar(34) + ",", "mini_url", mini_str)
      '   oBuilder.AppendFormat(Convert.ToChar(34) + "{0}" + Convert.ToChar(34) + " : " + Convert.ToChar(34) + "{1}" + Convert.ToChar(34) + ",", "long_url", long_str)
      '   oBuilder.AppendFormat(Convert.ToChar(34) + "{0}" + Convert.ToChar(34) + " : " + Convert.ToChar(34) + "{1}" + Convert.ToChar(34), "creditsName", it.CreditsName)
      '   oBuilder.Append("}")

      '   If iIndex <> li.Count - 1 Then
      '      oBuilder.Append(",")
      '   End If
      '   iIndex += 1
      'Next

      'oBuilder.Append("]}")
      'Return oBuilder.ToString()


      'Return imageList
   End Function

   Public Function GettTipsDataJSON(Optional ByVal category As String = "") As String
      Dim _adviceService As New AdviceService()
      Dim _adviceCategoryService As New AdviceCategoryService()
      Dim li As List(Of Advice)
      Dim catId As Integer = -1
      If Not String.IsNullOrEmpty(category) Then
         Dim cat As AdviceCategory = _adviceCategoryService.GetCategoryBySlug(category)
         If cat IsNot Nothing Then
            catId = cat.CategoryID
         End If
      End If

      li = _adviceService.GetAdvices(True, catId, 0, 20)
      Dim iIndex As Integer = 0
      Dim oBuilder As StringBuilder = New StringBuilder()
      oBuilder.Append("{" + Convert.ToChar(34) + "tips" + Convert.ToChar(34) + ": [")
      For Each it As Advice In li
         oBuilder.Append("{")
         oBuilder.AppendFormat(Convert.ToChar(34) + "{0}" + Convert.ToChar(34) + " : " + Convert.ToChar(34) + "{1}" + Convert.ToChar(34) + ",", "TipId", it.AdviceID)
         oBuilder.AppendFormat(Convert.ToChar(34) + "{0}" + Convert.ToChar(34) + " : " + Convert.ToChar(34) + "{1}" + Convert.ToChar(34) + ",", "AddedDate", it.AddedDate)
         oBuilder.AppendFormat(Convert.ToChar(34) + "{0}" + Convert.ToChar(34) + " : " + Convert.ToChar(34) + "{1}" + Convert.ToChar(34) + ",", "AddedBy", it.AddedBy)
         oBuilder.AppendFormat(Convert.ToChar(34) + "{0}" + Convert.ToChar(34) + " : " + Convert.ToChar(34) + "{1}" + Convert.ToChar(34) + ",", "Abstract", it.Abstract)
         oBuilder.AppendFormat(Convert.ToChar(34) + "{0}" + Convert.ToChar(34) + " : " + Convert.ToChar(34) + "{1}" + Convert.ToChar(34), "Title", it.Title)
         oBuilder.Append("}")

         If iIndex <> li.Count - 1 Then
            oBuilder.Append(",")
         End If
         iIndex += 1
      Next

      oBuilder.Append("]}")
      Return oBuilder.ToString()
   End Function

   'Public Function GetVideosImagesJSON(ByVal keys As List(Of String)) As String
   '   Dim baseUrl As String = FullBaseUrl()
   '   Dim _videoservice As IVideoService = New VideoService()
   '   Dim li As List(Of Video) = _videoservice.GetVideosByTags(keys.ToArray)
   '   Dim iIndex As Integer = 0
   '   Dim oBuilder As StringBuilder = New StringBuilder()
   '   oBuilder.Append("{" + Convert.ToChar(34) + "videos" + Convert.ToChar(34) + ": [")
   '   For Each it As Video In li
   '      oBuilder.Append("{")
   '      oBuilder.AppendFormat(Convert.ToChar(34) + "{0}" + Convert.ToChar(34) + " : " + Convert.ToChar(34) + "{1}" + Convert.ToChar(34) + ",", "videoId", it.VideoID)
   '      oBuilder.AppendFormat(Convert.ToChar(34) + "{0}" + Convert.ToChar(34) + " : " + Convert.ToChar(34) + "{1}" + Convert.ToChar(34) + ",", "name", it.Name)
   '      oBuilder.AppendFormat(Convert.ToChar(34) + "{0}" + Convert.ToChar(34) + " : " + Convert.ToChar(34) + "{1}" + Convert.ToChar(34) + ",", "title", it.Title)
   '      oBuilder.AppendFormat(Convert.ToChar(34) + "{0}" + Convert.ToChar(34) + " : " + Convert.ToChar(34) + "{1}" + Convert.ToChar(34) + ",", "url", it.VideoUrl)
   '      oBuilder.AppendFormat(Convert.ToChar(34) + "{0}" + Convert.ToChar(34) + " : " + Convert.ToChar(34) + "{1}" + Convert.ToChar(34), "imageurl", baseUrl & "uploads/videos/" & it.Name & "/default.jpg")
   '      oBuilder.Append("}")

   '      If iIndex <> li.Count - 1 Then
   '         oBuilder.Append(",")
   '      End If
   '      iIndex += 1
   '   Next

   '   oBuilder.Append("]}")
   '   Return oBuilder.ToString()
   'End Function



   Public Function PollVote(ByVal optionId As Integer) As String

      'Dim ret As String = ""

      'If HttpContext.Current.Request.Cookies("mcc_adv_votelist") IsNot Nothing Then
      '   Dim str As HttpCookie = HttpContext.Current.Request.Cookies("mcc_adv_votelist")
      '   Dim cv As String = HttpContext.Current.Server.HtmlDecode(str.Value)
      '   Dim idlist() As String = cv.Split(";")
      '   For Each it As String In idlist
      '      If it = id.ToString Then
      '         Return False
      '      End If
      '   Next
      'End If

      Dim _pollOpService As New PollOptionService
      Dim pid As Integer = -1
      pid = _pollOpService.VotePollOptions(optionId)

      'Dim cookie As HttpCookie = HttpContext.Current.Request.Cookies.Get("mcc_poll_votelist")
      'If cookie IsNot Nothing Then
      '   Dim str() As String = cookie.Value.Split(";")
      'Else
      '   cookie = New HttpCookie("mcc_poll_votelist")
      '   cookie.Value = pid
      'End If

      Dim tv As Integer = _pollOpService.GetTotalVotesByPollOptionId(optionId)

      Return "success" & "|" & pid.ToString & "|" & tv.ToString
   End Function
End Class