Imports MCC.Data
Imports Webdiyer.WebControls.Mvc

Public Enum adType As Integer
   Undefined = 0
   BannerVertical = 1
   BannerHorizontal = 2
   TextOnly = 3
   ImageVertical = 4
   ImageHorizontal = 5
   WidgetVertical = 6
   WidgetHorizontal = 7
   GoogleAds = 8
End Enum


Public Enum ZoneType As Integer
    header = 0
End Enum

Public Class AdService
   Inherits CacheObject
   Implements IAdService

   Private _adRepo As IAdRepository

   Public Sub New()
      Me.New(New AdRepository())
   End Sub

   Public Sub New(ByVal adRepo As IAdRepository)
      _adRepo = adRepo
   End Sub

   Public Function GetAdsCount() As Integer Implements IAdService.GetAdsCount
      Dim key As String = "ads_adscount"
      If Cache(key) IsNot Nothing Then
         Return CInt(Cache(key))
      Else
         Dim it As Integer = _adRepo.GetAds.Count
         CacheData(key, it)
         Return it

      End If
   End Function


   Protected Sub CacheData(ByVal key As String, ByVal data As Object)
      If data IsNot Nothing Then
         CacheObject.Cache.Insert(key, data, Nothing, DateTime.Now.AddSeconds(3600), TimeSpan.Zero)
      End If
   End Sub

   Public Function GetAds() As List(Of Ad) Implements IAdService.GetAds
      Dim key As String = "ads_ads_"
      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), List(Of Ad))
      Else

         Dim it As List(Of Ad) = _adRepo.GetAds.ToList
         CacheData(key, it)
         Return it

      End If
   End Function

   Public Function GetAds(ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "title") As PagedList(Of Ad) Implements IAdService.GetAds
      Dim key As String = "ads_ads_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), PagedList(Of Ad))
      Else
         Dim it As PagedList(Of Ad) = _adRepo.GetAds.SortBy(sortExp).ToPagedList(startrowindex, maximumrows)
         CacheData(key, it)
         Return it
      End If
   End Function


   Public Function GetAdById(ByVal adId As Integer) As Ad Implements IAdService.GetAdById
      Dim key As String = "ads_ads_" & adId.ToString & "_"

      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), Ad)
      Else
         Dim it As Ad = _adRepo.GetAds.WithID(adId).FirstOrDefault()
         CacheData(key, it)
         Return it
      End If
   End Function



   Public Sub InsertAd(ByVal _ad As Ad) Implements IAdService.InsertAd

      _ad.Description = ConvertNullToEmptyString(_ad.Description)
      _ad.keywords = ConvertNullToEmptyString(_ad.keywords)
      _ad.AddedDate = DateTime.Now
      _ad.AddedBy = CacheObject.CurrentUserName
      _ad.Approved = False

      _adRepo.InsertAd(_ad)
      PurgeCacheItems("ads_")

   End Sub


   Public Sub UpdateAd(ByVal _ad As Ad) Implements IAdService.UpdateAd
      If _ad IsNot Nothing Then
         _adRepo.UpdateAd(_ad)
         CacheObject.PurgeCacheItems("ads_ads_")
      End If
   End Sub


   Public Sub DeleteAd(ByVal adId As Integer) Implements IAdService.DeleteAd
      If adId > 0 Then
         _adRepo.DeleteAd(adId)
         PurgeCacheItems("ads_adscount")
         PurgeCacheItems("ads_ads_")
      End If
   End Sub

   Public Sub DeleteAds(ByVal Ids() As Integer) Implements IAdService.DeleteAds
      If Ids IsNot Nothing Then
         _adRepo.DeleteAds(Ids)
         PurgeCacheItems("ads_adscount")
         PurgeCacheItems("ads_ads_")
      End If
   End Sub

   Public Sub PurgeAdCache() Implements IAdService.PurgeAdCache
      PurgeCacheItems("ads_")
   End Sub

   Public Function GetAdsCountByArticle(ByVal articleId As Integer) As Integer Implements IAdService.GetAdsCountByArticle
      Dim key As String = "articleads_articleadscount_" & articleId.ToString
      If Cache(key) IsNot Nothing Then
         Return CInt(Cache(key))
      Else
         Dim it As Integer = _adRepo.GetArticleAds(articleId).Count
         CacheData(key, it)
         Return it
      End If
   End Function

   Public Function GetAdsByArticle(ByVal articleId As Integer) As List(Of Ad) Implements IAdService.GetAdsByArticle
      Dim key As String = "ads_articleads_" & articleId.ToString
      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), List(Of Ad))
      Else
         Dim it As List(Of Ad) = _adRepo.GetArticleAds(articleId).ToList
         CacheData(key, it)
         Return it
      End If
   End Function

   Public Function GetAdsIdsByArticle(ByVal articleId As Integer) As List(Of Integer) Implements IAdService.GetAdsIdsByArticle
      Dim key As String = "ads_articleadsid_" & articleId.ToString
      If Cache(key) IsNot Nothing Then
         Return DirectCast(Cache(key), List(Of Integer))
      Else
            Dim ad_list As List(Of Integer) = _adRepo.GetArticleAds(articleId).Select(Function(p) p.AdID).ToList
         CacheData(key, ad_list)
         Return ad_list
      End If
   End Function


   Public Function DeleteArticleAds(ByVal articleId As Integer) As Boolean Implements IAdService.DeleteArticleAds
      _adRepo.DeleteArticleAds(articleId)
      PurgeCacheItems("ads_articleadsid_" & articleId.ToString)
      PurgeCacheItems("ads_articleads_" & articleId.ToString)

   End Function

   Public Function DeleteArticleAd(ByVal articleId As Integer, ByVal adId As Integer) As Boolean Implements IAdService.DeleteArticleAd
      _adRepo.DeleteArticleAd(articleId, adId)
      PurgeCacheItems("ads_articleadsid_" & articleId.ToString)
      PurgeCacheItems("ads_articleads_" & articleId.ToString)
   End Function

   Public Function InsertArticleAd(ByVal articleId As Integer, ByVal adId As Integer) As Boolean Implements IAdService.InsertArticleAd

      If articleId > 0 AndAlso adId > 0 Then
         _adRepo.InsertArticleAd(articleId, adId)

         PurgeCacheItems("ads_articleadsid_" & articleId.ToString)
         PurgeCacheItems("ads_articleads_" & articleId.ToString)
      End If

      Return True
   End Function

   Public Function GetRandomAdByType(ByVal type As adType) As Ad Implements IAdService.GetRandomAdByType
      Dim ad As Ad
      ad = _adRepo.GetAds.Where(Function(p) p.Type = type).FirstOrDefault()
      Return ad
   End Function
End Class