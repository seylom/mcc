
var Questions = function () {
   var Id = 0;
   var Proxy = null;
   var proxyUrl = '';
   var Output = '#output';
   var AuthenticationMessage = '';
   var editor = null;


   var tooltipClickAuth = function (item) {
      item.unbind('click');
      item.attr('title', 'You must be logged in to use this feature');
      item.tipsy();
   };

   var InitAuthMessage = function (loginUrl) {
      AuthenticationMessage = 'Please <a href=' + loginUrl + '?returnurl=' +
                  escape(document.location) + ' class="globalred">Login</a> or <a href=' + loginUrl + '?returnurl=' +
                      escape(document.location) + ' class="globalred">Register</a> to vote';
   };

   var CheckAuth = function (src) {
      DisplayMessage("Message: Login", AuthenticationMessage);
   };

   var Initialize = function () {
      if ((typeof isAuthenticated !== "undefined") && isAuthenticated) {
         AllowVotes();
      }
      else {
         // rewrite click events vor votes on the page
         DenyVotes();
      }

      $('#postAnswerLnk').unbind('click').click(function () { $.scrollTo('#postyouranswer', 500); });
   };

   var AllowVotes = function () {
      //Id = parseInt($(itemId).val());
      Proxy = new serviceProxy(proxyUrl);

      $('#q_up').unbind("click").click(function () {
         qUp();
      });
      $('#q_down').unbind("click").click(function () {
         qDown();
      });

      $("img[id^='answer_up_']").unbind("click").click(function () {
         var id = $(this).attr('id').substring('answer_up_'.length);
         aUp(id);
      });
      $("img[id^='answer_down_']").unbind("click").click(function () {
         var id = $(this).attr('id').substr('answer_down_'.length);
         aDown(id);
      });

      $('.acc').unbind("click").click(function () {
         var id = $(this).attr('id').substring('acc_'.length);
         AcceptReject(id);
      });

      $('#qrplnk').unbind('click').click(function () {
         $("#qcommentbx").slideToggle(150);
      });

      $("form[name^='comment-form-']").bind("submit", function () {
         var id = $(this).attr('name').substr('comment-form-'.length);
         var p = $('#comment', $(this));
         if (p) {
            var r = p.val().trim();
            if (r) {
               CommentA(id, r);
               p.val("");
               $("#commentbx_" + id).fadeOut('slow');
            }
         }
         return false;
      });

      $("form[name='comment-form']").bind("submit", function () {
         if ($('#comment').val().trim() != "") {
            CommentQ($('#comment').val());
            $('#comment').val("");
            $("#qcommentbx").fadeOut('slow');
         }
         return false;
      });

      //      $('#qcmbtn').unbind('click').click(function() {


      //      });

      $("a[id^='arplnk_']").unbind('click').click(function () {
         var id = $(this).attr("id").substr('arplnk_'.length);
         $("#commentbx_" + id, "#answer_" + id).slideToggle(200);
      });

      $(".flw").unbind('click').click(function () {
         Follow();
      });

      //      $("input[id^='cmbtn_']").unbind('click').click(function() {
      //         var id = $(this).attr("id").substr('cmbtn_'.length);

      //         if ($('#commata_' + id).val().trim() != "") {
      //            CommentA(id, $('#commata_' + id).val());
      //            $('#commata_' + id).val("");
      //            $("#commentbx_" + id, "#answer_" + id).slideToggle(150); ;
      //         }
      //      });

      //               $('.replylnk').click(function() {
      //                  $('.cbx').hide();
      //                  var id = $(this).attr("id").substring('replylnk_'.length);
      //                  if (id) {
      //                     var itemId = "commentbx_" + id;
      //                     $("#" + itemId).show();
      //                  }
      //               });




   };

   var DenyVotes = function () {
      $("img[id^='answer_up_']").unbind("click").click(function () { CheckAuth(); });
      $("img[id^='answer_down_']").unbind("click").click(function () { CheckAuth(); });

      $("#q_up").unbind("click").click(function () { CheckAuth(); });
      $("#q_down").unbind("click").click(function () { CheckAuth(); });
      $('.acc').unbind("click").click(function () { CheckAuth(); });
      $('#btnPostAnswer').unbind('click').click(function () { CheckAuth(); });
      $("a[id^='arplnk_']").unbind('click').click(function () { CheckAuth(); });
      $("#qrplnk").unbind('click').click(function () { CheckAuth(); });
      $("a[id^='aflag_']").unbind('click').click(function () { CheckAuth(); });
      $("#qflag").unbind('click').click(function () { CheckAuth(); });
      $('#postAnswerLnk').unbind('click').click(function () { CheckAuth(); });

      tooltipClickAuth($(".qlnkauth"), 'You must be logged in to use this feature');

   };

   var qUp = function () {
      if (Id > 0) {
         var DTO = {};
         Proxy.invoke(Id + '/VoteQuestionUp', DTO, UpdateQuestion, function (err) { alert(err.message); }, true);
      }
   };

   var qDown = function () {
      if (Id > 0) {
         var DTO = {};
         Proxy.invoke(Id + '/VoteQuestionDown', DTO, UpdateQuestion, function (err) { alert(err.message); }, true);
      }
   };

   var aUp = function (id) {
      var iNum = parseInt(id);
      if (iNum > 0) {
         var DTO = {};
         Proxy.invoke(iNum + '/VoteAnswerUp', DTO, UpdateAnswer, function (err) { alert(err.message); }, true);
      }
   };

   var aDown = function (id) {
      var iNum = parseInt(id);
      if (iNum > 0) {
         var DTO = {};
         Proxy.invoke(iNum + '/VoteAnswerDown', DTO, UpdateAnswer, function (err) { alert(err.message); }, true);
      }
   };

   var AcceptReject = function (id) {
      var iNum = parseInt(id);
      if (iNum > 0) {
         var DTO = { 'questionId': Id }; //{ 'questionId': Id, 'answerId': iNum };
         Proxy.invoke(iNum + '/AcceptRejectAnswer', DTO, votefb, function (err) { alert(err.message); }, true);
      }
   };

   var PostAnswer = function (body) {
      if (Id > 0) {
         var DTO = { 'Id': Id, 'body': body };
         Proxy.invoke(Id + '/PostAnswer', DTO, RefreshAnswers, function (err) { alert(err.message); }, true);
      }
   };

   var UpdateQuestion = function (res) {
      if (res.Success == true) {
         $('#' + Output).html(res.Data);
      }
      else {
         DisplayMessage('Question vote', res.Message);
      }
   };

   var UpdateAnswer = function (res) {
      if (res.Success == true) {
         $('#votes_' + res.Id).html(res.Data);
      }
      else {
         DisplayMessage('Answer vote', res.Message);
      }
   };
   var RefreshAnswers = function (res) {
      if ((res) && (res.length == 1)) {
         if (res[0] == 'True') {
            $('#txtAnswer').val('');
         }
      }
   };

   var votefb = function (res) {
      if (res) {
         var id = res.Id;
         if (res.Success == true) {
            $('.answer').removeClass("rep").addClass("op");
            $('#answer_' + res.Id).removeClass("op").addClass("rep");

            $('.accbx').show();
            $('.accbx', '#answer_' + res.Id).hide();

            // move up if it is not the first one!
            var obj = $('#answer_' + res.Id);
            obj.parent().prepend(obj);

            $.scrollTo('#answers', 500);
         }
         else {
            $('.answer').removeClass("rep").addClass("op");
         }
      }
      else {

      }
   };

   var Update = function (res) {
      // update everything : questions, answers, comments ...
   };

   var CommentQ = function (body) {
      var DTO = { 'comment': body };
      Proxy.invoke(Id + '/CommentQuestion', DTO, cmQfeed, function (err) { alert(err.message); });

   };

   var CommentA = function (id, body) {
      var iNum = parseInt(id);
      if (iNum > 0) {
         var DTO = { 'Id': iNum, 'comment': body };
         Proxy.invoke(iNum + '/CommentAnswer', DTO, cmAfeed, function (err) { alert(err.message); });
      }
   };

   var cmQfeed = function (res) {
      if (res.Success) {
         var qm = commentItem(res);
         $('#qcomment-lst').append(qm);
      }
      else {

      }
   };

   var cmAfeed = function (res) {
      if (res.Success) {
         var qm = commentItem(res);
         $('#acomment-lst-' + res.Id).append(qm);
      }
      else {

      }
   };


   var commentItem = function (res) {
      return $('<div style="font-size: 10px; margin: 3px 0px; border-top: 1px dotted #b8cae8;">' +
                  '<table style="width: 100%">' +
                     '<tr>' +
                        '<td>' + res.Body + '</td><td style="width: 50px;">' +
                           '<a  class="global" href="/users/' + res.AddedBy + '" >' +
                                 res.AddedBy +
                           '</a>' +
                        '</td>' +
                     '</tr>' +
                  '</table>' +
               '</div>');
   };

   var AddComment = function (body) {


   };

   var Follow = function () {
      //toggle following status for current user
      if (Id > 0) {
         var DTO = { 'Id': Id };
         Proxy.invoke(Id + '/Follow', DTO, function (result) {
         var blk = "#qu_" + Id;
            if (result) {
               $(".flw", blk).removeClass("f_off");
               $(".flw", blk).addClass("f_on");
               $(".flw", blk).html("following");
            }
            else {
               $(".flw", blk).removeClass("f_on");
               $(".flw", blk).addClass("f_off");
               $(".flw", blk).html("follow");
            }
         }
         , function (err) { alert(err.message); }, true);
      }
   };

   return {
      Init: function (id, loginUrl, url, votebxId) {
         Id = id;
         proxyUrl = url;
         Output = votebxId;
         InitAuthMessage(loginUrl);
         Initialize();
      }
   }
} ();




//   var updater = function() {
//      var qId;
//      var Proxy;
//      var timer = 40000; //every 40 secs
//      return {
//         start: function() {
//            updater.answers.i
//            setTimeout(updater.answers.Query, timer);
//         },
//         answers: function() {
//            return {
//               Init: function() {
//                  qId = $('<%= _questionId.ClientID %>').val();
//                  Proxy = new serviceProxy('<%=ResolveUrl("~/asv.asmx/") %>');
//               },
//               Query: function() {
//                  if (qId) {
//                     var DTO = { 'id': qId };
//                     Proxy.invoke('AnswersByQuestionId', DTO, updater.answers.Update);
//                  }
//               },
//               Update: function(r) {
//                  if (r) {

//                  }
//               }
//            }
//         } ()
//      }
//   } ();
      