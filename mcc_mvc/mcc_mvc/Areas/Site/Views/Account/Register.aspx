<%@ Page Language="VB" MasterPageFile="~/Areas/Site/Views/Shared/TwoCol.master" Inherits="System.Web.Mvc.ViewPage(of RegisterViewModel)" %>

<asp:Content ID="Content2" ContentPlaceHolderID="PageTitle" runat="server">
   Register
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="middlePlaceHolder" runat="Server">
   <h2 class="title-dt">
      Registration: fast and easy process!</h2>
   <div id="reg" style="padding: 10px; background-color: #fafafa;">
      <%If Model.Messages.Count > 0 Then%>
      <div style="margin: 5px; padding: 5px; background-color: #fff3cb;">
         <% For Each it As String In Model.Messages%>
         <p>
            <%= it %></p>
         <% Next%>
      </div>
      <% End If%>
      <% Using Html.BeginForm()%>
      <table class="regTable" border="0" cellpadding="0" cellspacing="0" width="100%">
         <tr>
            <td class="right">
               <span style="font-weight: bold;">Username*</span>
            </td>
            <td class="left">
               <%=Html.TextBox("username", Model.Username, New With {.class = "rtc", .style = "width:200px;"})%>
            </td>
            <td>
            </td>
         </tr>
         <tr>
            <td class="right">
            </td>
            <td class="left" colspan="2">
               <span id="usernamecheck" style="color: #994138"></span>
            </td>
         </tr>
         <tr>
            <td class="right">
               <span style="font-weight: bold;">Password*</span>
            </td>
            <td class="left">
               <%=Html.Password("password", "", New With {.class = "rtc", .style = "width:200px;"})%>
            </td>
            <td id="passwordCheck">
            </td>
         </tr>
         <tr>
            <td class="right">
               <span style="font-weight: bold;">Confirm Password*</span>
            </td>
            <td class="left">
               <%=Html.Password("confirmPassword", "", New With {.class = "rtc", .style = "width:200px;"})%>
            </td>
            <td id="passwordCompareCheck">
            </td>
         </tr>
         <tr>
            <td class="right">
               <span style="font-weight: bold;">Email*</span>
            </td>
            <td class="left">
               <%=Html.TextBox("email", Model.Email, New With {.class = "rtc", .style = "width:200px;"})%>
            </td>
            <td id="Td1">
            </td>
         </tr>
         <tr>
            <td class="right">
               <span style="font-weight: bold;">Verification*</span>
            </td>
            <td class="left">
               <script type="text/javascript">
                  var RecaptchaOptions = {
                     theme: 'custom'
                  };
               </script>
               <%=Html.GenerateCaptcha()%>
            </td>
            <td id="Td2">
            </td>
         </tr>
         <tr>
            <td class="right">
               <%=Html.CheckBox("cbtermsofuse")%>
            </td>
            <td class="left" style="padding-left: 5px;">
               <span>Agree to the <a id="A1" class="global" href="/termsofuse/">terms of use</a></span>
            </td>
         </tr>
         <tr>
            <td class="right">
            </td>
            <td class="left">
               <span>
                  <input type="submit" class="rb-btn rb-submit" value="" onmouseover="this.className='rb-btn rb-submit-hover'"
                     onmouseout="this.className='rb-btn rb-submit'" />
               </span>
            </td>
         </tr>
      </table>
      <% End Using%>
      <div>
         (*) : Mandatory fields
      </div>
      <%--  <script type="text/javascript">
         function cmAgree_ClientValidate(sender, args) {
            args.IsValid = (document.getElementById("<%= cbtermsofuse.ClientID %>").checked);
         }

         var base_url = '<%= ResolveUrl("~/ajaxdata.ashx") %>';
         var user_ct = $('#<%= txtUsername.ClientID %>');

         $('#<%= txtUsername.ClientID %>').keyup(function() {
            var url = base_url + "?qid=ucheck&q=" + user_ct.val();
            $.get(url, function(res) {
               $("#usernamecheck").html(res);
            })
         });
      </script>--%>
   </div>
</asp:Content>
<asp:Content ID="sideContent" runat="server" ContentPlaceHolderID="rightPlaceHolder">
   <div class="widget">
      <h3 class="title">
         Privacy policy
      </h3>
      <div style="padding: 5px 10px; color: #9e2e1b;">
         <p style="margin-top: 10px;">
            We do not spam nor sell your email to any third party. We will not bother you with
            any communication or publication that you wouldn't have chosen to receive, unless
            you explicitly opt in. We take privacy very seriously and will take any necessary
            measure to keep it that way. Word!
            <br />
            <br />
            <span>- <i>the MiddleClassCrunch Team</i> -</span>
         </p>
      </div>
   </div>
</asp:Content>
<asp:Content ContentPlaceHolderID="scriptLoader" runat="server">
</asp:Content>
