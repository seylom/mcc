Imports Microsoft.VisualBasic

Imports System
Imports System.Data
Imports System.Configuration
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports System.Collections.Generic
Imports MCC.SiteLayers
Imports MCC.routines
Imports System.Linq
Imports Dynamic
Imports MCC.Data

Namespace Videos
   Public Class VideoCategoryRepository
      Inherits mccObject

      Public Shared Function GetCategoriesCount() As Integer
         Dim key As String = "VideoCategory_VideoCategorycount"
         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            Dim mdc As MCCDataContext = New MCCDataContext
            Dim i As Integer = mdc.mcc_VideoCategories.Count()
            CacheData(key, i)
            Return i
         End If
      End Function

      Public Shared Function GetCategories() As List(Of mcc_VideoCategory)
         Dim key As String = "VideoCategory_VideoCategory"
         If Cache(key) IsNot Nothing Then
            Return DirectCast(Cache(key), List(Of mcc_VideoCategory))
         Else
            Dim mdc As MCCDataContext = New MCCDataContext
            Dim li As List(Of mcc_VideoCategory) = (From t As mcc_VideoCategory In mdc.mcc_VideoCategories Select t).ToList
            CacheData(key, li)
            Return li
         End If
      End Function


      Public Shared Function GetCategories(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, Optional ByVal sort As String = "Title ASC") As List(Of mcc_VideoCategory)
         Dim key As String = "VideoCategory_VideoCategory" & startRowIndex & "_" & maximumRows & "_"
         If Cache(key) IsNot Nothing Then
            Return DirectCast(Cache(key), List(Of mcc_VideoCategory))
         Else
            Dim mdc As MCCDataContext = New MCCDataContext
            Dim li As New List(Of mcc_VideoCategory)
            If startRowIndex > 0 AndAlso maximumRows > 0 Then
               li = (From t As mcc_VideoCategory In mdc.mcc_VideoCategories).Skip(startRowIndex - 1).Take(startRowIndex * maximumRows).ToList
            Else
               li = (From t As mcc_VideoCategory In mdc.mcc_VideoCategories).Skip(0).Take(20).ToList
            End If
            CacheData(key, li)
            Return li
         End If
      End Function

      Public Shared Function GetCategoryById(ByVal categoryId As Integer) As mcc_VideoCategory
         Dim key As String = "VideoCategory_VideoCategory_" & categoryId.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Dim c As mcc_VideoCategory = DirectCast(Cache(key), mcc_VideoCategory)
            Return c
         Else
            Dim mdc As New MCCDataContext()
            Dim c As mcc_VideoCategory
            If mdc.mcc_VideoCategories.Count(Function(p) p.CategoryId = categoryId) > 0 Then
               c = (From it As mcc_VideoCategory In mdc.mcc_VideoCategories Where it.CategoryId = categoryId).FirstOrDefault
               CacheData(key, c)
               Return c
            Else
               Return Nothing
            End If
         End If
      End Function

      Public Shared Function GetCategoryBySlug(ByVal slug As String) As mcc_VideoCategory
         Dim key As String = "VideoCategory_VideoCategory_" & slug & "_"
         If Cache(key) IsNot Nothing Then
            Dim c As mcc_VideoCategory = DirectCast(Cache(key), mcc_VideoCategory)
            Return c
         Else
            Dim mdc As New MCCDataContext()
            Dim c As mcc_VideoCategory
            If mdc.mcc_VideoCategories.Count(Function(p) p.Slug = slug) > 0 Then
               c = (From it As mcc_VideoCategory In mdc.mcc_VideoCategories Where it.Slug = slug).FirstOrDefault
               CacheData(key, c)
               Return c
            Else
               Return Nothing
            End If
         End If
      End Function

      Public Shared Function GetCategoriesByVideoId(ByVal VideoId As Integer) As List(Of mcc_VideoCategory)
         Dim categories As List(Of mcc_VideoCategory) = Nothing
         Dim key As String = "Videos_Specified_Categories_" + VideoId.ToString

         If mccObject.Cache(key) IsNot Nothing Then
            categories = DirectCast(mccObject.Cache(key), List(Of mcc_VideoCategory))
         Else
            Dim mdc As New MCCDataContext()
            Dim i As Integer = mdc.mcc_CategoriesVideos.Count(Function(p) p.VideoId = VideoId)
            If i > 0 Then
               Dim li As List(Of Integer) = (From it As mcc_CategoriesVideo In mdc.mcc_CategoriesVideos Where it.VideoId = VideoId Select it.CategoryId).ToList
               categories = (From c As mcc_VideoCategory In mdc.mcc_VideoCategories Where li.Contains(c.CategoryId)).ToList
               CacheData(key, categories)
            End If
         End If
         Return categories
      End Function

      Public Shared Sub InsertCategory(ByVal title As String, ByVal importance As Integer, ByVal description As String, ByVal imageUrl As String, ByVal parentCategoryId As Integer)
         Dim mdc As MCCDataContext = New MCCDataContext
         Dim c As New mcc_VideoCategory

         With c
            .AddedDate = DateTime.Now
            .AddedBy = mccObject.CurrentUserName
            .Title = title
            .Importance = importance
            .Description = description
            .ImageUrl = imageUrl
            .Slug = routines.GetSlugFromString(title)
            .ParentCategoryID = parentCategoryId
         End With

         mdc.mcc_VideoCategories.InsertOnSubmit(c)
         mdc.SubmitChanges()
         mccObject.PurgeCacheItems("VideoCategory_")
      End Sub


      Public Shared Sub UpdateCategory(ByVal CategoryId As Integer, ByVal title As String, ByVal importance As Integer, ByVal description As String, ByVal imageUrl As String, ByVal parentCategoryId As Integer)
         Dim mdc As MCCDataContext = New MCCDataContext
         Dim c As mcc_VideoCategory = (From t In mdc.mcc_VideoCategories _
                                     Where t.CategoryId = CategoryId _
                                     Select t).Single()
         If c IsNot Nothing Then
            With c
               .Title = title
               .Description = description
               .Importance = importance
               .ImageUrl = imageUrl
               .Slug = routines.GetSlugFromString(title)
               .ParentCategoryID = parentCategoryId
            End With
            mdc.SubmitChanges()
            mccObject.PurgeCacheItems("VideoCategory_VideoCategory_")
         End If
      End Sub


      Public Shared Sub DeleteCategory(ByVal CategoryID As Integer)
         Dim mdc As MCCDataContext = New MCCDataContext
         Dim wrd As mcc_VideoCategory = (From t In mdc.mcc_VideoCategories _
                                     Where t.CategoryId = CategoryID _
                                     Select t).Single()

         If wrd IsNot Nothing Then
            mdc.mcc_VideoCategories.DeleteOnSubmit(wrd)
            Dim cs As System.Data.Linq.ChangeSet = mdc.GetChangeSet()
            mdc.SubmitChanges()
            mccObject.PurgeCacheItems("VideoCategory_")
         End If
      End Sub

      Protected Shared Sub CacheData(ByVal key As String, ByVal data As Object)
         If data IsNot Nothing Then
            mccObject.Cache.Insert(key, data, Nothing, DateTime.Now.AddSeconds(3600), TimeSpan.Zero)
         End If
      End Sub

   End Class
