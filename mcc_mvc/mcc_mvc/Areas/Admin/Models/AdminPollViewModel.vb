Imports MCC.Data

Public Class AdminPollViewModel
   Inherits baseViewModel

   Private _pollId As Integer
   Public Property PollID() As Integer
      Get
         Return _pollId
      End Get
      Set(ByVal value As Integer)
         _pollId = value
      End Set
   End Property


   Private _questionText As String
   Public Property QuestionText() As String
      Get
         Return _questionText
      End Get
      Set(ByVal value As String)
         _questionText = value
      End Set
   End Property


   Private _options As List(Of PollOption)
   Public Property Options() As List(Of PollOption)
      Get
         Return _options
      End Get
      Set(ByVal value As List(Of PollOption))
         _options = value
      End Set
   End Property


End Class
