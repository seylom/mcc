<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Site/Views/Shared/fullscreen.master" Inherits="System.Web.Mvc.ViewPage(of List(of mcc_category))" %>

<%@ Register Assembly="MccControls" Namespace="MccControls" TagPrefix="cc1" %>
<%@ Register Assembly="MccControls" Namespace="MccControls.MCC.WebControls" TagPrefix="mcc" %>
<%@ Import Namespace="MCC.UserAccountRoutines" %>
<%@ Import Namespace="MCC.Data" %>
 
<asp:Content ID="Content4" ContentPlaceHolderID="PageTitle" runat="server">
Article Topics
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="headerPlaceHolder" runat="Server">
   <%=appHelpers.CssTagUrl("tests.css")%>
   <%=appHelpers.AdvancedCssTagUrl("articles.css", MCC.Utils.FileVersion("articlecss"))%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainSiteContent" runat="Server">
   <% If ViewData.Model.Count = 0 Then%>
   <div style="margin: 5px 0; padding: 20px; background-color: #fff; border: #bababa;
      color: #a5370a">
      <center>
         <b>There are currently no categories...</b>
      </center>
   </div>
   <% Else%>
   <div id="tp-items" style="display: none;">
      <% Dim itemIndex As Integer = 0%>
      <%For Each cat As mcc_Category In ViewData.Model%>
      <div id='<%="item_" & itemIndex %>'>
         <div style="padding-bottom: 5px;">
            <a  id="lnkTitle" text='<%= Html.Encode(cat.Title) %>' 
               style="text-decoration:none;font-size: 20px; color: #515150; font-family: Georgia;"
               href='<%= "/Articles/Topics/" & cat.Slug & "/" %>'></a>
         </div>
         <div class="mc-abstract" style="border-top: 1px solid #eaeaea;">
            <div style="margin: 10px 0 15px 0">
               <%= Html.Encode(cat.Description) %>
            </div>
         </div>
    <%--     <div>
          <%= "+ " & MCC.Articles.ArticleRepository.GetArticleCount(cat.CategoryID).ToString() & " contribution(s)" %>
         </div>--%>
      </div>
      <% itemIndex += 1%>
        <%Next%>
      
   </div>
 
   <%End If%>
   
  
   <div id="items">
   </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottomScripts" runat="Server">
   <%=appHelpers.AdvancedScriptsTagUrl("mcc_basics.js", MCC.Utils.FileVersion("mcc_basics"))%>

   <script type="text/javascript">
      //<![CDATA[
      var votelist = [];
      function Vote(id, val) {
         var cv = mccUtils.Tools.Get_Cookie("mcc_adv_votelist");
         if (cv) {
            var cvlist = unescape(cv)
            votelist = cvlist.split(";");
            for (var i = 0; i < votelist.length; i++) {
               if (votelist[i] == id) {
                  alert('your vote was already registered');
                  return;
               }
            }
         }
         var vdurl = '<%= ResolveUrl("~/ajaxData.ashx?qId=vote&id=")%>'
         vdurl += id + "&val=" + val;
         $.get(vdurl, SaveVote);
      }


      function SaveVote(result) {
         var rs = result.split("|");
         if (rs[0] == 'success') {
            votelist.push(rs[1]);
            mccUtils.Tools.Set_Cookie("mcc_adv_votelist", votelist.join(";"));
         }
         else {
            alert('We are unable to process you vote at this time.Please try again later.');
         }
      }
      //]]>
   </script>

   <script type="text/javascript">
      //<![CDATA[

      $(document).ready(function() {
         Init();
         window.onresize = _fixLayout;
      });

      $(function() {
         $('#catlnk').click(function() { $("#catlist").toggle(); });
         $('#catlist').bind("mouseleave", function() { $('#catlist').fadeOut(300); });
      });


      function ShowHideCats() {
         $("#catlist").toggle();
      }



      var it_ar = [];
      var _colCount = -1;
      var isInitialized;

      function Init() {
         $("#tp-items").children().each(function(it, item) {
            it_ar.push(item);
         });
         $("#tp-items").empty();

         if (it_ar.length > 0) {
            if (_colCount == -1) {
               _setColCount();
            }
            if (_colCount > 0) {
               for (var i = 0; i < _colCount; i++) {
                  div = document.createElement("div");
                  var id = "col_" + i;
                  $(div).attr("id", id).addClass("tip-col");
                  $("#items").append(div);
               }
               var clr = document.createElement("div");
               $(clr).addClass("clearer");
               $("#items").append(clr);
            }
            //buildBlocks();
            buildAltBlocks();
         }
      }

      var _indexBuilt = 0;

      function buildAltBlocks() {
         if ((it_ar.length > 0) && (_indexBuilt < it_ar.length)) {
            var si = _smalestCol();
            var id = it_ar[_indexBuilt];
            if (id) {
               _indexBuilt += 1;
               $(id).addClass("tip").hover(function() {
                  $(this).css("background-color", "#fffde8");
               }, function() { $(this).css("background-color", "#ffffff"); }).appendTo($(si)).animate({ "backgroundColor": "#ffffff", "opacity": "1" }, "fast", buildAltBlocks);
            }
         }
      }

      function buildBlocks() {
         if (it_ar.length > 0) {
            for (var j = 0; j < it_ar.length; j++) {
               var it = it_ar[j];
               _injectBlock(it);
            }
            isInitialized = true;
         }
      }

      function _injectBlock(id) {
         var si = _smalestCol();
         if (si) {
            $(id).addClass("tip").hover(function() {
               $(this).css("background-color", "#fffde8");
            }, function() { $(this).css("background-color", "#ffffff"); }).appendTo($(si)).animate({ "opacity": "1" }, "slow");
         }
      }

      function _smalestCol() {
         var id, cols = $(".tip-col");
         if (cols) {
            for (var i = 0; i < cols.length; i++) {

               if (id) {
                  if ($(cols[i]).height() < $(id).height())
                     id = $(cols[i]);
               }
               else {
                  id = $(cols[i]);
               }
            }
         }
         return id;
      }

      function _tallestCol() {
         var id, cols = $(".tip-col");
         if (cols) {
            for (var i = 0; i < cols.length; i++) {

               if (id) {
                  if ($(cols[i]).height() > $(id).height())
                     id = $(cols[i]);
               }
               else {
                  id = $(cols[i]);
               }
            }
         }
         return id;
      }

      function _setColCount() {
         var w = $("#items").width();
         _colCount = Math.floor(w / 220);
         return _colCount;
      }

      function _fixLayout(event) {
         if (isInitialized) {
            //$("#items").empty();
            _updatePositions();
            //Init();
            //_setColCount();
         }
      }

      function _updatePositions() {
         var cc = _colCount;
         _setColCount();
         if (cc > _colCount) {
            var dcol = _colCount - cc;
            var its = [];
            for (var i = _colCount; i < cc; i++) {
               var bl = "col_" + i;
               $("div[id=" + bl + "]").children().each(function(it, item) {
                  its.push(item);
               })
               $("div[id=" + bl + "]").remove();
            }

            for (var j = 0; j < its.length; j++)
            { _injectBlock(its[j]); }
         }
         else if (cc < _colCount) {
            for (var k = 0; k < _colCount - cc; k++) {
               div = document.createElement("div");

               var id = cc + k;
               id = "col_" + id;

               $(div).attr("id", id).addClass("tip-col");
               $("#items > .clearer").before($(div));
            }

            buildBlocks();
         }
      }
      //]]>
   </script>

   <script type="text/javascript">
      //<![CDATA[
      (function(jQuery) {

         // We override the animation for all of these color styles
         jQuery.each(['backgroundColor', 'borderBottomColor', 'borderLeftColor', 'borderRightColor', 'borderTopColor', 'color', 'outlineColor'], function(i, attr) {
            jQuery.fx.step[attr] = function(fx) {
               if (fx.state == 0) {
                  fx.start = getColor(fx.elem, attr);
                  fx.end = getRGB(fx.end);
               }

               fx.elem.style[attr] = "rgb(" + [
				Math.max(Math.min(parseInt((fx.pos * (fx.end[0] - fx.start[0])) + fx.start[0]), 255), 0),
				Math.max(Math.min(parseInt((fx.pos * (fx.end[1] - fx.start[1])) + fx.start[1]), 255), 0),
				Math.max(Math.min(parseInt((fx.pos * (fx.end[2] - fx.start[2])) + fx.start[2]), 255), 0)
			].join(",") + ")";
            }
         });

         // Color Conversion functions from highlightFade
         // By Blair Mitchelmore
         // http://jquery.offput.ca/highlightFade/

         // Parse strings looking for color tuples [255,255,255]
         function getRGB(color) {
            var result;

            // Check if we're already dealing with an array of colors
            if (color && color.constructor == Array && color.length == 3)
               return color;

            // Look for rgb(num,num,num)
            if (result = /rgb\(\s*([0-9]{1,3})\s*,\s*([0-9]{1,3})\s*,\s*([0-9]{1,3})\s*\)/.exec(color))
               return [parseInt(result[1]), parseInt(result[2]), parseInt(result[3])];

            // Look for rgb(num%,num%,num%)
            if (result = /rgb\(\s*([0-9]+(?:\.[0-9]+)?)\%\s*,\s*([0-9]+(?:\.[0-9]+)?)\%\s*,\s*([0-9]+(?:\.[0-9]+)?)\%\s*\)/.exec(color))
               return [parseFloat(result[1]) * 2.55, parseFloat(result[2]) * 2.55, parseFloat(result[3]) * 2.55];

            // Look for #a0b1c2
            if (result = /#([a-fA-F0-9]{2})([a-fA-F0-9]{2})([a-fA-F0-9]{2})/.exec(color))
               return [parseInt(result[1], 16), parseInt(result[2], 16), parseInt(result[3], 16)];

            // Look for #fff
            if (result = /#([a-fA-F0-9])([a-fA-F0-9])([a-fA-F0-9])/.exec(color))
               return [parseInt(result[1] + result[1], 16), parseInt(result[2] + result[2], 16), parseInt(result[3] + result[3], 16)];

            // Otherwise, we're most likely dealing with a named color
            return colors[jQuery.trim(color).toLowerCase()];
         }

         function getColor(elem, attr) {
            var color;

            do {
               color = jQuery.curCSS(elem, attr);

               // Keep going until we find an element that has color, or we hit the body
               if (color != '' && color != 'transparent' || jQuery.nodeName(elem, "body"))
                  break;

               attr = "backgroundColor";
            } while (elem = elem.parentNode);

            return getRGB(color);
         };

         // Some named colors to work with
         // From Interface by Stefan Petre
         // http://interface.eyecon.ro/

         var colors = {
            aqua: [0, 255, 255],
            azure: [240, 255, 255],
            beige: [245, 245, 220],
            black: [0, 0, 0],
            blue: [0, 0, 255],
            brown: [165, 42, 42],
            cyan: [0, 255, 255],
            darkblue: [0, 0, 139],
            darkcyan: [0, 139, 139],
            darkgrey: [169, 169, 169],
            darkgreen: [0, 100, 0],
            darkkhaki: [189, 183, 107],
            darkmagenta: [139, 0, 139],
            darkolivegreen: [85, 107, 47],
            darkorange: [255, 140, 0],
            darkorchid: [153, 50, 204],
            darkred: [139, 0, 0],
            darksalmon: [233, 150, 122],
            darkviolet: [148, 0, 211],
            fuchsia: [255, 0, 255],
            gold: [255, 215, 0],
            green: [0, 128, 0],
            indigo: [75, 0, 130],
            khaki: [240, 230, 140],
            lightblue: [173, 216, 230],
            lightcyan: [224, 255, 255],
            lightgreen: [144, 238, 144],
            lightgrey: [211, 211, 211],
            lightpink: [255, 182, 193],
            lightyellow: [255, 255, 224],
            lime: [0, 255, 0],
            magenta: [255, 0, 255],
            maroon: [128, 0, 0],
            navy: [0, 0, 128],
            olive: [128, 128, 0],
            orange: [255, 165, 0],
            pink: [255, 192, 203],
            purple: [128, 0, 128],
            violet: [128, 0, 128],
            red: [255, 0, 0],
            silver: [192, 192, 192],
            white: [255, 255, 255],
            yellow: [255, 255, 0]
         };

      })(jQuery);

      //]]>
   </script>

</asp:Content>
