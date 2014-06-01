<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Site/Views/Shared/TwoCol.master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
Account Activation
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="middlePlaceHolder" runat="Server">
    <h2 style="font-size: 12px; font-weight: bold;">
        Only one more step to go ...
    </h2>
    <p style="margin:10px 0;">
        Thank you for registering at MiddleClassCrunch.com<br />
        <br />
        An email with an activation link has been sent to the address provided during registration.
        Open the email and click on the link in order to activate your account and be able to use it.<br />
        After that step is done, you are good to go.<br />
        Thank you.<br />
        <br />
        - the MiddleClassCrunch team -
    </p>
    <a  href="/"  class="global">Back to the home page</a>
</asp:Content>
