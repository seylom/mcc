<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Site/Views/Shared/TwoCol.master" Inherits="System.Web.Mvc.ViewPage(of activationViewModel)" %>

<asp:Content ContentPlaceHolderID="middlePlaceHolder" runat="server">
   <% If Model.Approved Then%>
   <h2 class="head-title">
      Welcome to MiddleClassCrunch.com.
   </h2>
   <p style="margin-top: 10px;">
      Your account has been successfully activated. You may now post comments and rate
      articles, vote in polls, access content reserved to members and much more.<br />
   </p>
   <br />
   Go to the <a href="/login" class="global">login</a> page or back to the <a id="A3"
      href="/" class="global">Home</a> page.
   <%Else%>
   <h2 class="head-title">
      Oups!... account deleted or not found.
   </h2>
   <p style="margin-top: 10px;">
      The account you are trying to activate was either deleted or doesn't exist.<br />
      If you are sure you created an account on MiddleClassCrunch, it might have been
      deleted after 3 days without valid activation.<br />
      <br />
      Unfortunately, a new registration process is necessary.<br />
      Sorry for the inconvenience.
   </p>
   <p style="margin-top: 10px;">
      If you believe that you are receiving this message in error, we apologize in advance.
      Please contact us and we will be more than happy to assist you.
   </p>
   <br />
   Thank you.
   <br />
   <br />
   - the MiddleClassCrunch team -
   <% End If%>
</asp:Content>
