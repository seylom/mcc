Imports MCC.Data

Public Class AskQuestionViewModel
   Inherits baseViewModel


   Private _title As String
   Public Property Title() As String
      Get
         Return _title
      End Get
      Set(ByVal value As String)
         _title = value
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


   Private _subscribe As Boolean
   Public Property Subscribe() As Boolean
      Get
         Return _subscribe
      End Get
      Set(ByVal value As Boolean)
         _subscribe = value
      End Set
   End Property


End Class
