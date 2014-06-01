<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage(of imageListViewModel)" %>

<asp:Content ID="Content2" ContentPlaceHolderID="PageTitle" runat="server">
   Images
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="headerPlaceHolder" runat="Server">
   <%=appHelpers.CssTagUrl("jqModal.css")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainSiteContent" runat="Server">
   <%="" %>
   <div style="border-bottom: 1px solid #eaeaea; padding: 5px; margin-bottom: 10px;">
      <h1 class="admin-title">
         Images</h1>
   </div>
   <%If Model.Images Is Nothing Or Model.Images.Count = 0 Then%>
   <center>
      <b>- No images are currently in the database -</b>
   </center>
   <%Else%>
   <% If Model.ViewMode = Mode.Tile Then%>
   <div style="padding: 5px; margin: 3px 0; background-color: #eaeaea">
      switch to<a class='tool_lnk' href="<%= Url.Action("Index",new with {.mode = 0}) %>"
         title='grid'>grid</a> view
   </div>
   <%=Html.AdvancedPager(Model.Images, "Index", "ImageAdmin", New Integer() {10, 30, 50})%>
   <div id="im-blocks">
      <% For Each it As SimpleImage In Model.Images%>
      <div class="image">
         <div class="imbc">
            <a class="img-holder" href='<%= Url.Action("UpdateImage",new with {.Id = it.ImageID}) %>'>
               <img style="border: 0 none;" alt='<%= it.name  %>' src='<%= Configs.Paths.CdnRoot & "/imageThumb.ashx?auto=1&w=130&h=110&cache=0&img=" & it.imageUrl.UrlEncode() %>' />
            </a>
         </div>
         <div class="name">
            <span>
               <%=it.Name%></span>
         </div>
      </div>
      <% Next%>
   </div>
   <%=Html.AdvancedPager(Model.Images, "Index", "ImageAdmin", New Integer() {10, 30, 50})%>
   <% Else%>
   <div style="padding: 5px; margin: 4px 0; background-color: #eaeaea">
      switch to<a class='tool_lnk' title='thumbnail' href="<%= Url.Action("Index",new with {.mode = 1}) %>">Thumbnail</a>
      view
   </div>
   <%=Html.AdvancedPager(Model.Images, "Index", "ImageAdmin", New Integer() {10, 30, 50})%>
   <table id="jTemplate" class="edit-info datatable">
      <col width="20px;" />
      <col />
      <col />
      <col />
      <col />
      <col />
      <thead>
         <tr>
            <th>
               <input type="checkbox" class="im_cb" id="checkCol" />
            </th>
            <th>
               Name
            </th>
            <th>
               Description
            </th>
            <th>
               Type
            </th>
            <th>
               Credits
            </th>
            <th>
            </th>
         </tr>
      </thead>
      <tbody>
         <% Dim idx As Integer = 0%>
         <% For Each it As SimpleImage In Model.Images%>
         <tr id='<%="item_" & it.imageID %>' class='image <%= IIF(idx Mod 2 = 0,"even", "odd") %>'
            tag='<%=it.imageID%>'>
            <td>
               <input type="checkbox" class="im_cb" tag='<%=it.imageID%>' />
            </td>
            <td>
               <a class="lnk" href="<%= Url.Action("ShowImage",new with {.Id =it.ImageID}) %>" title="<%= html.encode(it.Name)%>">
                  <%= html.encode(it.Name)%></a>
            </td>
            <td>
               <%=it.Description%>
            </td>
            <td>
               <%=appHelpers.ImageType(it.ImageType)%>
            </td>
            <td>
               <%=it.CreditsName%>
            </td>
            <td>
               [<a id="A1" class="global" href='<%= Url.Action("UpdateImage",new with {.Id = it.ImageID}) %>'
                  title="Edit image">Edit</a>]&nbsp;|&nbsp;[<a id="A2" class="global" href='<%= url.Action("CreateThumbnails", new with {.Id = it.ImageID})%>'
                     title="Edit Image">Thumb</a>]
            </td>
         </tr>
         <% idx += 1%>
         <%Next%>
      </tbody>
   </table>
   <%=Html.AdvancedPager(Model.Images, "Index", "ImageAdmin", New Integer() {10, 30, 50})%>
   <div id="tools" style="padding: 5px; margin-top: 10px; background-color: #e7faea;">
      <input id="btnDelete" type="button" value="Delete" title="Delete" disabled="disabled" />
   </div>
   <% End If%>
   <%End If%>

   <script type="text/html" id="item_template">
        <tr id="item_<#= ImageID #>" class="image" tag="<#= ImageID #>">
            <td><input class='itcb' type='checkbox' tag="<#= ImageID #>"/></td>
            <td><#= Name #></td>
            <td><#= encode(Description) #></td>
            <td><#= Uploaded #></td>
            <td>Image</td>
            <td><#= Credits #></td>
        </tr>
   </script>

   <script type="text/html" id="table_template">
        {#foreach $T.d as data}
        <tr class="image {#cycle values=['even','odd']}" tag="{$T.data.ImageID}">
            <td><input class='itcb' type='checkbox'/></td>
            <td>{$T.data.Name}</td>
            <td>...</td>
            <td>{$T.data.Uploaded}</td>
            <td>Image</td>
            <td>{$T.data.Credits}</td>
        </tr>
        {#/for}
   </script>

   <script type="text/html" id="listitem_template">
        <li>
            <a class="img-holder" href="~/admin/images_add_edit?uuid="<#= Uuid #>'>
                <img  id="img_thumb" src='<#= ImageUrl #>' />
            </a>
            <p>
                <b>name:</b> <span>
                    <#= Name #></span>
            </p>
            <p>
                <b>Tags:</b>
                 <span><#= Tags #></span>
            </p>
            <p>
                <b>Credits:</b>
                <span><#= Credits #></span>
            </p>
       </li>
   </script>

   <script type="text/html" id="list_template">
       {#foreach $T.d as data}
       <li>         
            <a class="img-holder" href='<%=ResolveUrl("~/admin/images_add_edit") %>?uuid={$T.data.Uuid}'>
                <img  id="img_thumb" border='0' src='<%=Configs.Paths.CdnRoot & "/imageThumb.ashx" %>?uuid={$T.data.Uuid}&w=100&h=100' />
            </a>
            <p>
                {$T.data.Name}
            </p>
       </li>
       {#/for}
   </script>

   <%-- <div id="jTemplate_list">
        <ul id="im-blocks">
            <li>
                <img src="~/_assets/images/progress.gif" runat="server" alt="loading..." /></li>
        </ul>
    </div>--%>
   <div id="jqmImage" class="jqmAlert jqmImage" style="display: none;">
      <div class="jqmAlertWindow">
         <div class="jqmAlertTitle">
            <h1>
               Edit Image Info: -</h1>
            <a class="jqmClose" href="#"><em>Close</em> </a>
         </div>
         <div class="jqmAlertBody">
            <div id="jqmProgress" style="display: none; height: 100px;">
               <img src="<%= apphelpers.ImageUrl("progress.gif") %>" style="height: 32px; width: 32px;"
                  alt="loading" />
            </div>
            <div id="jqmContent" style="display: none;">
               <table class="imageEdit" style="width: 100%">
                  <col style="width: 75px;" />
                  <col />
                  <tbody>
                     <tr>
                        <td class="right" style="width: 75px;">
                           Preview
                        </td>
                        <td>
                           <div style="width: 150px; height: 80px; background-color: #fff; padding: 1px; border: 1px solid #bababa">
                              <img id="pImage" alt="preview" src="" style="width: 150px; height: 80px;" />
                           </div>
                        </td>
                     </tr>
                     <tr>
                        <td class="right" style="width: 75px;">
                           image type
                        </td>
                        <td>
                           <select id="imgType" class="admsel">
                              <option title="Undefined" value="0">Undefined</option>
                              <option title="Article Image" value="1">Article Image</option>
                              <option title="Video Still" value="2">Video Still</option>
                              <option title="Avatar" value="3">Avatar</option>
                           </select>
                        </td>
                     </tr>
                     <tr>
                        <td class="right">
                           <span>Credits Name</span>
                        </td>
                        <td>
                           <input id="txtCreditsName" type="text" style="width: 100%" class="mtc" />
                        </td>
                     </tr>
                     <tr>
                        <td class="right">
                           <span>Credits Url</span>
                        </td>
                        <td>
                           <input id="txtCreditsUrl" type="text" style="width: 100%" class="mtc" />
                        </td>
                     </tr>
                     <tr>
                        <td class="right">
                           <span>Description</span>
                        </td>
                        <td>
                           <input id="txtDescription" style="width: 100%" class="mtac" />
                        </td>
                     </tr>
                     <tr>
                        <td class="right">
                           <span>Tags</span>
                        </td>
                        <td>
                           <input id="txtTags" type="text" style="width: 100%" class="mtc" />
                        </td>
                     </tr>
                  </tbody>
               </table>
            </div>
         </div>
         <div style="text-align: center">
            <input type="button" id="btnSave" validationgroup="Save" class="rb-btn rb-save" onmouseover="this.className='rb-btn rb-save-hover'"
               onmouseout="this.className='rb-btn rb-save'" />
            <input type="button" id="btnCancel" class="rb-btn rb-cancel" onmouseover="this.className='rb-btn rb-cancel-hover'"
               onmouseout="this.className='rb-btn rb-cancel'" />
         </div>
      </div>
   </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottomScripts" runat="Server">
   <%=appHelpers.ScriptsTagUrl("jquery/jquery-jtemplates_uncompressed.js")%>
   <%=appHelpers.ScriptsTagUrl("global.js")%>
   <%--  <%=appHelpers.AdvancedScriptsTagUrl("utils.js", MCC.Utils.FileVersion("utils"))%>
   <%=appHelpers.ScriptsTagUrl("json2.js")%>--%>
   <%=appHelpers.ScriptsTagUrl("jquery/jquery.tablesorter.min.js")%>
   <%--   <%=appHelpers.ScriptsTagUrl("jquery/jqmodal.js")%>--%>
   <%=appHelpers.ScriptsTagUrl("jquery/jquery.effects.js")%>

   <script type="text/javascript">
      $(document).ready(function() {
         Init();
      });

      $('#jqmImage').center();
      $('#jqmImage').jqm({ overlay: 10 });


      var Proxy = new serviceProxy('/admin/images/');
      var imageId = null;
      var imageList = [];
      var template = null;
      var pageIndex = 0;
      var pageSize = 10;

      function Init() {
         //            template = $("#list_template").html();

         //            var DTO = { 'pageIndex': pageIndex, 'pageSize': pageSize }
         //            Proxy.invoke("FetchImages", DTO, function(result) {

         //                if (result) {

         //                    //  get the grid and its template
         //                    var grid = $('#jTemplate_list ul');
         //                    grid.setTemplate($('#list_template').html());
         //                    //  run the result set through the template ...
         //                    grid.processTemplate(result);
         //                }
         //            },
         //                              onPageError);


         template = $("#list_template").html();

         //            $.ajax({
         //                type: 'POST',
         //                url: '<%= ResolveUrl("~/asv.asmx/FetchImages") %>',
         //                data: '{"pageIndex":' + pageIndex + ',"pageSize" : +' + pageSize + '}',
         //                contentType: 'application/json; charset=utf-8',
         //                dataType: 'json',
         //                success: function(result) {

         //                    if (result.d && result.d.length > 0) {

         //                        //  get the grid and its template
         //                        var grid = $('#jTemplate_list ul');
         //                        grid.setTemplate($('#list_template').html());
         //                        //  run the result set through the template ...
         //                        grid.processTemplate(result);
         //                    }
         //                }
         //            });


         $('.datatable').tablesorter({
            widgets: ['zebra'],
            headers: {
               0: { sorter: false },
               6: { sorter: false }
            }
         });

         $('#btnSave').click(function() {
            saveImage();
         });

         $('#btnCancel').click(function() {
            hideCBx();
         });

         $(":checkbox", '.datatable').click(function(ev) {
            var e = $.event.fix(ev);
            e.stopPropagation();

            if ($(this).attr('id') == 'checkCol') {
               var cb = $(this).is(':checked');
               $("input:checkbox", '.datatable').attr('checked', cb).parent().parent().toggleClass('sel');
            }
            else {

               $(this).parent().parent().toggleClass('sel')
            }


            if ($("input:checked", '.datatable').length > 0) {
               $('#btnDelete,#btnDeleteTop').attr('disabled', false); //.removeClass('rb-delete-disable').addClass('rb-delete');
            }
            else {
               $('#btnDelete,#btnDeleteTop').attr('disabled', true); //.removeClass('rb-delete').addClass('rb-delete-disable');
            }
         });

         $(".even,.odd").hover(function() { $(this).addClass('hilight'); }, function() { $(this).removeClass('highlight'); });

         $('#btnDelete,#btnDeleteTop').click(function() {
            if (confirm('Do you really want to delete the selected images?\r\n This operation cannot be reverted')) {

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
                     Proxy.invoke('DeleteImages', data, function(result) {
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

         $("tr.image").dblclick(function() {
            var pk = $(this).attr("tag");
            editImage($(this), parseInt(pk));
         }).hover(function() { $(this).addClass('hilight'); }, function() { $(this).removeClass('hilight'); });

         $("div.image").mouseover(function() {
            $('.del', $(this)).show();
         }).mouseout(function() {
            $('.del', $(this)).hide();
         });
      }

      function onPageError(error) {
         $('#jqmArticleCategory').jqmHide();
         showMessage(error.Message, false);
      }

      function showMessage(m, s) {
         if (s === undefined) {
            s = true;
         }

         var sClass = s ? 'success' : 'failure';
         if ($('#status_message').length > 0) {
            $('#status_message').html(m).addClass(sClass).fadeIn('normal');
         }
         else {
            $('.datatable').after($("<div class='category_status' style='display:none'><div>").attr('id', 'status_message').addClass(sClass).html(m).fadeIn('normal'));
         }

         setTimeout(function() {
            $('#status_message').fadeOut('normal', function() {
               $('#status_message').removeClass(sClass);
            });
         }, 4000);
      }


      function showProgress(s) {
         if (s) {
            $('#jqmContent').hide();
            $('#jqmProgress').show();
         }
         else {
            $('#jqmProgress').hide();
            $('#jqmContent').show();
         }
      }

      function editImage(ctl, id) {
         var jItem = $(ctl);
         if (id) {
            imageId = id;
         }
         else {
            imageId = parseInt(jItem.attr("tag"));
         }
         if (imageId < 1) {
            activeImage = scriptVars.blankImage;
            FillImageInfo(activeImage);
            showCBx();
         }
         else {

            var el = selectImage(imageId)

            if (el.ImageID > 0) {
               FillImageInfo(el);
               showProgress(false);
               showCBx();
            }
            else {
               var data = { 'Id': imageId };
               showProgress(true);
               showCBx();
               Proxy.invoke('EditImage', data, function(result) {
                  if (result) {
                     var c = result;
                     FillImageInfo(c);

                     for (var k = 0; k < imageList.length; k++) {
                        if (imageList[k].ImageID == c.ImageID) {
                           imageList.pop(k);
                           break;
                        }
                     }
                     imageList.push(c);
                     showProgress(false);
                  }
               }, onPageError);
            }
         }
      }

      function saveImage(ctl, evt) {
         var jItem = $(ctl);

         var image = activeImage;

         image.ImageID = imageId;
         if (imageId < 1)
            imageId = -1;

         if (isImageValid()) {
            image.CreditsName = $("#txtCreditsName").val();
            image.CreditsUrl = $("#txtCreditsUrl").val();
            image.Tags = $("#txtTags").val();
            image.Description = $("#txtDescription").val();
            image.Type = parseInt($("#imgType").val());

            //var DTO = { 'image': image };
            var DTO = image;

            Proxy.invoke('SaveImage', DTO, function(result) {
               updateImage(image, true);
               showMessage('Image successfully updated!');
               $("#jTemplate .datatable").trigger("update");
            }, onPageError);

         }
      }


      function updateImage(image, highlight) {
         if (highlight === undefined) {
            highlight = false;
         }

         hideCBx();

         var item = $(".image[tag=" + image.ImageID + "]");

         var html = parseTemplate($("#item_template").html(), image);

         var newItem = $(html).attr("tag", image.ImageID.toString()).click(function() {
            var pk = $(this).attr("tag");
            editImage($(this), parseInt(pk));
         }).hover(function() { newItem.addClass('highlight'); }, function() { newItem.removeClass('highlight'); });

         if (item.length > 0) {
            newItem.attr('class', item.attr('class'));
            item.after(newItem).remove();
         }
         else {
            newItem.appendTo($("#jTemplate .datatable tbody"));
         }

         if (highlight) {
            var bgc = newItem.css('background-color');
            newItem.css('backgroundColor', '#dcffd4');
            setTimeout(function() { newItem.animate({ 'backgroundColor': bgc }) }, 3000);
         }
      }

      function selectImage(id) {
         if (id > 0) {
            for (var i = 0; i < imageList.length; i++) {
               if (imageList[i].ImageID == id) {
                  return imageList[i];
               }
            }

            return scriptVars.blankImage;
         }
         else {
            return scriptVars.blankImage;
         }
      }

      function FillImageInfo(c) {
         activeImage = c;
         $('#pImage').attr('src', c.ImageUrl).attr('alt', c.Description);
         $("#txtDescription").val(c.Description || "");
         $("#txtCreditsName").val(c.CreditsName || "");
         $("#txtCreditsUrl").val(c.CreditsUrl || "");
         $("#txtTags").val(c.Tags || "");
         $("#imgType").val(c.Type || "0");
      }


      function EmptyInfo() {
         $('#pImage').attr('src', '');
         $("#txtDescription").val("");
         $("#txtCreditsName").val("");
         $("#txtCreditsUrl").val("");
         $("#txtTags").val("");
         $("#imgType").val("0");
      }


      function showCBx() {

         $('#jqmImage').jqmShow();
      }

      function hideCBx() {
         $('#jqmImage').jqmHide();
         EmptyInfo();
      }

      function isImageValid() {
         return true;
      }

   </script>

   <script type="text/javascript">
      //<![CDATA[
      var scriptVars = {
         blankImage: {
            "ImageID": 0,
            "Name": null,
            "Description": null,
            "CreditsName": null,
            "CreditsUrl": null,
            "Tags": null,
            "Type": null
         },

         Image1Id: "ctl00_Image1",
         MainContentId: "ctl00_MainContent"
      };
      //]]>
   </script>

</asp:Content>
