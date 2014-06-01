<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Site/Views/Shared/singlewrapper.master"
   Inherits="System.Web.Mvc.ViewPage(of videosViewData)" %>

<asp:Content ID="Content3" ContentPlaceHolderID="PageTitle" runat="server">
   Videos
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="middlePlaceHolder" runat="Server">
   <%=""%>
   <div  style="overflow: hidden;background-color:#fff; padding:10px;">
   <div style="margin-bottom:10px;color:#555555;">
   <h2>Ask MCC Videos</h2>
   </div>
      <div style="overflow: hidden;">
         <table width="100%;" style="border-collapse: collapse;">
            <tr>
               <td style="width: 610px;">
                  <div style="width: 608px; height: 342px; background-color: #000000">
                     <div id="preview" style="height: 100%; text-align: center; vertical-align: middle;
                        color: White;">
                     </div>
                  </div>
               </td>
               <td>
                  <div style="margin-left: 15px;">
                     <div style="height: 220px; background-color: #f5f5f5;">
                        <center>
                           ads go here!</center>
                     </div>
                     <div style="height: 112px; background-color: #eaeaea;">
                        <h3 id="stitle">
                        </h3>
                        <div id="sabstr" style="padding: 3px 5px;">
                        </div>
                     </div>
                  </div>
               </td>
            </tr>
         </table>
      </div>
      <div style="position: relative;">
         <div style="padding: 5px; background-color: #eaeaea; margin: 5px 0;">
         </div>
         <table width="100%" style="border-collapse: collapse;">
            <tr>
               <td style="width: 200px;" valign="top">
                  <div style="background-color: #f1f1f1; padding: 10px 0;">
                     <ul class="category-menu">
                        <li class="bullet" onmouseover="this.className='bullet_hover'" onmouseout="this.className='bullet'">
                           <span style="width: 8px; height: 8px"></span><a href="/videos/">All</a> </li>
                        <% For Each it As VideoCategory In Model.categories%>
                        <li class="bullet" onmouseover="this.className='bullet_hover'" onmouseout="this.className='bullet'">
                           <a href='<%= "/videos/Topics/" & it.Slug & "/" %>'>
                              <%= html.Encode(it.Title)%></a></li>
                        <%Next%>
                     </ul>
                  </div>
               </td>
               <td>
                  <%If Model.videos Is Nothing Or Model.videos.Count = 0 Then%>
                  <div style="margin: 5px 0; padding: 20px; background-color: #fff; border: #bababa;
                     color: #a5370a">
                     <center>
                        <b>No video found for the specified category, please try again later!</b>
                     </center>
                  </div>
                  <% Else%>
                  <div id="vd-cnt">
                     <ul id="vdlst">
                        <% For Each it As Video In Model.videos%>
                        <li>
                           <div style="position: relative">
                              <div class="vdi-cnt" style="position: relative;">
                                 <a id="item_<%= it.VideoId %>" class="videolnk" href='javascript:void(0);'>
                                    <div class="vilnk">
                                       <img class="vi" style="border: 0 none;" width="160px" height="100px" src='<%= Configs.paths.cdnRoot & Configs.paths.CdnVideos & it.Name & "/default.jpg"%>'
                                          alt="<%= html.Encode(it.Title) %>" />
                                    </div>
                                 </a>
                                 <div id='<%= "vo_" & it.VideoId %>' class="vplay overlay" style="display: none;">
                                    <span>Now playing ...</span>
                                 </div>
                              </div>
                              <div>
                                 <div class="vt">
                                    <a id="videoitem_<%= it.VideoId %>"  class="vtlnk" href='javascript:void(0);'>
                                       <%=Html.Encode(it.Title)%>
                                    </a>
                                 </div>
                                 <div class="vtime">
                                    <%=routines.ToMinutesAndSeconds(it.Duration)%>
                                 </div>
                              </div>
                           </div>
                        </li>
                        <% Next%>
                     </ul>
                  </div>
                  <%=Html.SimplePager(Model.videos)%>
                  <%End If%>
               </td>
            </tr>
         </table>
      </div>
   </div>
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="scriptLoader">
   <%=appHelpers.ScriptsTagUrl("global.js")%>
   <%=appHelpers.ScriptsTagUrl("flowplayer.js")%>
   <%=appHelpers.ScriptsTagUrl("video.js")%>

   <script type="text/javascript">
      //<![CDATA[

      $(document).ready(function() {
         Videos.Init('/videos/', '<%= Configs.Paths.CdnRoot & Configs.Paths.CdnVideos %>',0);
      });
      //]]>
   </script>
</asp:Content>
