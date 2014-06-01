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
Imports System.Globalization


''' <summary>
''' Summary description for GoogleDataPager
''' </summary>
Public Class AdvancedPagerField
   Inherits DataPagerField
   Private _startRowIndex As Integer
   Private _maximumRows As Integer
   Private _totalRowCount As Integer

   'Next and previous buttons by default are always enabled.
   Private _showPreviousPage As Boolean = True
   Private _showNextPage As Boolean = True

   Public Sub New()
   End Sub

#Region "Properties"
   Public Property NextPageText() As String
      Get
         Dim obj2 As Object = MyBase.ViewState("NextPageText")
         If obj2 IsNot Nothing Then
            Return DirectCast(obj2, String)
         End If
         Return "Next"
      End Get
      Set(ByVal value As String)
         If value <> Me.NextPageText Then
            MyBase.ViewState("NextPageText") = value
            Me.OnFieldChanged()
         End If
      End Set
   End Property

   Public Property PreviousPageText() As String
      Get
         Dim obj2 As Object = MyBase.ViewState("PreviousPageText")
         If obj2 IsNot Nothing Then
            Return DirectCast(obj2, String)
         End If

         Return "Previous"
      End Get
      Set(ByVal value As String)
         If value <> Me.PreviousPageText Then
            MyBase.ViewState("PreviousPageText") = value
            Me.OnFieldChanged()
         End If
      End Set
   End Property

   Public Property NextPageImageUrl() As String
      Get
         Dim obj2 As Object = MyBase.ViewState("NextPageImageUrl")
         If obj2 IsNot Nothing Then
            Return DirectCast(obj2, String)
         End If
         Return String.Empty
      End Get
      Set(ByVal value As String)
         If value <> Me.NextPageImageUrl Then
            MyBase.ViewState("NextPageImageUrl") = value
            Me.OnFieldChanged()
         End If
      End Set
   End Property

   Public Property PreviousPageImageUrl() As String
      Get
         Dim obj2 As Object = MyBase.ViewState("PreviousPageImageUrl")
         If obj2 IsNot Nothing Then
            Return DirectCast(obj2, String)
         End If
         Return String.Empty
      End Get
      Set(ByVal value As String)
         If value <> Me.PreviousPageImageUrl Then
            MyBase.ViewState("PreviousPageImageUrl") = value
            Me.OnFieldChanged()
         End If
      End Set
   End Property

   Public Property RenderNonBreakingSpacesBetweenControls() As Boolean
      Get
         Dim obj2 As Object = MyBase.ViewState("RenderNonBreakingSpacesBetweenControls")
         If obj2 IsNot Nothing Then
            Return CBool(obj2)
         End If
         Return True
      End Get
      Set(ByVal value As Boolean)
         If value <> Me.RenderNonBreakingSpacesBetweenControls Then
            MyBase.ViewState("RenderNonBreakingSpacesBetweenControls") = value
            Me.OnFieldChanged()
         End If
      End Set
   End Property

   <CssClassProperty()> _
   Public Property ButtonCssClass() As String
      Get
         Dim obj2 As Object = MyBase.ViewState("ButtonCssClass")
         If obj2 IsNot Nothing Then
            Return DirectCast(obj2, String)
         End If
         Return String.Empty
      End Get
      Set(ByVal value As String)
         If value <> Me.ButtonCssClass Then
            MyBase.ViewState("ButtonCssClass") = value
            Me.OnFieldChanged()
         End If
      End Set
   End Property

   Private ReadOnly Property EnablePreviousPage() As Boolean
      Get
         Return (Me._startRowIndex > 0)
      End Get
   End Property

   Private ReadOnly Property EnableNextPage() As Boolean
      Get
         Return ((Me._startRowIndex + Me._maximumRows) < Me._totalRowCount)
      End Get
   End Property
#End Region

   Public Overloads Overrides Sub CreateDataPagers(ByVal container As DataPagerFieldItem, ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal totalRowCount As Integer, ByVal fieldIndex As Integer)
      Me._startRowIndex = startRowIndex
      Me._maximumRows = maximumRows
      Me._totalRowCount = totalRowCount

      If String.IsNullOrEmpty(MyBase.DataPager.QueryStringField) Then
         Me.CreateDataPagersForCommand(container, fieldIndex)
      Else
         Me.CreateDataPagersForQueryString(container, fieldIndex)
      End If
   End Sub

   Protected Overloads Overrides Function CreateField() As DataPagerField
      Return New AdvancedPagerField()
   End Function

   Public Overloads Overrides Sub HandleEvent(ByVal e As CommandEventArgs)
      If String.Equals(e.CommandName, "UpdatePageSize") Then
         MyBase.DataPager.PageSize = Int32.Parse(e.CommandArgument.ToString())
         MyBase.DataPager.SetPageProperties(Me._startRowIndex, MyBase.DataPager.PageSize, True)
         Exit Sub
      End If

      If String.Equals(e.CommandName, "GoToItem") Then
         Dim newStartRowIndex As Integer = Int32.Parse(e.CommandArgument.ToString()) - 1
         MyBase.DataPager.SetPageProperties(newStartRowIndex, MyBase.DataPager.PageSize, True)
         Exit Sub
      End If

      If String.IsNullOrEmpty(MyBase.DataPager.QueryStringField) Then
         If String.Equals(e.CommandName, "Prev") Then
            Dim startRowIndex As Integer = Me._startRowIndex - MyBase.DataPager.PageSize
            If startRowIndex < 0 Then
               startRowIndex = 0
            End If
            MyBase.DataPager.SetPageProperties(startRowIndex, MyBase.DataPager.PageSize, True)
         ElseIf String.Equals(e.CommandName, "Next") Then
            Dim nextStartRowIndex As Integer = Me._startRowIndex + MyBase.DataPager.PageSize

            'If nextStartRowIndex > Me._totalRowCount Then
            '   nextStartRowIndex = Me._totalRowCount - MyBase.DataPager.PageSize
            'End If

            If nextStartRowIndex >= Me._totalRowCount Then
               nextStartRowIndex = Me._startRowIndex
            End If

            If nextStartRowIndex < 0 Then
               nextStartRowIndex = 0
            End If

            MyBase.DataPager.SetPageProperties(nextStartRowIndex, MyBase.DataPager.PageSize, True)
         End If
      End If
   End Sub

   Private Sub CreateDataPagersForCommand(ByVal container As DataPagerFieldItem, ByVal fieldIndex As Integer)
      'Goto item texbox
      Me.CreateGoToTexBox(container)

      'Control used to set the page size.
      Me.CreatePageSizeControl(container)

      'Set of records - total records
      Me.CreateLabelRecordControl(container)

      'Previous button
      If Me._showPreviousPage Then
         container.Controls.Add(Me.CreateControl("Prev", Me.PreviousPageText, fieldIndex, Me.PreviousPageImageUrl, Me._showPreviousPage))
         Me.AddNonBreakingSpace(container)
      End If

      'Next button
      If Me._showNextPage Then
         container.Controls.Add(Me.CreateControl("Next", Me.NextPageText, fieldIndex, Me.NextPageImageUrl, Me._showNextPage))
         Me.AddNonBreakingSpace(container)
      End If
   End Sub

   Private Function CreateControl(ByVal commandName As String, ByVal buttonText As String, ByVal fieldIndex As Integer, ByVal imageUrl As String, ByVal enabled As Boolean) As Control
      Dim control As IButtonControl

      control = New ImageButton()
      DirectCast(control, ImageButton).ImageUrl = imageUrl
      DirectCast(control, ImageButton).Enabled = enabled
      DirectCast(control, ImageButton).AlternateText = HttpUtility.HtmlDecode(buttonText)

      control.Text = buttonText
      control.CommandName = commandName
      control.CommandArgument = fieldIndex.ToString(CultureInfo.InvariantCulture)
      Dim control2 As WebControl = TryCast(control, WebControl)
      If (control2 IsNot Nothing) AndAlso Not String.IsNullOrEmpty(Me.ButtonCssClass) Then
         control2.CssClass = Me.ButtonCssClass
      End If

      Return TryCast(control, Control)
   End Function

   Private Sub AddNonBreakingSpace(ByVal container As DataPagerFieldItem)
      If Me.RenderNonBreakingSpacesBetweenControls Then
         container.Controls.Add(New LiteralControl(" "))
      End If
   End Sub

   Private Sub CreateLabelRecordControl(ByVal container As DataPagerFieldItem)
      Dim endRowIndex As Integer = Me._startRowIndex + MyBase.DataPager.PageSize

      If endRowIndex > Me._totalRowCount Then
         endRowIndex = Me._totalRowCount
      End If

      container.Controls.Add(New LiteralControl(String.Format("{0} - {1} of {2}", Me._startRowIndex + 1, endRowIndex, Me._totalRowCount)))

      Me.AddNonBreakingSpace(container)
      Me.AddNonBreakingSpace(container)
      Me.AddNonBreakingSpace(container)
   End Sub

   Private Sub CreatePageSizeControl(ByVal container As DataPagerFieldItem)
      container.Controls.Add(New LiteralControl("Show rows: "))

      Dim pageSizeDropDownList As New ButtonDropDownList()

      pageSizeDropDownList.CommandName = "UpdatePageSize"

      pageSizeDropDownList.Items.Add(New ListItem("10", "10"))
      pageSizeDropDownList.Items.Add(New ListItem("25", "25"))
      pageSizeDropDownList.Items.Add(New ListItem("50", "50"))
      pageSizeDropDownList.Items.Add(New ListItem("100", "100"))
      pageSizeDropDownList.Items.Add(New ListItem("250", "250"))
      pageSizeDropDownList.Items.Add(New ListItem("500", "500"))

      Dim pageSizeItem As ListItem = pageSizeDropDownList.Items.FindByValue(MyBase.DataPager.PageSize.ToString())

      If pageSizeItem Is Nothing Then
         pageSizeItem = New ListItem(MyBase.DataPager.PageSize.ToString(), MyBase.DataPager.PageSize.ToString())
         pageSizeDropDownList.Items.Insert(0, pageSizeItem)
      End If

      pageSizeItem.Selected = True
      container.Controls.Add(pageSizeDropDownList)

      Me.AddNonBreakingSpace(container)
      Me.AddNonBreakingSpace(container)
   End Sub

   Private Sub CreateGoToTexBox(ByVal container As DataPagerFieldItem)
      Dim label As New Label()
      label.Text = "Go to: "
      container.Controls.Add(label)

      Dim goToTextBox As New ButtonTextBox()

      goToTextBox.CommandName = "GoToItem"
      goToTextBox.Width = New Unit("20px")
      container.Controls.Add(goToTextBox)

      Me.AddNonBreakingSpace(container)
      Me.AddNonBreakingSpace(container)
   End Sub

   Private Sub CreateDataPagersForQueryString(ByVal container As DataPagerFieldItem, ByVal fieldIndex As Integer)
      Dim validPageIndex As Boolean = False
      If Not MyBase.QueryStringHandled Then
         Dim num As Integer
         MyBase.QueryStringHandled = True
         If Integer.TryParse(MyBase.QueryStringValue, num) Then
            num -= 1
            Dim currentPageIndex As Integer = Me._startRowIndex / Me._maximumRows
            Dim maxPageIndex As Integer = (Me._totalRowCount - 1) / Me._maximumRows
            If (num >= 0) AndAlso (num <= maxPageIndex) Then
               Me._startRowIndex = num * Me._maximumRows
               validPageIndex = True
            End If
         End If
      End If

      'Goto item texbox
      Me.CreateGoToTexBox(container)

      'Control used to set the page size.
      Me.CreatePageSizeControl(container)

      'Set of records - total records
      Me.CreateLabelRecordControl(container)

      If Me._showPreviousPage Then
         Dim pageIndex As Integer = (Me._startRowIndex / Me._maximumRows) - 1
         container.Controls.Add(Me.CreateLink(Me.PreviousPageText, pageIndex, Me.PreviousPageImageUrl, Me.EnablePreviousPage))
         Me.AddNonBreakingSpace(container)
      End If
      If Me._showNextPage Then
         Dim num4 As Integer = (Me._startRowIndex + Me._maximumRows) / Me._maximumRows
         container.Controls.Add(Me.CreateLink(Me.NextPageText, num4, Me.NextPageImageUrl, Me.EnableNextPage))
         Me.AddNonBreakingSpace(container)
      End If
      If validPageIndex Then
         MyBase.DataPager.SetPageProperties(Me._startRowIndex, Me._maximumRows, True)
      End If
   End Sub

   Private Function CreateLink(ByVal buttonText As String, ByVal pageIndex As Integer, ByVal imageUrl As String, ByVal enabled As Boolean) As HyperLink
      Dim pageNumber As Integer = pageIndex + 1
      Dim link As New HyperLink()
      link.Text = buttonText
      link.NavigateUrl = BuildPageUrlWithQueryStrings(pageNumber)
      link.ImageUrl = imageUrl
      link.Enabled = enabled
      If Not String.IsNullOrEmpty(Me.ButtonCssClass) Then
         link.CssClass = Me.ButtonCssClass
      End If
      Return link
   End Function

   Private Function BuildPageUrlWithQueryStrings(ByVal qsValue As String) As String
      Dim builder As New StringBuilder

      'Dim flag As Boolean = Page.Form.Method.Equals("GET", StringComparison.OrdinalIgnoreCase)
      Dim flag As Boolean = HttpContext.Current.Request.RequestType.Equals("GET", StringComparison.OrdinalIgnoreCase)
      Dim baseUrl As String = HttpContext.Current.Request.RawUrl


      If HttpContext.Current.Request.RawUrl.IndexOf("?") <> -1 Then
         baseUrl = baseUrl.Substring(0, baseUrl.IndexOf("?"))
         builder.Append(baseUrl)
      Else
         builder.Append(HttpContext.Current.Request.RawUrl)
      End If
      builder.Append("?")


      Dim str2 As String
      For Each str2 In HttpContext.Current.Request.QueryString.AllKeys
         If ((Not flag OrElse Not IsBuiltInHiddenField(str2)) AndAlso Not str2.Equals(MyBase.DataPager.QueryStringField, StringComparison.OrdinalIgnoreCase)) Then
            builder.Append(HttpUtility.UrlEncode(str2))
            builder.Append("=")
            builder.Append(HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString.Item(str2)))
            builder.Append("&")
         End If
      Next
      'End If

      'If HttpContext.Current.Request.RawUrl.ToLower.IndexOf(MyBase.DataPager.QueryStringField) <> -1 Then


      'Else
      builder.Append(MyBase.DataPager.QueryStringField)
      builder.Append("=")
      'End If


      Dim strqueryStringNavigateUrl As String = builder.ToString
      'End If
      Return (strqueryStringNavigateUrl & qsValue)
   End Function

   Friend Overloads Shared Function IsBuiltInHiddenField(ByVal hiddenFieldName As String) As Boolean
      If (hiddenFieldName.Length <= 2) Then
         Return False
      End If
      If ((hiddenFieldName.Chars(0) <> "_"c) OrElse (hiddenFieldName.Chars(1) <> "_"c)) Then
         Return False
      End If
      If (((Not hiddenFieldName.StartsWith("__VIEWSTATE", StringComparison.Ordinal) AndAlso Not String.Equals(hiddenFieldName, "__EVENTVALIDATION", StringComparison.Ordinal)) AndAlso (Not String.Equals(hiddenFieldName, "__LASTFOCUS", StringComparison.Ordinal) AndAlso Not String.Equals(hiddenFieldName, "__SCROLLPOSITIONX", StringComparison.Ordinal))) AndAlso ((Not String.Equals(hiddenFieldName, "__SCROLLPOSITIONY", StringComparison.Ordinal) AndAlso Not String.Equals(hiddenFieldName, "__EVENTTARGET", StringComparison.Ordinal)) AndAlso Not String.Equals(hiddenFieldName, "__EVENTARGUMENT", StringComparison.Ordinal))) Then
         Return String.Equals(hiddenFieldName, "__PREVIOUSPAGE", StringComparison.Ordinal)
      End If
      Return True
   End Function

End Class
