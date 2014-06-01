<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage(of FileUploadStatus)" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
Upload Videos
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainSiteContent" runat="Server">
   <div style="border-bottom: 1px solid #eaeaea; padding: 5px; margin-bottom: 10px;">
      <h1 class="admin-title">
         Upload Video</h1>
   </div>
   <%= Html.DisplayError() %>
   <% Using Html.BeginForm("UploadAddedFiles", "VideoAdmin", FormMethod.Post, New With {.enctype = "multipart/form-data"})%>
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
   <% If Model.Message IsNot Nothing Then%>
   <div style="padding: 5px; background-color: #e9fccc; margin-top: 5px;">
      <%For Each it As String In Model.Message%>
      <p><%=it%></p>
      <%Next%>
   </div>
   <% End If%>
</asp:Content>
