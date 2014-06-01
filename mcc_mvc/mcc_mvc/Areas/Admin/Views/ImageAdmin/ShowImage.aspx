<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage(of AdminImageViewModel)" %>

<asp:Content ContentPlaceHolderID="PageTitle" runat="server">
   View Image
   <%=Html.Encode(Model.Name)%>
</asp:Content>
<asp:Content ContentPlaceHolderID="mainSiteContent" runat="server">
   <table class="FullWidth">
      <tr>
         <td>
            <div>
               <img id="mainImg" src="<%= model.ImageUrl %>" alt="<%= Html.Encode(Model.Name) %>" />
            </div>
            <div style="margin-top:10px;font-size:10px;">
               <table>
                  <tr>
                     <td>
                        Name:
                     </td>
                     <td>
                        <%=Model.Name%>
                     </td>
                  </tr>
                  <tr>
                     <td>
                        ImageUrl:
                     </td>
                     <td>
                        <%=Model.ImageUrl%>
                     </td>
                  </tr>
                  <tr>
                     <td>
                        CreditsName:
                     </td>
                     <td>
                        <%=Model.CreditsName%>
                     </td>
                  </tr>
                  <tr>
                     <td>
                        Credits Url:
                     </td>
                     <td>
                        <%=Model.CreditsUrl%>
                     </td>
                  </tr>
                  <tr>
                     <td>
                        Description:
                     </td>
                     <td>
                        
                     </td>
                  </tr>
               </table>
            </div>
         </td>
         <td>
            <div>
            </div>
         </td>
      </tr>
   </table>
</asp:Content>
