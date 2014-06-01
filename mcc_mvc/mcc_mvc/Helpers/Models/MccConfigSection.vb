Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Configuration
Imports System.Web.Configuration
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls

Public Class MCCSection
   Inherits ConfigurationSection

   <ConfigurationProperty("defaultConnectionStringName", DefaultValue:="LocalSqlServer")> _
   Public Property DefaultConnectionStringName() As String
      Get
         Return CType(MyBase.Item("defaultConnectionStringName"), String)
      End Get
      Set(ByVal value As String)
         MyBase.Item("defaultConnectionStringName") = value
      End Set
   End Property

   <ConfigurationProperty("defaultCacheDuration", DefaultValue:="600")> _
Public Property DefaultCacheDuration() As Integer
      Get
         Return CType(MyBase.Item("defaultCacheDuration"), Integer)
      End Get
      Set(ByVal value As Integer)
         MyBase.Item("defaultCacheDuration") = value
      End Set
   End Property

   <ConfigurationProperty("contactForm", IsRequired:=True)> _
   Public ReadOnly Property ContactForm() As ContactFormElement
      Get
         Return CType(MyBase.Item("contactForm"), ContactFormElement)
      End Get
   End Property

   ' <ConfigurationProperty("articles", IsRequired:=True)> _
   ' Public ReadOnly Property Articles() As ArticlesElement
   '    Get
   '       Return CType(MyBase.Item("articles"), ArticlesElement)
   '    End Get
   ' End Property

   ' <ConfigurationProperty("advices", IsRequired:=True)> _
   'Public ReadOnly Property Advices() As AdvicesElement
   '    Get
   '       Return CType(MyBase.Item("advices"), AdvicesElement)
   '    End Get
   ' End Property


   '      <ConfigurationProperty("polls", IsRequired:=True)> _
   '   Public ReadOnly Property Polls() As PollsElement
   '         Get
   '            Return CType(MyBase.Item("polls"), PollsElement)
   '         End Get
   '      End Property

   '      <ConfigurationProperty("newsletters", IsRequired:=True)> _
   '   Public ReadOnly Property Newsletters() As NewslettersElement
   '         Get
   '            Return CType(MyBase.Item("newsletters"), NewslettersElement)
   '         End Get
   '      End Property

   '      <ConfigurationProperty("forums", IsRequired:=True)> _
   '   Public ReadOnly Property Forums() As ForumsElement
   '         Get
   '            Return CType(MyBase.Item("forums"), ForumsElement)
   '         End Get
   '      End Property

   '      <ConfigurationProperty("store", IsRequired:=True)> _
   '   Public ReadOnly Property Store() As StoreElement
   '         Get
   '            Return CType(MyBase.Item("store"), StoreElement)
   '         End Get
   '      End Property


   '      <ConfigurationProperty("quotes", IsRequired:=True)> _
   'Public ReadOnly Property Quotes() As QuotesElement
   '         Get
   '            Return CType(MyBase.Item("quotes"), QuotesElement)
   '         End Get
   '      End Property

   '      <ConfigurationProperty("messages", IsRequired:=True)> _
   'Public ReadOnly Property Messages() As MessagesElement
   '         Get
   '            Return CType(MyBase.Item("messages"), MessagesElement)
   '         End Get
   '      End Property

   '      <ConfigurationProperty("videos", IsRequired:=True)> _
   '     Public ReadOnly Property Videos() As VideosElement
   '         Get
   '            Return CType(MyBase.Item("videos"), VideosElement)
   '         End Get
   '      End Property

   '      <ConfigurationProperty("homepages", IsRequired:=True)> _
   '      Public ReadOnly Property HomePages() As HomePagesElement
   '         Get
   '            Return CType(MyBase.Item("homepages"), HomePagesElement)
   '         End Get
   '      End Property
End Class

Public Class ContactFormElement
   Inherits ConfigurationElement

   <ConfigurationProperty("mailSubject", DefaultValue:="Mail from MCC: {0}")> _
   Public Property MailSubject() As String
      Get
         Return CType(MyBase.Item("mailSubject"), String)
      End Get
      Set(ByVal value As String)
         MyBase.Item("mailSubject") = value
      End Set
   End Property

   <ConfigurationProperty("mailTo", IsRequired:=True)> _
Public Property MailTo() As String
      Get
         Return CType(MyBase.Item("mailTo"), String)
      End Get
      Set(ByVal value As String)
         MyBase.Item("mailTo") = value
      End Set
   End Property

   <ConfigurationProperty("mailCC")> _
   Public Property MailCC() As String
      Get
         Return CType(MyBase.Item("mailCC"), String)
      End Get
      Set(ByVal value As String)
         MyBase.Item("mailCC") = value
      End Set
   End Property
