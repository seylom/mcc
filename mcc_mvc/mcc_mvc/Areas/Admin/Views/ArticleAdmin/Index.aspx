<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage(of AdminArticleslistViewModel)" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
   Articles
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainSiteContent" runat="Server">
   <div style="border-bottom: 1px solid #eaeaea; padding: 5px; margin-bottom: 10px;">
      <h1 class="admin-title">
         Articles</h1>
   </div>
   <% If Model.Articles Is Nothing Or Model.Articles.Count <= 0 Then%>
   <center>
      <b>- No articles are currently in the database -</b>
   </center>
   <% Else%>
   <div style="padding: 5px; margin: 10px 0; background-color: #e7faea;">
      <input type="button" id="btnDeleteTop" disabled="disabled" value="Delete" title="Delete" />&nbsp;
      <a id="A1" href="<%= url.action("AddEditArticle") %>" class="global">Add Article</a>
   </div>
   <%=Html.AdvancedPager(Model.Articles, "Index", "ArticleAdmin", New Integer() {10, 30, 50})%>
   <table class="edit-info datatable">
      <col align="center" width="20px;" />
      <col />
      <col />
      <col />
      <col />
      <col />
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
               Status
            </th>
            <th>
               Release Date
            </th>
            <th>
               Expire Date
            </th>
            <th>
               ImageID
            </th>
            <th>
            </th>
         </tr>
      </thead>
      <tbody>
         <% Dim idx As Integer = 0%>
         <% For Each it As Article In Model.Articles%>
         <tr id='<%="item_" & it.ArticleID %>' class='<%= IIF(idx Mod 2 = 0,"even", "odd") %>'
            tag='<%= it.ArticleID %>' style="padding:5px 0">
            <td align="center">
               <input type="checkbox" tag='<%= it.ArticleID %>' class='cb_ar' />
            </td>
            <td>
               <a class="lnk" title='<%= Html.Encode(it.Title) %>' href='<%= url.Action("AddEditArticle",new with {.controller="ArticleAdmin",.Id = it.ArticleID}) %>'>
                  <%= Html.Encode(it.Title) %></a> <span>(<a class="peek" title="preview" href='<%=Url.Action("PeekArticles",new with {.controller = "ArticleAdmin",.Id= it.ArticleID}) %>'>Preview</a>)</span>
            </td>
            <td>
               <%=it.AddedBy%>
            </td>
            <td>
               <span class='<%= "notap st_" & it.Status %>'>
                  <%=Utils.GetStatusCaption(it.Status)%></span>
            </td>
            <td>
               <%=it.ReleaseDate.ToString()%>
            </td>
            <td>
               <%=it.ExpireDate.ToString()%>
            </td>
            <td>
               <%= it.ImageID %>
            </td>
            <td>
            </td>
         </tr>
         <%Next%>
      </tbody>
   </table>
   <%=Html.AdvancedPager(Model.Articles, "Index", "ArticleAdmin", New Integer() {10, 30, 50})%>
   <div id="tools" style="padding: 5px; margin-top: 10px; background-color: #e7faea;">
      <input id="btnDelete" type="button" disabled="disabled" value="Delete" title="Delete" />
   </div>
   <% End If%>
   <%--<script type="text/html" id="table_template">
        {#foreach $T.d as data}
        <tr class="id="item_{$T.data.ArticleID}" {#cycle values=['even','odd']}" tag="{$T.data.ArticleID}">
            <td><input type='checkbox' class="ar_cb"/></td>
            <td>{$T.data.ArticleID}</td>	
            <td>{$T.data.Title}</td>
            <td>...</td>
            <td>{$T.data.Importance}</td>
            <td>{$T.data.ImageUrl}</td>
            <td>{$T.data.ParentCategoryID}</td>
        </tr>
        {#/for}
    </script>--%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottomScripts" runat="Server">
   <%=appHelpers.ScriptsTagUrl("jquery/jquery.tablesorter.min.js")%>
   <%=appHelpers.ScriptsTagUrl("json2.js")%>
   <%=appHelpers.AdvancedScriptsTagUrl("utils.js", MCC.Utils.FileVersion("utils"))%>
   <%=appHelpers.ScriptsTagUrl("jquery/jquery.effects.js")%>

   <script type="text/javascript">

      $(document).ready(function() {
         Init();
      });

      var Proxy = new serviceProxy("/admin/articles/");

      function updateStripState() {

         if ($("input:checked", '.datatable').length > 0) {
            $('#btnDelete,#btnDeleteTop').removeClass('disabled');
         }
         else {
            $('#btnDelete,#btnDeleteTop').addClass('disabled');
         }
      }

      function Init() {
         $('.edit-info').tablesorter({
            widgets: ['zebra'],
            headers: {
               0: { sorter: false },
               6: { sorter: false }
            }
         });

         updateStripState();

         $(":checkbox", '.datatable').click(function(ev) {
            var e = $.event.fix(ev);
            e.stopPropagation();

            if ($(this).attr('id') == 'checkCol') {
               var cb = $(this).is(':checked');
               $("input:checkbox", '.datatable').attr('checked', cb);
            }

            //            if ($("input:checked", '.datatable').length > 0) {
            //               $('#btnDelete,#btnDeleteTop').removeClass('disabled');
            //            }
            //            else {
            //               $('#btnDelete,#btnDeleteTop').addClass('disabled');
            //            }

            updateStripState();
         });

         $(".even,.odd").hover(function() { $(this).addClass('hilight'); }, function() { $(this).removeClass('hilight'); });

         $('#btnDelete,#btnDeleteTop').click(function() {
            if ($("input:checked", '.datatable').length == 0)
               return;

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
                        $(".datatable").trigger("update");


                        var pageIndex = 0;
                        var pageSize = 30;

                        Proxy.invoke('GetArticles', { 'pageIndex': pageIndex, 'pageSize': pageSize }, function(result) {


                        });
                     });
                  }
               }
            }
            return false;
         })
      }

      var options =
        {
           autoOpen: false,
           width: '700px',
           modal: true,
           resizable: false,
           draggable: false,
           position: 'center',
           title: 'this is our new title',
           overlay: { 'background-color': '#ffffff', 'opacity': '.50' },
           buttons: {}
        };

      //$("#peek_window").dialog(options);

      function DisplayContent(rs) {

         //var result = 

         $("#it_body")[0].innerHTML = rs;
         $("#peek_window").dialog('open');
         //$("#it_body").removeClass("loading");
      }


      //        var first_press = false;
      //        function key_press() {
      //            if (first_press) {
      //                // they have already clicked once, we have a double
      //                //do_double_press();
      //                alert('key pressed twice')
      //                first_press = false;
      //            } else {
      //                // this is their first key press
      //                first_press = true;

      //                // if they don't click again in half a second, reset
      //                window.setTimeout(function() { first_press = false; }, 200);
      //            }
      //        }

      //        var delta = 200;
      //        var lastKeypressTime = 0;
      //        function KeyHandler(event) {
      //            if (String.fromCharCode(event.charCode).toUpperCase() == 'T') {
      //                var thisKeypressTime = new Date();
      //                if (thisKeypressTime - lastKeypressTime <= delta) {
      //                    //doDoubleKeypress();
      //                    
      //                    alert('double key T pressed')
      //                    // optional - if we'd rather not detect a triple-press
      //                    // as a second double-press, reset the timestamp
      //                    thisKeypressTime = 0;
      //                }
      //                lastKeypressTime = thisKeypressTime;
      //            }
      //        }




      //        document.onkeypress = KeyHandler;


      
  


   </script>

</asp:Content>
