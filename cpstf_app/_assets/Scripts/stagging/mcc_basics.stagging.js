

var mccUtils = function() {

   var ajax_url = '';

   var GetQuotes = function() {
      $.ajax({
         url: mccUtils.ajax_url,
         type: 'GET',
         data: 'qId=qot',
         dataType: 'json',
         success: mccUtils.QuoteList.Initialize
      });
   };

   var GetNextQuote = function() {
      if (mccUtils.QuoteList.quotes) {
         $('#mquo').animate({ opacity: 0 }, 400, "linear", mccUtils.NextQuote);
      }
   };
   var NextQuote = function() {
      QuoteList.injectQuote();
      if (QuoteList.currentId < mccUtils.QuoteList.length) {
         QuoteList.increment();
      }
      $('#mquo').animate({ 'opacity': 1 }, 400);
   };

   var QuoteList = {
      currentId: 0,
      length: 0,
      quotes: null,

      Initialize: function(mQuotes) {
         if (mQuotes) {
            QuoteList.length = mQuotes.quotes.length;
            QuoteList.quotes = mQuotes.quotes;
            this.currentId = 0;

            GetNextQuote();
            quoteTimer.InitializeTimer();
         }
         else
         { this.length = 0; }
      },

      increment: function() {
         this.currentId += 1;
      },
      decrement: function() {
         this.currentId = (this.currentId <= 0) ? 0 : this.currentId - 1;
      },
      injectQuote: function() {
         if (this.currentId >= this.length)
         { this.currentId = 0; }

         var bd = QuoteList.quotes[this.currentId];
         if (bd) {
            QuoteList.DisplayQuote(bd);
         }
      },
      DisplayQuote: function(rs) {
         if (rs) {
            $("#quote-body").html(rs.body);
            if (rs.role != "") {
               $("#small-quote").html("- " + rs.author + ", <i>" + rs.role + "</i>");
            } else { $("#small-quote").html("- " + rs.author); }
            $('#mquo').animate({ 'opacity': 1 }, 400);
         }
      }
   };

   var quoteTimer = {
      secs: 0,
      timerID: null,
      timerRunning: false,
      delay: 500,

      InitializeTimer: function() {
         this.secs = 10;
         this.StopTimer();
         this.StartTimer();
      },

      StopTimer: function() {
         if (this.timerRunning)
            clearTimeout(this.timerID);
         this.timerRunning = false;
      },

      StartTimer: function() {
         this.timerID = setInterval("mccUtils.Home.GetNextQuote()", 20000);
      }
   };

   return {
      init: function(url) {
         ajax_url = url;
         //mccUtils.Home.InitWidgets();
         //mccUtils.Home.QueryQuotes();
         //mccUtils.Home.quoteTimer.InitializeTimer();
      }
   }
} ();


function FetchQuote() {
   $('#mquo').animate({ opacity: 0 }, 400, "linear", FetchNextQuote);
}

function FetchNextQuote() {
   $.ajax({
      url: mccUtils.Home.ajax_url,
      type: 'GET',
      data: 'qId=qid',
      dataType: 'json',
      success: DisplayQuote
   });
}

function DisplayQuote(rs) {
   if (rs) {
      $("#quote-body").html(rs.body);
      if (rs.role != "") {
         $("#small-quote").html("- " + rs.author + ", <i>" + rs.role + "</i>");
      } else { $("#small-quote").html("- " + rs.author); }
      $('#mquo').animate({ 'opacity': 1 }, 400);
   }
}


