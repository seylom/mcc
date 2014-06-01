Public Class AdminUpdateVideoViewModel
   Inherits baseViewModel

   Private _videoId As Integer
   Public Property VideoID() As Integer
      Get
         Return _videoId
      End Get
      Set(ByVal value As Integer)
         _videoId = value
      End Set
   End Property


   Private _name As String
   Public Property Name() As String
      Get
         Return _name
      End Get
      Set(ByVal value As String)
         _name = value
      End Set
   End Property



   Private _videourl As String
   Public Property VideoUrl() As String
      Get
         Return _videourl
      End Get
      Set(ByVal value As String)
         _videourl = value
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


   Private _UploadFile As String
   Public Property UploadFile() As String
      Get
         Return _UploadFile
      End Get
      Set(ByVal value As String)
         _UploadFile = value
      End Set
   End Property


   Private _uploadResults As List(Of String)
   Public Property UploadResults() As List(Of String)
      Get
         Return _uploadResults
      End Get
      Set(ByVal value As List(Of String))
         _uploadResults = value
      End Set
   End Property


End Class
