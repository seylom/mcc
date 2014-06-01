Imports MCC.Data
Imports MCC.Services

Public Class ImageBrowserViewData


   Public Sub New()
      Dim _imagetagsrvr As New ImageTagService()
      _ImageTags = _imagetagsrvr.GetImageTags()


      Dim videoTagServ As New VideoTagService()
      _videoTags = Nothing
   End Sub

   Private _ImageTags As List(Of ImageTag)
   Public Property ImageTags() As List(Of ImageTag)
      Get
         Return _ImageTags
      End Get
      Set(ByVal value As List(Of ImageTag))
         _ImageTags = value
      End Set
   End Property



   Private _videoTags As List(Of VideoTag)
   Public Property VideoTags() As List(Of VideoTag)
      Get
         Return _videoTags
      End Get
      Set(ByVal value As List(Of VideoTag))
         _videoTags = value
      End Set
   End Property



End Class