End Namespace

'Namespace MCC.SiteItems.Videos
'   Public Class VideoCategory
'      Inherits BaseVideo

'      Private _title As String = ""
'      Public Property Title() As String
'         Get
'            Return _title
'         End Get
'         Set(ByVal value As String)
'            _title = value
'         End Set
'      End Property

'      Private _importance As Integer = 0
'      Public Property Importance() As Integer
'         Get
'            Return _importance
'         End Get
'         Private Set(ByVal value As Integer)
'            _importance = value
'         End Set
'      End Property

'      Private _description As String = ""
'      Public Property Description() As String
'         Get
'            Return _description
'         End Get
'         Set(ByVal value As String)
'            _description = value
'         End Set
'      End Property

'      Private _imageUrl As String = ""
'      Public Property ImageUrl() As String
'         Get
'            Return _imageUrl
'         End Get
'         Set(ByVal value As String)
'            _imageUrl = value
'         End Set
'      End Property

'      Private _slug As String = ""
'      Public Property Slug() As String
'         Get
'            If Not String.IsNullOrEmpty(_slug) Then
'               Return _slug
'            Else
'               _slug = GetSlugFromString(Title)
'               Return _slug
'            End If
'         End Get
'         Set(ByVal value As String)
'            _slug = value
'         End Set
'      End Property

'      Private _parentCategoryId As Integer
'      Public Property ParentCategoryId() As Integer
'         Get
'            Return _parentCategoryId
'         End Get
'         Set(ByVal value As Integer)
'            _parentCategoryId = value
'         End Set
'      End Property

'      Private _allVideos As List(Of Video) = Nothing
'      Public ReadOnly Property AllVideos() As List(Of Video)
'         Get
'            If _allVideos Is Nothing Then
'               _allVideos = Video.GetVideos(Me.ID, 0, mccObject.MaxRows)
'            End If
'            Return _allVideos
'         End Get
'      End Property

