Public Interface IBadWordRepository
   Function GetBadWords() As IQueryable(Of BadWord)
   Function InsertBadWords(ByVal bw As BadWord) As Integer
   Sub UpdateBadWord(ByVal bw As BadWord)
   Sub DeleteBadWord(ByVal wordId As Integer)
   Sub DeleteBadWords(ByVal Ids() As Integer)
End Interface
