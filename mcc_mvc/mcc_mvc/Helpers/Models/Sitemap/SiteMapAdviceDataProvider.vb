Imports Microsoft.VisualBasic

Public Class SiteMapAdvicesDataProvider
   Inherits SiteMapDataProvider
   Public Sub New()
      MyBase.New("Tips", "~/Tips/", "Tips")
   End Sub
End Class
