<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage(of adminTipsViewModel)" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
Tips
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainSiteContent" runat="Server">
   <div style="border-bottom: 1px solid #eaeaea; padding: 5px; margin-bottom: 10px;">
      <h1 class="admin-title">
         Tips</h1>
   </div>
   <% If Model.Tips Is Nothing Or Model.Tips.Count = 0 Then%>
   <center>
      <b>- No Advices are currently in the database -</b>
   </center>
   <% Else%>
    <%=Html.AdvancedPager(Model.Tips, "Index", "TipAdmin", New Integer() {10, 30, 50})%>
   <table class="edit-info">
      <col align="center" />
      <col align="left" />
      <col align="right" />
      <col align="center" />
      <col align="center" />
      <thead>
         <tr>
            <th colspan="2">
               Title
            </th>
            <th>
               Author
            </th>
            <th>
            </th>
         </tr>
      </thead>
      <tbody>
         <% Dim idx As Integer = 0%>
         <% For Each it As Advice In Model.Tips%>
         <tr class='<%= IIF(idx Mod 2 = 0,"even", "odd") %>'>
            <td style="width: 30px;">
               <%=idx & "."%>
            </td>
            <td>
               <a class="lnk" title='<%= Html.Encode(it.Title ) %>' href='<%= "/Tips/" & it.slug %>'><%= Html.Encode(it.Title ) %></a>
               <% If Not it.Approved Then%>
               <span style="font-size: 9px; font-style: italic">not approved</span>
               <% End If%>
            </td>
            <td>
               <%=it.AddedBy%>
            </td>
            <td style="width:80px;">
               <a href='<%= Url.Action("EditTip",new with {.Id = it.AdviceId}) %>' title="Edit article" class="lnk">
                  Edit</a> &nbsp;|&nbsp;<%=Html.ActionLink("Delete", "DeleteTip", New With {.Id = it.AdviceID}, New With {.class = "lnk"})%>
            </td>
         </tr>
         <% idx += 1%>
         <% Next%>
      </tbody>
   </table>
    <%=Html.AdvancedPager(Model.Tips, "Index", "TipAdmin", New Integer() {10, 30, 50})%>
   <% End If%>
   
</asp:Content>
