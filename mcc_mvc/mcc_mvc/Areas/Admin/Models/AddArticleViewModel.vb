Imports MCC.Data
Imports MCC.Services
Imports System.ComponentModel.DataAnnotations
Imports Webdiyer.WebControls.Mvc
Public Class AddArticleViewModel
   Inherits baseViewModel

   Private _articleservice As IArticleService
   Private _articleCategoryservice As IArticleCategoryService
   Private _videoservice As IVideoService
   Private _pollservice As IPollService
   Private _adservice As IAdService

   Public Sub New()
      Me.New(0, New ArticleService, New ArticleCategoryService, New VideoService, New PollService, New AdService)
   End Sub

   Public Sub New(ByVal Id As Integer, ByVal articlesrvr As IArticleService, ByVal articlecatsrvr As IArticleCategoryService, _
                  ByVal videosrvr As IVideoService, ByVal pollsrvr As IPollService, ByVal adsrvr As IAdService)
      _articleID = Id
      _articleservice = articlesrvr
      _articleCategoryservice = articlecatsrvr
      _videoservice = videosrvr
      _pollservice = pollsrvr
      _adservice = adsrvr

      InitData()
   End Sub

   ''' <summary>
   ''' Initialize some select items
   ''' </summary>
   ''' <remarks></remarks>
   Sub InitData()
      If _articleID > 0 Then
         _categoryIds = _articleCategoryservice.GetCategoriesByArticleId(_articleID).Select(Function(p) p.CategoryID).ToList
         _adIds = _adservice.GetAdsByArticle(_articleID).Select(Function(p) p.AdID).ToList
      Else
         _addedBy = HttpContext.Current.User.Identity.Name
      End If
   End Sub


#Region " 1. DTO for View use"

   Private _categories As List(Of ArticleCategory)
   Public Property Categories() As List(Of ArticleCategory)
      Get
         If _categories Is Nothing Then
            _categories = _articleCategoryService.GetCategories() '.ToSelectList(Function(f) f.CategoryID, Function(f) f.Title, "")
         End If
         Return _categories
      End Get
      Set(ByVal value As List(Of ArticleCategory))
         _categories = value
      End Set
   End Property


   Private _ads As PagedList(Of Ad)
   Public Property Ads() As PagedList(Of Ad)
      Get
         If _ads Is Nothing Then
            _ads = _adservice.GetAds(0, 10)
         End If
         Return _ads
      End Get
      Set(ByVal value As PagedList(Of Ad))
         _ads = value
      End Set
   End Property


   Private _videos As List(Of Video)
   Public Property Videos() As List(Of Video)
      Get
         If _videos Is Nothing AndAlso _articleID > 0 Then
            _videos = _videoservice.GetVideos
         End If
         Return _videos
      End Get
      Set(ByVal value As List(Of Video))
         _videos = value
      End Set
   End Property


   Private _polls As List(Of SelectListItem)
   Public Property Polls() As List(Of SelectListItem)
      Get
         If _polls Is Nothing Then
            _polls = _pollservice.GetPolls.ToSelectList(Function(p) p.QuestionText, Function(p) p.PollID, "- please select a poll -")
         End If
         Return _polls
      End Get
      Set(ByVal value As List(Of SelectListItem))
         _polls = value
      End Set
   End Property



   Private _countries As List(Of SelectListItem)
   Public Property Countries() As List(Of SelectListItem)
      Get
         If _countries Is Nothing Then
            _countries = mccHelpers.GetCountries.ToSelectList(Function(p) p, Function(p) p, "- please select a country -")
         End If
         Return _countries
      End Get
      Set(ByVal value As List(Of SelectListItem))
         _countries = value
      End Set
   End Property



   Private _authors As List(Of SelectListItem)
   Public Property Authors() As List(Of SelectListItem)
      Get
         If _authors Is Nothing Then
            _authors = Roles.GetUsersInRole("Editors").ToSelectList(Function(p) p, Function(p) p, "- please select an author -")
         End If
         Return _authors
      End Get
      Set(ByVal value As List(Of SelectListItem))
         _authors = value
      End Set
   End Property

#End Region