End Class

Public Class HomePagesElement
   Inherits ConfigurationElement

   <ConfigurationProperty("connectionStringName")> _
   Public Property ConnectionStringName() As String
      Get
         Return CType(MyBase.Item("connectionStringName"), String)
      End Get
      Set(ByVal value As String)
         MyBase.Item("connectionStringName") = value
      End Set
   End Property

   Public ReadOnly Property ConnectionString() As String
      Get
         Dim connStringName As String = (Microsoft.VisualBasic.IIf(String.IsNullOrEmpty(Me.ConnectionStringName), MCCGlobals.Settings.DefaultConnectionStringName, Me.ConnectionStringName))
         Return WebConfigurationManager.ConnectionStrings(connStringName).ConnectionString
      End Get
   End Property

   <ConfigurationProperty("providerType", DefaultValue:="MCC.SiteLayers.SqlClient.SqlHomePagesProvider")> _
 Public Property ProviderType() As String
      Get
         Return CType(MyBase.Item("providerType"), String)
      End Get
      Set(ByVal value As String)
         MyBase.Item("providerType") = value
      End Set
   End Property

   <ConfigurationProperty("ratingLockInterval", DefaultValue:="15")> _
   Public Property RatingLockInterval() As Integer
      Get
         Return CType(MyBase.Item("ratingLockInterval"), Integer)
      End Get
      Set(ByVal value As Integer)
         MyBase.Item("ratingLockInterval") = value
      End Set
   End Property

   <ConfigurationProperty("pageSize", DefaultValue:="10")> _
   Public Property PageSize() As Integer
      Get
         Return CType(MyBase.Item("pageSize"), Integer)
      End Get
      Set(ByVal value As Integer)
         MyBase.Item("pageSize") = value
      End Set
   End Property

   <ConfigurationProperty("enableCaching", DefaultValue:="true")> _
   Public Property EnableCaching() As Boolean
      Get
         Return CType(MyBase.Item("enableCaching"), Boolean)
      End Get
      Set(ByVal value As Boolean)
         MyBase.Item("enableCaching") = value
      End Set
   End Property

   <ConfigurationProperty("cacheDuration")> _
   Public Property CacheDuration() As Integer
      Get
         Dim duration As Integer = CType(MyBase.Item("cacheDuration"), Integer)
         Return (Microsoft.VisualBasic.IIf(duration > 0, duration, MCCGlobals.Settings.DefaultCacheDuration))
      End Get
      Set(ByVal value As Integer)
         MyBase.Item("cacheDuration") = value
      End Set
   End Property
End Class


Public Class ArticlesElement
   Inherits ConfigurationElement

   <ConfigurationProperty("connectionStringName")> _
   Public Property ConnectionStringName() As String
      Get
         Return CType(MyBase.Item("connectionStringName"), String)
      End Get
      Set(ByVal value As String)
         MyBase.Item("connectionStringName") = value
      End Set
   End Property

   Public ReadOnly Property ConnectionString() As String
      Get
         Dim connStringName As String = (Microsoft.VisualBasic.IIf(String.IsNullOrEmpty(Me.ConnectionStringName), MCCGlobals.Settings.DefaultConnectionStringName, Me.ConnectionStringName))
         Return WebConfigurationManager.ConnectionStrings(connStringName).ConnectionString
      End Get
   End Property

   <ConfigurationProperty("providerType", DefaultValue:="MCC.SiteLayers.SqlClient.SqlArticlesProvider")> _
 Public Property ProviderType() As String
      Get
         Return CType(MyBase.Item("providerType"), String)
      End Get
      Set(ByVal value As String)
         MyBase.Item("providerType") = value
      End Set
   End Property

   <ConfigurationProperty("ratingLockInterval", DefaultValue:="15")> _
   Public Property RatingLockInterval() As Integer
      Get
         Return CType(MyBase.Item("ratingLockInterval"), Integer)
      End Get
      Set(ByVal value As Integer)
         MyBase.Item("ratingLockInterval") = value
      End Set
   End Property

   <ConfigurationProperty("pageSize", DefaultValue:="10")> _
   Public Property PageSize() As Integer
      Get
         Return CType(MyBase.Item("pageSize"), Integer)
      End Get
      Set(ByVal value As Integer)
         MyBase.Item("pageSize") = value
      End Set
   End Property

   <ConfigurationProperty("rssItems", DefaultValue:="5")> _
   Public Property RssItems() As Integer
      Get
         Return CType(MyBase.Item("rssItems"), Integer)
      End Get
      Set(ByVal value As Integer)
         MyBase.Item("rssItems") = value
      End Set
   End Property

   <ConfigurationProperty("enableCaching", DefaultValue:="true")> _
   Public Property EnableCaching() As Boolean
      Get
         Return CType(MyBase.Item("enableCaching"), Boolean)
      End Get
      Set(ByVal value As Boolean)
         MyBase.Item("enableCaching") = value
      End Set
   End Property

   <ConfigurationProperty("cacheDuration")> _
   Public Property CacheDuration() As Integer
      Get
         Dim duration As Integer = CType(MyBase.Item("cacheDuration"), Integer)
         Return (Microsoft.VisualBasic.IIf(duration > 0, duration, MCCGlobals.Settings.DefaultCacheDuration))
      End Get
      Set(ByVal value As Integer)
         MyBase.Item("cacheDuration") = value
      End Set
   End Property
