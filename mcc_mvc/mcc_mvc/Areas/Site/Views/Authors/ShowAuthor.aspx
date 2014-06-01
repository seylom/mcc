<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Site/Views/Shared/TwoCol.master" Inherits="System.Web.Mvc.ViewPage(of ArticlesFormViewModel)" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headItems" runat="server">
   <%=appHelpers.AdvancedCssTagUrl("article.css", MCC.Utils.FileVersion("articlecss"))%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageTitle" runat="server">
   <%=Html.Encode(Model.PageTitle)%>
</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="middlePlaceHolder" runat="Server">
   
  <%Html.RenderPartial("~/Areas/Site/views/shared/aboutauthor.ascx", New AuthorInfoCard(Model.Author))%>
   
   
   <div style="margin: 20px 0;">
      <h4 style="font-size: 12px;">
         Articles by this author: (<%=Model.TotalArticlesCount%>)</h4>
   </div>
   
   
   <% If ViewData.Model.Articles.Count = 0 Then%>
   <div style="margin: 5px 0; padding: 20px; background-color: #fff;">
      <center>
         <b>No article found . Please try again later!</b>
      </center>
   </div>
   <%Else%>
   <%--<div class="PagerContainer">
      Page
      <asp:DataPager ID="bottomDataPager" runat="server" PageSize="12" PagedControlID="lvArticles"
         QueryStringField="page" EnableViewState="false">
         <Fields>
            <mcc_controls:AdvancedPagerField ButtonCssClass="button" NextPageImageUrl="http://cpstf.com/_assets/images/button_arrow_right.gif"
               PreviousPageImageUrl="http://cpstf.com/_assets/images/button_arrow_left.gif" />
         </Fields>
      </asp:DataPager>
   </div>--%>
   <% For Each ar As Article In Model.Articles%>
   <% Html.RenderPartial("~/Areas/Site/views/shared/articles/articleItem.ascx", ar)%>
   <% Next%>
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
            <li><a href='<%= "/Articles/" & it.Slug %>' title='<%=Html.Encode(it.Title) %>'>
               <%=Html.Encode(it.Title)%></a> </li>
            <%Next%>
         </ul>
      </div>
      <div class="adwidget">
         <div style="height: 100px;">
         </div>
         <div style="font-size: 10px; text-align: center;">
            sponsor ads
         </div>
      </div>
   </div>
</asp:Content>