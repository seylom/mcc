﻿<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default"
   EnableViewState="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <title></title>
   <style type="text/css">
      a
      {
         color: #994138;
         text-decoration: none;
         border-bottom: 1px dotted #994138;
      }
      a:hover
      {
         text-decoration: none;
         border-bottom: 1px solid #994138;
      }
   </style>
</head>
<body>
   <form id="form1" runat="server">
   <div>
      <div>
         <a href="http://www.middleclasscrunch.com" style="border: 0 none;">
            <asp:Image runat="server" ID="imgLogo" ImageUrl="~/_assets/images/logo_crunch2.png" /></a>
      </div>
      <h2 style="font-size: 14px; font-weight: bold; margin-top: 10px;">
         This is a server designed for static content at <a title="middleclasscrunch" href="http://www.middleclasscrrunch.com">
            MiddleClassCrunch</a>
         <br />
         (javascript files, css files, and various icons and pictures look for the website
         design.)
      </h2>
   </div>
   </form>
</body>
</html>
