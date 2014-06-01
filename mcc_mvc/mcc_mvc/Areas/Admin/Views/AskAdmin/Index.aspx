<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage(of AdminUserQuestionsViewModel)" %>

<asp:Content ContentPlaceHolderID="PageTitle" runat="server">
   Questions
</asp:Content>
<asp:Content ContentPlaceHolderID="mainSiteContent" runat="server">
   <div style="border-bottom: 1px solid #eaeaea; padding: 5px; margin-bottom: 10px;">
      <h1 class="admin-title">
         Questions</h1>
   </div>
   <% If Model.questions Is Nothing Or Model.questions.Count <= 0 Then%>
   <center>
      <b>- No questions are currently in the database -</b>
   </center>
   <% Else%>
   <div style="padding: 5px; margin: 10px 0; background-color: #e7faea;">
      <input type="button" id="btnDeleteTop" disabled="disabled" value="Delete" title="Delete" />
   </div>
   <%=Html.AdvancedPager(Model.questions, "Index", "AskAdmin", New Integer() {10, 30, 50})%>
   <table class="edit-info datatable">
      <col align="center" width="20px;" />
      <col />
      <col />
      <col width="80px" />
      <col align="center" width="50px" />
      <col align="center" width="50px" />
      <col align="center" />
      <thead>
         <tr>
            <th>
               <input type="checkbox" id="checkCol" />
            </th>
            <th>
               Title
            </th>
            <th>
               Author
            </th>
            <th>
               Added Date
            </th>
            <th>
               Comments
            </th>
            <th>
               Answers
            </th>
         </tr>
      </thead>
      <tbody>
         <% Dim idx As Integer = 0%>
         <% For Each it As UserQuestion In Model.questions%>
         <tr id='<%="item_" & it.UserQuestionID %>' class='<%= IIF(idx Mod 2 = 0,"even", "odd") %>'
            tag='<%= it.UserQuestionID %>'>
            <td align="center">
               <input type="checkbox" tag='<%= it.UserQuestionID %>' class='cb_ar' />
            </td>
            <td>
               <a class="lnk" title='<%= Html.Encode(it.Title) %>' href='<%= url.Action("ViewQuestion",new with {.controller="Ask",.Id = it.UserQuestionID,.slug=it.Slug}) %>'>
                  <%= Html.Encode(it.Title) %></a>
            </td>
            <td>
               <%=it.AddedBy%>
            </td>
            <td>
               <%=it.AddedDate.ToString("MM-dd-yyyy")%>
            </td>
            <td>
               <a class="lnk" href="<%= Url.Action("QuestionCommentsByQuestion", new with {.Id= it.userquestionID}) %>">
                  comments</a>
            </td>
            <td>
               <a class="lnk" href="<%= Url.Action("AnswersByQuestion", new with {.Id= it.userquestionID}) %>">
                  Answers</a>
            </td>
         </tr>
         <%Next%>
      </tbody>
   </table>
   <%=Html.AdvancedPager(Model.questions, "Index", "AskAdmin", New Integer() {10, 30, 50})%>
   <div id="tools" style="padding: 5px; margin-top: 10px; background-color: #e7faea;">
      <input id="btnDelete" type="button" disabled="disabled" value="Delete" title="Delete" />
   </div>
   <% End If%>
</asp:Content>
<asp:Content ContentPlaceHolderID="bottomScripts" runat="server">

   <%=appHelpers.ScriptsTagUrl("global.js")%>
   <%=appHelpers.ScriptsTagUrl("jquery/jquery.tablesorter.min.js")%>

   <script type="text/javascript">

      $(document).ready(function() {
         Init();
      });

      var Proxy = new serviceProxy('/admin/questions/');

      function Init() {
         $('.datatable').tablesorter({
            widgets: ['zebra'],
            headers: {
               0: { sorter: false },
               7: { sorter: false }
            }
         });
      }

      $(":checkbox", '.datatable').click(function(ev) {
         var e = $.event.fix(ev);
         e.stopPropagation();

         if ($(this).attr('id') == 'checkCol') {
            var cb = $(this).is(':checked');
            $("input:checkbox", '.datatable').attr('checked', cb);
         }

         if ($("input:checked", '.datatable').length > 0) {
            $('#btnDelete,#btnDeleteTop').attr('disabled', false); //.removeClass('rb-delete-disable').addClass('rb-delete');
         }
         else {
            $('#btnDelete,#btnDeleteTop').attr('disabled', true); //.removeClass('rb-delete').addClass('rb-delete-disable');
         }
      });

      $(".even,.odd").hover(function() { $(this).addClass('hilight'); }, function() { $(this).removeClass('hilight'); });

      $('#btnDelete,#btnDeleteTop').click(function() {
         if (confirm('Do you really want to delete the selected articles?\r\n This operation cannot be reverted')) {

            var ar_list = $('input:checked', '.datatable ');
            if (ar_list.length > 0) {
               var ids = [];

               ar_list.each(function(i, item) {
                  var tag = $(item).attr('tag');
                  if ((tag !== undefined) && (tag !== '')) {
                     ids.push(parseInt(tag));
                  }
               });

               if (ids.length > 0) {
                  var data = { 'Ids': ids };
                  Proxy.invoke('DeleteQuestions', data, function(result) {
                     for (var k = 0; k < ids.length; k++) {
                        $('#item_' + ids[k]).fadeOut('slow', function() {
                           $('#item_' + ids[k]).remove();
                        });
                     }

                  });
               }
            }
         }
         return false;
      });

   </script>

</asp:Content>
