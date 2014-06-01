<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Site/Views/Shared/TwoCol.master" Inherits="System.Web.Mvc.ViewPage(of AccountController.PasswordResetViewModel)" %>

<asp:Content ID="Content2" ContentPlaceHolderID="PageTitle" runat="server">
   Password Reset
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MiddlePlaceHolder" runat="server">
   <div style="margin: 10px">
      <%Html.ValidationSummary("Error submitting your information - ", New With {.class = "error"})%>
   </div>
   <% Using Html.BeginForm()%>
   <div>
      <p style="margin: 10px;" class="message">
         An Email with a reset code has been sent to the email address provided or the one
         in your file. Please allow a couple minutes depending on your mail server for delivery
         of that email.
         <br />
         <br />
         Enter below the code provided to you in your email in order to reset you password.
      </p>
      <div style="margin-bottom: 10px; background: #fafafa; padding: 10px;">
         <%If Model.Messages.Count > 0 Then%>
         <div style="margin: 5px; padding: 5px; background-color: #fff3cb;">
            <% For Each it As String In Model.Messages%>
            <p>
               <%= it %></p>
            <% Next%>
         </div>
         <% End If%>
         <table width="100%">
            <tr>
               <td align="right" style="width: 200px;">
                  <b style="font-size: 10px;">Reset Code:</b>
               </td>
               <td align="left">
                  <%=Html.TextBox("usercode", "", New With {.class = "rtc"})%>
               </td>
            </tr>
            <tr>
               <td>
               </td>
               <td>
                  <div style="padding: 5px 0">
                     <input type="submit" class="rb-btn rb-submit" onmouseover="this.className='rb-btn rb-submit-hover'"
                        onmouseout="this.className='rb-btn rb-submit'" value="" />
                  </div>
               </td>
            </tr>
         </table>
      </div>
      <%  End Using%>
   </div>
</asp:Content>
