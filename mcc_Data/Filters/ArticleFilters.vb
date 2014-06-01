Imports System.Linq
Imports System.Linq.Expressions

<HideModuleName()> _
Public Module ArticleFilters

   <System.Runtime.CompilerServices.Extension()> _
   Function WithArticleID(ByVal qry As IQueryable(Of MCC.Data.Article), ByVal ArticleId As Integer) As IQueryable(Of MCC.Data.Article)
      Return From o In qry Where (o.ArticleID = ArticleId) _
                   Select o
   End Function

   <System.Runtime.CompilerServices.Extension()> _
   Function WithSlug(ByVal qry As IQueryable(Of MCC.Data.Article), ByVal slug As String) As IQueryable(Of MCC.Data.Article)
      Return From o In qry Where (o.Slug = slug) _
                   Select o
   End Function

   <System.Runtime.CompilerServices.Extension()> _
   Function WithAuthor(ByVal qry As IQueryable(Of MCC.Data.Article), ByVal Author As String) As IQueryable(Of MCC.Data.Article)
      Return From o In qry Where (o.AddedBy.ToLower = Author.ToLower) _
                  Select o
   End Function

   <System.Runtime.CompilerServices.Extension()> _
   Function Published(ByVal qry As IQueryable(Of MCC.Data.Article)) As IQueryable(Of MCC.Data.Article)
      Return From p In qry Where _
             p.Approved = True AndAlso p.ReleaseDate <= DateTime.Now AndAlso p.ExpireDate > DateTime.Now _
             Select p
   End Function

   '   <System.Runtime.CompilerServices.Extension()> _
   'Function Published(ByVal qry As IQueryable(Of MCC.Data.ArticleAdv)) As IQueryable(Of MCC.Data.Article)
   '      Return From p In qry Where _
   '             p.Approved = True AndAlso p.ReleaseDate <= DateTime.Now AndAlso p.ExpireDate > DateTime.Now _
   '             Select p
   '   End Function

End Module


<HideModuleName()> _
Public Module ArticleCategoryFilter
   <System.Runtime.CompilerServices.Extension()> _
Function WithID(ByVal qry As IQueryable(Of MCC.Data.ArticleCategory), ByVal CategoryId As Integer) As IQueryable(Of MCC.Data.ArticleCategory)
      Return From o In qry Where (o.CategoryID = CategoryId) _
                   Select o
   End Function

   <System.Runtime.CompilerServices.Extension()> _
   Function WithSlug(ByVal qry As IQueryable(Of MCC.Data.ArticleCategory), ByVal slugs() As String) As IQueryable(Of MCC.Data.ArticleCategory)

      Return From o In qry Where (slugs.Contains(o.Slug)) _
                   Select o
   End Function
End Module


<HideModuleName()> _
Public Module ArticleCommentFilter
   <System.Runtime.CompilerServices.Extension()> _
   Function WithID(ByVal qry As IQueryable(Of MCC.Data.ArticleComment), ByVal commentId As Integer) As IQueryable(Of MCC.Data.ArticleComment)
      Return From o In qry Where (o.CommentID = commentId) _
                   Select o
   End Function

   <System.Runtime.CompilerServices.Extension()> _
   Function WithArticleID(ByVal qry As IQueryable(Of MCC.Data.ArticleComment), ByVal articleId As Integer) As IQueryable(Of MCC.Data.ArticleComment)
      Return From o In qry Where (o.ArticleID = articleId) _
                   Select o
   End Function

   '<System.Runtime.CompilerServices.Extension()> _
   'Function WithSlug(ByVal qry As IQueryable(Of MCC.Data.ArticleComment), ByVal slug As String) As IQueryable(Of MCC.Data.ArticleComment)
   '   Return From o In qry Where (o.Slug = slug) _
   '                Select o
   'End Function
End Module