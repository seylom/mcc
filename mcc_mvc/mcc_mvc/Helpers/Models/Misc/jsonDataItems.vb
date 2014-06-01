Imports Microsoft.VisualBasic
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Json
Imports System.IO
Imports MCC.Data


Public Class ArticleCategoryJSON
   Public Sub New()

   End Sub

   Public Sub New(ByVal cat As ArticleCategory)
      Title = cat.Title
      Description = cat.Description
      Importance = cat.Importance
      ImageUrl = cat.ImageUrl
      CategoryID = cat.CategoryID
      ParentCategoryID = cat.ParentCategoryID
   End Sub

   Private _name As String = ""

   Public Property Title() As String
      Get
         Return _name
      End Get
      Set(ByVal value As String)
         _name = value
      End Set
   End Property

   Private _description As String = ""
   Public Property Description() As String
      Get
         Return _description
      End Get
      Set(ByVal value As String)
         _description = value
      End Set
   End Property

   Private _id As Integer = 0
   Public Property CategoryID() As Integer
      Get
         Return _id
      End Get
      Set(ByVal value As Integer)
         _id = value
      End Set
   End Property


   Private _parentId As Integer = 0

   <DataMember()> _
   Public Property ParentCategoryID() As Integer
      Get
         Return _parentId
      End Get
      Set(ByVal value As Integer)
         _parentId = value
      End Set
   End Property

   Private _imageurl As String = ""
   Public Property ImageUrl() As String
      Get
         Return _imageurl
      End Get
      Set(ByVal value As String)
         _imageurl = value
      End Set
   End Property

   Private _importance As Integer = 0
   Public Property Importance() As Integer
      Get
         Return _importance
      End Get
      Set(ByVal value As Integer)
         _importance = value
      End Set
   End Property

End Class

Public Class ImageJSON
   Public Sub New()

   End Sub

   Public Sub New(ByVal im As SimpleImage)
      ImageID = im.ImageID
      Description = im.Description
      Name = im.Name
        Uploaded = im.AddedDate.ToString
      Tags = routines.Decode(im.Tags)
      CreditsName = im.CreditsName
      CreditsUrl = im.CreditsUrl
      ImageUrl = routines.AbsUrl(im.ImageUrl)
      _uuid = im.uuid
   End Sub

   Private _name As String = ""

   Public Property Name() As String
      Get
         Return _name
      End Get
      Set(ByVal value As String)
         _name = value
      End Set
   End Property

   Private _description As String = ""
   Public Property Description() As String
      Get
         Return _description
      End Get
      Set(ByVal value As String)
         _description = value
      End Set
   End Property

   Private _id As Integer = 0
   Public Property ImageID() As Integer
      Get
         Return _id
      End Get
      Set(ByVal value As Integer)
         _id = value
      End Set
   End Property

   Private _type As Integer
   Public Property Type() As String
      Get
         Return _type
      End Get
      Set(ByVal value As String)
         _type = value
      End Set
   End Property

   Private _tags As String = ""
   Public Property Tags() As String
      Get
         Return _tags
      End Get
      Set(ByVal value As String)
         _tags = value
      End Set
   End Property

   Private _creditsName As String = ""
   Public Property CreditsName() As String
      Get
         Return _creditsName
      End Get
      Set(ByVal value As String)
         _creditsName = value
      End Set
   End Property

   Private _creditsUrl As String = ""
   Public Property CreditsUrl() As String
      Get
         Return _creditsUrl
      End Get
      Set(ByVal value As String)
         _creditsUrl = value
      End Set
   End Property

   Private _uploaded As String
   Public Property Uploaded() As String
      Get
         Return _uploaded
      End Get
      Set(ByVal value As String)
         _uploaded = value
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

   Private _uuid As String
   Public Property Uuid() As String
      Get
         Return _uuid
      End Get
      Set(ByVal value As String)
         _uuid = value
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

   Private _miniImageUrl As String
   Public Property MiniImageUrl() As String
      Get
         Return _miniImageUrl
      End Get
      Set(ByVal value As String)
         _miniImageUrl = value
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
End Class

