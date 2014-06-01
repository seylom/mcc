Imports Microsoft.VisualBasic
Imports System.Linq
Imports MCC.Data

Namespace Words
   Public Class WordRepository
      Inherits mccObject

      Public Shared Function GetWordsCount() As Integer
         Dim key As String = "badwords_badwordscount"
         If Cache(key) IsNot Nothing Then
            Return CInt(Cache(key))
         Else
            Dim mdc As MCCDataContext = New MCCDataContext
            Dim i As Integer = mdc.mcc_BadWords.Count
            CacheData(key, i)
            Return i
         End If
      End Function

      Public Shared Function GetWords() As List(Of mcc_BadWord)
         Dim key As String = "badwords_badwords_"
         If Cache(key) IsNot Nothing Then
            Return DirectCast(Cache(key), List(Of mcc_BadWord))
         Else
            Dim mdc As MCCDataContext = New MCCDataContext
            Dim li As List(Of mcc_BadWord) = (From t As mcc_BadWord In mdc.mcc_BadWords Select t).ToList
            CacheData(key, li)
            Return li
         End If
      End Function


      Public Shared Function GetWords(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, Optional ByVal sort As String = "Word ASC") As List(Of mcc_BadWord)
         Dim key As String = "badwords_badwords_" & startRowIndex & "_" & maximumRows & "_"
         If Cache(key) IsNot Nothing Then
            Return DirectCast(Cache(key), List(Of mcc_BadWord))
         Else
            Dim mdc As MCCDataContext = New MCCDataContext
            Dim li As New List(Of mcc_BadWord)
            If startRowIndex > 0 AndAlso maximumRows > 0 Then
               li = (From t As mcc_BadWord In mdc.mcc_BadWords Select t).Skip(startRowIndex - 1).Take(startRowIndex * maximumRows).ToList
            Else
               li = (From t As mcc_BadWord In mdc.mcc_BadWords Select t).Skip(0).Take(20).ToList
            End If
            CacheData(key, li)
            Return li
         End If
      End Function


      Public Shared Sub InsertWord(ByVal word As String)
         Dim mdc As MCCDataContext = New MCCDataContext
         Dim bw As New mcc_BadWord With {.Word = word}
         mdc.mcc_BadWords.InsertOnSubmit(bw)
         mdc.SubmitChanges()
         mccObject.PurgeCacheItems("badwords_")
      End Sub


      Public Shared Sub UpdateWord(ByVal ID As Integer, ByVal word As String)
         Dim mdc As MCCDataContext = New MCCDataContext
         Dim wrd As mcc_BadWord = (From t In mdc.mcc_BadWords _
                                     Where t.ID = ID _
                                     Select t).Single()
         If wrd IsNot Nothing Then
            wrd.Word = word
            mdc.SubmitChanges()
            mccObject.PurgeCacheItems("badwords_badwords_")
         End If
      End Sub


      Public Shared Sub DeleteWord(ByVal ID As Integer)
         Dim mdc As MCCDataContext = New MCCDataContext
         Dim wrd As mcc_BadWord = (From t In mdc.mcc_BadWords _
                                     Where t.ID = ID _
                                     Select t).Single()

         If wrd IsNot Nothing Then
            mdc.mcc_BadWords.DeleteOnSubmit(wrd)
            Dim cs As System.Data.Linq.ChangeSet = mdc.GetChangeSet()
            mdc.SubmitChanges()
            mccObject.PurgeCacheItems("badwords_")
         End If
      End Sub

      Protected Shared Sub CacheData(ByVal key As String, ByVal data As Object)
         If data IsNot Nothing Then
            mccObject.Cache.Insert(key, data, Nothing, DateTime.Now.AddSeconds(3600), TimeSpan.Zero)
         End If
      End Sub

   End Class
End Namespace
