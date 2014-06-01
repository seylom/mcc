<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Site/Views/Shared/TwoCol.master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
Non Active Account
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="middlePlaceHolder" runat="Server">
    <h2 class="head-title">
        This account has not yet been activated!
    </h2>
    <p style="margin-top: 10px;">
        Please check your email account for the activation email that was sent to you. If
        you didn't receive it or don't have it anymore you may have it resent to you to
        the email adress you used during registration by <a href="#" class="global">clicking
            here</a>.
    </p>
    <p style="margin-top: 10px;">
        If you don't remember the email address, you won't have any other choice but to
        <a id="A1" href="/login" class="global">create a new account</a>.
    </p>
    <p style="margin-top: 10px;">
        If none of these options work for you, please <a id="A2" href="/contact" runat="server" class="global">contact us</a> and we will be happy to
        assist you!
    </p>
    <br />
    Thank you.
    <br />
    <br />
    - the MiddleClassCrunch team -
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="rightPlaceHolder" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="bottomPlaceHolder" runat="Server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="scriptLoader" runat="Server">
</asp:Content>
