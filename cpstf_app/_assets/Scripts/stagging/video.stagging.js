var Videos = function() {
   var _videourlprefix = '';
   var _currentVidId = 0;
   var Proxy = null;
   var ps = {
      playerPath: '/_assets/flowplayer-3.2.4.swf'
   };


   var settings = {
      videosList: [],
      videoQueue: [],
      currentPlaylist: []
   };

   var Setup = function(videopath, videoId) {
      _videourlprefix = videopath;
      _currentVidId = typeof videoId == "undefined" ? 0 : parseInt(videoId);


      $("a[id^='videoitem_']").unbind('click').click(function() {
         var id = $(this).attr('id').substring('videoitem_'.length);
         PlayVideo(id);
      });
      $("a[id^='item_']").unbind('click').click(function() {
         var id = $(this).attr('id').substring('item_'.length);
         PlayVideo(id);
      });


      flowplayer("preview", ps.playerPath);
      PlayDefault();


   };

   var PlayDefault = function() {
      var id = _currentVidId;
      if (id > 0) {

         PlayVideo(id);
      }
   };

   var LoadInfo = function(v) {
      if (v) {
         $('#stitle').html(v.Title);
         $('#sabstr').html(v.Description);
      }
   };

   var ShowVideo = function(v) {
      mvurl = _videourlprefix + v.VideoUrl;

      LoadInfo(v);
      if (mvurl) {
         $(".vplay").hide();
         $("#vo_" + v.VideoID).show();

         flowplayer("preview", ps.playerPath, {
            clip: {
               autoPlay: true,       // aplies to all Clips in the playlist
               url: mvurl
            }
         });
      }
   };

   var PlayVideo = function(id) {
      var sv;

      if (!sv) {

         var DTO = {};
         Proxy.invoke(id + '/GetVideoByID', DTO, SendToPlayback, function(err) { alert(err.message); });
      }
      else {
         ShowVideo(sv, id);
      }
   };

   var SendToPlayback = function(res) {
      settings.videosList.push(res);
      ShowVideo(res);
   };

   var addVideoToQueue = function(id, file) {
      for (var i = 0; i < settings.videoQueue.length; i++) {
         if (videoQueue[i].id == id)
            return;
      }

      var vdurl = '/ajaxData.ashx' + '?qId=video&id=' + id;

      $.ajax({
         url: mccUtils.Home.ajax_url,
         type: 'GET',
         data: 'qId=video&id=' + id,
         dataType: 'json',
         success: Append
      });

   };

   var RemoveVideoFromQueue = function(id) {
      for (var i = 0; i < settings.videoQueue.length; i++) {
         if (settings.videoQueue[i].id == id) {
            settings.videoQueue.splice(i, 1);
            settings.currentPlaylist.splice(i, 1);

            mccUtils.Tools.Set_Cookie('mcc_video_playlist', settings.currentPlaylist.join(";"));

            return;
         }
      }
   };

   var Next = function() {
      document.getElementById("queue").style.left = '0px';
   };
   var Previous = function() {
      document.getElementById("queue").style.left = '-504px';
   };

   var AddSelection = function(id) {
      var vdurl = "ajaxData.ashx?qId=video&id=" + id;
      $.get(vdurl, Append);
   };

   var Append = function(mq) {
      if (mq) {
         var li = document.createElement("li");

         if (!settings.videoQueue) {
            settings.videoQueue = new Array();
         }

         var vdImage = '/imageThumb.ashx' + '?uuid=' + mq.id + '&w=130&h=100';
         var vd = {
            id: mq.id,
            author: mq.author,
            description: mq.description,
            duration: mq.duration,
            file: mq.file,
            link: mq.link,
            image: vdImage,
            start: mq.start,
            title: mq.title,
            type: mq.type
         };


         var lnk = '<div style="position:relative;"><a href="javascript:void(0);" vfile="' + mq.file + '" vid="' + mq.id + '" onclick="Play(this);return false;"><img border="0" src="' + vdImage + '" /></a>';
         lnk += '<div style="position:absolute;bottom:0;right:0;padding:2px;background-color:#eaeaea;"><a  vid="' + mq.id + '"  href="javascript:void(0);" onclick="Remove(this)">remove</a></div></div>';
         li.setAttribute('vid', mq.id);
         li.innerHTML = lnk;

         document.getElementById("queue").appendChild(li);

         settings.videoQueue.push(vd);
         settings.currentPlaylist.push(vd.id);
         mccUtils.Tools.Set_Cookie('mcc_video_playlist', settings.currentPlaylist.join(";"));

      }
   };


   var Play = function(elt) {
      var file = $(elt).attr('vfile');
      if (file) {
         ShowVideo(file);
      }
   };

   var Remove = function(elt) {
      var file = $(elt).attr('vid');
      if (file) {
         RemoveVideoFromQueue(file);
         var q = '#queue > li[vid=' + file + ']';
         var cd = $(q)[0];
         cd.parentNode.removeChild(cd);
      }
   };

   return {
      Init: function(url, videopath, videoid) {
         Proxy = new serviceProxy(url);
         Setup(videopath, videoid);
      }
   }
} ();