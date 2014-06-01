Imports Microsoft.VisualBasic
Imports System.IO

Public Class FileUploadStatus

   Private _success As Boolean
   Public Property Success() As Boolean
      Get
         Return _success
      End Get
      Set(ByVal value As Boolean)
         _success = value
      End Set
   End Property

   Private _message As List(Of String) = New List(Of String)
   Public Property Message() As List(Of String)
      Get
         Return _message
      End Get
      Set(ByVal value As List(Of String))
         _message = value
      End Set
   End Property

End Class

Public NotInheritable Class VideoHelper

   Public Shared Sub CreateVideoStill(ByVal fileToCreateTheThumbFor As String, _
                               ByVal OutfileName As String, _
                               ByVal stillPosition As Integer, _
                               ByVal thumbWidth As Integer, _
                               ByVal thumbHeight As Integer)

      ' make sur the video exists
      Dim videoFile As New FileInfo(fileToCreateTheThumbFor)
      If Not videoFile.Exists() Then
         Return
      End If


      ' delete the file if it exists
      Dim fi As New FileInfo(OutfileName)
      If fi.Exists Then
         fi.Delete()
      End If

      Dim _sizeArg As String = ""
      Dim _stillPositionArg As String = ""
      Dim _fileInArg As String = ""
      Dim _fileOutArg As String = ""

      ' size
      If thumbWidth < 1 Or thumbHeight < 1 Then
         Return
      End If
      _sizeArg = "-s " & thumbWidth.ToString & "x" & thumbHeight.ToString

      'still position
      If stillPosition <= 0 Then
         Return
      End If
      _stillPositionArg = "-itsoffset -" & stillPosition.ToString

      _fileInArg = "-i " & Convert.ToChar(34) & fileToCreateTheThumbFor & Convert.ToChar(34)
      _fileOutArg = Convert.ToChar(34) & OutfileName & Convert.ToChar(34)


      Dim pr As New System.Diagnostics.Process
      pr.StartInfo.UseShellExecute = False
      pr.StartInfo.CreateNoWindow = True
      pr.StartInfo.RedirectStandardOutput = False
      pr.StartInfo.FileName = HttpContext.Current.Server.MapPath("~/bin/ffmpeg.exe")
      'pr.StartInfo.Arguments = "-itsoffset " & stillPosition & " -i " & fileToCreateTheThumbFor & " -vcodec mjpeg -vframes 1 -an -f rawvideo -s 320x240 " & OutfileName
      pr.StartInfo.Arguments = String.Format("{0} {1} -vcodec mjpeg -vframes 1 -an -f rawvideo {2} {3}", _stillPositionArg, _fileInArg, _sizeArg, _fileOutArg)
      pr.Start()
   End Sub


   Public Shared Function UploadVideoFile(ByVal fileToUpload As HttpPostedFileBase, ByVal videoname As String) As FileUploadStatus

      Dim status As New FileUploadStatus()

      ' should also check the file format!

      If fileToUpload.ContentLength < Configs.Uploads.MaxVideoSize Then

         Dim shortName As String = videoname & System.IO.Path.GetExtension(fileToUpload.FileName)

         Dim fi As New FileInfo(fileToUpload.FileName)

         Dim efn As String = String.Empty
         Dim sname = fi.Name.Replace(fi.Extension, "")

         'Dim dirUrl As String = CType(Me.Page, mccPage).BaseUrl & "Uploads/Videos/"

         Dim vdPath As String = "/uploads/videos/" & videoname & "/" & videoname & fi.Extension

         Dim _fi As New FileInfo(HttpContext.Current.Server.MapPath(vdPath))

         If _fi IsNot Nothing Then
            If Not Directory.Exists(_fi.Directory.FullName) Then
               Directory.CreateDirectory(_fi.Directory.FullName)
            End If
         End If

         Dim fname As String = HttpContext.Current.Server.MapPath(vdPath)
         fileToUpload.SaveAs(fname)
         VideoHelper.CreateVideoStill(fname, _fi.Directory.FullName & "\default.jpg", 30, 180, 120)


         ' upload to remote server
         Dim res As Boolean = routines.UploadLocalFileToFTP(_fi.FullName, Configs.Paths.CdnVideos & fi.Directory.Name)
         'Dim ct As Threading.Thread = Threading.Thread.CurrentThread
         Threading.Thread.Sleep(5000)

         If File.Exists(_fi.Directory.FullName & "\default.jpg") Then
            routines.UploadLocalFileToFTP(_fi.Directory.FullName & "\default.jpg", Configs.Paths.CdnVideos & fi.Directory.Name)
         End If

         If res Then
            status.Success = res
            status.Message.Add(fileToUpload.FileName & " uploaded successfully")
            Return status
         Else
            status.Success = res
            status.Message.Add(fileToUpload.FileName & " couldn't be uploaded for server reasons")
            Return status
         End If

         ' delete temp files
         File.Delete(_fi.Directory.FullName & "\default.jpg")
         _fi.Delete()
         'Directory.Delete(_fi.Directory.FullName, True)
      Else
         status.Success = False
         status.Message.Add(fileToUpload.FileName & " was too large to be uploaded. Please specify a smaller file.")
         Return status
      End If

      Return status
   End Function
End Class
