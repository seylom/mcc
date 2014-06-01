<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Site/Views/Shared/TwoCol.master" Inherits="System.Web.Mvc.ViewPage(of UserProfileViewModel)" %>

<asp:Content ID="Content4" ContentPlaceHolderID="PageTitle" runat="server">
   User Profile
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="headItems" runat="server">
   <%=appHelpers.CssTagUrl("bbcode/style.css")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="middlePlaceHolder" runat="Server">
   <h2 class="title-dt">
      User Information:</h2>
   <div id="tabs">
      <div id="tblks">
         <div class="tb_lkp">
            <a id="A0" class="global" href='/profile?v=pra#info_details'>Profile</a>
         </div>
         <div class="tb_lkp">
            <a id="A3" class="global" href='/profile?v=pra#notifications'>Notifications</a>
         </div>
      </div>
      <div id="info_details_bx" class="tabp">
         <%Using Html.BeginForm("UpdateProfile", "User")%>
         <%=Html.DisplayError(Model)%>
         <div id="pra" style="margin-top: 20px; border-top: 1px dotted #bababa; padding: 10px 0;">
            <table style="width: 100%;">
               <tr>
                  <td style="width: 150px" valign="top">
                     <label for="DisplayName" class="profile" title="Display Name:">
                        Display Name:
                     </label>
                  </td>
                  <td>
                     <%=Html.TextBox("DisplayName", ViewData.Model.DisplayName, New With {.class = "rtc", .style = "width:300px;"})%>
                  </td>
               </tr>
               <tr>
                  <td>
                     <label for="Email" class="profile" title="Email (Not shown!):">
                        Email</label>
                  </td>
                  <td>
                     <%=Html.TextBox("Email", Model.Email, New With {.class = "rtc", .style = "width:300px;"})%>
                  </td>
               </tr>
               <tr>
                  <td>
                     <label for="Website" class="profile" title="Website:">
                        Website</label>
                  </td>
                  <td>
                     <%=Html.TextBox("Website", ViewData.Model.Website, New With {.class = "rtc", .style = "width:300px;"})%>
                  </td>
               </tr>
               <tr>
                  <td>
                     <span class="profile">A few words about yourself...</span>
                  </td>
                  <td>
                     <% Html.RenderPartial("/Areas/Site/views/shared/bbCodeEditor.ascx", ViewData.Model.body)%>
                  </td>
               </tr>
               <tr>
                  <td>
                  </td>
                  <td>
                     <input type="submit" class="rb-btn rb-update" onmouseover="this.className='rb-btn rb-update-hover'"
                        onmouseout="this.className='rb-btn rb-update'" value="" />
                  </td>
               </tr>
            </table>
         </div>
         <%End Using%>
         <%Using Html.BeginForm("ChangePassword", "User")%>
         <%=Html.DisplayError(Model)%>
         <div style="margin-top: 20px; border-top: 1px dotted #bababa; padding: 5px 0;">
            <h3 style="font-size: 12px;">
               Change Password</h3>
            <div style="margin-top: 10px;">
               <table>
                  <tr>
                     <td style="width: 150px;">
                        <label for="Password" class="profile" title="Old Password:">
                           Old Password</label>
                     </td>
                     <td>
                        <%=Html.Password("Password", "", New With {.class = "rtc", .style = "width:300px;"})%>
                     </td>
                  </tr>
                  <tr>
                     <td>
                        <label for="NewPassword" class="profile" title="NewPassword:">
                           New Password</label>
                     </td>
                     <td>
                        <%=Html.Password("NewPassword", "", New With {.class = "rtc", .style = "width:300px;"})%>
                     </td>
                  </tr>
                  <tr>
                     <td>
                        <label for="ConfirmPassword" class="profile" title="ConfirmPassword:">
                           Confirm Password</label>
                     </td>
                     <td>
                        <%=Html.Password("ConfirmPassword", "", New With {.class = "rtc", .style = "width:300px;"})%>
                     </td>
                  </tr>
                  <tr>
                     <td>
                     </td>
                     <td>
                        <input type="submit" class="rb-btn rb-update" onmouseover="this.className='rb-btn rb-update-hover'"
                           onmouseout="this.className='rb-btn rb-update'" value="" />
                     </td>
                  </tr>
               </table>
            </div>
         </div>
         <% End Using%>
      </div>
      <div id="notifications_bx" class="tabp">
      </div>
   </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="rightPlaceHolder" runat="server">
   <div class="widget">
      <h3 class="title">
         Profile Information</h3>
      <div style="padding: 5px;">
         <img src="<%= MCC.Routines.Gravatar(Model.Email,80) %>" width="80" alt="avatar" height="80"
            style="background-color: red" />
         your <a href="http://www.gravatar.com" class="globalred" title="gravatar website">gravatar</a>
      </div>
      <div style="border-top:1px dotted #bababa;margin-top:10px;padding:5px 0;">
         <ul>
            <li style="margin-bottom:5px;"><a class="global" href="<%= url.action("UserQuestions","User", new with {.user = user.identity.name}) %>">
               your questions</a> </li>
            <li><a class="global"  href="<%= url.action("UserAnswers","User", new with {.user = user.identity.name}) %>">
               your answers</a> </li>
         </ul>
      </div>
   </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="scriptLoader" runat="Server">
   <%=appHelpers.ScriptsTagUrl("global.js")%>

   <script type="text/javascript">
      $(function InitTabs() {
         $('#tabs div.tabp').hide(); // Hide all divs

         var mhash = document.location.hash;
         if (mhash != '') {
            //$(mhash + "_bx").show();
            act();
         }
         else {
            $('#tabs div.tabp:first').show(); // Show the first div
         }

         $('#tabs div.tb_lkp:first').addClass('active'); // Set the class of the first link to active

         $('#tabs div.tb_lkp a').click(function() { //When any link is clicked
            //            $('#tabs div.tb_lkp').removeClass('active'); // Remove active class from all links
            //            $(this).parent().addClass('active'); //Set clicked link class to active
            //            var href = $(this).attr('hash');
            //            var currentTab = href; // Set variable currentTab to value of href attribute of clicked link
            //            $('#tabs div.tabp').hide(); // Hide all divs
            //            $(currentTab + "_bx").show(); // Show div with id equal to variable currentTab
            //            //return false;

            act(this);
         });

      });

      function act(el) {
         if (el) {
            $('#tabs div.tb_lkp').removeClass('active'); // Remove active class from all links
            $(el).parent().addClass('active'); //Set clicked link class to active
            var href = $(el).attr('hash');
            var currentTab = href; // Set variable currentTab to value of href attribute of clicked link
            $('#tabs div.tabp').hide(); // Hide all divs
            $(currentTab + "_bx").show(); // Show div with id equal to variable currentTab
            //return false;
         }
      }
    
   </script>

</asp:Content>
