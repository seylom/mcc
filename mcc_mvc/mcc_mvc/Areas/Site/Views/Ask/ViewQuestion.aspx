<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Site/Views/Shared/twoColAsk.master"
   ValidateRequest="false" Inherits="System.Web.Mvc.ViewPage(Of vmQuestionDetail)" %>

<asp:Content ContentPlaceHolderID="PageTitle" runat="server">
   <%= Html.Encode(Model.PageTitle) %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="headItems" runat="Server">
   <%=appHelpers.AdvancedCssTagUrl("jqModal.css",  MCC.Utils.FileVersion("jqmodal"))%>
   <%=appHelpers.CssTagUrl("bbcode/style.css")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="middlePlaceHolder" runat="Server">
   <input type="hidden" id="questionId" value="<%= Model.Question.UserQuestionID %>" />
   <h3 class="qtitle">
      <%=Html.Encode(ViewData.Model.Question.Title)%>
   </h3>
   <div id="qu_<%= Model.Question.UserQuestionId.ToString() %>" class="question">
      <div>
         <table style="width: 100%">
            <tr>
               <td valign="top" style="width: 60px">
                  <div class="qrbx">
                     <div>
                        <span id="qval" class="qrbval">
                           <%=ViewData.Model.Question.Votes %></span><span>points</span>
                     </div>
                     <div class="status">
                        <table class="tb_stat" width="100%">
                           <tr style="height: 5px;">
                              <td class="up">
                              </td>
                              <td class="down">
                              </td>
                           </tr>
                        </table>
                     </div>
                     <div class="qvote">
                        <a href="javascript:void(0);" class="vk">
                           <img id="q_up" class="voteup qlnkauth" alt="Vote as useful" title="vote as useful"
                              src='<%= appHelpers.ImageUrl("pixel-v.gif")%>' /></a> <a href="javascript:void(0);"
                                 class="vk">
                                 <img id="q_down" class="votedown qlnkauth" alt="Vote as not useful" title="vote as not useful"
                                    src='<%= appHelpers.ImageUrl("pixel-v.gif")%>' /></a>
                     </div>
                  </div>
                   <% If User.Identity.IsAuthenticated Then%>
                  <div class="flw <%= iif(model.Followed,"f_on","f_off") %>">
                     <%= IIf(model.Followed, "following", "follow")%>
                  </div>
                  <% End If%>
               </td>
               <td>
                  <div class="qrbx-author">
                     Asked by <a id="questionAuthorLnk" href="<%= "/users/" & Html.Encode(ViewData.Model.Question.AddedBy) %>"
                        class="globalred">
                        <%= Html.Encode(Model.Question.AddedBy) %></a> on
                     <%=Model.Question.AddedDate.ToString("MMM dd yyyy") & " at " & Model.Question.AddedDate.ToString("t")%>
                  </div>
                  <div class="qrbody">
                     <%= Utils.ConvertBBCodeToHTML(ViewData.Model.Question.Body) %>
                  </div>
                  <div class="qrtool">
                     <% If ViewData.Model.CurrentUserIsAuthor Or User.IsInRole("Administrators") Then%>
                     <span id="editBox"><a href="<%= Url.Action("EditQuestion", new with {.id = Model.Question.UserQuestionID}) %>"
                        class="global qlnkauth">Edit</a>
                        <%--                        <%=Html.ActionLink("Edit", "EditQuestion", New With {.id = Model.Question.UserQuestionID}, New With {.Class = "global qlnkauth"})%>--%>&nbsp;|&nbsp;</span>
                     <% End If%>
                     <span id="Span1"><a id="qflag" href="javascript:void(0);" class="global qlnkauth"
                        style="font-size: 10px;" title="Flag this question">Flag</a></span> &nbsp;|&nbsp;<a
                           id="qrplnk" class="replylnk global qlnkauth" href='javascript:void(0);' title="Add a comment to this question">Comment</a>
                  </div>
                  <div class="qrp" style="">
                     <a id="postAnswerLnk" href="javascript:void(0);">Propose an answer</a>
                  </div>
                  <div style="padding: 5px; margin: 30px 5px 0 0; background-color: #f5f5f5;">
                     <h2 style="font-size: 10px; font-weight: bold;">
                        Comments</h2>
                     <div id="qcomment-lst">
                        <% For Each it As UserQuestionComment In Model.Comments%>
                        <%Html.RenderPartial("~/Areas/Site/views/shared/ask/questionCommentItem.ascx", it)%>
                        <%Next%>
                     </div>
                     <div id="qcommentbx" style="display: none; margin-top: 5px;">
                        <div id="qcomment" class="cbx">
                           <%Using Html.BeginForm("CommentQuestion", "Ask", New With {.id = Model.Question.UserQuestionID}, FormMethod.Post, New With {.name = "comment-form"})%>
                           <table style="width: 100%">
                              <tr>
                                 <td>
                                    <%=Html.TextArea("comment", New With {.class = "commta", .rows = 1, .cols = 5, .style = "width:450px;"})%>
                                 </td>
                                 <td>
                                    <input id='qcmbtn' type="submit" value="Add Comment" title="Add Comment" />
                                 </td>
                              </tr>
                           </table>
                           <% End Using%>
                        </div>
                     </div>
                  </div>
               </td>
            </tr>
         </table>
      </div>
   </div>
   <div class="answers" style="border-top: 1px dotted #eaeaea; padding-top: 20px;">
      <h2 style="font-size: 12px; font-weight: bold; color: #555555; padding: 5px;">
         <%= Model.AnswersCount & if(Model.AnswersCount = 1, " Answer"," Answers") %></h2>
      <a id="answers"></a>
      <div id="answers-lst">
         <% If Model.AnswersCount > 0 Then%>
         <%For Each it As vmAnswerDetail In Model.Answers%>
         <% Html.RenderPartial("~/Areas/Site/views/shared/ask/answerItem.ascx", it)%>
         <% Next%>
         <%End If%>
      </div>
      <%=Html.SimplePager(Model.Answers)%>
   </div>
   <%--<uc:ad runat="server" ID="bottomAd" />--%>
   <div style="margin-top: 20px;">
      <a id="postyouranswer"></a>
      <% If UserIsAuthenticated Then%>
      <% Using Html.BeginForm("postanswer", "ask", New With {.id = Model.Question.UserQuestionID}, FormMethod.Post, New With {.name = "answer-form"})%>
      <div id="answerbx">
         <h2 style="font-weight: bold; margin-bottom: 10px; color: #994138; font-size: 12px;">
            Post your answer</h2>
         <%=Html.DisplayError() %>
         <div style="margin-top: 20px;">
            <% Html.RenderPartial("~/Areas/Site/views/shared/BBCodeEditor.ascx", "")%>
         </div>
         <div style="margin-top: 10px;">
            <input type="submit" id="btnPostAnswer" value="Post your answer" />
         </div>
      </div>
      <% End Using%>
      <% Else%>
      <div id="loginbx">
         <div class="loginRequired">
            <a href="/login" class="globalred">login</a> or <a href="/signup" class="globalred">
               register</a> <span>to post an answer</span>
         </div>
      </div>
      <% End If%>
   </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="rightPlaceHolder" runat="Server">
   <div class="widget">
      <h3 class="title">
         Ask a question</h3>
      <div>
         <div style="padding: 5px;">
            Do you have a question in mind you would like to ask? <a id="A5" href="/questions/ask"
               class="global">Ask it now!</a>
         </div>
      </div>
   </div>
   <div class="widget">
      <h3 class="title">
         Related Questions</h3>
      <div>
         <div style="padding: 5px;">
         </div>
      </div>
   </div>
   <div class="widget">
      <h3 class="title">
         Need a professional answer?</h3>
      <div>
         <div style="padding: 5px;">
            Not satisfied by your peers answers? use justanswers.com to get an answer from a
            professional!
         </div>
      </div>
   </div>
   <div class="widget">
      <h3 class="title">
         More tags</h3>
      <div>
         <div style="padding: 5px;">
         </div>
      </div>
   </div>
<%--   <div class="widget">
      <h3 class="title">
         Ads ...</h3>
      <div>
         <div id="sdTags" style="padding: 5px;">
         </div>
      </div>
   </div>--%>
   <div class="adwidget">
      <% Html.RenderPartial("~/Areas/Site/views/shared/ads/adLargeSquareGoogle.ascx")%>
   </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="scriptLoader" runat="Server">
   <%=appHelpers.ScriptsTagUrl("global.js")%>
   <%=appHelpers.AdvancedScriptsTagUrl("questions.js", MCC.Utils.FileVersion("questions"))%>
   <%Html.RenderPartial("~/Areas/Site/views/shared/isAuthenticated.ascx")%>
   <script type="text/javascript">

      $(function () {
         Questions.Init('<%= Model.Question.UserQuestionID %>', '/login', '/asv/questions/', 'qval');
      });
      //]]>
   </script>
</asp:Content>
