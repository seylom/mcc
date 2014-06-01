Imports System
Imports System.Data
Imports System.Configuration
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls

''' <summary>
''' Summary description for FixedHyperLink
''' </summary>
Public Class FixedHyperLink
    Inherits HyperLink
    Protected Overloads Overrides Sub RenderContents(writer As HtmlTextWriter)
        MyBase.RenderContents(writer)
    End Sub
End Class
