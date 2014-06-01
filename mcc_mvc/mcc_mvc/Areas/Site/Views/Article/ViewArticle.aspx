<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Site/Views/Shared/TwoCol.master" Inherits="System.Web.Mvc.ViewPage(of ArticleViewModel)" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headItems">
   <%=appHelpers.AdvancedCssTagUrl("article.css", MCC.Utils.FileVersion("articlecss"))%>
   <%=appHelpers.AdvancedCssTagUrl("user_article.css", MCC.Utils.FileVersion("user_articlecss"))%>
   <%=appHelpers.CssTagUrl("jquery.rating.css")%>
   <link rel="image_src" id="_articleImageDefault" href="<%=Configs.Paths.CdnRoot & "/imageThumb.ashx?img=" & Html.Encode(Configs.Paths.CdnImages &  Model.Article.ImageNewsUrl) & "&w=200&h=100" %>" />


</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
   <%=Html.Encode(Model.PageTitle)%>
</asp:Content>
<asp:Content ContentPlaceHolderID="Meta" runat="server">
   <meta name="Description" content="<%= Model.MetaDescription %>" />
   <meta name="Keywords" content="<%= Model.MetaKeywords %>" />

   <meta property="og:title" content="<%= Model.Article.Title %>"/>
   <meta property="og:type" content="Article"/>
   <meta property="og:url" content="<%= Request.Url.AbsoluteUri %>"/>
   <meta property="og:image" content="<%=Configs.Paths.CdnRoot & "/imageThumb.ashx?img=" & Html.Encode(Configs.Paths.CdnImages &  Model.Article.ImageNewsUrl) & "&w=200&h=100" %>" />
   <meta property="og:site_name" content="MiddleClassCrunch"/>
   <meta property="fb:admins" content="USER_ID"/>
   <meta property="og:description" content="<%= Model.MetaDescription %>"/>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="middlePlaceHolder" runat="server">
   <%Html.RenderPartial("~/Areas/Site/views/shared/articles/articleViewer.ascx", Model)%>
   <%= Html.Hidden("ArticleId",Model.Article.ArticleID) %>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="rightPlaceHolder" runat="Server">
   <div class="widget">
      <div id="paneArtImage" style="padding-left: 5px; margin-top: 10px;">
         <img class="arpic" width="200px" height="100px" id="imgPreview" alt="Article Image"
            src="<%= Configs.Paths.CdnRoot & "/imageThumb.ashx?img=" &  ViewData.Model.Article.ImageNewsUrl.UrlEncode() & "&w=200&h=100" %>" />
      </div>
      <table width="100%" class="artInfo">
         <tr>
            <td style="width: 40%;">
               <strong>Rating :</strong>
            </td>
            <td align="left">
               <div class="ratingwidget" style="display: none;">
                  <div id="yr">
                     <input name="star2" id="_ort1" title="very poor" type="radio" class="star {half:true}"
                        value="1" disabled="disabled" />
                     <input name="star2" id="_ort2"  title="poor" type="radio" class="star {half:true}"
                        value="2" disabled="disabled" />
                     <input name="star2" id="_ort3"  title="ok" type="radio" class="star {half:true}"
                        value="3" disabled="disabled" />
                     <input name="star2" id="_ort4"  title="good" type="radio" class="star {half:true}"
                        value="4" disabled="disabled" />
                     <input name="star2" id="_ort5"   title="excellent" type="radio" class="star {half:true}"
                        value="5" disabled="disabled" />
                     <input name="star2" id="_ort6"   title="very poor 2" type="radio" class="star {half:true}"
                        value="6" disabled="disabled" />
                     <input name="star2" id="_ort7"  title="poor 2" type="radio" class="star {half:true}"
                        value="7" checked="true" disabled="disabled" />
                     <input name="star2" id="_ort8"  title="ok 2" type="radio" class="star {half:true}"
                        value="8" disabled="disabled" />
                     <input name="star2" id="_ort9"  title="good 2" type="radio" class="star {half:true}"
                        value="9" disabled="disabled" />
                     <input name="star2" id="_ort10"   title="excellent 2" type="radio" class="star {half:true}"
                        value="10" disabled="disabled" />
                  </div>
               </div>
            </td>
         </tr>
         <tr>
            <td>
               <strong>Your Rating :</strong>
            </td>
            <td>
               <div class="ratingwidget" style="display: none;">
                  <div id="rtbx" style="font-weight: bold;">
                     <input name="star1" id="_rt1" title="very poor" type="radio" class="star"
                        value="1" />
                     <input name="star1" id="_rt2" title="poor" type="radio" class="star"
                        value="2" />
                     <input name="star1" id="_rt3" title="ok" type="radio" class="star"
                        value="3" />
                     <input name="star1" id="_rt4" title="good" type="radio" class="star"
                        value="4" />
                     <input name="star1" id="_rt5" title="excellent" type="radio" class="star"
                        value="5" />
                  </div>
               </div>
            </td>
         </tr>
         <tr>
            <td>
               <strong><span>Views : </span></strong>
            </td>
            <td>
               <%=ViewData.Model.Article.ViewCount & If(ViewData.Model.Article.ViewCount = 1, " time", " times")%>
            </td>
         </tr>
      </table>
   </div>
   <div class="widget">
      <h3 class="title">
         Tags</h3>
      <div>
         <div id="sdTags" style="padding: 5px;">
            <% For Each tg As String In Model.ArticleTags%>
            <a class="global tags" href="<%= "/search?q=" & tg %>" title="<%= Html.Encode(tg) %>">
               <%= Html.Encode(tg)%></a> <span>|</span>
            <% Next%>
         </div>
      </div>
   </div>
   <div class="adwidget">
      <% Html.RenderPartial("~/Areas/Site/views/shared/ads/adLargeSquareGoogle.ascx")%>
   </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="scriptLoader" runat="server">
   <%=appHelpers.ScriptsTagUrl("global.js")%>
   <%=appHelpers.ScriptsTagUrl("stagging/polls.stagging.js")%>

   <script type="text/javascript">
      //<![[CDATA

      var Proxy = new serviceProxy('/articles/');
      $(document).ready(function() {
         $('input[type=radio].star', '#yr').rating();
         $('input[type=radio].star', '#rtbx').rating({
            callback: function(value, link) {
               if (value && value != '') {
                  var _idval = parseInt($('#ArticleId').val());
                  var DTO = {'Id': _idval, 'rating' : parseInt(value)};
                  //fUrl = fUrl + _idval + '&val=' + value;

                  Proxy.invoke('RateArticle',DTO, function(result) {
                     if (result == true) {
                        $('#rtbx').html("Thank you!");
                     }
                     else {
                        alert('unable to process your vote at this time.');
                     }
                  });
               }
               else {
                  alert('you rating has been canceled')
               }
            }
         });

         jQuery('img.caption').addcaption();

         $('.ratingwidget').each(function(item) {
            $(this).show();
         });

         $('#font-small').click(function() {
            $('#artBody').removeClass("fmedium");
         });
         $('#font-medium').click(function() {
            $('#artBody').addClass("fmedium");
         });

         // init polls
         Polls.Init('<%=ResolveUrl("~/asv.asmx/") %>');
      });

      var ads_ar = [];


      $(function() {
         $("#ads").children().each(function(it, item) {
            ads_ar.push($(item));
         });
         $("#ads").empty();

         var cCount = $("p", "#artBody").length;

         // last ad should be at 70% of the article to fix size problems ...

         var ad_count = 0;

         var elts = $("p", "#artBody");
         if ((cCount <= 5) && (cCount > 0)) {
            ad_count = 1;
         }
         else if ((cCount <= 15) && (cCount > 3)) {
            ad_count = 3;
         }
         else if (cCount != 0) {
            ad_count = 1;
         }

         var step = 3;
         if (ads_ar.length == 1) {
            if (cCount >= 6) {
               step = 3
            }
         }
         else {
            var modval = Math.floor(cCount / ads_ar.length);
            if (modval < 3) {
               step = 3
            } else { step = modval; }
         }
         for (var i = 1; i <= ad_count; i++) {
            var k = i * step;
            if ((elts[k]) && (k < cCount - 3)) {
               var cls = Math.floor(Math.random() * 2) == 0 ? 'adl' : 'adr';
               ads_ar[i - 1].addClass(cls).insertAfter(elts[k]);
            }
         }
      });

      //]]>
   </script>

</asp:Content>
