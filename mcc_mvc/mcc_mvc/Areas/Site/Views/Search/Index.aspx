<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Site/Views/Shared/TwoCol.master" Inherits="System.Web.Mvc.ViewPage(of SearchResultsViewModel)" %>

<%@ Import Namespace="MCC.Services" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
   Search
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="middlePlaceHolder" runat="Server">
   <%=""%>
   <h2 class="head-title">
      Search Results</h2>
   <div id="searchPanelCt" style="margin: 10px;">
      <% Using Html.BeginForm(New With {.q = Model.Keywords})%>
      <table cellspacing="5">
         <tr>
            <td>
               Search for
            </td>
            <td>
               <input type="text" id="q" name="q" value="<%= Model.Keywords %>" class="rtc" style="width: 200px;" />
            </td>
            <td>
               in
            </td>
            <td>
               <%=Html.DropDownList("LocationSearch", CType(Model.Location, SearchLocation).ToSelectList)%>
            </td>
            <td>
               <%=Html.DropDownList("TypeSearch", CType(Model.Type, SearchType).ToSelectList)%>
            </td>
            <td>
               <input type="submit" class="rb-btn rb-search" onmouseover="this.className='rb-btn rb-search-hover'"
                  onmouseout="this.className='rb-btn rb-search'" value="" />
            </td>
         </tr>
      </table>
      <% End Using%>
   </div>
   <div style="margin-top: 10px;">
      <h5>
         Results:
      </h5>
      <div id="searchResultsBox">
         <%-- <div style="padding: 10px;">
         <span>Show me ...</span>&nbsp <span>
         <a href="javascript:void(0);"  onclick="Activatetab(this);">Everything</a></span>
         <asp:Label runat="server" ID="lblAllCount"></asp:Label>&nbsp|&nbsp;
         <a class="sr sr-articles" href="javascript:void(0);" onclick="Activatetab(this);">
                            Articles </a><asp:Label runat="server" ID="lblArticlesCount"></asp:Label>&nbsp|&nbsp;
         <a class="sr sr-videos" OnClientClick="Activatetab(this);">
                            Videos </a><asp:Label runat="server" ID="lblVideosCount"></asp:Label>&nbsp|&nbsp;
         <a class="sr sr-comments"
            onclick="Activatetab(this);">Comments</asp:LinkButton>
         <asp:Label runat="server" ID="lblCommentsCount"></asp:Label>
      </div>--%>
         <% If Model.Results IsNot Nothing Then%>
         <%=Html.AdvancedPager(Model.Results, "Index", "Search", New Integer() {10, 30, 50})%>
         <%For Each it As SearchResult In Model.Results%>
         <div style="padding: 10px; background-color: #fafafa; margin-bottom: 5px;">
            <h5>
               <a class='<%="search-result-title " & it.IconClassName%>' href='<%=it.Url%>'>
                  <%=routines.HighlightKeyword(Html.Encode(it.Title), Model.Keywords)%>
               </a>
            </h5>
            <div style="margin: 5px 0;">
               <p>
                  <%=routines.HighlightKeyword(it.Body.CleanString(), Model.Keywords)%>
               </p>
            </div>
            <div class="search-result-url">
               <span style="font-size: 10px;">
                  <%=routines.FullBaseUrl() & it.Url%></span>
            </div>
         </div>
         <% Next%>
         <%=Html.AdvancedPager(Model.Results, "Index", "Search", New Integer() {10, 30, 50})%>
         <% End If%>
      </div>
   </div>
   <%--    <script type="text/javascript">
        function toggleTips() {
            var search_tip = $('#searchPanelCt')[0];
            if (search_tip.style.display == "none") {
                search_tip.style.display = "block";
            }
            else {
                search_tip.style.display = "None";
            }

            //$("searchPanelCt")[0].style.display = "block";
        }

        function Activatetab(elt) {
            var sender = elt;
            var li = $('#searchCategory > li');
            if ((li) && (li.length > 0)) {
                for (var i = 0, len = li.length; i < len; i++) {
                    li[i].className = (li[i].id == sender.parentNode.id) ? "search-category-current" : "";
                }
            }
        }
    </script>--%>
</asp:Content>
