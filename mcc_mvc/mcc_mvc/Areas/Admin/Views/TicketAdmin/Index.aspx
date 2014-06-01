<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage(of adminTicketsViewModel)" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
   Tickets - Issues
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainSiteContent" runat="Server">
   <div style="border-bottom: 1px solid #eaeaea; padding: 5px; margin-bottom: 10px;">
      <h1 class="admin-title">
         Tickets</h1>
   </div>
   <div style="padding: 5px; background-color: #eaeaea; margin: 5px 0;">
      <span>Status: </span><a href="?status=all" class="global">All (<%=Model.AllCount%>)</a>&nbsp;|&nbsp;
      <a id="A2" href="?status=mytickets" class="global">My Tickets (<%=Model.AllCount%>)</a>&nbsp;|&nbsp;
      <a id="A3" href="?status=new" class="global">New (<%=Model.MyTicketsCount%>)</a>&nbsp;|&nbsp;
      <a id="A4" href="?status=assigned" class="global">Assigned (<%=Model.AssignedCount%>)</a>&nbsp;|&nbsp;<a
         id="A7" href="?status=resolved" class="global">Resolved (<%=Model.ResolvedCount%>)</a>&nbsp;|&nbsp;<a
            id="A8" href="?status=verified" class="global">Verified (<%=Model.VerifiedCount%>)</a>&nbsp;|&nbsp;
      <a id="A5" href="?status=closed" class="global">Closed (<%=Model.ClosedCount%>)</a>&nbsp;|&nbsp;<a
         id="A6" href="?status=reopened" class="global"> Reopened (<%=Model.ReopenedCount%>)</a>&nbsp;|&nbsp;<a
            id="A1" href="/admin/tickets/tickets_query" class="globalred">advanced filters</a>
   </div>
   <% If Model.Tickets Is Nothing Or Model.Tickets.Count = 0 Then%>
   <div style="padding: 10px; background-color: #eaeaea;">
      <center>
         No ticket found
      </center>
   </div>
   <% Else%>
   <%=Html.AdvancedPager(Model.Tickets, "Index", "TicketAdmin", New Integer() {10, 30, 50})%>
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
      <col align="center" />
      <thead>
         <tr>
            <th>
               Ticket
            </th>
            <th>
               Title
            </th>
            <th>
               Type
            </th>
            <th>
               Status
            </th>
            <th>
               Priority
            </th>
            <th>
               AddedBy
            </th>
            <th>
               Owner
            </th>
            <th>
               AddedDate
            </th>
            <th>
            </th>
            <th>
            </th>
         </tr>
      </thead>
      <tbody>
         <% Dim idx As Integer = 0%>
         <%For Each it As Ticket In Model.Tickets%>
         <tr class='<%= "color" & it.Priority  & IIF(idx Mod 2 = 0,"_odd","_even")  %>'>
            <td>
               <a class="lnk" title='<%= "#" & it.TicketId %>' href='<%=url.action("ShowTicket",new with {.Id = it.TicketID}) %>'>
                  <%=it.TicketID%>
               </a>
            </td>
            <td>
               <a class="lnk" title='<%= html.Encode(it.Title ) %>' href='<%=url.action("ShowTicket",new with {.Id = it.TicketID}) %>'>
                  <%= html.Encode(it.Title ) %>
               </a>
            </td>
            <td>
               <%=it.GetTypeCaption()%>
            </td>
            <td>
               <%=it.GetStatusCaption()%>
            </td>
            <td>
               <%it.GetPriorityCaption()%>
            </td>
            <td>
               <%=it.AddedBy %>
            </td>
            <td>
               <%=it.Owner%>
            </td>
            <td>
               <%=it.AddedDate.ToString("MMM dd yyyy")%>
            </td>
            <td>
               <a href="<%=url.Action("EditTicket","TicketAdmin", new with {.Id = it.TicketId})%>"
                  title="Edit Ticket" class="lnk">Edit</a>
            </td>
            <td>
               <% Using Html.BeginForm("DeleteTicket", "TicketAdmin", New With {.Id = it.TicketID})%>
               <input type="image" src="<%= apphelpers.ImageUrl("MicroIcons/delete.png") %>" />
               <% End Using%>
            </td>
         </tr>
         <% idx += 1%>
         <%Next%>
      </tbody>
   </table>
   <% End If%>
    <%=Html.AdvancedPager(Model.Tickets, "Index", "TicketAdmin", New Integer() {10, 30, 50})%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottomScripts" runat="Server">

   <script type="text/javascript">
      function ToggleFilters() {
         $("#options_field").toggleClass("collapsed")
      }
   </script>

</asp:Content>
