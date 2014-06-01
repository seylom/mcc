Imports MCC.Data
Imports System.Runtime.CompilerServices

<HideModuleName()> _
Module ModelMapper

#Region "Articles"


   <Extension()> _
   Function FillDTO(ByVal ar As AddArticleViewModel, ByRef outArticle As Article) As Article

      With outArticle
         .ArticleID = ar.ArticleID
         .ReleaseDate = ar.ReleaseDate
         .ExpireDate = ar.ExpireDate
         .Abstract = ar.Abstract
         .AddedBy = ar.AddedBy
         .Body = ar.Body
         .Title = ar.Title
         .Tags = ar.Tags
         .CommentsEnabled = ar.CommentsEnabled
         .Listed = ar.Listed
         .OnlyForMembers = ar.OnlyForMembers
         .VideoID = ar.VideoId
         .PollId = ar.PollId
         .ImageID = ar.ImageID
         .ImageNewsUrl = ar.ImageNewsUrl
         .ImageIconUrl = ar.ImageIconUrl
         .Approved = ar.Approved
      End With

      Return outArticle
   End Function

   <Extension()> _
  Function FillViewModel(ByRef ar As Article, ByVal outViewModel As AddArticleViewModel) As AddArticleViewModel

      With outViewModel
         .ArticleID = ar.ArticleID
         .ReleaseDate = ar.ReleaseDate
         .ExpireDate = ar.ExpireDate
         .Abstract = ar.Abstract
         .AddedBy = ar.AddedBy
         .Body = ar.Body
         .Title = ar.Title
         .Tags = ar.Tags
         .CommentsEnabled = ar.CommentsEnabled
         .Listed = ar.Listed
         .OnlyForMembers = ar.OnlyForMembers
         .VideoId = ar.VideoID
         .PollId = ar.PollId
         .ImageID = ar.ImageID
         .City = ar.City
         .State = ar.State
         .Country = ar.Country
         .ImageNewsUrl = ar.ImageNewsUrl
         .ImageIconUrl = ar.ImageIconUrl
         .Approved = ar.Approved
      End With

      Return outViewModel
   End Function
#End Region

#Region "Advices"

   <Extension()> _
   Function FillDTO(ByVal ar As AdminTipViewModel, ByRef outAdvice As Advice) As Advice

      With outAdvice
         .AdviceID = ar.AdviceID
         .Abstract = ar.Abstract
         .Body = ar.Body
         .Title = ar.Title
         .Tags = ar.Tags
         .CommentsEnabled = ar.CommentsEnabled
         .Listed = ar.Listed
         .OnlyForMembers = ar.OnlyForMembers
         .Approved = ar.Approved
      End With

      Return outAdvice
   End Function

   <Extension()> _
  Function FillViewModel(ByRef ar As Advice, ByVal outViewModel As AdminTipViewModel) As AdminTipViewModel
      With outViewModel
         .AdviceID = ar.AdviceID
         .Title = ar.Title
         .Tags = ar.Tags
         .CommentsEnabled = ar.CommentsEnabled
         .Listed = ar.Listed
         .OnlyForMembers = ar.OnlyForMembers
         .Body = ar.Body
         .Approved = ar.Approved
         .Abstract = ar.Abstract
      End With
      Return outViewModel
   End Function

#End Region

#Region "Tickets"

   <Extension()> _
   Function FillDTO(ByVal ar As ticketViewModel, ByRef outTicket As Ticket) As Ticket

      With outTicket
         .TicketID = ar.TicketID
         .AddedDate = ar.AddedDate
         .Addedby = ar.AddedBy
         .Title = ar.Title
         .Description = ar.Description
         .Owner = ar.Owner
         .Resolver = ar.Resolver
         .Type = ar.Type
         .Priority = ar.Priority
         .Keywords = ar.keywords
         .Status = ar.Status
      End With

      Return outTicket
   End Function

   <Extension()> _
  Function FillViewModel(ByRef ar As Ticket, ByVal outViewModel As ticketViewModel) As ticketViewModel
      With outViewModel
         .TicketID = ar.TicketID
         .AddedDate = ar.AddedDate
         .AddedBy = ar.Addedby
         .Title = ar.Title
         .Description = ar.Description
         .Owner = ar.Owner
         .Resolver = ar.Resolver
         .Type = ar.Type
         .Priority = ar.Priority
         .keywords = ar.Keywords
         .Status = ar.Status
      End With
      Return outViewModel
   End Function

#End Region