'      Private _publishedVideos As List(Of Video) = Nothing
'      Public ReadOnly Property PublishedVideos() As List(Of Video)
'         Get
'            If _publishedVideos Is Nothing Then
'               _publishedVideos = Video.GetVideos(True, Me.ID, 0, mccObject.MaxRows)
'            End If
'            Return _publishedVideos
'         End Get
'      End Property

'      Public Sub New(ByVal id As Integer, ByVal addedDate As DateTime, ByVal addedBy As String, ByVal title As String, ByVal importance As Integer, ByVal description As String, _
'       ByVal imageUrl As String, ByVal parentCategoryId As Integer)
'         Me.ID = id
'         Me.AddedDate = addedDate
'         Me.AddedBy = addedBy
'         Me.Title = title
'         Me.Importance = importance
'         Me.Description = description
'         Me.ImageUrl = imageUrl
'         Me.ParentCategoryId = parentCategoryId
'      End Sub

'      Public Sub New(ByVal mccCategory As CategoryDetails)
'         Me.ID = mccCategory.ID
'         Me.AddedDate = mccCategory.AddedDate
'         Me.AddedBy = mccCategory.AddedBy
'         Me.Title = mccCategory.Title
'         Me.Importance = mccCategory.Importance
'         Me.Description = mccCategory.Description
'         Me.ImageUrl = mccCategory.ImageUrl
'         Me.ParentCategoryId = mccCategory.ParentCategoryId
'      End Sub

'      Public Function Delete() As Boolean
'         Dim success As Boolean = VideoCategory.DeleteCategory(Me.ID)
'         If success Then
'            Me.ID = 0
'         End If
'         Return success
'      End Function

'      Public Function Update() As Boolean
'         Return VideoCategory.UpdateCategory(Me.ID, Me.Title, Me.Importance, Me.Description, Me.ImageUrl, Me.Slug, Me.ParentCategoryId)
'         'Return Category.UpdateCategory(Me.mCategory)
'      End Function

'      '**********************************
'      '      * Static methods
'      '      ***********************************


'      ''' <summary>
'      ''' Returns a collection with all the categories
'      ''' </summary>
'      Public Shared Function GetCategories() As List(Of VideoCategory)
'         Dim categories As List(Of VideoCategory) = Nothing
'         Dim key As String = "Videos_Categories"

'         If BaseVideo.Settings.EnableCaching AndAlso mccObject.Cache(key) IsNot Nothing Then
'            categories = DirectCast(mccObject.Cache(key), List(Of VideoCategory))
'         Else
'            Dim recordset As List(Of CategoryDetails) = SiteProvider.Videos.GetCategories()
'            categories = GetCategoryListFromCategoryDetailsList(recordset)
'            BaseVideo.CacheData(key, categories)
'         End If
'         Return categories
'      End Function

'      ''' <summary>
'      ''' Returns a Category object with the specified ID
'      ''' </summary>
'      Public Shared Function GetCategoryByID(ByVal categoryID As Integer) As VideoCategory
'         Dim category As VideoCategory = Nothing
'         Dim key As String = "Videos_Category_" + categoryID.ToString()

'         If BaseVideo.Settings.EnableCaching AndAlso mccObject.Cache(key) IsNot Nothing Then
'            category = DirectCast(mccObject.Cache(key), VideoCategory)
'         Else
'            category = GetCategoryFromCategoryDetails(SiteProvider.Videos.GetCategoryByID(categoryID))
'            BaseVideo.CacheData(key, category)
'         End If
'         Return category
'      End Function

'      ''' <summary>
'      ''' Returns an Category object with the specified slug
'      ''' </summary>
'      Public Shared Function GetCategoryBySlug(ByVal slug As String) As VideoCategory
'         Dim cat As VideoCategory = Nothing
'         Dim key As String = "Videos_Category_" + slug.ToString()

'         If BaseVideo.Settings.EnableCaching AndAlso mccObject.Cache(key) IsNot Nothing Then
'            cat = DirectCast(mccObject.Cache(key), VideoCategory)
'         Else
'            cat = GetCategoryFromCategoryDetails(SiteProvider.Videos.GetCategoryBySlug(slug))
'            VideoCategory.CacheData(key, cat)
'         End If
'         Return cat
'      End Function

'      Public Shared Function GetCategoriesByVideoId(ByVal VideoId As Integer) As List(Of VideoCategory)
'         Dim categories As List(Of VideoCategory) = Nothing
'         Dim key As String = "Videos_Specified_Categories_" + VideoId.ToString

