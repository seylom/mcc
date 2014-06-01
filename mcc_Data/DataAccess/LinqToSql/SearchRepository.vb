Imports EeekSoft.Query
Imports System.Data.Linq.SqlClient

Public Enum ResultType
   Article
   Video
   Question
   Tip
End Enum
Public Class SearchRepository
   Implements ISearchRepository

   Private _mdc As MCCDataContext
   Public Sub New()
      Me.New(new MCCDataContext)
   End Sub

   Public Sub New(ByVal db As MCCDataContext)
      _mdc = db
   End Sub
   Public Function FindArticles(ByVal query As String, ByVal type As SearchQueryType) As IQueryable(Of SearchResult) Implements ISearchRepository.FindArticles
      Select Case type
         Case SearchQueryType.AnyWord
            Dim q() As String = query.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)
            Dim rs = (From it As mcc_Article In _mdc.mcc_Articles.ToExpandable Where it.Abstract.ContainsAny(q) Or it.Body.ContainsAny(q) Or _
                     it.Title.ContainsAny(q) Select New SearchResult With {.Abstract = If(it.Abstract.Length > 150, it.Abstract.Substring(0, 150), it.Abstract), _
                                                                           .Body = If(it.Body.Length > 200, it.Body.Substring(0, 200), it.Body), _
                                                                           .Title = it.Title, _
                                                                           .Url = ArticleUrl(it.ArticleID, it.Slug), _
                                                                           .ResultType = SearchLocation.Articles}).ToExpandable
            Return rs
         Case SearchQueryType.AllWords
            Dim q() As String = query.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)
            Dim rs = (From it As mcc_Article In _mdc.mcc_Articles.ToExpandable Where it.Abstract.ContainsAll(q) Or it.Body.ContainsAll(q) Or _
                     it.Title.ContainsAny(q) Select New SearchResult With {.Abstract = If(it.Abstract.Length > 150, it.Abstract.Substring(0, 150), it.Abstract), _
                                                                           .Body = If(it.Body.Length > 200, it.Body.Substring(0, 200), it.Body), _
                                                                           .Title = it.Title, _
                                                                           .Url = ArticleUrl(it.ArticleID, it.Slug), _
                                                                           .ResultType = SearchLocation.Articles}).ToExpandable
            Return rs
         Case SearchQueryType.ExactPhrase
            Dim q() As String = query.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)
            Dim rs = (From it As mcc_Article In _mdc.mcc_Articles.ToExpandable Where it.Abstract.ContainsAll(q) Or it.Body.ContainsAll(q) Or _
                     it.Title.ContainsAny(q) Select New SearchResult With {.Abstract = If(it.Abstract.Length > 150, it.Abstract.Substring(0, 150), it.Abstract), _
                                                                           .Body = If(it.Body.Length > 200, it.Body.Substring(0, 200), it.Body), _
                                                                           .Title = it.Title, _
                                                                           .Url = ArticleUrl(it.ArticleID, it.Slug), _
                                                                           .ResultType = SearchLocation.Articles}).ToExpandable

         Case Else
            Throw New NotImplementedException("The site currently does not support this type of search")
      End Select
   End Function

   Public Function FindAdvices(ByVal query As String, ByVal type As SearchQueryType) As IQueryable(Of SearchResult) Implements ISearchRepository.FindAdvices
      Select Case type
         Case SearchQueryType.AnyWord
            Dim q() As String = query.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)
            Dim rs = From it As mcc_Advice In _mdc.mcc_Advices.ToExpandable Where it.Abstract.ContainsAny(q) Or it.Body.ContainsAny(q) Or _
                     it.Title.ContainsAny(q) Select New SearchResult With {.Abstract = "", _
                                                                           .Body = If(it.Body.Length > 200, it.Body.Substring(0, 200), it.Body), _
                                                                           .Title = it.Title, _
                                                                           .Url = TipUrl(it.AdviceID, it.Slug), _
                                                                           .ResultType = SearchLocation.Tips}
            Return rs
         Case SearchQueryType.AllWords
            Dim q() As String = query.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)
            Dim rs = From it As mcc_Advice In _mdc.mcc_Advices.ToExpandable Where it.Abstract.ContainsAll(q) Or it.Body.ContainsAll(q) Or _
                     it.Title.ContainsAll(q) Select New SearchResult With {.Abstract = "", _
                                                                           .Body = If(it.Body.Length > 200, it.Body.Substring(0, 200), it.Body), _
                                                                           .Title = it.Title, _
                                                                           .Url = TipUrl(it.AdviceID, it.Slug), _
                                                                            .ResultType = SearchLocation.Tips}
            Return rs
         Case SearchQueryType.ExactPhrase
            Throw New NotImplementedException("The site currently does not support this type of search")
         Case Else
            Throw New NotImplementedException("The site currently does not support this type of search")
      End Select
   End Function
   Public Function FindVideos(ByVal query As String, ByVal type As SearchQueryType) As IQueryable(Of SearchResult) Implements ISearchRepository.FindVideos
      Select Case type
         Case SearchQueryType.AnyWord
            Dim q() As String = query.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)
            Dim rs = From it As mcc_Video In _mdc.mcc_Videos.ToExpandable Where it.Abstract.ContainsAny(q) Or _
                     it.Title.ContainsAny(q) Select New SearchResult With {.Abstract = "", _
                                                                           .Body = If(it.Abstract.Length > 200, it.Abstract.Substring(0, 200), it.Abstract), _
                                                                           .Title = it.Title, _
                                                                           .Url = VideoUrl(it.VideoId, it.Slug), _
                                                                            .ResultType = SearchLocation.Videos}
            Return rs
         Case SearchQueryType.AllWords
            Dim q() As String = query.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)
            Dim rs = From it As mcc_Video In _mdc.mcc_Videos.ToExpandable Where it.Abstract.ContainsAll(q) Or _
                     it.Title.ContainsAll(q) Select New SearchResult With {.Abstract = "", _
                                                                           .Body = If(it.Abstract.Length > 200, it.Abstract.Substring(0, 200), it.Abstract), _
                                                                           .Title = it.Title, _
                                                                           .Url = VideoUrl(it.VideoId, it.Slug), _
                                                                            .ResultType = SearchLocation.Videos}
            Return rs
         Case SearchQueryType.ExactPhrase
            Throw New NotImplementedException("The site currently does not support this type of search")
         Case Else
            Throw New NotImplementedException("The site currently does not support this type of search")
      End Select
   End Function
   Public Function FindUserQuestions(ByVal query As String, ByVal type As SearchQueryType) As IQueryable(Of SearchResult) Implements ISearchRepository.FindUserQuestions
      Select Case type
         Case SearchQueryType.AnyWord
            Dim q() As String = query.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)
            Dim rs = From it As mcc_UserQuestion In _mdc.mcc_UserQuestions.ToExpandable Where it.Body.ContainsAny(q) Or _
                     it.Title.ContainsAny(q) Select New SearchResult With {.SearchResultID = it.UserQuestionId, _
                                                                           .Abstract = "", _
                                                                           .Body = If(it.Body.Length > 200, it.Body.Substring(0, 200), it.Body), _
                                                                           .Title = it.Title, _
                                                                           .Url = QuestionUrl(it.UserQuestionId, it.Slug), _
                                                                            .ResultType = SearchLocation.QuestionsAndAnswers}
            Return rs
         Case SearchQueryType.AllWords
            Dim q() As String = query.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)
            Dim rs = From it As mcc_UserQuestion In _mdc.mcc_UserQuestions.ToExpandable Where it.Body.ContainsAll(q) Or _
                     it.Title.ContainsAll(q) Select New SearchResult With {.SearchResultID = it.UserQuestionId, _
                                                                           .Abstract = "", _
                                                                           .Body = If(it.Body.Length > 200, it.Body.Substring(0, 200), it.Body), _
                                                                           .Title = it.Title, _
                                                                           .Url = QuestionUrl(it.UserQuestionId, it.Slug), _
                                                                           .ResultType = SearchLocation.QuestionsAndAnswers}
            Return rs
         Case SearchQueryType.ExactPhrase
            Throw New NotImplementedException("The site currently does not support this type of search")
         Case Else
            Throw New NotImplementedException("The site currently does not support this type of search")
      End Select
   End Function
   Public Function FindUserAnswers(ByVal query As String, ByVal type As SearchQueryType) As IQueryable(Of SearchResult) Implements ISearchRepository.FindUserAnswers
      Select Case type
         Case SearchQueryType.AnyWord
            Dim q() As String = query.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)
            Dim rs = From it As mcc_UserAnswer In _mdc.mcc_UserAnswers.ToExpandable Where it.Body.ContainsAny(q) _
                      Select New SearchResult With {.SearchResultID = it.UserAnswerId, _
                                                    .Abstract = "", _
                                                   .Body = If(it.Body.Length > 200, it.Body.Substring(0, 200), it.Body), _
                                                   .Title = it.mcc_UserQuestion.Title, _
                                                   .Url = QuestionUrl(it.UserQuestionId, ""), _
                                                    .ResultType = SearchLocation.QuestionsAndAnswers}
            Return rs
         Case SearchQueryType.AllWords
            Dim q() As String = query.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)
            Dim rs = From it As mcc_UserAnswer In _mdc.mcc_UserAnswers.ToExpandable Where it.Body.ContainsAll(q) _
                      Select New SearchResult With {.SearchResultID = it.UserAnswerId, _
                                                    .Abstract = "", _
                                                   .Body = If(it.Body.Length > 200, it.Body.Substring(0, 200), it.Body), _
                                                   .Title = it.mcc_UserQuestion.Title, _
                                                   .Url = QuestionUrl(it.UserQuestionId, ""), _
                                                    .ResultType = SearchLocation.QuestionsAndAnswers}
            Return rs
         Case SearchQueryType.ExactPhrase
            Throw New NotImplementedException("The site currently does not support this type of search")
         Case Else
            Throw New NotImplementedException("The site currently does not support this type of search")
      End Select
   End Function


    Public Function QuestionUrl(ByVal id As Integer, ByVal slug As String) As String
        Dim str As String = "/questions/{0}/{1}"
        Return String.Format(str, id, slug)
    End Function

    Public Function TipUrl(ByVal id As Integer, ByVal slug As String) As String
        Dim str As String = "/tips/{0}/{1}"
        Return String.Format(str, id, slug)
    End Function


    Public Function ArticleUrl(ByVal id As Integer, ByVal slug As String) As String
        Dim str As String = "/articles/{0}/{1}"
        Return String.Format(str, id, slug)
    End Function

    Public Function VideoUrl(ByVal id As Integer, ByVal slug As String) As String
        Dim str As String = "/videos/{0}/{1}"
        Return String.Format(str, id, slug)
    End Function


End Class
