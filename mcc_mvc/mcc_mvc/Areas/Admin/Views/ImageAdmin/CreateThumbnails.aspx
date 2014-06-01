<%@ Page Title="" Language="VB" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage(of CreateThumbsViewModel)" %>

<asp:Content ID="Content4" ContentPlaceHolderID="PageTitle" runat="server">
Create Thumbnails
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="headerPlaceHolder" runat="Server">
   <%=appHelpers.CssTagUrl("jquery.Jcrop.css")%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainSiteContent" runat="Server">
   <% Using Html.BeginForm(New With {.ImageID = Model.ImageID})%>
   <input type="hidden" id="ImageId" name="ImageId" value="<%= Model.ImageID %>" />
   <input type="hidden" id="X_Mini" name="X_Mini" value="<%= Model.X_Mini %>" />
   <input type="hidden" id="Y_Mini" name="Y_Mini" value="<%= Model.Y_Mini %>" />
   <input type="hidden" id="W_Mini" name="W_Mini" value="<%= Model.W_Mini %>" />
   <input type="hidden" id="H_Mini" name="H_Mini" value="<%= Model.H_Mini %>" />
   <input type="hidden" id="X_Long" name="X_Long" value="<%= Model.X_Long %>" />
   <input type="hidden" id="Y_Long" name="Y_Long" value="<%= Model.Y_Long %>" />
   <input type="hidden" id="W_Long" name="W_Long" value="<%= Model.W_Long %>" />
   <input type="hidden" id="H_Long" name="H_Long" value="<%= Model.H_Long %>" />
   <input type="hidden" id="X_Large" name="X_Large" value="<%= Model.X_Large %>" />
   <input type="hidden" id="Y_Large" name="Y_Large" value="<%= Model.Y_Large %>" />
   <input type="hidden" id="W_Large" name="W_Large" value="<%= Model.W_Large %>" />
   <input type="hidden" id="H_Large" name="H_Large" value="<%= Model.H_Large %>" />

   <div style="padding: 10px; background-color: #fef6dc;">
      <h2 style="font-size: 11px; font-weight: bold;">
         Image Info - (<a href="<%= url.Action("UpdateImage",new with {.Id = Model.ImageID}) %>" id="lnkUpdateImage" class="globalred">Replace Image</a>)
      </h2>
      <table width="100%" style="margin: 10px 0 10px 10px;">
         <tr>
            <td align="right" style="width: 100px;">
               <b>Name:</b>
            </td>
            <td>
               <i>[<%=Model.Name%>]</i>
            </td>
         </tr>
         <tr>
            <td align="right">
               <b>uuid:</b>
            </td>
            <td>
               <i>[<%=Model.Uuid%>]</i>
            </td>
         </tr>
         <tr>
            <td align="right">
               <b>Tags:</b>
            </td>
            <td>
               <%=Model.Tags%>
            </td>
         </tr>
         <tr>
            <td align="right">
               <b>Credits Name:</b>
            </td>
            <td>
               <%=Model.CreditsName%>
            </td>
         </tr>
         <tr>
            <td align="right">
               <b>Credits Url:</b>
            </td>
            <td>
               <%=Model.CreditsUrl%>
            </td>
         </tr>
      </table>
      <%=Html.DisplayError()%>
      <div id="cb_options" style="margin: 20px 10px;">
         <div>
            <%=Html.CheckBox("CreateLarge", Model.CreateLarge)%>
            <span>thumb - (400x200)</span>
         </div>
         <div>
            <%=Html.CheckBox("CreateMini", Model.CreateMini)%>
            <span>thumb - (80x80)</span>
         </div>
         <div>
            <%=Html.CheckBox("CreateLong", Model.CreateLong)%>
            <span>thumb - (250x100)</span>
         </div>
      </div>
      <div class="jcrop-holder">
         <img id="imgBase" style="border: 4px solid #bababa; background-color: #eaeaea;" alt=""
            src="<%= Model.BaseImageUrl %>" />
      </div>
      <div style="margin-top: 20px;">
         <h2 style="font-size: 11px; font-weight: bold;">
            Thumbnail creation: auto
         </h2>
         <div style="overflow: hidden; padding: 10px 0; border-top: 1px dashed #bababa; border-bottom: 1px dashed #bababa;
            margin: 10px 0">
            <div id="pn_400" style="float: left; background-color: #eaeaea; width: 400px; height: 200px;
               border: 4px solid #bababa; position: relative; overflow: hidden;">
               <img id="ip_400_200" alt="250x100 preview" src="<%= Model.LargeImageUrl %>" />
               <span style="position: absolute; padding: 3px; background-color: #daffd6; color: #194913;
                  bottom: 0; right: 0;">(<a href="javascript:void(0);" onclick="setCurrentId('ip_400_200')">400x200</a>)
               </span>
            </div>
            <div style="float: left; margin-left: 20px">
               <div id="pn_80" style="width: 80px; height: 80px; border: 4px solid #bababa; background-color: #eaeaea;
                  margin-bottom: 12px; position: relative; overflow: hidden;">
                  <img id="ip_80_80" alt="80x80 preview" src="<%= Model.MiniImageUrl %>" />
                  <span style="position: absolute; padding: 3px; background-color: #daffd6; color: #194913;
                     bottom: 0; right: 0;">(<a href="javascript:void(0);" onclick="setCurrentId('ip_80_80')">80x80</a>)
                  </span>
               </div>
               <div id="pn_250" style="width: 250px; height: 100px; border: 4px solid #bababa; background-color: #eaeaea;
                  overflow: hidden; position: relative;">
                  <img id="ip_250_100" alt="250x100 preview" src="<%= Model.LongImageUrl %>" />
                  <span style="position: absolute; padding: 3px; background-color: #daffd6; color: #194913;
                     bottom: 0; right: 0;">(<a href="javascript:void(0);" onclick="setCurrentId('ip_250_100')">250x100</a>)
                  </span>
               </div>
            </div>
         </div>
         <div>
            <div id="pn_200" style="background-color: #eaeaea; width: 200px; height: 100px; border: 4px solid #bababa;
               position: relative; overflow: hidden;">
               <img id="ip_200_100" alt="200x100 preview" src="" />
               <span style="position: absolute; padding: 3px; background-color: #daffd6; color: #194913;
                  bottom: 0; right: 0;">(<a href="javascript:void(0);" onclick="setCurrentId('ip_200_100')">200x100</a>)
               </span>
            </div>
         </div>
      </div>
      <div style="margin-top: 10px;">
         <input type="submit" value="Update" />
      </div>
   </div>
   <% End Using%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottomScripts" runat="Server">
  <%-- <%=appHelpers.ScriptsTagUrl("jquery/jquery.blockUI.js")%>
   <%=appHelpers.ScriptsTagUrl("jquery/jquery.metadata.min.js")%>
   <%=appHelpers.ScriptsTagUrl("jquery/jquery.MultiFile.pack.js")%>--%>
   <%=appHelpers.ScriptsTagUrl("jquery/jquery.Jcrop.min.js")%>

   <script type="text/javascript">

      var jcrop_api;
      var _imgId = '#imgBase';
      var _imgWidth = $(_imgId).width();
      var _imgHeight = $(_imgId).height();


      $(function() {
         InitJcrop();
      })

      function InitJcrop() {
         jcrop_api = $.Jcrop(_imgId, {
            onChange: showPreview,
            onSelect: showPreview
         });
      }

      function setCurrentId(id) {
         if (_currentId == id) {
            _currentId = '';
         }
         else {
            _currentId = id;
            displayThumbBox();
         }
      }

      var _currentId;

      function showPreview(coords) {

         if (_currentId !== '') {
            if (parseInt(coords.w) > 0) {
               var rx = 1;
               var ry = 1;

               jQuery('#' + _currentId).css({
                  width: Math.round(rx * _imgWidth) + 'px',
                  height: Math.round(ry * _imgHeight) + 'px',
                  marginLeft: '-' + Math.round(rx * coords.x) + 'px',
                  marginTop: '-' + Math.round(ry * coords.y) + 'px'
               });


               var f_coords = [coords.x, coords.y, coords.x2, coords.y2];
               switch (_currentId) {
                  case 'ip_400_200':

                     $('#X_Large').val(coords.x);
                     $('#Y_Large').val(coords.y);
                     $('#W_Large').val(coords.w);
                     $('#H_Large').val(coords.h);

                     thumb_400_200.box_coords = f_coords;
                     break;
                  case 'ip_80_80':

                     $('#X_Mini').val(coords.x);
                     $('#Y_Mini').val(coords.y);
                     $('#W_Mini').val(coords.w);
                     $('#H_Mini').val(coords.h);

                     thumb_80_80.box_coords = f_coords;
                     break;
                  case 'ip_250_100':

                     $('#X_Long').val(coords.x);
                     $('#Y_Long').val(coords.y);
                     $('#W_Long').val(coords.w);
                     $('#H_Long').val(coords.h);

                     thumb_250_100.box_coords = f_coords;
                     break;

               }
            }
         }
      }

      function thumb(id, width, height) {

         this.id = id;
         this.width = width;
         this.height = height;

         this.box_coords = [(_imgWidth - width) / 2, (_imgHeight - height) / 2, (_imgWidth + width) / 2, (_imgHeight + height) / 2];
      }


      function displayThumbBox() {
         var coord = [];
         switch (_currentId) {
            case 'ip_400_200':
               coords = thumb_400_200.box_coords;
               break;
            case 'ip_80_80':
               coords = thumb_80_80.box_coords;
               break;
            case 'ip_250_100':
               coords = thumb_250_100.box_coords;
               break;
            case 'ip_200_100':
               coords = thumb_200_100.box_coords;
               break;
         }
         jcrop_api.animateTo(coords);
      }

      thumb_400_200 = new thumb('ip_400_200', 400, 200);
      thumb_80_80 = new thumb('ip_80_80', 80, 80);
      thumb_250_100 = new thumb('ip_250_100', 250, 100);
      thumb_200_100 = new thumb('ip_200_100', 200, 100);

   </script>

</asp:Content>
