<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage(of AdminUpdateVideoViewModel)" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
Update Video File
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainSiteContent" runat="Server">
   <div style="border-bottom: 1px solid #eaeaea; padding: 5px; margin-bottom: 10px;">
      <h1 class="admin-title">
         Update Video File</h1>
   </div>
   <%= Html.DisplayError() %>
   <table style="width: 100%;">
      <tr>
         <td style="width: 70px;">
            <b>Title:</b>
         </td>
         <td>
            <%= Model.Title %>
         </td>
      </tr>
      <tr>
         <td>
            <b>Name:</b>
         </td>
         <td>
            <%= Model.Name %>
         </td>
      </tr>
      <tr>
         <td>
            <b>url:</b>
         </td>
         <td>
            <%= Model.VideoUrl %>
         </td>
      </tr>
      <tr>
         <td style="width: 50px;">
            <b>Description:</b>
         </td>
         <td>
            <%= Model.Description %>
         </td>
      </tr>
   </table>
   <% Using Html.BeginForm("UpdateVideoFile", "VideoAdmin", FormMethod.Post, New With {.enctype = "multipart/form-data"})%>
   <input type="hidden" name="VideoID" value="<%=Model.VideoID %>" />
   <input type="hidden" name="Name" value="<%=Model.Name %>" />
   <div style="padding: 5px; background: #eaeaea;">
      <table>
         <tr>
            <td style="width: 225px;">
               <input type="file" name="UploadFile" style="width: 200px;" />
            </td>
            <td>
               <input type="submit" class="rb-btn rb-upload" value="" />
            </td>
         </tr>
      </table>
   </div>
   <% End Using%>
   <% If Model.UploadResults IsNot Nothing Then%>
   <div style="padding: 5px; background-color: #e9fccc; margin-top: 5px;">
      <%For Each it As String In Model.UploadResults%>
      <p>
         it</p>
      <%Next%>
   </div>
   <% End If%>
</asp:Content>
