
// MCC Utils

// requires jquery!

(function($) {
   jQuery.fn.center = function() {
      this.css("position", "absolute");
      this.css("top", ($(window).height() - this.height()) / 2 + $(window).scrollTop() + "px");
      this.css("left", ($(window).width() - this.width()) / 2 + $(window).scrollLeft() + "px");
      return this;
   }

})(jQuery);

function DisplayMessage(title, body) {

   alert(body);

//   if (title) {
//      $("#messageTitle").html(title);
//   }

//   if (body) {
//      $("#messageBody").html(body);
//   }


//   $('#jqmMessage').center();
//   $('#jqmMessage').jqm({ overlay: 20 });
//   $('#jqmMessage').jqmShow();
}


function serviceProxy(serviceUrl) {
   var _I = this;
   this.serviceUrl = serviceUrl;

//   // *** Call a wrapped object
//   this.invoke = function(method, data, callback, error, bare) {
//      // *** Convert input data into JSON - REQUIRES Json2.js
//      var json = JSON.stringify(data);
//      // *** The service endpoint URL        
//      var url = _I.serviceUrl + method;
//      $.ajax({
//         url: url,
//         data: json,
//         type: "POST",
//         processData: false,
//         contentType: "application/json; charset=utf-8",
//         timeout: 10000,
//         dataType: "text",  // not "json" we'll parse
//         success:
//                    function(res) {
//                       if (!callback) return;
//                       // *** Use json library so we can fix up MS AJAX dates
//                       var result = JSON.parse(res);
//                       // *** Bare message IS result
//                       if (bare)
//                       { callback(result); return; }
//                       // *** Wrapped message contains top level object node
//                       // *** strip it off
//                       for (var property in result) {
//                          callback(result[property]);
//                          break;
//                       }
//                    },
//         error: function(xhr) {
//            if (!error)
//               return;

//            if (xhr.responseText) {
//               var err = JSON.parse(xhr.responseText);
//               if (err)
//                  error(err);
//               else
//                  error({ Message: "Unknown server error." });
//            }
//            return;
//         }
//      });
//   };


   this.invoke = function(method, data, callback, error, verb) {
      // *** The service endpoint URL
      if (typeof verb == undefined) {
         verb = "POST";
      }

      var url = _I.serviceUrl + method;
      $.ajax({
         url: url,
         data: data,
         type: verb,
         //contentType: "application/json; charset=utf-8",
         timeout: 10000,
         dataType: "json",  // not "json" we'll parse
         success: callback,
         error: function(xhr) {
            if (!error)
               return;

            if (xhr.responseText) {
               var err = JSON.parse(xhr.responseText);
               if (err)
                  error(err);
               else
                  error({ Message: "Unknown server error." });
            }
            return;
         }

      });
   }
}

