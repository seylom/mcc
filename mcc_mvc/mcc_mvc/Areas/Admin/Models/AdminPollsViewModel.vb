Imports MCC.Data
Imports MCC.Services
Imports Webdiyer.WebControls.Mvc
Public Class AdminPollsViewModel
   Inherits baseViewModel

   Private _pollservice As IPollService
   Public Sub New()
      Me.New(0, 30, New PollService)
   End Sub

   Public Sub New(ByVal pageIndex As Integer, ByVal pageSize As Integer)
      Me.New(pageIndex, pageSize, New PollService)
   End Sub

   Public Sub New(ByVal pageIndex As Integer, ByVal pageSize As Integer, ByVal pollservr As IPollService)
      _pageIndex = pageIndex
      _pageSize = pageSize
      _pollservice = pollservr

      _polls = _pollservice.GetPolls(pageIndex, pageSize)
   End Sub


   Private _polls As PagedList(Of Poll)
   Public Property Polls() As PagedList(Of Poll)
      Get
         Return _polls
      End Get
      Set(ByVal value As PagedList(Of Poll))
         _polls = value
      End Set
   End Property


   Private _pageIndex As Integer
   Public Property PageIndex() As Integer
      Get
         Return _pageIndex
      End Get
      Set(ByVal value As Integer)
         _pageIndex = value
      End Set
   End Property



   Private _pageSize As Integer
   Public Property PageSize() As Integer
      Get
         Return _pageSize
      End Get
      Set(ByVal value As Integer)
         _pageSize = value
      End Set
   End Property


End Class
