<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Site/Views/Shared/TwoCol.master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
   Account Activation
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="middlePlaceHolder" runat="Server">
   <h2 class="head-title">
      This account has already been activated!
   </h2>
   <p style="margin-top: 10px;">
      You may <a id="A1" href="/login" class="global" runat="server">log in</a> at any
      time.
   </p>
</asp:Content>
