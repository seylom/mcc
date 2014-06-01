Imports Microsoft.VisualBasic
Imports System.Linq
Imports System.IO
Imports MCC.Data

Namespace Ads
   Public Class AdRepository
      Inherits mccObject


      Public Shared Function GetAdsCount() As Integer
         Dim key As String = "ads_adscount"
         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            Using mdc As New MCCDataContext
               Dim it As Integer = mdc.mcc_Ads.Count()
               CacheData(key, it)
               Return it
            End Using
         End If
      End Function


      Protected Shared Sub CacheData(ByVal key As String, ByVal data As Object)
         If data IsNot Nothing Then
            mccObject.Cache.Insert(key, data, Nothing, DateTime.Now.AddSeconds(3600), TimeSpan.Zero)
         End If
      End Sub

      Public Shared Function GetAds() As List(Of mcc_Ad)
         Dim key As String = "ads_ads_"
         If Cache(key) IsNot Nothing Then
            Return DirectCast(Cache(key), List(Of mcc_Ad))
         Else
            Using mdc As New MCCDataContext
               Dim it As List(Of mcc_Ad) = mdc.mcc_Ads.ToList
               CacheData(key, it)
               Return it
            End Using
         End If
      End Function

      Public Shared Function GetAds(ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "title ASC") As List(Of mcc_Ad)
         Dim key As String = "ads_ads_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Return DirectCast(Cache(key), List(Of mcc_Ad))
         Else
            Using mdc As New MCCDataContext
               Dim it As New List(Of mcc_Ad)
               If mdc.mcc_Ads.Count() > 0 Then
                  If startrowindex > 0 AndAlso maximumrows > 0 Then
                     it = mdc.mcc_Ads.Skip(startrowindex).Take(maximumrows).ToList
                  Else
                     it = mdc.mcc_Ads.Skip(0).Take(30).ToList
                  End If
               End If
               CacheData(key, it)
               Return it
            End Using
         End If
      End Function


      Public Shared Function GetAdById(ByVal adId As Integer) As mcc_Ad
         Dim key As String = "ads_ads_" & adId.ToString & "_"

         If Cache(key) IsNot Nothing Then
            Return DirectCast(Cache(key), mcc_Ad)
         Else
            Using mdc As New MCCDataContext
               If mdc.mcc_Ads.Count(Function(p) p.AdId = adId) > 0 Then
                  Dim it As mcc_Ad = (From i As mcc_Ad In mdc.mcc_Ads Where i.AdId = adId).Single()
                  CacheData(key, it)
                  Return it
               Else
                  Return Nothing
               End If
            End Using
         End If
      End Function



      Public Shared Sub InsertAd(ByVal title As String, ByVal body As String, ByVal description As String, ByVal keywords As String, ByVal type As Integer)
         Using mdc As New MCCDataContext


            description = ConvertNullToEmptyString(description)
            keywords = ConvertNullToEmptyString(keywords)

            Dim ad As New mcc_Ad
            ad.AddedDate = DateTime.Now
            ad.Addedby = mccObject.CurrentUserName
            ad.Approved = False
            ad.Description = description
            ad.Keywords = keywords
            ad.Title = title
            ad.Body = body
            ad.Type = type

            mdc.mcc_Ads.InsertOnSubmit(ad)
            mdc.SubmitChanges()


            PurgeCacheItems("ads_")
         End Using
      End Sub


      Public Shared Sub UpdateAd(ByVal adId As Integer, ByVal title As String, ByVal body As String, ByVal description As String, ByVal keywords As String, ByVal type As Integer)
         Dim mdc As MCCDataContext = New MCCDataContext
         If mdc.mcc_Ads.Count(Function(p) p.AdId = adId) > 0 Then
            Dim wrd As mcc_Ad = (From t In mdc.mcc_Ads _
                                        Where t.AdId = adId _
                                        Select t).Single()

            description = ConvertNullToEmptyString(description)
            keywords = ConvertNullToEmptyString(keywords)
            If wrd IsNot Nothing Then
               wrd.Title = title
               wrd.Body = body
               wrd.Description = description
               wrd.Keywords = keywords
               wrd.Type = type

               mdc.SubmitChanges()
               mccObject.PurgeCacheItems("ads_ads_")
            End If
         End If
      End Sub


      Public Shared Sub DeleteAd(ByVal adId As Integer)
         Using mdc As New MCCDataContext
            If mdc.mcc_Ads.Count(Function(p) p.AdId = adId) > 0 Then
               Dim tg As mcc_Ad = (From it As mcc_Ad In mdc.mcc_Ads Where it.AdId = adId).Single()

               mdc.mcc_Ads.DeleteOnSubmit(tg)
               mdc.SubmitChanges()
               PurgeCacheItems("ads_adscount")
               PurgeCacheItems("ads_ads_")

            End If
         End Using
      End Sub

      Public Shared Sub DeleteAds(ByVal Ids() As Integer)
         Using mdc As New MCCDataContext
            If mdc.mcc_Ads.Count(Function(p) Ids.Contains(p.AdId)) > 0 Then

               Dim tg As List(Of mcc_Ad) = (From it As mcc_Ad In mdc.mcc_Ads Where Ids.Contains(it.AdId)).ToList


               mdc.mcc_Ads.DeleteAllOnSubmit(tg)
               mdc.SubmitChanges()
               PurgeCacheItems("ads_adscount")
               PurgeCacheItems("ads_ads_")
            End If
         End Using
      End Sub

      Public Shared Sub PurgeAdCache()
         PurgeCacheItems("ads_")
      End Sub

      Public Shared Function GetAdsCountByArticle(ByVal articleId As Integer) As Integer
         Dim key As String = "articleads_articleadscount_" & articleId.ToString
         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            Using mdc As New MCCDataContext
               Dim it As Integer = mdc.mcc_ArticleAds.Count(Function(p) p.ArticleID = articleId)
               CacheData(key, it)
               Return it
            End Using
         End If
      End Function

      Public Shared Function GetAdsByArticle(ByVal articleId As Integer) As List(Of mcc_Ad)
         Dim key As String = "ads_articleads_" & articleId.ToString
         If Cache(key) IsNot Nothing Then
            Return DirectCast(Cache(key), List(Of mcc_Ad))
         Else
            Using mdc As New MCCDataContext
               Dim ad_list As List(Of Integer) = (From i As mcc_ArticleAd In mdc.mcc_ArticleAds Where i.ArticleID = articleId Select i.AdID).ToList
               Dim it As List(Of mcc_Ad) = (From ad As mcc_Ad In mdc.mcc_Ads Where ad_list.Contains(ad.AdId)).ToList
               CacheData(key, it)
               Return it
            End Using
         End If
      End Function

      Public Shared Function GetAdsIdsByArticle(ByVal articleId As Integer) As List(Of Integer)
         Dim key As String = "ads_articleadsid_" & articleId.ToString
         If Cache(key) IsNot Nothing Then
            Return DirectCast(Cache(key), List(Of Integer))
         Else
            Using mdc As New MCCDataContext
               Dim ad_list As List(Of Integer) = (From i As mcc_ArticleAd In mdc.mcc_ArticleAds Where i.ArticleID = articleId Select i.AdID).ToList
               CacheData(key, ad_list)
               Return ad_list
            End Using
         End If
      End Function


      Public Shared Function DeleteArticleAds(ByVal articleId As Integer) As Boolean
         Using mdc As New MCCDataContext
            Dim adlist As List(Of mcc_ArticleAd) = (From it As mcc_ArticleAd In mdc.mcc_ArticleAds Where it.ArticleID = articleId).ToList
            mdc.mcc_ArticleAds.DeleteAllOnSubmit(adlist)
            mdc.SubmitChanges()
            PurgeCacheItems("ads_articleadsid_" & articleId.ToString)
            PurgeCacheItems("ads_articleads_" & articleId.ToString)
         End Using
      End Function

      Public Shared Function DeleteArticleAd(ByVal articleId As Integer, ByVal adId As Integer) As Boolean
         Using mdc As New MCCDataContext
            Dim adlist As List(Of mcc_ArticleAd) = (From it As mcc_ArticleAd In mdc.mcc_ArticleAds Where it.ArticleID = articleId AndAlso it.AdID = adId).ToList
            mdc.mcc_ArticleAds.DeleteAllOnSubmit(adlist)
            mdc.SubmitChanges()
            PurgeCacheItems("ads_articleadsid_" & articleId.ToString)
            PurgeCacheItems("ads_articleads_" & articleId.ToString)
         End Using
      End Function

      Public Shared Function InsertArticleAd(ByVal articleId As Integer, ByVal adId As Integer) As Boolean
         Using mdc As New MCCDataContext
            If mdc.mcc_ArticleAds.Count(Function(p) p.ArticleID = articleId AndAlso p.AdID = adId) = 0 Then
               Dim ad As New mcc_ArticleAd
               ad.ArticleID = articleId
               ad.AdID = adId

               mdc.mcc_ArticleAds.InsertOnSubmit(ad)
               mdc.SubmitChanges()

               PurgeCacheItems("ads_articleadsid_" & articleId.ToString)
               PurgeCacheItems("ads_articleads_" & articleId.ToString)
            End If
         End Using
         Return True
      End Function

      Public Shared Function GetRandomAdByType(ByVal type As Integer) As mcc_Ad
         Dim ad As mcc_Ad
         Using mdc As New MCCDataContext
            ad = (From it As mcc_Ad In mdc.mcc_Ads Where it.Type = type).FirstOrDefault()
         End Using
         Return ad
      End Function
   End Class
End Namespace
