<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage(of adminVideosViewModel)" %>

<asp:Content ID="Content2" ContentPlaceHolderID="PageTitle" runat="server">
Videos
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="headerPlaceHolder" runat="server">
   <%=appHelpers.CssTagUrl("jqModal.css")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainSiteContent" runat="Server">
   <div style="border-bottom: 1px solid #eaeaea; padding: 5px; margin-bottom: 10px;">
      <h1 class="admin-title">
         Videos Listing</h1>
   </div>
   <%If Model.Videos Is Nothing Or Model.Videos.Count = 0 Then%>
   <div style="margin: 5px 0; padding: 20px; background-color: #fff; border: #bababa;
      color: #a5370a">
      <center>
         <b>No video found for the specified video, please try again later!</b>
      </center>
   </div>
   <% Else%>
   <div id="tools" style="padding: 5px; margin-top: 10px; background-color: #e7faea;">
      <input id="btnDeleteTop" type="button" value="Delete" title="Delete" disabled="disabled" />
   </div>
    <%=Html.AdvancedPager(Model.Videos, "Index", "VideoAdmin", New Integer() {10, 30, 50})%>
   <div id="jTemplate">
      <table class="edit-info datatable">
         <col align="center" style="width: 30px;" />
         <col />
         <col />
         <col />
         <col />
         <col />
         <col />
         <col style="width: 100px;" />
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
                  Duration
               </th>
               <th>
                  ID
               </th>
               <th align="left">
                  Approved
               </th>
               <th align="left">
                  Status
               </th>
               <th>
                  Command
               </th>
            </tr>
         </thead>
         <tbody>
            <% Dim idx As Integer = 0%>
            <% For Each it As Video In Model.Videos%>
            <tr class='videoitem <%= IIF(idx Mod 2 = 0,"even", "odd") %>' tag='<%= it.VideoID  %>'>
               <td>
                  <input type="checkbox" tag='<%=it.VideoID %>' class='cb_ar' />
               </td>
               <td>
                  <%=Html.Encode(it.Title)%>
               </td>
               <td>
                  <%=it.AddedBy%>
               </td>
               <td>
                  <%=routines.ToMinutesAndSeconds(it.Duration)%>
               </td>
               <td>
                  <%=it.VideoID%>
               </td>
               <td>
                  <%=IIf(it.Approved, "approved", "Not approved")%>
               </td>
               <td>
                  <%=it.AddedDate.ToString()%>
               </td>
               <td style="width: 40px;" align="center">
                  <a class="global" href='<%= url.Action("EditVideo",new with {.Id = it.VideoID}) %>'
                     title="Edit video">Edit</a>&nbsp;|&nbsp; <a class="global" href='<%= url.Action("UpdateVideoFile",new with {.Id = it.VideoID})%>'
                        title="Edit video">Update</a>
               </td>
            </tr>
            <% idx += 1%>
            <% Next%>
         </tbody>
      </table>
      <%=Html.AdvancedPager(Model.Videos, "Index", "VideoAdmin", New Integer() {10, 30, 50})%>
   </div>
   
   <% End If%>
   <div id="jqmVideos" class="jqmAlert jqmVideos" style="display: none;">
      <div class="jqmAlertWindow">
         <div class="jqmAlertTitle">
            <h1>
               Edit Video: -</h1>
            <a class="jqmClose" href="#"><em>Close</em> </a>
         </div>
         <div class="jqmAlertBody">
            <div class="clear" style="margin-bottom: 10px;">
               <div style="float: left; width: 180px; height: 120px; background-color: #fff; padding: 1px;
                  border: 1px solid #bababa">
                  <img class="vi" width="180px" height="120px" alt='video preview' id="vdImage" src="" />
               </div>
               <div style="float: left;">
                  <table style="width: 100%">
                     <tr>
                        <td>
                        </td>
                     </tr>
                     <tr>
                        <td>
                        </td>
                     </tr>
                     <tr>
                        <td>
                        </td>
                     </tr>
                  </table>
               </div>
            </div>
            <table cellspacing="1" class="edit-info">
               <col style="width: 100px" align="right" />
               <col />
               <tbody>
                  <tr>
                     <td>
                        Title:-
                     </td>
                     <td>
                        <input type="text" id="txtTitle" style="width: 300px" class="mtc" />
                     </td>
                  </tr>
                  <tr>
                     <td>
                        Abstract:-
                     </td>
                     <td>
                        <textarea id="txtDescription" rows="4" cols="1" style="width: 300px" class="mtac"></textarea>
                     </td>
                  </tr>
                  <tr>
                     <td>
                        Tags:-
                     </td>
                     <td>
                        <input type="text" id="txtTags" style="width: 300px" class="mtc" />
                     </td>
                  </tr>
               </tbody>
            </table>
         </div>
         <div style="text-align: center">
            <input type="button" id="btnSave" validationgroup="Save" title="Save" value="Save Changes" />
            <input type="button" id="btnCancel" title="Cancel" value="Cancel" />
         </div>
      </div>
   </div>

   <script type="text/html" id="table_template">
        {#foreach $T.d as data}
        <tr class="videoitem {#cycle values=['even','odd']}" tag="{$T.data.VideoID}">
            <td><input class='cb_ar' type='checkbox'/></td>
            <td>{$T.data.Title}</td>	
            <td>{$T.data.Author}</td>
            <td>{$T.data.Duration}</td>
            <td>{$T.data.VideoID}</td>
            <td>{$T.data.Approved}</td>
            <td>{$T.data.Uploaded}</td>
            <td><img style="border-width: 0px;" src='<%= ResolveUrl("~/_assets/images/MicroIcons/edit.png")%>' title="Edit article"/></td>
        </tr>
        {#/for}
   </script>

   <script type="text/html" id="item_template">
        <tr class="videoitem" tag="<#= VideoID #>">
            <td><input class='cb_ar' type='checkbox'/></td>
            <td><#= Title #></td>
            <td><#= Author #></td>
            <td><#= Duration #></td> 
            <td><#= VideoID #></td>
            <td><#= Approved #></td>
            <td><#= Uploaded #></td>
            <td><img style="border-width: 0px;" src="../_assets/images/MicroIcons/edit.png" title="Edit article"/></td>
        </tr>
   </script>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottomScripts" runat="Server">
   <%-- <script type="text/javascript" src=<%=ResolveUrl("~/_assets/scripts/jquery/jquery.tablesorter.min.js") %>'></script> 
    <script type="text/javascript" src=<%=ResolveUrl("~/_assets/scripts/jquery/jqmodal.js") %>'></script>
    <script type="text/javascript" src=<%=ResolveUrl("~/_assets/scripts/json2.js") %>'></script>
    <script type="text/javascript" src=<%=ResolveUrl("~/_assets/scripts/utils.js") %>'></script>
    <script type="text/javascript" src=<%=ResolveUrl("~/_assets/scripts/jquery/jquery.effects.js") %>'></script>
--%>
   <%=appHelpers.ScriptsTagUrl("jquery/jquery.tablesorter.min.js")%>
   <%=appHelpers.ScriptsTagUrl("jquery/jqmodal.js")%>
   <%=appHelpers.ScriptsTagUrl("json2.js")%>
   <%=appHelpers.AdvancedScriptsTagUrl("utils.js", MCC.Utils.FileVersion("utils"))%>
   <%=appHelpers.ScriptsTagUrl("jquery/jquery.effects.js")%>

   <script type="text/javascript">
      //<![CDATA[
      $(document).ready(function() {
         Init();
      });


      var Proxy = new serviceProxy('<%=ResolveUrl("~/asv.asmx/") %>');

      function Init() {
         $('.edit-info').tablesorter({
            widgets: ['zebra'],
            headers: {
               0: { sorter: false },
               7: { sorter: false }
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

         $(".even,.odd").hover(function() { $(this).addClass('highlight'); }, function() { $(this).removeClass('highlight'); });

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
         });

         $(".videoitem").dblclick(function() {
            var pk = $(this).attr("tag");
            editVideo($(this), parseInt(pk));
         }).hover(function() { $(this).addClass('hilight'); }, function() { $(this).removeClass('hilight'); });

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

      //                    alert('double key T pressed')
      //                    // optional - if we'd rather not detect a triple-press
      //                    // as a second double-press, reset the timestamp
      //                    thisKeypressTime = 0;
      //                }
      //                lastKeypressTime = thisKeypressTime;
      //            }
      //        }
      //        
      //        document.onkeypress = KeyHandler;

      var template;

      $('#jqmVideos').center();
      $('#jqmVideos').jqm({ overlay: 10 });


      $(document).ready(function() {
         InitHandlers();
         Init();
      });


      function InitHandlers() {
         $('#btnDelete,#btnDeleteTop').click(function() {
            if ($("input:checked", '.datatable').length == 0)
               return;

            if (confirm('Do you really want to delete the selected Videos?\r\n This operation cannot be reverted')) {

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
                     Proxy.invoke('DeleteVideos', data, function(result) {
                        for (var k = 0; k < ids.length; k++) {
                           $('#item_' + ids[k]).fadeOut('slow', function() {
                              $('#item_' + ids[k]).remove();
                           });
                        }
                        $(".datatable").trigger("update");


                        var pageIndex = 0;
                        var pageSize = 30;

                        Proxy.invoke('GetVideos', { 'pageIndex': pageIndex, 'pageSize': pageSize }, function(result) {


                        });

                     });
                  }
               }
            }
            return false;
         });


         $('#btnSave').click(function() {
            saveVideo();
         });

         $('#btnCancel').click(function() {
            hideCBx();
         });
         $('#btnCreate').click(function() {
            editvideo(this, -1);
         });

         $('#txtTitle').blur(function() {
            if ($(this).val() !== '') {
               $('#titleReqLabel').hide();
            }
         });

      }

      function onPageError(error) {
         $('#jqmVideos').jqmHide();
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
            $('#jTemplate').after($("<div class='video_status' style='display:none'><div>").attr('id', 'status_message').addClass(sClass).html(m).fadeIn('normal'));
         }

         setTimeout(function() {
            $('#status_message').fadeOut('normal', function() {
               $('#status_message').removeClass(sClass);
            });
         }, 4000);
      }


      var CateoryList;
      var videoId = null;
      var activevideo = null;


      function showCBx() {
         $('#jqmVideos').jqmShow();
      }

      function hideCBx() {
         $('#jqmVideos').jqmHide();
         ClearValidation();
      }

      function saveVideo(ctl, evt) {
         var jItem = $(ctl);

         var video = activeVideo;

         video.VideoID = videoId;
         if (videoId < 1)
            videoId = -1;

         if (isVideoValid()) {
            video.Title = $("#txtTitle").val();
            video.Tags = $("#txtTags").val();
            video.Abstract = $("#txtDescription").val();

            //showProgress();

            //            Proxy.SaveBook(book,
            //                   function(savedPk) {
            //                       showProgress(true);
            //                       showStatus("Book has been saved.", 5000);
            //                       $("#" + scriptVars.panEditBookId).hide();
            //                       book.Pk = savedPk;
            //                       updateBook(book, true);

            //                       if (bookPk == -1)
            //                           $("#divBookListWrapper").scrollTop(9999);
            //                   },
            //                   onPageError);

            var DTO = { 'video': video };

            Proxy.invoke('saveVideo', DTO, function(result) {
               updateVideo(video, true);
               showMessage('video successfully updated!');
               $("#jTemplate .datatable").trigger("update");
            }, onPageError);

         }
      }


      function updateVideo(video, highlight) {

         if (highlight === undefined) {
            highlight = false;
         }

         hideCBx();

         var item = $(".videoitem[tag=" + video.VideoID + "]");

         var html = parseTemplate($("#item_template").html(), video);

         var newItem = $(html).attr("tag", video.VideoID.toString()).click(function() {
            var pk = $(this).attr("tag");
            editVideo($(this), parseInt(pk));
         }).hover(function() { newItem.addClass('hilight'); }, function() { newItem.removeClass('hilight'); });

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




      function selectVideo() {
         var item = $(ctl);
         var id = item.attr("tag");

         for (var i = 0; i < bookList.length; i++) {

            item = videoList[i];
            if (item.videoID != id)
               continue;

            var video = scriptVars.blankCatgory;

            video.Title = item.Title;
            video.ImageUrl = item.ImageUrl;
            video.Tags = item.Tags;
            video.Abstract = item.Abstract;

            FillVideoInfo(video);
         }
      }

      function editVideo(ctl, id) {
         var jItem = $(ctl);

         if (id) {
            videoId = id;
         }
         else {
            videoId = parseInt(jItem.attr("tag"));
         }
         if (videoId < 1) {
            activeVideo = scriptVars.blankVideo;
            FillVideoInfo(activeVideo);
            showCBx();
         }
         else {

            var data = { 'id': videoId };
            Proxy.invoke('GetVideoByID', data, function(result) {
               if (result) {
                  var c = result;
                  FillVideoInfo(c);
                  showCBx();
               }
            }, onPageError);
         }
      }


      function FillVideoInfo(c) {
         activeVideo = c;
         $("#txtTitle").val(c.Title || "");
         $("#txtDescription").val(c.Abstract || "");
         $("#txtTags").val(c.Tags || "");
         $("#vdImage").attr('src', c.ImageUrl)
      }

      //var Proxy = new serviceProxy('<%=ResolveUrl("~/asv.asmx/") %>');

      function ClearValidation() {
         $('#titleReqLabel').hide();
      }

      function isVideoValid() {
         var isValid = true;
         var title = $('#txtTitle').val();
         if (title == '') {
            $('#titleReqLabel').show();
            isValid = false;
         }
         return isValid;
      }




      var scriptVars = {
         blankVideo: {
            "VideoID": 0,
            "Title": null,
            "ImageUrl": null,
            "Abstract": null,
            "Tags": null,
            "Type": null

         },

         Image1Id: "ctl00_Image1",
         MainContentId: "ctl00_MainContent"
      };
      //]]>


   </script>

</asp:Content>
