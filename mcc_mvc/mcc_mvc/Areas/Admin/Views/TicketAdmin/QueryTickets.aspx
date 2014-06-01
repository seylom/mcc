<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="MCC.Tickets" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
Query Tickets
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainSiteContent" runat="Server">
    <asp:HiddenField runat="server" ID="_params" />
    <asp:HiddenField runat="server" ID="_command" Value="" />
    <div style="border-bottom: 1px solid #eaeaea; padding: 5px; margin-bottom: 10px;">
        <h1 class="admin-title">
            Advanced Tickets Filters</h1>
    </div>
    <div style="margin: 5px 0;">
        <fieldset id="options_field">
            <legend><a href="javascript:void(0);" onclick="ToggleFilters();return false;" class="global">
                Filters</a></legend>
            <table id="filters">
                <tr>
                    <td class="right" style="width:100px;">
                        Status
                    </td>
                    <td style="width: 70px;">
                        <asp:DropDownList runat="server" ID="ddlStatusChoice">
                            <asp:ListItem Text="is" Value="4" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="is not" Value="5"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:CheckBoxList ID="cbStatus" runat="server" CssClass="checkboxlist" RepeatDirection="Horizontal">
                            <asp:ListItem Text="new" Value="0">
                            </asp:ListItem>
                            <asp:ListItem Text="assigned" Value="1">
                            </asp:ListItem>
                            <asp:ListItem Text="resolved" Value="2">
                            </asp:ListItem>
                              <asp:ListItem Text="verified" Value="4">
                            </asp:ListItem>
                              <asp:ListItem Text="closed" Value="5">
                            </asp:ListItem>
                            <asp:ListItem Text="reopened" Value="3">
                            </asp:ListItem>
                        </asp:CheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td class="right">
                        Priority
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlPriorityChoice">
                            <asp:ListItem Text="is" Value="4" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="is not" Value="5"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:CheckBoxList ID="cbPriority" runat="server" CssClass="checkboxlist" RepeatDirection="Horizontal">
                            <asp:ListItem Text="low" Value="0">
                            </asp:ListItem>
                            <asp:ListItem Text="normal" Value="1">
                            </asp:ListItem>
                            <asp:ListItem Text="high" Value="2">
                            </asp:ListItem>
                        </asp:CheckBoxList>
                    </td>
                </tr>
                 <tr>
                    <td class="right">
                        Type
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlTypeChoice">
                            <asp:ListItem Text="is" Value="4" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="is not" Value="5"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:CheckBoxList ID="cbType" runat="server" CssClass="checkboxlist" RepeatDirection="Horizontal">
                            <asp:ListItem Text="Defect" Value="0">
                            </asp:ListItem>
                            <asp:ListItem Text="Enhancement" Value="1">
                            </asp:ListItem>
                            <asp:ListItem Text="Task" Value="2">
                            </asp:ListItem>
                        </asp:CheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td class="right">
                        Owner
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlOwnerChoice">
                            <asp:ListItem Text="is" Value="4" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="is not" Value="5"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlOwners" AppendDataBoundItems="true" >
                            <asp:ListItem Text="" Value="" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="unassigned" Value="unassigned"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="right">
                        Title
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlTitleChoice">
                            <asp:ListItem Text="contains" Value="0"></asp:ListItem>
                            <asp:ListItem Text="does NOT contain" Value="1"></asp:ListItem>
                            <asp:ListItem Text="starts with" Value="2"></asp:ListItem>
                            <asp:ListItem Text="ends with" Value="3"></asp:ListItem>
                            <asp:ListItem Text="is" Value="4"></asp:ListItem>
                            <asp:ListItem Text="is not" Value="5"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtTitleMatch" Width="400"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="right">
                        Description
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlDescriptionChoice">
                            <asp:ListItem Text="contains" Value="0" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="does NOT contain" Value="1"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtDescriptionMatch" Width="400"></asp:TextBox>
                    </td>
                </tr>
                <td class="right">
                    Keywords
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlKeywordsChoice">
                        <asp:ListItem Text="contains" Value="0" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="does NOT contain" Value="1"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="TextBox1" Width="400"></asp:TextBox>
                </td>
                <tr>
                    <td class="right" colspan="2">
                    </td>
                    <td>
                        <asp:Button ID="btnUpdatePage" runat="server" CssClass="rb-btn rb-update" onmouseover="this.className='rb-btn rb-update-hover'"
                            ValidationGroup="Update" onmouseout="this.className='rb-btn rb-update'" PostBackUrl="~/admin/Tickets/ticketsQuery.aspx" />
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    <asp:ListView ID="lvTickets" runat="server" DataSourceID="objTickets">
        <LayoutTemplate>
            <div class="PagerContainer">
                Page <span></span>
                <asp:DataPager ID="DataPager1" runat="server" PageSize="30" PagedControlID="lvTickets"
                    EnableViewState="false">
                    <%--<Fields>
                        <asp:NumericPagerField CurrentPageLabelCssClass="mcc-pager-current" NextPreviousButtonCssClass="mcc-pager-previous-next"
                            ButtonType="Link" NumericButtonCssClass="mcc-pager" PreviousPageText="..." NextPageText="..."
                            ButtonCount="3" />
                        <asp:NextPreviousPagerField ButtonCssClass="mcc-pager-previous-next" NextPageText="›"
                            LastPageText="&raquo;" RenderDisabledButtonsAsLabels="true" ShowFirstPageButton="false"
                            ShowPreviousPageButton="false" ShowLastPageButton="true" ShowNextPageButton="true" />
                    </Fields>--%>
                       <Fields>
                        <mcc_controls:AdvancedPagerField ButtonCssClass="button" NextPageImageUrl="~/_assets/images/button_arrow_right.gif"
                            PreviousPageImageUrl="~/_assets/images/button_arrow_left.gif" />
                    </Fields>
                </asp:DataPager>
            </div>
            <table class="edit-info">
                <col align="center" />
                <col align="left" />
                <col align="right" />
                <col align="center" />
                <col align="right" />
                <col align="right" />
                <col align="center" />
                <col align="center" />
                <col align="center" />
                <tr>
                    <th>
                        <asp:LinkButton ID="lnkTicket" runat="server" CssClass="lnkh" CommandName="Sort"
                            CommandArgument="TicketId" Text="Ticket"></asp:LinkButton>
                    </th>
                    <th>
                        <asp:LinkButton ID="lnkTitle" runat="server" CssClass="lnkh" CommandName="Sort" CommandArgument="Title"
                            Text="Title"></asp:LinkButton>
                    </th>
                    <th>
                        <asp:LinkButton ID="lnkType" runat="server" CssClass="lnkh" CommandName="Sort" CommandArgument="Type"
                            Text="Type"></asp:LinkButton>
                    </th>
                    <th>
                        <asp:LinkButton ID="lnkStatus" runat="server" CssClass="lnkh" CommandName="Sort"
                            CommandArgument="Status" Text="Status"></asp:LinkButton>
                    </th>
                    <th>
                        <asp:LinkButton ID="lnkPriority" runat="server" CssClass="lnkh" CommandName="Sort"
                            CommandArgument="Priority" Text="Priority"></asp:LinkButton>
                    </th>
                    <th>
                        <asp:LinkButton ID="lnkAddedBy" runat="server" CssClass="lnkh" CommandName="Sort"
                            CommandArgument="AddedBy" Text="Reporter"></asp:LinkButton>
                    </th>
                    <th>
                        <asp:LinkButton ID="lnkOwner" runat="server" CssClass="lnkh" CommandName="Sort" CommandArgument="Owner"
                            Text="Owner"></asp:LinkButton>
                    </th>
                    <th>
                        <asp:LinkButton ID="lnkaddeddate" runat="server" CssClass="lnkh" CommandName="Sort"
                            CommandArgument="AddedDate" Text="Created"></asp:LinkButton>
                    </th>
                    <th>
                        Command
                    </th>
                </tr>
                <tr id="itemPlaceholder" runat="server">
                </tr>
            </table>
            <div class="PagerContainer">
                Page <span></span>
                <asp:DataPager ID="bottomDataPager" runat="server" PageSize="30" PagedControlID="lvTickets"
                    EnableViewState="false">
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
        </LayoutTemplate>
        <ItemTemplate>
            <tr class='<%# "color" & Eval("Priority").ToString() & IIF(Container.DataItemIndex Mod 2 = 0,"_odd","_even")  %>'>
                <td>
                    <asp:HyperLink CssClass="lnk" runat="server" ID="lnkTicket" Text='<%# "#" & Eval("TicketId").ToString()%>'
                        NavigateUrl='<%#"~/admin/tickets/ViewTicket.aspx?id=" & Eval("TicketId").ToString() %>'></asp:HyperLink>
                </td>
                <td>
                    <asp:HyperLink CssClass="lnk" runat="server" ID="lnkTitle" Text='<%# MCC.Routines.Encode(Eval("Title")) %>'
                        NavigateUrl='<%#"~/admin/tickets/ViewTicket.aspx?id=" & Eval("TicketId").ToString() %>'></asp:HyperLink>
                </td>
                <td>
                    <%#TicketRepository.GetTypeCaption(Eval("Type"))%>
                </td>
                <td>
                    <%#TicketRepository.GetStatusCaption(Eval("Status"))%>
                </td>
                <td>
                    <%#TicketRepository.GetPriorityCaption(Eval("Priority"))%>
                </td>
                <td>
                    <asp:Label ID="Label1" runat="server" Text='<%#Eval("AddedBy").ToString()%>'></asp:Label>
                </td>
                <td>
                    <%#Eval("Owner")%>
                </td>
                <td>
                    <%#Eval("AddedDate", "{0:MMM d yyyy}")%>
                </td>
                <td>
                    <asp:HyperLink runat="server" ID="lnkEdit" NavigateUrl='<%#"~/admin/tickets/addEditTicket.aspx?id=" & Eval("TicketId") %>'
                        ImageUrl="~/_assets/images/MicroIcons/edit.png" ToolTip="Edit Ticket"></asp:HyperLink>
                    <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="~/_assets/images/MicroIcons/delete.png"
                        CausesValidation="false" ToolTip="Delete Ticket" AlternateText="Delete article"
                        OnClientClick="if (confirm('Are you sure you want to delete this article?') == false) return false;"
                        CommandArgument='<%# Eval("TicketId") %>' CommandName="Delete"></asp:ImageButton>
                </td>
            </tr>
        </ItemTemplate>
        <EmptyDataTemplate>
            <div style="padding: 10px; background-color: #eaeaea;">
                <center>
                    No ticket found
                </center>
            </div>
        </EmptyDataTemplate>
    </asp:ListView>
    <asp:ObjectDataSource runat="server" ID="objTickets" SelectMethod="GetTicketsByCommand"
        TypeName="MCC.Tickets.TicketRepository" InsertMethod="InsertTicket" DeleteMethod="DeleteTicket"
        UpdateMethod="UpdateTickets">
        <UpdateParameters>
            <asp:Parameter Name="TicketId" Type="Int32" />
            <asp:Parameter Name="title" Type="String" />
            <asp:Parameter Name="description" Type="String" />
            <asp:Parameter Name="type" Type="Int32" />
            <asp:Parameter Name="owner" Type="String" />
            <asp:Parameter Name="priority" Type="Int32" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="title" Type="String" />
            <asp:Parameter Name="description" Type="String" />
            <asp:Parameter Name="type" Type="Int32" />
            <asp:Parameter Name="owner" Type="String" />
            <asp:Parameter Name="priority" Type="Int32" />
        </InsertParameters>
        <DeleteParameters>
            <asp:Parameter Name="TicketId" Type="Int32" />
        </DeleteParameters>
        <SelectParameters>
            <asp:ControlParameter Name="command" Type="String" DefaultValue="" ControlID="_command" />
            <asp:ControlParameter Name="params" Type="Object" ControlID="_params" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottomScripts" runat="Server">

    <script type="text/javascript">
        function ToggleFilters() {
            $("#options_field").toggleClass("collapsed")
        }
    </script>

</asp:Content>