End Class


Public Class AdvicesElement
   Inherits ConfigurationElement

   <ConfigurationProperty("connectionStringName")> _
   Public Property ConnectionStringName() As String
      Get
         Return CType(MyBase.Item("connectionStringName"), String)
      End Get
      Set(ByVal value As String)
         MyBase.Item("connectionStringName") = value
      End Set
   End Property

   Public ReadOnly Property ConnectionString() As String
      Get
         Dim connStringName As String = (Microsoft.VisualBasic.IIf(String.IsNullOrEmpty(Me.ConnectionStringName), MCCGlobals.Settings.DefaultConnectionStringName, Me.ConnectionStringName))
         Return WebConfigurationManager.ConnectionStrings(connStringName).ConnectionString
      End Get
   End Property

   <ConfigurationProperty("providerType", DefaultValue:="MCC.SiteLayers.SqlClient.SqlAdvicesProvider")> _
 Public Property ProviderType() As String
      Get
         Return CType(MyBase.Item("providerType"), String)
      End Get
      Set(ByVal value As String)
         MyBase.Item("providerType") = value
      End Set
   End Property

   <ConfigurationProperty("ratingLockInterval", DefaultValue:="15")> _
   Public Property RatingLockInterval() As Integer
      Get
         Return CType(MyBase.Item("ratingLockInterval"), Integer)
      End Get
      Set(ByVal value As Integer)
         MyBase.Item("ratingLockInterval") = value
      End Set
   End Property

   <ConfigurationProperty("pageSize", DefaultValue:="10")> _
   Public Property PageSize() As Integer
      Get
         Return CType(MyBase.Item("pageSize"), Integer)
      End Get
      Set(ByVal value As Integer)
         MyBase.Item("pageSize") = value
      End Set
   End Property

   <ConfigurationProperty("rssItems", DefaultValue:="5")> _
   Public Property RssItems() As Integer
      Get
         Return CType(MyBase.Item("rssItems"), Integer)
      End Get
      Set(ByVal value As Integer)
         MyBase.Item("rssItems") = value
      End Set
   End Property

   <ConfigurationProperty("enableCaching", DefaultValue:="true")> _
   Public Property EnableCaching() As Boolean
      Get
         Return CType(MyBase.Item("enableCaching"), Boolean)
      End Get
      Set(ByVal value As Boolean)
         MyBase.Item("enableCaching") = value
      End Set
   End Property

   <ConfigurationProperty("cacheDuration")> _
   Public Property CacheDuration() As Integer
      Get
         Dim duration As Integer = CType(MyBase.Item("cacheDuration"), Integer)
         Return (Microsoft.VisualBasic.IIf(duration > 0, duration, MCCGlobals.Settings.DefaultCacheDuration))
      End Get
      Set(ByVal value As Integer)
         MyBase.Item("cacheDuration") = value
      End Set
   End Property
End Class


