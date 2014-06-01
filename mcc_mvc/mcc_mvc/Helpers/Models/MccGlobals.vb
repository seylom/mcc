Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Configuration
Imports System.Web.Configuration
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls


Public NotInheritable Class MCCGlobals

   Public Shared ReadOnly Settings As MCCSection = CType(WebConfigurationManager.GetSection("MCC"), MCCSection)
   Public Shared ThemesSelectorID As String = ""

   Shared Sub New()

   End Sub

   Public Shared Sub ProtectSection(ByVal sectionName As String, ByVal provider As String)
      Dim config As Configuration = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath)

      Dim section As ConfigurationSection = config.GetSection(sectionName)

      If section IsNot Nothing AndAlso Not section.SectionInformation.IsProtected Then
         section.SectionInformation.ProtectSection(provider)
         config.Save()
      End If
   End Sub

   Public Shared Sub UnProtectSection(ByVal sectionName As String)
      Dim config As Configuration = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath)

      Dim section As ConfigurationSection = config.GetSection(sectionName)

      If section IsNot Nothing AndAlso section.SectionInformation.IsProtected Then
         section.SectionInformation.UnprotectSection()
         config.Save()
      End If
   End Sub

End Class
