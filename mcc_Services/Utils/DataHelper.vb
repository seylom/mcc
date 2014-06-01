Imports System.Text.RegularExpressions

Public Module DataHelper

   <System.Runtime.CompilerServices.Extension()> _
   Public Function CleanString(ByVal str As String, Optional ByVal replacewith As String = "-") As String
      Dim repWith As String = "-"
      If Not String.IsNullOrEmpty(replacewith) Then
         repWith = replacewith
      End If

      'str = str.Trim.Replace(" ", repWith)
      'str = Regex.Replace(str, "[@^\.?:\/*""<>|'\$,!;&#%]", repWith).ToLower
      str = Regex.Replace(str, "[^\w-]", repWith).ToLower
        str = str.TrimEnd(repWith.ToCharArray()).TrimStart(repWith.ToCharArray())

      Return str
   End Function


   Public Function BuildUrlFromTitleAndId(ByVal Id As Integer, ByVal title As String) As String
      Dim str As String = ""
      str = title.ToSlug()
      'str = Id.ToString + "_" + str + ".aspx"
      str += ".aspx"
      Return str
   End Function

   <System.Runtime.CompilerServices.Extension()> _
   Public Function ToSlug(ByVal str As String, Optional ByVal replaceWith As String = "-") As String
      Dim slug As String = ""
      slug = CleanString(str, replaceWith)
      Return slug
   End Function

   Const ValidChars As String = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ"

   Public Function GenerateName(ByVal random As Random, ByVal length As Integer) As String
      Dim chars As Char() = New Char(length - 1) {}
      For i As Integer = 0 To length - 1
         chars(i) = ValidChars(random.[Next](ValidChars.Length))
      Next
      Return New String(chars).ToLower()
   End Function
End Module