#Region "images"
   <Extension()> _
  Function FillDTO(ByVal ar As CreateThumbsViewModel, ByRef outImg As SimpleImage) As SimpleImage

      With outImg
         .Name = ar.Name
         .Uuid = ar.Uuid
         .Tags = ar.Tags
         '.ImageUrl = ar.ImageUrl
         .CreditsName = ar.CreditsName
         .CreditsUrl = ar.CreditsUrl
      End With

      Return outImg
   End Function

   <Extension()> _
  Function FillViewModel(ByRef ar As SimpleImage, ByVal outViewModel As CreateThumbsViewModel) As CreateThumbsViewModel
      With outViewModel
         .ImageID = ar.ImageID
         .Name = ar.Name
         .Uuid = ar.Uuid
         .Tags = ar.Tags
         .ImageUrl = ar.ImageUrl
         .BaseImageUrl = Configs.Paths.CdnRoot & Configs.Paths.CdnImages & ar.ImageUrl
         .MiniImageUrl = Configs.Paths.CdnRoot & Configs.Paths.CdnImages & ar.ImageUrl.Insert(ar.ImageUrl.LastIndexOf("/") + 1, "mini_")
         .LongImageUrl = Configs.Paths.CdnRoot & Configs.Paths.CdnImages & ar.ImageUrl.Insert(ar.ImageUrl.LastIndexOf("/") + 1, "long_")
         .LargeImageUrl = Configs.Paths.CdnRoot & Configs.Paths.CdnImages & ar.ImageUrl.Insert(ar.ImageUrl.LastIndexOf("/") + 1, "large_")
         .CreditsName = ar.CreditsName
         .CreditsUrl = ar.CreditsUrl
      End With
      Return outViewModel
   End Function

   <Extension()> _
Function FillViewModel(ByRef ar As SimpleImage, ByVal outViewModel As AdminImageViewModel) As AdminImageViewModel
      With outViewModel
         .ImageID = ar.ImageID
         .Name = ar.Name
         .Uuid = ar.Uuid
         .ImageUrl = Configs.Paths.CdnRoot & Configs.Paths.CdnImages & ar.ImageUrl
         .CreditsName = ar.CreditsName
         .CreditsUrl = ar.CreditsUrl
      End With
      Return outViewModel
   End Function
#End Region

#Region "videos"
   <Extension()> _
  Function FillDTO(ByVal ar As AdminVideoViewModel, ByRef outImg As Video) As Video

      With outImg
         .Name = ar.Name
         .Abstract = ar.Abstract
         .Tags = ar.Tags
         .AddedDate = ar.AddedDate
         .AddedBy = ar.AddedBy
         .VideoUrl = ar.VideoUrl
         .CommentsEnabled = ar.CommentsEnabled
         .Approved = ar.Approved
         .Listed = ar.Listed
         .OnlyForMembers = ar.OnlyForMembers
         .VideoID = ar.VideoID
         .Title = ar.Title
      End With

      Return outImg
   End Function

   <Extension()> _
  Function FillViewModel(ByRef ar As Video, ByVal outViewModel As AdminVideoViewModel) As AdminVideoViewModel
      With outViewModel
         .Name = ar.Name
         .Abstract = ar.Abstract
         .Title = ar.Title
         .Tags = ar.Tags
         .AddedDate = ar.AddedDate
         .AddedBy = ar.AddedBy
         .VideoUrl = ar.VideoUrl
         .VideoStillUrl = Configs.Paths.CdnRoot & Configs.Paths.CdnVideos & ar.Name & "/" & "default.jpg"
         .CommentsEnabled = ar.CommentsEnabled
         .Approved = ar.Approved
         .Listed = ar.Listed
         .OnlyForMembers = ar.OnlyForMembers
         .VideoID = ar.VideoID
      End With
      Return outViewModel
   End Function

   <Extension()> _
 Function FillViewModel(ByRef ar As Video, ByVal outViewModel As AdminUpdateVideoViewModel) As AdminUpdateVideoViewModel
      With outViewModel
         .Name = ar.Name
         .Description = ar.Abstract
         .Title = ar.Title
         .VideoUrl = ar.VideoUrl
         .VideoID = ar.VideoID
      End With
      Return outViewModel
   End Function
#End Region


   <Extension()> _
Function FillViewModel(ByRef user As SiteUser, ByVal outViewModel As AdminUserViewModel) As AdminUserViewModel
      Dim displayname As String = String.Empty
      Dim website As String = String.Empty
      Dim about As String = String.Empty

      Dim pr As ProfileInfo = ProfileInfo.GetProfile(user.Username)
      If pr IsNot Nothing Then
         displayname = pr.DisplayName
         website = pr.Website
         about = pr.About
      End If
      With outViewModel
         .Username = user.Username
         .DisplayName = displayname
         .Email = user.Email
         .isOnline = user.IsOnline
         .IsApproved = user.IsApproved
         .IsLockedOut = user.IsLockedOut
         .Website = website
         .About = about
      End With
      Return outViewModel
   End Function
End Module