Public Class AdJSON
   Public Sub New()

   End Sub

   Public Sub New(ByVal a As Ad)
      AdID = a.AdId
      Description = a.Description
      Title = a.Title
      Body = a.Body
      Keywords = a.Keywords
      Approved = a.Approved
      Author = a.Addedby
      Type = a.Type
   End Sub


   Private _title As String = ""
   Public Property Title() As String
      Get
         Return _title
      End Get
      Set(ByVal value As String)
         _title = value
      End Set
   End Property

   Private _addedby As String = ""
   Public Property Author() As String
      Get
         Return _addedby
      End Get
      Set(ByVal value As String)
         _addedby = value
      End Set
   End Property

   Private _approved As Boolean = False
   Public Property Approved() As Boolean
      Get
         Return _approved
      End Get
      Set(ByVal value As Boolean)
         _approved = value
      End Set
   End Property


   Private _description As String = ""
   Public Property Description() As String
      Get
         Return _description
      End Get
      Set(ByVal value As String)
         _description = value
      End Set
   End Property

   Private _id As Integer = 0
   Public Property AdID() As Integer
      Get
         Return _id
      End Get
      Set(ByVal value As Integer)
         _id = value
      End Set
   End Property

   Private _keywords As String = ""
   Public Property Keywords() As String
      Get
         Return _keywords
      End Get
      Set(ByVal value As String)
         _keywords = value
      End Set
   End Property


   Private _type As Integer = 0
   Public Property Type() As Integer
      Get
         Return _type
      End Get
      Set(ByVal value As Integer)
         _type = value
      End Set
   End Property


   Private _body As String = ""
   Public Property Body() As String
      Get
         Return _body
      End Get
      Set(ByVal value As String)
         _body = value
      End Set
   End Property

End Class

Public Class VideoJSON
   Public Sub New()

   End Sub

   Public Sub New(ByVal a As Video)
      VideoID = a.VideoId
      Abstract = a.Abstract
      Title = a.Title
      File = Configs.Paths.CdnRoot & Configs.Paths.CdnVideos & a.VideoUrl
      ImageUrl = Configs.Paths.CdnRoot & Configs.Paths.CdnVideos & a.Name & "/default.jpg"
      Duration = routines.ToMinutesAndSeconds(a.Duration)
      Tags = a.Tags
      Author = a.AddedBy
      Uploaded = a.AddedDate.ToString
      Approved = a.Approved
   End Sub


   Private _tile As String = ""
   Public Property Title() As String
      Get
         Return _tile
      End Get
      Set(ByVal value As String)
         _tile = value
      End Set
   End Property


   Private _abstract As String = ""
   Public Property Abstract() As String
      Get
         Return _abstract
      End Get
      Set(ByVal value As String)
         _abstract = value
      End Set
   End Property

   Private _addedby As String = ""
   Public Property Author() As String
      Get
         Return _addedby
      End Get
      Set(ByVal value As String)
         _addedby = value
      End Set
   End Property

   Private _duration As String = ""
   Public Property Duration() As String
      Get
         Return _duration
      End Get
      Set(ByVal value As String)
         _duration = value
      End Set
   End Property

   Private _Id As String = ""
   Public Property VideoID() As String
      Get
         Return _Id
      End Get
      Set(ByVal value As String)
         _Id = value
      End Set
   End Property

   Private _file As String = ""
   Public Property File() As String
      Get
         Return _file
      End Get
      Set(ByVal value As String)
         _file = value
      End Set
   End Property

   Private _imageUrl As String = ""
   Public Property ImageUrl() As String
      Get
         Return _imageUrl
      End Get
      Set(ByVal value As String)
         _imageUrl = value
      End Set
   End Property

   Private _tags As String = ""
   Public Property Tags() As String
      Get
         Return _tags
      End Get
      Set(ByVal value As String)
         _tags = value
      End Set
   End Property

   Private _addeddate As String = ""
   Public Property Uploaded() As String
      Get
         Return _addeddate
      End Get
      Set(ByVal value As String)
         _addeddate = value
      End Set
   End Property

   Private _approved As Boolean = False
   Public Property Approved() As String
      Get
         Return _approved
      End Get
      Set(ByVal value As String)
         _approved = value
      End Set
   End Property

End Class

