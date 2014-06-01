Public Interface IAdRepository
   Function GetAds() As IQueryable(Of Ad)
  
   Function InsertAd(ByVal _ad As Ad) As Integer
   Sub DeleteAd(ByVal _adId As Integer)
   Sub DeleteAds(ByVal _adId() As Integer)
   Sub UpdateAd(ByVal _ad As Ad)

   Function GetArticleAds(ByVal articleId As Integer) As IQueryable(Of Ad)
   Sub DeleteArticleAds(ByVal articleId As Integer)
   Sub DeleteArticleAd(ByVal articleId As Integer, ByVal AdId As Integer)
    Function InsertArticleAd(ByVal articleId As Integer, ByVal AdId As Integer) As Boolean
    Function InsertArticleAds(ByVal articleId As Integer, ByVal AdIds() As Integer) As Boolean
End Interface