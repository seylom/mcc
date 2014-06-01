<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage(of adminVideoCategoriesViewModel)" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
   Video Categories
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainSiteContent" runat="Server">
   <div style="border-bottom: 1px solid #eaeaea; padding: 5px; margin-bottom: 10px;">
      <h1 class="admin-title">
         Videos Categories</h1>
   </div>
   <table class="edit-info">
      <thead>
         <tr>
            <th>
               Title
            </th>
            <th>
               Description
            </th>
            <th>
               Importance
            </th>
            <th>
               Image Url
            </th>
            <th>
            </th>
            <th>
            </th>
         </tr>
      </thead>
      <tbody>
            <%Dim idx As Integer = 0%>
            <% For Each it As VideoCategory In Model.Categories%>
            <tr class='<%= IIF(idx Mod 2 = 0,"even", "odd") %>'>
               <td class="title">
                  <%=Html.Encode(it.Title)%>
               </td>
               <td>
                  ...
               </td>
               <td class="importance">
                  <%=it.Importance%>
               </td>
               <td>
                  <a href="javascript:void(0);">
                     <%=it.ImageUrl%></a>
               </td>
               <td style="width:30px">
                  <a href="#">Edit</a>
               </td>
               <td  style="width:20px">
                  <%Using Html.BeginForm("DeleteCategory", "VideoAdmin", New With {.Id = it.CategoryID})%>
                  <input type="image" src="<%= apphelpers.ImageUrl("MicroIcons/delete.png") %>" />
                  <% End Using%>
               </td>
            </tr>
            <% idx += 1%>
            <%Next%>
      </tbody>
   </table>
   <%=Html.AdvancedPager(Model.Categories, "ShowCategories", "VideoAdmin", New Integer() {10, 30, 50})%>
</asp:Content>
