<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage(of AdminDashboardViewData)" %>

<asp:Content ID="Content2" ContentPlaceHolderID="PageTitle" runat="server">
   Welcome to the admin Section
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="mainSiteContent" runat="Server">
   <div style="border-bottom: 1px solid #eaeaea; padding: 5px; margin-bottom: 10px;">
      <h1 class="admin-title">
         Administration Dashboard</h1>
   </div>
   <div style="overflow: hidden;">
      <div class="dashbk dsh-nt" style="background-color: #fdfce6">
         <div class="dkhd">
            <span class="dash-hd">notifications</span>
         </div>
      </div>
      <div class="dashbk dsh-cm">
         <div class="dkhd">
            <span class="dash-hd">Recent Comments</span>
         </div>
         <div class="dashcnt">
            <% For Each it As ArticleComment In Model.Comments%>
            <div style="margin: 5px 0; background-color: #eaeaea; padding: 3px;">
               <%= If(it.Body.Length > 60, it.Body.Substring(0, 60) & "...", it.Body)%>
               <div style="color: #2e6373;">
                  <span style="font-size: 8px;">-- by
                     <%= it.AddedBy %></span>
                  <%--  <span>--- in this</span> <a href="<%= Url.Action("ShowArticle","Article",new with {.id = it.ArticleID}) %>">article</a>--%>
               </div>
            </div>
            <% Next%>
         </div>
      </div>
      <div class="dashbk dsh-us">
         <div class="dkhd">
            <span class="dash-hd">Recent users</span>
         </div>
      </div>
      <div class="dashbk dsh-ar">
         <div class="dkhd">
            <span class="dash-hd">Recent Articles</span>
         </div>
         <div class="dashcnt">
            <table class="edit-info">
               <tbody>
                  <% Dim idx As Integer = 0%>
                  <% For Each it As Article In Model.Articles%>
                  <tr class='<%= IIF(idx Mod 2 = 0,"even", "odd") %>'>
                     <td>
                        <a class="lnk" href="<%= Url.Action("ShowArticle","Article",new with {.id = it.ArticleID}) %>"
                           title="<%:it.title %>">
                           <%: it.Title%></a>
                     </td>
                  </tr>
                  <% idx += 1%>
                  <% Next%>
               </tbody>
            </table>
         </div>
      </div>
      <div class="dashbk dsh-qu">
         <div class="dkhd">
            <span class="dash-hd">Recent user questions</span>
         </div>
         <div class="dashcnt">
                     <table class="edit-info">
               <tbody>
                  <% Dim idx2 As Integer = 0%>
                  <% For Each it As UserQuestion In Model.Questions%>
                  <tr class='<%= IIF(idx2 Mod 2 = 0,"even", "odd") %>'>
                     <td>
                    
                        <a class="lnk" href="<%= Url.Action("viewquestion","Ask",new with {.id = it.UserQuestionID}) %>"
                           title="<%:it.title %>">
                           <%: it.Title%></a>
                     </td>
                  </tr>
                  <% idx2 += 1%>
                  <% Next%>
               </tbody>
            </table>

           <%-- <% For Each it As UserQuestion In Model.Questions%>
            <div style="margin: 5px 0">
               <h2 style="font-size: 12px;">
                  <a href="#" title="">
                     <%: it.Title%></a></h2>
            </div>
            <% Next%>--%>
         </div>
      </div>
   </div>
</asp:Content>
