<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage(of AdminVideoViewModel)" %>

<asp:Content ID="Content4" ContentPlaceHolderID="PageTitle" runat="server">
Edit Video:
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="headerPlaceHolder" runat="server">
   <%=appHelpers.CssTagUrl("datepicker.css")%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainSiteContent" runat="Server">
   <div style="border-bottom: 1px solid #eaeaea; padding: 5px; margin-bottom: 10px;">
      <h1 class="admin-title">
         Video Detail</h1>
   </div>
   <div id="tabs">
      <div id="tblks">
         <div class="tb_lk">
            <a id="A0" href='#info_details'>Details</a>
         </div>
         <div class="tb_lk">
            <a id="A1" href='#info_basic'>Basic info</a>
         </div>
         <div class="tb_lk">
            <a id="A3" href='#info_images'>Video Still</a>
         </div>
         <div class="tb_lk">
            <a id="A5" href='#info_categories'>Categories</a>
         </div>
         <div class="tb_lk">
            <a id="A4" href='#info_attributes'>Attributes</a>
         </div>
      </div>
      <div id="info_details" class="tab">
         <table class="edit-info" cellspacing="1">
            <col style="width: 100px;" />
            <col />
            <tbody>
               <tr>
                  <td class="right" style="width: 100px;">
                     ID :
                  </td>
                  <td class="left">
                     <%=Model.VideoID%>
                  </td>
               </tr>
               <tr>
                  <td class="right">
                     Added date :
                  </td>
                  <td class="left">
                     <%=Model.AddedDate%>
                  </td>
               </tr>
               <tr>
                  <td class="right">
                     Added by :
                  </td>
                  <td class="left">
                     <%=Model.AddedBy%>
                  </td>
               </tr>
            </tbody>
         </table>
      </div>
      <div id="info_basic" class="tab" style="display: none;">
         <table class="edit-info" cellspacing="1">
            <col style="width: 100px;" />
            <col />
            <tbody>
               <tr>
                  <td class="right">
                     Video url:
                  </td>
                  <td class="left">
                     <input type="text" class="rtc" name="VideoUrl" value="<%= Model.VideoUrl %>" style="width:90%" />
                     <input type="image" src='<%= appHelpers.ImageUrl("icon_post.gif") %>' style="width: 18px;
                        height: 9px;"  onclick="SetVideoUrl();return false;" />
                  </td>
               </tr>
               <tr>
                  <td class="right">
                     Tags:
                  </td>
                  <td class="left">
                     <input  name="Tags" type="text" value='<%=Model.Tags%>' class="rtc" style="width: 99%" />
                  </td>
               </tr>
               <tr>
                  <td class="right">
                     Title :
                  </td>
                  <td class="left">
                     <input name="Title" value='<%= Model.Title %>' class="rtc" style="width: 99%" />
                  </td>
               </tr>
               <tr>
                  <td class="right">
                     Abstract :
                  </td>
                  <td class="left">
                     <%=Html.TextArea("Abstract", Model.Abstract, New With {.class = "rtc", .style = "width:99%"})%>
                  </td>
               </tr>
            </tbody>
         </table>
      </div>
      <div id="info_images" class="tab" style="display: none;">
         <table class="edit-info" cellspacing="1">
            <col style="width: 100px;" />
            <col />
            <tbody>
               <tr>
                  <td class="right">
                     Video Still :
                  </td>
                  <td class="left">
                     <img class="ev_img" src='<%= Model.VideoStillUrl %>' alt="VideoStill" />
                     <input type="submit" value="Regenerate Video Still" />
                  </td>
               </tr>
            </tbody>
         </table>
      </div>
      <div id="info_categories" class="tab" style="display: none;">
         <% For Each it As VideoCategory In Model.Categories%>
         <div style="margin: 2px 0;">
            <%=CreateCheckBox("CategoryIds", it.CategoryID, Model.CategoryIds.Contains(it.CategoryID), Nothing)%>&nbsp;<%=it.Title%>
         </div>
         <%Next%>
      </div>
      <div id="info_attributes" class="tab" style="display: none;">
         <table class="edit-info" cellspacing="1">
            <col style="width: 100px;" />
            <col />
            <tbody>
               <tr>
                  <td class="right">
                     Approved :
                  </td>
                  <td class="left">
                     <%=Html.CheckBox("Approved", Model.Approved)%>
                  </td>
               </tr>
               <tr>
                  <td class="right">
                     Listed :
                  </td>
                  <td class="left">
                     <%=Html.CheckBox("Listed", Model.Listed)%>
                  </td>
               </tr>
               <tr>
                  <td class="right">
                     Only For Members :
                  </td>
                  <td class="left">
                     <%=Html.CheckBox("OnlyForMembers", Model.OnlyForMembers)%>
                  </td>
               </tr>
               <tr>
                  <td class="right">
                     Comments Enabled :
                  </td>
                  <td class="left">
                     <%=Html.CheckBox("CommentsEnabled", Model.CommentsEnabled)%>
                  </td>
               </tr>
            </tbody>
         </table>
      </div>
   </div>
   <div style="margin-top: 5px;">
      <input type="submit" class="rb-btn rb-update" value />
      <input type="button" class="rb-btn rb-cancel" />
      <a class="globalred" id="updateFileLink" href='<%= Url.Action("UpdateVideoFile",new with {.controller="VideoAdmin"})%>'>
         Update Video File</a>
   </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottomScripts" runat="server">
   <%=appHelpers.ScriptsTagUrl("date.js")%>
   <%=appHelpers.ScriptsTagUrl("jquery/jquery.datePicker.min-2.1.2.js")%>
   <%=appHelpers.LocalScriptsTagUrl("tinymce/jscripts/tiny_mce/tiny_mce_gzip.js")%>
   <%=appHelpers.ScriptsTagUrl("tinymce/jscripts/tiny_mce/plugins/imagemanager/js/mcimagemanager.js")%>

   <script type="text/javascript">
      //<![CDATA[
      $(document).ready(function() {
         $('.date-pick').datePicker();
         InitTabs();
      });
      
      function SetVideoUrl() {
//         mcImageManager.browse({
//            fields: 'VideoUrl',
//            no_host: true
//         });
      }
      

      function getFileId(url, info) {
         if ((info) && (info.name)) {
            var i = info.name.lastIndexOf(".");
            if (i != -1) {
               $('#ctl00_mainSiteContent_FVVideoLayoutPage_txtThumbnail')[0].value = info.name.substring(0, i);
            }
         }
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
      //]]>
   </script>

</asp:Content>
