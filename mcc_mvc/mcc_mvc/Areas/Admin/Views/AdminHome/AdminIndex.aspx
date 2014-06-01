<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage(of adminUsersViewModel)" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
Unapproved Accounts
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainSiteContent" runat="Server">
   <div style="border-bottom: 1px solid #eaeaea; padding: 5px; margin-bottom: 10px;">
      <h1 class="admin-title">
         Index</h1>
   </div>
   <table class="edit-info">
      <tr>
         <th colspan="2">
            Account Management Page.
         </th>
      </tr>
      <tr>
         <td class="right" style="width: 200px;">
            Number of registered Users:
         </td>
         <td class="left">
           
         </td>
      </tr>
      <tr>
         <td class="right">
            Number of users online:
         </td>
         <td class="left">
            
         </td>
      </tr>
   </table>
   <div style="margin: 5px 0;">
      <h2 style="font-size: 12px;">
         Unapproved accounts</h2>
   </div>
   <div style="padding: 5px; margin: 10px 0; background-color: #e7faea;">
      <input type="button" id="btnDeleteTop" disabled="disabled" value="Delete" title="Delete" />
   </div>
   <table class="edit-info datatable" style="margin-top: 10px;">
      <thead>
         <tr>
            <th>
               <input type="checkbox" id="checkCol" />
            </th>
            <th>
               Username
            </th>
            <th>
               Email
            </th>
            <th>
               Created
            </th>
            <th>
               Last Activity
            </th>
            <th>
               Approved
            </th>
            <th>
               Command
            </th>
         </tr>
      </thead>
      <tbody>
         <% Dim idx As Integer = 0%>
         <% For Each it As SiteUser In Model.Users%>
         <tr id='item_<%= it.UserID  %>' class='<%= IIF(idx Mod 2 = 0,"even","odd") %>'>
            <td>
               <input type="checkbox" tag='<%= it.UserID %>'
                  class='cb_ar scb' />
            </td>
            <td>
               <%=it.UserID%>
            </td>
            <td>
               <%=it.Email%>
            </td>
            <td>
               <%=it.CreationDate%>
            </td>
            <td>
               <%=it.LastActivityDate%>
            </td>
            <td>
               <%=it.IsApproved%>
            </td>
            <td>
               <a href='<%= "/Admin/Index/EditUser.aspx?Username=" & it.Username  %>'
                  title="Edit User">Edit</a>
            </td>
         </tr>
         <% idx += 1%>
         <%Next%>
      </tbody>
   </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottomScripts" runat="Server">
   <%=appHelpers.ScriptsTagUrl("jquery/jquery.tablesorter.min.js")%>
   <%=appHelpers.ScriptsTagUrl("json2.js")%>
   <%=appHelpers.AdvancedScriptsTagUrl("utils.js", MCC.Utils.FileVersion("utils"))%>

   <script type="text/javascript">
      $(document).ready(function() {
         Init();
      });

      var lastChecked = null;
      var Proxy = new serviceProxy('<%=ResolveUrl("~/asv.asmx/") %>');

      $('.scb').click(function(event) {
         if (!lastChecked) {
            lastChecked = this;
            return;
         }

         if (event.shiftKey) {
            var start = $('.scb').index(this);
            var end = $('.scb').index(lastChecked);

            if (lastChecked.checked) {
               $('.scb').slice(Math.min(start, end), Math.max(start, end)).attr('checked', lastChecked.checked)
               .parent().parent().addClass('sel');
            }
            else {
               $('.scb').slice(Math.min(start, end), Math.max(start, end)).attr('checked', lastChecked.checked)
               .parent().parent().removeClass('sel');
            }


            //            $('.scb').slice(Math.min(start, end), Math.max(start, end)).attr('checked', lastChecked.checked)
            //            .each(function(item) {
            //               $(item).parent().parent().removeClass('sel');
            //               if (lastChecked.checked) {
            //                  $(item).parent().parent().addClass('sel')
            //               }
            //            });
         }

         lastChecked = this;
      });



      function Init() {


         $('.edit-info').tablesorter({
            widgets: ['zebra'],
            headers: {
               0: { sorter: false },
               6: { sorter: false }
            }
         });

         $('#checkCol', '.datatable').click(function() {
            var cb = $(this).is(':checked');
            $("input:checkbox", '.datatable').attr('checked', cb).parent().parent().toggleClass('sel');
         })


         $(".scb", '.datatable').click(function(ev) {
            var e = $.event.fix(ev);
            e.stopPropagation();

            $(this).parent().parent().toggleClass('sel')
            if ($(this).is(':checked') == false) {
               $('#checkCol').attr('checked', false);
            }

            if ($(".scb:checked", '.datatable').length > 0) {
               $('#btnDelete,#btnDeleteTop').removeAttr('disabled')
            }
            else {
               $('#btnDelete,#btnDeleteTop').attr('disabled', 'disabled')
            }
         });

         $(".even,.odd").hover(function() { $(this).addClass('hilight'); }, function() { $(this).removeClass('hilight'); });

         $('#btnDelete,#btnDeleteTop').click(function() {
            if ($("input:checked", '.datatable').length == 0)
               return;

            if (confirm('Do you really want to delete the selected users?\r\n This operation cannot be reverted')) {

               var ar_list = $('input:checked', '.datatable ');
               if (ar_list.length > 0) {
                  var ids = [];

                  ar_list.each(function(i, item) {
                     var tag = $(item).attr('tag');
                     if ((tag !== undefined) && (tag !== '')) {
                        ids.push(tag);
                     }
                  });

                  if (ids.length > 0) {
                     var data = { 'Ids': ids };
                     Proxy.invoke('DeleteUsers', data, function(result) {
                        for (var k = 0; k < ids.length; k++) {
                           $('#item_' + ids[k]).fadeOut('slow', function() {
                              $('#item_' + ids[k]).remove();
                           });
                        }
                        //                        $(".datatable").trigger("update");
                        //                        var pageIndex = 0;
                        //                        var pageSize = 30;

                        //                        Proxy.invoke('GetUsers', { 'pageIndex': pageIndex, 'pageSize': pageSize }, function(result) {
                        //                           $("#results").setTemplateURL("../templates/users_multitemplate.tpl");
                        //                           $("#results").processTemplate(result);
                        //                        });
                     });
                  }
               }
            }
            return false;
         })
      }
   </script>

</asp:Content>
