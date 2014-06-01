Imports MCC.Data
Imports MCC.Services

Public Class AdminImageViewModel
   Inherits baseViewModel

   'Private _imageservice As IImageService

   'Public Sub New()
   '   Me.New(New ImageService)
   'End Sub

   'Public Sub New(ByVal imagesrvr As IImageService)
   '   _imageservice = imagesrvr
   'End Sub

   Private _imageId As Integer
   Public Property ImageID() As Integer
      Get
         Return _imageId
      End Get
      Set(ByVal value As Integer)
         _imageId = value
      End Set
   End Property

   Private _creditsUrl As String
   Public Property CreditsUrl() As String
      Get
         Return _creditsUrl
      End Get
      Set(ByVal value As String)
         _creditsUrl = value
      End Set
   End Property

   Private _creditsName As String
   Public Property CreditsName() As String
      Get
         Return _creditsName
      End Get
      Set(ByVal value As String)
         _creditsName = value
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


   Private _Uuid As String
   Public Property Uuid() As String
      Get
         Return _Uuid
      End Get
      Set(ByVal value As String)
         _Uuid = value
      End Set
   End Property


   Private _url As String
   Public Property ImageUrl() As String
      Get
         Return _url
      End Get
      Set(ByVal value As String)
         _url = value
      End Set
   End Property


End Class
