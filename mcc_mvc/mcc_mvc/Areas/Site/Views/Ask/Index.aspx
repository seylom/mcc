<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Site/Views/Shared/twoColAsk.master"
   Inherits="System.Web.Mvc.ViewPage(Of vmQuestions)" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headItems" runat="Server">
   <%=appHelpers.AdvancedCssTagUrl("article.css", MCC.Utils.FileVersion("articlecss"))%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="PageTitle" runat="server">
   <%=Html.Encode(Model.PageTitle)%>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="middlePlaceHolder" runat="Server">
   <b>Have a question? Ask away and get help</b>
   <div class="qrp">
      <a href="<%= url.action("AskQuestion") %>">Ask your question now</a>
   </div>
   <% If (ViewData.Model.QuestionsList IsNot Nothing AndAlso ViewData.Model.QuestionsList.Count > 0) Then%>
   <%=Html.AdvancedPager(Model.QuestionsList, "Index", "Ask", New Integer() {10, 30, 50})%>
   <% For Each it As UserQuestion In Model.QuestionsList%>
   <div id="qu_<%= it.UserQuestionId.ToString() %>" class="q">
      <div class="qx">
         <div class="qrbx">
            <span class="qrbval">
               <%= it.Votes %></span> <span style="font-size: 10px;">
                  <%= If( it.Votes = 1, "point", "points")%></span>
            <div class='state <%= if(it.BestUserAnswerId > 0,"resolved","unresolved") %>'>
            </div>
         </div>
         <% If User.Identity.IsAuthenticated Then%>
         <% Dim bres As Boolean = Model.QuestionFollowed.Contains(it.UserQuestionID)%>
         <span id="fw_<%= it.UserQuestionId.ToString()%>" class="flw <%= iif(bres,"f_on","f_off") %>" >
            <%= IIf(bres,"following","follow") %>
         </span>
         <% End If%>
      </div>
      <div class="qbx">
         <h3 class="artTitle">
            <%= Html.ActionLink(it.Title, "viewquestion", New With {.id = it.UserQuestionID, .slug = it.Slug})%>
         </h3>
         <div class="qrbx-author">
            Asked by <a id="A1" href='<%= "/users/" & it.AddedBy %>' class="globalred">
               <%=it.Addedby%></a> on
            <%=it.AddedDate.ToString("MMM dd yyyy") & " at " & it.AddedDate.ToString("t")%>
         </div>
         <div style="margin-top: 5px;">
            <%= Utils.ConvertBBCodeToHTML(it.Body) %>
         </div>
         <div style="margin-top: 5px;">
         </div>
         <div style="margin-top: 5px" class="qrbx-info">
            <a id="A2" href='<%="/questions/" & it.UserQuestionId.ToString()  & "/" & it.slug & "#answers" %>'
               class="global">
               <%=it.AnswerCount%>
               Answers</a>&nbsp;|&nbsp;<span><%=it.Views  %>
                  views</span>
         </div>
      </div>
   </div>
   <%Next%>
   <%=Html.AdvancedPager(Model.QuestionsList, "Index", "Ask", New Integer() {10, 30, 50})%>
   <%Else%>
   <div style="margin: 5px 0; padding: 20px; background-color: #fff;">
      <center>
         <b>No Questions found for the specified topic, please try again later!</b>
      </center>
   </div>
   <%End If%>
</asp:Content>
<asp:Content ID="content3" ContentPlaceHolderID="rightPlaceHolder" runat="server">
   <div class="widget">
      <h3 class="title">
         Welcome to Ask MCC</h3>
      <div>
         <div id="sdTags" runat="server" style="padding: 5px;">
            a place where Middle Class Crunch members help each other out.<br />
            <br />
            <span style="color: #994138"><b>Ask any financial-related question you want here. Be
               specific, and we or a fellow MCC user will try and provide an insightful answer.
               Come back often and don't forget to rate the answers to your liking</b></span>
         </div>
      </div>
   </div>
   <div class="widget">
      <h3 class="title">
         Notations</h3>
      <div>
         <div style="padding: 5px;">
            <div>
               <img src="<%= AppHelpers.ImageUrl("OK24.png") %>" alt="Accepted answer" /><span>:accepted
                  answer available.</span></div>
         </div>
      </div>
   </div>
   <div class="adwidget">
      <% Html.RenderPartial("~/Areas/Site/views/shared/ads/adLargeSquareGoogle.ascx")%>
   </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="scriptLoader" runat="Server">
   
   <%=appHelpers.ScriptsTagUrl("json2.js")%>
   <%=appHelpers.AdvancedScriptsTagUrl("utils.js", MCC.Utils.FileVersion("utils"))%>
    <%=appHelpers.ScriptsTagUrl("questions.js")%>

   <script type="text/javascript">

      $(document).ready(function () {
         Init();
      });

      var Proxy = new serviceProxy("/asv/questions/");

      function Init() {
         $(".flw").unbind("click").click(function () {
            //toggle following status for current user
            var Id = $(this).attr('id').substr("fw_".length);
            if (Id > 0) {
               var DTO = { 'Id': Id };
               Proxy.invoke(Id + '/Follow', DTO, function (result) {
                  var blk = "#qu_" + Id;
                  if (result) {
                     $(".flw", blk).removeClass("f_off");
                     $(".flw", blk).addClass("f_on");
                     $(".flw", blk).html("following");
                  }
                  else {
                     $(".flw", blk).removeClass("f_on");
                     $(".flw", blk).addClass("f_off");
                     $(".flw", blk).html("follow");
                  }
               }
               , function (err) { alert(err.message); }, true);
            }
         });
      }

   </script>
</asp:Content>
