Imports System.Web.Mvc.Html
Imports System.IO
Imports Webdiyer.WebControls.Mvc

Public Module HtmlHelpers


   '<System.Runtime.CompilerServices.Extension()> _
   'Public Function ImageActionLink(ByVal helper As AjaxHelper, ByVal imageUrl As String, ByVal altText As String, ByVal actionName As String, ByVal routeValues As Object, ByVal ajaxOptions As AjaxOptions) As String
   '   Dim builder = New TagBuilder("img")
   '   builder.MergeAttribute("src", imageUrl)
   '   builder.MergeAttribute("alt", altText)
   '   Dim link = helper.ActionLink("[replaceme]", actionName, routeValues, ajaxOptions)
   '   Return link.Replace("[replaceme]", builder.ToString(TagRenderMode.SelfClosing))
   'End Function

    <System.Runtime.CompilerServices.Extension()> _
    Public Function ImageActionLink(ByVal helper As htmlHelper, ByVal imageUrl As String, ByVal altText As String, ByVal actionName As String, ByVal routeValues As Object, ByVal absoluteUrl As Boolean) As String
        Dim builder = New TagBuilder("img")
        builder.MergeAttribute("src", imageUrl)
        builder.MergeAttribute("alt", altText)
        Dim link = helper.ActionLink("[replaceme]", actionName, routeValues, absoluteUrl)
        Return link.Replace("[replaceme]", builder.ToString(TagRenderMode.SelfClosing))
    End Function


   <System.Runtime.CompilerServices.Extension()> _
   Public Function DisplayError(ByVal helper As HtmlHelper) As String
      Dim result As String = ""

      If helper.ViewContext.TempData("ErrorMessage") IsNot Nothing Then
         result = String.Format("<div class=""error"">{0}</div>", helper.ViewContext.TempData("ErrorMessage"))
      End If

      Return result
   End Function

   <System.Runtime.CompilerServices.Extension()> _
   Public Function DisplayError(ByVal helper As HtmlHelper, ByVal model As baseViewModel) As String
      Dim wrap As String = String.Empty

      If model.Messages.Count > 0 Then
         Dim results As String = ""
         For Each it As String In model.Messages
            results += String.Format("<p>{0}</p>", it)
         Next
         wrap = String.Format("<div style='padding: 5px; background-color: #e9fccc; margin-top: 5px;'>{0}</div>", results)
      End If

      Return wrap
   End Function

   <System.Runtime.CompilerServices.Extension()> _
   Public Function Notify(ByVal helper As HtmlHelper) As String
      Dim result As String = ""
      If helper.ViewContext.TempData("ErrorMessage") IsNot Nothing Then
         result = String.Format("<div class=""error"">{0}</div>", helper.ViewContext.TempData("ErrorMessage"))
      ElseIf helper.ViewContext.TempData("Message") IsNot Nothing Then

         result = String.Format("<div class=""message"">{0}</div>", helper.ViewContext.TempData("Message"))
      End If

      Return result
   End Function

   <System.Runtime.CompilerServices.Extension()> _
  Public Function CheckBoxList(ByVal htmlHelper As HtmlHelper, ByVal name As String, ByVal values As IEnumerable(Of String), ByVal htmlAttributes As Object) As String
      Return CheckBoxList(htmlHelper, name, values, values, htmlAttributes)
   End Function

   <System.Runtime.CompilerServices.Extension()> _
   Public Function CheckBoxList(ByVal helper As HtmlHelper, ByVal name As String, ByVal values As IEnumerable(Of String), ByVal labels As IEnumerable(Of String), ByVal htmlAttributes As Object) As String

      If values Is Nothing Then
         Return ""
      End If

      If labels Is Nothing Then
         labels = New List(Of String)()
      End If

      Dim attributes As RouteValueDictionary = If(htmlAttributes Is Nothing, New RouteValueDictionary(), New RouteValueDictionary(htmlAttributes))
      attributes.Remove("checked")

      Dim sb As New StringBuilder()
      Dim modelValues As String() = New String() {}
      Dim modelState As ModelState

      If helper.ViewData.ModelState.TryGetValue(name, modelState) Then
         modelValues = DirectCast(modelState.Value.RawValue, String())
      End If

      Dim labelEnumerator As IEnumerator(Of String) = labels.GetEnumerator()

      For Each s As String In values
         Dim isChecked As Boolean = modelValues.Contains(s)
         sb.Append(CreateCheckBox(name, s, isChecked, attributes))

         labelEnumerator.MoveNext()

         If labelEnumerator.Current IsNot Nothing Then
            sb.AppendLine(labelEnumerator.Current)
         End If
      Next

      Dim divTag As New TagBuilder("div")
      divTag.InnerHtml = sb.ToString()

      If modelState IsNot Nothing AndAlso modelState.Errors.Count > 0 Then
         divTag.AddCssClass(HtmlHelper.ValidationInputCssClassName)
      End If

      Return divTag.ToString(TagRenderMode.Normal)
   End Function


   Public Function CreateCheckBox(ByVal name As String, ByVal value As String, ByVal isChecked As Boolean, ByVal htmlAttributes As IDictionary(Of String, Object)) As String

      Dim tagBuilder As New TagBuilder("input")
      tagBuilder.MergeAttributes(htmlAttributes)
      tagBuilder.MergeAttribute("type", "checkbox")
      tagBuilder.MergeAttribute("name", name, True)

      tagBuilder.GenerateId(name)

      If isChecked Then
         tagBuilder.MergeAttribute("checked", "checked")
      End If

      If value IsNot Nothing Then
         tagBuilder.MergeAttribute("value", value, True)
      End If

      Return tagBuilder.ToString(TagRenderMode.SelfClosing)
   End Function


   <System.Runtime.CompilerServices.Extension()> _
   Public Function ToSelectList(Of TEnum)(ByVal enumObj As TEnum) As SelectList
      Dim values = From e In [Enum].GetValues(GetType(TEnum)) _
          Select New With {.Id = e, .Name = e.ToString}

      Return New SelectList(values, "Id", "Name", enumObj)
   End Function

   <System.Runtime.CompilerServices.Extension()> _
   Public Function GenerateCaptcha(ByVal htmlhelper As HtmlHelper) As String

      Dim captchaControl As New Recaptcha.RecaptchaControl With {.ID = "recaptcha", _
                                                                 .Theme = "clean", _
                                                                  .PublicKey = "6LeFJgwAAAAAAPCp7sOWypi3JpcSV7YMHMDj1Zxy", _
                                                                  .PrivateKey = "6LeFJgwAAAAAANJ4LxmPSU94lpBQplmCbON0xC4l"}

      Dim htmlWriter As HtmlTextWriter = New HtmlTextWriter(New StringWriter())
      captchaControl.RenderControl(htmlWriter)

      Return htmlWriter.InnerWriter.ToString()
   End Function


   <System.Runtime.CompilerServices.Extension()> _
   Public Function CheckBoxList(ByVal helper As HtmlHelper, ByVal name As String, ByVal listInfo As List(Of CheckBoxListInfo)) As String
      Return helper.CheckBoxList(name, listInfo, CType(Nothing, IDictionary(Of String, Object)))
   End Function


   <System.Runtime.CompilerServices.Extension()> _
   Public Function CheckBoxList(ByVal helper As HtmlHelper, ByVal name As String, _
                                ByVal listInfo As List(Of CheckBoxListInfo), ByVal htmlAttributes As Object) As String
      Return helper.CheckBoxList(name, listInfo, CType(New RouteValueDictionary(htmlAttributes), IDictionary(Of String, Object)))
   End Function


   <System.Runtime.CompilerServices.Extension()> _
   Public Function CheckBoxList(ByVal htmlHelper As HtmlHelper, ByVal name As String, ByVal listInfo As List(Of CheckBoxListInfo), ByVal htmlAttributes As IDictionary(Of String, Object)) As String

      If String.IsNullOrEmpty(name) Then
         Throw New ArgumentException("The argument must have a value", "name")
      End If

      If (listInfo Is Nothing) Then
         Throw New ArgumentNullException("listInfo")
      End If

      If (listInfo.Count < 1) Then
         Throw New ArgumentException("The list must contain at least one value", "listInfo")
      End If

      Dim sb As StringBuilder = New StringBuilder()
      For Each info As CheckBoxListInfo In listInfo

         Dim builder As TagBuilder = New TagBuilder("input")
         If (info.IsChecked) Then
            builder.MergeAttribute("checked", "checked")
         End If
         builder.MergeAttributes(Of String, Object)(htmlAttributes)
         builder.MergeAttribute("type", "checkbox")
         builder.MergeAttribute("value", info.Value)
         builder.MergeAttribute("name", name)
         builder.InnerHtml = info.DisplayText
         sb.Append(builder.ToString(TagRenderMode.Normal))
         sb.Append("<br />")
      Next

      Return sb.ToString()
   End Function


   Public Class CheckBoxListInfo
      Public Sub New(ByVal value As String, ByVal displayText As String, ByVal isChecked As Boolean)
         Me.Value = value
         Me.DisplayText = displayText
         Me.IsChecked = isChecked
      End Sub

      Private _value As String
      Public Property Value() As String
         Get
            Return _value
         End Get
         Set(ByVal value As String)
            _value = value
         End Set
      End Property


      Private _displayText As String
      Public Property DisplayText() As String
         Get
            Return _displayText
         End Get
         Set(ByVal value As String)
            _displayText = value
         End Set
      End Property

      Private _isChecked As Boolean
      Public Property IsChecked() As Boolean
         Get
            Return _isChecked
         End Get
         Set(ByVal value As Boolean)
            _isChecked = value
         End Set
      End Property
   End Class

    <System.Runtime.CompilerServices.Extension()> _
    Public Function SimplePager(Of T)(ByVal helper As HtmlHelper, ByVal Model As PagedList(Of T)) As MvcHtmlString
        Dim pgr As PagerOptions = New PagerOptions With {.PageIndexParameterName = "page", _
                                                              .FirstPageText = "First", _
                                                              .PrevPageText = "Previous", _
                                                              .NextPageText = "Next", _
                                                              .LastPageText = "Last", _
                                                              .CurrentPagerItemWrapperFormatString = "<span class='selected'>{0}</span>", _
                                                              .NumericPagerItemWrapperFormatString = "<span class='unselected'>{0}</span>", _
                                                              .NavigationPagerItemWrapperFormatString = "<span class='unselected'>{0}</span>", _
                                                              .MorePagerItemWrapperFormatString = "<span class='unselected'>{0}</span>", _
                                                              .CssClass = "PageLinks", _
                                                              .SeparatorHtml = "", _
                                                              .NumericPagerItemCount = 3, _
                                                              .ShowDisabledPagerItems = False}
        Return helper.Pager(Model, pgr)
    End Function


    <System.Runtime.CompilerServices.Extension()> _
    Public Function AdvancedPager(Of T)(ByVal helper As HtmlHelper, ByVal Model As PagedList(Of T), ByVal ActionName As String, ByVal ControllerName As String, _
                                      ByVal PageSizes() As Integer) As MvcHtmlString

        If PageSizes IsNot Nothing AndAlso PageSizes.Count > 0 Then



            Dim links As String = String.Empty
            For Each it As Integer In PageSizes

                Dim _routeValues As New RouteValueDictionary
                _routeValues("size") = it

                Dim rq = helper.ViewContext.HttpContext.Request.QueryString
                For Each key As String In rq.Keys
                    If key <> "size" AndAlso key <> "page" Then
                        _routeValues(key) = rq(key)
                    End If
                Next
                _routeValues("Action") = ActionName
                _routeValues("Controller") = ControllerName
                Dim urlHelper = New UrlHelper(helper.ViewContext.RequestContext)

                If Model.PageSize = it Then
                    links += "<span class='selected'>" + it.ToString + "</span>"
                Else
                    links += "<span class='unselected'><a href='" + urlHelper.RouteUrl(_routeValues) + "' title='" + it.ToString + " items' >" + it.ToString + "</a></span>"
                End If

            Next

            Dim linksWrap As String = "<span class='PageLinks'>{0}</span>&nbsp;<span class='PageLinkText'>per page</span>"
            linksWrap = String.Format(linksWrap, links)

            Dim advpagerstring As String
            advpagerstring = "<div class='Pager'>" + _
                                 "<table class='FullWidth'>" + _
                                    "<tr>" + _
                                       "<td align='left'>{0}</td>" + _
                                       "<td align='right'>{1}</td>" + _
                                    "</tr>" + _
                                  "</table>" + _
                             "</div>"

            Return MvcHtmlString.Create(String.Format(advpagerstring, linksWrap, helper.SimplePager(Model)))
        Else
            Return helper.SimplePager(Model)
        End If
    End Function

    <System.Runtime.CompilerServices.Extension()> _
    Function UrlEncode(ByVal stringToEncode As String) As String
        Return HttpUtility.UrlEncode(stringToEncode)
    End Function
End Module


