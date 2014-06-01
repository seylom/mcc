<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage(of TipViewData)" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
Tip Categories
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainSiteContent" runat="Server">
   <div style="border-bottom: 1px solid #eaeaea; padding: 5px; margin-bottom: 10px;">
      <h1 class="admin-title">
         Tips Categories</h1>
   </div>
   <table class="edit-info" cellspacing="1">
      <thead>
         <tr class="header">
            <th>
               ID
            </th>
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
               Command
            </th>
         </tr>
      </thead>
      <tbody>
      <%Dim idx As Integer = 0%>
      <%For Each it As AdviceCategory In Model.Categories%>
        <tr  class='<%= IIF( idx Mod 2 = 0,"even", "odd") %>'>
                <td class="id">
                    <%=it.CategoryID%>
                </td>
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
                <td class="command">
                    <a  class="lnk" href="<%= Url.action("EditCategory",new with {.Id = It.CategoryId}) %>"  title="Edit" >Edit</a>            
                </td>
            </tr>
      
      
      <%idx += 1%>
      <%Next%>
      </tbody>
   </table> 
</asp:Content>
