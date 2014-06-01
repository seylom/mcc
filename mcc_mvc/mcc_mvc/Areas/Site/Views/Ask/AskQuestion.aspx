<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Site/Views/Shared/twoColAsk.master"
   Inherits="System.Web.Mvc.ViewPage(of AskQuestionViewModel)" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headItems" runat="server">
   <%=appHelpers.CssTagUrl("bbcode/style.css")%>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PageTitle" runat="server">
   Ask a question
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="middlePlaceHolder" runat="Server">
   <h2 style="font-size: 12px; margin: 5px 0;">
      Enter details for the question you would like to ask.</h2>
   <%=Html.DisplayError(Model) %>
   <%Using Html.BeginForm()%>
   <div style="margin: 20px 0 10px 0;">
      <table style="width: 100%">
         <tr>
            <td>
               <span>Title: (What is your question about ?)</span>
               <div style="margin: 5px 0 20px 0;">
                  <%= Html.TextBox("Title", Model.Title, New With {.class = "qtbx", .style = "width:95%"})%>
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
               <% Html.RenderPartial("~/Areas/Site/views/shared/bbcodeeditor.ascx", "")%>
            </td>
         </tr>
         <tr>
            <td>
               <div style="margin: 10px 0; text-align: left; vertical-align: middle;">
                  <%--<input type="checkbox" id="cbNotify" value="" title="Notify me of new answers" />--%>
                   <%= Html.CheckBox("Subscribe", Model.Subscribe, New With {.title = "Notify me of new answers"})%>
                  Notify me of new answer.
               </div>
            </td>
         </tr>
      </table>
      <input id="btnSubmit" type="submit" value="Post your question" />
   </div>
   <%End Using%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="rightPlaceHolder" runat="Server">
   <div class="widget">
      <h3 class="title">
         About MCC Questions</h3>
      <div>
         <div id="sdTags" runat="server" style="padding: 5px;">
            Ask your question on MCC and have fellow members give you answers
         </div>
      </div>
   </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="scriptLoader" runat="server">
   <%=appHelpers.ScriptsTagUrl("global.js")%>
   <%-- <script type="text/javascript">
      var __defval = "What is your question about ?"
      $(function() {
         $('#Title').blur(function() {
            if ($(this).val() == '') {
               $(this).val(__defval).addClass('qempty');
            }
         }).focus(function() {
            if ($(this).val() == __defval) {
               $(this).val("").removeClass('qempty');
            }
         })
      });
   </script>--%>
</asp:Content>
