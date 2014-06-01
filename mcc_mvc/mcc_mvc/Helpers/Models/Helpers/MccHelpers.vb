Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Configuration
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports System.Web.Caching
Imports System.IO
Imports System.Collections
Imports System.Collections.Specialized


Public Class mccHelpers

   Private Shared _countries As String() = New String() {"Afghanistan", "Albania", "Algeria", _
"American Samoa", "Andorra", "Angola", "Anguilla", "Antarctica", "Antigua And Barbuda", "Argentina", _
 "Armenia", "Aruba", "Australia", "Austria", "Azerbaijan", "Bahamas", "Bahrain", "Bangladesh", "Barbados", _
 "Belarus", "Belgium", "Belize", "Benin", "Bermuda", "Bhutan", "Bolivia", "Bosnia Hercegovina", "Botswana", _
 "Bouvet Island", "Brazil", "Brunei Darussalam", "Bulgaria", "Burkina Faso", "Burundi", "Byelorussian SSR", _
 "Cambodia", "Cameroon", "Canada", "Cape Verde", "Cayman Islands", "Central African Republic", "Chad", _
 "Chile", "China", "Christmas Island", "Cocos (Keeling) Islands", "Colombia", "Comoros", "Congo", "Cook Islands", _
 "Costa Rica", "Cote D'Ivoire", "Croatia", "Cuba", "Cyprus", "Czech Republic", "Czechoslovakia", "Denmark", _
 "Djibouti", "Dominica", "Dominican Republic", "East Timor", "Ecuador", "Egypt", "El Salvador", "England", _
 "Equatorial Guinea", "Eritrea", "Estonia", "Ethiopia", "Falkland Islands", "Faroe Islands", "Fiji", "Finland", _
 "France", "Gabon", "Gambia", "Georgia", "Germany", "Ghana", "Gibraltar", "Great Britain", "Greece", "Greenland", _
 "Grenada", "Guadeloupe", "Guam", "Guatemela", "Guernsey", "Guiana", "Guinea", "Guinea-Bissau", "Guyana", "Haiti", _
 "Heard Islands", "Honduras", "Hong Kong", "Hungary", "Iceland", "India", "Indonesia", "Iran", "Iraq", "Ireland", _
 "Isle Of Man", "Israel", "Italy", "Jamaica", "Japan", "Jersey", "Jordan", "Kazakhstan", "Kenya", "Kiribati", "Korea, South", _
 "Korea, North", "Kuwait", "Kyrgyzstan", "Lao People's Dem. Rep.", "Latvia", "Lebanon", "Lesotho", "Liberia", "Libya", _
 "Liechtenstein", "Lithuania", "Luxembourg", "Macau", "Macedonia", "Madagascar", "Malawi", "Malaysia", "Maldives", "Mali", _
 "Malta", "Mariana Islands", "Marshall Islands", "Martinique", "Mauritania", "Mauritius", "Mayotte", "Mexico", "Micronesia", _
 "Moldova", "Monaco", "Mongolia", "Montserrat", "Morocco", "Mozambique", "Myanmar", "Namibia", "Nauru", "Nepal", "Netherlands", _
 "Netherlands Antilles", "Neutral Zone", "New Caledonia", "New Zealand", "Nicaragua", "Niger", "Nigeria", "Niue", "Norfolk Island", _
 "Northern Ireland", "Norway", "Oman", "Pakistan", "Palau", "Panama", "Papua New Guinea", "Paraguay", "Peru", "Philippines", _
 "Pitcairn", "Poland", "Polynesia", "Portugal", "Puerto Rico", "Qatar", "Reunion", "Romania", "Russian Federation", "Rwanda", _
 "Saint Helena", "Saint Kitts", "Saint Lucia", "Saint Pierre", "Saint Vincent", "Samoa", "San Marino", "Sao Tome and Principe", _
 "Saudi Arabia", "Scotland", "Senegal", "Seychelles", "Sierra Leone", "Singapore", "Slovakia", "Slovenia", "Solomon Islands", _
 "Somalia", "South Africa", "South Georgia", "Spain", "Sri Lanka", "Sudan", "Suriname", "Svalbard", "Swaziland", "Sweden", _
 "Switzerland", "Syrian Arab Republic", "Taiwan", "Tajikista", "Tanzania", "Thailand", "Togo", "Tokelau", "Tonga", _
 "Trinidad and Tobago", "Tunisia", "Turkey", "Turkmenistan", "Turks and Caicos Islands", "Tuvalu", "Uganda", "Ukraine", _
 "United Arab Emirates", "United Kingdom", "United States", "Uruguay", "Uzbekistan", "Vanuatu", "Vatican City State", _
 "Venezuela", "Vietnam", "Virgin Islands", "Wales", "Western Sahara", "Yemen", "Yugoslavia", "Zaire", "Zambia", "Zimbabwe"}


   Public Shared Function GetCountries() As IQueryable(Of String)
      Dim countries As New List(Of String)
      countries.AddRange(_countries)
      Return countries.AsQueryable
   End Function

   Public Shared Function GetCountries(ByVal insertEmpty As Boolean) As SortedList
      Dim countries As SortedList = New SortedList
      If insertEmpty Then
         countries.Add("", "Please select one...")
      End If
      For Each country As String In _countries
         countries.Add(country, country)
      Next
      Return countries
   End Function

   Public Shared Function GetThemes() As String()
      If Not (HttpContext.Current.Cache("SiteThemes") Is Nothing) Then
         Return CType(HttpContext.Current.Cache("SiteThemes"), String())
      Else
         Dim themesDirPath As String = HttpContext.Current.Server.MapPath("~/App_Themes")
         Dim themes As String() = Directory.GetDirectories(themesDirPath)
         Dim i As Integer = 0
         While i <= themes.Length - 1
            themes(i) = Path.GetFileName(themes(i))
            System.Math.Min(System.Threading.Interlocked.Increment(i), i - 1)
         End While
         Dim dep As CacheDependency = New CacheDependency(themesDirPath)
         HttpContext.Current.Cache.Insert("SiteThemes", themes, dep)
         Return themes
      End If
   End Function

   Public Shared Sub SetInputControlsHighlight(ByVal container As Control, ByVal className As String, ByVal onlyTextBoxes As Boolean)
      'For Each ctl As Control In container.Controls
      '   If (onlyTextBoxes AndAlso TypeOf ctl Is TextBox) OrElse TypeOf ctl Is TextBox OrElse TypeOf ctl Is DropDownList OrElse TypeOf ctl Is ListBox OrElse TypeOf ctl Is CheckBox OrElse TypeOf ctl Is RadioButton OrElse TypeOf ctl Is RadioButtonList OrElse TypeOf ctl Is CheckBoxList Then
      '      Dim wctl As WebControl = CType(ctl, WebControl)
      '      wctl.Attributes.Add("onfocus", String.Format("this.className = '{0}';", className))
      '      wctl.Attributes.Add("onblur", "this.className = '';")
      '   Else
      '      If ctl.Controls.Count > 0 Then
      '         SetInputControlsHighlight(ctl, className, onlyTextBoxes)
      '      End If
      '   End If
      'Next
   End Sub

   Public Shared Function ConvertToHtml(ByVal content As String) As String
      content = HttpUtility.HtmlEncode(content)
      content = content.Replace(" ", "&nbsp;&nbsp;").Replace("" & Microsoft.VisualBasic.Chr(9) & "", "&nbsp;&nbsp;&nbsp;").Replace("" & Microsoft.VisualBasic.Chr(10) & "", "<br>")
      Return content
   End Function

   Public Shared Function RemoveHTMLTags(ByVal strHTML As String, ByVal intWorkFlow As Integer) As String

      Dim regEx As Regex
      Dim regOptions As RegexOptions
      Dim strResult As String

      strResult = strHTML

      regOptions = regOptions And RegexOptions.IgnoreCase


      If intWorkFlow <> 1 Then
         regEx = New Regex("<[^>]*>", regOptions)
         strResult = regEx.Replace(strResult, "")
      End If

      If intWorkFlow > 0 And intWorkFlow < 3 Then
         regEx = New Regex("[<]", regOptions)
         strResult = regEx.Replace(strResult, "<")

         regEx = New Regex("[>]", regOptions)
         strResult = regEx.Replace(strResult, ">")
      End If

      regEx = Nothing
      Return strResult
   End Function
End Class
