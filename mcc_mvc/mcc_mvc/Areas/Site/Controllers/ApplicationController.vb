Imports MCC.Services
Imports MCC.Data

Public Class ApplicationController
   Inherits System.Web.Mvc.Controller

   Private _userservice As IUserService = New UserService()
   Private _articlecategoryservice As IArticleCategoryService = New ArticleCategoryService
   Private _menu As MenuViewModel
   Public Sub New()
        Dim users As List(Of SiteUser) = _userservice.GetUsers(0, 12, UserFilterType.Username, "")
      _menu = New MenuViewModel()
      _menu.Money = _articlecategoryservice.GetChildrenCategories("money")
      _menu.News = _articlecategoryservice.GetChildrenCategories("news")
      _menu.Stories = _articlecategoryservice.GetChildrenCategories("stories")
      _menu.Rants = _articlecategoryservice.GetChildrenCategories("rants")
      ViewData("FeaturedUsers") = users
      ViewData("Menu") = _menu
   End Sub
End Class
