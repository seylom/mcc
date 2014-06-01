<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Site/Views/Shared/TwoCol.master" Inherits="System.Web.Mvc.ViewPage(of vdContactUs)" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
Contact Us
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="middlePlaceHolder" runat="Server">
   <div class="contactUsContainer" style="padding-bottom: 20px; margin-bottom: 20px;
      border-bottom: 1px solid #eaeaea;">
      <h4 style="font-size: 12px;">
         Please let us know how we can help you .</h4>
      <br />
      <br />
      <p>
         If you have specific inquiries, please feel free to browse the help topics to find
         information on the website. If you would like to report any unaccurate or offensive
         content please submit a request with the title of the article so that we can follow
         up easily.
         <br />
         <br />
      </p>
      <p>
         To easily find articles, topics and videos, don't forget that you can use our <a
            id="A1" class="global" href="/search" >search</a> functionnality.
         Like all new website, some settings and functionnalities may change in the future.
         Please feel free to submit your <a id="A2" class="global" href="/feedback">
            feedback</a> on the website about section you would like to see improved to help
         us better serve you needs.
         <br />
         <br />
      </p>
      <p>
         If you came across something that might be an issue or a bug in the website please
         <a id="A3" class="global" href="/reportIssues">report</a> it and
         we will make sure the problem is taken care of as soon as possible.
         <br />
         <br />
      </p>
      <p>
         Thank you.
         <br />
         <br />
         <span>- <i>the MiddleClassCrunch Team</i> -</span>
      </p>
   </div>
   <strong>Contact form</strong>
   
   <%=Html.Notify()%>
   
   <% Using Html.BeginForm()%>
   <table cellspacing="4" id="ContactUsTable" style="width: 100%; margin-top: 20px;">
      <tr>
         <td style="width: 50%">
            Name:
         </td>
         <td style="width: 330px;">
            Email address
         </td>
      </tr>
      <tr>
         <td style="width: 50%">
            <%=Html.TextBox("name", Model.Name, New With {.class = "rtc", .style = "width:90%"})%>
         </td>
         <td>
            <%=Html.TextBox("email", Model.Email, New With {.class = "rtc", .style = "width:90%"})%>
         </td>
      </tr>
      <tr>
         <td colspan="2" style="">
            Subject:
         </td>
      </tr>
      <tr>
         <td colspan="2">
            <%=Html.TextBox("subject", Model.Subject, New With {.rows = 5, .style = "width:95%", .class = "rtc"})%>
         </td>
      </tr>
   </table>
   <table style="width: 100%">
      <tr>
         <td>
            Message
         </td>
      </tr>
      <tr>
         <td>
            <%=Html.TextArea("body", Model.body, New With {.cols = 4, .rows = 5, .class = "rtc", .style = "width:95%;margin-bottom:5px;"})%>
         </td>
      </tr>
      <%-- <tr>
         <td>
            <mcc:mccCaptchaCtrl ID="MccCaptchaCtrl1" runat="server" CaptchaHeight="40" CaptchaWidth="200"
               CssClass="captcha" Text="Enter image characters below." TextboxClass="mtc" />
         </td>
      </tr>--%>
      <tr>
         <td>
            <input type="submit" value="" class="rb-btn rb-submit" onmouseover="this.className='rb-btn rb-submit-hover'"
               onmouseout="this.className='rb-btn rb-submit'" />
         </td>
      </tr>
   </table>
   <% End Using%>
</asp:Content>
