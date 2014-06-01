<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Site/Views/Shared/twoColAsk.master" Inherits="System.Web.Mvc.ViewPage(of UserQuestion)" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headItems" runat="Server">
   <%=appHelpers.CssTagUrl("bbcode/style.css")%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageTitle" runat="server">
Edit Question
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="middlePlaceHolder" runat="Server">
<% Using Html.BeginForm(New With {.action = "EditQuestion", .Title = "", .body = "", .questionId = Model.UserQuestionID, .slug = Model.Slug})%>
   <p style="margin-bottom: 20px;">
      <b>Edit your question : -</b>
   </p>
<%--   <%=Html.ValidationSummary("Error on the page ...", New With {.class = "error"})%>--%>
       <%=Html.DisplayError() %>
   <table style="width: 100%">
      <tr>
         <td>
            Your question? (in a few words)
         </td>
      </tr>
      <tr>
         <td>
            <div style="margin-bottom: 10px;">
               <%=Html.TextBox("Title", Model.Title, New With {.class = "qtbx", .style = "width:95%"})%>
               <%=Html.ValidationMessage("Title", "")%>
            </div>
         </td>
      </tr>
      <tr>
         <td>
            Details (add more info here.)
         </td>
      </tr>
      <tr>
         <td>
            <%Html.RenderPartial("~/Areas/Site/views/shared/BBCodeEditor.ascx", Model.Body)%>
         </td>
      </tr>
   </table>
   <div style="margin-top: 5px;">
      <input type="submit" id="btnSubmit" value="" class="rb-btn rb-update"/>
      <input type="button" id="btnCancel" value="" class="rb-btn rb-cancel"/>
   </div>
   
   <% End Using%>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="scriptLoader">
   <%=appHelpers.ScriptsTagUrl("global.js")%>
</asp:Content>