Imports Microsoft.VisualBasic


Public Class SiteMapVideosDataProvider
   Inherits SiteMapDataProvider


   Public Sub New()
      MyBase.New("Videos", "~/videos/", "videos")
   End Sub
End Class

