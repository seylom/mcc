<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ContentPlaceHolderID="PageTitle" runat="server">
   Management Tools
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainSiteContent" runat="Server">
   <div style="border-bottom: 1px solid #eaeaea; padding: 5px; margin-bottom: 10px;">
      <h1 class="admin-title">
         Tools</h1>
   </div>
   <div style="padding: 5px; margin: 5px; background-color: #effee9;">
      <%Using Html.BeginForm("PurgeServerCache","AdminHome")%>
      <input type="submit" value="Purge server Cache" />
      <%End Using%>
      <%Using Html.BeginForm("PurgeEvents","AdminHome")%>
      <input type="submit" value="Delete old Events" />
      <%End Using%>
      <%Using Html.BeginForm("RemoveNonActivatedAccounts","AdminHome")%>
      <input type="submit" value="Remove non activated accounts" />
      <%End Using%>
      <%Using Html.BeginForm("EncryptConfig","AdminHome")%>
      <input type="submit" value="Encrypt Database Connections" />
      <%End Using%>
      <%Using Html.BeginForm("DecryptConfig","AdminHome")%>
      <input type="submit" value="Decrypt Database Connections" />
      <%End Using%>
      <%Using Html.BeginForm("RefreshConfig","AdminHome")%>
      <input type="submit" value="Refresh Connection String Config" />
      <%End Using%>
   </div>
   <%-- <div style="padding: 5px; margin: 5px; background-color: #effee9;">
      <asp:Button runat="server" ID="btnTags" Text="Fix Articles Tags" />
      <asp:Button runat="server" ID="btnImageTags" Text="Fix Images Tags" />
      <asp:Button runat="server" ID="btnVideoId" Text="FixVideoId" />
      <asp:Button runat="server" ID="btnSetCategoryImageUrl" Text="Category Image Url" />
      <asp:Button runat="server" ID="btnSetPollIds" Text="Set Default poll Ids" />
      <asp:Button runat="server" ID="btnFixImagesPathToSimplePath" Text="Correct Image Paths" />
   </div>--%>
</asp:Content>
