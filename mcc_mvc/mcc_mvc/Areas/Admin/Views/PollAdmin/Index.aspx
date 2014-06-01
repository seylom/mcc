<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage(of adminPollsViewModel)" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
Polls
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainSiteContent" runat="Server">
   <div style="border-bottom: 1px solid #eaeaea; padding: 5px; margin-bottom: 10px;">
      <h1 class="admin-title">
         Polls</h1>
   </div>
   <table class="edit-info">
      <col align="center" />
      <col align="left" />
      <col align="right" />
      <col align="center" />
      <col align="center" />
      <thead>
         <tr>
            <th colspan="2">
               Question
            </th>
            <th>
               Author
            </th>
            <th>
               Status
            </th>
            <th>
               Command
            </th>
         </tr>
      </thead>
      <tbody>
         <%Dim idx As Integer = 0%>
         <% For Each it As Poll In Model.Polls%>
         <tr class='<%= IIF(idx Mod 2 = 0,"even", "odd") %>'>
            <td>
               <%=idx & "."%>
            </td>
            <td>
               <a class="lnk" href='<%="/admin/polls/viewpoll.aspx?id=" & it.PollId %>' tooltip='<%= it.QuestionText %>'>
                  <%= it.QuestionText %></a>
            </td>
            <td>
               <%=it.AddedBy%>
            </td>
            <td>
               <span id="Span2" class='<%= "notap st_" &it.isArchived  %>'>
                  <%=Utils.GetStatusCaption(it.isArchived)%></span>
            </td>
            <td>
               <a title="Edit Poll">Edit Poll</a> <a title="Delete Poll" href="javascript:void(0);"
                  onclick="if (confirm('Are you sure you want to delete this Poll?') == false) return false;">
               </a>
            </td>
         </tr>
         <% idx += 1%>
         <% Next%>
      </tbody>
   </table>
    <%=Html.AdvancedPager(Model.Polls, "Index", "PollAdmin", New Integer() {10, 30, 50})%>
   </div>
</asp:Content>
