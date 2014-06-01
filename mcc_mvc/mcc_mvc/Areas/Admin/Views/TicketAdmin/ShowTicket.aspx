<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage(of TicketViewModel)" %>

<%@ Import Namespace="MCC.Services" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
   Show Tickets
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainSiteContent" runat="Server">
   <div style="border-bottom: 1px solid #eaeaea; padding: 5px; margin-bottom: 10px;">
      <h1 class="admin-title">
         Ticket Detail</h1>
   </div>
   <div>
      <h1 class="ticket">
         <%--<asp:Label ID="lblTicket" runat="server"></asp:Label>--%>
      </h1>
      <div id="ticket">
         <div style="float: right; width: 200px;">
            Opened -
            <%=Model.AddedDate%>
         </div>
         <h2 style="font-size: 12px; font-weight: bold; margin-bottom: 20px;">
            <%=Html.Encode(Model.Title)%>
         </h2>
         <table id="properties">
            <tr>
               <td class="right">
                  Reported by:
               </td>
               <td class="left">
                  <%=Html.Encode(Model.AddedBy)%>
               </td>
               <td class="right">
                  Type:
               </td>
               <td class="left">
                  <%=TicketService.GetTypeCaption(Model.Type)%>
               </td>
               <td class="right">
               </td>
               <td class="left">
               </td>
               <td class="right">
                  Priority:
               </td>
               <td class="left">
                  <%=TicketService.GetPriorityCaption(Model.Priority)%>
               </td>
               <td class="right">
                  Status:
               </td>
               <td class="left">
                  <%=TicketService.GetStatusCaption(Model.Status)%>
               </td>
            </tr>
         </table>
         <h2 style="font-size: 12px; border-bottom: 1px solid #bababa; margin-bottom: 10px;">
            <span>Description</span></h2>
         <div>
            <%=Html.Encode(Model.Description)%>
         </div>
      </div>
      <div id="help">
      </div>
   </div>
   <div style="margin-top: 20px; padding: 5px 10px; background-color: #f1f1f1;">
      <h2 style="font-size: 10px; font-weight: bold;">
         <a href="javascript:void(0);" class="global" onclick="History()">Change History</a></h2>
      <div id="history" style="margin-top: 10px; display: none;">
         <% If Model.TicketChanges IsNot Nothing Then%>
         <% For Each it As TicketChange In Model.TicketChanges%>
         <div style="margin: 5px 0; background-color: #f1f1f1; padding: 5px;">
            <span style="color: #935045; border-bottom: 1px dotted #bababa;">
               <%="changed on " & it.AddedDate.ToString("MM dd yyyy") & " at " & it.AddedDate.ToString("d") & " by " & it.AddedBy%></span>
            <div style="margin-top: 5px; padding: 0 10px;">
               <%= it.Body%>
            </div>
            <div style="margin-top: 5px; background-color: #f8fafc">
               <%Using Html.BeginForm("DeleteTicketChange", "TicketAdmin", New With {.Id = it.TicketChangeID})%>
               <input type="image" src="<%= apphelpers.ImageUrl("MicroIcons/delete.png") %>" onclick="if (confirm('Are you sure you want to delete this ticket change?') == false) return false;" />
               <%End Using%>
               <%--     <%=Html.ActionLink("Delete", "DeleteTicketChange", New With {.Id = it.TicketChangeID, .onclick = "if (confirm('Are you sure you want to delete this ticket change?') == false) return false;"})%>--%>
            </div>
         </div>
         <% Next%>
         <% End If%>
      </div>
   </div>
   <% Using Html.BeginForm(New With {.action = "CreateTicketChange", .Id = Model.TicketID})%>
   <div style="margin-top: 10px; padding: 5px 10px; background-color: #f1f1f1">
      <h2 style="font-size: 10px; font-weight: bold; margin-bottom: 5px;">
         <a href='javascript:void(0);' class="global" onclick="CommentSetup();return false;">
            Change Comments</a></h2>
      <div id="comment_window" style="display: none;">
         <%=Html.TextArea("Body", "", New With {.rows = 5, .cols = 12, .style = "width:99%", .class = "mccEditor rtc"})%>
         <div style="margin-top: 10px;">
            <input type="submit" class="rb-btn rb-update" onmouseover="this.className='rb-btn rb-update-hover'"
               value="" onmouseout="this.className='rb-btn rb-update'" />
            <input type="button" class="rb-btn rb-cancel" onmouseover="this.className='rb-btn rb-cancel-hover'"
               onmouseout="this.className='rb-btn rb-cancel'" />
         </div>
      </div>
   </div>
   <% End Using%>
   <div style="margin-top: 10px;">
      <div style="padding: 5px 10px; background-color: #eaeaea">
         <h2 style="font-size: 10px; font-weight: bold; margin-bottom: 5px;">
            <a href='javascript:void(0);' onclick="PropertiesSetup();return false;" class="global">
               Change Properties</a></h2>
         <div id="prop_window" style="display: none;">
            <table class="edit-info">
               <tr>
                  <td class="right" style="width: 100px;">
                     Title
                  </td>
                  <td class="left">
                     <%=Html.TextBox("Title", Model.Title, New With {.class = "rtc", .style = "width:99%"})%>
                  </td>
               </tr>
               <tr>
                  <td class="right">
                     Type
                  </td>
                  <td class="left">
                     <%=Html.DropDownList("Type", CType(Model.Type, TicketType).ToSelectList())%>
                  </td>
               </tr>
               <tr>
                  <td class="right">
                     Owner
                  </td>
                  <td class="left">
                     <%--<asp:DropDownList runat="server" ID="ddlPropOwner" AppendDataBoundItems="true">
                        <asp:ListItem Text="unassigned" Value="unassigned"></asp:ListItem>
                     </asp:DropDownList>--%>
                  </td>
               </tr>
               <tr>
                  <td class="right">
                     State
                  </td>
                  <td class="left">
                     <%=Html.DropDownList("State", CType(Model.Type, TicketState).ToSelectList())%>
                  </td>
               </tr>
               <tr>
                  <td style="width: 100px;" class="right">
                     Piority
                  </td>
                  <td class="left">
                     <%=Html.DropDownList("Piority", CType(Model.Type, TicketPriority).ToSelectList())%>
                  </td>
               </tr>
               <tr>
                  <td class="right">
                     Description
                  </td>
                  <td class="left">
                     <%=Html.TextArea("Description", Model.Description, New With {.class = "mccEditor rtc", .rows = 8, .cols = 12, .style = "width:99%"})%>
                  </td>
               </tr>
            </table>
         </div>
      </div>
      <div style="margin-top: 10px;">
         <input type="submit" class="rb-btn rb-update" onmouseover="this.className='rb-btn rb-update-hover'"
            value="" onmouseout="this.className='rb-btn rb-update'" />
         <input type="button" class="rb-btn rb-cancel" onmouseover="this.className='rb-btn rb-cancel-hover'"
            onmouseout="this.className='rb-btn rb-cancel'" />
      </div>
   </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottomScripts" runat="Server">
   <%=appHelpers.ScriptsTagUrl("tinymce/jscripts/tiny_mce/tiny_mce_gzip.js")%>

   <script type="text/javascript">

      var _commentLoaded = false;
      var _propLoaded = false;

      function LoadMCE_Gzip() {
         tinyMCE_GZ.init({
            theme: "advanced",
            mode: "none",
            mode: "textareas",
            editor_selector: "mccEditor",
            disk_cache: true
         }, function() {
            tinyMCE.init({
               theme: "advanced",
               mode: "none",
               mode: "textareas",
               editor_selector: "mccEditor"
            });

         });
      }

      var _commentWindow = null;
      function CommentSetup() {
         if (_commentWindow === null) {
            _commentWindow = $("#comment_window")[0];
         }
         $(_commentWindow).toggle();

         if ((!_commentLoaded) && ($("#comment_window:visible").length > 0)) {
            LoadMCE_Gzip();
            _commentLoaded = true;

         }

      }

      var _propWindow = null;
      function PropertiesSetup() {
         if (_propWindow === null) {
            _propWindow = $("#prop_window")[0];
         }
         $(_propWindow).toggle();

         if ((!_propLoaded) && ($("#prop_window:visible").length > 0)) {
            LoadMCE_Gzip();
            _propLoaded = true;

         }

      }

      function History() {
         $("#history").toggle("fast");
      }
   </script>

</asp:Content>