Public Class VideosElement
   Inherits ConfigurationElement

   <ConfigurationProperty("connectionStringName")> _
   Public Property ConnectionStringName() As String
      Get
         Return CType(MyBase.Item("connectionStringName"), String)
      End Get
      Set(ByVal value As String)
         MyBase.Item("connectionStringName") = value
      End Set
   End Property

   Public ReadOnly Property ConnectionString() As String
      Get
         Dim connStringName As String = (Microsoft.VisualBasic.IIf(String.IsNullOrEmpty(Me.ConnectionStringName), MCCGlobals.Settings.DefaultConnectionStringName, Me.ConnectionStringName))
         Return WebConfigurationManager.ConnectionStrings(connStringName).ConnectionString
      End Get
   End Property

   <ConfigurationProperty("providerType", DefaultValue:="MCC.SiteLayers.SqlClient.SqlVideosProvider")> _
 Public Property ProviderType() As String
      Get
         Return CType(MyBase.Item("providerType"), String)
      End Get
      Set(ByVal value As String)
         MyBase.Item("providerType") = value
      End Set
   End Property

   <ConfigurationProperty("ratingLockInterval", DefaultValue:="15")> _
   Public Property RatingLockInterval() As Integer
      Get
         Return CType(MyBase.Item("ratingLockInterval"), Integer)
      End Get
      Set(ByVal value As Integer)
         MyBase.Item("ratingLockInterval") = value
      End Set
   End Property

   <ConfigurationProperty("pageSize", DefaultValue:="10")> _
   Public Property PageSize() As Integer
      Get
         Return CType(MyBase.Item("pageSize"), Integer)
      End Get
      Set(ByVal value As Integer)
         MyBase.Item("pageSize") = value
      End Set
   End Property

   <ConfigurationProperty("rssItems", DefaultValue:="5")> _
   Public Property RssItems() As Integer
      Get
         Return CType(MyBase.Item("rssItems"), Integer)
      End Get
      Set(ByVal value As Integer)
         MyBase.Item("rssItems") = value
      End Set
   End Property

   <ConfigurationProperty("enableCaching", DefaultValue:="true")> _
   Public Property EnableCaching() As Boolean
      Get
         Return CType(MyBase.Item("enableCaching"), Boolean)
      End Get
      Set(ByVal value As Boolean)
         MyBase.Item("enableCaching") = value
      End Set
   End Property

   <ConfigurationProperty("cacheDuration")> _
   Public Property CacheDuration() As Integer
      Get
         Dim duration As Integer = CType(MyBase.Item("cacheDuration"), Integer)
         Return (Microsoft.VisualBasic.IIf(duration > 0, duration, MCCGlobals.Settings.DefaultCacheDuration))
      End Get
      Set(ByVal value As Integer)
         MyBase.Item("cacheDuration") = value
      End Set
   End Property
End Class

Public Class PollsElement
   Inherits ConfigurationElement

   <ConfigurationProperty("connectionStringName")> _
   Public Property ConnectionStringName() As String
      Get
         Return CType(MyBase.Item("connectionStringName"), String)
      End Get
      Set(ByVal value As String)
         MyBase.Item("connectionStringName") = value
      End Set
   End Property

   Public ReadOnly Property ConnectionString() As String
      Get
         Dim connStringName As String = (Microsoft.VisualBasic.IIf(String.IsNullOrEmpty(Me.ConnectionStringName), MCCGlobals.Settings.DefaultConnectionStringName, Me.ConnectionStringName))
         Return WebConfigurationManager.ConnectionStrings(connStringName).ConnectionString
      End Get
   End Property

   <ConfigurationProperty("providerType", DefaultValue:="MCC.SiteLayers.SqlClient.SqlPollsProvider")> _
   Public Property ProviderType() As String
      Get
         Return CType(MyBase.Item("providerType"), String)
      End Get
      Set(ByVal value As String)
         MyBase.Item("providerType") = value
      End Set
   End Property

   <ConfigurationProperty("votingLockInterval", DefaultValue:="15")> _
   Public Property VotingLockInterval() As Integer
      Get
         Return CType(MyBase.Item("votingLockInterval"), Integer)
      End Get
      Set(ByVal value As Integer)
         MyBase.Item("votingLockInterval") = value
      End Set
   End Property

   <ConfigurationProperty("votingLockByCookie", DefaultValue:="true")> _
   Public Property VotingLockByCookie() As Boolean
      Get
         Return CType(MyBase.Item("votingLockByCookie"), Boolean)
      End Get
      Set(ByVal value As Boolean)
         MyBase.Item("votingLockByCookie") = value
      End Set
   End Property

   <ConfigurationProperty("votingLockByIP", DefaultValue:="true")> _
   Public Property VotingLockByIP() As Boolean
      Get
         Return CType(MyBase.Item("votingLockByIP"), Boolean)
      End Get
      Set(ByVal value As Boolean)
         MyBase.Item("votingLockByIP") = value
      End Set
   End Property

   <ConfigurationProperty("archiveIsPublic", DefaultValue:="false")> _
   Public Property ArchiveIsPublic() As Boolean
      Get
         Return CType(MyBase.Item("archiveIsPublic"), Boolean)
      End Get
      Set(ByVal value As Boolean)
         MyBase.Item("archiveIsPublic") = value
      End Set
   End Property

   <ConfigurationProperty("enableCaching", DefaultValue:="true")> _
   Public Property EnableCaching() As Boolean
      Get
         Return CType(MyBase.Item("enableCaching"), Boolean)
      End Get
      Set(ByVal value As Boolean)
         MyBase.Item("enableCaching") = value
      End Set
   End Property

   <ConfigurationProperty("cacheDuration")> _
   Public Property CacheDuration() As Integer
      Get
         Dim duration As Integer = CType(MyBase.Item("cacheDuration"), Integer)
         Return (Microsoft.VisualBasic.IIf(duration > 0, duration, MCCGlobals.Settings.DefaultCacheDuration))
      End Get
      Set(ByVal value As Integer)
         MyBase.Item("cacheDuration") = value
      End Set
   End Property
