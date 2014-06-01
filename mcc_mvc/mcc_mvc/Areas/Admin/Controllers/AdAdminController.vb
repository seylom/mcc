Imports MCC.Services
Imports MCC.Data

Public Class AdAdminController
   Inherits AdminController

   Private _adservice As IAdService

   Public Sub New()
      Me.New(New AdService)
   End Sub

   Public Sub New(ByVal adservr As IAdService)
      _adservice = adservr
   End Sub

   Function Index() As ActionResult

      Dim _viewData As New AdsViewModel()
      _viewData.Ads = _adservice.GetAds(0, 10)

      Return View(_viewData)
   End Function

   Function CreateAd()
      Dim _viewdata As New Ad()
      Return View(_viewdata)
   End Function

   <AcceptVerbs(HttpVerbs.Post), ValidateInput(False)> _
   Function CreateAd(ByVal vd As Ad)

      If Not ValidateAd(vd.Title, vd.Body, vd.Description) Then
         Return View(vd)
      End If

      If ModelState.IsValid Then
         Dim _adsrv As New AdService()
         Dim _ad As New Ad With {.Title = vd.Title, _
                                 .Body = vd.Body, _
                                 .Description = vd.Description, _
                                 .Approved = False, _
                                 .ZoneId = 0, _
                                 .Type = vd.Type, _
                                 .keywords = vd.keywords, _
                                 .AdvertizerID = 0}

         _adsrv.InsertAd(_ad)

         Return RedirectToAction("Index")
      End If

      Return View(vd)

   End Function

   Function EditAd(ByVal id As Integer)

      If id <= 0 Then
         Return RedirectToAction("Index")
      End If

      Dim adSrvr As New AdService()
      Dim _viewdata As Ad = adSrvr.GetAdById(id)

      If _viewdata Is Nothing Then
         Return RedirectToAction("Index")
      End If

      Return View("CreateAd", _viewdata)
   End Function

   <AcceptVerbs(HttpVerbs.Post), ValidateInput(False)> _
   Function EditAd(ByVal Id As Integer, ByVal Title As String, ByVal body As String, ByVal description As String, _
                                 ByVal keywords As String, ByVal TypeId As adType)

      If Id <= 0 Then
         Return RedirectToAction("Index")
      End If

      If Not ValidateAd(Title, body, description) Then
         TempData("ErrorMessage") = "Please complete all required fields!"
         Return View()
      End If

      Dim _adService As New AdService()
      Dim ud As Ad = _adService.GetAdById(Id)

      If ud Is Nothing Then
         Return RedirectToAction("Index")
      End If

      With ud
         .Title = Title
         .Body = body
         .Description = description
         .keywords = keywords
         .Type = TypeId
      End With

      _adService.UpdateAd(ud)

      Return RedirectToAction("Index")
   End Function


   Function ValidateAd(ByVal title As String, ByVal body As String, ByVal description As String) As Boolean
      Dim valid As Boolean = True

      If String.IsNullOrEmpty(title) Or String.IsNullOrEmpty(body) Or String.IsNullOrEmpty(description) Then
         valid = False
      End If

      Return valid
   End Function


End Class
