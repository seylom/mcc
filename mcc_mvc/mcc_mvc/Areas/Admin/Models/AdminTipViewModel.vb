Imports MCC.Data
Imports MCC.Services

Public Class AdminTipViewModel
   Inherits baseViewModel

   Private _advicecategoryservice As IAdviceCategoryService
   Private _adviceservice As IAdviceService

   Public Sub New()
      Me.New(New AdviceService, New AdviceCategoryService)
   End Sub

   Public Sub New(ByVal advicesrvr As IAdviceService, ByVal advicecatservr As IAdviceCategoryService)
      _adviceservice = advicesrvr
      _advicecategoryservice = advicecatservr
   End Sub

   Private _Categories As List(Of AdviceCategory)
   Public Property Categories() As List(Of AdviceCategory)
      Get
         Return _Categories
      End Get
      Set(ByVal value As List(Of AdviceCategory))
         _Categories = value
      End Set
   End Property

   Private _categoryIds As List(Of Integer)
   Public Property CategoryIds() As List(Of Integer)
      Get
         Return _categoryIds
      End Get
      Set(ByVal value As List(Of Integer))
         _categoryIds = value
      End Set
   End Property

   Private _id As Integer
   Public Property AdviceID() As Integer
      Get
         Return _id
      End Get
      Set(ByVal value As Integer)
         _id = value
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


   Private _Body As String
   Public Property Body() As String
      Get
         Return _Body
      End Get
      Set(ByVal value As String)
         _Body = value
      End Set
   End Property


   Private _approved As Boolean
   Public Property Approved() As Boolean
      Get
         Return _approved
      End Get
      Set(ByVal value As Boolean)
         _approved = value
      End Set
   End Property


   Private _listed As Boolean
   Public Property Listed() As Boolean
      Get
         Return _listed
      End Get
      Set(ByVal value As Boolean)
         _listed = value
      End Set
   End Property


   Private _onlyforMembers As Boolean
   Public Property OnlyForMembers() As Boolean
      Get
         Return _onlyforMembers
      End Get
      Set(ByVal value As Boolean)
         _onlyforMembers = value
      End Set
   End Property

   Private _tags As String
   Public Property Tags() As String
      Get
         Return _tags
      End Get
      Set(ByVal value As String)
         _tags = value
      End Set
   End Property

   Private _commentsEnabled As Boolean
   Public Property CommentsEnabled() As Boolean
      Get
         Return _commentsEnabled
      End Get
      Set(ByVal value As Boolean)
         _commentsEnabled = value
      End Set
   End Property


   Private _abstract As String
   Public Property Abstract() As String
      Get
         Return _abstract
      End Get
      Set(ByVal value As String)
         _abstract = value
      End Set
   End Property
End Class
