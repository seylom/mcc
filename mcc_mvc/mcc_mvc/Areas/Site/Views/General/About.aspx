<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Site/Views/Shared/TwoCol.master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content2" ContentPlaceHolderID="PageTitle" runat="server">
About
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="middlePlaceHolder" runat="Server">
    <h2 style="font-size: 11px; font-weight: bold; margin-bottom: 10px;">
        Who are we?</h2>
    <p style="margin-bottom: 10px;">
        Did you know the Middle Class used to thrive and rule America back in the day after
        World War ? This was a time when being part of this socio-economic population was
        a blessing and a guarantee for a cushy and promising existence.
    </p>
    <p style="margin-bottom: 10px;">
        Well, if you are a baby-boomer then you are aware of that. Maybe you even have fond
        memories of these long-gone days.
    </p>
    <p style="margin-bottom: 10px;">
        But things have changed since the late 80's... Being part of the Middle Class nowadays
        implies credit card debt, out of control mortgage payments and unhealthy job-hopping.
    </p>
    <p style="margin-bottom: 10px;">
        MiddleClassCrunch.com is an interactive think-tank for all of us Middle Class people
        who strive to better our financial lives. The solutions are plentiful and being
        creative in our times can tremendously help... but to succeed in that endeavor,
        we must channel our energy and gather our resources.
    </p>
    <p style="margin-bottom: 10px;">
        This is what this site is for. Come join our community, share your knowledge and
        learn a couple of things in the process!
    </p>
    <div style="padding-top: 20px; padding: 10px; margin-bottom: 10px;">
        <h2 style="font-size: 11px; font-weight: bold; margin-bottom: 10px;">
            The MiddleClassCrunch Team members</h2>
        <b>--- <i>the founders</i> ---</b>
       <%-- <uca:author ID="fabien" runat="server" Visible="true" Username="fabient" />--%>
       <%Html.RenderPartial("~/Areas/Site/views/shared/author.ascx", New AuthorInfoCard("fabient"))%>
        <br />
        <br />
        <%Html.RenderPartial("~/Areas/Site/views/shared/author.ascx", New AuthorInfoCard("webmaster"))%>
     <%--   <uca:author ID="seylom" runat="server" Visible="true" Username="webmaster" />--%>
        <br />
        <br />
        <%Html.RenderPartial("~/Areas/Site/views/shared/author.ascx", New AuthorInfoCard("keithb"))%>
        <%--<uca:author ID="keith" runat="server" Visible="true" Username="keithb" />--%>
    </div>
    <div style="padding-top: 20px; padding: 10px;">
        <b>--- <i>the Pro Contributors</i> ---</b>
        <%Html.RenderPartial("~/Areas/Site/views/shared/author.ascx", New AuthorInfoCard("davidh"))%>
       <%-- <uca:author ID="Author3" runat="server" Visible="true" Username="davidh" />--%>
    </div>
</asp:Content>

