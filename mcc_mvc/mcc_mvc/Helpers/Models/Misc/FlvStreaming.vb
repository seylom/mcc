Imports System
Imports System.IO
Imports System.Web


''' <summary>
''' Summary description for FLVStreaming
''' </summary>
Public Class FLVStreaming
   Implements IHttpHandler
   Private Shared ReadOnly _flvheader As Byte() = HexToByte("464C5601010000000900000009")
   '"FLV\x1\x1\0\0\0\x9\0\0\0\x9"
   Public Sub New()
   End Sub

   Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
      Try
         Dim pos As Integer
         Dim length As Integer

         ' Check start parameter if present
         Dim filename As String = Path.GetFileName(context.Request.FilePath)

         Using fs As New FileStream(context.Server.MapPath(filename), FileMode.Open, FileAccess.Read, FileShare.Read)
            Dim qs As String = context.Request.Params("start")

            If String.IsNullOrEmpty(qs) Then
               pos = 0
               length = Convert.ToInt32(fs.Length)
            Else
               pos = Convert.ToInt32(qs)
               length = Convert.ToInt32(fs.Length - pos) + _flvheader.Length
            End If

            ' Add HTTP header stuff: cache, content type and length        
            context.Response.Cache.SetCacheability(HttpCacheability.[Public])
            context.Response.Cache.SetLastModified(DateTime.Now)

            context.Response.AppendHeader("Content-Type", "video/x-flv")
            context.Response.AppendHeader("Content-Length", length.ToString())

            ' Append FLV header when sending partial file
            If pos > 0 Then
               context.Response.OutputStream.Write(_flvheader, 0, _flvheader.Length)
               fs.Position = pos
            End If

            ' Read buffer and write stream to the response stream
            Const buffersize As Integer = 16384
            Dim buffer As Byte() = New Byte(buffersize - 1) {}

            Dim count As Integer = fs.Read(buffer, 0, buffersize)
            While count > 0
               If context.Response.IsClientConnected Then
                  context.Response.OutputStream.Write(buffer, 0, count)
                  count = fs.Read(buffer, 0, buffersize)
               Else
                  count = -1
               End If
            End While
         End Using
      Catch ex As Exception
         System.Diagnostics.Debug.WriteLine(ex.ToString())
      End Try
   End Sub

   Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
      Get
         Return True
      End Get
   End Property

   Private Shared Function HexToByte(ByVal hexString As String) As Byte()
      Dim returnBytes As Byte() = New Byte(hexString.Length / 2 - 1) {}
      For i As Integer = 0 To returnBytes.Length - 1
         returnBytes(i) = Convert.ToByte(hexString.Substring(i * 2, 2), 16)
      Next
      Return returnBytes
   End Function

End Class
