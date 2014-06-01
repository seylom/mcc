Public Class ProfileInfo
   Inherits ProfileBase


   Public Property DisplayName() As String
      Get
         Return CType(Me.GetPropertyValue("DisplayName"), String)
      End Get
      Set(ByVal value As String)
         Me.SetPropertyValue("DisplayName", value)
      End Set
   End Property

   Private _about As String
   Public Property About() As String
      Get
         Return CType(Me.GetPropertyValue("About"), String)
      End Get
      Set(ByVal value As String)
         Me.SetPropertyValue("About", value)
      End Set
   End Property


   Private _website As String
   Public Property Website() As String
      Get
         Return CType(Me.GetPropertyValue("Website"), String)
      End Get
      Set(ByVal value As String)
         Me.SetPropertyValue("Website", value)
      End Set
   End Property

   Private _occupation As String
   Public Property Occupation() As String
      Get
         Return CType(Me.GetPropertyValue("Occupation"), String)
      End Get
      Set(ByVal value As String)
         Me.SetPropertyValue("Occupation", value)
      End Set
   End Property


   Private _imageUrl As String
   Public Property ImageUrl() As String
      Get
         Return CType(Me.GetPropertyValue("ImageUrl"), String)
      End Get
      Set(ByVal value As String)
         Me.SetPropertyValue("ImageUrl", value)
      End Set
   End Property


   Public ReadOnly Property Location() As ProfileGroupLocation
      Get
         Return CType(Me.GetPropertyValue("Location"), ProfileGroupLocation)
      End Get
   End Property



   Public Shared Function GetProfile(ByVal username As String) As ProfileInfo
      Return CType(Create(username), ProfileInfo)
   End Function

End Class
