<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Site/Views/Shared/singlewrapper.master"
   Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content2" ContentPlaceHolderID="PageTitle" runat="server">
   Iron Man 2 Contest!
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="middlePlaceHolder" runat="server">
   <div style="padding: 10px; background-color: #fff;">
      <center>
         <h2 style="font-size: 18px; margin-bottom: 10px;">
            FuTurXTV & MIDDLECLASSCRUNCH.COM</h2>
         <h2 style="font-size: 18px; margin-bottom: 20px;">
            IRON MAN 2 CONTEST
         </h2>
         
         <div style="margin: 20px 0;">
            <img src="<%= appHelpers.imageUrl("ads/ironman2poster.jpg") %>" alt="ironman2 pic1" />
         </div>
      </center>
      <p style="margin: 20px 0; font-size: 14px;">
         Want to win tickets to see Iron Man 2? <b style="color:#994138">Submit a short video of an original rap all
         about Iron Man for your chance to win!</b> 5 winners will be chosen to receive a pair
         of tickets to an advance screening of Iron Man 2 on May 5th. One GRAND PRIZE winner
         will also be chosen to receive an Iron Man 2 XBOX 360 game from SEGA.
      </p>
      <center>
         <div style="margin: 20px 0;">
            <img src="<%=appHelpers.imageUrl("ads/ironman2segagame.jpg") %>" alt="ironman2 pic2" />
         </div>
      </center>
      <p style="margin: 20px 0; font-size: 14px;">
         E-mail us your video link with Iron Man 2 Contest in the subject line and the following
         information to <b>ironman2contest@gmail.com</b>
      </p>
      <p style="margin: 20px 0; font-size: 14px;">
         - Your full name<br />
         - Full address<br />
         - E-mail address<br />
      </p>
      <p style="margin: 20px 0; font-size: 14px;">
         Winners chosen at random on May 3rd, 2010 and notified by e-mail. Check out Iron
         Man 2 at the <a href="http://www.Ironmanmovie.com" class="globalred">official site</a>
         for more. SEGA is registered in the U.S. Patent and Trademark Office. SEGA and the
         SEGA logo are either registered trademarks or trademarks of SEGA Corporation. ©
         SEGA. All rights reserved. <a class="globalred" href="http://www.ironman2game.com">
            www.ironman2game.com</a>. Iron Man 2 is in theatres May 7th. <a href="http://www.ironmanmovie.com"
               class="globalred">Ironmanmovie.com</a>
      </p>
   </div>
</asp:Content>
