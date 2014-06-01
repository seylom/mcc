Imports Microsoft.VisualBasic


Namespace MCC
   Public MustInherit Class BaseRepository

      Private _cacheKey As String = "CacheKey"
      Public Property CacheKey() As String
         Get
            Return _cacheKey
         End Get
         Set(ByVal value As String)
            _cacheKey = value
         End Set
      End Property
   End Class

End Namespace