End Class

Public Class NewslettersElement
   Inherits ConfigurationElement

   <ConfigurationProperty("connectionStringName")> _
   Public Property ConnectionStringName() As String
      Get
         Return CType(MyBase.Item("connectionStringName"), String)
      End Get
      Set(ByVal value As String)
         MyBase.Item("connectionStringName") = value
      End Set
   End Property

   Public ReadOnly Property ConnectionString() As String
      Get
         Dim connStringName As String = (Microsoft.VisualBasic.IIf(String.IsNullOrEmpty(Me.ConnectionStringName), MCCGlobals.Settings.DefaultConnectionStringName, Me.ConnectionStringName))
         Return WebConfigurationManager.ConnectionStrings(connStringName).ConnectionString
      End Get
   End Property

   <ConfigurationProperty("providerType", DefaultValue:="MCC.SiteLayers.SqlClient.SqlNewslettersProvider")> _
   Public Property ProviderType() As String
      Get
         Return CType(MyBase.Item("providerType"), String)
      End Get
      Set(ByVal value As String)
         MyBase.Item("providerType") = value
      End Set
   End Property

   <ConfigurationProperty("fromEmail", IsRequired:=True)> _
Public Property FromEmail() As String
      Get
         Return CType(MyBase.Item("fromEmail"), String)
      End Get
      Set(ByVal value As String)
         MyBase.Item("fromEmail") = value
      End Set
   End Property

   <ConfigurationProperty("fromDisplayName", IsRequired:=True)> _
Public Property FromDisplayName() As String
      Get
         Return CType(MyBase.Item("fromDisplayName"), String)
      End Get
      Set(ByVal value As String)
         MyBase.Item("fromDisplayName") = value
      End Set
   End Property

   <ConfigurationProperty("hideFromArchiveInterval", DefaultValue:="15")> _
   Public Property HideFromArchiveInterval() As Integer
      Get
         Return CType(MyBase.Item("hideFromArchiveInterval"), Integer)
      End Get
      Set(ByVal value As Integer)
         MyBase.Item("hideFromArchiveInterval") = value
      End Set
   End Property

   <ConfigurationProperty("archiveIsPublic", DefaultValue:="false")> _
   Public Property ArchiveIsPublic() As Boolean
      Get
         Return CType(MyBase.Item("archiveIsPublic"), Boolean)
      End Get
      Set(ByVal value As Boolean)
         MyBase.Item("archiveIsPublic") = value
      End Set
   End Property

   <ConfigurationProperty("enableCaching", DefaultValue:="true")> _
   Public Property EnableCaching() As Boolean
      Get
         Return CType(MyBase.Item("enableCaching"), Boolean)
      End Get
      Set(ByVal value As Boolean)
         MyBase.Item("enableCaching") = value
      End Set
   End Property

   <ConfigurationProperty("cacheDuration")> _
   Public Property CacheDuration() As Integer
      Get
         Dim duration As Integer = CType(MyBase.Item("cacheDuration"), Integer)
         Return (Microsoft.VisualBasic.IIf(duration > 0, duration, MCCGlobals.Settings.DefaultCacheDuration))
      End Get
      Set(ByVal value As Integer)
         MyBase.Item("cacheDuration") = value
      End Set
   End Property
End Class

Public Class ForumsElement
   Inherits ConfigurationElement

   <ConfigurationProperty("connectionStringName")> _
   Public Property ConnectionStringName() As String
      Get
         Return CType(MyBase.Item("connectionStringName"), String)
      End Get
      Set(ByVal value As String)
         MyBase.Item("connectionStringName") = value
      End Set
   End Property

   Public ReadOnly Property ConnectionString() As String
      Get
         Dim connStringName As String = (Microsoft.VisualBasic.IIf(String.IsNullOrEmpty(Me.ConnectionStringName), MCCGlobals.Settings.DefaultConnectionStringName, Me.ConnectionStringName))
         Return WebConfigurationManager.ConnectionStrings(connStringName).ConnectionString
      End Get
   End Property

   <ConfigurationProperty("providerType", DefaultValue:="MCC.SiteLayers.SqlClient.SqlForumsProvider")> _
   Public Property ProviderType() As String
      Get
         Return CType(MyBase.Item("providerType"), String)
      End Get
      Set(ByVal value As String)
         MyBase.Item("providerType") = value
      End Set
   End Property

   <ConfigurationProperty("threadsPageSize", DefaultValue:="25")> _
   Public Property ThreadsPageSize() As Integer
      Get
         Return CType(MyBase.Item("threadsPageSize"), Integer)
      End Get
      Set(ByVal value As Integer)
         MyBase.Item("threadsPageSize") = value
      End Set
   End Property

   <ConfigurationProperty("postsPageSize", DefaultValue:="10")> _
