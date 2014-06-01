<%@ Page Language="VB" Inherits="System.Web.Mvc.ViewPage(of article)" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <title>Print</title>
  <%=appHelpers.CssTagUrl("print.css")%>
</head>
<body>
   <div id="doc">
      <div id="hd" style="text-align: right;">
         <a href="javascript:void();" onclick="window.print();">Print</a>&nbsp;|&nbsp;<a id="ruArt"
            href="<%= Url.Action("ViewArticle", "article" ,new with { .id = Model.ArticleID,.slug = Model.Slug}) %>">Back to article</a>
      </div>
      <div id="bd">
         <h1 class="artTitle">
            <%=Html.Encode(Model.Title)%>
         </h1>
         <div class="byLine" style="clear: both;">
            <span>by</span>&nbsp<a href="<%=Url.Action("ShowAuthor", "Authors", New With {.username = Model.AddedBy})%>"><%=Model.AddedBy%></a><span>
               on </span>           
              <span style="color:#6a6868"> <%=Model.AddedDate.ToString()%></span><span></span>
         </div>
         <div class="artBody">
           <%=Model.Body%>
         </div>
      </div
      <div id="fu">
         <span>Copyright 2008 © Middle Class Crunch. All Right Reserved.</span>
      </div>
   </div>
</body>
</html>
