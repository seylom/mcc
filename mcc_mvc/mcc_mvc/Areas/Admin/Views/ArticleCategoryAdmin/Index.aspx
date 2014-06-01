<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage(of AdminArticleCategoriesViewModel)" %>

<asp:Content ID="Content4" ContentPlaceHolderID="PageTitle" runat="server">
Article Categories
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="headerPlaceHolder" runat="server">
   <%=appHelpers.CssTagUrl("jqModal.css")%>
   <%=appHelpers.CssTagUrl("imageBrowser.css")%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainSiteContent" runat="Server">
   <div style="border-bottom: 1px solid #eaeaea; padding: 5px; margin-bottom: 10px;">
      <h1 class="admin-title">
         Article Categories</h1>
   </div>
   <%if model.Categories is nothing or Model.Categories.count = 0 %>
   <%Else %>
    <%=Html.AdvancedPager(Model.Categories, "Index", "ArticleCategoryAdmin", New Integer() {10, 30, 50})%>
   <div id="jTemplate">
      <table class="edit-info datatable">
         <col width="20px" />
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
                  Title
               </th>
               <th>
                  Description
               </th>
               <th>
                  Importance
               </th>
               <th>
                  Image Url
               </th>
               <th>
                  Parent ID
               </th>
            </tr>
         </thead>
         <tbody>
            <%dim Idx as integer = 0 %>
            <% For each it as ArticleCategory in Model.Categories %>
            <tr class='categoryitem <%= IIF(idx Mod 2 = 0,"even", "odd") %>'
               tag='<%= it.CategoryID  %>'>
               <td>
                  <input type="checkbox"  tag='<%=it.CategoryID %>' />
               </td>
               <td>
                  <%=html.Encode(it.Title )%>
               </td>
               <td>
                  <span>... </span>
               </td>
               <td>
                  <%=it.Importance %>
               </td>
               <td>
                  <a href="javascript:void(0);">
                     <%= it.ImageUrl %></a>
               </td>
               <td>
                  <%= it.ParentCategoryID%>
               </td>
            </tr>
            <% idx+=1 %>
            <%Next %>
         </tbody>
      </table>
      <%=Html.AdvancedPager(Model.Categories, "Index", "ArticleCategoryAdmin", New Integer() {10, 30, 50})%>
   </div>

   <script type="text/html" id="table_template">
        {#foreach $T.d as data}
        <tr class="categoryitem {#cycle values=['even','odd']}" tag="{$T.data.CategoryID}">
            <td><input class='itcb' type='checkbox'/></td>
            <td>{$T.data.Title}</td>
            <td>...</td>
            <td>{$T.data.Importance}</td>
            <td>{$T.data.ImageUrl}</td>
            <td>{$T.data.ParentCategoryID}</td>
        </tr>
        {#/for}
   </script>

   <script type="text/html" id="item_template">
        <tr class="categoryitem" tag="<#= CategoryID #>">
            <td><input class='itcb' type='checkbox'/></td>
            <td><#= Title #></td>
            <td>...</td>
            <td><#= Importance #></td>
            <td><#= ImageUrl #></td>
            <td><#= ParentCategoryID #></td>
        </tr>
   </script>

   <%--<div id="jTemplate">
        <table class="edit-info datatable">
            <thead>
                <tr>
                    <th style="width: 10px;"><input type="checkbox" /></th>
                    <th>ID </th>
                    <th>Title </th>
                    <th>Description </th>
                    <th>Importance</th>
                    <th>Image Url</th>
                    <th>Parent ID</th>
                    <th>Command</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td colspan="8">Loading data ... </td>
                </tr>
            </tbody>
        </table>
    </div>--%>
   <div id="tools" style="padding: 5px; margin-top: 10px; background-color: #e7faea;">
      <input id="btnCreate" type="button" title="Create" value="Create" />
      <input id="btnDelete" type="button" title="Delete" value="Delete" disabled="disabled" />
   </div>
   <% End If %>
   <div id="jqmArticleCategory" class="jqmAlert jqmArticleCategory" style="display: none;">
      <div class="jqmAlertWindow">
         <div class="jqmAlertTitle jqDrag">
            <h1>
               Add/Edit Category: -</h1>
            <a class="jqmClose" href="#"><em>Close</em> </a>
         </div>
         <div class="jqmAlertBody">
            <table class="category" style="width: 100%">
               <tr>
                  <td class="right">
                     <span>Title</span>
                  </td>
                  <td>
                     <input id="txtTitle" type="text" style="width: 350px" class="mtcRed req" />
                     <label id="titleReqLabel" class="isreq" for="txtTitle" style="display: none;">
                        Please enter a title!</label>
                  </td>
               </tr>
               <tr>
                  <td>
                     <span>Description</span>
                  </td>
                  <td>
                     <textarea id="txtDescription" style="width: 350px" class="mtac" rows="5" cols="1"></textarea>
                  </td>
               </tr>
               <tr>
                  <td>
                     <span>Image Url</span>
                  </td>
                  <td>
                     <input id="txtImageUrl" type="text" style="width: 320px" class="mtcRed" />
                     <a id="lnkImage" href="javascript:void(0);" class="browselnk"></a>
                  </td>
               </tr>
               <tr>
                  <td>
                     <span>Importance</span>
                  </td>
                  <td>
                     <input id="txtImportance" type="text" style="width: 350px" class="mtc" />
                  </td>
               </tr>
               <tr>
                  <td>
                     <span>Parent Category</span>
                  </td>
                  <td>
                     <%=Html.DropDownList("ddlParentCategory", Model.ParentCategories)%>
                  </td>
               </tr>
            </table>
         </div>
         <div style="text-align: center">
            <input type="button" id="btnSave" validationgroup="Save" title="Save" value="Save Changes" />
            <input type="button" id="btnCancel" title="Cancel" value="Cancel" />
         </div>
      </div>
   </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottomScripts" runat="server">
   <%=appHelpers.ScriptsTagUrl("jquery/jquery-jtemplates_uncompressed.js")%>
   
   <%=appHelpers.ScriptsTagUrl("global.js")%>
   
<%--   <%=appHelpers.AdvancedScriptsTagUrl("utils.js", MCC.Utils.FileVersion("utils"))%>--%>
<%--   <%=appHelpers.ScriptsTagUrl("json2.js")%>   
      <%=appHelpers.ScriptsTagUrl("jquery/jqmodal.js")%> 
      <%=appHelpers.ScriptsTagUrl("jquery/jquery.effects.js")%>--%>
  <%=appHelpers.ScriptsTagUrl("jquery/jquery.tablesorter.min.js")%>

    
   <%=appHelpers.ScriptsTagUrl("jquery/jqDnR.js")%>
   <%=appHelpers.ScriptsTagUrl("jquery/jquery.autocomplete.js")%>
   <%=appHelpers.ScriptsTagUrl("jquery/jquery.bgiframe.min.js")%>
   <%=appHelpers.ScriptsTagUrl("jquery/jquery.dimensions.js")%>
  

   <script type="text/javascript">
      //<![CDATA[
      var template;

      var Proxy = new serviceProxy('/admin/articles/categories/');
      
      $('#jqmArticleCategory').center();
      $('#jqmArticleCategory').jqDrag('.jqDrag');
      $('#jqmArticleCategory').jqm({ overlay: 10 });


      $(document).ready(function() {

         //template = $("#item_template").html();
         //            $.ajax({
         //                type: 'POST',
         //                url: '<%= ResolveUrl("~/asv.asmx/FetchCategories") %>',
         //                data: '{}',
         //                contentType: 'application/json; charset=utf-8',
         //                dataType: 'json',
         //                success: function(result) {

         //                    if (result.d && result.d.length > 0) {

         //                        //                                                CateoryList = result.d;
         //                        //                                                $("#jTemplate .datatable tbody").empty();
         //                        //                                                $(result.d).each(function(i) {
         //                        //                                                    updateCategory(this);
         //                        //                                                });

         //                        //  get the grid and its template
         //                        var grid = $('#jTemplate .datatable tbody');
         //                        grid.setTemplate($('#table_template').html());
         //                        //  run the result set through the template ...
         //                        grid.processTemplate(result);

         //                        //  apply the tablesorter plugin
         //                        $('#jTemplate .datatable').tablesorter({
         //                            cssAsc: 'asc',
         //                            cssDesc: 'desc',
         //                            widgets: ['zebra'],
         //                            headers: {
         //                                0: { sorter: false }
         //                            }
         //                        });

         //                        InitMethods();
         //                    }
         //                }
         //            });



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

         $('#btnDelete').click(function() {
            if (confirm('Do you really want to delete the selected categories')) {
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
                     Proxy.invoke('DeleteCategories', data, function(result) {
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
         })
         
         $('#btnSave').click(function() {
            SaveCategory();
         });

         $('#btnCancel').click(function() {
            hideCBx();
         });
         $('#btnCreate').click(function() {
            editCategory(this, -1);
         });

         $('#txtTitle').blur(function() {
            if ($(this).val() !== '') {
               $('#titleReqLabel').hide();
            }
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

         $(".categoryitem").click(function() {
            var pk = $(this).attr("tag");
            editCategory($(this), parseInt(pk));
         }).hover(function() { $(this).addClass('hilight'); }, function() { $(this).removeClass('hilight'); });
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
            $('#jTemplate').after($("<div class='category_status' style='display:none'><div>").attr('id', 'status_message').addClass(sClass).html(m).fadeIn('normal'));
         }

         setTimeout(function() {
            $('#status_message').fadeOut('normal', function() {
               $('#status_message').removeClass(sClass);
            });
         }, 4000);
      }


      var CateoryList;
      var categoryId = null;
      var activeCategory = null;


      function showCBx() {
         $('#jqmArticleCategory').jqmShow();
      }

      function hideCBx() {
         $('#jqmArticleCategory').jqmHide();
         ClearValidation();
      }

      function SaveCategory(ctl, evt) {
         var jItem = $(ctl);

         var category = activeCategory;

         category.CategoryID = categoryId;
         if (categoryId < 1)
            categoryId = -1;

         if (isCategoryValid()) {
            category.Title = $("#txtTitle").val();
            category.Importance = $("#txtImportance").val();
            category.Description = $("#txtDescription").val();
            category.ImageUrl = $("#txtImageUrl").val();
            category.ParentCategoryID = parseInt($("#ddlParentCategory").val());

            //var DTO = { 'ac': category };
            //DTO = JSONcategory.stringify(DTO);
            var DTO = category;
            Proxy.invoke('SaveCategory', DTO, function(result) {
               updateCategory(category, true);
               showMessage('Category successfully updated!');
               $("#jTemplate .datatable").trigger("update");
            }, onPageError,"POST");

         }
      }


      function updateCategory(category, highlight) {

         if (highlight === undefined) {
            highlight = false;
         }

         hideCBx();

         var item = $(".categoryitem[tag=" + category.CategoryID + "]");

         var html = parseTemplate($("#item_template").html(), category);

         var newItem = $(html).attr("tag", category.CategoryID.toString()).click(function() {
            var pk = $(this).attr("tag");
            editCategory($(this), parseInt(pk));
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




      function selectCategory() {
         var item = $(ctl);
         var id = item.attr("tag");

         for (var i = 0; i < bookList.length; i++) {

            item = categoryList[i];
            if (item.CategoryID != id)
               continue;

            var category = scriptVars.blankCategory;

            category.Title = item.Title;
            category.ImageUrl = item.ImageUrl;
            category.Importance = item.Importance;
            category.ParentCategoyID = item.ParentCategoyID;
            category.Description = item.Description;

            FillCategoryInfo(category);
         }
      }

      function editCategory(ctl, id) {
         var jItem = $(ctl);

         if (id) {
            categoryId = id;
         }
         else {
            categoryId = parseInt(jItem.attr("tag"));
         }
         if (categoryId < 1) {
            activeCategory = scriptVars.blankCategory;
            FillCategoryInfo(activeCategory);
            showCBx();
         }
         else {

            var data = { 'Id': categoryId };
            Proxy.invoke('EditCategory', data, function(result) {
               if (result) {
                  var c = result;
                  FillCategoryInfo(c);
                  showCBx();
               }
            }, onPageError,"GET");
         }
      }

      function deleteCategory(ctl, ev) {
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


      function FillCategoryInfo(c) {
         activeCategory = c;
         $("#txtTitle").val(c.Title || "");
         $("#txtDescription").val(c.Description || "");
         $("#txtImportance").val(c.Importance || 0);
         $("#txtImageUrl").val(c.ImageUrl || "");
         $("#ddlParentCategory").val(c.ParentCategoryID || 0);
      }

      

      function ClearValidation() {
         $('#titleReqLabel').hide();
      }

      function isCategoryValid() {
         var isValid = true;
         var title = $('#txtTitle').val();
         if (title == '') {
            $('#titleReqLabel').show();
            isValid = false;
         }
         return isValid;
      }


      var scriptVars = {
         blankCategory: {
            "CategoryID": 0,
            "Title": null,
            "ImageUrl": null,
            "ParentCategoryID": null,
            "Importance": null,
            "Type": null
         },

         Image1Id: "ctl00_Image1",
         MainContentId: "ctl00_MainContent"
      };

      $('#lnkImage').click(function() {
         //ImageBrowser.Open({ input: '#lnkImage', output: '#txtImageUrl' });
      });

      //]]>


   </script>

</asp:Content>