Public Property PostsPageSize() As Integer
      Get
         Return CType(MyBase.Item("postsPageSize"), Integer)
      End Get
      Set(ByVal value As Integer)
         MyBase.Item("postsPageSize") = value
      End Set
   End Property

   <ConfigurationProperty("rssItems", DefaultValue:="5")> _
Public Property RssItems() As Integer
      Get
         Return CType(MyBase.Item("rssItems"), Integer)
      End Get
      Set(ByVal value As Integer)
         MyBase.Item("rssItems") = value
      End Set
   End Property

   <ConfigurationProperty("hotThreadPosts", DefaultValue:="25")> _
Public Property HotThreadPosts() As Integer
      Get
         Return CType(MyBase.Item("hotThreadPosts"), Integer)
      End Get
      Set(ByVal value As Integer)
         MyBase.Item("hotThreadPosts") = value
      End Set
   End Property

   <ConfigurationProperty("bronzePosterPosts", DefaultValue:="100")> _
Public Property BronzePosterPosts() As Integer
      Get
         Return CType(MyBase.Item("bronzePosterPosts"), Integer)
      End Get
      Set(ByVal value As Integer)
         MyBase.Item("bronzePosterPosts") = value
      End Set
   End Property

   <ConfigurationProperty("bronzePosterDescription", DefaultValue:="Bronze Poster")> _
Public Property BronzePosterDescription() As String
      Get
         Return CType(MyBase.Item("bronzePosterDescription"), String)
      End Get
      Set(ByVal value As String)
         MyBase.Item("bronzePosterDescription") = value
      End Set
   End Property

   <ConfigurationProperty("silverPosterPosts", DefaultValue:="500")> _
Public Property SilverPosterPosts() As Integer
      Get
         Return CType(MyBase.Item("silverPosterPosts"), Integer)
      End Get
      Set(ByVal value As Integer)
         MyBase.Item("silverPosterPosts") = value
      End Set
   End Property

   <ConfigurationProperty("silverPosterDescription", DefaultValue:="Silver Poster")> _
   Public Property SilverPosterDescription() As String
      Get
         Return CType(MyBase.Item("silverPosterDescription"), String)
      End Get
      Set(ByVal value As String)
         MyBase.Item("silverPosterDescription") = value
      End Set
   End Property

   <ConfigurationProperty("goldPosterPosts", DefaultValue:="1000")> _
   Public Property GoldPosterPosts() As Integer
      Get
         Return CType(MyBase.Item("goldPosterPosts"), Integer)
      End Get
      Set(ByVal value As Integer)
         MyBase.Item("goldPosterPosts") = value
      End Set
   End Property

   <ConfigurationProperty("goldPosterDescription", DefaultValue:="Gold Poster")> _
   Public Property GoldPosterDescription() As String
      Get
         Return CType(MyBase.Item("goldPosterDescription"), String)
      End Get
      Set(ByVal value As String)
         MyBase.Item("goldPosterDescription") = value
      End Set
   End Property

   <ConfigurationProperty("enableCaching", DefaultValue:="true")> _
   Public Property EnableCaching() As Boolean
      Get
         Return CType(MyBase.Item("enableCaching"), Boolean)
      End Get
      Set(ByVal value As Boolean)
         MyBase.Item("enableCaching") = value
      End Set
   End Property

   <ConfigurationProperty("cacheDuration")> _
   Public Property CacheDuration() As Integer
      Get
         Dim duration As Integer = CType(MyBase.Item("cacheDuration"), Integer)
         Return (Microsoft.VisualBasic.IIf(duration > 0, duration, MCCGlobals.Settings.DefaultCacheDuration))
      End Get
      Set(ByVal value As Integer)
         MyBase.Item("cacheDuration") = value
      End Set
   End Property
End Class

