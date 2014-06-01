<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Site/Views/Shared/TwoCol.master" Inherits="System.Web.Mvc.ViewPage(of ArticleByCategoryViewModel)" %>


<%@ Import Namespace="MCC.UserAccountRoutines" %>
<asp:Content ContentPlaceHolderID="PageTitle" runat="server">
<%= Model.Category.Title %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="headItems" runat="server">
   <%=appHelpers.AdvancedCssTagUrl("article.css", MCC.Utils.FileVersion("articlecss"))%>
</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="middlePlaceHolder" runat="Server">

   <% If (Model.Articles Is Nothing OrElse Model.Articles.Count = 0) Then%>
   <div style="margin: 5px 0; padding: 20px; background-color: #fff;">
      <center>
         <b>No article found for the specified topic, please try again later!</b>
      </center>
   </div>
   <%Else%>
      <div id="descriptionBox"  style="background-color: #c7dfe8;
      padding: 10px; margin-bottom: 5px;">
      <h3 class="artTitle">
         <%=Html.Encode(Model.Category.Title)%></h3>
      <p style="padding-top: 10px;">
         <%=Html.Encode(ViewData.Model.Category.Description)%>
      </p>
   </div>
   <% For Each it As Article In ViewData.Model.Articles%>
   <% Html.RenderPartial("~/Areas/Site/views/shared/articles/articleItem.ascx", it)%>
   <% Next%>
    <%=Html.AdvancedPager(Model.Articles, "ArticlesByCategory", "Article", New Integer() {10, 30, 50})%>
   <% End If%>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="rightPlaceHolder">
   <div class="wg">
      <div class="widget">
         <h3 class="title">
            Topics</h3>
         <ul class="category-menu">
            <li class="bullet" onmouseover="this.className='bullet_hover'" onmouseout="this.className='bullet'">
               <a href="/articles/" title='<%= "All" %>'>
               All
               </a> </li>
            <%For Each ca As ArticleCategory In ViewData.Model.Categories%>
            <li class="bullet" onmouseover="this.className='bullet_hover'" onmouseout="this.className='bullet'">
               <a href='<%= "/Articles/Topics/" & ca.Slug  & "/" %>' title='<%= Html.Encode(ca.Title) %>'>
               <%=Html.Encode(ca.Title)%>
               </a> </li>
            <%Next%>
         </ul>
      </div>
      <div class="widget">
         <h3 class="title">
            Recent Posts</h3>
         <ul class="category-menu">
            <%For Each it As Article In ViewData.Model.RecentArticles%>
            <li><a href='<%= url.Action("ShowArticle",new with {.id=it.ArticleID,.slug = it.Slug}) %>' title='<%=Html.Encode(it.Title) %>'>
               <%=Html.Encode(it.Title)%></a> </li>
            <%Next%>
         </ul>
      </div>
      <div class="adwidget">
         <% Html.RenderPartial("~/Areas/Site/views/shared/ads/adLargeSquareGoogle.ascx")%>
      </div>
   </div>
</asp:Content>
