<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Site/Views/Shared/singlewrapper.master"
   Inherits="System.Web.Mvc.ViewPage(of HomepageData)" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
   Practical financial advice for today's middle class
</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="middlePlaceHolder" runat="server">
 <div class="popmod-head">
      <center>
         <span><b>Featured</b></span>
      </center>
   </div>
   <div id="topbox" class="clear">
      <div class="column-left left-main">
         <div id="asec">
            <div class="pchbx">
            </div>
            <div id="arts">
               <% Dim currentIndex As Integer = 0%>
               <% For Each article As ArticleAdv In Model.MainArticles%>
               <% Html.RenderPartial("~/Areas/Site/views/shared/articles/frontpageArticleItem.ascx", New FrontPageArticleItem(article, currentIndex))%>
               <% currentIndex += 1%>
               <% Next%>
            </div>
         </div>
      </div>
      <div class="column-right right-main">
         <div id="rightbox">
            <h2 class="hdvt">
               <a href="/articles/topics/stories/" class="asklnk">Featured Story</a>
            </h2>
            <div id="spotsBlock">
               <table width="100%" border="0">
                  <% Dim spIndex As Integer = 0%>
                  <% For Each it As Article In Model.SpotlightArticles%>
                  <% Html.RenderPartial("~/Areas/Site/views/shared/articles/SpotArticleItem.ascx", New spotlightArticleItem(it, spIndex))%>
                  <% spIndex += 1%>
                  <%Next%>
               </table>
            </div>
         </div>
         <% Html.RenderPartial("~/Areas/Site/views/shared/ask/asklists.ascx", Model.questions)%>
      </div>
      <div id="trackholder" style="position: absolute; left: 350px; bottom: 5px;">
         <ul id="tracks">
            <li id="d0" class="ar current"><a href="javascript:void(0);"></a></li>
            <li id="d1" class="ar"><a href="javascript:void(0);"></a></li>
            <li id="d2" class="ar"><a href="javascript:void(0);"></a></li>
            <li id="d3" class="ar"><a href="javascript:void(0);"></a></li>
            <li id="d4" class="ar"><a href="javascript:void(0);"></a></li>
         </ul>
      </div>
   </div>
   <% Html.RenderPartial("~/Areas/Site/views/shared/popular.ascx", Model.PopularNews)%>
</asp:Content>
<asp:Content ID="videoContent" ContentPlaceHolderID="videoHolder" runat="server">
  <%-- <div id="videoItems" class="feat_videos">
      <div class="main-wrapper">
         <h2 class="fbif">
            Featured Videos</h2>
         <div style="overflow: hidden;">
            <% For Each vd As Video In Model.FeaturedVideos%>
            <% Html.RenderPartial("~/views/shared/videofeatureItemctrl.ascx", vd)%>
            <% Next%>
            <div style="float: right; width: 270px; padding: 0 5px; border-left: 1px dotted #000000;
               height: 130px; margin: 10px;">
               <h2 style="font-size: 10px; color: #b8b8b8; margin-left: 10px;">
                  Also...</h2>
            </div>
         </div>
      </div>
   </div>--%>
