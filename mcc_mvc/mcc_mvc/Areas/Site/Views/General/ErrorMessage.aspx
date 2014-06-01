<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Site/Views/Shared/TwoCol.master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="Content2" ContentPlaceHolderID="PageTitle" runat="server">
Error
</asp:Content>
<asp:Content ID="middlePlace" runat="server" ContentPlaceHolderID="middlePlaceHolder">
    <div class="mcc_ErrorMessageLog" style="overflow:hidden">
        <img src="<%= appHelpers.ImageUrl("error.png") %>" alt="Error" style="float:left;"/>
        <div>
            <%--<asp:Label ID="lblErrorMessage" runat="server"></asp:Label>--%>
            <p style="margin-bottom: 10px;">
                Sorry but we couldn't find the page you were looking for. This is probably one of
                the most annoying error you might encounter while browsing a website. To help you
                find what you were looking for, please check the url again and reload the page.
            </p>
            <p style="margin-bottom: 10px;">
                If you got onto this page after following one of our hyperlink, please signal us
                the dead link so that we can make the necessary modification to ensure a better
                browsing experience.
            </p>
            <p style="margin-bottom: 10px;">
                Last but not least, did you try our <a   href="/search/" class="global">
                    search</a> ?
            </p>
           <span>- <i>the MiddleClassCrunch Team</i> -</span>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="rightPlaceHolder">
    <div class="widget">
        <h2 class="title">More...
        </h2>
    </div>
</asp:Content>
