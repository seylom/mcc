<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Site/Views/Shared/TwoCol.master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
   Password Reset
</asp:Content>
<asp:Content ContentPlaceHolderID="middlePlaceHolder" runat="server">
   <h2 style="font-size: 12px; font-weight: bold; border-bottom: 1px dotted #bababa;
      padding-bottom: 10px;">
      Trouble accessing your account?
   </h2>
   <% Using Html.BeginForm()%>
   <div style="margin-top: 10px; background: f1f1f1; width: 500px;">
      <%If Model.Messages.Count > 0 Then%>
      <div style="margin: 5px; padding: 5px; background-color: #fff3cb;">
         <% For Each it As String In Model.Messages%>
         <p>
            <%= it %></p>
         <% Next%>
      </div>
      <% End If%>
      <div>
         <p style="margin-top: 10px;">
            Enter below your Username or Email address and we will send you information to reset
            you password.
         </p>
         <div style="margin-top: 10px; background: #fafafa; padding: 10px">
            <table width="100%">
               <tr>
                  <td align="right" style="width: 200px;">
                     <b style="font-size: 10px;">Username or Email:</b>
                  </td>
                  <td align="left">
                     <%=Html.TextBox("userinfo", "", New With {.class = "rtc"})%>
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
      </div>
   </div>
   <%End Using%>
</asp:Content>
