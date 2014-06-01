<%@ WebHandler Language="VB" Class="AjaxData" %>

Imports System
Imports System.Web
Imports System.Xml
Imports MCC
Imports System.Linq

Public Class AjaxData
   Implements IHttpHandler
    
   Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
      context.Response.ContentType = "text/plain"
      context.Response.Cache.SetCacheability(HttpCacheability.Public)
      context.Response.Cache.SetLastModified(DateTime.Now)
      context.Response.Cache.SetExpires(DateTime.Now.AddMinutes(2))
        
      Try
         If Not String.IsNullOrEmpty(context.Request.QueryString("qId")) Then
            Dim qsval As String = context.Request.QueryString("qId")
            Select Case qsval.ToLower
               Case "qot"
                  context.Response.Write(New AjaxDataFile().GetQuoteDataJSON())
               Case "req"
                  'Dim url As String = context.Request.QueryString("Url")
                  'If Not String.IsNullOrEmpty(url) Then
                  '   Dim xd As XmlDocument = New XmlDocument
                  '   xd.Load(url)
                  '   Dim strRes As String = XmlJSON.XmlToJSON(xd)
                  '   context.Response.ContentType = "text/json"
                  '   context.Response.Write(strRes)
                  'Else
                  '   context.Response.Write("Unable to load the specified Feeds")
                  'End If
               Case "feed"
                  Dim url As String = context.Request.QueryString("Url")
                  If Not String.IsNullOrEmpty(url) Then
                     Dim xd As XmlDocument = New XmlDocument
                     xd.Load(url)
                     context.Response.ContentType = "text/xml"
                     context.Response.Write(xd.InnerXml)
                  Else
                     context.Response.Write("Unable to load the specified Feeds")
                  End If
               Case "rposts"
                  context.Response.Write(New AjaxDataFile().FetchRecentPosts())
                  'Case "har"
                  '   context.Response.Write(New AjaxDataFile().FetchHomeArticle())
               Case "catlist"
                  context.Response.Write(New AjaxDataFile().GetCategoriesJSON())
               Case "vote"
                  Dim adviceId = context.Request.QueryString("id")
                  Dim val = context.Request.QueryString("val")
                  If Not String.IsNullOrEmpty(adviceId) Then
                     Dim i As Integer = Integer.Parse(adviceId)
                     Dim agr As Integer = Integer.Parse(val)
                     If i <> -1 Then
                        Dim agree As Boolean = IIf(agr = 0, False, True)
                        Dim ret As String = New AjaxDataFile().AdviceVote(i, agree)
                              
                        context.Response.Write(ret)

                     End If
                  Else
                     context.Response.Write("failure")
                  End If
               Case "votevals"
                  Dim adviceId = context.Request.QueryString("id")
                  If Not String.IsNullOrEmpty(adviceId) Then
                     Dim i As Integer = Integer.Parse(adviceId)
                     If i <> -1 Then
                        Dim str As String = Advices.AdviceRepository.AllVotes(i)
                        context.Response.Write(str)
                     End If
                  Else
                     context.Response.Write("failure")
                  End If
               Case "itp"
                  'Dim id = context.Request.QueryString("p")
                  'If Not String.IsNullOrEmpty(id.Trim) Then

                  '   Dim it As Integer = Integer.Parse(id)
                  '   context.Response.Write(Articles.ArticleRepository.GetArticleById(it).Body)
                  'End If
               Case "qid"
                  context.Response.ContentType = "text/json"
                  context.Response.Write(New AjaxDataFile().GetQuoteJSON())
               Case "tags"
                  Dim str As String = context.Request.QueryString("q")
                  If Not String.IsNullOrEmpty(str.Trim) Then
                     context.Response.ContentType = "text/json"
                     context.Response.Write(New AjaxDataFile().GetArticleTags(str.Trim))
                  End If
               Case "imtag"
                  Dim str As String = context.Request.QueryString("q")
                  If Not String.IsNullOrEmpty(str.Trim) Then
                     context.Response.ContentType = "text/json"
                     context.Response.Write(New AjaxDataFile().GetImageTags(str.Trim))
                  End If
               Case "fimg"
                  Dim str As String = context.Request.QueryString("tag")
                  If Not String.IsNullOrEmpty(str) Then
                     Dim keys As List(Of String) = str.Split(",").ToList()
                     context.Response.ContentType = "text/json"
                     context.Response.Write(New AjaxDataFile().GetImages(keys))
                  End If
               Case "fvd"
                  Dim str As String = context.Request.QueryString("tag")
                  If Not String.IsNullOrEmpty(str) Then
                     Dim keys As List(Of String) = str.Split(",").ToList()
                     context.Response.ContentType = "text/json"
                     context.Response.Write(New AjaxDataFile().GetVideosImagesJSON(keys))
                  End If
               Case "tips"
                  context.Response.ContentType = "text/json"
                  Dim q As String = context.Request.QueryString("q")
                  If Not String.IsNullOrEmpty(q.Trim) Then
                     context.Response.Write(New AjaxDataFile().GettTipsDataJSON(q))
                  Else
                     context.Response.Write(New AjaxDataFile().GettTipsDataJSON())
                  End If
               Case "polls"
                  context.Response.ContentType = "text/json"
                  Dim id As String = context.Request.QueryString("id")
                  If Not String.IsNullOrEmpty(id.Trim) Then
                     Dim it As Integer = -1
                     If Integer.TryParse(id, it) Then
                        context.Response.Write(New AjaxDataFile().PollVote(it))
                     End If
                  End If
                        
               Case "ucheck"
                  context.Response.ContentType = "text/json"
                  Dim q As String = context.Request.QueryString("q")
                                                          
                  If q.Length > 4 Then
                     If Not String.IsNullOrEmpty(q.Trim) Then
                        Dim b As Boolean = MCC.UserAccountRoutines.UserExistsInMembership(q.Trim)
                        Dim res As String = IIf(b, "Username already taken!", "Username available!")
                        context.Response.Write(res)
                     Else
                        context.Response.Write("")
                     End If
                  Else
                     context.Response.Write("Username is too short.")
                  End If
                       
                        
            End Select
         Else
            context.Response.Write("No data to be retrieved")
         End If
      Catch ex As Exception

      End Try
      
   End Sub
 
   Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
      Get
         Return False
      End Get
   End Property
    
    

End Class