mccUtils.Tools = function() {

   return {

      // this fixes an issue with the old method, ambiguous values 
      // with this test document.cookie.indexOf( name + "=" );

      // To use, simple do: Get_Cookie('cookie_name'); 
      // replace cookie_name with the real cookie name, '' are required
      Get_Cookie: function(check_name) {
         // first we'll split this cookie up into name/value pairs
         // note: document.cookie only returns name=value, not the other components
         var a_all_cookies = document.cookie.split(';');
         var a_temp_cookie = '';
         var cookie_name = '';
         var cookie_value = '';
         var b_cookie_found = false; // set boolean t/f default f

         for (i = 0; i < a_all_cookies.length; i++) {
            // now we'll split apart each name=value pair
            a_temp_cookie = a_all_cookies[i].split('=');


            // and trim left/right whitespace while we're at it
            cookie_name = a_temp_cookie[0].replace(/^\s+|\s+$/g, '');

            // if the extracted name matches passed check_name
            if (cookie_name == check_name) {
               b_cookie_found = true;
               // we need to handle case where cookie has no value but exists (no = sign, that is):
               if (a_temp_cookie.length > 1) {
                  cookie_value = unescape(a_temp_cookie[1].replace(/^\s+|\s+$/g, ''));
               }
               // note that in cases where cookie is initialized but no value, null is returned
               return cookie_value;
               break;
            }
            a_temp_cookie = null;
            cookie_name = '';
         }
         if (!b_cookie_found) {
            return null;
         }
      },


      /*
      only the first 2 parameters are required, the cookie name, the cookie
      value. Cookie time is in milliseconds, so the below expires will make the 
      number you pass in the Set_Cookie function call the number of days the cookie
      lasts, if you want it to be hours or minutes, just get rid of 24 and 60.

               Generally you don't need to worry about domain, path or secure for most applications
      so unless you need that, leave those parameters blank in the function call.
      */

      Set_Cookie: function(name, value, expires, path, domain, secure) {
         // set time, it's in milliseconds
         var today = new Date();
         today.setTime(today.getTime());
         // if the expires variable is set, make the correct expires time, the
         // current script below will set it for x number of days, to make it
         // for hours, delete * 24, for minutes, delete * 60 * 24
         if (expires) {
            expires = expires * 1000 * 60 * 60 * 24;
         }
         //alert( 'today ' + today.toGMTString() );// this is for testing purpose only
         var expires_date = new Date(today.getTime() + (expires));
         //alert('expires ' + expires_date.toGMTString());// this is for testing purposes only

         document.cookie = name + "=" + escape(value) +
       		((expires) ? ";expires=" + expires_date.toGMTString() : "") + //expires.toGMTString()
       		((path) ? ";path=" + path : "") +
       		((domain) ? ";domain=" + domain : "") +
       		((secure) ? ";secure" : "");
      },

      // this deletes the cookie when called
      Delete_Cookie: function(name, path, domain) {
         if (Get_Cookie(name)) document.cookie = name + "=" +
              		((path) ? ";path=" + path : "") +
              		((domain) ? ";domain=" + domain : "") +
              		";expires=Thu, 01-Jan-1970 00:00:01 GMT";
      }
   };
} ();


var MccInit = function() {
   var url = '';
   var Init = function() {

      $(function highlight_menu_links() {
         $("ul#nmenu a").each(function() {
            var current_href = this.href;
            if (document.location.href.toLowerCase() == current_href.toLowerCase()) {
               $(this).addClass("active");
            }
         });
      });

      $('#q').focus(function() {
         if ($(this).val() == "Search...") {
            $(this).val('').css({ 'color': '#000000' });
         }
      }).blur(function() {
         if ($(this).val() == "") {
            $(this).val('Search...').css({ 'color': '#999999' });
         }
      });

      $('#q').bind('keypress', function(e) {
         var code = (e.keyCode ? e.keyCode : e.which);
         if (code == 13) { //Enter keycode
            var qval = $('#q').val();
            document.location.href = url + 'search?q=' + escape(qval);
         }
      });

      (function($) {
         jQuery.fn.center = function() {
            this.css("position", "absolute");
            this.css("top", ($(window).height() - this.height()) / 2 + $(window).scrollTop() + "px");
            this.css("left", ($(window).width() - this.width()) / 2 + $(window).scrollLeft() + "px");
            return this;
         }

      })(jQuery);

      $('#lnkWhoAreWe').click(function() {
         $('#about_details').slideToggle('fast');
      });

      $(function() {
         var ift = mccUtils.Tools.Get_Cookie('mcc_ift');
         if (!ift) {
            mccUtils.Tools.Set_Cookie('mcc_ift', 1, 30);
            $('#about_details').show();
         }
      });
   };

   return {
      InitDef: function(baseUrl) {
         url = baseUrl;
         Init();
      }
   }
} ();

