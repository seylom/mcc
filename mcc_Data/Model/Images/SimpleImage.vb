
Public Enum ImageType As Integer
   Undefined = 0
   Article = 1
   VideoStill = 2
End Enum

Public Class SimpleImage


   Private _ImageId As Integer
   Public Property ImageID() As Integer
      Get
         Return _ImageId
      End Get
      Set(ByVal value As Integer)
         _ImageId = value
      End Set
   End Property

   Private _imageUrl As String
   Public Property ImageUrl() As String
      Get
         Return _imageUrl
      End Get
      Set(ByVal value As String)
         _imageUrl = value
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



   Private _creditsUrl As String
   Public Property CreditsUrl() As String
      Get
         Return _creditsUrl
      End Get
      Set(ByVal value As String)
         _creditsUrl = value
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


   Private _addedDate As DateTime
   Public Property AddedDate() As DateTime
      Get
         Return _addedDate
      End Get
      Set(ByVal value As DateTime)
         _addedDate = value
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


   Private _uuid As String
   Public Property Uuid() As String
      Get
         Return _uuid
      End Get
      Set(ByVal value As String)
         _uuid = value
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

   Private _type As Integer
   Public Property ImageType() As Integer
      Get
         Return _type
      End Get
      Set(ByVal value As Integer)
         _type = value
      End Set
   End Property


   Function Copy() As SimpleImage
      Dim it As New SimpleImage With {.ImageID = Me.ImageID, _
                                      .AddedDate = Me.AddedDate, _
                                      .Name = Me.Name, _
                                      .CreditsName = Me.CreditsName, _
                                      .CreditsUrl = Me.CreditsUrl, _
                                      .ImageType = Me.ImageType, _
                                      .ImageUrl = Me.ImageUrl, _
                                      .Description = Me.Description, _
                                      .Tags = Me.Tags, _
                                      .Uuid = Me.Uuid}
      Return it
   End Function
End Class
