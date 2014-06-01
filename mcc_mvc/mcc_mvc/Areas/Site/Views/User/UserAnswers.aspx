<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Site/Views/Shared/TwoCol.master" Inherits="System.Web.Mvc.ViewPage(of UserProfileAnswerViewModel)" %>

<asp:Content ID="Content2" ContentPlaceHolderID="PageTitle" runat="server">
   User answers
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="middleplaceholder" runat="server">
<div style="margin-bottom:10px;">
   User answers
</div>
   <% If Model.Answers IsNot Nothing Then%>
   <table style="width: 100%;">
      <tbody>
         <% For Each it As UserAnswer In Model.Answers%>
         <tr>
            <td>
               <div style="margin: 3px 0; padding: 3px; background-color: #f1f1f1">
                  <h2 style="font-size: 11px; margin: 3px;">
                     <a id="A2" class="global" href='<%= url.Action("ViewQuestion","Ask",new with {.Id = it.UserQuestionID,.slug=it.questionSlug})%>'>
                        <%=Html.Encode(it.QuestionTitle)%></a></h2>
               </div>
            </td>
         </tr>
         <% Next%>
      </tbody>
   </table>
   <%=Html.AdvancedPager(Model.Answers, "useranswers", "user", New Integer() {10, 30, 50})%>
   <% End If%>
</asp:Content>
