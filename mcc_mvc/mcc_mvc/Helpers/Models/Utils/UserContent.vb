Imports Microsoft.VisualBasic
Imports System.Linq
Imports MCC.Data

Namespace UserContents
   Public Class UserContent
      Inherits mccObject

      Public Shared Function GetUserContentCount() As Integer
         Dim mdc As New MCCDataContext()
         Dim key As String = "usercontent_usercontentCount"

         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            Dim it As Integer = mdc.mcc_Contributions.Count()
            CacheData(key, it)
            Return it
         End If
      End Function

      Public Shared Function GetUserContentCount(ByVal criteria As String) As Integer
         If Not String.IsNullOrEmpty(criteria) Then
            Dim mdc As New MCCDataContext()
            Dim key As String = "usercontent_usercontent_" & criteria & "_"

            If Cache(key) IsNot Nothing Then
               Return CInt(Cache(key))
            Else
               Dim it As Integer = mdc.mcc_Contributions.Count(Function(p) p.Body.Contains(criteria) Or p.Title.Contains(criteria))
               CacheData(key, it)
               Return it
            End If
         Else
            Return GetUserContentCount()
         End If
      End Function


      Public Shared Function GetUserContents() As List(Of mcc_Contribution)
         Dim key As String = "usercontent_usercontent_"
         If Cache(key) IsNot Nothing Then
            Dim li As List(Of mcc_Contribution) = DirectCast(Cache(key), List(Of mcc_Contribution))
            Return li
         Else
            Dim mdc As New MCCDataContext()
            Dim li As List(Of mcc_Contribution) = mdc.mcc_Contributions.ToList
            CacheData(key, li)
            Return li
         End If
      End Function

      Public Shared Function GetUserContents(ByVal criteria As String, ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "Title ASC") As List(Of mcc_Contribution)
         If Not String.IsNullOrEmpty(criteria) Then
            Dim key As String = "usercontent_usercontent_" & criteria & "_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
            If Cache(key) IsNot Nothing Then
               Dim li As List(Of mcc_Contribution) = DirectCast(Cache(key), List(Of mcc_Contribution))
               Return li
            Else
               Dim mdc As New MCCDataContext()
               Dim li As List(Of mcc_Contribution)
               If startrowindex > 0 AndAlso maximumrows > 0 Then
                  li = (From it As mcc_Contribution In mdc.mcc_Contributions Where it.Body.Contains(criteria) Or it.Title.Contains(criteria)).Skip(startrowindex - 1).Take(maximumrows).ToList
               Else
                  li = (From it As mcc_Contribution In mdc.mcc_Contributions Where it.Body.Contains(criteria) Or it.Title.Contains(criteria)).Skip(0).Take(10).ToList
               End If
               CacheData(key, li)
               Return li
            End If
         Else
            Return GetUserContents(startrowindex, maximumrows)
         End If
      End Function

      Public Shared Function GetUserContents(ByVal startrowindex As Integer, ByVal maximumrows As Integer, Optional ByVal sortExp As String = "Title ASC") As List(Of mcc_Contribution)
         Dim key As String = "usercontent_usercontent_" & startrowindex.ToString & "_" & maximumrows.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Dim li As List(Of mcc_Contribution) = DirectCast(Cache(key), List(Of mcc_Contribution))
            Return li
         Else
            Dim mdc As New MCCDataContext()
            Dim li As List(Of mcc_Contribution)
            If startrowindex > 0 AndAlso maximumrows > 0 Then
               li = mdc.mcc_Contributions.Skip(startrowindex - 1).Take(maximumrows).ToList
            Else
               li = mdc.mcc_Contributions.Skip(0).Take(10).ToList
            End If
            CacheData(key, li)
            Return li
         End If
      End Function

      Public Shared Function GetUserContentById(ByVal id As Integer) As mcc_Contribution
         Dim key As String = "usercontent_usercontent_" & id.ToString & "_"
         If Cache(key) IsNot Nothing Then
            Dim fb As mcc_Contribution = DirectCast(Cache(key), mcc_Contribution)
            Return fb
         Else
            Dim mdc As New MCCDataContext()
            Dim fb As mcc_Contribution
            If mdc.mcc_Contributions.Count(Function(p) p.ContributionID = id) > 0 Then
               fb = (From it As mcc_Contribution In mdc.mcc_Contributions Where it.ContributionID = id).FirstOrDefault
               CacheData(key, fb)
               Return fb
            Else
               Return Nothing
            End If
         End If
      End Function



      Public Shared Sub InsertUserContent(ByVal title As String, ByVal description As String, ByVal author As String, ByVal email As String, ByVal comments As String)
         Dim mdc As MCCDataContext = New MCCDataContext
         Dim fb As New mcc_Contribution()

         With fb
            .Title = title
            .Body = description
            .AddedDate = DateTime.Now
            .Approved = False
            .Reviewed = False
            .Comments = comments
            .Email = email
            .AddedBy = author
         End With

         mdc.mcc_Contributions.InsertOnSubmit(fb)
         mdc.SubmitChanges()
         mccObject.PurgeCacheItems("usercontent_")

      End Sub


      Public Shared Sub UpdateUserContent(ByVal ContributionId As Integer, ByVal body As String)
         Dim mdc As MCCDataContext = New MCCDataContext
         Dim wrd As mcc_Contribution = (From t In mdc.mcc_Contributions _
                                     Where t.ContributionID = ContributionId _
                                     Select t).Single()
         If wrd IsNot Nothing Then
            wrd.Body = body
            mdc.SubmitChanges()
            mccObject.PurgeCacheItems("usercontent_usercontent_")
         End If
      End Sub

      Public Shared Sub DeleteUserContent(ByVal ContributionId As Integer)
         Using mdc As New MCCDataContext
            If mdc.mcc_Contributions.Count(Function(p) p.ContributionID = ContributionId) Then
               Dim mc As mcc_Contribution = (From it As mcc_Contribution In mdc.mcc_Contributions Where it.ContributionID = ContributionId).Single()
               mdc.mcc_Contributions.DeleteOnSubmit(mc)
               mdc.SubmitChanges()
            End If
         End Using
      End Sub

      Protected Shared Sub CacheData(ByVal key As String, ByVal data As Object)
         If data IsNot Nothing Then
            mccObject.Cache.Insert(key, data, Nothing, DateTime.Now.AddSeconds(3600), TimeSpan.Zero)
         End If
      End Sub
   End Class

End Namespace

