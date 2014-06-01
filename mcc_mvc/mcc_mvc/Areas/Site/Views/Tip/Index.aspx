<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Site/Views/Shared/fullscreen.master"
   Inherits="System.Web.Mvc.ViewPage(of TipsViewData)" %>

<asp:Content ID="Content4" ContentPlaceHolderID="PageTitle" runat="server">
   Tips
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="headerPlaceHolder" runat="Server">
   <%=appHelpers.CssTagUrl("tests.css")%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainSiteContent" runat="Server">
   <%="" %>
   <table id="tip-table" width="100%" style="border-collapse: collapse;">
      <tr>
         <td valign="top">
            <%If Model.Tips IsNot Nothing AndAlso Model.Tips.Count > 0 Then%>
            <div id="tp-items" style="display: none;">
               <%For Each it As Advice In Model.Tips%>
               <div id='<%="item_" & it.AdviceID  %>'>
                  <a class="advice-title" href='<%= "/Tips/" & it.Slug %>'>
                     <%= Html.Encode(it.Title) %></a>
                  <div style="border-top: 1px solid #eaeaea;">
                     <div class="tip-body" onclick="window.document.location='<%= "/Tips/" & it.Slug%>'">
                        <%=Html.Encode(it.Abstract)%>
                     </div>
                     <div id='notifybox_<%= it.AdviceID%>' class="notifybox">
                        <span id='notify_<%= it.AdviceID%>'></span>
                     </div>
                     <div id='votes_<%= it.AdviceID%>' class="votesbox">
                        <span class="agree"><i>do <b>you</b> agree?</i></span>
                        <div style="margin-top: 10px;">
                           <a href="javascript:void(0);" id="voteuplnk_<%= it.AdviceID %>" class="vote_yes">Yes</a>
                           <span style="font-size: 10px;">vs</span> <a href="javascript:void(0);" id="votedownlnk_<%= it.AdviceID %>"
                              class="vote_no">No&nbsp;</a>
                        </div>
                     </div>
                     <div id='result_<%= it.AdviceID%>' class="resultsbox">
                        <span class="agree"><i>do <b>they</b> agree?</i></span>
                        <div style="text-align: left;">
                           <div>
                              <span>Yes</span> - <span id='votetext_yes_<%= it.AdviceID%>' style="color: #7e7e7e;">
                                 (<%=it.VoteUp%>)</span>
                           </div>
                           <div id='votebar_yes_<%= it.AdviceID%>' style="width: 0px; height: 5px; background-color: #4749ab;">
                           </div>
                           <div class="vote-details">
                              No - <span id='votetext_no_<%= it.AdviceID%>' style="color: #7e7e7e;">(<%=it.VoteDown%>)</span>
                           </div>
                           <div id='votebar_no_<%= it.AdviceID%>' style="width: 0px; height: 5px; background-color: #9e4b35;">
                           </div>
                        </div>
                     </div>
                     <div id='vote_discuss_<%= it.AdviceID%>' style="text-align: left; margin-top: 5px;">
                        <a id='result_link_<%= it.AdviceID%>' href="javascript:void(0);" class="global" onclick='showResults(<%= it.AdviceID %>,true);return false;'>
                           Results</a>&nbsp;|&nbsp;<a id='vote_link_<%= it.AdviceID%>' href="javascript:void(0);"
                              class="global" onclick='showVoteLinks(<%= it.AdviceID %>);return false;'> Vote</a>&nbsp;|&nbsp;<a
                                 id="A2" href='<%= "/Tips/" & it.slug & "/?comments_page" %>' class="global">Discuss</a>
                     </div>
                  </div>
               </div>
               <%Next%>
            </div>
            <%Else%>
            <div style="margin: 5px 0; background-color: #fff; border: #bababa; color: #a5370a">
               <center>
                  <b>No tips or advices in the specified category, please try again later!</b>
               </center>
            </div>
            <% End If%>
            <div id="items" style="overflow: hidden;">
            </div>
            <%=Html.SimplePager(Model.Tips)%>
         </td>
         <td style="width: 250px;" valign="top">
            <div class="widget" style="margin: 5px 0 5px 5px;">
               <h3 class="title">
                  Categories</h3>
               <div style="padding: 10px;">
                  <ul class="category-menu">
                     <li class="bullet"><span style="width: 8px; height: 8px"></span><a href="/Tips/">All</a>
                     </li>
                     <% For Each it As AdviceCategory In Model.categories%>
                     <li class="bullet" onmouseover="this.className='bullet_hover'" onmouseout="this.className='bullet'">
                        <a href='<%= "/Tips/Topics/" & it.Slug & "/" %>'>
                           <%=Html.Encode(it.Title) %></a> </li>
                     <%Next%>
                  </ul>
               </div>
            </div>
         </td>
      </tr>
   </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottomScripts" runat="Server">
   <%=appHelpers.ScriptsTagUrl("jquery/jquery.effects.js")%>
   <%=appHelpers.ScriptsTagUrl("global.js")%>
   <%=appHelpers.ScriptsTagUrl("stagging/tips.stagging.js")%>

   <script type="text/javascript">
      //<![CDATA[

      $(document).ready(function() {
         Tips.Init('/tips/');

      });

      //]]>
   </script>

</asp:Content>
