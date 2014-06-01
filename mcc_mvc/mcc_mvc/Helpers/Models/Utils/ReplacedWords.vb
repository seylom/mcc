Imports Microsoft.VisualBasic
Imports System.Linq
Imports MCC.Data
Namespace ReplacedWords
   Public Class Words

      Public Function GetWordsCount() As Integer
         Dim mdc As MCCDataContext = New MCCDataContext
         Return (From t As mcc_Word_Replace In mdc.mcc_Word_Replaces Select t).Count
      End Function

      Public Function GetWords() As List(Of mcc_Word_Replace)
         Dim mdc As MCCDataContext = New MCCDataContext
         Dim li As New List(Of mcc_Word_Replace)
         Return (From t As mcc_Word_Replace In mdc.mcc_Word_Replaces Select t).ToList
      End Function


      Public Function GetWords(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, Optional ByVal sort As String = "BadWord ASC") As List(Of mcc_Word_Replace)
         Dim mdc As MCCDataContext = New MCCDataContext
         Dim li As New List(Of mcc_Word_Replace)
         If startRowIndex > 0 AndAlso maximumRows > 0 Then
            Return (From t As mcc_Word_Replace In mdc.mcc_Word_Replaces Select t).Skip(startRowIndex - 1).Take(startRowIndex * maximumRows).ToList
         Else
            Return (From t As mcc_Word_Replace In mdc.mcc_Word_Replaces Select t).Skip(0).Take(20).ToList
         End If
      End Function


      Public Sub InsertWord(ByVal badword As String, ByVal goodword As String)
         Dim mdc As MCCDataContext = New MCCDataContext
         Dim bw As New mcc_Word_Replace With {.BadWord = badword, .GoodWord = goodword}
         mdc.mcc_Word_Replaces.InsertOnSubmit(bw)
         mdc.SubmitChanges()
      End Sub


      Public Sub UpdateWord(ByVal ID As Integer, ByVal badword As String, ByVal goodword As String)
         Dim mdc As MCCDataContext = New MCCDataContext
         Dim wrd As mcc_Word_Replace = (From t In mdc.mcc_Word_Replaces _
                                     Where t.ID = ID _
                                     Select t).Single()
         If wrd IsNot Nothing Then
            wrd.BadWord = badword
            wrd.GoodWord = goodword
            mdc.SubmitChanges()
         End If
      End Sub


      Public Sub DeleteWord(ByVal ID As Integer)
         Dim mdc As MCCDataContext = New MCCDataContext
         Dim wrd As mcc_Word_Replace = (From t In mdc.mcc_Word_Replaces _
                                     Where t.ID = ID _
                                     Select t).Single()

         If wrd IsNot Nothing Then
            mdc.mcc_Word_Replaces.DeleteOnSubmit(wrd)
            Dim cs As System.Data.Linq.ChangeSet = mdc.GetChangeSet()
            mdc.SubmitChanges()
         End If
      End Sub
   End Class
End Namespace
