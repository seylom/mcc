<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage(of adsViewModel)" %>

<asp:Content ContentPlaceHolderID="PageTitle" runat="server">
Ads
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="headerPlaceHolder" runat="Server">
   <%=appHelpers.CssTagUrl("jqModal.css")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainSiteContent" runat="Server">
   <div style="border-bottom: 1px solid #eaeaea; padding: 5px; margin-bottom: 10px;">
      <h1 class="admin-title">
         Manage Affiliates Ads</h1>
   </div>
   <%If Model.Ads Is Nothing Or Model.Ads.Count = 0 Then%>
   <center>
      <b>- No Affiliate Ad are currently in the database -</b>
   </center>
   <%Else%>
   <div id="Div1" style="padding: 5px; margin: 10px 0; background-color: #e7faea;">
      <input id="btnCreateTop" type="button" title="Create" value="Create" />
      <input type="button" id="btnDeleteTop" disabled="disabled" value="Delete" title="Delete" />
   </div>
    <%=Html.AdvancedPager(Model.Ads, "Index", "AdAdmin", New Integer() {10, 30, 50})%>
   <div id="jTemplate">
      <table class="edit-info datatable">
         <col style="width: 30px;" />
         <col />
         <col />
         <col />
         <col />
         <col />
         <thead>
            <tr>
               <th>
                  <input type="checkbox" id="checkCol" />
               </th>
               <th>
                  Title:
               </th>
               <th>
                  Description:
               </th>
               <th>
                  Status:
               </th>
               <th>
                  Type:
               </th>
               <th>
                  Author:
               </th>
            </tr>
         </thead>
         <tbody>
            <% dim idx as integer = 0 %>
            <%For Each it As Ad In Model.Ads%>
            <tr id='<%="item_" & it.AdID  %>' class='aditem <%= IIF(idx Mod 2 = 0,"even", "odd") %>'
               tag='<%= it.AdId %>'>
               <td>
                  <input tag='<%= it.AdID %>' class='ar_cb' type='checkbox' />
               </td>
               <td>
                  <a class="lnk" href="<%= url.Action("EditAd",new with {.Id = it.AdID })%>" title="<%=Html.Encode(it.Title)%>"><%=Html.Encode(it.Title)%></a>
               </td>
               <td>
                  <%=it.Description%>
               </td>
               <td>
                  <% If Not it.Approved Then%>
                    <span id="Span1" style="font-size: 9px; font-style: italic" >
                     not approved</span>
                  <%End If%>          
               </td>
               <td>
                  <%=appHelpers.AdType(CInt(it.Type))%>
               </td>
               <td>
                  <%=it.AddedBy%>
               </td>
            </tr>
            <% idx += 1%>
            <% Next%>
         </tbody>
      </table>
      <%=Html.AdvancedPager(Model.Ads, "Index", "AdAdmin", New Integer() {10, 30, 50})%>
   </div>
   <%--            <div class="PagerContainer">
                Page <span></span>
                <asp:DataPager ID="bottomDataPager" runat="server" PageSize="30" PagedControlID="lvAds"
                    EnableViewState="false">
                    <Fields>              
                        <mcc_controls:AdvancedPagerField ButtonCssClass="button" NextPageImageUrl="~/_assets/images/button_arrow_right.gif"
                            PreviousPageImageUrl="~/_assets/images/button_arrow_left.gif" />
                    </Fields>
                </asp:DataPager>
            </div>--%>
   <div id="tools" style="padding: 5px; margin-top: 10px; background-color: #e7faea;">
      <input id="btnCreate" type="button" title="Create" value="Create" />
      <input id="btnDelete" type="button" title="Delete" value="Delete" disabled="disabled" />
   </div>
   <% End If%>

   <script type="text/html" id="table_template">
        {#foreach $T.d as data}
        <tr class="aditem {#cycle values=['even','odd']}" tag="{$T.data.AdID}">
            <td><input class='ar_cb' type='checkbox'/></td>
            <td>{$T.data.AdID}</td>	
            <td>{$T.data.Title}</td>
            <td>...</td>
            <td>{$T.data.Approved}</td>
             <td>{$T.data.Type}</td>
            <td>{$T.data.Author}</td>
        </tr>
        {#/for}
   </script>

   <script type="text/html" id="item_template">
        <tr class="aditem" tag="<#= AdID #>">
            <td><input class='itcb' type='checkbox'/></td>
            <td><#= Title #></td>
            <td>...</td>
            <td><#= Approved #></td>
            <td><#= Type #></td>
            <td><#= Author #></td>
        </tr>
   </script>

   <div id="jqmArticleAd" class="jqmAlert jqmArticleAd" style="display: none;">
      <div class="jqmAlertWindow">
         <div class="jqmAlertTitle">
            <h1>
               Add/Edit Ad: -</h1>
            <a class="jqmClose" href="#"><em>Close</em> </a>
         </div>
         <div class="jqmAlertBody">
            <table class="category" style="width: 100%">
               <col width="80px" />
               <col />
               <tbody>
                  <tr>
                     <td class="right">
                        <span>Preview:</span>
                     </td>
                     <td>
                        <div id="preview_bx" style="background-color: #fff; max-height: 100px; overflow: hidden;
                           border: 1px solid blue; padding: 3px; max-width: 95%">
                        </div>
                     </td>
                  </tr>
                  <tr>
                     <td class="right">
                        <span>Ad Type</span>
                     </td>
                     <td>
                     <%--   <select id="adType">
                           <option title="Undefined" value="0">Undefined</option>
                           <option title="Banner(Vertical)" value="1">Banner(Vertical)</option>
                           <option title="Banner(Horizontal)" value="2">Banner(Horizontal)</option>
                           <option title="Text Only" value="3">Text Only</option>
                           <option title="Image(Vertical)" value="4">Image(Vertical)</option>
                           <option title="Image(Horizontal)" value="5">Image(Horizontal)</option>
                           <option title="Widget(Vertical)" value="6">Widget(Vertical)</option>
                           <option title="Widget(Horizontal)" value="7">Widget(Horizontal)</option>
                           <option title="GoogleAds" value="8">GoogleAds</option>
                        </select>--%>
                        
                        <%=Html.DropDownList("AdType", Model.AdItemType.ToSelectList())%>
                        
                     </td>
                  </tr>
                  <tr>
                     <td class="right">
                        <span>Title</span>
                     </td>
                     <td>
                        <input id="txtTitle" type="text" style="width: 95%" class="mtcRed req" />
                        <label id="titleReqLabel" class="isreq" for="txtTitle" style="display: none;">
                           Please enter a title!</label>
                     </td>
                  </tr>
                  <tr>
                     <td class="right">
                        <span>Description</span>
                     </td>
                     <td>
                        <textarea id="txtDescription" style="width: 95%" class="mtac" rows="3" cols="1"></textarea>
                     </td>
                  </tr>
                  <tr>
                     <td class="right">
                        <span>Body</span>
                     </td>
                     <td>
                        <textarea id="txtBody" style="width: 95%" class="mtac" rows="3" cols="1"></textarea>
                     </td>
                  </tr>
                  <tr>
                     <td class="right">
                        <span>Tags</span>
                     </td>
                     <td>
                        <input id="txtTags" type="text" style="width: 95%" class="mtc" />
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
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottomScripts" runat="Server">
   <%=appHelpers.ScriptsTagUrl("jquery/jquery-jtemplates_uncompressed.js")%>
   <%=appHelpers.AdvancedScriptsTagUrl("utils.js", MCC.Utils.FileVersion("utils"))%>
   <%=appHelpers.ScriptsTagUrl("json2.js")%>
   <%=appHelpers.ScriptsTagUrl("jquery/jquery.tablesorter.min.js")%>
   <%=appHelpers.ScriptsTagUrl("jquery/jqmodal.js")%>
   <%=appHelpers.ScriptsTagUrl("jquery/jquery.effects.js")%>

   <script type="text/javascript">

      var template;

      $('#jqmArticleAd').center();
      $('#jqmArticleAd').jqm({ overlay: 10 });


      $(document).ready(function() {
         InitHandlers();
         InitMethods();

         //  apply the tablesorter plugin
         $('#jTemplate .datatable').tablesorter({
            //                cssAsc: 'asc',
            //                cssDesc: 'desc',
            widgets: ['zebra'],
            headers: {
               0: { sorter: false }
            }
         });
      });


      function InitHandlers() {
         $('#btnSave').click(function() {
            saveAd();
         });

         $('#btnCancel').click(function() {
            hideCBx();
         });
         $('#btnCreate,#btnCreateTop').click(function() {
            editAd(this, -1);
         });

         $('#txtTitle').blur(function() {
            if ($(this).val() !== '') {
               $('#titleReqLabel').hide();
            }
         });

         $('#btnDelete,#btnDeleteTop').click(function() {
            if (confirm('Do you really want to delete the selected ads?\r\n This operation cannot be reverted')) {

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
                     Proxy.invoke('DeleteAds', data, function(result) {
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

      }




      function InitMethods() {

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

         $(".aditem").dblclick(function() {
            var pk = $(this).attr("tag");
            editAd($(this), parseInt(pk));
         }).hover(function() { $(this).addClass('hilight'); }, function() { $(this).removeClass('hilight'); });
      }

      function onPageError(error) {
         $('#jqmArticleAd').jqmHide();
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
            $('#jTemplate').after($("<div class='ad_status' style='display:none'><div>").attr('id', 'status_message').addClass(sClass).html(m).fadeIn('normal'));
         }

         setTimeout(function() {
            $('#status_message').fadeOut('normal', function() {
               $('#status_message').removeClass(sClass);
            });
         }, 4000);
      }


      var CateoryList;
      var adId = null;
      var activeAd = null;


      function showCBx() {
         $('#jqmArticleAd').jqmShow();
      }

      function hideCBx() {
         $('#jqmArticleAd').jqmHide();
         ClearValidation();
      }

      function saveAd(ctl, evt) {
         var jItem = $(ctl);

         var ad = activeAd;

         ad.AdID = adId;
         if (adId < 1)
            adId = -1;

         if (isAdValid()) {
            ad.Title = $("#txtTitle").val();
            ad.Keywords = $("#txtTags").val();
            ad.Description = $("#txtDescription").val();
            ad.Body = $("#txtBody").val();
            ad.Type = parseInt($("#adType").val());

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

            var DTO = { 'ad': ad };

            Proxy.invoke('saveAd', DTO, function(result) {
               updateAd(ad, true);
               showMessage('Ad successfully updated!');
               $("#jTemplate .datatable").trigger("update");
            }, onPageError);

         }
      }


      function updateAd(ad, highlight) {

         if (highlight === undefined) {
            highlight = false;
         }

         hideCBx();

         var item = $(".aditem[tag=" + ad.AdID + "]");

         var html = parseTemplate($("#item_template").html(), ad);

         var newItem = $(html).attr("tag", ad.AdID.toString()).click(function() {
            var pk = $(this).attr("tag");
            editAd($(this), parseInt(pk));
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




      function selectAd() {
         var item = $(ctl);
         var id = item.attr("tag");

         for (var i = 0; i < bookList.length; i++) {

            item = adList[i];
            if (item.AdID != id)
               continue;

            var ad = scriptVars.blankAd;

            ad.Title = item.Title;
            ad.Body = item.Body;
            ad.Keywords = item.Keywords;
            ad.Description = item.Description;
            ad.Type = item.Type;
            FillAdInfo(ad);
         }
      }

      function editAd(ctl, id) {
         var jItem = $(ctl);

         if (id) {
            adId = id;
         }
         else {
            adId = parseInt(jItem.attr("tag"));
         }
         if (adId < 1) {
            activeAd = scriptVars.blankAd;
            FillAdInfo(activeAd);
            showCBx();
         }
         else {

            var data = { 'id': adId };
            Proxy.invoke('GetAdByID', data, function(result) {
               if (result) {
                  var c = result;
                  FillAdInfo(c);
                  showCBx();
               }
            }, onPageError);
         }
      }

      function deleteAd(ctl, ev) {
         //            var jCateogryItem = $(ctl).parents("#divBookItem");
         //            var pk = jCateogryItem.attr("tag");

         //            var e = $.event.fix(ev);    // turn into jQuery event
         //            e.stopPropagation();

         //            showProgress();
         //            Proxy.DeleteBook(parseInt(pk),
         //                     function(result) {
         //                         showProgress(true);
         //                         showStatus("Book deleted.", 5000);

         //                         jBookItem.fadeOut(1000,
         //                                          function() {
         //                                              jBookItem.remove();
         //                                          });
         //                     }, onPageError);
         //            return false;
      }


      function FillAdInfo(c) {
         activeAd = c;
         $("#txtTitle").val(c.Title || "");
         $("#txtDescription").val(c.Description || "");
         $("#txtBody").val(c.Body || "");
         $("#txtTags").val(c.Keywords || "");
         $("#adType").val(c.Type || "0");
         $("#preview_bx").html(c.Body);
      }

      var Proxy = new serviceProxy('<%=ResolveUrl("~/asv.asmx/") %>');

      function ClearValidation() {
         $('#titleReqLabel').hide();
      }

      function isAdValid() {
         var isValid = true;
         var title = $('#txtTitle').val();
         if (title == '') {
            $('#titleReqLabel').show();
            isValid = false;
         }
         return isValid;
      }
      
   </script>

   <script type="text/javascript">
      //<![CDATA[
      var scriptVars = {
         blankAd: {
            "AdID": 0,
            "Title": null,
            "Description": null,
            "Body": null,
            "Keyword": null,
            "Type": 0
         },

         Image1Id: "ctl00_Image1",
         MainContentId: "ctl00_MainContent"
      };
      //]]>


   </script>

</asp:Content>
