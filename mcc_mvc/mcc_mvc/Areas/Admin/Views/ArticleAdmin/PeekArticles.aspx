<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage(of articleViewModel)" %>

<asp:Content ID="Content3" ContentPlaceHolderID="PageTitle" runat="server">
Update Article Status
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="headerPlaceHolder" runat="Server">
   <%=appHelpers.AdvancedCssTagUrl("article.css", MCC.Utils.FileVersion("articlecss"))%>
   <%=appHelpers.AdvancedCssTagUrl("user_Article.css", MCC.Utils.FileVersion("user_articlecss"))%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainSiteContent" runat="Server">
   <div style="border-bottom: 1px solid #eaeaea; padding: 5px; margin-bottom: 10px;">
      <h1 class="admin-title">
         Article peek</h1>
   </div>
   <table width="100%">
      <tr>
         <td>
            <div style="background-color: #fff; border: 1px solid #bababa; padding: 10px;">
               <%Html.RenderPartial("~/Areas/Site/views/shared/articles/articleViewer.ascx", ViewData.Model)%>
            </div>
         </td>
         <td style="width: 40%;" valign="top">
            <div style="padding: 10px; background-color: #ffe4b5; border: 1px solid #fda377;">
               <h2 class="head-title">
                  Modifications Board.</h2>
               <div style="margin-top: 10px;">
                  <a href="<%=Url.Action("UpdateStatus","ArticleAdmin",new with {.Id = Model.Article.ArticleID,.status = cint(model.status.Accepted)}) %>">Accept</a>
                  <%--<a href="<%=Url.Action("UpdateStatus",new with {.Id = Model.ArticleID,.status = model.status.re}) %>">Reject</a>--%>&nbsp|&nbsp 
                  <a href="#">Edit</a>&nbsp|&nbsp
                  <a href="#<%=Url.Action("UpdateStatus","ArticleAdmin",new with {.Id = Model.Article.ArticleID,.status = cint(model.status.Verified)}) %>">Set as verified</a>&nbsp|&nbsp
                  <a href="<%=Url.Action("UpdateStatus","ArticleAdmin",new with {.Id =Model.Article.ArticleID,.status = cint(model.status.Rejected)}) %>">Reject</a>&nbsp|&nbsp
                  <a href="<%=Url.Action("UpdateStatus","ArticleAdmin",new with {.Id = Model.Article.ArticleID,.status = cint(model.status.Approved)}) %>">Approve</a>
               </div>
            </div>
         </td>
      </tr>
   </table>
</asp:Content>
