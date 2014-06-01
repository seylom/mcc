Imports System
Imports System.Text
Imports System.Web.UI.WebControls

Namespace SEOFriendlyPaging
   Public Module Pager
      Sub New()
      End Sub
      Public Function CreatePagerLinks(ByVal pgdDataSource As PagedDataSource, ByVal BaseUrl As String) As String
         If pgdDataSource Is Nothing Then
            Return ""
         End If
         Dim sbPager As New StringBuilder()
         'sbPager.Append("More: ");
         If Not pgdDataSource.IsFirstPage Then
            ' first page link
            sbPager.Append("<a href=""")
            sbPager.Append(BaseUrl)
            sbPager.Append(""">|<</a> ")
            If pgdDataSource.CurrentPageIndex <> 1 Then
               ' previous page link
               sbPager.Append("<a href=""")
               sbPager.Append(BaseUrl)
               sbPager.Append("&page=")
               sbPager.Append(pgdDataSource.CurrentPageIndex.ToString())
               sbPager.Append(""" alt=""Previous Page""><<</a>  ")
            End If
         End If
         ' calc low and high limits for numeric links
         Dim intLow As Integer = pgdDataSource.CurrentPageIndex - 1
         Dim intHigh As Integer = pgdDataSource.CurrentPageIndex + 3
         If intLow < 1 Then
            intLow = 1
         End If
         If intHigh > pgdDataSource.PageCount Then
            intHigh = pgdDataSource.PageCount
         End If
         If intHigh - intLow < 5 Then
            While (intHigh < intLow + 4) AndAlso intHigh < pgdDataSource.PageCount
               intHigh += 1
            End While
         End If
         If intHigh - intLow < 5 Then
            While (intLow > intHigh - 4) AndAlso intLow > 1
               intLow -= 1
            End While
         End If
         For x As Integer = intLow To intHigh
            ' numeric links
            If x = pgdDataSource.CurrentPageIndex + 1 Then
               sbPager.Append(x.ToString() & "  ")
            Else
               sbPager.Append("<a href=""")
               sbPager.Append(BaseUrl)
               sbPager.Append("&Page=")
               sbPager.Append(x.ToString())
               sbPager.Append(""">")
               sbPager.Append(x.ToString())
               sbPager.Append("</a>  ")
            End If
         Next
         If Not pgdDataSource.IsLastPage Then
            If (pgdDataSource.CurrentPageIndex + 2) <> pgdDataSource.PageCount Then
               ' next page link
               sbPager.Append("<a href=""")
               sbPager.Append(BaseUrl)
               sbPager.Append("&Page=")
               sbPager.Append(Convert.ToString(pgdDataSource.CurrentPageIndex + 2))
               sbPager.Append(""">>></a>  ")
            End If
            ' last page link
            sbPager.Append("<a href=""")
            sbPager.Append(BaseUrl)
            sbPager.Append("&Page=")
            sbPager.Append((pgdDataSource.PageCount).ToString())
            sbPager.Append(""">>|</a>")
         End If
         ' convert the final links to a string and  return for assignment
         Return sbPager.ToString()
      End Function
   End Module
End Namespace
