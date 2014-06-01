<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Site/Views/Shared/TwoCol.master" Inherits="System.Web.Mvc.ViewPage(of ArticlesFormViewModel)" %>

<%@ Import Namespace="MCC.UserAccountRoutines" %>
<asp:Content ID="Content2" ContentPlaceHolderID="PageTitle" runat="server">
   Articles
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="headItems" runat="server">
   <%=appHelpers.AdvancedCssTagUrl("article.css", MCC.Utils.FileVersion("articlecss"))%>
</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="middlePlaceHolder" runat="Server">
   <% If ViewData.Model.Articles.Count = 0 Then%>
   <div style="margin: 5px 0; padding: 20px; background-color: #fff;">
      <center>
         <b>No article found . Please try again later!</b>
      </center>
   </div>
   <%Else%>
   <% For Each ar As Article In Model.Articles%>
   <% Html.RenderPartial("~/Areas/Site/views/shared/articles/articleItem.ascx", ar)%>
   <% Next%>
    <%=Html.AdvancedPager(Model.Articles, "Index", "Article", New Integer() {10, 30, 50})%>
   <%End If%>
</asp:Content>
<asp:Content ID="sideContent" runat="server" ContentPlaceHolderID="rightPlaceHolder">
   <div class="wg">
      <div class="widget">
         <h3 class="title">
            Topics</h3>
         <ul class="category-menu">
            <li class="bullet" onmouseover="this.className='bullet_hover'" onmouseout="this.className='bullet'">
               <a href="/Articles/" title='<%= "All" & "(" &  ViewDaTa.Model.TotalArticlesCount  & ")" %>'>
                  All </a></li>
            <%For Each ca As ArticleCategory In Model.Categories%>
            <li class="bullet" onmouseover="this.className='bullet_hover'" onmouseout="this.className='bullet'">
               <a href='<%= "/Articles/Topics/" & ca.Slug  & "/" %>' title='<%= Html.Encode(ca.Title)  %>'>
                  <%=Html.Encode(ca.Title)%>
               </a></li>
            <%Next%>
         </ul>
      </div>
      <div class="widget">
         <h3 class="title">
            Recent Posts</h3>
         <ul class="category-menu">
            <%For Each it As Article In Model.RecentArticles%>
            <li><a href='<%=url.Action("ShowArticle",new with {.Id= it.ArticleID, .slug = it.Slug}) %>' title='<%=Html.Encode(it.Title) %>'>
               <%=Html.Encode(it.Title)%></a> </li>
            <%Next%>
         </ul>
      </div>
      <div class="adwidget">
        <% Html.RenderPartial("~/Areas/Site/views/shared/ads/adLargeSquareGoogle.ascx")%>
      </div>
   </div>
</asp:Content>
