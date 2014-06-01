Public Class BadWordRepository
   Implements IBadWordRepository
   Private _mdc As MCCDataContext
   Public Sub New()
      Me.New(new MCCDataContext)
   End Sub
   Public Sub New(ByVal db As MCCDataContext)
      _mdc = db
   End Sub
   Public Function GetBadWords() As IQueryable(Of BadWord) Implements IBadWordRepository.GetBadWords
      Throw New NotImplementedException()
   End Function
   Public Function InsertBadWords(ByVal bw As BadWord) As Integer Implements IBadWordRepository.InsertBadWords
      Throw New NotImplementedException()
   End Function
   Public Sub UpdatebadWord(ByVal bw As BadWord) Implements IBadWordRepository.UpdatebadWord
      Throw New NotImplementedException()
   End Sub
   Public Sub DeleteBadWord(ByVal wordId As Integer) Implements IBadWordRepository.DeleteBadWord
      Throw New NotImplementedException()
   End Sub
   Public Sub DeleteBadWords(ByVal Ids As Integer()) Implements IBadWordRepository.DeleteBadWords
      Throw New NotImplementedException()
   End Sub
End Class