#Region "2. Article Info"


   'Private _cats As List(Of CheckBoxListInfo)
   'Public Property NewProperty() As List(Of CheckBoxListInfo)
   '   Get
   '      Return _cats
   '   End Get
   '   Set(ByVal value As List(Of CheckBoxListInfo))
   '      _cats = value
   '   End Set
   'End Property


   Public _categoryIds As List(Of Integer) = New List(Of Integer)
   Public Property CategoryIds() As List(Of Integer)
      Get
         If _categoryIds Is Nothing Then
            _categoryIds = New List(Of Integer)
         End If
         Return _categoryIds
      End Get
      Set(ByVal value As List(Of Integer))
         _categoryIds = value
      End Set
   End Property


   Private _addedBy As String = String.Empty
   Public Property AddedBy() As String
      Get
         Return _addedBy
      End Get
      Set(ByVal value As String)
         _addedBy = value
      End Set
   End Property

   Private _adIds As List(Of Integer)
   Public Property AdIds() As List(Of Integer)
      Get
         If _adIds Is Nothing Then
            _adIds = New List(Of Integer)
         End If
         Return _adIds
      End Get
      Set(ByVal value As List(Of Integer))
         _adIds = value
      End Set
   End Property

   Private _articleID As Integer = 0
   Public Property ArticleID() As Integer
      Get
         Return _articleID
      End Get
      Set(ByVal value As Integer)
         _articleID = value
      End Set
   End Property

   Private _title As String = String.Empty
   <Required(ErrorMessage:="Please Enter the article title")> _
   Public Property Title() As String
      Get
         Return _title
      End Get
      Set(ByVal value As String)
         _title = value
      End Set
   End Property



   Private _body As String = String.Empty
   <Required(ErrorMessage:="Please Enter the article body")> _
   Public Property Body() As String
      Get
         Return _body
      End Get
      Set(ByVal value As String)
         _body = value
      End Set
   End Property



   Private _abstract As String = String.Empty
   <Required(ErrorMessage:="Please Enter the abstract")> _
   Public Property Abstract() As String
      Get
         Return _abstract
      End Get
      Set(ByVal value As String)
         _abstract = value
      End Set
   End Property


   Private _city As String = String.Empty
   Public Property City() As String
      Get
         Return _city
      End Get
      Set(ByVal value As String)
         _city = value
      End Set
   End Property


   Private _state As String = String.Empty
   Public Property State() As String
      Get
         Return _state
      End Get
      Set(ByVal value As String)
         _state = value
      End Set
   End Property


   Private _country As String = String.Empty
   Public Property Country() As String
      Get
         Return _country
      End Get
      Set(ByVal value As String)
         _country = value
      End Set
   End Property



   Private _listed As Boolean = False
   Public Property Listed() As Boolean
      Get
         Return _listed
      End Get
      Set(ByVal value As Boolean)
         _listed = value
      End Set
   End Property


   Private _onlyForMembers As Boolean = False
   Public Property OnlyForMembers() As Boolean
      Get
         Return _onlyForMembers
      End Get
      Set(ByVal value As Boolean)
         _onlyForMembers = value
      End Set
   End Property


   Private _ImageNewsUrl As String = String.Empty
   <Required(ErrorMessage:="Please set the article full size image")> _
   Public Property ImageNewsUrl() As String
      Get
         Return _ImageNewsUrl
      End Get
      Set(ByVal value As String)
         _ImageNewsUrl = value
      End Set
   End Property


   Private _imageIconUrl As String = String.Empty
   <Required(ErrorMessage:="Please set the article mini image")> _
   Public Property ImageIconUrl() As String
      Get
         Return _imageIconUrl
      End Get
      Set(ByVal value As String)
         _imageIconUrl = value
      End Set
   End Property



   Private _releaseDate As DateTime
   <Required(ErrorMessage:="Please Enter a release date")> _
   Public Property ReleaseDate() As DateTime
      Get
         Return _releaseDate
      End Get
      Set(ByVal value As DateTime)
         _releaseDate = value
      End Set
   End Property


   Private _expireDate As DateTime
   <Required(ErrorMessage:="Please Enter an expiration date")> _
   Public Property ExpireDate() As DateTime
      Get
         Return _expireDate
      End Get
      Set(ByVal value As DateTime)
         _expireDate = value
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

   Private _commentsEnabled As Boolean = False
   Public Property CommentsEnabled() As Boolean
      Get
         Return _commentsEnabled
      End Get
      Set(ByVal value As Boolean)
         _commentsEnabled = value
      End Set
   End Property

   Private _imageID As Integer? = 0
   Public Property ImageID() As Integer?
      Get
         Return _imageID
      End Get
      Set(ByVal value As Integer?)
         _imageID = value
      End Set
   End Property

   Private _videoId As Integer
   Public Property VideoId() As Integer
      Get
         Return _videoId
      End Get
      Set(ByVal value As Integer)
         _videoId = value
      End Set
   End Property

   Private _pollId As Integer
   Public Property PollId() As Integer
      Get
         Return _pollId
      End Get
      Set(ByVal value As Integer)
         _pollId = value
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


#End Region

End Class
