<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage(of AdminUserViewModel)" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
   Edit User:
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainSiteContent" runat="Server">
   <div style="border-bottom: 1px solid #eaeaea; padding: 5px; margin-bottom: 10px;">
      <h1 class="admin-title">
         User Detail</h1>
   </div>
   <table class="edit-info" cellspacing="1" width="100%">
      <tbody>
         <tr>
            <td class="right" style="width: 250px;">
               UserName:
            </td>
            <td class="left">
               <%=Model.Username%>
            </td>
         </tr>
         <tr>
            <td class="right">
               E-mail:
            </td>
            <td class="left">
               <%=Model.Email%>
            </td>
         </tr>
         <%--  <tr>
            <td class="right">
               Registered:
            </td>
            <td class="left">
               <%=Model. %>
            </td>
         </tr>--%>
         <tr>
            <td class="right">
               Last Login:
            </td>
            <td class="left">
            </td>
         </tr>
         <tr>
            <td class="right">
               Last Activity:
            </td>
            <td class="left">
               <%=Model.LastActivityDate%>
            </td>
         </tr>
         <tr>
            <td class="right">
               Online Now
            </td>
            <td class="left">
               <%=Html.CheckBox("IsOnline", Model.isOnline, New With {.readonly = "readonly"})%>
            </td>
         </tr>
         <tr>
            <td class="right">
               Approved
            </td>
            <td class="left">
               <%=Html.CheckBox("IsApproved", Model.IsApproved)%>
            </td>
         </tr>
         <tr>
            <td class="right">
               Locked Out
            </td>
            <td class="left">
               <%=Html.CheckBox("IsLockedOut", Model.IsLockedOut)%>
            </td>
         </tr>
         <tr>
            <td class="right">
               Roles
            </td>
            <td class="left">
               <%Using Html.BeginForm("UpdateUserRoles", "UserAdmin", New With {.username = Model.Username})%>
               <% For Each it As String In Model.Roles%>
               <div style="margin: 2px 0;">
                  <%=CreateCheckBox("UserRoles", it, Model.UserRoles.Contains(it), Nothing)%>&nbsp;<%=it%>
               </div>
               <%Next%>
               <input type="submit" class="rb-btn rb-update" onmouseover="this.className='rb-btn rb-update-hover'"
                  value="" onmouseout="this.className='rb-btn rb-update'" />
               <% End Using%>
            </td>
         </tr>
      </tbody>
   </table>
   <%Using Html.BeginForm()%>
   <table class="edit-info" cellspacing="1" width="100%">
      <tr>
         <th colspan="2">
            profile Information
         </th>
      </tr>
      <tr>
         <td class="right" style="width: 250px;">
            DisplayName:
         </td>
         <td class="left">
            <input type="text" style="width: 90%" name="DisplayName" class="rtc" value="<%= Model.Displayname %>" />
         </td>
      </tr>
      <tr>
         <td class="right">
            E-mail:
         </td>
         <td class="left">
            <input type="text" style="width: 90%" name="Email" class="rtc" value="<%= Model.Email %>" />
         </td>
      </tr>
      <tr>
         <td class="right">
            Website
         </td>
         <td class="left">
            <input type="text" style="width: 90%" name="Website" class="rtc" value="<%= Model.Website %>" />
         </td>
      </tr>
      <tr>
         <td class="right">
            About
         </td>
         <td class="left">
            <%=Html.TextArea("About", Model.About, 5, 12, New With {.style = "width:90%", .class = "rtc"})%>
         </td>
      </tr>
      <tr>
         <td class="right">
         </td>
         <td class="left">
            <input type="submit" class="rb-btn rb-update" onmouseover="this.className='rb-btn rb-update-hover'"
               value="" onmouseout="this.className='rb-btn rb-update'" />
         </td>
      </tr>
   </table>
   <% End Using%>
</asp:Content>
