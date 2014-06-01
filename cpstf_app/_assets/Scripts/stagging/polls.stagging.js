var Polls = function() {

   var pollId = null;
   var pollvotelist = [];
   var Proxy = null;
   var optionName = null;

   var initSettings = function() {
      if (typeof __pollId !== "undefined") {
         pollId = __pollId;
      }

      if (typeof __optionName !== "undefined") {
         optionName = __optionName;
      }

      if (pollId) {
         if (_hasVote(pollId)) {
            $('#res').hide();
            $('#pollOptions').hide();
            $('#pollResults').show();
         }
         else {
            $('#res').show();
            $('#pollOptions').show();
            $('#pollResults').hide();
         }
      }

      $("#lnkResults").toggle(function() {
         $('.pollOptions').fadeOut("fast", function() {
            $('#pollResults').fadeIn("fast");
            $("#lnkTxt").text("Vote");
         });
      }, function() {
         $('#pollResults').fadeOut("fast", function() {
            $('#pollOptions').fadeIn("fast");
            $("#lnkTxt").text("Results");
         });
      });

      $(".votebtn").click(function() {
         var id = $("input[@name='" + optionName + "']:checked").val()

         //        $.ajax({
         //            url: '<%= ResolveUrl("~/ajaxData.ashx") %>',
         //            type: 'GET',
         //            data: 'qId=polls&id=' + id,
         //            success: displayPollResults
         //         });

         var DTO = { 'Id': parseInt(id) };

         Proxy.invoke('VotePoll', DTO, displayPollResults);

      });

   };

   var _hasVote = function(id) {
      var cv = mccUtils.Tools.Get_Cookie("mcc_poll_votelist");
      if (cv) {
         if (pollvotelist.length == 0) {
            var cvlist = unescape(cv)
            pollvotelist = cvlist.split(";");
         }
         for (var i = 0; i < pollvotelist.length; i++) {
            if (pollvotelist[i] == id) {
               return true;
            }
         }
      }
      return false;
   };

   var SaveVote = function(id) {
      //var rs = result.split("|");  // success|id|total_votes
      if (id) {
         pollvotelist.push(id);
         mccUtils.Tools.Set_Cookie("mcc_poll_votelist", pollvotelist.join(";"));
         //            $("#notify_" + rs[1]).html("Thank you for your vote!");
         //            $("#vote_link_" + rs[1]).replaceWith("<span id='" + "vote_link_" + rs[1] + "' style='color:#bababa'>Vote</span>");
         //            $("#votes_" + rs[1]).fadeOut("fast", function() {
         //                $("#notifybox_" + rs[1]).toggle().animate({ "backgroundColor": "#fefefe" }, "slow", function() {
         //                    displayResults(rs[1], rs[2], rs[3], true);
         //                });
         //            });
      }
      else {
         //            $("#notify_" + rs[1]).html('We are unable to process you vote at this time.Please try again later.');
         //            $("#notifybox_" + rs[1]).show()
      }
   };

   var displayPollResults = function(result) {

      var rs = result.split("|");
      if (rs[0] == 'success') {
         $('.votebtn').hide();
         $("#lnkTxt").hide();

         $('#pollOptions').fadeOut("normal", function() {
            $('#pollResults').fadeIn("normal");
         });
         SaveVote(rs[1]);
      }
   }

   return {
      Init: function(proxyUrl) {
         initSettings();
         Proxy = new serviceProxy(proxyUrl);
      }
   }
} ();
