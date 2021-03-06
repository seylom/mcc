<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage(of AdminUserAnswerCommentsViewModel)" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
   Answers Comments
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainSiteContent" runat="server">
   <div style="border-bottom: 1px solid #eaeaea; padding: 5px; margin-bottom: 10px;">
      <h1 class="admin-title">Answers Comments</h1>
   </div>
   <% If Model.Comments Is Nothing Or Model.Comments.Count <= 0 Then%>
   <center>
      <b>- No comments for answers -</b>
   </center>
   <% Else%>
   <div style="padding: 5px; margin: 10px 0; background-color: #e7faea;">
      <input type="button" id="btnDeleteTop" disabled="disabled" value="Delete" title="Delete" />
   </div>
   <%=Html.AdvancedPager(Model.Comments, "AnswerComments", "AskAdmin", New Integer() {10, 30, 50})%>
   <table class="edit-info datatable">
      <col align="center" width="20px;" />
      <col />
      <col width="80px" />
      <col align="center" />
      <thead>
         <tr>
            <th>
               <input type="checkbox" id="checkCol" />
            </th>
            <th>
            Body
            </th>
            <th>
               Author
            </th>
            <th style="width:80px;">
               Added Date
            </th>
         </tr>
      </thead>
      <tbody>
         <% Dim idx As Integer = 0%>
         <% For Each it As UserAnswerComment In Model.Comments%>
         <tr id='<%="item_" & it.userAnswerCommentID %>' class='<%= IIF(idx Mod 2 = 0,"even", "odd") %>'
            tag='<%= it.userAnswerCommentID %>'>
            <td align="center">
               <input type="checkbox" tag='<%= it.userAnswerCommentID %>' class='cb_ar' />
            </td>
            <td>
               <%=Utils.ConvertBBCodeToHTML(it.Body)%>
            </td>
            <td>
               <%=it.AddedBy%>
            </td>
            <td>
               <%=it.AddedDate.ToString("MM-dd-yyyy")%>
            </td>
         </tr>
         <%Next%>
      </tbody>
   </table>
   <%=Html.AdvancedPager(Model.Comments, "AnswerComments", "AskAdmin", New Integer() {10, 30, 50})%>
   <div id="tools" style="padding: 5px; margin-top: 10px; background-color: #e7faea;">
      <input id="btnDelete" type="button" disabled="disabled" value="Delete" title="Delete" />
   </div>
   <% End If%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottomScripts" runat="server">
    <%=appHelpers.ScriptsTagUrl("jquery/jquery.tablesorter.min.js")%>
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
         if (confirm('Do you really want to delete the selected comments?\r\n This operation cannot be reverted')) {

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
                  Proxy.invoke('DeleteAnswerComments', data, function(result) {
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