Public Class StoreElement
   Inherits ConfigurationElement

   <ConfigurationProperty("connectionStringName")> _
   Public Property ConnectionStringName() As String
      Get
         Return CType(MyBase.Item("connectionStringName"), String)
      End Get
      Set(ByVal value As String)
         MyBase.Item("connectionStringName") = value
      End Set
   End Property

   Public ReadOnly Property ConnectionString() As String
      Get
         Dim connStringName As String = (Microsoft.VisualBasic.IIf(String.IsNullOrEmpty(Me.ConnectionStringName), MCCGlobals.Settings.DefaultConnectionStringName, Me.ConnectionStringName))
         Return WebConfigurationManager.ConnectionStrings(connStringName).ConnectionString
      End Get
   End Property

   <ConfigurationProperty("providerType", DefaultValue:="MCC.SiteLayers.SqlClient.SqlStoreProvider")> _
   Public Property ProviderType() As String
      Get
         Return CType(MyBase.Item("providerType"), String)
      End Get
      Set(ByVal value As String)
         MyBase.Item("providerType") = value
      End Set
   End Property

   <ConfigurationProperty("ratingLockInterval", DefaultValue:="15")> _
   Public Property RatingLockInterval() As Integer
      Get
         Return CType(MyBase.Item("ratingLockInterval"), Integer)
      End Get
      Set(ByVal value As Integer)
         MyBase.Item("ratingLockInterval") = value
      End Set
   End Property

   <ConfigurationProperty("pageSize", DefaultValue:="10")> _
   Public Property PageSize() As Integer
      Get
         Return CType(MyBase.Item("pageSize"), Integer)
      End Get
      Set(ByVal value As Integer)
         MyBase.Item("pageSize") = value
      End Set
   End Property

   <ConfigurationProperty("rssItems", DefaultValue:="5")> _
   Public Property RssItems() As Integer
      Get
         Return CType(MyBase.Item("rssItems"), Integer)
      End Get
      Set(ByVal value As Integer)
         MyBase.Item("rssItems") = value
      End Set
   End Property

   <ConfigurationProperty("defaultOrderListInterval", DefaultValue:="7")> _
   Public Property DefaultOrderListInterval() As Integer
      Get
         Return CType(MyBase.Item("defaultOrderListInterval"), Integer)
      End Get
      Set(ByVal value As Integer)
         MyBase.Item("defaultOrderListInterval") = value
      End Set
   End Property

   <ConfigurationProperty("sandboxMode", DefaultValue:="false")> _
Public Property SandboxMode() As Boolean
      Get
         Return CType(MyBase.Item("sandboxMode"), Boolean)
      End Get
      Set(ByVal value As Boolean)
         MyBase.Item("sandboxMode") = value
      End Set
   End Property

   <ConfigurationProperty("businessEmail", IsRequired:=True)> _
Public Property BusinessEmail() As String
      Get
         Return CType(MyBase.Item("businessEmail"), String)
      End Get
      Set(ByVal value As String)
         MyBase.Item("businessEmail") = value
      End Set
   End Property

   <ConfigurationProperty("currencyCode", DefaultValue:="USD")> _
Public Property CurrencyCode() As String
      Get
         Return CType(MyBase.Item("currencyCode"), String)
      End Get
      Set(ByVal value As String)
         MyBase.Item("currencyCode") = value
      End Set
   End Property

   <ConfigurationProperty("lowAvailability", DefaultValue:="10")> _
Public Property LowAvailability() As Integer
      Get
         Return CType(MyBase.Item("lowAvailability"), Integer)
      End Get
      Set(ByVal value As Integer)
         MyBase.Item("lowAvailability") = value
      End Set
   End Property

   <ConfigurationProperty("enableCaching", DefaultValue:="true")> _
Public Property EnableCaching() As Boolean
      Get
         Return CType(MyBase.Item("enableCaching"), Boolean)
      End Get
      Set(ByVal value As Boolean)
         MyBase.Item("enableCaching") = value
      End Set
   End Property

   <ConfigurationProperty("cacheDuration")> _
   Public Property CacheDuration() As Integer
      Get
         Dim duration As Integer = CType(MyBase.Item("cacheDuration"), Integer)
         Return (Microsoft.VisualBasic.IIf(duration > 0, duration, MCCGlobals.Settings.DefaultCacheDuration))
      End Get
      Set(ByVal value As Integer)
         MyBase.Item("cacheDuration") = value
      End Set
   End Property
End Class