Public Class QuoteJSON
   Public Sub New()

   End Sub

   Public Sub New(ByVal qt As Quote)
      QuoteID = qt.QuoteId
      Description = qt.Body
      Author = qt.Author
      Role = qt.Role
   End Sub

   Private _Id As String = ""
    Public Property QuoteID() As Integer
        Get
            Return _Id
        End Get
        Set(ByVal value As Integer)
            _Id = value
        End Set
    End Property

   Private _description As String = ""
   Public Property Description() As String
      Get
         Return _description
      End Get
      Set(ByVal value As String)
         _description = value
      End Set
   End Property


   Private _author As String = ""
   Public Property Author() As String
      Get
         Return _author
      End Get
      Set(ByVal value As String)
         _author = value
      End Set
   End Property

   Private _role As String = ""
   Public Property Role() As String
      Get
         Return _role
      End Get
      Set(ByVal value As String)
         _role = value
      End Set
   End Property

End Class

Public Class VoteJSON

   Public Sub New(ByVal id As Integer, ByVal votes As Integer, ByVal success As Boolean, ByVal message As String)
      Me.Id = id
      Me.Votes = votes
      Me.Success = success
      Me.Message = message
   End Sub

   Public Sub New()

   End Sub

   Private _success As Boolean = False
   Public Property Success() As Boolean
      Get
         Return _success
      End Get
      Set(ByVal value As Boolean)
         _success = value
      End Set
   End Property

   Private _message As String = ""
   Public Property Message() As String
      Get
         Return _message
      End Get
      Set(ByVal value As String)
         _message = value
      End Set
   End Property


   Private _votes As Integer = 0
   Public Property Votes() As Integer
      Get
         Return _votes
      End Get
      Set(ByVal value As Integer)
         _votes = value
      End Set
   End Property


   Private _id As String = -1
   Public Property Id() As String
      Get
         Return _id
      End Get
      Set(ByVal value As String)
         _id = value
      End Set
   End Property


End Class


Public Class UserJSON
   Public Sub New()

   End Sub

   Public Sub New(ByVal uname As String, ByVal email As String, ByVal approved As Boolean, ByVal creationDate As DateTime, ByVal lastActivity As DateTime)
      Me.Username = uname
      Me.Email = email
      Me.Approved = approved
      Me.CreationDate = creationDate
      Me.LastActivityDate = lastActivity
   End Sub



   Private _username As String
   Public Property Username() As String
      Get
         Return _username
      End Get
      Set(ByVal value As String)
         _username = value
      End Set
   End Property


   Private _email As String
   Public Property Email() As String
      Get
         Return _email
      End Get
      Set(ByVal value As String)
         _email = value
      End Set
   End Property


   Private _approved As String
   Public Property Approved() As String
      Get
         Return _approved
      End Get
      Set(ByVal value As String)
         _approved = value
      End Set
   End Property


   Private _creationDate As DateTime
   Public Property CreationDate() As DateTime
      Get
         Return _creationDate
      End Get
      Set(ByVal value As DateTime)
         _creationDate = value
      End Set
   End Property


   Private _lastActivity As DateTime
   Public Property LastActivityDate() As DateTime
      Get
         Return _lastActivity
      End Get
      Set(ByVal value As DateTime)
         _lastActivity = value
      End Set
   End Property

End Class

Public Class CommentJSON

   Private _id As Integer
   ''' <summary>
   ''' Parent Id of the Instance we add comment to.
   ''' </summary>
   ''' <value></value>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Public Property Id() As Integer
      Get
         Return _id
      End Get
      Set(ByVal value As Integer)
         _id = value
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

   Private _success As Boolean
   Public Property Success() As Boolean
      Get
         Return _success
      End Get
      Set(ByVal value As Boolean)
         _success = value
      End Set
   End Property

   Private _addedBy As String
   Public Property AddedBy() As String
      Get
         Return _addedBy
      End Get
      Set(ByVal value As String)
         _addedBy = value
      End Set
   End Property



End Class


Public Class JSONHelper
   Public Shared Function Serialize(Of T)(ByVal obj As T) As String
      Dim serializer As New System.Runtime.Serialization.Json.DataContractJsonSerializer(obj.[GetType]())
      Dim ms As New MemoryStream()
      serializer.WriteObject(ms, obj)
      Dim retVal As String = Encoding.[Default].GetString(ms.ToArray())
      Return retVal
   End Function

   Public Shared Function Deserialize(Of T)(ByVal json As String) As T
      Dim obj As T = Activator.CreateInstance(Of T)()
      Dim ms As New MemoryStream(Encoding.Unicode.GetBytes(json))
      Dim serializer As New System.Runtime.Serialization.Json.DataContractJsonSerializer(obj.[GetType]())
      obj = DirectCast(serializer.ReadObject(ms), T)
      ms.Close()
      Return obj
   End Function
End Class
