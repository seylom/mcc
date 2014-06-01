<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Site/Views/Shared/TwoCol.master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ContentPlaceHolderID="PageTitle" runat="server">
Help
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="middlePlaceHolder" runat="Server">
   <div>
      <p style="margin-bottom: 20px;">
         To help you learn a little more about Middle Class Crunch website, we have added
         below a couple of links and descriptions to help you find the information you are
         looking for.
      </p>
      <span style="margin-top: 10px;">In this section: </span>
      <ul id="help-list" style="padding-left: 20px;">
         <li><a class="global" id="content_organization" href="#content_organization">How is
            the content organized on the website?</a></li>
         <li><a class="global" id="submit_content" href="#submit_content">How do i submit content
            on the website?</a></li>
         <li><a class="global" id="become_contributor" href="#become_contributor">How do i become
            a direct contributor to Middle Class Crunch?</a></li>
         <li><a class="global" id="correct_data" href="#correct_data">How to help make the website
            content more accurate or request additionnal details?</a></li>
         <li><a class="global" id="bug_report" href="#bug_report">How can i report a bug in
            the website?</a></li>
         <li><a class="global" id="search_pb" href="#search_pb">I can't find a topic using the
            search. What am i doing wrong?</a></li>
      </ul>
      <ul id="siteinfo-list">
         <li class="stli">
            <h4>
               How is the content organized on the website?</h4>
            <a href="#" rel="content_organization"></a>
            <p>
               The site is made of several sections that can be browse seemlessly using the top
               menu or the breadcrums. Some of the main sections are <a id="A1" class="global" href="/articles/">
                  articles</a>,<a class="global" id="A2" href="/videos/">videos</a>, <a class="global"
                     id="A3" href="/tools/">tools</a>, <a class="global" id="A4" href="/stories/">stories</a>,
               and <a class="global" href="/community/">community</a>
            </p>
            <p>
               Each of these sections contains information on a large variety of topics. Some sections
               may share information or use other section content to create a new one. Don't forget
               to use the searh functionnality if at any moment you feel lost or don't understand
               the organization of the website. You can also take a look at the <a id="A6" class="global"
                  href="/sitemap">sitemap</a> to have a quick overview of the website sections.
            </p>
         </li>
         <li class="stli">
            <h4>
               How do i submit content on the website?</h4>
            <a href="#" rel="submit_content"></a>
            <p>
               There are different ways of submitting content on Middle Class Crunch. you can post
               comments - ask questions or start discussion on the forum or you can submit an article
               to Middle Class Crunch by using the <a class="global" href="/submitContent">submit
                  an article</a> page.The website team will review your article before submitting
               it.
            </p>
         </li>
         <li class="stli">
            <h4>
               How do i become a direct contributor to Middle Class Crunch?</h4>
            <a href="#" rel="become_contributor"></a>
            <p>
               "A direct contributor is a person who is allowed to post content on a website and/or
               have an active role in the life of the website." With such a definition, you can
               understand that before allowing you to become a contributor not only will you need
               to submit a certain amount of articles you would have written yourself, but also
               the content of the article will need to be related to the website general content,
               not violate any of the <a id="A8" class="global" href="/termsofuse">terms of use</a>
               and be reviewed before posting, to preserve the integrity of the website and its
               users.
            </p>
            <p>
               You can also contribute on the website by <a id="A9" href="/submitcontent" class="global">
                  submitting content</a>.
            </p>
         </li>
         <li class="stli">
            <h4>
               How to help make the website content more accurate or request additionnal details?</h4>
            <a href="#" rel="correct_data"></a>
            <p>
               You can use the <a id="A10" href="/contact" class="global">contact us</a> us page
               to submit request to the MiddleClassCrunch Team. Add a good description of your
               request in order for us to be able to address your request fast and accurately.
               The Author of the article will likely be the one to get back to you. Don't forget
               to leave a contact information to make it possible.
            </p>
         </li>
         <li class="stli">
            <h4>
               How can i report a bug in the website?</h4>
            <a href="#" rel="bug_report"></a>
            <p>
               we create a page which sole purpose is to report bugs found during the use of the
               website. please make sure that Javascript is enable in order to be able to view
               the website pages. If you choose to disable javascript on your browser, then this
               website might not be for you. If you encounter any layout problem, please send us
               a request with a couple information about your browser. As we are writting these
               line, this website has been tested in IE6 - IE7, Firefox 3, Safari 3, Opera and
               Netscape
            </p>
         </li>
         <li class="stli">
            <h4 id="search_pbTitle">
               I can't find a topic using the search. What am i doing wrong?</h4>
            <a href="#" rel="search_pb"></a>
            <p>
               Either the topic doesn't exist or has been removed from our database. A few other
               reasons might include search keyword or tags not used correcly or a Site search
               under maintenance. If you think that it is just the system that is not robust enough
               for the kind of search result you would like to see listed, please give us a <a class="global"
                  href="/feedback">feedback</a> with a description of what you would like to see,
               or have it look like
            </p>
         </li>
      </ul>
   </div>
   <span>- <i>the MiddleClassCrunch Team</i> -</span>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="rightPlaceHolder" runat="Server">
   <div class="widget">
      <h2 class="title">
         Related Topics</h2>
      <div style="padding: 10px 0 10px 10px;">
         <ul class="category-menu">
            <li class="bullet" onmouseover="this.className='bullet_hover'" onmouseout="this.className='bullet'">
               <a href="/help">Help</a></li>
            <li class="bullet" onmouseover="this.className='bullet_hover'" onmouseout="this.className='bullet'">
               <a href="/information/termsofuse">Terms & Privacy</a></li>
            <li class="bullet" onmouseover="this.className='bullet_hover'" onmouseout="this.className='bullet'">
               <a href="/contact">Contact Us</a></li>
            <li class="bullet" onmouseover="this.className='bullet_hover'" onmouseout="this.className='bullet'">
               <a href="/reportIssues">Report a bug</a></li>
         </ul>
      </div>
   </div>
</asp:Content>
