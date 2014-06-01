<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" ValidateRequest="false"  Inherits="System.Web.Mvc.ViewPage(of TicketViewModel)" %>

<%@ Import Namespace="MCC.Services" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
Edit Ticket
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainSiteContent" runat="Server">
   <div style="border-bottom: 1px solid #eaeaea; padding: 5px; margin-bottom: 10px;">
      <h1 class="admin-title">
         Edit Ticket</h1>
   </div>
   <% Using Html.BeginForm()%>
   <%=Html.DisplayError()%>
   <table class="edit-info" style="width: 100%;">
      <tr>
         <td class="right" style="width: 100px;">
            Title
         </td>
         <td class="left">
            <%=Html.TextBox("Title", Model.Title, New With {.class = "rtc", .style = "width:90%"})%>
         </td>
      </tr>
      <tr>
         <td class="right">
            Type
         </td>
         <td class="left">
            <%=Html.DropDownList("Type", CType(Model.Type, TicketType).ToSelectList())%>
         </td>
      </tr>
      <tr>
         <td class="right" style="width: 100px;">
            Owner
         </td>
         <td class="left">
            <%--  <asp:DropDownList runat="server" ID="ddlOwner" AppendDataBoundItems="true" SelectedValue='<%#Bind("Owner") %>'>
               <asp:ListItem Text="unassigned" Value="unassigned"></asp:ListItem>
            </asp:DropDownList>--%>
         </td>
      </tr>
      <tr>
         <td class="right">
            Piority
         </td>
         <td class="left">
            <%=Html.DropDownList("Priority", CType(Model.Type, TicketPriority).ToSelectList())%>
         </td>
      </tr>
      <tr>
         <td class="right">
            Description
         </td>
         <td class="left">
            <%=Html.TextArea("Description", Model.Description, New With {.class = "rtc mccEditor", .rows = 5, .cols = 12, .style = "width:90%"})%>
         </td>
      </tr>
      <tr>
         <td>
         </td>
         <td class="left">
            <input type="submit" class="rb-btn rb-update" onmouseover="this.className='rb-btn rb-update-hover'"
               value="" onmouseout="this.className='rb-btn rb-update'" />
            <input type="button" class="rb-btn rb-cancel" onmouseover="this.className='rb-btn rb-cancel-hover'"
               onmouseout="this.className='rb-btn rb-cancel'" />
         </td>
      </tr>
   </table>
   <% End Using%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottomScripts" runat="Server">
   <%=appHelpers.LocalScriptsTagUrl("tinymce/jscripts/tiny_mce/tiny_mce.js")%>

   <script type="text/javascript">
      tinyMCE.init({
         theme: "advanced",
         mode: "none",
         mode: "textareas",
         editor_selector: "mccEditor",
         content_css: '<%=ResolveUrl("~/app_themes/adminsimple/adminsimple.css?v=1.4") %>'
      });
   </script>

</asp:Content>
