Namespace MCC.Areas.Admin.Controllers


   Public Class AdminHomeController
      Inherits AdminController

      '
      ' GET: /AdminHome/

      Function Index() As ActionResult

         Dim viewdata As New AdminDashboardViewData
         Return View(viewdata)
      End Function

      Function Notifications() As ActionResult
         Return View()
      End Function

      Function ShowTasks() As ActionResult
         Return View()
      End Function


      Function ShowRoles() As ActionResult


         Return View()
      End Function


      Function ManagementTools() As ActionResult
         Return View()
      End Function

      <AcceptVerbs(HttpVerbs.Post)> _
      Public Function PurgeServerCache() As ActionResult
         Utils.PurgeCache()
         Return RedirectToAction("ManagementTools")
      End Function

      <AcceptVerbs(HttpVerbs.Post)> _
      Protected Sub PurgeEvents()
         ' delete everything older than a month old!
         WebEvents.DeleteOldEvent(DateTime.Now)
      End Sub

      <AcceptVerbs(HttpVerbs.Post)> _
      Public Function RemoveNonActivatedAccounts() As ActionResult
         'Dim li As MembershipUserCollection = Membership.GetAllUsers
         'For Each it As MembershipUser In li
         '   If it.Then Then

         '   End If
         'Next
         Return RedirectToAction("ManagementTools")
      End Function

      <AcceptVerbs(HttpVerbs.Post)> _
      Public Function DecryptConfig() As ActionResult
         MCCGlobals.UnProtectSection("connectionStrings")
         Return RedirectToAction("ManagementTools")
      End Function

      <AcceptVerbs(HttpVerbs.Post)> _
      Public Function EncryptConfig() As ActionResult
         MCCGlobals.ProtectSection("connectionStrings", "DataProtectionConfigurationProvider")
         Return RedirectToAction("managementTools")
      End Function
   End Class
End Namespace
