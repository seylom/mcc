<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Site/Views/Shared/TwoCol.master" Inherits="System.Web.Mvc.ViewPage(of ProfileCommon)" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
Profile View
</asp:Content>
<asp:Content ContentPlaceHolderID="middlePlaceHolder" runat="server">
   <div style="margin-bottom: 5px;">
      <h3 style="color: #393737; border-bottom: 1px solid #bababa; padding: 3px 0; font-size: 18px;
         font-weight: normal;">
         <%=ViewData.Model.UserName%>'s Profile!</h3>
      <div style="margin-top: 10px;">
         <table width="100%">
            <col align="left" valign="top" />
            <col style="width: 200px;" align="left" valign="top" />
            <tbody>
               <tr>
                  <td valign="top">
                     <div style="overflow: hidden;">
                        <img style="width: 80px; height: 80px;" src='<%= mcc.routines.Gravatar(ViewData("Email"),80) %>'
                           alt="avatar" />
                        <table cellpadding="4">
                           <tr>
                              <td align="left">
                                 <b style="margin-left: 10px;">Website:</b>
                              </td>
                              <td style="padding-left: 10px;" align="left">
                                 <a href="<%="Http://" & ViewData.Model.Website%>" title="website">Website</a>
                              </td>
                           </tr>
                           <tr>
                              <td colspan="2">
                                 <div style="margin-top: 5px; padding: 5px 10px;">
                                    <%=Utils.ConvertBBCodeToHTML(ViewData.Model.About)%>
                                 </div>
                              </td>
                           </tr>
                        </table>
                     </div>
                  </td>
                  <td valign="top">
                  </td>
               </tr>
            </tbody>
         </table>
      </div>
      
      <div style="margin-top:10px;">
      
      </div>
   </div>
</asp:Content>
