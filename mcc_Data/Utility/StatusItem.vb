
Public Enum ErrorAction
   Redirect
   Notify
End Enum


Public Class StatusItem

   Private _itemID As Integer = 0
   Public Property Id() As Integer
      Get
         Return _itemID
      End Get
      Set(ByVal value As Integer)
         _itemID = value
      End Set
   End Property

   Private _success As Boolean = False
   Public Property Success() As Boolean
      Get
         Return _success
      End Get
      Set(ByVal value As Boolean)
         _success = value
      End Set
   End Property

   Private _message As String = String.Empty
   Public Property Message() As String
      Get
         Return _message
      End Get
      Set(ByVal value As String)
         _message = value
      End Set
   End Property


   Private _data As Object = Nothing
   Public Property Data() As Object
      Get
         Return _data
      End Get
      Set(ByVal value As Object)
         _data = value
      End Set
   End Property

   Private _action As ErrorAction = ErrorAction.Notify
   Public Property Action() As ErrorAction
      Get
         Return _action
      End Get
      Set(ByVal value As ErrorAction)
         _action = value
      End Set
   End Property

End Class
