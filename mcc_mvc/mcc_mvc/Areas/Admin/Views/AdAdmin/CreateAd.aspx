<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage(of Ad)" %>

<%@ Import Namespace="MCC.Services" %>
<asp:Content ID="Content2" ContentPlaceHolderID="PageTitle" runat="server">
Create Ads
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="mainsitecontent" runat="server">
   <h2>
      Create / Edit Ad</h2>
   <div style="margin-top: 10px; padding: 5px 10px; background-color: #f1f1f1">
      <h2 style="font-size: 10px; font-weight: bold; margin-bottom: 5px;">
         <a href='javascript:void(0);' class="global" onclick="InfoSetup();return false;">Article
            information</a></h2>
      <div id="info_window">
      
      <%=Html.DisplayError()%>
      
      <% Using Html.BeginForm("CreateAd", "AdAdmin", New With {.Id = Model.AdID})%>
         <table class="edit-info">
            <tr>
               <td class="right" style="width: 100px;">
                  Title:
               </td>
               <td class="left">
                  <%=Html.TextBox("Title", Model.Title, New With {.class = "rtc", .style = "width:90%"})%>
               </td>
            </tr>
            <tr>
               <td class="right" style="width: 100px;">
                  Type:
               </td>
               <td class="left">
               
                  <%=Html.DropDownList("TypeId", CType(Model.Type, adType).ToSelectList())%>
               
                <%--  <asp:DropDownList runat="server" ID="ddlType">
                     <asp:ListItem Text="Undefined" Value="0"></asp:ListItem>
                     <asp:ListItem Text="Banner(Vertical)" Value="1"></asp:ListItem>
                     <asp:ListItem Text="Banner(Vertical)" Value="2"></asp:ListItem>
                     <asp:ListItem Text="Text Onlly" Value="3"></asp:ListItem>
                     <asp:ListItem Text="Image(Vertical)" Value="4"></asp:ListItem>
                     <asp:ListItem Text="Image(Horizontal)" Value="5"></asp:ListItem>
                     <asp:ListItem Text="Widget(Vertical)" Value="6"></asp:ListItem>
                     <asp:ListItem Text="Widget(Horizontal)" Value="7"></asp:ListItem>
                     <asp:ListItem Text="GoogleAds" Value="8"></asp:ListItem>
                  </asp:DropDownList>      --%>
               </td>
            </tr>
            <tr>
               <td class="right">
                  Description:
               </td>
               <td class="left">
                 <%=Html.TextArea("Description", Model.Description, New With {.class = "rtc", .rows = "2", .style = "width:90%"})%>
                </td>
            </tr>
            <tr>
               <td class="right">
                  Body:
               </td>
               <td class="left">
                  <%=Html.TextArea("Body", Model.Body, New With {.class = "rtc", .rows = "2", .style = "width:90%"})%>
               </td>
            </tr>
            <tr>
               <td class="right">
                  Keywords:
               </td>
               <td class="left">
                   <%=Html.TextBox("Keywords", Model.keywords, New With {.class = "rtc", .style = "width:90%"})%>
               </td>
            </tr>
            <tr>
               <td colspan="2">
                  <input type="submit" class="rb-btn rb-insert" onmouseover="this.className='rb-btn rb-insert-hover'"
                      value="" onmouseout="this.className='rb-btn rb-insert'" />
                  <input type="button" class="rb-btn rb-cancel" onmouseover="this.className='rb-btn rb-cancel-hover'"
                      value="" onmouseout="this.className='rb-btn rb-cancel'" />
               </td>
            </tr>
         </table>
         <% End Using%>
      </div>
   </div>
</asp:Content>