</asp:Content>
<asp:Content ID="scriptContent" ContentPlaceHolderID="scriptLoader" runat="server">

   <script type="text/javascript">
      //<![CDATA[
      $(document).ready(function() {
         SetItemList();
         hl_timer.startTimer();
      });

      var pn_ar = [];
      var _cnt = 0;
      var _currentIndex = 0;
      var _nextIndex = 1;
      var _maxItems = 5;

      var $item = [];

      function SetItemList() {
         $('#arts').children().each(function(el) {
            $item.push($(this));
         })
      }

      function blendOut() {
         //_out = $("#item_" + _currentIndex);
         if ($item.length > 0) {

            var _out = $item[_currentIndex];
            if (_out) {
               _out.css("z-index", "0");
            }

            //_in = $("#item_" + _nextIndex);
            var _in = $item[_nextIndex];
            if (_in) {
               _in.css("z-index", "2");
            }

            if (_out) {
               _out.animate({ opacity: "0" }, "normal", blendIn);
            }
         }
      }


      function blendIn() {
         //_out = $("#item_" + _currentIndex);
         var _out = $item[_currentIndex];
         //_in = $("#item_" + _nextIndex);
         var _in = $item[_nextIndex];

         if ((_out) && (_in)) {
            _out.css("z-index", "0");
            _out.css("display", "none");
            setTracker();
            if (_in) {
               _in.css("display", "block");
               _in.animate({ opacity: "1" }, "normal", function() { IncrementIndex(); });
            }
         }
      }

      var hl_timer = {
         timerID: null,
         millisecs: 20000,
         timerRunning: false,

         startTimer: function() {
            this.timerID = setInterval("blendOut()", this.millisecs);
            this.timerRunning = true;
         },

         stopTimer: function() {
            if (this.timerRunning)
               clearTimeout(this.timerID)
            this.timerRunning = false;
         },

         resetTimer: function() {
            this.stopTimer();
            this.startTimer();
         }
      }

      var _tracks = null;

      $(function() {
         $("li.ar", '#tracks').click(function(item) {
            var lnks = $("li.ar", '#tracks');
            if ((lnks) && (lnks.length > 0)) {
               lnks.removeClass("current");
               var it = $(this);
               it.addClass("current");
               showItem(it[0].id);
               hl_timer.resetTimer();
            }
         });
      })


      function setTracker() {
         $("#d" + _currentIndex).removeClass("current");
         $("#d" + _nextIndex).addClass("current");
      }


      function Prev() {
         hl_timer.stopTimer();

         //$("#item_" + _currentIndex).css({ "z-index": "0", "opacity": "0", "display": "none" });
         $item[_currentIndex].css({ "z-index": "0", "opacity": "0", "display": "none" });
         $("#d" + _currentIndex).removeClass("current");
         DecrementIndex();
         //$("#item_" + _currentIndex).css({ "z-index": "2", "opacity": "1", "display": "block" });
         $item[_currentIndex].css({ "z-index": "2", "opacity": "1", "display": "block" });
         $("#d" + _currentIndex).addClass("current");

         hl_timer.startTimer();
      }

      function Next() {
         hl_timer.stopTimer();
         setTracker();
         //$("#item_" + _currentIndex).css({ "z-index": "0", "opacity": "0", "display": "none" });
         $item[_currentIndex].css({ "z-index": "0", "opacity": "0", "display": "none" });
         //$("#item_" + _nextIndex).css({ "z-index": "2", "opacity": "1", "display": "block" });
         $item[_nextIndex].css({ "z-index": "2", "opacity": "1", "display": "block" });

         IncrementIndex();
         hl_timer.startTimer();
      }


      function showItem(itemId) {
         $("#arts > div.nit").css({ "z-index": "0", "opacity": "0", "display": "none" });

         switch (itemId) {
            case 'd0':
               _currentIndex = 0;
               break;
            case 'd1':
               _currentIndex = 1;
               break;
            case 'd2':
               _currentIndex = 2;
               break;
            case 'd3':
               _currentIndex = 3;
               break;
            case 'd4':
               _currentIndex = 4;
               break;
         }

         $item[_currentIndex].css({ "z-index": "2", "opacity": "1", "display": "block" });
         setNextIndex();
      }


      function IncrementIndex() {
         if (_currentIndex < _maxItems - 1) {
            _currentIndex += 1;
         }
         else {
            _currentIndex = 0;
         }
         setNextIndex();
      }


      function DecrementIndex() {
         if (_currentIndex > 0) {
            _currentIndex -= 1;
         }
         else {
            _currentIndex = 4;
         }
         setNextIndex();
      }

      function setNextIndex() {
         if (_currentIndex < _maxItems - 1) {
            _nextIndex = _currentIndex + 1;
         }
         else {
            _nextIndex = 0;
         }
      }

      $(function() {
         $("#item_0").css("display", "block");
      });


      //      flowplayer("a.stvdi", '<%= ResolveUrl("~/mcc/videos/flowplayer-3.1.5.swf") %>',
      //      {
      //         plugins: {
      //            controls: null
      //         }
      //      });

      //]]>
   </script>

</asp:Content>
