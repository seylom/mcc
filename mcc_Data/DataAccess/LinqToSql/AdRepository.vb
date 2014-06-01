Public Class AdRepository
   Implements IAdRepository


   Private _mdc As MCCDataContext
   Public Sub New()
      Me.New(new MCCDataContext)
   End Sub

   Public Sub New(ByVal db As MCCDataContext)
      _mdc = db
   End Sub

   Public Function GetAds() As IQueryable(Of Ad) Implements IAdRepository.GetAds
      Dim q = From it As mcc_Ad In _mdc.mcc_Ads _
              Select New Ad With { _
                                    .AdID = it.AdId, _
                                    .AddedDate = it.AddedDate, _
                                    .AddedBy = it.Addedby, _
                                    .Approved = it.Approved, _
                                    .Body = it.Body, _
                                    .Description = it.Description, _
                                    .keywords = it.Keywords, _
                                    .Title = it.Title, _
                                    .Type = it.Type, _
                                    .ZoneId = it.ZoneId, _
                                    .Task = it.Task, _
                                    .AdvertizerID = it.AdvertizerId}
      Return q
   End Function
   Function GetArticleAds(ByVal articleId As Integer) As IQueryable(Of Ad) Implements IAdRepository.GetArticleAds
      Dim p = _mdc.mcc_ArticleAds.Where(Function(x) x.ArticleID = articleId).Select(Function(x) x.AdID).ToList
      Dim q = From it As mcc_Ad In _mdc.mcc_Ads Where p.Contains(it.AdId) _
              Select New Ad With { _
                                    .AdID = it.AdId, _
                                    .AddedDate = it.AddedDate, _
                                    .AddedBy = it.Addedby, _
                                    .Approved = it.Approved, _
                                    .Body = it.Body, _
                                    .Description = it.Description, _
                                    .keywords = it.Keywords, _
                                    .Title = it.Title, _
                                    .Type = it.Type, _
                                    .ZoneId = it.ZoneId, _
                                    .Task = it.Task, _
                                    .AdvertizerID = it.AdvertizerId}
      Return q

   End Function
   Public Function InsertAd(ByVal _ad As Ad) As Integer Implements IAdRepository.InsertAd
      If _ad IsNot Nothing Then
         Dim nad As New mcc_Ad
         With nad
            .AddedDate = _ad.AddedDate
            .Addedby = _ad.AddedBy
            .Approved = _ad.Approved
            .Body = _ad.Body
            .Description = _ad.Description
            .Keywords = _ad.keywords
            .Title = _ad.Title
            .Type = _ad.Type
            .ZoneId = _ad.ZoneId
            .Task = _ad.Task
            .AdvertizerId = _ad.AdvertizerID
         End With

         _mdc.mcc_Ads.InsertOnSubmit(nad)
         _mdc.SubmitChanges()
      End If
   End Function

   Public Sub DeleteAd(ByVal _adId As Integer) Implements IAdRepository.DeleteAd
      If _adId > 0 Then
         Dim q = _mdc.mcc_Ads.Where(Function(p) p.AdId = _adId).FirstOrDefault()
         If q IsNot Nothing Then
            _mdc.mcc_Ads.DeleteOnSubmit(q)
            _mdc.SubmitChanges()

            ' remove mapping on 
            Dim v = _mdc.mcc_ArticleAds.Where(Function(p) p.AdID = _adId).FirstOrDefault()
            If v IsNot Nothing Then
               _mdc.mcc_ArticleAds.DeleteOnSubmit(v)
            End If
         End If      
      End If
   End Sub
   Public Sub DeleteAds(ByVal _adIds As Integer()) Implements IAdRepository.DeleteAds
      If _adIds IsNot Nothing Then
         Dim q = _mdc.mcc_Ads.Where(Function(p) _adIds.Contains(p.AdId))
         If q IsNot Nothing Then
            _mdc.mcc_Ads.DeleteAllOnSubmit(q)
            _mdc.SubmitChanges()

            ' mapping on articles
            Dim v = _mdc.mcc_ArticleAds.Where(Function(p) _adIds.Contains(p.AdID)).FirstOrDefault()
            If v IsNot Nothing Then
               _mdc.mcc_ArticleAds.DeleteOnSubmit(v)
            End If
         End If
      End If
   End Sub
   Public Sub UpdateAd(ByVal _ad As Ad) Implements IAdRepository.UpdateAd
      If _ad IsNot Nothing Then
         Dim q = _mdc.mcc_Ads.Where(Function(p) p.AdId = _ad.AdID).FirstOrDefault()
         If q IsNot Nothing Then
            With q
               .Approved = _ad.Approved
               .Body = _ad.Body
               .Description = _ad.Description
               .Keywords = _ad.keywords
               .Title = _ad.Title
               .Type = _ad.Type
               .ZoneId = _ad.ZoneId
               .Task = _ad.Task
               .AdvertizerId = _ad.AdvertizerID
            End With

            _mdc.SubmitChanges()
         End If
      End If
   End Sub

   Public Sub DeleteArticleAds(ByVal articleId As Integer) Implements IAdRepository.DeleteArticleAds
      If articleId > 0 Then
         Dim q = _mdc.mcc_ArticleAds.Where(Function(p) p.ArticleID = articleId).FirstOrDefault()
         If q IsNot Nothing Then
            _mdc.mcc_ArticleAds.DeleteOnSubmit(q)
         End If
      End If
   End Sub

   Public Sub DeleteArticleAd(ByVal articleId As Integer, ByVal AdId As Integer) Implements IAdRepository.DeleteArticleAd
      If articleId > 0 Then
         Dim q = _mdc.mcc_ArticleAds.Where(Function(p) p.ArticleID = articleId And p.AdID = AdId).FirstOrDefault()
         If q IsNot Nothing Then
            _mdc.mcc_ArticleAds.DeleteOnSubmit(q)
         End If
      End If
   End Sub

    Public Function InsertArticleAd(ByVal articleId As Integer, ByVal AdId As Integer) As Boolean Implements IAdRepository.InsertArticleAd
        If articleId > 0 AndAlso AdId > 0 Then
            Dim nad As New mcc_ArticleAd
            nad.ArticleID = articleId
            nad.AdID = AdId

            _mdc.mcc_ArticleAds.InsertOnSubmit(nad)
            _mdc.SubmitChanges()
            Return True
        End If
        Return False
    End Function

    Public Function InsertArticleAds(ByVal articleId As Integer, ByVal AdIds() As Integer) As Boolean Implements IAdRepository.InsertArticleAds
        If articleId > 0 AndAlso AdIds IsNot Nothing Then
            For Each it As Integer In AdIds
                Dim idx As Integer = it
                If _mdc.mcc_Ads.Count(Function(p) p.AdId = idx) = 0 Then
                    Dim nad As New mcc_ArticleAd
                    nad.ArticleID = articleId
                    nad.AdID = idx
                    _mdc.mcc_ArticleAds.InsertOnSubmit(nad)
                End If
            Next
            _mdc.SubmitChanges()
            Return True
        End If
        Return False
    End Function
End Class