var _tmplCache = {};
this.parseTemplate = function(str, data) {
   /// <summary>
   /// Client side template parser that uses &lt;#= #&gt; and &lt;# code #&gt; expressions.
   /// and # # code blocks for template expansion.
   /// NOTE: chokes on single quotes in the document in some situations
   ///       use &amp;rsquo; for literals in text and avoid any single quote
   ///       attribute delimiters.
   /// </summary>    
   /// <param name="str" type="string">The text of the template to expand</param>    
   /// <param name="data" type="var">
   /// Any data that is to be merged. Pass an object and
   /// that object's properties are visible as variables.
   /// </param>    
   /// <returns type="string" />  
   var err = "";
   try {
      var func = _tmplCache[str];
      if (!func) {
         var strFunc =
            "var p=[],print=function(){p.push.apply(p,arguments);};" +
                        "with(obj){p.push('" +
         //                        str
         //                  .replace(/[\r\t\n]/g, " ")
         //                  .split("<#").join("\t")
         //                  .replace(/((^|#>)[^\t]*)'/g, "$1\r")
         //                  .replace(/\t=(.*?)#>/g, "',$1,'")
         //                  .split("\t").join("');")
         //                  .split("#>").join("p.push('")
         //                  .split("\r").join("\\'") + "');}return p.join('');";

            str.replace(/[\r\t\n]/g, " ")
               .replace(/'(?=[^#]*#>)/g, "\t")
               .split("'").join("\\'")
               .split("\t").join("'")
               .replace(/<#=(.+?)#>/g, "',$1,'")
               .split("<#").join("');")
               .split("#>").join("p.push('")
               + "');}return p.join('');";

         //alert(strFunc);
         func = new Function("obj", strFunc);
         _tmplCache[str] = func;
      }
      return func(data);
   } catch (e) { err = e.message; }
   return "< # ERROR: " + err.htmlEncode() + " # >";
};

Array.prototype.contains = function(obj) {
   var i = this.length;
   while (i--) {
      if (this[i] === obj) {
         return true;
      }
   }
   return false;
};

//made by minidxer 
//For more visit http://ntt.cc
// Trim() , Ltrim() , RTrim()
// trim blank space at the string

String.prototype.Trim = function() {
   return this.replace(/(^\s*)|(\s*$)/g, "");
};

// trim blank space at the beginning
String.prototype.LTrim = function() {
   return this.replace(/(^\s*)/g, "");
};

// trim blank space at the end
String.prototype.RTrim = function() {
   return this.replace(/(\s*$)/g, "");
};

function sharer(proxyUrl, url, title) {
   var _I = this;
   this._url = url;
   this._title = title;
   this._proxyUrl = proxyUrl;

   this.email = function(from, to, name, body, eresult) {
      var p = new serviceProxy(_I._proxyUrl);
      var DTO = { 'source': from, 'dest': to, 'name': name, 'url': _url, 'title': _title, 'body': body };
      p.invoke("ShareEmail", DTO, function(res) {
         if (res) {
            var it = res.split("|");
            if (it.length < 1)
            { return; }
            //alert(it[1]);
            if (eresult) {
               eresult(it);
            }

         }

      }, function(err) { alert(err); });
   }
}


/* 
jQuery addCaption plugin
Copyright 2008 by Paul Boutin 
Licensed under the Creative Commons Attribution 3.0 license
Version 1.0.0
http://www.redsgn.com/category/jquery/
	
*/

(function($) {
   $.fn.addcaption = function(options) {
      var defaults = {
         bgcolor: '#fff',
         fontcolor: '#333',
         font: 'Arial, Helvetica, sans-serif',
         fontsize: '10px;',
         margin: 'margin:5px;',
         align: 'float:right;',
         width: 'width:auto;',
         border: 'none;'
      };
      var options = $.extend(defaults, options);

      return this.each(function() {
         if ($(this).attr('alt')) {
            var margin = options.margin;
            if ($(this).attr('vspace') != -1 && $(this).attr('hspace') != -1) {
               margin = 'margin:' + $(this).attr('vspace') + 'px ' + $(this).attr('hspace') + 'px;';
               $(this).removeAttr('vspace').removeAttr('hspace');
            } else if ($(this).attr('vspace') != -1) {
               margin = 'margin:' + $(this).attr('vspace') + 'px 0px;';
               $(this).removeAttr('vspace');
            } else if ($(this).attr('hspace') != -1) {
               margin = 'margin:0px ' + $(this).attr('hspace') + 'px;';
               $(this).removeAttr('hspace');
            }
            var align = options.align;
            if ($(this).attr('align') && ($(this).attr('align').toLowerCase() == 'right' || $(this).attr('align').toLowerCase() == 'left')) {
               align = 'float:' + $(this).attr('align').toLowerCase() + ';';
            }
            if ($(this).attr('align')) {
               $(this).removeAttr('align');
            }
            var width = options.width;
            if ($(this).attr('width')) {
               width = 'width:' + $(this).attr('width') + 'px;';
            }
            var border = options.border;
            if ($(this).attr('border')) {
               border = 'border:' + $(this).attr('border') + 'px solid #000;';
               $(this).removeAttr('border');
            }
            $(this).attr('style', 'margin:auto;border:none;');
            $(this).wrap('<div class="imgcap"></div>');
            $(this).parent('div').append('<div class="icpbx" style="width:' + (parseInt($(this).attr('width')) - 4) + 'px;">' + $(this).attr('alt') + '</div>');
            if ($(this).parent('div').parent('a')) {
               if ($.browser.msie) {
                  $(this).parent('div').attr('style', $(this).parent('div').attr('style') + 'cursor:pointer;');
                  $(this).parent('div').parent('a').attr('style', 'text-decoration:none;cursor:hand;');
               } else {
                  $(this).parent('div').attr('style', $(this).parent('span').attr('style') + 'cursor:pointer;');
                  $(this).parent('div').parent('a').attr('style', 'text-decoration:none;cursor:pointer;');
               }
            }
         }
      });
   };
})(jQuery);



// tipsy
(function($) {
   $.fn.tipsy = function(options) {

      options = $.extend({}, $.fn.tipsy.defaults, options);

      return this.each(function() {

         var opts = $.fn.tipsy.elementOptions(this, options);

         $(this).hover(function() {

            $.data(this, 'cancel.tipsy', true);

            var tip = $.data(this, 'active.tipsy');
            if (!tip) {
               tip = $('<div class="tipsy"><div class="tipsy-inner"/></div>');
               tip.css({ position: 'absolute', zIndex: 100000 });
               $.data(this, 'active.tipsy', tip);
            }

            if ($(this).attr('title') || typeof ($(this).attr('original-title')) != 'string') {
               $(this).attr('original-title', $(this).attr('title') || '').removeAttr('title');
            }

            var title;
            if (typeof opts.title == 'string') {
               title = $(this).attr(opts.title == 'title' ? 'original-title' : opts.title);
            } else if (typeof opts.title == 'function') {
               title = opts.title.call(this);
            }

            tip.find('.tipsy-inner')[opts.html ? 'html' : 'text'](title || opts.fallback);

            var pos = $.extend({}, $(this).offset(), { width: this.offsetWidth, height: this.offsetHeight });
            tip.get(0).className = 'tipsy'; // reset classname in case of dynamic gravity
            tip.remove().css({ top: 0, left: 0, visibility: 'hidden', display: 'block' }).appendTo(document.body);
            var actualWidth = tip[0].offsetWidth, actualHeight = tip[0].offsetHeight;
            var gravity = (typeof opts.gravity == 'function') ? opts.gravity.call(this) : opts.gravity;

            switch (gravity.charAt(0)) {
               case 'n':
                  tip.css({ top: pos.top + pos.height, left: pos.left + pos.width / 2 - actualWidth / 2 }).addClass('tipsy-north');
                  break;
               case 's':
                  tip.css({ top: pos.top - actualHeight, left: pos.left + pos.width / 2 - actualWidth / 2 }).addClass('tipsy-south');
                  break;
               case 'e':
                  tip.css({ top: pos.top + pos.height / 2 - actualHeight / 2, left: pos.left - actualWidth }).addClass('tipsy-east');
                  break;
               case 'w':
                  tip.css({ top: pos.top + pos.height / 2 - actualHeight / 2, left: pos.left + pos.width }).addClass('tipsy-west');
                  break;
            }

            if (opts.fade) {
               tip.css({ opacity: 0, display: 'block', visibility: 'visible' }).animate({ opacity: 0.8 });
            } else {
               tip.css({ visibility: 'visible' });
            }

         }, function() {
            $.data(this, 'cancel.tipsy', false);
            var self = this;
            setTimeout(function() {
               if ($.data(this, 'cancel.tipsy')) return;
               var tip = $.data(self, 'active.tipsy');
               if (opts.fade) {
                  tip.stop().fadeOut(function() { $(this).remove(); });
               } else {
                  tip.remove();
               }
            }, 100);

         });

      });

   };

   // Overwrite this method to provide options on a per-element basis.
   // For example, you could store the gravity in a 'tipsy-gravity' attribute:
   // return $.extend({}, options, {gravity: $(ele).attr('tipsy-gravity') || 'n' });
   // (remember - do not modify 'options' in place!)
   $.fn.tipsy.elementOptions = function(ele, options) {
      return $.metadata ? $.extend({}, options, $(ele).metadata()) : options;
   };

   $.fn.tipsy.defaults = {
      fade: false,
      fallback: '',
      gravity: 'n',
      html: false,
      title: 'title'
   };

   $.fn.tipsy.autoNS = function() {
      return $(this).offset().top > ($(document).scrollTop() + $(window).height() / 2) ? 's' : 'n';
   };

   $.fn.tipsy.autoWE = function() {
      return $(this).offset().left > ($(document).scrollLeft() + $(window).width() / 2) ? 'e' : 'w';
   };

})(jQuery);


//
/**
* jQuery.ScrollTo - Easy element scrolling using jQuery.
* Copyright (c) 2007-2009 Ariel Flesler - aflesler(at)gmail(dot)com | http://flesler.blogspot.com
* Dual licensed under MIT and GPL.
* Date: 5/25/2009
* @author Ariel Flesler
* @version 1.4.2
*
* http://flesler.blogspot.com/2007/10/jqueryscrollto.html
*/
; (function(d) { var k = d.scrollTo = function(a, i, e) { d(window).scrollTo(a, i, e) }; k.defaults = { axis: 'xy', duration: parseFloat(d.fn.jquery) >= 1.3 ? 0 : 1 }; k.window = function(a) { return d(window)._scrollable() }; d.fn._scrollable = function() { return this.map(function() { var a = this, i = !a.nodeName || d.inArray(a.nodeName.toLowerCase(), ['iframe', '#document', 'html', 'body']) != -1; if (!i) return a; var e = (a.contentWindow || a).document || a.ownerDocument || a; return d.browser.safari || e.compatMode == 'BackCompat' ? e.body : e.documentElement }) }; d.fn.scrollTo = function(n, j, b) { if (typeof j == 'object') { b = j; j = 0 } if (typeof b == 'function') b = { onAfter: b }; if (n == 'max') n = 9e9; b = d.extend({}, k.defaults, b); j = j || b.speed || b.duration; b.queue = b.queue && b.axis.length > 1; if (b.queue) j /= 2; b.offset = p(b.offset); b.over = p(b.over); return this._scrollable().each(function() { var q = this, r = d(q), f = n, s, g = {}, u = r.is('html,body'); switch (typeof f) { case 'number': case 'string': if (/^([+-]=)?\d+(\.\d+)?(px|%)?$/.test(f)) { f = p(f); break } f = d(f, this); case 'object': if (f.is || f.style) s = (f = d(f)).offset() } d.each(b.axis.split(''), function(a, i) { var e = i == 'x' ? 'Left' : 'Top', h = e.toLowerCase(), c = 'scroll' + e, l = q[c], m = k.max(q, i); if (s) { g[c] = s[h] + (u ? 0 : l - r.offset()[h]); if (b.margin) { g[c] -= parseInt(f.css('margin' + e)) || 0; g[c] -= parseInt(f.css('border' + e + 'Width')) || 0 } g[c] += b.offset[h] || 0; if (b.over[h]) g[c] += f[i == 'x' ? 'width' : 'height']() * b.over[h] } else { var o = f[h]; g[c] = o.slice && o.slice(-1) == '%' ? parseFloat(o) / 100 * m : o } if (/^\d+$/.test(g[c])) g[c] = g[c] <= 0 ? 0 : Math.min(g[c], m); if (!a && b.queue) { if (l != g[c]) t(b.onAfterFirst); delete g[c] } }); t(b.onAfter); function t(a) { r.animate(g, j, b.easing, a && function() { a.call(this, n, b) }) } }).end() }; k.max = function(a, i) { var e = i == 'x' ? 'Width' : 'Height', h = 'scroll' + e; if (!d(a).is('html,body')) return a[h] - d(a)[e.toLowerCase()](); var c = 'client' + e, l = a.ownerDocument.documentElement, m = a.ownerDocument.body; return Math.max(l[h], m[h]) - Math.min(l[c], m[c]) }; function p(a) { return typeof a == 'object' ? a : { top: a, left: a} } })(jQuery);




//BBCode

(function(f) { var n; var e; var p = new Object(); var o; var x = new Array(""); var A = 0; var a = null; var s = null; var t = "\n"; var g = false; f.fn.bbcodeeditor = function(B) { e = f.extend({}, f.fn.bbcodeeditor.defaults, B); n = this; if (!f.browser.opera) { n.keydown(c) } else { n.keypress(c) } if (f.browser.msie) { f(document).mousedown(function(C) { if (s != null && s == n[0]) { a = document.selection.createRange() } s = C.target }) } if (f.browser.msie || f.browser.opera) { t = "\r\n" } if (e.bold != false) { e.bold.click(function() { r("bold text", "[b]", "[/b]") }) } if (e.italic != false) { e.italic.click(function() { r("italic text", "[i]", "[/i]") }) } if (e.underline != false) { e.underline.click(function() { r("underline text", "[u]", "[/u]") }) } if (e.link != false) { e.link.click(q) } if (e.quote != false) { e.quote.click(function() { r("quote", "[quote]", "[/quote]", true) }) } if (e.code != false) { e.code.click(function() { r("function(event) {", "[code]", "[/code]", true) }) } if (e.image != false) { e.image.click(h) } if (e.nlist != false) { e.nlist.click(function() { r("list item", "[list=1]" + t + "[*]", "[/list]", true) }) } if (e.blist != false) { e.blist.click(function() { r("list item", "[list]" + t + "[*]", "[/list]", true) }) } if (e.litem != false) { e.litem.click(function() { r("list item", "[*]", "", true) }) } if (e.usize != false) { e.usize.click(function() { j(true) }) } if (e.dsize != false) { e.dsize.click(function() { j(false) }) } if (e.back != false) { if (e.back_disable != false) { p.back = e.back[0].className } e.back.click(v); u(false) } if (e.forward != false) { if (e.forward_disable != false) { p.forward = e.forward[0].className } e.forward.click(l); m(false) } if (e.back != false || e.forward != false) { n.keyup(w) } f.fn.bbcodeeditor.preview(); window.onbeforeunload = k; return this }; function u(B) { if (!B) { if (e.back_disable == false) { e.back.css("opacity", 0.5) } else { if (e.back != false) { e.back[0].className = e.back_disable } } } else { if (e.back_disable == false) { e.back.css("opacity", 1) } else { if (e.back != false) { e.back[0].className = p.back } } } } function m(B) { if (!B) { if (e.forward_disable == false) { e.forward.css("opacity", 0.5) } else { if (e.forward != false) { e.forward[0].className = e.forward_disable } } } else { if (e.forward_disable == false) { e.forward.css("opacity", 1) } else { if (e.forward != false) { e.forward[0].className = p.forward } } } } function k(B) { if (e.exit_warning && !g && n[0].value != "") { var B = B || window.event; if (B) { B.returnValue = "You have started writing a post." } return "You have started writing a post." } } function w(B) { if (f.browser.msie) { a = document.selection.createRange() } if (B.keyCode != 17 && !(B.ctrlKey && (B.keyCode == 89 || B.keyCode == 90))) { if (n.val().length != 0) { u(true) } else { u(false) } if (A != 0) { x.slice(0, x.length - A); m(false); A = 0 } if (B.keyCode == 8 || B.keyCode == 13 || B.keyCode == 32 || B.keyCode == 46 || (B.ctrlKey && (B.keyCode == 67 || B.keyCode == 86))) { d() } f.fn.bbcodeeditor.preview() } } function c(L) { if (f.browser.msie) { a = document.selection.createRange() } if (e.keyboard && L.ctrlKey) { if (L.keyCode == 66 && e.bold != false) { L.preventDefault(); r("bold text", "[b]", "[/b]") } else { if (L.keyCode == 73 && e.italic != false) { L.preventDefault(); r("italic text", "[i]", "[/i]") } else { if (L.keyCode == 75 && e.code != false) { L.preventDefault(); r("function(event) {", "[code]", "[/code]", true) } else { if (L.keyCode == 76 && e.link != false) { L.preventDefault(); q() } else { if (L.keyCode == 80 && e.image != false) { L.preventDefault(); h() } else { if (L.keyCode == 81 && e.quote != false) { L.preventDefault(); r("quote", "[quote]", "[/quote]", true) } else { if (L.keyCode == 85 && e.underline != false) { L.preventDefault(); r("underline text", "[u]", "[/u]") } else { if (L.keyCode == 89 && e.forward != false) { L.preventDefault(); l() } else { if (L.keyCode == 90 && e.back != false) { L.preventDefault(); v() } } } } } } } } } } if (L.keyCode == 13) { var B = y().start; var Q = n[0].value.substring(0, B).lastIndexOf("\n"); Q = (Q == -1 ? 0 : Q + 1); var H = n[0].value.substring(Q, B).match(/^\t+/g); if (H != null) { L.preventDefault(); var D = i(); var M = t; for (var F = 0; F < H[0].length; F++) { M += "\t" } n[0].value = n[0].value.substring(0, B) + M + n[0].value.substring(B); z(B + M.length, B + M.length); b(D) } } else { if (L.keyCode == 9) { L.preventDefault(); var D = i(); d(); var I = y(); if (I.start != I.end && n[0].value.substr(I.start, 1) == "\n") { I.start++ } var H = n[0].value.substring(I.start, I.end).match(/\n/g); if (H != null) { var K = n[0].value.substring(0, I.start).lastIndexOf(t); var P = (K != -1 ? K : 0); if (!L.shiftKey) { var C = n[0].value.substring(P, I.end).replace(/\n/g, "\n\t"); n[0].value = (K == -1 ? "\t" : "") + n[0].value.substring(0, P) + C + n[0].value.substring(I.end); z(I.start + 1, I.end + H.length + 1) } else { var F = (n[0].value.substr((K != -1 ? K + t.length : 0), 1) == "\t" ? 1 : 0); var J = n[0].value.substring(P, I.end).match(/\n\t/g, "\n"); if (K == -1 && n[0].value.substr(0, 1) == "\t") { n[0].value = n[0].value.substr(1); J.push(0) } var C = n[0].value.substring(P, I.end).replace(/\n\t/g, "\n"); n[0].value = n[0].value.substring(0, P) + C + n[0].value.substring(I.end); z(I.start - F, I.end - (J != null ? J.length : 0)) } } else { if (!L.shiftKey) { n[0].value = n[0].value.substring(0, I.start) + "\t" + n[0].value.substring(I.start); z(I.start + 1, I.start + 1) } else { var O = n[0].value.substring(0, I.start).lastIndexOf("\n"); var N = (O == -1 ? 0 : O); var E = n[0].value.substring(N + 1).indexOf("\n"); if (E == -1) { E = n[0].value.length } else { E += N + 1 } if (O == -1) { var G = n[0].value.substring(N, E).match(/^\t/); var C = n[0].value.substring(N, E).replace(/^\t/, "") } else { var G = n[0].value.substring(N, E).match(/\n\t/); var C = n[0].value.substring(N, E).replace(/\n\t/, "\n") } n[0].value = n[0].value.substring(0, N) + C + n[0].value.substring(E); if (G != null) { z(I.start - (I.start - 1 > O ? 1 : 0), I.end - ((I.start - 1 > O || I.start != I.end) ? 1 : 0)) } } } b(D) } } } function i() { return { scrollTop: n.scrollTop(), scrollHeight: n[0].scrollHeight} } function b(B) { n.scrollTop(B.scrollTop + n[0].scrollHeight - B.scrollHeight) } function d() { A = 0; m(false); u(true); if (x[x.length - 1] != n[0].value) { x.push(n[0].value) } } function v() { var B = n.scrollTop(); if (A == 0) { d(); A++ } if (A != x.length) { A++; n[0].value = x[x.length - A]; f.fn.bbcodeeditor.preview(); m(true); if (A == x.length) { u(false) } } n.scrollTop(B) } function l() { var B = n.scrollTop(); if (A > 1) { n[0].value = x[x.length - --A]; f.fn.bbcodeeditor.preview(); u(true); if (A == 1) { m(false) } } n.scrollTop(B) } function r(F, G, J, C) { d(); var H = y(); var B = i(); if (C) { if (J != "[/list]" && G != "[*]") { G = G + t } if (G != "[*]") { J = t + J } if (H.start != 0 && n[0].value.substr(H.start - 1, 1) != t.substr(0, 1)) { G = t + G } if (n[0].value.length != H.end && n[0].value.substr(H.end, 1) != t.substr(0, 1)) { J = J + t } } if (H.start != H.end) { F = n[0].value; if (C) { var I = new RegExp("\\[" + J.substring((t.length == 2 ? 4 : 3), J.length - 1) + "(.*?)\\]" + t + (J == t + "[/list]" ? "\\[\\*\\]" : "") + "$"); var K = new RegExp("^" + t + "\\[/" + J.substring((t.length == 2 ? 4 : 3), J.length - 1) + "\\]") } else { var I = new RegExp("\\[" + J.substring(2, J.length - 1) + "([^\\]]*?)\\]$", "g"); var K = new RegExp("^\\[/" + J.substring(2, J.length - 1) + "\\]", "g") } var D = F.substring(0, H.start).match(I); var E = F.substring(H.end).match(K); if (D != null && E != null) { n[0].value = F.substring(0, H.start).replace(I, "") + F.substring(H.start, H.end) + F.substring(H.end).replace(K, ""); z(H.start - D[0].length, H.end - D[0].length) } else { n[0].value = n[0].value.substr(0, H.start) + G + n[0].value.substring(H.start, H.end) + J + n[0].value.substr(H.end); z(H.start + G.length, H.end + G.length) } } else { n[0].value = n[0].value.substring(0, H.start) + G + F + J + n[0].value.substring(H.end); z(H.start + G.length, H.start + G.length + F.length) } b(B); f.fn.bbcodeeditor.preview() } function z(F, C) { if (!f.browser.msie) { n[0].setSelectionRange(F, C); n.focus() } else { var B = n[0].value.substring(0, F).match(/\r/g); B = (B != null ? B.length : 0); var E = n[0].value.substring(F, C).match(/\r/g); E = (E != null ? E.length : 0); var D = n[0].createTextRange(); D.collapse(true); D.moveStart("character", F - B); D.moveEnd("character", C - F - E); D.select(); a = document.selection.createRange() } } function j(B) { if (B) { r("text", "[size=150]", "[/size]") } else { r("text", "[size=80]", "[/size]") } } function h() { var B = "http://"; r(B, "[img]", "[/img]") } function q(C) { var B = "http://"; r("link text", "[url=" + B + "]", "[/url]") } function y() { if (!f.browser.msie) { return { start: n[0].selectionStart, end: n[0].selectionEnd} } else { if (a == null) { return { start: n[0].value.length, end: n[0].value.length} } var M = a.duplicate(); var F = document.body.createTextRange(); F.moveToElementText(n[0]); F.setEndPoint("EndToStart", M); var D = document.body.createTextRange(); D.moveToElementText(n[0]); D.setEndPoint("StartToEnd", M); var H = false, C = false, L = false; var J, G, K, B, I, E; J = G = F.text; K = B = M.text; I = E = D.text; do { if (!H) { if (F.compareEndPoints("StartToEnd", F) == 0) { H = true } else { F.moveEnd("character", -1); if (F.text == J) { G += "\r\n" } else { H = true } } } if (!C) { if (M.compareEndPoints("StartToEnd", M) == 0) { C = true } else { M.moveEnd("character", -1); if (M.text == K) { B += "\r\n" } else { C = true } } } if (!L) { if (D.compareEndPoints("StartToEnd", D) == 0) { L = true } else { D.moveEnd("character", -1); if (D.text == I) { E += "\r\n" } else { L = true } } } } while ((!H || !C || !L)); return { start: G.length, end: G.length + B.length} } } f.fn.bbcodeeditor.defaults = { bold: false, italic: false, underline: false, link: false, quote: false, code: false, image: false, usize: false, nsize: false, nlist: false, blist: false, litem: false, back: false, back_disable: false, forward: false, forward_disable: false, exit_warning: false, preview: false, keyboard: true }; f.fn.bbcodeeditor.preview = function() { if (e.preview != false) { var B = n.val(); B = B.replace(/</g, "&lt;"); B = B.replace(/>/g, "&gt;"); B = B.replace(/[\r\n]/g, "%lb%"); var E = [/\[b\](.*?)\[\/b\]/gi, /\[i\](.*?)\[\/i\]/gi, /\[u\](.*?)\[\/u\]/gi, /\[size=(8\d|9\d|1\d\d|200)](.*?)\[\/size\]/gi, /\[url(?:\=?)(.*?)\](.*?)\[\/url\]/gi, /\[img(.*?)\](.*?)\[\/img\]/gi, /(?:%lb%|\s)*\[code(?:\=?)(?:.*?)\](?:%lb%|\s)*(.*?)(?:%lb%|\s)*\[\/code\](?:%lb%|\s)*/gi, /(?:%lb%|\s)*\[quote(?:\=?)(.*?)\](?:%lb%|\s)*(.*?)(?:%lb%|\s)*\[\/quote\](?:%lb%|\s)*/gi, /\[list(.*?)\](.*?)\[\*\](.*?)(?:%lb%|\s)*(\[\*\].*?\[\/list\]|\[\/list\])/i, /(?:%lb%|\s)*\[list\](?:%lb%|\s)*(.*?)(?:%lb%|\s)*\[\/list\](?:%lb%|\s)*/gi, /(?:%lb%|\s)*\[list=(\d)\](?:%lb%|\s)*(.*?)(?:%lb%|\s)*\[\/list\](?:%lb%|\s)*/gi, /(?:%lb%){3,}/g]; var D = ["<b>$1</b>", "<i>$1</i>", "<u>$1</u>", '<span style="font-size:$1%;">$2</span>', '<a href="$1">$2</a>', '<img $1 src="$2" />', "<pre><code>$1</code></pre>", "<blockquote>$2</blockquote>", "[list$1]$2<li>$3</li>$4", "<ul>$1</ul>", "<ol start=$1>$2</ol>", "%lb%%lb%"]; for (var C in E) { B = B.replace(E[C], D[C]); if (C == 8) { while (B.match(E[C], D[C])) { B = B.replace(E[C], D[C]) } } } B = B.replace(/%lb%/g, "<br />"); e.preview.html(B) } }; f.fn.bbcodeeditor.pause = function() { if (!g) { g = true } else { g = false } } })(jQuery);

//json2

/*
http://www.JSON.org/json2.js
2009-06-29

Public Domain.

NO WARRANTY EXPRESSED OR IMPLIED. USE AT YOUR OWN RISK.

See http://www.JSON.org/js.html

This file creates a global JSON object containing two methods: stringify
and parse.

JSON.stringify(value, replacer, space)
value       any JavaScript value, usually an object or array.

replacer    an optional parameter that determines how object
values are stringified for objects. It can be a
function or an array of strings.

space       an optional parameter that specifies the indentation
of nested structures. If it is omitted, the text will
be packed without extra whitespace. If it is a number,
it will specify the number of spaces to indent at each
level. If it is a string (such as '\t' or '&nbsp;'),
it contains the characters used to indent at each level.

This method produces a JSON text from a JavaScript value.

When an object value is found, if the object contains a toJSON
method, its toJSON method will be called and the result will be
stringified. A toJSON method does not serialize: it returns the
value represented by the name/value pair that should be serialized,
or undefined if nothing should be serialized. The toJSON method
will be passed the key associated with the value, and this will be
bound to the object holding the key.

For example, this would serialize Dates as ISO strings.

Date.prototype.toJSON = function (key) {
function f(n) {
// Format integers to have at least two digits.
return n < 10 ? '0' + n : n;
}

return this.getUTCFullYear()   + '-' +
f(this.getUTCMonth() + 1) + '-' +
f(this.getUTCDate())      + 'T' +
f(this.getUTCHours())     + ':' +
f(this.getUTCMinutes())   + ':' +
f(this.getUTCSeconds())   + 'Z';
};

You can provide an optional replacer method. It will be passed the
key and value of each member, with this bound to the containing
object. The value that is returned from your method will be
serialized. If your method returns undefined, then the member will
be excluded from the serialization.

If the replacer parameter is an array of strings, then it will be
used to select the members to be serialized. It filters the results
such that only members with keys listed in the replacer array are
stringified.

Values that do not have JSON representations, such as undefined or
functions, will not be serialized. Such values in objects will be
dropped; in arrays they will be replaced with null. You can use
a replacer function to replace those with JSON values.
JSON.stringify(undefined) returns undefined.

The optional space parameter produces a stringification of the
value that is filled with line breaks and indentation to make it
easier to read.

If the space parameter is a non-empty string, then that string will
be used for indentation. If the space parameter is a number, then
the indentation will be that many spaces.

Example:

text = JSON.stringify(['e', {pluribus: 'unum'}]);
// text is '["e",{"pluribus":"unum"}]'


text = JSON.stringify(['e', {pluribus: 'unum'}], null, '\t');
// text is '[\n\t"e",\n\t{\n\t\t"pluribus": "unum"\n\t}\n]'

text = JSON.stringify([new Date()], function (key, value) {
return this[key] instanceof Date ?
'Date(' + this[key] + ')' : value;
});
// text is '["Date(---current time---)"]'


JSON.parse(text, reviver)
This method parses a JSON text to produce an object or array.
It can throw a SyntaxError exception.

The optional reviver parameter is a function that can filter and
transform the results. It receives each of the keys and values,
and its return value is used instead of the original value.
If it returns what it received, then the structure is not modified.
If it returns undefined then the member is deleted.

Example:

// Parse the text. Values that look like ISO date strings will
// be converted to Date objects.

myData = JSON.parse(text, function (key, value) {
var a;
if (typeof value === 'string') {
a =
/^(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2}(?:\.\d*)?)Z$/.exec(value);
if (a) {
return new Date(Date.UTC(+a[1], +a[2] - 1, +a[3], +a[4],
+a[5], +a[6]));
}
}
return value;
});

myData = JSON.parse('["Date(09/09/2001)"]', function (key, value) {
var d;
if (typeof value === 'string' &&
value.slice(0, 5) === 'Date(' &&
value.slice(-1) === ')') {
d = new Date(value.slice(5, -1));
if (d) {
return d;
}
}
return value;
});


This is a reference implementation. You are free to copy, modify, or
redistribute.

This code should be minified before deployment.
See http://javascript.crockford.com/jsmin.html

USE YOUR OWN COPY. IT IS EXTREMELY UNWISE TO LOAD CODE FROM SERVERS YOU DO
NOT CONTROL.
*/

/*jslint evil: true */

/*members "", "\b", "\t", "\n", "\f", "\r", "\"", JSON, "\\", apply,
call, charCodeAt, getUTCDate, getUTCFullYear, getUTCHours,
getUTCMinutes, getUTCMonth, getUTCSeconds, hasOwnProperty, join,
lastIndex, length, parse, prototype, push, replace, slice, stringify,
test, toJSON, toString, valueOf
*/

// Create a JSON object only if one does not already exist. We create the
// methods in a closure to avoid creating global variables.

var JSON = JSON || {};

(function() {

   function f(n) {
      // Format integers to have at least two digits.
      return n < 10 ? '0' + n : n;
   }

   if (typeof Date.prototype.toJSON !== 'function') {

      Date.prototype.toJSON = function(key) {

         return isFinite(this.valueOf()) ?
                   this.getUTCFullYear() + '-' +
                 f(this.getUTCMonth() + 1) + '-' +
                 f(this.getUTCDate()) + 'T' +
                 f(this.getUTCHours()) + ':' +
                 f(this.getUTCMinutes()) + ':' +
                 f(this.getUTCSeconds()) + 'Z' : null;
      };

      String.prototype.toJSON =
        Number.prototype.toJSON =
        Boolean.prototype.toJSON = function(key) {
           return this.valueOf();
        };
   }

   var cx = /[\u0000\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g,
        escapable = /[\\\"\x00-\x1f\x7f-\x9f\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g,
        gap,
        indent,
        meta = {    // table of character substitutions
           '\b': '\\b',
           '\t': '\\t',
           '\n': '\\n',
           '\f': '\\f',
           '\r': '\\r',
           '"': '\\"',
           '\\': '\\\\'
        },
        rep;


   function quote(string) {

      // If the string contains no control characters, no quote characters, and no
      // backslash characters, then we can safely slap some quotes around it.
      // Otherwise we must also replace the offending characters with safe escape
      // sequences.

      escapable.lastIndex = 0;
      return escapable.test(string) ?
            '"' + string.replace(escapable, function(a) {
               var c = meta[a];
               return typeof c === 'string' ? c :
                    '\\u' + ('0000' + a.charCodeAt(0).toString(16)).slice(-4);
            }) + '"' :
            '"' + string + '"';
   }


   function str(key, holder) {

      // Produce a string from holder[key].

      var i,          // The loop counter.
            k,          // The member key.
            v,          // The member value.
            length,
            mind = gap,
            partial,
            value = holder[key];

      // If the value has a toJSON method, call it to obtain a replacement value.

      if (value && typeof value === 'object' &&
                typeof value.toJSON === 'function') {
         value = value.toJSON(key);
      }

      // If we were called with a replacer function, then call the replacer to
      // obtain a replacement value.

      if (typeof rep === 'function') {
         value = rep.call(holder, key, value);
      }

      // What happens next depends on the value's type.

      switch (typeof value) {
         case 'string':
            return quote(value);

         case 'number':

            // JSON numbers must be finite. Encode non-finite numbers as null.

            return isFinite(value) ? String(value) : 'null';

         case 'boolean':
         case 'null':

            // If the value is a boolean or null, convert it to a string. Note:
            // typeof null does not produce 'null'. The case is included here in
            // the remote chance that this gets fixed someday.

            return String(value);

            // If the type is 'object', we might be dealing with an object or an array or
            // null.

         case 'object':

            // Due to a specification blunder in ECMAScript, typeof null is 'object',
            // so watch out for that case.

            if (!value) {
               return 'null';
            }

            // Make an array to hold the partial results of stringifying this object value.

            gap += indent;
            partial = [];

            // Is the value an array?

            if (Object.prototype.toString.apply(value) === '[object Array]') {

               // The value is an array. Stringify every element. Use null as a placeholder
               // for non-JSON values.

               length = value.length;
               for (i = 0; i < length; i += 1) {
                  partial[i] = str(i, value) || 'null';
               }

               // Join all of the elements together, separated with commas, and wrap them in
               // brackets.

               v = partial.length === 0 ? '[]' :
                    gap ? '[\n' + gap +
                            partial.join(',\n' + gap) + '\n' +
                                mind + ']' :
                          '[' + partial.join(',') + ']';
               gap = mind;
               return v;
            }

            // If the replacer is an array, use it to select the members to be stringified.

            if (rep && typeof rep === 'object') {
               length = rep.length;
               for (i = 0; i < length; i += 1) {
                  k = rep[i];
                  if (typeof k === 'string') {
                     v = str(k, value);
                     if (v) {
                        partial.push(quote(k) + (gap ? ': ' : ':') + v);
                     }
                  }
               }
            } else {

               // Otherwise, iterate through all of the keys in the object.

               for (k in value) {
                  if (Object.hasOwnProperty.call(value, k)) {
                     v = str(k, value);
                     if (v) {
                        partial.push(quote(k) + (gap ? ': ' : ':') + v);
                     }
                  }
               }
            }

            // Join all of the member texts together, separated with commas,
            // and wrap them in braces.

            v = partial.length === 0 ? '{}' :
                gap ? '{\n' + gap + partial.join(',\n' + gap) + '\n' +
                        mind + '}' : '{' + partial.join(',') + '}';
            gap = mind;
            return v;
      }
   }

   // If the JSON object does not yet have a stringify method, give it one.

   if (typeof JSON.stringify !== 'function') {
      JSON.stringify = function(value, replacer, space) {

         // The stringify method takes a value and an optional replacer, and an optional
         // space parameter, and returns a JSON text. The replacer can be a function
         // that can replace values, or an array of strings that will select the keys.
         // A default replacer method can be provided. Use of the space parameter can
         // produce text that is more easily readable.

         var i;
         gap = '';
         indent = '';

         // If the space parameter is a number, make an indent string containing that
         // many spaces.

         if (typeof space === 'number') {
            for (i = 0; i < space; i += 1) {
               indent += ' ';
            }

            // If the space parameter is a string, it will be used as the indent string.

         } else if (typeof space === 'string') {
            indent = space;
         }

         // If there is a replacer, it must be a function or an array.
         // Otherwise, throw an error.

         rep = replacer;
         if (replacer && typeof replacer !== 'function' &&
                    (typeof replacer !== 'object' ||
                     typeof replacer.length !== 'number')) {
            throw new Error('JSON.stringify');
         }

         // Make a fake root object containing our value under the key of ''.
         // Return the result of stringifying the value.

         return str('', { '': value });
      };
   }


   // If the JSON object does not yet have a parse method, give it one.

   if (typeof JSON.parse !== 'function') {
      JSON.parse = function(text, reviver) {

         // The parse method takes a text and an optional reviver function, and returns
         // a JavaScript value if the text is a valid JSON text.

         var j;

         function walk(holder, key) {

            // The walk method is used to recursively walk the resulting structure so
            // that modifications can be made.

            var k, v, value = holder[key];
            if (value && typeof value === 'object') {
               for (k in value) {
                  if (Object.hasOwnProperty.call(value, k)) {
                     v = walk(value, k);
                     if (v !== undefined) {
                        value[k] = v;
                     } else {
                        delete value[k];
                     }
                  }
               }
            }
            return reviver.call(holder, key, value);
         }


         // Parsing happens in four stages. In the first stage, we replace certain
         // Unicode characters with escape sequences. JavaScript handles many characters
         // incorrectly, either silently deleting them, or treating them as line endings.

         cx.lastIndex = 0;
         if (cx.test(text)) {
            text = text.replace(cx, function(a) {
               return '\\u' +
                        ('0000' + a.charCodeAt(0).toString(16)).slice(-4);
            });
         }

         // In the second stage, we run the text against regular expressions that look
         // for non-JSON patterns. We are especially concerned with '()' and 'new'
         // because they can cause invocation, and '=' because it can cause mutation.
         // But just to be safe, we want to reject all unexpected forms.

         // We split the second stage into 4 regexp operations in order to work around
         // crippling inefficiencies in IE's and Safari's regexp engines. First we
         // replace the JSON backslash pairs with '@' (a non-JSON character). Second, we
         // replace all simple value tokens with ']' characters. Third, we delete all
         // open brackets that follow a colon or comma or that begin the text. Finally,
         // we look to see that the remaining characters are only whitespace or ']' or
         // ',' or ':' or '{' or '}'. If that is so, then the text is safe for eval.

         if (/^[\],:{}\s]*$/.
test(text.replace(/\\(?:["\\\/bfnrt]|u[0-9a-fA-F]{4})/g, '@').
replace(/"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g, ']').
replace(/(?:^|:|,)(?:\s*\[)+/g, ''))) {

            // In the third stage we use the eval function to compile the text into a
            // JavaScript structure. The '{' operator is subject to a syntactic ambiguity
            // in JavaScript: it can begin a block or an object literal. We wrap the text
            // in parens to eliminate the ambiguity.

            j = eval('(' + text + ')');

            // In the optional fourth stage, we recursively walk the new structure, passing
            // each name/value pair to a reviver function for possible transformation.

            return typeof reviver === 'function' ?
                    walk({ '': j }, '') : j;
         }

         // If the text is not JSON parseable, then a SyntaxError is thrown.

         throw new SyntaxError('JSON.parse');
      };
   }
} ());

// JqModal

/*
* jqModal - Minimalist Modaling with jQuery
*   (http://dev.iceburg.net/jquery/jqModal/)
*
* Copyright (c) 2007,2008 Brice Burgess <bhb@iceburg.net>
* Dual licensed under the MIT and GPL licenses:
*   http://www.opensource.org/licenses/mit-license.php
*   http://www.gnu.org/licenses/gpl.html
* 
* $Version: 03/01/2009 +r14
*/
(function($) {
   $.fn.jqm = function(o) {
      var p = {
         overlay: 50,
         overlayClass: 'jqmOverlay',
         closeClass: 'jqmClose',
         trigger: '.jqModal',
         ajax: F,
         ajaxText: '',
         target: F,
         modal: F,
         toTop: F,
         onShow: F,
         onHide: F,
         onLoad: F
      };
      return this.each(function() {
         if (this._jqm) return H[this._jqm].c = $.extend({}, H[this._jqm].c, o); s++; this._jqm = s;
         H[s] = { c: $.extend(p, $.jqm.params, o), a: F, w: $(this).addClass('jqmID' + s), s: s };
         if (p.trigger) $(this).jqmAddTrigger(p.trigger);
      });
   };

   $.fn.jqmAddClose = function(e) { return hs(this, e, 'jqmHide'); };
   $.fn.jqmAddTrigger = function(e) { return hs(this, e, 'jqmShow'); };
   $.fn.jqmShow = function(t) { return this.each(function() { t = t || window.event; $.jqm.open(this._jqm, t); }); };
   $.fn.jqmHide = function(t) { return this.each(function() { t = t || window.event; $.jqm.close(this._jqm, t) }); };

   $.jqm = {
      hash: {},
      open: function(s, t) {
         var h = H[s], c = h.c, cc = '.' + c.closeClass, z = (parseInt(h.w.css('z-index'))), z = (z > 0) ? z : 3000, o = $('<div></div>').css({ height: '100%', width: '100%', position: 'fixed', left: 0, top: 0, 'z-index': z - 1, opacity: c.overlay / 100 }); if (h.a) return F; h.t = t; h.a = true; h.w.css('z-index', z);
         if (c.modal) { if (!A[0]) L('bind'); A.push(s); }
         else if (c.overlay > 0) h.w.jqmAddClose(o);
         else o = F;

         h.o = (o) ? o.addClass(c.overlayClass).prependTo('body') : F;
         if (ie6) { $('html,body').css({ height: '100%', width: '100%' }); if (o) { o = o.css({ position: 'absolute' })[0]; for (var y in { Top: 1, Left: 1 }) o.style.setExpression(y.toLowerCase(), "(_=(document.documentElement.scroll" + y + " || document.body.scroll" + y + "))+'px'"); } }

         if (c.ajax) {
            var r = c.target || h.w, u = c.ajax, r = (typeof r == 'string') ? $(r, h.w) : $(r), u = (u.substr(0, 1) == '@') ? $(t).attr(u.substring(1)) : u;
            r.html(c.ajaxText).load(u, function() { if (c.onLoad) c.onLoad.call(this, h); if (cc) h.w.jqmAddClose($(cc, h.w)); e(h); });
         }
         else if (cc) h.w.jqmAddClose($(cc, h.w));

         if (c.toTop && h.o) h.w.before('<span id="jqmP' + h.w[0]._jqm + '"></span>').insertAfter(h.o);
         (c.onShow) ? c.onShow(h) : h.w.show(); e(h); return F;
      },
      close: function(s) {
         var h = H[s]; if (!h.a) return F; h.a = F;
         if (A[0]) { A.pop(); if (!A[0]) L('unbind'); }
         if (h.c.toTop && h.o) $('#jqmP' + h.w[0]._jqm).after(h.w).remove();
         if (h.c.onHide) h.c.onHide(h); else { h.w.hide(); if (h.o) h.o.remove(); } return F;
      },
      params: {}
   };
   var s = 0, H = $.jqm.hash, A = [], ie6 = $.browser.msie && ($.browser.version == "6.0"), F = false,
i = $('<iframe src="javascript:false;document.write(\'\');" class="jqm"></iframe>').css({ opacity: 0 }),
e = function(h) { if (ie6) if (h.o) h.o.html('<p style="width:100%;height:100%"/>').prepend(i); else if (!$('iframe.jqm', h.w)[0]) h.w.prepend(i); f(h); },
f = function(h) { try { $(':input:visible', h.w)[0].focus(); } catch (_) { } },
L = function(t) { $()[t]("keypress", m)[t]("keydown", m)[t]("mousedown", m); },
m = function(e) { var h = H[A[A.length - 1]], r = (!$(e.target).parents('.jqmID' + h.s)[0]); if (r) f(h); return !r; },
hs = function(w, t, c) {
   return w.each(function() {
      var s = this._jqm; $(t).each(function() {
         if (!this[c]) { this[c] = []; $(this).click(function() { for (var i in { jqmShow: 1, jqmHide: 1 }) for (var s in this[i]) if (H[this[i][s]]) H[this[i][s]].w[i](this); return F; }); } this[c].push(s);
      });
   });
};
})(jQuery);

//  jquery rating

/*
### jQuery Star Rating Plugin v3.12 - 2009-04-16 ###
* Home: http://www.fyneworks.com/jquery/star-rating/
* Code: http://code.google.com/p/jquery-star-rating-plugin/
*
* Dual licensed under the MIT and GPL licenses:
*   http://www.opensource.org/licenses/mit-license.php
*   http://www.gnu.org/licenses/gpl.html
###
*/

/*# AVOID COLLISIONS #*/
; if (window.jQuery) (function($) {
   /*# AVOID COLLISIONS #*/

   // IE6 Background Image Fix
   if ($.browser.msie) try { document.execCommand("BackgroundImageCache", false, true) } catch (e) { };
   // Thanks to http://www.visualjquery.com/rating/rating_redux.html

   // plugin initialization
   $.fn.rating = function(options) {
      if (this.length == 0) return this; // quick fail

      // Handle API methods
      if (typeof arguments[0] == 'string') {
         // Perform API methods on individual elements
         if (this.length > 1) {
            var args = arguments;
            return this.each(function() {
               $.fn.rating.apply($(this), args);
            });
         };
         // Invoke API method handler
         $.fn.rating[arguments[0]].apply(this, $.makeArray(arguments).slice(1) || []);
         // Quick exit...
         return this;
      };

      // Initialize options for this call
      var options = $.extend(
			{}/* new object */,
			$.fn.rating.options/* default options */,
			options || {} /* just-in-time options */
		);

      // Allow multiple controls with the same name by making each call unique
      $.fn.rating.calls++;

      // loop through each matched element
      this
		 .not('.star-rating-applied')
			.addClass('star-rating-applied')
		.each(function() {

		   // Load control parameters / find context / etc
		   var control, input = $(this);
		   var eid = (this.name || 'unnamed-rating').replace(/\[|\]/g, '_').replace(/^\_+|\_+$/g, '');
		   var context = $(this.form || document.body);

		   // FIX: http://code.google.com/p/jquery-star-rating-plugin/issues/detail?id=23
		   var raters = context.data('rating');
		   if (!raters || raters.call != $.fn.rating.calls) raters = { count: 0, call: $.fn.rating.calls };
		   var rater = raters[eid];

		   // if rater is available, verify that the control still exists
		   if (rater) control = rater.data('rating');

		   if (rater && control)//{// save a byte!
		   // add star to control if rater is available and the same control still exists
		      control.count++;

		   //}// save a byte!
		   else {
		      // create new control if first star or control element was removed/replaced

		      // Initialize options for this raters
		      control = $.extend(
					{}/* new object */,
					options || {} /* current call options */,
					($.metadata ? input.metadata() : ($.meta ? input.data() : null)) || {}, /* metadata options */
					{count: 0, stars: [], inputs: [] }
				);

		      // increment number of rating controls
		      control.serial = raters.count++;

		      // create rating element
		      rater = $('<span class="star-rating-control"/>');
		      input.before(rater);

		      // Mark element for initialization (once all stars are ready)
		      rater.addClass('rating-to-be-drawn');

		      // Accept readOnly setting from 'disabled' property
		      if (input.attr('disabled')) control.readOnly = true;

		      // Create 'cancel' button
		      rater.append(
					control.cancel = $('<div class="rating-cancel"><a title="' + control.cancel + '">' + control.cancelValue + '</a></div>')
					.mouseover(function() {
					   $(this).rating('drain');
					   $(this).addClass('star-rating-hover');
					   //$(this).rating('focus');
					})
					.mouseout(function() {
					   $(this).rating('draw');
					   $(this).removeClass('star-rating-hover');
					   //$(this).rating('blur');
					})
					.click(function() {
					   $(this).rating('select');
					})
					.data('rating', control)
				);

		   }; // first element of group

		   // insert rating star
		   var star = $('<div class="star-rating rater-' + control.serial + '"><a title="' + (this.title || this.value) + '">' + this.value + '</a></div>');
		   rater.append(star);

		   // inherit attributes from input element
		   if (this.id) star.attr('id', this.id);
		   if (this.className) star.addClass(this.className);

		   // Half-stars?
		   if (control.half) control.split = 2;

		   // Prepare division control
		   if (typeof control.split == 'number' && control.split > 0) {
		      var stw = ($.fn.width ? star.width() : 0) || control.starWidth;
		      var spi = (control.count % control.split), spw = Math.floor(stw / control.split);
		      star
		      // restrict star's width and hide overflow (already in CSS)
				.width(spw)
		      // move the star left by using a negative margin
		      // this is work-around to IE's stupid box model (position:relative doesn't work)
				.find('a').css({ 'margin-left': '-' + (spi * spw) + 'px' })
		   };

		   // readOnly?
		   if (control.readOnly)//{ //save a byte!
		   // Mark star as readOnly so user can customize display
		      star.addClass('star-rating-readonly');
		   //}  //save a byte!
		   else//{ //save a byte!
		   // Enable hover css effects
		      star.addClass('star-rating-live')
		   // Attach mouse events
					.mouseover(function() {
					   $(this).rating('fill');
					   $(this).rating('focus');
					})
					.mouseout(function() {
					   $(this).rating('draw');
					   $(this).rating('blur');
					})
					.click(function() {
					   $(this).rating('select');
					})
				;
		   //}; //save a byte!

		   // set current selection
		   if (this.checked) control.current = star;

		   // hide input element
		   input.hide();

		   // backward compatibility, form element to plugin
		   input.change(function() {
		      $(this).rating('select');
		   });

		   // attach reference to star to input element and vice-versa
		   star.data('rating.input', input.data('rating.star', star));

		   // store control information in form (or body when form not available)
		   control.stars[control.stars.length] = star[0];
		   control.inputs[control.inputs.length] = input[0];
		   control.rater = raters[eid] = rater;
		   control.context = context;

		   input.data('rating', control);
		   rater.data('rating', control);
		   star.data('rating', control);
		   context.data('rating', raters);
		}); // each element

      // Initialize ratings (first draw)
      $('.rating-to-be-drawn').rating('draw').removeClass('rating-to-be-drawn');

      return this; // don't break the chain...
   };

   /*--------------------------------------------------------*/

   /*
   ### Core functionality and API ###
   */
   $.extend($.fn.rating, {
      // Used to append a unique serial number to internal control ID
      // each time the plugin is invoked so same name controls can co-exist
      calls: 0,

      focus: function() {
         var control = this.data('rating'); if (!control) return this;
         if (!control.focus) return this; // quick fail if not required
         // find data for event
         var input = $(this).data('rating.input') || $(this.tagName == 'INPUT' ? this : null);
         // focus handler, as requested by focusdigital.co.uk
         if (control.focus) control.focus.apply(input[0], [input.val(), $('a', input.data('rating.star'))[0]]);
      }, // $.fn.rating.focus

      blur: function() {
         var control = this.data('rating'); if (!control) return this;
         if (!control.blur) return this; // quick fail if not required
         // find data for event
         var input = $(this).data('rating.input') || $(this.tagName == 'INPUT' ? this : null);
         // blur handler, as requested by focusdigital.co.uk
         if (control.blur) control.blur.apply(input[0], [input.val(), $('a', input.data('rating.star'))[0]]);
      }, // $.fn.rating.blur

      fill: function() { // fill to the current mouse position.
         var control = this.data('rating'); if (!control) return this;
         // do not execute when control is in read-only mode
         if (control.readOnly) return;
         // Reset all stars and highlight them up to this element
         this.rating('drain');
         this.prevAll().andSelf().filter('.rater-' + control.serial).addClass('star-rating-hover');
      }, // $.fn.rating.fill

      drain: function() { // drain all the stars.
         var control = this.data('rating'); if (!control) return this;
         // do not execute when control is in read-only mode
         if (control.readOnly) return;
         // Reset all stars
         control.rater.children().filter('.rater-' + control.serial).removeClass('star-rating-on').removeClass('star-rating-hover');
      }, // $.fn.rating.drain

      draw: function() { // set value and stars to reflect current selection
         var control = this.data('rating'); if (!control) return this;
         // Clear all stars
         this.rating('drain');
         // Set control value
         if (control.current) {
            control.current.data('rating.input').attr('checked', 'checked');
            control.current.prevAll().andSelf().filter('.rater-' + control.serial).addClass('star-rating-on');
         }
         else
            $(control.inputs).removeAttr('checked');
         // Show/hide 'cancel' button
         control.cancel[control.readOnly || control.required ? 'hide' : 'show']();
         // Add/remove read-only classes to remove hand pointer
         this.siblings()[control.readOnly ? 'addClass' : 'removeClass']('star-rating-readonly');
      }, // $.fn.rating.draw

      select: function(value) { // select a value
         var control = this.data('rating'); if (!control) return this;
         // do not execute when control is in read-only mode
         if (control.readOnly) return;
         // clear selection
         control.current = null;
         // programmatically (based on user input)
         if (typeof value != 'undefined') {
            // select by index (0 based)
            if (typeof value == 'number')
               return $(control.stars[value]).rating('select');
            // select by literal value (must be passed as a string
            if (typeof value == 'string')
            //return 
               $.each(control.stars, function() {
                  if ($(this).data('rating.input').val() == value) $(this).rating('select');
               });
         }
         else
            control.current = this[0].tagName == 'INPUT' ?
				 this.data('rating.star') :
					(this.is('.rater-' + control.serial) ? this : null);

         // Update rating control state
         this.data('rating', control);
         // Update display
         this.rating('draw');
         // find data for event
         var input = $(control.current ? control.current.data('rating.input') : null);
         // click callback, as requested here: http://plugins.jquery.com/node/1655
         if (control.callback) control.callback.apply(input[0], [input.val(), $('a', control.current)[0]]); // callback event
      }, // $.fn.rating.select

      readOnly: function(toggle, disable) { // make the control read-only (still submits value)
         var control = this.data('rating'); if (!control) return this;
         // setread-only status
         control.readOnly = toggle || toggle == undefined ? true : false;
         // enable/disable control value submission
         if (disable) $(control.inputs).attr("disabled", "disabled");
         else $(control.inputs).removeAttr("disabled");
         // Update rating control state
         this.data('rating', control);
         // Update display
         this.rating('draw');
      }, // $.fn.rating.readOnly

      disable: function() { // make read-only and never submit value
         this.rating('readOnly', true, true);
      }, // $.fn.rating.disable

      enable: function() { // make read/write and submit value
         this.rating('readOnly', false, false);
      } // $.fn.rating.select

   });

   /*--------------------------------------------------------*/

   /*
   ### Default Settings ###
   eg.: You can override default control like this:
   $.fn.rating.options.cancel = 'Clear';
   */
   $.fn.rating.options = { //$.extend($.fn.rating, { options: {
      cancel: 'Cancel Rating',   // advisory title for the 'cancel' link
      cancelValue: '',           // value to submit when user click the 'cancel' link
      split: 0,                  // split the star into how many parts?

      // Width of star image in case the plugin can't work it out. This can happen if
      // the jQuery.dimensions plugin is not available OR the image is hidden at installation
      starWidth: 16//,

      //NB.: These don't need to be pre-defined (can be undefined/null) so let's save some code!
      //half:     false,         // just a shortcut to control.split = 2
      //required: false,         // disables the 'cancel' button so user can only select one of the specified values
      //readOnly: false,         // disable rating plugin interaction/ values cannot be changed
      //focus:    function(){},  // executed when stars are focused
      //blur:     function(){},  // executed when stars are focused
      //callback: function(){},  // executed when a star is clicked
   }; //} });

   /*--------------------------------------------------------*/

   /*
   ### Default implementation ###
   The plugin will attach itself to file inputs
   with the class 'multi' when the page loads
   */
   $(function() {
      $('input[type=radio].multi').rating();
   });



   /*# AVOID COLLISIONS #*/
})(jQuery);
/*# AVOID COLLISIONS #*/


// jquery metadata

(function($) { $.extend({ metadata: { defaults: { type: 'class', name: 'metadata', cre: /({.*})/, single: 'metadata' }, setType: function(type, name) { this.defaults.type = type; this.defaults.name = name; }, get: function(elem, opts) { var settings = $.extend({}, this.defaults, opts); if (!settings.single.length) settings.single = 'metadata'; var data = $.data(elem, settings.single); if (data) return data; data = "{}"; if (settings.type == "class") { var m = settings.cre.exec(elem.className); if (m) data = m[1]; } else if (settings.type == "elem") { if (!elem.getElementsByTagName) return; var e = elem.getElementsByTagName(settings.name); if (e.length) data = $.trim(e[0].innerHTML); } else if (elem.getAttribute != undefined) { var attr = elem.getAttribute(settings.name); if (attr) data = attr; } if (data.indexOf('{') < 0) data = "{" + data + "}"; data = eval("(" + data + ")"); $.data(elem, settings.single, data); return data; } } }); $.fn.metadata = function(opts) { return $.metadata.get(this[0], opts); }; })(jQuery);