<%@ Page Language="VB" MasterPageFile="~/Areas/Site/Views/Shared/singlewrapper.master"
   Inherits="System.Web.Mvc.ViewPage(of logInViewModel)" %>

<%@ Import Namespace="System.Web" %>

<asp:Content ID="Content2" ContentPlaceHolderID="PageTitle" runat="server">
   Log In
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="middlePlaceHolder" runat="Server">
   <%=""%>
   <div id="LoginControlBase" class="mbc">
      <div>
         <h2 style="font-size: 11px; font-weight: bold;">
            Welcome to MiddleClassCrunch!</h2>
         <div style="margin: 10px 0;">
            Sin-in in order to rate , comments and view content reserved for registered users.<br />
            If you don't have an account take a couple of seconds to register for free!
         </div>
      </div>
      <table style="width: 100%">
         <tr>
            <td>
               <div style="width: 400px; padding: 10px; background-color: #fafafa;">
                  <div style="margin-bottom:10px;padding: 0 10px;">
                     Returning Users
                  </div>
                  <%If Model.Messages.Count > 0 Then%>
                  <div style="margin: 5px; padding: 5px; background-color: #fff3cb;">
                     <% For Each it As String In Model.Messages%>
                     <p>
                        <%= it %></p>
                     <% Next%>
                  </div>
                  <% End If%>
                  <% Using Html.BeginForm("LogOn","Account")%>
                  <input type="hidden" name="ReturnUrl" value="<%= IIf(Not String.IsNullOrEmpty(Request.QueryString("returnUrl")), Request.QueryString("returnUrl"), IIf(Request.UrlReferrer IsNot Nothing, Request.UrlReferrer, String.Empty))%>" />
                  <table cellpadding="4" cellspacing="6">
<%--                     <tr>
                        <td>
                        </td>
                        <td align="right">
                           <span style="font-weight: bold;"></span>
                        </td>
                     </tr>--%>
                     <tr>
                        <td align="right">
                           <span style="font-weight: bold;">* Username</span>
                        </td>
                        <td>
                           <%=Html.TextBox("Username", Model.Username, New With {.class = "rtc", .style = "width:200px;"})%>
                        </td>
                     </tr>
                     <tr>
                        <td align="right">
                           <span style="font-weight: bold;">* Password</span>
                        </td>
                        <td>
                           <%= Html.Password("Password", "", New With {.class = "rtc", .style = "width:200px;"})%>
                        </td>
                     </tr>
                     <tr>
                        <td>
                        </td>
                        <td align="left">
                           <input type="submit" class="rb-btn rb-signin" onmouseover="this.className='rb-btn rb-signin-hover'"
                              onmouseout="this.className='rb-btn rb-signin'" value="" />
                        </td>
                     </tr>
                     <tr>
                        <td>
                        </td>
                        <td align="left">
                           Forgot your <a class="global" href="/resetpassword">password</a><span>?</span>
                        </td>
                     </tr>
                     <tr>
                        <td align="right">
                           <%=Html.CheckBox("rememberMe", Model.RememberMe)%>
                        </td>
                        <td align="left">
                           <span>Remember me next time</span>
                        </td>
                     </tr>
                     <tr>
                        <td align="right"></td>
                        <td  align="left">
                        
                        </td>
                     </tr>
                  </table>

                  <div style="margin:10px 0;">
                  (*): Required Fields
                  </div>
                  <%End Using%>
                  <div style="padding: 10px; border-top: 1px dotted #bababa; margin-top: 10px; background-color: #eaeaea;">
                     <b>Don't have an account?</b><br />
                     <%=Html.ActionLink("register", "Register", New With {.returnUrl = "/"}, New With {.class = "global"})%>
                     now for free!
                  </div>
               </div>
            </td>
            <td valign="top">
              <%-- <div style="padding: 20px;">
                  <h2 style="font-size: 11px;">
                     You may also sign-in using your google account or any OpenId provider</h2>

                     <% Using Html.BeginForm("AuthenticateWithOpenId", "Account")%>
                     <div style=" margin-top:10px;">
                     <%= Html.TextBox("openid_identifier","",New With {.class = "rtc", .style="width:300px;"}) %>
                      <input id="btnOpenIpAuth" type="submit" class="rb-btn rb-signin" onmouseover="this.className='rb-btn rb-signin-hover'"
                              onmouseout="this.className='rb-btn rb-signin'" value="" />
                     </div>
                     <% End Using%>

                  <div style="text-align: left; margin-top: 5px; padding:0 10px;">
                     <a href="#" class="global" onclick="auth('google')">Google</a>&nbsp;|&nbsp<a href="#" class="global" onclick="auth('myopenid')">MyOpenId</a>&nbsp;|&nbsp;<a
                        href="#" class="global" onclick="auth('yahoo')">Yahoo</a>&nbsp;|&nbsp<a href="#" class="global" onclick="auth('facebook')">Facebook</a>
                  </div>
               </div>--%>
            </td>
         </tr>
      </table>
   </div>
</asp:Content>
<asp:Content ContentPlaceHolderID="scriptLoader" runat="server">
<script type="text/javascript">
   function auth(provider) {

      var providerUrl;
      var auto = false;

      switch (provider) {
         case 'google':
            providerUrl = 'https://www.google.com/accounts/o8/id';
            auto = true;
            break;
         case 'yahoo':
            providerUrl = 'http://yahoo.com/';
            auto = true;
            break;
         case 'myopenid':
            providerUrl = '<your account>.myopenid.com';
            auto = false;
            break;
         case 'facebook':
            providerUrl = 'https://graph.facebook.com/oauth/authorize';
            auto = true;
            break;
      }

      if (providerUrl)
      {
         $('#openid_identifier').val(providerUrl).focus();

         if (auto == true) {
            $('#btnOpenIpAuth').trigger('click');
         }
         //submit the link
      }
   }
</script>
</asp:Content>
