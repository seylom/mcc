<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Site/Views/Shared/TwoCol.master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="headItems">
   <%=appHelpers.CssTagUrl("print.css", "print")%>
   <%=appHelpers.AdvancedCssTagUrl("article.css", MCC.Utils.FileVersion("articlecss"))%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="middlePlaceHolder" runat="server">
   <asp:HiddenField ID="hiddenAdviceID" runat="server" Value="0" />
   <h3 class="artTitle">
      <asp:Label ID="lblTitle" runat="server"></asp:Label>
   </h3>
   <asp:Label ID="lblNotApproved" runat="server" Text="Not approved"></asp:Label>
   <div class="byLine">
      <asp:Label ID="lblPubDate" runat="server" ForeColor="#6a6868" Font-Names="arial"></asp:Label><span>&nbsp;|&nbsp;</span>
      <span class="dblink">
         <asp:HyperLink ID="dbLnk" runat="server" NavigateUrl="#">Discussion board &raquo;</asp:HyperLink></span>
   </div>
   <div class="filed-under fubx">
      <span>Categories : <span class="categories" id="lblCategories" runat="server" style="color: DarkRed;
         font-family: arial"></span></span>
   </div>
   <div id="pnTools" runat="server" class="clear">
      <div class="artOps" style="margin: 10px 0;">
         <asp:Panel ID="panEditAdvice" runat="server">
            <asp:ImageButton ID="btnApprove" runat="server" AlternateText="Approve Advice" CausesValidation="false"
               ImageUrl="~/_assets/images//MicroIcons/ok_img.gif" OnClick="btnApprove_Click"
               OnClientClick="if (confirm('Are you sure you want to approve this Advice?') == false) return false;" />
            &nbsp;<asp:HyperLink ID="lnkEditAdvice" runat="server" ImageUrl="~/_assets/images//MicroIcons/edit.png"
               NavigateUrl="~/Admin/Tips/AddEditAdvice.aspx?ID={0}" ToolTip="Edit Advice"></asp:HyperLink>
            &nbsp;<asp:ImageButton ID="btnDelete" runat="server" AlternateText="Delete Advice"
               CausesValidation="false" ImageUrl="~/_assets/images//MicroIcons/delete.png" OnClick="btnDelete_Click"
               OnClientClick="if (confirm('Are you sure you want to delete this Advice?') == false) return false;" /></asp:Panel>
      </div>
   </div>
   <div class="artBody" style="margin: 10px 0;">
      <asp:Literal ID="lblBody" runat="server"></asp:Literal>
   </div>
   <h2 style="font-size: 11px; font-style: italic;">
      What did they think about it?</h2>
   <div style="margin: 10px 0 3px 0;">
      They agree - (<asp:Label ID="lblAgree" runat="server">-</asp:Label>)
   </div>
   <div id="votebar_yes" runat="server" style="height: 10px; width: 0; background-color: #4749ab;">
   </div>
   <div style="margin: 10px 0 3px 0;">
      They disagree - (<asp:Label ID="lbldisagree" runat="server">-</asp:Label>)
   </div>
   <div id="votebar_no" runat="server" style="height: 10px; width: 0; background-color: #9e4b35;">
   </div>
   <div class="artTools">
      <a id="commentsLink" runat="server" href="#" class="globalred">Comments (<asp:Label
         runat="server" ID="bottomCommentCount"></asp:Label>)</a>
   </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="rightPlaceHolder" runat="Server">
   <div class="widget">
      <h3 class="title">
         Tags</h3>
      <div>
         <div id="sdTags" runat="server" style="padding: 5px;">
         </div>
      </div>
   </div>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="scriptLoader">
   <%=appHelpers.ScriptsTagUrl("jquery/jquery.metadata.min.js")%>
   <%=appHelpers.ScriptsTagUrl("jquery/jquery.rating.js")%>
</asp:Content>
