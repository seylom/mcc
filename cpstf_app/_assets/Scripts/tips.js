﻿var Tips=function(){var n=[];var o=-1;var p=false;var q=false;var r;var s=0;var t=$("#tp-items");var u=$("#items");var v=[];var x=[];var y=null;function Setup(){t.children().each(function(a,b){var c=b.id.substr("item_".length,b.id.length);J(c);n.push(b)});t.empty();if(n.length>0){if(o==-1){E()}if(o>0){for(var i=0;i<o;i++){div=document.createElement("div");var d="col_"+i;$(div).attr("id",d).addClass("tip-col");u.append(div)}}z()}window.onresize=F};var z=function(){if((n.length>0)&&(s<n.length)){var a=C();var b=n[s];if(b){s+=1;var c=$(a);$(b).addClass("tip").hover(function(){$(this).css("background-color","#fffde8")},function(){$(this).css("background-color","#ffffff")}).appendTo(c).animate({"backgroundColor":"#ffffff","opacity":"1"},"fast",z)}if(s==n.length-1){q=true;R();H()}}};var A=function(){if(n.length>0){for(var j=0;j<n.length;j++){var a=n[j];B(a)}r=true}};var B=function(a){var b=C();if(b){var c=$(b);$(a).addClass("tip").hover(function(){$(this).css("background-color","#fffde8")},function(){$(this).css("background-color","#ffffff")}).appendTo(c).animate({"opacity":"1"},"slow")}};var C=function(){var a,cols=$(".tip-col");if(cols){for(var i=0;i<cols.length;i++){if(a){if($(cols[i]).height()<$(a).height())a=$(cols[i])}else{a=$(cols[i])}}}return a}var D=function(){var a,cols=$(".tip-col");if(cols){for(var i=0;i<cols.length;i++){if(a){if($(cols[i]).height()>$(a).height())a=$(cols[i])}else{a=$(cols[i])}}}return a};var E=function(){var w=u.width();o=Math.floor(w/220);return o};var F=function(a){if(r){G()}};var G=function(){var c=o;E();if(c>o){var d=o-c;var e=[];for(var i=o;i<c;i++){var f="col_"+i;$("div[id="+f+"]").children().each(function(a,b){e.push(b)})$("div[id="+f+"]").remove()}for(var j=0;j<e.length;j++){B(e[j])}}else if(c<o){for(var k=0;k<o-c;k++){div=document.createElement("div");var g=c+k;g="col_"+g;$(div).attr("id",g).addClass("tip-col");$("#items > .clearer").before($(div))}A()}};var H=function(){$(function(){$("a[id^='voteuplnk_']").unbind('click').click(function(){var a=$(this).attr('id').substr('voteuplnk_'.length);K(a,1)});$("a[id^='votedownlnk_']").unbind('click').click(function(){var a=$(this).attr('id').substr('votedownlnk_'.length);K(a,0)})})};var I=function(a){var b=mccUtils.Tools.Get_Cookie("mcc_adv_votelist");if(b){if(v.length==0){var c=unescape(b);v=c.split(";")}for(var i=0;i<v.length;i++){if(v[i]==a){return true}}}return false};var J=function(a){var b=mccUtils.Tools.Get_Cookie("mcc_adv_votelist");if(b){if(v.length==0){var c=unescape(b);v=c.split(";")}for(var i=0;i<v.length;i++){if(v[i]==a){$("#vote_link_"+a).replaceWith("<span id='"+"vote_link_"+a+"' style='color:#bababa'>Vote</span>");$("#votes_"+a).hide();$("#result_link_"+a).replaceWith("<span id='"+"result_link_"+a+"' style='color:#bababa'>Results</span>");$("#result_"+a).show();return}}$("#vote_link_"+a).replaceWith("<span id='"+"vote_link_"+a+"' style='color:#bababa'>Vote</span>")}};var K=function(a,b){var c=mccUtils.Tools.Get_Cookie("mcc_adv_votelist");if(c){if(v.length==0){var d=unescape(c)v=d.split(";")}for(var i=0;i<v.length;i++){if(v[i]==a){$("#notify_"+a).html('vote already submitted!');$("#notifybox_"+a).show();return}}}var e={'value':b};y.invoke(a+'/vote',e,L,function(){alert('Unable to save your vote at this time.')})};var L=function(a){var b=a;if(b.success==true){v.push(b.id);mccUtils.Tools.Set_Cookie("mcc_adv_votelist",v.join(";"));$("#notify_"+b.id).html("Thank you for your vote!");$("#vote_link_"+b.id).replaceWith("<span id='"+"vote_link_"+b.id+"' style='color:#bababa'>Vote</span>");$("#votes_"+b.id).fadeOut("fast",function(){$("#notifybox_"+b.id).toggle().animate({"backgroundColor":"#fefefe"},"slow",function(){O(b.id,b.up,b.down,true)})})}else{$("#notify_"+b.id).html('We are unable to process you vote at this time.Please try again later.');$("#notifybox_"+b.id).show()}};var M=function(a){var b="#votes_"+a;var c="#result_"+a;$("#vote_link_"+a).replaceWith("<span id='"+"vote_link_"+a+"' style='color:#bababa'>Vote</span>");if(!I(a)){$("#result_link_"+a).replaceWith("<a id='"+"result_link_"+a+"' href='javascript:void(0);' class='global' onclick='showResults("+a+")'>Results</a>")}$(c).fadeOut("fast",function(){$(b).fadeIn("fast")})};var N=function(a,b){var c="#result_"+a;var d="#votes_"+a;if(!I(a)){$("#vote_link_"+a).replaceWith("<a id='"+"vote_link_"+a+"' href='javascript:void(0);' class='global' onclick='showVoteLinks("+a+")'>Vote</a>")}$("#result_link_"+a).replaceWith("<span id='"+"result_link_"+a+"' style='color:#bababa'>Results</span>");$(d).fadeOut("fast",function(){$(c).fadeIn("fast",P(a,false))})};var O=function(a,b,c,d){var e="#result_"+a;var f="#votes_"+a;if(!I(a)){$("#vote_link_"+a).replaceWith("<a id='"+"vote_link_"+a+"' href='javascript:void(0);' class='global' onclick='showVoteLinks("+a+")'>Vote</a>")}$("#result_link_"+a).replaceWith("<span id='"+"result_link_"+a+"' style='color:#bababa'>Results</span>");$(f).fadeOut("fast",function(){$(e).fadeIn("fast",Q(a,b,c,d))})};var P=function(j,k){var l=false;for(var i=0;i<x.length;i++){if(x[i][0]==j){Q(j,x[i][1],x[i][2],k);return}}var m='/tips/'+j+'/getvotevalues'$.get(m,function(a){var b=a.up;var c=a.down;var d=(b*100)/(b+c)var e=(c*100)/(b+c)var f=[j,d,e];x.push(f);var g="#votebar_yes_"+j;var h="#votebar_no_"+j;if(k==true){$(g).css("width","0px");$(h).css("width","0px");$(g).animate({"width":d+"%"},"slow");$(h).animate({"width":e+"%"},"slow")}else{$(g).css("width",d+"%");$(h).css("width",e+"%")}})};var Q=function(a,b,c,d){var e=b+c;var f=(e>0)?(b*100)/e:0;var g=(e>0)?(c*100)/e:0;var h="#votebar_yes_"+a;var i="#votebar_no_"+a;var j="("+Math.round(f)+"%, "+b+" votes)";var k="("+Math.round(g)+"%, "+c+" votes)";$("#votetext_yes_"+a).empty().html(j);$("#votetext_no_"+a).html(k);if(d==true){$(h).css("width","0px");$(i).css("width","0px");$(h).animate({"width":f+"%"},"slow");$(i).animate({"width":g+"%"},"slow")}else{$(h).css("width",f+"%");$(i).css("width",g+"%")}};var R=function(){var a=mccUtils.Tools.Get_Cookie("mcc_adv_votelist");if(a){if(v.length==0){var b=unescape(a)v=b.split(";")}for(var i=0;i<v.length;i++){P(v[i],true)}}};return{Init:function(a){Setup();y=new serviceProxy(a)}}}();