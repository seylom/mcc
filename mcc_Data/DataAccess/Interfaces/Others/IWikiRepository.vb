Public Interface IWikiRepository
   Function GetWikiPages() As IQueryable(Of WikiPage)
   Function InsertWikiPage(ByVal page As WikiPage) As Integer
   Sub UpdateWikiPage(ByVal page As WikiPage)
   Sub DeleteWikiPage(ByVal id As Integer)
   Sub DeleteWikis(ByVal Ids() As Integer)
End Interface
