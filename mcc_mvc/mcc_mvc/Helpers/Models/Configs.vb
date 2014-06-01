Imports Microsoft.VisualBasic

Namespace Configs.Paths
   Public Module Paths

#Region "CDN paths"
      Public ReadOnly Property CdnRoot() As String
         Get
            Return Utils.AssetPath("cdn.root")
         End Get
      End Property

      Public ReadOnly Property CdnFtpRoot() As String
         Get
            Return Utils.AssetPath("cdn.ftp.root")
         End Get
      End Property

      Public ReadOnly Property CdnImages() As String
         Get
            Return Utils.AssetPath("path.images")
         End Get
      End Property

      Public ReadOnly Property CdnVideos() As String
         Get
            Return Utils.AssetPath("path.videos")
         End Get
      End Property

      Public ReadOnly Property CdnUploads() As String
         Get
            Return Utils.AssetPath("path.uploads")
         End Get
      End Property

      Public ReadOnly Property CdnAvatars() As String
         Get
            Return Utils.AssetPath("path.avatars")
         End Get
      End Property

#End Region

#Region "Assets path"
      Public ReadOnly Property Css() As String
         Get
            Return Utils.AssetPath("path.assets.css")
         End Get
      End Property

      Public ReadOnly Property Images() As String
         Get
            Return Utils.AssetPath("path.assets.images")
         End Get
      End Property

      Public ReadOnly Property Scripts() As String
         Get
            Return Utils.AssetPath("path.assets.scripts")
         End Get
      End Property

      Public ReadOnly Property Themes() As String
         Get
            Return Utils.AssetPath("path.assets.themes")
         End Get
      End Property

      Public ReadOnly Property AssetsVideos() As String
         Get
            Return Utils.AssetPath("path.assets.videos")
         End Get
      End Property
#End Region
   End Module
End Namespace



Namespace Configs.Uploads
   Public Module Uploads

      Public ReadOnly Property MaxVideoSize() As Integer
         Get
            Dim str As String = Utils.AssetPath("uploads.videos.maxsize")
            Dim val As Integer = 0
            If Not Integer.TryParse(str, val) Then
               val = 50000000  ' 50MB
            End If

            Return val
         End Get
      End Property

      Public ReadOnly Property MaxImageSize() As Integer
         Get
            Dim str As String = Utils.AssetPath("uploads.image.maxsize")
            Dim val As Integer = 0
            If Not Integer.TryParse(str, val) Then
               val = 100000  ' 100k
            End If

            Return val
         End Get
      End Property

   End Module
End Namespace


Namespace Configs.Folders
   Module Folders
      Public ReadOnly Property ImagesFolder() As String
         Get
            Return Utils.AssetPath("folder.images")
         End Get
      End Property


      Public ReadOnly Property VideosFolder() As String
         Get
            Return Utils.AssetPath("folder.videos")
         End Get
      End Property
      Public ReadOnly Property AvatarsFolder() As String
         Get
            Return Utils.AssetPath("folder.avatars")
         End Get
      End Property
   End Module
End Namespace

Namespace Configs.Credentials
   Module Credentials
      Public ReadOnly Property FTPUserId() As String
         Get
            Return Utils.AssetPath("ftp.userid")
         End Get
      End Property

      Public ReadOnly Property FTPPassword() As String
         Get
            Return Utils.AssetPath("ftp.password")
         End Get
      End Property

   End Module
End Namespace
