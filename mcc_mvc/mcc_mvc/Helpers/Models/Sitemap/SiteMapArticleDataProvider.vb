Imports Microsoft.VisualBasic


Public Class SiteMapArticlesDataProvider
   Inherits SiteMapDataProvider


   Public Sub New()
      MyBase.New("Posts", "~/Articles/", "Posts")
   End Sub
End Class

