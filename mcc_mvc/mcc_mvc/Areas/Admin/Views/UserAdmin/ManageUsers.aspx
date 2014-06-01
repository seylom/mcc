<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage(of adminUsersViewModel)" %>

<%@ Import Namespace="Webdiyer.WebControls.Mvc" %>
<asp:Content ID="Content2" ContentPlaceHolderID="PageTitle" runat="server">
   Manage Users
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="mainSiteContent" runat="Server">
   <%=""%>
   <div style="border-bottom: 1px solid #eaeaea; padding: 5px; margin-bottom: 10px;">
      <h1 class="admin-title">
         Users</h1>
   </div>
   <% Using Html.BeginForm()%>
   <table class="edit-info">
      <tr>
         <th>
            Find Users
         </th>
      </tr>
      <tr>
         <td>
            <% For Each it As Char In Model.AlphabetItems%>
            <a href="#">
               <%=It.ToString %></a>&nbsp;
            <% Next%>
         </td>
      </tr>
      <tr>
         <td>
            <%= Html.DropDownList("FilterType",CType(Model.FilterType,UserFilterType).ToSelectList) %>
            <%=Html.TextBox("SearchKey", "", New With {.class = "rtc"})%>&nbsp;
            <input type="submit" value="search" />
         </td>
      </tr>
   </table>
   <% End Using%>
   <% If Model.Users Is Nothing Or Model.Users.Count = 0 Then%>
   <div style="margin: 10px 0; padding: 10px; border: 1px solid orange; background-color: #f7e2b5">
      No user found matching the search criteria!
   </div>
   <% Else%>
  <%=Html.AdvancedPager(Model.Users, "ManageUsers", "UserAdmin", New Integer() {100, 200, 500})%>
   <div id="Div1" style="padding: 5px; margin: 10px 0; background-color: #e7faea;">
      <input type="button" id="btnDeleteTop" disabled="disabled" value="Delete" title="Delete" />
   </div>
   <div id="results">
      <table class="edit-info datatable" width="100%;" style="margin-top: 10px;">
         <col align="center" />
         <col align="left" />
         <col align="right" />
         <col align="right" />
         <col align="right" />
         <col align="right" />
         <col align="center" />
         <col align="center" />
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
                  Edit
               </th>
               <th>
               </th>
            </tr>
         </thead>
         <tbody>
            <%Dim idx As Integer = 0%>
            <%For Each it As SiteUser In Model.Users%>
            <tr id='item_<%= it.Username  %>' class='<%= IIF(idx Mod 2 = 0,"even","odd") %>'>
               <td>
                  <input type="checkbox" tag='<%= it.Username  %>' class='cb_ar scb' />
               </td>
               <td>
                  <%=it.Username%>
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
                  <%=it.isApproved%>
               </td>
               <td style="width: 30px;">
                  <a href='<%= Url.Action("EditUser",new with {.username = it.Username}) %>' class="lnk"
                     title="Edit User">Edit</a>
                  <%--  <%=Html.ActionLink("Delete", "DeleteUser", New With {.username = it.Username}, New With {.class = "lnk"})%>--%>
               </td>
               <td style="width: 25px;">
                  <% Using Html.BeginForm("DeleteUser", "UserAdmin", New With {.username = it.Username})%>
                  <input type="image" src="<%= apphelpers.ImageUrl("MicroIcons/delete.png") %>" onclick="if (confirm('Are you sure you want to delete this ticket change?') == false) return false;" />
                  <%End Using%>
               </td>
            </tr>
            <% idx += 1%>
            <% Next%>
         </tbody>
      </table>
   </div>
   <% End If%>
   <%=Html.AdvancedPager(Model.Users, "ManageUsers", "UserAdmin", New Integer() {100, 200, 500})%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottomScripts" runat="server">
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
