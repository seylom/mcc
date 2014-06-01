<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage(of AdminArticleCommentsViewModel)" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
   Article Comments
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="mainSiteContent" runat="server">
<div>
   <input type="text" value="search" style="width:300px; padding:5px;"/><input type="button" value="Search"/>
</div>
   <div style="padding: 5px; margin: 10px 0; background-color: #e7faea;">
      <input type="button" id="btnDeleteTop" disabled="disabled" value="Delete" title="Delete" />
   </div>
   <%=Html.AdvancedPager(Model.Comments, "ArticleComments", "ArticleAdmin", New Integer() {30, 50, 100})%>
   <div>
   </div>
   <table width="100%" class="datatable">
      <thead>
         <tr style="background-color: #bababa;padding:4px;">
            <th>
               <input type="checkbox" id="checkCol" />
            </th>
            <th>
               Comments
            </th>
         </tr>
      </thead>
      <tbody>
         <%For Each it As ArticleComment In Model.Comments%>
         <tr id='<%="item_" & it.CommentId %>' style="background-color: #f1f1f1;">
            <td valign="middle" align="center">
               <input type="checkbox" tag='<%= it.CommentId %>' />
            </td>
            <td>
               <div style="padding: 10px; margin-bottom: 5px;">
                  <div style="font-size: 12px;">
                     <%= Html.Encode(If(it.Body.Length > 100, it.Body.Substring(0, 100), it.Body))%>
                  </div>
                  <div style="font-size: 10px; margin-top: 5px;">
                     <span style="color: #777777;">by
                        <%= it.AddedBy%></span>&nbsp;|&nbsp<span><%= it.AddedDate.ToString() %></span>
                  </div>
                  <div>
                     <h3 style="font-size: 10px;">
                        <%= "Article Title"%></h3>
                  </div>
               </div>
            </td>
         </tr>
         <% Next%>
      </tbody>
   </table>
   <div id="tools" style="padding: 5px; margin-top: 10px; background-color: #e7faea;">
      <input id="btnDelete" type="button" disabled="disabled" value="Delete" title="Delete" />
   </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="bottomScripts" runat="server">
   <%=appHelpers.ScriptsTagUrl("global.js")%>
   <script type="text/javascript">

      var Proxy = new serviceProxy('/admin/articles/');

      $(":checkbox", '.datatable').click(function (ev) {
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

      $('#btnDelete,#btnDeleteTop').click(function () {
         if (confirm('Do you really want to delete the selected comments?\r\n This operation cannot be reverted')) {

            var ar_list = $('input:checked', '.datatable ');
            if (ar_list.length > 0) {
               var ids = [];

               ar_list.each(function (i, item) {
                  var tag = $(item).attr('tag');
                  if ((tag !== undefined) && (tag !== '')) {
                     ids.push(parseInt(tag));
                  }
               });

               if (ids.length > 0) {
                  var data = { 'Ids': ids };
                  Proxy.invoke('DeleteComments', data, function (result) {
                     for (var k = 0; k < ids.length; k++) {
                        $('#item_' + ids[k]).fadeOut('slow', function () {
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
