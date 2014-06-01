Imports Microsoft.VisualBasic


Public NotInheritable Class PageHelpers

   Public Shared Sub AddIsAuthenticatedToPage(ByVal page As Page)
      If HttpContext.Current.User.Identity.IsAuthenticated Then
         Dim cs As ClientScriptManager = page.ClientScript
         If Not cs.IsClientScriptBlockRegistered("authenticated") Then
            cs.RegisterClientScriptBlock(GetType(String), "authenticated", "<script type='text/javascript'>" & _
                                         "var isAuthenticated = 'true';" & _
                                         "</script>")
         End If
      End If
   End Sub



End Class
