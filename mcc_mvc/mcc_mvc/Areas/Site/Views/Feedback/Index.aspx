<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Site/Views/Shared/TwoCol.master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="middlePlaceHolder" runat="Server">
    <h2 class="head-title">
        You talk, we listen !</h2>
    <p style="margin: 10px 0;">
        We would love to hear from you in order to improve our website. Feel free to submit
        your point of view, let us know what we can do better.
    </p>
    <p>
        <a id="A1" href="/feedback/submitfeedback"  class="global">Submit</a> your
        feedback anytime.
    </p>
    <div style="padding: 20px; border-bottom: 1px solid #b35433; background-color: #e7effc;
        margin: 10px 0;">
            <table>
                <tr>
                    <td>
                        <span><b>... or search for feedbacks...</b> </span>
                    </td>
                    <td>
                        <input type="text" value="" id="key" class="search_field" />
                    </td>
                    <td>
                        <input type="submit" class="rb-btn rb-search" value="" title="" onmouseover="this.className='rb-btn rb-search-hover'"
                            onmouseout="this.className='rb-btn rb-search'" />
                    </td>
                </tr>
            </table>
    </div>
    <%--<asp:ListView runat="server" ID="lvFeedbacks" DataSourceID="objFeedbacks" Style="margin-top: 20px;">
        <EmptyDataTemplate>
            <div style="margin: 5px 0; padding: 20px; background-color: #fff; border: #bababa; color: #a5370a">
                <center>
                    <b>
                        <%=IIf(txtSearch.Text.Length = 0, "Be the first to submit a feedback!", "No result for - " & txtSearch.Text & "-")%></b>
                </center>
            </div>
        </EmptyDataTemplate>
        <LayoutTemplate>
            <div class="PagerContainer">
                Page <span></span>
                <asp:DataPager ID="topDataPager" runat="server" PageSize="10" PagedControlID="lvFeedbacks">
                    <Fields>
                        <asp:NumericPagerField CurrentPageLabelCssClass="mcc-pager-current" NextPreviousButtonCssClass="mcc-pager-previous-next"
                            ButtonType="Link" NumericButtonCssClass="mcc-pager" PreviousPageText="..." NextPageText="..."
                            ButtonCount="3" />
                        <asp:NextPreviousPagerField ButtonCssClass="mcc-pager-previous-next" NextPageText="›"
                            LastPageText="&raquo;" RenderDisabledButtonsAsLabels="true" ShowFirstPageButton="false"
                            ShowPreviousPageButton="false" ShowLastPageButton="true" ShowNextPageButton="true" />
                    </Fields>
                </asp:DataPager>
            </div>
            <ol id="feedbacks">
                <li id="itemPlaceholder" runat="server"></li>
            </ol>
        </LayoutTemplate>
        <ItemTemplate>
            <li class='fb'>
                <div class="vote">
                    <center>
                        <div style="padding: 5px;">
                            <asp:ImageButton ID="btnApprove" runat="server" ImageUrl="~/_assets/images/MicroIcons/ok_img.gif"
                                AlternateText="Approve article" CausesValidation="false" ToolTip="Rate it" CommandName="Rate"
                                CommandArgument='<%# Eval("FeedbackId") %>'></asp:ImageButton>
                        </div>
                        <asp:Label ID="Label1" runat="server" Font-Size="12px" Text='<%#Eval("Votes") %>' Font-Bold="true"></asp:Label>
                        <asp:Label ID="Label2" runat="server" Text='<%# IIf(Eval("Votes") < 1, " vote", " votes")%>'></asp:Label>
                    </center>
                </div>
                <div class="feed">
                    <h4 class="feedslnk">
                        <asp:HyperLink runat="server" ID="lnkTitle" Text='<%#MCC.Routines.Encode(Eval("Title")) %>' NavigateUrl='<%# "~/feedback?id="& Eval("FeedbackId") %>'></asp:HyperLink></h4>
                    <p style="margin: 5px 0;">
                        <asp:Literal runat="server" ID="ltBody" Text='<%# MCC.routines.Encode(Eval("Description")) %>'></asp:Literal>
                    </p>
                </div>
                <br class="clearer" />
            </li>
        </ItemTemplate>
    </asp:ListView>--%>
</asp:Content>


