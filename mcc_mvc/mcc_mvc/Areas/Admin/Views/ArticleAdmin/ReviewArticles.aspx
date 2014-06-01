<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage(of AdminArticlesListViewModel)" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
   Review Articles
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainSiteContent" runat="Server">
   <div style="border-bottom: 1px solid #eaeaea; padding: 5px; margin-bottom: 10px;">
      <h1 class="admin-title">
         Review Articles</h1>
   </div>
   <div style="padding: 5px;">
      <a id="A1" href="?s=def" class="global">All</a>&nbsp;|&nbsp; <a id="A2" href="?s=pen"
         class="global">Pending (<%=Model.PendingCount%>)</a>&nbsp;|&nbsp; <a id="A3" href="?s=rej"
            class="global">Rejected (<%=Model.RejectedCount%>)</a>&nbsp;|&nbsp; <a id="A4" href="?s=ver"
               class="global">Verified (<%=Model.VerifiedCount%>)</a>&nbsp;|&nbsp; <a id="A5" href="?s=acc"
                  class="global">Accepted (<%=Model.AcceptedCount%>)</a>&nbsp;|&nbsp;
      <a id="A6" href="?s=app" class="global">Approved (<%=Model.ApprovedCount%>)</a>&nbsp;|&nbsp;
      <a id="A7" href="?s=qua" class="global">Quarantine (<%=Model.QuarantineCount%>)</a>
   </div>
   <% If Model.Articles Is Nothing Or Model.Articles.Count = 0 Then%>
   <center>
      <b>- No articles found -</b>
   </center>
   <% Else%>
   <div id="delBx" style="padding: 5px; margin: 10px 0; background-color: #e7faea;">
      <input type="button" id="btnDeleteTop" disabled="disabled" value="Delete" title="Delete" />
   </div>
   <%=Html.AdvancedPager(Model.Articles, "ReviewArticles", "ArticleAdmin", New Integer() {10, 30, 50})%>
   <table class="edit-info datatable">
      <col align="center" width="25px" />
      <col align="left" />
      <col align="right" />
      <col align="center" />
      <col align="right" />
      <col align="right" />
      <col align="center" />
      <thead>
         <tr>
            <th>
               <input type="checkbox" class="ar_cb" id="checkCol" />
            </th>
            <th>
               Title
            </th>
            <th>
               Author
            </th>
            <th>
               Status
            </th>
            <th>
               Release Date
            </th>
            <th>
               Expire Date
            </th>
            <th>
            </th>
         </tr>
      </thead>
      <tbody>
         <% Dim idx As Integer = 0%>
         <%For Each it As Article In Model.Articles%>
         <tr id='<%= "item_" & it.ArticleID %>' class='<%= IIF(Idx Mod 2 = 0,"even", "odd") %>'
            tag='<%= it.ArticleID %>'>
            <td>
               <input type="checkbox" class="ar_cb" tag='<%=it.ArticleID %>' />
            </td>
            <td>
               <a class="lnk" title='<%=Html.Encode(it.Title) %>' href='<%= url.Action("AddEditArticle", new with {.Id= it.ArticleID}) %>'>
                  <%=Html.Encode(it.Title) %></a> <span>(<a title="peek" href='<%=Url.Action("PeekArticles", new with {.id= it.ArticleID,.s="pending"})%>'>peek</a>)</span>
            </td>
            <td>
               <%=it.AddedBy%>
            </td>
            <td>
               <span class='<%= "notap st_" & it.Status  %>'>
                  <%=Utils.GetStatusCaption(it.Status)%></span>
            </td>
            <td>
               <%=it.ReleaseDate.ToString()%>
            </td>
            <td>
               <% =it.ExpireDate.ToString()%>
            </td>
            <td align="center">
               <a href='<%= url.Action("AddEditArticle", new with {.Id= it.ArticleID}) %>' title="Edit article">
                  Edit</a>
            </td>
         </tr>
         <%idx += 1%>
         <%Next%>
      </tbody>
   </table>
   <%=Html.SimplePager(Model.Articles)%>
   <div id="tools" style="padding: 5px; margin-top: 10px; background-color: #e7faea;">
      <input id="btnDelete" type="button" disabled="disabled" value="Delete" title="Delete" />
   </div>
   <% End If%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottomScripts" runat="Server">
   <%=appHelpers.ScriptsTagUrl("jquery/jquery.tablesorter.min.js")%>
   <%=appHelpers.ScriptsTagUrl("global.js")%>
   <%--<%=appHelpers.AdvancedScriptsTagUrl("utils.js", MCC.Utils.FileVersion("utils"))%>--%>
   <%=appHelpers.ScriptsTagUrl("jquery/jquery.tablesorter.min.js")%>

   <script type="text/javascript">

      $(document).ready(function() {
         Init();
      });

      var Proxy = new serviceProxy('/admin/articles/');

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
                  Proxy.invoke('DeleteArticles', data, function(result) {
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
