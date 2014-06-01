<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Site/Views/Shared/TwoCol.master" Inherits="System.Web.Mvc.ViewPage(of AccountController.PasswordResetViewModel)" %>
<asp:Content ID="Content2" ContentPlaceHolderID="PageTitle" runat="server">
Password Reset
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MiddlePlaceHolder" runat="server">

     <%Using Html.BeginForm%>
   <p style="margin: 10px;">
      Please change your password now. Your new password must be between 6 and 10 characters
      long.
   </p>
   <div style="margin: 10px 0; background: #fafafa; padding: 10px;">
      <table width="100%">
         <tr>
            <td align="right" style="width: 200px;">
               <b style="font-size: 10px;">New Password:</b>
            </td>
            <td align="left">
               <%=Html.Password("newPassword", "", New With {.class = "rtc"})%>
            </td>
         </tr>
         <tr>
            <td align="right">
               <b style="font-size: 10px;">Confirm New Password:</b>
            </td>
            <td align="left">
               <%=Html.Password("ConfirmPassword", "", New With {.class = "rtc"})%>
            </td>
         </tr>
         <tr>
            <td>
            </td>
            <td>
               <div style="padding: 10px 0">
                  <input type="submit" class="rb-btn rb-submit" onmouseover="this.className='rb-btn rb-submit-hover'"
                     onmouseout="this.className='rb-btn rb-submit'" value="" />
               </div>
            </td>
         </tr>
      </table>
   </div>
   <% End Using%>

</asp:Content>

