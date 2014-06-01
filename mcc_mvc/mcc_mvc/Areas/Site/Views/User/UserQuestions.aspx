<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Site/Views/Shared/TwoCol.master" Inherits="System.Web.Mvc.ViewPage(of UserProfileQuestionViewModel)" %>

<asp:Content ContentPlaceHolderID="PageTitle" runat="server">
   User questions
</asp:Content>
<asp:Content ContentPlaceHolderID="middleplaceholder" runat="server">
<div style="margin-bottom:10px;">
   User questions
</div>
   <% If Model.Questions IsNot Nothing Then%>
   <table style="width: 100%;">
      <tbody>
         <% For Each it As UserQuestion In Model.Questions%>
         <tr>
            <td>
               <div style="margin: 3px 0; padding: 3px; background-color: #f1f1f1">
                  <h2 style="font-size: 11px; margin: 3px;">
                     <a id="A1" class="global" href='<%= url.action("ViewQuestion","Ask",new with {.Id = it.UserQuestionID, .slug=it.slug}) %>'>
                        <%=Html.Encode(it.Title)%></a></h2>
                  <div style="font-size: 10px; color: #555555">
                     <span>
                        <%=it.Votes%>
                        votes</span>&nbsp;|&nbsp;<%=it.Views%>
                     views &nbsp;|&nbsp;<%=it.AnswerCount%>
                     answers
                  </div>
               </div>
            </td>
         </tr>
         <% Next%>
      </tbody>
   </table>
   <%=Html.AdvancedPager(Model.Questions, "userquestions", "user", New Integer() {10, 30, 50})%>
   <% End If%>
</asp:Content>