'         If BaseVideo.Settings.EnableCaching AndAlso mccObject.Cache(key) IsNot Nothing Then
'            categories = DirectCast(mccObject.Cache(key), List(Of VideoCategory))
'         Else
'            Dim recordset As List(Of CategoryDetails) = SiteProvider.Videos.GetCategoriesByID(VideoId)
'            categories = GetCategoryListFromCategoryDetailsList(recordset)
'            BaseVideo.CacheData(key, categories)
'         End If
'         Return categories
'      End Function


'      ''' <summary>
'      ''' Updates an existing category
'      ''' </summary>
'      Public Shared Function UpdateCategory(ByVal mccCategory As CategoryDetails) As Boolean
'         mccCategory.AddedDate = DateTime.Now
'         mccCategory.ImageUrl = mccObject.ConvertNullToEmptyString(mccCategory.ImageUrl)
'         Dim ret As Boolean = SiteProvider.Videos.UpdateCategory(mccCategory)
'         mccObject.PurgeCacheItems("Videos_categor")
'         Return ret
'      End Function

'      ''' <summary>
'      ''' Updates an existing category
'      ''' </summary>
'      Public Shared Function UpdateCategory(ByVal id As Integer, ByVal title As String, ByVal importance As Integer, ByVal description As String, ByVal imageUrl As String, ByVal slug As String, ByVal parentCategoryId As Integer) As Boolean
'         Dim record As New CategoryDetails(id, DateTime.Now, "", title, importance, description, imageUrl, slug, parentCategoryId)
'         record.ImageUrl = mccObject.ConvertNullToEmptyString(imageUrl)
'         Dim ret As Boolean = SiteProvider.Videos.UpdateCategory(record)
'         mccObject.PurgeCacheItems("Videos_categor")
'         Return ret
'      End Function

'      ''' <summary>
'      ''' Deletes an existing category
'      ''' </summary>
'      Public Shared Function DeleteCategory(ByVal id As Integer) As Boolean
'         Dim ret As Boolean = SiteProvider.Videos.DeleteCategory(id)
'         Dim tb As MCCEvents.MCCEvents.RecordDeletedEvent = New MCCEvents.MCCEvents.RecordDeletedEvent("category", id, Nothing)
'         tb.Raise()
'         mccObject.PurgeCacheItems("Videos_categor")
'         Return ret
'      End Function

'      ''' <summary>
'      ''' Creates a new category
'      ''' </summary>
'      Public Shared Function InsertCategory(ByVal mccCategory As CategoryDetails) As Integer
'         mccCategory.AddedDate = DateTime.Now
'         mccCategory.ImageUrl = mccObject.ConvertNullToEmptyString(mccCategory.ImageUrl)
'         Dim ret As Integer = SiteProvider.Videos.InsertCategory(mccCategory)
'         mccObject.PurgeCacheItems("Videos_categor")
'         Return ret
'      End Function

'      ''' <summary>
'      ''' Creates a new category
'      ''' </summary>
'      Public Shared Function InsertCategory(ByVal title As String, ByVal importance As Integer, ByVal description As String, ByVal imageUrl As String, ByVal slug As String, ByVal parentCategoryId As Integer) As Integer
'         Dim record As New CategoryDetails(0, DateTime.Now, mccObject.CurrentUserName, title, importance, description, imageUrl, slug, parentCategoryId)

'         record.ImageUrl = mccObject.ConvertNullToEmptyString(record.ImageUrl)

'         Dim ret As Integer = SiteProvider.Videos.InsertCategory(record)
'         mccObject.PurgeCacheItems("Videos_categor")
'         Return ret
'      End Function

'      ''' <summary>
'      ''' Returns a Category object filled with the data taken from the input CategoryDetails
'      ''' </summary>
'      Private Shared Function GetCategoryFromCategoryDetails(ByVal record As CategoryDetails) As VideoCategory
'         If record Is Nothing Then
'            Return Nothing
'         Else
'            Return New VideoCategory(record)
'         End If
'      End Function

'      ''' <summary>
'      ''' Returns a list of Category objects filled with the data taken from the input list of CategoryDetails
'      ''' </summary>
'      Private Shared Function GetCategoryListFromCategoryDetailsList(ByVal recordset As List(Of CategoryDetails)) As List(Of VideoCategory)
'         Dim categories As New List(Of VideoCategory)()
'         For Each record As CategoryDetails In recordset
'            categories.Add(GetCategoryFromCategoryDetails(record))
'         Next
'         Return categories
'      End Function
'   End Class
'End Namespace
