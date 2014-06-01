var Tips = function() {
   var it_ar = [];
   var _colCount = -1;
   var _isRezised = false;
   var _isLast = false;
   var isInitialized;
   var _indexBuilt = 0;
   var $tpitems = $("#tp-items");
   var $items = $("#items");
   var votelist = [];
   var votevalues = [];
   var Proxy = null;

   function Setup() {

      $tpitems.children().each(function(it, item) {
         var _advId = item.id.substr("item_".length, item.id.length);
         SetVotesInfo(_advId);
         it_ar.push(item);
      });
      $tpitems.empty();

      if (it_ar.length > 0) {
         if (_colCount == -1) {
            _setColCount();
         }
         if (_colCount > 0) {
            for (var i = 0; i < _colCount; i++) {
               div = document.createElement("div");
               var id = "col_" + i;
               $(div).attr("id", id).addClass("tip-col");
               $items.append(div);
            }
         }
         buildAltBlocks();

      }

      window.onresize = _fixLayout;
   };



   var buildAltBlocks = function() {
      if ((it_ar.length > 0) && (_indexBuilt < it_ar.length)) {
         var si = _smalestCol();
         var item = it_ar[_indexBuilt];
         if (item) {
            _indexBuilt += 1;

            $(item).addClass("tip").hover(function(){$(this).css("background-color","#fffde8");},function(){ $(this).css("background-color","#ffffff"); }).appendTo($(si)).animate({ "backgroundColor": "#ffffff", "opacity": "1" }, "fast", buildAltBlocks);
         }

         if (_indexBuilt == it_ar.length - 1) {
            _isLast = true;
            InitVotesValues();
            InitVoteLinks();
         }
      }
   };

   var buildBlocks = function() {
      if (it_ar.length > 0) {
         for (var j = 0; j < it_ar.length; j++) {
            var it = it_ar[j];
            _injectBlock(it);
         }
         isInitialized = true;
      }
   };

   var _injectBlock = function(id) {
      var si = _smalestCol();
      if (si) {
         $(id).addClass("tip").hover(function() {
            $(this).css("background-color","#fffde8");}, function() { $(this).css("background-color","#ffffff"); }).appendTo($(si)).animate({ "opacity": "1" }, "slow");
      }
   };

   var _smalestCol = function() {
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

   var _tallestCol = function() {
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
   };

   var _setColCount = function() {
      var w = $items.width();
      _colCount = Math.floor(w / 220);
      return _colCount;
   };

   var _fixLayout = function(event) {
      if (isInitialized) {
         //$("#items").empty();
         _updatePositions();
         //Init();
         //_setColCount();
      }
   };

   var _updatePositions = function() {
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
   };



   var InitVoteLinks = function() {
      $(function() {
         $("a[id^='voteuplnk_']").unbind('click').click(function() {
            var id = $(this).attr('id').substr('voteuplnk_'.length);
            Vote(id, 1);
         });

         $("a[id^='votedownlnk_']").unbind('click').click(function() {
            var id = $(this).attr('id').substr('votedownlnk_'.length);
            Vote(id, 0);
         });
      });
   };

   var _isRated = function(id) {
      var cv = mccUtils.Tools.Get_Cookie("mcc_adv_votelist");
      if (cv) {
         if (votelist.length == 0) {
            var cvlist = unescape(cv);
            votelist = cvlist.split(";");
         }
         for (var i = 0; i < votelist.length; i++) {
            if (votelist[i] == id) {
               return true;
            }
         }
      }
      return false;
   };

   var SetVotesInfo = function(id) {
      var cv = mccUtils.Tools.Get_Cookie("mcc_adv_votelist");
      if (cv) {
         if (votelist.length == 0) {
            var cvlist = unescape(cv);
            votelist = cvlist.split(";");
         }
         for (var i = 0; i < votelist.length; i++) {
            if (votelist[i] == id) {
               $("#vote_link_" + id).replaceWith("<span id='" + "vote_link_" + id + "' style='color:#bababa'>Vote</span>");
               $("#votes_" + id).hide();
               $("#result_link_" + id).replaceWith("<span id='" + "result_link_" + id + "' style='color:#bababa'>Results</span>");
               $("#result_" + id).show();
               return;
            }
         }
         $("#vote_link_" + id).replaceWith("<span id='" + "vote_link_" + id + "' style='color:#bababa'>Vote</span>");
      }
   };

   var Vote = function(id, val) {
      var cv = mccUtils.Tools.Get_Cookie("mcc_adv_votelist");
      if (cv) {
         if (votelist.length == 0) {
            var cvlist = unescape(cv)
            votelist = cvlist.split(";");
         }
         for (var i = 0; i < votelist.length; i++) {
            if (votelist[i] == id) {
               $("#notify_" + id).html('vote already submitted!');
               $("#notifybox_" + id).show();
               return;
            }
         }
      }

      var DTO = { 'value': val };
      Proxy.invoke(id + '/vote', DTO, SaveVote, function() {
         alert('Unable to save your vote at this time.')
      });
   };


   var SaveVote = function(result) {
      var rs = result;
      //var rs = result.split("|");  // success|id|yes_votes|no_votes
      if (rs.success == true) {
         votelist.push(rs.id);
         mccUtils.Tools.Set_Cookie("mcc_adv_votelist", votelist.join(";"));
         $("#notify_" + rs.id).html("Thank you for your vote!");
         $("#vote_link_" + rs.id).replaceWith("<span id='" + "vote_link_" + rs.id + "' style='color:#bababa'>Vote</span>");
         $("#votes_" + rs.id).fadeOut("fast", function() {
            $("#notifybox_" + rs.id).toggle().animate({ "backgroundColor": "#fefefe" }, "slow", function() {
               displayResults(rs.id, rs.up, rs.down, true);
            });
         });
      }
      else {
         $("#notify_" + rs.id).html('We are unable to process you vote at this time.Please try again later.');
         $("#notifybox_" + rs.id).show()
      }
   };

   var showVoteLinks = function(id) {
      var _idIn = "#votes_" + id;
      var _idOut = "#result_" + id;

      $("#vote_link_" + id).replaceWith("<span id='" + "vote_link_" + id + "' style='color:#bababa'>Vote</span>");

      if (!_isRated(id)) {
         $("#result_link_" + id).replaceWith("<a id='" + "result_link_" + id + "' href='javascript:void(0);' class='global' onclick='showResults(" + id + ")'>Results</a>");
      }

      $(_idOut).fadeOut("fast", function() {
         $(_idIn).fadeIn("fast");
      });
   };

   var showResults = function(id, effect) {
      var _idIn = "#result_" + id;
      var _idOut = "#votes_" + id;

      if (!_isRated(id)) {
         $("#vote_link_" + id).replaceWith("<a id='" + "vote_link_" + id + "' href='javascript:void(0);' class='global' onclick='showVoteLinks(" + id + ")'>Vote</a>");
      }
      $("#result_link_" + id).replaceWith("<span id='" + "result_link_" + id + "' style='color:#bababa'>Results</span>");

      $(_idOut).fadeOut("fast", function() {
         $(_idIn).fadeIn("fast", getVoteValues(id, false));
      });
   };

   var displayResults = function(id, voteup, votedown, effect) {
      var _idIn = "#result_" + id;
      var _idOut = "#votes_" + id;

      if (!_isRated(id)) {
         $("#vote_link_" + id).replaceWith("<a id='" + "vote_link_" + id + "' href='javascript:void(0);' class='global' onclick='showVoteLinks(" + id + ")'>Vote</a>");
      }
      $("#result_link_" + id).replaceWith("<span id='" + "result_link_" + id + "' style='color:#bababa'>Results</span>");

      $(_idOut).fadeOut("fast", function() {
         $(_idIn).fadeIn("fast", setVoteValues(id, voteup, votedown, effect));
      });
   };

   var getVoteValues = function(id, effect) {
      var _isCached = false;
      for (var i = 0; i < votevalues.length; i++) {
         if (votevalues[i][0] == id) {
            setVoteValues(id, votevalues[i][1], votevalues[i][2], effect);
            return;
         }
      }

      var vdurl = '/tips/' + id + '/getvotevalues'

      $.get(vdurl, function(result) {
         //var _votes = [];
         //_votes = result.split("|");

         var vup = result.up;
         var vdn = result.down;

         var vupres = (vup * 100) / (vup + vdn)
         var vdnres = (vdn * 100) / (vup + vdn)

         var votes = [id, vupres, vdnres];
         votevalues.push(votes);

         var _yid = "#votebar_yes_" + id;
         var _nid = "#votebar_no_" + id;

         if (effect == true) {
            $(_yid).css("width", "0px");
            $(_nid).css("width", "0px");

            $(_yid).animate({ "width": vupres + "%" }, "slow");
            $(_nid).animate({ "width": vdnres + "%" }, "slow");
         }
         else {
            $(_yid).css("width", vupres + "%");
            $(_nid).css("width", vdnres + "%");
         }
      });
   };

   var setVoteValues = function(id, voteup, votedown, effect) {

      var totalvotes = voteup + votedown;
      var vupres = (totalvotes > 0) ? (voteup * 100) / totalvotes : 0;
      var vdnres = (totalvotes > 0) ? (votedown * 100) / totalvotes : 0;


      //var votes = [id, vupres, vdnres];
      //votevalues.push(votes);

      var _yid = "#votebar_yes_" + id;
      var _nid = "#votebar_no_" + id;

      var yes_text = "(" + Math.round(vupres) + "%, " + voteup + " votes)";
      var no_text = "(" + Math.round(vdnres) + "%, " + votedown + " votes)";

      $("#votetext_yes_" + id).empty().html(yes_text);
      $("#votetext_no_" + id).html(no_text);

      if (effect == true) {
         $(_yid).css("width", "0px");
         $(_nid).css("width", "0px");

         $(_yid).animate({ "width": vupres + "%" }, "slow");
         $(_nid).animate({ "width": vdnres + "%" }, "slow");
      }
      else {
         $(_yid).css("width", vupres + "%");
         $(_nid).css("width", vdnres + "%");
      }
   };

   var InitVotesValues = function() {
      var cv = mccUtils.Tools.Get_Cookie("mcc_adv_votelist");
      if (cv) {
         if (votelist.length == 0) {
            var cvlist = unescape(cv)
            votelist = cvlist.split(";");
         }
         for (var i = 0; i < votelist.length; i++) {
            getVoteValues(votelist[i], true);
         }
      }
   };

   return {
      Init: function(url) {
         Setup();
         Proxy = new serviceProxy(url);
      }
   }
} ();