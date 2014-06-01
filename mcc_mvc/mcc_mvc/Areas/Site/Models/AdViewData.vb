Imports MCC.mccEnum
Imports MCC.Services

Public Class AdViewData
   Inherits baseViewModel



   Private _adType As adType = adType.Undefined
   Public Property Type() As adType
      Get
         Return _adType
      End Get
      Set(ByVal value As adType)
         _adType = value
      End Set
   End Property


   Private _title As String
   Public Property Title() As String
      Get
         Return _title
      End Get
      Set(ByVal value As String)
         _title = value
      End Set
   End Property



   Private _description As String
   Public Property Description() As String
      Get
         Return _description
      End Get
      Set(ByVal value As String)
         _description = value
      End Set
   End Property

   Private _body As String
   Public Property Body() As String
      Get
         Return _body
      End Get
      Set(ByVal value As String)
         _body = value
      End Set
   End Property


   Private _keywords As String
   Public Property Keywords() As String
      Get
         Return _keywords
      End Get
      Set(ByVal value As String)
         _keywords = value
      End Set
   End Property

End Class
