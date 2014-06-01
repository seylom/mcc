<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage(of AdminImageViewModel)" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
Update Image File
</asp:Content>
<asp:Content ContentPlaceHolderID="mainSiteContent" runat="server">
   <h2 style="font-size: 11px; font-weight: bold;">
      Image Info -
   </h2>
   <table width="100%" style="margin-left: 10px; margin-bottom: 10px;">
      <tr>
         <td align="right" style="width: 100px;">
            <b>path:</b>
         </td>
         <td>
            <i>[<%=Model.ImageUrl%>]</i>
         </td>
      </tr>
      <tr>
         <td align="right">
            <b>uuid:</b>
         </td>
         <td>
            <i>[<%=Model.Uuid%>]</i>
         </td>
      </tr>
      <tr>
         <td align="right">
            <b>Name:</b>
         </td>
         <td>
            <i>[<%=Model.Name%>]</i>
         </td>
      </tr>
      <tr>
         <td align="right">
            <b>Credits Name:</b>
         </td>
         <td>
            <%=Model.CreditsName%>
         </td>
      </tr>
      <tr>
         <td align="right">
            <b>Credits Url:</b>
         </td>
         <td>
            <%=Model.CreditsUrl%>
         </td>
      </tr>
   </table>
   <% Using Html.BeginForm("UpdateImage", "ImageAdmin", FormMethod.Post, New With {.enctype = "multipart/form-data"})%>
   <input type="hidden" name="ImageID" value="<%=Model.ImageID %>" />
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
  <% If Model.Messages.Count > 0 Then%>
   <div style="padding: 5px; background-color: #e9fccc; margin-top: 5px;">
      <%For Each it As String In Model.Messages%>
      <p>
         it</p>
      <%Next%>
   </div>
   <% End If%>
</asp:Content>
