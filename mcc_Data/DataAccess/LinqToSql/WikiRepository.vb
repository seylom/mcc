Public Class WikiRepository
   Implements IWikiRepository


   Private _mdc As MCCDataContext
   Public Sub New()
      Me.New(new MCCDataContext)
   End Sub
   Public Sub New(ByVal db As MCCDataContext)
      _mdc = db
   End Sub
   Public Function GetWikiPages() As IQueryable(Of WikiPage) Implements IWikiRepository.GetWikiPages
      Throw New NotImplementedException()
   End Function
   Public Function InsertWikiPage(ByVal page As WikiPage) As Integer Implements IWikiRepository.InsertWikiPage
      Throw New NotImplementedException()
   End Function
   Public Sub UpdateWikiPage(ByVal page As WikiPage) Implements IWikiRepository.UpdateWikiPage
      Throw New NotImplementedException()
   End Sub
   Public Sub DeleteWikiPage(ByVal id As Integer) Implements IWikiRepository.DeleteWikiPage
      Throw New NotImplementedException()
   End Sub
   Public Sub DeleteWikis(ByVal Ids As Integer()) Implements IWikiRepository.DeleteWikis
      Throw New NotImplementedException()
   End Sub
End Class
