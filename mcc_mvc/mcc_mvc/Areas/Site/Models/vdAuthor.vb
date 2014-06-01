Imports MCC.Data
Imports Webdiyer.WebControls.Mvc
Public Class vdAuthor
   Inherits baseViewModel



   Private _infoCard As AuthorInfoCard
   Public Property InfoCard() As AuthorInfoCard
      Get
         Return _infoCard
      End Get
      Set(ByVal value As AuthorInfoCard)
         _infoCard = value
      End Set
   End Property



   Private _username As String
   Public Property Username() As String
      Get
         Return _username
      End Get
      Set(ByVal value As String)
         _username = value
      End Set
   End Property



   Private _articles As PagedList(Of ArticleAdv)
   Public Property articles() As PagedList(Of ArticleAdv)
      Get
         Return _articles
      End Get
      Set(ByVal value As PagedList(Of ArticleAdv))
         _articles = value
      End Set
   End Property

   Private _categories As PagedList(Of ArticleCategory)
   Public Property NewProperty() As PagedList(Of ArticleCategory)
      Get
         Return _categories
      End Get
      Set(ByVal value As PagedList(Of ArticleCategory))
         _categories = value
      End Set
   End Property



End Class
