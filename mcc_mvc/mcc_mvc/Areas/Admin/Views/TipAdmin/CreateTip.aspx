<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage(of AdminTipViewModel)" %>

<asp:Content ID="Content3" ContentPlaceHolderID="PageTitle" runat="server">
Create Tip
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainSiteContent" runat="Server">
   <div style="border-bottom: 1px solid #eaeaea; padding: 5px; margin-bottom: 10px;">
      <h1 class="admin-title">
         Add Tip</h1>
   </div>
   <%Using Html.BeginForm(New With {.action = "EditTip"})%>
   <input type="hidden" value="<%=  Model.AdviceID %>" name="AdviceID" />
   <%=Html.DisplayError()%>
   <div id="tabs">
      <div id="tblks">
         <div class="tb_lk">
            <a id="A1" href='#info_basic'>Basic info</a>
         </div>
         <div class="tb_lk">
            <a id="A5" href='#info_categories'>Categories</a>
         </div>
         <div class="tb_lk">
            <a id="A4" href='#info_attributes'>Attributes</a>
         </div>
      </div>
      <div id="info_basic" class="tab">
         <table class="edit-info">
            <tr>
               <td class="right" style="width: 100px;">
                  Id
               </td>
               <td class="left">
               </td>
            </tr>
            <tr>
               <td class="right">
                  Approved
               </td>
               <td class="left">
                  <%=Html.CheckBox("Approved", Model.Approved)%>
               </td>
            </tr>
            <tr>
               <td class="right">
                  Listed
               </td>
               <td class="left">
                  <%=Html.CheckBox("Listed", Model.Listed)%>
               </td>
            </tr>
            <tr>
               <td class="right">
                  only for members
               </td>
               <td class="left">
                  <%=Html.CheckBox("OnlyForMembers", Model.OnlyForMembers)%>
               </td>
            </tr>
            <tr>
               <td class="right">
                  Comments Enabled
               </td>
               <td class="left">
                  <%=Html.CheckBox("CommentsEnabled", Model.CommentsEnabled)%>
               </td>
            </tr>
         </table>
      </div>
      <div id="info_categories" class="tab">
      </div>
      <div id="info_attributes" class="tab">
      </div>
   </div>
   <table class="edit-info" style="margin-top: 10px;">
      <tr>
         <td class="right" style="width: 100px">
            <span>Tags :</span>
         </td>
         <td class="left">
            <%=Html.TextBox("Tags", Model.Tags, New With {.class = "rtc", .style = "width:99%;"})%>
         </td>
      </tr>
      <tr>
         <td class="right">
            <span>Title :</span>
         </td>
         <td class="left">
            <%=Html.TextBox("Title", Model.Title, New With {.class = "rtc", .style = "width:99%;"})%>
         </td>
      </tr>
      <tr>
         <td class="right">
            <span>Characters count :</span>
         </td>
         <td>
            <span>chars count: </span>
            <input type="text" id="lblCharUsed" readonly="readonly" style="width: 50px;" value="0"
               class="rtc" />
            <span>| words count</span>
            <input type="text" id="lblWordUsed" readonly="readonly" style="width: 50px;" value="0"
               class="rtc" />
            <span>| Remaining: </span>
            <input type="text" id="lblCharLeft" readonly="readonly" style="width: 50px;" value="300"
               class="rtc" />
         </td>
      </tr>
      <tr>
         <td class="right">
            <span>Body :</span>
         </td>
         <td>
            <%=Html.TextArea("Abstract", Model.Abstract, New With {.class = "rtc", .rows = 5, .cols = 12, .style = "width:99%"})%>
         </td>
      </tr>
   </table>
   <div style="margin-top: 5px;">
      <input type="submit" class="rb-btn rb-save" onmouseover="this.className='rb-btn rb-save-hover'"
         value="" onmouseout="this.className='rb-btn rb-save'" />
      <input type="button" id="btnCancelTop" class="rb-btn rb-cancel" onmouseover="this.className='rb-btn rb-cancel-hover'"
         onmouseout="this.className='rb-btn rb-cancel'" />
   </div>
   <% End Using%>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="bottomScripts" runat="server">

   <script type="text/javascript" language="javascript">

      //      onchange = "char_count(this,'lblCharLeft',300);word_count(this,'lblWordUsed');tolcnt(this,'lblCharUsed');"
      //      onkeydown = "char_count(this,'lblCharLeft',300);word_count(this,'lblWordUsed');tolcnt(this,'lblCharUsed');"
      //      onkeyup = "char_count(this,'lblCharLeft',300);word_count(this,'lblWordUsed');tolcnt(this,'lblCharUsed');"

      var _charLeft = $('#lblCharLeft');

      $(document).ready(function() {
         $("#Abstract").keyup(function() {
            UpdateCharCount($(this));
         }).keydown(function() {
            UpdateCharCount($(this));
         }).change(function() {
            UpdateCharCount($(this));
         });
      });

      function UpdateCharCount(item) {
         char_count(item, _charLeft, 300);
         word_count(item, _charLeft);
         tolcnt(item, _charLeft);
      }

      function char_count(field, cntfield, maxlimit) {
         if (field.val().length > maxlimit) // if too long...trim it!
            field.val(field.val().substring(0, maxlimit));
         // otherwise, update 'characters left' counter
         else
            cntfield.val(maxlimit - field.val().length);
      }

      function tolcnt(w, x) {
         var y = w.value;
         a = y.replace(/\n/g, '');
         b = a.replace(/\r/g, '');
         z = b.length;

         x = $('input[id=' + x + ']');
         x.val(z);
      }

      function word_count(w, x) {
         var y = w.value;
         var r = 0;
         a = y.replace(/\s/g, ' ');
         a = a.split(' ');
         for (z = 0; z < a.length; z++) { if (a[z].length > 0) r++; }
         x = $('input[id=' + x + ']');
         x.val(r);
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
   </script>

</asp:Content>
