<%@ Page Title="Upload Images - Admin MiddleClassCrunch" Language="VB" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master"
   Inherits="System.Web.Mvc.ViewPage(of AdminUploadImagesViewModel)" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
Upload Images
</asp:Content>
<asp:Content ContentPlaceHolderID="mainSiteContent" runat="server">
   <h2>
      Upload Images to server</h2>
   <div style="padding: 10px; margin-top: 5px;">
      <ul style="list-style-type: square; padding-left: 10px;">
         <li>All major mime type ar accepted for upload</li>
         <li>For database size reason, make sure you optimize images before upload</li>
         <li>Image of size larger than 80k will not be accepted</li>
         <li>Image size should be under 20k for avatars,40k for video still and 80k for article
            image</li>
         <li>Try to keep the overall image size under 450 x 450 (px)</li>
      </ul>
   </div>
   <% Using Html.BeginForm("UploadAddedFiles", "ImageAdmin", FormMethod.Post, New With {.enctype = "multipart/form-data"})%>
   <div style="padding: 10px; background-color: #fef6dc">
      <table style="width: 100%;" cellspacing="3">
         <tr>
            <td style="width: 100px; padding-right: 10px;" align="right">
               To be used for...
            </td>
            <td align="left">
               <%=Html.DropDownList("ImgType", CType(Model.ImgType, ImageType).ToSelectList())%>
            </td>
         </tr>
      </table>
   </div>
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
   <% If Model.Messages.Count > 0 Then%>
   <div style="padding: 5px; background-color: #e9fccc; margin-top: 5px;">
      <%For Each it As String In Model.Messages%>
      <p>
         <%=it%></p>
      <%Next%>
   </div>
   <% End If%>
</asp:Content>
