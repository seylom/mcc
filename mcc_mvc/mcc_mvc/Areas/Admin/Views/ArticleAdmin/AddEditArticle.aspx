<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage(of AddArticleViewModel)" %>

<asp:Content ID="Content4" ContentPlaceHolderID="PageTitle" runat="server">
   Add Edit Article
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="headerPlaceHolder" runat="server">
   <%= appHelpers.CssTagUrl("datepicker.css") %>
   <%= appHelpers.CssTagUrl("jqModal.css") %>
   <%= appHelpers.CssTagUrl("imageBrowser.css") %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainSiteContent" runat="Server">
   <div style="border-bottom: 1px solid #eaeaea; padding: 5px; margin-bottom: 10px;">
      <h1 class="admin-title">
         Add Article</h1>
   </div>
   <div style="padding: 4px;">
      <%=Html.DisplayError(Model)%>
      <% Using Html.BeginForm()%>
      <%=Html.Hidden("ArticleID", Model.ArticleID)%>
      <div id="tabs">
         <div id="tblks">
            <div class="tb_lk">
               <a id="A1" href='#info_basic'>Basic info</a>
            </div>
            <div class="tb_lk">
               <a id="A2" href='#info_scheduling'>Scheduling</a>
            </div>
            <div class="tb_lk">
               <a id="A3" href='#info_images'>Images</a>
            </div>
            <div class="tb_lk">
               <a id="A5" href='#info_categories'>Categories</a>
            </div>
            <div class="tb_lk">
               <a id="A4" href='#info_attributes'>Attributes</a>
            </div>
         </div>
         <div id="info_basic" class="tab">
            <table class="edit-info" cellspacing="1">
               <tr>
                  <td class="right" style="width: 150px;">
                     City:
                  </td>
                  <td class="left">
                     <%=Html.TextBox("City", Model.City, New With {.class = "rtc", .style = "width:99%"})%>
                  </td>
               </tr>
               <tr>
                  <td class="right">
                     State:
                  </td>
                  <td class="left">
                     <%=Html.TextBox("State", Model.State, New With {.class = "rtc", .style = "width:99%"})%>
                  </td>
               </tr>
               <tr>
                  <td class="right">
                     Country:
                  </td>
                  <td class="left">
                     <%=Html.DropDownList("Country", Model.Countries, New With {.class = "ctr"})%>
                  </td>
               </tr>
               <tr>
                  <td class="right">
                     Listed:
                  </td>
                  <td class="left">
                     <%=Html.CheckBox("Listed", Model.Listed)%>
                  </td>
               </tr>
               <tr>
                  <td class="right">
                     Only for members:
                  </td>
                  <td class="left">
                     <%=Html.CheckBox("OnlyForMembers", Model.OnlyForMembers)%>
                  </td>
               </tr>
               <tr>
                  <td class="right">
                     Comments Enabled:
                  </td>
                  <td class="left">
                     <%=Html.CheckBox("CommentsEnabled", Model.CommentsEnabled)%>
                  </td>
               </tr>
               <tr>
                  <td class="right">
                     Approved:
                  </td>
                  <td class="left">
                     <%=Html.CheckBox("Approved", Model.Approved)%>
                  </td>
               </tr>
               <tr>
                  <td class="right">
                     Author:
                  </td>
                  <td class="left">
                     <%=Html.DropDownList("AddedBy", Model.Authors, New With {.class = "ctr"})%>
                  </td>
               </tr>
            </table>
         </div>
         <div id="info_scheduling" class="tab" style="display: none">
            <table class="edit-info" cellspacing="1">
               <col width="100px" />
               <col />
               <tbody>
                  <tr style="">
                     <td class="right">
                        Release Date:
                     </td>
                     <td class="left">
                        <%=Html.TextBox("ReleaseDate", Model.ReleaseDate, New With {.class = "rtc date-pick", .style = "width:80%"})%>
                     </td>
                  </tr>
                  <tr>
                     <td class="right">
                        Expire Date:
                     </td>
                     <td class="left">
                        <%=Html.TextBox("ExpireDate", Model.ExpireDate, New With {.class = "rtc date-pick", .style = "width:80%"})%>
                     </td>
                  </tr>
               </tbody>
            </table>
         </div>
         <div id="info_images" class="tab" style="display: none">
            <%=Html.Hidden("ImageID", Model.ImageID)%>
            <table class="edit-info">
               <col width="150px" />
               <col />
               <tbody>
                  <tr>
                     <td class="right">
                        Large - (400x200)
                     </td>
                     <td class="left">
                        <%=Html.TextBox("ImageNewsUrl", Model.ImageNewsUrl, New With {.class = "rtc", .style = "width:90%"})%>
                        <a id="lnkImage" href="javascript:void(0);" class="browselnk"></a>
                     </td>
                  </tr>
                  <tr>
                     <td class="right">
                        Mini - (80x80)
                     </td>
                     <td class="left">
                        <%=Html.TextBox("ImageIconUrl", Model.ImageIconUrl, New With {.class = "rtc", .style = "width:90%"})%>
                        <a id="lnkIcon" href="javascript:void(0);" class="browselnk"></a>
                     </td>
                  </tr>
               </tbody>
            </table>
         </div>
         <div id="info_categories" class="tab" style="display: none">
            <% For Each it As ArticleCategory In Model.Categories%>
            <div style="margin: 2px 0;">
               <%=CreateCheckBox("CategoryIds", it.CategoryID, If(Model.CategoryIds IsNot Nothing, Model.CategoryIds.Contains(it.CategoryID), False), Nothing)%>&nbsp;<%=it.Title%>
            </div>
            <%Next%>
         </div>
         <div id="info_attributes" class="tab" style="display: none">
            <table class="edit-info">
               <col width="150px" />
               <col />
               <tbody>
                  <tr>
                     <td class="right">
                        Poll:
                     </td>
                     <td class="left">
                        <%= Html.DropDownList("PollID",Model.Polls, New With {.class = "ctr"})%>
                     </td>
                  </tr>
                  <tr>
                     <td class="right">
                        Video:
                     </td>
                     <td class="left">
                        <a href='' class="stvdi"></a>
                        <%=Html.TextBox("VideoID", Model.VideoId, New With {.class = "rtc", .style = "width:50px"})%>
                        <a id="lnkVideoPick" href="javascript:void(0);" class="browselnk"></a>
                     </td>
                  </tr>
               </tbody>
            </table>
         </div>
      </div>
      <div style="margin-top: 10px; padding: 5px 10px; background-color: #f1f1f1">
         <h2 style="font-size: 10px; font-weight: bold; margin-bottom: 5px;">
            <a href='javascript:void(0);' class="global" onclick="InfoSetup();return false;">Article
               information</a></h2>
         <div id="info_window">
            <table class="edit-info" cellspacing="1">
               <tr>
                  <td class="right" style="width: 100px">
                     Tags:
                  </td>
                  <td class="left">
                     <input type="hidden" id="Tags" name="Tags" value='<%=Model.Tags %>' />
                     <input type="text" id="InputTags" style="width: 200px;" class="rtc" value="" />
                     <input type="button" id="addTags" value="Add" /><span>[<a id='editTags' href='javascript:void(0);'
                        title='Edit'>Edit</a>]</span>
                     <div id="result_list" style="display: none;">
                     </div>
                     <div style="padding: 5px 0; border: 1px dotted #bababa; margin-top: 5px;">
                        your tags:<span id="TagItems"></span>
                     </div>
                  </td>
               </tr>
               <tr>
                  <td class="right">
                     Title:
                  </td>
                  <td class="left">
                     <%=Html.TextBox("Title", Model.Title, New With {.class = "rtc", .style = "width:90%"})%>
                  </td>
               </tr>
               <tr>
                  <td colspan="2">
                     <input type="submit" value="" class="rb-btn rb-save" />
                  </td>
               </tr>
            </table>
         </div>
      </div>
      <div style="margin-top: 10px; padding: 5px 10px; background-color: #f1f1f1">
         <h2 style="font-size: 10px; font-weight: bold; margin-bottom: 5px;">
            <a href='javascript:void(0);' class="global" onclick="BodySetup();return false;">Article
               core (abstract|body)</a></h2>
         <div id="body_window" style="display: none;">
            <table class="edit-info" cellspacing="1">
               <tr>
                  <th>
                     abstract ( Min: 300 | Max : 4000 )
                  </th>
               </tr>
               <tr>
                  <td class="pn">
                     <span>chars count: </span>
                     <input type="text" readonly="readonly" id="lblCharUsed" value="0" style="width: 50px" />
                     <span>| words count</span>
                     <input type="text" id="lblWordUsed" style="width: 50px;" value="0" readonly="readonly" />
                     <span>| Remaining: </span>
                     <input type="text" value="4000" style="width: 50px;" readonly="readonly" id="lblCharLeft" />
                  </td>
               </tr>
               <tr>
                  <td class="pn">
                     <%=Html.TextArea("Abstract", Model.Abstract, New With {.style = "width:100%", .rows = 5, .cols = 12, .class = "rq-red"})%>
                  </td>
               </tr>
            </table>
            <table class="edit-info">
               <tr>
                  <td class="pn">
                     <%=Html.TextArea("Body", Model.Body, 5, 12, New With {.style = "width:100%;height:400px;", .class = "txtBody"})%>
                  </td>
               </tr>
               <tr>
                  <td class="pn">
                     <input type="submit" value="" class="rb-btn rb-save" />
                  </td>
               </tr>
            </table>
         </div>
      </div>
      <div style="padding: 5px 10px; background-color: #f1f1f1; margin-top: 10px;">
         <h2 style="font-size: 10px; font-weight: bold; margin-bottom: 5px;">
            <a href='javascript:void(0);' class="global" onclick="AdSetup();return false;">Article
               Ads for article:-</a></h2>
         <div id="ad_window" style="display: none;">
            <h2 style="font-size: 10px; font-weight: bold; margin-bottom: 10px;">
            </h2>
            <table class="edit-info" width="50%">
               <% Dim idx As Integer = 0%>
               <%For Each it As Ad In Model.Ads%>
               <tr class='<%= IIF(idx Mod 2 = 0,"even", "odd") %>'>
                  <td style="width: 20px; padding: 4px;" align="center">
                     <%=CreateCheckBox("AdIds", it.AdID, If(Model.AdIds IsNot Nothing, Model.AdIds.Contains(it.AdID), False), Nothing)%>
                  </td>
                  <td>
                     <a href="javascript:void(0);" onclick="previewAd('<%= it.AdId %>');return false;"
                        style="font-size: 9px; color: #5588a9">
                        <%=it.Title%></a>
                  </td>
               </tr>
               <% idx += 1%>
               <% Next%>
            </table>
         </div>
      </div>
      <%End Using%>
      <div id="jqmAdInfo" class="jqmAlert" style="display: none;">
         <div class="jqmAlertWindow">
            <div class="jqmAlertTitle">
               <h1 id="jqmAdTitle">
                  Ad preview</h1>
               <a class="jqmClose" href="#"><em>Close</em> </a>
            </div>
            <div id="jqmAdBody" class="jqmAlertBody">
            </div>
         </div>
      </div>
      <div id="jqmTagInfo" class="jqmAlert jqmTagInfo" style="display: none;">
         <div class="jqmAlertWindow">
            <div class="jqmAlertTitle jqDrag">
               <h1 id="H1">
                  Add tags to article</h1>
               <a class="jqmClose" href="#"><em>Close</em> </a>
            </div>
            <div id="jqmTagBody" class="jqmAlertBody">
               <table cellspacing="1" width="100%">
                  <tr>
                     <td valign="top" class="right" style="width: 50px;">
                        Tags:
                     </td>
                     <td>
                        <textarea id="editTagsBx" cols="1" rows="5" style="width: 100%">
                                where are the better days?
                        </textarea>
                     </td>
                  </tr>
               </table>
               <div class="command" style="text-align: right;">
                  <input type="button" id="btnSaveTags" value="Save Tags" />
                  <input type="button" id="btnCancelTags" value="Cancel" />
               </div>
               <div class="progress" style="display: none; padding-right: 25px; text-align: right;
                  vertical-align: baseline">
                  Saving...
               </div>
            </div>
         </div>
      </div>
   </div>
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="bottomScripts">
   <%= appHelpers.ScriptsTagUrl("jquery/jquery.datePicker.min-2.1.2.js") %>
   <%= appHelpers.ScriptsTagUrl("date.pack.js") %>
   <%= appHelpers.ScriptsTagUrl("global.js") %>
   <%= appHelpers.ScriptsTagUrl("jquery/jqDnR.js") %>
   <%--  <%=appHelpers.AdvancedScriptsTagUrl("utils.js", MCC.Utils.FileVersion("utils"))%>--%>
   <%=appHelpers.LocalScriptsTagUrl("tinymce/jscripts/tiny_mce/tiny_mce_gzip.js")%>

   <script type="text/javascript">

      $(document).ready(function() {
         InitHandlers();
         $('.date-pick').datePicker();
         InitTabs();
         $('#jqmTagInfo').jqDrag('.jqDrag');
         initTagsList();
      });

      var _editorIsLoaded = false;

      function LoadEditor() {
         tinyMCE_GZ.init({
            plugins: 'style,layer,table,save,advhr,advimage,advlink,emotions,iespell,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras',
            themes: 'simple,advanced',
            languages: 'en',
            disk_cache: true,
            debug: false
         }, function() {
            tinyMCE.init({
               mode: "exact",
               theme: "advanced",
               skin: "o2k7",
               skin_variant: "black",
               elements: "Body",
               plugins: "safari,pagebreak,style,layer,table,save,advhr,advimage,advlink,emotions,iespell,inlinepopups,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template",

               // Theme options
               theme_advanced_buttons1: "save,newdocument,|,bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,|,styleselect,formatselect,fontsizeselect",
               theme_advanced_buttons2: "cut,copy,paste,pastetext,pasteword,|,search,replace,|,bullist,numlist,|,outdent,indent,blockquote,|,undo,redo,|,link,unlink,anchor,image,cleanup,help,code,|,insertdate,inserttime,preview,|,forecolor,backcolor",
               theme_advanced_buttons3: "tablecontrols,|,hr,removeformat,visualaid,|,sub,sup,|,charmap,emotions,iespell,media,advhr,|,print,|,ltr,rtl,|,fullscreen",
               theme_advanced_buttons4: "insertlayer,moveforward,movebackward,absolute,|,styleprops,|,cite,abbr,acronym,del,ins,attribs,|,visualchars,nonbreaking,template,pagebreak,|,insertimage",
               theme_advanced_toolbar_location: "top",
               theme_advanced_toolbar_align: "left",
               theme_advanced_statusbar_location: "bottom",
               theme_advanced_resizing: true,
               convert_urls: false,

               //imagemanager_relative_urls:false,
               imagemanager_no_host: true,
               //urlconvertor_callback: "convLinkVC",
               //imagemanager_document_base_url: '',
               //imagemanager_document_base_url: '<%= MCC.routines.ResolveServerUrl("~/") %>',
               //imagemanager_remove_script_host: false,

               // Example content CSS (should be your site CSS)

               content_css: '<%=ResolveUrl("~/_assets/css/user_article.css") %>',

               // Drop lists for link/image/media/template dialogs
               template_external_list_url: "lists/template_list.js",
               external_link_list_url: "lists/link_list.js",
               external_image_list_url: "lists/image_list.js",
               media_external_list_url: "lists/media_list.js",

               // Replace values for the template plugin
               template_replace_values: {
                  username: "Some User",
                  staffid: "991234"
               },

               template_templates: [
		            {
		               title: "MCC page template 1",
		               src: '<%=ResolveUrl("~/admin/templates/template.txt") %>',
		               description: "three ad template"
		            },
		            {
		               title: "MCC Page Template 2",
		               src: '<%=ResolveUrl("~/admin/templates/template2.txt") %>',
		               description: "two ad template."
		            },
		             {
		                title: "MCC Page Template 3",
		                src: '<%=ResolveUrl("~/admin/templates/template3.txt") %>',
		                description: "two ad template."
		             }
	            ]

            });
         });
      }

      function convLinkVC(strUrl, node, on_save) {
         strUrl = strUrl.replace("../", "");
         return strUrl;
      }

      $(function() {
         Date.format = 'mm/dd/yyyy';
         $('.date-pick').datePicker();

         $("#InputTags").autocomplete('<%=ResolveUrl("~/ajaxData.ashx") %>',
            options = {
               highlightItem: true,
               multiple: true,
               multipleSeparator: ',',
               delay: 10,
               minChars: 1,
               cacheLength: 10,
               selectOnly: 1,
               extraParams: { qId: "tags" }
            });
      });


      function char_count(field, cntfield, maxlimit) {
         if ($(field)[0].value.length > maxlimit) // if too long...trim it!
            $(field)[0].value = $(field)[0].value.substring(0, maxlimit);
         // otherwise, update 'characters left' counter
         else
            $(cntfield)[0].value = maxlimit - $(field)[0].value.length;
      }

      function tolcnt(w, x) {
         var y = w.value;
         a = y.replace(/\n/g, '');
         b = a.replace(/\r/g, '');
         z = b.length;
         x.value = z;
      }

      function word_count(w, x) {
         var y = w.value;
         var r = 0;
         a = y.replace(/\s/g, ' ');
         a = a.split(' ');
         for (z = 0; z < a.length; z++) { if (a[z].length > 0) r++; }
         x.value = r;
      }

      var _infoWindow = null;
      function InfoSetup() {
         if (!_infoWindow) {
            _infoWindow = $("#info_window:first");
         }
         $(_infoWindow).toggle();
      }

      function AdSetup() {
         $("#ad_window").toggle();
      }

      var _bodyWindow = null;
      function BodySetup() {
         if (_bodyWindow === null) {
            _bodyWindow = $("#body_window:first");
         }

         _bodyWindow.toggle();
         if (!_editorIsLoaded) {
            _editorIsLoaded = true;
            LoadEditor();
         }
      }

      //        $(function() {

      //            $("#list_tags").autocomplete('<%=ResolveUrl("~/ajaxData.ashx") %>?qId=tags', properties = {
      //                matchContains: true,
      //                minChars: 1,
      //                delay: 10,
      //                selectFirst: false,
      //                intro_text: "Type Tags",
      //                no_result: "No Tags",
      //                highlightItem: true,
      //                multiple: true,
      //                multipleSeparator: ', ',
      //                matchSubset: 1,
      //                matchContains: 1,
      //                cacheLength: 10,
      //                selectOnly: 1
      //            });

      ////            $('.facelist').prepend('<li id="bit-101" class="token"><span><span><span><span>tags</span></span></span></span><span class="x"> .x</span></li>');

      //        });




      function SetArticleUrl() {

         if (!_editorIsLoaded) {
            _editorIsLoaded = true;
            LoadEditor();
         }

         mcImageManager.browse({
            fields: 'ImageNewsUrl',
            no_host: true
         });


      }

      function SetArticleIconUrl() {

         if (!_editorIsLoaded) {
            _editorIsLoaded = true;
            LoadEditor();
         }

         mcImageManager.browse({
            fields: 'ImageIconUrl',
            no_host: true
         });
      }

      function SetVideoUrl() {

         if (!_editorIsLoaded) {
            _editorIsLoaded = true;
            LoadEditor();
         }

         mcImageManager.browse({
            fields: 'VideoUrl',
            no_host: true
         });
      }


      function getFileId(url, info) {
         if ((info) && (info.focusedFile.name)) {
            var i = info.focusedFile.name.lastIndexOf(".");
            if (i != -1) {
               $('#ImageNewsUrl').val(info.focusedFile.name.substring(0, i));
            }
         }
      }

      $('#lnkImage').click(function() {
         MediaBrowser.Open({ input: '#lnkImage',
            output: '#ImageNewsUrl',
            outMediaId: '#ImageID',
            MediaType: 'Image'
         });
      });


      $('#lnkIcon').click(function() {
         MediaBrowser.Open({ input: '#lnkIcon',
            output: '#ImageIconUrl',
            MediaType: 'Image'
         });
      });

      $('#lnkVideoPick').click(function() {
         MediaBrowser.Open({ input: '#lnkVideoPick',
            output: '#VideoID',
            MediaType: 'Video'
         });
      });
      //]]>


      function previewAd(id) {
         id = parseInt(id);

         fetchAd(id)

      }

      function InitHandlers() {
         $(":checkbox").click(function(ev) {
            var e = $.event.fix(ev);
            e.stopPropagation();

            if ($(this).attr('id') == 'checkCol') {
               var cb = $(this).is(':checked');
               $(":checkbox").attr('checked', cb);
            }

            if ($("input:checked").length > 0) {
               $('#btnDelete').attr('disabled', false) //.removeClass('rb-delete-disable').addClass('rb-delete');
            }
            else {
               $('#btnDelete').attr('disabled', true) //.removeClass('rb-delete').addClass('rb-delete-disable');
            }
         });
      }

      function showAdPreview(ap) {

         if (ap) {
            $("#jqmAdTitle").html(ap.Title)
            $("#jqmAdInfo").center();
            $("#jqmAdInfo").jqm({ "overlay": 10 });
            $("#jqmAdBody").html(ap.Body)
            $("#jqmAdInfo").jqmShow();

         }
         else {

            $("#jqmAdInfo").center();
            $("#jqmAdInfo").jqm();
            $("#jqmAdBody").html("Unable to find info for the specified ad!")
            $("#jqmAdInfo").jqmShow();
         }
      }


      function fetchAd(id) {
         if (Proxy == null) {
            Proxy = new serviceProxy('<%=ResolveUrl("~/asv.asmx/") %>');
         }
         var DTO = { "id": id };
         Proxy.invoke('GetAdByID', DTO, showAdPreview);
      }



      function InitTabs() {
         $('#tabs div.tab').hide(); // Hide all divs
         $('#tabs div.tab:first').show(); // Show the first div
         $('#tabs div.tb_lk:first').addClass('active'); // Set the class of the first link to active
         $('#tabs div.tb_lk a').click(function() { //When any link is clicked
            $('#tabs div.tb_lk').removeClass('active'); // Remove active class from all links
            $(this).parent().addClass('active'); //Set clicked link class to active
            var currentTab = $(this).attr('href'); // Set variable currentTab to value of href attribute of clicked link
            $('#tabs div.tab').hide(); // Hide all divs
            $(currentTab).show(); // Show div with id equal to variable currentTab
            return false;
         });
      }

      //        var delta = 200;
      //        var lastKeypressTime = 0;
      //        function KeyHandler(event) {
      //            if (String.fromCharCode(event.charCode).toUpperCase() == 'T') {
      //                var thisKeypressTime = new Date();
      //                if (thisKeypressTime - lastKeypressTime <= delta) {
      //                    //doDoubleKeypress();

      //                    showTags();
      //                    // optional - if we'd rather not detect a triple-press
      //                    // as a second double-press, reset the timestamp
      //                    thisKeypressTime = 0;
      //                }
      //                lastKeypressTime = thisKeypressTime;
      //            }
      //        }
      //        document.onkeypress = KeyHandler;

      var Proxy = null;
      var tags = [];
      var tagControlId = 'Tags';

      $('#editTags').click(function() {
         EditTags();
      });

      $('#btnCancelTags').click(function() {
         $("#jqmTagInfo").jqmHide();
      });

      $('#btnSaveTags').click(function() {
         SaveTags();
         $("#jqmTagInfo").jqmHide();
      });

      function showTags() {
         $("#jqmTagInfo").center();
         $("#jqmTagInfo").jqm({ "overlay": 10 });
         $("#jqmTagInfo").jqmShow();
      }


      $('#addTags').click(function() {
         var _tag = $('#InputTags').val().Trim();
         var _tags = _tag.split(',');

         var itags = [];
         for (var k = 0; k < _tags.length; k++) {
            if (_tags[k].Trim() !== '') {
               itags.push(_tags[k].Trim());
            }
         }
         _tags = itags;

         if (_tags.length <= 0)
            return;


         // process only non-existing tags
         var ftags = [];
         for (var i = 0; i < _tags.length; i++) {
            if (!tags.contains(_tags[i])) {
               ftags.push(_tags[i]);
               tags.push(_tags[i]);
            }
         }

         for (var k = 0; k < ftags.length; k++) {
            var tg = $('<a class="tag_item" href="javascript:void(0);" alt="' + ftags[k] + '">' + ftags[k] + '</a>');

            if ($('#TagItems a').length > 0) {
               $('#TagItems').append(', ')
            }
            $('#TagItems').append(tg);
            $('#InputTags').val('');
         }
         updateTagsList();
      });



      function EditTags() {
         $('#editTagsBx').val('');
         for (var i = 0; i < tags.length; i++) {
            var tav = $('#editTagsBx').val();
            if (tav != '') {
               var apv = $('#editTagsBx').val();
               $('#editTagsBx').val(apv + "," + tags[i]);
            }
            else {
               $('#editTagsBx').val(tags[i]);
            }
         }
         showTags();
      }

      function SaveTags() {
         showEditTagProgress(true);
         var its = $('#editTagsBx').val();
         tags.splice(0, tags.length);
         tags = its.split(',');

         var ftags = [];

         for (var k = 0; k < tags.length; k++) {
            if (tags[k].Trim() !== '') {
               ftags.push(tags[k].Trim());
            }
         }

         tags = ftags;
         insertTags();
         updateTagsList();
      }


      function insertTags() {
         $('#TagItems').empty();

         for (var i = 0; i < tags.length; i++) {
            var tg = $('<a class="tag_item" href="javascript:void(0);" alt="' + tags[i] + '">' + tags[i] + '</a>');
            if ($('#TagItems a').length > 0) {
               $('#TagItems').append(', ');
            }
            $('#TagItems').append(tg);
         }
         showEditTagProgress(false);
      }

      function showEditTagProgress(v) {
         if (v) {
            $('#jqmTagInfo .command').hide();
            $('#jqmTagInfo .progress').show();
         }
         else {
            $('#jqmTagInfo .command').show();
            $('#jqmTagInfo .progress').hide();
         }
      }

      function initTagsList() {
         tags = $('#' + tagControlId).val().split(",");
         insertTags();
      }

      function updateTagsList() {
         $('#' + tagControlId).val(tags.join(","));
      }


      (function() {
         window.TagManager = {

            options: {},

            setup: function() {

            }
         }
      })();

   </script>

   <% Html.RenderPartial("~/Areas/Site/views/shared/imagebrowser.ascx", New ImageBrowserViewData())%>
</asp:Content>
