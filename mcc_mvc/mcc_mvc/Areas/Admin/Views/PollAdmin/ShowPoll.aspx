<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage(of AdminPollViewModel)" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
View Poll:
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainSiteContent" runat="Server">
   <div style="border-bottom: 1px solid #eaeaea; padding: 5px; margin-bottom: 10px;">
      <h1 class="admin-title">
         Add - Edit Poll Options</h1>
   </div>
   <div style="padding: 10px;">
      <b>Poll Question :</b>
      <%=Model.QuestionText%>
   </div>
   <div style="padding: 0 20px;">
      <span>Options : -</span>
   </div>
   <%For Each it As PollOption In Model.Options%>
   <div style="padding: 5px; background-color: #eaeaea; margin: 10px 0;">
      <%=it.OptionText%>
      <a href="#" title="Edit Option">Edit</a>
      <%Using Html.BeginForm("DeleteOption", "PollAdmin", New With {.Id = it.PollOptionID, .pollId = it.PollID})%>
      <input type="image" src="<%= apphelpers.ImageUrl("MicroIcons/delete.png") %>" />
     <%-- <% Html.ActionLink("Delete", "DeleteOption", New With {.Id = it.PollOptionID, .pollId = it.PollID})%>--%>
      <% End Using%>
   </div>
   <% Next%>
   <div>
      [ <a class="global" title="Add Option" href="<%=Url.Action("AddPollOption",new with {.PollId = Model.PollID}) %>">
         Add Option</a> ]
   </div>
   <%-- <div style="padding: 10px; background-color: #eaeaea; margin: 10px;">
      <ucp:poll runat="server" ID="pollPreview" PollId="1" />
   </div>--%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottomScripts" runat="Server">
   <%=appHelpers.ScriptsTagUrl("jquery/jquery.editinplace.packed.js")%>

   <script type="text/javascript">
      //        $(".option_boxes").editInPlace({
      //            url: '<%= ResolveUrl("~/ajaxdata.ashx") %>',
      //            show_buttons: true,
      //            bg_over: "#cff",
      //            saving_image: '<%= ResolveUrl("~/_assets/images/loader.gif") %>'
      //        });
        
   </script>

</asp:Content>
