<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Site/Views/Shared/TwoCol.master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ContentPlaceHolderID="PageTitle" runat="server">
Access Denied
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="middlePlaceHolder" runat="Server">
   <div class="clear">
      <div style="float: left;">
         <img src="<%= appHelpers.ImageUrl("accessdenied.png")  %>" alt="Access denied" />
      </div>
      <h4 style="font-size: 12px; margin-bottom: 10px;">
         Unsufficient Credentials ...</h4>
      <p style="margin-bottom: 10px;">
         Sorry, but you are trying to access pages with insufficient credentials. You must
         be a registered user with proper rights to access this page. If you already have
         an account, please <a class="global" href="/login" title="Log in">log in</a> using
         your credentials. Otherwise <a class="global" href="/signup" title="Register">register</a>
         now for free.
      </p>
      <p style="margin-bottom: 10px;">
         If you were trying to access content or functionnalities not reserved to subscribers,
         please make sure you are logged in with the proper credentials. If the content is
         reserved to subscibers consider <a class="global" href="/signup" alt="Register">registering</a>
         to our website for free (fast and easy process)
      </p>
      <p style="margin-bottom: 10px;">
         In case you have reached this page in an error, please contact us so that we can
         make the necessary adjustment to fix it.
      </p>
      <p style="margin-bottom: 10px;">
         <span>- <i>the MiddleClassCrunch team</i> -</span>
      </p>
   </div>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="rightPlaceHolder">
   <div class="widget">
      <h2 class="title">
         More...
      </h2>
   </div>
</asp:Content>
