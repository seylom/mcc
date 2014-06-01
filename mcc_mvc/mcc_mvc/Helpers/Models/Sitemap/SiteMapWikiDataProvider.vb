Imports Microsoft.VisualBasic


Public Class SiteMapWikisDataProvider
   Inherits SiteMapDataProvider


   Public Sub New()
      MyBase.New("Wiki Pages", "~/admin/Wiki_pages/", "Wiki Pages")
   End Sub
End Class
