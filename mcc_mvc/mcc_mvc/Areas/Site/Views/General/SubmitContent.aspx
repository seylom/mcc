<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Site/Views/Shared/TwoCol.master"
   Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
   Submit Content
</asp:Content>
<asp:Content ContentPlaceHolderID="middlePlaceHolder" runat="server">
   <p>
      Send us your original tips, articles or experiences via the <a href="/contact" class="global">contact us</a> page
      along with your name or <a href="http://www.gravatar.com" class="global">gravatar</a>. The best submissions
      will be published and posted on Middle Class Crunch's homepage. Send us a short
      bio about you and your business, and we'll feature you as one of our trusted <a href="/about" class="global">MCC Pro Contributors!</a>
   </p>
   <br />
   <br />
   <p>
      In the meantime you can always <a class="global" href="/signup">register</a> and discuss MCC's
      material if you're not a member yet.
   </p>
</asp:Content>
<%--<%@ Register Assembly="mccControls" Namespace="MccControls" TagPrefix="mcc" %>

<asp:Content ID="Content3" ContentPlaceHolderID="middlePlaceHolder" runat="Server">
    <h2 class="head-title">
        Submit your content to MiddleClassCrunch</h2>
    <p style="margin: 10px 0;">
        Here at MiddleClassCrunch, we believe that people's voice should be heard whenever
        they have something to say.<br />
        If you would like to send us something you wrote please feel free to do so. we will
        be more than happy to read it, and if it is good, we will post it on the website.
        <a id="A1" href="~/" runat="server" class="global">Learn more</a>
    </p>
    <div id="commentLoginReq" runat="server">
        <asp:Panel ID="panLoginToSubmitContent" runat="server">
            <div class="loginRequired">
                <asp:HyperLink ID="lnkLoginRequired" runat="server" NavigateUrl="~/login">login</asp:HyperLink>
                or
                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/signup">register</asp:HyperLink>
                <span>to submit content.</span>
            </div>
        </asp:Panel>
    </div>
    
    <asp:Panel runat="server" ID="pnSubmitContent">
    <p>
        Enter information below and submit <b>articles</b> | <b>tips</b> | <b>stories</b>
        now.
    </p>
    <div id="validationResult" style="margin-top: 5px">
        <asp:ValidationSummary CssClass="regValidationSummary" ID="InsertValidationSummary"
            runat="server" ValidationGroup="ContactForm" />
    </div>
    <div style="margin-top: 20px; background-color: #f8f9f9;">
        <table id="ContactUsTable" style="width: 100%;">
            <tr>
                <td colspan="2" style="">
                    <asp:Label ID="lblSubject" runat="server" Text="Title:"></asp:Label>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtTitle"
                        Display="None" ErrorMessage="Please enter a Title" ValidationGroup="ContactForm"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:TextBox ID="txtTitle" CssClass="mtc" runat="server" ValidationGroup="ContactForm"
                        Width="99%"></asp:TextBox>
                </td>
            </tr>
        </table>
        <table style="width: 100%">
            <tr>
                <td>
                    <asp:Label ID="lblBody" runat="server" Text="Body"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="txtBody" runat="server" CssClass="mtac" TextMode="MultiLine"
                        Rows="10" Width="99%"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvBody" runat="server" ControlToValidate="txtBody"
                        Display="None" ErrorMessage="Please enter the body of the post" ValidationGroup="ContactForm"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        - Additional Comments ...
                    </div>
                    <asp:TextBox ID="txtComments" runat="server" CssClass="mtac" TextMode="MultiLine"
                        Rows="4" Width="99%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <mcc:mccCaptchaCtrl ID="MccCaptchaCtrl1" runat="server" CaptchaHeight="40" CaptchaWidth="200"
                        CssClass="captcha" Text="Enter image characters below." TextboxClass="mtc" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ValidationGroup="ContactForm" ID="btnSubmit" runat="server" CssClass="rb-btn rb-submit"
                        CausesValidation="true" onmouseover="this.className='rb-btn rb-submit-hover'"
                        onmouseout="this.className='rb-btn rb-submit'" />
                </td>
            </tr>
        </table>
        <asp:Literal ID="lblConfirm" runat="server" Visible="false" Text="Your mail was submitted succesfully!<br/><br/>We will contact you shortly as a follow up to your mail. Thank you!<br/>the MiddleClassCrunch Team."></asp:Literal>
        <asp:Literal ID="lblError" runat="server" Visible="false" Text="We are unable to process your request at this moment, sorry for the inconvenience. Please try again later. <br/>the MiddleClassCrunch Team."></asp:Literal>
    </div>
    </asp:Panel>
</asp:Content>
--%>