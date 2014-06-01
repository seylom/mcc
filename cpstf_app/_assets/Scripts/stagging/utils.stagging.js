// requires jquery!

(function($) {
   jQuery.fn.center = function() {
      this.css("position", "absolute");
      this.css("top", ($(window).height() - this.height()) / 2 + $(window).scrollTop() + "px");
      this.css("left", ($(window).width() - this.width()) / 2 + $(window).scrollLeft() + "px");
      return this;
   }

})(jQuery);

function serviceProxy(serviceUrl) {
   var _I = this;
   this.serviceUrl = serviceUrl;

   // *** Call a wrapped object
   this.invoke = function(method, data, callback, error, bare) {
      // *** Convert input data into JSON - REQUIRES Json2.js
      var json = JSON.stringify(data);
      // *** The service endpoint URL        
      var url = _I.serviceUrl + method;
      $.ajax({
         url: url,
         data: json,
         type: "POST",
         processData: false,
         contentType: "application/json; charset=utf-8",
         timeout: 10000,
         dataType: "text",  // not "json" we'll parse
         success:
                    function(res) {
                       if (!callback) return;
                       // *** Use json library so we can fix up MS AJAX dates
                       var result = JSON.parse(res);
                       // *** Bare message IS result
                       if (bare)
                       { callback(result); return; }
                       // *** Wrapped message contains top level object node
                       // *** strip it off
                       for (var property in result) {
                          callback(result[property]);
                          break;
                       }
                    },
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