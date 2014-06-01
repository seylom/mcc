<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage(of AdminQuotesViewModel)" %>

<asp:Content ContentPlaceHolderID="PageTitle" runat="server">
   Quotes
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="headerPlaceHolder" runat="server">
   <%= appHelpers.CssTagUrl("jqModal.css") %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainSiteContent" runat="Server">
   <div style="border-bottom: 1px solid #eaeaea; padding: 5px; margin-bottom: 10px;">
      <h1 class="admin-title">
         Quotes</h1>
   </div>
   <% If Model.quotes IsNot Nothing Then%>
   <%=Html.AdvancedPager(Model.quotes, "Index", "QuoteAdmin", New Integer() {10, 30, 50})%>
   <table id="jTemplate" class="edit-info datatable">
      <col align="center" width="20px" />
      <col align="left" />
      <col align="right" />
      <col align="center" />
      <col align="right" />
      <col align="center" />
      <thead>
         <tr>
            <th>
               <input type="checkbox" class="im_cb" id="checkCol" />
            </th>
            <th>
               Body
            </th>
            <th>
               AddedBy
            </th>
            <th>
               Author
            </th>
            <th>
               Role / Function
            </th>
            <th>
            </th>
         </tr>
      </thead>
      <tbody>
         <% Dim idx As Integer = 0%>
         <% For Each it As Quote In Model.quotes%>
         <tr id='item_<%= it.QuoteID %>' class='quote <%= iif(idx Mod 2 = 0,"even", "odd") %>'
            tag='<%= it.QuoteID %>'>
            <td>
               <input type="checkbox" class="im_cb" tag='<%=it.QuoteID%>' />
            </td>
            <td>
               <%=it.Body%>
            </td>
            <td>
               <%=it.AddedBy%>
            </td>
            <td>
               <%=it.Author%>
            </td>
            <td>
               <%=it.Role%>
            </td>
            <td>
               [<a id="A1" class="global" href='<%= url.Action("EditQuote", new with {.Id = it.QuoteID}) %>'
                  title="Edit article">Edit</a>]
            </td>
         </tr>
         <% Next%>
      </tbody>
   </table>
   <%=Html.AdvancedPager(Model.quotes, "Index", "QuoteAdmin", New Integer() {10, 30, 50})%>
   <div id="tools" style="padding: 5px; margin-top: 10px; background-color: #e7faea;">
      <input id="btnDelete" type="button" value="Delete" title="Delete" disabled="disabled" />
   </div>
   <%Else%>
   <center>
      <b>- No quotes are currently in the database -</b>
   </center>
   <% End If%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottomScripts" runat="server">
   <%=appHelpers.ScriptsTagUrl("jquery/jquery.tablesorter.min.js")%>
   <%=appHelpers.ScriptsTagUrl("json2.js")%>
   <%=appHelpers.AdvancedScriptsTagUrl("utils.js", MCC.Utils.FileVersion("utils"))%>
   <%=appHelpers.ScriptsTagUrl("jquery/jquery.effects.js")%>

   <script type="text/javascript">

      $(document).ready(function() {
         Init();
      });


      var Proxy = new serviceProxy('<%=ResolveUrl("~/asv.asmx/") %>');

      function Init() {
         $('.edit-info').tablesorter({
            widgets: ['zebra'],
            headers: {
               0: { sorter: false },
               5: { sorter: false }
            }
         });

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
            if (confirm('Do you really want to delete the selected quotes?\r\n This operation cannot be reverted')) {

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
                     Proxy.invoke('DeleteQuotes', data, function(result) {
                        for (var k = 0; k < ids.length; k++) {
                           $('#item_' + ids[k]).fadeOut('slow', function() {
                              $('#item_' + ids[k]).remove();
                           });
                        }
                        $(".datatable").trigger("update");


                        var pageIndex = 0;
                        var pageSize = 30;

                        Proxy.invoke('GetQuotes', { 'pageIndex': pageIndex, 'pageSize': pageSize }, function(result) {


                        });

                     });
                  }
               }
            }
            return false;
         });

         //            $("tr.quote").dblclick(function() {
         //                var pk = $(this).attr("tag");
         //                editImage($(this), parseInt(pk));
         //            }).hover(function() { $(this).addClass('hilight'); }, function() { $(this).removeClass('hilight'); });
      }
        
   </script>

</asp:Content>
