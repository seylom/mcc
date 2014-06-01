Public Class CreateThumbsViewModel
   Inherits baseViewModel


   Private _imageUrl As String
   Public Property ImageUrl() As String
      Get
         Return _imageUrl
      End Get
      Set(ByVal value As String)
         _imageUrl = value
      End Set
   End Property


   Private _baseImageUrl As String
   Public Property BaseImageUrl() As String
      Get
         Return _baseImageUrl
      End Get
      Set(ByVal value As String)
         _baseImageUrl = value
      End Set
   End Property

   Private _miniUrl As String
   Public Property MiniImageUrl() As String
      Get
         Return _miniUrl
      End Get
      Set(ByVal value As String)
         _miniUrl = value
      End Set
   End Property


   Private _longImageUrl As String
   Public Property LongImageUrl() As String
      Get
         Return _longImageUrl
      End Get
      Set(ByVal value As String)
         _longImageUrl = value
      End Set
   End Property



   Private _largeImageUrl As String
   Public Property LargeImageUrl() As String
      Get
         Return _largeImageUrl
      End Get
      Set(ByVal value As String)
         _largeImageUrl = value
      End Set
   End Property


   Private _imageID As Integer
   Public Property ImageID() As Integer
      Get
         Return _imageID
      End Get
      Set(ByVal value As Integer)
         _imageID = value
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

   Private _tags As String
   Public Property Tags() As String
      Get
         Return _tags
      End Get
      Set(ByVal value As String)
         _tags = value
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

   Private _name As String
   Public Property Name() As String
      Get
         Return _name
      End Get
      Set(ByVal value As String)
         _name = value
      End Set
   End Property

   Private _cbCreateMini As Boolean
   Public Property CreateMini() As Boolean
      Get
         Return _cbCreateMini
      End Get
      Set(ByVal value As Boolean)
         _cbCreateMini = value
      End Set
   End Property

   Private _cbCreateLong As Boolean
   Public Property CreateLong() As Boolean
      Get
         Return _cbCreateLong
      End Get
      Set(ByVal value As Boolean)
         _cbCreateLong = value
      End Set
   End Property


   Private _createLarge As Boolean
   Public Property CreateLarge() As Boolean
      Get
         Return _createLarge
      End Get
      Set(ByVal value As Boolean)
         _createLarge = value
      End Set
   End Property

#Region "Large"

   Private _x_large As Integer
   Public Property X_Large() As Integer
      Get
         Return _x_large
      End Get
      Set(ByVal value As Integer)
         _x_large = value
      End Set
   End Property

   Private _y_large As Integer
   Public Property Y_Large() As Integer
      Get
         Return _y_large
      End Get
      Set(ByVal value As Integer)
         _y_large = value
      End Set
   End Property


   Private _w_large As Integer
   Public Property W_Large() As Integer
      Get
         Return _w_large
      End Get
      Set(ByVal value As Integer)
         _w_large = value
      End Set
   End Property

   Private _h_large As Integer
   Public Property H_Large() As Integer
      Get
         Return _h_large
      End Get
      Set(ByVal value As Integer)
         _h_large = value
      End Set
   End Property

#End Region

#Region "Long"

   Private _x_long As Integer
   Public Property X_Long() As Integer
      Get
         Return _x_long
      End Get
      Set(ByVal value As Integer)
         _x_long = value
      End Set
   End Property

   Private _y_long As Integer
   Public Property Y_Long() As Integer
      Get
         Return _y_long
      End Get
      Set(ByVal value As Integer)
         _y_long = value
      End Set
   End Property


   Private _w_long As Integer
   Public Property W_Long() As Integer
      Get
         Return _w_long
      End Get
      Set(ByVal value As Integer)
         _w_long = value
      End Set
   End Property

   Private _h_long As Integer
   Public Property H_Long() As Integer
      Get
         Return _h_long
      End Get
      Set(ByVal value As Integer)
         _h_long = value
      End Set
   End Property

#End Region

#Region "Mini"

   Private _x_mini As Integer
   Public Property X_Mini() As Integer
      Get
         Return _x_mini
      End Get
      Set(ByVal value As Integer)
         _x_mini = value
      End Set
   End Property

   Private _y_mini As Integer
   Public Property Y_Mini() As Integer
      Get
         Return _y_mini
      End Get
      Set(ByVal value As Integer)
         _y_mini = value
      End Set
   End Property


   Private _w_mini As Integer
   Public Property W_Mini() As Integer
      Get
         Return _w_mini
      End Get
      Set(ByVal value As Integer)
         _w_mini = value
      End Set
   End Property

   Private _h_mini As Integer
   Public Property H_Mini() As Integer
      Get
         Return _h_mini
      End Get
      Set(ByVal value As Integer)
         _h_mini = value
      End Set
   End Property

#End Region

End Class
