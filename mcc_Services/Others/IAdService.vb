Imports MCC.Data
Imports Webdiyer.WebControls.Mvc

Public Interface IAdService
   Function GetAdsCount() As Integer
   Function GetAds() As List(Of Ad)
   Function GetAds(ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "title") As PagedList(Of Ad)
   Function GetAdById(ByVal adId As Integer) As Ad
   Sub InsertAd(ByVal _ad As Ad)
   Sub UpdateAd(ByVal _ad As Ad)
   Sub DeleteAd(ByVal adId As Integer)
   Sub DeleteAds(ByVal Ids As Integer())
   Sub PurgeAdCache()
   Function GetAdsCountByArticle(ByVal articleId As Integer) As Integer
   Function GetAdsByArticle(ByVal articleId As Integer) As List(Of Ad)
   Function GetAdsIdsByArticle(ByVal articleId As Integer) As List(Of Integer)
   Function DeleteArticleAds(ByVal articleId As Integer) As Boolean
   Function DeleteArticleAd(ByVal articleId As Integer, ByVal adId As Integer) As Boolean
   Function InsertArticleAd(ByVal articleId As Integer, ByVal adId As Integer) As Boolean
   Function GetRandomAdByType(ByVal type As adType) As Ad
End Interface
