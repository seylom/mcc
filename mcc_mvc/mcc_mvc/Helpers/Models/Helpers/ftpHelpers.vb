Imports Microsoft.VisualBasic
Imports System.Net
Imports System.io

   Public Class ftpHelperObject

      Public ftpServerIP As String = String.Empty
      Private ftpUserID As String = String.Empty
      Private ftpPassword As String = String.Empty

      Public Sub New(ByVal ftpServerPath As String, ByVal userId As String, ByVal password As String)
         ftpUserID = userId
         ftpPassword = password
         ftpServerIP = ftpServerPath
      End Sub


      ''' <summary>
      ''' Method to upload the specified file to the specified FTP Server
      ''' </summary>
      ''' <param name="filename">file full name to be uploaded</param>
   Public Function Upload(ByVal filename As String) As Boolean
      Dim fileInf As New FileInfo(filename)
      Dim statusCode As FtpStatusCode = FtpStatusCode.Undefined

      Dim uri As String = (ftpServerIP) + fileInf.Name
      Dim reqFTP As FtpWebRequest

      ' Create FtpWebRequest object from the Uri provided
      reqFTP = DirectCast(FtpWebRequest.Create(uri), FtpWebRequest)

      ' Provide the WebPermission Credintials
      reqFTP.Credentials = New NetworkCredential(ftpUserID, ftpPassword)

      ' By default KeepAlive is true, where the control connection is not closed
      ' after a command is executed.
      reqFTP.KeepAlive = False

      ' Specify the command to be executed.
      reqFTP.Method = WebRequestMethods.Ftp.UploadFile

      ' Specify the data transfer type.
      reqFTP.UseBinary = True

      ' Notify the server about the size of the uploaded file
      reqFTP.ContentLength = fileInf.Length

      ' The buffer size is set to 2kb
      Dim buffLength As Integer = 2048
      Dim buff As Byte() = New Byte(buffLength - 1) {}
      Dim contentLen As Integer

      ' Opens a file stream (System.IO.FileStream) to read the file to be uploaded
      Dim fs As FileStream = fileInf.OpenRead()

      Try
         ' Stream to which the file to be upload is written
         Dim strm As Stream = reqFTP.GetRequestStream()

         ' Read from the file stream 2kb at a time
         contentLen = fs.Read(buff, 0, buffLength)

         ' Till Stream content ends
         While contentLen <> 0
            ' Write Content from the file stream to the FTP Upload Stream
            strm.Write(buff, 0, contentLen)
            contentLen = fs.Read(buff, 0, buffLength)
         End While

         ' Close the file stream and the Request Stream
         strm.Close()
         fs.Close()
      Catch ex As WebException
         Return False
      Catch ex As Exception

         Return False
      End Try

      Return True
   End Function

      ''' <summary>
      ''' 
      ''' </summary>
      ''' <param name="file"></param>
      ''' <remarks></remarks>
   Public Function UploadHttpPostFile(ByVal file As HttpPostedFileBase, ByVal shortname As String) As FtpStatusCode

      Dim uri As String = (ftpServerIP) + shortname
      Dim reqFTP As FtpWebRequest

      ' Create FtpWebRequest object from the Uri provided
      reqFTP = DirectCast(FtpWebRequest.Create(New Uri(uri)), FtpWebRequest)

      ' Provide the WebPermission Credintials
      reqFTP.Credentials = New NetworkCredential(ftpUserID, ftpPassword)

      ' By default KeepAlive is true, where the control connection is not closed
      ' after a command is executed.
      reqFTP.KeepAlive = False

      ' Specify the command to be executed.
      reqFTP.Method = WebRequestMethods.Ftp.UploadFile

      ' Specify the data transfer type.
      reqFTP.UseBinary = True

      ' Notify the server about the size of the uploaded file
      reqFTP.ContentLength = file.ContentLength

      Try
         ' Stream to which the file to be upload is written
         Dim strm As Stream = reqFTP.GetRequestStream()

         Dim streamObj As Stream = file.InputStream
         Dim buffer As [Byte]() = New [Byte](file.ContentLength - 1) {}
         streamObj.Read(buffer, 0, buffer.Length)
         streamObj.Close()
         streamObj = Nothing

         'Dim requestStream As Stream = requestObj.GetRequestStream()
         strm.Write(buffer, 0, buffer.Length)
         strm.Flush()
         strm.Close()

         reqFTP = Nothing

         Return True

      Catch ex As Exception
         Return False
      End Try

   End Function


      Public Sub DeleteFTP(ByVal fileName As String)
         Try
            Dim uri As String = ftpServerIP + fileName
            Dim reqFTP As FtpWebRequest
            reqFTP = DirectCast(FtpWebRequest.Create(New Uri(ftpServerIP + fileName)), FtpWebRequest)

            reqFTP.Credentials = New NetworkCredential(ftpUserID, ftpPassword)
            reqFTP.KeepAlive = False
            reqFTP.Method = WebRequestMethods.Ftp.DeleteFile

            Dim result As String = [String].Empty
            Dim response As FtpWebResponse = DirectCast(reqFTP.GetResponse(), FtpWebResponse)
            Dim size As Long = response.ContentLength
            Dim datastream As Stream = response.GetResponseStream()
            Dim sr As New StreamReader(datastream)
            result = sr.ReadToEnd()
            sr.Close()
            datastream.Close()
            response.Close()
         Catch ex As Exception
            'MessageBox.Show(ex.Message, "FTP 2.0 Delete")
         End Try
      End Sub

      'Public Function GetFilesDetailList() As String()
      '   Dim downloadFiles As String()
      '   Try
      '      Dim result As New StringBuilder()
      '      Dim ftp As FtpWebRequest
      '      ftp = DirectCast(FtpWebRequest.Create(New Uri(ftpServerIP), FtpWebRequest)
      '      ftp.Credentials = New NetworkCredential(ftpUserID, ftpPassword)
      '      ftp.Method = WebRequestMethods.Ftp.ListDirectoryDetails
      '      Dim response As WebResponse = ftp.GetResponse()
      '      Dim reader As New StreamReader(response.GetResponseStream())
      '      Dim line As String = reader.ReadLine()
      '      While line IsNot Nothing
      '         result.Append(line)
      '         result.Append(vbLf)
      '         line = reader.ReadLine()
      '      End While

      '      result.Remove(result.ToString().LastIndexOf(vbLf), 1)
      '      reader.Close()
      '      response.Close()
      '      'MessageBox.Show(result.ToString().Split('\n'));
      '      Return result.ToString().Split(ControlChars.Lf)
      '   Catch ex As Exception
      '      System.Windows.Forms.MessageBox.Show(ex.Message)
      '      downloadFiles = Nothing
      '      Return downloadFiles
      '   End Try
      'End Function

      Public Function GetFileList() As String()
         Dim downloadFiles As String()
         Dim result As New StringBuilder()
         Dim reqFTP As FtpWebRequest
         Try
            reqFTP = DirectCast(FtpWebRequest.Create(New Uri("ftp://" & ftpServerIP & "/")), FtpWebRequest)
            reqFTP.UseBinary = True
            reqFTP.Credentials = New NetworkCredential(ftpUserID, ftpPassword)
            reqFTP.Method = WebRequestMethods.Ftp.ListDirectory
            Dim response As WebResponse = reqFTP.GetResponse()
            Dim reader As New StreamReader(response.GetResponseStream())
            'MessageBox.Show(reader.ReadToEnd());
            Dim line As String = reader.ReadLine()
            While line IsNot Nothing
               result.Append(line)
               result.Append(vbLf)
               line = reader.ReadLine()
            End While
            result.Remove(result.ToString().LastIndexOf(ControlChars.Lf), 1)
            reader.Close()
            response.Close()
            'MessageBox.Show(response.StatusDescription);
            Return result.ToString().Split(ControlChars.Lf)
         Catch ex As Exception
         'System.Windows.Forms.MessageBox.Show(ex.Message)
            downloadFiles = Nothing
            Return downloadFiles
         End Try
      End Function
      Public Sub Download(ByVal filePath As String, ByVal fileName As String)
         Dim reqFTP As FtpWebRequest
         Try
            'filePath = <<The full path where the file is to be created.>>, 
            'fileName = <<Name of the file to be created(Need not be the name of the file on FTP server).>>
            Dim outputStream As New FileStream((filePath & "\") + fileName, FileMode.Create)

            reqFTP = DirectCast(FtpWebRequest.Create(New Uri(ftpServerIP + fileName)), FtpWebRequest)
            reqFTP.Method = WebRequestMethods.Ftp.DownloadFile
            reqFTP.UseBinary = True
            reqFTP.Credentials = New NetworkCredential(ftpUserID, ftpPassword)
            Dim response As FtpWebResponse = DirectCast(reqFTP.GetResponse(), FtpWebResponse)
            Dim ftpStream As Stream = response.GetResponseStream()
            Dim cl As Long = response.ContentLength
            Dim bufferSize As Integer = 2048
            Dim readCount As Integer
            Dim buffer As Byte() = New Byte(bufferSize - 1) {}

            readCount = ftpStream.Read(buffer, 0, bufferSize)
            While readCount > 0
               outputStream.Write(buffer, 0, readCount)
               readCount = ftpStream.Read(buffer, 0, bufferSize)
            End While

            ftpStream.Close()
            outputStream.Close()
            response.Close()
         Catch ex As Exception
            'MessageBox.Show(ex.Message)
         End Try
      End Sub


      Public Function GetFileSize(ByVal filename As String) As Long
         Dim reqFTP As FtpWebRequest
         Dim fileSize As Long = 0
         Try
            reqFTP = DirectCast(FtpWebRequest.Create(New Uri(ftpServerIP + filename)), FtpWebRequest)
            reqFTP.Method = WebRequestMethods.Ftp.GetFileSize
            reqFTP.UseBinary = True
            reqFTP.Credentials = New NetworkCredential(ftpUserID, ftpPassword)
            Dim response As FtpWebResponse = DirectCast(reqFTP.GetResponse(), FtpWebResponse)
            Dim ftpStream As Stream = response.GetResponseStream()
            fileSize = response.ContentLength

            ftpStream.Close()
            response.Close()
         Catch ex As Exception
            'MessageBox.Show(ex.Message)
         End Try
         Return fileSize
      End Function

      Public Sub Rename(ByVal currentFilename As String, ByVal newFilename As String)
         Dim reqFTP As FtpWebRequest
         Try
            reqFTP = DirectCast(FtpWebRequest.Create(New Uri(ftpServerIP + currentFilename)), FtpWebRequest)
            reqFTP.Method = WebRequestMethods.Ftp.Rename
            reqFTP.RenameTo = newFilename
            reqFTP.UseBinary = True
            reqFTP.Credentials = New NetworkCredential(ftpUserID, ftpPassword)
            Dim response As FtpWebResponse = DirectCast(reqFTP.GetResponse(), FtpWebResponse)
            Dim ftpStream As Stream = response.GetResponseStream()

            ftpStream.Close()
            response.Close()

            reqFTP = Nothing
         Catch ex As Exception
            'MessageBox.Show(ex.Message)
         End Try
      End Sub

      Public Function MakeDir(ByVal dirName As String) As FtpStatusCode
         Dim reqFTP As FtpWebRequest

         Dim statusCode As FtpStatusCode = FtpStatusCode.Undefined
         Try
            ' dirName = name of the directory to create.
            reqFTP = DirectCast(FtpWebRequest.Create(New Uri(ftpServerIP + dirName)), FtpWebRequest)
            reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory
            reqFTP.UseBinary = True
            reqFTP.Credentials = New NetworkCredential(ftpUserID, ftpPassword)
            Dim response As FtpWebResponse = DirectCast(reqFTP.GetResponse(), FtpWebResponse)
            Dim ftpStream As Stream = response.GetResponseStream()

            ftpStream.Close()
            response.Close()

            reqFTP = Nothing

         Catch wex As WebException
            Dim resp As FtpWebResponse = CType(wex.Response, FtpWebResponse)
            statusCode = resp.StatusCode
         Catch ex As Exception
            statusCode = statusCode
         Finally
            reqFTP = Nothing
         End Try

         Return statusCode
      End Function



   End Class
