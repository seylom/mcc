Imports System.Linq
Imports System.Linq.Expressions

<HideModuleName()> _
Public Module AdviceFilters

   <System.Runtime.CompilerServices.Extension()> _
   Function WithAdviceID(ByVal qry As IQueryable(Of MCC.Data.Advice), ByVal AdviceId As Integer) As IQueryable(Of MCC.Data.Advice)
      Return From o In qry Where (o.AdviceID = AdviceId) _
                   Select o
   End Function

   <System.Runtime.CompilerServices.Extension()> _
   Function WithSlug(ByVal qry As IQueryable(Of MCC.Data.Advice), ByVal slug As String) As IQueryable(Of MCC.Data.Advice)
      Return From o In qry Where (o.Slug = slug) _
                   Select o
   End Function

   <System.Runtime.CompilerServices.Extension()> _
   Function WithAuthor(ByVal qry As IQueryable(Of MCC.Data.Advice), ByVal Author As String) As IQueryable(Of MCC.Data.Advice)
      Return From o In qry Where (o.AddedBy.Equals(Author, StringComparison.OrdinalIgnoreCase)) _
                  Select o
   End Function

   <System.Runtime.CompilerServices.Extension()> _
   Function Published(ByVal qry As IQueryable(Of MCC.Data.Advice)) As IQueryable(Of MCC.Data.Advice)
      Return From p In qry Where _
             p.Approved = True _
             Select p
   End Function


End Module


<HideModuleName()> _
Public Module AdviceCategoryFilter
   <System.Runtime.CompilerServices.Extension()> _
Function WithID(ByVal qry As IQueryable(Of MCC.Data.AdviceCategory), ByVal CategoryId As Integer) As IQueryable(Of MCC.Data.AdviceCategory)
      Return From o In qry Where (o.CategoryID = CategoryId) _
                   Select o
   End Function

   <System.Runtime.CompilerServices.Extension()> _
   Function WithSlug(ByVal qry As IQueryable(Of MCC.Data.AdviceCategory), ByVal slug As String) As IQueryable(Of MCC.Data.AdviceCategory)
      Return From o In qry Where (o.Slug = slug) _
                   Select o
   End Function
End Module


<HideModuleName()> _
Public Module AdviceCommentFilter
   <System.Runtime.CompilerServices.Extension()> _
   Function WithID(ByVal qry As IQueryable(Of MCC.Data.AdviceComment), ByVal commentId As Integer) As IQueryable(Of MCC.Data.AdviceComment)
      Return From o In qry Where (o.CommentID = commentId) _
                   Select o
   End Function

   <System.Runtime.CompilerServices.Extension()> _
   Function WithAdviceID(ByVal qry As IQueryable(Of MCC.Data.AdviceComment), ByVal adviceId As Integer) As IQueryable(Of MCC.Data.AdviceComment)
      Return From o In qry Where (o.AdviceID = adviceId) _
                   Select o
   End Function

   '<System.Runtime.CompilerServices.Extension()> _
   'Function WithSlug(ByVal qry As IQueryable(Of MCC.Data.AdviceComment), ByVal slug As String) As IQueryable(Of MCC.Data.AdviceComment)
   '   Return From o In qry Where (o.Slug = slug) _
   '                Select o
   'End Function
End Module
