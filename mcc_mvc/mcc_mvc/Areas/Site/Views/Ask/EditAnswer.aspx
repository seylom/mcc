<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Site/Views/Shared/twoColAsk.master" Inherits="System.Web.Mvc.ViewPage(Of vmEditAnswerDetail)" %>


<asp:Content ID="Content1" ContentPlaceHolderID="headItems" runat="server">
 <%=appHelpers.CssTagUrl("bbcode/style.css")%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageTitle" runat="server">
Edit Answer
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="middlePlaceHolder" runat="Server">
   <h2 style="font-size:12px;margin-bottom:10px;" >
      <%=Html.Encode(Model.Title)%>
   </h2>
   
    <%=Html.DisplayError() %>
<% Using Html.BeginForm(New With {.action = "EditAnswer", .id = Model.Answer.UserAnswerID, .questionId = Model.Answer.UserQuestionID, .slug = Model.QuestionSlug})%>
<% Html.RenderPartial("~/Areas/Site/views/shared/BBCodeEditor.ascx", Model.Answer.Body)%>
   <div style="margin-top: 5px;">
      <input  type="submit" class="rb-btn rb-update" value="" />
      <input  type="button" class="rb-btn rb-cancel"  />
   </div>
   <%End Using%>
   
   
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="scriptLoader">
 <%=appHelpers.ScriptsTagUrl("global.js")%>

</asp:Content>