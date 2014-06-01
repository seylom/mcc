Imports System
Imports System.Data
Imports System.Configuration
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.Adapters
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls

Namespace MccHyperlink
   ''' <summary>
   ''' Summary description for HyperLinkControlAdapter
   ''' </summary>
   Public Class HyperLinkControlAdapter
      Inherits ControlAdapter
      Protected Overloads Overrides Sub Render(ByVal writer As HtmlTextWriter)
         Dim hl As HyperLink = TryCast(Me.Control, HyperLink)
         If hl Is Nothing Then
            MyBase.Render(writer)
            Return
         End If

         ' This code is copied from HyperLink.RenderContents (using
         ' Reflector). References to "this" have been changed to
         ' "hl", and we have to render the begin and end tags.
         Dim imageUrl As String = hl.ImageUrl
         If imageUrl.Length > 0 Then
            ' Let the HyperLink render its begin tag
            hl.RenderBeginTag(writer)

            Dim image As New Image()

            ' I think the next line is the bug. The URL gets
            ' resolved here, but the Image.UrlResolved property
            ' doesn't get set. So another attempt to resolve the
            ' URL is made in Image.AddAttributesToRender. It's in
            ' the callstack above that method that the exception
            ' or improperly resolved URL happens.

            'image.ImageUrl = MyBase.ResolveClientUrl(imageUrl)
            image.ImageUrl = imageUrl

            imageUrl = hl.ToolTip
            If imageUrl.Length <> 0 Then
               image.ToolTip = imageUrl
            End If

            imageUrl = hl.Text
            If imageUrl.Length <> 0 Then
               image.AlternateText = imageUrl
            End If

            image.RenderControl(writer)

            ' Wrap up by letting the HyperLink render its end tag
            hl.RenderEndTag(writer)
         Else
            ' HyperLink.RenderContents handles a couple of other
            ' cases if its ImageUrl property hasn't been set. We
            ' delegate to that behavior here.
            MyBase.Render(writer)
         End If
      End Sub
   End Class
End Namespace
