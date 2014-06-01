'Author: © Luis Ramirez 2008
'Web site: http://www.sqlnetframework.com
'Creation date: Feb 29, 2008

Imports System
Imports System.Data
Imports System.Configuration
Imports System.Linq
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Xml.Linq

   Public Class ButtonTextBox
      Inherits TextBox
      Implements IPostBackEventHandler
      Private Shared ReadOnly EventCommand As New Object()

      Public Sub New()
         MyBase.AutoPostBack = True
      End Sub

      Public Property CommandArgument() As String
         Get
            Dim str As String = DirectCast(Me.ViewState("CommandArgument"), String)
            If str IsNot Nothing Then
               Return str
            End If
            Return String.Empty
         End Get
         Set(ByVal value As String)
            Me.ViewState("CommandArgument") = value
         End Set
      End Property

      Public Property CommandName() As String
         Get
            Dim str As String = DirectCast(Me.ViewState("CommandName"), String)
            If str IsNot Nothing Then
               Return str
            End If
            Return String.Empty
         End Get
         Set(ByVal value As String)
            Me.ViewState("CommandName") = value
         End Set
      End Property

#Region "IPostBackEventHandler implementation"
      Private Sub RaisePostBackEvent(ByVal eventArgument As String) Implements IPostBackEventHandler.RaisePostBackEvent
         Me.CommandArgument = MyBase.Text
         Me.RaisePostBackEventMethod(eventArgument)
      End Sub
#End Region

      Protected Overridable Sub OnCommand(ByVal e As CommandEventArgs)
         Dim handler As CommandEventHandler = DirectCast(MyBase.Events(EventCommand), CommandEventHandler)
         If Not handler Is Nothing Then
            'RaiseEvent handler(Me, e)
            handler(Me, e)
         End If

         'It bubbles the event to the HandleEvent method of the GooglePagerField class.
         MyBase.RaiseBubbleEvent(Me, e)
      End Sub

      Protected Overridable Sub RaisePostBackEventMethod(ByVal eventArgument As String)
         If Me.CausesValidation Then
            Me.Page.Validate(Me.ValidationGroup)
         End If
         Me.OnCommand(New CommandEventArgs(Me.CommandName, Me.CommandArgument))
      End Sub
   End Class
