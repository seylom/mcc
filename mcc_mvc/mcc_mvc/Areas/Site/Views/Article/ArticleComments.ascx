<%@ Control Language="VB" Inherits="System.Web.Mvc.ViewUserControl(of ArticleCommentsViewModel)" %>

  <div class="artComments">
      <h5>
         Share your thoughts.<a name="comments"></a>
      </h5>
      <% For Each cm As ArticleComment In ViewData.Model.Comments%>
      <% Html.RenderPartial("~/Areas/Site/views/shared/articles/commentItem.ascx", cm)%>
      <% Next%>
      <%=Html.AdvancedPager(Model.Comments, "ArticleComments", "Article", New Integer() {10, 30, 50})%>
   </div>
   <% If Not UserIsAuthenticated() Then%>
   <div id="commentLoginReq">
      <div class="loginRequired">
         <a href="/login">login</a> or <a href="/signup">register</a> <span>to comment or discuss
            this article</span>
      </div>
   </div>
   <% Else%>
   <p style="margin: 10px 0">
      Post your comment</p>
   <div style="padding: 5px; margin: 5px 0">
      <%=Html.ValidationSummary("Unable to add comment. Please correct the errors and try again.")%>
   </div>
   <div id="post_comment" class="comment-pane">
      <% Using Html.BeginForm("PostComment", "Article", New With {.articleId = ViewData.Model.MainArticle.ArticleID})%>
      <div style="margin-bottom: 5px;">
         <%=Html.TextArea("commentBody", New With {.class = "rtc", .cols = "3", .rows = "5", .style = "width:100%"})%>
      </div>
      <div style="margin: 5px 0; padding: 5px">
         <%=Html.ValidationMessage("commentBody")%>
      </div>
      <input type="submit" value="" class="rb-btn rb-submit" onmouseover="this.className='rb-btn rb-submit-hover'"
         onmouseout="this.className='rb-btn rb-submit'" />
      <%End Using%>
   </div>
   <%End If%>