Public Class QuotesElement
   Inherits ConfigurationElement

   <ConfigurationProperty("connectionStringName")> _
   Public Property ConnectionStringName() As String
      Get
         Return CType(MyBase.Item("connectionStringName"), String)
      End Get
      Set(ByVal value As String)
         MyBase.Item("connectionStringName") = value
      End Set
   End Property

   Public ReadOnly Property ConnectionString() As String
      Get
         Dim connStringName As String = (Microsoft.VisualBasic.IIf(String.IsNullOrEmpty(Me.ConnectionStringName), MCCGlobals.Settings.DefaultConnectionStringName, Me.ConnectionStringName))
         Return WebConfigurationManager.ConnectionStrings(connStringName).ConnectionString
      End Get
   End Property

   <ConfigurationProperty("providerType", DefaultValue:="MCC.SiteLayers.SqlClient.SqlQuotesProvider")> _
 Public Property ProviderType() As String
      Get
         Return CType(MyBase.Item("providerType"), String)
      End Get
      Set(ByVal value As String)
         MyBase.Item("providerType") = value
      End Set
   End Property

   <ConfigurationProperty("ratingLockInterval", DefaultValue:="15")> _
   Public Property RatingLockInterval() As Integer
      Get
         Return CType(MyBase.Item("ratingLockInterval"), Integer)
      End Get
      Set(ByVal value As Integer)
         MyBase.Item("ratingLockInterval") = value
      End Set
   End Property

   <ConfigurationProperty("pageSize", DefaultValue:="10")> _
   Public Property PageSize() As Integer
      Get
         Return CType(MyBase.Item("pageSize"), Integer)
      End Get
      Set(ByVal value As Integer)
         MyBase.Item("pageSize") = value
      End Set
   End Property

   <ConfigurationProperty("rssItems", DefaultValue:="5")> _
   Public Property RssItems() As Integer
      Get
         Return CType(MyBase.Item("rssItems"), Integer)
      End Get
      Set(ByVal value As Integer)
         MyBase.Item("rssItems") = value
      End Set
   End Property

   <ConfigurationProperty("enableCaching", DefaultValue:="true")> _
   Public Property EnableCaching() As Boolean
      Get
         Return CType(MyBase.Item("enableCaching"), Boolean)
      End Get
      Set(ByVal value As Boolean)
         MyBase.Item("enableCaching") = value
      End Set
   End Property

   <ConfigurationProperty("cacheDuration")> _
   Public Property CacheDuration() As Integer
      Get
         Dim duration As Integer = CType(MyBase.Item("cacheDuration"), Integer)
         Return (Microsoft.VisualBasic.IIf(duration > 0, duration, MCCGlobals.Settings.DefaultCacheDuration))
      End Get
      Set(ByVal value As Integer)
         MyBase.Item("cacheDuration") = value
      End Set
   End Property
End Class

Public Class MessagesElement
   Inherits ConfigurationElement

   <ConfigurationProperty("connectionStringName")> _
   Public Property ConnectionStringName() As String
      Get
         Return CType(MyBase.Item("connectionStringName"), String)
      End Get
      Set(ByVal value As String)
         MyBase.Item("connectionStringName") = value
      End Set
   End Property

   Public ReadOnly Property ConnectionString() As String
      Get
         Dim connStringName As String = (Microsoft.VisualBasic.IIf(String.IsNullOrEmpty(Me.ConnectionStringName), MCCGlobals.Settings.DefaultConnectionStringName, Me.ConnectionStringName))
         Return WebConfigurationManager.ConnectionStrings(connStringName).ConnectionString
      End Get
   End Property

   <ConfigurationProperty("providerType", DefaultValue:="MCC.SiteLayers.SqlClient.SqlMessagesProvider")> _
 Public Property ProviderType() As String
      Get
         Return CType(MyBase.Item("providerType"), String)
      End Get
      Set(ByVal value As String)
         MyBase.Item("providerType") = value
      End Set
   End Property

   <ConfigurationProperty("ratingLockInterval", DefaultValue:="15")> _
   Public Property RatingLockInterval() As Integer
      Get
         Return CType(MyBase.Item("ratingLockInterval"), Integer)
      End Get
      Set(ByVal value As Integer)
         MyBase.Item("ratingLockInterval") = value
      End Set
   End Property

   <ConfigurationProperty("pageSize", DefaultValue:="10")> _
   Public Property PageSize() As Integer
      Get
         Return CType(MyBase.Item("pageSize"), Integer)
      End Get
      Set(ByVal value As Integer)
         MyBase.Item("pageSize") = value
      End Set
   End Property

   <ConfigurationProperty("rssItems", DefaultValue:="5")> _
   Public Property RssItems() As Integer
      Get
         Return CType(MyBase.Item("rssItems"), Integer)
      End Get
      Set(ByVal value As Integer)
         MyBase.Item("rssItems") = value
      End Set
   End Property

   <ConfigurationProperty("enableCaching", DefaultValue:="true")> _
   Public Property EnableCaching() As Boolean
      Get
         Return CType(MyBase.Item("enableCaching"), Boolean)
      End Get
      Set(ByVal value As Boolean)
         MyBase.Item("enableCaching") = value
      End Set
   End Property

   <ConfigurationProperty("cacheDuration")> _
   Public Property CacheDuration() As Integer
      Get
         Dim duration As Integer = CType(MyBase.Item("cacheDuration"), Integer)
         Return (Microsoft.VisualBasic.IIf(duration > 0, duration, MCCGlobals.Settings.DefaultCacheDuration))
      End Get
      Set(ByVal value As Integer)
         MyBase.Item("cacheDuration") = value
      End Set
   End Property
End Class
