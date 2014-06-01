Imports System.Linq
Imports System.Linq.Expressions

<HideModuleName()> _
Public Module VideoFilters

   <System.Runtime.CompilerServices.Extension()> _
   Function WithVideoID(ByVal qry As IQueryable(Of MCC.Data.Video), ByVal VideoId As Integer) As IQueryable(Of MCC.Data.Video)
      Return From o In qry Where (o.VideoID = VideoId) _
                   Select o
   End Function

   <System.Runtime.CompilerServices.Extension()> _
   Function WithSlug(ByVal qry As IQueryable(Of MCC.Data.Video), ByVal slug As String) As IQueryable(Of MCC.Data.Video)
      Return From o In qry Where (o.Slug = slug) _
                   Select o
   End Function

   <System.Runtime.CompilerServices.Extension()> _
   Function WithAuthor(ByVal qry As IQueryable(Of MCC.Data.Video), ByVal Author As String) As IQueryable(Of MCC.Data.Video)
      Return From o In qry Where (o.AddedBy.Equals(Author, StringComparison.OrdinalIgnoreCase)) _
                  Select o
   End Function

   <System.Runtime.CompilerServices.Extension()> _
   Function Published(ByVal qry As IQueryable(Of MCC.Data.Video)) As IQueryable(Of MCC.Data.Video)
      Return From p In qry Where _
             p.Approved = True _
             Select p
   End Function
End Module


<HideModuleName()> _
Public Module VideoCategoryFilter
   <System.Runtime.CompilerServices.Extension()> _
Function WithID(ByVal qry As IQueryable(Of MCC.Data.VideoCategory), ByVal CategoryId As Integer) As IQueryable(Of MCC.Data.VideoCategory)
      Return From o In qry Where (o.CategoryID = CategoryId) _
                   Select o
   End Function

   <System.Runtime.CompilerServices.Extension()> _
   Function WithSlug(ByVal qry As IQueryable(Of MCC.Data.VideoCategory), ByVal slug As String) As IQueryable(Of MCC.Data.VideoCategory)
      Return From o In qry Where (o.Slug = slug) _
                   Select o
   End Function
End Module


<HideModuleName()> _
Public Module VideoCommentFilter
   <System.Runtime.CompilerServices.Extension()> _
   Function WithID(ByVal qry As IQueryable(Of MCC.Data.VideoComment), ByVal commentId As Integer) As IQueryable(Of MCC.Data.VideoComment)
      Return From o In qry Where (o.CommentID = commentId) _
                   Select o
   End Function

   <System.Runtime.CompilerServices.Extension()> _
   Function WithVideoID(ByVal qry As IQueryable(Of MCC.Data.VideoComment), ByVal videoId As Integer) As IQueryable(Of MCC.Data.VideoComment)
      Return From o In qry Where (o.VideoID = videoId) _
                   Select o
   End Function

   '<System.Runtime.CompilerServices.Extension()> _
   'Function WithSlug(ByVal qry As IQueryable(Of MCC.Data.VideoComment), ByVal slug As String) As IQueryable(Of MCC.Data.VideoComment)
   '   Return From o In qry Where (o.Slug = slug) _
   '                Select o
   'End Function
End Module
